using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;
using GJ.DEV.COM; 
namespace GJ.DEV.PLC
{
   public class COmron_COM:IPLC
   {
      #region 构造函数
      public COmron_COM(int idNo = 0, string name = "Omron_COM")
      {
          this._idNo = idNo;
          this._name = name;
          com = new CSerialPort(_idNo,_name,EDataType.HEX格式);
      }
      public override string ToString()
      {
          return _name;
      }
      #endregion

      #region 字段
      private int _idNo;
      private string _name = "Omron_COM";
      private CSerialPort com=null;
      private int _wordNum = 2;
      private object _sync = new object();
      #endregion

      #region 属性
      /// <summary>
      /// 设备编号
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
      /// <param name="comName">115200,E,7,2</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Open(string comName, out string er, string setting = "115200,E,7,2")
      {
          if (!com.open(comName, out er, setting))
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
      /// 从机地址(1Byte)+功能码(1Byte)+寄存器地址(2Byte)+地址数量(2Byte)+CRC检验(2Byte)
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="ERegType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="N">地址长度</param>
      /// <param name="rData">16进制字符:数据值高位在前,低位在后</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int plcAddr, ERegType regType, int startAddr, int N, out string rData, out string er)
      {
          lock (_sync)
          {
              return readOmronREG(plcAddr, regType, startAddr, N, out rData, out er);
          }
      }
      /// <summary>
      /// 读单个线圈和寄存器值
      /// 从机地址(1Byte)+功能码(1Byte)+寄存器地址(2Byte)+地址数量(2Byte)+CRC检验(2Byte)
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="ERegType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="startBin">位地址(0..15)其他为1</param>
      /// <param name="N">地址长度</param>
      /// <param name="rVal">数据</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int plcAddr, ERegType regType, int startAddr, int startBin, out int rVal, out string er)
      {
          lock (_sync)
          {
              rVal = 0;

              er = string.Empty;

              try
              {
                  string rData = string.Empty;
                  if (!readOmronREG(plcAddr, regType, startAddr, startBin, out rData, out er))
                      return false;
                  rVal = System.Convert.ToInt32(rData, 16);
                  return true;
              }
              catch (Exception ex)
              {
                  er = ex.ToString();
                  return false;
              }
          }
      }
      /// <summary>
      /// 读多个线圈和寄存器值
      /// 从机地址(1Byte)+功能码(1Byte)+寄存器地址(2Byte)+地址数量(2Byte)+CRC检验(2Byte)
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="ERegType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="N">地址长度</param>
      /// <param name="rVal">数据</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int plcAddr, ERegType regType, int startAddr, ref int[] rVal, out string er)
      {
          lock (_sync)
          {
              rVal = null;

              er = string.Empty;

              try
              {
                  string rData = string.Empty;

                  int N = 0;

                  if (regType == ERegType.D)
                      N = rVal.Length;
                  else
                      N = (rVal.Length + 15) / 16;

                  if (!readOmronREG(plcAddr, regType, startAddr, N, out rData, out er))
                      return false;

                  int temVal = 0;

                  int z = 0;

                  for (int i = 0; i < N; i++)
                  {
                      temVal = System.Convert.ToInt32(rData.Substring(i * 4, 4), 16);
                      if (regType == ERegType.D)
                      {
                          rVal[i] = temVal;
                      }
                      else
                      {
                          for (int j = 0; j < 16; j++)
                          {
                              if (z < rVal.Length)
                              {
                                  if ((temVal & (1 << j)) == (1 << j))
                                      rVal[z] = 1;
                                  else
                                      rVal[z] = 0;
                                  z++;
                              }
                          }
                      }
                  }
                  return true;
              }
              catch (Exception ex)
              {
                  er = ex.ToString();
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
              er = string.Empty;

              try
              {
                  string rData = string.Empty;

                  string wData = string.Empty;

                  int wLen = N * 4;

                  for (int i = 0; i < N; i++)
                      wData += "0000";

                  string temp = string.Empty;

                  for (int i = 0; i < strHexVal.Length / 4; i++)
                      temp = strHexVal.Substring(i * 4, 4) + temp;

                  wData = wData + temp;

                  wData = wData.Substring(wData.Length - wLen * 2, wLen * 2);

                  return SendOmronCommand(plcAddr, "0102", regType, startAddr, 0, N, wData, out rData, out er);
              }
              catch (Exception ex)
              {
                  er = ex.ToString();
                  return false;
              }
          }         
      }
      /// <summary>
      /// 单写寄存器数据
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="startBin">位地址W100.0->startBin=0</param>
      /// <param name="wVal">寄存器数值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Write(int plcAddr, ERegType regType, int startAddr, int startBit, int wVal, out string er)
      {
          lock (_sync)
          {
              string rData = string.Empty;

              return SendOmronCommand(plcAddr, "0102", regType, startAddr, startBit, 1, wVal.ToString("X4"), out rData, out er);
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
                  string wContent = string.Empty;

                  int N = 0;

                  if (regType == ERegType.D)
                  {
                      N = wVal.Length;
                      for (int i = 0; i < wVal.Length; i++)
                      {
                          wContent += wVal[i].ToString("X4");
                      }
                  }
                  else
                  {
                      N = (wVal.Length + 15) / 16;
                      int z = 0;
                      for (int i = 0; i < N; i++)
                      {
                          int bitData = 0;
                          for (int j = 0; j < 16; j++)
                          {
                              if (z < wVal.Length)
                              {
                                  if (wVal[z] == 1)
                                      bitData += (1 << j);
                              }
                              z++;
                          }
                          wContent += bitData.ToString("X4");
                      }
                  }
                  string rData = string.Empty;

                  if (!SendOmronCommand(plcAddr, "0102", regType, startAddr, 0, N, wContent, out rData, out er))
                      return false;

                  return true;
              }
              catch (Exception ex)
              {
                  er = ex.ToString();
                  return false;
              }
          }
      }
      #endregion

      #region Fins协议
      private string c_SOI = "@";
      private string c_EOI =new string(new char[]{'\x2A','\x0D'}) ;
      private bool readOmronREG(int plcAddr, ERegType wDataType, int wStartAddr, int wLen, out string rData, out string er)
      {        
         return SendOmronCommand(plcAddr,"0101",wDataType,wStartAddr,0,wLen,string.Empty,out rData,out er); 
      }
      private bool writeOmronREG(int plcAddr, ERegType wDataType, int wStartAddr, int wStartBit, int wLen, string wWriteContent, ref string rData, ref string er)
      {
         return SendOmronCommand(plcAddr,"0102", wDataType, wStartAddr,wStartBit, wLen, wWriteContent, out rData, out er); 
      }
      private bool SendOmronCommand(int plcAddr,string wCommandType, ERegType wRegType, int wStartAddr, int wStartBit, int wWordCount, string wWriteContent,
                                    out string rData, out string er)
      {
         rData = string.Empty;

         er = string.Empty;

         try 
	      {	        		    
           if(wWriteContent!=string.Empty)
           {
            for (int i = 0; i < wWordCount; i++)
			{
			    wWriteContent="0000"+wWriteContent;
			}
            wWriteContent=wWriteContent.Substring(wWriteContent.Length - wWordCount * 4, wWordCount * 4);
           }
           string strCommand;
           strCommand = c_SOI + plcAddr.ToString("X2") + "FA000000000";
           strCommand+=wCommandType; 
           strCommand+=getRegAddr(wRegType);
           strCommand += wStartAddr.ToString("X4") + wStartBit.ToString("X2");  
           strCommand +=wWordCount.ToString("X4");
           if (wRegType ==ERegType.WB)
               strCommand += wWriteContent.Substring(wWriteContent.Length-2,2);
           else
               strCommand += wWriteContent;  
           strCommand+=codeFcs(strCommand);
           strCommand+=c_EOI;
           int lngWordLen=0;
           if(wWriteContent==string.Empty)
              lngWordLen =4;
           int rLen=27 + lngWordLen * wWordCount;
           if(!com.send(strCommand,rLen,out rData,out er))
              return false;
           if (!verifyReceive(plcAddr,rData, wCommandType))
            {
                er = CLanguage.Lan("接收数据错误") + "[checkSum]:" + rData;
               return false; 
            }
            if (wWriteContent != string.Empty)      //write       
               rData = "OK";
            else                                    //read
               rData = rData.Substring(23, lngWordLen * wWordCount);                
            return true;
	      }
	      catch (Exception e)
	      {
		      er=e.ToString(); 
		      return false;
	      }
      }
      /// <summary>
      /// 检查接收数据是否正确
      /// </summary>
      /// <param name="wResultString"></param>
      /// <param name="wAddr"></param>
      /// <param name="wCommandType"></param>
      /// <returns></returns>
      private bool verifyReceive(int plcAddr,string wResultString,string wCommandType)
      {
          if (plcAddr.ToString("X2") != wResultString.Substring(1, 2))
           return false;
         if(wCommandType!=wResultString.Substring(15,4))
            return false;
         if(!checkFcs(wResultString))
            return false;
        return true;
      }
      /// <summary>
      /// codeFcs校验和
      /// </summary>
      /// <param name="wData"></param>
      /// <returns></returns>
      private string codeFcs(string wData)
      {
        byte[] byteData=System.Text.Encoding.ASCII.GetBytes(wData);
        byte byteTemp=byteData[0];
        for (int i = 1; i < byteData.Length; i++)
			  byteTemp ^=byteData[i]; 
		  return byteTemp.ToString("X2");  
      }
      /// <summary>
      /// 检查FCS校验和
      /// </summary>
      /// <param name="wResultString"></param>
      /// <returns></returns>
      private bool checkFcs(string wResultString)
      {
        int _LastCount=4;
        string _temp=wResultString.Substring(0,wResultString.Length -_LastCount);  
        string _Fcs=codeFcs(_temp);
        if(_Fcs!=wResultString.Substring(wResultString.Length-_LastCount,2))
           return false;
         return true;
      }
      /// <summary>
      /// 获取设备寄存器类型地址
      /// B1->WR_WORD 31->WR_BIT  
      /// 82->DM_WORD 
      /// B0->IO_WORD 30->IO_BIT
      /// </summary>
      /// <param name="regType"></param>
      /// <returns></returns>
      private string getRegAddr(ERegType regType)
      {
          string addrType = string.Empty;

          switch (regType)
          {
              case ERegType.M:
                  break;
              case ERegType.W:
                  addrType = "B1";
                  break;
              case ERegType.D:
                  addrType = "82";
                  break;
              case ERegType.X:
                  addrType = "B0";
                  break;
              case ERegType.Y:
                  addrType = "B0";
                  break;
              case ERegType.WB:
                  addrType = "31";
                  break;
              default:
                  break;
          }
          return addrType;
      }
      #endregion

   }
}
