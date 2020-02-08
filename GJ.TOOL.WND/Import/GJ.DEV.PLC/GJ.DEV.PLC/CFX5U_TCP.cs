using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;
using GJ.COM;

namespace GJ.DEV.PLC
{
    public class CFX5U_TCP : IPLC
    {
        #region 构造函数
        public CFX5U_TCP(int idNo = 0, string name = "Mitsubishi_FX5U")
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
        private string _name = "Mitsubishi_FX5U";
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
                _idNo = value;
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
                _name = value;
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

                        rVal = System.Convert.ToInt32(rData.Substring(0, 4), 16);
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
                            rVal[i] = System.Convert.ToInt32(rData.Substring(i, 1), 16);
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

                        if (!WriteWordCmd(regType, startAddr, N, wData, out er))
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
                            wCmd += wVal[i].ToString("X4");

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

        #region SLMP通信协议--说明书:P25
        /*
        *
        *ASCII码通信协议:副标题(5000) + 网络编号(00) + PC编号(FF) + I/O编号(03FF) + 多点站号(00) + 请求数据长度(0018) + 保留(0000) + 指令 + 子指令 + 请求数据
        *
        *响应: 副标题(D000) + 网络编号(00) +  PC编号(FF) + I/O编号(03FF) + 多点站号(00) + 响应数据长度(000C) + 结束代码(0000) + 响应数据段
        
         * 网络编号:0x01-0xEF(1-239)              --FX5U不支持
         * PC编号: 0x01-0x78(1-120)   7D/7E为主站 --FX5U不支持
         * I/O编号：0x03FF->本站 0x0001-0x01FF 多点连接模块 
         * 多点站号:0x00
         * 请求数据长度= 保留(0000) + 指令 + 子指令 + 请求数据
         * 指令:0x0401-批量读取 0x1401-批量写入
         * 子指令:0x0000-->字 0x0001->位
         * 请求数据 =  元件代码[M*] + 起始元件地址[000000] + 元件数[0000] + 数据
         * 响应数据长度=结束代码(0000) + 响应数据段
        */

        /// <summary>
        /// 请求帧头
        /// </summary>
        private string _RequestHead = "500000FF03FF00";
        /// <summary>
        /// 响应帧头
        /// </summary>
        private string _ReponseHead = "D00000FF03FF00";
        /// <summary>
        /// PLC型号F3
        /// </summary>
        private string _PLCType = "FX5U-32MT/ES";
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
                _RequestHead = "5000";   //副标题
                _RequestHead += "00";    //网络编号
                _RequestHead += "FF";    //PC号
                _RequestHead += "03FF";  //I/O编号
                _RequestHead += "00";    //多点站号

                _ReponseHead = "D000";
                _ReponseHead += "00";    //网络编号
                _ReponseHead += "FF";    //PC号
                _ReponseHead += "03FF";  //I/O编号
                _ReponseHead += "00";    //多点站号

                string wData = "0000"; //保留位   

                wData += "0101"; //指令      

                wData += "0000";  //子指令

                string wCmd = CalSendSLMPValue(wData);

                int rLen = _ReponseHead.Length + 8;

                string rData = string.Empty;

                string rASCII = string.Empty;

                string rVal = string.Empty;

                if (!com.send(wCmd, rLen, out rData, out er))
                    return false;

                if (!CalRtnSLMPValue(rData, rLen, out rVal, out er))
                    return false;

                _PLCType = rVal;

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

                string regAddr = startAddr.ToString("D6");

                string regLen = N.ToString("X4");

                string wData = "0000"; //保留位   

                wData += "0401"; //指令-批量读取      

                wData += "0001";  //子指令-位读取

                wData += regName; //元件名称

                wData += regAddr; //元件地址

                wData += regLen;  //元件长度

                string wCmd = CalSendSLMPValue(wData);

                int rLen = _ReponseHead.Length + 8;

                string rASCII = string.Empty;

                if (!com.send(wCmd, rLen, out rData, out er))
                    return false;

                if (!CalRtnSLMPValue(rData, rLen, out rVal, out er))
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

                string regAddr = startAddr.ToString("D6");

                string regLen = N.ToString("X4");

                string wData = "0000"; //保留位   

