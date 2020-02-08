using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GJ.COM;
namespace GJ.DEV.FCMB
{
    public class CFMBCom
    {
      #region 构造函数
      public CFMBCom(EType fcmbType, int idNo = 0, string name = "IFMB")
      {
        this._idNo = idNo;

        this._name = name;

        this._fcmbType = fcmbType;

        //反射获取PLC类型

        string plcModule = "C" + fcmbType.ToString();

        Assembly asb = Assembly.GetAssembly(typeof(IFCMB));

        Type[] types = asb.GetTypes();

        object[] parameters = new object[2];

        parameters[0] = _idNo;

        parameters[1] = _name;

        foreach (Type t in types)
        {
            if (t.Name == plcModule && t.GetInterface("IFCMB") != null)
            {
                _devFCMB = (IFCMB)asb.CreateInstance(t.FullName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);
                break;
            }
        }
      }
      public override string ToString()
      {
          return _name;
      }
      #endregion
   
      #region 字段
      private int _idNo = 0;
      private string _name = string.Empty;
      private bool _conStatus = false;
      private EType _fcmbType = EType.FMB_V1;
      private IFCMB _devFCMB = null;
      #endregion

      #region 属性
      /// <summary>
      /// 编号
      /// </summary>
      public int idNo
      {
          set
          {
              _idNo = value;
          }
          get
          {
              return _idNo;
          }
      }
      /// <summary>
      /// 名称
      /// </summary>
      public string name
      {
         set
         {
             _name = value;  
         }
         get
         {
             return _name;  
         }
      }
      /// <summary>
      /// 连接状态
      /// </summary>
      public bool conStatus
      {
          get {
              return _conStatus;
              }
      }
      #endregion

