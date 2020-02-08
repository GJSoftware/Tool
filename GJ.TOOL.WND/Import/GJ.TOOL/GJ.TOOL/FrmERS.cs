using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GJ.PLUGINS;
using GJ.DEV.ERS;
using GJ.COM; 

namespace GJ.TOOL
{
    public partial class FrmERS : Form,IChildMsg
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
        public FrmERS()
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
            for (int i = 0; i <CHAN_MAX; i++)
            {
                Label lab1 = new Label();

                lab1.Dock = DockStyle.Fill;

                lab1.Margin = new Padding(0);

                lab1.Text = "CH" + (i + 1).ToString("D2"); 

                lab1.TextAlign = ContentAlignment.MiddleCenter;

                labCH.Add(lab1);

                Label lab2 = new Label();

                lab2.Dock = DockStyle.Fill;

                lab2.BorderStyle = BorderStyle.Fixed3D; 

                lab2.Margin = new Padding(1);

                lab2.BackColor = Color.White;

                lab2.Text = "---";

                lab2.TextAlign = ContentAlignment.MiddleCenter;

                labLoad.Add(lab2);

                TextBox txt = new TextBox();

                txt.Dock = DockStyle.Fill;

                txt.Margin = new Padding(1);

                txt.Text = "0.5";

                txt.TextAlign = HorizontalAlignment.Center;

                txtLoad.Add(txt);

                panel3.Controls.Add(labCH[i], 0, i+1);

                panel3.Controls.Add(txtLoad[i], 1, i + 1);

                panel3.Controls.Add(labLoad[i], 2, i + 1);
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
        private CERSCom comMon = null;
        /// <summary>
        /// 小板数量
        /// </summary>
        private const int CHAN_MAX = 8;
        /// <summary>
        /// 当前地址
        /// </summary>
        private int curAddr = 0;
        /// <summary>
        /// 扫描行
        /// </summary>
        private int rowNum = 0;
        /// <summary>
        /// 取消
        /// </summary>
        private bool cancel = false;
        #endregion

        #region 面板控件
        private List<Label> labCH = new List<Label>();
        private List<TextBox> txtLoad = new List<TextBox>();
        private List<Label> labLoad = new List<Label>();
        #endregion

        #region 面板回调函数
        private void FrmERS_Load(object sender, EventArgs e)
        {
            cmbCom.Items.Clear();
            string[] com = System.IO.Ports.SerialPort.GetPortNames();
            for (int i = 0; i < com.Length; i++)
                cmbCom.Items.Add(com[i]);
            if (com.Length > 0)
                cmbCom.Text = com[0];
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            string er = string.Empty;

            if (cmbCom.Text == "")
            {
                labStatus.Text = "请输入串口编号";
                labStatus.ForeColor = Color.Red;
                return;
            }

            if (btnOpen.Text == "打开")
            {
                comMon = new CERSCom(EType.GJ272_4);

                if (!comMon.Open(cmbCom.Text, out er, txtBaud.Text))
                {
                    comMon = null;
                    labStatus.Text = "打开串口失败:" + er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                btnOpen.Text = "关闭";
                labStatus.Text = "成功打开串口";
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
                btnOpen.Text = "打开";
                labStatus.Text = "关闭串口";
                labStatus.ForeColor = Color.Black;
                cmbCom.Enabled = true;
            }
        }
        private void btnInfo_Click(object sender, EventArgs e)
        {
            try
            {
                btnInfo.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                string name = string.Empty;

                labVer.Text = ""; 

                string ver = string.Empty;

                if (!comMon.ReadVersion(addr, out ver, out er))
                {
                    showInfo("读取地址[" + addr.ToString("D2") + "]设备版本错误:" + er, true);
                    return;
                }

                labVer.Text = ver;

                showInfo("读取地址[" + addr.ToString("D2") + "]基本信息OK");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnSetAddr.Enabled = true;
            }
        }
        private void btnSetLoad_Click(object sender, EventArgs e)
        {
            try
            {
                btnSetLoad.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                CERS_Load load = new CERS_Load();

                for (int i = 0; i < txtLoad.Count; i++)
                    load.cur[i] = System.Convert.ToDouble(txtLoad[i].Text);

                if (!comMon.SetNewLoad(addr, load, out er,chkEPROM.Checked))
                {
                    showInfo("设置地址[" + addr.ToString("D2") + "]负载电流错误:" + er, true);
                    return;
                }

                showInfo("设置地址[" + addr.ToString("D2") + "]负载电流OK");

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
        private void btnSignal_Click(object sender, EventArgs e)
        {
            try
            {
                btnSignal.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                int chan = System.Convert.ToInt16(txtLoadCH.Text);  

                string er = string.Empty;

                double load =  System.Convert.ToDouble(txtLoad[chan -1].Text);

                if (!comMon.SetNewLoad(addr, chan, load, out er, chkEPROM.Checked))
                {
                    showInfo("设置地址[" + addr.ToString("D2") + "]负载电流错误:" + er, true);
                    return;
                }

                showInfo("设置地址[" + addr.ToString("D2") + "]负载电流OK");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnSignal.Enabled = true;
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

                CERS_Load load = null;

                if (!comMon.ReadData(addr,out load, out er))
                {
                    showInfo("读取地址[" + addr.ToString("D2") + "]负载电流错误:" + er, true);
                    return;
                }

                for (int i = 0; i < labLoad.Count; i++)
                {
                    labLoad[i].Text = load.cur[i].ToString("0.00");  
                }

                showInfo("读取地址[" + addr.ToString("D2") + "]负载电流OK");

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
        private void btnReadSet_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadSet.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                CERS_Load load = null;

                if (!comMon.ReadLoadSet(addr, out load, out er))
                {
                    showInfo("读取地址[" + addr.ToString("D2") + "]负载电流错误:" + er, true);
                    return;
                }

                for (int i = 0; i < labLoad.Count; i++)
                {
                    txtLoad[i].Text = load.cur[i].ToString("0.00");
                }

                showInfo("读取地址[" + addr.ToString("D2") + "]负载电流OK");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnReadSet.Enabled = true;
            }
        }
        private void btnScan_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnScan.Text == "扫描")
                {
                    if (!checkSystem())
                        return;

                    btnScan.Text = "停止";

                    curAddr = System.Convert.ToInt16(txtStartAddr.Text);

                    ERSView.Rows.Clear();

                    rowNum = 0;

                    cancel = false;

                    scanHandler scan = new scanHandler(OnScan);

                    scan.BeginInvoke(null, null);   
                }
                else
                {
                    cancel = true;

                    btnScan.Text = "扫描";
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }
        private delegate void scanHandler();
        private void OnScan()
        {
            while (true)
            {
                if (cancel)
                    return;

                string er = string.Empty;

                bool pass = true;

                string ver = string.Empty;

                System.Threading.Thread.Sleep(50);

                if (!comMon.ReadVersion(curAddr, out ver, out er))
                    pass = false;

                System.Threading.Thread.Sleep(20);

                string str = string.Empty;

                CERS_Load load = null;

                if (!comMon.ReadData(curAddr, out load, out er))
                {
                    pass = false;
                }
                else
                {
                    for (int z = 0; z < load.cur.Length; z++)
                    {
                        if (z < load.cur.Length - 1)
                            str += load.cur[z].ToString() + "|";
                        else
                            str += load.cur[z].ToString();
                    }
                }

                showView(curAddr, pass, ver, str);

                if (curAddr < System.Convert.ToInt16(txtEndAddr.Text))
                {
                    curAddr++;
                    rowNum++;
                }
                else
                {
                    showEnd();
                    return;
                }
            }
        }
        private void showEnd()
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(showEnd));
            else
            {
                btnScan.Text = "扫描";
            }
        }
        private void showView(int addr, bool result, string ver, string data)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<int, bool, string, string>(showView), addr, result, ver, data);
            else
            {
                if (result)
                {
                    ERSView.Rows.Add(curAddr, "PASS", ver, data);
                    ERSView.Rows[rowNum].Cells[1].Style.BackColor = Color.LimeGreen;
                }
                else
                {
                    ERSView.Rows.Add(curAddr, "FAIL", ver, data);
                    ERSView.Rows[rowNum].Cells[1].Style.BackColor = Color.Red;
                }
                ERSView.CurrentCell = ERSView.Rows[ERSView.Rows.Count - 1].Cells[0];
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
