using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using GJ.COM;
namespace GJ.Iot
{
    /// <summary>
    /// MQTT协议
    /// </summary>
    public class CMQTT
    {
        #region 枚举与类
        /// <summary>
        /// 协议类型
        /// </summary>
        public enum MqttSslProtocols
        {
            None = 0,
            SSLv3 = 1,
            TLSv1_0 = 2,
            TLSv1_1 = 3,
            TLSv1_2 = 4
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        public enum EQOS
        {
            最多分发一次 = 0,
            至少分发一次 = 1,
            只分发一次 = 2,
        }
        #endregion

        #region 构造函数
        public CMQTT(int idNo = 0, string name = "GJIot", EQOS qos = EQOS.只分发一次)
        {
            this._idNo = idNo;
            this._name = name;
            this._qos = qos;
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
        /// 消息类型
        /// </summary>
        private EQOS _qos = EQOS.只分发一次;
        /// <summary>
        /// 客户端
        /// </summary>
        private object _MQTT = null;
        #endregion

        #region 属性
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected
        {
            get {
                  if (_MQTT == null)
                      return false;

                   Type type = _MQTT.GetType();

                   PropertyInfo property = type.GetProperty("IsConnected");

                   return (bool)property.GetValue(_MQTT, null);
                }
        }
        #endregion

        #region 事件定义
        /// <summary>
        /// 定义事件
        /// </summary>
        public COnEvent<CMessageArgs> OnMessageArgs = new COnEvent<CMessageArgs>();
        /// <summary>
        /// 定义数据消息
        /// </summary>
        public class CMessageArgs : EventArgs
        {
            public CMessageArgs(int idNo, string name,string topic, string message)
            {
                this.idNo = idNo;
                this.name = name;
                this.topic = topic;
                this.message = message;
            }
            public readonly int idNo;
            public readonly string name;
            public readonly string topic;
            public readonly string message;
        }
        #endregion

        #region 共享方法
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="er"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Connect(string ip, int port, out string er, string user = "admin", string password = "password")
        {
            er = string.Empty;

            try
            {
                string dllFile = Application.StartupPath + "\\M2Mqtt.Net.dll";

                if (!File.Exists(dllFile))
                {
                    er = "动态库【M2Mqtt.Net.dll】不存在,请检查文件是否存在?";
                    return false;
                }

                Assembly abs = Assembly.LoadFile(dllFile);

                foreach (Type item in abs.GetTypes())
                {
                    if (item.FullName == "uPLibrary.Networking.M2Mqtt.MqttClient")
                    {
                        //object[] obj = new object[6] { ip, port, false, MqttSslProtocols.TLSv1_0, null, null };
                        //_MQTT = abs.CreateInstance(item.FullName, true, System.Reflection.BindingFlags.Default, null, obj, null, null);// 创建类的实例 

                        //获取类型的结构信息
                        ConstructorInfo[] myconstructors = item.GetConstructors();
                        //构造Object数组，作为构造函数的输入参数 
                        object[] obj = new object[6] { ip, port, false, MqttSslProtocols.TLSv1_0, null, null };
                        //调用构造函数生成对象 
                        _MQTT = myconstructors[5].Invoke(obj);
                        break;
                    }
                }

                if (_MQTT == null)
                {
                    er = "未加载动态库【M2Mqtt.Net.dll】";
                    return false;
                }

                //生成客户端ID并连接服务器  
                string clientId = Guid.NewGuid().ToString();

                Type type = _MQTT.GetType();

                MethodInfo ConnectMethod = type.GetMethod("Connect", new Type[] { typeof(string), typeof(string), typeof(string) });

                byte res = (byte)ConnectMethod.Invoke(_MQTT, new object[] { clientId, user, password });

                if (res != 0)
                {
                    er = "连接服务端【"+ ip + ":" + port.ToString() +"】错误:" + res.ToString();
                    return false;
                }

                //添加事件

                BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

                EventInfo eventInfo = type.GetEvent("MqttMsgPublishReceived", bindingFlags);

                var methodInfo = this.GetType().GetMethod("MqttMsgPublishReceived", bindingFlags);

                var @delegate = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, methodInfo);

                eventInfo.AddEventHandler(_MQTT, @delegate);

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        public void Close()
        {
            try
            {
                if (_MQTT != null)
                {                
                    //清除事件

                    Type type = _MQTT.GetType();

                    BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

                    EventInfo eventInfo = type.GetEvent("MqttMsgPublishReceived", bindingFlags);

                    FieldInfo fi = eventInfo.DeclaringType.GetField("MqttMsgPublishReceived", bindingFlags);

                    if (fi != null)
                    {
                        // 将event对应的字段设置成null即可清除所有挂钩在该event上的delegate
                        fi.SetValue(_MQTT, null);
                    }

                    //关闭连接

                    MethodInfo CloseMethod = type.GetMethod("Disconnect");

                    CloseMethod.Invoke(_MQTT,null);

                    _MQTT = null;
                }
            }
            catch (Exception)
            {

            }        
        }
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Subscribe(string topic, out string er)
        {
            er = string.Empty;

            try
            {
                if (_MQTT == null)
                {
                    er = "未连接服务器";
                    return false;
                }

                Type type = _MQTT.GetType();

                PropertyInfo property = type.GetProperty("IsConnected");

                bool IsConnected = (bool)property.GetValue(_MQTT, null);

                if (!IsConnected)
                {
                    er = "服务端已断开";
                    return false;
                }

                string[] topics = new string[] { topic };

                //消息质量为 2   
                byte[] qos = new byte[] { (byte)_qos };

                MethodInfo SubscribeMethod = type.GetMethod("Subscribe", new Type[] { typeof(string[]), typeof(byte[]) });

                ushort res = (ushort)SubscribeMethod.Invoke(_MQTT, new object[] { topics, qos });

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
        /// <param name="topic"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool UnSubscribe(string topic, out string er)
        {
            er = string.Empty;

            try
            {
                if (_MQTT == null)
                {
                    er = "未连接服务器";
                    return false;
                }

                Type type = _MQTT.GetType();

                PropertyInfo property = type.GetProperty("IsConnected");

                bool IsConnected = (bool)property.GetValue(_MQTT, null);

                if (!IsConnected)
                {
                    er = "服务端已断开";
                    return false;
                }

                string[] topics = new string[] { topic };

                MethodInfo UnsubscribeMethod = type.GetMethod("Unsubscribe", new Type[] { typeof(string[]) });

                ushort res = (ushort)UnsubscribeMethod.Invoke(_MQTT, new object[] { topics });

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Publish(string topic,string message, out string er)
        {
            er = string.Empty;

            try
            {
                if (_MQTT == null)
                {
                    er = "未连接服务器";
                    return false;
                }

                Type type = _MQTT.GetType();

                PropertyInfo property = type.GetProperty("IsConnected");

                bool IsConnected = (bool)property.GetValue(_MQTT, null);

                if (!IsConnected)
                {
                    er = "服务端已断开";
                    return false;
                }

                //消息质量为 2   
                byte qos = (byte)_qos;

                byte[] messByte = Encoding.UTF8.GetBytes(message);

                MethodInfo PublishMethod = type.GetMethod("Publish", new Type[] { typeof(string), typeof(byte[]), typeof(byte), typeof(bool) });

                ushort res = (ushort)PublishMethod.Invoke(_MQTT, new object[] { topic, messByte, qos, false });

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
        /// 接收消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttMsgPublishReceived(object sender, EventArgs e)
        {
            PropertyInfo Topic = e.GetType().GetProperty("Topic");

            string topic = Topic.GetValue(e, null).ToString();

            PropertyInfo Message = e.GetType().GetProperty("Message");

            byte[] message = (byte[])Message.GetValue(e, null);

            ////处理接收到的消息  
            string msg = System.Text.Encoding.UTF8.GetString(message);

            OnMessageArgs.OnEvented(new CMessageArgs(_idNo, _name, topic, msg));
        }
        #endregion

    }
}
