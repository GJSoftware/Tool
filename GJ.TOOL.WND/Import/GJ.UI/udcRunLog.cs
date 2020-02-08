using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using GJ.COM;
namespace GJ.UI
{
   [ToolboxBitmap(typeof(udcRunLog), "RunLog.bmp")]
   public partial class udcRunLog : UserControl
   {
      #region 构造函数
      public udcRunLog()
      {
         InitializeComponent();
      }
      #endregion

      #region 枚举
      public enum ELog
      {
         /// <summary>
         /// 内容(黑色)
         /// </summary>
         Content, 
         /// <summary>
         /// 注意(蓝色)
         /// </summary>
         Action,
         /// <summary>
         /// 正常(绿色)
         /// </summary>
         OK,
         /// <summary>
         /// 异常(红色)
         /// </summary>
         NG,
         /// <summary>
         /// 错误(黄色)
         /// </summary>
         Err
      }
      #endregion

      #region 字段
      private bool titleVisable = true;
      private int maxLine = 1000;
      private double maxMB =1;
      private Color[] colorArray = new Color[] { Color.Black, Color.Blue, Color.Green, Color.Red, Color.DarkOrange };
      private bool saveEnable = true;
      private string saveFolder = string.Empty;
      private string saveName = "RunLog";
      #endregion

      #region 属性
       /// <summary>
      /// 标题
       /// </summary>
      [Localizable(false)]
      [Bindable(false)]
      [Browsable(true)] 
      [Category("自定义")]
      [Description("标题")]    
      public string mTitle
      {
         set { labTitle.Text = value; }
         get { return labTitle.Text; }
      }
       /// <summary>
      /// 标题可见
       /// </summary>
      [Localizable(false)]
      [Bindable(false)]
      [Browsable(true)]
      [Category("自定义")]
      [Description("标题可见")] 
      public bool mTitleEnable
      {
          set {
              titleVisable = value;
              if (titleVisable)
              {                 
                  panelMain.RowStyles[0].Height = 28;
                  panelMain.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                  labTitle.Visible = true;                  
              }
              else
              {
                  panelMain.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                  panelMain.RowStyles[0].Height = 0;
                  labTitle.Visible = false; 
                  
              }                   
            }
          get
          {
              return titleVisable;
          }
      }
       /// <summary>
      /// 字体
       /// </summary>
      [Localizable(false)]
      [Bindable(false)]
      [Browsable(true)]
      [Category("自定义")]
      [Description("字体")]  
      public Font mFont
      {
          set { rtbLog.Font = value; }
          get { return rtbLog.Font; }
      }
       /// <summary>
      /// 日志最大行数
       /// </summary>
      [Localizable(false)]
      [Bindable(false)]
      [Browsable(true)]
      [Category("自定义")]
      [Description("日志最大行数")] 
      public int mMaxLine
      {
          set { maxLine = value; }
          get { return maxLine; }
      }
       /// <summary>
      /// 日志文件大小
       /// </summary>
      [Localizable(false)]
      [Bindable(false)]
      [Browsable(true)]
      [Category("自定义")]
      [Description("日志文件大小")] 
      public double mMaxMB
      {
          set { maxMB = value; }
          get { return maxMB; }
      }
       /// <summary>
      /// 是否保存日志
       /// </summary>
      [Localizable(false)]
      [Bindable(false)]
      [Browsable(true)]
      [Category("自定义")]
      [Description("是否保存日志")] 
      public bool mSaveEnable
      {
          set { saveEnable = value; }
          get { return saveEnable; }
      }
      /// <summary>
      /// 设置保存日志名称
      /// </summary>
      [Localizable(false)]
      [Bindable(false)]
      [Browsable(true)]
      [Category("自定义")]
      [Description("设置保存日志文件夹")]
      public string mSaveFolder
      {
          set { saveFolder = value; }
          get { return saveFolder; }
      }
       /// <summary>
      /// 设置保存日志名称
       /// </summary>
      [Localizable(false)]
      [Bindable(false)]
      [Browsable(true)]
      [Category("自定义")]
      [Description("设置保存日志名称")]
      public string mSaveName
      {
          set { saveName = value; }
          get { return saveName; }
      }
      /// <summary>
      /// 设置日志边框格式
      /// </summary>
      [Localizable(false)]
      [Bindable(false)]
      [Browsable(true)]
      [Category("自定义")]
      [Description("设置日志边框格式")]
      public BorderStyle mBorderStyle
      {
          set { rtbLog.BorderStyle = value; }
      }
      #endregion