      #region 方法
      /// <summary>
      /// 打开串口
      /// </summary>
      /// <param name="comName"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Open(string comName, out string er, string setting = "57600,n,8,1")
      {
          er = string.Empty;

          try
          {
              if (_devFCMB == null)
              {
                  er = _devFCMB.ToString() + CLanguage.Lan("未找到程序集,请检查");
                  return false;
              }
              if (!_devFCMB.Open(comName, out er, setting))
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
          if (_devFCMB == null)
              return;

          _conStatus = false;

          _devFCMB.Close();
      }
      /// <summary>
      /// 读线圈和寄存器值
      /// 从机地址(1Byte)+功能码(1Byte)+寄存器地址(2Byte)+地址数量(2Byte)+CRC检验(2Byte)
      /// </summary>
      /// <param name="devType">地址类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="N">地址长度</param>
      /// <param name="rData">16进制字符:数据值高位在前,低位在后</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int devAddr, ERegType regType, int startAddr, int N, out string rData, out string er)
      {
          return _devFCMB.Read(devAddr, regType, startAddr, N, out rData, out er);
      }
      /// <summary>
      /// 返回单个数值
      /// </summary>
      /// <param name="devType"></param>
      /// <param name="startAddr"></param>
      /// <param name="N"></param>
      /// <param name="rVal">开始地址值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int devAddr, ERegType regType, int startAddr, out int rVal, out string er)
      {
          return _devFCMB.Read(devAddr, regType, startAddr, out rVal, out er);
      }
      /// <summary>
      /// 返回数值
      /// </summary>
      /// <param name="devType"></param>
      /// <param name="startAddr"></param>
      /// <param name="N"></param>
      /// <param name="rVal">地址N值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int devAddr, ERegType regType, int startAddr, ref int[] rVal, out string er)
      {
          return _devFCMB.Read(devAddr, regType, startAddr, ref rVal, out er);
      }
      /// <summary>
      /// 单写线圈和寄存器值
      /// 从机地址(1Byte)+功能码(1Byte)+寄存器地址(2Byte)+地址数量(2Byte)+字节数(1Byte)+数据+CRC检验(2Byte)
      /// </summary>
      /// <param name="devType">地址类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="wVal">单个值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Write(int devAddr, ERegType regType, int startAddr, int wVal, out string er)
      {
          return _devFCMB.Write(devAddr, regType, startAddr, wVal, out er);
      }
      /// <summary>
      /// 写多个线圈和寄存器
      /// </summary>
      /// <param name="devType">地址类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="wVal">多个值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Write(int devAddr, ERegType regType, int startAddr, int[] wVal, out string er)
      {
          return _devFCMB.Write(devAddr, regType, startAddr, wVal, out er);
      }
      #endregion

      #region 专用功能
      /// <summary>
      /// 读设备名称
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="name"></param>
      /// <returns></returns>
      public bool ReadName(int addr, out string name, out string er)
      { 
         return _devFCMB.ReadName(addr,out name,out er); 
      }
      /// <summary>
      /// 读设备序列号
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="sn"></param>
      /// <returns></returns>
      public bool ReadSn(int addr, out string sn, out string er)
      {
          return _devFCMB.ReadSn(addr, out sn, out er); 
      }
      /// <summary>
      /// 读地址D8000
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadAddr(out int curAddr, out string er)
      {
          return _devFCMB.ReadAddr(out curAddr, out er); 
      }
      /// <summary>
      /// 设置地址D8000
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetAddr(int curAddr, out string er)
      {
          return _devFCMB.SetAddr(curAddr, out er);
      }
      /// <summary>
      /// 读取版本号D8001
      /// </summary>
      /// <param name="rVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadVersion(int addr, out int rVal, out string er)
      {
          return _devFCMB.ReadVersion(addr, out rVal, out er);
      }
      /// <summary>
      /// 读取版本号D8001
      /// </summary>
      /// <param name="rVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadVersion(int addr, out string rVal, out string er)
      {
          return _devFCMB.ReadVersion(addr, out rVal, out er); 
      }
      /// <summary>
      /// 读波特率D8002
      /// </summary>
      /// <param name="baud"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadBaud(int addr, out int baud, out string er)
      {
          return _devFCMB.ReadBaud(addr, out baud, out er);  
      }
      /// <summary>
      /// 设置波特率D8002
      /// </summary>
      /// <param name="baud"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetBaud(int addr, int baud, out string er)
      {
          return _devFCMB.SetBaud(addr, baud, out er); 
      }
      /// <summary>
      /// 设置小板地址D8003
      /// </summary>
      /// <param name="addr">要求中位拨码开关为 on</param>
      /// <param name="childAddr">小板的开关需要为 on</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetChildAddr(int childAddr, out string er)
      {
          return _devFCMB.SetChildAddr(childAddr, out er);  
      }
      /// <summary>
      /// 当小板通信不上时值为 0，正常大于 0
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="childVer"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadChildVersion(int addr,int childMax, out List<int> childVer, out string er)
      {
          return _devFCMB.ReadChildVersion(addr, childMax, out childVer, out er);  
      }
      /// <summary>
      /// 快充模式配置D8007
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="qcm"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetQCM(int addr, EQCM qcm, double qcv, double qci, out string er, bool cc2=false)
      {
          return _devFCMB.SetQCM(addr, qcm, qcv, qci, out er, cc2);
      }
      /// <summary>
      /// 快充模式配置D8007
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="qcm"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadQCM(int addr, out EQCM qcm, out double qcv, out double qci, out string er)
      {
          return _devFCMB.ReadQCM(addr, out qcm,out qcv,out qci, out er); 
      }
      /// <summary>
      /// 时序控制模式D8070
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="mode"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetOnOffMode(int addr, EOnOffMode mode, out string er)
      {
          return _devFCMB.SetOnOffMode(addr, mode, out er);
      }
      /// <summary>
      /// 读取时序控制模式D8070
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="mode"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadOnOffMode(int addr, out EOnOffMode mode, out string er)
      {
          return _devFCMB.ReadOnOffMode(addr, out mode, out er); 
      }
      /// <summary>
      /// 时序总时间(Min)D8071
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="runMin"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetTotalTime(int addr, int runMin, out string er)
      {
          return _devFCMB.SetTotalTime(addr, runMin, out er); 
      }
      /// <summary>
      /// 读取总时间(Min)
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="runMin"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadTotalTime(int addr, out int runMin, out string er)
      {
          return _devFCMB.ReadTotalTime(addr, out runMin, out er); 
      }
      /// <summary>
      /// 设置时序段D8050
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="OnOff">
      /// 每 3 个数据为一组，分别为 on 时间、off 时间和重复次数
      /// 总共 10 组，时间单位为秒</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetOnOffTime(int addr, List<COnOff> OnOff, out string er)
      {
          return _devFCMB.SetOnOffTime(addr, OnOff, out er); 
      }
      /// <summary>
      /// 读取时序段D8050
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="OnOff"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadOnOffTime(int addr, out List<COnOff> OnOff, out string er)
      {
          return _devFCMB.ReadOnOffTime(addr, out OnOff, out er); 
      }
      /// <summary>
      /// 直接操作时序开关D806F
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="wOnOff"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetACON(int addr, int wOnOff, out string er,bool synC=false)
      {
          return _devFCMB.SetACON(addr, wOnOff, out er, synC); 
      }
      /// <summary>
      /// 控制AC ON/OFF
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="wOnOff"></param>
      /// <param name="er"></param>
      /// <param name="synC"></param>
      /// <param name="B400"></param>
      /// <returns></returns>
      public bool SetACON(int addr, int wOnOff, out string er, bool synC = false, bool B400 = false)
      {
          return _devFCMB.SetACON(addr, wOnOff, out er, synC, B400); 
      }
      /// <summary>
      /// 读取时序开关D806F
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="rOnOff">
      /// 0：控制开
      /// 1：控制关
      /// 3：读取状态
      /// </param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadACON(int addr, out int rOnOff, out string er)
      {
          return _devFCMB.ReadACON(addr, out rOnOff, out er); 
      }
      /// <summary>
      /// 读取IO信号D8179
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="io"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadIO(int addr, out List<int> io, out string er)
      {
          return _devFCMB.ReadIO(addr, out io, out er); 
      }
      /// <summary>
      /// 设置IO-D817A
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="ioType"></param>
      /// <param name="wOnOff"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetIO(int addr, EFMB_wIO ioType, int wOnOff, out string er)
      {
          return _devFCMB.SetIO(addr, ioType, wOnOff, out er);
      }
      /// <summary>
      /// 读取产品电压D8128
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="volt">5000 表示5V</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadVolt(int addr, out List<double> volt, out string er, bool sync = true, EVMODE voltMode = EVMODE.VOLT_40)
      {
          return _devFCMB.ReadVolt(addr, out volt, out er, sync, voltMode); 
      }
      /// <summary>
      /// 读取AC电压
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="acv">单位 0.1V，2200 表示 220.0V</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadAC(int addr, out double acv, out string er)
      {
          return _devFCMB.ReadAC(addr, out acv, out er); 
      }
      /// <summary>
      /// 读取测试D+、D-短路情况
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="chan">1-32</param>
      /// <param name="value">0:OK-->大于零:自测 D+对GND、D-对GND、D+对D-、D-对V+</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadDGND(int addr, int chan, out string status, out string er)
      {
          return _devFCMB.ReadDGND(addr, chan, out status, out er);
      }
      /// <summary>
      /// 读取测试D+、D-短路情况
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="chan">1-32</param>
      /// <param name="value">0:OK-->大于零:自测 D+对GND、D-对GND、D+对D-、D-对V+</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadDGND(int addr, int chan, out List<int> status, out string er)
      {
          return _devFCMB.ReadDGND(addr, chan, out status, out er);
      }
      /// <summary>
      /// 设置Y0-Y7
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="Y"></param>
      /// <param name="wOnOff"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetIO(int addr, EFMB_Y Y, int wOnOff, out string er)
      {
          return _devFCMB.SetIO(addr, Y, wOnOff, out er);
      }
      #endregion
    }
}
