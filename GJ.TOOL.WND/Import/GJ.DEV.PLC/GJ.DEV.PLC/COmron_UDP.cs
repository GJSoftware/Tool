using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;
using GJ.DEV.COM;
using System.Net;

namespace GJ.DEV.PLC
{
    /// <summary>
    /// 寄存器D范围:0-4095
    /// 寄存器W范围:0-255
    /// </summary>
    public class COmron_UDP : IPLC
    {
        #region 构造函数
        public COmron_UDP(int idNo = 0, string name = "OmronFins_TCP")
        {
            this._idNo = idNo;
            this._name = name;
            com = new CClientUDP(_idNo, _name, EDataType.HEX格式); 
        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 字段
        private int _idNo;
        private string _name = string.Empty;
        private CClientUDP com = null;
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
        /// <param name="setting">端口:9600</param>
        /// <returns></returns>
        public bool Open(string comName, out string er, string setting)
        {
            try
            {
                _plcIP = comName; 

                _plcPort = System.Convert.ToInt32(setting);

                formatFinsHead(comName);

                if (!com.open(comName, out er, setting))
                    return false;

                int rVal=0;

                if (!Read(0, ERegType.D, 100, 0, out rVal, out er))
                {
                    CTimer.DelayMs(20);
 
                    if (!Read(0, ERegType.D, 100, 0, out rVal, out er))
                    {
                        er = "无法连接远程PLC["+ comName + ":" + setting + "]";
                        return false;
                    }                
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
        /// 断开连接
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

                    if (!sendOmronCommand("0101", regType, startAddr, 0, N, "", out rCmd, out rData, out er))
                        return false;

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

                    if (regType == ERegType.WB)
                    {
                        if (!sendOmronCommand("0101", regType, startAddr, startBin, 1, "", out rCmd, out rData, out er))
                            return false;
                    }
                    else
                    {
                        if (!sendOmronCommand("0101", regType, startAddr, startBin, 1, "", out rCmd, out rData, out er))
                            return false;
                    }

                    rVal = System.Convert.ToInt32(rData,16);

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

                    if (regType == ERegType.WB)
                        addrLen = addrLen / 16;

                    if (!sendOmronCommand("0101", regType, startAddr, 0, addrLen, "", out rCmd, out rData, out er))
                        return false;

                    if (regType == ERegType.WB)
                    {
                        for (int i = 0; i < rData.Length / 2; i++)
                        {
                            int valByte = System.Convert.ToInt32(rData.Substring(rData.Length - (i + 1) * 2, 2), 16);
                            for (int j = 0; j < 8; j++)
                            {
                                if ((j + 8 * i) < N)
                                {
                                    if ((valByte & (1 << j)) == (1 << j))
                                        rVal[j + i * 8] = 1;
                                    else
                                        rVal[j + i * 8] = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < N; i++)
                            rVal[i] = System.Convert.ToInt32(rData.Substring(rData.Length - (i + 1) * 4, 4), 16);

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

                    if (!sendOmronCommand("0102", regType, startAddr, 0, N, strHex, out rCmd, out rData, out er))
                        return false;

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
                    string wCmd = convertToHex(wVal, 4);

                    string rData = string.Empty;

                    string rCmd = string.Empty;

                    if (regType == ERegType.WB)
                    {
                        if (!sendOmronCommand("0102", ERegType.WB, startAddr, startBin, 1, wCmd, out rCmd, out rData, out er))
                            return false;
                    }
                    else
                    {
                        if (!sendOmronCommand("0102", regType, startAddr, startBin, 1, wCmd, out rCmd, out rData, out er))
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

                    if (regType == ERegType.WB)
                    {
                        int wLen = (7 + N) / 8;    //写入字节数
                        for (int i = 0; i < wLen; i++)
                        {
                            int val = 0;
                            for (int j = 0; j < 8; j++)
                            {
                                if (i * 8 + j < N)
                                {
                                    int bit = (wVal[i * 8 + j] & 0x1) << j;
                                    val += bit;
                                }
                            }
                            wCmd += val.ToString("X2");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < N; i++)
                            wCmd = wVal[i].ToString("X4") + wCmd;
                    }


                    string rData = string.Empty;

                    string rCmd = string.Empty;


                    if (!sendOmronCommand("0102", regType, startAddr, 0, N, wCmd, out rCmd, out rData, out er))
                        return false;

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

        #region Fins通信协议
        /*
          协议:  Host Link header + FINS Command frame + Host Link FCS + Host Link terminator
         * 
         *       FINS Command frame=ICF(0x80) + RSV(0x00) + GCT(0x02) + DNA(0x00) + DA1(PLC IP地址) + DA2(0x00) +
         *                          SNA(0x00) + SA1(PC地址) + SA2(0x00)+ SID(0x00-0xFF)
         
        */
        /// <summary>
        /// 本地PC地址
        /// </summary>
        private string _localIP = string.Empty;
        /// <summary>
        /// 本地PC端口
        /// </summary>
        private int _localPort = 0;
        /// <summary>
        /// 本地IP最后1位
        /// </summary>
        private byte _localEndByte = 0;
        /// <summary>
        /// PLC地址
        /// </summary>
        private string _plcIP = string.Empty;
        /// <summary>
        /// PLC端口
        /// </summary>
        private int _plcPort = 9600;
        /// <summary>
        /// PLC地址最后1位
        /// </summary>
        private byte _plcEndByte = 0;
        /// <summary>
        /// Fins帧头
        /// </summary>
        private string _FinsHead = string.Empty;
        /// <summary>
        /// 格式化帧头
        /// </summary>
        private void formatFinsHead(string serIP)
        {
            try
            {
                //PLC IP地址
                IPAddress serAddr = IPAddress.Parse(serIP);

                byte[] serByte = serAddr.GetAddressBytes();

                _plcEndByte = serByte[3]; 

                //获取与PLC同一网段的本地IP

                IPAddress[] addrList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

                for (int i = 0; i < addrList.Length; i++)
                {
                    byte[] addrByte = addrList[i].GetAddressBytes();

                    if (addrByte[0] == serByte[0] && addrByte[1] == serByte[1] && addrByte[2] == serByte[2])
                    {
                        _localIP = addrList[i].ToString();

                        _localPort = _idNo; 

                        _localEndByte = addrByte[3];

                        break;
                    }
                }

                _FinsHead = "800002"; /*固定的帧头*/

                _FinsHead += "00" + _plcEndByte.ToString("X2") + "00";  /*设备的网络编号,节点号,单元号*/

                _FinsHead += "00" + _localEndByte.ToString("X2") + "00"; /*PC的网络编号,节点号,单元号*/

                _FinsHead += "00"; //SID               

            }
            catch (Exception)
            {                
                throw;
            }
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="wCommandType">读写类型</param>
        /// <param name="regType">寄存器类型</param>
        /// <param name="startAddr">开始地址</param>
        /// <param name="N">长度</param>
        /// <param name="strHex">
        /// 读:空,
        /// 写:wWriteContent为4位Hex数据(高位在前,低位在后)
        /// </param>
        /// <param name="rData"></param>
        /// <param name="rVal">高位在前,低位在后</param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool sendOmronCommand(string wCommandType, ERegType regType, int startAddr,int startBin, int N, string strHex, 
                                      out string rData, out string rVal, out string er)                                         
        {
             rData = string.Empty;
             
             rVal = string.Empty;

             er = string.Empty;

             try
             {
                 string startAddrCmd=string.Empty;

                 string startAddrLen=string.Empty;

                 string wRegData=string.Empty;

                 if(regType == ERegType.WB)  
                 {
                     startAddrCmd = convertToHex(startAddr, 4) + convertToHex(startBin, 2); 

                     startAddrLen="0001";

                    if(strHex!=string.Empty)
                    {
                        if(System.Convert.ToInt16(strHex,16)>0)
                            wRegData="01";
                        else
                            wRegData="00";
                    }
                 }
                 else
                 {
                    startAddrCmd=convertToHex(startAddr,4) + "00";
                    
                    startAddrLen=convertToHex(N, 4);

                    if (strHex != string.Empty)
                    {
                        string temp = string.Empty;

                        for (int i = 0; i < N; i++)
                            temp += "0000";

                        //反转:低位在前,高位在后
                        for (int i = 0; i < strHex.Length / 4; i++)
                            wRegData = strHex.Substring(i * 4, 4) + wRegData;

                        wRegData = temp + wRegData;

                        wRegData = wRegData.Substring(wRegData.Length - N * 4, N * 4);
                    }
                 }

                 string strCmd=_FinsHead; 

                 strCmd += wCommandType;

                 strCmd += getRegAddr(regType); 

                 strCmd += startAddrCmd;

                 strCmd += startAddrLen;

                 strCmd += wRegData;

                 int rLen = 14;

                 if(wCommandType =="0101") //读
                    rLen = N * 2 + 14;

                 if (!com.send(strCmd, rLen, out rData, out er))
                     return false;

                 if (wCommandType == "0101") //读
                     rLen = N;
                 else
                     rLen = 0;

                 if (!calFinsValue(wCommandType, regType,rData, rLen, out rVal, out er))
                     return false;

                 if (wCommandType == "0101") //读
                 {
                     if (rVal == string.Empty)
                     {
                         er = CLanguage.Lan("接收数据长度") + "=0";
                         return false;
                     }
                     //反转:高位在前,低位在后

                     string tempVal = string.Empty;

                     if (regType == ERegType.WB)
                     {
                         for (int i = 0; i < rVal.Length / 2; i++)
                             tempVal = rVal.Substring(i * 2, 2) + tempVal;
                     }
                     else
                     {
                         for (int i = 0; i < rVal.Length / 4; i++)
                             tempVal = rVal.Substring(i * 4, 4) + tempVal;
                     }
                     rVal = tempVal;
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
        /// 获取设备寄存器类型地址
        /// B1->WR_WORD 31->WR_BIT  
        /// 82->DM_WORD 
        /// B0->IO_WORD 30->IO_BIT
        /// </summary>
        /// <param name="regType"></param>
        /// <returns></returns>
        private string getRegAddr(ERegType regType)
        {
            string addrType = string.Empty;

            switch (regType)
            {
                case ERegType.M:
                    break;
                case ERegType.W:
                    addrType = "B1";
                    break;
                case ERegType.D:
                    addrType = "82";
                    break;
                case ERegType.X:
                    addrType = "B0";
                    break;
                case ERegType.Y:
                    addrType = "B0";
                    break;
                case ERegType.WB:
                    addrType = "31";
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
        private bool calFinsValue(string CmdType,ERegType regType,string rData, int rLen,out string rVal, out string er)
        {
            rVal = string.Empty;

            er = string.Empty;

            try
            {
                int N = 14; //固定长度=8

                string SOI_Fins = "C00002"; /*固定帧头*/

                SOI_Fins += "00" + _localEndByte.ToString("X2") + "00"; /* PC网络号，节点号(计算机IP最后一位)，单元号*/

                SOI_Fins += "00" + _plcEndByte.ToString("X2") + "00";   /* 设备网络号，节点号(计算机IP最后一位)，单元号*/

                SOI_Fins += "00" + CmdType; /*SID+MRC+SRC*/

                //SOI_Fins += "0000";  /*无错误*/

                if(rData.Length < SOI_Fins.Length)
                {
                    er = CLanguage.Lan("接收数据长度错误") + ":" + rData;
                    return false;
                }

                if (rData.Substring(0, SOI_Fins.Length) != SOI_Fins)
                {
                    er = CLanguage.Lan("接收数据错误") + ":" + rData;
                    return false;
                }

                if (regType != ERegType.WB)
                {
                    int len = N + rLen * 2;

                    if (rData.Length != len * 2)
                    {
                        er = CLanguage.Lan("接收数据长度错误") + "[" + len.ToString() + "]:" + rData;
                        return false;
                    }

                    rVal = rData.Substring(rData.Length - rLen * 4, rLen * 4);     
                }
                else
                {
                    int len = N + rLen;

                    if (rData.Length != len * 2)
                    {
                        er = CLanguage.Lan("接收数据长度错误") + "[" + len.ToString() + "]:" + rData;
                        return false;
                    }

                    rVal = rData.Substring(rData.Length - rLen * 2, rLen * 2);     
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
        /// 转化为16进制数据
        /// </summary>
        /// <param name="val"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private string convertToHex(int val, int num = 2)
        {
            string str = val.ToString("X" + num);

            for (int i = 0; i < 2; i++)
                str = "0" + str;

            str = str.Substring(str.Length - num, num);

            return str;

        }
        #endregion
    }
}
