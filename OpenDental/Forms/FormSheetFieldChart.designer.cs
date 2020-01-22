namespace OpenDental{
	partial class FormSheetFieldChart {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetFieldChart));
            this.groupBoxMain = new System.Windows.Forms.GroupBox();
            this.radioPermanent = new System.Windows.Forms.RadioButton();
            this.radioPrimary = new System.Windows.Forms.RadioButton();
            this.groupBoxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBoxMain.Controls.Add(this.radioPermanent);
            this.groupBoxMain.Controls.Add(this.radioPrimary);
            this.groupBoxMain.Location = new System.Drawing.Point(88, 37);
            this.groupBoxMain.Size = new System.Drawing.Size(225, 74);
            this.groupBoxMain.TabIndex = 101;
            this.groupBoxMain.Text = "Chart Type";
            // 
            // butDelete
            // 
            this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
            this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butDelete.Location = new System.Drawing.Point(12, 164);
            this.butDelete.Size = new System.Drawing.Size(77, 24);
            this.butDelete.TabIndex = 100;
            // 
            // butOK
            // 
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOK.Location = new System.Drawing.Point(316, 134);
            this.butOK.Size = new System.Drawing.Size(75, 24);
            this.butOK.TabIndex = 3;
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Location = new System.Drawing.Point(316, 164);
            this.butCancel.Size = new System.Drawing.Size(75, 24);
            this.butCancel.TabIndex = 2;
            // 
            // radioPermanent
            // 
            this.radioPermanent.Location = new System.Drawing.Point(17, 44);
            this.radioPermanent.Name = "radioPermanent";
            this.radioPermanent.Size = new System.Drawing.Size(202, 17);
            this.radioPermanent.TabIndex = 1;
            this.radioPermanent.TabStop = true;
            this.radioPermanent.Text = "Permanent Teeth";
            this.radioPermanent.UseVisualStyleBackColor = true;
            // 
            // radioPrimary
            // 
            this.radioPrimary.Location = new System.Drawing.Point(17, 21);
            this.radioPrimary.Name = "radioPrimary";
            this.radioPrimary.Size = new System.Drawing.Size(202, 17);
            this.radioPrimary.TabIndex = 0;
            this.radioPrimary.TabStop = true;
            this.radioPrimary.Text = "Primary Teeth";
            this.radioPrimary.UseVisualStyleBackColor = true;
            // 
            // FormSheetFieldChart
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(403, 200);
            this.Controls.Add(this.groupBoxMain);
            this.Controls.Add(this.butDelete);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.butCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(419, 238);
            this.Name = "FormSheetFieldChart";
            this.Text = "Edit Screen Chart";
            this.Load += new System.EventHandler(this.FormSheetFieldChart_Load);
            this.groupBoxMain.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMain;
        private System.Windows.Forms.RadioButton radioPermanent;
		private System.Windows.Forms.RadioButton radioPrimary;
	}
}