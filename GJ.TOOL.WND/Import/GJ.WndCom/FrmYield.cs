using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GJ.MES;
using GJ.PLUGINS;
using GJ.COM;
using System.IO;
using GJ.PDB;

namespace GJ.WndCom
{

    public partial class FrmYield : Form,IChildMsg
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
        public FrmYield()
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
        private string iniFile = Application.StartupPath + "\\iniFile.ini";
        private string url = "http://192.168.3.130/Service.asmx";
        #endregion

        #region 面板回调函数
        private void FrmYield_Load(object sender, EventArgs e)
        {

            txtUrl.Text = CIniFile.ReadFromIni("FrmYield", "url", iniFile, url);

            url = txtUrl.Text;

            List<string> keyName = new List<string>();

            List<string> keyValue = new List<string>();

            CIniFile.GetIniKeySection("PLCAlarm", out  keyName, out keyValue, iniFile);

            cmbStatName.Items.Clear();

            cmbStatName.Items.Add(CLanguage.Lan("所有工位"));

            for (int i = 0; i < keyValue.Count; i++)
            {
                cmbStatName.Items.Add(keyValue[i]); 
            }
            cmbStatName.SelectedIndex = 0;

            cmbbAlarm.Items.Clear();
            cmbbAlarm.Items.Add(CLanguage.Lan("全部"));
            cmbbAlarm.Items.Add(CLanguage.Lan("解除"));
            cmbbAlarm.Items.Add(CLanguage.Lan("报警"));
            cmbbAlarm.SelectedIndex = 0;

            cmbFixNumSlotNo.SelectedIndex = 0;

            check_web_status_handler check_web = new check_web_status_handler(check_web_status);

            check_web.BeginInvoke(null, null);

            txtAccess.Text = CIniFile.ReadFromIni("FrmYield", "AccessFile", iniFile, Application.StartupPath + "\\BURNIN\\PLC_BURNIN.accdb");

            txtCSV.Text = CIniFile.ReadFromIni("FrmYield", "CSVFile", iniFile);

            if (LoadPLCAccessDB(txtAccess.Text))
            {
                LoadPLCCSV(txtCSV.Text);
            }
        }
        private void btnQueryStat_Click(object sender, EventArgs e)
        {
            try
            {
                btnQueryStat.Enabled = false;

                if (!web_check_flag)
                {
                    labT1.Text = CLanguage.Lan("无法连接") + "[" + url + "]";
                    labT1.ForeColor = Color.Red;
                    return;
                }

                string er = string.Empty;

                CWeb2.CYield_Base input = new CWeb2.CYield_Base();

                input.StartTime = dpYieldStartTime.Value.Date.ToString("yyyy/MM/dd");
                input.EndTime = dpYieldEndTime.Value.Date.ToString("yyyy/MM/dd");
                input.FlowIndex = 0;
                input.FlowName = string.Empty;
                input.FlowGuid = string.Empty;
                input.LineNo = -1;
                input.LineName = string.Empty;
                input.OrderName = string.Empty;
                input.Model = string.Empty;

                List<CWeb2.CYield_Para> output = null;

                if (!CWeb2.QueryProductivity(input, out output, out er))
                {
                    labT1.Text = er;
                    labT1.ForeColor = Color.Red;
                    return;
                }

                YieldView.Rows.Clear();

                for (int i = 0; i < output.Count; i++)
                {
                    YieldView.Rows.Add(output[i].IdNo, output[i].Name, output[i].TTNum, output[i].FailNum);
                }

                labT1.Text = CLanguage.Lan("查询数量") + "【" + output.Count.ToString() + "】";
                labT1.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                labT1.Text = ex.ToString();
                labT1.ForeColor = Color.Red;
            }
            finally
            {
                btnQueryStat.Enabled = true;
            }
        }
        private void btnQueryAlarmList_Click(object sender, EventArgs e)
        {
            try
            {
                btnQueryAlarmList.Enabled = false;

                string er = string.Empty;

                if (!web_check_flag)
                {
                    labT1.Text = CLanguage.Lan("无法连接") + "[" + url + "]";
                    labT1.ForeColor = Color.Red;
                    return;
                }

                List<CWeb2.CAlarmRecord> alarmList = null;

                CWeb2.CAlarm_Base condition = new CWeb2.CAlarm_Base();

                condition.StartTime = dpAlarmStartDate.Value.Date.ToString("yyyy/MM/dd");

                condition.EndTime = dpAlarmEndDate.Value.Date.ToString("yyyy/MM/dd");

                if (cmbStatName.Text == CLanguage.Lan("所有工位"))
                    condition.StatName = "";
                else 
                    condition.StatName = cmbStatName.Text;

                condition.StatGuid = txtAlarmStatGuid.Text;

                condition.ErrNo = System.Convert.ToInt32(txtAlarmCode.Text);

                condition.bAlarm = cmbbAlarm.SelectedIndex - 1;

                if (!CWeb2.GetAlarmRecord(condition, out alarmList, out er))
                {
                    labT1.Text = er;
                    labT1.ForeColor = Color.Red;
                    return;
                }

                DataTable dt = new DataTable();

                dt.Columns.Add(CLanguage.Lan("编号"));
                dt.Columns.Add(CLanguage.Lan("工位名称"));
                dt.Columns.Add(CLanguage.Lan("工位标识"));
                dt.Columns.Add(CLanguage.Lan("报警状态"));
                dt.Columns.Add(CLanguage.Lan("报警代号"));
                dt.Columns.Add(CLanguage.Lan("报警时间"));
                dt.Columns.Add(CLanguage.Lan("报警信息"));
                dt.Columns.Add(CLanguage.Lan("备注") + "1");
                dt.Columns.Add(CLanguage.Lan("备注") + "2");

                for (int i = 0; i < alarmList.Count; i++)
                {
                    dt.Rows.Add(i + 1,
                                alarmList[i].StatName, alarmList[i].StatGuid,
                                (alarmList[i].bAlarm == 1 ? CLanguage.Lan("报警") : CLanguage.Lan("解除")), alarmList[i].ErrNo,
                                 alarmList[i].HappenTime, alarmList[i].AlarmInfo,
                                alarmList[i].Remark1, alarmList[i].Remark2
                                );
                }

                AlarmView.DataSource = dt;

                labT2.Text = CLanguage.Lan("查询数量") + "【" + alarmList.Count .ToString() + "】";
                labT2.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                labT2.Text = ex.ToString();
                labT2.ForeColor = Color.Red;
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
                labT2.Text = ex.ToString();
                labT2.ForeColor = Color.Red;
            }
            finally
            {
                btnExport.Enabled = true;
            }           
        }
        private void btnFixNumQuery_Click(object sender, EventArgs e)
        {
            try
            {
                btnFixNumQuery.Enabled = false;

                string er = string.Empty;

                if (!web_check_flag)
                {
                    labT2.Text = CLanguage.Lan("无法连接") + "[" + url + "]";
                    labT2.ForeColor = Color.Red;
                    return;
                }

                FixNumView.Rows.Clear();

                List<CWeb2.CFixUseNum> idCardList=null;

                CWeb2.CFixCondition condition = new CWeb2.CFixCondition()
                {
                     FlowName = txtStatName.Text,
                     IdCard = txtFixNumIdCard.Text,
                     SlotNo = cmbFixNumSlotNo.SelectedIndex - 1
                };

                if (!CWeb2.QueryIdCardUseNum(condition, out idCardList, out er))
                {
                    labT2.Text = er;
                    labT2.ForeColor = Color.Red;
                    return;
                }

                for (int i = 0; i < idCardList.Count; i++)
                {
                    FixNumView.Rows.Add(i + 1, idCardList[i].IdCard, idCardList[i].SlotNo,
                                       idCardList[i].TTNum, idCardList[i].FailNum, idCardList[i].ConFailNum);
                }

            }
            catch (Exception ex)
            {
                labT2.Text = ex.ToString();
                labT2.ForeColor = Color.Red;
            }
            finally
            {
                btnFixNumQuery.Enabled = true;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            url = txtUrl.Text;

            CIniFile.WriteToIni("FrmYield", "url", url, iniFile);
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

        #region 报警列表
        private void btnLoadAccess_Click(object sender, EventArgs e)
        {
            try
            {
                btnLoadAccess.Enabled = false;

                string fileDirectry = string.Empty;

                OpenFileDialog openFiledlg = new OpenFileDialog();

                openFiledlg.InitialDirectory = Application.StartupPath + "\\BURNIN";

                openFiledlg.Filter = "Access files (*.accdb)|*.accdb";

                if (openFiledlg.ShowDialog() != DialogResult.OK)
                    return;

                txtAccess.Text = openFiledlg.FileName;

                CIniFile.WriteToIni("FrmYield", "AccessFile", txtAccess.Text, iniFile);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                btnLoadAccess.Enabled = true;
            }
        }
        private void btnLoadCSV_Click(object sender, EventArgs e)
        {
            try
            {
                btnLoadCSV.Enabled = false;

                string fileDirectry = string.Empty;

                OpenFileDialog openFiledlg = new OpenFileDialog();

                openFiledlg.InitialDirectory = Application.StartupPath + "\\BURNIN";

                openFiledlg.Filter = "csv files (*.csv)|*.csv";

                if (openFiledlg.ShowDialog() != DialogResult.OK)
                    return;

                txtCSV.Text = openFiledlg.FileName;

                CIniFile.WriteToIni("FrmYield", "CSVFile", txtCSV.Text, iniFile);

                LoadPLCCSV(txtCSV.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                btnLoadCSV.Enabled = true;
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                btnRefresh.Enabled = false;

                if (LoadPLCAccessDB(txtAccess.Text))
                {
                    LoadPLCCSV(txtCSV.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                btnRefresh.Enabled = true;
            }
        }       
        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                btnModify.Enabled = false;

                SavePLCAccssDB(txtAccess.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                btnModify.Enabled = true;
            }
        }
        private bool LoadPLCAccessDB(string accessFile)
        {
            try
            {
                if (!File.Exists(accessFile))
                {
                    //MessageBox.Show("【" + accessFile + "】不存在", "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                PLCView.Rows.Clear();

                string er = string.Empty;

                CDBCOM db = new CDBCOM(EDBType.Access, "", accessFile);

                DataSet ds = null;

                string sqlCmd = string.Format("select * from AlamList order by idNo");

                if (!db.QuerySQL(sqlCmd, out ds, out er))
                {
                    MessageBox.Show(er, "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    PLCView.Rows.Add(ds.Tables[0].Rows[i]["RegFun"].ToString(),
                                     ds.Tables[0].Rows[i]["RegDesc"].ToString(),
                                     "");
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }
        private bool LoadPLCCSV(string csvFile)
        {
            try
            {
                if (csvFile == string.Empty)
                    return false;

                if (!File.Exists(csvFile))
                {
                    //MessageBox.Show("【" + csvFile + "】不存在", "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                int row = -1;

                Encoding gb2312 = Encoding.GetEncoding("GB2312");

                StreamReader sr = new StreamReader(csvFile, gb2312,true);

                while (sr.Peek() > 0)
                {
                    row++;

                    string info = sr.ReadLine();

                    if (row > PLCView.Rows.Count)
                        break;

                    if (row > 0)
                    {
                        string[] str = info.Split(',');

                        if (str.Length >= 3)
                        {
                            PLCView.Rows[row - 1].Cells[2].Value = str[2];
                        }
                    }
                }

                sr.Close();

                sr = null;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }
        private bool SavePLCAccssDB(string accessFile)
        {
            try
            {
                if (!File.Exists(accessFile))
                {
                    MessageBox.Show("【" + accessFile + "】不存在", "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                string er = string.Empty;

                CDBCOM db = new CDBCOM(EDBType.Access, "", accessFile);

                List<string> sqlCmdList = new List<string>();

                string sqlCmd = string.Empty;

                for (int i = 0; i < PLCView.Rows.Count; i++)
                {
                    string info = PLCView.Rows[i].Cells[2].Value.ToString();

                    sqlCmd = "update AlamList set RegDisable=0,RegDesc='" + info + "' where idNo=" + (i + 1).ToString();

                    sqlCmdList.Add(sqlCmd);
                }

                if (!db.excuteSQLArray(sqlCmdList, out er))
                {
                    MessageBox.Show(er, "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                MessageBox.Show("保存PLC配方OK", "保存配方", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }
        #endregion
   
    }
}
