using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using GJ.COM;
using GJ.PLUGINS;

namespace GJ.TOOL
{
    public partial class FrmUpgrade : Form, IChildMsg
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

        #region 常量定义
        /// <summary>
        /// 数据包头
        /// </summary>
        private const string C_XML_HEADER = "GJMES";
        /// <summary>
        /// 输入表单名
        /// </summary>
        private const string C_XML_BASE = "XmlBase";
        /// <summary>
        /// 返回表单名
        /// </summary>
        private const string C_XML_PARA = "XmlPara";
        /// <summary>
        /// 文件类型
        /// </summary>
        private string[] C_FILE_TYPE = new string[6] { "*.*", ".*", ".dll", ".ini", ".xml", ".accdb" };
        #endregion

        #region 类定义
        /// <summary>
        /// 自动升级
        /// </summary>
        private class CAutoUpgrade
        {
            public int ChkUpgrade = 0;

            public string PackageFolder = string.Empty;

            public int[] PackageType = new int[6];

            public string PackagePreFix = string.Empty;

            public string UpgradeFolder = string.Empty;
        }
        #endregion
        
        #region 显示窗口

        #region 字段
        private static FrmUpgrade dlg = null;
        private static int _height = 0;
        private static int _width = 0;
        private static string _para = string.Empty;
        private static object syncRoot = new object();
        #endregion

