using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;
using GJ.DEV.COM;
namespace GJ.DEV.RemoteIO
{
    public class CIO_24_16:IIO
    {
      #region 构造函数
      public CIO_24_16(int idNo = 0, string name = "IO_24_16")
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
      private string _name = "IO_24_16";
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
      /// <param name="comName">115200,n,8,1</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Open(string comName, out string er,string setting="115200,n,8,1")
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
      /// <summary>
      /// 读线圈
      /// 从机地址(1Byte)+功能码(1Byte)+寄存器地址(2Byte)+地址数量(2Byte)+CRC检验(2Byte)
      /// </summary>
      /// <param name="devType">地址类型</param>
      /// <param name="regAddr">开始地址</param>
      /// <param name="N">地址长度</param>
      /// <param name="rData">16进制字符:数据值高位在前,低位在后</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int devAddr, ERegType regType, int regAddr, int N, out string rData, out string er)
      {
          rData = string.Empty;

          er = string.Empty;

         try
         {
             string wCmd = devAddr.ToString("X2");
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
             wCmd += formatDevAddr(regType, regAddr);  //开始地址
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
      /// <summary>
      /// 读单个线圈
      /// </summary>
      /// <param name="devType"></param>
      /// <param name="regAddr"></param>
      /// <param name="N"></param>
      /// <param name="rVal">开始地址值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int devAddr, ERegType regType, int regAddr, out int rVal, out string er)
      {
          rVal = -1;

          er = string.Empty;

          try
          {
              int N = 1;

              string rData = string.Empty;

              if (!Read(devAddr,regType, regAddr, N, out rData, out er))
                  return false;
              
              if ((regType != ERegType.D))
              {
                  for (int i = 0; i < rData.Length / 2; i++)
                  {
                      int valByte =System.Convert.ToInt16(rData.Substring(rData.Length - (i + 1) * 2, 2), 16);
                      if ((valByte & (1 << 0)) == (1 << 0))
                          rVal = 1;
                      else
                          rVal = 0;
                  }
              }
              else
                  rVal =System.Convert.ToInt16(rData.Substring(rData.Length - 4, 4), 16);
              return true;
          }
          catch (Exception e)
          {
              er = e.ToString();
              return false;
          }
      }
      /// <summary>
      /// 返回数值
      /// </summary>
      /// <param name="devType"></param>
      /// <param name="regAddr"></param>
      /// <param name="N"></param>
      /// <param name="rVal">地址N值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int devAddr, ERegType regType, int regAddr, ref int[] rVal, out string er)
      {
          er = string.Empty;

          try
          {
              string rData = string.Empty;

              int N = rVal.Length;

              if (!Read(devAddr,regType,regAddr, N, out rData, out er))
                  return false;

              if (regType != ERegType.D)
              {
                  for (int i = 0; i < rData.Length / 2; i++)
                  {
                      int valByte =System.Convert.ToInt16(rData.Substring(rData.Length - (i + 1) * 2, 2), 16);
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
                      rVal[i] =System.Convert.ToInt16(rData.Substring(rData.Length - (i + 1) * 4, 4), 16);

              }
              return true;
          }
          catch (Exception e)
          {
              er = e.ToString();
              return false;
          }
      }
      /// <summary>
      /// 单写线圈和寄存器值
      /// 从机地址(1Byte)+功能码(1Byte)+寄存器地址(2Byte)+地址数量(2Byte)+字节数(1Byte)+数据+CRC检验(2Byte)
      /// </summary>
      /// <param name="devType">地址类型</param>
      /// <param name="regAddr">开始地址</param>
      /// <param name="wVal">单个值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Write(int devAddr, ERegType regType, int regAddr, int wVal, out string er)
      {
          er = string.Empty;

          try
          {
              int N = 1;   //单写1个值
              string wCmd = devAddr.ToString("X2");
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
              }
              wCmd += formatDevAddr(regType, regAddr);  //开始地址
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
      /// <summary>
      /// 写多个线圈和寄存器
      /// </summary>
      /// <param name="devType">地址类型</param>
      /// <param name="regAddr">开始地址</param>
      /// <param name="wVal">多个值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Write(int devAddr, ERegType regType, int regAddr, int[] wVal, out string er)
      {
          er = string.Empty;

          try
          {
              int N = wVal.Length;   //单写多个值
              string wCmd = devAddr.ToString("X2");
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
              wCmd += formatDevAddr(regType, regAddr);  //开始地址
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
          curAddr = 0;

          er = string.Empty;
          
          try
          {
              string rData=string.Empty;

              if (!Read(1, ERegType.D, 0xF000, 1, out rData, out er))
                  return false;

              curAddr =System.Convert.ToInt16(rData,16);  

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false; 
          }
      }
      /// <summary>
      /// 设置地址
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetAddr(int curAddr, out string er)
      {
          try
          {
              return Write(1, ERegType.D, 0xF000, curAddr, out er);
          }
          catch (Exception ex)
          {
              er=ex.ToString();
              return false; 
          }
      }
      /// <summary>
      /// 读波特率
      /// </summary>
      /// <param name="baud"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadBaud(int curAddr, out int baud, out string er)
      {
          baud =- 1;

          er = string.Empty;

          try
          {
              string rData = string.Empty;
              if (!Read(curAddr, ERegType.D, 0xF400, 1, out rData, out er))
                  return false;
              baud =System.Convert.ToInt16(rData,16);
              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 设置波特率
      /// </summary>
      /// <param name="baud"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetBaud(int curAddr, int baud, out string er)
      {
          try
          {
              return Write(curAddr, ERegType.D, 0xF400, baud, out er);
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读错误码
      /// </summary>
      /// <param name="rVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadErrCode(int curAddr, out int rVal, out string er)
      {
          rVal = -1;

          er = string.Empty;
          
          try
          {
              string rData = string.Empty;
              if (!Read(curAddr, ERegType.D, 0xF700, 1, out rData, out er))
                  return false;
              rVal =System.Convert.ToInt16(rData,16);
              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读错误码
      /// </summary>
      /// <param name="rVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadVersion(int curAddr, out int rVal, out string er)
      {
          rVal = -1;

          er = string.Empty;

          try
          {
              string rData = string.Empty;
              if (!Read(curAddr, ERegType.D, 0xFA00, 1, out rData, out er))
                  return false;
              rVal =System.Convert.ToInt16(rData,16);
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
      /// 格式化地址段
      /// </summary>
      /// <param name="devType">
      /// X0-X255:起始地址->0xF800
      /// Y0-Y255:起始地址->0xFC00
      /// </param>
      /// <param name="coilAddr"></param>
      /// <returns></returns>
      private string formatDevAddr(ERegType regType, int coilAddr)
      {
         int addr = 0;
         switch (regType)
         {
            case ERegType.X:
               addr = coilAddr + 0xF800;
               break;
            case ERegType.Y:
               addr = coilAddr + 0xFC00;
               break;
             case ERegType.D:
               addr = coilAddr;
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
