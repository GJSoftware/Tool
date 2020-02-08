using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.COM
{
    #region 枚举
    /// <summary>
    /// 数据类型
    /// </summary>
    public enum EDataType
    {
        ASCII格式,
        HEX格式
    }
    #endregion

    #region 串口消息
    /// <summary>
    /// 串口消息
    /// </summary>
    public class CComArgs : EventArgs
    {
        public CComArgs(int idNo, string recv)
        {
            this.idNo = idNo;
            this.recv = recv;        
        }
        public readonly int idNo;
        public readonly string recv;
    }
    #endregion

    #region TCP/IP消息
    /// <summary>
    /// Tcp连接状态
    /// </summary>
    public class CTcpConArgs : EventArgs
    {
        public int idNo;

        public string name;

        public readonly string conStatus;

        public readonly bool bErr;

        public readonly string remoteIP;

        public readonly int remoteStatus;

        public CTcpConArgs(int idNo, string name, string conStatus,
                           bool bErr = false, string remoteIP = "", int remoteStatus = 0)
        {
            this.idNo = idNo;
            this.name = name;
            this.conStatus = conStatus;
            this.bErr = bErr;
            this.remoteIP = remoteIP;
            this.remoteStatus = remoteStatus;
        }
    }
    /// <summary>
    /// Tcp数据接收类
    /// </summary>
    public class CTcpRecvArgs : EventArgs
    {
        public int idNo;

        public string name;

        public readonly string remoteEndPoint;

        public readonly string recvData;

        public readonly byte[] recvBytes;

        public CTcpRecvArgs(int idNo, string name, string remoteEndPoint, string recvData, byte[] recvBytes)
        {
            this.idNo = idNo;
            this.name = name;
            this.remoteEndPoint = remoteEndPoint;
            this.recvData = recvData;
            this.recvBytes = (byte[])recvBytes.Clone();
        }
    }
    #endregion

}
