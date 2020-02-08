using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;
using System.Diagnostics;

namespace GJ.DEV.BARCODE
{
    /// <summary>
    /// 西克条码枪
    /// </summary>
    public class CLECTOR620 : IBarCode
    {
        #region 构造函数
        public CLECTOR620(int idNo = 0, string name = "LECTOR620")
        {
            this._idNo = idNo;

            this._name = name;

            com = new CClientTCP(idNo, name, EDataType.ASCII格式);
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
        private string _name = "CLECTOR620";
        private EComMode _comMode = EComMode.TCP;
        private CClientTCP com = null;
        private string _recvData = string.Empty;
        private bool _recvThreshold = false;
        private bool _enableThreshold = false;
        private int _recieveFlag = 0;
        private string _recieveSOI = string.Empty;
        private string _recieveData = string.Empty;
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
        /// <param name="comName">192.168.60.101</param>
        /// <param name="setting">10000</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Open(string comName, out string er, string setting = "10000", bool recvThreshold = false)
        {
            er = string.Empty;

            try
            {
                this._recvThreshold = recvThreshold;

                if (!com.open(comName, out er, setting))
                    return false;

                com.OnRecved += new CClientTCP.EventOnRecvHandler(OnTCP_Recv);

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
            if (com != null)
            {
                com.OnRecved -= new CClientTCP.EventOnRecvHandler(OnTCP_Recv);
                com.close();
            }
        }
        /// <summary>
        /// 读取条码
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Read(out string serialNo, out string er, int rLen = 0, int timeOut = 500)
        {
            try
            {

                er = string.Empty;

                serialNo = string.Empty;

                if (com == null)
                {
                    er = "串口未打开";
                    return false;
                }

                _recieveFlag = 0;

                _enableThreshold = false;

                _recvThreshold = false;

                _recieveData = string.Empty;

                string rData = string.Empty;

                string wCmd = "+" + "\r\n";

                if (rLen == 0)
                    rLen = 1;

                if (!com.send(wCmd, rLen, out rData, out er, timeOut))
                    return false;

                if (rData == string.Empty)
                    return false;

                if (rData.Length > 0)
                {
                    if (rData.Substring(0, 1) == "?")
                        return false;
                }

                serialNo = rData;

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
                string strError = string.Empty;

                string Cmd = "-" + "\r\n";

                string rData = string.Empty;

                com.send(Cmd, 0, out rData, out strError);

                _recieveFlag = 0;
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

                if (com == null)
                {
                    er = "串口未打开";
                    return false;
                }

                _recieveFlag = 1;

                _enableThreshold = false;

                _recvThreshold = false;

                _recieveData = string.Empty;

                string rData = string.Empty;

                string wCmd = "+" + "\r\n";

                if (!com.send(wCmd, 0, out rData, out er))
                    return false;

                Stopwatch watcher = new Stopwatch();

                watcher.Start();

                while (true)
                {
                    System.Threading.Thread.Sleep(2);

                    if (rLen > 0 && _recieveData.Length >= rLen)
                        break;

                    if (_recieveData.Length > 0)
                    {
                        if (_recieveData.Substring(0, 1) == "?")
                            break;
                    }

                    if (watcher.ElapsedMilliseconds > timeOut)
                        break;
                }

                if (_recieveData == string.Empty || _recieveData.Substring(0, 1) == "?")
                    return false;

                serialNo = _recieveData;

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
                string strError = string.Empty;

                string Cmd = "-" + "\r\n";

                string rData = string.Empty;

                com.send(Cmd, 0, out rData, out strError);

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
                if (com == null)
                {
                    er = "串口未打开";
                    return false;
                }

                string rData = string.Empty;

                string wCmd = "+" + "\r\n";

                if (!com.send(wCmd, 0, out rData, out er))
                    return false;

                _enableThreshold = true;

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
                if (com == null)
                {
                    er = "串口未打开";
                    return false;
                }

                string rData = string.Empty;

                string wCmd = "-" + "\r\n";

                if (!com.send(wCmd, 0, out rData, out er))
                    return false;

                _enableThreshold = false;

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
        private bool formatSn(string SerialNo)
        {
            try
            {
                string Sn = string.Empty;

                for (int i = 0; i < SerialNo.Length; i++)
                {
                    char s = System.Convert.ToChar(SerialNo.Substring(i, 1));

                    if (s >= 48 && s <= 57)  //0-9
                    {
                        Sn += SerialNo.Substring(i, 1);
                    }
                    else if (s >= 65 && s <= 90) //A-Z
                    {
                        Sn += SerialNo.Substring(i, 1);
                    }
                    else if (s >= 97 && s <= 122) //a-z
                    {
                        Sn += SerialNo.Substring(i, 1);
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 串口中断接收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTCP_Recv(object sender, CTcpRecvArgs e)
        {
            if (_recvThreshold || _enableThreshold)
            {
                string recv = e.recvData;

                OnRecv(new CRecvArgs(_idNo, recv));
            }
            else if (_recieveFlag == 1)
            {
                _recieveData += e.recvData;

                _recieveData += "\r";
            }
        }
        #endregion
    }
}
