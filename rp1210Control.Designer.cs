namespace rp1210
{
    partial class rp1210Control
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtTX = new System.Windows.Forms.TextBox();
            this.txtRX = new System.Windows.Forms.TextBox();
            this.chkLogToFile = new System.Windows.Forms.CheckBox();
            this.chkJ1939Enable = new System.Windows.Forms.CheckBox();
            this.chkJ1587Enable = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tmrJ1939 = new System.Windows.Forms.Timer(this.components);
            this.cmdSend = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.cmbDriverList = new System.Windows.Forms.ComboBox();
            this.cmdConnect = new System.Windows.Forms.Button();
            this.cmdStop = new System.Windows.Forms.Button();
            this.cmbDeviceList = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(51, 34);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtTX);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtRX);
            this.splitContainer1.Size = new System.Drawing.Size(701, 381);
            this.splitContainer1.SplitterDistance = 189;
            this.splitContainer1.TabIndex = 27;
            // 
            // txtTX
            // 
            this.txtTX.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTX.Location = new System.Drawing.Point(3, 3);
            this.txtTX.Multiline = true;
            this.txtTX.Name = "txtTX";
            this.txtTX.Size = new System.Drawing.Size(695, 183);
            this.txtTX.TabIndex = 0;
            // 
            // txtRX
            // 
            this.txtRX.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRX.Location = new System.Drawing.Point(3, 4);
            this.txtRX.Multiline = true;
            this.txtRX.Name = "txtRX";
            this.txtRX.Size = new System.Drawing.Size(695, 181);
            this.txtRX.TabIndex = 0;
            // 
            // chkLogToFile
            // 
            this.chkLogToFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLogToFile.AutoSize = true;
            this.chkLogToFile.Location = new System.Drawing.Point(473, 9);
            this.chkLogToFile.Name = "chkLogToFile";
            this.chkLogToFile.Size = new System.Drawing.Size(73, 17);
            this.chkLogToFile.TabIndex = 25;
            this.chkLogToFile.Text = "LogToFile";
            this.chkLogToFile.UseVisualStyleBackColor = true;
            // 
            // chkJ1939Enable
            // 
            this.chkJ1939Enable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkJ1939Enable.AutoSize = true;
            this.chkJ1939Enable.Location = new System.Drawing.Point(552, 9);
            this.chkJ1939Enable.Name = "chkJ1939Enable";
            this.chkJ1939Enable.Size = new System.Drawing.Size(55, 17);
            this.chkJ1939Enable.TabIndex = 24;
            this.chkJ1939Enable.Text = "J1939";
            this.chkJ1939Enable.UseVisualStyleBackColor = true;
            // 
            // chkJ1587Enable
            // 
            this.chkJ1587Enable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkJ1587Enable.AutoSize = true;
            this.chkJ1587Enable.Location = new System.Drawing.Point(613, 9);
            this.chkJ1587Enable.Name = "chkJ1587Enable";
            this.chkJ1587Enable.Size = new System.Drawing.Size(55, 17);
            this.chkJ1587Enable.TabIndex = 23;
            this.chkJ1587Enable.Text = "J1587";
            this.chkJ1587Enable.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Devices";
            // 
            // tmrJ1939
            // 
            this.tmrJ1939.Interval = 50;
            this.tmrJ1939.Tick += new System.EventHandler(this.tmrJ1939_Tick);
            // 
            // cmdSend
            // 
            this.cmdSend.Location = new System.Drawing.Point(309, 5);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.Size = new System.Drawing.Size(75, 23);
            this.cmdSend.TabIndex = 28;
            this.cmdSend.Text = "Send";
            this.cmdSend.UseVisualStyleBackColor = true;
            this.cmdSend.Click += new System.EventHandler(this.cmdSend_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "TX";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 399);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "RX";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 424);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Status";
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatus.Location = new System.Drawing.Point(54, 421);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(695, 20);
            this.txtStatus.TabIndex = 20;
            // 
            // cmbDriverList
            // 
            this.cmbDriverList.FormattingEnabled = true;
            this.cmbDriverList.Location = new System.Drawing.Point(55, 7);
            this.cmbDriverList.Name = "cmbDriverList";
            this.cmbDriverList.Size = new System.Drawing.Size(121, 21);
            this.cmbDriverList.TabIndex = 18;
            this.cmbDriverList.SelectedIndexChanged += new System.EventHandler(this.cmbDriverList_SelectedIndexChanged);
            // 
            // cmdConnect
            // 
            this.cmdConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdConnect.Location = new System.Drawing.Point(674, 5);
            this.cmdConnect.Name = "cmdConnect";
            this.cmdConnect.Size = new System.Drawing.Size(75, 23);
            this.cmdConnect.TabIndex = 17;
            this.cmdConnect.Text = "Connect";
            this.cmdConnect.UseVisualStyleBackColor = true;
            this.cmdConnect.Click += new System.EventHandler(this.cmdConnect_Click);
            // 
            // cmdStop
            // 
            this.cmdStop.Location = new System.Drawing.Point(390, 5);
            this.cmdStop.Name = "cmdStop";
            this.cmdStop.Size = new System.Drawing.Size(75, 23);
            this.cmdStop.TabIndex = 29;
            this.cmdStop.Text = "Stop";
            this.cmdStop.UseVisualStyleBackColor = true;
            // 
            // cmbDeviceList
            // 
            this.cmbDeviceList.FormattingEnabled = true;
            this.cmbDeviceList.Location = new System.Drawing.Point(182, 7);
            this.cmbDeviceList.Name = "cmbDeviceList";
            this.cmbDeviceList.Size = new System.Drawing.Size(121, 21);
            this.cmbDeviceList.TabIndex = 30;
            // 
            // rp1210Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbDeviceList);
            this.Controls.Add(this.cmdStop);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.chkLogToFile);
            this.Controls.Add(this.chkJ1939Enable);
            this.Controls.Add(this.chkJ1587Enable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdSend);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.cmbDriverList);
            this.Controls.Add(this.cmdConnect);
            this.MinimumSize = new System.Drawing.Size(755, 446);
            this.Name = "rp1210Control";
            this.Size = new System.Drawing.Size(755, 446);
            this.Load += new System.EventHandler(this.rp1210Control_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtTX;
        private System.Windows.Forms.TextBox txtRX;
        private System.Windows.Forms.CheckBox chkLogToFile;
        private System.Windows.Forms.CheckBox chkJ1939Enable;
        private System.Windows.Forms.CheckBox chkJ1587Enable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer tmrJ1939;
        private System.Windows.Forms.Button cmdSend;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.ComboBox cmbDriverList;
        private System.Windows.Forms.Button cmdConnect;
        private System.Windows.Forms.Button cmdStop;
        private System.Windows.Forms.ComboBox cmbDeviceList;
    }
}
