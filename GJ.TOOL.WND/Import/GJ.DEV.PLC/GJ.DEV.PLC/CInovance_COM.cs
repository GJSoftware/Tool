using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;
using GJ.DEV.COM;  

namespace GJ.DEV.PLC
{
    /// <summary>
    /// 汇川PLC ModBus-RTU 串口
    /// 版本:V1.0.0 作者:kp.lin 日期:2017/08/15
    /// </summary>
   public class CInovance_COM:IPLC
   {
      #region 构造函数
      public CInovance_COM(int idNo = 0, string name = "Inovance_COM")
      {
          this._idNo = idNo;
          this._name = name;
          com = new CSerialPort(_idNo, _name, EDataType.HEX格式);   
      }
      public override string ToString()
      {
          return _name;
      }
      #endregion

      #region 字段
      /// <summary>
      /// ID编号-PLC地址
      /// </summary>
      private int _idNo = 0;
      /// <summary>
      /// 设备名称
      /// </summary>
      private string _name = "Inovance_COM";
      /// <summary>
      /// 通信接口
      /// </summary>
      private CSerialPort com;
      private int _wordNum = 1;
      private object _sync = new object();
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
      ///设备名称 
      /// </summary>
      public string name
      {
         get { return _name; }
         set { _name = value; }
      }
      /// <summary>
      /// 连接状态
      /// </summary>
      public bool conStatus
      {
          get
          {
              if (com == null)
                  return false;
              else
                  return com.conStatus;
          }
      }
      /// <summary>
      /// 字节长度
      /// </summary>
      public int wordNum
      {
          get { return _wordNum; }
      }
      #endregion

