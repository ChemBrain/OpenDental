namespace OpenDental{
	partial class FormSheetFieldSigBox {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetFieldSigBox));
			this.textUiLabelMobile = new System.Windows.Forms.TextBox();
			this.labelUiLabelMobile = new System.Windows.Forms.Label();
			this.checkRequired = new System.Windows.Forms.CheckBox();
			this.textName = new System.Windows.Forms.TextBox();
			this.labelName = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textUiLabelMobile
			// 
			this.textUiLabelMobile.Location = new System.Drawing.Point(106, 150);
			this.textUiLabelMobile.MaxLength = 65535;
			this.textUiLabelMobile.Name = "textUiLabelMobile";
			this.textUiLabelMobile.Size = new System.Drawing.Size(197, 20);
			this.textUiLabelMobile.TabIndex = 108;
			// 
			// labelUiLabelMobile
			// 
			this.labelUiLabelMobile.Location = new System.Drawing.Point(1, 151);
			this.labelUiLabelMobile.Name = "labelUiLabelMobile";
			this.labelUiLabelMobile.Size = new System.Drawing.Size(105, 16);
			this.labelUiLabelMobile.TabIndex = 109;
			this.labelUiLabelMobile.Text = "Mobile Caption";
			this.labelUiLabelMobile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkRequired
			// 
			this.checkRequired.AutoSize = true;
			this.checkRequired.Location = new System.Drawing.Point(52, 176);
			this.checkRequired.Name = "checkRequired";
			this.checkRequired.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkRequired.Size = new System.Drawing.Size(69, 17);
			this.checkRequired.TabIndex = 101;
			this.checkRequired.Text = "Required";
			this.checkRequired.UseVisualStyleBackColor = true;
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(15, 215);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(77, 24);
			this.butDelete.TabIndex = 100;
			// 
			// textHeight
			// 
			this.textHeight.Location = new System.Drawing.Point(106, 124);
			this.textHeight.MaxVal = 2000;
			this.textHeight.MinVal = 1;
			this.textHeight.Name = "textHeight";
			this.textHeight.Size = new System.Drawing.Size(69, 20);
			this.textHeight.TabIndex = 97;
            // 
            // labelHeight
            // 
            this.labelHeight.Location = new System.Drawing.Point(35, 125);
			this.labelHeight.Name = "labelHeight";
			this.labelHeight.Size = new System.Drawing.Size(71, 16);
			this.labelHeight.TabIndex = 96;
			this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWidth
			// 
			this.textWidth.Location = new System.Drawing.Point(106, 98);
			this.textWidth.MaxVal = 2000;
			this.textWidth.MinVal = 1;
			this.textWidth.Name = "textWidth";
			this.textWidth.Size = new System.Drawing.Size(69, 20);
			this.textWidth.TabIndex = 95;
            // 
            // labelWidth
            // 
            this.labelWidth.Location = new System.Drawing.Point(35, 99);
			this.labelWidth.Name = "labelWidth";
			this.labelWidth.Size = new System.Drawing.Size(71, 16);
			this.labelWidth.TabIndex = 94;
			this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textYPos
			// 
			this.textYPos.Location = new System.Drawing.Point(106, 72);
			this.textYPos.MaxVal = 9999999;
			this.textYPos.MinVal = -100;
			this.textYPos.Name = "textYPos";
			this.textYPos.Size = new System.Drawing.Size(69, 20);
			this.textYPos.TabIndex = 93;
            // 
            // labelYPos
            // 
            this.labelYPos.Location = new System.Drawing.Point(35, 73);
			this.labelYPos.Name = "labelYPos";
			this.labelYPos.Size = new System.Drawing.Size(71, 16);
			this.labelYPos.TabIndex = 92;
			this.labelYPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textXPos
			// 
			this.textXPos.Location = new System.Drawing.Point(106, 46);
			this.textXPos.MaxVal = 2000;
			this.textXPos.MinVal = -100;
			this.textXPos.Name = "textXPos";
			this.textXPos.Size = new System.Drawing.Size(69, 20);
			this.textXPos.TabIndex = 91;
            // 
            // labelXPos
            // 
            this.labelXPos.Location = new System.Drawing.Point(35, 47);
			this.labelXPos.Name = "labelXPos";
			this.labelXPos.Size = new System.Drawing.Size(71, 16);
			this.labelXPos.TabIndex = 90;
			this.labelXPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(226, 215);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(307, 215);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			// 
			// textName
			// 
			this.textName.Location = new System.Drawing.Point(106, 20);
			this.textName.MaxLength = 255;
			this.textName.Name = "textName";
			this.textName.Size = new System.Drawing.Size(197, 20);
			this.textName.TabIndex = 110;
			// 
			// labelName
			// 
			this.labelName.Location = new System.Drawing.Point(1, 21);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(105, 16);
			this.labelName.TabIndex = 111;
			this.labelName.Text = "Name";
			this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormSheetFieldSigBox
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(394, 251);
			this.Controls.Add(this.textName);
			this.Controls.Add(this.labelName);
			this.Controls.Add(this.textUiLabelMobile);
			this.Controls.Add(this.labelUiLabelMobile);
			this.Controls.Add(this.checkRequired);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textHeight);
			this.Controls.Add(this.labelHeight);
			this.Controls.Add(this.textWidth);
			this.Controls.Add(this.labelWidth);
			this.Controls.Add(this.textYPos);
			this.Controls.Add(this.labelYPos);
			this.Controls.Add(this.textXPos);
			this.Controls.Add(this.labelXPos);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormSheetFieldSigBox";
			this.Text = "Edit Signature";
			this.Load += new System.EventHandler(this.FormSheetFieldSigBox_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox checkRequired;
		private System.Windows.Forms.TextBox textUiLabelMobile;
		private System.Windows.Forms.Label labelUiLabelMobile;
		private System.Windows.Forms.TextBox textName;
		private System.Windows.Forms.Label labelName;
	}
}