using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;

namespace GJ.DEV.DD
{
    public class CDD_75V:IDD
    {

     #region 构造函数
      public CDD_75V(int idNo = 0, string name = "GJDD_75V")
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
     private string _name = "GJDD_75V";
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
     public bool Open(string comName, out string er, string setting)
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
     /// 设置新负载值
     /// </summary>
     /// <param name="wAddr"></param>
     /// <param name="loadPara"></param>
     /// <param name="er"></param>
     /// <returns></returns>
     public bool SetNewLoad(int wAddr, CwLoad loadPara, out string er)
     {
         er = string.Empty;

         try
         {
             string cmd0 = "01";
             string cmd1;
             if (loadPara.saveEEPROM == 1)
                 cmd1 = "06";
             else
                 cmd1 = "07";
             string wCmd = cmd0 + cmd1;
             string wData = string.Empty;
             for (int i = 0; i < loadPara.loadVal.Length; i++)
                 wData += ((int)(loadPara.loadVal[i] * 10)).ToString("X2");
             wData = CalDataFromDDCmd(wAddr, wCmd, wData);
             string rData = string.Empty;
             int rLen = 0;
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
     /// 设置负载值
     /// </summary>
     /// <param name="wStartAddr"></param>
     /// <param name="wEndAddr"></param>
     /// <param name="loadPara"></param>
     /// <param name="er"></param>
     /// <returns></returns>
     public bool SetNewLoad(int wStartAddr, int wEndAddr, CwLoad loadPara, out string er)
     {
         er = string.Empty;

         try
         {
             string cmd0 = "00";
             string cmd1;
             if (loadPara.saveEEPROM == 0)
                 cmd1 = "05";
             else
                 cmd1 = "04";
             string wCmd = cmd0 + cmd1;
             string wData = wStartAddr.ToString("X2") + wEndAddr.ToString("X2");
             for (int i = 0; i < loadPara.loadVal.Length; i++)
                 wData += ((int)(loadPara.loadVal[i] * 10)).ToString("X2");
             wData = CalDataFromDDCmd(0, wCmd, wData);
             string rData = string.Empty;
             int rLen = 0;
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
     /// 读取负载值
     /// </summary>
     /// <param name="wAddr"></param>
     /// <param name="loadSet"></param>
     /// <param name="er"></param>
     /// <returns></returns>
     public bool ReadLoadSet(int wAddr, ref CrLoad loadSet, out string er)
     {
         try
         {
             string cmd0 = "02";
             string cmd1 = "05";
             string wCmd = cmd0 + cmd1;
             string wData = string.Empty;
             wData = CalDataFromDDCmd(wAddr, wCmd, wData);
             string rData = string.Empty;
             int rLen = 16;
             if (!SendCmdToDDLoad(wData, rLen, out rData, out er))
                 return false;
             for (int i = 0; i < rData.Length / 2; i++)
             {
                 loadSet.loadVal[i] = ((double)System.Convert.ToInt32(rData.Substring(i * 2, 2), 16)) / 10;
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
     /// 读取数据
     /// </summary>
     /// <param name="wAddr"></param>
     /// <param name="rDataVal"></param>
     /// <param name="er"></param>
     /// <returns></returns>
     public bool ReadData(int wAddr, ref CrData rDataVal, out string er)
     {
         er = string.Empty;

         try
         {
             //读取12/26个电压通道的数据及一个含模块工作壮况的Byte
             string cmd0 = "02";
             string cmd1 = "02";
             string wCmd = cmd0 + cmd1;
             string wData = string.Empty;
             wData = CalDataFromDDCmd(wAddr, wCmd, wData);
             string rData = string.Empty;
             int rLen = 33;
             if (!SendCmdToDDLoad(wData, rLen, out rData, out er))
                 return false;
             for (int i = 0; i < (rData.Length - 2) / 4; i++)
             {
                 rDataVal.Volt[i] = ((double)(System.Convert.ToInt32(rData.Substring(i * 4, 4), 16))) / 1000;
             }
             int statusVal = System.Convert.ToInt16(rData.Substring(rData.Length - 2, 2), 16);
             rDataVal.Status = "";
             if ((statusVal & 1 << 0) == (1 << 0))
             {
                 rDataVal.S1 = 1;
                 rDataVal.Status += "S1_ON;";
             }
             else if ((statusVal & 1 << 1) == (1 << 1))
             {
                 rDataVal.OnOff = 1;
             }
             else if ((statusVal & 1 << 2) == (1 << 2))
             {
                 rDataVal.OTP = 1;
                 rDataVal.Status += "OTP;";
             }
             else if ((statusVal & 1 << 3) == (1 << 3))
             {
                 rDataVal.FanErr = 1;
                 rDataVal.Status += "FANErr;";
             }
             else if ((statusVal & 1 << 4) == (1 << 4))
             {
                 rDataVal.PS_On = 1;
             }
             else if ((statusVal & 1 << 6) == (1 << 6))
             {
                 rDataVal.OVP = 1;
                 rDataVal.Status += "OVP;";
             }
             else if ((statusVal & 1 << 7) == (1 << 7))
             {
                 rDataVal.OPP = 1;
                 rDataVal.Status += "OPP;";
             }
             if (rDataVal.Status == "")
                 rDataVal.Status = "OK";
             System.Threading.Thread.Sleep(20); 
             //读取8/16个电流通道的数据.(电流显示精度:0.001A)
             rData = string.Empty;
             rLen = 24;
             cmd0 = "02";
             cmd1 = "03";
             wCmd = cmd0 + cmd1;
             wData = string.Empty;
             wData = CalDataFromDDCmd(wAddr, wCmd, wData);            
             if (!SendCmdToDDLoad(wData, rLen, out rData, out er))
                 return false;
             for (int i = 0; i < rData.Length / 4; i++)
             {
                 rDataVal.Cur[i] = ((double)(System.Convert.ToInt32(rData.Substring(i * 4, 4), 16))) / 1000;
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
     /// PS_ON设置
     /// </summary>
     /// <param name="wStartAddr"></param>
     /// <param name="wEndAddr"></param>
     /// <param name="wOnOff"></param>
     /// <param name="er"></param>
     /// <returns></returns>
     public bool SetPS_ON(int wStartAddr, int wEndAddr, int wOnOff, out string er)
     {
         string cmd0 = "00";
         string cmd1 = "03";
         string wCmd = cmd0 + cmd1;
         string wData = wStartAddr.ToString("X2") + wEndAddr.ToString("X2");
         wData += wOnOff.ToString("X2");
         wData = CalDataFromDDCmd(0, wCmd, wData);
         string rData = string.Empty;
         int rLen = 0;
         if (!SendCmdToDDLoad(wData, rLen, out rData, out er))
             return false;
         return true;
     }
     #endregion

     #region 协议
     /* 
       * 发送:桢头(FD)+地址+命令01+命令02+长度+数据+检验和+桢尾(FE)
       * 应答:桢头(FD)+地址+长度+数据+检验和+桢尾(FE)         
      */
     private string SOI = "FD";
     private string EOI = "FE";
     private string ROI = "FC";
     /// <summary>
     /// 发串口信息
     /// </summary>
     /// <param name="wData"></param>
     /// <param name="rLen"></param>
     /// <param name="rData"></param>
     /// <param name="er"></param>
     /// <param name="wTimeOut"></param>
     /// <returns></returns>
     private bool SendCmdToDDLoad(string wData, int rLen, out string rData, out string er, int wTimeOut = 200)
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
                 if (!DD_CheckSum(recvData, ref er))
                     return false;
                 rData = recvData.Substring(10, recvData.Length - 16);
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
     /// <summary>
     /// 检验和
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
         if ((wData.Length / 2) != (rLen + 4))
         {
             er = "数据长度错误:" + wData;
             return false;
         }
         string chkStr = wData.Substring(2, wData.Length - 8);
         string chkSum = wData.Substring(wData.Length - 6, 4);
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
         if (checkSum.Substring(0, 2) == SOI || checkSum.Substring(0, 2) == EOI)
             checkSum = ROI + checkSum.Substring(2, 2);
         if (checkSum.Substring(2, 2) == SOI || checkSum.Substring(2, 2) == EOI)
             checkSum = checkSum.Substring(0, 2) + ROI;
         return checkSum;
     }
     #endregion

    }
}
