namespace GJ.TOOL
{
    partial class FrmSocket
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSocket));
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSerIP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labSerStatus = new System.Windows.Forms.Label();
            this.btnListen = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnSend1 = new System.Windows.Forms.Button();
            this.txtSerPort = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbClientList = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSerMessage = new System.Windows.Forms.TextBox();
            this.chkRtn1 = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.SerLog = new GJ.UI.udcRunLog();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtClientIP = new System.Windows.Forms.TextBox();
            this.txtClientPort = new System.Windows.Forms.TextBox();
            this.labClientStatus = new System.Windows.Forms.Label();
            this.btnCon = new System.Windows.Forms.Button();
            this.btnSend2 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtClientMessage = new System.Windows.Forms.TextBox();
            this.chkRtn = new System.Windows.Forms.CheckBox();
            this.clientLog = new GJ.UI.udcRunLog();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(4, 1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 32);
            this.label9.TabIndex = 0;
            this.label9.Text = "服务端IP:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1196, 867);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(5, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(575, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "Socket服务端";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("宋体", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(588, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(603, 40);
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
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 47);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(575, 815);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtSerIP, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.labSerStatus, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.btnListen, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSend1, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtSerPort, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.cmbClientList, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.txtSerMessage, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.chkRtn1, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.btnClear, 2, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(569, 194);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(4, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 32);
            this.label3.TabIndex = 0;
            this.label3.Text = "主机IP地址:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(4, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 32);
            this.label4.TabIndex = 1;
            this.label4.Text = "主机端口:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSerIP
            // 
            this.txtSerIP.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtSerIP.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSerIP.Location = new System.Drawing.Point(119, 4);
            this.txtSerIP.Name = "txtSerIP";
            this.txtSerIP.Size = new System.Drawing.Size(160, 26);
            this.txtSerIP.TabIndex = 2;
            this.txtSerIP.Text = "127.0.0.1";
            this.txtSerIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(4, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 60);
            this.label5.TabIndex = 3;
            this.label5.Text = "状态提示:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labSerStatus
            // 
            this.labSerStatus.AutoSize = true;
            this.labSerStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labSerStatus.Location = new System.Drawing.Point(119, 133);
            this.labSerStatus.Name = "labSerStatus";
            this.labSerStatus.Size = new System.Drawing.Size(336, 60);
            this.labSerStatus.TabIndex = 5;
            this.labSerStatus.Text = "--";
            this.labSerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnListen
            // 
            this.btnListen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnListen.ImageKey = "DisConnect";
            this.btnListen.ImageList = this.imageList1;
            this.btnListen.Location = new System.Drawing.Point(459, 1);
            this.btnListen.Margin = new System.Windows.Forms.Padding(0);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(109, 32);
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
            // btnSend1
            // 
            this.btnSend1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSend1.ImageKey = "Edit.ICO";
            this.btnSend1.ImageList = this.imageList1;
            this.btnSend1.Location = new System.Drawing.Point(459, 34);
            this.btnSend1.Margin = new System.Windows.Forms.Padding(0);
            this.btnSend1.Name = "btnSend1";
            this.btnSend1.Size = new System.Drawing.Size(109, 32);
            this.btnSend1.TabIndex = 7;
            this.btnSend1.Text = "发送";
            this.btnSend1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSend1.UseVisualStyleBackColor = true;
            this.btnSend1.Click += new System.EventHandler(this.btnSend1_Click);
            // 
            // txtSerPort
            // 
            this.txtSerPort.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtSerPort.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSerPort.Location = new System.Drawing.Point(119, 37);
            this.txtSerPort.Name = "txtSerPort";
            this.txtSerPort.Size = new System.Drawing.Size(160, 26);
            this.txtSerPort.TabIndex = 8;
            this.txtSerPort.Text = "8000";
            this.txtSerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(4, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 32);
            this.label6.TabIndex = 9;
            this.label6.Text = "客户端列表:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbClientList
            // 
            this.cmbClientList.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbClientList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClientList.FormattingEnabled = true;
            this.cmbClientList.Location = new System.Drawing.Point(119, 70);
            this.cmbClientList.Name = "cmbClientList";
            this.cmbClientList.Size = new System.Drawing.Size(211, 22);
            this.cmbClientList.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(4, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 32);
            this.label7.TabIndex = 11;
            this.label7.Text = "发送消息:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSerMessage
            // 
            this.txtSerMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSerMessage.Location = new System.Drawing.Point(119, 103);
            this.txtSerMessage.Name = "txtSerMessage";
            this.txtSerMessage.Size = new System.Drawing.Size(336, 23);
            this.txtSerMessage.TabIndex = 12;
            this.txtSerMessage.Text = "Hello World";
            // 
            // chkRtn1
            // 
            this.chkRtn1.AutoSize = true;
            this.chkRtn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkRtn1.Location = new System.Drawing.Point(462, 70);
            this.chkRtn1.Name = "chkRtn1";
            this.chkRtn1.Size = new System.Drawing.Size(103, 26);
            this.chkRtn1.TabIndex = 13;
            this.chkRtn1.Text = "回车换行";
            this.chkRtn1.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.Location = new System.Drawing.Point(459, 100);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(109, 32);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "断开客户端";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // SerLog
            // 
            this.SerLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SerLog.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SerLog.Location = new System.Drawing.Point(3, 203);
            this.SerLog.mFont = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SerLog.mMaxLine = 1000;
            this.SerLog.mMaxMB = 1D;
            this.SerLog.mSaveEnable = true;
            this.SerLog.mSaveFolder = "";
            this.SerLog.mSaveName = "SerLog";
            this.SerLog.mTitle = "运行日志";
            this.SerLog.mTitleEnable = true;
            this.SerLog.Name = "SerLog";
            this.SerLog.Size = new System.Drawing.Size(569, 609);
            this.SerLog.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.clientLog, 0, 1);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(588, 47);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 203F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(603, 815);
            this.tableLayoutPanel6.TabIndex = 3;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel7.ColumnCount = 3;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.tableLayoutPanel7.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.label10, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.label11, 0, 3);
            this.tableLayoutPanel7.Controls.Add(this.txtClientIP, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.txtClientPort, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.labClientStatus, 1, 3);
            this.tableLayoutPanel7.Controls.Add(this.btnCon, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.btnSend2, 2, 1);
            this.tableLayoutPanel7.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel7.Controls.Add(this.txtClientMessage, 1, 2);
            this.tableLayoutPanel7.Controls.Add(this.chkRtn, 2, 2);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 4;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(597, 197);
            this.tableLayoutPanel7.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(4, 34);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 32);
            this.label10.TabIndex = 1;
            this.label10.Text = "服务端口:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(4, 100);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 96);
            this.label11.TabIndex = 2;
            this.label11.Text = "状态指示:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtClientIP
            // 
            this.txtClientIP.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtClientIP.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtClientIP.Location = new System.Drawing.Point(103, 4);
            this.txtClientIP.Name = "txtClientIP";
            this.txtClientIP.Size = new System.Drawing.Size(187, 26);
            this.txtClientIP.TabIndex = 3;
            this.txtClientIP.Text = "127.0.0.1";
            this.txtClientIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtClientPort
            // 
            this.txtClientPort.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtClientPort.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtClientPort.Location = new System.Drawing.Point(103, 37);
            this.txtClientPort.Name = "txtClientPort";
            this.txtClientPort.Size = new System.Drawing.Size(116, 26);
            this.txtClientPort.TabIndex = 4;
            this.txtClientPort.Text = "8000";
            this.txtClientPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labClientStatus
            // 
            this.labClientStatus.AutoSize = true;
            this.labClientStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labClientStatus.Location = new System.Drawing.Point(103, 100);
            this.labClientStatus.Name = "labClientStatus";
            this.labClientStatus.Size = new System.Drawing.Size(380, 96);
            this.labClientStatus.TabIndex = 5;
            this.labClientStatus.Text = "--";
            this.labClientStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCon
            // 
            this.btnCon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCon.ImageKey = "DisConnect";
            this.btnCon.ImageList = this.imageList1;
            this.btnCon.Location = new System.Drawing.Point(487, 1);
            this.btnCon.Margin = new System.Windows.Forms.Padding(0);
            this.btnCon.Name = "btnCon";
            this.btnCon.Size = new System.Drawing.Size(109, 32);
            this.btnCon.TabIndex = 6;
            this.btnCon.Text = "连接";
            this.btnCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCon.UseVisualStyleBackColor = true;
            this.btnCon.Click += new System.EventHandler(this.btnCon_Click);
            // 
            // btnSend2
            // 
            this.btnSend2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSend2.ImageKey = "Edit.ICO";
            this.btnSend2.ImageList = this.imageList1;
            this.btnSend2.Location = new System.Drawing.Point(487, 34);
            this.btnSend2.Margin = new System.Windows.Forms.Padding(0);
            this.btnSend2.Name = "btnSend2";
            this.btnSend2.Size = new System.Drawing.Size(109, 32);
            this.btnSend2.TabIndex = 7;
            this.btnSend2.Text = "发送";
            this.btnSend2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSend2.UseVisualStyleBackColor = true;
            this.btnSend2.Click += new System.EventHandler(this.btnSend2_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(4, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 32);
            this.label8.TabIndex = 8;
            this.label8.Text = "发送消息:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtClientMessage
            // 
            this.txtClientMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtClientMessage.Location = new System.Drawing.Point(103, 70);
            this.txtClientMessage.Name = "txtClientMessage";
            this.txtClientMessage.Size = new System.Drawing.Size(380, 23);
            this.txtClientMessage.TabIndex = 9;
            this.txtClientMessage.Text = "Hello World";
            // 
            // chkRtn
            // 
            this.chkRtn.AutoSize = true;
            this.chkRtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkRtn.Location = new System.Drawing.Point(490, 70);
            this.chkRtn.Name = "chkRtn";
            this.chkRtn.Size = new System.Drawing.Size(103, 26);
            this.chkRtn.TabIndex = 10;
            this.chkRtn.Text = "回车换行";
            this.chkRtn.UseVisualStyleBackColor = true;
            // 
            // clientLog
            // 
            this.clientLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientLog.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clientLog.Location = new System.Drawing.Point(3, 206);
            this.clientLog.mFont = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clientLog.mMaxLine = 1000;
            this.clientLog.mMaxMB = 1D;
            this.clientLog.mSaveEnable = true;
            this.clientLog.mSaveFolder = "";
            this.clientLog.mSaveName = "ClientLog";
            this.clientLog.mTitle = "运行日志";
            this.clientLog.mTitleEnable = true;
            this.clientLog.Name = "clientLog";
            this.clientLog.Size = new System.Drawing.Size(597, 606);
            this.clientLog.TabIndex = 2;
            // 
            // FrmSocket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1196, 867);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmSocket";
            this.Text = "TCP调试工具";
            this.Load += new System.EventHandler(this.FrmSocket_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSerIP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labSerStatus;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnSend1;
        private System.Windows.Forms.TextBox txtSerPort;
        private UI.udcRunLog SerLog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtClientIP;
        private System.Windows.Forms.TextBox txtClientPort;
        private System.Windows.Forms.Label labClientStatus;
        private System.Windows.Forms.Button btnCon;
        private System.Windows.Forms.Button btnSend2;
        private UI.udcRunLog clientLog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbClientList;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSerMessage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtClientMessage;
        private System.Windows.Forms.CheckBox chkRtn;
        private System.Windows.Forms.CheckBox chkRtn1;
        private System.Windows.Forms.Button btnClear;
    }
}