using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GJ.SFCS
{
    public partial class FrmAction : Form
    {
        #region 构造函数
        private FrmAction()
        {
            InitializeComponent();
        }
        #endregion

        #region 字段
        private static FrmAction dlg = null;
        private static object syncRoot = new object();
        private static int bAlarmFlag = 0;
        private static string bAlarmInfo = string.Empty;
        #endregion

        #region 属性
        /// <summary>
        /// 窗口状态
        /// </summary>
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
        public static int AlarmFlag
        {
            get {
                    lock (syncRoot)
                    {
                        return bAlarmFlag;
                    }
                }
            set {
                    lock (syncRoot)
                    {
                        bAlarmFlag = value;
                    }
                }
        }
        public static string AlarmInfo
        {
            get {
                    lock (syncRoot)
                    {
                        return bAlarmInfo;
                    }
                }
            set {
                    lock (syncRoot)
                    {
                        bAlarmInfo = value;
                    }
                }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 创建唯一实例
        /// </summary>
        public static FrmAction CreateInstance()
        {
            lock (syncRoot)
            {
                if (dlg == null || dlg.IsDisposed)
                {
                    dlg = new FrmAction();

                    if (bAlarmInfo != string.Empty)
                    {
                        dlg.labInfo.Text = bAlarmInfo;
                    }
                }                
            }
            return dlg;
        }
        #endregion

        #region 面板回调函数
        private void btnOK_Click(object sender, EventArgs e)
        {
            lock (syncRoot)
            {
                bAlarmFlag = 0;

                this.Close();

                dlg = null;
            }
        }
        #endregion

    }
}
