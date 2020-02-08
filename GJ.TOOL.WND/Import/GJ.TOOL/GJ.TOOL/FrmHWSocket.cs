using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using GJ.PLUGINS;
using GJ.COM;
using GJ.UI;
using GJ.DEV.HuaWei;
using GJ.DEV.COM;

namespace GJ.TOOL
{
    public partial class FrmHWSocket : Form,IChildMsg
    {
        #region 插件方法
        /// <summary>
        /// 父窗口
        /// </summary>
        private Form _fatherForm = null;
        /// <summary>
        /// 父窗口GUID
        /// </summary>
        private string _fatherGuid = string.Empty;
        /// <summary>
        /// 显示当前窗口到父窗口容器中
        /// </summary>
        /// <param name="fatherForm">父窗口</param>
        /// <param name="control">父窗口容器</param>
        /// <param name="guid">父窗口全局名称</param>
        public void OnShowDlg(Form fatherForm, Control control, string guid)
        {
            _fatherForm = fatherForm;
            _fatherGuid = guid;
            this.Dock = DockStyle.Fill;
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Show();
            control.Controls.Add(this);
        }
        /// <summary>
        /// 关闭当前窗口
        /// </summary>
        public void OnCloseDlg()
        {
            if (clientTcp != null)
            {
                CloseClientTcp();
            }
            if (serTcp != null)
            {
                serTcp.Close();
                serTcp.OnLogArgs.OnEvent -= new COnEvent<CAPSocket.CLogArgs>.OnEventHandler(OnSerLog);
                serTcp = null;
            }
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mPwrLevel"></param>
        public void OnLogIn(string user, int[] mPwrLevel)
        {

        }
        /// <summary>
        /// 启动运行
        /// </summary>
        public void OnStartRun()
        {

        }
        /// <summary>
        /// 停止运行
        /// </summary>
        public void OnStopRun()
        {

        }
        /// <summary>
        /// 切换语言 
        /// </summary>
        public void OnChangeLAN()
        {

        }
        /// <summary>
        /// 消息事件
        /// </summary>
        /// <param name="para"></param>
        public void OnMessage(string name, int lPara, int wPara)
        {

        }
        #endregion

        #region 构造函数
        public FrmHWSocket()
        {
            InitializeComponent();

            InitialControl();

            SetDoubleBuffered();

            LoadIniPara();
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitialControl()
        {
               
        }
        /// <summary>
        /// 设置双缓冲,防止界面闪烁
        /// </summary>
        private void SetDoubleBuffered()
        {
            CUISetting.SetUIDoubleBuffered(this);
        }
        private void LoadIniPara()
        {
            tcpPort = System.Convert.ToInt32(CIniFile.ReadFromIni("ToolDebug", "Socket_SerTcpPort", iniFile, "8000"));

            xmlFile = CIniFile.ReadFromIni("ToolDebug", "Socket_Xml", iniFile, Application.StartupPath + "\\XML\\Socket.xml");

            clientIP = CIniFile.ReadFromIni("ToolDebug", "Socket_clientIP", iniFile, "127.0.0.1");

            clientPort = CIniFile.ReadFromIni("ToolDebug", "Socket_clientPort", iniFile, "8000");
        }
        #endregion

        #region 字段
        private const string SOI = "69";
        private string iniFile = Application.StartupPath + "\\iniFile.ini";
        private string xmlFile = Application.StartupPath + "\\XML\\Socket.xml";
        private int tcpPort = 8000;
        private CAPSocket serTcp = null;
        private Dictionary<string, CAPSocket.CPackage> packages = new Dictionary<string,CAPSocket.CPackage>();
        private CClientTCP clientTcp = null;
        private string clientIP = "127.0.0.1";
        private string clientPort = "8000";
        #endregion

        #region 面板回调函数
        private void FrmHWSocket_Load(object sender, EventArgs e)
        {
            txtSerPort.Text = tcpPort.ToString();

            txtSerXML.Text = xmlFile;

            cmbDevType.SelectedIndex = 0;

            txtSerIP.Text = clientIP;

            txtClientPort.Text = clientPort; 
        }
        private void btnListen_Click(object sender, EventArgs e)
        {
            try
            {
                btnListen.Enabled = false;

                string er=string.Empty;

                tcpPort = System.Convert.ToInt32(txtSerPort.Text); 

                if (serTcp == null)
                {
                    if (!LoadXmlPara(xmlFile))
                        return;

                    serTcp = new CAPSocket(0, "TCP服务端", packages);

                    serTcp.OnLogArgs.OnEvent += new COnEvent<CAPSocket.CLogArgs>.OnEventHandler(OnSerLog);

                    serTcp.OnCmdArgs.OnEvent += new COnEvent<CAPSocket.CCmdArgs>.OnEventHandler(OnSerCmd);

                    if (!serTcp.Listen(tcpPort, out er))
                    {
                        labSerStatus.Text = er;
                        labSerStatus.ForeColor = Color.Red;
                        return;
                    }

                    btnListen.Text = "停止";
                    btnListen.ImageKey = "Connect";
                }
                else
                {
                    serTcp.Close();

                    serTcp.OnLogArgs.OnEvent -= new COnEvent<CAPSocket.CLogArgs>.OnEventHandler(OnSerLog);

                    serTcp.OnCmdArgs.OnEvent -= new COnEvent<CAPSocket.CCmdArgs>.OnEventHandler(OnSerCmd);

                    serTcp = null;

                    btnListen.Text = "监听";

                    btnListen.ImageKey = "DisConnect";
                
                }

                CIniFile.WriteToIni("ToolDebug", "Socket_SerTcpPort", tcpPort.ToString(), iniFile);
            }
            catch (Exception ex)
            {
                labSerStatus.Text = ex.ToString();
                labSerStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnListen.Enabled = true;
            }
        }
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                BtnLoad.Enabled = false;

                OpenFileDialog openFiledlg = new OpenFileDialog();
                openFiledlg.InitialDirectory = Application.StartupPath + "\\XML";
                openFiledlg.Filter = "Para files (*.xml)|*.xml";
                if (openFiledlg.ShowDialog() != DialogResult.OK)
                    return;

                if (!LoadXmlPara(openFiledlg.FileName))
                    return;

                txtSerXML.Text = openFiledlg.FileName;

                xmlFile =openFiledlg.FileName;

                CIniFile.WriteToIni("ToolDebug", "Socket_Xml", xmlFile, iniFile);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                BtnLoad.Enabled = true;
            }
        }
        private void cmbDevType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbCmdNo.Items.Clear();

