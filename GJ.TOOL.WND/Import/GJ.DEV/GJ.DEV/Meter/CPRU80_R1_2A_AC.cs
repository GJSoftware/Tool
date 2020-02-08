using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;
using GJ.DEV.COM;

namespace GJ.DEV.Meter
{
   /*
    * 设置波特率方法:代号:1118 前2位设置为电表地址 后2位为波特率
    * 设置为12-->波特率1200
    * 设置为24->波特率2400
    * 设置为48-->波特率4800
    * 设置为96-->波特率9600
   */
   public class CPRU80_R1_2A_AC:IMeter
   {
      #region 构造函数
      public CPRU80_R1_2A_AC(int idNo = 0, string name = "PRU80_R1_2A_AC")
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
      private string _name = "PRU80_R1_2A_AC";
      private bool _conStatus = false;
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
      /// <param name="comName"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Open(string comName, out string er, string setting = "9600,n,8,1")
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
        /// 读电压
        /// </summary>
        /// <param name="acv"></param>
        /// <param name="er"></param>
        /// <returns></returns>
      public bool ReadACV(int devAddr,out double acv, out string er)
      {
        acv = 0;

        er = string.Empty;

        try
        {
            int regAddr = 0xd;

            string rData = string.Empty;

            if (!read(devAddr,regAddr, 1, out rData, out er))
                return false;

            int rVal = System.Convert.ToInt16(rData, 16);

            acv = ((double)rVal) / 10;

            return true;
        }
        catch (Exception ex)
        {
            er = ex.ToString();
            return false;
        }
      }
      /// <summary>
        /// 读电流
        /// </summary>
        /// <param name="acv"></param>
        /// <param name="er"></param>
        /// <returns></returns>
      public bool ReadACI(int devAddr, out double aci, out string er)
      {
        aci = 0;

        er = string.Empty;

        try
        {
            int regAddr = 0x10;

            string rData = string.Empty;

            if (!read(devAddr,regAddr, 1, out rData, out er))
                return false;

            int rVal = System.Convert.ToInt16(rData, 16);

            aci = ((double)rVal) / 10;

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
        /// 读寄存器值
        /// 从机地址(1Byte)+功能码(1Byte)+寄存器地址(2Byte)+地址数量(2Byte)+CRC检验(2Byte)
        /// </summary>
        /// <param name="devType">地址类型</param>
        /// <param name="startAddr">开始地址</param>
        /// <param name="N">地址长度</param>
        /// <param name="rData">16进制字符:数据值高位在前,低位在后</param>
        /// <param name="er"></param>
        /// <returns></returns>
      public bool read(int devAddr, int startAddr, int N, out string rData, out string er)
      {
        rData = string.Empty;

        er = string.Empty;

        try
        {
            string wCmd = devAddr.ToString("X2");
            int rLen = 0;
            wCmd += "03";      //寄存器功能码为03
            rLen = N * 2;
            wCmd += formatDevAddr(startAddr);  //开始地址
            wCmd += N.ToString("X4");                   //读地址长度
            wCmd += CCRC.Crc16(wCmd);                  //CRC16 低位前,高位后            
            if (!com.send(wCmd, 5 + rLen, out rData, out er))
                return false;
            if (!checkCRC(rData))
            {
                er = "crc16检验和错误:" + rData;
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
    /// 单写寄存器值
    /// 从机地址(1Byte)+功能码(1Byte)+寄存器地址(2Byte)+地址数量(2Byte)+字节数(1Byte)+数据+CRC检验(2Byte)
    /// </summary>
    /// <param name="devType">地址类型</param>
    /// <param name="startAddr">开始地址</param>
    /// <param name="wVal">单个值</param>
    /// <param name="er"></param>
    /// <returns></returns>
    public bool write(int devAddr, int startAddr, int wVal, out string er)
    {
        er = string.Empty;

        try
        {
            int N = 1;   //单写1个值
            string wCmd = devAddr.ToString("X2");
            int rLen = 0;
            int wLen = 0;
            string wData = string.Empty;
            wCmd += "10";        //寄存器功能码为16
            wLen = N * 2;          //写入字节数
            rLen = 8;           //回读长度
            wData = wVal.ToString("X" + wLen * 2);
            wCmd += formatDevAddr(startAddr);  //开始地址
            wCmd += N.ToString("X4");         //读地址长度
            wCmd += wLen.ToString("X2");     //写入字节数  
            wCmd += wData;                   //写入数据
            wCmd += CCRC.Crc16(wCmd);      //CRC16 低位前,高位后   
            string rData = string.Empty;
            if (!com.send(wCmd, rLen, out rData, out er))
                return false;
            if (!checkCRC(rData))
            {
                er = "crc16检验和错误:" + rData;
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
    /// 格式化地址段
    /// </summary>     
    private string formatDevAddr(int devAddr)
    {
        return devAddr.ToString("X4");
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
