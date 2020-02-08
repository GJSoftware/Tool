using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GJ.COM;
using GJ.PDB;
using System.IO;
using System.Diagnostics;
using GJ.PLUGINS;
using GJ.MES;

namespace GJ.WndCom
{
    public partial class FrmSample : Form, IChildMsg
    {
        #region 插件方法
        /// <summary>
        /// 父窗口
        /// </summary>
        private Form _father = null;
        /// <summary>
        /// 父窗口唯一标识
        /// </summary>
        private string _fatherGuid = string.Empty;
        /// <summary>
        /// 加载当前窗口及软件版本日期
        /// </summary>
        /// <param name="fatherForm"></param>
        /// <param name="control"></param>
        /// <param name="guid"></param>
        public void OnShowDlg(Form fatherForm, Control control, string guid)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<Form, Control, string>(OnShowDlg), fatherForm, control, guid);
            else
            {
                this._father = fatherForm;
                this._fatherGuid = guid;

                this.Dock = DockStyle.Fill;
                this.TopLevel = false;
                this.FormBorderStyle = FormBorderStyle.None;
                control.Controls.Add(this);
                this.Show();
            }
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
        /// 启动监控
        /// </summary>
        public void OnStartRun()
        {


        }
        /// <summary>
        /// 停止监控
        /// </summary>
        public void OnStopRun()
        {


        }
        /// <summary>
        /// 中英文切换
        /// </summary>
        public void OnChangeLAN()
        {
            SetUILanguage();
        }
        /// <summary>
        /// 消息响应
        /// </summary>
        /// <param name="para"></param>
        public void OnMessage(string name, int lPara, int wPara)
        {

        }
        #endregion

