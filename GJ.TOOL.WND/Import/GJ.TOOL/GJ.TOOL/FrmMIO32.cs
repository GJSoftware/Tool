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
using GJ.DEV.V3;
using System.Diagnostics;
using System.Threading;

namespace GJ.TOOL
{
    public partial class FrmMIO32 : Form, IChildMsg
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
        public FrmMIO32()
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
            for (int i = 0; i < MaxCH; i++)
            {
                Label lab1 = new Label();
                lab1.Dock = DockStyle.Fill;
                lab1.TextAlign = ContentAlignment.MiddleCenter;
                lab1.Font = new Font("宋体", 10.5f);
                lab1.Text = "CH" + (i + 1).ToString();
                lab1.Margin = new Padding(0);
                labCH.Add(lab1);

                Label lab2 = new Label();
                lab2.Dock = DockStyle.Fill;
                lab2.TextAlign = ContentAlignment.MiddleCenter;
                lab2.BackColor = Color.White;
                lab2.Font = new Font("宋体", 10.5f);
                lab2.Text = "0.00";
                lab2.Margin = new Padding(0);
                labVolt.Add(lab2);
            }

            for (int i = 0; i < MaxCH/2; i++)
            {
                panel4.Controls.Add(labCH[i], 0, i + 1);
                panel4.Controls.Add(labVolt[i], 1, i + 1);

                panel4.Controls.Add(labCH[i + MaxCH / 2], 2, i + 1);
                panel4.Controls.Add(labVolt[i + MaxCH / 2], 3, i + 1);
            }

            string[] X = new string[] { "[治具1到位]","[治具2到位]","[主接触器信号]",
                                         "","","","","",""};

            string[] Y = new string[] { "[AC主接触器]","[AC继电器1]","[12V-PSON1]",
                                        "[12V-PSON2]","[48V-PSON]","[AC继电器2]",
                                        "",""}; 

            for (int i = 0; i < MaxX; i++)
            {
                Label lab1 = new Label();
                lab1.Dock = DockStyle.Fill;
                lab1.TextAlign = ContentAlignment.MiddleLeft;
                lab1.Font = new Font("宋体", 10.5f);
                lab1.Text = "X" + (i + 1).ToString() + X[i];
                lab1.Margin = new Padding(3);
                labXno.Add(lab1);

                Label lab2 = new Label();
                lab2.Dock = DockStyle.Fill;
                lab2.TextAlign = ContentAlignment.MiddleCenter;
                lab2.Font = new Font("宋体", 10.5f);
                lab2.ImageList = imageList1;
                lab2.ImageKey = "L";
                lab2.Text = "";
                lab2.Margin = new Padding(0);
                labX.Add(lab2);

                panel4.Controls.Add(labXno[i], 4, i + 1);
                panel4.Controls.Add(labX[i], 5, i + 1);
            }

            for (int i = 0; i < MaxY; i++)
            {
                Label lab1 = new Label();
                lab1.Dock = DockStyle.Fill;
                lab1.TextAlign = ContentAlignment.MiddleLeft;
                lab1.Font = new Font("宋体", 10.5f);
                lab1.Text = "Y" + (i + 1).ToString() + Y[i];
                lab1.Margin = new Padding(3);
                labYno.Add(lab1);

                Label lab2 = new Label();
                lab2.Dock = DockStyle.Fill;
                lab2.TextAlign = ContentAlignment.MiddleCenter;
                lab2.Font = new Font("宋体", 10.5f);
                lab2.ImageList = imageList1;
                lab2.ImageKey = "L";
                lab2.Text = "";
                lab2.Margin = new Padding(0);
                labY.Add(lab2);

                Button btn = new Button();
                btn.Dock = DockStyle.Fill;
                btn.Font = new Font("宋体", 10.5f);
                btn.Name = "Y" + i.ToString();
                btn.Text = "ON";
                btn.Margin = new Padding(0);
                btn.Click += new EventHandler(btnY_Click);
                btnY.Add(btn); 

                panel4.Controls.Add(labYno[i], 6, i + 1);
                panel4.Controls.Add(labY[i], 7, i + 1);
                panel4.Controls.Add(btnY[i], 8, i + 1);
            }
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
        private const int MaxCH = 32;
        private const int MaxX = 9;
        private const int MaxY = 8;
        private CMIO32 comMon = null;
        private Stopwatch watcher = new Stopwatch();
        #endregion

