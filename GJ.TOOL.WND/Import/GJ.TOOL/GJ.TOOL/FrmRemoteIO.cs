using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using GJ.PLUGINS;
using GJ.DEV.RemoteIO;
using GJ.COM;
using GJ.PDB;
 
namespace GJ.TOOL
{
    public partial class FrmRemoteIO : Form,IChildMsg
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
                btnCon.Text = "打开";
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
        public FrmRemoteIO()
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

        #region 字段
        private EType _ioType = EType.IO_24_16;
        private CIOCom comMon = null;
        private List<CIOThread.CREG> _scanReg = null;
        private List<CIOThread.CREG> _rReg = null;
        private List<CIOThread.CREG> _wReg = null;
        private string iniFile = Application.StartupPath + "\\iniFile.ini";
        private string defaultDBFile = Application.StartupPath + "\\RemoteIO.accdb";
        private bool loadDefault = false;
        private CIOThread IOThread = null;
        private Thread motorThread = null;
        #endregion

        #region 面板回调函数
        private void FrmRemoteIO_Load(object sender, EventArgs e)
        {
            cmbCom.Items.Clear();
            cmbDevType.Items.Clear();
            cmbDevType.Items.Add("X");
            cmbDevType.Items.Add("Y");
            cmbDevType.SelectedIndex=0;
            string[] com = System.IO.Ports.SerialPort.GetPortNames();
            for (int i = 0; i < com.Length; i++)
                cmbCom.Items.Add(com[i]);
            if (com.Length > 0)
                cmbCom.Text = com[0];

            defaultDBFile = CIniFile.ReadFromIni("ToolDebug", "IO_DB", iniFile, defaultDBFile);

            _rRegRowMax = System.Convert.ToInt16(CIniFile.ReadFromIni("ToolDebug", "IO_rRegRowMax", iniFile, "20"));

            _rRegColMax = System.Convert.ToInt16(CIniFile.ReadFromIni("ToolDebug", "IO_rRegColMax", iniFile, "4"));

            _wRegRowMax = System.Convert.ToInt16(CIniFile.ReadFromIni("ToolDebug", "IO_wRegRowMax", iniFile, "20"));

            _wRegColMax = System.Convert.ToInt16(CIniFile.ReadFromIni("ToolDebug", "IO_wRegColMax", iniFile, "3"));

            labPlcDB.Text = defaultDBFile;

            onLoadDefaultDBHandler onLoadDefault = new onLoadDefaultDBHandler(onLoadDefaultDB);

            onLoadDefault.BeginInvoke(null, null); 
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                btnCon.Enabled = false;

                string er = string.Empty;

                if (btnCon.Text == CLanguage.Lan("连接"))
                {
                    if (open(out er))
                    {
                        btnCon.Text = CLanguage.Lan("断开");

                        btnCon.ImageKey = "ON";

                        show(CLanguage.Lan("连接正常"));
                    }
                    else
                    {
                        show(er, true);
                    }
                }
                else
                {

                    if (motorThread != null)
                    {
                        _dispose = true;

                        while (_dispose)
                        {
                            Application.DoEvents();
                        }

                        motorThread = null;
                    }

                    if (IOThread != null)
                    {
                        IOThread.SpinDown();
                        IOThread = null;
                    }

                    btnRun.Text = CLanguage.Lan("启动");

                    close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                btnCon.Enabled = true;
            }
        }
        private void btnSetAddr_Click(object sender, EventArgs e)
        {
            try
            {
                if (comMon == null)
                {
                    labStatus.Text = CLanguage.Lan("请确定已打开串口?");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtSetAddr.Text == "")
                {
                    labStatus.Text = CLanguage.Lan("请输入地址");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                string er = string.Empty;
                if (comMon.SetAddr(System.Convert.ToInt32(txtSetAddr.Text), out er))
                {
                    labStatus.Text = CLanguage.Lan("成功设置当前地址");
                    labStatus.ForeColor = Color.Blue;
                }
                else
                {
                    labStatus.Text = CLanguage.Lan("设置当前地址错误:")+ er;
                    labStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnReadAddr_Click(object sender, EventArgs e)
        {
            try
            {
                if (comMon == null)
                {
                    labStatus.Text = CLanguage.Lan("请确定已打开串口?");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtSetAddr.Text == "")
                {
                    labStatus.Text = CLanguage.Lan("请输入地址");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                int rData = 0;
                string er = string.Empty;
                if (comMon.ReadAddr(out rData, out er))
                {
                    txtSetAddr.Text = rData.ToString();
                    labStatus.Text = CLanguage.Lan("成功读取当前地址");
                    labStatus.ForeColor = Color.Blue;
                }
                else
                {
                    labStatus.Text = CLanguage.Lan("读取当前地址错误:") + er;
                    labStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnSetBaud_Click(object sender, EventArgs e)
        {
            try
            {
                if (comMon == null)
                {
                    labStatus.Text = CLanguage.Lan("请确定已打开串口?");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtSetBaud.Text == "")
                {
                    labStatus.Text = CLanguage.Lan("请输入波特率");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                string er = string.Empty;
                if (comMon.SetBaud(System.Convert.ToInt32(txtAddr.Text), System.Convert.ToInt32(txtSetBaud.Text), out er))
                {
                    labStatus.Text = CLanguage.Lan("成功设置当前波特率");
                    labStatus.ForeColor = Color.Blue;
                }
                else
                {
                    labStatus.Text = CLanguage.Lan("设置当前波特率错误:") + er;
                    labStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnReadBaud_Click(object sender, EventArgs e)
        {
            try
            {
                if (comMon == null)
                {
                    labStatus.Text = CLanguage.Lan("请确定已打开串口?");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtSetBaud.Text == "")
                {
                    labStatus.Text = CLanguage.Lan("请输入波特率");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                int rData = 0;
                string er = string.Empty;
                if (comMon.ReadBaud(System.Convert.ToInt32(txtAddr.Text), out rData, out er))
                {
                    if (rData > 57600)
                        rData = 115200;
                    txtSetBaud.Text = rData.ToString();
                    labStatus.Text = CLanguage.Lan("成功读取当前波特率");
                    labStatus.ForeColor = Color.Blue;
                }
                else
                {
                    labStatus.Text = CLanguage.Lan("读取当前波特率错误:") + er;
                    labStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnReadVer_Click(object sender, EventArgs e)
        {
            try
            {
                if (comMon == null)
                {
                    labStatus.Text = CLanguage.Lan("请确定已打开串口?");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                int rData = 0;
                string er = string.Empty;
                if (comMon.ReadVersion(System.Convert.ToInt32(txtAddr.Text), out rData, out er))
                {
                    txtSoftVer.Text = rData.ToString();
                    labStatus.Text = CLanguage.Lan("成功读取当前版本");
                    labStatus.ForeColor = Color.Blue;
                }
                else
                {
                    labStatus.Text = CLanguage.Lan("读取当前版本:") + er;
                    labStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnReadErr_Click(object sender, EventArgs e)
        {
            try
            {
                if (comMon == null)
                {
                    labStatus.Text = CLanguage.Lan("请确定已打开串口?");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                int rData = 0;
                string er = string.Empty;
                if (comMon.ReadErrCode(System.Convert.ToInt32(txtAddr.Text), out rData, out er))
                {
                    txtErrCode.Text = rData.ToString();
                    labStatus.Text = CLanguage.Lan("成功读取当前错误代码");
                    labStatus.ForeColor = Color.Blue;
                }
                else
                {
                    labStatus.Text = CLanguage.Lan("读取当前错误代码:") + er;
                    labStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnWrite_Click(object sender, EventArgs e)
        {
            try
            {
                if (comMon == null)
                {
                    labStatus.Text = CLanguage.Lan("请确定已打开串口?");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAddr.Text == "")
                {
                    labStatus.Text = CLanguage.Lan("请输入地址");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                string rData = string.Empty;
                string er = string.Empty;
                int devAddr = System.Convert.ToInt32(txtAddr.Text);
                ERegType devType = (ERegType)cmbDevType.SelectedIndex;
                int startAddr = System.Convert.ToInt32(txtStartAddr.Text);
                int N = System.Convert.ToInt32(txtLen.Text);
                if (N < 2)
                {
                    int val = System.Convert.ToInt32(txtData.Text);
                    if (!comMon.Write(devAddr, devType, startAddr, val, out er))
                    {
                        labStatus.Text = CLanguage.Lan("写入IO失败:") + er;
                        labStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                else
                {
                    string temp = txtData.Text;
                    if (temp.LastIndexOf(';') == (temp.Length - 1))
                        temp = temp.Substring(0, temp.Length - 1);
                    string[] valArray = temp.Split(';');
                    int[] val = new int[valArray.Length];
                    for (int i = 0; i < val.Length; i++)
                    {
                        if (valArray[i] != "")
                            val[i] = System.Convert.ToInt32(valArray[i]);
                    }
                    if (!comMon.Write(devAddr, devType, startAddr, val, out er))
                    {
                        labStatus.Text = CLanguage.Lan("写入IO失败:") + er;
                        labStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                labStatus.Text = CLanguage.Lan("写入IO成功");
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                if (comMon == null)
                {
                    labStatus.Text = CLanguage.Lan("请确定已打开串口?");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAddr.Text == "")
                {
                    labStatus.Text = CLanguage.Lan("请输入地址");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                cmbVal.Items.Clear();
                string rData = string.Empty;
                string er = string.Empty;
                int devAddr = System.Convert.ToInt32(txtAddr.Text);
                ERegType devType = (ERegType)cmbDevType.SelectedIndex;
                int startAddr = System.Convert.ToInt32(txtStartAddr.Text);
                int N = System.Convert.ToInt32(txtLen.Text);
                int[] rVal = new int[N];
                if (!comMon.Read(devAddr,devType, startAddr, ref rVal, out er))
                {
                    labStatus.Text = CLanguage.Lan("读取IO失败:") + er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                labRtn.Text = rData;
                for (int i = 0; i < rVal.Length; i++)
                {
                    cmbVal.Items.Add(i.ToString() + ":" + rVal[i].ToString());
                }
                if (rVal.Length > 0)
                    cmbVal.Text = "0:" + rVal[0].ToString();
                labStatus.Text = CLanguage.Lan("读取IO成功");
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }        
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                btnLoad.Enabled = false;

                if (!loadDefault)
                {
                    MessageBox.Show(CLanguage.Lan("正在初始化PLC寄存器,请稍等."), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                loadDefault = false;

                string er = string.Empty;

                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = Application.StartupPath;
                dlg.Filter = "DB files (*.accdb)|*.accdb";
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                labPlcDB.Text = dlg.FileName;

                defaultDBFile = labPlcDB.Text;

                if (!loadDB(labPlcDB.Text, out _scanReg, out _rReg, out _wReg, out er))
                {
                    MessageBox.Show(er, "Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                load_WR_REG(_rReg, _wReg);

                Initial_REG_Control();

                CIniFile.WriteToIni("ToolDebug", "IO_DB", defaultDBFile, iniFile);

                CIniFile.WriteToIni("ToolDebug", "IO_rRegRowMax", _rRegRowMax.ToString(), iniFile);

                CIniFile.WriteToIni("ToolDebug", "IO_rRegColMax", _rRegColMax.ToString(), iniFile);

                CIniFile.WriteToIni("ToolDebug", "IO_wRegRowMax", _wRegRowMax.ToString(), iniFile);

                CIniFile.WriteToIni("ToolDebug", "IO_wRegColMax", _wRegColMax.ToString(), iniFile);

            }
            catch (Exception)
            {

            }
            finally
            {
                loadDefault = true;

                btnLoad.Enabled = true;
            }
        }
        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                btnRun.Enabled = false;

                if (!loadDefault)
                {
                    MessageBox.Show(CLanguage.Lan("正在初始化IO寄存器,请稍等."), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                if (comMon == null)
                {
                    er = CLanguage.Lan("未连接IO,请连接");
                    show(er, true);
                    return;
                }

                if (btnRun.Text == CLanguage.Lan("启动"))
                {
                    if (_readREG.Count == 0 && _writeREG.Count == 0)
                    {
                        MessageBox.Show(CLanguage.Lan("没有可监控寄存器"), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    for (int i = 0; i < _writeBtn.Count; i++)
                        _writeBtn[i].Tag = "-1";

                    if (IOThread == null)
                    {
                        IOThread = new CIOThread(_scanReg, _rReg, _wReg);

                        IOThread.SpinUp(comMon);
                    }

                    if (motorThread == null)
                    {
                        _dispose = false;

                        motorThread = new Thread(OnMonitor);

                        motorThread.IsBackground = true;

                        motorThread.Start();
                    }

                    btnRun.Text = CLanguage.Lan("停止");
                }
                else
                {
                    if (motorThread != null)
                    {
                        _dispose = true;

                        while (_dispose)
                        {
                            Application.DoEvents();
                        }

                        motorThread = null;
                    }

                    if (IOThread != null)
                    {
                        IOThread.SpinDown();
                        IOThread = null;
                    }

                    btnRun.Text = CLanguage.Lan("启动");
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                btnRun.Enabled = true;
            }
        }
        private void onBtn_Click(object sender, EventArgs e)
        {
            string er = string.Empty;

            if (comMon == null)
            {
                er = CLanguage.Lan("未连接IO,请连接");
                show(er, true);
                return;
            }

            if (IOThread == null)
            {
                MessageBox.Show(CLanguage.Lan("请先启动监控"), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Button btn = (Button)sender;

            string des = btn.Name.Substring(3, btn.Name.Length - 3);

            int idNo = -1;

            for (int i = 0; i < _writeREG.Count; i++)
            {
                if (des == _writeREG[i].des)
                {
                    idNo = i;
                    break;
                }
            }
            if (idNo == -1)
            {
                MessageBox.Show(CLanguage.Lan("寄存器不存在") + "[" + des + "]", "Tip",
                                 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (_writeObj[idNo].GetType().ToString() == "System.Windows.Forms.PictureBox")
            {
                if (btn.Text == "ON")
                {
                    IOThread.AddIoWrite(des, 1);
                    btn.Text = "---";
                    btn.Tag = "-1";
                }
                else if (btn.Text == "OFF")
                {
                    IOThread.AddIoWrite(des, 0);
                    btn.Text = "---";
                    btn.Tag = "-1";
                }
            }
        }
        #endregion

        #region 委托
        private delegate void onLoadDefaultDBHandler();
        private void onLoadDefaultDB()
        {
            try
            {
                string er = string.Empty;

                if (!loadDB(labPlcDB.Text, out _scanReg, out _rReg, out _wReg, out er))
                    return;

                load_WR_REG(_rReg, _wReg);

                this.Invoke(new Action(() =>
                {
                    Initial_REG_Control();
                }));

            }
            catch (Exception)
            {

            }
            finally
            {
                loadDefault = true;
            }
        }
        #endregion

        #region 方法
        private bool open(out string er)
        {
            er = string.Empty;

            try
            {
                string comName = string.Empty;
                string comSetting = string.Empty;
                if (_ioType.ToString().EndsWith("TCP"))
                {
                    if (txtAddr.Text == "")
                    {
                        er = CLanguage.Lan("请输入地址");
                        return false;
                    }
                    if (txtBaud.Text == "")
                    {
                        er = CLanguage.Lan("请输入端口");
                        return false;
                    }
                    comName = txtAddr.Text;
                    comSetting = txtBaud.Text;
                }
                else
                {
                    if (cmbCom.Text == "")
                    {
                        er = CLanguage.Lan("请输入串口");
                        return false;
                    }
                    if (txtAddr.Text == "")
                    {
                        er = CLanguage.Lan("请输入地址");
                        return false;
                    }
                    if (txtBaud.Text == "")
                    {
                        er = "请输入PLC端口";
                        return false;
                    }
                    comName = cmbCom.Text;
                    comSetting = txtBaud.Text;
                }

                if (comMon != null)
                {
                    comMon.Close();
                    comMon = null;
                }

                comMon = new CIOCom(_ioType);

                if (!comMon.Open(comName, out er, comSetting))
                {
                    comMon.Close();
                    comMon = null;
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
        private void close()
        {
            if (comMon != null)
            {
                comMon.Close();
                comMon = null;
            }
            btnCon.Text = CLanguage.Lan("连接");
            btnCon.ImageKey = "OFF";
            labStatus.Text = CLanguage.Lan("断开连接");
            labStatus.ForeColor = Color.Black;
        }
        private bool loadDB(string dbFile, out List<CIOThread.CREG> scanReg, out List<CIOThread.CREG> rReg,
                                  out List<CIOThread.CREG> wReg, out string er)
        {

            scanReg = new List<CIOThread.CREG>();

            rReg = new List<CIOThread.CREG>();

            wReg = new List<CIOThread.CREG>();

            er = string.Empty;

            try
            {
                if (!System.IO.File.Exists(dbFile))
                {
                    er = CLanguage.Lan("找不到数据库文件") + "[" + Path.GetFileName(dbFile) + "]";
                    return false;
                }

                CDBCOM db = new CDBCOM(EDBType.Access, ".", dbFile);

                DataSet ds = null;

                //扫描寄存器
                string sqlCmd = "select * from scanIO order by idNo";

                if (!db.QuerySQL(sqlCmd, out ds, out er))
                    return false;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    CIOThread.CREG reg = new CIOThread.CREG();

                    reg.regName = ds.Tables[0].Rows[i]["IOName"].ToString();

                    reg.regDes = reg.regName;

                    reg.len = System.Convert.ToInt16(ds.Tables[0].Rows[i]["IOLen"].ToString());

                    reg.devAddr = System.Convert.ToInt16(reg.regName.Substring(0, 2));

                    if (Enum.IsDefined(typeof(ERegType), reg.regName.Substring(3, 1)))
                        reg.regType = (ERegType)Enum.Parse(typeof(ERegType), reg.regName.Substring(3, 1));

                    reg.regAddr = System.Convert.ToInt32(reg.regName.Substring(4, reg.regName.Length - 4));

                    scanReg.Add(reg);

                }

                //读寄存器

                sqlCmd = "select * from rIO where IOUsed=1 order by idNo";

                if (!db.QuerySQL(sqlCmd, out ds, out er))
                    return false;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    CIOThread.CREG reg = new CIOThread.CREG();

                    reg.regName = ds.Tables[0].Rows[i]["IOName"].ToString();

                    reg.regDes = ds.Tables[0].Rows[i]["IODesc"].ToString();

                    reg.devAddr = System.Convert.ToInt16(reg.regName.Substring(0, 2));

                    if (Enum.IsDefined(typeof(ERegType), reg.regName.Substring(3, 1)))
                        reg.regType = (ERegType)Enum.Parse(typeof(ERegType), reg.regName.Substring(3, 1));

                    reg.regAddr = System.Convert.ToInt32(reg.regName.Substring(4, reg.regName.Length - 4));

                    reg.len = 1;

                    rReg.Add(reg);
                }

                //写寄存器

                sqlCmd = "select * from wIO where IOUsed=1 order by idNo";

                if (!db.QuerySQL(sqlCmd, out ds, out er))
                    return false;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    CIOThread.CREG reg = new CIOThread.CREG();

                    reg.regName = ds.Tables[0].Rows[i]["IOName"].ToString();

                    reg.regDes = ds.Tables[0].Rows[i]["IODesc"].ToString();

                    reg.devAddr = System.Convert.ToInt16(reg.regName.Substring(0, 2));

                    if (Enum.IsDefined(typeof(ERegType), reg.regName.Substring(3, 1)))
                        reg.regType = (ERegType)Enum.Parse(typeof(ERegType), reg.regName.Substring(3, 1));

                    reg.regAddr = System.Convert.ToInt32(reg.regName.Substring(4, reg.regName.Length - 4));

                    reg.len = 1;

                    wReg.Add(reg);
                }

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        private void show(string info, bool alarm = false)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string, bool>(show), info, alarm);
            else
            {
                labStatus.Text = info;
                if (alarm)
                    labStatus.ForeColor = Color.Red;
                else
                    labStatus.ForeColor = Color.Blue;
            }
        }
        #endregion

        #region 寄存器扫描显示
        /// <summary>
        /// 寄存器类
        /// </summary>
        private class CREGBase
        {
            public string name = string.Empty;
            public string des = string.Empty;
            public int devAddr = 0;
            public int type = 0;  //0:D 1:M
            public int addr = 0;
            public int len = 0;
            public int val = 0;
            public override string ToString()
            {
                return des + "-[" + name + "]";
            }
        }
        /// <summary>
        /// 读寄存器面板
        /// </summary>
        private List<TableLayoutPanel> _rRegPanel = new List<TableLayoutPanel>();
        /// <summary>
        /// 写 寄存器面板
        /// </summary>
        private List<TableLayoutPanel> _wRegPanel = new List<TableLayoutPanel>();
        private const int _rRegRowSize = 24;
        private int _rRegRowMax = 16;
        private int _rRegColMax = 4;
        private int _wRegRowMax = 16;
        private int _wRegColMax = 3;
        /// <summary>
        /// 读寄存器
        /// </summary>
        private List<CREGBase> _readREG = new List<CREGBase>();
        /// <summary>
        /// 读寄存器标题
        /// </summary>
        private List<Label> _readLab = new List<Label>();
        /// <summary>
        /// 读寄存器数据
        /// </summary>
        private List<object> _readObj = new List<object>();
        /// <summary>
        /// 写寄存器
        /// </summary>
        private List<CREGBase> _writeREG = new List<CREGBase>();
        /// <summary>
        /// 写寄存器标题
        /// </summary>
        private List<Label> _writeLab = new List<Label>();
        /// <summary>
        /// 写寄存器数据
        /// </summary>
        private List<object> _writeObj = new List<object>();
        /// <summary>
        /// 写寄存器设置 
        /// </summary>
        private List<Button> _writeBtn = new List<Button>();
        /// <summary>
        /// 加载读写寄存器
        /// </summary>
        /// <param name="rReg"></param>
        private void load_WR_REG(List<CIOThread.CREG> rReg, List<CIOThread.CREG> wReg)
        {
            try
            {
                _readREG.Clear();
                _readLab.Clear();
                _readObj.Clear();
                //读寄存器
                for (int i = 0; i < rReg.Count; i++)
                {
                    CREGBase reg = new CREGBase();
                    reg.name = rReg[i].regName;
                    reg.des = rReg[i].regDes;
                    reg.devAddr = rReg[i].devAddr;
                    reg.type = 1;
                    reg.addr = rReg[i].regAddr;
                    reg.len = rReg[i].len;
                    _readREG.Add(reg);
                    //读寄存器标题
                    Label labTitle = new Label();
                    labTitle.Name = "rTi" + reg.des;
                    labTitle.Dock = DockStyle.Fill;
                    labTitle.TextAlign = ContentAlignment.MiddleLeft;
                    labTitle.Text = reg.ToString();
                    _readLab.Add(labTitle);
                    //读寄存器值
                    if (reg.type == 0)
                    {
                        Label labReg = new Label();
                        labReg.Name = "rDa" + reg.des;
                        labReg.Text = "0";
                        labReg.Dock = DockStyle.Fill;
                        labReg.Margin = new Padding(1);
                        labReg.TextAlign = ContentAlignment.MiddleCenter;
                        labReg.BorderStyle = BorderStyle.Fixed3D;
                        labReg.BackColor = Color.White;
                        _readObj.Add(labReg);
                    }
                    else
                    {
                        PictureBox picReg = new PictureBox();
                        picReg.Name = reg.des;
                        picReg.Dock = DockStyle.Fill;
                        picReg.Margin = new Padding(1);
                        picReg.Image = imageList1.Images["L"];
                        picReg.SizeMode = PictureBoxSizeMode.CenterImage;
                        _readObj.Add(picReg);
                    }
                }
                _writeREG.Clear();
                _writeLab.Clear();
                _writeObj.Clear();
                _writeBtn.Clear();
                //写寄存器
                for (int i = 0; i < wReg.Count; i++)
                {
                    CREGBase reg = new CREGBase();
                    reg.name = wReg[i].regName;
                    reg.des = wReg[i].regDes;
                    reg.devAddr = wReg[i].devAddr;
                    reg.type = 1;
                    reg.addr = wReg[i].regAddr;
                    reg.len = wReg[i].len;
                    _writeREG.Add(reg);
                    //写寄存器标题
                    Label labTitle = new Label();
                    labTitle.Name = "wTi" + reg.des;
                    labTitle.Dock = DockStyle.Fill;
                    labTitle.TextAlign = ContentAlignment.MiddleLeft;
                    labTitle.Text = reg.ToString();
                    _writeLab.Add(labTitle);
                    //写寄存器值
                    if (reg.type == 0)
                    {
                        TextBox txtReg = new TextBox();
                        txtReg.Name = "wDa" + reg.des;
                        txtReg.Text = "0";
                        txtReg.Dock = DockStyle.Fill;
                        txtReg.Margin = new Padding(1);
                        txtReg.TextAlign = HorizontalAlignment.Center;
                        _writeObj.Add(txtReg);

                        Button btnReg = new Button();
                        btnReg.Name = "wOp" + reg.des;
                        btnReg.Text = CLanguage.Lan("设置");
                        btnReg.Tag = "1";
                        btnReg.Dock = DockStyle.Fill;
                        btnReg.Margin = new Padding(0);
                        btnReg.TextAlign = ContentAlignment.MiddleCenter;
                        btnReg.Click += new EventHandler(onBtn_Click);
                        _writeBtn.Add(btnReg);
                    }
                    else
                    {
                        PictureBox picReg = new PictureBox();
                        picReg.Name = "wDa" + reg.des;
                        picReg.Dock = DockStyle.Fill;
                        picReg.Margin = new Padding(0);
                        picReg.Image = imageList1.Images["L"];
                        picReg.SizeMode = PictureBoxSizeMode.CenterImage;
                        _writeObj.Add(picReg);

                        Button btnReg = new Button();
                        btnReg.Name = "wOp" + reg.des;
                        btnReg.Text = "ON";
                        btnReg.Tag = "1";
                        btnReg.Dock = DockStyle.Fill;
                        btnReg.Margin = new Padding(0);
                        btnReg.TextAlign = ContentAlignment.MiddleCenter;
                        btnReg.Click += new EventHandler(onBtn_Click);
                        _writeBtn.Add(btnReg);
                    }
                }
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }          
        }
        /// <summary>
        /// 初始化寄存器容器
        /// </summary>
        private void Initial_REG_Control()
        {
            try
            {

                tbCtlr.TabPages.Clear();

                //初始化读寄存器面板 

                _rRegPanel.Clear();

                int pageNum = _rRegRowMax * _rRegColMax;

                int regPagNum = (_readREG.Count + pageNum - 1) / pageNum;

                for (int i = 0; i < regPagNum; i++)
                {
                    TabPage page = new TabPage();

                    page.BorderStyle = BorderStyle.Fixed3D;

                    page.Text = CLanguage.Lan("读寄存器") + (i + 1).ToString();

                    int pageRegNum = _readREG.Count % pageNum;

                    if (i < _readREG.Count / pageNum)
                        pageRegNum = pageNum;

                    TableLayoutPanel panel = new TableLayoutPanel();

                    panel.ColumnCount = _rRegColMax * 2;
                    for (int z = 0; z < _rRegColMax; z++)
                    {
                        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));
                    }

                    panel.RowCount = _rRegRowMax + 2;
                    panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
                    for (int z = 0; z < _rRegRowMax; z++)
                        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, _rRegRowSize));
                    panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

                    for (int z = 0; z < _rRegColMax; z++)
                    {
                        Label labREG = new Label();
                        labREG.Dock = DockStyle.Fill;
                        labREG.Text = CLanguage.Lan("寄存器功能描述");
                        labREG.TextAlign = ContentAlignment.MiddleCenter;
                        Label labData = new Label();
                        labData.Dock = DockStyle.Fill;
                        labData.Text = CLanguage.Lan("数值");
                        labData.TextAlign = ContentAlignment.MiddleCenter;
                        panel.Controls.Add(labREG, z * 2, 0);
                        panel.Controls.Add(labData, z * 2 + 1, 0);
                    }
                    panel.Dock = DockStyle.Fill;
                    panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                    panel.GetType().GetProperty("DoubleBuffered",
                                            System.Reflection.BindingFlags.Instance |
                                            System.Reflection.BindingFlags.NonPublic)
                                            .SetValue(panel, true, null);
                    page.Controls.Add(panel);

                    tbCtlr.TabPages.Add(page);

                    //加载寄存器

                    for (int z = i * pageNum; z < (i + 1) * pageNum; z++)
                    {
                        if (_readLab.Count - 1 < z)
                            break;

                        int regInPageNum = z % pageNum;

                        int regInPageRow = regInPageNum % _rRegRowMax + 1;

                        int regInPageCol = (regInPageNum / _rRegRowMax) * 2;

                        panel.Controls.Add(_readLab[z], regInPageCol, regInPageRow);

                        panel.Controls.Add((Control)_readObj[z], regInPageCol + 1, regInPageRow);

                    }
                }

                //初始化写寄存器面板

                _wRegPanel.Clear();

                pageNum = _wRegRowMax * _wRegColMax;

                regPagNum = (_writeREG.Count + pageNum - 1) / pageNum;

                for (int i = 0; i < regPagNum; i++)
                {
                    TabPage page = new TabPage();

                    page.BorderStyle = BorderStyle.Fixed3D;

                    page.Text = CLanguage.Lan("写寄存器") + (i + 1).ToString();

                    int pageRegNum = _writeREG.Count % pageNum;

                    if (i < _writeREG.Count / pageNum)
                        pageRegNum = pageNum;

                    TableLayoutPanel panel = new TableLayoutPanel();

                    panel.ColumnCount = _wRegColMax * 3;

                    for (int z = 0; z < _wRegColMax; z++)
                    {
                        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));
                        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));
                    }

                    panel.RowCount = _wRegRowMax + 2;

                    panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));

                    for (int z = 0; z < _wRegRowMax; z++)
                        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, _rRegRowSize));

                    panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

                    for (int z = 0; z < _wRegColMax; z++)
                    {
                        Label labREG = new Label();
                        labREG.Dock = DockStyle.Fill;
                        labREG.Text = CLanguage.Lan("寄存器功能描述");
                        labREG.TextAlign = ContentAlignment.MiddleCenter;
                        Label labData = new Label();
                        labData.Dock = DockStyle.Fill;
                        labData.Text = CLanguage.Lan("数值");
                        labData.TextAlign = ContentAlignment.MiddleCenter;
                        Label labOp = new Label();
                        labOp.Dock = DockStyle.Fill;
                        labOp.Text = CLanguage.Lan("操作");
                        labOp.TextAlign = ContentAlignment.MiddleCenter;
                        panel.Controls.Add(labREG, z * 3, 0);
                        panel.Controls.Add(labData, z * 3 + 1, 0);
                        panel.Controls.Add(labOp, z * 3 + 2, 0);
                    }
                    panel.Dock = DockStyle.Fill;
                    panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                    panel.GetType().GetProperty("DoubleBuffered",
                                            System.Reflection.BindingFlags.Instance |
                                            System.Reflection.BindingFlags.NonPublic)
                                            .SetValue(panel, true, null);
                    page.Controls.Add(panel);

                    tbCtlr.TabPages.Add(page);

                    //加载寄存器

                    for (int z = i * pageNum; z < (i + 1) * pageNum; z++)
                    {
                        if (_writeLab.Count - 1 < z)
                            break;

                        int regInPageNum = z % pageNum;

                        int regInPageRow = regInPageNum % _rRegRowMax + 1;

                        int regInPageCol = (regInPageNum / _rRegRowMax) * 3;

                        panel.Controls.Add(_writeLab[z], regInPageCol, regInPageRow);

                        panel.Controls.Add((Control)_writeObj[z], regInPageCol + 1, regInPageRow);

                        panel.Controls.Add(_writeBtn[z], regInPageCol + 2, regInPageRow);

                    }
                }
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }    
        }
        #endregion

        #region 寄存器线程
        /// <summary>
        /// 线程时间
        /// </summary>
        private int _delayMs = 100;
        /// <summary>
        /// 线程销毁
        /// </summary>
        private bool _dispose = false;
        /// <summary>
        /// 启动扫描线程
        /// </summary>
        private void OnMonitor()
        {
            try
            {
                while (true)
                {
                    if (_dispose)
                        return;

                    Thread.Sleep(_delayMs);

                    this.Invoke(new Action(() =>
                    {

                        //读寄存器
                        for (int i = 0; i < _readREG.Count; i++)
                        {
                            int val = IOThread.rIOVal[_readREG[i].des];

                            if (_readREG[i].type == 0) //D地址
                            {
                                Label lab = (Label)_readObj[i];

                                lab.Text = val.ToString();
                            }
                            else                     //位地址
                            {
                                PictureBox pic = (PictureBox)_readObj[i];
                                if (val == -1)                             //通信异常
                                    pic.Image = imageList1.Images["F"];
                                else if (val == 1)
                                    pic.Image = imageList1.Images["H"];
                                else
                                    pic.Image = imageList1.Images["L"];
                            }
                        }

                    }));

                    this.Invoke(new Action(() =>
                    {

                        //写寄存器
                        for (int i = 0; i < _writeREG.Count; i++)
                        {
                            int val = IOThread.wIOVal[_writeREG[i].des];

                            Button btn = (Button)_writeBtn[i];

                            if (_writeREG[i].type == 0) //D地址
                            {
                                TextBox txt = (TextBox)_writeObj[i];
                                if (val == -1)
                                {
                                    txt.Text = val.ToString();
                                    btn.Text = "--";
                                    btn.Tag = "1";
                                }
                                else if (btn.Tag.ToString() != val.ToString() || btn.Text == "--")
                                {
                                    txt.Text = val.ToString();
                                    btn.Text = CLanguage.Lan("设置");
                                    btn.Tag = val.ToString();
                                }
                            }
                            else                     //位地址
                            {
                                PictureBox pic = (PictureBox)_writeObj[i];
                                if (val == -1)
                                {
                                    pic.Image = imageList1.Images["F"];
                                    btn.Text = "--";
                                    btn.Tag = "1";
                                }
                                else
                                {
                                    if (btn.Tag.ToString() != val.ToString() || btn.Text == "--")
                                    {
                                        if (val == 1)
                                        {
                                            pic.Image = imageList1.Images["H"];
                                            btn.Text = "OFF";
                                        }
                                        else
                                        {
                                            pic.Image = imageList1.Images["L"];
                                            btn.Text = "ON";
                                        }
                                        btn.Tag = val.ToString();
                                    }
                                }
                            }
                        }
                    }));
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                _dispose = false;
            }
        }
        #endregion

        #region 扫描监控
        private int _rowNum = 0;
        private bool _scanStopFlag = false;
        private int _startAddr = 1;
        private int _endAddr = 36;
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
                    _startAddr = System.Convert.ToInt16(txtStAddr.Text);
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

                    int rVer = 0;

                    int rCode = 0;

                    if (!comMon.ReadVersion(i, out rVer, out er))
                    {
                        System.Threading.Thread.Sleep(50);

                        if (!comMon.ReadVersion(i, out rVer, out er))
                        {
                            result = false;
                        }
                    }

                    System.Threading.Thread.Sleep(50);

                    if (!comMon.ReadErrCode(i, out rCode, out er))
                    {
                        System.Threading.Thread.Sleep(50);

                        if (!comMon.ReadErrCode(i, out rCode, out er))
                        {
                            result = false;
                        }
                    }

                    ShowID(i, rVer, rCode, result);

                    _rowNum++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
        private void ShowID(int Addr, int rVer, int rSn, bool result)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<int, int, int, bool>(ShowID), Addr, rVer, rSn, result);
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

    }
}
