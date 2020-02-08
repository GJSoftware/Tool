//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using PSoCProgrammerCOMLib;
//using System.Diagnostics;
//namespace GJ.DEV.Cypress
//{
//    public class CPSoC4Com
//    {
//        #region 枚举
//        /// <summary>
//        /// 通讯协议
//        /// </summary>
//        public enum EProtocol
//        {
//            JTAG = 1,
//            ISSP = 2,
//            I2C = 4,
//            SWD = 8,
//            SPI = 16,
//            SWD_SWV = 32
//        }
//        /// <summary>
//        /// 接口PIN
//        /// </summary>
//        public enum EPIN
//        {
//            PIN5,
//            PIN10
//        }
//        /// <summary>
//        /// 时钟速度MHz
//        /// </summary>
//        public enum EFrequencies
//        {
//            FREQ_48_0 = 0,
//            FREQ_24_0 = 4,
//            FREQ_16_0 = 16,
//            FREQ_03_2 = 24,
//            FREQ_00_4 = 26,
//            FREQ_06_0 = 96,
//            FREQ_12_0 = 132,
//            FREQ_08_0 = 144,
//            FREQ_04_0 = 145,
//            FREQ_01_6 = 152,
//            FREQ_00_8 = 153,
//            FREQ_00_2 = 154,
//            FREQ_01_5 = 192,
//            FREQ_03_0 = 224,
//            FREQ_RESET = 252,
//        }
//        /// <summary>
//        /// 擦除位置
//        /// </summary>
//        public enum ESonosArrays
//        {
//            ARRAY_FLASH = 1,
//            ARRAY_EEPROM = 2,
//            ARRAY_NVL_USER = 4,
//            ARRAY_NVL_FACTORY = 8,
//            ARRAY_NVL_WO_LATCHES = 16,
//            ARRAY_ALL = 31,
//        }
//        /// <summary>
//        /// 模式
//        /// </summary>
//        public enum EMode
//        {
//            /// <summary>
//            /// 同步
//            /// </summary>
//            Sync,
//            /// <summary>
//            /// 异步
//            /// </summary>
//            Async
//        }
//        #endregion

//        #region 类定义
//        /// <summary>
//        /// 消息定义
//        /// </summary>
//        public class CConArgs : EventArgs
//        {
//            public int idNo;

//            public string name;

//            public readonly string info;

//            public readonly bool bErr;

//            public CConArgs(int idNo, string name, string info, bool bErr = false)
//            {
//                this.idNo = idNo;
//                this.name = name;
//                this.info = info;
//                this.bErr = bErr;
//            }
//        }
//        /// <summary>
//        /// 初始化参数
//        /// </summary>
//        public class CPara
//        {
//            /// <summary>
//            /// IC电压
//            /// </summary>
//            public double Voltage = 3.3;
//            /// <summary>
//            /// 通讯协议
//            /// </summary>
//            public EProtocol Protocol = EProtocol.SWD;
//            /// <summary>
//            /// 时钟速度
//            /// </summary>
//            public EFrequencies Freq = EFrequencies.FREQ_03_0;
//            /// <summary>
//            /// 接口PIN
//            /// </summary>
//            public EPIN Pin = EPIN.PIN5;
//            /// <summary>
//            /// 烧录文件路径
//            /// </summary>
//            public string HexFile = string.Empty;
//            /// <summary>
//            /// 动态库
//            /// </summary>
//            public string DllFile = @"C:\Program Files (x86)\Cypress\Programmer\PP_COM_Wrapper.dll";
//        }
//        #endregion

//        #region 定义消息
//        /// <summary>
//        /// 状态事件委托
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        public delegate void EventOnConHander(object sender, CConArgs e);
//        /// <summary>
//        /// 状态消息
//        /// </summary>
//        public event EventOnConHander OnConed;
//        /// <summary>
//        /// 状态触发事件
//        /// </summary>
//        /// <param name="e"></param>
//        private void OnCon(CConArgs e)
//        {
//            if (OnConed != null)
//            {
//                OnConed(this, e);
//            }
//        }
//        #endregion

