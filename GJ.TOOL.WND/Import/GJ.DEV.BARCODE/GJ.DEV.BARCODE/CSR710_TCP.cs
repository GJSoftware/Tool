using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using GJ.DEV.COM;
namespace GJ.DEV.BARCODE
{
    public class CSR710_TCP : IBarCode
    {
        #region 构造函数
        public CSR710_TCP(int idNo = 0, string name = "SR710")
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
        private string _name = "SR710";
        private EComMode _comMode = EComMode.Telnet;
        private CTelnet _com;
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
        /// <param name="comName">192.168.1.125</param>
        /// <param name="setting">10001</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Open(string comName, out string er, string setting = "10001", bool recvThreshold = false)
        {
            er = string.Empty;

            try
            {
                if (_com != null)
                {
                    _com.close();
                    _com = null;
                }

                _com = new CTelnet(idNo, name, EDataType.ASCII格式);

                if (!_com.open(comName, out er, setting))
                {
                    _com = null;
                    return false;
                }

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
            if (_com != null)
            {
                _com.close();
                _com = null;
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

            er = string.Empty;

            serialNo = string.Empty;

            if (_com == null)
            {
                er = "设备未打开";
                return false;
            }

            string wData = "\x02LON\x03";

            string rEOI = "\r";

            string rData = string.Empty;

            try
            {
                if (!_com.send(wData, rEOI, out rData, out er))
                    return false;
                
                if (rData == string.Empty)
                {
                    er = "接收数据超时";
                    return false;
                }

                if (rData.Contains("\r"))
                {
                    rData = rData.Substring(0, rData.IndexOf("\r"));
                }

                if (rData.StartsWith("\0\0"))
                {
                    rData = "";
                    er = "返回值为空";
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
                wData = "\x02LOFF\x03";

                _com.send(wData, 0, out rData, out er);
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
            er = string.Empty;

            serialNo = string.Empty;

            try
            {

                return false;
            }
            catch (Exception ex)
            {
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

                return false;
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
        #endregion
    }
}
