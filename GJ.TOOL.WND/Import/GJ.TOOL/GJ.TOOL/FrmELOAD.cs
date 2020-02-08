using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GJ.COM;
using GJ.PLUGINS;
using GJ.DEV.ELOAD;

namespace GJ.TOOL
{
    public partial class FrmELOAD : Form, IChildMsg
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
        public FrmELOAD()
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
            cmbMode = new ComboBox[]{
                                 cmbMode1,cmbMode2,cmbMode3,cmbMode4,
                                 cmbMode5,cmbMode6,cmbMode7,cmbMode8
                                };
            txtVon = new TextBox[]{
                              txtVon1,txtVon2,txtVon3,txtVon4,
                              txtVon5,txtVon6,txtVon7,txtVon8
                             };
            txtLoad = new TextBox[]{
                               txtLoad1,txtLoad2,txtLoad3,txtLoad4,
                               txtLoad5,txtLoad6,txtLoad7,txtLoad8
                              };
            labCH = new Label[]{
                             labCH1,labCH2,labCH3,labCH4,
                             labCH5,labCH6,labCH7,labCH8
                            };
            labVs = new Label[]{
                           labVs1, labVs2,labVs3,labVs4,
                           labVs5,labVs6,labVs7,labVs8
                          };
            labV = new Label[]{
                           labV1,labV2,labV3,labV4,
                           labV5,labV6,labV7,labV8
                           };
            labCur = new Label[]{
                           labCur1,labCur2,labCur3,labCur4,
                           labCur5,labCur6,labCur7,labCur8
                           };
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
        private CELCom comMon = null;
        private ComboBox[] cmbMode;
        private TextBox[] txtVon;
        private TextBox[] txtLoad;
        private Label[] labCH;
        private Label[] labVs;
        private Label[] labV;
        private Label[] labCur;
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
        private int count = 0;
        #endregion

