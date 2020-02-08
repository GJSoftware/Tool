namespace GJ.TOOL
{
    partial class FrmI2C
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmI2C));
            this.btnReadVer = new System.Windows.Forms.Button();
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
            this.label2 = new System.Windows.Forms.Label();
            this.labVer = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbPlaceType = new System.Windows.Forms.ComboBox();
            this.cmbReadType = new System.Windows.Forms.ComboBox();
            this.cmbModel = new System.Windows.Forms.ComboBox();
            this.txtI2CAddr = new System.Windows.Forms.TextBox();
            this.txtCmdNum = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtScanTime = new System.Windows.Forms.TextBox();
            this.txtDelayTime = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.cmbUUTNo = new System.Windows.Forms.ComboBox();
            this.btnReadPara = new System.Windows.Forms.Button();
            this.btnSetPara = new System.Windows.Forms.Button();
            this.BtnReadData = new System.Windows.Forms.Button();
            this.chkScan = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.labACON = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.labTimes = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReadVer
            // 
            this.btnReadVer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReadVer.Location = new System.Drawing.Point(370, 31);
            this.btnReadVer.Margin = new System.Windows.Forms.Padding(0);
            this.btnReadVer.Name = "btnReadVer";
            this.btnReadVer.Size = new System.Drawing.Size(72, 30);
            this.btnReadVer.TabIndex = 34;
            this.btnReadVer.Text = "读取版本";
            this.btnReadVer.UseVisualStyleBackColor = true;
            this.btnReadVer.Click += new System.EventHandler(this.btnReadVer_Click);
            // 
            // panel1
            // 
            this.panel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.panel1.ColumnCount = 8;
            this.panel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.panel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.panel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.panel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.panel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.panel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.panel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.panel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel1.Controls.Add(this.labStatus, 7, 0);
            this.panel1.Controls.Add(this.label29, 0, 0);
            this.panel1.Controls.Add(this.cmbCom, 1, 0);
            this.panel1.Controls.Add(this.label1, 0, 1);
            this.panel1.Controls.Add(this.txtBaud, 1, 1);
            this.panel1.Controls.Add(this.btnOpen, 2, 0);
            this.panel1.Controls.Add(this.label3, 3, 0);
            this.panel1.Controls.Add(this.txtAddr, 4, 0);
            this.panel1.Controls.Add(this.btnSetAddr, 5, 0);
            this.panel1.Controls.Add(this.label2, 3, 1);
            this.panel1.Controls.Add(this.labVer, 4, 1);
            this.panel1.Controls.Add(this.label9, 6, 0);
            this.panel1.Controls.Add(this.btnReadVer, 5, 1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.RowCount = 2;
            this.panel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panel1.Size = new System.Drawing.Size(1008, 62);
            this.panel1.TabIndex = 2;
            // 
            // labStatus
            // 
            this.labStatus.AutoSize = true;
            this.labStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.labStatus.Location = new System.Drawing.Point(520, 1);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(0, 29);
            this.labStatus.TabIndex = 9;
            this.labStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label29.Location = new System.Drawing.Point(4, 1);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(56, 29);
            this.label29.TabIndex = 18;
            this.label29.Text = "串口号:";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbCom
            // 
            this.cmbCom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCom.FormattingEnabled = true;
            this.cmbCom.Location = new System.Drawing.Point(67, 4);
            this.cmbCom.Name = "cmbCom";
            this.cmbCom.Size = new System.Drawing.Size(86, 20);
            this.cmbCom.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 30);
            this.label1.TabIndex = 20;
            this.label1.Text = "波特率:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtBaud
            // 
            this.txtBaud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBaud.Location = new System.Drawing.Point(67, 34);
            this.txtBaud.Name = "txtBaud";
            this.txtBaud.Size = new System.Drawing.Size(86, 21);
            this.txtBaud.TabIndex = 21;
            this.txtBaud.Text = "57600,n,8,1";
            this.txtBaud.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnOpen
            // 
            this.btnOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpen.Location = new System.Drawing.Point(157, 1);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(0);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(62, 29);
            this.btnOpen.TabIndex = 22;
            this.btnOpen.Text = "打开";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(223, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 29);
            this.label3.TabIndex = 23;
            this.label3.Text = "I2C地址:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtAddr
            // 
            this.txtAddr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAddr.Location = new System.Drawing.Point(302, 4);
            this.txtAddr.Name = "txtAddr";
            this.txtAddr.Size = new System.Drawing.Size(64, 21);
            this.txtAddr.TabIndex = 24;
            this.txtAddr.Text = "1";
            this.txtAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnSetAddr
            // 
            this.btnSetAddr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSetAddr.Location = new System.Drawing.Point(370, 1);
            this.btnSetAddr.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetAddr.Name = "btnSetAddr";
            this.btnSetAddr.Size = new System.Drawing.Size(72, 29);
            this.btnSetAddr.TabIndex = 25;
            this.btnSetAddr.Text = "设置地址";
            this.btnSetAddr.UseVisualStyleBackColor = true;
            this.btnSetAddr.Click += new System.EventHandler(this.btnSetAddr_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(223, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 30);
            this.label2.TabIndex = 26;
            this.label2.Text = "I2C版本:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labVer
            // 
            this.labVer.AutoSize = true;
            this.labVer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labVer.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labVer.ForeColor = System.Drawing.Color.Green;
            this.labVer.Location = new System.Drawing.Point(302, 31);
            this.labVer.Name = "labVer";
            this.labVer.Size = new System.Drawing.Size(64, 30);
            this.labVer.TabIndex = 27;
            this.labVer.Text = "---";
            this.labVer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(446, 1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 29);
            this.label9.TabIndex = 28;
            this.label9.Text = "状态指示:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 730);
            this.splitContainer1.SplitterDistance = 62;
            this.splitContainer1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.ColumnCount = 2;
            this.panel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 516F));
            this.panel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.RowCount = 1;
            this.panel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel2.Size = new System.Drawing.Size(1008, 664);
            this.panel2.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 212F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(510, 658);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.cmbPlaceType, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbReadType, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmbModel, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtI2CAddr, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtCmdNum, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label10, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label11, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtScanTime, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtDelayTime, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.label18, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.cmbUUTNo, 3, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnReadPara, 3, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnSetPara, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.BtnReadData, 3, 4);
            this.tableLayoutPanel2.Controls.Add(this.chkScan, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.label19, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.labACON, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.label20, 2, 5);
            this.tableLayoutPanel2.Controls.Add(this.labTimes, 3, 5);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(504, 206);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(4, 4);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 27);
            this.label4.TabIndex = 0;
            this.label4.Text = "UUT Place Type:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(4, 38);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 27);
            this.label5.TabIndex = 1;
            this.label5.Text = "Read I2C Type:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(4, 72);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 27);
            this.label6.TabIndex = 2;
            this.label6.Text = "I2C Model:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(4, 106);
            this.label7.Margin = new System.Windows.Forms.Padding(3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 27);
            this.label7.TabIndex = 3;
            this.label7.Text = "I2C Address(Hex):";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(4, 140);
            this.label8.Margin = new System.Windows.Forms.Padding(3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 27);
            this.label8.TabIndex = 4;
            this.label8.Text = "Command Num:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPlaceType
            // 
            this.cmbPlaceType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPlaceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlaceType.FormattingEnabled = true;
            this.cmbPlaceType.Location = new System.Drawing.Point(125, 4);
            this.cmbPlaceType.Name = "cmbPlaceType";
            this.cmbPlaceType.Size = new System.Drawing.Size(118, 20);
            this.cmbPlaceType.TabIndex = 5;
            // 
            // cmbReadType
            // 
            this.cmbReadType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbReadType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReadType.FormattingEnabled = true;
            this.cmbReadType.Items.AddRange(new object[] {
            "不同步OnOff信号",
            "同步OnOff信号",
            "通讯下命令6读取"});
            this.cmbReadType.Location = new System.Drawing.Point(125, 38);
            this.cmbReadType.Name = "cmbReadType";
            this.cmbReadType.Size = new System.Drawing.Size(118, 20);
            this.cmbReadType.TabIndex = 6;
            // 
            // cmbModel
            // 
            this.cmbModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModel.FormattingEnabled = true;
            this.cmbModel.Location = new System.Drawing.Point(125, 72);
            this.cmbModel.Name = "cmbModel";
            this.cmbModel.Size = new System.Drawing.Size(118, 20);
            this.cmbModel.TabIndex = 7;
            // 
            // txtI2CAddr
            // 
            this.txtI2CAddr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtI2CAddr.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtI2CAddr.Location = new System.Drawing.Point(125, 106);
            this.txtI2CAddr.Name = "txtI2CAddr";
            this.txtI2CAddr.Size = new System.Drawing.Size(118, 23);
            this.txtI2CAddr.TabIndex = 8;
            this.txtI2CAddr.Text = "BE";
            this.txtI2CAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCmdNum
            // 
            this.txtCmdNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCmdNum.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCmdNum.Location = new System.Drawing.Point(125, 140);
            this.txtCmdNum.Name = "txtCmdNum";
            this.txtCmdNum.Size = new System.Drawing.Size(118, 23);
            this.txtCmdNum.TabIndex = 9;
            this.txtCmdNum.Text = "8";
            this.txtCmdNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(250, 4);
            this.label10.Margin = new System.Windows.Forms.Padding(3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(124, 27);
            this.label10.TabIndex = 10;
            this.label10.Text = "Scan Cycle Time(S):";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(250, 38);
            this.label11.Margin = new System.Windows.Forms.Padding(3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(124, 27);
            this.label11.TabIndex = 11;
            this.label11.Text = "AC Delay Time(S):";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtScanTime
            // 
            this.txtScanTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtScanTime.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtScanTime.Location = new System.Drawing.Point(381, 4);
            this.txtScanTime.Name = "txtScanTime";
            this.txtScanTime.Size = new System.Drawing.Size(119, 23);
            this.txtScanTime.TabIndex = 12;
            this.txtScanTime.Text = "5";
            this.txtScanTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtDelayTime
            // 
            this.txtDelayTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDelayTime.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDelayTime.Location = new System.Drawing.Point(381, 38);
            this.txtDelayTime.Name = "txtDelayTime";
            this.txtDelayTime.Size = new System.Drawing.Size(119, 23);
            this.txtDelayTime.TabIndex = 13;
            this.txtDelayTime.Text = "3";
            this.txtDelayTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Location = new System.Drawing.Point(250, 72);
            this.label18.Margin = new System.Windows.Forms.Padding(3);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(124, 27);
            this.label18.TabIndex = 14;
            this.label18.Text = "Read UUT Side:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbUUTNo
            // 
            this.cmbUUTNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbUUTNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUUTNo.FormattingEnabled = true;
            this.cmbUUTNo.Location = new System.Drawing.Point(381, 72);
            this.cmbUUTNo.Name = "cmbUUTNo";
            this.cmbUUTNo.Size = new System.Drawing.Size(119, 20);
            this.cmbUUTNo.TabIndex = 15;
            this.cmbUUTNo.SelectedIndexChanged += new System.EventHandler(this.cmbUUTNo_SelectedIndexChanged);
            // 
            // btnReadPara
            // 
            this.btnReadPara.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReadPara.Location = new System.Drawing.Point(378, 103);
            this.btnReadPara.Margin = new System.Windows.Forms.Padding(0);
            this.btnReadPara.Name = "btnReadPara";
            this.btnReadPara.Size = new System.Drawing.Size(125, 33);
            this.btnReadPara.TabIndex = 16;
            this.btnReadPara.Text = "Read I2C Para";
            this.btnReadPara.UseVisualStyleBackColor = true;
            this.btnReadPara.Click += new System.EventHandler(this.btnReadPara_Click);
            // 
            // btnSetPara
            // 
            this.btnSetPara.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSetPara.Location = new System.Drawing.Point(247, 103);
            this.btnSetPara.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetPara.Name = "btnSetPara";
            this.btnSetPara.Size = new System.Drawing.Size(130, 33);
            this.btnSetPara.TabIndex = 17;
            this.btnSetPara.Text = "Set I2C Para";
            this.btnSetPara.UseVisualStyleBackColor = true;
            this.btnSetPara.Click += new System.EventHandler(this.btnSetPara_Click);
            // 
            // BtnReadData
            // 
            this.BtnReadData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnReadData.Location = new System.Drawing.Point(378, 137);
            this.BtnReadData.Margin = new System.Windows.Forms.Padding(0);
            this.BtnReadData.Name = "BtnReadData";
            this.BtnReadData.Size = new System.Drawing.Size(125, 33);
            this.BtnReadData.TabIndex = 18;
            this.BtnReadData.Text = "Start Read";
            this.BtnReadData.UseVisualStyleBackColor = true;
            this.BtnReadData.Click += new System.EventHandler(this.BtnReadData_Click);
            // 
            // chkScan
            // 
            this.chkScan.AutoSize = true;
            this.chkScan.Checked = true;
            this.chkScan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkScan.Location = new System.Drawing.Point(250, 140);
            this.chkScan.Name = "chkScan";
            this.chkScan.Size = new System.Drawing.Size(124, 27);
            this.chkScan.TabIndex = 19;
            this.chkScan.Text = "Continun Scan";
            this.chkScan.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Location = new System.Drawing.Point(4, 174);
            this.label19.Margin = new System.Windows.Forms.Padding(3);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(114, 28);
            this.label19.TabIndex = 20;
            this.label19.Text = "AC ON/OFF:";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labACON
            // 
            this.labACON.AutoSize = true;
            this.labACON.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labACON.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labACON.Location = new System.Drawing.Point(125, 171);
            this.labACON.Name = "labACON";
            this.labACON.Size = new System.Drawing.Size(118, 34);
            this.labACON.TabIndex = 21;
            this.labACON.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Location = new System.Drawing.Point(250, 174);
            this.label20.Margin = new System.Windows.Forms.Padding(3);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(124, 28);
            this.label20.TabIndex = 22;
            this.label20.Text = "Use Times:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labTimes
            // 
            this.labTimes.AutoSize = true;
            this.labTimes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labTimes.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labTimes.Location = new System.Drawing.Point(381, 171);
            this.labTimes.Name = "labTimes";
            this.labTimes.Size = new System.Drawing.Size(119, 34);
            this.labTimes.TabIndex = 23;
            this.labTimes.Text = "0";
            this.labTimes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.panel3.ColumnCount = 6;
            this.panel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.panel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panel3.Controls.Add(this.label12, 0, 0);
            this.panel3.Controls.Add(this.label13, 1, 0);
            this.panel3.Controls.Add(this.label14, 2, 0);
            this.panel3.Controls.Add(this.label15, 3, 0);
            this.panel3.Controls.Add(this.label16, 4, 0);
            this.panel3.Controls.Add(this.label17, 5, 0);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 215);
            this.panel3.Name = "panel3";
            this.panel3.RowCount = 14;
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel3.Size = new System.Drawing.Size(504, 440);
            this.panel3.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(4, 1);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 28);
            this.label12.TabIndex = 0;
            this.label12.Text = "Cmd(Hex)";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(87, 1);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(76, 28);
            this.label13.TabIndex = 1;
            this.label13.Text = "Reg(Hex)";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(170, 1);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 28);
            this.label14.TabIndex = 2;
            this.label14.Text = "Status";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(253, 1);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(76, 28);
            this.label15.TabIndex = 3;
            this.label15.Text = "Byte1";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(336, 1);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(76, 28);
            this.label16.TabIndex = 4;
            this.label16.Text = "Byte0";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(419, 1);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(81, 28);
            this.label17.TabIndex = 5;
            this.label17.Text = "Read Value";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "L");
            this.imageList1.Images.SetKeyName(1, "H");
            this.imageList1.Images.SetKeyName(2, "F");
            // 
            // FrmI2C
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmI2C";
            this.Text = "FrmI2C";
            this.Load += new System.EventHandler(this.FrmI2C_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReadVer;
        private System.Windows.Forms.TableLayoutPanel panel1;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ComboBox cmbCom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBaud;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAddr;
        private System.Windows.Forms.Button btnSetAddr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labVer;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TableLayoutPanel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbPlaceType;
        private System.Windows.Forms.ComboBox cmbReadType;
        private System.Windows.Forms.ComboBox cmbModel;
        private System.Windows.Forms.TextBox txtI2CAddr;
        private System.Windows.Forms.TextBox txtCmdNum;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtScanTime;
        private System.Windows.Forms.TextBox txtDelayTime;
        private System.Windows.Forms.TableLayoutPanel panel3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox cmbUUTNo;
        private System.Windows.Forms.Button btnReadPara;
        private System.Windows.Forms.Button btnSetPara;
        private System.Windows.Forms.Button BtnReadData;
        private System.Windows.Forms.CheckBox chkScan;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label labACON;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label labTimes;
    }
}