using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using GJ.COM;

namespace GJ.DEV.FCMB
{
    public class CFMBThread
    {
        #region 构造函数
        public CFMBThread(int idNo, string name, int startAddr, int endAddr, bool autoMode = false,bool synC=true, EVMODE voltMode=EVMODE.VOLT_40)
        {
            this._idNo = idNo;

            this._name = name;

            this._startAddr = startAddr;

            this._endAddr = endAddr;

            this._autoMode = autoMode;

            this._synC = synC;

            this._voltMode = voltMode; 

            for (int i = startAddr; i <= endAddr; i++)
            {
                CFMB fmb = new CFMB();

                fmb.Base.addr = i;

                fmb.Base.name = _name + "-【" + CLanguage.Lan("快充板") + i.ToString("D2") + "】";

                fmb.Base.status = ESTATUS.运行;

                fmb.Base.conStatus = true;

                _Mon.Add(i, fmb);   

            }
        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 字段
        /// <summary>
        /// 编号
        /// </summary>
        private int _idNo = 0;
        /// <summary>
        /// 名称
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
        /// 同步模式
        /// </summary>
        private bool _synC = true;
        /// <summary>
        /// 采集电压通道
        /// </summary>
        private EVMODE _voltMode = EVMODE.VOLT_40; 
        /// <summary>
        /// 延时时间
        /// </summary>
        private volatile int _delayMs = 5;
        /// <summary>
        /// 自动模式:不间断扫描;手动模式:需手动启动扫描
        /// </summary>
        private volatile bool _autoMode = true;
        /// <summary>
        /// 暂停监控
        /// </summary>
        private volatile bool _paused = false;
        /// <summary>
        /// 退出线程
        /// </summary>
        private volatile bool _dispose = false;
        /// <summary>
        /// 线程状态
        /// </summary>
        private volatile EThreadStatus _threadStatus = EThreadStatus.空闲;
        /// <summary>
        /// 通信设备
        /// </summary>
        private CFMBCom _devFMB = null;
        /// <summary>
        /// 监控线程
        /// </summary>
        private Thread _threadFMB = null;
        /// <summary>
        /// 线程同步锁
        /// </summary>
        private ReaderWriterLock _syncLock = new ReaderWriterLock();
        /// <summary>
        /// 快充板基本信息:【地址->信息】
        /// </summary>
        public Dictionary<int, CFMB> _Mon = new Dictionary<int, CFMB>();
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
        /// <summary>
        /// 初始化标志
        /// </summary>
        public bool iniOK
        {
            set { _iniOK = value; }
            get { return _iniOK; }
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
        public void SpinUp(CFMBCom devFMB, bool autoMode = false)
        {
            this._devFMB = devFMB;

            this.autoMode = autoMode; 

            if (_threadFMB == null)
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
                _threadFMB = new Thread(OnStart);
                _threadFMB.Name = _name;
                _threadFMB.IsBackground = true;
                _threadFMB.Start();
                OnStatusArgs.OnEvented(new CConArgs(CLanguage.Lan("创建监控线程") + _threadFMB.Name));
            }
        }
        /// <summary>
        /// 销毁线程
        /// </summary>
        public void SpinDown()
        {
            try
            {
                if (_threadFMB != null)
                {
                    _dispose = true;
                    do
                    {
                        if (!_threadFMB.IsAlive)
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
                _threadFMB = null;
            }
        }
        /// <summary>
        /// 暂停线程
        /// </summary>
        public void Paused()
        {
            if (!_autoMode && _threadFMB != null)
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
            if (!_autoMode && _threadFMB != null)
            {
                _paused = false;
                _threadStatus = EThreadStatus.运行;
            }
        }
        /// <summary>
        /// 线程方法
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
                    
                    //初始化读取控制板状态
  
                    if(!readInitial(out er))
                       OnDataArgs.OnEvented(new CDataArgs(er, false, true));

                    //快速写入操作
                    if(!readAndWriteData(out er))
                        OnDataArgs.OnEvented(new CDataArgs(er, false, true));

                    ////设置快充板信号
                    //if (!writeData(out er))
                    //    OnDataArgs.OnEvented(new CDataArgs(er, false, true));

                    ////读取快充板信号及电压值
                    //if (!readData(out er))
                    //    OnDataArgs.OnEvented(new CDataArgs(er, false, true));

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
                OnStatusArgs.OnEvented(new CConArgs(_threadFMB.Name + CLanguage.Lan("监控线程异常错误") + ":" + ex.ToString(), true));
            }
            finally
            {                
                _dispose = false;
                _threadStatus = EThreadStatus.退出;
                OnStatusArgs.OnEvented(new CConArgs(_threadFMB.Name + CLanguage.Lan("监控线程销毁退出"), true));
            }
        }
        /// <summary>
        /// 读取初始状态
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readInitial(out string er)
        {
            er = string.Empty;

            try
            {
                if (_iniOK)
                    return true;

                string e = string.Empty;

                bool alarm = false;

                for (int i = _startAddr; i <= _endAddr; i++)
                {
                    if (_dispose)
                        return true;

                    if (_Mon[i].Base.status == ESTATUS.禁用)
                        continue;

                    if (!readVersion(i, out er))
                    {
                        e += er;
                        alarm = true;
                        _Mon[i].Base.conStatus = false;
                        continue;
                    }

                    Thread.Sleep(10);  

                    if(!SetIO(i,EFMB_wIO.错误信号灯,0,out er))
                    {
                        e += er;
                        alarm = true;
                        _Mon[i].Base.conStatus = false;
                        continue;
                    }

                    //if (!readName(i, out er))
                    //{
                    //    e += er;
                    //    alarm = true;
                    //    _Mon[i].Base.conStatus = false;
                    //    continue;
                    //}

                    //if (!readSn(i, out er))
                    //{                        
                    //    e += er;
                    //    alarm = true;
                    //    _Mon[i].Base.conStatus = false;
                    //    continue;
                    //}

                    //if (!readChildVersion(i, out er))
                    //{
                    //    e += er;
                    //    alarm = true;
                    //    _Mon[i].Base.conStatus = false;
                    //    continue;
                    //}
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
        /// 在扫描过程中优先写数据
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readAndWriteData(out string er)
        {
            er = string.Empty;

            try
            {
                string e = string.Empty;

                bool alarm = false;

                for (int i = _startAddr; i <= _endAddr; i++)
                {
                    if (_dispose)
                        return true;

                    if (!writeData(out er))
                    {
                        OnDataArgs.OnEvented(new CDataArgs(er, false, true));
                    }

                    if (_Mon[i].Base.status == ESTATUS.禁用) //不读取数据
                    {
                        _Mon[i].Base.conStatus = false;
                        continue;
                    }
                    else if (_Mon[i].Base.status == ESTATUS.运行 || !_iniOK) //读取信号和电压值
                    {
                        if (!readSignal(i, out er))
                        {
                            e += er;
                            alarm = true;
                            _Mon[i].Base.conStatus = false;
                        }
                        else
                        {
                            _Mon[i].Base.conStatus = true;
                        }
                        if (_Mon[i].Base.conStatus)
                        {
                            if (!readVolt(i, out er))
                            {
                                e += er;
                                alarm = true;
                                _Mon[i].Base.conStatus = false;
                            }
                        }
                    }
                    else if (_Mon[i].Base.status == ESTATUS.空闲) //只读取信号
                    {
                        if (!readSignal(i, out er))
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

                    //读取快充模式
                    if (!_Mon[i].Base.conStatus && (_Mon[i].Para.QCM.op == EOP.读取 || !_iniOK))
                    {
                        if (!readQCM(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }
                    }
                    //读取ONOFF时序 
                    if (_Mon[i].Base.conStatus && (_Mon[i].Para.OnOff.op == EOP.读取 || !_iniOK))
                    {
                        if (!readTimer(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }
                    }
                    //读取AC ON/OFF
                    if (_Mon[i].Base.conStatus && (_Mon[i].Para.ACON.op == EOP.读取 || !_iniOK))
                    {
                        if (!readACON(i, out er))
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
        /// 写寄存器
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeData(out string er)
        {
            er = string.Empty;

            try
            {
                string e = string.Empty;

                bool alarm = false;

                for (int i = _startAddr; i <= _endAddr; i++)
                {
                    if (_dispose)
                        return true;

                    if (_Mon[i].Base.status == ESTATUS.禁用)
                        continue;

                    //写入快充模式--->防止通信写入中控版异常-->需要设置2次
                    if (_Mon[i].Base.conStatus && _Mon[i].Para.QCM.op == EOP.写入)
                    {
                        if (!writeQCM(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }

                        Thread.Sleep(100);

                        if (!writeQCM(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }
                    }

                    //写入ONOFF时序 
                    if (_Mon[i].Base.conStatus && _Mon[i].Para.OnOff.op == EOP.写入)
                    {
                        if (!writeTimer(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }
                    }
                    
                    //写入AC ON/OFF
                    if (_Mon[i].Base.conStatus && _Mon[i].Para.ACON.op == EOP.写入)
                    {
                        if (!writeACON(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }
                    }
                    
                    //写入IO信号
                    if (_Mon[i].Base.conStatus)
                    {
                        if (!writeIO(i, out er))
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
        /// 读寄存器
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readData(out string er)
        {
            er = string.Empty;

            try
            {
                string e = string.Empty;

                bool alarm = false;

                for (int i = _startAddr; i <= _endAddr; i++)
                {
                    if (_dispose)
                        return true;

                    if (_Mon[i].Base.status == ESTATUS.禁用) //不读取数据
                    {
                        _Mon[i].Base.conStatus = false;
                        continue;
                    }
                    else if (_Mon[i].Base.status == ESTATUS.运行 || !_iniOK) //读取信号和电压值
                    {
                        if (!readSignal(i, out er))
                        {
                            e += er;
                            alarm = true;
                            _Mon[i].Base.conStatus = false;
                        }
                        else
                        {
                            _Mon[i].Base.conStatus = true;
                        }

                        if (_Mon[i].Base.conStatus)
                        {
                            if (!readVolt(i, out er))
                            {
                                e += er;
                                alarm = true;
                                _Mon[i].Base.conStatus = false;
                            }
                        }
                    }
                    else if (_Mon[i].Base.status == ESTATUS.空闲) //只读取信号
                    {
                        if (!readSignal(i, out er))
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
                   
                    //读取快充模式
                    if (!_Mon[i].Base.conStatus && (_Mon[i].Para.QCM.op == EOP.读取 || !_iniOK))
                    {
                        if (!readQCM(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }
                    }
                    //读取ONOFF时序 
                    if (_Mon[i].Base.conStatus && (_Mon[i].Para.OnOff.op == EOP.读取 || !_iniOK))
                    {
                        if (!readTimer(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }
                    }
                    //读取AC ON/OFF
                    if (_Mon[i].Base.conStatus && (_Mon[i].Para.ACON.op == EOP.读取 || !_iniOK))
                    { 
                        if(!readACON(i,out er))
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
        /// 读取设备名称
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readName(int addr,out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                Thread.Sleep(_delayMs);

                string devName = string.Empty;

                if (!_devFMB.ReadName(addr, out devName, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.ReadName(addr, out devName, out er))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("设备名称") + ";";
                        return false;
                    }
                }

                _Mon[addr].Base.devName = devName;

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
        /// 读取设备Sn
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readSn(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                Thread.Sleep(_delayMs);

                string Sn = string.Empty;

                if (!_devFMB.ReadSn(addr, out Sn, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.ReadSn(addr, out Sn, out er))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("设备SN") + ";";
                        return false;
                    }
                }
                
                _Mon[addr].Base.devSn = Sn;

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
        /// 读取设备版本
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readVersion(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                Thread.Sleep(_delayMs);

                string devVer = string.Empty;

                if (!_devFMB.ReadVersion(addr, out devVer, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.ReadVersion(addr, out devVer, out er))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("设备版本") + ";";
                        return false;
                    }
                }

                _Mon[addr].Base.devVer = devVer; 

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
        /// 读取设备小板版本
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readChildVersion(int addr,int childMax, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                List<int> childVer = null;

                Thread.Sleep(_delayMs);

                if (!_devFMB.ReadChildVersion(addr,childMax, out childVer, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.ReadChildVersion(addr, childMax,out childVer, out er))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("设备小板") + ";";
                        return false;
                    }
                }

                for (int i = 0; i < childVer.Count; i++)
                    _Mon[addr].Base.childVer[i] = ((double)childVer[i] / 10).ToString("0.0"); 
    
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
        /// 读取快充板信号状态
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readSignal(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                List<int> io = null;

                Thread.Sleep(_delayMs);

                if (!_devFMB.ReadIO(addr, out io, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.ReadIO(addr, out io, out er))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("读IO") + ";";
                        return false;
                    }
                }

                for (int i = 0; i < io.Count; i++)
                    _Mon[addr].Para.rIO[(EFMB_rIO)i] = io[i];

                Thread.Sleep(_delayMs);

                double acv=0;

                if (!_devFMB.ReadAC(addr, out acv, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.ReadAC(addr, out acv, out er))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("读电压") + "AC;";
                        return false;
                    }
                }

                _Mon[addr].Para.CurACVolt = acv; 

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
        /// 读取快充板电压值
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readVolt(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                Thread.Sleep(_delayMs);

                List<double> Volt = null;

                if (!_devFMB.ReadVolt(addr, out Volt, out er, _synC, _voltMode))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.ReadVolt(addr, out Volt, out er, _synC, _voltMode))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("读电压");
                        return false;
                    }
                }

                for (int i = 0; i < Volt.Count; i++)                
                    _Mon[addr].Para.Volt[i] = Volt[i];  
                
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
        /// 写入快充模式
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeQCM(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                Thread.Sleep(_delayMs);

                if (!_devFMB.SetQCM(addr, _Mon[addr].Para.QCM.qcm, _Mon[addr].Para.QCM.qcv, _Mon[addr].Para.QCM.qci, out er, _Mon[addr].Para.QCM.cc2))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.SetQCM(addr, _Mon[addr].Para.QCM.qcm, _Mon[addr].Para.QCM.qcv, _Mon[addr].Para.QCM.qci, out er, _Mon[addr].Para.QCM.cc2))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("写快充模式") + ";";
                        return false;
                    }
                }

                _Mon[addr].Para.QCM.op = EOP.写入OK;

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
        /// 读取快充模式
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readQCM(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                EQCM qcm = EQCM.Normal;

                double qcv = 0;

                double qci = 0;

                Thread.Sleep(_delayMs);

                if (!_devFMB.ReadQCM(addr, out qcm, out qcv, out qci,out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.ReadQCM(addr, out qcm, out qcv, out qci, out er))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("读快充模式") + ";";
                        return false;
                    }
                }

                _Mon[addr].Para.QCM.qcm = qcm;

                _Mon[addr].Para.QCM.qcv = qcv;

                _Mon[addr].Para.QCM.qci = qci;

                _Mon[addr].Para.QCM.op = EOP.读取OK; 

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
        /// 写入ONOFF时序
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeTimer(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                Thread.Sleep(_delayMs);

                if (!_devFMB.SetOnOffMode(addr, _Mon[addr].Para.OnOff.mode, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.SetOnOffMode(addr, _Mon[addr].Para.OnOff.mode, out er))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("写ONOFF模式") + ";";
                        return false;
                    }
                }

                Thread.Sleep(_delayMs);

                if (!_devFMB.SetTotalTime(addr, _Mon[addr].Para.OnOff.TotalTime/60, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.SetTotalTime(addr, _Mon[addr].Para.OnOff.TotalTime / 60, out er))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("写总时间") + ";";
                        return false;
                    }
                }

                Thread.Sleep(_delayMs);

                if (!_devFMB.SetOnOffTime(addr, _Mon[addr].Para.OnOff.OnOff, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.SetOnOffTime(addr, _Mon[addr].Para.OnOff.OnOff, out er))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("写时序时间") + ";";
                        return false;
                    }
                }

                _Mon[addr].Para.OnOff.op = EOP.写入OK;

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
        /// 读取ONOFF时序
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readTimer(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                EOnOffMode mode = EOnOffMode.上位机控制; 

                Thread.Sleep(_delayMs);

                if (!_devFMB.ReadOnOffMode(addr, out mode, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.ReadOnOffMode(addr, out mode, out er))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("读ONOFF模式");
                        return false;
                    }
                }

                int runMin = 0;

                Thread.Sleep(_delayMs);

                if (!_devFMB.ReadTotalTime(addr, out runMin, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.ReadTotalTime(addr, out runMin, out er))
                    {
                        er = _Mon[addr].ToString() + "读总时间;";
                        return false;
                    }
                }

                List<COnOff> OnOff = null;

                Thread.Sleep(_delayMs);

                if (!_devFMB.ReadOnOffTime(addr, out OnOff, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.ReadOnOffTime(addr, out OnOff, out er))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("读时序时间") + ";";
                        return false;
                    }
                }

                _Mon[addr].Para.OnOff.mode = mode;

                _Mon[addr].Para.OnOff.TotalTime = runMin * 60;

                for (int i = 0; i < OnOff.Count; i++)
                {
                    _Mon[addr].Para.OnOff.OnOff[i].OnOffTime = OnOff[i].OnOffTime;
                    _Mon[addr].Para.OnOff.OnOff[i].OnTime = OnOff[i].OnTime;
                    _Mon[addr].Para.OnOff.OnOff[i].OffTime = OnOff[i].OffTime;  
                }

                _Mon[addr].Para.OnOff.op = EOP.读取OK;  

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
        /// 写入AC ON/OFF
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeACON(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                Thread.Sleep(_delayMs);

                if (!_devFMB.SetACON(addr, _Mon[addr].Para.ACON.wOnOff, out er, _Mon[addr].Para.ACON.synC,_Mon[addr].Para.ACON.B400))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.SetACON(addr, _Mon[addr].Para.ACON.wOnOff, out er, _Mon[addr].Para.ACON.synC, _Mon[addr].Para.ACON.B400))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("写ACON") + ";";
                        return false;
                    }
                }

                _Mon[addr].Para.ACON.op = EOP.写入OK;

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
        /// 读取AC ON/OFF
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readACON(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                int rOnOff = 0;

                Thread.Sleep(_delayMs);

                if (!_devFMB.ReadACON(addr, out rOnOff, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.ReadACON(addr, out rOnOff, out er))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("读ACON") + ";";
                        return false;
                    }
                }

                double acv = 0;

                if (!_devFMB.ReadAC(addr, out acv, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devFMB.ReadAC(addr, out acv, out er))
                    {
                        er = _Mon[addr].ToString() + CLanguage.Lan("读电压") + "AC;";
                        return false;
                    }
                }

                _Mon[addr].Para.ACON.wOnOff = rOnOff;

                _Mon[addr].Para.ACON.acVolt = acv; 

                _Mon[addr].Para.QCM.op = EOP.读取OK;

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
        /// 写入IO信号
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeIO(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                foreach (EFMB_wIO ioType in _Mon[addr].Para.wIO.Keys)
                {
                    if (_Mon[addr].Para.wIO[ioType].op == EOP.写入)
                    {
                        Thread.Sleep(_delayMs);

                        if (!_devFMB.SetIO(addr, ioType, _Mon[addr].Para.wIO[ioType].ioVal, out er))
                        {
                            Thread.Sleep(_delayMs);

                            if (!_devFMB.SetIO(addr, ioType, _Mon[addr].Para.wIO[ioType].ioVal, out er))
                            {
                                er = _Mon[addr].ToString() + CLanguage.Lan("写信号") + "["+ ioType.ToString() +"];";
                                return false;
                            }
                        }
                        _Mon[addr].Para.wIO[ioType].op = EOP.写入OK;                    
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
                    er = CLanguage.Lan("快充板地址")+ "[" + addr.ToString("D2") + "]" + CLanguage.Lan("不存在");
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
        /// 设置快充板状态
        /// </summary>
        /// <param name="startAddr"></param>
        /// <param name="status"></param>
        public bool SetFMBStatus(int startAddr,List<ESTATUS> status,out string er)
        {
            er = string.Empty;

            try
            {
                for (int i = 0; i < status.Count; i++)
                {
                    int addr = startAddr + i;

                    if (_Mon.ContainsKey(addr))
                    {
                        _Mon[addr].Base.status = status[i];
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

            }            
        }
        /// <summary>
        /// 读取和设置快充模式
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="qcm"></param>
        /// <returns></returns>
        public bool SetFMBQCM(int addr, CFMB_QCM qcm,out string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = CLanguage.Lan("快充板地址") + "[" + addr.ToString("D2") + "]" + CLanguage.Lan("不存在");
                    return false;
                }

                _Mon[addr].Para.QCM.qcm = qcm.qcm;

                _Mon[addr].Para.QCM.qcv = qcm.qcv;

                _Mon[addr].Para.QCM.qci = qcm.qci;

                _Mon[addr].Para.QCM.op = qcm.op;

                _Mon[addr].Para.QCM.cc2 = qcm.cc2;

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
        /// 读取或设置ONOFF时序
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="onoff"></param>
        /// <returns></returns>
        public bool SetFMBTimer(int addr, CFMB_ONOFF onoff,out string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = CLanguage.Lan("快充板地址") + "[" + addr.ToString("D2") + "]" + CLanguage.Lan("不存在");
                    return false;
                }

                _Mon[addr].Para.OnOff.mode = onoff.mode;

                _Mon[addr].Para.OnOff.TotalTime = onoff.TotalTime;

                for (int i = 0; i <  _Mon[addr].Para.OnOff.OnOff.Count; i++)
                {
                    _Mon[addr].Para.OnOff.OnOff[i].OnOffTime = 0;
                    _Mon[addr].Para.OnOff.OnOff[i].OnTime = 0;
                    _Mon[addr].Para.OnOff.OnOff[i].OffTime = 0;
                }

                for (int i = 0; i < onoff.OnOff.Count; i++)
                {
                    if (i < _Mon[addr].Para.OnOff.OnOff.Count)
                    {
                        _Mon[addr].Para.OnOff.OnOff[i].OnOffTime = onoff.OnOff[i].OnOffTime;
                        _Mon[addr].Para.OnOff.OnOff[i].OnTime = onoff.OnOff[i].OnTime;
                        _Mon[addr].Para.OnOff.OnOff[i].OffTime = onoff.OnOff[i].OffTime;
                    }
                }

                _Mon[addr].Para.OnOff.op = onoff.op;  

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
        /// 读取或设置AC ON/OFF
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="acOnOff"></param>
        /// <returns></returns>
        public bool SetACONOFF(int addr, CFMB_ACON acOnOff,out string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = CLanguage.Lan("快充板地址") + "[" + addr.ToString("D2") + "]" + CLanguage.Lan("不存在");
                    return false;
                }

                _Mon[addr].Para.ACON.wOnOff = acOnOff.wOnOff;

                _Mon[addr].Para.ACON.synC = acOnOff.synC; 

                _Mon[addr].Para.ACON.op = acOnOff.op;

                _Mon[addr].Para.ACON.B400 = acOnOff.B400;  
                
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
        /// 设置IO控制信号
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="ioType"></param>
        /// <param name="io"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SetIO(int addr, EFMB_wIO ioType, int ioVal, out string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = CLanguage.Lan("快充板地址") + "[" + addr.ToString("D2") + "]" + CLanguage.Lan("不存在");
                    return false;
                }

                if (!_Mon[addr].Para.wIO.ContainsKey(ioType))
                {
                    er = CLanguage.Lan("快充板IO信号") + "[" + ioType.ToString() + "]" + CLanguage.Lan("不存在");
                    return false;                    
                }

                _Mon[addr].Para.wIO[ioType].ioVal = ioVal;

                _Mon[addr].Para.wIO[ioType].op = EOP.写入;                 

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
            }         
        }
        #endregion

    }
}