                wData += "1401"; //指令-批量写入      

                wData += "0001";  //子指令-位写入

                wData += regName; //元件名称

                wData += regAddr; //元件地址

                wData += regLen;  //元件长度

                wData += content;  //数据

                string rData = string.Empty;

                string rVal = string.Empty;

                string wCmd = CalSendSLMPValue(wData);

                int rLen = _ReponseHead.Length + 8;

                string rASCII = string.Empty;

                if (!com.send(wCmd, rLen, out rData, out er))
                    return false;

                if (!CalRtnSLMPValue(rData, rLen, out rVal, out er))
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

                string regAddr = startAddr.ToString("D6");

                string regLen = N.ToString("X4");

                string wData = "0000"; //保留位   

                wData += "0401"; //指令-批量读取      

                wData += "0000";  //字指令-位读取

                wData += regName; //元件名称

                wData += regAddr; //元件地址

                wData += regLen;  //元件长度

                string wCmd = CalSendSLMPValue(wData);

                int rLen = _ReponseHead.Length + 8;

                string rASCII = string.Empty;

                if (!com.send(wCmd, rLen, out rData, out er))
                    return false;

                if (!CalRtnSLMPValue(rData, rLen, out rVal, out er))
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

                string regAddr = startAddr.ToString("D6");

                string regLen = N.ToString("X4");

                string wData = "0000"; //保留位   

                wData += "1401"; //指令-批量写入      

                wData += "0000";  //子指令-字写入

                wData += regName; //元件名称

                wData += regAddr; //元件地址

                wData += regLen;  //元件长度

                wData += content;  //数据

                string rData = string.Empty;

                string rVal = string.Empty;

                string wCmd = CalSendSLMPValue(wData);

                int rLen = _ReponseHead.Length + 8;

                string rASCII = string.Empty;

                if (!com.send(wCmd, rLen, out rData, out er))
                    return false;

                if (!CalRtnSLMPValue(rData, rLen, out rVal, out er))
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
                    addrType = "M*";
                    break;
                case ERegType.D:
                    addrType = "D*";
                    break;
                case ERegType.X:
                    addrType = "X*";
                    break;
                case ERegType.Y:
                    addrType = "Y*";
                    break;
                default:
                    break;
            }
            return addrType;
        }
        /// <summary>
        /// 格式化发送16进制数据
        /// </summary>
        /// <param name="wCmd"></param>
        /// <returns></returns>
        private string CalSendSLMPValue(string wData)
        {
            try
            {
                string Cmd = _RequestHead;

                string len = wData.Length.ToString("X4");

                Cmd += len;

                Cmd += wData;

                string HexData = ASCII_To_StrHex(Cmd);

                return HexData;

            }
            catch (Exception)
            {
                return "";
            }
        }
        /// <summary>
        /// 解析返回数据
        /// </summary>
        /// <param name="rData"></param>
        /// <param name="rVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool CalRtnSLMPValue(string rData, int rLen, out string rVal, out string er)
        {
            rVal = string.Empty;

            er = string.Empty;

            try
            {
                string StrASCII = StrHex_To_ASCII(rData);

                if (StrASCII.Length < _ReponseHead.Length + 8)
                {
                    er = CLanguage.Lan("接收数据长度错误") + ":" + StrASCII;
                    return false;
                }

                if (StrASCII.Substring(0, _ReponseHead.Length) != _ReponseHead)
                {
                    er = CLanguage.Lan("接收数据帧头错误") + ":" + StrASCII;
                    return false;
                }

                string rCmd = StrASCII.Substring(_ReponseHead.Length, StrASCII.Length - _ReponseHead.Length);

                int N = System.Convert.ToInt32(rCmd.Substring(0, 4),16);

                string EOI = rCmd.Substring(4, 4);

                if (EOI != "0000")
                {
                    er = CLanguage.Lan("接收数据错误") + ":" + StrASCII;
                    return false;
                }

                rVal = rCmd.Substring(8, rCmd.Length - 8);

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
                string StrASII = string.Empty;

                if (strHex == string.Empty)
                    return "";

                byte[] array = new byte[strHex.Length / 2];

                for (int i = 0; i < strHex.Length / 2; i++)
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
        #endregion
    }
}
