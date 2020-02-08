using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GJ.DEV.LED
{
    public class CLEDCom
    {
      #region 构造函数
      public CLEDCom(EType monType, int idNo = 0, string name = "ILED")
      {
        this._idNo = idNo;

        this._name = name;

        this._monType = monType;

        //反射获取PLC类型

        string plcModule = "C" + monType.ToString();

        Assembly asb = Assembly.GetAssembly(typeof(ILED));

        Type[] types = asb.GetTypes();

        object[] parameters = new object[2];

        parameters[0] = _idNo;

        parameters[1] = _name;

        foreach (Type t in types)
        {
            if (t.Name == plcModule && t.GetInterface("ILED") != null)
            {
                _devMon = (ILED)asb.CreateInstance(t.FullName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);

                _maxCH = _devMon.maxCH;

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
      private int _maxCH = 8;
      private EType _monType = EType.DA_320_8;
      private ILED _devMon = null;
      #endregion

      #region 属性
      /// <summary>
      /// 编号
      /// </summary>
      public int idNo
      {
          set{_idNo = value;}
          get{return _idNo;}
      }
      /// <summary>
      /// 名称
      /// </summary>
      public string name
      {
         set{_name = value;}
         get{return _name;}
      }
      /// <summary>
      /// 连接状态
      /// </summary>
      public bool conStatus
      {
          get { return _conStatus; }
      }
      /// <summary>
      /// 负载通道
      /// </summary>
      public int maxCH
      {
          get { return _maxCH; }
      }
      #endregion

      #region 方法
      /// <summary>
      /// 打开串口
      /// </summary>
      /// <param name="comName"></param>
      /// <param name="setting"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Open(string comName, out string er, string setting)
      {
          er = string.Empty;

          try
          {
              if (_devMon == null)
                  return false;

              if (!_devMon.Open(comName, out er, setting))
                  return false;

              _maxCH = _devMon.maxCH;

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
      /// <returns></returns>
      public void Close()
      {
          try
          {
              if (_devMon == null)
                  return;

              _devMon.Close();

              _conStatus = false;
          }
          catch (Exception)
          {

              throw;
          }         
      }
      /// <summary>
      /// 设置地址
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
      /// 读取负载设置
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="chanList"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadLoadSetting(int wAddr, out List<CLOAD> chanList, out string er)
      {
          return _devMon.ReadLoadSetting(wAddr, out chanList, out er);  
      }
      /// <summary>
      /// 设置8个电流通道负载
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="chanList"></param>
      /// <param name="saveEEPROM"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetLoadValue(int wAddr, List<CLOAD> chanList, bool saveEEPROM, out string er)
      {
          return _devMon.SetLoadValue(wAddr, chanList, saveEEPROM, out er);
      }
      /// <summary>
      /// 设置8个电流通道负载
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="chanList"></param>
      /// <param name="saveEEPROM"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetLoadValue(int wAddr, int chanNo, CLOAD chanPara, bool saveEEPROM, out string er)
      {
          return _devMon.SetLoadValue(wAddr, chanNo,chanPara, saveEEPROM, out er);  
      }
      /// <summary>
      /// 读取负载数据
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="data"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadLoadValue(int wAddr, ref CData data, out string er)
      {
          return _devMon.ReadLoadValue(wAddr, ref data, out er); 
      }
      #endregion

    }
}
