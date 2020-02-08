using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GJ.DEV.PLC
{
    #region 状态定义
    /// <summary>
    /// PLC类型
    /// </summary>
    public enum EPlcType
    {
        /// <summary>
        /// 汇川TCP
        /// </summary>
        Inovance_TCP,
        /// <summary>
        /// 汇川串口
        /// </summary>
        Inovance_COM,
        /// <summary>
        /// 欧姆龙TCP
        /// </summary>
        Omron_TCP,
        /// <summary>
        /// 欧姆龙UDP
        /// </summary>
        Omron_UDP,
        /// <summary>
        /// 欧姆龙串口
        /// </summary>
        Omron_COM,
        /// <summary>
        /// 台达串口
        /// </summary>
        Delta_COM,
        /// <summary>
        /// 产电TCP
        /// </summary>
        Lsis_TCP,
        /// <summary>
        /// 三菱FX3U
        /// </summary>
        FX3U_TCP,
        /// <summary>
        /// 三菱FX5U
        /// </summary>
        FX5U_TCP
    }
    /// <summary>
    /// 连接状态
    /// </summary>
    public enum EStatus
    {
        空闲,
        连接中,
        正常,
        断开,
        错误
    }
    /// <summary>
    /// 通信接口
    /// </summary>
    public enum EConType
    {
        RS232,
        TCP
    }
    /// <summary>
    /// 线圈:M,W,X,Y;寄存器:D
    /// </summary>
    public enum ERegType
    {
        M,
        W,
        D,
        X,
        Y,
        WB
    }
    /// <summary>
    /// 消息状态
    /// </summary>
    public enum EMessage
    {
        启动,
        正常,
        异常,
        暂停,
        退出
    }
    #endregion

    #region PLC消息
    /// <summary>
    /// 定义状态消息
    /// </summary>
    public class CPLCConArgs : EventArgs
    {
        public CPLCConArgs(int idNo, string name, string status, EMessage e)
        {
            this.idNo = idNo;
            this.name = name;
            this.status = status;
            this.e = e;
        }
        public readonly int idNo;
        public readonly string name;
        public readonly string status;
        public readonly EMessage e;
    }
    /// <summary>
    /// 定义数据消息
    /// </summary>
    public class CPLCDataArgs : EventArgs
    {
        public CPLCDataArgs(int idNo, string name, string rData, EMessage e)
        {
            this.idNo = idNo;
            this.name = name;
            this.rData = rData;
            this.e = e;
        }
        public readonly int idNo;
        public readonly string name;
        public readonly string rData;
        public readonly EMessage e;
    }
    #endregion

    #region 参数类
    public class CPLCPara
    {
        #region 常量定义
        public const int NG = 2;
        public const int ON = 1;
        public const int OFF = 0;
        #endregion
    }
    #endregion

    #region PLC报警列表
    /// <summary>
    /// 报警寄存器
    /// </summary>
    public class CPLCAlarmReg
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int idNo{get;set;}
        /// <summary>
        /// 寄存器地址
        /// </summary>
        public int RegNo { get; set; }
        /// <summary>
        /// 位地址
        /// </summary>
        public int RegBit { get; set; }
        /// <summary>
        /// 寄存器功能
        /// </summary>
        public string RegFun { get; set; }
        /// <summary>
        /// 报警等级
        /// </summary>
        public int RegLevel { get; set; }
        /// <summary>
        /// 寄存器值
        /// </summary>
        public int RegVal { get; set; }
        /// <summary>
        /// 当前读值
        /// </summary>
        public int CurVal { get; set; }
        /// <summary>
        /// 报警时间
        /// </summary>
        public string HappenTime { get; set; }
    }
    /// <summary>
    /// 报警类
    /// </summary>
    public class CPLCAlarmList
    {
        /// <summary>
        /// D寄存器位数
        /// </summary>
        public static int C_BIT_MAX = 16;
        /// <summary>
        /// 寄存器映射
        /// </summary>
        private Dictionary<int, int> regMapp = new Dictionary<int, int>();
        /// <summary>
        /// 寄存器列表
        /// </summary>
        private List<CPLCAlarmReg> regList = new List<CPLCAlarmReg>();

        private int idNo = 0;

        private string name = string.Empty;

        private ReaderWriterLock objlock = new ReaderWriterLock();

        public CPLCAlarmList(int idNo, string name)
        {
            this.idNo = idNo;
            this.name = name;
        }
        public override string ToString()
        {
            return name;
        }
        /// <summary>
        /// 添加寄存器
        /// </summary>
        /// <param name="reg"></param>
        public void AddReg(CPLCAlarmReg reg)
        {
            try
            {
                objlock.AcquireWriterLock(-1);

                regList.Add(reg);

                if (!regMapp.ContainsKey(reg.idNo))
                {
                    regMapp.Add(reg.idNo, regList.Count);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objlock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 获取报警信息
        /// </summary>
        /// <param name="plcThread"></param>
        /// <param name="alarmReg"></param>
        /// <param name="releaseReg"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool GetAlarmCode(List<int> RegVal, out List<CPLCAlarmReg> alarmReg, out List<CPLCAlarmReg> releaseReg,out string er)
        {
            er = string.Empty;

            alarmReg = new List<CPLCAlarmReg>();

            releaseReg = new List<CPLCAlarmReg>(); 

            try
            {
                objlock.AcquireWriterLock(-1);

                string sNowTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                for (int i = 0; i < RegVal.Count; i++)
                {
                    if (RegVal[i] == -1)
                        continue;

                    for (int z = 0; z < C_BIT_MAX; z++)
                    {
                        int idNo = i * C_BIT_MAX + z;

                        if (!regMapp.ContainsKey(idNo + 1))
                            continue;

                        int index = regMapp[idNo + 1] - 1;   //编号从1开始

                        int bitVal = 0;

                        if ((RegVal[i] & (1 << z)) == (1 << z))
                            bitVal = 1;

                        regList[index].CurVal = bitVal;

                        if (regList[index].CurVal == -1)
                            continue;

                        if (regList[index].CurVal == CPLCPara.ON)
                        {
                            if (regList[index].RegVal != regList[index].CurVal)
                            {
                                regList[index].RegVal = regList[index].CurVal;

                                CPLCAlarmReg reg = new CPLCAlarmReg
                                {
                                    idNo = regList[index].idNo,
                                    RegNo = i,
                                    RegBit = z,
                                    RegVal = regList[index].RegVal,
                                    RegFun = regList[index].RegFun,
                                    CurVal = regList[index].CurVal,
                                    HappenTime = sNowTime
                                };
                                alarmReg.Add(reg);
                            }
                        }
                        else
                        {
                            if (regList[index].RegVal != regList[index].CurVal)
                            {
                                regList[index].RegVal = regList[index].CurVal;

                                CPLCAlarmReg reg = new CPLCAlarmReg
                                {
                                    idNo = regList[index].idNo,
                                    RegNo = i,
                                    RegBit = z,
                                    RegVal = regList[index].RegVal,
                                    RegFun = regList[index].RegFun,
                                    CurVal = regList[index].CurVal,
                                    HappenTime = sNowTime
                                };
                                releaseReg.Add(reg);
                            }
                        }

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                objlock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 获取当前报警信息
        /// </summary>
        /// <param name="RegVal"></param>
        /// <param name="alarmReg"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool GetCurAlarmInfo(List<int> RegVal, out List<CPLCAlarmReg> alarmReg, out string er)
        {
            er = string.Empty;

            alarmReg = new List<CPLCAlarmReg>();

            try
            {
                objlock.AcquireWriterLock(-1);

                string sNowTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                for (int i = 0; i < RegVal.Count; i++)
                {
                    if (RegVal[i] == -1)
                        continue;

                    for (int z = 0; z < C_BIT_MAX; z++)
                    {
                        int idNo = i * C_BIT_MAX + z;

                        if (!regMapp.ContainsKey(idNo + 1))
                            continue;

                        int index = regMapp[idNo + 1] - 1;

                        int bitVal = 0;

                        if ((RegVal[i] & (1 << z)) == (1 << z))
                            bitVal = 1;

                        if (bitVal == CPLCPara.ON)
                        {
                            CPLCAlarmReg reg = new CPLCAlarmReg
                            {
                                idNo = regList[index].idNo,
                                RegNo = i,
                                RegBit = z,
                                RegVal = regList[index].RegVal,
                                RegFun = regList[index].RegFun,
                                CurVal = regList[index].CurVal,
                                HappenTime = sNowTime
                            };
                            alarmReg.Add(reg);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                objlock.ReleaseWriterLock();
            }
        }
    }
    #endregion
   

}
