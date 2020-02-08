using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GJ.DEV.ERS
{
   public class CERSCom
   {
      #region 构造函数
      public CERSCom(EType monType, int idNo = 0, string name = "IERS")
      {
        this._idNo = idNo;

        this._name = name;

        this._monType = monType;

        //反射获取PLC类型

        string plcModule = "C" + monType.ToString();

        Assembly asb = Assembly.GetAssembly(typeof(IERS));

        Type[] types = asb.GetTypes();

        object[] parameters = new object[2];

        parameters[0] = _idNo;

        parameters[1] = _name;

        foreach (Type t in types)
        {
            if (t.Name == plcModule && t.GetInterface("IERS") != null)
            {
                _devMon = (IERS)asb.CreateInstance(t.FullName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);

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
      private int _maxCH = 4;
      private EType _monType = EType.GJ272_4;
      private IERS _devMon = null;
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
       /// <param name="er"></param>
       /// <param name="setting"></param>
       /// <returns></returns>
       public bool Open(string comName, out string er, string setting = "9600,n,8,1")
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
       /// 读取版本
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
       /// 设置负载全部通道电流
       /// </summary>
       /// <param name="wAddr"></param>
       /// <param name="loadPara"></param>
       /// <param name="er"></param>
       /// <param name="saveEPROM"></param>
       /// <returns></returns>
       public bool SetNewLoad(int wAddr, CERS_Load loadPara, out string er, bool saveEPROM = true)
       {
           return _devMon.SetNewLoad(wAddr, loadPara, out er, saveEPROM);
       }
       /// <summary>
       /// 设置负载单通道电流
       /// </summary>
       /// <param name="wAddr"></param>
       /// <param name="CH"></param>
       /// <param name="loadVal"></param>
       /// <param name="er"></param>
       /// <param name="saveEPROM"></param>
       /// <returns></returns>
       public bool SetNewLoad(int wAddr, int CH, double loadVal, out string er, bool saveEPROM = true)
       {
           return _devMon.SetNewLoad(wAddr, CH, loadVal, out er, saveEPROM);
       }
       public bool ReadLoadSet(int wAddr, out CERS_Load loadVal, out string er)
       {
           return _devMon.ReadLoadSet(wAddr, out loadVal, out er);
       }
       /// <summary>
       /// 读取负载电流
       /// </summary>
       /// <param name="wAddr"></param>
       /// <param name="dataVal"></param>
       /// <param name="er"></param>
       /// <returns></returns>
       public bool ReadData(int wAddr, out CERS_Load dataVal, out string er)
       {
           return _devMon.ReadData(wAddr,out dataVal, out er);
       }
       /// <summary>
       /// 快充电压上升或下降
       /// </summary>
       /// <param name="wAddr"></param>
       /// <param name="CH">0,1,2,3</param>
       /// <param name="wRaise"></param>
       /// <param name="er"></param>
       /// <returns></returns>
       public bool SetQCMTK(int wAddr, int CH, bool wRaise, out string er)
       {
           return _devMon.SetQCMTK(wAddr, CH, wRaise, out er);
       }
       #endregion

   }
}
