using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;

namespace GJ.DEV.I2C
{
    public class CI2C_Server:II2C
    {
        
     #region 构造函数
     public CI2C_Server(int idNo = 0, string name = "I2C_Server")
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
     private string _name = "I2C_Server";
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
     /// <param name="setting"></param>
     /// <returns></returns>
     public bool Open(string comName,  out string er,string setting="57600,n,8,1")
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
     /// 设置地址
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
             
             wData = CalDataFromDDCmd(0, wCmd, wData);
             
             string rData = string.Empty;
             
             int rLen = 7;

             if (!SendCmdToDDLoad(wData, rLen, out rData, out er))
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
     /// 读取版本
     /// </summary>
     /// <param name="wAddr"></param>
     /// <param name="ver"></param>
     /// <param name="er"></param>
     /// <returns></returns>
     public bool ReadVersion(int wAddr,out string ver, out string er)
     {
         ver = string.Empty;

         er = string.Empty;

         try
         {
             string cmd0 = "02";

             string cmd1 = "05";

             string wCmd = cmd0 + cmd1;

             string wData = string.Empty;

             wData = CalDataFromDDCmd(wAddr, wCmd, wData);

             string rData = string.Empty;

             int rLen = 8;

             if (!SendCmdToDDLoad(wData, rLen, out rData, out er))
                 return false;

             double val = (double)System.Convert.ToInt16(rData,16) / 10;

             ver = val.ToString("0.0");

             return true;
         }
         catch (Exception ex)
         {
             er = ex.ToString();
             return false;
         }
     }
     /// <summary>
     /// 设置I2C运行参数
     /// </summary>
     /// <param name="wAddr"></param>
     /// <param name="para"></param>
     /// <param name="er"></param>
     /// <returns></returns>
     public bool SendToSetI2C_RunPara(int wAddr, CI2C_RunPara para, out string er)
     {
         er = string.Empty;

         try
         {
             string cmd0 = "01";

             string cmd1 = "04";

             string wCmd = cmd0 + cmd1;

             string wData = string.Empty;

             wData += ((int)para.PlaceType).ToString("X2");

             wData += ((int)para.ReadType).ToString("X2");

             wData += para.ScanCycle.ToString("X2");

             wData += para.ACONDelay.ToString("X2");

             wData += ((int)para.RunI2CType).ToString("X2");

             wData += para.RdCmdNum.ToString("X2");

             for (int i = 0; i < para.RdCmdNum; i++)
             {
                 wData += para.I2C_Addr; 
                 wData += para.Cmd[i].CmdOP;
                 wData += para.Cmd[i].RegNo;  
             }

             wData = CalDataFromDDCmd(wAddr, wCmd, wData);

             string rData = string.Empty;

             int rLen = 8;

             if (!SendCmdToDDLoad(wData, rLen, out rData, out er))
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
     /// 读取I2C运行参数
     /// </summary>
     /// <param name="wAddr"></param>
     /// <param name="para"></param>
     /// <param name="er"></param>
     /// <returns></returns>
     public bool ReadI2C_RunPara(int wAddr,ref CI2C_RunPara para, out string er)
     {
         er = string.Empty;

         try
         {
             string cmd0 = "02";

             string cmd1 = "08";

             string wCmd = cmd0 + cmd1;

             string wData = string.Empty;

             wData = CalDataFromDDCmd(wAddr, wCmd, wData);

             string rData = string.Empty;

             int rLen = 13 + 3 * 0;

             if (!SendCmdToDDLoad(wData, rLen, out rData, out er))
                 return false;

             para.PlaceType = (EPlace)System.Convert.ToInt16(rData.Substring(0, 2),16);

             para.ReadType = (EReadType)System.Convert.ToInt16(rData.Substring(2, 2), 16);

             para.ScanCycle = System.Convert.ToInt16(rData.Substring(4, 2), 16);

             para.ACONDelay = System.Convert.ToInt16(rData.Substring(6, 2), 16);

             para.RunI2CType = (EModel)System.Convert.ToInt16(rData.Substring(8, 2), 16);

             para.RdCmdNum = System.Convert.ToInt16(rData.Substring(10, 2), 16);

             for (int i = 0; i < para.RdCmdNum; i++)
             {
                 para.I2C_Addr = rData.Substring(12 + i * 6, 2);

                 para.Cmd[i].CmdOP = rData.Substring(14 + i * 6, 2);

                 para.Cmd[i].RegNo = rData.Substring(14 + i * 6, 2);
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
     /// 读取I2C数据
     /// </summary>
     /// <param name="wAddr"></param>
     /// <param name="uutNo">产品1或产品2</param>
     /// <param name="data"></param>
     /// <param name="er"></param>
     /// <returns></returns>
     public bool ReadI2C_Data(int wAddr, int uutNo, ref CI2C_Data data, out string er)
     {
         er = string.Empty;

         try
         {
             string cmd0 = "02";

             string cmd1 = uutNo.ToString("X2");

             string wCmd = cmd0 + cmd1;

             string wData = string.Empty;

             wData = CalDataFromDDCmd(wAddr, wCmd, wData);

             string rData = string.Empty;

             int rLen = 9 + 4 * data.CmdNum;

             if (!SendCmdToDDLoad(wData, rLen, out rData, out er))
                 return false;

             int sgnVal = System.Convert.ToInt16(rData.Substring(0, 2));

             if ((sgnVal & (1 << 3)) == (1 << 3))
                 data.AC_ONOFF = 1;
             else
                 data.AC_ONOFF = 0;

             if(data.CmdNum==0)
                 data.CmdNum=(rData.Length / 2 - 1)/4;

             for (int i = 0; i < data.CmdNum; i++)
             {
                 data.Val[i].CmdNo = System.Convert.ToInt16(rData.Substring(2 + 8 * i , 2));

                 data.Val[i].RdStatus = System.Convert.ToInt16(rData.Substring(4 + 8 * i, 2));

                 data.Val[i].RdByte0 = System.Convert.ToInt16(rData.Substring(6 + 8 * i, 2));

                 data.Val[i].RdByte1 = System.Convert.ToInt16(rData.Substring(8 + 8 * i, 2));  
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

     #region 协议
     /* 
       * 发送:桢头(FD)+地址+命令01+命令02+长度+数据+检验和+桢尾(FE)
       * 应答:桢头(FD)+地址+长度+数据+检验和+桢尾(FE)         
      */
     private string SOI = "EE";
     private string EOI = "EF";
     private string ROI = "ED";
     /// <summary>
     /// 发送和接收数据
     /// </summary>
     /// <param name="wData"></param>
     /// <param name="rLen"></param>
     /// <param name="rData"></param>
     /// <param name="er"></param>
     /// <param name="wTimeOut"></param>
     /// <returns></returns>
     private bool SendCmdToDDLoad(string wData, int rLen, out string rData, out string er, int wTimeOut = 200)
     {
         er = string.Empty;

         rData = string.Empty;

         try
         {
             string recvData = string.Empty;
             if (!com.send(wData, rLen, out recvData, out er, wTimeOut))
                 return false;
             if (rLen > 0)
             {
                 if (!DD_CheckSum(recvData, ref er))
                     return false;
                 rData = recvData.Substring(10, recvData.Length - 14);
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
     /// 数据格式化
     /// </summary>
     /// <param name="wAddr"></param>
     /// <param name="wCmd"></param>
     /// <param name="wData"></param>
     /// <returns></returns>
     private string CalDataFromDDCmd(int wAddr, string wCmd, string wData)
     {
         try
         {
             string cmd = string.Empty;
             int len = 4 + wData.Length / 2;
             string chkData = string.Empty;
             for (int i = 0; i < wData.Length / 2; i++)
             {
                 if (wData.Substring(i * 2, 2) == SOI || wData.Substring(i * 2, 2) == EOI)
                     chkData += ROI;
                 else
                     chkData += wData.Substring(i * 2, 2);
             }
             cmd = wAddr.ToString("X2") + wCmd + len.ToString("X2") + chkData;
             cmd = SOI + cmd + CalCheckSum(cmd) + EOI;
             return cmd;
         }
         catch (Exception)
         {             
             throw;
         }        
     }
     /// <summary>
     /// CheckSum检查
     /// </summary>
     /// <param name="wData"></param>
     /// <param name="er"></param>
     /// <returns></returns>
     private bool DD_CheckSum(string wData, ref string er)
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
        int rLen = System.Convert.ToInt16(wData.Substring(8, 2), 16);
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
     /// <summary>
     /// 检验和-(地址+命令01+命令02+长度+数据)%256
     /// </summary>
     /// <param name="wData"></param>
     /// <returns></returns>
     private string CalCheckSum(string wData)
     {
        int sum = 0;
        for (int i = 0; i < wData.Length / 2; i++)
        {
           sum += System.Convert.ToInt16(wData.Substring(i * 2, 2), 16);
        }
        string checkSum = sum.ToString("X4");
        checkSum = checkSum.Substring(2, 2);
        if (checkSum == SOI || checkSum == EOI)
           checkSum = ROI;
        return checkSum;
     }
     #endregion

    }
}
