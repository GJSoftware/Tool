using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.Mon
{
    #region 枚举
    /// <summary>
    /// 类型
    /// </summary>
    public enum EType
    {
        MON32
    }
    /// <summary>
    /// ON/OFF状态
    /// </summary>
    public enum EOnOff
    {
        ON,
        OFF
    }
    /// <summary>
    /// AC同步模式
    /// </summary>
    public enum ESynON
    {
        同步,
        异步
    }
    /// <summary>
    /// 错误代码
    /// </summary>
    public enum EErrCode
    {
        正常,
        治具到位信号异常,
        气缸升到位信号异常,
        AC同步信号异常,
        治具1AC不通,
        治具2AC不通,
        治具位12AC都不通,
        气缸降到位信号异常,
        有负载回路不良
    }
    /// <summary>
    /// 运行模式
    /// </summary>
    public enum EBIMode
    {
        普通老化模式,
        雅达老化模式,
        同步老化模式,
        可控制快充模式,
        海思快充模式
    }
    /// <summary>
    /// 运行模式
    /// </summary>
    public enum ERunMode
    {
        普通老化房模式,
        自动线模式
    }
    #endregion

    #region 内部类
    /// <summary>
    /// 设置控制板运行参数
    /// </summary>
    public class CwRunPara
    {
        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public CwRunPara Clone()
        {
            CwRunPara para = new CwRunPara();
            para.runTolTime = this.runTolTime;
            para.secMinCnt = this.secMinCnt;
            para.runTypeFlag = this.runTypeFlag;
            para.startFlag = this.startFlag;
            para.onoff_RunTime = this.onoff_RunTime;
            para.onoff_YXDH = this.onoff_YXDH;
            para.onoff_Cnt = this.onoff_Cnt;
            para.onoff_Flag = this.onoff_Flag;
            return para;
        }
        /// <summary>
        /// 老化总时间计时(Unit:Min)
        /// </summary>
        public int runTolTime;
        /// <summary>
        /// 60Sec/Min的计时
        /// </summary>
        public int secMinCnt;
        /// <summary>
        /// 1->只有治具1进行老化，2->只有治具2进行老化，3->治具1,2同时进行老化 4->空治具老化
        /// </summary>
        public int runTypeFlag;
        /// <summary>
        /// 0->无启动老化请求，1->PC请求启动老化,2->自检 3-〉老化中 4-〉老化结束
        /// </summary>
        public int startFlag;
        /// <summary>
        /// ON/OFF运行段的计时 (Unit:Sec)
        /// </summary>
        public int onoff_RunTime;
        /// <summary>
        /// OnOff的运行段号
        /// </summary>
        public int onoff_YXDH;
        /// <summary>
        /// OnOff的运行次数
        /// </summary>
        public int onoff_Cnt;
        /// <summary>
        /// OnOff_Flag=ON,OnOff_Flag=Off
        /// </summary>
        public int onoff_Flag;
    }
    /// <summary>
    /// 回读控制板运行参数
    /// </summary>
    public class CrRunPara
    {
        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public CrRunPara Clone()
        {
            CrRunPara para = new CrRunPara();
            para.runTolTime = this.runTolTime;
            para.secMinCnt = this.secMinCnt;
            para.runTypeFlag = this.runTypeFlag;
            para.startFlag = this.startFlag;  
            para.biFinishFlag = this.biFinishFlag;
            para.onoff_RunTime = this.onoff_RunTime;
            para.onoff_YXDH = this.onoff_YXDH;
            para.onoff_Cnt = this.onoff_Cnt;
            para.onoff_Flag = this.onoff_Flag;
            para.s1 = this.s1;
            para.s2 = this.s2;
            para.ac_Sync = this.ac_Sync;
            para.ac_on = this.ac_on;
            para.upAir = this.upAir;
            para.downAir = this.downAir;
            for (int i = 0; i < this.x.Length; i++)
                para.x[i] = this.x[i];
            para.errCode = this.errCode;
            para.QC_TYPE = this.QC_TYPE;
            para.QC_VOLT = this.QC_VOLT;
            para.QC_Y_VOLT = this.QC_Y_VOLT;
            for (int i = 0; i < this.Y.Length; i++)
                para.Y[i] = this.Y[i];
            return para;
        }
        public int runTolTime;     //老化总时间计时(Unit:Min)
        public int secMinCnt;      //60Sec/Min的计时
        public int runTypeFlag;    //1->只有治具1进行老化，2->只有治具2进行老化，3->治具1,2同时进行老化 4->空治具老化 
        public int startFlag;      //0->无启动老化请求，1->PC请求启动老化,2->自检 3-〉老化中 4-〉老化结束
        public int biFinishFlag;   //0->完成老化，1->老化未完成  [掉电保存]
        public int onoff_RunTime;  //ON/OFF运行段的计时 (Unit:Sec)
        public int onoff_YXDH;    //OnOff的运行段号
        public int onoff_Cnt;     //OnOff的运行次数
        public int onoff_Flag;    //OnOff_Flag=ON,OnOff_Flag=Off
        public int s1;           //设定地址的指拨开关信号
        public int s2;           //切换手动/自动运行模式的指拨开关信号
        public int ac_Sync;      //主接触器辅助触点过来的同步信号
        public int ac_on;        //AC 输入ON
        public int upAir;     //升气缸按钮信号
        public int downAir;   //降气缸按钮信号
        public int[] x = new int[10];  //X1 - X9 输入信号
        public EErrCode errCode;      //0->无故障;1->治具到位信号异常;2->气缸升到位信号异常;3->AC同步信号异常
                                     // 4->治具1AC不通;5->治具2AC不通; 6->2治具位AC都不通;
                                     //7->气缸降到位信号异常;8->有负载回路不良
        /// <summary>
        /// 快充模式
        /// </summary>
        public int QC_TYPE = 0;
        /// <summary>
        /// 快充电压
        /// </summary>
        public int QC_VOLT = 0;
        /// <summary>
        /// 快充QC2.0控制D+/D-
        /// </summary>
        public int QC_Y_VOLT = 0;
        /// <summary>
        /// 快充控制模式
        /// </summary>
        public int[] Y = new int[10];  //Y1-Y9;

    }
    /// <summary>
    /// 设置控制板ON/OFF参数
    /// </summary>
    public class COnOffPara
    {
        public COnOffPara Clone()
        {
            COnOffPara para = new COnOffPara();

            para.BIToTime = this.BIToTime;

            for (int i = 0; i < 4; i++)
            {
                para.wOnOff[i] = this.wOnOff[i];
                para.wON[i] = this.wON[i];
                para.wOFF[i] = this.wOFF[i];
                para.wQCVolt[i] = this.wQCVolt[i];  
            }

            para.wQCType = this.wQCType;

            return para;
        }
        /// <summary>
        /// Unit Min
        /// </summary>
        public int BIToTime;
        /// <summary>
        /// 快充类型，0->QC2.0;1-QC3.0;2->MTK  Ver5.1后才有此功能 3->海思
        /// </summary>
        public int wQCType = 0;
        /// <summary>
        /// ON1,ON2,ON3,ON4设定值  (Unit:Sec)
        /// </summary> 
        public int[] wON = new int[4];
        /// <summary>
        /// OFF1,OFF2,OFF3,OFF4设定值  (Unit:Sec)
        /// </summary>
        public int[] wOFF = new int[4];
        /// <summary>
        /// OnOff1_Cycle,OnOff2_Cycle,OnOff3_Cycle,OnOff4_Cycle的设定值
        /// </summary>
        public int[] wOnOff = new int[4];
        /// <summary>
        /// 4个ONOFF阶段对应的快充电压设定:0->5V;1->9V;2->12V;3->20V  ,Ver5.1后才有此功能
        /// </summary>
        public int[] wQCVolt = new int[4];
    }
    #endregion

    #region 线程
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
    /// 控制板状态
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
    /// 基本信息
    /// </summary>
    public class CMon_Base
    {
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
        public string devVer = string.Empty;
        /// <summary>
        /// 通信状态
        /// </summary>
        public bool conStatus = false;
    }
    /// <summary>
    /// 状态信息
    /// </summary>
    public class CMon_Para
    {
        public CMon_Para()
        {
            for (int i = 0; i < 32; i++)
                Volt.Add(0); 
        }
        /// <summary>
        /// 状态信号
        /// </summary>
        public CrRunPara Sinal = new CrRunPara();
        /// <summary>
        /// 电压值
        /// </summary>
        public List<double> Volt = new List<double>();
        /// <summary>
        /// AC同步信号
        /// </summary>
        public int rOnOff = 0;
        /// <summary>
        /// 时序读取
        /// </summary>
        public EOP TIMER_OP = EOP.空闲;
        /// <summary>
        /// 快充设置读取
        /// </summary>
        public EOP QCV_OP = EOP.空闲;
        /// <summary>
        /// 写入状态 
        /// </summary>
        public EOP RUN_OP = EOP.空闲;
        /// <summary>
        /// 强制结束
        /// </summary>
        public EOP STOP_OP = EOP.空闲;
        /// <summary>
        /// ON/OFF时序
        /// </summary>
        public COnOffPara Timer = new COnOffPara();
        /// <summary>
        /// 运行状态
        /// </summary>
        public CwRunPara RunPara = new CwRunPara();
    }
    /// <summary>
    /// 控制板
    /// </summary>
    public class CMon
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        public CMon_Base Base = new CMon_Base();
        /// <summary>
        /// 参数信息
        /// </summary>
        public CMon_Para Para = new CMon_Para();

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
}
