using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.ELOAD
{
    #region 枚举
    /// <summary>
    /// 设备类型
    /// </summary>
    public enum EType
    {
        EL_20_16,
        EL_40_08,
        EL_50_08,
        EL_100_04
    }
    /// <summary>
    /// 拉载模式
    /// </summary>
    public enum EMode
    {
        CC,
        CV,
        LED
    }
    #endregion

    #region 参数类
    /// <summary>
    ///设置电子负载参数
    /// </summary>
    public class CEL_SetPara
    {
        private const int ELMaxCH = 8;
        /// <summary>
        /// 写入EEPROM 0:不擦除旧数据及写新数据 1:擦除旧数据 2:写新数据
        /// </summary>
        public int SaveEEPROM = 2;
        /// <summary>
        /// OTP_START温度(90-130)
        /// </summary>
        public int OTP_Start = 94;
        /// <summary>
        /// OTP_STOP温度(65-100)
        /// </summary>
        public int OTP_Stop = 70;
        /// <summary>
        /// 0:PWM_STOP 1:PWM_START 
        /// </summary>
        public int PWM_Status = 1;
        /// <summary>
        /// PWM频率 
        /// </summary>
        public int PWM_Freq = 100;
        /// <summary>
        /// PWM占空比
        /// </summary>
        public int PWM_DutyCycle = 1;
        /// <summary>
        /// 工作状态 0:停止 1：启动 
        /// </summary>
        public int Run_Status = 1;
        /// <summary>
        /// 工作功率 0:20W/100W 1:40W/150W   
        /// </summary>
        public int[] Run_Power = new int[ELMaxCH];
        /// <summary>
        /// 工作模式 0:CC模式 1:CV模式 2:LED模式
        /// </summary>
        public EMode[] Run_Mode = new EMode[ELMaxCH];
        /// <summary>
        /// 置工作数据
        /// </summary>
        public double[] Run_Val = new double[ELMaxCH];
        /// <summary>
        /// 设置Von
        /// </summary>
        public double[] Run_Von = new double[ELMaxCH];
    }
    /// <summary>
    /// 回读电子负载设置
    /// </summary>
    public class CEL_ReadSetPara
    {
        private const int ELMaxCH = 8;
        public string[] status = new string[ELMaxCH];
        public EMode[] LoadMode = new EMode[ELMaxCH];
        public double[] LoadVal = new double[ELMaxCH];
        public double[] Von = new double[ELMaxCH];
    }
    /// <summary>
    /// 回读电子负载读值
    /// </summary>
    public class CEL_ReadData
    {
        private const int ELMaxCH = 8;
        /// <summary>
        /// 温度0
        /// </summary>
        public int NTC_0;
        /// <summary>
        /// 温度1
        /// </summary>
        public int NTC_1;
        /// <summary>
        /// ON OFF
        /// </summary>
        public int ONOFF;
        public int OCP;
        public int OVP;
        public int OPP;
        public int OTP;
        /// <summary>
        /// 状态指示
        /// </summary>
        public string Status;
        /// <summary>
        /// Vs电压
        /// </summary>
        public double[] Vs = new double[ELMaxCH];
        /// <summary>
        /// Load电压
        /// </summary>
        public double[] Volt = new double[ELMaxCH];
        /// <summary>
        /// 负载读值
        /// </summary>
        public double[] Load = new double[ELMaxCH];
    }
    #endregion

}
