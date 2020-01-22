namespace OpenDental{
	partial class FormPaySplitSelect {
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
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPaySplitSelect));
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkShowAll = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(8, 26);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(559, 494);
			this.gridMain.TabIndex = 140;
			this.gridMain.Title = "Unallocated Paysplits";
			this.gridMain.TranslationName = "TableUnallocatedPaysplits";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(573, 462);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(573, 494);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkShowAll
			// 
			this.checkShowAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowAll.Location = new System.Drawing.Point(483, 6);
			this.checkShowAll.Name = "checkShowAll";
			this.checkShowAll.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.checkShowAll.Size = new System.Drawing.Size(84, 18);
			this.checkShowAll.TabIndex = 149;
			this.checkShowAll.Text = "Show All";
			this.checkShowAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowAll.UseVisualStyleBackColor = true;
			this.checkShowAll.CheckedChanged += new System.EventHandler(this.checkShowAll_CheckedChanged);
			// 
			// FormPaySplitSelect
			// 
			this.ClientSize = new System.Drawing.Size(660, 532);
			this.Controls.Add(this.checkShowAll);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPaySplitSelect";
			this.ShowInTaskbar = false;
			this.Text = "Select Paysplit";
			this.Load += new System.EventHandler(this.FormPaySplitSelect_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.CheckBox checkShowAll;
	}
}