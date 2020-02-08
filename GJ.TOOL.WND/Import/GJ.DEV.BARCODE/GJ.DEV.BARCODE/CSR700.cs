using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using GJ.DEV.BARCODE;
using GJ.COM;

namespace GJ.Device.BarReader
{
    class CSR700 : IBarCode
    {
        #region 构造函数
        public CSR700(int idNo = 0, string name = "SR700")
        {
            this._idNo = idNo;
            this._name = name;
        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 字段
        private int _idNo = 0;
        private string _name = "SR700";
        private SerialPort _rs232;
        private EComMode _comMode = EComMode.SerialPort;
        private string _recvData = string.Empty;
        private bool _recvThreshold = false;
        private bool _enableThreshold = false;

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

        #region 共享方法
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="comName"></param>
        /// <param name="er"></param>
        /// <param name="setting"></param>
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
                    er = CLanguage.Lan("串口设置参数错误");
                    return false;
                }
                int bandRate = Convert.ToInt32(arrayPara[0]);
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
                int dataBit = Convert.ToInt32(arrayPara[2]);
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
        /// <param name="serailNo"></param>
        /// <param name="er"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public bool Read(out string serailNo, out string er, int rLen = 0, int timeOut = 1000)
        {
            serailNo = string.Empty;

            er = string.Empty;

            try
            {
                if (_rs232 == null)
                {

                    er = CLanguage.Lan("串口未打开");
                    return false;
                }

                Stopwatch watcher = new Stopwatch();

                watcher.Start();

                if (!Send("\x02LON\x03", "\x02LOFF\x03", 0x02, 0x03, 0x0d, out serailNo, out er, timeOut))
                {
                    return false;
                }

                if (rLen != 0 && serailNo.Length != rLen)
                {
                    return false;
                }

                watcher.Stop();

                er = "耗时:" + watcher.ElapsedMilliseconds.ToString() + "ms";

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
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
                    er = CLanguage.Lan("串口未打开");
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
                    er = CLanguage.Lan("串口未打开");
                    return false;
                }

                _enableThreshold = true;

                byte[] wByte = System.Text.ASCIIEncoding.ASCII.GetBytes("TEST1\r");

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
                    er = CLanguage.Lan("串口未打开");
                    return false;
                }

                _enableThreshold = false;

                byte[] wByte = System.Text.ASCIIEncoding.ASCII.GetBytes("QUIT\r");

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

        #region 发送字符串
        private bool Send(string stratData, string endData, Byte STX, Byte ETX, Byte CR, out string rData, out string er, int timeOut = 500)
        {
            rData = string.Empty;

            er = string.Empty;

            try
            {
                byte[] wByte = null;
                int wByteLen = 0;
                wByteLen = System.Text.ASCIIEncoding.ASCII.GetByteCount(stratData);
                wByte = new byte[wByteLen];
                wByte = System.Text.ASCIIEncoding.ASCII.GetBytes(stratData);
                _rs232.DiscardInBuffer();
                _rs232.DiscardOutBuffer();
                _rs232.Write(wByte, 0, wByteLen);
                Stopwatch watcher = new Stopwatch();
                watcher.Start();
                do
                {
                    System.Threading.Thread.Sleep(5);

                    if (_rs232.BytesToRead > 0)
                    {
                        bool readOK = false;
                        int rByteLen = _rs232.BytesToRead;
                        byte[]  rByte = new byte[rByteLen];
                        _rs232.Read(rByte, 0, rByteLen);
                        rData += Encoding.GetEncoding("Shift_JIS").GetString(rByte);
                        for (int i = 0; i < rByte.Length; i++)
                        {
                            if (rByte[i] == CR)
                                readOK = true;
                        }
                        if (readOK)
                            break;
                    }

                } while (watcher.ElapsedMilliseconds < timeOut);

                if (rData == string.Empty) 
                {
                    er = CLanguage.Lan("接收数据超时");
                    return false;
                }

                if (rData.Contains("\r"))
                {
                    rData = rData.Substring(0, rData.IndexOf("\r"));
                }

                if (rData.StartsWith("\0\0"))
                {
                    rData = "";
                    er = CLanguage.Lan("返回值为空");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                byte[] wByte = null;
                int wByteLen = 0;
                wByteLen = System.Text.ASCIIEncoding.ASCII.GetByteCount(endData);
                wByte = new byte[wByteLen];
                wByte = System.Text.ASCIIEncoding.ASCII.GetBytes(endData);
                _rs232.DiscardInBuffer();
                _rs232.DiscardOutBuffer();
                _rs232.Write(wByte, 0, wByteLen);            
            }
        }
        private bool CheckDataSize(Byte[] recvBytes, int recvSize, bool binaryDataMode = false)
        {
            const int dataSizeLen = 4;

            if (!binaryDataMode)
            {
                return true;
            }

            if (recvSize < dataSizeLen)
            {
                return false;
            }

            int dataSize = 0;
            int mul = 1;
            for (int i = 0; i < dataSizeLen; i++)
            {
                dataSize += (recvBytes[dataSizeLen - 1 - i] - '0') * mul;
                mul *= 10;
            }

            return (dataSize + 1 == recvSize);
        }
        #endregion

        #endregion

        #region 私有方法
        private void OnRS232_Recv(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (_recvThreshold || _enableThreshold)
                {
                    System.Threading.Thread.Sleep(5);

                    string recv = _rs232.ReadExisting();

                    string[] recvs = recv.Split(':');

                    string serialNo = recvs[0];

                    if (serialNo.LastIndexOf("ERROR") >= 0 || serialNo.LastIndexOf("OK") >= 0)
                        return;

                    OnRecv(new CRecvArgs(_idNo, serialNo));
                }
            }
            catch (Exception)
            {

            }            
        }
        #endregion
    }
}
