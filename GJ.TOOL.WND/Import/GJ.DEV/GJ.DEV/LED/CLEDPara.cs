using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.LED
{
    #region 枚举
    /// <summary>
    /// ERS类型
    /// </summary>
    public enum EType
    {
        DA_320_8,
        DA_750_4,
        DA_200_16,
        DA_250_8
    }
    /// <summary>
    /// 线程状态
    /// </summary>
    public enum EThreadStatus
    {
        空闲,
        运行,
        暂停,
        退出
    }
    /// <summary>
    /// 操作
    /// </summary>
    public enum EOP
    {
        空闲,
        读取,
        读取OK,
        写入,
        写入OK
    }
    /// <summary>
    /// ERS状态
    /// </summary>
    public enum ESTATUS
    {
        /// <summary>
        /// 不监控
        /// </summary>
        禁用 = -1,
        /// <summary>
        /// 只监控信号
        /// </summary>
        空闲 = 0,
        /// <summary>
        /// 监控
        /// </summary>
        运行 = 1
    }
    /// <summary>
    /// 负载模式
    /// </summary>
    public enum EMODE
    {
        /// <summary>
        /// (恒流_慢速)模式
        /// </summary>
        CC_Slow,
        /// <summary>
        /// （恒压）模式
        /// </summary>
        CV,
        /// <summary>
        /// （恒功率）模式
        /// </summary>
        CP,
        /// <summary>
        /// （恒阻）模式
        /// </summary>
        CR,
        /// <summary>
        /// （恒流_快速）模式
        /// </summary>
        CC_Fast,
        /// <summary>
        /// LED模式
        /// </summary>
        LED,
        /// <summary>
        /// MTK PE快充模式
        /// </summary>
        MTK
    }
    /// <summary>
    /// DC_DC开启
    /// </summary>
    public enum EDC
    {
        OFF,
        ON
    }
    /// <summary>
    /// LLC输入过压
    /// </summary>
    public enum ELLC
    {
        正常,
        OVP
    }
    /// <summary>
    /// 输入电压状态
    /// </summary>
    public enum EAC
    {
        欠压,
        正常,
        过压
    }
    /// <summary>
    /// 超功率保护
    /// </summary>
    public enum EOPP
    {
        正常,
        OPP
    }
    /// <summary>
    /// 超温保护
    /// </summary>
    public enum EOTP
    {
        正常,
        OTP
    }
    /// <summary>
    /// 直流母线电压状态
    /// </summary>
    public enum EVBus
    {
        欠压,
        正常,
        过压
    }
    /// <summary>
    /// 大电流通道过流
    /// </summary>
    public enum EMaxCUR
    {
        正常,
        过流
    }
    /// <summary>
    /// 小电流通道过流
    /// </summary>
    public enum EMinCUR
    {
        正常,
        过流
    }
    /// <summary>
    /// 逆变过温
    /// </summary>
    public enum EINVOTP
    {
        正常,
        OTP
    }
    /// <summary>
    /// AD采样故障
    /// </summary>
    public enum EINVAD
    {
        正常,
        故障
    }
    /// <summary>
    /// 电网电压状态
    /// </summary>
    public enum EINVACBus
    {
        欠压,
        正常,
        过压
    }
    /// <summary>
    /// 风扇故障
    /// </summary>
    public enum EINVFan
    {
        正常,
        故障
    }
    /// <summary>
    /// 超时故障
    /// </summary>
    public enum EINVTIME
    {
        正常,
        超时
    }
    /// <summary>
    /// 直流母线电压状态
    /// </summary>
    public enum EINVDCBus
    {
        欠压,
        正常,
        过压
    }
    #endregion

    #region 参数类
    /// <summary>
    /// 负载参数
    /// </summary>
    public class CLOAD
    {
        /// <summary>
        /// 负载模式(1Byte)
        /// </summary>
        public EMODE Mode = EMODE.CC_Slow;
        /// <summary>
        /// Von点
        /// </summary>
        public double Von = 10;
        /// <summary>
        /// 负载值
        /// </summary>
        public double load = 0;
        /// <summary>
        /// 模式附加参数
        /// </summary>
        public double mark = 0;
        /// <summary>
        /// 单位
        /// </summary>
        public string unit = "A";
    }
    /// <summary>
    /// 通道状态
    /// </summary>
    public class CCHSTAT
    {
        /// <summary>
        /// DC-DC开启
        /// </summary>
        public EDC DC_DC = EDC.OFF;
        /// <summary>
        /// LLC输入过压
        /// </summary>
        public ELLC LLC_AC = ELLC.正常;
        /// <summary>
        /// 输入电压状态
        /// </summary>
        public EAC AC_STATUS = EAC.正常;
        /// <summary>
        /// 超功率保护
        /// </summary>
        public EOPP OPP = EOPP.正常;
        /// <summary>
        /// 超温保护
        /// </summary>
        public EOTP OTP = EOTP.正常;
        /// <summary>
        /// 直流母线电压状态
        /// </summary>
        public EVBus VBus = EVBus.正常;
        /// <summary>
        /// 大电流通道过流
        /// </summary>
        public EMaxCUR maxCur = EMaxCUR.正常;
        /// <summary>
        /// 小电流通道过流
        /// </summary>
        public EMinCUR minCur = EMinCUR.正常;
    }
    /// <summary>
    /// 逆变状态
    /// </summary>
    public class CINVSTAT
    {
        /// <summary>
        /// 逆变过温
        /// </summary>
        public EINVOTP OTP = EINVOTP.正常;
        /// <summary>
        /// AD采样故障
        /// </summary>
        public EINVAD AD = EINVAD.正常;
        /// <summary>
        /// 电网电压状态
        /// </summary>
        public EINVACBus ACBus = EINVACBus.正常;
        /// <summary>
        /// 风扇故障
        /// </summary>
        public EINVFan Fan = EINVFan.正常;
        /// <summary>
        /// 超时故障
        /// </summary>
        public EINVTIME Time = EINVTIME.正常;
        /// <summary>
        /// 直流母线电压状态
        /// </summary>
        public EINVDCBus DCBus = EINVDCBus.正常;
    }
    /// <summary>
    /// 负载通道数据
    /// </summary>
    public class CHAN_DATA
    {
        /// <summary>
        /// 负载电压
        /// </summary>
        public double volt = 0;
        /// <summary>
        /// 负载电流
        /// </summary>
        public double current = 0;
        /// <summary>
        /// 输入电压
        /// </summary>
        public double input_ac = 0;
        /// <summary>
        /// 通道状态
        /// </summary>
        public CCHSTAT ch_status = new CCHSTAT();
        /// <summary>
        /// 通道报警提示
        /// </summary>
        public string alarmInfo = string.Empty;
    }
    /// <summary>
    /// 回读负载值
    /// </summary>
    public class CData
    {
        public CData()
        {
            for (int i = 0; i < _maxCH; i++)
                chan.Add(new CHAN_DATA());
        }
        private const int _maxCH = 16;
        /// <summary>
        /// 负载通道数据
        /// </summary>
        public List<CHAN_DATA> chan = new List<CHAN_DATA>();
        /// <summary>
        /// 逆变状态
        /// </summary>
        public CINVSTAT inv_status = new CINVSTAT();
        /// <summary>
        /// 逆变报警提示
        /// </summary>
        public string alarmInfo = string.Empty;
        /// <summary>
        /// 返回值
        /// </summary>
        public string rCmd = string.Empty;
    }
    #endregion

    #region 线程控制类
    /// <summary>
    /// 设置负载单通道设置
    /// </summary>
    public class COP_SetLoadVal
    {
        /// <summary>
        /// 操作标志
        /// </summary>
        public EOP op = EOP.空闲;
        /// <summary>
        /// 设置负载通道值
        /// </summary>
        public CLOAD LoadSeting = new CLOAD();
        /// <summary>
        /// 保存EEProm
        /// </summary>
        public bool SaveEPROM = false;
    }
    /// <summary>
    /// 读取负载通道设置值
    /// </summary>
    public class COP_ReadLoadSetting
    {
        /// <summary>
        /// 操作标志
        /// </summary>
        public EOP op = EOP.空闲;
        /// <summary>
        /// 读取负载通道值
        /// </summary>
        public CLOAD LoadSeting = new CLOAD();
    }
     /// <summary>
    /// DA负载基础信息
    /// </summary>
    public class CBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 地址
        /// </summary>
        public int addr = 0;
        /// <summary>
        /// 状态
        /// </summary>
        public ESTATUS status = ESTATUS.运行;
        /// <summary>
        /// 通信状态
        /// </summary>
        public bool conStatus = false;
    }
    /// <summary>
    /// 线程操作类
    /// </summary>
    public class CPara
    {
       public CPara(int maxCH)
       {
           for (int i = 0; i < maxCH; i++)
           {
               OP_SetLoadVal.Add(new COP_SetLoadVal());

               OP_ReadLoadSetting.Add(new COP_ReadLoadSetting());  
           }
       }
       /// <summary>
       /// 设置负载通道设置
       /// </summary>
       public List<COP_SetLoadVal> OP_SetLoadVal = new List<COP_SetLoadVal>();
       /// <summary>
       /// 读取负载通道设置值
       /// </summary>
       public List<COP_ReadLoadSetting> OP_ReadLoadSetting = new List<COP_ReadLoadSetting>();
       
    }
    /// <summary>
    /// 线程设备类
    /// </summary>
    public class CLED
    {
        public CLED()
        {
            for (int i = 0; i < _maxCH; i++)
            {
                SetLoadVal.Add(new CLOAD());
                ReadLoadSetting.Add(new CLOAD()); 
            }
        }
        private const int _maxCH = 8;
        /// <summary>
        /// 基本信息
        /// </summary>
        public CBase Base = new CBase();
        /// <summary>
        /// 回读负载参数
        /// </summary>
        public CData Data = new CData();
        /// <summary>
        /// 负载通道设置
        /// </summary>
        public List<CLOAD> SetLoadVal = new List<CLOAD>();
        /// <summary>
        /// 读取负载通道设置
        /// </summary>
        public List<CLOAD> ReadLoadSetting = new List<CLOAD>();
        /// <summary>
        /// 线程操作类
        /// </summary>
        public CPara Para = new CPara(_maxCH);
    }
    #endregion

    #region 事件定义
    public class CConArgs : EventArgs
    {
        public readonly string conStatus;
        public readonly bool bErr;
        public CConArgs(string conStatus, bool bErr = false)
        {
            this.conStatus = conStatus;
            this.bErr = bErr;
        }
    }
    public class CDataArgs : EventArgs
    {
        public readonly string rData;
        public readonly bool bErr;
        public readonly bool bComplete;
        public CDataArgs(string rData, bool bComplete = true, bool bErr = false)
        {
            this.rData = rData;
            this.bComplete = bComplete;
            this.bErr = bErr;
        }
    }
    #endregion

}
