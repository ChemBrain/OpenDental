namespace OpenDental {
	partial class FormPatientStatusTool {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing&&(components!=null)) {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientStatusTool));
			this.butCancel = new OpenDental.UI.Button();
			this.groupCriteria = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.radioActiveWithout = new System.Windows.Forms.RadioButton();
			this.radioInactiveWith = new System.Windows.Forms.RadioButton();
			this.comboClinic = new OpenDental.UI.ComboBoxClinicPicker();
			this.listOptions = new System.Windows.Forms.ListBox();
			this.odDatePickerSince = new OpenDental.UI.ODDatePicker();
			this.butSelectAll = new OpenDental.UI.Button();
			this.butDeselectAll = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butCreateList = new OpenDental.UI.Button();
			this.butRun = new OpenDental.UI.Button();
			this.groupCriteria.SuspendLayout();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(716, 529);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// groupCriteria
			// 
			this.groupCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupCriteria.Controls.Add(this.label1);
			this.groupCriteria.Controls.Add(this.radioActiveWithout);
			this.groupCriteria.Controls.Add(this.radioInactiveWith);
			this.groupCriteria.Controls.Add(this.comboClinic);
			this.groupCriteria.Controls.Add(this.listOptions);
			this.groupCriteria.Controls.Add(this.odDatePickerSince);
			this.groupCriteria.Location = new System.Drawing.Point(19, 12);
			this.groupCriteria.Name = "groupCriteria";
			this.groupCriteria.Size = new System.Drawing.Size(691, 82);
			this.groupCriteria.TabIndex = 83;
			this.groupCriteria.TabStop = false;
			this.groupCriteria.Text = "Criteria";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(318, 35);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 16);
			this.label1.TabIndex = 86;
			this.label1.Text = "Since";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// radioActiveWithout
			// 
			this.radioActiveWithout.Checked = true;
			this.radioActiveWithout.Location = new System.Drawing.Point(21, 23);
			this.radioActiveWithout.Name = "radioActiveWithout";
			this.radioActiveWithout.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioActiveWithout.Size = new System.Drawing.Size(139, 17);
			this.radioActiveWithout.TabIndex = 80;
			this.radioActiveWithout.TabStop = true;
			this.radioActiveWithout.Text = "Active patient without";
			this.radioActiveWithout.UseVisualStyleBackColor = true;
			// 
			// radioInactiveWith
			// 
			this.radioInactiveWith.Location = new System.Drawing.Point(21, 46);
			this.radioInactiveWith.Name = "radioInactiveWith";
			this.radioInactiveWith.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioInactiveWith.Size = new System.Drawing.Size(139, 17);
			this.radioInactiveWith.TabIndex = 79;
			this.radioInactiveWith.Text = "Inactive patients with";
			this.radioInactiveWith.UseVisualStyleBackColor = true;
			// 
			// comboClinic
			// 
			this.comboClinic.IncludeAll = true;
			this.comboClinic.IncludeUnassigned = true;
			this.comboClinic.Location = new System.Drawing.Point(485, 32);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(197, 21);
			this.comboClinic.TabIndex = 83;
			// 
			// listOptions
			// 
			this.listOptions.FormattingEnabled = true;
			this.listOptions.Location = new System.Drawing.Point(175, 19);
			this.listOptions.Name = "listOptions";
			this.listOptions.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listOptions.Size = new System.Drawing.Size(120, 56);
			this.listOptions.TabIndex = 77;
			// 
			// odDatePickerSince
			// 
			this.odDatePickerSince.BackColor = System.Drawing.Color.Transparent;
			this.odDatePickerSince.DefaultDateTime = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
			this.odDatePickerSince.Location = new System.Drawing.Point(301, 32);
			this.odDatePickerSince.MaximumSize = new System.Drawing.Size(0, 184);
			this.odDatePickerSince.MinimumSize = new System.Drawing.Size(227, 23);
			this.odDatePickerSince.Name = "odDatePickerSince";
			this.odDatePickerSince.Size = new System.Drawing.Size(227, 23);
			this.odDatePickerSince.TabIndex = 85;
			// 
			// butSelectAll
			// 
			this.butSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSelectAll.Location = new System.Drawing.Point(19, 527);
			this.butSelectAll.Name = "butSelectAll";
			this.butSelectAll.Size = new System.Drawing.Size(83, 26);
			this.butSelectAll.TabIndex = 76;
			this.butSelectAll.Text = "Select All";
			this.butSelectAll.UseVisualStyleBackColor = true;
			this.butSelectAll.Click += new System.EventHandler(this.butSelectAll_Click);
			// 
			// butDeselectAll
			// 
			this.butDeselectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDeselectAll.Location = new System.Drawing.Point(108, 527);
			this.butDeselectAll.Name = "butDeselectAll";
			this.butDeselectAll.Size = new System.Drawing.Size(82, 26);
			this.butDeselectAll.TabIndex = 75;
			this.butDeselectAll.Text = "Deselect All";
			this.butDeselectAll.UseVisualStyleBackColor = true;
			this.butDeselectAll.Click += new System.EventHandler(this.butDeselectAll_Click);
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(19, 100);
			this.gridMain.Name = "gridMain";
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(772, 421);
			this.gridMain.TabIndex = 6;
			this.gridMain.Title = "Patients to Convert";
			this.gridMain.TranslationName = "Patients to Convert";
			// 
			// butCreateList
			// 
			this.butCreateList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butCreateList.Location = new System.Drawing.Point(716, 68);
			this.butCreateList.Name = "butCreateList";
			this.butCreateList.Size = new System.Drawing.Size(75, 26);
			this.butCreateList.TabIndex = 74;
			this.butCreateList.Text = "Create List";
			this.butCreateList.UseVisualStyleBackColor = true;
			this.butCreateList.Click += new System.EventHandler(this.butCreateList_Click);
			// 
			// butRun
			// 
			this.butRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butRun.Location = new System.Drawing.Point(628, 529);
			this.butRun.Name = "butRun";
			this.butRun.Size = new System.Drawing.Size(75, 24);
			this.butRun.TabIndex = 3;
			this.butRun.Text = "&Run";
			this.butRun.Click += new System.EventHandler(this.butRun_Click);
			// 
			// FormPatientStatusTool
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(803, 565);
			this.Controls.Add(this.groupCriteria);
			this.Controls.Add(this.butSelectAll);
			this.Controls.Add(this.butDeselectAll);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butCreateList);
			this.Controls.Add(this.butRun);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(0, 0);
			this.Name = "FormPatientStatusTool";
			this.Text = "Patient Status Setter";
			this.Load += new System.EventHandler(this.FormPatientStatusTool_Load);
			this.groupCriteria.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butRun;
		private OpenDental.UI.Button butCancel;
		private UI.ODGrid gridMain;
		private UI.Button butCreateList;
		private UI.Button butDeselectAll;
		private UI.Button butSelectAll;
		private System.Windows.Forms.ListBox listOptions;
		private System.Windows.Forms.GroupBox groupCriteria;
		private UI.ComboBoxClinicPicker comboClinic;
		private System.Windows.Forms.RadioButton radioActiveWithout;
		private System.Windows.Forms.RadioButton radioInactiveWith;
		private System.Windows.Forms.Label label1;
		private UI.ODDatePicker odDatePickerSince;
	}
}