        #region 面板控件
        private List<Label> labCH = new List<Label>();
        private List<Label> labVolt = new List<Label>();
        private List<Label> labXno = new List<Label>();
        private List<Label> labX = new List<Label>();
        private List<Label> labYno = new List<Label>();
        private List<Label> labY = new List<Label>();
        private List<Button> btnY = new List<Button>();
        #endregion

        #region 面板回调函数
        private void FrmMIO32_Load(object sender, EventArgs e)
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
                    comMon = new CMIO32();

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

                string er = string.Empty;

                string version = string.Empty;

                string name = string.Empty;

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
                        labModuleVer.Text = version;

                        labModuleVer.ForeColor = Color.Blue;

                        showInfo("读取地址[" + addr.ToString("D2") + "]基本信息OK");
                    }
                }
                else
                {
                    labModuleVer.Text = version;

                    labModuleVer.ForeColor = Color.Blue;

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

                for (int i = 0; i < labVolt.Count; i++)
                {
                    labVolt[i].Text = "---";
                }

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                List<double> volt = null;
                 
                List<int> x = null;

                Thread.Sleep(50);

                if (!comMon.ReadVolt(addr, out volt,out x, out er))
                {
                     Thread.Sleep(50);

                     if (!comMon.ReadVolt(addr, out volt,out x, out er))
                     {
                         showInfo("读取地址[" + addr.ToString("D2") + "]电压与X点错误:" + er, true);
                         return;
                     }
                }

                for (int i = 0; i < volt.Count; i++)
                {
                    labVolt[i].Text = volt[i].ToString();
                }

                for (int i = 0; i < x.Count; i++)
			    {
			       if(x[i]==1)
                   {
                      labX[i].ImageKey="H";
                   }
                   else 
                   {
                      labX[i].ImageKey="L";
                   }
			    }

                List<int> Y = null;

                if (!comMon.ReadY(addr, out Y, out er))
                {
                    Thread.Sleep(50);

                    if (!comMon.ReadY(addr, out Y, out er))
                    {
                        showInfo("读取地址[" + addr.ToString("D2") + "]Y点错误:" + er, true);
                        return;
                    }
                }

                for (int i = 0; i < Y.Count; i++)
                {
                    if (Y[i] == 1)
                    {
                        labY[i].ImageKey = "H";
                    }
                    else
                    {
                        labY[i].ImageKey = "L";
                    }
                }

                showInfo("读取地址[" + addr.ToString("D2") + "]数据OK");

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
        private void btnY_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            int idNo = System.Convert.ToInt16(btn.Name.Substring(1, btn.Name.Length-1));

            try
            {
                btnY[idNo].Enabled = false;

                string er = string.Empty;

                int OnOff = 0;

                if (btnY[idNo].Text == "ON")
                {
                    OnOff = 1;
                }
                
                int addr = System.Convert.ToInt16(txtAddr.Text);

                Thread.Sleep(50);

                if (!comMon.ControlY_OnOff(addr, idNo,OnOff, out er))
                {
                    Thread.Sleep(50);

                    if (!comMon.ControlY_OnOff(addr, idNo, OnOff, out er))
                    {
                        showInfo("设置地址[" + addr.ToString("D2") + "]Y" + (idNo+1).ToString() + "=" + OnOff.ToString() + ":" + er, true);
                        return;
                    }
                }

                List<int> Y = null;

                if (!comMon.ReadY(addr, out Y, out er))
                {
                    Thread.Sleep(50);

                    if (!comMon.ReadY(addr, out Y, out er))
                    {
                        showInfo("读取地址[" + addr.ToString("D2") + "]Y" + (idNo + 1).ToString() + "=" + OnOff.ToString() + ":" + er, true);
                        return;
                    }
                }

                if (Y[idNo] == 1)
                {
                    btnY[idNo].Text = "OFF";
                    labY[idNo].ImageKey = "H";
                }
                else
                {
                    btnY[idNo].Text = "ON";
                    labY[idNo].ImageKey = "L";
                }

                showInfo("设置地址[" + addr.ToString("D2") + "]Y" + (idNo+1).ToString() + "=" + OnOff.ToString() + " OK");
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnY[idNo].Enabled = true;
            }
        }
        #endregion

        #region 监控扫描
        private int _rowNum = 0;
        private bool _scanStopFlag = false;
        private int _startAddr = 1;
        private int _endAddr = 70;
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

                    ShowID(i, rVer, rName, result);

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
        private void ShowID(int Addr, string rVer, string rSn, bool result)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<int, string, string, bool>(ShowID), Addr, rVer, rSn, result);
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
    }
}
