using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ;
using GJ.DEV.COM;

namespace GJ.DEV.HIPOT
{
    public class CExtech7100 : IHP
    {
        #region 通信指令

        private const int endCode = 0x0A;

        #region 基本指令
        /// <summary>
        /// 执行测试
        /// </summary>
        private const string test = "TEST";
        /// <summary>
        /// 停止测试
        /// </summary>
        private const string reset = "RESET";
        /// <summary>
        /// 设定MEMORY(1-8)与STEP(1-3)
        /// </summary>
        private const string T_newMemoryAndStep = "MSS";
        /// <summary>
        /// 设定ACW
        /// </summary>
        private const string newACW = "ACW";
        /// <summary>
        /// 设定DCW
        /// </summary>
        private const string newDCW = "DCW";
        /// <summary>
        /// 设定IR
        /// </summary>
        private const string newIR = "IR";
        #endregion

        #region 参数设定
        /// <summary>
        /// 设定电压(ACW:0.00-5.00;DCW:0.00-6.00;IR:30-1000)
        /// </summary>
        private const string T_voltage = "VOLT";
        /// <summary>
        /// 设定上限(ACW:0.00-20.00;DCW:0-7500;IR:0-9999)
        /// </summary>
        private const string T_hi_limit = "MAXL";
        /// <summary>
        /// 设定下限(ACW:0.00-20.00;DCW:0-7500;IR:0-9999)
        /// </summary>
        private const string T_lo_limit = "MINL";
        /// <summary>
        /// 设定缓升时间(0.1-999.9)
        /// </summary>
        private const string T_ramp_up_time = "RUP";
        /// <summary>
        /// 设定AC,DC测试时间和IR延时时间(ACW:0.3-999.9;DCW:0.4-999.9;IR:0.5-999.9)
        /// </summary>
        private const string T_dwell_time = "DDT";
        /// <summary>
        /// 设定缓降时间(ACW:0-999.9;DCW:1.0-999.9;IR:1.0-999.9)
        /// </summary>
        private const string T_ramp_down_time = "RDN";
        /// <summary>
        /// 设定ARC灵敏度(0-9:ACW,DCW)
        /// </summary>
        private const string T_arc = "ARC";
        /// <summary>
        /// 设置频率(50 or 60 ACW)
        /// </summary>
        private const string T_hz = "FREQ";
        /// <summary>
        /// 设定步骤连接(1=ON,0=OFF)
        /// </summary>
        private const string T_connect = "CONN";

        #endregion

        #region 系统设定
        /// <summary>
        /// 设定PLC遥控功能(1=ON,0=OFF)
        /// </summary>
        private const string T_setPLC = "PLC";
        /// <summary>
        /// 设定单一步骤测试(1=ON,0=OFF)
        /// </summary>
        private const string T_setStep = "SSTP";
        /// <summary>
        /// 设定音量(0-9)
        /// </summary>
        private const string T_setVolume = "ALAR";
        /// <summary>
        /// 设定背光(0-9)
        /// </summary>
        private const string t_setLight = "CNTR";
        /// <summary>
        /// 设定显示结果(L=Last,A=All,P=P/F)
        /// </summary>
        private const string setShowResult = "RLT";
        /// <summary>
        /// 设定SMART GFI(1=ON,0=OFF)
        /// </summary>
        private const string setSmart_GFI = "SGFI";
        #endregion

        #region 查询指令
        /// <summary>
        /// (TD?)MX-X_,FUNC,STATUS,V,A(R),T
        /// </summary>
        private const string get_nowStatus = "TD";
        /// <summary>
        /// (RD n?)MX-X_,FUNC,STATUS,V,A(R),T
        /// </summary>
        private const string get_SingleStatus = "RD";
        /// <summary>
        /// (SALL?)SALL nn,nn
        /// </summary>
        private const string get_Sall = "SALL";
        /// <summary>
        /// (RR?)n(0=CLOSE,1=OPEN)
        /// </summary>
        private const string get_RR = "RR";
        /// <summary>
        /// (RI?)n(0=CLOSE,1=OPEN)
        /// </summary>
        private const string get_RL = "RL";
        #endregion

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="idNo"></param>
        /// <param name="name"></param>
        /// <param name="chanNum"></param>
        /// <param name="path"></param>
        public CExtech7100(int idNo = 0, string name = "CExtech7400")
        {
            this._idNo = idNo;
            this._name = name;
            this._failStop = "0";
            com = new COM.CSerialPort();
        }

