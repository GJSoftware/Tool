using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.ERS
{
   public interface IERS
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
      /// 读版本
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="version"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      bool ReadVersion(int wAddr, out string version, out string er);
      /// <summary>
      /// 设置负载全部通道电流
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="loadPara"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      bool SetNewLoad(int wAddr, CERS_Load loadPara, out string er, bool saveEPROM = true);
      /// <summary>
      /// 设置负载单通道电流
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="CH"></param>
      /// <param name="loadVal"></param>
      /// <param name="er"></param>
      /// <param name="saveEPROM"></param>
      /// <returns></returns>
      bool SetNewLoad(int wAddr, int CH, double loadVal, out string er, bool saveEPROM = true);
      /// <summary>
      /// 读当前负载值
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="dataVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      bool ReadLoadSet(int wAddr, out CERS_Load loadVal, out string er);
      /// <summary>
      /// 读电压和电流
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="dataVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      bool ReadData(int wAddr, out CERS_Load dataVal, out string er);
      /// <summary>
      /// 快充电压上升或下降
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="CH"></param>
      /// <param name="wRaise"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      bool SetQCMTK(int wAddr, int CH, bool wRaise, out string er);
      #endregion

   }
}
