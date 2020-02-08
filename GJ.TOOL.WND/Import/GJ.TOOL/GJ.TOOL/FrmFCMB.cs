using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GJ.PLUGINS;
using GJ.DEV.FCMB;
using GJ.COM;
namespace GJ.TOOL
{
    public partial class FrmFCMB : Form,IChildMsg
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
        public FrmFCMB()
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
            for (int i = 0; i <CHILD_MAX; i++)
            {
                Label lab1 = new Label();

                lab1.Dock = DockStyle.Fill;

                lab1.Margin = new Padding(0);

                lab1.Text = (i + 1).ToString("D2"); 

                lab1.TextAlign = ContentAlignment.MiddleCenter;

                labAddrs.Add(lab1);

                Label lab2 = new Label();

                lab2.Dock = DockStyle.Fill;

                lab2.BorderStyle = BorderStyle.Fixed3D; 

                lab2.Margin = new Padding(1);

                lab2.BackColor = Color.White;

                lab2.Text = "---";

                lab2.TextAlign = ContentAlignment.MiddleCenter;

                labVers.Add(lab2);

                Label lab3 = new Label();

                lab3.Dock = DockStyle.Fill;

                lab3.Margin = new Padding(1);

                lab3.BackColor = Color.White;

                lab3.BorderStyle = BorderStyle.Fixed3D; 

                lab3.Text = "---";

                lab3.TextAlign = ContentAlignment.MiddleCenter;

                labVolts.Add(lab3);

                int row = 0;

                int col = 0;

                if (i < CHILD_MAX / 2)
                {
                    row = i + 1;

                    col = 0;
                }
                else
                {
                    row = i - (CHILD_MAX / 2) + 1;

                    col = 3;
                }

                panel3.Controls.Add(labAddrs[i], col, row);

                panel3.Controls.Add(labVers[i], col + 1, row);

                panel3.Controls.Add(labVolts[i], col+ 2, row);
            }

            labIO.Add(labX1);
            labIO.Add(labX2);
            labIO.Add(labX3);
            labIO.Add(labX4);
            labIO.Add(labX5);
            labIO.Add(labX6);
            labIO.Add(labX7);
            labIO.Add(labX8);
            labIO.Add(labX9);
            labIO.Add(labX10);
            labIO.Add(labX11);
            labIO.Add(labX12);
            labIO.Add(labX13);
            labIO.Add(labX14);
            labIO.Add(labX15);

            labXI = new List<Label>() {
                                        labXI1, labXI2, labXI3, labXI4, labXI5, labXI6, labXI7, labXI8,
                                        labXI9,labXI10,labXI11,labXI12,labXI13,labXI14,labXI15,labXI16
                                       };


