namespace GJ.TOOL.WND
{
   partial class WndFrmMain
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
          this.components = new System.ComponentModel.Container();
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WndFrmMain));
          this.splitPanel = new System.Windows.Forms.SplitContainer();
          this.panel2 = new System.Windows.Forms.TableLayoutPanel();
          this.panel3 = new System.Windows.Forms.TableLayoutPanel();
          this.progressBar1 = new System.Windows.Forms.ProgressBar();
          this.labStatus = new System.Windows.Forms.Label();
          this.treeView1 = new System.Windows.Forms.TreeView();
          this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.tblrExpandAll = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
          this.tblrCollapse = new System.Windows.Forms.ToolStripMenuItem();
          this.menuStrip1 = new System.Windows.Forms.MenuStrip();
          this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
          this.menuOpen = new System.Windows.Forms.ToolStripMenuItem();
          this.保存SToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
          this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
          this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
          this.menuUser = new System.Windows.Forms.ToolStripMenuItem();
          this.menuLogIn = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
          this.menuLogOut = new System.Windows.Forms.ToolStripMenuItem();
          this.menuLanguage = new System.Windows.Forms.ToolStripMenuItem();
          this.menuChinese = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
          this.menuEnglish = new System.Windows.Forms.ToolStripMenuItem();
          this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
          this.menuVersion = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
          this.menuInstruction = new System.Windows.Forms.ToolStripMenuItem();
          this.panel1 = new System.Windows.Forms.TableLayoutPanel();
          this.toolStrip1 = new System.Windows.Forms.ToolStrip();
          this.tlUser = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
          this.tlOpen = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
          this.tlSave = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
          this.tlExit = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
          this.toolUserLabel = new System.Windows.Forms.ToolStripLabel();
          this.toolCurUser = new System.Windows.Forms.ToolStripLabel();
          this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
          this.toolVersion = new System.Windows.Forms.ToolStripLabel();
          this.tlToolName = new System.Windows.Forms.ToolStripLabel();
          this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
          this.tbrVer = new System.Windows.Forms.ToolStripLabel();
          ((System.ComponentModel.ISupportInitialize)(this.splitPanel)).BeginInit();
          this.splitPanel.Panel1.SuspendLayout();
          this.splitPanel.SuspendLayout();
          this.panel2.SuspendLayout();
          this.panel3.SuspendLayout();
          this.contextMenuStrip1.SuspendLayout();
          this.menuStrip1.SuspendLayout();
          this.panel1.SuspendLayout();
          this.toolStrip1.SuspendLayout();
          this.SuspendLayout();
          // 
          // splitPanel
          // 
          this.splitPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
          resources.ApplyResources(this.splitPanel, "splitPanel");
          this.splitPanel.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
          this.splitPanel.Name = "splitPanel";
          // 
          // splitPanel.Panel1
          // 
          this.splitPanel.Panel1.Controls.Add(this.panel2);
          // 
          // panel2
          // 
          resources.ApplyResources(this.panel2, "panel2");
          this.panel2.Controls.Add(this.panel3, 0, 1);
          this.panel2.Controls.Add(this.treeView1, 0, 0);
          this.panel2.Name = "panel2";
          // 
          // panel3
          // 
          resources.ApplyResources(this.panel3, "panel3");
          this.panel3.Controls.Add(this.progressBar1, 0, 0);
          this.panel3.Controls.Add(this.labStatus, 1, 0);
          this.panel3.Name = "panel3";
          // 
          // progressBar1
          // 
          resources.ApplyResources(this.progressBar1, "progressBar1");
          this.progressBar1.Name = "progressBar1";
          this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
          // 
          // labStatus
          // 
          resources.ApplyResources(this.labStatus, "labStatus");
          this.labStatus.BackColor = System.Drawing.Color.Lime;
          this.labStatus.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
          this.labStatus.Name = "labStatus";
          // 
          // treeView1
          // 
          this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
          resources.ApplyResources(this.treeView1, "treeView1");
          this.treeView1.FullRowSelect = true;
          this.treeView1.Name = "treeView1";
          this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
          // 
          // contextMenuStrip1
          // 
          this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tblrExpandAll,
            this.toolStripMenuItem2,
            this.tblrCollapse});
          this.contextMenuStrip1.Name = "contextMenuStrip1";
          resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
          // 
          // tblrExpandAll
          // 
          this.tblrExpandAll.Name = "tblrExpandAll";
          resources.ApplyResources(this.tblrExpandAll, "tblrExpandAll");
          this.tblrExpandAll.Click += new System.EventHandler(this.tblrExpandAll_Click);
          // 
          // toolStripMenuItem2
          // 
          this.toolStripMenuItem2.Name = "toolStripMenuItem2";
          resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
          // 
          // tblrCollapse
          // 
          this.tblrCollapse.Name = "tblrCollapse";
          resources.ApplyResources(this.tblrCollapse, "tblrCollapse");
          this.tblrCollapse.Click += new System.EventHandler(this.tblrCollapse_Click);
          // 
          // menuStrip1
          // 
          this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuUser,
            this.menuLanguage,
            this.menuHelp});
          resources.ApplyResources(this.menuStrip1, "menuStrip1");
          this.menuStrip1.Name = "menuStrip1";
          // 
          // menuFile
          // 
          this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpen,
            this.保存SToolStripMenuItem,
            this.menuSave,
            this.toolStripSeparator2,
            this.menuExit});
          this.menuFile.Name = "menuFile";
          resources.ApplyResources(this.menuFile, "menuFile");
          // 
          // menuOpen
          // 
          this.menuOpen.Name = "menuOpen";
          resources.ApplyResources(this.menuOpen, "menuOpen");
          this.menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
          // 
          // 保存SToolStripMenuItem
          // 
          this.保存SToolStripMenuItem.Name = "保存SToolStripMenuItem";
          resources.ApplyResources(this.保存SToolStripMenuItem, "保存SToolStripMenuItem");
          // 
          // menuSave
          // 
          this.menuSave.Name = "menuSave";
          resources.ApplyResources(this.menuSave, "menuSave");
          this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
          // 
          // toolStripSeparator2
          // 
          this.toolStripSeparator2.Name = "toolStripSeparator2";
          resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
          // 
          // menuExit
          // 
          this.menuExit.Name = "menuExit";
          resources.ApplyResources(this.menuExit, "menuExit");
          this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
          // 
          // menuUser
          // 
          this.menuUser.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLogIn,
            this.toolStripSeparator1,
            this.menuLogOut});
          this.menuUser.Name = "menuUser";
          resources.ApplyResources(this.menuUser, "menuUser");
          // 
          // menuLogIn
          // 
          this.menuLogIn.Name = "menuLogIn";
          resources.ApplyResources(this.menuLogIn, "menuLogIn");
          // 
          // toolStripSeparator1
          // 
          this.toolStripSeparator1.Name = "toolStripSeparator1";
          resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
          // 
          // menuLogOut
          // 
          this.menuLogOut.Name = "menuLogOut";
          resources.ApplyResources(this.menuLogOut, "menuLogOut");
          // 
          // menuLanguage
          // 
          this.menuLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuChinese,
            this.toolStripSeparator4,
            this.menuEnglish});
          this.menuLanguage.Name = "menuLanguage";
          resources.ApplyResources(this.menuLanguage, "menuLanguage");
          // 
          // menuChinese
          // 
          this.menuChinese.Name = "menuChinese";
          resources.ApplyResources(this.menuChinese, "menuChinese");
          this.menuChinese.Click += new System.EventHandler(this.menuChinese_Click);
          // 
          // toolStripSeparator4
          // 
          this.toolStripSeparator4.Name = "toolStripSeparator4";
          resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
          // 
          // menuEnglish
          // 
          this.menuEnglish.Name = "menuEnglish";
          resources.ApplyResources(this.menuEnglish, "menuEnglish");
          this.menuEnglish.Click += new System.EventHandler(this.menuEnglish_Click);
          // 
          // menuHelp
          // 
          this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuVersion,
            this.toolStripMenuItem1,
            this.menuInstruction});
          this.menuHelp.Name = "menuHelp";
          resources.ApplyResources(this.menuHelp, "menuHelp");
          // 
          // menuVersion
          // 
          this.menuVersion.Name = "menuVersion";
          resources.ApplyResources(this.menuVersion, "menuVersion");
          this.menuVersion.Click += new System.EventHandler(this.menuVersion_Click);
          // 
          // toolStripMenuItem1
          // 
          this.toolStripMenuItem1.Name = "toolStripMenuItem1";
          resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
          // 
          // menuInstruction
          // 
          this.menuInstruction.Name = "menuInstruction";
          resources.ApplyResources(this.menuInstruction, "menuInstruction");
          this.menuInstruction.Click += new System.EventHandler(this.menuInstruction_Click);
          // 
          // panel1
          // 
          resources.ApplyResources(this.panel1, "panel1");
          this.panel1.Controls.Add(this.toolStrip1, 0, 0);
          this.panel1.Controls.Add(this.splitPanel, 0, 1);
          this.panel1.Name = "panel1";
          // 
          // toolStrip1
          // 
          this.toolStrip1.BackColor = System.Drawing.SystemColors.ButtonFace;
          this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
          this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlUser,
            this.toolStripSeparator7,
            this.tlOpen,
            this.toolStripSeparator8,
            this.tlSave,
            this.toolStripSeparator9,
            this.tlExit,
            this.toolStripSeparator10,
            this.toolUserLabel,
            this.toolCurUser,
            this.toolStripSeparator11,
            this.toolVersion,
            this.tlToolName,
            this.toolStripSeparator3,
            this.tbrVer});
          resources.ApplyResources(this.toolStrip1, "toolStrip1");
          this.toolStrip1.Name = "toolStrip1";
          // 
          // tlUser
          // 
          resources.ApplyResources(this.tlUser, "tlUser");
          this.tlUser.Name = "tlUser";
          this.tlUser.Click += new System.EventHandler(this.tlUser_Click);
          // 
          // toolStripSeparator7
          // 
          this.toolStripSeparator7.Name = "toolStripSeparator7";
          resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
          // 
          // tlOpen
          // 
          resources.ApplyResources(this.tlOpen, "tlOpen");
          this.tlOpen.Name = "tlOpen";
          this.tlOpen.Click += new System.EventHandler(this.tlOpen_Click);
          // 
          // toolStripSeparator8
          // 
          this.toolStripSeparator8.Name = "toolStripSeparator8";
          resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
          // 
          // tlSave
          // 
          resources.ApplyResources(this.tlSave, "tlSave");
          this.tlSave.Name = "tlSave";
          this.tlSave.Click += new System.EventHandler(this.tlSave_Click);
          // 
          // toolStripSeparator9
          // 
          this.toolStripSeparator9.Name = "toolStripSeparator9";
          resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
          // 
          // tlExit
          // 
          resources.ApplyResources(this.tlExit, "tlExit");
          this.tlExit.Name = "tlExit";
          this.tlExit.Click += new System.EventHandler(this.toolExit_Click);
          // 
          // toolStripSeparator10
          // 
          this.toolStripSeparator10.Name = "toolStripSeparator10";
          resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
          // 
          // toolUserLabel
          // 
          resources.ApplyResources(this.toolUserLabel, "toolUserLabel");
          this.toolUserLabel.Name = "toolUserLabel";
          // 
          // toolCurUser
          // 
          resources.ApplyResources(this.toolCurUser, "toolCurUser");
          this.toolCurUser.ForeColor = System.Drawing.Color.Blue;
          this.toolCurUser.Name = "toolCurUser";
          // 
          // toolStripSeparator11
          // 
          this.toolStripSeparator11.Name = "toolStripSeparator11";
          resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
          // 
          // toolVersion
          // 
          resources.ApplyResources(this.toolVersion, "toolVersion");
          this.toolVersion.Name = "toolVersion";
          // 
          // tlToolName
          // 
          resources.ApplyResources(this.tlToolName, "tlToolName");
          this.tlToolName.ForeColor = System.Drawing.Color.Red;
          this.tlToolName.Name = "tlToolName";
          // 
          // toolStripSeparator3
          // 
          this.toolStripSeparator3.Name = "toolStripSeparator3";
          resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
          // 
          // tbrVer
          // 
          resources.ApplyResources(this.tbrVer, "tbrVer");
          this.tbrVer.Name = "tbrVer";
          // 
          // WndFrmMain
          // 
          resources.ApplyResources(this, "$this");
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.Controls.Add(this.panel1);
          this.Controls.Add(this.menuStrip1);
          this.MainMenuStrip = this.menuStrip1;
          this.Name = "WndFrmMain";
          this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
          this.Load += new System.EventHandler(this.WndFrmMain_Load);
          this.splitPanel.Panel1.ResumeLayout(false);
          ((System.ComponentModel.ISupportInitialize)(this.splitPanel)).EndInit();
          this.splitPanel.ResumeLayout(false);
          this.panel2.ResumeLayout(false);
          this.panel3.ResumeLayout(false);
          this.panel3.PerformLayout();
          this.contextMenuStrip1.ResumeLayout(false);
          this.menuStrip1.ResumeLayout(false);
          this.menuStrip1.PerformLayout();
          this.panel1.ResumeLayout(false);
          this.panel1.PerformLayout();
          this.toolStrip1.ResumeLayout(false);
          this.toolStrip1.PerformLayout();
          this.ResumeLayout(false);
          this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.MenuStrip menuStrip1;
      private System.Windows.Forms.ToolStripMenuItem menuUser;
      private System.Windows.Forms.ToolStripMenuItem menuFile;
      private System.Windows.Forms.ToolStripMenuItem menuOpen;
      private System.Windows.Forms.ToolStripSeparator 保存SToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem menuSave;
      private System.Windows.Forms.ToolStripMenuItem menuLogIn;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
      private System.Windows.Forms.ToolStripMenuItem menuLogOut;
      private System.Windows.Forms.TableLayoutPanel panel1;
      private System.Windows.Forms.ToolStrip toolStrip1;
      private System.Windows.Forms.ToolStripButton tlUser;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
      private System.Windows.Forms.ToolStripButton tlOpen;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
      private System.Windows.Forms.ToolStripButton tlSave;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
      private System.Windows.Forms.ToolStripButton tlExit;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
      private System.Windows.Forms.ToolStripLabel toolUserLabel;
      private System.Windows.Forms.ToolStripLabel toolCurUser;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
      private System.Windows.Forms.ToolStripLabel toolVersion;
      private System.Windows.Forms.SplitContainer splitPanel;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
      private System.Windows.Forms.ToolStripMenuItem menuExit;
      private System.Windows.Forms.ToolStripLabel tlToolName;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
      private System.Windows.Forms.TableLayoutPanel panel2;
      private System.Windows.Forms.TreeView treeView1;
      private System.Windows.Forms.TableLayoutPanel panel3;
      private System.Windows.Forms.ProgressBar progressBar1;
      private System.Windows.Forms.Label labStatus;
      private System.Windows.Forms.ToolStripLabel tbrVer;
      private System.Windows.Forms.ToolStripMenuItem menuLanguage;
      private System.Windows.Forms.ToolStripMenuItem menuChinese;
      private System.Windows.Forms.ToolStripMenuItem menuEnglish;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
      private System.Windows.Forms.ToolStripMenuItem menuHelp;
      private System.Windows.Forms.ToolStripMenuItem menuVersion;
      private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
      private System.Windows.Forms.ToolStripMenuItem menuInstruction;
      private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
      private System.Windows.Forms.ToolStripMenuItem tblrExpandAll;
      private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
      private System.Windows.Forms.ToolStripMenuItem tblrCollapse;
   }
}