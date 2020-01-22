namespace OpenDental{
	partial class FormSheetFieldLine {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetFieldLine));
			this.label1 = new System.Windows.Forms.Label();
			this.checkPmtOpt = new System.Windows.Forms.CheckBox();
			this.butColor = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(364, 141);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(364, 171);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(15, 171);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(77, 24);
			this.butDelete.TabIndex = 100;
			// 
			// label1
			// 
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(204, 67);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(193, 53);
			this.label1.TabIndex = 110;
			this.label1.Text = "The line will extend from x,y to x+w,y+h.  So negative width and height are allow" +
    "ed.";
			// 
			// textHeight
			// 
			this.textHeight.Location = new System.Drawing.Point(129, 90);
			this.textHeight.MaxVal = 2000;
			this.textHeight.MinVal = -2000;
			this.textHeight.Name = "textHeight";
			this.textHeight.Size = new System.Drawing.Size(69, 20);
			this.textHeight.TabIndex = 109;
			// 
			// label8
			// 
			this.labelHeight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.labelHeight.Location = new System.Drawing.Point(58, 91);
			this.labelHeight.Name = "labelHeight";
			this.labelHeight.Size = new System.Drawing.Size(71, 16);
			this.labelHeight.TabIndex = 108;
			this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWidth
			// 
			this.textWidth.Location = new System.Drawing.Point(129, 64);
			this.textWidth.MaxVal = 2000;
			this.textWidth.MinVal = -2000;
			this.textWidth.Name = "textWidth";
			this.textWidth.Size = new System.Drawing.Size(69, 20);
			this.textWidth.TabIndex = 107;
			// 
			// label7
			// 
			this.labelWidth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.labelWidth.Location = new System.Drawing.Point(58, 65);
			this.labelWidth.Name = "labelWidth";
			this.labelWidth.Size = new System.Drawing.Size(71, 16);
			this.labelWidth.TabIndex = 106;
			this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textYPos
			// 
			this.textYPos.Location = new System.Drawing.Point(129, 38);
			this.textYPos.MaxVal = 9999999;
			this.textYPos.MinVal = -100;
			this.textYPos.Name = "textYPos";
			this.textYPos.Size = new System.Drawing.Size(69, 20);
			this.textYPos.TabIndex = 105;
			// 
			// label6
			// 
			this.labelYPos.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.labelYPos.Location = new System.Drawing.Point(58, 39);
			this.labelYPos.Name = "labelYPos";
			this.labelYPos.Size = new System.Drawing.Size(71, 16);
			this.labelYPos.TabIndex = 104;
			this.labelYPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textXPos
			// 
			this.textXPos.Location = new System.Drawing.Point(129, 12);
			this.textXPos.MaxVal = 2000;
			this.textXPos.MinVal = -100;
			this.textXPos.Name = "textXPos";
			this.textXPos.Size = new System.Drawing.Size(69, 20);
			this.textXPos.TabIndex = 103;
			// 
			// label5
			// 
			this.labelXPos.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.labelXPos.Location = new System.Drawing.Point(58, 13);
			this.labelXPos.Name = "labelXPos";
			this.labelXPos.Size = new System.Drawing.Size(71, 16);
			this.labelXPos.TabIndex = 102;
			this.labelXPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPmtOpt
			// 
			this.checkPmtOpt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPmtOpt.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPmtOpt.Location = new System.Drawing.Point(12, 138);
			this.checkPmtOpt.Name = "checkPmtOpt";
			this.checkPmtOpt.Size = new System.Drawing.Size(130, 20);
			this.checkPmtOpt.TabIndex = 236;
			this.checkPmtOpt.Text = "Is Payment Option";
			this.checkPmtOpt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butColor
			// 
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColor.Location = new System.Drawing.Point(129, 116);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(30, 20);
			this.butColor.TabIndex = 237;
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			// 
			// label2
			// 
			this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label2.Location = new System.Drawing.Point(52, 116);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(71, 16);
			this.label2.TabIndex = 238;
			this.label2.Text = "Color";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormSheetFieldLine
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(451, 207);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butColor);
			this.Controls.Add(this.checkPmtOpt);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textHeight);
			this.Controls.Add(this.labelHeight);
			this.Controls.Add(this.textWidth);
			this.Controls.Add(this.labelWidth);
			this.Controls.Add(this.textYPos);
			this.Controls.Add(this.labelYPos);
			this.Controls.Add(this.textXPos);
			this.Controls.Add(this.labelXPos);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormSheetFieldLine";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Line";
			this.Load += new System.EventHandler(this.FormSheetFieldLine_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox checkPmtOpt;
		private System.Windows.Forms.Button butColor;
		private System.Windows.Forms.Label label2;
	}
}