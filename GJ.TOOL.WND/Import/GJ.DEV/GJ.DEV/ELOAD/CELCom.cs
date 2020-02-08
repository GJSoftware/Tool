using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GJ.DEV.ELOAD
{
   public class CELCom
   {
      #region 构造函数
      public CELCom(EType monType, int idNo = 0, string name = "IEL")
      {
        this._idNo = idNo;

        this._name = name;

        this._monType = monType;

        //反射获取PLC类型

        string plcModule = "C" + monType.ToString();

        Assembly asb = Assembly.GetAssembly(typeof(IEL));

        Type[] types = asb.GetTypes();

        object[] parameters = new object[2];

        parameters[0] = _idNo;

        parameters[1] = _name;

        foreach (Type t in types)
        {
            if (t.Name == plcModule && t.GetInterface("IEL") != null)
            {
                _devMon = (IEL)asb.CreateInstance(t.FullName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);
                break;
            }
        }
        if (_devMon != null)
        {
            _maxCH = _devMon.maxCH; 
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
      private EType _monType = EType.EL_20_16;
      private IEL _devMon = null;
      #endregion

      #region 属性
      /// <summary>
      /// 编号
      /// </summary>
      public int idNo
      {
          set { _idNo = value; }
          get { return _idNo; }
      }
      /// <summary>
      /// 名称
      /// </summary>
      public string name
      {
          set { _name = value; }
          get { return _name; }
      }
      /// <summary>
      /// 连接状态
      /// </summary>
      public bool conStatus
      {
          get { return _conStatus; }
      }
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
      /// <param name="er"></param>
      /// <param name="setting"></param>
      /// <returns></returns>
      public bool Open(string comName, out string er, string setting = "57600,n,8,1")
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
      /// 设置负载
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="wDataSet"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetELData(int wAddr, CEL_SetPara wDataSet, out string er)
      {
          return _devMon.SetELData(wAddr, wDataSet, out er);  
      }
      /// <summary>
      /// 读取负载设置
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="rLoadVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadELLoadSet(int wAddr, CEL_ReadSetPara rDataSet, out string er)
      {
          return _devMon.ReadELLoadSet(wAddr, rDataSet, out er); 
      }
      /// <summary>
      /// 读取负载数值
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="rDataVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadELData(int wAddr, CEL_ReadData rDataVal, out string er)
      {
          return _devMon.ReadELData(wAddr, rDataVal, out er);  
      }
      #endregion

   }

}
