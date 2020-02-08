using System;
using System.Collections.Generic;
using System.Text;
using GJ.DEV.COM;
using GJ.DEV.HIPOT;

namespace GJ.Device.Hipot
{
    public class CExtech7440 : IHP
    {
        #region 通信指令

        private const int endCode = 0x0A;

        #region 基本指令
        /// <summary>
        /// 执行测试
        /// </summary>
        private const string test = "FA";
        /// <summary>
        /// 停止测试
        /// </summary>
        private const string reset = "FB";
        /// <summary>
        /// 设定ACW
        /// </summary>
        private const string newACW = "FC";
        /// <summary>
        /// 设定DCW
        /// </summary>
        private const string newDCW = "FD";
        /// <summary>
        /// 设定IR
        /// </summary>
        private const string newIR = "FE";


        private const string newGB = "FF";

        #endregion

        #region 参数设定

        #region ACW

        /// <summary>
        /// 设定电压(ACW:0-5000)
        /// </summary>
        private const string T_voltage_acw = "SA";
        /// <summary>
        /// 设定上限(ACW:0.00-40.00)
        /// </summary>
        private const string T_hi_limit_acw = "SB";
        /// <summary>
        /// 设定下限(ACW:0.000-9.999)
        /// </summary>
        private const string T_lo_limit_acw = "SC";
        /// <summary>
        /// 设定缓升时间(0.1-999.9)
        /// </summary>
        private const string T_ramp_up_time_acw = "SD";
        /// <summary>
        /// 设定测试时间(ACW:0,0.3-999.9)
        /// </summary>
        private const string T_dwell_time_acw = "SE";
        /// <summary>
        /// 设定ARC灵敏度(ACW:1-9)
        /// </summary>
        private const string T_arc_acw = "SF";
        /// <summary>
        /// 设定通道(ACW:H,L,O)
        /// </summary>
        private const string T_channel_acw = "SG";
        /// <summary>
        /// 设定AC补偿(ACW:0-3.500)
        /// </summary>
        private const string T_offset_acw = "SH";
        /// <summary>
        /// 设置频率(ACW:60)
        /// </summary>
        private const string T_60hz_acw = "FI";
        /// <summary>
        /// 设置频率(ACW:50)
        /// </summary>
        private const string T_50hz_acw = "FJ";
        /// <summary>
        /// 开启ARC
        /// </summary>
        private const string T_arc_on_acw = "FK";
        /// <summary>
        /// 关闭ARC
        /// </summary>
        private const string T_arc_off_acw = "FL";
        /// <summary>
        /// 设定AC自动补偿
        /// </summary>
        private const string T_auto_offser_acw = "FX";

        #endregion

        #region DCW

        /// <summary>
        /// 设定电压(DCW:0-6000)
        /// </summary>
        private const string T_voltage_dcw = "SI";
        /// <summary>
        /// 设定上限(DCW:0-3500)
        /// </summary>
        private const string T_hi_limit_dcw = "SJ";
        /// <summary>
        /// 设定下限(DCW:0.00-999.9)
        /// </summary>
        private const string T_lo_limit_dcw = "SK";
        /// <summary>
        /// 设定缓升时间(0.4-999.9)
        /// </summary>
        private const string T_ramp_up_time_dcw = "SL";
        /// <summary>
        /// 设定测试时间(DCW:0,0.3-999.9)
        /// </summary>
        private const string T_dwell_time_dcw = "SM";
        /// <summary>
        /// 设定最低充电流(DCW:0.0-350.0)
        /// </summary>
        private const string T_charge_lo_dcw = "SO";
        /// <summary>
        /// 设定ARC灵敏度(DCW:0-9)
        /// </summary>
        private const string T_arc_dcw = "SP";
        /// <summary>
        /// 设定通道(DCW:H,L,O)
        /// </summary>
        private const string T_channel_dcw = "SQ";
        /// <summary>
        /// 设定AC补偿(DCW:0.0-350.0)
        /// </summary>
        private const string T_offset_dcw = "SR";
        /// <summary>
        /// 开启ARC
        /// </summary>
        private const string T_arc_on_dcw = "FM";
        /// <summary>
        /// 关闭ARC
        /// </summary>
        private const string T_arc_off_dcw = "FN";
        /// <summary>
        /// 设定DCW自动最低充电流
        /// </summary>
        private const string T_auto_charge_lo_dcw = "FV";
        /// <summary>
        /// 设定DC自动补偿
        /// </summary>
        private const string T_auto_offset_dcw = "FY";
        /// <summary>
        /// 设定开启缓冲电流
        /// </summary>
        private const string T_ramp_hi_on_dcw = "F8";
        /// <summary>
        /// 设定关闭缓冲电流
        /// </summary>
        private const string T_ramp_hi_off_dcw = "F9";