            labYI = new List<Label>() {
                                        labYI0, labYI1, labYI2, labYI3, labYI4, labYI5, labYI6, labYI7
                                       };
            labY = new List<Label>()
                                    {
                                      labY0,labY1,labY2,labY3,labY4,labY5,labY6,labY7
                                    };
            btnY = new List<Button>()
                                    {
                                      btnY0,btnY1,btnY2,btnY3,btnY4,btnY5,btnY6,btnY7
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
        /// <summary>
        /// 快充板
        /// </summary>
        private CFMBCom comMon = null;
        /// <summary>
        /// 小板数量
        /// </summary>
        private const int CHILD_MAX = 40;
        /// <summary>
        /// 采集电压模式
        /// </summary>
        private EVMODE _voltMode = EVMODE.VOLT_40; 
        #endregion

        #region 面板控件
        private List<Label> labAddrs = new List<Label>();
        private List<Label> labVers = new List<Label>();
        private List<Label> labVolts = new List<Label>();
        private List<Label> labXI = new List<Label>();
        private List<Label> labIO = new List<Label>();
        private List<TextBox> txtTimer = new List<TextBox>();
        private List<Label> labYI = new List<Label>();   
        private List<Label> labY = new List<Label>();
        private List<Button> btnY = new List<Button>();
        #endregion

        #region 面板回调函数
        private void FrmFCMB_Load(object sender, EventArgs e)
        {
            cmbCom.Items.Clear();
            string[] com = System.IO.Ports.SerialPort.GetPortNames();
            for (int i = 0; i < com.Length; i++)
                cmbCom.Items.Add(com[i]);
            if (com.Length > 0)
                cmbCom.Text = com[0];

            cmbVoltMode.Items.Clear();
            cmbVoltMode.Items.Add("40 "+ CLanguage.Lan("通道"));
            cmbVoltMode.Items.Add("32 "+ CLanguage.Lan("通道"));
            cmbVoltMode.SelectedIndex = 1;

            cmbXType.Items.Clear();
            cmbXType.Items.Add(CLanguage.Lan("类型") + "1");
            cmbXType.Items.Add(CLanguage.Lan("类型") + "2");
            cmbXType.SelectedIndex = 0;

            cmbMode.Items.Clear();
            cmbMode.Items.Add(CLanguage.Lan("普通模式"));
            cmbMode.Items.Add(CLanguage.Lan("苹果专用"));
            cmbMode.SelectedIndex = 0;

            string[] qcms = Enum.GetNames(typeof(EQCM));
            cmbQCM.Items.Clear();
            for (int i = 0; i < qcms.Length; i++)
                cmbQCM.Items.Add(qcms[i]);            
            cmbQCM.SelectedIndex = 0;

            for (int i = 0; i < btnY.Count; i++)
                btnY[i].Click += new EventHandler(btnY_Click);            
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            string er = string.Empty;

            if (cmbCom.Text == "")
            {
                labStatus.Text = CLanguage.Lan("请输入串口编号");
                labStatus.ForeColor = Color.Red;
                return;
            }

            if (btnOpen.Text == CLanguage.Lan("打开"))
            {
                comMon = new CFMBCom(EType.FMB_V1);

                if (!comMon.Open(cmbCom.Text, out er, txtBaud.Text))
                {
                    comMon = null;
                    labStatus.Text = CLanguage.Lan("打开串口失败") + ":" + er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                btnOpen.Text = CLanguage.Lan("关闭");
                labStatus.Text =  CLanguage.Lan("成功打开串口");
                labStatus.ForeColor = Color.Blue;
                cmbCom.Enabled = false;
            }
            else
            {
                if (comMon != null)
                {
                    comMon.Close();
                    comMon = null;
                }
                btnOpen.Text =  CLanguage.Lan("打开");
                labStatus.Text =  CLanguage.Lan("关闭串口");
                labStatus.ForeColor = Color.Black;
                cmbCom.Enabled = true;
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

                if (comMon.SetAddr(addr, out er))
                    showInfo(CLanguage.Lan("成功设置当前地址") + "[" + addr.ToString("D2") + "]");
                else
                    showInfo(CLanguage.Lan("设置当前地址失败") + "[" + addr.ToString("D2") + "]:" + er, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnSetAddr.Enabled = true;
            }
        }
        private void btnChildAddr_Click(object sender, EventArgs e)
        {
            try
            {
                btnChildAddr.Enabled = false;

                if (!checkSystem())
                    return;

                int childAddr = System.Convert.ToInt16(txtChildAddr.Text);  

                string er = string.Empty;

                if (comMon.SetChildAddr(childAddr,out er))
                    showInfo(CLanguage.Lan("成功设置当前地址") + "[" + childAddr.ToString("D2") + "]");
                else
                    showInfo(CLanguage.Lan("设置当前地址失败") + "[" + childAddr.ToString("D2") + "]:" + er, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnChildAddr.Enabled = true;
            }
        }
        private void btnInfo_Click(object sender, EventArgs e)
        {
            try
            {
                btnInfo.Enabled=false;

                labName.Text=string.Empty;

                labVer.Text =string.Empty;

                for (int i = 0; i < labVers.Count; i++)                
                    labVers[i].Text = "";

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                string name=string.Empty;

                if (!comMon.ReadName(addr, out name, out er))
                {
                    showInfo(CLanguage.Lan("读取设备名称地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                labName.Text = name;

                //string sn = string.Empty;

                //if (!comMon.ReadSn(addr, out sn, out er))
                //{
                //    showInfo("读取地址[" + addr.ToString("D2") + "]设备序号错误:" + er, true);
                //    return;
                //}

                string ver = string.Empty;

                if (!comMon.ReadVersion(addr, out ver, out er))
                {
                    showInfo(CLanguage.Lan("读取设备版本地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                labVer.Text = ver;

                List<int> childVers = null;

                int childMax = 0;

                if (cmbVoltMode.SelectedIndex == 1)
                {
                    childMax = 32;
                }
                else
                {
                    childMax = 40;
                }

                if (!comMon.ReadChildVersion(addr, childMax, out childVers, out er))
                {
                    showInfo(CLanguage.Lan("读取小板版本地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                for (int i = 0; i < childMax; i++)
                {
                    if (childVers[i] == 0)
                    {
                        labVers[i].Text = CLanguage.Lan("异常");
                        labVers[i].ForeColor = Color.Red;
                    }
                    else
                    {
                        labVers[i].Text = ((double)childVers[i] / 10).ToString("0.0");
                        labVers[i].ForeColor = Color.Blue;
                    }
                }

                showInfo(CLanguage.Lan("读取基本信息地址") + "[" + addr.ToString("D2") + "]OK");
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
              
              btnInfo.Enabled=true;
            }
        }
        private void btnReadV_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadV.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                List<double> volts = null;

                string name = string.Empty;

                if (!comMon.ReadVolt(addr, out volts, out er, chkSync.Checked, _voltMode))
                {
                    showInfo(CLanguage.Lan("读取电压地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                for (int i = 0; i < volts.Count; i++)
                {
                    labVolts[i].Text = volts[i].ToString();
                    labVolts[i].ForeColor = Color.Black;
                }

                showInfo(CLanguage.Lan("读取电压地址") + "[" + addr.ToString("D2") + "]OK");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnReadV.Enabled = true;
            }
        }
        private void btnDD_Click(object sender, EventArgs e)
        {
            try
            {
                btnDD.Enabled = false;

                if (!checkSystem())
                    return;

                for (int i = 0; i < 32; i++)
                    labVolts[i].Text = "";

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                string value = string.Empty;

                for (int i = 0; i < 32; i++)
                {
                    System.Threading.Thread.Sleep(10);

                    if (!comMon.ReadDGND(addr, i + 1, out value, out er))
                    {
                        System.Threading.Thread.Sleep(50);

                        if (!comMon.ReadDGND(addr, i + 1, out value, out er))
                        {
                            labVolts[i].Text = CLanguage.Lan("异常");
                            labVolts[i].ForeColor =Color.Red;
                        }
                    }

                    labVolts[i].Text = value;

                    if (value == CLanguage.Lan("正常"))
                        labVolts[i].ForeColor = Color.Blue;
                    else
                        labVolts[i].ForeColor = Color.Red;

                    Application.DoEvents();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnDD.Enabled = true;
            }
        }
        private void btnWriteQCM_Click(object sender, EventArgs e)
        {
            try
            {
                btnWriteQCM.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                EQCM qcm = (EQCM)Enum.Parse(typeof(EQCM), cmbQCM.Text); 

                double qcv = System.Convert.ToDouble(txtQCV.Text);

                double qci = System.Convert.ToDouble(txtQCI.Text);

                if (!comMon.SetQCM(addr, qcm,qcv, qci, out er))
                {
                    showInfo(CLanguage.Lan("设置快充模式地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                showInfo(CLanguage.Lan("设置快充模式地址") + "[" + addr.ToString("D2") + "]OK");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnWriteQCM.Enabled = true;
            }
        }
        private void btnReadQCM_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadQCM.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                EQCM qcm = EQCM.Normal;

                double qcv = 0;

                double qci = 0;

                if (!comMon.ReadQCM(addr, out qcm,out qcv,out qci, out er))
                {
                    showInfo(CLanguage.Lan("读取快充模式地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }
                cmbQCM.Text = qcm.ToString();

                txtQCV.Text = qcv.ToString();   

                txtQCI.Text = qci.ToString();

                showInfo(CLanguage.Lan("读取快充模式地址") + "[" + addr.ToString("D2") + "]OK");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnReadQCM.Enabled = true;
            }
        }
        private void btnReadIO_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadIO.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                List<int> io = null;

                if (!comMon.ReadIO (addr, out io, out er))
                {
                    for (int i = 0; i < labIO.Count; i++)
                    {
                        labIO[i].ImageKey = "F";
                    }
                    showInfo(CLanguage.Lan("读取IO信号地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                for (int i = 0; i < labIO.Count; i++)
                {
                    if (io[i] == 1)
                        labIO[i].ImageKey = "H";
                    else
                        labIO[i].ImageKey = "L";
                }

                if (cmbXType.SelectedIndex == 1)
                {
                    for (int i = 4; i < 10; i++)
                    {
                        if (io[i] == 1)
                        {
                            labY[i - 4].ImageKey = "H";
                            btnY[i - 4].Text = "OFF";
                        }
                        else
                        {
                            labY[i - 4].ImageKey = "L";
                            btnY[i - 4].Text = "ON";
                        }
                    }
                }

                System.Threading.Thread.Sleep(10); 

                double acv=0;

                if (!comMon.ReadAC(addr, out acv, out er))
                {
                    showInfo(CLanguage.Lan("读取AC电压值地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                labACV.Text = acv.ToString();

                showInfo(CLanguage.Lan("读取IO信号地址") + "[" + addr.ToString("D2") + "]OK");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnReadIO.Enabled = true;
            }
        }
        private void btnAlarm_Click(object sender, EventArgs e)
        {
            try
            {
                btnAlarm.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                int wOnOff = 0;

                if (btnAlarm.Text == CLanguage.Lan("报警") + "ON")
                    wOnOff = 1;

                if (comMon.SetIO(addr, EFMB_wIO.错误信号灯, wOnOff, out er))
                {
                    if (btnAlarm.Text == CLanguage.Lan("报警") + "ON")
                        btnAlarm.Text = CLanguage.Lan("报警") + "OFF";
                    else
                        btnAlarm.Text = CLanguage.Lan("报警") + "ON";

                    showInfo(CLanguage.Lan("设置IO信号当前地址") + "[" + addr.ToString("D2") + "]OK");
                }
                else
                {
                    showInfo(CLanguage.Lan("设置IO信号当前地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误")+":" + er, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnAlarm.Enabled = true;
            }
        }
        private void btnRelay_Click(object sender, EventArgs e)
        {
            try
            {
                btnRelay.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                int wOnOff = 0;

                if (btnRelay.Text == CLanguage.Lan("继电器") +"ON")
                    wOnOff = 1;

                if (comMon.SetIO(addr, EFMB_wIO.继电器信号, wOnOff, out er))
                {
                    if (btnRelay.Text == CLanguage.Lan("继电器") + "ON")
                        btnRelay.Text = CLanguage.Lan("继电器") + "OFF";
                    else
                        btnRelay.Text = CLanguage.Lan("继电器") + "ON";

                    showInfo(CLanguage.Lan("设置IO信号当前地址") + "[" + addr.ToString("D2") + "]OK");
                }
                else
                {
                    showInfo(CLanguage.Lan("设置IO信号当前地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnRelay.Enabled = true;
            }
        }
        private void btnAir_Click(object sender, EventArgs e)
        {
            try
            {
                btnAir.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                if (btnAir.Text == CLanguage.Lan("气缸顶升"))
                {
                    if (!comMon.SetIO(addr, EFMB_wIO.气缸控制2, 0, out er))
                    {
                        showInfo(CLanguage.Lan("气缸控制2信号错误:") + er, true);
                        return;
                    }

                    System.Threading.Thread.Sleep(50);

                    if (!comMon.SetIO(addr, EFMB_wIO.气缸控制1, 1, out er))
                    {
                        showInfo(CLanguage.Lan("气缸控制1信号错误:") + er, true);
                        return;
                    }

                    btnAir.Text = CLanguage.Lan("气缸下降");
                }
                else
                {
                    if (!comMon.SetIO(addr, EFMB_wIO.气缸控制1, 0, out er))
                    {
                        showInfo(CLanguage.Lan("气缸控制1信号错误:") + er, true);
                        return;
                    }                   

                    System.Threading.Thread.Sleep(50);

                    if (!comMon.SetIO(addr, EFMB_wIO.气缸控制2, 1, out er))
                    {
                        showInfo(CLanguage.Lan("气缸控制2信号错误:") + er, true);
                        return;
                    }

                    btnAir.Text = CLanguage.Lan("气缸顶升");
                }

                showInfo(CLanguage.Lan("设置IO信号当前地址") + "[" + addr.ToString("D2") + "]OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnAir.Enabled = true;
            }
        }
        private void btnAir1_Click(object sender, EventArgs e)
        {
            try
            {
                btnAir1.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                int wOnOff = 0;

                if (btnAir1.Text == CLanguage.Lan("气缸") + "Y1ON")
                    wOnOff = 1;

                if (comMon.SetIO(addr, EFMB_wIO.气缸控制1, wOnOff, out er))
                {
                    if (btnAir1.Text == CLanguage.Lan("气缸") + "Y1ON")
                        btnAir1.Text = CLanguage.Lan("气缸") + "Y1OFF";
                    else
                        btnAir1.Text = CLanguage.Lan("气缸") + "Y1ON";

                    showInfo(CLanguage.Lan("设置IO信号当前地址") + "[" + addr.ToString("D2") + "]OK");
                }
                else
                {
                    showInfo(CLanguage.Lan("设置IO信号当前地址")+ "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") +":" + er, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnAir1.Enabled = true;
            }
        }
        private void btnAir2_Click(object sender, EventArgs e)
        {
            try
            {
                btnAir2.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                int wOnOff = 0;

                if (btnAir2.Text == CLanguage.Lan("气缸") + "Y2ON")
                    wOnOff = 1;

                if (comMon.SetIO(addr, EFMB_wIO.气缸控制2, wOnOff, out er))
                {
                    if (btnAir2.Text == CLanguage.Lan("气缸") + "Y2ON")
                        btnAir2.Text = CLanguage.Lan("气缸") + "Y2OFF";
                    else
                        btnAir2.Text = CLanguage.Lan("气缸") + "Y2ON";

                    showInfo(CLanguage.Lan("设置IO信号当前地址") + "[" + addr.ToString("D2") + "]OK");
                }
                else
                {
                    showInfo(CLanguage.Lan("设置IO信号当前地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnAir2.Enabled = true;
            }
        }
        private void btnAC_Click(object sender, EventArgs e)
        {
            try
            {
                btnAC.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                int wOnOff = 0;

                bool synC = chkSync.Checked;

                bool B400 = (cmbMode.SelectedIndex == 0 ? false : true);

                if (btnAC.Text == "AC ON")
                    wOnOff = 1;

                if (comMon.SetACON(addr, wOnOff, out er, synC, B400))
                {
                    if (btnAC.Text == "AC ON")
                    {
                        btnAC.Text = "AC OFF";
                    }
                    else
                    {
                        btnAC.Text = "AC ON";
                    }
                    if (synC)
                    {
                        showInfo(CLanguage.Lan("设置同步AC模式当前地址") + "[" + addr.ToString("D2") + "]【" + btnAC.Text + "】OK");
                    }
                    else
                    {
                        showInfo(CLanguage.Lan("设置异步AC模式当前地址") + "[" + addr.ToString("D2") + "]【" + btnAC.Text + "】OK");
                    }
                }
                else
                {
                    if (synC)
                    {
                        showInfo(CLanguage.Lan("设置同步AC模式当前地址") + "[" + addr.ToString("D2") + "]【" + btnAC.Text + "】"+ CLanguage.Lan("错误") + ":" + er, true);
                    }
                    else
                    {
                        showInfo(CLanguage.Lan("设置异步AC模式当前地址") + "[" + addr.ToString("D2") + "]【" + btnAC.Text + "】"+ CLanguage.Lan("错误") + ":" + er, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnAC.Enabled = true;
            }
        }
        private void btnWReg_Click(object sender, EventArgs e)
        {
            try
            {
                btnWReg.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt32(txtAddr.Text);

                int regAddr = System.Convert.ToInt32(txtRegAddr.Text, 16);

                int regVal = System.Convert.ToInt32(txtRegVal.Text); 

                string er = string.Empty;

                if (!comMon.Write(addr, ERegType.D, regAddr, regVal, out er))
                {
                    showInfo(CLanguage.Lan("写入寄存器当前地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                showInfo(CLanguage.Lan("写入寄存器当前地址") + "[" + addr.ToString("D2") + "]OK");  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnWReg.Enabled = true;
            }
        }
        private void btnRReg_Click(object sender, EventArgs e)
        {
            try
            {
                btnWReg.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt32(txtAddr.Text);

                int regAddr = System.Convert.ToInt32(txtRegAddr.Text, 16);

                int regVal = 0;

                string er = string.Empty;

                if (!comMon.Read(addr, ERegType.D, regAddr, out regVal, out er))
                {
                    showInfo(CLanguage.Lan("读取寄存器当前地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                txtRegVal.Text = regVal.ToString();

                showInfo(CLanguage.Lan("读取寄存器当前地址") + "[" + addr.ToString("D2") + "]OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnWReg.Enabled = true;
            }
        }
        private void cmbVoltMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVoltMode.SelectedIndex == 1)
                _voltMode = EVMODE.VOLT_32;
            else
                _voltMode = EVMODE.VOLT_40; 
        }
        private void cmbXType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] X1 = new string[]
                                     {
                                      "X0" + CLanguage.Lan("[接触器同步信号]:"),
                                      "X1" + CLanguage.Lan("[治具到位信号1]:"),
                                      "X2" + CLanguage.Lan("[治具到位信号2]:"),
                                      "X3" + CLanguage.Lan("[治具到位信号3]:"),
                                      "X4" + CLanguage.Lan("[治具到位信号4]:"),
                                      "X5" + CLanguage.Lan("[自动/手动切换信号]:"),
                                      "X6" + CLanguage.Lan("[手动顶升信号]:"),
                                      "X7" + CLanguage.Lan("[手动下降信号]:"),
                                      "X8" + CLanguage.Lan("[S1 状态]:"),
                                      "X9" + CLanguage.Lan("[AC电压信号]:"),
                                      "X10" + CLanguage.Lan("[继电器粘连警告]:"),
                                      "X11" + CLanguage.Lan("[检测到AC]:"),
                                      "X12" + CLanguage.Lan("[Run 信号灯]:"),
                                      "X13" + CLanguage.Lan("[Error 信号灯]:"),
                                      "X14" + CLanguage.Lan("[气缸控制1]:"),
                                      "X15" + CLanguage.Lan("[气缸控制2]:")                                  
                                     };
            string[] X2 = new string[]
                                     {
                                      "X1" + CLanguage.Lan("[治具到位信号1]:"),
                                      "X2" + CLanguage.Lan("[治具到位信号2]:"),
                                      "X3" + CLanguage.Lan("[治具1手动控制信号]:"),
                                      "X4" + CLanguage.Lan("[治具2手动控制信号]:"),
                                      "X5" + CLanguage.Lan("[治具1状态灯控制信号]:"),
                                      "X6" + CLanguage.Lan("[治具2状态灯控制信号]:"),
                                      "X7" + CLanguage.Lan("[治具1到位指示灯信号]:"),
                                      "X8" + CLanguage.Lan("[治具2到位指示灯信号]:"),
                                      "X9" + CLanguage.Lan("[治具1AC控制信号]:"),
                                      "X10" + CLanguage.Lan("[治具2AC控制信号]:"),
                                      "X11" + CLanguage.Lan("[未定义]:"),
                                      "X12" + CLanguage.Lan("[未定义]:"),
                                      "X13" + CLanguage.Lan("[未定义]:"),
                                      "X14" + CLanguage.Lan("[未定义]:"),
                                      "X15" + CLanguage.Lan("[未定义]:"),
                                      "X16" + CLanguage.Lan("[未定义]:")                                
                                     };
            if (cmbXType.SelectedIndex == 0)
            {
                for (int i = 0; i < labXI.Count; i++)
                    labXI[i].Text = X1[i];
            }
            else
            {
                for (int i = 0; i < labXI.Count; i++)
                    labXI[i].Text = X2[i];
            }
        }
        private void btnY_Click(object sender, EventArgs e)
        {
             Button btn = (Button)sender;

            try
            {

                int idNo = System.Convert.ToUInt16(btn.Name.Substring(4, btn.Name.Length - 4));

                btn.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                int wOnOff = (btn.Text=="ON"?1:0);

                System.Threading.Thread.Sleep(50);

                if (!comMon.SetIO(addr, (EFMB_Y)idNo, wOnOff, out er))
                {
                    System.Threading.Thread.Sleep(50);

                    if (!comMon.SetIO(addr, (EFMB_Y)idNo, wOnOff, out er))
                    {
                        showInfo(CLanguage.Lan("设置信号当前地址") + "[" + addr.ToString("D2") + "]Y" + idNo.ToString() + 
                               "->" + wOnOff.ToString() + CLanguage.Lan("错误") + ":" + er, true);
                        return;
                    }
                }

                showInfo(CLanguage.Lan("设置信号当前地址") + "[" + addr.ToString("D2") + "]Y" + idNo.ToString() + "->" + wOnOff.ToString() + " OK");

                System.Threading.Thread.Sleep(50);

                //读取Y点信号
                List<int> io = null;

                if (!comMon.ReadIO(addr, out io, out er))
                {
                    System.Threading.Thread.Sleep(50);

                    if (!comMon.ReadIO(addr, out io, out er))
                    {
                        showInfo(CLanguage.Lan("读取IO信号地址") + "[" + addr.ToString("D2") + "]" + CLanguage.Lan("错误")+":" + er, true);
                        return;
                    }
                }

                for (int i = 0; i < labIO.Count; i++)
                {
                    if (io[i] == 1)
                        labIO[i].ImageKey = "H";
                    else
                        labIO[i].ImageKey = "L";
                }

                for (int i = 4; i < 10; i++)
                {
                    if (io[i] == 1)
                    {
                        labY[i-4].ImageKey = "H";
                        btnY[i-4].Text = "OFF";
                    }
                    else
                    {
                        labY[i-4].ImageKey = "L";
                        btnY[i-4].Text = "ON";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btn.Enabled = true;
            }
        }
        private void btnMon_Click(object sender, EventArgs e)
        {
            try
            {
                btnMon.Enabled = false;

                if (!checkSystem())
                    return;

                if (btnMon.Text == "监控")
                {
                    Count = 0;
                    timer1.Interval = 1000;
                    timer1.Enabled = true;
                    btnMon.Text = "停止";
                }
                else
                {
                    timer1.Enabled = false;
                    btnMon.Text = "监控";
                }

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                List<double> volts = null;

                string name = string.Empty;

                if (!comMon.ReadVolt(addr, out volts, out er, chkSync.Checked, _voltMode))
                {
                    showInfo(CLanguage.Lan("读取电压地址") + "[" + addr.ToString("D2") + "]" + CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                for (int i = 0; i < volts.Count; i++)
                {
                    labVolts[i].Text = volts[i].ToString();
                    labVolts[i].ForeColor = Color.Black;
                }

                showInfo(CLanguage.Lan("读取电压地址") + "[" + addr.ToString("D2") + "]OK");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnMon.Enabled = true;
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
                labStatus.Text =  CLanguage.Lan("请确定已打开串口?");
                labStatus.ForeColor = Color.Red;
                return false;
            }
            if (txtAddr.Text == "")
            {
                labStatus.Text =  CLanguage.Lan("请输入地址");
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
            catch (Exception)
            {
                throw;
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

                    string rSn = string.Empty;

                    if (!comMon.ReadVersion(i, out rVer, out er))
                    {
                        System.Threading.Thread.Sleep(50);

                        if (!comMon.ReadVersion(i, out rVer, out er))
                        {
                            result = false;
                        }
                    }

                    System.Threading.Thread.Sleep(50);

                    if (!comMon.ReadName(i, out rSn, out er))
                    {
                        System.Threading.Thread.Sleep(50);

                        if (!comMon.ReadName(i, out rSn, out er))
                        {
                            result = false;
                        }
                    }

                    ShowID(i, rVer, rSn, result);

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

        #region 监控电压
        private int Count = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            try 
	        {	        
		       int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                List<double> volts = null;

                string name = string.Empty;

                if (!comMon.ReadVolt(addr, out volts, out er, chkSync.Checked, _voltMode))
                {
                    showInfo(CLanguage.Lan("读取电压地址") + "[" + addr.ToString("D2") + "]" + CLanguage.Lan("错误") + ":" + er + "-->" + (Count++).ToString(), true);
                    return;
                }

                for (int i = 0; i < volts.Count; i++)
                {
                    labVolts[i].Text = volts[i].ToString();
                    labVolts[i].ForeColor = Color.Black;
                }

                showInfo(CLanguage.Lan("读取电压地址") + "[" + addr.ToString("D2") + "]OK->" + (Count++).ToString());
	        }
	        catch (Exception)
	        {		

	        }               
        }
        #endregion



    }
}
