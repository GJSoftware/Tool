using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using GJ.PLUGINS;
using GJ.DEV.I2C;
namespace GJ.TOOL
{
    public partial class FrmI2C : Form, IChildMsg
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
            if (BtnReadData.Text != "Start Read")
            {
                cancelMonitor = true;

                while (cancelMonitor)
                    Application.DoEvents();
            }

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
        public FrmI2C()
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
            for (int i = 0; i < Max_CmdNum; i++)
            {
                TextBox txt1 = new TextBox();
                txt1.Dock = DockStyle.Fill;
                txt1.Margin = new Padding(1);
                txt1.TextAlign = HorizontalAlignment.Center;
                txt1.Text = "00";

                TextBox txt2 = new TextBox();
                txt2.Dock = DockStyle.Fill;
                txt2.Margin = new Padding(1);
                txt2.TextAlign = HorizontalAlignment.Center;
                txt2.Text = Reg_Name[i];

                Label lab1 = new Label();
                lab1.Dock = DockStyle.Fill;
                lab1.TextAlign = ContentAlignment.MiddleCenter;
                lab1.Margin = new Padding(1);
                lab1.BackColor = Color.Black;
                lab1.ForeColor = Color.Cyan;

                Label lab2 = new Label();
                lab2.Dock = DockStyle.Fill;
                lab2.TextAlign = ContentAlignment.MiddleCenter;
                lab2.Margin = new Padding(1);
                lab2.BackColor = Color.Black;
                lab2.ForeColor = Color.Cyan;

                Label lab3 = new Label();
                lab3.Dock = DockStyle.Fill;
                lab3.TextAlign = ContentAlignment.MiddleCenter;
                lab3.Margin = new Padding(1);
                lab3.BackColor = Color.Black;
                lab3.ForeColor = Color.Cyan;

                Label lab4 = new Label();
                lab4.Dock = DockStyle.Fill;
                lab4.TextAlign = ContentAlignment.MiddleCenter;
                lab4.Margin = new Padding(1);
                lab4.BackColor = Color.Black;
                lab4.ForeColor = Color.Cyan;

                txtCmdNo.Add(txt1);
                txtRegNo.Add(txt2);
                labReadStatus.Add(lab1);
                labByte1.Add(lab2);
                labByte0.Add(lab3);
                labValue.Add(lab4);

                panel3.Controls.Add(txtCmdNo[i], 0, i + 1);
                panel3.Controls.Add(txtRegNo[i], 1, i + 1);
                panel3.Controls.Add(labReadStatus[i], 2, i + 1);
                panel3.Controls.Add(labByte1[i], 3, i + 1);
                panel3.Controls.Add(labByte0[i], 4, i + 1);
                panel3.Controls.Add(labValue[i], 5, i + 1); 
            }   
        }
        /// <summary>
        /// 设置双缓冲,防止界面闪烁
        /// </summary>
        private void SetDoubleBuffered()
        {
            splitContainer1.Panel1.GetType().GetProperty("DoubleBuffered",
                                           System.Reflection.BindingFlags.Instance |
                                           System.Reflection.BindingFlags.NonPublic)
                                           .SetValue(splitContainer1.Panel1, true, null);

            splitContainer1.Panel2.GetType().GetProperty("DoubleBuffered",
                                           System.Reflection.BindingFlags.Instance |
                                           System.Reflection.BindingFlags.NonPublic)
                                           .SetValue(splitContainer1.Panel2, true, null);

            panel1.GetType().GetProperty("DoubleBuffered",
                                           System.Reflection.BindingFlags.Instance |
                                           System.Reflection.BindingFlags.NonPublic)
                                           .SetValue(panel1, true, null);

            panel2.GetType().GetProperty("DoubleBuffered",
                                           System.Reflection.BindingFlags.Instance |
                                           System.Reflection.BindingFlags.NonPublic)
                                           .SetValue(panel2, true, null);

            panel3.GetType().GetProperty("DoubleBuffered",
                                           System.Reflection.BindingFlags.Instance |
                                           System.Reflection.BindingFlags.NonPublic)
                                           .SetValue(panel3, true, null);

        }
        #endregion

        #region 字段
        private const int Max_CmdNum = 12;
        private string[] Reg_Name = new string[]{
                                                  "10","11","20","21",
                                                   "42","44","46","40",
                                                   "00","00","00","00",
                                                   "00","00","00","00"
                                                  };
        /// <summary>
        /// 快充板
        /// </summary>
        private CI2CCom comMon = null;
        /// <summary>
        /// 取消监控
        /// </summary>
        private bool cancelMonitor = false;
        /// <summary>
        /// 产品监控
        /// </summary>
        private int uutNo = 0;
        /// <summary>
        /// 地址
        /// </summary>
        private int I2C_Addr = 0;
        /// <summary>
        /// 延时MS
        /// </summary>
        private int delayMs = 5000;
        /// <summary>
        /// 扫描次数
        /// </summary>
        private int scanNum = 0;
        #endregion

        #region 面板控件
        private List<TextBox> txtCmdNo = new List<TextBox>();
        private List<TextBox> txtRegNo = new List<TextBox>();
        private List<Label> labReadStatus = new List<Label>();
        private List<Label> labByte1 = new List<Label>();
        private List<Label> labByte0 = new List<Label>();
        private List<Label> labValue = new List<Label>();
        #endregion

        #region 面板回调函数
        private void FrmI2C_Load(object sender, EventArgs e)
        {
            cmbCom.Items.Clear();
            string[] com = System.IO.Ports.SerialPort.GetPortNames();
            for (int i = 0; i < com.Length; i++)
                cmbCom.Items.Add(com[i]);
            if (com.Length > 0)
                cmbCom.SelectedIndex = 0;

            cmbPlaceType.Items.Clear();
            cmbPlaceType.Items.Add("没任何产品"); 
            cmbPlaceType.Items.Add("只有左边产品"); 
            cmbPlaceType.Items.Add("只有右边产品");
            cmbPlaceType.Items.Add("两边都有产品");
            cmbPlaceType.SelectedIndex = 3; 

            cmbReadType.Items.Clear();
            cmbReadType.Items.Add("不同步OnOff信号"); 
            cmbReadType.Items.Add("同步OnOff信号");
            cmbReadType.Items.Add("通讯下命令6读取");
            cmbReadType.SelectedIndex = 0;

            cmbModel.Items.Clear();
            cmbModel.Items.Add("通用机种"); 
            cmbModel.Items.Add("HP机种");
            cmbModel.SelectedIndex = 0;

            cmbUUTNo.Items.Clear();
            cmbUUTNo.Items.Add("左边产品");
            cmbUUTNo.Items.Add("右边产品");
            cmbUUTNo.SelectedIndex = 0;
 
         }
        private void cmbUUTNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            uutNo = cmbUUTNo.SelectedIndex + 1;
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                btnOpen.Enabled = false;

                string er = string.Empty;

                if (cmbCom.Text == "")
                {
                    labStatus.Text = "请输入串口编号";
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                if (btnOpen.Text == "打开")
                {
                    comMon = new CI2CCom(EType.I2C_Server);
                    if (!comMon.Open(cmbCom.Text, out er, txtBaud.Text))
                    {
                        comMon = null;
                        labStatus.Text = "打开串口失败:" + er;
                        labStatus.ForeColor = Color.Red;
                        return;
                    }
                    btnOpen.Text = "关闭";
                    labStatus.Text = "成功打开串口";
                    labStatus.ForeColor = Color.Blue;
                    cmbCom.Enabled = false;
                }
                else
                {
                    if (BtnReadData.Text != "Start Read")
                    {
                        cancelMonitor = true;

                        while (cancelMonitor)
                            Application.DoEvents();
                    }

                    if (comMon != null)
                    {
                        comMon.Close();
                        comMon = null;
                    }
                    btnOpen.Text = "打开";
                    labStatus.Text = "关闭串口";
                    labStatus.ForeColor = Color.Black;
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

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                if (comMon.SetNewAddr(addr, out er))
                    showInfo("成功设置当前地址[" + addr.ToString("D2") + "]");
                else
                    showInfo("设置当前地址[" + addr.ToString("D2") + "]失败:" + er, true);
 
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
        private void btnReadVer_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadVer.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                string ver = string.Empty;

                if (!comMon.ReadVersion(addr, out ver, out er))
                {
                    showInfo("读取版本[" + addr.ToString("D2") + "]失败:" + er, true);
                    return;
                }

                labVer.Text = ver;

                showInfo("成功读取版本[" + addr.ToString("D2") + "]");

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                btnReadVer.Enabled = true;
            }
        }
        private void btnSetPara_Click(object sender, EventArgs e)
        {
            try
            {
                btnSetPara.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                CI2C_RunPara para = new CI2C_RunPara();

                para.PlaceType = (EPlace)cmbPlaceType.SelectedIndex;

                para.ReadType = (EReadType)cmbReadType.SelectedIndex;

                para.RunI2CType = (EModel)cmbModel.SelectedIndex;

                para.I2C_Addr = txtI2CAddr.Text;  

                para.RdCmdNum = System.Convert.ToInt16(txtCmdNum.Text);

                para.ScanCycle = System.Convert.ToInt16(txtScanTime.Text);

                para.ACONDelay = System.Convert.ToInt16(txtDelayTime.Text);

                for (int i = 0; i <  para.RdCmdNum; i++)
                {
                    para.Cmd[i].CmdOP = txtCmdNo[i].Text;
                    para.Cmd[i].RegNo = txtRegNo[i].Text;   
                }

                if (comMon.SendToSetI2C_RunPara(addr, para, out er))
                    showInfo("成功设置I2C运行参数[" + addr.ToString("D2") + "]");
                else
                    showInfo("设置I2C运行参数[" + addr.ToString("D2") + "]失败:" + er, true);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                btnSetPara.Enabled = true;
            }
        }
        private void btnReadPara_Click(object sender, EventArgs e)
        {
            try
            {
                btnReadPara.Enabled = false;

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                CI2C_RunPara para = new CI2C_RunPara();

                if (!comMon.ReadI2C_RunPara(addr, ref para, out er))
                {
                    showInfo("读取I2C运行参数[" + addr.ToString("D2") + "]失败:" + er, true);
                    return;
                }

                showInfo("成功读取I2C运行参数[" + addr.ToString("D2") + "]");

                cmbPlaceType.SelectedIndex = (int)para.PlaceType;

                cmbReadType.SelectedIndex = (int)para.ReadType;

                cmbModel.SelectedIndex = (int)para.RunI2CType;

                txtI2CAddr.Text=para.I2C_Addr;

                txtCmdNum.Text = para.RdCmdNum.ToString();

                txtScanTime.Text = para.ScanCycle.ToString();

                txtDelayTime.Text = para.ACONDelay.ToString();   

                for (int i = 0; i < para.RdCmdNum; i++)
                {
                    txtCmdNo[i].Text = para.Cmd[i].CmdOP;
                    txtRegNo[i].Text = para.Cmd[i].RegNo;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                btnReadPara.Enabled = true;
            }
        }
        private void BtnReadData_Click(object sender, EventArgs e)
        {
            try
            {
                BtnReadData.Enabled = false;

                if (BtnReadData.Text != "Start Read")
                {
                    cancelMonitor = true;
                    return;
                }

                if (!checkSystem())
                    return;

                int addr = System.Convert.ToInt16(txtAddr.Text);

                string er = string.Empty;

                if (!chkScan.Checked)
                {
                    Stopwatch watcher = new Stopwatch();

                    watcher.Start(); 

                    CI2C_Data data = new CI2C_Data();

                    if (!comMon.ReadI2C_Data(addr, uutNo, ref data, out er))
                    {
                        showInfo("读取I2C数据[" + addr.ToString("D2") + "]失败:" + er, true);
                        return;
                    }

                    watcher.Stop();

                    showData(data, watcher.ElapsedMilliseconds);

                    showInfo("成功读取I2C数据[" + addr.ToString("D2") + "]");
                }
                else
                {

                    I2C_Addr = System.Convert.ToInt16(txtAddr.Text);

                    delayMs = System.Convert.ToInt16(txtScanTime.Text);  

                    cancelMonitor = false;

                    scanNum = 0;

                    BtnReadData.Text = "Stop Read"; 

                    OnScanHandler scan = new OnScanHandler(OnScan);

                    scan.BeginInvoke(null, null);  
                
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                BtnReadData.Enabled = true;
            }
        }
        #endregion

        #region 方法
        private delegate void OnScanHandler();
        private void OnScan()
        {
            try
            {
                int TimeInteral = 100;

                int TimeMs = delayMs * 1000 / TimeInteral;

                int count = 0;

                while (true)
                {

                    if (cancelMonitor)
                        return;

                    if (count < TimeMs)
                    {
                        count++;
                        System.Threading.Thread.Sleep(TimeInteral);
                        continue;
                    }

                    count = 0;

                    string er = string.Empty;

                    Stopwatch watcher = new Stopwatch();

                    watcher.Start();

                    CI2C_Data data = new CI2C_Data();

                    scanNum++;

                    if (!comMon.ReadI2C_Data(I2C_Addr, uutNo, ref data, out er))
                    {
                        watcher.Stop();

                        showData(data, watcher.ElapsedMilliseconds);

                        showInfo("读取I2C数据[" + I2C_Addr.ToString("D2") + "]失败:" + er + "->" + 
                                 watcher.ElapsedMilliseconds.ToString() + "ms", true);
                        
                        continue;
                    }

                    watcher.Stop();

                    showData(data, watcher.ElapsedMilliseconds);

                    showInfo("成功读取I2C数据[" + I2C_Addr.ToString("D2") + "]:" + watcher.ElapsedMilliseconds.ToString() + "ms");
 
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                cancelMonitor = false;

                this.Invoke(new Action(() =>
                {
                    BtnReadData.Text = "Start Read";
                }
                ));
            }
        }
        /// <summary>
        /// 检查设置
        /// </summary>
        /// <returns></returns>
        private bool checkSystem()
        {
            if (comMon == null)
            {
                labStatus.Text = "请确定已打开串口?";
                labStatus.ForeColor = Color.Red;
                return false;
            }
            if (txtAddr.Text == "")
            {
                labStatus.Text = "请输入地址.";
                labStatus.ForeColor = Color.Red;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="er"></param>
        /// <param name="alarm"></param>
        private void showInfo(string er, bool alarm = false)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string, bool>(showInfo), er, alarm);
            else
            {
                if (!alarm)
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Blue;
                }
                else
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                }
            }
        }
        /// <summary>
        /// 显示结果
        /// </summary>
        /// <param name="data"></param>
        private void showData(CI2C_Data data,long msTimes)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<CI2C_Data,long>(showData), data,msTimes);
            else
            {
                labACON.Text=(data.AC_ONOFF==1?"ON":"OFF"); 

                for (int i = 0; i < data.CmdNum; i++)
                {
                    labReadStatus[i].Text = data.Val[i].CmdNo.ToString("X2") + ":" + data.Val[i].RdStatus.ToString("X2");   
                    labByte1[i].Text = data.Val[i].RdByte1.ToString("X2");
                    labByte0[i].Text = data.Val[i].RdByte0.ToString("X2");  
                }

                labTimes.Text = scanNum.ToString() + "->" + msTimes.ToString() + "ms";
            }
        }
        #endregion

       
       
    }
}
