using OpenDentBusiness;

namespace OpenDental {
	partial class FormReactivationSetup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReactivationSetup));
			this.gridMain = new OpenDental.UI.ODGrid();
			this.label8 = new System.Windows.Forms.Label();
			this.textPostcardsPerSheet = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.textDaysPast = new OpenDental.ValidNumber();
			this.checkGroupFamilies = new System.Windows.Forms.CheckBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.comboStatusMailedReactivation = new UI.ComboBoxPlus();
			this.label26 = new System.Windows.Forms.Label();
			this.textMaxReminders = new OpenDental.ValidNumber();
			this.label4 = new System.Windows.Forms.Label();
			this.comboStatusEmailedReactivation = new UI.ComboBoxPlus();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.comboStatusEmailTextReactivation = new UI.ComboBoxPlus();
			this.label5 = new System.Windows.Forms.Label();
			this.comboStatusTextedReactivation = new UI.ComboBoxPlus();
			this.label6 = new System.Windows.Forms.Label();
			this.labelContactInterval = new System.Windows.Forms.Label();
			this.textDaysContactInterval = new OpenDental.ValidNum();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.EditableEnterMovesDown = false;
			this.gridMain.Location = new System.Drawing.Point(16, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(935, 451);
			this.gridMain.TabIndex = 67;
			this.gridMain.Title = "Messages";
			this.gridMain.TranslationName = "TableReactivationMsgs";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label8.Location = new System.Drawing.Point(487, 552);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(176, 16);
			this.label8.TabIndex = 19;
			this.label8.Text = "Postcards per sheet (1,3,or 4)";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPostcardsPerSheet
			// 
			this.textPostcardsPerSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textPostcardsPerSheet.Location = new System.Drawing.Point(663, 549);
			this.textPostcardsPerSheet.Name = "textPostcardsPerSheet";
			this.textPostcardsPerSheet.Size = new System.Drawing.Size(34, 20);
			this.textPostcardsPerSheet.TabIndex = 18;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.textDaysPast);
			this.groupBox3.Controls.Add(this.checkGroupFamilies);
			this.groupBox3.Controls.Add(this.label14);
			this.groupBox3.Location = new System.Drawing.Point(471, 473);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(253, 70);
			this.groupBox3.TabIndex = 54;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Reactivation List Default View";
			// 
			// textDaysPast
			// 
			this.textDaysPast.Location = new System.Drawing.Point(192, 42);
			this.textDaysPast.MaxVal = 10000;
			this.textDaysPast.MinVal = 0;
			this.textDaysPast.Name = "textDaysPast";
			this.textDaysPast.Size = new System.Drawing.Size(53, 20);
			this.textDaysPast.TabIndex = 65;
			// 
			// checkGroupFamilies
			// 
			this.checkGroupFamilies.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkGroupFamilies.Location = new System.Drawing.Point(85, 18);
			this.checkGroupFamilies.Name = "checkGroupFamilies";
			this.checkGroupFamilies.Size = new System.Drawing.Size(121, 18);
			this.checkGroupFamilies.TabIndex = 49;
			this.checkGroupFamilies.Text = "Group Families";
			this.checkGroupFamilies.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkGroupFamilies.UseVisualStyleBackColor = true;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(6, 42);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(184, 20);
			this.label14.TabIndex = 50;
			this.label14.Text = "Days Past (e.g. 1095, blank, etc)";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label25
			// 
			this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label25.Location = new System.Drawing.Point(45, 473);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(157, 16);
			this.label25.TabIndex = 57;
			this.label25.Text = "Status for mailed Reactivation";
			this.label25.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboStatusMailedReactivation
			// 
			this.comboStatusMailedReactivation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboStatusMailedReactivation.Location = new System.Drawing.Point(204, 469);
			this.comboStatusMailedReactivation.Name = "comboStatusMailedReactivation";
			this.comboStatusMailedReactivation.Size = new System.Drawing.Size(261, 21);
			this.comboStatusMailedReactivation.TabIndex = 58;
			// 
			// label26
			// 
			this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label26.Location = new System.Drawing.Point(45, 496);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(157, 16);
			this.label26.TabIndex = 59;
			this.label26.Text = "Status for e-mailed Reactivation";
			this.label26.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textMaxReminders
			// 
			this.textMaxReminders.Location = new System.Drawing.Point(161, 43);
			this.textMaxReminders.MaxVal = 10000;
			this.textMaxReminders.MinVal = 0;
			this.textMaxReminders.Name = "textMaxReminders";
			this.textMaxReminders.Size = new System.Drawing.Size(53, 20);
			this.textMaxReminders.TabIndex = 68;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(13, 42);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(146, 20);
			this.label4.TabIndex = 67;
			this.label4.Text = "Max # Reminders (e.g. 4)";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboStatusEmailedReactivation
			// 
			this.comboStatusEmailedReactivation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboStatusEmailedReactivation.Location = new System.Drawing.Point(204, 492);
			this.comboStatusEmailedReactivation.Name = "comboStatusEmailedReactivation";
			this.comboStatusEmailedReactivation.Size = new System.Drawing.Size(261, 21);
			this.comboStatusEmailedReactivation.TabIndex = 60;
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(795, 549);
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
			this.butCancel.Location = new System.Drawing.Point(876, 549);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// comboStatusEmailTextReactivation
			// 
			this.comboStatusEmailTextReactivation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboStatusEmailTextReactivation.Location = new System.Drawing.Point(204, 538);
			this.comboStatusEmailTextReactivation.Name = "comboStatusEmailTextReactivation";
			this.comboStatusEmailTextReactivation.Size = new System.Drawing.Size(261, 21);
			this.comboStatusEmailTextReactivation.TabIndex = 78;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label5.Location = new System.Drawing.Point(2, 542);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(200, 16);
			this.label5.TabIndex = 77;
			this.label5.Text = "Status for e-mailed and texted Reactivation";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboStatusTextedReactivation
			// 
			this.comboStatusTextedReactivation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboStatusTextedReactivation.Location = new System.Drawing.Point(204, 515);
			this.comboStatusTextedReactivation.Name = "comboStatusTextedReactivation";
			this.comboStatusTextedReactivation.Size = new System.Drawing.Size(261, 21);
			this.comboStatusTextedReactivation.TabIndex = 76;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label6.Location = new System.Drawing.Point(45, 519);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(157, 16);
			this.label6.TabIndex = 75;
			this.label6.Text = "Status for texted Reactivation";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelContactInterval
			// 
			this.labelContactInterval.Location = new System.Drawing.Point(6, 16);
			this.labelContactInterval.Name = "labelContactInterval";
			this.labelContactInterval.Size = new System.Drawing.Size(153, 20);
			this.labelContactInterval.TabIndex = 50;
			this.labelContactInterval.Text = "Contact Interval (days)";
			this.labelContactInterval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDaysContactInterval
			// 
			this.textDaysContactInterval.Location = new System.Drawing.Point(161, 17);
			this.textDaysContactInterval.MaxVal = 10000;
			this.textDaysContactInterval.MinVal = 0;
			this.textDaysContactInterval.Name = "textDaysContactInterval";
			this.textDaysContactInterval.Size = new System.Drawing.Size(53, 20);
			this.textDaysContactInterval.TabIndex = 65;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.textMaxReminders);
			this.groupBox1.Controls.Add(this.textDaysContactInterval);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.labelContactInterval);
			this.groupBox1.Location = new System.Drawing.Point(730, 473);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(221, 70);
			this.groupBox1.TabIndex = 65;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Contact Rules";
			// 
			// FormReactivationSetup
			// 
			this.ClientSize = new System.Drawing.Size(963, 585);
			this.Controls.Add(this.comboStatusEmailTextReactivation);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.comboStatusTextedReactivation);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.textPostcardsPerSheet);
			this.Controls.Add(this.comboStatusEmailedReactivation);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.label26);
			this.Controls.Add(this.comboStatusMailedReactivation);
			this.Controls.Add(this.label25);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(990, 734);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(830, 360);
			this.Name = "FormReactivationSetup";
			this.ShowInTaskbar = false;
			this.Text = "Setup Reactivation";
			this.Load += new System.EventHandler(this.FormReactivationSetup_Load);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textPostcardsPerSheet;
		private System.Windows.Forms.CheckBox checkGroupFamilies;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label25;
		private UI.ComboBoxPlus comboStatusMailedReactivation;
		private UI.ComboBoxPlus comboStatusEmailedReactivation;
		private System.Windows.Forms.Label label26;
		private ValidNumber textDaysPast;
		private OpenDental.UI.ODGrid gridMain;
		private ValidNumber textMaxReminders;//""= infinite, 0=disabled; Must Be ValidNumber not ValidNum.
		private System.Windows.Forms.Label label4;
		private UI.ComboBoxPlus comboStatusEmailTextReactivation;
		private System.Windows.Forms.Label label5;
		private UI.ComboBoxPlus comboStatusTextedReactivation;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label labelContactInterval;
		private ValidNum textDaysContactInterval;
		private System.Windows.Forms.GroupBox groupBox1;
	}
}
