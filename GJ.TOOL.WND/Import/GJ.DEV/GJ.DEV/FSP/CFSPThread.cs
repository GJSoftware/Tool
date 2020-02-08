using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;
using System.Threading;
using System.Diagnostics;

namespace GJ.DEV.FSP
{
    public class CFSPThread
    {
        #region 构造函数
        public CFSPThread(int idNo, string name, int startAddr, int endAddr, bool autoMode = false)
        {
            this._idNo = idNo;

            this._name = name;

            this._startAddr = startAddr;

            this._endAddr = endAddr;

            this._autoMode = autoMode;

            for (int i = startAddr; i <= endAddr; i++)
            {
                CMon mon = new CMon();

                mon.Base.addr = i;

                mon.Base.name = _name + "-【" + i.ToString("D2") + "】";

                mon.Base.status = ESTATUS.运行;

                mon.Base.conStatus = true;

                _Mon.Add(i, mon); 
            }
        }
        public override string ToString()
        {
            return "<" + name + ">";
        }
        #endregion

        #region 字段
        /// <summary>
        /// 线程id
        /// </summary>
        private int _idNo = 0;
        /// <summary>
        /// 线程名称
        /// </summary>
        private string _name = string.Empty;
        /// <summary>
        /// 开始地址
        /// </summary>
        private int _startAddr = 1;
        /// <summary>
        /// 结束地址
        /// </summary>
        private int _endAddr = 56;
        /// <summary>
        /// 自动模式:不间断扫描;手动模式:需手动启动扫描
        /// </summary>
        private bool _autoMode = true;
        /// <summary>
        /// 暂停监控
        /// </summary>
        private volatile bool _paused = false;
        /// <summary>
        /// 退出线程
        /// </summary>
        private volatile bool _dispose = false;
        /// <summary>
        /// 扫描间隔
        /// </summary>
        private volatile int _delayMs = 10;
        /// <summary>
        /// 线程状态
        /// </summary>
        private volatile EThreadStatus _threadStatus = EThreadStatus.空闲;
        /// <summary>
        /// 通信设备
        /// </summary>
        private CDD_48V _devMon = null;
        /// <summary>
        /// ERS线程
        /// </summary>
        private Thread _threadMon = null;
        /// <summary>
        /// 线程同步锁
        /// </summary>
        private ReaderWriterLock _syncLock = new ReaderWriterLock();
        /// <summary>
        /// 基本信息:【地址->信息】
        /// </summary>
        public Dictionary<int, CMon> _Mon = new Dictionary<int, CMon>();
        /// <summary>
        /// 初始化标志
        /// </summary>
        private bool _iniOK = false;
        #endregion

        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        public int idNo
        {
            get { return _idNo; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string name
        {
            get { return _name; }
        }
        /// <summary>
        /// 自动模式
        /// </summary>
        public bool autoMode
        {
            get { return _autoMode; }
            set { _autoMode = value; }
        }
        /// <summary>
        /// 设置延时
        /// </summary>
        public int delayMs
        {
            set { _delayMs = value; }
        }
        /// <summary>
        /// 线程状态
        /// </summary>
        public EThreadStatus threadStatus
        {
            get { return _threadStatus; }
        }
        #endregion

        #region 事件定义
        /// <summary>
        /// 状态事件
        /// </summary>
        public COnEvent<CConArgs> OnStatusArgs = new COnEvent<CConArgs>();
        /// <summary>
        /// 数据事件
        /// </summary>
        public COnEvent<CDataArgs> OnDataArgs = new COnEvent<CDataArgs>();
        /// <summary>
        /// 操作消息
        /// </summary>
        public COnEvent<COPArgs> OnOpArgs = new COnEvent<COPArgs>();
        #endregion

        #region 线程
        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="plc"></param>
        public void SpinUp(CDD_48V devMon, bool autoMode = false)
        {
            this._devMon = devMon;

            this.autoMode = autoMode;

            if (_threadMon == null)
            {
                _dispose = false;

                if (_autoMode)
                {
                    _paused = false;
                    _threadStatus = EThreadStatus.运行;
                }
                else
                {
                    _paused = true;
                    _threadStatus = EThreadStatus.暂停;
                }
                _iniOK = false;
                _threadMon = new Thread(OnStart);
                _threadMon.Name = _name;
                _threadMon.IsBackground = true;
                _threadMon.Start();
                OnStatusArgs.OnEvented(new CConArgs(idNo, name, "创建" + _threadMon.Name + "监控线程"));
            }
        }
        /// <summary>
        /// 销毁线程
        /// </summary>
        public void SpinDown()
        {
            try
            {
                if (_threadMon != null)
                {
                    _dispose = true;
                    do
                    {
                        if (!_threadMon.IsAlive)
                            break;

                       CTimer.DelayMs(5);

                    } while (_dispose);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                _threadMon = null;
            }
        }
        /// <summary>
        /// 暂停线程
        /// </summary>
        public void Paused()
        {
            if (!_autoMode && _threadMon != null)
            {
                _paused = true;
                _threadStatus = EThreadStatus.暂停;
            }
        }
        /// <summary>
        /// 恢复线程
        /// </summary>
        public void Continued()
        {
            if (!_autoMode && _threadMon != null)
            {
                _paused = false;
                _threadStatus = EThreadStatus.运行;
            }
        }
        /// <summary>
        /// 启动监控
        /// </summary>
        private void OnStart()
        {
            try
            {
                while (true)
                {
                    if (_dispose)
                        return;

                    Thread.Sleep(_delayMs);

                    if (!_autoMode && _paused)
                        continue;

                    Stopwatch wather = new Stopwatch();

                    wather.Start();

                    string er = string.Empty;

                    if (_iniOK)
                    {
                        if (!WriteData(out er))
                            OnDataArgs.OnEvented(new CDataArgs(idNo, name, er, false, true));
                    }

                    if (!ReadData(out er))
                        OnDataArgs.OnEvented(new CDataArgs(idNo, name, er, false, true));

                    _iniOK = true;

                    wather.Stop();

                    if (!_autoMode)
                    {
                        _paused = true;
                        _threadStatus = EThreadStatus.暂停;
                        OnDataArgs.OnEvented(new CDataArgs(idNo, name, "监控扫描时间:" + wather.ElapsedMilliseconds.ToString() + "ms"));
                        continue;
                    }

                    _threadStatus = EThreadStatus.运行;

                }
            }
            catch (Exception ex)
            {
                OnStatusArgs.OnEvented(new CConArgs(idNo, name, _threadMon.Name + "监控线程异常错误:" + ex.ToString(), true));
            }
            finally
            {
                _dispose = false;
                _threadStatus = EThreadStatus.退出;
                OnStatusArgs.OnEvented(new CConArgs(idNo, name, _threadMon.Name + "监控线程销毁退出", true));
            }
        }
        /// <summary>
        /// 读ERS信号
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool ReadData(out string er)
        {
            er = string.Empty;

            try
            {
                bool alarm = false;

                string e = string.Empty;

                for (int i =_startAddr; i <=_endAddr; i++)
                {
                     if (_dispose)
                        return true;

                    if (_Mon[i].Base.status == ESTATUS.禁用) 
                        continue;

                    if (!ReadModuleData(i, out er))
                    {
                        e += er;
                        alarm = true;
                        _Mon[i].Base.conStatus = false;
                    }
                    else
                    {
                        _Mon[i].Base.conStatus = true;
                    }

                    if (!_Mon[i].Base.conStatus)
                        continue;

                    ReadModuelPara(i);
                }

                if (alarm)
                {
                    er = e;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 写ERS信号
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool WriteData(out string er)
        {
            er = string.Empty;

            try
            {
                for (int i = _startAddr; i <= _endAddr; i++)
                {
                    if (_dispose)
                        return true;

                    if (!_Mon[i].Base.conStatus)
                        continue;

                    SetModuelPara(i);

                    PowerOff(i);

                }

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        #endregion

        #region 读方法
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="status"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Request_WriteMonStatus(int addr, ESTATUS status, out string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = "[" + addr.ToString("D2") + "]" + CLanguage.Lan("不存在");
                    return false;
                }

                _Mon[addr].Base.status = status;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 读取输入电压及电流
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool ReadModuleData(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                CModule module = null;

                Thread.Sleep(_delayMs);

                if (!_devMon.ReadModuleData(addr, out module, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadModuleData(addr, out module, out er))
                    {
                        er = "地址【" + addr.ToString("D2") + "】;";
                        return false;
                    }
                }

                _Mon[addr].Para.Module = module.Clone();

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
       /// <summary>
        /// 读取设置参数
       /// </summary>
       /// <param name="addr"></param>
       /// <param name="er"></param>
       /// <param name="bContinue">失败持续发送</param>
       /// <returns></returns>
        private void ReadModuelPara(int addr)
        {
            string er = string.Empty;

            try
            {
                if (_Mon[addr].ReadPara.Op != EOP.读取)
                    return;

                Thread.Sleep(_delayMs);

                double dcv = 0;

                double dci = 0;

                if (!_devMon.ReadModuelPara(addr, out dcv,out dci, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadModuelPara(addr, out dcv, out dci, out er))
                    {
                        if (!_Mon[addr].ReadPara.bContinue)
                        {
                            _Mon[addr].ReadPara.Op = EOP.读取NG;
                        }

                        OnOpArgs.OnEvented(new COPArgs(addr,EOPTYPE.读取输出参数,false)); 

                        return;
                    }
                }

                _Mon[addr].ReadPara.Volt = dcv;

                _Mon[addr].ReadPara.Current = dci;

                _Mon[addr].ReadPara.Op = EOP.读取OK;

                OnOpArgs.OnEvented(new COPArgs(addr, EOPTYPE.读取输出参数, true)); 

            }
            catch (Exception ex)
            {
                OnOpArgs.OnEvented(new COPArgs(addr, EOPTYPE.读取输出参数, false,ex.ToString())); 
            }
        }
        #endregion

        #region 写方法
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private void SetModuelPara(int addr)
        {
            string er = string.Empty;

            try
            {
                if (_Mon[addr].WritePara.Op != EOP.写入)
                    return;

                Thread.Sleep(_delayMs);

                if (!_devMon.SetModuelPara(addr, _Mon[addr].WritePara.Volt, _Mon[addr].WritePara.Current, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.SetModuelPara(addr, _Mon[addr].WritePara.Volt, _Mon[addr].WritePara.Current, out er))
                    {
                        if (!_Mon[addr].WritePara.bContinue)
                        {
                            _Mon[addr].WritePara.Op = EOP.写入NG;
                        }

                        er = "地址【" + addr.ToString("D2") + "】设置参数";

                        OnOpArgs.OnEvented(new COPArgs(addr, EOPTYPE.设置输出参数, false));

                        return;
                    }
                }

                _Mon[addr].WritePara.Op = EOP.写入OK;

                OnOpArgs.OnEvented(new COPArgs(addr, EOPTYPE.设置输出参数, true));

            }
            catch (Exception ex)
            {
                OnOpArgs.OnEvented(new COPArgs(addr, EOPTYPE.设置输出参数, false,ex.ToString()));
            }
        }
        /// <summary>
        /// 开关机
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private void PowerOff(int addr)
        {
            string er = string.Empty;

            try
            {
                if (_Mon[addr].PowerOff.Op != EOP.写入)
                    return;

                Thread.Sleep(_delayMs);

                if (!_devMon.PowerOff(addr, _Mon[addr].PowerOff.PowerOff, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.PowerOff(addr, _Mon[addr].PowerOff.PowerOff, out er))
                    {
                        if (!_Mon[addr].PowerOff.bContinue)
                        {
                            _Mon[addr].PowerOff.Op = EOP.写入NG;
                        }

                        OnOpArgs.OnEvented(new COPArgs(addr, EOPTYPE.设置开关机, false));
                    }
                }

                _Mon[addr].PowerOff.Op = EOP.写入OK;

                OnOpArgs.OnEvented(new COPArgs(addr, EOPTYPE.设置开关机, true));
            }
            catch (Exception ex)
            {
                OnOpArgs.OnEvented(new COPArgs(addr, EOPTYPE.设置开关机, false,ex.ToString()));
            }
        }
        #endregion

        #region 读取操作类
        /// <summary>
        /// 请求读取参数
       /// </summary>
       /// <param name="addr"></param>
       /// <param name="er"></param>
       /// <param name="bContinue">失败持续</param>
       /// <returns></returns>
        public bool Request_ReadPara(int addr, out string er, bool bContinue = true)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = CLanguage.Lan("地址") + "[" + addr.ToString("D2") + "]" + CLanguage.Lan("不存在");
                    return false;
                }

                _Mon[addr].ReadPara.bContinue = bContinue;

                _Mon[addr].ReadPara.Op = EOP.读取;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        #endregion

        #region 写操作类
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="dcv"></param>
        /// <param name="dci"></param>
        /// <param name="er"></param>
        /// <param name="bContinue"></param>
        /// <returns></returns>
        public bool Request_WritePara(int addr, double dcv, double dci, out string er, bool bContinue = true)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = CLanguage.Lan("地址") + "[" + addr.ToString("D2") + "]" + CLanguage.Lan("不存在");
                    return false;
                }

                _Mon[addr].WritePara.Volt = dcv;

                _Mon[addr].WritePara.Current = dci;

                _Mon[addr].WritePara.bContinue = bContinue;

                _Mon[addr].WritePara.Op = EOP.写入;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 设置开关机：1->关机 0->开机
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="powerOff"></param>
        /// <param name="er"></param>
        /// <param name="bContinue"></param>
        /// <returns></returns>
        public bool Request_PowerOff(int addr, int powerOff, out string er, bool bContinue = true)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = CLanguage.Lan("地址") + "[" + addr.ToString("D2") + "]" + CLanguage.Lan("不存在");
                    return false;
                }

                _Mon[addr].PowerOff.PowerOff = powerOff;

                _Mon[addr].PowerOff.bContinue = bContinue;

                _Mon[addr].PowerOff.Op = EOP.写入;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        #endregion
    }
}