        #region 属性
        public static bool IsAvalible
        {
            get
            {
                lock (syncRoot)
                {
                    if (dlg != null && !dlg.IsDisposed)
                        return true;
                    else
                        return false;
                }
            }
        }
        public static Form mdlg
        {
            get
            {
                return dlg;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 创建唯一实例
        /// </summary>
        public static FrmUpgrade CreateInstance(int height = 0, int width = 0, string para = "")
        {
            _height = height;

            _width = width;

            _para = para;

            lock (syncRoot)
            {
                if (dlg == null || dlg.IsDisposed)
                {
                    dlg = new FrmUpgrade();
                }
            }
            return dlg;
        }
        #endregion

        #endregion

        #region 面板控件
        private CheckBox[] chkBox = null;
        #endregion

        #region 字段
        private string iniFile = Application.StartupPath + "\\iniFile.ini";
        private bool checkFlag = false;
        private Dictionary<string, CVersionControl.CUpgrade> WebVersion = new Dictionary<string, CVersionControl.CUpgrade>();
        private CVersionControl.CUpgrade CurVersion = new CVersionControl.CUpgrade();
        private CVersionControl.CDownFile CurDownFile = new CVersionControl.CDownFile();
        private CAutoUpgrade AutoUpgrade = new CAutoUpgrade();
        #endregion

        #region 构造函数
        public FrmUpgrade()
        {
            InitializeComponent();

            IntialControl();

            SetDoubleBuffered();
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化界面
        /// </summary>
        private void IntialControl()
        {
            chkBox = new CheckBox[] { checkBox0 ,checkBox1, checkBox2, checkBox3, checkBox4, checkBox5 };

            LoadIniFile();
        }
        /// <summary>
        /// 设置双缓冲,防止界面闪烁
        /// </summary>
        private void SetDoubleBuffered()
        {
            CUISetting.SetUIDoubleBuffered(this);
        }
        #endregion

        #region 委托
        private delegate void CheckSystemHandler(string projectNumber);
        #endregion

        #region 面板回调函数
        private void WndFrmVersionUpgrade_Load(object sender, EventArgs e)
        {
            if (_height == -1 || _width == -1)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                if (_height != 0)
                    this.Height = _height;
                if (_width != 0)
                    this.Width = _width;
            }

            cmbPageSize.SelectedIndex = 0;
        }
        private void cmbVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string verion = cmbVersion.Text;

            if (WebVersion.ContainsKey(verion))
            {
                labProjectNumber.Text = WebVersion[verion].ProjectNumber;

                labProjectName.Text = WebVersion[verion].ProjectName;

                labProjectCustom.Text = WebVersion[verion].ProjectCustom;

                labSoftWareVersion.Text = WebVersion[verion].SoftWareVersion;

                labSoftWareAuthor.Text = WebVersion[verion].SoftWareAuthor;

                labSoftWareDate.Text = WebVersion[verion].SoftWareDate;

            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveIniFile();

                MessageBox.Show("保存参数成功", "软件自动升级", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void btnulr_Click(object sender, EventArgs e)
        {
            try
            {
                string projectNumber = txtProjectNumber.Text;

                if (projectNumber == string.Empty)
                {
                    ShowLog("项目编号不能为空,请输入该项目编号", true);
                    return;
                }

                btnulr.Enabled = false;

                SaveIniFile();

                WebVersion.Clear();

                cmbVersion.Items.Clear();

                labProjectNumber.Text = string.Empty;

                labProjectName.Text = string.Empty;

                labProjectCustom.Text = string.Empty;

                labSoftWareVersion.Text = string.Empty;

                labSoftWareAuthor.Text = string.Empty;

                labSoftWareDate.Text = string.Empty;

                cmbVersion.Text = string.Empty;

                CheckSystemHandler checkSystem = new CheckSystemHandler(OnCheckSystem);

                checkSystem.BeginInvoke(projectNumber, null, null);

            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
        }
        private void OnCheckSystem(string projectNumber)
        {
            try
            {
                string version = string.Empty;

                string er = string.Empty;

                CVersionControl.WebUlr = CurVersion.WebUlr;

                if (!CVersionControl.CheckSystem(projectNumber, out version, out er))
                {
                    checkFlag = false;
                    ShowLog(er, true);
                    return;
                }

                ShowWebVersion(version);

                ShowLog("连接正常", false);

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
        private void ShowLog(string info, bool bAlarm)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string, bool>(ShowLog), info, bAlarm);
            else
            {
                labStatus.Text = CLanguage.Lan(info);

                if (bAlarm)
                    labStatus.ForeColor = Color.Red;
                else
                    labStatus.ForeColor = Color.Blue;
            }
        }
        private void ShowTime(string info)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string>(ShowTime), info);
            else
            {
                labTime.Text = info;
            }
        }
        private void SetProcessMax(int maxValue)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<int>(SetProcessMax), maxValue);
            else
            {
                progressBar1.Maximum = maxValue;
            }
        }
        private void SetProcessValue(int value)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<int>(SetProcessValue), value);
            else
            {
                progressBar1.Value = value;
            }
        }
        private void SetImportBtn(bool able)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<bool>(SetImportBtn), able);
            else
            {
                btnImport.Enabled = able;
            }
        }
        private void SetDownLoadBtn(bool able)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<bool>(SetDownLoadBtn), able);
            else
            {
                btnDownLoad.Enabled = able;
            }
        }
        private void SetPackageBtn(bool able)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<bool>(SetPackageBtn), able);
            else
            {
                btnPacking.Enabled = able;
            }
        }
        private void SetUpgradeBtn(bool able)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<bool>(SetUpgradeBtn), able);
            else
            {
                btnUpgrading.Enabled = able;
            }
        }
        private void SetBtnCon(bool able)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<bool>(SetBtnCon), able);
            else
            {
                btnulr.Enabled = able;
            }
        }
        private void ShowWebVersion(string version)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string>(ShowWebVersion), version);
            else
            {
                DataSet dsXml = CXml.ConvertXMLToDataSet(version);

                if (dsXml == null)
                    return;

                int number = 0;

                if (dsXml.Tables.Contains(C_XML_BASE))
                {
                    number = System.Convert.ToInt16(dsXml.Tables[C_XML_BASE].Rows[0]["Number"].ToString());
                }

                if (dsXml.Tables.Contains(C_XML_PARA))
                {
                    for (int i = 0; i < dsXml.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        CVersionControl.CUpgrade parameter = new CVersionControl.CUpgrade();

                        parameter.ProjectNumber = dsXml.Tables[C_XML_PARA].Rows[i]["ProjectNumber"].ToString();

                        parameter.ProjectName = dsXml.Tables[C_XML_PARA].Rows[i]["ProjectName"].ToString();

                        parameter.ProjectCustom = dsXml.Tables[C_XML_PARA].Rows[i]["ProjectCustom"].ToString();

                        parameter.SoftWareName = dsXml.Tables[C_XML_PARA].Rows[i]["SoftWareName"].ToString();

                        parameter.SoftWareVersion = dsXml.Tables[C_XML_PARA].Rows[i]["SoftWareVersion"].ToString();

                        parameter.SoftWareAuthor = dsXml.Tables[C_XML_PARA].Rows[i]["SoftWareAuthor"].ToString();

                        parameter.SoftWareDate = dsXml.Tables[C_XML_PARA].Rows[i]["SoftWareDate"].ToString();

                        WebVersion.Add(parameter.SoftWareVersion, parameter);

                        cmbVersion.Items.Add(parameter.SoftWareVersion);
                    }                    
                }

                if (cmbVersion.Items.Count > 0)
                {
                    cmbVersion.SelectedIndex = 0;
                }
            }
        }
        private void ShowUpgradeVersion(string version)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string>(ShowUpgradeVersion), version);
            else
            {
                txtSoftWareVersion.Text = version;

                CurVersion.SoftWareVersion = version;

                CIniFile.WriteToIni("VerUpgrade", "SoftWareVersion", CurVersion.SoftWareVersion, iniFile);
            }
        }
        private void btnPackFloder_Click(object sender, EventArgs e)
        {
            try
            {
                btnPackFloder.Enabled = false;

                FolderBrowserDialog dlg = new FolderBrowserDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtPackFolder.Text = dlg.SelectedPath;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                btnPackFloder.Enabled = true;
            }
        }
        private void btnUpgradeFolder_Click(object sender, EventArgs e)
        {
            try
            {
                btnUpgradeFolder.Enabled = false;

                FolderBrowserDialog dlg = new FolderBrowserDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtUpgradeFolder.Text = dlg.SelectedPath;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                btnUpgradeFolder.Enabled = true;
            }
        }
        #endregion

        #region 方法
        private void LoadIniFile()
        {

            //当前版本

            CurVersion.WebUlr = CIniFile.ReadFromIni("VerUpgrade", "WebUlr", iniFile, "http://139.159.216.163:8081/Service.asmx");

            CurVersion.ProjectNumber = CIniFile.ReadFromIni("VerUpgrade", "ProjectNumber", iniFile);

            CurVersion.ProjectName = CIniFile.ReadFromIni("VerUpgrade", "ProjectName", iniFile);

            CurVersion.ProjectCustom = CIniFile.ReadFromIni("VerUpgrade", "ProjectCustom", iniFile);

            CurVersion.SoftWareVersion = CIniFile.ReadFromIni("VerUpgrade", "SoftWareVersion", iniFile);

            CurVersion.SoftWareAuthor = CIniFile.ReadFromIni("VerUpgrade", "SoftWareAuthor", iniFile);

            CurVersion.SoftWareDate = CIniFile.ReadFromIni("VerUpgrade", "SoftWareDate", iniFile);

            CurVersion.ZipFile = CIniFile.ReadFromIni("VerUpgrade", "ZipFile", iniFile, Application.StartupPath);

            CurVersion.FilePageSize = System.Convert.ToInt32(CIniFile.ReadFromIni("VerUpgrade", "FilePageSize", iniFile,"1024")) * 1024;

            txtulr.Text = CurVersion.WebUlr;

            txtProjectNumber.Text = CurVersion.ProjectNumber;

            txtProjectName.Text = CurVersion.ProjectName;

            txtProjectCustom.Text = CurVersion.ProjectCustom;

            txtSoftWareVersion.Text = CurVersion.SoftWareVersion;

            txtSoftWareAuthor.Text = CurVersion.SoftWareAuthor;

            if (CurVersion.SoftWareDate != string.Empty)
            {
                dtSoftWareDate.Value = System.Convert.ToDateTime(CurVersion.SoftWareDate);
            }

            txtZipFile.Text = CurVersion.ZipFile;

            cmbPageSize.Text = (CurVersion.FilePageSize/1024).ToString();

            //下载版本

            CurDownFile.ZipFolder = CIniFile.ReadFromIni("VerUpgrade", "ZipFolder", iniFile, Application.StartupPath);

            CurDownFile.FilePageSize = CurVersion.FilePageSize;

            txtDownLoadFile.Text = CurDownFile.ZipFolder;

            //自动升级

            AutoUpgrade.ChkUpgrade = System.Convert.ToInt16(CIniFile.ReadFromIni("VerUpgrade", "ChkUpgrade", iniFile, "0"));

            AutoUpgrade.PackageFolder = CIniFile.ReadFromIni("VerUpgrade", "PackageFolder", iniFile, Application.StartupPath);

            AutoUpgrade.UpgradeFolder = CIniFile.ReadFromIni("VerUpgrade", "UpgradeFolder", iniFile, Application.StartupPath);

            AutoUpgrade.PackagePreFix = CIniFile.ReadFromIni("VerUpgrade", "PackagePreFix", iniFile);

            for (int i = 0; i < AutoUpgrade.PackageType.Length; i++)
            {
                AutoUpgrade.PackageType[i] = System.Convert.ToInt16(CIniFile.ReadFromIni("VerUpgrade", C_FILE_TYPE[i], iniFile, "0"));
            }

            chkWeb.Checked =(AutoUpgrade.ChkUpgrade ==1?true:false);

            txtPackFolder.Text = AutoUpgrade.PackageFolder;

            txtUpgradeFolder.Text = AutoUpgrade.UpgradeFolder;

            txtprefix.Text = AutoUpgrade.PackagePreFix;

             for (int i = 0; i < AutoUpgrade.PackageType.Length; i++)
             {
                 chkBox[i].Checked = (AutoUpgrade.PackageType[i]==1?true:false);
             }
          
        }
        private void SaveIniFile()
        {

            CurVersion.WebUlr = txtulr.Text;

            CurVersion.ProjectNumber = txtProjectNumber.Text;

            CurVersion.ProjectName = txtProjectName.Text;

            CurVersion.ProjectCustom = txtProjectCustom.Text;

            CurVersion.SoftWareVersion = txtSoftWareVersion.Text;

            CurVersion.SoftWareAuthor = txtSoftWareAuthor.Text;

            CurVersion.SoftWareDate = dtSoftWareDate.Value.ToString("yyyy/MM/dd");

            CurVersion.ZipFile = txtZipFile.Text;

            CurVersion.FilePageSize = System.Convert.ToInt32(cmbPageSize.Text) * 1024;

            CIniFile.WriteToIni("VerUpgrade", "WebUlr", CurVersion.WebUlr, iniFile);

            CIniFile.WriteToIni("VerUpgrade", "ProjectNumber", CurVersion.ProjectNumber, iniFile);

            CIniFile.WriteToIni("VerUpgrade", "ProjectName", CurVersion.ProjectName, iniFile);

            CIniFile.WriteToIni("VerUpgrade", "ProjectCustom", CurVersion.ProjectCustom, iniFile);

            CIniFile.WriteToIni("VerUpgrade", "SoftWareVersion", CurVersion.SoftWareVersion, iniFile);

            CIniFile.WriteToIni("VerUpgrade", "SoftWareAuthor", CurVersion.SoftWareAuthor, iniFile);

            CIniFile.WriteToIni("VerUpgrade", "SoftWareDate", CurVersion.SoftWareDate, iniFile);

            CIniFile.WriteToIni("VerUpgrade", "ZipFile", CurVersion.ZipFile, iniFile);

            CIniFile.WriteToIni("VerUpgrade", "FilePageSize", (CurVersion.FilePageSize/1024).ToString(), iniFile);

            CurDownFile.ZipFolder = txtDownLoadFile.Text;

            CurDownFile.FilePageSize = CurVersion.FilePageSize;

            CIniFile.WriteToIni("VerUpgrade", "ZipFolder", CurDownFile.ZipFolder, iniFile);

            AutoUpgrade.ChkUpgrade = (chkWeb.Checked ? 1 : 0);

            AutoUpgrade.PackageFolder = txtPackFolder.Text;

            AutoUpgrade.UpgradeFolder = txtUpgradeFolder.Text;

            AutoUpgrade.PackagePreFix = txtprefix.Text;

            for (int i = 0; i < AutoUpgrade.PackageType.Length; i++)
            {
                AutoUpgrade.PackageType[i] = (chkBox[i].Checked ? 1 : 0);
            }

            CIniFile.WriteToIni("VerUpgrade", "ChkUpgrade", AutoUpgrade.ChkUpgrade.ToString(), iniFile);

            CIniFile.WriteToIni("VerUpgrade", "PackageFolder", AutoUpgrade.PackageFolder, iniFile);

            CIniFile.WriteToIni("VerUpgrade", "UpgradeFolder", AutoUpgrade.UpgradeFolder, iniFile);

            CIniFile.WriteToIni("VerUpgrade", "PackagePreFix", AutoUpgrade.PackagePreFix, iniFile);

            for (int i = 0; i < AutoUpgrade.PackageType.Length; i++)
            {
                CIniFile.WriteToIni("VerUpgrade", C_FILE_TYPE[i], AutoUpgrade.PackageType[i].ToString(), iniFile);
            }
        }
        #endregion

        #region 上载
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFiledlg = new OpenFileDialog();
                openFiledlg.Filter = "zip files (*.zip)|*.zip";
                if (openFiledlg.ShowDialog() != DialogResult.OK)
                    return;

                txtZipFile.Text = openFiledlg.FileName;
            }
            catch (Exception)
            {                
                throw;
            }
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveIniFile();

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }
                if (!File.Exists(txtZipFile.Text))
                {
                    ShowLog("Zip文件不存在", true);
                    return;
                }
                if (txtProjectNumber.Text == string.Empty)
                {
                    ShowLog("项目编号不能为空", true);
                    return;
                }
                if (txtSoftWareVersion.Text == string.Empty)
                {
                    ShowLog("当前版本不能为空", true);
                    return;
                }

                if (MessageBox.Show("确定上载文件当前版本为[" + txtSoftWareVersion.Text + "]?", "上载文件", 
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                btnImport.Enabled = false;

                CurVersion.ZipFile = txtZipFile.Text;

                CurVersion.SoftWareName = Path.GetFileName(txtZipFile.Text);

                CurVersion.FilePageSize = System.Convert.ToInt32(cmbPageSize.Text) * 1024;

                Task.Factory.StartNew(() => UpLoadZipFile());

            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
        }
        #endregion

        #region 下载
        private void btnFolder_Click(object sender, EventArgs e)
        {
            try
            {
                btnFolder.Enabled = false;

                FolderBrowserDialog dlg = new FolderBrowserDialog();
                
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtDownLoadFile.Text = dlg.SelectedPath;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                btnFolder.Enabled = true;
            }
        }
        private void btnDownLoad_Click(object sender, EventArgs e)
        {
            try
            {
                CurDownFile.ProjectNumber = labProjectNumber.Text;
                CurDownFile.SoftWareVersion = labSoftWareVersion.Text;
                CurDownFile.ZipFolder = txtDownLoadFile.Text;
                CurDownFile.FilePageSize = System.Convert.ToInt32(cmbPageSize.Text) * 1024;
                CurDownFile.CurrentPageIndex = 0;

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }
                if (!Directory.Exists(txtDownLoadFile.Text))
                {
                    ShowLog("Zip下载文件夹不存在", true);
                    return;
                }
                if (CurDownFile.ProjectNumber == string.Empty)
                {
                    ShowLog("项目编号不能为空", true);
                    return;
                }
                if (CurDownFile.SoftWareVersion == string.Empty)
                {
                    ShowLog("当前版本不能为空", true);
                    return;
                }

                btnDownLoad.Enabled = false;

                Task.Factory.StartNew(() => DownZipFile());
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
        }
        #endregion

        #region 上载与下载文件
        /// <summary>
        /// 上载文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private void UpLoadZipFile()
        {
            CurVersion.UpLoadFlag = false;

            Stopwatch watcher = new Stopwatch();

            watcher.Start();

            try
            {
                string er = string.Empty;

                if (!CVersionControl.CreatZipFile(CurVersion, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                int checkSum = 0;

                FileStream fs = new FileStream(CurVersion.ZipFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                int size = (int)fs.Length;

                int bufferSize = CurVersion.FilePageSize;

                int count = (int)Math.Ceiling((double)size / (double)bufferSize);

                SetProcessMax(count);

                for (int i = 0; i < count; i++)
                {
                    int readSize = bufferSize;

                    if (i == count - 1)
                    {
                        readSize = size - bufferSize * i;
                    }

                    byte[] buffer = new byte[readSize];

                    fs.Read(buffer, 0, readSize);

                    StringBuilder ret = new StringBuilder();

                    foreach (byte b in buffer)
                    {
                        checkSum += b;

                        ret.AppendFormat("{0:X2}", b);
                    }

                    SetProcessValue(i + 1);

                    int pageCheckSum = 0;

                    if (!CVersionControl.UpLoadFile(CurVersion, string.Empty, ret.ToString(), out pageCheckSum, out er))
                    {
                        ShowLog(er, true);
                        return;
                    }

                    string waitTime = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0");

                    ShowTime((i + 1).ToString() + "->" + waitTime + "ms");

                    Thread.Sleep(5);
                }

                fs.Close();

                int CheckSumFromFile = 0;

                if (!CVersionControl.VerifyZipFile(CurVersion, out CheckSumFromFile, out er))
                {
                   ShowLog(er, true);
                   return;
                }

                if (checkSum != CheckSumFromFile)
                {
                    ShowLog("上载文件检验和不一致", true);
                    return;
                }

                CurVersion.UpLoadFlag = true;
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                SetImportBtn(true);

                if (CurVersion.UpLoadFlag)
                {
                    ShowLog("上载Zip文件OK:" + ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s", false);
                }
            }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        private void DownZipFile()
        {
            CurDownFile.DownLoadFlag = false;

            Stopwatch watcher = new Stopwatch();

            watcher.Start();

            try
            {
                string er = string.Empty;

                int checkSum = 0;

                if (!CVersionControl.CreateDownLoadZip(CurDownFile, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                checkSum = CurDownFile.CheckSum;

                CurDownFile.CurrentPageIndex = 0;

                string fileName = CurDownFile.ZipFolder + "\\" + CurDownFile.SoftWareName;

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                int size = CurDownFile.TotalPageBytes;

                int bufferSize = CurDownFile.FilePageSize;

                int count = (int)Math.Ceiling((double)size / (double)bufferSize);

                SetProcessMax(count);

                for (int i = 0; i < count; i++)
                {
                    CurDownFile.CurrenPageSize = bufferSize;

                    if (i == count - 1)
                    {
                        CurDownFile.FilePageSize = size - bufferSize * i;
                    }

                   SetProcessValue(i + 1);

                   if (!CVersionControl.AppenDownLoadZip(CurDownFile, out er))
                   {
                       ShowLog(er, true);
                       return;
                   }
                   
                   int len = CurDownFile.CurByteStream.Length/2;

                   byte[] buffer = new byte[len];

                   for (int z = 0; z < len; z++)
                   {
                       string str = CurDownFile.CurByteStream.ToString(z * 2, 2);
                       buffer[z] = System.Convert.ToByte(str, 16);
                   }              

                   FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                   fs.Seek(0, SeekOrigin.End);
                   fs.Write(buffer, 0, buffer.Length);
                   fs.Close();

                   CurDownFile.CurrentPageIndex += CurDownFile.FilePageSize;

                   string waitTime = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0");

                   ShowTime((i + 1).ToString() + "->" + waitTime + "ms");

                   Thread.Sleep(5);
                }

                //校验和

                int CheckSumFromFile = 0;

                FileStream rfs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                int rSize = (int)rfs.Length;

                byte[] rBuffer = new byte[rSize];

                rfs.Read(rBuffer, 0, rSize);

                foreach (byte b in rBuffer)
                {
                    CheckSumFromFile += b;
                }

                rfs.Close();


                if (checkSum != CheckSumFromFile)
                {
                    ShowLog("上载文件检验和不一致", true);
                    return;
                }

                CurDownFile.DownLoadFlag = true;
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                SetDownLoadBtn(true);

                if (CurDownFile.DownLoadFlag)
                {
                    ShowLog("下载Zip文件OK:" + ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s", false);
                }
            }
        }
        /// <summary>
        /// 自动打包上载文件
        /// </summary>
        private void AutoUpLoadZipFile()
        {
            CurVersion.UpLoadFlag = false;

            Stopwatch watcher = new Stopwatch();

            watcher.Start();

            try
            {
                string er = string.Empty;

                //打包Zip文件

                List<string> fileTypeList = new List<string>();

                for (int i = 0; i < AutoUpgrade.PackageType.Length; i++)
                {
                    if (AutoUpgrade.PackageType[i] == 1)
                    {
                        fileTypeList.Add(C_FILE_TYPE[i]);
                    }
                }
               
                string[] strArray = AutoUpgrade.PackagePreFix.Split(';');

                List<string> preFixList = new List<string>();

                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i] != string.Empty)
                    {
                        preFixList.Add(strArray[i].ToUpper());
                    }
                }

                CZipHelper.ZipFileDirectory(AutoUpgrade.PackageFolder, CurVersion.ZipFile, fileTypeList, preFixList);

                //上载Web服务器

                if (!CVersionControl.CreatZipFile(CurVersion, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                int checkSum = 0;

                FileStream fs = new FileStream(CurVersion.ZipFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                int size = (int)fs.Length;

                int bufferSize = CurVersion.FilePageSize;

                int count = (int)Math.Ceiling((double)size / (double)bufferSize);

                SetProcessMax(count);

                for (int i = 0; i < count; i++)
                {
                    int readSize = bufferSize;

                    if (i == count - 1)
                    {
                        readSize = size - bufferSize * i;
                    }

                    byte[] buffer = new byte[readSize];

                    fs.Read(buffer, 0, readSize);

                    StringBuilder ret = new StringBuilder();

                    foreach (byte b in buffer)
                    {
                        checkSum += b;

                        ret.AppendFormat("{0:X2}", b);
                    }

                    SetProcessValue(i + 1);

                    int pageCheckSum = 0;

                    if (!CVersionControl.UpLoadFile(CurVersion, string.Empty, ret.ToString(), out pageCheckSum, out er))
                    {
                        ShowLog(er, true);
                        return;
                    }

                    string waitTime = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0");

                    ShowTime((i + 1).ToString() + "->" + waitTime + "ms");

                    Thread.Sleep(5);
                }

                fs.Close();

                int CheckSumFromFile = 0;

                if (!CVersionControl.VerifyZipFile(CurVersion,out CheckSumFromFile, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                if (checkSum != CheckSumFromFile)
                {
                    ShowLog("上载文件检验和不一致", true);
                    return;
                }

                //删除Zip文件
                if (File.Exists(CurVersion.ZipFile))
                {
                    File.Delete(CurVersion.ZipFile);
                }

                CurVersion.UpLoadFlag = true;
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                SetPackageBtn(true);

                if (CurVersion.UpLoadFlag)
                {
                    ShowLog("上载Zip文件OK:" + ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s", false);
                }
            }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        private void AutoDownZipFile()
        {
            CurDownFile.DownLoadFlag = false;

            Stopwatch watcher = new Stopwatch();

            watcher.Start();

            try
            {
                string er = string.Empty;

                //从Web下载最新版本

                int checkSum = 0;

                if (!CVersionControl.CreateDownLoadZip(CurDownFile, out er))
                {
                    ShowLog(er, true);
                    return;
                }

                checkSum = CurDownFile.CheckSum;

                CurDownFile.CurrentPageIndex = 0;

                string fileName = CurDownFile.ZipFolder + "\\" + CurDownFile.SoftWareName;

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                int size = CurDownFile.TotalPageBytes;

                int bufferSize = CurDownFile.FilePageSize;

                int count = (int)Math.Ceiling((double)size / (double)bufferSize);

                SetProcessMax(count);

                for (int i = 0; i < count; i++)
                {
                    CurDownFile.CurrenPageSize = bufferSize;

                    if (i == count - 1)
                    {
                        CurDownFile.FilePageSize = size - bufferSize * i;
                    }

                    SetProcessValue(i + 1);

                    if (!CVersionControl.AppenDownLoadZip(CurDownFile, out er))
                    {
                        ShowLog(er, true);
                        return;
                    }

                    int len = CurDownFile.CurByteStream.Length / 2;

                    byte[] buffer = new byte[len];

                    for (int z = 0; z < len; z++)
                    {
                        string str = CurDownFile.CurByteStream.ToString(z * 2, 2);
                        buffer[z] = System.Convert.ToByte(str, 16);
                    }

                    FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fs.Seek(0, SeekOrigin.End);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();

                    CurDownFile.CurrentPageIndex += CurDownFile.FilePageSize;

                    string waitTime = ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0");

                    ShowTime((i + 1).ToString() + "->" + waitTime + "ms");

                    Thread.Sleep(5);
                }

                //校验和

                int CheckSumFromFile = 0;

                FileStream rfs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                int rSize = (int)rfs.Length;

                byte[] rBuffer = new byte[rSize];

                rfs.Read(rBuffer, 0, rSize);

                foreach (byte b in rBuffer)
                {
                    CheckSumFromFile += b;
                }

                rfs.Close();


                if (checkSum != CheckSumFromFile)
                {
                    ShowLog("上载文件检验和不一致", true);
                    return;
                }

                //解压文件
                CZipHelper.UnZip(fileName, CurDownFile.ZipFolder);

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                //修改当前版本记录

                ShowUpgradeVersion(CurDownFile.SoftWareVersion);

                CurDownFile.DownLoadFlag = true;
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }
            finally
            {
                SetUpgradeBtn(true);

                if (CurDownFile.DownLoadFlag)
                {
                    ShowLog("下载Zip文件OK:" + ((double)watcher.ElapsedMilliseconds / 1000).ToString("0.0") + "s", false);
                }
            }
        }
        #endregion

        #region 自动打包
        private void btnPacking_Click(object sender, EventArgs e)
        {
            try
            {
             
                SaveIniFile();

                if(!Directory.Exists(AutoUpgrade.PackageFolder))
                {
                    ShowLog("自动打包文件夹不存在", true);
                    return;
                }

                string zipFile = Path.GetDirectoryName(AutoUpgrade.PackageFolder) + "\\" + txtSoftWareVersion.Text + ".zip";

                CurVersion.ZipFile = zipFile;

                CurVersion.SoftWareName = Path.GetFileName(zipFile);

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }
                if (txtProjectNumber.Text == string.Empty)
                {
                    ShowLog("项目编号不能为空", true);
                    return;
                }
                if (txtSoftWareVersion.Text == string.Empty)
                {
                    ShowLog("当前版本不能为空", true);
                    return;
                }

                if (MessageBox.Show("确定上载文件当前版本为[" + txtSoftWareVersion.Text + "]?", "上载文件",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                btnPacking.Enabled = false;

                Task.Factory.StartNew(() => AutoUpLoadZipFile());
              
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }          
        }
        #endregion

        #region 自动升级版本
        private void btnUpgrading_Click(object sender, EventArgs e)
        {
            try
            {
                SaveIniFile();

                if (!checkFlag)
                {
                    ShowLog("请先连接Web接口", true);
                    return;
                }
                if (labProjectNumber.Text == string.Empty)
                {
                    ShowLog("项目编号不能为空", true);
                    return;
                }
                if (labSoftWareVersion.Text == string.Empty)
                {
                    ShowLog("当前版本不能为空", true);
                    return;
                }
                if (!Directory.Exists(AutoUpgrade.UpgradeFolder))
                {
                    ShowLog("自动升级文件夹", true);
                    return;
                }

                CurDownFile.ProjectNumber = labProjectNumber.Text;
                CurDownFile.SoftWareVersion = labSoftWareVersion.Text;
                CurDownFile.ZipFolder = AutoUpgrade.UpgradeFolder;
                CurDownFile.FilePageSize = System.Convert.ToInt32(cmbPageSize.Text) * 1024;
                CurDownFile.CurrentPageIndex = 0;

                string curVerion = txtSoftWareVersion.Text;

                string updateVersion = CurDownFile.SoftWareVersion;

                if (updateVersion.CompareTo(curVerion) < 1)
                {
                    MessageBox.Show("当前软件版本为[" + curVerion + "],不能更新为版本[" + updateVersion + "]", "软件自动升级",
                                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (MessageBox.Show("确定更新当前软件版本为[" + updateVersion + "]?", "软件自动升级",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                btnUpgrading.Enabled = false;

                Task.Factory.StartNew(() => AutoDownZipFile());
               
            }
            catch (Exception ex)
            {
                ShowLog(ex.ToString(), true);
            }     
        }
        #endregion
    }

    public class CVersionControl
    {
        #region 常量定义
        /// <summary>
        /// 数据包头
        /// </summary>
        private const string C_XML_HEADER = "GJMES";
        /// <summary>
        /// 输入表单名
        /// </summary>
        private const string C_XML_BASE = "XmlBase";
        /// <summary>
        /// 返回表单名
        /// </summary>
        private const string C_XML_PARA = "XmlPara";
        #endregion

        #region 类定义
        /// <summary>
        /// 上载类
        /// </summary>
        public class CUpgrade
        {
            public string WebUlr = string.Empty;

            public string ProjectNumber = string.Empty;

            public string ProjectName = string.Empty;

            public string ProjectCustom = string.Empty;

            public string SoftWareName = string.Empty;

            public string SoftWareVersion = string.Empty;

            public string SoftWareAuthor = string.Empty;

            public string SoftWareDate = string.Empty;

            public string ZipFile = string.Empty;

            public bool UpLoadFlag = false;

            public int FilePageSize = 1024 * 1024; //1MB

        }
        /// <summary>
        /// 下载类
        /// </summary>
        public class CDownFile
        {
            public string ProjectNumber = string.Empty;

            public string SoftWareName = string.Empty;

            public string SoftWareVersion = string.Empty;

            public string ZipFolder = string.Empty;
            /// <summary>
            /// 扇页大小
            /// </summary>
            public int FilePageSize = 1024 * 1024; //1MB
            /// <summary>
            /// 开始位置
            /// </summary>
            public int CurrentPageIndex = 0;
            /// <summary>
            /// 下载数量
            /// </summary>
            public int CurrenPageSize = 0;
            /// <summary>
            /// 下载标志
            /// </summary>
            public bool DownLoadFlag = false;
            /// <summary>
            /// 总字节数
            /// </summary>
            public int TotalPageBytes = 0;
            /// <summary>
            /// 剩余字节数
            /// </summary>
            public int LeftPageBytes = 0;
            /// <summary>
            /// 当前字节流数
            /// </summary>
            public int CurPageByteCount = 0;
            /// <summary>
            /// 当前字节流
            /// </summary>
            public StringBuilder CurByteStream = new StringBuilder();
            /// <summary>
            /// 校验和
            /// </summary>
            public int CheckSum = 0;
        }
        #endregion

        #region Web接口
        /// <summary>
        /// 服务地址
        /// </summary>
        public static string WebUlr = string.Empty;
        /// <summary>
        /// 同步锁
        /// </summary>
        private static ReaderWriterLock webLock = new ReaderWriterLock();
        /// <summary>
        /// 检查Web接口状态
        /// </summary>
        /// <param name="ulrWeb"></param>
        /// <param name="version"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool CheckSystem(string projectNumber, out string version, out string er)
        {
            version = string.Empty;

            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "CheckSystem";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("ProjectNumber");
                in_ds.Tables[C_XML_BASE].Rows.Add(
                                                  projectNumber
                                                 );

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                version = reponseXml;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 创建ZipFile
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool CreatZipFile(CUpgrade CurVersion, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "UpLoad";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("CommandType");
                in_ds.Tables[C_XML_BASE].Columns.Add("ProjectNumber");
                in_ds.Tables[C_XML_BASE].Columns.Add("ProjectName");
                in_ds.Tables[C_XML_BASE].Columns.Add("ProjectCustom");
                in_ds.Tables[C_XML_BASE].Columns.Add("SoftWareName");
                in_ds.Tables[C_XML_BASE].Columns.Add("SoftWareVersion");
                in_ds.Tables[C_XML_BASE].Columns.Add("SoftWareAuthor");
                in_ds.Tables[C_XML_BASE].Columns.Add("SoftWareDate");
                in_ds.Tables[C_XML_BASE].Columns.Add("SourcePackage");
                in_ds.Tables[C_XML_BASE].Columns.Add("UpgradePackage");
                in_ds.Tables[C_XML_BASE].Rows.Add(0,
                                                  CurVersion.ProjectNumber,
                                                  CurVersion.ProjectName,
                                                  CurVersion.ProjectCustom,
                                                  CurVersion.SoftWareName,
                                                  CurVersion.SoftWareVersion,
                                                  CurVersion.SoftWareAuthor,
                                                  CurVersion.SoftWareDate,
                                                  string.Empty,
                                                  string.Empty
                                                 );

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 上传Zip文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool UpLoadFile(CUpgrade CurVersion, string soureFile, string upgradeFile, out int checkSum, out string er)
        {
            er = string.Empty;

            checkSum = 0;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "UpLoad";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("CommandType");
                in_ds.Tables[C_XML_BASE].Columns.Add("ProjectNumber");
                in_ds.Tables[C_XML_BASE].Columns.Add("ProjectName");
                in_ds.Tables[C_XML_BASE].Columns.Add("ProjectCustom");
                in_ds.Tables[C_XML_BASE].Columns.Add("SoftWareName");
                in_ds.Tables[C_XML_BASE].Columns.Add("SoftWareVersion");
                in_ds.Tables[C_XML_BASE].Columns.Add("SoftWareAuthor");
                in_ds.Tables[C_XML_BASE].Columns.Add("SoftWareDate");
                in_ds.Tables[C_XML_BASE].Columns.Add("SourcePackage");
                in_ds.Tables[C_XML_BASE].Columns.Add("UpgradePackage");
                in_ds.Tables[C_XML_BASE].Rows.Add(1,
                                                  CurVersion.ProjectNumber,
                                                  CurVersion.ProjectName,
                                                  CurVersion.ProjectCustom,
                                                  CurVersion.SoftWareName,
                                                  CurVersion.SoftWareVersion,
                                                  CurVersion.SoftWareAuthor,
                                                  CurVersion.SoftWareDate,
                                                  soureFile,
                                                  upgradeFile
                                                 );
                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                DataSet dsXml = CXml.ConvertXMLToDataSet(reponseXml);

                if (dsXml != null)
                {
                    if (dsXml.Tables.Contains(C_XML_BASE))
                    {
                        checkSum = System.Convert.ToInt32(dsXml.Tables[C_XML_BASE].Rows[0]["CheckSum"].ToString());
                    }
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 上载文件检验
        /// </summary>
        public static bool VerifyZipFile(CUpgrade CurVersion, out int CheckSum, out string er)
        {
            er = string.Empty;

            CheckSum = 0;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "UpLoad";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("CommandType");
                in_ds.Tables[C_XML_BASE].Columns.Add("ProjectNumber");
                in_ds.Tables[C_XML_BASE].Columns.Add("ProjectName");
                in_ds.Tables[C_XML_BASE].Columns.Add("ProjectCustom");
                in_ds.Tables[C_XML_BASE].Columns.Add("SoftWareName");
                in_ds.Tables[C_XML_BASE].Columns.Add("SoftWareVersion");
                in_ds.Tables[C_XML_BASE].Columns.Add("SoftWareAuthor");
                in_ds.Tables[C_XML_BASE].Columns.Add("SoftWareDate");
                in_ds.Tables[C_XML_BASE].Columns.Add("SourcePackage");
                in_ds.Tables[C_XML_BASE].Columns.Add("UpgradePackage");
                in_ds.Tables[C_XML_BASE].Rows.Add(2,
                                                  CurVersion.ProjectNumber,
                                                  CurVersion.ProjectName,
                                                  CurVersion.ProjectCustom,
                                                  CurVersion.SoftWareName,
                                                  CurVersion.SoftWareVersion,
                                                  CurVersion.SoftWareAuthor,
                                                  CurVersion.SoftWareDate,
                                                  string.Empty,
                                                  string.Empty
                                                 );

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                DataSet dsXml = CXml.ConvertXMLToDataSet(reponseXml);

                if (dsXml != null)
                {
                    if (dsXml.Tables.Contains(C_XML_BASE))
                    {
                        CheckSum = System.Convert.ToInt32(dsXml.Tables[C_XML_BASE].Rows[0]["CheckSum"].ToString());
                    }
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 创建下载ZipFile
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool CreateDownLoadZip(CDownFile CurDownFile, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "DownLoad";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("CommandType");
                in_ds.Tables[C_XML_BASE].Columns.Add("ProjectNumber");
                in_ds.Tables[C_XML_BASE].Columns.Add("SoftWareVersion");
                in_ds.Tables[C_XML_BASE].Columns.Add("FilePageSize");
                in_ds.Tables[C_XML_BASE].Columns.Add("CurrenPageIndex");
                in_ds.Tables[C_XML_BASE].Rows.Add(0,
                                                  CurDownFile.ProjectNumber,
                                                  CurDownFile.SoftWareVersion,
                                                  CurDownFile.FilePageSize,
                                                  CurDownFile.CurrentPageIndex
                                                 );

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                CurDownFile.CurByteStream.Clear();

                DataSet dsXml = CXml.ConvertXMLToDataSet(reponseXml);

                if (dsXml != null)
                {
                    if (dsXml.Tables.Contains(C_XML_BASE))
                    {
                        CurDownFile.SoftWareName = dsXml.Tables[C_XML_BASE].Rows[0]["CurZipFile"].ToString();
                        CurDownFile.TotalPageBytes = System.Convert.ToInt32(dsXml.Tables[C_XML_BASE].Rows[0]["TotalPageBytes"].ToString());
                        CurDownFile.CurPageByteCount = System.Convert.ToInt32(dsXml.Tables[C_XML_BASE].Rows[0]["CurPageByteCount"].ToString());
                        CurDownFile.CurByteStream.Append(dsXml.Tables[C_XML_BASE].Rows[0]["CurByteStream"].ToString());
                        CurDownFile.CheckSum = System.Convert.ToInt32(dsXml.Tables[C_XML_BASE].Rows[0]["CheckSum"].ToString());
                        CurDownFile.LeftPageBytes = System.Convert.ToInt32(dsXml.Tables[C_XML_BASE].Rows[0]["LeftPageBytes"].ToString());
                    }
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 创建下载ZipFile
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool AppenDownLoadZip(CDownFile CurDownFile, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "DownLoad";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("CommandType");
                in_ds.Tables[C_XML_BASE].Columns.Add("ProjectNumber");
                in_ds.Tables[C_XML_BASE].Columns.Add("SoftWareVersion");
                in_ds.Tables[C_XML_BASE].Columns.Add("CurrenPageIndex");
                in_ds.Tables[C_XML_BASE].Columns.Add("FilePageSize");
                in_ds.Tables[C_XML_BASE].Rows.Add(1,
                                                  CurDownFile.ProjectNumber,
                                                  CurDownFile.SoftWareVersion,
                                                  CurDownFile.CurrentPageIndex,
                                                  CurDownFile.CurrenPageSize
                                                 );

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                CurDownFile.CurByteStream.Clear();

                DataSet dsXml = CXml.ConvertXMLToDataSet(reponseXml);

                if (dsXml != null)
                {
                    if (dsXml.Tables.Contains(C_XML_BASE))
                    {
                        CurDownFile.SoftWareName = dsXml.Tables[C_XML_BASE].Rows[0]["CurZipFile"].ToString();
                        CurDownFile.TotalPageBytes = System.Convert.ToInt32(dsXml.Tables[C_XML_BASE].Rows[0]["TotalPageBytes"].ToString());
                        CurDownFile.CurPageByteCount = System.Convert.ToInt32(dsXml.Tables[C_XML_BASE].Rows[0]["CurPageByteCount"].ToString());
                        CurDownFile.CheckSum = System.Convert.ToInt32(dsXml.Tables[C_XML_BASE].Rows[0]["CheckSum"].ToString());
                        CurDownFile.LeftPageBytes = System.Convert.ToInt32(dsXml.Tables[C_XML_BASE].Rows[0]["LeftPageBytes"].ToString());
                        CurDownFile.CurByteStream.Append(dsXml.Tables[C_XML_BASE].Rows[0]["CurByteStream"].ToString());
                    }
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region XML请求响应格式
        /// <summary>
        /// 应答请求消息
        /// </summary>
        /// <param name="strXml"></param>
        /// <param name="dataXml"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private static bool ReponseXml(string requestName, string requestXml, out string reponseXml, out DataSet ds, out string er)
        {
            reponseXml = string.Empty;

            er = string.Empty;

            ds = null;

            try
            {
                string dataXml = string.Empty;

                //格式化XML
                string strXML = FormatRequestXml(requestName, requestXml);

                if (!CNet.PostMessageToHttp(WebUlr, strXML, out reponseXml, out er))
                    return false;

                if (!FormatReponseXml(reponseXml, out dataXml, out er))
                    return false;

                if (dataXml == string.Empty)
                {
                    er = CLanguage.Lan("数据错误") + ":" + reponseXml;
                    return false;
                }

                reponseXml = dataXml;

                ds = CXml.ConvertXMLToDataSet(reponseXml);

                if (ds == null)
                {
                    er = CLanguage.Lan("数据错误") + ":" + reponseXml;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 初始化Http请求Xml
        /// </summary>
        /// <param name="requestName"></param>
        /// <param name="requestXml"></param>
        /// <returns></returns>
        private static string FormatRequestXml(string requestName, string requestXml)
        {
            try
            {
                requestXml = requestXml.Replace("&", "&amp;");
                requestXml = requestXml.Replace("<", "&lt;");
                requestXml = requestXml.Replace(">", "&gt;");
                requestXml = requestXml.Replace("'", "&apos;");
                requestXml = requestXml.Replace("\"", "&quot;"); //((char)34) 双引号

                string webXml = string.Empty;
                webXml += "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "\r\n";
                webXml += "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance" +
                          "\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema" + "\" xmlns:soap=\"" +
                          "http://schemas.xmlsoap.org/soap/envelope/" + "\">" + "\r\n";
                webXml += "<soap:Body>" + "\r\n";
                webXml += "<" + requestName + " xmlns=\"" + "http://tempuri.org/" + "\">" + "\r\n";
                webXml += "<xmlRequest>" + requestXml + "</xmlRequest>" + "\r\n";
                webXml += "</" + requestName + ">" + "\r\n";
                webXml += "</soap:Body>" + "\r\n";
                webXml += "</soap:Envelope>";
                return webXml;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 解析响应Xml数据
        /// </summary>
        /// <param name="reponseXml"></param>
        /// <param name="xmlData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private static bool FormatReponseXml(string reponseXml, out string xmlData, out  string er)
        {
            xmlData = string.Empty;

            er = string.Empty;

            try
            {
                string result = string.Empty;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reponseXml);
                XmlElement rootElem = doc.DocumentElement;   //获取根节点  
                XmlNodeList xnl = rootElem.ChildNodes;       //得到根节点的所有子节点
                if (xnl.Count == 0)
                {
                    er = "服务器应答超时:" + rootElem.InnerXml;
                    return false;
                }
                foreach (XmlNode xn1 in xnl)
                {
                    // 将节点转换为元素，便于得到节点的属性值
                    XmlElement xe = (XmlElement)xn1;
                    reponseXml = xe.InnerXml;
                    XmlDocument doc1 = new XmlDocument();
                    doc1.LoadXml(reponseXml);
                    XmlElement rootElem1 = doc1.DocumentElement;   //获取根节点
                    XmlNodeList xnl1 = rootElem1.ChildNodes;
                    if (xnl1.Count < 3)
                    {
                        er = "服务器应答数据错误:" + rootElem1.InnerXml;
                        return false;
                    }
                    xmlData = xnl1.Item(0).InnerText;
                    result = xnl1.Item(1).InnerText;
                    er = xnl1.Item(2).InnerText;
                }
                if (result.ToUpper() != "TRUE")
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        #endregion
    }
}
