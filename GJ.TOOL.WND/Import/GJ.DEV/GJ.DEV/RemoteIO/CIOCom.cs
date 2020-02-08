using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GJ.COM;
namespace GJ.DEV.RemoteIO
{
    public class CIOCom
    {
      #region 构造函数
      public CIOCom(EType ioType,int idNo=0,string name="IIO")
        {
            this._idNo = idNo;

            this._name = name;

            this._ioType = ioType;

            //反射获取PLC类型

            string plcModule = "C" + ioType.ToString();

            Assembly asb = Assembly.GetAssembly(typeof(IIO));

            Type[] types = asb.GetTypes();

            object[] parameters = new object[2];

            parameters[0] = _idNo;

            parameters[1] = _name;

            foreach (Type t in types)
            {
                if (t.Name == plcModule && t.GetInterface("IIO") != null)
                {
                    _devIO = (IIO)asb.CreateInstance(t.FullName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);
                    break;
                }
            }
        }
      public override string ToString()
      {
          return _name;
      }
      #endregion

      #region IO电平定义
      public const int XON = 1;
      public const int XOFF = 0;
      public const int YON = 1;
      public const int YOFF = 0;
      #endregion

      #region 字段
      private int _idNo = 0;
      private string _name = string.Empty;
      private bool _conStatus = false;
      private EType _ioType = EType.IO_24_16;
      private IIO _devIO = null;
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
      public bool Open(string comName, out string er,string setting="115200,n,8,1")
      {
          er = string.Empty;

          try
          {
              if (_devIO == null)
              {
                  er = _ioType.ToString() + CLanguage.Lan("未找到程序集,请检查");
                  return false;
              }
              if (!_devIO.Open(comName, out er, setting))
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
         if (_devIO == null)
            return;

         _conStatus = false;
         
         _devIO.Close();
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
         return _devIO.Read(devAddr,regType, startAddr, N, out rData, out er); 
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
         return _devIO.Read(devAddr,regType,startAddr,out rVal, out er); 
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
         return _devIO.Read(devAddr,regType,startAddr, ref rVal, out er);
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
         return _devIO.Write(devAddr,regType,startAddr, wVal, out er);   
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
         return _devIO.Write(devAddr,regType,startAddr, wVal, out er);  
      }
      #endregion

      #region 专用功能
      /// <summary>
      /// 读地址
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadAddr(out int curAddr, out string er)
      {
         return _devIO.ReadAddr(out curAddr,out er); 
      }
      /// <summary>
      /// 设置地址
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetAddr(int curAddr, out string er)
      {
        return _devIO.SetAddr(curAddr,out er);  
      }
      /// <summary>
      /// 读波特率
      /// </summary>
      /// <param name="baud"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadBaud(int curAddr, out int baud, out string er)
      {
         return _devIO.ReadBaud(curAddr,out baud,out er);  
      }
      /// <summary>
      /// 设置波特率
      /// </summary>
      /// <param name="baud"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetBaud(int curAddr, int baud, out string er)
      {
         return _devIO.SetBaud(curAddr,baud,out er); 
      }
      /// <summary>
      /// 读错误码
      /// </summary>
      /// <param name="rVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadErrCode(int curAddr, out int rVal, out string er)
      {
         return _devIO.ReadErrCode(curAddr,out rVal,out er); 
      }
      /// <summary>
      /// 读错误码
      /// </summary>
      /// <param name="rVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadVersion(int curAddr, out int rVal, out string er)
      {
          return _devIO.ReadVersion(curAddr,out rVal,out er); 
      }
      #endregion
    }
}
