namespace OpenDental{
	partial class FormSheetFieldInput {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetFieldInput));
			this.textUiLabelMobile = new System.Windows.Forms.TextBox();
			this.labelUiLabelMobile = new System.Windows.Forms.Label();
			this.textReportableName = new System.Windows.Forms.TextBox();
			this.labelReportableName = new System.Windows.Forms.Label();
			this.textTabOrder = new OpenDental.ValidNum();
			this.labelTabOrder = new System.Windows.Forms.Label();
			this.checkRequired = new System.Windows.Forms.CheckBox();
            this.comboGrowthBehavior = new UI.ComboBoxPlus();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkFontIsBold = new System.Windows.Forms.CheckBox();
			this.textFontSize = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.comboFontName = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.listFields = new System.Windows.Forms.ListBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textUiLabelMobile
			// 
			this.textUiLabelMobile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textUiLabelMobile.Location = new System.Drawing.Point(268, 43);
			this.textUiLabelMobile.MaxLength = 65535;
			this.textUiLabelMobile.Name = "textUiLabelMobile";
			this.textUiLabelMobile.Size = new System.Drawing.Size(197, 20);
			this.textUiLabelMobile.TabIndex = 106;
			// 
			// labelUiLabelMobile
			// 
			this.labelUiLabelMobile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelUiLabelMobile.Location = new System.Drawing.Point(163, 44);
			this.labelUiLabelMobile.Name = "labelUiLabelMobile";
			this.labelUiLabelMobile.Size = new System.Drawing.Size(105, 16);
			this.labelUiLabelMobile.TabIndex = 107;
			this.labelUiLabelMobile.Text = "Mobile Caption";
			this.labelUiLabelMobile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textReportableName
			// 
			this.textReportableName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textReportableName.Location = new System.Drawing.Point(268, 17);
			this.textReportableName.Name = "textReportableName";
			this.textReportableName.Size = new System.Drawing.Size(197, 20);
			this.textReportableName.TabIndex = 104;
			// 
			// labelReportableName
			// 
			this.labelReportableName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelReportableName.Location = new System.Drawing.Point(127, 18);
			this.labelReportableName.Name = "labelReportableName";
			this.labelReportableName.Size = new System.Drawing.Size(141, 16);
			this.labelReportableName.TabIndex = 105;
			this.labelReportableName.Text = "Reportable Name";
			this.labelReportableName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTabOrder
			// 
			this.textTabOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textTabOrder.Location = new System.Drawing.Point(268, 326);
			this.textTabOrder.MaxVal = 2000;
			this.textTabOrder.MinVal = -100;
			this.textTabOrder.Name = "textTabOrder";
			this.textTabOrder.Size = new System.Drawing.Size(69, 20);
			this.textTabOrder.TabIndex = 103;
			// 
			// labelTabOrder
			// 
			this.labelTabOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTabOrder.Location = new System.Drawing.Point(197, 327);
			this.labelTabOrder.Name = "labelTabOrder";
			this.labelTabOrder.Size = new System.Drawing.Size(71, 16);
			this.labelTabOrder.TabIndex = 102;
			this.labelTabOrder.Text = "Tab Order";
			this.labelTabOrder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkRequired
			// 
			this.checkRequired.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkRequired.AutoSize = true;
			this.checkRequired.Location = new System.Drawing.Point(214, 303);
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
			this.butDelete.Location = new System.Drawing.Point(15, 375);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(77, 24);
			this.butDelete.TabIndex = 100;
			// 
			// comboGrowthBehavior
			// 
			this.comboGrowthBehavior.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboGrowthBehavior.Location = new System.Drawing.Point(268, 171);
			this.comboGrowthBehavior.Name = "comboGrowthBehavior";
			this.comboGrowthBehavior.Size = new System.Drawing.Size(197, 21);
			this.comboGrowthBehavior.TabIndex = 99;
			// 
			// label9
			// 
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label9.Location = new System.Drawing.Point(161, 172);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(107, 16);
			this.label9.TabIndex = 98;
			this.label9.Text = "Growth Behavior";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textHeight
			// 
			this.textHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textHeight.Location = new System.Drawing.Point(268, 277);
			this.textHeight.MaxVal = 2000;
			this.textHeight.MinVal = 1;
			this.textHeight.Name = "textHeight";
			this.textHeight.Size = new System.Drawing.Size(69, 20);
			this.textHeight.TabIndex = 97;
			// 
			// label8
			// 
			this.labelHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelHeight.Location = new System.Drawing.Point(197, 278);
			this.labelHeight.Name = "labelHeight";
			this.labelHeight.Size = new System.Drawing.Size(71, 16);
			this.labelHeight.TabIndex = 96;
			this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWidth
			// 
			this.textWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textWidth.Location = new System.Drawing.Point(268, 251);
			this.textWidth.MaxVal = 2000;
			this.textWidth.MinVal = 1;
			this.textWidth.Name = "textWidth";
			this.textWidth.Size = new System.Drawing.Size(69, 20);
			this.textWidth.TabIndex = 95;
			// 
			// label7
			// 
			this.labelWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelWidth.Location = new System.Drawing.Point(197, 252);
			this.labelWidth.Name = "labelWidth";
			this.labelWidth.Size = new System.Drawing.Size(71, 16);
			this.labelWidth.TabIndex = 94;
			this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textYPos
			// 
			this.textYPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textYPos.Location = new System.Drawing.Point(268, 225);
			this.textYPos.MaxVal = 9999999;
			this.textYPos.MinVal = -100;
			this.textYPos.Name = "textYPos";
			this.textYPos.Size = new System.Drawing.Size(69, 20);
			this.textYPos.TabIndex = 93;
			// 
			// label6
			// 
			this.labelYPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelYPos.Location = new System.Drawing.Point(197, 226);
			this.labelYPos.Name = "labelYPos";
			this.labelYPos.Size = new System.Drawing.Size(71, 16);
			this.labelYPos.TabIndex = 92;
			this.labelYPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textXPos
			// 
			this.textXPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textXPos.Location = new System.Drawing.Point(268, 199);
			this.textXPos.MaxVal = 2000;
			this.textXPos.MinVal = -100;
			this.textXPos.Name = "textXPos";
			this.textXPos.Size = new System.Drawing.Size(69, 20);
			this.textXPos.TabIndex = 91;
			// 
			// label5
			// 
			this.labelXPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelXPos.Location = new System.Drawing.Point(197, 200);
			this.labelXPos.Name = "labelXPos";
			this.labelXPos.Size = new System.Drawing.Size(71, 16);
			this.labelXPos.TabIndex = 90;
			this.labelXPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.checkFontIsBold);
			this.groupBox1.Controls.Add(this.textFontSize);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.comboFontName);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(182, 69);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(283, 96);
			this.groupBox1.TabIndex = 88;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Font";
			// 
			// checkFontIsBold
			// 
			this.checkFontIsBold.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFontIsBold.Location = new System.Drawing.Point(16, 66);
			this.checkFontIsBold.Name = "checkFontIsBold";
			this.checkFontIsBold.Size = new System.Drawing.Size(85, 20);
			this.checkFontIsBold.TabIndex = 90;
			this.checkFontIsBold.Text = "Bold";
			this.checkFontIsBold.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFontIsBold.UseVisualStyleBackColor = true;
			// 
			// textFontSize
			// 
			this.textFontSize.Location = new System.Drawing.Point(86, 40);
			this.textFontSize.Name = "textFontSize";
			this.textFontSize.Size = new System.Drawing.Size(44, 20);
			this.textFontSize.TabIndex = 89;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(13, 41);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(71, 16);
			this.label4.TabIndex = 89;
			this.label4.Text = "Size";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboFontName
			// 
			this.comboFontName.FormattingEnabled = true;
			this.comboFontName.Location = new System.Drawing.Point(86, 14);
			this.comboFontName.Name = "comboFontName";
			this.comboFontName.Size = new System.Drawing.Size(191, 21);
			this.comboFontName.TabIndex = 88;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(13, 15);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(71, 16);
			this.label3.TabIndex = 87;
			this.label3.Text = "Name";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(13, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(108, 16);
			this.label2.TabIndex = 86;
			this.label2.Text = "Field Name";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listFields
			// 
			this.listFields.FormattingEnabled = true;
			this.listFields.Location = new System.Drawing.Point(15, 37);
			this.listFields.Name = "listFields";
			this.listFields.Size = new System.Drawing.Size(142, 290);
			this.listFields.TabIndex = 85;
			this.listFields.SelectedIndexChanged += new System.EventHandler(this.listFields_SelectedIndexChanged);
			this.listFields.DoubleClick += new System.EventHandler(this.listFields_DoubleClick);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(407, 345);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(407, 375);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
            this.butCancel.TabIndex = 2;
			// 
			// FormSheetFieldInput
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(494, 411);
			this.Controls.Add(this.textUiLabelMobile);
			this.Controls.Add(this.labelUiLabelMobile);
			this.Controls.Add(this.textReportableName);
			this.Controls.Add(this.labelReportableName);
			this.Controls.Add(this.textTabOrder);
			this.Controls.Add(this.labelTabOrder);
			this.Controls.Add(this.checkRequired);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.comboGrowthBehavior);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.textHeight);
			this.Controls.Add(this.labelHeight);
			this.Controls.Add(this.textWidth);
			this.Controls.Add(this.labelWidth);
			this.Controls.Add(this.textYPos);
			this.Controls.Add(this.labelYPos);
			this.Controls.Add(this.textXPos);
			this.Controls.Add(this.labelXPos);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.listFields);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormSheetFieldInput";
			this.Text = "Edit Input Field";
			this.Load += new System.EventHandler(this.FormSheetFieldInput_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox listFields;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboFontName;
		private System.Windows.Forms.CheckBox checkFontIsBold;
		private System.Windows.Forms.TextBox textFontSize;
		private System.Windows.Forms.Label label4;
        private UI.ComboBoxPlus comboGrowthBehavior;
        private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox checkRequired;
		private System.Windows.Forms.Label labelTabOrder;
		private ValidNum textTabOrder;
		private System.Windows.Forms.TextBox textReportableName;
		private System.Windows.Forms.Label labelReportableName;
		private System.Windows.Forms.TextBox textUiLabelMobile;
		private System.Windows.Forms.Label labelUiLabelMobile;
	}
}