using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using GJ.COM;
using System.Threading.Tasks;

namespace GJ.DEV.Phoenix
{
    #region 事件消息
    /// <summary>
    /// 串口接收数据消息
    /// </summary>
    public class CRecvArgs : EventArgs
    {
        public CRecvArgs(int idNo,string name, string recvData)
        {
            this.idNo = idNo;
            this.name = name;
            this.recvData = recvData;
        }
        public readonly int idNo = 0;
        public readonly string name = string.Empty;
        public readonly string recvData = string.Empty;
    }
    /// <summary>
    /// 串口数据接收完毕
    /// </summary>
    public class CRecvCompleteArgs : EventArgs
    {
        public int idNo = 0;
        public string name = string.Empty;
        public readonly string rData;
        public readonly bool bComplete;
        public CRecvCompleteArgs(int idNo, string name, string rData, bool bComplete)
        {
            this.idNo = idNo;
            this.name = name;
            this.rData = rData;
            this.bComplete = bComplete;
        }
    }
    public delegate void OnRecvHandler(object sender, CRecvArgs e);
    #endregion

    public class CProgrammer
    {
        #region 构造函数
        public CProgrammer(int idNo = 0, string name = "Phoenix Debugger PCB")
        {
            this._idNo = idNo;
            this._name = name;
        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 事件
        /// <summary>
        /// 接收数据事件
        /// </summary>
        public event OnRecvHandler OnRecved;
        void OnRecv(CRecvArgs e)
        {
            if (OnRecved != null)
            {
                OnRecved(this, e);
            }
        }
        /// <summary>
        /// 数据事件
        /// </summary>
        public COnEvent<CRecvCompleteArgs> OnCompleteArgs = new COnEvent<CRecvCompleteArgs>();
        #endregion

        #region 字段
        private int _idNo = 0;
        private string _name = "Phoenix Debugger PCB";
        private SerialPort _rs232;
        private ReaderWriterLock _comLock = new ReaderWriterLock();
        private bool _recvThreshold = true;
        private volatile string _recvData = string.Empty;
        #endregion

        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        public int idNo
        {
            get { return _idNo; }
            set { _idNo = value; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion

        #region 共享方法
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="comName"></param>
        /// <param name="setting">115200,n,8,1</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Open(string comName, out string er, string setting = "1250000,n,8,1")
        {
            er = string.Empty;

            try
            {
                string[] arrayPara = setting.Split(',');

                if (arrayPara.Length != 4)
                {
                    er = "串口设置参数错误";
                    return false;
                }
                int bandRate = System.Convert.ToInt32(arrayPara[0]);
                Parity parity = Parity.None;
                switch (arrayPara[1].ToUpper())
                {
                    case "O":
                        parity = Parity.Odd;
                        break;
                    case "E":
                        parity = Parity.Even;
                        break;
                    case "M":
                        parity = Parity.Mark;
                        break;
                    case "S":
                        parity = Parity.Space;
                        break;
                    default:
                        break;
                }
                int dataBit = System.Convert.ToInt32(arrayPara[2]);
                StopBits stopBits = StopBits.One;
                switch (arrayPara[3])
                {
                    case "0":
                        stopBits = StopBits.None;
                        break;
                    case "1.5":
                        stopBits = StopBits.OnePointFive;
                        break;
                    case "2":
                        stopBits = StopBits.Two;
                        break;
                    default:
                        break;
                }
                if (_rs232 != null)
                {
                    if (_rs232.IsOpen)
                        _rs232.Close();
                    _rs232 = null;
                }

                _rs232 = new SerialPort(comName, bandRate, parity, dataBit, stopBits);

                _rs232.DataReceived += new SerialDataReceivedEventHandler(OnRS232_Recv);

                _rs232.Open();

                _recvThreshold = true;

                return true;
            }
            catch (Exception e)
            {
                er = e.ToString();
                return false;
            }
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public void Close()
        {
            if (_rs232 != null)
            {
                if (_rs232.IsOpen)
                    _rs232.Close();
                _rs232 = null;
            }
        }
        /// <summary>
        /// 发送串口数据
        /// </summary>
        /// <param name="wCmd"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SendCmdToCOM(string wCmd, out string er,int waitTime = 0, int delayTime=200)
        {
            er = string.Empty;

            try
            {
                if (_rs232 == null)
                {
                    er = CLanguage.Lan("串口未打开");
                    return false;
                }

                _recvThreshold = false;

                Task.Factory.StartNew(()=>{


                    try
                    {
                        int enumerateTime = 5000;

                        int EOIWaitTime = 200;

                        bool bFirst = false;

                        bool bEOI = false;

                        //发送数据
                        if (wCmd != string.Empty)
                        {
                            _recvData = string.Empty;

                            wCmd += "\r\n";

                            byte[] wByte = null;

                            int wByteLen = 0;

                            wByteLen = System.Text.Encoding.UTF8.GetByteCount(wCmd);

                            wByte = new byte[wByteLen];

                            wByte = System.Text.Encoding.UTF8.GetBytes(wCmd);

                            _rs232.DiscardInBuffer();

                            _rs232.DiscardOutBuffer();

                            _rs232.Write(wByte, 0, wByteLen);
                        }
                        else           //等待清除串口缓存数据
                        {
                            waitTime = 3000;

                            enumerateTime = 3000;

                            EOIWaitTime = 1000;
                        }

                        //等待接收数据

                        Stopwatch watcher = new Stopwatch();

                        watcher.Start();

                        while (true)
                        {
                            Thread.Sleep(5);

                            if (_rs232.BytesToRead > 0)
                            {
                                string recv = _rs232.ReadExisting();

                                _recvData += recv;

                                OnRecv(new CRecvArgs(_idNo, _name, recv));

                                watcher.Restart();

                                bEOI = false;

                                continue;
                            }

                            if (bEOI) //结束数据
                            {
                                if (watcher.ElapsedMilliseconds > EOIWaitTime)
                                    break;

                                continue;
                            }

                            if (waitTime == 0) //不等待检测数据
                            {
                                if (watcher.ElapsedMilliseconds > delayTime)
                                    break;

                                continue;
                            }

                            if (_recvData == string.Empty)
                            {
                                if (watcher.ElapsedMilliseconds > delayTime)
                                {
                                    watcher.Stop();
                                    break;
                                }
                                continue;
                            }

                            if (!bFirst)  //初始等待
                            {
                                if (watcher.ElapsedMilliseconds < waitTime)
                                {
                                    string[] rValList = _recvData.Split('\n');

                                    if (_recvData.Contains("Done Enumerating"))
                                        bEOI = true;

                                    if (_recvData.Contains("DFP (B192) Enumeration Pending"))
                                        bEOI = true;

                                    if (rValList.Length < 5)
                                        continue;

                                    bFirst = true;

                                    watcher.Restart();

                                    continue;
                                }

                                watcher.Restart();

                                bFirst = true;

                                continue;
                            }

                            if (watcher.ElapsedMilliseconds < enumerateTime)
                            {
                                if (_recvData.Contains("Done Enumerating"))
                                    bEOI = true;

                                if (_recvData.Contains("DFP (B192) Enumeration Pending"))
                                    bEOI = true;

                                continue;
                            }

                            break;
                        }

                        OnCompleteArgs.OnEvented(new CRecvCompleteArgs(_idNo, _name, _recvData, true));

                        _recvData = string.Empty;

                        _recvThreshold = true;
                    }
                    catch (Exception)
                    {
                        OnCompleteArgs.OnEvented(new CRecvCompleteArgs(_idNo, _name, _recvData, true));

                        _recvData = string.Empty;

                        _recvThreshold = true;
                    }                    

                });

                return true;

            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 发送串口数据
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SendCmd(string wCmd,out string er,int waitTime=0, int delayTime=200)
        {
            er = string.Empty;

            try
            {
                _comLock.AcquireWriterLock(-1);

                if (_rs232 == null)
                {
                    er = CLanguage.Lan("串口未打开");
                    return false;
                }

                _recvThreshold = true;

                if (wCmd == string.Empty) //等待清除串口缓存数据
                {
                    waitTime = -1;
                }

                if (wCmd != string.Empty)
                {
                    _recvData = string.Empty;

                    wCmd += "\r\n";

                    byte[] wByte = null;

                    int wByteLen = 0;

                    wByteLen = System.Text.Encoding.UTF8.GetByteCount(wCmd);

                    wByte = new byte[wByteLen];

                    wByte = System.Text.Encoding.UTF8.GetBytes(wCmd);

                    _rs232.DiscardInBuffer();

                    _rs232.DiscardOutBuffer();

                    _rs232.Write(wByte, 0, wByteLen);

                }

                WaitForRecieve(waitTime, delayTime);

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                _comLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 等待串口数据接收
        /// </summary>
        /// <returns></returns>
        public void WaitForRecieve(int waitTime = 0, int delayTime=1000)
        { 
            Task.Factory.StartNew(()=>{

                                        int enumerateTime = 5000;

                                        int EOIWaitTime = 200;

                                        int rLen = 0;

                                        bool bFirst = false;

                                        bool bEOI = false;

                                        if (waitTime == -1)
                                        {
                                            waitTime = 3000;

                                            enumerateTime = 3000;

                                            EOIWaitTime = 1000;
                                        }

                                        System.Threading.Thread.Sleep(50);

                                        Stopwatch watcher = new Stopwatch();

                                        watcher.Start();

                                        while (true)
                                        {
                                            System.Threading.Thread.Sleep(5);

                                            if (_recvData.Length > rLen)
                                            {
                                                rLen = _recvData.Length;

                                                watcher.Restart();

                                                bEOI = false;

                                                continue;
                                            }

                                            if (bEOI) //结束数据
                                            {
                                                if (watcher.ElapsedMilliseconds > EOIWaitTime)                                                
                                                    break;
                                                continue;
                                            }

                                            if (waitTime == 0) //不等待检测数据
                                            {
                                                if (watcher.ElapsedMilliseconds > delayTime)
                                                    break;
                                                continue;
                                            }

                                            if (_recvData == string.Empty)
                                            {
                                                if (watcher.ElapsedMilliseconds > delayTime)
                                                {
                                                    watcher.Stop();
                                                    break;
                                                }
                                                continue;
                                            }

                                            if (!bFirst)  //初始等待
                                            {
                                                if (watcher.ElapsedMilliseconds < waitTime)
                                                {
                                                    string[] rValList=_recvData.Split('\n');

                                                    if (_recvData.Contains("Done Enumerating"))
                                                        bEOI = true;
     
                                                    if (_recvData.Contains("DFP (B192) Enumeration Pending"))
                                                        bEOI = true;

                                                    if (rValList.Length < 5)
                                                        continue;

                                                    bFirst = true;

                                                    watcher.Restart();
                                                      
                                                    continue;                                                    
                                                }

                                                watcher.Restart();

                                                bFirst = true;

                                                continue;
                                            }

                                            if (watcher.ElapsedMilliseconds < enumerateTime)
                                            {
                                                if (_recvData.Contains("Done Enumerating"))
                                                    bEOI = true;                                                

                                                if (_recvData.Contains("DFP (B192) Enumeration Pending"))
                                                    bEOI = true;

                                                continue;
                                            }

                                            break;
                                        }

                                        OnCompleteArgs.OnEvented(new CRecvCompleteArgs(_idNo,_name,_recvData,true));

                                        _recvData = string.Empty;

                                       }); 
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 串口中断接收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRS232_Recv(object sender, SerialDataReceivedEventArgs e)
        {
            if (_recvThreshold)
            {
                string recv = _rs232.ReadExisting();

                _recvData += recv;

                OnRecv(new CRecvArgs(_idNo,_name, recv));
            }
        }
        #endregion
    }
}
