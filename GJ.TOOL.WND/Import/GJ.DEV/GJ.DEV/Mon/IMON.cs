using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.Mon
{
    public interface IMON
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
        /// 读控制板版本
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="version"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadVersion(int wAddr, out string version, out string er);
        /// <summary>
        /// 设定工作模式
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="wMode"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetWorkMode(int wAddr, int wMode, out string er);
        /// <summary>
        /// 设置ON/OFF参数
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="wPara"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetOnOffPara(int wAddr, COnOffPara wPara, out string er);
        /// <summary>
        /// 启动老化测试
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="wPara"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetRunStart(int wAddr, CwRunPara wPara, out string er);
        /// <summary>
        /// 强制结束
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ForceFinish(int wAddr, out string er);
        /// <summary>
        /// 启动AC ON/AC OFF
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="wOnOff"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool RemoteACOnOff(int wAddr, EOnOff wOnOff, out string er);
        /// <summary>
        /// 控制通道Relay ON
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="wRelayNo"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetRelayOn(int wAddr, int wRelayNo, out string er);
        /// <summary>
        /// 回读ON/OFF参数
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="rPara"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadOnOffPara(int wAddr, ref COnOffPara rPara, out string er);
        /// <summary>
        /// 从监控板读取电压及各个状态数据，电压数据基于同步AC ON/OFF信号
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="rVolt"></param>
        /// <param name="er"></param>
        /// <param name="synNo"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        bool ReadVolt(int wAddr, out List<double> rVolt, out int rOnOff, out string er,
                     ESynON synNo = ESynON.同步, ERunMode mode = ERunMode.自动线模式);                                 
        /// <summary>
        /// 读取控制板测试信号数据
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="rPara"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadRunData(int wAddr, ref CrRunPara rPara, out string er);
        /// <summary>
        /// 发扫描命令
        /// </summary>
        void SetScanAll();
        /// <summary>
        /// 设置快充模式
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="wPara"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetGJM_RunQC_Para(int wAddr, COnOffPara wPara, out string er);
        /// <summary>
        /// 读取快充模式
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="rPara"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadGJM_RunQC_Para(int wAddr, ref COnOffPara rPara, out string er);
        /// <summary>
        /// 暂停和继续
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="wContinue"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ControlPauseOrContinue(int wAddr, int wContinue, out string er);
        /// <summary>
        /// Y点控制
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="wYno"></param>
        /// <param name="wOnOff"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ControlY_OnOff(int wAddr, int wYno, int wOnOff, out string er);
        #endregion
    }
}
