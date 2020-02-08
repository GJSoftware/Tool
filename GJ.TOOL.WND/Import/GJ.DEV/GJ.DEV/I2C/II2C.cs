using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.I2C
{
    public interface II2C
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
        /// <param name="er"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        bool Open(string comName, out string er, string setting = "57600,n,8,1");        
        /// <summary>
        /// 关闭串口
        /// </summary>
        void Close();        
        /// <summary>
        /// 设置地址
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetNewAddr(int wAddr, out string er);        
        /// <summary>
        /// 读取版本
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="ver"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadVersion(int wAddr, out string ver, out string er);        
        /// <summary>
        /// 设置I2C运行参数
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="para"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SendToSetI2C_RunPara(int wAddr, CI2C_RunPara para, out string er);        
        /// <summary>
        /// 读取I2C运行参数
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="para"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadI2C_RunPara(int wAddr, ref CI2C_RunPara para, out string er);        
        /// <summary>
        /// 读取I2C数据
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="uutNo">产品1或产品2</param>
        /// <param name="data"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadI2C_Data(int wAddr, int uutNo, ref CI2C_Data data, out string er);        
        #endregion

    }
}
