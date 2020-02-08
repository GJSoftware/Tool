using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading; 
namespace GJ.DEV.Cypress
{
    #region 程序集 PP_COM_Wrapper.dll, v2.0.50727
    // C:\Program Files (x86)\Cypress\Programmer\PP_COM_Wrapper.dll
    //IC:CYPD3135-32LQXQT
    #endregion

    public class CPSoC4
    {
        #region 枚举
        /// <summary>
        /// 通讯协议
        /// </summary>
        public enum EProtocol
        {
            JTAG = 1,
            ISSP = 2,
            I2C = 4,
            SWD = 8,
            SPI = 16,
            SWD_SWV = 32
        }
        /// <summary>
        /// 接口PIN
        /// </summary>
        public enum EPIN
        { 
           PIN5,
           PIN10
        }
        /// <summary>
        /// 时钟速度MHz
        /// </summary>
        public enum EFrequencies
        {
            FREQ_48_0 = 0,
            FREQ_24_0 = 4,
            FREQ_16_0 = 16,
            FREQ_03_2 = 24,
            FREQ_00_4 = 26,
            FREQ_06_0 = 96,
            FREQ_12_0 = 132,
            FREQ_08_0 = 144,
            FREQ_04_0 = 145,
            FREQ_01_6 = 152,
            FREQ_00_8 = 153,
            FREQ_00_2 = 154,
            FREQ_01_5 = 192,
            FREQ_03_0 = 224,
            FREQ_RESET = 252,

        }
        /// <summary>
        /// 擦除位置
        /// </summary>
        public enum ESonosArrays
        {
            ARRAY_FLASH = 1,
            ARRAY_EEPROM = 2,
            ARRAY_NVL_USER = 4,
            ARRAY_NVL_FACTORY = 8,
            ARRAY_NVL_WO_LATCHES = 16,
            ARRAY_ALL = 31,
        }
        /// <summary>
        /// 模式
        /// </summary>
        public enum EMode
        { 
           /// <summary>
           /// 同步
           /// </summary>
           Sync,
           /// <summary>
           /// 异步
           /// </summary>
           Async
        }
        #endregion

        #region 类定义
        /// <summary>
        /// 消息定义
        /// </summary>
        public class CConArgs : EventArgs
        {
            public int idNo;

            public string name;

            public readonly string info;

            public readonly bool bErr;

            public CConArgs(int idNo, string name, string info, bool bErr=false)
            {
                this.idNo = idNo;
                this.name = name;
                this.info = info;
                this.bErr = bErr;
            }
        }
        /// <summary>
        /// 初始化参数
        /// </summary>
        public class CPara
        {
            /// <summary>
            /// IC电压
            /// </summary>
            public double Voltage = 3.3;
            /// <summary>
            /// 通讯协议
            /// </summary>
            public EProtocol Protocol = EProtocol.SWD;
            /// <summary>
            /// 时钟速度
            /// </summary>
            public EFrequencies Freq = EFrequencies.FREQ_03_0;
            /// <summary>
            /// 接口PIN
            /// </summary>
            public EPIN Pin = EPIN.PIN5;
            /// <summary>
            /// 烧录文件路径
            /// </summary>
            public string HexFile = string.Empty;
            /// <summary>
            /// 动态库
            /// </summary>
            public string DllFile = @"C:\Program Files (x86)\Cypress\Programmer\PP_COM_Wrapper.dll";
        }
        #endregion

        #region 定义消息
        /// <summary>
        /// 状态事件委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventOnConHander(object sender, CConArgs e);
        /// <summary>
        /// 状态消息
        /// </summary>
        public event EventOnConHander OnConed;
        /// <summary>
        /// 状态触发事件
        /// </summary>
        /// <param name="e"></param>
        private void OnCon(CConArgs e)
        {
            if (OnConed != null)
            {
                OnConed(this, e);
            }
        }
        #endregion

