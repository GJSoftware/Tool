namespace GJ.TOOL
{
    partial class FrmRemoteIO
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRemoteIO));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.labStatus = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.cmbCom = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBaud = new System.Windows.Forms.TextBox();
            this.btnCon = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAddr = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDevType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtStartAddr = new System.Windows.Forms.TextBox();
            this.txtLen = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtData = new System.Windows.Forms.TextBox();
            this.labRtn = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbVal = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.txtSetAddr = new System.Windows.Forms.TextBox();
            this.btnSetAddr = new System.Windows.Forms.Button();
            this.btnReadAddr = new System.Windows.Forms.Button();
            this.txtSetBaud = new System.Windows.Forms.TextBox();
            this.btnSetBaud = new System.Windows.Forms.Button();
            this.btnReadBaud = new System.Windows.Forms.Button();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.txtSoftVer = new System.Windows.Forms.TextBox();
            this.txtErrCode = new System.Windows.Forms.TextBox();
            this.btnReadVer = new System.Windows.Forms.Button();
            this.btnReadErr = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.DevView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label117 = new System.Windows.Forms.Label();
            this.txtStAddr = new System.Windows.Forms.TextBox();
            this.label118 = new System.Windows.Forms.Label();
            this.txtEndAddr = new System.Windows.Forms.TextBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tbCtlr = new System.Windows.Forms.TabControl();
            this.panel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.labPlcDB = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DevView)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel6.SuspendLayout();
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
            this.panel1.Controls.Add(this.label2, 5, 0);
            this.panel1.Controls.Add(this.labStatus, 6, 0);
            this.panel1.Controls.Add(this.label29, 0, 0);
            this.panel1.Controls.Add(this.cmbCom, 1, 0);
            this.panel1.Controls.Add(this.label1, 0, 1);
            this.panel1.Controls.Add(this.txtBaud, 1, 1);
            this.panel1.Controls.Add(this.btnCon, 2, 0);
            this.panel1.Controls.Add(this.label3, 3, 0);
            this.panel1.Controls.Add(this.txtAddr, 4, 0);
            this.panel1.Name = "panel1";
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
            // label29
            // 
            resources.ApplyResources(this.label29, "label29");
            this.label29.Name = "label29";
            // 
            // cmbCom
            // 
            resources.ApplyResources(this.cmbCom, "cmbCom");
            this.cmbCom.FormattingEnabled = true;
            this.cmbCom.Name = "cmbCom";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtBaud
            // 
            resources.ApplyResources(this.txtBaud, "txtBaud");
            this.txtBaud.Name = "txtBaud";
            // 
            // btnCon
            // 
            resources.ApplyResources(this.btnCon, "btnCon");
            this.btnCon.Name = "btnCon";
            this.btnCon.UseVisualStyleBackColor = true;
            this.btnCon.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txtAddr
            // 
            resources.ApplyResources(this.txtAddr, "txtAddr");
            this.txtAddr.Name = "txtAddr";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.panel4, 0, 2);
            this.panel2.Controls.Add(this.panel3, 0, 1);
            this.panel2.Controls.Add(this.panel5, 0, 0);
            this.panel2.Controls.Add(this.tabControl1, 0, 3);
            this.panel2.Name = "panel2";
            // 
            // panel4
            // 
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Controls.Add(this.label5, 0, 0);
            this.panel4.Controls.Add(this.cmbDevType, 1, 0);
            this.panel4.Controls.Add(this.label6, 2, 0);
            this.panel4.Controls.Add(this.label7, 4, 0);
            this.panel4.Controls.Add(this.txtStartAddr, 3, 0);
            this.panel4.Controls.Add(this.txtLen, 5, 0);
            this.panel4.Controls.Add(this.label8, 6, 0);
            this.panel4.Controls.Add(this.label9, 6, 1);
            this.panel4.Controls.Add(this.txtData, 7, 0);
            this.panel4.Controls.Add(this.labRtn, 7, 1);
            this.panel4.Controls.Add(this.label10, 4, 1);
            this.panel4.Controls.Add(this.cmbVal, 5, 1);
            this.panel4.Name = "panel4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbDevType
            // 
            resources.ApplyResources(this.cmbDevType, "cmbDevType");
            this.cmbDevType.FormattingEnabled = true;
            this.cmbDevType.Name = "cmbDevType";
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
            // txtData
            // 
            resources.ApplyResources(this.txtData, "txtData");
            this.txtData.Name = "txtData";
            // 
            // labRtn
            // 
            resources.ApplyResources(this.labRtn, "labRtn");
            this.labRtn.Name = "labRtn";
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
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.label4, 0, 0);
            this.panel3.Controls.Add(this.btnRead, 2, 0);
            this.panel3.Controls.Add(this.btnWrite, 1, 0);
            this.panel3.Name = "panel3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // btnRead
            // 
            resources.ApplyResources(this.btnRead, "btnRead");
            this.btnRead.Name = "btnRead";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnWrite
            // 
            resources.ApplyResources(this.btnWrite, "btnWrite");
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // panel5
            // 
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Controls.Add(this.label11, 0, 0);
            this.panel5.Controls.Add(this.label30, 0, 1);
            this.panel5.Controls.Add(this.txtSetAddr, 1, 0);
            this.panel5.Controls.Add(this.btnSetAddr, 2, 0);
            this.panel5.Controls.Add(this.btnReadAddr, 3, 0);
            this.panel5.Controls.Add(this.txtSetBaud, 1, 1);
            this.panel5.Controls.Add(this.btnSetBaud, 2, 1);
            this.panel5.Controls.Add(this.btnReadBaud, 3, 1);
            this.panel5.Controls.Add(this.label31, 4, 0);
            this.panel5.Controls.Add(this.label32, 4, 1);
            this.panel5.Controls.Add(this.txtSoftVer, 5, 0);
            this.panel5.Controls.Add(this.txtErrCode, 5, 1);
            this.panel5.Controls.Add(this.btnReadVer, 6, 0);
            this.panel5.Controls.Add(this.btnReadErr, 6, 1);
            this.panel5.Name = "panel5";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.Name = "label30";
            // 
            // txtSetAddr
            // 
            resources.ApplyResources(this.txtSetAddr, "txtSetAddr");
            this.txtSetAddr.Name = "txtSetAddr";
            // 
            // btnSetAddr
            // 
            resources.ApplyResources(this.btnSetAddr, "btnSetAddr");
            this.btnSetAddr.Name = "btnSetAddr";
            this.btnSetAddr.UseVisualStyleBackColor = true;
            this.btnSetAddr.Click += new System.EventHandler(this.btnSetAddr_Click);
            // 
            // btnReadAddr
            // 
            resources.ApplyResources(this.btnReadAddr, "btnReadAddr");
            this.btnReadAddr.Name = "btnReadAddr";
            this.btnReadAddr.UseVisualStyleBackColor = true;
            this.btnReadAddr.Click += new System.EventHandler(this.btnReadAddr_Click);
            // 
            // txtSetBaud
            // 
            resources.ApplyResources(this.txtSetBaud, "txtSetBaud");
            this.txtSetBaud.Name = "txtSetBaud";
            // 
            // btnSetBaud
            // 
            resources.ApplyResources(this.btnSetBaud, "btnSetBaud");
            this.btnSetBaud.Name = "btnSetBaud";
            this.btnSetBaud.UseVisualStyleBackColor = true;
            this.btnSetBaud.Click += new System.EventHandler(this.btnSetBaud_Click);
            // 
            // btnReadBaud
            // 
            resources.ApplyResources(this.btnReadBaud, "btnReadBaud");
            this.btnReadBaud.Name = "btnReadBaud";
            this.btnReadBaud.UseVisualStyleBackColor = true;
            this.btnReadBaud.Click += new System.EventHandler(this.btnReadBaud_Click);
            // 
            // label31
            // 
            resources.ApplyResources(this.label31, "label31");
            this.label31.Name = "label31";
            // 
            // label32
            // 
            resources.ApplyResources(this.label32, "label32");
            this.label32.Name = "label32";
            // 
            // txtSoftVer
            // 
            resources.ApplyResources(this.txtSoftVer, "txtSoftVer");
            this.txtSoftVer.Name = "txtSoftVer";
            // 
            // txtErrCode
            // 
            resources.ApplyResources(this.txtErrCode, "txtErrCode");
            this.txtErrCode.Name = "txtErrCode";
            // 
            // btnReadVer
            // 
            resources.ApplyResources(this.btnReadVer, "btnReadVer");
            this.btnReadVer.Name = "btnReadVer";
            this.btnReadVer.UseVisualStyleBackColor = true;
            this.btnReadVer.Click += new System.EventHandler(this.btnReadVer_Click);
            // 
            // btnReadErr
            // 
            resources.ApplyResources(this.btnReadErr, "btnReadErr");
            this.btnReadErr.Name = "btnReadErr";
            this.btnReadErr.UseVisualStyleBackColor = true;
            this.btnReadErr.Click += new System.EventHandler(this.btnReadErr_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tabPage1.Controls.Add(this.tableLayoutPanel2);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.DevView, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // DevView
            // 
            this.DevView.AllowUserToAddRows = false;
            this.DevView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DevView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DevView.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 10.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DevView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DevView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DevView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 10.5F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DevView.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.DevView, "DevView");
            this.DevView.Name = "DevView";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 10.5F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DevView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DevView.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.DevView.RowTemplate.Height = 23;
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.FillWeight = 120F;
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.FillWeight = 150F;
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.FillWeight = 150F;
            resources.ApplyResources(this.Column4, "Column4");
            this.Column4.Name = "Column4";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.label117, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtStAddr, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label118, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtEndAddr, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnScan, 4, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // label117
            // 
            resources.ApplyResources(this.label117, "label117");
            this.label117.Name = "label117";
            // 
            // txtStAddr
            // 
            resources.ApplyResources(this.txtStAddr, "txtStAddr");
            this.txtStAddr.Name = "txtStAddr";
            // 
            // label118
            // 
            resources.ApplyResources(this.label118, "label118");
            this.label118.Name = "label118";
            // 
            // txtEndAddr
            // 
            resources.ApplyResources(this.txtEndAddr, "txtEndAddr");
            this.txtEndAddr.Name = "txtEndAddr";
            // 
            // btnScan
            // 
            resources.ApplyResources(this.btnScan, "btnScan");
            this.btnScan.Name = "btnScan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tbCtlr, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel6, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tbCtlr
            // 
            resources.ApplyResources(this.tbCtlr, "tbCtlr");
            this.tbCtlr.Name = "tbCtlr";
            this.tbCtlr.SelectedIndex = 0;
            this.tbCtlr.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            // 
            // panel6
            // 
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Controls.Add(this.btnRun, 3, 0);
            this.panel6.Controls.Add(this.btnLoad, 2, 0);
            this.panel6.Controls.Add(this.labPlcDB, 1, 0);
            this.panel6.Controls.Add(this.label33, 0, 0);
            this.panel6.Name = "panel6";
            // 
            // btnRun
            // 
            resources.ApplyResources(this.btnRun, "btnRun");
            this.btnRun.Name = "btnRun";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnLoad
            // 
            resources.ApplyResources(this.btnLoad, "btnLoad");
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // labPlcDB
            // 
            resources.ApplyResources(this.labPlcDB, "labPlcDB");
            this.labPlcDB.BackColor = System.Drawing.Color.White;
            this.labPlcDB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labPlcDB.Name = "labPlcDB";
            // 
            // label33
            // 
            resources.ApplyResources(this.label33, "label33");
            this.label33.Name = "label33";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "L");
            this.imageList1.Images.SetKeyName(1, "H");
            this.imageList1.Images.SetKeyName(2, "F");
            // 
            // FrmRemoteIO
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmRemoteIO";
            this.Load += new System.EventHandler(this.FrmRemoteIO_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DevView)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel panel2;
        private System.Windows.Forms.TableLayoutPanel panel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbDevType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtStartAddr;
        private System.Windows.Forms.TextBox txtLen;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label labRtn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbVal;
        private System.Windows.Forms.TableLayoutPanel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.TableLayoutPanel panel5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox txtSetAddr;
        private System.Windows.Forms.Button btnSetAddr;
        private System.Windows.Forms.Button btnReadAddr;
        private System.Windows.Forms.TextBox txtSetBaud;
        private System.Windows.Forms.Button btnSetBaud;
        private System.Windows.Forms.Button btnReadBaud;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox txtSoftVer;
        private System.Windows.Forms.TextBox txtErrCode;
        private System.Windows.Forms.Button btnReadVer;
        private System.Windows.Forms.Button btnReadErr;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.TableLayoutPanel panel1;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ComboBox cmbCom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBaud;
        private System.Windows.Forms.Button btnCon;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAddr;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel panel6;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label labPlcDB;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TabControl tbCtlr;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView DevView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label117;
        private System.Windows.Forms.TextBox txtStAddr;
        private System.Windows.Forms.Label label118;
        private System.Windows.Forms.TextBox txtEndAddr;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
    }
}