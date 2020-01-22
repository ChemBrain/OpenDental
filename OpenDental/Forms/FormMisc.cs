using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	///<summary></summary>
	public class FormMisc : ODForm {
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private IContainer components;
		private System.Windows.Forms.TextBox textMainWindowTitle;
		private System.Windows.Forms.Label label3;
		private OpenDental.ValidNumber textSigInterval;
		private OpenDental.UI.Button butLanguages;
		private Label label4;
		private ToolTip toolTip1;
		private ComboBox comboShowID;
		private Label label15;
		private Label label17;
		private GroupBox groupBox6;
		private CheckBox checkTitleBarShowSite;
		private TextBox textWebServiceServerName;
		private Label label2;
		private ValidNumber textInactiveSignal;
		private Label label5;
		private CheckBox checkPrefFName;
		private CheckBox checkRefresh;
		private CheckBox checkImeCompositionCompatibility;
		private TextBox textLanguageAndRegion;
		private Label label6;
		private UI.Button butPickLanguageAndRegion;
    private CheckBox checkTimeCardUseLocal;
		private ComboBox comboTrackClinic;
		private Label labelTrackClinic;
		private Label label1;
		private UI.Button butDecimal;
		private TextBox textNumDecimals;
		private Label label7;
		private TextBox textSyncCode;
		private Label label8;
		private UI.Button butClearCode;
		private Label label9;
		private ValidNum textAuditEntries;
		private CheckBox checkAgingIsEnterprise;
		private UI.Button butClearAgingBeginDateT;
		private Label labelAgingBeginDateT;
		private TextBox textAgingBeginDateT;
		private Label labelAutoAgingRunTime;
		private TextBox textAutoAgingRunTime;
		private CheckBox checkUseClinicAbbr;
		private CheckBox checkSubmitExceptions;
		private CheckBox checkTitleBarShowSpecialty;
		private ComboBox comboTheme;
		private Label label10;
		private CheckBox checkUserTheme;
		private GroupBox groupSelectPatient;
		private CheckBox checkPatientSelectFilterRestrictedClinics;
		private List<string> _trackLastClinicBy;
		//private List<Def> posAdjTypes;

		///<summary></summary>
		public FormMisc(){
			InitializeComponent();
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMisc));
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.textAuditEntries = new OpenDental.ValidNum();
			this.label9 = new System.Windows.Forms.Label();
			this.butClearCode = new OpenDental.UI.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.textSyncCode = new System.Windows.Forms.TextBox();
			this.butDecimal = new OpenDental.UI.Button();
			this.textNumDecimals = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.comboTrackClinic = new System.Windows.Forms.ComboBox();
			this.labelTrackClinic = new System.Windows.Forms.Label();
			this.checkTimeCardUseLocal = new System.Windows.Forms.CheckBox();
			this.butPickLanguageAndRegion = new OpenDental.UI.Button();
			this.textLanguageAndRegion = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.checkImeCompositionCompatibility = new System.Windows.Forms.CheckBox();
			this.checkRefresh = new System.Windows.Forms.CheckBox();
			this.checkPrefFName = new System.Windows.Forms.CheckBox();
			this.textInactiveSignal = new OpenDental.ValidNumber();
			this.label5 = new System.Windows.Forms.Label();
			this.textWebServiceServerName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.checkTitleBarShowSpecialty = new System.Windows.Forms.CheckBox();
			this.checkUseClinicAbbr = new System.Windows.Forms.CheckBox();
			this.textMainWindowTitle = new System.Windows.Forms.TextBox();
			this.checkTitleBarShowSite = new System.Windows.Forms.CheckBox();
			this.label15 = new System.Windows.Forms.Label();
			this.comboShowID = new System.Windows.Forms.ComboBox();
			this.label17 = new System.Windows.Forms.Label();
			this.groupSelectPatient = new System.Windows.Forms.GroupBox();
			this.checkPatientSelectFilterRestrictedClinics = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.butLanguages = new OpenDental.UI.Button();
			this.textSigInterval = new OpenDental.ValidNumber();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.checkAgingIsEnterprise = new System.Windows.Forms.CheckBox();
			this.butClearAgingBeginDateT = new OpenDental.UI.Button();
			this.labelAgingBeginDateT = new System.Windows.Forms.Label();
			this.textAgingBeginDateT = new System.Windows.Forms.TextBox();
			this.labelAutoAgingRunTime = new System.Windows.Forms.Label();
			this.textAutoAgingRunTime = new System.Windows.Forms.TextBox();
			this.checkSubmitExceptions = new System.Windows.Forms.CheckBox();
			this.comboTheme = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.checkUserTheme = new System.Windows.Forms.CheckBox();
			this.groupBox6.SuspendLayout();
			this.groupSelectPatient.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolTip1
			// 
			this.toolTip1.AutomaticDelay = 0;
			this.toolTip1.AutoPopDelay = 600000;
			this.toolTip1.InitialDelay = 0;
			this.toolTip1.IsBalloon = true;
			this.toolTip1.ReshowDelay = 0;
			this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.toolTip1.ToolTipTitle = "Help";
			// 
			// textAuditEntries
			// 
			this.textAuditEntries.Location = new System.Drawing.Point(375, 409);
			this.textAuditEntries.MaxLength = 5;
			this.textAuditEntries.MaxVal = 10000;
			this.textAuditEntries.MinVal = 1;
			this.textAuditEntries.Name = "textAuditEntries";
			this.textAuditEntries.Size = new System.Drawing.Size(74, 20);
			this.textAuditEntries.TabIndex = 225;
			this.textAuditEntries.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(6, 410);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(367, 17);
			this.label9.TabIndex = 224;
			this.label9.Text = "Number of Audit Trail entries displayed";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butClearCode
			// 
			this.butClearCode.Location = new System.Drawing.Point(455, 386);
			this.butClearCode.Name = "butClearCode";
			this.butClearCode.Size = new System.Drawing.Size(43, 21);
			this.butClearCode.TabIndex = 222;
			this.butClearCode.Text = "Clear";
			this.butClearCode.Click += new System.EventHandler(this.butClearCode_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(6, 387);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(367, 17);
			this.label8.TabIndex = 221;
			this.label8.Text = "Sync code for CEMT.  Clear if you want to sync from a different source";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSyncCode
			// 
			this.textSyncCode.Location = new System.Drawing.Point(375, 386);
			this.textSyncCode.Name = "textSyncCode";
			this.textSyncCode.ReadOnly = true;
			this.textSyncCode.Size = new System.Drawing.Size(74, 20);
			this.textSyncCode.TabIndex = 220;
			// 
			// butDecimal
			// 
			this.butDecimal.Location = new System.Drawing.Point(455, 296);
			this.butDecimal.Name = "butDecimal";
			this.butDecimal.Size = new System.Drawing.Size(23, 21);
			this.butDecimal.TabIndex = 219;
			this.butDecimal.Text = "...";
			this.butDecimal.Click += new System.EventHandler(this.butDecimal_Click);
			// 
			// textNumDecimals
			// 
			this.textNumDecimals.Location = new System.Drawing.Point(402, 297);
			this.textNumDecimals.Name = "textNumDecimals";
			this.textNumDecimals.ReadOnly = true;
			this.textNumDecimals.Size = new System.Drawing.Size(47, 20);
			this.textNumDecimals.TabIndex = 217;
			this.textNumDecimals.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(6, 298);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(394, 17);
			this.label7.TabIndex = 218;
			this.label7.Text = "Currency number of digits after decimal";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTrackClinic
			// 
			this.comboTrackClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTrackClinic.FormattingEnabled = true;
			this.comboTrackClinic.Location = new System.Drawing.Point(319, 361);
			this.comboTrackClinic.Name = "comboTrackClinic";
			this.comboTrackClinic.Size = new System.Drawing.Size(130, 21);
			this.comboTrackClinic.TabIndex = 215;
			// 
			// labelTrackClinic
			// 
			this.labelTrackClinic.Location = new System.Drawing.Point(6, 364);
			this.labelTrackClinic.Name = "labelTrackClinic";
			this.labelTrackClinic.Size = new System.Drawing.Size(311, 17);
			this.labelTrackClinic.TabIndex = 216;
			this.labelTrackClinic.Text = "Track Last Clinic By";
			this.labelTrackClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkTimeCardUseLocal
			// 
			this.checkTimeCardUseLocal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTimeCardUseLocal.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTimeCardUseLocal.Location = new System.Drawing.Point(6, 176);
			this.checkTimeCardUseLocal.Name = "checkTimeCardUseLocal";
			this.checkTimeCardUseLocal.Size = new System.Drawing.Size(443, 17);
			this.checkTimeCardUseLocal.TabIndex = 214;
			this.checkTimeCardUseLocal.Text = "Time Cards use local time";
			this.checkTimeCardUseLocal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPickLanguageAndRegion
			// 
			this.butPickLanguageAndRegion.Location = new System.Drawing.Point(455, 274);
			this.butPickLanguageAndRegion.Name = "butPickLanguageAndRegion";
			this.butPickLanguageAndRegion.Size = new System.Drawing.Size(23, 21);
			this.butPickLanguageAndRegion.TabIndex = 206;
			this.butPickLanguageAndRegion.Text = "...";
			this.butPickLanguageAndRegion.Click += new System.EventHandler(this.butPickLanguageAndRegion_Click);
			// 
			// textLanguageAndRegion
			// 
			this.textLanguageAndRegion.Location = new System.Drawing.Point(284, 275);
			this.textLanguageAndRegion.Name = "textLanguageAndRegion";
			this.textLanguageAndRegion.ReadOnly = true;
			this.textLanguageAndRegion.Size = new System.Drawing.Size(165, 20);
			this.textLanguageAndRegion.TabIndex = 204;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(6, 273);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(276, 17);
			this.label6.TabIndex = 205;
			this.label6.Text = "Language and region used by program";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkImeCompositionCompatibility
			// 
			this.checkImeCompositionCompatibility.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkImeCompositionCompatibility.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkImeCompositionCompatibility.Location = new System.Drawing.Point(6, 320);
			this.checkImeCompositionCompatibility.Name = "checkImeCompositionCompatibility";
			this.checkImeCompositionCompatibility.Size = new System.Drawing.Size(443, 17);
			this.checkImeCompositionCompatibility.TabIndex = 203;
			this.checkImeCompositionCompatibility.Text = "Text boxes use foreign language Input Method Editor (IME) composition";
			this.checkImeCompositionCompatibility.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkRefresh
			// 
			this.checkRefresh.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRefresh.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRefresh.Location = new System.Drawing.Point(55, 13);
			this.checkRefresh.Name = "checkRefresh";
			this.checkRefresh.Size = new System.Drawing.Size(382, 17);
			this.checkRefresh.TabIndex = 202;
			this.checkRefresh.Text = "New Computers default to refresh while typing in Select Patient window";
			this.checkRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRefresh.UseVisualStyleBackColor = true;
			// 
			// checkPrefFName
			// 
			this.checkPrefFName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPrefFName.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPrefFName.Location = new System.Drawing.Point(55, 30);
			this.checkPrefFName.Name = "checkPrefFName";
			this.checkPrefFName.Size = new System.Drawing.Size(382, 17);
			this.checkPrefFName.TabIndex = 79;
			this.checkPrefFName.Text = "Search for preferred name in first name field in Select Patient window";
			this.checkPrefFName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInactiveSignal
			// 
			this.textInactiveSignal.Location = new System.Drawing.Point(375, 225);
			this.textInactiveSignal.MaxVal = 1000000;
			this.textInactiveSignal.MinVal = 1;
			this.textInactiveSignal.Name = "textInactiveSignal";
			this.textInactiveSignal.Size = new System.Drawing.Size(74, 20);
			this.textInactiveSignal.TabIndex = 201;
			this.textInactiveSignal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(6, 221);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(367, 27);
			this.label5.TabIndex = 200;
			this.label5.Text = "Disable signal interval after this many minutes of user inactivity\r\nLeave blank t" +
    "o disable";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWebServiceServerName
			// 
			this.textWebServiceServerName.Location = new System.Drawing.Point(284, 339);
			this.textWebServiceServerName.Name = "textWebServiceServerName";
			this.textWebServiceServerName.Size = new System.Drawing.Size(165, 20);
			this.textWebServiceServerName.TabIndex = 197;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 340);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(276, 17);
			this.label2.TabIndex = 198;
			this.label2.Text = "Update Server Name";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(453, 17);
			this.label1.TabIndex = 196;
			this.label1.Text = "See Setup | Module Preferences for setup options that were previously in this win" +
    "dow.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.checkTitleBarShowSpecialty);
			this.groupBox6.Controls.Add(this.checkUseClinicAbbr);
			this.groupBox6.Controls.Add(this.textMainWindowTitle);
			this.groupBox6.Controls.Add(this.checkTitleBarShowSite);
			this.groupBox6.Controls.Add(this.label15);
			this.groupBox6.Controls.Add(this.comboShowID);
			this.groupBox6.Controls.Add(this.label17);
			this.groupBox6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox6.Location = new System.Drawing.Point(12, 24);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(453, 113);
			this.groupBox6.TabIndex = 195;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Main Window Title";
			// 
			// checkTitleBarShowSpecialty
			// 
			this.checkTitleBarShowSpecialty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTitleBarShowSpecialty.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTitleBarShowSpecialty.Location = new System.Drawing.Point(6, 60);
			this.checkTitleBarShowSpecialty.Name = "checkTitleBarShowSpecialty";
			this.checkTitleBarShowSpecialty.Size = new System.Drawing.Size(431, 17);
			this.checkTitleBarShowSpecialty.TabIndex = 234;
			this.checkTitleBarShowSpecialty.Text = "Show patient specialty in main title bar and account patient select";
			this.checkTitleBarShowSpecialty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkUseClinicAbbr
			// 
			this.checkUseClinicAbbr.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseClinicAbbr.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUseClinicAbbr.Location = new System.Drawing.Point(6, 94);
			this.checkUseClinicAbbr.Name = "checkUseClinicAbbr";
			this.checkUseClinicAbbr.Size = new System.Drawing.Size(431, 17);
			this.checkUseClinicAbbr.TabIndex = 233;
			this.checkUseClinicAbbr.Text = "Use clinic abbreviation in main title bar (clinics must be turned on)";
			this.checkUseClinicAbbr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMainWindowTitle
			// 
			this.textMainWindowTitle.Location = new System.Drawing.Point(170, 13);
			this.textMainWindowTitle.Name = "textMainWindowTitle";
			this.textMainWindowTitle.Size = new System.Drawing.Size(267, 20);
			this.textMainWindowTitle.TabIndex = 38;
			// 
			// checkTitleBarShowSite
			// 
			this.checkTitleBarShowSite.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTitleBarShowSite.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTitleBarShowSite.Location = new System.Drawing.Point(6, 77);
			this.checkTitleBarShowSite.Name = "checkTitleBarShowSite";
			this.checkTitleBarShowSite.Size = new System.Drawing.Size(431, 17);
			this.checkTitleBarShowSite.TabIndex = 74;
			this.checkTitleBarShowSite.Text = "Show Site (public health must also be turned on)";
			this.checkTitleBarShowSite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(6, 14);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(163, 17);
			this.label15.TabIndex = 39;
			this.label15.Text = "Title Text";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboShowID
			// 
			this.comboShowID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboShowID.FormattingEnabled = true;
			this.comboShowID.Location = new System.Drawing.Point(307, 35);
			this.comboShowID.Name = "comboShowID";
			this.comboShowID.Size = new System.Drawing.Size(130, 21);
			this.comboShowID.TabIndex = 72;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(6, 37);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(299, 17);
			this.label17.TabIndex = 73;
			this.label17.Text = "Show ID in title bar";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupSelectPatient
			// 
			this.groupSelectPatient.Controls.Add(this.checkPatientSelectFilterRestrictedClinics);
			this.groupSelectPatient.Controls.Add(this.checkRefresh);
			this.groupSelectPatient.Controls.Add(this.checkPrefFName);
			this.groupSelectPatient.Location = new System.Drawing.Point(12, 514);
			this.groupSelectPatient.Name = "groupSelectPatient";
			this.groupSelectPatient.Size = new System.Drawing.Size(453, 69);
			this.groupSelectPatient.TabIndex = 235;
			this.groupSelectPatient.TabStop = false;
			this.groupSelectPatient.Text = "Select Patient Window";
			// 
			// checkPatientSelectFilterRestrictedClinics
			// 
			this.checkPatientSelectFilterRestrictedClinics.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPatientSelectFilterRestrictedClinics.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPatientSelectFilterRestrictedClinics.Location = new System.Drawing.Point(55, 46);
			this.checkPatientSelectFilterRestrictedClinics.Name = "checkPatientSelectFilterRestrictedClinics";
			this.checkPatientSelectFilterRestrictedClinics.Size = new System.Drawing.Size(382, 18);
			this.checkPatientSelectFilterRestrictedClinics.TabIndex = 291;
			this.checkPatientSelectFilterRestrictedClinics.Text = "Hide patients from restricted clinics when viewing \"All\" clinics";
			this.checkPatientSelectFilterRestrictedClinics.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPatientSelectFilterRestrictedClinics.Visible = false;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 252);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(352, 17);
			this.label4.TabIndex = 64;
			this.label4.Text = "Languages used by patients";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butLanguages
			// 
			this.butLanguages.Location = new System.Drawing.Point(360, 249);
			this.butLanguages.Name = "butLanguages";
			this.butLanguages.Size = new System.Drawing.Size(88, 24);
			this.butLanguages.TabIndex = 63;
			this.butLanguages.Text = "Edit Languages";
			this.butLanguages.Click += new System.EventHandler(this.butLanguages_Click);
			// 
			// textSigInterval
			// 
			this.textSigInterval.Location = new System.Drawing.Point(375, 195);
			this.textSigInterval.MaxVal = 1000000;
			this.textSigInterval.MinVal = 1;
			this.textSigInterval.Name = "textSigInterval";
			this.textSigInterval.Size = new System.Drawing.Size(74, 20);
			this.textSigInterval.TabIndex = 57;
			this.textSigInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(455, 614);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 8;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(374, 614);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 7;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 192);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(367, 27);
			this.label3.TabIndex = 56;
			this.label3.Text = "Process signal interval in seconds.  Usually every 6 to 20 seconds\r\nLeave blank t" +
    "o disable autorefresh";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAgingIsEnterprise
			// 
			this.checkAgingIsEnterprise.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAgingIsEnterprise.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAgingIsEnterprise.Location = new System.Drawing.Point(6, 454);
			this.checkAgingIsEnterprise.Name = "checkAgingIsEnterprise";
			this.checkAgingIsEnterprise.Size = new System.Drawing.Size(443, 17);
			this.checkAgingIsEnterprise.TabIndex = 226;
			this.checkAgingIsEnterprise.Text = "Aging for enterprise Galera cluster environments using FamAging table";
			this.checkAgingIsEnterprise.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAgingIsEnterprise.Click += new System.EventHandler(this.checkUseFamAgingTable_Click);
			// 
			// butClearAgingBeginDateT
			// 
			this.butClearAgingBeginDateT.Location = new System.Drawing.Point(455, 475);
			this.butClearAgingBeginDateT.Name = "butClearAgingBeginDateT";
			this.butClearAgingBeginDateT.Size = new System.Drawing.Size(43, 21);
			this.butClearAgingBeginDateT.TabIndex = 229;
			this.butClearAgingBeginDateT.Text = "Clear";
			this.butClearAgingBeginDateT.Click += new System.EventHandler(this.butClearAgingBeginDateT_Click);
			// 
			// labelAgingBeginDateT
			// 
			this.labelAgingBeginDateT.Location = new System.Drawing.Point(6, 472);
			this.labelAgingBeginDateT.Name = "labelAgingBeginDateT";
			this.labelAgingBeginDateT.Size = new System.Drawing.Size(291, 27);
			this.labelAgingBeginDateT.TabIndex = 228;
			this.labelAgingBeginDateT.Text = "DateTime the currently running aging started\r\nUsually blank";
			this.labelAgingBeginDateT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAgingBeginDateT
			// 
			this.textAgingBeginDateT.Location = new System.Drawing.Point(299, 475);
			this.textAgingBeginDateT.Name = "textAgingBeginDateT";
			this.textAgingBeginDateT.ReadOnly = true;
			this.textAgingBeginDateT.Size = new System.Drawing.Size(150, 20);
			this.textAgingBeginDateT.TabIndex = 227;
			this.textAgingBeginDateT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelAutoAgingRunTime
			// 
			this.labelAutoAgingRunTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelAutoAgingRunTime.Location = new System.Drawing.Point(6, 429);
			this.labelAutoAgingRunTime.Name = "labelAutoAgingRunTime";
			this.labelAutoAgingRunTime.Size = new System.Drawing.Size(367, 27);
			this.labelAutoAgingRunTime.TabIndex = 231;
			this.labelAutoAgingRunTime.Text = "Automated aging run time, only applies if using Daily aging \r\nLeave blank to disa" +
    "ble";
			this.labelAutoAgingRunTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAutoAgingRunTime
			// 
			this.textAutoAgingRunTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textAutoAgingRunTime.Location = new System.Drawing.Point(375, 432);
			this.textAutoAgingRunTime.Name = "textAutoAgingRunTime";
			this.textAutoAgingRunTime.Size = new System.Drawing.Size(74, 20);
			this.textAutoAgingRunTime.TabIndex = 230;
			this.textAutoAgingRunTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// checkSubmitExceptions
			// 
			this.checkSubmitExceptions.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSubmitExceptions.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSubmitExceptions.Location = new System.Drawing.Point(6, 499);
			this.checkSubmitExceptions.Name = "checkSubmitExceptions";
			this.checkSubmitExceptions.Size = new System.Drawing.Size(443, 17);
			this.checkSubmitExceptions.TabIndex = 232;
			this.checkSubmitExceptions.Text = "Automatically submit unhandled exceptions";
			this.checkSubmitExceptions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTheme
			// 
			this.comboTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTheme.FormattingEnabled = true;
			this.comboTheme.Location = new System.Drawing.Point(319, 140);
			this.comboTheme.Name = "comboTheme";
			this.comboTheme.Size = new System.Drawing.Size(130, 21);
			this.comboTheme.TabIndex = 230;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(7, 141);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(311, 17);
			this.label10.TabIndex = 231;
			this.label10.Text = "Theme";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkUserTheme
			// 
			this.checkUserTheme.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUserTheme.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUserTheme.Location = new System.Drawing.Point(255, 161);
			this.checkUserTheme.Name = "checkUserTheme";
			this.checkUserTheme.Size = new System.Drawing.Size(194, 17);
			this.checkUserTheme.TabIndex = 234;
			this.checkUserTheme.Text = "Users can set their own theme";
			this.checkUserTheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUserTheme.UseVisualStyleBackColor = true;
			// 
			// FormMisc
			// 
			this.ClientSize = new System.Drawing.Size(542, 644);
			this.Controls.Add(this.groupSelectPatient);
			this.Controls.Add(this.checkUserTheme);
			this.Controls.Add(this.checkSubmitExceptions);
			this.Controls.Add(this.comboTheme);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.butClearAgingBeginDateT);
			this.Controls.Add(this.labelAgingBeginDateT);
			this.Controls.Add(this.textAgingBeginDateT);
			this.Controls.Add(this.checkAgingIsEnterprise);
			this.Controls.Add(this.textAuditEntries);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.butClearCode);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textSyncCode);
			this.Controls.Add(this.butDecimal);
			this.Controls.Add(this.textNumDecimals);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.comboTrackClinic);
			this.Controls.Add(this.labelTrackClinic);
			this.Controls.Add(this.checkTimeCardUseLocal);
			this.Controls.Add(this.butPickLanguageAndRegion);
			this.Controls.Add(this.textLanguageAndRegion);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.checkImeCompositionCompatibility);
			this.Controls.Add(this.textInactiveSignal);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textWebServiceServerName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butLanguages);
			this.Controls.Add(this.textSigInterval);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.labelAutoAgingRunTime);
			this.Controls.Add(this.textAutoAgingRunTime);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(558, 650);
			this.Name = "FormMisc";
			this.ShowInTaskbar = false;
			this.Text = "Miscellaneous Setup";
			this.Load += new System.EventHandler(this.FormMisc_Load);
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupSelectPatient.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormMisc_Load(object sender, System.EventArgs e) {
			if(PrefC.GetLong(PrefName.ProcessSigsIntervalInSecs)==0){
				textSigInterval.Text="";
			}
			else{
				textSigInterval.Text=PrefC.GetLong(PrefName.ProcessSigsIntervalInSecs).ToString();
			}
			if(PrefC.GetLong(PrefName.SignalInactiveMinutes)==0) {
				textInactiveSignal.Text="";
			}
			else {
				textInactiveSignal.Text=PrefC.GetLong(PrefName.SignalInactiveMinutes).ToString();
			}
			foreach(OdTheme theme in Enum.GetValues(typeof(OdTheme))) {
				if(theme==OdTheme.None) {
					continue;
				}
				comboTheme.Items.Add(theme);
			}
			comboTheme.SelectedIndex=PrefC.GetInt(PrefName.ColorTheme);
			checkUserTheme.Checked=PrefC.GetBool(PrefName.ThemeSetByUser);
      checkTimeCardUseLocal.Checked=PrefC.GetBool(PrefName.LocalTimeOverridesServerTime);
			checkRefresh.Checked=!PrefC.GetBool(PrefName.PatientSelectUsesSearchButton);
			checkPrefFName.Checked=PrefC.GetBool(PrefName.PatientSelectUseFNameForPreferred);
			if(PrefC.HasClinicsEnabled){
				checkPatientSelectFilterRestrictedClinics.Visible=true;
				checkPatientSelectFilterRestrictedClinics.Checked=PrefC.GetBool(PrefName.PatientSelectFilterRestrictedClinics);
			}
			textMainWindowTitle.Text=PrefC.GetString(PrefName.MainWindowTitle);
			checkUseClinicAbbr.Checked=PrefC.GetBool(PrefName.TitleBarClinicUseAbbr);
			checkTitleBarShowSpecialty.Checked=PrefC.GetBool(PrefName.TitleBarShowSpecialty);
			comboShowID.Items.Add(Lan.g(this,"None"));
			comboShowID.Items.Add(Lan.g(this,"PatNum"));
			comboShowID.Items.Add(Lan.g(this,"ChartNumber"));
			comboShowID.Items.Add(Lan.g(this,"Birthdate"));
			comboShowID.SelectedIndex=PrefC.GetInt(PrefName.ShowIDinTitleBar);
			checkImeCompositionCompatibility.Checked=PrefC.GetBool(PrefName.ImeCompositionCompatibility);
			checkTitleBarShowSite.Checked=PrefC.GetBool(PrefName.TitleBarShowSite);
			textWebServiceServerName.Text=PrefC.GetString(PrefName.WebServiceServerName);
			if(PrefC.GetString(PrefName.LanguageAndRegion)!="") {
				textLanguageAndRegion.Text=PrefC.GetLanguageAndRegion().DisplayName;
			}
			else {
				textLanguageAndRegion.Text=Lan.g(this,"None");
			}
			textNumDecimals.Text=CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
			_trackLastClinicBy=new List<string> { "None","Workstation","User" };//must be in english because these values are stored in DB.
			_trackLastClinicBy.ForEach(x => comboTrackClinic.Items.Add(Lan.g(this,x)));//translation is for display only.
			comboTrackClinic.SelectedIndex=_trackLastClinicBy.FindIndex(x => x==PrefC.GetString(PrefName.ClinicTrackLast));
			if(comboTrackClinic.SelectedIndex==-1) {
				comboTrackClinic.SelectedIndex=0;
			}
			if(!PrefC.HasClinicsEnabled) {
				labelTrackClinic.Visible=false;
				comboTrackClinic.Visible=false;
				checkUseClinicAbbr.Visible=false;
			}
			textSyncCode.Text=PrefC.GetString(PrefName.CentralManagerSyncCode);
			textAuditEntries.Text=PrefC.GetString(PrefName.AuditTrailEntriesDisplayed);
			if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {
				//Enterprise
				checkAgingIsEnterprise.Checked=true;
				DateTime agingBeginDateT=PrefC.GetDateT(PrefName.AgingBeginDateTime);
				if(agingBeginDateT>DateTime.MinValue) {
					textAgingBeginDateT.Text=agingBeginDateT.ToString();
				}
			}
			else {
				//Classic
				labelAgingBeginDateT.Visible=false;
				textAgingBeginDateT.Visible=false;
				butClearAgingBeginDateT.Visible=false;
			}
			if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)) {
				textAutoAgingRunTime.Enabled=false;
			}
			else {
				DateTime agingDateTDue=PrefC.GetDateT(PrefName.AgingServiceTimeDue);
				if(agingDateTDue!=DateTime.MinValue) {
					textAutoAgingRunTime.Text=agingDateTDue.ToShortTimeString();
				}
			}
			checkSubmitExceptions.Checked=PrefC.GetBool(PrefName.SendUnhandledExceptionsToHQ);
			if(PrefC.IsCloudMode) {
				textWebServiceServerName.ReadOnly=true;
			}
		}

		private void butLanguages_Click(object sender,EventArgs e) {
			FormLanguagesUsed FormL=new FormLanguagesUsed();
			FormL.ShowDialog();
			if(FormL.DialogResult==DialogResult.OK){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}

		private void butPickLanguageAndRegion_Click(object sender,EventArgs e) {
			FormLanguageAndRegion FormLAR=new FormLanguageAndRegion();//FormLanguageAndRegion saves pref to DB.
			FormLAR.ShowDialog();
			if(PrefC.GetString(PrefName.LanguageAndRegion)!="") {
				textLanguageAndRegion.Text=PrefC.GetLanguageAndRegion().DisplayName;
			}
			else {
				textLanguageAndRegion.Text=Lan.g(this,"None");
			}
		}

		private void butDecimal_Click(object sender,EventArgs e) {
			FormDecimalSettings FormDS=new FormDecimalSettings();
			FormDS.ShowDialog();
		}

		private void butClearCode_Click(object sender,EventArgs e) {
			textSyncCode.Text="";
		}

		private void checkUseFamAgingTable_Click(object sender,EventArgs e) {
			if(checkAgingIsEnterprise.Checked && !PrefC.GetBool(PrefName.AgingIsEnterprise)) {
				//Enabling feature that is turned off according to pref table.
				string msgTxt=Lan.g(this,"In order to enable enterprise aging, enter the password from our manual below.");
				if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)) {
					//Conditional warning that only affects Monthly Aging customers.
					msgTxt+="\r\n"+Lan.g(this,"Note: This will change your aging from calculated monthly to calculated daily.");
				}
				InputBox iBox=new InputBox(msgTxt) { Text=Lan.g(this,"Enter Password") };
				iBox.ShowDialog();
				if(iBox.DialogResult!=DialogResult.OK) {
					checkAgingIsEnterprise.Checked=false;
					return;
				}
				if(iBox.textResult.Text!="abracadabra"){//to keep non-enterprise customers from unwittingly enabling this feature 
					checkAgingIsEnterprise.Checked=false;
					MsgBox.Show(this,"Incorrect password.  Refer to the online manual or contact Open Dental support for assistance.");
					return;
				}
			}
		}
		
		///<summary>This button and the associated label and textbox are only visible if AgingIsEnterprise is set in the database.  When the user first
		///enables the AgingIsEnterprise feature, there will be no need to clear the DateTime since it will already be blank.  Once the form closes and
		///AgingIsEnterprise is enabled, the next time the form is accessed they may have the option to clear the DateTime.  Requires SecurityAdmin permission.</summary>
		private void butClearAgingBeginDateT_Click(object sender,EventArgs e) {
			//user doesn't have permission or the pref is already cleared
			if(!Security.IsAuthorized(Permissions.SecurityAdmin) || PrefC.GetDateT(PrefName.AgingBeginDateTime)==DateTime.MinValue) {//blank=DateTime.MinValue
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"This will override the lock on the famaging table, potentially allowing a second connection to start "
				+"the aging calculations which could cause both calculations to fail.  If this happens, this flag will need to be cleared again and aging "
				+"started by a single connection.\r\nAre you sure you want to clear this value?"))
			{
				return;
			}
			textAgingBeginDateT.Text="";
			Prefs.UpdateString(PrefName.AgingBeginDateTime,"");
			DataValid.SetInvalid(InvalidType.Prefs);
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textAuditEntries.errorProvider1.GetError(textAuditEntries)!="") {
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			if(string.IsNullOrWhiteSpace(textSigInterval.Text) && PrefC.GetLong(PrefName.ProcessSigsIntervalInSecs)!=0) {
				bool proceed=MsgBox.Show(sender,MsgBoxButtons.YesNo,"Disabling the process signal interval prevents the use of kiosks.\r\n"
					+"This should not be done if there are multiple workstations in the office.\r\n"
					+"Proceed?");
				if (!proceed) {
					textSigInterval.Text=PrefC.GetLong(PrefName.ProcessSigsIntervalInSecs).ToString();
					return;
				}
			}
			if(PIn.Long(textSigInterval.Text)>=(5+(PIn.Long(textInactiveSignal.Text)*60)) && PIn.Long(textInactiveSignal.Text)!=0) {//Signal Refresh time is less than or equal to 5 seconds plus the number of seconds in textSigInterval
				string question=Lans.g(this,"The inactive signal time is less than or equal to the signal refresh time.")+"\r\n"
					+Lans.g(this,"This could inadvertently cause signals to not correctly refresh.  Continue?");
				if(MessageBox.Show(question,"",MessageBoxButtons.YesNo)!=DialogResult.Yes) {
					return;
				}
			}
			DateTime autoAgingRunTime=DateTime.MinValue;
			if(!string.IsNullOrWhiteSpace(textAutoAgingRunTime.Text)
				&& !PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)
				&& !DateTime.TryParse(textAutoAgingRunTime.Text,out autoAgingRunTime))
			{
				MsgBox.Show(this,"Automated Aging Run Time must be blank or a valid time of day.");
				return;
			}
			if(comboTrackClinic.SelectedIndex<0) {
				comboTrackClinic.SelectedIndex=0;
			}
			bool changed=false;
			if( Prefs.UpdateString(PrefName.MainWindowTitle,textMainWindowTitle.Text)
				| Prefs.UpdateLong(PrefName.ShowIDinTitleBar,comboShowID.SelectedIndex)
				| Prefs.UpdateBool(PrefName.TitleBarShowSite, checkTitleBarShowSite.Checked)
				| Prefs.UpdateString(PrefName.WebServiceServerName,textWebServiceServerName.Text)
        | Prefs.UpdateBool(PrefName.LocalTimeOverridesServerTime,checkTimeCardUseLocal.Checked)
				| Prefs.UpdateBool(PrefName.PatientSelectUseFNameForPreferred,checkPrefFName.Checked)
				| Prefs.UpdateBool(PrefName.PatientSelectUsesSearchButton,!checkRefresh.Checked)
				| Prefs.UpdateBool(PrefName.ImeCompositionCompatibility,checkImeCompositionCompatibility.Checked)
				| Prefs.UpdateString(PrefName.ClinicTrackLast,_trackLastClinicBy[comboTrackClinic.SelectedIndex])
				| Prefs.UpdateString(PrefName.CentralManagerSyncCode,textSyncCode.Text)
				| Prefs.UpdateString(PrefName.AuditTrailEntriesDisplayed,textAuditEntries.Text)
				| Prefs.UpdateBool(PrefName.AgingIsEnterprise,checkAgingIsEnterprise.Checked)
				| Prefs.UpdateBool(PrefName.TitleBarClinicUseAbbr,checkUseClinicAbbr.Checked)
				| Prefs.UpdateBool(PrefName.TitleBarShowSpecialty,checkTitleBarShowSpecialty.Checked)
				| Prefs.UpdateBool(PrefName.SendUnhandledExceptionsToHQ,checkSubmitExceptions.Checked)
				| Prefs.UpdateBool(PrefName.ThemeSetByUser,checkUserTheme.Checked)
				| Prefs.UpdateBool(PrefName.PatientSelectFilterRestrictedClinics,checkPatientSelectFilterRestrictedClinics.Checked)
			)
			{
				changed=true;
			}
			if(Prefs.UpdateInt(PrefName.ColorTheme,comboTheme.SelectedIndex)) {
				changed=true;
			}
			if(textSigInterval.Text==""){
				if(Prefs.UpdateLong(PrefName.ProcessSigsIntervalInSecs,0)){
					changed=true;
				}
			}
			else{
				if(Prefs.UpdateLong(PrefName.ProcessSigsIntervalInSecs,PIn.Long(textSigInterval.Text))){
					changed=true;
				}
			}
			if(textInactiveSignal.Text=="") {
				if(Prefs.UpdateLong(PrefName.SignalInactiveMinutes,0)) {
					changed=true;
				}
			}
			else {
				if(Prefs.UpdateLong(PrefName.SignalInactiveMinutes,PIn.Long(textInactiveSignal.Text))) {
					changed=true;
				}
			}
			//if AgingIsEnterprise, change the aging to daily aging if it is currently set to monthly.  AgingIsEnterprise requires daily aging.
			if(checkAgingIsEnterprise.Checked && Prefs.UpdateBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily,false)) {
				changed=true;
			}
			if(autoAgingRunTime==DateTime.MinValue) {
				if(Prefs.UpdateString(PrefName.AgingServiceTimeDue,"")) {
					changed=true;
				}
			}
			else {
				if(Prefs.UpdateString(PrefName.AgingServiceTimeDue,POut.DateT(autoAgingRunTime,false))) {
					changed=true;
				}
			}
			if(changed){
				//ComputerPrefs may not need to be invalidated here, since task computer settings moved to FormTaskSetup.  Leaving here for now just in case.
				DataValid.SetInvalid(InvalidType.Prefs, InvalidType.Computers);
				ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}





