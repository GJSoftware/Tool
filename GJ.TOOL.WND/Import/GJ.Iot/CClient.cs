using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;

namespace GJ.Iot
{
    /// <summary>
    /// 订阅消息:
    /// 1.主控端广播指令消息--->上报信息
    /// 2.主控端回复指令消息--->接收控制应答
    /// 发布消息
    /// 1.广播主控端请求指示消息
    /// 1.应答主控端指令消息
    /// 2.上报状态信息
    /// 3.上报报警信息
    /// 4.上报采样信息
    /// </summary>
    public class CClient
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="idNo">工控机编号</param>
        /// <param name="name">工控机名称</param>
        /// <param name="factory">工厂名称</param>
        /// <param name="devices">设备节点列表ID+Name</param>
        public CClient(int idNo, string name, string pc_idNo, string pc_name, string factory, List<CDevList> devices)
        {
            this._idNo = idNo;
            this._name = name;
            this._pc_idNo = pc_idNo;
            this._pc_name = pc_name;
            this._factory = factory;
            this._devices = devices;
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
        /// 设备信息ID+Name
        /// </summary>
        private List<CDevList> _devices = null;
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

        #region 属性
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected
        {
            get {
                if (_mqtt == null)
                    return false;
                return _mqtt.IsConnected;
                }
        }
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
        /// 广播指令事件
        /// </summary>
        public COnEvent<CCmdArgs> OnCmdRPTArgs = new COnEvent<CCmdArgs>();
        /// <summary>
        /// 应答指令事件
        /// </summary>
        public COnEvent<CCmdArgs> OnCmdREQArgs = new COnEvent<CCmdArgs>();
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
                     string er=string.Empty;
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
        public bool Publish(string topic, string message,out string er)
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
        public bool SubscribeTopics(List<string> topics,out string er)
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

                lock (objLock)
                {
                    for (int i = 0; i < topics.Count; i++)
                    {
                        if (_subscribeTopics.Contains(topics[i]))
                            _subscribeTopics.Remove(topics[i]);
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
        #endregion

        #region 专用方法
        /// <summary>
        /// 广播请求指示
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Request_Command(out string er)
        {
            er = string.Empty;

            try
            {
                CMessage<CData_Cmd> message = new CMessage<CData_Cmd>();

                message.Header = new CHeader()
                                 {
                                    ID = _pc_idNo,
                                    Name = _pc_name,
                                    Type = (int)ENodeType.客户端,
                                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                 };

                message.Data = new List<CData_Cmd>();

                for (int i = 0; i < _devices.Count; i++)
                {
                    message.Data.Add(new CData_Cmd()
                                    {
                                        ID = _devices[i].idNo,
                                        Name = _devices[i].Name,
                                        CmdType = (int)ECmdType.请求指示,
                                        CmdName = string.Empty,
                                        CmdInfo  = string.Empty,
                                        Remark1 =string.Empty,
                                        Remark2 = string.Empty  
                                    });
                }

                string topic = _publishTopics[EMessageType.广播指令];

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
        /// 应答指令请求
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Reponse_Command(string status,out string er)
        {
            er = string.Empty;

            try
            {
                CMessage<CData_Cmd> message = new CMessage<CData_Cmd>();

                message.Header = new CHeader()
                {
                    ID = _pc_idNo,
                    Name = _pc_name,
                    Type = (int)ENodeType.客户端,
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                message.Data = new List<CData_Cmd>();

                for (int i = 0; i < _devices.Count; i++)
                {
                    message.Data.Add(new CData_Cmd()
                    {
                        ID = _devices[i].idNo,
                        Name = _devices[i].Name,
                        CmdType = (int)ECmdType.应答指令,
                        CmdName = (ECmdType.应答指令).ToString(),
                        CmdInfo = status,
                        Remark1 = string.Empty,
                        Remark2 = string.Empty
                    });
                }

                string topic = _publishTopics[EMessageType.应答指令];

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
        /// 上报状态
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Report_Status(List<CData_Status> msg, out string er)
        {
            er = string.Empty;

            try
            {
                CMessage<CData_Status> message = new CMessage<CData_Status>();

                message.Header = new CHeader()
                {
                    ID = _pc_idNo,
                    Name = _pc_name,
                    Type = (int)ENodeType.客户端,
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                message.Data = msg;

                string topic = _publishTopics[EMessageType.状态消息];

                string cmd = CJSon.Serializer<CMessage<CData_Status>>(message);

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

                message = "GJDG/PRD/" + _factory + "/CORE/NODE/SYSTEM/COMMAND/RPT";
                _subscribeTopics.Add(message);
                _recieveTopics.Add(message, EMessageType.广播指令);

                message = "GJDG/PRD/" + _factory + "/CORE/NODE/SYSTEM/COMMAND/REQ";
                _subscribeTopics.Add(message);
                _recieveTopics.Add(message, EMessageType.应答指令);

                //发布消息-不需回复
                _publishTopics.Clear();
                _publishTopics.Add(EMessageType.广播指令, "GJDG/PRD/" + _factory + "/NODE/CORE/CLIENT/COMMAND/RPT");
                _publishTopics.Add(EMessageType.应答指令, "GJDG/PRD/" + _factory + "/NODE/CORE/CLIENT/COMMAND/REQ");
                _publishTopics.Add(EMessageType.状态消息, "GJDG/PRD/" + _factory + "/NODE/CORE/CLIENT/STATUS/RPT");
                _publishTopics.Add(EMessageType.报警消息, "GJDG/PRD/" + _factory + "/NODE/CORE/CLIENT/ALARM/RPT");
                _publishTopics.Add(EMessageType.抽样消息, "GJDG/PRD/" + _factory + "/NODE/CORE/CLIENT/SAMPLE/RPT");
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
                    OnMessageArgs.OnEvented(new CCMessageArgs(e.idNo,e.name,e.topic,e.message,true,"主题不存在")); 
                    return;
                }

                CMessage<CData_Cmd> message = CJSon.Deserialize<CMessage<CData_Cmd>>(e.message);

                EMessageType msgType = _recieveTopics[e.topic];

                //解析设备编号

                List<CData_Cmd> data = new List<CData_Cmd>();

                for (int i = 0; i < message.Data.Count; i++)
                {
                    if (message.Data[i].ID == "-1")  //广播设备编号
                    {
                        for (int z = 0; z < _devices.Count; z++)
                        {
                            data.Add(new CData_Cmd()
                                     {
                                        ID = _devices[i].idNo,
                                        Name = _devices[i].Name,
                                        CmdName = message.Data[i].CmdName,
                                        CmdType = message.Data[i].CmdType, 
                                        CmdInfo = message.Data[i].CmdInfo,  
                                        Remark1 =message.Data[i].Remark1,
                                        Remark2 = message.Data[i].Remark2 
                                     });
                        }
                        break;
                    }
                    else if (IsDeviceId(message.Data[i].ID))  //指定设备编号
                    {
                        data.Add(new CData_Cmd()
                        {
                            ID = _devices[i].idNo,
                            Name = _devices[i].Name,
                            CmdName = message.Data[i].CmdName,
                            CmdType = message.Data[i].CmdType,
                            CmdInfo = message.Data[i].CmdInfo,
                            Remark1 = message.Data[i].Remark1,
                            Remark2 = message.Data[i].Remark2
                        });
                    }
                }

                if (data.Count == 0)
                {
                    OnMessageArgs.OnEvented(new CCMessageArgs(e.idNo, e.name, e.topic, e.message, false, "NULL")); 
                    return;
                }

                CMessage<CData_Cmd> data_cmd = new CMessage<CData_Cmd>();

                data_cmd.Header = message.Header;

                data_cmd.Data = data;

                string msg = CJSon.Serializer<List<CData_Cmd>>(data);

                if (msgType == EMessageType.广播指令)  //接收主控端指令指示
                {
                    OnCmdRPTArgs.OnEvented(new CCmdArgs(e.topic, msg, data_cmd)); 
                }
                else if (msgType == EMessageType.应答指令)  //接收主端控制指令
                {
                    OnCmdREQArgs.OnEvented(new CCmdArgs(e.topic, msg, data_cmd)); 
                }

                OnMessageArgs.OnEvented(new CCMessageArgs(e.idNo, e.name, e.topic, e.message, false, msgType.ToString())); 
            }
            catch (Exception ex)
            {
                OnMessageArgs.OnEvented(new CCMessageArgs(e.idNo, e.name, e.topic, e.message, true, ex.ToString())); 
            }
        }
        private bool IsDeviceId(string devId)
        {
            for (int i = 0; i < _devices.Count; i++)
            {
                if (devId.ToUpper() == _devices[i].idNo)
                    return true;
            }

            return false;
        }
        #endregion

    }
}
