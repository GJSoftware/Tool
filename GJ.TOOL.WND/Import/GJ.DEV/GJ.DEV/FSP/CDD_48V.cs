using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;

namespace GJ.DEV.FSP
{
    public class CDD_48V
    {

      #region 构造函数
        public CDD_48V(int idNo = 0, string name = "DC12V整流模块")
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
      private string _name = "DC12V整流模块";
      private bool _conStatus=false;
      private CSerialPort com = null;
      private int _TimeOut = 100;
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
      /// 读取模块信息
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="module"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadModuleData(int addr, out CModule module, out string er)
      {
          module = new CModule();

          er = string.Empty;

          try
          {
              string wCmd = string.Empty;

              string CID = "03";

              string INFO = string.Empty;

              wCmd = FormatRequestCmd(addr, CID, INFO);

              string rVal = string.Empty;

              string rData = string.Empty;

              int rLen = 6 + 12;

              if (!SendCmdToCOM(wCmd, rLen, out rData, out er,_TimeOut))
                  return false;

              if (!CalReponseCmd(CID, rData, out rVal, out er))
                  return false;

              //输出电压
              module.Volt = (double)System.Convert.ToInt32(rVal.Substring(0, 4)) / 100; 
              //输出电流
              module.Current = (double)System.Convert.ToInt32(rVal.Substring(4, 4)) / 100;
              //内部温度
              module.Temp = (double)System.Convert.ToInt32(rVal.Substring(8, 2));
              //风扇1转速
              module.FanSpeed1 = System.Convert.ToInt32(rVal.Substring(10, 4));
              //风扇2转速
              module.FanSpeed2 = System.Convert.ToInt32(rVal.Substring(14, 4));
              //模块告警量
              int status = System.Convert.ToInt32(rVal.Substring(18, 2));
              //模块保护类型
              int protect = System.Convert.ToInt32(rVal.Substring(20, 2));

              module.Status = string.Empty;

              if ((status & (1 << 0)) != 0) //0:限流标志
              {
                  module.Status += "限流告警";
              }
              if ((status & (1 << 2)) != 0) //2:模块开关机
              {
                  module.Status += "模块关机";
              }
              if ((status & (1 << 4)) != 0) //模块风扇故障
              {
                  module.Status += "模块风扇故障";
              }
              if ((status & (1 << 5)) != 0) //交流故障
              {
                  module.Status += "交流故障";
              }
              if ((status & (1 << 6)) != 0) //模块保护
              {
                  module.Status += "模块保护";
              }

              module.Alarm = string.Empty;

              if (protect == 1)
                  module.Alarm = "短路保护";
              else if(protect ==2)
                  module.Alarm = "过温保护";
              else if (protect == 3)
                  module.Alarm = "输出过压保护";
              else if (protect == 4)
                  module.Alarm = "输入过压保护";
              else if (protect == 5)
                  module.Alarm = "输入欠压保护";
              else if (protect == 6)
                  module.Alarm = "AC掉电";

              if (module.Status == string.Empty)
                  module.Status = "正常";

              if (module.Alarm == string.Empty)
                  module.Alarm = "正常";

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 控制开关机->0:开机 1:关机
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="wOnOff"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool PowerOff(int addr,int wOnOff,out string er)
      {
          er = string.Empty;

          try
          {
              string wCmd = string.Empty;

              string CID = "04";

              string INFO = wOnOff.ToString("X2") + "00";

              wCmd = FormatRequestCmd(addr, CID, INFO);

              string rVal = string.Empty;

              string rData = string.Empty;

              int rLen = 6 + 2;

              if (!SendCmdToCOM(wCmd, rLen, out rData, out er, _TimeOut))
                  return false;

              if (!CalReponseCmd(CID, rData, out rVal, out er))
                  return false;

              int rOnOff = System.Convert.ToInt16(rVal.Substring(0,2),16);

              if (wOnOff != rOnOff)
              {
                  er = "返回开关机状态错误:" + rData;
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
      /// 设置模块输出电压电流
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="volt"></param>
      /// <param name="current"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetModuelPara(int addr, double volt, double current, out string er)
      {
          er = string.Empty;

          try
          {
              string wCmd = string.Empty;

              string CID = "06";

              string INFO = ((int)(volt*100)).ToString("X4") + ((int)(current*100)).ToString("X4");

              wCmd = FormatRequestCmd(addr, CID, INFO);

              string rVal = string.Empty;

              string rData = string.Empty;

              int rLen = 0;

              if (!SendCmdToCOM(wCmd, rLen, out rData, out er, _TimeOut))
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
      /// 获取模块电压电流设置参数
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="volt"></param>
      /// <param name="current"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadModuelPara(int addr, out double volt, out double current, out string er)
      {
          er = string.Empty;

          volt = 0;

          current = 0;

          try
          {
              string wCmd = string.Empty;

              string CID = "07";

              string INFO = string.Empty;

              wCmd = FormatRequestCmd(addr, CID, INFO);

              string rVal = string.Empty;

              string rData = string.Empty;

              int rLen = 6 + 5;

              if (!SendCmdToCOM(wCmd, rLen, out rData, out er, _TimeOut))
                  return false;

              if (!CalReponseCmd(CID, rData, out rVal, out er))
                  return false;

              //输出电压
              volt = (double)System.Convert.ToInt32(rVal.Substring(0, 4)) / 100;

              //输出电流
              current = (double)System.Convert.ToInt32(rVal.Substring(4, 4)) / 100;

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      #endregion

      #region 协议: 帧头(3Bytes) + 设备地址(1Byte) + 长度(1Byte) + 命令(1Byte) + 信息(N Bytes) + 校验和(1Byte)
      /*
        帧头:0xC0,0xC0,0xC0
        设备地址:0x00、0x01、0x02……（0x99为广播帧，只作用于调压、开关机命令）
        长度= (CID + INFO)长度
        校验和=ADDR + LENGTH + CID +INFO
      */
      private const string Head = "C0C0C0";
      /// <summary>
      /// 发串口数据并接收数据
      /// </summary>
      /// <param name="wCmd"></param>
      /// <param name="rLen"></param>
      /// <param name="rData"></param>
      /// <param name="er"></param>
      /// <param name="wTimeOut"></param>
      /// <returns></returns>
      private bool SendCmdToCOM(string wCmd, int rLen, out string rData, out string er, int wTimeOut = 200)
      {
          rData = string.Empty;

          er = string.Empty;

          try
          {
              string recvData = string.Empty;

              if (!com.send(wCmd, rLen, out recvData, out er, wTimeOut))
                  return false;

              rData = recvData;

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 格式输入命令
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="CID"></param>
      /// <param name="INFO"></param>
      /// <returns></returns>
      private string FormatRequestCmd(int addr, string CID, string INFO)
      {
          string wCmd = string.Empty;

          try
          {
              int Len = (CID + INFO).Length / 2;

              wCmd += addr.ToString("X2");

              wCmd += Len.ToString("X2");

              wCmd += CID;

              wCmd += INFO;

              int sum = 0;

              for (int i = 0; i < wCmd.Length; i++)
              {
                  sum += System.Convert.ToInt32(wCmd.Substring(i * 2, 2),16);
              }

              string CheckSum = (sum % 256).ToString("X2");

              return wCmd;
          }
          catch (Exception)
          {
              return wCmd;
          }
      }
      /// <summary>
      /// 格式化输出数据
      /// </summary>
      /// <param name="CID"></param>
      /// <param name="rData"></param>
      /// <param name="rVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      private bool CalReponseCmd(string CID, string rData, out string rVal, out string er)
      {
          rVal = string.Empty;

          er = string.Empty;

          try
          {
              if (rData.Length == 0)
              {
                  er = "模块通信超时";
                  return false;
              }

              if (rData.Length < 7)
              {
                  er = "接收数据长度错误:" + rData;
                  return false;
              }

              if (rData.Substring(0, 6) != Head)
              {
                  er = "帧头[C0C0C0]错误:" + rData;
                  return false;
              }

              int rLen = System.Convert.ToInt16(rData.Substring(8,2),16) - 1; //INFO长度

              int N = rLen + 7;

              if (rData.Length != N * 2)
              {
                  er = "接收数据长度["+ N.ToString() +"]错误:" + rData;
                  return false;
              }

              string rCID = rData.Substring(10, 2);

              string CalCID = (System.Convert.ToInt16(CID, 16) + 0x80).ToString("X2");

              if (CalCID != rCID)
              {
                  er = "应答命令[" + CalCID + "]错误:" + rData;
                  return false;
              }

              string rCheckSum = rData.Substring(rData.Length - 2, 2);

              string rCmd = rData.Substring(6, rData.Length - 8);

              int sum = 0;

              for (int i = 0; i < rCmd.Length; i++)
              {
                  sum += System.Convert.ToInt32(rCmd.Substring(i * 2, 2), 16);
              }

              string CheckSum = (sum % 256).ToString("X2");

              if (rCheckSum != CheckSum)
              {
                  er = "校验和[" + CheckSum + "]错误:" + rData;
                  return false;
              }

              rVal = rData.Substring(12, rLen * 2);

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
