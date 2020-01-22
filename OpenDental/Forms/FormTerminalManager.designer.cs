﻿namespace OpenDental {
	partial class FormTerminalManager {
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

		///<summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTerminalManager));
			this.gridMain = new OpenDental.UI.ODGrid();
			this.groupBoxPassword = new System.Windows.Forms.GroupBox();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.butSave = new OpenDental.UI.Button();
			this.groupBoxPatient = new System.Windows.Forms.GroupBox();
			this.butPatForms = new OpenDental.UI.Button();
			this.labelSheets = new System.Windows.Forms.Label();
			this.labelPatient = new System.Windows.Forms.Label();
			this.listSheets = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.contrClinicPicker = new OpenDental.UI.ComboBoxClinicPicker();
			this.butClose = new OpenDental.UI.Button();
			this.groupBoxPassword.SuspendLayout();
			this.groupBoxPatient.SuspendLayout();
			this.SuspendLayout();
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(21, 92);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(715, 295);
			this.gridMain.TabIndex = 2;
			this.gridMain.Title = "Active Kiosks";
			this.gridMain.TranslationName = "TableTerminals";
			this.gridMain.OnSelectionCommitted += new System.EventHandler(this.gridMain_OnSelectionCommitted);
			// 
			// groupBoxPassword
			// 
			this.groupBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxPassword.Controls.Add(this.textPassword);
			this.groupBoxPassword.Controls.Add(this.label2);
			this.groupBoxPassword.Controls.Add(this.butSave);
			this.groupBoxPassword.Location = new System.Drawing.Point(21, 394);
			this.groupBoxPassword.Name = "groupBoxPassword";
			this.groupBoxPassword.Size = new System.Drawing.Size(482, 80);
			this.groupBoxPassword.TabIndex = 12;
			this.groupBoxPassword.TabStop = false;
			this.groupBoxPassword.Text = "Password";
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(10, 50);
			this.textPassword.Name = "textPassword";
			this.textPassword.Size = new System.Drawing.Size(129, 20);
			this.textPassword.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(7, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(327, 31);
			this.label2.TabIndex = 4;
			this.label2.Text = "To close a kiosk, go to that computer and click the hidden button at the lower ri" +
    "ght.  You will need to enter this password:";
			// 
			// butSave
			// 
			this.butSave.Location = new System.Drawing.Point(145, 48);
			this.butSave.Name = "butSave";
			this.butSave.Size = new System.Drawing.Size(97, 24);
			this.butSave.TabIndex = 6;
			this.butSave.Text = "Save Password";
			this.butSave.UseVisualStyleBackColor = true;
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			// 
			// groupBoxPatient
			// 
			this.groupBoxPatient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxPatient.Controls.Add(this.butPatForms);
			this.groupBoxPatient.Controls.Add(this.labelSheets);
			this.groupBoxPatient.Controls.Add(this.labelPatient);
			this.groupBoxPatient.Controls.Add(this.listSheets);
			this.groupBoxPatient.Location = new System.Drawing.Point(742, 90);
			this.groupBoxPatient.Name = "groupBoxPatient";
			this.groupBoxPatient.Size = new System.Drawing.Size(168, 297);
			this.groupBoxPatient.TabIndex = 11;
			this.groupBoxPatient.TabStop = false;
			this.groupBoxPatient.Text = "Current Patient";
			// 
			// butPatForms
			// 
			this.butPatForms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPatForms.Location = new System.Drawing.Point(11, 263);
			this.butPatForms.Name = "butPatForms";
			this.butPatForms.Size = new System.Drawing.Size(146, 24);
			this.butPatForms.TabIndex = 16;
			this.butPatForms.Text = "Add or Remove Forms";
			this.butPatForms.Click += new System.EventHandler(this.butPatForms_Click);
			// 
			// labelSheets
			// 
			this.labelSheets.Location = new System.Drawing.Point(11, 41);
			this.labelSheets.Name = "labelSheets";
			this.labelSheets.Size = new System.Drawing.Size(146, 17);
			this.labelSheets.TabIndex = 10;
			this.labelSheets.Text = "Forms for Kiosk";
			this.labelSheets.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelPatient
			// 
			this.labelPatient.Location = new System.Drawing.Point(11, 19);
			this.labelPatient.Name = "labelPatient";
			this.labelPatient.Size = new System.Drawing.Size(147, 18);
			this.labelPatient.TabIndex = 9;
			this.labelPatient.Text = "Fname Lname";
			// 
			// listSheets
			// 
			this.listSheets.FormattingEnabled = true;
			this.listSheets.Location = new System.Drawing.Point(11, 62);
			this.listSheets.Name = "listSheets";
			this.listSheets.Size = new System.Drawing.Size(146, 186);
			this.listSheets.TabIndex = 8;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(18, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(580, 49);
			this.label1.TabIndex = 3;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// contrClinicPicker
			// 
			this.contrClinicPicker.IncludeAll = true;
			this.contrClinicPicker.IncludeUnassigned = true;
			this.contrClinicPicker.Location = new System.Drawing.Point(90, 60);
			this.contrClinicPicker.Name = "contrClinicPicker";
			this.contrClinicPicker.Size = new System.Drawing.Size(200, 21);
			this.contrClinicPicker.TabIndex = 16;
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(835, 450);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 15;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormTerminalManager
			// 
			this.ClientSize = new System.Drawing.Size(922, 486);
			this.Controls.Add(this.contrClinicPicker);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.groupBoxPassword);
			this.Controls.Add(this.groupBoxPatient);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormTerminalManager";
			this.Text = "Kiosk Manager";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTerminalManager_FormClosing);
			this.Load += new System.EventHandler(this.FormTerminalManager_Load);
			this.groupBoxPassword.ResumeLayout(false);
			this.groupBoxPassword.PerformLayout();
			this.groupBoxPatient.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.ODGrid gridMain;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textPassword;
		private OpenDental.UI.Button butSave;
		private System.Windows.Forms.GroupBox groupBoxPatient;
		private System.Windows.Forms.ListBox listSheets;
		private System.Windows.Forms.Label labelSheets;
		private System.Windows.Forms.Label labelPatient;
		private System.Windows.Forms.GroupBox groupBoxPassword;
		private OpenDental.UI.Button butClose;
		private UI.Button butPatForms;
		private UI.ComboBoxClinicPicker contrClinicPicker;
	}
}
