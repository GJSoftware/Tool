using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GJ.DEV.Mon
{
    public class CMONCom
    {
      #region 构造函数
      public CMONCom(EType monType, int idNo = 0, string name = "IMON")
      {
        this._idNo = idNo;

        this._name = name;

        this._monType = monType;

        //反射获取PLC类型

        string plcModule = "C" + monType.ToString();

        Assembly asb = Assembly.GetAssembly(typeof(IMON));

        Type[] types = asb.GetTypes();

        object[] parameters = new object[2];

        parameters[0] = _idNo;

        parameters[1] = _name;

        foreach (Type t in types)
        {
            if (t.Name == plcModule && t.GetInterface("IMON") != null)
            {
                _devMon = (IMON)asb.CreateInstance(t.FullName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);
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
      private EType _monType = EType.MON32;
      private IMON _devMon = null;
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
              if (_devMon == null)
              {
                  er = _devMon.ToString() + "未找到程序集,请检查";
                  return false;
              }
              if (!_devMon.Open(comName, out er, setting))
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
          if (_devMon == null)
              return;

          _conStatus = false;

          _devMon.Close();
      }
      /// <summary>
      /// 设置新地址
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetNewAddr(int wAddr, out string er)
      {
         return _devMon.SetNewAddr(wAddr, out er);  
      }
      /// <summary>
      /// 读版本
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="version"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadVersion(int wAddr, out string version, out string er)
      {
        return _devMon.ReadVersion(wAddr, out version, out er); 
      }
      /// <summary>
      /// 设定工作模式
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="wMode">
      /// 00：主控ACONOFF的工作模式 
      /// 01：从控ACONOFF的工作模式
      /// 02：ACStatus仅同步In+In-信号的工作模式
      /// 03：主控ACONOFF及可控制快充QC2.0模式
      /// 04: 主控快充模式-兼容海思模式
      /// </param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetWorkMode(int wAddr, int wMode, out string er)
      {
         return _devMon.SetWorkMode(wAddr, wMode, out er); 
      }
      /// <summary>
      /// 设置ON/OFF参数
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="wPara"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetOnOffPara(int wAddr, COnOffPara wPara, out string er)
      {
         return _devMon.SetOnOffPara(wAddr, wPara, out er);
      }
      /// <summary>
      /// 启动老化测试
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="wPara"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetRunStart(int wAddr, CwRunPara wPara, out string er)
      {
         return _devMon.SetRunStart(wAddr, wPara, out er); 
      }
      /// <summary>
      /// 强制结束
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ForceFinish(int wAddr, out string er)
      {
         return _devMon.ForceFinish(wAddr, out er); 
      }
      /// <summary>
      /// 启动AC ON/AC OFF
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool RemoteACOnOff(int wAddr, EOnOff wOnOff, out string er)
      {
         return _devMon.RemoteACOnOff(wAddr, wOnOff, out er); 
      }
      /// <summary>
      /// 控制通道Relay ON
      /// </summary>
      /// <param name="wAddr">iAdrs=0：为广播命令，iRlyNo=(1~32), 当iRlyN=101时,All Relay OFF</param>
      /// <param name="wOnOff"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetRelayOn(int wAddr, int wRelayNo, out string er)
      {
         return _devMon.SetRelayOn(wAddr, wRelayNo, out er); 
      }
      /// <summary>
      /// 控制暂停运行或继续运行
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="wContinue">1:继续 0:暂停</param>
      /// <param name="er"></par
      public bool ControlPauseOrContinue(int wAddr, int wContinue, out string er)
      {
          return _devMon.ControlPauseOrContinue(wAddr, wContinue, out er);
      }
      /// <summary>
      /// 回读ON/OFF参数
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="rPara"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadOnOffPara(int wAddr, ref COnOffPara rPara, out string er)
      {
         return _devMon.ReadOnOffPara(wAddr, ref rPara, out er); 
      }
      /// <summary>
      /// 从监控板读取电压及各个状态数据，电压数据基于同步AC ON/OFF信号
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="rVolt"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadVolt(int wAddr, out List<double> rVolt, out int rOnOff,out string er,
                           ESynON synNo = ESynON.同步, ERunMode mode = ERunMode.自动线模式)                                     
      {
          return _devMon.ReadVolt(wAddr, out rVolt, out rOnOff, out er, synNo,mode); 
      }
      /// <summary>
      /// 读取控制板测试信号数据
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="rPara"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadRunData(int wAddr, ref CrRunPara rPara, out string er)
      {
          return _devMon.ReadRunData(wAddr, ref rPara, out er); 
      }
      /// <summary>
      /// 发扫描命令
      /// </summary>
      public void SetScanAll()
      {
          _devMon.SetScanAll(); 
      }
      /// <summary>
      /// 设定快充参数
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="rPara"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetGJM_RunQC_Para(int wAddr, COnOffPara wPara, out string er)
      {
          return _devMon.SetGJM_RunQC_Para(wAddr, wPara, out er); 
      }
      /// <summary>
      /// 读取快充参数
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="wPara"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadGJM_RunQC_Para(int wAddr, ref COnOffPara rPara, out string er)
      {
          return _devMon.ReadGJM_RunQC_Para(wAddr, ref rPara, out er); 
      }
      /// <summary>
      /// 控制Y点ON/OFF
      /// </summary>
      /// <param name="wAddr">iAdrs=0：为广播命令，iRlyNo=(1~32), 当iRlyN=101时,All Relay OFF</param>
      /// <param name="wOnOff"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ControlY_OnOff(int wAddr, int wYno, int wOnOff, out string er)
      {
          return _devMon.ControlY_OnOff(wAddr, wYno, wOnOff, out er); 
      }
      #endregion
    }
}
