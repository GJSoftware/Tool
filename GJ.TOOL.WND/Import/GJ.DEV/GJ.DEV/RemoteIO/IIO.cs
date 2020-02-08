using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.RemoteIO
{
    public interface IIO
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
        /// 读N个寄存器16进制字符
        /// </summary>
        /// <param name="startAddr"></param>
        /// <param name="N"></param>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool Read(int devAddr, ERegType regType, int regAddr, int N, out string rData, out string er);
        /// <summary>
        /// 读单个寄存器数值
        /// </summary>
        /// <param name="devAddr"></param>
        /// <param name="regType"></param>
        /// <param name="regAddr"></param>
        /// <param name="rVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool Read(int devAddr, ERegType regType, int regAddr, out int rVal, out string er);
        /// <summary>
        /// 读N个寄存器数值
        /// </summary>
        /// <param name="devAddr"></param>
        /// <param name="regType"></param>
        /// <param name="regAddr"></param>
        /// <param name="rVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool Read(int devAddr, ERegType regType, int regAddr, ref int[] rVal, out string er);
        /// <summary>
        /// 写寄存器数值
        /// </summary>
        /// <param name="devAddr"></param>
        /// <param name="regType"></param>
        /// <param name="regAddr"></param>
        /// <param name="wVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool Write(int devAddr, ERegType regType, int regAddr, int wVal, out string er);
        /// <summary>
        /// 写多个寄存器数值
        /// </summary>
        /// <param name="devAddr"></param>
        /// <param name="regType"></param>
        /// <param name="regAddr"></param>
        /// <param name="wVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool Write(int devAddr, ERegType regType, int regAddr, int[] wVal, out string er);
        #endregion

        #region 专用功能
        /// <summary>
        /// 读地址
        /// </summary>
        /// <param name="curAddr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadAddr(out int curAddr, out string er);
        /// <summary>
        /// 设置地址
        /// </summary>
        /// <param name="curAddr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetAddr(int curAddr, out string er);
        /// <summary>
        /// 读波特率
        /// </summary>
        /// <param name="baud"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadBaud(int curAddr, out int baud, out string er);
        /// <summary>
        /// 设置波特率
        /// </summary>
        /// <param name="baud"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetBaud(int curAddr, int baud, out string er);
        /// <summary>
        /// 读错误码
        /// </summary>
        /// <param name="rVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadErrCode(int curAddr, out int rVal, out string er);
        /// <summary>
        /// 读错误码
        /// </summary>
        /// <param name="rVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadVersion(int curAddr, out int rVal, out string er);
        #endregion
    }
}
