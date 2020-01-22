namespace OpenDental{
	partial class FormEServicesSetup {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEServicesSetup));
			this.label37 = new System.Windows.Forms.Label();
			this.menuWebSchedVerifyTextTemplate = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.insertReplacementsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label23 = new System.Windows.Forms.Label();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabSignup = new System.Windows.Forms.TabPage();
			this.webBrowserSignup = new System.Windows.Forms.WebBrowser();
			this.tabEConnector = new System.Windows.Forms.TabPage();
			this.label25 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.checkEmailsWithDiffProcess = new System.Windows.Forms.CheckBox();
			this.labelInstallWarning = new System.Windows.Forms.Label();
			this.butInstallEConnector = new OpenDental.UI.Button();
			this.labelListenerServiceAck = new System.Windows.Forms.Label();
			this.butListenerServiceAck = new OpenDental.UI.Button();
			this.label27 = new System.Windows.Forms.Label();
			this.butListenerServiceHistoryRefresh = new OpenDental.UI.Button();
			this.label26 = new System.Windows.Forms.Label();
			this.gridListenerServiceStatusHistory = new OpenDental.UI.ODGrid();
			this.butStartListenerService = new OpenDental.UI.Button();
			this.label24 = new System.Windows.Forms.Label();
			this.labelListenerStatus = new System.Windows.Forms.Label();
			this.butListenerAlertsOff = new OpenDental.UI.Button();
			this.textListenerServiceStatus = new System.Windows.Forms.TextBox();
			this.tabMobileSynch = new System.Windows.Forms.TabPage();
			this.checkTroubleshooting = new System.Windows.Forms.CheckBox();
			this.butDelete = new OpenDental.UI.Button();
			this.textDateTimeLastRun = new System.Windows.Forms.Label();
			this.groupPreferences = new System.Windows.Forms.GroupBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.textMobileUserName = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.butCurrentWorkstation = new OpenDental.UI.Button();
			this.textMobilePassword = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.textMobileSynchWorkStation = new System.Windows.Forms.TextBox();
			this.textSynchMinutes = new OpenDental.ValidNumber();
			this.label18 = new System.Windows.Forms.Label();
			this.butSaveMobileSynch = new OpenDental.UI.Button();
			this.textDateBefore = new OpenDental.ValidDate();
			this.labelMobileSynchURL = new System.Windows.Forms.Label();
			this.textMobileSyncServerURL = new System.Windows.Forms.TextBox();
			this.labelMinutesBetweenSynch = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.butFullSync = new OpenDental.UI.Button();
			this.butSync = new OpenDental.UI.Button();
			this.tabMobileWeb = new System.Windows.Forms.TabPage();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.butSetupMobileWebUsers = new OpenDental.UI.Button();
			this.label29 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textHostedUrlMobileWeb = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.tabPatientPortal = new System.Windows.Forms.TabPage();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.comboPPClinicUrl = new OpenDental.UI.ComboBoxClinicPicker();
			this.label39 = new System.Windows.Forms.Label();
			this.radioPatientPortalPayment = new System.Windows.Forms.RadioButton();
			this.radioPatientPortalLogin = new System.Windows.Forms.RadioButton();
			this.butCopyToClipboard = new OpenDental.UI.Button();
			this.textHostedUrlPortal = new System.Windows.Forms.TextBox();
			this.butNavigateTo = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.groupPatientPortalInvites = new System.Windows.Forms.GroupBox();
			this.textStatusInvites = new System.Windows.Forms.TextBox();
			this.butActivateInvites = new OpenDental.UI.Button();
			this.comboClinicsPPInvites = new OpenDental.UI.ComboBoxClinicPicker();
			this.checkUseDefaultsPPInvites = new System.Windows.Forms.CheckBox();
			this.butAddPPInviteRule = new OpenDental.UI.Button();
			this.gridPatPortalInviteRules = new OpenDental.UI.ODGrid();
			this.checkIsPPInvitesEnabled = new System.Windows.Forms.CheckBox();
			this.groupBoxNotification = new System.Windows.Forms.GroupBox();
			this.butEditWebMailNotificationBody = new OpenDental.UI.Button();
			this.browserWebMailNotificatonBody = new System.Windows.Forms.WebBrowser();
			this.label9 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.textBoxNotificationSubject = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.textPatientFacingUrlPortal = new System.Windows.Forms.TextBox();
			this.tabWebSched = new System.Windows.Forms.TabPage();
			this.linkLabelAboutWebSched = new System.Windows.Forms.LinkLabel();
			this.labelWebSchedDesc = new System.Windows.Forms.Label();
			this.tabControlWebSched = new System.Windows.Forms.TabControl();
			this.tabWebSchedRecalls = new System.Windows.Forms.TabPage();
			this.groupWebSchedProvRule = new System.Windows.Forms.GroupBox();
			this.butProvRulePickClinic = new OpenDental.UI.Button();
			this.checkUseDefaultProvRule = new System.Windows.Forms.CheckBox();
			this.comboClinicProvRule = new OpenDental.UI.ComboBoxClinicPicker();
			this.listBoxWebSchedProviderPref = new System.Windows.Forms.ListBox();
			this.label21 = new System.Windows.Forms.Label();
			this.checkWSRDoubleBooking = new System.Windows.Forms.CheckBox();
			this.comboWSRConfirmStatus = new OpenDental.UI.ComboBoxPlus();
			this.label36 = new System.Windows.Forms.Label();
			this.label43 = new System.Windows.Forms.Label();
			this.label44 = new System.Windows.Forms.Label();
			this.textWebSchedRecallApptSearchDays = new OpenDental.ValidNumber();
			this.checkRecallAllowProvSelection = new System.Windows.Forms.CheckBox();
			this.groupWebSchedText = new System.Windows.Forms.GroupBox();
			this.labelWebSchedPerBatch = new System.Windows.Forms.Label();
			this.textWebSchedPerBatch = new OpenDental.ValidNumber();
			this.radioDoNotSendText = new System.Windows.Forms.RadioButton();
			this.radioSendText = new System.Windows.Forms.RadioButton();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.listboxWebSchedRecallIgnoreBlockoutTypes = new System.Windows.Forms.ListBox();
			this.label32 = new System.Windows.Forms.Label();
			this.butWebSchedRecallBlockouts = new OpenDental.UI.Button();
			this.groupBoxWebSchedAutomation = new System.Windows.Forms.GroupBox();
			this.radioSendToEmailNoPreferred = new System.Windows.Forms.RadioButton();
			this.radioDoNotSend = new System.Windows.Forms.RadioButton();
			this.radioSendToEmailOnlyPreferred = new System.Windows.Forms.RadioButton();
			this.radioSendToEmail = new System.Windows.Forms.RadioButton();
			this.groupWebSchedPreview = new System.Windows.Forms.GroupBox();
			this.butWebSchedPickClinic = new OpenDental.UI.Button();
			this.butWebSchedPickProv = new OpenDental.UI.Button();
			this.label22 = new System.Windows.Forms.Label();
			this.comboWebSchedProviders = new System.Windows.Forms.ComboBox();
			this.butWebSchedToday = new OpenDental.UI.Button();
			this.gridWebSchedTimeSlots = new OpenDental.UI.ODGrid();
			this.textWebSchedDateStart = new OpenDental.ValidDate();
			this.labelWebSchedClinic = new System.Windows.Forms.Label();
			this.labelWebSchedRecallTypes = new System.Windows.Forms.Label();
			this.comboWebSchedClinic = new System.Windows.Forms.ComboBox();
			this.comboWebSchedRecallTypes = new System.Windows.Forms.ComboBox();
			this.gridWebSchedOperatories = new OpenDental.UI.ODGrid();
			this.label35 = new System.Windows.Forms.Label();
			this.butRecallSchedSetup = new OpenDental.UI.Button();
			this.label31 = new System.Windows.Forms.Label();
			this.gridWebSchedRecallTypes = new OpenDental.UI.ODGrid();
			this.label20 = new System.Windows.Forms.Label();
			this.tabWebSchedNewPatAppts = new System.Windows.Forms.TabPage();
			this.checkWSNPDoubleBooking = new System.Windows.Forms.CheckBox();
			this.label38 = new System.Windows.Forms.Label();
			this.groupBoxWSNPHostedURLs = new System.Windows.Forms.GroupBox();
			this.panelHostedURLs = new System.Windows.Forms.FlowLayoutPanel();
			this.checkNewPatAllowProvSelection = new System.Windows.Forms.CheckBox();
			this.checkWebSchedNewPatForcePhoneFormatting = new System.Windows.Forms.CheckBox();
			this.comboWSNPConfirmStatuses = new OpenDental.UI.ComboBoxPlus();
			this.labelWebSchedNewPatConfirmStatus = new System.Windows.Forms.Label();
			this.groupBox13 = new System.Windows.Forms.GroupBox();
			this.textWebSchedNewPatApptMessage = new System.Windows.Forms.TextBox();
			this.groupBox11 = new System.Windows.Forms.GroupBox();
			this.listboxWebSchedNewPatIgnoreBlockoutTypes = new System.Windows.Forms.ListBox();
			this.label33 = new System.Windows.Forms.Label();
			this.butWebSchedNewPatBlockouts = new OpenDental.UI.Button();
			this.gridWebSchedNewPatApptOps = new OpenDental.UI.ODGrid();
			this.label42 = new System.Windows.Forms.Label();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.labelWSNPClinic = new System.Windows.Forms.Label();
			this.labelWSNPAApptType = new System.Windows.Forms.Label();
			this.comboWSNPClinics = new System.Windows.Forms.ComboBox();
			this.comboWSNPADefApptType = new OpenDental.UI.ComboBoxPlus();
			this.labelWSNPTimeSlots = new System.Windows.Forms.Label();
			this.butWebSchedNewPatApptsToday = new OpenDental.UI.Button();
			this.gridWebSchedNewPatApptTimeSlots = new OpenDental.UI.ODGrid();
			this.textWebSchedNewPatApptsDateStart = new OpenDental.ValidDate();
			this.gridWSNPAReasons = new OpenDental.UI.ODGrid();
			this.label41 = new System.Windows.Forms.Label();
			this.label40 = new System.Windows.Forms.Label();
			this.textWebSchedNewPatApptSearchDays = new OpenDental.ValidNumber();
			this.tabWebSchedVerify = new System.Windows.Forms.TabPage();
			this.butRestoreWebSchedVerify = new OpenDental.UI.Button();
			this.label28 = new System.Windows.Forms.Label();
			this.checkUseDefaultsVerify = new System.Windows.Forms.CheckBox();
			this.groupBoxASAP = new System.Windows.Forms.GroupBox();
			this.groupBoxASAPTextTemplate = new System.Windows.Forms.GroupBox();
			this.textASAPTextTemplate = new System.Windows.Forms.TextBox();
			this.groupBoxASAPEmail = new System.Windows.Forms.GroupBox();
			this.butAsapEditEmail = new OpenDental.UI.Button();
			this.browserAsapEmailBody = new System.Windows.Forms.WebBrowser();
			this.textASAPEmailSubj = new System.Windows.Forms.TextBox();
			this.groupBoxRadioASAP = new System.Windows.Forms.GroupBox();
			this.radioASAPTextAndEmail = new System.Windows.Forms.RadioButton();
			this.radioASAPEmail = new System.Windows.Forms.RadioButton();
			this.radioASAPText = new System.Windows.Forms.RadioButton();
			this.radioASAPNone = new System.Windows.Forms.RadioButton();
			this.groupBoxNewPat = new System.Windows.Forms.GroupBox();
			this.groupBoxNewPatTextTemplate = new System.Windows.Forms.GroupBox();
			this.textNewPatTextTemplate = new System.Windows.Forms.TextBox();
			this.groupBoxNewPatEmail = new System.Windows.Forms.GroupBox();
			this.butNewPatEditEmail = new OpenDental.UI.Button();
			this.browserNewPatEmailBody = new System.Windows.Forms.WebBrowser();
			this.textNewPatEmailSubj = new System.Windows.Forms.TextBox();
			this.groupBoxRadioNewPat = new System.Windows.Forms.GroupBox();
			this.radioNewPatTextAndEmail = new System.Windows.Forms.RadioButton();
			this.radioNewPatEmail = new System.Windows.Forms.RadioButton();
			this.radioNewPatText = new System.Windows.Forms.RadioButton();
			this.radioNewPatNone = new System.Windows.Forms.RadioButton();
			this.groupBoxRecall = new System.Windows.Forms.GroupBox();
			this.groupBoxRecallTextTemplate = new System.Windows.Forms.GroupBox();
			this.textRecallTextTemplate = new System.Windows.Forms.TextBox();
			this.groupBoxRecallEmail = new System.Windows.Forms.GroupBox();
			this.butRecallEditEmail = new OpenDental.UI.Button();
			this.browserRecallEmailBody = new System.Windows.Forms.WebBrowser();
			this.textRecallEmailSubj = new System.Windows.Forms.TextBox();
			this.groupBoxRadioRecall = new System.Windows.Forms.GroupBox();
			this.radioRecallTextAndEmail = new System.Windows.Forms.RadioButton();
			this.radioRecallEmail = new System.Windows.Forms.RadioButton();
			this.radioRecallText = new System.Windows.Forms.RadioButton();
			this.radioRecallNone = new System.Windows.Forms.RadioButton();
			this.comboClinicVerify = new OpenDental.UI.ComboBoxClinicPicker();
			this.tabTexting = new System.Windows.Forms.TabPage();
			this.groupShortCode = new System.Windows.Forms.GroupBox();
			this.labelUnsavedShortCodeChanges = new System.Windows.Forms.Label();
			this.butSaveShortCodes = new OpenDental.UI.Button();
			this.labelShortCodeOptInClinicTitle = new System.Windows.Forms.Label();
			this.textShortCodeOptInClinicTitle = new System.Windows.Forms.TextBox();
			this.checkOptInPrompt = new System.Windows.Forms.CheckBox();
			this.comboShortCodeClinic = new OpenDental.UI.ComboBoxClinicPicker();
			this.butDefaultClinicClear = new OpenDental.UI.Button();
			this.butDefaultClinic = new OpenDental.UI.Button();
			this.butBackMonth = new OpenDental.UI.Button();
			this.dateTimePickerSms = new System.Windows.Forms.DateTimePicker();
			this.gridSmsSummary = new OpenDental.UI.ODGrid();
			this.butFwdMonth = new OpenDental.UI.Button();
			this.butThisMonth = new OpenDental.UI.Button();
			this.tabECR = new System.Windows.Forms.TabPage();
			this.butAddThankYouVerify = new OpenDental.UI.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.gridConfStatuses = new OpenDental.UI.ODGrid();
			this.checkUseDefaultsEC = new System.Windows.Forms.CheckBox();
			this.textStatusReminders = new System.Windows.Forms.TextBox();
			this.butActivateReminder = new OpenDental.UI.Button();
			this.textStatusConfirmations = new System.Windows.Forms.TextBox();
			this.butActivateConfirm = new OpenDental.UI.Button();
			this.groupAutomationStatuses = new System.Windows.Forms.GroupBox();
			this.radio2ClickConfirm = new System.Windows.Forms.RadioButton();
			this.radio1ClickConfirm = new System.Windows.Forms.RadioButton();
			this.comboStatusEFailed = new OpenDental.UI.ComboBoxPlus();
			this.label50 = new System.Windows.Forms.Label();
			this.checkEnableNoClinic = new System.Windows.Forms.CheckBox();
			this.comboStatusEDeclined = new OpenDental.UI.ComboBoxPlus();
			this.comboStatusESent = new OpenDental.UI.ComboBoxPlus();
			this.comboStatusEAccepted = new OpenDental.UI.ComboBoxPlus();
			this.label51 = new System.Windows.Forms.Label();
			this.label52 = new System.Windows.Forms.Label();
			this.label53 = new System.Windows.Forms.Label();
			this.checkIsConfirmEnabled = new System.Windows.Forms.CheckBox();
			this.comboClinicEConfirm = new System.Windows.Forms.ComboBox();
			this.label54 = new System.Windows.Forms.Label();
			this.butAddConfirmation = new OpenDental.UI.Button();
			this.butAddReminder = new OpenDental.UI.Button();
			this.gridRemindersMain = new OpenDental.UI.ODGrid();
			this.tabEClipboard = new System.Windows.Forms.TabPage();
			this.labelEClipboardNotSignedUp = new System.Windows.Forms.Label();
			this.clinicPickerEClipboard = new OpenDental.UI.ComboBoxClinicPicker();
			this.label10 = new System.Windows.Forms.Label();
			this.gridMobileAppDevices = new OpenDental.UI.ODGrid();
			this.groupEClipboardSheets = new System.Windows.Forms.GroupBox();
			this.butEClipboardAddSheets = new OpenDental.UI.Button();
			this.butEClipboardUp = new OpenDental.UI.Button();
			this.butEClipboardDown = new OpenDental.UI.Button();
			this.butEClipboardRight = new OpenDental.UI.Button();
			this.butEClipboardLeft = new OpenDental.UI.Button();
			this.gridEClipboardSheetsInUse = new OpenDental.UI.ODGrid();
			this.listEClipboardSheetsAvailable = new System.Windows.Forms.ListBox();
			this.labelOps = new System.Windows.Forms.Label();
			this.groupEClipboardRules = new System.Windows.Forms.GroupBox();
			this.checkEClipboardAllowSelfPortrait = new System.Windows.Forms.CheckBox();
			this.checkEClipboardCreateMissingForms = new System.Windows.Forms.CheckBox();
			this.labelEClipboardMessage = new System.Windows.Forms.Label();
			this.textEClipboardMessage = new System.Windows.Forms.TextBox();
			this.checkEClipboardAllowSheets = new System.Windows.Forms.CheckBox();
			this.checkEClipboardAllowCheckIn = new System.Windows.Forms.CheckBox();
			this.checkEClipboardPopupKiosk = new System.Windows.Forms.CheckBox();
			this.checkEClipboardUseDefaults = new System.Windows.Forms.CheckBox();
			this.tabMisc = new System.Windows.Forms.TabPage();
			this.groupDateFormat = new System.Windows.Forms.GroupBox();
			this.label30 = new System.Windows.Forms.Label();
			this.labelDateCustom = new System.Windows.Forms.Label();
			this.textDateCustom = new System.Windows.Forms.TextBox();
			this.label34 = new System.Windows.Forms.Label();
			this.radioDateCustom = new System.Windows.Forms.RadioButton();
			this.radioDateMMMMdyyyy = new System.Windows.Forms.RadioButton();
			this.radioDatem = new System.Windows.Forms.RadioButton();
			this.radioDateLongDate = new System.Windows.Forms.RadioButton();
			this.radioDateShortDate = new System.Windows.Forms.RadioButton();
			this.groupNotUsed = new System.Windows.Forms.GroupBox();
			this.butShowOldMobileSych = new OpenDental.UI.Button();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.dateRunEnd = new System.Windows.Forms.DateTimePicker();
			this.label46 = new System.Windows.Forms.Label();
			this.dateRunStart = new System.Windows.Forms.DateTimePicker();
			this.label47 = new System.Windows.Forms.Label();
			this.label48 = new System.Windows.Forms.Label();
			this.butClose = new OpenDental.UI.Button();
			this.menuWebSchedVerifyTextTemplate.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabSignup.SuspendLayout();
			this.tabEConnector.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabMobileSynch.SuspendLayout();
			this.groupPreferences.SuspendLayout();
			this.tabMobileWeb.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabPatientPortal.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupPatientPortalInvites.SuspendLayout();
			this.groupBoxNotification.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabWebSched.SuspendLayout();
			this.tabControlWebSched.SuspendLayout();
			this.tabWebSchedRecalls.SuspendLayout();
			this.groupWebSchedProvRule.SuspendLayout();
			this.groupWebSchedText.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBoxWebSchedAutomation.SuspendLayout();
			this.groupWebSchedPreview.SuspendLayout();
			this.tabWebSchedNewPatAppts.SuspendLayout();
			this.groupBoxWSNPHostedURLs.SuspendLayout();
			this.groupBox13.SuspendLayout();
			this.groupBox11.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.tabWebSchedVerify.SuspendLayout();
			this.groupBoxASAP.SuspendLayout();
			this.groupBoxASAPTextTemplate.SuspendLayout();
			this.groupBoxASAPEmail.SuspendLayout();
			this.groupBoxRadioASAP.SuspendLayout();
			this.groupBoxNewPat.SuspendLayout();
			this.groupBoxNewPatTextTemplate.SuspendLayout();
			this.groupBoxNewPatEmail.SuspendLayout();
			this.groupBoxRadioNewPat.SuspendLayout();
			this.groupBoxRecall.SuspendLayout();
			this.groupBoxRecallTextTemplate.SuspendLayout();
			this.groupBoxRecallEmail.SuspendLayout();
			this.groupBoxRadioRecall.SuspendLayout();
			this.tabTexting.SuspendLayout();
			this.groupShortCode.SuspendLayout();
			this.tabECR.SuspendLayout();
			this.groupAutomationStatuses.SuspendLayout();
			this.tabEClipboard.SuspendLayout();
			this.groupEClipboardSheets.SuspendLayout();
			this.groupEClipboardRules.SuspendLayout();
			this.tabMisc.SuspendLayout();
			this.groupDateFormat.SuspendLayout();
			this.groupNotUsed.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.SuspendLayout();
			// 
			// label37
			// 
			this.label37.Location = new System.Drawing.Point(0, 0);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(100, 23);
			this.label37.TabIndex = 0;
			// 
			// menuWebSchedVerifyTextTemplate
			// 
			this.menuWebSchedVerifyTextTemplate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertReplacementsToolStripMenuItem,
            this.undoToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.selectAllToolStripMenuItem});
			this.menuWebSchedVerifyTextTemplate.Name = "menuASAPEmailBody";
			this.menuWebSchedVerifyTextTemplate.Size = new System.Drawing.Size(137, 136);
			this.menuWebSchedVerifyTextTemplate.Text = "Insert Replacements";
			// 
			// insertReplacementsToolStripMenuItem
			// 
			this.insertReplacementsToolStripMenuItem.Name = "insertReplacementsToolStripMenuItem";
			this.insertReplacementsToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.insertReplacementsToolStripMenuItem.Text = "Insert Fields";
			this.insertReplacementsToolStripMenuItem.Click += new System.EventHandler(this.WebSchedVerify_ContextMenuReplacementsClick);
			// 
			// undoToolStripMenuItem
			// 
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			this.undoToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.undoToolStripMenuItem.Text = "Undo";
			this.undoToolStripMenuItem.Click += new System.EventHandler(this.WebSchedVerify_ContextMenuUndoClick);
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.cutToolStripMenuItem.Text = "Cut";
			this.cutToolStripMenuItem.Click += new System.EventHandler(this.WebSchedVerify_ContextMenuCutClick);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.WebSchedVerify_ContextMenuCopyClick);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.pasteToolStripMenuItem.Text = "Paste";
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.WebSchedVerify_ContextMenuPasteClick);
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.selectAllToolStripMenuItem.Text = "Select All";
			this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.WebSchedVerify_ContextMenuSelectAllClick);
			// 
			// label23
			// 
			this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label23.Location = new System.Drawing.Point(13, 9);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(1159, 21);
			this.label23.TabIndex = 244;
			this.label23.Text = "eServices refer to Open Dental features that can be delivered electronically via " +
    "the internet.  All eServices hosted by Open Dental use the eConnector Service.";
			this.label23.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabSignup);
			this.tabControl.Controls.Add(this.tabEConnector);
			this.tabControl.Controls.Add(this.tabMobileSynch);
			this.tabControl.Controls.Add(this.tabMobileWeb);
			this.tabControl.Controls.Add(this.tabPatientPortal);
			this.tabControl.Controls.Add(this.tabWebSched);
			this.tabControl.Controls.Add(this.tabTexting);
			this.tabControl.Controls.Add(this.tabECR);
			this.tabControl.Controls.Add(this.tabEClipboard);
			this.tabControl.Controls.Add(this.tabMisc);
			this.tabControl.Location = new System.Drawing.Point(12, 33);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(1162, 621);
			this.tabControl.TabIndex = 53;
			this.tabControl.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Deselecting);
			// 
			// tabSignup
			// 
			this.tabSignup.BackColor = System.Drawing.SystemColors.Control;
			this.tabSignup.Controls.Add(this.webBrowserSignup);
			this.tabSignup.Location = new System.Drawing.Point(4, 22);
			this.tabSignup.Name = "tabSignup";
			this.tabSignup.Padding = new System.Windows.Forms.Padding(3);
			this.tabSignup.Size = new System.Drawing.Size(1154, 595);
			this.tabSignup.TabIndex = 9;
			this.tabSignup.Text = "Signup";
			// 
			// webBrowserSignup
			// 
			this.webBrowserSignup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowserSignup.Location = new System.Drawing.Point(3, 3);
			this.webBrowserSignup.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowserSignup.Name = "webBrowserSignup";
			this.webBrowserSignup.Size = new System.Drawing.Size(1148, 589);
			this.webBrowserSignup.TabIndex = 0;
			// 
			// tabEConnector
			// 
			this.tabEConnector.BackColor = System.Drawing.SystemColors.Control;
			this.tabEConnector.Controls.Add(this.label25);
			this.tabEConnector.Controls.Add(this.groupBox3);
			this.tabEConnector.Location = new System.Drawing.Point(4, 22);
			this.tabEConnector.Name = "tabEConnector";
			this.tabEConnector.Padding = new System.Windows.Forms.Padding(3);
			this.tabEConnector.Size = new System.Drawing.Size(1154, 595);
			this.tabEConnector.TabIndex = 4;
			this.tabEConnector.Text = "eConnector Service";
			// 
			// label25
			// 
			this.label25.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label25.Location = new System.Drawing.Point(111, 8);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(932, 68);
			this.label25.TabIndex = 251;
			this.label25.Text = resources.GetString("label25.Text");
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox3.Controls.Add(this.checkEmailsWithDiffProcess);
			this.groupBox3.Controls.Add(this.labelInstallWarning);
			this.groupBox3.Controls.Add(this.butInstallEConnector);
			this.groupBox3.Controls.Add(this.labelListenerServiceAck);
			this.groupBox3.Controls.Add(this.butListenerServiceAck);
			this.groupBox3.Controls.Add(this.label27);
			this.groupBox3.Controls.Add(this.butListenerServiceHistoryRefresh);
			this.groupBox3.Controls.Add(this.label26);
			this.groupBox3.Controls.Add(this.gridListenerServiceStatusHistory);
			this.groupBox3.Controls.Add(this.butStartListenerService);
			this.groupBox3.Controls.Add(this.label24);
			this.groupBox3.Controls.Add(this.labelListenerStatus);
			this.groupBox3.Controls.Add(this.butListenerAlertsOff);
			this.groupBox3.Controls.Add(this.textListenerServiceStatus);
			this.groupBox3.Location = new System.Drawing.Point(114, 79);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(929, 382);
			this.groupBox3.TabIndex = 249;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "eConnector Service Monitor";
			// 
			// checkEmailsWithDiffProcess
			// 
			this.checkEmailsWithDiffProcess.AutoSize = true;
			this.checkEmailsWithDiffProcess.Location = new System.Drawing.Point(10, 318);
			this.checkEmailsWithDiffProcess.Name = "checkEmailsWithDiffProcess";
			this.checkEmailsWithDiffProcess.Size = new System.Drawing.Size(286, 17);
			this.checkEmailsWithDiffProcess.TabIndex = 257;
			this.checkEmailsWithDiffProcess.Text = "Send emails with a different process on the eConnector";
			this.checkEmailsWithDiffProcess.UseVisualStyleBackColor = true;
			// 
			// labelInstallWarning
			// 
			this.labelInstallWarning.Location = new System.Drawing.Point(524, 72);
			this.labelInstallWarning.Name = "labelInstallWarning";
			this.labelInstallWarning.Size = new System.Drawing.Size(399, 13);
			this.labelInstallWarning.TabIndex = 256;
			this.labelInstallWarning.Text = " By clicking \'Install\' you consent to running the eConnector as a service";
			// 
			// butInstallEConnector
			// 
			this.butInstallEConnector.Location = new System.Drawing.Point(594, 45);
			this.butInstallEConnector.Name = "butInstallEConnector";
			this.butInstallEConnector.Size = new System.Drawing.Size(61, 24);
			this.butInstallEConnector.TabIndex = 255;
			this.butInstallEConnector.Text = "Install";
			this.butInstallEConnector.Click += new System.EventHandler(this.butInstallEConnector_Click);
			// 
			// labelListenerServiceAck
			// 
			this.labelListenerServiceAck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelListenerServiceAck.Location = new System.Drawing.Point(278, 306);
			this.labelListenerServiceAck.Name = "labelListenerServiceAck";
			this.labelListenerServiceAck.Size = new System.Drawing.Size(578, 13);
			this.labelListenerServiceAck.TabIndex = 254;
			this.labelListenerServiceAck.Text = "Acknowledge all errors.  This will stop the eServices menu from showing yellow.";
			this.labelListenerServiceAck.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butListenerServiceAck
			// 
			this.butListenerServiceAck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butListenerServiceAck.Location = new System.Drawing.Point(862, 300);
			this.butListenerServiceAck.Name = "butListenerServiceAck";
			this.butListenerServiceAck.Size = new System.Drawing.Size(61, 24);
			this.butListenerServiceAck.TabIndex = 253;
			this.butListenerServiceAck.Text = "Ack";
			this.butListenerServiceAck.Click += new System.EventHandler(this.butListenerServiceAck_Click);
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(7, 18);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(916, 19);
			this.label27.TabIndex = 252;
			this.label27.Text = "Open Dental monitors the status of the eConnector Service and alerts all workstat" +
    "ions when status is critical.";
			// 
			// butListenerServiceHistoryRefresh
			// 
			this.butListenerServiceHistoryRefresh.Location = new System.Drawing.Point(862, 111);
			this.butListenerServiceHistoryRefresh.Name = "butListenerServiceHistoryRefresh";
			this.butListenerServiceHistoryRefresh.Size = new System.Drawing.Size(61, 24);
			this.butListenerServiceHistoryRefresh.TabIndex = 251;
			this.butListenerServiceHistoryRefresh.Text = "Refresh";
			this.butListenerServiceHistoryRefresh.Click += new System.EventHandler(this.butListenerServiceHistoryRefresh_Click);
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(3, 94);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(853, 37);
			this.label26.TabIndex = 250;
			this.label26.Text = resources.GetString("label26.Text");
			this.label26.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// gridListenerServiceStatusHistory
			// 
			this.gridListenerServiceStatusHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridListenerServiceStatusHistory.Location = new System.Drawing.Point(6, 141);
			this.gridListenerServiceStatusHistory.Name = "gridListenerServiceStatusHistory";
			this.gridListenerServiceStatusHistory.Size = new System.Drawing.Size(917, 157);
			this.gridListenerServiceStatusHistory.TabIndex = 249;
			this.gridListenerServiceStatusHistory.Title = "eConnector History";
			this.gridListenerServiceStatusHistory.TranslationName = "FormEServicesSetup";
			this.gridListenerServiceStatusHistory.WrapText = false;
			// 
			// butStartListenerService
			// 
			this.butStartListenerService.Enabled = false;
			this.butStartListenerService.Location = new System.Drawing.Point(527, 45);
			this.butStartListenerService.Name = "butStartListenerService";
			this.butStartListenerService.Size = new System.Drawing.Size(61, 24);
			this.butStartListenerService.TabIndex = 245;
			this.butStartListenerService.Text = "Start";
			this.butStartListenerService.Click += new System.EventHandler(this.butStartListenerService_Click);
			// 
			// label24
			// 
			this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label24.Location = new System.Drawing.Point(115, 349);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(578, 29);
			this.label24.TabIndex = 248;
			this.label24.Text = "Before you stop monitoring, first uninstall the eConnector Service.\r\nMonitoring w" +
    "ill automatically resume when an active eConnector Service has been detected.";
			// 
			// labelListenerStatus
			// 
			this.labelListenerStatus.Location = new System.Drawing.Point(177, 48);
			this.labelListenerStatus.Name = "labelListenerStatus";
			this.labelListenerStatus.Size = new System.Drawing.Size(238, 17);
			this.labelListenerStatus.TabIndex = 244;
			this.labelListenerStatus.Text = "Current eConnector Service Status";
			this.labelListenerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butListenerAlertsOff
			// 
			this.butListenerAlertsOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butListenerAlertsOff.Location = new System.Drawing.Point(9, 350);
			this.butListenerAlertsOff.Name = "butListenerAlertsOff";
			this.butListenerAlertsOff.Size = new System.Drawing.Size(100, 24);
			this.butListenerAlertsOff.TabIndex = 247;
			this.butListenerAlertsOff.Text = "Stop Monitoring";
			this.butListenerAlertsOff.Click += new System.EventHandler(this.butListenerAlertsOff_Click);
			// 
			// textListenerServiceStatus
			// 
			this.textListenerServiceStatus.Location = new System.Drawing.Point(421, 47);
			this.textListenerServiceStatus.Name = "textListenerServiceStatus";
			this.textListenerServiceStatus.ReadOnly = true;
			this.textListenerServiceStatus.Size = new System.Drawing.Size(100, 20);
			this.textListenerServiceStatus.TabIndex = 246;
			// 
			// tabMobileSynch
			// 
			this.tabMobileSynch.BackColor = System.Drawing.SystemColors.Control;
			this.tabMobileSynch.Controls.Add(this.checkTroubleshooting);
			this.tabMobileSynch.Controls.Add(this.butDelete);
			this.tabMobileSynch.Controls.Add(this.textDateTimeLastRun);
			this.tabMobileSynch.Controls.Add(this.groupPreferences);
			this.tabMobileSynch.Controls.Add(this.label19);
			this.tabMobileSynch.Controls.Add(this.butFullSync);
			this.tabMobileSynch.Controls.Add(this.butSync);
			this.tabMobileSynch.Location = new System.Drawing.Point(4, 22);
			this.tabMobileSynch.Name = "tabMobileSynch";
			this.tabMobileSynch.Size = new System.Drawing.Size(1154, 595);
			this.tabMobileSynch.TabIndex = 2;
			this.tabMobileSynch.Text = "Mobile Synch (old-style)";
			// 
			// checkTroubleshooting
			// 
			this.checkTroubleshooting.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.checkTroubleshooting.Location = new System.Drawing.Point(636, 230);
			this.checkTroubleshooting.Name = "checkTroubleshooting";
			this.checkTroubleshooting.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkTroubleshooting.Size = new System.Drawing.Size(184, 24);
			this.checkTroubleshooting.TabIndex = 254;
			this.checkTroubleshooting.Text = "Synch Troubleshooting Mode";
			this.checkTroubleshooting.UseVisualStyleBackColor = true;
			// 
			// butDelete
			// 
			this.butDelete.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.butDelete.Location = new System.Drawing.Point(504, 279);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(68, 24);
			this.butDelete.TabIndex = 253;
			this.butDelete.Text = "Delete All";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textDateTimeLastRun
			// 
			this.textDateTimeLastRun.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.textDateTimeLastRun.Location = new System.Drawing.Point(505, 230);
			this.textDateTimeLastRun.Name = "textDateTimeLastRun";
			this.textDateTimeLastRun.Size = new System.Drawing.Size(207, 18);
			this.textDateTimeLastRun.TabIndex = 252;
			this.textDateTimeLastRun.Text = "3/4/2011 4:15 PM";
			this.textDateTimeLastRun.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupPreferences
			// 
			this.groupPreferences.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupPreferences.Controls.Add(this.label13);
			this.groupPreferences.Controls.Add(this.label14);
			this.groupPreferences.Controls.Add(this.textMobileUserName);
			this.groupPreferences.Controls.Add(this.label15);
			this.groupPreferences.Controls.Add(this.butCurrentWorkstation);
			this.groupPreferences.Controls.Add(this.textMobilePassword);
			this.groupPreferences.Controls.Add(this.label16);
			this.groupPreferences.Controls.Add(this.label17);
			this.groupPreferences.Controls.Add(this.textMobileSynchWorkStation);
			this.groupPreferences.Controls.Add(this.textSynchMinutes);
			this.groupPreferences.Controls.Add(this.label18);
			this.groupPreferences.Controls.Add(this.butSaveMobileSynch);
			this.groupPreferences.Controls.Add(this.textDateBefore);
			this.groupPreferences.Controls.Add(this.labelMobileSynchURL);
			this.groupPreferences.Controls.Add(this.textMobileSyncServerURL);
			this.groupPreferences.Controls.Add(this.labelMinutesBetweenSynch);
			this.groupPreferences.Location = new System.Drawing.Point(236, 7);
			this.groupPreferences.Name = "groupPreferences";
			this.groupPreferences.Size = new System.Drawing.Size(682, 212);
			this.groupPreferences.TabIndex = 251;
			this.groupPreferences.TabStop = false;
			this.groupPreferences.Text = "Preferences";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(8, 183);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(575, 19);
			this.label13.TabIndex = 246;
			this.label13.Text = "To change your password, enter a new one in the box and Save.  To keep the old pa" +
    "ssword, leave the box empty.";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(222, 48);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(343, 18);
			this.label14.TabIndex = 244;
			this.label14.Text = "Set to 0 to stop automatic Synchronization";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textMobileUserName
			// 
			this.textMobileUserName.Location = new System.Drawing.Point(177, 131);
			this.textMobileUserName.Name = "textMobileUserName";
			this.textMobileUserName.Size = new System.Drawing.Size(247, 20);
			this.textMobileUserName.TabIndex = 242;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(5, 132);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(169, 19);
			this.label15.TabIndex = 243;
			this.label15.Text = "User Name";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butCurrentWorkstation
			// 
			this.butCurrentWorkstation.Location = new System.Drawing.Point(430, 101);
			this.butCurrentWorkstation.Name = "butCurrentWorkstation";
			this.butCurrentWorkstation.Size = new System.Drawing.Size(115, 24);
			this.butCurrentWorkstation.TabIndex = 247;
			this.butCurrentWorkstation.Text = "Current Workstation";
			this.butCurrentWorkstation.Click += new System.EventHandler(this.butCurrentWorkstation_Click);
			// 
			// textMobilePassword
			// 
			this.textMobilePassword.Location = new System.Drawing.Point(177, 159);
			this.textMobilePassword.Name = "textMobilePassword";
			this.textMobilePassword.PasswordChar = '*';
			this.textMobilePassword.Size = new System.Drawing.Size(247, 20);
			this.textMobilePassword.TabIndex = 243;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(4, 105);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(170, 18);
			this.label16.TabIndex = 246;
			this.label16.Text = "Workstation for Synching";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(5, 160);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(169, 19);
			this.label17.TabIndex = 244;
			this.label17.Text = "Password";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMobileSynchWorkStation
			// 
			this.textMobileSynchWorkStation.Location = new System.Drawing.Point(177, 103);
			this.textMobileSynchWorkStation.Name = "textMobileSynchWorkStation";
			this.textMobileSynchWorkStation.Size = new System.Drawing.Size(247, 20);
			this.textMobileSynchWorkStation.TabIndex = 245;
			// 
			// textSynchMinutes
			// 
			this.textSynchMinutes.Location = new System.Drawing.Point(177, 47);
			this.textSynchMinutes.MaxVal = 255;
			this.textSynchMinutes.MinVal = 0;
			this.textSynchMinutes.Name = "textSynchMinutes";
			this.textSynchMinutes.Size = new System.Drawing.Size(39, 20);
			this.textSynchMinutes.TabIndex = 241;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(5, 76);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(170, 18);
			this.label18.TabIndex = 85;
			this.label18.Text = "Exclude Appointments Before";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butSaveMobileSynch
			// 
			this.butSaveMobileSynch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSaveMobileSynch.Location = new System.Drawing.Point(615, 182);
			this.butSaveMobileSynch.Name = "butSaveMobileSynch";
			this.butSaveMobileSynch.Size = new System.Drawing.Size(61, 24);
			this.butSaveMobileSynch.TabIndex = 240;
			this.butSaveMobileSynch.Text = "Save";
			this.butSaveMobileSynch.Click += new System.EventHandler(this.butSaveMobileSynch_Click);
			// 
			// textDateBefore
			// 
			this.textDateBefore.Location = new System.Drawing.Point(177, 75);
			this.textDateBefore.Name = "textDateBefore";
			this.textDateBefore.Size = new System.Drawing.Size(100, 20);
			this.textDateBefore.TabIndex = 84;
			// 
			// labelMobileSynchURL
			// 
			this.labelMobileSynchURL.Location = new System.Drawing.Point(6, 20);
			this.labelMobileSynchURL.Name = "labelMobileSynchURL";
			this.labelMobileSynchURL.Size = new System.Drawing.Size(169, 19);
			this.labelMobileSynchURL.TabIndex = 76;
			this.labelMobileSynchURL.Text = "Host Server Address";
			this.labelMobileSynchURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMobileSyncServerURL
			// 
			this.textMobileSyncServerURL.Location = new System.Drawing.Point(177, 19);
			this.textMobileSyncServerURL.Name = "textMobileSyncServerURL";
			this.textMobileSyncServerURL.Size = new System.Drawing.Size(445, 20);
			this.textMobileSyncServerURL.TabIndex = 75;
			// 
			// labelMinutesBetweenSynch
			// 
			this.labelMinutesBetweenSynch.Location = new System.Drawing.Point(6, 48);
			this.labelMinutesBetweenSynch.Name = "labelMinutesBetweenSynch";
			this.labelMinutesBetweenSynch.Size = new System.Drawing.Size(169, 19);
			this.labelMinutesBetweenSynch.TabIndex = 79;
			this.labelMinutesBetweenSynch.Text = "Minutes Between Synch";
			this.labelMinutesBetweenSynch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label19
			// 
			this.label19.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label19.Location = new System.Drawing.Point(335, 230);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(167, 18);
			this.label19.TabIndex = 250;
			this.label19.Text = "Date/time of last sync";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butFullSync
			// 
			this.butFullSync.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.butFullSync.Location = new System.Drawing.Point(578, 279);
			this.butFullSync.Name = "butFullSync";
			this.butFullSync.Size = new System.Drawing.Size(68, 24);
			this.butFullSync.TabIndex = 249;
			this.butFullSync.Text = "Full Synch";
			this.butFullSync.Click += new System.EventHandler(this.butFullSync_Click);
			// 
			// butSync
			// 
			this.butSync.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.butSync.Location = new System.Drawing.Point(652, 279);
			this.butSync.Name = "butSync";
			this.butSync.Size = new System.Drawing.Size(68, 24);
			this.butSync.TabIndex = 248;
			this.butSync.Text = "Synch";
			this.butSync.Click += new System.EventHandler(this.butSync_Click);
			// 
			// tabMobileWeb
			// 
			this.tabMobileWeb.BackColor = System.Drawing.SystemColors.Control;
			this.tabMobileWeb.Controls.Add(this.groupBox5);
			this.tabMobileWeb.Controls.Add(this.groupBox2);
			this.tabMobileWeb.Location = new System.Drawing.Point(4, 22);
			this.tabMobileWeb.Name = "tabMobileWeb";
			this.tabMobileWeb.Padding = new System.Windows.Forms.Padding(3);
			this.tabMobileWeb.Size = new System.Drawing.Size(1154, 595);
			this.tabMobileWeb.TabIndex = 10;
			this.tabMobileWeb.Text = "Mobile Web";
			// 
			// groupBox5
			// 
			this.groupBox5.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox5.Controls.Add(this.butSetupMobileWebUsers);
			this.groupBox5.Controls.Add(this.label29);
			this.groupBox5.Location = new System.Drawing.Point(153, 101);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(848, 97);
			this.groupBox5.TabIndex = 79;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Setup Users";
			// 
			// butSetupMobileWebUsers
			// 
			this.butSetupMobileWebUsers.Location = new System.Drawing.Point(9, 67);
			this.butSetupMobileWebUsers.Name = "butSetupMobileWebUsers";
			this.butSetupMobileWebUsers.Size = new System.Drawing.Size(220, 24);
			this.butSetupMobileWebUsers.TabIndex = 250;
			this.butSetupMobileWebUsers.Text = "Setup Mobile Web Users";
			this.butSetupMobileWebUsers.Click += new System.EventHandler(this.butSetupMobileWebUsers_Click);
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(6, 16);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(664, 42);
			this.label29.TabIndex = 72;
			this.label29.Text = resources.GetString("label29.Text");
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox2.Controls.Add(this.textHostedUrlMobileWeb);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Location = new System.Drawing.Point(153, 6);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(848, 89);
			this.groupBox2.TabIndex = 78;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Mobile Web";
			// 
			// textHostedUrlMobileWeb
			// 
			this.textHostedUrlMobileWeb.Location = new System.Drawing.Point(144, 61);
			this.textHostedUrlMobileWeb.Name = "textHostedUrlMobileWeb";
			this.textHostedUrlMobileWeb.ReadOnly = true;
			this.textHostedUrlMobileWeb.Size = new System.Drawing.Size(349, 20);
			this.textHostedUrlMobileWeb.TabIndex = 74;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(12, 63);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(126, 17);
			this.label12.TabIndex = 73;
			this.label12.Text = "Hosted URL";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(6, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(664, 42);
			this.label5.TabIndex = 72;
			this.label5.Text = resources.GetString("label5.Text");
			// 
			// tabPatientPortal
			// 
			this.tabPatientPortal.BackColor = System.Drawing.SystemColors.Control;
			this.tabPatientPortal.Controls.Add(this.groupBox4);
			this.tabPatientPortal.Controls.Add(this.groupPatientPortalInvites);
			this.tabPatientPortal.Controls.Add(this.groupBoxNotification);
			this.tabPatientPortal.Controls.Add(this.groupBox1);
			this.tabPatientPortal.Location = new System.Drawing.Point(4, 22);
			this.tabPatientPortal.Name = "tabPatientPortal";
			this.tabPatientPortal.Padding = new System.Windows.Forms.Padding(3);
			this.tabPatientPortal.Size = new System.Drawing.Size(1154, 595);
			this.tabPatientPortal.TabIndex = 1;
			this.tabPatientPortal.Text = "Patient Portal";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.comboPPClinicUrl);
			this.groupBox4.Controls.Add(this.label39);
			this.groupBox4.Controls.Add(this.radioPatientPortalPayment);
			this.groupBox4.Controls.Add(this.radioPatientPortalLogin);
			this.groupBox4.Controls.Add(this.butCopyToClipboard);
			this.groupBox4.Controls.Add(this.textHostedUrlPortal);
			this.groupBox4.Controls.Add(this.butNavigateTo);
			this.groupBox4.Controls.Add(this.label2);
			this.groupBox4.Controls.Add(this.label3);
			this.groupBox4.Location = new System.Drawing.Point(6, 109);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(536, 203);
			this.groupBox4.TabIndex = 51;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Construct Hosted URL";
			// 
			// comboPPClinicUrl
			// 
			this.comboPPClinicUrl.HqDescription = "None";
			this.comboPPClinicUrl.IncludeUnassigned = true;
			this.comboPPClinicUrl.Location = new System.Drawing.Point(88, 168);
			this.comboPPClinicUrl.Name = "comboPPClinicUrl";
			this.comboPPClinicUrl.Size = new System.Drawing.Size(194, 21);
			this.comboPPClinicUrl.TabIndex = 269;
			this.comboPPClinicUrl.SelectedIndexChanged += new System.EventHandler(this.comboPPClinicUrl_SelectedIndexChanged);
			// 
			// label39
			// 
			this.label39.Location = new System.Drawing.Point(15, 143);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(104, 17);
			this.label39.TabIndex = 277;
			this.label39.Text = "Destination";
			this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// radioPatientPortalPayment
			// 
			this.radioPatientPortalPayment.BackColor = System.Drawing.Color.Transparent;
			this.radioPatientPortalPayment.Location = new System.Drawing.Point(259, 143);
			this.radioPatientPortalPayment.Name = "radioPatientPortalPayment";
			this.radioPatientPortalPayment.Size = new System.Drawing.Size(163, 17);
			this.radioPatientPortalPayment.TabIndex = 275;
			this.radioPatientPortalPayment.Text = "Make Payment";
			this.radioPatientPortalPayment.UseVisualStyleBackColor = false;
			this.radioPatientPortalPayment.CheckedChanged += new System.EventHandler(this.radioPatientPortal_CheckedChanged);
			// 
			// radioPatientPortalLogin
			// 
			this.radioPatientPortalLogin.Checked = true;
			this.radioPatientPortalLogin.Location = new System.Drawing.Point(125, 143);
			this.radioPatientPortalLogin.Name = "radioPatientPortalLogin";
			this.radioPatientPortalLogin.Size = new System.Drawing.Size(128, 17);
			this.radioPatientPortalLogin.TabIndex = 274;
			this.radioPatientPortalLogin.TabStop = true;
			this.radioPatientPortalLogin.Text = "Login Page";
			this.radioPatientPortalLogin.UseVisualStyleBackColor = true;
			this.radioPatientPortalLogin.CheckedChanged += new System.EventHandler(this.radioPatientPortal_CheckedChanged);
			// 
			// butCopyToClipboard
			// 
			this.butCopyToClipboard.Location = new System.Drawing.Point(245, 107);
			this.butCopyToClipboard.Name = "butCopyToClipboard";
			this.butCopyToClipboard.Size = new System.Drawing.Size(115, 24);
			this.butCopyToClipboard.TabIndex = 269;
			this.butCopyToClipboard.Text = "Copy to Clipboard";
			this.butCopyToClipboard.Click += new System.EventHandler(this.butCopyToClipboard_Click);
			// 
			// textHostedUrlPortal
			// 
			this.textHostedUrlPortal.Location = new System.Drawing.Point(122, 81);
			this.textHostedUrlPortal.Name = "textHostedUrlPortal";
			this.textHostedUrlPortal.ReadOnly = true;
			this.textHostedUrlPortal.Size = new System.Drawing.Size(397, 20);
			this.textHostedUrlPortal.TabIndex = 46;
			// 
			// butNavigateTo
			// 
			this.butNavigateTo.Location = new System.Drawing.Point(122, 107);
			this.butNavigateTo.Name = "butNavigateTo";
			this.butNavigateTo.Size = new System.Drawing.Size(117, 24);
			this.butNavigateTo.TabIndex = 270;
			this.butNavigateTo.Text = "Navigate to URL";
			this.butNavigateTo.Click += new System.EventHandler(this.butNavigateTo_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(9, 82);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(110, 17);
			this.label2.TabIndex = 44;
			this.label2.Text = "Hosted URL";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(15, 27);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(504, 51);
			this.label3.TabIndex = 45;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// groupPatientPortalInvites
			// 
			this.groupPatientPortalInvites.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupPatientPortalInvites.Controls.Add(this.textStatusInvites);
			this.groupPatientPortalInvites.Controls.Add(this.butActivateInvites);
			this.groupPatientPortalInvites.Controls.Add(this.comboClinicsPPInvites);
			this.groupPatientPortalInvites.Controls.Add(this.checkUseDefaultsPPInvites);
			this.groupPatientPortalInvites.Controls.Add(this.butAddPPInviteRule);
			this.groupPatientPortalInvites.Controls.Add(this.gridPatPortalInviteRules);
			this.groupPatientPortalInvites.Controls.Add(this.checkIsPPInvitesEnabled);
			this.groupPatientPortalInvites.Location = new System.Drawing.Point(548, 6);
			this.groupPatientPortalInvites.Name = "groupPatientPortalInvites";
			this.groupPatientPortalInvites.Size = new System.Drawing.Size(600, 576);
			this.groupPatientPortalInvites.TabIndex = 50;
			this.groupPatientPortalInvites.TabStop = false;
			this.groupPatientPortalInvites.Text = "Patient Portal Invites";
			// 
			// textStatusInvites
			// 
			this.textStatusInvites.Location = new System.Drawing.Point(51, 48);
			this.textStatusInvites.Name = "textStatusInvites";
			this.textStatusInvites.ReadOnly = true;
			this.textStatusInvites.Size = new System.Drawing.Size(147, 20);
			this.textStatusInvites.TabIndex = 268;
			this.textStatusInvites.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// butActivateInvites
			// 
			this.butActivateInvites.Location = new System.Drawing.Point(51, 19);
			this.butActivateInvites.Name = "butActivateInvites";
			this.butActivateInvites.Size = new System.Drawing.Size(147, 23);
			this.butActivateInvites.TabIndex = 267;
			this.butActivateInvites.Text = "Activate Invites";
			this.butActivateInvites.UseVisualStyleBackColor = true;
			this.butActivateInvites.Click += new System.EventHandler(this.butActivateInvites_Click);
			// 
			// comboClinicsPPInvites
			// 
			this.comboClinicsPPInvites.HqDescription = "Defaults";
			this.comboClinicsPPInvites.IncludeUnassigned = true;
			this.comboClinicsPPInvites.Location = new System.Drawing.Point(5, 85);
			this.comboClinicsPPInvites.Name = "comboClinicsPPInvites";
			this.comboClinicsPPInvites.Size = new System.Drawing.Size(193, 21);
			this.comboClinicsPPInvites.TabIndex = 266;
			this.comboClinicsPPInvites.SelectionChangeCommitted += new System.EventHandler(this.comboClinicsPPInvites_SelectionChangeCommitted);
			// 
			// checkUseDefaultsPPInvites
			// 
			this.checkUseDefaultsPPInvites.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUseDefaultsPPInvites.Location = new System.Drawing.Point(5, 132);
			this.checkUseDefaultsPPInvites.Name = "checkUseDefaultsPPInvites";
			this.checkUseDefaultsPPInvites.Size = new System.Drawing.Size(105, 19);
			this.checkUseDefaultsPPInvites.TabIndex = 265;
			this.checkUseDefaultsPPInvites.Text = "Use Defaults";
			this.checkUseDefaultsPPInvites.CheckedChanged += new System.EventHandler(this.checkUseDefaultsPPInvites_CheckedChanged);
			// 
			// butAddPPInviteRule
			// 
			this.butAddPPInviteRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAddPPInviteRule.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddPPInviteRule.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddPPInviteRule.Location = new System.Drawing.Point(100, 546);
			this.butAddPPInviteRule.Name = "butAddPPInviteRule";
			this.butAddPPInviteRule.Size = new System.Drawing.Size(98, 24);
			this.butAddPPInviteRule.TabIndex = 264;
			this.butAddPPInviteRule.Text = "Add  Invite";
			this.butAddPPInviteRule.UseVisualStyleBackColor = true;
			this.butAddPPInviteRule.Click += new System.EventHandler(this.butAddPPInviteRule_Click);
			// 
			// gridPatPortalInviteRules
			// 
			this.gridPatPortalInviteRules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridPatPortalInviteRules.HasMultilineHeaders = true;
			this.gridPatPortalInviteRules.Location = new System.Drawing.Point(206, 12);
			this.gridPatPortalInviteRules.Name = "gridPatPortalInviteRules";
			this.gridPatPortalInviteRules.Size = new System.Drawing.Size(379, 558);
			this.gridPatPortalInviteRules.TabIndex = 171;
			this.gridPatPortalInviteRules.Title = "Patient Portal Invite Rules";
			this.gridPatPortalInviteRules.TranslationName = "TableInviteRules";
			this.gridPatPortalInviteRules.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPatPortalInviteRules_CellDoubleClick);
			// 
			// checkIsPPInvitesEnabled
			// 
			this.checkIsPPInvitesEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsPPInvitesEnabled.Location = new System.Drawing.Point(5, 112);
			this.checkIsPPInvitesEnabled.Name = "checkIsPPInvitesEnabled";
			this.checkIsPPInvitesEnabled.Size = new System.Drawing.Size(186, 19);
			this.checkIsPPInvitesEnabled.TabIndex = 170;
			this.checkIsPPInvitesEnabled.Text = "Enable Invites for Clinic";
			// 
			// groupBoxNotification
			// 
			this.groupBoxNotification.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBoxNotification.Controls.Add(this.butEditWebMailNotificationBody);
			this.groupBoxNotification.Controls.Add(this.browserWebMailNotificatonBody);
			this.groupBoxNotification.Controls.Add(this.label9);
			this.groupBoxNotification.Controls.Add(this.label7);
			this.groupBoxNotification.Controls.Add(this.textBoxNotificationSubject);
			this.groupBoxNotification.Controls.Add(this.label6);
			this.groupBoxNotification.Controls.Add(this.label4);
			this.groupBoxNotification.Location = new System.Drawing.Point(6, 318);
			this.groupBoxNotification.Name = "groupBoxNotification";
			this.groupBoxNotification.Size = new System.Drawing.Size(536, 264);
			this.groupBoxNotification.TabIndex = 48;
			this.groupBoxNotification.TabStop = false;
			this.groupBoxNotification.Text = "Notification Email";
			// 
			// butEditWebMailNotificationBody
			// 
			this.butEditWebMailNotificationBody.Location = new System.Drawing.Point(460, 239);
			this.butEditWebMailNotificationBody.Name = "butEditWebMailNotificationBody";
			this.butEditWebMailNotificationBody.Size = new System.Drawing.Size(70, 20);
			this.butEditWebMailNotificationBody.TabIndex = 320;
			this.butEditWebMailNotificationBody.Text = "Edit";
			this.butEditWebMailNotificationBody.UseVisualStyleBackColor = true;
			this.butEditWebMailNotificationBody.Click += new System.EventHandler(this.butEditWebMailNotificationBody_Click);
			// 
			// browserWebMailNotificatonBody
			// 
			this.browserWebMailNotificatonBody.AllowWebBrowserDrop = false;
			this.browserWebMailNotificatonBody.Location = new System.Drawing.Point(93, 120);
			this.browserWebMailNotificatonBody.MinimumSize = new System.Drawing.Size(20, 20);
			this.browserWebMailNotificatonBody.Name = "browserWebMailNotificatonBody";
			this.browserWebMailNotificatonBody.Size = new System.Drawing.Size(426, 114);
			this.browserWebMailNotificatonBody.TabIndex = 319;
			this.browserWebMailNotificatonBody.WebBrowserShortcutsEnabled = false;
			// 
			// label9
			// 
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label9.Location = new System.Drawing.Point(15, 16);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(504, 56);
			this.label9.TabIndex = 52;
			this.label9.Text = resources.GetString("label9.Text");
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(90, 100);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(440, 15);
			this.label7.TabIndex = 48;
			this.label7.Text = "[URL] will be replaced with the value of \'Patient Facing URL\' as entered above.";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBoxNotificationSubject
			// 
			this.textBoxNotificationSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxNotificationSubject.Location = new System.Drawing.Point(93, 75);
			this.textBoxNotificationSubject.Name = "textBoxNotificationSubject";
			this.textBoxNotificationSubject.Size = new System.Drawing.Size(426, 20);
			this.textBoxNotificationSubject.TabIndex = 45;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(12, 120);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(75, 17);
			this.label6.TabIndex = 47;
			this.label6.Text = "Body";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(9, 76);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(78, 17);
			this.label4.TabIndex = 44;
			this.label4.Text = "Subject";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.textPatientFacingUrlPortal);
			this.groupBox1.Location = new System.Drawing.Point(6, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(536, 97);
			this.groupBox1.TabIndex = 49;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Patient Facing URL";
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label1.Location = new System.Drawing.Point(15, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(504, 30);
			this.label1.TabIndex = 51;
			this.label1.Text = "This will be the link that patients will use to reach your office\'s patient porta" +
    "l. This is also the URL that will be on the printout given to patients.";
			// 
			// label8
			// 
			this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label8.Location = new System.Drawing.Point(-143, 129);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(129, 17);
			this.label8.TabIndex = 52;
			this.label8.Text = "Patient Facing URL";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPatientFacingUrlPortal
			// 
			this.textPatientFacingUrlPortal.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.textPatientFacingUrlPortal.Location = new System.Drawing.Point(18, 56);
			this.textPatientFacingUrlPortal.Name = "textPatientFacingUrlPortal";
			this.textPatientFacingUrlPortal.Size = new System.Drawing.Size(501, 20);
			this.textPatientFacingUrlPortal.TabIndex = 50;
			// 
			// tabWebSched
			// 
			this.tabWebSched.BackColor = System.Drawing.SystemColors.Control;
			this.tabWebSched.Controls.Add(this.linkLabelAboutWebSched);
			this.tabWebSched.Controls.Add(this.labelWebSchedDesc);
			this.tabWebSched.Controls.Add(this.tabControlWebSched);
			this.tabWebSched.Location = new System.Drawing.Point(4, 22);
			this.tabWebSched.Name = "tabWebSched";
			this.tabWebSched.Size = new System.Drawing.Size(1154, 595);
			this.tabWebSched.TabIndex = 3;
			this.tabWebSched.Text = "Web Sched";
			// 
			// linkLabelAboutWebSched
			// 
			this.linkLabelAboutWebSched.Location = new System.Drawing.Point(823, 0);
			this.linkLabelAboutWebSched.Name = "linkLabelAboutWebSched";
			this.linkLabelAboutWebSched.Size = new System.Drawing.Size(31, 28);
			this.linkLabelAboutWebSched.TabIndex = 303;
			this.linkLabelAboutWebSched.TabStop = true;
			this.linkLabelAboutWebSched.Text = "help";
			this.linkLabelAboutWebSched.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.linkLabelAboutWebSched.Click += new System.EventHandler(this.linkLabelAboutWebSched_Click);
			// 
			// labelWebSchedDesc
			// 
			this.labelWebSchedDesc.Location = new System.Drawing.Point(221, 0);
			this.labelWebSchedDesc.Name = "labelWebSchedDesc";
			this.labelWebSchedDesc.Size = new System.Drawing.Size(602, 28);
			this.labelWebSchedDesc.TabIndex = 52;
			this.labelWebSchedDesc.Text = "Web Sched is a separate service that gives your patients an easy way to schedule " +
    "appointments via the web within seconds.";
			this.labelWebSchedDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tabControlWebSched
			// 
			this.tabControlWebSched.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlWebSched.Controls.Add(this.tabWebSchedRecalls);
			this.tabControlWebSched.Controls.Add(this.tabWebSchedNewPatAppts);
			this.tabControlWebSched.Controls.Add(this.tabWebSchedVerify);
			this.tabControlWebSched.Location = new System.Drawing.Point(3, 17);
			this.tabControlWebSched.Name = "tabControlWebSched";
			this.tabControlWebSched.SelectedIndex = 0;
			this.tabControlWebSched.Size = new System.Drawing.Size(1148, 575);
			this.tabControlWebSched.TabIndex = 302;
			// 
			// tabWebSchedRecalls
			// 
			this.tabWebSchedRecalls.Controls.Add(this.groupWebSchedProvRule);
			this.tabWebSchedRecalls.Controls.Add(this.checkWSRDoubleBooking);
			this.tabWebSchedRecalls.Controls.Add(this.comboWSRConfirmStatus);
			this.tabWebSchedRecalls.Controls.Add(this.label36);
			this.tabWebSchedRecalls.Controls.Add(this.label43);
			this.tabWebSchedRecalls.Controls.Add(this.label44);
			this.tabWebSchedRecalls.Controls.Add(this.textWebSchedRecallApptSearchDays);
			this.tabWebSchedRecalls.Controls.Add(this.checkRecallAllowProvSelection);
			this.tabWebSchedRecalls.Controls.Add(this.groupWebSchedText);
			this.tabWebSchedRecalls.Controls.Add(this.groupBox6);
			this.tabWebSchedRecalls.Controls.Add(this.groupBoxWebSchedAutomation);
			this.tabWebSchedRecalls.Controls.Add(this.groupWebSchedPreview);
			this.tabWebSchedRecalls.Controls.Add(this.gridWebSchedOperatories);
			this.tabWebSchedRecalls.Controls.Add(this.label35);
			this.tabWebSchedRecalls.Controls.Add(this.butRecallSchedSetup);
			this.tabWebSchedRecalls.Controls.Add(this.label31);
			this.tabWebSchedRecalls.Controls.Add(this.gridWebSchedRecallTypes);
			this.tabWebSchedRecalls.Controls.Add(this.label20);
			this.tabWebSchedRecalls.Location = new System.Drawing.Point(4, 22);
			this.tabWebSchedRecalls.Name = "tabWebSchedRecalls";
			this.tabWebSchedRecalls.Padding = new System.Windows.Forms.Padding(3);
			this.tabWebSchedRecalls.Size = new System.Drawing.Size(1140, 549);
			this.tabWebSchedRecalls.TabIndex = 0;
			this.tabWebSchedRecalls.Text = "Recalls";
			this.tabWebSchedRecalls.UseVisualStyleBackColor = true;
			// 
			// groupWebSchedProvRule
			// 
			this.groupWebSchedProvRule.Controls.Add(this.butProvRulePickClinic);
			this.groupWebSchedProvRule.Controls.Add(this.checkUseDefaultProvRule);
			this.groupWebSchedProvRule.Controls.Add(this.comboClinicProvRule);
			this.groupWebSchedProvRule.Controls.Add(this.listBoxWebSchedProviderPref);
			this.groupWebSchedProvRule.Controls.Add(this.label21);
			this.groupWebSchedProvRule.Location = new System.Drawing.Point(114, 426);
			this.groupWebSchedProvRule.Name = "groupWebSchedProvRule";
			this.groupWebSchedProvRule.Size = new System.Drawing.Size(439, 103);
			this.groupWebSchedProvRule.TabIndex = 333;
			this.groupWebSchedProvRule.TabStop = false;
			this.groupWebSchedProvRule.Text = "Provider Rule";
			// 
			// butProvRulePickClinic
			// 
			this.butProvRulePickClinic.Location = new System.Drawing.Point(414, 12);
			this.butProvRulePickClinic.Name = "butProvRulePickClinic";
			this.butProvRulePickClinic.Size = new System.Drawing.Size(18, 21);
			this.butProvRulePickClinic.TabIndex = 314;
			this.butProvRulePickClinic.Text = "...";
			this.butProvRulePickClinic.Click += new System.EventHandler(this.butProvRulePickClinic_Click);
			// 
			// checkUseDefaultProvRule
			// 
			this.checkUseDefaultProvRule.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseDefaultProvRule.Location = new System.Drawing.Point(92, 12);
			this.checkUseDefaultProvRule.Name = "checkUseDefaultProvRule";
			this.checkUseDefaultProvRule.Size = new System.Drawing.Size(104, 24);
			this.checkUseDefaultProvRule.TabIndex = 312;
			this.checkUseDefaultProvRule.Text = "Use Defaults";
			this.checkUseDefaultProvRule.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseDefaultProvRule.UseVisualStyleBackColor = true;
			this.checkUseDefaultProvRule.Visible = false;
			this.checkUseDefaultProvRule.Click += new System.EventHandler(this.checkUseDefaultProvRule_Click);
			// 
			// comboClinicProvRule
			// 
			this.comboClinicProvRule.HqDescription = "Defaults";
			this.comboClinicProvRule.IncludeUnassigned = true;
			this.comboClinicProvRule.Location = new System.Drawing.Point(209, 12);
			this.comboClinicProvRule.Name = "comboClinicProvRule";
			this.comboClinicProvRule.Size = new System.Drawing.Size(200, 21);
			this.comboClinicProvRule.TabIndex = 311;
			this.comboClinicProvRule.SelectionChangeCommitted += new System.EventHandler(this.comboClinicProvRule_SelectionChangeCommitted);
			// 
			// listBoxWebSchedProviderPref
			// 
			this.listBoxWebSchedProviderPref.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.listBoxWebSchedProviderPref.FormattingEnabled = true;
			this.listBoxWebSchedProviderPref.Items.AddRange(new object[] {
            "First Available",
            "Primary Provider",
            "Secondary Provider",
            "Last Seen Hygienist"});
			this.listBoxWebSchedProviderPref.Location = new System.Drawing.Point(313, 41);
			this.listBoxWebSchedProviderPref.Name = "listBoxWebSchedProviderPref";
			this.listBoxWebSchedProviderPref.Size = new System.Drawing.Size(120, 56);
			this.listBoxWebSchedProviderPref.TabIndex = 309;
			this.listBoxWebSchedProviderPref.SelectedIndexChanged += new System.EventHandler(this.listBoxWebSchedProviderPref_SelectedIndexChanged);
			// 
			// label21
			// 
			this.label21.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label21.Location = new System.Drawing.Point(11, 43);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(301, 56);
			this.label21.TabIndex = 310;
			this.label21.Text = resources.GetString("label21.Text");
			this.label21.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkWSRDoubleBooking
			// 
			this.checkWSRDoubleBooking.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWSRDoubleBooking.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkWSRDoubleBooking.Location = new System.Drawing.Point(115, 529);
			this.checkWSRDoubleBooking.Name = "checkWSRDoubleBooking";
			this.checkWSRDoubleBooking.Size = new System.Drawing.Size(216, 18);
			this.checkWSRDoubleBooking.TabIndex = 329;
			this.checkWSRDoubleBooking.Text = "Prevent double booking";
			this.checkWSRDoubleBooking.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboWSRConfirmStatus
			// 
			this.comboWSRConfirmStatus.Location = new System.Drawing.Point(820, 8);
			this.comboWSRConfirmStatus.Name = "comboWSRConfirmStatus";
			this.comboWSRConfirmStatus.Size = new System.Drawing.Size(191, 21);
			this.comboWSRConfirmStatus.TabIndex = 328;
			// 
			// label36
			// 
			this.label36.Location = new System.Drawing.Point(593, 10);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(221, 17);
			this.label36.TabIndex = 327;
			this.label36.Text = "Web Sched Recall Confirm Status";
			this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label43
			// 
			this.label43.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label43.Location = new System.Drawing.Point(284, 8);
			this.label43.Name = "label43";
			this.label43.Size = new System.Drawing.Size(252, 17);
			this.label43.TabIndex = 332;
			this.label43.Text = "days.  Empty includes all possible openings.";
			this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label44
			// 
			this.label44.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label44.Location = new System.Drawing.Point(70, 8);
			this.label44.Name = "label44";
			this.label44.Size = new System.Drawing.Size(167, 17);
			this.label44.TabIndex = 330;
			this.label44.Text = "Search for openings after";
			this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWebSchedRecallApptSearchDays
			// 
			this.textWebSchedRecallApptSearchDays.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.textWebSchedRecallApptSearchDays.Location = new System.Drawing.Point(243, 7);
			this.textWebSchedRecallApptSearchDays.MaxVal = 365;
			this.textWebSchedRecallApptSearchDays.MinVal = 0;
			this.textWebSchedRecallApptSearchDays.Name = "textWebSchedRecallApptSearchDays";
			this.textWebSchedRecallApptSearchDays.Size = new System.Drawing.Size(38, 20);
			this.textWebSchedRecallApptSearchDays.TabIndex = 331;
			this.textWebSchedRecallApptSearchDays.Leave += new System.EventHandler(this.textWebSchedRecallApptSearchDays_Leave);
			this.textWebSchedRecallApptSearchDays.Validated += new System.EventHandler(this.textWebSchedRecallApptSearchDays_Validated);
			// 
			// checkRecallAllowProvSelection
			// 
			this.checkRecallAllowProvSelection.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRecallAllowProvSelection.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRecallAllowProvSelection.Location = new System.Drawing.Point(337, 528);
			this.checkRecallAllowProvSelection.Name = "checkRecallAllowProvSelection";
			this.checkRecallAllowProvSelection.Size = new System.Drawing.Size(216, 18);
			this.checkRecallAllowProvSelection.TabIndex = 314;
			this.checkRecallAllowProvSelection.Text = "Allow patients to select provider";
			this.checkRecallAllowProvSelection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupWebSchedText
			// 
			this.groupWebSchedText.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupWebSchedText.Controls.Add(this.labelWebSchedPerBatch);
			this.groupWebSchedText.Controls.Add(this.textWebSchedPerBatch);
			this.groupWebSchedText.Controls.Add(this.radioDoNotSendText);
			this.groupWebSchedText.Controls.Add(this.radioSendText);
			this.groupWebSchedText.Location = new System.Drawing.Point(576, 489);
			this.groupWebSchedText.Name = "groupWebSchedText";
			this.groupWebSchedText.Size = new System.Drawing.Size(484, 55);
			this.groupWebSchedText.TabIndex = 313;
			this.groupWebSchedText.TabStop = false;
			this.groupWebSchedText.Text = "Send Text Messages Automatically To";
			// 
			// labelWebSchedPerBatch
			// 
			this.labelWebSchedPerBatch.Location = new System.Drawing.Point(288, 9);
			this.labelWebSchedPerBatch.Name = "labelWebSchedPerBatch";
			this.labelWebSchedPerBatch.Size = new System.Drawing.Size(136, 40);
			this.labelWebSchedPerBatch.TabIndex = 314;
			this.labelWebSchedPerBatch.Text = "Max number of texts sent every 10 minutes per clinic";
			this.labelWebSchedPerBatch.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textWebSchedPerBatch
			// 
			this.textWebSchedPerBatch.Location = new System.Drawing.Point(426, 28);
			this.textWebSchedPerBatch.MaxVal = 100000000;
			this.textWebSchedPerBatch.MinVal = 1;
			this.textWebSchedPerBatch.Name = "textWebSchedPerBatch";
			this.textWebSchedPerBatch.Size = new System.Drawing.Size(39, 20);
			this.textWebSchedPerBatch.TabIndex = 242;
			// 
			// radioDoNotSendText
			// 
			this.radioDoNotSendText.Location = new System.Drawing.Point(7, 16);
			this.radioDoNotSendText.Name = "radioDoNotSendText";
			this.radioDoNotSendText.Size = new System.Drawing.Size(229, 16);
			this.radioDoNotSendText.TabIndex = 77;
			this.radioDoNotSendText.Text = "Do Not Send";
			this.radioDoNotSendText.UseVisualStyleBackColor = true;
			this.radioDoNotSendText.CheckedChanged += new System.EventHandler(this.WebSchedRecallAutoSendRadioButtons_CheckedChanged);
			// 
			// radioSendText
			// 
			this.radioSendText.Location = new System.Drawing.Point(7, 32);
			this.radioSendText.Name = "radioSendText";
			this.radioSendText.Size = new System.Drawing.Size(278, 18);
			this.radioSendText.TabIndex = 0;
			this.radioSendText.Text = "Patients with wireless phone (unless \'Text OK\' = No)";
			this.radioSendText.UseVisualStyleBackColor = true;
			this.radioSendText.CheckedChanged += new System.EventHandler(this.WebSchedRecallAutoSendRadioButtons_CheckedChanged);
			// 
			// groupBox6
			// 
			this.groupBox6.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox6.Controls.Add(this.listboxWebSchedRecallIgnoreBlockoutTypes);
			this.groupBox6.Controls.Add(this.label32);
			this.groupBox6.Controls.Add(this.butWebSchedRecallBlockouts);
			this.groupBox6.Location = new System.Drawing.Point(669, 294);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(339, 103);
			this.groupBox6.TabIndex = 312;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Ignore Blockout Types";
			// 
			// listboxWebSchedRecallIgnoreBlockoutTypes
			// 
			this.listboxWebSchedRecallIgnoreBlockoutTypes.FormattingEnabled = true;
			this.listboxWebSchedRecallIgnoreBlockoutTypes.Location = new System.Drawing.Point(213, 13);
			this.listboxWebSchedRecallIgnoreBlockoutTypes.Name = "listboxWebSchedRecallIgnoreBlockoutTypes";
			this.listboxWebSchedRecallIgnoreBlockoutTypes.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listboxWebSchedRecallIgnoreBlockoutTypes.Size = new System.Drawing.Size(120, 82);
			this.listboxWebSchedRecallIgnoreBlockoutTypes.TabIndex = 197;
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(6, 14);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(206, 20);
			this.label32.TabIndex = 223;
			this.label32.Text = "Currently Ignored Blockout Types";
			this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butWebSchedRecallBlockouts
			// 
			this.butWebSchedRecallBlockouts.Location = new System.Drawing.Point(144, 72);
			this.butWebSchedRecallBlockouts.Name = "butWebSchedRecallBlockouts";
			this.butWebSchedRecallBlockouts.Size = new System.Drawing.Size(68, 23);
			this.butWebSchedRecallBlockouts.TabIndex = 197;
			this.butWebSchedRecallBlockouts.Text = "Edit";
			this.butWebSchedRecallBlockouts.Click += new System.EventHandler(this.butWebSchedRecallBlockouts_Click);
			// 
			// groupBoxWebSchedAutomation
			// 
			this.groupBoxWebSchedAutomation.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBoxWebSchedAutomation.Controls.Add(this.radioSendToEmailNoPreferred);
			this.groupBoxWebSchedAutomation.Controls.Add(this.radioDoNotSend);
			this.groupBoxWebSchedAutomation.Controls.Add(this.radioSendToEmailOnlyPreferred);
			this.groupBoxWebSchedAutomation.Controls.Add(this.radioSendToEmail);
			this.groupBoxWebSchedAutomation.Location = new System.Drawing.Point(576, 401);
			this.groupBoxWebSchedAutomation.Name = "groupBoxWebSchedAutomation";
			this.groupBoxWebSchedAutomation.Size = new System.Drawing.Size(484, 84);
			this.groupBoxWebSchedAutomation.TabIndex = 73;
			this.groupBoxWebSchedAutomation.TabStop = false;
			this.groupBoxWebSchedAutomation.Text = "Send Email Messages Automatically To";
			// 
			// radioSendToEmailNoPreferred
			// 
			this.radioSendToEmailNoPreferred.Location = new System.Drawing.Point(7, 47);
			this.radioSendToEmailNoPreferred.Name = "radioSendToEmailNoPreferred";
			this.radioSendToEmailNoPreferred.Size = new System.Drawing.Size(438, 18);
			this.radioSendToEmailNoPreferred.TabIndex = 1;
			this.radioSendToEmailNoPreferred.Text = "Patients with email address and no other preferred recall method is selected.";
			this.radioSendToEmailNoPreferred.UseVisualStyleBackColor = true;
			this.radioSendToEmailNoPreferred.CheckedChanged += new System.EventHandler(this.WebSchedRecallAutoSendRadioButtons_CheckedChanged);
			// 
			// radioDoNotSend
			// 
			this.radioDoNotSend.Location = new System.Drawing.Point(7, 16);
			this.radioDoNotSend.Name = "radioDoNotSend";
			this.radioDoNotSend.Size = new System.Drawing.Size(438, 16);
			this.radioDoNotSend.TabIndex = 77;
			this.radioDoNotSend.Text = "Do Not Send";
			this.radioDoNotSend.UseVisualStyleBackColor = true;
			// 
			// radioSendToEmailOnlyPreferred
			// 
			this.radioSendToEmailOnlyPreferred.Location = new System.Drawing.Point(7, 63);
			this.radioSendToEmailOnlyPreferred.Name = "radioSendToEmailOnlyPreferred";
			this.radioSendToEmailOnlyPreferred.Size = new System.Drawing.Size(438, 18);
			this.radioSendToEmailOnlyPreferred.TabIndex = 74;
			this.radioSendToEmailOnlyPreferred.Text = "Patients with email address and email is selected as their preferred recall metho" +
    "d.";
			this.radioSendToEmailOnlyPreferred.UseVisualStyleBackColor = true;
			this.radioSendToEmailOnlyPreferred.CheckedChanged += new System.EventHandler(this.WebSchedRecallAutoSendRadioButtons_CheckedChanged);
			// 
			// radioSendToEmail
			// 
			this.radioSendToEmail.Location = new System.Drawing.Point(7, 32);
			this.radioSendToEmail.Name = "radioSendToEmail";
			this.radioSendToEmail.Size = new System.Drawing.Size(438, 16);
			this.radioSendToEmail.TabIndex = 0;
			this.radioSendToEmail.Text = "Patients with email address";
			this.radioSendToEmail.UseVisualStyleBackColor = true;
			this.radioSendToEmail.CheckedChanged += new System.EventHandler(this.WebSchedRecallAutoSendRadioButtons_CheckedChanged);
			// 
			// groupWebSchedPreview
			// 
			this.groupWebSchedPreview.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupWebSchedPreview.Controls.Add(this.butWebSchedPickClinic);
			this.groupWebSchedPreview.Controls.Add(this.butWebSchedPickProv);
			this.groupWebSchedPreview.Controls.Add(this.label22);
			this.groupWebSchedPreview.Controls.Add(this.comboWebSchedProviders);
			this.groupWebSchedPreview.Controls.Add(this.butWebSchedToday);
			this.groupWebSchedPreview.Controls.Add(this.gridWebSchedTimeSlots);
			this.groupWebSchedPreview.Controls.Add(this.textWebSchedDateStart);
			this.groupWebSchedPreview.Controls.Add(this.labelWebSchedClinic);
			this.groupWebSchedPreview.Controls.Add(this.labelWebSchedRecallTypes);
			this.groupWebSchedPreview.Controls.Add(this.comboWebSchedClinic);
			this.groupWebSchedPreview.Controls.Add(this.comboWebSchedRecallTypes);
			this.groupWebSchedPreview.Location = new System.Drawing.Point(114, 253);
			this.groupWebSchedPreview.Name = "groupWebSchedPreview";
			this.groupWebSchedPreview.Size = new System.Drawing.Size(439, 172);
			this.groupWebSchedPreview.TabIndex = 252;
			this.groupWebSchedPreview.TabStop = false;
			this.groupWebSchedPreview.Text = "Available Times For Patients";
			// 
			// butWebSchedPickClinic
			// 
			this.butWebSchedPickClinic.Location = new System.Drawing.Point(414, 139);
			this.butWebSchedPickClinic.Name = "butWebSchedPickClinic";
			this.butWebSchedPickClinic.Size = new System.Drawing.Size(18, 21);
			this.butWebSchedPickClinic.TabIndex = 313;
			this.butWebSchedPickClinic.Text = "...";
			this.butWebSchedPickClinic.Click += new System.EventHandler(this.butWebSchedPickClinic_Click);
			// 
			// butWebSchedPickProv
			// 
			this.butWebSchedPickProv.Location = new System.Drawing.Point(414, 98);
			this.butWebSchedPickProv.Name = "butWebSchedPickProv";
			this.butWebSchedPickProv.Size = new System.Drawing.Size(18, 21);
			this.butWebSchedPickProv.TabIndex = 312;
			this.butWebSchedPickProv.Text = "...";
			this.butWebSchedPickProv.Click += new System.EventHandler(this.butWebSchedPickProv_Click);
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(200, 81);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(182, 14);
			this.label22.TabIndex = 310;
			this.label22.Text = "Provider";
			this.label22.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboWebSchedProviders
			// 
			this.comboWebSchedProviders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboWebSchedProviders.Location = new System.Drawing.Point(200, 98);
			this.comboWebSchedProviders.MaxDropDownItems = 30;
			this.comboWebSchedProviders.Name = "comboWebSchedProviders";
			this.comboWebSchedProviders.Size = new System.Drawing.Size(209, 21);
			this.comboWebSchedProviders.TabIndex = 311;
			this.comboWebSchedProviders.SelectionChangeCommitted += new System.EventHandler(this.comboWebSchedProviders_SelectionChangeCommitted);
			// 
			// butWebSchedToday
			// 
			this.butWebSchedToday.Location = new System.Drawing.Point(334, 16);
			this.butWebSchedToday.Name = "butWebSchedToday";
			this.butWebSchedToday.Size = new System.Drawing.Size(75, 21);
			this.butWebSchedToday.TabIndex = 309;
			this.butWebSchedToday.Text = "Today";
			this.butWebSchedToday.Click += new System.EventHandler(this.butWebSchedToday_Click);
			// 
			// gridWebSchedTimeSlots
			// 
			this.gridWebSchedTimeSlots.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridWebSchedTimeSlots.Location = new System.Drawing.Point(18, 16);
			this.gridWebSchedTimeSlots.Name = "gridWebSchedTimeSlots";
			this.gridWebSchedTimeSlots.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridWebSchedTimeSlots.Size = new System.Drawing.Size(174, 148);
			this.gridWebSchedTimeSlots.TabIndex = 302;
			this.gridWebSchedTimeSlots.Title = "Time Slots";
			this.gridWebSchedTimeSlots.TranslationName = "FormEServicesSetup";
			this.gridWebSchedTimeSlots.WrapText = false;
			// 
			// textWebSchedDateStart
			// 
			this.textWebSchedDateStart.Location = new System.Drawing.Point(203, 16);
			this.textWebSchedDateStart.Name = "textWebSchedDateStart";
			this.textWebSchedDateStart.Size = new System.Drawing.Size(90, 20);
			this.textWebSchedDateStart.TabIndex = 303;
			this.textWebSchedDateStart.Text = "07/08/2015";
			this.textWebSchedDateStart.TextChanged += new System.EventHandler(this.textWebSchedDateStart_TextChanged);
			// 
			// labelWebSchedClinic
			// 
			this.labelWebSchedClinic.Location = new System.Drawing.Point(200, 122);
			this.labelWebSchedClinic.Name = "labelWebSchedClinic";
			this.labelWebSchedClinic.Size = new System.Drawing.Size(182, 14);
			this.labelWebSchedClinic.TabIndex = 264;
			this.labelWebSchedClinic.Text = "Clinic";
			this.labelWebSchedClinic.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelWebSchedRecallTypes
			// 
			this.labelWebSchedRecallTypes.Location = new System.Drawing.Point(200, 40);
			this.labelWebSchedRecallTypes.Name = "labelWebSchedRecallTypes";
			this.labelWebSchedRecallTypes.Size = new System.Drawing.Size(182, 14);
			this.labelWebSchedRecallTypes.TabIndex = 254;
			this.labelWebSchedRecallTypes.Text = "Recall Type";
			this.labelWebSchedRecallTypes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboWebSchedClinic
			// 
			this.comboWebSchedClinic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboWebSchedClinic.Location = new System.Drawing.Point(200, 139);
			this.comboWebSchedClinic.MaxDropDownItems = 30;
			this.comboWebSchedClinic.Name = "comboWebSchedClinic";
			this.comboWebSchedClinic.Size = new System.Drawing.Size(209, 21);
			this.comboWebSchedClinic.TabIndex = 305;
			this.comboWebSchedClinic.SelectionChangeCommitted += new System.EventHandler(this.comboWebSchedClinic_SelectionChangeCommitted);
			// 
			// comboWebSchedRecallTypes
			// 
			this.comboWebSchedRecallTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboWebSchedRecallTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboWebSchedRecallTypes.Location = new System.Drawing.Point(200, 57);
			this.comboWebSchedRecallTypes.MaxDropDownItems = 30;
			this.comboWebSchedRecallTypes.Name = "comboWebSchedRecallTypes";
			this.comboWebSchedRecallTypes.Size = new System.Drawing.Size(209, 21);
			this.comboWebSchedRecallTypes.TabIndex = 304;
			this.comboWebSchedRecallTypes.SelectionChangeCommitted += new System.EventHandler(this.comboWebSchedRecallTypes_SelectionChangeCommitted);
			// 
			// gridWebSchedOperatories
			// 
			this.gridWebSchedOperatories.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.gridWebSchedOperatories.Location = new System.Drawing.Point(114, 48);
			this.gridWebSchedOperatories.Name = "gridWebSchedOperatories";
			this.gridWebSchedOperatories.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridWebSchedOperatories.Size = new System.Drawing.Size(532, 202);
			this.gridWebSchedOperatories.TabIndex = 307;
			this.gridWebSchedOperatories.Title = "Operatories Considered";
			this.gridWebSchedOperatories.TranslationName = "FormEServicesSetup";
			this.gridWebSchedOperatories.WrapText = false;
			this.gridWebSchedOperatories.DoubleClick += new System.EventHandler(this.gridWebSchedOperatories_DoubleClick);
			// 
			// label35
			// 
			this.label35.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label35.Location = new System.Drawing.Point(666, 31);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(345, 15);
			this.label35.TabIndex = 254;
			this.label35.Text = "Double click to edit.";
			this.label35.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// butRecallSchedSetup
			// 
			this.butRecallSchedSetup.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.butRecallSchedSetup.Location = new System.Drawing.Point(905, 258);
			this.butRecallSchedSetup.Name = "butRecallSchedSetup";
			this.butRecallSchedSetup.Size = new System.Drawing.Size(103, 24);
			this.butRecallSchedSetup.TabIndex = 308;
			this.butRecallSchedSetup.Text = "Recall Setup";
			this.butRecallSchedSetup.Click += new System.EventHandler(this.butWebSchedSetup_Click);
			// 
			// label31
			// 
			this.label31.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label31.Location = new System.Drawing.Point(389, 31);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(257, 15);
			this.label31.TabIndex = 254;
			this.label31.Text = "Double click to edit.";
			this.label31.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// gridWebSchedRecallTypes
			// 
			this.gridWebSchedRecallTypes.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.gridWebSchedRecallTypes.Location = new System.Drawing.Point(669, 48);
			this.gridWebSchedRecallTypes.Name = "gridWebSchedRecallTypes";
			this.gridWebSchedRecallTypes.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridWebSchedRecallTypes.Size = new System.Drawing.Size(342, 202);
			this.gridWebSchedRecallTypes.TabIndex = 307;
			this.gridWebSchedRecallTypes.Title = "Recall Types";
			this.gridWebSchedRecallTypes.TranslationName = "FormEServicesSetup";
			this.gridWebSchedRecallTypes.WrapText = false;
			this.gridWebSchedRecallTypes.DoubleClick += new System.EventHandler(this.gridWebSchedRecallTypes_DoubleClick);
			// 
			// label20
			// 
			this.label20.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label20.Location = new System.Drawing.Point(666, 256);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(233, 28);
			this.label20.TabIndex = 247;
			this.label20.Text = "Customize the notification message that will be sent to the patient.";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabWebSchedNewPatAppts
			// 
			this.tabWebSchedNewPatAppts.Controls.Add(this.checkWSNPDoubleBooking);
			this.tabWebSchedNewPatAppts.Controls.Add(this.label38);
			this.tabWebSchedNewPatAppts.Controls.Add(this.groupBoxWSNPHostedURLs);
			this.tabWebSchedNewPatAppts.Controls.Add(this.checkNewPatAllowProvSelection);
			this.tabWebSchedNewPatAppts.Controls.Add(this.checkWebSchedNewPatForcePhoneFormatting);
			this.tabWebSchedNewPatAppts.Controls.Add(this.comboWSNPConfirmStatuses);
			this.tabWebSchedNewPatAppts.Controls.Add(this.labelWebSchedNewPatConfirmStatus);
			this.tabWebSchedNewPatAppts.Controls.Add(this.groupBox13);
			this.tabWebSchedNewPatAppts.Controls.Add(this.groupBox11);
			this.tabWebSchedNewPatAppts.Controls.Add(this.gridWebSchedNewPatApptOps);
			this.tabWebSchedNewPatAppts.Controls.Add(this.label42);
			this.tabWebSchedNewPatAppts.Controls.Add(this.groupBox7);
			this.tabWebSchedNewPatAppts.Controls.Add(this.gridWSNPAReasons);
			this.tabWebSchedNewPatAppts.Controls.Add(this.label41);
			this.tabWebSchedNewPatAppts.Controls.Add(this.label40);
			this.tabWebSchedNewPatAppts.Controls.Add(this.textWebSchedNewPatApptSearchDays);
			this.tabWebSchedNewPatAppts.Location = new System.Drawing.Point(4, 22);
			this.tabWebSchedNewPatAppts.Name = "tabWebSchedNewPatAppts";
			this.tabWebSchedNewPatAppts.Padding = new System.Windows.Forms.Padding(3);
			this.tabWebSchedNewPatAppts.Size = new System.Drawing.Size(1140, 549);
			this.tabWebSchedNewPatAppts.TabIndex = 1;
			this.tabWebSchedNewPatAppts.Text = "New Patient Appts";
			this.tabWebSchedNewPatAppts.UseVisualStyleBackColor = true;
			// 
			// checkWSNPDoubleBooking
			// 
			this.checkWSNPDoubleBooking.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWSNPDoubleBooking.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkWSNPDoubleBooking.Location = new System.Drawing.Point(61, 503);
			this.checkWSNPDoubleBooking.Name = "checkWSNPDoubleBooking";
			this.checkWSNPDoubleBooking.Size = new System.Drawing.Size(216, 18);
			this.checkWSNPDoubleBooking.TabIndex = 331;
			this.checkWSNPDoubleBooking.Text = "Prevent double booking";
			this.checkWSNPDoubleBooking.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label38
			// 
			this.label38.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label38.Location = new System.Drawing.Point(117, 60);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(245, 15);
			this.label38.TabIndex = 330;
			this.label38.Text = "Double click to edit.";
			this.label38.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// groupBoxWSNPHostedURLs
			// 
			this.groupBoxWSNPHostedURLs.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBoxWSNPHostedURLs.Controls.Add(this.panelHostedURLs);
			this.groupBoxWSNPHostedURLs.Location = new System.Drawing.Point(390, 226);
			this.groupBoxWSNPHostedURLs.Name = "groupBoxWSNPHostedURLs";
			this.groupBoxWSNPHostedURLs.Size = new System.Drawing.Size(675, 204);
			this.groupBoxWSNPHostedURLs.TabIndex = 329;
			this.groupBoxWSNPHostedURLs.TabStop = false;
			this.groupBoxWSNPHostedURLs.Text = "Hosted URLs will be where new patients need to visit in order to create an appoin" +
    "tment. Use Signup to enable.";
			// 
			// panelHostedURLs
			// 
			this.panelHostedURLs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelHostedURLs.AutoScroll = true;
			this.panelHostedURLs.Location = new System.Drawing.Point(0, 13);
			this.panelHostedURLs.Name = "panelHostedURLs";
			this.panelHostedURLs.Size = new System.Drawing.Size(675, 186);
			this.panelHostedURLs.TabIndex = 0;
			// 
			// checkNewPatAllowProvSelection
			// 
			this.checkNewPatAllowProvSelection.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.checkNewPatAllowProvSelection.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkNewPatAllowProvSelection.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNewPatAllowProvSelection.Location = new System.Drawing.Point(61, 479);
			this.checkNewPatAllowProvSelection.Name = "checkNewPatAllowProvSelection";
			this.checkNewPatAllowProvSelection.Size = new System.Drawing.Size(216, 18);
			this.checkNewPatAllowProvSelection.TabIndex = 328;
			this.checkNewPatAllowProvSelection.Text = "Allow patients to select provider";
			this.checkNewPatAllowProvSelection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkWebSchedNewPatForcePhoneFormatting
			// 
			this.checkWebSchedNewPatForcePhoneFormatting.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.checkWebSchedNewPatForcePhoneFormatting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWebSchedNewPatForcePhoneFormatting.Location = new System.Drawing.Point(555, 38);
			this.checkWebSchedNewPatForcePhoneFormatting.Name = "checkWebSchedNewPatForcePhoneFormatting";
			this.checkWebSchedNewPatForcePhoneFormatting.Size = new System.Drawing.Size(237, 19);
			this.checkWebSchedNewPatForcePhoneFormatting.TabIndex = 327;
			this.checkWebSchedNewPatForcePhoneFormatting.Text = "Force US phone number format";
			this.checkWebSchedNewPatForcePhoneFormatting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWebSchedNewPatForcePhoneFormatting.UseVisualStyleBackColor = true;
			this.checkWebSchedNewPatForcePhoneFormatting.Click += new System.EventHandler(this.checkWebSchedNewPatForcePhoneFormatting_Click);
			// 
			// comboWSNPConfirmStatuses
			// 
			this.comboWSNPConfirmStatuses.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.comboWSNPConfirmStatuses.Location = new System.Drawing.Point(774, 11);
			this.comboWSNPConfirmStatuses.Name = "comboWSNPConfirmStatuses";
			this.comboWSNPConfirmStatuses.Size = new System.Drawing.Size(291, 21);
			this.comboWSNPConfirmStatuses.TabIndex = 326;
			// 
			// labelWebSchedNewPatConfirmStatus
			// 
			this.labelWebSchedNewPatConfirmStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.labelWebSchedNewPatConfirmStatus.Location = new System.Drawing.Point(562, 12);
			this.labelWebSchedNewPatConfirmStatus.Name = "labelWebSchedNewPatConfirmStatus";
			this.labelWebSchedNewPatConfirmStatus.Size = new System.Drawing.Size(206, 17);
			this.labelWebSchedNewPatConfirmStatus.TabIndex = 325;
			this.labelWebSchedNewPatConfirmStatus.Text = "Confirm Status";
			this.labelWebSchedNewPatConfirmStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox13
			// 
			this.groupBox13.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox13.Controls.Add(this.textWebSchedNewPatApptMessage);
			this.groupBox13.Location = new System.Drawing.Point(390, 431);
			this.groupBox13.Name = "groupBox13";
			this.groupBox13.Size = new System.Drawing.Size(304, 103);
			this.groupBox13.TabIndex = 324;
			this.groupBox13.TabStop = false;
			this.groupBox13.Text = "Appointment Message";
			// 
			// textWebSchedNewPatApptMessage
			// 
			this.textWebSchedNewPatApptMessage.Location = new System.Drawing.Point(6, 17);
			this.textWebSchedNewPatApptMessage.Multiline = true;
			this.textWebSchedNewPatApptMessage.Name = "textWebSchedNewPatApptMessage";
			this.textWebSchedNewPatApptMessage.Size = new System.Drawing.Size(292, 80);
			this.textWebSchedNewPatApptMessage.TabIndex = 313;
			// 
			// groupBox11
			// 
			this.groupBox11.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox11.Controls.Add(this.listboxWebSchedNewPatIgnoreBlockoutTypes);
			this.groupBox11.Controls.Add(this.label33);
			this.groupBox11.Controls.Add(this.butWebSchedNewPatBlockouts);
			this.groupBox11.Location = new System.Drawing.Point(700, 433);
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.Size = new System.Drawing.Size(365, 103);
			this.groupBox11.TabIndex = 323;
			this.groupBox11.TabStop = false;
			this.groupBox11.Text = "Ignore Blockout Types";
			// 
			// listboxWebSchedNewPatIgnoreBlockoutTypes
			// 
			this.listboxWebSchedNewPatIgnoreBlockoutTypes.FormattingEnabled = true;
			this.listboxWebSchedNewPatIgnoreBlockoutTypes.Location = new System.Drawing.Point(213, 13);
			this.listboxWebSchedNewPatIgnoreBlockoutTypes.Name = "listboxWebSchedNewPatIgnoreBlockoutTypes";
			this.listboxWebSchedNewPatIgnoreBlockoutTypes.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listboxWebSchedNewPatIgnoreBlockoutTypes.Size = new System.Drawing.Size(146, 82);
			this.listboxWebSchedNewPatIgnoreBlockoutTypes.TabIndex = 197;
			// 
			// label33
			// 
			this.label33.Location = new System.Drawing.Point(6, 14);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(206, 20);
			this.label33.TabIndex = 223;
			this.label33.Text = "Currently Ignored Blockout Types";
			this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butWebSchedNewPatBlockouts
			// 
			this.butWebSchedNewPatBlockouts.Location = new System.Drawing.Point(139, 72);
			this.butWebSchedNewPatBlockouts.Name = "butWebSchedNewPatBlockouts";
			this.butWebSchedNewPatBlockouts.Size = new System.Drawing.Size(68, 23);
			this.butWebSchedNewPatBlockouts.TabIndex = 197;
			this.butWebSchedNewPatBlockouts.Text = "Edit";
			this.butWebSchedNewPatBlockouts.Click += new System.EventHandler(this.butWebSchedNewPatBlockouts_Click);
			// 
			// gridWebSchedNewPatApptOps
			// 
			this.gridWebSchedNewPatApptOps.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.gridWebSchedNewPatApptOps.Location = new System.Drawing.Point(390, 78);
			this.gridWebSchedNewPatApptOps.Name = "gridWebSchedNewPatApptOps";
			this.gridWebSchedNewPatApptOps.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridWebSchedNewPatApptOps.Size = new System.Drawing.Size(675, 142);
			this.gridWebSchedNewPatApptOps.TabIndex = 309;
			this.gridWebSchedNewPatApptOps.Title = "Operatories Considered";
			this.gridWebSchedNewPatApptOps.TranslationName = "FormEServicesSetup";
			this.gridWebSchedNewPatApptOps.WrapText = false;
			this.gridWebSchedNewPatApptOps.DoubleClick += new System.EventHandler(this.gridWebSchedNewPatApptOps_DoubleClick);
			// 
			// label42
			// 
			this.label42.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label42.Location = new System.Drawing.Point(820, 60);
			this.label42.Name = "label42";
			this.label42.Size = new System.Drawing.Size(245, 15);
			this.label42.TabIndex = 308;
			this.label42.Text = "Double click to edit.";
			this.label42.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// groupBox7
			// 
			this.groupBox7.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox7.Controls.Add(this.labelWSNPClinic);
			this.groupBox7.Controls.Add(this.labelWSNPAApptType);
			this.groupBox7.Controls.Add(this.comboWSNPClinics);
			this.groupBox7.Controls.Add(this.comboWSNPADefApptType);
			this.groupBox7.Controls.Add(this.labelWSNPTimeSlots);
			this.groupBox7.Controls.Add(this.butWebSchedNewPatApptsToday);
			this.groupBox7.Controls.Add(this.gridWebSchedNewPatApptTimeSlots);
			this.groupBox7.Controls.Add(this.textWebSchedNewPatApptsDateStart);
			this.groupBox7.Location = new System.Drawing.Point(61, 226);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(301, 228);
			this.groupBox7.TabIndex = 304;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Available Times For Patients";
			// 
			// labelWSNPClinic
			// 
			this.labelWSNPClinic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelWSNPClinic.Location = new System.Drawing.Point(195, 77);
			this.labelWSNPClinic.Name = "labelWSNPClinic";
			this.labelWSNPClinic.Size = new System.Drawing.Size(102, 16);
			this.labelWSNPClinic.TabIndex = 324;
			this.labelWSNPClinic.Text = "Clinic";
			this.labelWSNPClinic.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelWSNPAApptType
			// 
			this.labelWSNPAApptType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelWSNPAApptType.Location = new System.Drawing.Point(195, 129);
			this.labelWSNPAApptType.Name = "labelWSNPAApptType";
			this.labelWSNPAApptType.Size = new System.Drawing.Size(102, 17);
			this.labelWSNPAApptType.TabIndex = 320;
			this.labelWSNPAApptType.Text = "Reason";
			// 
			// comboWSNPClinics
			// 
			this.comboWSNPClinics.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboWSNPClinics.Location = new System.Drawing.Point(195, 96);
			this.comboWSNPClinics.MaxDropDownItems = 30;
			this.comboWSNPClinics.Name = "comboWSNPClinics";
			this.comboWSNPClinics.Size = new System.Drawing.Size(100, 21);
			this.comboWSNPClinics.TabIndex = 323;
			this.comboWSNPClinics.SelectionChangeCommitted += new System.EventHandler(this.comboWSNPClinics_SelectionChangeCommitted);
			// 
			// comboWSNPADefApptType
			// 
			this.comboWSNPADefApptType.Location = new System.Drawing.Point(195, 148);
			this.comboWSNPADefApptType.Name = "comboWSNPADefApptType";
			this.comboWSNPADefApptType.Size = new System.Drawing.Size(100, 21);
			this.comboWSNPADefApptType.TabIndex = 319;
			this.comboWSNPADefApptType.SelectionChangeCommitted += new System.EventHandler(this.comboWebSchedNewPatApptsApptType_SelectionChangeCommitted);
			// 
			// labelWSNPTimeSlots
			// 
			this.labelWSNPTimeSlots.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelWSNPTimeSlots.Location = new System.Drawing.Point(12, 196);
			this.labelWSNPTimeSlots.Name = "labelWSNPTimeSlots";
			this.labelWSNPTimeSlots.Size = new System.Drawing.Size(277, 26);
			this.labelWSNPTimeSlots.TabIndex = 318;
			this.labelWSNPTimeSlots.Text = "Select a clinic to view its available time slots.";
			// 
			// butWebSchedNewPatApptsToday
			// 
			this.butWebSchedNewPatApptsToday.Location = new System.Drawing.Point(204, 46);
			this.butWebSchedNewPatApptsToday.Name = "butWebSchedNewPatApptsToday";
			this.butWebSchedNewPatApptsToday.Size = new System.Drawing.Size(79, 21);
			this.butWebSchedNewPatApptsToday.TabIndex = 309;
			this.butWebSchedNewPatApptsToday.Text = "Today";
			this.butWebSchedNewPatApptsToday.Click += new System.EventHandler(this.butWebSchedNewPatApptsToday_Click);
			// 
			// gridWebSchedNewPatApptTimeSlots
			// 
			this.gridWebSchedNewPatApptTimeSlots.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridWebSchedNewPatApptTimeSlots.Location = new System.Drawing.Point(15, 20);
			this.gridWebSchedNewPatApptTimeSlots.Name = "gridWebSchedNewPatApptTimeSlots";
			this.gridWebSchedNewPatApptTimeSlots.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridWebSchedNewPatApptTimeSlots.Size = new System.Drawing.Size(174, 169);
			this.gridWebSchedNewPatApptTimeSlots.TabIndex = 302;
			this.gridWebSchedNewPatApptTimeSlots.Title = "Time Slots";
			this.gridWebSchedNewPatApptTimeSlots.TranslationName = "FormEServicesSetup";
			this.gridWebSchedNewPatApptTimeSlots.WrapText = false;
			// 
			// textWebSchedNewPatApptsDateStart
			// 
			this.textWebSchedNewPatApptsDateStart.Location = new System.Drawing.Point(204, 20);
			this.textWebSchedNewPatApptsDateStart.Name = "textWebSchedNewPatApptsDateStart";
			this.textWebSchedNewPatApptsDateStart.Size = new System.Drawing.Size(79, 20);
			this.textWebSchedNewPatApptsDateStart.TabIndex = 303;
			this.textWebSchedNewPatApptsDateStart.Text = "07/08/2015";
			this.textWebSchedNewPatApptsDateStart.TextChanged += new System.EventHandler(this.textWebSchedNewPatApptsDateStart_TextChanged);
			// 
			// gridWSNPAReasons
			// 
			this.gridWSNPAReasons.AllowSelection = false;
			this.gridWSNPAReasons.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.gridWSNPAReasons.Location = new System.Drawing.Point(61, 78);
			this.gridWSNPAReasons.Name = "gridWSNPAReasons";
			this.gridWSNPAReasons.Size = new System.Drawing.Size(301, 142);
			this.gridWSNPAReasons.TabIndex = 303;
			this.gridWSNPAReasons.Title = "Appointment Types";
			this.gridWSNPAReasons.TranslationName = "FormEServicesSetup";
			this.gridWSNPAReasons.DoubleClick += new System.EventHandler(this.gridWSNPAReasons_DoubleClick);
			// 
			// label41
			// 
			this.label41.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label41.Location = new System.Drawing.Point(270, 12);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(252, 17);
			this.label41.TabIndex = 244;
			this.label41.Text = "days.  Empty includes all possible openings.";
			this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label40
			// 
			this.label40.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label40.Location = new System.Drawing.Point(56, 12);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(167, 17);
			this.label40.TabIndex = 242;
			this.label40.Text = "Search for openings after";
			this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWebSchedNewPatApptSearchDays
			// 
			this.textWebSchedNewPatApptSearchDays.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.textWebSchedNewPatApptSearchDays.Location = new System.Drawing.Point(229, 11);
			this.textWebSchedNewPatApptSearchDays.MaxVal = 365;
			this.textWebSchedNewPatApptSearchDays.MinVal = 0;
			this.textWebSchedNewPatApptSearchDays.Name = "textWebSchedNewPatApptSearchDays";
			this.textWebSchedNewPatApptSearchDays.Size = new System.Drawing.Size(38, 20);
			this.textWebSchedNewPatApptSearchDays.TabIndex = 243;
			this.textWebSchedNewPatApptSearchDays.Leave += new System.EventHandler(this.textWebSchedNewPatApptSearchDays_Leave);
			this.textWebSchedNewPatApptSearchDays.Validated += new System.EventHandler(this.textWebSchedNewPatApptSearchDays_Validated);
			// 
			// tabWebSchedVerify
			// 
			this.tabWebSchedVerify.Controls.Add(this.butRestoreWebSchedVerify);
			this.tabWebSchedVerify.Controls.Add(this.label28);
			this.tabWebSchedVerify.Controls.Add(this.checkUseDefaultsVerify);
			this.tabWebSchedVerify.Controls.Add(this.groupBoxASAP);
			this.tabWebSchedVerify.Controls.Add(this.groupBoxNewPat);
			this.tabWebSchedVerify.Controls.Add(this.groupBoxRecall);
			this.tabWebSchedVerify.Controls.Add(this.comboClinicVerify);
			this.tabWebSchedVerify.Location = new System.Drawing.Point(4, 22);
			this.tabWebSchedVerify.Name = "tabWebSchedVerify";
			this.tabWebSchedVerify.Padding = new System.Windows.Forms.Padding(3);
			this.tabWebSchedVerify.Size = new System.Drawing.Size(1140, 549);
			this.tabWebSchedVerify.TabIndex = 2;
			this.tabWebSchedVerify.Text = "Notify";
			this.tabWebSchedVerify.UseVisualStyleBackColor = true;
			// 
			// butRestoreWebSchedVerify
			// 
			this.butRestoreWebSchedVerify.Location = new System.Drawing.Point(1059, 6);
			this.butRestoreWebSchedVerify.Name = "butRestoreWebSchedVerify";
			this.butRestoreWebSchedVerify.Size = new System.Drawing.Size(75, 23);
			this.butRestoreWebSchedVerify.TabIndex = 304;
			this.butRestoreWebSchedVerify.Text = "Undo All";
			this.butRestoreWebSchedVerify.UseVisualStyleBackColor = true;
			this.butRestoreWebSchedVerify.Click += new System.EventHandler(this.WebSchedVerify_butUndoClick);
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(646, 3);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(407, 28);
			this.label28.TabIndex = 269;
			this.label28.Text = "Right-click on any text box to choose from a list of valid template fields.";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkUseDefaultsVerify
			// 
			this.checkUseDefaultsVerify.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUseDefaultsVerify.Location = new System.Drawing.Point(384, 10);
			this.checkUseDefaultsVerify.Name = "checkUseDefaultsVerify";
			this.checkUseDefaultsVerify.Size = new System.Drawing.Size(105, 19);
			this.checkUseDefaultsVerify.TabIndex = 267;
			this.checkUseDefaultsVerify.Text = "Use Defaults";
			this.checkUseDefaultsVerify.CheckedChanged += new System.EventHandler(this.WebSchedVerify_CheckUseDefaultsChanged);
			// 
			// groupBoxASAP
			// 
			this.groupBoxASAP.Controls.Add(this.groupBoxASAPTextTemplate);
			this.groupBoxASAP.Controls.Add(this.groupBoxASAPEmail);
			this.groupBoxASAP.Controls.Add(this.groupBoxRadioASAP);
			this.groupBoxASAP.Location = new System.Drawing.Point(762, 32);
			this.groupBoxASAP.Name = "groupBoxASAP";
			this.groupBoxASAP.Size = new System.Drawing.Size(372, 497);
			this.groupBoxASAP.TabIndex = 2;
			this.groupBoxASAP.TabStop = false;
			this.groupBoxASAP.Text = "ASAP";
			// 
			// groupBoxASAPTextTemplate
			// 
			this.groupBoxASAPTextTemplate.Controls.Add(this.textASAPTextTemplate);
			this.groupBoxASAPTextTemplate.Location = new System.Drawing.Point(6, 139);
			this.groupBoxASAPTextTemplate.Name = "groupBoxASAPTextTemplate";
			this.groupBoxASAPTextTemplate.Size = new System.Drawing.Size(360, 120);
			this.groupBoxASAPTextTemplate.TabIndex = 121;
			this.groupBoxASAPTextTemplate.TabStop = false;
			this.groupBoxASAPTextTemplate.Text = "Text Message";
			// 
			// textASAPTextTemplate
			// 
			this.textASAPTextTemplate.ContextMenuStrip = this.menuWebSchedVerifyTextTemplate;
			this.textASAPTextTemplate.Location = new System.Drawing.Point(6, 19);
			this.textASAPTextTemplate.Multiline = true;
			this.textASAPTextTemplate.Name = "textASAPTextTemplate";
			this.textASAPTextTemplate.Size = new System.Drawing.Size(348, 95);
			this.textASAPTextTemplate.TabIndex = 314;
			this.textASAPTextTemplate.Tag = OpenDentBusiness.PrefName.WebSchedVerifyASAPText;
			this.textASAPTextTemplate.Leave += new System.EventHandler(this.WebSchedVerify_TextLeave);
			// 
			// groupBoxASAPEmail
			// 
			this.groupBoxASAPEmail.Controls.Add(this.butAsapEditEmail);
			this.groupBoxASAPEmail.Controls.Add(this.browserAsapEmailBody);
			this.groupBoxASAPEmail.Controls.Add(this.textASAPEmailSubj);
			this.groupBoxASAPEmail.Location = new System.Drawing.Point(6, 259);
			this.groupBoxASAPEmail.Name = "groupBoxASAPEmail";
			this.groupBoxASAPEmail.Size = new System.Drawing.Size(360, 232);
			this.groupBoxASAPEmail.TabIndex = 122;
			this.groupBoxASAPEmail.TabStop = false;
			this.groupBoxASAPEmail.Text = "E-mail Subject and Body";
			// 
			// butAsapEditEmail
			// 
			this.butAsapEditEmail.Location = new System.Drawing.Point(284, 209);
			this.butAsapEditEmail.Name = "butAsapEditEmail";
			this.butAsapEditEmail.Size = new System.Drawing.Size(70, 20);
			this.butAsapEditEmail.TabIndex = 318;
			this.butAsapEditEmail.Text = "Edit";
			this.butAsapEditEmail.UseVisualStyleBackColor = true;
			this.butAsapEditEmail.Click += new System.EventHandler(this.WebSchedVerify_AsapEditEmailClick);
			// 
			// browserAsapEmailBody
			// 
			this.browserAsapEmailBody.AllowWebBrowserDrop = false;
			this.browserAsapEmailBody.Location = new System.Drawing.Point(7, 45);
			this.browserAsapEmailBody.MinimumSize = new System.Drawing.Size(20, 20);
			this.browserAsapEmailBody.Name = "browserAsapEmailBody";
			this.browserAsapEmailBody.Size = new System.Drawing.Size(347, 158);
			this.browserAsapEmailBody.TabIndex = 317;
			this.browserAsapEmailBody.WebBrowserShortcutsEnabled = false;
			// 
			// textASAPEmailSubj
			// 
			this.textASAPEmailSubj.ContextMenuStrip = this.menuWebSchedVerifyTextTemplate;
			this.textASAPEmailSubj.Location = new System.Drawing.Point(6, 19);
			this.textASAPEmailSubj.Multiline = true;
			this.textASAPEmailSubj.Name = "textASAPEmailSubj";
			this.textASAPEmailSubj.Size = new System.Drawing.Size(348, 20);
			this.textASAPEmailSubj.TabIndex = 314;
			this.textASAPEmailSubj.Tag = OpenDentBusiness.PrefName.WebSchedVerifyASAPEmailSubj;
			this.textASAPEmailSubj.Leave += new System.EventHandler(this.WebSchedVerify_TextLeave);
			// 
			// groupBoxRadioASAP
			// 
			this.groupBoxRadioASAP.Controls.Add(this.radioASAPTextAndEmail);
			this.groupBoxRadioASAP.Controls.Add(this.radioASAPEmail);
			this.groupBoxRadioASAP.Controls.Add(this.radioASAPText);
			this.groupBoxRadioASAP.Controls.Add(this.radioASAPNone);
			this.groupBoxRadioASAP.Location = new System.Drawing.Point(6, 18);
			this.groupBoxRadioASAP.Name = "groupBoxRadioASAP";
			this.groupBoxRadioASAP.Size = new System.Drawing.Size(360, 115);
			this.groupBoxRadioASAP.TabIndex = 1;
			this.groupBoxRadioASAP.TabStop = false;
			this.groupBoxRadioASAP.Tag = OpenDentBusiness.PrefName.WebSchedVerifyASAPType;
			this.groupBoxRadioASAP.Text = "Communication Method";
			// 
			// radioASAPTextAndEmail
			// 
			this.radioASAPTextAndEmail.Location = new System.Drawing.Point(7, 89);
			this.radioASAPTextAndEmail.Name = "radioASAPTextAndEmail";
			this.radioASAPTextAndEmail.Size = new System.Drawing.Size(175, 17);
			this.radioASAPTextAndEmail.TabIndex = 3;
			this.radioASAPTextAndEmail.TabStop = true;
			this.radioASAPTextAndEmail.Tag = OpenDentBusiness.WebSchedVerifyType.TextAndEmail;
			this.radioASAPTextAndEmail.Text = "Text and E-mail";
			this.radioASAPTextAndEmail.UseVisualStyleBackColor = true;
			this.radioASAPTextAndEmail.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioASAPEmail
			// 
			this.radioASAPEmail.Location = new System.Drawing.Point(7, 66);
			this.radioASAPEmail.Name = "radioASAPEmail";
			this.radioASAPEmail.Size = new System.Drawing.Size(175, 17);
			this.radioASAPEmail.TabIndex = 2;
			this.radioASAPEmail.TabStop = true;
			this.radioASAPEmail.Tag = OpenDentBusiness.WebSchedVerifyType.Email;
			this.radioASAPEmail.Text = "E-mail";
			this.radioASAPEmail.UseVisualStyleBackColor = true;
			this.radioASAPEmail.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioASAPText
			// 
			this.radioASAPText.Location = new System.Drawing.Point(7, 43);
			this.radioASAPText.Name = "radioASAPText";
			this.radioASAPText.Size = new System.Drawing.Size(175, 17);
			this.radioASAPText.TabIndex = 1;
			this.radioASAPText.TabStop = true;
			this.radioASAPText.Tag = OpenDentBusiness.WebSchedVerifyType.Text;
			this.radioASAPText.Text = "Text";
			this.radioASAPText.UseVisualStyleBackColor = true;
			this.radioASAPText.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioASAPNone
			// 
			this.radioASAPNone.Location = new System.Drawing.Point(7, 20);
			this.radioASAPNone.Name = "radioASAPNone";
			this.radioASAPNone.Size = new System.Drawing.Size(175, 17);
			this.radioASAPNone.TabIndex = 0;
			this.radioASAPNone.TabStop = true;
			this.radioASAPNone.Tag = OpenDentBusiness.WebSchedVerifyType.None;
			this.radioASAPNone.Text = "None";
			this.radioASAPNone.UseVisualStyleBackColor = true;
			this.radioASAPNone.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// groupBoxNewPat
			// 
			this.groupBoxNewPat.Controls.Add(this.groupBoxNewPatTextTemplate);
			this.groupBoxNewPat.Controls.Add(this.groupBoxNewPatEmail);
			this.groupBoxNewPat.Controls.Add(this.groupBoxRadioNewPat);
			this.groupBoxNewPat.Location = new System.Drawing.Point(384, 32);
			this.groupBoxNewPat.Name = "groupBoxNewPat";
			this.groupBoxNewPat.Size = new System.Drawing.Size(372, 497);
			this.groupBoxNewPat.TabIndex = 1;
			this.groupBoxNewPat.TabStop = false;
			this.groupBoxNewPat.Text = "New Patient";
			// 
			// groupBoxNewPatTextTemplate
			// 
			this.groupBoxNewPatTextTemplate.Controls.Add(this.textNewPatTextTemplate);
			this.groupBoxNewPatTextTemplate.Location = new System.Drawing.Point(6, 139);
			this.groupBoxNewPatTextTemplate.Name = "groupBoxNewPatTextTemplate";
			this.groupBoxNewPatTextTemplate.Size = new System.Drawing.Size(360, 120);
			this.groupBoxNewPatTextTemplate.TabIndex = 121;
			this.groupBoxNewPatTextTemplate.TabStop = false;
			this.groupBoxNewPatTextTemplate.Text = "Text Message";
			// 
			// textNewPatTextTemplate
			// 
			this.textNewPatTextTemplate.ContextMenuStrip = this.menuWebSchedVerifyTextTemplate;
			this.textNewPatTextTemplate.Location = new System.Drawing.Point(6, 19);
			this.textNewPatTextTemplate.Multiline = true;
			this.textNewPatTextTemplate.Name = "textNewPatTextTemplate";
			this.textNewPatTextTemplate.Size = new System.Drawing.Size(348, 95);
			this.textNewPatTextTemplate.TabIndex = 314;
			this.textNewPatTextTemplate.Tag = OpenDentBusiness.PrefName.WebSchedVerifyNewPatText;
			this.textNewPatTextTemplate.Leave += new System.EventHandler(this.WebSchedVerify_TextLeave);
			// 
			// groupBoxNewPatEmail
			// 
			this.groupBoxNewPatEmail.Controls.Add(this.butNewPatEditEmail);
			this.groupBoxNewPatEmail.Controls.Add(this.browserNewPatEmailBody);
			this.groupBoxNewPatEmail.Controls.Add(this.textNewPatEmailSubj);
			this.groupBoxNewPatEmail.Location = new System.Drawing.Point(6, 259);
			this.groupBoxNewPatEmail.Name = "groupBoxNewPatEmail";
			this.groupBoxNewPatEmail.Size = new System.Drawing.Size(360, 232);
			this.groupBoxNewPatEmail.TabIndex = 122;
			this.groupBoxNewPatEmail.TabStop = false;
			this.groupBoxNewPatEmail.Text = "E-mail Subject and Body";
			// 
			// butNewPatEditEmail
			// 
			this.butNewPatEditEmail.Location = new System.Drawing.Point(284, 209);
			this.butNewPatEditEmail.Name = "butNewPatEditEmail";
			this.butNewPatEditEmail.Size = new System.Drawing.Size(70, 20);
			this.butNewPatEditEmail.TabIndex = 316;
			this.butNewPatEditEmail.Text = "Edit";
			this.butNewPatEditEmail.UseVisualStyleBackColor = true;
			this.butNewPatEditEmail.Click += new System.EventHandler(this.WebSchedVerify_NewPatEditEmailClick);
			// 
			// browserNewPatEmailBody
			// 
			this.browserNewPatEmailBody.AllowWebBrowserDrop = false;
			this.browserNewPatEmailBody.Location = new System.Drawing.Point(7, 45);
			this.browserNewPatEmailBody.MinimumSize = new System.Drawing.Size(20, 20);
			this.browserNewPatEmailBody.Name = "browserNewPatEmailBody";
			this.browserNewPatEmailBody.Size = new System.Drawing.Size(347, 158);
			this.browserNewPatEmailBody.TabIndex = 315;
			this.browserNewPatEmailBody.WebBrowserShortcutsEnabled = false;
			// 
			// textNewPatEmailSubj
			// 
			this.textNewPatEmailSubj.ContextMenuStrip = this.menuWebSchedVerifyTextTemplate;
			this.textNewPatEmailSubj.Location = new System.Drawing.Point(6, 19);
			this.textNewPatEmailSubj.Multiline = true;
			this.textNewPatEmailSubj.Name = "textNewPatEmailSubj";
			this.textNewPatEmailSubj.Size = new System.Drawing.Size(348, 20);
			this.textNewPatEmailSubj.TabIndex = 314;
			this.textNewPatEmailSubj.Tag = OpenDentBusiness.PrefName.WebSchedVerifyNewPatEmailSubj;
			this.textNewPatEmailSubj.Leave += new System.EventHandler(this.WebSchedVerify_TextLeave);
			// 
			// groupBoxRadioNewPat
			// 
			this.groupBoxRadioNewPat.Controls.Add(this.radioNewPatTextAndEmail);
			this.groupBoxRadioNewPat.Controls.Add(this.radioNewPatEmail);
			this.groupBoxRadioNewPat.Controls.Add(this.radioNewPatText);
			this.groupBoxRadioNewPat.Controls.Add(this.radioNewPatNone);
			this.groupBoxRadioNewPat.Location = new System.Drawing.Point(6, 18);
			this.groupBoxRadioNewPat.Name = "groupBoxRadioNewPat";
			this.groupBoxRadioNewPat.Size = new System.Drawing.Size(360, 115);
			this.groupBoxRadioNewPat.TabIndex = 1;
			this.groupBoxRadioNewPat.TabStop = false;
			this.groupBoxRadioNewPat.Tag = OpenDentBusiness.PrefName.WebSchedVerifyNewPatType;
			this.groupBoxRadioNewPat.Text = "Communication Method";
			// 
			// radioNewPatTextAndEmail
			// 
			this.radioNewPatTextAndEmail.Location = new System.Drawing.Point(7, 89);
			this.radioNewPatTextAndEmail.Name = "radioNewPatTextAndEmail";
			this.radioNewPatTextAndEmail.Size = new System.Drawing.Size(175, 17);
			this.radioNewPatTextAndEmail.TabIndex = 3;
			this.radioNewPatTextAndEmail.TabStop = true;
			this.radioNewPatTextAndEmail.Tag = OpenDentBusiness.WebSchedVerifyType.TextAndEmail;
			this.radioNewPatTextAndEmail.Text = "Text and E-mail";
			this.radioNewPatTextAndEmail.UseVisualStyleBackColor = true;
			this.radioNewPatTextAndEmail.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioNewPatEmail
			// 
			this.radioNewPatEmail.Location = new System.Drawing.Point(7, 66);
			this.radioNewPatEmail.Name = "radioNewPatEmail";
			this.radioNewPatEmail.Size = new System.Drawing.Size(175, 17);
			this.radioNewPatEmail.TabIndex = 2;
			this.radioNewPatEmail.TabStop = true;
			this.radioNewPatEmail.Tag = OpenDentBusiness.WebSchedVerifyType.Email;
			this.radioNewPatEmail.Text = "E-mail";
			this.radioNewPatEmail.UseVisualStyleBackColor = true;
			this.radioNewPatEmail.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioNewPatText
			// 
			this.radioNewPatText.Location = new System.Drawing.Point(7, 43);
			this.radioNewPatText.Name = "radioNewPatText";
			this.radioNewPatText.Size = new System.Drawing.Size(175, 17);
			this.radioNewPatText.TabIndex = 1;
			this.radioNewPatText.TabStop = true;
			this.radioNewPatText.Tag = OpenDentBusiness.WebSchedVerifyType.Text;
			this.radioNewPatText.Text = "Text";
			this.radioNewPatText.UseVisualStyleBackColor = true;
			this.radioNewPatText.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioNewPatNone
			// 
			this.radioNewPatNone.Location = new System.Drawing.Point(7, 20);
			this.radioNewPatNone.Name = "radioNewPatNone";
			this.radioNewPatNone.Size = new System.Drawing.Size(175, 17);
			this.radioNewPatNone.TabIndex = 0;
			this.radioNewPatNone.TabStop = true;
			this.radioNewPatNone.Tag = OpenDentBusiness.WebSchedVerifyType.None;
			this.radioNewPatNone.Text = "None";
			this.radioNewPatNone.UseVisualStyleBackColor = true;
			this.radioNewPatNone.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// groupBoxRecall
			// 
			this.groupBoxRecall.Controls.Add(this.groupBoxRecallTextTemplate);
			this.groupBoxRecall.Controls.Add(this.groupBoxRecallEmail);
			this.groupBoxRecall.Controls.Add(this.groupBoxRadioRecall);
			this.groupBoxRecall.Location = new System.Drawing.Point(6, 32);
			this.groupBoxRecall.Name = "groupBoxRecall";
			this.groupBoxRecall.Size = new System.Drawing.Size(372, 497);
			this.groupBoxRecall.TabIndex = 0;
			this.groupBoxRecall.TabStop = false;
			this.groupBoxRecall.Text = "Recall";
			// 
			// groupBoxRecallTextTemplate
			// 
			this.groupBoxRecallTextTemplate.Controls.Add(this.textRecallTextTemplate);
			this.groupBoxRecallTextTemplate.Location = new System.Drawing.Point(6, 139);
			this.groupBoxRecallTextTemplate.Name = "groupBoxRecallTextTemplate";
			this.groupBoxRecallTextTemplate.Size = new System.Drawing.Size(360, 120);
			this.groupBoxRecallTextTemplate.TabIndex = 121;
			this.groupBoxRecallTextTemplate.TabStop = false;
			this.groupBoxRecallTextTemplate.Text = "Text Message";
			// 
			// textRecallTextTemplate
			// 
			this.textRecallTextTemplate.ContextMenuStrip = this.menuWebSchedVerifyTextTemplate;
			this.textRecallTextTemplate.Location = new System.Drawing.Point(6, 19);
			this.textRecallTextTemplate.Multiline = true;
			this.textRecallTextTemplate.Name = "textRecallTextTemplate";
			this.textRecallTextTemplate.Size = new System.Drawing.Size(348, 95);
			this.textRecallTextTemplate.TabIndex = 314;
			this.textRecallTextTemplate.Tag = OpenDentBusiness.PrefName.WebSchedVerifyRecallText;
			this.textRecallTextTemplate.Leave += new System.EventHandler(this.WebSchedVerify_TextLeave);
			// 
			// groupBoxRecallEmail
			// 
			this.groupBoxRecallEmail.Controls.Add(this.butRecallEditEmail);
			this.groupBoxRecallEmail.Controls.Add(this.browserRecallEmailBody);
			this.groupBoxRecallEmail.Controls.Add(this.textRecallEmailSubj);
			this.groupBoxRecallEmail.Location = new System.Drawing.Point(6, 259);
			this.groupBoxRecallEmail.Name = "groupBoxRecallEmail";
			this.groupBoxRecallEmail.Size = new System.Drawing.Size(360, 232);
			this.groupBoxRecallEmail.TabIndex = 122;
			this.groupBoxRecallEmail.TabStop = false;
			this.groupBoxRecallEmail.Text = "E-mail Subject and Body";
			// 
			// butRecallEditEmail
			// 
			this.butRecallEditEmail.Location = new System.Drawing.Point(284, 209);
			this.butRecallEditEmail.Name = "butRecallEditEmail";
			this.butRecallEditEmail.Size = new System.Drawing.Size(70, 20);
			this.butRecallEditEmail.TabIndex = 318;
			this.butRecallEditEmail.Text = "Edit";
			this.butRecallEditEmail.UseVisualStyleBackColor = true;
			this.butRecallEditEmail.Click += new System.EventHandler(this.WebSchedVerify_RecallEditEmailClick);
			// 
			// browserRecallEmailBody
			// 
			this.browserRecallEmailBody.AllowWebBrowserDrop = false;
			this.browserRecallEmailBody.Location = new System.Drawing.Point(7, 45);
			this.browserRecallEmailBody.MinimumSize = new System.Drawing.Size(20, 20);
			this.browserRecallEmailBody.Name = "browserRecallEmailBody";
			this.browserRecallEmailBody.Size = new System.Drawing.Size(347, 158);
			this.browserRecallEmailBody.TabIndex = 317;
			this.browserRecallEmailBody.WebBrowserShortcutsEnabled = false;
			// 
			// textRecallEmailSubj
			// 
			this.textRecallEmailSubj.ContextMenuStrip = this.menuWebSchedVerifyTextTemplate;
			this.textRecallEmailSubj.Location = new System.Drawing.Point(6, 19);
			this.textRecallEmailSubj.Multiline = true;
			this.textRecallEmailSubj.Name = "textRecallEmailSubj";
			this.textRecallEmailSubj.Size = new System.Drawing.Size(348, 20);
			this.textRecallEmailSubj.TabIndex = 314;
			this.textRecallEmailSubj.Tag = OpenDentBusiness.PrefName.WebSchedVerifyRecallEmailSubj;
			this.textRecallEmailSubj.Leave += new System.EventHandler(this.WebSchedVerify_TextLeave);
			// 
			// groupBoxRadioRecall
			// 
			this.groupBoxRadioRecall.Controls.Add(this.radioRecallTextAndEmail);
			this.groupBoxRadioRecall.Controls.Add(this.radioRecallEmail);
			this.groupBoxRadioRecall.Controls.Add(this.radioRecallText);
			this.groupBoxRadioRecall.Controls.Add(this.radioRecallNone);
			this.groupBoxRadioRecall.Location = new System.Drawing.Point(6, 18);
			this.groupBoxRadioRecall.Name = "groupBoxRadioRecall";
			this.groupBoxRadioRecall.Size = new System.Drawing.Size(360, 115);
			this.groupBoxRadioRecall.TabIndex = 1;
			this.groupBoxRadioRecall.TabStop = false;
			this.groupBoxRadioRecall.Tag = OpenDentBusiness.PrefName.WebSchedVerifyRecallType;
			this.groupBoxRadioRecall.Text = "Communication Method";
			// 
			// radioRecallTextAndEmail
			// 
			this.radioRecallTextAndEmail.Location = new System.Drawing.Point(7, 89);
			this.radioRecallTextAndEmail.Name = "radioRecallTextAndEmail";
			this.radioRecallTextAndEmail.Size = new System.Drawing.Size(175, 17);
			this.radioRecallTextAndEmail.TabIndex = 3;
			this.radioRecallTextAndEmail.TabStop = true;
			this.radioRecallTextAndEmail.Tag = OpenDentBusiness.WebSchedVerifyType.TextAndEmail;
			this.radioRecallTextAndEmail.Text = "Text and E-mail";
			this.radioRecallTextAndEmail.UseVisualStyleBackColor = true;
			this.radioRecallTextAndEmail.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioRecallEmail
			// 
			this.radioRecallEmail.Location = new System.Drawing.Point(7, 66);
			this.radioRecallEmail.Name = "radioRecallEmail";
			this.radioRecallEmail.Size = new System.Drawing.Size(175, 17);
			this.radioRecallEmail.TabIndex = 2;
			this.radioRecallEmail.TabStop = true;
			this.radioRecallEmail.Tag = OpenDentBusiness.WebSchedVerifyType.Email;
			this.radioRecallEmail.Text = "E-mail";
			this.radioRecallEmail.UseVisualStyleBackColor = true;
			this.radioRecallEmail.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioRecallText
			// 
			this.radioRecallText.Location = new System.Drawing.Point(7, 43);
			this.radioRecallText.Name = "radioRecallText";
			this.radioRecallText.Size = new System.Drawing.Size(175, 17);
			this.radioRecallText.TabIndex = 1;
			this.radioRecallText.TabStop = true;
			this.radioRecallText.Tag = OpenDentBusiness.WebSchedVerifyType.Text;
			this.radioRecallText.Text = "Text";
			this.radioRecallText.UseVisualStyleBackColor = true;
			this.radioRecallText.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// radioRecallNone
			// 
			this.radioRecallNone.Location = new System.Drawing.Point(7, 20);
			this.radioRecallNone.Name = "radioRecallNone";
			this.radioRecallNone.Size = new System.Drawing.Size(175, 17);
			this.radioRecallNone.TabIndex = 0;
			this.radioRecallNone.TabStop = true;
			this.radioRecallNone.Tag = OpenDentBusiness.WebSchedVerifyType.None;
			this.radioRecallNone.Text = "None";
			this.radioRecallNone.UseVisualStyleBackColor = true;
			this.radioRecallNone.CheckedChanged += new System.EventHandler(this.WebSchedVerify_RadioButtonCheckChanged);
			// 
			// comboClinicVerify
			// 
			this.comboClinicVerify.HqDescription = "Default";
			this.comboClinicVerify.IncludeUnassigned = true;
			this.comboClinicVerify.Location = new System.Drawing.Point(150, 8);
			this.comboClinicVerify.Name = "comboClinicVerify";
			this.comboClinicVerify.Size = new System.Drawing.Size(216, 21);
			this.comboClinicVerify.TabIndex = 268;
			this.comboClinicVerify.SelectedIndexChanged += new System.EventHandler(this.WebSchedVerify_ComboClinicSelectedIndexChanged);
			// 
			// tabTexting
			// 
			this.tabTexting.BackColor = System.Drawing.SystemColors.Control;
			this.tabTexting.Controls.Add(this.groupShortCode);
			this.tabTexting.Controls.Add(this.butDefaultClinicClear);
			this.tabTexting.Controls.Add(this.butDefaultClinic);
			this.tabTexting.Controls.Add(this.butBackMonth);
			this.tabTexting.Controls.Add(this.dateTimePickerSms);
			this.tabTexting.Controls.Add(this.gridSmsSummary);
			this.tabTexting.Controls.Add(this.butFwdMonth);
			this.tabTexting.Controls.Add(this.butThisMonth);
			this.tabTexting.Location = new System.Drawing.Point(4, 22);
			this.tabTexting.Name = "tabTexting";
			this.tabTexting.Padding = new System.Windows.Forms.Padding(3);
			this.tabTexting.Size = new System.Drawing.Size(1154, 595);
			this.tabTexting.TabIndex = 6;
			this.tabTexting.Text = "Texting Services";
			// 
			// groupShortCode
			// 
			this.groupShortCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupShortCode.Controls.Add(this.labelUnsavedShortCodeChanges);
			this.groupShortCode.Controls.Add(this.butSaveShortCodes);
			this.groupShortCode.Controls.Add(this.labelShortCodeOptInClinicTitle);
			this.groupShortCode.Controls.Add(this.textShortCodeOptInClinicTitle);
			this.groupShortCode.Controls.Add(this.checkOptInPrompt);
			this.groupShortCode.Controls.Add(this.comboShortCodeClinic);
			this.groupShortCode.Location = new System.Drawing.Point(3, 506);
			this.groupShortCode.Name = "groupShortCode";
			this.groupShortCode.Size = new System.Drawing.Size(375, 85);
			this.groupShortCode.TabIndex = 270;
			this.groupShortCode.TabStop = false;
			this.groupShortCode.Text = "Appt Texts";
			// 
			// labelUnsavedShortCodeChanges
			// 
			this.labelUnsavedShortCodeChanges.ForeColor = System.Drawing.Color.Red;
			this.labelUnsavedShortCodeChanges.Location = new System.Drawing.Point(188, 59);
			this.labelUnsavedShortCodeChanges.Name = "labelUnsavedShortCodeChanges";
			this.labelUnsavedShortCodeChanges.Size = new System.Drawing.Size(100, 19);
			this.labelUnsavedShortCodeChanges.TabIndex = 272;
			this.labelUnsavedShortCodeChanges.Text = "unsaved changes*";
			this.labelUnsavedShortCodeChanges.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butSaveShortCodes
			// 
			this.butSaveShortCodes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSaveShortCodes.Location = new System.Drawing.Point(291, 58);
			this.butSaveShortCodes.Name = "butSaveShortCodes";
			this.butSaveShortCodes.Size = new System.Drawing.Size(81, 23);
			this.butSaveShortCodes.TabIndex = 271;
			this.butSaveShortCodes.Text = "Save";
			this.butSaveShortCodes.UseVisualStyleBackColor = true;
			this.butSaveShortCodes.Click += new System.EventHandler(this.butSaveShortCodes_Click);
			// 
			// labelShortCodeOptInClinicTitle
			// 
			this.labelShortCodeOptInClinicTitle.Location = new System.Drawing.Point(74, 37);
			this.labelShortCodeOptInClinicTitle.Name = "labelShortCodeOptInClinicTitle";
			this.labelShortCodeOptInClinicTitle.Size = new System.Drawing.Size(100, 18);
			this.labelShortCodeOptInClinicTitle.TabIndex = 3;
			this.labelShortCodeOptInClinicTitle.Text = "OptIn Office Title";
			this.labelShortCodeOptInClinicTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textShortCodeOptInClinicTitle
			// 
			this.textShortCodeOptInClinicTitle.Location = new System.Drawing.Point(174, 35);
			this.textShortCodeOptInClinicTitle.Name = "textShortCodeOptInClinicTitle";
			this.textShortCodeOptInClinicTitle.Size = new System.Drawing.Size(198, 20);
			this.textShortCodeOptInClinicTitle.TabIndex = 2;
			this.textShortCodeOptInClinicTitle.TextChanged += new System.EventHandler(this.textShortCodeOptInClinicTitle_TextChanged);
			// 
			// checkOptInPrompt
			// 
			this.checkOptInPrompt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkOptInPrompt.Location = new System.Drawing.Point(1, 55);
			this.checkOptInPrompt.Name = "checkOptInPrompt";
			this.checkOptInPrompt.Size = new System.Drawing.Size(187, 24);
			this.checkOptInPrompt.TabIndex = 1;
			this.checkOptInPrompt.Text = "Opt In Prompt on Appt Complete";
			this.checkOptInPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkOptInPrompt.UseVisualStyleBackColor = true;
			this.checkOptInPrompt.Click += new System.EventHandler(this.checkOptInPrompt_Click);
			// 
			// comboShortCodeClinic
			// 
			this.comboShortCodeClinic.HqDescription = "Default";
			this.comboShortCodeClinic.IncludeUnassigned = true;
			this.comboShortCodeClinic.Location = new System.Drawing.Point(137, 12);
			this.comboShortCodeClinic.Name = "comboShortCodeClinic";
			this.comboShortCodeClinic.SelectionModeMulti = true;
			this.comboShortCodeClinic.Size = new System.Drawing.Size(235, 21);
			this.comboShortCodeClinic.TabIndex = 0;
			this.comboShortCodeClinic.SelectionChangeCommitted += new System.EventHandler(this.comboShortCodeClinic_SelectionChangeCommitted);
			// 
			// butDefaultClinicClear
			// 
			this.butDefaultClinicClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDefaultClinicClear.Location = new System.Drawing.Point(980, 512);
			this.butDefaultClinicClear.Name = "butDefaultClinicClear";
			this.butDefaultClinicClear.Size = new System.Drawing.Size(81, 23);
			this.butDefaultClinicClear.TabIndex = 269;
			this.butDefaultClinicClear.Text = "Clear Default";
			this.butDefaultClinicClear.UseVisualStyleBackColor = true;
			this.butDefaultClinicClear.Click += new System.EventHandler(this.butDefaultClinicClear_Click);
			// 
			// butDefaultClinic
			// 
			this.butDefaultClinic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDefaultClinic.Location = new System.Drawing.Point(1067, 512);
			this.butDefaultClinic.Name = "butDefaultClinic";
			this.butDefaultClinic.Size = new System.Drawing.Size(81, 23);
			this.butDefaultClinic.TabIndex = 262;
			this.butDefaultClinic.Text = "Set Default";
			this.butDefaultClinic.UseVisualStyleBackColor = true;
			this.butDefaultClinic.Click += new System.EventHandler(this.butDefaultClinic_Click);
			// 
			// butBackMonth
			// 
			this.butBackMonth.AdjustImageLocation = new System.Drawing.Point(-3, -1);
			this.butBackMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butBackMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butBackMonth.Image = ((System.Drawing.Image)(resources.GetObject("butBackMonth.Image")));
			this.butBackMonth.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butBackMonth.Location = new System.Drawing.Point(490, 515);
			this.butBackMonth.Name = "butBackMonth";
			this.butBackMonth.Size = new System.Drawing.Size(32, 22);
			this.butBackMonth.TabIndex = 268;
			this.butBackMonth.Text = "M";
			this.butBackMonth.Click += new System.EventHandler(this.butBackMonth_Click);
			// 
			// dateTimePickerSms
			// 
			this.dateTimePickerSms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.dateTimePickerSms.CustomFormat = "MMM yyyy";
			this.dateTimePickerSms.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateTimePickerSms.Location = new System.Drawing.Point(522, 515);
			this.dateTimePickerSms.Name = "dateTimePickerSms";
			this.dateTimePickerSms.Size = new System.Drawing.Size(113, 20);
			this.dateTimePickerSms.TabIndex = 258;
			this.dateTimePickerSms.ValueChanged += new System.EventHandler(this.dateTimePickerSms_ValueChanged);
			// 
			// gridSmsSummary
			// 
			this.gridSmsSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridSmsSummary.HasMultilineHeaders = true;
			this.gridSmsSummary.Location = new System.Drawing.Point(0, 6);
			this.gridSmsSummary.Name = "gridSmsSummary";
			this.gridSmsSummary.Size = new System.Drawing.Size(1150, 500);
			this.gridSmsSummary.TabIndex = 252;
			this.gridSmsSummary.Title = "Text Messaging Phone Number and Usage Summary";
			this.gridSmsSummary.TranslationName = "FormEServicesSetup";
			this.gridSmsSummary.WrapText = false;
			// 
			// butFwdMonth
			// 
			this.butFwdMonth.AdjustImageLocation = new System.Drawing.Point(5, -1);
			this.butFwdMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butFwdMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butFwdMonth.Image = ((System.Drawing.Image)(resources.GetObject("butFwdMonth.Image")));
			this.butFwdMonth.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butFwdMonth.Location = new System.Drawing.Point(635, 515);
			this.butFwdMonth.Name = "butFwdMonth";
			this.butFwdMonth.Size = new System.Drawing.Size(29, 22);
			this.butFwdMonth.TabIndex = 267;
			this.butFwdMonth.Text = "M";
			this.butFwdMonth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butFwdMonth.Click += new System.EventHandler(this.butFwdMonth_Click);
			// 
			// butThisMonth
			// 
			this.butThisMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butThisMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butThisMonth.Location = new System.Drawing.Point(541, 540);
			this.butThisMonth.Name = "butThisMonth";
			this.butThisMonth.Size = new System.Drawing.Size(75, 22);
			this.butThisMonth.TabIndex = 262;
			this.butThisMonth.Text = "This Month";
			this.butThisMonth.Click += new System.EventHandler(this.butThisMonth_Click);
			// 
			// tabECR
			// 
			this.tabECR.Controls.Add(this.butAddThankYouVerify);
			this.tabECR.Controls.Add(this.label11);
			this.tabECR.Controls.Add(this.gridConfStatuses);
			this.tabECR.Controls.Add(this.checkUseDefaultsEC);
			this.tabECR.Controls.Add(this.textStatusReminders);
			this.tabECR.Controls.Add(this.butActivateReminder);
			this.tabECR.Controls.Add(this.textStatusConfirmations);
			this.tabECR.Controls.Add(this.butActivateConfirm);
			this.tabECR.Controls.Add(this.groupAutomationStatuses);
			this.tabECR.Controls.Add(this.checkIsConfirmEnabled);
			this.tabECR.Controls.Add(this.comboClinicEConfirm);
			this.tabECR.Controls.Add(this.label54);
			this.tabECR.Controls.Add(this.butAddConfirmation);
			this.tabECR.Controls.Add(this.butAddReminder);
			this.tabECR.Controls.Add(this.gridRemindersMain);
			this.tabECR.Location = new System.Drawing.Point(4, 22);
			this.tabECR.Name = "tabECR";
			this.tabECR.Padding = new System.Windows.Forms.Padding(3);
			this.tabECR.Size = new System.Drawing.Size(1154, 595);
			this.tabECR.TabIndex = 8;
			this.tabECR.Text = "Automated eReminders & eConfirmations";
			this.tabECR.UseVisualStyleBackColor = true;
			// 
			// butAddThankYouVerify
			// 
			this.butAddThankYouVerify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAddThankYouVerify.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddThankYouVerify.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddThankYouVerify.Location = new System.Drawing.Point(665, 564);
			this.butAddThankYouVerify.Name = "butAddThankYouVerify";
			this.butAddThankYouVerify.Size = new System.Drawing.Size(147, 23);
			this.butAddThankYouVerify.TabIndex = 271;
			this.butAddThankYouVerify.Text = "Add Thank You Rule";
			this.butAddThankYouVerify.UseVisualStyleBackColor = true;
			this.butAddThankYouVerify.Visible = false;
			this.butAddThankYouVerify.Click += new System.EventHandler(this.butAddThankYou_Click);
			// 
			// label11
			// 
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label11.Location = new System.Drawing.Point(18, 527);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(272, 65);
			this.label11.TabIndex = 175;
			this.label11.Text = "Don\'t Send: If an appointment has this status, do not send a confirmation.\r\nDon\'t" +
    " Change: If an appointment has this status, do not change status when a response" +
    " is received.";
			// 
			// gridConfStatuses
			// 
			this.gridConfStatuses.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridConfStatuses.HasMultilineHeaders = true;
			this.gridConfStatuses.Location = new System.Drawing.Point(18, 301);
			this.gridConfStatuses.Name = "gridConfStatuses";
			this.gridConfStatuses.Size = new System.Drawing.Size(272, 220);
			this.gridConfStatuses.TabIndex = 265;
			this.gridConfStatuses.Title = "Confirmation Statuses";
			this.gridConfStatuses.TranslationName = "TableStatuses";
			this.gridConfStatuses.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridConfStatuses_CellDoubleClick);
			// 
			// checkUseDefaultsEC
			// 
			this.checkUseDefaultsEC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkUseDefaultsEC.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUseDefaultsEC.Location = new System.Drawing.Point(554, 568);
			this.checkUseDefaultsEC.Name = "checkUseDefaultsEC";
			this.checkUseDefaultsEC.Size = new System.Drawing.Size(105, 19);
			this.checkUseDefaultsEC.TabIndex = 263;
			this.checkUseDefaultsEC.Text = "Use Defaults";
			this.checkUseDefaultsEC.Click += new System.EventHandler(this.checkUseDefaultsEC_CheckedChanged);
			// 
			// textStatusReminders
			// 
			this.textStatusReminders.Location = new System.Drawing.Point(6, 37);
			this.textStatusReminders.Name = "textStatusReminders";
			this.textStatusReminders.ReadOnly = true;
			this.textStatusReminders.Size = new System.Drawing.Size(131, 20);
			this.textStatusReminders.TabIndex = 262;
			this.textStatusReminders.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// butActivateReminder
			// 
			this.butActivateReminder.Location = new System.Drawing.Point(143, 35);
			this.butActivateReminder.Name = "butActivateReminder";
			this.butActivateReminder.Size = new System.Drawing.Size(147, 23);
			this.butActivateReminder.TabIndex = 261;
			this.butActivateReminder.Text = "Activate eReminders";
			this.butActivateReminder.UseVisualStyleBackColor = true;
			this.butActivateReminder.Click += new System.EventHandler(this.butActivateReminder_Click);
			// 
			// textStatusConfirmations
			// 
			this.textStatusConfirmations.Location = new System.Drawing.Point(6, 66);
			this.textStatusConfirmations.Name = "textStatusConfirmations";
			this.textStatusConfirmations.ReadOnly = true;
			this.textStatusConfirmations.Size = new System.Drawing.Size(131, 20);
			this.textStatusConfirmations.TabIndex = 260;
			this.textStatusConfirmations.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// butActivateConfirm
			// 
			this.butActivateConfirm.Location = new System.Drawing.Point(143, 64);
			this.butActivateConfirm.Name = "butActivateConfirm";
			this.butActivateConfirm.Size = new System.Drawing.Size(147, 23);
			this.butActivateConfirm.TabIndex = 257;
			this.butActivateConfirm.Text = "Activate eConfirmations";
			this.butActivateConfirm.UseVisualStyleBackColor = true;
			this.butActivateConfirm.Click += new System.EventHandler(this.butActivateConfirm_Click);
			// 
			// groupAutomationStatuses
			// 
			this.groupAutomationStatuses.Controls.Add(this.radio2ClickConfirm);
			this.groupAutomationStatuses.Controls.Add(this.radio1ClickConfirm);
			this.groupAutomationStatuses.Controls.Add(this.comboStatusEFailed);
			this.groupAutomationStatuses.Controls.Add(this.label50);
			this.groupAutomationStatuses.Controls.Add(this.checkEnableNoClinic);
			this.groupAutomationStatuses.Controls.Add(this.comboStatusEDeclined);
			this.groupAutomationStatuses.Controls.Add(this.comboStatusESent);
			this.groupAutomationStatuses.Controls.Add(this.comboStatusEAccepted);
			this.groupAutomationStatuses.Controls.Add(this.label51);
			this.groupAutomationStatuses.Controls.Add(this.label52);
			this.groupAutomationStatuses.Controls.Add(this.label53);
			this.groupAutomationStatuses.Location = new System.Drawing.Point(18, 95);
			this.groupAutomationStatuses.Name = "groupAutomationStatuses";
			this.groupAutomationStatuses.Size = new System.Drawing.Size(272, 200);
			this.groupAutomationStatuses.TabIndex = 169;
			this.groupAutomationStatuses.TabStop = false;
			this.groupAutomationStatuses.Text = "Global eConfirmation Settings";
			// 
			// radio2ClickConfirm
			// 
			this.radio2ClickConfirm.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radio2ClickConfirm.Location = new System.Drawing.Point(3, 164);
			this.radio2ClickConfirm.Name = "radio2ClickConfirm";
			this.radio2ClickConfirm.Size = new System.Drawing.Size(245, 30);
			this.radio2ClickConfirm.TabIndex = 176;
			this.radio2ClickConfirm.TabStop = true;
			this.radio2ClickConfirm.Text = "Confirm in portal after clicking link (2-click confirmation)";
			this.radio2ClickConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// radio1ClickConfirm
			// 
			this.radio1ClickConfirm.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radio1ClickConfirm.Location = new System.Drawing.Point(3, 137);
			this.radio1ClickConfirm.Name = "radio1ClickConfirm";
			this.radio1ClickConfirm.Size = new System.Drawing.Size(245, 31);
			this.radio1ClickConfirm.TabIndex = 175;
			this.radio1ClickConfirm.TabStop = true;
			this.radio1ClickConfirm.Text = "Confirm from link in message (1-click confirmation)";
			this.radio1ClickConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboStatusEFailed
			// 
			this.comboStatusEFailed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboStatusEFailed.Location = new System.Drawing.Point(97, 92);
			this.comboStatusEFailed.Name = "comboStatusEFailed";
			this.comboStatusEFailed.Size = new System.Drawing.Size(151, 21);
			this.comboStatusEFailed.TabIndex = 173;
			// 
			// label50
			// 
			this.label50.Location = new System.Drawing.Point(6, 93);
			this.label50.Name = "label50";
			this.label50.Size = new System.Drawing.Size(89, 16);
			this.label50.TabIndex = 174;
			this.label50.Text = "Failed";
			this.label50.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkEnableNoClinic
			// 
			this.checkEnableNoClinic.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEnableNoClinic.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEnableNoClinic.Location = new System.Drawing.Point(9, 117);
			this.checkEnableNoClinic.Name = "checkEnableNoClinic";
			this.checkEnableNoClinic.Size = new System.Drawing.Size(239, 17);
			this.checkEnableNoClinic.TabIndex = 172;
			this.checkEnableNoClinic.Text = "Allow eMessages from Appts w/o Clinic";
			this.checkEnableNoClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboStatusEDeclined
			// 
			this.comboStatusEDeclined.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboStatusEDeclined.Location = new System.Drawing.Point(97, 67);
			this.comboStatusEDeclined.Name = "comboStatusEDeclined";
			this.comboStatusEDeclined.Size = new System.Drawing.Size(151, 21);
			this.comboStatusEDeclined.TabIndex = 170;
			// 
			// comboStatusESent
			// 
			this.comboStatusESent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboStatusESent.Location = new System.Drawing.Point(97, 17);
			this.comboStatusESent.Name = "comboStatusESent";
			this.comboStatusESent.Size = new System.Drawing.Size(151, 21);
			this.comboStatusESent.TabIndex = 166;
			// 
			// comboStatusEAccepted
			// 
			this.comboStatusEAccepted.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboStatusEAccepted.Location = new System.Drawing.Point(97, 42);
			this.comboStatusEAccepted.Name = "comboStatusEAccepted";
			this.comboStatusEAccepted.Size = new System.Drawing.Size(151, 21);
			this.comboStatusEAccepted.TabIndex = 168;
			// 
			// label51
			// 
			this.label51.Location = new System.Drawing.Point(6, 68);
			this.label51.Name = "label51";
			this.label51.Size = new System.Drawing.Size(89, 16);
			this.label51.TabIndex = 171;
			this.label51.Text = "Not Accepted";
			this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label52
			// 
			this.label52.Location = new System.Drawing.Point(6, 18);
			this.label52.Name = "label52";
			this.label52.Size = new System.Drawing.Size(89, 16);
			this.label52.TabIndex = 167;
			this.label52.Text = "Sent";
			this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label53
			// 
			this.label53.Location = new System.Drawing.Point(6, 43);
			this.label53.Name = "label53";
			this.label53.Size = new System.Drawing.Size(89, 16);
			this.label53.TabIndex = 169;
			this.label53.Text = "Accepted";
			this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIsConfirmEnabled
			// 
			this.checkIsConfirmEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsConfirmEnabled.Location = new System.Drawing.Point(630, 9);
			this.checkIsConfirmEnabled.Name = "checkIsConfirmEnabled";
			this.checkIsConfirmEnabled.Size = new System.Drawing.Size(216, 19);
			this.checkIsConfirmEnabled.TabIndex = 167;
			this.checkIsConfirmEnabled.Text = "Enable Automation for Clinic";
			this.checkIsConfirmEnabled.CheckedChanged += new System.EventHandler(this.checkIsConfirmEnabled_CheckedChanged);
			// 
			// comboClinicEConfirm
			// 
			this.comboClinicEConfirm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinicEConfirm.Location = new System.Drawing.Point(427, 8);
			this.comboClinicEConfirm.MaxDropDownItems = 30;
			this.comboClinicEConfirm.Name = "comboClinicEConfirm";
			this.comboClinicEConfirm.Size = new System.Drawing.Size(194, 21);
			this.comboClinicEConfirm.TabIndex = 164;
			this.comboClinicEConfirm.SelectedIndexChanged += new System.EventHandler(this.comboClinicEConfirm_SelectedIndexChanged);
			// 
			// label54
			// 
			this.label54.Location = new System.Drawing.Point(366, 9);
			this.label54.Name = "label54";
			this.label54.Size = new System.Drawing.Size(57, 16);
			this.label54.TabIndex = 165;
			this.label54.Text = "Clinic";
			this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butAddConfirmation
			// 
			this.butAddConfirmation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAddConfirmation.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddConfirmation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddConfirmation.Location = new System.Drawing.Point(296, 565);
			this.butAddConfirmation.Name = "butAddConfirmation";
			this.butAddConfirmation.Size = new System.Drawing.Size(119, 24);
			this.butAddConfirmation.TabIndex = 93;
			this.butAddConfirmation.Text = "Add Confirmation";
			this.butAddConfirmation.UseVisualStyleBackColor = true;
			this.butAddConfirmation.Click += new System.EventHandler(this.butAddConfirmation_Click);
			// 
			// butAddReminder
			// 
			this.butAddReminder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAddReminder.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddReminder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddReminder.Location = new System.Drawing.Point(421, 565);
			this.butAddReminder.Name = "butAddReminder";
			this.butAddReminder.Size = new System.Drawing.Size(124, 24);
			this.butAddReminder.TabIndex = 92;
			this.butAddReminder.Text = "Add  Reminder";
			this.butAddReminder.UseVisualStyleBackColor = true;
			this.butAddReminder.Click += new System.EventHandler(this.butAddReminder_Click);
			// 
			// gridRemindersMain
			// 
			this.gridRemindersMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridRemindersMain.HasMultilineHeaders = true;
			this.gridRemindersMain.Location = new System.Drawing.Point(296, 35);
			this.gridRemindersMain.Name = "gridRemindersMain";
			this.gridRemindersMain.Size = new System.Drawing.Size(852, 524);
			this.gridRemindersMain.TabIndex = 68;
			this.gridRemindersMain.Title = "eReminder and eConfirmation Rules";
			this.gridRemindersMain.TranslationName = "TableRules";
			this.gridRemindersMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridRemindersMain_CellDoubleClick);
			// 
			// tabEClipboard
			// 
			this.tabEClipboard.BackColor = System.Drawing.SystemColors.Control;
			this.tabEClipboard.Controls.Add(this.labelEClipboardNotSignedUp);
			this.tabEClipboard.Controls.Add(this.clinicPickerEClipboard);
			this.tabEClipboard.Controls.Add(this.label10);
			this.tabEClipboard.Controls.Add(this.gridMobileAppDevices);
			this.tabEClipboard.Controls.Add(this.groupEClipboardSheets);
			this.tabEClipboard.Controls.Add(this.groupEClipboardRules);
			this.tabEClipboard.Controls.Add(this.checkEClipboardUseDefaults);
			this.tabEClipboard.Location = new System.Drawing.Point(4, 22);
			this.tabEClipboard.Name = "tabEClipboard";
			this.tabEClipboard.Padding = new System.Windows.Forms.Padding(3);
			this.tabEClipboard.Size = new System.Drawing.Size(1154, 595);
			this.tabEClipboard.TabIndex = 11;
			this.tabEClipboard.Text = "eClipboard";
			// 
			// labelEClipboardNotSignedUp
			// 
			this.labelEClipboardNotSignedUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelEClipboardNotSignedUp.Location = new System.Drawing.Point(12, 6);
			this.labelEClipboardNotSignedUp.Name = "labelEClipboardNotSignedUp";
			this.labelEClipboardNotSignedUp.Size = new System.Drawing.Size(321, 19);
			this.labelEClipboardNotSignedUp.TabIndex = 273;
			this.labelEClipboardNotSignedUp.Text = "Go to the Signup Portal to enable this feature.";
			this.labelEClipboardNotSignedUp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// clinicPickerEClipboard
			// 
			this.clinicPickerEClipboard.HqDescription = "Default";
			this.clinicPickerEClipboard.IncludeUnassigned = true;
			this.clinicPickerEClipboard.Location = new System.Drawing.Point(453, 12);
			this.clinicPickerEClipboard.Name = "clinicPickerEClipboard";
			this.clinicPickerEClipboard.Size = new System.Drawing.Size(200, 21);
			this.clinicPickerEClipboard.TabIndex = 272;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(827, 26);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(321, 19);
			this.label10.TabIndex = 271;
			this.label10.Text = "To add devices to this list, log in to the mobile app via the device. ";
			// 
			// gridMobileAppDevices
			// 
			this.gridMobileAppDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMobileAppDevices.Location = new System.Drawing.Point(508, 48);
			this.gridMobileAppDevices.Name = "gridMobileAppDevices";
			this.gridMobileAppDevices.Size = new System.Drawing.Size(640, 523);
			this.gridMobileAppDevices.TabIndex = 270;
			this.gridMobileAppDevices.Title = "Mobile App Devices";
			this.gridMobileAppDevices.TranslationName = "Checkin Devices";
			this.gridMobileAppDevices.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMobileAppDevices_CellClick);
			// 
			// groupEClipboardSheets
			// 
			this.groupEClipboardSheets.Controls.Add(this.butEClipboardAddSheets);
			this.groupEClipboardSheets.Controls.Add(this.butEClipboardUp);
			this.groupEClipboardSheets.Controls.Add(this.butEClipboardDown);
			this.groupEClipboardSheets.Controls.Add(this.butEClipboardRight);
			this.groupEClipboardSheets.Controls.Add(this.butEClipboardLeft);
			this.groupEClipboardSheets.Controls.Add(this.gridEClipboardSheetsInUse);
			this.groupEClipboardSheets.Controls.Add(this.listEClipboardSheetsAvailable);
			this.groupEClipboardSheets.Controls.Add(this.labelOps);
			this.groupEClipboardSheets.Location = new System.Drawing.Point(15, 291);
			this.groupEClipboardSheets.Name = "groupEClipboardSheets";
			this.groupEClipboardSheets.Size = new System.Drawing.Size(487, 273);
			this.groupEClipboardSheets.TabIndex = 266;
			this.groupEClipboardSheets.TabStop = false;
			this.groupEClipboardSheets.Text = "Specify which forms are added upon patient arrival";
			// 
			// butEClipboardAddSheets
			// 
			this.butEClipboardAddSheets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butEClipboardAddSheets.Location = new System.Drawing.Point(6, 244);
			this.butEClipboardAddSheets.Name = "butEClipboardAddSheets";
			this.butEClipboardAddSheets.Size = new System.Drawing.Size(170, 23);
			this.butEClipboardAddSheets.TabIndex = 503;
			this.butEClipboardAddSheets.Text = "Add Custom Sheets";
			this.butEClipboardAddSheets.UseVisualStyleBackColor = true;
			this.butEClipboardAddSheets.Click += new System.EventHandler(this.ButEClipboardAddSheets_Click);
			// 
			// butEClipboardUp
			// 
			this.butEClipboardUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butEClipboardUp.Image = global::OpenDental.Properties.Resources.up;
			this.butEClipboardUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEClipboardUp.Location = new System.Drawing.Point(444, 208);
			this.butEClipboardUp.Name = "butEClipboardUp";
			this.butEClipboardUp.Size = new System.Drawing.Size(33, 26);
			this.butEClipboardUp.TabIndex = 267;
			this.butEClipboardUp.Click += new System.EventHandler(this.ButEClipboardUp_Click);
			// 
			// butEClipboardDown
			// 
			this.butEClipboardDown.Image = global::OpenDental.Properties.Resources.down;
			this.butEClipboardDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEClipboardDown.Location = new System.Drawing.Point(444, 240);
			this.butEClipboardDown.Name = "butEClipboardDown";
			this.butEClipboardDown.Size = new System.Drawing.Size(33, 26);
			this.butEClipboardDown.TabIndex = 268;
			this.butEClipboardDown.Click += new System.EventHandler(this.ButEClipboardDown_Click);
			// 
			// butEClipboardRight
			// 
			this.butEClipboardRight.Image = global::OpenDental.Properties.Resources.Right;
			this.butEClipboardRight.Location = new System.Drawing.Point(182, 97);
			this.butEClipboardRight.Name = "butEClipboardRight";
			this.butEClipboardRight.Size = new System.Drawing.Size(35, 26);
			this.butEClipboardRight.TabIndex = 64;
			this.butEClipboardRight.Click += new System.EventHandler(this.ButEClipboardRight_Click);
			// 
			// butEClipboardLeft
			// 
			this.butEClipboardLeft.AdjustImageLocation = new System.Drawing.Point(-1, 0);
			this.butEClipboardLeft.Image = global::OpenDental.Properties.Resources.Left;
			this.butEClipboardLeft.Location = new System.Drawing.Point(182, 131);
			this.butEClipboardLeft.Name = "butEClipboardLeft";
			this.butEClipboardLeft.Size = new System.Drawing.Size(35, 26);
			this.butEClipboardLeft.TabIndex = 63;
			this.butEClipboardLeft.Click += new System.EventHandler(this.ButEClipboardLeft_Click);
			// 
			// gridEClipboardSheetsInUse
			// 
			this.gridEClipboardSheetsInUse.Location = new System.Drawing.Point(223, 22);
			this.gridEClipboardSheetsInUse.Name = "gridEClipboardSheetsInUse";
			this.gridEClipboardSheetsInUse.Size = new System.Drawing.Size(215, 244);
			this.gridEClipboardSheetsInUse.TabIndex = 62;
			this.gridEClipboardSheetsInUse.Title = "Sheets In Use";
			this.gridEClipboardSheetsInUse.TranslationName = "TableAvailableRows";
			this.gridEClipboardSheetsInUse.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.GridEClipboardSheetsInUse_CellDoubleClick);
			// 
			// listEClipboardSheetsAvailable
			// 
			this.listEClipboardSheetsAvailable.Location = new System.Drawing.Point(6, 39);
			this.listEClipboardSheetsAvailable.Name = "listEClipboardSheetsAvailable";
			this.listEClipboardSheetsAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listEClipboardSheetsAvailable.Size = new System.Drawing.Size(170, 199);
			this.listEClipboardSheetsAvailable.TabIndex = 42;
			// 
			// labelOps
			// 
			this.labelOps.Location = new System.Drawing.Point(3, 17);
			this.labelOps.Name = "labelOps";
			this.labelOps.Size = new System.Drawing.Size(214, 18);
			this.labelOps.TabIndex = 41;
			this.labelOps.Text = "Available Sheets (Custom Sheets Only)";
			this.labelOps.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupEClipboardRules
			// 
			this.groupEClipboardRules.Controls.Add(this.checkEClipboardAllowSelfPortrait);
			this.groupEClipboardRules.Controls.Add(this.checkEClipboardCreateMissingForms);
			this.groupEClipboardRules.Controls.Add(this.labelEClipboardMessage);
			this.groupEClipboardRules.Controls.Add(this.textEClipboardMessage);
			this.groupEClipboardRules.Controls.Add(this.checkEClipboardAllowSheets);
			this.groupEClipboardRules.Controls.Add(this.checkEClipboardAllowCheckIn);
			this.groupEClipboardRules.Controls.Add(this.checkEClipboardPopupKiosk);
			this.groupEClipboardRules.Location = new System.Drawing.Point(15, 48);
			this.groupEClipboardRules.Name = "groupEClipboardRules";
			this.groupEClipboardRules.Size = new System.Drawing.Size(487, 237);
			this.groupEClipboardRules.TabIndex = 265;
			this.groupEClipboardRules.TabStop = false;
			this.groupEClipboardRules.Text = "Behavior Rules";
			// 
			// checkEClipboardAllowSelfPortrait
			// 
			this.checkEClipboardAllowSelfPortrait.Location = new System.Drawing.Point(19, 42);
			this.checkEClipboardAllowSelfPortrait.Name = "checkEClipboardAllowSelfPortrait";
			this.checkEClipboardAllowSelfPortrait.Size = new System.Drawing.Size(458, 17);
			this.checkEClipboardAllowSelfPortrait.TabIndex = 268;
			this.checkEClipboardAllowSelfPortrait.Text = "Allow patients to take self-portrait in mobile app";
			this.checkEClipboardAllowSelfPortrait.UseVisualStyleBackColor = true;
			this.checkEClipboardAllowSelfPortrait.Click += new System.EventHandler(this.CheckEClipboardAllowSelfPortrait_Click);
			// 
			// checkEClipboardCreateMissingForms
			// 
			this.checkEClipboardCreateMissingForms.Location = new System.Drawing.Point(19, 87);
			this.checkEClipboardCreateMissingForms.Name = "checkEClipboardCreateMissingForms";
			this.checkEClipboardCreateMissingForms.Size = new System.Drawing.Size(458, 17);
			this.checkEClipboardCreateMissingForms.TabIndex = 267;
			this.checkEClipboardCreateMissingForms.Text = "Add specified forms upon patient arrival";
			this.checkEClipboardCreateMissingForms.UseVisualStyleBackColor = true;
			this.checkEClipboardCreateMissingForms.Click += new System.EventHandler(this.CheckEClipboardCreateMissingForms_Click);
			// 
			// labelEClipboardMessage
			// 
			this.labelEClipboardMessage.Location = new System.Drawing.Point(16, 133);
			this.labelEClipboardMessage.Name = "labelEClipboardMessage";
			this.labelEClipboardMessage.Size = new System.Drawing.Size(461, 18);
			this.labelEClipboardMessage.TabIndex = 266;
			this.labelEClipboardMessage.Text = "Message to show patients after successful check-in";
			this.labelEClipboardMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textEClipboardMessage
			// 
			this.textEClipboardMessage.Location = new System.Drawing.Point(19, 154);
			this.textEClipboardMessage.Multiline = true;
			this.textEClipboardMessage.Name = "textEClipboardMessage";
			this.textEClipboardMessage.Size = new System.Drawing.Size(458, 66);
			this.textEClipboardMessage.TabIndex = 3;
			this.textEClipboardMessage.TextChanged += new System.EventHandler(this.TextEClipboardMessage_TextChanged);
			// 
			// checkEClipboardAllowSheets
			// 
			this.checkEClipboardAllowSheets.Location = new System.Drawing.Point(19, 64);
			this.checkEClipboardAllowSheets.Name = "checkEClipboardAllowSheets";
			this.checkEClipboardAllowSheets.Size = new System.Drawing.Size(458, 17);
			this.checkEClipboardAllowSheets.TabIndex = 2;
			this.checkEClipboardAllowSheets.Text = "Allow patients to fill out forms in mobile app";
			this.checkEClipboardAllowSheets.UseVisualStyleBackColor = true;
			this.checkEClipboardAllowSheets.Click += new System.EventHandler(this.CheckEClipboardAllowSheets_Click);
			// 
			// checkEClipboardAllowCheckIn
			// 
			this.checkEClipboardAllowCheckIn.Location = new System.Drawing.Point(19, 19);
			this.checkEClipboardAllowCheckIn.Name = "checkEClipboardAllowCheckIn";
			this.checkEClipboardAllowCheckIn.Size = new System.Drawing.Size(458, 17);
			this.checkEClipboardAllowCheckIn.TabIndex = 1;
			this.checkEClipboardAllowCheckIn.Text = "Allow self check-in";
			this.checkEClipboardAllowCheckIn.UseVisualStyleBackColor = true;
			this.checkEClipboardAllowCheckIn.Click += new System.EventHandler(this.CheckEClipboardAllowCheckIn_Click);
			// 
			// checkEClipboardPopupKiosk
			// 
			this.checkEClipboardPopupKiosk.Location = new System.Drawing.Point(19, 110);
			this.checkEClipboardPopupKiosk.Name = "checkEClipboardPopupKiosk";
			this.checkEClipboardPopupKiosk.Size = new System.Drawing.Size(458, 17);
			this.checkEClipboardPopupKiosk.TabIndex = 0;
			this.checkEClipboardPopupKiosk.Text = "Show kiosk manager when staff changes patient status to arrived";
			this.checkEClipboardPopupKiosk.UseVisualStyleBackColor = true;
			this.checkEClipboardPopupKiosk.Click += new System.EventHandler(this.CheckEClipboardPopupKiosk_Click);
			// 
			// checkEClipboardUseDefaults
			// 
			this.checkEClipboardUseDefaults.Location = new System.Drawing.Point(15, 28);
			this.checkEClipboardUseDefaults.Name = "checkEClipboardUseDefaults";
			this.checkEClipboardUseDefaults.Size = new System.Drawing.Size(298, 17);
			this.checkEClipboardUseDefaults.TabIndex = 269;
			this.checkEClipboardUseDefaults.Text = "Use Defaults for this clinic";
			this.checkEClipboardUseDefaults.UseVisualStyleBackColor = true;
			this.checkEClipboardUseDefaults.Click += new System.EventHandler(this.CheckEClipboardUseDefaults_Click);
			// 
			// tabMisc
			// 
			this.tabMisc.BackColor = System.Drawing.SystemColors.Control;
			this.tabMisc.Controls.Add(this.groupDateFormat);
			this.tabMisc.Controls.Add(this.groupNotUsed);
			this.tabMisc.Controls.Add(this.groupBox8);
			this.tabMisc.Location = new System.Drawing.Point(4, 22);
			this.tabMisc.Name = "tabMisc";
			this.tabMisc.Padding = new System.Windows.Forms.Padding(3);
			this.tabMisc.Size = new System.Drawing.Size(1154, 595);
			this.tabMisc.TabIndex = 7;
			this.tabMisc.Text = "Miscellaneous";
			// 
			// groupDateFormat
			// 
			this.groupDateFormat.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupDateFormat.Controls.Add(this.label30);
			this.groupDateFormat.Controls.Add(this.labelDateCustom);
			this.groupDateFormat.Controls.Add(this.textDateCustom);
			this.groupDateFormat.Controls.Add(this.label34);
			this.groupDateFormat.Controls.Add(this.radioDateCustom);
			this.groupDateFormat.Controls.Add(this.radioDateMMMMdyyyy);
			this.groupDateFormat.Controls.Add(this.radioDatem);
			this.groupDateFormat.Controls.Add(this.radioDateLongDate);
			this.groupDateFormat.Controls.Add(this.radioDateShortDate);
			this.groupDateFormat.Location = new System.Drawing.Point(152, 98);
			this.groupDateFormat.Name = "groupDateFormat";
			this.groupDateFormat.Size = new System.Drawing.Size(851, 160);
			this.groupDateFormat.TabIndex = 251;
			this.groupDateFormat.TabStop = false;
			this.groupDateFormat.Text = "Date Format";
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(6, 16);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(838, 13);
			this.label30.TabIndex = 315;
			this.label30.Text = "This date format will be applied to eReminders, eConfirmations, manual confirmati" +
    "ons, ASAP List texts, and other forms of patient communication.";
			// 
			// labelDateCustom
			// 
			this.labelDateCustom.Location = new System.Drawing.Point(217, 114);
			this.labelDateCustom.Name = "labelDateCustom";
			this.labelDateCustom.Size = new System.Drawing.Size(225, 20);
			this.labelDateCustom.TabIndex = 314;
			this.labelDateCustom.Text = "labelDateCustom";
			this.labelDateCustom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textDateCustom
			// 
			this.textDateCustom.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.textDateCustom.Location = new System.Drawing.Point(86, 114);
			this.textDateCustom.Multiline = true;
			this.textDateCustom.Name = "textDateCustom";
			this.textDateCustom.Size = new System.Drawing.Size(123, 20);
			this.textDateCustom.TabIndex = 313;
			this.textDateCustom.TextChanged += new System.EventHandler(this.textDateCustom_TextChanged);
			// 
			// label34
			// 
			this.label34.Location = new System.Drawing.Point(89, 138);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(755, 20);
			this.label34.TabIndex = 74;
			this.label34.Text = "E.g. of custom date format is MMMM d, yyyy";
			// 
			// radioDateCustom
			// 
			this.radioDateCustom.Location = new System.Drawing.Point(15, 114);
			this.radioDateCustom.Name = "radioDateCustom";
			this.radioDateCustom.Size = new System.Drawing.Size(70, 20);
			this.radioDateCustom.TabIndex = 82;
			this.radioDateCustom.Text = "Custom:";
			this.radioDateCustom.UseVisualStyleBackColor = true;
			// 
			// radioDateMMMMdyyyy
			// 
			this.radioDateMMMMdyyyy.Location = new System.Drawing.Point(15, 74);
			this.radioDateMMMMdyyyy.Name = "radioDateMMMMdyyyy";
			this.radioDateMMMMdyyyy.Size = new System.Drawing.Size(438, 20);
			this.radioDateMMMMdyyyy.TabIndex = 81;
			this.radioDateMMMMdyyyy.Text = "March 15, 2018";
			this.radioDateMMMMdyyyy.UseVisualStyleBackColor = true;
			// 
			// radioDatem
			// 
			this.radioDatem.Location = new System.Drawing.Point(15, 94);
			this.radioDatem.Name = "radioDatem";
			this.radioDatem.Size = new System.Drawing.Size(438, 20);
			this.radioDatem.TabIndex = 80;
			this.radioDatem.Text = "March 15";
			this.radioDatem.UseVisualStyleBackColor = true;
			// 
			// radioDateLongDate
			// 
			this.radioDateLongDate.Location = new System.Drawing.Point(15, 54);
			this.radioDateLongDate.Name = "radioDateLongDate";
			this.radioDateLongDate.Size = new System.Drawing.Size(438, 20);
			this.radioDateLongDate.TabIndex = 79;
			this.radioDateLongDate.Text = "Thursday March 15, 2018";
			this.radioDateLongDate.UseVisualStyleBackColor = true;
			this.radioDateLongDate.Click += new System.EventHandler(this.radioDateFormat_Click);
			// 
			// radioDateShortDate
			// 
			this.radioDateShortDate.Location = new System.Drawing.Point(15, 34);
			this.radioDateShortDate.Name = "radioDateShortDate";
			this.radioDateShortDate.Size = new System.Drawing.Size(438, 20);
			this.radioDateShortDate.TabIndex = 78;
			this.radioDateShortDate.Text = "3/15/2018";
			this.radioDateShortDate.UseVisualStyleBackColor = true;
			this.radioDateShortDate.Click += new System.EventHandler(this.radioDateFormat_Click);
			// 
			// groupNotUsed
			// 
			this.groupNotUsed.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupNotUsed.Controls.Add(this.butShowOldMobileSych);
			this.groupNotUsed.Location = new System.Drawing.Point(152, 520);
			this.groupNotUsed.Name = "groupNotUsed";
			this.groupNotUsed.Size = new System.Drawing.Size(851, 58);
			this.groupNotUsed.TabIndex = 250;
			this.groupNotUsed.TabStop = false;
			this.groupNotUsed.Text = "No Longer Used";
			this.groupNotUsed.Visible = false;
			// 
			// butShowOldMobileSych
			// 
			this.butShowOldMobileSych.Location = new System.Drawing.Point(9, 21);
			this.butShowOldMobileSych.Name = "butShowOldMobileSych";
			this.butShowOldMobileSych.Size = new System.Drawing.Size(225, 24);
			this.butShowOldMobileSych.TabIndex = 249;
			this.butShowOldMobileSych.Text = "Show Mobile Synch (old-style)";
			this.butShowOldMobileSych.Visible = false;
			this.butShowOldMobileSych.Click += new System.EventHandler(this.butShowOldMobileSych_Click);
			// 
			// groupBox8
			// 
			this.groupBox8.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox8.Controls.Add(this.dateRunEnd);
			this.groupBox8.Controls.Add(this.label46);
			this.groupBox8.Controls.Add(this.dateRunStart);
			this.groupBox8.Controls.Add(this.label47);
			this.groupBox8.Controls.Add(this.label48);
			this.groupBox8.Location = new System.Drawing.Point(152, 7);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(851, 81);
			this.groupBox8.TabIndex = 76;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Automated eServices Schedule";
			// 
			// dateRunEnd
			// 
			this.dateRunEnd.CustomFormat = " ";
			this.dateRunEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.dateRunEnd.Location = new System.Drawing.Point(144, 56);
			this.dateRunEnd.Name = "dateRunEnd";
			this.dateRunEnd.ShowUpDown = true;
			this.dateRunEnd.Size = new System.Drawing.Size(90, 20);
			this.dateRunEnd.TabIndex = 7;
			this.dateRunEnd.Value = new System.DateTime(2015, 11, 3, 22, 0, 0, 0);
			// 
			// label46
			// 
			this.label46.Location = new System.Drawing.Point(6, 16);
			this.label46.Name = "label46";
			this.label46.Size = new System.Drawing.Size(838, 13);
			this.label46.TabIndex = 72;
			this.label46.Text = "This applies to eConfirmations, eReminders, and WebSched recall notifications. It" +
    " dictates the time interval that the service will automatically notify patients." +
    "";
			// 
			// dateRunStart
			// 
			this.dateRunStart.CustomFormat = " ";
			this.dateRunStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.dateRunStart.Location = new System.Drawing.Point(144, 36);
			this.dateRunStart.Name = "dateRunStart";
			this.dateRunStart.ShowUpDown = true;
			this.dateRunStart.Size = new System.Drawing.Size(90, 20);
			this.dateRunStart.TabIndex = 6;
			this.dateRunStart.Value = new System.DateTime(2015, 11, 3, 7, 0, 0, 0);
			// 
			// label47
			// 
			this.label47.Location = new System.Drawing.Point(12, 58);
			this.label47.Name = "label47";
			this.label47.Size = new System.Drawing.Size(126, 17);
			this.label47.TabIndex = 5;
			this.label47.Text = "End Time";
			this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label48
			// 
			this.label48.Location = new System.Drawing.Point(12, 38);
			this.label48.Name = "label48";
			this.label48.Size = new System.Drawing.Size(126, 17);
			this.label48.TabIndex = 4;
			this.label48.Text = "Start Time";
			this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(1097, 660);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 23);
			this.butClose.TabIndex = 500;
			this.butClose.Text = "OK";
			this.butClose.UseVisualStyleBackColor = true;
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormEServicesSetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1184, 692);
			this.Controls.Add(this.label23);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.tabControl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(990, 731);
			this.Name = "FormEServicesSetup";
			this.Text = "eServices Setup";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEServicesSetup_FormClosing);
			this.Load += new System.EventHandler(this.FormEServicesSetup_Load);
			this.menuWebSchedVerifyTextTemplate.ResumeLayout(false);
			this.tabControl.ResumeLayout(false);
			this.tabSignup.ResumeLayout(false);
			this.tabEConnector.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tabMobileSynch.ResumeLayout(false);
			this.groupPreferences.ResumeLayout(false);
			this.groupPreferences.PerformLayout();
			this.tabMobileWeb.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabPatientPortal.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupPatientPortalInvites.ResumeLayout(false);
			this.groupPatientPortalInvites.PerformLayout();
			this.groupBoxNotification.ResumeLayout(false);
			this.groupBoxNotification.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabWebSched.ResumeLayout(false);
			this.tabControlWebSched.ResumeLayout(false);
			this.tabWebSchedRecalls.ResumeLayout(false);
			this.tabWebSchedRecalls.PerformLayout();
			this.groupWebSchedProvRule.ResumeLayout(false);
			this.groupWebSchedText.ResumeLayout(false);
			this.groupWebSchedText.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBoxWebSchedAutomation.ResumeLayout(false);
			this.groupWebSchedPreview.ResumeLayout(false);
			this.groupWebSchedPreview.PerformLayout();
			this.tabWebSchedNewPatAppts.ResumeLayout(false);
			this.tabWebSchedNewPatAppts.PerformLayout();
			this.groupBoxWSNPHostedURLs.ResumeLayout(false);
			this.groupBox13.ResumeLayout(false);
			this.groupBox13.PerformLayout();
			this.groupBox11.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			this.tabWebSchedVerify.ResumeLayout(false);
			this.groupBoxASAP.ResumeLayout(false);
			this.groupBoxASAPTextTemplate.ResumeLayout(false);
			this.groupBoxASAPTextTemplate.PerformLayout();
			this.groupBoxASAPEmail.ResumeLayout(false);
			this.groupBoxASAPEmail.PerformLayout();
			this.groupBoxRadioASAP.ResumeLayout(false);
			this.groupBoxNewPat.ResumeLayout(false);
			this.groupBoxNewPatTextTemplate.ResumeLayout(false);
			this.groupBoxNewPatTextTemplate.PerformLayout();
			this.groupBoxNewPatEmail.ResumeLayout(false);
			this.groupBoxNewPatEmail.PerformLayout();
			this.groupBoxRadioNewPat.ResumeLayout(false);
			this.groupBoxRecall.ResumeLayout(false);
			this.groupBoxRecallTextTemplate.ResumeLayout(false);
			this.groupBoxRecallTextTemplate.PerformLayout();
			this.groupBoxRecallEmail.ResumeLayout(false);
			this.groupBoxRecallEmail.PerformLayout();
			this.groupBoxRadioRecall.ResumeLayout(false);
			this.tabTexting.ResumeLayout(false);
			this.groupShortCode.ResumeLayout(false);
			this.groupShortCode.PerformLayout();
			this.tabECR.ResumeLayout(false);
			this.tabECR.PerformLayout();
			this.groupAutomationStatuses.ResumeLayout(false);
			this.tabEClipboard.ResumeLayout(false);
			this.groupEClipboardSheets.ResumeLayout(false);
			this.groupEClipboardRules.ResumeLayout(false);
			this.groupEClipboardRules.PerformLayout();
			this.tabMisc.ResumeLayout(false);
			this.groupDateFormat.ResumeLayout(false);
			this.groupDateFormat.PerformLayout();
			this.groupNotUsed.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TextBox textBoxNotificationSubject;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBoxNotification;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textPatientFacingUrlPortal;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPatientPortal;
		private UI.Button butClose;
		private System.Windows.Forms.TabPage tabMobileSynch;
		private System.Windows.Forms.CheckBox checkTroubleshooting;
		private UI.Button butDelete;
		private System.Windows.Forms.Label textDateTimeLastRun;
		private System.Windows.Forms.GroupBox groupPreferences;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox textMobileUserName;
		private System.Windows.Forms.Label label15;
		private UI.Button butCurrentWorkstation;
		private System.Windows.Forms.TextBox textMobilePassword;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.TextBox textMobileSynchWorkStation;
		private ValidNumber textSynchMinutes;
		private System.Windows.Forms.Label label18;
		private UI.Button butSaveMobileSynch;
		private ValidDate textDateBefore;
		private System.Windows.Forms.Label labelMobileSynchURL;
		private System.Windows.Forms.TextBox textMobileSyncServerURL;
		private System.Windows.Forms.Label labelMinutesBetweenSynch;
		private System.Windows.Forms.Label label19;
		private UI.Button butFullSync;
		private UI.Button butSync;
		private System.Windows.Forms.TabPage tabWebSched;
		private System.Windows.Forms.Label labelWebSchedDesc;
		private UI.Button butRecallSchedSetup;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.TabPage tabEConnector;
		private System.Windows.Forms.GroupBox groupBox3;
		private UI.Button butStartListenerService;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label labelListenerStatus;
		private UI.Button butListenerAlertsOff;
		private System.Windows.Forms.TextBox textListenerServiceStatus;
		private System.Windows.Forms.Label label23;
		private UI.ODGrid gridListenerServiceStatusHistory;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private UI.Button butListenerServiceHistoryRefresh;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label labelListenerServiceAck;
		private UI.Button butListenerServiceAck;
		private System.Windows.Forms.TabPage tabTexting;
		private UI.ODGrid gridSmsSummary;
		private System.Windows.Forms.DateTimePicker dateTimePickerSms;
		private UI.Button butBackMonth;
		private UI.Button butFwdMonth;
		private UI.Button butThisMonth;
		private UI.ODGrid gridWebSchedRecallTypes;
		private System.Windows.Forms.Label label35;
		private UI.ODGrid gridWebSchedTimeSlots;
		private System.Windows.Forms.GroupBox groupWebSchedPreview;
		private System.Windows.Forms.Label labelWebSchedClinic;
		private System.Windows.Forms.Label labelWebSchedRecallTypes;
		private System.Windows.Forms.ComboBox comboWebSchedClinic;
		private System.Windows.Forms.ComboBox comboWebSchedRecallTypes;
		private ValidDate textWebSchedDateStart;
		private UI.Button butWebSchedToday;
		private UI.ODGrid gridWebSchedOperatories;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.ListBox listBoxWebSchedProviderPref;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.ComboBox comboWebSchedProviders;
		private UI.Button butWebSchedPickClinic;
		private UI.Button butWebSchedPickProv;
		private System.Windows.Forms.Label label37;
		private UI.Button butInstallEConnector;
		private UI.Button butDefaultClinic;
		private UI.Button butDefaultClinicClear;
		private System.Windows.Forms.TabControl tabControlWebSched;
		private System.Windows.Forms.TabPage tabWebSchedRecalls;
		private System.Windows.Forms.TabPage tabWebSchedNewPatAppts;
		private System.Windows.Forms.Label label41;
		private ValidNumber textWebSchedNewPatApptSearchDays;
		private System.Windows.Forms.Label label40;
		private UI.ODGrid gridWebSchedNewPatApptOps;
		private System.Windows.Forms.Label label42;
		private System.Windows.Forms.GroupBox groupBox7;
		private UI.Button butWebSchedNewPatApptsToday;
		private UI.ODGrid gridWebSchedNewPatApptTimeSlots;
		private ValidDate textWebSchedNewPatApptsDateStart;
		private UI.ODGrid gridWSNPAReasons;
		private System.Windows.Forms.Label labelWSNPTimeSlots;
		private System.Windows.Forms.LinkLabel linkLabelAboutWebSched;
		private System.Windows.Forms.GroupBox groupBoxWebSchedAutomation;
		private System.Windows.Forms.RadioButton radioDoNotSend;
		private System.Windows.Forms.RadioButton radioSendToEmailOnlyPreferred;
		private System.Windows.Forms.RadioButton radioSendToEmailNoPreferred;
		private System.Windows.Forms.RadioButton radioSendToEmail;
		private System.Windows.Forms.TabPage tabMisc;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.Label label46;
		private System.Windows.Forms.DateTimePicker dateRunEnd;
		private System.Windows.Forms.DateTimePicker dateRunStart;
		private System.Windows.Forms.Label label47;
		private System.Windows.Forms.Label label48;
		private System.Windows.Forms.TabPage tabECR;
		private System.Windows.Forms.TextBox textStatusReminders;
		private UI.Button butActivateReminder;
		private System.Windows.Forms.TextBox textStatusConfirmations;
		private UI.Button butActivateConfirm;
		private System.Windows.Forms.GroupBox groupAutomationStatuses;
		private UI.ComboBoxPlus comboStatusEFailed;
		private System.Windows.Forms.Label label50;
		private System.Windows.Forms.CheckBox checkEnableNoClinic;
		private UI.ComboBoxPlus comboStatusEDeclined;
		private UI.ComboBoxPlus comboStatusESent;
		private UI.ComboBoxPlus comboStatusEAccepted;
		private System.Windows.Forms.Label label51;
		private System.Windows.Forms.Label label52;
		private System.Windows.Forms.Label label53;
		private System.Windows.Forms.CheckBox checkIsConfirmEnabled;
		private System.Windows.Forms.ComboBox comboClinicEConfirm;
		private System.Windows.Forms.Label label54;
		private UI.Button butAddConfirmation;
		private UI.Button butAddReminder;
		private UI.ODGrid gridRemindersMain;
		private System.Windows.Forms.CheckBox checkUseDefaultsEC;
		private System.Windows.Forms.TabPage tabSignup;
		private System.Windows.Forms.WebBrowser webBrowserSignup;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.ListBox listboxWebSchedRecallIgnoreBlockoutTypes;
		private System.Windows.Forms.Label label32;
		private UI.Button butWebSchedRecallBlockouts;
		private System.Windows.Forms.GroupBox groupBox11;
		private System.Windows.Forms.ListBox listboxWebSchedNewPatIgnoreBlockoutTypes;
		private System.Windows.Forms.Label label33;
		private UI.Button butWebSchedNewPatBlockouts;
		private System.Windows.Forms.TabPage tabMobileWeb;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox textHostedUrlMobileWeb;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupNotUsed;
		private UI.Button butShowOldMobileSych;
		private System.Windows.Forms.GroupBox groupBox5;
		private UI.Button butSetupMobileWebUsers;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.GroupBox groupWebSchedText;
		private System.Windows.Forms.RadioButton radioDoNotSendText;
		private System.Windows.Forms.RadioButton radioSendText;
		private System.Windows.Forms.Label labelWebSchedPerBatch;
		private ValidNumber textWebSchedPerBatch;
		private UI.ODGrid gridConfStatuses;
		private System.Windows.Forms.Label label11;
		private UI.ComboBoxPlus comboWSNPConfirmStatuses;
		private System.Windows.Forms.Label labelWebSchedNewPatConfirmStatus;
		private System.Windows.Forms.Label labelInstallWarning;
		private System.Windows.Forms.CheckBox checkRecallAllowProvSelection;
		private System.Windows.Forms.TabPage tabWebSchedVerify;
		private System.Windows.Forms.CheckBox checkUseDefaultsVerify;
		private System.Windows.Forms.GroupBox groupBoxASAP;
		private System.Windows.Forms.GroupBox groupBoxASAPTextTemplate;
		private System.Windows.Forms.TextBox textASAPTextTemplate;
		private System.Windows.Forms.GroupBox groupBoxASAPEmail;
		private System.Windows.Forms.TextBox textASAPEmailSubj;
		private System.Windows.Forms.GroupBox groupBoxRadioASAP;
		private System.Windows.Forms.RadioButton radioASAPTextAndEmail;
		private System.Windows.Forms.RadioButton radioASAPEmail;
		private System.Windows.Forms.RadioButton radioASAPText;
		private System.Windows.Forms.RadioButton radioASAPNone;
		private System.Windows.Forms.GroupBox groupBoxNewPat;
		private System.Windows.Forms.GroupBox groupBoxNewPatTextTemplate;
		private System.Windows.Forms.TextBox textNewPatTextTemplate;
		private System.Windows.Forms.GroupBox groupBoxNewPatEmail;
		private System.Windows.Forms.TextBox textNewPatEmailSubj;
		private System.Windows.Forms.GroupBox groupBoxRadioNewPat;
		private System.Windows.Forms.RadioButton radioNewPatTextAndEmail;
		private System.Windows.Forms.RadioButton radioNewPatText;
		private System.Windows.Forms.RadioButton radioNewPatNone;
		private System.Windows.Forms.GroupBox groupBoxRecall;
		private System.Windows.Forms.GroupBox groupBoxRecallTextTemplate;
		private System.Windows.Forms.TextBox textRecallTextTemplate;
		private System.Windows.Forms.GroupBox groupBoxRecallEmail;
		private System.Windows.Forms.TextBox textRecallEmailSubj;
		private System.Windows.Forms.GroupBox groupBoxRadioRecall;
		private System.Windows.Forms.RadioButton radioRecallTextAndEmail;
		private System.Windows.Forms.RadioButton radioRecallEmail;
		private System.Windows.Forms.RadioButton radioRecallText;
		private System.Windows.Forms.RadioButton radioRecallNone;
		private UI.ComboBoxClinicPicker comboClinicVerify;
		private System.Windows.Forms.RadioButton radioNewPatEmail;
		private System.Windows.Forms.ContextMenuStrip menuWebSchedVerifyTextTemplate;
		private System.Windows.Forms.ToolStripMenuItem insertReplacementsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
		private System.Windows.Forms.Label label28;
		private UI.Button butRestoreWebSchedVerify;
		private System.Windows.Forms.CheckBox checkWebSchedNewPatForcePhoneFormatting;
		private System.Windows.Forms.GroupBox groupPatientPortalInvites;
		private System.Windows.Forms.CheckBox checkUseDefaultsPPInvites;
		private UI.Button butAddPPInviteRule;
		private UI.ODGrid gridPatPortalInviteRules;
		private System.Windows.Forms.CheckBox checkIsPPInvitesEnabled;
		private UI.ComboBoxClinicPicker comboClinicsPPInvites;
		private System.Windows.Forms.TextBox textStatusInvites;
		private UI.Button butActivateInvites;
		private System.Windows.Forms.CheckBox checkNewPatAllowProvSelection;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.GroupBox groupDateFormat;
		private System.Windows.Forms.RadioButton radioDateCustom;
		private System.Windows.Forms.RadioButton radioDateMMMMdyyyy;
		private System.Windows.Forms.RadioButton radioDatem;
		private System.Windows.Forms.RadioButton radioDateLongDate;
		private System.Windows.Forms.RadioButton radioDateShortDate;
		private System.Windows.Forms.TextBox textDateCustom;
		private System.Windows.Forms.Label labelDateCustom;
		private System.Windows.Forms.Label label30;
		private UI.ComboBoxPlus comboWSRConfirmStatus;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label labelWSNPAApptType;
		private UI.ComboBoxPlus comboWSNPADefApptType;
		private System.Windows.Forms.RadioButton radio2ClickConfirm;
		private System.Windows.Forms.RadioButton radio1ClickConfirm;
		private System.Windows.Forms.GroupBox groupBoxWSNPHostedURLs;
		private System.Windows.Forms.FlowLayoutPanel panelHostedURLs;
		private System.Windows.Forms.Label labelWSNPClinic;
		private System.Windows.Forms.ComboBox comboWSNPClinics;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.GroupBox groupBox13;
		private System.Windows.Forms.TextBox textWebSchedNewPatApptMessage;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.TextBox textHostedUrlPortal;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private UI.Button butCopyToClipboard;
		private UI.Button butNavigateTo;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.RadioButton radioPatientPortalPayment;
		private System.Windows.Forms.RadioButton radioPatientPortalLogin;
		private UI.ComboBoxClinicPicker comboPPClinicUrl;
		private System.Windows.Forms.CheckBox checkWSRDoubleBooking;
		private System.Windows.Forms.CheckBox checkWSNPDoubleBooking;
		private System.Windows.Forms.CheckBox checkEmailsWithDiffProcess;
		private System.Windows.Forms.TabPage tabEClipboard;
		private System.Windows.Forms.GroupBox groupEClipboardRules;
		private System.Windows.Forms.CheckBox checkEClipboardPopupKiosk;
		private System.Windows.Forms.CheckBox checkEClipboardAllowSheets;
		private System.Windows.Forms.CheckBox checkEClipboardAllowCheckIn;
		private System.Windows.Forms.TextBox textEClipboardMessage;
		private System.Windows.Forms.Label labelEClipboardMessage;
		private System.Windows.Forms.CheckBox checkEClipboardCreateMissingForms;
		private System.Windows.Forms.GroupBox groupEClipboardSheets;
		private System.Windows.Forms.ListBox listEClipboardSheetsAvailable;
		private System.Windows.Forms.Label labelOps;
		private UI.ODGrid gridEClipboardSheetsInUse;
		private UI.Button butEClipboardRight;
		private UI.Button butEClipboardLeft;
		private System.Windows.Forms.CheckBox checkEClipboardAllowSelfPortrait;
		private UI.Button butAsapEditEmail;
		private System.Windows.Forms.WebBrowser browserAsapEmailBody;
		private UI.Button butNewPatEditEmail;
		private System.Windows.Forms.WebBrowser browserNewPatEmailBody;
		private UI.Button butRecallEditEmail;
		private System.Windows.Forms.WebBrowser browserRecallEmailBody;
		private UI.Button butEditWebMailNotificationBody;
		private System.Windows.Forms.WebBrowser browserWebMailNotificatonBody;
		private UI.Button butEClipboardUp;
		private UI.Button butEClipboardDown;
		private System.Windows.Forms.CheckBox checkEClipboardUseDefaults;
		private UI.ODGrid gridMobileAppDevices;
		private System.Windows.Forms.Label label10;
		private UI.ComboBoxClinicPicker clinicPickerEClipboard;
		private System.Windows.Forms.Label labelEClipboardNotSignedUp;
		private UI.Button butEClipboardAddSheets;
		private System.Windows.Forms.Label label43;
		private System.Windows.Forms.Label label44;
		private ValidNumber textWebSchedRecallApptSearchDays;
		private System.Windows.Forms.GroupBox groupWebSchedProvRule;
		private System.Windows.Forms.CheckBox checkUseDefaultProvRule;
		private UI.ComboBoxClinicPicker comboClinicProvRule;
		private UI.Button butProvRulePickClinic;
		private UI.Button butAddThankYouVerify;
		private System.Windows.Forms.GroupBox groupShortCode;
		private System.Windows.Forms.CheckBox checkOptInPrompt;
		private UI.ComboBoxClinicPicker comboShortCodeClinic;
		private System.Windows.Forms.Label labelShortCodeOptInClinicTitle;
		private System.Windows.Forms.TextBox textShortCodeOptInClinicTitle;
		private UI.Button butSaveShortCodes;
		private System.Windows.Forms.Label labelUnsavedShortCodeChanges;
	}
}