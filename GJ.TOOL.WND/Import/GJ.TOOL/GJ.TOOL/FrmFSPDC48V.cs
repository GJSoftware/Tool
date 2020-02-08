using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GJ.PLUGINS;
using System.Diagnostics;
using GJ.COM;
using GJ.DEV.FSP;

namespace GJ.TOOL
{
    public partial class FrmFSPDC48V : Form,IChildMsg
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
        public FrmFSPDC48V()
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
        private CDD_48V comMon = null;
        private Stopwatch watcher = new Stopwatch();
        #endregion

        #region 面板回调函数
        private void FrmFSPDC12V_Load(object sender, EventArgs e)
        {
            cmbCOM.Items.Clear();
            string[] com = System.IO.Ports.SerialPort.GetPortNames();
            for (int i = 0; i < com.Length; i++)
                cmbCOM.Items.Add(com[i]);
            if (com.Length > 0)
                cmbCOM.Text = com[0];
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                btnOpen.Enabled = false;

                string er = string.Empty;

                if (cmbCOM.Text == "")
                {
                    labStatus.Text = "请输入串口编号";
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                if (btnOpen.Text == "打开")
                {
                    comMon = new CDD_48V();

                    if (!comMon.Open(cmbCOM.Text, out er, txtBaud.Text))
                    {
                        comMon = null;
                        labStatus.Text = "打开串口失败:" + er;
                        labStatus.ForeColor = Color.Red;
                        return;
                    }

                    btnOpen.Text = "关闭";
                    labStatus.Text = "成功打开串口";
                    labStatus.ForeColor = Color.Blue;
                    cmbCOM.Enabled = false;
                }
                else
                {
                    if (comMon != null)
                    {
                        comMon.Close();
                        comMon = null;
                    }
                    btnOpen.Text = "打开";
                    labStatus.Text = "关闭串口";
                    labStatus.ForeColor = Color.Black;
                    cmbCOM.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnOpen.Enabled = true;
            }
        }
        private void btnSetPara_Click(object sender, EventArgs e)
        {
            try
            {
                btnSetPara.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                double dcv = System.Convert.ToDouble(txtVoltSetting.Text);

                double dci = System.Convert.ToDouble(txtCurSetting.Text);

                string er = string.Empty;

                if (comMon.SetModuelPara(addr, dcv, dci, out er))
                    showInfo("成功设置当前地址[" + addr.ToString("D2") + "]参数");
                else
                    showInfo("设置当前地址[" + addr.ToString("D2") + "]参数失败:" + er, true);
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnSetPara.Enabled = true;
            }
        }
        private void btnReadPara_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadPara.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                double dcv = 0;

                double dci = 0;

                string er = string.Empty;

                txtVoltSetting.Text = "0.0";

                txtCurSetting.Text = "0.0";

                if (!comMon.ReadModuelPara(addr, out dcv, out dci, out er))
                {
                    showInfo("读取当前地址[" + addr.ToString("D2") + "]参数失败:" + er, true);

                    return;
                }

                txtVoltSetting.Text = dcv.ToString("0.00");

                txtCurSetting.Text = dci.ToString("0.00");

                showInfo("读取当前地址[" + addr.ToString("D2") + "]参数设置");

            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnReadPara.Enabled = true;
            }
        }
        private void btnPowerOff_Click(object sender, EventArgs e)
        {
            try
            {
                btnPowerOff.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                int powerOff = 0;

                if (btnPowerOff.Text == "模块关机")
                {
                    powerOff = 1;
                }

                string er = string.Empty;

                if (!comMon.PowerOff(addr, powerOff, out er))
                {
                    showInfo("设置当前地址[" + addr.ToString("D2") + "]" + btnPowerOff.Text + "失败:" + er, true);

                    return;
                }

                showInfo("成功设置当前地址[" + addr.ToString("D2") + "]" + btnPowerOff.Text);

                if (powerOff == 1)
                    btnPowerOff.Text = "模块开机";
                else
                    btnPowerOff.Text = "模块关机";

            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnPowerOff.Enabled = true;
            }
        }
        private void btnReadData_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadData.Enabled = false;

                if (!checkSystem())
                    return;

                labDCV.Text = "---";
                labDCI.Text = "---";
                labTemp.Text = "---";
                labMStatus.Text = "---";
                labMStatus.ForeColor = Color.Black;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                CModule module = null;

                string er = string.Empty;

                if (!comMon.ReadModuleData(addr, out module, out er))
                {
                    showInfo("读取当前地址[" + addr.ToString("D2") + "]数据失败:" + er, true);

                    return;
                }

                labDCV.Text = module.Volt.ToString();
                labDCI.Text = module.Current.ToString();
                labTemp.Text = module.Temp.ToString();
                labMStatus.Text = module.Status + "|" + module.Alarm;

                if (module.Status == "正常" && module.Alarm == "正常")
                    labMStatus.ForeColor = Color.Blue;
                else
                    labMStatus.ForeColor = Color.Red;

                showInfo("读取当前地址[" + addr.ToString("D2") + "]数据");

            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnReadData.Enabled = true;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 检查设置
        /// </summary>
        /// <returns></returns>
        private bool checkSystem()
        {
            if (comMon == null)
            {
                labStatus.Text = "请确定已打开串口?";
                labStatus.ForeColor = Color.Red;
                return false;
            }
            if (txtAddr.Text == "")
            {
                labStatus.Text = "请输入地址.";
                labStatus.ForeColor = Color.Red;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="er"></param>
        /// <param name="alarm"></param>
        private void showInfo(string er, bool alarm = false)
        {
            if (!alarm)
            {
                labStatus.Text = er;
                labStatus.ForeColor = Color.Blue;
            }
            else
            {
                labStatus.Text = er;
                labStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region 扫描监控
        private int _rowNum = 0;
        private bool _scanStopFlag = false;
        private int _startAddr = 1;
        private int _endAddr = 36;
        private int _delayMs = 100;
        private delegate void ScanHandler();
        private void btnScan_Click(object sender, EventArgs e)
        {
            try
            {
                btnScan.Enabled = false;

                if (btnScan.Text == CLanguage.Lan("开始扫描"))
                {
                    if (comMon == null)
                    {
                        labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                        labStatus.ForeColor = Color.Red;
                        return;
                    }

                    _startAddr = System.Convert.ToInt16(txtStartAddr.Text);

                    _endAddr = System.Convert.ToInt16(txtEndAddr.Text);

                    DevView.Rows.Clear();

                    _rowNum = 0;

                    ScanHandler scanEvent = new ScanHandler(OnScan);

                    scanEvent.BeginInvoke(null, null);

                    _scanStopFlag = false;

                    btnScan.Text = CLanguage.Lan("停止扫描");
                }
                else
                {
                    _scanStopFlag = true;
                }
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnScan.Enabled = true;
            }
        }
        private void OnScan()
        {
            try
            {
                string er = string.Empty;

                for (int i = _startAddr; i <= _endAddr; i++)
                {
                    if (_scanStopFlag)
                        return;

                    System.Threading.Thread.Sleep(_delayMs);

                    bool result = true;

                    string status = string.Empty;

                    CModule module = null;

                    if (!comMon.ReadModuleData(i, out module, out er))
                    {
                        System.Threading.Thread.Sleep(50);

                        if (!comMon.ReadModuleData(i, out module, out er))
                        {
                            result = false;
                        }
                    }

                    if (result)
                    {
                        status = module.Status + "|" + module.Alarm;
                    }

                    ShowID(i, status, result);

                    _rowNum++;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ShowEnd();
            }
        }
        private void ShowEnd()
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(ShowEnd));
            else
            {
                btnScan.Text = CLanguage.Lan("开始扫描");
            }
        }
        private void ShowID(int Addr, string status, bool result)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<int, string, bool>(ShowID), Addr, status, result);
            else
            {
                if (result)
                {
                    DevView.Rows.Add(Addr, "PASS", status);
                    DevView.Rows[_rowNum].Cells[1].Style.BackColor = Color.LimeGreen;
                }
                else
                {
                    DevView.Rows.Add(Addr, "FAIL", status);
                    DevView.Rows[_rowNum].Cells[1].Style.BackColor = Color.Red;
                }
                DevView.CurrentCell = DevView.Rows[DevView.Rows.Count - 1].Cells[0];
            }
        }
        #endregion
       
    }
}
