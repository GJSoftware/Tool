using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using GJ.COM;
using GJ.PLUGINS;
using GJ.DEV.Phoenix;

namespace GJ.TOOL
{
    public partial class FrmPhoenix : Form, IChildMsg
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
        public FrmPhoenix()
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
        private CProgrammer comMon = null;
        private Stopwatch watcher = new Stopwatch();
        private bool recvComplete = false;
        private string recvData = string.Empty;
        #endregion

        #region 面板调用函数
        private void FrmPhoenix_Load(object sender, EventArgs e)
        {
            string[] com = System.IO.Ports.SerialPort.GetPortNames();
            cmbCOM.Items.Clear();
            for (int i = 0; i < com.Length; i++)
                cmbCOM.Items.Add(com[i]);
            if (com.Length > 0)
                cmbCOM.Text = com[0];
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                btnOpen.Enabled = false;

                if (cmbCOM.Text == "")
                {
                    labStatus.Text = "请输入串口编号";
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                string er = string.Empty;

                if (comMon == null)
                {
                    comMon = new CProgrammer();

                    comMon.OnRecved += new OnRecvHandler(OnSerialPortRecv);

                    comMon.OnCompleteArgs.OnEvent += new COM.COnEvent<CRecvCompleteArgs>.OnEventHandler(OnComplete); 

                    if (!comMon.Open(cmbCOM.Text, out er, txtBand.Text))
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
                    txtBand.Enabled = false;
                }
                else
                {
                    comMon.OnRecved -= new OnRecvHandler(OnSerialPortRecv);
                    comMon.Close();
                    comMon = null;
                    btnOpen.Text = "打开";
                    labStatus.Text = "关闭串口.";
                    labStatus.ForeColor = Color.Blue;
                    cmbCOM.Enabled = true;
                    txtBand.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnOpen.Enabled = true;
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                btnSend.Enabled = false;

                if (comMon == null)
                {
                    labStatus.Text = "请确定串口是否打开?";
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                string er=string.Empty;

                if (!comMon.SendCmd(txtCmd.Text, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                labStatus.Text = "发送命令["+ txtCmd.Text +"]OK";

                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnSend.Enabled = true;
            }
        }
        private void btnRead_Click(object sender, EventArgs e)
        {
            string FirmWare = string.Empty;

            string HardWave = string.Empty;

            txtFirmWave.Text = FirmWare;

            txtHardWave.Text = HardWave; 

            try
            {
                btnRead.Enabled = false;

                if (comMon == null)
                {
                    labStatus.Text = "请确定串口是否打开?";
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                string er = string.Empty;
                
                //检测是否烧录器在传送数据?

                recvComplete = false;

                recvData = string.Empty;

                watcher.Restart();

                if (!comMon.SendCmd("", out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                while (!recvComplete)
                {
                    Application.DoEvents();

                    labStatus.Text = "烧录器忙碌中:" + ((double)watcher.ElapsedMilliseconds / 1000).ToString() + "s";

                    labStatus.ForeColor = Color.Blue;
                }
                
                //启动读版本命令

                recvComplete = false;

                recvData = string.Empty;

                watcher.Restart();

                if (!comMon.SendCmd(txtVersion.Text,out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                while (!recvComplete)
                {
                    Application.DoEvents();

                    labStatus.Text = "正在读取版本:" + ((double)watcher.ElapsedMilliseconds / 1000).ToString() + "s";

                    labStatus.ForeColor = Color.Blue;
                }

                if (!CalEnumerateData(recvData, out FirmWare, out HardWave, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                labStatus.Text = "读取产品版本OK:" + ((double)watcher.ElapsedMilliseconds / 1000).ToString() + "s";
                labStatus.ForeColor = Color.Blue;

            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                labTimes.Text = ((double)watcher.ElapsedMilliseconds / 1000).ToString();

                txtFirmWave.Text = FirmWare;

                txtHardWave.Text = HardWave; 

                btnRead.Enabled = true;
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                btnUpdate.Enabled = false;

                labModifyFirmware.Text = "";

                labModifyHardware.Text = "";

                if (comMon == null)
                {
                    labStatus.Text = "请确定串口是否打开?";
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                string er = string.Empty;

                recvComplete = false;

                recvData = string.Empty;

                int waitTime = System.Convert.ToInt32(txtWaitTimes.Text) * 1000;

                watcher.Restart();

                if (!comMon.SendCmd(txtUpdate.Text, out er, waitTime))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                
                while (!recvComplete)
                {
                    Application.DoEvents();

                    labStatus.Text = "正在升级版本:" + ((double)watcher.ElapsedMilliseconds / 1000).ToString() + "s";

                    labStatus.ForeColor = Color.Blue;
                }

                string modifyFirmware = string.Empty;

                string modifyHardware = string.Empty;

                if (!CalUpdateData(recvData,out modifyFirmware, out modifyHardware, out er))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                labModifyFirmware.Text = modifyFirmware;

                labModifyHardware.Text = modifyHardware;

                if (txtUpdatVersion.Text != modifyFirmware)
                {
                    labModifyFirmware.ForeColor = Color.Red;

                    labStatus.Text = "更新版本[" + txtUpdatVersion.Text  + "]与产品版本[" + modifyFirmware + "]不一致";
                    
                    labStatus.ForeColor = Color.Red;
                    
                    return;
                }

                labModifyFirmware.ForeColor = Color.Lime;

                labStatus.Text = "更新产品版本信息OK:" + ((double)watcher.ElapsedMilliseconds / 1000).ToString() + "s";

                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                labTimes.Text = ((double)watcher.ElapsedMilliseconds / 1000).ToString();

                btnUpdate.Enabled = true;
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            runLog.Clear();
        }
        #endregion

        #region 串口触发接收
        private void OnSerialPortRecv(object sender, CRecvArgs e)
        {
            Log(e.recvData);
        }
        private void Log(string recv)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string>(Log), recv);
            else
            {
                runLog.ScrollToCaret();
                runLog.AppendText(recv);
            }
        }
        private void OnComplete(object sender, GJ.DEV.Phoenix.CRecvCompleteArgs e)
        {
            recvData = e.rData;
            recvComplete = e.bComplete; 
        }
        #endregion

        #region 烧录器数据解析
        /// <summary>
        /// 解析读取信息字符串
        /// </summary>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool CalEnumerateData(string rData, out string FirmWare,out string HardWare, out string er)
        {
            FirmWare = string.Empty;

            HardWare = string.Empty;

            er = string.Empty;

            try
            {
                string[] rDataList = rData.Split('\n');

                List<string> rValList = new List<string>();

                for (int i = 0; i < rDataList.Length; i++)
                {
                    if (rDataList[i] != string.Empty)
                        rValList.Add(rDataList[i]); 
                }

                if (rValList.Count < 2)
                {
                    er = "烧录器通信异常,请检查烧录器连接是否正常?";
                    return false;
                }

                if (rValList[0].ToLower() != "b192 enumerate")
                {
                    er = "发送查询命令错误:"+ rValList[0];
                    return false;
                }

                if (rData.Contains("DFP (B192) Enumeration Pending"))
                {
                    er = "检测不到烧录产品,请确认已插上产品并通电?";
                    return false;
                }

                if (rData.Contains("b192SendVDM Failed"))
                {
                     er = "检测不到烧录产品,请确认已插上产品并通电?";
                     return false;
                }

                if (rValList.Count < 4)
                {
                    er = "接收到数据长度错误:" + rValList.Count.ToString();
                    return false;
                }

                for (int i = 0; i < rValList.Count; i++)
                {
                    string StrCmd = rValList[i].Replace("\0", "");

                    if (StrCmd.Contains("Firmware Version"))
                    {
                        string SOI = "Firmware Version[0x000000B0]: ";

                        FirmWare = StrCmd.Substring(SOI.Length, StrCmd.Length - SOI.Length);
                    }

                    if (StrCmd.Contains("Hardware Version"))
                    {
                        string SOI = "Hardware Version[0x000000C0]: ";

                        HardWare = StrCmd.Substring(SOI.Length, StrCmd.Length-SOI.Length);
                    }
                }

                if (rValList[rValList.Count - 1] != "__ENUMERATION_PASSED")
                {
                    er = "数据帧尾结果错误:" + rValList[rValList.Count - 1];

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
        /// 解析更新信息字符串
        /// </summary>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private bool CalUpdateData(string rData,out string FirmWare,out string HardWare, out string er)
        {
            er = string.Empty;

            FirmWare = string.Empty;

            HardWare = string.Empty;

            try
            {
                string[] rDataList = rData.Split('\n');

                List<string> rValList = new List<string>();

                for (int i = 0; i < rDataList.Length; i++)
                {
                    if (rDataList[i] != string.Empty)
                        rValList.Add(rDataList[i]);
                }

                if (rValList.Count < 2)
                {
                    er = "烧录器通信异常,请检查烧录器连接是否正常?";
                    return false;
                }

                if (rValList[0].ToLower() != "b192 update")
                {
                    er = "发送更新命令错误:" + rValList[0];
                    return false;
                }

                if (rData.Contains("DFP (B192) Enumeration Pending"))
                {
                    er = "检测不到烧录产品,请确认已插上产品并通电?";
                    return false;
                }

                //if (rData.Contains("b192SendVDM Failed"))
                //{
                //    er = "检测不到烧录产品,请确认已插上产品并通电?";
                //    return false;
                //}

                for (int i = 0; i < rValList.Count; i++)
                {
                    string StrCmd = rValList[i].Replace("\0", "");

                    if (StrCmd.Contains("Firmware Version"))
                    {
                        string SOI = "Firmware Version[0x000000B0]: ";

                        FirmWare = StrCmd.Substring(SOI.Length, StrCmd.Length - SOI.Length);
                    }

                    if (StrCmd.Contains("Hardware Version"))
                    {
                        string SOI = "Hardware Version[0x000000C0]: ";

                        HardWare = StrCmd.Substring(SOI.Length, StrCmd.Length - SOI.Length);
                    }
                }

                if (FirmWare == string.Empty)
                {
                    er = "烧录升级产品信息失败";
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
        #endregion

    }
}