//        #region 构造函数
//        public CPSoC4Com(int idNo = 0, string name = "CYPD3135-32LQXQT", string dllFile = @"C:\Program Files (x86)\Cypress\Programmer\PP_COM_Wrapper.dll")
//        {
//            this._idNo = idNo;

//            this._name = name;

//            this._dllFile = dllFile;
//        }
//        public override string ToString()
//        {
//            return _name;
//        }
//        #endregion

//        #region 字段
//        //Error constants
//        private const int S_OK = 0;
//        private const int E_FAIL = -1;

//        //Chip Level Protection constants
//        private const int CHIP_PROT_VIRGIN = 0x00;
//        private const int CHIP_PROT_OPEN = 0x01;
//        private const int CHIP_PROT_PROTECTED = 0x02;
//        private const int CHIP_PROT_KILL = 0x04;
//        private const int CHIP_PROT_MASK = 0x0F;

//        /// <summary>
//        /// 编号
//        /// </summary>
//        private int _idNo = 0;
//        /// <summary>
//        /// 名称
//        /// </summary>
//        private string _name = "CYPD3135-32LQXQT";
//        /// <summary>
//        /// 动态库路径
//        /// </summary>
//        private string _dllFile = @"C:\Program Files (x86)\Cypress\Programmer\PP_COM_Wrapper.dll";
//        /// <summary>
//        /// PP_COM_Wrapper.dll动态库
//        /// </summary>
//        private PSoCProgrammerCOM_Object _PPCom = null;
//        /// <summary>
//        /// 烧录文件大小
//        /// </summary>
//        private int _imageSize = 0;
//        /// <summary>
//        /// 校验和
//        /// </summary>
//        private int _checkSum_Privileged = 0;
//        /// <summary>
//        /// 对应烧录端口列表
//        /// </summary>
//        public Dictionary<string, string> PortName = new Dictionary<string, string>();
//        /// <summary>
//        /// 端口状态
//        /// </summary>
//        private bool _IsPort = false;
//        /// <summary>
//        /// 初始化状态
//        /// </summary>
//        private bool _IsInitOK = false;
//        /// <summary>
//        /// 服务器进程Id
//        /// </summary>
//        private int _serverProcessId = 0;
//        #endregion

//        #region 方法
//        /// <summary>
//        /// 初始化烧录器
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <param name="defaultPath"></param>
//        /// <returns></returns>
//        public bool FindPSocId(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                _IsPort = false;

//                _IsInitOK = false;

//                PortName.Clear();

//                if (_PPCom == null)
//                {
//                    _PPCom = new PSoCProgrammerCOM_Object();
//                }

//                if (!GetPorts(out strError))
//                    return false;

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 打开烧录器
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <param name="defaultPath"></param>
//        /// <returns></returns>
//        public bool OpenPSoc(string PSocId, out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                _IsPort = false;

//                _IsInitOK = false;

//                if (!OpenPort(PSocId, out strError))
//                    return false;

