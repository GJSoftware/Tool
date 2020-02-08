using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GJ.COM;
using GJ.DEV.COM;
namespace GJ.DEV.HuaWei
{
    /// <summary>
    /// 华为AP项目Socket信息交付类
    /// </summary>
    public class CAPSocket
    {
        #region 枚举
        /// <summary>
        /// 设备类型
        /// </summary>
        public enum EDevType
        {
           整体=0x0,
           上料位=0x2,
           下料位=0x3,
           移载机=0x4,
           老化位=0x5,
           缓存位=0x8,
           治具=0x9
        }
        /// <summary>
        /// 整体命令字
        /// </summary>
        public enum ESysCmdNo
        {
            查询告警 = 0x1,
            查询状态 = 0x3,
            库位插拔次数 = 0x4
        }
        /// <summary>
        /// 上料位命令字
        /// </summary>
        public enum ELoadUpCmdNo
        {
            查询状态 = 0x1
        }
        /// <summary>
        /// 下料位命令字
        /// </summary>
        public enum EUnLoadCmdNo
        {
            查询状态 = 0x1
        }
        /// <summary>
        /// 移载机命令字
        /// </summary>
        public enum ERobotCmdNo
        {
            查询状态 = 0x1,
            移库操作 = 0x4
        }
        /// <summary>
        /// 老化位命令字
        /// </summary>
        public enum EBICmdNo
        {
            查询设定温度 = 0x1,
            读取当前温度 = 0x2,
            查询所有库位 = 0x5,
            设定库位状态 = 0x6,
            查询单个库位 = 0x7,
            查询库位信息 = 0x8
        }
        /// <summary>
        /// 缓存位命令字
        /// </summary>
        public enum ECacheCmdNo
        {
            查询状态 =0x1
        }
        /// <summary>
        /// 治具命令字
        /// </summary>
        public enum EFixtureCmdNo
        {
            查询插拔次数 = 0x1
        }
        #endregion

        #region 消息定义
        /// <summary>
        /// 状态
        /// </summary>
        private enum EResult
        {
            Context,
            Action,
            OK,
            NG,
            Error
        }
        /// <summary>
        /// 日志消息
        /// </summary>
        public class CLogArgs : EventArgs
        {
            public readonly int idNo = 0;

            public string name = string.Empty;

            public string info = string.Empty;

            public int lPara = 0;

            public int wPara = 0;

            public CLogArgs(int idNo, string name, string info, int lPara = 0, int wPara = 0)
            {
                this.idNo = idNo;
                this.name = name;
                this.info = info;
                this.lPara = lPara;
                this.wPara = wPara;
            }
        }
        /// <summary>
        /// 消息响应
        /// </summary>
        public class CCmdArgs : EventArgs
        {
            public int devType = 0;

            public int cmdNo = 0;

            public int wr = 0;

            public string dataVal = string.Empty;

            public string remoteEndPoint = string.Empty;

            public string recvData = string.Empty;

            public CCmdArgs(int wr, int devType, int cmdNo, string dataVal, string remoteEndPoint, string recvData)
            {
                this.devType = devType;
                this.cmdNo = cmdNo;
                this.wr = wr;
                this.dataVal = dataVal;
                this.remoteEndPoint = remoteEndPoint;
                this.recvData = recvData;
            }
        }
        #endregion

