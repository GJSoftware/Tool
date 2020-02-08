using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GJ.COM;
using System.Diagnostics;

namespace GJ.DEV.LED
{
    public class CLEDThread
    {
        #region 构造函数
        public CLEDThread(int idNo, string name, int startAddr, int endAddr, bool autoMode = false)
        {
            this._idNo = idNo;

            this._name = name;

            this._startAddr = startAddr;

            this._endAddr = endAddr;

            this._autoMode = autoMode;

            for (int i = startAddr; i <= endAddr; i++)
            {
                CLED ers = new CLED();

                ers.Base.addr = i;

                ers.Base.name = _name + "-【" + CLanguage.Lan("DA模块") + i.ToString("D2") + "】";

                ers.Base.status = ESTATUS.运行;

                ers.Base.conStatus = true;

                _Mon.Add(i, ers); 
            }
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
        private int _endAddr = 40;
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
        private CLEDCom _devMon = null;
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
        public Dictionary<int, CLED> _Mon = new Dictionary<int, CLED>();
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

        #region 线程
        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="plc"></param>
        public void SpinUp(CLEDCom devMon, bool autoMode = false)
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
                OnStatusArgs.OnEvented(new CConArgs(CLanguage.Lan("创建监控线程") + _threadMon.Name));
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
                        GJ.COM.CTimer.DelayMs(5);
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

                    //if (_iniOK)
                    //{
                    //    if (!writeData(out er))
                    //        OnDataArgs.OnEvented(new CDataArgs(er, false, true));
                    //}

                    //if (!readData(out er))
                    //    OnDataArgs.OnEvented(new CDataArgs(er, false, true));

                    if(!readAndWriteData(out er))
                        OnDataArgs.OnEvented(new CDataArgs(er, false, true));

                    _iniOK = true;

                    wather.Stop();

                    if (!_autoMode)
                    {
                        _paused = true;
                        _threadStatus = EThreadStatus.暂停;
                        OnDataArgs.OnEvented(new CDataArgs(CLanguage.Lan("监控扫描时间") + ":" + wather.ElapsedMilliseconds.ToString() + "ms"));
                        continue;
                    }

                    _threadStatus = EThreadStatus.运行;

                }
            }
            catch (Exception ex)
            {
                OnStatusArgs.OnEvented(new CConArgs(_threadMon.Name + CLanguage.Lan("监控线程异常错误") + ":" + ex.ToString(), true));
            }
            finally
            {
                _dispose = false;
                _threadStatus = EThreadStatus.退出;
                OnStatusArgs.OnEvented(new CConArgs(_threadMon.Name + CLanguage.Lan("监控线程销毁退出"), true));
            }
        }
        /// <summary>
        /// 读ERS信号
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readAndWriteData(out string er)
        {
            er = string.Empty;

            bool alarm = false;

            string e = string.Empty;

            try
            {
                for (int i = _startAddr; i <= _endAddr; i++)
                {
                    if (_dispose)
                        return true;

                    if (_Mon[i].Base.status == ESTATUS.禁用)
                    {
                        _Mon[i].Base.conStatus = false;
                        continue;
                    }

                    if (_iniOK)
                    {
                        if (!writeData(out er))
                            OnDataArgs.OnEvented(new CDataArgs(er, false, true));
                    }

                    if (!readLoadData(i, out er))
                    {
                        e += er;
                        alarm = true;
                        _Mon[i].Base.conStatus = false;
                    }
                    else
                    {
                        _Mon[i].Base.conStatus = true;
                    }
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
        /// 读ERS信号
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readData(out string er)
        {
            er = string.Empty;

            bool alarm = false;

            string e = string.Empty;

            try
            {
                for (int i = _startAddr; i <= _endAddr; i++)
                {
                    if (_dispose)
                        return true;

                    if (_Mon[i].Base.status == ESTATUS.禁用)
                    {
                        _Mon[i].Base.conStatus = false;
                        continue;
                    }
                    if (!readLoadData(i, out er))
                    {
                        e += er;
                        alarm = true;
                        _Mon[i].Base.conStatus = false;
                    }
                    else
                    {
                        _Mon[i].Base.conStatus = true;
                    }
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
        private bool writeData(out string er)
        {
            er = string.Empty;

            bool alarm = false;

            string e = string.Empty;

            try
            {
                for (int i = _startAddr; i <= _endAddr; i++)
                {
                    if (_dispose)
                        return true;

                    if (_Mon[i].Base.status == ESTATUS.禁用)
                        continue;

                    //设置负载单通道电流
                    if (_Mon[i].Base.conStatus)
                    {
                        if (!writeCHLoad(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }
                    }
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
        /// 读取负载数据
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readLoadData(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                Thread.Sleep(_delayMs);

                CData loadData = new CData();

                if (!_devMon.ReadLoadValue(addr, ref loadData, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadLoadValue(addr, ref loadData, out er))
                    {
                        er = _devMon.ToString() + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("读电流") + ";";
                        return false;
                    }
                }

                _Mon[addr].Data.inv_status = loadData.inv_status;
                _Mon[addr].Data.alarmInfo = loadData.alarmInfo;
                _Mon[addr].Data.rCmd = loadData.rCmd;
                for (int i = 0; i < _Mon[addr].Data.chan.Count; i++)
                {
                    _Mon[addr].Data.chan[i].ch_status = loadData.chan[i].ch_status;
                    _Mon[addr].Data.chan[i].alarmInfo = loadData.chan[i].alarmInfo;
                    _Mon[addr].Data.chan[i].input_ac = loadData.chan[i].input_ac;
                    _Mon[addr].Data.chan[i].volt = loadData.chan[i].volt;
                    _Mon[addr].Data.chan[i].current = loadData.chan[i].current; 
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
                _syncLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 设置负载单通道电流
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeCHLoad(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                for (int i = 0; i < _Mon[addr].Para.OP_SetLoadVal.Count; i++)
                {
                    if (_Mon[addr].Para.OP_SetLoadVal[i].op == EOP.写入)
                    {
                        Thread.Sleep(_delayMs);

                        //设置负载
                        if (!_devMon.SetLoadValue(addr, i + 1, _Mon[addr].Para.OP_SetLoadVal[i].LoadSeting, _Mon[addr].Para.OP_SetLoadVal[i].SaveEPROM, out er))
                        {
                            Thread.Sleep(_delayMs);

                            if (!_devMon.SetLoadValue(addr, i + 1, _Mon[addr].Para.OP_SetLoadVal[i].LoadSeting, _Mon[addr].Para.OP_SetLoadVal[i].SaveEPROM, out er))
                            {
                                er = _devMon.ToString() + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("设置通道电流") + "["+ (i+1).ToString() +"];";
                                return false;
                            }
                        }

                        Thread.Sleep(_delayMs);

                        //回读负载设置

                        List<CLOAD> loadSettings=null;

                        if (!_devMon.ReadLoadSetting(addr, out loadSettings, out er))
                        {
                            Thread.Sleep(_delayMs);

                            if (!_devMon.ReadLoadSetting(addr, out loadSettings, out er))
                            {
                                er = _devMon.ToString() + "[" + addr.ToString("D2") + "]" + CLanguage.Lan("回读通道电流") + "[" + (i + 1).ToString() + "];";
                                return false;
                            }
                        }

                        //比较设置电流值是否设置成功?
                        if (_Mon[addr].Para.OP_SetLoadVal[i].LoadSeting.load == loadSettings[i].load)
                        {
                            _Mon[addr].Para.OP_SetLoadVal[i].op = EOP.写入OK;
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
                _syncLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 设置DA负载状态
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="status"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SetStatus(int addr, ESTATUS status, out string er)
        {
            er = string.Empty;

            try
            {

                if (!_Mon.ContainsKey(addr))
                {
                    er = CLanguage.Lan("DA负载地址") + "[" + addr.ToString("D2") + CLanguage.Lan("不存在");
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
            finally
            {
            }
        }
        /// <summary>
        /// 设置负载单个通道电流
        /// </summary>
        /// <param name="addr">1-40</param>
        /// <param name="chan">1-4</param>
        /// <param name="loadVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SetCHLoad(int addr, int chan, CLOAD load, out string er, bool saveEPROM = true)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = CLanguage.Lan("DA负载地址") + "[" + addr.ToString("D2") + CLanguage.Lan("不存在");
                    return false;
                }

                _Mon[addr].Para.OP_SetLoadVal[chan - 1].LoadSeting.Mode = load.Mode;

                _Mon[addr].Para.OP_SetLoadVal[chan - 1].LoadSeting.Von = load.Von; 

                _Mon[addr].Para.OP_SetLoadVal[chan - 1].LoadSeting.load = load.load;

                _Mon[addr].Para.OP_SetLoadVal[chan - 1].LoadSeting.mark = load.mark;

                _Mon[addr].Para.OP_SetLoadVal[chan - 1].op = EOP.写入;

                _Mon[addr].Para.OP_SetLoadVal[chan - 1].SaveEPROM = saveEPROM;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
            }
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
        #endregion
    }
}
