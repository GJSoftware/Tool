using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
namespace GJ.COM
{
    public class CWCF
    {
        #region 绑定协议
        /// <summary>
        /// 绑定协议
        /// </summary>
        public enum EProtocol
        {
            /// <summary>
            /// 最基础的http绑定
            /// </summary>
            BasicHttpBinding,
            /// <summary>
            /// TCP
            /// </summary>
            NetTcpbingding,
            /// <summary>
            /// 进程间通信
            /// </summary>
            NetNamePipeBinding,
            /// <summary>
            /// 特殊的http/Https协议
            /// </summary>
            WSHttpBinding,
            /// <summary>
            /// 消息队列
            /// </summary>
            NetMsmqBindiing
        }
        #endregion

        #region WCF服务工厂
        public static T CreateServiceByUrl<T>(string url)
        {
            return CreateServiceByUrl<T>(url, EProtocol.BasicHttpBinding);
        }
        public static T CreateServiceByUrl<T>(string url, EProtocol bing)
        {
            try
            {
                if (string.IsNullOrEmpty(url)) throw new NotSupportedException("This url is not Null or Empty!");
                EndpointAddress address = new EndpointAddress(url);
                Binding binding = CreateBinding(bing);
                ChannelFactory<T> factory = new ChannelFactory<T>(binding, address);
                return factory.CreateChannel();
            }
            catch (Exception ex)
            {
                throw new Exception("创建服务工厂出现异常:" + ex.ToString());
            }
        }
        #endregion

        #region 创建传输协议
        /// <summary>
        /// 创建传输协议
        /// </summary>
        /// <param name="binding">传输协议名称</param>
        /// <returns></returns>
        private static Binding CreateBinding(EProtocol binding)
        {
            Binding bindinginstance = null;

            if (binding == EProtocol.BasicHttpBinding)
            {
                BasicHttpBinding ws = new BasicHttpBinding();
                ws.MaxBufferSize = 2147483647;
                ws.MaxBufferPoolSize = 2147483647;
                ws.MaxReceivedMessageSize = 2147483647;
                ws.ReaderQuotas.MaxStringContentLength = 2147483647;
                ws.CloseTimeout = new TimeSpan(0, 30, 0);
                ws.OpenTimeout = new TimeSpan(0, 30, 0);
                ws.ReceiveTimeout = new TimeSpan(0, 30, 0);
                ws.SendTimeout = new TimeSpan(0, 30, 0);
                ws.Security.Mode = BasicHttpSecurityMode.None;
                bindinginstance = ws;
            }
            else if (binding == EProtocol.NetTcpbingding)
            {
                NetTcpBinding ws = new NetTcpBinding();
                ws.MaxReceivedMessageSize = 65535000;
                ws.Security.Mode = SecurityMode.None;
                bindinginstance = ws;
            }
            else if (binding == EProtocol.WSHttpBinding)
            {
                WSHttpBinding ws = new WSHttpBinding(SecurityMode.None);
                ws.MaxBufferPoolSize = 65535000;
                ws.MaxReceivedMessageSize = 65535000;
                ws.TextEncoding = Encoding.UTF8;
                ws.Security.Message.ClientCredentialType = System.ServiceModel.MessageCredentialType.Windows;
                ws.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Windows;
                bindinginstance = ws;
            }
            return bindinginstance;
        }
        #endregion
    }
}
