using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.DD
{
    public interface IDD
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
        /// 设置地址
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetNewAddr(int wAddr, out string er);
        /// <summary>
        /// 设置负载
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="loadPara"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetNewLoad(int wAddr, CwLoad loadPara, out string er);
        /// <summary>
        /// 设置负载
        /// </summary>
        /// <param name="wStartAddr"></param>
        /// <param name="wEndAddr"></param>
        /// <param name="loadPara"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetNewLoad(int wStartAddr, int wEndAddr, CwLoad loadPara, out string er);
        /// <summary>
        /// 读取负载值
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="loadSet"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadLoadSet(int wAddr, ref CrLoad loadSet, out string er);
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadData(int wAddr, ref CrData rData, out string er);
        /// <summary>
        /// 设置PS_ON
        /// </summary>
        /// <param name="wStartAddr"></param>
        /// <param name="wEndAddr"></param>
        /// <param name="wOnOff"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetPS_ON(int wStartAddr, int wEndAddr, int wOnOff, out string er);
        #endregion
    }
}
