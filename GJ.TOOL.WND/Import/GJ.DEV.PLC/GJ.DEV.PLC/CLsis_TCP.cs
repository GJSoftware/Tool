using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;

namespace GJ.DEV.PLC
{
    /// <summary>
    ///产电PLC TCP
    /// 版本:V1.0.0 作者:kp.lin 日期:2017/08/15
    /// </summary>
    public class CLsis_TCP : IPLC
    {
        #region 构造函数 
        public CLsis_TCP(int idNo = 0, string name = "Lsis_TCP")
        {
            this._idNo = idNo;
            this._name = name;
            com = new CClientTCP(_idNo, _name, EDataType.ASCII格式); 
        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 字段
        private int _idNo = 0;
        private string _name = "Lsis_TCP";
        private CClientTCP com=null;
        private int _wordNum = 1;
        private object _sync = new object();
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
                if (com == null)
                    return false;
                else
                    return com.conStatus;
            }
        }
        /// <summary>
        /// 字节长度
        /// </summary>
        public int wordNum
        {
            get { return _wordNum; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 打开连接
        /// </summary>
        /// <param name="comName">IP地址“192.168.0.10”</param>
        /// <param name="er"></param>
        /// <param name="setting">端口 2004:TCP/IP,2005:UTP/IP</param>
        /// <returns></returns>
        public bool Open(string comName, out string er, string setting = "2004")
        {
            if (!com.open(comName, out er, setting))
                return false;
            return true;
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            com.close();
        }
        /// <summary>
        /// 读取寄存器数据（高位在前,低位在后）
        /// </summary>
        /// <param name="plcAddr">PLC地址(TCP为0)</param>
        /// <param name="regType">寄存器类型</param>
        /// <param name="startAddr">开始地址</param>
        /// <param name="N">长度</param>
        /// <param name="rData">反转处理:高在前,低位在后</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Read(int plcAddr, ERegType regType, int startAddr, int N, out string rData, out string er)
        {
            lock (_sync)
            {
                er = string.Empty;

                rData = string.Empty;

                try
                {
                    int[] rVals = new int[N];

                    if (!Read(plcAddr, regType, startAddr, ref rVals, out er))
                        return false;

                    for (int i = 0; i < rVals.Length; i++)
                        rData = rVals[i].ToString("X4") + rData;

                    return true;
                }
                catch (Exception ex)
                {
                    er = ex.ToString();
                    return false;
                }
            }
        }
        /// <summary>
        /// 读取单个寄存器
        /// </summary>
        /// <param name="plcAddr">PLC地址(TCP:0)</param>
        /// <param name="regType">寄存器类型</param>
        /// <param name="startAddr">开始地址 </param>
        /// <param name="startBin">位地址(无=0)</param>
        /// <param name="rVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Read(int plcAddr, ERegType regType, int startAddr, int startBin, out int rVal, out string er)
        {
            lock (_sync)
            {
                rVal = -1;

                er = string.Empty;

                try
                {
                    int[] rVals = new int[1];

                    if (!Read(plcAddr, regType, startAddr, ref rVals, out er))
                        return false;

                    rVal = rVals[0];

                    return true;
                }
                catch (Exception e)
                {
                    er = e.Message;
                    return false;
                }
            }
        }
        /// <summary>
        /// 读取多个寄存器
        /// </summary>
        /// <param name="plcAddr">PLC地址(TCP:0)</param>
        /// <param name="regType">寄存器类型</param>
        /// <param name="startAddr">开始地址</param>
        /// <param name="rVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Read(int plcAddr, ERegType regType, int startAddr, ref int[] rVal, out string er)
        {
            lock (_sync)
            {
                er = string.Empty;

                try
                {
                    List<byte> commandBytes = getCommandByteArray(regType, startAddr, rVal.Length, true, rVal);

                    byte[] returnBytes;

                    int returnSize = 32 + (rVal.Length * 2);

                    if (!com.send(commandBytes.ToArray(), returnSize, out returnBytes, out er))
                        return false;

                    if (returnBytes.Length < returnSize)
                    {
                        er = "Time Out";
                        return false;
                    }

                    byte[] headerBytes = returnBytes.Skip(0).Take(19).ToArray();

                    int bcc = (int)BCC(headerBytes.ToList());

                    if ((int)BCC(headerBytes.ToList()) != (int)returnBytes[19])
                    {
                        er = "BCC Check Error";
                        return false;
                    }

                    if (returnBytes[26] != 0 || returnBytes[27] != 0)
                    {
                        er = "Read Data Error";
                        return false;
                    }

                    if (returnBytes[20] != 0x55)
                    {
                        er = "Read Data Error";
                        return false;
                    }

                    byte[] byteVal = returnBytes.Skip(32).Take(rVal.Length * 2).ToArray();

                    for (int i = 0; i < rVal.Length; i++)
                    {
                        rVal[i] = byteVal[i * 2];
                        rVal[i] += (byteVal[i * 2 + 1] * 256);
                    }
                    return true;
                }
                catch (Exception e)
                {
                    er = e.Message;
                    return false;
                }
            }
        }
        /// <summary>
        /// 写寄存器数据（高位在前,低位在后）
        /// </summary>
        /// <param name="plcAddr">PLC地址(TCP为0)</param>
        /// <param name="regType">寄存器类型</param>
        /// <param name="startAddr">开始地址</param>
        /// <param name="N">长度</param>
        /// <param name="strHex">16进制字符FFFF,反转处理:低位在后，高在前</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Write(int plcAddr, ERegType regType, int startAddr, int N, string strHex, out string er)
        {
            lock (_sync)
            {
                er = string.Empty;

                try
                {
                    int[] wVals = new int[N];

                    for (int i = 0; i < strHex.Length / 4; i++)
                        wVals[i] = System.Convert.ToInt32(strHex.Substring(strHex.Length - i * 4, 4), 16);

                    if (!Write(plcAddr, regType, startAddr, wVals, out er))
                        return false;

                    return true;
                }
                catch (Exception ex)
                {
                    er = ex.ToString();
                    return false;
                }
            }
        
        }
        /// <summary>
       /// 写单个寄存器
       /// </summary>
       /// <param name="plcAddr">PLC地址(TCP为0)</param>
       /// <param name="regType">寄存器类型</param>
       /// <param name="startAddr">开始地址</param>
       /// <param name="startBin">位地址(无=0)</param>
       /// <param name="wVal"></param>
       /// <param name="er"></param>
       /// <returns></returns>
        public bool Write(int plcAddr, ERegType regType, int startAddr, int startBin, int wVal, out string er)
        {
            lock (_sync)
            {
                er = string.Empty;

                try
                {
                    int[] wVals = { wVal };

                    if (!Write(plcAddr, regType, startAddr, wVals, out er))
                        return false;

                    return true;
                }
                catch (Exception e)
                {
                    er = e.Message;
                    return false;
                }
            }
        }
        /// <summary>
        /// 写多个寄存器
        /// </summary>
        /// <param name="plcAddr">PLC地址(TCP为0)</param>
        /// <param name="regType">寄存器类型</param>
        /// <param name="startAddr">开始地址</param>
        /// <param name="wVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Write(int plcAddr, ERegType regType, int startAddr, int[] wVal, out string er)
        {
            lock (_sync)
            {
                try
                {
                    List<byte> commandBytes = getCommandByteArray(regType, startAddr, wVal.Length, false, wVal);

                    byte[] returnBytes;

                    if (!com.send(commandBytes.ToArray(), 30, out returnBytes, out er))
                        return false;

                    if (returnBytes.Length < 30)
                    {
                        er = "Time Out";
                        return false;
                    }

                    byte[] headerBytes = returnBytes.Skip(0).Take(19).ToArray();

                    if ((int)BCC(headerBytes.ToList()) != (int)returnBytes[19])
                    {
                        er = "BCC Check Error";
                        return false;
                    }

                    if (returnBytes[26] != 0 || returnBytes[27] != 0)
                    {
                        er = "Write Data Error";
                        return false;
                    }

                    if (returnBytes[20] != 0x59)
                    {
                        er = "Write Data Error";
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    er = e.Message;
                    return false;
                }
            }
        }
        #endregion

        #region XGT协议
        /*********************************XGT协议******************************************************
        //
        //一、发送
        //格式 = 公司Header + 命令 + 数据类型 + 数据
         //
        //1.公司Header = LSIS自有 + PLC信息 + CPU信息 + 帧方向 + 帧顺序编号 + 长度 + 信息位置 + 检查Sum
        //(1).LSIS自有：公司ID1："LSIS-XGT\n\n"  10个字节
        //(2).PLC信息:h00~hFF                    2个字节
        //(3).CPU信息:hA0                        1个字节
        //(4).帧方向:h33                         1个字节
        //(5).帧顺序编号:h0000~hFFFF             2个字节
        //(6).信息位置:h00~hFF                   1个字节
        //(7).检查Sum:h00~hFF                    1个字节
        //
        //2.命令 
        //  读取：h5400 写入：h5800              2个字节
        //
        //3.数据类型                             2个字节
        //  位：h0000 字节：h0010 字：h0020 双字：h0030 长字：h0040 连续：h0140
        //
        //4.数据 = 预留区域 + 块数目 + 变量长度 + 数据地址 + 数据数目
        //(1).预留区域:h0000                    2个字节
        //(2).块数目:h0100~h1000                2个字节
        //(3).变量长度:h0400~h1000              2个字节
        //(4).数据地址 例如"%DB1024","WB2048"
        //(5).数据数目

        //二、返回
        //格式 = 公司Header + 命令 + 数据类型 + 数据
        //
        //1.公司Header = LSIS自有 + PLC信息 + CPU信息 + 帧方向 + 帧顺序编号 + 长度 + 信息位置 + 检查Sum
        //(1).LSIS自有：公司ID1："LSIS-XGT\n\n"  10个字节
        //(2).PLC信息:h00~hFF                    2个字节
        //(3).CPU信息:hA0                        1个字节
        //(4).帧方向:h33                         1个字节
        //(5).帧顺序编号:h0000~hFFFF             2个字节
        //(6).信息位置:h00~hFF                   1个字节
        //(7).检查Sum:h00~hFF                    1个字节
        //
        //2.命令 
        //  读取：h5500 写入：h5900              2个字节
        //
        //3.数据类型                             2个字节
        //  位：h0000 字节：h0010 字：h0020 双字：h0030 长字：h0040 连续：h0140
        //
        //4.数据 = 预留区域 + 错误状态 + 数据
        //(1).预留区域:h0000                    2个字节
        //(2).错误状态:h0100~h1000                2个字节
        //(3).数据长度                        2个字节
        //(4).数据                            “长度”个字节    
         ************************************************************************************************/
        /// <summary>
        /// 获取命令字符串
        /// </summary>
        /// <param name="regType"></param>
        /// <param name="startAddr"></param>
        /// <param name="number"></param>
        /// <param name="isRead"></param>
        /// <param name="wVal"></param>
        /// <returns></returns>
        private List<byte> getCommandByteArray(ERegType regType, int startAddr, int number, bool isRead, int[] wVal)
        {
            List<byte> functionBytes = getFunctionByteArray(regType, startAddr, number, isRead, wVal);
            List<byte> headerBytes = getHeaderByteArray(functionBytes);

            headerBytes.AddRange(functionBytes);
            return headerBytes;
        }
        /// <summary>
        /// 获取帧头
        /// </summary>
        /// <param name="commandBytes"></param>
        /// <returns></returns>
        private List<byte> getHeaderByteArray(List<byte> commandBytes)
        {
            List<byte> headerBytes = new List<byte>();

            headerBytes.AddRange(System.Text.Encoding.UTF8.GetBytes("LSIS-XGT"));
            headerBytes.Add(0x00);
            headerBytes.Add(0x00); //公司ID

            headerBytes.Add(0x00);
            headerBytes.Add(0x00); //PLC信息

            headerBytes.Add(0xA0);  //CPU信息
            headerBytes.Add(0x33);  //帧源

            headerBytes.Add(0x00);
            headerBytes.Add(0x01); //Invoked ID

            headerBytes.Add((byte)((commandBytes.Count >> 0) & 0xFF));
            headerBytes.Add((byte)((commandBytes.Count >> 8) & 0xFF));      //长度

            headerBytes.Add(0x00); //位置

            //headerBytes.Add(BCC(headerBytes));
            headerBytes.Add(0x09);

            return headerBytes;

        }
        /// <summary>
        /// 获取功能码字符串
        /// </summary>
        /// <param name="regType"></param>
        /// <param name="startAddr"></param>
        /// <param name="number"></param>
        /// <param name="isRead"></param>
        /// <param name="wVal"></param>
        /// <returns></returns>
        private List<byte> getFunctionByteArray(ERegType regType, int startAddr, int number, bool isRead, int[] wVal)
        {
            List<byte> commandBytes = new List<byte>();

            if (isRead) //命令 54:读,58：写 占两个字节
            {
                commandBytes.Add(0x54);
                commandBytes.Add(0x00);
            }
            else
            {
                commandBytes.Add(0x58);
                commandBytes.Add(0x00);
            }

            commandBytes.Add(0x14);
            commandBytes.Add(0x00);  //数据类型 0000:位,0010:字节,0020:字,0030:双字,0040:长字,1400:连续

            commandBytes.Add(0x00);
            commandBytes.Add(0x00);  //预留区域

            commandBytes.Add(0x01);
            commandBytes.Add(0x00);  //块数目 0100~1000

            string devAddr = "%MB" + (startAddr * 2);

            if (regType == ERegType.D)
            {
                devAddr = "%DB" + (startAddr * 2);
            }

            commandBytes.Add((byte)((devAddr.Length >> 0) & 0xFF));
            commandBytes.Add((byte)((devAddr.Length >> 8) & 0xFF)); //设备类型地址数目

            commandBytes.AddRange(System.Text.Encoding.UTF8.GetBytes(devAddr)); //设备地址

            commandBytes.Add((byte)(((number * 2) >> 0) & 0xFF));
            commandBytes.Add((byte)(((number * 2) >> 8) & 0xFF));   //数据长度

            if (!isRead)
            {
                foreach (int val in wVal)
                {
                    if (val != 0)
                    {
                        commandBytes.Add((byte)(val % 256));
                        commandBytes.Add((byte)(val / 256));
                    }
                    else
                    {
                        commandBytes.Add(0x00);
                        commandBytes.Add(0x00);
                    }
                }
            }


            return commandBytes;
        }
        /// <summary>
        /// BCC计算
        /// </summary>
        /// <param name="headerBytes"></param>
        /// <returns></returns>
        private byte BCC(List<byte> headerBytes)
        {
            int iCheck = 0;

            foreach (byte b in headerBytes)
            {
                iCheck += b;
            }

            return (byte)(iCheck % 256);

        }
        #endregion

    }
}