            if (cmbDevType.SelectedIndex == 0)  //整体
            {
                cmbCmdNo.Items.Add("查询告警(0x01)");   
                cmbCmdNo.Items.Add("查询状态(0x03)"); 
                cmbCmdNo.Items.Add("库位插拔次数(0x04)"); 
            }
            else if (cmbDevType.SelectedIndex == 1) //上料位
            { 
               cmbCmdNo.Items.Add("查询状态(0x01)");   
            }
            else if (cmbDevType.SelectedIndex == 2)  //下料位
            {
                cmbCmdNo.Items.Add("查询状态(0x01)");
            }
            else if (cmbDevType.SelectedIndex == 3)  //移载机
            {
                cmbCmdNo.Items.Add("查询状态(0x01)");
                cmbCmdNo.Items.Add("移库操作(0x04)");
            }
            else if (cmbDevType.SelectedIndex == 4)  //老化位
            {
                cmbCmdNo.Items.Add("查询设定温度(0x01)");
                cmbCmdNo.Items.Add("读取当前温度(0x02)");
                cmbCmdNo.Items.Add("查询所有库位(0x05)");
                cmbCmdNo.Items.Add("设定库位状态(0x06)");
                cmbCmdNo.Items.Add("查询单个库位(0x07)");
                cmbCmdNo.Items.Add("查询库位信息(0x08)");
            }
            else if (cmbDevType.SelectedIndex == 5)  //缓存位
            {
                cmbCmdNo.Items.Add("查询状态(0x01)");
            }
            else if (cmbDevType.SelectedIndex == 6)  //治具
            {
                cmbCmdNo.Items.Add("查询插拔次数(0x01)");
            }

