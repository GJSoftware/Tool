using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
namespace GJ.DEV.RemoteIO
{
    #region 枚举
    /// <summary>
    /// 类型
    /// </summary>
    public enum EType
    {
        IO_24_16
    }
    /// <summary>
    /// 线程状态
    /// </summary>
    public enum EThreadStatus
    {
        空闲,
        运行,
        暂停,
        退出
    }
    /// <summary>
    /// 寄存器类型
    /// </summary>
    public enum ERegType
    {
        X,
        Y,
        D
    }
    #endregion

    #region 消息定义
    public class CConArgs : EventArgs
    {
        public readonly string conStatus;
        public readonly bool bErr;
        public CConArgs(string conStatus, bool bErr = false)
        {
            this.conStatus = conStatus;
            this.bErr = bErr;
        }
    }
    public class CDataArgs : EventArgs
    {
        public readonly string rData;
        public readonly bool bErr;
        public readonly bool bComplete;
        public CDataArgs(string rData, bool bComplete = true, bool bErr = false)
        {
            this.rData = rData;
            this.bComplete = bComplete;
            this.bErr = bErr;
        }
    }
    #endregion

}
