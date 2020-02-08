namespace GJ.TOOL
{
    partial class FrmPhoenix
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
            this.label2 = new System.Windows.Forms.Label();
            this.cmbCOM = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOpen = new System.Windows.Forms.Button();
            this.runLog = new System.Windows.Forms.RichTextBox();
            this.labStatus = new System.Windows.Forms.Label();
            this.labSetting = new System.Windows.Forms.Label();
            this.txtBand = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.labTimes = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtUpdate = new System.Windows.Forms.TextBox();
            this.txtCmd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.TableLayoutPanel();
            this.labModifyHardware = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFirmWave = new System.Windows.Forms.TextBox();
            this.txtHardWave = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtWaitTimes = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtUpdatVersion = new System.Windows.Forms.TextBox();
            this.labModifyFirmware = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(4, 4);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "串口编号:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbCOM
            // 
            this.cmbCOM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCOM.FormattingEnabled = true;
            this.cmbCOM.Location = new System.Drawing.Point(98, 4);
            this.cmbCOM.Name = "cmbCOM";
            this.cmbCOM.Size = new System.Drawing.Size(108, 20);
            this.cmbCOM.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(281, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 30);
            this.label3.TabIndex = 4;
            this.label3.Text = "状态指示:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOpen
            // 
            this.btnOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpen.Location = new System.Drawing.Point(210, 1);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(0);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(67, 30);
            this.btnOpen.TabIndex = 5;
            this.btnOpen.Text = "打开";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // runLog
            // 
            this.runLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runLog.Location = new System.Drawing.Point(5, 277);
            this.runLog.Name = "runLog";
            this.runLog.Size = new System.Drawing.Size(652, 217);
            this.runLog.TabIndex = 2;
            this.runLog.Text = "";
            // 
            // labStatus
            // 
            this.labStatus.AutoSize = true;
            this.labStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labStatus.Location = new System.Drawing.Point(365, 4);
            this.labStatus.Margin = new System.Windows.Forms.Padding(3);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(283, 24);
            this.labStatus.TabIndex = 6;
            this.labStatus.Text = "---";
            this.labStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labSetting
            // 
            this.labSetting.AutoSize = true;
            this.labSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labSetting.Location = new System.Drawing.Point(4, 32);
            this.labSetting.Name = "labSetting";
            this.labSetting.Size = new System.Drawing.Size(87, 30);
            this.labSetting.TabIndex = 10;
            this.labSetting.Text = "串口波特率:";
            this.labSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBand
            // 
            this.txtBand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBand.Location = new System.Drawing.Point(98, 35);
            this.txtBand.Name = "txtBand";
            this.txtBand.Size = new System.Drawing.Size(108, 21);
            this.txtBand.TabIndex = 11;
            this.txtBand.Text = "1250000,N,8,1";
            this.txtBand.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel1
            // 
            this.panel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.panel1.ColumnCount = 1;
            this.panel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel1.Controls.Add(this.panel2, 0, 0);
            this.panel1.Controls.Add(this.runLog, 0, 3);
            this.panel1.Controls.Add(this.panel3, 0, 1);
            this.panel1.Controls.Add(this.panel4, 0, 2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.RowCount = 4;
            this.panel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.panel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.panel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.panel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel1.Size = new System.Drawing.Size(662, 499);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.panel2.ColumnCount = 5;
            this.panel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.panel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            this.panel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.panel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.panel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel2.Controls.Add(this.label2, 0, 0);
            this.panel2.Controls.Add(this.cmbCOM, 1, 0);
            this.panel2.Controls.Add(this.label3, 3, 0);
            this.panel2.Controls.Add(this.btnOpen, 2, 0);
            this.panel2.Controls.Add(this.labStatus, 4, 0);
            this.panel2.Controls.Add(this.labSetting, 0, 1);
            this.panel2.Controls.Add(this.txtBand, 1, 1);
            this.panel2.Controls.Add(this.label9, 3, 1);
            this.panel2.Controls.Add(this.labTimes, 4, 1);
            this.panel2.Controls.Add(this.btnClear, 2, 1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(5, 5);
            this.panel2.Name = "panel2";
            this.panel2.RowCount = 2;
            this.panel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panel2.Size = new System.Drawing.Size(652, 63);
            this.panel2.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(281, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 30);
            this.label9.TabIndex = 12;
            this.label9.Text = "监控时间(S):";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labTimes
            // 
            this.labTimes.AutoSize = true;
            this.labTimes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labTimes.Location = new System.Drawing.Point(365, 32);
            this.labTimes.Name = "labTimes";
            this.labTimes.Size = new System.Drawing.Size(283, 30);
            this.labTimes.TabIndex = 13;
            this.labTimes.Text = "---";
            this.labTimes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.Location = new System.Drawing.Point(210, 32);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(67, 30);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // panel3
            // 
            this.panel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.panel3.ColumnCount = 3;
            this.panel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.panel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.panel3.Controls.Add(this.btnUpdate, 2, 2);
            this.panel3.Controls.Add(this.btnSend, 2, 0);
            this.panel3.Controls.Add(this.txtUpdate, 1, 2);
            this.panel3.Controls.Add(this.txtCmd, 1, 0);
            this.panel3.Controls.Add(this.label1, 0, 1);
            this.panel3.Controls.Add(this.txtVersion, 1, 1);
            this.panel3.Controls.Add(this.btnRead, 2, 1);
            this.panel3.Controls.Add(this.label4, 0, 2);
            this.panel3.Controls.Add(this.label5, 0, 0);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(5, 76);
            this.panel3.Name = "panel3";
            this.panel3.RowCount = 3;
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panel3.Size = new System.Drawing.Size(652, 89);
            this.panel3.TabIndex = 3;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUpdate.Location = new System.Drawing.Point(559, 59);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(92, 29);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "升级版本";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnSend
            // 
            this.btnSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSend.Location = new System.Drawing.Point(559, 1);
            this.btnSend.Margin = new System.Windows.Forms.Padding(0);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(92, 28);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtUpdate
            // 
            this.txtUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUpdate.Font = new System.Drawing.Font("Arial Narrow", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdate.Location = new System.Drawing.Point(97, 62);
            this.txtUpdate.Name = "txtUpdate";
            this.txtUpdate.Size = new System.Drawing.Size(458, 24);
            this.txtUpdate.TabIndex = 7;
            this.txtUpdate.Text = "b192 update";
            // 
            // txtCmd
            // 
            this.txtCmd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCmd.Font = new System.Drawing.Font("Arial Narrow", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCmd.Location = new System.Drawing.Point(97, 4);
            this.txtCmd.Name = "txtCmd";
            this.txtCmd.Size = new System.Drawing.Size(458, 24);
            this.txtCmd.TabIndex = 5;
            this.txtCmd.Text = "b192 enumerate";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "固件版本命令:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtVersion
            // 
            this.txtVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVersion.Font = new System.Drawing.Font("Arial Narrow", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVersion.Location = new System.Drawing.Point(97, 33);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(458, 24);
            this.txtVersion.TabIndex = 1;
            this.txtVersion.Text = "b192 enumerate";
            // 
            // btnRead
            // 
            this.btnRead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRead.Location = new System.Drawing.Point(559, 30);
            this.btnRead.Margin = new System.Windows.Forms.Padding(0);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(92, 28);
            this.btnRead.TabIndex = 2;
            this.btnRead.Text = "读取版本";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(4, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 29);
            this.label4.TabIndex = 3;
            this.label4.Text = "固件升级命令:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(4, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 28);
            this.label5.TabIndex = 4;
            this.label5.Text = "发送串口命令:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.panel4.ColumnCount = 5;
            this.panel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.panel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 152F));
            this.panel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.panel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 154F));
            this.panel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel4.Controls.Add(this.labModifyHardware, 3, 2);
            this.panel4.Controls.Add(this.label6, 0, 0);
            this.panel4.Controls.Add(this.label7, 0, 1);
            this.panel4.Controls.Add(this.txtFirmWave, 1, 0);
            this.panel4.Controls.Add(this.txtHardWave, 1, 1);
            this.panel4.Controls.Add(this.label8, 0, 2);
            this.panel4.Controls.Add(this.txtWaitTimes, 1, 2);
            this.panel4.Controls.Add(this.label10, 2, 0);
            this.panel4.Controls.Add(this.label11, 2, 1);
            this.panel4.Controls.Add(this.txtUpdatVersion, 3, 0);
            this.panel4.Controls.Add(this.labModifyFirmware, 3, 1);
            this.panel4.Controls.Add(this.label12, 2, 2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(5, 173);
            this.panel4.Name = "panel4";
            this.panel4.RowCount = 3;
            this.panel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panel4.Size = new System.Drawing.Size(652, 96);
            this.panel4.TabIndex = 4;
            // 
            // labModifyHardware
            // 
            this.labModifyHardware.AutoSize = true;
            this.labModifyHardware.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labModifyHardware.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labModifyHardware.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labModifyHardware.ForeColor = System.Drawing.Color.Cyan;
            this.labModifyHardware.Location = new System.Drawing.Point(383, 63);
            this.labModifyHardware.Margin = new System.Windows.Forms.Padding(0);
            this.labModifyHardware.Name = "labModifyHardware";
            this.labModifyHardware.Size = new System.Drawing.Size(154, 32);
            this.labModifyHardware.TabIndex = 13;
            this.labModifyHardware.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(4, 1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 30);
            this.label6.TabIndex = 0;
            this.label6.Text = "Firmwave Version:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(4, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 30);
            this.label7.TabIndex = 1;
            this.label7.Text = "Hardware Version:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFirmWave
            // 
            this.txtFirmWave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFirmWave.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFirmWave.Location = new System.Drawing.Point(125, 4);
            this.txtFirmWave.Name = "txtFirmWave";
            this.txtFirmWave.Size = new System.Drawing.Size(146, 23);
            this.txtFirmWave.TabIndex = 2;
            this.txtFirmWave.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtHardWave
            // 
            this.txtHardWave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHardWave.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtHardWave.Location = new System.Drawing.Point(125, 35);
            this.txtHardWave.Name = "txtHardWave";
            this.txtHardWave.Size = new System.Drawing.Size(146, 23);
            this.txtHardWave.TabIndex = 3;
            this.txtHardWave.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(4, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 32);
            this.label8.TabIndex = 4;
            this.label8.Text = "Wait Time(S):";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtWaitTimes
            // 
            this.txtWaitTimes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWaitTimes.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtWaitTimes.Location = new System.Drawing.Point(125, 66);
            this.txtWaitTimes.Name = "txtWaitTimes";
            this.txtWaitTimes.Size = new System.Drawing.Size(146, 23);
            this.txtWaitTimes.TabIndex = 5;
            this.txtWaitTimes.Text = "10";
            this.txtWaitTimes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(278, 1);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 30);
            this.label10.TabIndex = 6;
            this.label10.Text = "Update Version:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(278, 32);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 30);
            this.label11.TabIndex = 7;
            this.label11.Text = "Modify Firmwave:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtUpdatVersion
            // 
            this.txtUpdatVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUpdatVersion.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUpdatVersion.Location = new System.Drawing.Point(386, 4);
            this.txtUpdatVersion.Name = "txtUpdatVersion";
            this.txtUpdatVersion.Size = new System.Drawing.Size(148, 26);
            this.txtUpdatVersion.TabIndex = 8;
            this.txtUpdatVersion.Text = "01030063";
            this.txtUpdatVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labModifyFirmware
            // 
            this.labModifyFirmware.AutoSize = true;
            this.labModifyFirmware.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labModifyFirmware.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labModifyFirmware.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labModifyFirmware.ForeColor = System.Drawing.Color.Lime;
            this.labModifyFirmware.Location = new System.Drawing.Point(383, 32);
            this.labModifyFirmware.Margin = new System.Windows.Forms.Padding(0);
            this.labModifyFirmware.Name = "labModifyFirmware";
            this.labModifyFirmware.Size = new System.Drawing.Size(154, 30);
            this.labModifyFirmware.TabIndex = 9;
            this.labModifyFirmware.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(278, 63);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 32);
            this.label12.TabIndex = 10;
            this.label12.Text = "Modify Hardwave:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmPhoenix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 499);
            this.Controls.Add(this.panel1);
            this.Name = "FrmPhoenix";
            this.Text = "FrmPhoenix";
            this.Load += new System.EventHandler(this.FrmPhoenix_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbCOM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.RichTextBox runLog;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.Label labSetting;
        private System.Windows.Forms.TextBox txtBand;
        private System.Windows.Forms.TableLayoutPanel panel1;
        private System.Windows.Forms.TableLayoutPanel panel2;
        private System.Windows.Forms.TableLayoutPanel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labTimes;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtUpdate;
        private System.Windows.Forms.TextBox txtCmd;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel panel4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtFirmWave;
        private System.Windows.Forms.TextBox txtHardWave;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtWaitTimes;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtUpdatVersion;
        private System.Windows.Forms.Label labModifyFirmware;
        private System.Windows.Forms.Label labModifyHardware;
        private System.Windows.Forms.Label label12;
    }
}