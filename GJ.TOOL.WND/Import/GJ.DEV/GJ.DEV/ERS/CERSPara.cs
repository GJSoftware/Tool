using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.ERS
{
    #region 枚举
    /// <summary>
    /// ERS类型
    /// </summary>
    public enum EType
    {
        GJ272_4
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
    #endregion

    #region 参数类
    /// <summary>
    /// 设置负载
    /// </summary>
    public class CERS_Load
    {
        /// <summary>
        /// 负载最大通道数
        /// </summary>
        private const int CHAN_MAX = 8;
        /// <summary>
        /// 串联电压值
        /// </summary>
        public double[] volt = new double[CHAN_MAX];
        /// <summary>
        /// 负载电流值
        /// </summary>
        public double[] cur = new double[CHAN_MAX];
    }
    #endregion

    #region 线程控制类
    /// <summary>
    /// ERS基础信息
    /// </summary>
    public class CERS_Base
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
    /// 读取负载设置
    /// </summary>
    public class CERS_ReadLoad
    {
        /// <summary>
        /// 操作标志
        /// </summary>
        public EOP op = EOP.空闲;
        /// <summary>
        /// 回读设置值
        /// </summary>
        public double[] LoadSet = new double[8];
    }
    /// <summary>
    /// 设置负载全部通道设置
    /// </summary>
    public class CERS_SetALLLoad
    {
        /// <summary>
        /// 操作标志
        /// </summary>
        public EOP op = EOP.空闲;
        /// <summary>
        /// 回读设置值
        /// </summary>
        public double[] LoadSet = new double[8];
        /// <summary>
        /// 保存于EPROM
        /// </summary>
        public bool saveEPROM = true;
    }
    /// <summary>
    /// 设置负载单通道设置
    /// </summary>
    public class CERS_SetCHLoad
    {
        /// <summary>
        /// 操作标志
        /// </summary>
        public EOP op = EOP.空闲;
        /// <summary>
        /// 负载电流
        /// </summary>
        public double loadVal = 0;
        /// <summary>
        /// 保存于EPROM
        /// </summary>
        public bool saveEPROM = true;
    }
    /// <summary>
    /// 负载快充设置
    /// </summary>
    public class CERS_QCM
    {
        /// <summary>
        /// 操作标志
        /// </summary>
        public EOP op = EOP.空闲;
        /// <summary>
        /// 上升电压
        /// </summary>
        public bool raise = true;
    }
    /// <summary>
    /// ERS测试参数
    /// </summary>
    public class CERS_Para
    {
        public CERS_Para()
        {
            for (int i = 0; i < 8; i++)
            {
                SetCHLoad.Add(new CERS_SetCHLoad());

                SetQCM.Add(new CERS_QCM()); 
            }
        }
        /// <summary>
        /// 串联电压
        /// </summary>
        public double[] Volt = new double[8]; 
        /// <summary>
        /// 回读电流
        /// </summary>
        public double[] Current = new double[8];
        /// <summary>
        /// 读取电流设定
        /// </summary>
        public CERS_ReadLoad ReadLoad = new CERS_ReadLoad();
        /// <summary>
        /// 设置负载通道
        /// </summary>
        public CERS_SetALLLoad SetAllLoad = new CERS_SetALLLoad();
        /// <summary>
        /// 设置负载单通道
        /// </summary>
        public List<CERS_SetCHLoad> SetCHLoad = new List<CERS_SetCHLoad>();
        /// <summary>
        /// 设置快充电压
        /// </summary>
        public List<CERS_QCM> SetQCM = new List<CERS_QCM>(); 
    }

    /// <summary>
    /// ERS类
    /// </summary>
    public class CERS
    {
        public CERS_Base Base = new CERS_Base();

        public CERS_Para Para = new CERS_Para();
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
