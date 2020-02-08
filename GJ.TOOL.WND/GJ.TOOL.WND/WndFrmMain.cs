/*
V1.0.4
1. 增加三菱FX3U,FX5U PLC通信模块
2. 快充中继板D+D-短路检测功能

V1.0.5  --2018/8/11
1.增加快充中控板用于IO功能 
2.苹果Phoenix烧录器
3.10通道AC电流采集模块GJMI_10
4.I2C转RS485模块RS485_I2C

V1.0.6 --2018/11/10
1.增加100W电子负载
2.电流采集板
3.I2C转RS485
4.GJ_V3控制板

V1.0.7 --2019/02/19
1.华为Socket通信
2.中英切换功能
3.Web2.0工具

V1.0.8 --2019/06/06
1.软件自动升级版本
2.GJ.COM增加CZipHelper类
3.增加Honeywell条码枪H3320G模块

V1.0.9 --2019/10/15
1.Socket调试工具
2.风扇控制板
3.IO板X8Y14
4.ATE板X6Y6
5.基恩士条码枪SR700,SR710_TCP
6.康耐士DM70

V1.1.0---2019/12/04
1.优化快充模块监控线程
2.增加电子负载I_40_08
3.MQTT增加工厂编号


V1.1.1---2020/01/04
1.设备监控增加监控功能
*/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using GJ.COM;
using GJ.PLUGINS;
using GJ.PDB;
namespace GJ.TOOL.WND
{
   public partial class WndFrmMain : Form
   {
      #region 版本控制
       private string _title = string.Empty;
       private string _verName = "V1.1.1";
       private string _verDate = "2020/01/04";
       #endregion

      #region 插件方法
       /// <summary>
       /// 子窗口
       /// </summary>
       private Dictionary<string, object> _childFormList = new Dictionary<string, object>();
       /// <summary>
       /// 加载测试工位
       /// </summary>
       private void loadChildForm(string dlgName, Control fatherControl)
       {
           try
           {
               string er = string.Empty;

               if (selObj != null)
               {
                   if (!CReflect.SendWndMethod((Form)selObj, EMessType.OnCloseDlg, out er, null))
                   {
                       MessageBox.Show(er, "消息机制", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                   }
               }

               selObj = null;

               string dlgFullName = "GJ.TOOL.Frm" + dlgName;

               string[] fileNames = Directory.GetFiles(Application.StartupPath);

               foreach (string file in fileNames)
               {
                   string fileName = Path.GetFileName(file);

                   if ((fileName.ToUpper().StartsWith("GJ")) && fileName.ToUpper().EndsWith(".DLL"))
                   {
                       try
                       {
                           //载入dll
                           Assembly asb = Assembly.LoadFrom(file);

                           Type[] types = asb.GetTypes();

                           foreach (Type t in types)
                           {
                               if (t.GetInterface("IChildMsg") != null)
                               {
                                   if (t.FullName == dlgFullName)
                                       selObj = asb.CreateInstance(t.FullName);
                               }
                           }
                       }
                       catch (Exception ex)
                       {
                           MessageBox.Show(ex.ToString());
                           return;
                       }
                   }
               }
               if (selObj == null)
               {
                   //MessageBox.Show("加载动态库" + dlgFullName + "失败");
                   return;
               }

               Type type = selObj.GetType();

               MethodInfo OnWndDlg = type.GetMethod("OnShowDlg");

               if (OnWndDlg != null)
               {
                   foreach (Control obj in fatherControl.Controls)
                   {
                       fatherControl.Controls.Remove(obj);
                       obj.Dispose();
                   }

                   OnWndDlg.Invoke(selObj, new object[] { this, fatherControl, dlgName });
               }
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.ToString());
           }
       }
       /// <summary>
       /// 设置窗口消息
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="message"></param>
       private void setDlgMessage(object sender, string message)
       {
           Type type = sender.GetType();
           MethodInfo mehthod = type.GetMethod(message);
           if (mehthod != null)
               mehthod.Invoke(sender, null);
       }
       #endregion

      #region 构造函数
       public WndFrmMain()
       {
           InitializeComponent();

           IntialControl();

           SetDoubleBuffered();

           LoadLanguge();

           loadIniFile();

           SetLanguage();

       }
       #endregion

