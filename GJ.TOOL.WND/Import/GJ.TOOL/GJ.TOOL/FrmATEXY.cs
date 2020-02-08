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
using GJ.DEV.ATEXY;
namespace GJ.TOOL
{
    public partial class FrmATEXY : Form,IChildMsg
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
        public FrmATEXY()
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
            picX = new PictureBox[] {picX1, picX2, picX3, picX4, picX5, picX6};

            picK = new PictureBox[] { picK1, picK2, picK3, picK4, picK5, picK6 };                                   
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
        private CGJX6Y6 comMon = null;
        #endregion

        #region 面板控件
        private PictureBox[] picX = null;
        private PictureBox[] picK = null;
        #endregion

        #region 面板回调函数
        private void FrmATEXY_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < picX.Length; i++)
                picX[i].Image = ImageList1.Images["OFF"];
            for (int i = 0; i < picK.Length; i++)
            {
                picK[i].Image = ImageList1.Images["OFF"];
                picK[i].Tag = "OFF";
                picK[i].Click += new EventHandler(picKClick);
            }

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
                    labStatus.Text = CLanguage.Lan("请输入串口编号");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                string er = string.Empty;

                if (comMon == null)
                {
                    comMon = new CGJX6Y6();

                    if (!comMon.Open(cmbCOM.Text, out er))
                    {
                        labStatus.Text = er;
                        labStatus.ForeColor = Color.Red;
                        comMon = null;
                        return;
                    }
                    btnOpen.Text = CLanguage.Lan("关闭");
                    labStatus.Text = CLanguage.Lan("成功打开串口.");
                    labStatus.ForeColor = Color.Blue;
                }
                else
                {
                    comMon.Close();
                    comMon = null;
                    btnOpen.Text = CLanguage.Lan("打开");
                    labStatus.Text = CLanguage.Lan("关闭串口.");
                    labStatus.ForeColor = Color.Blue;
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
                if (txtAddr.Text == "")
                {
                    labStatus.Text = CLanguage.Lan("请输入模块地址");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                string er = string.Empty;

                int wAddr = System.Convert.ToInt32(txtAddr.Text);

                List<int> X = null;

                List<int> Y = null;

                if (!comMon.ReadXY(wAddr, out X,out Y, out er))
                {
                    labStatus.Text = CLanguage.Lan("读取IO板X1-X8信号失败:") + er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                for (int i = 0; i < X.Count; i++)
                {
                    picX[i].Image = (X[i] == 1 ? ImageList1.Images["ON"] : ImageList1.Images["OFF"]);
                }
                for (int i = 0; i < Y.Count; i++)
                {
                    picK[i].Image = (Y[i] == 1 ? ImageList1.Images["ON"] : ImageList1.Images["OFF"]);
                }

                labStatus.Text = CLanguage.Lan("读取IO板XY信号成功.");
                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnRead.Enabled = true;
            }           
        }
        private void btnOff_Click(object sender, EventArgs e)
        {
            try
            {
                btnOff.Enabled = false;

                if (comMon == null)
                {
                    labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAddr.Text == "")
                {
                    labStatus.Text = CLanguage.Lan("请输入模块地址");
                    labStatus.ForeColor = Color.Red;
                    return;
                }
                string er = string.Empty;
                
                int wAddr = System.Convert.ToInt32(txtAddr.Text);

                int OnOff = (btnOff.Text == "All OFF" ? 1 : 0);

                if (!comMon.CtrlYRelay(wAddr, 0, OnOff, out er))
                {
                    labStatus.Text = CLanguage.Lan("设置IO板Y信号错误:") + er;
                    labStatus.ForeColor = Color.Red;
                    return;
                }

                if (btnOff.Text == "All OFF")
                {
                    for (int i = 0; i < picK.Length; i++)
                    {
                        picK[i].Image = ImageList1.Images["ON"];
                        picK[i].Tag = "ON";
                    }

                    btnOff.Text = "All ON";
                }
                else
                {
                    for (int i = 0; i < picK.Length; i++)
                    {
                        picK[i].Image = ImageList1.Images["OFF"];
                        picK[i].Tag = "OFF";
                    }

                    btnOff.Text = "All OFF";
                }
              
                labStatus.Text = CLanguage.Lan("设置IO板Y信号成功.");

                labStatus.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                labStatus.Text = ex.ToString();
                labStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnOff.Enabled = true;
            }
        }
        private void picKClick(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;

            int idNo = System.Convert.ToInt16(pic.Name.Substring(4, pic.Name.Length - 4));

            if (comMon == null)
            {
                labStatus.Text = CLanguage.Lan("请确定串口是否打开?");
                labStatus.ForeColor = Color.Red;
                return;
            }
            if (txtAddr.Text == "")
            {
                labStatus.Text = CLanguage.Lan("请输入模块地址");
                labStatus.ForeColor = Color.Red;
                return;
            }
            string er = string.Empty;

            int wAddr = System.Convert.ToInt32(txtAddr.Text);

            int onoff = 0;

            if (pic.Tag.ToString() == "OFF")
                onoff = 1;

            if (!comMon.CtrlYRelay(wAddr, idNo, onoff, out er))
            {
                labStatus.Text = CLanguage.Lan("设置IO板Y信号失败:") + er;
                labStatus.ForeColor = Color.Red;
                return;
            }
            if (onoff == 1)
            {
                pic.Tag = "ON";
                pic.Image = ImageList1.Images["ON"];
            }
            else
            {
                pic.Tag = "OFF";
                pic.Image = ImageList1.Images["OFF"];
            }
            labStatus.Text = CLanguage.Lan("设置IO板Y信号成功.");
            labStatus.ForeColor = Color.Blue;         
        }
        #endregion

    }
}
