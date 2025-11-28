namespace DVLD_System.Licenses
{
    partial class frmShowPersonLicenseInfoWithHistory
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
            this.btClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ctrrDrivereLicenses1 = new DVLD_System.Licenses.Cnotrol.ctrrDrivereLicenses();
            this.ctrPersonDetailsWithFilter1 = new DVLD_System.Controls.ctrPersonDetailsWithFilter();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btClose
            // 
            this.btClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btClose.Location = new System.Drawing.Point(1017, 1118);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(127, 39);
            this.btClose.TabIndex = 2;
            this.btClose.Text = "Close";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(545, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 32);
            this.label1.TabIndex = 3;
            this.label1.Text = "License History";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::DVLD_System.Properties.Resources.LicenseHistory;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(30, 36);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(165, 165);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // ctrrDrivereLicenses1
            // 
            this.ctrrDrivereLicenses1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ctrrDrivereLicenses1.Location = new System.Drawing.Point(233, 635);
            this.ctrrDrivereLicenses1.Name = "ctrrDrivereLicenses1";
            this.ctrrDrivereLicenses1.Size = new System.Drawing.Size(1105, 464);
            this.ctrrDrivereLicenses1.TabIndex = 1;
            // 
            // ctrPersonDetailsWithFilter1
            // 
            this.ctrPersonDetailsWithFilter1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ctrPersonDetailsWithFilter1.FilterEnabled = true;
            this.ctrPersonDetailsWithFilter1.Location = new System.Drawing.Point(233, 96);
            this.ctrPersonDetailsWithFilter1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ctrPersonDetailsWithFilter1.Name = "ctrPersonDetailsWithFilter1";
            this.ctrPersonDetailsWithFilter1.ShowAddPerson = true;
            this.ctrPersonDetailsWithFilter1.Size = new System.Drawing.Size(1105, 505);
            this.ctrPersonDetailsWithFilter1.TabIndex = 0;
            this.ctrPersonDetailsWithFilter1.OnPersonSelected += new System.Action<int>(this.ctrPersonDetailsWithFilter1_OnPersonSelected);
            // 
            // frmShowPersonLicenseInfoWithHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(0, 1000);
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(1432, 840);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.ctrrDrivereLicenses1);
            this.Controls.Add(this.ctrPersonDetailsWithFilter1);
            this.Name = "frmShowPersonLicenseInfoWithHistory";
            this.Text = "Show Person LicenseInfo With History";
            this.Load += new System.EventHandler(this.frmShowPersonLicenseInfoWithHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DVLD_System.Controls.ctrPersonDetailsWithFilter ctrPersonDetailsWithFilter1;
        private Cnotrol.ctrrDrivereLicenses ctrrDrivereLicenses1;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}