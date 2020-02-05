/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using CodeBase;
using DentalXChange.Dps.Pos;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.Rendering.Printing;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using PayConnectService = OpenDentBusiness.PayConnectService;
using OpenDentBusiness.WebTypes.Shared.XWeb;
using PdfSharp.Pdf;

namespace OpenDental {
	///<summary></summary>
	public class FormPayment:ODForm {
		#region UI Elements
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textCheckNum;
		private System.Windows.Forms.TextBox textBankBranch;
		private IContainer components=null;
		private OpenDental.ValidDate textDate;
		private OpenDental.ValidDouble textAmount;
		private System.Windows.Forms.ListBox listPayType;
		private OpenDental.UI.Button butDeletePayment;
		private OpenDental.ODtextBox textNote;//(not including discounts)
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox textPaidBy;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.Label labelClinic;
		private OpenDental.ValidDate textDateEntry;
		private System.Windows.Forms.Label label12;
		private Label labelDepositAccount;
		private ComboBox comboDepositAccount;
		private Panel panelXcharge;
		private ContextMenu contextMenuXcharge;
		private MenuItem menuXcharge;
		private TextBox textDepositAccount;
		private TextBox textDeposit;
		private Label labelDeposit;
		private CheckBox checkPayTypeNone;
		private OpenDental.UI.Button butPayConnect;
		private ContextMenu contextMenuPayConnect;
		private MenuItem menuPayConnect;
		private UI.ComboBoxPlus comboCreditCards;
		private Label labelCreditCards;
		private CheckBox checkRecurring;
		private UI.Button butPrintReceipt;
		private GroupBox groupXWeb;
		private UI.Button butReturn;
		private UI.Button butVoid;
		private UI.Button butPrePay;
		private CheckBox checkProcessed;
		private UI.Button butEmailReceipt;
		private Label labelPayPlan;
		private TabControl tabControlSplits;
		private TabPage tabPageSplits;
		private TabPage tabPageAllocated;
		private ODGrid gridAllocated;
		private ODGrid gridSplits;
		private ODGrid gridCharges;
		private UI.Button butAddManual;
		private UI.Button butCreatePartial;
		private TextBox textSplitTotal;
		private Label label13;
		private CheckBox checkShowAll;
		private UI.Button butPay;
		private UI.Button butClear;
		private UI.Button butDelete;
		private ComboBox comboGroupBy;
		private Label label7;
		private TextBox textChargeTotal;
		private Label label8;
		private TextBox textFilterProcCodes;
		private Label labelProcCodes;
		private Label labelProvFilter;
		private Label labelClinicFilter;
		private ComboBoxMulti comboClinicFilter;
		private Label labelPatFilter;
		private ComboBoxMulti comboPatientFilter;
		private NumericUpDown amtMaxEnd;
		private Label labelMaxAmount;
		private NumericUpDown amtMinEnd;
		private UI.Button button1;
		private Label labelMinFilter;
		private Label labelTypeFilter;
		private ComboBoxMulti comboTypeFilter;
		private GroupBox groupBoxFiltering;
		private Label labelDateFrom;
		private DateTimePicker datePickTo;
		private DateTimePicker datePickFrom;
		private ComboBoxPlus comboProviderFilter;
		private CheckBox checkShowSuperfamily;
		private Label label9;
		private UI.Button butShowHide;
		private CheckBox checkIncludeExplicitCreditsOnly;
		private TabControl tabProcCharges;
		private TabPage tabPageCharges;
		private TabPage tabPageTreatPlan;
		#endregion
		///<summary></summary>
		public bool IsNew=false;
		///<summary>Set this value to a PaySplitNum if you want one of the splits highlighted when opening this form.</summary>
		public long InitialPaySplitNum;
		private Patient _patCur;
		private Payment _paymentCur;
		///<summary>A current list of splits showing on the left grid.</summary>
		private List<PaySplit> _listSplitsCur=new List<PaySplit>();
		///<summary>The original splits that existed when this window was opened.  Empty for new payments.</summary>
		private List<PaySplit> _listPaySplitsOld;
		//private double _splitTotal=0;
		private long[] _arrayDepositAcctNums;
		///<summary>This table gets created and filled once at the beginning.  After that, only the last column gets carefully updated.</summary>
		private DataTable _tableBalances;
		///<summary>Program X-Charge.</summary>
		private Program _xProg;
		///<summary>The local override path or normal path for X-Charge.</summary>
		private string _xPath;
		///<summary>Stored CreditCards for _patCur.</summary>
		private List<CreditCard> _listCreditCards;
		///<summary>Set to true when X-Charge or PayConnect makes a successful transaction, except for voids.</summary>
		private bool _wasCreditCardSuccessful;
		private PayConnectService.creditCardRequest _payConnectRequest;
		private PaySimple.ApiResponse _paySimpleResponse;
		private System.Drawing.Printing.PrintDocument _pd2;
		private Payment _paymentOld;
		private bool _promptSignature;
		private bool _printReceipt;
		///<summary>Local cache of all of the clinic nums the current user has permission to access at the time the form loads.  Filled at the same time
		///as comboClinic and is used to set payment.ClinicNum when saving.</summary>
		private List<long> _listUserClinicNums;
		private bool _isCCDeclined;
		///<summary>Set to a positive amount if there is an unearned amount for the patient and they want to use it.</summary>
		public double UnearnedAmt;
		///<summary>Used to track position inside the MakeXChargeTransaction(), for troubleshooting purposes.</summary>
		private string _xChargeMilestone;
		private List<PayPlan> _listValidPayPlans;
		private List<PaySplit> _listPaySplitAllocations;
		///<summary>List of current account charges for the family.  Gets filled from AutoSplitForPayment</summary>
		private List<AccountEntry> _listAccountCharges;
		///<summary>The amount entered for the current payment.  Amount currently available for paying off charges.
		///If this value is zero, it will be set to the summation of the split amounts when OK is clicked.</summary>
		public decimal AmtTotal;
		///<summary>A dictionary or patients that we may need to reference to fill the grids to eliminate unnecessary calls to the DB.
		///Should contain all patients in the current family along with any patients of payment plans of which a member of this family is the guarantor.</summary>
		private Dictionary<long,Patient> _dictPatients;
		///<summary>Set to true if this payment is supposed to be an income transfer.</summary>
		public bool IsIncomeTransfer;		
		///<summary>The XWebResponse that created this payment. Will only be set if the payment originated from XWeb..</summary>
		private XWebResponse _xWebResponse;
		///<summary>List of select procedures sent by the ContrAccount.If this list contains any procedures, they will be paid off first.</summary>
		//private List<Procedure> ListProcs;
		///<summary>When making income transfer splits, PaySplitAssociated objects are used.  It keeps track of split associations until we insert into
		///the database.  PaySplitOrig is the PaySplit the PaySplitLinked will "link to" with its FSplitNum.</summary>
		private List<PaySplits.PaySplitAssociated> _listPaySplitsAssociated;
		/// <summary>Direct family members of the current patient.</summary>
		private readonly Family _famCur;
		/// <summary>Superfamily of the current patient, if one exists.</summary>
		private Family _superFamCur;
		private List<Def> _listPaymentTypeDefs;
		private SplitContainer splitContainerCharges;
		private bool _isInit;
		private int _originalHeight;
		private bool _preferCurrentPat;
		private Panel butPaySimple;
		private ContextMenu contextMenuPaySimple;
		private MenuItem menuPaySimple;
		///<summary>Procedures and payplan charges from account module we want to make splits for on this payment.</summary>
		public List<AccountEntry> ListEntriesPayFirst;
		private ODGrid gridTreatPlan;
		private Label labelTPProcWarning;
		private bool _isPayConnectPortal;
		private PayConnectResponseWeb _payConnectResponseWeb;

		///<summary>Holds most all the data needed to load the form.</summary>
		private PaymentEdit.LoadData _loadData;

		///<summary>Returns either the family or super family of the current patients 
		///depending on whether or not the "Show Charges for Superfamily" checkbox is checked.</summary>
		private Family _curFamOrSuperFam {
			get {
				if(checkShowSuperfamily.Checked) {
					return _superFamCur;
				}
				else {
					return _famCur;
				}
			}
		}

		///<summary>The list of patnums in _currentFamily, either the superfamily or the regular family depending on the superfam checkbox state.</summary>
		private List<long> _listPatNums {
			get {
				return _curFamOrSuperFam.ListPats.Select(x => x.PatNum).ToList();
			}
		}

		public string XchargeMilestone {
			get {
				return _xChargeMilestone;
			}
		}
		
		/// <summary>List of selected patnums to filter outstanding charges grid on.</summary>
		private List<long> _listFilteredPatNums {
			get {				
				//Get filtered patients
				if(comboPatientFilter.ListSelectedIndices.Contains(0)) {//Contains "All"
					return comboPatientFilter.Items.Cast<ODBoxItem<Patient>>().Where(x => x.Tag!=null).Select(x => x.Tag.PatNum).ToList();
				}
				else {
					return comboPatientFilter.ListSelectedItems.Cast<ODBoxItem<Patient>>().Where(x => x.Tag!=null).Select(x => x.Tag.PatNum).ToList();
				}
			}
		}

		/// <summary>List of selected clinic nums to filter outstanding charges grid on.</summary>
		private List<long> _listFilteredClinics {
			get {
				//Get filtered clinics
				if(comboClinicFilter.ListSelectedIndices.Contains(0)) {//Contains "All"
					return comboClinicFilter.Items.Cast<ODBoxItem<Clinic>>().Where(x => x.Tag!=null).Select(x => x.Tag.ClinicNum).ToList();
				}
				else {
					return comboClinicFilter.ListSelectedItems.Cast<ODBoxItem<Clinic>>().Where(x => x.Tag!=null).Select(x => x.Tag.ClinicNum).ToList();
				}
			}
		}

		/// <summary>List of user-inputed proc codes to filter outstanding charges grid on.</summary>
		private List<long> _listFilteredProcCodes {
			get {
				List<long> listFilteredProcCodes=new List<long>();
				//Proc codes
				List<string> listCodes=textFilterProcCodes.Text.Split(new char[] { ',' }).ToList();
				foreach(string code in listCodes) {
					long retrievedCode=ProcedureCodes.GetCodeNum(code.Trim());  //returns 0 if code not found
					if(retrievedCode!=0) {
						listFilteredProcCodes.Add(retrievedCode);
					}
				}
				return listFilteredProcCodes;
			}
		}

		/// <summary>List of selected charge types to filter outstanding charges grid on: PaySplit, PayPlan Charge, Adjustment, Procedure.</summary>
		private List<string> _listFilteredType {
			get {
				List<string> listTypes=new List<string>();
				//Get filtered types
				if(comboTypeFilter.ListSelectedIndices.Contains(0)) {//'All' is selected
					for(int i=1;i<comboTypeFilter.Items.Count;i++) {//Starts at 1 to not include 'All'
						listTypes.Add(comboTypeFilter.Items[i].ToString());
					}
				}
				else {
					//Find all selected types
					for(int i=0;i<comboTypeFilter.ListSelectedItems.Count;i++) {
						listTypes.Add(comboTypeFilter.ListSelectedItems[i].ToString());
					}
				}
				return listTypes;
			}
		}

		///<summary>PatCur and FamCur are not for the PatCur of the payment.  They are for the patient and family from which this window was accessed.
		///Use listSelectedProcs to automatically attach payment to specific procedures.</summary>
		public FormPayment(Patient patCur,Family famCur,Payment paymentCur,bool preferCurrentPat) {
			InitializeComponent();// Required for Windows Form Designer support
			_patCur=patCur;
			_famCur=famCur;
			_paymentCur=paymentCur;
			_preferCurrentPat=preferCurrentPat;
			Lan.F(this);
			panelXcharge.ContextMenu=contextMenuXcharge;
			butPayConnect.ContextMenu=contextMenuPayConnect;
			butPaySimple.ContextMenu=contextMenuPaySimple;
			_paymentOld=paymentCur.Clone();
		}

