using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.FCMB
{
    /// <summary>
    /// 快充中控板
    /// </summary>
    public interface IFCMB
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
        /// 读设备名称
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool ReadName(int addr, out string name,out string er);
        /// <summary>
        /// 读设备序列号
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        bool ReadSn(int addr, out string sn, out string er);
        /// <summary>
        /// 读地址D8000
        /// </summary>
        /// <param name="curAddr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadAddr(out int curAddr, out string er);
        /// <summary>
        /// 设置地址D8000
        /// </summary>
        /// <param name="curAddr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetAddr(int curAddr, out string er);
        /// <summary>
        /// 读取版本号D8001
        /// </summary>
        /// <param name="rVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadVersion(int addr, out int rVal, out string er);
        /// <summary>
        /// 读取版本号D8001
        /// </summary>
        /// <param name="rVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadVersion(int addr, out string rVal, out string er);
        /// <summary>
        /// 读波特率D8002
        /// </summary>
        /// <param name="baud"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadBaud(int addr, out int baud, out string er);
        /// <summary>
        /// 设置波特率D8002
        /// </summary>
        /// <param name="baud"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetBaud(int addr, int baud, out string er);
        /// <summary>
        /// 设置小板地址D8003
        /// </summary>
        /// <param name="addr">要求中位拨码开关为 on</param>
        /// <param name="childAddr">小板的开关需要为 on</param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetChildAddr(int childAddr, out string er);
        /// <summary>
        /// 当小板通信不上时值为 0，正常大于 0
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="childVer"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadChildVersion(int addr, int childMax, out List<int> childVer, out string er);
        /// <summary>
        /// 快充模式配置D8007
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="qcm"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetQCM(int addr, EQCM qcm, double qcv, double qci, out string er,bool cc2);
        /// <summary>
        /// 快充模式配置D800
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="qcm"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadQCM(int addr, out EQCM qcm, out double qcv, out double qci, out string er);
        /// <summary>
        /// 时序控制模式D8070
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="mode"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetOnOffMode(int addr, EOnOffMode mode, out string er);
        /// <summary>
        /// 读取时序控制模式D8070
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="mode"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadOnOffMode(int addr, out EOnOffMode mode, out string er);
        /// <summary>
        /// 时序总时间(Min)D8071
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="runMin"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetTotalTime(int addr, int runMin, out string er);
        /// <summary>
        /// 读取总时间(Min)
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="runMin"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadTotalTime(int addr, out int runMin, out string er);
        /// <summary>
        /// 设置时序段D8050
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="OnOff">
        /// 每 3 个数据为一组，分别为 on 时间、off 时间和重复次数
        /// 总共 10 组，时间单位为秒</param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetOnOffTime(int addr, List<COnOff> OnOff, out string er);
        /// <summary>
        /// 读取时序段D8050
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="OnOff"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadOnOffTime(int addr, out List<COnOff> OnOff, out string er);
        /// <summary>
        /// 直接操作时序开关D806F
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="wOnOff"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetACON(int addr, int wOnOff, out string er,bool synC);
        /// <summary>
        /// 控制AC ONOFF
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="wOnOff"></param>
        /// <param name="er"></param>
        /// <param name="synC"></param>
        /// <param name="B400"></param>
        /// <returns></returns>
        bool SetACON(int addr, int wOnOff, out string er, bool synC, bool B400);
        /// <summary>
        /// 读取时序开关D806F
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="rOnOff">
        /// 0：控制开
        /// 1：控制关
        /// </param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadACON(int addr, out int rOnOff, out string er);
        /// <summary>
        /// 读取IO信号D8179
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="io"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadIO(int addr, out List<int> io, out string er);
        /// <summary>
        /// 设置IO-D817A
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="ioType"></param>
        /// <param name="wOnOff"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetIO(int addr, EFMB_wIO ioType, int wOnOff, out string er);
        /// <summary>
        /// 读取产品电压D8128
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="volt">5000 表示5V</param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadVolt(int addr, out List<double> volt, out string er, bool sync = true, EVMODE voltMode = EVMODE.VOLT_40);
        /// <summary>
        /// 读取AC电压
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="acv">单位 0.1V，2200 表示 220.0V</param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadAC(int addr, out double acv, out string er);
        /// <summary>
        /// 读取测试D+、D-短路情况
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="chan">1-32</param>
        /// <param name="value">0:OK-->大于零:自测 D+对GND、D-对GND、D+对D-、D-对V+</param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadDGND(int addr, int chan, out string status, out string er);
        /// <summary>
        /// 读取测试D+、D-短路情况
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="chan"></param>
        /// <param name="status"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadDGND(int addr, int chan, out List<int> status, out string er);
        /// <summary>
        /// Y0-Y7控制
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="Y"></param>
        /// <param name="wOnOff"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetIO(int addr, EFMB_Y Y, int wOnOff, out string er);
        #endregion
    }
}