            cmbCmdNo.SelectedIndex = 0;
        }
        private void btnCon_Click(object sender, EventArgs e)
        {
            try
            {
                btnCon.Enabled = false;

                string er = string.Empty;

                clientIP = txtSerIP.Text;

                clientPort = txtSerPort.Text;

                if (clientTcp == null)
                {
                    if (!OpenClientTcp(txtSerIP.Text, txtSerPort.Text, out er))
                    {
                        labClientStatus.Text = er;
                        labClientStatus.ForeColor = Color.Red;
                        return;
                    }
                    labClientStatus.Text = er;
                    labClientStatus.ForeColor = Color.Blue;

                    btnSendCmd.Enabled = true;
                    btnCon.Text = "断开";
                    btnCon.ImageKey = "Connect"; 
                }
                else
                {
                    CloseClientTcp();

                    labClientStatus.Text = "断开服务器连接";
                    labClientStatus.ForeColor = Color.Blue;

                    btnSendCmd.Enabled = false;
                    btnCon.Text = "连接";
                    btnCon.ImageKey = "DisConnect"; 
                }

                CIniFile.WriteToIni("ToolDebug", "Socket_clientIP", clientIP, iniFile);

                CIniFile.WriteToIni("ToolDebug", "Socket_clientPortt", clientPort, iniFile);
            }
            catch (Exception ex)
            {
                labClientStatus.Text = ex.ToString();
                labClientStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnCon.Enabled = true;
            }
        }
        private void btnSendCmd_Click(object sender, EventArgs e)
        {
            try
            {
                btnSendCmd.Enabled = false;

                string devType = GetStrHexFromString(cmbDevType.Text);

                string cmdNo = GetStrHexFromString(cmbCmdNo.Text);

                string strData = txtCmdData.Text;

                string StrCmd = FormatRequestCommand(devType, cmdNo, strData);

                string er = string.Empty;

                string recvData = string.Empty;

                string recvVal = string.Empty;

                int len = 10 * 2;

                clientLog.Log("[发送]:" + StrCmd, udcRunLog.ELog.Action);

                Stopwatch watcher = new Stopwatch();

                watcher.Start();

                txtSendCmd.Text = StrCmd;

                if (!clientTcp.send(StrCmd, len, out recvData, out er))
                {
                    watcher.Stop();
                    labClientStatus.Text = "接收数据超时:" + watcher.ElapsedMilliseconds.ToString() + "ms";
                    labClientStatus.ForeColor = Color.Red;
                    return;
                }

                txtRecvCmd.Text = recvData;

                clientLog.Log("[接收]:" + recvData, udcRunLog.ELog.OK);

                watcher.Stop();

                if (!ReponseCommand(recvData, out recvVal))
                {
                    txtRevcVal.Text = recvVal;
                    txtRevcVal.ForeColor = Color.Red;
                    labClientStatus.Text = "接收数据错误:" + watcher.ElapsedMilliseconds.ToString() + "ms";
                    labClientStatus.ForeColor = Color.Blue;
                }
                else
                {
                    txtRevcVal.Text = recvVal;
                    txtRevcVal.ForeColor = Color.Black;
                    labClientStatus.Text = "接收数据正常:" + watcher.ElapsedMilliseconds.ToString() + "ms";
                    labClientStatus.ForeColor = Color.Blue;
                }
              
            }
            catch (Exception ex)
            {
                labClientStatus.Text = ex.ToString();
                labClientStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnSendCmd.Enabled = true;
            }
        }
        #endregion

        #region 面板方法
        private void ShowRequest(CAPSocket.CCmdArgs e)
        { 
           if(this.InvokeRequired)
               this.Invoke(new Action<CAPSocket.CCmdArgs>(ShowRequest),e);
           else
           {
               listSer.Items.Clear();

               if (e.wr == 0)
                   labWR.Text = "读操作";
               else
                   labWR.Text = "写操作";

               CAPSocket.EDevType devType = (CAPSocket.EDevType)e.devType;

               labSerDevName.Text = devType.ToString() + "(0x" + e.devType.ToString("X2") + ")";

               string cmdName = string.Empty;

               switch (devType)
               {
                   case CAPSocket.EDevType.整体:
                       cmdName = ((CAPSocket.ESysCmdNo)e.cmdNo).ToString();
                       break;
                   case CAPSocket.EDevType.上料位:
                       cmdName = ((CAPSocket.ELoadUpCmdNo)e.cmdNo).ToString();
                       break;
                   case CAPSocket.EDevType.下料位:
                       cmdName = ((CAPSocket.EUnLoadCmdNo)e.cmdNo).ToString();
                       break;
                   case CAPSocket.EDevType.移载机:
                       cmdName = ((CAPSocket.ERobotCmdNo)e.cmdNo).ToString();
                       break;
                   case CAPSocket.EDevType.老化位:
                       cmdName = ((CAPSocket.EBICmdNo)e.cmdNo).ToString();
                       break;
                   case CAPSocket.EDevType.缓存位:
                       cmdName = ((CAPSocket.ECacheCmdNo)e.cmdNo).ToString();
                       break;
                   case CAPSocket.EDevType.治具:
                       cmdName = ((CAPSocket.EFixtureCmdNo)e.cmdNo).ToString();
                       break;
                   default:
                       break;
               }

               labSerCmdNo.Text = cmdName + "(0x" + e.cmdNo.ToString("X2") + ")";

               if (e.dataVal != string.Empty)
               {
                   string[] dataVal = e.dataVal.Split(';');

                   for (int i = 0; i < dataVal.Length; i++)
                   {
                       listSer.Items.Add(dataVal[i]); 
                   }
               }
           }
        }
        #endregion

