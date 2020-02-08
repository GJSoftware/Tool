using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;

namespace GJ.Iot
{
    /// <summary>
    /// 订阅消息:
    /// 1.客户端广播请求指示消息
    /// 2.客户端应答命令消息
    /// 3.客户端上报状态信息
    /// 4.客户端上报报警信息
    /// 5.客户端上报采样信息
    /// 发布消息:
    /// 1.广播客户端上报指令消息
    /// 2.发送客户端控制指令消息
    /// </summary>
    public class CService
    {
        #region 构造函数
        public CService(int idNo, string name, string pc_idNo, string pc_name, string factory)
        {
            this._idNo = idNo;
            this._name = name;
            this._pc_idNo = pc_idNo;
            this._pc_name = pc_name;
            this._factory = factory;
            InitialTopics();
        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 字段
        /// <summary>
        /// 编号
        /// </summary>
        private int _idNo = 0;
        /// <summary>
        /// 名称
        /// </summary>
        private string _name = string.Empty;
        /// <summary>
        /// 工控机编号
        /// </summary>
        private string _pc_idNo = string.Empty;
        /// <summary>
        /// 工控机名称
        /// </summary>
        private string _pc_name = string.Empty;
        /// <summary>
        /// 工厂名称
        /// </summary>
        private string _factory = string.Empty;
        /// <summary>
        /// MQTT服务
        /// </summary>
        private CMQTT _mqtt = null;
        /// <summary>
        /// 同步锁
        /// </summary>
        private object objLock = new object();
        /// <summary>
        /// 订阅主题列表
        /// </summary>
        private List<string> _subscribeTopics = new List<string>();
        /// <summary>
        /// 接收消息主题
        /// </summary>
        private Dictionary<string, EMessageType> _recieveTopics = new Dictionary<string, EMessageType>();
        /// <summary>
        /// 发布消息主题
        /// </summary>
        private Dictionary<EMessageType, string> _publishTopics = new Dictionary<EMessageType, string>();
        #endregion

        #region 定义事件
        /// <summary>
        /// 定义数据消息
        /// </summary>
        public class CCMessageArgs : EventArgs
        {
            public CCMessageArgs(int idNo, string name, string topic, string message, bool bAlarm, string AlarmInfo)
            {
                this.idNo = idNo;
                this.name = name;
                this.topic = topic;
                this.message = message;
                this.bAlarm = bAlarm;
                this.AlarmInfo = AlarmInfo;
            }
            public readonly int idNo;
            public readonly string name;
            public readonly string topic;
            public readonly string message;
            public readonly bool bAlarm;
            public readonly string AlarmInfo;
        }
        /// <summary>
        /// 状态消息
        /// </summary>
        public class CStatusArgs : EventArgs
        {
            public CStatusArgs(string topic, string message, CMessage<CData_Status> data)
            {
                this.topic = topic;
                this.message = message;
                this.data = data;
            }
            public readonly string topic;
            public readonly string message;
            public CMessage<CData_Status> data = null;
        }
        /// <summary>
        /// 命令消息
        /// </summary>
        public class CCmdArgs : EventArgs
        {
            public CCmdArgs(string topic, string message, CMessage<CData_Cmd> data)
            {
                this.topic = topic;
                this.message = message;
                this.data = data;
            }
            public readonly string topic;
            public readonly string message;
            public CMessage<CData_Cmd> data = null;
        }
        /// <summary>
        /// 定义事件
        /// </summary>
        public COnEvent<CCMessageArgs> OnMessageArgs = new COnEvent<CCMessageArgs>();
        /// <summary>
        /// 状态事件
        /// </summary>
        public COnEvent<CStatusArgs> OnStatusArgs = new COnEvent<CStatusArgs>();
        /// <summary>
        /// 广播指令事件
        /// </summary>
        public COnEvent<CCmdArgs> OnRPTCmdArgs = new COnEvent<CCmdArgs>();
        /// <summary>
        /// 应答指令事件
        /// </summary>
        public COnEvent<CCmdArgs> OnREQCmdArgs = new COnEvent<CCmdArgs>();
        #endregion

        #region 共享方法
        /// <summary>
        /// 连接服务端
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Connect(string ip, int port, out string er)
        {
            er = string.Empty;

            try
            {
                if (_mqtt != null)
                {
                    _mqtt.OnMessageArgs.OnEvent -= new COnEvent<CMQTT.CMessageArgs>.OnEventHandler(OnMessageRecv);
                    _mqtt.Close();
                    _mqtt = null;
                }

                _mqtt = new CMQTT(_idNo, _name);

                if (!_mqtt.Connect(ip, port, out er))
                {
                    _mqtt = null;
                    return false;
                }

                if (!SubscribeTopics(_subscribeTopics, out er))
                {
                    _mqtt.Close();
                    _mqtt = null;
                    return false;
                }

                _mqtt.OnMessageArgs.OnEvent += new COnEvent<CMQTT.CMessageArgs>.OnEventHandler(OnMessageRecv);

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 断开服务端
        /// </summary>
        public void Close()
        {
            try
            {
                if (_mqtt != null)
                {
                    string er = string.Empty;
                    _mqtt.OnMessageArgs.OnEvent -= new COnEvent<CMQTT.CMessageArgs>.OnEventHandler(OnMessageRecv);
                    UnSubscribeTopics(_subscribeTopics, out er);
                    _mqtt.Close();
                    _mqtt = null;
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Publish(string topic, string message, out string er)
        {
            er = string.Empty;

            try
            {
                if (!_mqtt.Publish(topic, message, out er))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 订阅主题
        /// </summary>
        /// <param name="topics"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SubscribeTopics(List<string> topics, out string er)
        {
            er = string.Empty;

            try
            {
                for (int i = 0; i < topics.Count; i++)
                {
                    if (!_mqtt.Subscribe(topics[i], out er))
                        return false;
                }

                lock (objLock)
                {
                    for (int i = 0; i < topics.Count; i++)
                    {
                        if (!_subscribeTopics.Contains(topics[i]))
                            _subscribeTopics.Add(topics[i]);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="topics"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool UnSubscribeTopics(List<string> topics, out string er)
        {
            er = string.Empty;

            try
            {
                for (int i = 0; i < topics.Count; i++)
                {
                    if (!_mqtt.UnSubscribe(topics[i], out er))
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        #endregion

        #region 专用方法
        /// <summary>
        /// 全局设备广播消息
        /// </summary>
        /// <returns></returns>
        public bool Publish_ALL(EMessageType messageType, ECmdType cmdType,string cmdName, string cmdInfo,out string er)
        {
            try
            {
                CMessage<CData_Cmd> message = new CMessage<CData_Cmd>();

                CHeader header = new CHeader()
                {
                    ID = _pc_idNo,
                    Name = _pc_name,
                    Type = (int)ENodeType.主控端,
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                message.Header = header;
                
                message.Data = new List<CData_Cmd>();

                message.Data.Add(new CData_Cmd()
                                 {
                                      ID="-1",
                                      Name ="-1",
                                      CmdType =(int)cmdType,
                                      CmdName = cmdName,
                                      CmdInfo = cmdInfo,
                                      Remark1=string.Empty, 
                                      Remark2 =string.Empty          
                                 });

                string topic = _publishTopics[messageType];

                string cmd = CJSon.Serializer<CMessage<CData_Cmd>>(message);

                if (!_mqtt.Publish(topic, cmd, out er))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 发布点对点消息
        /// </summary>
        /// <returns></returns>
        public bool Publish_Device(EMessageType messageType, ECmdType cmdType, string cmdName, string cmdInfo, List<CDevList> devList, out string er)
        {
            try
            {
                CMessage<CData_Cmd> message = new CMessage<CData_Cmd>();

                CHeader header = new CHeader()
                {
                    ID = _pc_idNo,
                    Name = _pc_name,
                    Type = (int)ENodeType.主控端,
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                message.Header = header;

                message.Data = new List<CData_Cmd>();

                for (int i = 0; i < devList.Count; i++)
                {
                    message.Data.Add(new CData_Cmd()
                    {
                        ID = devList[i].idNo,
                        Name = devList[i].Name,
                        CmdType = (int)cmdType,
                        CmdName = cmdName,
                        CmdInfo = cmdInfo,
                        Remark1 = string.Empty,
                        Remark2 = string.Empty
                    });
                }

                string topic = _publishTopics[messageType];

                string cmd = CJSon.Serializer<CMessage<CData_Cmd>>(message);

                if (!_mqtt.Publish(topic, cmd, out er))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 订阅主题
        /// </summary>
        private void InitialTopics()
        {
            lock (objLock)
            {
                string message = string.Empty;

                //订阅消息
                _subscribeTopics.Clear();
                
                _recieveTopics.Clear();

                message = "GJDG/PRD/" + _factory + "/NODE/CORE/CLIENT/COMMAND/RPT";
                _subscribeTopics.Add(message);
                _recieveTopics.Add(message, EMessageType.广播指令);

                message = "GJDG/PRD/" + _factory + "/NODE/CORE/CLIENT/COMMAND/REQ";
                _subscribeTopics.Add(message);
                _recieveTopics.Add(message, EMessageType.应答指令);

                message = "GJDG/PRD/" + _factory + "/NODE/CORE/CLIENT/STATUS/RPT";
                _subscribeTopics.Add(message);
                _recieveTopics.Add(message, EMessageType.状态消息);

                message = "GJDG/PRD/" + _factory + "/NODE/CORE/CLIENT/ALARM/RPT";
                _subscribeTopics.Add(message);
                _recieveTopics.Add(message, EMessageType.报警消息);

                message = "GJDG/PRD/" + _factory + "/NODE/CORE/CLIENT/SAMPLE/RPT";
                _subscribeTopics.Add(message);
                _recieveTopics.Add(message, EMessageType.抽样消息);

                //发布消息
                _publishTopics.Clear();
                //上报状态消息--需回复
                _publishTopics.Add(EMessageType.广播指令, "GJDG/PRD/" + _factory + "/CORE/NODE/SYSTEM/COMMAND/RPT");
                _publishTopics.Add(EMessageType.应答指令, "GJDG/PRD/" + _factory + "/CORE/NODE/SYSTEM/COMMAND/REQ");
            }

        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMessageRecv(object sender, CMQTT.CMessageArgs e)
        {
            try
            {
                if (!_recieveTopics.ContainsKey(e.topic))
                {
                    OnMessageArgs.OnEvented(new CCMessageArgs(e.idNo, e.name, e.topic, e.message, true, "主题不存在"));
                    return;
                }

                EMessageType msgType = _recieveTopics[e.topic];

                switch (msgType)
                {
                    case EMessageType.广播指令:  //请求指示
                        CMessage<CData_Cmd> cmd_RPT = CJSon.Deserialize<CMessage<CData_Cmd>>(e.message);
                        OnRPTCmdArgs.OnEvented(new CCmdArgs(e.topic, e.message, cmd_RPT));  
                        break;
                    case EMessageType.应答指令:  //应答指令
                         CMessage<CData_Cmd> cmd_REQ = CJSon.Deserialize<CMessage<CData_Cmd>>(e.message);
                         OnREQCmdArgs.OnEvented(new CCmdArgs(e.topic, e.message, cmd_REQ));  
                        break;
                    case EMessageType.状态消息:
                        CMessage<CData_Status> status_data = CJSon.Deserialize<CMessage<CData_Status>>(e.message);
                        OnStatusArgs.OnEvented(new CStatusArgs(e.topic, e.message, status_data));
                        break;
                    case EMessageType.报警消息:
                        break;
                    case EMessageType.抽样消息:
                        break;
                    default:
                        break;
                }

                OnMessageArgs.OnEvented(new CCMessageArgs(e.idNo, e.name, e.topic, e.message, false, msgType.ToString())); 
            }
            catch (Exception ex)
            {
                OnMessageArgs.OnEvented(new CCMessageArgs(e.idNo, e.name, e.topic, e.message, true, ex.ToString()));
            }
        }
        #endregion

    }
}
