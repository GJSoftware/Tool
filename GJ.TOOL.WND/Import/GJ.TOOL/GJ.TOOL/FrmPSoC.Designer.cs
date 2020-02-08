namespace GJ.TOOL
{
    partial class FrmPSoC
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPSoC = new System.Windows.Forms.ComboBox();
            this.btnCon = new System.Windows.Forms.Button();
            this.labStatus = new System.Windows.Forms.Label();
            this.btnFindId = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbProtocol = new System.Windows.Forms.ComboBox();
            this.cmbVoltage = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbClock = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbPin = new System.Windows.Forms.ComboBox();
            this.btnInitial = new System.Windows.Forms.Button();
            this.btnProgram = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbICType = new System.Windows.Forms.ComboBox();
            this.btnAbort = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtHexFile = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer1.Size = new System.Drawing.Size(832, 577);
            this.splitContainer1.SplitterDistance = 247;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.ColumnCount = 1;
            this.panel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel1.Controls.Add(this.panel2, 0, 0);
            this.panel1.Controls.Add(this.panel3, 0, 1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.RowCount = 2;
            this.panel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.panel1.Size = new System.Drawing.Size(832, 247);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.panel2.ColumnCount = 5;
            this.panel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 119F));
            this.panel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 146F));
            this.panel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.panel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.panel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel2.Controls.Add(this.label1, 0, 0);
            this.panel2.Controls.Add(this.cmbPSoC, 1, 0);
            this.panel2.Controls.Add(this.btnCon, 3, 0);
            this.panel2.Controls.Add(this.labStatus, 4, 0);
            this.panel2.Controls.Add(this.btnFindId, 2, 0);
            this.panel2.Controls.Add(this.label2, 0, 2);
            this.panel2.Controls.Add(this.label3, 0, 3);
            this.panel2.Controls.Add(this.cmbProtocol, 1, 2);
            this.panel2.Controls.Add(this.cmbVoltage, 1, 3);
            this.panel2.Controls.Add(this.label4, 0, 4);
            this.panel2.Controls.Add(this.cmbClock, 1, 4);
            this.panel2.Controls.Add(this.label5, 0, 5);
            this.panel2.Controls.Add(this.cmbPin, 1, 5);
            this.panel2.Controls.Add(this.btnInitial, 2, 5);
            this.panel2.Controls.Add(this.btnProgram, 3, 5);
            this.panel2.Controls.Add(this.label7, 0, 1);
            this.panel2.Controls.Add(this.cmbICType, 1, 1);
            this.panel2.Controls.Add(this.btnAbort, 3, 1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.RowCount = 6;
            this.panel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.panel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.panel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.panel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.panel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.panel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel2.Size = new System.Drawing.Size(826, 200);
            this.panel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "烧录器ID号:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPSoC
            // 
            this.cmbPSoC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPSoC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPSoC.FormattingEnabled = true;
            this.cmbPSoC.Location = new System.Drawing.Point(124, 4);
            this.cmbPSoC.Name = "cmbPSoC";
            this.cmbPSoC.Size = new System.Drawing.Size(140, 22);
            this.cmbPSoC.TabIndex = 1;
            // 
            // btnCon
            // 
            this.btnCon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCon.Location = new System.Drawing.Point(349, 1);
            this.btnCon.Margin = new System.Windows.Forms.Padding(0);
            this.btnCon.Name = "btnCon";
            this.btnCon.Size = new System.Drawing.Size(78, 32);
            this.btnCon.TabIndex = 2;
            this.btnCon.Text = "Open";
            this.btnCon.UseVisualStyleBackColor = true;
            this.btnCon.Click += new System.EventHandler(this.btnCon_Click);
            // 
            // labStatus
            // 
            this.labStatus.AutoSize = true;
            this.labStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labStatus.Location = new System.Drawing.Point(431, 1);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(391, 32);
            this.labStatus.TabIndex = 3;
            this.labStatus.Text = "---";
            this.labStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnFindId
            // 
            this.btnFindId.BackColor = System.Drawing.Color.Transparent;
            this.btnFindId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindId.Location = new System.Drawing.Point(268, 1);
            this.btnFindId.Margin = new System.Windows.Forms.Padding(0);
            this.btnFindId.Name = "btnFindId";
            this.btnFindId.Size = new System.Drawing.Size(80, 32);
            this.btnFindId.TabIndex = 4;
            this.btnFindId.Text = "Find ID";
            this.btnFindId.UseVisualStyleBackColor = false;
            this.btnFindId.Click += new System.EventHandler(this.btnFindId_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(4, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 32);
            this.label2.TabIndex = 5;
            this.label2.Text = "通讯协议:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(4, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 32);
            this.label3.TabIndex = 6;
            this.label3.Text = "IC电压(V):";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbProtocol
            // 
            this.cmbProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProtocol.FormattingEnabled = true;
            this.cmbProtocol.Items.AddRange(new object[] {
            "JTAG",
            "ISSP",
            "SWD"});
            this.cmbProtocol.Location = new System.Drawing.Point(124, 70);
            this.cmbProtocol.Name = "cmbProtocol";
            this.cmbProtocol.Size = new System.Drawing.Size(140, 22);
            this.cmbProtocol.TabIndex = 7;
            this.cmbProtocol.SelectedValueChanged += new System.EventHandler(this.cmbProtocol_SelectedValueChanged);
            // 
            // cmbVoltage
            // 
            this.cmbVoltage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbVoltage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVoltage.FormattingEnabled = true;
            this.cmbVoltage.Items.AddRange(new object[] {
            "1.8",
            "2.5",
            "3.3",
            "5.0"});
            this.cmbVoltage.Location = new System.Drawing.Point(124, 103);
            this.cmbVoltage.Name = "cmbVoltage";
            this.cmbVoltage.Size = new System.Drawing.Size(140, 22);
            this.cmbVoltage.TabIndex = 8;
            this.cmbVoltage.SelectedValueChanged += new System.EventHandler(this.cmbVoltage_SelectedValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(4, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 32);
            this.label4.TabIndex = 9;
            this.label4.Text = "时钟速度(MHz):";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbClock
            // 
            this.cmbClock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbClock.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClock.FormattingEnabled = true;
            this.cmbClock.Items.AddRange(new object[] {
            "24",
            "16",
            "12",
            "8",
            "6",
            "3.2",
            "3.0",
            "1.6",
            "1.5"});
            this.cmbClock.Location = new System.Drawing.Point(124, 136);
            this.cmbClock.Name = "cmbClock";
            this.cmbClock.Size = new System.Drawing.Size(140, 22);
            this.cmbClock.TabIndex = 10;
            this.cmbClock.SelectedValueChanged += new System.EventHandler(this.cmbClock_SelectedValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(4, 166);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 33);
            this.label5.TabIndex = 11;
            this.label5.Text = "PIN脚数量:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPin
            // 
            this.cmbPin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPin.FormattingEnabled = true;
            this.cmbPin.Items.AddRange(new object[] {
            "PIN5",
            "PIN10"});
            this.cmbPin.Location = new System.Drawing.Point(124, 169);
            this.cmbPin.Name = "cmbPin";
            this.cmbPin.Size = new System.Drawing.Size(140, 22);
            this.cmbPin.TabIndex = 12;
            this.cmbPin.SelectedValueChanged += new System.EventHandler(this.cmbPin_SelectedValueChanged);
            // 
            // btnInitial
            // 
            this.btnInitial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInitial.Location = new System.Drawing.Point(268, 166);
            this.btnInitial.Margin = new System.Windows.Forms.Padding(0);
            this.btnInitial.Name = "btnInitial";
            this.btnInitial.Size = new System.Drawing.Size(80, 33);
            this.btnInitial.TabIndex = 13;
            this.btnInitial.Text = "Initial";
            this.btnInitial.UseVisualStyleBackColor = true;
            this.btnInitial.Click += new System.EventHandler(this.btnInitial_Click);
            // 
            // btnProgram
            // 
            this.btnProgram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProgram.Location = new System.Drawing.Point(349, 166);
            this.btnProgram.Margin = new System.Windows.Forms.Padding(0);
            this.btnProgram.Name = "btnProgram";
            this.btnProgram.Size = new System.Drawing.Size(78, 33);
            this.btnProgram.TabIndex = 14;
            this.btnProgram.Text = "Program";
            this.btnProgram.UseVisualStyleBackColor = true;
            this.btnProgram.Click += new System.EventHandler(this.btnProgram_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(4, 34);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 32);
            this.label7.TabIndex = 15;
            this.label7.Text = "烧录芯片型号:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbICType
            // 
            this.cmbICType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbICType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbICType.FormattingEnabled = true;
            this.cmbICType.Items.AddRange(new object[] {
            "CYPD3135-32LQXQT"});
            this.cmbICType.Location = new System.Drawing.Point(124, 37);
            this.cmbICType.Name = "cmbICType";
            this.cmbICType.Size = new System.Drawing.Size(140, 22);
            this.cmbICType.TabIndex = 16;
            // 
            // btnAbort
            // 
            this.btnAbort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAbort.Location = new System.Drawing.Point(349, 34);
            this.btnAbort.Margin = new System.Windows.Forms.Padding(0);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(78, 32);
            this.btnAbort.TabIndex = 17;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // panel3
            // 
            this.panel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.panel3.ColumnCount = 3;
            this.panel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.panel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.panel3.Controls.Add(this.label6, 0, 0);
            this.panel3.Controls.Add(this.txtHexFile, 1, 0);
            this.panel3.Controls.Add(this.btnLoad, 2, 0);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 209);
            this.panel3.Name = "panel3";
            this.panel3.RowCount = 1;
            this.panel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel3.Size = new System.Drawing.Size(826, 35);
            this.panel3.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(4, 1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 33);
            this.label6.TabIndex = 0;
            this.label6.Text = "烧录文件(*.Hex):";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtHexFile
            // 
            this.txtHexFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHexFile.Location = new System.Drawing.Point(131, 4);
            this.txtHexFile.Name = "txtHexFile";
            this.txtHexFile.Size = new System.Drawing.Size(596, 23);
            this.txtHexFile.TabIndex = 1;
            // 
            // btnLoad
            // 
            this.btnLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoad.Location = new System.Drawing.Point(731, 1);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(0);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(94, 33);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // FrmPSoC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 577);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FrmPSoC";
            this.Text = "FrmPSoC";
            this.Load += new System.EventHandler(this.FrmPSoC_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel panel1;
        private System.Windows.Forms.TableLayoutPanel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPSoC;
        private System.Windows.Forms.Button btnCon;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.Button btnFindId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbProtocol;
        private System.Windows.Forms.ComboBox cmbVoltage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbClock;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbPin;
        private System.Windows.Forms.TableLayoutPanel panel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtHexFile;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnInitial;
        private System.Windows.Forms.Button btnProgram;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbICType;
        private System.Windows.Forms.Button btnAbort;


    }
}