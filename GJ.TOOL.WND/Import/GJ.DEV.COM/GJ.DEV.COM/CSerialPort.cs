using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using GJ.COM;
namespace GJ.DEV.COM
{
   /// <summary>
   /// 串口通信类
    /// 版本:V1.0.0 作者:kp.lin 日期:2017/08/10
   /// </summary>
   public class CSerialPort
   {
      #region 定义事件
      public delegate void EventOnRecvHandler(object sender, CComArgs e);
      /// <summary>
      /// 串口数据中断事件
      /// </summary>
      public event EventOnRecvHandler OnRecved;
      /// <summary>
       /// 触发事件
       /// </summary>
       /// <param name="e"></param>
      private void OnRecv(CComArgs e)
      {
           if (OnRecved != null)
              OnRecved(this,e);
      }
      #endregion

      #region 构造函数
       /// <summary>
       /// 串口通信类
       /// </summary>
       /// <param name="idNo">设备ID</param>
       /// <param name="name">设备名称</param>
       /// <param name="dataType">数据格式:16进制或ASCII</param>
       public CSerialPort(int idNo=0, string name="SerialPort", EDataType dataType = EDataType.ASCII格式)
       {
           this._idNo = idNo;
           this._name = name;
           this._comDataType = dataType; 
       }
       public override string ToString()
       {
           return _name;
       }
       #endregion

      #region 字段
       private int _idNo = 0;
       private string _name = "SerialPort";
       private bool _conStatus = false;
       private EDataType _comDataType = EDataType.ASCII格式;
       private SerialPort _rs232;      
       private ReaderWriterLock _comLock = new ReaderWriterLock();
       #endregion