        #region 面板回调函数
        private void FrmELOAD_Load(object sender, EventArgs e)
        {
            cmbCom.Items.Clear();
            string[] com = System.IO.Ports.SerialPort.GetPortNames();
            for (int i = 0; i < com.Length; i++)
                cmbCom.Items.Add(com[i]);
            if (com.Length > 0)
                cmbCom.Text = com[0];
            cmdType.SelectedIndex = 0;

            for (int i = 0; i < cmbMode.Length; i++)
            {
                cmbMode[i].Items.Clear();
                cmbMode[i].Items.Add(EMode.CC.ToString());
                cmbMode[i].Items.Add(EMode.CV.ToString());
                cmbMode[i].Items.Add(EMode.LED.ToString());
                cmbMode[i].SelectedIndex = 0; 
            }
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                btnOpen.Enabled = false;

                if (cmbCom.Text == "")
                {
                    labStatus.Text = "请输入串口编号";
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                string er = string.Empty;

                if (comMon == null)
                {
                    if (!Enum.IsDefined(typeof(EType), cmdType.Text))
                    {
                        labStatus.Text = "找不到【" + cmdType.Text + "】类型";
                        labStatus.ForeColor = Color.Red;
                        return;
                    }

                    EType ddType = (EType)Enum.Parse(typeof(EType), cmdType.Text);

                    comMon = new CELCom(ddType, 0, cmdType.Text);

                    if (!comMon.Open(cmbCom.Text, out er,txtBaud.Text))
                    {
                        labStatus.Text = er;
                        labStatus.ForeColor = Color.Red;
                        comMon = null;
                        return;
                    }
                    btnOpen.Text = "关闭";
                    labStatus.Text = "成功打开串口.";
                    labStatus.ForeColor = Color.Blue;
                    cmbCom.Enabled = false;
                }
                else
                {
                    comMon.Close();
                    comMon = null;
                    btnOpen.Text = "打开";
                    labStatus.Text = "关闭串口.";
                    labStatus.ForeColor = Color.Blue;
                    cmbCom.Enabled = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                btnOpen.Enabled = true;
            }
        }
        private void btnSetAddr_Click(object sender, EventArgs e)
        {
            try
            {
                btnSetAddr.Enabled = false;
 
                if (comMon == null)
                {
                    labStatus.Text = "请确定串口是否打开?";
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAddr.Text == "")
                {
                    labStatus.Text = "请输入设置地址号.";
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                int wAddr = System.Convert.ToInt16(txtAddr.Text);
                string er = string.Empty;
                if (!comMon.SetNewAddr(wAddr, out er))
                {
                    labStatus.Text = "设置地址失败:" + er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                labStatus.Text = "设置新地址OK.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {
                throw;
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

                if (comMon == null)
                {
                    labStatus.Text = "请确定串口是否打开?";
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAddr.Text == "")
                {
                    labStatus.Text = "请输入设置地址号.";
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                int wAddr = System.Convert.ToInt16(txtAddr.Text);
                string er = string.Empty;
                CEL_SetPara wPara = new CEL_SetPara();
                for (int i = 0; i < comMon.maxCH; i++)
                {
                    wPara.Run_Mode[i] = (EMode)cmbMode[i].SelectedIndex;
                    wPara.Run_Val[i] = System.Convert.ToDouble(txtLoad[i].Text);
                    wPara.Run_Von[i] = System.Convert.ToDouble(txtVon[i].Text);
                }
                if (!comMon.SetELData(wAddr, wPara, out er))
                {
                    labStatus.Text = "设置模块负载参数失败:" + er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                labStatus.Text = "设置模块负载参数成功.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                btnSetLoad.Enabled = true;
            }            
        }
        private void btnReadSet_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadSet.Enabled = false;

                if (comMon == null)
                {
                    labStatus.Text = "请确定串口是否打开?";
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAddr.Text == "")
                {
                    labStatus.Text = "请输入设置地址号.";
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                int wAddr = System.Convert.ToInt16(txtAddr.Text);
                string er = string.Empty;
                CEL_ReadSetPara rPara = new CEL_ReadSetPara();
                if (!comMon.ReadELLoadSet(wAddr, rPara, out er))
                {
                    labStatus.Text = "读取模块负载参数失败:" + er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                labStatus.Text = "读取模块负载参数成功.";
                labStatus.ForeColor = Color.Blue;
                for (int i = 0; i < comMon.maxCH; i++)
                {
                    cmbMode[i].SelectedIndex = (int)rPara.LoadMode[i];
                    txtLoad[i].Text = rPara.LoadVal[i].ToString("0.0");
                    txtVon[i].Text = rPara.Von[i].ToString("0.0");
                }
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red; 
            }
            finally
            {
                btnReadSet.Enabled = true;
            }
        }
        private void btnReadData_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadData.Enabled = false;

                if (comMon == null)
                {
                    labStatus.Text = "请确定串口是否打开?";
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAddr.Text == "")
                {
                    labStatus.Text = "请输入设置地址号.";
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                int wAddr = System.Convert.ToInt16(txtAddr.Text);
                string er = string.Empty;
                CEL_ReadData rData = new CEL_ReadData();
                if (!comMon.ReadELData(wAddr, rData, out er))
                {
                    labStatus.Text = "读取模块数据失败:" + er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                labStatus.Text = "读取模块数据成功.";
                labStatus.ForeColor = Color.Blue;
                for (int i = 0; i < comMon.maxCH; i++)
                {
                    labV[i].Text = rData.Volt[i].ToString("0.00");
                    labVs[i].Text = rData.Vs[i].ToString("0.00");
                    labCur[i].Text = rData.Load[i].ToString("0.00");
                }
                labSatus.Text = rData.Status;
                labOnOff.Text = rData.ONOFF.ToString();
                labT0.Text = rData.NTC_0.ToString();
                labT1.Text = rData.NTC_1.ToString();
                labOCP.Text = rData.OCP.ToString();
                labOPP.Text = rData.OPP.ToString();
                labOTP.Text = rData.OTP.ToString();
                labOVP.Text = rData.OVP.ToString();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                btnReadData.Enabled = true;
            }
        }
        private void btnMon_Click(object sender, EventArgs e)
        {
            try
            {
                btnMon.Enabled = false;

                if (comMon == null)
                {
                    labStatus.Text = "请确定串口是否打开?";
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAddr.Text == "")
                {
                    labStatus.Text = "请输入设置地址号.";
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                if (btnMon.Text == "监控")
                {
                    count = 0;
                    timer1.Enabled = true;
                    btnMon.Text = "停止";
                }
                else
                {
                    timer1.Enabled = false;
                    btnMon.Text = "监控";
                }
             
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnMon.Enabled = true;
            }
        }
        private void btnScan_Click(object sender, EventArgs e)
        {
            try
            {               
                if (btnScan.Text == "扫描")
                {                 
                    if (comMon == null)
                    {
                        labStatus.Text = "请确定串口是否打开?";
                        labStatus.ForeColor = Color.Red;
                        return;
                    }
                    if (txtAddr.Text == "")
                    {
                        labStatus.Text = "请输入设置地址号.";
                        labStatus.ForeColor = Color.Red;
                        return;
                    }
                    int wStartAddr = System.Convert.ToInt16(txtStartAddr.Text);                
                    int wEndAddr = System.Convert.ToInt16(txtEndAddr.Text);
                    if (wStartAddr >= wEndAddr)
                    {
                        labStatus.Text = "末尾地址需大于等于设置地址.";
                        labStatus.ForeColor = Color.Red;
                        return;
                    }                    
                    curAddr = System.Convert.ToInt16(txtStartAddr.Text);
                    gridMon.Rows.Clear();
                    rowNum = 0;
                    cancel = false;
                    scanHandler scan = new scanHandler(OnScan);
                     scan.BeginInvoke(null, null); 
                    btnScan.Text = "停止";
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
            finally
            {
               
            }
        }
        private void cmdType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Enum.IsDefined(typeof(EType), cmdType.Text))
            {
                labStatus.Text = "找不到【" + cmdType.Text + "】类型";
                labStatus.ForeColor = Color.Red;
                return;
            }

            EType ddType = (EType)Enum.Parse(typeof(EType), cmdType.Text);
           
            string str = ddType.ToString();

            int maxCH = System.Convert.ToInt16(str.Substring(str.Length - 2, 2));

            SetCH(maxCH);

            if (ddType == EType.EL_40_08)
                txtBaud.Text = "38400,N,8,1";
            else
                txtBaud.Text = "57600,N,8,1";
        }
        private void SetCH(int maxCH)
        {
            for (int i = 0; i < labCH.Length; i++)
            {
                if (i < maxCH)
                {
                    txtVon[i].Enabled = true;
                    txtLoad[i].Enabled = true;
                    cmbMode[i].Enabled = true;
                    labVs[i].Enabled = true;
                    labV[i].Enabled = true;
                    labCur[i].Enabled = true;
                    labVs[i].BackColor = Color.White;
                    labV[i].BackColor = Color.White;
                    labCur[i].BackColor = Color.White;
                }
                else
                {
                    txtVon[i].Enabled = false;
                    txtLoad[i].Enabled = false;
                    cmbMode[i].Enabled = false;
                    labVs[i].Enabled = false;
                    labV[i].Enabled = false;
                    labCur[i].Enabled = false;
                    labVs[i].BackColor = Button.DefaultBackColor;
                    labV[i].BackColor = Button.DefaultBackColor;
                    labCur[i].BackColor = Button.DefaultBackColor;
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                int wAddr = System.Convert.ToInt16(txtAddr.Text);
                
                string er = string.Empty;
                
                CEL_ReadData rData = new CEL_ReadData();
                
                if (!comMon.ReadELData(wAddr, rData, out er))
                {
                    labStatus.Text = "读取模块数据失败:" + er + "-->" + (count++).ToString();
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                
                labStatus.Text = "读取模块数据成功-->" + (count++).ToString();
                
                labStatus.ForeColor = Color.Blue;
                
                for (int i = 0; i < comMon.maxCH; i++)
                {
                    labV[i].Text = rData.Volt[i].ToString("0.00");
                    labVs[i].Text = rData.Vs[i].ToString("0.00");
                    labCur[i].Text = rData.Load[i].ToString("0.00");
                }
                
                labSatus.Text = rData.Status;
                
                labOnOff.Text = rData.ONOFF.ToString();
                
                labT0.Text = rData.NTC_0.ToString();
                
                labT1.Text = rData.NTC_1.ToString();
                
                labOCP.Text = rData.OCP.ToString();
                
                labOPP.Text = rData.OPP.ToString();
                
                labOTP.Text = rData.OTP.ToString();
                
                labOVP.Text = rData.OVP.ToString();
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region 异步扫描
        private delegate void scanHandler();
        private void OnScan()
        {
            while (true)
            { 
                 if (cancel)
                    return;

                 System.Threading.Thread.Sleep(50);

                 string er = string.Empty;

                 bool result = true;

                 string status = string.Empty;

                 string strVal = string.Empty;

                 CEL_ReadData rData = new CEL_ReadData();

                 if (comMon.ReadELData(curAddr, rData, out er))
                 {
                     status += rData.ONOFF.ToString() + "|";
                     if (rData.Status != "OK")
                         status += rData.Status;
                     for (int j = 0; j < comMon.maxCH; j++)
                     {
                         strVal += rData.Volt[j].ToString("0.00") + "V-";
                         strVal += rData.Load[j].ToString("0.00") + "A|";
                     }
                 }
                 else
                 {
                     result = false;                     
                 }

                 ShowGrid(curAddr, result, status, strVal);

                 if (curAddr < System.Convert.ToInt16(txtEndAddr.Text))
                 {
                     curAddr++;
                     rowNum++;
                 }
                 else
                 {
                     ShowEnd();
                     return;
                 }
            }
        }
        private void ShowGrid(int addr, bool result,string status, string data)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<int, bool, string, string>(ShowGrid), addr, result, status, data);
            else
            {
                if (result)
                {
                    gridMon.Rows.Add(curAddr, "PASS", status, data);
                    gridMon.Rows[rowNum].Cells[1].Style.BackColor = Color.LimeGreen;
                }
                else
                {
                    gridMon.Rows.Add(curAddr, "FAIL", status, data);
                    gridMon.Rows[rowNum].Cells[1].Style.BackColor = Color.Red;
                }
                gridMon.CurrentCell = gridMon.Rows[gridMon.Rows.Count - 2].Cells[0];
            }
        }
        private void ShowEnd()
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(ShowEnd));
            else
            {
                btnScan.Text = "扫描";
            }
        }
        #endregion



    }
}