		///<summary></summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPayment));
			this.contextMenuXcharge = new System.Windows.Forms.ContextMenu();
			this.menuXcharge = new System.Windows.Forms.MenuItem();
			this.contextMenuPayConnect = new System.Windows.Forms.ContextMenu();
			this.menuPayConnect = new System.Windows.Forms.MenuItem();
			this._pd2 = new System.Drawing.Printing.PrintDocument();
			this.label9 = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butDeletePayment = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butPrintReceipt = new OpenDental.UI.Button();
			this.butEmailReceipt = new OpenDental.UI.Button();
			this.splitContainerCharges = new System.Windows.Forms.SplitContainer();
			this.butPaySimple = new System.Windows.Forms.Panel();
			this.butShowHide = new OpenDental.UI.Button();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.textDeposit = new System.Windows.Forms.TextBox();
			this.butPayConnect = new OpenDental.UI.Button();
			this.labelDepositAccount = new System.Windows.Forms.Label();
			this.labelDeposit = new System.Windows.Forms.Label();
			this.textDepositAccount = new System.Windows.Forms.TextBox();
			this.comboCreditCards = new OpenDental.UI.ComboBoxPlus();
			this.label1 = new System.Windows.Forms.Label();
			this.panelXcharge = new System.Windows.Forms.Panel();
			this.labelCreditCards = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.comboDepositAccount = new System.Windows.Forms.ComboBox();
			this.checkRecurring = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textDateEntry = new OpenDental.ValidDate();
			this.butPrePay = new OpenDental.UI.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.checkProcessed = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupXWeb = new System.Windows.Forms.GroupBox();
			this.butReturn = new OpenDental.UI.Button();
			this.butVoid = new OpenDental.UI.Button();
			this.labelClinic = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.textPaidBy = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.listPayType = new System.Windows.Forms.ListBox();
			this.textNote = new OpenDental.ODtextBox();
			this.textAmount = new OpenDental.ValidDouble();
			this.textCheckNum = new System.Windows.Forms.TextBox();
			this.textDate = new OpenDental.ValidDate();
			this.textBankBranch = new System.Windows.Forms.TextBox();
			this.checkPayTypeNone = new System.Windows.Forms.CheckBox();
			this.tabProcCharges = new System.Windows.Forms.TabControl();
			this.tabPageCharges = new System.Windows.Forms.TabPage();
			this.checkShowSuperfamily = new System.Windows.Forms.CheckBox();
			this.gridCharges = new OpenDental.UI.ODGrid();
			this.checkIncludeExplicitCreditsOnly = new System.Windows.Forms.CheckBox();
			this.checkShowAll = new System.Windows.Forms.CheckBox();
			this.tabPageTreatPlan = new System.Windows.Forms.TabPage();
			this.labelTPProcWarning = new System.Windows.Forms.Label();
			this.gridTreatPlan = new OpenDental.UI.ODGrid();
			this.butPay = new OpenDental.UI.Button();
			this.labelPayPlan = new System.Windows.Forms.Label();
			this.tabControlSplits = new System.Windows.Forms.TabControl();
			this.tabPageSplits = new System.Windows.Forms.TabPage();
			this.gridSplits = new OpenDental.UI.ODGrid();
			this.tabPageAllocated = new System.Windows.Forms.TabPage();
			this.gridAllocated = new OpenDental.UI.ODGrid();
			this.textChargeTotal = new System.Windows.Forms.TextBox();
			this.butCreatePartial = new OpenDental.UI.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.comboGroupBy = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBoxFiltering = new System.Windows.Forms.GroupBox();
			this.comboProviderFilter = new OpenDental.UI.ComboBoxPlus();
			this.datePickTo = new System.Windows.Forms.DateTimePicker();
			this.labelDateFrom = new System.Windows.Forms.Label();
			this.datePickFrom = new System.Windows.Forms.DateTimePicker();
			this.textFilterProcCodes = new System.Windows.Forms.TextBox();
			this.labelMaxAmount = new System.Windows.Forms.Label();
			this.labelMinFilter = new System.Windows.Forms.Label();
			this.labelTypeFilter = new System.Windows.Forms.Label();
			this.amtMinEnd = new System.Windows.Forms.NumericUpDown();
			this.amtMaxEnd = new System.Windows.Forms.NumericUpDown();
			this.button1 = new OpenDental.UI.Button();
			this.comboPatientFilter = new OpenDental.UI.ComboBoxMulti();
			this.labelProcCodes = new System.Windows.Forms.Label();
			this.comboTypeFilter = new OpenDental.UI.ComboBoxMulti();
			this.comboClinicFilter = new OpenDental.UI.ComboBoxMulti();
			this.labelPatFilter = new System.Windows.Forms.Label();
			this.labelClinicFilter = new System.Windows.Forms.Label();
			this.labelProvFilter = new System.Windows.Forms.Label();
			this.butClear = new OpenDental.UI.Button();
			this.label13 = new System.Windows.Forms.Label();
			this.textSplitTotal = new System.Windows.Forms.TextBox();
			this.butAddManual = new OpenDental.UI.Button();
			this.contextMenuPaySimple = new System.Windows.Forms.ContextMenu();
			this.menuPaySimple = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerCharges)).BeginInit();
			this.splitContainerCharges.Panel1.SuspendLayout();
			this.splitContainerCharges.Panel2.SuspendLayout();
			this.splitContainerCharges.SuspendLayout();
			this.groupXWeb.SuspendLayout();
			this.tabProcCharges.SuspendLayout();
			this.tabPageCharges.SuspendLayout();
			this.tabPageTreatPlan.SuspendLayout();
			this.tabControlSplits.SuspendLayout();
			this.tabPageSplits.SuspendLayout();
			this.tabPageAllocated.SuspendLayout();
			this.groupBoxFiltering.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.amtMinEnd)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.amtMaxEnd)).BeginInit();
			this.SuspendLayout();
			// 
			// contextMenuXcharge
			// 
			this.contextMenuXcharge.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuXcharge});
			// 
			// menuXcharge
			// 
			this.menuXcharge.Index = 0;
			this.menuXcharge.Text = "Settings";
			this.menuXcharge.Click += new System.EventHandler(this.menuXcharge_Click);
			// 
			// contextMenuPayConnect
			// 
			this.contextMenuPayConnect.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPayConnect});
			// 
			// menuPayConnect
			// 
			this.menuPayConnect.Index = 0;
			this.menuPayConnect.Text = "Settings";
			this.menuPayConnect.Click += new System.EventHandler(this.menuPayConnect_Click);
			// 
			// label9
			// 
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label9.Location = new System.Drawing.Point(100, 658);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(165, 30);
			this.label9.TabIndex = 141;
			this.label9.Text = "Deletes entire payment \r\nand all splits";
			this.label9.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(943, 664);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 8;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butDeletePayment
			// 
			this.butDeletePayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDeletePayment.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDeletePayment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDeletePayment.Location = new System.Drawing.Point(11, 664);
			this.butDeletePayment.Name = "butDeletePayment";
			this.butDeletePayment.Size = new System.Drawing.Size(84, 24);
			this.butDeletePayment.TabIndex = 7;
			this.butDeletePayment.Text = "&Delete";
			this.butDeletePayment.Click += new System.EventHandler(this.butDeletePayment_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(1022, 664);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 9;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butPrintReceipt
			// 
			this.butPrintReceipt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrintReceipt.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrintReceipt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrintReceipt.Location = new System.Drawing.Point(389, 664);
			this.butPrintReceipt.Name = "butPrintReceipt";
			this.butPrintReceipt.Size = new System.Drawing.Size(101, 24);
			this.butPrintReceipt.TabIndex = 135;
			this.butPrintReceipt.TabStop = false;
			this.butPrintReceipt.Text = "&Print Receipt";
			this.butPrintReceipt.Visible = false;
			this.butPrintReceipt.Click += new System.EventHandler(this.butPrintReceipt_Click);
			// 
			// butEmailReceipt
			// 
			this.butEmailReceipt.AdjustImageLocation = new System.Drawing.Point(-3, 0);
			this.butEmailReceipt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEmailReceipt.Image = global::OpenDental.Properties.Resources.email1;
			this.butEmailReceipt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEmailReceipt.Location = new System.Drawing.Point(499, 664);
			this.butEmailReceipt.Name = "butEmailReceipt";
			this.butEmailReceipt.Size = new System.Drawing.Size(110, 24);
			this.butEmailReceipt.TabIndex = 140;
			this.butEmailReceipt.Text = "&E-Mail Receipt";
			this.butEmailReceipt.Visible = false;
			this.butEmailReceipt.Click += new System.EventHandler(this.butEmailReceipt_Click);
			// 
			// splitContainerCharges
			// 
			this.splitContainerCharges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainerCharges.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainerCharges.IsSplitterFixed = true;
			this.splitContainerCharges.Location = new System.Drawing.Point(0, 0);
			this.splitContainerCharges.Name = "splitContainerCharges";
			this.splitContainerCharges.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerCharges.Panel1
			// 
			this.splitContainerCharges.Panel1.Controls.Add(this.butPaySimple);
			this.splitContainerCharges.Panel1.Controls.Add(this.butShowHide);
			this.splitContainerCharges.Panel1.Controls.Add(this.comboClinic);
			this.splitContainerCharges.Panel1.Controls.Add(this.textDeposit);
			this.splitContainerCharges.Panel1.Controls.Add(this.butPayConnect);
			this.splitContainerCharges.Panel1.Controls.Add(this.labelDepositAccount);
			this.splitContainerCharges.Panel1.Controls.Add(this.labelDeposit);
			this.splitContainerCharges.Panel1.Controls.Add(this.textDepositAccount);
			this.splitContainerCharges.Panel1.Controls.Add(this.comboCreditCards);
			this.splitContainerCharges.Panel1.Controls.Add(this.label1);
			this.splitContainerCharges.Panel1.Controls.Add(this.panelXcharge);
			this.splitContainerCharges.Panel1.Controls.Add(this.labelCreditCards);
			this.splitContainerCharges.Panel1.Controls.Add(this.label2);
			this.splitContainerCharges.Panel1.Controls.Add(this.comboDepositAccount);
			this.splitContainerCharges.Panel1.Controls.Add(this.checkRecurring);
			this.splitContainerCharges.Panel1.Controls.Add(this.label3);
			this.splitContainerCharges.Panel1.Controls.Add(this.textDateEntry);
			this.splitContainerCharges.Panel1.Controls.Add(this.butPrePay);
			this.splitContainerCharges.Panel1.Controls.Add(this.label4);
			this.splitContainerCharges.Panel1.Controls.Add(this.label12);
			this.splitContainerCharges.Panel1.Controls.Add(this.checkProcessed);
			this.splitContainerCharges.Panel1.Controls.Add(this.label5);
			this.splitContainerCharges.Panel1.Controls.Add(this.groupXWeb);
			this.splitContainerCharges.Panel1.Controls.Add(this.labelClinic);
			this.splitContainerCharges.Panel1.Controls.Add(this.label6);
			this.splitContainerCharges.Panel1.Controls.Add(this.textPaidBy);
			this.splitContainerCharges.Panel1.Controls.Add(this.label11);
			this.splitContainerCharges.Panel1.Controls.Add(this.listPayType);
			this.splitContainerCharges.Panel1.Controls.Add(this.textNote);
			this.splitContainerCharges.Panel1.Controls.Add(this.textAmount);
			this.splitContainerCharges.Panel1.Controls.Add(this.textCheckNum);
			this.splitContainerCharges.Panel1.Controls.Add(this.textDate);
			this.splitContainerCharges.Panel1.Controls.Add(this.textBankBranch);
			this.splitContainerCharges.Panel1.Controls.Add(this.checkPayTypeNone);
			// 
			// splitContainerCharges.Panel2
			// 
			this.splitContainerCharges.Panel2.Controls.Add(this.tabProcCharges);
			this.splitContainerCharges.Panel2.Controls.Add(this.butPay);
			this.splitContainerCharges.Panel2.Controls.Add(this.labelPayPlan);
			this.splitContainerCharges.Panel2.Controls.Add(this.tabControlSplits);
			this.splitContainerCharges.Panel2.Controls.Add(this.textChargeTotal);
			this.splitContainerCharges.Panel2.Controls.Add(this.butCreatePartial);
			this.splitContainerCharges.Panel2.Controls.Add(this.label8);
			this.splitContainerCharges.Panel2.Controls.Add(this.butDelete);
			this.splitContainerCharges.Panel2.Controls.Add(this.comboGroupBy);
			this.splitContainerCharges.Panel2.Controls.Add(this.label7);
			this.splitContainerCharges.Panel2.Controls.Add(this.groupBoxFiltering);
			this.splitContainerCharges.Panel2.Controls.Add(this.butClear);
			this.splitContainerCharges.Panel2.Controls.Add(this.label13);
			this.splitContainerCharges.Panel2.Controls.Add(this.textSplitTotal);
			this.splitContainerCharges.Panel2.Controls.Add(this.butAddManual);
			this.splitContainerCharges.Size = new System.Drawing.Size(1111, 655);
			this.splitContainerCharges.SplitterDistance = 251;
			this.splitContainerCharges.SplitterWidth = 1;
			this.splitContainerCharges.TabIndex = 205;
			// 
			// butPaySimple
			// 
			this.butPaySimple.BackgroundImage = global::OpenDental.Properties.Resources.PaySimple_Button_2019_26x75;
			this.butPaySimple.Location = new System.Drawing.Point(979, 24);
			this.butPaySimple.Name = "butPaySimple";
			this.butPaySimple.Size = new System.Drawing.Size(76, 26);
			this.butPaySimple.TabIndex = 140;
			this.butPaySimple.MouseClick += new System.Windows.Forms.MouseEventHandler(this.butPaySimple_Click);
			// 
			// butShowHide
			// 
			this.butShowHide.Image = global::OpenDental.Properties.Resources.arrowDownTriangle;
			this.butShowHide.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butShowHide.Location = new System.Drawing.Point(3, 222);
			this.butShowHide.Name = "butShowHide";
			this.butShowHide.Size = new System.Drawing.Size(91, 24);
			this.butShowHide.TabIndex = 139;
			this.butShowHide.Text = "Hide Splits";
			this.butShowHide.UseVisualStyleBackColor = true;
			this.butShowHide.Click += new System.EventHandler(this.butShowHide_Click);
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(108, 19);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(198, 21);
			this.comboClinic.TabIndex = 92;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// textDeposit
			// 
			this.textDeposit.Location = new System.Drawing.Point(784, 165);
			this.textDeposit.Name = "textDeposit";
			this.textDeposit.ReadOnly = true;
			this.textDeposit.Size = new System.Drawing.Size(100, 20);
			this.textDeposit.TabIndex = 125;
			// 
			// butPayConnect
			// 
			this.butPayConnect.Location = new System.Drawing.Point(872, 24);
			this.butPayConnect.Name = "butPayConnect";
			this.butPayConnect.Size = new System.Drawing.Size(75, 24);
			this.butPayConnect.TabIndex = 129;
			this.butPayConnect.Text = "PayConnect";
			this.butPayConnect.Click += new System.EventHandler(this.butPayConnect_Click);
			// 
			// labelDepositAccount
			// 
			this.labelDepositAccount.Location = new System.Drawing.Point(496, 155);
			this.labelDepositAccount.Name = "labelDepositAccount";
			this.labelDepositAccount.Size = new System.Drawing.Size(260, 17);
			this.labelDepositAccount.TabIndex = 114;
			this.labelDepositAccount.Text = "Pay into Account";
			this.labelDepositAccount.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelDeposit
			// 
			this.labelDeposit.ForeColor = System.Drawing.Color.Firebrick;
			this.labelDeposit.Location = new System.Drawing.Point(781, 146);
			this.labelDeposit.Name = "labelDeposit";
			this.labelDeposit.Size = new System.Drawing.Size(199, 16);
			this.labelDeposit.TabIndex = 126;
			this.labelDeposit.Text = "Attached to deposit";
			this.labelDeposit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textDepositAccount
			// 
			this.textDepositAccount.Location = new System.Drawing.Point(496, 194);
			this.textDepositAccount.Name = "textDepositAccount";
			this.textDepositAccount.ReadOnly = true;
			this.textDepositAccount.Size = new System.Drawing.Size(260, 20);
			this.textDepositAccount.TabIndex = 119;
			// 
			// comboCreditCards
			// 
			this.comboCreditCards.Location = new System.Drawing.Point(784, 76);
			this.comboCreditCards.Name = "comboCreditCards";
			this.comboCreditCards.Size = new System.Drawing.Size(270, 21);
			this.comboCreditCards.TabIndex = 130;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(493, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(154, 16);
			this.label1.TabIndex = 7;
			this.label1.Text = "Payment Type";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// panelXcharge
			// 
			this.panelXcharge.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelXcharge.BackgroundImage")));
			this.panelXcharge.Location = new System.Drawing.Point(784, 23);
			this.panelXcharge.Name = "panelXcharge";
			this.panelXcharge.Size = new System.Drawing.Size(59, 26);
			this.panelXcharge.TabIndex = 118;
			this.panelXcharge.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelXcharge_MouseClick);
			// 
			// labelCreditCards
			// 
			this.labelCreditCards.Location = new System.Drawing.Point(784, 56);
			this.labelCreditCards.Name = "labelCreditCards";
			this.labelCreditCards.Size = new System.Drawing.Size(198, 17);
			this.labelCreditCards.TabIndex = 131;
			this.labelCreditCards.Text = "Credit Card";
			this.labelCreditCards.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(14, 163);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 16);
			this.label2.TabIndex = 8;
			this.label2.Text = "Note";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboDepositAccount
			// 
			this.comboDepositAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDepositAccount.FormattingEnabled = true;
			this.comboDepositAccount.Location = new System.Drawing.Point(496, 172);
			this.comboDepositAccount.Name = "comboDepositAccount";
			this.comboDepositAccount.Size = new System.Drawing.Size(260, 21);
			this.comboDepositAccount.TabIndex = 113;
			// 
			// checkRecurring
			// 
			this.checkRecurring.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRecurring.Location = new System.Drawing.Point(784, 108);
			this.checkRecurring.Name = "checkRecurring";
			this.checkRecurring.Size = new System.Drawing.Size(196, 18);
			this.checkRecurring.TabIndex = 132;
			this.checkRecurring.Text = "Apply to Recurring Charge";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 145);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 9;
			this.label3.Text = "Bank-Branch";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateEntry
			// 
			this.textDateEntry.Location = new System.Drawing.Point(108, 61);
			this.textDateEntry.Name = "textDateEntry";
			this.textDateEntry.ReadOnly = true;
			this.textDateEntry.Size = new System.Drawing.Size(100, 20);
			this.textDateEntry.TabIndex = 93;
			// 
			// butPrePay
			// 
			this.butPrePay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrePay.Location = new System.Drawing.Point(209, 101);
			this.butPrePay.Name = "butPrePay";
			this.butPrePay.Size = new System.Drawing.Size(61, 20);
			this.butPrePay.TabIndex = 136;
			this.butPrePay.Text = "Prepay";
			this.butPrePay.Click += new System.EventHandler(this.butPrePay_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 125);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 16);
			this.label4.TabIndex = 10;
			this.label4.Text = "Check #";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(6, 65);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(100, 16);
			this.label12.TabIndex = 94;
			this.label12.Text = "Entry Date";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkProcessed
			// 
			this.checkProcessed.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcessed.Location = new System.Drawing.Point(784, 127);
			this.checkProcessed.Name = "checkProcessed";
			this.checkProcessed.Size = new System.Drawing.Size(196, 18);
			this.checkProcessed.TabIndex = 137;
			this.checkProcessed.Text = "Mark as Processed";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(6, 105);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 16);
			this.label5.TabIndex = 11;
			this.label5.Text = "Amount";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupXWeb
			// 
			this.groupXWeb.Controls.Add(this.butReturn);
			this.groupXWeb.Controls.Add(this.butVoid);
			this.groupXWeb.Location = new System.Drawing.Point(646, 30);
			this.groupXWeb.Name = "groupXWeb";
			this.groupXWeb.Size = new System.Drawing.Size(110, 85);
			this.groupXWeb.TabIndex = 138;
			this.groupXWeb.TabStop = false;
			this.groupXWeb.Text = "XWeb";
			// 
			// butReturn
			// 
			this.butReturn.Location = new System.Drawing.Point(17, 20);
			this.butReturn.Name = "butReturn";
			this.butReturn.Size = new System.Drawing.Size(75, 24);
			this.butReturn.TabIndex = 140;
			this.butReturn.Text = "Return";
			this.butReturn.Click += new System.EventHandler(this.butReturn_Click);
			// 
			// butVoid
			// 
			this.butVoid.Location = new System.Drawing.Point(17, 49);
			this.butVoid.Name = "butVoid";
			this.butVoid.Size = new System.Drawing.Size(75, 24);
			this.butVoid.TabIndex = 139;
			this.butVoid.Text = "Void";
			this.butVoid.Click += new System.EventHandler(this.butVoid_Click);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(18, 23);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(86, 14);
			this.labelClinic.TabIndex = 91;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(6, 85);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 16);
			this.label6.TabIndex = 12;
			this.label6.Text = "Payment Date";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPaidBy
			// 
			this.textPaidBy.Location = new System.Drawing.Point(108, 41);
			this.textPaidBy.Name = "textPaidBy";
			this.textPaidBy.ReadOnly = true;
			this.textPaidBy.Size = new System.Drawing.Size(242, 20);
			this.textPaidBy.TabIndex = 32;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(6, 43);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(100, 16);
			this.label11.TabIndex = 33;
			this.label11.Text = "Paid By";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listPayType
			// 
			this.listPayType.Location = new System.Drawing.Point(496, 56);
			this.listPayType.Name = "listPayType";
			this.listPayType.Size = new System.Drawing.Size(120, 95);
			this.listPayType.TabIndex = 4;
			this.listPayType.Click += new System.EventHandler(this.listPayType_Click);
			// 
			// textNote
			// 
			this.textNote.AcceptsTab = true;
			this.textNote.BackColor = System.Drawing.SystemColors.Window;
			this.textNote.DetectLinksEnabled = false;
			this.textNote.DetectUrls = false;
			this.textNote.Location = new System.Drawing.Point(108, 163);
			this.textNote.MaxLength = 4000;
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.Payment;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(361, 83);
			this.textNote.SpellCheckIsEnabled = false;
			this.textNote.TabIndex = 1;
			this.textNote.TabStop = false;
			this.textNote.Text = "";
			// 
			// textAmount
			// 
			this.textAmount.Location = new System.Drawing.Point(108, 101);
			this.textAmount.MaxVal = 100000000D;
			this.textAmount.MinVal = -100000000D;
			this.textAmount.Name = "textAmount";
			this.textAmount.Size = new System.Drawing.Size(100, 20);
			this.textAmount.TabIndex = 2;
			// 
			// textCheckNum
			// 
			this.textCheckNum.Location = new System.Drawing.Point(108, 121);
			this.textCheckNum.Name = "textCheckNum";
			this.textCheckNum.Size = new System.Drawing.Size(100, 20);
			this.textCheckNum.TabIndex = 0;
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(108, 81);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(100, 20);
			this.textDate.TabIndex = 4;
			// 
			// textBankBranch
			// 
			this.textBankBranch.Location = new System.Drawing.Point(108, 141);
			this.textBankBranch.Name = "textBankBranch";
			this.textBankBranch.Size = new System.Drawing.Size(100, 20);
			this.textBankBranch.TabIndex = 1;
			// 
			// checkPayTypeNone
			// 
			this.checkPayTypeNone.Location = new System.Drawing.Point(496, 38);
			this.checkPayTypeNone.Name = "checkPayTypeNone";
			this.checkPayTypeNone.Size = new System.Drawing.Size(204, 18);
			this.checkPayTypeNone.TabIndex = 128;
			this.checkPayTypeNone.Text = "None (Income Transfer)";
			this.checkPayTypeNone.UseVisualStyleBackColor = true;
			this.checkPayTypeNone.CheckedChanged += new System.EventHandler(this.checkPayTypeNone_CheckedChanged);
			this.checkPayTypeNone.Click += new System.EventHandler(this.checkPayTypeNone_Click);
			// 
			// tabProcCharges
			// 
			this.tabProcCharges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabProcCharges.Controls.Add(this.tabPageCharges);
			this.tabProcCharges.Controls.Add(this.tabPageTreatPlan);
			this.tabProcCharges.Location = new System.Drawing.Point(496, 78);
			this.tabProcCharges.Name = "tabProcCharges";
			this.tabProcCharges.SelectedIndex = 0;
			this.tabProcCharges.Size = new System.Drawing.Size(608, 293);
			this.tabProcCharges.TabIndex = 1;
			// 
			// tabPageCharges
			// 
			this.tabPageCharges.Controls.Add(this.checkShowSuperfamily);
			this.tabPageCharges.Controls.Add(this.gridCharges);
			this.tabPageCharges.Controls.Add(this.checkIncludeExplicitCreditsOnly);
			this.tabPageCharges.Controls.Add(this.checkShowAll);
			this.tabPageCharges.Location = new System.Drawing.Point(4, 22);
			this.tabPageCharges.Name = "tabPageCharges";
			this.tabPageCharges.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageCharges.Size = new System.Drawing.Size(600, 267);
			this.tabPageCharges.TabIndex = 0;
			this.tabPageCharges.Text = "Outstanding";
			this.tabPageCharges.UseVisualStyleBackColor = true;
			// 
			// checkShowSuperfamily
			// 
			this.checkShowSuperfamily.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowSuperfamily.Location = new System.Drawing.Point(230, 2);
			this.checkShowSuperfamily.Name = "checkShowSuperfamily";
			this.checkShowSuperfamily.Size = new System.Drawing.Size(193, 20);
			this.checkShowSuperfamily.TabIndex = 204;
			this.checkShowSuperfamily.Text = "Show Super Family Charges";
			this.checkShowSuperfamily.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowSuperfamily.UseVisualStyleBackColor = true;
			this.checkShowSuperfamily.Click += new System.EventHandler(this.checkShowSuperfamily_Click);
			// 
			// gridCharges
			// 
			this.gridCharges.AllowSortingByColumn = true;
			this.gridCharges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridCharges.Location = new System.Drawing.Point(1, 22);
			this.gridCharges.Name = "gridCharges";
			this.gridCharges.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridCharges.Size = new System.Drawing.Size(598, 239);
			this.gridCharges.TabIndex = 144;
			this.gridCharges.Title = "Outstanding Charges";
			this.gridCharges.TranslationName = "TableOutstandingCharges";
			this.gridCharges.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCharges_CellClick);
			// 
			// checkIncludeExplicitCreditsOnly
			// 
			this.checkIncludeExplicitCreditsOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeExplicitCreditsOnly.Location = new System.Drawing.Point(4, 2);
			this.checkIncludeExplicitCreditsOnly.Name = "checkIncludeExplicitCreditsOnly";
			this.checkIncludeExplicitCreditsOnly.Size = new System.Drawing.Size(218, 20);
			this.checkIncludeExplicitCreditsOnly.TabIndex = 205;
			this.checkIncludeExplicitCreditsOnly.Text = "Show Only Allocated Credits";
			this.checkIncludeExplicitCreditsOnly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeExplicitCreditsOnly.UseVisualStyleBackColor = true;
			this.checkIncludeExplicitCreditsOnly.CheckedChanged += new System.EventHandler(this.checkIncludeExplicitCreditsOnly_CheckedChanged);
			// 
			// checkShowAll
			// 
			this.checkShowAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowAll.Location = new System.Drawing.Point(431, 2);
			this.checkShowAll.Name = "checkShowAll";
			this.checkShowAll.Size = new System.Drawing.Size(148, 20);
			this.checkShowAll.TabIndex = 147;
			this.checkShowAll.Text = "Show All Charges";
			this.checkShowAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowAll.UseVisualStyleBackColor = true;
			this.checkShowAll.Click += new System.EventHandler(this.checkShowAll_Clicked);
			// 
			// tabPageTreatPlan
			// 
			this.tabPageTreatPlan.Controls.Add(this.labelTPProcWarning);
			this.tabPageTreatPlan.Controls.Add(this.gridTreatPlan);
			this.tabPageTreatPlan.Location = new System.Drawing.Point(4, 22);
			this.tabPageTreatPlan.Name = "tabPageTreatPlan";
			this.tabPageTreatPlan.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageTreatPlan.Size = new System.Drawing.Size(600, 267);
			this.tabPageTreatPlan.TabIndex = 1;
			this.tabPageTreatPlan.Text = "Treat Plan";
			this.tabPageTreatPlan.UseVisualStyleBackColor = true;
			// 
			// labelTPProcWarning
			// 
			this.labelTPProcWarning.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.labelTPProcWarning.Location = new System.Drawing.Point(2, 4);
			this.labelTPProcWarning.Name = "labelTPProcWarning";
			this.labelTPProcWarning.Size = new System.Drawing.Size(596, 17);
			this.labelTPProcWarning.TabIndex = 146;
			this.labelTPProcWarning.Text = "Treatment Planned Procedures will not be reflected in the total for this account." +
    "";
			this.labelTPProcWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelTPProcWarning.Visible = false;
			// 
			// gridTreatPlan
			// 
			this.gridTreatPlan.AllowSortingByColumn = true;
			this.gridTreatPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridTreatPlan.Location = new System.Drawing.Point(1, 22);
			this.gridTreatPlan.Name = "gridTreatPlan";
			this.gridTreatPlan.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridTreatPlan.Size = new System.Drawing.Size(598, 239);
			this.gridTreatPlan.TabIndex = 145;
			this.gridTreatPlan.Title = "Treatment Planned Procedures";
			this.gridTreatPlan.TranslationName = "TableOutstandingCharges";
			this.gridTreatPlan.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTreatPlan_CellClick);
			// 
			// butPay
			// 
			this.butPay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPay.Image = global::OpenDental.Properties.Resources.Left;
			this.butPay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPay.Location = new System.Drawing.Point(502, 375);
			this.butPay.Name = "butPay";
			this.butPay.Size = new System.Drawing.Size(79, 23);
			this.butPay.TabIndex = 146;
			this.butPay.Text = "Pay";
			this.butPay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.butPay.Click += new System.EventHandler(this.butPay_Click);
			// 
			// labelPayPlan
			// 
			this.labelPayPlan.Location = new System.Drawing.Point(256, 101);
			this.labelPayPlan.Name = "labelPayPlan";
			this.labelPayPlan.Size = new System.Drawing.Size(231, 17);
			this.labelPayPlan.TabIndex = 141;
			this.labelPayPlan.Text = "splits attached to payment plan.";
			this.labelPayPlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelPayPlan.Visible = false;
			// 
			// tabControlSplits
			// 
			this.tabControlSplits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlSplits.Controls.Add(this.tabPageSplits);
			this.tabControlSplits.Controls.Add(this.tabPageAllocated);
			this.tabControlSplits.Location = new System.Drawing.Point(7, 100);
			this.tabControlSplits.Name = "tabControlSplits";
			this.tabControlSplits.SelectedIndex = 0;
			this.tabControlSplits.Size = new System.Drawing.Size(487, 271);
			this.tabControlSplits.TabIndex = 5;
			// 
			// tabPageSplits
			// 
			this.tabPageSplits.Controls.Add(this.gridSplits);
			this.tabPageSplits.Location = new System.Drawing.Point(4, 22);
			this.tabPageSplits.Name = "tabPageSplits";
			this.tabPageSplits.Size = new System.Drawing.Size(479, 245);
			this.tabPageSplits.TabIndex = 0;
			this.tabPageSplits.Text = "Splits";
			this.tabPageSplits.UseVisualStyleBackColor = true;
			// 
			// gridSplits
			// 
			this.gridSplits.AllowSortingByColumn = true;
			this.gridSplits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridSplits.Location = new System.Drawing.Point(0, 0);
			this.gridSplits.Name = "gridSplits";
			this.gridSplits.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridSplits.Size = new System.Drawing.Size(479, 239);
			this.gridSplits.TabIndex = 0;
			this.gridSplits.Title = "Current Payment Splits";
			this.gridSplits.TranslationName = "TableCurrentSplits";
			this.gridSplits.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSplits_CellDoubleClick);
			this.gridSplits.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSplits_CellClick);
			// 
			// tabPageAllocated
			// 
			this.tabPageAllocated.Controls.Add(this.gridAllocated);
			this.tabPageAllocated.Location = new System.Drawing.Point(4, 22);
			this.tabPageAllocated.Name = "tabPageAllocated";
			this.tabPageAllocated.Size = new System.Drawing.Size(479, 245);
			this.tabPageAllocated.TabIndex = 1;
			this.tabPageAllocated.Text = "Allocated";
			this.tabPageAllocated.UseVisualStyleBackColor = true;
			// 
			// gridAllocated
			// 
			this.gridAllocated.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridAllocated.Location = new System.Drawing.Point(0, 0);
			this.gridAllocated.Name = "gridAllocated";
			this.gridAllocated.Size = new System.Drawing.Size(479, 239);
			this.gridAllocated.TabIndex = 117;
			this.gridAllocated.Title = "Split Allocations";
			this.gridAllocated.TranslationName = "TablePaySplitAllocations";
			// 
			// textChargeTotal
			// 
			this.textChargeTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textChargeTotal.Location = new System.Drawing.Point(1049, 376);
			this.textChargeTotal.Name = "textChargeTotal";
			this.textChargeTotal.ReadOnly = true;
			this.textChargeTotal.Size = new System.Drawing.Size(51, 20);
			this.textChargeTotal.TabIndex = 156;
			this.textChargeTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butCreatePartial
			// 
			this.butCreatePartial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butCreatePartial.Image = global::OpenDental.Properties.Resources.Add;
			this.butCreatePartial.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCreatePartial.Location = new System.Drawing.Point(591, 375);
			this.butCreatePartial.Name = "butCreatePartial";
			this.butCreatePartial.Size = new System.Drawing.Size(114, 23);
			this.butCreatePartial.TabIndex = 150;
			this.butCreatePartial.Text = "Add Partials";
			this.butCreatePartial.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.butCreatePartial.Click += new System.EventHandler(this.butCreatePartialSplit_Click);
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label8.Location = new System.Drawing.Point(990, 377);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(58, 18);
			this.label8.TabIndex = 155;
			this.label8.Text = "Total";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(9, 374);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(100, 24);
			this.butDelete.TabIndex = 144;
			this.butDelete.Text = "Delete Splits";
			this.butDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.butDelete.Click += new System.EventHandler(this.butDeleteSplits_Click);
			// 
			// comboGroupBy
			// 
			this.comboGroupBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.comboGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGroupBy.Location = new System.Drawing.Point(793, 376);
			this.comboGroupBy.MaxDropDownItems = 30;
			this.comboGroupBy.Name = "comboGroupBy";
			this.comboGroupBy.Size = new System.Drawing.Size(111, 21);
			this.comboGroupBy.TabIndex = 153;
			this.comboGroupBy.SelectionChangeCommitted += new System.EventHandler(this.comboGroupBy_SelectionChangeCommitted);
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label7.Location = new System.Drawing.Point(715, 377);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(77, 18);
			this.label7.TabIndex = 154;
			this.label7.Text = "Group By";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBoxFiltering
			// 
			this.groupBoxFiltering.Controls.Add(this.comboProviderFilter);
			this.groupBoxFiltering.Controls.Add(this.datePickTo);
			this.groupBoxFiltering.Controls.Add(this.labelDateFrom);
			this.groupBoxFiltering.Controls.Add(this.datePickFrom);
			this.groupBoxFiltering.Controls.Add(this.textFilterProcCodes);
			this.groupBoxFiltering.Controls.Add(this.labelMaxAmount);
			this.groupBoxFiltering.Controls.Add(this.labelMinFilter);
			this.groupBoxFiltering.Controls.Add(this.labelTypeFilter);
			this.groupBoxFiltering.Controls.Add(this.amtMinEnd);
			this.groupBoxFiltering.Controls.Add(this.amtMaxEnd);
			this.groupBoxFiltering.Controls.Add(this.button1);
			this.groupBoxFiltering.Controls.Add(this.comboPatientFilter);
			this.groupBoxFiltering.Controls.Add(this.labelProcCodes);
			this.groupBoxFiltering.Controls.Add(this.comboTypeFilter);
			this.groupBoxFiltering.Controls.Add(this.comboClinicFilter);
			this.groupBoxFiltering.Controls.Add(this.labelPatFilter);
			this.groupBoxFiltering.Controls.Add(this.labelClinicFilter);
			this.groupBoxFiltering.Controls.Add(this.labelProvFilter);
			this.groupBoxFiltering.Location = new System.Drawing.Point(496, -1);
			this.groupBoxFiltering.Name = "groupBoxFiltering";
			this.groupBoxFiltering.Size = new System.Drawing.Size(603, 80);
			this.groupBoxFiltering.TabIndex = 203;
			this.groupBoxFiltering.TabStop = false;
			this.groupBoxFiltering.Text = "Filtering";
			// 
			// comboProviderFilter
			// 
			this.comboProviderFilter.BackColor = System.Drawing.SystemColors.Window;
			this.comboProviderFilter.Location = new System.Drawing.Point(106, 33);
			this.comboProviderFilter.Name = "comboProviderFilter";
			this.comboProviderFilter.SelectionModeMulti = true;
			this.comboProviderFilter.Size = new System.Drawing.Size(90, 21);
			this.comboProviderFilter.TabIndex = 207;
			this.comboProviderFilter.SelectionChangeCommitted += new System.EventHandler(this.FilterChangeCommitted);
			// 
			// datePickTo
			// 
			this.datePickTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.datePickTo.Location = new System.Drawing.Point(199, 56);
			this.datePickTo.Name = "datePickTo";
			this.datePickTo.Size = new System.Drawing.Size(90, 20);
			this.datePickTo.TabIndex = 204;
			this.datePickTo.ValueChanged += new System.EventHandler(this.FilterChangeCommitted);
			// 
			// labelDateFrom
			// 
			this.labelDateFrom.Location = new System.Drawing.Point(15, 58);
			this.labelDateFrom.Name = "labelDateFrom";
			this.labelDateFrom.Size = new System.Drawing.Size(90, 16);
			this.labelDateFrom.TabIndex = 206;
			this.labelDateFrom.Text = "From/To Dates:";
			this.labelDateFrom.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// datePickFrom
			// 
			this.datePickFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.datePickFrom.Location = new System.Drawing.Point(106, 56);
			this.datePickFrom.Name = "datePickFrom";
			this.datePickFrom.Size = new System.Drawing.Size(90, 20);
			this.datePickFrom.TabIndex = 203;
			this.datePickFrom.ValueChanged += new System.EventHandler(this.FilterChangeCommitted);
			// 
			// textFilterProcCodes
			// 
			this.textFilterProcCodes.Location = new System.Drawing.Point(395, 56);
			this.textFilterProcCodes.Name = "textFilterProcCodes";
			this.textFilterProcCodes.Size = new System.Drawing.Size(90, 20);
			this.textFilterProcCodes.TabIndex = 187;
			this.textFilterProcCodes.TextChanged += new System.EventHandler(this.FilterChangeCommitted);
			// 
			// labelMaxAmount
			// 
			this.labelMaxAmount.Location = new System.Drawing.Point(488, 14);
			this.labelMaxAmount.Name = "labelMaxAmount";
			this.labelMaxAmount.Size = new System.Drawing.Size(89, 18);
			this.labelMaxAmount.TabIndex = 198;
			this.labelMaxAmount.Text = "Amt End Max";
			this.labelMaxAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelMinFilter
			// 
			this.labelMinFilter.Location = new System.Drawing.Point(395, 14);
			this.labelMinFilter.Name = "labelMinFilter";
			this.labelMinFilter.Size = new System.Drawing.Size(87, 18);
			this.labelMinFilter.TabIndex = 200;
			this.labelMinFilter.Text = "Amt End Min.";
			this.labelMinFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelTypeFilter
			// 
			this.labelTypeFilter.Location = new System.Drawing.Point(199, 14);
			this.labelTypeFilter.Name = "labelTypeFilter";
			this.labelTypeFilter.Size = new System.Drawing.Size(60, 18);
			this.labelTypeFilter.TabIndex = 202;
			this.labelTypeFilter.Text = "Type";
			this.labelTypeFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// amtMinEnd
			// 
			this.amtMinEnd.DecimalPlaces = 2;
			this.amtMinEnd.Location = new System.Drawing.Point(395, 33);
			this.amtMinEnd.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
			this.amtMinEnd.Minimum = new decimal(new int[] {
            99999999,
            0,
            0,
            -2147483648});
			this.amtMinEnd.Name = "amtMinEnd";
			this.amtMinEnd.Size = new System.Drawing.Size(90, 20);
			this.amtMinEnd.TabIndex = 199;
			this.amtMinEnd.ThousandsSeparator = true;
			this.amtMinEnd.ValueChanged += new System.EventHandler(this.FilterChangeCommitted);
			// 
			// amtMaxEnd
			// 
			this.amtMaxEnd.DecimalPlaces = 2;
			this.amtMaxEnd.Location = new System.Drawing.Point(488, 33);
			this.amtMaxEnd.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
			this.amtMaxEnd.Minimum = new decimal(new int[] {
            99999999,
            0,
            0,
            -2147483648});
			this.amtMaxEnd.Name = "amtMaxEnd";
			this.amtMaxEnd.Size = new System.Drawing.Size(90, 20);
			this.amtMaxEnd.TabIndex = 197;
			this.amtMaxEnd.ThousandsSeparator = true;
			this.amtMaxEnd.ValueChanged += new System.EventHandler(this.FilterChangeCommitted);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(503, 55);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 24);
			this.button1.TabIndex = 141;
			this.button1.Text = "Refresh";
			this.button1.Click += new System.EventHandler(this.FilterChangeCommitted);
			// 
			// comboPatientFilter
			// 
			this.comboPatientFilter.ArraySelectedIndices = new int[0];
			this.comboPatientFilter.AutoScroll = true;
			this.comboPatientFilter.BackColor = System.Drawing.SystemColors.Window;
			this.comboPatientFilter.Items = ((System.Collections.ArrayList)(resources.GetObject("comboPatientFilter.Items")));
			this.comboPatientFilter.Location = new System.Drawing.Point(13, 33);
			this.comboPatientFilter.Name = "comboPatientFilter";
			this.comboPatientFilter.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboPatientFilter.SelectedIndices")));
			this.comboPatientFilter.Size = new System.Drawing.Size(90, 21);
			this.comboPatientFilter.TabIndex = 195;
			this.comboPatientFilter.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.FilterChangeCommitted);
			// 
			// labelProcCodes
			// 
			this.labelProcCodes.Location = new System.Drawing.Point(301, 56);
			this.labelProcCodes.Name = "labelProcCodes";
			this.labelProcCodes.Size = new System.Drawing.Size(93, 18);
			this.labelProcCodes.TabIndex = 188;
			this.labelProcCodes.Text = "Proc Codes:";
			this.labelProcCodes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTypeFilter
			// 
			this.comboTypeFilter.ArraySelectedIndices = new int[0];
			this.comboTypeFilter.AutoScroll = true;
			this.comboTypeFilter.BackColor = System.Drawing.SystemColors.Window;
			this.comboTypeFilter.Items = ((System.Collections.ArrayList)(resources.GetObject("comboTypeFilter.Items")));
			this.comboTypeFilter.Location = new System.Drawing.Point(199, 33);
			this.comboTypeFilter.Name = "comboTypeFilter";
			this.comboTypeFilter.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboTypeFilter.SelectedIndices")));
			this.comboTypeFilter.Size = new System.Drawing.Size(100, 21);
			this.comboTypeFilter.TabIndex = 201;
			this.comboTypeFilter.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.FilterChangeCommitted);
			// 
			// comboClinicFilter
			// 
			this.comboClinicFilter.ArraySelectedIndices = new int[0];
			this.comboClinicFilter.AutoScroll = true;
			this.comboClinicFilter.BackColor = System.Drawing.SystemColors.Window;
			this.comboClinicFilter.Items = ((System.Collections.ArrayList)(resources.GetObject("comboClinicFilter.Items")));
			this.comboClinicFilter.Location = new System.Drawing.Point(302, 33);
			this.comboClinicFilter.Name = "comboClinicFilter";
			this.comboClinicFilter.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboClinicFilter.SelectedIndices")));
			this.comboClinicFilter.Size = new System.Drawing.Size(90, 21);
			this.comboClinicFilter.TabIndex = 193;
			this.comboClinicFilter.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.FilterChangeCommitted);
			// 
			// labelPatFilter
			// 
			this.labelPatFilter.Location = new System.Drawing.Point(13, 14);
			this.labelPatFilter.Name = "labelPatFilter";
			this.labelPatFilter.Size = new System.Drawing.Size(67, 18);
			this.labelPatFilter.TabIndex = 196;
			this.labelPatFilter.Text = "Patients";
			this.labelPatFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelClinicFilter
			// 
			this.labelClinicFilter.Location = new System.Drawing.Point(302, 14);
			this.labelClinicFilter.Name = "labelClinicFilter";
			this.labelClinicFilter.Size = new System.Drawing.Size(60, 18);
			this.labelClinicFilter.TabIndex = 194;
			this.labelClinicFilter.Text = "Clinics";
			this.labelClinicFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelProvFilter
			// 
			this.labelProvFilter.Location = new System.Drawing.Point(106, 14);
			this.labelProvFilter.Name = "labelProvFilter";
			this.labelProvFilter.Size = new System.Drawing.Size(67, 18);
			this.labelProvFilter.TabIndex = 192;
			this.labelProvFilter.Text = "Providers";
			this.labelProvFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butClear
			// 
			this.butClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butClear.Location = new System.Drawing.Point(115, 374);
			this.butClear.Name = "butClear";
			this.butClear.Size = new System.Drawing.Size(89, 24);
			this.butClear.TabIndex = 145;
			this.butClear.Text = "Delete All";
			this.butClear.Click += new System.EventHandler(this.butDeleteAllSplits_Click);
			// 
			// label13
			// 
			this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label13.Location = new System.Drawing.Point(380, 377);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(58, 18);
			this.label13.TabIndex = 148;
			this.label13.Text = "Total";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSplitTotal
			// 
			this.textSplitTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textSplitTotal.Location = new System.Drawing.Point(439, 376);
			this.textSplitTotal.Name = "textSplitTotal";
			this.textSplitTotal.ReadOnly = true;
			this.textSplitTotal.Size = new System.Drawing.Size(51, 20);
			this.textSplitTotal.TabIndex = 149;
			this.textSplitTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butAddManual
			// 
			this.butAddManual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAddManual.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddManual.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddManual.Location = new System.Drawing.Point(210, 374);
			this.butAddManual.Name = "butAddManual";
			this.butAddManual.Size = new System.Drawing.Size(92, 24);
			this.butAddManual.TabIndex = 151;
			this.butAddManual.Text = "Add Split";
			this.butAddManual.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.butAddManual.Click += new System.EventHandler(this.butAddManualSplit_Click);
			// 
			// contextMenuPaySimple
			// 
			this.contextMenuPaySimple.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPaySimple});
			// 
			// menuPaySimple
			// 
			this.menuPaySimple.Index = 0;
			this.menuPaySimple.Text = "Settings";
			this.menuPaySimple.Click += new System.EventHandler(this.menuPaySimple_Click);
			// 
			// FormPayment
			// 
			this.ClientSize = new System.Drawing.Size(1111, 696);
			this.Controls.Add(this.splitContainerCharges);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butDeletePayment);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butPrintReceipt);
			this.Controls.Add(this.butEmailReceipt);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(1111, 325);
			this.Name = "FormPayment";
			this.ShowInTaskbar = false;
			this.Text = "Payment";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPayment_FormClosing);
			this.Load += new System.EventHandler(this.FormPayment_Load);
			this.splitContainerCharges.Panel1.ResumeLayout(false);
			this.splitContainerCharges.Panel1.PerformLayout();
			this.splitContainerCharges.Panel2.ResumeLayout(false);
			this.splitContainerCharges.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerCharges)).EndInit();
			this.splitContainerCharges.ResumeLayout(false);
			this.groupXWeb.ResumeLayout(false);
			this.tabProcCharges.ResumeLayout(false);
			this.tabPageCharges.ResumeLayout(false);
			this.tabPageTreatPlan.ResumeLayout(false);
			this.tabControlSplits.ResumeLayout(false);
			this.tabPageSplits.ResumeLayout(false);
			this.tabPageAllocated.ResumeLayout(false);
			this.groupBoxFiltering.ResumeLayout(false);
			this.groupBoxFiltering.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.amtMinEnd)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.amtMaxEnd)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormPayment_Load(object sender,System.EventArgs e) {
			_loadData=PaymentEdit.GetLoadData(_patCur,_paymentCur,_listPatNums,IsNew,(IsIncomeTransfer || _paymentCur.PayType==0));
			_superFamCur=_loadData.SuperFam;
			_isPayConnectPortal=_paymentCur.PaymentSource.In(CreditCardSource.PayConnectPortal,CreditCardSource.PayConnectPortalLogin);
			if(_isPayConnectPortal) {
				groupXWeb.Text="PayConnect Portal";
			}
			if(PrefC.GetEnum<YN>(PrefName.PrePayAllowedForTpProcs)==YN.No) {
				tabProcCharges.TabPages.Remove(tabPageTreatPlan);
			}
			else {
				if(tabProcCharges.TabPages.Contains(tabPageTreatPlan)){
					tabProcCharges.TabPages.Remove(tabPageTreatPlan);
				}
				tabProcCharges.TabPages.Add(tabPageTreatPlan);
			}
			if(IsNew) {
				checkPayTypeNone.Enabled=true;
				if(!Security.IsAuthorized(Permissions.PaymentCreate)) {//date not checked here
					DialogResult=DialogResult.Cancel;
					return;
				}
				butDeletePayment.Enabled=false;
			}
			else {
				checkPayTypeNone.Enabled=false;
				checkRecurring.Checked=_paymentCur.IsRecurringCC;
				if(!Security.IsAuthorized(Permissions.PaymentEdit,_paymentCur.PayDate)) {
					butOK.Enabled=false;
					butDeletePayment.Enabled=false;
					butAddManual.Enabled=false;
					gridSplits.Enabled=false;
					butPay.Enabled=false;
					butCreatePartial.Enabled=false;
					butClear.Enabled=false;
					butDelete.Enabled=false;
					checkRecurring.Enabled=false;
					panelXcharge.Enabled=false;
					butPayConnect.Enabled=false;
					butPaySimple.Enabled=false;
					if(Security.IsAuthorized(Permissions.SplitCreatePastLockDate,true)) {
						//Since we are enabling the OK button, we need to make sure everything else is disabled (except for Add).
						butOK.Enabled=true;
						butAddManual.Enabled=true;
						comboClinic.Enabled=false;
						textDate.ReadOnly=true;
						textAmount.ReadOnly=true;
						butPrePay.Enabled=false;
						textCheckNum.ReadOnly=true;
						textBankBranch.ReadOnly=true;
						textNote.ReadOnly=true;
						checkPayTypeNone.Enabled=false;
						listPayType.Enabled=false;
						comboDepositAccount.Enabled=false;
						comboCreditCards.Enabled=false;
						checkProcessed.Enabled=false;
						gridSplits.Enabled=true;
					}
				}
			}
			if(PrefC.HasClinicsEnabled) {
				_listUserClinicNums=new List<long>();
				List<Clinic> listClinics=Clinics.GetForUserod(Security.CurUser);
				comboClinic.Items.Clear();
				comboClinic.Items.Add(Lan.g(this,"None"));
				_listUserClinicNums.Add(0);//this way both lists have the same number of items in it
				comboClinic.SelectedIndex=0;
				for(int i=0;i<listClinics.Count;i++) {
					comboClinic.Items.Add(listClinics[i].Abbr);
					_listUserClinicNums.Add(listClinics[i].ClinicNum);
					if(listClinics[i].ClinicNum==_paymentCur.ClinicNum) {
						comboClinic.SelectedIndex=i+1;
					}
				}
			}
			else {//clinics not enabled
				comboClinic.Visible=false;
				labelClinic.Visible=false;
				comboClinicFilter.Visible=false;
				labelClinicFilter.Visible=false;
			}
			if(_paymentCur.ProcessStatus==ProcessStat.OfficeProcessed) {
				checkProcessed.Visible=false;//This checkbox will only show if the payment originated online.
			}
			else if(_paymentCur.ProcessStatus==ProcessStat.OnlineProcessed) {
				checkProcessed.Checked=true;
			}
			_listCreditCards=_loadData.ListCreditCards;
			FillCreditCards();
			_tableBalances=_loadData.TableBalances;
			//this works even if patient not in family
			textPaidBy.Text=_curFamOrSuperFam.GetNameInFamFL(_paymentCur.PatNum);
			textDateEntry.Text=_paymentCur.DateEntry.ToShortDateString();
			textDate.Text=_paymentCur.PayDate.ToShortDateString();
			textAmount.Text=_paymentCur.PayAmt.ToString("F");
			textCheckNum.Text=_paymentCur.CheckNum;
			textBankBranch.Text=_paymentCur.BankBranch;
			_listPaymentTypeDefs=Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
			for(int i=0;i<_listPaymentTypeDefs.Count;i++) {
				listPayType.Items.Add(_listPaymentTypeDefs[i].ItemName);
				if(IsNew && PrefC.GetBool(PrefName.PaymentsPromptForPayType)) {//skip auto selecting payment type if preference is enabled and payment is new
					continue;//user will be forced to selectan indexbefore closing or clicking ok
				}
				if(_listPaymentTypeDefs[i].DefNum==_paymentCur.PayType) {
					listPayType.SelectedIndex=i;
				}
			}
			textNote.Text=_paymentCur.PayNote;
			Deposit deposit=null;
			if(_paymentCur.DepositNum!=0) {
				deposit=Deposits.GetOne(_paymentCur.DepositNum);
			}
			if(deposit==null) {//If there was none or it got deleted, disable controls.
				labelDeposit.Visible=false;
				textDeposit.Visible=false;
			}
			else {
				textDeposit.Text=deposit.DateDeposit.ToShortDateString();
				textAmount.ReadOnly=true;
				textAmount.BackColor=SystemColors.Control;
				butPay.Enabled=false;
			}
			_listSplitsCur=_loadData.ListSplits;//Count might be 0
			_listPaySplitsOld=new List<PaySplit>();
			_listPaySplitsAssociated=new List<PaySplits.PaySplitAssociated>();
			for(int i=0;i<_listSplitsCur.Count;i++) {
				_listPaySplitsOld.Add(_listSplitsCur[i].Copy());
			}
			_listPaySplitAllocations=_loadData.ListPaySplitAllocations;
			if(_listPaySplitAllocations.Count==0) {
				tabControlSplits.TabPages.Remove(tabPageAllocated);
			}
			FillListPaySplitAssociated();
			if(IsNew && UnearnedAmt>0) {
				List<PaySplits.PaySplitAssociated> listUnearnedPayAssociated=PaymentEdit.AllocateUnearned(ListEntriesPayFirst,ref _listSplitsCur
					,_paymentCur,UnearnedAmt,_curFamOrSuperFam);
				if(listUnearnedPayAssociated.Count>0) {
					_listPaySplitsAssociated.AddRange(listUnearnedPayAssociated);
				}
			}
			if(IsNew) {
				//Fill comboDepositAccount based on autopay for listPayType.SelectedIndex
				SetComboDepositAccounts();
				textDepositAccount.Visible=false;
			}
			else {
				//put a description in the textbox.  If the user clicks on the same or another item in listPayType,
				//then the textbox will go away, and be replaced by comboDepositAccount.
				labelDepositAccount.Visible=false;
				comboDepositAccount.Visible=false;
				Transaction trans=_loadData.Transaction;
				if(trans==null) {
					textDepositAccount.Visible=false;
				}
				else {
					//add only the description based on PaymentCur attached to transaction
					List<JournalEntry> jeL=JournalEntries.GetForTrans(trans.TransactionNum);
					for(int i=0;i<jeL.Count;i++) {
						Account account=Accounts.GetAccount(jeL[i].AccountNum);
						//The account could be null if the AccountNum was never set correctly due to the automatic payment entry setup missing an income account from older versions.
						if(account!=null && account.AcctType==AccountType.Asset) {
							textDepositAccount.Text=jeL[i].DateDisplayed.ToShortDateString();
							if(jeL[i].DebitAmt>0) {
								textDepositAccount.Text+=" "+jeL[i].DebitAmt.ToString("c");
							}
							else {//negative
								textDepositAccount.Text+=" "+(-jeL[i].CreditAmt).ToString("c");
							}
							break;
						}
					}
				}
			}
			if(!string.IsNullOrEmpty(_paymentCur.Receipt)) {
				if(PrefC.GetBool(PrefName.AllowEmailCCReceipt)) {
					butEmailReceipt.Visible=true;
				}
				butPrintReceipt.Visible=true;
			}
			_listValidPayPlans=_loadData.ListValidPayPlans;
			comboGroupBy.Items.Add("None");
			comboGroupBy.Items.Add("Provider");
			if(PrefC.HasClinicsEnabled) {
				comboGroupBy.Items.Add("Clinic and Provider");
			}
			comboGroupBy.SelectedIndex=0;
			if(IsIncomeTransfer || _paymentCur.PayType==0) {
				checkPayTypeNone.Checked=true;
			}
			if(_patCur.SuperFamily<=0) {
				checkShowSuperfamily.Visible=false;
			}
			else { 
				//Check the Super Family box if there are any splits from a member in the super family who is not in the immediate family.
				List<Patient> listSuperFamExclusive=_superFamCur.ListPats.Where(x => !_famCur.IsInFamily(x.PatNum)).ToList();
				if(!IsNew && (_listSplitsCur.Any(x => x.PatNum.In(listSuperFamExclusive.Select(y => y.PatNum))))) {
					checkShowSuperfamily.Checked=true;
				}
			}
			Init(_loadData);
			if(InitialPaySplitNum!=0) {
				gridSplits.SetSelected(false);
				for(int i=0;i<_listSplitsCur.Count;i++) {
					if(InitialPaySplitNum==_listSplitsCur[i].SplitNum) {
						gridSplits.SetSelected(i,true);
					}
				}
				HighlightChargesForSplits();
			}
			CheckUIState();
			_originalHeight=Height;
			if(PrefC.GetBool(PrefName.PaymentWindowDefaultHideSplits)) {
				ToggleShowHideSplits();//set hidden
			}
			textCheckNum.Select();
			Plugins.HookAddCode(this,"FormPayment.Load_end",_paymentCur,IsNew);
		}

		private void FillCreditCards() {
			AddCreditCardsToCombo(_listCreditCards);
			comboCreditCards.SelectedIndex=0;
			bool isXWebCardPresent=_listCreditCards.Any(x => x.IsXWeb());
			bool isPayConnectPortalCardPresent=_listCreditCards.Any(x => x.IsPayConnectPortal());
			_xWebResponse=_loadData.XWebResponse;
			_payConnectResponseWeb=_loadData.PayConnectResponseWeb;
			string _payConnectTransType="";
			if(_payConnectResponseWeb!=null && _payConnectResponseWeb.IsFromWebPortal) {
				_payConnectTransType=_payConnectResponseWeb.TransType.ToString();
			}
			groupXWeb.Visible=false;
			if(isXWebCardPresent || _xWebResponse!=null 
				|| _isPayConnectPortal || _payConnectResponseWeb!=null) 
			{
				groupXWeb.Visible=true;
			}
			//PayConnect will only let you void a payment in the first 25 minutes.
			if((_payConnectResponseWeb==null || _payConnectTransType==PayConnectService.transType.VOID.ToString() || DateTime.Now > _payConnectResponseWeb.DateTimeEntry.AddMinutes(25))
				&& (_xWebResponse==null || _xWebResponse.XTransactionType==XWebTransactionType.CreditVoidTransaction)) 
			{
				//Can't run an XWeb/PayConnect void unless this payment is attached to a non-void transaction.
				butVoid.Visible=false;
				groupXWeb.Height=55;
			}
			if(!isXWebCardPresent && _payConnectResponseWeb==null) {
				butReturn.Visible=false;
				butVoid.Location=butReturn.Location;
				groupXWeb.Height=55;
			}
		}

		private void AddCreditCardsToCombo(List<CreditCard> listCreditCards,Func<CreditCard,bool> funcSelectCard=null) {
			comboCreditCards.Items.Clear();
			for(int i=0;i<listCreditCards.Count;i++) {
				string cardNum=listCreditCards[i].CCNumberMasked;
				if(Regex.IsMatch(cardNum,"^\\d{12}(\\d{0,7})")) { //Credit cards can have a minimum of 12 digits, maximum of 19
					int idxLast4Digits=(cardNum.Length-4);
					cardNum=(new string('X',12))+cardNum.Substring(idxLast4Digits);//replace the first 12 with 12 X's
				}
				if(listCreditCards[i].IsXWeb()) {
					cardNum+=" (XWeb)";
				}
				string tokensForCC=listCreditCards[i].GetTokenString();
				cardNum+=(string.IsNullOrEmpty(tokensForCC) ? "" : " "+tokensForCC);
				comboCreditCards.Items.Add(cardNum,listCreditCards[i]);
				if(funcSelectCard!=null && funcSelectCard(listCreditCards[i])) {
					comboCreditCards.SelectedIndex=i;
				}
			}
			comboCreditCards.Items.Add(Lan.g(this,"New Card"),new CreditCard()); //CreditCardNum=0
			if(comboCreditCards.SelectedIndex < 0) {
				comboCreditCards.SelectedIndex=comboCreditCards.Items.Count-1;
			}
		}

		///<summary>Populating _listPaySplitsAssociated with splits that are linked to each other.</summary>
		private void FillListPaySplitAssociated() {
			//All paysplits that have an FSplitNum of 0 are original paysplits.
			//Only loop through all allocated paysplits for this particular payment and find their corresponding original paysplit and make a PaySplitAssociated object.
			//We purposefully do not use _loadData.ListPaySplitAllocations because it contains paysplits that are not associated to this particular payment.
			List<PaySplit> listPaySplitAllocations=_listSplitsCur.FindAll(x => x.FSplitNum > 0);
			foreach(PaySplit paySplitAllocated in listPaySplitAllocations) {
				if(paySplitAllocated.FSplitNum==0) {
					continue;
				}
				//Find the corresponding original paysplit for the current allocated paysplit.
				//Look through all of the paysplits that are associated to this particular payment first (prefer _listSplitsCur due to the sync).
				PaySplit psOrig=_listSplitsCur.Find(x => x.SplitNum==paySplitAllocated.FSplitNum);
				if(psOrig==null) {
					//The original is not associated to this particular payment, look through all of the associated prepayment paysplits next.
					psOrig=_loadData.ListPrePaysForPayment.Find(x => x.SplitNum==paySplitAllocated.FSplitNum);
				}
				if(psOrig!=null) {
					_listPaySplitsAssociated.Add(new PaySplits.PaySplitAssociated(psOrig,paySplitAllocated));
				}
			}
		}

		///<summary>Performs all of the Load functionality.</summary>
		private void Init(PaymentEdit.LoadData loadData=null,bool doAutoSplit=true) {
			_isInit=true;
			AmtTotal=(decimal)_paymentCur.PayAmt;
			if(ListEntriesPayFirst==null) {
				ListEntriesPayFirst=new List<AccountEntry>();//will be null when double clicking from account module. 
			}
			bool doShowExplicitCreditsOnly=checkIncludeExplicitCreditsOnly.Checked && checkIncludeExplicitCreditsOnly.Visible;
			PaymentEdit.InitData initData=PaymentEdit.Init(_loadData.ListAssociatedPatients,_famCur,_superFamCur,_paymentCur,_listSplitsCur
				,ListEntriesPayFirst,_patCur.PatNum,_dictPatients,checkPayTypeNone.Checked,_preferCurrentPat,loadData,doAutoSplit,doShowExplicitCreditsOnly);
			_paymentCur.PayAmt=(double)AmtTotal;//Reset it.	
			textSplitTotal.Text=initData.SplitTotal.ToString("f");
			_dictPatients=initData.DictPats;
			if(checkPayTypeNone.Checked) {
				if(PrefC.HasClinicsEnabled) {
					comboGroupBy.SelectedIndex=2;
				}
				else { 
					comboGroupBy.SelectedIndex=1;
				}
			}
			//Get data from constructing charges list, linking credits, and auto splitting.
			_listSplitsCur=initData.AutoSplitData.ListSplitsCur;
			_listAccountCharges=initData.AutoSplitData.ListAccountCharges;
			_paymentCur=initData.AutoSplitData.Payment;
			FillFilters();
			RefreshGridsAndSelections();
			_isInit=false;
		}

		///<summary>Called whenever gridSplits requires refreshing - will preserve former selections by the user.</summary>
		private void RefreshGridsAndSelections() {
			FillGridSplits();
			//Select all charges on the right side that the paysplits are associated with.  Helps the user see what charges are attached.
			gridSplits.SetSelected(true);
			HighlightChargesForSplits();
		}

		///<summary>Mimics FormClaimPayEdit.CheckUIState().</summary>
		private void CheckUIState() {
			_xProg=Programs.GetCur(ProgramName.Xcharge);
			_xPath=Programs.GetProgramPath(_xProg);
			Program progPayConnect=Programs.GetCur(ProgramName.PayConnect);
			Program progPaySimple=Programs.GetCur(ProgramName.PaySimple);
			if(_xProg==null || progPayConnect==null || progPaySimple==null) {//Should not happen.
				panelXcharge.Visible=(_xProg!=null);
				butPayConnect.Visible=(progPayConnect!=null);
				butPaySimple.Visible=(progPaySimple!=null);
				return;
			}
			panelXcharge.Visible=false;
			butPayConnect.Visible=false;
			butPaySimple.Visible=false;
			if(checkPayTypeNone.Checked) {
				return;
			}
			if(!progPayConnect.Enabled && !_xProg.Enabled && !progPaySimple.Enabled) {//if none enabled
				//show all so user can pick
				panelXcharge.Visible=true;
				butPayConnect.Visible=true;
				butPaySimple.Visible=true;
				return;
			}
			//show if enabled.  User could have all enabled.
			if(progPayConnect.Enabled) {
				//if clinics are disabled, PayConnect is enabled if marked enabled
				if(!PrefC.HasClinicsEnabled) {
					butPayConnect.Visible=true;
				}
				else {//if clinics are enabled, PayConnect is enabled if the PaymentType is valid and the Username and Password are not blank
					string paymentType=ProgramProperties.GetPropVal(progPayConnect.ProgramNum,"PaymentType",_paymentCur.ClinicNum);
					if(!string.IsNullOrEmpty(ProgramProperties.GetPropVal(progPayConnect.ProgramNum,"Username",_paymentCur.ClinicNum))
						&& !string.IsNullOrEmpty(ProgramProperties.GetPropVal(progPayConnect.ProgramNum,"Password",_paymentCur.ClinicNum))
						&& _listPaymentTypeDefs.Any(x => x.DefNum.ToString()==paymentType))
					{
						butPayConnect.Visible=true;
					}
				}
			}
			//show if enabled.  User could have both enabled.
			if(_xProg.Enabled) {
				//if clinics are disabled, X-Charge is enabled if marked enabled
				if(!PrefC.HasClinicsEnabled) {
					panelXcharge.Visible=true;
				}
				else {//if clinics are enabled, X-Charge is enabled if the PaymentType is valid and the Username and Password are not blank
					string paymentType=ProgramProperties.GetPropVal(_xProg.ProgramNum,"PaymentType",_paymentCur.ClinicNum);
					if(!string.IsNullOrEmpty(ProgramProperties.GetPropVal(_xProg.ProgramNum,"Username",_paymentCur.ClinicNum))
						&& !string.IsNullOrEmpty(ProgramProperties.GetPropVal(_xProg.ProgramNum,"Password",_paymentCur.ClinicNum))
						&& _listPaymentTypeDefs.Any(x => x.DefNum.ToString()==paymentType))
					{
						panelXcharge.Visible=true;
					}
				}
			}
			if(progPaySimple.Enabled) {
				//if clinics are disabled, PaySimple is enabled if marked enabled
				if(!PrefC.HasClinicsEnabled) {
					butPaySimple.Visible=true;
				}
				else {//if clinics are enabled, PaySimple is enabled if the PaymentType is valid and the Username and Key are not blank
					string paymentType=ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.ProgramNum,PaySimple.PropertyDescs.PaySimplePayTypeCC,_paymentCur.ClinicNum);
					if(!string.IsNullOrEmpty(ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.ProgramNum,PaySimple.PropertyDescs.PaySimpleApiUserName,_paymentCur.ClinicNum))
						&& !string.IsNullOrEmpty(ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.ProgramNum,PaySimple.PropertyDescs.PaySimpleApiKey,_paymentCur.ClinicNum))
						&& _listPaymentTypeDefs.Any(x => x.DefNum.ToString()==paymentType)) {
						butPaySimple.Visible=true;
					}
				}
			}
			if(panelXcharge.Visible==false && butPayConnect.Visible==false && butPaySimple.Visible==false) {
				//This is an office with clinics and one of the payment processing bridges is enabled but this particular clinic doesn't have one set up.
				if(_xProg.Enabled) {
					panelXcharge.Visible=true;
				}
				if(progPayConnect.Enabled) {
					butPayConnect.Visible=true;
				}
				if(progPaySimple.Enabled) {
					butPaySimple.Visible=true;
				}
			}
		}

		private void FillGridAllocated() {
			if(_listPaySplitAllocations.Count==0) {
				return;
			}
			gridAllocated.BeginUpdate();
			gridAllocated.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TablePaySplitAllocations","Date"),80);
			gridAllocated.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaySplitAllocations","Clinic"),80);
			gridAllocated.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaySplitAllocations","Patient"),140);
			gridAllocated.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaySplitAllocations","Amount"),80,HorizontalAlignment.Right);
			gridAllocated.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaySplitAllocations","Unearned"),50);
			gridAllocated.ListGridColumns.Add(col);
			gridAllocated.ListGridRows.Clear();
			GridRow row;
			for(int i=0;i<_listPaySplitAllocations.Count;i++) {
				row=new GridRow();
				row.Cells.Add(_listPaySplitAllocations[i].DatePay.ToShortDateString());
				row.Cells.Add(Clinics.GetAbbr(_listPaySplitAllocations[i].ClinicNum));
				row.Cells.Add(_curFamOrSuperFam.GetNameInFamFL(_listPaySplitAllocations[i].PatNum));
				row.Cells.Add(_listPaySplitAllocations[i].SplitAmt.ToString("F"));
				row.Cells.Add(Defs.GetName(DefCat.PaySplitUnearnedType,_listPaySplitAllocations[i].UnearnedType));//handles 0 just fine
				gridAllocated.ListGridRows.Add(row);
			}
			gridAllocated.EndUpdate();
		}

		///<summary>Fills the paysplit grid.</summary>
		private void FillGridSplits() {
			//Fill left grid with paysplits created
			List<long> listMissingProcsNums=_listSplitsCur.Where(x => x.ProcNum!=0 && !_loadData.ListProcsForSplits.Any(y => y.ProcNum==x.ProcNum))
				.Select(x => x.ProcNum).ToList();
			_loadData.ListProcsForSplits.AddRange(Procedures.GetManyProc(listMissingProcsNums,false));
			gridSplits.BeginUpdate();
			gridSplits.ListGridColumns.Clear();
			GridColumn col;
			col=new GridColumn(Lan.g(this,"Date"),65,HorizontalAlignment.Center,GridSortingStrategy.DateParse);
			gridSplits.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Prov"),40, GridSortingStrategy.StringCompare);
			gridSplits.ListGridColumns.Add(col);
			if(PrefC.HasClinicsEnabled) {//Clinics
				col=new GridColumn(Lan.g(this,"Clinic"),40, GridSortingStrategy.StringCompare);
				gridSplits.ListGridColumns.Add(col);
			}
			col=new GridColumn(Lan.g(this,"Patient"),100,GridSortingStrategy.StringCompare);
			gridSplits.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Code"),60, GridSortingStrategy.StringCompare);
			gridSplits.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Type"),100, GridSortingStrategy.StringCompare);
			gridSplits.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Amount"),55,HorizontalAlignment.Right, GridSortingStrategy.AmountParse);
			gridSplits.ListGridColumns.Add(col);
			gridSplits.ListGridRows.Clear();
			GridRow row;
			decimal splitTotal=0;
			for(int i=0;i<_listSplitsCur.Count;i++) {
				splitTotal+=(decimal)_listSplitsCur[i].SplitAmt;
				row=new GridRow();
				row.Tag=_listSplitsCur[i];
				row.Cells.Add(_listSplitsCur[i].DatePay.ToShortDateString());//Date
				row.Cells.Add(Providers.GetAbbr(_listSplitsCur[i].ProvNum));//Prov
				if(PrefC.HasClinicsEnabled) {//Clinics
					if(_listSplitsCur[i].ClinicNum!=0) {
						row.Cells.Add(Clinics.GetAbbr(_listSplitsCur[i].ClinicNum));//Clinic
					}
					else {
						row.Cells.Add("");//Clinic
					}
				}
				Patient patCur;
				if(!_dictPatients.TryGetValue(_listSplitsCur[i].PatNum,out patCur)) {
					patCur=Patients.GetLim(_listSplitsCur[i].PatNum);
					_dictPatients[patCur.PatNum]=patCur;
				}
				string patName=patCur.LName + ", " + patCur.FName;
				row.Cells.Add(patName);//Patient
				Procedure proc=new Procedure();
				if(_listSplitsCur[i].ProcNum!=0) {
					proc=_loadData.ListProcsForSplits.FirstOrDefault(x => x.ProcNum==_listSplitsCur[i].ProcNum)??new Procedure();
				}
				row.Cells.Add(ProcedureCodes.GetStringProcCode(proc.CodeNum));//ProcCode
				string type="";
				if(_listSplitsCur[i].PayPlanNum!=0) {
					type+="PayPlanCharge";//Type
					if(_listSplitsCur[i].IsInterestSplit && _listSplitsCur[i].ProcNum==0 && _listSplitsCur[i].ProvNum!=0) {
						type+=" (interest)";
					}
				}
				if(_listSplitsCur[i].ProcNum!=0) {//Procedure
					string procDesc=Procedures.GetDescription(proc);
					if(type!="") {
						type+="\r\n";
					}
					type+="Proc: "+procDesc;//Type
				}
				if(_listSplitsCur[i].UnearnedType!=0 && _listSplitsCur[i].ProvNum==0) {//Unattached split
					if(type!="") {
						type+="\r\n";
					}
					type+="Unallocated";//Type
				}
				else if(_listSplitsCur[i].UnearnedType!=0 && _listSplitsCur[i].ProvNum!=0) {
					if(type!="") {
						type+="\r\n";
					}
					type+=Defs.GetName(DefCat.PaySplitUnearnedType,_listSplitsCur[i].UnearnedType);//Type
				}
				else if(_listSplitsCur[i].AdjNum!=0) {
					if(type!="") {
						type+="\r\n";
					}
					type+="Adjustment";
				}
				row.Cells.Add(type);
				if(row.Cells[row.Cells.Count-1].Text=="Unallocated") {
					row.Cells[row.Cells.Count-1].ColorText=System.Drawing.Color.Red;
				}
				row.Cells.Add(_listSplitsCur[i].SplitAmt.ToString("f"));//Amount
				gridSplits.ListGridRows.Add(row);
			}
			textSplitTotal.Text=splitTotal.ToString("f");
			gridSplits.EndUpdate();
			FillGridCharges();
			FillGridTreatPlan();
			FillGridAllocated();
		}

		///<summary>Fills charge grid, and then split grid.</summary>
		private void FillGridCharges() {
			//Fill right-hand grid with all the charges, filtered based on checkbox and filters.
			gridCharges.BeginUpdate();
			gridCharges.ListGridColumns.Clear();
			GridColumn col;
			decimal chargeTotal=0;
			#region Group By Provider
			if(comboGroupBy.SelectedIndex==1) {//Group by 'Provider'
				col=new GridColumn(Lan.g(this,"Prov"),checkPayTypeNone.Checked?70:110);
				gridCharges.ListGridColumns.Add(col);
				if(checkPayTypeNone.Checked) {
					col=new GridColumn(Lan.g(this,"Patient"),119);
					gridCharges.ListGridColumns.Add(col);
				}
				col=new GridColumn(Lan.g(this,"Codes"),checkPayTypeNone.Checked?170:249);
				gridCharges.ListGridColumns.Add(col);
				col=new GridColumn(Lan.g(this,"Amt Orig"),70,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
				gridCharges.ListGridColumns.Add(col);
				col=new GridColumn(Lan.g(this,"Amt Start"),70,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
				gridCharges.ListGridColumns.Add(col);
				col=new GridColumn(Lan.g(this,"Amt End"),70,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
				gridCharges.ListGridColumns.Add(col);
				gridCharges.ListGridRows.Clear();
				//Item1=ProvNum, Item2=PatNum
				List<Tuple<long,long>> listAddedProvNums=new List<Tuple<long,long>>();//this needs to be prov/patnum
				foreach(AccountEntry entryCharge in _listAccountCharges) {
					if(Math.Round(entryCharge.AmountStart,3)==0) {
						continue;
					}
					if(!DoShowAccountEntry(entryCharge)) {
						continue;
					}
					if(listAddedProvNums.Any(x => x.Item1==entryCharge.ProvNum && x.Item2==entryCharge.PatNum)) {
						continue;
					}
					listAddedProvNums.Add(Tuple.Create(entryCharge.ProvNum,entryCharge.PatNum));
					List<AccountEntry> listEntriesForProvAndPatientAll=_listAccountCharges.FindAll(x => x.ProvNum==entryCharge.ProvNum 
						&& x.PatNum==entryCharge.PatNum);
					List<AccountEntry> listCompletedEntries=listEntriesForProvAndPatientAll
						.FindAll(x => x.GetType()==typeof(Procedure) && ((Procedure)x.Tag).ProcStatus==ProcStat.C);
					listCompletedEntries.AddRange(listEntriesForProvAndPatientAll.FindAll(x => x.GetType()!=typeof(Procedure)));
					if(Math.Round(listCompletedEntries.Sum(x => x.AmountStart),3)!=0) {
						gridCharges.ListGridRows.Add(FillChargesHelper(listCompletedEntries,entryCharge,false));
						chargeTotal+=listCompletedEntries.Sum(x => x.AmountEnd);
					}
				}
			}
			#endregion
			#region Group By Clinic and Provider
			else if(comboGroupBy.SelectedIndex==2) {//Group by 'Clinic and Provider'
				col=new GridColumn(Lan.g(this,"Prov"),checkPayTypeNone.Checked?70:100);
				gridCharges.ListGridColumns.Add(col);
				if(checkPayTypeNone.Checked) {
					col=new GridColumn(Lan.g(this,"Patient"),100);
					gridCharges.ListGridColumns.Add(col);
				}
				col=new GridColumn(Lan.g(this,"Clinic"),60);
				gridCharges.ListGridColumns.Add(col);
				col=new GridColumn(Lan.g(this,"Codes"),checkPayTypeNone.Checked?130:200);
				gridCharges.ListGridColumns.Add(col);
				col=new GridColumn(Lan.g(this,"Amt Orig"),70,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
				gridCharges.ListGridColumns.Add(col);
				col=new GridColumn(Lan.g(this,"Amt Start"),70,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
				gridCharges.ListGridColumns.Add(col);
				col=new GridColumn(Lan.g(this,"Amt End"),70,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
				gridCharges.ListGridColumns.Add(col);
				gridCharges.ListGridRows.Clear();
				//Item1=ProvNum,Item2=ClinicNum,Item3=PatNum
				List<Tuple<long,long,long>> listAddedProvNums=new List<Tuple<long,long,long>>();//this needs to be clinic/prov/patnum
				foreach(AccountEntry entryCharge in _listAccountCharges) {
					if(Math.Round(entryCharge.AmountStart,3)==0) {
						continue;
					}
					if(!DoShowAccountEntry(entryCharge)) {
						continue;
					}
					if(listAddedProvNums.Any(x => x.Item1==entryCharge.ProvNum && x.Item2==entryCharge.ClinicNum && x.Item3==entryCharge.PatNum)) {
						continue;
					}
					listAddedProvNums.Add(Tuple.Create(entryCharge.ProvNum,entryCharge.ClinicNum,entryCharge.PatNum));
					//we do this in this way so we can ensure that any TP procedures that we may have will show AFTER the completed ones.
					List<AccountEntry> listEntriesForProvAndClinicAndPatientAll=_listAccountCharges.FindAll(x => x.ProvNum==entryCharge.ProvNum 
						&& x.ClinicNum==entryCharge.ClinicNum && x.PatNum==entryCharge.PatNum);
					List<AccountEntry> listCompletedEntries=listEntriesForProvAndClinicAndPatientAll.FindAll(x => x.GetType()==typeof(Procedure) 
						&& ((Procedure)x.Tag).ProcStatus==ProcStat.C);
					listCompletedEntries.AddRange(listEntriesForProvAndClinicAndPatientAll.FindAll(x => x.GetType()!=typeof(Procedure)));
					if(Math.Round(listCompletedEntries.Sum(x => x.AmountStart),3)!=0) {
						gridCharges.ListGridRows.Add(FillChargesHelper(listCompletedEntries,entryCharge,true));
						chargeTotal+=listCompletedEntries.Sum(x => x.AmountEnd);
					}
				}
			}
			#endregion
			#region Group By None
			else { //Group by 'None'
				col=new GridColumn(Lan.g(this,"Date"),65,GridSortingStrategy.DateParse);
				gridCharges.ListGridColumns.Add(col);
				col=new GridColumn(Lan.g(this,"Patient"),92,GridSortingStrategy.StringCompare);
				gridCharges.ListGridColumns.Add(col);
				col=new GridColumn(Lan.g(this,"Prov"),40,GridSortingStrategy.StringCompare);
				gridCharges.ListGridColumns.Add(col);
				if(PrefC.HasClinicsEnabled) {//Clinics
					col=new GridColumn(Lan.g(this,"Clinic"),55,GridSortingStrategy.StringCompare);
					gridCharges.ListGridColumns.Add(col);
				}
				col=new GridColumn(Lan.g(this,"Code"),45,GridSortingStrategy.StringCompare);
				gridCharges.ListGridColumns.Add(col);
				col=new GridColumn(Lan.g(this,"Tth"),25,GridSortingStrategy.ToothNumberParse);
				gridCharges.ListGridColumns.Add(col);
				col=new GridColumn(Lan.g(this,"Type"),90,GridSortingStrategy.StringCompare);
				gridCharges.ListGridColumns.Add(col);
				col=new GridColumn(Lan.g(this,"AmtOrig"),55,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
				gridCharges.ListGridColumns.Add(col);
				col=new GridColumn(Lan.g(this,"AmtStart"),57,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
				gridCharges.ListGridColumns.Add(col);
				col=new GridColumn(Lan.g(this,"AmtEnd"),55,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
				gridCharges.ListGridColumns.Add(col);
				gridCharges.ListGridRows.Clear();
				GridRow row;
				for(int i=0;i<_listAccountCharges.Count;i++) {
					AccountEntry entryCharge=_listAccountCharges[i];
					if(entryCharge.GetType()==typeof(Procedure) && ((Procedure)entryCharge.Tag).ProcStatus==ProcStat.TP) {
						continue;
					}
					if(!checkShowAll.Checked && Math.Round(entryCharge.AmountStart,3)==0) {
						continue;
					}
					if(!DoShowAccountEntry(entryCharge)) {
						continue;
					}
					if(!checkShowAll.Checked) {//Filter out those that are paid in full and from other payments if checkbox unchecked.
						bool isFound=false;
						if(Math.Round(entryCharge.AmountEnd,3)!=0) {
							isFound=true;
						}
						for(int j = 0;j<gridSplits.ListGridRows.Count;j++) {
							PaySplit entryCredit=(PaySplit)gridSplits.ListGridRows[j].Tag;
							if(entryCharge.SplitCollection.Contains(entryCredit)) {
							//Charge is paid for by a split in this payment, display it.
								if(entryCharge.GetType()==typeof(Procedure) && entryCredit.PayPlanNum!=0) {
									//Don't show the charge if it's a proc being paid by a payplan split.
									//From the user's perspective they're paying the "debits" not the procs.
								}
								else {
									isFound=true;
									break;
								}
							}
							else if(entryCharge.GetType()==typeof(PayPlanCharge) && entryCredit.PayPlanNum==((PayPlanCharge)entryCharge.Tag).PayPlanNum && Math.Round(entryCharge.AmountStart,3)!=0) {
								isFound=true;
								break;
							}
						}
						if(!isFound) {//Hiding charges that aren't associated with the current payment or have been paid in full.
							continue;
						}
					}
					row=new GridRow();
					row.Tag=_listAccountCharges[i];
					row.Cells.Add(entryCharge.Date.ToShortDateString());//Date
					Patient patCur;
					if(!_dictPatients.TryGetValue(entryCharge.PatNum,out patCur)) {
						patCur=Patients.GetLim(entryCharge.PatNum);
						_dictPatients[patCur.PatNum]=patCur;
					}
					string patName=patCur.LName + ", " + patCur.FName;
					if(entryCharge.Tag.GetType()==typeof(PayPlanChargeType)) {
						patName+="\r\n"+Lan.g(this,"Guar")+": "+_dictPatients[((PayPlanCharge)entryCharge.Tag).Guarantor].LName +", "
							+ _dictPatients[((PayPlanCharge)entryCharge.Tag).Guarantor].FName;
					}
					row.Cells.Add(patName);//Patient
					row.Cells.Add(Providers.GetAbbr(entryCharge.ProvNum));//Provider
					if(PrefC.HasClinicsEnabled) {//Clinics
						row.Cells.Add(Clinics.GetAbbr(entryCharge.ClinicNum));
					}
					string procCode="";
					string tth="";
					Procedure proc=null;
					if(entryCharge.Tag.GetType()==typeof(Procedure)) {
						proc=(Procedure)entryCharge.Tag;
						tth=proc.ToothNum=="" ? proc.Surf : Tooth.ToInternat(proc.ToothNum);
						procCode+=ProcedureCodes.GetStringProcCode(proc.CodeNum);
					}
					row.Cells.Add(procCode);//ProcCode
					row.Cells.Add(tth);
					if(entryCharge.GetType()==typeof(PaySplit)) {
						row.Cells.Add("Unallocated");
					}
					else {
						row.Cells.Add(entryCharge.GetType().Name);//Type
					}
					if(entryCharge.GetType()==typeof(Procedure)) {
						//Get the proc and add its description if the row is a proc.
						row.Cells[row.Cells.Count-1].Text=Lan.g(this,"Proc")+": "+Procedures.GetDescription(proc);
					}
					row.Cells.Add(entryCharge.AmountOriginal.ToString("f"));//Amount Original
					row.Cells.Add(entryCharge.AmountStart.ToString("f"));//Amount Start
					row.Cells.Add(entryCharge.AmountEnd.ToString("f"));//Amount End
					chargeTotal+=entryCharge.AmountEnd;
					gridCharges.ListGridRows.Add(row);
				}
			}
			#endregion
			textChargeTotal.Text=chargeTotal.ToString("f");
			gridCharges.EndUpdate();
		}

		///<summary>Fills Treatment Plan Procedures grid.</summary>
		private void FillGridTreatPlan() {
			//Fill right-hand grid with all the TP procedures.
			gridTreatPlan.BeginUpdate();
			gridTreatPlan.ListGridColumns.Clear();
			GridColumn col;
			#region Group By None
			col=new GridColumn(Lan.g(this,"Date"),65,GridSortingStrategy.DateParse);
			gridTreatPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Patient"),92,GridSortingStrategy.StringCompare);
			gridTreatPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Prov"),40,GridSortingStrategy.StringCompare);
			gridTreatPlan.ListGridColumns.Add(col);
			if(PrefC.HasClinicsEnabled) {//Clinics
				col=new GridColumn(Lan.g(this,"Clinic"),55,GridSortingStrategy.StringCompare);
				gridTreatPlan.ListGridColumns.Add(col);
			}
			col=new GridColumn(Lan.g(this,"Code"),45,GridSortingStrategy.StringCompare);
			gridTreatPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Tth"),25,GridSortingStrategy.ToothNumberParse);
			gridTreatPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Type"),90,GridSortingStrategy.StringCompare);
			gridTreatPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"AmtOrig"),55,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
			gridTreatPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"AmtStart"),57,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
			gridTreatPlan.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"AmtEnd"),55,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
			gridTreatPlan.ListGridColumns.Add(col);
			gridTreatPlan.ListGridRows.Clear();
			GridRow row;
			for(int i=0;i<_listAccountCharges.Count;i++) {
				AccountEntry entryCharge=_listAccountCharges[i];
				if(entryCharge.GetType()!=typeof(Procedure) || ((Procedure)entryCharge.Tag).ProcStatus!=ProcStat.TP) {
					continue;
				}
				if(!DoShowAccountEntry(entryCharge)) {
					continue;
				}
				row=new GridRow();
				row.Tag=_listAccountCharges[i];
				row.Cells.Add(entryCharge.Date.ToShortDateString());//Date
				Patient patCur;
				if(!_dictPatients.TryGetValue(entryCharge.PatNum,out patCur)) {
					patCur=Patients.GetLim(entryCharge.PatNum);
					_dictPatients[patCur.PatNum]=patCur;
				}
				string patName=patCur.LName + ", " + patCur.FName;
				if(entryCharge.Tag.GetType()==typeof(PayPlanChargeType)) {
					patName+="\r\n"+Lan.g(this,"Guar")+": "+_dictPatients[((PayPlanCharge)entryCharge.Tag).Guarantor].LName +", "
						+ _dictPatients[((PayPlanCharge)entryCharge.Tag).Guarantor].FName;
				}
				row.Cells.Add(patName);//Patient
				row.Cells.Add(Providers.GetAbbr(entryCharge.ProvNum));//Provider
				if(PrefC.HasClinicsEnabled) {//Clinics
					row.Cells.Add(Clinics.GetAbbr(entryCharge.ClinicNum));
				}
				string procCode="";
				string tth="";
				Procedure proc=null;
				if(entryCharge.Tag.GetType()==typeof(Procedure)) {
					proc=(Procedure)entryCharge.Tag;
					tth=proc.ToothNum=="" ? proc.Surf : Tooth.ToInternat(proc.ToothNum);
					procCode+=ProcedureCodes.GetStringProcCode(proc.CodeNum);
				}
				row.Cells.Add(procCode);//ProcCode
				row.Cells.Add(tth);//Tooth Number
				if(entryCharge.GetType()==typeof(PaySplit)) {
					row.Cells.Add("Unallocated");
				}
				else {
					row.Cells.Add(entryCharge.GetType().Name);//Type
				}
				if(entryCharge.GetType()==typeof(Procedure)) {
					//Get the proc and add its description if the row is a proc.
					row.Cells[row.Cells.Count-1].Text=Lan.g(this,"Proc")+": "+Procedures.GetDescription(proc);
				}
				row.Cells.Add(entryCharge.AmountOriginal.ToString("f"));//Amount Original
				row.Cells.Add(entryCharge.AmountStart.ToString("f"));//Amount Start
				row.Cells.Add(entryCharge.AmountEnd.ToString("f"));//Amount End
				gridTreatPlan.ListGridRows.Add(row);
			}
			#endregion
			gridTreatPlan.EndUpdate();
		}

		private GridRow FillChargesHelper(List<AccountEntry> listEntriesForRow,AccountEntry entryCharge,bool includeClinic) {
			GridRow row=new GridRow();
			row.Tag=listEntriesForRow;
			row.Cells.Add(Providers.GetAbbr(entryCharge.ProvNum));//Provider
			if(checkPayTypeNone.Checked) {
				Patient pat;
				if(!_dictPatients.TryGetValue(entryCharge.PatNum,out pat)) {
					pat=Patients.GetLim(entryCharge.PatNum);
					_dictPatients[pat.PatNum]=pat;
				}
				row.Cells.Add(pat.LName+", "+pat.FName);//patient
			}
			if(includeClinic) {
				row.Cells.Add(Clinics.GetAbbr(entryCharge.ClinicNum));
			}
			string procCodes="";
			int addedCodes=0;
			for(int i=0;i<listEntriesForRow.Count;i++) {
				if(Math.Round(listEntriesForRow[i].AmountStart,3)==0) {
					continue;
				}
				if(listEntriesForRow[i].Tag.GetType()==typeof(Procedure)) {
					Procedure procedure=(Procedure)listEntriesForRow[i].Tag;
					if(procCodes!="") {
						procCodes+=", ";
					}
					if(procedure.ProcStatus==ProcStat.TP) {
						procCodes+="(TP)";//this needs to be handled differently. TP Procs need to be in their own provider grouping
					}
					procCodes+=ProcedureCodes.GetStringProcCode(procedure.CodeNum);
					addedCodes++;
				}
				if((includeClinic && addedCodes==9) || (!includeClinic && addedCodes==10)) {//this column is shorter when filtering by prov + clinic
					procCodes+=", (...)";
					break;
				}
			}
			row.Cells.Add(procCodes);//ProcCodes
			row.Cells.Add(listEntriesForRow.Sum(x => x.AmountOriginal).ToString("f"));//Amount Original
			row.Cells.Add(listEntriesForRow.Sum(x => x.AmountStart).ToString("f"));//Amount Start
			row.Cells.Add(listEntriesForRow.Sum(x => x.AmountEnd).ToString("f"));//Amount End
			return row;
		}

		///<summary>Returns true if the AccountEntry matches the currently selected filters.</summary>
		private bool DoShowAccountEntry(AccountEntry entryCharge) { 
			if(entryCharge.GetType()==typeof(PayPlanCharge)) {
				if(!_listFilteredPatNums.Contains(((PayPlanCharge)entryCharge.Tag).PatNum))	{
					return false;
				}
			}
			if(!_listFilteredPatNums.Contains(entryCharge.PatNum)) {
				return false;
			}
			List<long> listProvNums=comboProviderFilter.GetSelectedProvNums();
			if(!listProvNums.Contains(entryCharge.ProvNum)) {
				return false;
			}
			if(PrefC.HasClinicsEnabled && comboGroupBy.SelectedIndex!=1 && !_listFilteredClinics.Contains(entryCharge.ClinicNum)) {
				return false;
			}
			//proc code filter
			if(_listFilteredProcCodes.Count>0
				&& (entryCharge.Tag.GetType()!=typeof(Procedure) || !_listFilteredProcCodes.Contains(((Procedure)entryCharge.Tag).CodeNum)))
			{
				return false;
			}
			//Charge Amount Filter
			if(amtMaxEnd.Value!=0 && entryCharge.AmountEnd > amtMaxEnd.Value) {
				return false;
			}
			//Charge Amount Filter
			if(amtMinEnd.Value!=0 && entryCharge.AmountEnd < amtMinEnd.Value) {
				return false;
			}
			//daterange filter
			if((entryCharge.Date.Date < datePickFrom.Value.Date) || (entryCharge.Date.Date > datePickTo.Value.Date)) { 
				return false;
			}
			//Type Filter
			if(!_listFilteredType.Contains(entryCharge.GetType().Name)) {
				return false;
			}
			return true;
		}

		///<summary>Fills the combo boxes with correct values.</summary>
		private void FillFilters() {
			//Fill min/max date/amt
			//If there are no account charges, the date time will be DateTime.MinimumDate
			//Get latest proc, or tomorows date, whichever is later.
			if(_listAccountCharges.Count(x => x.Date>=datePickFrom.MinDate)==0) {
				datePickFrom.Value=datePickFrom.Value;
				datePickTo.Value=DateTime.Today;
			}
			else {
				datePickFrom.Value=_listAccountCharges.Where(x => x.Date>=datePickFrom.MinDate).Min(x => x.Date);
				datePickTo.Value=ODMathLib.Max(_listAccountCharges.Max(x => x.Date),DateTime.Today);
			}
			amtMinEnd.Value=0;
			amtMaxEnd.Value=0; 
			//Fill Patient Combo
			comboPatientFilter.Items.Clear();
			comboPatientFilter.Items.Add(new ODBoxItem<Patient>(Lan.g(this,"All")));
			foreach(Patient pat in _listAccountCharges.Where(x => x.PatNum.In(_listPatNums) 
					//Also include patients outside of the family if someone in the family is the guarantor of their payplan.
					|| (x.Tag.GetType()==typeof(PayPlanCharge) && ((PayPlanCharge)x.Tag).Guarantor.In(_listPatNums)))
				.Where(x => _dictPatients.ContainsKey(x.PatNum)).Select(x => _dictPatients[x.PatNum])
				.DistinctBy(x => x.PatNum)) 
			{
				comboPatientFilter.Items.Add(new ODBoxItem<Patient>(pat.GetNameFirstOrPreferred(),pat));
			}
			comboPatientFilter.SetSelected(0,true);
			//Fill Provider Combo
			comboProviderFilter.Items.Clear();
			comboProviderFilter.IncludeAll=true;
			comboProviderFilter.Items.AddProvNone();
			List<Provider> listProviders=_listAccountCharges.Select(x => Providers.GetFirstOrDefault(y => y.ProvNum==x.ProvNum))
				.Where(x => x!=null)
				.DistinctBy(x => x.ProvNum)
				.ToList();
			comboProviderFilter.Items.AddProvsAbbr(listProviders);
			comboProviderFilter.IsAllSelected=true;
			//Fill Clinics Combo
			if(PrefC.HasClinicsEnabled) {
				List<Clinic> listClinicsTemp=Clinics.GetDeepCopy();
				listClinicsTemp.Add(new Clinic() {Abbr=Lan.g(this,"Unassigned")});
				comboClinicFilter.Items.Clear();
				comboClinicFilter.Items.Add(new ODBoxItem<Clinic>(Lan.g(this,"All")));
				foreach(Clinic clinic in _listAccountCharges.Select(x => listClinicsTemp.FirstOrDefault(y => y.ClinicNum==x.ClinicNum))
					.DistinctBy(x => x.ClinicNum))
				{
					comboClinicFilter.Items.Add(new ODBoxItem<Clinic>(clinic.Abbr,clinic));
				}
				comboClinicFilter.SetSelected(0,true);
			}
			//Fill Type Combo
			comboTypeFilter.Items.Clear();
			comboTypeFilter.Items.Add(Lan.g(this,"All"));
			comboTypeFilter.Items.AddRange(_listAccountCharges.Select(x => x.GetType().Name).Distinct().ToList());
			comboTypeFilter.SetSelected(0,true);
		}
		
		///<summary>Called whenever any of the filtering objects are changed.  Rechecks filtering and refreshes the grid.</summary>
		private void FilterChangeCommitted(object sender, EventArgs e) {
			if(_isInit) {
				return;
			}
			FillGridCharges();
			FillGridTreatPlan();
		}

		///<summary>Adds one split to _listPaySplits to work with.  Does not link the payment plan, that must be done outside this method.
		///Called when checkPayPlan click, or upon load if auto attaching to payplan, or upon OK click if no splits were created.</summary>
		private bool AddOneSplit(bool promptForPayPlan = false) {
			PaySplit paySplitCur=new PaySplit();
			paySplitCur.PatNum=_patCur.PatNum;
			paySplitCur.PayNum=_paymentCur.PayNum;
			paySplitCur.DatePay=_paymentCur.PayDate;//this may be updated upon closing
			if(PrefC.GetInt(PrefName.RigorousAccounting) < (int)RigorousAccounting.DontEnforce) {
				paySplitCur.ProvNum=0;
				paySplitCur.UnearnedType=PrefC.GetLong(PrefName.PrepaymentUnearnedType);//Use default unallocated type
			}
			else { 
				paySplitCur.ProvNum=Patients.GetProvNum(_patCur);
			}
			paySplitCur.ClinicNum=_paymentCur.ClinicNum;
			paySplitCur.SplitAmt=PIn.Double(textAmount.Text);
			if(promptForPayPlan && _listValidPayPlans.Count > 0) {
				FormPayPlanSelect FormPPS = new FormPayPlanSelect(_listValidPayPlans,true);
				FormPPS.ShowDialog();
				if(FormPPS.DialogResult!=DialogResult.OK) {
					return false;
				}
				paySplitCur.PayPlanNum=FormPPS.SelectedPayPlanNum;
			}
			_listSplitsCur.Add(paySplitCur);
			_paymentCur.PayAmt=PIn.Double(textAmount.Text);
			return true;
		}

		private void listPayType_Click(object sender,EventArgs e) {
			if(Plugins.HookMethod(this,"FormPayment.listPayType_Click",listPayType.SelectedItem)) {
				return;
			}
			textDepositAccount.Visible=false;
			SetComboDepositAccounts();
		}

		///<summary>Called from all 4 places where listPayType gets changed.</summary>
		private void SetComboDepositAccounts() {
			if(listPayType.SelectedIndex==-1 || checkPayTypeNone.Checked) {
				if(IsNew && (PayClinicSetting)PrefC.GetInt(PrefName.PaymentClinicSetting)==PayClinicSetting.PatientDefaultClinic) {
					labelDepositAccount.Visible=false;
					comboDepositAccount.Visible=false;
				}
				return;
			}
			AccountingAutoPay autoPay=AccountingAutoPays.GetForPayType(
				_listPaymentTypeDefs[listPayType.SelectedIndex].DefNum);
			if(autoPay==null) {
				labelDepositAccount.Visible=false;
				comboDepositAccount.Visible=false;
			}
			else {
				labelDepositAccount.Visible=true;
				comboDepositAccount.Visible=true;
				_arrayDepositAcctNums=AccountingAutoPays.GetPickListAccounts(autoPay);
				comboDepositAccount.Items.Clear();
				for(int i=0;i<_arrayDepositAcctNums.Length;i++) {
					comboDepositAccount.Items.Add(Accounts.GetDescript(_arrayDepositAcctNums[i]));
				}
				if(comboDepositAccount.Items.Count>0) {
					comboDepositAccount.SelectedIndex=0;
				}
			}
		}

		private void ToggleShowHideSplits() {
			splitContainerCharges.Panel2Collapsed = !splitContainerCharges.Panel2Collapsed;
			if(splitContainerCharges.Panel2Collapsed) {
				butShowHide.Text=Lan.g(this,"Show Splits");
				Height = splitContainerCharges.SplitterDistance+100;//Plus 100 to give room for the buttons
				this.butShowHide.Image = global::OpenDental.Properties.Resources.arrowDownTriangle;
			}
			else {
				butShowHide.Text=Lan.g(this,"Hide Splits");
				Height = _originalHeight;
				this.butShowHide.Image = global::OpenDental.Properties.Resources.arrowUpTriangle;
			}
		}

		private void panelXcharge_MouseClick(object sender,MouseEventArgs e) {
			if(e.Button != MouseButtons.Left) {
				return;
			}
			if(!CanAddNewCreditCard(Programs.GetCur(ProgramName.Xcharge),ProgramProperties.PropertyDescs.XCharge.XChargePreventSavingNewCC)) {
				return;
			}
			_xChargeMilestone="";
			try {
				MakeXChargeTransaction();
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g(this,"Error processing transaction.\r\n\r\nPlease contact support with the details of this error:")
					//The rest of the message is not translated on purpose because we here at HQ need to always be able to quickly read this part.
					+"\r\nLast valid milestone reached: "+_xChargeMilestone,ex);
			}
		}

		///<summary>Launches the XCharge transaction window and then executes whatever type of transaction was selected for the current payment.
		///This is to help troubleshooting. Returns null upon failure, otherwise returns the transaction detail as a string.
		///If prepaidAmt is not zero, then will show the xcharge window with the given prepaid amount and let the user enter card # and exp.
		///A patient is not required for prepaid cards.</summary>
		public string MakeXChargeTransaction(double prepaidAmt=0) {
			if(ODBuild.IsWeb()) {
				MsgBox.Show(this,"XCharge is not available while viewing through the web.");
				return null;
			}
			//Need to refresh this list locally in case we are coming from another form
			_listPaymentTypeDefs=_listPaymentTypeDefs??Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
			_xChargeMilestone="Validation";
			CreditCard cc=null;
			List<CreditCard> creditCards=null;
			if(prepaidAmt!=0) {
				CheckUIState();//To ensure that _xProg is set and _xPath is set.  Normally this would happen when loading.  Needed for HasXCharge().
			}
			if(!HasXCharge()) {//Will show setup window if xcharge is not enabled or not completely setup yet.
				return null;
			}
			if(prepaidAmt==0) {//Validation for regular credit cards (not prepaid cards).
				if(textAmount.Text=="" || PIn.Double(textAmount.Text)==0) {
					MsgBox.Show(this,"Please enter an amount first.");
					textAmount.Focus();
					return null;
				}
				creditCards=CreditCards.Refresh(_patCur.PatNum);
				if(comboCreditCards.SelectedIndex < creditCards.Count && comboCreditCards.SelectedIndex >-1) {
					cc=creditCards[comboCreditCards.SelectedIndex];
				}
				if(cc!=null && cc.IsXWeb()) {
					MsgBox.Show(this,"Cards saved through XWeb cannot be used with the XCharge client program.");
					return null;
				}
				if(_listSplitsCur.Count>0 && PIn.Double(textAmount.Text)!=PIn.Double(textSplitTotal.Text)
					&& (_listSplitsCur.Count!=1 || _listSplitsCur[0].PayPlanNum==0)) //Not one paysplit attached to payplan
				{
					MsgBox.Show(this,"Split totals must equal payment amount before running a credit card transaction.");
					return null;
				}
			}
			if(PIn.Date(textDate.Text).Date > DateTime.Today.Date
					&& !PrefC.GetBool(PrefName.FutureTransDatesAllowed) && !PrefC.GetBool(PrefName.AccountAllowFutureDebits))
			{
				MsgBox.Show(this,"Payment date cannot be in the future.");
				return null;
			}
			_xChargeMilestone="XResult File";
			string resultfile=PrefC.GetRandomTempFile("txt");
			try {
				File.Delete(resultfile);//delete the old result file.
			}
			catch {
				MsgBox.Show(this,"Could not delete XResult.txt file.  It may be in use by another program, flagged as read-only, or you might not have "
					+"sufficient permissions.");
				return null;
			}
			_xChargeMilestone="Properties";
			bool needToken=false;
			bool newCard=false;
			bool hasXToken=false;
			bool notRecurring=false;
			if(prepaidAmt==0) {
				//These UI changes only need to happen for regular credit cards when the payment window is displayed.
				string xPayTypeNum=ProgramProperties.GetPropVal(_xProg.ProgramNum,"PaymentType",_paymentCur.ClinicNum);
				//still need to add functionality for accountingAutoPay
				listPayType.SelectedIndex=Defs.GetOrder(DefCat.PaymentTypes,PIn.Long(xPayTypeNum));
				SetComboDepositAccounts();
			}
			/*XCharge.exe [/TRANSACTIONTYPE:type] [/AMOUNT:amount] [/ACCOUNT:account] [/EXP:exp]
				[“/TRACK:track”] [/ZIP:zip] [/ADDRESS:address] [/RECEIPT:receipt] [/CLERK:clerk]
				[/APPROVALCODE:approval] [/AUTOPROCESS] [/AUTOCLOSE] [/STAYONTOP] [/MID]
				[/RESULTFILE:”C:\Program Files\X-Charge\LocalTran\XCResult.txt”*/
			ProcessStartInfo info=new ProcessStartInfo(_xPath);
			Patient pat=null;
			if(prepaidAmt==0) {
				pat=Patients.GetPat(_paymentCur.PatNum);
				if(pat==null) {
					MsgBox.Show(this,"Invalid patient associated to this payment.");
					return null;
				}
			}
			info.Arguments="";
			double amt=PIn.Double(textAmount.Text);
			if(prepaidAmt != 0) {
				amt=prepaidAmt;
			}
			if(amt<0) {//X-Charge always wants a positive number, even for returns.
				amt*=-1;
			}
			info.Arguments+="/AMOUNT:"+amt.ToString("F2")+" ";
			_xChargeMilestone="Get Selected Credit Card";
			FormXchargeTrans FormXT=null;
			int tranType=0;//Default to 0 "Purchase" for prepaid cards.
			string cashBack=null;
			if(prepaidAmt==0) {//All regular cards (not prepaid)
				_xChargeMilestone="Transaction Window Launch";
				//Show window to lock in the transaction type.
				FormXT=new FormXchargeTrans();
				FormXT.PrintReceipt=PIn.Bool(ProgramProperties.GetPropVal(_xProg.ProgramNum,"PrintReceipt",_paymentCur.ClinicNum));
				FormXT.PromptSignature=PIn.Bool(ProgramProperties.GetPropVal(_xProg.ProgramNum,"PromptSignature",_paymentCur.ClinicNum));
				FormXT.ClinicNum=_paymentCur.ClinicNum;
				FormXT.ShowDialog();
				if(FormXT.DialogResult!=DialogResult.OK) {
					return null;
				}
				_xChargeMilestone="Transaction Window Digest";
				_paymentCur.PaymentSource=CreditCardSource.XServer;
				_paymentCur.ProcessStatus=ProcessStat.OfficeProcessed;
				tranType=FormXT.TransactionType;
				decimal cashAmt=FormXT.CashBackAmount;
				cashBack=cashAmt.ToString("F2");
				_promptSignature=FormXT.PromptSignature;
				_printReceipt=FormXT.PrintReceipt;
			}
			_xChargeMilestone="Check Duplicate Cards";
			if(cc!=null && !string.IsNullOrEmpty(cc.XChargeToken)) {//Have CC on file with an XChargeToken
				hasXToken=true;
				if(CreditCards.GetXChargeTokenCount(cc.XChargeToken,false)!=1) {
					MsgBox.Show(this,"This card shares a token with another card. Delete it from the Credit Card Manage window and re-add it.");
					return null;
				}
				/*       ***** An example of how recurring charges work***** 
				C:\Program Files\X-Charge\XCharge.exe /TRANSACTIONTYPE:Purchase /LOCKTRANTYPE
				/AMOUNT:10.00 /LOCKAMOUNT /XCACCOUNTID:XAW0JWtx5kjG8 /RECEIPT:RC001
				/LOCKRECEIPT /CLERK:Clerk /LOCKCLERK /RESULTFILE:C:\ResultFile.txt /USERID:system
				/PASSWORD:system /STAYONTOP /AUTOPROCESS /AUTOCLOSE /HIDEMAINWINDOW
				/RECURRING /SMALLWINDOW /NORESULTDIALOG
				*/
			}
			else if(cc!=null) {//Have CC on file, no XChargeToken so not a recurring charge, and might need a token.
				notRecurring=true;
				if(!PrefC.GetBool(PrefName.StoreCCnumbers)) {//Use token only if user has has pref unchecked in module setup (allow store credit card nums).
					needToken=true;//Will create a token from result file so credit card info isn't saved in our db.
				}
			}
			else {//CC is null, add card option was selected in credit card drop down, no other possibility.
				newCard=true;
			}
			_xChargeMilestone="Arguments Fill Card Info";
			info.Arguments+=GetXChargeTransactionTypeCommands(tranType,hasXToken,notRecurring,cc,cashBack);
			if(prepaidAmt!=0) {
				//Zip and address are optional fields and for prepaid cards this information is probably not provided to the user.
			}
			else if(newCard) {
				info.Arguments+="\"/ZIP:"+pat.Zip+"\" ";
				info.Arguments+="\"/ADDRESS:"+pat.Address+"\" ";
			}
			else {
				if(cc.CCExpiration!=null && cc.CCExpiration.Year>2005) {
					info.Arguments+="/EXP:"+cc.CCExpiration.ToString("MMyy")+" ";
				}
				if(!string.IsNullOrEmpty(cc.Zip)) {
					info.Arguments+="\"/ZIP:"+cc.Zip+"\" ";
				}
				else {
					info.Arguments+="\"/ZIP:"+pat.Zip+"\" ";
				}
				if(!string.IsNullOrEmpty(cc.Address)) {
					info.Arguments+="\"/ADDRESS:"+cc.Address+"\" ";
				}
				else {
					info.Arguments+="\"/ADDRESS:"+pat.Address+"\" ";
				}
				if(hasXToken) {//Special parameter for tokens.
					info.Arguments+="/RECURRING ";
				}
			}
			_xChargeMilestone="Arguments Fill X-Charge Settings";
			if(prepaidAmt==0) {
				info.Arguments+="/RECEIPT:Pat"+_paymentCur.PatNum.ToString()+" ";//aka invoice#
			}
			else {
				info.Arguments+="/RECEIPT:PREPAID ";//aka invoice#
			}
			info.Arguments+="\"/CLERK:"+Security.CurUser.UserName+"\" /LOCKCLERK ";
			info.Arguments+="/RESULTFILE:\""+resultfile+"\" ";
			info.Arguments+="/USERID:"+ProgramProperties.GetPropVal(_xProg.ProgramNum,"Username",_paymentCur.ClinicNum)+" ";
			info.Arguments+="/PASSWORD:"+CodeBase.MiscUtils.Decrypt(ProgramProperties.GetPropVal(_xProg.ProgramNum,"Password",_paymentCur.ClinicNum))+" ";
			info.Arguments+="/PARTIALAPPROVALSUPPORT:T ";
			info.Arguments+="/AUTOCLOSE ";
			info.Arguments+="/HIDEMAINWINDOW ";
			info.Arguments+="/SMALLWINDOW ";
			info.Arguments+="/GETXCACCOUNTID ";
			info.Arguments+="/NORESULTDIALOG ";
			_xChargeMilestone="X-Charge Launch";
			Cursor=Cursors.WaitCursor;
			Process process=new Process();
			process.StartInfo=info;
			process.EnableRaisingEvents=true;
			process.Start();
			process.WaitForExit();
			_xChargeMilestone="X-Charge Complete";
			Thread.Sleep(200);//Wait 2/10 second to give time for file to be created.
			Cursor=Cursors.Default;
			string resulttext="";
			string line="";
			bool showApprovedAmtNotice=false;
			bool xAdjust=false;
			bool xReturn=false;
			bool xVoid=false;
			double approvedAmt=0;
			double additionalFunds=0;
			string xChargeToken="";
			string accountMasked="";
			string expiration="";
			string signatureResult="";
			string receipt="";
			bool isDigitallySigned=false;
			bool updateCard=false;
			string newAccount="";
			long creditCardNum;
			DateTime newExpiration=new DateTime();
			_xChargeMilestone="Digest XResult";
			try {
				using(TextReader reader=new StreamReader(resultfile)) {
					line=reader.ReadLine();
					/*Example of successful transaction:
						RESULT=SUCCESS
						TYPE=Purchase
						APPROVALCODE=000064
						ACCOUNT=XXXXXXXXXXXX6781
						ACCOUNTTYPE=VISA*
						AMOUNT=1.77
						AVSRESULT=Y
						CVRESULT=M
					*/
					while(line!=null) {
						if(!line.StartsWith("RECEIPT=")) {//Don't include the receipt string in the PayNote
							if(resulttext!="") {
								resulttext+="\r\n";
							}
							resulttext+=line;
						}
						if(line.StartsWith("RESULT=")) {
							if(line!="RESULT=SUCCESS") {
								//Charge was a failure and there might be a description as to why it failed. Continue to loop through line.
								while(line!=null) {
									line=reader.ReadLine();
									if(line!=null && !line.StartsWith("RECEIPT=")) {//Don't include the receipt string in the PayNote
										resulttext+="\r\n"+line;
									}
								}
								needToken=false;//Don't update CCard due to failure
								newCard=false;//Don't insert CCard due to failure
								_isCCDeclined=true;
								break;
							}
							if(tranType==1) {
								xReturn=true;
							}
							if(tranType==6) {
								xAdjust=true;
							}
							if(tranType==7) {
								xVoid=true;
							}
							_isCCDeclined=false;
						}
						if(line.StartsWith("APPROVEDAMOUNT=")) {
							approvedAmt=PIn.Double(line.Substring(15));
							if(approvedAmt != amt) {
								showApprovedAmtNotice=true;
							}
						}
						if(line.StartsWith("XCACCOUNTID=")) {
							xChargeToken=PIn.String(line.Substring(12));
						}
						if(line.StartsWith("ACCOUNT=")) {
							accountMasked=PIn.String(line.Substring(8));
						}
						if(line.StartsWith("EXPIRATION=")) {
							expiration=PIn.String(line.Substring(11));
						}
						if(line.StartsWith("ADDITIONALFUNDSREQUIRED=")) {
							additionalFunds=PIn.Double(line.Substring(24));
						}
						if(line.StartsWith("SIGNATURE=") && line.Length>10) {
							signatureResult=PIn.String(line.Substring(10));
							//A successful digitally signed signature will say SIGNATURE=C:\Users\Folder\Where\The\Signature\Is\Stored.bmp
							if(signatureResult!="NOT SUPPORTED" && signatureResult!="FAILED") {
								isDigitallySigned=true;
							}
						}
						if(line.StartsWith("RECEIPT=")) {
							receipt=PIn.String(line.Replace("RECEIPT=","").Replace("\\n","\n"));//The receipt from X-Charge escapes the newline characters
							if(isDigitallySigned) {
								//Replace X____________________________ with 'Electronically signed'
								receipt.Split('\n').ToList().FindAll(x => x.StartsWith("X___")).ForEach(x => x="Electronically signed");
							}
							receipt=receipt.Replace("\r","").Replace("\n","\r\n");//remove any existing \r's before replacing \n's with \r\n's
						}
						if(line=="XCACCOUNTIDUPDATED=T") {//Decline minimizer updated the account information since the last time this card was charged
							updateCard=true;
						}
						if(line.StartsWith("ACCOUNT=")) {
							newAccount=line.Substring("ACCOUNT=".Length);
						}
						if(line.StartsWith("EXPIRATION=")) {
							string expStr=line.Substring("EXPIRATION=".Length);//Expiration should be MMYY
							newExpiration=new DateTime(PIn.Int("20"+expStr.Substring(2)),PIn.Int(expStr.Substring(0,2)),1);//First day of the month
						}
						line=reader.ReadLine();
					}
					if(needToken && !string.IsNullOrEmpty(xChargeToken) && prepaidAmt==0) {//never save token for prepaid cards
						_xChargeMilestone="Update Token";
						DateTime expDate=new DateTime(PIn.Int("20"+expiration.Right(2)),PIn.Int(expiration.Left(2)),1);
						//If the stored CC used for this X-Charge payment has a PayConnect token, and X-Charge returns a different masked number or exp date, we
						//will clear out the PayConnect token since this CC no longer refers to the same card that was used to generate the PayConnect token.
						if(!string.IsNullOrEmpty(cc.PayConnectToken) //there is a PayConnect token for this saved CC
							&& Regex.IsMatch(cc.CCNumberMasked,@"X+[0-9]{4}") //the saved CC has a masked number with the pattern XXXXXXXXXXXX1234
							&& (cc.CCNumberMasked.Right(4)!=accountMasked.Right(4) //and either the last four digits don't match what X-Charge returned
									|| cc.CCExpiration.Year!=expDate.Year //or the exp year doesn't match that returned by X-Charge
									|| cc.CCExpiration.Month!=expDate.Month)) //or the exp month doesn't match that returned by X-Charge
						{
							cc.PayConnectToken="";
							cc.PayConnectTokenExp=DateTime.MinValue;
						}
						//Only way this code can be hit is if they have set up a credit card and it does not have a token.
						//So we'll use the created token from result file and assign it to the coresponding account.
						//Also will delete the credit card number and replace it with secure masked number.
						cc.XChargeToken=xChargeToken;
						cc.CCNumberMasked=accountMasked;
						cc.CCExpiration=expDate;
						cc.Procedures=PrefC.GetString(PrefName.DefaultCCProcs);
						cc.CCSource=CreditCardSource.XServer;
						CreditCards.Update(cc);
					}
					if(newCard && prepaidAmt==0) {//never save card information to the patient account for prepaid cards
						if(!string.IsNullOrEmpty(xChargeToken) && FormXT.SaveToken) {
							_xChargeMilestone="Create New Credit Card Entry";
							cc=new CreditCard();
							List<CreditCard> itemOrderCount=CreditCards.Refresh(_patCur.PatNum);
							cc.ItemOrder=itemOrderCount.Count;
							cc.PatNum=_patCur.PatNum;
							cc.CCExpiration=new DateTime(Convert.ToInt32("20"+expiration.Substring(2,2)),Convert.ToInt32(expiration.Substring(0,2)),1);
							cc.XChargeToken=xChargeToken;
							cc.CCNumberMasked=accountMasked;
							cc.Procedures=PrefC.GetString(PrefName.DefaultCCProcs);
							cc.CCSource=CreditCardSource.XServer;
							cc.ClinicNum=_paymentCur.ClinicNum;
							creditCardNum=CreditCards.Insert(cc);
						}
						else if(string.IsNullOrEmpty(xChargeToken)) {//Shouldn't happen again but leaving just in case.
							MsgBox.Show(this,"X-Charge didn't return a token so credit card information couldn't be saved.");
						}
					}
					if(updateCard && newAccount!="" && newExpiration.Year>1880 && prepaidAmt==0) {//Never save credit card info to patient for prepaid cards.
						if(textNote.Text!="") {
							textNote.Text+="\r\n";
						}
						if(cc.CCNumberMasked != newAccount) {
							textNote.Text+=Lan.g(this,"Account number changed from")+" "+cc.CCNumberMasked+" "
								+Lan.g(this,"to")+" "+newAccount;
						}
						if(cc.CCExpiration != newExpiration) {
							textNote.Text+=Lan.g(this,"Expiration changed from")+" "+cc.CCExpiration.ToString("MMyy")+" "
								+Lan.g(this,"to")+" "+newExpiration.ToString("MMyy");
						}
						cc.CCNumberMasked=newAccount;
						cc.CCExpiration=newExpiration;
						CreditCards.Update(cc);
					}
				}
			}
			catch {
				MessageBox.Show(Lan.g(this,"There was a problem charging the card.  Please run the credit card report from inside X-Charge to verify that "
					+"the card was not actually charged.")+"\r\n"+Lan.g(this,"If the card was charged, you need to make sure that the payment amount matches.")
					+"\r\n"+Lan.g(this,"If the card was not charged, please try again."));
				return null;
			}
			_xChargeMilestone="Check Approved Amount";
			if(showApprovedAmtNotice && !xVoid && !xAdjust && !xReturn) {
				MessageBox.Show(Lan.g(this,"The amount you typed in")+": "+amt.ToString("C")+"\r\n"+Lan.g(this,"does not match the approved amount returned")
					+": "+approvedAmt.ToString("C")+".\r\n"+Lan.g(this,"The amount will be changed to reflect the approved amount charged."),"Alert",
					MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				textAmount.Text=approvedAmt.ToString("F");
			}
			if(xAdjust) {
				_xChargeMilestone="Check Adjust";
				MessageBox.Show(Lan.g(this,"The amount will be changed to the X-Charge approved amount")+": "+approvedAmt.ToString("C"));
				textNote.Text="";
				textAmount.Text=approvedAmt.ToString("F");
			}
			else if(xReturn) {
				_xChargeMilestone="Check Return";
				textAmount.Text="-"+approvedAmt.ToString("F");
			}
			else if(xVoid) {//For prepaid cards, tranType is set to 0 "Purchase", therefore xVoid will be false.
				_xChargeMilestone="Check Void";
				if(IsNew) {
					if(!_wasCreditCardSuccessful) {
						textAmount.Text="-"+approvedAmt.ToString("F");
						textNote.Text+=resulttext;
					}
					_paymentCur.Receipt=receipt;
					if(_printReceipt && receipt!="") {
						PrintReceipt(receipt,Lan.g(this,"X-Charge receipt printed"));
						_printReceipt=false;
					}
					if(SavePaymentToDb()) {
						DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
					}
					return resulttext;
				}
				_xChargeMilestone="Create Negative Payment";
				if(!IsNew || _wasCreditCardSuccessful) {//Create a new negative payment if the void is being run from an existing payment
					if(_listSplitsCur.Count==0) {
						AddOneSplit();
						FillGridSplits();
					}
					else if(_listSplitsCur.Count==1//if one split
						&& _listSplitsCur[0].PayPlanNum!=0//and split is on a payment plan
						&& _listSplitsCur[0].SplitAmt!=_paymentCur.PayAmt)//and amount doesn't match payment
					{
						_listSplitsCur[0].SplitAmt=_paymentCur.PayAmt;//make amounts match automatically
						textSplitTotal.Text=textAmount.Text;
					}
					_paymentCur.IsSplit=_listSplitsCur.Count>1;
					Payment voidPayment=_paymentCur.Clone();
					voidPayment.PayAmt*=-1;//the negation of the original amount
					voidPayment.PayNote=resulttext;
					voidPayment.Receipt=receipt;
					if(_printReceipt && receipt!="") {
						PrintReceipt(receipt,Lan.g(this,"X-Charge receipt printed"));
					}
					voidPayment.PaymentSource=CreditCardSource.XServer;
					voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
					voidPayment.PayNum=Payments.Insert(voidPayment);
					foreach(PaySplit splitCur in _listSplitsCur) {//Modify the paysplits for the original transaction to work for the void transaction
						PaySplit split=splitCur.Copy();
						split.SplitAmt*=-1;
						split.PayNum=voidPayment.PayNum;
						PaySplits.Insert(split);
					}
					string strErrorMsg=Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum,_listSplitsCur);
					if(!string.IsNullOrEmpty(strErrorMsg)) {
						MessageBox.Show(strErrorMsg);
					}
				}
				DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
				return resulttext;
			}
			_xChargeMilestone="Check Additional Funds";
			_wasCreditCardSuccessful=!_isCCDeclined;//If the transaction is not a void transaction, we will void this transaction if the user hits Cancel
			if(additionalFunds>0) {
				MessageBox.Show(Lan.g(this,"Additional funds required")+": "+additionalFunds.ToString("C"));
			}
			if(textNote.Text!="") {
				textNote.Text+="\r\n";
			}
			textNote.Text+=resulttext;
			_xChargeMilestone="Receipt";
			_paymentCur.Receipt=receipt;
			if(!string.IsNullOrEmpty(receipt)) {
				butPrintReceipt.Visible=true;
				if(PrefC.GetBool(PrefName.AllowEmailCCReceipt)) {
					butEmailReceipt.Visible=true;
				}
				if(_printReceipt && prepaidAmt==0) {
					PrintReceipt(receipt,Lan.g(this,"X-Charge receipt printed"));
				}
			}
			_xChargeMilestone="Reselect Credit Card in Combo";
			if(cc!=null && !string.IsNullOrEmpty(cc.XChargeToken) && cc.CCExpiration!=null) {
				//Refresh comboCreditCards and select the index of the card used for this payment if the token was saved
				creditCards=CreditCards.Refresh(_patCur.PatNum);
				AddCreditCardsToCombo(creditCards,x => x.XChargeToken==cc.XChargeToken
					&& x.CCExpiration.Year==cc.CCExpiration.Year
					&& x.CCExpiration.Month==cc.CCExpiration.Month);
			}
			if(_isCCDeclined) {
				return null;
			}
			return resulttext;
		}

		///<summary>Only used to void a transaction that has just been completed when the user hits Cancel. Uses the same Print Receipt settings as the 
		///original transaction.</summary>
		private void VoidXChargeTransaction(string transID,string amount,bool isDebit) {
			ProcessStartInfo info=new ProcessStartInfo(_xProg.Path);
			string resultfile=PrefC.GetRandomTempFile("txt");
			File.Delete(resultfile);//delete the old result file.
			info.Arguments="";
			if(isDebit) {
				info.Arguments+="/TRANSACTIONTYPE:DEBITRETURN /LOCKTRANTYPE ";
			}
			else {
				info.Arguments+="/TRANSACTIONTYPE:VOID /LOCKTRANTYPE ";
			}
			info.Arguments+="/XCTRANSACTIONID:"+transID+" /LOCKXCTRANSACTIONID ";
			info.Arguments+="/AMOUNT:"+amount+" /LOCKAMOUNT ";
			info.Arguments+="/RECEIPT:Pat"+_paymentCur.PatNum.ToString()+" ";//aka invoice#
			info.Arguments+="\"/CLERK:"+Security.CurUser.UserName+"\" /LOCKCLERK ";
			info.Arguments+="/RESULTFILE:\""+resultfile+"\" ";
			info.Arguments+="/USERID:"+ProgramProperties.GetPropVal(_xProg.ProgramNum,"Username",_paymentCur.ClinicNum)+" ";
			info.Arguments+="/PASSWORD:"+CodeBase.MiscUtils.Decrypt(ProgramProperties.GetPropVal(_xProg.ProgramNum,"Password",_paymentCur.ClinicNum))+" ";
			info.Arguments+="/AUTOCLOSE ";
			info.Arguments+="/HIDEMAINWINDOW /SMALLWINDOW ";
			if(!isDebit) {
				info.Arguments+="/AUTOPROCESS ";
			}
			info.Arguments+="/PROMPTSIGNATURE:F ";
			info.Arguments+="/RECEIPTINRESULT ";
			Cursor=Cursors.WaitCursor;
			Process process=new Process();
			process.StartInfo=info;
			process.EnableRaisingEvents=true;
			process.Start();
			process.WaitForExit();
			Thread.Sleep(200);//Wait 2/10 second to give time for file to be created.
			Cursor=Cursors.Default;
			//Next, record the voided payment within Open Dental.  We use to delete the payment but Nathan wants us to negate voids with another payment.
			string resulttext="";
			string line="";
			bool showApprovedAmtNotice=false;
			double approvedAmt=0;
			string receipt="";
			Payment voidPayment=_paymentCur.Clone();
			voidPayment.PayAmt*=-1;//the negation of the original amount
			try {
				using(TextReader reader=new StreamReader(resultfile)) {
					line=reader.ReadLine();
					/*Example of successful void transaction:
						RESULT=SUCCESS
						TYPE=Void
						APPROVALCODE=000000
						SWIPED=F
						CLERK=Admin
						XCACCOUNTID=XAWpQPwLm7MXZ
						XCTRANSACTIONID=15042616
						ACCOUNT=XXXXXXXXXXXX6781
						EXPIRATION=1215
						ACCOUNTTYPE=VISA
						APPROVEDAMOUNT=11.00
					*/
					while(line!=null) {
						if(!line.StartsWith("RECEIPT=")) {//Don't include the receipt string in the PayNote
							if(resulttext!="") {
								resulttext+="\r\n";
							}
							resulttext+=line;
						}
						if(line.StartsWith("RESULT=")) {
							if(line!="RESULT=SUCCESS") {
								//Void was a failure and there might be a description as to why it failed. Continue to loop through line.
								while(line!=null) {
									line=reader.ReadLine();
									resulttext+="\r\n"+line;
								}
								break;
							}
						}
						if(line.StartsWith("APPROVEDAMOUNT=")) {
							approvedAmt=PIn.Double(line.Substring(15));
							if(approvedAmt != _paymentCur.PayAmt) {
								showApprovedAmtNotice=true;
							}
						}
						if(line.StartsWith("RECEIPT=") && line.Length>8) {
							receipt=PIn.String(line.Substring(8));
							receipt=receipt.Replace("\\n","\r\n");//The receipt from X-Charge escapes the newline characters
						}
						line=reader.ReadLine();
					}
				}
			}
			catch {
				MessageBox.Show(Lan.g(this,"There was a problem voiding this transaction.")+"\r\n"+Lan.g(this,"Please run the credit card report from inside "
					+"X-Charge to verify that the transaction was voided.")+"\r\n"+Lan.g(this,"If the transaction was not voided, please create a new payment "
					+"to void the transaction."));
				return;
			}
			if(showApprovedAmtNotice) {
				MessageBox.Show(Lan.g(this,"The amount of the original transaction")+": "+_paymentCur.PayAmt.ToString("C")+"\r\n"+Lan.g(this,"does not match "
					+"the approved amount returned")+": "+approvedAmt.ToString("C")+".\r\n"+Lan.g(this,"The amount will be changed to reflect the approved "
					+"amount charged."),"Alert",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				voidPayment.PayAmt=approvedAmt;
			}
			if(textNote.Text!="") {
				textNote.Text+="\r\n";
			}
			voidPayment.PayNote=resulttext;
			voidPayment.Receipt=receipt;
			if(_printReceipt && receipt!="") {
				PrintReceipt(receipt,Lan.g(this,"X-Charge receipt printed"));
			}
			voidPayment.PaymentSource=CreditCardSource.XServer;
			voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
			voidPayment.PayNum=Payments.Insert(voidPayment);
			for(int i=0;i<_listSplitsCur.Count;i++) {//Modify the paysplits for the original transaction to work for the void transaction
				PaySplit split=_listSplitsCur[i].Copy();
				split.SplitAmt*=-1;
				split.PayNum=voidPayment.PayNum;
				PaySplits.Insert(split);
			}
			SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,voidPayment.PatNum,Patients.GetLim(voidPayment.PatNum).GetNameLF()+", "
				+voidPayment.PayAmt.ToString("c"));
		}

		private bool HasXCharge() {
			_listPaymentTypeDefs=_listPaymentTypeDefs??Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
			if(_xProg==null) {
				MsgBox.Show(this,"X-Charge entry is missing from the database.");//should never happen
				return false;
			}
			bool isSetupRequired=false;
			//if X-Charge is enabled, but the Username or Password are blank or the PaymentType is not a valid DefNum, setup is required
			if(_xProg.Enabled) {
				//X-Charge is enabled if the username and password are set and the PaymentType is a valid DefNum
				//If clinics are disabled, _paymentCur.ClinicNum will be 0 and the Username and Password will be the 'Headquarters' or practice credentials
				string paymentType=ProgramProperties.GetPropVal(_xProg.ProgramNum,"PaymentType",_paymentCur.ClinicNum);
				if(string.IsNullOrEmpty(ProgramProperties.GetPropVal(_xProg.ProgramNum,"Username",_paymentCur.ClinicNum))
					|| string.IsNullOrEmpty(ProgramProperties.GetPropVal(_xProg.ProgramNum,"Password",_paymentCur.ClinicNum))
					|| !_listPaymentTypeDefs.Any(x => x.DefNum.ToString()==paymentType))
				{
					isSetupRequired=true;
				}
			}
			else {//Program link not enabled.  Launch a promo site.
				ODException.SwallowAnyException(() =>
					Process.Start("http://www.opendental.com/resources/redirects/redirectopenedge.html")
				);
				return false;
			}
			//if X-Charge is enabled and the Username and Password is set and the PaymentType is a valid DefNum,
			//make sure the path (either local override or program path) is valid
			if(!isSetupRequired && !File.Exists(_xPath)) {
				MsgBox.Show(this,"Path is not valid.");
				isSetupRequired=true;
			}
			//if setup is required and the user is authorized for setup, load the X-Charge setup form, but return false so the validation can happen again
			if(isSetupRequired && Security.IsAuthorized(Permissions.Setup)) {
				FormXchargeSetup FormX=new FormXchargeSetup();
				FormX.ShowDialog();
				CheckUIState();//user may have made a change in setup that affects the state of the UI, e.g. X-Charge is no longer enabled for this clinic
				return false;
			}
			return true;
		}

		private string GetXChargeTransactionTypeCommands(int tranType,bool hasXToken,bool notRecurring,CreditCard CCard,string cashBack) {
			string tranText="";
			switch(tranType) {
				case 0:
					tranText+="/TRANSACTIONTYPE:PURCHASE /LOCKTRANTYPE /LOCKAMOUNT ";
					if(hasXToken && CCard!=null) {
						tranText+="/XCACCOUNTID:"+CCard.XChargeToken+" ";
						tranText+="/AUTOPROCESS ";
						tranText+="/GETXCACCOUNTIDSTATUS ";
					}
					if(notRecurring && CCard!=null) {
						tranText+="/ACCOUNT:"+CCard.CCNumberMasked+" ";
						tranText+="/AUTOPROCESS ";
					}
					break;
				case 1:
					tranText+="/TRANSACTIONTYPE:RETURN /LOCKTRANTYPE /LOCKAMOUNT ";
					if(hasXToken) {
						tranText+="/XCACCOUNTID:"+CCard.XChargeToken+" ";
						tranText+="/AUTOPROCESS ";
						tranText+="/GETXCACCOUNTIDSTATUS ";
					}
					if(notRecurring) {
						tranText+="/ACCOUNT:"+CCard.CCNumberMasked+" ";
						tranText+="/AUTOPROCESS ";
					}
					break;
				case 2:
					tranText+="/TRANSACTIONTYPE:DEBITPURCHASE /LOCKTRANTYPE /LOCKAMOUNT ";
					tranText+="/CASHBACK:"+cashBack+" ";
					break;
				case 3:
					tranText+="/TRANSACTIONTYPE:DEBITRETURN /LOCKTRANTYPE /LOCKAMOUNT ";
					break;
				case 4:
					tranText+="/TRANSACTIONTYPE:FORCE /LOCKTRANTYPE /LOCKAMOUNT ";
					break;
				case 5:
					tranText+="/TRANSACTIONTYPE:PREAUTH /LOCKTRANTYPE /LOCKAMOUNT ";
					if(hasXToken) {
						tranText+="/XCACCOUNTID:"+CCard.XChargeToken+" ";
						tranText+="/AUTOPROCESS ";
						tranText+="/GETXCACCOUNTIDSTATUS ";
					}
					if(notRecurring) {
						tranText+="/ACCOUNT:"+CCard.CCNumberMasked+" ";
						tranText+="/AUTOPROCESS ";
					}
					break;
				case 6:
					tranText+="/TRANSACTIONTYPE:ADJUSTMENT /LOCKTRANTYPE ";//excluding /LOCKAMOUNT, amount must be editable in X-Charge to make an adjustment
					string adjustTransactionID="";
					string[] noteSplit=Regex.Split(textNote.Text,"\r\n");
					foreach(string XCTrans in noteSplit) {
						if(XCTrans.StartsWith("XCTRANSACTIONID=")) {
							adjustTransactionID=XCTrans.Substring(16);
						}
					}
					if(adjustTransactionID!="") {
						tranText+="/XCTRANSACTIONID:"+adjustTransactionID+" ";
						tranText+="/AUTOPROCESS ";
					}
					break;
				case 7:
					tranText+="/TRANSACTIONTYPE:VOID /LOCKTRANTYPE /LOCKAMOUNT ";
					break;
			}
			if(_promptSignature) {
				tranText+="/PROMPTSIGNATURE:T /SAVESIGNATURE:T ";
			}
			else {
				tranText+="/PROMPTSIGNATURE:F ";
			}
			tranText+="/RECEIPTINRESULT ";//So that we can make a few changes to the receipt ourselves
			return tranText;
		}
		
		private void butReturn_Click(object sender,EventArgs e) {
			if(MsgBox.Show(MsgBoxButtons.YesNo,Lan.g(this,"Are you sure you want to return this transaction?"))) {
				if(_xWebResponse!=null) {
					XWebReturn();
				}
				else if(_payConnectResponseWeb!=null) {
					PayConnectReturn();
				}
			}
		}

		private void butVoid_Click(object sender,EventArgs e) {
			if(MsgBox.Show(MsgBoxButtons.YesNo,Lan.g(this,"Are you sure you want to void this transaction?"))) {
				if(_xWebResponse!=null) {
					XWebVoid();
				}
				else if(_payConnectResponseWeb!=null) {
					PayConnectVoid();
				}
			}
		}

		private void PayConnectReturn() {
			string refNum=_payConnectResponseWeb.RefNumber;
			if(refNum.IsNullOrEmpty()) {
				MsgBox.Show(this,"Missing PayConnect Reference Number. This return cannot be processed.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			if(!PayConnectL.VoidOrRefundPayConnectPortalTransaction(_payConnectResponseWeb,_paymentCur,PayConnectService.transType.RETURN,refNum,(decimal)_paymentCur.PayAmt)) {
				Cursor=Cursors.Default;
				return;
			}
			Cursor=Cursors.Default;
			MsgBox.Show(this,"Return successful.");
		}

		private void PayConnectVoid() {
			string refNum=_payConnectResponseWeb.RefNumber;
			if(refNum.IsNullOrEmpty()) {
				MsgBox.Show(this,"Missing PayConnect Reference Number. This void cannot be processed.");
				butVoid.Enabled=false;
				return;
			}
			Cursor=Cursors.WaitCursor;
			if(!PayConnectL.VoidOrRefundPayConnectPortalTransaction(_payConnectResponseWeb,_paymentCur,PayConnectService.transType.VOID,refNum,(decimal)_paymentCur.PayAmt)) {
				Cursor=Cursors.Default;
				return;
			}
			Cursor=Cursors.Default;
			MsgBox.Show(this,"Void successful.");
		}

		private void XWebReturn() {
			CreditCard cc=null;
			List<CreditCard> creditCards=CreditCards.Refresh(_patCur.PatNum);
			if(comboCreditCards.SelectedIndex < creditCards.Count && comboCreditCards.SelectedIndex >-1) {
				cc=creditCards[comboCreditCards.SelectedIndex];
			}
			if(cc==null) {
				MsgBox.Show(this,"Card no longer available. Return cannot be processed.");
				return;
			}
			if(!cc.IsXWeb()) {
				MsgBox.Show(this,"Only cards that were created from XWeb can process an XWeb return.");
				return;
			}
			FormXWeb FormXW=new FormXWeb(_patCur.PatNum,cc,XWebTransactionType.CreditReturnTransaction,createPayment:false);
			FormXW.LockCardInfo=true;
			if(FormXW.ShowDialog()==DialogResult.OK) {
				if(FormXW.ResponseResult!=null) {
					textNote.Text=FormXW.ResponseResult.GetFormattedNote(false);
					textAmount.Text=(-FormXW.ResponseResult.Amount).ToString();//XWeb amounts are always positive even for returns and voids.
					_xWebResponse=FormXW.ResponseResult;
					_xWebResponse.PaymentNum=_paymentCur.PayNum;
					XWebResponses.Update(_xWebResponse);
					butVoid.Visible=true;
					groupXWeb.Height=85;
				}
				MsgBox.Show(this,"Return successful.");
			}
		}

		private void XWebVoid() {
			if(!Security.IsAuthorized(Permissions.PaymentCreate)) {
				return;
			}
			double amount=_xWebResponse.Amount;
			if(_xWebResponse.XTransactionType==XWebTransactionType.CreditReturnTransaction 
				|| _xWebResponse.XTransactionType==XWebTransactionType.DebitReturnTransaction) 
			{
				amount=-amount;//The amount in an xwebresponse is always stored as a positive number.
			}
			if(MessageBox.Show(Lan.g(this,"Void the XWeb transaction of amount")+" "+amount.ToString("f")+" "+Lan.g(this,"attached to this payment?"),
				"",MessageBoxButtons.YesNo)==DialogResult.No)
			{
				return;
			}
			try {
				Cursor=Cursors.WaitCursor;
				string payNote=Lan.g(this,"Void XWeb payment made from within Open Dental");
				XWebs.VoidPayment(_patCur.PatNum,payNote,_xWebResponse.XWebResponseNum);
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Void successful. A new payment has been created for this void transaction.");
			}
			catch(ODException ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(ex.Message);
			}
		}

		private void butPayConnect_Click(object sender,EventArgs e) {
			if(!CanAddNewCreditCard(Programs.GetCur(ProgramName.PayConnect),PayConnect.ProgramProperties.PayConnectPreventSavingNewCC)) {
				return;
			}
			if(comboCreditCards.SelectedIndex < _listCreditCards.Count) {
				CreditCard cc=_listCreditCards[comboCreditCards.SelectedIndex];
				if(cc!=null && cc.CCSource==CreditCardSource.PayConnectPortal) {
					MsgBox.Show(this,"The selected credit card can only be used to void and return payments made with this card.  Use the Void and Return buttons in this window instead.");
					return;
				}
			}
			MakePayConnectTransaction();
		}

		///<summary>Launches the PayConnect transaction window.  Returns null upon failure, otherwise returns the transaction detail as a string.
		///If prepaidAmt is not zero, then will show the PayConnect window with the given prepaid amount and let the user enter card # and exp.
		///A patient is not required for prepaid cards.</summary>
		public string MakePayConnectTransaction(double prepaidAmt=0) {
			if(!HasPayConnect()) {
				return null;
			}
			if(prepaidAmt==0) {//Validation for regular credit cards (not prepaid cards).
				if(textAmount.Text=="") {
					MsgBox.Show(this,"Please enter an amount first.");
					textAmount.Focus();
					return null;
				}
				if(_listSplitsCur.Count>0 && PIn.Double(textAmount.Text)!=PIn.Double(textSplitTotal.Text)
					&& (_listSplitsCur.Count!=1 || _listSplitsCur[0].PayPlanNum==0)) //Not one paysplit attached to payplan
				{
					MsgBox.Show(this, "Split totals must equal payment amount before running a credit card transaction.");
					return null;
				}
			}
			if(PIn.Date(textDate.Text).Date > DateTime.Today.Date
					&& !PrefC.GetBool(PrefName.FutureTransDatesAllowed) && !PrefC.GetBool(PrefName.AccountAllowFutureDebits))
			{
				MsgBox.Show(this,"Payment date cannot be in the future.");
				return null;
			}
			CreditCard cc=null;
			List<CreditCard> creditCards=null;
			decimal amount=Math.Abs(PIn.Decimal(textAmount.Text));//PayConnect always wants a positive number even for voids and returns.
			if(prepaidAmt==0) {
				creditCards=CreditCards.Refresh(_patCur.PatNum);
				if(comboCreditCards.SelectedIndex < creditCards.Count) {
					cc=creditCards[comboCreditCards.SelectedIndex];
				}
			}
			else {//Prepaid card
				amount=(decimal)prepaidAmt;
			}
			FormPayConnect FormP=new FormPayConnect(_paymentCur.ClinicNum,_patCur,amount,cc);
			FormP.ShowDialog();
			if(prepaidAmt==0 && FormP.Response!=null) {//Regular credit cards (not prepaid cards).
				//If PayConnect response is not null, refresh comboCreditCards and select the index of the card used for this payment if the token was saved
				creditCards=CreditCards.Refresh(_patCur.PatNum);
				AddCreditCardsToCombo(creditCards,x => x.PayConnectToken==FormP.Response.PaymentToken
					&&x.PayConnectTokenExp.Year==FormP.Response.TokenExpiration.Year
					&&x.PayConnectTokenExp.Month==FormP.Response.TokenExpiration.Month);
				Program prog=Programs.GetCur(ProgramName.PayConnect);
				//still need to add functionality for accountingAutoPay
				string paytype=ProgramProperties.GetPropVal(prog.ProgramNum,"PaymentType",_paymentCur.ClinicNum);//paytype could be an empty string
				listPayType.SelectedIndex=Defs.GetOrder(DefCat.PaymentTypes,PIn.Long(paytype));
				SetComboDepositAccounts();
			}
			string resultNote=null;
			if(FormP.Response!=null) {
				resultNote=Lan.g(this,"Transaction Type")+": "+Enum.GetName(typeof(PayConnectService.transType),FormP.TranType)+Environment.NewLine+
					Lan.g(this,"Status")+": "+FormP.Response.Description+Environment.NewLine+
					Lan.g(this,"Amount")+": "+FormP.AmountCharged+Environment.NewLine+
					Lan.g(this,"Card Type")+": "+FormP.Response.CardType+Environment.NewLine+
					Lan.g(this,"Account")+": "+FormP.CardNumber.Right(4).PadLeft(FormP.CardNumber.Length,'X');
			}
			if(prepaidAmt!=0) {
				if(FormP.Response!=null && FormP.Response.StatusCode=="0") { //The transaction succeeded.
					return resultNote;
				}
				return null;
			}
			if(FormP.Response!=null) {
				if(FormP.Response.StatusCode=="0") { //The transaction succeeded.
					_isCCDeclined=false;
					resultNote+=Environment.NewLine
						+Lan.g(this,"Auth Code")+": "+FormP.Response.AuthCode+Environment.NewLine
						+Lan.g(this,"Ref Number")+": "+FormP.Response.RefNumber;
					if(FormP.TranType==PayConnectService.transType.RETURN) {
						textAmount.Text="-"+FormP.AmountCharged;
						_paymentCur.Receipt=FormP.ReceiptStr;
					}
					else if(FormP.TranType==PayConnectService.transType.AUTH) {
						textAmount.Text=FormP.AmountCharged;
					}
					else if(FormP.TranType==PayConnectService.transType.SALE) {
						textAmount.Text=FormP.AmountCharged;
						_paymentCur.Receipt=FormP.ReceiptStr;
					}
					if(FormP.TranType==PayConnectService.transType.VOID) {//Close FormPayment window now so the user will not have the option to hit Cancel
						if(IsNew) {
							if(!_wasCreditCardSuccessful) {
								textAmount.Text="-"+FormP.AmountCharged;
								textNote.Text+=((textNote.Text=="")?"":Environment.NewLine)+resultNote;
							}
							_paymentCur.Receipt=FormP.ReceiptStr;
							if(SavePaymentToDb()) {
								MsgBox.Show(this,"Void successful.");
								DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
							}
							return resultNote;
						}
						if(!IsNew || _wasCreditCardSuccessful) {//Create a new negative payment if the void is being run from an existing payment
							if(_listSplitsCur.Count==0) {
								AddOneSplit();
								FillGridSplits();
							}
							else if(_listSplitsCur.Count==1//if one split
								&& _listSplitsCur[0].PayPlanNum!=0//and split is on a payment plan
								&& _listSplitsCur[0].SplitAmt!=_paymentCur.PayAmt)//and amount doesn't match payment
							{
								_listSplitsCur[0].SplitAmt=_paymentCur.PayAmt;//make amounts match automatically
								textSplitTotal.Text=textAmount.Text;
							}
							_paymentCur.IsSplit=_listSplitsCur.Count>1;
							Payment voidPayment=_paymentCur.Clone();
							voidPayment.PayAmt*=-1;//the negation of the original amount
							voidPayment.PayNote=resultNote;
							voidPayment.Receipt=FormP.ReceiptStr;
							voidPayment.PaymentSource=CreditCardSource.PayConnect;
							voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
							voidPayment.PayNum=Payments.Insert(voidPayment);
							foreach(PaySplit splitCur in _listSplitsCur) {//Modify the paysplits for the original transaction to work for the void transaction
								PaySplit split=splitCur.Copy();
								split.SplitAmt*=-1;
								split.PayNum=voidPayment.PayNum;
								PaySplits.Insert(split);
							}
							string strErrorMsg=Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum,_listSplitsCur);
							if(!string.IsNullOrEmpty(strErrorMsg)) {
								MessageBox.Show(strErrorMsg);
							}
						}
						MsgBox.Show(this,"Void successful.");
						DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
						return resultNote;
					}
					else {//Not Void
						_wasCreditCardSuccessful=true; //Will void the transaction if user cancels out of window.
					}
					_payConnectRequest=FormP.Request;
				}
				textNote.Text+=((textNote.Text=="")?"":Environment.NewLine)+resultNote;
				textNote.SelectionStart=textNote.TextLength;
				textNote.ScrollToCaret();//Scroll to the end of the text box to see the newest notes.
				_paymentCur.PayNote=textNote.Text;
				_paymentCur.PaymentSource=CreditCardSource.PayConnect;
				_paymentCur.ProcessStatus=ProcessStat.OfficeProcessed;
				Payments.Update(_paymentOld,true);
			}
			if(!string.IsNullOrEmpty(_paymentCur.Receipt)) {
				butPrintReceipt.Visible=true;
				if(PrefC.GetBool(PrefName.AllowEmailCCReceipt)) {
					butEmailReceipt.Visible=true;
				}
			}
			if(FormP.Response==null || FormP.Response.StatusCode!="0") { //The transaction failed.
				if(FormP.TranType==PayConnectService.transType.SALE || FormP.TranType==PayConnectService.transType.AUTH) {
					textAmount.Text=FormP.AmountCharged;//Preserve the amount so the user can try the payment again more easily.
				}
				_isCCDeclined=true;
				_wasCreditCardSuccessful=false;
				return null;
			}
			return resultNote;
		}

		///<summary>Returns true if payconnect is enabled and completely setup.</summary>
		private bool HasPayConnect() {
			_listPaymentTypeDefs=_listPaymentTypeDefs??Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
			Program prog=Programs.GetCur(ProgramName.PayConnect);
			bool isSetupRequired=false;
			if(prog.Enabled) {
				//If clinics are disabled, _paymentCur.ClinicNum will be 0 and the Username and Password will be the 'Headquarters' or practice credentials
				string paymentType=ProgramProperties.GetPropVal(prog.ProgramNum,"PaymentType",_paymentCur.ClinicNum);
				if(string.IsNullOrEmpty(ProgramProperties.GetPropVal(prog.ProgramNum,"Username",_paymentCur.ClinicNum))
					|| string.IsNullOrEmpty(ProgramProperties.GetPropVal(prog.ProgramNum,"Password",_paymentCur.ClinicNum))
					|| !_listPaymentTypeDefs.Any(x => x.DefNum.ToString()==paymentType)) 
				{
					isSetupRequired=true;
				}
			}
			else {//Program link not enabled.  Launch a promo site.
				ODException.SwallowAnyException(() =>
					Process.Start("http://www.opendental.com/resources/redirects/redirectpayconnect.html")
				);
				return false;
			}
			if(isSetupRequired) {
				if(!Security.IsAuthorized(Permissions.Setup)) {
					return false;
				}
				FormPayConnectSetup FormPCS=new FormPayConnectSetup();
				FormPCS.ShowDialog();
				if(FormPCS.DialogResult!=DialogResult.OK) {
					return false;
				}
				//The user could have corrected the PayConnect bridge, recursively try again.
				return HasPayConnect();
			}
			return true;
		}

		private void butPaySimple_Click(object sender,MouseEventArgs e) {
			if(e.Button!=MouseButtons.Left) {
				return;
			}
			if(!CanAddNewCreditCard(Programs.GetCur(ProgramName.PaySimple),PaySimple.PropertyDescs.PaySimplePreventSavingNewCC)) {
				return;
			}
			MakePaySimpleTransaction();
		}

		///<summary>Launches the PaySimple transaction window.  Returns null upon failure, otherwise returns the transaction detail as a string.
		///If prepaidAmt is not zero, then will show the PaySimple window with the given prepaid amount and let the user enter card # and exp.
		///A patient is not required for prepaid cards.</summary>
		public string MakePaySimpleTransaction(double prepaidAmt=0,string carrierName="") {
			if(!HasPaySimple()) {
				return null;
			}
			CreditCard cc=null;
			List<CreditCard> creditCards=null;
			decimal amount=Math.Abs(PIn.Decimal(textAmount.Text));//PaySimple always wants a positive number even for voids and returns.
			if(prepaidAmt==0) {
				creditCards=CreditCards.Refresh(_patCur.PatNum);
				if(comboCreditCards.SelectedIndex < creditCards.Count) {
					cc=creditCards[comboCreditCards.SelectedIndex];
				}
			}
			else {//Prepaid card
				amount=(decimal)prepaidAmt;
			}
			FormPaySimple form=new FormPaySimple(_paymentCur.ClinicNum,_patCur,amount,cc,carrierName:carrierName);
			form.ShowDialog();
			Program prog=Programs.GetCur(ProgramName.PaySimple);
			if(prepaidAmt==0) {//Regular credit cards (not prepaid cards).
				//If PaySimple response is not null, refresh comboCreditCards and select the index of the card used for this payment if the token was saved
				creditCards=CreditCards.Refresh(_patCur.PatNum);
				string paySimpleToken=cc==null ? "" : cc.PaySimpleToken;
				if(form.ApiResponseOut!=null) {
					paySimpleToken=form.ApiResponseOut.PaySimpleToken;
				}
				AddCreditCardsToCombo(creditCards,x => x.PaySimpleToken==paySimpleToken && !string.IsNullOrEmpty(paySimpleToken));
				//still need to add functionality for accountingAutoPay
				//paytype could be an empty string
				string paytype=ProgramProperties.GetPropValForClinicOrDefault(prog.ProgramNum,PaySimple.PropertyDescs.PaySimplePayTypeCC,_paymentCur.ClinicNum);
				listPayType.SelectedIndex=Defs.GetOrder(DefCat.PaymentTypes,PIn.Long(paytype));
				SetComboDepositAccounts();
			}
			if(prepaidAmt!=0) {
				if(form.ApiResponseOut!=null) { //The transaction succeeded.
					if(form.ApiResponseOut.CCSource==CreditCardSource.PaySimpleACH) {
						_paymentCur.PaymentStatus=PaymentStatus.PaySimpleAchPosted;
						_paymentCur.ExternalId=form.ApiResponseOut.RefNumber;
					}
					return form.ApiResponseOut.ToNoteString();
				}
				return null;
			}
			string resultNote=null;
			if(form.ApiResponseOut!=null) { //The transaction succeeded.
				_isCCDeclined=false;
				resultNote=form.ApiResponseOut.ToNoteString();
				_paymentCur.PaymentSource=form.ApiResponseOut.CCSource;
				if(form.ApiResponseOut.CCSource==CreditCardSource.PaySimpleACH) {
					string paytype=ProgramProperties.GetPropValForClinicOrDefault(prog.ProgramNum,PaySimple.PropertyDescs.PaySimplePayTypeACH,
						_paymentCur.ClinicNum);
					_paymentCur.PaymentStatus=PaymentStatus.PaySimpleAchPosted;
					_paymentCur.ExternalId=form.ApiResponseOut.RefNumber;
					int defOrder=Defs.GetOrder(DefCat.PaymentTypes,PIn.Long(paytype));
					//paytype could be an empty string, so then leave listPayType as it was.
					if(defOrder>=-1) {
						listPayType.SelectedIndex=defOrder;
					}
				}
				if(form.ApiResponseOut.TransType==PaySimple.TransType.RETURN) {
					textAmount.Text="-"+form.ApiResponseOut.Amount.ToString("F");
					_paymentCur.Receipt=form.ApiResponseOut.TransactionReceipt;
				}
				else if(form.ApiResponseOut.TransType==PaySimple.TransType.AUTH) {
					textAmount.Text=form.ApiResponseOut.Amount.ToString("F");
				}
				else if(form.ApiResponseOut.TransType==PaySimple.TransType.SALE) {
					textAmount.Text=form.ApiResponseOut.Amount.ToString("F");
					_paymentCur.Receipt=form.ApiResponseOut.TransactionReceipt;
				}
				if(form.ApiResponseOut.TransType==PaySimple.TransType.VOID) {//Close FormPayment window now so the user will not have the option to hit Cancel
					if(IsNew) {
						if(!_wasCreditCardSuccessful) {
							textAmount.Text="-"+form.ApiResponseOut.Amount.ToString("F");
							textNote.Text+=((textNote.Text=="") ? "" : Environment.NewLine)+resultNote;
						}
						_paymentCur.Receipt=form.ApiResponseOut.TransactionReceipt;
						if(SavePaymentToDb()) {
							MsgBox.Show(this,"Void successful.");
							DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
						}
						return resultNote;
					}
					if(!IsNew || _wasCreditCardSuccessful) {//Create a new negative payment if the void is being run from an existing payment
						if(_listSplitsCur.Count==0) {
							AddOneSplit();
							FillGridSplits();
						}
						else if(_listSplitsCur.Count==1//if one split
							&& _listSplitsCur[0].PayPlanNum!=0//and split is on a payment plan
							&& _listSplitsCur[0].SplitAmt!=_paymentCur.PayAmt)//and amount doesn't match payment
						{
							_listSplitsCur[0].SplitAmt=_paymentCur.PayAmt;//make amounts match automatically
							textSplitTotal.Text=textAmount.Text;
						}
						_paymentCur.IsSplit=_listSplitsCur.Count>1;
						Payment voidPayment=_paymentCur.Clone();
						voidPayment.PayAmt*=-1;//the negation of the original amount
						voidPayment.PayNote=resultNote;
						voidPayment.Receipt=form.ApiResponseOut.TransactionReceipt;
						voidPayment.PaymentSource=CreditCardSource.PaySimple;
						voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
						voidPayment.PayNum=Payments.Insert(voidPayment);
						foreach(PaySplit splitCur in _listSplitsCur) {//Modify the paysplits for the original transaction to work for the void transaction
							PaySplit split=splitCur.Copy();
							split.SplitAmt*=-1;
							split.PayNum=voidPayment.PayNum;
							PaySplits.Insert(split);
						}
						string strErrorMsg=Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum,_listSplitsCur);
						if(!string.IsNullOrEmpty(strErrorMsg)) {
							MessageBox.Show(strErrorMsg);
						}
					}
					MsgBox.Show(this,"Void successful.");
					DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
					return resultNote;
				}
				else {//Not Void
					_wasCreditCardSuccessful=true; //Will void the transaction if user cancels out of window.
				}
				_paySimpleResponse=form.ApiResponseOut;
			}
			if(!string.IsNullOrWhiteSpace(resultNote)) {
				textNote.Text+=((textNote.Text=="") ? "" : Environment.NewLine)+resultNote;
			}
			textNote.SelectionStart=textNote.TextLength;
			textNote.ScrollToCaret();//Scroll to the end of the text box to see the newest notes.
			_paymentCur.PayNote=textNote.Text;
			if(_paymentCur.PaymentSource==CreditCardSource.None) {
				_paymentCur.PaymentSource=CreditCardSource.PaySimple;
			}
			_paymentCur.ProcessStatus=ProcessStat.OfficeProcessed;
			Payments.Update(_paymentCur,true);
			if(!string.IsNullOrEmpty(_paymentCur.Receipt)) {
				butPrintReceipt.Visible=true;
				if(PrefC.GetBool(PrefName.AllowEmailCCReceipt)) {
					butEmailReceipt.Visible=true;
				}
			}
			if(form.ApiResponseOut==null || form.ApiResponseOut.Status.ToLower()=="failed") { //The transaction failed.
				//PaySimple checks the transaction type here and sets the amount the user chose to the textAmount textbox. 
				//We don't have that information here so do nothing.
				_isCCDeclined=true;
				_wasCreditCardSuccessful=false;
				return null;
			}
			return resultNote;
		}

		///<summary>Returns true if PaySimple is enabled and completely setup.</summary>
		private bool HasPaySimple() {
			_listPaymentTypeDefs=_listPaymentTypeDefs??Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
			Program prog=Programs.GetCur(ProgramName.PaySimple);
			bool isSetupRequired=false;
			if(prog.Enabled) {
				//If clinics are disabled, _paymentCur.ClinicNum will be 0 and the Username and Key will be the 'Headquarters' or practice credentials
				string paymentType=ProgramProperties.GetPropValForClinicOrDefault(prog.ProgramNum,PaySimple.PropertyDescs.PaySimplePayTypeCC,_paymentCur.ClinicNum);
				if(string.IsNullOrEmpty(ProgramProperties.GetPropValForClinicOrDefault(prog.ProgramNum,PaySimple.PropertyDescs.PaySimpleApiUserName,_paymentCur.ClinicNum))
					|| string.IsNullOrEmpty(ProgramProperties.GetPropValForClinicOrDefault(prog.ProgramNum,PaySimple.PropertyDescs.PaySimpleApiKey,_paymentCur.ClinicNum))
					|| !_listPaymentTypeDefs.Any(x => x.DefNum.ToString()==paymentType)) 
				{
					isSetupRequired=true;
				}
			}
			else {//Program link not enabled.  Launch a promo website.
				ODException.SwallowAnyException(() =>
					Process.Start("http://www.opendental.com/resources/redirects/redirectpaysimple.html")
				);
				return false;
			}
			if(isSetupRequired) {
				if(!Security.IsAuthorized(Permissions.Setup)) {
					return false;
				}
				FormPaySimpleSetup form=new FormPaySimpleSetup();
				form.ShowDialog();
				if(form.DialogResult!=DialogResult.OK) {
					return false;
				}
				//The user could have corrected the PaySimple bridge, recursively try again.
				return HasPaySimple();
			}
			return true;
		}

		///<summary>Returns true if the user can add a new credit card.</summary>
		private bool CanAddNewCreditCard(Program prog,string progPropertyDescription) {
			if(comboCreditCards.GetSelected<CreditCard>()==null) {
				MsgBox.Show(this,"Invalid credit card selected.");
				return false;
			}
			bool hasPreventCcAdd=PIn.Bool(ProgramProperties.GetPropVal(prog.ProgramNum,progPropertyDescription,_paymentCur.ClinicNum));
			CreditCard ccSelected=comboCreditCards.GetSelected<CreditCard>();
			if(ccSelected==null) {
				return !hasPreventCcAdd;
			}
			bool hasToken=false;
			if(prog.ProgName==ProgramName.Xcharge.ToString() && !string.IsNullOrEmpty(ccSelected.XChargeToken)) {
				hasToken=true;
			}
			else if(prog.ProgName==ProgramName.PayConnect.ToString() && !string.IsNullOrEmpty(ccSelected.PayConnectToken)) {
				hasToken=true;
			}
			else if(prog.ProgName==ProgramName.PaySimple.ToString() && !string.IsNullOrEmpty(ccSelected.PaySimpleToken)) {
				hasToken=true;
			}
			if(hasPreventCcAdd && (ccSelected.CreditCardNum==0 || !hasToken)) {
				MsgBox.Show(this,"Cannot add a new credit card.");
				return false;
			}
			return true;
		}

		private void VoidPayConnectTransaction(string refNum,string amount) {
			PayConnectResponse response=null;
			string receiptStr="";
			Cursor=Cursors.WaitCursor;
			if(_payConnectRequest==null) {//The payment was made through the terminal.
				ODProgress.ShowAction(() => {
						PosRequest posRequest=PosRequest.CreateVoidByReference(refNum);
						PosResponse posResponse=DpsPos.ProcessCreditCard(posRequest);
						response=PayConnectTerminal.ToPayConnectResponse(posResponse);
						receiptStr=PayConnectTerminal.BuildReceiptString(posRequest,posResponse,null,0);
					},
					startingMessage:Lan.g(this,"Processing void on terminal."),
					actionException:ex => {
						this.Invoke(() => {
							Cursor=Cursors.Default;
							MessageBox.Show(Lan.g(this,"Error voiding payment:")+" "+ex.Message);
						});
					});
			}
			else {//The payment was made through the web service.
				_payConnectRequest.TransType=PayConnectService.transType.VOID;
				_payConnectRequest.RefNumber=refNum;
				_payConnectRequest.Amount=PIn.Decimal(amount);
				PayConnectService.transResponse transResponse=PayConnect.ProcessCreditCard(_payConnectRequest,_paymentCur.ClinicNum,x => MessageBox.Show(x));
				response=new PayConnectResponse(transResponse,_payConnectRequest);
				receiptStr=PayConnect.BuildReceiptString(_payConnectRequest,transResponse,null,0);
			}
			Cursor=Cursors.Default;
			if(response==null || response.StatusCode!="0") {//error in transaction
				MsgBox.Show(this,"This credit card payment has already been processed and will have to be voided manually through the web interface.");
				return;
			}
			else {//Record a new payment for the voided transaction
				Payment voidPayment=_paymentCur.Clone();
				voidPayment.PayAmt*=-1; //The negated amount of the original payment
				voidPayment.Receipt=receiptStr;
				voidPayment.PayNote=Lan.g(this,"Transaction Type")+": "+Enum.GetName(typeof(PayConnectService.transType),PayConnectService.transType.VOID)
					+Environment.NewLine+Lan.g(this,"Status")+": "+response.Description+Environment.NewLine
					+Lan.g(this,"Amount")+": "+voidPayment.PayAmt+Environment.NewLine
					+Lan.g(this,"Auth Code")+": "+response.AuthCode+Environment.NewLine
					+Lan.g(this,"Ref Number")+": "+response.RefNumber;
				voidPayment.PaymentSource=CreditCardSource.PayConnect;
				voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
				voidPayment.PayNum=Payments.Insert(voidPayment);
				for(int i=0;i<_listSplitsCur.Count;i++) {//Modify the paysplits for the original transaction to work for the void transaction
					PaySplit split=_listSplitsCur[i].Copy();
					split.SplitAmt*=-1;
					split.PayNum=voidPayment.PayNum;
					PaySplits.Insert(split);
				}
				SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,voidPayment.PatNum,
					Patients.GetLim(voidPayment.PatNum).GetNameLF()+", "+voidPayment.PayAmt.ToString("c"));
			}
		}

		private void VoidPaySimpleTransaction(string refNum,string originalReceipt) {
			PaySimple.ApiResponse response=null;
			string receiptStr="";
			Cursor=Cursors.WaitCursor;
			try {
				response=PaySimple.VoidPayment(refNum,_paymentCur.ClinicNum);
			}
			catch(PaySimpleException ex) {
				MessageBox.Show(ex.Message);
				if(ex.ErrorType==PaySimpleError.CustomerDoesNotExist && MsgBox.Show(this,MsgBoxButtons.OKCancel,
					"Delete the link to the customer id for this patient?")) 
				{
					PatientLinks.DeletePatNumTos(ex.CustomerId,PatientLinkType.PaySimple);
				}
				return;
			}
			catch(ODException wex) {
				MessageBox.Show(wex.Message);//This should have already been Lans.g if applicable.
				return;
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Error:")+" "+ex.Message);
				return;
			}
			string[] arrayReceiptFields=originalReceipt.Replace("\r\n","\n").Replace("\r","\n").Split(new string[] { "\n" },StringSplitOptions.RemoveEmptyEntries);
			string ccNum="";
			string expDateStr="";
			string nameOnCard="";
			for(int i=0;i<arrayReceiptFields.Length;i++) {
				if(arrayReceiptFields[i].StartsWith("Name")) {
					nameOnCard=arrayReceiptFields[i].Substring(4).Replace(".","");
				}
				if(arrayReceiptFields[i].StartsWith("Account")) {
					ccNum=arrayReceiptFields[i].Substring(7).Replace(".","");
				}
				if(arrayReceiptFields[i].StartsWith("Exp Date")) {
					expDateStr=arrayReceiptFields[i].Substring(8).Replace(".","");
				}
			}
			//ACH payments do not have expDates. ACH payments can be voided as long as the void occurs before the batch is closed.
			int expMonth=-1;
			int expYear=-1;
			if(!string.IsNullOrEmpty(expDateStr) && expDateStr.Length > 2) {
				expMonth=PIn.Int(expDateStr.Substring(0,2));
				expYear=PIn.Int(expDateStr.Substring(2));
			}
			response.BuildReceiptString(ccNum,expMonth,expYear,nameOnCard,_paymentCur.ClinicNum);
			receiptStr=response.TransactionReceipt;
			Cursor=Cursors.Default;
			Payment voidPayment=_paymentCur.Clone();
			voidPayment.PayAmt*=-1; //The negated amount of the original payment
			voidPayment.Receipt=receiptStr;
			voidPayment.PayNote=response.ToNoteString();
			voidPayment.PaymentSource=CreditCardSource.PaySimple;
			voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
			voidPayment.PayNum=Payments.Insert(voidPayment);
			for(int i=0;i<_listSplitsCur.Count;i++) {//Modify the paysplits for the original transaction to work for the void transaction
				PaySplit split=_listSplitsCur[i].Copy();
				split.SplitAmt*=-1;
				split.PayNum=voidPayment.PayNum;
				PaySplits.Insert(split);
			}
			SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,voidPayment.PatNum,
				Patients.GetLim(voidPayment.PatNum).GetNameLF()+", "+voidPayment.PayAmt.ToString("c"));
		}

		private void menuXcharge_Click(object sender,EventArgs e) {
			if(Security.IsAuthorized(Permissions.Setup)) {
				FormXchargeSetup FormX=new FormXchargeSetup();
				FormX.ShowDialog();
				CheckUIState();
			}
		}

		private void menuPayConnect_Click(object sender,EventArgs e) {
			if(Security.IsAuthorized(Permissions.Setup)) {
				FormPayConnectSetup fpcs=new FormPayConnectSetup();
				fpcs.ShowDialog();
				CheckUIState();
			}
		}

		private void menuPaySimple_Click(object sender,EventArgs e) {
			if(Security.IsAuthorized(Permissions.Setup)) {
				FormPaySimpleSetup form=new FormPaySimpleSetup();
				form.ShowDialog();
				CheckUIState();
			}
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			//_listUserClinicNums contains all clinics the user has access to as well as ClinicNum 0 for 'none'
			_paymentCur.ClinicNum=_listUserClinicNums[comboClinic.SelectedIndex];
			if(_listSplitsCur.Count>0) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Change clinic for all splits?")) {
					return;
				}
				for(int i=0;i<_listSplitsCur.Count;i++) {
					_listSplitsCur[i].ClinicNum=_paymentCur.ClinicNum;
				}
				FillGridSplits();
			}
			CheckUIState();
		}

		private void checkPayTypeNone_CheckedChanged(object sender,EventArgs e) {
			//this fires before the click event.  The Checked property also reflects the new value.
			if(checkPayTypeNone.Checked) {
				checkShowAll.Checked=false;
				checkShowAll.Enabled=false;
				listPayType.Visible=false;
				butPay.Text=Lan.g(this,"Transfer");
				if(PrefC.HasClinicsEnabled) {
					comboGroupBy.SelectedIndex=2;
				}
				else { 
					comboGroupBy.SelectedIndex=1;
				}
				comboGroupBy.Enabled=false;
				if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
					butCreatePartial.Text=Lan.g(this,"Proc Breakdown");
					butPay.Visible=false;
				}
				else {
					butCreatePartial.Visible=false;
				}
				checkIncludeExplicitCreditsOnly.Visible=false;
			}
			else {
				checkShowAll.Enabled=true;
				listPayType.Visible=true;
				butPay.Text=Lan.g(this,"Pay");
				comboGroupBy.Enabled=true;
				butCreatePartial.Visible=true;
				butCreatePartial.Text=Lan.g(this,"Add Partials");
				butPay.Visible=true;
				checkIncludeExplicitCreditsOnly.Visible=true;
			}
			CheckUIState();
			SetComboDepositAccounts();
		}

		private void checkPayTypeNone_Click(object sender,EventArgs e) {
			List<long> listPatNumsFamily=_famCur.ListPats.Select(x => x.PatNum).ToList();
			if(checkShowSuperfamily.Checked) {
				listPatNumsFamily.AddRange(_superFamCur.ListPats.Select(x => x.PatNum).ToList());
				listPatNumsFamily=listPatNumsFamily.Distinct().ToList();
			}
			_loadData.ConstructChargesData=PaymentEdit.GetConstructChargesData(listPatNumsFamily,_patCur.PatNum,_loadData.ListSplits,_paymentCur.PayNum,checkPayTypeNone.Checked);
			Init(_loadData,false);
		}

		///<summary>Allows editing of an individual double clicked paysplit entry.</summary>
		private void gridSplits_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PaySplit paySplitOld=(PaySplit)gridSplits.ListGridRows[e.Row].Tag;
			PaySplit paySplit=paySplitOld.Copy();
			if(paySplit.DateEntry!=DateTime.MinValue && !Security.IsAuthorized(Permissions.PaymentEdit,paySplit.DatePay,false)) {
				return;
			}
			PaySplits.PaySplitAssociated splitAssociatedOld=_listPaySplitsAssociated.Find(x => x.PaySplitLinked==paySplitOld);
			FormPaySplitEdit FormPSE=new FormPaySplitEdit(_curFamOrSuperFam,checkPayTypeNone.Checked);
			FormPSE.ListSplitsCur=_listSplitsCur;
			FormPSE.PaySplitCur=paySplit;
			FormPSE.SplitAssociated=splitAssociatedOld??new PaySplits.PaySplitAssociated(null,null);
			FormPSE.ListPaySplitAssociated=_listPaySplitsAssociated;
			if(paySplit.IsInterestSplit && !MsgBox.Show(this,MsgBoxButtons.OKCancel,"Editing or deleting interest splits for a payment plan charge can"
				+" cause future splits to be allocated to the wrong provider. Do you want to continue?")) {
				return;
			}
			else if(FormPSE.ShowDialog()==DialogResult.OK) {//paySplit contains all the info we want.  
				//Delete paysplit from paysplit grid, credit the charge it's associated to.  
				//Paysplit may be re-associated with a different charge and we wouldn't know, so we need to do this.
				//Paysplits associated to payplancharge cannot be associated to a different payplancharge from this window.
				if(FormPSE.PaySplitCur==null){//remove old association from list.
					_listPaySplitsAssociated.Remove(splitAssociatedOld);
				}
				else{
					//Since DeleteSelected() deletes the current selected paysplit from the list of ListPaySplitCur, we need to do this in order to maintain object references on form closing.
					//detaching split in PSE window causes SplitAssociated to be null
					_listPaySplitsAssociated.Remove(splitAssociatedOld);
					if(FormPSE.SplitAssociated!=null && FormPSE.SplitAssociated.PaySplitOrig!=null) {
						PaySplits.PaySplitAssociated splitAssociatedNew=new PaySplits.PaySplitAssociated(FormPSE.SplitAssociated.PaySplitOrig,paySplit);
						_listPaySplitsAssociated.Add(splitAssociatedNew);
					}
				}
				List<long> listEditedCharge=DeleteSelected(paySplit);
				if(paySplit!=null && !_dictPatients.ContainsKey(paySplit.PatNum)) {
					//add new patnum to _dictPatients
					Patient pat=Patients.GetLim(paySplit.PatNum);
					if(pat!=null) {
						_dictPatients[paySplit.PatNum]=pat;
					}
				}
				long payPlanChargeNum=0;
				if(listEditedCharge.Count>0) {
					payPlanChargeNum=listEditedCharge[0];
				}
				if(FormPSE.PaySplitCur==null) {//Deleted the paysplit, just return here.
					FillGridSplits();
					return;
				}
				if(!_listSplitsCur.Contains(paySplitOld)) {//Paysplit was -actually- deleted from the list.
					_listSplitsCur.Add(paySplit);
					PaymentEdit.UpdateForManualSplit(paySplit,_listAccountCharges,payPlanChargeNum,isAllCharges:checkShowAll.Checked);
					FillGridSplits();
					//If the PaySplit we doubleclicked is in the grid set it selected and highlight associated splits.
					for(int i=0;i<gridSplits.ListGridRows.Count;i++){
						if(((PaySplit)gridSplits.ListGridRows[i].Tag).IsSame(paySplitOld)){
							gridSplits.SetSelected(i,true);
						}
					}
					_paymentCur.PayAmt-=paySplit.SplitAmt;
				}		
			}
			HighlightChargesForSplits();
		}
		
		///<summary>When a paysplit is selected this method highlights all charges associated with it.</summary>
		private void gridSplits_CellClick(object sender,ODGridClickEventArgs e) {
			HighlightChargesForSplits();
		}

		///<summary>When a charge is selected this method highlights all paysplits associated with it.</summary>
		private void gridCharges_CellClick(object sender,ODGridClickEventArgs e) {
			CellClickHelper(gridCharges);
		}

		///<summary>Takes an ODGrid and evaluates its single click events - proccessing selections based on paySplit type and entry type.
		///This method requires the passed in grids to have the same number of rows.</summary>
		private void CellClickHelper(ODGrid grid) {
			gridSplits.SetSelected(false);
			decimal chargeTotal=0;
			for(int i=0;i<grid.SelectedIndices.Length;i++) {
				if(comboGroupBy.SelectedIndex>0 && grid.ListGridRows[i].Tag is List<AccountEntry>) {
					List<AccountEntry> listAccountEntryCharges=(List<AccountEntry>)grid.ListGridRows[grid.SelectedIndices[i]].Tag;
					chargeTotal+=listAccountEntryCharges.Sum(x => x.AmountEnd);
					for(int j=0;j<gridSplits.ListGridRows.Count;j++) {
						PaySplit paySplit=(PaySplit)gridSplits.ListGridRows[j].Tag;
						foreach(AccountEntry entry in listAccountEntryCharges) {
							if(entry.SplitCollection.Contains(paySplit)) {
								gridSplits.SetSelected(j,true);
							}
							if(entry.GetType()==typeof(PayPlanCharge)) {
								if(paySplit.PayPlanNum==((PayPlanCharge)entry.Tag).PayPlanNum) {
									gridSplits.SetSelected(j,true);
								}
							}
						}
					}
				}
				else { 
					AccountEntry accountEntryCharge=(AccountEntry)grid.ListGridRows[grid.SelectedIndices[i]].Tag;
					for(int j=0;j<gridSplits.ListGridRows.Count;j++) {
						PaySplit paySplit=(PaySplit)gridSplits.ListGridRows[j].Tag;
						if(accountEntryCharge.SplitCollection.Contains(paySplit)) {
							gridSplits.SetSelected(j,true);
						}
						if(accountEntryCharge.GetType()==typeof(PayPlanCharge)) {
							if(paySplit.PayPlanNum==((PayPlanCharge)accountEntryCharge.Tag).PayPlanNum) {
								gridSplits.SetSelected(j,true);
							}
						}
					}
					chargeTotal+=accountEntryCharge.AmountEnd;
				}
			}
			if(grid==gridCharges){
				textChargeTotal.Text=chargeTotal.ToString("f");
			}
		}

		///<summary>When a Treatment Plan Procedure is selected this method highlights all paysplits associated with it.</summary>
		private void gridTreatPlan_CellClick(object sender,ODGridClickEventArgs e) {
			CellClickHelper(gridTreatPlan);
		}

		///<summary>Highlights the charges that corresponds to the selected paysplit.</summary>
		private void HighlightChargesForSplits() {
			gridCharges.SetSelected(false);
			gridTreatPlan.SetSelected(false);
			for(int i=0;i<gridSplits.SelectedIndices.Length;i++) {
				PaySplit paySplit=(PaySplit)gridSplits.ListGridRows[gridSplits.SelectedIndices[i]].Tag;
				HighlightChargesForSplitsHelper(gridCharges,paySplit);//Perform Selections for gridCharges
				HighlightChargesForSplitsHelper(gridTreatPlan,paySplit);//Perform Selections fo gridTreatPlan
				for(int k = 0;k<gridSplits.ListGridRows.Count;k++) {
					PaySplit splitAttached=(PaySplit)gridSplits.ListGridRows[k].Tag;
					PaySplits.PaySplitAssociated psAssociated=_listPaySplitsAssociated.Find(x=>x.PaySplitOrig==paySplit && x.PaySplitLinked==splitAttached);
					if(psAssociated!=null) {
						gridSplits.SetSelected(k,true);
					}
				}
			}
		}

		///<summary>Given an ODGrid and the associated paySplits from gridSplits, this method will do selection logic to show which paySplits are associated
		///to which gridRows in the passed in ODGrid.</summary>
		private void HighlightChargesForSplitsHelper(ODGrid grid,PaySplit paySplit) {
			for(int j=0;j<grid.ListGridRows.Count;j++) {
				if(comboGroupBy.SelectedIndex>0 && grid.ListGridRows[j].Tag is List<AccountEntry>) {
					List<AccountEntry> listEntriesForProv=(List<AccountEntry>)grid.ListGridRows[j].Tag;
					foreach(AccountEntry entry in listEntriesForProv) {
						if(entry.SplitCollection.Contains(paySplit)) {
							grid.SetSelected(j,true);
						}
					}
				}
				else {
					AccountEntry accountEntryCharge=(AccountEntry)grid.ListGridRows[j].Tag;
					if(accountEntryCharge.SplitCollection.Contains(paySplit)) {
						grid.SetSelected(j,true);
					}
				}
			}
		}

		///<summary>Deletes selected paysplits from the grid and attributes amounts back to where they originated from.
		///This will return a list of payment plan charges that were affected. This is so that splits can be correctly re-attributed to the payplancharge
		///when the user edits the paysplit. There should only ever be one payplancharge in that list, since the user can only edit one split at a time.</summary>
		private List<long> DeleteSelected(PaySplit paySplitToBeAdded = null) {
			bool suppressMessage=false;
			//we need to return the payplancharge that the paysplit was associated to so that this paysplit can be correctly re-attributed to that charge.
			List<long> listPayPlanChargeNum=new List<long>();
			for(int i=gridSplits.SelectedIndices.Length-1;i>=0;i--) {
				int idx=gridSplits.SelectedIndices[i];
				PaySplit paySplit=(PaySplit)gridSplits.ListGridRows[idx].Tag;
				if(paySplit.SplitNum!=0 && _listPaySplitAllocations.FindAll(x => x.FSplitNum==paySplit.SplitNum && x.PayNum!=paySplit.PayNum).Count>0) {
					MsgBox.Show(this,"Splits in this payment have been allocated elsewhere.  Please delete those first.");
					return new List<long>();
				}
				if(paySplit.DateEntry!=DateTime.MinValue && !Security.IsAuthorized(Permissions.PaymentEdit,paySplit.DatePay,suppressMessage)) {
					suppressMessage=true;
					continue;//Don't delete this paysplit
				}
				if(paySplit.PayPlanNum!=0) {
					foreach(AccountEntry charge in _listAccountCharges) {
						if(charge.GetType()!=typeof(PayPlanCharge)) {//If the charge is not a payplancharge, we don't care
							continue;
						}
						if(!charge.SplitCollection.Contains(paySplit)) {//Only care about the charge if this paysplit is for that charge.
							continue;
						}
						if(paySplit.SplitAmt<=0) {
							break;
						}
						//It is now a charge for the payplan
						//When a split is deleted, put the money back on the payplan charge
						listPayPlanChargeNum.Add(charge.PriKey);
						decimal chargeAmtNew=charge.AmountEnd+(decimal)paySplit.SplitAmt;//Take the current value of the charge and add the split amt to it
						if(Math.Abs(chargeAmtNew)>Math.Abs(charge.AmountStart)) {//The split has more in it than the debit can take, use only a part of it
							//Find out how much of the split goes into the debit
							decimal debitDifference=charge.AmountStart-charge.AmountEnd;
							paySplit.SplitAmt-=(double)debitDifference;
							charge.AmountEnd=charge.AmountStart;
							_paymentCur.PayAmt+=(double)debitDifference;
							if(paySplit.ProcNum!=0) { 
								Procedure proc=(Procedure)_listAccountCharges.Find(x => x.GetType()==typeof(Procedure) && x.PriKey==paySplit.ProcNum).Tag;
								proc.ProcFee+=(double)debitDifference;//Put money back on the procfee so when we add splits later it calculates correctly
							}
						}
						else {
							charge.AmountEnd+=(decimal)paySplit.SplitAmt;//Give the money back to the charge so it will display.  Uses full paysplit amount.
							_paymentCur.PayAmt+=paySplit.SplitAmt;
							if(paySplit.ProcNum!=0) {
								Procedure proc=(Procedure)_listAccountCharges.Find(x => x.GetType()==typeof(Procedure) && x.PriKey==paySplit.ProcNum).Tag;
								proc.ProcFee+=paySplit.SplitAmt;//Put money back on the procfee so when we add splits later it calculates correctly
							}
							paySplit.SplitAmt=0;
						}
						charge.SplitCollection.Remove(paySplit);
					}
				}
				else if(paySplit.ProcNum!=0) { 
					for(int j=0;j<_listAccountCharges.Count;j++) {
						AccountEntry charge=_listAccountCharges[j];
						if(!charge.SplitCollection.Contains(paySplit)) {
							continue;
						}
						charge.AmountEnd+=(decimal)paySplit.SplitAmt;//Give the money back to the charge so it will display.
						charge.SplitCollection.Remove(paySplit);
					}
				}
				else if(paySplit.AdjNum!=0) {
					for(int j=0;j<_listAccountCharges.Count;j++) {
						AccountEntry charge=_listAccountCharges[j];
						if(!charge.SplitCollection.Contains(paySplit)) {
							continue;
						}
						charge.AmountEnd+=(decimal)paySplit.SplitAmt;//Give the money back to the charge so it will display.
						charge.SplitCollection.Remove(paySplit);
					}
				}
				else if(paySplit.FSplitNum!=0) {//Most likely a negative split used to income transfer.
					for(int j=0;j<_listAccountCharges.Count;j++) {
						AccountEntry charge=_listAccountCharges[j];
						if(!charge.SplitCollection.Contains(paySplit)) {
							continue;
						}
						charge.AmountEnd+=(decimal)paySplit.SplitAmt;
					}
				}
				else if(checkPayTypeNone.Checked) {//payment window income transfers don't match any of the previous criteria but need to be handled.
					for(int j=0;j<_listAccountCharges.Count;j++) {
						AccountEntry charge=_listAccountCharges[j];
						if(!charge.SplitCollection.Contains(paySplit)) {
							continue;
						}
						charge.AmountEnd+=(decimal)paySplit.SplitAmt;
						charge.SplitCollection.Remove(paySplit);
					}
				}
				else {//Implicit payments
					//Do nothing.  Continue showing the charge as owing $0.
				}
				if(paySplitToBeAdded!=null) {
					//We need to do this in order to maintain object references on form closing. 		
					UpdateListAssociationsBeforeDelete(paySplit,paySplitToBeAdded);
				}
				else {
					//Remove pay split associated if Split is deleted and not replaced.
					_listPaySplitsAssociated.RemoveAll(x => x.PaySplitOrig.IsSame(paySplit) || x.PaySplitLinked.IsSame(paySplit));
				}
				_listSplitsCur.Remove(paySplit);
			}
			return listPayPlanChargeNum;
		}

		///<summary>Goes through _listPaySplitsAssociated and replaces the paySplitToBeDeleted with paySplitToBeAdded.</summary>
		private void UpdateListAssociationsBeforeDelete(PaySplit paySplitToBeDeleted,PaySplit paySplitToBeAdded) {
			foreach(PaySplits.PaySplitAssociated payAssociatedLink in _listPaySplitsAssociated.Where(x => x.PaySplitLinked==paySplitToBeDeleted)) {
				payAssociatedLink.PaySplitLinked=paySplitToBeAdded;
			}
			foreach(PaySplits.PaySplitAssociated payAssociatedOrig in _listPaySplitsAssociated.FindAll(x => x.PaySplitOrig==paySplitToBeDeleted)) {
				payAssociatedOrig.PaySplitOrig=paySplitToBeAdded;
			}
		}

		///<summary>Creates a split similar to how CreateSplitsForPayment does it, but with selected rows of the grid.
		///If payAmt==0, attempt to pay charge in full.</summary>
		private void CreateSplit(AccountEntry charge,decimal payAmt,bool isManual=false) {
			PaymentEdit.PayResults createdSplit=PaymentEdit.CreatePaySplit(charge.AccountEntryNum,payAmt,_paymentCur,PIn.Decimal(textAmount.Text)
				,_listSplitsCur,_listAccountCharges,isManual);
			_listSplitsCur=createdSplit.ListSplitsCur;
			_listAccountCharges=createdSplit.ListAccountCharges;
			_paymentCur=createdSplit.Payment;
		}

		///<summary>Creates paysplits associated to the patient passed in for the current payment until the payAmt has been met.  
		///Returns the list of new paysplits that have been created.  PaymentAmt will attempt to move toward 0 as paysplits are created.</summary>
		private List<PaySplit> AutoSplitForPayment(DateTime date,PaymentEdit.LoadData loadData=null) {
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(_listPatNums,_patCur.PatNum,_listSplitsCur,_paymentCur
				,ListEntriesPayFirst,checkPayTypeNone.Checked,_preferCurrentPat,loadData);
			//Create Auto-splits for the current payment to any remaining non-zero charges FIFO by date.
			//At this point we have a list of procs, positive adjustments, and payplancharges that require payment if the Amount>0.   
			//Create and associate new paysplits to their respective charge items.
			PaymentEdit.AutoSplit autoSplit=PaymentEdit.AutoSplitForPayment(constructResults);
			_listAccountCharges=autoSplit.ListAccountCharges;
			_listSplitsCur=autoSplit.ListSplitsCur;
			_paymentCur.PayAmt=autoSplit.Payment.PayAmt;
			return autoSplit.ListAutoSplits;
		}

		private void butDeleteSplits_Click(object sender,EventArgs e) {
			for(int i = 0;i<gridSplits.SelectedIndices.Length;i++) {
				if(((PaySplit)gridSplits.ListGridRows[gridSplits.SelectedIndices[i]].Tag).IsInterestSplit) {
					if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"Deleting interest splits for a payment plan charge can cause line item accounting"
						+" to appear to be off in the payment window. Do you want to continue?")) {
						break;//they clicked continue.
					}
					else {
						return;//return out of this method -- they don't want to delete.
					}
				}
			}
			DeleteSelected();
			FillGridSplits();
		}

		///<summary>Deletes all payment splits in the grid.</summary>
		private void butDeleteAllSplits_Click(object sender,EventArgs e) {
			gridSplits.SetSelected(true);
			DeleteSelected();
			FillGridSplits();
		}

		///<summary>Creates a paysplit for the user to edit manually.</summary>
		private void butAddManualSplit_Click(object sender,EventArgs e) {
			PaySplit paySplit=new PaySplit();
			paySplit.SplitAmt=0;
			paySplit.DatePay=_paymentCur.PayDate;
			paySplit.DateEntry=MiscData.GetNowDateTime();//just a nicety for the user.  Insert uses server time.
			paySplit.PayNum=_paymentCur.PayNum;
			paySplit.ProvNum=Patients.GetProvNum(_patCur);
			paySplit.ClinicNum=_paymentCur.ClinicNum;
			paySplit.IsNew=true;
			FormPaySplitEdit FormPSE=new FormPaySplitEdit(_curFamOrSuperFam,checkPayTypeNone.Checked);
      FormPSE.ListSplitsCur=_listSplitsCur;
			FormPSE.PaySplitCur=paySplit;
			FormPSE.IsNew=true;
			FormPSE.ListPaySplitAssociated=_listPaySplitsAssociated;
			if(FormPSE.ShowDialog()==DialogResult.OK) {
				if(!_dictPatients.ContainsKey(paySplit.PatNum)) {
					//add new patnum to _dictPatients
					Patient pat=Patients.GetLim(paySplit.PatNum);
					if(pat!=null) {
						_dictPatients[paySplit.PatNum]=pat;
					}
				}
				long prePaymentOrigNum=0;
				if(FormPSE.SplitAssociated!=null && FormPSE.SplitAssociated.PaySplitOrig!=null && FormPSE.SplitAssociated.PaySplitLinked!=null) {
					if(paySplit.SplitAmt<0) {
						//if prepayment, check the charge grid for the original prepayment so we can update the charge grid amounts.
						List<AccountEntry> listAccountEntries=new List<AccountEntry>();
						if(comboGroupBy.SelectedIndex!=0) {//Either 'Provider' or 'Clinic and Provider' selected
							listAccountEntries=gridCharges.ListGridRows.SelectMany(x => x.Tag as List<AccountEntry>).ToList();
						}
						else {//'None' selected
							listAccountEntries=gridCharges.ListGridRows.Select(x => x.Tag as AccountEntry).ToList();
						}
						AccountEntry charge=listAccountEntries.FirstOrDefault(x => x.PriKey==FormPSE.SplitAssociated.PaySplitOrig.SplitNum);
						if(charge!=null) {
							prePaymentOrigNum=charge.PriKey;
						}
					}
					_listPaySplitsAssociated.Add(FormPSE.SplitAssociated);
				}
				_listSplitsCur.Add(paySplit);
				PaymentEdit.UpdateForManualSplit(paySplit,_listAccountCharges,FormPSE.PayPlanChargeNum,prePaymentOrigNum,checkShowAll.Checked);
				FillGridSplits();
			}
		}

		///<summary>This method goes through the passed in grid and creates paysplit(s) for selected procedures.</summary>
		private void PayClickHelper(ODGrid grid,List<List<AccountEntry>> listSelectedEntries) {
			if(grid.SelectedIndices.Length==0) {//Nothing selected, Select All
				foreach(GridRow row in grid.ListGridRows) {
					if(comboGroupBy.SelectedIndex>0 && row.Tag is List<AccountEntry>) {//Group by provider and/or clinic
						listSelectedEntries.Add((List<AccountEntry>)row.Tag);
					}
					else {
						listSelectedEntries.Add(new List<AccountEntry> { (AccountEntry)row.Tag });
					}
				}
			}
			else { 
				for(int i=0;i<grid.SelectedIndices.Length;i++) {
					if(comboGroupBy.SelectedIndex>0 && grid.ListGridRows[grid.SelectedIndices[i]].Tag is List<AccountEntry>) {//Group by provider and/or clinic
						listSelectedEntries.Add((List<AccountEntry>)grid.ListGridRows[grid.SelectedIndices[i]].Tag);
					}
					else {
						listSelectedEntries.Add(new List<AccountEntry> { (AccountEntry)grid.ListGridRows[grid.SelectedIndices[i]].Tag });
					}
				}
			}
		}

		private void butPay_Click(object sender,EventArgs e) {
			_paymentCur.PayAmt=PIn.Double(textAmount.Text)-_listSplitsCur.Sum(x => x.SplitAmt);
			tabControlSplits.SelectedIndex=0;
			List<List<AccountEntry>> listSelectedEntries=new List<List<AccountEntry>>();
			if(tabProcCharges.SelectedTab==tabPageCharges){
				PayClickHelper(gridCharges,listSelectedEntries);
			}
			else if(tabProcCharges.SelectedTab==tabPageTreatPlan){
				PayClickHelper(gridTreatPlan,listSelectedEntries);
			}
			PaymentEdit.PayResults createdSplits=PaymentEdit.MakePayment(listSelectedEntries,_listSplitsCur,checkShowAll.Checked,_paymentCur
				,comboGroupBy.SelectedIndex,checkPayTypeNone.Checked,PIn.Decimal(textAmount.Text),_listAccountCharges);
			_listSplitsCur=createdSplits.ListSplitsCur;
			_listAccountCharges=createdSplits.ListAccountCharges;
			_paymentCur=createdSplits.Payment;
			_paymentCur.PayAmt=PIn.Double(textAmount.Text);//Reset it
			FillGridSplits();//Fills charge grid too.
		}

		///<summary>A method which, for a given grid, allows the user to split a payment between procedures on it. 
		///doCheckOverPaidProc is true by default, this boolean will cause an popup message for the user if the amount 
		///allocated exceeds the amount due for a selected procedure from the grid.</summary>
		private void CreatePartialSplitClickHelper(ODGrid grid,bool doCheckOverPaidProc=true) {
			for(int i=0;i<grid.SelectedIndices.Length;i++) {
				string chargeDescript="";
				if(comboGroupBy.SelectedIndex>0 && grid.ListGridRows[grid.SelectedIndices[i]].Tag is List<AccountEntry>) {
					chargeDescript=grid.ListGridRows[grid.SelectedIndices[i]].Cells[0].Text;
					FormAmountEdit FormAE=new FormAmountEdit(chargeDescript);
					FormAE.Amount=PIn.Decimal(grid.ListGridRows[grid.SelectedIndices[i]].Cells[4].Text);
					FormAE.ShowDialog();
					if(FormAE.DialogResult==DialogResult.OK) {
						decimal amount=FormAE.Amount;
						if(amount==0) {
							continue;
						}
						List<AccountEntry> listEntries=(List<AccountEntry>)grid.ListGridRows[grid.SelectedIndices[i]].Tag;
						//Order the list by AmountEnd to create all of paysplit that do not result in an overpayment before allowing the user to cancel out. 
						listEntries=listEntries.OrderByDescending(x => x.AmountEnd).ToList();
						for(int j=0;j<listEntries.Count;j++) {
							AccountEntry entry=listEntries[j];
							if(doCheckOverPaidProc && j==listEntries.Count-1 && PaymentEdit.IsProcOverPaid(amount,entry)) {//if last account entry and overpaying
								if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"At least one of the procedure's remaining amount is negative. Continue?")) {
									FillGridSplits();//Fills charge grid too.
									return;//return to the payment window. 
								}
								doCheckOverPaidProc=false;//user wants to manually create a split that overpays the charge. No need to check for overpaid procedures again.
							}
							CreateSplit(entry,amount,!doCheckOverPaidProc);
							if(_listSplitsCur.Count>0) {
								amount-=(decimal)_listSplitsCur[_listSplitsCur.Count-1].SplitAmt;
							}
							if(amount.IsLessThanOrEqualToZero()) {
								break;
							}
						}
					}
				}
				else { 
					if(PrefC.HasClinicsEnabled) {//Clinics
						chargeDescript=grid.ListGridRows[grid.SelectedIndices[i]].Cells[4].Text;
					}
					else {
						chargeDescript=grid.ListGridRows[grid.SelectedIndices[i]].Cells[3].Text;
					}
					FormAmountEdit FormAE=new FormAmountEdit(chargeDescript);
					AccountEntry selectedEntry=(AccountEntry)grid.ListGridRows[grid.SelectedIndices[i]].Tag;
					FormAE.Amount=selectedEntry.AmountEnd;
					FormAE.ShowDialog();
					if(FormAE.DialogResult==DialogResult.OK) {
						decimal amount=FormAE.Amount;
						if(amount!=0) {
							if(PaymentEdit.IsProcOverPaid(amount,selectedEntry) && !MsgBox.Show(this,MsgBoxButtons.YesNo,"Remaining amount is negative. Continue?")) {
								break;
							}
							CreateSplit(selectedEntry,amount,true);
						}
					}
				}
			}
		}

		private void butCreatePartialSplit_Click(object sender,EventArgs e) {
			if(checkPayTypeNone.Checked) {//Button only visible if EnforceFully is enabled
				FormPaySplitManage FormPSM=new FormPaySplitManage(checkPayTypeNone.Checked);
				FormPSM.ListSplitsCur=_listSplitsCur;
				FormPSM.ListSplitsAssociated=_listPaySplitsAssociated;
				FormPSM.PayDate=PIn.Date(textDate.Text);
				FormPSM.PatCur=_patCur;
				FormPSM.PaymentAmt=0;
				FormPSM.PaymentCur=_paymentCur;
				FormPSM.FamCur=_famCur;
				if(FormPSM.ShowDialog()==DialogResult.OK) {
					FillGridSplits();
				}
				return;
			}
			if(tabProcCharges.SelectedTab==tabPageCharges){
				CreatePartialSplitClickHelper(gridCharges);
			}
			else if(tabProcCharges.SelectedTab==tabPageTreatPlan){
				CreatePartialSplitClickHelper(gridTreatPlan);
			}
			FillGridSplits();//Fills charge grid too.
		}

		private void checkIncludeExplicitCreditsOnly_CheckedChanged(object sender,EventArgs e) {
			List<long> listPatNumsFamily=_famCur.ListPats.Select(x => x.PatNum).ToList();
			if(_patCur.SuperFamily>0 && checkShowSuperfamily.Checked) {
				listPatNumsFamily.AddRange(_superFamCur.ListPats.Select(x => x.PatNum).ToList());
				listPatNumsFamily=listPatNumsFamily.Distinct().ToList();
			}
			_loadData.ConstructChargesData=PaymentEdit.GetConstructChargesData(listPatNumsFamily,_patCur.PatNum,_loadData.ListSplits,_paymentCur.PayNum,checkPayTypeNone.Checked);
			Init(_loadData,false);
		}

		private void checkShowAll_Clicked(object sender,EventArgs e) {
			RefreshGridsAndSelections();
		}

		///<summary>Constructs a list of AccountCharges and goes through and links those charges to credits.</summary>
		private void checkShowSuperfamily_Click(object sender,EventArgs e) {
			if(_patCur.SuperFamily==0) { //if no super family, just return.
				return;
			}
			List<long> listPatNumsFamily=_famCur.ListPats.Select(x => x.PatNum).ToList();
			if(checkShowSuperfamily.Checked) {
				listPatNumsFamily.AddRange(_superFamCur.ListPats.Select(x => x.PatNum).ToList());
				listPatNumsFamily=listPatNumsFamily.Distinct().ToList();
			}
			_loadData.ConstructChargesData=PaymentEdit.GetConstructChargesData(listPatNumsFamily,_patCur.PatNum,_loadData.ListSplits,_paymentCur.PayNum,checkPayTypeNone.Checked);
			Init(_loadData,false);
		}

		private void comboGroupBy_SelectionChangeCommitted(object sender,EventArgs e) {
			//Go through and disable/enable filters depending on the group by state.
			if(comboGroupBy.SelectedIndex==1) {	//Group By Providers
				comboTypeFilter.ArraySelectedIndices=new int[] { 0 };	//Make sure "All" is selected.
				comboClinicFilter.ArraySelectedIndices=new int[] { 0 };
				comboTypeFilter.Enabled=false;
				comboClinicFilter.Enabled=false;
			}
			else if(comboGroupBy.SelectedIndex==2) {	//Group by providers and clinics
				comboTypeFilter.ArraySelectedIndices=new int[] { 0 };
				comboTypeFilter.Enabled=false;
				comboClinicFilter.Enabled=true;
			}
			else {		//Not grouping by anything
				comboTypeFilter.Enabled=true;
				comboClinicFilter.Enabled=true;
			}
			FillGridSplits();
		}

		private void butDeletePayment_Click(object sender,System.EventArgs e) {
			if(textDeposit.Visible) {//this will get checked again by the middle layer
				MsgBox.Show(this,"This payment is attached to a deposit.  Not allowed to delete.");
				return;
			}
			if(PaySplits.GetSplitsForPrepay(_listSplitsCur).Count>0) {
				MsgBox.Show(this,"This prepayment has been allocated.  Not allowed to delete.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will delete the entire payment and all splits.")) {
				return;
			}
			//If payment is attached to a transaction which is more than 48 hours old, then not allowed to delete.
			//This is hard coded.  User would have to delete or detach from within transaction rather than here.
			Transaction trans=Transactions.GetAttachedToPayment(_paymentCur.PayNum);
			if(trans != null) {
				if(trans.DateTimeEntry < MiscData.GetNowDateTime().AddDays(-2)) {
					MsgBox.Show(this,"Not allowed to delete.  This payment is already attached to an accounting transaction.  You will need to detach it from "
						+"within the accounting section of the program.");
					return;
				}
				if(Transactions.IsReconciled(trans)) {
					MsgBox.Show(this,"Not allowed to delete.  This payment is attached to an accounting transaction that has been reconciled.  You will need "
						+"to detach it from within the accounting section of the program.");
					return;
				}
				try {
					Transactions.Delete(trans);
				}
				catch(ApplicationException ex) {
					MessageBox.Show(ex.Message);
					return;
				}
			}
			try {
				Payments.Delete(_paymentCur);
			}
			catch(ApplicationException ex) {//error if attached to deposit slip
				MessageBox.Show(ex.Message);
				return;
			}
			if(!IsNew) {
				SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,_paymentCur.PatNum,"Delete for: "+Patients.GetLim(_paymentCur.PatNum).GetNameLF()+", "
					+_paymentOld.PayAmt.ToString("c")+", with payment type '"+Payments.GetPaymentTypeDesc(_paymentOld,_listPaymentTypeDefs)+"'",
					0,_paymentOld.SecDateTEdit);
			}
			DialogResult=DialogResult.OK;
		}

		private void butPrintReceipt_Click(object sender,EventArgs e) {
			PrintReceipt(_paymentCur.Receipt,Lan.g(this,"Receipt printed"));
		}

		private void butEmailReceipt_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.EmailSend)){
				return;
			}
			if(string.IsNullOrWhiteSpace(_paymentCur.Receipt)) {
				MsgBox.Show(this,"There is no receipt to send for this payment.");
				return;
			}
			List<string> errors=new List<string>();
			if(!EmailAddresses.ExistsValidEmail()) {
				errors.Add(Lan.g(this,"SMTP server name missing in e-mail setup."));
			}
			if(PrefC.AtoZfolderUsed==DataStorageType.InDatabase){
				errors.Add(Lan.g(this,"No AtoZ folder."));
			}
			if(errors.Count>0) {
				MessageBox.Show(this,Lan.g(this,"The following errors need to be resolved before creating an email")+":\r\n"+string.Join("\r\n",errors));
				return;
			}
			string attachPath=EmailAttaches.GetAttachPath();
			Random rnd=new Random();
			string tempFile=ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),
				DateTime.Now.ToString("yyyyMMdd")+"_"+DateTime.Now.TimeOfDay.Ticks.ToString()+rnd.Next(1000).ToString()+".pdf");
			PdfDocumentRenderer pdfRenderer=new PdfDocumentRenderer(true,PdfFontEmbedding.Always);
			pdfRenderer.Document=CreatePDFDoc(_paymentCur.Receipt);
			pdfRenderer.RenderDocument();
			pdfRenderer.PdfDocument.Save(tempFile);
			FileAtoZ.Copy(tempFile,FileAtoZ.CombinePaths(attachPath,Path.GetFileName(tempFile)),FileAtoZSourceDestination.LocalToAtoZ);
			EmailMessage message=new EmailMessage();
			message.PatNum=_paymentCur.PatNum;
			message.ToAddress=_patCur.Email;
			EmailAddress address=EmailAddresses.GetByClinic(_patCur.ClinicNum);
			message.FromAddress=address.GetFrom();
			message.Subject=Lan.g(this,"Receipt for payment received ")+_paymentCur.PayDate.ToShortDateString();
			EmailAttach attachRcpt=new EmailAttach() {
				DisplayedFileName="Receipt.pdf",
				ActualFileName=Path.GetFileName(tempFile)
			};
			message.Attachments=new List<EmailAttach>() { attachRcpt };
			FormEmailMessageEdit FormE=new FormEmailMessageEdit(message,address);
			FormE.IsNew=true;
			FormE.ShowDialog();
		}

		private void PrintReceipt(string receiptStr,string strAuditDescription) {//TODO: Implement ODprintout pattern - MigraDoc
			MigraDocPrintDocument printdoc=new MigraDocPrintDocument(new DocumentRenderer(CreatePDFDoc(receiptStr)));
			printdoc.Renderer.PrepareDocument();
#if DEBUG
			FormRpPrintPreview pView=new FormRpPrintPreview(printdoc);
			pView.ShowDialog();
#else
			if(PrinterL.SetPrinter(_pd2,PrintSituation.Receipt,_patCur.PatNum,strAuditDescription)) {
				printdoc.PrinterSettings=_pd2.PrinterSettings;
				try {
					printdoc.Print();
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.g(this,"Unable to print receipt")+". "+ex.Message);
				}
			}
