using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GJ.PLUGINS;
using GJ.DEV.DD;
using GJ.COM;

namespace GJ.TOOL
{
    public partial class FrmDD : Form,IChildMsg
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
        public FrmDD()
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
            for (int i = 0; i < 12; i++)
            {
                Label lab1 = new Label();

                lab1.Dock = DockStyle.Fill;

                lab1.Margin = new Padding(3);

                lab1.Text = "CH" + (i + 1).ToString("D2") + ":";

                lab1.TextAlign = ContentAlignment.MiddleCenter;

                labCH.Add(lab1);

                Label lab2 = new Label();

                lab2.Dock = DockStyle.Fill;

                lab2.BorderStyle = BorderStyle.Fixed3D;

                lab2.Margin = new Padding(2);

                lab2.BackColor = Color.Black;

                lab2.ForeColor = Color.Cyan;

                lab2.Text = "---";

                lab2.TextAlign = ContentAlignment.MiddleCenter;

                labV.Add(lab2);

                panel3.Controls.Add(labCH[i], 0, i + 1);

                panel3.Controls.Add(labV[i], 2, i + 1);
            }

            for (int i = 0; i <8; i++)
            {

                Label lab3 = new Label();

                lab3.Dock = DockStyle.Fill;

                lab3.BorderStyle = BorderStyle.Fixed3D;

                lab3.Margin = new Padding(2);

                lab3.BackColor = Color.Black;

                lab3.ForeColor = Color.Cyan;

                lab3.Text = "---";

                lab3.TextAlign = ContentAlignment.MiddleCenter;

                labA.Add(lab3);

                TextBox txt = new TextBox();

                txt.Dock = DockStyle.Fill;

                txt.Margin = new Padding(3);

                txt.Text = "0.5";

                txt.TextAlign = HorizontalAlignment.Center;

                txtLoad.Add(txt);

                panel3.Controls.Add(txtLoad[i], 1, i + 1);

                panel3.Controls.Add(labA[i], 3, i + 1);
            }

           
        }
        /// <summary>
        /// 设置双缓冲,防止界面闪烁
        /// </summary>
        private void SetDoubleBuffered()
        {
            panel1.GetType().GetProperty("DoubleBuffered",
                                           System.Reflection.BindingFlags.Instance |
                                           System.Reflection.BindingFlags.NonPublic)
                                           .SetValue(panel1, true, null);
            panel2.GetType().GetProperty("DoubleBuffered",
                                            System.Reflection.BindingFlags.Instance |
                                            System.Reflection.BindingFlags.NonPublic)
                                            .SetValue(panel2, true, null);
            panel3.GetType().GetProperty("DoubleBuffered",
                                            System.Reflection.BindingFlags.Instance |
                                            System.Reflection.BindingFlags.NonPublic)
                                            .SetValue(panel3, true, null);
        }
        #endregion

        #region 字段
        /// <summary>
        /// 快充板
        /// </summary>
        private CDDCom comMon = null;
        /// <summary>
        /// 小板数量
        /// </summary>
        private const int CHAN_MAX = 8;
        #endregion

        #region 面板控件
        private List<Label> labCH = new List<Label>();
        private List<TextBox> txtLoad = new List<TextBox>();
        private List<Label> labV = new List<Label>();
        private List<Label> labA = new List<Label>();
        #endregion

        #region 面板回调函数

        private void FrmDD_Load(object sender, EventArgs e)
        {
            cmbCom.Items.Clear();
            string[] com = System.IO.Ports.SerialPort.GetPortNames();
            for (int i = 0; i < com.Length; i++)
                cmbCom.Items.Add(com[i]);
            if (com.Length > 0)
                cmbCom.SelectedIndex = 0;
            cmbDDType.SelectedIndex = 0;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                btnOpen.Enabled = false;

                if (cmbCom.Text == "")
                {
                    labStatus.Text = "请输入串口编号";
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                string er = string.Empty;

                if (comMon == null)
                {
                    if (!Enum.IsDefined(typeof(EType), cmbDDType.Text))
                    {
                        labStatus.Text = "找不到【" + cmbDDType.Text + "】类型";
                        labStatus.ForeColor = Color.Red;
                        return;
                    }

                    EType ddType = (EType)Enum.Parse(typeof(EType), cmbDDType.Text);

                    comMon = new CDDCom(ddType, 0, cmbDDType.Text);

                    if (!comMon.Open(cmbCom.Text, out er))
                    {
                        labStatus.Text = er;
                        labStatus.ForeColor = Color.Red;
                        comMon = null;
                        return;
                    }
                    btnOpen.Text = "关闭";
                    labStatus.Text = "成功打开串口.";
                    labStatus.ForeColor = Color.Blue;
                    cmbCom.Enabled = false;
                    cmbDDType.Enabled = false;

                }
                else
                {
                    comMon.Close();
                    comMon = null;
                    btnOpen.Text = "打开";
                    labStatus.Text = "关闭串口.";
                    labStatus.ForeColor = Color.Blue;
                    cmbCom.Enabled = true;
                    cmbDDType.Enabled = true;
                }
            }
            catch (Exception)
            {

                throw;
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

                if (comMon.SetNewAddr(addr, out er))
                    showInfo("成功设置当前地址[" + addr.ToString("D2") + "]");
                else
                    showInfo("设置当前地址[" + addr.ToString("D2") + "]失败:" + er, true);
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
        private void btnSetA_Click(object sender, EventArgs e)
        {
            try
            {
                btnSetA.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                CwLoad para = new CwLoad();

                para.saveEEPROM = (chkEPROM.Checked ? 1 : 0);

                for (int i = 0; i < para.loadVal.Length; i++)
                {
                    para.loadVal[i] = System.Convert.ToDouble(txtLoad[i].Text); 
                }

                if (comMon.SetNewLoad(addr,para, out er))
                    showInfo("成功设置负载电流[" + addr.ToString("D2") + "]");
                else
                    showInfo("设置当前负载电流[" + addr.ToString("D2") + "]失败:" + er, true);

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                btnSetA.Enabled = true;
            }
        }
        private void btnReadA_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadA.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                CrLoad para = new CrLoad();

                if (!comMon.ReadLoadSet(addr,ref para, out er))
                {
                  showInfo("读取电流设置[" + addr.ToString("D2") + "]失败:" + er, true);
                  return;
                }

                for (int i = 0; i < para.loadVal.Length; i++)
                {
                    txtLoad[i].Text = para.loadVal[i].ToString();
                }

                showInfo("成功读取电流设置[" + addr.ToString("D2") + "]");
              
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                btnReadA.Enabled = true;
            }
        }

        private void btnReadData_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadData.Enabled = false;

                if (!checkSystem())
                    return;

                for (int i = 0; i < labV.Count; i++)
                {
                    labV[i].Text = "---";
                    labV[i].ForeColor = Color.Cyan;
                }

                for (int i = 0; i < labA.Count; i++)
                {
                    labA[i].Text = "---";
                    labA[i].ForeColor = Color.Cyan;
                }

                int addr = System.Convert.ToInt16(txtAddr.Text);

                double Vmin = System.Convert.ToDouble(txtVmin.Text);

                double Vmax = System.Convert.ToDouble(txtVmax.Text);

                double Imin = System.Convert.ToDouble(txtImin.Text);

                double Imax = System.Convert.ToDouble(txtImax.Text);  

                string er = string.Empty;

                CrData data = new CrData();

                if (!comMon.ReadData(addr, ref data, out er))
                {
                    showInfo("读取DD模块数据[" + addr.ToString("D2") + "]失败:" + er, true);
                    return;
                }

                labOnOff.Text = (data.OnOff == 1 ? "ON" : "OFF"); 

                for (int i = 0; i < data.Volt.Length; i++)
                {
                    labV[i].Text = data.Volt[i].ToString();

                    if (data.Volt[i] >= Vmin && data.Volt[i] <= Vmax)
                        labV[i].ForeColor = Color.Cyan;
                    else
                        labV[i].ForeColor = Color.Red;
                }

                for (int i = 0; i < data.Cur.Length; i++)
                {
                    labA[i].Text = data.Cur[i].ToString();

                    if (data.Cur[i] >= Imin && data.Cur[i] <= Imax)
                        labA[i].ForeColor = Color.Cyan;
                    else
                        labA[i].ForeColor = Color.Red;
                }

                showInfo("成功读取DD模块数据[" + addr.ToString("D2") + "]");

            }
            catch (Exception)
            {

                throw;
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
        private int _endAddr = 80;
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

                    string rData = string.Empty;

                    CrData data = new CrData();

                    if (!comMon.ReadData(i, ref data, out er))
                    {
                        result = false;
                    }
                    else
                    {
                        for (int ch = 0; ch < data.Volt.Length; ch++)
                        {
                            rData += data.Volt[ch].ToString("0.000") + "|";
                        }
                    }

                    ShowID(i, rData, result);

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
        private void ShowID(int Addr, string rData,bool result)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<int, string, bool>(ShowID), Addr, rData, result);
            else
            {
                if (result)
                {
                    DevView.Rows.Add(Addr, "PASS", rData);
                    DevView.Rows[_rowNum].Cells[1].Style.BackColor = Color.LimeGreen;
                }
                else
                {
                    DevView.Rows.Add(Addr, "FAIL", rData);
                    DevView.Rows[_rowNum].Cells[1].Style.BackColor = Color.Red;
                }
                DevView.CurrentCell = DevView.Rows[DevView.Rows.Count - 1].Cells[0];
            }
        }
        #endregion
       

    }
}
