using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;
using GJ.DEV.COM;

namespace GJ.DEV.Switch
{
    public class CRS485_I2C
    {

      #region 类定义
        /// <summary>
        /// 模块信息
        /// </summary>
        public class CDataVal
        {
            /// <summary>
            /// 复制
            /// </summary>
            /// <returns></returns>
            public CDataVal Clone()
            {
                CDataVal module = new CDataVal();

                module.ACV = this.ACV;

                module.ACI = this.ACI;

                module.DCV = this.DCV;

                module.DCI = this.DCI;

                module.Status = this.Status;

                return module;
            }
            /// <summary>
            /// 输入电压
            /// </summary>
            public double ACV = 0;
            /// <summary>
            /// 输入电流
            /// </summary>
            public double ACI = 0;
            /// <summary>
            /// 输出电压
            /// </summary>
            public double DCV = 0;
            /// <summary>
            /// 模块输出电流
            /// </summary>
            public double DCI = 0;
            /// <summary>
            /// 模块告警量
            /// </summary>
            public string Status = string.Empty;
        }
       #endregion

      #region 枚举
        /// <summary>
      /// 选配监控
      /// </summary>
      public enum EMonitor
      {
          输出电压状态,
          输出电流状态,
          输入状态,
          温度状态,
          温度1,
          温度2,
          温度3,
          温度4,
          温度5,
          输出功率,
          输入功率,
          风扇1速度,
          风扇2速度
      }
      #endregion

      #region 构造函数
      public CRS485_I2C(int idNo = 0, string name = "RS485_I2C")
      {
          _idNo = idNo;
          
          _name = name;

      }
      public override string ToString()
      {
          return _name ;
      }
      #endregion

      #region 字段
      private int _idNo = 0;
      private string _name = "RS485_I2C";
      private bool _conStatus=false;
      private CSerialPort com = null;
      /// <summary>
      /// 选择监控参数数量
      /// </summary>
      private int _MonitorParaNum = 13;
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
      /// <summary>
      /// 选择监控参数数
      /// </summary>
      public int MonitorParaNum
      {
          get { return _MonitorParaNum; }
          set { _MonitorParaNum = value; }
                           
      }
      #endregion

      #region ModBus通信协议
      /// <summary>
      /// 打开串口
      /// </summary>
      /// <param name="comName">115200,n,8,1</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Open(string comName, out string er,string setting="57600,n,8,1")
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
             string wCmd = devAddr.ToString("X2"); //设备地址

             wCmd += "03";                        //寄存器功能码为03
             
             wCmd += regAddr.ToString("X4");   //开始地址

             wCmd += N.ToString("X2");         //读地址长度

             wCmd += CCRC.Crc16(wCmd);         //CRC16 低位前,高位后   

             int rLen = N * 2;
       
             if (!com.send(wCmd, 5 + rLen, out rData, out er,_TimeOut))
                 return false;

             if (!checkCRC(rData))
             {
                 er = CLanguage.Lan("crc16检验和错误") + ":" + rData;
                 return false;
             }

             string temp = rData.Substring(6, rLen * 2);  //2个字节为寄存器值，高在前,低位在后，寄存器小排最前面；

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
              
              rVal =System.Convert.ToInt16(rData.Substring(rData.Length - 4, 4), 16);

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
                 rVal[i] =System.Convert.ToInt16(rData.Substring(rData.Length - (i + 1) * 4, 4), 16);

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

              int wLen = N * 2;          //写入字节数

              string wData = wVal.ToString("X" + wLen * 2);

              int rLen = 4;           //回读长度

              string wCmd = string.Empty;

              wCmd = devAddr.ToString("X2");        //设备地址
              wCmd += "10";                         //寄存器功能码为16
              wCmd += regAddr.ToString("X4");       //开始地址
              wCmd += N.ToString("X4");             //地址长度
              wCmd += wLen.ToString("X2");          //写入字节数  
              wCmd += wData;                        //写入数据
              wCmd += CCRC.Crc16(wCmd);             //CRC16 低位前,高位后 
  
              string rData = string.Empty;

              if (!com.send(wCmd, rLen, out rData, out er,_TimeOut))
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
              string wCmd = string.Empty;

              int N = wVal.Length;   //单写多个值   
           
              int rLen = 8;

              int wLen = N * 2;

              string wData = string.Empty;

              for (int i = 0; i < N; i++)
                  wData += wVal[i].ToString("X4");

              wCmd = devAddr.ToString("X2");   //设备地址
              wCmd += "10";                    //寄存器功能码为16
              wCmd += regAddr.ToString("X4");  //开始地址
              wCmd += N.ToString("X4");        //读地址长度
              wCmd += wLen.ToString("X2");     //写入字节数  
              wCmd += wData;                   //写入数据
              wCmd += CCRC.Crc16(wCmd);        //CRC16 低位前,高位后   

