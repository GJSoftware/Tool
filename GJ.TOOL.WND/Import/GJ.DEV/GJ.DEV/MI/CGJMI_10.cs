using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;

namespace GJ.DEV.MI
{
    public class CGJMI_10
    {
      #region 构造函数
      public CGJMI_10(int idNo = 0, string name = "GJMI_10")
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
      private string _name = "GJMI_10";
      private bool _conStatus = false;
      private CSerialPort com = null;
      private int _TimeOut = 100;
      private int _chanMax = 10;
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
      /// 最大通道数
      /// </summary>
      public int chanMax
      {
          get { return _chanMax; }
      }
      #endregion

      #region 方法
      /// <summary>
      /// 打开串口
      /// </summary>
      /// <param name="comName"></param>
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
            string Cmd = "01";

            string Info = wAddr.ToString("X4");

            string rData = string.Empty;

            string rVal = string.Empty;

            string wCmd = CalSendCmdFormat(0, Cmd, Info);

            int rLen = 6 + 2 * 1; 

            if (!com.send(wCmd, rLen, out rData, out er, _TimeOut))
               return false;

            if (!CalRtnData(rData, out rVal,out er))
               return false;
            
            return true;
         }
         catch (Exception e)
         {
            er = e.ToString();
            return false;
         }
      }
      /// <summary>
      /// 读版本
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="version"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadVersion(int wAddr, out string version, out string er)
      {
          er = string.Empty;

          version = string.Empty;

          try
          {
              string Cmd = "06";

              string Info = string.Empty;

              string rData = string.Empty;

              string rVal = string.Empty;

              string wCmd = CalSendCmdFormat(wAddr, Cmd, Info);

              int rLen = 6 + 2 * 1;

              if (!com.send(wCmd, rLen, out rData, out er, _TimeOut))
                  return false;

              if (!CalRtnData(rData, out rVal, out er))
                  return false;

              int rValData = System.Convert.ToInt16(rVal, 16);

              version = (System.Convert.ToDouble(rValData) / 1000).ToString();

              return true;
          }
          catch (Exception e)
          {
              er = e.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读设备名称
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="name"></param>
      /// <returns></returns>
      public bool ReadName(int wAddr, out string name, out string er)
      {
          name = string.Empty;

          er = string.Empty;

          try
          {
              string Cmd = "07";

              string Info = string.Empty;

              string rData = string.Empty;

              string rVal = string.Empty;

              string wCmd = CalSendCmdFormat(wAddr, Cmd, Info);

              int rLen = 6 + 2 * 24;

              if (!com.send(wCmd, rLen, out rData, out er, _TimeOut))
                  return false;

              if (!CalRtnData(rData, out rVal, out er))
                  return false;

              List<int> child = new List<int>();

              for (int i = 0; i < rVal.Length/2; i++)
              {
                  child.Add(System.Convert.ToInt16(rVal.Substring(i * 2, 2),16)); 
              }

              StringBuilder stringBuilder = new StringBuilder();

              for (int i = 0; i < child.Count; i++)
              {
                  if (child[i] != 0)
                      stringBuilder.AppendFormat("\\u{0}", child[i].ToString("X4"));
              }

              name = UnicodeToString(stringBuilder.ToString());

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取10个通道:电流+电压
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="acv"></param>
      /// <param name="aci"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadVoltAndCurrent(int wAddr, out List<double> acv, out List<double> aci, out string er)
      {
          acv = new List<double>();

          aci = new List<double>();

          er = string.Empty;

          try
          {
              string Cmd = "11";

              string Info = string.Empty;

              string rData = string.Empty;

              string rVal = string.Empty;

              string wCmd = CalSendCmdFormat(wAddr, Cmd, Info);

              int rLen = 6 + 2 * (_chanMax + 1);

              if (!com.send(wCmd, rLen, out rData, out er, _TimeOut))
                  return false;

              if (!CalRtnData(rData, out rVal, out er))
                  return false;

              for (int i = 0; i < _chanMax; i++)
              {
                  string strACI = rVal.Substring(i * 4, 4);

                  int ai = System.Convert.ToInt32(strACI, 16);

                  aci.Add(System.Convert.ToDouble(ai) / 1000);

              }

              for (int i = 0; i < _chanMax; i++)
              {
                  string strACV = rVal.Substring(rVal.Length-4, 4);

                  int ac = System.Convert.ToInt32(strACV, 16);

                  acv.Add((double)ac / 10);
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
      /// 读取有效功率
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="pwr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadPower(int wAddr, out List<double> pwr,out string er)
      {
          pwr = new List<double>();

          er = string.Empty;

          try
          {
              string Cmd = "13";

              string Info = string.Empty;

              string rData = string.Empty;

              string rVal = string.Empty;

              string wCmd = CalSendCmdFormat(wAddr, Cmd, Info);

              int rLen = 6 + 2 * _chanMax;

              if (!com.send(wCmd, rLen, out rData, out er, _TimeOut))
                  return false;

              if (!CalRtnData(rData, out rVal, out er))
                  return false;

              for (int i = 0; i < _chanMax; i++)
              {
                  string strPwr = rVal.Substring(i * 4, 4);

                  int val = System.Convert.ToInt32(strPwr,16);

                  pwr.Add((Double)val / 10); 
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
    
      #region 10通道电流板协议
      /*
       协议:SOI(0xEE)+ADDR(1-127)+LENGHT(1Byte)+CMD/RTN(1Bytes)+INFO(Word)+CHKSUM(1Byte)+EOI(0xEF)
       * CHKSUM =(0xff00 - (SOI,EOI,CHKSUM)除外数值) & 0xFF
      */
      private const string SOI = "EE";
      private const string EOI = "EF";
      /// <summary>
      /// 格式化命令
      /// </summary>
      /// <param name="Addr"></param>
      /// <param name="Cmd"></param>
      /// <param name="Info"></param>
      /// <returns></returns>
      private string CalSendCmdFormat(int Addr, string Cmd, string Info)
      {
          try
          {
              string strCmd = Addr.ToString("X2");

              int len = Info.Length / 4;

              strCmd += len.ToString("X2");

              strCmd += Cmd;

              strCmd += Info;

              strCmd += CalCheckSum(strCmd);

              strCmd = SOI + strCmd + EOI;

              return strCmd;
          }
          catch (Exception)
          {
              return "";
          }
      }
      /// <summary>
      /// 获取有效数据段
      /// </summary>
      /// <param name="wCmd"></param>
      /// <param name="rVal">有效数据</param>
      /// <returns></returns>
      private bool CalRtnData(string rData, out string rVal, out string er)
      {
          er = string.Empty;

          rVal = string.Empty;

          try
          {
              if (rData.Length < 2 * 6)
              {
                  er = "数据长度错误:" + rData;
                  return false;
              }

              if (rData.Substring(0, 2) != SOI)
              {
                  er = "数据帧头错误:" + rData;
                  return false;
              }

              if (rData.Substring(rData.Length - 2, 2) != EOI)
              {
                  er = "数据帧尾错误:" + rData;
                  return false;
              }

              int len = System.Convert.ToInt16(rData.Substring(4, 2),16);

              //if (rData.Length != 2 * 6 + len * 4)
              //{
              //    er = "INFO长度错误:" + rData;
              //    return false;
              //}

              if (!CalRtnCheckSum(rData))
              {
                  er = "校验和错误:" + rData;
                  return false;
              }

              string Rtn = rData.Substring(6, 2);

              if (Rtn == "F0")
              {
                  rVal = rData.Substring(8, rData.Length - 12);

                  return true;
              }
             
              if (Rtn == "F1")
              {
                  er = "[RTN]CHKSUM错误";
              }
              else if (Rtn == "F2")
              {
                  er = "[RTN]无效数据";
              }
              else if (Rtn == "F3")
              {
                  er = "[RTN]CMD无效";
              }
              else if (Rtn == "F4")
              {
                  er = "[RTN]长度错误";
              }
              else
              {
                  er = "[RTN]其它错误";
              }
              return false;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 计算检验和
      /// </summary>
      /// <param name="wCmd"></param>
      /// <returns></returns>
      private string CalCheckSum(string strCmd)
      {
         int sum = 0;

         for (int i = 0; i < strCmd.Length / 2; i++)
         {
             sum += System.Convert.ToInt32(strCmd.Substring(i * 2, 2), 16);
         }

         sum = (0xFF00 - sum) & 0xFF;

         return sum.ToString("X2");
      }
      /// <summary>
      /// 计算返回数据校验和
      /// </summary>
      /// <param name="rData"></param>
      /// <returns></returns>
      private bool CalRtnCheckSum(string rData)
      {
          try
          {
              string strCmd = rData.Substring(2, rData.Length - 4);

              int sum = 0;

              for (int i = 0; i < strCmd.Length / 2; i++)
              {
                  sum += System.Convert.ToInt32(strCmd.Substring(i * 2, 2), 16);
              }

              if (sum % 0x100 != 0)
                  return false;

              return true;
          }
          catch (Exception)
          {
              return false;
          }
      }
      #endregion

      #region 中文转换
      /// <summary>  
      /// Unicode字符串转为正常字符串  
      /// </summary>  
      /// <param name="srcText"></param>  
      /// <returns></returns>  
      public string UnicodeToString(string srcText)
      {
          try
          {
              string dst = "";
              string src = srcText;
              int len = srcText.Length / 6;
              for (int i = 0; i <= len - 1; i++)
              {
                  string str = "";
                  str = src.Substring(0, 6).Substring(2);
                  src = src.Substring(6);
                  byte[] bytes = new byte[2];
                  bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                  bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                  dst += Encoding.Unicode.GetString(bytes);
              }
              return dst;
          }
          catch (Exception)
          {
              return "";
          }

      }
      #endregion
    }
}
