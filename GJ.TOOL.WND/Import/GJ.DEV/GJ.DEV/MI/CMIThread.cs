using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;
using System.Threading;
using System.Diagnostics;

namespace GJ.DEV.MI
{
    public class CMIThread
    {
        #region 构造函数
        public CMIThread(int idNo, string name, int startAddr, int endAddr, bool autoMode = false)
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
            return  name;
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
        private int _endAddr = 70;
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
        private CGJMI_10 _devMon = null;
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
        public void SpinUp(CGJMI_10 devMon, bool autoMode = false)
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

                    if (!ReadVoltAndCurrent(i, out er))
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
        private bool WriteData(out string er)
        {
            er = string.Empty;

            try
            {
                 string e = string.Empty;

                 for (int i = _startAddr; i <= _endAddr; i++)
                 {
                     if (_dispose)
                         return true;

                     if (!_Mon[i].Base.conStatus)
                         continue;
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

        #region 方法
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
        private bool ReadVoltAndCurrent(int addr, out string er)
        {
            er = string.Empty;

            try
            {
                List<double> acv =null;

                List<double> aci=null;

                Thread.Sleep(_delayMs);

                if (!_devMon.ReadVoltAndCurrent(addr, out acv, out aci, out er))
                {
                    Thread.Sleep(_delayMs);

                    if (!_devMon.ReadVoltAndCurrent(addr, out acv, out aci, out er))
                    {
                        er = "地址【" + addr.ToString("D2") + "】;";
                        return false;
                    }
                }

                for (int i = 0; i < acv.Count; i++)
                {
                    _Mon[addr].Para.Volt[i] = acv[i];

                    _Mon[addr].Para.Current[i] = aci[i];
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
    }
}
