using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;
using GJ.COM;

namespace GJ.DEV.ELOAD
{
    public class CEL_100_04 : IEL
    {
        #region 构造函数
        public CEL_100_04(int idNo = 0, string name = "EL_100_04_N5")
        {
            _idNo = idNo;
            _name = name;
        }
        #endregion

        #region 字段
        private int _idNo = 0;
        private string _name = "EL_100_04_N5";
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
      /// 读线圈
      /// 从机地址(1Byte)+功能码(1Byte)+寄存器地址(2Byte)+地址数量(2Byte)+CRC检验(2Byte)
      /// </summary>
      /// <param name="devType">地址类型</param>
      /// <param name="regAddr">开始地址</param>
      /// <param name="N">地址长度</param>
      /// <param name="rData">16进制字符:数据值高位在前,低位在后</param>
      /// <param name="er"></param>
      /// <returns></returns>
        public bool Read(int devAddr, int regAddr, int N, out string rData, out string er)
        {
          rData = string.Empty;

          er = string.Empty;

         try
         {

             int rLen  = N * 2;

             string wCmd = devAddr.ToString("X2");

             wCmd += "03";      //寄存器功能码为03
             
             wCmd += regAddr.ToString("X4");  //开始地址

             wCmd += N.ToString("X4");        //读地址长度---1Byte

             wCmd += CCRC.Crc16(wCmd);                  //CRC16 低位前,高位后     
       
             if (!com.send(wCmd, 5 + rLen, out rData, out er))
                 return false;

             if (!checkCRC(rData))
             {
                 er = CLanguage.Lan("crc16检验和错误") + ":" + rData;
                 return false;
             }
            
            string temp = rData.Substring(6, rLen * 2);

            rData = temp;     //2个字节为寄存器值，高在前,低位在后，寄存器小排最前面；

            //转换为寄存器小排最后

            rData = string.Empty;

            for (int i = 0; i < temp.Length / 4; i++)
            {
                rData = temp.Substring(i * 4, 4) + rData;
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
        public bool Read(int devAddr, int regAddr, out int rVal, out string er)
        {
          rVal = -1;

          er = string.Empty;

          try
          {
              int N = 1;

              string rData = string.Empty;

              if (!Read(devAddr, regAddr, N, out rData, out er))
                  return false;

              rVal = System.Convert.ToInt16(rData.Substring(rData.Length - 4, 4), 16);

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
        public bool Read(int devAddr, int regAddr, ref int[] rVal, out string er)
       {
          er = string.Empty;

          try
          {
              string rData = string.Empty;

              int N = rVal.Length;

              if (!Read(devAddr,regAddr, N, out rData, out er))
                  return false;
       
              for (int i = 0; i < N; i++)
                rVal[i] = System.Convert.ToInt16(rData.Substring(rData.Length - (i + 1) * 4, 4), 16);
              
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
        public bool Write(int devAddr, int regAddr, int wVal, out string er)
        {
          er = string.Empty;

          try
          {
              int N = 1;   //单写1个值

              int rLen = 8;  //回读长度

              int wLen = N * 2;

              string wData = string.Empty;

              wData = wVal.ToString("X" + wLen * 2);

              string wCmd = devAddr.ToString("X2");   //设备地址
              wCmd += "07";                          //寄存器功能码0x6 -不保存 EPROM 0x07->保存EPROM          
              wCmd += regAddr.ToString("X4");        //开始地址
              wCmd += wData;                         //写入数据
              wCmd += CCRC.Crc16(wCmd);              //CRC16 低位前,高位后   
              
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
        public bool Write(int devAddr, int regAddr, int[] wVal, out string er)
        {
          er = string.Empty;

          try
          {
              int N = wVal.Length;   //单写多个值
            
              int rLen = 8;

              int wLen= N * 2;

              string wData = string.Empty;
              for (int i = 0; i < N; i++)
              {
                 wData += wVal[i].ToString("X4");
              }

              string wCmd = devAddr.ToString("X2");
              
              wCmd += "11";        //寄存器功能码为0x10 -不保存EPROM 0x11->保存EPROM  
              wCmd += regAddr.ToString("X4");  //开始地址
              wCmd += N.ToString("X4");                  //读地址长度
              wCmd += wLen.ToString("X2");               //写入字节数  
              wCmd += wData;                             //写入数据
              wCmd += CCRC.Crc16(wCmd);   
             
              //CRC16 低位前,高位后   
              rLen = 8;

              if (devAddr == 0)
                  rLen = 0;
              
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
        /// 设置新地址-->0x0A00:默认值0x64=100
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SetNewAddr(int wAddr, out string er)
        {
            er = string.Empty;

            try
            {
                if (!Write(0, 0x0A00, wAddr, out er))
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

                int[] Val = new int[ELCH * 4];

                for (int i = 0; i < ELCH; i++)
                {
                    //设定工作模式--0x0A08

                    Val[i * ELCH + 0] = (int)wDataSet.Run_Mode[i]; 

                    //设定通道工作数据1--0x0A09

                    if (wDataSet.Run_Mode[i] == EMode.CC)
                        Val[i * ELCH + 1] = (int)(wDataSet.Run_Val[i] * 1000);
                    else
                        Val[i * ELCH + 1] = (int)(wDataSet.Run_Val[i] * 100);

                    //设定通道工作数据2--0x0A0A

                    Val[i * ELCH + 2] = (int)(wDataSet.Run_Von[i] * 100);

                    //设定通道工作数据3--0x0A0B

                    Val[i * ELCH + 3] = 0;
                }

                if (!Write(wAddr, 0x0A08, Val, out er))
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

                int[] Val = new int[ELCH * 4];

                if (!Read(wAddr, 0x0A08, ref Val, out er))
                    return false;

                for (int i = 0; i < ELCH; i++)
                {
                    rDataSet.status[i] = "OK";

                    rDataSet.LoadMode[i] = (EMode)Val[i * ELCH + 0];

                    if (rDataSet.LoadMode[i] == EMode.CC)
                        rDataSet.LoadVal[i] = (double)Val[i * ELCH + 1] / 1000;
                    else
                        rDataSet.LoadVal[i] = (double)Val[i * ELCH + 1] / 100;

                    rDataSet.Von[i] = (double)Val[i * ELCH + 2] / 100;

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
                int[] rOnOff = new int[1];

                if (!Read(wAddr, 0x0B14, ref rOnOff, out er))
                    return false;

                rDataVal.ONOFF = rOnOff[0];

                int[] Val = new int[ELCH * 2 + 1];

                if (!Read(wAddr, 0x0B16, ref Val, out er))
                    return false;

                for (int i = 0; i < ELCH; i++)
                {
                    rDataVal.Volt[i] = (double)Val[i * 2 + 0] / 100;

                    rDataVal.Load[i] = (double)Val[i * 2 + 1] / 1000;
                }

                int Status = Val[ELCH * 2];

                if ((Status & (1 << 0)) != 0 || (Status & (1 << 4)) != 0 || (Status & (1 << 8)) != 0 || (Status & (1 << 12)) != 0)
                {
                     rDataVal.OTP = 1;
                }
                if ((Status & (1 << 1)) != 0 || (Status & (1 << 5)) != 0 || (Status & (1 << 9)) != 0 || (Status & (1 << 13)) != 0)
                {
                     rDataVal.OPP = 1;
                }
                if ((Status & (1 << 2)) != 0 || (Status & (1 << 6)) != 0 || (Status & (1 << 10)) != 0 || (Status & (1 << 14)) != 0)
                {
                     rDataVal.OCP = 1;
                }
                if ((Status & (1 << 3)) != 0 || (Status & (1 << 7)) != 0 || (Status & (1 << 11)) != 0 || (Status & (1 << 15)) != 0)
                {
                     rDataVal.OVP = 1;
                }
              
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
                {
                    rDataVal.Status = "OK";
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
            if (wCmd == string.Empty)
                return true;
            string crc = CCRC.Crc16(wCmd.Substring(0, wCmd.Length - 4));
            if (crc != wCmd.Substring(wCmd.Length - 4, 4))
                return false;
            return true;
        }
        #endregion
    }
}
