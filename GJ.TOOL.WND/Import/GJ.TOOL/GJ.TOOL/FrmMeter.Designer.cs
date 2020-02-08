namespace GJ.TOOL
{
    partial class FrmMeter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMeter));
            this.panel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbCOM = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBand = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpen = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMutiAddr = new System.Windows.Forms.TextBox();
            this.labStatus = new System.Windows.Forms.Label();
            this.btnRead = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labVolt = new System.Windows.Forms.Label();
            this.labCurrent = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.panel2, 0, 0);
            this.panel1.Controls.Add(this.panel3, 0, 1);
            this.panel1.Name = "panel1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.label2, 0, 1);
            this.panel2.Controls.Add(this.cmbCOM, 1, 1);
            this.panel2.Controls.Add(this.label4, 0, 0);
            this.panel2.Controls.Add(this.cmbType, 1, 0);
            this.panel2.Controls.Add(this.label5, 0, 2);
            this.panel2.Controls.Add(this.txtBand, 1, 2);
            this.panel2.Controls.Add(this.label1, 0, 3);
            this.panel2.Controls.Add(this.btnOpen, 2, 0);
            this.panel2.Controls.Add(this.label3, 3, 0);
            this.panel2.Controls.Add(this.txtMutiAddr, 1, 3);
            this.panel2.Controls.Add(this.labStatus, 4, 0);
            this.panel2.Controls.Add(this.btnRead, 2, 3);
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
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbType
            // 
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            resources.GetString("cmbType.Items")});
            this.cmbType.Name = "cmbType";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txtBand
            // 
            resources.ApplyResources(this.txtBand, "txtBand");
            this.txtBand.Name = "txtBand";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // txtMutiAddr
            // 
            resources.ApplyResources(this.txtMutiAddr, "txtMutiAddr");
            this.txtMutiAddr.Name = "txtMutiAddr";
            // 
            // labStatus
            // 
            resources.ApplyResources(this.labStatus, "labStatus");
            this.labStatus.Name = "labStatus";
            // 
            // btnRead
            // 
            resources.ApplyResources(this.btnRead, "btnRead");
            this.btnRead.Name = "btnRead";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.label6, 0, 0);
            this.panel3.Controls.Add(this.label7, 1, 0);
            this.panel3.Controls.Add(this.labVolt, 0, 1);
            this.panel3.Controls.Add(this.labCurrent, 1, 1);
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
            // labVolt
            // 
            resources.ApplyResources(this.labVolt, "labVolt");
            this.labVolt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.labVolt.Name = "labVolt";
            // 
            // labCurrent
            // 
            resources.ApplyResources(this.labCurrent, "labCurrent");
            this.labCurrent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.labCurrent.Name = "labCurrent";
            // 
            // FrmMeter
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "FrmMeter";
            this.Load += new System.EventHandler(this.FrmMeter_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel panel1;
        private System.Windows.Forms.TableLayoutPanel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbCOM;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBand;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMutiAddr;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.TableLayoutPanel panel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labVolt;
        private System.Windows.Forms.Label labCurrent;
    }
}