        #region 构造函数
        public CPSoC4(int idNo = 0, string name = "CYPD3135-32LQXQT", string dllFile = @"C:\Program Files (x86)\Cypress\Programmer\PP_COM_Wrapper.dll")
        {
           this._idNo=idNo;

           this._name=name;

           this._dllFile = dllFile; 
        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 字段
        //Error constants
        private const int S_OK = 0;
        private const int E_FAIL = -1;

        //Chip Level Protection constants
        private const int CHIP_PROT_VIRGIN = 0x00;
        private const int CHIP_PROT_OPEN = 0x01;
        private const int CHIP_PROT_PROTECTED = 0x02;
        private const int CHIP_PROT_KILL = 0x04;
        private const int CHIP_PROT_MASK = 0x0F;

        /// <summary>
        /// 编号
        /// </summary>
        private int _idNo=0;
        /// <summary>
        /// 名称
        /// </summary>
        private string _name = "CYPD3135-32LQXQT";
        /// <summary>
        /// 动态库路径
        /// </summary>
        private string _dllFile = @"C:\Program Files (x86)\Cypress\Programmer\PP_COM_Wrapper.dll";
        /// <summary>
        /// PP_COM_Wrapper.dll动态库
        /// </summary>
        private object _PPCom = null;
        /// <summary>
        /// 烧录文件大小
        /// </summary>
        private int _imageSize = 0;
        /// <summary>
        /// 校验和
        /// </summary>
        private int _checkSum_Privileged = 0;
        /// <summary>
        /// 对应烧录端口列表
        /// </summary>
        public Dictionary<string, string> PortName = new Dictionary<string, string>();
        /// <summary>
        /// 端口状态
        /// </summary>
        public bool _IsPort = false;
        /// <summary>
        /// 初始化状态
        /// </summary>
        public bool _IsInitOK = false;
        /// <summary>
        /// 进程ID
        /// </summary>
        private int _serverProcessId = 0;
        #endregion

        #region 属性
        /// <summary>
        /// 进程ID
        /// </summary>
        public int ServerProcessId
        {
            get { return _serverProcessId; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 初始化烧录器
        /// </summary>
        /// <param name="er"></param>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        public bool FindPSocId(out string er)
        {
            er = string.Empty;

            try
            {
                _IsPort = false;

                _IsInitOK = false;

                PortName.Clear();

                if (!LoadAPI(out er, _dllFile))
                    return false;

                //int clientProcessId = Process.GetCurrentProcess().Id;

                int clientProcessId = 0; //需为0

                if (!_StartSelfTerminator(clientProcessId, out er))
                    return false;

                if (!GetPorts(out er))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 打开烧录器
        /// </summary>
        /// <param name="er"></param>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        public bool OpenPSoc(string PSocId,out string er)
        {
            er = string.Empty;

            try
            {
                _IsPort = false;

                _IsInitOK = false;

                if (!OpenPort(PSocId, out er))                
                    return false;

                _IsPort = true;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 关闭烧录器
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ClosePSoc(out string er)
        {
            er = string.Empty;

            try
            {
                if (!_IsPort)
                    return true;

                if (!ClosePort(out er))
                    return false;

                _IsPort = false;

                _IsInitOK = false;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 终止检测
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public bool AbortPSoc(int processId,out string er)
        {
            return _StartSelfTerminator(processId, out er); 
        }
        /// <summary>
        /// 初始化烧录器
        /// </summary>
        /// <param name="para"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool InitialPSoc(CPara para, out string er)
        {
            er = string.Empty;

            try
            {

                if (!SetPowerVoltage(para.Voltage, out er))
                    return false;

                if (!PowerOn(out er))
                    return false;                

                if (!SetProtocol(para.Protocol, out er))
                    return false;

                if (!SetProtocolConnector(para.Pin, out er))
                    return false;

                if (!SetProtocolClock(para.Freq, out er))
                    return false;

                if (!HEX_ReadFile(para.HexFile, out er))
                    return false;

                if(!HEX_ReadChipProtection(out er))
                    return false;

                //if (!SetAutoReset(1, out er))
                //    return false;

                if (!SetAcquireMode("SoftReset", out er))
                    return false;

                _IsInitOK = true;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;                
            }
        }
        /// <summary>
        /// 烧录
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool ProgramPSoc(out string er)
        {
            er=string.Empty;

            string waitTimes = string.Empty;

            Stopwatch watcher = new Stopwatch();

            watcher.Start();

            try
            {
                if (!_IsInitOK)
                {
                    er = "未初始化烧录器参数";
                    return false;
                }

                OnCon(new CConArgs(_idNo, _name, "检测在线烧录IC型号.."));

                if (!DAP_Acquire(out er))
                {
                    OnCon(new CConArgs(_idNo, _name, er, true));
                    return false;
                }

                string Info = string.Empty;

                object siliconObj = null;

                if (!PSoC4_GetSiliconID(out siliconObj, out er))
                {
                    OnCon(new CConArgs(_idNo, _name, er, true));
                    return false;
                }

                byte[] chipByte;

                chipByte = siliconObj as byte[];

                if (chipByte.Length >= 4)
                {
                    string siliconId = ((chipByte[0] << 8) + chipByte[1]).ToString("X4");

                    string version = chipByte[2].ToString("X2");

                    string family = chipByte[3].ToString("X2");

                    Info = "Silicon:" + siliconId + " Family:" + family + " Revison:" + version;
                }

                waitTimes = "[" + ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s]";

                OnCon(new CConArgs(_idNo, _name, "获取在线烧录IC型号[" +  Info + "]:" + waitTimes));

                object jtagID = null;

                if (!PSoC4_GetSiliconID(out jtagID, out er))
                {
                    OnCon(new CConArgs(_idNo, _name, er, true));
                    return false;
                }

                waitTimes = "[" + ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s]";

                OnCon(new CConArgs(_idNo, _name, "校验烧录IC与Hex文件匹配:" + waitTimes));

                object hexJtagID = null;

                if (!HEX_ReadJtagID(out hexJtagID, out er))
                {
                    OnCon(new CConArgs(_idNo, _name, er, true));
                    return false;
                }

                byte[] hexJtagIDByte, chipJtagIDByte;

                chipJtagIDByte = jtagID as byte[];

                hexJtagIDByte = hexJtagID as byte[];

                // Currently this cycle is commented, since the real silicon ID is not generated by PSoC Creator
                for (byte i = 0; i < 4; i++)
                {
                    if (i == 2) continue; //ignore revision, 11(AA),12(AB),13(AC), etc
                    if (hexJtagIDByte[i] != chipJtagIDByte[i])
                    {
                        er = "烧录文件Hex与在线芯片不一致";
                        OnCon(new CConArgs(_idNo, _name, er, true));
                        return false;
                    }
                }

                waitTimes = "[" + ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s]";

                OnCon(new CConArgs(_idNo, _name, "擦除芯片Flash:" + waitTimes));

                //Check chip level protection here. If PROTECTED then move to OPEN by PSoC4_WriteProtection() API.

                //Otherwise use PSoC4_EraseAll() - in OPEN/VIRGIN modes.

                object flashProt, chipProt;

                byte[] data;

                if (!PSoC4_ReadProtection(out flashProt, out chipProt, out er))
                {
                    OnCon(new CConArgs(_idNo, _name, er, true));
                    return false;
                }

                bool IsChipNotProtected = true;

                data = chipProt as byte[];

                //Check Result
                if ((data[0] & CHIP_PROT_PROTECTED) == CHIP_PROT_PROTECTED)
                {
                    IsChipNotProtected = false;
                }

                if (IsChipNotProtected) //OPEN mode
                {
                    //Erase All - Flash and Protection bits. Still be in OPEN mode.
                    if (!PSoC4_EraseAll(out er))
                    {
                        OnCon(new CConArgs(_idNo, _name, er, true));
                        return false;
                    }
                }
                else
                {
                    //Move to OPEN from PROTECTED. It automatically erases Flash and its Protection bits.
                    if (!PSoC4_WriteProtection(out er))
                    {
                        OnCon(new CConArgs(_idNo, _name, er, true));
                        return false;
                    }

                    //Need to reacquire chip here to boot in OPEN mode.

                    //ChipLevelProtection is applied only after Reset.
                    if (!DAP_Acquire(out er))
                    {
                        OnCon(new CConArgs(_idNo, _name, er, true));
                        return false;
                    }
                }

                waitTimes = "[" + ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s]";

                OnCon(new CConArgs(_idNo, _name, "获取芯片Privilege CheckSum:" + waitTimes));

                int checkSum_Privileged = 0;

                if (!PSoC4_CheckSum(out checkSum_Privileged, out er))
                {
                    OnCon(new CConArgs(_idNo, _name, er, true));
                    return false;
                }

                waitTimes = "[" + ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s]";

                OnCon(new CConArgs(_idNo, _name, "烧录芯片Flash:" + waitTimes));

                int rowsPerArray = 0;

                int rowSize = 0;

                if (!PSoC4_GetFlashInfo(out rowsPerArray, out rowSize, out er))
                {
                    OnCon(new CConArgs(_idNo, _name, er, true));
                    return false;
                }

                int totalRows = _imageSize / rowSize;

                for (int i = 0; i < totalRows; i++)
                {
                    if (!PSoC4_ProgramRowFromHex(i, out er))
                    {
                        OnCon(new CConArgs(_idNo, _name, er, true));
                        return false;
                    }
                }

                waitTimes = "[" + ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s]";

                OnCon(new CConArgs(_idNo, _name, "校验烧录芯片Flash:" + waitTimes));

                rowsPerArray = 0;

                rowSize = 0;

                if (!PSoC4_GetFlashInfo(out rowsPerArray, out rowSize, out er))
                {
                    OnCon(new CConArgs(_idNo, _name, er, true));
                    return false;
                }

                totalRows = _imageSize / rowSize;

                for (int i = 0; i < totalRows; i++)
                {
                    if (!PSoC4_VerifyRowFromHex(i, out er))
                    {
                        OnCon(new CConArgs(_idNo, _name, er, true));
                        return false;
                    }
                }

                waitTimes = "[" + ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s]";

                OnCon(new CConArgs(_idNo, _name, "写入烧录芯片写保护:" + waitTimes));

                if (!PSoC4_ProtectAll(out er))
                {
                    OnCon(new CConArgs(_idNo, _name, er, true));
                    return false;
                }

                waitTimes = "[" + ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s]";

                OnCon(new CConArgs(_idNo, _name, "校验烧录芯片写保护:" + waitTimes));

                if (!PSoC4_VerifyProtect(out er))
                {
                    OnCon(new CConArgs(_idNo, _name, er, true));
                    return false;
                }

                waitTimes = "[" + ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s]";

                OnCon(new CConArgs(_idNo, _name, "校验烧录芯片与Hex文件:" + waitTimes));

                int checkSum_UserPrivileged = 0;

                if (!PSoC4_CheckSum(out checkSum_UserPrivileged, out er))
                {
                    OnCon(new CConArgs(_idNo, _name, er, true));
                    return false;
                }

                short hexChecksum = 0;

                if (!HEX_ReadChecksum(out hexChecksum, out er))
                {
                    OnCon(new CConArgs(_idNo, _name, er, true));
                    return false;
                }

                int checkSum_User = checkSum_UserPrivileged - checkSum_Privileged; //find checksum of User Flash rows

                checkSum_User &= 0xFFFF;

                int hexChecks = hexChecksum & 0xFFFF;

                if (checkSum_User != hexChecks)
                {
                    er = "Mismatch of Checksum: Expected 0x" + checkSum_User.ToString("X") + ", Got 0x" + hexChecksum.ToString("X");
                    OnCon(new CConArgs(_idNo, _name, er, true));
                    return false;
                }

                //waitTimes = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s";

                //OnCon(new CConArgs(_idNo, _name, "释放芯片烧录IC:" + waitTimes));

                //if (!DAP_ReleaseChip(out er))
                //{
                //    OnCon(new CConArgs(_idNo, _name, er, true));
                //    return false;
                //}


                waitTimes = "[" + ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s]";

                OnCon(new CConArgs(_idNo, _name, "烧录芯片IC完成:" + waitTimes));


                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                watcher.Stop(); 
            }
        }
        /// <summary>
        /// STEP0:总线复位
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool STEP_LineReset(out string er)
        {
            return SWD_LineReset(out er);
        }
        /// <summary>
        /// STEP1:读IO信号
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool STEP_IOR_RAW(out string er)
        {
            return SWDIOR_RAW(out er); 
        }
        /// <summary>
        /// STEP2_0:检测烧录IC
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool STEP_Acquire(out string er)
        {
            er = string.Empty;

            try
            {

                if (!DAP_Acquire(out er))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }               
        }
        /// <summary>
        /// STEP2_1:获取IC信息
        /// </summary>
        /// <param name="siliconId"></param>
        /// <param name="family"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public bool STEP_GetDeviceInfo(out string siliconId, out string version, out string family, out string er)
        {
            siliconId = string.Empty;

            family = string.Empty;

            version = string.Empty;

            try
            {
                object siliconObj = null;

                if (!PSoC4_GetSiliconID(out siliconObj, out er))
                    return false;

                byte[] chipByte;

                chipByte = siliconObj as byte[];

                if (chipByte.Length >=4)
                {
                    siliconId = ((chipByte[0] << 8) + chipByte[1]).ToString("X4");

                    version = chipByte[2].ToString("X2");

                    family = chipByte[3].ToString("X2");
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
        /// STEP3:校验烧录IC与Hex文件匹配
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool STEP_CheckICAndFile(out string er)
        {
            er = string.Empty;

            try
            {
                object jtagID = null;

                if (!PSoC4_GetSiliconID(out jtagID, out er))
                    return false;

                object hexJtagID = null;

                if (!HEX_ReadJtagID(out hexJtagID, out er))
                    return false;

                byte[] hexJtagIDByte, chipJtagIDByte;

                chipJtagIDByte = jtagID as byte[];

                hexJtagIDByte = hexJtagID as byte[];

                // Currently this cycle is commented, since the real silicon ID is not generated by PSoC Creator
                for (byte i = 0; i < 4; i++)
                {
                    if (i == 2) continue; //ignore revision, 11(AA),12(AB),13(AC), etc
                    if (hexJtagIDByte[i] != chipJtagIDByte[i])
                    {
                        er = "Programming Hex is different from device ID";
                        return false;
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
        /// STEP4:擦除Flash
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool STEP_EraseFlash(out string er)
        {
            er = string.Empty;

            try
            {
                //Check chip level protection here. If PROTECTED then move to OPEN by PSoC4_WriteProtection() API.

                //Otherwise use PSoC4_EraseAll() - in OPEN/VIRGIN modes.

                object flashProt, chipProt;

                byte[] data;

                if (!PSoC4_ReadProtection(out flashProt, out chipProt, out er))
                    return false;
                
                bool IsChipNotProtected = true;

                data = chipProt as byte[];

                //Check Result
                if ((data[0] & CHIP_PROT_PROTECTED) == CHIP_PROT_PROTECTED)
                {
                    IsChipNotProtected = false;
                }

                if (IsChipNotProtected) //OPEN mode
                {
                    //Erase All - Flash and Protection bits. Still be in OPEN mode.
                    if (!PSoC4_EraseAll(out er))
                        return false;                    
                }
                else
                {
                    //Move to OPEN from PROTECTED. It automatically erases Flash and its Protection bits.
                    if (!PSoC4_WriteProtection(out er))
                        return false;

                    //Need to reacquire chip here to boot in OPEN mode.

                    //ChipLevelProtection is applied only after Reset.
                    if (!DAP_Acquire(out er))
                        return false;                    
                }

                _checkSum_Privileged = 0;

                if (!PSoC4_CheckSum(out _checkSum_Privileged, out er))
                    return false;
                
                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// STEP5:写入Flash
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool STEP_WriteFlash(out string er)
        {
            er = string.Empty;

            try
            {

                int rowsPerArray = 0;

                int rowSize = 0;

                if (!PSoC4_GetFlashInfo(out rowsPerArray, out rowSize, out er))
                    return false;

                int totalRows = _imageSize / rowSize;

                for (int i = 0; i < totalRows; i++)
                {
                    if (!PSoC4_ProgramRowFromHex(i, out er))
                        return false;                    
                }

                //校验烧录芯片Flash---???

                rowsPerArray = 0;

                rowSize = 0;

                if (!PSoC4_GetFlashInfo(out rowsPerArray, out rowSize, out er))
                    return false;

                totalRows = _imageSize / rowSize;

                for (int i = 0; i < totalRows; i++)
                {
                    if (!PSoC4_VerifyRowFromHex(i, out er))
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
        /// STEP6:写入写保护
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool STEP_WriteProtect(out string er)
        {
            er = string.Empty;

            try
            {
                if (!PSoC4_ProtectAll(out er))
                    return false;

                if (!PSoC4_VerifyProtect(out er))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }             
        }
        /// <summary>
        /// STEP7:校验和检查
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool STEP_CheckSum(out string er)
        {
            er = string.Empty;

            try
            {
                int checkSum_UserPrivileged = 0;

                if (!PSoC4_CheckSum(out checkSum_UserPrivileged, out er))
                    return false;
                
                short hexChecksum = 0;

                if (!HEX_ReadChecksum(out hexChecksum, out er))
                    return false;

                int checkSum_User = checkSum_UserPrivileged - _checkSum_Privileged; //find checksum of User Flash rows

                checkSum_User &= 0xFFFF;

                int hexChecks = hexChecksum & 0xFFFF;

                if (checkSum_User != hexChecks)
                {
                    er = "Mismatch of Checksum: Expected 0x" + checkSum_User.ToString("X") + ", Got 0x" + hexChecksum.ToString("X");
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
        /// 释放IC
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool STEP_ReleaseChip(out string er)
        {
            return DAP_ReleaseChip(out er); 
        }
        /// <summary>
        /// PowerOn
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool STEP_PowerOn(out string er)
        {
            return PowerOn(out er); 
        }
        /// <summary>
        /// PowerOff
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool STEP_PowerOff(out string er)
        {
            return PowerOff(out er);
        }
        #endregion

        #region 烧录器方法
        /// <summary>
        /// 加载动态库
        /// </summary>
        /// <param name="er"></param>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        private bool LoadAPI(out string er, string defaultPath = @"C:\Program Files (x86)\Cypress\Programmer\PP_COM_Wrapper.dll")
        {
            er = string.Empty;

            try
            {
                if (!File.Exists(defaultPath))
                {
                    er = "文件不存在[" + defaultPath + "]";
                    return false;
                }

                if (_PPCom != null)
                    _PPCom = null;

                Assembly abs = Assembly.LoadFile(defaultPath);

                foreach (Type item in abs.GetTypes())
                {
                    if (item.FullName == "PP_COM_Wrapper.PSoCProgrammerCOM_ObjectClass")
                    {
                        _PPCom = abs.CreateInstance(item.FullName);
                    }
                }

                if (_PPCom == null)
                {
                    er = "加载程序集PSoCProgrammerCOM_ObjectClass失败";
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
        /// 获取烧录器列表
        /// </summary>
        /// <param name="DevProg"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool GetPorts(out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("GetPorts");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

               object posts=null;

               string strError = string.Empty;

               object[] invokeArgs = new object[] { posts, strError };

               int hr = (int)Method.Invoke(_PPCom, invokeArgs);

               er = invokeArgs[1].ToString();

               if (hr < 0)                
                   return false;

               string[] portName = invokeArgs[0] as string[];

               if (portName.Length == 0)
               {
                   er = "电脑检测不到烧录器";
                   return false;
               }

               for (int i = 0; i < portName.Length; i++)
               {
                   if (portName[i].StartsWith("MiniProg3"))
                   {
                       string id = portName[i].Split('/')[1];

                       PortName.Add(id, portName[i]); 
                   }
               }

               if (PortName.Count == 0)
               {
                   er = "电脑检测不到烧录器MiniProg3";
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
        /// 打开烧录器
        /// </summary>
        /// <param name="PSoCId"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool OpenPort(string PSoCId, out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                if (!PortName.ContainsKey(PSoCId))
                {
                    er = "找不到烧录器ID号["+ PSoCId +"]";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("OpenPort");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { PortName[PSoCId], strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {                    
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
        /// 关闭烧录器
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool ClosePort(out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("ClosePort");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[0].ToString();

                if (hr < 0)
                {                   
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
        /// 端口是否打开?
        /// </summary>
        /// <returns></returns>
        private bool IsPortOpen(out int isOpen, out string er)
        {
            isOpen = 0;

            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("IsPortOpen");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { isOpen, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {
                    return false;
                }

                isOpen = (int)invokeArgs[0];

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 设置IC电压值
        /// </summary>
        /// <param name="voltage"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool SetPowerVoltage(double voltage, out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("SetPowerVoltage");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { voltage.ToString(), strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {                    
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
        /// IC电压PowerOn
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool PowerOn(out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("PowerOn");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[0].ToString();

                if (hr < 0)
                {                    
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
        /// IC电压PowerOn
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool PowerOff(out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("PowerOff");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[0].ToString();

                if (hr < 0)
                {
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
        /// 设置通讯协议
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool SetProtocol(EProtocol protocol, out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("SetProtocol");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { protocol, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {                    
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
        /// 设置接口PIN类型
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool SetProtocolConnector(EPIN pin, out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("SetProtocolConnector");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { (int)pin, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {                    
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
        /// 设置时钟速度(MHz)
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool SetProtocolClock(EFrequencies freq, out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("SetProtocolClock");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { freq, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {                    
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
        /// 导入烧录文件(Hex)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool HEX_ReadFile(string filePath, out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("HEX_ReadFile");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                int imageSize = 0;

                object[] invokeArgs = new object[] { filePath , imageSize, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[2].ToString();

                if (hr < 0)
                {                    
                    return false;
                }

                _imageSize = System.Convert.ToInt32(invokeArgs[1]); 

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 读取烧录文件
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool HEX_ReadChipProtection(out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("HEX_ReadChipProtection");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }
                
                object data=null;

                string strError = string.Empty;

                object[] invokeArgs = new object[] { data, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {                 
                    return false;
                }

                byte hex_chipProt = (invokeArgs[0] as byte[])[0];

                if (hex_chipProt == CHIP_PROT_VIRGIN)
                {
                    er = "HEX文件不允许对芯片烧录操作";
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
        /// 设置模式
        /// </summary>
        /// <param name="mode">Reset:外部供电;Power:烧录器供电;PowerDetect:自动检测供电</param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool SetAcquireMode(string mode, out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("SetAcquireMode");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { mode, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {                   
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
        /// 获取IC
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool DAP_Acquire(out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("DAP_Acquire");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[0].ToString();

                if (hr < 0)
                {                   
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
        /// 获取IC
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool DAP_AcquireChip(out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("DAP_AcquireChip");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[0].ToString();

                if (hr < 0)
                {
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
        /// 获取芯片ID
        /// </summary>
        /// <param name="jtagID"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool PSoC4_GetSiliconID(out object jtagID, out string er)
        {
            jtagID = null;

            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("PSoC4_GetSiliconID");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { jtagID, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {                    
                    return false;
                }

                jtagID = invokeArgs[0];

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 从HEX文件读芯片ID
        /// </summary>
        /// <param name="hexJtagID"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool HEX_ReadJtagID(out object hexJtagID, out string er)
        {
            hexJtagID = null;

            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("HEX_ReadJtagID");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { hexJtagID, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {                    
                    return false;
                }

                hexJtagID = invokeArgs[0];

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 读取写保护
        /// </summary>
        /// <param name="flashProt"></param>
        /// <param name="chipProt"></param>
        /// <returns></returns>
        private bool PSoC4_ReadProtection(out object flashProt, out object chipProt,out string er)
        {
            flashProt = null;

            chipProt = null;

            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("PSoC4_ReadProtection");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { flashProt, chipProt, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[2].ToString();

                if (hr < 0)
                {
                    return false;
                }

                flashProt = invokeArgs[0];

                chipProt = invokeArgs[1];

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 写保护
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool PSoC4_WriteProtection(out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("PSoC4_WriteProtection");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;
                
                //Move to OPEN from PROTECTED. It automatically erases Flash and its Protection bits.
                byte[] flashProt = new byte[] { }; // do not care in PROTECTED mode
                
                byte[] chipProt = new byte[] { CHIP_PROT_OPEN }; //move to OPEN

                object[] invokeArgs = new object[] { flashProt, chipProt ,strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[2].ToString();

                if (hr < 0)
                {                   
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
        /// 擦除内存
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool PSoC4_EraseAll(out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("PSoC4_EraseAll");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[0].ToString();

                if (hr < 0)
                {                   
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
        /// Find checksum of Privileged Flash.
        /// Will be used in calculation of User CheckSum later
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool PSoC4_CheckSum(out int checkSum, out string er)
        {
            checkSum = 0;

            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("PSoC4_CheckSum");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { 0x8000, checkSum, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[2].ToString();

                if (hr < 0)
                {                    
                    return false;
                }

                checkSum = (int)invokeArgs[1];

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 获取Flash信息
        /// </summary>
        /// <param name="rowsPerArray"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        private bool PSoC4_GetFlashInfo(out int rowsPerArray,out int rowSize,out string er)
        {
           rowsPerArray = 0;

           rowSize = 0;

           er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("PSoC4_GetFlashInfo");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { rowsPerArray, rowSize, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[2].ToString();

                if (hr < 0)
                {                  
                    return false;
                }

                rowsPerArray = (int)invokeArgs[0];

                rowSize = (int)invokeArgs[1];

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 烧录行
        /// </summary>
        /// <param name="rowID"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool PSoC4_ProgramRowFromHex(int rowID,out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("PSoC4_ProgramRowFromHex");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { rowID, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {                   
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
        /// 校验行
        /// </summary>
        /// <param name="rowID"></param>
        /// <param name="verResult"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool PSoC4_VerifyRowFromHex(int rowID, out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("PSoC4_VerifyRowFromHex");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                int verResult = 0;

                string strError = string.Empty;

                object[] invokeArgs = new object[] { rowID, verResult, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[2].ToString();

                if (hr < 0)
                {                   
                    return false;
                }

                verResult = (int)invokeArgs[1];

                if (verResult == 0)
                {
                    er = "Verification failed on " + rowID + " row.";
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
        /// 加密内存
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool PSoC4_ProtectAll(out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("PSoC4_ProtectAll");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[0].ToString();

                if (hr < 0)
                {                    
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
        /// 校验加密
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool PSoC4_VerifyProtect(out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("PSoC4_VerifyProtect");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[0].ToString();

                if (hr < 0)
                {                    
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
        /// 烧录文件校验和
        /// </summary>
        /// <param name="hexChecksum"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool HEX_ReadChecksum(out short hexChecksum, out string er)
        {
            hexChecksum = 0;

            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("HEX_ReadChecksum");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { hexChecksum, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {
                    return false;
                }

                hexChecksum = (short)invokeArgs[0];

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 释放IC
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool DAP_ReleaseChip(out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("DAP_ReleaseChip");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);
                
                er = invokeArgs[0].ToString();

                if (hr < 0)
                {                    
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
        /// 获取芯片信息
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="family"></param>
        /// <param name="familyCode"></param>
        /// <param name="pins"></param>
        /// <param name="flashSize"></param>
        /// <param name="aquireMode"></param>
        /// <param name="siliconIDs"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool GetDeviceInfo(string devName, out string family, out int familyCode, out int pins,
                                   out int flashSize, out int aquireMode, out object siliconIDs, out string er)
        {
            family = string.Empty;

            familyCode = 0;

            pins = 0;

            flashSize = 0;

            aquireMode = 0;

            siliconIDs = null;

            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("GetDeviceInfo");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { devName, family, familyCode, pins, flashSize, aquireMode, siliconIDs, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[7].ToString();

                if (hr < 0)
                {
                    return false;
                }

                family = invokeArgs[1].ToString();

                familyCode = (int)invokeArgs[2];

                pins = (int)invokeArgs[3];

                flashSize = (int)invokeArgs[4];

                aquireMode = (int)invokeArgs[5];

                siliconIDs = invokeArgs[6];

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 设置芯片类型
        /// </summary>
        /// <param name="familyCode"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool SetChipType(int familyCode, out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("SetChipType");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { familyCode, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {
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
        /// 释放进程COM
        /// </summary>
        /// <param name="clientProcessId">0</param>
        /// <returns></returns>
        private bool _StartSelfTerminator(int clientProcessId,out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("_StartSelfTerminator");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { clientProcessId };

                _serverProcessId = (int)Method.Invoke(_PPCom, invokeArgs); ;

                if (_serverProcessId < 0)
                {
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
        /// 设置同步,异步
        /// </summary>
        /// <param name="mode">0:同步 1：异步</param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool USB2IIC_AsyncMode(EMode mode, out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("USB2IIC_AsyncMode");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { (int)mode, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {
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
        /// 自动复位
        /// </summary>
        /// <param name="autoReset">1:自动复位</param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool SetAutoReset(int autoReset, out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("SetAutoReset");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { autoReset, strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[1].ToString();

                if (hr < 0)
                {
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
        /// 复位总线
        /// </summary>
        /// <param name="autoRest">1:自动复位</param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool SWD_LineReset(out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("SWD_LineReset");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                string strError = string.Empty;

                object[] invokeArgs = new object[] { strError };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                er = invokeArgs[0].ToString();

                if (hr < 0)
                {
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
        /// 复位总线
        /// </summary>
        /// <param name="autoRest">1:自动复位</param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool SWDIOR_RAW(out string er)
        {
            er = string.Empty;

            try
            {
                if (_PPCom == null)
                {
                    er = "程序集PSoCProgrammerCOM_ObjectClass未初始化";
                    return false;
                }

                Type type = _PPCom.GetType();

                MethodInfo Method = type.GetMethod("swdior_raw");

                if (Method == null)
                {
                    er = "未找到该方法";
                    return false;
                }

                byte[] dataIN = new byte[1];

                object dataOUT=null;

                string strError = string.Empty;

                object[] invokeArgs = new object[] { dataIN, dataOUT };

                int hr = (int)Method.Invoke(_PPCom, invokeArgs);

                byte[] data= invokeArgs[1] as byte[];

                if (hr < 0 || (data[data.Length-1] & 0x01)!=0x01)
                {
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
        #endregion

    }
}
