using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using GJ.MES;
using GJ.PLUGINS;
using GJ.COM;
using GJ.PDB;

namespace GJ.WndCom
{

    public partial class FrmSnQuery : Form,IChildMsg
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
        public FrmSnQuery()
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
        private string sysDB = Application.StartupPath + "\\BURIN\\BURNIN.accdb";
        #endregion

        #region 面板回调函数
        private void FrmYield_Load(object sender, EventArgs e)
        {
            dpStartDate.Value = DateTime.Now.Date;

            dpEndDate.Value = DateTime.Now.Date;

            txtUrl.Text = CIniFile.ReadFromIni("FrmSnQuery", "url", iniFile, url);

            labSysDB.Text = CIniFile.ReadFromIni("FrmSnQuery", "sysDB", iniFile, sysDB);

            url = txtUrl.Text;

            sysDB = labSysDB.Text;

            check_web_status_handler check_web = new check_web_status_handler(check_web_status);

            check_web.BeginInvoke(null, null); 
        }
        private void btnQueryFailSn_Click(object sender, EventArgs e)
        {
            try
            {
                btnQueryFailSn.Enabled = false;

                snFailView.Rows.Clear();

                string er = string.Empty;

                string startDate = dpStartDate.Value.ToString("yyyy/MM/dd") + " 00:00:00";

                string endDate = dpEndDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";

                CDBCOM db = new CDBCOM(EDBType.Access, "", sysDB);

                DataSet ds = null;

                string sqlCmd = string.Empty;

                if (txtFailSn.Text == string.Empty)
                {
                    sqlCmd = string.Format("select * from FailRecord where StartTime >= '{0}' and StartTime <='{1}'" +
                                           " order by StartTime desc,IdCard,SlotNo", startDate, endDate);
                }
                else
                {
                    sqlCmd = string.Format("select * from FailRecord where  StartTime >= '{0}' and StartTime <='{1}' and SerialNo='{2}'",
                                         startDate,endDate, txtFailSn.Text);
                }

                if (!db.QuerySQL(sqlCmd, out ds, out er))
                {
                    labFailStatus.Text = er;
                    labFailStatus.ForeColor = Color.Red;
                    return;
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string serialNo = ds.Tables[0].Rows[i]["SerialNo"].ToString();

                    string fixName = ds.Tables[0].Rows[i]["IdCard"].ToString() + "-" +
                                     System.Convert.ToInt16(ds.Tables[0].Rows[i]["SlotNo"].ToString()).ToString("D2");

                    string localName = ds.Tables[0].Rows[i]["LocalName"].ToString();

                    string startTime = ds.Tables[0].Rows[i]["StartTime"].ToString();

                    string endTime = ds.Tables[0].Rows[i]["EndTime"].ToString();

                    string failInfo = ds.Tables[0].Rows[i]["FailInfo"].ToString();

                    string failTime = ds.Tables[0].Rows[i]["FailTime"].ToString();

                    string filePath = ds.Tables[0].Rows[i]["ReportPath"].ToString();

                    snFailView.Rows.Add(serialNo, fixName, localName, startTime, endTime, failInfo, failTime, filePath);
                }

                labFailStatus.Text = CLanguage.Lan("查询不良数量") + ":" + "【" + ds.Tables[0].Rows.Count.ToString() + "】";

                labFailStatus.ForeColor = Color.Blue;

            }
            catch (Exception ex)
            {
                labFailStatus.Text = ex.ToString();
                labFailStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnQueryFailSn.Enabled = true;
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                saveFiledlg.InitialDirectory = Application.StartupPath;
                saveFiledlg.Filter = "csv files (*.csv)|*.csv";
                if (saveFiledlg.ShowDialog() != DialogResult.OK)
                    return;
                string filePath = saveFiledlg.FileName;
                StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8);
                string strWrite = string.Empty;

                strWrite += "产品条码,治具槽位,老化位置,开始时间,结束时间,不良数据,不良时间";
                sw.WriteLine(strWrite);
                for (int i = 0; i < snFailView.Rows.Count; i++)
                {
                    strWrite = string.Empty;
                    strWrite += snFailView.Rows[i].Cells[0].Value.ToString() + ",";
                    strWrite += snFailView.Rows[i].Cells[1].Value.ToString() + ",";
                    strWrite += snFailView.Rows[i].Cells[2].Value.ToString() + ",";
                    strWrite += snFailView.Rows[i].Cells[3].Value.ToString() + ",";
                    strWrite += snFailView.Rows[i].Cells[4].Value.ToString() + ",";
                    strWrite += snFailView.Rows[i].Cells[5].Value.ToString() + ",";
                    strWrite += snFailView.Rows[i].Cells[6].Value.ToString() + ",";
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
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {

            try
            {
                btnQuery.Enabled = false;

                if (!web_check_flag)
                {
                    labStatus.Text = CLanguage.Lan("无法连接") + "[" + url + "]";
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                if (txtSn.Text == "")
                    return;

                string er = string.Empty;

                int snType = 0;

                string serialNo = txtSn.Text;

                if (chkSnType.Checked)
                    snType = 1;
                   
                CWeb2.CSn Sn = null;

                if (!CWeb2.GetSnRecord(serialNo, out Sn, out er, snType))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                SnView.Rows.Clear();

                for (int i = 0; i < Sn.Para.Count; i++)
                {
                    SnView.Rows.Add(Sn.Para[i].FlowName, Sn.Para[i].Result == 0 ? "PASS" : "FAIL",
                                    Sn.Para[i].StartTime, Sn.Para[i].EndTime,
                                    Sn.Para[i].IdCard + "-" + Sn.Para[i].SlotNo.ToString(), Sn.Para[i].TestData,
                                    Sn.Para[i].Remark1, Sn.Para[i].Remark2);
                    if (Sn.Para[i].Result != 0)
                        SnView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    SnView.Rows[i].Selected = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                btnQuery.Enabled = true; 
            }
        }
        private void snFailView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string Sn = snFailView.Rows[e.RowIndex].Cells[0].Value.ToString();

                string ReportPath = snFailView.Rows[e.RowIndex].Cells[7].Value.ToString();

                if (MessageBox.Show(CLanguage.Lan("确定要打开机型测试报表") + "SN[" + Sn + "]?", "Tip", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                if (!File.Exists(ReportPath))
                {
                    MessageBox.Show(CLanguage.Lan("找不到测试报表")+"[" + ReportPath + "]," + CLanguage.Lan("请检查是否删除?"), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                Process.Start(ReportPath);
            }
        }
        private void labSysDB_DoubleClick(object sender, EventArgs e)
        {
            string fileDirectry = string.Empty;
            fileDirectry = Application.StartupPath;
            OpenFileDialog openFiledlg = new OpenFileDialog();
            openFiledlg.InitialDirectory = fileDirectry;
            openFiledlg.Filter = "accdb files (*.accdb)|*.accdb";
            if (openFiledlg.ShowDialog() != DialogResult.OK)
                return;
            labSysDB.Text = openFiledlg.FileName;
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            url =txtUrl.Text;

            sysDB = labSysDB.Text;

            CIniFile.WriteToIni("FrmSnQuery", "url", url, iniFile);

            CIniFile.WriteToIni("FrmSnQuery", "sysDB", sysDB, iniFile);
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
