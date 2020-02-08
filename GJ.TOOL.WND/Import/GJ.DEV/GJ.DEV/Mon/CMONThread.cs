using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using GJ.COM;

namespace GJ.DEV.Mon
{
    public class CMONThread
    {
        #region 构造函数
        public CMONThread(int idNo, string name, int startAddr, int endAddr, bool autoMode = false)
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

                mon.Base.name = _name + "-【控制板" + i.ToString("D2") + "】";

                mon.Base.status = ESTATUS.运行;

                mon.Base.conStatus = true;

                _Mon.Add(i, mon);   

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
        /// 延时时间
        /// </summary>
        private volatile int _delayMs = 10;
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
        private CMONCom _devMon = null;
        /// <summary>
        /// 监控线程
        /// </summary>
        private Thread _threadMon = null;
        /// <summary>
        /// 初始化标志
        /// </summary>
        private bool _iniOK = false;
        /// <summary>
        /// 控制板基本信息:【地址->信息】
        /// </summary>
        public Dictionary<int, CMon> _Mon = new Dictionary<int, CMon>();
        /// <summary>
        /// 老化模式
        /// </summary>
        private EBIMode _runMode = EBIMode.普通老化模式;
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
        public void SpinUp(CMONCom devMon,EBIMode runMode=EBIMode.普通老化模式, bool autoMode = false)
        {
            this._devMon = devMon;

            this.autoMode = autoMode;

            this._runMode = runMode; 

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

                    //设置老化模式
                    if (!iniOK)
                    { 
                       if(writeIniPara(out er))
                           OnDataArgs.OnEvented(new CDataArgs(er, false, true));
                    }

                    //设置快充板信号
                    if (!writeData(out er))
                        OnDataArgs.OnEvented(new CDataArgs(er, false, true));

                    //读取快充板信号及电压值
                    if (!readData(out er))
                        OnDataArgs.OnEvented(new CDataArgs(er, false, true));

                    
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
        /// 设置模式
        /// </summary>
        /// <returns></returns>
        private bool writeIniPara(out string er)
        {
            er = string.Empty;

            try
            {

                if (_devMon.SetWorkMode(0, (int)_runMode, out er))
                    return false;

                Thread.Sleep(2000);
                
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

                    //写入ONOFF时序 
                    if (_Mon[i].Base.conStatus && _Mon[i].Para.TIMER_OP == EOP.写入)
                    {
                        if (!writeTimer(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }
                    }
                    //写入快充模式
                    if (_Mon[i].Base.conStatus && _Mon[i].Para.QCV_OP == EOP.写入)
                    {
                        if (!writeQCV(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }
                    }
                    //写入运行状态
                    if (_Mon[i].Base.conStatus && _Mon[i].Para.RUN_OP == EOP.写入)
                    {
                        if (!writeRun(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }
                    }
                    if (_Mon[i].Base.conStatus && _Mon[i].Para.STOP_OP == EOP.写入)
                    {
                        if (!writeStop(i, out er))
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
                        if (_Mon[i].Base.conStatus)
                        {
                            if (!readSignal(i, out er))
                            {
                                e += er;
                                alarm = true;
                                _Mon[i].Base.conStatus = false;
                            }
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
                   
                    //读取普通时序 
                    if (_Mon[i].Base.conStatus && (_Mon[i].Para.TIMER_OP == EOP.读取))
                    {
                        if (!readTimer(i, out er))
                        {
                            e += er;
                            alarm = true;
                        }
                    }
                    //读取快充电压
                    if (_Mon[i].Base.conStatus && (_Mon[i].Para.QCV_OP == EOP.读取))
                    {
                        if (!readQCV(i, out er))
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
                Thread.Sleep(_delayMs);

                CrRunPara para = new CrRunPara();

                if (!_devMon.ReadRunData(addr, ref para, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadRunData(addr, ref para, out er))
                    {
                        er = _Mon[addr].ToString() + "读信号;";
                        return false;
                    }
                }

                _Mon[addr].Para.Sinal = para.Clone();

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
        /// 读取电压值
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readVolt(int addr, out string er)
        {
            er = string.Empty;

            try
            {

                Thread.Sleep(_delayMs);

                List<double> Volt = null;

                int rOnOff = 0;

                if (!_devMon.ReadVolt(addr, out Volt,out rOnOff, out er, ESynON.同步))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadVolt(addr, out Volt, out rOnOff, out er, ESynON.同步))
                    {
                        er = _Mon[addr].ToString() + "读电压;";
                        return false;
                    }
                }

                for (int i = 0; i < Volt.Count; i++)
                    _Mon[addr].Para.Volt[i] = Volt[i];

                _Mon[addr].Para.rOnOff = rOnOff; 

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
        /// 读取时序
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readTimer(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                Thread.Sleep(_delayMs);

                COnOffPara para = new COnOffPara();

                if (!_devMon.ReadOnOffPara(addr, ref para, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadOnOffPara(addr, ref para, out er))
                    {
                        er = _Mon[addr].ToString() + "读时序;";
                        return false;
                    }
                }

                _Mon[addr].Para.Timer.BIToTime = para.BIToTime;

                for (int i = 0; i < _Mon[addr].Para.Timer.wOnOff.Length; i++)
                {
                    _Mon[addr].Para.Timer.wOnOff[i] = para.wOnOff[i];
                    _Mon[addr].Para.Timer.wON[i] = para.wON[i];
                    _Mon[addr].Para.Timer.wOFF[i] = para.wOFF[i]; 
                }

                _Mon[addr].Para.TIMER_OP = EOP.读取OK;

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
        /// 读取快充电压
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readQCV(int addr, out string er)
        {
            er = string.Empty;

            try
            {

                Thread.Sleep(_delayMs);

                COnOffPara para = new COnOffPara();

                if (!_devMon.ReadGJM_RunQC_Para(addr, ref para, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadGJM_RunQC_Para(addr, ref para, out er))
                    {
                        er = _Mon[addr].ToString() + "读快充;";
                        return false;
                    }
                }
              
                _Mon[addr].Para.Timer.wQCType = para.wQCType;

                for (int i = 0; i < _Mon[addr].Para.Timer.wQCVolt.Length; i++)
                {
                    _Mon[addr].Para.Timer.wQCVolt[i] = para.wQCVolt[i];
                }

                _Mon[addr].Para.TIMER_OP = EOP.读取OK;

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
        /// 写入时序
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeTimer(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                Thread.Sleep(_delayMs);

                if (!_devMon.SetOnOffPara(addr, _Mon[addr].Para.Timer, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.SetOnOffPara(addr, _Mon[addr].Para.Timer, out er))
                    {
                        er = _Mon[addr].ToString() + "写时序;";
                        return false;
                    }
                }

                Thread.Sleep(600);

                _Mon[addr].Para.TIMER_OP = EOP.写入OK;

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
        /// 写快充
        /// </summary>
        /// <param name="add"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeQCV(int addr, out string er)
        {
            er = string.Empty;

            try
            {

                Thread.Sleep(_delayMs);

                if (!_devMon.SetGJM_RunQC_Para(addr, _Mon[addr].Para.Timer, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.SetGJM_RunQC_Para(addr, _Mon[addr].Para.Timer, out er))
                    {
                        er = _Mon[addr].ToString() + "写快充;";
                        return false;
                    }
                }

                Thread.Sleep(300);

                _Mon[addr].Para.TIMER_OP = EOP.写入OK;

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
        /// 写入运行状态
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeRun(int addr, out string er)
        {
            er = string.Empty;

            try
            {

                Thread.Sleep(_delayMs);

                if (!_devMon.SetRunStart(addr, _Mon[addr].Para.RunPara, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.SetRunStart(addr, _Mon[addr].Para.RunPara, out er))
                    {
                        er = _Mon[addr].ToString() + "写状态;";
                        return false;
                    }
                }

                Thread.Sleep(300);

                _Mon[addr].Para.RUN_OP = EOP.写入OK;

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
        /// 写入结束老化
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeStop(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                Thread.Sleep(_delayMs);

                if (!_devMon.ForceFinish(addr, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.ForceFinish(addr, out er))
                    {
                        er = _Mon[addr].ToString() + "写结束;";
                        return false;
                    }
                }

                _Mon[addr].Para.STOP_OP = EOP.写入OK;

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
                    er = "控制板地址[" + addr.ToString("D2") + "不存在";
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
        public bool SetTimer(int addr, COnOffPara para,out string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = "控制板地址[" + addr.ToString("D2") + "不存在";
                    return false;
                }

                _Mon[addr].Para.Timer.BIToTime = para.BIToTime;

                for (int i = 0; i < _Mon[addr].Para.Timer.wOnOff.Length; i++)
                {
                    _Mon[addr].Para.Timer.wOnOff[i] = para.wOnOff[i];
                    _Mon[addr].Para.Timer.wON[i] = para.wON[i];
                    _Mon[addr].Para.Timer.wOFF[i] = para.wOFF[i];  
                }

                _Mon[addr].Para.TIMER_OP = EOP.写入; 

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
        public bool ReadTimer(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = "控制板地址[" + addr.ToString("D2") + "不存在";
                    return false;
                }

                _Mon[addr].Para.TIMER_OP = EOP.读取;

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
        public bool SetQCV(int addr, COnOffPara para, out string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = "控制板地址[" + addr.ToString("D2") + "不存在";
                    return false;
                }

                _Mon[addr].Para.Timer.wQCType = para.wQCType;

                for (int i = 0; i < _Mon[addr].Para.Timer.wQCVolt.Length; i++)
                {
                    _Mon[addr].Para.Timer.wQCVolt[i] = para.wQCVolt[i];
                }

                _Mon[addr].Para.QCV_OP = EOP.写入;

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
        public bool ReadQCV(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = "控制板地址[" + addr.ToString("D2") + "不存在";
                    return false;
                }

                _Mon[addr].Para.QCV_OP = EOP.读取;

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
        /// 启动老化
        /// </summary>
        /// <param name="startAddr"></param>
        /// <param name="status"></param>
        public bool SetRun(int addr, CwRunPara para, out string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = "控制板地址[" + addr.ToString("D2") + "不存在";
                    return false;
                }

                _Mon[addr].Para.RunPara = para.Clone();


                _Mon[addr].Para.RUN_OP = EOP.写入;

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
        /// 结束老化
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SetStop(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = "控制板地址[" + addr.ToString("D2") + "不存在";
                    return false;
                }

                _Mon[addr].Para.STOP_OP = EOP.写入;

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
