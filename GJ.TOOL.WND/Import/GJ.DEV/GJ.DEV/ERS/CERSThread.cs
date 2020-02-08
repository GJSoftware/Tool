using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using GJ.COM;

namespace GJ.DEV.ERS
{
    public class CERSThread
    {
        #region 构造函数
        public CERSThread(int idNo, string name, int startAddr, int endAddr, bool autoMode = false)
        {
            this._idNo = idNo;

            this._name = name;

            this._startAddr = startAddr;

            this._endAddr = endAddr;

            this._autoMode = autoMode;

            for (int i = startAddr; i <= endAddr; i++)
            {
                CERS ers = new CERS();

                ers.Base.addr = i;

                ers.Base.name = _name + "-【" + i.ToString("D2") + "】";

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
        private CERSCom _devMon = null;
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
        public Dictionary<int, CERS> _Mon = new Dictionary<int, CERS>();
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
        #endregion

        #region 线程
        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="plc"></param>
        public void SpinUp(CERSCom devERS, bool autoMode = false)
        {
            this._devMon = devERS;

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
                OnStatusArgs.OnEvented(new CConArgs("创建" + _threadMon.Name + "监控线程"));
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
                        if (!writeData(out er))
                            OnDataArgs.OnEvented(new CDataArgs(er, false, true));
                    }

                    if (!readData(out er))
                        OnDataArgs.OnEvented(new CDataArgs(er, false, true));

                    _iniOK = true;

                    wather.Stop();

                    if (!_autoMode)
                    {
                        _paused = true;
                        _threadStatus = EThreadStatus.暂停;
                        OnDataArgs.OnEvented(new CDataArgs("监控扫描时间:" + wather.ElapsedMilliseconds.ToString() + "ms"));
                        continue;
                    }

                    _threadStatus = EThreadStatus.运行;

                }
            }
            catch (Exception ex)
            {
                OnStatusArgs.OnEvented(new CConArgs(_threadMon.Name + "监控线程异常错误:" + ex.ToString(), true));
            }
            finally
            {
                _dispose = false;
                _threadStatus = EThreadStatus.退出;
                OnStatusArgs.OnEvented(new CConArgs(_threadMon.Name + "监控线程销毁退出", true));
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
                for (int i =_startAddr; i <=_endAddr; i++)
                {
                     if (_dispose)
                        return true;

                     if (_Mon[i].Base.status == ESTATUS.禁用)
                     {
                         _Mon[i].Base.conStatus = false;
                         continue;
                     }

                    if (!readLoadCurrent(i, out er))
                    {
                        e += er;
                        alarm = true;
                        _Mon[i].Base.conStatus = false;
                    }
                    else
                    {
                        _Mon[i].Base.conStatus = true;
                    }

                    if (_Mon[i].Base.conStatus && _Mon[i].Para.ReadLoad.op == EOP.读取)
                    {
                        if (!readLoadSet(i, out er))
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

                    //设置负载全部通道电流
                    if (_Mon[i].Base.conStatus && _Mon[i].Para.SetAllLoad.op == EOP.写入)
                    {
                        if (!writeAllLoad(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }
                    }
                    //设置负载单通道电流
                    if (_Mon[i].Base.conStatus)
                    {
                        if (!writeCHLoad(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }
                    }
                    //设置快充模式
                    if (_Mon[i].Base.conStatus)
                    {
                        if (!writeCHQCM(i, out er))
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
        /// 读取负载电流
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readLoadCurrent(int addr,out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                Thread.Sleep(_delayMs);

                CERS_Load load = null;

                if (!_devMon.ReadData(addr, out load, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadData(addr, out load, out er))
                    {
                        er = _Mon[addr].Base.name + "读电流;";
                        return false;
                    }
                }

                for (int i = 0; i < load.cur.Length; i++)
                {
                    _Mon[addr].Para.Volt[i] = load.volt[i];
                    _Mon[addr].Para.Current[i] = load.cur[i];
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
        /// 读取负载设置
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readLoadSet(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                Thread.Sleep(_delayMs);

                CERS_Load load = null;

                if (!_devMon.ReadLoadSet(addr, out load, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadLoadSet(addr, out load, out er))
                    {
                        er = _Mon[addr].Base.name +"读电流设置;";
                        return false;
                    }
                }

                for (int i = 0; i < load.cur.Length; i++)
                    _Mon[addr].Para.ReadLoad.LoadSet[i] = load.cur[i];

                _Mon[addr].Para.ReadLoad.op = EOP.读取OK;

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
        /// 设置负载所有通道电流
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeAllLoad(int addr, out string er)
        {

            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                CERS_Load load = new CERS_Load();

                for (int i = 0; i < load.cur.Length; i++)
                    load.cur[i]=_Mon[addr].Para.SetAllLoad.LoadSet[i];

                Thread.Sleep(_delayMs);

                if (!_devMon.SetNewLoad(addr, load, out er, _Mon[addr].Para.SetAllLoad.saveEPROM))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.SetNewLoad(addr, load, out er, _Mon[addr].Para.SetAllLoad.saveEPROM))
                    {
                        er = _Mon[addr].Base.name +"设置电流;";
                        return false;
                    }
                }

                _Mon[addr].Para.SetAllLoad.op = EOP.写入OK;

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

                for (int i = 0; i < _Mon[addr].Para.SetCHLoad.Count; i++)
                {
                    if (_Mon[addr].Para.SetCHLoad[i].op == EOP.写入)
                    {
                        //设置电流

                        Thread.Sleep(_delayMs);

                        if (!_devMon.SetNewLoad(addr, i + 1, _Mon[addr].Para.SetCHLoad[i].loadVal, out er, _Mon[addr].Para.SetCHLoad[i].saveEPROM))
                        {
                            Thread.Sleep(_delayMs);

                            if (!_devMon.SetNewLoad(addr, i + 1, _Mon[addr].Para.SetCHLoad[i].loadVal, out er, _Mon[addr].Para.SetCHLoad[i].saveEPROM))
                            {
                                er = _Mon[addr].Base.name +"设置单通道电流;";
                                return false;
                            }
                        }

                        //回读电流

                        Thread.Sleep(_delayMs);

                        CERS_Load LoadSetting=null;

                        if (!_devMon.ReadLoadSet(addr, out LoadSetting, out er))
                        {
                            Thread.Sleep(_delayMs);

                            if (!_devMon.ReadLoadSet(addr, out LoadSetting, out er))
                            {
                                er = _Mon[addr].Base.name + "回读设置电流;";
                                return false;
                            }
                        }

                        if(_Mon[addr].Para.SetCHLoad[i].loadVal==LoadSetting.cur[i]) 
                           _Mon[addr].Para.SetCHLoad[i].op = EOP.写入OK;

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
        /// <summary>
        /// 设置快充电压
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeCHQCM(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                for (int i = 0; i < _Mon[addr].Para.SetQCM.Count; i++)
                {
                    if (_Mon[addr].Para.SetQCM[i].op == EOP.写入)
                    {
                        Thread.Sleep(_delayMs);

                        if (!_devMon.SetQCMTK(addr, i + 1, _Mon[addr].Para.SetQCM[i].raise,out er))
                        {
                            Thread.Sleep(_delayMs);

                            if (!_devMon.SetQCMTK(addr, i + 1, _Mon[addr].Para.SetQCM[i].raise, out er))
                            {
                                er = _Mon[addr].Base.name +"设置快充;";
                                return false;
                            }
                        }

                        _Mon[addr].Para.SetQCM[i].op = EOP.写入OK;
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
        /// 设置控制板状态
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
                    er = "ERS地址[" + addr.ToString("D2") + "不存在";
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
        /// 设置所有负载通道电流
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="load"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public bool SetAllLoad(int addr,CERS_Load load,out string er,bool saveEPROM=true)
        {
        er = string.Empty;

            try
            {

                if (!_Mon.ContainsKey(addr))
                {
                    er = "ERS地址[" + addr.ToString("D2") + "不存在";
                    return false;
                }

                for (int i = 0; i < load.cur.Length; i++)
                    _Mon[addr].Para.SetAllLoad.LoadSet[i] = load.cur[i];

                _Mon[addr].Para.SetAllLoad.saveEPROM = saveEPROM;  

                _Mon[addr].Para.SetAllLoad.op = EOP.写入;  

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
        public bool SetCHLoad(int addr, int chan, double loadVal, out string er,bool saveEPROM=true)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = "ERS地址[" + addr.ToString("D2") + "不存在";
                    return false;
                }

                _Mon[addr].Para.SetCHLoad[chan - 1].loadVal = loadVal;

                _Mon[addr].Para.SetCHLoad[chan - 1].saveEPROM = saveEPROM;

                _Mon[addr].Para.SetCHLoad[chan - 1].op = EOP.写入;  

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
        /// 设置快充电压上升或下降
        /// </summary>
        /// <param name="wAddr"></param>
        /// <param name="CH"></param>
        /// <param name="wRaise"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool setMTKRaise(int addr, int chan, bool raise, ref string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = "ERS地址[" + addr.ToString("D2") + "不存在";
                    return false;
                }

                _Mon[addr].Para.SetQCM[chan - 1].raise = raise;

                _Mon[addr].Para.SetQCM[chan - 1].op = EOP.写入;

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

    }

}
