using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.Meter
{
    public interface IMeter
    {
        #region 属性
        /// <summary>
        /// 设备ID
        /// </summary>
        int idNo
        { set; get; }
        /// <summary>
        /// 设备名称
        /// </summary>
        string name
        { set; get; }
        /// <summary>
        /// 连接状态
        /// </summary>
        bool conStatus
        { get; }
        #endregion

        #region 方法
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="comName"></param>
        /// <param name="setting"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool Open(string comName, out string er, string setting);
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        void Close();
        /// <summary>
        /// 读取电压
        /// </summary>
        /// <param name="devAddr"></param>
        /// <param name="acv"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadACV(int devAddr, out double acv, out string er);
        /// <summary>
        /// 读取电流
        /// </summary>
        /// <param name="devAddr"></param>
        /// <param name="aci"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadACI(int devAddr, out double aci, out string er);
        #endregion
    }
}
