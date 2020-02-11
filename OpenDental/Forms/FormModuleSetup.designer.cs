using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	partial class FormModuleSetup {
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

		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormModuleSetup));
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.checkIsAlertRadiologyProcsEnabled = new System.Windows.Forms.CheckBox();
			this.checkImagesModuleTreeIsCollapsed = new System.Windows.Forms.CheckBox();
			this.checkApptsCheckFrequency = new System.Windows.Forms.CheckBox();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.tabAppts = new System.Windows.Forms.TabPage();
			this.checkBrokenApptRequiredOnMove = new System.Windows.Forms.CheckBox();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.checkAppointmentBubblesDisabled = new System.Windows.Forms.CheckBox();
			this.textApptProvbarWidth = new OpenDental.ValidNum();
			this.checkSolidBlockouts = new System.Windows.Forms.CheckBox();
			this.label58 = new System.Windows.Forms.Label();
			this.checkApptExclamation = new System.Windows.Forms.CheckBox();
			this.textApptFontSize = new System.Windows.Forms.TextBox();
			this.butApptLineColor = new System.Windows.Forms.Button();
			this.checkApptBubbleDelay = new System.Windows.Forms.CheckBox();
			this.butColor = new System.Windows.Forms.Button();
			this.label23 = new System.Windows.Forms.Label();
			this.comboDelay = new System.Windows.Forms.ComboBox();
			this.label25 = new System.Windows.Forms.Label();
			this.checkApptModuleDefaultToWeek = new System.Windows.Forms.CheckBox();
			this.checkApptRefreshEveryMinute = new System.Windows.Forms.CheckBox();
			this.apptClickDelay = new System.Windows.Forms.Label();
			this.label54 = new System.Windows.Forms.Label();
			this.checkApptsAllowOverlap = new System.Windows.Forms.CheckBox();
			this.checkPreventChangesToComplAppts = new System.Windows.Forms.CheckBox();
			this.textApptAutoRefreshRange = new OpenDental.ValidNumber();
			this.labelApptAutoRefreshRange = new System.Windows.Forms.Label();
			this.checkUnscheduledListNoRecalls = new System.Windows.Forms.CheckBox();
			this.checkReplaceBlockouts = new System.Windows.Forms.CheckBox();
			this.labelApptSchedEnforceSpecialty = new System.Windows.Forms.Label();
			this.comboApptSchedEnforceSpecialty = new System.Windows.Forms.ComboBox();
			this.textApptWithoutProcsDefaultLength = new OpenDental.ValidNumber();
			this.labelApptWithoutProcsDefaultLength = new System.Windows.Forms.Label();
			this.checkApptAllowEmptyComplete = new System.Windows.Forms.CheckBox();
			this.checkApptAllowFutureComplete = new System.Windows.Forms.CheckBox();
			this.comboTimeArrived = new OpenDental.UI.ComboBoxPlus();
			this.checkApptsRequireProcs = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.checkApptModuleProductionUsesOps = new System.Windows.Forms.CheckBox();
			this.comboTimeSeated = new OpenDental.UI.ComboBoxPlus();
			this.checkUseOpHygProv = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.checkApptModuleAdjInProd = new System.Windows.Forms.CheckBox();
			this.comboTimeDismissed = new OpenDental.UI.ComboBoxPlus();
			this.checkApptTimeReset = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label37 = new System.Windows.Forms.Label();
			this.comboBrokenApptProc = new System.Windows.Forms.ComboBox();
			this.checkBrokenApptCommLog = new System.Windows.Forms.CheckBox();
			this.checkBrokenApptAdjustment = new System.Windows.Forms.CheckBox();
			this.comboBrokenApptAdjType = new OpenDental.UI.ComboBoxPlus();
			this.label7 = new System.Windows.Forms.Label();
			this.textWaitRoomWarn = new System.Windows.Forms.TextBox();
			this.checkAppointmentTimeIsLocked = new System.Windows.Forms.CheckBox();
			this.label22 = new System.Windows.Forms.Label();
			this.comboSearchBehavior = new System.Windows.Forms.ComboBox();
			this.textApptBubNoteLength = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.checkWaitingRoomFilterByView = new System.Windows.Forms.CheckBox();
			this.tabFamily = new System.Windows.Forms.TabPage();
			this.checkPatientDOBMasked = new System.Windows.Forms.CheckBox();
			this.checkPatientSSNMasked = new System.Windows.Forms.CheckBox();
			this.groupBoxClaimSnapshot = new System.Windows.Forms.GroupBox();
			this.comboClaimSnapshotTrigger = new System.Windows.Forms.ComboBox();
			this.textClaimSnapshotRunTime = new System.Windows.Forms.TextBox();
			this.label30 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.groupBoxSuperFamily = new System.Windows.Forms.GroupBox();
			this.comboSuperFamSort = new System.Windows.Forms.ComboBox();
			this.labelSuperFamSort = new System.Windows.Forms.Label();
			this.checkSuperFamSync = new System.Windows.Forms.CheckBox();
			this.checkSuperFamAddIns = new System.Windows.Forms.CheckBox();
			this.checkSuperFamCloneCreate = new System.Windows.Forms.CheckBox();
			this.label15 = new System.Windows.Forms.Label();
			this.checkInsPlanExclusionsUseUCR = new System.Windows.Forms.CheckBox();
			this.checkInsPlanExclusionsMarkDoNotBill = new System.Windows.Forms.CheckBox();
			this.checkFixedBenefitBlankLikeZero = new System.Windows.Forms.CheckBox();
			this.checkAllowPatsAtHQ = new System.Windows.Forms.CheckBox();
			this.checkAutoFillPatEmail = new System.Windows.Forms.CheckBox();
			this.checkPreferredReferrals = new System.Windows.Forms.CheckBox();
			this.checkTextMsgOkStatusTreatAsNo = new System.Windows.Forms.CheckBox();
			this.checkPatInitBillingTypeFromPriInsPlan = new System.Windows.Forms.CheckBox();
			this.checkFamPhiAccess = new System.Windows.Forms.CheckBox();
			this.checkClaimTrackingRequireError = new System.Windows.Forms.CheckBox();
			this.checkPPOpercentage = new System.Windows.Forms.CheckBox();
			this.checkInsurancePlansShared = new System.Windows.Forms.CheckBox();
			this.checkClaimUseOverrideProcDescript = new System.Windows.Forms.CheckBox();
			this.checkInsDefaultAssignmentOfBenefits = new System.Windows.Forms.CheckBox();
			this.checkSelectProv = new System.Windows.Forms.CheckBox();
			this.comboCobRule = new System.Windows.Forms.ComboBox();
			this.checkAllowedFeeSchedsAutomate = new System.Windows.Forms.CheckBox();
			this.checkGoogleAddress = new System.Windows.Forms.CheckBox();
			this.checkInsPPOsecWriteoffs = new System.Windows.Forms.CheckBox();
			this.checkInsDefaultShowUCRonClaims = new System.Windows.Forms.CheckBox();
			this.checkCoPayFeeScheduleBlankLikeZero = new System.Windows.Forms.CheckBox();
			this.tabAccount = new System.Windows.Forms.TabPage();
			this.groupBox10 = new System.Windows.Forms.GroupBox();
			this.comboFinanceChargeAdjType = new OpenDental.UI.ComboBoxPlus();
			this.comboBillingChargeAdjType = new OpenDental.UI.ComboBoxPlus();
			this.comboSalesTaxAdjType = new OpenDental.UI.ComboBoxPlus();
			this.textTaxPercent = new System.Windows.Forms.TextBox();
			this.label41 = new System.Windows.Forms.Label();
			this.comboPayPlanAdj = new OpenDental.UI.ComboBoxPlus();
			this.label42 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.comboRigorousAdjustments = new System.Windows.Forms.ComboBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.listboxBadDebtAdjs = new System.Windows.Forms.ListBox();
			this.label29 = new System.Windows.Forms.Label();
			this.butBadDebt = new OpenDental.UI.Button();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.checkPaymentsTransferPatientIncomeOnly = new System.Windows.Forms.CheckBox();
			this.checkStoreCCTokens = new System.Windows.Forms.CheckBox();
			this.comboPaymentClinicSetting = new System.Windows.Forms.ComboBox();
			this.label38 = new System.Windows.Forms.Label();
			this.checkPaymentsPromptForPayType = new System.Windows.Forms.CheckBox();
			this.checkHidePaysplits = new System.Windows.Forms.CheckBox();
			this.checkAllowPrepayProvider = new System.Windows.Forms.CheckBox();
			this.label40 = new System.Windows.Forms.Label();
			this.comboUnallocatedSplits = new OpenDental.UI.ComboBoxPlus();
			this.label28 = new System.Windows.Forms.Label();
			this.checkAllowFutureDebits = new System.Windows.Forms.CheckBox();
			this.checkAllowEmailCCReceipt = new System.Windows.Forms.CheckBox();
			this.label39 = new System.Windows.Forms.Label();
			this.comboRigorousAccounting = new System.Windows.Forms.ComboBox();
			this.comboAutoSplitPref = new System.Windows.Forms.ComboBox();
			this.checkAgingProcLifo = new System.Windows.Forms.CheckBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.checkAllowPrePayToTpProcs = new System.Windows.Forms.CheckBox();
			this.checkIsRefundable = new System.Windows.Forms.CheckBox();
			this.label57 = new System.Windows.Forms.Label();
			this.comboTpUnearnedType = new OpenDental.UI.ComboBoxPlus();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.checkCanadianPpoLabEst = new System.Windows.Forms.CheckBox();
			this.checkInsEstRecalcReceived = new System.Windows.Forms.CheckBox();
			this.checkPromptForSecondaryClaim = new System.Windows.Forms.CheckBox();
			this.checkInsPayNoWriteoffMoreThanProc = new System.Windows.Forms.CheckBox();
			this.checkClaimTrackingExcludeNone = new System.Windows.Forms.CheckBox();
			this.label55 = new System.Windows.Forms.Label();
			this.comboZeroDollarProcClaimBehavior = new System.Windows.Forms.ComboBox();
			this.labelClaimCredit = new System.Windows.Forms.Label();
			this.comboClaimCredit = new System.Windows.Forms.ComboBox();
			this.checkAllowFuturePayments = new System.Windows.Forms.CheckBox();
			this.groupBoxClaimIdPrefix = new System.Windows.Forms.GroupBox();
			this.butReplacements = new OpenDental.UI.Button();
			this.textClaimIdentifier = new System.Windows.Forms.TextBox();
			this.checkAllowProcAdjFromClaim = new System.Windows.Forms.CheckBox();
			this.checkProviderIncomeShows = new System.Windows.Forms.CheckBox();
			this.checkClaimFormTreatDentSaysSigOnFile = new System.Windows.Forms.CheckBox();
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical = new System.Windows.Forms.CheckBox();
			this.checkEclaimsMedicalProvTreatmentAsOrdering = new System.Windows.Forms.CheckBox();
			this.checkEclaimsSeparateTreatProv = new System.Windows.Forms.CheckBox();
			this.label20 = new System.Windows.Forms.Label();
			this.textClaimAttachPath = new System.Windows.Forms.TextBox();
			this.checkClaimsValidateACN = new System.Windows.Forms.CheckBox();
			this.textInsWriteoffDescript = new System.Windows.Forms.TextBox();
			this.label17 = new System.Windows.Forms.Label();
			this.groupRepeatingCharges = new System.Windows.Forms.GroupBox();
			this.labelRepeatingChargesAutomatedTime = new System.Windows.Forms.Label();
			this.textRepeatingChargesAutomatedTime = new OpenDental.ValidTime();
			this.checkRepeatingChargesRunAging = new System.Windows.Forms.CheckBox();
			this.checkRepeatingChargesAutomated = new System.Windows.Forms.CheckBox();
			this.groupRecurringCharges = new System.Windows.Forms.GroupBox();
			this.checkRecurPatBal0 = new System.Windows.Forms.CheckBox();
			this.label56 = new System.Windows.Forms.Label();
			this.comboRecurringChargePayType = new OpenDental.UI.ComboBoxPlus();
			this.labelRecurringChargesAutomatedTime = new System.Windows.Forms.Label();
			this.textRecurringChargesTime = new OpenDental.ValidTime();
			this.checkRecurringChargesAutomated = new System.Windows.Forms.CheckBox();
			this.checkRecurringChargesUseTransDate = new System.Windows.Forms.CheckBox();
			this.checkRecurChargPriProv = new System.Windows.Forms.CheckBox();
			this.checkBalancesDontSubtractIns = new System.Windows.Forms.CheckBox();
			this.checkAllowFutureTrans = new System.Windows.Forms.CheckBox();
			this.checkPpoUseUcr = new System.Windows.Forms.CheckBox();
			this.groupCommLogs = new System.Windows.Forms.GroupBox();
			this.checkCommLogAutoSave = new System.Windows.Forms.CheckBox();
			this.checkShowFamilyCommByDefault = new System.Windows.Forms.CheckBox();
			this.checkAccountShowPaymentNums = new System.Windows.Forms.CheckBox();
			this.checkShowAllocateUnearnedPaymentPrompt = new System.Windows.Forms.CheckBox();
			this.checkAgingMonthly = new System.Windows.Forms.CheckBox();
			this.checkStatementInvoiceGridShowWriteoffs = new System.Windows.Forms.CheckBox();
			this.groupPayPlans = new System.Windows.Forms.GroupBox();
			this.label59 = new System.Windows.Forms.Label();
			this.textDynamicPayPlan = new OpenDental.ValidTime();
			this.label27 = new System.Windows.Forms.Label();
			this.comboPayPlansVersion = new System.Windows.Forms.ComboBox();
			this.checkHideDueNow = new System.Windows.Forms.CheckBox();
			this.checkPayPlansUseSheets = new System.Windows.Forms.CheckBox();
			this.checkPayPlansExcludePastActivity = new System.Windows.Forms.CheckBox();
			this.tabTreatPlan = new System.Windows.Forms.TabPage();
			this.checkPromptSaveTP = new System.Windows.Forms.CheckBox();
			this.labelDiscountPercentage = new System.Windows.Forms.Label();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.textInsImplant = new System.Windows.Forms.TextBox();
			this.label53 = new System.Windows.Forms.Label();
			this.label52 = new System.Windows.Forms.Label();
			this.textInsDentures = new System.Windows.Forms.TextBox();
			this.label51 = new System.Windows.Forms.Label();
			this.textInsPerioMaint = new System.Windows.Forms.TextBox();
			this.label50 = new System.Windows.Forms.Label();
			this.textInsDebridement = new System.Windows.Forms.TextBox();
			this.label49 = new System.Windows.Forms.Label();
			this.textInsSealant = new System.Windows.Forms.TextBox();
			this.label48 = new System.Windows.Forms.Label();
			this.textInsFlouride = new System.Windows.Forms.TextBox();
			this.label47 = new System.Windows.Forms.Label();
			this.textInsCrown = new System.Windows.Forms.TextBox();
			this.label46 = new System.Windows.Forms.Label();
			this.textInsSRP = new System.Windows.Forms.TextBox();
			this.label45 = new System.Windows.Forms.Label();
			this.textInsCancerScreen = new System.Windows.Forms.TextBox();
			this.label44 = new System.Windows.Forms.Label();
			this.textInsProphy = new System.Windows.Forms.TextBox();
			this.label43 = new System.Windows.Forms.Label();
			this.textInsExam = new System.Windows.Forms.TextBox();
			this.label35 = new System.Windows.Forms.Label();
			this.textInsBW = new System.Windows.Forms.TextBox();
			this.label34 = new System.Windows.Forms.Label();
			this.textInsPano = new System.Windows.Forms.TextBox();
			this.label36 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.groupInsHist = new System.Windows.Forms.GroupBox();
			this.textInsHistProphy = new System.Windows.Forms.TextBox();
			this.labelInsHistProphy = new System.Windows.Forms.Label();
			this.textInsHistPerioLR = new System.Windows.Forms.TextBox();
			this.labelInsHistPerioLR = new System.Windows.Forms.Label();
			this.textInsHistPerioLL = new System.Windows.Forms.TextBox();
			this.labelInsHistPerioLL = new System.Windows.Forms.Label();
			this.textInsHistPerioUL = new System.Windows.Forms.TextBox();
			this.labelInsHistPerioUL = new System.Windows.Forms.Label();
			this.textInsHistPerioUR = new System.Windows.Forms.TextBox();
			this.labelInsHistPerioUR = new System.Windows.Forms.Label();
			this.textInsHistFMX = new System.Windows.Forms.TextBox();
			this.labelInsHistFMX = new System.Windows.Forms.Label();
			this.textInsHistPerioMaint = new System.Windows.Forms.TextBox();
			this.labelInsHistPerioMaint = new System.Windows.Forms.Label();
			this.textInsHistExam = new System.Windows.Forms.TextBox();
			this.labelInsHistDebridement = new System.Windows.Forms.Label();
			this.textInsHistBW = new System.Windows.Forms.TextBox();
			this.labelInsHistExam = new System.Windows.Forms.Label();
			this.textInsHistDebridement = new System.Windows.Forms.TextBox();
			this.labelInsHistBW = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.checkFrequency = new System.Windows.Forms.CheckBox();
			this.groupTreatPlanSort = new System.Windows.Forms.GroupBox();
			this.radioTreatPlanSortTooth = new System.Windows.Forms.RadioButton();
			this.radioTreatPlanSortOrder = new System.Windows.Forms.RadioButton();
			this.textTreatNote = new OpenDental.ODtextBox();
			this.checkTPSaveSigned = new System.Windows.Forms.CheckBox();
			this.comboProcDiscountType = new OpenDental.UI.ComboBoxPlus();
			this.checkTreatPlanShowCompleted = new System.Windows.Forms.CheckBox();
			this.textDiscountPercentage = new System.Windows.Forms.TextBox();
			this.checkTreatPlanItemized = new System.Windows.Forms.CheckBox();
			this.tabChart = new System.Windows.Forms.TabPage();
			this.checkShowPlannedApptPrompt = new System.Windows.Forms.CheckBox();
			this.checkAllowSettingProcsComplete = new System.Windows.Forms.CheckBox();
			this.comboToothNomenclature = new System.Windows.Forms.ComboBox();
			this.textProblemsIndicateNone = new System.Windows.Forms.TextBox();
			this.label32 = new System.Windows.Forms.Label();
			this.checkBoxRxClinicUseSelected = new System.Windows.Forms.CheckBox();
			this.checkProcNoteConcurrencyMerge = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.comboProcCodeListSort = new System.Windows.Forms.ComboBox();
			this.checkProcProvChangesCp = new System.Windows.Forms.CheckBox();
			this.labelToothNomenclature = new System.Windows.Forms.Label();
			this.comboProcFeeUpdatePrompt = new System.Windows.Forms.ComboBox();
			this.checkPerioTreatImplantsAsNotMissing = new System.Windows.Forms.CheckBox();
			this.labelProcFeeUpdatePrompt = new System.Windows.Forms.Label();
			this.checkAutoClearEntryStatus = new System.Windows.Forms.CheckBox();
			this.butProblemsIndicateNone = new OpenDental.UI.Button();
			this.checkPerioSkipMissingTeeth = new System.Windows.Forms.CheckBox();
			this.label9 = new System.Windows.Forms.Label();
			this.checkProcGroupNoteDoesAggregate = new System.Windows.Forms.CheckBox();
			this.textMedicationsIndicateNone = new System.Windows.Forms.TextBox();
			this.checkProvColorChart = new System.Windows.Forms.CheckBox();
			this.checkSignatureAllowDigital = new System.Windows.Forms.CheckBox();
			this.textAllergiesIndicateNone = new System.Windows.Forms.TextBox();
			this.butMedicationsIndicateNone = new OpenDental.UI.Button();
			this.textMedDefaultStopDays = new System.Windows.Forms.TextBox();
			this.checkClaimProcsAllowEstimatesOnCompl = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.checkProcEditRequireAutoCode = new System.Windows.Forms.CheckBox();
			this.checkChartNonPatientWarn = new System.Windows.Forms.CheckBox();
			this.label14 = new System.Windows.Forms.Label();
			this.checkProcLockingIsAllowed = new System.Windows.Forms.CheckBox();
			this.checkProcsPromptForAutoNote = new System.Windows.Forms.CheckBox();
			this.textICD9DefaultForNewProcs = new System.Windows.Forms.TextBox();
			this.labelIcdCodeDefault = new System.Windows.Forms.Label();
			this.checkScreeningsUseSheets = new System.Windows.Forms.CheckBox();
			this.butDiagnosisCode = new OpenDental.UI.Button();
			this.butAllergiesIndicateNone = new OpenDental.UI.Button();
			this.checkDxIcdVersion = new System.Windows.Forms.CheckBox();
			this.checkMedicalFeeUsedForNewProcs = new System.Windows.Forms.CheckBox();
			this.tabImages = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.radioImagesModuleTreeIsPersistentPerUser = new System.Windows.Forms.RadioButton();
			this.radioImagesModuleTreeIsCollapsed = new System.Windows.Forms.RadioButton();
			this.radioImagesModuleTreeIsExpanded = new System.Windows.Forms.RadioButton();
			this.tabManage = new System.Windows.Forms.TabPage();
			this.checkEraAllowTotalPayment = new System.Windows.Forms.CheckBox();
			this.checkIncludeEraWOPercCoPay = new System.Windows.Forms.CheckBox();
			this.checkClockEventAllowBreak = new System.Windows.Forms.CheckBox();
			this.textClaimsReceivedDays = new OpenDental.ValidNumber();
			this.checkShowAutoDeposit = new System.Windows.Forms.CheckBox();
			this.checkEraOneClaimPerPage = new System.Windows.Forms.CheckBox();
			this.checkClaimPaymentBatchOnly = new System.Windows.Forms.CheckBox();
			this.labelClaimsReceivedDays = new System.Windows.Forms.Label();
			this.checkScheduleProvEmpSelectAll = new System.Windows.Forms.CheckBox();
			this.checkClaimsSendWindowValidateOnLoad = new System.Windows.Forms.CheckBox();
			this.checkTimeCardADP = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkStatementsAlphabetically = new System.Windows.Forms.CheckBox();
			this.checkBillingShowProgress = new System.Windows.Forms.CheckBox();
			this.label24 = new System.Windows.Forms.Label();
			this.textBillingElectBatchMax = new OpenDental.ValidNum();
			this.checkStatementShowAdjNotes = new System.Windows.Forms.CheckBox();
			this.checkIntermingleDefault = new System.Windows.Forms.CheckBox();
			this.checkStatementShowReturnAddress = new System.Windows.Forms.CheckBox();
			this.checkStatementShowProcBreakdown = new System.Windows.Forms.CheckBox();
			this.checkStatementShowNotes = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.comboUseChartNum = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.textStatementsCalcDueDate = new OpenDental.ValidNumber();
			this.textPayPlansBillInAdvanceDays = new OpenDental.ValidNum();
			this.comboTimeCardOvertimeFirstDayOfWeek = new System.Windows.Forms.ComboBox();
			this.label16 = new System.Windows.Forms.Label();
			this.checkRxSendNewToQueue = new System.Windows.Forms.CheckBox();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.tabControlMain.SuspendLayout();
			this.tabAppts.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabFamily.SuspendLayout();
			this.groupBoxClaimSnapshot.SuspendLayout();
			this.groupBoxSuperFamily.SuspendLayout();
			this.tabAccount.SuspendLayout();
			this.groupBox10.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox9.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBoxClaimIdPrefix.SuspendLayout();
			this.groupRepeatingCharges.SuspendLayout();
			this.groupRecurringCharges.SuspendLayout();
			this.groupCommLogs.SuspendLayout();
			this.groupPayPlans.SuspendLayout();
			this.tabTreatPlan.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupInsHist.SuspendLayout();
			this.groupTreatPlanSort.SuspendLayout();
			this.tabChart.SuspendLayout();
			this.tabImages.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabManage.SuspendLayout();
			this.groupBox1.SuspendLayout();
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
			// checkIsAlertRadiologyProcsEnabled
			// 
			this.checkIsAlertRadiologyProcsEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsAlertRadiologyProcsEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsAlertRadiologyProcsEnabled.Location = new System.Drawing.Point(141, 432);
			this.checkIsAlertRadiologyProcsEnabled.Name = "checkIsAlertRadiologyProcsEnabled";
			this.checkIsAlertRadiologyProcsEnabled.Size = new System.Drawing.Size(364, 17);
			this.checkIsAlertRadiologyProcsEnabled.TabIndex = 229;
			this.checkIsAlertRadiologyProcsEnabled.Text = "OpenDentalService alerts for scheduled non-CPOE radiology procedures";
			this.checkIsAlertRadiologyProcsEnabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsAlertRadiologyProcsEnabled.UseVisualStyleBackColor = true;
			// 
			// checkImagesModuleTreeIsCollapsed
			// 
			this.checkImagesModuleTreeIsCollapsed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkImagesModuleTreeIsCollapsed.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkImagesModuleTreeIsCollapsed.Location = new System.Drawing.Point(81, 7);
			this.checkImagesModuleTreeIsCollapsed.Name = "checkImagesModuleTreeIsCollapsed";
			this.checkImagesModuleTreeIsCollapsed.Size = new System.Drawing.Size(359, 17);
			this.checkImagesModuleTreeIsCollapsed.TabIndex = 47;
			this.checkImagesModuleTreeIsCollapsed.Text = "Document tree collapses when patient changes";
			this.checkImagesModuleTreeIsCollapsed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkApptsCheckFrequency
			// 
			this.checkApptsCheckFrequency.Location = new System.Drawing.Point(0, 0);
			this.checkApptsCheckFrequency.Name = "checkApptsCheckFrequency";
			this.checkApptsCheckFrequency.Size = new System.Drawing.Size(104, 24);
			this.checkApptsCheckFrequency.TabIndex = 0;
			// 
			// tabControlMain
			// 
			this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlMain.Controls.Add(this.tabAppts);
			this.tabControlMain.Controls.Add(this.tabFamily);
			this.tabControlMain.Controls.Add(this.tabAccount);
			this.tabControlMain.Controls.Add(this.tabTreatPlan);
			this.tabControlMain.Controls.Add(this.tabChart);
			this.tabControlMain.Controls.Add(this.tabImages);
			this.tabControlMain.Controls.Add(this.tabManage);
			this.tabControlMain.Location = new System.Drawing.Point(-3, 1);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(1176, 667);
			this.tabControlMain.TabIndex = 196;
			// 
			// tabAppts
			// 
			this.tabAppts.BackColor = System.Drawing.SystemColors.Window;
			this.tabAppts.Controls.Add(this.checkBrokenApptRequiredOnMove);
			this.tabAppts.Controls.Add(this.groupBox8);
			this.tabAppts.Controls.Add(this.checkApptsAllowOverlap);
			this.tabAppts.Controls.Add(this.checkPreventChangesToComplAppts);
			this.tabAppts.Controls.Add(this.textApptAutoRefreshRange);
			this.tabAppts.Controls.Add(this.labelApptAutoRefreshRange);
			this.tabAppts.Controls.Add(this.checkUnscheduledListNoRecalls);
			this.tabAppts.Controls.Add(this.checkReplaceBlockouts);
			this.tabAppts.Controls.Add(this.labelApptSchedEnforceSpecialty);
			this.tabAppts.Controls.Add(this.comboApptSchedEnforceSpecialty);
			this.tabAppts.Controls.Add(this.textApptWithoutProcsDefaultLength);
			this.tabAppts.Controls.Add(this.labelApptWithoutProcsDefaultLength);
			this.tabAppts.Controls.Add(this.checkApptAllowEmptyComplete);
			this.tabAppts.Controls.Add(this.checkApptAllowFutureComplete);
			this.tabAppts.Controls.Add(this.comboTimeArrived);
			this.tabAppts.Controls.Add(this.checkApptsRequireProcs);
			this.tabAppts.Controls.Add(this.label3);
			this.tabAppts.Controls.Add(this.checkApptModuleProductionUsesOps);
			this.tabAppts.Controls.Add(this.comboTimeSeated);
			this.tabAppts.Controls.Add(this.checkUseOpHygProv);
			this.tabAppts.Controls.Add(this.label5);
			this.tabAppts.Controls.Add(this.checkApptModuleAdjInProd);
			this.tabAppts.Controls.Add(this.comboTimeDismissed);
			this.tabAppts.Controls.Add(this.checkApptTimeReset);
			this.tabAppts.Controls.Add(this.label6);
			this.tabAppts.Controls.Add(this.groupBox2);
			this.tabAppts.Controls.Add(this.textWaitRoomWarn);
			this.tabAppts.Controls.Add(this.checkAppointmentTimeIsLocked);
			this.tabAppts.Controls.Add(this.label22);
			this.tabAppts.Controls.Add(this.comboSearchBehavior);
			this.tabAppts.Controls.Add(this.textApptBubNoteLength);
			this.tabAppts.Controls.Add(this.label13);
			this.tabAppts.Controls.Add(this.label21);
			this.tabAppts.Controls.Add(this.checkWaitingRoomFilterByView);
			this.tabAppts.Location = new System.Drawing.Point(4, 22);
			this.tabAppts.Name = "tabAppts";
			this.tabAppts.Padding = new System.Windows.Forms.Padding(3);
			this.tabAppts.Size = new System.Drawing.Size(1168, 641);
			this.tabAppts.TabIndex = 0;
			this.tabAppts.Text = "Appts";
			// 
			// checkBrokenApptRequiredOnMove
			// 
			this.checkBrokenApptRequiredOnMove.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBrokenApptRequiredOnMove.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBrokenApptRequiredOnMove.Location = new System.Drawing.Point(63, 572);
			this.checkBrokenApptRequiredOnMove.Name = "checkBrokenApptRequiredOnMove";
			this.checkBrokenApptRequiredOnMove.Size = new System.Drawing.Size(397, 17);
			this.checkBrokenApptRequiredOnMove.TabIndex = 290;
			this.checkBrokenApptRequiredOnMove.Text = "Force users to break scheduled appointments before rescheduling";
			this.checkBrokenApptRequiredOnMove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.checkAppointmentBubblesDisabled);
			this.groupBox8.Controls.Add(this.textApptProvbarWidth);
			this.groupBox8.Controls.Add(this.checkSolidBlockouts);
			this.groupBox8.Controls.Add(this.label58);
			this.groupBox8.Controls.Add(this.checkApptExclamation);
			this.groupBox8.Controls.Add(this.textApptFontSize);
			this.groupBox8.Controls.Add(this.butApptLineColor);
			this.groupBox8.Controls.Add(this.checkApptBubbleDelay);
			this.groupBox8.Controls.Add(this.butColor);
			this.groupBox8.Controls.Add(this.label23);
			this.groupBox8.Controls.Add(this.comboDelay);
			this.groupBox8.Controls.Add(this.label25);
			this.groupBox8.Controls.Add(this.checkApptModuleDefaultToWeek);
			this.groupBox8.Controls.Add(this.checkApptRefreshEveryMinute);
			this.groupBox8.Controls.Add(this.apptClickDelay);
			this.groupBox8.Controls.Add(this.label54);
			this.groupBox8.Location = new System.Drawing.Point(584, 17);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(449, 285);
			this.groupBox8.TabIndex = 289;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Appearance";
			// 
			// checkAppointmentBubblesDisabled
			// 
			this.checkAppointmentBubblesDisabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAppointmentBubblesDisabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAppointmentBubblesDisabled.Location = new System.Drawing.Point(14, 19);
			this.checkAppointmentBubblesDisabled.Name = "checkAppointmentBubblesDisabled";
			this.checkAppointmentBubblesDisabled.Size = new System.Drawing.Size(425, 17);
			this.checkAppointmentBubblesDisabled.TabIndex = 234;
			this.checkAppointmentBubblesDisabled.Text = "Default appointment bubble to \'disabled\' for new appointment views";
			this.checkAppointmentBubblesDisabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAppointmentBubblesDisabled.UseVisualStyleBackColor = true;
			// 
			// textApptProvbarWidth
			// 
			this.textApptProvbarWidth.Location = new System.Drawing.Point(389, 258);
			this.textApptProvbarWidth.MaxVal = 20;
			this.textApptProvbarWidth.MinVal = 0;
			this.textApptProvbarWidth.Name = "textApptProvbarWidth";
			this.textApptProvbarWidth.Size = new System.Drawing.Size(50, 20);
			this.textApptProvbarWidth.TabIndex = 288;
			// 
			// checkSolidBlockouts
			// 
			this.checkSolidBlockouts.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSolidBlockouts.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSolidBlockouts.Location = new System.Drawing.Point(31, 65);
			this.checkSolidBlockouts.Name = "checkSolidBlockouts";
			this.checkSolidBlockouts.Size = new System.Drawing.Size(408, 17);
			this.checkSolidBlockouts.TabIndex = 220;
			this.checkSolidBlockouts.Text = "Use solid blockouts instead of outlines on the Appointments Module";
			this.checkSolidBlockouts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSolidBlockouts.UseVisualStyleBackColor = true;
			// 
			// label58
			// 
			this.label58.Location = new System.Drawing.Point(72, 261);
			this.label58.Name = "label58";
			this.label58.Size = new System.Drawing.Size(315, 16);
			this.label58.TabIndex = 286;
			this.label58.Text = "Width of provider time bar on left of each appointment";
			this.label58.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkApptExclamation
			// 
			this.checkApptExclamation.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptExclamation.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptExclamation.Location = new System.Drawing.Point(14, 88);
			this.checkApptExclamation.Name = "checkApptExclamation";
			this.checkApptExclamation.Size = new System.Drawing.Size(425, 17);
			this.checkApptExclamation.TabIndex = 222;
			this.checkApptExclamation.Text = "Show ! on appts for ins not sent, if added to Appt View (might cause slowdown)";
			this.checkApptExclamation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptExclamation.UseVisualStyleBackColor = true;
			// 
			// textApptFontSize
			// 
			this.textApptFontSize.Location = new System.Drawing.Point(389, 235);
			this.textApptFontSize.Name = "textApptFontSize";
			this.textApptFontSize.Size = new System.Drawing.Size(50, 20);
			this.textApptFontSize.TabIndex = 285;
			// 
			// butApptLineColor
			// 
			this.butApptLineColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butApptLineColor.Location = new System.Drawing.Point(415, 136);
			this.butApptLineColor.Name = "butApptLineColor";
			this.butApptLineColor.Size = new System.Drawing.Size(24, 21);
			this.butApptLineColor.TabIndex = 226;
			this.butApptLineColor.Click += new System.EventHandler(this.butApptLineColor_Click);
			// 
			// checkApptBubbleDelay
			// 
			this.checkApptBubbleDelay.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptBubbleDelay.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptBubbleDelay.Location = new System.Drawing.Point(14, 42);
			this.checkApptBubbleDelay.Name = "checkApptBubbleDelay";
			this.checkApptBubbleDelay.Size = new System.Drawing.Size(425, 17);
			this.checkApptBubbleDelay.TabIndex = 221;
			this.checkApptBubbleDelay.Text = "Appointment bubble popup delay";
			this.checkApptBubbleDelay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptBubbleDelay.UseVisualStyleBackColor = true;
			// 
			// butColor
			// 
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColor.Location = new System.Drawing.Point(415, 111);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(24, 21);
			this.butColor.TabIndex = 225;
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(165, 115);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(246, 16);
			this.label23.TabIndex = 223;
			this.label23.Text = "Waiting room alert color";
			this.label23.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboDelay
			// 
			this.comboDelay.AllowDrop = true;
			this.comboDelay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDelay.FormattingEnabled = true;
			this.comboDelay.Location = new System.Drawing.Point(325, 186);
			this.comboDelay.Name = "comboDelay";
			this.comboDelay.Size = new System.Drawing.Size(114, 21);
			this.comboDelay.TabIndex = 232;
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(165, 141);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(246, 16);
			this.label25.TabIndex = 224;
			this.label25.Text = "Appointment time line color";
			this.label25.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkApptModuleDefaultToWeek
			// 
			this.checkApptModuleDefaultToWeek.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptModuleDefaultToWeek.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptModuleDefaultToWeek.Location = new System.Drawing.Point(33, 163);
			this.checkApptModuleDefaultToWeek.Name = "checkApptModuleDefaultToWeek";
			this.checkApptModuleDefaultToWeek.Size = new System.Drawing.Size(406, 17);
			this.checkApptModuleDefaultToWeek.TabIndex = 220;
			this.checkApptModuleDefaultToWeek.Text = "Appointments Module defaults to week view";
			this.checkApptModuleDefaultToWeek.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkApptRefreshEveryMinute
			// 
			this.checkApptRefreshEveryMinute.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptRefreshEveryMinute.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptRefreshEveryMinute.Location = new System.Drawing.Point(33, 213);
			this.checkApptRefreshEveryMinute.Name = "checkApptRefreshEveryMinute";
			this.checkApptRefreshEveryMinute.Size = new System.Drawing.Size(406, 17);
			this.checkApptRefreshEveryMinute.TabIndex = 235;
			this.checkApptRefreshEveryMinute.Text = "Refresh every 60 seconds, keeps waiting room times refreshed";
			this.checkApptRefreshEveryMinute.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// apptClickDelay
			// 
			this.apptClickDelay.Location = new System.Drawing.Point(166, 188);
			this.apptClickDelay.Name = "apptClickDelay";
			this.apptClickDelay.Size = new System.Drawing.Size(157, 18);
			this.apptClickDelay.TabIndex = 233;
			this.apptClickDelay.Text = "Appointment click delay";
			this.apptClickDelay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label54
			// 
			this.label54.Location = new System.Drawing.Point(100, 238);
			this.label54.Name = "label54";
			this.label54.Size = new System.Drawing.Size(287, 16);
			this.label54.TabIndex = 251;
			this.label54.Text = "Appointment font size. Default is 8. Decimals allowed";
			this.label54.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkApptsAllowOverlap
			// 
			this.checkApptsAllowOverlap.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptsAllowOverlap.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptsAllowOverlap.Location = new System.Drawing.Point(181, 555);
			this.checkApptsAllowOverlap.Name = "checkApptsAllowOverlap";
			this.checkApptsAllowOverlap.Size = new System.Drawing.Size(279, 17);
			this.checkApptsAllowOverlap.TabIndex = 284;
			this.checkApptsAllowOverlap.Text = "Appointments allow overlap";
			this.checkApptsAllowOverlap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptsAllowOverlap.ThreeState = true;
			// 
			// checkPreventChangesToComplAppts
			// 
			this.checkPreventChangesToComplAppts.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPreventChangesToComplAppts.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPreventChangesToComplAppts.Location = new System.Drawing.Point(34, 538);
			this.checkPreventChangesToComplAppts.Name = "checkPreventChangesToComplAppts";
			this.checkPreventChangesToComplAppts.Size = new System.Drawing.Size(426, 17);
			this.checkPreventChangesToComplAppts.TabIndex = 283;
			this.checkPreventChangesToComplAppts.Text = "Prevent changes to completed appointments with completed procedures";
			this.checkPreventChangesToComplAppts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPreventChangesToComplAppts.UseVisualStyleBackColor = true;
			// 
			// textApptAutoRefreshRange
			// 
			this.textApptAutoRefreshRange.Location = new System.Drawing.Point(391, 516);
			this.textApptAutoRefreshRange.MaxVal = 600;
			this.textApptAutoRefreshRange.MinVal = -1;
			this.textApptAutoRefreshRange.Name = "textApptAutoRefreshRange";
			this.textApptAutoRefreshRange.Size = new System.Drawing.Size(70, 20);
			this.textApptAutoRefreshRange.TabIndex = 282;
			// 
			// labelApptAutoRefreshRange
			// 
			this.labelApptAutoRefreshRange.Location = new System.Drawing.Point(26, 519);
			this.labelApptAutoRefreshRange.Name = "labelApptAutoRefreshRange";
			this.labelApptAutoRefreshRange.Size = new System.Drawing.Size(363, 16);
			this.labelApptAutoRefreshRange.TabIndex = 281;
			this.labelApptAutoRefreshRange.Text = "Number of days out to automatically refresh Appointments Module (-1 for all)";
			this.labelApptAutoRefreshRange.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkUnscheduledListNoRecalls
			// 
			this.checkUnscheduledListNoRecalls.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUnscheduledListNoRecalls.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUnscheduledListNoRecalls.Location = new System.Drawing.Point(55, 497);
			this.checkUnscheduledListNoRecalls.Name = "checkUnscheduledListNoRecalls";
			this.checkUnscheduledListNoRecalls.Size = new System.Drawing.Size(406, 17);
			this.checkUnscheduledListNoRecalls.TabIndex = 280;
			this.checkUnscheduledListNoRecalls.Text = "Do not allow recall appointments on the Unscheduled List";
			this.checkUnscheduledListNoRecalls.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkReplaceBlockouts
			// 
			this.checkReplaceBlockouts.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkReplaceBlockouts.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReplaceBlockouts.Location = new System.Drawing.Point(55, 479);
			this.checkReplaceBlockouts.Name = "checkReplaceBlockouts";
			this.checkReplaceBlockouts.Size = new System.Drawing.Size(406, 17);
			this.checkReplaceBlockouts.TabIndex = 279;
			this.checkReplaceBlockouts.Text = "Allow \'Block appointment scheduling\' blockouts to replace conflicting blockouts";
			this.checkReplaceBlockouts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelApptSchedEnforceSpecialty
			// 
			this.labelApptSchedEnforceSpecialty.Location = new System.Drawing.Point(48, 453);
			this.labelApptSchedEnforceSpecialty.Name = "labelApptSchedEnforceSpecialty";
			this.labelApptSchedEnforceSpecialty.Size = new System.Drawing.Size(247, 17);
			this.labelApptSchedEnforceSpecialty.TabIndex = 278;
			this.labelApptSchedEnforceSpecialty.Text = "Enforce clinic specialties";
			this.labelApptSchedEnforceSpecialty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboApptSchedEnforceSpecialty
			// 
			this.comboApptSchedEnforceSpecialty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboApptSchedEnforceSpecialty.FormattingEnabled = true;
			this.comboApptSchedEnforceSpecialty.Location = new System.Drawing.Point(297, 452);
			this.comboApptSchedEnforceSpecialty.Name = "comboApptSchedEnforceSpecialty";
			this.comboApptSchedEnforceSpecialty.Size = new System.Drawing.Size(163, 21);
			this.comboApptSchedEnforceSpecialty.TabIndex = 277;
			// 
			// textApptWithoutProcsDefaultLength
			// 
			this.textApptWithoutProcsDefaultLength.Location = new System.Drawing.Point(361, 390);
			this.textApptWithoutProcsDefaultLength.MaxVal = 600;
			this.textApptWithoutProcsDefaultLength.MinVal = 0;
			this.textApptWithoutProcsDefaultLength.Name = "textApptWithoutProcsDefaultLength";
			this.textApptWithoutProcsDefaultLength.Size = new System.Drawing.Size(100, 20);
			this.textApptWithoutProcsDefaultLength.TabIndex = 276;
			// 
			// labelApptWithoutProcsDefaultLength
			// 
			this.labelApptWithoutProcsDefaultLength.Location = new System.Drawing.Point(40, 393);
			this.labelApptWithoutProcsDefaultLength.Name = "labelApptWithoutProcsDefaultLength";
			this.labelApptWithoutProcsDefaultLength.Size = new System.Drawing.Size(319, 16);
			this.labelApptWithoutProcsDefaultLength.TabIndex = 275;
			this.labelApptWithoutProcsDefaultLength.Text = "Appointment without procedures default length";
			this.labelApptWithoutProcsDefaultLength.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkApptAllowEmptyComplete
			// 
			this.checkApptAllowEmptyComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptAllowEmptyComplete.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptAllowEmptyComplete.Location = new System.Drawing.Point(54, 432);
			this.checkApptAllowEmptyComplete.Name = "checkApptAllowEmptyComplete";
			this.checkApptAllowEmptyComplete.Size = new System.Drawing.Size(406, 17);
			this.checkApptAllowEmptyComplete.TabIndex = 274;
			this.checkApptAllowEmptyComplete.Text = "Allow setting appointments without procedures complete";
			this.checkApptAllowEmptyComplete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkApptAllowFutureComplete
			// 
			this.checkApptAllowFutureComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptAllowFutureComplete.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptAllowFutureComplete.Location = new System.Drawing.Point(54, 414);
			this.checkApptAllowFutureComplete.Name = "checkApptAllowFutureComplete";
			this.checkApptAllowFutureComplete.Size = new System.Drawing.Size(406, 17);
			this.checkApptAllowFutureComplete.TabIndex = 273;
			this.checkApptAllowFutureComplete.Text = "Allow setting future appointments complete";
			this.checkApptAllowFutureComplete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTimeArrived
			// 
			this.comboTimeArrived.Location = new System.Drawing.Point(297, 117);
			this.comboTimeArrived.Name = "comboTimeArrived";
			this.comboTimeArrived.Size = new System.Drawing.Size(163, 21);
			this.comboTimeArrived.TabIndex = 253;
			// 
			// checkApptsRequireProcs
			// 
			this.checkApptsRequireProcs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptsRequireProcs.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptsRequireProcs.Location = new System.Drawing.Point(54, 370);
			this.checkApptsRequireProcs.Name = "checkApptsRequireProcs";
			this.checkApptsRequireProcs.Size = new System.Drawing.Size(406, 17);
			this.checkApptsRequireProcs.TabIndex = 272;
			this.checkApptsRequireProcs.Text = "Appointments require procedures";
			this.checkApptsRequireProcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptsRequireProcs.CheckedChanged += new System.EventHandler(this.checkApptsRequireProcs_CheckedChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(48, 121);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(247, 15);
			this.label3.TabIndex = 254;
			this.label3.Text = "Time Arrived trigger";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkApptModuleProductionUsesOps
			// 
			this.checkApptModuleProductionUsesOps.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptModuleProductionUsesOps.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptModuleProductionUsesOps.Location = new System.Drawing.Point(54, 352);
			this.checkApptModuleProductionUsesOps.Name = "checkApptModuleProductionUsesOps";
			this.checkApptModuleProductionUsesOps.Size = new System.Drawing.Size(406, 17);
			this.checkApptModuleProductionUsesOps.TabIndex = 271;
			this.checkApptModuleProductionUsesOps.Text = "Appointments Module production uses operatories";
			this.checkApptModuleProductionUsesOps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTimeSeated
			// 
			this.comboTimeSeated.Location = new System.Drawing.Point(297, 139);
			this.comboTimeSeated.Name = "comboTimeSeated";
			this.comboTimeSeated.Size = new System.Drawing.Size(163, 21);
			this.comboTimeSeated.TabIndex = 255;
			// 
			// checkUseOpHygProv
			// 
			this.checkUseOpHygProv.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseOpHygProv.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUseOpHygProv.Location = new System.Drawing.Point(54, 334);
			this.checkUseOpHygProv.Name = "checkUseOpHygProv";
			this.checkUseOpHygProv.Size = new System.Drawing.Size(406, 17);
			this.checkUseOpHygProv.TabIndex = 270;
			this.checkUseOpHygProv.Text = "Force op\'s hygiene provider as secondary provider";
			this.checkUseOpHygProv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(48, 143);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(247, 15);
			this.label5.TabIndex = 256;
			this.label5.Text = "Time Seated (in op) trigger";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkApptModuleAdjInProd
			// 
			this.checkApptModuleAdjInProd.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptModuleAdjInProd.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptModuleAdjInProd.Location = new System.Drawing.Point(54, 316);
			this.checkApptModuleAdjInProd.Name = "checkApptModuleAdjInProd";
			this.checkApptModuleAdjInProd.Size = new System.Drawing.Size(406, 17);
			this.checkApptModuleAdjInProd.TabIndex = 269;
			this.checkApptModuleAdjInProd.Text = "Add daily adjustments to net production";
			this.checkApptModuleAdjInProd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTimeDismissed
			// 
			this.comboTimeDismissed.Location = new System.Drawing.Point(297, 161);
			this.comboTimeDismissed.Name = "comboTimeDismissed";
			this.comboTimeDismissed.Size = new System.Drawing.Size(163, 21);
			this.comboTimeDismissed.TabIndex = 257;
			// 
			// checkApptTimeReset
			// 
			this.checkApptTimeReset.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptTimeReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptTimeReset.Location = new System.Drawing.Point(54, 298);
			this.checkApptTimeReset.Name = "checkApptTimeReset";
			this.checkApptTimeReset.Size = new System.Drawing.Size(406, 17);
			this.checkApptTimeReset.TabIndex = 268;
			this.checkApptTimeReset.Text = "Reset calendar to today on Clinic select";
			this.checkApptTimeReset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(48, 165);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(247, 15);
			this.label6.TabIndex = 258;
			this.label6.Text = "Time Dismissed trigger";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label37);
			this.groupBox2.Controls.Add(this.comboBrokenApptProc);
			this.groupBox2.Controls.Add(this.checkBrokenApptCommLog);
			this.groupBox2.Controls.Add(this.checkBrokenApptAdjustment);
			this.groupBox2.Controls.Add(this.comboBrokenApptAdjType);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Location = new System.Drawing.Point(54, 17);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(418, 95);
			this.groupBox2.TabIndex = 267;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Broken Appointment Automation";
			// 
			// label37
			// 
			this.label37.Location = new System.Drawing.Point(2, 15);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(240, 15);
			this.label37.TabIndex = 235;
			this.label37.Text = "Broken appointment procedure type";
			this.label37.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboBrokenApptProc
			// 
			this.comboBrokenApptProc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBrokenApptProc.FormattingEnabled = true;
			this.comboBrokenApptProc.Location = new System.Drawing.Point(244, 11);
			this.comboBrokenApptProc.MaxDropDownItems = 30;
			this.comboBrokenApptProc.Name = "comboBrokenApptProc";
			this.comboBrokenApptProc.Size = new System.Drawing.Size(162, 21);
			this.comboBrokenApptProc.TabIndex = 234;
			// 
			// checkBrokenApptCommLog
			// 
			this.checkBrokenApptCommLog.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBrokenApptCommLog.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBrokenApptCommLog.Location = new System.Drawing.Point(21, 33);
			this.checkBrokenApptCommLog.Name = "checkBrokenApptCommLog";
			this.checkBrokenApptCommLog.Size = new System.Drawing.Size(385, 17);
			this.checkBrokenApptCommLog.TabIndex = 61;
			this.checkBrokenApptCommLog.Text = "Make broken appointment commlog";
			this.checkBrokenApptCommLog.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBrokenApptCommLog.UseVisualStyleBackColor = true;
			// 
			// checkBrokenApptAdjustment
			// 
			this.checkBrokenApptAdjustment.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBrokenApptAdjustment.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBrokenApptAdjustment.Location = new System.Drawing.Point(21, 49);
			this.checkBrokenApptAdjustment.Name = "checkBrokenApptAdjustment";
			this.checkBrokenApptAdjustment.Size = new System.Drawing.Size(385, 17);
			this.checkBrokenApptAdjustment.TabIndex = 217;
			this.checkBrokenApptAdjustment.Text = "Make broken appointment adjustment";
			this.checkBrokenApptAdjustment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBrokenApptAdjustment.UseVisualStyleBackColor = true;
			// 
			// comboBrokenApptAdjType
			// 
			this.comboBrokenApptAdjType.Location = new System.Drawing.Point(204, 67);
			this.comboBrokenApptAdjType.Name = "comboBrokenApptAdjType";
			this.comboBrokenApptAdjType.Size = new System.Drawing.Size(203, 21);
			this.comboBrokenApptAdjType.TabIndex = 70;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(6, 70);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(197, 15);
			this.label7.TabIndex = 71;
			this.label7.Text = "Broken appt default adj type";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textWaitRoomWarn
			// 
			this.textWaitRoomWarn.Location = new System.Drawing.Point(377, 272);
			this.textWaitRoomWarn.Name = "textWaitRoomWarn";
			this.textWaitRoomWarn.Size = new System.Drawing.Size(83, 20);
			this.textWaitRoomWarn.TabIndex = 266;
			// 
			// checkAppointmentTimeIsLocked
			// 
			this.checkAppointmentTimeIsLocked.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAppointmentTimeIsLocked.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAppointmentTimeIsLocked.Location = new System.Drawing.Point(246, 208);
			this.checkAppointmentTimeIsLocked.Name = "checkAppointmentTimeIsLocked";
			this.checkAppointmentTimeIsLocked.Size = new System.Drawing.Size(213, 17);
			this.checkAppointmentTimeIsLocked.TabIndex = 259;
			this.checkAppointmentTimeIsLocked.Text = "Appointment time locked by default";
			this.checkAppointmentTimeIsLocked.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAppointmentTimeIsLocked.MouseUp += new System.Windows.Forms.MouseEventHandler(this.checkAppointmentTimeIsLocked_MouseUp);
			// 
			// label22
			// 
			this.label22.BackColor = System.Drawing.Color.White;
			this.label22.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label22.Location = new System.Drawing.Point(128, 275);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(246, 16);
			this.label22.TabIndex = 265;
			this.label22.Text = "Waiting room alert time in minutes (0 to disable)";
			this.label22.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboSearchBehavior
			// 
			this.comboSearchBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSearchBehavior.FormattingEnabled = true;
			this.comboSearchBehavior.Location = new System.Drawing.Point(257, 183);
			this.comboSearchBehavior.MaxDropDownItems = 30;
			this.comboSearchBehavior.Name = "comboSearchBehavior";
			this.comboSearchBehavior.Size = new System.Drawing.Size(203, 21);
			this.comboSearchBehavior.TabIndex = 260;
			// 
			// textApptBubNoteLength
			// 
			this.textApptBubNoteLength.Location = new System.Drawing.Point(376, 227);
			this.textApptBubNoteLength.Name = "textApptBubNoteLength";
			this.textApptBubNoteLength.Size = new System.Drawing.Size(83, 20);
			this.textApptBubNoteLength.TabIndex = 264;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(39, 188);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(217, 15);
			this.label13.TabIndex = 261;
			this.label13.Text = "Search Behavior";
			this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(128, 230);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(246, 16);
			this.label21.TabIndex = 263;
			this.label21.Text = "Appointment bubble max note length (0 for no limit)";
			this.label21.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkWaitingRoomFilterByView
			// 
			this.checkWaitingRoomFilterByView.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWaitingRoomFilterByView.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkWaitingRoomFilterByView.Location = new System.Drawing.Point(53, 250);
			this.checkWaitingRoomFilterByView.Name = "checkWaitingRoomFilterByView";
			this.checkWaitingRoomFilterByView.Size = new System.Drawing.Size(406, 17);
			this.checkWaitingRoomFilterByView.TabIndex = 262;
			this.checkWaitingRoomFilterByView.Text = "Filter the waiting room based on the selected appointment view";
			this.checkWaitingRoomFilterByView.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabFamily
			// 
			this.tabFamily.BackColor = System.Drawing.SystemColors.Window;
			this.tabFamily.Controls.Add(this.checkPatientDOBMasked);
			this.tabFamily.Controls.Add(this.checkPatientSSNMasked);
			this.tabFamily.Controls.Add(this.groupBoxClaimSnapshot);
			this.tabFamily.Controls.Add(this.groupBoxSuperFamily);
			this.tabFamily.Controls.Add(this.label15);
			this.tabFamily.Controls.Add(this.checkInsPlanExclusionsUseUCR);
			this.tabFamily.Controls.Add(this.checkInsPlanExclusionsMarkDoNotBill);
			this.tabFamily.Controls.Add(this.checkFixedBenefitBlankLikeZero);
			this.tabFamily.Controls.Add(this.checkAllowPatsAtHQ);
			this.tabFamily.Controls.Add(this.checkAutoFillPatEmail);
			this.tabFamily.Controls.Add(this.checkPreferredReferrals);
			this.tabFamily.Controls.Add(this.checkTextMsgOkStatusTreatAsNo);
			this.tabFamily.Controls.Add(this.checkPatInitBillingTypeFromPriInsPlan);
			this.tabFamily.Controls.Add(this.checkFamPhiAccess);
			this.tabFamily.Controls.Add(this.checkClaimTrackingRequireError);
			this.tabFamily.Controls.Add(this.checkPPOpercentage);
			this.tabFamily.Controls.Add(this.checkInsurancePlansShared);
			this.tabFamily.Controls.Add(this.checkClaimUseOverrideProcDescript);
			this.tabFamily.Controls.Add(this.checkInsDefaultAssignmentOfBenefits);
			this.tabFamily.Controls.Add(this.checkSelectProv);
			this.tabFamily.Controls.Add(this.comboCobRule);
			this.tabFamily.Controls.Add(this.checkAllowedFeeSchedsAutomate);
			this.tabFamily.Controls.Add(this.checkGoogleAddress);
			this.tabFamily.Controls.Add(this.checkInsPPOsecWriteoffs);
			this.tabFamily.Controls.Add(this.checkInsDefaultShowUCRonClaims);
			this.tabFamily.Controls.Add(this.checkCoPayFeeScheduleBlankLikeZero);
			this.tabFamily.Location = new System.Drawing.Point(4, 22);
			this.tabFamily.Name = "tabFamily";
			this.tabFamily.Padding = new System.Windows.Forms.Padding(3);
			this.tabFamily.Size = new System.Drawing.Size(1168, 641);
			this.tabFamily.TabIndex = 1;
			this.tabFamily.Text = "Family";
			// 
			// checkPatientDOBMasked
			// 
			this.checkPatientDOBMasked.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPatientDOBMasked.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPatientDOBMasked.Location = new System.Drawing.Point(53, 565);
			this.checkPatientDOBMasked.Name = "checkPatientDOBMasked";
			this.checkPatientDOBMasked.Size = new System.Drawing.Size(422, 17);
			this.checkPatientDOBMasked.TabIndex = 281;
			this.checkPatientDOBMasked.Text = "Mask patient date of birth";
			this.checkPatientDOBMasked.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPatientSSNMasked
			// 
			this.checkPatientSSNMasked.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPatientSSNMasked.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPatientSSNMasked.Location = new System.Drawing.Point(53, 542);
			this.checkPatientSSNMasked.Name = "checkPatientSSNMasked";
			this.checkPatientSSNMasked.Size = new System.Drawing.Size(422, 17);
			this.checkPatientSSNMasked.TabIndex = 280;
			this.checkPatientSSNMasked.Text = "Mask patient Social Security Numbers";
			this.checkPatientSSNMasked.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBoxClaimSnapshot
			// 
			this.groupBoxClaimSnapshot.Controls.Add(this.comboClaimSnapshotTrigger);
			this.groupBoxClaimSnapshot.Controls.Add(this.textClaimSnapshotRunTime);
			this.groupBoxClaimSnapshot.Controls.Add(this.label30);
			this.groupBoxClaimSnapshot.Controls.Add(this.label31);
			this.groupBoxClaimSnapshot.Location = new System.Drawing.Point(693, 150);
			this.groupBoxClaimSnapshot.Name = "groupBoxClaimSnapshot";
			this.groupBoxClaimSnapshot.Size = new System.Drawing.Size(383, 71);
			this.groupBoxClaimSnapshot.TabIndex = 279;
			this.groupBoxClaimSnapshot.TabStop = false;
			this.groupBoxClaimSnapshot.Text = "Claim Snapshot";
			// 
			// comboClaimSnapshotTrigger
			// 
			this.comboClaimSnapshotTrigger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboClaimSnapshotTrigger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClaimSnapshotTrigger.FormattingEnabled = true;
			this.comboClaimSnapshotTrigger.Location = new System.Drawing.Point(229, 16);
			this.comboClaimSnapshotTrigger.MaxDropDownItems = 30;
			this.comboClaimSnapshotTrigger.Name = "comboClaimSnapshotTrigger";
			this.comboClaimSnapshotTrigger.Size = new System.Drawing.Size(148, 21);
			this.comboClaimSnapshotTrigger.TabIndex = 221;
			// 
			// textClaimSnapshotRunTime
			// 
			this.textClaimSnapshotRunTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textClaimSnapshotRunTime.Location = new System.Drawing.Point(267, 42);
			this.textClaimSnapshotRunTime.Name = "textClaimSnapshotRunTime";
			this.textClaimSnapshotRunTime.Size = new System.Drawing.Size(110, 20);
			this.textClaimSnapshotRunTime.TabIndex = 222;
			// 
			// label30
			// 
			this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label30.Location = new System.Drawing.Point(101, 43);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(165, 17);
			this.label30.TabIndex = 223;
			this.label30.Text = "Service Run Time";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label31
			// 
			this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label31.Location = new System.Drawing.Point(101, 18);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(127, 17);
			this.label31.TabIndex = 224;
			this.label31.Text = "Claim Snapshot Trigger";
			this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBoxSuperFamily
			// 
			this.groupBoxSuperFamily.Controls.Add(this.comboSuperFamSort);
			this.groupBoxSuperFamily.Controls.Add(this.labelSuperFamSort);
			this.groupBoxSuperFamily.Controls.Add(this.checkSuperFamSync);
			this.groupBoxSuperFamily.Controls.Add(this.checkSuperFamAddIns);
			this.groupBoxSuperFamily.Controls.Add(this.checkSuperFamCloneCreate);
			this.groupBoxSuperFamily.Location = new System.Drawing.Point(693, 24);
			this.groupBoxSuperFamily.Name = "groupBoxSuperFamily";
			this.groupBoxSuperFamily.Size = new System.Drawing.Size(383, 111);
			this.groupBoxSuperFamily.TabIndex = 278;
			this.groupBoxSuperFamily.TabStop = false;
			this.groupBoxSuperFamily.Text = "Super Family";
			// 
			// comboSuperFamSort
			// 
			this.comboSuperFamSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboSuperFamSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSuperFamSort.FormattingEnabled = true;
			this.comboSuperFamSort.Location = new System.Drawing.Point(249, 15);
			this.comboSuperFamSort.MaxDropDownItems = 30;
			this.comboSuperFamSort.Name = "comboSuperFamSort";
			this.comboSuperFamSort.Size = new System.Drawing.Size(128, 21);
			this.comboSuperFamSort.TabIndex = 217;
			// 
			// labelSuperFamSort
			// 
			this.labelSuperFamSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelSuperFamSort.Location = new System.Drawing.Point(94, 17);
			this.labelSuperFamSort.Name = "labelSuperFamSort";
			this.labelSuperFamSort.Size = new System.Drawing.Size(154, 17);
			this.labelSuperFamSort.TabIndex = 218;
			this.labelSuperFamSort.Text = "Super family sorting strategy";
			this.labelSuperFamSort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkSuperFamSync
			// 
			this.checkSuperFamSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkSuperFamSync.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSuperFamSync.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSuperFamSync.Location = new System.Drawing.Point(62, 42);
			this.checkSuperFamSync.Name = "checkSuperFamSync";
			this.checkSuperFamSync.Size = new System.Drawing.Size(315, 17);
			this.checkSuperFamSync.TabIndex = 219;
			this.checkSuperFamSync.Text = "Allow syncing patient information to all super family members";
			this.checkSuperFamSync.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkSuperFamAddIns
			// 
			this.checkSuperFamAddIns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkSuperFamAddIns.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSuperFamAddIns.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSuperFamAddIns.Location = new System.Drawing.Point(6, 64);
			this.checkSuperFamAddIns.Name = "checkSuperFamAddIns";
			this.checkSuperFamAddIns.Size = new System.Drawing.Size(371, 17);
			this.checkSuperFamAddIns.TabIndex = 221;
			this.checkSuperFamAddIns.Text = "Copy super guarantor\'s primary insurance to all new super family members";
			this.checkSuperFamAddIns.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkSuperFamCloneCreate
			// 
			this.checkSuperFamCloneCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkSuperFamCloneCreate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSuperFamCloneCreate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSuperFamCloneCreate.Location = new System.Drawing.Point(62, 86);
			this.checkSuperFamCloneCreate.Name = "checkSuperFamCloneCreate";
			this.checkSuperFamCloneCreate.Size = new System.Drawing.Size(315, 17);
			this.checkSuperFamCloneCreate.TabIndex = 227;
			this.checkSuperFamCloneCreate.Text = "New patient clones use super family instead of regular family";
			this.checkSuperFamCloneCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(45, 201);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(296, 17);
			this.label15.TabIndex = 264;
			this.label15.Text = "Coordination of Benefits (COB) rule";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkInsPlanExclusionsUseUCR
			// 
			this.checkInsPlanExclusionsUseUCR.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsPlanExclusionsUseUCR.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkInsPlanExclusionsUseUCR.Location = new System.Drawing.Point(53, 496);
			this.checkInsPlanExclusionsUseUCR.Name = "checkInsPlanExclusionsUseUCR";
			this.checkInsPlanExclusionsUseUCR.Size = new System.Drawing.Size(422, 17);
			this.checkInsPlanExclusionsUseUCR.TabIndex = 277;
			this.checkInsPlanExclusionsUseUCR.Text = "Ins plans with exclusions use UCR fee (can be overridden by plan)";
			this.checkInsPlanExclusionsUseUCR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkInsPlanExclusionsMarkDoNotBill
			// 
			this.checkInsPlanExclusionsMarkDoNotBill.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsPlanExclusionsMarkDoNotBill.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkInsPlanExclusionsMarkDoNotBill.Location = new System.Drawing.Point(53, 519);
			this.checkInsPlanExclusionsMarkDoNotBill.Name = "checkInsPlanExclusionsMarkDoNotBill";
			this.checkInsPlanExclusionsMarkDoNotBill.Size = new System.Drawing.Size(422, 17);
			this.checkInsPlanExclusionsMarkDoNotBill.TabIndex = 276;
			this.checkInsPlanExclusionsMarkDoNotBill.Text = "Ins plans with exclusions mark as Do Not Bill Ins";
			this.checkInsPlanExclusionsMarkDoNotBill.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkFixedBenefitBlankLikeZero
			// 
			this.checkFixedBenefitBlankLikeZero.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFixedBenefitBlankLikeZero.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkFixedBenefitBlankLikeZero.Location = new System.Drawing.Point(50, 125);
			this.checkFixedBenefitBlankLikeZero.Name = "checkFixedBenefitBlankLikeZero";
			this.checkFixedBenefitBlankLikeZero.Size = new System.Drawing.Size(425, 17);
			this.checkFixedBenefitBlankLikeZero.TabIndex = 275;
			this.checkFixedBenefitBlankLikeZero.Text = "Fixed benefit fee schedules treat blank entries as zero";
			this.checkFixedBenefitBlankLikeZero.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAllowPatsAtHQ
			// 
			this.checkAllowPatsAtHQ.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowPatsAtHQ.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllowPatsAtHQ.Location = new System.Drawing.Point(53, 473);
			this.checkAllowPatsAtHQ.Name = "checkAllowPatsAtHQ";
			this.checkAllowPatsAtHQ.Size = new System.Drawing.Size(422, 17);
			this.checkAllowPatsAtHQ.TabIndex = 274;
			this.checkAllowPatsAtHQ.Text = "Allow new patients to be added with an unassigned clinic";
			this.checkAllowPatsAtHQ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAutoFillPatEmail
			// 
			this.checkAutoFillPatEmail.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAutoFillPatEmail.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAutoFillPatEmail.Location = new System.Drawing.Point(50, 450);
			this.checkAutoFillPatEmail.Name = "checkAutoFillPatEmail";
			this.checkAutoFillPatEmail.Size = new System.Drawing.Size(425, 17);
			this.checkAutoFillPatEmail.TabIndex = 273;
			this.checkAutoFillPatEmail.Text = "Autofill patient\'s email with the guarantor\'s when adding many new patients";
			this.checkAutoFillPatEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPreferredReferrals
			// 
			this.checkPreferredReferrals.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPreferredReferrals.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPreferredReferrals.Location = new System.Drawing.Point(50, 427);
			this.checkPreferredReferrals.Name = "checkPreferredReferrals";
			this.checkPreferredReferrals.Size = new System.Drawing.Size(425, 17);
			this.checkPreferredReferrals.TabIndex = 272;
			this.checkPreferredReferrals.Text = "Show preferred referrals only in the Select Referral window by default";
			this.checkPreferredReferrals.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkTextMsgOkStatusTreatAsNo
			// 
			this.checkTextMsgOkStatusTreatAsNo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTextMsgOkStatusTreatAsNo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTextMsgOkStatusTreatAsNo.Location = new System.Drawing.Point(50, 229);
			this.checkTextMsgOkStatusTreatAsNo.Name = "checkTextMsgOkStatusTreatAsNo";
			this.checkTextMsgOkStatusTreatAsNo.Size = new System.Drawing.Size(425, 17);
			this.checkTextMsgOkStatusTreatAsNo.TabIndex = 265;
			this.checkTextMsgOkStatusTreatAsNo.Text = "Text Msg OK status, treat ?? as No instead of Yes";
			this.checkTextMsgOkStatusTreatAsNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPatInitBillingTypeFromPriInsPlan
			// 
			this.checkPatInitBillingTypeFromPriInsPlan.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPatInitBillingTypeFromPriInsPlan.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPatInitBillingTypeFromPriInsPlan.Location = new System.Drawing.Point(50, 404);
			this.checkPatInitBillingTypeFromPriInsPlan.Name = "checkPatInitBillingTypeFromPriInsPlan";
			this.checkPatInitBillingTypeFromPriInsPlan.Size = new System.Drawing.Size(425, 17);
			this.checkPatInitBillingTypeFromPriInsPlan.TabIndex = 268;
			this.checkPatInitBillingTypeFromPriInsPlan.Text = "New patient primary insurance plan sets patient billing type";
			this.checkPatInitBillingTypeFromPriInsPlan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkFamPhiAccess
			// 
			this.checkFamPhiAccess.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFamPhiAccess.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkFamPhiAccess.Location = new System.Drawing.Point(50, 254);
			this.checkFamPhiAccess.Name = "checkFamPhiAccess";
			this.checkFamPhiAccess.Size = new System.Drawing.Size(425, 17);
			this.checkFamPhiAccess.TabIndex = 269;
			this.checkFamPhiAccess.Text = "Allow guarantor access to family health information in the Patient Portal";
			this.checkFamPhiAccess.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClaimTrackingRequireError
			// 
			this.checkClaimTrackingRequireError.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimTrackingRequireError.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimTrackingRequireError.Location = new System.Drawing.Point(50, 379);
			this.checkClaimTrackingRequireError.Name = "checkClaimTrackingRequireError";
			this.checkClaimTrackingRequireError.Size = new System.Drawing.Size(425, 17);
			this.checkClaimTrackingRequireError.TabIndex = 266;
			this.checkClaimTrackingRequireError.Text = "Require error code when adding claim custom tracking status";
			this.checkClaimTrackingRequireError.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPPOpercentage
			// 
			this.checkPPOpercentage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPPOpercentage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPPOpercentage.Location = new System.Drawing.Point(50, 50);
			this.checkPPOpercentage.Name = "checkPPOpercentage";
			this.checkPPOpercentage.Size = new System.Drawing.Size(425, 17);
			this.checkPPOpercentage.TabIndex = 258;
			this.checkPPOpercentage.Text = "Default new insurance plans to PPO Percentage plan type";
			this.checkPPOpercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkInsurancePlansShared
			// 
			this.checkInsurancePlansShared.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsurancePlansShared.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkInsurancePlansShared.Location = new System.Drawing.Point(50, 25);
			this.checkInsurancePlansShared.Name = "checkInsurancePlansShared";
			this.checkInsurancePlansShared.Size = new System.Drawing.Size(425, 17);
			this.checkInsurancePlansShared.TabIndex = 257;
			this.checkInsurancePlansShared.Text = "InsPlan option at bottom, \'Change Plan for all subscribers\', is default";
			this.checkInsurancePlansShared.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClaimUseOverrideProcDescript
			// 
			this.checkClaimUseOverrideProcDescript.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimUseOverrideProcDescript.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimUseOverrideProcDescript.Location = new System.Drawing.Point(50, 354);
			this.checkClaimUseOverrideProcDescript.Name = "checkClaimUseOverrideProcDescript";
			this.checkClaimUseOverrideProcDescript.Size = new System.Drawing.Size(425, 17);
			this.checkClaimUseOverrideProcDescript.TabIndex = 263;
			this.checkClaimUseOverrideProcDescript.Text = "Use the description for the charted procedure code on printed claims";
			this.checkClaimUseOverrideProcDescript.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkInsDefaultAssignmentOfBenefits
			// 
			this.checkInsDefaultAssignmentOfBenefits.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsDefaultAssignmentOfBenefits.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkInsDefaultAssignmentOfBenefits.Location = new System.Drawing.Point(50, 175);
			this.checkInsDefaultAssignmentOfBenefits.Name = "checkInsDefaultAssignmentOfBenefits";
			this.checkInsDefaultAssignmentOfBenefits.Size = new System.Drawing.Size(425, 17);
			this.checkInsDefaultAssignmentOfBenefits.TabIndex = 267;
			this.checkInsDefaultAssignmentOfBenefits.Text = "Insurance plans default to assignment of benefits";
			this.checkInsDefaultAssignmentOfBenefits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsDefaultAssignmentOfBenefits.Click += new System.EventHandler(this.checkInsDefaultAssignmentOfBenefits_Click);
			// 
			// checkSelectProv
			// 
			this.checkSelectProv.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSelectProv.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSelectProv.Location = new System.Drawing.Point(50, 329);
			this.checkSelectProv.Name = "checkSelectProv";
			this.checkSelectProv.Size = new System.Drawing.Size(425, 17);
			this.checkSelectProv.TabIndex = 256;
			this.checkSelectProv.Text = "Primary Provider defaults to \'Select Provider\' in Patient Edit and Add Family win" +
    "dows";
			this.checkSelectProv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboCobRule
			// 
			this.comboCobRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCobRule.FormattingEnabled = true;
			this.comboCobRule.Location = new System.Drawing.Point(347, 200);
			this.comboCobRule.MaxDropDownItems = 30;
			this.comboCobRule.Name = "comboCobRule";
			this.comboCobRule.Size = new System.Drawing.Size(128, 21);
			this.comboCobRule.TabIndex = 262;
			this.comboCobRule.SelectionChangeCommitted += new System.EventHandler(this.comboCobRule_SelectionChangeCommitted);
			// 
			// checkAllowedFeeSchedsAutomate
			// 
			this.checkAllowedFeeSchedsAutomate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowedFeeSchedsAutomate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllowedFeeSchedsAutomate.Location = new System.Drawing.Point(50, 75);
			this.checkAllowedFeeSchedsAutomate.Name = "checkAllowedFeeSchedsAutomate";
			this.checkAllowedFeeSchedsAutomate.Size = new System.Drawing.Size(425, 17);
			this.checkAllowedFeeSchedsAutomate.TabIndex = 259;
			this.checkAllowedFeeSchedsAutomate.Text = "Use Blue Book";
			this.checkAllowedFeeSchedsAutomate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowedFeeSchedsAutomate.Click += new System.EventHandler(this.checkAllowedFeeSchedsAutomate_Click);
			// 
			// checkGoogleAddress
			// 
			this.checkGoogleAddress.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkGoogleAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkGoogleAddress.Location = new System.Drawing.Point(50, 304);
			this.checkGoogleAddress.Name = "checkGoogleAddress";
			this.checkGoogleAddress.Size = new System.Drawing.Size(425, 17);
			this.checkGoogleAddress.TabIndex = 271;
			this.checkGoogleAddress.Text = "Show Google Maps in Patient Edit window";
			this.checkGoogleAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkInsPPOsecWriteoffs
			// 
			this.checkInsPPOsecWriteoffs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsPPOsecWriteoffs.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkInsPPOsecWriteoffs.Location = new System.Drawing.Point(50, 279);
			this.checkInsPPOsecWriteoffs.Name = "checkInsPPOsecWriteoffs";
			this.checkInsPPOsecWriteoffs.Size = new System.Drawing.Size(425, 17);
			this.checkInsPPOsecWriteoffs.TabIndex = 270;
			this.checkInsPPOsecWriteoffs.Text = "Calculate secondary insurance PPO write-offs (not recommended, see manual)";
			this.checkInsPPOsecWriteoffs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsPPOsecWriteoffs.UseVisualStyleBackColor = true;
			// 
			// checkInsDefaultShowUCRonClaims
			// 
			this.checkInsDefaultShowUCRonClaims.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsDefaultShowUCRonClaims.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkInsDefaultShowUCRonClaims.Location = new System.Drawing.Point(50, 150);
			this.checkInsDefaultShowUCRonClaims.Name = "checkInsDefaultShowUCRonClaims";
			this.checkInsDefaultShowUCRonClaims.Size = new System.Drawing.Size(425, 17);
			this.checkInsDefaultShowUCRonClaims.TabIndex = 261;
			this.checkInsDefaultShowUCRonClaims.Text = "Insurance plans default to show UCR fee on claims";
			this.checkInsDefaultShowUCRonClaims.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsDefaultShowUCRonClaims.Click += new System.EventHandler(this.checkInsDefaultShowUCRonClaims_Click);
			// 
			// checkCoPayFeeScheduleBlankLikeZero
			// 
			this.checkCoPayFeeScheduleBlankLikeZero.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCoPayFeeScheduleBlankLikeZero.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkCoPayFeeScheduleBlankLikeZero.Location = new System.Drawing.Point(50, 100);
			this.checkCoPayFeeScheduleBlankLikeZero.Name = "checkCoPayFeeScheduleBlankLikeZero";
			this.checkCoPayFeeScheduleBlankLikeZero.Size = new System.Drawing.Size(425, 17);
			this.checkCoPayFeeScheduleBlankLikeZero.TabIndex = 260;
			this.checkCoPayFeeScheduleBlankLikeZero.Text = "Copay fee schedules treat blank entries as zero";
			this.checkCoPayFeeScheduleBlankLikeZero.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabAccount
			// 
			this.tabAccount.BackColor = System.Drawing.SystemColors.Window;
			this.tabAccount.Controls.Add(this.groupBox10);
			this.tabAccount.Controls.Add(this.groupBox9);
			this.tabAccount.Controls.Add(this.checkAgingProcLifo);
			this.tabAccount.Controls.Add(this.groupBox5);
			this.tabAccount.Controls.Add(this.groupBox7);
			this.tabAccount.Controls.Add(this.groupRepeatingCharges);
			this.tabAccount.Controls.Add(this.groupRecurringCharges);
			this.tabAccount.Controls.Add(this.checkBalancesDontSubtractIns);
			this.tabAccount.Controls.Add(this.checkAllowFutureTrans);
			this.tabAccount.Controls.Add(this.checkPpoUseUcr);
			this.tabAccount.Controls.Add(this.groupCommLogs);
			this.tabAccount.Controls.Add(this.checkAccountShowPaymentNums);
			this.tabAccount.Controls.Add(this.checkShowAllocateUnearnedPaymentPrompt);
			this.tabAccount.Controls.Add(this.checkAgingMonthly);
			this.tabAccount.Controls.Add(this.checkStatementInvoiceGridShowWriteoffs);
			this.tabAccount.Controls.Add(this.groupPayPlans);
			this.tabAccount.Location = new System.Drawing.Point(4, 22);
			this.tabAccount.Name = "tabAccount";
			this.tabAccount.Padding = new System.Windows.Forms.Padding(3);
			this.tabAccount.Size = new System.Drawing.Size(1168, 641);
			this.tabAccount.TabIndex = 2;
			this.tabAccount.Text = "Account";
			// 
			// groupBox10
			// 
			this.groupBox10.Controls.Add(this.comboFinanceChargeAdjType);
			this.groupBox10.Controls.Add(this.comboBillingChargeAdjType);
			this.groupBox10.Controls.Add(this.comboSalesTaxAdjType);
			this.groupBox10.Controls.Add(this.textTaxPercent);
			this.groupBox10.Controls.Add(this.label41);
			this.groupBox10.Controls.Add(this.comboPayPlanAdj);
			this.groupBox10.Controls.Add(this.label42);
			this.groupBox10.Controls.Add(this.label4);
			this.groupBox10.Controls.Add(this.label12);
			this.groupBox10.Controls.Add(this.label33);
			this.groupBox10.Controls.Add(this.label26);
			this.groupBox10.Controls.Add(this.comboRigorousAdjustments);
			this.groupBox10.Controls.Add(this.groupBox4);
			this.groupBox10.Location = new System.Drawing.Point(23, 277);
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.Size = new System.Drawing.Size(355, 242);
			this.groupBox10.TabIndex = 302;
			this.groupBox10.TabStop = false;
			this.groupBox10.Text = "Adjustments";
			// 
			// comboFinanceChargeAdjType
			// 
			this.comboFinanceChargeAdjType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboFinanceChargeAdjType.Location = new System.Drawing.Point(224, 16);
			this.comboFinanceChargeAdjType.Name = "comboFinanceChargeAdjType";
			this.comboFinanceChargeAdjType.Size = new System.Drawing.Size(125, 21);
			this.comboFinanceChargeAdjType.TabIndex = 276;
			// 
			// comboBillingChargeAdjType
			// 
			this.comboBillingChargeAdjType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBillingChargeAdjType.Location = new System.Drawing.Point(224, 42);
			this.comboBillingChargeAdjType.Name = "comboBillingChargeAdjType";
			this.comboBillingChargeAdjType.Size = new System.Drawing.Size(125, 21);
			this.comboBillingChargeAdjType.TabIndex = 279;
			// 
			// comboSalesTaxAdjType
			// 
			this.comboSalesTaxAdjType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboSalesTaxAdjType.Location = new System.Drawing.Point(224, 68);
			this.comboSalesTaxAdjType.Name = "comboSalesTaxAdjType";
			this.comboSalesTaxAdjType.Size = new System.Drawing.Size(125, 21);
			this.comboSalesTaxAdjType.TabIndex = 288;
			// 
			// textTaxPercent
			// 
			this.textTaxPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textTaxPercent.Location = new System.Drawing.Point(296, 121);
			this.textTaxPercent.Name = "textTaxPercent";
			this.textTaxPercent.Size = new System.Drawing.Size(53, 20);
			this.textTaxPercent.TabIndex = 285;
			// 
			// label41
			// 
			this.label41.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label41.BackColor = System.Drawing.SystemColors.Window;
			this.label41.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label41.Location = new System.Drawing.Point(19, 151);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(162, 14);
			this.label41.TabIndex = 298;
			this.label41.Text = "Enforce Valid Adjustments";
			this.label41.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboPayPlanAdj
			// 
			this.comboPayPlanAdj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboPayPlanAdj.Location = new System.Drawing.Point(224, 94);
			this.comboPayPlanAdj.Name = "comboPayPlanAdj";
			this.comboPayPlanAdj.Size = new System.Drawing.Size(125, 21);
			this.comboPayPlanAdj.TabIndex = 302;
			// 
			// label42
			// 
			this.label42.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label42.BackColor = System.Drawing.SystemColors.Window;
			this.label42.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label42.Location = new System.Drawing.Point(114, 97);
			this.label42.Name = "label42";
			this.label42.Size = new System.Drawing.Size(108, 15);
			this.label42.TabIndex = 301;
			this.label42.Text = "Payment Plan adj type";
			this.label42.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.BackColor = System.Drawing.SystemColors.Window;
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(62, 19);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(158, 15);
			this.label4.TabIndex = 278;
			this.label4.Text = "Finance charge adj type";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label12
			// 
			this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label12.BackColor = System.Drawing.SystemColors.Window;
			this.label12.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label12.Location = new System.Drawing.Point(112, 46);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(108, 15);
			this.label12.TabIndex = 277;
			this.label12.Text = "Billing charge adj type";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label33
			// 
			this.label33.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label33.BackColor = System.Drawing.SystemColors.Window;
			this.label33.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label33.Location = new System.Drawing.Point(112, 71);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(108, 15);
			this.label33.TabIndex = 287;
			this.label33.Text = "Sales Tax adj type";
			this.label33.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label26
			// 
			this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label26.BackColor = System.Drawing.SystemColors.Window;
			this.label26.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label26.Location = new System.Drawing.Point(136, 123);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(155, 16);
			this.label26.TabIndex = 286;
			this.label26.Text = "Sales Tax percentage";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboRigorousAdjustments
			// 
			this.comboRigorousAdjustments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboRigorousAdjustments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboRigorousAdjustments.Location = new System.Drawing.Point(186, 147);
			this.comboRigorousAdjustments.MaxDropDownItems = 30;
			this.comboRigorousAdjustments.Name = "comboRigorousAdjustments";
			this.comboRigorousAdjustments.Size = new System.Drawing.Size(163, 21);
			this.comboRigorousAdjustments.TabIndex = 299;
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.listboxBadDebtAdjs);
			this.groupBox4.Controls.Add(this.label29);
			this.groupBox4.Controls.Add(this.butBadDebt);
			this.groupBox4.Location = new System.Drawing.Point(23, 175);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(326, 59);
			this.groupBox4.TabIndex = 283;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Bad Debt Adjustments";
			// 
			// listboxBadDebtAdjs
			// 
			this.listboxBadDebtAdjs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.listboxBadDebtAdjs.FormattingEnabled = true;
			this.listboxBadDebtAdjs.Location = new System.Drawing.Point(201, 11);
			this.listboxBadDebtAdjs.Name = "listboxBadDebtAdjs";
			this.listboxBadDebtAdjs.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listboxBadDebtAdjs.Size = new System.Drawing.Size(120, 43);
			this.listboxBadDebtAdjs.TabIndex = 197;
			// 
			// label29
			// 
			this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label29.Location = new System.Drawing.Point(53, 12);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(147, 20);
			this.label29.TabIndex = 223;
			this.label29.Text = "Current Bad Debt adj types:";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butBadDebt
			// 
			this.butBadDebt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butBadDebt.Location = new System.Drawing.Point(129, 33);
			this.butBadDebt.Name = "butBadDebt";
			this.butBadDebt.Size = new System.Drawing.Size(68, 21);
			this.butBadDebt.TabIndex = 197;
			this.butBadDebt.Text = "Edit";
			this.butBadDebt.Click += new System.EventHandler(this.butBadDebt_Click);
			// 
			// groupBox9
			// 
			this.groupBox9.Controls.Add(this.checkPaymentsTransferPatientIncomeOnly);
			this.groupBox9.Controls.Add(this.checkStoreCCTokens);
			this.groupBox9.Controls.Add(this.comboPaymentClinicSetting);
			this.groupBox9.Controls.Add(this.label38);
			this.groupBox9.Controls.Add(this.checkPaymentsPromptForPayType);
			this.groupBox9.Controls.Add(this.checkHidePaysplits);
			this.groupBox9.Controls.Add(this.checkAllowPrepayProvider);
			this.groupBox9.Controls.Add(this.label40);
			this.groupBox9.Controls.Add(this.comboUnallocatedSplits);
			this.groupBox9.Controls.Add(this.label28);
			this.groupBox9.Controls.Add(this.checkAllowFutureDebits);
			this.groupBox9.Controls.Add(this.checkAllowEmailCCReceipt);
			this.groupBox9.Controls.Add(this.label39);
			this.groupBox9.Controls.Add(this.comboRigorousAccounting);
			this.groupBox9.Controls.Add(this.comboAutoSplitPref);
			this.groupBox9.Location = new System.Drawing.Point(23, 8);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(355, 264);
			this.groupBox9.TabIndex = 301;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "Payments";
			// 
			// checkPaymentsTransferPatientIncomeOnly
			// 
			this.checkPaymentsTransferPatientIncomeOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkPaymentsTransferPatientIncomeOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPaymentsTransferPatientIncomeOnly.Location = new System.Drawing.Point(95, 240);
			this.checkPaymentsTransferPatientIncomeOnly.Name = "checkPaymentsTransferPatientIncomeOnly";
			this.checkPaymentsTransferPatientIncomeOnly.Size = new System.Drawing.Size(255, 17);
			this.checkPaymentsTransferPatientIncomeOnly.TabIndex = 304;
			this.checkPaymentsTransferPatientIncomeOnly.Text = "Only transfer patient generated income";
			this.checkPaymentsTransferPatientIncomeOnly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPaymentsTransferPatientIncomeOnly.UseVisualStyleBackColor = true;
			this.checkPaymentsTransferPatientIncomeOnly.Visible = false;
			// 
			// checkStoreCCTokens
			// 
			this.checkStoreCCTokens.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkStoreCCTokens.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStoreCCTokens.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkStoreCCTokens.Location = new System.Drawing.Point(136, 17);
			this.checkStoreCCTokens.Name = "checkStoreCCTokens";
			this.checkStoreCCTokens.Size = new System.Drawing.Size(213, 17);
			this.checkStoreCCTokens.TabIndex = 280;
			this.checkStoreCCTokens.Text = "Automatically store credit card tokens";
			this.checkStoreCCTokens.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStoreCCTokens.UseVisualStyleBackColor = true;
			// 
			// comboPaymentClinicSetting
			// 
			this.comboPaymentClinicSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboPaymentClinicSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPaymentClinicSetting.FormattingEnabled = true;
			this.comboPaymentClinicSetting.Location = new System.Drawing.Point(186, 38);
			this.comboPaymentClinicSetting.MaxDropDownItems = 30;
			this.comboPaymentClinicSetting.Name = "comboPaymentClinicSetting";
			this.comboPaymentClinicSetting.Size = new System.Drawing.Size(163, 21);
			this.comboPaymentClinicSetting.TabIndex = 290;
			// 
			// label38
			// 
			this.label38.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label38.Location = new System.Drawing.Point(59, 38);
			this.label38.Margin = new System.Windows.Forms.Padding(0);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(125, 21);
			this.label38.TabIndex = 291;
			this.label38.Text = "Patient Payments use";
			this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPaymentsPromptForPayType
			// 
			this.checkPaymentsPromptForPayType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkPaymentsPromptForPayType.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPaymentsPromptForPayType.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPaymentsPromptForPayType.Location = new System.Drawing.Point(79, 63);
			this.checkPaymentsPromptForPayType.Name = "checkPaymentsPromptForPayType";
			this.checkPaymentsPromptForPayType.Size = new System.Drawing.Size(270, 17);
			this.checkPaymentsPromptForPayType.TabIndex = 284;
			this.checkPaymentsPromptForPayType.Text = "Payments prompt for Payment Type";
			this.checkPaymentsPromptForPayType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkHidePaysplits
			// 
			this.checkHidePaysplits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkHidePaysplits.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHidePaysplits.Location = new System.Drawing.Point(95, 223);
			this.checkHidePaysplits.Name = "checkHidePaysplits";
			this.checkHidePaysplits.Size = new System.Drawing.Size(255, 17);
			this.checkHidePaysplits.TabIndex = 297;
			this.checkHidePaysplits.Text = "Hide paysplits from Payment window by default";
			this.checkHidePaysplits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHidePaysplits.UseVisualStyleBackColor = true;
			// 
			// checkAllowPrepayProvider
			// 
			this.checkAllowPrepayProvider.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkAllowPrepayProvider.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowPrepayProvider.Location = new System.Drawing.Point(140, 177);
			this.checkAllowPrepayProvider.Name = "checkAllowPrepayProvider";
			this.checkAllowPrepayProvider.Size = new System.Drawing.Size(210, 17);
			this.checkAllowPrepayProvider.TabIndex = 303;
			this.checkAllowPrepayProvider.Text = "Allow prepayments to providers";
			this.checkAllowPrepayProvider.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowPrepayProvider.UseVisualStyleBackColor = true;
			// 
			// label40
			// 
			this.label40.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label40.BackColor = System.Drawing.SystemColors.Window;
			this.label40.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label40.Location = new System.Drawing.Point(73, 200);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(108, 14);
			this.label40.TabIndex = 296;
			this.label40.Text = "Auto-split payments preferring:";
			this.label40.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboUnallocatedSplits
			// 
			this.comboUnallocatedSplits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboUnallocatedSplits.Location = new System.Drawing.Point(224, 84);
			this.comboUnallocatedSplits.Name = "comboUnallocatedSplits";
			this.comboUnallocatedSplits.Size = new System.Drawing.Size(125, 21);
			this.comboUnallocatedSplits.TabIndex = 281;
			// 
			// label28
			// 
			this.label28.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label28.BackColor = System.Drawing.SystemColors.Window;
			this.label28.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label28.Location = new System.Drawing.Point(91, 82);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(129, 31);
			this.label28.TabIndex = 282;
			this.label28.Text = "Default unearned type for unallocated paysplits";
			this.label28.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkAllowFutureDebits
			// 
			this.checkAllowFutureDebits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkAllowFutureDebits.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowFutureDebits.Location = new System.Drawing.Point(23, 113);
			this.checkAllowFutureDebits.Name = "checkAllowFutureDebits";
			this.checkAllowFutureDebits.Size = new System.Drawing.Size(327, 17);
			this.checkAllowFutureDebits.TabIndex = 289;
			this.checkAllowFutureDebits.Text = "Allow future dated payments (not recommended, see manual)";
			this.checkAllowFutureDebits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowFutureDebits.UseVisualStyleBackColor = true;
			// 
			// checkAllowEmailCCReceipt
			// 
			this.checkAllowEmailCCReceipt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkAllowEmailCCReceipt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowEmailCCReceipt.Location = new System.Drawing.Point(140, 130);
			this.checkAllowEmailCCReceipt.Name = "checkAllowEmailCCReceipt";
			this.checkAllowEmailCCReceipt.Size = new System.Drawing.Size(210, 17);
			this.checkAllowEmailCCReceipt.TabIndex = 292;
			this.checkAllowEmailCCReceipt.Text = "Allow emailing credit card receipts";
			this.checkAllowEmailCCReceipt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowEmailCCReceipt.UseVisualStyleBackColor = true;
			// 
			// label39
			// 
			this.label39.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label39.BackColor = System.Drawing.SystemColors.Window;
			this.label39.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label39.Location = new System.Drawing.Point(73, 155);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(108, 14);
			this.label39.TabIndex = 293;
			this.label39.Text = "Enforce Valid Paysplits";
			this.label39.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboRigorousAccounting
			// 
			this.comboRigorousAccounting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboRigorousAccounting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboRigorousAccounting.FormattingEnabled = true;
			this.comboRigorousAccounting.Location = new System.Drawing.Point(186, 151);
			this.comboRigorousAccounting.MaxDropDownItems = 30;
			this.comboRigorousAccounting.Name = "comboRigorousAccounting";
			this.comboRigorousAccounting.Size = new System.Drawing.Size(163, 21);
			this.comboRigorousAccounting.TabIndex = 294;
			// 
			// comboAutoSplitPref
			// 
			this.comboAutoSplitPref.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboAutoSplitPref.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAutoSplitPref.FormattingEnabled = true;
			this.comboAutoSplitPref.Location = new System.Drawing.Point(186, 197);
			this.comboAutoSplitPref.MaxDropDownItems = 30;
			this.comboAutoSplitPref.Name = "comboAutoSplitPref";
			this.comboAutoSplitPref.Size = new System.Drawing.Size(163, 21);
			this.comboAutoSplitPref.TabIndex = 295;
			// 
			// checkAgingProcLifo
			// 
			this.checkAgingProcLifo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAgingProcLifo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAgingProcLifo.Location = new System.Drawing.Point(804, 149);
			this.checkAgingProcLifo.Name = "checkAgingProcLifo";
			this.checkAgingProcLifo.Size = new System.Drawing.Size(351, 17);
			this.checkAgingProcLifo.TabIndex = 307;
			this.checkAgingProcLifo.Text = "Transactions attached to a procedure offset each other before aging";
			this.checkAgingProcLifo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAgingProcLifo.ThreeState = true;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.checkAllowPrePayToTpProcs);
			this.groupBox5.Controls.Add(this.checkIsRefundable);
			this.groupBox5.Controls.Add(this.label57);
			this.groupBox5.Controls.Add(this.comboTpUnearnedType);
			this.groupBox5.Location = new System.Drawing.Point(23, 524);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(355, 87);
			this.groupBox5.TabIndex = 300;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Treatment planned prepayments";
			// 
			// checkAllowPrePayToTpProcs
			// 
			this.checkAllowPrePayToTpProcs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkAllowPrePayToTpProcs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowPrePayToTpProcs.Location = new System.Drawing.Point(23, 14);
			this.checkAllowPrePayToTpProcs.Name = "checkAllowPrePayToTpProcs";
			this.checkAllowPrePayToTpProcs.Size = new System.Drawing.Size(330, 17);
			this.checkAllowPrePayToTpProcs.TabIndex = 5;
			this.checkAllowPrePayToTpProcs.Text = "Allow prepayments to allocate to treatment planned procedures";
			this.checkAllowPrePayToTpProcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowPrePayToTpProcs.UseVisualStyleBackColor = true;
			this.checkAllowPrePayToTpProcs.Click += new System.EventHandler(this.CheckAllowPrePayToTpProcs_Click);
			// 
			// checkIsRefundable
			// 
			this.checkIsRefundable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkIsRefundable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsRefundable.Location = new System.Drawing.Point(140, 32);
			this.checkIsRefundable.Name = "checkIsRefundable";
			this.checkIsRefundable.Size = new System.Drawing.Size(213, 17);
			this.checkIsRefundable.TabIndex = 10;
			this.checkIsRefundable.Text = "TP prepayments are non-refundable";
			this.checkIsRefundable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsRefundable.UseVisualStyleBackColor = true;
			this.checkIsRefundable.Visible = false;
			// 
			// label57
			// 
			this.label57.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label57.BackColor = System.Drawing.SystemColors.Window;
			this.label57.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label57.Location = new System.Drawing.Point(35, 50);
			this.label57.Name = "label57";
			this.label57.Size = new System.Drawing.Size(145, 27);
			this.label57.TabIndex = 253;
			this.label57.Text = "Default treatment planned procedure unearned type";
			this.label57.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboTpUnearnedType
			// 
			this.comboTpUnearnedType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboTpUnearnedType.Location = new System.Drawing.Point(186, 55);
			this.comboTpUnearnedType.Name = "comboTpUnearnedType";
			this.comboTpUnearnedType.Size = new System.Drawing.Size(163, 21);
			this.comboTpUnearnedType.TabIndex = 15;
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.checkCanadianPpoLabEst);
			this.groupBox7.Controls.Add(this.checkInsEstRecalcReceived);
			this.groupBox7.Controls.Add(this.checkPromptForSecondaryClaim);
			this.groupBox7.Controls.Add(this.checkInsPayNoWriteoffMoreThanProc);
			this.groupBox7.Controls.Add(this.checkClaimTrackingExcludeNone);
			this.groupBox7.Controls.Add(this.label55);
			this.groupBox7.Controls.Add(this.comboZeroDollarProcClaimBehavior);
			this.groupBox7.Controls.Add(this.labelClaimCredit);
			this.groupBox7.Controls.Add(this.comboClaimCredit);
			this.groupBox7.Controls.Add(this.checkAllowFuturePayments);
			this.groupBox7.Controls.Add(this.groupBoxClaimIdPrefix);
			this.groupBox7.Controls.Add(this.checkAllowProcAdjFromClaim);
			this.groupBox7.Controls.Add(this.checkProviderIncomeShows);
			this.groupBox7.Controls.Add(this.checkClaimFormTreatDentSaysSigOnFile);
			this.groupBox7.Controls.Add(this.checkClaimMedTypeIsInstWhenInsPlanIsMedical);
			this.groupBox7.Controls.Add(this.checkEclaimsMedicalProvTreatmentAsOrdering);
			this.groupBox7.Controls.Add(this.checkEclaimsSeparateTreatProv);
			this.groupBox7.Controls.Add(this.label20);
			this.groupBox7.Controls.Add(this.textClaimAttachPath);
			this.groupBox7.Controls.Add(this.checkClaimsValidateACN);
			this.groupBox7.Controls.Add(this.textInsWriteoffDescript);
			this.groupBox7.Controls.Add(this.label17);
			this.groupBox7.Location = new System.Drawing.Point(402, 7);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(379, 462);
			this.groupBox7.TabIndex = 304;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Insurance";
			// 
			// checkCanadianPpoLabEst
			// 
			this.checkCanadianPpoLabEst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkCanadianPpoLabEst.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCanadianPpoLabEst.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkCanadianPpoLabEst.Location = new System.Drawing.Point(20, 437);
			this.checkCanadianPpoLabEst.Name = "checkCanadianPpoLabEst";
			this.checkCanadianPpoLabEst.Size = new System.Drawing.Size(353, 17);
			this.checkCanadianPpoLabEst.TabIndex = 297;
			this.checkCanadianPpoLabEst.Text = "Canadian PPO insurance plans create lab estimates";
			this.checkCanadianPpoLabEst.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCanadianPpoLabEst.UseVisualStyleBackColor = true;
			// 
			// checkInsEstRecalcReceived
			// 
			this.checkInsEstRecalcReceived.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkInsEstRecalcReceived.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsEstRecalcReceived.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkInsEstRecalcReceived.Location = new System.Drawing.Point(29, 418);
			this.checkInsEstRecalcReceived.Name = "checkInsEstRecalcReceived";
			this.checkInsEstRecalcReceived.Size = new System.Drawing.Size(344, 17);
			this.checkInsEstRecalcReceived.TabIndex = 296;
			this.checkInsEstRecalcReceived.Text = "Recalculate estimates for received claim procedures";
			this.checkInsEstRecalcReceived.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPromptForSecondaryClaim
			// 
			this.checkPromptForSecondaryClaim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkPromptForSecondaryClaim.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPromptForSecondaryClaim.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPromptForSecondaryClaim.Location = new System.Drawing.Point(29, 400);
			this.checkPromptForSecondaryClaim.Name = "checkPromptForSecondaryClaim";
			this.checkPromptForSecondaryClaim.Size = new System.Drawing.Size(344, 17);
			this.checkPromptForSecondaryClaim.TabIndex = 295;
			this.checkPromptForSecondaryClaim.Text = "Prompt for secondary claims";
			this.checkPromptForSecondaryClaim.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkInsPayNoWriteoffMoreThanProc
			// 
			this.checkInsPayNoWriteoffMoreThanProc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkInsPayNoWriteoffMoreThanProc.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsPayNoWriteoffMoreThanProc.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkInsPayNoWriteoffMoreThanProc.Location = new System.Drawing.Point(11, 382);
			this.checkInsPayNoWriteoffMoreThanProc.Name = "checkInsPayNoWriteoffMoreThanProc";
			this.checkInsPayNoWriteoffMoreThanProc.Size = new System.Drawing.Size(362, 17);
			this.checkInsPayNoWriteoffMoreThanProc.TabIndex = 294;
			this.checkInsPayNoWriteoffMoreThanProc.Text = "Disallow write-offs greater than the adjusted procedure fee";
			this.checkInsPayNoWriteoffMoreThanProc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClaimTrackingExcludeNone
			// 
			this.checkClaimTrackingExcludeNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkClaimTrackingExcludeNone.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimTrackingExcludeNone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimTrackingExcludeNone.Location = new System.Drawing.Point(11, 364);
			this.checkClaimTrackingExcludeNone.Name = "checkClaimTrackingExcludeNone";
			this.checkClaimTrackingExcludeNone.Size = new System.Drawing.Size(362, 17);
			this.checkClaimTrackingExcludeNone.TabIndex = 293;
			this.checkClaimTrackingExcludeNone.Text = "Exclude \'None\' as an option on Custom Tracking Status";
			this.checkClaimTrackingExcludeNone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label55
			// 
			this.label55.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label55.Location = new System.Drawing.Point(22, 339);
			this.label55.Name = "label55";
			this.label55.Size = new System.Drawing.Size(180, 17);
			this.label55.TabIndex = 292;
			this.label55.Text = "Creating claims with $0 procedures";
			this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboZeroDollarProcClaimBehavior
			// 
			this.comboZeroDollarProcClaimBehavior.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboZeroDollarProcClaimBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboZeroDollarProcClaimBehavior.FormattingEnabled = true;
			this.comboZeroDollarProcClaimBehavior.Location = new System.Drawing.Point(205, 337);
			this.comboZeroDollarProcClaimBehavior.Name = "comboZeroDollarProcClaimBehavior";
			this.comboZeroDollarProcClaimBehavior.Size = new System.Drawing.Size(168, 21);
			this.comboZeroDollarProcClaimBehavior.TabIndex = 291;
			// 
			// labelClaimCredit
			// 
			this.labelClaimCredit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelClaimCredit.Location = new System.Drawing.Point(4, 215);
			this.labelClaimCredit.Name = "labelClaimCredit";
			this.labelClaimCredit.Size = new System.Drawing.Size(196, 17);
			this.labelClaimCredit.TabIndex = 290;
			this.labelClaimCredit.Text = "Payment exceeds procedure balance";
			this.labelClaimCredit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClaimCredit
			// 
			this.comboClaimCredit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboClaimCredit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClaimCredit.FormattingEnabled = true;
			this.comboClaimCredit.Location = new System.Drawing.Point(205, 211);
			this.comboClaimCredit.Name = "comboClaimCredit";
			this.comboClaimCredit.Size = new System.Drawing.Size(168, 21);
			this.comboClaimCredit.TabIndex = 289;
			// 
			// checkAllowFuturePayments
			// 
			this.checkAllowFuturePayments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkAllowFuturePayments.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowFuturePayments.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllowFuturePayments.Location = new System.Drawing.Point(194, 237);
			this.checkAllowFuturePayments.Name = "checkAllowFuturePayments";
			this.checkAllowFuturePayments.Size = new System.Drawing.Size(179, 17);
			this.checkAllowFuturePayments.TabIndex = 288;
			this.checkAllowFuturePayments.Text = "Allow future payments";
			this.checkAllowFuturePayments.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBoxClaimIdPrefix
			// 
			this.groupBoxClaimIdPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxClaimIdPrefix.Controls.Add(this.butReplacements);
			this.groupBoxClaimIdPrefix.Controls.Add(this.textClaimIdentifier);
			this.groupBoxClaimIdPrefix.Location = new System.Drawing.Point(194, 260);
			this.groupBoxClaimIdPrefix.Name = "groupBoxClaimIdPrefix";
			this.groupBoxClaimIdPrefix.Size = new System.Drawing.Size(179, 71);
			this.groupBoxClaimIdPrefix.TabIndex = 287;
			this.groupBoxClaimIdPrefix.TabStop = false;
			this.groupBoxClaimIdPrefix.Text = "Claim Identification Prefix";
			// 
			// butReplacements
			// 
			this.butReplacements.Location = new System.Drawing.Point(68, 42);
			this.butReplacements.Name = "butReplacements";
			this.butReplacements.Size = new System.Drawing.Size(107, 23);
			this.butReplacements.TabIndex = 240;
			this.butReplacements.Text = "Replacements";
			this.butReplacements.UseVisualStyleBackColor = true;
			this.butReplacements.Click += new System.EventHandler(this.butReplacements_Click);
			// 
			// textClaimIdentifier
			// 
			this.textClaimIdentifier.Location = new System.Drawing.Point(4, 19);
			this.textClaimIdentifier.Name = "textClaimIdentifier";
			this.textClaimIdentifier.Size = new System.Drawing.Size(171, 20);
			this.textClaimIdentifier.TabIndex = 238;
			// 
			// checkAllowProcAdjFromClaim
			// 
			this.checkAllowProcAdjFromClaim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkAllowProcAdjFromClaim.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowProcAdjFromClaim.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllowProcAdjFromClaim.Location = new System.Drawing.Point(28, 189);
			this.checkAllowProcAdjFromClaim.Name = "checkAllowProcAdjFromClaim";
			this.checkAllowProcAdjFromClaim.Size = new System.Drawing.Size(345, 17);
			this.checkAllowProcAdjFromClaim.TabIndex = 286;
			this.checkAllowProcAdjFromClaim.Text = "Allow procedure adjustments from Edit Claim window";
			this.checkAllowProcAdjFromClaim.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkProviderIncomeShows
			// 
			this.checkProviderIncomeShows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkProviderIncomeShows.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProviderIncomeShows.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProviderIncomeShows.Location = new System.Drawing.Point(11, 19);
			this.checkProviderIncomeShows.Name = "checkProviderIncomeShows";
			this.checkProviderIncomeShows.Size = new System.Drawing.Size(362, 17);
			this.checkProviderIncomeShows.TabIndex = 277;
			this.checkProviderIncomeShows.Text = "Show provider income transfer window after entering insurance payment";
			this.checkProviderIncomeShows.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClaimFormTreatDentSaysSigOnFile
			// 
			this.checkClaimFormTreatDentSaysSigOnFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkClaimFormTreatDentSaysSigOnFile.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimFormTreatDentSaysSigOnFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimFormTreatDentSaysSigOnFile.Location = new System.Drawing.Point(11, 55);
			this.checkClaimFormTreatDentSaysSigOnFile.Name = "checkClaimFormTreatDentSaysSigOnFile";
			this.checkClaimFormTreatDentSaysSigOnFile.Size = new System.Drawing.Size(362, 17);
			this.checkClaimFormTreatDentSaysSigOnFile.TabIndex = 282;
			this.checkClaimFormTreatDentSaysSigOnFile.Text = "Claim form treating provider shows \'Signature On File\' rather than name";
			this.checkClaimFormTreatDentSaysSigOnFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClaimMedTypeIsInstWhenInsPlanIsMedical
			// 
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.Location = new System.Drawing.Point(11, 37);
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.Name = "checkClaimMedTypeIsInstWhenInsPlanIsMedical";
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.Size = new System.Drawing.Size(362, 17);
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.TabIndex = 281;
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.Text = "Set medical claims to institutional when using medical insurance";
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkEclaimsMedicalProvTreatmentAsOrdering
			// 
			this.checkEclaimsMedicalProvTreatmentAsOrdering.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkEclaimsMedicalProvTreatmentAsOrdering.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEclaimsMedicalProvTreatmentAsOrdering.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEclaimsMedicalProvTreatmentAsOrdering.Location = new System.Drawing.Point(-2, 135);
			this.checkEclaimsMedicalProvTreatmentAsOrdering.Name = "checkEclaimsMedicalProvTreatmentAsOrdering";
			this.checkEclaimsMedicalProvTreatmentAsOrdering.Size = new System.Drawing.Size(375, 17);
			this.checkEclaimsMedicalProvTreatmentAsOrdering.TabIndex = 285;
			this.checkEclaimsMedicalProvTreatmentAsOrdering.Text = "On medical e-claims, send treating provider as ordering provider by default";
			this.checkEclaimsMedicalProvTreatmentAsOrdering.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkEclaimsSeparateTreatProv
			// 
			this.checkEclaimsSeparateTreatProv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkEclaimsSeparateTreatProv.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEclaimsSeparateTreatProv.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEclaimsSeparateTreatProv.Location = new System.Drawing.Point(28, 153);
			this.checkEclaimsSeparateTreatProv.Name = "checkEclaimsSeparateTreatProv";
			this.checkEclaimsSeparateTreatProv.Size = new System.Drawing.Size(345, 17);
			this.checkEclaimsSeparateTreatProv.TabIndex = 276;
			this.checkEclaimsSeparateTreatProv.Text = "On e-claims, send treating provider info for each separate procedure";
			this.checkEclaimsSeparateTreatProv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label20
			// 
			this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label20.BackColor = System.Drawing.SystemColors.Window;
			this.label20.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label20.Location = new System.Drawing.Point(23, 113);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(147, 17);
			this.label20.TabIndex = 279;
			this.label20.Text = "Claim attachment export path";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textClaimAttachPath
			// 
			this.textClaimAttachPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textClaimAttachPath.Location = new System.Drawing.Point(176, 109);
			this.textClaimAttachPath.Name = "textClaimAttachPath";
			this.textClaimAttachPath.Size = new System.Drawing.Size(197, 20);
			this.textClaimAttachPath.TabIndex = 278;
			// 
			// checkClaimsValidateACN
			// 
			this.checkClaimsValidateACN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkClaimsValidateACN.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimsValidateACN.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimsValidateACN.Location = new System.Drawing.Point(28, 171);
			this.checkClaimsValidateACN.Name = "checkClaimsValidateACN";
			this.checkClaimsValidateACN.Size = new System.Drawing.Size(345, 17);
			this.checkClaimsValidateACN.TabIndex = 280;
			this.checkClaimsValidateACN.Text = "Require ACN# in remarks on claims with ADDP group name";
			this.checkClaimsValidateACN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsWriteoffDescript
			// 
			this.textInsWriteoffDescript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsWriteoffDescript.Location = new System.Drawing.Point(210, 79);
			this.textInsWriteoffDescript.Name = "textInsWriteoffDescript";
			this.textInsWriteoffDescript.Size = new System.Drawing.Size(163, 20);
			this.textInsWriteoffDescript.TabIndex = 283;
			// 
			// label17
			// 
			this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label17.BackColor = System.Drawing.SystemColors.Window;
			this.label17.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label17.Location = new System.Drawing.Point(58, 76);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(145, 27);
			this.label17.TabIndex = 284;
			this.label17.Text = "PPO write-off description\r\n(blank for \"Write-off\")";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupRepeatingCharges
			// 
			this.groupRepeatingCharges.Controls.Add(this.labelRepeatingChargesAutomatedTime);
			this.groupRepeatingCharges.Controls.Add(this.textRepeatingChargesAutomatedTime);
			this.groupRepeatingCharges.Controls.Add(this.checkRepeatingChargesRunAging);
			this.groupRepeatingCharges.Controls.Add(this.checkRepeatingChargesAutomated);
			this.groupRepeatingCharges.Location = new System.Drawing.Point(804, 514);
			this.groupRepeatingCharges.Name = "groupRepeatingCharges";
			this.groupRepeatingCharges.Size = new System.Drawing.Size(357, 78);
			this.groupRepeatingCharges.TabIndex = 245;
			this.groupRepeatingCharges.TabStop = false;
			this.groupRepeatingCharges.Text = "Repeating Charges";
			// 
			// labelRepeatingChargesAutomatedTime
			// 
			this.labelRepeatingChargesAutomatedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelRepeatingChargesAutomatedTime.BackColor = System.Drawing.SystemColors.Window;
			this.labelRepeatingChargesAutomatedTime.Enabled = false;
			this.labelRepeatingChargesAutomatedTime.Location = new System.Drawing.Point(127, 53);
			this.labelRepeatingChargesAutomatedTime.Name = "labelRepeatingChargesAutomatedTime";
			this.labelRepeatingChargesAutomatedTime.Size = new System.Drawing.Size(154, 17);
			this.labelRepeatingChargesAutomatedTime.TabIndex = 243;
			this.labelRepeatingChargesAutomatedTime.Text = "Repeating charges run time";
			this.labelRepeatingChargesAutomatedTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRepeatingChargesAutomatedTime
			// 
			this.textRepeatingChargesAutomatedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textRepeatingChargesAutomatedTime.Enabled = false;
			this.textRepeatingChargesAutomatedTime.Location = new System.Drawing.Point(283, 52);
			this.textRepeatingChargesAutomatedTime.Name = "textRepeatingChargesAutomatedTime";
			this.textRepeatingChargesAutomatedTime.Size = new System.Drawing.Size(68, 20);
			this.textRepeatingChargesAutomatedTime.TabIndex = 241;
			this.textRepeatingChargesAutomatedTime.Leave += new System.EventHandler(this.PromptRecurringRepeatingChargesTimes);
			// 
			// checkRepeatingChargesRunAging
			// 
			this.checkRepeatingChargesRunAging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkRepeatingChargesRunAging.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRepeatingChargesRunAging.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRepeatingChargesRunAging.Location = new System.Drawing.Point(72, 14);
			this.checkRepeatingChargesRunAging.Name = "checkRepeatingChargesRunAging";
			this.checkRepeatingChargesRunAging.Size = new System.Drawing.Size(279, 17);
			this.checkRepeatingChargesRunAging.TabIndex = 239;
			this.checkRepeatingChargesRunAging.Text = "Repeating charges runs aging after posting charges";
			this.checkRepeatingChargesRunAging.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkRepeatingChargesAutomated
			// 
			this.checkRepeatingChargesAutomated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkRepeatingChargesAutomated.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRepeatingChargesAutomated.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRepeatingChargesAutomated.Location = new System.Drawing.Point(125, 32);
			this.checkRepeatingChargesAutomated.Name = "checkRepeatingChargesAutomated";
			this.checkRepeatingChargesAutomated.Size = new System.Drawing.Size(226, 17);
			this.checkRepeatingChargesAutomated.TabIndex = 238;
			this.checkRepeatingChargesAutomated.Text = "Repeating charges run automatically";
			this.checkRepeatingChargesAutomated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRepeatingChargesAutomated.CheckedChanged += new System.EventHandler(this.checkRepeatingChargesAutomated_CheckedChanged);
			this.checkRepeatingChargesAutomated.Click += new System.EventHandler(this.PromptRecurringRepeatingChargesTimes);
			// 
			// groupRecurringCharges
			// 
			this.groupRecurringCharges.Controls.Add(this.checkRecurPatBal0);
			this.groupRecurringCharges.Controls.Add(this.label56);
			this.groupRecurringCharges.Controls.Add(this.comboRecurringChargePayType);
			this.groupRecurringCharges.Controls.Add(this.labelRecurringChargesAutomatedTime);
			this.groupRecurringCharges.Controls.Add(this.textRecurringChargesTime);
			this.groupRecurringCharges.Controls.Add(this.checkRecurringChargesAutomated);
			this.groupRecurringCharges.Controls.Add(this.checkRecurringChargesUseTransDate);
			this.groupRecurringCharges.Controls.Add(this.checkRecurChargPriProv);
			this.groupRecurringCharges.Location = new System.Drawing.Point(804, 358);
			this.groupRecurringCharges.Name = "groupRecurringCharges";
			this.groupRecurringCharges.Size = new System.Drawing.Size(357, 153);
			this.groupRecurringCharges.TabIndex = 244;
			this.groupRecurringCharges.TabStop = false;
			this.groupRecurringCharges.Text = "Recurring Charges";
			// 
			// checkRecurPatBal0
			// 
			this.checkRecurPatBal0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkRecurPatBal0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRecurPatBal0.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRecurPatBal0.Location = new System.Drawing.Point(96, 121);
			this.checkRecurPatBal0.Name = "checkRecurPatBal0";
			this.checkRecurPatBal0.Size = new System.Drawing.Size(255, 32);
			this.checkRecurPatBal0.TabIndex = 246;
			this.checkRecurPatBal0.Text = "Allow recurring charges to run in the absence of a patient balance";
			this.checkRecurPatBal0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRecurPatBal0.UseVisualStyleBackColor = true;
			// 
			// label56
			// 
			this.label56.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label56.BackColor = System.Drawing.SystemColors.Window;
			this.label56.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label56.Location = new System.Drawing.Point(93, 97);
			this.label56.Name = "label56";
			this.label56.Size = new System.Drawing.Size(90, 15);
			this.label56.TabIndex = 253;
			this.label56.Text = "Pay type for CC";
			this.label56.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboRecurringChargePayType
			// 
			this.comboRecurringChargePayType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboRecurringChargePayType.Location = new System.Drawing.Point(188, 94);
			this.comboRecurringChargePayType.Name = "comboRecurringChargePayType";
			this.comboRecurringChargePayType.Size = new System.Drawing.Size(163, 21);
			this.comboRecurringChargePayType.TabIndex = 252;
			// 
			// labelRecurringChargesAutomatedTime
			// 
			this.labelRecurringChargesAutomatedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelRecurringChargesAutomatedTime.BackColor = System.Drawing.SystemColors.Window;
			this.labelRecurringChargesAutomatedTime.Location = new System.Drawing.Point(122, 68);
			this.labelRecurringChargesAutomatedTime.Name = "labelRecurringChargesAutomatedTime";
			this.labelRecurringChargesAutomatedTime.Size = new System.Drawing.Size(159, 17);
			this.labelRecurringChargesAutomatedTime.TabIndex = 243;
			this.labelRecurringChargesAutomatedTime.Text = "Recurring charges run time";
			this.labelRecurringChargesAutomatedTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRecurringChargesTime
			// 
			this.textRecurringChargesTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textRecurringChargesTime.Location = new System.Drawing.Point(283, 69);
			this.textRecurringChargesTime.Name = "textRecurringChargesTime";
			this.textRecurringChargesTime.Size = new System.Drawing.Size(68, 20);
			this.textRecurringChargesTime.TabIndex = 241;
			this.textRecurringChargesTime.Leave += new System.EventHandler(this.PromptRecurringRepeatingChargesTimes);
			// 
			// checkRecurringChargesAutomated
			// 
			this.checkRecurringChargesAutomated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkRecurringChargesAutomated.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRecurringChargesAutomated.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRecurringChargesAutomated.Location = new System.Drawing.Point(125, 48);
			this.checkRecurringChargesAutomated.Name = "checkRecurringChargesAutomated";
			this.checkRecurringChargesAutomated.Size = new System.Drawing.Size(226, 17);
			this.checkRecurringChargesAutomated.TabIndex = 240;
			this.checkRecurringChargesAutomated.Text = "Recurring charges run automatically";
			this.checkRecurringChargesAutomated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRecurringChargesAutomated.CheckedChanged += new System.EventHandler(this.checkRecurringChargesAutomated_CheckedChanged);
			this.checkRecurringChargesAutomated.Click += new System.EventHandler(this.PromptRecurringRepeatingChargesTimes);
			// 
			// checkRecurringChargesUseTransDate
			// 
			this.checkRecurringChargesUseTransDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkRecurringChargesUseTransDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRecurringChargesUseTransDate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRecurringChargesUseTransDate.Location = new System.Drawing.Point(125, 30);
			this.checkRecurringChargesUseTransDate.Name = "checkRecurringChargesUseTransDate";
			this.checkRecurringChargesUseTransDate.Size = new System.Drawing.Size(226, 17);
			this.checkRecurringChargesUseTransDate.TabIndex = 239;
			this.checkRecurringChargesUseTransDate.Text = "Recurring charges use transaction date";
			this.checkRecurringChargesUseTransDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkRecurChargPriProv
			// 
			this.checkRecurChargPriProv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkRecurChargPriProv.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRecurChargPriProv.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRecurChargPriProv.Location = new System.Drawing.Point(125, 12);
			this.checkRecurChargPriProv.Name = "checkRecurChargPriProv";
			this.checkRecurChargPriProv.Size = new System.Drawing.Size(226, 17);
			this.checkRecurChargPriProv.TabIndex = 238;
			this.checkRecurChargPriProv.Text = "Recurring charges use primary provider";
			this.checkRecurChargPriProv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkBalancesDontSubtractIns
			// 
			this.checkBalancesDontSubtractIns.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBalancesDontSubtractIns.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBalancesDontSubtractIns.Location = new System.Drawing.Point(923, 8);
			this.checkBalancesDontSubtractIns.Name = "checkBalancesDontSubtractIns";
			this.checkBalancesDontSubtractIns.Size = new System.Drawing.Size(232, 17);
			this.checkBalancesDontSubtractIns.TabIndex = 55;
			this.checkBalancesDontSubtractIns.Text = "Balances don\'t subtract insurance estimate";
			this.checkBalancesDontSubtractIns.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAllowFutureTrans
			// 
			this.checkAllowFutureTrans.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowFutureTrans.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllowFutureTrans.Location = new System.Drawing.Point(867, 131);
			this.checkAllowFutureTrans.Name = "checkAllowFutureTrans";
			this.checkAllowFutureTrans.Size = new System.Drawing.Size(288, 17);
			this.checkAllowFutureTrans.TabIndex = 244;
			this.checkAllowFutureTrans.Text = "Allow future dated transactions";
			this.checkAllowFutureTrans.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPpoUseUcr
			// 
			this.checkPpoUseUcr.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPpoUseUcr.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPpoUseUcr.Location = new System.Drawing.Point(867, 77);
			this.checkPpoUseUcr.Name = "checkPpoUseUcr";
			this.checkPpoUseUcr.Size = new System.Drawing.Size(288, 17);
			this.checkPpoUseUcr.TabIndex = 228;
			this.checkPpoUseUcr.Text = "Use UCR fee for billed fee even if PPO fee is higher";
			this.checkPpoUseUcr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupCommLogs
			// 
			this.groupCommLogs.Controls.Add(this.checkCommLogAutoSave);
			this.groupCommLogs.Controls.Add(this.checkShowFamilyCommByDefault);
			this.groupCommLogs.Location = new System.Drawing.Point(804, 300);
			this.groupCommLogs.Name = "groupCommLogs";
			this.groupCommLogs.Size = new System.Drawing.Size(357, 54);
			this.groupCommLogs.TabIndex = 243;
			this.groupCommLogs.TabStop = false;
			this.groupCommLogs.Text = "Commlogs";
			// 
			// checkCommLogAutoSave
			// 
			this.checkCommLogAutoSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkCommLogAutoSave.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCommLogAutoSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkCommLogAutoSave.Location = new System.Drawing.Point(146, 12);
			this.checkCommLogAutoSave.Name = "checkCommLogAutoSave";
			this.checkCommLogAutoSave.Size = new System.Drawing.Size(205, 17);
			this.checkCommLogAutoSave.TabIndex = 225;
			this.checkCommLogAutoSave.Text = "Commlogs auto save";
			this.checkCommLogAutoSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCommLogAutoSave.UseVisualStyleBackColor = true;
			// 
			// checkShowFamilyCommByDefault
			// 
			this.checkShowFamilyCommByDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowFamilyCommByDefault.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowFamilyCommByDefault.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowFamilyCommByDefault.Location = new System.Drawing.Point(142, 30);
			this.checkShowFamilyCommByDefault.Name = "checkShowFamilyCommByDefault";
			this.checkShowFamilyCommByDefault.Size = new System.Drawing.Size(209, 17);
			this.checkShowFamilyCommByDefault.TabIndex = 75;
			this.checkShowFamilyCommByDefault.Text = "Show family commlog entries by default";
			this.checkShowFamilyCommByDefault.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowFamilyCommByDefault.Click += new System.EventHandler(this.CheckShowFamilyCommByDefault_Click);
			// 
			// checkAccountShowPaymentNums
			// 
			this.checkAccountShowPaymentNums.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAccountShowPaymentNums.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAccountShowPaymentNums.Location = new System.Drawing.Point(867, 59);
			this.checkAccountShowPaymentNums.Name = "checkAccountShowPaymentNums";
			this.checkAccountShowPaymentNums.Size = new System.Drawing.Size(288, 17);
			this.checkAccountShowPaymentNums.TabIndex = 194;
			this.checkAccountShowPaymentNums.Text = "Show payment numbers in Account Module";
			this.checkAccountShowPaymentNums.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowAllocateUnearnedPaymentPrompt
			// 
			this.checkShowAllocateUnearnedPaymentPrompt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowAllocateUnearnedPaymentPrompt.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowAllocateUnearnedPaymentPrompt.Location = new System.Drawing.Point(840, 113);
			this.checkShowAllocateUnearnedPaymentPrompt.Name = "checkShowAllocateUnearnedPaymentPrompt";
			this.checkShowAllocateUnearnedPaymentPrompt.Size = new System.Drawing.Size(315, 17);
			this.checkShowAllocateUnearnedPaymentPrompt.TabIndex = 242;
			this.checkShowAllocateUnearnedPaymentPrompt.Text = "Prompt user to allocate unearned income after creating a claim";
			this.checkShowAllocateUnearnedPaymentPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAgingMonthly
			// 
			this.checkAgingMonthly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAgingMonthly.Enabled = false;
			this.checkAgingMonthly.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAgingMonthly.Location = new System.Drawing.Point(867, 24);
			this.checkAgingMonthly.Name = "checkAgingMonthly";
			this.checkAgingMonthly.Size = new System.Drawing.Size(288, 35);
			this.checkAgingMonthly.TabIndex = 57;
			this.checkAgingMonthly.Text = "Aging calculated monthly instead of daily (not available with enterprise aging)";
			this.checkAgingMonthly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkStatementInvoiceGridShowWriteoffs
			// 
			this.checkStatementInvoiceGridShowWriteoffs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStatementInvoiceGridShowWriteoffs.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkStatementInvoiceGridShowWriteoffs.Location = new System.Drawing.Point(867, 95);
			this.checkStatementInvoiceGridShowWriteoffs.Name = "checkStatementInvoiceGridShowWriteoffs";
			this.checkStatementInvoiceGridShowWriteoffs.Size = new System.Drawing.Size(288, 17);
			this.checkStatementInvoiceGridShowWriteoffs.TabIndex = 238;
			this.checkStatementInvoiceGridShowWriteoffs.Text = "Invoices\' payments grid shows write-offs\r\n";
			this.checkStatementInvoiceGridShowWriteoffs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupPayPlans
			// 
			this.groupPayPlans.Controls.Add(this.label59);
			this.groupPayPlans.Controls.Add(this.textDynamicPayPlan);
			this.groupPayPlans.Controls.Add(this.label27);
			this.groupPayPlans.Controls.Add(this.comboPayPlansVersion);
			this.groupPayPlans.Controls.Add(this.checkHideDueNow);
			this.groupPayPlans.Controls.Add(this.checkPayPlansUseSheets);
			this.groupPayPlans.Controls.Add(this.checkPayPlansExcludePastActivity);
			this.groupPayPlans.Location = new System.Drawing.Point(804, 171);
			this.groupPayPlans.Name = "groupPayPlans";
			this.groupPayPlans.Size = new System.Drawing.Size(357, 125);
			this.groupPayPlans.TabIndex = 240;
			this.groupPayPlans.TabStop = false;
			this.groupPayPlans.Text = "Pay Plans";
			// 
			// label59
			// 
			this.label59.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label59.BackColor = System.Drawing.SystemColors.Window;
			this.label59.Location = new System.Drawing.Point(127, 98);
			this.label59.Name = "label59";
			this.label59.Size = new System.Drawing.Size(154, 17);
			this.label59.TabIndex = 245;
			this.label59.Text = "Dynamic Pay Plan run time";
			this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDynamicPayPlan
			// 
			this.textDynamicPayPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textDynamicPayPlan.Location = new System.Drawing.Point(283, 97);
			this.textDynamicPayPlan.Name = "textDynamicPayPlan";
			this.textDynamicPayPlan.Size = new System.Drawing.Size(68, 20);
			this.textDynamicPayPlan.TabIndex = 244;
			// 
			// label27
			// 
			this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label27.BackColor = System.Drawing.SystemColors.Window;
			this.label27.Location = new System.Drawing.Point(33, 54);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(123, 17);
			this.label27.TabIndex = 242;
			this.label27.Text = "Pay Plan charge logic:";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPayPlansVersion
			// 
			this.comboPayPlansVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboPayPlansVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPayPlansVersion.FormattingEnabled = true;
			this.comboPayPlansVersion.Location = new System.Drawing.Point(157, 51);
			this.comboPayPlansVersion.Name = "comboPayPlansVersion";
			this.comboPayPlansVersion.Size = new System.Drawing.Size(194, 21);
			this.comboPayPlansVersion.TabIndex = 241;
			this.comboPayPlansVersion.SelectionChangeCommitted += new System.EventHandler(this.comboPayPlansVersion_SelectionChangeCommitted);
			// 
			// checkHideDueNow
			// 
			this.checkHideDueNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkHideDueNow.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHideDueNow.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHideDueNow.Location = new System.Drawing.Point(33, 76);
			this.checkHideDueNow.Name = "checkHideDueNow";
			this.checkHideDueNow.Size = new System.Drawing.Size(318, 17);
			this.checkHideDueNow.TabIndex = 239;
			this.checkHideDueNow.Text = "Hide \"Due Now\" in Payment Plans Grid";
			this.checkHideDueNow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPayPlansUseSheets
			// 
			this.checkPayPlansUseSheets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkPayPlansUseSheets.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPayPlansUseSheets.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPayPlansUseSheets.Location = new System.Drawing.Point(33, 30);
			this.checkPayPlansUseSheets.Name = "checkPayPlansUseSheets";
			this.checkPayPlansUseSheets.Size = new System.Drawing.Size(318, 17);
			this.checkPayPlansUseSheets.TabIndex = 227;
			this.checkPayPlansUseSheets.Text = "Pay Plans use Sheets";
			this.checkPayPlansUseSheets.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPayPlansExcludePastActivity
			// 
			this.checkPayPlansExcludePastActivity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkPayPlansExcludePastActivity.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPayPlansExcludePastActivity.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPayPlansExcludePastActivity.Location = new System.Drawing.Point(33, 12);
			this.checkPayPlansExcludePastActivity.Name = "checkPayPlansExcludePastActivity";
			this.checkPayPlansExcludePastActivity.Size = new System.Drawing.Size(318, 17);
			this.checkPayPlansExcludePastActivity.TabIndex = 236;
			this.checkPayPlansExcludePastActivity.Text = "Payment Plans exclude past activity by default";
			this.checkPayPlansExcludePastActivity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabTreatPlan
			// 
			this.tabTreatPlan.BackColor = System.Drawing.SystemColors.Window;
			this.tabTreatPlan.Controls.Add(this.checkPromptSaveTP);
			this.tabTreatPlan.Controls.Add(this.labelDiscountPercentage);
			this.tabTreatPlan.Controls.Add(this.groupBox6);
			this.tabTreatPlan.Controls.Add(this.label19);
			this.tabTreatPlan.Controls.Add(this.groupInsHist);
			this.tabTreatPlan.Controls.Add(this.label1);
			this.tabTreatPlan.Controls.Add(this.checkFrequency);
			this.tabTreatPlan.Controls.Add(this.groupTreatPlanSort);
			this.tabTreatPlan.Controls.Add(this.textTreatNote);
			this.tabTreatPlan.Controls.Add(this.checkTPSaveSigned);
			this.tabTreatPlan.Controls.Add(this.comboProcDiscountType);
			this.tabTreatPlan.Controls.Add(this.checkTreatPlanShowCompleted);
			this.tabTreatPlan.Controls.Add(this.textDiscountPercentage);
			this.tabTreatPlan.Controls.Add(this.checkTreatPlanItemized);
			this.tabTreatPlan.Location = new System.Drawing.Point(4, 22);
			this.tabTreatPlan.Name = "tabTreatPlan";
			this.tabTreatPlan.Padding = new System.Windows.Forms.Padding(3);
			this.tabTreatPlan.Size = new System.Drawing.Size(1168, 641);
			this.tabTreatPlan.TabIndex = 3;
			this.tabTreatPlan.Text = "Treat\' Plan";
			// 
			// checkPromptSaveTP
			// 
			this.checkPromptSaveTP.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPromptSaveTP.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPromptSaveTP.Location = new System.Drawing.Point(122, 278);
			this.checkPromptSaveTP.Name = "checkPromptSaveTP";
			this.checkPromptSaveTP.Size = new System.Drawing.Size(302, 17);
			this.checkPromptSaveTP.TabIndex = 241;
			this.checkPromptSaveTP.Text = "Prompt to save Treatment Plans";
			this.checkPromptSaveTP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPromptSaveTP.UseVisualStyleBackColor = false;
			// 
			// labelDiscountPercentage
			// 
			this.labelDiscountPercentage.Location = new System.Drawing.Point(119, 158);
			this.labelDiscountPercentage.Margin = new System.Windows.Forms.Padding(0);
			this.labelDiscountPercentage.Name = "labelDiscountPercentage";
			this.labelDiscountPercentage.Size = new System.Drawing.Size(246, 16);
			this.labelDiscountPercentage.TabIndex = 240;
			this.labelDiscountPercentage.Text = "Procedure discount percentage";
			this.labelDiscountPercentage.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.textInsImplant);
			this.groupBox6.Controls.Add(this.label53);
			this.groupBox6.Controls.Add(this.label52);
			this.groupBox6.Controls.Add(this.textInsDentures);
			this.groupBox6.Controls.Add(this.label51);
			this.groupBox6.Controls.Add(this.textInsPerioMaint);
			this.groupBox6.Controls.Add(this.label50);
			this.groupBox6.Controls.Add(this.textInsDebridement);
			this.groupBox6.Controls.Add(this.label49);
			this.groupBox6.Controls.Add(this.textInsSealant);
			this.groupBox6.Controls.Add(this.label48);
			this.groupBox6.Controls.Add(this.textInsFlouride);
			this.groupBox6.Controls.Add(this.label47);
			this.groupBox6.Controls.Add(this.textInsCrown);
			this.groupBox6.Controls.Add(this.label46);
			this.groupBox6.Controls.Add(this.textInsSRP);
			this.groupBox6.Controls.Add(this.label45);
			this.groupBox6.Controls.Add(this.textInsCancerScreen);
			this.groupBox6.Controls.Add(this.label44);
			this.groupBox6.Controls.Add(this.textInsProphy);
			this.groupBox6.Controls.Add(this.label43);
			this.groupBox6.Controls.Add(this.textInsExam);
			this.groupBox6.Controls.Add(this.label35);
			this.groupBox6.Controls.Add(this.textInsBW);
			this.groupBox6.Controls.Add(this.label34);
			this.groupBox6.Controls.Add(this.textInsPano);
			this.groupBox6.Controls.Add(this.label36);
			this.groupBox6.Location = new System.Drawing.Point(497, 58);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(316, 346);
			this.groupBox6.TabIndex = 232;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Frequency Limitations";
			// 
			// textInsImplant
			// 
			this.textInsImplant.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsImplant.Location = new System.Drawing.Point(137, 313);
			this.textInsImplant.Name = "textInsImplant";
			this.textInsImplant.Size = new System.Drawing.Size(173, 20);
			this.textInsImplant.TabIndex = 27;
			// 
			// label53
			// 
			this.label53.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label53.Location = new System.Drawing.Point(3, 314);
			this.label53.Name = "label53";
			this.label53.Size = new System.Drawing.Size(135, 17);
			this.label53.TabIndex = 250;
			this.label53.Text = "Implant Codes";
			this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label52
			// 
			this.label52.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label52.Location = new System.Drawing.Point(68, 16);
			this.label52.Name = "label52";
			this.label52.Size = new System.Drawing.Size(246, 17);
			this.label52.TabIndex = 248;
			this.label52.Text = "(all codes should be comma separated)";
			this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsDentures
			// 
			this.textInsDentures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsDentures.Location = new System.Drawing.Point(137, 290);
			this.textInsDentures.Name = "textInsDentures";
			this.textInsDentures.Size = new System.Drawing.Size(173, 20);
			this.textInsDentures.TabIndex = 25;
			// 
			// label51
			// 
			this.label51.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label51.Location = new System.Drawing.Point(3, 291);
			this.label51.Name = "label51";
			this.label51.Size = new System.Drawing.Size(135, 17);
			this.label51.TabIndex = 247;
			this.label51.Text = "Dentures Codes";
			this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsPerioMaint
			// 
			this.textInsPerioMaint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsPerioMaint.Location = new System.Drawing.Point(137, 267);
			this.textInsPerioMaint.Name = "textInsPerioMaint";
			this.textInsPerioMaint.Size = new System.Drawing.Size(173, 20);
			this.textInsPerioMaint.TabIndex = 23;
			// 
			// label50
			// 
			this.label50.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label50.Location = new System.Drawing.Point(3, 268);
			this.label50.Name = "label50";
			this.label50.Size = new System.Drawing.Size(135, 17);
			this.label50.TabIndex = 245;
			this.label50.Text = "Perio Maintenance Codes";
			this.label50.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsDebridement
			// 
			this.textInsDebridement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsDebridement.Location = new System.Drawing.Point(137, 244);
			this.textInsDebridement.Name = "textInsDebridement";
			this.textInsDebridement.Size = new System.Drawing.Size(173, 20);
			this.textInsDebridement.TabIndex = 21;
			// 
			// label49
			// 
			this.label49.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label49.Location = new System.Drawing.Point(3, 245);
			this.label49.Name = "label49";
			this.label49.Size = new System.Drawing.Size(135, 17);
			this.label49.TabIndex = 243;
			this.label49.Text = "Full Debridement Codes";
			this.label49.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsSealant
			// 
			this.textInsSealant.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsSealant.Location = new System.Drawing.Point(137, 175);
			this.textInsSealant.Name = "textInsSealant";
			this.textInsSealant.Size = new System.Drawing.Size(173, 20);
			this.textInsSealant.TabIndex = 15;
			// 
			// label48
			// 
			this.label48.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label48.Location = new System.Drawing.Point(3, 176);
			this.label48.Name = "label48";
			this.label48.Size = new System.Drawing.Size(135, 17);
			this.label48.TabIndex = 241;
			this.label48.Text = "Sealant Codes";
			this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsFlouride
			// 
			this.textInsFlouride.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsFlouride.Location = new System.Drawing.Point(137, 152);
			this.textInsFlouride.Name = "textInsFlouride";
			this.textInsFlouride.Size = new System.Drawing.Size(173, 20);
			this.textInsFlouride.TabIndex = 13;
			// 
			// label47
			// 
			this.label47.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label47.Location = new System.Drawing.Point(3, 153);
			this.label47.Name = "label47";
			this.label47.Size = new System.Drawing.Size(135, 17);
			this.label47.TabIndex = 239;
			this.label47.Text = "Fluoride Codes";
			this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsCrown
			// 
			this.textInsCrown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsCrown.Location = new System.Drawing.Point(137, 198);
			this.textInsCrown.Name = "textInsCrown";
			this.textInsCrown.Size = new System.Drawing.Size(173, 20);
			this.textInsCrown.TabIndex = 17;
			// 
			// label46
			// 
			this.label46.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label46.Location = new System.Drawing.Point(3, 199);
			this.label46.Name = "label46";
			this.label46.Size = new System.Drawing.Size(135, 17);
			this.label46.TabIndex = 237;
			this.label46.Text = "Crown Codes";
			this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsSRP
			// 
			this.textInsSRP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsSRP.Location = new System.Drawing.Point(137, 221);
			this.textInsSRP.Name = "textInsSRP";
			this.textInsSRP.Size = new System.Drawing.Size(173, 20);
			this.textInsSRP.TabIndex = 19;
			// 
			// label45
			// 
			this.label45.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label45.Location = new System.Drawing.Point(3, 222);
			this.label45.Name = "label45";
			this.label45.Size = new System.Drawing.Size(135, 17);
			this.label45.TabIndex = 235;
			this.label45.Text = "SRP Codes";
			this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsCancerScreen
			// 
			this.textInsCancerScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsCancerScreen.Location = new System.Drawing.Point(137, 106);
			this.textInsCancerScreen.Name = "textInsCancerScreen";
			this.textInsCancerScreen.Size = new System.Drawing.Size(173, 20);
			this.textInsCancerScreen.TabIndex = 9;
			// 
			// label44
			// 
			this.label44.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label44.Location = new System.Drawing.Point(3, 107);
			this.label44.Name = "label44";
			this.label44.Size = new System.Drawing.Size(135, 17);
			this.label44.TabIndex = 233;
			this.label44.Text = "Cancer Screening Codes";
			this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsProphy
			// 
			this.textInsProphy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsProphy.Location = new System.Drawing.Point(137, 129);
			this.textInsProphy.Name = "textInsProphy";
			this.textInsProphy.Size = new System.Drawing.Size(173, 20);
			this.textInsProphy.TabIndex = 11;
			// 
			// label43
			// 
			this.label43.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label43.Location = new System.Drawing.Point(3, 130);
			this.label43.Name = "label43";
			this.label43.Size = new System.Drawing.Size(135, 17);
			this.label43.TabIndex = 231;
			this.label43.Text = "Prophylaxis Codes";
			this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsExam
			// 
			this.textInsExam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsExam.Location = new System.Drawing.Point(137, 83);
			this.textInsExam.Name = "textInsExam";
			this.textInsExam.Size = new System.Drawing.Size(173, 20);
			this.textInsExam.TabIndex = 7;
			// 
			// label35
			// 
			this.label35.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label35.Location = new System.Drawing.Point(2, 61);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(135, 17);
			this.label35.TabIndex = 229;
			this.label35.Text = "Pano/FMX Codes";
			this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsBW
			// 
			this.textInsBW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsBW.Location = new System.Drawing.Point(137, 37);
			this.textInsBW.Name = "textInsBW";
			this.textInsBW.Size = new System.Drawing.Size(173, 20);
			this.textInsBW.TabIndex = 3;
			// 
			// label34
			// 
			this.label34.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label34.Location = new System.Drawing.Point(3, 84);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(135, 17);
			this.label34.TabIndex = 228;
			this.label34.Text = "Exam Codes";
			this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsPano
			// 
			this.textInsPano.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsPano.Location = new System.Drawing.Point(137, 60);
			this.textInsPano.Name = "textInsPano";
			this.textInsPano.Size = new System.Drawing.Size(173, 20);
			this.textInsPano.TabIndex = 5;
			// 
			// label36
			// 
			this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label36.Location = new System.Drawing.Point(3, 38);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(135, 17);
			this.label36.TabIndex = 227;
			this.label36.Text = "Bitewing Codes";
			this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(38, 130);
			this.label19.Margin = new System.Windows.Forms.Padding(0);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(221, 15);
			this.label19.TabIndex = 239;
			this.label19.Text = "Procedure discount adj type";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupInsHist
			// 
			this.groupInsHist.Controls.Add(this.textInsHistProphy);
			this.groupInsHist.Controls.Add(this.labelInsHistProphy);
			this.groupInsHist.Controls.Add(this.textInsHistPerioLR);
			this.groupInsHist.Controls.Add(this.labelInsHistPerioLR);
			this.groupInsHist.Controls.Add(this.textInsHistPerioLL);
			this.groupInsHist.Controls.Add(this.labelInsHistPerioLL);
			this.groupInsHist.Controls.Add(this.textInsHistPerioUL);
			this.groupInsHist.Controls.Add(this.labelInsHistPerioUL);
			this.groupInsHist.Controls.Add(this.textInsHistPerioUR);
			this.groupInsHist.Controls.Add(this.labelInsHistPerioUR);
			this.groupInsHist.Controls.Add(this.textInsHistFMX);
			this.groupInsHist.Controls.Add(this.labelInsHistFMX);
			this.groupInsHist.Controls.Add(this.textInsHistPerioMaint);
			this.groupInsHist.Controls.Add(this.labelInsHistPerioMaint);
			this.groupInsHist.Controls.Add(this.textInsHistExam);
			this.groupInsHist.Controls.Add(this.labelInsHistDebridement);
			this.groupInsHist.Controls.Add(this.textInsHistBW);
			this.groupInsHist.Controls.Add(this.labelInsHistExam);
			this.groupInsHist.Controls.Add(this.textInsHistDebridement);
			this.groupInsHist.Controls.Add(this.labelInsHistBW);
			this.groupInsHist.Location = new System.Drawing.Point(876, 35);
			this.groupInsHist.Name = "groupInsHist";
			this.groupInsHist.Size = new System.Drawing.Size(256, 252);
			this.groupInsHist.TabIndex = 233;
			this.groupInsHist.TabStop = false;
			this.groupInsHist.Text = "Insurance History";
			// 
			// textInsHistProphy
			// 
			this.textInsHistProphy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsHistProphy.Location = new System.Drawing.Point(151, 88);
			this.textInsHistProphy.Name = "textInsHistProphy";
			this.textInsHistProphy.Size = new System.Drawing.Size(99, 20);
			this.textInsHistProphy.TabIndex = 21;
			// 
			// labelInsHistProphy
			// 
			this.labelInsHistProphy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInsHistProphy.Location = new System.Drawing.Point(7, 89);
			this.labelInsHistProphy.Name = "labelInsHistProphy";
			this.labelInsHistProphy.Size = new System.Drawing.Size(143, 18);
			this.labelInsHistProphy.TabIndex = 243;
			this.labelInsHistProphy.Text = "Prophylaxis Code";
			this.labelInsHistProphy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsHistPerioLR
			// 
			this.textInsHistPerioLR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsHistPerioLR.Location = new System.Drawing.Point(151, 180);
			this.textInsHistPerioLR.Name = "textInsHistPerioLR";
			this.textInsHistPerioLR.Size = new System.Drawing.Size(99, 20);
			this.textInsHistPerioLR.TabIndex = 15;
			// 
			// labelInsHistPerioLR
			// 
			this.labelInsHistPerioLR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInsHistPerioLR.Location = new System.Drawing.Point(7, 181);
			this.labelInsHistPerioLR.Name = "labelInsHistPerioLR";
			this.labelInsHistPerioLR.Size = new System.Drawing.Size(143, 18);
			this.labelInsHistPerioLR.TabIndex = 241;
			this.labelInsHistPerioLR.Text = "Perio Scaling LR Code";
			this.labelInsHistPerioLR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsHistPerioLL
			// 
			this.textInsHistPerioLL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsHistPerioLL.Location = new System.Drawing.Point(151, 157);
			this.textInsHistPerioLL.Name = "textInsHistPerioLL";
			this.textInsHistPerioLL.Size = new System.Drawing.Size(99, 20);
			this.textInsHistPerioLL.TabIndex = 13;
			// 
			// labelInsHistPerioLL
			// 
			this.labelInsHistPerioLL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInsHistPerioLL.Location = new System.Drawing.Point(7, 158);
			this.labelInsHistPerioLL.Name = "labelInsHistPerioLL";
			this.labelInsHistPerioLL.Size = new System.Drawing.Size(143, 18);
			this.labelInsHistPerioLL.TabIndex = 239;
			this.labelInsHistPerioLL.Text = "Perio Scaling LL Code";
			this.labelInsHistPerioLL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsHistPerioUL
			// 
			this.textInsHistPerioUL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsHistPerioUL.Location = new System.Drawing.Point(151, 203);
			this.textInsHistPerioUL.Name = "textInsHistPerioUL";
			this.textInsHistPerioUL.Size = new System.Drawing.Size(99, 20);
			this.textInsHistPerioUL.TabIndex = 17;
			// 
			// labelInsHistPerioUL
			// 
			this.labelInsHistPerioUL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInsHistPerioUL.Location = new System.Drawing.Point(7, 204);
			this.labelInsHistPerioUL.Name = "labelInsHistPerioUL";
			this.labelInsHistPerioUL.Size = new System.Drawing.Size(143, 18);
			this.labelInsHistPerioUL.TabIndex = 237;
			this.labelInsHistPerioUL.Text = "Perio Scaling UL Code";
			this.labelInsHistPerioUL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsHistPerioUR
			// 
			this.textInsHistPerioUR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsHistPerioUR.Location = new System.Drawing.Point(151, 226);
			this.textInsHistPerioUR.Name = "textInsHistPerioUR";
			this.textInsHistPerioUR.Size = new System.Drawing.Size(99, 20);
			this.textInsHistPerioUR.TabIndex = 19;
			// 
			// labelInsHistPerioUR
			// 
			this.labelInsHistPerioUR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInsHistPerioUR.Location = new System.Drawing.Point(7, 227);
			this.labelInsHistPerioUR.Name = "labelInsHistPerioUR";
			this.labelInsHistPerioUR.Size = new System.Drawing.Size(143, 18);
			this.labelInsHistPerioUR.TabIndex = 235;
			this.labelInsHistPerioUR.Text = "Perio Scaling UR Code";
			this.labelInsHistPerioUR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsHistFMX
			// 
			this.textInsHistFMX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsHistFMX.Location = new System.Drawing.Point(151, 42);
			this.textInsHistFMX.Name = "textInsHistFMX";
			this.textInsHistFMX.Size = new System.Drawing.Size(99, 20);
			this.textInsHistFMX.TabIndex = 9;
			// 
			// labelInsHistFMX
			// 
			this.labelInsHistFMX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInsHistFMX.Location = new System.Drawing.Point(7, 43);
			this.labelInsHistFMX.Name = "labelInsHistFMX";
			this.labelInsHistFMX.Size = new System.Drawing.Size(143, 18);
			this.labelInsHistFMX.TabIndex = 233;
			this.labelInsHistFMX.Text = "Pano/FMX Code";
			this.labelInsHistFMX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsHistPerioMaint
			// 
			this.textInsHistPerioMaint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsHistPerioMaint.Location = new System.Drawing.Point(151, 134);
			this.textInsHistPerioMaint.Name = "textInsHistPerioMaint";
			this.textInsHistPerioMaint.Size = new System.Drawing.Size(99, 20);
			this.textInsHistPerioMaint.TabIndex = 11;
			// 
			// labelInsHistPerioMaint
			// 
			this.labelInsHistPerioMaint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInsHistPerioMaint.Location = new System.Drawing.Point(7, 135);
			this.labelInsHistPerioMaint.Name = "labelInsHistPerioMaint";
			this.labelInsHistPerioMaint.Size = new System.Drawing.Size(143, 18);
			this.labelInsHistPerioMaint.TabIndex = 231;
			this.labelInsHistPerioMaint.Text = "Perio Maintenance Code";
			this.labelInsHistPerioMaint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsHistExam
			// 
			this.textInsHistExam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsHistExam.Location = new System.Drawing.Point(151, 65);
			this.textInsHistExam.Name = "textInsHistExam";
			this.textInsHistExam.Size = new System.Drawing.Size(99, 20);
			this.textInsHistExam.TabIndex = 7;
			// 
			// labelInsHistDebridement
			// 
			this.labelInsHistDebridement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInsHistDebridement.Location = new System.Drawing.Point(7, 112);
			this.labelInsHistDebridement.Name = "labelInsHistDebridement";
			this.labelInsHistDebridement.Size = new System.Drawing.Size(143, 18);
			this.labelInsHistDebridement.TabIndex = 229;
			this.labelInsHistDebridement.Text = "Full Debridement Code";
			this.labelInsHistDebridement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsHistBW
			// 
			this.textInsHistBW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsHistBW.Location = new System.Drawing.Point(151, 19);
			this.textInsHistBW.Name = "textInsHistBW";
			this.textInsHistBW.Size = new System.Drawing.Size(99, 20);
			this.textInsHistBW.TabIndex = 3;
			// 
			// labelInsHistExam
			// 
			this.labelInsHistExam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInsHistExam.Location = new System.Drawing.Point(7, 66);
			this.labelInsHistExam.Name = "labelInsHistExam";
			this.labelInsHistExam.Size = new System.Drawing.Size(143, 18);
			this.labelInsHistExam.TabIndex = 228;
			this.labelInsHistExam.Text = "Exam Code";
			this.labelInsHistExam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsHistDebridement
			// 
			this.textInsHistDebridement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textInsHistDebridement.Location = new System.Drawing.Point(151, 111);
			this.textInsHistDebridement.Name = "textInsHistDebridement";
			this.textInsHistDebridement.Size = new System.Drawing.Size(99, 20);
			this.textInsHistDebridement.TabIndex = 5;
			// 
			// labelInsHistBW
			// 
			this.labelInsHistBW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInsHistBW.Location = new System.Drawing.Point(7, 20);
			this.labelInsHistBW.Name = "labelInsHistBW";
			this.labelInsHistBW.Size = new System.Drawing.Size(143, 18);
			this.labelInsHistBW.TabIndex = 227;
			this.labelInsHistBW.Text = "Bitewing Code";
			this.labelInsHistBW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(11, 35);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 52);
			this.label1.TabIndex = 35;
			this.label1.Text = "Default Note";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkFrequency
			// 
			this.checkFrequency.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFrequency.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkFrequency.Location = new System.Drawing.Point(593, 35);
			this.checkFrequency.Name = "checkFrequency";
			this.checkFrequency.Size = new System.Drawing.Size(220, 17);
			this.checkFrequency.TabIndex = 1;
			this.checkFrequency.Text = "Enable Insurance Frequency Checking";
			this.checkFrequency.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFrequency.UseVisualStyleBackColor = false;
			this.checkFrequency.Click += new System.EventHandler(this.checkFrequency_Click);
			// 
			// groupTreatPlanSort
			// 
			this.groupTreatPlanSort.Controls.Add(this.radioTreatPlanSortTooth);
			this.groupTreatPlanSort.Controls.Add(this.radioTreatPlanSortOrder);
			this.groupTreatPlanSort.Location = new System.Drawing.Point(253, 217);
			this.groupTreatPlanSort.Name = "groupTreatPlanSort";
			this.groupTreatPlanSort.Size = new System.Drawing.Size(171, 55);
			this.groupTreatPlanSort.TabIndex = 218;
			this.groupTreatPlanSort.TabStop = false;
			this.groupTreatPlanSort.Text = "Sort procedures by";
			// 
			// radioTreatPlanSortTooth
			// 
			this.radioTreatPlanSortTooth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.radioTreatPlanSortTooth.Location = new System.Drawing.Point(8, 16);
			this.radioTreatPlanSortTooth.Name = "radioTreatPlanSortTooth";
			this.radioTreatPlanSortTooth.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioTreatPlanSortTooth.Size = new System.Drawing.Size(157, 15);
			this.radioTreatPlanSortTooth.TabIndex = 54;
			this.radioTreatPlanSortTooth.Text = "Tooth";
			this.radioTreatPlanSortTooth.UseVisualStyleBackColor = true;
			this.radioTreatPlanSortTooth.Click += new System.EventHandler(this.radioTreatPlanSortTooth_Click);
			// 
			// radioTreatPlanSortOrder
			// 
			this.radioTreatPlanSortOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.radioTreatPlanSortOrder.Checked = true;
			this.radioTreatPlanSortOrder.Location = new System.Drawing.Point(8, 33);
			this.radioTreatPlanSortOrder.Name = "radioTreatPlanSortOrder";
			this.radioTreatPlanSortOrder.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioTreatPlanSortOrder.Size = new System.Drawing.Size(157, 15);
			this.radioTreatPlanSortOrder.TabIndex = 53;
			this.radioTreatPlanSortOrder.TabStop = true;
			this.radioTreatPlanSortOrder.Text = "Order Entered";
			this.radioTreatPlanSortOrder.UseVisualStyleBackColor = true;
			this.radioTreatPlanSortOrder.Click += new System.EventHandler(this.radioTreatPlanSortOrder_Click);
			// 
			// textTreatNote
			// 
			this.textTreatNote.AcceptsTab = true;
			this.textTreatNote.BackColor = System.Drawing.SystemColors.Window;
			this.textTreatNote.DetectLinksEnabled = false;
			this.textTreatNote.DetectUrls = false;
			this.textTreatNote.Location = new System.Drawing.Point(61, 33);
			this.textTreatNote.MaxLength = 32767;
			this.textTreatNote.Name = "textTreatNote";
			this.textTreatNote.QuickPasteType = OpenDentBusiness.QuickPasteType.TreatPlan;
			this.textTreatNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textTreatNote.Size = new System.Drawing.Size(363, 66);
			this.textTreatNote.TabIndex = 215;
			this.textTreatNote.Text = "";
			// 
			// checkTPSaveSigned
			// 
			this.checkTPSaveSigned.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTPSaveSigned.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTPSaveSigned.Location = new System.Drawing.Point(122, 195);
			this.checkTPSaveSigned.Name = "checkTPSaveSigned";
			this.checkTPSaveSigned.Size = new System.Drawing.Size(302, 17);
			this.checkTPSaveSigned.TabIndex = 213;
			this.checkTPSaveSigned.Text = "Save signed Treatment Plans to PDF";
			this.checkTPSaveSigned.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTPSaveSigned.UseVisualStyleBackColor = false;
			// 
			// comboProcDiscountType
			// 
			this.comboProcDiscountType.Location = new System.Drawing.Point(261, 128);
			this.comboProcDiscountType.Name = "comboProcDiscountType";
			this.comboProcDiscountType.Size = new System.Drawing.Size(163, 21);
			this.comboProcDiscountType.TabIndex = 201;
			// 
			// checkTreatPlanShowCompleted
			// 
			this.checkTreatPlanShowCompleted.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTreatPlanShowCompleted.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTreatPlanShowCompleted.Location = new System.Drawing.Point(65, 105);
			this.checkTreatPlanShowCompleted.Name = "checkTreatPlanShowCompleted";
			this.checkTreatPlanShowCompleted.Size = new System.Drawing.Size(359, 17);
			this.checkTreatPlanShowCompleted.TabIndex = 47;
			this.checkTreatPlanShowCompleted.Text = "Show completed work on graphical tooth chart";
			this.checkTreatPlanShowCompleted.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDiscountPercentage
			// 
			this.textDiscountPercentage.Location = new System.Drawing.Point(371, 155);
			this.textDiscountPercentage.Name = "textDiscountPercentage";
			this.textDiscountPercentage.Size = new System.Drawing.Size(53, 20);
			this.textDiscountPercentage.TabIndex = 211;
			// 
			// checkTreatPlanItemized
			// 
			this.checkTreatPlanItemized.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTreatPlanItemized.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTreatPlanItemized.Location = new System.Drawing.Point(284, 178);
			this.checkTreatPlanItemized.Name = "checkTreatPlanItemized";
			this.checkTreatPlanItemized.Size = new System.Drawing.Size(140, 17);
			this.checkTreatPlanItemized.TabIndex = 212;
			this.checkTreatPlanItemized.Text = "Itemize Treatment Plan";
			this.checkTreatPlanItemized.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTreatPlanItemized.UseVisualStyleBackColor = false;
			this.checkTreatPlanItemized.Click += new System.EventHandler(this.checkTreatPlanItemized_Click);
			// 
			// tabChart
			// 
			this.tabChart.BackColor = System.Drawing.SystemColors.Window;
			this.tabChart.Controls.Add(this.checkShowPlannedApptPrompt);
			this.tabChart.Controls.Add(this.checkAllowSettingProcsComplete);
			this.tabChart.Controls.Add(this.checkIsAlertRadiologyProcsEnabled);
			this.tabChart.Controls.Add(this.comboToothNomenclature);
			this.tabChart.Controls.Add(this.textProblemsIndicateNone);
			this.tabChart.Controls.Add(this.label32);
			this.tabChart.Controls.Add(this.checkBoxRxClinicUseSelected);
			this.tabChart.Controls.Add(this.checkProcNoteConcurrencyMerge);
			this.tabChart.Controls.Add(this.label8);
			this.tabChart.Controls.Add(this.comboProcCodeListSort);
			this.tabChart.Controls.Add(this.checkProcProvChangesCp);
			this.tabChart.Controls.Add(this.labelToothNomenclature);
			this.tabChart.Controls.Add(this.comboProcFeeUpdatePrompt);
			this.tabChart.Controls.Add(this.checkPerioTreatImplantsAsNotMissing);
			this.tabChart.Controls.Add(this.labelProcFeeUpdatePrompt);
			this.tabChart.Controls.Add(this.checkAutoClearEntryStatus);
			this.tabChart.Controls.Add(this.butProblemsIndicateNone);
			this.tabChart.Controls.Add(this.checkPerioSkipMissingTeeth);
			this.tabChart.Controls.Add(this.label9);
			this.tabChart.Controls.Add(this.checkProcGroupNoteDoesAggregate);
			this.tabChart.Controls.Add(this.textMedicationsIndicateNone);
			this.tabChart.Controls.Add(this.checkProvColorChart);
			this.tabChart.Controls.Add(this.checkSignatureAllowDigital);
			this.tabChart.Controls.Add(this.textAllergiesIndicateNone);
			this.tabChart.Controls.Add(this.butMedicationsIndicateNone);
			this.tabChart.Controls.Add(this.textMedDefaultStopDays);
			this.tabChart.Controls.Add(this.checkClaimProcsAllowEstimatesOnCompl);
			this.tabChart.Controls.Add(this.label11);
			this.tabChart.Controls.Add(this.checkProcEditRequireAutoCode);
			this.tabChart.Controls.Add(this.checkChartNonPatientWarn);
			this.tabChart.Controls.Add(this.label14);
			this.tabChart.Controls.Add(this.checkProcLockingIsAllowed);
			this.tabChart.Controls.Add(this.checkProcsPromptForAutoNote);
			this.tabChart.Controls.Add(this.textICD9DefaultForNewProcs);
			this.tabChart.Controls.Add(this.labelIcdCodeDefault);
			this.tabChart.Controls.Add(this.checkScreeningsUseSheets);
			this.tabChart.Controls.Add(this.butDiagnosisCode);
			this.tabChart.Controls.Add(this.butAllergiesIndicateNone);
			this.tabChart.Controls.Add(this.checkDxIcdVersion);
			this.tabChart.Controls.Add(this.checkMedicalFeeUsedForNewProcs);
			this.tabChart.Location = new System.Drawing.Point(4, 22);
			this.tabChart.Name = "tabChart";
			this.tabChart.Padding = new System.Windows.Forms.Padding(3);
			this.tabChart.Size = new System.Drawing.Size(1168, 641);
			this.tabChart.TabIndex = 4;
			this.tabChart.Text = "Chart";
			// 
			// checkShowPlannedApptPrompt
			// 
			this.checkShowPlannedApptPrompt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowPlannedApptPrompt.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowPlannedApptPrompt.Location = new System.Drawing.Point(169, 450);
			this.checkShowPlannedApptPrompt.Name = "checkShowPlannedApptPrompt";
			this.checkShowPlannedApptPrompt.Size = new System.Drawing.Size(336, 17);
			this.checkShowPlannedApptPrompt.TabIndex = 230;
			this.checkShowPlannedApptPrompt.Text = "Prompt for Planned Appointment";
			this.checkShowPlannedApptPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowPlannedApptPrompt.UseVisualStyleBackColor = true;
			// 
			// checkAllowSettingProcsComplete
			// 
			this.checkAllowSettingProcsComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowSettingProcsComplete.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllowSettingProcsComplete.Location = new System.Drawing.Point(104, 44);
			this.checkAllowSettingProcsComplete.Name = "checkAllowSettingProcsComplete";
			this.checkAllowSettingProcsComplete.Size = new System.Drawing.Size(401, 17);
			this.checkAllowSettingProcsComplete.TabIndex = 74;
			this.checkAllowSettingProcsComplete.Text = "Allow setting procedures complete.  (It\'s better to only set appointments complet" +
    "e)";
			this.checkAllowSettingProcsComplete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowSettingProcsComplete.UseVisualStyleBackColor = true;
			// 
			// comboToothNomenclature
			// 
			this.comboToothNomenclature.FormattingEnabled = true;
			this.comboToothNomenclature.Location = new System.Drawing.Point(762, 67);
			this.comboToothNomenclature.Name = "comboToothNomenclature";
			this.comboToothNomenclature.Size = new System.Drawing.Size(255, 21);
			this.comboToothNomenclature.TabIndex = 195;
			// 
			// textProblemsIndicateNone
			// 
			this.textProblemsIndicateNone.Location = new System.Drawing.Point(334, 65);
			this.textProblemsIndicateNone.Name = "textProblemsIndicateNone";
			this.textProblemsIndicateNone.ReadOnly = true;
			this.textProblemsIndicateNone.Size = new System.Drawing.Size(146, 20);
			this.textProblemsIndicateNone.TabIndex = 198;
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(626, 171);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(134, 15);
			this.label32.TabIndex = 220;
			this.label32.Text = "Procedure Code List sort";
			this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkBoxRxClinicUseSelected
			// 
			this.checkBoxRxClinicUseSelected.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBoxRxClinicUseSelected.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxRxClinicUseSelected.Location = new System.Drawing.Point(93, 414);
			this.checkBoxRxClinicUseSelected.Name = "checkBoxRxClinicUseSelected";
			this.checkBoxRxClinicUseSelected.Size = new System.Drawing.Size(412, 17);
			this.checkBoxRxClinicUseSelected.TabIndex = 228;
			this.checkBoxRxClinicUseSelected.Text = "Rx use selected clinic from Clinics menu instead of selected patient\'s default cl" +
    "inic";
			this.checkBoxRxClinicUseSelected.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBoxRxClinicUseSelected.UseVisualStyleBackColor = true;
			// 
			// checkProcNoteConcurrencyMerge
			// 
			this.checkProcNoteConcurrencyMerge.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcNoteConcurrencyMerge.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcNoteConcurrencyMerge.Location = new System.Drawing.Point(673, 195);
			this.checkProcNoteConcurrencyMerge.Name = "checkProcNoteConcurrencyMerge";
			this.checkProcNoteConcurrencyMerge.Size = new System.Drawing.Size(342, 15);
			this.checkProcNoteConcurrencyMerge.TabIndex = 229;
			this.checkProcNoteConcurrencyMerge.Text = "Procedure notes merge together when concurrency issues occur";
			this.checkProcNoteConcurrencyMerge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcNoteConcurrencyMerge.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(123, 68);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(209, 15);
			this.label8.TabIndex = 197;
			this.label8.Text = "Indicator patient has no problems";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboProcCodeListSort
			// 
			this.comboProcCodeListSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProcCodeListSort.FormattingEnabled = true;
			this.comboProcCodeListSort.Location = new System.Drawing.Point(762, 168);
			this.comboProcCodeListSort.MaxDropDownItems = 30;
			this.comboProcCodeListSort.Name = "comboProcCodeListSort";
			this.comboProcCodeListSort.Size = new System.Drawing.Size(255, 21);
			this.comboProcCodeListSort.TabIndex = 219;
			// 
			// checkProcProvChangesCp
			// 
			this.checkProcProvChangesCp.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcProvChangesCp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcProvChangesCp.Location = new System.Drawing.Point(70, 396);
			this.checkProcProvChangesCp.Name = "checkProcProvChangesCp";
			this.checkProcProvChangesCp.Size = new System.Drawing.Size(435, 17);
			this.checkProcProvChangesCp.TabIndex = 227;
			this.checkProcProvChangesCp.Text = "Do not allow different procedure and claim procedure providers when attached to c" +
    "laim";
			this.checkProcProvChangesCp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcProvChangesCp.UseVisualStyleBackColor = true;
			// 
			// labelToothNomenclature
			// 
			this.labelToothNomenclature.Location = new System.Drawing.Point(648, 70);
			this.labelToothNomenclature.Name = "labelToothNomenclature";
			this.labelToothNomenclature.Size = new System.Drawing.Size(112, 15);
			this.labelToothNomenclature.TabIndex = 196;
			this.labelToothNomenclature.Text = "Tooth Nomenclature";
			this.labelToothNomenclature.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboProcFeeUpdatePrompt
			// 
			this.comboProcFeeUpdatePrompt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProcFeeUpdatePrompt.FormattingEnabled = true;
			this.comboProcFeeUpdatePrompt.Location = new System.Drawing.Point(286, 370);
			this.comboProcFeeUpdatePrompt.Name = "comboProcFeeUpdatePrompt";
			this.comboProcFeeUpdatePrompt.Size = new System.Drawing.Size(219, 21);
			this.comboProcFeeUpdatePrompt.TabIndex = 225;
			// 
			// checkPerioTreatImplantsAsNotMissing
			// 
			this.checkPerioTreatImplantsAsNotMissing.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPerioTreatImplantsAsNotMissing.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPerioTreatImplantsAsNotMissing.Location = new System.Drawing.Point(720, 148);
			this.checkPerioTreatImplantsAsNotMissing.Name = "checkPerioTreatImplantsAsNotMissing";
			this.checkPerioTreatImplantsAsNotMissing.Size = new System.Drawing.Size(295, 15);
			this.checkPerioTreatImplantsAsNotMissing.TabIndex = 216;
			this.checkPerioTreatImplantsAsNotMissing.Text = "Perio exams treat implants as not missing";
			this.checkPerioTreatImplantsAsNotMissing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPerioTreatImplantsAsNotMissing.UseVisualStyleBackColor = true;
			// 
			// labelProcFeeUpdatePrompt
			// 
			this.labelProcFeeUpdatePrompt.Location = new System.Drawing.Point(123, 373);
			this.labelProcFeeUpdatePrompt.Name = "labelProcFeeUpdatePrompt";
			this.labelProcFeeUpdatePrompt.Size = new System.Drawing.Size(161, 15);
			this.labelProcFeeUpdatePrompt.TabIndex = 226;
			this.labelProcFeeUpdatePrompt.Text = "Procedure fee update behavior";
			this.labelProcFeeUpdatePrompt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAutoClearEntryStatus
			// 
			this.checkAutoClearEntryStatus.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAutoClearEntryStatus.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAutoClearEntryStatus.Location = new System.Drawing.Point(720, 47);
			this.checkAutoClearEntryStatus.Name = "checkAutoClearEntryStatus";
			this.checkAutoClearEntryStatus.Size = new System.Drawing.Size(295, 15);
			this.checkAutoClearEntryStatus.TabIndex = 73;
			this.checkAutoClearEntryStatus.Text = "Reset entry status to \'TreatPlan\' when switching patients";
			this.checkAutoClearEntryStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAutoClearEntryStatus.UseVisualStyleBackColor = true;
			// 
			// butProblemsIndicateNone
			// 
			this.butProblemsIndicateNone.Location = new System.Drawing.Point(484, 65);
			this.butProblemsIndicateNone.Name = "butProblemsIndicateNone";
			this.butProblemsIndicateNone.Size = new System.Drawing.Size(21, 21);
			this.butProblemsIndicateNone.TabIndex = 199;
			this.butProblemsIndicateNone.Text = "...";
			this.butProblemsIndicateNone.Click += new System.EventHandler(this.butProblemsIndicateNone_Click);
			// 
			// checkPerioSkipMissingTeeth
			// 
			this.checkPerioSkipMissingTeeth.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPerioSkipMissingTeeth.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPerioSkipMissingTeeth.Location = new System.Drawing.Point(720, 130);
			this.checkPerioSkipMissingTeeth.Name = "checkPerioSkipMissingTeeth";
			this.checkPerioSkipMissingTeeth.Size = new System.Drawing.Size(295, 15);
			this.checkPerioSkipMissingTeeth.TabIndex = 215;
			this.checkPerioSkipMissingTeeth.Text = "Perio exams always skip missing teeth";
			this.checkPerioSkipMissingTeeth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPerioSkipMissingTeeth.UseVisualStyleBackColor = true;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(123, 93);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(209, 15);
			this.label9.TabIndex = 200;
			this.label9.Text = "Indicator patient has no medications";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkProcGroupNoteDoesAggregate
			// 
			this.checkProcGroupNoteDoesAggregate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcGroupNoteDoesAggregate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcGroupNoteDoesAggregate.Location = new System.Drawing.Point(720, 94);
			this.checkProcGroupNoteDoesAggregate.Name = "checkProcGroupNoteDoesAggregate";
			this.checkProcGroupNoteDoesAggregate.Size = new System.Drawing.Size(295, 15);
			this.checkProcGroupNoteDoesAggregate.TabIndex = 206;
			this.checkProcGroupNoteDoesAggregate.Text = "Procedure Group Notes aggregate";
			this.checkProcGroupNoteDoesAggregate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcGroupNoteDoesAggregate.UseVisualStyleBackColor = true;
			// 
			// textMedicationsIndicateNone
			// 
			this.textMedicationsIndicateNone.Location = new System.Drawing.Point(334, 90);
			this.textMedicationsIndicateNone.Name = "textMedicationsIndicateNone";
			this.textMedicationsIndicateNone.ReadOnly = true;
			this.textMedicationsIndicateNone.Size = new System.Drawing.Size(146, 20);
			this.textMedicationsIndicateNone.TabIndex = 201;
			// 
			// checkProvColorChart
			// 
			this.checkProvColorChart.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProvColorChart.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProvColorChart.Location = new System.Drawing.Point(720, 112);
			this.checkProvColorChart.Name = "checkProvColorChart";
			this.checkProvColorChart.Size = new System.Drawing.Size(295, 15);
			this.checkProvColorChart.TabIndex = 214;
			this.checkProvColorChart.Text = "Use provider color in chart";
			this.checkProvColorChart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProvColorChart.UseVisualStyleBackColor = true;
			// 
			// checkSignatureAllowDigital
			// 
			this.checkSignatureAllowDigital.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSignatureAllowDigital.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSignatureAllowDigital.Location = new System.Drawing.Point(169, 348);
			this.checkSignatureAllowDigital.Name = "checkSignatureAllowDigital";
			this.checkSignatureAllowDigital.Size = new System.Drawing.Size(336, 17);
			this.checkSignatureAllowDigital.TabIndex = 223;
			this.checkSignatureAllowDigital.Text = "Allow digital signatures";
			this.checkSignatureAllowDigital.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSignatureAllowDigital.UseVisualStyleBackColor = true;
			// 
			// textAllergiesIndicateNone
			// 
			this.textAllergiesIndicateNone.Location = new System.Drawing.Point(334, 115);
			this.textAllergiesIndicateNone.Name = "textAllergiesIndicateNone";
			this.textAllergiesIndicateNone.ReadOnly = true;
			this.textAllergiesIndicateNone.Size = new System.Drawing.Size(146, 20);
			this.textAllergiesIndicateNone.TabIndex = 204;
			// 
			// butMedicationsIndicateNone
			// 
			this.butMedicationsIndicateNone.Location = new System.Drawing.Point(484, 90);
			this.butMedicationsIndicateNone.Name = "butMedicationsIndicateNone";
			this.butMedicationsIndicateNone.Size = new System.Drawing.Size(21, 21);
			this.butMedicationsIndicateNone.TabIndex = 202;
			this.butMedicationsIndicateNone.Text = "...";
			this.butMedicationsIndicateNone.Click += new System.EventHandler(this.butMedicationsIndicateNone_Click);
			// 
			// textMedDefaultStopDays
			// 
			this.textMedDefaultStopDays.Location = new System.Drawing.Point(466, 243);
			this.textMedDefaultStopDays.Name = "textMedDefaultStopDays";
			this.textMedDefaultStopDays.Size = new System.Drawing.Size(39, 20);
			this.textMedDefaultStopDays.TabIndex = 212;
			// 
			// checkClaimProcsAllowEstimatesOnCompl
			// 
			this.checkClaimProcsAllowEstimatesOnCompl.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimProcsAllowEstimatesOnCompl.Checked = true;
			this.checkClaimProcsAllowEstimatesOnCompl.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkClaimProcsAllowEstimatesOnCompl.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimProcsAllowEstimatesOnCompl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.checkClaimProcsAllowEstimatesOnCompl.Location = new System.Drawing.Point(169, 320);
			this.checkClaimProcsAllowEstimatesOnCompl.Name = "checkClaimProcsAllowEstimatesOnCompl";
			this.checkClaimProcsAllowEstimatesOnCompl.Size = new System.Drawing.Size(336, 25);
			this.checkClaimProcsAllowEstimatesOnCompl.TabIndex = 222;
			this.checkClaimProcsAllowEstimatesOnCompl.Text = "Allow estimates to be created for backdated completed procedures\r\n(not recommende" +
    "d, see manual)";
			this.checkClaimProcsAllowEstimatesOnCompl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimProcsAllowEstimatesOnCompl.UseVisualStyleBackColor = true;
			this.checkClaimProcsAllowEstimatesOnCompl.CheckedChanged += new System.EventHandler(this.checkClaimProcsAllowEstimatesOnCompl_CheckedChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(104, 246);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(361, 15);
			this.label11.TabIndex = 213;
			this.label11.Text = "Medication order default days until stop date (0 for no automatic stop date)";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkProcEditRequireAutoCode
			// 
			this.checkProcEditRequireAutoCode.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcEditRequireAutoCode.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcEditRequireAutoCode.Location = new System.Drawing.Point(169, 302);
			this.checkProcEditRequireAutoCode.Name = "checkProcEditRequireAutoCode";
			this.checkProcEditRequireAutoCode.Size = new System.Drawing.Size(336, 17);
			this.checkProcEditRequireAutoCode.TabIndex = 221;
			this.checkProcEditRequireAutoCode.Text = "Require use of suggested auto codes";
			this.checkProcEditRequireAutoCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcEditRequireAutoCode.UseVisualStyleBackColor = true;
			// 
			// checkChartNonPatientWarn
			// 
			this.checkChartNonPatientWarn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkChartNonPatientWarn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkChartNonPatientWarn.Location = new System.Drawing.Point(169, 223);
			this.checkChartNonPatientWarn.Name = "checkChartNonPatientWarn";
			this.checkChartNonPatientWarn.Size = new System.Drawing.Size(336, 17);
			this.checkChartNonPatientWarn.TabIndex = 211;
			this.checkChartNonPatientWarn.Text = "Non-Patient warning";
			this.checkChartNonPatientWarn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkChartNonPatientWarn.UseVisualStyleBackColor = true;
			this.checkChartNonPatientWarn.Click += new System.EventHandler(this.checkChartNonPatientWarn_Click);
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(123, 118);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(209, 15);
			this.label14.TabIndex = 203;
			this.label14.Text = "Indicator patient has no allergies";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkProcLockingIsAllowed
			// 
			this.checkProcLockingIsAllowed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcLockingIsAllowed.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcLockingIsAllowed.Location = new System.Drawing.Point(169, 204);
			this.checkProcLockingIsAllowed.Name = "checkProcLockingIsAllowed";
			this.checkProcLockingIsAllowed.Size = new System.Drawing.Size(336, 17);
			this.checkProcLockingIsAllowed.TabIndex = 210;
			this.checkProcLockingIsAllowed.Text = "Procedure locking is allowed";
			this.checkProcLockingIsAllowed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcLockingIsAllowed.UseVisualStyleBackColor = true;
			this.checkProcLockingIsAllowed.Click += new System.EventHandler(this.checkProcLockingIsAllowed_Click);
			// 
			// checkProcsPromptForAutoNote
			// 
			this.checkProcsPromptForAutoNote.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcsPromptForAutoNote.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcsPromptForAutoNote.Location = new System.Drawing.Point(169, 284);
			this.checkProcsPromptForAutoNote.Name = "checkProcsPromptForAutoNote";
			this.checkProcsPromptForAutoNote.Size = new System.Drawing.Size(336, 17);
			this.checkProcsPromptForAutoNote.TabIndex = 218;
			this.checkProcsPromptForAutoNote.Text = "Procedures Prompt For Auto Note";
			this.checkProcsPromptForAutoNote.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcsPromptForAutoNote.UseVisualStyleBackColor = true;
			// 
			// textICD9DefaultForNewProcs
			// 
			this.textICD9DefaultForNewProcs.Location = new System.Drawing.Point(395, 178);
			this.textICD9DefaultForNewProcs.Name = "textICD9DefaultForNewProcs";
			this.textICD9DefaultForNewProcs.Size = new System.Drawing.Size(85, 20);
			this.textICD9DefaultForNewProcs.TabIndex = 209;
			// 
			// labelIcdCodeDefault
			// 
			this.labelIcdCodeDefault.Location = new System.Drawing.Point(6, 181);
			this.labelIcdCodeDefault.Name = "labelIcdCodeDefault";
			this.labelIcdCodeDefault.Size = new System.Drawing.Size(388, 15);
			this.labelIcdCodeDefault.TabIndex = 203;
			this.labelIcdCodeDefault.Text = "Default ICD-10 code for new procedures and when set complete";
			this.labelIcdCodeDefault.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkScreeningsUseSheets
			// 
			this.checkScreeningsUseSheets.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkScreeningsUseSheets.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkScreeningsUseSheets.Location = new System.Drawing.Point(169, 266);
			this.checkScreeningsUseSheets.Name = "checkScreeningsUseSheets";
			this.checkScreeningsUseSheets.Size = new System.Drawing.Size(336, 17);
			this.checkScreeningsUseSheets.TabIndex = 217;
			this.checkScreeningsUseSheets.Text = "Screenings use Sheets";
			this.checkScreeningsUseSheets.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkScreeningsUseSheets.UseVisualStyleBackColor = true;
			// 
			// butDiagnosisCode
			// 
			this.butDiagnosisCode.Location = new System.Drawing.Point(484, 178);
			this.butDiagnosisCode.Name = "butDiagnosisCode";
			this.butDiagnosisCode.Size = new System.Drawing.Size(21, 21);
			this.butDiagnosisCode.TabIndex = 213;
			this.butDiagnosisCode.Text = "...";
			this.butDiagnosisCode.Click += new System.EventHandler(this.butDiagnosisCode_Click);
			// 
			// butAllergiesIndicateNone
			// 
			this.butAllergiesIndicateNone.Location = new System.Drawing.Point(484, 115);
			this.butAllergiesIndicateNone.Name = "butAllergiesIndicateNone";
			this.butAllergiesIndicateNone.Size = new System.Drawing.Size(21, 21);
			this.butAllergiesIndicateNone.TabIndex = 205;
			this.butAllergiesIndicateNone.Text = "...";
			this.butAllergiesIndicateNone.Click += new System.EventHandler(this.butAllergiesIndicateNone_Click);
			// 
			// checkDxIcdVersion
			// 
			this.checkDxIcdVersion.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDxIcdVersion.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkDxIcdVersion.Location = new System.Drawing.Point(169, 159);
			this.checkDxIcdVersion.Name = "checkDxIcdVersion";
			this.checkDxIcdVersion.Size = new System.Drawing.Size(336, 17);
			this.checkDxIcdVersion.TabIndex = 212;
			this.checkDxIcdVersion.Text = "Use ICD-10 Diagnosis Codes (uncheck for ICD-9)";
			this.checkDxIcdVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDxIcdVersion.UseVisualStyleBackColor = true;
			this.checkDxIcdVersion.Click += new System.EventHandler(this.checkDxIcdVersion_Click);
			// 
			// checkMedicalFeeUsedForNewProcs
			// 
			this.checkMedicalFeeUsedForNewProcs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMedicalFeeUsedForNewProcs.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkMedicalFeeUsedForNewProcs.Location = new System.Drawing.Point(169, 141);
			this.checkMedicalFeeUsedForNewProcs.Name = "checkMedicalFeeUsedForNewProcs";
			this.checkMedicalFeeUsedForNewProcs.Size = new System.Drawing.Size(336, 17);
			this.checkMedicalFeeUsedForNewProcs.TabIndex = 208;
			this.checkMedicalFeeUsedForNewProcs.Text = "Use medical fee for new procedures";
			this.checkMedicalFeeUsedForNewProcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMedicalFeeUsedForNewProcs.UseVisualStyleBackColor = true;
			// 
			// tabImages
			// 
			this.tabImages.BackColor = System.Drawing.SystemColors.Window;
			this.tabImages.Controls.Add(this.groupBox3);
			this.tabImages.Location = new System.Drawing.Point(4, 22);
			this.tabImages.Name = "tabImages";
			this.tabImages.Padding = new System.Windows.Forms.Padding(3);
			this.tabImages.Size = new System.Drawing.Size(1168, 641);
			this.tabImages.TabIndex = 5;
			this.tabImages.Text = "Images";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.radioImagesModuleTreeIsPersistentPerUser);
			this.groupBox3.Controls.Add(this.radioImagesModuleTreeIsCollapsed);
			this.groupBox3.Controls.Add(this.radioImagesModuleTreeIsExpanded);
			this.groupBox3.Location = new System.Drawing.Point(52, 43);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(408, 76);
			this.groupBox3.TabIndex = 51;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Folder Expansion Preference";
			// 
			// radioImagesModuleTreeIsPersistentPerUser
			// 
			this.radioImagesModuleTreeIsPersistentPerUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.radioImagesModuleTreeIsPersistentPerUser.Location = new System.Drawing.Point(62, 51);
			this.radioImagesModuleTreeIsPersistentPerUser.Name = "radioImagesModuleTreeIsPersistentPerUser";
			this.radioImagesModuleTreeIsPersistentPerUser.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioImagesModuleTreeIsPersistentPerUser.Size = new System.Drawing.Size(337, 17);
			this.radioImagesModuleTreeIsPersistentPerUser.TabIndex = 54;
			this.radioImagesModuleTreeIsPersistentPerUser.TabStop = true;
			this.radioImagesModuleTreeIsPersistentPerUser.Text = "Document tree folders persistent expand/collapse per user";
			this.radioImagesModuleTreeIsPersistentPerUser.UseVisualStyleBackColor = true;
			// 
			// radioImagesModuleTreeIsCollapsed
			// 
			this.radioImagesModuleTreeIsCollapsed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.radioImagesModuleTreeIsCollapsed.Location = new System.Drawing.Point(62, 34);
			this.radioImagesModuleTreeIsCollapsed.Name = "radioImagesModuleTreeIsCollapsed";
			this.radioImagesModuleTreeIsCollapsed.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioImagesModuleTreeIsCollapsed.Size = new System.Drawing.Size(337, 17);
			this.radioImagesModuleTreeIsCollapsed.TabIndex = 53;
			this.radioImagesModuleTreeIsCollapsed.TabStop = true;
			this.radioImagesModuleTreeIsCollapsed.Text = "Document tree collapses when patient changes";
			this.radioImagesModuleTreeIsCollapsed.UseVisualStyleBackColor = true;
			// 
			// radioImagesModuleTreeIsExpanded
			// 
			this.radioImagesModuleTreeIsExpanded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.radioImagesModuleTreeIsExpanded.Location = new System.Drawing.Point(17, 17);
			this.radioImagesModuleTreeIsExpanded.Name = "radioImagesModuleTreeIsExpanded";
			this.radioImagesModuleTreeIsExpanded.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioImagesModuleTreeIsExpanded.Size = new System.Drawing.Size(382, 17);
			this.radioImagesModuleTreeIsExpanded.TabIndex = 0;
			this.radioImagesModuleTreeIsExpanded.TabStop = true;
			this.radioImagesModuleTreeIsExpanded.Text = "Expand the document tree each time the Images Module is visited";
			this.radioImagesModuleTreeIsExpanded.UseVisualStyleBackColor = true;
			// 
			// tabManage
			// 
			this.tabManage.BackColor = System.Drawing.SystemColors.Window;
			this.tabManage.Controls.Add(this.checkEraAllowTotalPayment);
			this.tabManage.Controls.Add(this.checkIncludeEraWOPercCoPay);
			this.tabManage.Controls.Add(this.checkClockEventAllowBreak);
			this.tabManage.Controls.Add(this.textClaimsReceivedDays);
			this.tabManage.Controls.Add(this.checkShowAutoDeposit);
			this.tabManage.Controls.Add(this.checkEraOneClaimPerPage);
			this.tabManage.Controls.Add(this.checkClaimPaymentBatchOnly);
			this.tabManage.Controls.Add(this.labelClaimsReceivedDays);
			this.tabManage.Controls.Add(this.checkScheduleProvEmpSelectAll);
			this.tabManage.Controls.Add(this.checkClaimsSendWindowValidateOnLoad);
			this.tabManage.Controls.Add(this.checkTimeCardADP);
			this.tabManage.Controls.Add(this.groupBox1);
			this.tabManage.Controls.Add(this.comboTimeCardOvertimeFirstDayOfWeek);
			this.tabManage.Controls.Add(this.label16);
			this.tabManage.Controls.Add(this.checkRxSendNewToQueue);
			this.tabManage.Location = new System.Drawing.Point(4, 22);
			this.tabManage.Name = "tabManage";
			this.tabManage.Padding = new System.Windows.Forms.Padding(3);
			this.tabManage.Size = new System.Drawing.Size(1168, 641);
			this.tabManage.TabIndex = 6;
			this.tabManage.Text = "Manage";
			// 
			// checkEraAllowTotalPayment
			// 
			this.checkEraAllowTotalPayment.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEraAllowTotalPayment.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEraAllowTotalPayment.Location = new System.Drawing.Point(89, 524);
			this.checkEraAllowTotalPayment.Name = "checkEraAllowTotalPayment";
			this.checkEraAllowTotalPayment.Size = new System.Drawing.Size(421, 17);
			this.checkEraAllowTotalPayment.TabIndex = 252;
			this.checkEraAllowTotalPayment.Text = "ERA allow total payments";
			this.checkEraAllowTotalPayment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIncludeEraWOPercCoPay
			// 
			this.checkIncludeEraWOPercCoPay.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeEraWOPercCoPay.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIncludeEraWOPercCoPay.Location = new System.Drawing.Point(73, 469);
			this.checkIncludeEraWOPercCoPay.Name = "checkIncludeEraWOPercCoPay";
			this.checkIncludeEraWOPercCoPay.Size = new System.Drawing.Size(437, 17);
			this.checkIncludeEraWOPercCoPay.TabIndex = 250;
			this.checkIncludeEraWOPercCoPay.Text = "ERA posts write-offs for Category Percentage and Medicaid/Flat Copay";
			this.checkIncludeEraWOPercCoPay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClockEventAllowBreak
			// 
			this.checkClockEventAllowBreak.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClockEventAllowBreak.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClockEventAllowBreak.Location = new System.Drawing.Point(89, 505);
			this.checkClockEventAllowBreak.Name = "checkClockEventAllowBreak";
			this.checkClockEventAllowBreak.Size = new System.Drawing.Size(421, 17);
			this.checkClockEventAllowBreak.TabIndex = 249;
			this.checkClockEventAllowBreak.Text = "Allow paid 30 minute breaks";
			this.checkClockEventAllowBreak.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textClaimsReceivedDays
			// 
			this.textClaimsReceivedDays.Location = new System.Drawing.Point(450, 411);
			this.textClaimsReceivedDays.MaxVal = 999999;
			this.textClaimsReceivedDays.MinVal = 1;
			this.textClaimsReceivedDays.Name = "textClaimsReceivedDays";
			this.textClaimsReceivedDays.Size = new System.Drawing.Size(60, 20);
			this.textClaimsReceivedDays.TabIndex = 248;
			this.textClaimsReceivedDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// checkShowAutoDeposit
			// 
			this.checkShowAutoDeposit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowAutoDeposit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowAutoDeposit.Location = new System.Drawing.Point(88, 487);
			this.checkShowAutoDeposit.Name = "checkShowAutoDeposit";
			this.checkShowAutoDeposit.Size = new System.Drawing.Size(422, 17);
			this.checkShowAutoDeposit.TabIndex = 246;
			this.checkShowAutoDeposit.Text = "Insurance payments show auto deposit";
			this.checkShowAutoDeposit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkEraOneClaimPerPage
			// 
			this.checkEraOneClaimPerPage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEraOneClaimPerPage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEraOneClaimPerPage.Location = new System.Drawing.Point(89, 451);
			this.checkEraOneClaimPerPage.Name = "checkEraOneClaimPerPage";
			this.checkEraOneClaimPerPage.Size = new System.Drawing.Size(421, 17);
			this.checkEraOneClaimPerPage.TabIndex = 206;
			this.checkEraOneClaimPerPage.Text = "ERA prints one page per claim";
			this.checkEraOneClaimPerPage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClaimPaymentBatchOnly
			// 
			this.checkClaimPaymentBatchOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimPaymentBatchOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimPaymentBatchOnly.Location = new System.Drawing.Point(89, 434);
			this.checkClaimPaymentBatchOnly.Name = "checkClaimPaymentBatchOnly";
			this.checkClaimPaymentBatchOnly.Size = new System.Drawing.Size(421, 17);
			this.checkClaimPaymentBatchOnly.TabIndex = 205;
			this.checkClaimPaymentBatchOnly.Text = "Finalize claim payments in Batch Insurance window only";
			this.checkClaimPaymentBatchOnly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelClaimsReceivedDays
			// 
			this.labelClaimsReceivedDays.Location = new System.Drawing.Point(88, 411);
			this.labelClaimsReceivedDays.MaximumSize = new System.Drawing.Size(1000, 300);
			this.labelClaimsReceivedDays.Name = "labelClaimsReceivedDays";
			this.labelClaimsReceivedDays.Size = new System.Drawing.Size(361, 20);
			this.labelClaimsReceivedDays.TabIndex = 203;
			this.labelClaimsReceivedDays.Text = "Show claims received after days (blank to disable)";
			this.labelClaimsReceivedDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkScheduleProvEmpSelectAll
			// 
			this.checkScheduleProvEmpSelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkScheduleProvEmpSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkScheduleProvEmpSelectAll.Location = new System.Drawing.Point(90, 129);
			this.checkScheduleProvEmpSelectAll.Name = "checkScheduleProvEmpSelectAll";
			this.checkScheduleProvEmpSelectAll.Size = new System.Drawing.Size(421, 17);
			this.checkScheduleProvEmpSelectAll.TabIndex = 202;
			this.checkScheduleProvEmpSelectAll.Text = "Select all provider/employees when loading schedules";
			this.checkScheduleProvEmpSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClaimsSendWindowValidateOnLoad
			// 
			this.checkClaimsSendWindowValidateOnLoad.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimsSendWindowValidateOnLoad.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimsSendWindowValidateOnLoad.Location = new System.Drawing.Point(90, 112);
			this.checkClaimsSendWindowValidateOnLoad.Name = "checkClaimsSendWindowValidateOnLoad";
			this.checkClaimsSendWindowValidateOnLoad.Size = new System.Drawing.Size(421, 17);
			this.checkClaimsSendWindowValidateOnLoad.TabIndex = 199;
			this.checkClaimsSendWindowValidateOnLoad.Text = "Claims Send window validate on load (can cause slowness)";
			this.checkClaimsSendWindowValidateOnLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkTimeCardADP
			// 
			this.checkTimeCardADP.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTimeCardADP.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTimeCardADP.Location = new System.Drawing.Point(152, 95);
			this.checkTimeCardADP.Name = "checkTimeCardADP";
			this.checkTimeCardADP.Size = new System.Drawing.Size(359, 17);
			this.checkTimeCardADP.TabIndex = 198;
			this.checkTimeCardADP.Text = "ADP export includes employee name";
			this.checkTimeCardADP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkStatementsAlphabetically);
			this.groupBox1.Controls.Add(this.checkBillingShowProgress);
			this.groupBox1.Controls.Add(this.label24);
			this.groupBox1.Controls.Add(this.textBillingElectBatchMax);
			this.groupBox1.Controls.Add(this.checkStatementShowAdjNotes);
			this.groupBox1.Controls.Add(this.checkIntermingleDefault);
			this.groupBox1.Controls.Add(this.checkStatementShowReturnAddress);
			this.groupBox1.Controls.Add(this.checkStatementShowProcBreakdown);
			this.groupBox1.Controls.Add(this.checkStatementShowNotes);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.comboUseChartNum);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label18);
			this.groupBox1.Controls.Add(this.textStatementsCalcDueDate);
			this.groupBox1.Controls.Add(this.textPayPlansBillInAdvanceDays);
			this.groupBox1.Location = new System.Drawing.Point(108, 145);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(413, 261);
			this.groupBox1.TabIndex = 197;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Billing and Statements";
			// 
			// checkStatementsAlphabetically
			// 
			this.checkStatementsAlphabetically.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStatementsAlphabetically.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkStatementsAlphabetically.Location = new System.Drawing.Point(25, 239);
			this.checkStatementsAlphabetically.Name = "checkStatementsAlphabetically";
			this.checkStatementsAlphabetically.Size = new System.Drawing.Size(377, 16);
			this.checkStatementsAlphabetically.TabIndex = 219;
			this.checkStatementsAlphabetically.Text = "Print statements alphabetically";
			this.checkStatementsAlphabetically.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkBillingShowProgress
			// 
			this.checkBillingShowProgress.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBillingShowProgress.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBillingShowProgress.Location = new System.Drawing.Point(25, 220);
			this.checkBillingShowProgress.Name = "checkBillingShowProgress";
			this.checkBillingShowProgress.Size = new System.Drawing.Size(377, 16);
			this.checkBillingShowProgress.TabIndex = 218;
			this.checkBillingShowProgress.Text = "Show progress when sending statements";
			this.checkBillingShowProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(25, 193);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(316, 20);
			this.label24.TabIndex = 217;
			this.label24.Text = "Max number of statements per batch (0 for no limit)";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBillingElectBatchMax
			// 
			this.textBillingElectBatchMax.Location = new System.Drawing.Point(342, 194);
			this.textBillingElectBatchMax.MaxVal = 255;
			this.textBillingElectBatchMax.MinVal = 0;
			this.textBillingElectBatchMax.Name = "textBillingElectBatchMax";
			this.textBillingElectBatchMax.Size = new System.Drawing.Size(60, 20);
			this.textBillingElectBatchMax.TabIndex = 216;
			this.textBillingElectBatchMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// checkStatementShowAdjNotes
			// 
			this.checkStatementShowAdjNotes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStatementShowAdjNotes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkStatementShowAdjNotes.Location = new System.Drawing.Point(34, 45);
			this.checkStatementShowAdjNotes.Name = "checkStatementShowAdjNotes";
			this.checkStatementShowAdjNotes.Size = new System.Drawing.Size(368, 17);
			this.checkStatementShowAdjNotes.TabIndex = 215;
			this.checkStatementShowAdjNotes.Text = "Show notes for adjustments";
			this.checkStatementShowAdjNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIntermingleDefault
			// 
			this.checkIntermingleDefault.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIntermingleDefault.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIntermingleDefault.Location = new System.Drawing.Point(25, 172);
			this.checkIntermingleDefault.Name = "checkIntermingleDefault";
			this.checkIntermingleDefault.Size = new System.Drawing.Size(377, 16);
			this.checkIntermingleDefault.TabIndex = 214;
			this.checkIntermingleDefault.Text = "Account Module statements default to intermingled mode";
			this.checkIntermingleDefault.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkStatementShowReturnAddress
			// 
			this.checkStatementShowReturnAddress.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStatementShowReturnAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkStatementShowReturnAddress.Location = new System.Drawing.Point(125, 11);
			this.checkStatementShowReturnAddress.Name = "checkStatementShowReturnAddress";
			this.checkStatementShowReturnAddress.Size = new System.Drawing.Size(277, 17);
			this.checkStatementShowReturnAddress.TabIndex = 206;
			this.checkStatementShowReturnAddress.Text = "Show return address";
			this.checkStatementShowReturnAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkStatementShowProcBreakdown
			// 
			this.checkStatementShowProcBreakdown.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStatementShowProcBreakdown.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkStatementShowProcBreakdown.Location = new System.Drawing.Point(34, 62);
			this.checkStatementShowProcBreakdown.Name = "checkStatementShowProcBreakdown";
			this.checkStatementShowProcBreakdown.Size = new System.Drawing.Size(368, 17);
			this.checkStatementShowProcBreakdown.TabIndex = 212;
			this.checkStatementShowProcBreakdown.Text = "Show procedure breakdown";
			this.checkStatementShowProcBreakdown.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkStatementShowNotes
			// 
			this.checkStatementShowNotes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStatementShowNotes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkStatementShowNotes.Location = new System.Drawing.Point(34, 28);
			this.checkStatementShowNotes.Name = "checkStatementShowNotes";
			this.checkStatementShowNotes.Size = new System.Drawing.Size(368, 17);
			this.checkStatementShowNotes.TabIndex = 211;
			this.checkStatementShowNotes.Text = "Show notes for payments";
			this.checkStatementShowNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(22, 109);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(318, 27);
			this.label2.TabIndex = 204;
			this.label2.Text = "Days to calculate due date (Usually 10 or 15.  Leave blank to show \"Due on Receip" +
    "t\")";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboUseChartNum
			// 
			this.comboUseChartNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUseChartNum.FormattingEnabled = true;
			this.comboUseChartNum.Location = new System.Drawing.Point(273, 82);
			this.comboUseChartNum.Name = "comboUseChartNum";
			this.comboUseChartNum.Size = new System.Drawing.Size(130, 21);
			this.comboUseChartNum.TabIndex = 207;
			// 
			// label10
			// 
			this.label10.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label10.Location = new System.Drawing.Point(76, 85);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(195, 15);
			this.label10.TabIndex = 208;
			this.label10.Text = "Account Numbers use";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label18
			// 
			this.label18.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label18.Location = new System.Drawing.Point(23, 141);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(318, 27);
			this.label18.TabIndex = 209;
			this.label18.Text = "Days in advance to bill payment plan amounts due\r\n(Usually 10 or 15)";
			this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textStatementsCalcDueDate
			// 
			this.textStatementsCalcDueDate.Location = new System.Drawing.Point(343, 113);
			this.textStatementsCalcDueDate.MaxVal = 255;
			this.textStatementsCalcDueDate.MinVal = 0;
			this.textStatementsCalcDueDate.Name = "textStatementsCalcDueDate";
			this.textStatementsCalcDueDate.Size = new System.Drawing.Size(60, 20);
			this.textStatementsCalcDueDate.TabIndex = 205;
			this.textStatementsCalcDueDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPayPlansBillInAdvanceDays
			// 
			this.textPayPlansBillInAdvanceDays.Location = new System.Drawing.Point(343, 145);
			this.textPayPlansBillInAdvanceDays.MaxVal = 255;
			this.textPayPlansBillInAdvanceDays.MinVal = 0;
			this.textPayPlansBillInAdvanceDays.Name = "textPayPlansBillInAdvanceDays";
			this.textPayPlansBillInAdvanceDays.Size = new System.Drawing.Size(60, 20);
			this.textPayPlansBillInAdvanceDays.TabIndex = 210;
			this.textPayPlansBillInAdvanceDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// comboTimeCardOvertimeFirstDayOfWeek
			// 
			this.comboTimeCardOvertimeFirstDayOfWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTimeCardOvertimeFirstDayOfWeek.FormattingEnabled = true;
			this.comboTimeCardOvertimeFirstDayOfWeek.Location = new System.Drawing.Point(340, 68);
			this.comboTimeCardOvertimeFirstDayOfWeek.Name = "comboTimeCardOvertimeFirstDayOfWeek";
			this.comboTimeCardOvertimeFirstDayOfWeek.Size = new System.Drawing.Size(170, 21);
			this.comboTimeCardOvertimeFirstDayOfWeek.TabIndex = 195;
			// 
			// label16
			// 
			this.label16.BackColor = System.Drawing.SystemColors.Window;
			this.label16.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label16.Location = new System.Drawing.Point(87, 72);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(248, 13);
			this.label16.TabIndex = 196;
			this.label16.Text = "Time Card first day of week for overtime";
			this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkRxSendNewToQueue
			// 
			this.checkRxSendNewToQueue.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRxSendNewToQueue.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRxSendNewToQueue.Location = new System.Drawing.Point(151, 45);
			this.checkRxSendNewToQueue.Name = "checkRxSendNewToQueue";
			this.checkRxSendNewToQueue.Size = new System.Drawing.Size(359, 17);
			this.checkRxSendNewToQueue.TabIndex = 47;
			this.checkRxSendNewToQueue.Text = "Send all new prescriptions to electronic queue";
			this.checkRxSendNewToQueue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(1098, 670);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(63, 24);
			this.butCancel.TabIndex = 8;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(1029, 670);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(63, 24);
			this.butOK.TabIndex = 7;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// FormModuleSetup
			// 
			this.ClientSize = new System.Drawing.Size(1171, 697);
			this.Controls.Add(this.tabControlMain);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormModuleSetup";
			this.ShowInTaskbar = false;
			this.Text = "Module Preferences";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormModuleSetup_FormClosing);
			this.Load += new System.EventHandler(this.FormModuleSetup_Load);
			this.tabControlMain.ResumeLayout(false);
			this.tabAppts.ResumeLayout(false);
			this.tabAppts.PerformLayout();
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.tabFamily.ResumeLayout(false);
			this.groupBoxClaimSnapshot.ResumeLayout(false);
			this.groupBoxClaimSnapshot.PerformLayout();
			this.groupBoxSuperFamily.ResumeLayout(false);
			this.tabAccount.ResumeLayout(false);
			this.groupBox10.ResumeLayout(false);
			this.groupBox10.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox9.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			this.groupBoxClaimIdPrefix.ResumeLayout(false);
			this.groupBoxClaimIdPrefix.PerformLayout();
			this.groupRepeatingCharges.ResumeLayout(false);
			this.groupRepeatingCharges.PerformLayout();
			this.groupRecurringCharges.ResumeLayout(false);
			this.groupRecurringCharges.PerformLayout();
			this.groupCommLogs.ResumeLayout(false);
			this.groupPayPlans.ResumeLayout(false);
			this.groupPayPlans.PerformLayout();
			this.tabTreatPlan.ResumeLayout(false);
			this.tabTreatPlan.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupInsHist.ResumeLayout(false);
			this.groupInsHist.PerformLayout();
			this.groupTreatPlanSort.ResumeLayout(false);
			this.tabChart.ResumeLayout(false);
			this.tabChart.PerformLayout();
			this.tabImages.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.tabManage.ResumeLayout(false);
			this.tabManage.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion
		private System.Windows.Forms.CheckBox checkTreatPlanShowCompleted;
		private System.Windows.Forms.CheckBox checkBalancesDontSubtractIns;
		private CheckBox checkAgingMonthly;
		private CheckBox checkAutoClearEntryStatus;
		private CheckBox checkShowFamilyCommByDefault;
		private CheckBox checkAllowSettingProcsComplete;
		private CheckBox checkImagesModuleTreeIsCollapsed;
		private CheckBox checkRxSendNewToQueue;
		private CheckBox checkProcGroupNoteDoesAggregate;
		private CheckBox checkMedicalFeeUsedForNewProcs;
		private CheckBox checkAccountShowPaymentNums;
		private CheckBox checkIntermingleDefault;
		private CheckBox checkStatementShowReturnAddress;
		private CheckBox checkStatementShowProcBreakdown;
		private CheckBox checkStatementShowNotes;
		private CheckBox checkStatementShowAdjNotes;
		private CheckBox checkProcLockingIsAllowed;
		private CheckBox checkTimeCardADP;
		private CheckBox checkChartNonPatientWarn;
		private CheckBox checkTreatPlanItemized;
		private CheckBox checkClaimsSendWindowValidateOnLoad;
		private CheckBox checkProvColorChart;
		private CheckBox checkApptModuleDefaultToWeek;
		private CheckBox checkPerioSkipMissingTeeth;
		private CheckBox checkPerioTreatImplantsAsNotMissing;
		private CheckBox checkScreeningsUseSheets;
		private CheckBox checkTPSaveSigned;
		private CheckBox checkDxIcdVersion;
		private CheckBox checkSuperFamSync;
		private CheckBox checkPayPlansUseSheets;
		private CheckBox checkPpoUseUcr;
		private CheckBox checkProcsPromptForAutoNote;
		private CheckBox checkSuperFamAddIns;
		private CheckBox checkBillingShowProgress;
		private CheckBox checkProcEditRequireAutoCode;
		private CheckBox checkClaimProcsAllowEstimatesOnCompl;
		private CheckBox checkApptsCheckFrequency;
		private CheckBox checkPayPlansExcludePastActivity;
		private CheckBox checkScheduleProvEmpSelectAll;
		private CheckBox checkSignatureAllowDigital;
		private CheckBox checkClaimPaymentBatchOnly;
		private CheckBox checkStatementInvoiceGridShowWriteoffs;
		private CheckBox checkProcProvChangesCp;
		private CheckBox checkSuperFamCloneCreate;
		private CheckBox checkHideDueNow;
		private CheckBox checkBoxRxClinicUseSelected;
		private RadioButton radioImagesModuleTreeIsExpanded;
		private RadioButton radioImagesModuleTreeIsCollapsed;
		private RadioButton radioImagesModuleTreeIsPersistentPerUser;
		private RadioButton radioTreatPlanSortTooth;
		private RadioButton radioTreatPlanSortOrder;
		private TabPage tabAppts;
		private TabPage tabFamily;
		private TabPage tabAccount;
		private TabPage tabTreatPlan;
		private TabPage tabChart;
		private TabPage tabImages;
		private TabPage tabManage;
		private System.Windows.Forms.Label label1;
		private Label label8;
		private Label label9;
		private Label label14;
		private Label label16;
		private Label label2;
		private Label label10;
		private Label label11;
		private Label label18;
		private Label label24;
		private Label label31;
		private Label label30;
		private Label label32;
		private Label label27;
		private Label labelIcdCodeDefault;
		private Label labelSuperFamSort;
		private Label labelProcFeeUpdatePrompt;
		private Label apptClickDelay;
		private Label labelClaimsReceivedDays;
		private ComboBox comboTimeCardOvertimeFirstDayOfWeek;
		private ComboBox comboUseChartNum;
		private UI.ComboBoxPlus comboProcDiscountType;
		private ComboBox comboSuperFamSort;
		private ComboBox comboClaimSnapshotTrigger;
		private ComboBox comboProcCodeListSort;
		private ComboBox comboDelay;
		private ComboBox comboPayPlansVersion;
		private ComboBox comboProcFeeUpdatePrompt;
		private TextBox textDiscountPercentage;
		private TextBox textMedicationsIndicateNone;
		private TextBox textAllergiesIndicateNone;
		private TextBox textMedDefaultStopDays;
		private TextBox textClaimSnapshotRunTime;
		private TextBox textICD9DefaultForNewProcs;
		private TextBox textProblemsIndicateNone;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private UI.Button butProblemsIndicateNone;
		private UI.Button butDiagnosisCode;
		private UI.Button butMedicationsIndicateNone;
		private UI.Button butAllergiesIndicateNone;
		private GroupBox groupBox1;
		private GroupBox groupBox3;
		private GroupBox groupPayPlans;
		private GroupBox groupTreatPlanSort;
		private TabControl tabControlMain;
		private ODtextBox textTreatNote;
		private ToolTip toolTip1;
		private ValidNumber textStatementsCalcDueDate;
		private ValidNum textPayPlansBillInAdvanceDays;
		private ValidNum textBillingElectBatchMax;
		private Label labelDiscountPercentage;
		private Label label19;
		private GroupBox groupBox6;
		private TextBox textInsImplant;
		private Label label53;
		private Label label52;
		private TextBox textInsDentures;
		private Label label51;
		private TextBox textInsPerioMaint;
		private Label label50;
		private TextBox textInsDebridement;
		private Label label49;
		private TextBox textInsSealant;
		private Label label48;
		private TextBox textInsFlouride;
		private Label label47;
		private TextBox textInsCrown;
		private Label label46;
		private TextBox textInsSRP;
		private Label label45;
		private TextBox textInsCancerScreen;
		private Label label44;
		private TextBox textInsProphy;
		private Label label43;
		private TextBox textInsExam;
		private Label label35;
		private TextBox textInsBW;
		private Label label34;
		private TextBox textInsPano;
		private Label label36;
		private CheckBox checkFrequency;
		private CheckBox checkProcNoteConcurrencyMerge;
		private CheckBox checkSolidBlockouts;
		private CheckBox checkApptExclamation;
		private CheckBox checkApptBubbleDelay;
		private Label label23;
		private Label label25;
		private Button butColor;
		private Button butApptLineColor;
		private CheckBox checkAppointmentBubblesDisabled;
		private CheckBox checkApptRefreshEveryMinute;
		private CheckBox checkEraOneClaimPerPage;
		private CheckBox checkIsAlertRadiologyProcsEnabled;
		private CheckBox checkShowAllocateUnearnedPaymentPrompt;
		private ComboBox comboToothNomenclature;
		private Label labelToothNomenclature;
		private CheckBox checkShowPlannedApptPrompt;
		private GroupBox groupCommLogs;
		private CheckBox checkCommLogAutoSave;
		private CheckBox checkAllowFutureTrans;
		private CheckBox checkShowAutoDeposit;
		private ValidNumber textClaimsReceivedDays;
		private CheckBox checkClockEventAllowBreak;
		private GroupBox groupRecurringCharges;
		private Label labelRecurringChargesAutomatedTime;
		private ValidTime textRecurringChargesTime;
		private CheckBox checkRecurringChargesAutomated;
		private CheckBox checkRecurringChargesUseTransDate;
		private CheckBox checkRecurChargPriProv;
		private CheckBox checkIncludeEraWOPercCoPay;
		private CheckBox checkStatementsAlphabetically;
		private GroupBox groupInsHist;
		private TextBox textInsHistProphy;
		private Label labelInsHistProphy;
		private TextBox textInsHistPerioLR;
		private Label labelInsHistPerioLR;
		private TextBox textInsHistPerioLL;
		private Label labelInsHistPerioLL;
		private TextBox textInsHistPerioUL;
		private Label labelInsHistPerioUL;
		private TextBox textInsHistPerioUR;
		private Label labelInsHistPerioUR;
		private TextBox textInsHistFMX;
		private Label labelInsHistFMX;
		private TextBox textInsHistPerioMaint;
		private Label labelInsHistPerioMaint;
		private TextBox textInsHistExam;
		private Label labelInsHistDebridement;
		private TextBox textInsHistBW;
		private Label labelInsHistExam;
		private TextBox textInsHistDebridement;
		private Label labelInsHistBW;
		private GroupBox groupRepeatingCharges;
		private Label labelRepeatingChargesAutomatedTime;
		private ValidTime textRepeatingChargesAutomatedTime;
		private CheckBox checkRepeatingChargesRunAging;
		private CheckBox checkRepeatingChargesAutomated;
		private Label label56;
		private UI.ComboBoxPlus comboRecurringChargePayType;
		private CheckBox checkEraAllowTotalPayment;
		private CheckBox checkRecurPatBal0;
		private Label label54;
		private CheckBox checkApptsAllowOverlap;
		private CheckBox checkPreventChangesToComplAppts;
		private ValidNumber textApptAutoRefreshRange;
		private Label labelApptAutoRefreshRange;
		private CheckBox checkUnscheduledListNoRecalls;
		private CheckBox checkReplaceBlockouts;
		private Label labelApptSchedEnforceSpecialty;
		private ComboBox comboApptSchedEnforceSpecialty;
		private ValidNumber textApptWithoutProcsDefaultLength;
		private Label labelApptWithoutProcsDefaultLength;
		private CheckBox checkApptAllowEmptyComplete;
		private CheckBox checkApptAllowFutureComplete;
		private UI.ComboBoxPlus comboTimeArrived;
		private CheckBox checkApptsRequireProcs;
		private Label label3;
		private CheckBox checkApptModuleProductionUsesOps;
		private UI.ComboBoxPlus comboTimeSeated;
		private CheckBox checkUseOpHygProv;
		private Label label5;
		private CheckBox checkApptModuleAdjInProd;
		private UI.ComboBoxPlus comboTimeDismissed;
		private CheckBox checkApptTimeReset;
		private Label label6;
		private GroupBox groupBox2;
		private Label label37;
		private ComboBox comboBrokenApptProc;
		private CheckBox checkBrokenApptCommLog;
		private CheckBox checkBrokenApptAdjustment;
		private UI.ComboBoxPlus comboBrokenApptAdjType;
		private Label label7;
		private TextBox textWaitRoomWarn;
		private CheckBox checkAppointmentTimeIsLocked;
		private Label label22;
		private ComboBox comboSearchBehavior;
		private TextBox textApptBubNoteLength;
		private Label label13;
		private Label label21;
		private CheckBox checkWaitingRoomFilterByView;
		private Label label15;
		private CheckBox checkInsPlanExclusionsUseUCR;
		private CheckBox checkInsPlanExclusionsMarkDoNotBill;
		private CheckBox checkFixedBenefitBlankLikeZero;
		private CheckBox checkAllowPatsAtHQ;
		private CheckBox checkAutoFillPatEmail;
		private CheckBox checkPreferredReferrals;
		private CheckBox checkTextMsgOkStatusTreatAsNo;
		private CheckBox checkPatInitBillingTypeFromPriInsPlan;
		private CheckBox checkFamPhiAccess;
		private CheckBox checkClaimTrackingRequireError;
		private CheckBox checkPPOpercentage;
		private CheckBox checkInsurancePlansShared;
		private CheckBox checkClaimUseOverrideProcDescript;
		private CheckBox checkInsDefaultAssignmentOfBenefits;
		private CheckBox checkSelectProv;
		private ComboBox comboCobRule;
		private CheckBox checkAllowedFeeSchedsAutomate;
		private CheckBox checkGoogleAddress;
		private CheckBox checkInsPPOsecWriteoffs;
		private CheckBox checkInsDefaultShowUCRonClaims;
		private CheckBox checkCoPayFeeScheduleBlankLikeZero;
		private GroupBox groupBox5;
		private CheckBox checkAllowPrePayToTpProcs;
		private CheckBox checkIsRefundable;
		private Label label57;
		private UI.ComboBoxPlus comboTpUnearnedType;
		private CheckBox checkAllowPrepayProvider;
		private Label label42;
		private Label label41;
		private CheckBox checkHidePaysplits;
		private Label label40;
		private Label label39;
		private CheckBox checkAllowEmailCCReceipt;
		private Label label38;
		private CheckBox checkAllowFutureDebits;
		private CheckBox checkStoreCCTokens;
		private Label label26;
		private Label label33;
		private Label label12;
		private Label label4;
		private CheckBox checkPaymentsPromptForPayType;
		private Label label28;
		private GroupBox groupBox7;
		private CheckBox checkCanadianPpoLabEst;
		private CheckBox checkInsEstRecalcReceived;
		private CheckBox checkPromptForSecondaryClaim;
		private CheckBox checkInsPayNoWriteoffMoreThanProc;
		private CheckBox checkClaimTrackingExcludeNone;
		private Label label55;
		private ComboBox comboZeroDollarProcClaimBehavior;
		private Label labelClaimCredit;
		private ComboBox comboClaimCredit;
		private CheckBox checkAllowFuturePayments;
		private GroupBox groupBoxClaimIdPrefix;
		private UI.Button butReplacements;
		private TextBox textClaimIdentifier;
		private CheckBox checkAllowProcAdjFromClaim;
		private CheckBox checkProviderIncomeShows;
		private CheckBox checkClaimFormTreatDentSaysSigOnFile;
		private CheckBox checkClaimMedTypeIsInstWhenInsPlanIsMedical;
		private CheckBox checkEclaimsMedicalProvTreatmentAsOrdering;
		private CheckBox checkEclaimsSeparateTreatProv;
		private Label label20;
		private TextBox textClaimAttachPath;
		private CheckBox checkClaimsValidateACN;
		private TextBox textInsWriteoffDescript;
		private Label label17;
		private UI.ComboBoxPlus comboPayPlanAdj;
		private ComboBox comboRigorousAdjustments;
		private ComboBox comboAutoSplitPref;
		private ComboBox comboRigorousAccounting;
		private ComboBox comboPaymentClinicSetting;
		private TextBox textTaxPercent;
		private UI.ComboBoxPlus comboSalesTaxAdjType;
		private UI.ComboBoxPlus comboBillingChargeAdjType;
		private UI.ComboBoxPlus comboFinanceChargeAdjType;
		private GroupBox groupBox4;
		private ListBox listboxBadDebtAdjs;
		private Label label29;
		private UI.Button butBadDebt;
		private UI.ComboBoxPlus comboUnallocatedSplits;
		private GroupBox groupBoxSuperFamily;
		private GroupBox groupBoxClaimSnapshot;
		private TextBox textApptFontSize;
		private Label label58;
		private ValidNum textApptProvbarWidth;
		private GroupBox groupBox8;
		private CheckBox checkAgingProcLifo;
		private Label label59;
		private ValidTime textDynamicPayPlan;
		private GroupBox groupBox10;
		private GroupBox groupBox9;
		private CheckBox checkPatientSSNMasked;
		private CheckBox checkPatientDOBMasked;
		private CheckBox checkPaymentsTransferPatientIncomeOnly;
		private CheckBox checkBrokenApptRequiredOnMove;
		private CheckBox checkPromptSaveTP;
	}
}