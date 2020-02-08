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
    public class CDA_200_16:ILED
    {
        #region 构造
        public CDA_200_16(int idNo, string name)
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
        private int _maxCH = 16;
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
        public bool Open(string comName, out string er, string setting = "9600,N,8,1")
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
                string wCmd = CalDataFromELCmd(0, CID, INFO);
                string rData = string.Empty;
                int rLen = C_LEN;
                if (!SendCmdToELoad(wCmd, rLen, out rData, out er))
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
                string wCmd = CalDataFromELCmd(wAddr, CID, INFO);
                string rData = string.Empty;
                int rLen = C_LEN + 2;
                if (!SendCmdToELoad(wCmd, rLen, out rData, out er))
                    return false;
                string temp = string.Empty;
                string rVal = string.Empty;
                if (!ReturnResult(rData, out rVal, out  temp))
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
                
                string INFO = string.Empty;
                
                string wCmd = CalDataFromELCmd(wAddr, CID, INFO);
                
                string rData = string.Empty;
                
                int rLen = C_LEN + 96;
                
                if (!SendCmdToELoad(wCmd, rLen, out rData, out er))
                    return false;
                
                string temp = string.Empty;
                
                string rVal = string.Empty;
                
                if (!ReturnResult(rData, out rVal, out  temp))
                    return false;

                for (int i = 0; i < _maxCH; i++)
                {
                    int byte0 = System.Convert.ToInt16(rVal.Substring(i * 12, 2), 16);  //负载模式

                    int byte1 = System.Convert.ToInt16(rVal.Substring(i * 12 + 2, 4), 16);  //Von点

                    int byte2 = System.Convert.ToInt16(rVal.Substring(i * 12 + 6, 4), 16);  //负载值

                    int byte3 = System.Convert.ToInt16(rVal.Substring(i * 12 + 10, 2), 16); //模式附加参数

                    CLOAD para = new CLOAD();

                    para.Mode = (EMODE)byte0;

                    para.Von = (double)byte1 / 20;  //读值=Von*20(V) --->Von:0.5V-20V

                    switch (para.Mode)
                    {
                        case EMODE.CC_Slow:
                            para.load = (double)byte2 / 100; //CCSlow或CCFast或者MTK模式:读值=电流（A）*1000，取值范围0.1A～5A。
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
                            para.mark = (double)byte3;   /*当负载模式为MTK快充模式时，该参数为MKT快充电压类型。取值为1、2、3、4、5、6中的一个。
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
        /// 设置单个个电流通道负载
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

                if (chanPara.Von < 1)
                {
                    chanPara.Von = 3;
                }

                INFO += ((int)(chanPara.Von * 20)).ToString("X4"); //Von点-2Bytes

                switch (chanPara.Mode)
                {
                    case EMODE.CC_Slow:
                        INFO += ((int)(chanPara.load * 100)).ToString("X4"); //负载值:CCSlow或CCFast或者MTK模式时，设定值=电流（A）*1000，取值范围0.1A～5A
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
                        INFO += ((int)(chanPara.load * 100)).ToString("X4"); //负载值:CCSlow或CCFast或者MTK模式时，设定值=电流（A）*1000，取值范围0.1A～5A
                        INFO += ((int)(chanPara.mark * 10)).ToString("X2");   //模式附加参数:负载启动延迟时间。设定值=延迟时间（秒）*10，取值范围0～20秒
                        break;
                    case EMODE.LED:
                        INFO += ((int)(chanPara.load * 20)).ToString("X4"); //负载值:LED模式时，设定值=电压（V）*20，取值范围12V～400V
                        INFO += ((int)(chanPara.mark * 10)).ToString("X2");   //模式附加参数:当负载模式为LED模式时，该参数为LED电流。设定值=电流（A）*10，取值范围：0.1A～5A
                        break;
                    case EMODE.MTK:
                        INFO += ((int)(chanPara.load * 100)).ToString("X4"); //负载值:CCSlow或CCFast或者MTK模式时，设定值=电流（A）*1000，取值范围0.1A～5A
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

                string wCmd = CalDataFromELCmd(wAddr, CID, INFO);

                string rData = string.Empty;
                
                int rLen = C_LEN;
                
                if (!SendCmdToELoad(wCmd, rLen, out rData, out er))
                {
                    System.Threading.Thread.Sleep(100);

                   if (!SendCmdToELoad(wCmd, rLen, out rData, out er))
                      return false;
                }
                
                string temp = string.Empty;
                
                string rVal = string.Empty;
                
                if (!ReturnResult(rData, out rVal, out  temp))
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
        public bool SetLoadValue(int wAddr, List<CLOAD> chanList, bool saveEEPROM, out string er)
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

                    if (chanList[i].Von < 1)
                    {
                        chanList[i].Von = 3;
                    }

                    INFO += ((int)(chanList[i].Von * 20)).ToString("X4"); //Von点-2Bytes

                    switch (chanList[i].Mode)
                    {
                        case EMODE.CC_Slow:
                            INFO += ((int)(chanList[i].load * 100)).ToString("X4"); //负载值:CCSlow或CCFast或者MTK模式时，设定值=电流（A）*1000，取值范围0.1A～5A
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
                            INFO += ((int)(chanList[i].load * 100)).ToString("X4"); //负载值:CCSlow或CCFast或者MTK模式时，设定值=电流（A）*1000，取值范围0.1A～5A
                            INFO += ((int)(chanList[i].mark * 10)).ToString("X2");   //模式附加参数:负载启动延迟时间。设定值=延迟时间（秒）*10，取值范围0～20秒
                            break;
                        case EMODE.LED:
                            INFO += ((int)(chanList[i].load * 20)).ToString("X4"); //负载值:LED模式时，设定值=电压（V）*20，取值范围12V～400V
                            INFO += ((int)(chanList[i].mark * 10)).ToString("X2");   //模式附加参数:当负载模式为LED模式时，该参数为LED电流。设定值=电流（A）*10，取值范围：0.1A～5A
                            break;
                        case EMODE.MTK:
                            INFO += ((int)(chanList[i].load * 100)).ToString("X4"); //负载值:CCSlow或CCFast或者MTK模式时，设定值=电流（A）*1000，取值范围0.1A～5A
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
                string wCmd = CalDataFromELCmd(wAddr, CID, INFO);
                string rData = string.Empty;
                int rLen = C_LEN;
                if (!SendCmdToELoad(wCmd, rLen, out rData, out er))
                    return false;
                string temp = string.Empty;
                string rVal = string.Empty;
                if (!ReturnResult(rData, out rVal, out  temp))
                {
                    er = rData;
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

                string wCmd = CalDataFromELCmd(wAddr, CID, INFO);

                string rData = string.Empty;

                int rLen = C_LEN + 97;

                if (!SendCmdToELoad(wCmd, rLen, out rData, out er))
                    return false;

                string temp = string.Empty;

                string rVal = string.Empty;

                if (!ReturnResult(rData, out rVal, out  temp))
                    return false;

                for (int i = 0; i < _maxCH; i++)
                {
                    int byte0 = System.Convert.ToInt16(rVal.Substring(i * 12, 4), 16);  //1-2Bytes:电压值

                    int byte1 = System.Convert.ToInt16(rVal.Substring(i * 12 + 4, 4), 16); //3-4:电流值

                    int byte2 = System.Convert.ToInt16(rVal.Substring(i * 12 + 8, 4), 16); //5-6状态信息

                    double vol = (double)byte0 / 20;

                    double curr = (double)byte1 / 100;

                    data.chan[i].volt = vol;

                    data.chan[i].current = curr;

                    data.chan[i].alarmInfo = string.Empty;

                    //通道状态信息

                    //bit0
                    if ((byte2 & (1 << 0)) == (1 << 0))
                        data.chan[i].ch_status.DC_DC = EDC.ON;
                    else
                        data.chan[i].ch_status.DC_DC = EDC.OFF;

                    //bit1
                    if ((byte2 & (1 << 1)) == (1 << 1))
                    {
                        data.chan[i].ch_status.LLC_AC = ELLC.OVP;
                        data.chan[i].alarmInfo += "OVP;";
                    }
                    else
                    {
                        data.chan[i].ch_status.LLC_AC = ELLC.正常;
                    }

                    //bit2-bit3
                    int bit2_3 = (byte2 & 0xC) / 0x4;

                    if (bit2_3 == 0)
                    {
                        data.chan[i].ch_status.AC_STATUS = EAC.欠压;
                        data.chan[i].alarmInfo += CLanguage.Lan("输入电压欠压") + ";";
                    }
                    if (bit2_3 == 0x1)
                    {
                        data.chan[i].ch_status.AC_STATUS = EAC.正常;
                    }
                    else if (bit2_3 == 0x10)
                    {
                        data.chan[i].ch_status.AC_STATUS = EAC.过压;
                        data.chan[i].alarmInfo += CLanguage.Lan("输入电压过压") + ";";
                    }

                    //bit4
                    if ((byte2 & (1 << 4)) == (1 << 4))
                    {
                        data.chan[i].ch_status.OPP = EOPP.OPP;
                        data.chan[i].alarmInfo += "OPP;";
                    }
                    else
                    {
                        data.chan[i].ch_status.OPP = EOPP.正常;
                    }

                    //bit5
                    if ((byte2 & (1 << 5)) == (1 << 5))
                    {
                        data.chan[i].ch_status.OTP = EOTP.OTP;
                        data.chan[i].alarmInfo += "OTP;";
                    }
                    else
                    {
                        data.chan[i].ch_status.OTP = EOTP.正常;
                    }

                    //bit6-bit7
                    int bit6_7 = (byte2 & 0xC0) / 0x40;

                    if (bit6_7 == 0)
                    {
                        data.chan[i].ch_status.VBus = EVBus.欠压;
                        data.chan[i].alarmInfo += CLanguage.Lan("直流母线电压欠压") + ";";
                    }
                    if (bit6_7 == 0x1)
                    {
                        data.chan[i].ch_status.VBus = EVBus.正常;
                    }
                    else if (bit6_7 == 0x10)
                    {
                        data.chan[i].ch_status.VBus = EVBus.过压;
                        data.chan[i].alarmInfo += CLanguage.Lan("直流母线电压过压") + ";";
                    }

                    //bit8
                    if ((byte2 & (1 << 8)) == (1 << 8))
                    {
                        data.chan[i].ch_status.maxCur = EMaxCUR.过流;
                        data.chan[i].alarmInfo += CLanguage.Lan("大电流通道过流") + ";";
                    }
                    else
                    {
                        data.chan[i].ch_status.maxCur = EMaxCUR.正常;
                    }

                    //bit9
                    if ((byte2 & (1 << 9)) == (1 << 9))
                    {
                        data.chan[i].ch_status.minCur = EMinCUR.过流;
                        data.chan[i].alarmInfo += CLanguage.Lan("小电流通道过流") + ";";
                    }
                    else
                    {
                        data.chan[i].ch_status.minCur = EMinCUR.正常;
                    }

                }

                data.alarmInfo = string.Empty;

                string strInvStatus = Convert.ToString(Convert.ToInt16(rData.Substring(96 * 2, 2), 16), 2);

                strInvStatus = strInvStatus.PadLeft(8, '0');

                CINVSTAT inverStatus = new CINVSTAT
                {
                    OTP = strInvStatus.Substring(0, 1) == "1" ? EINVOTP.OTP : EINVOTP.正常,
                    AD = strInvStatus.Substring(1, 1) == "1" ? EINVAD.故障 : EINVAD.正常
                };

                if (inverStatus.OTP != EINVOTP.正常)
                {
                    data.alarmInfo += "OTP;";
                }
                if (inverStatus.AD != EINVAD.正常)
                {
                    data.alarmInfo += "AD;";
                }

                if (strInvStatus.Substring(2, 2) == "00")
                {
                    inverStatus.ACBus = EINVACBus.欠压;
                    data.alarmInfo += "欠压1;";
                }
                else if (strInvStatus.Substring(2, 2) == "10")
                {
                    inverStatus.ACBus = EINVACBus.过压;
                    data.alarmInfo += "过压2;";
                }
                else
                {
                    inverStatus.ACBus = EINVACBus.正常;
                }

                inverStatus.Fan = strInvStatus.Substring(4, 1) == "1" ? EINVFan.故障 : EINVFan.正常;

                inverStatus.OTP = strInvStatus.Substring(5, 1) == "1" ? EINVOTP.OTP : EINVOTP.正常;

                if (inverStatus.Fan != EINVFan.正常)
                {
                    data.alarmInfo += "Fan;";
                }
                if (inverStatus.OTP != EINVOTP.正常)
                {
                    data.alarmInfo += "OTP;";
                }

                if (strInvStatus.Substring(6, 2) == "00")
                {
                    inverStatus.DCBus = EINVDCBus.欠压;
                    data.alarmInfo += "欠压1;";
                }
                else if (strInvStatus.Substring(6, 2) == "10")
                {
                    inverStatus.DCBus = EINVDCBus.过压;
                    data.alarmInfo += "过压2;";
                }
                else
                {
                    inverStatus.DCBus = EINVDCBus.正常;
                }

                if (data.alarmInfo == string.Empty)
                {
                    data.alarmInfo = "正常";
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

        #region 协议
        /*
         * 发送:桢头+地址+命令+长度+数据+校验和+桢尾
         * SOI+ADR+CID+LENGTH+INFO+CHKSUM+EOI
         * SOI = 0xEE
         * EOI = 0xEF
         * if(CHKSUM == SOI || CHKSUM == EOI) CHKSUM = 0xED
         * 命令
         * 
        */

        private string SOI = "EE";
        private string EOI = "EF";
        private string ROI = "ED";
        /// <summary>
        /// 协议格式
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="wCmd"></param>
        /// <param name="wData"></param>
        /// <returns></returns>
        private string CalDataFromELCmd(int wAddr, string wCmd, string wData)
        {
            try
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
                cmd = SOI + cmd + CheckSum(cmd) + EOI;
                return cmd;
            }
            catch (Exception)
            {
                throw;
            }

        }
        private string CheckSum(string strHex)
        {
            try
            {
                int sum = 0;

                for (int i = 0; i < strHex.Length / 2; i++)
                    sum += System.Convert.ToInt16(strHex.Substring(i * 2, 2), 16);

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
        /// 检验和
        /// </summary>
        /// <param name="wData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool EL_CheckSum(string wData, out string er)
        {
            er = string.Empty;

            try
            {
                if (wData.Substring(0, 2) != SOI)
                {
                    if (CLanguage.languageType == CLanguage.EL.中文) er = "数据帧头错误:" + wData;
                    else er = "Data frame header error:" + wData;
                    return false;
                }

                if (wData.Substring(wData.Length - 2, 2) != EOI)
                {
                    if (CLanguage.languageType == CLanguage.EL.中文) er = "数据帧尾错误:" + wData;
                    else er = "Data frame end error:" + wData;
                    return false;
                }

                string recChk = wData.Substring(wData.Length - 6, 4);
                string chkSum = CheckSum(wData.Substring(2, wData.Length - 8));
                if (recChk != chkSum)
                {
                    if (CLanguage.languageType == CLanguage.EL.中文) er = "校验和错误:" + wData;
                    else er = "checksum error:" + wData;
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
        private bool ReturnResult(string rData, out string rVal, out string err)
        {
            rVal = string.Empty;
            err = string.Empty;
            if (!EL_CheckSum(rData, out err))
                return false;
            string chkFlag = rData.Substring(4, 2);
            switch (chkFlag)
            {
                case "F0":
                    rVal = rData.Substring(8, rData.Length - C_LEN * 2);
                    err = string.Empty;
                    return true;
                case "F1":
                    err = "校验和错误";
                    return false;
                case "F2":
                    err = "长度错误";
                    return false;
                case "F3":
                    err = "命令错误";
                    return false;
                case "F4":
                    err = "命令错误";
                    return false;
                default:
                    if (rData.Length > 2)
                    {
                        err = string.Empty;
                        return true;
                    }
                    else
                    {
                        err = "不明错误," + rData;
                        return false;
                    }
            }
        }
        /// <summary>
        /// 发送和接收数据
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="wData"></param>
        /// <param name="rLen"></param>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <param name="wTimeOut"></param>
        /// <returns></returns>
        private bool SendCmdToELoad(string wData, int rLen, out string rData, out string er, int wTimeOut = 300)
        {
            rData = string.Empty;

            er = string.Empty;
            string rVal = string.Empty;
            try
            {
                string recvData = string.Empty;

                if (!com.send(wData, rLen, out recvData, out er, wTimeOut))
                    return false;
                //if (rLen > 0)
                //{
                //    if (!ReturnResult(recvData, out rVal, out er))
                //        return false;
                //    rData = rVal;
                //}
                rData = recvData;
                return true;
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
