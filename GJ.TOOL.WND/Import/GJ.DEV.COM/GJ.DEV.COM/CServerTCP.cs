using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading; 
using System.Net;
using System.Net.Sockets;
using GJ.COM;
namespace GJ.DEV.COM
{
    /// <summary>
    /// TCP服务端通信类
    /// 版本:V1.0.0 作者:kp.lin 日期:2017/08/10
    /// </summary>
    public class CServerTCP
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
        /// <param name="datatype">数据类型</param>
        public CServerTCP(int idNo, string name, EDataType datatype = EDataType.ASCII格式)
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

        private string _name = "serverTCP";

        private bool _conStatus = false;

        private EDataType _comDataType = EDataType.ASCII格式;

        /// <summary>
        /// 监听套接字
        /// </summary>
        private Socket _socketWatch = null;
        /// <summary>
        /// 监听线程
        /// </summary>
        private Thread _threadWatch = null;
        /// <summary>
        /// 客户端线程
        /// </summary>
        private Dictionary<string, Thread> _dictThread = new Dictionary<string, Thread>();
        /// <summary>
        /// 客户端Socket
        /// </summary>
        public Dictionary<string, Socket> _dictSocket = new Dictionary<string, Socket>();
        /// <summary>
        /// 客户端数据长度
        /// </summary>
        public Dictionary<string, int> _recvLen = new Dictionary<string, int>();
        /// <summary>
        /// 客户端接收数据
        /// </summary>
        public Dictionary<string, string> _recvData = new Dictionary<string, string>();
        /// <summary>
        /// 客户端接收字节
        /// </summary>
        public Dictionary<string, byte[]> _recvByte = new Dictionary<string, byte[]>();
        /// <summary>
        /// 同步锁
        /// </summary>
        private System.Threading.ReaderWriterLock socketLock = new ReaderWriterLock();
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

