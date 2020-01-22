﻿namespace OpenDental {
	partial class FormEtrans835s {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEtrans835s));
			this.imageListCalendar = new System.Windows.Forms.ImageList(this.components);
			this.smsService1 = new OpenDental.CallFireService.SMSService();
			this.listStatus = new System.Windows.Forms.ListBox();
			this.textCarrier = new OpenDental.ODtextBox();
			this.butRefresh = new OpenDental.UI.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textRangeMin = new OpenDental.ValidNum();
			this.textRangeMax = new OpenDental.ValidNum();
			this.labelDaysOldMax = new System.Windows.Forms.Label();
			this.labelDaysOldMin = new System.Windows.Forms.Label();
			this.textCheckTrace = new OpenDental.ODtextBox();
			this.butClose = new OpenDental.UI.Button();
			this.labelCheckTrace = new System.Windows.Forms.Label();
			this.labelCarrier = new System.Windows.Forms.Label();
			this.labelStatus = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.dateRangePicker = new OpenDental.UI.ODDateRangePicker();
			this.textControlId = new OpenDental.ODtextBox();
			this.labelControlId = new System.Windows.Forms.Label();
			this.comboClinics = new OpenDental.UI.ComboBoxClinicPicker();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageListCalendar
			// 
			this.imageListCalendar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListCalendar.ImageStream")));
			this.imageListCalendar.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListCalendar.Images.SetKeyName(0, "arrowDownTriangle.gif");
			this.imageListCalendar.Images.SetKeyName(1, "arrowUpTriangle.gif");
			// 
			// smsService1
			// 
			this.smsService1.Credentials = null;
			this.smsService1.Url = "https://www.callfire.com/service/SMSService";
			this.smsService1.UseDefaultCredentials = false;
			// 
			// listStatus
			// 
			this.listStatus.FormattingEnabled = true;
			this.listStatus.Location = new System.Drawing.Point(805, 8);
			this.listStatus.Name = "listStatus";
			this.listStatus.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listStatus.Size = new System.Drawing.Size(120, 56);
			this.listStatus.TabIndex = 106;
			// 
			// textCarrier
			// 
			this.textCarrier.AcceptsTab = true;
			this.textCarrier.BackColor = System.Drawing.SystemColors.Window;
			this.textCarrier.DetectLinksEnabled = false;
			this.textCarrier.DetectUrls = false;
			this.textCarrier.Location = new System.Drawing.Point(586, 8);
			this.textCarrier.Name = "textCarrier";
			this.textCarrier.QuickPasteType = OpenDentBusiness.QuickPasteType.InsPlan;
			this.textCarrier.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textCarrier.Size = new System.Drawing.Size(161, 21);
			this.textCarrier.TabIndex = 105;
			this.textCarrier.Text = "";
			this.textCarrier.Multiline=false;
			// 
			// butRefresh
			// 
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Location = new System.Drawing.Point(947, 54);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 26);
			this.butRefresh.TabIndex = 82;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textRangeMin);
			this.groupBox2.Controls.Add(this.textRangeMax);
			this.groupBox2.Controls.Add(this.labelDaysOldMax);
			this.groupBox2.Controls.Add(this.labelDaysOldMin);
			this.groupBox2.Location = new System.Drawing.Point(19, 31);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(240, 40);
			this.groupBox2.TabIndex = 104;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Amount Range (leave both blank to show all)";
			// 
			// textRangeMin
			// 
			this.textRangeMin.CausesValidation = false;
			this.textRangeMin.Location = new System.Drawing.Point(48, 15);
			this.textRangeMin.MaxVal = 50000;
			this.textRangeMin.MinVal = 0;
			this.textRangeMin.Name = "textRangeMin";
			this.textRangeMin.Size = new System.Drawing.Size(60, 20);
			this.textRangeMin.TabIndex = 8;
			// 
			// textRangeMax
			// 
			this.textRangeMax.CausesValidation = false;
			this.textRangeMax.Location = new System.Drawing.Point(149, 15);
			this.textRangeMax.MaxVal = 50000;
			this.textRangeMax.MinVal = 0;
			this.textRangeMax.Name = "textRangeMax";
			this.textRangeMax.Size = new System.Drawing.Size(60, 20);
			this.textRangeMax.TabIndex = 9;
			// 
			// labelDaysOldMax
			// 
			this.labelDaysOldMax.Location = new System.Drawing.Point(109, 16);
			this.labelDaysOldMax.Name = "labelDaysOldMax";
			this.labelDaysOldMax.Size = new System.Drawing.Size(40, 18);
			this.labelDaysOldMax.TabIndex = 99;
			this.labelDaysOldMax.Text = "Max";
			this.labelDaysOldMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDaysOldMin
			// 
			this.labelDaysOldMin.Location = new System.Drawing.Point(3, 16);
			this.labelDaysOldMin.Name = "labelDaysOldMin";
			this.labelDaysOldMin.Size = new System.Drawing.Size(45, 18);
			this.labelDaysOldMin.TabIndex = 100;
			this.labelDaysOldMin.Text = "Min";
			this.labelDaysOldMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCheckTrace
			// 
			this.textCheckTrace.AcceptsTab = true;
			this.textCheckTrace.BackColor = System.Drawing.SystemColors.Window;
			this.textCheckTrace.DetectLinksEnabled = false;
			this.textCheckTrace.DetectUrls = false;
			this.textCheckTrace.Location = new System.Drawing.Point(586, 29);
			this.textCheckTrace.Name = "textCheckTrace";
			this.textCheckTrace.QuickPasteType = OpenDentBusiness.QuickPasteType.FinancialNotes;
			this.textCheckTrace.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textCheckTrace.Size = new System.Drawing.Size(161, 21);
			this.textCheckTrace.TabIndex = 10;
			this.textCheckTrace.Text = "";
			this.textCheckTrace.Multiline=false;
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(947, 757);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 6;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// labelCheckTrace
			// 
			this.labelCheckTrace.Location = new System.Drawing.Point(425, 31);
			this.labelCheckTrace.Name = "labelCheckTrace";
			this.labelCheckTrace.Size = new System.Drawing.Size(161, 16);
			this.labelCheckTrace.TabIndex = 96;
			this.labelCheckTrace.Text = "Check# or EFT Trace#";
			this.labelCheckTrace.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelCarrier
			// 
			this.labelCarrier.Location = new System.Drawing.Point(531, 10);
			this.labelCarrier.Name = "labelCarrier";
			this.labelCarrier.Size = new System.Drawing.Size(55, 16);
			this.labelCarrier.TabIndex = 84;
			this.labelCarrier.Text = "Carrier";
			this.labelCarrier.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelStatus
			// 
			this.labelStatus.Location = new System.Drawing.Point(755, 8);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(49, 16);
			this.labelStatus.TabIndex = 86;
			this.labelStatus.Text = "Status";
			this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(19, 83);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(1003, 669);
			this.gridMain.TabIndex = 8;
			this.gridMain.Title = "ERAs";
			this.gridMain.TranslationName = "TableEtrans835s";
			this.gridMain.DoubleClick += new System.EventHandler(this.gridMain_DoubleClick);
			// 
			// dateRangePicker
			// 
			this.dateRangePicker.BackColor = System.Drawing.Color.Transparent;
			this.dateRangePicker.DefaultDateTimeFrom = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
			this.dateRangePicker.DefaultDateTimeTo = new System.DateTime(2019, 5, 20, 0, 0, 0, 0);
			this.dateRangePicker.Location = new System.Drawing.Point(24, 5);
			this.dateRangePicker.MinimumSize = new System.Drawing.Size(453, 22);
			this.dateRangePicker.Name = "dateRangePicker";
			this.dateRangePicker.Size = new System.Drawing.Size(453, 24);
			this.dateRangePicker.TabIndex = 108;
			// 
			// textControlId
			// 
			this.textControlId.AcceptsTab = true;
			this.textControlId.BackColor = System.Drawing.SystemColors.Window;
			this.textControlId.DetectLinksEnabled = false;
			this.textControlId.DetectUrls = false;
			this.textControlId.Location = new System.Drawing.Point(316, 50);
			this.textControlId.Name = "textControlId";
			this.textControlId.QuickPasteType = OpenDentBusiness.QuickPasteType.InsPlan;
			this.textControlId.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.textControlId.Size = new System.Drawing.Size(111, 21);
			this.textControlId.TabIndex = 110;
			this.textControlId.Text = "";
			// 
			// labelControlId
			// 
			this.labelControlId.Location = new System.Drawing.Point(261, 52);
			this.labelControlId.Name = "labelControlId";
			this.labelControlId.Size = new System.Drawing.Size(55, 16);
			this.labelControlId.TabIndex = 109;
			this.labelControlId.Text = "Control ID";
			this.labelControlId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClinics
			// 
			this.comboClinics.IncludeAll = true;
			this.comboClinics.IncludeUnassigned = true;
			this.comboClinics.Location = new System.Drawing.Point(549, 51);
			this.comboClinics.Name = "comboClinics";
			this.comboClinics.SelectionModeMulti = true;
			this.comboClinics.Size = new System.Drawing.Size(198, 21);
			this.comboClinics.TabIndex = 111;
			// 
			// FormEtrans835s
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1034, 788);
			this.Controls.Add(this.comboClinics);
			this.Controls.Add(this.textControlId);
			this.Controls.Add(this.labelControlId);
			this.Controls.Add(this.dateRangePicker);
			this.Controls.Add(this.listStatus);
			this.Controls.Add(this.textCarrier);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.textCheckTrace);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.labelCheckTrace);
			this.Controls.Add(this.labelCarrier);
			this.Controls.Add(this.labelStatus);
			this.MinimumSize = new System.Drawing.Size(1050, 827);
			this.Name = "FormEtrans835s";
			this.Text = "Electronic EOBs - ERA 835s";
			this.Load += new System.EventHandler(this.FormEtrans835s_Load);
			this.Shown += new System.EventHandler(this.FormEtrans835s_Shown);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private UI.Button butClose;
		private UI.Button butRefresh;
		private System.Windows.Forms.ImageList imageListCalendar;
		private System.Windows.Forms.Label labelCheckTrace;
		private System.Windows.Forms.Label labelStatus;
		private System.Windows.Forms.Label labelCarrier;
		private ODtextBox textCheckTrace;
		private System.Windows.Forms.GroupBox groupBox2;
		private ValidNum textRangeMin;
		private ValidNum textRangeMax;
		private System.Windows.Forms.Label labelDaysOldMax;
		private System.Windows.Forms.Label labelDaysOldMin;
		private ODtextBox textCarrier;
		private CallFireService.SMSService smsService1;
		private System.Windows.Forms.ListBox listStatus;
		private UI.ODGrid gridMain;
		private UI.ODDateRangePicker dateRangePicker;
		private ODtextBox textControlId;
		private System.Windows.Forms.Label labelControlId;
		private UI.ComboBoxClinicPicker comboClinics;
	}
}