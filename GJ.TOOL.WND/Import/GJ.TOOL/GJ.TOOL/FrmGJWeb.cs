using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using GJ.PLUGINS;
using GJ.MES;
using GJ.COM;

namespace GJ.TOOL
{
    public partial class FrmGJWeb : Form, IChildMsg
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
        public FrmGJWeb()
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
        private string iniFile = Application.StartupPath + "\\iniFile.ini";
        /// <summary>
        /// 时间监控
        /// </summary>
        private Stopwatch watcher = new Stopwatch();
        #endregion

        #region 面板回调函数
        private void FrmGJWeb_Load(object sender, EventArgs e)
        {
            cmbStatNum.SelectedIndex = 0;

            InitialFlowPanel(cmbStatNum.SelectedIndex + 1);

            cmbSlotMax.SelectedIndex = 19;

            cmbUseStatus.SelectedIndex = 1;  

            cmbMes.SelectedIndex = 0;

            cmbChkSn.SelectedIndex = 0;

            txtUlrWeb.Text = CIniFile.ReadFromIni("ToolDebug", "ulrWeb", iniFile, "http://100.100.20.175:8080/Service.asmx");

            InitialSnPanel(cmbSlotMax.SelectedIndex+1);

            dtYieldStartTime.Value = DateTime.Now;

            dtYieldEndTime.Value = DateTime.Now;
        }
        private void btnCon_Click(object sender, EventArgs e)
        {
            try
            {
                btnCon.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    string er=string.Empty;

                    string ulrWeb=txtUlrWeb.Text;

                    string version=string.Empty;

                    watcher.Restart();

                    if (!CGJWeb.CheckSystem(ulrWeb, out version, out er))
                    {
                        watcher.Stop();
                        labStatus.Text = er;
                        labStatus.ForeColor = Color.Red;
                        return;
                    }
                    watcher.Stop();

                    rtbRtn.Text = version; 
                    labStatus.Text = "连接Web服务端正常.";
                    labStatus.ForeColor = Color.Blue;
                    btnCon.ImageKey = "Con";
                    btnCon.Text = "断开";

                    CIniFile.WriteToIni("ToolDebug", "ulrWeb", ulrWeb, iniFile);
                }
                else
                {                    
                    labStatus.Text = "断开Web服务端连接.";
                    labStatus.ForeColor = Color.Blue;
                    btnCon.ImageKey = "Dis";
                    btnCon.Text = "连接";
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                btnCon.Enabled = true;

                labTimes.Text = watcher.ElapsedMilliseconds.ToString(); 
            }
        }
        private void btnFun_Click(object sender, EventArgs e)
        {
            try
            {
                btnFun.Enabled = false;

                watcher.Restart();

                string er = string.Empty;

                string reponseXml = string.Empty;

                if (!CGJWeb.PostFunction(txtFunName.Text, rtbSend.Text, out  reponseXml, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                rtbRtn.Text = reponseXml;
                labStatus.Text = "调用["+ txtFunName.Text +"]成功.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

                btnFun.Enabled = true;

                watcher.Stop();

                labTimes.Text = watcher.ElapsedMilliseconds.ToString(); 
            }
        }
        private void btnXML_Click(object sender, EventArgs e)
        {
            try
            {
                btnXML.Enabled = false;

                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "XML files (*.xml)|*.xml";
                //dlg.InitialDirectory = Application.StartupPath;
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                txtFunName.Text = Path.GetFileNameWithoutExtension(dlg.FileName);
                StreamReader sr=new StreamReader(dlg.FileName);
                string rData=sr.ReadToEnd();
                sr.Close();
                sr = null;
                rtbSend.Text=rData; 
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                btnXML.Enabled = true;
            }
        }
        #endregion

        #region 流程设置
        private TableLayoutPanel FlowPanel = null;
        private List<Label> labStatId = new List<Label>();
        private List<TextBox> txtStatName = new List<TextBox>();
        private List<ComboBox> cmbFlowId = new List<ComboBox>();
        private List<TextBox> txtFlowName = new List<TextBox>();
        private List<ComboBox> cmbDisable = new List<ComboBox>(); 
        private void btnSet_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要重新设置站别与流程?", "流程设置", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                DialogResult.Yes)
                return;

            InitialFlowPanel(cmbStatNum.SelectedIndex + 1); 
        }
        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                btnRead.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    MessageBox.Show("请先连接Web服务器", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                List<CGJWeb.CSTAT_FLOW> StatFlow = null;

                watcher.Restart();

                if (!CGJWeb.GetStatFlowList(out StatFlow, out er))
                {
                    watcher.Stop();
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;                   
                    return;
                }
                watcher.Stop();

                RefreshFlowPanel(StatFlow);

                rtbRtn.Text = er;
                labStatus.Text = "读取站别流程成功.";
                labStatus.ForeColor = Color.Blue;

            }
            catch (Exception)
            {                
                throw;
            }
            finally
            {
                btnRead.Enabled = true;

                labTimes.Text = watcher.ElapsedMilliseconds.ToString();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    MessageBox.Show("请先连接Web服务器", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (MessageBox.Show("确定要保存站别与流程设置?", "流程设置", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                                      DialogResult.Yes)
                    return;

                for (int i = 0; i < labStatId.Count; i++)
                {
                    if (txtStatName[i].Text == "")
                    {
                        MessageBox.Show("站别名称不能为空", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (txtFlowName[i].Text == "")
                    {
                        MessageBox.Show("流程名称不能为空", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                string er = string.Empty;

                watcher.Restart();

                List<int> flowIds = new List<int>();

                List<CGJWeb.CFLOW> flowList = new List<CGJWeb.CFLOW>();

                for (int i = 0; i < cmbFlowId.Count; i++)
                {
                    if (cmbDisable[i].SelectedIndex==0 && !flowIds.Contains(cmbFlowId[i].SelectedIndex + 1))
                    {
                        flowIds.Add(cmbFlowId[i].SelectedIndex + 1);

                        CGJWeb.CFLOW flow = new CGJWeb.CFLOW();

                        flow.Id = cmbFlowId[i].SelectedIndex + 1;

                        flow.Name = txtFlowName[i].Text;

                        flow.Disable = 0;

                        flowList.Add(flow); 
                    }
                }

                if (!CGJWeb.SetFlowList(flowList, out er))
                {
                    watcher.Stop();
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                List<CGJWeb.CSTAT> statList = new List<CGJWeb.CSTAT>();

                for (int i = 0; i < labStatId.Count; i++)
                {
                    CGJWeb.CSTAT stat = new CGJWeb.CSTAT();

                    stat.Id = i + 1;

                    stat.Name = txtStatName[i].Text;

                    stat.Disable = cmbDisable[i].SelectedIndex;

                    statList.Add(stat); 
                }
                if (!CGJWeb.SetStatList(statList, out er))
                {
                    watcher.Stop();
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                List<CGJWeb.CSTAT_FLOW> statFlowList = new List<CGJWeb.CSTAT_FLOW>();

                for (int i = 0; i < labStatId.Count; i++)
                {
                    CGJWeb.CSTAT_FLOW statFlow = new CGJWeb.CSTAT_FLOW();

                    statFlow.StatId = i + 1;

                    statFlow.StatName = txtStatName[i].Text;

                    statFlow.FlowId = cmbFlowId[i].SelectedIndex + 1;

                    statFlow.FlowName = txtFlowName[i].Text;

                    statFlow.Disable = cmbDisable[i].SelectedIndex;

                    statFlowList.Add(statFlow);
                }

                if (!CGJWeb.SetStatToFlowList(statFlowList, out er))
                {
                    watcher.Stop();
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                labStatus.Text = "保存站别流程成功.";
                labStatus.ForeColor = Color.Blue;

                MessageBox.Show("保存站别流程成功", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                watcher.Stop();
                btnSave.Enabled = true;
                labTimes.Text = watcher.ElapsedMilliseconds.ToString();
            }
        }
        /// <summary>
        /// 初始化设置
        /// </summary>
        /// <param name="statNum"></param>
        private void InitialFlowPanel(int statNum)
        {
            if (FlowPanel != null)
            {
                foreach (Control item in FlowPanel.Controls)
                    item.Dispose();
                FlowPanel.Dispose();
                FlowPanel = null;
            }
            labStatId.Clear();
            txtStatName.Clear();
            cmbFlowId.Clear();
            txtFlowName.Clear();
            cmbDisable.Clear();  

            FlowPanel = new TableLayoutPanel();
            FlowPanel.Dock = DockStyle.Fill;
            FlowPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            FlowPanel.GetType().GetProperty("DoubleBuffered",
                                            System.Reflection.BindingFlags.Instance |
                                            System.Reflection.BindingFlags.NonPublic)
                                            .SetValue(FlowPanel, true, null);
            FlowPanel.RowCount = statNum + 2;
            for (int i = 0; i < statNum + 1; i++)
                FlowPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            FlowPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            FlowPanel.ColumnCount = 6;
            FlowPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,80));
            FlowPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            FlowPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            FlowPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            FlowPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            FlowPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            Label labT1 = new Label();
            labT1.Dock = DockStyle.Fill;
            labT1.Margin = new Padding(1);
            labT1.TextAlign = ContentAlignment.MiddleCenter;  
            labT1.Text = "编号";
            FlowPanel.Controls.Add(labT1, 0, 0); 

            Label labT2 = new Label();
            labT2.Dock = DockStyle.Fill;
            labT2.Margin = new Padding(1);
            labT2.TextAlign = ContentAlignment.MiddleCenter;  
            labT2.Text = "站别名称";
            FlowPanel.Controls.Add(labT2, 1, 0); 

            Label labT3 = new Label();
            labT3.Dock = DockStyle.Fill;
            labT3.Margin = new Padding(1);
            labT3.TextAlign = ContentAlignment.MiddleCenter;  
            labT3.Text = "流程编号";
            FlowPanel.Controls.Add(labT3, 2, 0); 

            Label labT4 = new Label();
            labT4.Dock = DockStyle.Fill;
            labT4.Margin = new Padding(1);
            labT4.TextAlign = ContentAlignment.MiddleCenter;  
            labT4.Text = "流程名称";
            FlowPanel.Controls.Add(labT4, 3, 0);

            Label labT5 = new Label();
            labT5.Dock = DockStyle.Fill;
            labT5.Margin = new Padding(1);
            labT5.TextAlign = ContentAlignment.MiddleCenter;
            labT5.Text = "使用状态";
            FlowPanel.Controls.Add(labT5, 4, 0); 

            for (int i = 0; i < statNum; i++)
            {
                Label lab1 = new Label();
                lab1.Dock = DockStyle.Fill;
                lab1.Margin = new Padding(1);
                lab1.Text = (i+1).ToString();
                lab1.TextAlign = ContentAlignment.MiddleCenter;  
                labStatId.Add(lab1);

                TextBox txt1 = new TextBox();
                txt1.Dock = DockStyle.Fill;
                txt1.Margin = new Padding(1);
                txt1.TextAlign = HorizontalAlignment.Center;
                txt1.Text = "";
                txtStatName.Add(txt1);

                ComboBox cmb1 = new ComboBox();
                cmb1.Dock = DockStyle.Fill;
                cmb1.Margin = new Padding(1);
                cmb1.DropDownStyle = ComboBoxStyle.DropDownList;
                for (int z = 0; z < 12; z++)
                    cmb1.Items.Add(z + 1);
                cmb1.SelectedIndex = i;
                cmbFlowId.Add(cmb1);

                TextBox txt2 = new TextBox();
                txt2.Dock = DockStyle.Fill;
                txt2.Margin = new Padding(1);
                txt2.TextAlign = HorizontalAlignment.Center;
                txt2.Text = "";
                txtFlowName.Add(txt2);

                ComboBox cmb2 = new ComboBox();
                cmb2.Dock = DockStyle.Fill;
                cmb2.Margin = new Padding(1);
                cmb2.DropDownStyle = ComboBoxStyle.DropDownList;
                cmb2.Items.Add("启用");
                cmb2.Items.Add("禁用");
                cmb2.SelectedIndex = 0;
                cmbDisable.Add(cmb2);  

                FlowPanel.Controls.Add(labStatId[i], 0, i + 1);
                FlowPanel.Controls.Add(txtStatName[i], 1, i + 1);
                FlowPanel.Controls.Add(cmbFlowId[i], 2, i + 1);
                FlowPanel.Controls.Add(txtFlowName[i], 3, i + 1);
                FlowPanel.Controls.Add(cmbDisable[i], 4, i + 1); 
                
            }

            panel6.Controls.Add(FlowPanel, 0, 1);   
        }
        /// <summary>
        /// 刷新设置
        /// </summary>
        /// <param name="StatFlow"></param>
        private void RefreshFlowPanel(List<CGJWeb.CSTAT_FLOW> StatFlow)
        {
            if (FlowPanel != null)
            {
                foreach (Control item in FlowPanel.Controls)
                    item.Dispose();
                FlowPanel.Dispose();
                FlowPanel = null;
            }
            labStatId.Clear();
            txtStatName.Clear();
            cmbFlowId.Clear();
            txtFlowName.Clear();
            cmbDisable.Clear();

            FlowPanel = new TableLayoutPanel();
            FlowPanel.Dock = DockStyle.Fill;
            FlowPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            FlowPanel.GetType().GetProperty("DoubleBuffered",
                                            System.Reflection.BindingFlags.Instance |
                                            System.Reflection.BindingFlags.NonPublic)
                                            .SetValue(FlowPanel, true, null);
            FlowPanel.RowCount = StatFlow.Count + 2;
            for (int i = 0; i < StatFlow.Count + 1; i++)
                FlowPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            FlowPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            FlowPanel.ColumnCount = 6;
            FlowPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            FlowPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            FlowPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            FlowPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            FlowPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            FlowPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            Label labT1 = new Label();
            labT1.Dock = DockStyle.Fill;
            labT1.Margin = new Padding(1);
            labT1.TextAlign = ContentAlignment.MiddleCenter;
            labT1.Text = "编号";
            FlowPanel.Controls.Add(labT1, 0, 0);

            Label labT2 = new Label();
            labT2.Dock = DockStyle.Fill;
            labT2.Margin = new Padding(1);
            labT2.TextAlign = ContentAlignment.MiddleCenter;
            labT2.Text = "站别名称";
            FlowPanel.Controls.Add(labT2, 1, 0);

            Label labT3 = new Label();
            labT3.Dock = DockStyle.Fill;
            labT3.Margin = new Padding(1);
            labT3.TextAlign = ContentAlignment.MiddleCenter;
            labT3.Text = "流程编号";
            FlowPanel.Controls.Add(labT3, 2, 0);

            Label labT4 = new Label();
            labT4.Dock = DockStyle.Fill;
            labT4.Margin = new Padding(1);
            labT4.TextAlign = ContentAlignment.MiddleCenter;
            labT4.Text = "流程名称";
            FlowPanel.Controls.Add(labT4, 3, 0);

            Label labT5 = new Label();
            labT5.Dock = DockStyle.Fill;
            labT5.Margin = new Padding(1);
            labT5.TextAlign = ContentAlignment.MiddleCenter;
            labT5.Text = "使用状态";
            FlowPanel.Controls.Add(labT5, 4, 0);

            for (int i = 0; i < StatFlow.Count; i++)
            {
                Label lab1 = new Label();
                lab1.Dock = DockStyle.Fill;
                lab1.Margin = new Padding(1);
                lab1.Text = (i + 1).ToString();
                lab1.TextAlign = ContentAlignment.MiddleCenter;
                labStatId.Add(lab1);

                TextBox txt1 = new TextBox();
                txt1.Dock = DockStyle.Fill;
                txt1.Margin = new Padding(1);
                txt1.TextAlign = HorizontalAlignment.Center;
                txt1.Text = StatFlow[i].StatName;
                txtStatName.Add(txt1);

                ComboBox cmb1 = new ComboBox();
                cmb1.Dock = DockStyle.Fill;
                cmb1.Margin = new Padding(1);
                cmb1.DropDownStyle = ComboBoxStyle.DropDownList;
                for (int z = 0; z < 12; z++)
                    cmb1.Items.Add(z + 1);
                cmb1.SelectedIndex = StatFlow[i].FlowId-1;
                cmbFlowId.Add(cmb1);

                TextBox txt2 = new TextBox();
                txt2.Dock = DockStyle.Fill;
                txt2.Margin = new Padding(1);
                txt2.TextAlign = HorizontalAlignment.Center;
                txt2.Text = StatFlow[i].FlowName;
                txtFlowName.Add(txt2);

                ComboBox cmb2 = new ComboBox();
                cmb2.Dock = DockStyle.Fill;
                cmb2.Margin = new Padding(1);
                cmb2.DropDownStyle = ComboBoxStyle.DropDownList;
                cmb2.Items.Add("启用");
                cmb2.Items.Add("禁用");
                cmb2.SelectedIndex = StatFlow[i].Disable;
                cmbDisable.Add(cmb2);

                FlowPanel.Controls.Add(labStatId[i], 0, i + 1);
                FlowPanel.Controls.Add(txtStatName[i], 1, i + 1);
                FlowPanel.Controls.Add(cmbFlowId[i], 2, i + 1);
                FlowPanel.Controls.Add(txtFlowName[i], 3, i + 1);
                FlowPanel.Controls.Add(cmbDisable[i], 4, i + 1);

            }

            panel6.Controls.Add(FlowPanel, 0, 1);   
        }
        #endregion

        #region 治具信息
        private TableLayoutPanel SnPanel = null;
        private List<TextBox> txtSn = new List<TextBox>();
        private List<TextBox> txtSnFlow = new List<TextBox>();
        private List<ComboBox> cmbSnResult = new List<ComboBox>();
        private void btnSetSn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要重新设置条码?", "治具信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                DialogResult.Yes)
                return;

            InitialSnPanel(cmbSlotMax.SelectedIndex + 1);
        }
        private void btnBandFix_Click(object sender, EventArgs e)
        {
            try
            {
                btnBandFix.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    MessageBox.Show("请先连接Web服务器", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                CGJWeb.CFIX_BAND fixture = new CGJWeb.CFIX_BAND();

                fixture.Base.IdCard = txtIdCard.Text;
                fixture.Base.MaxSlot = cmbSlotMax.SelectedIndex + 1;
                fixture.Base.LineNo = System.Convert.ToInt16(txtLineNo.Text);
                fixture.Base.LineName = txtLineName.Text;
                fixture.Base.Model = txtModel.Text;
                fixture.Base.OrderName = txtOrder.Text;
                fixture.Base.MesFlag = cmbMes.SelectedIndex;
                fixture.Base.UseStatus = (CGJWeb.EFIX_STATUS)(cmbUseStatus.SelectedIndex - 1);  

                for (int i = 0; i < fixture.Base.MaxSlot; i++)
                {
                    CGJWeb.CSN_INFO sn = new CGJWeb.CSN_INFO();

                    sn.SerialNo = txtSn[i].Text;  

                    fixture.Para.Add(sn); 
                }

                watcher.Restart(); 

                if (!CGJWeb.BandSnToFixture(fixture, out er))
                {                    
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                rtbRtn.Text = er;
                labStatus.Text = "绑定治具条码OK.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {                
                throw;
            }
            finally
            {
                watcher.Stop();

                btnBandFix.Enabled = true;

                labTimes.Text = watcher.ElapsedMilliseconds.ToString();
            }
        }
        private void btnGetFix_Click(object sender, EventArgs e)
        {
            try
            {
                btnGetFix.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    MessageBox.Show("请先连接Web服务器", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                watcher.Restart();

                CGJWeb.CFIX_BAND fixture = new CGJWeb.CFIX_BAND();

                if (!chkSnFlow.Checked)
                {
                    if (!CGJWeb.GetInfoFromFixture(txtIdCard.Text, out fixture, out er))
                    {
                        labStatus.Text = er;
                        labStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                else
                {
                    if (!CGJWeb.GetFlowSnFromFixture(txtIdCard.Text,txtFlow.Text, out fixture, out er))
                    {
                        labStatus.Text = er;
                        labStatus.ForeColor = Color.Red;
                        return;
                    }
                }

                cmbSlotMax.SelectedIndex=fixture.Base.MaxSlot-1;
                cmbUseStatus.SelectedIndex = (int)fixture.Base.UseStatus+1; 
                txtLineNo.Text= fixture.Base.LineNo.ToString();
                txtLineName.Text=fixture.Base.LineName;
                txtModel.Text=fixture.Base.Model;
                txtOrder.Text=fixture.Base.OrderName;
                cmbMes.SelectedIndex=fixture.Base.MesFlag;

                for (int i = 0; i < fixture.Base.MaxSlot; i++)
                {
                    txtSn[i].Text = fixture.Para[i].SerialNo;
                    txtSnFlow[i].Text = fixture.Para[i].CurFlowName;
                    cmbSnResult[i].SelectedIndex = fixture.Para[i].CurResult;  
                }

                rtbRtn.Text = er;
                labStatus.Text = "读取治具条码OK.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                watcher.Stop();

                btnGetFix.Enabled = true;

                labTimes.Text = watcher.ElapsedMilliseconds.ToString();
            }
        }
        private void btnWriteResult_Click(object sender, EventArgs e)
        {
            try
            {
                btnWriteResult.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    MessageBox.Show("请先连接Web服务器", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                watcher.Restart();

                CGJWeb.CFIX_RESULT fixture=new CGJWeb.CFIX_RESULT();

                fixture.Base.IdCard = txtIdCard.Text;

                fixture.Base.CurFlowName = txtFlow.Text;

                fixture.Base.CurStatName = txtStat.Text;

                fixture.Base.StartTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                fixture.Base.EndTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                fixture.Base.LocalName = "L1-01";

                fixture.Base.CheckSn = cmbChkSn.SelectedIndex;

                for (int i = 0; i < cmbSlotMax.SelectedIndex+1; i++)
                {
                    CGJWeb.CSN_RESULT Sn = new CGJWeb.CSN_RESULT();

                    Sn.SerialNo = txtSn[i].Text;

                    Sn.CurResult = cmbSnResult[i].SelectedIndex;

                    fixture.Para.Add(Sn);  
                }

                if (!CGJWeb.UpdateFixtureResult(fixture, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                rtbRtn.Text = er;
                labStatus.Text = "上传治具测试结果OK.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {                
                throw;
            }
            finally
            {
                watcher.Stop();

                btnWriteResult.Enabled = true;

                labTimes.Text = watcher.ElapsedMilliseconds.ToString();
            }
        }
        private void btnReBand_Click(object sender, EventArgs e)
        {
            try
            {
                btnBandFix.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    MessageBox.Show("请先连接Web服务器", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                CGJWeb.CFIX_BAND fixture = new CGJWeb.CFIX_BAND();

                fixture.Base.IdCard = txtIdCard.Text;
                fixture.Base.MaxSlot = cmbSlotMax.SelectedIndex + 1;
                fixture.Base.LineNo = System.Convert.ToInt16(txtLineNo.Text);
                fixture.Base.LineName = txtLineName.Text;
                fixture.Base.Model = txtModel.Text;
                fixture.Base.OrderName = txtOrder.Text;
                fixture.Base.MesFlag = cmbMes.SelectedIndex;

                for (int i = 0; i < fixture.Base.MaxSlot; i++)
                {
                    CGJWeb.CSN_INFO sn = new CGJWeb.CSN_INFO();

                    sn.SerialNo = txtSn[i].Text;

                    sn.CurFlowName = txtSnFlow[i].Text;

                    sn.CurStatName = txtStat.Text;

                    sn.CurResult = cmbSnResult[i].SelectedIndex; 

                    fixture.Para.Add(sn);
                }

                watcher.Restart();

                if (!CGJWeb.RegroupSnToFixture(fixture, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                rtbRtn.Text = er;
                labStatus.Text = "绑定治具条码OK.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                watcher.Stop();

                btnBandFix.Enabled = true;

                labTimes.Text = watcher.ElapsedMilliseconds.ToString();
            }
        }
        private void btnUseStatus_Click(object sender, EventArgs e)
        {
            try
            {
                btnUseStatus.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    MessageBox.Show("请先连接Web服务器", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                watcher.Restart();

                CGJWeb.EFIX_STATUS useStatus = (CGJWeb.EFIX_STATUS)(cmbUseStatus.SelectedIndex - 1);

                if (!CGJWeb.SetFixtureUseStaus(txtIdCard.Text, useStatus, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                rtbRtn.Text = er;
                labStatus.Text = "设置治具状态OK.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                watcher.Stop();

                btnUseStatus.Enabled = true;

                labTimes.Text = watcher.ElapsedMilliseconds.ToString();
            }
        }
        private void InitialSnPanel(int slotMax)
        {
            if (SnPanel != null)
            {
                foreach (Control item in SnPanel.Controls)
                    item.Dispose();
                SnPanel.Dispose();
                SnPanel = null;
            }
            txtSn.Clear();
            txtSnFlow.Clear();
            cmbSnResult.Clear();

            SnPanel = new TableLayoutPanel();
            SnPanel.Dock = DockStyle.Fill;
            SnPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            SnPanel.GetType().GetProperty("DoubleBuffered",
                                            System.Reflection.BindingFlags.Instance |
                                            System.Reflection.BindingFlags.NonPublic)
                                            .SetValue(SnPanel, true, null);
            SnPanel.RowCount = slotMax + 1;
            SnPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            for (int i = 0; i < slotMax; i++)
                SnPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));            
            SnPanel.ColumnCount = 4;
            SnPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));
            SnPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            SnPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            SnPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));

            Label labT1 = new Label();
            labT1.Dock = DockStyle.Fill;
            labT1.Margin = new Padding(1);
            labT1.TextAlign = ContentAlignment.MiddleCenter;
            labT1.Text = "编号";
            SnPanel.Controls.Add(labT1, 0, 0);

            Label labT2 = new Label();
            labT2.Dock = DockStyle.Fill;
            labT2.Margin = new Padding(1);
            labT2.TextAlign = ContentAlignment.MiddleCenter;
            labT2.Text = "产品条码";
            SnPanel.Controls.Add(labT2, 1, 0);

            Label labT3 = new Label();
            labT3.Dock = DockStyle.Fill;
            labT3.Margin = new Padding(1);
            labT3.TextAlign = ContentAlignment.MiddleCenter;
            labT3.Text = "流程名称";
            SnPanel.Controls.Add(labT3, 2, 0);

            Label labT4 = new Label();
            labT4.Dock = DockStyle.Fill;
            labT4.Margin = new Padding(1);
            labT4.TextAlign = ContentAlignment.MiddleCenter;
            labT4.Text = "结果";
            SnPanel.Controls.Add(labT4, 3, 0);

            for (int i = 0; i < slotMax; i++)
            {
                Label lab1 = new Label();
                lab1.Dock = DockStyle.Fill;
                lab1.Margin = new Padding(1);
                lab1.Text = (i + 1).ToString("D2");
                lab1.TextAlign = ContentAlignment.MiddleCenter;
                SnPanel.Controls.Add(lab1, 0, i+1);  

                TextBox txt1 = new TextBox();
                txt1.Dock = DockStyle.Fill;
                txt1.Margin = new Padding(1);
                txt1.TextAlign = HorizontalAlignment.Center;
                txt1.Text = "A000000" + (i+1).ToString("D2");
                txtSn.Add(txt1);
                SnPanel.Controls.Add(txtSn[i], 1, i+1); 

                TextBox txt2 = new TextBox();
                txt2.Dock = DockStyle.Fill;
                txt2.Margin = new Padding(1);
                txt2.TextAlign = HorizontalAlignment.Center;
                txt2.Text = "";
                txtSnFlow.Add(txt2);
                SnPanel.Controls.Add(txtSnFlow[i], 2, i + 1); 

                ComboBox cmb1 = new ComboBox();
                cmb1.Dock = DockStyle.Fill;
                cmb1.Margin = new Padding(1);
                cmb1.DropDownStyle = ComboBoxStyle.DropDownList;
                cmb1.Items.Add("良品");
                cmb1.Items.Add("不良"); 
                cmb1.SelectedIndex = 0;
                cmbSnResult.Add(cmb1);
                SnPanel.Controls.Add(cmbSnResult[i], 3, i + 1); 
            }

            panel11.Controls.Add(SnPanel, 1, 0); 
        }
        #endregion

        #region 产能
        private void btnStatYield_Click(object sender, EventArgs e)
        {
            try
            {
                btnStatYield.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    MessageBox.Show("请先连接Web服务器", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                watcher.Restart();

                List<CGJWeb.CSTAT_YIELD> yields = null;

                if (!CGJWeb.GetStatYield(txtStat.Text, out yields, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                rtbRtn.Text = er;
                labStatus.Text = "读取工位产能OK.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                watcher.Stop();

                btnStatYield.Enabled = true;

                labTimes.Text = watcher.ElapsedMilliseconds.ToString();
            }
        }
        private void btnFixYield_Click(object sender, EventArgs e)
        {
            try
            {
                btnFixYield.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    MessageBox.Show("请先连接Web服务器", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                watcher.Restart();

                CGJWeb.CFIX_YIELD yields = null;

                if (!CGJWeb.GetIdCardYield(txtIdCard.Text, txtStat.Text, out yields, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                rtbRtn.Text = er;
                labStatus.Text = "读取治具产能OK.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                watcher.Stop();

                btnFixYield.Enabled = true;

                labTimes.Text = watcher.ElapsedMilliseconds.ToString();
            }
        }
        private void btnClrStat_Click(object sender, EventArgs e)
        {
            try
            {
                btnClrStat.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    MessageBox.Show("请先连接Web服务器", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                watcher.Restart();

                if (!CGJWeb.ClrStatYield(txtStat.Text, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                rtbRtn.Text = er;
                labStatus.Text = "归零工位产能OK.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                watcher.Stop();

                btnClrStat.Enabled = true;

                labTimes.Text = watcher.ElapsedMilliseconds.ToString();
            }
        }
        private void btnClrFix_Click(object sender, EventArgs e)
        {
            try
            {
                btnClrFix.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    MessageBox.Show("请先连接Web服务器", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                watcher.Restart();

                if (!CGJWeb.ClrIdCardYield(txtIdCard.Text, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                rtbRtn.Text = er;
                labStatus.Text = "归零工位产能OK.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                watcher.Stop();

                btnClrFix.Enabled = true;

                labTimes.Text = watcher.ElapsedMilliseconds.ToString();
            }
        }
        private void btnYieldQuery_Click(object sender, EventArgs e)
        {
            try
            {
                btnYieldQuery.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    MessageBox.Show("请先连接Web服务器", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                watcher.Restart();

                List<CGJWeb.CYieldRequest> request = new List<CGJWeb.CYieldRequest>();

                List<CGJWeb.CYieldReponse> reponse = null;

                request.Add(new CGJWeb.CYieldRequest()
                            {
                                FlowName = txtYieldFlow.Text,
                                StartTime = dtYieldStartTime.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                                EndTime = dtYieldEndTime.Value.ToString("yyyy/MM/dd HH:mm:ss")
                            });

                if (!CGJWeb.GetYieldInStationAndTime(request, out reponse, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                if (reponse.Count > 0)
                {
                    labYieldTTNum.Text = reponse[0].TTNum.ToString();
                    labYieldFailNum.Text = reponse[0].FailNum.ToString();
                }
                else
                {
                    labYieldTTNum.Text = "0";
                    labYieldFailNum.Text = "0";
                }

                rtbRtn.Text = er;
                labStatus.Text = "读取工位时间段产能统计OK.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                watcher.Stop();

                btnYieldQuery.Enabled = true;

                labTimes.Text = watcher.ElapsedMilliseconds.ToString();
            }
        }
        #endregion

        #region 扩展功能
        private void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                btnResult.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    MessageBox.Show("请先连接Web服务器", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                watcher.Restart();

                List<CGJWeb.CSN_TESTDATA> rData = null;

                if (!CGJWeb.GetSnTestData(txtSn[0].Text, out rData, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                rtbRtn.Text = er;
                labStatus.Text = "读取条码[" + txtSn[0].Text + "]信息OK.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                watcher.Stop();

                btnResult.Enabled = true;

                labTimes.Text = watcher.ElapsedMilliseconds.ToString();
            }
        }
        private void btnChkSn_Click(object sender, EventArgs e)
        {
            try
            {
                btnChkSn.Enabled = false;

                if (btnCon.Text == "连接")
                {
                    MessageBox.Show("请先连接Web服务器", "流程设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                watcher.Restart();

                if (!CGJWeb.CheckSn(txtSn[0].Text,txtFlow.Text, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                rtbRtn.Text = er;
                labStatus.Text = "检查条码[" + txtSn[0].Text + "]OK.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                watcher.Stop();

                btnChkSn.Enabled = true;

                labTimes.Text = watcher.ElapsedMilliseconds.ToString();
            }
        }
        #endregion


    }
}
