using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using GJ.COM;

namespace GJ.DEV.Telnet
{
    public class CTelnet
    {
        #region 构造函数
        public CTelnet(int idNo=0, string name="Telnet")
        {
            this._idNo = idNo;
            this._name = name;
        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 字段
        enum Verbs
        {
            WILL = 251,
            WONT = 252,
            DO = 253,
            DONT = 254,
            IAC = 255
        }
        enum Options
        {
            SGA = 3
        } 
        /// <summary>
        /// 编号
        /// </summary>
        private int _idNo = 0;
        /// <summary>
        /// 名称
        /// </summary>
        private string _name = string.Empty;
        /// <summary>
        /// TCP客户端
        /// </summary>
        private TcpClient _client = null;
        #endregion

        #region 方法
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Connect(string ip, int port,out string er)
        {
            er = string.Empty;

            try
            {
                if (_client == null)
                {
                    _client = new TcpClient();

                    _client.Connect(ip, port);
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;                
            }
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        public void Close()
        {
            try
            {
                if (_client != null)
                {
                    _client.Close();
                    _client = null;
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="recvData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadBufferData(out string recvData, out string er, int timeOut = 2000)
        {
            er = string.Empty;

            recvData = string.Empty;

            try
            {
                if (_client == null)
                {
                    er = CLanguage.Lan("未连接服务端");
                    return false;
                }

                Stopwatch wather = new Stopwatch();

                wather.Start();

                StringBuilder sb = new StringBuilder();

                while (true)
                {
                    if (_client.Available > 0)
                    {
                        ParseTelnet(sb);
                        break;
                    }
                    if (wather.ElapsedMilliseconds > timeOut)
                        break;
                    System.Threading.Thread.Sleep(5);
                }

                wather.Stop();

                recvData = sb.ToString();

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="recvData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SendCmd(string msg, out string recvData, out string er,int timeOut = 2000)
        {
            er = string.Empty;

            recvData = string.Empty;

            try
            {
                if (_client == null)
                {
                    er = CLanguage.Lan("未连接服务端");
                    return false;
                }

                Stopwatch wather = new Stopwatch();

                wather.Start();

                byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(msg.Replace("\0xFF", "\0xFF\0xFF"));

                _client.GetStream().Write(buf,0,buf.Length);

                System.Threading.Thread.Sleep(20);

                StringBuilder sb = new StringBuilder();

                while (true)
                {
                   System.Threading.Thread.Sleep(5);

                   if(_client.Available > 0)
                   {
                      ParseTelnet(sb);
                      break;
                   }

                   if (wather.ElapsedMilliseconds > timeOut)
                       break;
                }

                recvData = sb.ToString();
               
                wather.Stop();

                er = "耗时:" + wather.ElapsedMilliseconds.ToString() + "ms";

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        void ParseTelnet(StringBuilder sb)
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

     
    }
}
