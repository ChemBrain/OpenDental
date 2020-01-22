namespace OpenDental{
	partial class FormErxSetup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormErxSetup));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkEnabled = new System.Windows.Forms.CheckBox();
			this.groupErxOptions = new System.Windows.Forms.GroupBox();
			this.radioDoseSpot = new System.Windows.Forms.RadioButton();
			this.radioNewCrop = new System.Windows.Forms.RadioButton();
			this.tabControlErxSoftware = new System.Windows.Forms.TabControl();
			this.tabNewCrop = new System.Windows.Forms.TabPage();
			this.textNewCropAccountID = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.tabDoseSpot = new System.Windows.Forms.TabPage();
			this.checkShowHiddenClinics = new System.Windows.Forms.CheckBox();
			this.gridProperties = new OpenDental.UI.ODGrid();
			this.radioDoseSpotLegacy = new System.Windows.Forms.RadioButton();
			this.groupErxOptions.SuspendLayout();
			this.tabControlErxSoftware.SuspendLayout();
			this.tabNewCrop.SuspendLayout();
			this.tabDoseSpot.SuspendLayout();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(376, 342);
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
			this.butCancel.Location = new System.Drawing.Point(457, 342);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkEnabled
			// 
			this.checkEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEnabled.Location = new System.Drawing.Point(23, 4);
			this.checkEnabled.Name = "checkEnabled";
			this.checkEnabled.Size = new System.Drawing.Size(98, 18);
			this.checkEnabled.TabIndex = 42;
			this.checkEnabled.Text = "Enabled";
			// 
			// groupErxOptions
			// 
			this.groupErxOptions.Controls.Add(this.radioDoseSpotLegacy);
			this.groupErxOptions.Controls.Add(this.radioDoseSpot);
			this.groupErxOptions.Controls.Add(this.radioNewCrop);
			this.groupErxOptions.Location = new System.Drawing.Point(23, 25);
			this.groupErxOptions.Name = "groupErxOptions";
			this.groupErxOptions.Size = new System.Drawing.Size(171, 80);
			this.groupErxOptions.TabIndex = 43;
			this.groupErxOptions.TabStop = false;
			this.groupErxOptions.Text = "eRx Solution";
			// 
			// radioDoseSpot
			// 
			this.radioDoseSpot.Location = new System.Drawing.Point(16, 36);
			this.radioDoseSpot.Name = "radioDoseSpot";
			this.radioDoseSpot.Size = new System.Drawing.Size(96, 17);
			this.radioDoseSpot.TabIndex = 1;
			this.radioDoseSpot.Text = "DoseSpot";
			this.radioDoseSpot.UseVisualStyleBackColor = true;
			this.radioDoseSpot.Click += new System.EventHandler(this.radioDoseSpot_Click);
			// 
			// radioNewCrop
			// 
			this.radioNewCrop.Checked = true;
			this.radioNewCrop.Location = new System.Drawing.Point(16, 17);
			this.radioNewCrop.Name = "radioNewCrop";
			this.radioNewCrop.Size = new System.Drawing.Size(96, 17);
			this.radioNewCrop.TabIndex = 0;
			this.radioNewCrop.TabStop = true;
			this.radioNewCrop.Text = "Legacy";
			this.radioNewCrop.UseVisualStyleBackColor = true;
			this.radioNewCrop.Click += new System.EventHandler(this.radioNewCrop_Click);
			// 
			// tabControlErxSoftware
			// 
			this.tabControlErxSoftware.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlErxSoftware.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabControlErxSoftware.Controls.Add(this.tabNewCrop);
			this.tabControlErxSoftware.Controls.Add(this.tabDoseSpot);
			this.tabControlErxSoftware.ItemSize = new System.Drawing.Size(100, 10);
			this.tabControlErxSoftware.Location = new System.Drawing.Point(12, 111);
			this.tabControlErxSoftware.Name = "tabControlErxSoftware";
			this.tabControlErxSoftware.SelectedIndex = 0;
			this.tabControlErxSoftware.Size = new System.Drawing.Size(527, 225);
			this.tabControlErxSoftware.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabControlErxSoftware.TabIndex = 44;
			// 
			// tabNewCrop
			// 
			this.tabNewCrop.Controls.Add(this.textNewCropAccountID);
			this.tabNewCrop.Controls.Add(this.label7);
			this.tabNewCrop.Location = new System.Drawing.Point(4, 14);
			this.tabNewCrop.Name = "tabNewCrop";
			this.tabNewCrop.Padding = new System.Windows.Forms.Padding(3);
			this.tabNewCrop.Size = new System.Drawing.Size(519, 207);
			this.tabNewCrop.TabIndex = 0;
			this.tabNewCrop.Text = "Basic";
			this.tabNewCrop.UseVisualStyleBackColor = true;
			// 
			// textNewCropAccountID
			// 
			this.textNewCropAccountID.Location = new System.Drawing.Point(170, 6);
			this.textNewCropAccountID.Name = "textNewCropAccountID";
			this.textNewCropAccountID.ReadOnly = true;
			this.textNewCropAccountID.Size = new System.Drawing.Size(200, 20);
			this.textNewCropAccountID.TabIndex = 60;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 7);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(153, 18);
			this.label7.TabIndex = 59;
			this.label7.Text = "Account ID";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabDoseSpot
			// 
			this.tabDoseSpot.Controls.Add(this.checkShowHiddenClinics);
			this.tabDoseSpot.Controls.Add(this.gridProperties);
			this.tabDoseSpot.Location = new System.Drawing.Point(4, 14);
			this.tabDoseSpot.Name = "tabDoseSpot";
			this.tabDoseSpot.Padding = new System.Windows.Forms.Padding(3);
			this.tabDoseSpot.Size = new System.Drawing.Size(519, 207);
			this.tabDoseSpot.TabIndex = 1;
			this.tabDoseSpot.Text = "DoseSpot";
			this.tabDoseSpot.UseVisualStyleBackColor = true;
			// 
			// checkShowHiddenClinics
			// 
			this.checkShowHiddenClinics.AutoSize = true;
			this.checkShowHiddenClinics.Location = new System.Drawing.Point(7, 6);
			this.checkShowHiddenClinics.Name = "checkShowHiddenClinics";
			this.checkShowHiddenClinics.Size = new System.Drawing.Size(123, 17);
			this.checkShowHiddenClinics.TabIndex = 6;
			this.checkShowHiddenClinics.Text = "Show Hidden Clinics";
			this.checkShowHiddenClinics.UseVisualStyleBackColor = true;
			this.checkShowHiddenClinics.CheckedChanged += new System.EventHandler(this.checkShowHiddenClinics_CheckedChanged);
			// 
			// gridProperties
			// 
			this.gridProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridProperties.Location = new System.Drawing.Point(6, 26);
			this.gridProperties.Name = "gridProperties";
			this.gridProperties.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridProperties.Size = new System.Drawing.Size(510, 174);
			this.gridProperties.TabIndex = 5;
			this.gridProperties.Title = "Properties";
			this.gridProperties.TranslationName = "GridProperties";
			this.gridProperties.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridProperties_CellDoubleClick);
			// 
			// radioDoseSpotLegacy
			// 
			this.radioDoseSpotLegacy.Location = new System.Drawing.Point(16, 55);
			this.radioDoseSpotLegacy.Name = "radioDoseSpotLegacy";
			this.radioDoseSpotLegacy.Size = new System.Drawing.Size(149, 17);
			this.radioDoseSpotLegacy.TabIndex = 2;
			this.radioDoseSpotLegacy.Text = "DoseSpot with Legacy";
			this.radioDoseSpotLegacy.UseVisualStyleBackColor = true;
			this.radioDoseSpotLegacy.Click += new System.EventHandler(this.radioDoseSpotLegacy_Click);
			// 
			// FormErxSetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(551, 378);
			this.Controls.Add(this.tabControlErxSoftware);
			this.Controls.Add(this.groupErxOptions);
			this.Controls.Add(this.checkEnabled);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(567, 417);
			this.Name = "FormErxSetup";
			this.Text = "Erx Setup";
			this.Load += new System.EventHandler(this.FormErxSetup_Load);
			this.groupErxOptions.ResumeLayout(false);
			this.tabControlErxSoftware.ResumeLayout(false);
			this.tabNewCrop.ResumeLayout(false);
			this.tabNewCrop.PerformLayout();
			this.tabDoseSpot.ResumeLayout(false);
			this.tabDoseSpot.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.CheckBox checkEnabled;
		private System.Windows.Forms.GroupBox groupErxOptions;
		private System.Windows.Forms.RadioButton radioDoseSpot;
		private System.Windows.Forms.RadioButton radioNewCrop;
		private System.Windows.Forms.TabControl tabControlErxSoftware;
		private System.Windows.Forms.TabPage tabNewCrop;
		private System.Windows.Forms.TabPage tabDoseSpot;
		private System.Windows.Forms.TextBox textNewCropAccountID;
		private System.Windows.Forms.Label label7;
		private UI.ODGrid gridProperties;
		private System.Windows.Forms.CheckBox checkShowHiddenClinics;
		private System.Windows.Forms.RadioButton radioDoseSpotLegacy;
	}
}