using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;
using System.Net;
using GJ.COM;

namespace GJ.DEV.PLC
{
    public class CFX3U_TCP:IPLC
    {
        #region 构造函数
        public CFX3U_TCP(int idNo = 0, string name = "Mitsubishi_FX3U")
        {
            this._idNo = idNo;
            this._name = name;
            com = new CClientTCP(_idNo, _name, EDataType.HEX格式); 
        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 字段
        private int _idNo;
        private string _name = "Mitsubishi_FX3U";
        private CClientTCP com = null;
        private int _wordNum = 2;
        private object _sync = new object();
        #endregion

        #region 属性
        /// <summary>
        /// ID编号
        /// </summary>
        public int idNo
        {
            get
            {
                return _idNo;
            }
            set
            {
                _idNo=value;
            }
        }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name=value;
            }
        }
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool conStatus
        {
            get
            {
                if (com == null)
                    return false;
                else
                    return com.conStatus;
            }
        }
        /// <summary>
        /// 字节长度
        /// </summary>
        public int wordNum
        {
            get { return _wordNum; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <param name="comName">IP地址</param>
        /// <param name="er"></param>
        /// <param name="setting">端口:5002</param>
        /// <returns></returns>
        public bool Open(string comName, out string er, string setting)
        {
            try
            {
                if (!com.open(comName, out er, setting))
                    return false;

                if (!GetPLCType(out er))
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
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            com.close();
        }
        /// <summary>
        /// 读线圈和寄存器值
        /// </summary>
        /// <param name="plcAddr">0</param>
        /// <param name="regType">寄存器类型</param>
        /// <param name="startAddr">开始地址</param>
        /// <param name="N">双字节(FFFF)为1个长度</param>
        /// <param name="rData">16进制字符(双字节):数据值高位在前,低位在后</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Read(int plcAddr, ERegType regType, int startAddr, int N, out string rData, out string er)
        {
            lock (_sync)
            {
                rData = string.Empty;

                er = string.Empty;

                try
                {
                    string rCmd = string.Empty;

                    if (regType != ERegType.D)
                    {
                        if (!ReadBinCmd(regType, startAddr, N, out rCmd, out rData, out er))
                            return false;

                        rData = rData.Substring(0, N);
                    }
                    else
                    {
                        if (!ReadWordCmd(regType, startAddr, N, out rCmd, out rData, out er))
                            return false;

                        rData = rData.Substring(0, N * 4);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    er = ex.ToString();
                    return false;
                }
            }
        }
        /// <summary>
        /// 读单个寄存器值
        /// </summary>
        /// <param name="plcAddr">0</param>
        /// <param name="ERegType">寄存器类型</param>
        /// <param name="startAddr">开始地址</param>
        /// <param name="N">双字节(FFFF)为1个长度</param>
        /// <param name="rVal">数据值</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Read(int plcAddr, ERegType regType, int startAddr, int startBin, out int rVal, out string er)
        {
            lock (_sync)
            {
                rVal = 0;

                er = string.Empty;

                try
                {
                    string rData = string.Empty;

                    string rCmd = string.Empty;

                    if (regType != ERegType.D)
                    {
                        if (!ReadBinCmd(regType, startAddr, 1, out rCmd, out rData, out er))
                            return false;

                        rVal = System.Convert.ToInt16(rData.Substring(0, 1), 16);
                    }
                    else
                    {
                        if (!ReadWordCmd(regType, startAddr, 1, out rCmd, out rData, out er))
                            return false;

                        rVal = System.Convert.ToInt32(rData.Substring(0,4), 16);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    er = ex.ToString();
                    return false;
                }
            }
        }
        /// <summary>
        /// 读多个寄存器值
        /// </summary>
        /// <param name="plcAddr">0</param>
        /// <param name="regType">寄存器类型</param>
        /// <param name="startAddr">开始地址</param>
        /// <param name="rVal">多个寄存器数据</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Read(int plcAddr, ERegType regType, int startAddr, ref int[] rVal, out string er)
        {
            lock (_sync)
            {
                er = string.Empty;

                try
                {
                    string rData = string.Empty;

                    string rCmd = string.Empty;

                    int N = rVal.Length;

                    int addrLen = rVal.Length;

                    if (regType != ERegType.D)
                    {
                        if (!ReadBinCmd(regType, startAddr, addrLen, out rCmd, out rData, out er))
                            return false;

                        for (int i = 0; i < N; i++)
                        {
                             rVal[i] = System.Convert.ToInt32(rData.Substring(i,1), 16);
                        }
                    }
                    else
                    {

                        if (!ReadWordCmd(regType, startAddr, addrLen, out rCmd, out rData, out er))
                            return false;

                        for (int i = 0; i < N; i++)
                            rVal[i] = System.Convert.ToInt32(rData.Substring(i * 4, 4), 16);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    er = ex.ToString();
                    return false;
                }
            }
        }
        /// <summary>
        /// 写多线圈和寄存器值
        /// </summary>
        /// <param name="plcAddr">0</param>
        /// <param name="regType">寄存器类型</param>
        /// <param name="startAddr">开始地址</param>
        /// <param name="N">双字节(FFFF)为1个长度</param>
        /// <param name="strHex">16进制字符格式:FFFF FFFF(N=2) 高4位在前，低4位在后</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Write(int plcAddr, ERegType regType, int startAddr, int N, string strHex, out string er)
        {
            lock (_sync)
            {
                er = string.Empty;

                try
                {
                    string rCmd = string.Empty;

                    string rData = string.Empty;

                    if (regType != ERegType.D)
                    {
                        string wData = string.Empty;

                        for (int i = 0; i < N; i++)
                            wData += "0000";

                        //反转:低为在前,高位在后->Mobus协议

                        string temp = string.Empty;

                        for (int i = 0; i < strHex.Length / 4; i++)
                        {
                            temp = strHex.Substring(i * 4, 4) + temp;
                        }

                        wData += temp;

                        wData = wData.Substring(wData.Length - N * 4, N * 4);

                        //解析单位点

                        string wBit = string.Empty;

                        for (int i = 0; i < wData.Length / 4; i++)
                        {
                            for (int z = 0; z < 16; z++)
                            {
                                int HexBit = System.Convert.ToInt32(wData.Substring(i * 4, 4));

                                if ((HexBit & (1 << z)) == (1 << z))
                                    wBit += "1";
                                else
                                    wBit += "0";
                            }
                        }

                        if (!WriteBinCmd(regType, startAddr, N * 16, wBit, out er))
                            return false;
                    }
                    else
                    {
                        if (!WriteWordCmd(regType, startAddr, N, strHex, out er))
                            return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    er = ex.ToString();
                    return false;
                }
            }
        }
        /// <summary>
        /// 单写寄存器数据
        /// </summary>
        /// <param name="plcAddr">0</param>
        /// <param name="regType">寄存器类型</param>
        /// <param name="startAddr">开始地址</param>
        /// <param name="startBin">位地址W100.0->startBin=0</param>
        /// <param name="wVal">寄存器数值</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Write(int plcAddr, ERegType regType, int startAddr, int startBin, int wVal, out string er)
        {
            lock (_sync)
            {
                er = string.Empty;

                try
                {
                    string wCmd = ConvertToHex(wVal, 4);

                    if (regType != ERegType.D)
                    {
                        if (wVal == 1)
                            wCmd = "1";
                        else
                            wCmd = "0";

                        if (!WriteBinCmd(regType, startAddr, 1, wCmd, out er))
                            return false;
                    }
                    else
                    {
                        if (!WriteWordCmd(regType, startAddr, 1, wCmd, out er))
                            return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    er = ex.ToString();
                    return false;
                }
            }
        }
        /// <summary>
        /// 写多个线圈和寄存器
        /// </summary>
        /// <param name="plcAddr">0</param>
        /// <param name="regType">寄存器类型</param>
        /// <param name="startAddr">开始地址</param>
        /// <param name="wVal">多个寄存器数据值</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Write(int plcAddr, ERegType regType, int startAddr, int[] wVal, out string er)
        {
            lock (_sync)
            {
                er = string.Empty;

                try
                {
                    int N = wVal.Length;   //单写多个值

                    string wCmd = string.Empty;

                    if (regType != ERegType.D)
                    {
                        for (int i = 0; i < wVal.Length; i++)
                        {
                            if (wVal[i] == 1)
                                wCmd += "1";
                            else
                                wCmd += "0";
                        }

                        if (!WriteBinCmd(regType, startAddr, N, wCmd, out er))
                            return false;

                    }
                    else
                    {
                        for (int i = 0; i < N; i++)
                            wCmd = wVal[i].ToString("X4") + wCmd;

                        if (!WriteWordCmd(regType, startAddr, N, wCmd, out er))
                            return false;
                    }
                   
                    return true;
                }
                catch (Exception ex)
                {
                    er = ex.ToString();
                    return false;
                }
            }
        }
        #endregion

        #region MC通信协议--说明书:P156
        /*
        *
        *ASCII码通信协议:副标题(00)+ PC号(固定:FF)+ 监视定时器(00) + 数据段<低在前高在后>(起始元件地址[00000000] + 元件代码[0000] + 元件数[00]) + 结束代码(00) + {数据} 
        *
        *响应: 副标题(80) + 结束代码(00为正常,50-60为异常) + {数据段<低在前高在后>(起始元件地址[00000000] + 元件代码[0000] + 元件数[00])}
        
        *副标题(15)-->读取PLC型号
        *副标题(00)-->成批读位单位
        *副标题(01)-->成批读字16位单位
        *副标题(02)-->成批写位单位
        *副标题(03)-->成批写字16位单位
        *
        *监视定时器 0000-->无限等待  0001-FFFF 以250ms为单位
        *
        */
        /// <summary>
        /// Fins帧头
        /// </summary>
        private string _FinsHead = string.Empty;
        /// <summary>
        /// PLC型号F3
        /// </summary>
        private string _PLCType = "F3";
        /// <summary>
        /// 获取PLC型号15
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool GetPLCType(out string er)
        {
            er = string.Empty;

            try
            {
                _FinsHead = "15";   //命令
                _FinsHead += "FF";   //PC号
                _FinsHead += "000A";//监控定时器000A

                string wCmd = ASCII_To_StrHex(_FinsHead);

                int rLen = 8;

                string rData = string.Empty;

                string rASCII = string.Empty;

                string rVal = string.Empty;

                if (!com.send(wCmd, rLen, out rData, out er))
                    return false;

                rASCII = StrHex_To_ASCII(rData);

                if (!IsSOI(rASCII, "9500"))
                {
                    er = "接收帧头错误:" + rASCII;
                    return false;
                }

                if (!IsEOI(rASCII, "00"))
                {
                    er = "接收帧头错误:" + rASCII;
                    return false;
                }

                _PLCType = rASCII.Substring(4, 2); 

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 读取位单位寄存器
        /// </summary>
        /// <param name="regType"></param>
        /// <param name="startAddr"></param>
        /// <param name="N"></param>
        /// <param name="strHex"></param>
        /// <param name="rData"></param>
        /// <param name="rVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool ReadBinCmd(ERegType regType, int startAddr, int N, out string rData, out string rVal, out string er)
        {
            rData = string.Empty;

            rVal = string.Empty;

            er = string.Empty;

            try
            {
                string regName = GetRegAddr(regType);

                string regAddr = startAddr.ToString("X8");

                string regLen = N.ToString("X2");

                string wCmd = "00"; //副标题命令 --位读

                wCmd += "FF"; //PC号

                wCmd += "000A"; //监控定时器

                wCmd += StrHexToInversion(regName); //元件类型

                wCmd += StrHexToInversion(regAddr); //元件地址

                wCmd += regLen; //元件长度

                wCmd += "00";  //结束代码

                int rLen = 4 + N;  //D地址 

                string wHexCmd = ASCII_To_StrHex(wCmd);

                if (!com.send(wHexCmd, rLen, out rData, out er))
                    return false;

                if (!CalFinsValue("80", rData, N, out rVal, out er))
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
        /// 读取16位字单位寄存器
        /// </summary>
        /// <param name="regType"></param>
        /// <param name="startAddr"></param>
        /// <param name="N"></param>
        /// <param name="strHex"></param>
        /// <param name="rData"></param>
        /// <param name="rVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool ReadWordCmd(ERegType regType, int startAddr, int N, out string rData, out string rVal, out string er)
        {
            rData = string.Empty;

            rVal = string.Empty;

            er = string.Empty;

            try
            {
                string regName = GetRegAddr(regType);

                string regAddr = startAddr.ToString("X8");

                string regLen = N.ToString("X2");

                string wCmd = "01"; //副标题命令 --16点为单位读取

                wCmd += "FF"; //PC号

                wCmd +="000A"; //监控定时器

                wCmd += StrHexToInversion(regName); //元件类型

                wCmd += StrHexToInversion(regAddr); //元件地址

                wCmd += regLen; //元件长度

                wCmd += "00";  //结束代码

                int rLen = 4 + N * 4;  //D地址 

                string wHexCmd = ASCII_To_StrHex(wCmd);

                if (!com.send(wHexCmd, rLen, out rData, out er))
                    return false;

                if(!CalFinsValue("81",rData, N,out rVal,out er))
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
        /// 写入位单位寄存器
        /// </summary>
        /// <param name="regType"></param>
        /// <param name="startAddr"></param>
        /// <param name="N"></param>
        /// <param name="content"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool WriteBinCmd(ERegType regType, int startAddr, int N, string content, out string er)
        {
            er = string.Empty;

            try
            {
                string regName = GetRegAddr(regType);

                string regAddr = startAddr.ToString("X8");

                string regLen = N.ToString("X2");

                string wCmd = "02"; //副标题命令 --单位写

                wCmd += "FF"; //PC号

                wCmd += "000A"; //监控定时器

                wCmd += StrHexToInversion(regName); //元件类型

                wCmd += StrHexToInversion(regAddr); //元件地址

                wCmd += regLen; //元件长度

                wCmd += "00";  //结束代码

                if (content.Length % 2 != 0)
                    content += "0";

                wCmd += content;

                int rLen = 4; 

                string rData = string.Empty;

                string rVal = string.Empty;

                string wHexCmd = ASCII_To_StrHex(wCmd);

                if (!com.send(wHexCmd, rLen, out rData, out er))
                    return false;

                if (!CalFinsValue("82", rData, N, out rVal, out er))
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
        /// 写入16位字单位寄存器
        /// </summary>
        /// <param name="regType"></param>
        /// <param name="startAddr"></param>
        /// <param name="N"></param>
        /// <param name="content"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool WriteWordCmd(ERegType regType, int startAddr, int N, string content, out string er)
        {
            er = string.Empty;

            try
            {
                string regName = GetRegAddr(regType);

                string regAddr = startAddr.ToString("X8");

                string regLen = N.ToString("X2");

                string wCmd = "03"; //副标题命令 --16点为单位写

                wCmd += "FF"; //PC号

                wCmd += "000A"; //监控定时器

                wCmd += StrHexToInversion(regName); //元件类型

                wCmd += StrHexToInversion(regAddr); //元件地址

                wCmd += regLen; //元件长度

                wCmd += "00";  //结束代码

                string wData = string.Empty;

                for (int i = 0; i < N; i++)
                    wData += "0000";

                //反转:低为在前,高位在后->Mobus协议

                string temp = string.Empty;

                for (int i = 0; i < content.Length / 4; i++)
                    temp = content.Substring(i * 4, 4) + temp;

                wData += temp;

                wData = wData.Substring(wData.Length - N * 4, N * 4);

                wCmd += wData;

                int rLen = 4;  //D地址 

                string rData = string.Empty;

                string rVal = string.Empty;

                string wHexCmd = ASCII_To_StrHex(wCmd);

                if (!com.send(wHexCmd, rLen, out rData, out er))
                    return false;

                if (!CalFinsValue("83", rData, N, out rVal, out er))
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
        /// 获取设备寄存器类型地址
        /// B1->WR_WORD 31->WR_BIT  
        /// 82->DM_WORD 
        /// B0->IO_WORD 30->IO_BIT
        /// </summary>
        /// <param name="regType"></param>
        /// <returns></returns>
        private string GetRegAddr(ERegType regType)
        {
            string addrType = string.Empty;

            switch (regType)
            {
                case ERegType.M:
                    addrType = "4D20";
                    break;
                case ERegType.D:
                    addrType = "4420";
                    break;
                case ERegType.X:
                    addrType = "5820";
                    break;
                case ERegType.Y:
                    addrType = "5920";
                    break;
                default:
                    break;
            }
            return addrType;
        }
        /// <summary>
        /// 解析返回数据
        /// </summary>
        /// <param name="rData"></param>
        /// <param name="rVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool CalFinsValue(string soi, string rData, int rLen, out string rVal, out string er)
        {
            rVal = string.Empty;

            er = string.Empty;

            try
            {
                int N = 4; //固定长度=4

                string StrASCII = StrHex_To_ASCII(rData);

                if (!IsSOI(StrASCII, soi + "00"))
                {
                    er = CLanguage.Lan("接收数据帧头错误") + ":" + StrASCII;
                    return false;
                }

                rVal = StrASCII.Substring(N, StrASCII.Length - N);

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();

                return false;
            }
        }
        /// <summary>
        /// 转化为16进制数据
        /// </summary>
        /// <param name="val"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private string ConvertToHex(int val, int num = 2)
        {
            string str = val.ToString("X" + num);

            for (int i = 0; i < 2; i++)
                str = "0" + str;

            str = str.Substring(str.Length - num, num);

            return str;

        }
        /// <summary>
        /// 16进字符高低为交换
        /// </summary>
        /// <param name="strHex"></param>
        /// <returns></returns>
        private string StrHexToInversion(string strHex)
        {
            try
            {
                string Str = string.Empty;

                for (int i = 0; i < strHex.Length/2; i++)
                {
                    Str = Str + strHex.Substring(i * 2, 2);
                }

                return Str;
            }
            catch (Exception)
            {
                return strHex;
            }
        }
        /// <summary>
        /// ASCII转16字符串
        /// </summary>
        /// <returns></returns>
        private string ASCII_To_StrHex(string strASCII)
        {
            try
            {
                if (strASCII == string.Empty)
                    return "";

                string StrHex = string.Empty;

                byte[] ASCI_Bytes = System.Text.Encoding.ASCII.GetBytes(strASCII);

                for (int i = 0; i < ASCI_Bytes.Length; i++)
                {
                    StrHex += ASCI_Bytes[i].ToString("X2"); 
                }

                return StrHex;
            }
            catch (Exception)
            {
                return "";
            }
        }
        /// <summary>
        /// 16字符串转ASCII
        /// </summary>
        /// <param name="strHex"></param>
        /// <returns></returns>
        private string StrHex_To_ASCII(string strHex)
        {
            try
            {
                string StrASII=string.Empty;

                if (strHex == string.Empty)
                    return "";

                byte[] array = new byte[strHex.Length / 2]; 

                for (int i = 0; i < strHex.Length/2; i++)
                {
                    array[i] = System.Convert.ToByte(strHex.Substring(i * 2, 2), 16);

                    char c = System.Convert.ToChar(array[i]);

                    StrASII += c;
                }

                return StrASII;
            }
            catch (Exception)
            {
                return "";
            }
        }
        /// <summary>
        /// 帧头检查
        /// </summary>
        /// <param name="StrASCII"></param>
        /// <param name="SOI"></param>
        /// <returns></returns>
        private bool IsSOI(string StrASCII, string SOI)
        {
            try
            {
                int len = SOI.Length;

                if (StrASCII.Substring(0, len) != SOI)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 帧尾检查
        /// </summary>
        /// <param name="StrASCII"></param>
        /// <param name="SOI"></param>
        /// <returns></returns>
        private bool IsEOI(string StrASCII, string EOI)
        {
            try
            {
                int len = EOI.Length;

                if (StrASCII.Substring(StrASCII.Length - len, len) != EOI)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
