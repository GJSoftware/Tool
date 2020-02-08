using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics; 
using GJ.COM;

namespace GJ.DEV.RemoteIO
{
    public class CIOThread
    {
        #region 寄存器定义
        /// <summary>
        /// IO基本类
        /// </summary>
        public class CREG
        {
            /// <summary>
            /// 设备地址
            /// </summary>
            public int devAddr;
            /// <summary>
            /// 寄存器名称
            /// </summary>
            public string regName;
            /// <summary>
            /// 寄存器描述
            /// </summary>
            public string regDes;
            /// <summary>
            /// 寄存器类型
            /// </summary>
            public ERegType regType;
            /// <summary>
            /// 寄存器地址
            /// </summary>
            public int regAddr;    
            /// <summary>
            /// 寄存器长度
            /// </summary>
            public int len;
            /// <summary>
            /// 复制
            /// </summary>
            /// <returns></returns>
            public CREG Clone()
            {
                CREG reg = new CREG();
                reg.devAddr = this.devAddr;
                reg.regName = this.regName;
                reg.regDes = this.regDes;
                reg.regType = this.regType;
                reg.regAddr = this.regAddr;
                reg.len = this.len;
                return reg;
            }
        }
        /// <summary>
        /// 写寄存器值
        /// </summary>
        public class CWIOVal
        {
            public CWIOVal(string regDes, int regVal)
            {
                this.regDes = regDes;
                this.regVal = regVal;
            }
            public string regDes;
            public int regVal;
        }
        /// <summary>
        /// 写多个寄存器值
        /// </summary>
        public class CWMutiIOVal
        {
            public CWMutiIOVal(int devAddr, ERegType regType, int regNo, int[] regVal)
            {
                this.devAddr = devAddr; 
                this.regType = regType;                 
                this.regNo = regNo;
                this.regVal = regVal;
            }
            public int devAddr;
            public ERegType regType;            
            public int regNo;
            public int[] regVal;
        }
        /// <summary>
        /// 扫描寄存器
        /// </summary>
        private List<CREG> scanReg = new List<CREG>();
        /// <summary>
        /// IO读寄存器【寄存器描述-寄存器名】
        /// </summary>
        public Dictionary<string, CREG> rIOREG = new Dictionary<string, CREG>();
        /// <summary>
        /// IO读寄存器绑定:【寄存器名与描述】
        /// </summary>
        private Dictionary<string, string> rIOMap = new Dictionary<string, string>();
        /// <summary>
        /// IO读寄存器值【寄存器描述-寄存器值】
        /// </summary>
        public Dictionary<string, int> rIOVal = new Dictionary<string, int>();
        /// <summary>
        /// IO写寄存器【寄存器描述-寄存器名】
        /// </summary>
        public Dictionary<string, CREG> wIOREG = new Dictionary<string, CREG>();
        /// <summary>
        /// IO写寄存器绑定:【寄存器名与描述】
        /// </summary>
        private Dictionary<string, string> wIOMap = new Dictionary<string, string>();
        /// <summary>
        /// IO写寄存器值【寄存器描述-寄存器值】
        /// </summary>
        public Dictionary<string, int> wIOVal = new Dictionary<string, int>();
        /// <summary>
        /// 写寄存器操作
        /// </summary>
        private volatile Queue<CWIOVal> wSetIOVal = new Queue<CWIOVal>();
        /// <summary>
        /// 写多个寄存器操作
        /// </summary>
        private volatile Queue<CWMutiIOVal> wSetMutiIOVal = new Queue<CWMutiIOVal>();
        #endregion

