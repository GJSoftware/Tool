using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;
using GJ.DEV.COM;

namespace GJ.DEV.FCMB
{
    public class CFMB_V1:IFCMB
    {
      #region 构造函数
      public CFMB_V1(int idNo = 0, string name = "CFMB_V1")
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
      private string _name = "CFMB_V1";
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
      /// <param name="comName">57600,n,8,1</param>
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
      /// 读线圈
      /// 从机地址(1Byte)+功能码(1Byte)+寄存器地址(2Byte)+地址数量(2Byte)+CRC检验(2Byte)
      /// </summary>
      /// <param name="devType">地址类型</param>
      /// <param name="regAddr">开始地址</param>
      /// <param name="N">地址长度</param>
      /// <param name="rData">16进制字符:数据值高位在前,低位在后</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Read(int devAddr, ERegType regType, int regAddr, int N, out string rData, out string er)
      {
          rData = string.Empty;

          er = string.Empty;

         try
         {
             string wCmd = devAddr.ToString("X2");
             int rLen = 0;

             if (regType != ERegType.D)
             {
                 wCmd += "01";          //线圈功能码为01
                 rLen = (N + 7) / 8;    //8个线圈为1Byte:前8个线圈存第1字节,最小线圈存最低位
             }
             else
             {
                 wCmd += "03";      //寄存器功能码为03
                 rLen = N * 2;
             }

             wCmd += regAddr.ToString("X4");  //开始地址

             wCmd += N.ToString("X2");        //读地址长度---1Byte

             wCmd += CCRC.Crc16(wCmd);                  //CRC16 低位前,高位后     
       
             if (!com.send(wCmd, 5 + rLen, out rData, out er))
                 return false;

             if (!checkCRC(rData))
             {
                 er = CLanguage.Lan("crc16检验和错误") + ":" + rData;
                 return false;
             }
             string temp = rData.Substring(6, rLen * 2);

             if (regType != ERegType.D)
             {
                 //转化线圈数据为 Mn,Mn-1..M1,M0(高位在前,低位在后)
                 string coil_HL = string.Empty;
                 int coilLen = temp.Length / 2;
                 for (int i = 0; i < coilLen; i++)
                 {
                     coil_HL += temp.Substring(temp.Length - (i + 1) * 2, 2);
                 }
                 rData = coil_HL;
             }
             else
             {
                 rData = temp;     //2个字节为寄存器值，高在前,低位在后，寄存器小排最前面；
                 //转换为寄存器小排最后
                 rData = string.Empty;
                 for (int i = 0; i < temp.Length / 4; i++)
                 {
                     rData = temp.Substring(i * 4, 4) + rData;
                 }
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
      public bool Read(int devAddr, ERegType regType, int regAddr, out int rVal, out string er)
      {
          rVal = -1;

          er = string.Empty;

          try
          {
              int N = 1;

              string rData = string.Empty;

              if (!Read(devAddr,regType, regAddr, N, out rData, out er))
                  return false;
              
              if ((regType != ERegType.D))
              {
                  for (int i = 0; i < rData.Length / 2; i++)
                  {
                      int valByte = System.Convert.ToInt16(rData.Substring(rData.Length - (i + 1) * 2, 2), 16);
                      if ((valByte & (1 << 0)) == (1 << 0))
                          rVal = 1;
                      else
                          rVal = 0;
                  }
              }
              else
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
      public bool Read(int devAddr, ERegType regType, int regAddr, ref int[] rVal, out string er)
      {
          er = string.Empty;

          try
          {
              string rData = string.Empty;

              int N = rVal.Length;

              if (!Read(devAddr,regType,regAddr, N, out rData, out er))
                  return false;

              if (regType != ERegType.D)
              {
                  for (int i = 0; i < rData.Length / 2; i++)
                  {
                      int valByte = System.Convert.ToInt16(rData.Substring(rData.Length - (i + 1) * 2, 2), 16);
                      for (int j = 0; j < 8; j++)
                      {
                          if ((j + 8 * i) < N)
                          {
                              if ((valByte & (1 << j)) == (1 << j))
                                  rVal[j + i * 8] = 1;
                              else
                                  rVal[j + i * 8] = 0;
                          }
                      }
                  }
              }
              else
              {
                  for (int i = 0; i < N; i++)
                      rVal[i] = System.Convert.ToInt16(rData.Substring(rData.Length - (i + 1) * 4, 4), 16);

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
      /// 单写线圈和寄存器值
      /// 从机地址(1Byte)+功能码(1Byte)+寄存器地址(2Byte)+地址数量(2Byte)+字节数(1Byte)+数据+CRC检验(2Byte)
      /// </summary>
      /// <param name="devType">地址类型</param>
      /// <param name="regAddr">开始地址</param>
      /// <param name="wVal">单个值</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Write(int devAddr, ERegType regType, int regAddr, int wVal, out string er)
      {
          er = string.Empty;

          try
          {
              int N = 1;   //单写1个值
              string wCmd = devAddr.ToString("X2");
              int rLen = 0;
              int wLen = 0;
              string wData = string.Empty;
              if (regType != ERegType.D)
              {
                  wCmd += "0F";          //线圈功能码为15
                  wLen = (7 + N) / 8;    //写入字节数
                  rLen = 8;              //回读长度
                  wData = wVal.ToString("X" + wLen * 2);
              }
              else
              {
                  wCmd += "10";        //寄存器功能码为16
                  wLen = N * 2;          //写入字节数
                  rLen = 8;           //回读长度
                  wData = wVal.ToString("X" + wLen * 2);
              }
              wCmd += regAddr.ToString("X4");  //开始地址
              wCmd += N.ToString("X4");                  //读地址长度
              wCmd += wLen.ToString("X2");               //写入字节数  
              wCmd += wData;                             //写入数据
              wCmd += CCRC.Crc16(wCmd);                //CRC16 低位前,高位后   
              
              string rData = string.Empty;
              
              rLen = 4;

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
      public bool Write(int devAddr, ERegType regType, int regAddr, int[] wVal, out string er)
      {
          er = string.Empty;

          try
          {
              int N = wVal.Length;   //单写多个值
              string wCmd = devAddr.ToString("X2");
              int rLen = 0;
              int wLen = 0;
              string wData = string.Empty;
              if (regType != ERegType.D)
              {
                  wCmd += "0F";          //线圈功能码为15
                  wLen = (7 + N) / 8;    //写入字节数
                  rLen = 8;              //回读长度
                  for (int i = 0; i < wLen; i++)
                  {
                      int val = 0;
                      for (int j = 0; j < 8; j++)
                      {
                          if (i * 8 + j < N)
                          {
                              int bit = (wVal[i * 8 + j] & 0x1) << j;
                              val += bit;
                          }
                      }
                      wData += val.ToString("X2");
                  }
              }
              else
              {
                  wCmd += "10";        //寄存器功能码为16
                  wLen = N * 2;          //写入字节数
                  rLen = 8;           //回读长度
                  for (int i = 0; i < N; i++)
                  {
                      wData += wVal[i].ToString("X4");
                  }

              }
              wCmd += regAddr.ToString("X4");  //开始地址
              wCmd += N.ToString("X4");                  //读地址长度
              wCmd += wLen.ToString("X2");               //写入字节数  
              wCmd += wData;                             //写入数据
              wCmd += CCRC.Crc16(wCmd);                //CRC16 低位前,高位后   
              rLen = 4;
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
      /// 读设备名称
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

              if (!Read(addr, ERegType.D, 0x8800, ref child, out er))
                  return false;

              StringBuilder stringBuilder = new StringBuilder();

              for (int i = 0; i < child.Length; i++)
              {
                  if(child[i]!=0)
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
      /// 读设备序列号
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="sn"></param>
      /// <returns></returns>
      public bool ReadSn(int addr, out string sn, out string er)
      {
          sn = string.Empty;

          er = string.Empty;

          try
          {
              int[] child = new int[31];

              if (!Read(addr, ERegType.D, 0x8820, ref child, out er))
                  return false;

              StringBuilder stringBuilder = new StringBuilder();

              for (int i = 0; i < child.Length; i++)
              {
                  if (child[i] != 0)
                      stringBuilder.AppendFormat("\\u{0}", child[i].ToString("X4"));
              }

              sn = UnicodeToString(stringBuilder.ToString()); 

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }      
      }
      /// <summary>
      /// 读地址D8000
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

              if (!Read(1, ERegType.D, 0x8000, 1, out rData, out er))
                  return false;

              curAddr = System.Convert.ToInt16(rData,16);  

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false; 
          }
      }
      /// <summary>
      /// 设置地址D8000
      /// </summary>
      /// <param name="curAddr"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetAddr(int curAddr, out string er)
      {
          try
          {
              return Write(0, ERegType.D, 0x8000, curAddr, out er);
          }
          catch (Exception ex)
          {
              er=ex.ToString();
              return false; 
          }
      }
      /// <summary>
      /// 读取版本号D8001
      /// </summary>
      /// <param name="rVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadVersion(int addr, out int rVal, out string er)
      {
          rVal = 0;

          er = string.Empty;

          try
          {
              string rData = string.Empty;

              if (!Read(addr, ERegType.D, 0x8001, 1, out rData, out er))
                  return false;

              rVal = System.Convert.ToInt16(rData, 16);

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取版本号D8001
      /// </summary>
      /// <param name="rVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadVersion(int addr, out string rVal, out string er)
      {
          rVal = string.Empty;

          er = string.Empty;

          try
          {
              string rData = string.Empty;

              if (!Read(addr, ERegType.D, 0x8001, 1, out rData, out er))
                  return false;
              
              rVal =  ((double)System.Convert.ToInt16(rData, 16)/10).ToString("0.0") ;

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读波特率D8002
      /// </summary>
      /// <param name="baud"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadBaud(int addr, out int baud, out string er)
      {
          baud =- 1;

          er = string.Empty;

          try
          {
              string rData = string.Empty;
              if (!Read(addr, ERegType.D, 0x8002, 1, out rData, out er))
                  return false;
              baud = System.Convert.ToInt16(rData,16);
              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 设置波特率D8002
      /// </summary>
      /// <param name="baud"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetBaud(int addr, int baud, out string er)
      {
          try
          {
              return Write(addr, ERegType.D, 0x8002, baud, out er);
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 设置小板地址D8003
      /// </summary>
      /// <param name="addr">要求中位拨码开关为 on</param>
      /// <param name="childAddr">小板的开关需要为 on</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetChildAddr(int childAddr, out string er)
      {
          er = string.Empty;

          try
          {
              if (!Write(0, ERegType.D, 0x8003, childAddr, out er))
              {
                  string[] valBytes = er.Split(':');

                  if (valBytes.Length < 2)
                      return false;

                  if (valBytes[1].Length == 14)
                      return true;

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
      /// 获取小板版本号D8100
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="childVer"></param>
      /// <param name="er"></param>
      /// <returns>当小板通信不上时值为 0，正常大于 0</returns>
      public bool ReadChildVersion(int addr,int childMax, out List<int> childVer, out string er)
      {
          childVer = new List<int>(); 

          er = string.Empty;

          try
          {
              int[] child = new int[childMax];

              if (childMax == 32)
              {
                  if (!Read(addr, ERegType.D, 0x8030, ref child, out er))
                      return false;
              }
              else
              {
                  if (!Read(addr, ERegType.D, 0x8100, ref child, out er))
                      return false;
              }

              for (int i = 0; i < child.Length; i++)
                  childVer.Add(child[i]);  

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }      
      }
      /// <summary>
      /// 快充模式配置D8007--D8009
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="qcm">8007</param>
      /// <param name="qcv">8008</param>
      /// <param name="qci">8009</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetQCM(int addr, EQCM qcm, double qcv,double qci, out string er,bool cc2 = false)
      {
          try
          {
              int qcmVal = (int)qcm;

              if (cc2)
              {
                  qcmVal += 0x600;
              }

              if (qcm == EQCM.MTK1_0 || qcm == EQCM.MTK2_0)
                  qcmVal = 0;

              int qcvVal = (int)(qcv*1000);

              int qciVal = (int)(qci * 1000);

              int[] wVal = new int[] { qcmVal, qcvVal, qciVal };

              return Write(addr,ERegType.D, 0x8007,wVal,out er);
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取快充模式
     /// </summary>
     /// <param name="addr"></param>
      /// <param name="qcm">0x8007</param>
      /// <param name="qcv">0x8008</param>
      /// <param name="qci">0x8009</param>
     /// <param name="er"></param>
     /// <returns></returns>
      public bool ReadQCM(int addr, out EQCM qcm, out double qcv, out double qci, out string er)
      {
          qcm = EQCM.Normal;

          qcv = 0;

          qci = 0;

          er = string.Empty;

          try
          {
              int[] rVal = new int[3]; 

              if (!Read(addr, ERegType.D, 0x8007,ref rVal, out er))
                  return false;

              qcm = (EQCM)(rVal[0]%0x100);

              qcv = ((double)rVal[1]) / 1000;

              qci = ((double)rVal[2]) / 1000;

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 时序控制模式D8070
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="mode"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetOnOffMode(int addr, EOnOffMode mode, out string er)
      {
          try
          {
              return Write(addr, ERegType.D, 0x8070, (int)mode, out er);
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取时序控制模式D8070
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="mode"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadOnOffMode(int addr, out EOnOffMode mode, out string er)
      {
          mode=EOnOffMode.上位机控制;

          er = string.Empty;

          try
          {
              string rData = string.Empty;

              if (!Read(addr, ERegType.D, 0x8070, 1, out rData, out er))
                  return false;

              mode = (EOnOffMode)System.Convert.ToInt16(rData, 16);

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 时序总时间(Min)-D8071
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="runMin"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetTotalTime(int addr, int runMin, out string er)
      {
          try
          {
              return Write(addr, ERegType.D, 0x8071, runMin, out er);
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取总时间(Min)
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="runMin"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadTotalTime(int addr, out int runMin, out string er)
      {
          runMin = 0;

          er = string.Empty;

          try
          {
              string rData = string.Empty;

              if (!Read(addr, ERegType.D, 0x8071, 1, out rData, out er))
                  return false;

              runMin = System.Convert.ToInt16(rData, 16);

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 设置时序段D8050
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="OnOff">
      /// 每 3 个数据为一组，分别为 on 时间、off 时间和重复次数
      /// 总共 10 组，时间单位为秒</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetOnOffTime(int addr, List<COnOff> OnOff, out string er)
      {
          er = string.Empty;

          try
          {
              int count=0;

              int[] wOnOff = new int[30];

              for (int i = 0; i < OnOff.Count; i++)
              {
                  wOnOff[count] = OnOff[i].OnOffTime;
                  
                  wOnOff[count+1] = OnOff[i].OnTime;
                  
                  wOnOff[count+2] = OnOff[i].OffTime;

                  count = count + 3;
              }

              if (!Write(addr, ERegType.D, 0x8050,wOnOff, out er))
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
      /// 读取时序段D8050
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="OnOff"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadOnOffTime(int addr, out List<COnOff> OnOff, out string er)
      {
          OnOff = new List<COnOff>();

          er = string.Empty;

          try
          {
              int[] child = new int[30];

              if (!Read(addr, ERegType.D, 0x8050, ref child, out er))
                  return false;

              for (int i = 0; i < child.Length/3; i++)
              {
                  COnOff rOnOff = new COnOff();

                  rOnOff.OnOffTime = child[i * 3];

                  rOnOff.OnTime = child[i * 3 + 1];

                  rOnOff.OffTime = child[i * 3 + 2];

                  OnOff.Add(rOnOff); 
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
      /// 直接操作时序开关D806F
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="wOnOff"></param>
      /// <param name="er"></param>
      /// <param name="synC">同步AC信号</param>
      /// <returns></returns>
      public bool SetACON(int addr, int wOnOff, out string er,bool synC = false)
      {
          try
          {

              int onoff = (wOnOff == 1 ? 0x4:0x8);

              if (synC)
              {
                  onoff += 0x100;
              }

              return Write(addr, ERegType.D, 0x817A, onoff, out er);
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// AC控制:B400->苹果项目专用
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="wOnOff"></param>
      /// <param name="er"></param>
      /// <param name="synC"></param>
      /// <param name="B400"></param>
      /// <returns></returns>
      public bool SetACON(int addr, int wOnOff, out string er, bool synC = false, bool B400 = false)
      {
          try
          {

              int onoff = (wOnOff == 1 ? 0x4 : 0x8);

              if (synC)
              {
                  onoff += 0x100;
              }

              if (B400)
              {
                  onoff += 0x400;
              }

              return Write(addr, ERegType.D, 0x817A, onoff, out er);
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取时序开关D806F
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="rOnOff">
      /// 0：控制开
      /// 1：控制关
      /// 3：读取状态
      /// </param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadACON(int addr, out int rOnOff, out string er)
      {
          rOnOff = 0;

          er = string.Empty;

          try
          {
              string rData = string.Empty;

              if (!Read(addr, ERegType.D, 0x806F, 1, out rData, out er))
                  return false;

              rOnOff = System.Convert.ToInt16(rData, 16);

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 设置IO-D817A
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="ioType"></param>
      /// <param name="wOnOff"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetIO(int addr, EFMB_wIO ioType, int wOnOff, out string er)
      {
          er = string.Empty;

          try
          {
              int val = 0;

              if (ioType == EFMB_wIO.错误信号灯)
              {
                  if (wOnOff == 1)
                      val = 0x1;
                  else
                      val = 0x2;

                  if (!Write(addr, ERegType.D, 0x817A, val, out er))
                      return false;
              }
              else if (ioType == EFMB_wIO.继电器信号)
              {
                  if (wOnOff == 1)
                      val = 0x4;
                  else
                      val = 0x8;

                  if (!Write(addr, ERegType.D, 0x817A, val, out er))
                      return false;
              }
              else if (ioType == EFMB_wIO.气缸控制1)
              {
                  if (wOnOff == 1)
                      val = 0x10;
                  else
                      val = 0x20;

                  if (!Write(addr, ERegType.D, 0x817A, val, out er))
                      return false;
              }
              else if (ioType == EFMB_wIO.气缸控制2)
              {
                  if (wOnOff == 1)
                      val = 0x40;
                  else
                      val = 0x80;

                  if (!Write(addr, ERegType.D, 0x817A, val, out er))
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
      /// 读取IO信号D8179
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="io"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadIO(int addr, out List<int> io, out string er)
      {
          io = new List<int>();

          er = string.Empty;

          try
          {
              int io_Val = 0;

              if (!Read(addr, ERegType.D, 0x8179, out io_Val, out er))
                return false;

              for (int i = 0; i < 16; i++)
              {
                  int bit = (io_Val & (1 << i));

                  if (bit == 0)
                      io.Add(0);
                  else
                      io.Add(1); 
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
      /// 读取产品电压D8128
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="volt">5000 表示5V</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadVolt(int addr, out List<double> volt, out string er, bool sync = true,EVMODE voltMode=EVMODE.VOLT_40)
      {
          volt = new List<double>();

          er = string.Empty;

          try
          {
              if (voltMode == EVMODE.VOLT_40)
              {
                  int[] child = new int[CFMBPara.Child_Max];

                  if (!sync)
                  {
                      if (!Read(addr, ERegType.D, 0x8128, ref child, out er))  
                          return false;
                  }
                  else
                  {
                      if (!Read(addr, ERegType.D, 0x8150, ref child, out er))
                          return false;
                  }

                  for (int i = 0; i < child.Length; i++)
                      volt.Add(((double)child[i] / 1000));
              }
              else
              {
                  for (int i = 0; i < CFMBPara.Child_Max; i++)
                      volt.Add(0);

                  int[] child = new int[32];

                  if (!Read(addr, ERegType.D, 0x8090, ref child, out er))
                      return false;

                  for (int i = 0; i < child.Length; i++)
                      volt[i]=(double)child[i] / 1000;

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
      /// 读取AC电压
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="acv">单位 0.1V，2200 表示 220.0V</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadAC(int addr, out double acv, out string er)
      {
          acv = -1;

          er = string.Empty;

          try
          {
              string rData = string.Empty;
              
              if (!Read(addr, ERegType.D, 0x8178, 1, out rData, out er))
                  return false;
              
              int rVal = System.Convert.ToInt16(rData, 16);

              acv = ((double)rVal / 10); 

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取测试D+、D-短路情况
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="chan">1-32</param>
      /// <param name="value">0:OK-->大于零:自测 D+对GND、D-对GND、D+对D-、D-对V+</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadDGND(int addr, int chan,out string status, out string er)
      {
          status = "异常";

          er = string.Empty;

          try
          {
              string rData = string.Empty;

              if (!Read(addr, ERegType.D, 0x81E0 + chan -1, 1, out rData, out er))
                  return false;

              int value = System.Convert.ToInt16(rData, 16);

              if (rData == "8000")
              {
                  return false;
              }
             
              if(value == 0)
              {
                  status = "正常";
                  return true;
              }

              string[] C_D_GND = new string[] { "D+V-", "D-V-", "D+D-", "D-V+" };

              status = string.Empty;

              for (int i = 0; i < 4; i++)
              {
                  int bit = (value & (1 << i));

                  if (bit != 0)
                  {
                      if (status != string.Empty)
                          status += ";";

                      status = C_D_GND[i];
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
      /// 读取测试D+、D-短路情况
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="chan">1-32</param>
      /// <param name="value">0:OK-->大于零:自测 D+对GND、D-对GND、D+对D-、D-对V+</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadDGND(int addr, int chan, out List<int> status, out string er)
      {
          status = new List<int>();

          er = string.Empty;

          try
          {
              string rData = string.Empty;

              if (!Read(addr, ERegType.D, 0x81E0 + chan - 1, 1, out rData, out er))
                  return false;

              int value = System.Convert.ToInt16(rData, 16);

              if (rData == "8000")
              {
                  return false;
              }

              for (int i = 0; i < 4; i++)
              {
                  int bit = (value & (1 << i));

                  if (bit != 0)
                  {
                      status.Add(1);
                  }
                  else
                  {
                      status.Add(0);
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
      /// 设置Y0-Y7->D817A
      /// </summary>
      /// <param name="addr"></param>
      /// <param name="Y"></param>
      /// <param name="wOnOff"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetIO(int addr, EFMB_Y Y, int wOnOff, out string er)
      {
          er = string.Empty;

          try
          {
              int val = 0;

              int Y_Val = (int)Y;

              if (wOnOff == 1)
              {
                  val = (1 << Y_Val * 2);
              }
              else
              {
                  val = (1 << (Y_Val * 2 + 1));
              }

              if (!Write(addr, ERegType.D, 0x817A, val, out er))
                  return false;

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
         string crc = CCRC.Crc16(wCmd.Substring(0, wCmd.Length - 4));
         if (crc != wCmd.Substring(wCmd.Length - 4, 4))
            return false;
         return true;
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
