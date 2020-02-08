namespace GJ.TOOL
{
    partial class FrmHWTelnet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHWTelnet));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSerIP = new System.Windows.Forms.TextBox();
            this.txtSerPort = new System.Windows.Forms.TextBox();
            this.labStatus = new System.Windows.Forms.Label();
            this.btnCon = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnSendCmd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCmd = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPower = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.labPower = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCH = new System.Windows.Forms.TextBox();
            this.btnCheck = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.runLog = new System.Windows.Forms.RichTextBox();
            this.chkRtn = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 187F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1176, 852);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel7.ColumnCount = 3;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel7.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.label10, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.label11, 0, 3);
            this.tableLayoutPanel7.Controls.Add(this.txtSerIP, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.txtSerPort, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.labStatus, 1, 3);
            this.tableLayoutPanel7.Controls.Add(this.btnCon, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.btnSendCmd, 2, 3);
            this.tableLayoutPanel7.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel7.Controls.Add(this.txtCmd, 1, 2);
            this.tableLayoutPanel7.Controls.Add(this.btnClear, 2, 2);
            this.tableLayoutPanel7.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel7.Controls.Add(this.btnPower, 2, 4);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel3, 1, 4);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 5;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(1170, 181);
            this.tableLayoutPanel7.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(4, 1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(111, 35);
            this.label9.TabIndex = 0;
            this.label9.Text = "服务端IP:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(4, 37);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(111, 35);
            this.label10.TabIndex = 1;
            this.label10.Text = "服务端口:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(4, 109);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 35);
            this.label11.TabIndex = 2;
            this.label11.Text = "状态指示:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSerIP
            // 
            this.txtSerIP.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtSerIP.Location = new System.Drawing.Point(122, 4);
            this.txtSerIP.Name = "txtSerIP";
            this.txtSerIP.Size = new System.Drawing.Size(187, 23);
            this.txtSerIP.TabIndex = 3;
            this.txtSerIP.Text = "192.168.0.233";
            this.txtSerIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSerPort
            // 
            this.txtSerPort.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtSerPort.Location = new System.Drawing.Point(122, 40);
            this.txtSerPort.Name = "txtSerPort";
            this.txtSerPort.Size = new System.Drawing.Size(187, 23);
            this.txtSerPort.TabIndex = 4;
            this.txtSerPort.Text = "10001";
            this.txtSerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labStatus
            // 
            this.labStatus.AutoSize = true;
            this.labStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labStatus.Location = new System.Drawing.Point(122, 109);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(943, 35);
            this.labStatus.TabIndex = 5;
            this.labStatus.Text = "--";
            this.labStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCon
            // 
            this.btnCon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCon.ImageKey = "DisConnect";
            this.btnCon.ImageList = this.imageList1;
            this.btnCon.Location = new System.Drawing.Point(1069, 1);
            this.btnCon.Margin = new System.Windows.Forms.Padding(0);
            this.btnCon.Name = "btnCon";
            this.btnCon.Size = new System.Drawing.Size(100, 35);
            this.btnCon.TabIndex = 6;
            this.btnCon.Text = "连接";
            this.btnCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCon.UseVisualStyleBackColor = true;
            this.btnCon.Click += new System.EventHandler(this.btnCon_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Connect");
            this.imageList1.Images.SetKeyName(1, "DisConnect");
            this.imageList1.Images.SetKeyName(2, "Open");
            this.imageList1.Images.SetKeyName(3, "Edit.ICO");
            this.imageList1.Images.SetKeyName(4, "clear.ico");
            // 
            // btnSendCmd
            // 
            this.btnSendCmd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSendCmd.Enabled = false;
            this.btnSendCmd.ImageKey = "Edit.ICO";
            this.btnSendCmd.ImageList = this.imageList1;
            this.btnSendCmd.Location = new System.Drawing.Point(1069, 109);
            this.btnSendCmd.Margin = new System.Windows.Forms.Padding(0);
            this.btnSendCmd.Name = "btnSendCmd";
            this.btnSendCmd.Size = new System.Drawing.Size(100, 35);
            this.btnSendCmd.TabIndex = 7;
            this.btnSendCmd.Text = "发送";
            this.btnSendCmd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSendCmd.UseVisualStyleBackColor = true;
            this.btnSendCmd.Click += new System.EventHandler(this.btnSendCmd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 35);
            this.label1.TabIndex = 8;
            this.label1.Text = "发送数据:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCmd
            // 
            this.txtCmd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCmd.Location = new System.Drawing.Point(122, 76);
            this.txtCmd.Name = "txtCmd";
            this.txtCmd.Size = new System.Drawing.Size(943, 23);
            this.txtCmd.TabIndex = 9;
            this.txtCmd.Text = "dis poe power interface MultiGE 0/0/1";
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.ImageKey = "clear.ico";
            this.btnClear.ImageList = this.imageList1;
            this.btnClear.Location = new System.Drawing.Point(1069, 73);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 35);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "清除";
            this.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(4, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 35);
            this.label2.TabIndex = 11;
            this.label2.Text = "PD Power(mW):";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPower
            // 
            this.btnPower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPower.Enabled = false;
            this.btnPower.ImageKey = "Edit.ICO";
            this.btnPower.ImageList = this.imageList1;
            this.btnPower.Location = new System.Drawing.Point(1069, 145);
            this.btnPower.Margin = new System.Windows.Forms.Padding(0);
            this.btnPower.Name = "btnPower";
            this.btnPower.Size = new System.Drawing.Size(100, 35);
            this.btnPower.TabIndex = 13;
            this.btnPower.Text = "读功率";
            this.btnPower.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPower.UseVisualStyleBackColor = true;
            this.btnPower.Click += new System.EventHandler(this.btnPower_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 6;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 188F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 118F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.tableLayoutPanel3.Controls.Add(this.labPower, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtCH, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCheck, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.chkRtn, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(119, 145);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(949, 35);
            this.tableLayoutPanel3.TabIndex = 14;
            // 
            // labPower
            // 
            this.labPower.BackColor = System.Drawing.Color.White;
            this.labPower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labPower.Location = new System.Drawing.Point(0, 0);
            this.labPower.Margin = new System.Windows.Forms.Padding(0);
            this.labPower.Name = "labPower";
            this.labPower.Size = new System.Drawing.Size(188, 35);
            this.labPower.TabIndex = 13;
            this.labPower.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(191, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 35);
            this.label3.TabIndex = 14;
            this.label3.Text = "通道(1-44):";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCH
            // 
            this.txtCH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCH.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCH.Location = new System.Drawing.Point(300, 3);
            this.txtCH.Name = "txtCH";
            this.txtCH.Size = new System.Drawing.Size(112, 29);
            this.txtCH.TabIndex = 15;
            this.txtCH.Text = "1";
            this.txtCH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnCheck
            // 
            this.btnCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCheck.Enabled = false;
            this.btnCheck.Location = new System.Drawing.Point(856, 0);
            this.btnCheck.Margin = new System.Windows.Forms.Padding(0);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(93, 35);
            this.btnCheck.TabIndex = 16;
            this.btnCheck.Text = "登录检测";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.runLog, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 190);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1170, 659);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // runLog
            // 
            this.runLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runLog.Location = new System.Drawing.Point(3, 3);
            this.runLog.Name = "runLog";
            this.runLog.Size = new System.Drawing.Size(1164, 653);
            this.runLog.TabIndex = 0;
            this.runLog.Text = "";
            // 
            // chkRtn
            // 
            this.chkRtn.AutoSize = true;
            this.chkRtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkRtn.Location = new System.Drawing.Point(418, 3);
            this.chkRtn.Name = "chkRtn";
            this.chkRtn.Size = new System.Drawing.Size(114, 29);
            this.chkRtn.TabIndex = 17;
            this.chkRtn.Text = "回车换行";
            this.chkRtn.UseVisualStyleBackColor = true;
            // 
            // FrmHWTelnet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 852);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FrmHWTelnet";
            this.Text = "FrmTelnet";
            this.Load += new System.EventHandler(this.FrmHWTelnet_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtSerIP;
        private System.Windows.Forms.TextBox txtSerPort;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.Button btnCon;
        private System.Windows.Forms.Button btnSendCmd;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCmd;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RichTextBox runLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPower;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label labPower;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCH;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.CheckBox chkRtn;
    }
}