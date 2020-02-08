using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GJ.DEV.LED;
using GJ.PLUGINS;
using GJ.COM;
using System.Diagnostics;

namespace GJ.TOOL
{
    public partial class FrmLED : Form, IChildMsg
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
        public FrmLED()
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
            for (int i = 0; i < _chanMax; i++)
            {
                Label lab1 = new Label();
                lab1.Dock = DockStyle.Fill;
                lab1.Margin = new Padding(0);
                lab1.Text = "CH" + (i + 1).ToString("D2");
                lab1.TextAlign = ContentAlignment.MiddleCenter;
                labCH.Add(lab1);

                ComboBox cmb = new ComboBox();
                cmb.Dock = DockStyle.Fill;
                cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                cmb.Items.Add("CC_Slow");
                cmb.Items.Add("CV");
                cmb.Items.Add("CP");
                cmb.Items.Add("CR");
                cmb.Items.Add("CC_Fast");
                cmb.Items.Add("LED");
                cmb.Items.Add("MTK");
                cmb.SelectedIndex = 0;
                cmbMode.Add(cmb);

                TextBox txt1 = new TextBox();
                txt1.Dock = DockStyle.Fill;
                txt1.Margin = new Padding(1);
                txt1.Text = "3";
                txt1.TextAlign = HorizontalAlignment.Center;
                txtVon.Add(txt1);

                TextBox txt2 = new TextBox();
                txt2.Dock = DockStyle.Fill;
                txt2.Margin = new Padding(1);
                txt2.Text = "0.5";
                txt2.TextAlign = HorizontalAlignment.Center;
                txtLoad.Add(txt2);

                Label lab2 = new Label();
                lab2.Dock = DockStyle.Fill;
                lab2.BorderStyle = BorderStyle.Fixed3D;
                lab2.Margin = new Padding(1);
                lab2.BackColor = Color.White;
                lab2.Text = "---";
                lab2.TextAlign = ContentAlignment.MiddleCenter;
                labLoad.Add(lab2);

                Label lab3 = new Label();
                lab3.Dock = DockStyle.Fill;
                lab3.BorderStyle = BorderStyle.Fixed3D;
                lab3.Margin = new Padding(1);
                lab3.BackColor = Color.White;
                lab3.Text = "---";
                lab3.TextAlign = ContentAlignment.MiddleCenter;
                labVolt.Add(lab3);
 
                panel3.Controls.Add(labCH[i], 0, i + 1);
                panel3.Controls.Add(cmbMode[i], 1, i + 1);
                panel3.Controls.Add(txtVon[i], 2, i + 1);
                panel3.Controls.Add(txtLoad[i], 3, i + 1);
                panel3.Controls.Add(labVolt[i], 4, i + 1);
                panel3.Controls.Add(labLoad[i], 5, i + 1);
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
        /// <summary>
        /// 快充板
        /// </summary>
        private CLEDCom comMon = null;
        /// <summary>
        /// 小板数量
        /// </summary>
        private const int _chanMax = 16;
        #endregion

        #region 面板控件
        private List<Label> labCH = new List<Label>();
        private List<ComboBox> cmbMode = new List<ComboBox>();
        private List<TextBox> txtVon = new List<TextBox>(); 
        private List<TextBox> txtLoad = new List<TextBox>();
        private List<Label> labVolt = new List<Label>();
        private List<Label> labLoad = new List<Label>();
        #endregion

        #region 面板回调函数
        private void FrmLED_Load(object sender, EventArgs e)
        {
            cmbCom.Items.Clear();
            string[] com = System.IO.Ports.SerialPort.GetPortNames();
            for (int i = 0; i < com.Length; i++)
                cmbCom.Items.Add(com[i]);
            if (com.Length > 0)
                cmbCom.Text = com[0];
            cmbType.SelectedIndex = 0;
        }
        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int chan = labCH.Count;

            EType devType = (EType)Enum.Parse(typeof(EType), cmbType.Text);

            if (devType == EType.DA_750_4)
                chan = 4;
            else if (devType == EType.DA_320_8)
                chan = 8;

            for (int i = 0; i < labCH.Count; i++)
            {
                if (i < chan)
                {
                    labCH[i].Visible = true;
                    cmbMode[i].Visible = true;
                    txtVon[i].Visible = true;
                    txtLoad[i].Visible = true;
                    labVolt[i].Visible = true;
                    labLoad[i].Visible = true;
                }
                else
                {
                    labCH[i].Visible = false;
                    cmbMode[i].Visible = false;
                    txtVon[i].Visible = false;
                    txtLoad[i].Visible = false;
                    labVolt[i].Visible = false;
                    labLoad[i].Visible = false;
                }
            }
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                btnOpen.Enabled = false;

                string er = string.Empty;

                if (cmbCom.Text == "")
                {
                    labStatus.Text = CLanguage.Lan("请输入串口编号");
                    labStatus.ForeColor = Color.Red;
                    return;
                }   

                if (comMon == null)
                {
                    if (!Enum.IsDefined(typeof(EType), cmbType.Text))
                    {
                        labStatus.Text =  CLanguage.Lan("找不到类型") + "【" + cmbType.Text + "】";
                        labStatus.ForeColor = Color.Red;
                        return;
                    }

                    EType devType = (EType)Enum.Parse(typeof(EType), cmbType.Text);

                    comMon = new CLEDCom(devType, 0, cmbType.Text);

                    if (!comMon.Open(cmbCom.Text, out er,txtBaud.Text))
                    {
                        labStatus.Text = er;
                        labStatus.ForeColor = Color.Red;
                        comMon = null;
                        return;
                    }

                    btnOpen.Text = CLanguage.Lan("关闭");
                    labStatus.Text = CLanguage.Lan("成功打开串口");
                    labStatus.ForeColor = Color.Blue;
                    cmbCom.Enabled = false;
                    cmbType.Enabled = false;
                }
                else
                {
                    comMon.Close();
                    comMon = null;
                    btnOpen.Text = CLanguage.Lan("打开");
                    labStatus.Text = CLanguage.Lan("关闭串口");
                    labStatus.ForeColor = Color.Blue;
                    cmbCom.Enabled = true;
                    cmbType.Enabled = true;
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
                    showInfo(CLanguage.Lan("成功设置当前地址") + "[" + addr.ToString("D2") + "]");
                else
                    showInfo(CLanguage.Lan("设置当前地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
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
        private void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                btnVer.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                string name = string.Empty;

                labVersion.Text = "";

                string ver = string.Empty;

                if (!comMon.ReadVersion(addr, out ver, out er))
                {
                    showInfo(CLanguage.Lan("读取设备版本地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                labVersion.Text = ver;

                showInfo(CLanguage.Lan("读取设备版本地址") + "[" + addr.ToString("D2") + "]OK");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnVer.Enabled = true;
            }
        }
        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                btnSetLoad.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                List<CLOAD> loadList = new List<CLOAD>();

                for (int i = 0; i < txtLoad.Count; i++)
                {
                    CLOAD load = new CLOAD();

                    load.Mode = (EMODE)Enum.Parse(typeof(EMODE), cmbMode[i].Text);

                    load.Von = System.Convert.ToDouble(txtVon[i].Text);

                    load.load = System.Convert.ToDouble(txtLoad[i].Text);

                    if (load.Mode != EMODE.MTK)
                        load.mark = System.Convert.ToInt16(txtDelayS.Text);
                    else
                        load.mark = cmbQCV.SelectedIndex+3;

                    loadList.Add(load); 
                }

                Stopwatch watcher = new Stopwatch();

                watcher.Start();

                if (!comMon.SetLoadValue(addr, loadList,true, out er))
                {
                    showInfo(CLanguage.Lan("设置负载电流地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                watcher.Stop();

                showInfo(CLanguage.Lan("设置负载电流地址") + "[" + addr.ToString("D2") + "]OK:" + watcher.ElapsedMilliseconds.ToString() + "ms");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnSetLoad.Enabled = true;
            }
        }
        private void btnSetCH_Click(object sender, EventArgs e)
        {
            try
            {
                btnSetCH.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                int chan = System.Convert.ToInt16(txtCH.Text);    
 
                CLOAD load = new CLOAD();

                load.Mode = (EMODE)Enum.Parse(typeof(EMODE), cmbMode[chan-1].Text);

                load.Von = System.Convert.ToDouble(txtVon[chan - 1].Text);

                load.load = System.Convert.ToDouble(txtLoad[chan - 1].Text);

                if (load.Mode != EMODE.MTK)
                    load.mark = System.Convert.ToInt16(txtDelayS.Text);
                else
                    load.mark = cmbQCV.SelectedIndex + 3;

                if (!comMon.SetLoadValue(addr, chan, load, true, out er))
                {
                    showInfo(CLanguage.Lan("设置地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("负载") +
                                           "[" + chan.ToString() + "]"+ CLanguage.Lan("电流") + CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                showInfo(CLanguage.Lan("设置地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("负载") +
                                      "[" + chan.ToString() + "]" + CLanguage.Lan("电流") + "OK");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnSetCH.Enabled = true;
            }
        }

        private void btnReadLoad_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadLoad.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                List<CLOAD> loadList = null;

                if (!comMon.ReadLoadSetting(addr, out loadList, out er))
                {
                    showInfo(CLanguage.Lan("读取负载电流地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                for (int i = 0; i < comMon.maxCH; i++)
                {
                    cmbMode[i].Text = loadList[i].Mode.ToString();
                    txtVon[i].Text = loadList[i].Von.ToString();
                    txtLoad[i].Text = loadList[i].load.ToString();
                }

                showInfo(CLanguage.Lan("读取负载电流地址") + "[" + addr.ToString("D2") + "]OK");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnReadLoad.Enabled = true;
            }
        }
        private void btnReadData_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadData.Enabled = false;

                if (!checkSystem())
                    return;

                Stopwatch watcher = new Stopwatch();

                watcher.Start();

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                CData data = new CData();

                if (!comMon.ReadLoadValue(addr, ref data, out er))
                {
                    showInfo(CLanguage.Lan("读取负载电流地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":" + er, true);
                    return;
                }

                for (int i = 0; i < comMon.maxCH; i++)
                {
                    labVolt[i].Text = data.chan[i].volt.ToString();
                    labLoad[i].Text = data.chan[i].current.ToString(); 
                }

                labLed1.ImageKey = (data.inv_status.OTP == EINVOTP.正常 ? "H" : "F");
                labLed2.ImageKey = (data.inv_status.AD == EINVAD.正常 ? "H" : "F");
                labLed3.ImageKey = (data.inv_status.ACBus == EINVACBus.正常 ? "H" : "F");
                labLed4.ImageKey = (data.inv_status.Fan == EINVFan.正常 ? "H" : "F");
                labLed5.ImageKey = (data.inv_status.Time == EINVTIME.正常 ? "H" : "F");
                labLed6.ImageKey = (data.inv_status.DCBus == EINVDCBus.正常 ? "H" : "F");

                watcher.Stop();

                showInfo(CLanguage.Lan("读取负载电流地址") + "[" + addr.ToString("D2") + "]OK:" + watcher.ElapsedMilliseconds.ToString() + "ms");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnReadData.Enabled = true;
            }
        }
        private void btnMonitor_Click(object sender, EventArgs e)
        {
            try
            {
                btnMonitor.Enabled = false;

                timer1.Interval = System.Convert.ToInt32(txtMonTime.Text);

                if (btnMonitor.Text == CLanguage.Lan("监控"))
                {
                    if (!checkSystem())
                        return;

                    timer1.Enabled = true;

                    btnMonitor.Text = CLanguage.Lan("停止");
                }
                else
                {
                    timer1.Enabled = false;

                    btnMonitor.Text = CLanguage.Lan("监控");
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnMonitor.Enabled = true;
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
                labStatus.Text = CLanguage.Lan("请确定已打开串口?");
                labStatus.ForeColor = Color.Red;
                return false;
            }
            if (txtAddr.Text == "")
            {
                labStatus.Text = CLanguage.Lan("请输入地址");
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
        private int _endAddr = 40;
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

                    CData data = new CData();

                    if (!comMon.ReadLoadValue(i, ref data, out er))
                    {
                        System.Threading.Thread.Sleep(50);

                        if (!comMon.ReadLoadValue(i, ref data, out er))
                        {
                            result = false;
                        }
                    }

                    if (result)
                    {
                        rSn += CLanguage.Lan("电网电压:") + data.inv_status.ACBus.ToString() + "|";

                        rSn += CLanguage.Lan("母线电压:") + data.inv_status.DCBus.ToString() + "|";

                        rSn += CLanguage.Lan("AD采样:") + data.inv_status.AD.ToString() + "|";

                        rSn += CLanguage.Lan("风扇:") + data.inv_status.Fan.ToString() + "|";

                        rSn += CLanguage.Lan("OTP:") + data.inv_status.OTP.ToString() + "|";

                        rSn += CLanguage.Lan("状态:") + data.alarmInfo;
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

        #region 定时扫描
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                Stopwatch watcher = new Stopwatch();

                watcher.Start();

                CData data = new CData();

                if (!comMon.ReadLoadValue(addr, ref data, out er))
                {
                    watcher.Stop();
                    showInfo(CLanguage.Lan("读取负载电流地址") + "[" + addr.ToString("D2") + "]"+ CLanguage.Lan("错误") + ":[" + 
                                           watcher.ElapsedMilliseconds.ToString() + "ms]:" + er, true);
                    return;
                }

                for (int i = 0; i < comMon.maxCH; i++)
                {
                    labVolt[i].Text = data.chan[i].volt.ToString();
                    labLoad[i].Text = data.chan[i].current.ToString();
                }

                labLed1.ImageKey = (data.inv_status.OTP == EINVOTP.正常 ? "H" : "F");
                labLed2.ImageKey = (data.inv_status.AD == EINVAD.正常 ? "H" : "F");
                labLed3.ImageKey = (data.inv_status.ACBus == EINVACBus.正常 ? "H" : "F");
                labLed4.ImageKey = (data.inv_status.Fan == EINVFan.正常 ? "H" : "F");
                labLed5.ImageKey = (data.inv_status.Time == EINVTIME.正常 ? "H" : "F");
                labLed6.ImageKey = (data.inv_status.DCBus == EINVDCBus.正常 ? "H" : "F");

                watcher.Stop();

                showInfo(CLanguage.Lan("读取负载电流地址") + "[" + addr.ToString("D2") + "]OK:" + watcher.ElapsedMilliseconds.ToString() + "ms");
            }
            catch (Exception)
            {

            }          
        }
        #endregion

    }
}
