using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using GJ.COM;
namespace GJ.DEV.COM
{
    /// <summary>
    /// TCP客户端通信类
    /// 版本:V1.0.0 作者:kp.lin 日期:2017/08/10
    /// </summary>
    public class CClientTCP
    {
       #region 定义事件
        /// <summary>
        /// 状态事件委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventOnConHander(object sender, CTcpConArgs e);
       /// <summary>
       /// 状态消息
       /// </summary>
        public event EventOnConHander OnConed;
        /// <summary>
        /// 状态触发事件
        /// </summary>
        /// <param name="e"></param>
        private void OnCon(CTcpConArgs e)
        {
            if (OnConed != null)
            {
                OnConed(this, e);
            }
        }
        /// <summary>
        /// 接收事件委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventOnRecvHandler(object sender, CTcpRecvArgs e);
       /// <summary>
       /// 接收数据消息
       /// </summary>
        public event EventOnRecvHandler OnRecved;
       /// <summary>
        /// 接收触发事件
       /// </summary>
       /// <param name="e"></param>
       private void OnRecv(CTcpRecvArgs e)
       {
           if (OnRecved != null)
           {
               OnRecved(this, e);
           }
       }
       #endregion

       #region 构造函数
       /// <summary>
       /// ModBus通信时以HEX格式,自定义以字符格式
       /// </summary>
       /// <param name="idNo">设备ID</param>
       /// <param name="name">设备名称</param>
       /// <param name="datatype">数据格式</param>
       public CClientTCP(int idNo, string name, EDataType datatype = EDataType.HEX格式)
       {
           this._idNo = idNo;
           this._name = name;
           this._comDataType = datatype;
       }
       public override string ToString()
       {
           return _name;
       }
       #endregion
     
       #region 字段
       /// <summary>
       /// 接收数据最大容量
       /// </summary>
       private const double C_MAX_MB = 1;
       private int _idNo = 0;
       private string _name = "clientTCP";
       private bool _conStatus = false;
       private EDataType _comDataType = EDataType.ASCII格式;
       private string _serName = string.Empty;
       private Socket _socketClient = null;
       private Thread _threadClient = null;
       private volatile int _recvLen= 0; 
       private volatile string _recvData = string.Empty;
       private volatile byte[] _recvBytes =null; 
       #endregion

       #region 属性
        /// <summary>
        /// 设备ID
        /// </summary>
        public int idNo
        {
            get { return _idNo; }
            set { _idNo = value; }
        }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool conStatus
        {
            get { return _conStatus; }
        }
        #endregion

