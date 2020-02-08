using System;
using System.Collections.Generic;
using System.Text;

namespace GJ.DEV.BARCODE
{
    /// <summary>
    /// 条码枪接口
    /// </summary>
    public interface IBarCode
    {
        #region 事件定义
        /// <summary>
        /// 接收数据事件
        /// </summary>
        event OnRecvHandler OnRecved;
        #endregion

        #region 属性
        /// <summary>
        /// id编号
        /// </summary>
        int idNo
        {
            get;
            set;
        }
        /// <summary>
        /// 设备名称
        /// </summary>
        string name
        {
            get;
            set;
        }
        /// <summary>
        /// 通信接口
        /// </summary>
        EComMode comMode
        {
            get;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="comName"></param>
        /// <param name="er"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        bool Open(string comName, out string er, string setting = "115200,n,8,1", bool recvThreshold=false);
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        void Close();
        /// <summary>
        /// 读条码
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="er"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        bool Read(out string serialNo, out string er, int rLen = 0, int timeOut = 1000);
        bool Read(out string serialNo, out string er, string SOI, int rLen = 0, int timeOut = 2000);
        /// <summary>
        /// 触发条码枪接收数据
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        bool Triger_Start(out string er);
        /// <summary>
        ///  停止触发条码
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        bool Triger_End(out string er);
        #endregion

    }
}
