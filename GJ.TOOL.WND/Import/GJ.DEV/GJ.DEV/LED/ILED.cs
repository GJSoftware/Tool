using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.LED
{
    public interface ILED
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
        /// <summary>
        /// 负载通道数
        /// </summary>
        int maxCH
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
        /// 读版本
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="version"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadVersion(int wAddr, out string version, out string er);
        /// <summary>
        /// 读取负载设置
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="chanList"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadLoadSetting(int wAddr, out List<CLOAD> chanList, out string er);
        /// <summary>
        /// 设置8个电流通道负载
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="chanList"></param>
        /// <param name="saveEEPROM"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetLoadValue(int wAddr, List<CLOAD> chanList, bool saveEEPROM, out string er);
        /// <summary>
        /// 设置8个电流通道负载
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="chanList"></param>
        /// <param name="saveEEPROM"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetLoadValue(int wAddr, int chanNo, CLOAD chanPara, bool saveEEPROM, out string er);
        /// <summary>
        /// 读取负载数据
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="data"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadLoadValue(int wAddr, ref CData data, out string er);
        #endregion
    }
}
