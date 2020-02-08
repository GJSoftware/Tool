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
using GJ.MES;
using System.IO;

namespace GJ.TOOL
{
    public partial class FrmWeb2 : Form, IChildMsg
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
        public FrmWeb2()
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
        /// <summary>
        /// INI文件
        /// </summary>
        private string iniFile = Application.StartupPath + "\\iniFile.ini";
        /// <summary>
        /// 接口定义
        /// </summary>
        private string ulr = string.Empty;
        /// <summary>
        /// 检查接口状态
        /// </summary>
        private bool checkFlag = false;
        #endregion

        #region 面板回调函数
        private void FrmWeb_Load(object sender, EventArgs e)
        {
            LoadIniFile();

            txtUlr.Text = ulr;

            cmbMesFlag.SelectedIndex = 0;

            cmbSnType.SelectedIndex = 0;

            cmbCheckSn.SelectedIndex = 1;

            txtFlowGuid.Text = Guid.NewGuid().ToString();

            cmbFixMesFlag.SelectedIndex = 0;

            cmbFixType.SelectedIndex = 1;  

            cmbFixSnType.SelectedIndex = 0;

            cmbFixChkSn.SelectedIndex = 1;

            cmbMaxSlot.SelectedIndex = 15;

            txtFixFlowGuid.Text = Guid.NewGuid().ToString();

            cmbSqlType.SelectedIndex = 1;

            cmbbAlarm.SelectedIndex = 0;

            cmbFixNumSlotNo.SelectedIndex = 0;

            cmbPartType.SelectedIndex = 0; 
        }
        #endregion