        #endregion

        #region IR

        /// <summary>
        /// 设定电压(IR:100-1000)
        /// </summary>
        private const string T_voltage_ir = "SS";
        /// <summary>
        /// 设定最低充电流(IR:0.000-9.999)
        /// </summary>
        private const string T_charge_lo_ir = "ST";
        /// <summary>
        /// 设定上限(IR:0-9999)
        /// </summary>
        private const string T_hi_limit_ir = "SU";
        /// <summary>
        /// 设定下限(IR:1-9999)
        /// </summary>
        private const string T_lo_limit_ir = "SV";
        /// <summary>
        /// 设定缓升时间(0,0.5-999.9)
        /// </summary>
        private const string T_delay_time_ir = "SL";
        /// <summary>
        /// 设定通道(IR:H,L,O)
        /// </summary>
        private const string T_channel_ir = "SX";
        /// <summary>
        /// 设定IR自动最低充电流
        /// </summary>
        private const string T_auto_charge_lo_ir = "FW";

        #endregion

        #region GB

        /// <summary>
        /// 设定电流(GB:3.00-30.00)
        /// </summary>
        private const string T_current_gb = "SY";
        /// <summary>
        /// 设定电压(GB:3-8)
        /// </summary>
        private const string T_voltage_gb = "SZ";
        /// <summary>
        /// 设定上限(GB:0-600)
        /// </summary>
        private const string T_hi_limit_gb = "S0";
        /// <summary>
        /// 设定测试时间(DCW:0.5-999.9)
        /// </summary>
        private const string T_dwell_time_gb = "S2";
        /// <summary>
        /// 设定通道(GB:1-16)
        /// </summary>
        private const string T_channel_gb = "S3";
        /// <summary>
        /// 设定补偿(DCW:0-200)
        /// </summary>
        private const string T_offset_gb = "S4";
        /// <summary>
        /// 设置频率(GB:60)
        /// </summary>
        private const string T_60hz_gb = "FO";
        /// <summary>
        /// 设置频率(GB:50)
        /// </summary>
        private const string T_50hz_gb = "FP";
        /// <summary>
        /// 设定GB自动补偿
        /// </summary>
        private const string T_auto_offset_gb = "FU";

        #endregion

        #region 共用指令

        /// <summary>
        /// 设定MEMORY(1-50)
        /// </summary>
        private const string T_newMemory = "S5";
        /// <summary>
        /// 设定Step(1-8)
        /// </summary>
        private const string T_newStep = "S6";
        /// <summary>
        /// 开启步骤连接
        /// </summary>
        private const string T_connect_on = "FQ";
        /// <summary>
        /// 关闭步骤连接
        /// </summary>
        private const string T_connect_off = "FR";
        /// <summary>
        /// 开启Fail Stop
        /// </summary>
        private const string T_fail_stop_on = "FS";
        /// <summary>
        /// 关闭Fail Stop
        /// </summary>
        private const string T_fail_stop_off = "FT";
        /// <summary>
        /// 开启防高压触电功能
        /// </summary>
        private const string T_gfi_on = "EA";
        /// <summary>
        /// 关闭防高压触电功能
        /// </summary>
        private const string T_gfi_off = "EB";
        /// <summary>
        /// 启用全部PASS回应
        /// </summary>
        private const string T_all_pass_srq_enable = "F0";
        /// <summary>
        /// 禁用全部PASS回应
        /// </summary>
        private const string T_all_pass_srq_disable = "F1";
        /// <summary>
        /// 启用FAIL回应
        /// </summary>
        private const string T_fail_srq_enable = "F2";
        /// <summary>
        /// 禁用FAIL回应
        /// </summary>
        private const string T_fail_srq_disable = "F3";
        /// <summary>
        /// 启用中断回应
        /// </summary>
        private const string T_abort_srq_enable = "F4";
        /// <summary>
        /// 禁用中断回应
        /// </summary>
        private const string T_abort_srq_disable = "F5";
        /// <summary>
        /// 启用错误命令回应
        /// </summary>
        private const string T_error_srq_enable = "F6";
        /// <summary>
        /// 禁用错误命令回应
        /// </summary>
        private const string T_error_srq_disable = "F7";

