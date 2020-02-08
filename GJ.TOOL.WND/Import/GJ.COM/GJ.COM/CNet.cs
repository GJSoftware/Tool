using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Xml;

namespace GJ.COM
{
    public class CNet
    {
        /// <summary>
        /// 获取本机名称
        /// </summary>
        /// <returns></returns>
        public static string HostName()
        {
            return Dns.GetHostName(); 
        }
        /// <summary>
        /// 获取192.168段本地IP
        /// </summary>
        /// <returns></returns>
        public static string HostIP()
        {
            try
            {
                IPAddress[] addrList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

                for (int i = 0; i < addrList.Length; i++)
                {
                    byte[] addrByte = addrList[i].GetAddressBytes();

                    if (addrByte[0] == 192 && addrByte[1] == 168)
                    {
                        return addrList[i].ToString();
                    }
                }
                return "127.0.0.1";
            }
            catch (Exception)
            {
                return "127.0.0.1";
            }       
        }
        /// <summary>
        /// Ping远程电脑IP
        /// </summary>
        /// <param name="remoteHost"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool PingRemotePC(string remoteHost, out string er)
        {
            er = string.Empty;

            try
            {
                if (remoteHost == "127.0.0.1" || remoteHost.ToLower() == "localhost")
                    return true;
                bool pingFlag = false;
                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = @"ping -n 1 " + remoteHost;
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (proc.HasExited == false)
                {
                    proc.WaitForExit(500);
                }
                string pingResult = proc.StandardOutput.ReadToEnd();

                if ( pingResult.IndexOf("无法访问目标主机")==-1 && pingResult.IndexOf("(0% 丢失)") != -1)
                    pingFlag = true;
                else
                    er = "连接[" + remoteHost + "]超时:" + pingResult;
                proc.StandardOutput.Close();
                return pingFlag;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 登录远程电脑
        /// </summary>
        /// <param name="remoteHost">电脑名称或IP</param>
        /// <param name="userName">登录用户</param>
        /// <param name="passWord">登录密码</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool ConnectRemotePC(string remoteHost, string userName, string passWord, out string er)
        {
            er = string.Empty;

            try
            {
                if (remoteHost == "127.0.0.1" || remoteHost.ToLower() == "localhost")
                    return true;
                bool connFlag = false;
                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = @"net use \\" + remoteHost + " " + passWord + " " + " /user:" + userName + " >NUL";
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (proc.HasExited == false)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                if (errormsg != "")
                    er = "登录系统:" + errormsg + "[" + dosLine + "]";
                else
                    connFlag = true;
                proc.StandardError.Close();
                return connFlag;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 登录远程目录
        /// </summary>
        /// <param name="remoteFloder">远程目录</param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool ConnectRemoteFolder(string remoteFloder, string userName, string passWord, out string er)
        {
            er = string.Empty;

            try
            {
                bool connFlag = false;
                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = @"net use " + remoteFloder + " \"" + passWord + "\" /user:\"" + userName + " >NUL";
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (proc.HasExited == false)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                while (proc.HasExited == false)
                {
                    proc.WaitForExit(1000);
                }
                if (errormsg != "")
                    er = "登录目录:" + errormsg + "[" + dosLine + "]";
                else
                    connFlag = true;
                proc.StandardError.Close();
                return connFlag;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 向HTTP发消息和接收消息
        /// </summary>
        /// <param name="requestXml"></param>
        /// <param name="reponseXml"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool PostMessageToHttp(string ulrWeb, string requestXml, out string reponseXml, out string er)
        {
            reponseXml = string.Empty;

            er = string.Empty;

            try
            {
                if (ulrWeb == string.Empty)
                {
                    er = "Web接口地址不能为空";
                    return false;
                }

                Encoding gb2312 = Encoding.GetEncoding("GB2312");

                //发送数据转换为Bytes
                byte[] sendByte = gb2312.GetBytes(requestXml);

                //发送HTTP的POST请求
                HttpWebRequest request = (HttpWebRequest)(HttpWebRequest.Create(ulrWeb));
                //Post请求方式
                request.Method = "POST";
                //内容类型
                request.ContentType = "text/xml;charset=GB2312";
                //设置请求的 ContentLength
                request.ContentLength = sendByte.Length;
                //获得请求流
                Stream fs = request.GetRequestStream();
                fs.Write(sendByte, 0, sendByte.Length);
                fs.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();
                XmlTextReader xmlReader = new XmlTextReader(s);
                xmlReader.MoveToContent();
                string recv = xmlReader.ReadInnerXml();
                //recv = recv.Replace("&amp;", "&");
                //recv = recv.Replace("&lt;", "<");
                //recv = recv.Replace("&gt;", ">");
                //recv = recv.Replace("&apos;", "'");
                //recv = recv.Replace("&quot;", "\""); //((char)34) 双引号
                reponseXml = recv;
                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }

        #region GET/POST方法
        /// <summary>
        /// POST方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestData">JSON格式</param>
        /// <param name="reponseData">JSON格式</param>
        /// <param name="er"></param>
        /// <param name="reponseTimeOut">-1:一直等待</param>
        /// <returns></returns>
        public static bool HttpPost(string url, string requestData, out string reponseData, out string er, int reponseTimeOut = 5000)
        {
            reponseData = string.Empty;

            er = string.Empty;

            try
            {
                string retString = string.Empty;

                byte[] buf = System.Text.Encoding.GetEncoding("utf-8").GetBytes(requestData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = reponseTimeOut;
                request.Method = "POST";
                request.ContentLength = buf.Length;

                request.ContentType = "application/json;charset=UTF-8";
                //request.MaximumAutomaticRedirections = 1;
                //request.AllowAutoRedirect = true;

                Stream myRequestStream = request.GetRequestStream();
                myRequestStream.Write(buf, 0, buf.Length);
                myRequestStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                reponseData = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return true;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    WebResponse response = ex.Response;

                    HttpWebResponse httpResponse = (HttpWebResponse)response;

                    Stream data = response.GetResponseStream();

                    var reader = new StreamReader(data);

                    er = reader.ReadToEnd();
                }
                if (er == string.Empty)
                {
                    er = ex.ToString();
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
        /// GET方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestData">JSON格式</param>
        /// <param name="reponseData">JSON格式</param>
        /// <param name="er"></param>
        /// <param name="reponseTimeOut">-1:一直等待</param>
        /// <returns></returns>
        public static bool HttpGet(string url, string requestData, out string reponseData, out string er, int reponseTimeOut = 5000)
        {
            reponseData = string.Empty;

            er = string.Empty;

            try
            {
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);

                string strUrl = url + (requestData == "" ? "" : "?") + requestData;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
                request.Timeout = reponseTimeOut;
                request.Method = "GET";
                //request.ContentType = "text/html;charset=UTF-8";
                request.ContentType = "application/json;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                reponseData = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return true;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    WebResponse response = ex.Response;

                    HttpWebResponse httpResponse = (HttpWebResponse)response;

                    Stream data = response.GetResponseStream();

                    var reader = new StreamReader(data);

                    er = reader.ReadToEnd();
                }

                if (er == string.Empty)
                {
                    er = ex.ToString();
                }

                return false;
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
