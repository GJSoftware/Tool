using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GJ.DEV.I2C
{
    public class CI2CCom
    {
      #region 构造函数
      public CI2CCom(EType monType, int idNo = 0, string name = "II2C")
      {
        this._idNo = idNo;

        this._name = name;

        this._monType = monType;

        //反射获取PLC类型

        string plcModule = "C" + monType.ToString();

        Assembly asb = Assembly.GetAssembly(typeof(II2C));

        Type[] types = asb.GetTypes();

        object[] parameters = new object[2];

        parameters[0] = _idNo;

        parameters[1] = _name;

        foreach (Type t in types)
        {
            if (t.Name == plcModule && t.GetInterface("II2C") != null)
            {
                _devMon = (II2C)asb.CreateInstance(t.FullName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);
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
      private EType _monType = EType.I2C_Server;
      private II2C _devMon = null;
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
      /// 设置I2C运行参数
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="para"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SendToSetI2C_RunPara(int wAddr, CI2C_RunPara para, out string er)
      {
          return _devMon.SendToSetI2C_RunPara(wAddr, para, out er);  
      }
      /// <summary>
      /// 读取I2C运行参数
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="para"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadI2C_RunPara(int wAddr, ref CI2C_RunPara para, out string er)
      {
          return _devMon.ReadI2C_RunPara(wAddr, ref para, out er); 
      }
      /// <summary>
      /// 读取I2C数据
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="uutNo">产品1或产品2</param>
      /// <param name="data"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadI2C_Data(int wAddr, int uutNo, ref CI2C_Data data, out string er)
      {
          return _devMon.ReadI2C_Data(wAddr, uutNo, ref data, out er);
      }
      #endregion
    }
}
