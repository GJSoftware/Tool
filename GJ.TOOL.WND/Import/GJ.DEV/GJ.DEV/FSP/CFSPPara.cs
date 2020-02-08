using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.FSP
{
    #region 枚举
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
        读取NG,
        写入,
        写入OK,
        写入NG
    }
    /// <summary>
    /// 状态
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
    /// 操作类型
    /// </summary>
    public enum EOPTYPE
    {
      空闲状态,
      读取输出参数,
      设置输出参数,
      设置开关机
    }
    #endregion

    #region 事件定义
    public class CConArgs : EventArgs
    {
        public readonly int idNo;
        public readonly string name;
        public readonly string conStatus;
        public readonly bool bErr;
        public CConArgs(int idNo, string name, string conStatus, bool bErr = false)
        {
            this.idNo = idNo;
            this.name = name;
            this.conStatus = conStatus;
            this.bErr = bErr;
        }
    }
    public class CDataArgs : EventArgs
    {
        public readonly int idNo;
        public readonly string name;
        public readonly string rData;
        public readonly bool bErr;
        public readonly bool bComplete;
        public CDataArgs(int idNo, string name, string rData, bool bComplete = true, bool bErr = false)
        {
            this.idNo = idNo;
            this.name = name;
            this.rData = rData;
            this.bComplete = bComplete;
            this.bErr = bErr;
        }
    }
    public class COPArgs : EventArgs
    {
        public readonly int Addr = 0;

        public readonly EOPTYPE Op_Type=EOPTYPE.空闲状态;

        public readonly bool Status=false;

        public readonly string AlarmCode = string.Empty;

        public readonly int lPara=0;

        public readonly int wPara=0;

        public COPArgs(int addr, EOPTYPE op_Type, bool status,string alarmCode="通信异常", int lPara=0,int wPara=0)
        {
            this.Addr = addr;
            this.Op_Type = op_Type;
            this.Status = status;
            this.AlarmCode = alarmCode;
            this.lPara = lPara;
            this.wPara = wPara;
        }
    }
    #endregion

    #region 类定义
    /// <summary>
    /// 模块信息
    /// </summary>
    public class CModule
      {
          /// <summary>
          /// 复制
          /// </summary>
          /// <returns></returns>
          public CModule Clone()
          {
              CModule module = new CModule();

              module.Volt = this.Volt;

              module.Current = this.Current;

              module.Temp = this.Temp;

              module.FanSpeed1 = this.FanSpeed1;

              module.FanSpeed2 = this.FanSpeed2;

              module.Status = this.Status;

              module.Alarm = this.Alarm;

              return module;
          }
          /// <summary>
          /// 输出电压
          /// </summary>
          public double Volt = 0;
          /// <summary>
          /// 模块输出电流
          /// </summary>
          public double Current = 0;
          /// <summary>
          /// 内部温度
          /// </summary>
          public double Temp = 0;
          /// <summary>
          /// 风扇1转速
          /// </summary>
          public int FanSpeed1 = 0;
          /// <summary>
          /// 风扇2转速
          /// </summary>
          public int FanSpeed2 = 0;
          /// <summary>
          /// 模块告警量
          /// </summary>
          public string Status = string.Empty;
          /// <summary>
          /// 模块保护类型
          /// </summary>
          public string Alarm = string.Empty;
      }
    /// <summary>
    /// 基础信息
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
    /// 参数
    /// </summary>
    public class CPara
    {
        public CModule Module = new CModule();
    }
    /// <summary>
    /// 读取参数
    /// </summary>
    public class COP_ReadPara
    {
        /// <summary>
        /// 操作标志
        /// </summary>
        public EOP Op = EOP.空闲;
        /// <summary>
        /// 失败持续发送
        /// </summary>
        public bool bContinue = true;
        /// <summary>
        /// 设定电压
        /// </summary>
        public double Volt = 0;
        /// <summary>
        /// 设定电流
        /// </summary>
        public double Current = 0;
    }
        /// <summary>
    /// 设置参数
    /// </summary>
    public class COP_WritePara
    {
        /// <summary>
        /// 操作标志
        /// </summary>
        public EOP Op = EOP.空闲;
        /// <summary>
        /// 失败持续发送
        /// </summary>
        public bool bContinue = true;
        /// <summary>
        /// 设定电压
        /// </summary>
        public double Volt = 0;
        /// <summary>
        /// 设定电流
        /// </summary>
        public double Current = 0;
    }
    /// <summary>
    /// 设置关机
    /// </summary>
    public class COP_PowerOff
    {
        /// <summary>
        /// 操作标志
        /// </summary>
        public EOP Op = EOP.空闲;
        /// <summary>
        /// 失败持续发送
        /// </summary>
        public bool bContinue = true;
        /// <summary>
        /// 关机->1;开机->0;
        /// </summary>
        public int PowerOff = 1;
    }
    /// <summary>
    /// 监控类
    /// </summary>
    public class CMon
    {
        public CBase Base = new CBase();

        public CPara Para = new CPara();

        public COP_ReadPara ReadPara = new COP_ReadPara();

        public COP_WritePara WritePara = new COP_WritePara();

        public COP_PowerOff PowerOff = new COP_PowerOff();
    }
    #endregion
}
