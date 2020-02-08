using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;

namespace GJ.DEV.ELOAD
{
   public class CEL_40_08:IEL 
   {
      #region 构造函数
       public CEL_40_08(int idNo = 0, string name = "EL_40_8L")
       {
           _idNo = idNo;
           _name = name;
       }
      #endregion

      #region 字段
      private int _idNo = 0;
      private string _name = "EL_40_8L";
      private bool _conStatus = false;
      private CSerialPort com;
      private int ELCH = 4;
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
      public bool Open(string comName, out string er, string setting = "38400,n,8,1")
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
            string cmd0 = "00";
            string cmd1 = "00";
            string wCmd = cmd0 + cmd1;
            string wData = wAddr.ToString("X2");            
            wData = CalDataFromELCmd(0, wCmd, wData);
            string rData = string.Empty;
            int rLen = 6;
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
            string cmd0 = "01";
            string cmd1 = "01";
            string wCmd = cmd0 + cmd1;
            string wData = string.Empty;
            string rData = string.Empty;
            int rLen = 9;
            wData += wDataSet.SaveEEPROM.ToString("X2");   // D0 ――>设定写EEPROM状态00：不擦除旧数据及写新数据    01：擦除旧数据　　 02：写新数据
            wData += wDataSet.PWM_Status.ToString("X2");  //D1――>设定PWM状态 00：PWM＿stop　　01：PWM＿start
            wData += wDataSet.PWM_Freq.ToString("X4");    //D2.D3=Data  ――>设定PWM频率
            wData += wDataSet.PWM_DutyCycle.ToString("X4"); //D4.D5=Data  ――>设定PWM占空比
            wData += wDataSet.Run_Status.ToString("X2");  //D6. ――>设定工作状态　00：停止　　01：运行
            wData += "00"; //D7. ――>设定工作功率　00：40W　　01：80W
            for (int i = 0; i < ELCH; i++)   //(D8-D12) X4
            {
                //D8 ――>设定工作模式  00：CC模式 01：CV模式02：LED模式(1通道工作方式)
                wData += ((int)wDataSet.Run_Mode[i]).ToString("X2");

               //D9.D10=Data  ――>设定工作状态　工作数据 
               if (wDataSet.Run_Mode[i] == EMode.CC)             
                  wData += ((int)(wDataSet.Run_Val[i] * 1000)).ToString("X4");
               else
                  wData += ((int)(wDataSet.Run_Val[i] * 10)).ToString("X4");

               //D11.D12=Data ――>设定工作状态　V_ON数据
               wData += ((int)(wDataSet.Run_Von[i] * 10)).ToString("X4");   
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
      public  bool ReadELLoadSet(int wAddr, CEL_ReadSetPara rDataSet, out string er)
      {
          er = string.Empty;

          try
          {
              string cmd0 = "02";

              string cmd1 = "02";

              string wCmd = cmd0 + cmd1;

              string wData = string.Empty;

              string rData = string.Empty;

              int rLen = 47;

              int highByte = 0;

              int lowByte = 0;

              wData = CalDataFromELCmd(wAddr, wCmd, wData);

              if (!SendCmdToELoad(wData, rLen, out rData, out er))
                  return false;

              //D0――>读取PWM状态 00：PWM＿stop　01：PWM＿start

              //D1.D2=Data  ――>读取PWM频率
		      
              //D3.D4=Data  ――>读取PWM占空比

              string rValData = rData.Substring(10, rData.Length - 10);

              //单通道(D5-D13) X4

             //D5.――>读取工作状态　00：停止　　01：运行
             //D6.――>读取工作功率　00：40W　　01：80W
             //D7.――>读取工作模式  00：CC模式 01：CV模式02：LED模式
             //D8.D9=Data.  ――>读取工作状态　工作数据
             //D10.D11=Data ――>读取工作状态　V_ON数据
             //D12. 		 ――>读取工作状态　CC模式下过功率保护时间
             //D13. 		 ――>读取工作状态　CV、LED模式下过功率保护时间    

  
              for (int i = 0; i < ELCH; i++)
              {
                  string strCH = rValData.Substring(i*18,18);

                  if (strCH.Substring(0, 2) == "FF")   //D5-读取工作状态　00：停止　　01：运行
                  {
                      rDataSet.status[i] = "TimeOut";
                      continue;
                  }

                  rDataSet.status[i] = "OK";    //D6--读取工作功率　00：40W　　01：80W

                  rDataSet.LoadMode[i] = (EMode)System.Convert.ToInt16(strCH.Substring(4, 2), 16);  //D7:读取工作模式  00：CC模式 01：CV模式02：LED模式

                  highByte = System.Convert.ToInt16(strCH.Substring(6, 2), 16);

                  lowByte = System.Convert.ToInt16(strCH.Substring(8, 2), 16);

                  if (rDataSet.LoadMode[i] == EMode.CC)   //D8,D9-->读取工作状态　工作数据
                  {
                      rDataSet.LoadVal[i] = ((double)(highByte * 240 + lowByte)) / 1000;
                  }
                  else
                  {
                      rDataSet.LoadVal[i] = ((double)(highByte * 240 + lowByte)) / 10;
                  }

                  highByte = System.Convert.ToInt16(strCH.Substring(10, 2), 16);

                  lowByte = System.Convert.ToInt16(strCH.Substring(12, 2), 16);

                  //D10.D11=Data ――>读取工作状态　V_ON数据
                  rDataSet.Von[i] = ((double)(highByte * 240 + lowByte)) / 10;
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

              int rLen = 41;

              wData = CalDataFromELCmd(wAddr, wCmd, wData);

              if (!SendCmdToELoad(wData, rLen, out rData, out er))
                  return false;

              /*
                D0.     ――>读取工状态（Start、Stop）0,1
                D1.     ――>读取工作功率（40W、80W） 2,3
                D2. D3  ――>Vsen电压                 4,5,6,7
                D4. D5  ――>电压                     8,9,10,11
                D6. D7  ――>电流                     12,13,14,15
                (D0-D7) X4
                D32.     ――>OCP
                D33.     ――>OVP
                D34.     ――>OPP
                D35.     ――>OTP
              */

              int highByte = 0;

              int lowByte = 0;

              for (int i = 0; i < ELCH; i++)
              {
                  string strCH = rData.Substring(i * 16, 16);

                  highByte = System.Convert.ToInt16(strCH.Substring(4, 2), 16);
                  lowByte = System.Convert.ToInt16(strCH.Substring(6, 2), 16);
                  rDataVal.Vs[i] = ((double)(highByte * 240 + lowByte)) / 10;

                  highByte = System.Convert.ToInt16(strCH.Substring(8, 2), 16);
                  lowByte = System.Convert.ToInt16(strCH.Substring(10, 2), 16);
                  rDataVal.Volt[i] = ((double)(highByte * 240 + lowByte)) / 10;

                  highByte = System.Convert.ToInt16(strCH.Substring(12, 2), 16);
                  lowByte = System.Convert.ToInt16(strCH.Substring(14, 2), 16);
                  rDataVal.Load[i] = ((double)(highByte * 240 + lowByte)) / 1000;
              }

              string strDD = rData.Substring(rData.Length - 8, 8);

              rDataVal.OCP = System.Convert.ToInt16(strDD.Substring(0, 2), 16);
              rDataVal.OVP = System.Convert.ToInt16(strDD.Substring(2, 2), 16);
              rDataVal.OPP = System.Convert.ToInt16(strDD.Substring(4, 2), 16);
              rDataVal.OTP = System.Convert.ToInt16(strDD.Substring(6, 2), 16);
              if (rDataVal.OCP == 1 || rDataVal.OVP == 1 || rDataVal.OPP == 1 || rDataVal.OTP == 1)
              {
                  rDataVal.Status = "Alarm:";
                  if (rDataVal.OCP == 1)
                      rDataVal.Status += "OCP;";
                  if (rDataVal.OVP == 1)
                      rDataVal.Status += "OVP;";
                  if (rDataVal.OPP == 1)
                      rDataVal.Status += "OPP;";
                  if (rDataVal.OTP == 1)
                      rDataVal.Status += "OTP;";
              }
              else
                  rDataVal.Status = "OK";
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
       * 发送:桢头(FE)+地址+命令01+命令02+长度+数据+检验和+桢尾(FF)
       * 应答:桢头(FE)+地址+长度+数据+检验和+桢尾(FF)         
      */
      private const string SOI = "FE";
      private const string EOI = "FF";
      /// <summary>
      /// 协议格式
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="wCmd"></param>
      /// <param name="wData"></param>
      /// <returns></returns>
      private string CalDataFromELCmd(int wAddr,string wCmd,string wData)
      {
         string cmd = string.Empty;
         int len=4+wData.Length/2;
         string chkData = string.Empty; 
         for (int i = 0; i < wData.Length/2; i++)
         {
            if (wData.Substring(i * 2, 2) == SOI)
               chkData += "FC";
            else if(wData.Substring(i * 2, 2) == EOI)
               chkData += "FB";
            else
               chkData += wData.Substring(i * 2, 2);
         }
         cmd = wAddr.ToString("X2") + wCmd + len.ToString("X2") + chkData;
         cmd = SOI + cmd + CalCheckSum(cmd) + EOI;
         return cmd; 
      }
      /// <summary>
      /// 检验和-(地址+命令01+命令02+长度+数据)%256
      /// </summary>
      /// <param name="wData"></param>
      /// <returns></returns>
      private string CalCheckSum(string wData)
      {
         int sum = 0;
         for (int i = 0; i < wData.Length /2 ; i++)
            sum += System.Convert.ToInt32(wData.Substring(i * 2, 2), 16); 
         sum %= 0x100;
         if (sum == 0xFE)
            sum -= 2;
         else if (sum == 0xFF)
            sum -= 4;
         return sum.ToString("X2"); 
      }
      /// <summary>
      /// 检验和
      /// </summary>
      /// <param name="wData"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      private bool EL_CheckSum(string wData, out string er)
      {
          er = string.Empty;

          try
          {
              if (wData.Substring(0, 2) != SOI)
              {
                  er = "数据桢头错误:" + wData;
                  return false;
              }
              if (wData.Substring(wData.Length - 2, 2) != EOI)
              {
                  er = "数据桢尾错误:" + wData;
                  return false;
              }
              if (wData.Length / 2 < 6)
              {
                  er = "数据长度小于6:" + wData;
                  return false;
              }
              int rLen = System.Convert.ToInt16(wData.Substring(4, 2), 16);
              if ((wData.Length / 2) != (rLen + 3))
              {
                  er = "数据长度错误:" + wData;
                  return false;
              }
              string chkStr = wData.Substring(2, wData.Length - 6);
              string chkSum = wData.Substring(wData.Length - 4, 2);
              if (chkSum != CalCheckSum(chkStr))
              {
                  er = "数据CheckSum错误:" + wData;
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
      private bool SendCmdToELoad(string wData, int rLen, out string rData, out string er, int wTimeOut = 1000)
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
                  rData = recvData.Substring(6, recvData.Length - 10);
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
