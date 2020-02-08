using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace GJ.DEV.FCMB
{
    #region 枚举
    /// <summary>
    /// 类型
    /// </summary>
    public enum EType
    {
        FMB_V1
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
    /// 寄存器类型
    /// </summary>
    public enum ERegType
    {
        X,
        Y,
        D
    }
    /// <summary>
    /// 快充模式
    /// </summary>
    public enum EQCM
    {
        /// <summary>
        /// 关闭当前快充模式，电压设定为 5V
        /// </summary>
        Normal = 0,
        /// <summary>
        /// QC2.0
        /// </summary>
        QC2_0 = 1,
        /// <summary>
        /// QC3.0
        /// </summary>
        QC3_0 = 2,
        /// <summary>
        /// HW FCP
        /// </summary>
        FCP = 3,
        /// <summary>
        /// HW SCP
        /// </summary>
        SCP = 4,
        /// <summary>
        /// USB PD3.0--苹果
        /// </summary>
        PD3_0 = 5,
        /// <summary>
        /// MTK PE3.0(已知支持一款充电器)
        /// </summary>
        PE3_0 = 6,
        /// <summary>
        ///PE1.0(需要小板硬件支持)--VIVO
        /// </summary>
        VIVO = 7,
        /// <summary>
        /// MTK PE2.0(需要小板硬件支持)
        /// </summary>
        Reserve = 8,
        /// <summary>
        /// OPPO
        /// </summary>
        OPPO = 9,
        /// <summary>
        /// FSCP--华为
        /// </summary>
        FSCP=10,
        /// <summary>
        /// 负载控制MTK1.0->设置为普通模式
        /// </summary>
        MTK1_0 = 90,
        /// <summary>
        /// 负载控制MTK2.0->设置为普通模式
        /// </summary>
        MTK2_0 =100,
        /// <summary>
        /// ATE快充板:高8位=0x205
        /// Bit0为1连接CAP负载
        /// Bit1为1连接CC线
        /// Bit2为0连接CC1,为1连接 CC2
        /// </summary>
        PD3_1=517
    }
    /// <summary>
    /// 时序模式
    /// </summary>
    public enum EOnOffMode
    {
        上位机控制=0,
        正常运行=1,
        暂停运行=2,
        继续运行=3,
        重计时运行=4,
        停止运行=5
    }
    /// <summary>
    /// 控制板状态
    /// </summary>
    public enum ESTATUS
    { 
      /// <summary>
      /// 不监控
      /// </summary>
      禁用=-1,
      /// <summary>
      /// 只监控信号
      /// </summary>
      空闲=0,
      /// <summary>
      /// 监控
      /// </summary>
      运行=1
    }
    /// <summary>
    /// IO信号输入
    /// </summary>
    public enum EFMB_rIO
    {
       AC同步信号=0,
       治具到位信号1=1,
       治具到位信号2=2,
       治具到位信号3=3,
       治具到位信号4=4,
       自动切换信号=5,
       手动顶升信号=6,
       手动下降信号=7,
       S1状态=8,
       AC电压信号=9,
       继电器粘连警告=10,
       检测AC电压=11,
       运行信号灯=12,
       错误信号灯=13,
       气缸控制1=14,
       气缸控制2=15
    }
    /// <summary>
    /// IO信号输出
    /// </summary>
    public enum EFMB_wIO
    {
        错误信号灯,
        继电器信号,
        气缸控制1,
        气缸控制2
    }
    /// <summary>
    /// 电压采集模式
    /// </summary>
    public enum EVMODE
    { 
       VOLT_40,
       VOLT_32
    }
    /// <summary>
    /// IO信号输出
    /// </summary>
    public enum EFMB_Y
    { 
      Y0,
      Y1,
      Y2,
      Y3,
      Y4,
      Y5,
      Y6,
      Y7
    }
    #endregion

    #region 类定义
    /// <summary>
    /// 时序段类
    /// </summary>
    public class COnOff
    {
        /// <summary>
        /// 总时间(S)
        /// </summary>
        public int OnOffTime = 0;
        /// <summary>
        /// ON时间(S)
        /// </summary>
        public int OnTime = 0;
        /// <summary>
        /// OFF时间(S)
        /// </summary>
        public int OffTime = 0;
    }
    /// <summary>
    /// 时序控制
    /// </summary>
    public class CFMB_ONOFF
    {
        public CFMB_ONOFF()
        {
            for (int i = 0; i < 10; i++)
                OnOff.Add(new COnOff());
        }
        public EOP op = EOP.空闲; 
        /// <summary>
        /// 时序控制模式
        /// </summary>
        public EOnOffMode mode = EOnOffMode.上位机控制;
        /// <summary>
        /// 总时间(S)
        /// </summary>
        public int TotalTime = 0;
        /// <summary>
        /// 时序段-10段
        /// </summary>
        public List<COnOff> OnOff = new List<COnOff>();
    }
    /// <summary>
    /// 快充控制
    /// </summary>
    public class CFMB_QCM
    {
        /// <summary>
        /// 操作标志
        /// </summary>
        public EOP op = EOP.空闲; 
        /// <summary>
        /// 快充模式
        /// </summary>
        public EQCM qcm = EQCM.Normal;
        /// <summary>
        /// 快充电压
        /// </summary>
        public double qcv = 0;
        /// <summary>
        /// 快充电流:默认为0
        /// </summary>
        public double qci = 0;
        /// <summary>
        /// 启动cc2,默认为cc1
        /// </summary>
        public bool cc2 = false;
    }
    /// <summary>
    /// AC控制
    /// </summary>
    public class CFMB_ACON
    {
        /// <summary>
        /// 操作标志
        /// </summary>
        public EOP op = EOP.空闲;
        /// <summary>
        /// AC电压
        /// </summary>
        public double acVolt = 0;
        /// <summary>
        /// 开关
        /// </summary>
        public int wOnOff = 0;
        /// <summary>
        /// 同步AC
        /// </summary>
        public bool synC = false;
        /// <summary>
        /// 苹果模式
        /// </summary>
        public bool B400 = false;
    }
    /// <summary>
    /// 写IO信号
    /// </summary>
    public class CFMB_IO
    {
        /// <summary>
        /// 操作标志
        /// </summary>
        public EOP op = EOP.空闲;
        /// <summary>
        /// IO值
        /// </summary>
        public int ioVal = 0;
    }
    /// <summary>
    /// 基本信息
    /// </summary>
    public class CFMB_Info
    {
        public CFMB_Info()
        {
            for (int i = 0; i < CFMBPara.Child_Max; i++)
                childVer.Add(""); 
        }
        /// <summary>
        /// 地址
        /// </summary>
        public int addr = 0;
        /// <summary>
        /// 名称
        /// </summary>
        public string name = string.Empty;
        /// <summary>
        /// 状态
        /// </summary>
        public ESTATUS status = ESTATUS.运行;
        /// <summary>
        /// 设备名称
        /// </summary>
        public string devName = string.Empty;
        /// <summary>
        /// 设备SN
        /// </summary>
        public string devSn = string.Empty;
        /// <summary>
        /// 设备版本
        /// </summary>
        public string devVer =string.Empty;
        /// <summary>
        /// 小板版本 0:表示通信异常
        /// </summary>
        public List<string> childVer = new List<string>();
        /// <summary>
        /// 通信状态
        /// </summary>
        public bool conStatus = false;
    }
    /// <summary>
    /// 测试变量
    /// </summary>
    public class CFMB_Para
    {
        public CFMB_Para()
        {
            rIO.Add(EFMB_rIO.AC同步信号, 0);
            rIO.Add(EFMB_rIO.治具到位信号1, 0);
            rIO.Add(EFMB_rIO.治具到位信号2, 0);
            rIO.Add(EFMB_rIO.治具到位信号3, 0);
            rIO.Add(EFMB_rIO.治具到位信号4, 0);
            rIO.Add(EFMB_rIO.自动切换信号, 0);
            rIO.Add(EFMB_rIO.手动顶升信号, 0);
            rIO.Add(EFMB_rIO.手动下降信号, 0);
            rIO.Add(EFMB_rIO.S1状态, 0);
            rIO.Add(EFMB_rIO.AC电压信号, 0);  
            rIO.Add(EFMB_rIO.继电器粘连警告, 0);  
            rIO.Add(EFMB_rIO.检测AC电压, 0);  
            rIO.Add(EFMB_rIO.运行信号灯, 0);
            rIO.Add(EFMB_rIO.错误信号灯, 0);
            rIO.Add(EFMB_rIO.气缸控制1, 0);
            rIO.Add(EFMB_rIO.气缸控制2, 0);

            wIO.Add(EFMB_wIO.错误信号灯,new CFMB_IO());
            wIO.Add(EFMB_wIO.继电器信号, new CFMB_IO());
            wIO.Add(EFMB_wIO.气缸控制1, new CFMB_IO());
            wIO.Add(EFMB_wIO.气缸控制2, new CFMB_IO());

            for (int i = 0; i < CFMBPara.Child_Max; i++)
                Volt.Add(0);  
        }
        /// <summary>
        /// 读IO信号
        /// </summary>
        public Dictionary<EFMB_rIO, int> rIO = new Dictionary<EFMB_rIO, int>();
        /// <summary>
        /// 电压值
        /// </summary>
        public List<double> Volt = new List<double>();
        /// <summary>
        /// 快充控制
        /// </summary>
        public CFMB_QCM QCM = new CFMB_QCM();
        /// <summary>
        /// ON/OFF时序
        /// </summary>
        public CFMB_ONOFF OnOff = new CFMB_ONOFF();
        /// <summary>
        /// 控制AC ON/OFF
        /// </summary>
        public CFMB_ACON ACON = new CFMB_ACON();
        /// <summary>
        /// 当前输入AC电压
        /// </summary>
        public double CurACVolt = 0;
        /// <summary>
        /// 写IO信号
        /// </summary>
        public Dictionary<EFMB_wIO, CFMB_IO> wIO = new Dictionary<EFMB_wIO, CFMB_IO>();  
    }
    /// <summary>
    /// 快充板
    /// </summary>
    public class CFMB
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        public CFMB_Info Base = new CFMB_Info();
        /// <summary>
        /// 测试信息 
        /// </summary>
        public CFMB_Para Para = new CFMB_Para();

        public override string ToString()
        {
            return Base.name;
        }
    }
    #endregion

    #region 消息定义
    /// <summary>
    /// 状态消息
    /// </summary>
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
    /// <summary>
    /// 数据消息
    /// </summary>
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

    /// <summary>
    /// 全局类
    /// </summary>
    public class CFMBPara
    {
        /// <summary>
        /// 小板数量
        /// </summary>
        public static int Child_Max = 40;
    }

}
