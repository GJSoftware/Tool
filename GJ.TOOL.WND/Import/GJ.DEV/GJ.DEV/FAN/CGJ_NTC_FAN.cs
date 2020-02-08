using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;

namespace GJ.DEV.FAN
{
    /// <summary>
    /// 风扇控制板V1.0.0
    /// </summary>
    public class CGJ_NTC_FAN
    {
      #region 构造函数
      public CGJ_NTC_FAN(int idNo = 0, string name = "GJ_NTC_FAN")
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
      private string _name = "GJ_NTC_FAN";
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
      /// <param name="comName">9600,n,8,1</param>
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
      /// 关闭串口
      /// </summary>
      public void Close()
      {
          try
          {
              if (com == null)
                  return;
              com.close();
              com = null;
              _conStatus = false;
          }
          catch (Exception)
          {

          }
      }
      /// <summary>
      /// 设置地址
      /// </summary>
      /// <param name="address"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetAddress(int address, out string er)
      {
          er = string.Empty;

          try
          {

              if (address < 1 || address > 240)
              {
                  er = "地址超出范围(1~240),当前输入地址" + address + " ";
                  return false;
              }

              string Addr = "00";

              string Cmd12 = "00" + "00";

              string Data = address.ToString("X2");

              string Cmd = CalDataFromMon32Cmd(Addr, Cmd12, Data);

              int rLen = 5 + 1;

              string rData = string.Empty;

              if (!SendCmdToMon32(Cmd, rLen, out rData, out er))
                  return false;

              int val = System.Convert.ToInt16(rData, 16);

              if (val != 1)
              {
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
      /// 读取版本号
     /// </summary>
     /// <param name="addr"></param>
     /// <param name="ver"></param>
     /// <param name="er"></param>
     /// <returns></returns>
      public bool ReadVersion(int addr, out int ver, out string er)
      {
          ver = 0;

          er = string.Empty;

          try
          {
              string Addr = addr.ToString("X2");

              string Cmd12 = "02" + "04";

              string Data = string.Empty;

              string Cmd = CalDataFromMon32Cmd(Addr, Cmd12, Data);

              int rLen = 5 + 1;

              string rData = string.Empty;

              if (!SendCmdToMon32(Cmd, rLen, out rData, out er))
                  return false;

              ver = System.Convert.ToInt16(rData,16);

              return true;

          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取温度值
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="meanTemp">底层程序有+55度
      /// DS_0的温度
      /// DS_1的温度
      /// NTC_0的温度
      /// NTC_1的温度
      /// NTC_2的温度
      /// NTC_3的温度
      /// NTC_4的温度
      /// NTC_5的温度
      /// NTC_6的温度
      /// NTC_7的温度
      /// </param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadTempValue(int addr,  out List<int> meanTemp, out string er)
      {

          meanTemp = new List<int>();

          try
          {

              string Addr = addr.ToString("X2");

              string Cmd12 = "02" + "01";

              string Data = string.Empty;

              string Cmd = CalDataFromMon32Cmd(Addr, Cmd12, Data);

              int rLen = 5 + 32;

              string rData = string.Empty;

              if (!SendCmdToMon32(Cmd, rLen, out rData, out er))
                  return false;

              for (int i = 1; i < 11; i++)
              {
                  string s = rData.Substring(i * 2, 2);

                  meanTemp.Add(System.Convert.ToInt16(s, 16) - 55);
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
      /// 设定温控点
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="tempPoint">
      /// 风扇100%占空比的温度点设定。（默认>45度）
      /// 风扇90%占空比的温度点设定。（默认>42度）
      /// 风扇80%占空比的温度点设定。（默认>39度）
      /// 风扇70%占空比的温度点设定。（默认>36度）
      /// 风扇60%占空比的温度点设定。（默认>33度）
      /// 风扇50%占空比的温度点设定。（默认>30度）
      /// 风扇40%占空比的温度点设定。（默认>27度）
      /// 风扇30%占空比的温度点设定。（默认>24度）
      /// 风扇20%占空比的温度点设定。（默认>21度）
      /// 风扇10%占空比的温度点设定。（默认>18度）
      /// </param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetTempControlPoint(int addr, List<int> tempPoint, out string er)
      {
          try
          {

              if (tempPoint.Count != 10)
              {
                  er = "温度控制点数量应为10,当前数量:" + tempPoint.Count;
                  return false;
              }

              string Addr = addr.ToString("X2");

              string Cmd12 = "01" + "01";

              string Data = string.Empty;

              Data += "02"; //00:不擦除 01：擦除旧数据 02：写新数据 

              for (int i = 0; i < tempPoint.Count; i++)
              {
                  Data += tempPoint[i].ToString("X2");
              }

              string Cmd = CalDataFromMon32Cmd(Addr, Cmd12, Data);

              int rLen = 5 + 1;

              string rData = string.Empty;

              if (addr == 0)              
                  rLen = 0;

              if (!SendCmdToMon32(Cmd, rLen, out rData, out er))
                  return false;

              if (addr != 0)
              {
                  int val = System.Convert.ToInt16(rData, 16);

                  if (val != 1)
                  {
                      return false;
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
      /// <summary>
      /// 读取温度控制点
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="temp"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadTempCtrlPoint(int addr, out List<int> temp, out string er)
      {
          temp = new List<int>();
          er = string.Empty;

          try
          {

              string Addr = addr.ToString("X2");

              string Cmd12 = "02" + "02";

              string Data = string.Empty;

              string Cmd = CalDataFromMon32Cmd(Addr, Cmd12, Data);

              int rLen = 5 + 10;

              string rData = string.Empty;

              if (!SendCmdToMon32(Cmd, rLen, out rData, out er))
                  return false;

              for (int i = 0; i < 10; i++)
              {
                  string s = rData.Substring(i * 2, 2);

                  temp.Add(System.Convert.ToInt16(s, 16));
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
        private string SOI = "FE";
        private string EOI = "FF";
        /// <summary>
        /// 发串口信息
        /// </summary>
        /// <param name="wData"></param>
        /// <param name="rLen"></param>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <param name="wTimeOut"></param>
        /// <returns></returns>
        private bool SendCmdToMon32(string wData, int rLen, out string rData, out string er, int wTimeOut = 200)
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
                    if (!CheckSum(recvData, ref er))
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
        /// <summary>
        /// 数据格式化
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="wCmd"></param>
        /// <param name="wData"></param>
        /// <returns></returns>
        private string CalDataFromMon32Cmd(string wAddr, string wCmd, string wData)
        {
            string cmd = string.Empty;
            int len = 4 + wData.Length / 2;
            string chkData = string.Empty;
            for (int i = 0; i < wData.Length / 2; i++)
            {
                if (wData.Substring(i * 2, 2) == SOI || wData.Substring(i * 2, 2) == EOI)
                {
                    if (wData.Substring(i * 2, 2) == SOI)
                        chkData += (Convert.ToInt16(SOI, 16) - 2).ToString("X2");
                    if (wData.Substring(i * 2, 2) == EOI)
                        chkData += (Convert.ToInt16(EOI, 16) - 2).ToString("X2");
                }
                else
                    chkData += wData.Substring(i * 2, 2);
            }
            cmd = wAddr + wCmd + len.ToString("X2") + chkData;
            cmd = SOI + cmd + CalCheckSum(cmd) + EOI;
            return cmd;
        }
        /// <summary>
        /// 检验和
        /// </summary>
        /// <param name="wData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool CheckSum(string wData, ref string er)
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
            string calSum = CalCheckSum(chkStr);
            //if (chkSum != calSum)
            //{
            //    er = "数据CheckSum错误:" + wData;
            //    return false;
            //}
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
            //0   1   2   3   4   5   6   7
            //0XFE    ADR Command1    Command2    Data_Long   Data    ChkSum  0XFF
            //FE 00 00 00 05 01 06 FF
            sum = sum %256;
            string checkSum = sum.ToString("X2");
            if (checkSum == SOI || checkSum == EOI)
                checkSum = "FC";
            return checkSum;

        }
        #endregion

    }
}
