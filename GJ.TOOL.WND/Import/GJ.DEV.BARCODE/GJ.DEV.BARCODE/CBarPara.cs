using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.BARCODE
{
    /// <summary>
    /// 条码枪型号
    /// </summary>
    public enum EBarType
    { 
       CR1000,
       KS100,
       CR85,
       CR55,
       DCR202,
       LECTOR620,
       H3320G,
       SR700,
       DM70,
       SR710_TCP
    }
    /// <summary>
    /// 通信接口
    /// </summary>
    public enum EComMode
    { 
      SerialPort,
      TCP,
      Telnet
    }

    #region 事件消息
    /// <summary>
    /// 接收数据消息
    /// </summary>
    public class CRecvArgs : EventArgs
    {
        public CRecvArgs(int idNo, string recvData)
        {
            this.idNo = idNo;
            this.recvData = recvData;
        }
        public  readonly int idNo = 0;
        public readonly string recvData = string.Empty;
    }
    public delegate void OnRecvHandler(object sender,CRecvArgs e);
    #endregion
}
