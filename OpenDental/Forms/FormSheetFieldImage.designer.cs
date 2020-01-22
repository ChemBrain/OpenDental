namespace OpenDental{
	partial class FormSheetFieldImage {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetFieldImage));
            this.label1 = new System.Windows.Forms.Label();
            this.textFullPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboFieldName = new System.Windows.Forms.ComboBox();
            this.textWidth2 = new System.Windows.Forms.TextBox();
            this.textHeight2 = new System.Windows.Forms.TextBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.groupFileSize = new System.Windows.Forms.GroupBox();
            this.checkRatio = new System.Windows.Forms.CheckBox();
            this.butShrink = new OpenDental.UI.Button();
            this.butImport = new OpenDental.UI.Button();
            this.groupFileSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupFileSize.Controls.Add(this.textWidth2);
            this.groupFileSize.Controls.Add(this.textHeight2);
            this.groupFileSize.Location = new System.Drawing.Point(198, 369);
            this.groupFileSize.Size = new System.Drawing.Size(63, 66);
            this.groupFileSize.TabIndex = 113;
            this.groupFileSize.Text = "File Size";
            // 
            // textXPos
            // 
            this.textXPos.Location = new System.Drawing.Point(141, 331);
            this.textXPos.MaxVal = 2000;
            this.textXPos.MinVal = -100;
            this.textXPos.Size = new System.Drawing.Size(51, 20);
            this.textXPos.TabIndex = 91;
            // 
            // textYPos
            // 
            this.textYPos.Location = new System.Drawing.Point(141, 357);
            this.textYPos.MaxVal = 9999999;
            this.textYPos.MinVal = -100;
            this.textYPos.Size = new System.Drawing.Size(51, 20);
            this.textYPos.TabIndex = 93;
            // 
            // textWidth
            // 
            this.textWidth.Location = new System.Drawing.Point(141, 383);
            this.textWidth.MaxVal = 4000;
            this.textWidth.MinVal = 1;
            this.textWidth.Size = new System.Drawing.Size(51, 20);
            this.textWidth.TabIndex = 95;
            this.textWidth.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textWidth_KeyUp);
            // 
            // textHeight
            // 
            this.textHeight.Location = new System.Drawing.Point(141, 409);
            this.textHeight.MaxVal = 4000;
            this.textHeight.MinVal = 1;
            this.textHeight.Size = new System.Drawing.Size(51, 20);
            this.textHeight.TabIndex = 97;
            this.textHeight.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textHeight_KeyUp);
            // 
            // butDelete
            // 
            this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
            this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butDelete.Location = new System.Drawing.Point(15, 525);
            this.butDelete.Size = new System.Drawing.Size(77, 24);
            this.butDelete.TabIndex = 100;
            // 
            // butOK
            // 
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOK.Location = new System.Drawing.Point(514, 495);
            this.butOK.Size = new System.Drawing.Size(75, 24);
            this.butOK.TabIndex = 3;
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Location = new System.Drawing.Point(514, 525);
            this.butCancel.Size = new System.Drawing.Size(75, 24);
            this.butCancel.TabIndex = 2;
            // 
            // labelXPos
            // 
            this.labelXPos.Location = new System.Drawing.Point(70, 332);
            this.labelXPos.Name = "labelXPos";
            this.labelXPos.Size = new System.Drawing.Size(71, 16);
            this.labelXPos.TabIndex = 90;
            this.labelXPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelYPos
            // 
            this.labelYPos.Location = new System.Drawing.Point(70, 358);
            this.labelYPos.Name = "labelYPos";
            this.labelYPos.Size = new System.Drawing.Size(71, 16);
            this.labelYPos.TabIndex = 92;
            this.labelYPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelWidth
            // 
            this.labelWidth.Location = new System.Drawing.Point(70, 384);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(71, 16);
            this.labelWidth.TabIndex = 94;
            this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHeight
            // 
            this.labelHeight.Location = new System.Drawing.Point(70, 410);
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
            this.label1.Text = "File Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textFullPath
            // 
            this.textFullPath.Location = new System.Drawing.Point(141, 43);
            this.textFullPath.Name = "textFullPath";
            this.textFullPath.ReadOnly = true;
            this.textFullPath.Size = new System.Drawing.Size(434, 20);
            this.textFullPath.TabIndex = 104;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(26, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 16);
            this.label2.TabIndex = 103;
            this.label2.Text = "Full Path";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboFieldName
            // 
            this.comboFieldName.FormattingEnabled = true;
            this.comboFieldName.Location = new System.Drawing.Point(141, 16);
            this.comboFieldName.MaxDropDownItems = 30;
            this.comboFieldName.Name = "comboFieldName";
            this.comboFieldName.Size = new System.Drawing.Size(257, 21);
            this.comboFieldName.TabIndex = 106;
            this.comboFieldName.SelectionChangeCommitted += new System.EventHandler(this.comboFieldName_SelectionChangeCommitted);
            this.comboFieldName.TextUpdate += new System.EventHandler(this.comboFieldName_TextUpdate);
            // 
            // textWidth2
            // 
            this.textWidth2.Location = new System.Drawing.Point(6, 14);
            this.textWidth2.Name = "textWidth2";
            this.textWidth2.ReadOnly = true;
            this.textWidth2.Size = new System.Drawing.Size(51, 20);
            this.textWidth2.TabIndex = 110;
            // 
            // textHeight2
            // 
            this.textHeight2.Location = new System.Drawing.Point(6, 40);
            this.textHeight2.Name = "textHeight2";
            this.textHeight2.ReadOnly = true;
            this.textHeight2.Size = new System.Drawing.Size(51, 20);
            this.textHeight2.TabIndex = 111;
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(141, 69);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(255, 255);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 112;
            this.pictureBox.TabStop = false;
            // 
            // checkRatio
            // 
            this.checkRatio.Checked = true;
            this.checkRatio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkRatio.Location = new System.Drawing.Point(267, 413);
            this.checkRatio.Name = "checkRatio";
            this.checkRatio.Size = new System.Drawing.Size(104, 20);
            this.checkRatio.TabIndex = 115;
            this.checkRatio.Text = "Maintain Ratio";
            this.checkRatio.UseVisualStyleBackColor = true;
            // 
            // butShrink
            // 
            this.butShrink.Location = new System.Drawing.Point(267, 381);
            this.butShrink.Name = "butShrink";
            this.butShrink.Size = new System.Drawing.Size(79, 24);
            this.butShrink.TabIndex = 114;
            this.butShrink.Text = "ShrinkToFit";
            this.butShrink.Click += new System.EventHandler(this.butShrink_Click);
            // 
            // butImport
            // 
            this.butImport.Location = new System.Drawing.Point(404, 14);
            this.butImport.Name = "butImport";
            this.butImport.Size = new System.Drawing.Size(75, 24);
            this.butImport.TabIndex = 105;
            this.butImport.Text = "Import";
            this.butImport.Click += new System.EventHandler(this.butImport_Click);
            // 
            // FormSheetFieldImage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(601, 561);
            this.Controls.Add(this.checkRatio);
            this.Controls.Add(this.butShrink);
            this.Controls.Add(this.groupFileSize);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.comboFieldName);
            this.Controls.Add(this.butImport);
            this.Controls.Add(this.textFullPath);
            this.Controls.Add(this.label2);
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
            this.Name = "FormSheetFieldImage";
            this.Text = "Edit Image";
            this.Load += new System.EventHandler(this.FormSheetFieldImage_Load);
            this.Controls.SetChildIndex(this.butCancel, 0);
            this.Controls.SetChildIndex(this.butOK, 0);
            this.Controls.SetChildIndex(this.labelXPos, 0);
            this.Controls.SetChildIndex(this.textXPos, 0);
            this.Controls.SetChildIndex(this.labelYPos, 0);
            this.Controls.SetChildIndex(this.textYPos, 0);
            this.Controls.SetChildIndex(this.labelWidth, 0);
            this.Controls.SetChildIndex(this.textWidth, 0);
            this.Controls.SetChildIndex(this.labelHeight, 0);
            this.Controls.SetChildIndex(this.textHeight, 0);
            this.Controls.SetChildIndex(this.butDelete, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.textFullPath, 0);
            this.Controls.SetChildIndex(this.butImport, 0);
            this.Controls.SetChildIndex(this.comboFieldName, 0);
            this.Controls.SetChildIndex(this.pictureBox, 0);
            this.Controls.SetChildIndex(this.groupFileSize, 0);
            this.Controls.SetChildIndex(this.butShrink, 0);
            this.Controls.SetChildIndex(this.checkRatio, 0);
            this.groupFileSize.ResumeLayout(false);
            this.groupFileSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textFullPath;
		private System.Windows.Forms.Label label2;
		private OpenDental.UI.Button butImport;
		private System.Windows.Forms.ComboBox comboFieldName;
		private System.Windows.Forms.TextBox textWidth2;
		private System.Windows.Forms.TextBox textHeight2;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.GroupBox groupFileSize;
		private OpenDental.UI.Button butShrink;
		private System.Windows.Forms.CheckBox checkRatio;
	}
}