      #region 方法
      /// <summary>
      /// 打开串口
      /// </summary>
      /// <param name="comName">115200,n,8,2</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Open(string comName, out string er, string setting = "115200,n,8,2")
      {
          if (!com.open(comName, out er,setting))
            return false;
         return true;
      }
      /// <summary>
      /// 关闭串口
      /// </summary>
      public void Close()
      {
         com.close();
      }
      /// <summary>
      /// 读线圈和寄存器值
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="N">双字节(FFFF)为1个长度</param>
      /// <param name="rData">16进制字符(双字节):数据值高位在前,低位在后</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int plcAddr, ERegType regType, int startAddr, int N, out string rData, out string er)
      {
          lock (_sync)
          {
              rData = string.Empty;

              er = string.Empty;

              try
              {
                  string wCmd = plcAddr.ToString("X2");

                  int rLen = 0;

                  if (regType != ERegType.D)
                  {
                      wCmd += "01";          //线圈功能码为01
                      rLen = (N + 7) / 8;    //8个线圈为1Byte:前8个线圈存第1字节,最小线圈存最低位
                  }
                  else
                  {
                      wCmd += "03";      //寄存器功能码为03
                      rLen = N * 2;
                  }
                  wCmd += formatDevAddr(regType, startAddr);  //开始地址
                  wCmd += N.ToString("X4");                   //读地址长度
                  wCmd += CCRC.Crc16(wCmd);                  //CRC16 低位前,高位后            
                  if (!com.send(wCmd, 5 + rLen, out rData, out er))
                      return false;
                  if (!checkCRC(rData))
                  {
                      er = CLanguage.Lan("crc16检验和错误") + ":" + rData;
                      return false;
                  }
                  string temp = rData.Substring(6, rLen * 2);
                  if (regType != ERegType.D)
                  {
                      //转化线圈数据为 Mn,Mn-1..M1,M0(高位在前,低位在后)
                      string coil_HL = string.Empty;
                      int coilLen = temp.Length / 2;
                      for (int i = 0; i < coilLen; i++)
                      {
                          coil_HL += temp.Substring(temp.Length - (i + 1) * 2, 2);
                      }
                      rData = coil_HL;
                  }
                  else
                  {
                      rData = temp;     //2个字节为寄存器值，高在前,低位在后，寄存器小排最前面；
                      //转换为寄存器小排最后
                      rData = string.Empty;
                      for (int i = 0; i < temp.Length / 4; i++)
                      {
                          rData = temp.Substring(i * 4, 4) + rData;
                      }
                  }
                  return true;
              }
              catch (Exception e)
              {
                  er = e.ToString();
                  return false;
              }
          }
      }
      /// <summary>
      /// 读单个寄存器值
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="ERegType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="startBin">无</param>
      /// <param name="N">双字节(FFFF)为1个长度</param>
      /// <param name="rVal">数据值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int plcAddr, ERegType regType, int startAddr, int startBin,out int rVal, out string er)
      {
          lock (_sync)
          {
              rVal = 0;

              er = string.Empty;

              try
              {
                  int N = 1;

                  string rData = string.Empty;

                  if (!Read(plcAddr, regType, startAddr, N, out rData, out er))
                      return false;

                  if ((regType != ERegType.D))
                  {
                      for (int i = 0; i < rData.Length / 2; i++)
                      {
                          int valByte = System.Convert.ToInt32(rData.Substring(rData.Length - (i + 1) * 2, 2), 16);
                          if ((valByte & (1 << 0)) == (1 << 0))
                              rVal = 1;
                          else
                              rVal = 0;
                      }
                  }
                  else
                  {
                      rVal = System.Convert.ToInt32(rData.Substring(rData.Length - 4, 4), 16);
                  }
                  return true;
              }
              catch (Exception e)
              {
                  er = e.ToString();
                  return false;
              }
          }
      }
      /// <summary>
      /// 读多个寄存器值
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="rVal">多个寄存器数据</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int plcAddr, ERegType regType, int startAddr, ref int[] rVal, out string er)
      {
          lock (_sync)
          {
              er = string.Empty;

              try
              {
                  string rData = string.Empty;

                  int N = rVal.Length;

                  if (!Read(plcAddr, regType, startAddr, N, out rData, out er))
                      return false;

                  if (regType != ERegType.D)
                  {
                      for (int i = 0; i < rData.Length / 2; i++)
                      {
                          int valByte = System.Convert.ToInt32(rData.Substring(rData.Length - (i + 1) * 2, 2), 16);
                          for (int j = 0; j < 8; j++)
                          {
                              if ((j + 8 * i) < N)
                              {
                                  if ((valByte & (1 << j)) == (1 << j))
                                      rVal[j + i * 8] = 1;
                                  else
                                      rVal[j + i * 8] = 0;
                              }
                          }
                      }
                  }
                  else
                  {
                      for (int i = 0; i < N; i++)
                      {
                          rVal[i] = System.Convert.ToInt32(rData.Substring(rData.Length - (i + 1) * 4, 4), 16);
                      }

                  }
                  return true;
              }
              catch (Exception e)
              {
                  er = e.ToString();
                  return false;
              }
          }
      }
      /// <summary>
      /// 写多线圈和寄存器值
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="N">双字节(FFFF)为1个长度</param>
      /// <param name="strHex">16进制字符格式:FFFF FFFF(N=2) 高4位在前，低4位在后</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Write(int plcAddr, ERegType regType, int startAddr, int N, string strHexVal, out string er)
      {
          lock (_sync)
          {
              try
              {
                  //N:单写1个值 --->双字节0xFFFF --->word字节0xFFFFFFFF  
                  string wCmd = plcAddr.ToString("X2");
                  int rLen = 0;
                  int wLen = 0;
                  string wData = string.Empty;
                  if (regType != ERegType.D)
                  {
                      wCmd += "0F";          //线圈功能码为15
                      wLen = (7 + N) / 8;    //写入字节数
                      rLen = 8;              //回读长度
                  }
                  else
                  {
                      wCmd += "10";        //寄存器功能码为16
                      wLen = N * 2;          //写入字节数
                      rLen = 8;           //回读长度
                  }
                  //反转:低位在前,高位在后
                  for (int i = 0; i < N; i++)
                      wData += "0000";

                  string temp = string.Empty;

                  for (int i = 0; i < strHexVal.Length / 4; i++)
                      temp = strHexVal.Substring(i * 4, 4) + temp;

                  wData = wData + temp;

                  wData = wData.Substring(wData.Length - wLen * 2, wLen * 2);
                  wCmd += formatDevAddr(regType, startAddr);  //开始地址
                  wCmd += N.ToString("X4");                  //读地址长度
                  wCmd += wLen.ToString("X2");               //写入字节数  
                  wCmd += wData;                             //写入数据
                  wCmd += CCRC.Crc16(wCmd);                //CRC16 低位前,高位后   
                  string rData = string.Empty;
                  if (!com.send(wCmd, rLen, out rData, out er))
                      return false;
                  if (!checkCRC(rData))
                  {
                      er = CLanguage.Lan("crc16检验和错误") + ":" + rData;
                      return false;
                  }
                  return true;
              }
              catch (Exception e)
              {
                  er = e.ToString();
                  return false;
              }
          }
      }
      /// <summary>
      /// 单写线圈和寄存器值
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="startBin">0</param>
      /// <param name="wVal">数据值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Write(int plcAddr, ERegType regType, int startAddr, int startBit, int wVal, out string er)
      {
          lock (_sync)
          {
              er = string.Empty;

              try
              {
                  int N = 1;   //单写1个值
                  if (startBit > 0)
                      N = startBit;
                  string wCmd = plcAddr.ToString("X2");
                  int rLen = 0;
                  int wLen = 0;
                  string wData = string.Empty;
                  if (regType != ERegType.D)
                  {
                      wCmd += "0F";          //线圈功能码为15
                      wLen = (7 + N) / 8;    //写入字节数
                      rLen = 8;              //回读长度
                      wData = wVal.ToString("X" + wLen * 2);
                  }
                  else
                  {
                      wCmd += "10";        //寄存器功能码为16
                      wLen = N * 2;          //写入字节数
                      rLen = 8;           //回读长度
                      wData = wVal.ToString("X" + wLen * 2);
                      string temp = wData;
                      wData = string.Empty;
                      for (int i = 0; i < temp.Length / 4; i++)
                          wData = temp.Substring(i * 4, 4) + wData;
                  }
                  wCmd += formatDevAddr(regType, startAddr);  //开始地址
                  wCmd += N.ToString("X4");                  //读地址长度
                  wCmd += wLen.ToString("X2");               //写入字节数  
                  wCmd += wData;                             //写入数据
                  wCmd += CCRC.Crc16(wCmd);                //CRC16 低位前,高位后   
                  string rData = string.Empty;
                  if (!com.send(wCmd, rLen, out rData, out er))
                      return false;
                  if (!checkCRC(rData))
                  {
                      er = CLanguage.Lan("crc16检验和错误") + ":" + rData;
                      return false;
                  }
                  return true;
              }
              catch (Exception e)
              {
                  er = e.ToString();
                  return false;
              }
          }
      }
      /// <summary>
      /// 写多个线圈和寄存器
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="wVal">多个寄存器数据值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Write(int plcAddr, ERegType regType, int startAddr, int[] wVal, out string er)
      {
          lock (_sync)
          {
              er = string.Empty;

              try
              {
                  int N = wVal.Length;   //单写多个值
                  string wCmd = plcAddr.ToString("X2");
                  int rLen = 0;
                  int wLen = 0;
                  string wData = string.Empty;
                  if (regType != ERegType.D)
                  {
                      wCmd += "0F";          //线圈功能码为15
                      wLen = (7 + N) / 8;    //写入字节数
                      rLen = 8;              //回读长度
                      for (int i = 0; i < wLen; i++)
                      {
                          int val = 0;
                          for (int j = 0; j < 8; j++)
                          {
                              if (i * 8 + j < N)
                              {
                                  int bit = (wVal[i * 8 + j] & 0x1) << j;
                                  val += bit;
                              }
                          }
                          wData += val.ToString("X2");
                      }
                  }
                  else
                  {
                      wCmd += "10";        //寄存器功能码为16
                      wLen = N * 2;          //写入字节数
                      rLen = 8;           //回读长度
                      for (int i = 0; i < N; i++)
                      {
                          wData += wVal[i].ToString("X4");
                      }

                  }
                  wCmd += formatDevAddr(regType, startAddr);  //开始地址
                  wCmd += N.ToString("X4");                  //读地址长度
                  wCmd += wLen.ToString("X2");               //写入字节数  
                  wCmd += wData;                             //写入数据
                  wCmd += CCRC.Crc16(wCmd);                //CRC16 低位前,高位后   
                  string rData = string.Empty;
                  if (!com.send(wCmd, rLen, out rData, out er))
                      return false;
                  if (!checkCRC(rData))
                  {
                      er = CLanguage.Lan("crc16检验和错误") + ":" + rData;
                      return false;
                  }
                  return true;
              }
              catch (Exception e)
              {
                  er = e.ToString();
                  return false;
              }
          }
      }
      #endregion

      #region ModBus-RTU通信协议
      /// <summary>
      /// 格式化地址段
      /// </summary>
      /// <param name="regType">
      /// M0-M3071:起始地址->0x0
      /// M8000-M8255:起始地址->0x1F40
      /// X0-X255:起始地址->0xF800
      /// Y0-Y255:起始地址->0xFC00
      /// D0-D8255:起始地址->0x0
      /// </param>
      /// <param name="devAddr"></param>
      /// <returns></returns>
      private string formatDevAddr(ERegType regType, int devAddr)
      {
         int addr = 0;
         switch (regType)
         {
             case ERegType.M:
               addr = devAddr;
               break;
             case ERegType.X:
               addr = devAddr + 0xF800;
               break;
             case ERegType.Y:
               addr = devAddr + 0xFC00;
               break;
             case ERegType.D:
               addr = devAddr;
               break;
            default:
               break;
         }
         return addr.ToString("X4");
      }
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