        #region 字段
        /// <summary>
        /// 设备名称
        /// </summary>
        private string _name = "CExtech7400";
        /// <summary>
        /// 设备编号
        /// </summary>
        private int _idNo = 0;
        /// <summary>
        /// 设备状态
        /// </summary>
        private bool _conStatus = false;
        /// <summary>
        /// 通道数量
        /// </summary>
        private int _chanNum = 1;
        /// <summary>
        /// 测试步骤数量
        /// </summary>
        private int _stepNum = 1;
        /// <summary>
        /// 产品测试数量
        /// </summary>
        private int _uutMax = 8;
        /// <summary>
        /// 设置失败停止
        /// </summary>
        private string _failStop = "0";
        /// <summary>
        /// Memory编号
        /// </summary>
        private string _memoryNum = "1";
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
        /// <summary>
        /// 字节长度
        /// </summary>
        public int chanMax
        {
            get { return _chanNum; }
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

                com = new CSerialPort(_idNo, _name, EDataType.ASCII格式);

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
        /// 初始化设备
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Init(out string er, int uutMax, int stepNum)
        {
            er = string.Empty;

            _uutMax = uutMax;

            _stepNum = stepNum; 

            try
            {
                string devName = string.Empty;

                string rData = string.Empty;

                if (!writeCmd(reset, out er))
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
        /// 导入程序
        /// </summary>
        /// <param name="proName"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ImportProgram(string proName, out string er)
        {
            er = string.Empty;

            try
            {
                if (!writeCmd(T_newMemoryAndStep + " " + proName + "1", out er))
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
        /// 获取测试步骤
        /// </summary>
        /// <param name="stepName"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadStepName(out List<EStepName> stepName, out string er, int chan = 1)
        {
            stepName = new List<EStepName>();

            try
            {
                er = "不支持读测试步骤";
                return false;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 读取测试步骤设置值
        /// </summary>
        /// <param name="stepNo"></param>
        /// <param name="rStepVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadStepSetting(int stepNo, out EStepName stepName, out List<double> stepVal, out string er)
        {
            stepName = EStepName.AC;

            stepVal = new List<double>();

            er = string.Empty;

            try
            {
                er = "不支持读设定步骤";
                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 设置高压通道
        /// </summary>
        /// <param name="chanList"></param>
        /// <returns></returns>
        public bool SetChanEnable(List<int> chanList, out string er)
        {
            er = string.Empty;

            try
            {
                er = "只有一个通道无需设置";
                return false;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 设置测试步骤
        /// </summary>
        /// <param name="step"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SetTestPara(List<CHPPara.CStep> step, out string er, string proName, bool saveToDev)
        {
            er = string.Empty;

            try
            {
                string[] pro = proName.Split(',');
                string memory = pro[0];
                string smartGFI = pro[1];
                string cmdMemory = T_newMemoryAndStep + " " + memory;
                string cmd = string.Empty;
                string rData = string.Empty;
                _stepNum = step.Count;
                _memoryNum = memory;
                //写入接地中断停止设置SMART GFI
                if (!writeCmd(setSmart_GFI + " " + smartGFI, out er))
                    return false;
                System.Threading.Thread.Sleep(110);
                //单一步骤设置OFF
                if (!writeCmd(T_setStep + " 0", out er))
                    return false;
                System.Threading.Thread.Sleep(110);

                for (int i = 0; i < step.Count; i++)
                {
                    string stepNo = (step[i].stepNo + 1).ToString();

                    if (!writeCmd(cmdMemory + stepNo, out er))
                        return false;
                    System.Threading.Thread.Sleep(110);
                    switch (step[i].name)
                    {
                        case EStepName.AC:
                            if (!writeCmd(newACW, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //VOLTAGE
                            cmd = T_voltage + " " + ((double)(step[i].para[0].setVal)).ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //HIGHT LIMIT
                            cmd = T_hi_limit + " " + ((step[i].para[1].setVal)).ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //LOW LIMIT
                            cmd = T_lo_limit + " " + ((step[i].para[2].setVal)).ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //RAMP UP TIME 
                            cmd = T_ramp_up_time + " " + step[i].para[3].setVal.ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //DWELL TIME           
                            cmd = T_dwell_time + " " + step[i].para[4].setVal.ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //RAMP DOWN TIME          
                            cmd = T_ramp_down_time + " " + step[i].para[5].setVal.ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //电弧灵敏度
                            for (int lv = 1; lv < 10; lv++)
                            {
                                if (step[i].para[6].setVal.ToString() == lv.ToString())
                                {
                                    cmd = T_arc + " " + lv.ToString();
                                    break;
                                }
                            }
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //FREQUENCY
                            if (step[i].para[7].setVal.ToString() == "0")
                                cmd = T_hz + " 60";
                            else
                                cmd = T_hz + " 50";
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            break;
                        case EStepName.DC:
                            if (!writeCmd(newDCW, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //VOLTAGE
                            cmd = T_voltage + " " + ((double)(step[i].para[0].setVal)).ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //HIGHT LIMIT
                            cmd = T_hi_limit + " " + ((step[i].para[1].setVal)).ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //LOW LIMIT
                            cmd = T_lo_limit + " " + ((step[i].para[2].setVal)).ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //RAMP UP TIME 
                            cmd = T_ramp_up_time + " " + step[i].para[3].setVal.ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //DWELL TIME     
                            cmd = T_dwell_time + " " + step[i].para[4].setVal.ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //RAMP DOWN TIME 
                            cmd = T_ramp_down_time + " " + step[i].para[5].setVal.ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //ARC LIMIT
                            for (int lv = 1; lv < 10; lv++)
                            {
                                if (step[i].para[6].setVal.ToString() == lv.ToString())
                                {
                                    cmd = T_arc + " " + lv.ToString();
                                    break;
                                }
                            }
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            break;
                        case EStepName.IR:
                            if (!writeCmd(newIR, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //VOLTAGE
                            cmd = T_voltage + " " + ((double)(step[i].para[0].setVal)).ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //HIGHT LIMIT
                            cmd = T_hi_limit + " " + ((double)(step[i].para[1].setVal)).ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //LOW LIMIT
                            cmd = T_lo_limit + " " + ((double)(step[i].para[2].setVal)).ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //RAMP UP TIME 
                            cmd = T_ramp_up_time + " " + step[i].para[3].setVal.ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //DWELL TIME           
                            cmd = T_dwell_time + " " + step[i].para[4].setVal.ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            //RAMP DOWN TIME          
                            cmd = T_ramp_down_time + " " + step[i].para[5].setVal.ToString();
                            if (!writeCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);
                            break;
                        case EStepName.OSC:
                            break;
                        case EStepName.GB:
                            break;
                        case EStepName.PA:
                            break;
                        default:
                            break;
                    }

                    if (i == step.Count - 1)
                    {
                        if (!writeCmd(T_connect + " 0", out er))
                            return false;
                    }
                    else
                    {
                        if (!writeCmd(T_connect + " 1", out er))
                            return false;
                    }
                    System.Threading.Thread.Sleep(110);
                }
                return writeCmd(cmdMemory + "1", out er);
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 启动测试
        /// </summary>
        public bool Start(out string er)
        {

            er = string.Empty;

            try
            {
                if (_failStop == "1")
                {
                    if (!writeCmd(reset, out er))
                        return false;
                }
                statusStep = 1;
                return writeCmd(test, out er);
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }            
        }
        /// <summary>
        /// 停止测试
        /// </summary>
        public bool Stop(out string er)
        {
            return writeCmd(reset, out er);
        }
        /// <summary>
        /// 读状态已pass步骤
        /// </summary>
        private int statusStep = 1;
        /// <summary>
        /// 读取测试状态
        /// </summary>
        /// <param name="status"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadStatus(out EHPStatus status, out string er)
        {
            status = EHPStatus.STOPPED;

            er = string.Empty;

            try
            {

                string rData = string.Empty;
                string cmd = get_SingleStatus + " " + statusStep.ToString() + "?";
                if (!sendCmdToHP(cmd, out rData, out er))
                    status = EHPStatus.RUNNING;
                else
                    status = EHPStatus.STOPPED;             
                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 读取测试结果
        /// </summary>
        /// <param name="uutMax"></param>
        /// <param name="stepMax"></param>
        /// <param name="uut"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadResult(int uutMax, int stepMax, out List<CCHResult> uut, out string er)
        {
            er = string.Empty;

            uut = new List<CCHResult>();

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
        /// <summary>
        /// 读取测试结果
        /// </summary>
        /// <param name="chan"></param>
        /// <param name="chanResult"></param>
        /// <param name="stepResult"></param>
        /// <param name="stepCode"></param>
        /// <param name="stepMode"></param>
        /// <param name="stepVal"></param>
        /// <param name="stepUnit"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadResult(int chan, out int chanResult,
                               out List<int> stepResult, out List<string> stepCode,
                               out List<EStepName> stepMode, out List<double> stepVal,
                               out List<string> stepUnit, out string er)
        {

            chanResult = 0;

            stepResult = new List<int>();

            stepCode = new List<string>();

            stepMode = new List<EStepName>();

            stepVal = new List<double>();

            stepUnit = new List<string>();


            er = string.Empty;

            try
            {
                int xxx = System.Environment.TickCount;
                //获取测试结果
                string rData = string.Empty;

                for (int i = 0; i < _stepNum; i++)
                {
                    if (!sendCmdToHP(get_SingleStatus + " " + (i + 1).ToString() + "?", out rData, out er))
                        return false;
                    if (rData.Length <20)
                    {
                        er = "读取单步测试结果返回值长度不够";
                        return false;
                    }
                    string[] result = rData.Split(',');

                    if (result[2].ToUpper() == "PASS")
                        stepResult.Add(0);
                    else
                        stepResult.Add(1);

                    switch (result[1])
                    {
                        case "ACW":
                            stepMode.Add(EStepName.AC);
                            stepVal.Add(Convert.ToDouble(result[3].Substring(0, result[3].Length - 2)));
                            stepUnit.Add("kV");
                            stepVal.Add(Convert.ToDouble(result[4].Substring(0, result[4].Length - 2)));
                            stepUnit.Add("mA");
                            stepCode.Add(result[2]);
                            break;
                        case "DCW":
                            stepMode.Add(EStepName.DC);
                            stepVal.Add(Convert.ToDouble(result[3].Substring(0, result[3].Length - 2)));
                            stepUnit.Add("kV");
                            stepVal.Add(Convert.ToDouble(result[4].Substring(0, result[4].Length - 2)));
                            stepUnit.Add("uA");
                            stepCode.Add(result[2]);
                            break;
                        case "IR":
                            stepMode.Add(EStepName.IR);
                            if (result[3].Substring(0, result[3].Length - 1) == "----")
                                stepVal.Add(-1);
                            else 
                                stepVal.Add(Convert.ToDouble(result[3].Substring(0, result[3].Length - 1)));
                            stepUnit.Add("V");
                            if (result[4][0] == '>')
                                stepVal.Add(-2);
                            else if (result[4][0] == '<')
                                stepVal.Add(-3);
                            else if (result[4].Substring(0, result[4].Length - 1) == "----")
                                stepVal.Add(-1);
                            else
                                stepVal.Add(Convert.ToDouble(result[4].Substring(0, result[4].Length - 1)));
                            stepUnit.Add("MΩ");
                            stepCode.Add(result[2]);
                            break;
                        default:
                            stepMode.Add(EStepName.PA);
                            stepVal.Add(0);
                            stepUnit.Add("null");
                            stepVal.Add(0);
                            stepUnit.Add("null");
                            stepCode.Add("null");
                            break;
                    }
                    if (result[2].ToUpper() != "PASS")
                        break;  
                }
                chanResult = 0;
                for (int i = 0; i < stepResult.Count; i++)
                {
                    if (stepResult[i] != 0)
                    {
                        chanResult = 1;
                        break;
                    }
                }
                int b = System.Environment.TickCount - xxx;
                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 写入命令
        /// </summary>
        /// <param name="wCmd"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool WriteCmd(string wCmd, out string er)
        {
            er = string.Empty;

            try
            {
                if (!writeCmd(wCmd, out er))
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
        /// 读取命令
        /// </summary>
        /// <param name="wCmd"></param>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadCmd(string wCmd, out string rData, out string er)
        {
            er = string.Empty;

            rData = string.Empty;

            try
            {
                if (!sendCmdToHP(wCmd, out rData, out er))
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

        #region 仪器通信
        /// <summary>
        /// 读取仪器设备
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readIDN(out string devName, out string er)
        {
            devName = string.Empty;

            er = string.Empty;

            try
            {
                string cmd = "SDN?";

                string rData = string.Empty;

                if (!sendCmdToHP(cmd, out rData, out er))
                    return false;
                string[] valList = rData.Split(',');
                if (valList.Length < 2)
                    return false;
                devName = valList[0] + valList[1];
                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 读错误信息
        /// </summary>
        /// <param name="errCode"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readError(out string errCode, out string er)
        {
            errCode = string.Empty;

            er = string.Empty;


            try
            {
                string cmd = "SYST:ERR?";

                string rData = string.Empty;

                if (!sendCmdToHP(cmd, out rData, out er))
                    return false;

                string[] codeList = rData.Split(',');

                int code = System.Convert.ToInt32(codeList[0]);

                if (code == 0)
                    errCode = "OK";
                else
                    errCode = GJ.COM.CLanguage.Lan("错误信息:") + codeList[1].Replace("\"", "") + "-" + code.ToString();

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }

        }
        /// <summary>
        /// 远程控制
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool remote(out string er)
        {
            er = string.Empty;

            try
            {
                string cmd = "SPR?";

                string rData = string.Empty;

                if (!sendCmdToHP(cmd, out rData, out er))
                    return false;
                if (rData != "1")
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
        /// 保存机种文件到高压机种
        /// </summary>
        /// <param name="proName"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool saveProgram(string proName, out string er)
        {
            er = string.Empty;

            try
            {
                if (proName == string.Empty)
                    return true;

                er = string.Empty;

                string cmd = "MEM:STAT:DEF? \"" + proName + "\"";

                string rData = string.Empty;

                int proNo = 0;

                if (sendCmdToHP(cmd, out rData, out er)) //程序名存在
                {
                    proNo = System.Convert.ToInt16(rData);

                    if (!writeCmd("*SAV " + proNo, out er))
                        return false;

                    if (!writeCmd("MEM:STAT:DEF \"" + proName + "\"," + proNo, out er))
                        return false;

                }
                else
                {
                    //查询存储空间
                    cmd = "MEM:NST?";
                    if (!sendCmdToHP(cmd, out rData, out er))
                        return false;
                    int maxProNo = System.Convert.ToInt16(rData);

                    //查询可用位置
                    cmd = "MEM:FREE:STAT?";
                    if (!sendCmdToHP(cmd, out rData, out er))
                        return false;
                    int proIndex = System.Convert.ToInt16(rData);

                    proNo = maxProNo - proIndex;

                    if (proNo == 0)
                    {
                        er = GJ.COM.CLanguage.Lan("高压机无可用存储程序,请删除多余程序");
                        return false;
                    }

                    writeCmd("*SAV " + proNo, out er);

                    writeCmd("MEM:STAT:DEF \"" + proName + "\"," + proNo, out er);
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
        /// 写命令
        /// </summary>
        /// <param name="wCmd"></param>
        /// <param name="delayMs"></param>
        private bool writeCmd(string wCmd, out string er, int delayMs = 5, bool readResult = false, int timeOutMs = 500)
        {
            string rData = string.Empty;

            er = string.Empty;

            try
            {
                if (!sendCmdToHP(wCmd, out rData, out er))
                    return false;
                if (!readResult)
                {
                    if (rData != Encoding.ASCII.GetString(new Byte[1] { 0x6 }))
                    {
                        er = "返回数据错误,发送数据:" + wCmd + ",回读数据:" + rData;
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
        /// 发送和接收数据
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="wData"></param>
        /// <param name="rLen"></param>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <param name="wTimeOut"></param>
        /// <returns></returns>
        private bool sendCmdToHP(string wData, out string rData, out string er, string rEOI = "\n", int wTimeOut = 1000)
        {
            rData = string.Empty;

            er = string.Empty;

            try
            {
                string recvData = string.Empty;
                wData += "\n";
                if (!com.send(wData, rEOI, out rData, out er, wTimeOut))
                    return false;
                rData = rData.Replace("\r", "");
                rData = rData.Replace("\n", "");
                if (rData == Encoding.ASCII.GetString(new Byte[1] { 0x15 }))
                {
                    er = "传输指令字符串错误:" + wData.Replace("\n", "");
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
        #endregion
    }
}
