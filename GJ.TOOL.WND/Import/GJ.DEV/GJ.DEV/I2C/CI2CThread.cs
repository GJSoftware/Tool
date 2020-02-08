using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using GJ.COM;

namespace GJ.DEV.I2C
{
    public class CI2CThread
    {
        #region 构造函数
        public CI2CThread(int idNo, string name, int startAddr, int endAddr, bool autoMode = false)
        {
            this._idNo = idNo;

            this._name = name;

            this._startAddr = startAddr;

            this._endAddr = endAddr;

            this._autoMode = autoMode;

            for (int i = startAddr; i <= endAddr; i++)
            {
                CI2C mon = new CI2C();

                mon.Base.addr = i;

                mon.Base.name = _name + "-【I2C板" + i.ToString("D2") + "】";

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
        private CI2CCom _devMon = null;
        /// <summary>
        /// 监控线程
        /// </summary>
        private Thread _threadMon = null;
        /// <summary>
        /// 线程同步锁
        /// </summary>
        private ReaderWriterLock _syncLock = new ReaderWriterLock();
        /// <summary>
        /// 初始化标志
        /// </summary>
        private bool _iniOK = false;
        /// <summary>
        /// 控制板基本信息:【地址->信息】
        /// </summary>
        public Dictionary<int, CI2C> _Mon = new Dictionary<int, CI2C>();
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
        public void SpinUp(CI2CCom devMon, bool autoMode = false)
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

                    //设置快充板信号
                    if(!writeData(out er))
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

                    if (_Mon[i].Base.conStatus && _Mon[i].Para.wRunPara_OP == EOP.写入)
                    {
                        if (!writeRunPara(i, out er))
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
                    }

                    //读取普通时序 
                    if (_Mon[i].Base.conStatus && (_Mon[i].Para.rRunPara_OP == EOP.读取))
                    {
                        if (!readRunPara(i, out er))
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
        /// 读取I2C数据状态
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

                if (_Mon[addr].Para.wRunPara.PlaceType == EPlace.只有左边产品 || _Mon[addr].Para.wRunPara.PlaceType == EPlace.两边都有产品)
                {
                    CI2C_Data data = new CI2C_Data();

                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadI2C_Data(addr,1, ref data, out er))
                    {
                        Thread.Sleep(_delayMs);

                        if (!_devMon.ReadI2C_Data(addr, 1, ref data, out er))
                        {
                            er = _Mon[addr].ToString() + "读数据1;";
                            return false;
                        }
                    }

                    _Mon[addr].Para.RunData[0] = data.Clone();

                }

                if (_Mon[addr].Para.wRunPara.PlaceType == EPlace.只有右边产品 || _Mon[addr].Para.wRunPara.PlaceType == EPlace.两边都有产品)
                {
                    CI2C_Data data = new CI2C_Data();

                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadI2C_Data(addr, 2, ref data, out er))
                    {
                        Thread.Sleep(_delayMs);

                        if (!_devMon.ReadI2C_Data(addr, 2, ref data, out er))
                        {
                            er = _Mon[addr].ToString() + "读数据2;";
                            return false;
                        }
                    }

                    _Mon[addr].Para.RunData[1] = data.Clone();

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
        /// 读取I2C设置参数状态
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readRunPara(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                CI2C_RunPara para = new CI2C_RunPara();

                Thread.Sleep(_delayMs);

                if (!_devMon.ReadI2C_RunPara(addr, ref para, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadI2C_RunPara(addr, ref para, out er))
                    {
                        er = _Mon[addr].ToString() + "读参数;";
                        return false;
                    }
                }

                _Mon[addr].Para.rRunPara = para.Clone();

                _Mon[addr].Para.rRunPara_OP = EOP.读取OK;  

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
        /// 写入I2C设置参数状态
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeRunPara(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                _syncLock.AcquireWriterLock(-1);

                Thread.Sleep(_delayMs);

                if (!_devMon.SendToSetI2C_RunPara(addr,_Mon[addr].Para.wRunPara, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.SendToSetI2C_RunPara(addr, _Mon[addr].Para.wRunPara, out er))
                    {
                        er = _Mon[addr].ToString() + "写参数;";
                        return false;
                    }
                }

                _Mon[addr].Para.wRunPara_OP = EOP.写入OK;

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
        public bool SetStatus(int addr, ESTATUS status, EPlace placeType, out string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = "I2C地址[" + addr.ToString("D2") + "不存在";
                    return false;
                }

                _Mon[addr].Base.status = status;

                _Mon[addr].Para.wRunPara.PlaceType = placeType;  

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
        /// 设置I2C运行参数
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="para"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SetRunPara(int addr, CI2C_RunPara para, out string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = "I2C地址[" + addr.ToString("D2") + "不存在";
                    return false;
                }

                _Mon[addr].Para.wRunPara = para.Clone(); 

                _Mon[addr].Para.wRunPara_OP = EOP.写入;

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
        /// 读取I2C运行参数
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ReadRunPara(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                if (!_Mon.ContainsKey(addr))
                {
                    er = "I2C地址[" + addr.ToString("D2") + "不存在";
                    return false;
                }

                _Mon[addr].Para.rRunPara_OP = EOP.读取;

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