        #region 协议定义
        /// <summary>
        /// 协议标识
        /// </summary>
        private const string SOI = "69";
        /// <summary>
        /// 数据域
        /// </summary>
        public class CData
        {
            /// <summary>
            /// 数据长度
            /// </summary>
            public string Len = "00";
            /// <summary>
            /// 数据
            /// </summary>
            public string Data = string.Empty;
        }
        /// <summary>
        /// 请求类
        /// </summary>
        public class CRequest
        {
            /// <summary>
            /// 协议标识
            /// </summary>
            public string SOI = "69";
            /// <summary>
            /// 设备类型域:上料机/下料机/移栽机/老化库位
            /// </summary>
            public string DevType = "01";
            /// <summary>
            /// 设备协议版本
            /// </summary>
            public string Version = "01";
            /// <summary>
            /// 报文标识
            /// </summary>
            public string CmdType = "0201";
            /// <summary>
            /// 标志位
            /// 由上位机（主控）向下位机发送则为0x00
            /// 由下位机向上位机发送为0x01
            /// </summary>
            public string CmdFlag = "01";
            /// <summary>
            /// 命令字
            /// </summary>
            public string CmdNo1 = "00";
            /// <summary>
            /// 保留长度:可变报文长度
            /// </summary>
            public string CmdLen = "00";
            /// <summary>
            /// 可变报文头
            /// </summary>
            public string CmdNo2 = string.Empty;
            /// <summary>
            /// 数据个数
            /// </summary>
            public string DataNum = "00";
            /// <summary>
            /// 数据域(长度+数据)
            /// </summary>
            public List<CData> Data = new List<CData>();
            /// <summary>
            /// 从帧起始符开始到校验码之前的所有各字节的模256的和
            /// 即各字节二进制算术和，不计超过256的溢出值
            /// </summary>
            public string CheckSum = "00";
        }
        /// <summary>
        /// 响应答类
        /// </summary>
        public class CReponse
        {
            /// <summary>
            /// 协议标识
            /// </summary>
            public string SOI = "69";
            /// <summary>
            /// 设备类型域:上料机/下料机/移栽机/老化库位
            /// </summary>
            public string DevType = "01";
            /// <summary>
            /// 设备协议版本
            /// </summary>
            public string Version = "01";
            /// <summary>
            /// 报文标识
            /// </summary>
            public string CmdType = "0201";
            /// <summary>
            /// 标志位：0x01 表示响应 
            /// </summary>
            public string CmdFlag = "01";
            /// <summary>
            /// 命令字
            /// </summary>
            public string CmdNo1 = "00";
            /// <summary>
            /// 保留长度:可变报文长度
            /// </summary>
            public string CmdLen = "00";
            /// <summary>
            /// 可变报文头
            /// </summary>
            public string CmdNo2 = string.Empty;
            /// <summary>
            /// 数据个数：2个
            /// 一个表示响应状态码
            /// 一个表示响应值
            /// </summary>
            public string DataNum = "02";
            /// <summary>
            /// 数据域(长度+数据)
            /// </summary>
            public List<CData> Data = new List<CData>();
            /// <summary>
            /// 从帧起始符开始到校验码之前的所有各字节的模256的和
            /// 即各字节二进制算术和，不计超过256的溢出值
            /// </summary>
            public string CheckSum = "00";
        }
        #endregion

        #region 数据包
        /// <summary>
        /// 报文数据包
        /// </summary>
        public class CPackage
        {
            /// <summary>
            /// 设备类型
            /// </summary>
            public int Name { get; set; }
            /// <summary>
            /// 命令字
            /// </summary>
            public int CmdNo { get; set; }
            /// <summary>
            /// 读写R:0 W:1 WR:2
            /// </summary>
            public int WR { get; set; }
            /// <summary>
            /// 数据包
            /// </summary>
            public List<string> Data { get; set; }
        }
        #endregion

        #region 构造函数
        public CAPSocket(int idNo, string name, Dictionary<string, CPackage> Packages)
        {
            this.idNo = idNo;
            this.name = name;
            this.Packages = Packages;
        }
        public override string ToString()
        {
            return this.name;
        }
        #endregion

        #region 消息定义
        /// <summary>
        /// 机种信息消息
        /// </summary>
        public COnEvent<CLogArgs> OnLogArgs = new COnEvent<CLogArgs>();
        /// <summary>
        /// 命令消息
        /// </summary>
        public COnEvent<CCmdArgs> OnCmdArgs = new COnEvent<CCmdArgs>();
        #endregion

        #region 字段
        /// <summary>
        /// 编号
        /// </summary>
        private int idNo = 0;
        /// <summary>
        /// 名称
        /// </summary>
        private string name = string.Empty;
        /// <summary>
        /// TCP端口
        /// </summary>
        private int tcpPort = 8000;
        /// <summary>
        /// 服务端口
        /// </summary>
        private CServerTCP serTcp = null;
        /// <summary>
        /// 报文数据包
        /// </summary>
        private Dictionary<string, CPackage> Packages { get; set; }
        #endregion

