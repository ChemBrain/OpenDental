namespace OpenDental{
	partial class FormFamilyBalancer {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFamilyBalancer));
			this.butGetNextBatch = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkAllocateCharges = new System.Windows.Forms.CheckBox();
			this.datePicker = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.checkGuarAllocate = new System.Windows.Forms.CheckBox();
			this.label15 = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butTransfer = new OpenDental.UI.Button();
			this.labelBatchCount = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// butGetNextBatch
			// 
			this.butGetNextBatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butGetNextBatch.Location = new System.Drawing.Point(14, 352);
			this.butGetNextBatch.Name = "butGetNextBatch";
			this.butGetNextBatch.Size = new System.Drawing.Size(95, 26);
			this.butGetNextBatch.TabIndex = 148;
			this.butGetNextBatch.Text = "Get Next Batch";
			this.butGetNextBatch.Click += new System.EventHandler(this.butGetNextBatch_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.checkAllocateCharges);
			this.groupBox1.Controls.Add(this.datePicker);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.checkGuarAllocate);
			this.groupBox1.Location = new System.Drawing.Point(14, 384);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(275, 92);
			this.groupBox1.TabIndex = 143;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Income Transfer Payment Settings";
			// 
			// checkAllocateCharges
			// 
			this.checkAllocateCharges.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllocateCharges.Location = new System.Drawing.Point(6, 40);
			this.checkAllocateCharges.Name = "checkAllocateCharges";
			this.checkAllocateCharges.Size = new System.Drawing.Size(254, 24);
			this.checkAllocateCharges.TabIndex = 150;
			this.checkAllocateCharges.Text = "Attach transfers to outstanding charges";
			// 
			// datePicker
			// 
			this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.datePicker.Location = new System.Drawing.Point(6, 19);
			this.datePicker.Name = "datePicker";
			this.datePicker.Size = new System.Drawing.Size(132, 20);
			this.datePicker.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(144, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 134;
			this.label1.Text = "Transfer Split Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkGuarAllocate
			// 
			this.checkGuarAllocate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkGuarAllocate.Location = new System.Drawing.Point(6, 61);
			this.checkGuarAllocate.Name = "checkGuarAllocate";
			this.checkGuarAllocate.Size = new System.Drawing.Size(254, 24);
			this.checkGuarAllocate.TabIndex = 138;
			this.checkGuarAllocate.Text = "Transfer patient balances to family guarantor";
			this.checkGuarAllocate.Visible = false;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(12, 3);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(436, 65);
			this.label15.TabIndex = 135;
			this.label15.Text = resources.GetString("label15.Text");
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(378, 520);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 3;
			this.butCancel.Text = "Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasDropDowns = true;
			this.gridMain.Location = new System.Drawing.Point(14, 68);
			this.gridMain.Name = "gridMain";
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(433, 278);
			this.gridMain.TabIndex = 0;
			this.gridMain.Title = "Family Balance Breakdown";
			this.gridMain.TranslationName = "TableFamilyBalanceBreakdown";
			// 
			// butTransfer
			// 
			this.butTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butTransfer.Location = new System.Drawing.Point(14, 482);
			this.butTransfer.Name = "butTransfer";
			this.butTransfer.Size = new System.Drawing.Size(95, 26);
			this.butTransfer.TabIndex = 149;
			this.butTransfer.Text = "Run";
			this.butTransfer.Click += new System.EventHandler(this.butTransfer_Click);
			// 
			// labelBatchCount
			// 
			this.labelBatchCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelBatchCount.Location = new System.Drawing.Point(110, 359);
			this.labelBatchCount.Name = "labelBatchCount";
			this.labelBatchCount.Size = new System.Drawing.Size(181, 18);
			this.labelBatchCount.TabIndex = 150;
			this.labelBatchCount.Text = "Total batches: 0";
			// 
			// FormFamilyBalancer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(463, 557);
			this.Controls.Add(this.labelBatchCount);
			this.Controls.Add(this.butTransfer);
			this.Controls.Add(this.butGetNextBatch);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(479, 596);
			this.Name = "FormFamilyBalancer";
			this.Text = "Family Balancer";
			this.Load += new System.EventHandler(this.FormFamilyBalancer_Load);
			this.Shown += new System.EventHandler(this.FormFamilyBalancer_Shown);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private UI.ODGrid gridMain;
		private UI.Button butCancel;
		private System.Windows.Forms.DateTimePicker datePicker;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.CheckBox checkGuarAllocate;
		private System.Windows.Forms.GroupBox groupBox1;
		private UI.Button butGetNextBatch;
		private UI.Button butTransfer;
		private System.Windows.Forms.CheckBox checkAllocateCharges;
		private System.Windows.Forms.Label labelBatchCount;
	}
}