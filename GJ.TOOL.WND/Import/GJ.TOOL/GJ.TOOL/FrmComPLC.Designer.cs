namespace GJ.TOOL
{
    partial class FrmComPLC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmComPLC));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCon = new System.Windows.Forms.Button();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.txtBaud = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labStatus = new System.Windows.Forms.Label();
            this.labTimes = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtAddr = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.cmbPLCType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCom = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labBaud = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbRegType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtStartAddr = new System.Windows.Forms.TextBox();
            this.txtLen = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbVal = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.TableLayoutPanel();
            this.txtW = new System.Windows.Forms.TextBox();
            this.labT1 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.TableLayoutPanel();
            this.txtR = new System.Windows.Forms.TextBox();
            this.labT2 = new System.Windows.Forms.Label();
            this.btnW = new System.Windows.Forms.Button();
            this.btnR = new System.Windows.Forms.Button();
            this.label33 = new System.Windows.Forms.Label();
            this.cmbDataType = new System.Windows.Forms.ComboBox();
            this.txtBin = new System.Windows.Forms.TextBox();
            this.tbCtlr = new System.Windows.Forms.TabControl();
            this.panel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.labPlcDB = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tmrPLC = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.btnCon, 4, 0);
            this.panel1.Controls.Add(this.txtBaud, 3, 1);
            this.panel1.Controls.Add(this.label2, 5, 0);
            this.panel1.Controls.Add(this.labStatus, 6, 0);
            this.panel1.Controls.Add(this.labTimes, 6, 1);
            this.panel1.Controls.Add(this.label11, 5, 1);
            this.panel1.Controls.Add(this.txtAddr, 3, 0);
            this.panel1.Controls.Add(this.label29, 0, 0);
            this.panel1.Controls.Add(this.cmbPLCType, 1, 0);
            this.panel1.Controls.Add(this.label1, 0, 1);
            this.panel1.Controls.Add(this.cmbCom, 1, 1);
            this.panel1.Controls.Add(this.label4, 2, 0);
            this.panel1.Controls.Add(this.labBaud, 2, 1);
            this.panel1.Name = "panel1";
            // 
            // btnCon
            // 
            resources.ApplyResources(this.btnCon, "btnCon");
            this.btnCon.ImageList = this.imageList2;
            this.btnCon.Name = "btnCon";
            this.btnCon.UseVisualStyleBackColor = true;
            this.btnCon.Click += new System.EventHandler(this.btnCon_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "ON");
            this.imageList2.Images.SetKeyName(1, "OFF");
            // 
            // txtBaud
            // 
            resources.ApplyResources(this.txtBaud, "txtBaud");
            this.txtBaud.Name = "txtBaud";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // labStatus
            // 
            resources.ApplyResources(this.labStatus, "labStatus");
            this.labStatus.Name = "labStatus";
            // 
            // labTimes
            // 
            resources.ApplyResources(this.labTimes, "labTimes");
            this.labTimes.Name = "labTimes";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // txtAddr
            // 
            resources.ApplyResources(this.txtAddr, "txtAddr");
            this.txtAddr.Name = "txtAddr";
            // 
            // label29
            // 
            resources.ApplyResources(this.label29, "label29");
            this.label29.Name = "label29";
            // 
            // cmbPLCType
            // 
            resources.ApplyResources(this.cmbPLCType, "cmbPLCType");
            this.cmbPLCType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPLCType.FormattingEnabled = true;
            this.cmbPLCType.Items.AddRange(new object[] {
            resources.GetString("cmbPLCType.Items"),
            resources.GetString("cmbPLCType.Items1"),
            resources.GetString("cmbPLCType.Items2"),
            resources.GetString("cmbPLCType.Items3"),
            resources.GetString("cmbPLCType.Items4"),
            resources.GetString("cmbPLCType.Items5"),
            resources.GetString("cmbPLCType.Items6"),
            resources.GetString("cmbPLCType.Items7"),
            resources.GetString("cmbPLCType.Items8")});
            this.cmbPLCType.Name = "cmbPLCType";
            this.cmbPLCType.SelectedIndexChanged += new System.EventHandler(this.cmbPLCType_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbCom
            // 
            resources.ApplyResources(this.cmbCom, "cmbCom");
            this.cmbCom.FormattingEnabled = true;
            this.cmbCom.Name = "cmbCom";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // labBaud
            // 
            resources.ApplyResources(this.labBaud, "labBaud");
            this.labBaud.Name = "labBaud";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.panel4, 0, 0);
            this.panel2.Controls.Add(this.tbCtlr, 0, 2);
            this.panel2.Controls.Add(this.panel3, 0, 1);
            this.panel2.Name = "panel2";
            // 
            // panel4
            // 
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Controls.Add(this.label5, 0, 0);
            this.panel4.Controls.Add(this.cmbRegType, 1, 0);
            this.panel4.Controls.Add(this.label6, 2, 0);
            this.panel4.Controls.Add(this.label7, 4, 0);
            this.panel4.Controls.Add(this.txtStartAddr, 3, 0);
            this.panel4.Controls.Add(this.txtLen, 5, 0);
            this.panel4.Controls.Add(this.label8, 6, 0);
            this.panel4.Controls.Add(this.label9, 6, 1);
            this.panel4.Controls.Add(this.label10, 4, 1);
            this.panel4.Controls.Add(this.cmbVal, 5, 1);
            this.panel4.Controls.Add(this.label30, 2, 1);
            this.panel4.Controls.Add(this.panel5, 7, 0);
            this.panel4.Controls.Add(this.panel6, 7, 1);
            this.panel4.Controls.Add(this.btnW, 8, 0);
            this.panel4.Controls.Add(this.btnR, 8, 1);
            this.panel4.Controls.Add(this.label33, 0, 1);
            this.panel4.Controls.Add(this.cmbDataType, 1, 1);
            this.panel4.Controls.Add(this.txtBin, 3, 1);
            this.panel4.Name = "panel4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbRegType
            // 
            resources.ApplyResources(this.cmbRegType, "cmbRegType");
            this.cmbRegType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegType.FormattingEnabled = true;
            this.cmbRegType.Items.AddRange(new object[] {
            resources.GetString("cmbRegType.Items"),
            resources.GetString("cmbRegType.Items1"),
            resources.GetString("cmbRegType.Items2"),
            resources.GetString("cmbRegType.Items3"),
            resources.GetString("cmbRegType.Items4"),
            resources.GetString("cmbRegType.Items5")});
            this.cmbRegType.Name = "cmbRegType";
            this.cmbRegType.SelectedIndexChanged += new System.EventHandler(this.cmbRegType_SelectedIndexChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // txtStartAddr
            // 
            resources.ApplyResources(this.txtStartAddr, "txtStartAddr");
            this.txtStartAddr.Name = "txtStartAddr";
            // 
            // txtLen
            // 
            resources.ApplyResources(this.txtLen, "txtLen");
            this.txtLen.Name = "txtLen";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // cmbVal
            // 
            resources.ApplyResources(this.cmbVal, "cmbVal");
            this.cmbVal.FormattingEnabled = true;
            this.cmbVal.Name = "cmbVal";
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.Name = "label30";
            // 
            // panel5
            // 
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Controls.Add(this.txtW, 0, 0);
            this.panel5.Controls.Add(this.labT1, 1, 0);
            this.panel5.Name = "panel5";
            // 
            // txtW
            // 
            resources.ApplyResources(this.txtW, "txtW");
            this.txtW.Name = "txtW";
            // 
            // labT1
            // 
            resources.ApplyResources(this.labT1, "labT1");
            this.labT1.ForeColor = System.Drawing.Color.Blue;
            this.labT1.Name = "labT1";
            // 
            // panel6
            // 
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Controls.Add(this.txtR, 0, 0);
            this.panel6.Controls.Add(this.labT2, 1, 0);
            this.panel6.Name = "panel6";
            // 
            // txtR
            // 
            resources.ApplyResources(this.txtR, "txtR");
            this.txtR.Name = "txtR";
            // 
            // labT2
            // 
            resources.ApplyResources(this.labT2, "labT2");
            this.labT2.ForeColor = System.Drawing.Color.Blue;
            this.labT2.Name = "labT2";
            // 
            // btnW
            // 
            resources.ApplyResources(this.btnW, "btnW");
            this.btnW.Name = "btnW";
            this.btnW.UseVisualStyleBackColor = true;
            this.btnW.Click += new System.EventHandler(this.btnW_Click);
            // 
            // btnR
            // 
            resources.ApplyResources(this.btnR, "btnR");
            this.btnR.Name = "btnR";
            this.btnR.UseVisualStyleBackColor = true;
            this.btnR.Click += new System.EventHandler(this.btnR_Click);
            // 
            // label33
            // 
            resources.ApplyResources(this.label33, "label33");
            this.label33.Name = "label33";
            // 
            // cmbDataType
            // 
            this.cmbDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbDataType, "cmbDataType");
            this.cmbDataType.FormattingEnabled = true;
            this.cmbDataType.Items.AddRange(new object[] {
            resources.GetString("cmbDataType.Items"),
            resources.GetString("cmbDataType.Items1")});
            this.cmbDataType.Name = "cmbDataType";
            this.cmbDataType.SelectedIndexChanged += new System.EventHandler(this.cmbDataType_SelectedIndexChanged);
            // 
            // txtBin
            // 
            resources.ApplyResources(this.txtBin, "txtBin");
            this.txtBin.Name = "txtBin";
            // 
            // tbCtlr
            // 
            resources.ApplyResources(this.tbCtlr, "tbCtlr");
            this.tbCtlr.Name = "tbCtlr";
            this.tbCtlr.SelectedIndex = 0;
            this.tbCtlr.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.label3, 0, 0);
            this.panel3.Controls.Add(this.btnLoad, 2, 0);
            this.panel3.Controls.Add(this.btnRun, 3, 0);
            this.panel3.Controls.Add(this.labPlcDB, 1, 0);
            this.panel3.Name = "panel3";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnLoad
            // 
            resources.ApplyResources(this.btnLoad, "btnLoad");
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnRun
            // 
            resources.ApplyResources(this.btnRun, "btnRun");
            this.btnRun.Name = "btnRun";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // labPlcDB
            // 
            resources.ApplyResources(this.labPlcDB, "labPlcDB");
            this.labPlcDB.BackColor = System.Drawing.Color.White;
            this.labPlcDB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labPlcDB.Name = "labPlcDB";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "L");
            this.imageList1.Images.SetKeyName(1, "H");
            this.imageList1.Images.SetKeyName(2, "F");
            // 
            // tmrPLC
            // 
            this.tmrPLC.Interval = 500;
            // 
            // FrmComPLC
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "FrmComPLC";
            this.Load += new System.EventHandler(this.FrmComPLC_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCon;
        private System.Windows.Forms.TextBox txtBaud;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.Label labTimes;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbVal;
        private System.Windows.Forms.TextBox txtAddr;
        private System.Windows.Forms.Label labPlcDB;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TableLayoutPanel panel3;
        private System.Windows.Forms.TableLayoutPanel panel4;
        private System.Windows.Forms.ComboBox cmbRegType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtStartAddr;
        private System.Windows.Forms.TextBox txtLen;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel panel2;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ComboBox cmbPLCType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCom;
        private System.Windows.Forms.TableLayoutPanel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Timer tmrPLC;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labBaud;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TableLayoutPanel panel5;
        private System.Windows.Forms.TextBox txtW;
        private System.Windows.Forms.Label labT1;
        private System.Windows.Forms.TableLayoutPanel panel6;
        private System.Windows.Forms.TextBox txtR;
        private System.Windows.Forms.Label labT2;
        private System.Windows.Forms.Button btnW;
        private System.Windows.Forms.Button btnR;
        private System.Windows.Forms.TabControl tbCtlr;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.ComboBox cmbDataType;
        private System.Windows.Forms.TextBox txtBin;
        private System.Windows.Forms.ImageList imageList2;
    }
}