#endif
		}

		private MigraDoc.DocumentObjectModel.Document CreatePDFDoc(string receiptStr) {
			string[] receiptLines=receiptStr.Split(new string[] { "\r\n" },StringSplitOptions.None);
			MigraDoc.DocumentObjectModel.Document doc=new MigraDoc.DocumentObjectModel.Document();
			doc.DefaultPageSetup.PageWidth=Unit.FromInch(3.0);
			doc.DefaultPageSetup.PageHeight=Unit.FromInch(0.181*receiptLines.Length+0.56);//enough to print text plus 9/16 in. (0.56) extra space at bottom.
			doc.DefaultPageSetup.TopMargin=Unit.FromInch(0.25);
			doc.DefaultPageSetup.LeftMargin=Unit.FromInch(0.25);
			doc.DefaultPageSetup.RightMargin=Unit.FromInch(0.25);
			MigraDoc.DocumentObjectModel.Font bodyFontx=MigraDocHelper.CreateFont(8,false);
			bodyFontx.Name=FontFamily.GenericMonospace.Name;
			Section section=doc.AddSection();
			Paragraph par=section.AddParagraph();
			ParagraphFormat parformat=new ParagraphFormat();
			parformat.Alignment=ParagraphAlignment.Left;
			parformat.Font=bodyFontx;
			par.Format=parformat;
			par.AddFormattedText(receiptStr,bodyFontx);
			return doc;
		}

		private void butPrePay_Click(object sender,EventArgs e) {
			if(PIn.Double(textAmount.Text)==0) {
				MsgBox.Show(this,"Amount cannot be zero.");
				return;
			}
			if(_listSplitsCur.Count>0) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"This will replace all Payment Splits with one split for the total amount.  Continue?")) {
					return;
				}
			}
			_listSplitsCur.Clear();
			PaySplit split=new PaySplit();
			split.PatNum=_patCur.PatNum;
			split.PayNum=_paymentCur.PayNum;
			split.FSplitNum=0;
			split.SplitAmt=PIn.Double(textAmount.Text);
			split.DatePay=DateTime.Now;
			split.ClinicNum=_paymentCur.ClinicNum;
			split.UnearnedType=PrefC.GetLong(PrefName.PrepaymentUnearnedType);
			_listSplitsCur.Add(split);
			FillGridSplits();
			Application.DoEvents();
			if(!SavePaymentToDb()) {
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private bool SavePaymentToDb() {
			if(textDate.errorProvider1.GetError(textDate)!="" || textAmount.errorProvider1.GetError(textAmount)!="") {
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return false;
			}
			if(PIn.Date(textDate.Text).Date > DateTime.Today.Date
					&& !PrefC.GetBool(PrefName.FutureTransDatesAllowed) && !PrefC.GetBool(PrefName.AccountAllowFutureDebits))
			{
				MsgBox.Show(this,"Payment date cannot be in the future.");
				return false;
			}
			if(checkPayTypeNone.Checked) {
				if(PIn.Double(textAmount.Text)!=0) {
					MsgBox.Show(this,"Amount must be zero for a transfer.");
					return false;
				}
			}
			else {
				double amt=PIn.Double(textAmount.Text);
				if(amt==0 && _listSplitsCur.Count==0) {
					MessageBox.Show(Lan.g(this,"Please enter an amount or create payment splits."));
					return false;
				}
				if(amt!=0 && listPayType.SelectedIndex==-1) {
					MsgBox.Show(this,"A payment type must be selected.");
					return false;
				}
			}
			if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
				if(_listSplitsCur.Any(x => x.ProcNum==0 && x.UnearnedType==0 && x.AdjNum==0)) {//if no procs and no adjust and not an unearned type
					MsgBox.Show(this,"A procedure, adjustment, or unearned type must be selected for each of the payment splits.");
					return false;
				}
			}
			double unearnedCur=_listSplitsCur.FindAll(x => x.UnearnedType>0).Sum(x => x.SplitAmt);
			if(unearnedCur.IsLessThan(0)
				&& !(unearnedCur+(double)PaySplits.GetUnearnedForFam(_famCur,payNumExcluded:_paymentCur.PayNum)).IsGreaterThanOrEqualToZero()
				&& !MsgBox.Show(this,MsgBoxButtons.YesNo,"You are attempting to create a negative unearned amount. Continue?"))
			{
				return false;
			}
			object[] parameters={ listPayType.SelectedItem,textNote.Text,_isCCDeclined,_paymentCur };
			Plugins.HookAddCode(this,"FormPayment.SavePaymentToDb_afterUnearnedCurCheck",parameters);
			textNote.Text=(string)parameters[1];
			_isCCDeclined=(bool)parameters[2];
			if(_isCCDeclined) {
				textAmount.Text=0.ToString("f");//So that a declined transaction does not affect account balance
				_listSplitsCur.ForEach(x => x.SplitAmt=0);
				textSplitTotal.Text=0.ToString("f");
			}
			if(IsNew) {
				//prevents backdating of initial payment
				if(!Security.IsAuthorized(Permissions.PaymentCreate,PIn.Date(textDate.Text))) {
					return false;
				}
			}
			else {
				//Editing an old entry will already be blocked if the date was too old, and user will not be able to click OK button
				//This catches it if user changed the date to be older. If user has SplitCreatePastLockDate permission and has not changed the date, then
				//it is okay to save the payment.
				if((!Security.IsAuthorized(Permissions.SplitCreatePastLockDate,true)
					|| _paymentOld.PayDate!=PIn.Date(textDate.Text))
					&& !Security.IsAuthorized(Permissions.PaymentEdit,PIn.Date(textDate.Text))) {
					return false;
				}
			}
			bool accountingSynchRequired=false;
			double accountingOldAmt=_paymentCur.PayAmt;
			long accountingNewAcct=-1;//the old acctNum will be retrieved inside the validation code.
			if(textDepositAccount.Visible) {//Not visable when IsNew or _loadData.Transaction is null or if listPayType is clicked.
				accountingNewAcct=-1;//indicates no change
			}
			else if(comboDepositAccount.Visible && comboDepositAccount.Items.Count>0 && comboDepositAccount.SelectedIndex!=-1) {
				//comboDepositAccount is set invisible when IsNew is false 
				//or if listPayType.SelectedIndex==-1 || checkPayTypeNone.Checked and IsNew and PrefName.PaymentClinicSetting is PayClinicSetting.PatientDefaultClinic
				//or if AccountingAutoPay can not be found based on listPayType.SelectedIndex.
				accountingNewAcct=_arrayDepositAcctNums[comboDepositAccount.SelectedIndex];
			}
			else {//neither textbox nor combo visible. Or something's wrong with combobox
				accountingNewAcct=0;
			}
			try {
				accountingSynchRequired=Payments.ValidateLinkedEntries(accountingOldAmt,PIn.Double(textAmount.Text),IsNew,
					_paymentCur.PayNum,accountingNewAcct);
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);//not able to alter, so must not allow user to continue.
				return false;
			}
			if(_paymentCur.ProcessStatus!=ProcessStat.OfficeProcessed) {
				if(checkProcessed.Checked) {
					_paymentCur.ProcessStatus=ProcessStat.OnlineProcessed;
				}
				else {
					_paymentCur.ProcessStatus=ProcessStat.OnlinePending;
				}
			}
			_paymentCur.PayAmt=PIn.Double(textAmount.Text);//handles blank
			_paymentCur.PayDate=PIn.Date(textDate.Text);
			#region Recurring charge logic
			//User chose to have a recurring payment so we need to know if the card has recurring setup and which month to apply the payment to.
			if(IsNew && checkRecurring.Checked && comboCreditCards.SelectedIndex!=_listCreditCards.Count) {
				//Check if a recurring charge is setup for the selected card.
				if(_listCreditCards[comboCreditCards.SelectedIndex].ChargeAmt==0 
					|| _listCreditCards[comboCreditCards.SelectedIndex].DateStart.Year < 1880) 
				{
					MsgBox.Show(this,"The selected credit card has not been setup for recurring charges.");
					return false;
				}
				//Check if a stop date was set and if that date falls in on today or in the past.
				if(_listCreditCards[comboCreditCards.SelectedIndex].DateStop.Year > 1880
					&& _listCreditCards[comboCreditCards.SelectedIndex].DateStop<=DateTime.Now) 
				{
					MsgBox.Show(this,"This card is no longer accepting recurring charges based on the stop date.");
					return false;
				}
				//Have the user decide what month to apply the recurring charge towards.
				FormCreditRecurringDateChoose formDateChoose=new FormCreditRecurringDateChoose(_listCreditCards[comboCreditCards.SelectedIndex],_patCur);
				formDateChoose.ShowDialog();
				if(formDateChoose.DialogResult!=DialogResult.OK) {
					MsgBox.Show(this,"Uncheck the \"Apply to Recurring Charge\" box.");
					return false;
				}
				//This will change the PayDate to work better with the recurring charge automation.  User was notified in previous window.
				if(!PrefC.GetBool(PrefName.RecurringChargesUseTransDate)) {
					_paymentCur.PayDate=formDateChoose.PayDate;
				}
				_paymentCur.RecurringChargeDate=formDateChoose.PayDate;
			}
			else if(IsNew && checkRecurring.Checked && comboCreditCards.SelectedIndex==_listCreditCards.Count) {
				MsgBox.Show(this,"Cannot apply a recurring charge to a new card.");
				return false;
			}
			#endregion
			_paymentCur.CheckNum=textCheckNum.Text;
			_paymentCur.BankBranch=textBankBranch.Text;
			_paymentCur.PayNote=textNote.Text;
			_paymentCur.IsRecurringCC=checkRecurring.Checked;
			if((PIn.Double(textAmount.Text)==0 && listPayType.SelectedIndex==-1) || checkPayTypeNone.Checked) {
				_paymentCur.PayType=0;
			}
			else {
				_paymentCur.PayType=_listPaymentTypeDefs[listPayType.SelectedIndex].DefNum;
			}
			if(_listSplitsCur.Count==0) {//Existing payment with no splits.
				if(!_isCCDeclined && PrefC.GetInt(PrefName.RigorousAccounting) < (int)RigorousAccounting.DontEnforce) {
					_listSplitsCur.AddRange(AutoSplitForPayment(_paymentCur.PayDate,_loadData));
					_paymentCur.PayAmt=PIn.Double(textAmount.Text);//AutoSplitForPayment reduces PayAmt - Set it back to what it should be.
				}
				else if(!_isCCDeclined
					&& Payments.AllocationRequired(_paymentCur.PayAmt,_paymentCur.PatNum)
					&& _curFamOrSuperFam.ListPats.Length>1 //Has other family members
					&& MsgBox.Show(this,MsgBoxButtons.YesNo,"Apply part of payment to other family members?"))
				{
					_listSplitsCur=Payments.Allocate(_paymentCur);//PayAmt needs to be set first
				}
				else {//Either no allocation required, or user does not want to allocate.  Just add one split.
					if(checkPayTypeNone.Checked) {//No splits created and it's an income transfer.  Delete payment? (it's not a useful payment)
						Payments.Delete(_paymentCur);
						return true;
					}
					else {
						if(!AddOneSplit(true)) {
							return false;
						}
					}
				}
				if(_listSplitsCur.Count==0) {//There's still no split.
					if(!AddOneSplit(true)) {
						return false;
					}
				}
			}
			else {//A new or existing payment with splits.
				if(_listSplitsCur.Count==1//if one split
					&& _listSplitsCur[0].PayPlanNum!=0//and split is on a payment plan
					&& PIn.Double(textAmount.Text) != _listSplitsCur[0].SplitAmt)//and amount doesn't match payment
				{
					_listSplitsCur[0].SplitAmt=PIn.Double(textAmount.Text);//make amounts match automatically
					textSplitTotal.Text=textAmount.Text;
				}
				if(_paymentCur.PayAmt!=PIn.Double(textSplitTotal.Text)) {
					MsgBox.Show(this,"Split totals must equal payment amount.");
					//work on reallocation schemes here later
					return false;
				}
			}
			if(_listSplitsCur.Count>1) {
				_paymentCur.IsSplit=true;
			}
			else {
				_paymentCur.IsSplit=false;
			}
			try {
				Payments.Update(_paymentCur,true);
			}
			catch(ApplicationException ex) {//this catches bad dates.
				MessageBox.Show(ex.Message);
				return false;
			}
			//Set all DatePays the same.
			for(int i=0;i<_listSplitsCur.Count;i++) {
				_listSplitsCur[i].DatePay=_paymentCur.PayDate;
			}
			if(IsNew && !_isCCDeclined) {
				//Currently we do not want to modify historical data or credit transaction values. Moving forward zero dollar splits are not valid.
				_listSplitsCur.RemoveAll(x => x.SplitAmt.IsZero());
			}
			bool hasChanged=PaySplits.Sync(_listSplitsCur,_listPaySplitsOld);
			foreach(PaySplits.PaySplitAssociated split in _listPaySplitsAssociated) {
				//Update the FSplitNum after inserts are made. 
				if(split.PaySplitLinked!=null && split.PaySplitOrig!=null) {
					PaySplits.UpdateFSplitNum(split.PaySplitOrig.SplitNum,split.PaySplitLinked.SplitNum);
				}
			}
			//Accounting synch is done here.  All validation was done further up
			//If user is trying to change the amount or linked account of an entry that was already copied and linked to accounting section
			if(accountingSynchRequired && !checkPayTypeNone.Checked) {
				Payments.AlterLinkedEntries(accountingOldAmt,_paymentCur.PayAmt,IsNew,_paymentCur.PayNum,accountingNewAcct,_paymentCur.PayDate,
					_curFamOrSuperFam.GetNameInFamFL(_paymentCur.PatNum));
			}
			if(IsNew) {
				SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,_paymentCur.PatNum,Payments.GetSecuritylogEntryText(_paymentCur,_paymentOld,IsNew,_listPaymentTypeDefs));
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,_paymentCur.PatNum,Payments.GetSecuritylogEntryText(_paymentCur,_paymentOld,IsNew,_listPaymentTypeDefs),
					0,_paymentOld.SecDateTEdit);
			}
			if(hasChanged) {
				string strErrorMsg=Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum,_listSplitsCur.Union(_listPaySplitsOld).ToList());
				if(!string.IsNullOrEmpty(strErrorMsg)) {
					MessageBox.Show(strErrorMsg);
				}
			}
			return true;
		}

		private void butShowHide_Click(object sender,EventArgs e) {
			ToggleShowHideSplits();
		}

		private string SecurityLogEntryHelper(string oldVal,string newVal,string textInLog) {
			if(oldVal!=newVal) {
				return "\r\n"+textInLog+" changed back to '"+oldVal+"' from '"+newVal+"'";
			}
			return "";
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(!SavePaymentToDb()) {
				return;
			}
			DialogResult=DialogResult.OK;
			Plugins.HookAddCode(this,"FormPayment.butOK_Click_end",_paymentCur,_listSplitsCur);
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
		
		private void FormPayment_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult==DialogResult.OK) {
				return;
			}
			//make additional logging so user knows changes they had just made to any paysplits were rolled back.
			//individual audit trails for splits
			foreach(PaySplit split in _listSplitsCur) {
				PaySplit oldSplit=_listPaySplitsOld.FirstOrDefault(x => x.SplitNum==split.SplitNum);
				string secLogText="Payment changes canceled:";
				string changesMade="";
				if(oldSplit==null) {//null when splits are new
					secLogText="New paysplit canceled.";
					SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,split.PatNum,secLogText,0);
					continue;
				}
				changesMade+=SecurityLogEntryHelper(Providers.GetAbbr(oldSplit.ProvNum),Providers.GetAbbr(split.ProvNum),"Provider");
				changesMade+=SecurityLogEntryHelper(Clinics.GetAbbr(oldSplit.ClinicNum),Clinics.GetAbbr(split.ClinicNum),"Clinic");
				changesMade+=SecurityLogEntryHelper(oldSplit.SplitAmt.ToString("F"),split.SplitAmt.ToString("F"),"Amount");
				changesMade+=SecurityLogEntryHelper(oldSplit.PatNum.ToString(),split.PatNum.ToString(),"Patient number");
				if(changesMade!="") {
					SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,split.PatNum,secLogText+changesMade,0,oldSplit.SecDateTEdit);
				}
			}
			if(!IsNew && !_wasCreditCardSuccessful) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!_wasCreditCardSuccessful) {//new payment that was not a credit card payment that has already been processed
				try {
					Payments.Delete(_paymentCur);
				}
				catch(ApplicationException ex) {
					MsgBox.Show(ex.Message);
					e.Cancel=true;//they must either OK the payment and complete, or go back and detach the deposit that was just made. 
					return;
				}
				DialogResult=DialogResult.Cancel;
				return;
			}
			//Successful CC payment
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"This will void the transaction that has just been completed. Are you sure you want to continue?")) {
				e.Cancel=true;//Stop the form from closing
				return;
			}
			DateTime payDateCur=PIn.Date(textDate.Text);
			if(payDateCur==null || payDateCur==DateTime.MinValue) {
				MsgBox.Show(this,"Invalid Payment Date");
				e.Cancel=true;//Stop the form from closing
				return;
			}
			if(payDateCur.Date > DateTime.Today.Date && !PrefC.GetBool(PrefName.AccountAllowFutureDebits) && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
				MsgBox.Show(this,"Payment Date must not be a future date.");
				e.Cancel=true;//Stop the form from closing
				return;
			}
			//Save the credit card transaction as a new payment
			_paymentCur.PayAmt=PIn.Double(textAmount.Text);//handles blank
			_paymentCur.PayDate=payDateCur;
			_paymentCur.CheckNum=textCheckNum.Text;
			_paymentCur.BankBranch=textBankBranch.Text;
			_paymentCur.IsRecurringCC=false;
			_paymentCur.PayNote=textNote.Text;
			if(checkPayTypeNone.Checked) {
				_paymentCur.PayType=0;
			}
			else {
				_paymentCur.PayType=_listPaymentTypeDefs[listPayType.SelectedIndex].DefNum;
			}
			if(_listSplitsCur.Count==0) {
				AddOneSplit();
				//FillMain();
			}
			else if(_listSplitsCur.Count==1//if one split
				&& _listSplitsCur[0].PayPlanNum!=0//and split is on a payment plan
				&& _listSplitsCur[0].SplitAmt!=_paymentCur.PayAmt)//and amount doesn't match payment
			{
				_listSplitsCur[0].SplitAmt=_paymentCur.PayAmt;//make amounts match automatically
				textSplitTotal.Text=textAmount.Text;
			}
			if(_paymentCur.PayAmt!=PIn.Double(textSplitTotal.Text)) {
				MsgBox.Show(this,"Split totals must equal payment amount.");
				DialogResult=DialogResult.None;
				return;
			}
			if(_listSplitsCur.Count>1) {
				_paymentCur.IsSplit=true;
			}
			else {
				_paymentCur.IsSplit=false;
			}
			try {
				Payments.Update(_paymentCur,true);
			}
			catch(ApplicationException ex) {//this catches bad dates.
				MessageBox.Show(ex.Message);
				e.Cancel=true;
				return;
			}
			//Set all DatePays the same.
			for(int i=0;i<_listSplitsCur.Count;i++) {
				_listSplitsCur[i].DatePay=_paymentCur.PayDate;
			}
			bool hasChanged=PaySplits.Sync(_listSplitsCur,_listPaySplitsOld);
			if(IsNew) {
				SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,_paymentCur.PatNum,Payments.GetSecuritylogEntryText(_paymentCur,_paymentOld,IsNew,
					_listPaymentTypeDefs));
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,_paymentCur.PatNum,Payments.GetSecuritylogEntryText(_paymentCur,_paymentOld,IsNew,
					_listPaymentTypeDefs),0,_paymentOld.SecDateTEdit);
			}
			if(hasChanged) {
				string strErrorMsg=Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum,_listSplitsCur.Union(_listPaySplitsOld).ToList());
				if(!string.IsNullOrEmpty(strErrorMsg)) {
					MessageBox.Show(strErrorMsg);
				}
			}
			string refNum="";
			string amount="";
			string transactionID="";
			string paySimplePaymentId="";
			bool isDebit=false;
			string[] arrayNoteFields=textNote.Text.Replace("\r\n","\n").Replace("\r","\n").Split(new string[] { "\n" },StringSplitOptions.RemoveEmptyEntries);
			for(int i=0;i<arrayNoteFields.Length;i++) {
				if(arrayNoteFields[i].StartsWith("Amount: ")) {
					amount=arrayNoteFields[i].Substring(8);
				}
				if(arrayNoteFields[i].StartsWith("Ref Number: ")) {
					refNum=arrayNoteFields[i].Substring(12);
				}
				if(arrayNoteFields[i].StartsWith("XCTRANSACTIONID=")) {
					transactionID=arrayNoteFields[i].Substring(16);
				}
				if(arrayNoteFields[i].StartsWith("APPROVEDAMOUNT=")) {
					amount=arrayNoteFields[i].Substring(15);
				}
				if(arrayNoteFields[i].StartsWith("TYPE=") && arrayNoteFields[i].Substring(5)=="Debit Purchase") {
					isDebit=true;
				}
				if(arrayNoteFields[i].StartsWith(Lan.g("PaySimple","PaySimple Transaction Number"))) {
					paySimplePaymentId=arrayNoteFields[i].Split(':')[1].Trim();//Better than substring 28, because we do not know how long the translation will be.
				}
			}
			if(refNum!="") {//Void the PayConnect transaction if there is one
				VoidPayConnectTransaction(refNum,amount);
			}
			else if(transactionID!="" && HasXCharge()) {//Void the X-Charge transaction if there is one
				VoidXChargeTransaction(transactionID,amount,isDebit);
			}
			else if(!string.IsNullOrWhiteSpace(paySimplePaymentId)) {
				string originalReceipt=_paymentCur.Receipt;
				if(_paySimpleResponse!=null) {
					originalReceipt=_paySimpleResponse.TransactionReceipt;
				}
				VoidPaySimpleTransaction(paySimplePaymentId,originalReceipt);
			}
			else {
				MsgBox.Show(this,"Unable to void transaction");
			}
			DialogResult=DialogResult.Cancel;
		}
	}
}