      #region 初始化
    /// <summary>
    /// 绑定控件
    /// </summary>
    private void IntialControl()
    {
        try
        {
             
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }
    /// <summary>
    /// 设置双缓冲,防止界面闪烁
    /// </summary>
    private void SetDoubleBuffered()
    {        
        splitPanel.Panel1.GetType().GetProperty("DoubleBuffered",
                                        System.Reflection.BindingFlags.Instance |
                                        System.Reflection.BindingFlags.NonPublic)
                                        .SetValue(splitPanel.Panel1, true, null);
        splitPanel.Panel2.GetType().GetProperty("DoubleBuffered",
                                        System.Reflection.BindingFlags.Instance |
                                        System.Reflection.BindingFlags.NonPublic)
                                        .SetValue(splitPanel.Panel2, true, null);
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

      #region 面板回调函数
      private void WndFrmMain_Load(object sender, EventArgs e)
      {  
         this.WindowState = FormWindowState.Maximized;
      }
      private void tblrExpandAll_Click(object sender, EventArgs e)
      {
          treeView1.ExpandAll();
      }
      private void tblrCollapse_Click(object sender, EventArgs e)
      {
          treeView1.CollapseAll();
      }
      private void tlUser_Click(object sender, EventArgs e)
      {
        
      }
      private void tlOpen_Click(object sender, EventArgs e)
     {
        open();
     }
      private void tlSave_Click(object sender, EventArgs e)
      {
        save(); 
      }
      private void toolExit_Click(object sender, EventArgs e)
      {
        exit();
      }
      private void menuOpen_Click(object sender, EventArgs e)
      {
        open();
      }
      private void menuSave_Click(object sender, EventArgs e)
      {
        save();
      }
      private void menuExit_Click(object sender, EventArgs e)
      {
        exit();
      }
      private void menuVersion_Click(object sender, EventArgs e)
      {

      }
      private void menuInstruction_Click(object sender, EventArgs e)
      {

      }
      private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
      {
          try
          {

              labStatus.Text = "正在加载..";
              labStatus.BackColor = Color.Red;
              progressBar1.Value = 0;
              labStatus.Refresh();
              progressBar1.Refresh();

              if (e.Node.Text == "仪器设备")
                  return;

              if (e.Node.Parent.Text == "仪器设备")
                  return;

              if (e.Node.Text == "")
                  return;

              this.tlToolName.Text = e.Node.Text;

              string deviceName = e.Node.Text;

              if (deviceName == selDeviceName)
                  return;

              //移除面板显示
              foreach (Control item in splitPanel.Panel2.Controls)
              {
                  ///表示需关闭仪器设备端口
                  setDlgMessage(item, "OnCloseDlg");
                  splitPanel.Panel2.Controls.Remove(item);
              }

              if (!_childFormList.ContainsKey(deviceName))
              {
                  loadChildForm(deviceName, splitPanel.Panel2);
              }
              else
              {
                  splitPanel.Panel2.Controls.Add((Form)_childFormList[deviceName]);
              }

              selDeviceName = deviceName;
              
          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.ToString());
          }
          finally
          {
              labStatus.Text = "加载完毕..";
              labStatus.BackColor = Color.Green;
              progressBar1.Value = progressBar1.Maximum;
          }
      }
      /// <summary>
    /// 重写窗体消息
    /// </summary>
    /// <param name="m">屏蔽关闭按钮</param>
      protected override void WndProc(ref Message m)
      {
        const int WM_SYSCOMMAND = 0x0112;
        const int SC_CLOSE = 0xF060;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        if (m.Msg == WM_SYSCOMMAND)
        {
            switch ((int)m.WParam)
            {
                case SC_CLOSE:
                    return;
                case SC_MINIMIZE:
                    break;
                case SC_MAXIMIZE:
                    break;
                default:
                    break;
            }
        }
        base.WndProc(ref m);
    }
      #endregion

      #region 字段
      private string devIniFile = Application.StartupPath + "\\Device.ini";
      private string devXmlFile = string.Empty;
      private string selDeviceName = string.Empty;
      private object selObj = null;
      #endregion

      #region 方法
      private void open()
      {
          try
          {
              OpenFileDialog dlg = new OpenFileDialog();
              dlg.InitialDirectory = Application.StartupPath;
              dlg.Filter = "xml files (*.xml)|*.xml";
              if (dlg.ShowDialog() == DialogResult.OK)
              {
                  treeView1.Nodes.Clear();
                  CXml.LoadXmlToTreeView(dlg.FileName, treeView1);
                  treeView1.ExpandAll();
                  this.Text = "调式工具(冠佳电子)--" + dlg.FileName;
                  devXmlFile = dlg.FileName;
                  saveIniFile();
              }
          }
          catch (Exception er)
          {
              MessageBox.Show(er.ToString());
          }
      }
      private void save()
      {
          try
          {
              SaveFileDialog dlg = new SaveFileDialog();
              dlg.InitialDirectory = Application.StartupPath;
              dlg.Filter = "xml files (*.xml)|*.xml";
              if (dlg.ShowDialog() == DialogResult.OK)
                  CXml.SaveTreeViewToXml(treeView1, dlg.FileName);
          }
          catch (Exception er)
          {
              MessageBox.Show(er.ToString());
          }
      }
      private void exit()
      {
          if (MessageBox.Show("确定要退出系统?", "退出系统",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          {
              string er = string.Empty;

              if (selObj != null)
              {
                  if (!CReflect.SendWndMethod((Form)selObj, EMessType.OnCloseDlg, out er, null))
                  {
                      MessageBox.Show(er, "消息机制", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  }
              }

              Application.Exit();
          }
      }
      private void loadIniFile()
      {
          devXmlFile = CIniFile.ReadFromIni("Device", "config", devIniFile, "Device.xml");
      }
      private void saveIniFile()
      {
          CIniFile.WriteToIni("Device", "config", devXmlFile, devIniFile);      
      }
      private void loadSysXml()
      {
          if (File.Exists(devXmlFile))
          {
              treeView1.Nodes.Clear();
              CXml.LoadXmlToTreeView(devXmlFile, treeView1);
              treeView1.ExpandAll();
              this.Text = _title + devXmlFile;
          }
      }
      #endregion

      #region 语言设置
      private void menuChinese_Click(object sender, EventArgs e)
      {
          ChangeLanguage(CLanguage.EL.中文);

          string er = string.Empty;

          foreach (string key in _childFormList.Keys)
          {
              Form childForm = (Form)_childFormList[key];

              if (!CReflect.SendWndMethod(childForm, EMessType.OnChangeLAN, out er, null))
              {
                  MessageBox.Show(er, "消息机制", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  return;
              }       
          }             
      }
      private void menuEnglish_Click(object sender, EventArgs e)
      {
          ChangeLanguage(CLanguage.EL.英语);

          string er = string.Empty;

          foreach (string key in _childFormList.Keys)
          {
              Form childForm = (Form)_childFormList[key];

              if (!CReflect.SendWndMethod(childForm, EMessType.OnChangeLAN, out er, null))
              {
                  MessageBox.Show(er, "消息机制", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  return;
              }
          }
      }
      /// <summary>
        /// 加载中英文字典
        /// </summary>
      private void LoadLanguge()
        { 
            try 
	        {
                CLanguage.LoadLanType();
                string lanDB = Application.StartupPath + "\\LAN.accdb";
                if (!File.Exists(lanDB))
                    return;
                CDBCOM db = new CDBCOM(EDBType.Access, ".", lanDB);
                string er = string.Empty;
                DataSet ds = null;
                string sqlCmd="select * from LanList order by idNo";
                if (!db.QuerySQL(sqlCmd, out ds, out er))
                    return;
                Dictionary<string, string> lan = new Dictionary<string, string>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string LAN_CH = ds.Tables[0].Rows[i]["LAN_CH"].ToString();
                    string LAN_EN = ds.Tables[0].Rows[i]["LAN_EN"].ToString();
                    if (!lan.ContainsKey(LAN_CH))
                        lan.Add(LAN_CH, LAN_EN);
                }
                CLanguage.Load(lan, out er);
	        }
	        catch (Exception)
	        {		
		        throw;
	        }            
        }
      /// <summary>
        /// 加载中英文界面
        /// </summary>
      private void SetLanguage()
        {
            CLanguage.SetLanguage(this);

            switch (CLanguage.languageType)
            {
                case CLanguage.EL.中文:
                    menuEnglish.Checked = false;
                    menuChinese.Checked = true;
                    tbrVer.Text = "版本:" + _verName;
                    _title = "常用调试工具--东莞市冠佳电子设备有限公司 更新日期:[" + _verDate + "]";  
                    break;
                case CLanguage.EL.英语:
                    menuEnglish.Checked = true;
                    menuChinese.Checked = false;
                    tbrVer.Text = "Version:" + _verName;
                     _title = "Common Debug Tool--Dongguan Guan Jia electronic equipment Co.,Ltd Modify Date:[" + _verDate + "]";
                    break;
                default:
                    break;
            }

            this.Text = _title;

            loadSysXml();
        }
      /// <summary>
        /// 切换界面语言
        /// </summary>
        /// <param name="language"></param>
      private void ChangeLanguage(CLanguage.EL language)
        {
            CLanguage.SetLanType(language);
            
            SetLanguage();
        }
      #endregion
    
   }
}
