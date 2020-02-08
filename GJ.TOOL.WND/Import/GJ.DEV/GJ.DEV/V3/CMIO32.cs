using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;

namespace GJ.DEV.V3
{
    public class CMIO32
    {
        #region 构造函数
        public CMIO32(int idNo = 0, string name = "GJMon32_V3")
        {
            _idNo = idNo;

            _name = name;
        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 字段
        private int _idNo = 0;
        private string _name = "GJMon32_V3";
        private bool _conStatus = false;
        private CSerialPort com = null;
        private int _TimeOut = 100;
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
        /// <param name="comName"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Open(string comName, out string er, string setting = "57600,n,8,1")
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
        /// 设置新地址
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SetNewAddr(int wAddr, out string er)
        {
            er = string.Empty;

            try
            {
                string wCmd = string.Empty;
                string rData = string.Empty;
                string rVal = string.Empty;
                wCmd = "00" + "00" + "00" + "05" + wAddr.ToString("X2");
                wCmd = SOI + wCmd + CalCheckSum(wCmd) + EOI;
                int rLen = 7;
                if (!com.send(wCmd, rLen, out rData, out er, _TimeOut))
                    return false;
                if (!ToCheckSum(rData, ref rVal))
                {
                    er = "检验和错误:" + rData;
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
        /// 读版本
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="version"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadVersion(int wAddr, out string version, out string er)
        {
            version = string.Empty;

            er = string.Empty;

            try
            {
                string wCmd = string.Empty;
                string rData = string.Empty;
                string rVal = string.Empty;
                wCmd = wAddr.ToString("X2") + "02" + "05" + "04";
                wCmd = SOI + wCmd + CalCheckSum(wCmd) + EOI;
                int rLen = 8;
                if (!com.send(wCmd, rLen, out rData, out er, _TimeOut))
                    return false;
                if (!ToCheckSum(rData, ref rVal))
                {
                    er = "检验和错误:" + rData;
                    return false;
                }
                double ver = ((double)System.Convert.ToInt16(rVal, 16)) / 10;
                version = ver.ToString("0.0");
                return true;
            }
            catch (Exception e)
            {
                er = e.ToString();
                return false;
            }
        }
        /// <summary>
        /// 读取32通道电压及X0-X8
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="rVolt"></param>
        /// <param name="X">X1-X9</param>
        /// <param name="rOnOff"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadVolt(int wAddr,out List<double> rVolt, out List<int> X, out string er)
        {
            rVolt = new List<double>();

            for (int i = 0; i < 32; i++)
                rVolt.Add(0);

            X = new List<int>();

            for (int i = 0; i < 9; i++)
                X.Add(0); 

            er = string.Empty;

            try
            {
                string wCmd = string.Empty;

                string rData = string.Empty;

                string rVal = string.Empty;

                wCmd = wAddr.ToString("X2") + "02" + "01" + "04";

                wCmd = SOI + wCmd + CalCheckSum(wCmd) + EOI;

                //SOI + Addr + CMD1,CMD2 + CNT + DATA(32电压通道) + ON/OFF + Signal + ErrCode + CheckSum + EOI

                int rLen = 1 + 1 + 2 + 1 + 32 * 3 + 1 + 2 + 1 + 1 + 1;
                
                if (!com.send(wCmd, rLen, out rData, out er, _TimeOut))
                    return false;
                if (!ToCheckSum(rData, ref rVal))
                {
                    er = "检验和错误:" + rData;
                    return false;
                }

                string pol = rVal.Substring(32 * 4 - 1, 64);

                string status = rVal.Substring(rVal.Length - 8, 8);

                for (int i = 0; i < 32; i++)
                {
                    int valTemp = System.Convert.ToInt32(rVal.Substring(i * 4, 4), 16);

                    rVolt[i] = ((double)valTemp) / 1000;

                    if (pol.Substring(i * 2, 2) == "2D")
                        rVolt[i] *= -1;
                }

                //Byte0
                int rOnOff = System.Convert.ToInt16(status.Substring(0, 2), 16);

                //Byte2:Bit0-S1 Bit1-S2 Bit2-Sgin Bit3-ACON Bit4-ACOFF Bit5-AirUp Bit6-AirDw Bit7-X9
                //Byte1:Bit0-X1 Bit1-X2 Bit3-X3 Bit3-X4 Bit4-X5 Bit5-X6 Bit6-X7 Bit7-X8

                //Byte1
                int sgnVal1 = System.Convert.ToInt16(status.Substring(2, 2), 16);

                for (int i = 0; i < 7; i++)
                {
                    X[i] = ((sgnVal1 & (1 << i)) == (1 << i)) ? 1 : 0;
                }

                //Byte1
                int sgnVal2 = System.Convert.ToInt16(status.Substring(4, 2), 16);

                X[8] = ((sgnVal2 & (1 << 7)) == (1 << 7)) ? 1 : 0;

                return true;
            }
            catch (Exception e)
            {
                er = e.ToString();
                return false;
            }
        }
        /// <summary>
        /// 读取Y点输出
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="Y"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadY(int wAddr, out List<int> Y, out string er)
        {
            Y = new List<int>();

            er = string.Empty;

            try
            {
                string wCmd = string.Empty;

                string rData = string.Empty;

                string rVal = string.Empty;

                wCmd = wAddr.ToString("X2") + "02" + "11" + "04";

                wCmd = SOI + wCmd + CalCheckSum(wCmd) + EOI;

                int rLen = 8;

                if (!com.send(wCmd, rLen, out rData, out er, _TimeOut))
                    return false;

                if (!ToCheckSum(rData, ref rVal))
                {
                    er = "检验和错误:" + rData;
                    return false;
                }

                int YBin = System.Convert.ToInt16(rVal, 16);

                for (int i = 0; i < 7; i++)
                {
                    int bit  = ((YBin & (1 << i)) == (1 << i)) ? 1 : 0;

                    Y.Add(bit); 
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
        /// 控制Y0-Y7点ON/OFF
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="wYno">0-7</param>
        /// <param name="wOnOff"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ControlY_OnOff(int wAddr, int wYno, int wOnOff, out string er)
        {
            er = string.Empty;

            try
            {
                string wCmd = string.Empty;

                string rData = string.Empty;

                string rVal = string.Empty;

                if (wAddr == 0)
                    wCmd = wAddr.ToString("X2") + "00" + "07" + "06" + (wYno+1).ToString("X2") + wOnOff.ToString("X2");
                else
                    wCmd = wAddr.ToString("X2") + "01" + "14" + "06" + (wYno+1).ToString("X2") + wOnOff.ToString("X2");

                wCmd = SOI + wCmd + CalCheckSum(wCmd) + EOI;

                int rLen = 0;

                for (int i = 0; i < 2; i++)
                {
                    System.Threading.Thread.Sleep(20);

                    if (!com.send(wCmd, rLen, out rData, out er, _TimeOut))
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
        #endregion

        #region 控制板协议
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
            {
                sum += System.Convert.ToInt16(wCmd.Substring(i * 2, 2), 16);
            }
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
