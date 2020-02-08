using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.DD
{
    #region 枚举
     /// <summary>
    /// 类型
    /// </summary>
    public enum EType
    {
        DD_35V,
        DD_75V
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

    #region 设备参数
    /// <summary>
    /// 设置负载
    /// </summary>
    public class CwLoad
    {
        public CwLoad Clone()
        {
            CwLoad para = new CwLoad();

            para.saveEEPROM = this.saveEEPROM;

            for (int i = 0; i < 8; i++)
                para.loadVal[i] = this.loadVal[i];

            return para;

        }
        public int saveEEPROM = 1;
        public double[] loadVal = new double[8];
    }
    /// <summary>
    /// 读取负载设置
    /// </summary>
    public class CrLoad
    {
        public CrLoad Clone()
        {
            CrLoad para = new CrLoad();

            for (int i = 0; i < 8; i++)
                para.loadVal[i] = this.loadVal[i];

            return para;
        }

        public double[] loadVal = new double[8];
    }
    /// <summary>
    /// 读取数据
    /// </summary>
    public class CrData
    {
        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public CrData Clone()
        {
            CrData para = new CrData();
            for (int i = 0; i < 12; i++)
                para.Volt[i] = this.Volt[i];
            for (int i = 0; i < 8; i++)
                para.Cur[i] = this.Cur[i];
            para.S1 = this.S1;
            para.OnOff = this.OnOff; 
            para.PS_On = this.PS_On;
            para.OTP = this.OTP; 
            para.OVP = this.OVP;
            para.OPP = this.OPP;
            para.FanErr = this.FanErr;
            para.Status = this.Status;  
            return para;
        }
        public double[] Volt = new double[12];
        public double[] Cur = new double[8];
        public int S1 = 0;
        public int OnOff = 0;
        public int PS_On = 0;
        public int OTP = 0;
        public int OVP = 0;
        public int OPP = 0;
        public int FanErr = 0;
        public string Status;
    }
    #endregion

    #region 线程参数
    /// <summary>
    /// 基础信息
    /// </summary>
    public class CDD_Base
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
    /// 测试变量
    /// </summary>
    public class CDD_Para
    {
        public CDD_Para Clone()
        {
            CDD_Para para = new CDD_Para();

            para.rLoad_OP = this.rLoad_OP;

            para.wLoad_OP = this.wLoad_OP;

            para.LoadRead = this.LoadRead.Clone();

            para.LoadSet = this.LoadSet.Clone();

            para.Data = this.Data.Clone();

            return para;
        }

        public EOP rLoad_OP = EOP.空闲;

        public EOP wLoad_OP = EOP.空闲;

        public CrLoad LoadRead = new CrLoad();

        public CwLoad LoadSet = new CwLoad();

        public CrData Data = new CrData();
    }
    /// <summary>
    /// DD类
    /// </summary>
    public class CDD
    {
        public CDD_Base Base = new CDD_Base();

        public CDD_Para Para = new CDD_Para();

        public override string ToString()
        {
            return Base.name;
        }
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