//                _IsPort = true;

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 关闭烧录器
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        public bool ClosePSoc(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (!_IsPort)
//                    return true;

//                if (!ClosePort(out strError))
//                    return false;

//                _IsPort = false;

//                _IsInitOK = false;

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 终止检测
//        /// </summary>
//        /// <param name="processId"></param>
//        /// <returns></returns>
//        public bool AbortPSoc(int processId, out string strError)
//        {
//            return _StartSelfTerminator(processId, out strError);
//        }
//        /// <summary>
//        /// 初始化烧录器
//        /// </summary>
//        /// <param name="para"></param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        public bool InitialPSoc(CPara para, out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (!SetPowerVoltage(para.Voltage, out strError))
//                    return false;

//                if (!SetAcquireMode("Power", out strError))
//                    return false;

//                if (!SetProtocol(para.Protocol, out strError))
//                    return false;

//                if (!SetProtocolConnector(para.Pin, out strError))
//                    return false;

//                if (!SetProtocolClock(para.Freq, out strError))
//                    return false;

//                if (!HEX_ReadFile(para.HexFile, out strError))
//                    return false;

//                if (!HEX_ReadChipProtection(out strError))
//                    return false;

//                if (!PowerOn(out strError))
//                    return false;

//                //if (!SetAutoReset(0, out strError))
//                //    return false;

//                _IsInitOK = true;

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 烧录
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        public bool ProgramPSoc(out string strError)
//        {
//            strError = string.Empty;

//            string waitTimes = string.Empty;

//            Stopwatch watcher = new Stopwatch();

//            watcher.Start();

//            try
//            {
//                if (!_IsInitOK)
//                {
//                    strError = "未初始化烧录器参数";
//                    return false;
//                }

//                OnCon(new CConArgs(_idNo, _name, "检测在线烧录IC型号.."));

//                if (!SWD_LineReset(out strError))
//                    return false;

//                //System.Threading.Thread.Sleep(100);

//                if (!DAP_Acquire(out strError))
//                {
//                    OnCon(new CConArgs(_idNo, _name, strError, true));
//                    return false;
//                }

//                //if (!DAP_AcquireChip(out strError))
//                //{
//                //    OnCon(new CConArgs(_idNo, _name, strError, true));
//                //    return false;
//                //}

//                waitTimes = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s";

//                OnCon(new CConArgs(_idNo, _name, "获取在线烧录IC型号:" + waitTimes));

//                object jtagID = null;

//                if (!PSoC4_GetSiliconID(out jtagID, out strError))
//                {
//                    OnCon(new CConArgs(_idNo, _name, strError, true));
//                    return false;
//                }

//                waitTimes = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s";

//                OnCon(new CConArgs(_idNo, _name, "校验烧录IC与Hex文件匹配:" + waitTimes));

//                object hexJtagID = null;

//                if (!HEX_ReadJtagID(out hexJtagID, out strError))
//                {
//                    OnCon(new CConArgs(_idNo, _name, strError, true));
//                    return false;
//                }

//                byte[] hexJtagIDByte, chipJtagIDByte;

//                chipJtagIDByte = jtagID as byte[];

//                hexJtagIDByte = hexJtagID as byte[];

//                // Currently this cycle is commented, since the real silicon ID is not generated by PSoC Creator
//                for (byte i = 0; i < 4; i++)
//                {
//                    if (i == 2) continue; //ignore revision, 11(AA),12(AB),13(AC), etc
//                    if (hexJtagIDByte[i] != chipJtagIDByte[i])
//                    {
//                        strError = "烧录文件Hex与在线芯片不一致";
//                        OnCon(new CConArgs(_idNo, _name, strError, true));
//                        return false;
//                    }
//                }

//                waitTimes = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s";

//                OnCon(new CConArgs(_idNo, _name, "擦除芯片Flash:" + waitTimes));

//                //Check chip level protection here. If PROTECTED then move to OPEN by PSoC4_WriteProtection() API.

//                //Otherwise use PSoC4_EraseAll() - in OPEN/VIRGIN modes.

//                object flashProt, chipProt;

//                byte[] data;

//                if (!PSoC4_ReadProtection(out flashProt, out chipProt, out strError))
//                {
//                    OnCon(new CConArgs(_idNo, _name, strError, true));
//                    return false;
//                }

//                bool IsChipNotProtected = true;

//                data = chipProt as byte[];

//                //Check Result
//                if ((data[0] & CHIP_PROT_PROTECTED) == CHIP_PROT_PROTECTED)
//                {
//                    IsChipNotProtected = false;
//                }

//                if (IsChipNotProtected) //OPEN mode
//                {
//                    //Erase All - Flash and Protection bits. Still be in OPEN mode.
//                    if (!PSoC4_EraseAll(out strError))
//                    {
//                        OnCon(new CConArgs(_idNo, _name, strError, true));
//                        return false;
//                    }
//                }
//                else
//                {
//                    //Move to OPEN from PROTECTED. It automatically erases Flash and its Protection bits.
//                    if (!PSoC4_WriteProtection(out strError))
//                    {
//                        OnCon(new CConArgs(_idNo, _name, strError, true));
//                        return false;
//                    }

//                    //Need to reacquire chip here to boot in OPEN mode.

//                    //ChipLevelProtection is applied only after Reset.
//                    if (!DAP_Acquire(out strError))
//                    {
//                        OnCon(new CConArgs(_idNo, _name, strError, true));
//                        return false;
//                    }
//                }

//                waitTimes = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s";

//                OnCon(new CConArgs(_idNo, _name, "获取芯片Privilege CheckSum:" + waitTimes));

//                int checkSum_Privileged = 0;

//                if (!PSoC4_CheckSum(out checkSum_Privileged, out strError))
//                {
//                    OnCon(new CConArgs(_idNo, _name, strError, true));
//                    return false;
//                }

//                waitTimes = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s";

//                OnCon(new CConArgs(_idNo, _name, "烧录芯片Flash:" + waitTimes));

//                int rowsPerArray = 0;

//                int rowSize = 0;

//                if (!PSoC4_GetFlashInfo(out rowsPerArray, out rowSize, out strError))
//                {
//                    OnCon(new CConArgs(_idNo, _name, strError, true));
//                    return false;
//                }

//                int totalRows = _imageSize / rowSize;

//                for (int i = 0; i < totalRows; i++)
//                {
//                    if (!PSoC4_ProgramRowFromHex(i, out strError))
//                    {
//                        OnCon(new CConArgs(_idNo, _name, strError, true));
//                        return false;
//                    }
//                }

//                waitTimes = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s";

//                OnCon(new CConArgs(_idNo, _name, "校验烧录芯片Flash:" + waitTimes));

//                rowsPerArray = 0;

//                rowSize = 0;

//                if (!PSoC4_GetFlashInfo(out rowsPerArray, out rowSize, out strError))
//                {
//                    OnCon(new CConArgs(_idNo, _name, strError, true));
//                    return false;
//                }

//                totalRows = _imageSize / rowSize;

//                for (int i = 0; i < totalRows; i++)
//                {
//                    if (!PSoC4_VerifyRowFromHex(i, out strError))
//                    {
//                        OnCon(new CConArgs(_idNo, _name, strError, true));
//                        return false;
//                    }
//                }

//                waitTimes = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s";

//                OnCon(new CConArgs(_idNo, _name, "写入烧录芯片写保护:" + waitTimes));

//                if (!PSoC4_ProtectAll(out strError))
//                {
//                    OnCon(new CConArgs(_idNo, _name, strError, true));
//                    return false;
//                }

//                waitTimes = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s";

//                OnCon(new CConArgs(_idNo, _name, "校验烧录芯片写保护:" + waitTimes));

//                if (!PSoC4_VerifyProtect(out strError))
//                {
//                    OnCon(new CConArgs(_idNo, _name, strError, true));
//                    return false;
//                }

//                waitTimes = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s";

//                OnCon(new CConArgs(_idNo, _name, "校验烧录芯片与Hex文件:" + waitTimes));

//                int checkSum_UserPrivileged = 0;

//                if (!PSoC4_CheckSum(out checkSum_UserPrivileged, out strError))
//                {
//                    OnCon(new CConArgs(_idNo, _name, strError, true));
//                    return false;
//                }

//                short hexChecksum = 0;

//                if (!HEX_ReadChecksum(out hexChecksum, out strError))
//                {
//                    OnCon(new CConArgs(_idNo, _name, strError, true));
//                    return false;
//                }

//                int checkSum_User = checkSum_UserPrivileged - checkSum_Privileged; //find checksum of User Flash rows

//                checkSum_User &= 0xFFFF;

//                int hexChecks = hexChecksum & 0xFFFF;

//                if (checkSum_User != hexChecks)
//                {
//                    strError = "Mismatch of Checksum: Expected 0x" + checkSum_User.ToString("X") + ", Got 0x" + hexChecksum.ToString("X");
//                    OnCon(new CConArgs(_idNo, _name, strError, true));
//                    return false;
//                }

//                //waitTimes = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s";

//                //OnCon(new CConArgs(_idNo, _name, "释放芯片烧录IC:" + waitTimes));

//                //if (!DAP_ReleaseChip(out strError))
//                //{
//                //    OnCon(new CConArgs(_idNo, _name, strError, true));
//                //    return false;
//                //}

//                waitTimes = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s";

//                OnCon(new CConArgs(_idNo, _name, "烧录芯片IC完成:" + waitTimes));

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//            finally
//            {
//                watcher.Stop();
//            }
//        }
//        /// <summary>
//        /// STEP0:总线复位
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        public bool STEP_LineReset(out string strError)
//        {

//            return SWD_LineReset(out strError);
//        }
//        /// <summary>
//        /// STEP2:检测烧录IC
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        public bool STEP_Acquire(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {

//                if (!DAP_Acquire(out strError))
//                    return false;

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// STEP3:校验烧录IC与Hex文件匹配
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        public bool STEP_CheckICAndFile(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                object jtagID = null;

//                if (!PSoC4_GetSiliconID(out jtagID, out strError))
//                    return false;

//                object hexJtagID = null;

//                if (!HEX_ReadJtagID(out hexJtagID, out strError))
//                    return false;

//                byte[] hexJtagIDByte, chipJtagIDByte;

//                chipJtagIDByte = jtagID as byte[];

//                hexJtagIDByte = hexJtagID as byte[];

//                // Currently this cycle is commented, since the real silicon ID is not generated by PSoC Creator
//                for (byte i = 0; i < 4; i++)
//                {
//                    if (i == 2) continue; //ignore revision, 11(AA),12(AB),13(AC), etc
//                    if (hexJtagIDByte[i] != chipJtagIDByte[i])
//                    {
//                        strError = "烧录文件Hex与在线芯片不一致";
//                        return false;
//                    }
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// STEP4:擦除Flash
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        public bool STEP_EraseFlash(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                //Check chip level protection here. If PROTECTED then move to OPEN by PSoC4_WriteProtection() API.

//                //Otherwise use PSoC4_EraseAll() - in OPEN/VIRGIN modes.

//                object flashProt, chipProt;

//                byte[] data;

//                if (!PSoC4_ReadProtection(out flashProt, out chipProt, out strError))
//                    return false;

//                bool IsChipNotProtected = true;

//                data = chipProt as byte[];

//                //Check Result
//                if ((data[0] & CHIP_PROT_PROTECTED) == CHIP_PROT_PROTECTED)
//                {
//                    IsChipNotProtected = false;
//                }

//                if (IsChipNotProtected) //OPEN mode
//                {
//                    //Erase All - Flash and Protection bits. Still be in OPEN mode.
//                    if (!PSoC4_EraseAll(out strError))
//                        return false;
//                }
//                else
//                {
//                    //Move to OPEN from PROTECTED. It automatically erases Flash and its Protection bits.
//                    if (!PSoC4_WriteProtection(out strError))
//                        return false;

//                    //Need to reacquire chip here to boot in OPEN mode.

//                    //ChipLevelProtection is applied only after Reset.
//                    if (!DAP_Acquire(out strError))
//                        return false;
//                }

//                _checkSum_Privileged = 0;

//                if (!PSoC4_CheckSum(out _checkSum_Privileged, out strError))
//                    return false;

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// STEP5:写入Flash
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        public bool STEP_WriteFlash(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {

//                int rowsPerArray = 0;

//                int rowSize = 0;

//                if (!PSoC4_GetFlashInfo(out rowsPerArray, out rowSize, out strError))
//                    return false;

//                int totalRows = _imageSize / rowSize;

//                for (int i = 0; i < totalRows; i++)
//                {
//                    if (!PSoC4_ProgramRowFromHex(i, out strError))
//                        return false;
//                }

//                //校验烧录芯片Flash---???

//                rowsPerArray = 0;

//                rowSize = 0;

//                if (!PSoC4_GetFlashInfo(out rowsPerArray, out rowSize, out strError))
//                    return false;

//                totalRows = _imageSize / rowSize;

//                for (int i = 0; i < totalRows; i++)
//                {
//                    if (!PSoC4_VerifyRowFromHex(i, out strError))
//                        return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// STEP6:写入写保护
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        public bool STEP_WriteProtect(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (!PSoC4_ProtectAll(out strError))
//                    return false;

//                if (!PSoC4_VerifyProtect(out strError))
//                    return false;

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// STEP7:校验和检查
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        public bool STEP_CheckSum(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                int checkSum_UserPrivileged = 0;

//                if (!PSoC4_CheckSum(out checkSum_UserPrivileged, out strError))
//                    return false;

//                short hexChecksum = 0;

//                if (!HEX_ReadChecksum(out hexChecksum, out strError))
//                    return false;

//                int checkSum_User = checkSum_UserPrivileged - _checkSum_Privileged; //find checksum of User Flash rows

//                checkSum_User &= 0xFFFF;

//                int hexChecks = hexChecksum & 0xFFFF;

//                if (checkSum_User != hexChecks)
//                {
//                    strError = "Mismatch of Checksum: Expected 0x" + checkSum_User.ToString("X") + ", Got 0x" + hexChecksum.ToString("X");
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        #endregion

//        #region 烧录器方法
//        /// <summary>
//        /// 获取烧录器列表
//        /// </summary>
//        /// <param name="DevProg"></param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool GetPorts(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_Object未初始化";
//                    return false;
//                }

//                object posts = null;


//                int hr = _PPCom.GetPorts(out posts, out strError);

//                if (hr < 0)
//                    return false;

//                string[] portName = posts as string[];

//                if (portName.Length == 0)
//                {
//                    strError = "电脑检测不到烧录器";
//                    return false;
//                }

//                for (int i = 0; i < portName.Length; i++)
//                {
//                    if (portName[i].StartsWith("MiniProg3"))
//                    {
//                        string id = portName[i].Split('/')[1];

//                        PortName.Add(id, portName[i]);
//                    }
//                }

//                if (PortName.Count == 0)
//                {
//                    strError = "电脑检测不到烧录器MiniProg3";
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 打开烧录器
//        /// </summary>
//        /// <param name="PSoCId"></param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool OpenPort(string PSoCId, out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                if (!PortName.ContainsKey(PSoCId))
//                {
//                    strError = "找不到烧录器ID号[" + PSoCId + "]";
//                    return false;
//                }

//                int hr = _PPCom.OpenPort(PortName[PSoCId], out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 关闭烧录器
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool ClosePort(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.ClosePort(out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 端口是否打开?
//        /// </summary>
//        /// <returns></returns>
//        private bool IsPortOpen(out int isOpen, out string strError)
//        {
//            isOpen = 0;

//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.IsPortOpen(out isOpen, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 设置IC电压值
//        /// </summary>
//        /// <param name="voltage"></param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool SetPowerVoltage(double voltage, out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.SetPowerVoltage(voltage.ToString(), out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// IC电压PowerOn
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool PowerOn(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.PowerOn(out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// IC电压PowerOn
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool PowerOff(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.PowerOff(out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 设置通讯协议
//        /// </summary>
//        /// <param name="protocol"></param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool SetProtocol(EProtocol protocol, out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.SetProtocol((enumInterfaces)protocol, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 设置接口PIN类型
//        /// </summary>
//        /// <param name="pin"></param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool SetProtocolConnector(EPIN pin, out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.SetProtocolConnector((int)pin, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 设置时钟速度(MHz)
//        /// </summary>
//        /// <param name="freq"></param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool SetProtocolClock(EFrequencies freq, out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.SetProtocolClock((enumFrequencies)freq, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 导入烧录文件(Hex)
//        /// </summary>
//        /// <param name="filePath"></param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool HEX_ReadFile(string filePath, out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.HEX_ReadFile(filePath, out _imageSize, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 读取烧录文件
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool HEX_ReadChipProtection(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                object data = null;

//                int hr = _PPCom.HEX_ReadChipProtection(out data, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                byte hex_chipProt = (data as byte[])[0];

//                if (hex_chipProt == CHIP_PROT_VIRGIN)
//                {
//                    strError = "HEX文件不允许对芯片烧录操作";
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 设置模式
//        /// </summary>
//        /// <param name="mode">Reset:外部供电;Power:烧录器供电;PowerDetect:自动检测供电</param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool SetAcquireMode(string mode, out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.SetAcquireMode(mode, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 获取IC
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool DAP_Acquire(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.DAP_Acquire(out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 获取IC
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool DAP_AcquireChip(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.DAP_AcquireChip(out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 获取芯片ID
//        /// </summary>
//        /// <param name="jtagID"></param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool PSoC4_GetSiliconID(out object jtagID, out string strError)
//        {
//            jtagID = null;

//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.PSoC4_GetSiliconID(out jtagID, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 从HEX文件读芯片ID
//        /// </summary>
//        /// <param name="hexJtagID"></param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool HEX_ReadJtagID(out object hexJtagID, out string strError)
//        {
//            hexJtagID = null;

//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.HEX_ReadJtagID(out hexJtagID, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 读取写保护
//        /// </summary>
//        /// <param name="flashProt"></param>
//        /// <param name="chipProt"></param>
//        /// <returns></returns>
//        private bool PSoC4_ReadProtection(out object flashProt, out object chipProt, out string strError)
//        {
//            flashProt = null;

//            chipProt = null;

//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.PSoC4_ReadProtection(out flashProt, out chipProt, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 写保护
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool PSoC4_WriteProtection(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                //Move to OPEN from PROTECTED. It automatically erases Flash and its Protection bits.
//                byte[] flashProt = new byte[] { }; // do not care in PROTECTED mode

//                byte[] chipProt = new byte[] { CHIP_PROT_OPEN }; //move to OPEN

//                int hr = _PPCom.PSoC4_WriteProtection(flashProt, chipProt, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 擦除内存
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool PSoC4_EraseAll(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.PSoC4_EraseAll(out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// Find checksum of Privileged Flash.
//        /// Will be used in calculation of User CheckSum later
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool PSoC4_CheckSum(out int checkSum, out string strError)
//        {
//            checkSum = 0;

//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.PSoC4_CheckSum(0x8000, out checkSum, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 获取Flash信息
//        /// </summary>
//        /// <param name="rowsPerArray"></param>
//        /// <param name="?"></param>
//        /// <returns></returns>
//        private bool PSoC4_GetFlashInfo(out int rowsPerArray, out int rowSize, out string strError)
//        {
//            rowsPerArray = 0;

//            rowSize = 0;

//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.PSoC4_GetFlashInfo(out rowsPerArray, out rowSize, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 烧录行
//        /// </summary>
//        /// <param name="rowID"></param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool PSoC4_ProgramRowFromHex(int rowID, out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.PSoC4_ProgramRowFromHex(rowID, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 校验行
//        /// </summary>
//        /// <param name="rowID"></param>
//        /// <param name="verResult"></param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool PSoC4_VerifyRowFromHex(int rowID, out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int verResult = 0;

//                int hr = _PPCom.PSoC4_VerifyRowFromHex(rowID, out verResult, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                if (verResult == 0)
//                {
//                    strError = "Verification failed on " + rowID + " row.";
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 加密内存
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool PSoC4_ProtectAll(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.PSoC4_ProtectAll(out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 校验加密
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool PSoC4_VerifyProtect(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.PSoC4_VerifyProtect(out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 烧录文件校验和
//        /// </summary>
//        /// <param name="hexChecksum"></param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool HEX_ReadChecksum(out short hexChecksum, out string strError)
//        {
//            hexChecksum = 0;

//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.HEX_ReadChecksum(out hexChecksum, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 释放IC
//        /// </summary>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool DAP_ReleaseChip(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.DAP_ReleaseChip(out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 释放进程COM
//        /// </summary>
//        /// <param name="clientProcessId"></param>
//        /// <returns></returns>
//        private bool _StartSelfTerminator(int clientProcessId, out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                _serverProcessId = _PPCom._StartSelfTerminator(clientProcessId);

//                if (_serverProcessId < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 自动复位
//        /// </summary>
//        /// <param name="autoRest">1:自动复位</param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool SetAutoReset(int autoRest, out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.SetAutoReset(autoRest, out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        /// <summary>
//        /// 复位总线
//        /// </summary>
//        /// <param name="autoRest">1:自动复位</param>
//        /// <param name="strError"></param>
//        /// <returns></returns>
//        private bool SWD_LineReset(out string strError)
//        {
//            strError = string.Empty;

//            try
//            {
//                if (_PPCom == null)
//                {
//                    strError = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
//                    return false;
//                }

//                int hr = _PPCom.SWD_LineReset(out strError);

//                if (hr < 0)
//                {
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                strError = ex.ToString();
//                return false;
//            }
//        }
//        #endregion

//    }
//}