        #region TCP/IP事件
        /// <summary>
        /// TCP同步锁
        /// </summary>
        private ReaderWriterLock _TCPLock = new ReaderWriterLock();
        /// <summary>
        /// TCP状态消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTcpStatus(object sender, CTcpConArgs e)
        {
            try
            {
                _TCPLock.AcquireWriterLock(-1);

                if (!e.bErr)
                    OnLogArgs.OnEvented(new CLogArgs(0, name, e.conStatus, (int)EResult.Action));
                else
                    OnLogArgs.OnEvented(new CLogArgs(0, name, e.conStatus, (int)EResult.NG));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _TCPLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// TCP数据消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTcpRecv(object sender, CTcpRecvArgs e)
        {
            try
            {
                _TCPLock.AcquireWriterLock(-1);

                string er = string.Empty;

                int wr = 0;

                int devType = 0;

                int cmdNo = 0;

                string recvVal = string.Empty;

                string strReponse = string.Empty;

                if(!ReposeCommand(e.recvData,out wr,out devType,out cmdNo,out recvVal, out strReponse))
                {
                    OnLogArgs.OnEvented(new CLogArgs(1, name, e.recvData, (int)EResult.Action));
                    OnLogArgs.OnEvented(new CLogArgs(1, name, strReponse, (int)EResult.NG));
                    return;
                }

                if (wr == 2)
                {
                    OnLogArgs.OnEvented(new CLogArgs(1, name, e.recvData, (int)EResult.Action));
                    OnCmdArgs.OnEvented(new CCmdArgs(wr, devType, cmdNo, recvVal, e.remoteEndPoint, e.recvData));
                    return;
                }

                string rData = string.Empty;

                if (serTcp.send(e.remoteEndPoint, strReponse, 0, out rData, out er))
                {
                    OnLogArgs.OnEvented(new CLogArgs(1, name, e.recvData, (int)EResult.Action));
                    OnLogArgs.OnEvented(new CLogArgs(1, name, strReponse, (int)EResult.OK));
                    OnCmdArgs.OnEvented(new CCmdArgs(wr, devType, cmdNo, recvVal, e.remoteEndPoint, e.recvData)); 
                }
                else
                {
                    OnLogArgs.OnEvented(new CLogArgs(1, name, e.recvData, (int)EResult.Action));
                    OnLogArgs.OnEvented(new CLogArgs(1, name, strReponse, (int)EResult.NG));
                    OnCmdArgs.OnEvented(new CCmdArgs(wr, devType, cmdNo, recvVal, e.remoteEndPoint, e.recvData)); 
                }
               
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _TCPLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 启用监听
        /// </summary>
        /// <param name="tcpPort"></param>
        /// <returns></returns>
        public bool Listen(int tcpPort,out string er)
        {
            er=string.Empty;

            try
            {
                this.tcpPort = tcpPort;

                if (serTcp != null)
                { 
                   Close();
                }               
                serTcp = new CServerTCP(0, "TCP服务端");
                serTcp.OnConed += new CServerTCP.EventOnConHander(OnTcpStatus);
                serTcp.OnRecved += new CServerTCP.EventOnRecvHandler(OnTcpRecv);
                serTcp.Listen(tcpPort);
                return true;
            }
            catch (Exception ex)
            {                
                er=ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 关闭监听
        /// </summary>
        public void Close()
        {
            try
            {
                if (serTcp != null)
                {
                    serTcp.OnConed -= new CServerTCP.EventOnConHander(OnTcpStatus);
                    serTcp.OnRecved -= new CServerTCP.EventOnRecvHandler(OnTcpRecv);
                    serTcp.close();
                    serTcp = null;
                    OnLogArgs.OnEvented(new CLogArgs(idNo, name, "停止TCP服务器监听:端口" + "[" + tcpPort.ToString() + "]",(int)EResult.Context));
                }
            }
            catch (Exception ex)
            {
                OnLogArgs.OnEvented(new CLogArgs(idNo, name, ex.ToString(),(int)EResult.Error));
            }
        }
        /// <summary>
        /// 设置报文数据
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="package"></param>
        public void SetPackageValue(string keyName, CPackage package)
        {
            if (!Packages.ContainsKey(keyName))
            {
                Packages.Add(keyName, package);
            }
            else
            {
                Packages[keyName] = package;
            }
        }
        /// <summary>
        /// 设置报文数据
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="dataValue"></param>
        public void SetPackageValue(string keyName, string dataValue)
        {
            if (!Packages.ContainsKey(keyName))
                return;

            Packages[keyName].Data.Clear();

            string[] strArray = dataValue.Split(';');

            for (int z = 0; z < strArray.Length; z++)
            {
                Packages[keyName].Data.Add(strArray[z]);
            }
        }
        /// <summary>
        /// 应答客户端需求
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public void SendCmdToClient(string remoteEndPoint, string recvData)
        {
            try
            {
                string er = string.Empty;

                int wr = 0;

                int devType = 0;

                int cmdNo = 0;

                string recvVal = string.Empty;

                string strReponse = string.Empty;

                if (!ReposeCommand(recvData, out wr, out devType, out cmdNo, out recvVal, out strReponse))
                {
                    OnLogArgs.OnEvented(new CLogArgs(1, name, er, (int)EResult.NG));
                    return;
                }

                string rData = string.Empty;

                if (serTcp.send(remoteEndPoint, strReponse, 0, out rData, out er))
                {
                    OnLogArgs.OnEvented(new CLogArgs(1, name, strReponse, (int)EResult.OK));
                }
                else
                {
                    OnLogArgs.OnEvented(new CLogArgs(1, name, er, (int)EResult.NG));
                }
            }
            catch (Exception ex)
            {
                OnLogArgs.OnEvented(new CLogArgs(1, name, ex.ToString(), (int)EResult.Error));
            }
        }
        /// <summary>
        /// 应答消息
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        private bool ReposeCommand(string strData,out int wr,out int devType,out int cmdNo,out string strVal, out string strReponse)
        {
            strReponse = string.Empty;

            devType = 0;

            cmdNo = 0;

            wr = 0;

            strVal = string.Empty;

            try
            {
                string er = string.Empty;

                if (!CheckCommand(strData, out er))
                {
                    strReponse = er;
                    return false;
                }

                string DevType = strData.Substring(2, 2);

                string CmdNo = strData.Substring(12,2);

                string keyName = DevType + CmdNo;

                string strCmd = strData.Substring(0, 10);

                strCmd += "01";

                strCmd += CmdNo;

                strCmd +="0100";

                if (!Packages.ContainsKey(keyName))
                {
                    strReponse = "设备类型[" + DevType + "]和命令字[" + CmdNo + "]不存在";
                    return false;
                }

                wr = Packages[keyName].WR;

                devType = System.Convert.ToInt16(DevType, 16);

                cmdNo = System.Convert.ToInt16(CmdNo, 16);

                if (wr != 0) //返回写入数据内容
                {
                    int len = strData.Length;

                    string str = strData.Substring(9 * 2, len - 10 * 2);

                    int dataNum = System.Convert.ToInt16(str.Substring(0, 2), 16);

                    string recvData = str.Substring(2, str.Length - 2);

                    for (int i = 0; i < dataNum; i++)
                    {
                        int dataLen = System.Convert.ToInt16(recvData.Substring(0, 2), 16);

                        string strHex = recvData.Substring(2, dataLen * 2);

                        strVal += GetASCIIFromStrHex(strHex);

                        if (i != dataNum - 1)
                            strVal += ";";

                        recvData = recvData.Substring((dataLen + 1) * 2, recvData.Length - (dataLen + 1) * 2);
                    }
                }

                strCmd += Packages[keyName].Data.Count.ToString("X2"); //数据个数

                for (int i = 0; i < Packages[keyName].Data.Count; i++)
			    {
                    string s1 = Packages[keyName].Data[i].Length.ToString("X2"); //数据长度

                    string s2 = GetStrHexFromAscII(Packages[keyName].Data[i]);  //数据

                    strCmd += s1; //数据长度

                    strCmd += s2;
			    }

                strReponse = strCmd + CheckSum(strCmd);

                return true;
            }
            catch (Exception ex)
            {
                strReponse = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 检查接收命令
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool CheckCommand(string strData, out string er)
        {
            er = string.Empty;

            try
            {
                int len = 10 * 2;

                if (strData.Length < len)
                {
                    er = "接收数据长度错误:" + strData;
                    return false;
                }

                if (strData.Substring(0, 2) != SOI)
                {
                    er = "协议标识[" + SOI + "]错误:" + strData;
                    return false;
                }

                if (strData.Substring(10, 2) != "00")
                {
                    er = "标志位[00]错误:" + strData.Substring(10, 2);
                    return false;
                }

                string CheckSum = strData.Substring(strData.Length - 2, 2);

                int sum = 0;

                for (int i = 0; i < (strData.Length - 1)/2; i++)
                {
                    sum += System.Convert.ToInt16(strData.Substring(i * 2, 2), 16);  
                }

                string calCheckSum = (sum % 256).ToString("X2");

                if (CheckSum != calCheckSum)
                {
                    er = "校验和[" + CheckSum + "]与[" + calCheckSum + "]错误:" + strData;
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
        /// 字符串转16进制
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        private string GetStrHexFromAscII(string strData)
        {
            try
            {
                string strHex = string.Empty;

                for (int i = 0; i < strData.Length; i++)
                {
                    char c = System.Convert.ToChar(strData.Substring(i, 1));

                    strHex += System.Convert.ToByte(c).ToString("X2");
                }

                return strHex;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private string GetASCIIFromStrHex(string strHex)
        {
            try
            {
                string ASCII = string.Empty;

                for (int i = 0; i < strHex.Length / 2; i++)
                {
                    int valByte = System.Convert.ToByte(strHex.Substring(i * 2, 2), 16);

                    char c = System.Convert.ToChar(valByte);

                    ASCII += c;
                }

                return ASCII;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 校验和
        /// </summary>
        /// <param name="StrHex"></param>
        /// <returns></returns>
        private string CheckSum(string StrHex)
        {
            try
            {
                int sum = 0;

                for (int i = 0; i < StrHex.Length / 2; i++)
                {
                    sum += System.Convert.ToByte(StrHex.Substring(i * 2, 2), 16);
                }

                sum = sum % 256;

                string strVal = sum.ToString("X2");

                return strVal;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
