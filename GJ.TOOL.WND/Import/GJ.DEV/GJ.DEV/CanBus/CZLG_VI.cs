using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace GJ.DEV.CanBus
{ 
    /// <summary>
    /// 广州致远
    /// </summary>
    public class CZLG_VI
    {

        #region 结构体定义
        /// <summary>
        /// 1.ZLGCAN系列接口卡信息的数据类型。
        /// </summary>
        public struct VCI_BOARD_INFO
        {
            public UInt16 hw_Version;
            public UInt16 fw_Version;
            public UInt16 dr_Version;
            public UInt16 in_Version;
            public UInt16 irq_Num;
            public byte can_Num;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] str_Serial_Num;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
            public byte[] str_hw_Type;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] Reserved;
        }

        ///// <summary>
        ///// 2.定义CAN信息帧的数据类型。
        ///// </summary>
        //unsafe public struct VCI_CAN_OBJ  //使用不安全代码
        //{
        //    public uint ID;
        //    public uint TimeStamp;
        //    public byte TimeFlag;
        //    public byte SendType;
        //    public byte RemoteFlag;//是否是远程帧
        //    public byte ExternFlag;//是否是扩展帧
        //    public byte DataLen;
        //    public fixed byte Data[8];
        //    public fixed byte Reserved[3];
        //}

        ///2.定义CAN信息帧的数据类型。
        public struct VCI_CAN_OBJ
        {
            public UInt32 ID;
            public UInt32 TimeStamp;
            public byte TimeFlag;
            public byte SendType;
            public byte RemoteFlag;//是否是远程帧
            public byte ExternFlag;//是否是扩展帧
            public byte DataLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] Data;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] Reserved;
            public void Init()
            {
                Data = new byte[8];
                Reserved = new byte[3];
            }
        }

        /// <summary>
        /// 3.定义CAN控制器状态的数据类型。
        /// </summary>
        public struct VCI_CAN_STATUS
        {
            public byte ErrInterrupt;
            public byte regMode;
            public byte regStatus;
            public byte regALCapture;
            public byte regECCapture;
            public byte regEWLimit;
            public byte regRECounter;
            public byte regTECounter;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] Reserved;
        }

        /// <summary>
        /// 4.定义错误信息的数据类型。
        /// </summary>
        public struct VCI_ERR_INFO
        {
            public UInt32 ErrCode;
            public byte Passive_ErrData1;
            public byte Passive_ErrData2;
            public byte Passive_ErrData3;
            public byte ArLost_ErrData;
        }

        /// <summary>
        /// 5.定义初始化CAN的数据类型
        /// </summary>
        public struct VCI_INIT_CONFIG
        {
            public UInt32 AccCode;
            public UInt32 AccMask;
            public UInt32 Reserved;
            public byte Filter;
            public byte Timing0;
            public byte Timing1;
            public byte Mode;
        }

        public struct CHGDESIPANDPORT
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] szpwd;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] szdesip;
            public Int32 desport;

            public void Init()
            {
                szpwd = new byte[10];
                szdesip = new byte[20];
            }
        }

        ///////// new add struct for filter /////////
        //typedef struct _VCI_FILTER_RECORD{
        //    DWORD ExtFrame;	//是否为扩展帧
        //    DWORD Start;
        //    DWORD End;
        //}VCI_FILTER_RECORD,*PVCI_FILTER_RECORD;
        public struct VCI_FILTER_RECORD
        {
            public UInt32 ExtFrame;
            public UInt32 Start;
            public UInt32 End;
        }

        #endregion

        #region 常量定义

        /// 设备型号
        public const int VCI_PCI5121 = 1;
        public const int VCI_PCI9810 = 2;
        public const int VCI_USBCAN1 = 3;
        public const int VCI_USBCAN2 = 4;
        public const int VCI_USBCAN2A = 4;
        public const int VCI_PCI9820 = 5;
        public const int VCI_CAN232 = 6;
        public const int VCI_PCI5110 = 7;
        public const int VCI_CANLITE = 8;
        public const int VCI_ISA9620 = 9;
        public const int VCI_ISA5420 = 10;
        public const int VCI_PC104CAN = 11;
        public const int VCI_CANETUDP = 12;
        public const int VCI_CANETE = 12;
        public const int VCI_DNP9810 = 13;
        public const int VCI_PCI9840 = 14;
        public const int VCI_PC104CAN2 = 15;
        public const int VCI_PCI9820I = 16;
        public const int VCI_CANETTCP = 17;
        public const int VCI_PEC9920 = 18;
        public const int VCI_PCI5010U = 19;
        public const int VCI_USBCAN_E_U = 20;
        public const int VCI_USBCAN_2E_U = 21;
        public const int VCI_PCI5020U = 22;
        public const int VCI_EG20T_CAN = 23;
        public const int VCI_PCIE9120I = 27;
        public const int VCI_PCIE9110I = 28;
        public const int VCI_PCIE9140I = 29;
        public const int VCI_USBCAN_8E_U = 34;

        ///函数调用返回状态值
        public const UInt32 STATUS_OK = 1;
        public const UInt32 STATUS_ERR = 0;

        ///CAN错误码
        public const UInt32 ERR_CAN_OVERFLOW = 0x0001;	//CAN控制器内部FIFO溢出
        public const UInt32 ERR_CAN_ERRALARM = 0x0002;	//CAN控制器错误报警
        public const UInt32 ERR_CAN_PASSIVE = 0x0004;	//CAN控制器消极错误
        public const UInt32 ERR_CAN_LOSE = 0x0008;    	//CAN控制器仲裁丢失
        public const UInt32 ERR_CAN_BUSERR = 0x0010;  	//CAN控制器总线错误
        public const UInt32 ERR_CAN_BUSOFF = 0x0020;      //总线关闭错误
        //通用错误码
        public const UInt32 ERR_DEVICEOPENED = 0x0100;	//设备已经打开
        public const UInt32 ERR_DEVICEOPEN = 0x0200;  	//打开设备错误
        public const UInt32 ERR_DEVICENOTOPEN = 0x0400;	//设备没有打开
        public const UInt32 ERR_BUFFEROVERFLOW = 0x0800;	//缓冲区溢出
        public const UInt32 ERR_DEVICENOTEXIST = 0x1000;	//此设备不存在
        public const UInt32 ERR_LOADKERNELDLL = 0x2000;	//装载动态库失败
        public const UInt32 ERR_CMDFAILED = 0x4000;	//执行命令失败错误码
        public const UInt32 ERR_BUFFERCREATE = 0x8000;	//内存不足

        /// <summary>
        /// 设备类型
        /// </summary>
        public enum EVCI
        { 
          PCI5121 = 1,
          PCI9810 = 2,
          USBCAN1 = 3,
          USBCAN2 = 4,
          PCI9820 = 5,
          CAN232 = 6,
          PCI5110 = 7,
          CANLITE = 8,
          ISA9620 = 9,
          ISA5420 = 10,
          PC104CAN = 11,
          CANETUDP = 12,
          DNP9810 = 13,
          PCI9840 = 14,
          PC104CAN2 = 15,
          PCI9820I = 16,
          CANETTCP = 17,
          PEC9920 = 18,
          PCI5010U = 19,
          USBCAN_E_U = 20,
          USBCAN_2E_U = 21,
          PCI5020U = 22,
          EG20T_CAN = 23,
          PCIE9120I = 27,
          PCIE9110I = 28,
          PCIE9140I = 29,
          USBCAN_8E_U = 34
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public enum EErrCode
        {
          /// <summary>
          /// 正常
          /// </summary>
          ERR_NORMAL=0x0000,
          /// <summary>
          ///CAN控制器内部FIFO溢出
          /// </summary>
          ERR_CAN_OVERFLOW = 0x0001,	
          /// <summary>
          /// CAN控制器错误报警
          /// </summary>
          ERR_CAN_ERRALARM = 0x0002,
	      /// <summary>
          /// CAN控制器消极错误
	      /// </summary>
          ERR_CAN_PASSIVE = 0x0004,
          /// <summary>
          /// CAN控制器仲裁丢失
          /// </summary>
          ERR_CAN_LOSE = 0x0008,   	
          /// <summary>
          /// CAN控制器总线错误
          /// </summary>
          ERR_CAN_BUSERR = 0x0010,  	
          /// <summary>
          /// 总线关闭错误
          /// </summary>
          ERR_CAN_BUSOFF = 0x0020,               
          /// <summary>
          /// 设备已经打开
          /// </summary>
          ERR_DEVICEOPENED = 0x0100,	
          /// <summary>
          /// 打开设备错误
          /// </summary>
          ERR_DEVICEOPEN = 0x0200, 
          /// <summary>
          /// 设备没有打开
          /// </summary>
          ERR_DEVICENOTOPEN = 0x0400,	
          /// <summary>
          /// 缓冲区溢出
          /// </summary>
          ERR_BUFFEROVERFLOW = 0x0800,	
          /// <summary>
          /// 此设备不存在
          /// </summary>
          ERR_DEVICENOTEXIST = 0x1000,	
          /// <summary>
          /// 装载动态库失败
          /// </summary>
          ERR_LOADKERNELDLL = 0x2000,
	      /// <summary>
          /// 执行命令失败错误码
	      /// </summary>
          ERR_CMDFAILED = 0x4000,	
          /// <summary>
          /// 内存不足
          /// </summary>
          ERR_BUFFERCREATE = 0x8000	
        }
        /// <summary>
        /// 波特率
        /// </summary>
        public enum ECanBaud
        { 
           _1000Kbs,
           _800Kbs,
           _500Kbs,
           _250Kbs,
           _125Kbs,
           _100Kbs,
           _50Kbs,
           _20Kbs,
           _10Kbs,
           _5Kbs
        }
        public enum EFilter
        { 
          双滤波=0,
          单滤波=1
        }
        public enum ERunMode
        { 
          正常模式=0,
          只听模式=1
        }
        public enum ESendType
        { 
           正常发送=0,
           单次发送=1,
           自发自收=2,
           单次自发自收=3
        }
        public enum ERemoteFlag
        { 
           数据帧=0,
           远程帧=1   //数据段空
        }
        public enum EExternFlag
        { 
           标准帧=0,  //11位ID
           扩展帧=1   //29位ID
        }
        public static byte[] BTR0 = new byte[]
        {
           0x0,
           0x0,
           0x0,
           0x1,
           0x3,
           0x4,
           0x9,
           0x18,
           0x31,
           0xBF,
        };
        public static byte[] BTR1=new byte[]
        {
          0x14,
          0x16,
          0x1C,
          0x1C,
          0x1C,
          0x1C,
          0x1C,
          0x1C,
          0x1C,
          0xFF
        };

        //usb-e-u 波特率
        public static UInt32[] GCanBrTab = new UInt32[]
        {
	       0x060003,
           0x060004,
           0x060007,
		   0x1C0008,
           0x1C0011,
           0x160023,
		   0x1C002C, 
           0x1600B3,
           0x1C00E0,
		   0x1C01C1
        };
        #endregion

        #region 函数声明

        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_OpenDevice(UInt32 DeviceType, UInt32 DeviceInd, UInt32 Reserved);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_CloseDevice(UInt32 DeviceType, UInt32 DeviceInd);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_InitCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_INIT_CONFIG pInitConfig);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ReadBoardInfo(UInt32 DeviceType, UInt32 DeviceInd, ref VCI_BOARD_INFO pInfo);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ReadErrInfo(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_ERR_INFO pErrInfo);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ReadCANStatus(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_STATUS pCANStatus);

        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_GetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_SetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData);
        //public unsafe static extern UInt32 VCI_SetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, byte* pData);

        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_GetReceiveNum(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ClearBuffer(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);

        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_StartCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ResetCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);

        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_Transmit(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pSend, UInt32 Len);

        //[DllImport("controlcan.dll")]
        //static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pReceive, UInt32 Len, Int32 WaitTime);
        [DllImport("controlcan.dll", CharSet = CharSet.Ansi)]
        public static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, IntPtr pReceive, UInt32 Len, Int32 WaitTime);
        #endregion

        #region 字段
        /// <summary>
        /// CanBus总线状态
        /// </summary>
        private bool _devOpen =false;
        /// <summary>
        /// CanBus设备类型
        /// </summary>
        private UInt32 _devType = 0;
        /// <summary>
        /// CanBus总线编号
        /// </summary>
        private UInt32 _canBusId = 0;
        #endregion

        #region 方法
        /// <summary>
        /// 打开CanBus总线
        /// </summary>
        /// <param name="er"></param>
        /// <param name="devType"></param>
        /// <param name="canComId"></param>
        /// <returns></returns>
        public bool OpenCanBus(EVCI devType, UInt32 canBusId,out string er)
        {
            er = string.Empty;

            try
            {
                _devType = (UInt32)devType;

                _canBusId = canBusId;

                if (_devOpen)
                {
                    VCI_CloseDevice(_devType, _canBusId);
                    _devOpen = false; 
                }

                if (VCI_OpenDevice(_devType, _canBusId, 0) != STATUS_OK)
                {
                    er = "打开设备失败,请检查设备类型和设备索引号是否正确;";
                    return false;
                }

                _devOpen = true;

                return true; 
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false; 
            }
        }
        /// <summary>
        /// 关闭CanBus总线 
        /// </summary>
        public void CloseCanBus()
        {
            if (_devOpen)
            {
                VCI_CloseDevice(_devType, _canBusId);
                _devOpen = false; 
            }
        }
        /// <summary>
        /// 读错误代码
        /// </summary>
        /// <param name="canBusId"></param>
        /// <returns></returns>
        public bool ReadError(UInt32 canBusId , out string er)
        {
            er = string.Empty;

            try
            {
                if (!_devOpen)
                {
                    er = "CanBus设备总线【"+ canBusId.ToString() +"】未打开";
                    return false;
                }

                VCI_ERR_INFO err_Info = new VCI_ERR_INFO();

                if (VCI_ReadErrInfo((UInt32)_devType, _canBusId, canBusId, ref err_Info) != STATUS_OK)
                {
                    er = "读取CanBus错误信息通信异常";

                    return false;
                }

                if (!Enum.IsDefined(typeof(EErrCode), err_Info.ErrCode))
                {
                    er = "CanBus错误代码=" + err_Info.ErrCode.ToString();
                    return false;
                }

                EErrCode errCode = (EErrCode)err_Info.ErrCode;

                if (errCode != EErrCode.ERR_NORMAL)
                {
                    er = errCode.ToString();

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
        /// 初始化设备
        /// </summary>
        /// <param name="canBand"></param>
        /// <param name="runMode"></param>
        /// <param name="filter"></param>
        /// <param name="accCode"></param>
        /// <param name="accMark"></param>
        /// <returns></returns>
        public bool InitalCanBus(UInt32 canBusId, ECanBaud canBand, out string er,
                                 UInt32 accCode=0x0,UInt32 accMark=0xFFFFFFFF,
                                 ERunMode runMode = ERunMode.正常模式, EFilter filter = EFilter.单滤波                                 
                                 )
        {

            er = string.Empty;

            try
            {
                if (!_devOpen)
                {
                    er = "CanBus总线未打开";
                    return false;
                }

                VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();
                config.AccCode = accCode;
                config.AccMask =accMark;
                config.Timing0 = BTR0[(int)canBand];
                config.Timing1 = BTR1[(int)canBand];
                config.Filter = (Byte)filter;
                config.Mode = (Byte)runMode;

                if (VCI_InitCAN(_devType, _canBusId, canBusId, ref config) != STATUS_OK)
                {
                    er = "初始化【"+ ((EVCI)_devType).ToString()  +"】设备【" + canBusId.ToString() + "】失败";
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
        /// 启动CanBus设备
        /// </summary>
        /// <param name="canComId"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SetCanRun(UInt32 canIndex, out string er)
        {
            er = string.Empty;

            try
            {
                if (!_devOpen)
                {
                    er = "CanBus总线未打开";
                    return false;
                }

                if (VCI_StartCAN(_devType, _canBusId, canIndex) != STATUS_OK)
                {
                    er = "启动设备【" + ((EVCI)_devType).ToString() + "】总线【"+ 
                                   _canBusId.ToString() +"】编号【" + canIndex.ToString() + "】失败";
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
        /// 复位CanBus设备
        /// </summary>
        /// <param name="canComId"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool SetCanReset(UInt32 canIndex, out string er)
        {
            er = string.Empty;

            try
            {
                if (!_devOpen)
                {
                    er = "CanBus总线未打开";
                    return false;
                }

                if (VCI_ResetCAN(_devType, _canBusId, canIndex) != STATUS_OK)
                {
                    er = "复位设备【" + ((EVCI)_devType).ToString() + "】总线【" +
                                        _canBusId.ToString() + "】编号【" + canIndex.ToString() + "】失败";
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
        /// 发送数据
        /// </summary>
        /// <param name="canBusId"></param>
        /// <param name="canIndex"></param>
        /// <param name="wData"></param>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <param name="wTimeOut"></param>
        /// <param name="remoteFlag"></param>
        /// <param name="externFlag"></param>
        /// <param name="sendType"></param>
        /// <param name="recvFlag"></param>
        /// <returns></returns>
        public bool SendCmd(UInt32 canBusId,
                            string canIndex,string wData,out string rData,
                            out string er,int wTimeOut=100,                         
                            ERemoteFlag remoteFlag=ERemoteFlag.数据帧,
                            EExternFlag externFlag=EExternFlag.标准帧,
                            ESendType sendType=ESendType.正常发送,bool recvFlag=true)
        {
            rData = string.Empty;

            er = string.Empty;

            try
            {
                if (!_devOpen)
                {
                    er = "CanBus总线未打开";
                    return false;
                }
                if (VCI_ClearBuffer(_devType, _canBusId, canBusId) != STATUS_OK)
                {
                    er = "清除缓存失败";
                    return false;
                }
                VCI_CAN_OBJ sendobj = new VCI_CAN_OBJ();
                sendobj.Init();
                sendobj.SendType = (byte)sendType;
                sendobj.RemoteFlag = (byte)remoteFlag;
                sendobj.ExternFlag = (byte)externFlag;
                sendobj.ID = System.Convert.ToUInt32(canIndex, 16);
                int len = wData.Length / 2;
                sendobj.DataLen = System.Convert.ToByte(len);
                for (int i = 0; i < len; i++)
                    sendobj.Data[i]= System.Convert.ToByte(wData.Substring(i * 2, 2),16);
                if (VCI_Transmit(_devType, _canBusId, canBusId, ref sendobj, 1) != STATUS_OK)
                {
                    er = "发送数据失败";
                    return false;
                }
                //不接收数据
                if (!recvFlag)
                    return true;
                //接收数据
                UInt32 getRecNum = 0;
                int waitTimes = System.Environment.TickCount;
                while (System.Environment.TickCount - waitTimes < wTimeOut)
                {
                    UInt32 res = new UInt32();
                    res = VCI_GetReceiveNum(_devType, _canBusId, canBusId);
                    if (res == 0)
                        continue;
                    getRecNum = res;
                    break;
                }
                if (getRecNum == 0)
                {
                    er = "接收数据超时";
                    return false;
                }
                UInt32 con_maxlen = 50;
                IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * (Int32)con_maxlen);
                UInt32 resLen = VCI_Receive(_devType, _canBusId, canBusId, pt, con_maxlen, wTimeOut);
                rData = string.Empty;
                for (UInt32 i = 0; i < resLen; i++)
                {
                    VCI_CAN_OBJ obj = (VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((UInt32)pt + i * Marshal.SizeOf(typeof(VCI_CAN_OBJ))), typeof(VCI_CAN_OBJ));

                    if (obj.RemoteFlag == 0)  //数据帧
                    {
                        byte rLen = (byte)(obj.DataLen % 9);
                        for (byte j = 0; j < rLen; j++)
                            rData += obj.Data[j].ToString("X2");
                    }
                }
                Marshal.FreeHGlobal(pt);
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
