namespace GJ.TOOL
{
    partial class FrmLED
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLED));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labStatus = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.cmbCom = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBaud = new System.Windows.Forms.TextBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAddr = new System.Windows.Forms.TextBox();
            this.btnSetAddr = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtStartAddr = new System.Windows.Forms.TextBox();
            this.txtEndAddr = new System.Windows.Forms.TextBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labVersion = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbQCV = new System.Windows.Forms.ComboBox();
            this.btnSetLoad = new System.Windows.Forms.Button();
            this.btnReadLoad = new System.Windows.Forms.Button();
            this.btnVer = new System.Windows.Forms.Button();
            this.btnReadData = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label14 = new System.Windows.Forms.Label();
            this.txtCH = new System.Windows.Forms.TextBox();
            this.btnSetCH = new System.Windows.Forms.Button();
            this.txtDelayS = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.TableLayoutPanel();
            this.DevView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.labLed1 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.labLed2 = new System.Windows.Forms.Label();
            this.labLed3 = new System.Windows.Forms.Label();
            this.labLed4 = new System.Windows.Forms.Label();
            this.labLed5 = new System.Windows.Forms.Label();
            this.labLed6 = new System.Windows.Forms.Label();
            this.btnMonitor = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtMonTime = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DevView)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
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
            this.panel1.Controls.Add(this.labStatus, 7, 0);
            this.panel1.Controls.Add(this.label29, 0, 0);
            this.panel1.Controls.Add(this.cmbCom, 1, 0);
            this.panel1.Controls.Add(this.label1, 0, 2);
            this.panel1.Controls.Add(this.txtBaud, 1, 2);
            this.panel1.Controls.Add(this.btnOpen, 2, 0);
            this.panel1.Controls.Add(this.label3, 3, 0);
            this.panel1.Controls.Add(this.txtAddr, 4, 0);
            this.panel1.Controls.Add(this.btnSetAddr, 5, 0);
            this.panel1.Controls.Add(this.label9, 6, 0);
            this.panel1.Controls.Add(this.panel4, 7, 2);
            this.panel1.Controls.Add(this.label10, 0, 1);
            this.panel1.Controls.Add(this.cmbType, 1, 1);
            this.panel1.Controls.Add(this.label2, 3, 1);
            this.panel1.Controls.Add(this.labVersion, 4, 1);
            this.panel1.Controls.Add(this.label13, 3, 2);
            this.panel1.Controls.Add(this.cmbQCV, 4, 2);
            this.panel1.Controls.Add(this.btnSetLoad, 5, 2);
            this.panel1.Controls.Add(this.btnReadLoad, 6, 2);
            this.panel1.Controls.Add(this.btnVer, 5, 1);
            this.panel1.Controls.Add(this.btnReadData, 6, 1);
            this.panel1.Controls.Add(this.panel5, 7, 1);
            this.panel1.Name = "panel1";
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
            // btnOpen
            // 
            resources.ApplyResources(this.btnOpen, "btnOpen");
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
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
            // btnSetAddr
            // 
            resources.ApplyResources(this.btnSetAddr, "btnSetAddr");
            this.btnSetAddr.Name = "btnSetAddr";
            this.btnSetAddr.UseVisualStyleBackColor = true;
            this.btnSetAddr.Click += new System.EventHandler(this.btnSetAddr_Click);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // panel4
            // 
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Controls.Add(this.label4, 0, 0);
            this.panel4.Controls.Add(this.label5, 2, 0);
            this.panel4.Controls.Add(this.txtStartAddr, 1, 0);
            this.panel4.Controls.Add(this.txtEndAddr, 3, 0);
            this.panel4.Controls.Add(this.btnScan, 4, 0);
            this.panel4.Name = "panel4";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txtStartAddr
            // 
            resources.ApplyResources(this.txtStartAddr, "txtStartAddr");
            this.txtStartAddr.Name = "txtStartAddr";
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
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // cmbType
            // 
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            resources.GetString("cmbType.Items"),
            resources.GetString("cmbType.Items1"),
            resources.GetString("cmbType.Items2"),
            resources.GetString("cmbType.Items3")});
            this.cmbType.Name = "cmbType";
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // labVersion
            // 
            resources.ApplyResources(this.labVersion, "labVersion");
            this.labVersion.Name = "labVersion";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // cmbQCV
            // 
            resources.ApplyResources(this.cmbQCV, "cmbQCV");
            this.cmbQCV.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQCV.FormattingEnabled = true;
            this.cmbQCV.Items.AddRange(new object[] {
            resources.GetString("cmbQCV.Items"),
            resources.GetString("cmbQCV.Items1"),
            resources.GetString("cmbQCV.Items2"),
            resources.GetString("cmbQCV.Items3")});
            this.cmbQCV.Name = "cmbQCV";
            // 
            // btnSetLoad
            // 
            resources.ApplyResources(this.btnSetLoad, "btnSetLoad");
            this.btnSetLoad.Name = "btnSetLoad";
            this.btnSetLoad.UseVisualStyleBackColor = true;
            this.btnSetLoad.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnReadLoad
            // 
            resources.ApplyResources(this.btnReadLoad, "btnReadLoad");
            this.btnReadLoad.Name = "btnReadLoad";
            this.btnReadLoad.UseVisualStyleBackColor = true;
            this.btnReadLoad.Click += new System.EventHandler(this.btnReadLoad_Click);
            // 
            // btnVer
            // 
            resources.ApplyResources(this.btnVer, "btnVer");
            this.btnVer.Name = "btnVer";
            this.btnVer.UseVisualStyleBackColor = true;
            this.btnVer.Click += new System.EventHandler(this.btnVer_Click);
            // 
            // btnReadData
            // 
            resources.ApplyResources(this.btnReadData, "btnReadData");
            this.btnReadData.Name = "btnReadData";
            this.btnReadData.UseVisualStyleBackColor = true;
            this.btnReadData.Click += new System.EventHandler(this.btnReadData_Click);
            // 
            // panel5
            // 
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Controls.Add(this.label14, 0, 0);
            this.panel5.Controls.Add(this.txtCH, 1, 0);
            this.panel5.Controls.Add(this.btnSetCH, 2, 0);
            this.panel5.Controls.Add(this.txtDelayS, 4, 0);
            this.panel5.Controls.Add(this.label15, 3, 0);
            this.panel5.Name = "panel5";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // txtCH
            // 
            resources.ApplyResources(this.txtCH, "txtCH");
            this.txtCH.Name = "txtCH";
            // 
            // btnSetCH
            // 
            resources.ApplyResources(this.btnSetCH, "btnSetCH");
            this.btnSetCH.Name = "btnSetCH";
            this.btnSetCH.UseVisualStyleBackColor = true;
            this.btnSetCH.Click += new System.EventHandler(this.btnSetCH_Click);
            // 
            // txtDelayS
            // 
            resources.ApplyResources(this.txtDelayS, "txtDelayS");
            this.txtDelayS.Name = "txtDelayS";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.DevView, 1, 0);
            this.panel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.panel2.Name = "panel2";
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
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            resources.ApplyResources(this.Column4, "Column4");
            this.Column4.Name = "Column4";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.label6, 0, 0);
            this.panel3.Controls.Add(this.label7, 2, 0);
            this.panel3.Controls.Add(this.label11, 1, 0);
            this.panel3.Controls.Add(this.label12, 3, 0);
            this.panel3.Controls.Add(this.label8, 5, 0);
            this.panel3.Controls.Add(this.label22, 4, 0);
            this.panel3.Name = "panel3";
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
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.label16, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label17, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label18, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label19, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label20, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label21, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.labLed1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.labLed2, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.labLed3, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.labLed4, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.labLed5, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.labLed6, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnMonitor, 4, 3);
            this.tableLayoutPanel2.Controls.Add(this.panel6, 4, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // labLed1
            // 
            resources.ApplyResources(this.labLed1, "labLed1");
            this.labLed1.ImageList = this.imageList1;
            this.labLed1.Name = "labLed1";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "L");
            this.imageList1.Images.SetKeyName(1, "H");
            this.imageList1.Images.SetKeyName(2, "F");
            // 
            // labLed2
            // 
            resources.ApplyResources(this.labLed2, "labLed2");
            this.labLed2.ImageList = this.imageList1;
            this.labLed2.Name = "labLed2";
            // 
            // labLed3
            // 
            resources.ApplyResources(this.labLed3, "labLed3");
            this.labLed3.ImageList = this.imageList1;
            this.labLed3.Name = "labLed3";
            // 
            // labLed4
            // 
            resources.ApplyResources(this.labLed4, "labLed4");
            this.labLed4.ImageList = this.imageList1;
            this.labLed4.Name = "labLed4";
            // 
            // labLed5
            // 
            resources.ApplyResources(this.labLed5, "labLed5");
            this.labLed5.ImageList = this.imageList1;
            this.labLed5.Name = "labLed5";
            // 
            // labLed6
            // 
            resources.ApplyResources(this.labLed6, "labLed6");
            this.labLed6.ImageList = this.imageList1;
            this.labLed6.Name = "labLed6";
            // 
            // btnMonitor
            // 
            resources.ApplyResources(this.btnMonitor, "btnMonitor");
            this.btnMonitor.Name = "btnMonitor";
            this.btnMonitor.UseVisualStyleBackColor = true;
            this.btnMonitor.Click += new System.EventHandler(this.btnMonitor_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.txtMonTime);
            this.panel6.Controls.Add(this.label23);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // txtMonTime
            // 
            resources.ApplyResources(this.txtMonTime, "txtMonTime");
            this.txtMonTime.Name = "txtMonTime";
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmLED
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmLED";
            this.Load += new System.EventHandler(this.FrmLED_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DevView)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DevView;
        private System.Windows.Forms.TableLayoutPanel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel panel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtStartAddr;
        private System.Windows.Forms.TextBox txtEndAddr;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ComboBox cmbCom;
        private System.Windows.Forms.TextBox txtBaud;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAddr;
        private System.Windows.Forms.Button btnSetAddr;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labVersion;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbQCV;
        private System.Windows.Forms.Button btnSetLoad;
        private System.Windows.Forms.Button btnReadLoad;
        private System.Windows.Forms.Button btnVer;
        private System.Windows.Forms.Button btnReadData;
        private System.Windows.Forms.TableLayoutPanel panel5;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtCH;
        private System.Windows.Forms.Button btnSetCH;
        private System.Windows.Forms.TextBox txtDelayS;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel panel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label labLed1;
        private System.Windows.Forms.Label labLed2;
        private System.Windows.Forms.Label labLed3;
        private System.Windows.Forms.Label labLed4;
        private System.Windows.Forms.Label labLed5;
        private System.Windows.Forms.Label labLed6;
        private System.Windows.Forms.Button btnMonitor;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox txtMonTime;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
    }
}