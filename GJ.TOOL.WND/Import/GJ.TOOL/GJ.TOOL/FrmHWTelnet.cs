using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GJ.COM;
using GJ.PLUGINS;
using GJ.DEV.Telnet;
using System.Diagnostics;
namespace GJ.TOOL
{
    public partial class FrmHWTelnet : Form, IChildMsg
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
        public FrmHWTelnet()
        {
            InitializeComponent();

            InitialControl();

            SetDoubleBuffered();

            LoadIniFile();

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
        private CTelnet _client = null;
        private string _ip = "192.168.0.233";
        private int _port = 10001;
        #endregion

        #region 面板回调函数
        private void FrmHWTelnet_Load(object sender, EventArgs e)
        {
           
        }
        private void btnCon_Click(object sender, EventArgs e)
        {
            try
            {
                btnCon.Enabled = false;

                SaveIniFile();

                string er = string.Empty;

                if (_client == null)
                {
                    _client = new CTelnet();
                    if (!_client.Connect(_ip, _port, out er))
                    {
                        ShowStatus(er, true);
                        _client = null;
                        return;
                    }
                    ShowStatus("连接["+ _ip + ":" + _port.ToString() +"]正常", false);
                    btnCon.Text = "断开";
                    btnSendCmd.Enabled = true;
                    btnCheck.Enabled = true;
                    btnPower.Enabled = true;
                }
                else
                {
                    _client.Close();
                    _client = null;
                    ShowStatus("断开[" + _ip + ":" + _port.ToString() + "]连接", false);
                    btnCon.Text = "连接";
                    btnSendCmd.Enabled = false;
                    btnCheck.Enabled = false;
                    btnPower.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), true);
                _client = null;
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

                string er = string.Empty;

                string recv =string.Empty;

                string strCmd = txtCmd.Text;

                if (chkRtn.Checked)
                {
                    strCmd += "\r\n";
                }

                ShowLog(txtCmd.Text + "\r\n", Color.Blue);

                if (!_client.SendCmd(strCmd, out recv, out er))
                {
                    ShowStatus(er, true);
                    return;
                }

                ShowStatus(er, false);
                ShowLog(recv, Color.Green);
            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), true);
            }
            finally
            {
                btnSendCmd.Enabled = true;
            }
        }
        private void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                btnCheck.Enabled = false;

                string er = string.Empty;

                Stopwatch watcher = new Stopwatch();

                watcher.Start(); 

                if (!CheckTelnetReady(out er))
                {
                    ShowStatus(er, true);
                    return;
                }

                watcher.Stop();

                string waitTime = watcher.ElapsedMilliseconds.ToString() + "ms";

                ShowStatus("登录交换机正常:" + waitTime, false);
                return;
            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), true);
            }
            finally
            {
                btnCheck.Enabled = true;
            }
        }
        private void btnPower_Click(object sender, EventArgs e)
        {
            try
            {
                btnPower.Enabled = false;

                string er = string.Empty;

                string recv = string.Empty;

                string strCmd = "dis poe power interface MultiGE 0/0/" + txtCH.Text;

                Stopwatch watcher = new Stopwatch();

                watcher.Start(); 

                ShowLog(strCmd + "\r\n", Color.Blue);

                if (!_client.SendCmd(strCmd + "\n", out recv, out er))
                {
                    ShowStatus(er, true);
                    return;
                }

                string soi = "PD power(mW)";

                int len = soi.Length;

                double power = -1;

                string[] str = recv.Split('\n');

                for (int i = 0; i < str.Length; i++)
                {
                    str[i] = str[i].Replace("\r", "");

                    if (str[i].Length > len && str[i].Substring(0, len) == soi)
                    {
                        len = str[i].IndexOf(":");

                        string s = str[i].Substring(len + 1, str[i].Length - len - 1);

                        power = System.Convert.ToDouble(s);
                    }
                }

                labPower.Text = power.ToString();

                watcher.Stop();

                string waitTime = watcher.ElapsedMilliseconds.ToString() + "ms";

                if (power == -1)
                {
                    ShowStatus("读取产品功率错误:" + waitTime, true);
                }
                else
                {
                    ShowStatus("读取产品功率正常:" + waitTime, false);
                }

                ShowLog(recv, Color.Green);
            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), true);
            }
            finally
            {
                btnPower.Enabled = true;
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            runLog.Clear();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 检测交换机上电就绪
        /// </summary>
        /// <returns></returns>
        private bool CheckTelnetReady(out string er)
        {
            er = string.Empty;

            try
            {
                int tryTime = 3;

                string recvData = string.Empty;

                string value = string.Empty;
        
                for (int i = 0; i < tryTime; i++)
                {
                    string msg = string.Empty;

                    if (!_client.ReadBufferData(out value, out er))
                        return false;

                    if(value==string.Empty)
                    {
                       msg = string.Empty;

                       if (!ReadTelnetStream(msg,out recvData, out er))
                        return false;
                    }

                    if (ParseNameAndValue(recvData, "Username", out value))
                    {
                        msg = "admin";

                        if (!ReadTelnetStream(msg, out recvData, out er))
                            return false;
                    }

                    if (ParseNameAndValue(recvData, "Password", out value))
                    {
                        msg = "Admin@huawei.com";

                        if (!ReadTelnetStream(msg,out recvData, out er))
                            return false;
                    }

                    if (ParseNameAndValue(recvData, "<HUAWEI>", out value))
                        return true;

                    System.Threading.Thread.Sleep(1000); 
                }

                return false;
            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), true);
                return false;
            }
        }
        /// <summary>
        /// 获取Key值
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool ParseNameAndValue(string recvData, string keyName, out string value)
        {
            value = string.Empty;

            try
            {
                int len = keyName.Length;

                string[] str = recvData.Split('\n');

                for (int i = 0; i < str.Length; i++)
                {
                    str[i] = str[i].Replace("\r", "");

                    if (str[i].Length >= len && str[i].Substring(0, len) == keyName)
                    {
                        len = str[i].IndexOf(":");

                        if (len != -1)
                        {
                            value = str[i].Substring(len + 1, str[i].Length - len - 1);
                        }

                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), true);
                return false;
            }
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="recvData"></param>
        /// <returns></returns>
        private bool ReadTelnetStream(string strCmd, out string recvData, out string er)
        {
            recvData = string.Empty;

            er = string.Empty;

            try
            {
                strCmd += "\n";

                ShowLog(strCmd, Color.Blue);

                if (!_client.SendCmd(strCmd, out recvData, out er))
                    return false;

                ShowLog(recvData, Color.Green);

                return true;
            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), true);
                return false;
            }
        }
        private void ShowLog(string msg, Color color)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string, Color>(ShowLog), msg, color);
            else
            {
                int lines = runLog.TextLength;
                runLog.AppendText(msg);
                int lens  = runLog.TextLength;
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
        private void LoadIniFile()
        {
            _ip = CIniFile.ReadFromIni("ToolDebug", "Telnet_IP", iniFile, "192.168.0.233");
            _port = System.Convert.ToInt32(CIniFile.ReadFromIni("ToolDebug", "Telnet_Port", iniFile, "10001"));
            txtSerPort.Text = _ip;
            txtSerPort.Text = _port.ToString();
        }
        private void SaveIniFile()
        {
            _ip = txtSerIP.Text;
            _port = System.Convert.ToInt32(txtSerPort.Text);
            CIniFile.WriteToIni("ToolDebug", "Telnet_IP", _ip, iniFile);
            CIniFile.WriteToIni("ToolDebug", "Telnet_Port", _port.ToString(), iniFile);
        }
        #endregion

    }
}