        private const string T_setp_1_value = "?1";
        private const string T_setp_2_value = "?2";
        private const string T_setp_3_value = "?3";
        private const string T_setp_4_value = "?4";
        private const string T_setp_5_value = "?5";
        private const string T_setp_6_value = "?6";
        private const string T_setp_7_value = "?7";
        private const string T_setp_8_value = "?8";
        private const string T_gnd_offset = "?A";
        private const string T_dcw_charge_lo = "?B";
        private const string T_ir_charge_lo = "?C";
        private const string T_reset_status = "?D";
        private const string T_oled_display = "?K";

        #endregion

        #endregion

        #region 系统设定
        /// <summary>
        /// 设定PLC遥控功能(1=ON,0=OFF)
        /// </summary>
        private const string T_setPLC = "PLC";
        
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
        public CExtech7440(int idNo = 0, string name = "CExtech7400")
        {
            this._idNo = idNo;
            this._name = name;
            this._failStop = "0";
            com = new CSerialPort();
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

                if (!WriteCmd(reset, out er))
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
                if (!WriteCmd(T_newMemory + " " + proName + "1", out er))
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
                //string smartGFI = pro[1];
                string cmdMemory = T_newMemory + " " + memory;
                string cmd = string.Empty;
                string rData = string.Empty;
                _stepNum = step.Count;
                _memoryNum = memory;

                if (!WriteCmd(T_gfi_on, out er))
                    return false;
                System.Threading.Thread.Sleep(110);

                if (!WriteCmd(T_fail_stop_on, out er))
                    return false;
                System.Threading.Thread.Sleep(110);

                if (!WriteCmd(cmdMemory, out er))
                    return false;
                System.Threading.Thread.Sleep(110);

                for (int i = 0; i < step.Count; i++)
                {
                    string stepNo = (i + 1).ToString();

                    if (!WriteCmd(T_newStep + " " + stepNo, out er))
                        return false;
                    System.Threading.Thread.Sleep(110);
                    switch (step[i].name)
                    {
                        case EStepName.AC:
                            if (!WriteCmd(newACW, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Voltage
                            cmd = T_voltage_acw + " " + ((double)(step[i].para[0].setVal)).ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Hight Limit
                            cmd = T_hi_limit_acw + " " + ((step[i].para[1].setVal)).ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Low Limit
                            cmd = T_lo_limit_acw + " " + ((step[i].para[2].setVal)).ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Ramp Time
                            cmd = T_ramp_up_time_acw + " " + step[i].para[3].setVal.ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Dwell Time          
                            cmd = T_dwell_time_acw + " " + step[i].para[4].setVal.ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //ARC Level
                            if (step[i].para[5].setVal > 0)
                            {
                                cmd = T_arc_acw + " " + step[i].para[5].setVal.ToString();
                                if (!WriteCmd(cmd, out er))
                                    return false;
                            }
                            System.Threading.Thread.Sleep(110);

                            //Channel
                            string acw_channels = step[i].para[6].setVal.ToString();
                            acw_channels = acw_channels.Replace("1", "H");
                            acw_channels = acw_channels.Replace("2", "L");
                            acw_channels = acw_channels.Replace("0", "O");
                            acw_channels = acw_channels.PadLeft(8, 'O');
                            cmd = T_channel_acw + " " + acw_channels;
                            if (!WriteCmd(cmd, out er))
                                return false;

                            //Offset 
                            cmd = T_offset_acw + " " + step[i].para[7].setVal.ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;

                            //Frequency
                            if (step[i].para[8].setVal.ToString() == "0") cmd = T_60hz_acw;
                            else cmd = T_50hz_acw;
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //ARC On/ OFF

                            if (step[i].para[8].setVal.ToString() == "0") cmd = T_arc_off_acw;
                            else cmd = T_arc_on_acw;
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            break;
                        case EStepName.DC:
                            if (!WriteCmd(newDCW, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Voltage
                            cmd = T_voltage_dcw + " " + ((double)(step[i].para[0].setVal)).ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Hight Limit
                            cmd = T_hi_limit_dcw + " " + ((step[i].para[1].setVal)).ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Low Limit
                            cmd = T_lo_limit_dcw + " " + ((step[i].para[2].setVal)).ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Ramp Time
                            cmd = T_ramp_up_time_dcw + " " + step[i].para[3].setVal.ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Dwell Time   
                            cmd = T_dwell_time_dcw + " " + step[i].para[4].setVal.ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;

                            //Charge Lo
                            cmd = T_charge_lo_dcw + " " + step[i].para[5].setVal.ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //ARC Level
                            if (step[i].para[5].setVal > 0)
                            {
                                cmd = T_arc_dcw + " " + step[i].para[6].setVal.ToString();
                                if (!WriteCmd(cmd, out er))
                                    return false;
                            }
                            System.Threading.Thread.Sleep(110);

                            //Channel
                            string dcw_channels = step[i].para[7].setVal.ToString();
                            dcw_channels = dcw_channels.Replace("1", "H");
                            dcw_channels = dcw_channels.Replace("2", "L");
                            dcw_channels = dcw_channels.Replace("0", "O");
                            dcw_channels = dcw_channels.PadLeft(8, 'O');
                            cmd = T_channel_dcw + " " + dcw_channels;
                            if (!WriteCmd(cmd, out er))
                                return false;

                            //Offset
                            cmd = T_offset_dcw + " " + step[i].para[8].setVal.ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;

                            //ARC ON/ OFF
                            if (step[i].para[9].setVal.ToString() == "0") cmd = T_arc_off_acw;
                            else cmd = T_arc_on_acw;
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Ramp Hi
                            if (step[i].para[10].setVal.ToString() == "0") cmd = T_ramp_hi_off_dcw;
                            else cmd = T_ramp_hi_on_dcw;
                            if (!WriteCmd(cmd, out er))
                                return false;

                            break;
                        case EStepName.IR:
                            if (!WriteCmd(newIR, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //"","","","","","Channel"
                            //Voltage
                            cmd = T_voltage_ir + " " + ((double)(step[i].para[0].setVal)).ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Charge Lo
                            cmd = T_charge_lo_ir + " " + step[i].para[1].setVal.ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);


                            //Hight Limit
                            cmd = T_hi_limit_ir + " " + ((double)(step[i].para[2].setVal)).ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Low Limit
                            cmd = T_lo_limit_ir + " " + ((double)(step[i].para[3].setVal)).ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Delay Time
                            cmd = T_delay_time_ir + " " + step[i].para[4].setVal.ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Channel
                            string ir_channels = step[i].para[5].setVal.ToString();
                            ir_channels = ir_channels.Replace("1", "H");
                            ir_channels = ir_channels.Replace("2", "L");
                            ir_channels = ir_channels.Replace("0", "O");
                            ir_channels = ir_channels.PadLeft(8, 'O');
                            cmd = T_channel_ir + " " + ir_channels;
                            if (!WriteCmd(cmd, out er))
                                return false;
                            break;
                        case EStepName.OSC:
                            break;
                        case EStepName.GB:
                            if (!WriteCmd(newGB, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //"", "", "", "", "Channel","Offset", "Frequency"

                            //Current
                            cmd = T_current_gb + " " + ((double)(step[i].para[0].setVal)).ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Voltage
                            cmd = T_voltage_gb + " " + ((double)(step[i].para[1].setVal)).ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Hight Limit
                            cmd = T_hi_limit_gb + " " + ((double)(step[i].para[2].setVal)).ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Dwell Time
                            cmd = T_dwell_time_gb + " " + step[i].para[3].setVal.ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Channel
                            cmd = T_channel_gb + " " + step[i].para[4].setVal.ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Offset 
                            cmd = T_offset_gb + " " + step[i].para[5].setVal.ToString();
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            //Frequency
                            if (step[i].para[6].setVal.ToString() == "0") cmd = T_60hz_gb;
                            else cmd = T_50hz_gb;
                            if (!WriteCmd(cmd, out er))
                                return false;
                            System.Threading.Thread.Sleep(110);

                            break;
                        case EStepName.PA:
                            break;
                        default:
                            break;
                    }

                    if (i == step.Count - 1)
                    {
                        if (!WriteCmd(T_connect_off, out er))
                            return false;
                    }
                    else
                    {
                        if (!WriteCmd(T_connect_on, out er))
                            return false;
                    }
                    System.Threading.Thread.Sleep(110);
                }
                //return WriteCmd(cmdMemory + "1", out er);

                return true;
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
                    if (!WriteCmd(reset, out er))
                        return false;
                }
                //statusStep = 1;
                return WriteCmd(test, out er);
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
            return WriteCmd(reset, out er);
        }
        /// <summary>
        /// 读状态已pass步骤
        /// </summary>
        //private int statusStep = 1;
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
                if (!SendCmdToHP("?K", out rData, out er))
                {
                    return false;
                }

                string[] data = rData.Split(',');
                string result = data[2].ToUpper();
                int step = Convert.ToInt32(data[3]);
                
                if (result == "FALL" || (step == chanMax && result != "RAMP" && result != "DWELL" && result != "DELAY"))
                {
                    status = EHPStatus.STOPPED;
                }
                else
                {
                    status = EHPStatus.RUNNING;
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
                int xxx = Environment.TickCount;
                //获取测试结果
                string rData = string.Empty;

                for (int i = 0; i < _stepNum; i++)
                {
                    if (!SendCmdToHP("?"+ (i + 1), out rData, out er))
                        return false;
                    if (rData.Length != 40)
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
                        case "GND":
                            stepMode.Add(EStepName.IR);
                            stepVal.Add(Convert.ToDouble(result[3].Substring(0, result[3].Length - 1)));
                            stepUnit.Add("A");
                            stepVal.Add(Convert.ToDouble(result[4].Substring(0, result[4].Length - 2)));
                            stepUnit.Add("mΩ");
                            stepCode.Add(result[2]);
                            break;
                        default:
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
                int b = Environment.TickCount - xxx;
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
                if (!WriteCmd_Inner(wCmd, out er))
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
                if (!SendCmdToHP(wCmd, out rData, out er))
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
        private bool ReadIDN(out string devName, out string er)
        {
            devName = string.Empty;

            er = string.Empty;

            try
            {
                string cmd = "SDN?";

                string rData = string.Empty;

                if (!SendCmdToHP(cmd, out rData, out er))
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
        private bool ReadError(out string errCode, out string er)
        {
            errCode = string.Empty;

            er = string.Empty;


            try
            {
                string cmd = "SYST:ERR?";

                string rData = string.Empty;

                if (!SendCmdToHP(cmd, out rData, out er))
                    return false;

                string[] codeList = rData.Split(',');

                int code = Convert.ToInt32(codeList[0]);

                if (code == 0)
                    errCode = "OK";
                else
                    errCode = "Error Infor:" + codeList[1].Replace("\"", "") + "-" + code.ToString();

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
        private bool Remote(out string er)
        {
            er = string.Empty;

            try
            {
                string cmd = "SPR?";

                string rData = string.Empty;

                if (!SendCmdToHP(cmd, out rData, out er))
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
        private bool SaveProgram(string proName, out string er)
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

                if (SendCmdToHP(cmd, out rData, out er)) //程序名存在
                {
                    proNo = Convert.ToInt16(rData);

                    if (!WriteCmd("*SAV " + proNo, out er))
                        return false;

                    if (!WriteCmd("MEM:STAT:DEF \"" + proName + "\"," + proNo, out er))
                        return false;

                }
                else
                {
                    //查询存储空间
                    cmd = "MEM:NST?";
                    if (!SendCmdToHP(cmd, out rData, out er))
                        return false;
                    int maxProNo = Convert.ToInt16(rData);

                    //查询可用位置
                    cmd = "MEM:FREE:STAT?";
                    if (!SendCmdToHP(cmd, out rData, out er))
                        return false;
                    int proIndex = Convert.ToInt16(rData);

                    proNo = maxProNo - proIndex;

                    if (proNo == 0)
                    {
                        er = "高压机无可用存储程序,请删除多余程序";
                        return false;
                    }

                    WriteCmd("*SAV " + proNo, out er);

                    WriteCmd("MEM:STAT:DEF \"" + proName + "\"," + proNo, out er);
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
        private bool WriteCmd_Inner(string wCmd, out string er, int delayMs = 5, bool readResult = false, int timeOutMs = 500)
        {
            string rData = string.Empty;

            er = string.Empty;

            try
            {
                if (!SendCmdToHP(wCmd, out rData, out er))
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
        private bool SendCmdToHP(string wData, out string rData, out string er, string rEOI = "\n", int wTimeOut = 1000)
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

