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
using GJ.DEV.BARCODE;
using GJ.PLUGINS;
using GJ.COM;
namespace GJ.TOOL
{
    public partial class FrmBarCode : Form,IChildMsg
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
            if (comMon != null)
            {
                comMon.Close();
                comMon = null;
                btnOpen.Text = "打开";
                labStatus.Text = "关闭串口.";
                labStatus.ForeColor = Color.Blue;
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
        public FrmBarCode()
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
        private CBarCOM comMon = null;
        #endregion

        #region 面板控件
    
        #endregion

        #region 面板回调函数
        private void FrmCR1000_Load(object sender, EventArgs e)
        {
            string[] com = System.IO.Ports.SerialPort.GetPortNames();
            cmbCOM.Items.Clear();
            for (int i = 0; i < com.Length; i++)
                cmbCOM.Items.Add(com[i]);
            if (com.Length > 0)
                cmbCOM.Text = com[0];
            cmbBarType.SelectedIndex = 0; 
        }
        private void cmbBarType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EBarType barType = (EBarType)Enum.Parse(typeof(EBarType), cmbBarType.Text);

            if (comMon == null)
            {
                comMon = new CBarCOM(barType, 0, cmbBarType.Text);

                if (comMon.comMode == EComMode.SerialPort)
                {
                    cmbCOM.Enabled = true;
                    txtSerIP.Enabled = false;
                    labSetting.Text = CLanguage.Lan("串口波特率:");
                    if (cmbBarType.Text == "SR700")
                        txtBand.Text = "115200,E,8,1";
                    else
                       txtBand.Text = "115200,N,8,1";
                }
                else
                {
                    cmbCOM.Enabled = false;
                    txtSerIP.Enabled = true;
                    labSetting.Text = CLanguage.Lan("TCP端口:");
                    if (cmbBarType.Text == "SR710_TCP")
                    {
                        txtSerIP.Text = "192.168.1.125";
                        txtBand.Text = "10001";
                    }
                    else
                    {
                        txtBand.Text = "10000";
                    }
                }

                comMon = null;
            }
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                btnOpen.Enabled = false;

                if (cmbCOM.Text == "" && txtSerIP.Text == string.Empty)
                {
                    labStatus.Text = CLanguage.Lan("请输入串口编号");
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                string er = string.Empty;

                if (comMon == null)
                {
                    if (!Enum.IsDefined(typeof(EBarType), cmbBarType.Text))
                    {
                        labStatus.Text = CLanguage.Lan("找不到类型") + "【" + cmbBarType.Text + "】";
                        labStatus.ForeColor = Color.Red;
                        return;
                    }

                    EBarType barType = (EBarType)Enum.Parse(typeof(EBarType), cmbBarType.Text);

                    comMon = new CBarCOM(barType, 0, cmbBarType.Text);

                    string comName = string.Empty;

                    string comSetting = string.Empty;

                    if (comMon.comMode == EComMode.SerialPort)
                    {
                        comName = cmbCOM.Text;
                        comSetting = txtBand.Text;
                    }
                    else
                    {
                        comName = txtSerIP.Text;
                        comSetting = txtBand.Text;
                    }

                    if (!comMon.Open(comName, out er, comSetting))
                    {
                        labStatus.Text = er;
                        labStatus.ForeColor = Color.Red;
                        comMon = null;
                        return;
                    }
                    btnOpen.Text = CLanguage.Lan("关闭");
                    labStatus.Text = CLanguage.Lan("成功打开串口");
                    labStatus.ForeColor = Color.Blue;
                    cmbCOM.Enabled = false;
                    cmbBarType.Enabled = false;
                    txtSerIP.Enabled = false;
                    txtBand.Enabled = false;
                }
                else
                {
                    comMon.Close();
                    comMon = null;
                    btnOpen.Text = CLanguage.Lan("打开");
                    labStatus.Text = CLanguage.Lan("关闭串口");
                    labStatus.ForeColor = Color.Blue;
                    cmbCOM.Enabled = true;
                    cmbBarType.Enabled = true;
                    txtSerIP.Enabled = true;
                    txtBand.Enabled = true;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                btnOpen.Enabled = true;
            }
        }
        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                btnRead.Enabled = false;

                if (comMon == null)
                {
                    labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                if (btnTriger.Text == CLanguage.Lan("停止触发"))
                {
                    labStatus.Text = CLanguage.Lan("请先停止触发");
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                Stopwatch watcher = new Stopwatch();

                watcher.Restart(); 

                string serialNo = string.Empty;

                string er = string.Empty;

                int delayTime = System.Convert.ToInt32(txtDelayTimes.Text);

                if (comMon.Read(out serialNo, out er, 0, delayTime))
                {
                    labSn.Text = serialNo;
                    labLen.Text = serialNo.Length.ToString();
                    labStatus.Text = CLanguage.Lan("读取条码") + "OK.";
                    labStatus.ForeColor = Color.Blue;
                }
                else
                {
                    labSn.Text = serialNo;
                    labStatus.Text = CLanguage.Lan("读取条码") + CLanguage.Lan("错误")+ ":" + er;
                    labStatus.ForeColor = Color.Red;
                }

                watcher.Stop();

                labTimes.Text = watcher.ElapsedMilliseconds.ToString() + "ms";
            }
            catch (Exception)
            {

            }
            finally
            {
                btnRead.Enabled = true;
            }           
        }
        private void btnTriger_Click(object sender, EventArgs e)
        {
            try
            {
                btnTriger.Enabled = false;

                string er=string.Empty;

                if (comMon == null)
                {
                    labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                runLog.Clear();

                _ttNum = 0;

                int delayTime = System.Convert.ToInt32(txtDelayTimes.Text);

                if (btnTriger.Text == CLanguage.Lan("启动触发"))
                {
                    _cts = new CancellationTokenSource();

                    Task.Factory.StartNew(() => OnScanTask(delayTime));

                    btnTriger.Text = CLanguage.Lan("停止触发");
                }
                else
                {
                    _cts.Cancel();
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                btnTriger.Enabled = true;
            }            
        }
        private void btn_Click(object sender, EventArgs e)
        {
            runLog.Clear();

            _ttNum = 0;
        }
        #endregion

        #region 连续扫描扫描
        private int _ttNum = 0;
        private CancellationTokenSource _cts = null;
        private void OnScanTask(int waitTime)
        {
            try
            {
                while (true)
                {

                    if (_cts.IsCancellationRequested)
                        return;

                    Thread.Sleep(500);

                    string serialNo = string.Empty;

                    string er = string.Empty;

                    Stopwatch watcher = new Stopwatch();

                    watcher.Restart();

                    if (!comMon.Read(out serialNo, out er, 0, waitTime))
                    {
                        watcher.Stop();

                        Log(er,watcher.ElapsedMilliseconds.ToString());
                    }
                    else
                    {
                        watcher.Stop();

                        Log(serialNo, er);
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                ShowEndTriger();
            }
        }
        private void ShowEndTriger()
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(ShowEndTriger));
            else
            {
                btnTriger.Text = CLanguage.Lan("启动触发");
            }
        }
        private void Log(string recv, string er)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string, string>(Log), recv, er);
            else
            {
                runLog.ScrollToCaret();

                runLog.AppendText(recv + "\r\n");

                if (runLog.Lines.Length > 200)
                    runLog.Clear();

                _ttNum++;

                labTT.Text = _ttNum.ToString();

                labTimes.Text = er;
            }
        }
        #endregion

    }
}