        #region 方法
        private bool LoadXmlPara(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("加载XML文件[" + filePath + "]不存在", "XML文件", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
                    return false;
                }

                packages.Clear();

                StreamReader sr = new StreamReader(filePath,Encoding.UTF8);

                string strXml = sr.ReadToEnd();

                sr.Close();

                DataSet ds = CXml.ConvertXMLToDataSet(strXml);

                if (ds == null)
                {
                    MessageBox.Show("加载XML文件参数错误", "XML文件", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                if (ds.DataSetName != "Socket")
                {
                    MessageBox.Show("加载XML文件参数[Socket]错误", "XML文件", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string DeviceType = ds.Tables[0].Rows[i]["DeviceType"].ToString();
                    string CommandNo = ds.Tables[0].Rows[i]["CommandNo"].ToString();
                    string CmdWR = ds.Tables[0].Rows[i]["CmdWR"].ToString();
                    string DataValue = ds.Tables[0].Rows[i]["DataValue"].ToString();

                    CAPSocket.CPackage pack = new CAPSocket.CPackage();
                    
                    pack.Name=System.Convert.ToByte(DeviceType, 16);

                    pack.CmdNo = System.Convert.ToByte(CommandNo, 16);

                    pack.WR = System.Convert.ToByte(CmdWR, 16);

                    pack.Data = new List<string>();

                    string[] strArray = DataValue.Split(';');

                    for (int z = 0; z < strArray.Length; z++)
                    {
                        pack.Data.Add(strArray[z]);
                    }

                    packages.Add(DeviceType + CommandNo,pack);
                        
                }

                return true;
            }
            catch (Exception)
            {                
                throw;
            }
        }
        private bool OpenClientTcp(string ip,string port, out string er)
        {
            try
            {
                clientTcp = new CClientTCP(0, "Socket客户端", EDataType.ASCII格式);
                clientTcp.OnConed += new CClientTCP.EventOnConHander(OnTcpCon);
                clientTcp.OnRecved += new CClientTCP.EventOnRecvHandler(OnTcpRecv);
                clientTcp.open(ip, out er, port);

                Stopwatch watcher = new Stopwatch();

                watcher.Start();

                while (true)
                {
                    Application.DoEvents();

                    if (clientTcp.conStatus)
                        break;
                    if (watcher.ElapsedMilliseconds > 2000)
                        break;
                }

                watcher.Stop();

                if (!clientTcp.conStatus)
                {
                    er = "无法连接测试服务端[" + ip.ToString() + ":" + port + "]";
                    clientTcp.OnConed -= new CClientTCP.EventOnConHander(OnTcpCon);
                    clientTcp.OnRecved -= new CClientTCP.EventOnRecvHandler(OnTcpRecv);
                    clientTcp.close();
                    clientTcp = null;                    
                    clientLog.Log(er, udcRunLog.ELog.NG);
                    return false;
                }

                er = "正常连接测试服务端[" + ip.ToString() + ":" + port + "]";

                clientLog.Log(er, udcRunLog.ELog.Action);

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                clientLog.Log(er, udcRunLog.ELog.Err);
                return false;
            }           
        }
        private void CloseClientTcp()
        {
            try
            {
                if (clientTcp != null)
                {
                    clientTcp.OnConed -= new CClientTCP.EventOnConHander(OnTcpCon);
                    clientTcp.OnRecved -= new CClientTCP.EventOnRecvHandler(OnTcpRecv);
                    clientTcp.close();
                    clientTcp = null;
                    clientLog.Log("断开连接测试服务端[" + txtSerIP.Text + ":" + txtSerPort.Text + "]", udcRunLog.ELog.NG);                               
                }
            }
            catch (Exception ex)
            {
                clientLog.Log(ex.ToString(), udcRunLog.ELog.Err);
            }
        }
        private string GetStrHexFromString(string strData)
        {
            try
            {
                int index = strData.IndexOf("0x");

                string strHex = strData.Substring(index + 2, strData.Length - index - 3);

                string str = string.Empty;

                for (int i = 0; i < strHex.Length; i++)
                {
                    char c = System.Convert.ToChar(strHex.Substring(i, 1));

                    if (Char.IsLetterOrDigit(c))
                        str += strHex.Substring(i, 1);
                }
                return str;
            }
            catch (Exception)
            {                
                throw;
            }
        }
        private string GetStrHexFromAscII(string strData)
        {
            try
            {
                string strHex = string.Empty;

                for (int i = 0; i < strData.Length; i++)
                {
                    char c =System.Convert.ToChar(strData.Substring(i, 1));

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

                for (int i = 0; i < strHex.Length/2; i++)
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
        private string CheckSum(string StrHex)
        {
            try
            {
                int sum = 0;

                for (int i = 0; i < StrHex.Length/2; i++)
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
        private string FormatRequestCommand(string devType, string cmdNo, string strData)
        {
            try
            {
                string CmdType = Environment.TickCount.ToString("X4");

                string StrCmd = "69"; //协议标识

                StrCmd += devType;  //设备类型域

                StrCmd += "01";   //设备协议版本

                StrCmd += CmdType.Substring(CmdType.Length - 4, 4); //报文标识

                StrCmd += "00"; //标志位

                StrCmd += cmdNo; //命令字

                StrCmd += "0100"; //可变报文长度

                if (strData.Length == 0)  
                {
                    StrCmd += "00";            //数据个数
                }
                else
                {
                    string[] strArray = strData.Split(';');

                    StrCmd += strArray.Length.ToString("X2"); //数据个数

                    for (int i = 0; i < strArray.Length; i++)
                    {
                         StrCmd += strArray[i].Length.ToString("X2"); //数据个数

                         StrCmd += GetStrHexFromAscII(strArray[i]);
                    }
                }

                StrCmd += CheckSum(StrCmd);

                return StrCmd;
            }
            catch (Exception)
            {                
                throw;
            }
        }
        /// <summary>
        /// 解析接收数据
        /// </summary>
        /// <param name="recvData"></param>
        /// <param name="recvVal"></param>
        /// <returns></returns>
        private bool ReponseCommand(string recvData, out string recvVal)
        {
            recvVal = string.Empty;

            try
            {
                if (!CheckCommand(recvData, out recvVal))
                    return false;

                int len = recvData.Length;

                string strData = recvData.Substring(9 * 2, len - 10 * 2);

                int dataNum = System.Convert.ToInt16(strData.Substring(0, 2),16);

                strData = strData.Substring(2, strData.Length - 2);

                for (int i = 0; i < dataNum; i++)
                {
                    int dataLen = System.Convert.ToInt16(strData.Substring(0, 2),16);

                    string strHex = strData.Substring(2, dataLen * 2);

                    recvVal += GetASCIIFromStrHex(strHex);

                    if (i != dataNum-1)
                        recvVal += ";";

                    strData = strData.Substring((dataLen + 1) * 2, strData.Length - (dataLen + 1) * 2);
                }

                return true;
            }
            catch (Exception ex)
            {
                recvVal = ex.ToString();

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

                if (strData.Substring(10, 2) != "01")
                {
                    er = "标志位[01]错误:" + strData.Substring(10, 2);
                    return false;
                }

                string CheckSum = strData.Substring(strData.Length - 2, 2);

                int sum = 0;

                for (int i = 0; i < (strData.Length - 1) / 2; i++)
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
        #endregion

        #region 客户端TCP/IP消息
        private void OnTcpCon(object sender, CTcpConArgs e)
        {
            if (!e.bErr)
            {
                clientLog.Log(e.conStatus, udcRunLog.ELog.Action); 
            }
            else
            {
                clientLog.Log(e.conStatus, udcRunLog.ELog.NG); 
            }
        }
        private void OnTcpRecv(object sender, CTcpRecvArgs e)
        {
              
        }
        #endregion

        #region 服务端消息响应
        private void OnSerLog(object sender, CAPSocket.CLogArgs e)
        {
            string info = e.info;

            if (e.idNo == 1)
            {
                if (e.lPara == 1)
                {
                    info = "[接收]:" + e.info;
                }
                else
                {
                    info = "[发送]:" + e.info;
                }
            }            

            if (e.lPara == 0)
            {
                SerLog.Log(info, udcRunLog.ELog.Content);
            }
            else if (e.lPara == 1)
            {
                SerLog.Log(info, udcRunLog.ELog.Action);
            }
            else if (e.lPara == 2)
            {
                SerLog.Log(info, udcRunLog.ELog.OK);
            }
            else if (e.lPara == 3)
            {
                SerLog.Log(info, udcRunLog.ELog.NG);
            }
            else
            {
                SerLog.Log(info, udcRunLog.ELog.Err);
            }
        }
        private void OnSerCmd(object sender, CAPSocket.CCmdArgs e)
        {
            ShowRequest(e);
        }
        #endregion

    }
}