       #region 方法
        /// <summary>
        /// 连接服务端
        /// </summary>
        /// <param name="comName">服务端IP地址</param>
        /// <param name="setting">服务端TCP端口</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool open(string comName, out string er, string setting = "502")
        {
            IPAddress ip = IPAddress.Parse(comName);

            IPEndPoint endPoint = new IPEndPoint(ip, System.Convert.ToInt32(setting));
            
            _socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            _serName = "[" + comName + ":" + setting + "]";
            
            try
            {
                _conStatus = false;

                er = _serName + CLanguage.Lan("服务端连接中");

                OnCon(new CTcpConArgs(_idNo,name,er,false));

                _socketClient.Connect(endPoint);
            }
            catch (SocketException se)
            {
                _socketClient = null;

                er = CLanguage.Lan("无法连接服务端") + _serName + ":" + se.ToString();

                OnCon(new CTcpConArgs(_idNo, name, er,true));

                return false;
            }
            catch (Exception e)
            {
                _socketClient = null;

                er = CLanguage.Lan("无法连接服务端") + _serName + ":" + e.ToString();

                OnCon(new CTcpConArgs(_idNo, name, er, true));

                return false;
            }

            _conStatus = true;

            er =  _serName + CLanguage.Lan("服务端连接正常");

            OnCon(new CTcpConArgs(_idNo, name, er, false));

            _threadClient = new Thread(RecMsg);
            
            _threadClient.IsBackground = true;
            
            _threadClient.Start();
            
            return true;
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public bool close()
        {
            try
            {
                if (_threadClient != null)
                {
                    _threadClient.Abort();
                    _threadClient = null;
                }
                if (_socketClient != null)
                {
                    _socketClient.Dispose();
                    _socketClient = null;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                _conStatus = false;

                string status = _serName + CLanguage.Lan("客户端断开连接");

                OnCon(new CTcpConArgs(_idNo,_name,status,true)); 
            }
        }
        /// <summary>
        /// 发送数据及接收数据
        /// </summary>
        /// <param name="wData">数据格式:16进制字符和ASCII字符</param>
        /// <param name="rLen">设置为0则不返回数据</param>
        /// <param name="rData">数据格式:16进制字符和ASCII字符</param>
        /// <param name="er"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public bool send(string wData, int rLen, out string rData, out string er, int timeOut = 500)
        {
            er = string.Empty;

            rData = string.Empty;

            try
            {
                if (_socketClient == null)
                    return false;

                if (!_conStatus)
                    return false;

                byte[] arrMsg = null;

                if (_comDataType == EDataType.HEX格式)
                {
                    arrMsg = new byte[wData.Length / 2];

                    for (int i = 0; i < wData.Length / 2; i++)
                        arrMsg[i] = System.Convert.ToByte(wData.Substring(i * 2, 2), 16);
                }
                else
                {
                    arrMsg = System.Text.Encoding.UTF8.GetBytes(wData);
                }

                _recvLen = 0;

                _recvData = string.Empty;

                _recvBytes = null;                

                _socketClient.Send(arrMsg); // 发送消息；                

                if (rLen == 0)
                    return true;

                int waitTime = Environment.TickCount;
                do
                {
                    System.Threading.Thread.Sleep(5);
                } while ((_recvLen < rLen) && (Environment.TickCount - waitTime) < timeOut);

                if (_recvLen == 0)
                {
                    er = CLanguage.Lan("接收数据超时");
                    return false;
                }

                rData = string.Empty;

                if (_recvBytes == null)
                {
                    er = CLanguage.Lan("接收数据超时");
                    return false;
                }

                if (_comDataType == EDataType.HEX格式)
                {
                    for (int i = 0; i < _recvBytes.Length; i++)
                    {
                        rData += _recvBytes[i].ToString("X2");
                    }
                }
                else
                {
                    rData = _recvData; 
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
        /// 发送数据及接收数据
        /// </summary>
        /// <param name="wData">数据格式:16进制字符和ASCII字符</param>
        /// <param name="rEOI">设置为空则不返回数据</param>
        /// <param name="rData">数据格式:16进制字符和ASCII字符</param>
        /// <param name="er"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public bool send(string wData, string rEOI, out string rData, out string er, int timeOut = 500)
        {
            er = string.Empty;

            rData = string.Empty;

            try
            {
                if (_socketClient == null)
                    return false;

                if (!_conStatus)
                    return false;

                byte[] arrMsg = null;

                if (_comDataType == EDataType.HEX格式)
                {
                    arrMsg = new byte[wData.Length / 2];

                    for (int i = 0; i < wData.Length / 2; i++)
                        arrMsg[i] = System.Convert.ToByte(wData.Substring(i * 2, 2), 16);
                }
                else
                {
                    arrMsg = System.Text.Encoding.UTF8.GetBytes(wData);
                }

                _recvLen = 0;

                _recvData = string.Empty;

                _recvBytes = null;

                _socketClient.Send(arrMsg); // 发送消息；                

                if (rEOI == string.Empty)
                    return true;

                byte[] byteEOI = System.Text.Encoding.UTF8.GetBytes(rEOI);

                int waitTime = Environment.TickCount;

                do
                {
                    System.Threading.Thread.Sleep(2);

                    if (_recvLen > byteEOI.Length)
                    {
                        bool flag = true;

                        for (int i = 0; i <byteEOI.Length; i++)
                        {
                            if (byteEOI[byteEOI.Length -1 - i] != _recvBytes[_recvLen - 1 - i])
                                flag = false; 
                        }

                        if (flag)
                            break;
                    }

                } while (Environment.TickCount - waitTime < timeOut);

                if (_recvLen == 0)
                {
                    er = CLanguage.Lan("接收数据超时");
                    return false;
                }

                rData = string.Empty;

                if (_comDataType == EDataType.HEX格式)
                {
                    for (int i = 0; i < _recvLen; i++)
                    {
                        rData += _recvBytes[i].ToString("X2");
                    }
                }
                else
                {
                    rData = _recvData;
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
        /// 发送数据及接收数据
        /// </summary>
        /// <param name="wBytes">发送字节</param>
        /// <param name="rLen">设置为0则不返回数据</param>
        /// <param name="rBytes">接收字节</param>
        /// <param name="er"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public bool send(byte[] wBytes, int rLen, out byte[] rBytes, out string er, int timeOut = 500)
        {

            er = string.Empty;

            rBytes = null;

            try
            {
                if (_socketClient == null)
                    return false;

                if (!_conStatus)
                    return false;
                
                _recvData = string.Empty;

                _recvLen = 0;

                _recvBytes = null; 

                _socketClient.Send(wBytes); // 发送消息； 

                if (rLen == 0)
                    return true;
          
                int waitTime = Environment.TickCount;

                do
                {
                    System.Threading.Thread.Sleep(2);
                } while ((_recvLen < rLen) && (Environment.TickCount - waitTime) < timeOut);
                
                if (_recvLen == 0)
                {
                    er = CLanguage.Lan("接收数据超时");
                    return false;
                }
                
                rBytes = new byte[_recvLen]; 
                for (int i = 0; i < _recvLen; i++)
                {
                    rBytes[i] = _recvBytes[i];
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
        /// 接收数据
        /// </summary>
        private void RecMsg()
        {
            try
            {
                while (true)
                {
                    // 定义一个1M的缓存区；  
                    byte[] arrMsgRec = new byte[(int)(1024 * 1024 * C_MAX_MB)];

                    // 将接受到的数据存入到输入  arrMsgRec中；  
                    int length = -1;

                    try
                    {
                        length = _socketClient.Receive(arrMsgRec); // 接收数据，并返回数据的长度；  
                    }
                    catch (SocketException se)
                    {
                        _socketClient = null;

                        _conStatus = false;

                        OnCon(new CTcpConArgs(_idNo, _name, _serName + CLanguage.Lan("服务端断开连接") + ":" + se.ToString(), true));

                        return;
                    }
                    catch (Exception ex)
                    {
                        _socketClient = null;

                        _conStatus = false;

                        OnCon(new CTcpConArgs(_idNo, _name, _serName + CLanguage.Lan("服务端断开连接") + ":" + ex.ToString(), true));

                        return;
                    }
                    if (length == 0)
                    {
                        _socketClient = null;

                        _conStatus = false;

                        OnCon(new CTcpConArgs(_idNo, _name, _serName + CLanguage.Lan("服务端断开连接"), true));

                        return;
                    }

                    _recvData = System.Text.Encoding.UTF8.GetString(arrMsgRec, 0, length);// 将接受到的字节数据转化成字符串；

                    _recvBytes = new byte[length];

                    for (int i = 0; i < length; i++)
                        _recvBytes[i] = arrMsgRec[i];

                    _recvLen = length;

                    OnRecv(new CTcpRecvArgs(_idNo, _name, _socketClient.RemoteEndPoint.ToString(), _recvData, _recvBytes));
                }
            }
            catch (Exception ex)
            {
                _conStatus = false;

                OnCon(new CTcpConArgs(_idNo, _name, _serName + CLanguage.Lan("服务端断开连接") + ":" + ex.ToString(), true));
            }
            finally
            {

            }
        }
        #endregion
    }
}
