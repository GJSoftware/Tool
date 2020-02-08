using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.COM;
using GJ.DEV.COM;

namespace GJ.DEV.Temp
{
    public class CLK150002B
    {
        #region 构造函数
        public CLK150002B(int idNo = 0, string name = "LK150002B")
        {
            _idNo = idNo;
            _name = name;
        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 字段
        private int _idNo = 0;
        private string _name = "LK150002B";
        private bool _conStatus = false;
        private CSerialPort com = null;
        #endregion

        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        public int idNo
        {
            get { return _idNo; }
            set { _idNo = value; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public bool conStatus
        {
            get { return _conStatus; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="comName">38400,n,8,1</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Open(string comName, out string er, string setting = "9600,n,8,2")
        {
            er = string.Empty;

            try
            {
                if (com != null)
                {
                    com.close();
                    com = null;
                }

                com = new CSerialPort(idNo, name, EDataType.HEX格式);

                if (!com.open(comName, out er, setting))
                    return false;

                _conStatus = true;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            if (com == null)
                return;
            com.close();
            com = null;
            _conStatus = false;
        }
        #endregion

        #region 专用功能
        /// <summary>
        /// 启动机组
        /// </summary>
        /// <param name="devAddr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Start(int devAddr, out string er)
        {
            er = string.Empty;

            try
            {
                string wCmd = devAddr.ToString("X2");

                string rData = string.Empty;

                int regAddr = 0;

                int rLen = 8;

                wCmd += "05";                    //寄存器功能码为05

                wCmd += regAddr.ToString("X4");  //开机命令0000

                wCmd += "FF00";                  //有效命令FF00 无效命令0000

                wCmd += CCRC.Crc16(wCmd);                  //CRC16 低位前,高位后     

                if (!com.send(wCmd, rLen, out rData, out er))
                    return false;

                if (!checkCRC(rData))
                {
                    er = "crc16检验和错误:" + rData;
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
        /// 停止机组
        /// </summary>
        /// <param name="devAddr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Stop(int devAddr, out string er)
        {
            er = string.Empty;

            try
            {

                string wCmd = devAddr.ToString("X2");

                string rData = string.Empty;

                int regAddr = 1;

                int rLen = 8;

                wCmd += "05";                    //寄存器功能码为05

                wCmd += regAddr.ToString("X4");  //停止命令0001

                wCmd += "FF00";                  //有效命令FF00 无效命令0000

                wCmd += CCRC.Crc16(wCmd);                  //CRC16 低位前,高位后     

                if (!com.send(wCmd, rLen, out rData, out er))
                    return false;

                if (!checkCRC(rData))
                {
                    er = "crc16检验和错误:" + rData;
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
        /// 复位机组并消音
        /// </summary>
        /// <param name="devAddr"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Reset(int devAddr, out string er)
        {
            er = string.Empty;

            try
            {

                string wCmd = devAddr.ToString("X2");

                string rData = string.Empty;

                int regAddr = 8;

                int rLen = 8;

                wCmd += "05";                    //寄存器功能码为05

                wCmd += regAddr.ToString("X4");  //复位命令0008

                wCmd += "FF00";                  //有效命令FF00 无效命令0000

                wCmd += CCRC.Crc16(wCmd);                  //CRC16 低位前,高位后     

                if (!com.send(wCmd, rLen, out rData, out er))
                    return false;

                if (!checkCRC(rData))
                {
                    er = "crc16检验和错误:" + rData;
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
        ///  运行状态
        /// </summary>
        /// <param name="devAddr"></param>
        /// <param name="rData">0：待机 1：启动中 2：运行中 3：停机中 4：严重故障 5：防冻</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool RunState(int devAddr, out string rData, out string er)
        {
            er = string.Empty;

            rData = string.Empty;

            try
            {

                string wCmd = devAddr.ToString("X2");

                int rLen = 7;

                wCmd += "04";                    //寄存器功能码为05

                wCmd += "17000001";

                wCmd += CCRC.Crc16(wCmd);                  //CRC16 低位前,高位后     

                if (!com.send(wCmd, rLen, out rData, out er))
                    return false;

                if (!checkCRC(rData))
                {
                    er = "crc16检验和错误:" + rData;
                    return false;
                }

                rData = rData.Substring(6, 4);

                switch (rData)
                {
                    case "0000":
                        rData = "待机中";
                        break;
                    case "0001":
                        rData = "启动中";
                        break;
                    case "0002":
                        rData = "运行中";
                        break;
                    case "0003":
                        rData = "停机中";
                        break;
                    case "0004":
                        rData = "严重故障";
                        break;
                    case "0005":
                        rData = "防冻";
                        break;
                    default:
                        break;
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
        /// 空调温度
        /// </summary>
        /// <param name="devAddr"></param>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool AirTemp(int devAddr, out double[] rTemp, out string er)
        {
            er = string.Empty;

            rTemp = new double[2];

            bool bSta = true;

            try
            {
                for (int i = 0; i < 2; i++)
                {
                    string wCmd = devAddr.ToString("X2");

                    string rData = string.Empty;

                    long regAddr = 0x170A + i;

                    int rLen = 7;

                    wCmd += "04";                    //寄存器功能码为04

                    wCmd += Convert.ToString(regAddr, 16);

                    wCmd += "0001";

                    wCmd += CCRC.Crc16(wCmd);                  //CRC16 低位前,高位后     

                    if (!com.send(wCmd, rLen, out rData, out er))
                        return false;

                    if (!checkCRC(rData))
                    {
                        er = "crc16检验和错误:" + rData;
                        return false;
                    }
                    rData = rData.Substring(6, 4);

                    switch (rData)
                    {
                        case "8041":
                            rData = string.Empty;
                            er += "探头[" + (i + 1).ToString() + "]短路;";
                            break;
                        case "8042":
                            rData = string.Empty;
                            er += "探头[" + (i + 1).ToString() + "]断路;";
                            break;
                        case "8043":
                            rData = string.Empty;
                            er += "探头[" + (i + 1).ToString() + "]故障;";
                            break;
                        case "8044":
                            rData = string.Empty;
                            er += "探头[" + (i + 1).ToString() + "]不存在;";
                            break;
                        default:
                            rTemp[i] = (double)Convert.ToInt32(rData, 16) / 10;
                            break;
                    }
                    if (rData == "")
                        bSta = false;
                }
                return bSta;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        #endregion

        #region ModBus-RTU通信协议
        /// <summary>
        /// 检查CRC
        /// </summary>
        /// <param name="wCmd"></param>
        /// <returns></returns>
        private bool checkCRC(string wCmd)
        {
            string crc = CCRC.Crc16(wCmd.Substring(0, wCmd.Length - 4));
            if (crc != wCmd.Substring(wCmd.Length - 4, 4))
                return false;
            return true;
        }
        #endregion
    }
}
