using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using GJ.COM;
using GJ.DEV.FAN;
using System.Threading;
using GJ.PLUGINS;
namespace GJ.TOOL
{
    public partial class FrmGJFan : Form, IChildMsg
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
        public FrmGJFan()
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
            txtPWM = new List<TextBox>()
            {
                txtPWM1,txtPWM2,txtPWM3,txtPWM4,txtPWM5,
                txtPWM6,txtPWM7,txtPWM8,txtPWM9,txtPWM10
            };

            labPWM = new List<Label>()
            {
               labPWM1,labPWM2,labPWM3,labPWM4,labPWM5,
               labPWM6,labPWM7,labPWM8,labPWM9,labPWM10
            };

            labT = new List<Label>()
            {
                labT1,labT2,labT3,labT4,labT5,
                labT6,labT7,labT8,labT9,labT10
            };
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
        private const int MaxCH = 10;
        private CGJ_NTC_FAN comMon = null;
        private Stopwatch watcher = new Stopwatch();
        #endregion

        #region 面板控件
        private List<TextBox> txtPWM = null;
        private List<Label> labPWM = null;
        private List<Label> labT = null;
        #endregion

        #region 面板回调函数
        private void FrmGJFan_Load(object sender, EventArgs e)
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
                    comMon = new CGJ_NTC_FAN();

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
        private void btnSetAddr_Click(object sender, EventArgs e)
        {
            try
            {
                btnSetAddr.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                if (comMon.SetAddress(addr, out er))
                    showInfo("成功设置当前地址[" + addr.ToString("D2") + "]");
                else
                    showInfo("设置当前地址[" + addr.ToString("D2") + "]失败:" + er, true);
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnSetAddr.Enabled = true;
            }
        }
        private void btnVersion_Click(object sender, EventArgs e)
        {
            try
            {
                btnVersion.Enabled = false;

                labModuleVer.Text = string.Empty;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                int version = 0;

                Thread.Sleep(100);

                if (!comMon.ReadVersion(addr, out version, out er))
                {
                    Thread.Sleep(100);

                    if (!comMon.ReadVersion(addr, out version, out er))
                    {
                        labModuleVer.Text = "异常";

                        labModuleVer.ForeColor = Color.Red;

                        showInfo("读取地址[" + addr.ToString("D2") + "]设备版本错误:" + er, true);
                    }
                    else
                    {
                        labModuleVer.Text = version.ToString();

                        labModuleVer.ForeColor = Color.Blue;
                    }
                }
                else
                {
                    labModuleVer.Text = version.ToString();

                    labModuleVer.ForeColor = Color.Blue;

                    showInfo("读取地址[" + addr.ToString("D2") + "]设备版本信息OK");
                }
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnVersion.Enabled = true;
            }
        }
        private void btnSetPWM_Click(object sender, EventArgs e)
        {
            try
            {
                btnSetPWM.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                List<int> TemPWM = new List<int>();

                for (int i = 0; i < txtPWM.Count; i++)
                {
                    TemPWM.Add(System.Convert.ToInt16(txtPWM[i].Text)); 
                }

                Thread.Sleep(100);

                if (!comMon.SetTempControlPoint(addr, TemPWM, out er))
                {
                    Thread.Sleep(100);

                    if (!comMon.SetTempControlPoint(addr, TemPWM, out er))
                    {
                        showInfo("设置地址[" + addr.ToString("D2") + "]占空比温度错误:" + er, true);

                        return;
                    }                  
                }

                showInfo("设置地址[" + addr.ToString("D2") + "]占空比温度OK");

            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnSetPWM.Enabled = true;
            }
        }
        private void btnReadPWM_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadPWM.Enabled = false;

                for (int i = 0; i < labPWM.Count; i++)
                {
                    labPWM[i].Text = "-";
                }

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                List<int> TemPWM = new List<int>();

                Thread.Sleep(100);

                if (!comMon.ReadTempCtrlPoint(addr, out TemPWM, out er))
                {
                    Thread.Sleep(100);

                    if (!comMon.ReadTempCtrlPoint(addr, out TemPWM, out er))
                    {
                        showInfo("读取地址[" + addr.ToString("D2") + "]占空比温度错误:" + er, true);

                        return;
                    }
                }

                for (int i = 0; i < labPWM.Count; i++)
                {
                    labPWM[i].Text = TemPWM[i].ToString();
                }

                showInfo("读取地址[" + addr.ToString("D2") + "]占空比温度OK");

            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnReadPWM.Enabled = true;
            }
        }

        private void btnReadT_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadT.Enabled = false;

                for (int i = 0; i < labT.Count; i++)
                {
                    labT[i].Text = "-";
                }

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                List<int> Temp = new List<int>();

                Thread.Sleep(100);

                if (!comMon.ReadTempValue(addr, out Temp, out er))
                {
                    Thread.Sleep(100);

                    if (!comMon.ReadTempValue(addr, out Temp, out er))
                    {
                        showInfo("读取地址[" + addr.ToString("D2") + "]监控温度错误:" + er, true);

                        return;
                    }
                }

                for (int i = 0; i < labT.Count; i++)
                {
                    labT[i].Text = Temp[i].ToString();
                }

                showInfo("读取地址[" + addr.ToString("D2") + "]监控温度OK");

            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnReadT.Enabled = true;
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
        private int _delayMs = 50;
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

                    int rVer = 0;

                    if (!comMon.ReadVersion(i, out rVer, out er))
                    {
                        System.Threading.Thread.Sleep(50);

                        if (!comMon.ReadVersion(i, out rVer, out er))
                        {
                            result = false;
                        }
                    }

                    ShowID(i, rVer, result);

                    _rowNum++;
                }
            }
            catch (Exception)
            {
               
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
        private void ShowID(int Addr, int rVer, bool result)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<int, int, bool>(ShowID), Addr, rVer, result);
            else
            {
                if (result)
                {
                    DevView.Rows.Add(Addr, "PASS", rVer);
                    DevView.Rows[_rowNum].Cells[1].Style.BackColor = Color.LimeGreen;
                }
                else
                {
                    DevView.Rows.Add(Addr, "FAIL", rVer);
                    DevView.Rows[_rowNum].Cells[1].Style.BackColor = Color.Red;
                }
                DevView.CurrentCell = DevView.Rows[DevView.Rows.Count - 1].Cells[0];
            }
        }
        #endregion

    }
}