        #region 语言设置
        /// <summary>
        /// 设置中英文界面
        /// </summary>
        private void SetUILanguage()
        {
            GJ.COM.CLanguage.SetLanguage(this);

            switch (GJ.COM.CLanguage.languageType)
            {
                case CLanguage.EL.中文:
                    break;
                case CLanguage.EL.英语:
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 构造函数
        public FrmSample()
        {
            InitializeComponent();

            IntialControl();

            SetDoubleBuffered();
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 绑定控件
        /// </summary>
        private void IntialControl()
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
        /// web检查状态
        /// </summary>
        private bool web_check_flag = false;
        private SaveFileDialog saveFiledlg = new SaveFileDialog();
        private string iniFile = Application.StartupPath + "\\iniFile.ini";
        private string url = "http://192.168.3.130/Service.asmx";
        #endregion

        #region 面板回调函数
        private void FrmSample_Load(object sender, EventArgs e)
        {
            cmbResult.SelectedIndex = 0;

            cmbPartType.Items.Clear();
            cmbPartType.Items.Add(CLanguage.Lan("治具"));
            cmbPartType.Items.Add(CLanguage.Lan("库位"));
            cmbPartType.SelectedIndex = 0;

            txtUrl.Text = CIniFile.ReadFromIni("FrmSample", "url", iniFile, url);

            url = txtUrl.Text;

            check_web_status_handler check_web = new check_web_status_handler(check_web_status);

            check_web.BeginInvoke(null, null); 
        }
        private void btnQuerySn_Click(object sender, EventArgs e)
        {
            try
            {
                btnQuerySn.Enabled = false;

                string er = string.Empty;

                if (!web_check_flag)
                {
                    labStatus.Text = CLanguage.Lan("无法连接") + "[" + url + "]";
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                CWeb2.CSn_Query condition = new CWeb2.CSn_Query();

                condition.StartTime = dpStartDate.Value.ToString("yyyy/MM/dd") + " 00:00:00";

                condition.EndTime = dpEndDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";

                condition.FlowName = txtFlowName.Text;

                condition.SerialNo = txtSn.Text;

                condition.Result = cmbResult.SelectedIndex - 1;

                List<CWeb2.CSn_Para> SnList = null;

                if (!CWeb2.QuerySnRecord(condition, out SnList, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                DataTable dt = new DataTable();

                dt.Columns.Add(CLanguage.Lan("编号"));
                dt.Columns.Add(CLanguage.Lan("产品条码"));
                dt.Columns.Add(CLanguage.Lan("工位名称"));                
                dt.Columns.Add(CLanguage.Lan("测试结果"));
                dt.Columns.Add(CLanguage.Lan("开始时间"));
                dt.Columns.Add(CLanguage.Lan("结束时间"));
                dt.Columns.Add(CLanguage.Lan("测试数据"));
                dt.Columns.Add(CLanguage.Lan("治具RFID"));
                dt.Columns.Add(CLanguage.Lan("备注") + "1");
                dt.Columns.Add(CLanguage.Lan("备注") + "2");

                for (int i = 0; i < SnList.Count; i++)
                {
                    dt.Rows.Add(i + 1, SnList[i].SerialNo,
                                SnList[i].FlowName, SnList[i].Result==0?"PASS":"FAIL",
                                SnList[i].StartTime, SnList[i].EndTime,
                                SnList[i].TestData, SnList[i].IdCard + "-" + SnList[i].SlotNo.ToString(),
                                SnList[i].Remark1, SnList[i].Remark2
                                );
                }

                SnView.DataSource = dt;

                labStatus.Text = CLanguage.Lan("查询数量") + ":【" + SnList.Count.ToString() + "】";

                labStatus.ForeColor = Color.Blue;

            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnQuerySn.Enabled = true;
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                btnExport.Enabled = false;

                saveFiledlg.InitialDirectory = Application.StartupPath;
                saveFiledlg.Filter = "csv files (*.csv)|*.csv";
                if (saveFiledlg.ShowDialog() != DialogResult.OK)
                    return;
                string filePath = saveFiledlg.FileName;
                StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8);
                string strWrite = string.Empty;
                strWrite = "编号,产品条码,工位名称,测试结果,开始时间,结束时间,测试数据,治具RFID,备注1,备注2";
                sw.WriteLine(strWrite);
                for (int i = 0; i < SnView.Rows.Count; i++)
                {
                    strWrite = string.Empty;
                    strWrite += SnView.Rows[i].Cells[0].Value.ToString() + ",";
                    strWrite += SnView.Rows[i].Cells[1].Value.ToString() + ",";
                    strWrite += SnView.Rows[i].Cells[2].Value.ToString() + ",";
                    strWrite += SnView.Rows[i].Cells[3].Value.ToString() + ",";
                    strWrite += SnView.Rows[i].Cells[4].Value.ToString() + ",";
                    strWrite += SnView.Rows[i].Cells[5].Value.ToString() + ",";
                    strWrite += SnView.Rows[i].Cells[6].Value.ToString() + ",";
                    strWrite += SnView.Rows[i].Cells[7].Value.ToString() + ",";
                    strWrite += SnView.Rows[i].Cells[8].Value.ToString() + ",";
                    strWrite += SnView.Rows[i].Cells[9].Value.ToString();
                    sw.WriteLine(strWrite);
                }
                sw.Flush();
                sw.Close();
                sw = null;
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnExport.Enabled = true;
            }
        }
        private void SnView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string Sn = SnView.Rows[e.RowIndex].Cells[1].Value.ToString();

                string ReportPath = SnView.Rows[e.RowIndex].Cells[10].Value.ToString();

                if (MessageBox.Show(CLanguage.Lan("确定要打开机型测试报表")+ "SN[" + Sn + "]?", "Tip", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                if (!File.Exists(ReportPath))
                {
                    MessageBox.Show(CLanguage.Lan("找不到测试报表") + "[" + ReportPath + "],"+ CLanguage.Lan("请检查是否删除?"), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  
                    return;
                }

                Process.Start(ReportPath);
            }
        }
        private void btnPartQuery_Click(object sender, EventArgs e)
        {
            try
            {
                btnPartQuery.Enabled = false;

                if (!web_check_flag)
                {
                    labStatus.Text = CLanguage.Lan("无法连接") + "[" + url + "]";
                    labStatus.ForeColor = Color.Red;
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
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
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
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
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
        private void btnSave_Click(object sender, EventArgs e)
        {
            url = txtUrl.Text;

            CIniFile.WriteToIni("FrmSample", "url", url, iniFile);
        }
        #endregion      

        #region 委托
        private delegate void check_web_status_handler();
        /// <summary>
        /// 检查web状态
        /// </summary>
        private void check_web_status()
        {
            try
            {
                string er = string.Empty;

                string ver = string.Empty;

                if (!CWeb2.CheckSystem(url, out ver, out er))
                    return;

                web_check_flag = true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion     



    }
}
