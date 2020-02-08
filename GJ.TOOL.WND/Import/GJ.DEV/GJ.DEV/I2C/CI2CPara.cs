using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.I2C
{
    #region 枚举
    /// <summary>
    /// 类型
    /// </summary>
    public enum EType
    {
        I2C_Server
    }
    #endregion

    #region 内部类
    /// <summary>
    /// 产品摆放位置
    /// </summary>
    public enum EPlace
    {
      没任何产品,
      只有左边产品,
      只有右边产品,
      两边都有产品
    }
    /// <summary>
    /// 读取方式
    /// </summary>
    public enum EReadType
    {
      不同步OnOff信号,
      同步OnOff信号,
      通讯下命令6读取
    }
    /// <summary>
    /// 机型类型
    /// </summary>
    public enum EModel
    {
      通用机种,
      HP机种
    }
    /// <summary>
    /// 命令格式
    /// </summary>
    public class CI2C_Cmd
    {
        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public CI2C_Cmd Clone()
        {
            CI2C_Cmd para = new CI2C_Cmd();

            para.CmdOP = this.CmdOP;

            para.RegNo = this.RegNo;

            return para;
        }
        /// <summary>
        /// I2C的操作命令 （要求为1个Hex字符）
        /// </summary>
        public string CmdOP = string.Empty;
        /// <summary>
        /// 操作命令对应的寄存器 （要求为1个Hex字符）
        /// </summary>
        public string RegNo = string.Empty;
    }
    /// <summary>
    /// I2C测试类
    /// </summary>
    public class CI2C_RunPara
    {
       public CI2C_RunPara()
       {
           for (int i = 0; i < 20; i++)
               Cmd.Add(new CI2C_Cmd());  
       }
       /// <summary>
       /// 复制 
       /// </summary>
       /// <returns></returns>
       public CI2C_RunPara Clone()
       {
           CI2C_RunPara para = new CI2C_RunPara();

           para.PlaceType = this.PlaceType;

           para.ReadType = this.ReadType;

           para.ScanCycle = this.ScanCycle;

           para.ACONDelay = this.ACONDelay;

           para.RunI2CType = this.RunI2CType;

           para.RdCmdNum = this.RdCmdNum;

           para.I2C_Addr = this.I2C_Addr;

           for (int i = 0; i < 20; i++)
               para.Cmd[i] = this.Cmd[i].Clone();

           return para;
       }
      /// <summary>
      /// 产品摆放位置
      /// </summary>
      public EPlace PlaceType=EPlace.没任何产品;
      /// <summary>
      /// 读取方式
      /// </summary>
      public EReadType ReadType=EReadType.不同步OnOff信号;
      /// <summary>
      /// 扫描间隔时间， 范围(1~30)Sec  默认5Sec
      /// </summary>
      public int ScanCycle=5;
      /// <summary>
      /// AC ON 产品启动的延时: 范围(1~30)Sec 默认3Sec
      /// </summary>
      public int ACONDelay=3;
      /// <summary>
      /// I2C 的协议类型,0->通用机种，1->HP机种
      /// </summary>
      public EModel RunI2CType=EModel.通用机种; 
      /// <summary>
      /// 需执行I2C读取命令的数量总计
      /// </summary>
      public int RdCmdNum =0;
      /// <summary>
      /// I2C的地址 （要求为1个Hex字符）
      /// </summary>
      public string I2C_Addr="BE";
      /// <summary>
      /// 命令格式
      /// </summary>
      public List<CI2C_Cmd> Cmd = new List<CI2C_Cmd>(); 
    }
    /// <summary>
    /// 命令数据
    /// </summary>
    public class CI2C_Val
    {  
        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public CI2C_Val Clone()
        {
            CI2C_Val para = new CI2C_Val();

            para.CmdNo = this.CmdNo;

            para.RdByte0 = this.RdByte0;

            para.RdByte1 = this.RdByte1;

            para.RdStatus = this.RdStatus;

            return para;
        }
        /// <summary>
        /// 命令编号
        /// </summary>
        public int CmdNo = 0;
        /// <summary>
        /// 命令状态
        /// 0:正常
        /// 1:通信错误
        /// 2:从地址没响应
        /// 3-10:命令错误
        /// 0xFA:从机校验和错误
        /// 0xFB:主机校验和错误
        /// </summary>
        public int RdStatus = 0;
        /// <summary>
        /// 接收数据字节0
        /// </summary>
        public int RdByte0 = 0;
        /// <summary>
        /// 接收数据字节1
        /// </summary>
        public int RdByte1 = 0; 
    }
    /// <summary>
    /// 读取数据
    /// </summary>
    public class CI2C_Data
    {
      public CI2C_Data()
      {
          for (int i = 0; i < 20; i++)
              Val.Add(new CI2C_Val());    
      }
      /// <summary>
      /// 复制
      /// </summary>
      /// <returns></returns>
      public CI2C_Data Clone()
      {
          CI2C_Data para = new CI2C_Data();

          para.CmdNum = this.CmdNum;

          para.AC_ONOFF = this.AC_ONOFF;

          for (int i = 0; i < 20; i++)
              para.Val[i] = this.Val[i].Clone();

          return para;
      }
      /// <summary>
      /// I2C读取命令的数量总计
      /// </summary>
      public int CmdNum=0;
      /// <summary>
      /// 同步信号
      /// </summary>
      public int AC_ONOFF=0;
      /// <summary>
      /// 命令数据
      /// </summary>
      public List<CI2C_Val> Val = new List<CI2C_Val>(); 
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
    public class CI2C_Base
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
    public class CI2C_Para
    {
        public CI2C_Para()
        {
            RunData.Add(new CI2C_Data());

            RunData.Add(new CI2C_Data()); 
        }

        public EOP rRunPara_OP = EOP.空闲;

        public EOP wRunPara_OP = EOP.空闲; 

        public CI2C_RunPara rRunPara = new CI2C_RunPara();

        public CI2C_RunPara wRunPara = new CI2C_RunPara();

        public List<CI2C_Data> RunData = new List<CI2C_Data>();
    }
    /// <summary>
    /// I2C板
    /// </summary>
    public class CI2C
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        public CI2C_Base Base = new CI2C_Base();
        /// <summary>
        /// 参数信息
        /// </summary>
        public CI2C_Para Para = new CI2C_Para();

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
