using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;

namespace GJ.DEV.ERS
{
    public class CGJ272_4 : IERS
    {
        #region 构造函数
        public CGJ272_4(int idNo = 0, string name = "GJ272_4")
        {
            _idNo = idNo;

            _name = name;
        }
        #endregion

        #region 字段
        private int _idNo = 0;
        private string _name = "GJ272_4";
        private bool _conStatus = false;
        private CSerialPort com;        
        private const int _maxCH = 4;
        #endregion

        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        public int idNo
        {
            get{return _idNo;}
            set{_idNo = value;}
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string name
        {
            get{return _name;}
            set{_name = value;}
        }
        /// <summary>
        /// 状态
        /// </summary>
        public bool conStatus
        {
            get { return _conStatus; }
        }
        /// <summary>
        /// 负载通道
        /// </summary>
        public int maxCH
        {
            get { return _maxCH; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="comName"></param>
        /// <param name="er"></param>
        /// <param name="setting"></param>
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
        /// 设地址
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SetNewAddr(int wAddr, out string er)
        {
            er = string.Empty;

            try
            {
                string cmd0 = "0F";
                string wData = wAddr.ToString("X2");
                wData = CalDataFromERS272Cmd(0, cmd0, wData);
                string rData = string.Empty;
                int rLen = 8;
                if (!SendCmdToERS272(wData, rLen, out rData, out er))
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
                string cmd0 = "11";
                string wData = wAddr.ToString("X2");
                wData = CalDataFromERS272Cmd(wAddr, cmd0, wData);
                string rData = string.Empty;
                int rLen = 9;
                if (!SendCmdToERS272(wData, rLen, out rData, out er))
                    return false;
                string temp = string.Empty;
                string rVal = string.Empty;
                if (!checkErrorCode(rData, out rVal, out temp))
                    return false;
                version = System.Convert.ToInt16(rVal, 16).ToString();
                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 设置负载全部通道电流
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="loadPara"></param>
        /// <param name="er"></param>
        /// <param name="saveEPROM"></param>
        /// <returns></returns>
        public bool SetNewLoad(int wAddr, CERS_Load loadPara, out string er, bool saveEPROM = true)
        {
            er = string.Empty;

            try
            {
                bool setOK = true;
                string cmd0 = string.Empty;
                string wData = string.Empty;
                string rData = string.Empty;
                for (int i = 0; i < _maxCH; i++)
                {
                    if (saveEPROM)
                        cmd0 = (i + 0x3).ToString("X2");
                    else
                        cmd0 = (i + 0x16).ToString("X2");
                    wData = ((int)(loadPara.cur[i] * 1024)).ToString("X4");
                    wData = CalDataFromERS272Cmd(wAddr, cmd0, wData);
                    int rLen = 8;
                    if (!SendCmdToERS272(wData, rLen, out rData, out er))
                    {
                        setOK = false;
                        continue;
                    }
                    string temp = string.Empty;
                    string rVal = string.Empty;
                    if (!checkErrorCode(rData, out rVal, out temp))
                    {
                        er += temp;
                        setOK = false;
                        continue;
                    }
                }
                return setOK;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 设置负载单通道电流
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="CH">1-4</param>
        /// <param name="loadVal"></param>
        /// <param name="er"></param>
        /// <param name="saveEPROM"></param>
        /// <returns></returns>
        public bool SetNewLoad(int wAddr, int CH, double loadVal, out string er, bool saveEPROM = true)
        {
            er = string.Empty;

            try
            {
                string cmd0 = string.Empty;
                string wData = string.Empty;
                string rData = string.Empty;

                if (saveEPROM)
                    cmd0 = (CH + 0x2).ToString("X2");
                else
                    cmd0 = (CH + 0x15).ToString("X2");
                wData = ((int)(loadVal * 1024)).ToString("X4");
                wData = CalDataFromERS272Cmd(wAddr, cmd0, wData);
                int rLen = 7;
                if (!SendCmdToERS272(wData, rLen, out rData, out er))
                    return false;
                string temp = string.Empty;
                string rVal = string.Empty;
                if (!checkErrorCode(rData, out rVal, out temp))
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
        /// 回读负载设定
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="loadVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadLoadSet(int wAddr, out CERS_Load loadVal, out string er)
        {
            er = string.Empty;

            loadVal = new CERS_Load();

            try
            {
                string cmd0 = "84";
                string wData =string.Empty;
                wData = CalDataFromERS272Cmd(wAddr, cmd0, wData);
                string rData = string.Empty;
                int rLen = 15;
                if (!SendCmdToERS272(wData, rLen, out rData, out er))
                    return false;
                string rVal = string.Empty;
                if (!checkErrorCode(rData, out rVal, out er))
                    return false;
                for (int i = 0; i < 4; i++)
                {
                    int valTemp = System.Convert.ToInt32(rVal.Substring(i * 4, 4), 16);
                    loadVal.cur[i] = ((double)valTemp) / 1024;
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
        /// 读取负载电流
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="dataVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadData(int wAddr, out CERS_Load dataVal, out string er)
        {
            er = string.Empty;

            dataVal = new CERS_Load();

            try
            {
                string cmd0 = "12";
                string wData = wAddr.ToString("X2");
                wData = CalDataFromERS272Cmd(wAddr, cmd0, wData);
                string rData = string.Empty;
                int rLen = 48;
                if (!SendCmdToERS272(wData, rLen, out rData, out er))
                    return false;

                string rVal = string.Empty;
                if (!checkErrorCode(rData, out rVal, out er))
                    return false;

                int valTemp = 0;

                valTemp = System.Convert.ToInt16(rVal.Substring(0, 4), 16);
                dataVal.volt[0] = ((double)valTemp) * 320 / 4096;

                valTemp = System.Convert.ToInt16(rVal.Substring(4, 4), 16);
                dataVal.volt[1] = ((double)valTemp) * 320 / 4096;

                valTemp = System.Convert.ToInt16(rVal.Substring(8, 4), 16);
                dataVal.cur[0] = ((double)valTemp) / 1024;

                valTemp = System.Convert.ToInt16(rVal.Substring(12, 4), 16);
                dataVal.cur[1] = ((double)valTemp) / 1024;

                valTemp = System.Convert.ToInt16(rVal.Substring(56, 4), 16);
                dataVal.volt[2] = ((double)valTemp) * 320 / 4096;

                valTemp = System.Convert.ToInt16(rVal.Substring(60, 4), 16);
                dataVal.volt[3] = ((double)valTemp) * 320 / 4096;

                valTemp = System.Convert.ToInt16(rVal.Substring(64, 4), 16);
                dataVal.cur[2] = ((double)valTemp) / 1024;

                valTemp = System.Convert.ToInt16(rVal.Substring(68, 4), 16);
                dataVal.cur[3] = ((double)valTemp) / 1024;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 设置快充MTK电压上升或下降
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="CH">0,1,2,3</param>
        /// <param name="wRaise"></param>
        /// <returns></returns>
        public bool SetQCMTK(int wAddr, int CH, bool wRaise, out string er)
        {
            er = string.Empty;

            try
            {
                string cmd0 = string.Empty;
                string wData = string.Empty;
                string rData = string.Empty;
                cmd0 = "0C";
                wData = CH.ToString("X2");
                if (wRaise)
                    wData += "01";
                else
                    wData += "02";
                wData = CalDataFromERS272Cmd(wAddr, cmd0, wData);
                int rLen = 7;
                if (!SendCmdToERS272(wData, rLen, out rData, out er))
                    return false;
                string temp = string.Empty;
                string rVal = string.Empty;
                if (!checkErrorCode(rData, out rVal, out temp))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        #endregion

        #region 协议
        /* 
       * 发送:桢头(7E)+地址+命令01+命令02+长度+数据+检验和+桢尾(7F)
       * 应答:桢头(7E)+地址+长度+数据+检验和+桢尾(7F)         
      */
        private string SOI = "7E";
        private string EOI = "7F";
        private string ROI = "7D";
        /// <summary>
        /// 发串口数据并接收数据
        /// </summary>
        /// <param name="wData"></param>
        /// <param name="rLen"></param>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <param name="wTimeOut"></param>
        /// <returns></returns>
        private bool SendCmdToERS272(string wData, int rLen, out string rData, out string er, int wTimeOut = 500)
        {
            rData = string.Empty;

            er = string.Empty;

            try
            {
                string recvData = string.Empty;
                if (!com.send(wData, rLen, out recvData, out er, wTimeOut))
                    return false;
                rData = recvData;
                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }           
        }
        /// <summary>
        /// 格式化命令
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="wCmd"></param>
        /// <param name="wData"></param>
        /// <returns></returns>
        private string CalDataFromERS272Cmd(int wAddr, string wCmd, string wData)
        {
            string cmd = string.Empty;
            int len = 3 + wData.Length / 2;
            string chkData = string.Empty;
            for (int i = 0; i < wData.Length / 2; i++)
            {
                if (wData.Substring(i * 2, 2) == SOI || wData.Substring(i * 2, 2) == EOI)
                    chkData += ROI;
                else
                    chkData += wData.Substring(i * 2, 2);
            }
            cmd = wAddr.ToString("X2") + wCmd + len.ToString("X2") + chkData;
            cmd = SOI + cmd + CalCheckSum(cmd) + EOI;
            return cmd;
        }
        /// <summary>
        /// 校验和计算
        /// </summary>
        /// <param name="wData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool ERS_CheckSum(string wData, ref string er)
        {
            if (wData.Substring(0, 2) != SOI)
            {
                er = "数据桢头错误:" + wData;
                return false;
            }
            if (wData.Substring(wData.Length - 2, 2) != EOI)
            {
                er = "数据桢尾错误:" + wData;
                return false;
            }
            if (wData.Length / 2 < 6)
            {
                er = "数据长度小于6:" + wData;
                return false;
            }
            int rLen = System.Convert.ToInt16(wData.Substring(6, 2), 16);
            if ((wData.Length / 2) != (rLen + 4))
            {
                er = "数据长度错误:" + wData;
                return false;
            }
            string chkStr = wData.Substring(2, wData.Length - 8);

            string chkSum = wData.Substring(wData.Length - 6, 4);

            if (chkSum != CalCheckSum(chkStr))
            {
                er = "数据CheckSum错误:" + wData;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 检验和-(地址+命令01+命令02+长度+数据)%256
        /// </summary>
        /// <param name="wData"></param>
        /// <returns></returns>
        private string CalCheckSum(string wData)
        {
            int sum = 0;

            for (int i = 0; i < wData.Length / 2; i++)
                sum += System.Convert.ToInt16(wData.Substring(i * 2, 2), 16);
            sum = sum % 0x10000;

            string checkSum = sum.ToString("X4");

            string Byte_H = checkSum.Substring(0, 2);

            string Byte_L = checkSum.Substring(2, 2);

            Byte_H = Byte_H.Replace(SOI, ROI);

            Byte_H = Byte_H.Replace(EOI, ROI);

            Byte_L = Byte_L.Replace(SOI, ROI);

            Byte_L = Byte_L.Replace(EOI, ROI);

            checkSum = Byte_H + Byte_L;

            return checkSum;
        }
        /// <summary>
        /// 错误信息检查
        /// </summary>
        /// <param name="wData"></param>
        /// <param name="rVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool checkErrorCode(string wData, out string rVal, out string er)
        {
            rVal = string.Empty;

            er = string.Empty;
            
            try
            {
                if (!ERS_CheckSum(wData, ref er))
                    return false;

                string chkFlag = wData.Substring(4, 2);

                switch (chkFlag)
                {
                    case "F0":
                        rVal = wData.Substring(8, wData.Length - 14);
                        return true;
                    case "F1":
                        er = "CHKSUM错误";
                        break;
                    case "F2":
                        er = "LENGTH错误";
                        break;
                    case "F3":
                        er = "CID无效";
                        break;
                    case "F4":
                        er = "无效数据";
                        break;
                    case "F5":
                        er = "模块地址太大";
                        break;
                    case "F6":
                        er = "DSP变量个数设置错误";
                        break;
                    default:
                        er = "异常错误";
                        break;
                }
                return false;
            }
            catch (Exception ex)
            {
                er = ex.ToString();

                return false;
            }
        }
        #endregion

    }
}
