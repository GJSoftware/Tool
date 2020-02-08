using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;

namespace GJ.DEV.COM
{
    public class CTelnet
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
       public CTelnet(int idNo = 0, string name = "Telnet", EDataType dataType = EDataType.ASCII格式)
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
       private string _name = "Telnet";
       private bool _conStatus = false;
       private EDataType _comDataType = EDataType.ASCII格式;
       /// <summary>
       /// TCP客户端
       /// </summary>
       private TcpClient _client = null;   
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

      #region Telnet协议
      private enum Verbs
      {
          WILL = 251,
          WONT = 252,
          DO = 253,
          DONT = 254,
          IAC = 255
      }
      private enum Options
      {
          SGA = 3
      } 
      private void ParseTelnet(StringBuilder sb)
      {
          while (_client.Available > 0)
          {
              int input = _client.GetStream().ReadByte();
              switch (input)
              {
                  case -1:
                      break;
                  case (int)Verbs.IAC:
                      // interpret as command
                      int inputverb = _client.GetStream().ReadByte();
                      if (inputverb == -1) break;
                      switch (inputverb)
                      {
                          case (int)Verbs.IAC:
                              //literal IAC = 255 escaped, so append char 255 to string
                              sb.Append(inputverb);
                              break;
                          case (int)Verbs.DO:
                          case (int)Verbs.DONT:
                          case (int)Verbs.WILL:
                          case (int)Verbs.WONT:
                              // reply to all commands with "WONT", unless it is SGA (suppres go ahead) 
                              int inputoption = _client.GetStream().ReadByte();
                              if (inputoption == -1) break;
                              _client.GetStream().WriteByte((byte)Verbs.IAC);
                              if (inputoption == (int)Options.SGA)
                                  _client.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WILL : (byte)Verbs.DO);
                              else
                                  _client.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WONT : (byte)Verbs.DONT);
                              _client.GetStream().WriteByte((byte)inputoption);
                              break;
                          default:
                              break;
                      }
                      break;
                  default:
                      sb.Append((char)input);
                      break;
              }
          }
      }
      #endregion

      #region 方法
      /// <summary>
      /// 打开串口
      /// </summary>
      /// <param name="comName">IP地址</param>
      /// <param name="setting">端口</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool open(string comName,out string er,string setting="10001")
      {
          er = string.Empty;

          try
          {
              if (_client == null)
              {
                  int port = System.Convert.ToInt32(setting);

                  _client = new TcpClient();

                  _client.Connect(comName, port);
              }

              _conStatus = true;

              return true;
          }
          catch (Exception ex)
          {
              _conStatus = false;
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 关闭串口
      /// </summary>
      /// <returns></returns>
      public bool close()
      {
          if (_client != null)
          {
              _client.Close();
              _client = null;
              _conStatus = false;
          }

          return true;
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

              if (_client == null)
              {
                  er = CLanguage.Lan("未连接服务端");
                  return false;
              }

              Stopwatch wather = new Stopwatch();

              wather.Start();

              byte[] wByte = null;

              int wByteLen = 0;

              wData = wData.Replace("\0xFF", "\0xFF\0xFF");

              if (_comDataType == EDataType.HEX格式)
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

              _client.GetStream().Write(wByte, 0, wByte.Length);

              System.Threading.Thread.Sleep(20);

              StringBuilder sb = new StringBuilder();

              while (true)
              {
                  System.Threading.Thread.Sleep(5);

                  if (_client.Available > 0)
                  {
                      ParseTelnet(sb);

                      if (rLen != 0 && sb.Length < timeOut)
                          continue;
                      
                      break;
                  }

                  if (wather.ElapsedMilliseconds > timeOut)
                      break;
              }

              rData = sb.ToString();

              if (rLen > 0 && rData.Length != rLen)
              {
                  er = "接收数据超时:" + rData;
                  return false;
              }

              wather.Stop();

              er = "耗时:" + wather.ElapsedMilliseconds.ToString() + "ms";

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

              if (_client == null)
              {
                  er = CLanguage.Lan("未连接服务端");
                  return false;
              }

              Stopwatch wather = new Stopwatch();

              wather.Start();

              byte[] wByte = null;

              int wByteLen = 0;

              wData = wData.Replace("\0xFF", "\0xFF\0xFF");

              if (_comDataType == EDataType.HEX格式)
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

              _client.GetStream().Write(wByte, 0, wByte.Length);

              StringBuilder sb = new StringBuilder();

              while (true)
              {
                  System.Threading.Thread.Sleep(20);

                  if (_client.Available > 0)
                  {
                      ParseTelnet(sb);

                      int index = sb.ToString().IndexOf(rEOI);

                      if (index < 0)
                          continue;

                      break;
                  }

                  if (wather.ElapsedMilliseconds > timeOut)
                      break;
              }

              rData = sb.ToString();

              if ((rData.Length == 0) || (rData.Length< rEOI.Length) || (rData.Substring(rData.Length - rEOI.Length, rEOI.Length) != rEOI))
              {
                  er = "接收数据超时";
                  return false;
              }

              wather.Stop();

              er = "耗时:" + wather.ElapsedMilliseconds.ToString() + "ms";

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

              if (_client == null)
              {
                  er = CLanguage.Lan("未连接服务端");
                  return false;
              }

              Stopwatch wather = new Stopwatch();

              wather.Start();

              _client.GetStream().Write(wBytes, 0, wBytes.Length);

              System.Threading.Thread.Sleep(20);

              StringBuilder sb = new StringBuilder();

              while (true)
              {
                  System.Threading.Thread.Sleep(5);

                  if (_client.Available > 0)
                  {
                      ParseTelnet(sb);

                      if (rLen != 0 && sb.Length < timeOut)
                          continue;

                      break;
                  }

                  if (wather.ElapsedMilliseconds > timeOut)
                      break;
              }

              string rData = sb.ToString();

              if (rLen > 0 && rData.Length != rLen)
              {
                  er = "接收数据超时:" + rData;
                  return false;
              }

              rBytes = System.Text.Encoding.UTF8.GetBytes(rData); 

              wather.Stop();

              er = "耗时:" + wather.ElapsedMilliseconds.ToString() + "ms";

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
      #endregion


    }
}