      #region 同步锁
      private AutoResetEvent mHEvent = new AutoResetEvent(true);
      #endregion

      #region 方法
      /// <summary>
      /// 保存数据
      /// </summary>
      /// <param name="wMessage"></param>
      /// <param name="wLog"></param>
      public void Log(string wMessage, ELog wLog,bool saveFlag=true)
      {
         if (this.InvokeRequired)    //跨线程调用
             this.Invoke(new Action<string,ELog,bool> (Log), wMessage, wLog, saveFlag);
         else
         {
            try
            {
               mHEvent.WaitOne();                   //防止对同一文件写入数据   
               if (rtbLog.Lines.Length > maxLine)   //清空数据
                  rtbLog.Clear();
               if (wMessage == string.Empty)
                  return;               
               string insertNow = DateTime.Now.ToString("HH:mm:ss.") + DateTime.Now.Millisecond.ToString("D3") + " | ";
               if (saveFolder == string.Empty)
                   rtbLog.AppendText(insertNow);
               int lines = rtbLog.Text.Length;
               int lens = wMessage.Length;
               rtbLog.AppendText(wMessage + "\r\n");
               rtbLog.Select(lines, lens);
               rtbLog.SelectionColor = colorArray[(int)wLog];
               rtbLog.ScrollToCaret();
               rtbLog.Refresh();
               string saveInfo=insertNow + "[" +wLog.ToString()+"]: " + wMessage;
               if (saveFlag && saveEnable)               
                  SaveToTxt(saveInfo);               
            }
            catch (Exception)
            {
               
            }
            finally
            {
               mHEvent.Set(); 
            }
         }
      }
      /// <summary>
      /// 清除数据
      /// </summary>
      public void Clear()
      {
          try
          {
              mHEvent.WaitOne(); 

              rtbLog.Clear();
          }
          catch (Exception)
          {

          }
          finally
          {
              mHEvent.Set();
          }
         
      }
      /// <summary>
      /// 保存日志
      /// </summary>
      /// <param name="insertNow"></param>
      /// <param name="wMessage"></param>
      /// <param name="wLog"></param>
      private void SaveToTxt(string wMessage)
      {
          try
          {
              //获取保存文件名称
              string fileName = string.Empty;

              string path = System.Windows.Forms.Application.StartupPath +"\\" +
                            saveName + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";

              if (saveFolder != string.Empty)
              {
                  path = System.Windows.Forms.Application.StartupPath + "\\" +
                         saveFolder + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
                  if (!Directory.Exists(path))
                      Directory.CreateDirectory(path);
                  fileName = path + saveName + ".log";
                  StreamWriter sw1 = new StreamWriter(fileName, true, Encoding.UTF8);
                  sw1.WriteLine(wMessage);
                  sw1.Flush();
                  sw1.Close();
                  sw1.Dispose();
                  return;
              }

              if (!Directory.Exists(path))
              {
                  Directory.CreateDirectory(path);
                  fileName = path + saveName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
              }
              else
              {
                  string[] dirFileName = Directory.GetFiles(path);
                  if (dirFileName.Length == 0)
                      fileName = path + saveName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
                  else
                  {
                      DateTime rDate = DateTime.Now;
                      for (int i = 0; i < dirFileName.Length; i++)
                      {
                          if (i == 0)
                          {
                              rDate = File.GetLastWriteTime(dirFileName[i]);
                              fileName = dirFileName[i];
                              continue;
                          }
                          if (rDate < File.GetLastWriteTime(dirFileName[i]))
                          {
                              rDate = File.GetLastWriteTime(dirFileName[i]);
                              fileName = dirFileName[i];
                          }
                      }
                  }                 
              }
              //判断文件是否过大？
              if (File.Exists(fileName))
              {
                  double rSize = new FileInfo(fileName).Length / 1024 / 1024;  //取文件大小为 KB--MB
                  if (rSize > maxMB)
                      fileName = path + saveName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
              }
              StreamWriter sw = new StreamWriter(fileName, true, Encoding.UTF8);
              sw.WriteLine(wMessage);
              sw.Flush();
              sw.Close();
              sw.Dispose();
          }
          catch (Exception e)
          {
              MessageBox.Show(e.ToString());  
          }
      }
      #endregion

      #region 面板回调函数
      private void udcRunLog_Load(object sender, EventArgs e)
      {
          labTitle.Text = CLanguage.Lan("运行日志");
      }
      #endregion

   
   }
}
