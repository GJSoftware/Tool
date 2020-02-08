using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using GJ.COM;
using GJ.DEV.PLC;
using GJ.PDB;
using GJ.PLUGINS;
namespace GJ.TOOL
{
    public partial class FrmComPLC : Form,IChildMsg
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
            if (motorThread != null)
            {
                _dispose = true;

                while (_dispose)
                {
                    Application.DoEvents();
                }

                motorThread = null;
            }

            if (PLCThread != null)
            {
                PLCThread.SpinDown();
                PLCThread = null;
            }

            btnRun.Text = "启动";

            close();
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
        public FrmComPLC()
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
        private EPlcType _plcType = EPlcType.Inovance_TCP;
        private ERegType _regType = ERegType.D;
        private CPLCCOM _devPLC = null;
        private List<CPLCThread.CREG> _scanReg = null;
        private List<CPLCThread.CREG> _rReg = null;
        private List<CPLCThread.CREG> _wReg = null;
        private string iniFile = Application.StartupPath + "\\iniFile.ini";
        private string defaultDBFile = Application.StartupPath + "\\PLC.accdb";
        private bool loadDefault = false;
        private CPLCThread PLCThread = null;
        private Thread motorThread = null;
        #endregion

        #region 面板回调函数
        private void FrmComPLC_Load(object sender, EventArgs e)
        {
            string[] com = System.IO.Ports.SerialPort.GetPortNames();
            for (int i = 0; i < com.Length; i++)
                cmbCom.Items.Add(com[i]);
            if (com.Length > 0)
                cmbCom.SelectedIndex = 0;

            cmbPLCType.SelectedIndex = 0;
            cmbRegType.SelectedIndex = 0;

            cmbDataType.Items.Clear();
            cmbDataType.Items.Add(CLanguage.Lan("10进制数值"));
            cmbDataType.Items.Add(CLanguage.Lan("16进制数值"));
            cmbDataType.SelectedIndex = 0;

            defaultDBFile = CIniFile.ReadFromIni("ToolDebug", "PLC_DB", iniFile, defaultDBFile);

            _rRegRowMax = System.Convert.ToInt16(CIniFile.ReadFromIni("ToolDebug", "PLC_rRegRowMax", iniFile, "20"));

            _rRegColMax = System.Convert.ToInt16(CIniFile.ReadFromIni("ToolDebug", "PLC_rRegColMax", iniFile, "4"));

            _wRegRowMax = System.Convert.ToInt16(CIniFile.ReadFromIni("ToolDebug", "PLC_wRegRowMax", iniFile, "20"));

            _wRegColMax = System.Convert.ToInt16(CIniFile.ReadFromIni("ToolDebug", "PLC_wRegColMax", iniFile, "3"));

            txtAddr.Text = CIniFile.ReadFromIni("ToolDebug", "PLC_IP", iniFile, "192.168.3.101");

            cmbCom.Text = CIniFile.ReadFromIni("ToolDebug", "PLC_COM", iniFile, "COM1");

            labPlcDB.Text = defaultDBFile;

            onLoadDefaultDBHandler onLoadDefault = new onLoadDefaultDBHandler(onLoadDefaultDB);

            onLoadDefault.BeginInvoke(null, null); 
        }
        private void cmbPLCType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strType = cmbPLCType.Text;

