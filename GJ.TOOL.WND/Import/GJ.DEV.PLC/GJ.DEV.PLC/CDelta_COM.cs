using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;

namespace GJ.DEV.PLC
{
    /// <summary>
    /// 台达PLC串口
    /// 版本:V1.0.0 作者:kp.lin 日期:2017/08/15
    /// </summary>
    public class CDelta_COM : IPLC
    {
      #region 构造函数
      public CDelta_COM(int idNo=0,string name="Delta_COM")
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
      private int _idNo = 0;
      private string _name = "Delta_COM";
      private CSerialPort com=null;
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
          get {
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
      /// <param name="comName">setting</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Open(string comName,out string er, string setting)
      {
          if (!com.open(comName,out er, setting))
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
      /// 起始字元(1Byte)+ 通訊位址("01"-1Byte)+通訊命令(1Byte)+数据(1-32Byte)+LRC(2Byte)+END(2Byte)
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="N">地址长度</param>
      /// <param name="rData">16进制字符:数据值高位在前,低位在后</param>
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
                  if (!readDeltaREG(plcAddr, regType, startAddr, N, out rData, out er))
                      return false;

                  //反转:高位在前,低位在后
                  string temp = string.Empty;

                  for (int i = 0; i < rData.Length / 4; i++)
                      temp = rData.Substring(i * 4, 4) + temp;

                  rData = temp;

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
      /// 读单个线圈和寄存器值
      /// 起始字元(1Byte)+ 通訊位址("01"-1Byte)+通訊命令(1Byte)+数据(1-32Byte)+LRC(2Byte)+END(2Byte)
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="startBin">无=0</param>
      /// <param name="N">地址长度</param>
      /// <param name="rVal">数据</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int plcAddr, ERegType regType, int startAddr, int startBin, out int rVal, out string er)
      {
          lock (_sync)
          {
              rVal = -1;

              er = string.Empty;

              try
              {
                  string rData = string.Empty;

                  if (!readDeltaREG(plcAddr, regType, startAddr, 1, out rData, out er))
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
      /// 读线圈和寄存器值
      /// 起始字元(1Byte)+ 通訊位址("01"-1Byte)+通訊命令(1Byte)+数据(1-32Byte)+LRC(2Byte)+END(2Byte)
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="N">地址长度</param>
      /// <param name="rVal">数据</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int plcAddr,ERegType regType, int startAddr, ref int[] rVal, out string er)
      {
          lock (_sync)
          {
              er = string.Empty;

              try
              {
                  string rData = string.Empty;

                  int addr = 0;

                  int N = 0;

                  if (regType == ERegType.D)
                  {
                      N = rVal.Length;
                      addr = startAddr;
                  }
                  else
                  {
                      N = (rVal.Length + 15) / 16;
                      addr = (startAddr / 16) * 16;
                  }

                  if (!readDeltaREG(plcAddr, regType, addr, N, out rData, out er))
                      return false;

                  int temVal = 0;

                  for (int i = 0; i < N; i++)
                  {
                      temVal = System.Convert.ToInt32(rData.Substring(i * 4, 2), 16) * 256 + System.Convert.ToInt32(rData.Substring(i * 4 + 2, 2), 16);

                      if (regType == ERegType.D)
                      {
                          rVal[i] = temVal;
                      }
                      else
                      {
                          int getAdrsNo = startAddr % 16;
                          if ((temVal & (1 << getAdrsNo)) == (1 << getAdrsNo))
                          {
                              rVal[0] = 1;
                          }
                          else
                          {
                              rVal[0] = 0;
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
      /// 写寄存器数据（高位在前,低位在后）
      /// </summary>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="N">长度</param>
      /// <param name="strHex">16进制字符FFFF,反转处理:低位在后，高在前</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Write(int plcAddr, ERegType regType, int startAddr, int N, string strHex, out string er)
      {
          lock (_sync)
          {
              er = string.Empty;

              try
              {
                  //反转：低位在前,高位在后

                  string rData = string.Empty;

                  string wData = string.Empty;

                  string temp = string.Empty;

                  for (int i = 0; i < N; i++)
                      wData += "0000";

                  for (int i = 0; i < strHex.Length; i++)
                      temp = strHex.Substring(i * 4, 4) + temp;

                  wData += temp;

                  wData = wData.Substring(wData.Length - N * 4, N * 4);

                  if (!SendDeltaCommand(plcAddr, ":", regType, startAddr, N, 1, wData, out rData, out er))
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
      /// <summary>
      /// 单写线圈和寄存器值
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="regType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="wVal">单个值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Write(int plcAddr,ERegType regType, int startAddr,int startBit, int wVal, out string er)
      {
          lock (_sync)
          {
              er = string.Empty;

              try
              {
                  string rData = string.Empty;

                  string wData = string.Empty;

                  if (regType == ERegType.D)
                  {
                      wData = wVal.ToString("X4");
                  }
                  else
                  {
                      if (wVal == 0)
                      {
                          wData = "0000";
                      }
                      if (wVal == 1)
                      {
                          wData = "FF00";
                      }
                  }

                  if (!SendDeltaCommand(plcAddr, ":", regType, startAddr, 1, 1, wData, out rData, out er))
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
      /// <summary>
      /// 写多个线圈和寄存器
      /// </summary>
      /// <param name="plcAddr">PLC地址</param>
      /// <param name="devType">寄存器类型</param>
      /// <param name="startAddr">开始地址</param>
      /// <param name="wVal">多个值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Write(int plcAddr,ERegType regType, int startAddr, int[] wVal, out string er)
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
                      for (int i = 0; i < wVal.Length; i++)
                      {
                          if (wVal[i] == 0)
                          {
                              wContent = "0000";
                          }
                          if (wVal[i] == 1)
                          {
                              wContent = "FF00";
                          }
                      }
                  }

                  string rData = string.Empty;

                  if (!SendDeltaCommand(plcAddr, ":", regType, startAddr, N, 1, wContent, out rData, out er))
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

      #region 协议
      private bool readDeltaREG(int plcAddr,ERegType wDataType, int wStartAddr, int wLen,out string rData, out string er)
      {
        rData=string.Empty;

        er=string.Empty;

        try 
	    {	        
		    int wr = 0;

            if (wDataType == ERegType.D)
                wr = 1;
            else
                wr = 16;   
       
            string wDate = wr.ToString("X4");

            if(!SendDeltaCommand(plcAddr,":", wDataType, wStartAddr, wLen, 2, wDate, out rData, out er))
                return false;

            return true;
	    }
	    catch (Exception ex)
	    {
		  er=ex.ToString();
          return false;
	    }         
      }
      private bool writeDeltaREG(int plcAddr,ERegType wDataType, int wStartAddr, int wLen, string wWriteContent, out string rData, out string er)
      { 
        rData =string.Empty;
            
        er=string.Empty;

        try 
	    {	        
		    if(!SendDeltaCommand(plcAddr,":", wDataType, wStartAddr, wLen, 1, wWriteContent, out rData, out er))
                return false;

            return true;
	    }
	    catch (Exception ex)
	    {
		    er=ex.ToString();
            return false;
	    }         
      }
      private bool SendDeltaCommand(int plcAddr,string wCommandType, ERegType wDataType, int wStartAddr, int wWordCount, int wReadWhite, string wWriteContent,
                                    out string rData, out string er)
      {
          rData = string.Empty;

          er = string.Empty;

          try
          {
              string strCommand=string.Empty; 

              strCommand += plcAddr.ToString("X2");
              strCommand += getFunctioncode(wDataType,wReadWhite);
              strCommand += checkAddrs(wDataType,wStartAddr );
              strCommand += wWriteContent;
              strCommand += codeLRC(strCommand);
              strCommand += Chr(13);
              strCommand += Chr(10);
              strCommand = wCommandType+strCommand;
              int rLen = 0;
              int lngWordLen = 4;
              if (wReadWhite == 1)
              {
                  rLen = 17;
              }
              else
              {
                  rLen = 11 + lngWordLen * wWordCount;
              }
             
              if (!com.send(strCommand, rLen, out rData, out er))
                  return false;

              if (!verifyReceive(rData,ref er))
              {
                  er = er + rData;
                  return false;
              }
              if (wReadWhite == 1)      //write       
                  rData = "OK";
              else                                    //read
                  rData = rData.Substring(7, lngWordLen * wWordCount);

              return true;
          }
          catch (Exception e)
          {
              er = e.ToString();
              return false;
          }
      }
      /// <summary>
      /// 通讯命令;转换16进制地址
      /// </summary>
      /// <param name="wDataType"></param>
      /// <param name="wReadwrite"></param>
      /// <returns>getFunctioncode</returns>
      private string checkAddrs(ERegType wDataType, int wStartAddr)
      {
            int _addrs=0;
            if (wDataType == ERegType.X)
            {
                _addrs = 1024 + wStartAddr;
                return _addrs.ToString("X4");
            }
            else
                if (wDataType == ERegType.Y)
                {
                    _addrs = 1280 + wStartAddr;
                    return _addrs.ToString("X4");
                }
                else
                    if (wDataType == ERegType.M)
                    {
                        if (wStartAddr<1535 )
                            _addrs = 2048 + wStartAddr;
                        else
                            _addrs = 45056 + wStartAddr - 1536;
                        return _addrs.ToString("X4");
                    }
                    else
                    {
                        if (wDataType == ERegType.D)
                            if (wStartAddr < 4096)
                                _addrs = 4096 + wStartAddr;
                            else
                                _addrs = 36864 + wStartAddr - 4096;
                        return _addrs.ToString("X4");
                    }
      }
      /// <summary>
      /// LRC校验和
      /// </summary>
      /// <param name="lrcData"></param>
      /// <returns>codeLRC</returns>
      private string codeLRC(string lrcData)
      {
         int[] sum = new int[(lrcData.Length /2)];
         int lrcsum = 0;
         for (int i = 0; i < (lrcData.Length / 2); i++)
         {
             sum[i] = System.Convert.ToInt32(lrcData.Substring(i * 2, 2), 16);
             lrcsum += sum[i];
         }
         lrcsum = lrcsum % 256;//模FF
         lrcsum = ~lrcsum+1 ;//取反+1
         string lrcread = lrcsum.ToString("X8");
         lrcread = lrcread.Substring(lrcread.Length - 2, 2);
         return lrcread;

      }
      /// <summary>
      /// 通讯命令;02读取M,X,Y地址状态,03读取D地址数据,05设定M，Y状态,06设定D地址数据,
      /// </summary>
      /// <param name="wDataType"></param>
      /// <param name="wReadwrite"></param>
      /// <returns>getFunctioncode</returns>
      private string getFunctioncode(ERegType wDataType, int wReadwrite)
      {
        if (wReadwrite==1)
        {
            if (wDataType==ERegType.D)
              return "06";
            else
                return "05";
        }
        else
        {
            if (wDataType==ERegType.D)
                return "03";
            else
                return "02";
        }
      }
      /// <summary>
      /// ASCII转换
      /// </summary>
      /// <param name="asciiCode"></param>
      /// <returns>Chr</returns>
      public static string Chr(int asciiCode)
      {
          if (asciiCode >= 0 && asciiCode <= 255)
          {
              System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
              byte[] byteArray = new byte[] { (byte)asciiCode };
              string strCharacter = asciiEncoding.GetString(byteArray);
              return (strCharacter);
          }
          else
          {
              throw new Exception("ASCII Code is not valid.");
          }
      }
      /// <summary>
      /// 检查接收数据是否正确
      /// </summary>
      /// <param name="wResultString"></param>
      /// <param name="wAddr"></param>
      /// <param name="wCommandType"></param>
      /// <returns></returns>
      private bool verifyReceive(string wResultString, ref string er)
      {
          int inS1 = wResultString.IndexOf(Chr(58))+1;
          int inS2 = inS1 + (wResultString.Substring(inS1, wResultString.Length - inS1)).IndexOf(Chr(58)) + 1;
          if (inS2 > inS1)
          {
              inS1 = inS2;
          }
          if (inS1 > 0)
          {
              if (codeLRC(wResultString.Substring(1, wResultString.Length - 5)) == (wResultString.Substring(wResultString.Length - 4, 2)))
              {
                  return true;
              }
              else
              {
                  er = "ChkLRCErr:" + wResultString;
                  return false;             
              }
          }
          else
          {
              er = "NoLong:" + wResultString;
              return false;
          }
      }
      /// <summary>
      /// codeFcs校验和
      /// </summary>
      /// <param name="wData"></param>
      /// <returns></returns>
      private string codeFcs(string wData)
      {
          byte[] byteData = System.Text.Encoding.ASCII.GetBytes(wData);
          byte byteTemp = byteData[0];
          for (int i = 1; i < byteData.Length; i++)
              byteTemp ^= byteData[i];
          return byteTemp.ToString("X2");
      }
      /// <summary>
      /// 检查FCS校验和
      /// </summary>
      /// <param name="wResultString"></param>
      /// <returns></returns>
      private bool checkFcs(string wResultString)
      {
          int _LastCount = 4;
          string _temp = wResultString.Substring(0, wResultString.Length - _LastCount);
          string _Fcs = codeFcs(_temp);
          if (_Fcs != wResultString.Substring(wResultString.Length - _LastCount, 2))
              return false;
          return true;
      }
      #endregion    

    }
}
