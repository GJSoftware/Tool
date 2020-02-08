namespace GJ.UI
{
   partial class udcRunLog
   {
      /// <summary> 
      /// 必需的设计器变量。
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// 清理所有正在使用的资源。
      /// </summary>
      /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region 组件设计器生成的代码

      /// <summary> 
      /// 设计器支持所需的方法 - 不要
      /// 使用代码编辑器修改此方法的内容。
      /// </summary>
      private void InitializeComponent()
      {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(udcRunLog));
            this.panelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labTitle = new System.Windows.Forms.Label();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.SaveLogWorker = new System.ComponentModel.BackgroundWorker();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            resources.ApplyResources(this.panelMain, "panelMain");
            this.panelMain.Controls.Add(this.labTitle, 0, 0);
            this.panelMain.Controls.Add(this.rtbLog, 0, 1);
            this.panelMain.Name = "panelMain";
            // 
            // labTitle
            // 
            resources.ApplyResources(this.labTitle, "labTitle");
            this.labTitle.ForeColor = System.Drawing.Color.Black;
            this.labTitle.Name = "labTitle";
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.rtbLog, "rtbLog");
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            // 
            // udcRunLog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.DoubleBuffered = true;
            this.Name = "udcRunLog";
            this.Load += new System.EventHandler(this.udcRunLog_Load);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel panelMain;
      private System.Windows.Forms.Label labTitle;
      private System.Windows.Forms.RichTextBox rtbLog;
      private System.ComponentModel.BackgroundWorker SaveLogWorker;
   }
}