      #region 属性
      /// <summary>
       /// 编号ID
       /// </summary>
      public int idNo
      {
         get{return _idNo;}
         set{_idNo = value;}
      }
      /// <summary>
       /// 名称
       /// </summary>
      public string name
      {
         get{return _name;}
         set{_name = value;}
      }
      /// <summary>
      /// 数据格式
      /// </summary>
      public EDataType comDataType
      {
         get {return _comDataType; }
         set{_comDataType = value;}         
      }
      /// <summary>
       /// 串口状态
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
      /// <param name="comName">串口名称</param>
      /// <param name="setting">9600,n,8,1</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool open(string comName,out string er,string setting="9600,N,8,1")
      {
         er = string.Empty;

         try
         {
            string[] arrayPara = setting.Split(',');
            if (arrayPara.Length != 4)
            {
               er = CLanguage.Lan("串口设置参数错误");
               return false;
            }
            int bandRate = System.Convert.ToInt32(arrayPara[0]);
            Parity parity = Parity.None;
            switch (arrayPara[1].ToUpper())
            {
               case "O":
                  parity = Parity.Odd;
                  break;
               case "E":
                  parity = Parity.Even;
                  break;
               case "M":
                  parity = Parity.Mark;
                  break;
               case "S":
                  parity = Parity.Space;
                  break;
               default:
                  break;
            }
            int dataBit = System.Convert.ToInt32(arrayPara[2]);
            StopBits stopBits = StopBits.One;
            switch (arrayPara[3])
            {
               case "0":
                  stopBits = StopBits.None;
                  break;
               case "1.5":
                  stopBits = StopBits.OnePointFive;
                  break;
               case "2":
                  stopBits = StopBits.Two;
                  break;
               default:
                  break;
            }
            if (_rs232 != null)
            {
               if (_rs232.IsOpen)
                  _rs232.Close();
               _rs232 = null;
            }
            _rs232 = new SerialPort(comName, bandRate, parity, dataBit, stopBits);
            _rs232.DataReceived += new SerialDataReceivedEventHandler(OnDataRecieve); 
            _rs232.RtsEnable = false;
            _rs232.DtrEnable = true;  
            _rs232.Open();
            _conStatus=true;
            return true;
         }
         catch (Exception e)
         {
            er = e.ToString();
            _conStatus = false;
            return false;
         }
      }
      /// <summary>
      /// 关闭串口
      /// </summary>
      /// <returns></returns>
      public bool close()
      {
         if (_rs232 != null)
         {
            if (_rs232.IsOpen)
               _rs232.Close();
            _rs232.DataReceived -= new SerialDataReceivedEventHandler(OnDataRecieve); 
            _rs232 = null;
            _conStatus = false;
         }
         return true;
      }
      /// <summary>
      /// 设置波特率
      /// </summary>
      /// <param name="setting"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool setBaud(out string er, string setting="9600,N,8,1")
      {
          er = string.Empty;

          try
          {
              if (_rs232 == null)
              {
                  er =CLanguage.Lan("串口未初始化");
                  return false;
              }
              string[] arrayPara = setting.Split(',');
              if (arrayPara.Length != 4)
              {
                  er = CLanguage.Lan("串口设置参数错误");
                  return false;
              }
              int bandRate = System.Convert.ToInt32(arrayPara[0]);
              Parity parity = Parity.None;
              switch (arrayPara[1].ToUpper())
              {
                  case "O":
                      parity = Parity.Odd;
                      break;
                  case "E":
                      parity = Parity.Even;
                      break;
                  case "M":
                      parity = Parity.Mark;
                      break;
                  case "S":
                      parity = Parity.Space;
                      break;
                  default:
                      break;
              }
              int dataBit = System.Convert.ToInt32(arrayPara[2]);
              StopBits stopBits = StopBits.One;
              switch (arrayPara[3])
              {
                  case "0":
                      stopBits = StopBits.None;
                      break;
                  case "1.5":
                      stopBits = StopBits.OnePointFive;
                      break;
                  case "2":
                      stopBits = StopBits.Two;
                      break;
                  default:
                      break;
              }
              _rs232.BaudRate = bandRate;
              _rs232.Parity = parity;
              _rs232.DataBits = dataBit;
              _rs232.StopBits = stopBits;
              return true; 
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 发送数据及接收数据
      /// </summary>
      /// <param name="wData">写入字符串</param>
      /// <param name="rLen">读取字符长度;0表示不返回数据</param>
      /// <param name="rData">返回字符串</param>
      /// <param name="er"></param>
      /// <param name="timeOut">超时时间</param>
      /// <returns></returns>
      public bool send(string wData, int rLen, out string rData, out string er, int timeOut = 500)
      {
          er = string.Empty;

          rData = string.Empty;

          try
          {
              _comLock.AcquireWriterLock(-1);

              if (_rs232 == null)
              {
                  er = CLanguage.Lan("串口未打开");
                  return false;
              }
             
              byte[] wByte = null;

              int wByteLen = 0;

              if (_comDataType ==EDataType.HEX格式)
              {
                  wByteLen = wData.Length / 2;
                  wByte = new byte[wByteLen];
                  for (int i = 0; i < wByteLen; i++)
                      wByte[i] = System.Convert.ToByte(wData.Substring(i * 2, 2), 16);
              }
              else
              {
                  wByteLen = System.Text.Encoding.UTF8.GetByteCount(wData);
                  wByte = new byte[wByteLen];
                  wByte = System.Text.Encoding.UTF8.GetBytes(wData);
              }

              _rs232.DiscardInBuffer();

              _rs232.DiscardOutBuffer();
              
              _rs232.Write(wByte, 0, wByteLen);
              
              if (rLen == 0)
                  return true;

              Stopwatch watcher = new Stopwatch();

              watcher.Start(); 

              while (true)
              {
                  System.Threading.Thread.Sleep(5);

                  if (_rs232.BytesToRead >= rLen)
                      break;

                  if (watcher.ElapsedMilliseconds > timeOut)
                      break;
              }

              watcher.Stop(); 

              if (_rs232.BytesToRead==0)
              {
                  er =CLanguage.Lan("接收数据超时");
                  return false;
              }

              int rByteLen = _rs232.BytesToRead;

              byte[] rByte = new byte[rByteLen];

              _rs232.Read(rByte, 0, rByteLen);

              if (_comDataType == EDataType.HEX格式)
              {
                  for (int i = 0; i < rByteLen; i++)
                      rData += rByte[i].ToString("X2");

              }
              else
              {
                  rData = System.Text.Encoding.UTF8.GetString(rByte);
              }
              if (rByteLen != rLen)
              {
                  er =CLanguage.Lan("接收数据长度错误") + ":" + rData;
                  return false;
              }
              return true;
          }
          catch (Exception e)
          {
              er = e.ToString();
              return false;
          }
          finally
          {
              _comLock.ReleaseWriterLock(); 
          }
      }
      /// <summary>
      /// 发送数据及接收数据
      /// </summary>
      /// <param name="wData">写入字符串</param>
      /// <param name="rEOI">等待接收到帧尾数据</param>
      /// <param name="rData">返回字符串</param>
      /// <param name="er"></param>
      /// <param name="timeOut"></param>
      /// <returns></returns>
      public bool send(string wData, string rEOI, out string rData, out string er, int timeOut = 500)
      {
          er = string.Empty;

          rData = string.Empty;

          try
          {
              _comLock.AcquireWriterLock(-1);

              if (_rs232 == null)
              {
                  er = CLanguage.Lan("串口未打开");
                  return false;
              }              

              byte[] wByte = null;

              int wByteLen = 0;

              wByteLen = System.Text.Encoding.UTF8.GetByteCount(wData);

              wByte = new byte[wByteLen];
              
              wByte = System.Text.Encoding.UTF8.GetBytes(wData);

              rData = string.Empty;

              _rs232.DiscardInBuffer();
              
              _rs232.DiscardOutBuffer();
              
              _rs232.Write(wByte, 0, wByteLen);

              if (rEOI == string.Empty)  //不接收数据
                  return true;

              int rLen = rEOI.Length;

              Stopwatch watcher = new Stopwatch();

              watcher.Start(); 

              do
              {
                  System.Threading.Thread.Sleep(2);

                  if (_rs232.BytesToRead > 0)
                  {
                      rData += _rs232.ReadExisting();

                      continue;
                  }

                  if (rData.Length > rLen)
                  {
                      if (rData.Substring(rData.Length - rLen, rLen) == rEOI)
                          break;
                  }

              } while (watcher.ElapsedMilliseconds < timeOut);

              watcher.Stop(); 

              if (rData.Length == 0 || rData.Substring(rData.Length - rLen, rLen) != rEOI)
              {
                  er = CLanguage.Lan("接收数据超时") + ":" + rData;
                  return false;
              }

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
          finally
          {
              _comLock.ReleaseWriterLock();
          }
      }
      /// <summary>
      /// 发送字节
      /// </summary>
      /// <param name="wBytes">发送字节</param>
      /// <param name="rLen">接收字节长度 0：不返回</param>
      /// <param name="rBytes">接收字节</param>
      /// <param name="er"></param>
      /// <param name="timeOut"></param>
      /// <returns></returns>
      public bool send(byte[] wBytes, int rLen, out byte[] rBytes, out string er, int timeOut = 500)
      {

          er = string.Empty;

          rBytes = null;

          try
          {
              _comLock.AcquireWriterLock(-1);

              if (_rs232 == null)
              {
                  er = CLanguage.Lan("串口未打开");
                  return false;
              }
              
              _rs232.DiscardInBuffer();
              _rs232.DiscardOutBuffer();
              _rs232.Write(wBytes, 0, wBytes.Length);

              if (rLen == 0)
                  return true;

              Stopwatch watcher = new Stopwatch();

              watcher.Start(); 

              do
              {
                  System.Threading.Thread.Sleep(2);
              } while ((_rs232.BytesToRead < rLen) && (watcher.ElapsedMilliseconds) < timeOut);

              watcher.Stop();

              if (_rs232.BytesToRead == 0)
              {
                  er = CLanguage.Lan("接收数据超时");
                  return false;
              }

              int rByteLen = _rs232.BytesToRead;

              byte[] rByte = new byte[rByteLen];
              
              _rs232.Read(rByte, 0, rByteLen);             
              
              if (rByteLen != rLen)
              {
                  er = CLanguage.Lan("接收数据长度错误") + ":" + rByteLen.ToString();
                  return false;
              }

              rBytes = new byte[rByteLen];

              for (int i = 0; i < rByteLen; i++)
              {
                  rBytes[i] = rByte[i];
              }

              return true;
          }
          catch (Exception e)
          {
              er = e.ToString();
              return false;
          }
          finally
          {
              _comLock.ReleaseWriterLock();
          }
      }
      #endregion

      #region 串口中断接收
      private void OnDataRecieve(object sender, SerialDataReceivedEventArgs e)
      { 
         OnRecv(new CComArgs(_idNo,e.ToString())); 
      }
      #endregion

   }
}
