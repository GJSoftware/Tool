using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;
namespace GJ.DEV.PLC
{
   [Author("kp.lin", "V1.0.0.1", "2019/01/02", "修改System.Environment.TickCount为负值造成数据异常")]
   [Author("kp.lin", "V1.0.0.2", "2019/01/05", "增加PLC报警列表获取")]
   public interface IPLC
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
      /// 字节长度(M,W):汇川=8位;欧姆龙=16位
      /// </summary>
      int wordNum
      { get; }
      #endregion

      #region 方法
      /// <summary>
      /// 打开通信接口
      /// </summary>
      /// <param name="comName">串口编号或IP地址</param>
      /// <param name="setting">波特率或端口号</param>
      /// <param name="er"></param>
      /// <returns></returns>
      bool Open(string comName,out string er ,string setting);
      /// <summary>
      /// 关闭通信接口
      /// </summary>
      /// <returns></returns>
      void Close();
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
      bool Read(int plcAddr,ERegType regType, int startAddr, int N, out string rData, out string er);
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
      bool Read(int plcAddr,ERegType regType, int startAddr, int startBin, out int rVal, out string er);
      /// <summary>
       /// 读取多个寄存器数据
       /// </summary>
      /// <param name="plcAddr">PLC地址(TCP为0)</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
       /// <param name="rVal">数据</param>
       /// <param name="er"></param>
       /// <returns></returns>
      bool Read(int plcAddr,ERegType regType, int startAddr, ref int[] rVal, out string er);
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
      bool Write(int plcAddr,ERegType regType, int startAddr, int N, string strHex, out string er);
      /// <summary>
      /// 写单个寄存器数据
      /// </summary>
      /// <param name="plcAddr">PLC地址(TCP为0)</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="wVal">写入数据</param>
      /// <param name="er"></param>
      /// <returns></returns>
      bool Write(int plcAddr,ERegType regType, int startAddr,int startBin, int wVal, out string er);
      /// <summary>
       /// 写多个寄存器数据
       /// </summary>
      /// <param name="plcAddr">PLC地址(TCP为0)</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
       /// <param name="wVal">写入数据</param>
       /// <param name="er"></param>
       /// <returns></returns>
      bool Write(int plcAddr,ERegType regType, int startAddr, int[] wVal, out string er);
      #endregion
   }
}
