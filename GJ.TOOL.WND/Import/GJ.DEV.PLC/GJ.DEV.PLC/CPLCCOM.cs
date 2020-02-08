using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GJ.COM;

namespace GJ.DEV.PLC
{
    /// <summary>
    /// PLC工厂类
    /// </summary>
    public class CPLCCOM
    {
      #region 构造函数
      public CPLCCOM(EPlcType plcType, int idNo = 0, string name="IPLC")
        {
            this._idNo = idNo;

            this._name = name;

            this._plcType = plcType;

            //反射获取PLC类型

            string plcModule = "C" + plcType.ToString();

            Assembly asb = Assembly.GetAssembly(typeof(IPLC)) ;

            Type[] types = asb.GetTypes();

            object[] parameters = new object[2];

            parameters[0] = _idNo;

            parameters[1] = _name;

            foreach (Type t in types)
            {
                if (t.Name == plcModule && t.GetInterface("IPLC") != null)
                {
                    _devPLC = (IPLC)asb.CreateInstance(t.FullName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);
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
      private EPlcType _plcType = EPlcType.Inovance_TCP;
      private IPLC _devPLC = null;
      #endregion

      #region 属性
        /// <summary>
        /// 设备ID
        /// </summary>
        public int idNo
        {
            get { return _idNo; }
            set { _idNo = value; } 
        }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 设备类型
        /// </summary>
        public EPlcType plcType
        {
            get { return _plcType; }
        }
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool conStatus
        {
            get {
                if (_devPLC == null)
                    return false;
                else
                    return _devPLC.conStatus; 
                }
        }
        /// <summary>
        /// 单字节和双字节(M,W)
        /// </summary>
        public int wordNum
        {
            get
            {
                if (_devPLC == null)
                    return 0;
                else
                    return _devPLC.wordNum;
            }
        }
        #endregion

      #region 方法
      /// <summary>
      /// 打开通信接口
      /// </summary>
      /// <param name="comName">串口编号或IP地址</param>
      /// <param name="setting">波特率或端口号</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Open(string comName, out string er, string setting)
      {
          if (_devPLC == null)
          {
              er = _plcType.ToString() + CLanguage.Lan("未找到程序集,请检查");
              return false;
          }
          return _devPLC.Open(comName, out er, setting); 
      }
      /// <summary>
      /// 关闭通信接口
      /// </summary>
      /// <returns></returns>
      public void Close()
      {
          _devPLC.Close();
      }
      /// <summary>
      /// 读取寄存器数据（高位在前,低位在后）
      /// </summary>
      /// <param name="plcAddr">PLC地址(TCP为0)</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="N">长度</param>
      /// <param name="rData">反转处理:高在前,低位在后</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int plcAddr, ERegType regType, int startAddr, int N, out string rData, out string er)
      {
          return _devPLC.Read(plcAddr, regType, startAddr, N,out rData, out er);  
      }
      /// <summary>
      /// 读取单个寄存器数据
       /// </summary>
      /// <param name="plcAddr">PLC地址(TCP为0)</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="startBin">位地址(W1.0)无则为0</param>
      /// <param name="rVal">数据</param>
       /// <param name="er"></param>
       /// <returns></returns>
      public bool Read(int plcAddr, ERegType regType, int startAddr, int startBin, out int rVal, out string er)
      { 
        return _devPLC.Read(plcAddr,regType,startAddr, startBin, out rVal, out er);
      }
      /// <summary>
       /// 读取多个寄存器数据
       /// </summary>
      /// <param name="plcAddr">PLC地址(TCP为0)</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
       /// <param name="rVal">数据</param>
       /// <param name="er"></param>
       /// <returns></returns>
      public bool Read(int plcAddr, ERegType regType, int startAddr, ref int[] rVal, out string er)
      {
          return _devPLC.Read(plcAddr, regType, startAddr, ref rVal, out er); 
      }
      /// <summary>
      /// 写寄存器数据（高位在前,低位在后）
      /// </summary>
      /// <param name="plcAddr">PLC地址(TCP为0)</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="N">长度</param>
      /// <param name="strHex">16进制字符FFFF,反转处理:高在前,低位在后</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Write(int plcAddr, ERegType regType, int startAddr, int N, string strHex, out string er)
      {
          return _devPLC.Write(plcAddr, regType, startAddr, N, strHex, out er);
      }
      /// <summary>
      /// 写单个寄存器数据
      /// </summary>
      /// <param name="plcAddr">PLC地址(TCP为0)</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="wVal">写入数据</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Write(int plcAddr, ERegType regType, int startAddr, int startBin, int wVal, out string er)
      {
          return _devPLC.Write(plcAddr, regType, startAddr, startBin, wVal, out er);
      }
      /// <summary>
       /// 写多个寄存器数据
       /// </summary>
      /// <param name="plcAddr">PLC地址(TCP为0)</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
       /// <param name="wVal">写入数据</param>
       /// <param name="er"></param>
       /// <returns></returns>
      public bool Write(int plcAddr, ERegType regType, int startAddr, int[] wVal, out string er)
      {
          return _devPLC.Write(plcAddr, regType, startAddr, wVal, out er);
      }
       #endregion

    }
}
