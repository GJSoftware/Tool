using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;

namespace GJ.DEV.ATEXY
{
    public class CGJX6Y6
    {
        #region 构造函数
        public CGJX6Y6(int idNo = 0, string name = "ATEX6Y6")
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
        private string _name = "16转8信号切换板";
        private bool _conStatus = false;
        private CSerialPort com = null;
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
        /// 状态
        /// </summary>
        public bool conStatus
        {
            get { return _conStatus; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="comName">9600,n,8,1</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Open(string comName, out string er, string setting = "9600,n,8,1")
        {
            er = string.Empty;

            try
            {
                if (com != null)
                {
                    com.close();
                    com = null;
                }

                com = new CSerialPort(idNo, name, EDataType.HEX格式);

                if (!com.open(comName, out er, setting))
                    return false;

                _conStatus = true;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            if (com == null)
                return;
            com.close();
            com = null;
            _conStatus = false;
        }
        /// <summary>
        /// 控制Y1-Y6
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="relayNo">iRly=0:所有Y点OFF</param>
        /// <param name="OnOff"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool CtrlYRelay(int wAddr, int relayNo, int OnOff, out string er)
        {
            er = string.Empty;

            try
            {
                string wCmd = string.Empty;

                string rData = string.Empty;

                string rVal = string.Empty;

                wCmd = wAddr.ToString("X2") + "01" + "01" + "06" + relayNo.ToString("X2") + OnOff.ToString("X2");

                wCmd = SOI + wCmd + CalCheckSum(wCmd) + EOI;

                int rLen = 0;

                if (!com.send(wCmd, rLen, out rData, out er))
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
        /// 读X,Y状态(1-6)
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadXY(int wAddr, out List<int> X, out List<int> Y, out string er)
        {
            X = new List<int>();

            Y = new List<int>();

            er = string.Empty;

            try
            {
                string wCmd = string.Empty;

                string rData = string.Empty;

                string rVal = string.Empty;

                wCmd = wAddr.ToString("X2") + "02" + "01" + "04";

                wCmd = SOI + wCmd + CalCheckSum(wCmd) + EOI;

                int rLen = 23;

                if (!com.send(wCmd, rLen, out rData, out er))
                    return false;

                if (!ToCheckSum(rData, ref rVal))
                {
                    er = "检验和错误:" + rData;
                    return false;
                }
                for (int i = 0; i < 6; i++)
                {
                    X.Add(System.Convert.ToInt32(rVal.Substring(i * 2, 2), 16));
                    Y.Add(System.Convert.ToInt32(rVal.Substring((i + 6) * 2, 2), 16));
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

        #region 通信协议
        private const string SOI = "EE";
        private const string EOI = "EF";
        private const string ROI = "ED";
        /// <summary>
        /// 计算检验和
        /// </summary>
        /// <param name="wCmd"></param>
        /// <returns></returns>
        private string CalCheckSum(string wCmd)
        {
            int sum = 0;
            for (int i = 0; i < wCmd.Length / 2; i++)
                sum += System.Convert.ToInt16(wCmd.Substring(i * 2, 2), 16);
            sum = sum % 0x100;
            string chkSum = sum.ToString("X2");
            if (chkSum == SOI || chkSum == EOI)
                chkSum = ROI;
            return chkSum;
        }
        /// <summary>
        /// 检查检验和
        /// </summary>
        /// <param name="wCmd"></param>
        /// <param name="rVal">有效数据</param>
        /// <returns></returns>
        private bool ToCheckSum(string wCmd, ref string rVal)
        {
            int s1 = wCmd.IndexOf(SOI);
            int s2 = wCmd.LastIndexOf(EOI);
            if (s2 == 0 || s1 >= s2 || s2 - s1 < 12)
                return false;
            wCmd = wCmd.Substring(s1, s2 - s1 + 2);
            int sum = 0;
            for (int i = 1; i <= (wCmd.Length - 6) / 2; i++)
            {
                sum += System.Convert.ToInt16(wCmd.Substring(i * 2, 2), 16);
            }
            sum = sum % 256;
            string calSum = sum.ToString("X2");
            if (calSum == SOI || calSum == EOI)
                calSum = ROI;
            string getSum = wCmd.Substring(wCmd.Length - 4, 2);
            if (calSum != getSum)
                return false;
            rVal = wCmd.Substring(10, wCmd.Length - 14);
            return true;
        }
        /// <summary>
        /// 检查数据是否为桢头和桢尾
        /// </summary>
        /// <param name="wCmd"></param>
        /// <returns></returns>
        private string chkSOIEOI(string wCmd)
        {
            string rCmd = string.Empty;
            int len = wCmd.Length / 2;
            for (int i = 0; i < len; i++)
            {
                string temp = wCmd.Substring(i * 2, 2);
                if (temp == SOI || temp == EOI)
                    rCmd += ROI;
                else
                    rCmd += temp;
            }
            return rCmd;
        }
        #endregion

    }
}
