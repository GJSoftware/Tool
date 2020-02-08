using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GJ.DEV.Meter
{
    public class CMeterCom
    {
      #region 构造函数
      public CMeterCom(EType devType, int idNo = 0, string name = "IMeter")
      {
        this._idNo = idNo;

        this._name = name;

        this._devType = devType;

        //反射获取PLC类型

        string plcModule = "C" + devType.ToString();

        Assembly asb = Assembly.GetAssembly(typeof(IMeter));

        Type[] types = asb.GetTypes();

        object[] parameters = new object[2];

        parameters[0] = _idNo;

        parameters[1] = _name;

        foreach (Type t in types)
        {
            if (t.Name == plcModule && t.GetInterface("IMeter") != null)
            {
                _devMon = (IMeter)asb.CreateInstance(t.FullName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);
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
      private EType _devType = EType.PRU80_R1_2A_AC;
      private IMeter _devMon = null;
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
      public bool Open(string comName, out string er, string setting = "9600,n,8,1")
      {
         er = string.Empty;

          try
          {
              if (_devMon == null)
              {
                  er = _devType.ToString() + "未找到程序集,请检查";
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
        /// 读电压
        /// </summary>
        /// <param name="acv"></param>
        /// <param name="er"></param>
        /// <returns></returns>
      public bool ReadACV(int devAddr,out double acv, out string er)
      {
          return _devMon.ReadACV(devAddr, out acv, out er); 
      }
      /// <summary>
      /// 读电流
      /// </summary>
      /// <param name="acv"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadACI(int devAddr, out double aci, out string er)
      {
          return _devMon.ReadACI(devAddr, out aci, out er);
      }  
      #endregion
    }
}
