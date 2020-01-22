namespace OpenDental{
	partial class FormSheetFieldSpecial {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetFieldSpecial));
			this.label2 = new System.Windows.Forms.Label();
            
			this.labelSpecialInfo = new System.Windows.Forms.Label();
			this.listBoxAvailable = new System.Windows.Forms.ListBox();
			this.comboGrowthBehavior = new UI.ComboBoxPlus();
			this.labelGrowth = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(13, 23);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(123, 15);
			this.label2.TabIndex = 86;
			this.label2.Text = "FieldName:\r\n";
            // 
            // labelXPos
            // 
            this.labelXPos.Location = new System.Drawing.Point(197, 117);
			this.labelXPos.Name = "labelXPos";
			this.labelXPos.Size = new System.Drawing.Size(71, 16);
			this.labelXPos.TabIndex = 90;
			this.labelXPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelYPos
            // 
            this.labelYPos.Location = new System.Drawing.Point(197, 143);
			this.labelYPos.Name = "labelYPos";
			this.labelYPos.Size = new System.Drawing.Size(71, 16);
			this.labelYPos.TabIndex = 92;
			this.labelYPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelWidth
            // 
            this.labelWidth.Location = new System.Drawing.Point(197, 169);
			this.labelWidth.Name = "labelWidth";
			this.labelWidth.Size = new System.Drawing.Size(71, 16);
			this.labelWidth.TabIndex = 94;
			this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHeight
            // 
            this.labelHeight.Location = new System.Drawing.Point(197, 195);
			this.labelHeight.Name = "labelHeight";
			this.labelHeight.Size = new System.Drawing.Size(71, 16);
			this.labelHeight.TabIndex = 96;
			this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textHeight
			// 
			this.textHeight.Location = new System.Drawing.Point(268, 194);
			this.textHeight.MaxVal = 2000;
			this.textHeight.MinVal = -100;
			this.textHeight.Name = "textHeight";
			this.textHeight.Size = new System.Drawing.Size(69, 20);
			this.textHeight.TabIndex = 97;
			// 
			// textWidth
			// 
			this.textWidth.Location = new System.Drawing.Point(268, 168);
			this.textWidth.MaxVal = 2000;
			this.textWidth.MinVal = -100;
			this.textWidth.Name = "textWidth";
			this.textWidth.Size = new System.Drawing.Size(69, 20);
			this.textWidth.TabIndex = 95;
			// 
			// textYPos
			// 
			this.textYPos.Location = new System.Drawing.Point(268, 142);
			this.textYPos.MaxVal = 9999999;
			this.textYPos.MinVal = -100;
			this.textYPos.Name = "textYPos";
			this.textYPos.Size = new System.Drawing.Size(69, 20);
			this.textYPos.TabIndex = 93;
			// 
			// textXPos
			// 
			this.textXPos.Location = new System.Drawing.Point(268, 116);
			this.textXPos.MaxVal = 2000;
			this.textXPos.MinVal = -100;
			this.textXPos.Name = "textXPos";
			this.textXPos.Size = new System.Drawing.Size(69, 20);
			this.textXPos.TabIndex = 91;
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(391, 205);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(391, 235);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(15, 235);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(77, 24);
			this.butDelete.TabIndex = 100;
			// 
			// labelSpecialInfo
			// 
			this.labelSpecialInfo.Location = new System.Drawing.Point(175, 22);
			this.labelSpecialInfo.Name = "labelSpecialInfo";
			this.labelSpecialInfo.Size = new System.Drawing.Size(207, 65);
			this.labelSpecialInfo.TabIndex = 101;
			this.labelSpecialInfo.Text = "Will contain information pertaining to the type of special field selected.";
			// 
			// listBoxAvailable
			// 
			this.listBoxAvailable.FormattingEnabled = true;
			this.listBoxAvailable.Location = new System.Drawing.Point(16, 41);
			this.listBoxAvailable.Name = "listBoxAvailable";
			this.listBoxAvailable.Size = new System.Drawing.Size(153, 173);
			this.listBoxAvailable.TabIndex = 103;
			this.listBoxAvailable.SelectedIndexChanged += new System.EventHandler(this.listBoxAvailable_SelectedIndexChanged);
			// 
			// comboGrowthBehavior
			// 
			this.comboGrowthBehavior.Location = new System.Drawing.Point(269, 89);
			this.comboGrowthBehavior.Name = "comboGrowthBehavior";
			this.comboGrowthBehavior.Size = new System.Drawing.Size(197, 21);
			this.comboGrowthBehavior.TabIndex = 116;
			this.comboGrowthBehavior.Visible = false;
			// 
			// labelGrowth
			// 
			this.labelGrowth.Location = new System.Drawing.Point(175, 90);
			this.labelGrowth.Name = "labelGrowth";
			this.labelGrowth.Size = new System.Drawing.Size(94, 16);
			this.labelGrowth.TabIndex = 115;
			this.labelGrowth.Text = "Growth Behavior";
			this.labelGrowth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelGrowth.Visible = false;
			// 
			// FormSheetFieldSpecial
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(478, 271);
			this.Controls.Add(this.comboGrowthBehavior);
			this.Controls.Add(this.labelGrowth);
			this.Controls.Add(this.listBoxAvailable);
			this.Controls.Add(this.labelSpecialInfo);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textHeight);
			this.Controls.Add(this.labelHeight);
			this.Controls.Add(this.textWidth);
			this.Controls.Add(this.labelWidth);
			this.Controls.Add(this.textYPos);
			this.Controls.Add(this.labelYPos);
			this.Controls.Add(this.textXPos);
			this.Controls.Add(this.labelXPos);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormSheetFieldSpecial";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Special";
			this.Load += new System.EventHandler(this.FormSheetFieldDefEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelSpecialInfo;
		private System.Windows.Forms.ListBox listBoxAvailable;
		private UI.ComboBoxPlus comboGrowthBehavior;
		private System.Windows.Forms.Label labelGrowth;
   }
}