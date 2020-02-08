using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GJ.PLUGINS;
using GJ.DEV.Mon;
using GJ.COM;

namespace GJ.TOOL
{
    public partial class FrmGJMon32 : Form, IChildMsg
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
        public FrmGJMon32()
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

            labV = new Label[] { 
                             labV1, labV2, labV3, labV4, labV5, labV6, labV7, labV8, 
                             labV9, labV10, labV11, labV12, labV13, labV14, labV15, labV16, 
                             labV17, labV18, labV19, labV20, labV21, labV22, labV23, labV24, 
                             labV25, labV26, labV27, labV28, labV29, labV30, labV31, labV32
                             };
            txtOnOff = new TextBox[] { txtOnOff1, txtOnOff2, txtOnOff3, txtOnOff4 };
            txtOn = new TextBox[] { txtOn1, txtOn2, txtOn3, txtOn4 };
            txtOff = new TextBox[] { txtOff1, txtOff2, txtOff3, txtOff4 };
            LabX = new Label[] { labX1, labX2, labX3, labX4, labX5, labX6, labX7, labX8, labX9 };
        }
        /// <summary>
        /// 设置双缓冲,防止界面闪烁
        /// </summary>
        private void SetDoubleBuffered()
        {
            CUISetting.SetUIDoubleBuffered(this);
        }
        #endregion

        #region 面板控件
        private Label[] labV;
        private TextBox[] txtOnOff;
        private TextBox[] txtOn;
        private TextBox[] txtOff;
        private Label[] LabX;
        #endregion

        #region 字段
        private CMONCom comMon = null;
        private ERunMode runMode = ERunMode.自动线模式;
        private ESynON synOn = ESynON.异步; 
        #endregion

        #region 面板回调函数
        private void FrmGJMon32_Load(object sender, EventArgs e)
        {
            cmbCOM.Items.Clear();
            string[] com = System.IO.Ports.SerialPort.GetPortNames();
            for (int i = 0; i < com.Length; i++)
                cmbCOM.Items.Add(com[i]);
            if (com.Length > 0)
                cmbCOM.Text = com[0];
        }
        private void FrmGJMon32_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (comMon != null)
            {
                comMon.Close();
                comMon = null;
            }
        }       
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (cmbCOM.Text == "")
            {
                labStatus.Text = "请输入串口编号";
                labStatus.ForeColor = Color.Red;
                return;
            }
            string er = string.Empty;
            if (comMon == null)
            {
                comMon = new CMONCom(EType.MON32);

                if (!comMon.Open(cmbCOM.Text, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    comMon = null;
                    return;
                }                
                btnOpen.Text = "关闭";
                labStatus.Text = "成功打开串口.";
                labStatus.ForeColor = Color.Blue;
                cmbCOM.Enabled = false; 
            }
            else
            {
                comMon.Close();
                comMon = null;
                btnOpen.Text = "打开";
                labStatus.Text = "关闭串口.";
                labStatus.ForeColor = Color.Black;
                cmbCOM.Enabled = true; 
            }
        }
        private void btnSet_Click(object sender, EventArgs e)
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
        private void btnVer_Click(object sender, EventArgs e)
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
            int wAddr = System.Convert.ToInt16(txtAddr.Text);
            string ver = string.Empty;
            string er = string.Empty;
            if (!comMon.ReadVersion(wAddr, out ver, out er))
            {
                labStatus.Text = "读取模块版本失败:" + er;
                labStatus.ForeColor = Color.Red;
                return;
            }
            labVersion.Text = ver;
            labStatus.Text = "成功读取模块版本.";
            labStatus.ForeColor = Color.Blue;
        }
        private void btnVolt_Click(object sender, EventArgs e)
        {
            try
            {
                btnVolt.Enabled = false;

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
                int rOnOff = 0;
                List<double> monV = null;
                if (!comMon.ReadVolt(wAddr, out monV, out rOnOff, out er, synOn, runMode))
                {
                    labStatus.Text = "读取模块电压失败:" + er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                double Vmin = System.Convert.ToDouble(txtVmin.Text);
                double Vmax = System.Convert.ToDouble(txtVmax.Text);
                for (int i = 0; i < 32; i++)
                {
                    labV[i].Text = monV[i].ToString("0.000");
                    if (monV[i] >= Vmin && monV[i] <= Vmax)
                        labV[i].ForeColor = Color.Cyan;
                    else
                        labV[i].ForeColor = Color.Red;
                }
                labOnOff.Text = (rOnOff == 1) ? "ON" : "OFF";
                labStatus.Text = "成功读取模块电压值.";
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                btnVolt.Enabled = true;
            }            
        }        
        private void btnStart_Click(object sender, EventArgs e)
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
            CwRunPara wPara = new CwRunPara();
            wPara.runTolTime = 0;
            wPara.secMinCnt = 0;
            wPara.onoff_RunTime = 0;
            wPara.onoff_Cnt = 0;
            wPara.onoff_Flag = 1;
            wPara.onoff_YXDH = 1;
            wPara.runTypeFlag = 3;
            wPara.startFlag = 1;
            int wAddr = System.Convert.ToInt16(txtAddr.Text);
            string er = string.Empty;
            if (!comMon.SetRunStart(wAddr, wPara, out er))
            {
                labStatus.Text = "启动模块运行失败:" + er;
                labStatus.ForeColor = Color.Red;
                return;
            }
            labStatus.Text = "成功启动模块运行.";
            labStatus.ForeColor = Color.Blue;
        }
        private void btnStop_Click(object sender, EventArgs e)
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
            int wAddr = System.Convert.ToInt16(txtAddr.Text);
            string er = string.Empty;
            if (!comMon.ForceFinish(wAddr, out er))
            {
                labStatus.Text = "停止模块运行失败:" + er;
                labStatus.ForeColor = Color.Red;
                return;
            }
            labStatus.Text = "成功停止模块运行.";
            labStatus.ForeColor = Color.Blue;
        }
        private void btnSetPara_Click(object sender, EventArgs e)
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
            int wAddr = System.Convert.ToInt16(txtAddr.Text);
            string er = string.Empty;
            COnOffPara wPara = new COnOffPara();
            wPara.BIToTime = System.Convert.ToInt32(txtBIToTime.Text);
            for (int i = 0; i < 4; i++)
            {
                wPara.wOnOff[i] = System.Convert.ToInt32(txtOnOff[i].Text);
                wPara.wON[i] = System.Convert.ToInt32(txtOn[i].Text);
                wPara.wOFF[i] = System.Convert.ToInt32(txtOff[i].Text);
            }
            if (!comMon.SetOnOffPara(wAddr, wPara, out er))
            {
                labStatus.Text = "设置模块ONOFF参数失败:" + er;
                labStatus.ForeColor = Color.Red;
                return;
            }
            labStatus.Text = "成功设置模块ONOFF参数.";
            labStatus.ForeColor = Color.Blue;
        }
        private void btnReadPara_Click(object sender, EventArgs e)
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
            int wAddr = System.Convert.ToInt16(txtAddr.Text);

            string er = string.Empty;

            COnOffPara rPara = new COnOffPara();

            if (!comMon.ReadOnOffPara(wAddr, ref rPara, out er))
            {
                labStatus.Text = "读取模块ONOFF参数失败:" + er;
                labStatus.ForeColor = Color.Red;
                return;
            }

            txtBIToTime.Text = rPara.BIToTime.ToString();

            for (int i = 0; i < 4; i++)
            {
                txtOnOff[i].Text = rPara.wOnOff[i].ToString();
                txtOn[i].Text = rPara.wON[i].ToString();
                txtOff[i].Text = rPara.wOFF[i].ToString();
            }
            labStatus.Text = "成功读取模块ONOFF参数.";
            labStatus.ForeColor = Color.Blue;
        }
        private void btnReadSgn_Click(object sender, EventArgs e)
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
            int wAddr = System.Convert.ToInt16(txtAddr.Text);

            string er = string.Empty;

            CrRunPara rPara = new CrRunPara();

            if (!comMon.ReadRunData(wAddr, ref rPara, out er))
            {
                labStatus.Text = "读取模块控制信号失败:" + er;
                labStatus.ForeColor = Color.Red;
                return;
            }
            labRunMin.Text = rPara.runTolTime.ToString();
            labRunSec.Text = rPara.secMinCnt.ToString();
            labStartFlag.Text = rPara.startFlag.ToString();
            labFinishFlag.Text = rPara.biFinishFlag.ToString();
            if (rPara.onoff_Flag == 1)
            {
                labAcOn.Text = "ON";
                labRunOnOff.Text = "ON" + rPara.onoff_YXDH.ToString();
            }
            else
            {
                labAcOn.Text = "OFF";
                labRunOnOff.Text = "OFF" + rPara.onoff_YXDH.ToString();
            }
            labOnOffTime.Text = rPara.onoff_RunTime.ToString();
            labOnOffCycle.Text = rPara.onoff_Cnt.ToString();
            labRunFlag.Text = rPara.runTypeFlag.ToString();
            if (rPara.ac_Sync == 1)
                labRelayON.Text = "ON";
            else
                labRelayON.Text = "OFF";
            if (rPara.s1 != 1)
                labS1.BackColor = Color.Black;
            else
                labS1.BackColor = Color.Red;
            if (rPara.s2 == 1)
                labS2.BackColor = Color.Red;
            else
                labS2.BackColor = Color.Black;
            for (int i = 0; i < 9; i++)
            {
                if (rPara.x[i+1] != 1)
                    LabX[i].BackColor = Color.Black;
                else
                    LabX[i].BackColor = Color.Red;
            }
            labErrCode.Text = rPara.errCode.ToString();
            if (rPara.errCode == EErrCode.正常)
                labErrCode.ForeColor = Color.LightGreen;
            else
                labErrCode.ForeColor = Color.Red;
            labStatus.Text = "成功读取模块控制信号.";
            labStatus.ForeColor = Color.Blue;     
        }
        private void btnRlyON_Click(object sender, EventArgs e)
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
            int wAddr = System.Convert.ToInt16(txtAddr.Text);
            int relayNo = System.Convert.ToInt16(txtRelayNo.Text);
            string er = string.Empty;
            if (!comMon.SetRelayOn(wAddr, relayNo, out er))
            {
                labStatus.Text = "设置模块Relay ON失败:" + er;
                labStatus.ForeColor = Color.Red;
                return;
            }
            labStatus.Text = "成功设置模块Relay ON.";
            labStatus.ForeColor = Color.Blue;
        }
        private void btnRlyOff_Click(object sender, EventArgs e)
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
            int wAddr = System.Convert.ToInt16(txtAddr.Text);
            int relayNo = 101;
            string er = string.Empty;
            if (!comMon.SetRelayOn(wAddr, relayNo, out er))
            {
                labStatus.Text = "设置模块Relay OFF失败:" + er;
                labStatus.ForeColor = Color.Red;
                return;
            }
            labStatus.Text = "成功设置模块Relay OFF.";
            labStatus.ForeColor = Color.Blue;
        }
        private void btnACON_Click(object sender, EventArgs e)
        {
            try
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
                EOnOff onoff=EOnOff.OFF;
 
                if (btnACON.Text != "AC ON")                
                    onoff = EOnOff.ON;

                string er = string.Empty;

                int wAddr = System.Convert.ToInt16(txtAddr.Text);

                if (!comMon.RemoteACOnOff(wAddr, onoff, out er))
                {
                    labStatus.Text = "设置模块AC ON失败:" + er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                if (btnACON.Text == "AC ON")
                {
                    labStatus.Text = "成功设置AC ON.";
                    labStatus.ForeColor = Color.Blue;
                    btnACON.Text = "AC OFF";
                }
                else
                {
                    labStatus.Text = "成功设置AC OFF.";
                    labStatus.ForeColor = Color.Blue;
                    btnACON.Text = "AC ON";
                }

            }
            catch (Exception)
            {                
                throw;
            }
           
        }
        private void chkMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMode.Checked)
                runMode = ERunMode.普通老化房模式;
            else
                runMode = ERunMode.自动线模式; 
        }
        private void chkSync_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSync.Checked)
                synOn = ESynON.同步;
            else
                synOn = ESynON.异步;
        }
        private void btnScan_Click(object sender, EventArgs e)
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
            if (btnScan.Text == "启动扫描")
            {
                gridMon.Rows.Clear();
                gridMon.Refresh();
                curStep = 0;
                curAddr = System.Convert.ToInt16(txtStartAddr.Text);                
                btnScan.Text = "停止扫描";
                timer1.Interval = 20; 
                timer1.Enabled = true;
            }
            else
            {
                btnScan.Text = "启动扫描";
                timer1.Enabled = false;
            }
            int wStartAddr = System.Convert.ToInt16(txtStartAddr.Text);
            int wEndAddr = System.Convert.ToInt16(txtEndAddr.Text);
            if (wStartAddr > wEndAddr)
            {
                labStatus.Text = "结束地址需大于等于开始地址.";
                labStatus.ForeColor = Color.Red;
                return;
            }

        }
        private int curStep = 0;
        private int curAddr = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            int wEndAddr = System.Convert.ToInt16(txtEndAddr.Text);

            if (curAddr > wEndAddr)
            {
                btnScan.Text = "启动扫描";
                timer1.Enabled = false;
                return;
            }
            gridMon.RowCount++;
            int i = curStep;
            string er = string.Empty;
            string ver = string.Empty;
            gridMon.Rows[i].Cells[0].Value = curAddr.ToString();
            gridMon.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (comMon.ReadVersion(curAddr, out ver, out er))
            {
                gridMon.Rows[i].Cells[1].Value = "PASS";
                gridMon.Rows[i].Cells[1].Style.ForeColor = Color.Blue;
                gridMon.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                gridMon.Rows[i].Cells[2].Value = ver;
                int rOnOff = 0;
                List<double> volt = null;
                string temp = string.Empty;
                System.Threading.Thread.Sleep(5);
                if (comMon.ReadVolt(curAddr, out volt, out rOnOff, out er, synOn, runMode))
                {
                    for (int j = 0; j < 32; j++)
                        temp += (j + 1).ToString("D2") + ":" + volt[j].ToString("0.00");
                }
                if (rOnOff == 1)
                    gridMon.Rows[i].Cells[3].Value = "ON";
                else
                    gridMon.Rows[i].Cells[3].Value = "OFF";
                gridMon.Rows[i].Cells[4].Value = temp;
            }
            else
            {
                gridMon.Rows[i].Cells[1].Value = "FAIL";
                gridMon.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                gridMon.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            gridMon.CurrentCell = gridMon.Rows[i].Cells[0];
            gridMon.Refresh();
            if (curAddr < wEndAddr)
            {
                curAddr++;
                curStep++;
            }
            else
            {
                btnScan.Text = "启动扫描";
                timer1.Enabled = false;
                return;
            }
        }
        #endregion

       
    }
}
