using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GJ.PLUGINS;
using GJ.DEV.CARD;
using GJ.COM;
namespace GJ.TOOL
{
    public partial class FrmIDCard : Form,IChildMsg
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
                btnOpen.Text = CLanguage.Lan("打开");
                labStatus.Text = CLanguage.Lan("关闭串口");
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
            cmbVer.Items.Clear();
            cmbVer.Items.Add(CLanguage.Lan("旧版本A"));
            cmbVer.Items.Add(CLanguage.Lan("新版本B"));
            cmbVer.SelectedIndex = 1;
            cmdMode.Items.Clear();
            cmdMode.Items.Add(CLanguage.Lan("模式") + "A");
            cmdMode.Items.Add(CLanguage.Lan("模式") + "B");
            cmdMode.Items.Add(CLanguage.Lan("模式") + "C");
            cmdMode.SelectedIndex = 0; 
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
        public FrmIDCard()
        {
            InitializeComponent();

            InitialControl();

            SetDoubleBuffered();
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 绑定控件
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
       
        #region 面板回调函数
        private void FrmIDCard_Load(object sender, EventArgs e)
        {
            string[] com = System.IO.Ports.SerialPort.GetPortNames();
            cmbCOM.Items.Clear();
            for (int i = 0; i < com.Length; i++)
                cmbCOM.Items.Add(com[i]);
            if (com.Length > 0)
                cmbCOM.Text = com[0];
            cmbVer.Items.Clear();
            cmbVer.Items.Add(CLanguage.Lan("旧版本A"));
            cmbVer.Items.Add(CLanguage.Lan("新版本B"));
            cmbVer.SelectedIndex = 1;
            cmdMode.Items.Clear();
            cmdMode.Items.Add(CLanguage.Lan("模式") + "A");
            cmdMode.Items.Add(CLanguage.Lan("模式") + "B");
            cmdMode.Items.Add(CLanguage.Lan("模式") + "C");
            cmdMode.SelectedIndex = 0; 
        }
        private void FrmIDCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (comMon != null)
            {
                comMon.Close();
                comMon = null;
            }
        }
        private void cmbVer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comMon == null)
            {
                labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                labStatus.ForeColor = Color.Red;
                return;
            }
            if (cmbVer.SelectedIndex == 0)
                comMon.version = "A";
            else
                comMon.version = "B";  
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (cmbCOM.Text == "")
            {
                labStatus.Text = CLanguage.Lan("请输入串口编号");
                labStatus.ForeColor = Color.Red;
                return;
            }
            string er = string.Empty;
            if (comMon == null)
            {
                comMon = new CCARDCom(ECardType.MFID);
                if (!comMon.Open(cmbCOM.Text, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    comMon = null;
                    return;
                }
                if (cmbVer.SelectedIndex == 0)
                    comMon.version = "A";
                else
                    comMon.version = "B";  
                btnOpen.Text = CLanguage.Lan("关闭");
                labStatus.Text = CLanguage.Lan("成功打开串口");
                labStatus.ForeColor = Color.Blue;
                cmbCOM.Enabled = false; 
            }
            else
            {
                comMon.Close();
                comMon = null;
                btnOpen.Text = CLanguage.Lan("打开");
                labStatus.Text = CLanguage.Lan("关闭串口");
                labStatus.ForeColor = Color.Blue;
                cmbCOM.Enabled = true;
            }
        }
        private void btnRAddr_Click(object sender, EventArgs e)
        {
            if (comMon == null)
            {
                labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                labStatus.ForeColor = Color.Red;
                return;
            }
            if (txtSn.Text == "")
            {
                labStatus.Text = CLanguage.Lan("请输入模块序列号");
                labStatus.ForeColor = Color.Red;
                return;
            }

            string er = string.Empty;

            int idAddr = 0;

            if (comMon.GetRecorderID(txtSn.Text, out idAddr, out er))
            {
                txtAddr.Text = idAddr.ToString();
                labStatus.Text =CLanguage.Lan("读取模块地址成功");
                labStatus.ForeColor = Color.Blue;
            }
            else
            {
                txtAddr.Text = "0";
                labStatus.Text = CLanguage.Lan("读取模块地址失败") + ":" + er;
                labStatus.ForeColor = Color.Red;
            }
        }
        private void btnSAddr_Click(object sender, EventArgs e)
        {
            if (comMon == null)
            {
                labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                labStatus.ForeColor = Color.Red;
                return;
            }
            if (txtSn.Text == "")
            {
                labStatus.Text = CLanguage.Lan("请输入模块序列号");
                labStatus.ForeColor = Color.Red;
                return;
            }
            if (txtAddr.Text == "")
            {
                labStatus.Text =CLanguage.Lan("请输入模块地址");
                labStatus.ForeColor = Color.Red;
                return;
            }
            string er = string.Empty;

            int idAddr = System.Convert.ToInt32(txtAddr.Text);

            if (comMon.SetRecorderID(txtSn.Text, idAddr, out er))
            {
                labStatus.Text =CLanguage.Lan("设置模块地址成功");
                labStatus.ForeColor = Color.Blue;
            }
            else
            {
                txtAddr.Text = "0";
                labStatus.Text = CLanguage.Lan("设置模块地址失败") + ":" + er;
                labStatus.ForeColor = Color.Red;
            }
        }
        private void btnRSn_Click(object sender, EventArgs e)
        {
            if (comMon == null)
            {
                labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                labStatus.ForeColor = Color.Red;
                return;
            }

            if (txtAddr.Text == "")
            {
                labStatus.Text = CLanguage.Lan("请输入模块地址");
                labStatus.ForeColor = Color.Red;
                return;
            }
            string er = string.Empty;

            int idAddr = System.Convert.ToInt32(txtAddr.Text);

            string Sn = string.Empty;

            if (comMon.GetRecorderSn(idAddr, out Sn, out er))
            {
                txtSn.Text = Sn;
                labStatus.Text = CLanguage.Lan("读取模块序列号成功");
                labStatus.ForeColor = Color.Blue;
            }
            else
            {
                labStatus.Text = CLanguage.Lan("读取模块序列号失败") + ":" + er;
                labStatus.ForeColor = Color.Red;
            }
        }
        private void btnRMode_Click(object sender, EventArgs e)
        {
            if (comMon == null)
            {
                labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                labStatus.ForeColor = Color.Red;
                return;
            }

            if (txtAddr.Text == "")
            {
                labStatus.Text = CLanguage.Lan("请输入模块地址");
                labStatus.ForeColor = Color.Red;
                return;
            }
            string er = string.Empty;

            int idAddr = System.Convert.ToInt32(txtAddr.Text);

            EMode mode = EMode.A;

            if (comMon.GetRecordMode(idAddr, out mode, out er))
            {
                cmdMode.SelectedIndex = (int)mode;
                labStatus.Text = CLanguage.Lan("读取模块工作模式成功");
                labStatus.ForeColor = Color.Blue;
            }
            else
            {
                labStatus.Text = CLanguage.Lan("读取模块工作模式失败") + ":" + er;
                labStatus.ForeColor = Color.Red;
            }
        }
        private void btnSMode_Click(object sender, EventArgs e)
        {
            if (comMon == null)
            {
                labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                labStatus.ForeColor = Color.Red;
                return;
            }
            if (!chkBoard.Checked && txtAddr.Text == "")
            {
                labStatus.Text = CLanguage.Lan("请输入模块地址");
                labStatus.ForeColor = Color.Red;
                return;
            }
            string er = string.Empty;

            int idAddr = System.Convert.ToInt32(txtAddr.Text);

            EMode mode = (EMode)cmdMode.SelectedIndex;

            if (chkBoard.Checked)
            {
                if (comMon.SetRecorderWorkMode(mode, out er))
                {
                    labStatus.Text = CLanguage.Lan("广播设置模块工作模式成功");
                    labStatus.ForeColor = Color.Blue;
                }
                else
                {
                    labStatus.Text = CLanguage.Lan("广播设置模块工作模式失败") + ":" + er;
                    labStatus.ForeColor = Color.Red;
                }
            }
            else
            {
                if (comMon.SetRecorderWorkMode(idAddr, mode, out er))
                {
                    labStatus.Text = CLanguage.Lan("设置模块工作模式成功");
                    labStatus.ForeColor = Color.Blue;
                }
                else
                {
                    labStatus.Text =CLanguage.Lan("设置模块工作模式失败") + ":" + er;
                    labStatus.ForeColor = Color.Red;
                }
            }
        }
        private void btnRIdCard_Click(object sender, EventArgs e)
        {
            if (comMon == null)
            {
                labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                labStatus.ForeColor = Color.Red;
                return;
            }
            if (txtAddr.Text == "")
            {
                labStatus.Text = CLanguage.Lan("请输入模块地址");
                labStatus.ForeColor = Color.Red;
                return;
            }
            string er = string.Empty;

            int idAddr = System.Convert.ToInt32(txtAddr.Text);

            string idCard = string.Empty;

            if (comMon.GetRecord(idAddr, out idCard, out er))
            {
                txtIdCard.Text = idCard;
                labStatus.Text = CLanguage.Lan("读取模块卡片资料成功");
                labStatus.ForeColor = Color.Blue;
            }
            else
            {
                labStatus.Text = CLanguage.Lan("读取模块卡片资料失败")+ ":"+ er;
                labStatus.ForeColor = Color.Red;
            }
        }
        private void btnRAIdCard_Click(object sender, EventArgs e)
        {
            if (comMon == null)
            {
                labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                labStatus.ForeColor = Color.Red;
                return;
            }
            if (txtAddr.Text == "")
            {
                labStatus.Text = CLanguage.Lan("请输入模块地址");
                labStatus.ForeColor = Color.Red;
                return;
            }
            string er = string.Empty;

            int idAddr = System.Convert.ToInt32(txtAddr.Text);

            string idCard = string.Empty;

            if (comMon.GetRecordAgain(idAddr, out idCard, out er))
            {
                txtIdCard.Text = idCard;
                labStatus.Text = CLanguage.Lan("重读模块卡片资料成功");
                labStatus.ForeColor = Color.Blue;
            }
            else
            {
                labStatus.Text = CLanguage.Lan("重读模块卡片资料失败") + ":" + er;
                labStatus.ForeColor = Color.Red;
            }
        }
        private void btnRIdTrigger_Click(object sender, EventArgs e)
        {
            if (comMon == null)
            {
                labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                labStatus.ForeColor = Color.Red;
                return;
            }
            if (txtAddr.Text == "")
            {
                labStatus.Text =CLanguage.Lan("请输入模块地址");
                labStatus.ForeColor = Color.Red;
                return;
            }
            string er = string.Empty;

            int idAddr = System.Convert.ToInt32(txtAddr.Text);

            string idCard = string.Empty;

            bool rTrigger = false;

            if (comMon.GetRecordTriggerSignal(idAddr, out idCard, out rTrigger, out er))
            {
                txtIdCard.Text = idCard;
                if (rTrigger)
                    labTrigger.ImageKey = "H";
                else
                    labTrigger.ImageKey = "L";
                labStatus.Text =CLanguage.Lan("读取模块卡片资料成功");
                labStatus.ForeColor = Color.Blue;
            }
            else
            {
                labStatus.Text = CLanguage.Lan("读取模块卡片资料失败") + ":" + er;
                labStatus.ForeColor = Color.Red;
            }
        }
        private void btnRTrigger_Click(object sender, EventArgs e)
        {
            if (comMon == null)
            {
                labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                labStatus.ForeColor = Color.Red;
                return;
            }
            if (txtAddr.Text == "")
            {
                labStatus.Text = CLanguage.Lan("请输入模块地址");
                labStatus.ForeColor = Color.Red;
                return;
            }
            string er = string.Empty;

            int idAddr = System.Convert.ToInt32(txtAddr.Text);

            bool rTrigger = false;

            if (comMon.GetTriggerSignal(idAddr, out rTrigger, out er))
            {
                if (rTrigger)
                    labTrigger.ImageKey = "H";
                else
                    labTrigger.ImageKey = "L";
                labStatus.Text = CLanguage.Lan("读取模块触发信号成功");
                labStatus.ForeColor = Color.Blue;
            }
            else
            {
                labStatus.Text = CLanguage.Lan("读取模块触发信号失败") + ":" + er;
                labStatus.ForeColor = Color.Red;
            }
        }
        private void cmbVer_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comMon == null)
            {
                labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                labStatus.ForeColor = Color.Red;
                return;
            }
            if (cmbVer.SelectedIndex == 0)
                comMon.version = "A";
            else
                comMon.version = "B";  
        }
        #endregion

        #region 字段
        private CCARDCom comMon = null;
        #endregion

        #region 扫描监控
        private int _rowNum = 0;
        private bool _scanStopFlag = false;
        private int _startAddr = 1;
        private int _endAddr = 16;
        private int _delayMs = 200;
        private bool _chkData = true;
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

                    _delayMs = System.Convert.ToInt32(txtDelayMs.Text);
                    _chkData = chkData.Checked;
                    _startAddr = System.Convert.ToInt16(txtStartAddr.Text);
                    _endAddr = System.Convert.ToInt16(txtEndAddr.Text);

                    IDView.Rows.Clear();

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

                    string rSn = string.Empty;

                    if (!comMon.GetRecorderSn(i, out rSn, out er))
                        result = false;

                    if (_chkData)
                    {
                        System.Threading.Thread.Sleep(_delayMs);

                        if (!comMon.GetRecord(i, out rData, out er))
                            result = false;
                    }

                    ShowID(i, rSn, rData, result);

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
        private void ShowID(int Addr,string rSn, string rData, bool result)
        { 
          if(this.InvokeRequired)
              this.Invoke(new Action<int, string, string, bool>(ShowID), Addr,rSn, rData, result);
          else
          {
              if (result)
              {
                  IDView.Rows.Add(Addr, "PASS", rSn, rData);
                  IDView.Rows[_rowNum].Cells[1].Style.BackColor = Color.LimeGreen;
              }
              else
              {
                  IDView.Rows.Add(Addr, "FAIL", rSn, rData);
                  IDView.Rows[_rowNum].Cells[1].Style.BackColor = Color.Red;
              }
              IDView.CurrentCell = IDView.Rows[IDView.Rows.Count - 1].Cells[0];
          }
        }
        #endregion

    }
}