        #region 构造函数
        public CIOThread(List<CREG>scanReg,List<CREG> rReg,List<CREG> wReg,int idNo=0,string name="远程IO线程")
        {
            this._idNo = idNo;

            this._name = name;

            this.scanReg = scanReg; 

            //读寄存器

            rIOREG.Clear();

            rIOMap.Clear();

            rIOVal.Clear(); 

            for (int i = 0; i < rReg.Count; i++)
            {
                if (!rIOREG.ContainsKey(rReg[i].regDes))
                {
                    rIOREG.Add(rReg[i].regDes, rReg[i].Clone());

                    rIOMap.Add(rReg[i].regName, rReg[i].regDes);

                    rIOVal.Add(rReg[i].regDes, -1); 
                }
            }
 
            //写寄存器
            
            wIOREG.Clear();

            wIOMap.Clear();

            wIOVal.Clear();

            for (int i = 0; i < wReg.Count; i++)
            {
                if (!wIOREG.ContainsKey(wReg[i].regDes))
                {
                    wIOREG.Add(wReg[i].regDes, wReg[i].Clone());

                    wIOMap.Add(wReg[i].regName, wReg[i].regDes);

                    wIOVal.Add(wReg[i].regDes, -1);
                }
            }
 
  
        }
        public override string ToString()
        {
            return _name;
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
        /// IO设备
        /// </summary>
        private CIOCom _devIO = null;
        /// <summary>
        /// IO监控线程
        /// </summary>
        private Thread threadIO = null;
        /// <summary>
        /// 线程同步锁
        /// </summary>
        private ReaderWriterLock ioWriteLock = new ReaderWriterLock();
        /// <summary>
        /// 扫描间隔
        /// </summary>
        private volatile int _delayMs = 50;
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
        #endregion

        #region 属性
        /// <summary>
        /// 设备ID
        /// </summary>
        public int idNo
        {
            get { return _idNo; }
            set { _idNo = value; }
        }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool conStatus
        {
            get
            {
                if (_devIO == null)
                    return false;
                else
                    return _devIO.conStatus;

            }
        }
        /// <summary>
        /// 自动模式和手动模式
        /// </summary>
        public bool autoMode
        {
            set { _autoMode = value; }
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
        public void SpinUp(CIOCom devIO,bool autoMode = true)
        {
            this._devIO = devIO;

            this._autoMode = autoMode; 

            if (threadIO == null)
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

                foreach (string keyVal in rIOREG.Keys)
                {
                    if (rIOVal.ContainsKey(keyVal))
                        rIOVal[keyVal] = -1;
                }

                foreach (string keyVal in wIOREG.Keys)
                {
                    if (wIOVal.ContainsKey(keyVal))
                        wIOVal[keyVal] = -1;
                }

                threadIO = new Thread(OnStart);
                threadIO.Name = _name;
                threadIO.IsBackground = true;
                threadIO.Start();
                OnStatusArgs.OnEvented(new CConArgs(CLanguage.Lan("创建监控线程") + threadIO.Name));
            }
        }
        /// <summary>
        /// 销毁线程
        /// </summary>
        public void SpinDown()
        {
            try
            {
                if (threadIO != null)
                {
                    _dispose = true;
                    do
                    {
                        if (!threadIO.IsAlive)
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
                threadIO = null;
            }
        }
        /// <summary>
        /// 暂停线程
        /// </summary>
        public void Paused()
        {
            if (!_autoMode && threadIO != null)
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
            if (!_autoMode && threadIO != null)
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

                    if (!writeIO(out er))
                        OnDataArgs.OnEvented(new CDataArgs(er, false, true));

                    if (!writeMutiIO(out er))
                        OnDataArgs.OnEvented(new CDataArgs(er, false, true));
                    
                    if (!readIO(out er))
                        OnDataArgs.OnEvented(new CDataArgs(er, false, true));

                    wather.Stop();

                    if (!_autoMode)
                    {
                        _paused = true;
                        _threadStatus = EThreadStatus.暂停;
                        OnDataArgs.OnEvented(new CDataArgs(CLanguage.Lan("扫描时间") + ":" + wather.ElapsedMilliseconds.ToString() + "ms"));
                        continue;
                    }

                    _threadStatus = EThreadStatus.运行;
                    
                }
            }
            catch (Exception ex)
            {
                OnStatusArgs.OnEvented(new CConArgs(threadIO.Name + CLanguage.Lan("监控线程异常错误") + ":" + ex.ToString(), true));
            }
            finally
            {
                _dispose = false;
                _threadStatus = EThreadStatus.退出;
                OnStatusArgs.OnEvented(new CConArgs(threadIO.Name + CLanguage.Lan("监控线程销毁退出"), true));
            }
        }
        /// <summary>
        /// 读寄存器
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readIO(out string er)
        {
            er = string.Empty;

            try
            {
                ioWriteLock.AcquireWriterLock(-1);

                bool flag = true;

                for (int i = 0; i < scanReg.Count; i++)
                {
                    bool regOk = true;

                    if (_dispose)
                        return true;

                    //读多个寄存器值
                    int rLen = scanReg[i].len * 8;

                    int[] rVal = new int[rLen];

                    Thread.Sleep(10);

                    if (!_devIO.Read(scanReg[i].devAddr, scanReg[i].regType, scanReg[i].regAddr, ref rVal, out er))
                    {
                        Thread.Sleep(20);

                        if (!_devIO.Read(scanReg[i].devAddr, scanReg[i].regType, scanReg[i].regAddr, ref rVal, out er))
                        {
                            er = "[" + scanReg[i].devAddr.ToString("D2") + "_" + scanReg[i].regType.ToString() + scanReg[i].regAddr.ToString() + "]读操作错误:" + er;
                            flag = false;
                            regOk = false;
                        }

                    }
                    //获取寄存器值  
                    if (regOk)
                    {
                        for (int j = 0; j < scanReg[i].len; j++)
                        {
                            for (int z = 0; z < 8; z++)
                            {
                                string regName = string.Empty;
                                regName = scanReg[i].devAddr.ToString("D2") + "_" + scanReg[i].regType.ToString() +
                                         (scanReg[i].regAddr + j * 8 + z).ToString();
                                if (rIOMap.ContainsKey(regName))
                                    rIOVal[rIOMap[regName]] = rVal[j * 8 + z];
                                if (wIOMap.ContainsKey(regName))
                                    wIOVal[wIOMap[regName]] = rVal[j * 8 + z];
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < scanReg[i].len; j++)
                        {
                            for (int z = 0; z < 8; z++)
                            {
                                string regName = string.Empty;
                                regName = scanReg[i].devAddr.ToString("D2") + "_" + scanReg[i].regType.ToString() +
                                         (scanReg[i].regAddr + j * 8 + z).ToString();
                                if (rIOMap.ContainsKey(regName))
                                    rIOVal[rIOMap[regName]] = -1;
                                if (wIOMap.ContainsKey(regName))
                                    wIOVal[wIOMap[regName]] = -1;
                            }
                        }
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                ioWriteLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 写单个寄存器
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeIO(out string er)
        {
            er = string.Empty;

            try
            {
                ioWriteLock.AcquireWriterLock(-1);

                bool flag = true;

                for (int i = 0; i < wSetIOVal.Count; )
                {
                    bool regOk = true;

                    if (_dispose)
                        return true;

                    CWIOVal wIO = wSetIOVal.Dequeue();

                    if (!wIOREG.ContainsKey(wIO.regDes))
                    {
                        er = "[" + wIO.regDes + "]"+ CLanguage.Lan("写操作错误") + ":" + CLanguage.Lan("该寄存器不存在");
                        flag = false;
                        regOk = false;
                    }

                    string regKey = wIO.regDes;

                    int regVal = wIO.regVal;

                    Thread.Sleep(10);

                    if (!_devIO.Write(wIOREG[regKey].devAddr, wIOREG[regKey].regType, wIOREG[regKey].regAddr, regVal, out er))
                    {
                        Thread.Sleep(10);

                        if (!_devIO.Write(wIOREG[regKey].devAddr, wIOREG[regKey].regType, wIOREG[regKey].regAddr, regVal, out er))
                        {
                            er = "[" + regKey + "]" +  CLanguage.Lan("写操作错误") + ":" + er;
                            flag = false;
                            regOk = false;
                        }
                    }

                    if (!regOk)
                    {
                        wSetIOVal.Enqueue(wIO); 
                    }                    
                }
                return flag;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                ioWriteLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 写多个寄存器值
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeMutiIO(out string er)
        {
            er = string.Empty;

            try
            {
                ioWriteLock.AcquireWriterLock(-1);

                for (int i = 0; i < wSetMutiIOVal.Count; )
                {

                    if (_dispose)
                        return true;

                    Thread.Sleep(_delayMs);

                    bool writeOK = true;

                    CWMutiIOVal mutiIO = wSetMutiIOVal.Dequeue();

                    if (!_devIO.Write(mutiIO.devAddr, mutiIO.regType, mutiIO.regNo, mutiIO.regVal, out er))
                    {
                        Thread.Sleep(_delayMs);

                        if (!_devIO.Write(mutiIO.devAddr, mutiIO.regType, mutiIO.regNo, mutiIO.regVal, out er))
                        {                           
                            er = "[" + mutiIO.regNo + "]" + CLanguage.Lan("写操作错误") + ":" + er;
                            writeOK = false;
                        }
                    }

                    if (!writeOK)
                       wSetMutiIOVal.Enqueue(mutiIO);
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
                ioWriteLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 添加写寄存器操作
        /// </summary>
        /// <param name="regDes"></param>
        /// <param name="regVal"></param>
        /// <returns></returns>
        public bool AddIoWrite(string regDes, int regVal)
        {
            try
            {
                if (!wIOVal.ContainsKey(regDes))
                    return false;

                CWIOVal wIO=new CWIOVal(regDes,regVal);

                if(!wSetIOVal.Contains(wIO))  
                {
                    wSetIOVal.Enqueue(wIO);

                    wIOVal[regDes] = -1;
                }
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 写入多个寄存器值
        /// </summary>
        /// <param name="startREG"></param>
        /// <param name="regVal"></param>
        /// <returns></returns>
        public bool AddMutiIOWrite(int devAddr,ERegType regType, int regNo, int[] regVal)
        {
            try
            {
                string regName = string.Empty;

                for (int i = 0; i < regVal.Length; i++)
                {
                    for (int z = 0; z < 8; z++)
                    {
                        regName = devAddr.ToString("D2") + "_" + regType.ToString() + (regNo + i).ToString();

                        if (wIOVal.ContainsKey(regName))
                            wIOVal[regName] = -1;
                    }
                    
                }

                CWMutiIOVal mutiIo=new CWMutiIOVal(devAddr,regType,regNo,regVal);

                if (!wSetMutiIOVal.Contains(mutiIo))
                    wSetMutiIOVal.Enqueue(mutiIo); 

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

    }
}
