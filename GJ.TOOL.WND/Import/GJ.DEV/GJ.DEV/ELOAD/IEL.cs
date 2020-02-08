using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.ELOAD
{
  public interface IEL
  {
     #region 属性
      /// <summary>
      /// 设备ID
      /// </summary>
      int idNo
      { set; get; }
      /// <summary>
      /// 设备名称
      /// </summary>
      string name
      { set; get; }
      /// <summary>
      /// 连接状态
      /// </summary>
      bool conStatus
      { get; }
      /// <summary>
      /// 负载通道数
      /// </summary>
      int maxCH
      { get; }
      #endregion

     #region 方法
      /// <summary>
      /// 打开串口
      /// </summary>
      /// <param name="comName"></param>
      /// <param name="setting"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      bool Open(string comName, out string er, string setting = "9600,n,8,1");
      /// <summary>
      /// 关闭串口
      /// </summary>
      /// <returns></returns>
      void Close();
     /// <summary>
     /// 设置地址
     /// </summary>
     /// <param name="wAddr"></param>
     /// <param name="er"></param>
     /// <returns></returns>
     bool SetNewAddr(int wAddr, out string er);
     /// <summary>
     /// 设置负载
     /// </summary>
     /// <param name="wAddr"></param>
     /// <param name="wDataSet"></param>
     /// <param name="er"></param>
     /// <returns></returns>
     bool SetELData(int wAddr, CEL_SetPara wDataSet, out string er);
     /// <summary>
     /// 读取负载设置
     /// </summary>
     /// <param name="wAddr"></param>
     /// <param name="rLoadVal"></param>
     /// <param name="er"></param>
     /// <returns></returns>
     bool ReadELLoadSet(int wAddr, CEL_ReadSetPara rDataSet, out string er);
     /// <summary>
     /// 读取负载数值
     /// </summary>
     /// <param name="wAddr"></param>
     /// <param name="rDataVal"></param>
     /// <param name="er"></param>
     /// <returns></returns>
     bool ReadELData(int wAddr, CEL_ReadData rDataVal, out string er);
     #endregion
  }
}
