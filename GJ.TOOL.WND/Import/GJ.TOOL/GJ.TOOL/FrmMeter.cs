using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GJ.DEV.Meter;
using GJ.PLUGINS;
using GJ.COM;
namespace GJ.TOOL
{
    public partial class FrmMeter : Form, IChildMsg
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
        public FrmMeter()
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
        private CMeterCom comMon = null;
        #endregion

        #region 面板回调函数
        private void FrmMeter_Load(object sender, EventArgs e)
        {
            string[] com = System.IO.Ports.SerialPort.GetPortNames();
            cmbCOM.Items.Clear();
            for (int i = 0; i < com.Length; i++)
                cmbCOM.Items.Add(com[i]);
            if (com.Length > 0)
                cmbCOM.Text = com[0];
            cmbType.SelectedIndex = 0; 
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (cmbCOM.Text == "")
            {
                labStatus.Text = CLanguage.Lan("请输入串口编号");
                labStatus.ForeColor = Color.Red;
                return;
            }

            string er = string.Empty;

            if (comMon == null)
            {
                if (!Enum.IsDefined(typeof(EType), cmbType.Text))
                {
                    labStatus.Text = CLanguage.Lan("找不到类型") + "【" + cmbType.Text + "】";
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                EType devType = (EType)Enum.Parse(typeof(EType), cmbType.Text);

                comMon = new CMeterCom(devType, 0, cmbType.Text);

                if (!comMon.Open(cmbCOM.Text, out er, txtBand.Text))
                {
                    labStatus.Text = er;
                    labStatus.ForeColor = Color.Red;
                    comMon = null;
                    return;
                }
                btnOpen.Text = CLanguage.Lan("关闭");
                labStatus.Text = CLanguage.Lan("成功打开串口");
                labStatus.ForeColor = Color.Blue;
                cmbCOM.Enabled = false;
            }
            else
            {
                comMon.Close();
                comMon = null;
                btnOpen.Text = CLanguage.Lan("打开");
                labStatus.Text = CLanguage.Lan("关闭串口");
                labStatus.ForeColor = Color.Blue;
                cmbCOM.Enabled = true;
            }
        }
        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                btnRead.Enabled = false;

                if (comMon == null)
                {
                    labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                string er = string.Empty;

                int devAddr = System.Convert.ToInt16(txtMutiAddr.Text);   

                double acv = 0;

                double aci = 0;

                if (!comMon.ReadACV(devAddr, out acv, out er))
                {
                    System.Threading.Thread.Sleep(200);

                    if (!comMon.ReadACV(devAddr, out acv, out er))
                    {
                        labStatus.Text = CLanguage.Lan("读取电表电压值错误:") + er;
                        labStatus.ForeColor = Color.Red;
                        return;
                    }
                }

                if (!comMon.ReadACI(devAddr, out aci, out er))
                {
                    System.Threading.Thread.Sleep(200);

                    if (!comMon.ReadACV(devAddr, out aci, out er))
                    {
                        labStatus.Text = CLanguage.Lan("读取电表电流值错误:") + er;
                        labStatus.ForeColor = Color.Red;
                        return;
                    }
                }

                labVolt.Text = acv.ToString();

                labCurrent.Text = aci.ToString();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); 
            }
            finally
            {
                btnRead.Enabled = true;
            }           
        }
        #endregion
    }
}
