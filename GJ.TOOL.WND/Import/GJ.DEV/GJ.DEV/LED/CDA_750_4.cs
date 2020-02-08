using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM; 
using GJ.DEV.COM;

#region 重命名
using CPara = GJ.DEV.LED.CLOAD;
using CData = GJ.DEV.LED.CData;
using CCHData = GJ.DEV.LED.CHAN_DATA;
#endregion

namespace GJ.DEV.LED
{
    public class CDA_750_4:ILED
    {
        #region 构造函数
        public CDA_750_4(int idNo, string name)
        {
            this._idNo = idNo;
            this._name = name;
        }
        public override string ToString()
        {
            return string.Format("<{0}>",_name);
        }
        #endregion

        #region 字段
        private int _idNo = 0;
        private string _name = string.Empty;
        private bool _conStatus = false;
        private int _maxCH = 8;
        private CSerialPort com;
        /// <summary>
        /// 协议数据长度=7+X--->SOI(1)+ADR(1)+CID(1)+LENGTH(1)+INFO(X)+CHKSUM(2)+EOI(1)
        /// </summary>
        private const int C_LEN = 7;
        #endregion

        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        public int idNo
        {
            get { return _idNo; }
            set {_idNo = value; }
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
        /// <summary>
        /// 负载通道数
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
        /// <param name="setting">9600,n,8,1</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Open(string comName, out string er, string setting="9600,N,8,1")
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
        /// 设置地址
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SetNewAddr(int wAddr, out string er)
        {
            er = string.Empty;

            try
            {
                if (wAddr < 1 || wAddr > 127)
                {
                    er = CLanguage.Lan("设置地址范围") + "[1-127]:" + wAddr.ToString();
                    return false;
                }
                string CID = "01";
                string INFO = wAddr.ToString("X2");
                string wCmd = CalDataFromERS272Cmd(0, CID,INFO);
                string rData = string.Empty;
                int rLen = C_LEN;
                if (!SendCmdToERS272(wCmd, rLen, out rData, out er))
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
        /// 读取版本
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="version"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadVersion(int wAddr, out string version, out string er)
        {
            er = string.Empty;

            version = string.Empty;

            try
            {
                string CID = "09";
                string INFO = string.Empty;
                string wCmd = CalDataFromERS272Cmd(wAddr, CID, INFO);
                string rData = string.Empty;
                int rLen = C_LEN +2 ;
                if (!SendCmdToERS272(wCmd, rLen, out rData, out er))
                    return false;
                string temp = string.Empty;
                string rVal = string.Empty;
                if (!checkErrorCode(rData, out rVal, out  temp))
                    return false;
                version = rVal;
                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }        
        }
        /// <summary>
        /// 读取负载设置
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="chanList"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadLoadSetting(int wAddr, out List<CLOAD> chanList, out string er)
        {
            chanList = new List<CLOAD>();

            chanList.Clear();

            er = string.Empty;

            try
            {
                string CID = "06";
                string INFO =string.Empty;
                string wCmd = CalDataFromERS272Cmd(wAddr, CID, INFO);
                string rData = string.Empty;
                int rLen = C_LEN + 48;
                if (!SendCmdToERS272(wCmd, rLen, out rData, out er))
                    return false;
                string temp = string.Empty;
                string rVal = string.Empty;
                if (!checkErrorCode(rData, out rVal, out  temp))
                    return false;
                /*
                 * 每个通道含6个字节数据，数据从第1通道开始，共6字节*8通道=48字节数据。
                 * 每通道的第1字节为负载模式，第2/3字节为Von点，第4/5字节为负载值，第6字节为模式附加参数，
                 * 模式附加参数根据模式不同而有不同的含义
                */

                for (int i = 0; i < _maxCH; i++)
                {
                    string rCmd = rVal.Substring(i * 12, 12);  //单通道数据

                    int byte0 = System.Convert.ToInt16(rCmd.Substring(0, 2), 16);  //负载模式

                    int byte1 = System.Convert.ToInt16(rCmd.Substring(2, 4), 16);  //Von点

                    int byte2 = System.Convert.ToInt16(rCmd.Substring(6, 4), 16);  //负载值

                    int byte3 = System.Convert.ToInt16(rCmd.Substring(10, 2), 16); //模式附加参数

                    CLOAD para = new CLOAD();

                    para.Mode = (EMODE)byte0;
                    
                    para.Von = (double)byte1 / 20;  //读值=Von*20(V) --->Von:0.5V-20V

                    switch (para.Mode)
                    {
                        case EMODE.CC_Slow:  
                            para.load = (double)byte2 / 1000; //CCSlow或CCFast或者MTK模式:读值=电流（A）*1000，取值范围0.1A～5A。
                            para.unit = "A";
                            para.mark = (double)byte3 / 10;  //当负载模式为CC Slow或者CC_Fast时，该参数的含义是负载启动延迟时间。                                                             
                            break;                            //读值=延迟时间（秒）*10，取值范围0～20秒
                        case EMODE.CV:   
                            para.load = (double)byte2 / 20;  //CV模式:读值=电压（V）*20，取值范围12V～400V
                            para.unit = "V";
                            para.mark = 0;                  //当负载模式为CV/CP/CR时，该参数无实际以及，需赋值为0
                            break;
                        case EMODE.CP:   
                            para.load = (double)byte2 / 10; //CP模式:读值=功率（W）*10，取值范围2W～320W
                            para.unit = "W";
                            para.mark = 0;    //当负载模式为CV/CP/CR时，该参数无实际以及，需赋值为0
                            break;
                        case EMODE.CR:  
                            para.load = (double)byte2 / 10; //CR模式:读值=阻值（欧）*10，取值范围2.4欧～4000欧
                            para.unit = "ohm";
                            para.mark = 0;    //当负载模式为CV/CP/CR时，该参数无实际以及，需赋值为0
                            break;
                        case EMODE.CC_Fast:
                            para.load = (double)byte2 / 1000;
                            para.unit = "A";
                            para.mark = (double)byte3 / 10;  //当负载模式为CC Slow或者CC_Fast时，该参数的含义是负载启动延迟时间。                                                             
                            break;                            //读值=延迟时间（秒）*10，取值范围0～20秒
                        case EMODE.LED:  
                            para.load = (double)byte2 / 20; //LED模式:读值=电压（V）*20，取值范围12V～400V
                            para.unit = "V";                
                            para.mark = (double)byte3 / 10;//当负载模式为LED模式时，该参数为LED电流。读值=电流（A）*10，取值范围：0.1A～5A
                            break;
                        case EMODE.MTK:
                            para.load = (double)byte2 / 1000;
                            para.unit = "A";
                            para.mark=(double)byte3;   /*当负载模式为MTK快充模式时，该参数为MKT快充电压类型。取值为1、2、3、4、5、6中的一个。
                                                        含义分别为：
                                                        1：MTK PE1.0   电压上升命令
                                                        2：MTK PE1.0   电压下降命令
                                                        3：MTK PE2.0   7V命令
                                                        4：MTK PE2.0  8V命令
                                                        5：MTK PE2.0  9V命令
                                                        6：MTK PE2.0  12V命令
                                                        */
                            break;
                        default:
                            break;
                    }

                    chanList.Add(para); 
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
        /// 设置8个电流通道负载
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="chanPara"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SetLoadValue(int wAddr, int chanNo, CLOAD chanPara, bool saveEEPROM, out string er)
        {
            er = string.Empty;

            try
            {

                string CID = string.Empty;

                string INFO = string.Empty;

                if (saveEEPROM)
                    CID = "11";
                else
                    CID = "12";

                INFO += chanNo.ToString("X2");  //通道号-1Bytes

                INFO += ((int)chanPara.Mode).ToString("X2");  //负载模式-1Bytes

                if (chanPara.Von < 10)
                    chanPara.Von = 10;

                INFO += ((int)(chanPara.Von * 20)).ToString("X4"); //Von点-2Bytes

                switch (chanPara.Mode)
                {
                    case EMODE.CC_Slow:
                        INFO += ((int)(chanPara.load * 1000)).ToString("X4"); //负载值:CCSlow或CCFast或者MTK模式时，设定值=电流（A）*1000，取值范围0.1A～5A
                        INFO += ((int)(chanPara.mark * 10)).ToString("X2");   //模式附加参数:负载启动延迟时间。设定值=延迟时间（秒）*10，取值范围0～20秒
                        break;
                    case EMODE.CV:
                        INFO += ((int)(chanPara.load * 20)).ToString("X4"); //负载值:CV模式时，设定值=电压（V）*20，取值范围12V～400V
                        INFO += "00";                                          //模式附加参数:该参数无实际以及，需赋值为0
                        break;
                    case EMODE.CP:
                        INFO += ((int)(chanPara.load * 10)).ToString("X4"); //负载值:CP模式时，设定值=功率（W）*10，取值范围2W～320W
                        INFO += "00";                                          //模式附加参数:该参数无实际以及，需赋值为0
                        break;
                    case EMODE.CR:
                        INFO += ((int)(chanPara.load * 10)).ToString("X4"); //负载值:CR模式时，设定值=阻值（欧）*10，取值范围2.4欧～4000欧
                        INFO += "00";                                          //模式附加参数:该参数无实际以及，需赋值为0
                        break;
                    case EMODE.CC_Fast:
                        INFO += ((int)(chanPara.load * 1000)).ToString("X4"); //负载值:CCSlow或CCFast或者MTK模式时，设定值=电流（A）*1000，取值范围0.1A～5A
                        INFO += ((int)(chanPara.mark * 10)).ToString("X2");   //模式附加参数:负载启动延迟时间。设定值=延迟时间（秒）*10，取值范围0～20秒
                        break;
                    case EMODE.LED:
                        INFO += ((int)(chanPara.load * 20)).ToString("X4"); //负载值:LED模式时，设定值=电压（V）*20，取值范围12V～400V
                        INFO += ((int)(chanPara.mark * 10)).ToString("X2");   //模式附加参数:当负载模式为LED模式时，该参数为LED电流。设定值=电流（A）*10，取值范围：0.1A～5A
                        break;
                    case EMODE.MTK:
                        INFO += ((int)(chanPara.load * 1000)).ToString("X4"); //负载值:CCSlow或CCFast或者MTK模式时，设定值=电流（A）*1000，取值范围0.1A～5A
                        INFO += ((int)(chanPara.mark)).ToString("X2");
                        break;
                    /*模式附加参数:当负载模式为MTK快充模式时，
                        * 该参数为MKT快充电压类型。取值为1、2、3、4、5、6中的一个。                         
                        * 含义分别为：
                        1：MTK PE1.0   电压上升命令
                        2：MTK PE1.0   电压下降命令
                        3：MTK PE2.0   7V命令
                        4：MTK PE2.0   8V命令
                        5：MTK PE2.0   9V命令
                        6：MTK PE2.0   12V命令
                    */
                    default:
                        break;
                }

                string wCmd = CalDataFromERS272Cmd(wAddr, CID, INFO);
                string rData = string.Empty;
                int rLen = C_LEN;
                if (!SendCmdToERS272(wCmd, rLen, out rData, out er))
                    return false;

                string temp = string.Empty;
                string rVal = string.Empty;
                if (!checkErrorCode(rData, out rVal, out  temp))
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
        /// 设置8个电流通道负载
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="chanList"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SetLoadValue(int wAddr, List<CLOAD> chanList,bool saveEEPROM, out string er)
        {
            er = string.Empty;

            try
            {

                if (chanList.Count != _maxCH)
                {
                    er = CLanguage.Lan("设置负载通道数量") + "[" + _maxCH.ToString() + "]" + CLanguage.Lan("错误") + ":" + chanList.Count.ToString();
                    return false;
                }

                string CID = string.Empty;

                string INFO = string.Empty;

                if (saveEEPROM)
                    CID = "40";
                else
                    CID = "41";

                for (int i = 0; i < chanList.Count; i++)
                {
                    INFO += ((int)chanList[i].Mode).ToString("X2");  //负载模式-1Bytes

                    INFO += ((int)(chanList[i].Von * 20)).ToString("X4"); //Von点-2Bytes

                    switch (chanList[i].Mode)
                    {
                        case EMODE.CC_Slow:  
                            INFO += ((int)(chanList[i].load * 1000)).ToString("X4"); //负载值:CCSlow或CCFast或者MTK模式时，设定值=电流（A）*1000，取值范围0.1A～5A
                            INFO += ((int)(chanList[i].mark * 10)).ToString("X2");   //模式附加参数:负载启动延迟时间。设定值=延迟时间（秒）*10，取值范围0～20秒
                            break;
                        case EMODE.CV:
                            INFO += ((int)(chanList[i].load * 20)).ToString("X4"); //负载值:CV模式时，设定值=电压（V）*20，取值范围12V～400V
                            INFO += "00";                                          //模式附加参数:该参数无实际以及，需赋值为0
                            break;
                        case EMODE.CP:
                            INFO += ((int)(chanList[i].load * 10)).ToString("X4"); //负载值:CP模式时，设定值=功率（W）*10，取值范围2W～320W
                            INFO += "00";                                          //模式附加参数:该参数无实际以及，需赋值为0
                            break;
                        case EMODE.CR:
                            INFO += ((int)(chanList[i].load * 10)).ToString("X4"); //负载值:CR模式时，设定值=阻值（欧）*10，取值范围2.4欧～4000欧
                            INFO += "00";                                          //模式附加参数:该参数无实际以及，需赋值为0
                            break;
                        case EMODE.CC_Fast:
                            INFO += ((int)(chanList[i].load * 1000)).ToString("X4"); //负载值:CCSlow或CCFast或者MTK模式时，设定值=电流（A）*1000，取值范围0.1A～5A
                            INFO += ((int)(chanList[i].mark * 10)).ToString("X2");   //模式附加参数:负载启动延迟时间。设定值=延迟时间（秒）*10，取值范围0～20秒
                            break;
                        case EMODE.LED:
                            INFO += ((int)(chanList[i].load * 20)).ToString("X4"); //负载值:LED模式时，设定值=电压（V）*20，取值范围12V～400V
                            INFO += ((int)(chanList[i].mark * 10)).ToString("X2");   //模式附加参数:当负载模式为LED模式时，该参数为LED电流。设定值=电流（A）*10，取值范围：0.1A～5A
                            break;
                        case EMODE.MTK:
                            INFO += ((int)(chanList[i].load * 1000)).ToString("X4"); //负载值:CCSlow或CCFast或者MTK模式时，设定值=电流（A）*1000，取值范围0.1A～5A
                            INFO += ((int)(chanList[i].mark)).ToString("X2");
                            break;
                            /*模式附加参数:当负载模式为MTK快充模式时，
                             * 该参数为MKT快充电压类型。取值为1、2、3、4、5、6中的一个。                         
                             * 含义分别为：
                                1：MTK PE1.0   电压上升命令
                                2：MTK PE1.0   电压下降命令
                                3：MTK PE2.0   7V命令
                                4：MTK PE2.0   8V命令
                                5：MTK PE2.0   9V命令
                                6：MTK PE2.0   12V命令
                            */
                        default:
                            break;
                    }
                }
                string wCmd = CalDataFromERS272Cmd(wAddr, CID, INFO);
                string rData = string.Empty;
                int rLen = C_LEN;
                if (!SendCmdToERS272(wCmd, rLen, out rData, out er))
                    return false;
                string temp = string.Empty;
                string rVal = string.Empty;
                if (!checkErrorCode(rData, out rVal, out  temp))
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
        /// 读取负载数据
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="data"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadLoadValue(int wAddr, ref CData data, out string er)
        {

            er = string.Empty;

            try
            {
                string CID = "0F";
                string INFO = string.Empty;
                string wCmd = CalDataFromERS272Cmd(wAddr, CID, INFO);
                string rData = string.Empty;
                int rLen = C_LEN + 65;
                if (!SendCmdToERS272(wCmd, rLen, out rData, out er))
                    return false;
                string temp = string.Empty;
                string rVal = string.Empty;
                if (!checkErrorCode(rData, out rVal, out  temp))
                    return false;
                /*                 
                 * 第1-64字节为各个通道的实时数据，
                 * 从第1通道开始，每个通道含8个字节数据
                 * 每通道的第1/2字节为负载电压值
                 * 第3/4字节为大电流通道的电流值
                 * 第5/6字节为LLC输入电压值
                 * 第7/8字节为通道状态信息
                 * 第65字节数据为逆变器状态信息。
                 */

                for (int i = 0; i < _maxCH; i++)
                {
                     string rCmd = rVal.Substring(i * 16, 16);  //单通道数据

                    int byte0 = System.Convert.ToInt16(rCmd.Substring(0, 4), 16);  //负载电压值

                    int byte1 = System.Convert.ToInt16(rCmd.Substring(4, 4), 16);  //负载电流值

                    int byte2 = System.Convert.ToInt16(rCmd.Substring(8, 4), 16);  //输入电压值

                    int byte3 = System.Convert.ToInt16(rCmd.Substring(12, 4), 16); //通道状态信息

                    CCHData ch = new CCHData();

                    ch.volt = (double)byte0 / 20;   //电压（V）=2byte值 /20

                    ch.current = (double)byte1 / 1000; //电流（A）=2byte值 /1000

                    ch.input_ac = (double)byte2 / 20;   //电压（V）=2byte值 /20

                    ch.alarmInfo = string.Empty;

                    //通道状态信息

                    //bit0
                    if ((byte3 & (1 << 0)) == (1 << 0))
                        ch.ch_status.DC_DC = EDC.ON;
                    else
                        ch.ch_status.DC_DC = EDC.OFF;                    

                    //bit1
                    if ((byte3 & (1 << 1)) == (1 << 1))
                    {
                        ch.ch_status.LLC_AC = ELLC.OVP;
                        ch.alarmInfo += "OVP;";
                    }
                    else
                    {
                        ch.ch_status.LLC_AC = ELLC.正常;
                    }

                    //bit2-bit3
                    int bit2_3= (byte3 & 0xC)/0x4;

                    if (bit2_3 == 0)
                    {
                        ch.ch_status.AC_STATUS = EAC.欠压;
                        ch.alarmInfo += CLanguage.Lan("输入电压欠压") + ";";
                    }
                    if (bit2_3 == 0x1)
                    {
                        ch.ch_status.AC_STATUS = EAC.正常;
                    }
                    else if (bit2_3 == 0x10)
                    {
                        ch.ch_status.AC_STATUS = EAC.过压;
                        ch.alarmInfo += CLanguage.Lan("输入电压过压") + ";";
                    }

                    //bit4
                    if ((byte3 & (1 << 4)) == (1 << 4))
                    {
                        ch.ch_status.OPP = EOPP.OPP;
                        ch.alarmInfo += "OPP;";
                    }
                    else
                    {
                        ch.ch_status.OPP = EOPP.正常;
                    }

                    //bit5
                    if ((byte3 & (1 << 5)) == (1 << 5))
                    {
                        ch.ch_status.OTP = EOTP.OTP;
                        ch.alarmInfo += "OTP;";
                    }
                    else
                    {
                        ch.ch_status.OTP = EOTP.正常;
                    }

                    //bit6-bit7
                    int bit6_7 = (byte3 & 0xC0) / 0x40;

                    if (bit6_7 == 0)
                    {
                        ch.ch_status.VBus = EVBus.欠压;
                        ch.alarmInfo += CLanguage.Lan("直流母线电压欠压") + ";";
                    }
                    if (bit6_7 == 0x1)
                    {
                        ch.ch_status.VBus = EVBus.正常;
                    }
                    else if (bit6_7 == 0x10)
                    {
                        ch.ch_status.VBus = EVBus.过压;
                        ch.alarmInfo += CLanguage.Lan("直流母线电压过压") + ";";
                    }

                    //bit8
                    if ((byte3 & (1 << 8)) == (1 << 8))
                    {
                        ch.ch_status.maxCur = EMaxCUR.过流;
                        ch.alarmInfo += CLanguage.Lan("大电流通道过流") + ";";
                    }
                    else
                    {
                        ch.ch_status.maxCur = EMaxCUR.正常;
                    }

                    //bit9
                    if ((byte3 & (1 << 9)) == (1 << 9))
                    {
                        ch.ch_status.minCur = EMinCUR.过流;
                        ch.alarmInfo += CLanguage.Lan("小电流通道过流") + ";";
                    }
                    else
                    {
                        ch.ch_status.minCur = EMinCUR.正常;
                    }

                    data.chan[i].volt = ch.volt;
                    data.chan[i].current = ch.current; 
                    data.chan[i].ch_status = ch.ch_status;
                    data.chan[i].alarmInfo = ch.alarmInfo;
                    data.chan[i].input_ac = ch.input_ac;
                }

                //逆变状态

                int byte4 = System.Convert.ToInt16(rVal.Substring(rVal.Length - 4, 4), 16); //通道状态信息

                data.alarmInfo= string.Empty;

                //bit0
                if ((byte4 & (1 << 0)) == (1 << 0))
                {
                    data.inv_status.OTP = EINVOTP.OTP;
                    data.alarmInfo += "OTP";
                }
                else
                {
                    data.inv_status.OTP = EINVOTP.正常;
                }
                //bit1
                if ((byte4 & (1 << 1)) == (1 << 1))
                {
                    data.inv_status.AD = EINVAD.故障;
                    data.alarmInfo += CLanguage.Lan("AD采样故障") + ";";
                }
                else
                {
                    data.inv_status.AD = EINVAD.正常;
                }

                //bit2-bit3
                int _bit2_3 = (byte4 & 0xC) / 0x4;

                if (_bit2_3 == 0)
                {
                    data.inv_status.ACBus = EINVACBus.欠压;
                    data.alarmInfo += CLanguage.Lan("电网电压状态欠压") + ";";
                }
                if (_bit2_3 == 0x1)
                {
                    data.inv_status.ACBus = EINVACBus.正常;
                }
                else if (_bit2_3 == 0x10)
                {
                    data.inv_status.ACBus = EINVACBus.过压;
                    data.alarmInfo += CLanguage.Lan("电网电压状态过压") + ";";
                }

                //bit4
                if ((byte4 & (1 << 4)) == (1 << 4))
                {
                    data.inv_status.Fan = EINVFan.故障;
                    data.alarmInfo += CLanguage.Lan("风扇故障") + ";";
                }
                else
                {
                    data.inv_status.Fan = EINVFan.正常;
                }
                //bit5
                if ((byte4 & (1 << 5)) == (1 << 5))
                {
                    data.inv_status.Time = EINVTIME.超时;
                    data.alarmInfo +=  CLanguage.Lan("超时故障") + ";" ;
                }
                else
                {
                    data.inv_status.Time = EINVTIME.正常;
                }

                //bit6-bit7
                int _bit6_7 = (byte4 & 0xC0) / 0x40;

                if (_bit6_7 == 0)
                {
                    data.inv_status.DCBus = EINVDCBus.欠压;
                    data.alarmInfo += CLanguage.Lan("直流母线电压状态欠压") + ";";
                }
                if (_bit6_7 == 0x1)
                {
                    data.inv_status.DCBus = EINVDCBus.正常;
                }
                else if (_bit6_7 == 0x10)
                {
                    data.inv_status.DCBus = EINVDCBus.过压;
                    data.alarmInfo += CLanguage.Lan("直流母线电压状态过压") + ";";
                }

                data.rCmd = rData; 

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
       * 发送:桢头[SOI](7E)+地址[ADR](值=0为广播命令,默认地址:77)+命令[CID]+长度[LENGTH]+数据[INFO]+检验和[CHKSUM]+桢尾[EOI](7F)
       * 应答:桢头(7E)+地址+长度+数据+检验和+桢尾(7F)         
      */
        /// <summary>
        /// 桢头
        /// </summary>
        private string SOI = "EE";
        /// <summary>
        /// 代替桢头及桢尾
        /// </summary>
        private string EOI = "EF";
        /// <summary>
        /// 桢尾
        /// </summary>
        private string ROI = "ED";
        /// <summary>
        /// 发串口数据
        /// </summary>
        /// <param name="wData"></param>
        /// <param name="rLen"></param>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <param name="wTimeOut"></param>
        /// <returns></returns>
        private bool SendCmdToERS272(string wData, int rLen, out string rData, out string er, int wTimeOut = 300)
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
        /// 格式协议数据
        /// </summary>
        /// <param name="ADR"></param>
        /// <param name="CID"></param>
        /// <param name="INFO"></param>
        /// <returns></returns>
        private string CalDataFromERS272Cmd(int ADR, string CID, string INFO)
        {
            try
            {
                string cmd = string.Empty;
                int len = 3 + INFO.Length / 2;
                string chkData = string.Empty;
                for (int i = 0; i < INFO.Length / 2; i++)
                {
                    if (INFO.Substring(i * 2, 2) == SOI || INFO.Substring(i * 2, 2) == EOI)
                        chkData += ROI;
                    else
                        chkData += INFO.Substring(i * 2, 2);
                }
                cmd = ADR.ToString("X2") + CID + len.ToString("X2") + chkData;
                cmd = SOI + cmd + CalCheckSum(cmd) + EOI;
                return cmd;
            }
            catch (Exception)
            {                
                throw;
            }          
        }
        /// <summary>
        /// 计算CheckSum
        /// </summary>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool ERS_CheckSum(string rData, out string er)
        {
            er = string.Empty;

            try
            {
                if (rData.Substring(0, 2) != SOI)
                {
                    er = CLanguage.Lan("数据桢头错误") + ":" + rData;
                    return false;
                }
                if (rData.Substring(rData.Length - 2, 2) != EOI)
                {
                    er = CLanguage.Lan("数据桢尾错误") + ":" + rData;
                    return false;
                }
                if (rData.Length / 2 < C_LEN)
                {
                    er = CLanguage.Lan("数据长度小于") + C_LEN.ToString() + ":" + rData;
                    return false;
                }
                int rLen = System.Convert.ToInt16(rData.Substring(6, 2), 16);
                if ((rData.Length / 2) != (rLen + 4))
                {
                    er = CLanguage.Lan("数据长度错误") + ":" + rData;
                    return false;
                }
                string chkStr = rData.Substring(2, rData.Length - 8);
                string chkSum = rData.Substring(rData.Length - 6, 4);
                if (chkSum != CalCheckSum(chkStr))
                {
                    er = CLanguage.Lan("数据CheckSum错误") + ":" + rData;
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
        /// <summary>
        /// 检验和-(地址+命令01+命令02+长度+数据)%256
        /// </summary>
        /// <param name="wCmd"></param>
        /// <returns></returns>
        private string CalCheckSum(string wCmd)
        {
            try
            {
                int sum = 0;
                
                for (int i = 0; i < wCmd.Length / 2; i++)
                    sum += System.Convert.ToInt16(wCmd.Substring(i * 2, 2), 16);
                
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
            catch (Exception)
            {                
                throw;
            }            
        }
        /// <summary>
        /// 检查通讯数据
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
                er = string.Empty;
                if (!ERS_CheckSum(wData, out er))
                    return false;
                string chkFlag = wData.Substring(4, 2);
                switch (chkFlag)
                {
                    case "F0":
                        rVal = wData.Substring(8, wData.Length - C_LEN * 2);
                        return true;
                    case "F1":
                        er = CLanguage.Lan("CHKSUM错误");
                        break;
                    case "F2":
                        er = CLanguage.Lan("LENGTH错误");
                        break;
                    case "F3":
                        er = CLanguage.Lan("CID无效");
                        break;
                    case "F4":
                        er = CLanguage.Lan("无效数据");
                        break;
                    default:
                        er = CLanguage.Lan("异常错误");
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
