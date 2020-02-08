namespace GJ.TOOL
{
    partial class FrmBarCode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBarCode));
            this.panel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbCOM = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOpen = new System.Windows.Forms.Button();
            this.labStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbBarType = new System.Windows.Forms.ComboBox();
            this.labSetting = new System.Windows.Forms.Label();
            this.txtBand = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnTriger = new System.Windows.Forms.Button();
            this.btnClr = new System.Windows.Forms.Button();
            this.labLen = new System.Windows.Forms.Label();
            this.labTT = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDelayTimes = new System.Windows.Forms.TextBox();
            this.labTimes = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSerIP = new System.Windows.Forms.TextBox();
            this.labSn = new System.Windows.Forms.Label();
            this.runLog = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.panel2, 0, 0);
            this.panel1.Controls.Add(this.labSn, 0, 1);
            this.panel1.Controls.Add(this.runLog, 0, 2);
            this.panel1.Name = "panel1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.label2, 0, 1);
            this.panel2.Controls.Add(this.cmbCOM, 1, 1);
            this.panel2.Controls.Add(this.label3, 3, 1);
            this.panel2.Controls.Add(this.btnOpen, 2, 1);
            this.panel2.Controls.Add(this.labStatus, 4, 1);
            this.panel2.Controls.Add(this.label4, 0, 0);
            this.panel2.Controls.Add(this.cmbBarType, 1, 0);
            this.panel2.Controls.Add(this.labSetting, 0, 3);
            this.panel2.Controls.Add(this.txtBand, 1, 3);
            this.panel2.Controls.Add(this.btnRead, 2, 3);
            this.panel2.Controls.Add(this.btnTriger, 2, 4);
            this.panel2.Controls.Add(this.btnClr, 3, 4);
            this.panel2.Controls.Add(this.labLen, 3, 3);
            this.panel2.Controls.Add(this.labTT, 4, 4);
            this.panel2.Controls.Add(this.label1, 0, 4);
            this.panel2.Controls.Add(this.txtDelayTimes, 1, 4);
            this.panel2.Controls.Add(this.labTimes, 4, 3);
            this.panel2.Controls.Add(this.label6, 0, 2);
            this.panel2.Controls.Add(this.txtSerIP, 1, 2);
            this.panel2.Name = "panel2";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbCOM
            // 
            resources.ApplyResources(this.cmbCOM, "cmbCOM");
            this.cmbCOM.FormattingEnabled = true;
            this.cmbCOM.Name = "cmbCOM";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnOpen
            // 
            resources.ApplyResources(this.btnOpen, "btnOpen");
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // labStatus
            // 
            resources.ApplyResources(this.labStatus, "labStatus");
            this.labStatus.Name = "labStatus";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbBarType
            // 
            resources.ApplyResources(this.cmbBarType, "cmbBarType");
            this.cmbBarType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBarType.FormattingEnabled = true;
            this.cmbBarType.Items.AddRange(new object[] {
            resources.GetString("cmbBarType.Items"),
            resources.GetString("cmbBarType.Items1"),
            resources.GetString("cmbBarType.Items2"),
            resources.GetString("cmbBarType.Items3"),
            resources.GetString("cmbBarType.Items4"),
            resources.GetString("cmbBarType.Items5"),
            resources.GetString("cmbBarType.Items6"),
            resources.GetString("cmbBarType.Items7"),
            resources.GetString("cmbBarType.Items8"),
            resources.GetString("cmbBarType.Items9")});
            this.cmbBarType.Name = "cmbBarType";
            this.cmbBarType.SelectedIndexChanged += new System.EventHandler(this.cmbBarType_SelectedIndexChanged);
            // 
            // labSetting
            // 
            resources.ApplyResources(this.labSetting, "labSetting");
            this.labSetting.Name = "labSetting";
            // 
            // txtBand
            // 
            resources.ApplyResources(this.txtBand, "txtBand");
            this.txtBand.Name = "txtBand";
            // 
            // btnRead
            // 
            resources.ApplyResources(this.btnRead, "btnRead");
            this.btnRead.Name = "btnRead";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnTriger
            // 
            resources.ApplyResources(this.btnTriger, "btnTriger");
            this.btnTriger.Name = "btnTriger";
            this.btnTriger.UseVisualStyleBackColor = true;
            this.btnTriger.Click += new System.EventHandler(this.btnTriger_Click);
            // 
            // btnClr
            // 
            resources.ApplyResources(this.btnClr, "btnClr");
            this.btnClr.Name = "btnClr";
            this.btnClr.UseVisualStyleBackColor = true;
            this.btnClr.Click += new System.EventHandler(this.btn_Click);
            // 
            // labLen
            // 
            resources.ApplyResources(this.labLen, "labLen");
            this.labLen.Name = "labLen";
            // 
            // labTT
            // 
            resources.ApplyResources(this.labTT, "labTT");
            this.labTT.Name = "labTT";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtDelayTimes
            // 
            resources.ApplyResources(this.txtDelayTimes, "txtDelayTimes");
            this.txtDelayTimes.Name = "txtDelayTimes";
            // 
            // labTimes
            // 
            resources.ApplyResources(this.labTimes, "labTimes");
            this.labTimes.Name = "labTimes";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // txtSerIP
            // 
            resources.ApplyResources(this.txtSerIP, "txtSerIP");
            this.txtSerIP.Name = "txtSerIP";
            // 
            // labSn
            // 
            resources.ApplyResources(this.labSn, "labSn");
            this.labSn.Name = "labSn";
            // 
            // runLog
            // 
            resources.ApplyResources(this.runLog, "runLog");
            this.runLog.Name = "runLog";
            // 
            // FrmBarCode
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "FrmBarCode";
            this.Load += new System.EventHandler(this.FrmCR1000_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel panel1;
        private System.Windows.Forms.TableLayoutPanel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbCOM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.Label labSn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbBarType;
        private System.Windows.Forms.Label labSetting;
        private System.Windows.Forms.TextBox txtBand;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnTriger;
        private System.Windows.Forms.Button btnClr;
        private System.Windows.Forms.Label labLen;
        private System.Windows.Forms.RichTextBox runLog;
        private System.Windows.Forms.Label labTT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDelayTimes;
        private System.Windows.Forms.Label labTimes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSerIP;
    }
}