            if (!Enum.IsDefined(typeof(EPlcType), strType))
            {
                MessageBox.Show(CLanguage.Lan("找不到对应程序集") + "[" + strType + "]", "Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _plcType = (EPlcType)Enum.Parse(typeof(EPlcType), strType);

            if (strType.EndsWith("TCP") || strType.EndsWith("UDP"))
            {
                if (strType.StartsWith("Inovance"))
                {
                    labBaud.Text = "PLC" + CLanguage.Lan("端口" + ":");
                    txtBaud.Text = "502";
                }
                else if (strType.StartsWith("FX"))
                {
                    labBaud.Text = "PLC"+ CLanguage.Lan("端口" + ":");
                    txtBaud.Text = "5002";
                }
                else
                {
                    labBaud.Text = "PLC"+ CLanguage.Lan("端口") + ":";
                    txtBaud.Text = "9600";
                }
            }
            else
            {
                txtAddr.Text = "1";
                labBaud.Text = CLanguage.Lan("串口波特率") + ":";                
                txtBaud.Text = "115200,E,7,2";  
            }

            close();

        }
        private void cmbRegType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strType = cmbRegType.Text;

            if (!Enum.IsDefined(typeof(ERegType), strType))
            {
                MessageBox.Show(CLanguage.Lan("找不到对应程序集") + "[" + strType + "]", "Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _regType = (ERegType)Enum.Parse(typeof(ERegType), strType);

            if (_regType == ERegType.WB)
            {
                txtLen.Text = "1";
                txtLen.Enabled = false; 
            }
            else
            {
                txtLen.Enabled = true; 
            }
        }
        private void cmbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDataType.SelectedIndex == 0)
            {
                labT1.Text = CLanguage.Lan("数值:低位在前,高位在后");
                labT2.Text = CLanguage.Lan("数值格式") + ":1;2;3";
                txtW.Text = "1;2;3"; 
            }
            else
            {
                labT1.Text =  CLanguage.Lan("字符:高位在前,低位在后");
                labT2.Text = CLanguage.Lan("字符格式") + ":0003 0002 0001";
                txtW.Text = "0003 0002 0001"; 
            }
        }
        private void btnCon_Click(object sender, EventArgs e)
        {
            try
            {
                btnCon.Enabled = false;

                string er=string.Empty;

                if (btnCon.Text == CLanguage.Lan("连接"))
                {
                    if (open(out er))
                    {
                        btnCon.Text = CLanguage.Lan("断开");

                        btnCon.ImageKey = "ON";

                        show(CLanguage.Lan("连接正常"));

                        CIniFile.WriteToIni("ToolDebug", "PLC_IP", txtAddr.Text, iniFile);

                        CIniFile.WriteToIni("ToolDebug", "PLC_COM", cmbCom.Text, iniFile);
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

                    if (PLCThread != null)
                    {
                        PLCThread.SpinDown();
                        PLCThread = null;
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
        private void btnW_Click(object sender, EventArgs e)
        {
            try
            {
                btnW.Enabled = false;

                string er = string.Empty;

                if (_devPLC == null)
                {
                    er = CLanguage.Lan("未连接PLC,请连接");
                    show(er, true);
                    return;
                }

                int plcAddr=0;

                if (!_plcType.ToString().EndsWith("TCP") && !_plcType.ToString().EndsWith("UDP"))
                    plcAddr=System.Convert.ToInt16(txtAddr.Text);

                int startAddr = System.Convert.ToInt16(txtStartAddr.Text);

                int startBin = System.Convert.ToInt16(txtBin.Text);

                int N = System.Convert.ToInt16(txtLen.Text);
               
                if (cmbDataType.SelectedIndex == 0) //数值操作
                {                  
                    string[] strArray = txtW.Text.Split(';');

                    int[] wVal = new int[strArray.Length];

                    for (int i = 0; i < strArray.Length; i++)
                        wVal[i] = System.Convert.ToInt32(strArray[i]);

                    if (wVal.Length > 1)
                    {
                        if (!_devPLC.Write(plcAddr, _regType, startAddr, wVal, out er))
                        {
                            show(CLanguage.Lan("写入错误") + ":" + er, true);
                        }
                        else
                        {
                            show(CLanguage.Lan("写入") + "OK");
                        }
                    }
                    else
                    {
                        if (!_devPLC.Write(plcAddr, _regType, startAddr, startBin, wVal[0], out er))
                        {
                            show(CLanguage.Lan("写入错误") + ":" + er, true);
                        }
                        else
                        {
                            show(CLanguage.Lan("写入") + "OK");
                        }
                    }
                }
                else                               //16进制字符操作
                {
                    string strHex = txtW.Text.Replace(" ", "");

                    if (!_devPLC.Write(plcAddr, _regType, startAddr, N, strHex, out er))
                    {
                        show(CLanguage.Lan("写入错误") + ":" + er, true);
                    }
                    else
                    {
                        show(CLanguage.Lan("写入")+ "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnW.Enabled = true;
            }
        }
        private void btnR_Click(object sender, EventArgs e)
        {
            try
            {
                btnR.Enabled = false; 

                  string er = string.Empty;

                if (_devPLC == null)
                {
                    er = CLanguage.Lan("未连接PLC,请连接");
                    show(er, true);
                    return;
                }

                int plcAddr=0;

                if (!_plcType.ToString().EndsWith("TCP") && !_plcType.ToString().EndsWith("UDP"))  
                    plcAddr=System.Convert.ToInt16(txtAddr.Text);

                int startAddr = System.Convert.ToInt16(txtStartAddr.Text);

                int startBin = System.Convert.ToInt16(txtBin.Text);

                int N = System.Convert.ToInt16(txtLen.Text);

                if (cmbDataType.SelectedIndex == 0) //数值操作
                {

                    int[] rVal = new int[N];

                    if (N > 1)
                    {
                        if (!_devPLC.Read(plcAddr, _regType, startAddr, ref rVal, out er))
                        {
                            show(CLanguage.Lan("读取错误") + ":" + er, true);
                        }
                        else
                        {
                            cmbVal.Items.Clear();
                            txtR.Text = "";                             
                            for (int i = 0; i < rVal.Length; i++)
                            {
                                txtR.Text += rVal[i];
                                if (i < rVal.Length - 1)
                                    txtR.Text += ";";
                                cmbVal.Items.Add(i.ToString() + ":" + rVal[i]); 
                            }
                            cmbVal.SelectedIndex = 0;
                            show(CLanguage.Lan("读取") +"OK");
                        }
                    }
                    else
                    {
                        int val = 0;

                        if (!_devPLC.Read(plcAddr, _regType, startAddr, startBin, out val, out er))
                        {
                            show(CLanguage.Lan("读取错误") + ":" + er, true);
                        }
                        else
                        {
                           cmbVal.Items.Clear();
                           cmbVal.Items.Add("0:" + val);  
                           txtR.Text =val.ToString();
                           cmbVal.SelectedIndex = 0;
                           show(CLanguage.Lan("读取") + "OK");
                        }
                    }
                }
                else                               //16进制字符操作
                {
                    string rData = string.Empty;

                    if (!_devPLC.Read(plcAddr, _regType, startAddr, N, out rData, out er))
                    {
                        show(CLanguage.Lan("读取错误") + ":" + er, true);
                    }
                    else
                    {
                        txtR.Text = rData;
                        show(CLanguage.Lan("读取") + "OK");
                    }                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnR.Enabled = true; 
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
                dlg.InitialDirectory = Application.StartupPath + "\\PlcLog";
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

                CIniFile.WriteToIni("ToolDebug", "PLC_DB", defaultDBFile, iniFile);

                CIniFile.WriteToIni("ToolDebug", "PLC_rRegRowMax", _rRegRowMax.ToString(), iniFile);

                CIniFile.WriteToIni("ToolDebug", "PLC_rRegColMax", _rRegColMax.ToString(), iniFile);

                CIniFile.WriteToIni("ToolDebug", "PLC_wRegRowMax", _wRegRowMax.ToString(), iniFile);

                CIniFile.WriteToIni("ToolDebug", "PLC_wRegColMax", _wRegColMax.ToString(), iniFile);

            }
            catch (Exception)
            {

                throw;
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
                    MessageBox.Show(CLanguage.Lan("正在初始化PLC寄存器,请稍等."), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string er = string.Empty;

                if (_devPLC == null)
                {
                    er = CLanguage.Lan("未连接PLC,请连接");
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

                    if (PLCThread == null)
                    {
                        PLCThread = new CPLCThread(_scanReg, _rReg, _wReg);

                        PLCThread.SpinUp(_devPLC);                      
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

                    if (PLCThread != null)
                    {
                        PLCThread.SpinDown();
                        PLCThread = null;
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

            if (_devPLC == null)
            {
                er = CLanguage.Lan("未连接PLC,请连接");
                show(er, true);
                return;
            }

            if (PLCThread == null)
            { 
               MessageBox.Show(CLanguage.Lan("请先启动监控"), "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return;
            }

            Button btn = (Button)sender;

            string des = btn.Name.Substring(3, btn.Name.Length - 3);

            int idNo=-1;

            for (int i = 0; i < _writeREG.Count; i++)
			{
			   if(des==_writeREG[i].des)
               {
                  idNo=i;
                  break;
               }
			}
            if(idNo==-1)
            {
               MessageBox.Show(CLanguage.Lan("寄存器") + "[" + des +"]" + CLanguage.Lan("不存在"),"Tip", 
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return;
            }

            if (_writeObj[idNo].GetType().ToString()  == "System.Windows.Forms.PictureBox")
            {
                if (btn.Text == "ON")
                {
                    PLCThread.addREGWrite(des, 1);
                    btn.Text = "---";
                    btn.Tag = "-1";
                }
                else if (btn.Text == "OFF")
                {
                    PLCThread.addREGWrite(des, 0);
                    btn.Text = "---";
                    btn.Tag = "-1";
                }
            }
            else
            {
                int val = System.Convert.ToInt32(((TextBox)_writeObj[idNo]).Text);
                PLCThread.addREGWrite(des, val);
                btn.Text = "---";
                btn.Tag = "-1";
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

                this.Invoke(new Action(() => {
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
                if (_plcType.ToString().EndsWith("TCP") || _plcType.ToString().EndsWith("UDP"))
                {
                    if (txtAddr.Text == "")
                    {
                        er = CLanguage.Lan("请输入PLC地址");
                        return false;
                    }
                    if (txtBaud.Text == "")
                    {
                        er =  CLanguage.Lan("请输入PLC端口");
                        return false;
                    }
                    comName = txtAddr.Text;
                    comSetting = txtBaud.Text;
                }
                else
                {
                    if (cmbCom.Text == "")
                    {
                        er =  CLanguage.Lan("请输入PLC串口");
                        return false;
                    }
                    if (txtAddr.Text == "")
                    {
                        er =  CLanguage.Lan("请输入PLC地址");
                        return false;
                    }
                    if (txtBaud.Text == "")
                    {
                        er =  CLanguage.Lan("请输入PLC端口");
                        return false;
                    }
                    comName = cmbCom.Text;
                    comSetting = txtBaud.Text;
                }

                if (_devPLC != null)
                {
                    _devPLC.Close();
                    _devPLC = null;
                }

                _devPLC = new CPLCCOM(_plcType);

                if (!_devPLC.Open(comName, out er, comSetting))
                {
                    _devPLC.Close();
                    _devPLC = null;
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
            if (_devPLC != null)
            {
                _devPLC.Close();
                _devPLC = null;
            }
            btnCon.Text =  CLanguage.Lan("连接");
            btnCon.ImageKey = "OFF";
            labStatus.Text =  CLanguage.Lan("断开连接");
            labStatus.ForeColor = Color.Black; 
        }
        private bool loadDB(string dbFile, out List<CPLCThread.CREG> scanReg, out List<CPLCThread.CREG> rReg,
                                           out List<CPLCThread.CREG> wReg, out string er)
        {

            scanReg = new List<CPLCThread.CREG>();

            rReg = new List<CPLCThread.CREG>();

            wReg = new List<CPLCThread.CREG>();

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
                string sqlCmd = "select * from scanREG order by idNo";

                if (!db.QuerySQL(sqlCmd, out ds, out er))
                    return false;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    CPLCThread.CREG reg = new CPLCThread.CREG();

                    reg.regName = ds.Tables[0].Rows[i]["regName"].ToString();

                    reg.regDes = reg.regName; 
  
                    if(Enum.IsDefined(typeof(ERegType),reg.regName.Substring(0,1)))
                        reg.regType = (ERegType)Enum.Parse(typeof(ERegType), reg.regName.Substring(0, 1));

                    reg.startAddr =System.Convert.ToInt32(reg.regName.Substring(1, reg.regName.Length - 1));

                    reg.regLen = System.Convert.ToInt16(ds.Tables[0].Rows[i]["regLen"].ToString());

                    scanReg.Add(reg); 

                }

                //读寄存器

                sqlCmd = "select * from rREG where regUsed=1 order by idNo";

                if (!db.QuerySQL(sqlCmd, out ds, out er))
                    return false;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    CPLCThread.CREG reg = new CPLCThread.CREG();

                    reg.regName = ds.Tables[0].Rows[i]["regName"].ToString();

                    reg.regDes = ds.Tables[0].Rows[i]["regDes"].ToString();

                    if (Enum.IsDefined(typeof(ERegType), reg.regName.Substring(0, 1)))
                        reg.regType = (ERegType)Enum.Parse(typeof(ERegType), reg.regName.Substring(0, 1));

                    string regAddr = reg.regName.Substring(1, reg.regName.Length - 1);

                    string[] arrayAddr = regAddr.Split('.');

                    if (arrayAddr.Length == 1)
                    {
                        reg.startAddr = System.Convert.ToInt32(reg.regName.Substring(1, reg.regName.Length - 1));

                        reg.startBin = 0;
                    }
                    else if (arrayAddr.Length == 2)
                    {
                        reg.startAddr = System.Convert.ToInt32(arrayAddr[0]);

                        reg.startBin = System.Convert.ToInt32(arrayAddr[1]);
                    }

                    reg.regLen = System.Convert.ToInt16(ds.Tables[0].Rows[i]["regLen"].ToString());

                    rReg.Add(reg);
                }

                //写寄存器

                sqlCmd = "select * from wREG where regUsed=1 order by idNo";

                if (!db.QuerySQL(sqlCmd, out ds, out er))
                    return false;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    CPLCThread.CREG reg = new CPLCThread.CREG();

                    reg.regName = ds.Tables[0].Rows[i]["regName"].ToString();

                    reg.regDes = ds.Tables[0].Rows[i]["regDes"].ToString();

                    if (Enum.IsDefined(typeof(ERegType), reg.regName.Substring(0, 1)))
                        reg.regType = (ERegType)Enum.Parse(typeof(ERegType), reg.regName.Substring(0, 1));

                    string regAddr=reg.regName.Substring(1, reg.regName.Length - 1);

                    string[] arrayAddr=regAddr.Split('.');
 
                    if(arrayAddr.Length==1) 
                    {
                       reg.startAddr = System.Convert.ToInt32(reg.regName.Substring(1, reg.regName.Length - 1));

                       reg.startBin =0;
                    }
                    else if (arrayAddr.Length == 2) 
                    {
                       reg.startAddr = System.Convert.ToInt32(arrayAddr[0]);

                       reg.startBin = System.Convert.ToInt32(arrayAddr[1]);
                    }

                    reg.regLen = System.Convert.ToInt16(ds.Tables[0].Rows[i]["regLen"].ToString());

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
        private void show(string info, bool alarm=false)
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
            public int type = 0;  //0:D 1:M
            public int addr = 0;
            public int len = 0;
            public int val = 0;
            public override string ToString()
            {
                return des+ "-["+  name +"]";
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
        private int _rRegRowMax = 20;
        private int _rRegColMax = 4;
        private int _wRegRowMax = 20;
        private  int _wRegColMax = 3;
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
        private void load_WR_REG(List<CPLCThread.CREG> rReg, List<CPLCThread.CREG> wReg)
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

                if (rReg[i].regType == ERegType.D)
                    reg.type = 0;
                else
                    reg.type = 1;

                reg.addr = rReg[i].startAddr;

                reg.len = rReg[i].regLen;

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

                if (wReg[i].regType == ERegType.D)
                    reg.type = 0;
                else
                    reg.type = 1;

                reg.addr = wReg[i].startAddr;

                reg.len = wReg[i].regLen;

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
                    btnReg.Name =  "wOp"  + reg.des;
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
            catch (Exception)
            {
                
                throw;
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
                        labData.Text =CLanguage.Lan("数值");
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

                    for (int z = i * pageNum; z < (i+1) * pageNum; z++)
                    {
                        if (_readLab.Count-1 < z)
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
            catch (Exception)
            {
                
                throw;
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
                            int val = PLCThread.rREG_Val(_readREG[i].des);

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
                            int val = PLCThread.wREG_Val(_writeREG[i].des);

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
                                else if (btn.Tag.ToString() != val.ToString() || btn.Text=="--")
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

                throw;
            }
            finally
            {
                _dispose = false;
            }        
        }
        #endregion

    }
}
