using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GJ.PLUGINS;
using GJ.COM;
using GJ.DEV.COM;
using GJ.UI;
using System.Diagnostics;

namespace GJ.TOOL
{
    public partial class FrmSocket : Form, IChildMsg
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
            CloseTCPServer();
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
        public FrmSocket()
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
            clientIP = CIniFile.ReadFromIni("ToolDebug", "Socket_SerIP", iniFile, "127.0.0.1");
            tcpPort = System.Convert.ToInt32(CIniFile.ReadFromIni("ToolDebug", "Socket_SerPort", iniFile, "8000"));
            clientIP = CIniFile.ReadFromIni("ToolDebug", "Socket_clientIP", iniFile, "127.0.0.1");
            clientPort = System.Convert.ToInt32(CIniFile.ReadFromIni("ToolDebug", "Socket_clientPort", iniFile, "8000"));
        }
        private void SaveIniPara()
        {
            CIniFile.WriteToIni("ToolDebug", "Socket_SerIP",txtSerIP.Text, iniFile);
            CIniFile.WriteToIni("ToolDebug", "Socket_SerPort", txtSerPort.Text, iniFile);
            CIniFile.WriteToIni("ToolDebug", "Socket_clientIP", txtClientIP.Text, iniFile);
            CIniFile.WriteToIni("ToolDebug", "Socket_clientPort", txtClientPort.Text, iniFile);
        }
        #endregion

        #region 字段
        private string iniFile = Application.StartupPath + "\\iniFile.ini";
        private string xmlFile = Application.StartupPath + "\\XML\\Socket.xml";
        private string tcpIP = "127.0.0.1";
        private int tcpPort = 8000;
        private string clientIP = "127.0.0.1";
        private int clientPort = 8000;
        #endregion

        #region 面板回调函数
        private void FrmSocket_Load(object sender, EventArgs e)
        {
            txtSerIP.Text = tcpIP;

            txtSerPort.Text = tcpPort.ToString();

            txtClientIP.Text = clientIP;

            txtClientPort.Text = clientPort.ToString(); 
        }
        #endregion

        #region 服务端
        private CServerTCP _devSerTCP = null;
        private List<string> remoteClient = new List<string>();
        private void btnListen_Click(object sender, EventArgs e)
        {
            try
            {
                btnListen.Enabled = false;

                if (btnListen.Text == "监听")
                {
                    if (OpenTCPServer())
                    {
                        btnListen.Text = "停止";
                        btnListen.ImageKey = "Connect";
                        labSerStatus.Text = "启动监听正常";
                        labSerStatus.ForeColor = Color.Blue;

                        remoteClient.Clear();
                        cmbClientList.Items.Clear();
                        cmbClientList.Text = string.Empty;
                    }
                    else
                    {
                        labSerStatus.Text = "启动监听错误";
                        labSerStatus.ForeColor = Color.Red;
                    }
                }
                else
                {
                    CloseTCPServer();
                    btnListen.Text = "监听";
                    btnListen.ImageKey = "DisConnect";
                    labSerStatus.Text = "停止监听";
                    labSerStatus.ForeColor = Color.Black;

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                SaveIniPara();

                btnListen.Enabled = true;
            }
        }
        private void btnSend1_Click(object sender, EventArgs e)
        {
            try
            {
                btnSend1.Enabled = false;

                if (btnListen.Text == "监听")
                {
                    labSerStatus.Text = "未启动服务端监听";
                    labSerStatus.ForeColor = Color.Red;
                    return;
                }

                if (!remoteClient.Contains(cmbClientList.Text))
                {
                    labSerStatus.Text = "客户端已不存在";
                    labSerStatus.ForeColor = Color.Red;
                    return;
                }

                string er = string.Empty;

                string rData = string.Empty;

                string wData = txtSerMessage.Text;

                if (chkRtn1.Checked)
                {
                    wData += "\r\n";
                }

                _devSerTCP.send(cmbClientList.Text, wData, 0, out rData, out er);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                btnSend1.Enabled = true;
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                btnClear.Enabled = false;

                if (btnListen.Text == "监听")
                {
                    labSerStatus.Text = "未启动服务端监听";
                    labSerStatus.ForeColor = Color.Red;
                    return;
                }

                _devSerTCP.Clear();

                cmbClientList.Items.Clear();

                cmbClientList.Text = string.Empty;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                btnClear.Enabled = true;
            }
        }
        private void ChangeClient()
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(ChangeClient));
            else
            {
                cmbClientList.Items.Clear();

                for (int i = 0; i < remoteClient.Count; i++)
                {
                    cmbClientList.Items.Add(remoteClient[i]);
                }

                if (remoteClient.Count == 0)
                {
                    cmbClientList.Text = string.Empty;
                }
                else
                {
                    if (!remoteClient.Contains(cmbClientList.Text))
                    {
                        cmbClientList.SelectedIndex = 0;
                    }
                }                
            }
        }
        /// <summary>
        /// 打开TCP服务端
        /// </summary>
        /// <returns></returns>
        private bool OpenTCPServer()
        {
            try
            {
                if (_devSerTCP == null)
                {
                    _devSerTCP = new CServerTCP(0, "TCP服务端");
                    _devSerTCP.OnConed += new CServerTCP.EventOnConHander(OnTcpStatus);
                    _devSerTCP.OnRecved += new CServerTCP.EventOnRecvHandler(OnTcpRecv);
                    _devSerTCP.Listen(System.Convert.ToInt32(txtSerPort.Text));
                }
                return true;
            }
            catch (Exception ex)
            {
                SerLog.Log(ex.ToString(), udcRunLog.ELog.Err);
                return false;
            }
        }
        /// <summary>
        /// 断开服务端监听
        /// </summary>
        private void CloseTCPServer()
        {
            try
            {
                if (_devSerTCP != null)
                {
                    _devSerTCP.OnConed -= new CServerTCP.EventOnConHander(OnTcpStatus);
                    _devSerTCP.OnRecved -= new CServerTCP.EventOnRecvHandler(OnTcpRecv);
                    _devSerTCP.close();
                    _devSerTCP = null;
                    SerLog.Log("停止测试TCP服务器监听:端口" + "[" + tcpPort.ToString() + "]", udcRunLog.ELog.Action);
                }
            }
            catch (Exception ex)
            {
                SerLog.Log(ex.ToString(), udcRunLog.ELog.Err);
            }
        }
        /// <summary>
        /// TCP状态消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTcpStatus(object sender, CTcpConArgs e)
        {
            try
            {
                if (!e.bErr)
                {
                    if (!remoteClient.Contains(e.remoteIP))
                    { 
                        remoteClient.Add(e.remoteIP);

                        ChangeClient();
                    }

                    SerLog.Log(e.conStatus, udcRunLog.ELog.Action);

                }
                else
                {
                    if (remoteClient.Contains(e.remoteIP))
                    {
                        remoteClient.Remove(e.remoteIP);

                        ChangeClient();
                    }

                    SerLog.Log(e.conStatus, udcRunLog.ELog.NG);
                }
            }
            catch (Exception)
            {
                throw;
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
                SerLog.Log(e.recvData, udcRunLog.ELog.Action);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 客户端
        private CClientTCP _devTCP = null;
        private void btnCon_Click(object sender, EventArgs e)
        {
            try
            {
                btnCon.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    if (OpenTCP())
                    {
                        btnCon.Text = "断开";
                        btnCon.ImageKey = "Connect";
                        labClientStatus.Text = "连接正常";
                        labClientStatus.ForeColor = Color.Blue;
                    }
                    else
                    {
                        labClientStatus.Text = "启动监听错误";
                        labClientStatus.ForeColor = Color.Red;
                    }
                }
                else
                {
                    CloseTCP();
                    btnCon.Text = "连接";
                    btnCon.ImageKey = "DisConnect";
                    labClientStatus.Text = "断开连接";
                    labClientStatus.ForeColor = Color.Black;

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                SaveIniPara();
                btnCon.Enabled = true;
            }
        }
        private void btnSend2_Click(object sender, EventArgs e)
        {
            try
            {
                btnSend2.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    labClientStatus.Text = "未连接服务器";
                    labClientStatus.ForeColor = Color.Red;
                    return;
                }

                string er = string.Empty;

                string rData = string.Empty;

                string wData = txtClientMessage.Text;

                if (chkRtn.Checked)
                {
                    wData += "\r\n";
                }

                _devTCP.send(wData, 0, out rData, out er);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                btnSend2.Enabled = true;
            }
        }
        /// <summary>
        /// 连接TCP服务端
        /// </summary>
        /// <returns></returns>
        private bool OpenTCP()
        {
            try
            {
                string er = string.Empty;

                if (_devTCP == null)
                {
                    _devTCP = new CClientTCP(0, "Client", EDataType.ASCII格式);
                    _devTCP.OnConed += new CClientTCP.EventOnConHander(OnClientTcpCon);
                    _devTCP.OnRecved += new CClientTCP.EventOnRecvHandler(OnClientTcpRecv);
                    _devTCP.open(txtClientIP.Text, out er, txtClientPort.Text);

                    Stopwatch watcher = new Stopwatch();

                    watcher.Start();

                    while (true)
                    {
                        Application.DoEvents();

                        if (_devTCP.conStatus)
                            break;
                        if (watcher.ElapsedMilliseconds > 2000)
                            break;
                    }

                    watcher.Stop();

                    if (!_devTCP.conStatus)
                    {
                        _devTCP.OnConed -= new CClientTCP.EventOnConHander(OnClientTcpCon);
                        _devTCP.OnRecved -= new CClientTCP.EventOnRecvHandler(OnClientTcpRecv);
                        _devTCP.close();
                        _devTCP = null;
                        clientLog.Log("无法连接测试服务端[" + txtClientIP.Text + ":" +
                                                    txtClientPort.Text + "]", udcRunLog.ELog.NG);
                        return false;
                    }

                    clientLog.Log("正常连接测试服务端[" + txtClientIP.Text + ":" +
                                                txtClientPort.Text + "]", udcRunLog.ELog.Action);
                }

                return true;
            }
            catch (Exception ex)
            {
                clientLog.Log(ex.ToString(), udcRunLog.ELog.Err);
                return false;
            }
        }
        /// <summary>
        /// 断开TCP连接
        /// </summary>
        private void CloseTCP()
        {
            try
            {
                if (_devTCP != null)
                {
                    _devTCP.OnConed -= new CClientTCP.EventOnConHander(OnClientTcpCon);
                    _devTCP.OnRecved -= new CClientTCP.EventOnRecvHandler(OnClientTcpRecv);
                    _devTCP.close();
                    _devTCP = null;
                    clientLog.Log("断开连接测试服务端[" + txtClientIP.Text + ":" +
                                                          txtClientPort.Text + "]", udcRunLog.ELog.Content);
                }
            }
            catch (Exception ex)
            {
                clientLog.Log(ex.ToString(), udcRunLog.ELog.Err);
            }
        }
        private void OnClientTcpCon(object sender, CTcpConArgs e)
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
        private void OnClientTcpRecv(object sender, CTcpRecvArgs e)
        {
            clientLog.Log(e.recvData, udcRunLog.ELog.OK);  
        }
        #endregion

    }
}
