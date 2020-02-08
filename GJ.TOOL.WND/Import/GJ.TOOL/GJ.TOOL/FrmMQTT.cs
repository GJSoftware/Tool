using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GJ.COM;
using GJ.PLUGINS;
using GJ.Iot;
namespace GJ.TOOL
{
    public partial class FrmMQTT : Form, IChildMsg
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
            cycleTime = -1; 

            if (_mqtt != null)
            {
                _mqtt.OnMessageArgs.OnEvent -= new COnEvent<CMQTT.CMessageArgs>.OnEventHandler(OnMessageRecieve);
                _mqtt.Close();
                _mqtt = null;
            }
            if (_client!=null)
            {
                _client.OnMessageArgs.OnEvent -= new COnEvent<CClient.CCMessageArgs>.OnEventHandler(OnClientRecieve);
                _client.OnCmdRPTArgs.OnEvent -= new COnEvent<CClient.CCmdArgs>.OnEventHandler(OnClientRPTCmd);
                _client.OnCmdREQArgs.OnEvent -= new COnEvent<CClient.CCmdArgs>.OnEventHandler(OnClientREQCmd);
                _client.Close();
                _client = null;
            }
            if (_service != null)
            {
                _service.OnMessageArgs.OnEvent -= new COnEvent<CService.CCMessageArgs>.OnEventHandler(OnServiceRecieve);
                _service.OnStatusArgs.OnEvent -= new COnEvent<CService.CStatusArgs>.OnEventHandler(OnServiceStatus);
                _service.OnREQCmdArgs.OnEvent -= new COnEvent<CService.CCmdArgs>.OnEventHandler(OnServiceREQCmd);
                _service.Close();
                _service = null;
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
        public FrmMQTT()
        {
            InitializeComponent();

            InitialControl();

            SetDoubleBuffered();

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
        #endregion

        #region 字段
        private string iniFile = Application.StartupPath + "\\iniFile.ini";
        private string _ip = "127.0.0.1";
        private int _port = 61613;
        #endregion

        #region 面板回调函数
        private void FrmIot_Load(object sender, EventArgs e)
        {
            LoadIniFile();

            cmbMessagType.SelectedIndex = 0;

            cmbRunStatus.SelectedIndex = 0;

            cmbMesType.SelectedIndex = 0;

            cmbCmdType.SelectedIndex = 0;

            _clientDevIdNo = txtDevIdNo.Text;

            _clientDevName = txtDevName.Text;

            _clientmessageType = 0;

            _clientRunStatus = 0;

            cmbCmdInfo.SelectedIndex = 0;
        }
        #endregion

        #region MQTT协议
        private CMQTT _mqtt = null;   
        private string _topic = string.Empty;
        private string _message = string.Empty;
        private void btnCon_Click(object sender, EventArgs e)
        {
            try
            {
                btnCon.Enabled = false;

                string er = string.Empty;

                if (_mqtt == null)
                {
                    _mqtt = new CMQTT();

                    _mqtt.OnMessageArgs.OnEvent += new COnEvent<CMQTT.CMessageArgs>.OnEventHandler(OnMessageRecieve);

                    if (!_mqtt.Connect(txtSerIP.Text, System.Convert.ToInt32(txtSerPort.Text), out er))
                    {
                        ShowStatus(er, true);
                        _mqtt = null;
                        return;
                    }

                    ShowStatus("连接服务端正常", false);
                    btnCon.Text = "断开";
                    btnCon.ImageKey = "Connect";
                    btnPulish.Enabled = true;
                    btnSubscribe.Enabled = true;
                }
                else
                {
                    _mqtt.OnMessageArgs.OnEvent -= new COnEvent<CMQTT.CMessageArgs>.OnEventHandler(OnMessageRecieve);
                    _mqtt.Close();
                    _mqtt = null;
                    ShowStatus("断开服务端", false);
                    btnCon.ImageKey = "DisConnect";
                    btnCon.Text = "连接";
                    btnPulish.Enabled = false;
                    btnSubscribe.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                _mqtt = null;
                ShowStatus(ex.ToString(), false);
            }
            finally
            {
                SaveIniFile();
                btnCon.Enabled = true;
            }
        }
        private void btnSubscribe_Click(object sender, EventArgs e)
        {
            try
            {
                btnSubscribe.Enabled = false;

                string er = string.Empty;

                if (btnSubscribe.Text == "订阅主题")
                {
                    if (!_mqtt.Subscribe(txtTopic.Text, out er))
                    {
                        ShowStatus(er, true);
                        return;
                    }

                    ShowStatus("订阅消息主题正常", false);

                    btnSubscribe.Text = "取消订阅";
                }
                else
                {
                    if (!_mqtt.UnSubscribe(txtTopic.Text, out er))
                    {
                        ShowStatus(er, true);
                        return;
                    }

                    ShowStatus("取消订阅消息主题正常", false);

                    btnSubscribe.Text = "订阅主题";
                }
            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), false);
            }
            finally
            {
                SaveIniFile();
                btnSubscribe.Enabled = true;
            }
        }
        private void btnPulish_Click(object sender, EventArgs e)
        {
            try
            {
                btnPulish.Enabled = false;

                string er = string.Empty;

                if (!_mqtt.Publish(txtTopic.Text, txtMessage.Text, out er))
                {
                    ShowStatus(er, true);
                    return;
                }

                ShowStatus("发布消息内容正常", false);
            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), false);
            }
            finally
            {
                SaveIniFile();
                btnPulish.Enabled = true;
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            runLog.Clear();
        }
        private void OnMessageRecieve(object sender, CMQTT.CMessageArgs e)
        {
            ShowLog("【Time】:" + DateTime.Now.ToString("HH:mm:ss.") + DateTime.Now.Millisecond.ToString("D3") + "\r\n", Color.Black);

            ShowLog("【Topic】:", Color.Black);

            ShowLog(e.topic + "\r\n", Color.Blue);

            ShowLog("【Message】:", Color.Black);

            ShowLog(e.message + "\r\n", Color.Green);
        }
        private void ShowLog(string msg, Color color)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string, Color>(ShowLog), msg, color);
            else
            {
                int lines = runLog.TextLength;
                runLog.AppendText(msg);
                int lens = runLog.TextLength;
                runLog.Select(lines, lens);
                runLog.SelectionColor = color;
                runLog.ScrollToCaret();
            }
        }
        private void ShowStatus(string status, bool bAlarm)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string, bool>(ShowStatus), status, bAlarm);
            else
            {
                labStatus.Text = status;

                if (bAlarm)
                {
                    labStatus.ForeColor = Color.Red;
                }
                else
                {
                    labStatus.ForeColor = Color.Blue;
                }
            }
        }
        #endregion

        #region 客户端
        private int _nodeCount = 0;
        private CClient _client = null;
        private string _clientDevIdNo = string.Empty;
        private string _clientDevName = string.Empty;
        private int _clientmessageType = 0;
        private int _clientRunStatus = 0;
        private void btnClient_Click(object sender, EventArgs e)
        {
            try
            {
                btnClient.Enabled = false;

                string er = string.Empty;

                if (_client == null)
                {
                    List<CDevList> devices = new List<CDevList>();
                    devices.Add(new CDevList()
                                {
                                 idNo = txtDevIdNo.Text,
                                 Name = txtDevName.Text
                                });
                    _client = new CClient(0, "Client", CNet.HostName(), "LOADUP", txtClientFactory.Text, devices);
                    if (!_client.Connect(txtSerIP.Text, System.Convert.ToInt32(txtSerPort.Text), out er))
                    {
                        ShowClientStatus(er, true);
                        _client = null;
                        return;
                    }
                    _client.OnMessageArgs.OnEvent += new COnEvent<CClient.CCMessageArgs>.OnEventHandler(OnClientRecieve);
                    _client.OnCmdRPTArgs.OnEvent += new COnEvent<CClient.CCmdArgs>.OnEventHandler(OnClientRPTCmd);
                    _client.OnCmdREQArgs.OnEvent += new COnEvent<CClient.CCmdArgs>.OnEventHandler(OnClientREQCmd);
                    ShowClientStatus("连接服务端正常", false);
                    btnClient.Text = "断开";
                    btnClient.ImageKey = "Connect";
                    btnClientSend.Enabled = true;
                }
                else
                {
                    cycleTime = -1;
                    _client.OnMessageArgs.OnEvent -= new COnEvent<CClient.CCMessageArgs>.OnEventHandler(OnClientRecieve);
                    _client.OnCmdRPTArgs.OnEvent -= new COnEvent<CClient.CCmdArgs>.OnEventHandler(OnClientRPTCmd);
                    _client.OnCmdREQArgs.OnEvent -= new COnEvent<CClient.CCmdArgs>.OnEventHandler(OnClientREQCmd);
                    _client.Close();
                    _client = null;
                    ShowClientStatus("断开服务端", false);
                    btnClient.ImageKey = "DisConnect";
                    btnClient.Text = "连接";
                    btnClientSend.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                _client = null;
                ShowClientStatus(ex.ToString(), false);
            }
            finally
            {
                SaveIniFile();
                btnClient.Enabled = true;
            }
        }
        private void btnClientSend_Click(object sender, EventArgs e)
        {
            try
            {
                btnClientSend.Enabled = false;

                string er=string.Empty;

                if (cmbMessagType.SelectedIndex == 0)  //上报状态
                {
                    if (!Reponse_Status(out er))
                    {
                        ShowClientStatus(er, true);
                        return;
                    }
                }
                else if (cmbMessagType.SelectedIndex == 1) //回复指令
                {
                    if (!_client.Request_Command(out er))
                    {
                        ShowClientStatus(er, true);
                        return;
                    }
                }

                ShowClientStatus("发布消息OK", false);
            }
            catch (Exception ex)
            {
                ShowClientStatus(ex.ToString(), false);
            }
            finally
            {
                btnClientSend.Enabled = true;
            }
        }
        private void btnClearClient_Click(object sender, EventArgs e)
        {

            _nodeCount = 0;
            rbtClient.Clear();
        }
        private void txtDevIdNo_TextChanged(object sender, EventArgs e)
        {
            _clientDevIdNo = txtDevIdNo.Text;
        }
        private void txtDevName_TextChanged(object sender, EventArgs e)
        {
            _clientDevName = txtDevName.Text;  
        }       
        private void cmbMessagType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _clientmessageType = cmbMessagType.SelectedIndex;
        }
        private void cmbRunStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            _clientRunStatus = cmbRunStatus.SelectedIndex;
        }
        private bool Reponse_Status(out string er)
        {
            er = string.Empty;

            try
            {
                List<CData_Status> message = new List<CData_Status>();

                CData_Status data = new CData_Status()
                {
                    ID = _clientDevIdNo,
                    Name = _clientDevName,
                    Type = 0,
                    RunStatus = _clientRunStatus,
                    TTNum = 10,
                    FailNum = 1,
                    AlarmLevel = 0,
                    AlarmCode =string.Empty,
                    AlarmInfo =string.Empty,
                    Remark1 =string.Empty,
                    Remark2 =string.Empty
                };

                message.Add(data);

                if (!_client.Report_Status(message, out er))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        private void OnClientRecieve(object sender, CClient.CCMessageArgs e)
        {
            _nodeCount++;

            ShowClientLog("【Time】:" + DateTime.Now.ToString("HH:mm:ss.") + DateTime.Now.Millisecond.ToString("D3") + "\r\n", Color.Black);

            ShowClientLog("【Topic】:", Color.Black);

            ShowClientLog(e.topic + "\r\n", Color.Blue);

            ShowClientLog("【Message】:", Color.Black);

            ShowClientLog(e.message + "\r\n", Color.Green);

            ShowClientLog("--------------------------------------------------------------------------------------------------->" + 
                           _nodeCount.ToString() + "\r\n", Color.DarkOrange);
        }
        private void ShowClientLog(string msg, Color color)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string, Color>(ShowClientLog), msg, color);
            else
            {
                int lines = rbtClient.TextLength;
                rbtClient.AppendText(msg);
                int lens = rbtClient.TextLength;
                rbtClient.Select(lines, lens);
                rbtClient.SelectionColor = color;
                rbtClient.ScrollToCaret();
            }
        }
        private void ShowClientStatus(string status, bool bAlarm)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string, bool>(ShowClientStatus), status, bAlarm);
            else
            {
                labClient.Text = status;

                if (bAlarm)
                {
                    labClient.ForeColor = Color.Red;
                }
                else
                {
                    labClient.ForeColor = Color.Blue;
                }
            }
        }
        #endregion

        #region 客户端处理消息
        /// <summary>
        /// -1：停止上报 0:由客户端自己设置上报
        /// </summary>
        private double cycleTime = 0;
        private int rpt_count = 0;
        /// <summary>
        /// 监控时间
        /// </summary>
        private Stopwatch watcher_Status = new Stopwatch();
        /// <summary>
        /// 任务状态
        /// </summary>
        private bool task_Status = false;
        private void OnClientRPTCmd(object sender, CClient.CCmdArgs e)
        {
            ShowClientLog("【RPT COMMAND】:" + DateTime.Now.ToString("HH:mm:ss.") + DateTime.Now.Millisecond.ToString("D3") + "\r\n", Color.Black);

            ShowClientLog("【Topic】:", Color.Black);

            ShowClientLog(e.topic + "\r\n", Color.Blue);

            ShowClientLog("【Message】:", Color.Black);

            ShowClientLog(e.message + "\r\n", Color.Green);

            ECmdType cmdType = (ECmdType)e.data.Data[0].CmdType;

            ShowClientCmdName(e.data.Header.ID, e.data.Header.Name, cmdType.ToString());

            if (cmdType == ECmdType.上报状态)
            {
                cycleTime = System.Convert.ToDouble(e.data.Data[0].CmdInfo);

                ShowCycleTime(cycleTime);

                if (cycleTime != -1)
                {
                    if (!task_Status)
                    {
                        task_Status = true;
                        Task.Factory.StartNew(() => OnTask_Report()); 
                    }
                }
            }
        }
        private void OnClientREQCmd(object sender, CClient.CCmdArgs e)
        {
            ShowClientLog("【REQ COMMAND】:" + DateTime.Now.ToString("HH:mm:ss.") + DateTime.Now.Millisecond.ToString("D3") + "\r\n", Color.Black);

            ShowClientLog("【Topic】:", Color.Black);

            ShowClientLog(e.topic + "\r\n", Color.Blue);

            ShowClientLog("【Message】:", Color.Black);

            ShowClientLog(e.message + "\r\n", Color.Green);

            ECmdType cmdType = (ECmdType)e.data.Data[0].CmdType;

            ShowClientCmdName(e.data.Header.ID, e.data.Header.Name, cmdType.ToString());

            if (cmdType == ECmdType.控制指令)
            {
                string er = string.Empty;

                int runStatus = System.Convert.ToInt16(e.data.Data[0].CmdInfo);

                ShowRunStatus(runStatus);

                if (!_client.Reponse_Command(runStatus.ToString(), out er))
                {
                    ShowClientStatus(er, true);
                }
            }
        }
        private void ShowClientCmdName(string id,string name,string info)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string, string,string>(ShowClientCmdName),id,name, info);
            else
            {
                labSerId.Text = id;
                labSerName.Text = name;
                labCmdType.Text = info;
            }
        }
        private void ShowCycleTime(double cycleTime)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<double>(ShowCycleTime), cycleTime);
            else
            {
                if (cycleTime == -1)
                {
                    labCycleTime.Text = "停止上报";
                }
                else
                {
                    labCycleTime.Text = cycleTime.ToString() + "s";
                }
            }
        }
        private void ShowCountTime(int count)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<int>(ShowCountTime), count);
            else
            {
                labRPTTime.Text = count.ToString();
            }
        }
        private void ShowRunStatus(int status)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<int>(ShowRunStatus), status);
            else
            {
                cmbRunStatus.SelectedIndex = status;
            }
        }
        private void OnTask_Report()
        {
            try
            {
                rpt_count = 0;

                while (true)
                {
                    Thread.Sleep(5);

                    if (cycleTime <=0)
                        return;

                    if (watcher_Status.IsRunning && watcher_Status.ElapsedMilliseconds < cycleTime * 1000)
                        continue;

                    watcher_Status.Restart();
                
                    string er=string.Empty;

                    if (!Reponse_Status(out er))
                    {
                        ShowClientStatus(er, true);
                    }

                    rpt_count++;

                    ShowCountTime(rpt_count);
                }
            }
            catch (Exception ex)
            {
                ShowClientStatus(ex.ToString(), true);
            }
            finally
            {
                task_Status = false;
            }
        }
        #endregion

        #region 主控端
        private int _serCount = 0;
        private CService _service = null;
        private void btnServer_Click(object sender, EventArgs e)
        {
            try
            {
                btnServer.Enabled = false;

                string er = string.Empty;

                if (_service == null)
                {
                    _service = new CService(0, "Service", CNet.HostName(), "HOST", txtSetFactory.Text);
                    if (!_service.Connect(txtSerIP.Text, System.Convert.ToInt32(txtSerPort.Text), out er))
                    {
                        ShowServiceStatus(er, true);
                        _service = null;
                        return;
                    }
                    _service.OnMessageArgs.OnEvent += new COnEvent<CService.CCMessageArgs>.OnEventHandler(OnServiceRecieve);
                    _service.OnStatusArgs.OnEvent += new COnEvent<CService.CStatusArgs>.OnEventHandler(OnServiceStatus);
                    _service.OnRPTCmdArgs.OnEvent += new COnEvent<CService.CCmdArgs>.OnEventHandler(OnServiceRPTCmd);
                    _service.OnREQCmdArgs.OnEvent += new COnEvent<CService.CCmdArgs>.OnEventHandler(OnServiceREQCmd);

                    ShowServiceStatus("连接服务端正常", false);
                    btnServer.Text = "断开";
                    btnServer.ImageKey = "Connect";
                    btnServerCmd.Enabled = true;
                }
                else
                {
                    _service.OnMessageArgs.OnEvent -= new COnEvent<CService.CCMessageArgs>.OnEventHandler(OnServiceRecieve);
                    _service.OnStatusArgs.OnEvent -= new COnEvent<CService.CStatusArgs>.OnEventHandler(OnServiceStatus);
                    _service.OnRPTCmdArgs.OnEvent -= new COnEvent<CService.CCmdArgs>.OnEventHandler(OnServiceRPTCmd);
                    _service.OnREQCmdArgs.OnEvent -= new COnEvent<CService.CCmdArgs>.OnEventHandler(OnServiceREQCmd);                 
                    _service.Close();
                    _service = null;
                    ShowServiceStatus("断开服务端", false);
                    btnServer.ImageKey = "DisConnect";
                    btnServer.Text = "连接";
                    btnServerCmd.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                btnServer = null;
                ShowServiceStatus(ex.ToString(), false);
            }
            finally
            {
                SaveIniFile();
                btnServer.Enabled = true;
            }
        }
        private void btnServerCmd_Click(object sender, EventArgs e)
        {
            try
            {
                btnServerCmd.Enabled = false;

                string er = string.Empty;

                EMessageType messType = EMessageType.广播指令;

                string cmdInfo = txtCmdInfo.Text;

                if (cmbMesType.SelectedIndex == 1)
                {
                    messType = EMessageType.应答指令;

                    EDevRunStatus runStat = (EDevRunStatus)Enum.Parse(typeof(EDevRunStatus), cmbCmdInfo.Text);

                    cmdInfo = ((int)runStat).ToString();
                }

                ECmdType cmdType = (ECmdType)cmbCmdType.SelectedIndex;

                string cmdName = cmdType.ToString();               

                if (!_service.Publish_ALL(messType, cmdType, cmdName, cmdInfo, out er))
                {
                    ShowServiceStatus(er, true);
                    return;
                }

                ShowServiceStatus("发布消息OK", false);
            }
            catch (Exception ex)
            {
                ShowServiceStatus(ex.ToString(), false);
            }
            finally
            {
                btnServerCmd.Enabled = true;
            }
        }
        private void btnClearSer_Click(object sender, EventArgs e)
        {
            _serCount = 0;
            rtbService.Clear();
        }
        private void OnServiceRecieve(object sender, CService.CCMessageArgs e)
        {
            _serCount++;

            ShowServiceLog("【Time】:" + DateTime.Now.ToString("HH:mm:ss.") + DateTime.Now.Millisecond.ToString("D3") + "\r\n", Color.Black);

            ShowServiceLog("【Topic】:", Color.Black);

            ShowServiceLog(e.topic + "\r\n", Color.Blue);

            ShowServiceLog("【Message】:", Color.Black);

            ShowServiceLog(e.message + "\r\n", Color.Green);

            ShowServiceLog("--------------------------------------------------------------------------------------------------->" +
                           _serCount.ToString() + "\r\n", Color.DarkOrange);
        }
        private void cmbMesType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbCmdType.SelectedIndex = cmbMesType.SelectedIndex; 
        }
        private void ShowServiceLog(string msg, Color color)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string, Color>(ShowServiceLog), msg, color);
            else
            {
                int lines = rtbService.TextLength;
                if (lines > 20000)
                    rtbService.Clear();
                rtbService.AppendText(msg);
                int lens = rtbService.TextLength;
                rtbService.Select(lines, lens);
                rtbService.SelectionColor = color;
                rtbService.ScrollToCaret();
            }
        }
        private void ShowServiceStatus(string status, bool bAlarm)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string, bool>(ShowServiceStatus), status, bAlarm);
            else
            {
                labSerStatus.Text = status;

                if (bAlarm)
                {
                    labSerStatus.ForeColor = Color.Red;
                }
                else
                {
                    labSerStatus.ForeColor = Color.Blue;
                }
            }
        }
        #endregion

        #region 主控端处理消息
        private object cmdLock = new object();
        private class CDevice
        {
            public string IdNo = string.Empty;
            public string Name = string.Empty;
            public EDevRunStatus RunStatus = EDevRunStatus.未运行;
            public int Count = 0;
        }
        /// <summary>
        /// 设备列表
        /// </summary>
        private Dictionary<string, CDevice> _deviceList = new Dictionary<string, CDevice>();
        private Dictionary<string, int> _deviceRows = new Dictionary<string, int>();
        private void OnServiceStatus(object sender, CService.CStatusArgs e)
        {
            lock (cmdLock)
            {
                ShowServiceLog("【STATUS】:" + DateTime.Now.ToString("HH:mm:ss.") + DateTime.Now.Millisecond.ToString("D3") + "\r\n", Color.Black);

                ShowServiceLog("【Topic】:", Color.Black);

                ShowServiceLog(e.topic + "\r\n", Color.Blue);

                ShowServiceLog("【Message】:", Color.Black);

                ShowServiceLog(e.message + "\r\n", Color.Green);

                for (int i = 0; i < e.data.Data.Count; i++)
                {
                    ShowDeviceNode(e);
                }
            }
        }
        private void OnServiceRPTCmd(object sender, CService.CCmdArgs e)
        {
            lock (cmdLock)
            {
                ShowServiceLog("【RPT COMMAND】:" + DateTime.Now.ToString("HH:mm:ss.") + DateTime.Now.Millisecond.ToString("D3") + "\r\n", Color.Black);

                ShowServiceLog("【Topic】:", Color.Black);

                ShowServiceLog(e.topic + "\r\n", Color.Blue);

                ShowServiceLog("【Message】:", Color.Black);

                ShowServiceLog(e.message + "\r\n", Color.Green);

                ShowServiceStatus("接收到设备编号【" + e.data.Data[0].ID + "】请求指令", false);

                List<CDevList> devList = new List<CDevList>();

                for (int i = 0; i < e.data.Data.Count; i++)
                {
                    devList.Add(new CDevList()
                    {
                        idNo = e.data.Data[i].ID,
                        Name = e.data.Data[i].Name
                    }
                               );
                }

                string er = string.Empty;

                string cmdInfo = txtCmdInfo.Text;

                ECmdType cmdType = ECmdType.上报状态;

                string cmdName = cmdType.ToString();

                if (!_service.Publish_Device(EMessageType.广播指令, cmdType, cmdName, cmdInfo, devList, out er))
                {
                    ShowServiceStatus(er, true);
                    return;
                }
            }
        }
        private void OnServiceREQCmd(object sender, CService.CCmdArgs e)
        {
            lock (cmdLock)
            {
                ShowServiceLog("【REQ COMMAND】:" + DateTime.Now.ToString("HH:mm:ss.") + DateTime.Now.Millisecond.ToString("D3") + "\r\n", Color.Black);

                ShowServiceLog("【Topic】:", Color.Black);

                ShowServiceLog(e.topic + "\r\n", Color.Blue);

                ShowServiceLog("【Message】:", Color.Black);

                ShowServiceLog(e.message + "\r\n", Color.Green);

                ShowServiceStatus("接收到设备编号【" + e.data.Data[0].ID + "】应答指令", false);
            }
        }
        private void ShowDeviceNode(CService.CStatusArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<CService.CStatusArgs>(ShowDeviceNode),e);
            else
            {

                for (int i = 0; i < e.data.Data.Count; i++)
                {
                    string idNo = e.data.Data[i].ID;

                    EDevRunStatus runStatus = (EDevRunStatus)e.data.Data[i].RunStatus;

                    if (!_deviceList.ContainsKey(idNo))
                    {
                        _deviceList.Add(idNo, new CDevice()
                        {
                            IdNo = idNo,
                            Name = e.data.Data[i].Name,
                            RunStatus = runStatus,
                            Count = 0
                        });

                        NodeView.Rows.Add(NodeView.Rows.Count + 1, idNo, e.data.Data[i].Name, runStatus.ToString(), 
                                                                         e.data.Data[i].TTNum,e.data.Data[i].FailNum,1);
                        _deviceRows.Add(idNo, NodeView.Rows.Count);
                    }

                    _deviceList[idNo].RunStatus = runStatus;

                    _deviceList[idNo].Count++;

                    NodeView.Rows[_deviceRows[idNo] - 1].Cells[3].Value = runStatus.ToString();

                    NodeView.Rows[_deviceRows[idNo] - 1].Cells[4].Value = e.data.Data[i].TTNum;

                    NodeView.Rows[_deviceRows[idNo] - 1].Cells[5].Value = e.data.Data[i].FailNum;

                    NodeView.Rows[_deviceRows[idNo] - 1].Cells[6].Value = _deviceList[idNo].Count;
                }
              
            }
        }
        #endregion

        #region 方法
        private void LoadIniFile()
        {
            _ip = CIniFile.ReadFromIni("ToolDebug", "MQTT_IP", iniFile, "127.0.0.1");
            _port = System.Convert.ToInt32(CIniFile.ReadFromIni("ToolDebug", "MQTT_Port", iniFile, "61613"));
            _topic = CIniFile.ReadFromIni("ToolDebug", "MQTT_Topic", iniFile, "GJDG/PRD/KLK/CORE/NODE/SYSTEM/STATUS/RPT");
            _message = CIniFile.ReadFromIni("ToolDebug", "MQTT_Message", iniFile, "Hello World");
            txtSerIP.Text = _ip;
            txtSerPort.Text = _port.ToString();
            txtTopic.Text = _topic;
            txtMessage.Text = _message;
        }
        private void SaveIniFile()
        {
            _ip = txtSerIP.Text;
            _port = System.Convert.ToInt32(txtSerPort.Text);
            _topic = txtTopic.Text;
            _message = txtMessage.Text;
            CIniFile.WriteToIni("ToolDebug", "MQTT_IP", _ip, iniFile);
            CIniFile.WriteToIni("ToolDebug", "MQTT_Port", _port.ToString(), iniFile);
            CIniFile.WriteToIni("ToolDebug", "MQTT_Topic", _topic, iniFile);
            CIniFile.WriteToIni("ToolDebug", "MQTT_Message", _message, iniFile);
        }
        #endregion

    }
}