        #region 共享方法
        /// <summary>
        /// 启动监听
        /// </summary>
        /// <param name="tcpHost">主机IP</param>
        /// <param name="tcpPort">主机端口</param>
        public void Listen(int tcpPort = 8000)
        {
            try
            {
                _socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress ip = IPAddress.Any;

                IPEndPoint endPoint = new IPEndPoint(ip, tcpPort);
                
                // 将负责监听的套接字绑定到唯一的ip和端口上
                _socketWatch.Bind(endPoint);
            }
            catch (Exception e)
            {
                _conStatus = false;   
                OnCon(new CTcpConArgs(_idNo, _name, e.ToString(), true));
            }
            // 设置监听队列的长度；  
            _socketWatch.Listen(20);
            // 创建负责监听的线程；  
            _threadWatch = new Thread(WatchConnecting);
            _threadWatch.IsBackground = true;
            _threadWatch.Start();
            _dictSocket.Clear();
            _dictThread.Clear();
            _conStatus = true;
            OnCon(new CTcpConArgs(_idNo, _name, CLanguage.Lan("服务端启动监听")+ "[" +
                                                CLanguage.Lan("端口") + ":" + tcpPort.ToString() + "]"));
        }
        /// <summary>
        /// 清除已有连接
        /// </summary>
        public void Clear()
        {
            try
            {
                socketLock.AcquireWriterLock(-1);

                for (int i = 0; i < _dictSocket.Count;i++)
                {
                    string strKey = _dictSocket.ElementAt(i).Key;
                    _dictSocket[strKey].Shutdown(SocketShutdown.Both);
                    _dictSocket[strKey].Close();
                    _dictSocket[strKey].Dispose();
                }
                _dictSocket.Clear();
                _recvLen.Clear();
                _recvData.Clear();
                _recvByte.Clear();
                _dictThread.Clear();
            }
            catch (Exception)
            {

            }
            finally
            {
                socketLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 关闭监听
        /// </summary>
        /// <returns></returns>
        public bool close()
        {
            try
            {
                _conStatus =false;
 
                for (int i = 0; i < _dictSocket.Count; i++)
                {
                    string strKey = _dictSocket.ElementAt(i).Key;
                    _dictSocket[strKey].Shutdown(SocketShutdown.Both);
                    _dictSocket[strKey].Close();
                    _dictSocket[strKey].Dispose();
                }
                if (_threadWatch != null)
                {
                    _socketWatch.Close();
                    _threadWatch.Abort();
                    _threadWatch = null;
                }
                _socketWatch.Close();
                _socketWatch = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 发送数据及接收数据
        /// </summary>
        /// <param name="strKey">客户端IP地址</param>
        /// <param name="wData">发送字符串</param>
        /// <param name="rLen">接收字符串长度</param>
        /// <param name="rData">接收字符串</param>
        /// <param name="er"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public bool send(string strKey,string wData, int rLen, out string rData, out string er, int timeOut = 500)
        {
            er = string.Empty;

            rData = string.Empty;

            if (!_conStatus)
                return false;

            if (!_dictSocket.ContainsKey(strKey))
                return false;

            try
            {

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

                _recvLen[strKey] = 0;

                _recvData[strKey] = string.Empty;

                _recvByte[strKey] = null;

                _dictSocket[strKey].Send(arrMsg);// 解决了 sokConnection是局部变量，不能再本函数中引用的问题；


                if (rLen == 0)
                    return true;

                int waitTime = Environment.TickCount;
                do
                {
                    System.Threading.Thread.Sleep(2);
                } while ((_recvLen[strKey] < rLen) && (Environment.TickCount - waitTime) < timeOut);

                if (_recvLen[strKey] == 0)
                {
                    er = CLanguage.Lan("接收数据超时");
                    return false;
                }

                rData = string.Empty;

                if (_comDataType == EDataType.HEX格式)
                {
                    for (int i = 0; i < _recvLen[strKey]; i++)
                    {
                        rData += _recvByte[strKey][i].ToString("X2");
                    }
                }
                else
                {
                    rData = _recvData[strKey];
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
        /// <param name="strKey">客户端IP地址</param>
        /// <param name="wData">发送字符串</param>
        /// <param name="rEOI">接收帧尾字符串</param>
        /// <param name="rData">接收字符串</param>
        /// <param name="er"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public bool send(string strKey, string wData, string rEOI, out string rData, out string er, int timeOut = 500)
        {
            er = string.Empty;

            rData = string.Empty;

            if (!_conStatus)
                return false;

            if (!_dictSocket.ContainsKey(strKey))
                return false;

            try
            {

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

                _recvLen[strKey] = 0;

                _recvData[strKey] = string.Empty;

                _recvByte[strKey] = null;

                _dictSocket[strKey].Send(arrMsg);// 解决了 sokConnection是局部变量，不能再本函数中引用的问题；


                if (rEOI == string.Empty)
                    return true;

                byte[] byteEOI = System.Text.Encoding.UTF8.GetBytes(rEOI);

                int waitTime = Environment.TickCount;

                do
                {
                    System.Threading.Thread.Sleep(2);

                    if (_recvLen[strKey] > byteEOI.Length)
                    {
                        bool flag = true;

                        for (int i = 0; i < byteEOI.Length; i++)
                        {
                            if (byteEOI[byteEOI.Length - 1 - i] != _recvByte[strKey][_recvLen[strKey] - 1 - i])
                                flag = false;
                        }

                        if (flag)
                            break;
                    }

                } while (Environment.TickCount - waitTime < timeOut);

                if (_recvLen[strKey] == 0)
                {
                    er = CLanguage.Lan("接收数据超时");
                    return false;
                }

                rData = string.Empty;

                if (_comDataType == EDataType.HEX格式)
                {
                    for (int i = 0; i < _recvLen[strKey]; i++)
                    {
                        rData += _recvByte[strKey][i].ToString("X2");
                    }
                }
                else
                {
                    rData = _recvData[strKey];
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
        /// <param name="strKey">客户端IP地址</param>
        /// <param name="wBytes">发送字节</param>
        /// <param name="rLen">接收字节长度</param>
        /// <param name="rBytes">接收字节</param>
        /// <param name="er"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public bool send(string strKey, byte[] wBytes, int rLen, out byte[] rBytes, out string er, int timeOut = 500)
        {
            er = string.Empty;

            rBytes = null;

            if (!_conStatus)
                return false;

            if (!_dictSocket.ContainsKey(strKey))
                return false;

            try
            {               
                _recvLen[strKey] = 0;

                _recvData[strKey] = string.Empty;

                _recvByte[strKey] = null;

                _dictSocket[strKey].Send(wBytes);// 解决了 sokConnection是局部变量，不能再本函数中引用的问题；

                if (rLen == 0)
                    return true;

                int waitTime = Environment.TickCount;
                do
                {
                    System.Threading.Thread.Sleep(2);
                } while ((_recvLen[strKey] < rLen) && (Environment.TickCount - waitTime) < timeOut);

                if (_recvLen[strKey] == 0)
                {
                    er = CLanguage.Lan("接收数据超时");
                    return false;
                }

                rBytes = new byte[_recvLen[strKey]];
                for (int i = 0; i < _recvLen[strKey]; i++)
                    rBytes[i] = _recvByte[strKey][i];

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
        /// 监控线程
        /// </summary>
        private void WatchConnecting()
        {
            try
            {
                while (true)  // 持续不断的监听客户端的连接请求；  
                {
                    // 开始监听客户端连接请求，Accept方法会阻断当前的线程；  
                    Socket sokConnection = _socketWatch.Accept(); // 一旦监听到一个客户端的请求，就返回一个与该客户端通信的 套接字；  

                    OnCon(new CTcpConArgs(_idNo, _name, CLanguage.Lan("连接客户端") + "[" + sokConnection.RemoteEndPoint.ToString() + "].", false,
                                          sokConnection.RemoteEndPoint.ToString(), 1));

                    // 将与客户端连接的 套接字 对象添加到集合中；  
                    socketLock.AcquireWriterLock(-1);
                    _dictSocket.Add(sokConnection.RemoteEndPoint.ToString(), sokConnection);
                    _recvLen.Add(sokConnection.RemoteEndPoint.ToString(), 0);
                    _recvData.Add(sokConnection.RemoteEndPoint.ToString(), "");
                    _recvByte.Add(sokConnection.RemoteEndPoint.ToString(), null);
                    Thread thr = new Thread(RecMsg);
                    thr.IsBackground = true;
                    thr.Start(sokConnection);
                    _dictThread.Add(sokConnection.RemoteEndPoint.ToString(), thr);  //  将新建的线程添加到线程的集合中去。
                    socketLock.ReleaseWriterLock();
                }
            }
            catch (Exception)
            {
                //throw;
            }
            finally
            {
                _conStatus = false;  
                OnCon(new CTcpConArgs(_idNo, _name, CLanguage.Lan("服务端断开连接"), true));
            }

        }
        /// <summary>
        /// 与客户端连接套接字通信
        /// </summary>
        /// <param name="sokConnectionparn"></param>
        private void RecMsg(object sokConnectionparn)
        {
            try
            {
                Socket sokClient = sokConnectionparn as Socket;

                while (true)
                {
                    // 定义一个2M的缓存区；  
                    byte[] arrMsgRec = new byte[(int)(1024 * 1024 * C_MAX_MB)];
                    // 将接受到的数据存入到输入  arrMsgRec中；  
                    int length = -1;
                    try
                    {
                        length = sokClient.Receive(arrMsgRec); // 接收数据，并返回数据的长度；  
                    }
                    catch (SocketException)
                    {
                        if (_conStatus)
                        {
                            bool bClear = false;
                            // 从 通信套接字 集合中删除被中断连接的通信套接字； 
                            socketLock.AcquireWriterLock(-1);
                            if (_dictSocket.Count > 0 && _dictSocket.ContainsKey(sokClient.RemoteEndPoint.ToString()))
                            {
                                _dictSocket.Remove(sokClient.RemoteEndPoint.ToString());
                                _recvLen.Remove(sokClient.RemoteEndPoint.ToString());
                                _recvData.Remove(sokClient.RemoteEndPoint.ToString());
                                _recvByte.Remove(sokClient.RemoteEndPoint.ToString());
                                // 从通信线程集合中删除被中断连接的通信线程对象；  
                               _dictThread.Remove(sokClient.RemoteEndPoint.ToString());
                               bClear = true;
                            }
                            socketLock.ReleaseWriterLock();
                            if (bClear)
                            {
                                OnCon(new CTcpConArgs(_idNo, _name, CLanguage.Lan("客户端断开连接") + "[" + sokClient.RemoteEndPoint.ToString() +
                                                                  "]", true, sokClient.RemoteEndPoint.ToString(), 2));
                            }
                        }
                        break;
                    }
                    catch (Exception)
                    {
                        if (_conStatus)
                        {
                            bool bClear = false;
                            // 从 通信套接字 集合中删除被中断连接的通信套接字； 
                            socketLock.AcquireWriterLock(-1);
                            if (_dictSocket.Count > 0 && _dictSocket.ContainsKey(sokClient.RemoteEndPoint.ToString()))
                            {
                                _dictSocket.Remove(sokClient.RemoteEndPoint.ToString());
                                _recvLen.Remove(sokClient.RemoteEndPoint.ToString());
                                _recvData.Remove(sokClient.RemoteEndPoint.ToString());
                                _recvByte.Remove(sokClient.RemoteEndPoint.ToString());
                                // 从通信线程集合中删除被中断连接的通信线程对象；  
                                _dictThread.Remove(sokClient.RemoteEndPoint.ToString());
                                bClear = true;
                            }
                            socketLock.ReleaseWriterLock();
                            if (bClear)
                            {
                                OnCon(new CTcpConArgs(_idNo, _name, CLanguage.Lan("客户端断开连接") + "[" + sokClient.RemoteEndPoint.ToString() +
                                                                  "]", true, sokClient.RemoteEndPoint.ToString(), 2));
                            }
                        }
                        break;
                    }
                    try
                    {
                        // 表示接收到的是数据；  
                        if (length == 0)
                        {
                            if (_conStatus)
                            {
                                bool bClear = false;
                                // 从 通信套接字 集合中删除被中断连接的通信套接字； 
                                socketLock.AcquireWriterLock(-1);
                                if (_dictSocket.Count > 0 && _dictSocket.ContainsKey(sokClient.RemoteEndPoint.ToString()))
                                {
                                    _dictSocket.Remove(sokClient.RemoteEndPoint.ToString());
                                    _recvLen.Remove(sokClient.RemoteEndPoint.ToString());
                                    _recvData.Remove(sokClient.RemoteEndPoint.ToString());
                                    _recvByte.Remove(sokClient.RemoteEndPoint.ToString());
                                    // 从通信线程集合中删除被中断连接的通信线程对象；  
                                    _dictThread.Remove(sokClient.RemoteEndPoint.ToString());
                                    bClear = true;
                                }
                                socketLock.ReleaseWriterLock();
                                if (bClear)
                                {
                                    OnCon(new CTcpConArgs(_idNo, _name, CLanguage.Lan("客户端断开连接") + "[" + sokClient.RemoteEndPoint.ToString() +
                                                                      "]", true, sokClient.RemoteEndPoint.ToString(), 2));
                                }
                            }
                            break;
                        }
                        if (_conStatus)
                        {
                            _recvLen[sokClient.RemoteEndPoint.ToString()] = length;
                            _recvData[sokClient.RemoteEndPoint.ToString()] = System.Text.Encoding.UTF8.GetString(arrMsgRec, 0, length);// 将接受到的字节数据转化成字符串
                            _recvByte[sokClient.RemoteEndPoint.ToString()] = new byte[length];
                            for (int i = 0; i < length; i++)
                                _recvByte[sokClient.RemoteEndPoint.ToString()][i] = arrMsgRec[i];
                            OnRecv(new CTcpRecvArgs(_idNo, _name, sokClient.RemoteEndPoint.ToString(),
                                                    _recvData[sokClient.RemoteEndPoint.ToString()],
                                                    _recvByte[sokClient.RemoteEndPoint.ToString()]));                            
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            finally
            {

            }
        }
        #endregion

    }
}
