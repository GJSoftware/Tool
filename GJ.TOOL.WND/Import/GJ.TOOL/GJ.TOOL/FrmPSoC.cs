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
using GJ.UI;
using GJ.PLUGINS;
using GJ.DEV.Cypress;
namespace GJ.TOOL
{
    public partial class FrmPSoC : Form, IChildMsg
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
                string er = string.Empty;

                comMon.OnConed -= new CPSoC4.EventOnConHander(OnCConArgs);  

                comMon.ClosePSoc(out er);

                comMon = null;
            }

            SaveIniFile(); 
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
        public FrmPSoC()
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
            runLog = new udcRunLog();
            runLog.mTitleEnable = false;
            runLog.mFont = new Font("宋体", 12);
            runLog.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(runLog);     
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
        /// 运行日志
        /// </summary>
        private udcRunLog runLog = null;
        /// <summary>
        /// INI文件
        /// </summary>
        private string IniFile = Application.StartupPath + "\\IniFile.ini"; 
        /// <summary>
        /// 烧录接口
        /// </summary>
        private CPSoC4 comMon = null;
        /// <summary>
        /// 初始化参数
        /// </summary>
        private CPSoC4.CPara para = new CPSoC4.CPara();
        /// <summary>
        /// 终止烧录
        /// </summary>
        private bool comAbort = false;
        #endregion

        #region 面板回调函数
        private void FrmPSoC_Load(object sender, EventArgs e)
        {
            LoadUI();
        }
        private void btnFindId_Click(object sender, EventArgs e)
        {
            try
            {
                btnFindId.Enabled = false;

                string er = string.Empty;

                cmbPSoC.Items.Clear();

                if (comMon != null)
                {
                    comMon.OnConed -= new CPSoC4.EventOnConHander(OnCConArgs);

                    comMon = null;
                }

                comMon = new CPSoC4(0, cmbICType.Text, para.DllFile);

                if (!comMon.FindPSocId(out er))
                {

                    btnFindId.BackColor = Color.Transparent;

                    comMon = null;

                    ShowStatus(er, true);

                    return;
                }

                foreach (string key in comMon.PortName.Keys)
                    cmbPSoC.Items.Add(key);

                cmbPSoC.SelectedIndex = 0;

                ShowStatus("成功初始化烧录器端口", false);

                btnFindId.BackColor = Color.Lime;

            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), true);

                comMon = null;
            }
            finally
            {
                btnFindId.Enabled = true;
            }
        }
        private void btnCon_Click(object sender, EventArgs e)
        {
            try
            {
                btnCon.Enabled = false;

                string er = string.Empty;

                if (comMon == null)
                {
                    ShowStatus("未初始化烧录器端口", true);
                    return;
                }

                if (btnCon.Text == "Open")
                {
                    if (cmbPSoC.Text == "")
                    {
                        ShowStatus("请选择烧录器ID号", true);
                        return;
                    }

                    if (!comMon.OpenPSoc(cmbPSoC.Text, out er))
                    {
                        ShowStatus(er, true);

                        return;
                    }

                    comMon.OnConed += new CPSoC4.EventOnConHander(OnCConArgs);

                    ShowStatus("成功打开烧录器ID号[" + cmbPSoC.Text + "]", false);

                    btnCon.Text = "Close";
                }
                else
                {
                    if (!comMon.ClosePSoc(out er))
                        ShowStatus(er, true);
                    else
                        ShowStatus("成功关闭烧录器", false);

                    comMon.OnConed -= new CPSoC4.EventOnConHander(OnCConArgs);

                    btnCon.Text = "Open";
                }                   
            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), true);
            }
            finally
            {
                btnCon.Enabled = true;
            }
        }
        private void btnAbort_Click(object sender, EventArgs e)
        {
            try
            {
                btnAbort.Enabled = false;

                Process[] pro = Process.GetProcesses();

                for (int i = 0; i < pro.Length; i++)
                {
                    if (pro[i].ProcessName=="PSoCProgrammerCOM")
                    {
                        if (MessageBox.Show("确定要终止烧录？", "终止烧录", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            pro[i].Kill();

                            comAbort = true;
                        }                        
                    }
                }              
            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), true);
            }
            finally
            {
                btnAbort.Enabled = true;
            }
        }
        private void btnInitial_Click(object sender, EventArgs e)
        {
            try
            {
                btnInitial.Enabled = false;

                string er = string.Empty;

                if (comMon == null)
                {
                    ShowStatus("未初始化烧录器端口", true);
                    return;
                }

                if (!comMon.InitialPSoc(para, out er))
                {
                    ShowStatus(er, true);
                    return;
                }

                ShowStatus("初始化烧录器参数OK", false);
            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), true);
            }
            finally
            {
                btnInitial.Enabled = true;
            }
        }
        private void btnProgram_Click(object sender, EventArgs e)
        {
            try
            {
                btnProgram.Enabled = false;

                string er = string.Empty;

                if (comMon == null)
                {
                    ShowStatus("未初始化烧录器端口", true);
                    return;
                }

                OnProgramHandler OnProgramEvent = new OnProgramHandler(OnProgram);

               IAsyncResult result= OnProgramEvent.BeginInvoke(null, null);

               while (!result.IsCompleted)
               {
                   Application.DoEvents();
               }
            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), true);
            }
            finally
            {
                if (comAbort)
                {
                    ResetCom();
                }
                btnProgram.Enabled = true;
            }
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                btnLoad.Enabled = false;
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = Application.StartupPath;
                dlg.Filter = "Hex files (*.hex)|*.hex";
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                txtHexFile.Text = dlg.FileName;
                para.HexFile = dlg.FileName;
            }
            catch (Exception ex)
            {
                ShowStatus(ex.ToString(), true);
            }
            finally
            {
                btnLoad.Enabled = true;
            }
        }
        private void cmbProtocol_SelectedValueChanged(object sender, EventArgs e)
        {
            para.Protocol = (CPSoC4.EProtocol)Enum.Parse(typeof(CPSoC4.EProtocol), cmbProtocol.Text);

            SaveIniFile();
        }
        private void cmbVoltage_SelectedValueChanged(object sender, EventArgs e)
        {
            para.Voltage = System.Convert.ToDouble(cmbVoltage.Text);

            SaveIniFile();
        }
        private void cmbClock_SelectedValueChanged(object sender, EventArgs e)
        {
            int clock = cmbClock.SelectedIndex;

            switch (clock)
	        {
                case 0:
                    para.Freq=CPSoC4.EFrequencies.FREQ_24_0; 
                    break;
                case 1:
                    para.Freq=CPSoC4.EFrequencies.FREQ_16_0; 
                    break;
                case 2:
                    para.Freq=CPSoC4.EFrequencies.FREQ_12_0; 
                    break;
                case 3:
                    para.Freq=CPSoC4.EFrequencies.FREQ_08_0; 
                    break;
                case 4:
                    para.Freq=CPSoC4.EFrequencies.FREQ_06_0; 
                    break;
                case 5:
                    para.Freq=CPSoC4.EFrequencies.FREQ_03_2; 
                    break;
                case 6:
                    para.Freq=CPSoC4.EFrequencies.FREQ_03_0; 
                    break;
                case 7:
                    para.Freq=CPSoC4.EFrequencies.FREQ_01_6; 
                    break;
                case 8:
                    para.Freq=CPSoC4.EFrequencies.FREQ_01_5; 
                    break;
		        default:
                     break;
	        }

            SaveIniFile();
        }
        private void cmbPin_SelectedValueChanged(object sender, EventArgs e)
        {
            para.Pin = (CPSoC4.EPIN)cmbPin.SelectedIndex;

            SaveIniFile();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 重启端口连接
        /// </summary>
        private void ResetCom()
        {
            try
            {
                string er = string.Empty;

                if (!comMon.FindPSocId(out er))
                {
                    btnFindId.BackColor = Color.Transparent;

                    comMon.OnConed -= new CPSoC4.EventOnConHander(OnCConArgs);

                    comMon = null;

                    btnCon.Text = "Open";

                    ShowStatus(er, true);

                    return;
                }

                if (!comMon.OpenPSoc(cmbPSoC.Text, out er))
                {
                    ShowStatus(er, true);

                    btnCon.Text = "Open";

                    return;
                }

                if (!comMon.InitialPSoc(para, out er))
                {
                    ShowStatus(er, true);

                    btnCon.Text = "Open";

                    return;
                }
            }
            catch (Exception ex)
            {
                runLog.Log(ex.ToString(), udcRunLog.ELog.Err); 
            }
        }
        /// <summary>
        /// 显示状态
        /// </summary>
        /// <param name="info"></param>
        /// <param name="bAlarm"></param>
        private void ShowStatus(string info, bool bAlarm)
        {
            if(this.InvokeRequired) 
                this.Invoke(new Action<string, bool>(ShowStatus), info, bAlarm);
            else
            {
                labStatus.Text = info;
                if (bAlarm)
                    labStatus.ForeColor = Color.Red;
                else
                    labStatus.ForeColor = Color.Blue; 
            }
        }
        /// <summary>
        /// 保存INI文件
        /// </summary>
        private void SaveIniFile()
        {
            CIniFile.WriteToIni("PsoC", "Protocol", ((int)para.Protocol).ToString(), IniFile);
            CIniFile.WriteToIni("PsoC", "Voltage", para.Voltage.ToString(), IniFile);
            CIniFile.WriteToIni("PsoC", "Freq", ((int)para.Freq).ToString(), IniFile); 
            CIniFile.WriteToIni("PsoC", "Pin", ((int)para.Pin).ToString(), IniFile);
            CIniFile.WriteToIni("PsoC", "HexFile", para.HexFile, IniFile);
        }
        /// <summary>
        /// 加载INI文件
        /// </summary>
        private void LoadIniFile()
        {
            para.DllFile = CIniFile.ReadFromIni("PsoC", "DllFile", IniFile);
            para.Protocol= (CPSoC4.EProtocol)System.Convert.ToInt32(CIniFile.ReadFromIni("PsoC", "Protocol", IniFile, "8"));
            para.Voltage = System.Convert.ToDouble(CIniFile.ReadFromIni("PsoC", "Voltage", IniFile, "3.3"));
            para.Freq = (CPSoC4.EFrequencies)System.Convert.ToInt32(CIniFile.ReadFromIni("PsoC", "Freq", IniFile, "224"));
            para.Pin = (CPSoC4.EPIN)System.Convert.ToInt32(CIniFile.ReadFromIni("PsoC", "Pin", IniFile, "0"));
            para.HexFile = CIniFile.ReadFromIni("PsoC", "HexFile", IniFile, @"C:\Program Files (x86)\Cypress\Programmer\PP_COM_Wrapper.dll");
        }
        /// <summary>
        /// 加载设置
        /// </summary>
        private void LoadUI()
        {
            cmbICType.SelectedIndex = 0; 
            cmbProtocol.Text = para.Protocol.ToString();
            cmbVoltage.Text = para.Voltage.ToString();
            cmbPin.Text = para.Pin.ToString();
            txtHexFile.Text = para.HexFile;
            switch (para.Freq)
            {
                case CPSoC4.EFrequencies.FREQ_24_0:
                    cmbClock.SelectedIndex = 0;  
                    break;
                case CPSoC4.EFrequencies.FREQ_16_0:
                    cmbClock.SelectedIndex = 1; 
                    break;
                case CPSoC4.EFrequencies.FREQ_12_0:
                    cmbClock.SelectedIndex = 2; 
                    break;
                case CPSoC4.EFrequencies.FREQ_08_0:
                    cmbClock.SelectedIndex = 3; 
                    break;
                case CPSoC4.EFrequencies.FREQ_06_0:
                    cmbClock.SelectedIndex = 4; 
                    break;
                case CPSoC4.EFrequencies.FREQ_03_2:
                    cmbClock.SelectedIndex = 5; 
                    break;
                case CPSoC4.EFrequencies.FREQ_03_0:
                    cmbClock.SelectedIndex = 6; 
                    break;
                case CPSoC4.EFrequencies.FREQ_01_6:
                    cmbClock.SelectedIndex = 7; 
                    break;
                case CPSoC4.EFrequencies.FREQ_01_5:
                    cmbClock.SelectedIndex = 8; 
                    break;
                default:
                    break;
            }           
        }
        #endregion

        #region 委托烧录
        private delegate void OnProgramHandler();
        private void OnProgram()
        {
            try
            {
                string er = string.Empty;

                comAbort = false;

                ShowStatus("开始烧录芯片IC", false);

                Stopwatch watcher = new Stopwatch();

                watcher.Start();

                if (!comMon.ProgramPSoc(out er))
                {
                    if (!comAbort)
                    {
                        ShowStatus("烧录错误:" + er, true);
                    }
                    else
                    {
                        ShowStatus("强制终止烧录", true);
                    }
                    return;
                }

                watcher.Stop();

                string waitTimes = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s";

                ShowStatus("烧录芯片IC完成:" + waitTimes, false);
            }
            catch (Exception ex)
            {
                runLog.Log(ex.ToString(), udcRunLog.ELog.Err); 
            }
        }
        #endregion

        #region 消息响应
        private void OnCConArgs(object sender, CPSoC4.CConArgs e)
        {
            try
            {
                if (!e.bErr)
                {
                    runLog.Log(e.info, udcRunLog.ELog.Action);  
                }
                else
                {
                    if (comAbort)
                    {
                        runLog.Log("强制终止烧录,请检查烧录线路是否正常?", udcRunLog.ELog.NG);
                    }
                    else
                    {
                        runLog.Log(e.info, udcRunLog.ELog.NG);
                    }
                }
            }
            catch (Exception ex)
            {
                runLog.Log(ex.ToString(), udcRunLog.ELog.Err);
            }
        }
        #endregion

    }
}
