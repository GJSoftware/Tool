using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;

namespace GJ.DEV.BARCODE
{
    /// <summary>
    /// 劳易测
    /// </summary>
    public class CCR85 : IBarCode
    {
        #region 构造函数
        public CCR85(int idNo = 0, string name = "CR50")
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
        #endregion

        #region 字段
        private int _idNo = 0;
        private string _name = "CR85";
        private EComMode _comMode = EComMode.SerialPort;
        private SerialPort _rs232;
        private string _recvData = string.Empty;
        private bool _recvThreshold = false;
        private bool _enableThreshold = false;
        private int _recieveFlag = 0;
        private string _recieveSOI = string.Empty;
        private int _recieveLen = 0;
        private string _recieveSn = string.Empty; 
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
        /// <summary>
        /// 通信接口
        /// </summary>
        public EComMode comMode
        {
            get { return _comMode; }
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
        public bool Open(string comName, out string er, string setting = "115200,n,8,1", bool recvThreshold = false)
        {
            er = string.Empty;

            try
            {
                this._recvThreshold = recvThreshold;

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
        /// 读取条码
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Read(out string serialNo, out string er, int rLen = 0, int timeOut = 1000)
        {
            try
            {

                er = string.Empty;

                serialNo = string.Empty;

                if (_rs232 == null)
                {
                    er = "串口未打开";
                    return false;
                }

                _recieveFlag = 0; 

                _enableThreshold = false;

                byte[] wByte = new byte[] { 0xEE, 0xEE, 0xEE, 0xEE, 0x24, 0x01, 0x03, 0x00, 0x1F, 0x5C };

                byte[] rByte = new byte[] { 0x01, 0x58, 0x1E, 0x61, 0x70, 0x2F, 0x64, 0x04 };

                string rSOI = System.Text.ASCIIEncoding.ASCII.GetString(rByte);

                _rs232.DiscardInBuffer();

                _rs232.DiscardOutBuffer();

                _rs232.Write(wByte, 0, wByte.Length);

                string rData = string.Empty;

                Stopwatch watcher = new Stopwatch();

                watcher.Start();

                while (true)
                {
                    System.Threading.Thread.Sleep(20);

                    if (_rs232.BytesToRead > 0)
                    {
                        System.Threading.Thread.Sleep(300);

                        rData += _rs232.ReadExisting(); 
                    }

                    int index = rData.LastIndexOf(rSOI) + rSOI.Length;

                    if (index>=0 && index <= rData.Length)
                        rData = rData.Substring(index, rData.Length - index);

                    if (rLen > 0)
                    {
                        if (rData.Length >= rLen)
                        {
                            System.Threading.Thread.Sleep(20);
                            if (_rs232.BytesToRead == 0)
                                break;
                        }
                    }
                    else
                    {
                        if (rData.Length > 0)
                        {
                            System.Threading.Thread.Sleep(20);
                            if (_rs232.BytesToRead == 0)
                                break;
                        }
                    }

                    if (watcher.ElapsedMilliseconds > timeOut)
                        break;
                }

                watcher.Stop();

                serialNo = rData;

                serialNo = serialNo.Replace(rSOI, "");

                serialNo = serialNo.Replace("\r", "");

                serialNo = serialNo.Replace("\n", "");

                if (serialNo==string.Empty)
                {
                    er = "扫描不到条码";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                serialNo = "";
                er = ex.ToString();
                return false;
            }
            finally
            {
                byte[] wByte = new byte[] { 0xEE, 0xEE, 0xEE, 0xEE, 0x50, 0x05, 0x28, 0x33, 0x35, 0x29, 0x30, 0x00, 0x65, 0x5B };

                _rs232.DiscardInBuffer();

                _rs232.DiscardOutBuffer();

                _rs232.Write(wByte, 0, wByte.Length);
            }
        }
        /// <summary>
        /// 读取条码
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Read(out string serialNo, out string er, string SOI, int rLen, int timeOut = 2000)
        {
            try
            {

                er = string.Empty;

                serialNo = string.Empty;

                if (_rs232 == null)
                {
                    er = "串口未打开";
                    return false;
                }

                _enableThreshold = false;

                _recieveFlag = 1;

                _recieveSOI = SOI;

                _recieveLen = rLen; 

                _recieveSn = string.Empty;
  
                //启动连续扫描

                byte[] rByte = new byte[] { 0x01, 0x58, 0x1E, 0x61, 0x70, 0x2F, 0x64, 0x04 };

                string rSOI = System.Text.ASCIIEncoding.ASCII.GetString(rByte);

                byte[] wByte = new byte[] { 0xEE, 0xEE, 0xEE, 0xEE, 0x50, 0x06, 0x28, 0x43, 0x34, 0x29, 0x30, 0x33, 0x00, 0x01, 0x75 };

                _rs232.DiscardInBuffer();

                _rs232.DiscardOutBuffer();

                _rs232.Write(wByte, 0, wByte.Length);

                string rData = string.Empty;

                Stopwatch watcher = new Stopwatch();

                watcher.Start();

                while (_recieveSn==string.Empty)
                {
                    System.Threading.Thread.Sleep(5);  
                    if (watcher.ElapsedMilliseconds > timeOut)
                        break;
                }

                watcher.Stop();

                if (_recieveSn == string.Empty)
                {
                    serialNo = "";
                    er = "扫描不到条码";
                    return false;
                }

                serialNo = _recieveSn;

                serialNo = serialNo.Replace(rSOI, "");

                serialNo = serialNo.Replace("\r", "");

                serialNo = serialNo.Replace("\n", "");

                return true;
            }
            catch (Exception ex)
            {
                serialNo = "";
                er = ex.ToString();
                return false;
            }
            finally
            {
                //byte[] wByte = new byte[] { 0xEE, 0xEE, 0xEE, 0xEE, 0x50, 0x06, 0x28, 0x43, 0x34, 0x29, 0x46, 0x46, 0x00, 0x1C, 0x71 };

                //_rs232.DiscardInBuffer();

                //_rs232.DiscardOutBuffer();

                //_rs232.Write(wByte, 0, wByte.Length);

                _recieveFlag = 0;
            }
        }
        /// <summary>
        /// 触发条码枪接收数据
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Triger_Start(out string er)
        {
            er = string.Empty;

            try
            {
                if (_rs232 == null)
                {
                    er = "串口未打开";
                    return false;
                }

                _enableThreshold = true;

                byte[] wByte = new byte[] { 0xEE, 0xEE, 0xEE, 0xEE, 0x50, 0x06, 0x28, 0x43, 0x34, 0x29, 0x30, 0x33, 0x00, 0x01, 0x75 };

                _rs232.DiscardInBuffer();

                _rs232.DiscardOutBuffer();

                _rs232.Write(wByte, 0, wByte.Length);

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 触发接收串口数据
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public bool Triger_End(out string er)
        {
            er = string.Empty;

            try
            {
                if (_rs232 == null)
                {
                    er = "串口未打开";
                    return false;
                }

                _enableThreshold = false;

                byte[] wByte = new byte[] { 0xEE, 0xEE, 0xEE, 0xEE, 0x50, 0x06, 0x28, 0x43, 0x34, 0x29, 0x46, 0x46, 0x00, 0x1C, 0x71 };

                _rs232.DiscardInBuffer();

                _rs232.DiscardOutBuffer();

                _rs232.Write(wByte, 0, wByte.Length);

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
        /// 格式化条码有效字符
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        private string formatSn(string serialNo)
        {
            string Sn = string.Empty;

            for (int i = 0; i < serialNo.Length; i++)
            {
                char s = System.Convert.ToChar(serialNo.Substring(i, 1));

                if (s > (char)32 && s < (char)126)
                {
                    Sn += serialNo.Substring(i, 1);
                }
            }

            return Sn;
        }
        /// <summary>
        /// 串口中断接收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRS232_Recv(object sender, SerialDataReceivedEventArgs e)
        {
            if (_recvThreshold || _enableThreshold)
            {
                System.Threading.Thread.Sleep(20);

                string recv = _rs232.ReadExisting();

                OnRecv(new CRecvArgs(_idNo, recv));
            }
            else if (_recieveFlag == 1)
            {
                if (_recieveSn != string.Empty)
                    return;

                System.Threading.Thread.Sleep(20);

                string sn = string.Empty;

                string recv = _rs232.ReadExisting();

                byte[] rByte = new byte[] { 0x01, 0x58, 0x1E, 0x61, 0x70, 0x2F, 0x64, 0x04 };

                string rSOI = System.Text.ASCIIEncoding.ASCII.GetString(rByte);

                recv = recv.Replace(rSOI, "");

                recv = recv.Replace("\r", "");

                recv = recv.Replace("\n", "");

                if (_recieveSOI != string.Empty)
                {
                    if (recv.Length < _recieveSOI.Length)
                        return;

                    int s1 = recv.LastIndexOf(_recieveSOI);

                    if (s1 < 0)
                        return;

                    sn = recv.Substring(s1, recv.Length - s1);  
                }

                if (_recieveLen > 0)
                {
                    if (recv.Length < _recieveLen)
                        return;
                }

                _recieveSn = sn; 
            }
        }
        #endregion

    }
}
