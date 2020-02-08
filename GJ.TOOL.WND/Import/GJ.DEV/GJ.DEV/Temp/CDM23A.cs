using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;
using GJ.COM;

namespace GJ.DEV.Temp
{
    public class CDM23A
    {
      #region 构造函数
      public CDM23A(int idNo = 0, string name = "DM23A")
      {
          _idNo = idNo;          
          _name = name;
      }
      public override string ToString()
      {
          return _name;
      }
      #endregion

      #region 字段
      private int _idNo = 0;
      private string _name = "DM23A";
      private bool _conStatus=false;
      private CSerialPort com = null;
      #endregion
      
      #region 属性
      /// <summary>
      /// 编号
      /// </summary>
      public int idNo
      {
          get { return _idNo; }
          set { _idNo = value; }
      }
      /// <summary>
      /// 名称
      /// </summary>
      public string name
      {
         get { return _name; }
         set { _name = value; }
      }
      /// <summary>
      /// 状态
      /// </summary>
      public bool conStatus
      {
          get { return _conStatus; }
      }
      #endregion

      #region 方法
      /// <summary>
      /// 打开串口
      /// </summary>
      /// <param name="comName">38400,n,8,1</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Open(string comName, out string er,string setting="38400,n,8,1")
      {
          er = string.Empty;

          try
          {
              if (com != null)
              {
                  com.close();
                  com = null;
              }

              com = new CSerialPort(idNo, name, EDataType.HEX格式);  

              if (!com.open(comName, out er, setting))
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
          if (com == null)
              return;
          com.close();
          com = null;
          _conStatus = false;
      }
      #endregion

      #region 专用功能
      /// <summary>
      /// 启动机组
      /// </summary>
      /// <param name="devAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Start(int devAddr, out string er)
      {
          er = string.Empty;

          try
          {

              string wCmd = devAddr.ToString("X2");

              string rData = string.Empty;

              int regAddr = 0;

              int rLen = 8;

              wCmd += "05";                    //寄存器功能码为05

              wCmd += regAddr.ToString("X4");  //开始地址

              wCmd += "FF00";                  //启动命令FF00

              wCmd += CCRC.Crc16(wCmd);                  //CRC16 低位前,高位后     

              if (!com.send(wCmd, rLen, out rData, out er))
                  return false;

              if (!checkCRC(rData))
              {
                  er = "crc16检验和错误:" + rData;
                  return false;
              }

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 停止机组
      /// </summary>
      /// <param name="devAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Stop(int devAddr, out string er)
      {
          er = string.Empty;

          try
          {

              string wCmd = devAddr.ToString("X2");

              string rData = string.Empty;

              int regAddr = 0;

              int rLen = 8;

              wCmd += "05";                    //寄存器功能码为05

              wCmd += regAddr.ToString("X4");  //开始地址

              wCmd += "0000";                  //启动命令0000

              wCmd += CCRC.Crc16(wCmd);                  //CRC16 低位前,高位后     

              if (!com.send(wCmd, rLen, out rData, out er))
                  return false;

              if (!checkCRC(rData))
              {
                  er = "crc16检验和错误:" + rData;
                  return false;
              }

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 复位机组并消音
      /// </summary>
      /// <param name="devAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Reset(int devAddr, out string er)
      {
          er = string.Empty;

          try
          {

              string wCmd = devAddr.ToString("X2");

              string rData = string.Empty;

              int rLen = 8;

              wCmd += "0F";                    //寄存器功能码为0F

              wCmd += "00010002";              //开始地址

              wCmd += "0103";                  //命令

              wCmd += CCRC.Crc16(wCmd);                  //CRC16 低位前,高位后     

              if (!com.send(wCmd, rLen, out rData, out er))
                  return false;

              if (!checkCRC(rData))
              {
                  er = "crc16检验和错误:" + rData;
                  return false;
              }

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      #endregion

      #region ModBus-RTU通信协议
      /// <summary>
      /// 检查CRC
      /// </summary>
      /// <param name="wCmd"></param>
      /// <returns></returns>
      private bool checkCRC(string wCmd)
      {
         string crc = CCRC.Crc16(wCmd.Substring(0, wCmd.Length - 4));
         if (crc != wCmd.Substring(wCmd.Length - 4, 4))
            return false;
         return true;
      }
      #endregion

    }
}
