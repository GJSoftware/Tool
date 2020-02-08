namespace GJ.TOOL
{
    partial class FrmHWSocket
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHWSocket));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSerPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSerXML = new System.Windows.Forms.TextBox();
            this.labSerStatus = new System.Windows.Forms.Label();
            this.btnListen = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.BtnLoad = new System.Windows.Forms.Button();
            this.SerLog = new GJ.UI.udcRunLog();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labSerDevName = new System.Windows.Forms.Label();
            this.labSerCmdNo = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.labWR = new System.Windows.Forms.Label();
            this.listSer = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSerIP = new System.Windows.Forms.TextBox();
            this.txtClientPort = new System.Windows.Forms.TextBox();
            this.labClientStatus = new System.Windows.Forms.Label();
            this.btnCon = new System.Windows.Forms.Button();
            this.btnSendCmd = new System.Windows.Forms.Button();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbDevType = new System.Windows.Forms.ComboBox();
            this.cmbCmdNo = new System.Windows.Forms.ComboBox();
            this.txtSendCmd = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtRecvCmd = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtRevcVal = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtCmdData = new System.Windows.Forms.TextBox();
            this.clientLog = new GJ.UI.udcRunLog();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.85931F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.14069F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.66805F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.33195F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1227, 966);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(5, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(590, 44);
            this.label1.TabIndex = 0;
            this.label1.Text = "Socket服务端";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("宋体", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(603, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(619, 44);
            this.label2.TabIndex = 1;
            this.label2.Text = "Socket客户端";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.SerLog, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 51);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 271F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(590, 910);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtSerPort, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtSerXML, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.labSerStatus, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.btnListen, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.BtnLoad, 2, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(584, 96);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(4, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 30);
            this.label3.TabIndex = 0;
            this.label3.Text = "TCP端口:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(4, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 30);
            this.label4.TabIndex = 1;
            this.label4.Text = "加载XML:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSerPort
            // 
            this.txtSerPort.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtSerPort.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSerPort.Location = new System.Drawing.Point(103, 4);
            this.txtSerPort.Name = "txtSerPort";
            this.txtSerPort.Size = new System.Drawing.Size(138, 26);
            this.txtSerPort.TabIndex = 2;
            this.txtSerPort.Text = "8000";
            this.txtSerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(4, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 32);
            this.label5.TabIndex = 3;
            this.label5.Text = "状态提示:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSerXML
            // 
            this.txtSerXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSerXML.Location = new System.Drawing.Point(103, 35);
            this.txtSerXML.Name = "txtSerXML";
            this.txtSerXML.Size = new System.Drawing.Size(378, 23);
            this.txtSerXML.TabIndex = 4;
            // 
            // labSerStatus
            // 
            this.labSerStatus.AutoSize = true;
            this.labSerStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labSerStatus.Location = new System.Drawing.Point(103, 63);
            this.labSerStatus.Name = "labSerStatus";
            this.labSerStatus.Size = new System.Drawing.Size(378, 32);
            this.labSerStatus.TabIndex = 5;
            this.labSerStatus.Text = "--";
            this.labSerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnListen
            // 
            this.btnListen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnListen.ImageKey = "DisConnect";
            this.btnListen.ImageList = this.imageList1;
            this.btnListen.Location = new System.Drawing.Point(485, 1);
            this.btnListen.Margin = new System.Windows.Forms.Padding(0);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(98, 30);
            this.btnListen.TabIndex = 6;
            this.btnListen.Text = "监听";
            this.btnListen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Connect");
            this.imageList1.Images.SetKeyName(1, "DisConnect");
            this.imageList1.Images.SetKeyName(2, "Open");
            this.imageList1.Images.SetKeyName(3, "Edit.ICO");
            // 
            // BtnLoad
            // 
            this.BtnLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnLoad.ImageKey = "Open";
            this.BtnLoad.ImageList = this.imageList1;
            this.BtnLoad.Location = new System.Drawing.Point(485, 32);
            this.BtnLoad.Margin = new System.Windows.Forms.Padding(0);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(98, 30);
            this.BtnLoad.TabIndex = 7;
            this.BtnLoad.Text = "加载";
            this.BtnLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // SerLog
            // 
            this.SerLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SerLog.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SerLog.Location = new System.Drawing.Point(3, 105);
            this.SerLog.mFont = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SerLog.mMaxLine = 1000;
            this.SerLog.mMaxMB = 1D;
            this.SerLog.mSaveEnable = true;
            this.SerLog.mSaveFolder = "";
            this.SerLog.mSaveName = "SerLog";
            this.SerLog.mTitle = "运行日志";
            this.SerLog.mTitleEnable = true;
            this.SerLog.Name = "SerLog";
            this.SerLog.Size = new System.Drawing.Size(584, 496);
            this.SerLog.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(3, 604);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(584, 35);
            this.label6.TabIndex = 2;
            this.label6.Text = "报文数据解析";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.listSer, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 642);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.84906F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.15094F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(584, 265);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel5.ColumnCount = 6;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.Controls.Add(this.label7, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.label8, 4, 0);
            this.tableLayoutPanel5.Controls.Add(this.labSerDevName, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.labSerCmdNo, 5, 0);
            this.tableLayoutPanel5.Controls.Add(this.label18, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.labWR, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(578, 36);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(196, 1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 34);
            this.label7.TabIndex = 0;
            this.label7.Text = "设备类型:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(388, 1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 34);
            this.label8.TabIndex = 1;
            this.label8.Text = "命令字:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labSerDevName
            // 
            this.labSerDevName.AutoSize = true;
            this.labSerDevName.BackColor = System.Drawing.Color.White;
            this.labSerDevName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labSerDevName.Location = new System.Drawing.Point(294, 1);
            this.labSerDevName.Margin = new System.Windows.Forms.Padding(0);
            this.labSerDevName.Name = "labSerDevName";
            this.labSerDevName.Size = new System.Drawing.Size(90, 34);
            this.labSerDevName.TabIndex = 2;
            this.labSerDevName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labSerCmdNo
            // 
            this.labSerCmdNo.AutoSize = true;
            this.labSerCmdNo.BackColor = System.Drawing.Color.White;
            this.labSerCmdNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labSerCmdNo.Location = new System.Drawing.Point(486, 1);
            this.labSerCmdNo.Margin = new System.Windows.Forms.Padding(0);
            this.labSerCmdNo.Name = "labSerCmdNo";
            this.labSerCmdNo.Size = new System.Drawing.Size(91, 34);
            this.labSerCmdNo.TabIndex = 3;
            this.labSerCmdNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Location = new System.Drawing.Point(4, 1);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(94, 34);
            this.label18.TabIndex = 4;
            this.label18.Text = "读写状态:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labWR
            // 
            this.labWR.AutoSize = true;
            this.labWR.BackColor = System.Drawing.Color.White;
            this.labWR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labWR.Location = new System.Drawing.Point(102, 1);
            this.labWR.Margin = new System.Windows.Forms.Padding(0);
            this.labWR.Name = "labWR";
            this.labWR.Size = new System.Drawing.Size(90, 34);
            this.labWR.TabIndex = 5;
            this.labWR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listSer
            // 
            this.listSer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSer.FormattingEnabled = true;
            this.listSer.ItemHeight = 14;
            this.listSer.Location = new System.Drawing.Point(3, 45);
            this.listSer.Name = "listSer";
            this.listSer.Size = new System.Drawing.Size(578, 217);
            this.listSer.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel8, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.clientLog, 0, 2);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(603, 51);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 3;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 211F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(619, 910);
            this.tableLayoutPanel6.TabIndex = 3;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel7.ColumnCount = 3;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanel7.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.label10, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.label11, 0, 2);
            this.tableLayoutPanel7.Controls.Add(this.txtSerIP, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.txtClientPort, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.labClientStatus, 1, 2);
            this.tableLayoutPanel7.Controls.Add(this.btnCon, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.btnSendCmd, 2, 2);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 3;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(613, 98);
            this.tableLayoutPanel7.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(4, 1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 31);
            this.label9.TabIndex = 0;
            this.label9.Text = "服务端IP:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(4, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(78, 31);
            this.label10.TabIndex = 1;
            this.label10.Text = "服务端口:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(4, 65);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 32);
            this.label11.TabIndex = 2;
            this.label11.Text = "状态指示:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSerIP
            // 
            this.txtSerIP.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtSerIP.Location = new System.Drawing.Point(89, 4);
            this.txtSerIP.Name = "txtSerIP";
            this.txtSerIP.Size = new System.Drawing.Size(161, 23);
            this.txtSerIP.TabIndex = 3;
            this.txtSerIP.Text = "127.0.0.1";
            this.txtSerIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtClientPort
            // 
            this.txtClientPort.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtClientPort.Location = new System.Drawing.Point(89, 36);
            this.txtClientPort.Name = "txtClientPort";
            this.txtClientPort.Size = new System.Drawing.Size(100, 23);
            this.txtClientPort.TabIndex = 4;
            this.txtClientPort.Text = "8000";
            this.txtClientPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labClientStatus
            // 
            this.labClientStatus.AutoSize = true;
            this.labClientStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labClientStatus.Location = new System.Drawing.Point(89, 65);
            this.labClientStatus.Name = "labClientStatus";
            this.labClientStatus.Size = new System.Drawing.Size(433, 32);
            this.labClientStatus.TabIndex = 5;
            this.labClientStatus.Text = "--";
            this.labClientStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCon
            // 
            this.btnCon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCon.ImageKey = "DisConnect";
            this.btnCon.ImageList = this.imageList1;
            this.btnCon.Location = new System.Drawing.Point(526, 1);
            this.btnCon.Margin = new System.Windows.Forms.Padding(0);
            this.btnCon.Name = "btnCon";
            this.btnCon.Size = new System.Drawing.Size(86, 31);
            this.btnCon.TabIndex = 6;
            this.btnCon.Text = "连接";
            this.btnCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCon.UseVisualStyleBackColor = true;
            this.btnCon.Click += new System.EventHandler(this.btnCon_Click);
            // 
            // btnSendCmd
            // 
            this.btnSendCmd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSendCmd.Enabled = false;
            this.btnSendCmd.ImageKey = "Edit.ICO";
            this.btnSendCmd.ImageList = this.imageList1;
            this.btnSendCmd.Location = new System.Drawing.Point(526, 65);
            this.btnSendCmd.Margin = new System.Windows.Forms.Padding(0);
            this.btnSendCmd.Name = "btnSendCmd";
            this.btnSendCmd.Size = new System.Drawing.Size(86, 32);
            this.btnSendCmd.TabIndex = 7;
            this.btnSendCmd.Text = "发送";
            this.btnSendCmd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSendCmd.UseVisualStyleBackColor = true;
            this.btnSendCmd.Click += new System.EventHandler(this.btnSendCmd_Click);
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.label12, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.label13, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.label14, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.cmbDevType, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.cmbCmdNo, 1, 1);
            this.tableLayoutPanel8.Controls.Add(this.txtSendCmd, 1, 3);
            this.tableLayoutPanel8.Controls.Add(this.label15, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.txtRecvCmd, 1, 4);
            this.tableLayoutPanel8.Controls.Add(this.label16, 0, 5);
            this.tableLayoutPanel8.Controls.Add(this.txtRevcVal, 1, 5);
            this.tableLayoutPanel8.Controls.Add(this.label17, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.txtCmdData, 1, 2);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 107);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 6;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(613, 205);
            this.tableLayoutPanel8.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(4, 1);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(127, 32);
            this.label12.TabIndex = 0;
            this.label12.Text = "设备类型(Hex):";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(4, 34);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(127, 32);
            this.label13.TabIndex = 1;
            this.label13.Text = "命令字(Hex):";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Location = new System.Drawing.Point(4, 100);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(127, 32);
            this.label14.TabIndex = 2;
            this.label14.Text = "发送数据(字符串):";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDevType
            // 
            this.cmbDevType.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbDevType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevType.FormattingEnabled = true;
            this.cmbDevType.Items.AddRange(new object[] {
            "整体(0x00)",
            "上料位(0x02)",
            "下料位(0x03)",
            "移载机(0x04)",
            "老化位(0x05)",
            "缓存位(0x08)",
            "治具(0x09)"});
            this.cmbDevType.Location = new System.Drawing.Point(138, 4);
            this.cmbDevType.Name = "cmbDevType";
            this.cmbDevType.Size = new System.Drawing.Size(182, 22);
            this.cmbDevType.TabIndex = 3;
            this.cmbDevType.SelectedIndexChanged += new System.EventHandler(this.cmbDevType_SelectedIndexChanged);
            // 
            // cmbCmdNo
            // 
            this.cmbCmdNo.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbCmdNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCmdNo.FormattingEnabled = true;
            this.cmbCmdNo.Location = new System.Drawing.Point(138, 37);
            this.cmbCmdNo.Name = "cmbCmdNo";
            this.cmbCmdNo.Size = new System.Drawing.Size(182, 22);
            this.cmbCmdNo.TabIndex = 4;
            // 
            // txtSendCmd
            // 
            this.txtSendCmd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSendCmd.Location = new System.Drawing.Point(138, 103);
            this.txtSendCmd.Name = "txtSendCmd";
            this.txtSendCmd.Size = new System.Drawing.Size(471, 23);
            this.txtSendCmd.TabIndex = 5;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Location = new System.Drawing.Point(4, 133);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(127, 32);
            this.label15.TabIndex = 6;
            this.label15.Text = "接收数据(字符串):";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRecvCmd
            // 
            this.txtRecvCmd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRecvCmd.Location = new System.Drawing.Point(138, 136);
            this.txtRecvCmd.Name = "txtRecvCmd";
            this.txtRecvCmd.Size = new System.Drawing.Size(471, 23);
            this.txtRecvCmd.TabIndex = 7;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Location = new System.Drawing.Point(4, 166);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(127, 38);
            this.label16.TabIndex = 8;
            this.label16.Text = "接收数据解析:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRevcVal
            // 
            this.txtRevcVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRevcVal.Location = new System.Drawing.Point(138, 169);
            this.txtRevcVal.Name = "txtRevcVal";
            this.txtRevcVal.Size = new System.Drawing.Size(471, 23);
            this.txtRevcVal.TabIndex = 9;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Location = new System.Drawing.Point(4, 67);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(127, 32);
            this.label17.TabIndex = 10;
            this.label17.Text = "命令数据(字符串):";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCmdData
            // 
            this.txtCmdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCmdData.Location = new System.Drawing.Point(138, 70);
            this.txtCmdData.Name = "txtCmdData";
            this.txtCmdData.Size = new System.Drawing.Size(471, 23);
            this.txtCmdData.TabIndex = 11;
            this.txtCmdData.Text = "11;12";
            // 
            // clientLog
            // 
            this.clientLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientLog.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clientLog.Location = new System.Drawing.Point(3, 318);
            this.clientLog.mFont = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clientLog.mMaxLine = 1000;
            this.clientLog.mMaxMB = 1D;
            this.clientLog.mSaveEnable = true;
            this.clientLog.mSaveFolder = "";
            this.clientLog.mSaveName = "ClientLog";
            this.clientLog.mTitle = "运行日志";
            this.clientLog.mTitleEnable = true;
            this.clientLog.Name = "clientLog";
            this.clientLog.Size = new System.Drawing.Size(613, 589);
            this.clientLog.TabIndex = 2;
            // 
            // FrmHWSocket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1227, 966);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmHWSocket";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "华为AP项目Socket通信协议";
            this.Load += new System.EventHandler(this.FrmHWSocket_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSerPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSerXML;
        private System.Windows.Forms.Label labSerStatus;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button BtnLoad;
        private UI.udcRunLog SerLog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labSerDevName;
        private System.Windows.Forms.Label labSerCmdNo;
        private System.Windows.Forms.ListBox listSer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtSerIP;
        private System.Windows.Forms.TextBox txtClientPort;
        private System.Windows.Forms.Label labClientStatus;
        private System.Windows.Forms.Button btnCon;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbDevType;
        private System.Windows.Forms.Button btnSendCmd;
        private System.Windows.Forms.ComboBox cmbCmdNo;
        private System.Windows.Forms.TextBox txtSendCmd;
        private UI.udcRunLog clientLog;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtRecvCmd;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtRevcVal;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtCmdData;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label labWR;
    }
}