        #region 面板方法
        private void ShowLog(string info, bool bAlarm)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string, bool>(ShowLog), info, bAlarm);
            else
            {
                rtbLog.Text = CLanguage.Lan(info);

                if (bAlarm)
                    rtbLog.ForeColor = Color.Red;
                else
                    rtbLog.ForeColor = Color.Blue;

                labTime.Text = CWeb2.WaitTime.ToString() + "ms";
            }
        }
        private void SetBtnCon(bool able)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<bool>(SetBtnCon), able);
            else
            {
                btnCon.Enabled = able;
            }
        }
        private void LoadIniFile()
        {
            ulr = CIniFile.ReadFromIni("Web2", "ulr", iniFile, "http://192.168.3.130/Service.asmx");
        }
        private void SaveIniFile()
        {
            CIniFile.WriteToIni("Web2", "ulr", ulr, iniFile);
        }
        #endregion

        #region 基本方法
        private void btnCon_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUlr.Text == string.Empty)
                {
                    ShowLog("请设置Web接口地址", true);
                    return;
                }

                ulr = txtUlr.Text;

                SaveIniFile();

                btnCon.Enabled = false;

                rtbLog.Clear();

                CheckSystemHandler checkSystem = new CheckSystemHandler(OnCheckSystem);

                checkSystem.BeginInvoke(null, null);

            }
            catch (Exception)
            {
                throw;
            }
        }
        private void btnVerList_Click(object sender, EventArgs e)
        {
            try
            {
                btnVerList.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                string verList = string.Empty;

                if (!CWeb2.QueryVersionRecord(out verList, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                ShowLog(er, false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnVerList.Enabled = true;
            }
        }
        private delegate void CheckSystemHandler();
        private void OnCheckSystem()
        {
            try
            {
                string version = string.Empty;

                string er = string.Empty;

                if (!CWeb2.CheckSystem(ulr, out version, out er))
                {
                    checkFlag = false;
                    ShowLog(er, true);
                    return;
                }

                ShowLog(er, false);

                checkFlag = true;
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                SetBtnCon(true);
            }
        }
        #endregion

        #region 流程功能
        private TreeNode curNode = null;
        private void RefreshFlowListView(List<CWeb2.CFlow> flowList)
        {
            try
            {
                  cmbFlowDisable.SelectedIndex = 0; 

                  FlowView.Nodes.Clear();

                  FlowView.ImageList = imageList1;

                  for (int i = 0; i < flowList.Count; i++)
                  {
                      FlowView.Nodes.Add(CLanguage.Lan("测试流程") + ":" + flowList[i].Index.ToString());
                      FlowView.Nodes[i].ImageKey = "Flow";
                      FlowView.Nodes[i].SelectedImageKey = "Flow";

                      TreeNode foldNode = FlowView.Nodes[i];

                      for (int z = 0; z < flowList[i].StatList.Count; z++)
                      {
                          string desc = flowList[i].StatList[z].Id + ":" + flowList[i].StatList[z].Name;

                          foldNode.Nodes.Add(desc);

                          if (flowList[i].StatList[z].Status == 0)
                          {
                              foldNode.Nodes[z].ImageKey = "ON";
                              foldNode.Nodes[z].SelectedImageKey = "ON";
                          }
                          else
                          {
                              foldNode.Nodes[z].ImageKey = "OFF";
                              foldNode.Nodes[z].SelectedImageKey = "OFF";
                          }
                      }
                  }

                  FlowView.ExpandAll();
                  
            }
            catch (Exception)
            {                
                throw;
            }
        }
        private List<CWeb2.CFlow> ViewChangeToFlowList()
        {
            List<CWeb2.CFlow> flowList = new List<CWeb2.CFlow>();

            try
            {
                for (int i = 0; i < FlowView.Nodes.Count; i++)
                {
                    string[] value1 = FlowView.Nodes[i].Text.Split(':');

                    CWeb2.CFlow flow = new CWeb2.CFlow();

                    flow.Index = System.Convert.ToInt16(value1[1]);

                    flow.StatList = new List<CWeb2.CSTAT>();

                    for (int z = 0; z <  FlowView.Nodes[i].Nodes.Count; z++)
                    {
                        TreeNode node = FlowView.Nodes[i].Nodes[z];

                        string[] value2 = node.Text.Split(':');

                        CWeb2.CSTAT stat = new CWeb2.CSTAT();

                        stat.Id = System.Convert.ToInt16(value2[0]);

                        stat.Name = value2[1];

                        stat.Status = (node.ImageKey == "ON" ? 0 : 1);

                        flow.StatList.Add(stat); 
                    }

                    flowList.Add(flow); 
                }

                return flowList;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private void btnBrowseFlow_Click(object sender, EventArgs e)
        {
            try
            {
                btnBrowseFlow.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                List<CWeb2.CFlow> flowList = null;

                if (!CWeb2.QueryFlowList(out flowList, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                RefreshFlowListView(flowList);

                ShowLog(er, false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnBrowseFlow.Enabled = true;
            }
        }
        private void btnSaveFlow_Click(object sender, EventArgs e)
        {
            try
            {
                btnSaveFlow.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                if (MessageBox.Show("确定要保存当前测试流程信息?", "Tip",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                List<CWeb2.CFlow> flowList = ViewChangeToFlowList();

                string er = string.Empty;

                if (!CWeb2.UpdateFlowList(flowList, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                ShowLog(er, false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnSaveFlow.Enabled = true;
            }
        }
        private void FlowView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            e.Node.ContextMenuStrip = menuFlow;

            curNode = e.Node;

            if (e.Node.ImageKey == "Flow")
            {
                SetFlowMenuShow();
            }
            else
            {
                if (e.Node.ImageKey == "ON")
                    SetStationMenuShow(1);
                else
                    SetStationMenuShow(0);
            } 
        }
        private void FlowView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.ImageKey == "Flow")
            {
                SetFlowValue();
            }
            else
            {
                if (e.Node.ImageKey == "ON")
                    SetStationValue(1);
                else
                    SetStationValue(0);
            }
        }
        private void menuDisable_Click(object sender, EventArgs e)
        {
            if (curNode.ImageKey == "ON")
            {
                curNode.ImageKey = "OFF";
                curNode.SelectedImageKey = "OFF";
            }
            else
            {
                curNode.ImageKey = "ON";
                curNode.SelectedImageKey = "ON";
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (curNode == null)
                return;

            string value = curNode.Text;

            string[] strArray = value.Split(':');

            if (curNode.ImageKey == "Flow")
            {
                int flowIndex = System.Convert.ToInt16(txtFlowIndex.Text);

                if (MessageBox.Show("确定要修改当前流程编号为【" + flowIndex.ToString() + "】?", "Tip",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                curNode.Text = CLanguage.Lan("测试流程") + ":" + flowIndex.ToString();
            }
            else
            {
                int flowId = System.Convert.ToInt16(txtFlowId.Text);

                string flowName = txtFlowName.Text;

                int disable = cmbFlowDisable.SelectedIndex;

                if (MessageBox.Show("确定要修改当前工位【" + strArray[1] + "】参数?", "Tip",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                curNode.Text = flowId.ToString() + ":" + flowName;

                if (disable == 0)
                {
                    curNode.ImageKey = "ON";
                    curNode.SelectedImageKey = "ON";
                }
                else
                {
                    curNode.ImageKey = "OFF";
                    curNode.SelectedImageKey = "OFF";
                }

            }
        }
        private void menuNew_Click(object sender, EventArgs e)
        {
            if (curNode.ImageKey == "Flow")
            {
                int count = FlowView.Nodes.Count;

                if (txtFlowIndex.Text != string.Empty)
                {
                    count = System.Convert.ToInt16(txtFlowIndex.Text);
                }

                 TreeNode node = FlowView.Nodes.Add(CLanguage.Lan("测试流程") + ":" + count.ToString());

                 node.ImageKey = "Flow";

                 node.SelectedImageKey = "Flow";
            }
        }
        private void menuNewSat_Click(object sender, EventArgs e)
        {
            int flowId = System.Convert.ToInt16(txtFlowId.Text);

            string flowName = txtFlowName.Text;

            int disable = cmbFlowDisable.SelectedIndex;

            string nodeName =flowId.ToString() + ":" + flowName;

            if (curNode.ImageKey == "Flow")
            {
                TreeNode node = curNode.Nodes.Add(nodeName);

                if (disable == 0)
                {
                    node.ImageKey = "ON";
                    node.SelectedImageKey = "ON";
                }
                else
                {
                    node.ImageKey = "OFF";
                    node.SelectedImageKey = "OFF";
                }
            }
            else
            {
                TreeNode fatherNode = curNode.Parent;

                TreeNode node = fatherNode.Nodes.Insert(curNode.Index +1, nodeName);

                if (disable == 0)
                {
                    node.ImageKey = "ON";
                    node.SelectedImageKey = "ON";
                }
                else
                {
                    node.ImageKey = "OFF";
                    node.SelectedImageKey = "OFF";
                }
            }
        }
        private void menuInsert_Click(object sender, EventArgs e)
        {
            int flowId = System.Convert.ToInt16(txtFlowId.Text);

            string flowName = txtFlowName.Text;

            int disable = cmbFlowDisable.SelectedIndex;

            string nodeName = flowId.ToString() + ":" + flowName;

            if (curNode.ImageKey != "Flow")
            {
                TreeNode fatherNode = curNode.Parent;

                TreeNode node = fatherNode.Nodes.Insert(curNode.Index, nodeName);

                if (disable == 0)
                {
                    node.ImageKey = "ON";
                    node.SelectedImageKey = "ON";
                }
                else
                {
                    node.ImageKey = "OFF";
                    node.SelectedImageKey = "OFF";
                }
            }
        }
        private void menuDelete_Click(object sender, EventArgs e)
        {
            curNode.Remove();
        }
        private void SetFlowMenuShow()
        {
            menuDisable.Enabled = false;
            menuDelete.Enabled = true;
            menuNew.Enabled = true;
            menuNewSat.Enabled = true;
            menuInsert.Enabled = false;

            lab2.Visible = true;
            lab3.Visible = true;
            lab4.Visible = true;
            txtFlowIndex.Enabled = true;
            txtFlowId.Visible = true;
            txtFlowName.Visible = true;
            cmbFlowDisable.Visible = true;
        }
        private void SetStationMenuShow(int disable)
        {
            if (disable == 0)
                menuDisable.Text = "启用(&O)";
            else
                menuDisable.Text = "禁用(&F)";
            menuDisable.Enabled = true;
            menuDelete.Enabled = true;
            menuNew.Enabled = false;
            menuNewSat.Enabled = true;
            menuInsert.Enabled = true;

            lab2.Visible = true;
            lab3.Visible = true;
            lab4.Visible = true;
            txtFlowIndex.Enabled = false;
            txtFlowId.Visible = true;
            txtFlowName.Visible = true;
            cmbFlowDisable.Visible = true;
        }
        private void SetFlowValue()
        {
            string value = curNode.Text;

            string[] strArray = value.Split(':');

            if (strArray.Length >= 2)
            {
                txtFlowIndex.Text = strArray[1];
            }
        }
        private void SetStationValue(int disable)
        {
            string value = curNode.Text;

            string[] strArray = value.Split(':');

            if (strArray.Length >= 2)
            {
                txtFlowId.Text = strArray[0];
                txtFlowName.Text = strArray[1];
            }

            if (disable == 0)
                cmbFlowDisable.SelectedIndex = 1;
            else
                cmbFlowDisable.SelectedIndex = 0;
        }
        #endregion

        #region 条码流程
        private void btnBandSn_Click(object sender, EventArgs e)
        {
            try
            {
                btnBandSn.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                CWeb2.CUUT uut = new CWeb2.CUUT();

                uut.Base.FlowIndex = System.Convert.ToInt16(txtSnFlowInde.Text);
                uut.Base.FlowName = txtSnFlowName.Text;
                uut.Base.FlowGuid = txtFlowGuid.Text;
                uut.Base.LineNo = System.Convert.ToInt16(txtLineNo.Text);
                uut.Base.LineName = txtLineName.Text;
                uut.Base.Model = txtModel.Text;
                uut.Base.OrderName = txtOrder.Text;
                uut.Base.MesFlag = cmbMesFlag.SelectedIndex;
                uut.Base.SnType = (CWeb2.ESnType)cmbSnType.SelectedIndex;

                uut.Para.SerialNo = txtSn.Text;
                uut.Para.InnerSn = txtInnerSn.Text;
                uut.Para.IdCard = txtIdCard.Text;
                uut.Para.SlotNo = System.Convert.ToInt16(txtSlotNo.Text);
                uut.Para.Remark1 = txtRemark1.Text;
                uut.Para.Remark2 = txtRemark2.Text;

                if (!CWeb2.BandSnToFlow(uut, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                ShowLog(er, false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnBandSn.Enabled = true;
            }
        }
        private void btnSnInfo_Click(object sender, EventArgs e)
        {
            try
            {
                btnSnInfo.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string serialNo = txtSn.Text;

                int snType = cmbSnType.SelectedIndex;

                if (snType != 0)
                {
                    serialNo = txtInnerSn.Text;
                }

                string er = string.Empty;

                CWeb2.CUUT uut = null;

                if (!CWeb2.GetSnFlowInfo(serialNo, out uut, out er, snType))
                {
                    ShowLog(er, true);
                    return;
                }

                txtSnFlowInde.Text = uut.Base.FlowIndex.ToString();
                txtSnFlowName.Text = uut.Base.FlowName;
                txtFlowGuid.Text = uut.Base.FlowGuid;
                txtLineNo.Text = uut.Base.LineNo.ToString();
                txtLineName.Text = uut.Base.LineName;
                txtModel.Text = uut.Base.Model;
                txtOrder.Text = uut.Base.OrderName;
                cmbMesFlag.SelectedIndex = uut.Base.MesFlag;
                cmbSnType.SelectedIndex = (int)uut.Base.SnType;

                txtSn.Text = uut.Para.SerialNo;
                txtInnerSn.Text = uut.Para.InnerSn;
                txtIdCard.Text = uut.Para.IdCard;
                txtSlotNo.Text = uut.Para.SlotNo.ToString();
                txtResult.Text = uut.Para.Result.ToString();
                txtRemark1.Text = uut.Para.Remark1;
                txtRemark2.Text = uut.Para.Remark2;

                ShowLog(er, false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnSnInfo.Enabled = true;
            }
        }
        private void btnUpdateSnResult_Click(object sender, EventArgs e)
        {
            try
            {
                btnUpdateSnResult.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                CWeb2.CUUT uut = new CWeb2.CUUT();

                uut.Base.FlowIndex = System.Convert.ToInt16(txtSnFlowInde.Text);
                uut.Base.FlowName = txtSnFlowName.Text;
                uut.Base.FlowGuid = txtFlowGuid.Text;
                uut.Base.SnType = (CWeb2.ESnType)cmbSnType.SelectedIndex;
                uut.Base.CheckSn = cmbCheckSn.SelectedIndex;

                if (uut.Base.SnType == 0)
                    uut.Para.SerialNo = txtSn.Text;
                else
                    uut.Para.SerialNo = txtInnerSn.Text;

                uut.Para.Result =System.Convert.ToInt16(txtResult.Text);
                uut.Para.TestData = txtTestData.Text;
                uut.Para.StartTime = txtStartTime.Text;
                uut.Para.EndTime = txtEndTime.Text;
                uut.Para.Remark1 = txtRemark1.Text;
                uut.Para.Remark2 = txtRemark2.Text;

                if (!CWeb2.UpdateSnResult(uut, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                ShowLog(er, false);

            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnUpdateSnResult.Enabled = true;
            }
        }
        private void btnSnRecord_Click(object sender, EventArgs e)
        {
            try
            {
                btnSnRecord.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                string serialNo = txtSn.Text;

                int snType = cmbSnType.SelectedIndex;

                if (snType != 0)
                    serialNo = txtInnerSn.Text;

                SnRecordView.Rows.Clear();
 
                CWeb2.CSn Sn = null;

                if (!CWeb2.GetSnRecord(serialNo, out Sn, out er, snType))
                {
                    labSnStatus.Text = er;
                    labSnStatus.ForeColor = Color.Red;
                    ShowLog(er, true);
                    return;
                }

                txtSn.Text = Sn.Base.SerialNo;
                txtInnerSn.Text = Sn.Base.InnerSn;
                txtLineNo.Text = Sn.Base.LineNo.ToString();
                txtLineName.Text = Sn.Base.LineName;
                txtModel.Text = Sn.Base.Model;
                txtOrder.Text = Sn.Base.OrderName;
                cmbMesFlag.SelectedIndex = Sn.Base.MesFlag;
                txtIdCard.Text = Sn.Base.IdCard;
                txtSlotNo.Text = Sn.Base.SlotNo.ToString();
                txtRemark1.Text = Sn.Base.Remark1;
                txtRemark2.Text = Sn.Base.Remark2;

                for (int i = 0; i < Sn.Para.Count; i++)
                {
                    if(Sn.Para[i].Result==0)
                    {
                        SnRecordView.Rows.Add(i + 1, Sn.Para[i].FlowName, "PASS", Sn.Para[i].TestData,
                                                  Sn.Para[i].StartTime, Sn.Para[i].EndTime, Sn.Para[i].FlowGuid);
                        SnRecordView.Rows[i].DefaultCellStyle.ForeColor = Color.Blue; 
                    }
                    else
                    {
                        SnRecordView.Rows.Add(i + 1, Sn.Para[i].FlowName, "FAIL", Sn.Para[i].TestData,
                                                      Sn.Para[i].StartTime, Sn.Para[i].EndTime, Sn.Para[i].FlowGuid);
                        SnRecordView.Rows[i].DefaultCellStyle.ForeColor = Color.Red; 
                    }                    
                }

                labSnStatus.Text = "条码【" + serialNo + "】查询OK【" + Sn.Para.Count.ToString() + "】";
                labSnStatus.ForeColor = Color.Blue;

                ShowLog(er, false);

            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnSnRecord.Enabled = true;
            }
        }
        private void btnSnRework_Click(object sender, EventArgs e)
        {
            try
            {
                btnBandSn.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                CWeb2.CUUT uut = new CWeb2.CUUT();

                uut.Base.FlowIndex = System.Convert.ToInt16(txtSnFlowInde.Text);
                uut.Base.FlowName = txtSnFlowName.Text;
                uut.Base.FlowGuid = txtFlowGuid.Text;
                uut.Base.SnType = (CWeb2.ESnType)cmbSnType.SelectedIndex;

                uut.Para.SerialNo = txtSn.Text;
                uut.Para.InnerSn = txtInnerSn.Text;
                uut.Para.IdCard = txtIdCard.Text;
                uut.Para.SlotNo = System.Convert.ToInt16(txtSlotNo.Text);
                uut.Para.Remark1 = txtRemark1.Text;
                uut.Para.Remark2 = txtRemark2.Text;

                if (!CWeb2.ReworkSnFlow(uut, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                ShowLog(er, false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnBandSn.Enabled = true;
            }
        }
        private void btnChkSn_Click(object sender, EventArgs e)
        {
            try
            {
                btnChkSn.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                int SnType = cmbSnType.SelectedIndex;

                string serialNo = string.Empty;

                if (SnType == 0)
                {
                    serialNo = txtSn.Text;
                }
                else
                {
                    serialNo = txtInnerSn.Text;
                }

                if (!CWeb2.CheckSn(txtSnFlowName.Text, serialNo, out er, SnType))
                {
                    ShowLog(er, true);
                    return;
                }

                ShowLog(er, false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnChkSn.Enabled = true;
            }
        }
        #endregion

        #region 内外条码匹配
        private void btnSnMapping_Click(object sender, EventArgs e)
        {
            try
            {
                btnSnMapping.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                CWeb2.CMap map = new CWeb2.CMap();

                map.Para.Add(new CWeb2.CMap_Para()
                             {
                                 SerialNo = txtSn.Text,
                                 InnerSn = txtInnerSn.Text
                             });

                if (!CWeb2.SerialNoMappingInnerSn(map, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                ShowLog(er, false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnSnMapping.Enabled = true;
            }
        }
        #endregion

        #region 治具管控
        private int fixtureIndex = -1;
        private void FixtureView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            fixtureIndex = e.RowIndex;
            labFixIndex.Text = fixtureIndex.ToString();
        }
        private void menuFixInsert_Click(object sender, EventArgs e)
        {
            if (fixtureIndex == -1)
                return;
            FixtureView.Rows.Insert(fixtureIndex, 1); 
        }
        private void menuFixAdd_Click(object sender, EventArgs e)
        {
            if (fixtureIndex == -1)
                return;
            FixtureView.Rows.Insert(fixtureIndex + 1, 1);
        }
        private void menuFixDelete_Click(object sender, EventArgs e)
        {
            if (fixtureIndex == -1)
                return;
            FixtureView.Rows.RemoveAt(fixtureIndex); 
        }
        private void cmbMaxSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            FixtureView.Rows.Clear();

            string guid = Guid.NewGuid().ToString();

            string[] array = guid.Split('-');

            string serialNo = array[array.Length-1].ToUpper();

            string innerSn = array[0].ToUpper();

            string sNowTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            for (int i = 0; i <= cmbMaxSlot.SelectedIndex; i++)
            {
                string s1 =  serialNo + (i+1).ToString("D2");

                string s2 = innerSn + (i+1).ToString("D2");

                FixtureView.Rows.Add(i, s1, s2, txtFixFlowName.Text, "PASS", "", "", guid,"",sNowTime,sNowTime);
            }
        }
        private void btnBandFix_Click(object sender, EventArgs e)
        {
            try
            {
                btnBandFix.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                CWeb2.CFixture fixture = new CWeb2.CFixture();

                fixture.Base.FlowIndex = System.Convert.ToInt16(txtFixFlowIndex.Text);
                fixture.Base.FlowName = txtFixFlowName.Text;
                fixture.Base.FlowGuid = txtFixFlowGuid.Text;
                fixture.Base.LineNo = System.Convert.ToInt16(txtFixLineNo.Text);
                fixture.Base.LineName = txtFixLineName.Text;
                fixture.Base.Model = txtFixModel.Text;
                fixture.Base.OrderName = txtFixOrder.Text;
                fixture.Base.MesFlag = cmbFixMesFlag.SelectedIndex;
                fixture.Base.SnType = (CWeb2.ESnType)cmbFixSnType.SelectedIndex;
                fixture.Base.MaxSlot = cmbMaxSlot.SelectedIndex + 1;
                fixture.Base.IdCard = txtFixIdCard.Text;
                fixture.Base.FixtureType = (CWeb2.EFixtureType)(cmbFixType.SelectedIndex - 1);
                fixture.Base.CheckSn = cmbFixChkSn.SelectedIndex;

                for (int i = 0; i < FixtureView.Rows.Count - 1; i++)
                {
                    CWeb2.CFix_Para para = new CWeb2.CFix_Para();
                    if (FixtureView.Rows[i].Cells[0].Value != null)
                        para.SlotNo = System.Convert.ToInt16(FixtureView.Rows[i].Cells[0].Value.ToString());
                    if (FixtureView.Rows[i].Cells[1].Value != null)
                        para.SerialNo = FixtureView.Rows[i].Cells[1].Value.ToString();
                    if (FixtureView.Rows[i].Cells[2].Value != null)
                        para.InnerSn = FixtureView.Rows[i].Cells[2].Value.ToString();
                    if (FixtureView.Rows[i].Cells[5].Value != null)
                        para.Remark1 = FixtureView.Rows[i].Cells[5].Value.ToString();
                    if (FixtureView.Rows[i].Cells[6].Value != null)
                        para.Remark2 = FixtureView.Rows[i].Cells[6].Value.ToString();
                    fixture.Para.Add(para); 
                }

                if (!CWeb2.BandSnToFixture(fixture, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                ShowLog(er, false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnBandFix.Enabled = true;
            }
        }
        private void btnReworkFix_Click(object sender, EventArgs e)
        {
            try
            {
                btnReworkFix.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                CWeb2.CFixture fixture = new CWeb2.CFixture();

                fixture.Base.FlowIndex = System.Convert.ToInt16(txtFixFlowIndex.Text);
                fixture.Base.LineNo = System.Convert.ToInt16(txtFixLineNo.Text);
                fixture.Base.LineName = txtFixLineName.Text;
                fixture.Base.Model = txtFixModel.Text;
                fixture.Base.OrderName = txtFixOrder.Text;
                fixture.Base.MesFlag = cmbFixMesFlag.SelectedIndex;
                fixture.Base.FixtureType = (CWeb2.EFixtureType)(cmbFixType.SelectedIndex - 1);
                fixture.Base.SnType = (CWeb2.ESnType)cmbFixSnType.SelectedIndex;
                fixture.Base.MaxSlot = cmbMaxSlot.SelectedIndex + 1;
                fixture.Base.IdCard = txtFixIdCard.Text;

                for (int i = 0; i < FixtureView.Rows.Count - 1; i++)
                {
                    CWeb2.CFix_Para para = new CWeb2.CFix_Para();
                    if (FixtureView.Rows[i].Cells[0].Value!=null)
                        para.SlotNo = System.Convert.ToInt16(FixtureView.Rows[i].Cells[0].Value.ToString());
                    if(FixtureView.Rows[i].Cells[1].Value!=null)
                        para.SerialNo = FixtureView.Rows[i].Cells[1].Value.ToString();
                    if(FixtureView.Rows[i].Cells[2].Value!=null)
                        para.InnerSn = FixtureView.Rows[i].Cells[2].Value.ToString();
                    if (FixtureView.Rows[i].Cells[4].Value != null)
                        para.Result = (FixtureView.Rows[i].Cells[4].Value.ToString() == "PASS" ? 0 : 1);
                    if(FixtureView.Rows[i].Cells[5].Value!=null)
                       para.Remark1 = FixtureView.Rows[i].Cells[5].Value.ToString();
                    if(FixtureView.Rows[i].Cells[6].Value!=null)
                       para.Remark2 = FixtureView.Rows[i].Cells[6].Value.ToString();
                    if (FixtureView.Rows[i].Cells[3].Value!=null)
                        para.FlowName = FixtureView.Rows[i].Cells[3].Value.ToString();
                    if (FixtureView.Rows[i].Cells[7].Value != null)
                        para.FlowGuid = FixtureView.Rows[i].Cells[7].Value.ToString();
                    fixture.Para.Add(para);
                }

                if (!CWeb2.RegroupSnToFixture(fixture, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                ShowLog(er, false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnReworkFix.Enabled = true;
            }
        }
        private void btnFixInfo_Click(object sender, EventArgs e)
        {
            try
            {
                btnFixInfo.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string idCard = txtFixIdCard.Text;

                string er = string.Empty;

                //CWeb2.CFix_Base fix = null;

                //if (!CWeb2.GetFixtureInfo(idCard, out fix, out er))
                //{
                //   ShowLog(er, true);
                //   return;
                //}

                CWeb2.CFixture fixture = null;

                if (!CWeb2.GetFixtureInfoByIdCard(idCard, out fixture, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                txtFixFlowIndex.Text = fixture.Base.FlowIndex.ToString();
                txtFixFlowName.Text = fixture.Base.FlowName;
                txtFixFlowGuid.Text = fixture.Base.FlowGuid;
                txtFixLineNo.Text = fixture.Base.LineNo.ToString();
                txtFixLineName.Text = fixture.Base.LineName;
                txtFixModel.Text = fixture.Base.Model;
                txtFixOrder.Text = fixture.Base.OrderName;
                cmbFixMesFlag.SelectedIndex = fixture.Base.MesFlag;
                cmbFixSnType.SelectedIndex = (int)fixture.Base.SnType;
                cmbFixType.SelectedIndex = (int)fixture.Base.FixtureType + 1;

                FixtureView.Rows.Clear();

                for (int i = 0; i < fixture.Para.Count; i++)
                {
                    FixtureView.Rows.Add(fixture.Para[i].SlotNo,
                                         fixture.Para[i].SerialNo, fixture.Para[i].InnerSn,
                                         fixture.Para[i].FlowName, fixture.Para[i].Result == 0 ? "PASS" : "FAIL",
                                         fixture.Para[i].Remark1, fixture.Para[i].Remark2,
                                         fixture.Para[i].FlowGuid,"","",""
                                         );
                }

                ShowLog(er, false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnFixInfo.Enabled = true;
            }
        }
        private void btnUpdateFixture_Click(object sender, EventArgs e)
        {
            try
            {
                btnUpdateFixture.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                CWeb2.CFixture fixture = new CWeb2.CFixture();

                fixture.Base.FlowIndex = System.Convert.ToInt16(txtFixFlowIndex.Text);
                fixture.Base.FlowName = txtFixFlowName.Text;
                fixture.Base.FlowGuid = txtFixFlowGuid.Text;               
                fixture.Base.SnType = (CWeb2.ESnType)cmbFixSnType.SelectedIndex;
                fixture.Base.IdCard = txtFixIdCard.Text;
                fixture.Base.CheckSn = cmbFixChkSn.SelectedIndex;

                for (int i = 0; i < FixtureView.Rows.Count - 1; i++)
                {
                    CWeb2.CFix_Para para = new CWeb2.CFix_Para();
                    if (FixtureView.Rows[i].Cells[0].Value != null)
                        para.SlotNo = System.Convert.ToInt16(FixtureView.Rows[i].Cells[0].Value.ToString());
                    if (FixtureView.Rows[i].Cells[1].Value != null)
                        para.SerialNo = FixtureView.Rows[i].Cells[1].Value.ToString();
                    if (FixtureView.Rows[i].Cells[2].Value != null)
                        para.InnerSn = FixtureView.Rows[i].Cells[2].Value.ToString();
                    if (FixtureView.Rows[i].Cells[4].Value != null)
                        para.Result = (FixtureView.Rows[i].Cells[4].Value.ToString()=="PASS"?0:1);
                    if (FixtureView.Rows[i].Cells[5].Value != null)
                        para.Remark1 = FixtureView.Rows[i].Cells[5].Value.ToString();
                    if (FixtureView.Rows[i].Cells[6].Value != null)
                        para.Remark2 = FixtureView.Rows[i].Cells[6].Value.ToString();
                    if (FixtureView.Rows[i].Cells[8].Value != null)
                        para.TestData = FixtureView.Rows[i].Cells[8].Value.ToString();
                    if (FixtureView.Rows[i].Cells[9].Value != null)
                        para.StartTime = FixtureView.Rows[i].Cells[9].Value.ToString();
                    if (FixtureView.Rows[i].Cells[10].Value != null)
                        para.EndTime = FixtureView.Rows[i].Cells[10].Value.ToString();
                    fixture.Para.Add(para);
                }

                if (!CWeb2.UpdateFixtureResult(fixture, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                ShowLog(er, false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnUpdateFixture.Enabled = true;
            }
        }
        #endregion

        #region 治具查询
        private void btnFixNumQuery_Click(object sender, EventArgs e)
        {
            try
            {
                btnFixNumQuery.Enabled = false;

                string er = string.Empty;

                 if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                FixNumView.Rows.Clear();

                CWeb2.CFixCondition condition = new CWeb2.CFixCondition()
                {
                     FlowIndex=0,
                     FlowName = txtFixNumFlowName.Text,
                     IdCard = txtFixNumIdCard.Text,
                     SlotNo = cmbFixNumSlotNo.SelectedIndex - 1
                };

                List<CWeb2.CFixUseNum> idCardList=null;

                if (!CWeb2.QueryIdCardUseNum(condition, out idCardList, out er))
                {
                    labT3.Text = er;
                    labT3.ForeColor = Color.Red;
                    return;
                }

                for (int i = 0; i < idCardList.Count; i++)
                {
                    FixNumView.Rows.Add(i + 1, idCardList[i].FlowName, idCardList[i].IdCard, idCardList[i].SlotNo,
                                       idCardList[i].TTNum, idCardList[i].FailNum, idCardList[i].ConFailNum);
                }

            }
            catch (Exception ex)
            {
                labT3.Text = ex.ToString();
                labT3.ForeColor = Color.Red;
            }
            finally
            {
                btnFixNumQuery.Enabled = true;
            }
        }
        #endregion

        #region 生产统计
        private void btnQueryStat_Click(object sender, EventArgs e)
        {
            try
            {
                btnQueryStat.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                CWeb2.CYield_Base input = new CWeb2.CYield_Base();

                input.StartTime = dpYieldStartTime.Value.Date.ToString("yyyy/MM/dd");
                input.EndTime = dpYieldEndTime.Value.Date.ToString("yyyy/MM/dd");
                input.FlowIndex = System.Convert.ToInt16(txtYieldFlowIndex.Text);
                input.FlowName = txtYieldFlowName.Text;
                input.FlowGuid = txtYieldFlowGuid.Text;
                if (txtYieldLineNo.Text == "")
                    input.LineNo = -1;
                else
                    input.LineNo = System.Convert.ToInt16(txtYieldLineNo.Text);
                input.LineName = txtYieldLineName.Text;
                input.OrderName = txtYieldOrder.Text;
                input.Model = txtYieldModel.Text;

                List<CWeb2.CYield_Para> output = null;

                if (!CWeb2.QueryProductivity(input, out output, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                YieldView.Rows.Clear();

                for (int i = 0; i < output.Count; i++)
                {
                    YieldView.Rows.Add(output[i].IdNo, output[i].Name, output[i].TTNum, output[i].FailNum);
                }

                ShowLog(er, false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnQueryStat.Enabled = true;
            }
        }      
        #endregion

        #region 报警记录
        private void btnQueryAlarmList_Click(object sender, EventArgs e)
        {
            try
            {
                btnQueryAlarmList.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                List<CWeb2.CAlarmRecord> alarmList = null;

                CWeb2.CAlarm_Base condition = new CWeb2.CAlarm_Base();

                condition.StartTime = dpAlarmStartDate.Value.Date.ToString("yyyy/MM/dd");

                condition.EndTime = dpAlarmEndDate.Value.Date.ToString("yyyy/MM/dd");

                condition.StatName = txtAlarmStatName.Text;

                condition.StatGuid = txtAlarmStatGuid.Text;

                condition.ErrNo = System.Convert.ToInt32(txtAlarmCode.Text);

                condition.bAlarm = cmbbAlarm.SelectedIndex - 1;

                if (!CWeb2.GetAlarmRecord(condition, out alarmList, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                DataTable dt = new DataTable();

                dt.Columns.Add("编号");
                dt.Columns.Add("工位名称");
                dt.Columns.Add("工位标识");
                dt.Columns.Add("报警状态");
                dt.Columns.Add("报警代号");
                dt.Columns.Add("报警时间");
                dt.Columns.Add("报警信息");
                dt.Columns.Add("备注1");
                dt.Columns.Add("备注2");

                for (int i = 0; i < alarmList.Count; i++)
                {
                    dt.Rows.Add(i + 1,
                                alarmList[i].StatName, alarmList[i].StatGuid,
                                (alarmList[i].bAlarm == 1 ? "报警" : "解除"), alarmList[i].ErrNo,
                                 alarmList[i].HappenTime,alarmList[i].AlarmInfo,
                                alarmList[i].Remark1, alarmList[i].Remark2
                                );
                }

                AlarmView.DataSource = dt;

                ShowLog(er, false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnQueryAlarmList.Enabled = true; 
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                btnExport.Enabled = false;

                if (AlarmView.RowCount == 0)
                    return;

                SaveFileDialog saveFiledlg = new SaveFileDialog();
                saveFiledlg.InitialDirectory = Application.StartupPath;
                saveFiledlg.Filter = "csv files (*.csv)|*.csv";
                if (saveFiledlg.ShowDialog() != DialogResult.OK)
                    return;
                string filePath = saveFiledlg.FileName;
                StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8);
                string strWrite = string.Empty;

                strWrite += "编号,工位名称,工位标识,报警状态,报警代号,报警时间,报警信息,备注1,备注2";
                sw.WriteLine(strWrite);
                for (int i = 0; i < AlarmView.Rows.Count; i++)
                {
                    strWrite = string.Empty;
                    strWrite += AlarmView.Rows[i].Cells[0].Value.ToString() + ",";
                    strWrite += AlarmView.Rows[i].Cells[1].Value.ToString() + ",";
                    strWrite += AlarmView.Rows[i].Cells[2].Value.ToString() + ",";
                    strWrite += AlarmView.Rows[i].Cells[3].Value.ToString() + ",";
                    strWrite += AlarmView.Rows[i].Cells[4].Value.ToString() + ",";
                    strWrite += AlarmView.Rows[i].Cells[5].Value.ToString() + ",";
                    strWrite += AlarmView.Rows[i].Cells[6].Value.ToString() + ",";
                    strWrite += AlarmView.Rows[i].Cells[7].Value.ToString() + ",";
                    strWrite += AlarmView.Rows[i].Cells[8].Value.ToString() + ",";
                    sw.WriteLine(strWrite);
                }

                sw.Flush();
                sw.Close();
                sw = null;
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnExport.Enabled = true;
            }           
        }
        #endregion

        #region 易损件记录
        private void btnPartQuery_Click(object sender, EventArgs e)
        {
            try
            {
                btnPartQuery.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                PartView.Rows.Clear();
                
                CWeb2.CPartCondition conditon = new CWeb2.CPartCondition();
                conditon.StartTime = dpPartStartDate.Value.Date.ToString("yyyy/MM/dd");
                conditon.EndTime = dpPartEndDate.Value.Date.ToString("yyyy/MM/dd");
                conditon.PartType = (CWeb2.EPartType)cmbPartType.SelectedIndex;
                conditon.PartName = txtPartName.Text;
                conditon.PartSlotNo = System.Convert.ToInt16(txtPartSlotNo.Text);
                conditon.TTNum = System.Convert.ToInt32(txtPartTTNum.Text);
                conditon.FailNum = System.Convert.ToInt32(txtPartFailNum.Text);
                conditon.ConFailNum = System.Convert.ToInt32(txtPartConFailNum.Text);

                List<CWeb2.CPartRecord> partRecord = null;

                if (!CWeb2.QueryFailPartRecord(conditon, out partRecord, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                for (int i = 0; i < partRecord.Count; i++)
                {
                    PartView.Rows.Add(
                                      i + 1, partRecord[i].PartName,
                                      partRecord[i].PartSlotNo, partRecord[i].PartCarrier,
                                      partRecord[i].LocalName, partRecord[i].TTNum,
                                      partRecord[i].FailNum, partRecord[i].ConFailNum,
                                      partRecord[i].AlarmTime
                                      );
                }
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnPartQuery.Enabled = true;
            }
        }
        private void btnPartExport_Click(object sender, EventArgs e)
        {
            try
            {
                btnPartExport.Enabled = false;

                SaveFileDialog saveFiledlg = new SaveFileDialog();
                saveFiledlg.InitialDirectory = Application.StartupPath;
                saveFiledlg.Filter = "csv files (*.csv)|*.csv";
                if (saveFiledlg.ShowDialog() != DialogResult.OK)
                    return;
                string filePath = saveFiledlg.FileName;
                StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8);
                string strWrite = string.Empty;

                strWrite += "编号,易损件名称,槽位号,当前位置,机型条码,测试次数,累计不良次数,连续不良次数,不良时间";
                sw.WriteLine(strWrite);
                for (int i = 0; i < PartView.Rows.Count; i++)
                {
                    strWrite = string.Empty;
                    strWrite += PartView.Rows[i].Cells[0].Value.ToString() + ",";
                    strWrite += PartView.Rows[i].Cells[1].Value.ToString() + ",";
                    strWrite += PartView.Rows[i].Cells[2].Value.ToString() + ",";
                    strWrite += PartView.Rows[i].Cells[3].Value.ToString() + ",";
                    strWrite += PartView.Rows[i].Cells[4].Value.ToString() + ",";
                    strWrite += PartView.Rows[i].Cells[5].Value.ToString() + ",";
                    strWrite += PartView.Rows[i].Cells[6].Value.ToString() + ",";
                    strWrite += PartView.Rows[i].Cells[7].Value.ToString() + ",";
                    strWrite += PartView.Rows[i].Cells[8].Value.ToString();
                    sw.WriteLine(strWrite);
                }

                sw.Flush();
                sw.Close();
                sw = null;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                btnPartExport.Enabled = true;
            }
        }
        #endregion

        #region 机型信息
        private void btnModelAdd_Click(object sender, EventArgs e)
        {
            try
            {
                btnModelAdd.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                CWeb2.CModel model =new CWeb2.CModel();

                model.Base.LineNo = System.Convert.ToInt16(txtModelLineNo.Text);
                model.Base.LineName = txtModelLineName.Text;
                model.Base.StationName = txtModelStation.Text;

                model.Para.Add(new CWeb2.CModel_Para()
                              {
                               ModelName = txtModelName.Text,
                               ModelSn  = txtModelSn.Text,
                               ModelPara = rtbModelPara.Text
                              }
                              );

                if (!CWeb2.InsertModelInfo(model, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                ShowLog("更新机型信息OK", false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnModelAdd.Enabled = true;
            }
        }
        private void btnModelQuery_Click(object sender, EventArgs e)
        {
            try
            {
                btnModelQuery.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                bool IsExist = false;
                CWeb2.CModel_Condition condition = new CWeb2.CModel_Condition();
                condition.LineNo = System.Convert.ToInt16(txtModelLineNo.Text);
                condition.LineName = txtModelLineName.Text;
                condition.StationName = txtModelStation.Text;
                condition.ModelName = txtModelName.Text;
                condition.ModelSn = txtModelSn.Text;
                condition.Status = System.Convert.ToInt16(txtModelStatus.Text);

                CWeb2.CModel model = null;

                if (!CWeb2.GetModelInfo(condition,out IsExist,out model, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                if (!IsExist)
                {
                    ShowLog("查询不到机型信息", true);
                    return;
                }

                rtbModelPara.Text = model.Para[0].ModelPara; 

                ShowLog("获取机型信息OK", false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnModelQuery.Enabled = true;
            }
        }
        private void btnModelUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                btnModelUpdate.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;
                CWeb2.CModel_Condition condition = new CWeb2.CModel_Condition();
                condition.LineNo = System.Convert.ToInt16(txtModelLineNo.Text);
                condition.LineName = txtModelLineName.Text;
                condition.StationName = txtModelStation.Text;
                condition.ModelName = txtModelName.Text;
                condition.ModelSn = txtModelSn.Text;
                condition.Status = System.Convert.ToInt16(txtModelStatus.Text);

                if (!CWeb2.UpdateModelInfoStatus(condition,out er))
                {
                    ShowLog(er, true);
                    return;
                }

                ShowLog("更新机型信息状态OK", false);
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                btnModelUpdate.Enabled = true;
            }
        }
        #endregion

        #region 执行SQL语句
        private void BtnSqlExecute_Click(object sender, EventArgs e)
        {
            try
            {
                BtnSqlExecute.Enabled = false;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }

                string er = string.Empty;

                CWeb2.ESqlCmdType sqlCmdType = (CWeb2.ESqlCmdType)cmbSqlType.SelectedIndex;

                List<string> cmdList = new List<string>();

                cmdList.Add(txtSqlCmd.Text);

                List<DataTable> dt = null;

                if (!CWeb2.ExcuteSQLCmd(sqlCmdType, cmdList, out dt, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                if (sqlCmdType == CWeb2.ESqlCmdType.ExecuteQuery)
                {
                    sqlTableView.DataSource = dt[0];
                }

                ShowLog(er, false); 
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                BtnSqlExecute.Enabled = true;
            }
        }
        #endregion

      

    }
}
