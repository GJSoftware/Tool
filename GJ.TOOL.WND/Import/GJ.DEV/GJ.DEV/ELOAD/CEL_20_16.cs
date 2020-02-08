using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;
using GJ.DEV.COM;

namespace GJ.DEV.ELOAD
{
   public class CEL_20_16:IEL
   {
      #region 构造函数
      public CEL_20_16(int idNo = 0, string name = "EL_20_16")
      {
          _idNo = idNo;
          _name = name;
      }
      #endregion

      #region 字段
      private int _idNo = 0;
      private string _name = "EL_20_16";
      private bool _conStatus = false;
      private CSerialPort com;
      private int ELCH = 8;
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
      /// <summary>
      /// 负载通道数
      /// </summary>
      public int maxCH
      { 
          get { return ELCH; }
      }
      #endregion

      #region 方法
      /// <summary>
      /// 57600,n,8,1
      /// </summary>
      /// <param name="comName"></param>
      /// <param name="setting"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Open(string comName, out string er, string setting = "57600,n,8,1")
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
      /// 关闭设备
      /// </summary>
      /// <returns></returns>
      public void Close()
      {
          if (com == null)
              return;
          com.close();
          com = null;
          _conStatus = false;
      }
      /// <summary>
      /// 设置新地址
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetNewAddr(int wAddr, out string er)
      {
         er = string.Empty;

         try
         {
            string cmd0="00";
            string cmd1="00";
            string wCmd=cmd0 +cmd1;
            string wData=wAddr.ToString("X2");             
            string rData=string.Empty;
            int rLen = 7;
            wData=CalDataFromELCmd(0,wCmd,wData);
            if(!SendCmdToELoad(wData,rLen,out rData , out er))
               return false;
            return true;
         }
         catch (Exception ex)
         {
            er = ex.ToString();
            return false;
         }
      }
      /// <summary>
      /// 设置负载值
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="wDataSet"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetELData(int wAddr, CEL_SetPara wDataSet, out string er)
      {
         er = string.Empty;

         try
         {
            string cmd0="01";
            string cmd1="01";
            string wCmd=cmd0 +cmd1;
            string wData=string.Empty;             
            string rData=string.Empty;
            int rLen = 0;            
            wData +=wDataSet.SaveEEPROM.ToString("X2");     //D0
            wData +=wDataSet.OTP_Start.ToString("X2");      //D1
            wData +=wDataSet.OTP_Stop.ToString("X2");       //D2
            wData +=wDataSet.PWM_Status.ToString("X2");     //D3
            wData +=wDataSet.PWM_Freq.ToString("X4");       //D4,D5
            wData +=wDataSet.PWM_DutyCycle.ToString("X4");  //D6,D7
            wData +=wDataSet.Run_Status.ToString("X2");     //D8
            wData +=wDataSet.Run_Power[0].ToString("X2");   //D9
            for (int i = 0; i < ELCH; i++)
			{
			    wData +=((int)wDataSet.Run_Mode[i]).ToString("X2");            //D10
            if (wDataSet.Run_Mode[i] == EMode.CC)
                wData += ((int)(wDataSet.Run_Val[i]*1000)).ToString("X4");     //D11,D12
            else
                wData += ((int)(wDataSet.Run_Val[i] * 100)).ToString("X4");  
            wData += ((int)(wDataSet.Run_Von[i] * 100)).ToString("X4");        //D13,D14
			}
            wData = CalDataFromELCmd(wAddr, wCmd, wData);
            if (!SendCmdToELoad(wData, rLen, out rData, out er))
               return false;              
            return true;
         }
         catch (Exception ex)
         {
            er = ex.ToString();
            return false;
         }
      }
      /// <summary>
      /// 读取负载设定值
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="rDataSet"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadELLoadSet(int wAddr, CEL_ReadSetPara rDataSet, out string er)
      {
         er = string.Empty;

         try
         {
            string cmd0="02";
            string cmd1="02";
            string wCmd=cmd0 +cmd1;
            string wData=string.Empty;             
            string rData=string.Empty;
            int rLen = 55;
            wData = CalDataFromELCmd(wAddr, wCmd, wData);
            if (!SendCmdToELoad(wData, rLen, out rData, out er))
               return false;   
            for (int i = 0; i < ELCH; i++)
			   {
			     if(rData.Substring(i*10+18,2)=="FF")
              {
                 rDataSet.status[i]="TimeOut"; 
                 continue; 
              }
              rDataSet.status[i]="OK";
              rDataSet.LoadMode[i]=(EMode)System.Convert.ToInt16(rData.Substring(i*10+18,2),16); 
              if(rDataSet.LoadMode[i]==EMode.CC)
                  rDataSet.LoadVal[i] = ((double)System.Convert.ToInt16(rData.Substring(i * 10 + 18 + 2, 4), 16)) / 1000;   
              else
                  rDataSet.LoadVal[i] = ((double)System.Convert.ToInt16(rData.Substring(i * 10 + 18 + 2, 4), 16)) / 100;
              rDataSet.Von[i] = ((double)System.Convert.ToInt16(rData.Substring(i * 10 + 18 + 6, 4), 16)) / 100;
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
      /// 读取负载数据
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="rDataVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadELData(int wAddr, CEL_ReadData rDataVal, out string er)
      {
         er = string.Empty;

         try
         {
            string cmd0 = "02";
            string cmd1 = "01";
            string wCmd = cmd0 + cmd1;
            string wData = string.Empty;
            string rData = string.Empty;
            int rLen = 61;
            wData = CalDataFromELCmd(wAddr, wCmd, wData);
            if (!SendCmdToELoad(wData, rLen, out rData, out er))
               return false;   
            rDataVal.NTC_0=System.Convert.ToInt16(rData.Substring(0,2),16);   
            rDataVal.NTC_1=System.Convert.ToInt16(rData.Substring(2,2),16);
            rDataVal.ONOFF=System.Convert.ToInt16(rData.Substring(4,2),16);
            for (int i = 0; i < ELCH; i++)
			{
			   rDataVal.Vs[i]=((double)System.Convert.ToInt16(rData.Substring(6 + i * 12,4),16))/100;
               rDataVal.Volt[i] =((double)System.Convert.ToInt16(rData.Substring(10 + i * 12, 4), 16)) / 100;
               rDataVal.Load[i]=((double)System.Convert.ToInt16(rData.Substring(14 + i * 12,4),16))/1000;
			}
            rDataVal.OCP=System.Convert.ToInt16(rData.Substring(102,2),16)/100;
            rDataVal.OVP=System.Convert.ToInt16(rData.Substring(104,2),16)/100;
            rDataVal.OPP=System.Convert.ToInt16(rData.Substring(106,2),16)/100;
            rDataVal.OTP=System.Convert.ToInt16(rData.Substring(108,2),16)/100;
            if(rDataVal.OCP==1 || rDataVal.OVP==1 || rDataVal.OPP ==1 || rDataVal.OTP ==1)
            {
               rDataVal.Status="Alarm:";
               if(rDataVal.OCP ==1)
                  rDataVal.Status +="OCP;";
               if(rDataVal.OVP ==1)
                  rDataVal.Status +="OVP;";
               if(rDataVal.OPP ==1)
                  rDataVal.Status +="OPP;";
               if(rDataVal.OTP ==1)
                  rDataVal.Status +="OTP;";
            }
            else
               rDataVal.Status="OK";  
            return true;
         }
         catch (Exception ex)
         {
            er = ex.ToString();
            return false;
         }
      }
      #endregion

      #region 协议
      /*
       * 发送:桢头+地址+命令01+命令02+长度+数据+CRC16_L+CRC16_H
       * 应答:桢头+地址+长度+数据+CRC16_L+CRC16_H
      */
      private string SOI = "AA";
      /// <summary>
      /// 协议格式
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="wCmd"></param>
      /// <param name="wData"></param>
      /// <returns></returns>
      private string CalDataFromELCmd(int wAddr,string wCmd,string wData)
      {
          try
          {
              string cmd = string.Empty;
              int nLen = 1 + 1 + 2 + 1 + wData.Length / 2;
              cmd = SOI + wAddr.ToString("X2") + wCmd + nLen.ToString("X2") + wData;
              cmd += CCRC.Crc16(cmd);
              return cmd;
          }
          catch (Exception ex)
          {
              return ex.ToString();
          }          
      }
      /// <summary>
      /// 检验和
      /// </summary>
      /// <param name="wData"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      private bool EL_CheckSum(string wData,out string er)
      {
          er = string.Empty;

          try
          {
              if (wData.Substring(0, 2) != SOI)
              {
                  er = "数据桢头错误:" + wData;
                  return false;
              }
              if (wData.Length / 2 < 6)
              {
                  er = "数据长度小于6:" + wData;
                  return false;
              }
              int rLen = System.Convert.ToInt16(wData.Substring(4, 2), 16);
              if ((wData.Length / 2) != (rLen + 2))
              {
                  er = "数据长度错误:" + wData;
                  return false;
              }
              string chkStr = wData.Substring(0, rLen * 2);
              string chkSum = wData.Substring(rLen * 2, 4);
              if (chkSum != CCRC.Crc16(chkStr))
              {
                  er = "数据CheckSum错误:" + wData;
                  return false;
              }
              if (wData.Substring(6, 2) != "01")
              {
                  er = "返回数据确认FAIL:" + wData;
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
      /// 发送和接收数据
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="wData"></param>
      /// <param name="rLen"></param>
      /// <param name="rData"></param>
      /// <param name="er"></param>
      /// <param name="wTimeOut"></param>
      /// <returns></returns>
      private bool SendCmdToELoad(string wData,int rLen,out string rData,out string er,int wTimeOut=300)
      {
          rData = string.Empty;

          er = string.Empty;

          try
          {
              string recvData = string.Empty;
              if (!com.send(wData, rLen, out recvData, out er, wTimeOut))
                  return false;
              if (rLen > 0)
              {
                  if (!EL_CheckSum(recvData, out er))
                      return false;
                  rData = recvData.Substring(8, recvData.Length - 12);
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

   }
}
