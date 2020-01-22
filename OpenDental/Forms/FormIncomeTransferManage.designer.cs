namespace OpenDental{
	partial class FormIncomeTransferManage {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIncomeTransferManage));
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.gridCharges = new OpenDental.UI.ODGrid();
			this.gridSplits = new OpenDental.UI.ODGrid();
			this.butTransfer = new OpenDental.UI.Button();
			this.butClear = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.textSplitTotal = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textChargeTotal = new System.Windows.Forms.TextBox();
			this.checkIncludeHiddenSplits = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(564, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(662, 17);
			this.label3.TabIndex = 8;
			this.label3.Text = "Select positive and negative charges, then click Transfer to create transfer spli" +
    "ts from both sources.";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(7, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(548, 17);
			this.label4.TabIndex = 14;
			this.label4.Text = "Select payment splits and click the Delete Split button to remove selected splits" +
    ", or Delete All to remove all splits. ";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gridCharges
			// 
			this.gridCharges.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridCharges.HasDropDowns = true;
			this.gridCharges.Location = new System.Drawing.Point(564, 44);
			this.gridCharges.Name = "gridCharges";
			this.gridCharges.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridCharges.Size = new System.Drawing.Size(662, 580);
			this.gridCharges.TabIndex = 13;
			this.gridCharges.Title = "Income Sources";
			this.gridCharges.TranslationName = "TableOutstandingCharges";
			this.gridCharges.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCharges_CellClick);
			// 
			// gridSplits
			// 
			this.gridSplits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridSplits.Location = new System.Drawing.Point(7, 44);
			this.gridSplits.Name = "gridSplits";
			this.gridSplits.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridSplits.Size = new System.Drawing.Size(548, 580);
			this.gridSplits.TabIndex = 12;
			this.gridSplits.Title = "Current Payment Splits";
			this.gridSplits.TranslationName = "TableCurrentSplits";
			this.gridSplits.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSplits_CellClick);
			// 
			// butTransfer
			// 
			this.butTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butTransfer.Image = global::OpenDental.Properties.Resources.Left;
			this.butTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butTransfer.Location = new System.Drawing.Point(564, 631);
			this.butTransfer.Name = "butTransfer";
			this.butTransfer.Size = new System.Drawing.Size(90, 24);
			this.butTransfer.TabIndex = 18;
			this.butTransfer.Text = "Transfer";
			this.butTransfer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.butTransfer.Click += new System.EventHandler(this.butTransfer_Click);
			// 
			// butClear
			// 
			this.butClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butClear.Location = new System.Drawing.Point(124, 631);
			this.butClear.Name = "butClear";
			this.butClear.Size = new System.Drawing.Size(89, 24);
			this.butClear.TabIndex = 17;
			this.butClear.Text = "Delete All";
			this.butClear.Click += new System.EventHandler(this.butClearAll_Click);
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(7, 631);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(111, 24);
			this.butDelete.TabIndex = 16;
			this.butDelete.Text = "Delete Selected";
			this.butDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.butDelete.Click += new System.EventHandler(this.butDeleteSplit_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(1062, 660);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(1143, 660);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(443, 628);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 23);
			this.label2.TabIndex = 30;
			this.label2.Text = "Total";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSplitTotal
			// 
			this.textSplitTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textSplitTotal.Location = new System.Drawing.Point(484, 630);
			this.textSplitTotal.Name = "textSplitTotal";
			this.textSplitTotal.ReadOnly = true;
			this.textSplitTotal.Size = new System.Drawing.Size(55, 20);
			this.textSplitTotal.TabIndex = 29;
			this.textSplitTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(1113, 628);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 23);
			this.label1.TabIndex = 32;
			this.label1.Text = "Total";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textChargeTotal
			// 
			this.textChargeTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textChargeTotal.Location = new System.Drawing.Point(1154, 630);
			this.textChargeTotal.Name = "textChargeTotal";
			this.textChargeTotal.ReadOnly = true;
			this.textChargeTotal.Size = new System.Drawing.Size(55, 20);
			this.textChargeTotal.TabIndex = 31;
			this.textChargeTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// checkIncludeHiddenSplits
			// 
			this.checkIncludeHiddenSplits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkIncludeHiddenSplits.BackColor = System.Drawing.SystemColors.Control;
			this.checkIncludeHiddenSplits.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeHiddenSplits.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIncludeHiddenSplits.ForeColor = System.Drawing.SystemColors.ControlText;
			this.checkIncludeHiddenSplits.Location = new System.Drawing.Point(930, 5);
			this.checkIncludeHiddenSplits.Name = "checkIncludeHiddenSplits";
			this.checkIncludeHiddenSplits.Size = new System.Drawing.Size(296, 17);
			this.checkIncludeHiddenSplits.TabIndex = 185;
			this.checkIncludeHiddenSplits.Text = "Include hidden payment splits";
			this.checkIncludeHiddenSplits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeHiddenSplits.UseVisualStyleBackColor = false;
			this.checkIncludeHiddenSplits.Click += new System.EventHandler(this.checkIncludeHiddenSplits_Click);
			// 
			// FormIncomeTransferManage
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(1230, 696);
			this.Controls.Add(this.checkIncludeHiddenSplits);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textChargeTotal);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textSplitTotal);
			this.Controls.Add(this.butTransfer);
			this.Controls.Add(this.butClear);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.gridCharges);
			this.Controls.Add(this.gridSplits);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(1246, 734);
			this.Name = "FormIncomeTransferManage";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Income Transfer Manager";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIncomeTransferManage_FormClosing);
			this.Load += new System.EventHandler(this.FormIncomeTransferManage_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label3;
		private UI.ODGrid gridSplits;
		private UI.ODGrid gridCharges;
		private System.Windows.Forms.Label label4;
		private UI.Button butDelete;
		private UI.Button butClear;
		private UI.Button butTransfer;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textSplitTotal;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textChargeTotal;
		private System.Windows.Forms.CheckBox checkIncludeHiddenSplits;
	}
}