              string rData = string.Empty;

              if (!com.send(wCmd, rLen, out rData, out er, _TimeOut))
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
      /// 读地址0x8000
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadAddr(out int curAddr, out string er)
      {
          curAddr = 0;

          er = string.Empty;
          
          try
          {
              string rData=string.Empty;

              if (!Read(1, 0x8000, 1, out rData, out er))
                  return false;

              curAddr =System.Convert.ToInt16(rData,16);  

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false; 
          }
      }
      /// <summary>
      /// 读版本0x8001
      /// </summary>
      /// <param name="rVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadVersion(int curAddr, out string version, out string er)
      {
          version = string.Empty;

          er = string.Empty;

          try
          {
              string rData = string.Empty;

              if (!Read(curAddr, 0x8001, 1, out rData, out er))
                  return false;

              version = ((double)System.Convert.ToInt16(rData, 16) / 10).ToString("0.0");

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读设备名称0x8800
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="name"></param>
      /// <returns></returns>
      public bool ReadName(int addr, out string name, out string er)
      {
          name = string.Empty;

          er = string.Empty;

          try
          {
              int[] child = new int[32];

              if (!Read(addr, 0x8800, ref child, out er))
                  return false;

              StringBuilder stringBuilder = new StringBuilder();

              for (int i = 0; i < child.Length; i++)
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
      /// 设置地址0x8002
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetNewAddr(int curAddr, out string er)
      {
          try
          {
              return Write(1, 0x8002, curAddr, out er);
          }
          catch (Exception ex)
          {
              er=ex.ToString();
              return false; 
          }
      }
      /// <summary>
      /// 读取模块信息
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="module"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadModuleData(int curAddr, out CDataVal module, out string er)
      {
          module = new CDataVal();

          er = string.Empty;

          try
          {
              string[] ErrCode = new string[] { 
                                                "异常报警", "通信存储或逻辑", "温度报警", "输入欠压",
                                                "输出过流", "输出过压", "模块无输出", "模块忙碌",
                                                "未知错误", "其他错误", "风扇警告", "功率偏低",
                                                "制造商信息错误", "输入电流错误", "输出电流错误", "输出错误"
                                              };

              string status = string.Empty;

              string rData = string.Empty;

              if (!Read(curAddr, 0x800D, 5, out rData, out er))
                  return false;

              int rLen = rData.Length / 4;

              string[] rDataList = new string[rLen];

              for (int i = 0; i < rLen; i++)
              {
                  rDataList[rLen - i - 1] = rData.Substring(i * 4, 4); 
              }

              //状态

              if (rDataList[0] == "FFFF")
              {
                  module.Status = "I2C通信异常";
                  return true;
              }

              int rVal = System.Convert.ToInt32(rDataList[0], 16);

              for (int i = 0; i < 16; i++)
              {
                  int bit = (rVal & (1 << i));

                  if (bit != 0)
                  {
                      module.Status += ErrCode[i] + ";";
                  }
              }

              if (module.Status == string.Empty)
                  module.Status = "正常";

              //输入电压
              if (rDataList[1] == "FFFF")
              {
                  module.ACV = 0;
              }
              else
              {
                  rVal = System.Convert.ToInt32(rDataList[1], 16);
                  module.ACV = calLinear11(rVal);
              }

              //输入电流
              if (rDataList[1] == "FFFF")
              {
                  module.ACI = 0;
              }
              else
              {
                  rVal = System.Convert.ToInt32(rDataList[2], 16);
                  module.ACI = calLinear11(rVal);
              }

              //输出电压
              if (rDataList[1] == "FFFF")
              {
                  module.DCV = 0;
              }
              else
              {
                  rVal = System.Convert.ToInt32(rDataList[3], 16);
                  module.DCV =  calLinear11(rVal);
              }
              //输出电流
              if (rDataList[1] == "FFFF")
              {
                  module.DCI = 0;
              }
              else
              {
                  rVal = System.Convert.ToInt32(rDataList[4], 16);
                  module.DCI = calLinear11(rVal);
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
      /// 读取运行状态0x800D
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="status">正常</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadStatus(int curAddr, out string status, out string er)
      {
          status = string.Empty;

          er = string.Empty;

          try
          {
              string[] ErrCode = new string[] { 
                                                "异常报警", "通信存储或逻辑", "温度报警", "输入欠压",
                                                "输出过流", "输出过压", "模块无输出", "模块忙碌",
                                                "未知错误", "其他错误", "风扇警告", "功率偏低",
                                                "制造商信息错误", "输入电流错误", "输出电流错误", "输出错误"
                                              };

              string rData = string.Empty;

              if (!Read(curAddr, 0x800D, 1, out rData, out er))
                  return false;

              if (rData == "FFFF")
              {
                  status = "I2C通信异常";
                  return true;
              }

              int rVal = System.Convert.ToInt16(rData, 16);

              for (int i = 0; i < 16; i++)
              {
                  int bit = (rVal & (1 << i));

                  if (bit != 0)
                  {
                      status += ErrCode[i] + ";";
                  }
              }

              if (status == string.Empty)
                  status = "正常";

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取输入电压0x800E
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="acv"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadInputVolt(int curAddr, out double acv, out string er)
      {
          acv = 0;

          er = string.Empty;

          try
          {

              string rData = string.Empty;

              if (!Read(curAddr, 0x800E, 1, out rData, out er))
                  return false;

              int rVal = System.Convert.ToInt16(rData, 16);

              acv = calLinear11(rVal);

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取输入电流0x800F
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="aci"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadInputCurrent(int curAddr, out double aci, out string er)
      {
          aci = 0;

          er = string.Empty;

          try
          {

              string rData = string.Empty;

              if (!Read(curAddr, 0x800F, 1, out rData, out er))
                  return false;

              int rVal = System.Convert.ToInt16(rData, 16);

              aci = calLinear11(rVal);

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取输出电压0x8010
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="dcv"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadOutputVolt(int curAddr, out double dcv, out string er)
      {
          dcv = 0;

          er = string.Empty;

          try
          {

              string rData = string.Empty;

              if (!Read(curAddr, 0x8010, 1, out rData, out er))
                  return false;

              int rVal = System.Convert.ToInt16(rData, 16);

              dcv = calLinear11(rVal);

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取输入电流0x8011
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="temp"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadOutputCurrent(int curAddr, out double dci, out string er)
      {
          dci = 0;

          er = string.Empty;

          try
          {

              string rData = string.Empty;

              if (!Read(curAddr, 0x8011, 1, out rData, out er))
                  return false;

              int rVal = System.Convert.ToInt16(rData, 16);

              dci = calLinear11(rVal);

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 设置开关0x8008
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetONOFF(int curAddr, int wOnOff, out string er)
      {
          try
          {
              return Write(curAddr, 0x8008, wOnOff, out er);
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 设置选配监控参数0x8050
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="er"></param>
      /// <param name="para"></param>
      /// <returns></returns>
      public bool SetMonitorPara(int curAddr,out string er, params EMonitor[] para)
      {
          er = string.Empty;

          try
          {
              int Val = 0;

              for (int i = 0; i < para.Length; i++)
              {
                  Val += (1 << (int)para[i]);
              }

              return Write(curAddr, 0x8050, Val, out er);
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取选配参数
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="rVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadMonitorValue(int curAddr, out int[] rVal, out string er)
      {
          er = string.Empty;

          rVal = new int[_MonitorParaNum];

          try
          {
              if (!Read(curAddr, 0x8051, ref rVal, out er))
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
      /// 设置I2C地址-->可发广播命令
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="I2CAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetI2CAddr(int curAddr, int I2CAddr, out string er)
      {
          try
          {
              return Write(curAddr, 0x8002, I2CAddr, out er);
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      #endregion

      #region 计算方法
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
      /// <summary>
      /// 这个格式数据为用 16 位数据表示小数，高 5 位为指数位，低 11 位为小数位
      /// 计算公式为：值 = 2 ^ 高5位 * 低 11 位。
      ///其中高5位和低11位都是有符号数
      /// 当最高位为 0 时:2 ^ 高5位 = 2 ^ (16位数据 >> 11)，
      ///当最高位为 1 时:2 ^ 高5位 = 1 / 2 ^ (0x20 – (16位数据 >> 11))
      /// </summary>
      /// <param name="rVal"></param>
      /// <returns></returns>
      private double calLinear11(int rVal)
      {
          try
          {
              int high5 = 0;
              
              double d = 0;

              double low11 = 0;

              high5 = rVal >> 11;

              low11 = rVal & 0x7FF;

               //可以不考虑低 11 位为负数的情况，因为电压电流不会出现负数。
              if (low11 >= 0x400)
              {
                  low11 = -(0x800 - low11);
              }

              if ((rVal & 0x8000) == 0) 
              {
                 d = (1 << high5) * low11;
              } 
              else
              {
                d = low11 / (1 << (0x20 - high5));
               }

               return d;
          }
          catch (Exception)
          {
              return 0;
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
