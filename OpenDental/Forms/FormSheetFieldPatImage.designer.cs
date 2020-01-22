namespace OpenDental{
	partial class FormSheetFieldPatImage {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetFieldPatImage));
			this.label1 = new System.Windows.Forms.Label();
			this.comboImageCategory = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
            // 
            // labelXPos
            // 
            this.labelXPos.Location = new System.Drawing.Point(70, 44);
			this.labelXPos.Name = "labelXPos";
			this.labelXPos.Size = new System.Drawing.Size(71, 16);
			this.labelXPos.TabIndex = 90;
			this.labelXPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelYPos
            // 
            this.labelYPos.Location = new System.Drawing.Point(70, 70);
			this.labelYPos.Name = "labelYPos";
			this.labelYPos.Size = new System.Drawing.Size(71, 16);
			this.labelYPos.TabIndex = 92;
			this.labelYPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelWidth
            // 
            this.labelWidth.Location = new System.Drawing.Point(70, 96);
			this.labelWidth.Name = "labelWidth";
			this.labelWidth.Size = new System.Drawing.Size(71, 16);
			this.labelWidth.TabIndex = 94;
			this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHeight
            // 
            this.labelHeight.Location = new System.Drawing.Point(70, 122);
			this.labelHeight.Name = "labelHeight";
			this.labelHeight.Size = new System.Drawing.Size(71, 16);
			this.labelHeight.TabIndex = 96;
			this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(26, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(113, 16);
			this.label1.TabIndex = 101;
			this.label1.Text = "Image Category";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboImageCategory
			// 
			this.comboImageCategory.FormattingEnabled = true;
			this.comboImageCategory.Location = new System.Drawing.Point(141, 16);
			this.comboImageCategory.MaxDropDownItems = 100;
			this.comboImageCategory.Name = "comboImageCategory";
			this.comboImageCategory.Size = new System.Drawing.Size(257, 21);
			this.comboImageCategory.TabIndex = 106;
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(15, 167);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(77, 24);
			this.butDelete.TabIndex = 100;
			// 
			// textHeight
			// 
			this.textHeight.Location = new System.Drawing.Point(141, 121);
			this.textHeight.MaxVal = 2000;
			this.textHeight.MinVal = 1;
			this.textHeight.Name = "textHeight";
			this.textHeight.Size = new System.Drawing.Size(51, 20);
			this.textHeight.TabIndex = 97;
			// 
			// textWidth
			// 
			this.textWidth.Location = new System.Drawing.Point(141, 95);
			this.textWidth.MaxVal = 2000;
			this.textWidth.MinVal = 1;
			this.textWidth.Name = "textWidth";
			this.textWidth.Size = new System.Drawing.Size(51, 20);
			this.textWidth.TabIndex = 95;
			// 
			// textYPos
			// 
			this.textYPos.Location = new System.Drawing.Point(141, 69);
			this.textYPos.MaxVal = 9999999;
			this.textYPos.MinVal = -100;
			this.textYPos.Name = "textYPos";
			this.textYPos.Size = new System.Drawing.Size(51, 20);
			this.textYPos.TabIndex = 93;
			// 
			// textXPos
			// 
			this.textXPos.Location = new System.Drawing.Point(141, 43);
			this.textXPos.MaxVal = 2000;
			this.textXPos.MinVal = -100;
			this.textXPos.Name = "textXPos";
			this.textXPos.Size = new System.Drawing.Size(51, 20);
			this.textXPos.TabIndex = 91;
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(420, 137);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(420, 167);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			// 
			// FormSheetFieldPatImage
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(507, 203);
			this.Controls.Add(this.comboImageCategory);
			this.Controls.Add(this.label1);
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
			this.Name = "FormSheetFieldPatImage";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Pat Image";
			this.Load += new System.EventHandler(this.FormSheetFieldPatImage_Load);
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboImageCategory;
	}
}