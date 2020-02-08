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
using GJ.COM;
using GJ.DEV.Switch;
using GJ.PLUGINS;
namespace GJ.TOOL
{
    public partial class FrmRS485_I2C : Form, IChildMsg
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
        public FrmRS485_I2C()
        {
            InitializeComponent();

            InitialControl();
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitialControl()
        {
            chkMon = new CheckBox[] { 
                                      checkBox1, checkBox2, checkBox3, checkBox4,
                                      checkBox5, checkBox6, checkBox7, checkBox8,
                                      checkBox9, checkBox10, checkBox11, checkBox12,
                                      checkBox13
                                      };
            labMon = new Label[]{
                                 labMon1,labMon2,labMon3,labMon4,
                                 labMon5,labMon6,labMon7,labMon8,
                                 labMon9,labMon10,labMon11,labMon12,
                                 labMon13
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
        private CRS485_I2C comMon = null;
        private Stopwatch watcher = new Stopwatch();
        #endregion

        #region 面板控件
        private CheckBox[] chkMon = null;
        private Label[] labMon = null;
        #endregion

        #region 面板回调函数
        private void FrmRS485_I2C_Load(object sender, EventArgs e)
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
                    comMon = new CRS485_I2C();

                    if (!comMon.Open(cmbCOM.Text, out er, txtBaud.Text))
                    {
                        comMon = null;
                        labStatus.Text = "打开串口失败:" + er;
                        labStatus.ForeColor = Color.Red;
                        return;
                    }

                    int addr = 0;

                    int i2cAddr = System.Convert.ToInt16(txtI2CAddr.Text, 16);

                    for (int i = 0; i < 3; i++)
                    {
                        Thread.Sleep(200);
 
                        comMon.SetI2CAddr(addr, i2cAddr, out er);
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
        private void btnInfo_Click(object sender, EventArgs e)
        {
            try
            {
                btnInfo.Enabled = false;

                labModuleVer.Text = string.Empty;

                labModuleName.Text = string.Empty;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                bool chkFlag = true;

                string er = string.Empty;

                string version = string.Empty;

                string name = string.Empty;

                Thread.Sleep(100);

                if (!comMon.ReadVersion(addr, out version, out er))
                {
                    Thread.Sleep(100);

                    if (!comMon.ReadVersion(addr, out version, out er))
                    {
                        chkFlag = false;

                        labModuleVer.Text = "异常";

                        labModuleVer.ForeColor = Color.Red;

                        showInfo("读取地址[" + addr.ToString("D2") + "]设备版本错误:" + er, true);
                    }
                    else
                    {
                        labModuleVer.Text = version;

                        labModuleVer.ForeColor = Color.Blue;
                    }
                }
                else
                {
                    labModuleVer.Text = version;

                    labModuleVer.ForeColor = Color.Blue;
                }

                //Thread.Sleep(100);

                //if (!comMon.ReadName(addr, out name, out er))
                //{
                //    Thread.Sleep(100);

                //    if (!comMon.ReadName(addr, out name, out er))
                //    {
                //        chkFlag = false;

                //        labModuleName.Text = er;

                //        labModuleName.ForeColor = Color.Red;

                //        showInfo("读取地址[" + addr.ToString("D2") + "]设备名称错误:" + er, true);
                //    }
                //    else
                //    {
                //        labModuleName.Text = name;

                //        labModuleName.ForeColor = Color.Blue;
                //    }
                //}
                //else
                //{
                //    labModuleName.Text = name;

                //    labModuleName.ForeColor = Color.Blue;
                //}

                if (chkFlag)
                {
                    showInfo("读取地址[" + addr.ToString("D2") + "]基本信息OK");
                }

            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnInfo.Enabled = true;
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

                if (comMon.SetNewAddr(addr, out er))
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
        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                btnRead.Enabled = false;

                if (!checkSystem())
                    return;

                labACV.Text = string.Empty;

                labACI.Text = string.Empty;

                labDCV.Text = string.Empty;

                labDCI.Text = string.Empty;

                labModuleStatus.Text = string.Empty;

                bool ChkFlag = true;

                string er = string.Empty;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                //1.读输入电压 

                double acv = 0;
                
                Thread.Sleep(50);

                if (!comMon.ReadInputVolt(addr, out acv, out er))
                { 
                    Thread.Sleep(50);

                    if (!comMon.ReadInputVolt(addr, out acv, out er))
                    {
                        labACV.Text = "异常";
                        labACV.ForeColor = Color.Red;
                        ChkFlag = false;
                    }
                    else
                    {
                        labACV.Text = acv.ToString("0.00");
                        labACV.ForeColor = Color.Black;
                    }
                }
                else
                {
                    labACV.Text = acv.ToString("0.00");
                    labACV.ForeColor = Color.Black;
                }

                //2.读输入电流 

                double aci = 0;

                Thread.Sleep(50);

                if (!comMon.ReadInputCurrent(addr, out aci, out er))
                {
                    Thread.Sleep(50);

                    if (!comMon.ReadInputCurrent(addr, out aci, out er))
                    {
                        labACI.Text = "异常";
                        labACI.ForeColor = Color.Red;
                        ChkFlag = false;
                    }
                    else
                    {
                        labACI.Text = acv.ToString("0.00");
                        labACI.ForeColor = Color.Black;
                    }
                }
                else
                {
                    labACI.Text = aci.ToString("0.00");
                    labACI.ForeColor = Color.Black;
                }

                //3.读输出电压 

                double dcv = 0;

                Thread.Sleep(50);

                if (!comMon.ReadOutputVolt(addr, out dcv, out er))
                {
                    Thread.Sleep(50);

                    if (!comMon.ReadOutputVolt(addr, out dcv, out er))
                    {
                        labDCV.Text = "异常";
                        labDCV.ForeColor = Color.Red;
                        ChkFlag = false;
                    }
                    else
                    {
                        labDCV.Text = dcv.ToString("0.00");
                        labDCV.ForeColor = Color.Black;
                    }
                }
                else
                {
                    labDCV.Text = dcv.ToString("0.00");
                    labDCV.ForeColor = Color.Black;
                }

                //4.读输入电流 

                double dci = 0;

                Thread.Sleep(50);

                if (!comMon.ReadOutputCurrent(addr, out dci, out er))
                {
                    Thread.Sleep(50);

                    if (!comMon.ReadOutputCurrent(addr, out dci, out er))
                    {
                        labDCI.Text = "异常";
                        labDCI.ForeColor = Color.Red;
                        ChkFlag = false;
                    }
                    else
                    {
                        labDCI.Text = dci.ToString("0.00");
                        labDCI.ForeColor = Color.Black;
                    }
                }
                else
                {
                    labDCI.Text = dci.ToString("0.00");
                    labDCI.ForeColor = Color.Black;
                }

                //4.读输入电流 

                string  status = string.Empty;

                Thread.Sleep(50);

                if (!comMon.ReadStatus(addr, out status, out er))
                {
                    Thread.Sleep(50);

                    if (!comMon.ReadStatus(addr, out status, out er))
                    {
                        labModuleStatus.Text = "异常";
                        labModuleStatus.ForeColor = Color.Red;
                        ChkFlag = false;
                    }
                    else
                    {
                        labModuleStatus.Text = status;

                        if(status=="正常")
                           labModuleStatus.ForeColor = Color.Blue;
                        else
                           labModuleStatus.ForeColor = Color.Red;
                    }
                }
                else
                {
                    labModuleStatus.Text = status;

                    if (status == "正常")
                    {
                        labModuleStatus.ForeColor = Color.Blue;
                    }
                    else
                    {
                        labModuleStatus.ForeColor = Color.Red;
                    }
                }

                if (ChkFlag)
                {
                    showInfo("读取地址[" + addr.ToString("D2") + "]基本信息OK");
                }
                else
                {
                    showInfo("读取地址[" + addr.ToString("D2") + "]基本信息FAIL",true);
                }
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnRead.Enabled = true;
            }
        }
        private void btnOnOff_Click(object sender, EventArgs e)
        {
            try
            {
                btnOnOff.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                int wOnOff = 0;

                if (btnOnOff.Text == "Set ON")
                {
                    wOnOff = 1;
                }

                if (comMon.SetONOFF(addr, wOnOff, out er))
                {
                    showInfo("成功设置地址[" + addr.ToString("D2") + "]ONOFF=" + wOnOff.ToString());

                    if (wOnOff == 1)
                        btnOnOff.Text = "Set OFF";
                    else
                        btnOnOff.Text = "Set ON";
                }
                else
                {
                    showInfo("设置地址[" + addr.ToString("D2") + "]ONOFF=" + wOnOff.ToString()+ "失败:" + er, true);
                }
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnOnOff.Enabled = true;
            }
        }
        private void btnSetMon_Click(object sender, EventArgs e)
        {
            try
            {
                btnSetMon.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                List<CRS485_I2C.EMonitor> Monitor = new List<CRS485_I2C.EMonitor>();

                for (int i = 0; i < chkMon.Length; i++)
                {
                    if (chkMon[i].Checked)
                    {
                        CRS485_I2C.EMonitor mon = (CRS485_I2C.EMonitor)Enum.Parse(typeof(CRS485_I2C.EMonitor), chkMon[i].Text);

                        Monitor.Add(mon); 
                    }
                }

                if (comMon.SetMonitorPara(addr, out er,Monitor.ToArray()))
                {
                    showInfo("成功设置地址[" + addr.ToString("D2") + "]监控参数OK");
                }
                else
                {
                    showInfo("设置地址[" + addr.ToString("D2") + "]监控参数失败:" + er, true);
                }
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnSetMon.Enabled = true;
            }
        }
        private void btnGetMon_Click(object sender, EventArgs e)
        {
            try
            {
                btnGetMon.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);


                for (int i = 0; i < labMon.Length; i++)
                {
                    labMon[i].Text = "---";
                }

                string er = string.Empty;

                int[] rVal = null;


                if (!comMon.ReadMonitorValue(addr,out rVal, out er))
                {
                    showInfo("读取地址[" + addr.ToString("D2") + "]监控参数失败:" + er, true);
                    return;
                }

                for (int i = 0; i < rVal.Length; i++)
                {
                    if (i < labMon.Length && chkMon[i].Checked)
                    {
                        labMon[i].Text = rVal[i].ToString();
                    }
                }

                showInfo("读取地址[" + addr.ToString("D2") + "]监控参数OK");
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnGetMon.Enabled = true;
            }
        }
        private void btnI2CAddr_Click(object sender, EventArgs e)
        {
            try
            {
                btnI2CAddr.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                int i2cAddr = System.Convert.ToInt16(txtI2CAddr.Text, 16);

                string er = string.Empty;

                if (comMon.SetI2CAddr(addr,i2cAddr, out er))
                {
                    showInfo("成功设置I2C地址[" + addr.ToString("D2") + "]OK");
                }
                else
                {
                    showInfo("设置I2C地址[" + addr.ToString("D2") + "]失败:" + er, true);
                }
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnI2CAddr.Enabled = true;
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

                    string rVer = string.Empty;

                    string rName = string.Empty;

                    if (!comMon.ReadVersion(i, out rVer, out er))
                    {
                        System.Threading.Thread.Sleep(50);

                        if (!comMon.ReadVersion(i, out rVer, out er))
                        {
                            result = false;
                        }
                    }

                    System.Threading.Thread.Sleep(50);

                    //if (!comMon.ReadName(i, out rName, out er))
                    //{
                    //    System.Threading.Thread.Sleep(50);

                    //    if (!comMon.ReadName(i, out rName, out er))
                    //    {
                    //        result = false;
                    //    }
                    //}

                    ShowID(i, rVer, rName, result);

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
        private void ShowID(int Addr, string rVer, string rSn, bool result)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<int, string, string, bool>(ShowID), Addr, rVer, rSn, result);
            else
            {
                if (result)
                {
                    DevView.Rows.Add(Addr, "PASS", rVer, rSn);
                    DevView.Rows[_rowNum].Cells[1].Style.BackColor = Color.LimeGreen;
                }
                else
                {
                    DevView.Rows.Add(Addr, "FAIL", rVer, rSn);
                    DevView.Rows[_rowNum].Cells[1].Style.BackColor = Color.Red;
                }
                DevView.CurrentCell = DevView.Rows[DevView.Rows.Count - 1].Cells[0];
            }
        }
        #endregion



    }
}
