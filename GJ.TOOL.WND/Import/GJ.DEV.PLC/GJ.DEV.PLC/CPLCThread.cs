using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics; 
using GJ.COM;
using GJ.DEV.PLC;
namespace GJ.DEV.PLC
{
    /// <summary>
    /// PLC线程类->提供实时监控PLC信号
    /// 版本:V1.0.0 作者:kp.lin 日期:2017/08/15
    /// </summary>
    public class CPLCThread
    {
        #region 寄存器定义
        public class CUSER_PLCREG
        {
            /// <summary>
            /// 扫描寄存器
            /// </summary>
            public List<CPLCThread.CREG> scanREG = new List<CPLCThread.CREG>();
            /// <summary>
            /// 写寄存器
            /// </summary>
            public List<CPLCThread.CREG> wREG = new List<CPLCThread.CREG>();
            /// <summary>
            /// 读寄存器
            /// </summary>
            public List<CPLCThread.CREG> rREG = new List<CPLCThread.CREG>();
        }
        /// <summary>
        /// 寄存器定义
        /// </summary>
        public class CREG
        {
            /// <summary>
            /// 寄存器名称(标准名称D100)
            /// </summary>
            public string regName = string.Empty;
            /// <summary>
            /// 自定义描述
            /// </summary>
            public string regDes = string.Empty;
            /// <summary>
            /// 寄存器类型
            /// </summary>
            public ERegType regType = ERegType.D;
            /// <summary>
            /// 开始地址
            /// </summary>
            public int startAddr = 0;
            /// <summary>
            /// 位地址
            /// </summary>
            public int startBin = 0;
            /// <summary>
            /// 寄存器长度
            /// </summary>
            public int regLen = 0;           
        }
        /// <summary>
        /// 写寄存器值
        /// </summary>
        private class CWREG
        {
            public CWREG(string regDes, int regVal)
            {
                this.Des = regDes;
                this.Val = regVal; 
            }
            public string Des;
            public int Val;
        }
        /// <summary>
        /// 写多个寄存器值
        /// </summary>
        private class CWMutiREG
        {
            public CWMutiREG(ERegType regType, int regAddr, int[] regVal)
            {
                this.regType = regType;
                this.Addr = regAddr;
                this.Val = regVal;
                this.dataType = 0;
            }
            public CWMutiREG(ERegType regType, int regAddr, int regLen, string regHex)
            {
                this.regType = regType;
                this.Addr = regAddr;
                this.Len = regLen;
                this.HexVal = regHex;
                this.dataType = 1;
            }
            /// <summary>
            /// 0:数值 1:16进值
            /// </summary>
            public int dataType = 0;
            public ERegType regType;
            public int Addr;
            public int Len;
            public int[] Val;           
            public string HexVal;
        }
        /// <summary>
        /// 扫描寄存器
        /// </summary>
        private List<CREG> _scanReg = null;
        /// <summary>
        /// 读寄存器匹对[寄存器名->寄存器描述]
        /// </summary>
        private Dictionary<string, string> _rRegMap = new Dictionary<string, string>();
        /// <summary>
        /// 读寄存器[寄存器描述->寄存器信息]
        /// </summary>
        private Dictionary<string,CREG> _rReg = new Dictionary<string,CREG>();
        /// <summary>
        /// 读寄存器数值[寄存器描述->寄存器值]
        /// </summary>
        private Dictionary<string, int> _rRegVal = new Dictionary<string, int>();
        /// <summary>
        /// 读寄存器16进制字符值[寄存器描述->寄存器值]
        /// </summary>
        private Dictionary<string, string> _rRegHex = new Dictionary<string, string>();
        /// <summary>
        /// 写寄存器匹对[寄存器名->寄存器描述]
        /// </summary>
        private Dictionary<string, string> _wRegMap = new Dictionary<string, string>();
        /// <summary>
        /// 写寄存器[寄存器描述->寄存器信息]
        /// </summary>
        private Dictionary<string, CREG> _wReg =new Dictionary<string,CREG>();
        /// <summary>
        /// 写寄存器数值[寄存器描述->寄存器值]
        /// </summary>
        private Dictionary<string, int> _wRegVal = new Dictionary<string, int>();
        /// <summary>
        /// 写存器16进制字符值[寄存器描述->寄存器值]
        /// </summary>
        private Dictionary<string, string> _wRegHex = new Dictionary<string, string>();
        /// <summary>
        /// 写入寄存器值
        /// </summary>
        private Queue<CWREG> _writeReg = new Queue<CWREG>();
        /// <summary>
        /// 写入多个寄存器值
        /// </summary>
        private Queue<CWMutiREG> _writeMutiReg = new Queue<CWMutiREG>();
        #endregion

