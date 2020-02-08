using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using GJ.COM;

namespace GJ.DEV.DD
{
    public class CDDThread
    {
        #region 构造函数
        public CDDThread(int idNo, string name, int startAddr, int endAddr, bool autoMode = false)
        {
            this._idNo = idNo;

            this._name = name;

            this._startAddr = startAddr;

            this._endAddr = endAddr;

            this._autoMode = autoMode;

            for (int i = startAddr; i <= endAddr; i++)
            {
                CDD mon = new CDD();

                mon.Base.addr = i;

                mon.Base.name = _name + "-【DD模块" + i.ToString("D2") + "】";

                mon.Base.status = ESTATUS.运行;

                mon.Base.conStatus = true;

                _Mon.Add(i, mon); 
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
        private volatile int _delayMs = 5;
        /// <summary>
        /// 线程状态
        /// </summary>
        private volatile EThreadStatus _threadStatus = EThreadStatus.空闲;
        /// <summary>
        /// 通信设备
        /// </summary>
        private CDDCom _devMon = null;
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
        public Dictionary<int, CDD> _Mon = new Dictionary<int, CDD>();
        /// <summary>
        /// 设置EPROM
        /// </summary>
        private int _saveEPROM = 1;
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
        public void SpinUp(CDDCom devMon,int saveEPROM=1, bool autoMode = false)
        {
            this._devMon = devMon;

            this.autoMode = autoMode;

            this._saveEPROM = saveEPROM;  

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

                    //快速写入操作
                    if (!readAndWriteData(out er))
                        OnDataArgs.OnEvented(new CDataArgs(er, false, true));

                    //if (_iniOK)
                    //{
                    //    if (!writeData(out er))
                    //        OnDataArgs.OnEvented(new CDataArgs(er, false, true));
                    //}
                    
                    //if (!readData(out er))
                    //    OnDataArgs.OnEvented(new CDataArgs(er, false, true));

                    _iniOK = true;

                    wather.Stop();

                    if (!_autoMode)
                    {
                        _paused = true;
                        _threadStatus = EThreadStatus.暂停;
                        OnDataArgs.OnEvented(new CDataArgs(name + "监控扫描时间:" + wather.ElapsedMilliseconds.ToString() + "ms"));
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

                    if (_iniOK)
                    {
                        if (!writeData(out er))
                            OnDataArgs.OnEvented(new CDataArgs(er, false, true));
                    }

                    if (_Mon[i].Base.status == ESTATUS.禁用)
                        continue;

                    if (!readModuleData(i, out er))
                    {
                        e += er;
                        alarm = true;
                        _Mon[i].Base.conStatus = false;
                    }
                    else
                    {
                        _Mon[i].Base.conStatus = true;
                    }

                    if (_Mon[i].Base.conStatus && _Mon[i].Para.rLoad_OP == EOP.读取)
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
                        continue;

                    if (!readModuleData(i , out er))
                    {
                        e += er;
                        alarm = true;
                        _Mon[i].Base.conStatus = false;
                    }
                    else
                    {
                        _Mon[i].Base.conStatus = true;
                    }

                    if (_Mon[i].Base.conStatus && _Mon[i].Para.rLoad_OP == EOP.读取)
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

                    //设置负载通道电流
                    if (_Mon[i].Base.conStatus && _Mon[i].Para.wLoad_OP == EOP.写入)
                    {
                        if (!writeLoadSet(i, out er))
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
        /// 读取数据
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readModuleData(int addr,out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                Thread.Sleep(_delayMs);

                CrData para = new CrData();

                if (!_devMon.ReadData(addr, ref para, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadData(addr, ref para, out er))
                    {
                        er = _Mon[addr].ToString() + "读数据;";
                        return false;
                    }
                }

                _Mon[addr].Para.Data = para.Clone();

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

                CrLoad para = new CrLoad();

                if (!_devMon.ReadLoadSet(addr, ref para, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadLoadSet(addr, ref para, out er))
                    {
                        er = _Mon[addr].ToString() + "读数据;";
                        return false;
                    }
                }

                _Mon[addr].Para.LoadRead = para.Clone();

                _Mon[addr].Para.rLoad_OP = EOP.读取OK;  

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
        /// 负载设置
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeLoadSet(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                Thread.Sleep(_delayMs);

                if (!_devMon.SetNewLoad(addr, _Mon[addr].Para.LoadSet, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.SetNewLoad(addr, _Mon[addr].Para.LoadSet, out er))
                    {
                        er = _Mon[addr].ToString() + "设负载;";
                        return false;
                    }
                }

                _Mon[addr].Para.wLoad_OP = EOP.写入OK;

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
                    er = "DD地址[" + addr.ToString("D2") + "不存在";
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
        public bool SetLoad(int addr,CwLoad para,out string er)
        {
            er = string.Empty;

            try
            {

                if (!_Mon.ContainsKey(addr))
                {
                    er = "DD模块地址[" + addr.ToString("D2") + "不存在";
                    return false;
                }

                _Mon[addr].Para.LoadSet = para.Clone();

                _Mon[addr].Para.LoadSet.saveEEPROM = _saveEPROM;

                _Mon[addr].Para.wLoad_OP = EOP.写入;   

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
        public bool ReadLoad(int addr,out string er)
        {
            er = string.Empty;

            try
            {

                if (!_Mon.ContainsKey(addr))
                {
                    er = "DD模块地址[" + addr.ToString("D2") + "不存在";
                    return false;
                }

                _Mon[addr].Para.rLoad_OP = EOP.读取;

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
