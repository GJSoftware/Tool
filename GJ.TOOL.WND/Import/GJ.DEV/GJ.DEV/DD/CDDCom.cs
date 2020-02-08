using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GJ.DEV.DD
{
    public class CDDCom
    {

      #region 构造函数
      public CDDCom(EType monType, int idNo = 0, string name = "IDD")
      {
        this._idNo = idNo;

        this._name = name;

        this._monType = monType;

        //反射获取PLC类型

        string plcModule = "C" + monType.ToString();

        Assembly asb = Assembly.GetAssembly(typeof(IDD));

        Type[] types = asb.GetTypes();

        object[] parameters = new object[2];

        parameters[0] = _idNo;

        parameters[1] = _name;

        foreach (Type t in types)
        {
            if (t.Name == plcModule && t.GetInterface("IDD") != null)
            {
                _devMon = (IDD)asb.CreateInstance(t.FullName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);
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
      private EType _monType = EType.DD_35V; 
      private IDD _devMon = null;
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
         get {return _conStatus;}
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
      public bool Open(string comName, out string er,string setting ="57600,n,8,1")
      {
          er = string.Empty;

          try
          {
              if (_devMon == null)
                  return false;

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
      /// <param name="loadPara"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetNewLoad(int wAddr, CwLoad loadPara, out string er)
      {
          return _devMon.SetNewLoad(wAddr, loadPara, out er); 
      }
      /// <summary>
      /// 设置负载
      /// </summary>
      /// <param name="wStartAddr"></param>
      /// <param name="wEndAddr"></param>
      /// <param name="loadPara"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetNewLoad(int wStartAddr, int wEndAddr, CwLoad loadPara, out string er)
      {
          return _devMon.SetNewLoad(wStartAddr, wEndAddr, loadPara, out er); 
      }
      /// <summary>
      /// 读取负载设置
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="loadSet"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadLoadSet(int wAddr,ref CrLoad loadSet, out string er)
      {
          return _devMon.ReadLoadSet(wAddr,ref loadSet, out er); 
      }
      /// <summary>
      /// 读取数据
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="rData"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadData(int wAddr,ref CrData rData, out string er)
      {
          return _devMon.ReadData(wAddr, ref rData, out er); 
      }
      /// <summary>
      /// 设置PS_ON
      /// </summary>
      /// <param name="wStartAddr"></param>
      /// <param name="wEndAddr"></param>
      /// <param name="wOnOff"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetPS_ON(int wStartAddr, int wEndAddr, int wOnOff, out string er)
      {
          return _devMon.SetPS_ON(wStartAddr, wEndAddr, wOnOff, out er);   
      }
      #endregion

    }
}
