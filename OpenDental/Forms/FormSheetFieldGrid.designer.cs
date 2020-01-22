namespace OpenDental{
	partial class FormSheetFieldGrid {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetFieldGrid));
            this.comboGrowthBehavior = new UI.ComboBoxPlus();
            this.labelGrowthBehavior = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textGridType = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelXPos
            // 
            this.labelXPos.Location = new System.Drawing.Point(143, 100);
            this.labelXPos.Size = new System.Drawing.Size(71, 16);
            this.labelXPos.TabIndex = 105;
            this.labelXPos.Text = "X Pos";
            this.labelXPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelYPos
            // 
            this.labelYPos.Location = new System.Drawing.Point(143, 126);
            this.labelYPos.Size = new System.Drawing.Size(71, 16);
            this.labelYPos.TabIndex = 107;
            this.labelYPos.Text = "Y Pos";
            this.labelYPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelWidth
            // 
            this.labelWidth.Location = new System.Drawing.Point(143, 152);
            this.labelWidth.Size = new System.Drawing.Size(71, 16);
            this.labelWidth.TabIndex = 109;
            this.labelWidth.Text = "Width";
            this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHeight
            // 
            this.labelHeight.Location = new System.Drawing.Point(143, 178);
            this.labelHeight.Size = new System.Drawing.Size(71, 16);
            this.labelHeight.TabIndex = 111;
            this.labelHeight.Text = "Height";
            this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textXPos
            // 
            this.textXPos.Location = new System.Drawing.Point(214, 99);
            this.textXPos.MaxVal = 2000;
            this.textXPos.MinVal = -100;
            this.textXPos.Size = new System.Drawing.Size(69, 20);
            this.textXPos.TabIndex = 106;
            // 
            // textYPos
            // 
            this.textYPos.Location = new System.Drawing.Point(214, 125);
            this.textYPos.MaxVal = 9999999;
            this.textYPos.MinVal = -100;
            this.textYPos.Size = new System.Drawing.Size(69, 20);
            this.textYPos.TabIndex = 108;
            // 
            // textWidth
            // 
            this.textWidth.Enabled = false;
            this.textWidth.Location = new System.Drawing.Point(214, 151);
            this.textWidth.MaxVal = 2000;
            this.textWidth.MinVal = -100;
            this.textWidth.Size = new System.Drawing.Size(69, 20);
            this.textWidth.TabIndex = 110;
            // 
            // textHeight
            // 
            this.textHeight.Enabled = false;
            this.textHeight.Location = new System.Drawing.Point(214, 177);
            this.textHeight.MaxVal = 2000;
            this.textHeight.MinVal = -100;
            this.textHeight.Size = new System.Drawing.Size(69, 20);
            this.textHeight.TabIndex = 112;
            // 
            // comboGrowthBehavior
            // 
            this.comboGrowthBehavior.Enabled = false;
            this.comboGrowthBehavior.Location = new System.Drawing.Point(214, 71);
            this.comboGrowthBehavior.Size = new System.Drawing.Size(197, 21);
            this.comboGrowthBehavior.TabIndex = 114;
            // 
            // butDelete
            // 
            this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
            this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butDelete.Location = new System.Drawing.Point(15, 247);
            this.butDelete.Size = new System.Drawing.Size(77, 24);
            this.butDelete.TabIndex = 115;
            this.butDelete.Text = "Delete";
            // 
            // butOK
            // 
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOK.Location = new System.Drawing.Point(404, 217);
            this.butOK.Size = new System.Drawing.Size(75, 24);
            this.butOK.TabIndex = 3;
            this.butOK.Text = "&OK";
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Location = new System.Drawing.Point(404, 247);
            this.butCancel.Size = new System.Drawing.Size(75, 24);
            this.butCancel.TabIndex = 2;
            this.butCancel.Text = "&Cancel";
            // 
            // labelGrowthBehavior
            // 
            this.labelGrowthBehavior.Location = new System.Drawing.Point(107, 72);
            this.labelGrowthBehavior.Name = "labelGrowthBehavior";
            this.labelGrowthBehavior.Size = new System.Drawing.Size(107, 16);
            this.labelGrowthBehavior.TabIndex = 113;
            this.labelGrowthBehavior.Text = "Growth Behavior";
            this.labelGrowthBehavior.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(107, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 16);
            this.label3.TabIndex = 116;
            this.label3.Text = "Grid Type";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textGridType
            // 
            this.textGridType.Enabled = false;
            this.textGridType.Location = new System.Drawing.Point(214, 45);
            this.textGridType.Name = "textGridType";
            this.textGridType.Size = new System.Drawing.Size(197, 20);
            this.textGridType.TabIndex = 127;
            // 
            // FormSheetFieldGrid
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(491, 283);
            this.Controls.Add(this.textGridType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.butDelete);
            this.Controls.Add(this.comboGrowthBehavior);
            this.Controls.Add(this.labelGrowthBehavior);
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
            this.Name = "FormSheetFieldGrid";
            this.Text = "Edit Grid";
            this.Load += new System.EventHandler(this.FormSheetFieldGrid_Load);
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
            this.Controls.SetChildIndex(this.labelGrowthBehavior, 0);
            this.Controls.SetChildIndex(this.comboGrowthBehavior, 0);
            this.Controls.SetChildIndex(this.butDelete, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.textGridType, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion

		private UI.ComboBoxPlus comboGrowthBehavior;
		private System.Windows.Forms.Label labelGrowthBehavior;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textGridType;
	}
}