        #region 构造函数
        /// <summary>
        /// PLCThread类
        /// </summary>
        /// <param name="scanReg">扫描寄存器</param>
        /// <param name="rReg">读寄存器</param>
        /// <param name="wReg">写寄存器</param>
        /// <param name="idNo">设备ID</param>
        /// <param name="name">设备名称</param>
        public CPLCThread(List<CREG> scanReg,List<CREG> rReg,List<CREG> wReg, int idNo=0,string name="PLC Thread")
        {
            this._idNo = idNo;

            this._name = name;

            this._scanReg = scanReg;

            //读寄存器

            _rRegMap.Clear();

            _rReg.Clear();

            _rRegVal.Clear(); 

            _rRegHex.Clear();

            for (int i = 0; i < rReg.Count; i++)
            {
                if (!_rReg.ContainsKey(rReg[i].regDes))
                {
                    CREG reg = rReg[i];

                    _rRegMap.Add(reg.regName, reg.regDes);

                    _rReg.Add(reg.regDes, reg);

                    _rRegVal.Add(reg.regDes, -1); 

                    _rRegHex.Add(reg.regDes, "FFFF"); 
                }
            }

            //写寄存器

            _wRegMap.Clear();

            _wReg.Clear();

            _wRegVal.Clear(); 

            _wRegHex.Clear();

            for (int i = 0; i < wReg.Count; i++)
            {
                if (!_wReg.ContainsKey(wReg[i].regDes))
                {
                    CREG reg = wReg[i];

                    _wRegMap.Add(reg.regName, reg.regDes);

                    _wReg.Add(reg.regDes, reg);

                    _wRegVal.Add(reg.regDes, -1);

                    _wRegHex.Add(reg.regDes, "FFFF");
                }
            }

        }
        /// <summary>
        /// PLCThread类
        /// </summary>
        /// <param name="scanReg">扫描寄存器</param>
        /// <param name="rReg">读寄存器</param>
        /// <param name="wReg">写寄存器</param>
        /// <param name="idNo">设备ID</param>
        /// <param name="name">设备名称</param>
        public CPLCThread(CUSER_PLCREG plcReg, int idNo = 0, string name = "PLC Thread")
        {
            this._idNo = idNo;

            this._name = name;

            this._scanReg = plcReg.scanREG;

            //读寄存器

            _rRegMap.Clear();

            _rReg.Clear();

            _rRegVal.Clear();

            _rRegHex.Clear();

            for (int i = 0; i < plcReg.rREG.Count; i++)
            {
                if (!_rReg.ContainsKey(plcReg.rREG[i].regDes))
                {
                    CREG reg = plcReg.rREG[i];

                    _rRegMap.Add(reg.regName, reg.regDes);

                    _rReg.Add(reg.regDes, reg);

                    _rRegVal.Add(reg.regDes, -1);

                    _rRegHex.Add(reg.regDes, "FFFF");
                }
            }

            //写寄存器

            _wRegMap.Clear();

            _wReg.Clear();

            _wRegVal.Clear();

            _wRegHex.Clear();

            for (int i = 0; i < plcReg.wREG.Count; i++)
            {
                if (!_wReg.ContainsKey(plcReg.wREG[i].regDes))
                {
                    CREG reg = plcReg.wREG[i];

                    _wRegMap.Add(reg.regName, reg.regDes);

                    _wReg.Add(reg.regDes, reg);

                    _wRegVal.Add(reg.regDes, -1);

                    _wRegHex.Add(reg.regDes, "FFFF");
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
        public COnEvent<CPLCConArgs> OnConArgs = new COnEvent<CPLCConArgs>();
        /// <summary>
        /// 数据事件
        /// </summary>
        public COnEvent<CPLCDataArgs> OnDataArgs = new COnEvent<CPLCDataArgs>(); 
        #endregion

        #region 字段
        /// <summary>
        /// 设备ID
        /// </summary>
        private int _idNo = 0;
        /// <summary>
        /// 设备名称
        /// </summary>
        private string _name = string.Empty;
        /// <summary>
        /// PLC地址
        /// </summary>
        private int _plcAddr = 1;
        /// <summary>
        /// 读写延时
        /// </summary>
        private int _delayMs = 30;
        /// <summary>
        /// PLC设备
        /// </summary>
        private CPLCCOM _devPLC = null;        
        /// <summary>
        /// 同步锁
        /// </summary>
        private object _synC = new object();
        /// <summary>
        /// 重连次数
        /// </summary>
        private int _conPLCAgain = 0;
        /// <summary>
        /// 软件监控时钟
        /// </summary>
        private Stopwatch _softWatcher = new Stopwatch();
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
            get {
                if (_devPLC == null)
                    return false;
                else
                    return _devPLC.conStatus;
             
               }
        }
        /// <summary>
        /// 读写完成
        /// </summary>
        public bool complete
        {
            get { return _complete; }
        }
        /// <summary>
        /// 线程状态
        /// </summary>
        public EThreadStatus threadStatus
        {
            get { return _threadStatus; }
        }
        /// <summary>
        /// 重连次数
        /// </summary>
        public int ConPLCAgain
        {
            get { return _conPLCAgain; }
            set { _conPLCAgain = value; } 
        }
        /// <summary>
        /// 软件监控时钟
        /// </summary>
        public Stopwatch SoftWatcher
        {
            get { return _softWatcher; }
            set { _softWatcher = value; }
        }
        #endregion

        #region 线程状态
         /// <summary>
        /// 线程状态
        /// </summary>
        public enum EThreadStatus
        { 
         空闲,
         运行,
         暂停
        }
        #endregion

        #region 线程
        /// <summary>
        /// PLC线程
        /// </summary>
        private Thread _ThreadPLC = null;
        /// <summary>
        /// 线程状态
        /// </summary>
        private EThreadStatus _threadStatus = EThreadStatus.空闲; 
        /// <summary>
        /// 自动模式:不间断扫描;手动模式:需手动启动扫描
        /// </summary>
        private volatile bool _autoMode = true;
        /// <summary>
        /// 扫描完毕
        /// </summary>
        private bool _complete = false;
        /// <summary>
        /// 销毁线程标志
        /// </summary>
        private bool _dispose = false;
        /// <summary>
        /// 线程暂停标志
        /// </summary>
        private bool _paused = false;
        /// <summary>
        /// 启动线程 
        /// </summary>
        /// <param name="devPLC">PLC设备</param>
        /// <param name="autoMode">自动模式:不间断扫描;手动模式:需手动启动扫描</param>
        /// <param name="plcAddr">PLC地址:默认为1,TCP通信该参数无效</param>
        public void SpinUp(CPLCCOM devPLC,bool autoMode=true, int plcAddr=1)
        {
            try
            {
                this._devPLC = devPLC;

                this._autoMode = autoMode;

                this._plcAddr = plcAddr;

                _complete = false;

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

                if (_ThreadPLC == null)
                {
                    _ThreadPLC = new Thread(OnRun);
                    _ThreadPLC.Name = _name;
                    _ThreadPLC.IsBackground = true;
                    _ThreadPLC.Start();                
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        /// <summary>
        /// 销毁线程
        /// </summary>
        public void SpinDown()
        {
            try
            {
                if (_ThreadPLC != null)
                {
                    _dispose = true;
                    do
                    {
                        if (!_ThreadPLC.IsAlive)
                            break;
                        Thread.Sleep(20); 

                    } while (_dispose);

                }
            }
            catch (Exception)
            {

            }
            finally
            {
                _ThreadPLC = null;
            }
        }
        /// <summary>
        /// 暂停线程运行
        /// </summary>
        public void paused()
        {
            if (!_autoMode && _ThreadPLC != null)
            {
                _paused = true;
                _threadStatus = EThreadStatus.暂停;
            }
        }
        /// <summary>
        /// 恢复线程运行
        /// </summary>
        public void continued()
        {
            if (!_autoMode && _ThreadPLC != null)
            {
                _paused = false;
                _threadStatus = EThreadStatus.运行;
            }
        }
        /// <summary>
        /// PLC线程
        /// </summary>
        private void OnRun()
        {
            try
            {
                OnConArgs.OnEvented(new CPLCConArgs(_idNo, _name, _name + CLanguage.Lan("监控线程启动"),EMessage.启动));  

                while (true)
                {
                    Thread.Sleep(_delayMs);    

                    if (_dispose)
                        return;

                    if (!_devPLC.conStatus)
                        continue;

                    if (!_autoMode && _paused)
                    {
                        Thread.Sleep(_delayMs);
                        continue;
                    }

                    string er = string.Empty;

                    Stopwatch watcher = new Stopwatch();

                    watcher.Start(); 

                    if (!writeMutiREG(out er))
                    {                        
                        setPLCDataAlarm(er);
                    }

                    if (!writeREG(out er))
                    {                       
                        setPLCDataAlarm(er);
                    }

                    if (!readREG(out er))
                    {                       
                        setPLCDataAlarm(er);
                    }

                    watcher.Stop();

                    _complete = true;

                    if (!_autoMode)
                    {
                        _paused = true;

                        _threadStatus = EThreadStatus.暂停;

                        OnDataArgs.OnEvented(new CPLCDataArgs(_idNo, _name, watcher.ElapsedMilliseconds.ToString(), EMessage.暂停));
                    }
                    else
                    {
                        OnDataArgs.OnEvented(new CPLCDataArgs(_idNo, _name, watcher.ElapsedMilliseconds.ToString(), EMessage.正常));
                    }
                }
            }
            catch (Exception ex)
            {
                OnConArgs.OnEvented(new CPLCConArgs(_idNo, _name, _name + CLanguage.Lan("监控线程错误") + ":"+ex.ToString(), EMessage.异常));  
            }
            finally
            {
                _dispose = false;

                OnConArgs.OnEvented(new CPLCConArgs(_idNo, _name, _name + CLanguage.Lan("监控线程退出"), EMessage.退出));  
            }
        }
        #endregion

        #region 寄存器读写方法
        /// <summary>
        /// 读寄存器值
        /// </summary>
        /// <param name="regDes">寄存器描述</param>
        /// <returns>-1:表示通信异常</returns>
        public int rREG_Val(string regDes)
        {
            lock (_synC)
            {
                if (!_rRegVal.ContainsKey(regDes) || !_devPLC.conStatus)
                    return -1;
                return _rRegVal[regDes]; 
            }
        }
        /// <summary>
        /// 读寄存器16进值字符
        /// </summary>
        /// <param name="regDes">寄存器描述</param>
        /// <returns>FFFF:表示通信异常</returns>
        public string rREG_HEX(string regDes)
        {
            lock (_synC)
            {
                if (!_rRegVal.ContainsKey(regDes) || !_devPLC.conStatus)
                    return "FFFF";
                return _rRegHex[regDes];
            }
        }
        /// <summary>
        /// 写寄存器值
        /// </summary>
        /// <param name="regDes">寄存器描述</param>
        /// <returns>-1:表示通信异常</returns>
        public int wREG_Val(string regDes)
        {
            lock (_synC)
            {
                if (!_wRegVal.ContainsKey(regDes) || !_devPLC.conStatus)
                    return -1;
                return _wRegVal[regDes];
            }
        }
        /// <summary>
        /// 写寄存器16进值字符
        /// </summary>
        /// <param name="regDes">寄存器描述</param>
        /// <returns>FFFF:表示通信异常</returns>
        public string wREG_Hex(string regDes)
        {
            lock (_synC)
            {
                if (!_wRegVal.ContainsKey(regDes) || !_devPLC.conStatus)
                    return "FFFF";
                return _wRegHex[regDes];
            }
        }
        /// <summary>
        /// 添加写寄存器操作
        /// </summary>
        /// <param name="regDes">寄存器描述</param>
        /// <param name="regVal">写入数据</param>
        /// <returns></returns>
        public bool addREGWrite(string regDes, int regVal)
        {
           lock(_synC)
           {
               if (!_wRegVal.ContainsKey(regDes))
                   return false;

               _writeReg.Enqueue(new CWREG(regDes, regVal));

               _wRegVal[regDes] = -1;

               return true;
           }
        }
        /// <summary>
        /// 写入多寄存器值
        /// </summary>
        /// <param name="regType">寄存器类型</param>
        /// <param name="regAddr">寄存器开始地址</param>
        /// <param name="regVal">寄存器数值</param>
        public void addMutiREGWrite(ERegType regType, int regAddr, int[] regVal)
        {
            lock (_synC)
            { 
                _writeMutiReg.Enqueue(new CWMutiREG(regType,regAddr,regVal));   
            
            }
        }
        /// <summary>
        /// 写入多寄存器值16进值
        /// </summary>
        /// <param name="regType">寄存器类型</param>
        /// <param name="regNo">寄存器开始地址</param>
        /// <param name="regLen">寄存器长度</param>
        /// <param name="regHexVal">高位在前,低位在后</param>
        public void addMutiREGWrite(ERegType regType, int regAddr, int regLen, string regHex)
        {
            lock (_synC)
            { 
              _writeMutiReg.Enqueue(new CWMutiREG(regType, regAddr,regLen,regHex));  
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 读寄存器
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool readREG(out string er)
        {
            er = string.Empty;

            try
            {
                for (int i = 0; i < _scanReg.Count; i++)
                {
                    //读取扫描寄存器数据

                    string strHex = string.Empty;

                    int N = _scanReg[i].regLen;

                    if (_scanReg[i].regType != ERegType.D)
                    {
                        N = _scanReg[i].regLen * _devPLC.wordNum * 8;
                    }

                    int[] rVal = new int[N];

                    Thread.Sleep(_delayMs);

                    if (!_devPLC.Read(_plcAddr, _scanReg[i].regType, _scanReg[i].startAddr, ref rVal, out er))
                    {
                        Thread.Sleep(_delayMs);

                        if (!_devPLC.Read(_plcAddr, _scanReg[i].regType, _scanReg[i].startAddr, ref rVal, out er))
                        {
                            er = CLanguage.Lan("读错误") + "【" + _scanReg[i].regType.ToString() + _scanReg[i].startAddr.ToString() + "】:" + er;
                            return false;
                        }
                    }

                    //解析扫描寄存器数据

                    string regName = string.Empty;

                    if (_scanReg[i].regType == ERegType.D)
                    {
                        for (int z = 0; z < _scanReg[i].regLen; z++)
                        {
                            regName = _scanReg[i].regType + (_scanReg[i].startAddr + z).ToString();

                            calrRegVal(_scanReg[i], z, 0, regName, rVal, out er);

                            calwRegVal(_scanReg[i], z, 0, regName, rVal, out er);
                        }                        
                    }
                    else if (_scanReg[i].regType == ERegType.W)  //双字节:W100.00--W100.15 
                    {
                        for (int z = 0; z < _scanReg[i].regLen; z++)
                        {
                            for (int w = 0; w < _devPLC.wordNum * 8; w++)
                            {
                                regName = _scanReg[i].regType + (_scanReg[i].startAddr + z).ToString() + "." + w.ToString("D2");

                                calrRegVal(_scanReg[i], z, w, regName, rVal, out er);

                                calwRegVal(_scanReg[i], z, w, regName, rVal, out er);
                            }
                        }
                    }
                    else                            //单字节:M100-M107
                    {                       
                        for (int z = 0; z < _scanReg[i].regLen; z++)
                        {
                            for (int m = 0; m < _devPLC.wordNum * 8; m++)
                            {
                                regName = _scanReg[i].regType + (_scanReg[i].startAddr + z * _devPLC.wordNum * 8 + m).ToString("D2");

                                calrRegVal(_scanReg[i], z, m, regName, rVal, out er);

                                calwRegVal(_scanReg[i], z, m, regName, rVal, out er);
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
        }
        /// <summary>
        /// 写寄存器
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeREG(out string er)
        {
            er = string.Empty;

            try
            {
                while (_writeReg.Count > 0)
                {
                    CWREG reg = null;

                    lock (_synC)
                    {
                        reg = _writeReg.Dequeue();
                    }

                    if (!_wReg.ContainsKey(reg.Des))
                    {
                        er = CLanguage.Lan("[写操作错误]:寄存器不存在") + "[" + reg.Des + "]";
                        return false;
                    }

                    if (_wReg[reg.Des].regType != ERegType.D)
                    {
                        Thread.Sleep(_delayMs);

                        if (!_devPLC.Write(_plcAddr, _wReg[reg.Des].regType, _wReg[reg.Des].startAddr, _wReg[reg.Des].startBin, reg.Val, out er))
                        {
                            Thread.Sleep(_delayMs);

                            if (!_devPLC.Write(_plcAddr, _wReg[reg.Des].regType, _wReg[reg.Des].startAddr, _wReg[reg.Des].startBin, reg.Val, out er))
                            {
                                er = CLanguage.Lan("[写操作错误]:寄存器") +"[" + reg.Des + "]" + er;

                                _writeReg.Enqueue(reg);

                                return false;
                            }
                        }
                    }
                    else
                    {
                        string strHex = reg.Val.ToString("X" + _wReg[reg.Des].regLen * 4) ;

                        Thread.Sleep(_delayMs);

                        if (!_devPLC.Write(_plcAddr, _wReg[reg.Des].regType, _wReg[reg.Des].startAddr, _wReg[reg.Des].regLen,strHex, out er))
                        {
                            Thread.Sleep(_delayMs);

                            if (!_devPLC.Write(_plcAddr, _wReg[reg.Des].regType, _wReg[reg.Des].startAddr, _wReg[reg.Des].regLen, strHex, out er))
                            {
                                er = CLanguage.Lan("[写操作错误]:寄存器") + "[" + reg.Des + "]" + er;

                                _writeReg.Enqueue(reg);

                                return false;
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
        }
        /// <summary>
        /// 多写寄存器
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool writeMutiREG(out string er)
        {
            er = string.Empty;

            try
            {
                while (_writeMutiReg.Count > 0)
                {
                    CWMutiREG reg = null;

                    lock (_synC)
                    {
                        reg = _writeMutiReg.Dequeue();
                    }

                    if (reg.dataType == 0) //写入数值
                    {
                         Thread.Sleep(_delayMs);

                         if (!_devPLC.Write(_plcAddr, reg.regType,reg.Addr, reg.Val, out er))
                         {
                             Thread.Sleep(_delayMs);

                             if (!_devPLC.Write(_plcAddr, reg.regType, reg.Addr, reg.Val, out er))
                             {
                                 er = CLanguage.Lan("[写操作错误]:多寄存器") + "[" + reg.regType.ToString() + reg.Addr.ToString()  + "]" + er;

                                 _writeMutiReg.Enqueue(reg);

                                 return false;
                             }
                         }
                    }
                    else                  //写入16进制字符串
                    {
                        Thread.Sleep(_delayMs);

                        if (!_devPLC.Write(_plcAddr, reg.regType, reg.Addr, reg.Len, reg.HexVal, out er))
                        {
                            Thread.Sleep(_delayMs);

                            if (!_devPLC.Write(_plcAddr, reg.regType, reg.Addr, reg.Len, reg.HexVal, out er))
                            {
                                er =  CLanguage.Lan("[写操作错误]:多寄存器") + "[" + reg.regType.ToString() + reg.Addr.ToString() + "]" + er;

                                _writeMutiReg.Enqueue(reg);

                                return false;
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
        }
        /// <summary>
        /// 解析读寄存器数值
        /// </summary>
        /// <param name="scanReg"></param>
        /// <param name="regName"></param>
        /// <param name="rVal"></param>
        private bool calrRegVal(CREG scanReg,int reg_offset,int bin_offset, string regName, int[] rVal,out string er)
        {
            er = string.Empty;

            try
            {
                  //解析读寄存器
                if (!_rRegMap.ContainsKey(regName))
                    return true;

                string regDes = _rRegMap[regName];

                string regHex = string.Empty;

                int regVal = 0;

                if (_rReg[regDes].regType == ERegType.D)
                {
                    if (_rReg[regDes].regLen > scanReg.regLen - reg_offset)
                    {
                        er = "[" + _rReg[regDes].regName + "]"+ CLanguage.Lan("设置读长度超出扫描寄存器") + "[" + scanReg.regName + "]";
                        return false;
                    }

                    for (int i = 0; i < _rReg[regDes].regLen; i++)
                        regHex = rVal[reg_offset + i].ToString("X4") + regHex;

                    lock (_synC)
                    {
                        _rRegHex[regDes] = regHex;
                        if (_rReg[regDes].regLen <= 2)
                            _rRegVal[regDes] = System.Convert.ToInt32(regHex, 16);
                    }
                }
                else
                {
                    if (_rReg[regDes].regLen > (scanReg.regLen - reg_offset) * 8 * _devPLC.wordNum - bin_offset)
                    {
                        er = "[" + _rReg[regDes].regName + "]" + CLanguage.Lan("设置读长度超出扫描寄存器") + "[" + scanReg.regName + "]";
                        return false;
                    }
                    
                    int offset= reg_offset * 8 * _devPLC.wordNum + bin_offset;                   

                    for (int i = 0; i < _rReg[regDes].regLen; i++)
                    { 
                        if(rVal[offset+i]==1)
                            regVal += (1 << i);
                    }

                    lock (_synC)
                    {
                        _rRegVal[regDes] = regVal;

                        _rRegHex[regDes] = System.Convert.ToString(regVal, 16);
                    }
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
        /// 解析写寄存器数值
        /// </summary>
        /// <param name="scanReg"></param>
        /// <param name="regName"></param>
        /// <param name="rVal"></param>
        private bool calwRegVal(CREG scanReg, int reg_offset, int bin_offset, string regName, int[] rVal, out string er)
        {
            er = string.Empty;

            try
            {
                //解析读寄存器
                if (!_wRegMap.ContainsKey(regName))
                    return true;

                string regDes = _wRegMap[regName];

                string regHex = string.Empty;

                int regVal = 0;

                if (_wReg[regDes].regType == ERegType.D)
                {
                    if (_wReg[regDes].regLen > scanReg.regLen - reg_offset)
                    {
                        er = "[" + _wReg[regDes].regName + "]" + CLanguage.Lan("设置写长度超出扫描寄存器") + "[" + scanReg.regName + "]";
                        return false;
                    }

                    for (int i = 0; i < _wReg[regDes].regLen; i++)
                        regHex = rVal[reg_offset + i].ToString("X4") + regHex;

                    lock (_synC)
                    {
                        _wRegHex[regDes] = regHex;
                        if (_wReg[regDes].regLen <= 2)
                            _wRegVal[regDes] = System.Convert.ToInt32(regHex, 16);
                    }
                }
                else
                {
                    if (_wReg[regDes].regLen > (scanReg.regLen - reg_offset) * 8 * _devPLC.wordNum - bin_offset)
                    {
                        er = "[" + _wReg[regDes].regName + "]" + CLanguage.Lan("设置写长度超出扫描寄存器") + "[" + scanReg.regName + "]";
                        return false;
                    }

                    int offset = reg_offset * 8 * _devPLC.wordNum + bin_offset;

                    for (int i = 0; i < _wReg[regDes].regLen; i++)
                    {
                        if (rVal[offset + i] == 1)
                            regVal += (1 << i);
                    }

                    lock (_synC)
                    {
                        _wRegVal[regDes] = regVal;
                        _wRegHex[regDes] = System.Convert.ToString(regVal, 16);
                    }
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
        /// 设置读写报警
        /// </summary>
        /// <param name="er"></param>
        private void setPLCDataAlarm(string er)
        { 
           OnDataArgs.OnEvented(new CPLCDataArgs(_idNo,_name,er,EMessage.异常));   
        }
        #endregion
    }
}
