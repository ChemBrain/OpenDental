//#define TRIALONLY //Do not set here because ContrChart.ProcButtonClicked and FormOpenDental also need to test this value.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
///<summary>All this dialog does is set the patnum and it is up to the calling form to do an immediate refresh, or possibly just change the patnum back to what it was.  So the other patient fields must remain intact during all logic in this form, especially if SelectionModeOnly.</summary>
	public class FormPatientSelect : ODForm{
		private System.Windows.Forms.Label label1;
		private IContainer components=null;
		private Patients Patients;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butAddPt;
		/// <summary>Use when you want to specify a patient without changing the current patient.  If true, then the Add Patient button will not be visible.</summary>
		public bool SelectionModeOnly;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textLName;
		private System.Windows.Forms.TextBox textFName;
		private System.Windows.Forms.TextBox textAddress;
		private ValidPhone textPhone;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox checkHideInactive;
		private System.Windows.Forms.GroupBox groupAddPt;
		private System.Windows.Forms.CheckBox checkGuarantors;
		private System.Windows.Forms.TextBox textCity;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textState;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textPatNum;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox textChartNumber;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox textSSN;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.GroupBox groupBox1;
		private OpenDental.UI.Button butSearch;
		///<summary>When closing the form, this indicates whether a new patient was added from within this form.</summary>
		public bool NewPatientAdded;
		///<summary>Only used when double clicking blank area in Appts. Sets this value to the currently selected pt.  That patient will come up on the screen already selected and user just has to click OK. Or they can select a different pt or add a new pt.  If 0, then no initial patient is selected.</summary>
		public long InitialPatNum;
		private DataTable _DataTablePats;
		private OpenDental.UI.ODGrid gridMain;
		///<summary>When closing the form, this will hold the value of the newly selected PatNum.</summary>
		public long SelectedPatNum;
		private CheckBox checkShowArchived;
		private TextBox textBirthdate;
		private Label label2;
		private ComboBox comboBillingType;
		private OpenDental.UI.Button butGetAll;
		private CheckBox checkRefresh;
		private OpenDental.UI.Button butAddAll;
		private ComboBox comboSite;
		private Label labelSite;
		private TextBox selectedTxtBox;
		private TextBox textSubscriberID;
		private Label label13;
		private TextBox textEmail;
		private Label labelEmail;
		private TextBox textCountry;
		private Label labelCountry;
		private ComboBox comboClinic;
		private Label labelClinic;
		private List<DisplayField> _ListDisplayFields;
		private TextBox textRegKey;
		private Label labelRegKey;
		///<summary>List of all the clinics this userod has access to.  When comboClinic.SelectedIndex=0 it refers to all clinics in this list.  Otherwise their selected clinic will always be _listClinics[comboClinic.SelectedIndex-1].</summary>
		private List<Clinic> _listClinics;
		///<summary>Set to true if constructor passed in patient object to prefill text boxes.  Used to make sure fillGrid is not called 
		///before FormSelectPatient_Load.</summary>
		private bool _isPreFillLoad=false;
		///<summary>If set, initial patient list will be set to these patients.</summary>
		public List<long> ExplicitPatNums;
		private ODThread _fillGridThread=null;
		private DateTime _dateTimeLastSearch;
		private DateTime _dateTimeLastRequest;
		private CheckBox checkShowMerged;
		private TextBox textInvoiceNumber;
		private Label labelInvoiceNumber;
		private UI.Button butOnScreenKeyboard;
		private Process _processOnScreenKeyboard=null;
		private PtTableSearchParams _ptTableSearchParams;
		private List<Site> _listSites;
		private List<Def> _listBillingTypeDefs;
		private Timer timerFillGrid;
		///<summary>Local cache of the pref PatientSelectSearchMinChars, since this will be used in every textbox.TextChanged event and we don't want to
		///parse the pref and convert to an int with every character entered.</summary>
		private int _patSearchMinChars=1;
		///<summary>Used to adjust gridpat contextmenu for right click, and unmask text</summary>
		private Point _lastClickedPoint;

		#region On Screen Keyboard Dll Imports
		[System.Runtime.InteropServices.DllImport("kernel32.dll",SetLastError = true)]
		static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

		[System.Runtime.InteropServices.DllImport("kernel32.dll",SetLastError = true)]
		static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);
		#endregion

		///<summary></summary>
		public FormPatientSelect():this(null) {
		}

		///<summary>This takes a partially built patient object and uses it to prefill textboxes to assist in searching.  
		///Currently only implements FName,LName.</summary>
		public FormPatientSelect(Patient pat){
			InitializeComponent();//required first
			Patients=new Patients();
			Lan.F(this);
			if(pat!=null) {
				PreFillSearchBoxes(pat);
			}
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if (components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientSelect));
			this.textLName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupAddPt = new System.Windows.Forms.GroupBox();
			this.butAddAll = new OpenDental.UI.Button();
			this.butAddPt = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butOnScreenKeyboard = new OpenDental.UI.Button();
			this.textInvoiceNumber = new System.Windows.Forms.TextBox();
			this.labelInvoiceNumber = new System.Windows.Forms.Label();
			this.checkShowMerged = new System.Windows.Forms.CheckBox();
			this.textRegKey = new System.Windows.Forms.TextBox();
			this.labelRegKey = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.textCountry = new System.Windows.Forms.TextBox();
			this.labelCountry = new System.Windows.Forms.Label();
			this.textEmail = new System.Windows.Forms.TextBox();
			this.labelEmail = new System.Windows.Forms.Label();
			this.textSubscriberID = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.comboSite = new System.Windows.Forms.ComboBox();
			this.labelSite = new System.Windows.Forms.Label();
			this.comboBillingType = new System.Windows.Forms.ComboBox();
			this.textBirthdate = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.checkShowArchived = new System.Windows.Forms.CheckBox();
			this.textChartNumber = new System.Windows.Forms.TextBox();
			this.textSSN = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.textPatNum = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.textState = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.textCity = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.checkGuarantors = new System.Windows.Forms.CheckBox();
			this.checkHideInactive = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textAddress = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textPhone = new OpenDental.ValidPhone();
			this.label4 = new System.Windows.Forms.Label();
			this.textFName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkRefresh = new System.Windows.Forms.CheckBox();
			this.butGetAll = new OpenDental.UI.Button();
			this.butSearch = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.timerFillGrid = new System.Windows.Forms.Timer(this.components);
			this.groupAddPt.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textLName
			// 
			this.textLName.Location = new System.Drawing.Point(166, 55);
			this.textLName.Name = "textLName";
			this.textLName.Size = new System.Drawing.Size(90, 20);
			this.textLName.TabIndex = 0;
			this.textLName.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textLName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(11, 58);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(154, 12);
			this.label1.TabIndex = 3;
			this.label1.Text = "Last Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupAddPt
			// 
			this.groupAddPt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupAddPt.Controls.Add(this.butAddAll);
			this.groupAddPt.Controls.Add(this.butAddPt);
			this.groupAddPt.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupAddPt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupAddPt.Location = new System.Drawing.Point(670, 579);
			this.groupAddPt.Name = "groupAddPt";
			this.groupAddPt.Size = new System.Drawing.Size(262, 51);
			this.groupAddPt.TabIndex = 2;
			this.groupAddPt.TabStop = false;
			this.groupAddPt.Text = "Add New Family:";
			// 
			// butAddAll
			// 
			this.butAddAll.Location = new System.Drawing.Point(148, 20);
			this.butAddAll.Name = "butAddAll";
			this.butAddAll.Size = new System.Drawing.Size(75, 23);
			this.butAddAll.TabIndex = 1;
			this.butAddAll.Text = "Add Many";
			this.butAddAll.Click += new System.EventHandler(this.butAddAll_Click);
			// 
			// butAddPt
			// 
			this.butAddPt.Location = new System.Drawing.Point(42, 20);
			this.butAddPt.Name = "butAddPt";
			this.butAddPt.Size = new System.Drawing.Size(75, 23);
			this.butAddPt.TabIndex = 0;
			this.butAddPt.Text = "&Add Pt";
			this.butAddPt.Click += new System.EventHandler(this.butAddPt_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(775, 690);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(76, 26);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(857, 690);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(76, 26);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.butOnScreenKeyboard);
			this.groupBox2.Controls.Add(this.textInvoiceNumber);
			this.groupBox2.Controls.Add(this.labelInvoiceNumber);
			this.groupBox2.Controls.Add(this.checkShowMerged);
			this.groupBox2.Controls.Add(this.textRegKey);
			this.groupBox2.Controls.Add(this.labelRegKey);
			this.groupBox2.Controls.Add(this.comboClinic);
			this.groupBox2.Controls.Add(this.labelClinic);
			this.groupBox2.Controls.Add(this.textCountry);
			this.groupBox2.Controls.Add(this.labelCountry);
			this.groupBox2.Controls.Add(this.textEmail);
			this.groupBox2.Controls.Add(this.labelEmail);
			this.groupBox2.Controls.Add(this.textSubscriberID);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Controls.Add(this.comboSite);
			this.groupBox2.Controls.Add(this.labelSite);
			this.groupBox2.Controls.Add(this.comboBillingType);
			this.groupBox2.Controls.Add(this.textBirthdate);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.checkShowArchived);
			this.groupBox2.Controls.Add(this.textChartNumber);
			this.groupBox2.Controls.Add(this.textSSN);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.textPatNum);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.textState);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.textCity);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.checkGuarantors);
			this.groupBox2.Controls.Add(this.checkHideInactive);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.textAddress);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.textPhone);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.textFName);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.textLName);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(670, 2);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(262, 499);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Search by:";
			// 
			// butOnScreenKeyboard
			// 
			this.butOnScreenKeyboard.Location = new System.Drawing.Point(166, 10);
			this.butOnScreenKeyboard.Name = "butOnScreenKeyboard";
			this.butOnScreenKeyboard.Size = new System.Drawing.Size(90, 23);
			this.butOnScreenKeyboard.TabIndex = 54;
			this.butOnScreenKeyboard.Text = "Keyboard";
			this.butOnScreenKeyboard.UseVisualStyleBackColor = true;
			this.butOnScreenKeyboard.Click += new System.EventHandler(this.butOnScreenKeyboard_Click);
			// 
			// textInvoiceNumber
			// 
			this.textInvoiceNumber.Location = new System.Drawing.Point(166, 295);
			this.textInvoiceNumber.Name = "textInvoiceNumber";
			this.textInvoiceNumber.Size = new System.Drawing.Size(90, 20);
			this.textInvoiceNumber.TabIndex = 12;
			this.textInvoiceNumber.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textInvoiceNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// labelInvoiceNumber
			// 
			this.labelInvoiceNumber.Location = new System.Drawing.Point(11, 296);
			this.labelInvoiceNumber.Name = "labelInvoiceNumber";
			this.labelInvoiceNumber.Size = new System.Drawing.Size(156, 17);
			this.labelInvoiceNumber.TabIndex = 53;
			this.labelInvoiceNumber.Text = "Invoice Number";
			this.labelInvoiceNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowMerged
			// 
			this.checkShowMerged.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowMerged.Location = new System.Drawing.Point(11, 477);
			this.checkShowMerged.Name = "checkShowMerged";
			this.checkShowMerged.Size = new System.Drawing.Size(236, 17);
			this.checkShowMerged.TabIndex = 21;
			this.checkShowMerged.Text = "Show Merged Patients";
			this.checkShowMerged.Visible = false;
			this.checkShowMerged.CheckedChanged += new System.EventHandler(this.OnDataEntered);
			// 
			// textRegKey
			// 
			this.textRegKey.Location = new System.Drawing.Point(166, 335);
			this.textRegKey.Name = "textRegKey";
			this.textRegKey.Size = new System.Drawing.Size(90, 20);
			this.textRegKey.TabIndex = 14;
			this.textRegKey.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textRegKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// labelRegKey
			// 
			this.labelRegKey.Location = new System.Drawing.Point(11, 336);
			this.labelRegKey.Name = "labelRegKey";
			this.labelRegKey.Size = new System.Drawing.Size(156, 17);
			this.labelRegKey.TabIndex = 50;
			this.labelRegKey.Text = "RegKey";
			this.labelRegKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(98, 398);
			this.comboClinic.MaxDropDownItems = 40;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(158, 21);
			this.comboClinic.TabIndex = 17;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.OnDataEntered);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(11, 402);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(86, 14);
			this.labelClinic.TabIndex = 47;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCountry
			// 
			this.textCountry.Location = new System.Drawing.Point(166, 315);
			this.textCountry.Name = "textCountry";
			this.textCountry.Size = new System.Drawing.Size(90, 20);
			this.textCountry.TabIndex = 13;
			this.textCountry.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textCountry.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// labelCountry
			// 
			this.labelCountry.Location = new System.Drawing.Point(11, 316);
			this.labelCountry.Name = "labelCountry";
			this.labelCountry.Size = new System.Drawing.Size(156, 17);
			this.labelCountry.TabIndex = 46;
			this.labelCountry.Text = "Country";
			this.labelCountry.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textEmail
			// 
			this.textEmail.Location = new System.Drawing.Point(166, 275);
			this.textEmail.Name = "textEmail";
			this.textEmail.Size = new System.Drawing.Size(90, 20);
			this.textEmail.TabIndex = 11;
			this.textEmail.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textEmail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// labelEmail
			// 
			this.labelEmail.Location = new System.Drawing.Point(11, 279);
			this.labelEmail.Name = "labelEmail";
			this.labelEmail.Size = new System.Drawing.Size(156, 12);
			this.labelEmail.TabIndex = 43;
			this.labelEmail.Text = "E-mail";
			this.labelEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSubscriberID
			// 
			this.textSubscriberID.Location = new System.Drawing.Point(166, 255);
			this.textSubscriberID.Name = "textSubscriberID";
			this.textSubscriberID.Size = new System.Drawing.Size(90, 20);
			this.textSubscriberID.TabIndex = 10;
			this.textSubscriberID.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textSubscriberID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(11, 259);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(156, 12);
			this.label13.TabIndex = 41;
			this.label13.Text = "Subscriber ID";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboSite
			// 
			this.comboSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSite.Location = new System.Drawing.Point(98, 377);
			this.comboSite.MaxDropDownItems = 40;
			this.comboSite.Name = "comboSite";
			this.comboSite.Size = new System.Drawing.Size(158, 21);
			this.comboSite.TabIndex = 16;
			this.comboSite.SelectionChangeCommitted += new System.EventHandler(this.OnDataEntered);
			// 
			// labelSite
			// 
			this.labelSite.Location = new System.Drawing.Point(11, 381);
			this.labelSite.Name = "labelSite";
			this.labelSite.Size = new System.Drawing.Size(86, 14);
			this.labelSite.TabIndex = 38;
			this.labelSite.Text = "Site";
			this.labelSite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboBillingType
			// 
			this.comboBillingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBillingType.FormattingEnabled = true;
			this.comboBillingType.Location = new System.Drawing.Point(98, 356);
			this.comboBillingType.Name = "comboBillingType";
			this.comboBillingType.Size = new System.Drawing.Size(158, 21);
			this.comboBillingType.TabIndex = 15;
			this.comboBillingType.SelectionChangeCommitted += new System.EventHandler(this.OnDataEntered);
			// 
			// textBirthdate
			// 
			this.textBirthdate.Location = new System.Drawing.Point(166, 235);
			this.textBirthdate.Name = "textBirthdate";
			this.textBirthdate.Size = new System.Drawing.Size(90, 20);
			this.textBirthdate.TabIndex = 9;
			this.textBirthdate.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textBirthdate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(11, 239);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(156, 12);
			this.label2.TabIndex = 27;
			this.label2.Text = "Birthdate";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowArchived
			// 
			this.checkShowArchived.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowArchived.Location = new System.Drawing.Point(11, 460);
			this.checkShowArchived.Name = "checkShowArchived";
			this.checkShowArchived.Size = new System.Drawing.Size(245, 16);
			this.checkShowArchived.TabIndex = 20;
			this.checkShowArchived.Text = "Show Archived/Deceased/Hidden Clinics";
			this.checkShowArchived.CheckedChanged += new System.EventHandler(this.checkShowArchived_CheckedChanged);
			// 
			// textChartNumber
			// 
			this.textChartNumber.Location = new System.Drawing.Point(166, 215);
			this.textChartNumber.Name = "textChartNumber";
			this.textChartNumber.Size = new System.Drawing.Size(90, 20);
			this.textChartNumber.TabIndex = 8;
			this.textChartNumber.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textChartNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// textSSN
			// 
			this.textSSN.Location = new System.Drawing.Point(166, 175);
			this.textSSN.Name = "textSSN";
			this.textSSN.Size = new System.Drawing.Size(90, 20);
			this.textSSN.TabIndex = 6;
			this.textSSN.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textSSN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(11, 179);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(155, 12);
			this.label12.TabIndex = 24;
			this.label12.Text = "SSN";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(11, 360);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(87, 14);
			this.label11.TabIndex = 21;
			this.label11.Text = "Billing Type";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(11, 219);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(156, 12);
			this.label10.TabIndex = 20;
			this.label10.Text = "Chart Number";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPatNum
			// 
			this.textPatNum.Location = new System.Drawing.Point(166, 195);
			this.textPatNum.Name = "textPatNum";
			this.textPatNum.Size = new System.Drawing.Size(90, 20);
			this.textPatNum.TabIndex = 7;
			this.textPatNum.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textPatNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(11, 199);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(156, 12);
			this.label9.TabIndex = 18;
			this.label9.Text = "Patient Number";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textState
			// 
			this.textState.Location = new System.Drawing.Point(166, 155);
			this.textState.Name = "textState";
			this.textState.Size = new System.Drawing.Size(90, 20);
			this.textState.TabIndex = 5;
			this.textState.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textState.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(11, 159);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(154, 12);
			this.label8.TabIndex = 16;
			this.label8.Text = "State";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCity
			// 
			this.textCity.Location = new System.Drawing.Point(166, 135);
			this.textCity.Name = "textCity";
			this.textCity.Size = new System.Drawing.Size(90, 20);
			this.textCity.TabIndex = 4;
			this.textCity.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textCity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(11, 137);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(152, 14);
			this.label7.TabIndex = 14;
			this.label7.Text = "City";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkGuarantors
			// 
			this.checkGuarantors.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkGuarantors.Location = new System.Drawing.Point(11, 426);
			this.checkGuarantors.Name = "checkGuarantors";
			this.checkGuarantors.Size = new System.Drawing.Size(245, 16);
			this.checkGuarantors.TabIndex = 18;
			this.checkGuarantors.Text = "Guarantors Only";
			this.checkGuarantors.CheckedChanged += new System.EventHandler(this.OnDataEntered);
			// 
			// checkHideInactive
			// 
			this.checkHideInactive.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHideInactive.Location = new System.Drawing.Point(11, 443);
			this.checkHideInactive.Name = "checkHideInactive";
			this.checkHideInactive.Size = new System.Drawing.Size(245, 16);
			this.checkHideInactive.TabIndex = 19;
			this.checkHideInactive.Text = "Hide Inactive Patients";
			this.checkHideInactive.CheckedChanged += new System.EventHandler(this.OnDataEntered);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(11, 38);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(245, 14);
			this.label6.TabIndex = 10;
			this.label6.Text = "Hint: enter values in multiple boxes.";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// textAddress
			// 
			this.textAddress.Location = new System.Drawing.Point(166, 115);
			this.textAddress.Name = "textAddress";
			this.textAddress.Size = new System.Drawing.Size(90, 20);
			this.textAddress.TabIndex = 3;
			this.textAddress.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(11, 118);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(154, 12);
			this.label5.TabIndex = 9;
			this.label5.Text = "Address";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPhone
			// 
			this.textPhone.Location = new System.Drawing.Point(166, 95);
			this.textPhone.Name = "textPhone";
			this.textPhone.Size = new System.Drawing.Size(90, 20);
			this.textPhone.TabIndex = 2;
			this.textPhone.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textPhone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(11, 97);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(155, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "Phone (any)";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textFName
			// 
			this.textFName.Location = new System.Drawing.Point(166, 75);
			this.textFName.Name = "textFName";
			this.textFName.Size = new System.Drawing.Size(90, 20);
			this.textFName.TabIndex = 1;
			this.textFName.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.textFName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(11, 79);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(154, 12);
			this.label3.TabIndex = 5;
			this.label3.Text = "First Name";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.checkRefresh);
			this.groupBox1.Controls.Add(this.butGetAll);
			this.groupBox1.Controls.Add(this.butSearch);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(670, 504);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(262, 75);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Search";
			// 
			// checkRefresh
			// 
			this.checkRefresh.Location = new System.Drawing.Point(11, 51);
			this.checkRefresh.Name = "checkRefresh";
			this.checkRefresh.Size = new System.Drawing.Size(245, 18);
			this.checkRefresh.TabIndex = 72;
			this.checkRefresh.Text = "Refresh while typing";
			this.checkRefresh.UseVisualStyleBackColor = true;
			this.checkRefresh.Click += new System.EventHandler(this.checkRefresh_Click);
			// 
			// butGetAll
			// 
			this.butGetAll.Location = new System.Drawing.Point(148, 22);
			this.butGetAll.Name = "butGetAll";
			this.butGetAll.Size = new System.Drawing.Size(75, 23);
			this.butGetAll.TabIndex = 1;
			this.butGetAll.Text = "Get All";
			this.butGetAll.Click += new System.EventHandler(this.butGetAll_Click);
			// 
			// butSearch
			// 
			this.butSearch.Location = new System.Drawing.Point(42, 22);
			this.butSearch.Name = "butSearch";
			this.butSearch.Size = new System.Drawing.Size(75, 23);
			this.butSearch.TabIndex = 0;
			this.butSearch.Text = "&Search";
			this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(3, 2);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(665, 718);
			this.gridMain.TabIndex = 9;
			this.gridMain.Title = "Select Patient";
			this.gridMain.TranslationName = "FormPatientSelect";
			this.gridMain.WrapText = false;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			this.gridMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridMain_MouseDown);
			// 
			// timerFillGrid
			// 
			this.timerFillGrid.Interval = 1;
			this.timerFillGrid.Tick += new System.EventHandler(this.OnDataEntered);
			// 
			// FormPatientSelect
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(944, 727);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.groupAddPt);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(960, 766);
			this.Name = "FormPatientSelect";
			this.ShowInTaskbar = false;
			this.Text = "Select Patient";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPatientSelect_FormClosing);
			this.Load += new System.EventHandler(this.FormSelectPatient_Load);
			this.groupAddPt.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		///<summary></summary>
		public void FormSelectPatient_Load(object sender, System.EventArgs e){
			if(!PrefC.GetBool(PrefName.DockPhonePanelShow)) {
				labelCountry.Visible=false;
				textCountry.Visible=false;
			}
			if(!PrefC.GetBool(PrefName.DistributorKey)) {
				labelRegKey.Visible=false;
				textRegKey.Visible=false;
			}
			if(SelectionModeOnly){
				groupAddPt.Visible=false;
			}
			//Cannot add new patients from OD select patient interface.  Patient must be added from HL7 message.
			if(HL7Defs.IsExistingHL7Enabled()) {
				HL7Def def=HL7Defs.GetOneDeepEnabled();
				if(def.ShowDemographics!=HL7ShowDemographics.ChangeAndAdd) {
					groupAddPt.Visible=false;
				}
			}
			else {
				if(Programs.UsingEcwTightOrFullMode()) {
					groupAddPt.Visible=false;
				}
			}
			comboBillingType.Items.Add(Lan.g(this,"All"));
			comboBillingType.SelectedIndex=0;
			_listBillingTypeDefs=Defs.GetDefsForCategory(DefCat.BillingTypes,true);
			for(int i=0;i<_listBillingTypeDefs.Count;i++){
				comboBillingType.Items.Add(_listBillingTypeDefs[i].ItemName);
			}
			if(PrefC.GetBool(PrefName.EasyHidePublicHealth)){
				comboSite.Visible=false;
				labelSite.Visible=false;
			}
			else{
				comboSite.Items.Add(Lan.g(this,"All"));
				comboSite.SelectedIndex=0;
				_listSites=Sites.GetDeepCopy();
				for(int i=0;i<_listSites.Count;i++) {
					comboSite.Items.Add(_listSites[i].Description);
				}
			}
			if(!PrefC.HasClinicsEnabled) {
				labelClinic.Visible=false;
				comboClinic.Visible=false;
			}
			else {
				//if the current user is restricted to a clinic (or in the future many clinics), All will refer to only those clinics the user has access to. May only be one clinic.
				comboClinic.Items.Add(new ODBoxItem<Clinic>(Lan.g(this,"All"),new Clinic()));
				comboClinic.SelectedIndex=0;
				if(Security.IsAuthorized(Permissions.UnrestrictedSearch,true)) {//user has permission to search all clinics though restricted. 
					_listClinics=Clinics.GetDeepCopy();
				}
				else {//only authorized to search restricted clinics.
					_listClinics=Clinics.GetAllForUserod(Security.CurUser);//could be only one if the user is restricted
				}
				for(int i=0;i<_listClinics.Count;i++) {
					if(_listClinics[i].IsHidden) {
						continue;//Don't add hidden clinics to the combo
					}
					comboClinic.Items.Add(new ODBoxItem<Clinic>(_listClinics[i].Abbr,_listClinics[i]));
					if(Clinics.ClinicNum==_listClinics[i].ClinicNum) {
						comboClinic.SelectedIndex=comboClinic.Items.Count-1;
					}
				}
			}
			if(PrefC.GetBool(PrefName.PatientSSNMasked)) {
				//Add "View SS#" right click option, MenuItemPopup() will show and hide it as needed.
				if(gridMain.ContextMenu==null) {
					gridMain.ContextMenu=new ContextMenu();//ODGrid will automatically attach the default Popups
				}
				ContextMenu menu = gridMain.ContextMenu;
				MenuItem menuItemUnmaskSSN=new MenuItem();
				menuItemUnmaskSSN.Enabled=false;
				menuItemUnmaskSSN.Visible=false;
				menuItemUnmaskSSN.Name="ViewSS#";
				menuItemUnmaskSSN.Text="View SS#";
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					menuItemUnmaskSSN.Text="View SIN";
				}
				menuItemUnmaskSSN.Click+= new System.EventHandler(this.MenuItemUnmaskSSN_Click);
				menu.MenuItems.Add(menuItemUnmaskSSN);
				menu.Popup+=MenuItemPopupUnmaskSSN;
			}
			if(PrefC.GetBool(PrefName.PatientDOBMasked)) {
				//Add "View DOB" right click option, MenuItemPopup() will show and hide it as needed.
				if(gridMain.ContextMenu==null) {
					gridMain.ContextMenu=new ContextMenu();//ODGrid will automatically attach the default Popups
				}
				ContextMenu menu = gridMain.ContextMenu;
				MenuItem menuItemUnmaskDOB=new MenuItem();
				menuItemUnmaskDOB.Enabled=false;
				menuItemUnmaskDOB.Visible=false;
				menuItemUnmaskDOB.Name="ViewDOB";
				menuItemUnmaskDOB.Text="View DOB";
				menuItemUnmaskDOB.Click+= new System.EventHandler(this.MenuItemUnmaskDOB_Click);
				menu.MenuItems.Add(menuItemUnmaskDOB);
				menu.Popup+=MenuItemPopupUnmaskDOB;
			}
			FillSearchOption();
			SetGridCols();
			//Using PrefC.GetString on the following two prefs so that we can call PIn.Int with hasExceptions=false and using the Math.Max and Math.Min we
			//are guaranteed to get a valid number from these prefs.
			timerFillGrid.Interval=PIn.Int(PrefC.GetString(PrefName.PatientSelectSearchPauseMs));
			_patSearchMinChars=PIn.Int(PrefC.GetString(PrefName.PatientSelectSearchMinChars));
			if(ExplicitPatNums!=null && ExplicitPatNums.Count>0) {
				FillGrid(false,ExplicitPatNums);
				return;
			}
			if(InitialPatNum!=0){
				Patient iPatient=Patients.GetLim(InitialPatNum);
				textLName.Text=iPatient.LName;
				FillGrid(false);
				return;
			}
			//Always fillGrid if _isPreFilledLoad.  Since the first name and last name are pre-filled, the results should be minimal.
			//Also FillGrid if checkRefresh is checked and either PatientSelectSearchWithEmptyParams is set or there is a character in at least one textbox
			if(_isPreFillLoad || DoRefreshGrid()) {
				FillGrid(true);
				_isPreFillLoad=false;
			}
			//Set the Textbox Enter Event Handler to keep track of which TextBox had focus last.  
			//This helps dictate the desired text box for input after opening up the On Screen Keyboard.
			SetAllTextBoxEnterEventListeners();
      if(ODBuild.IsWeb()) {
        //Keyboard does not currently work with WEB users. 
        butOnScreenKeyboard.Visible=false;
      }
		}

		///<summary>This used to be called all the time, now only needs to be called on load.</summary>
		private void FillSearchOption() {
			switch(ComputerPrefs.LocalComputer.PatSelectSearchMode) {
				case SearchMode.Default:
					checkRefresh.Checked=!PrefC.GetBool(PrefName.PatientSelectUsesSearchButton);//Use global preference
					break;
				case SearchMode.RefreshWhileTyping:
					checkRefresh.Checked=true;
					break;
				case SearchMode.UseSearchButton:
				default:
					checkRefresh.Checked=false;
					break;
			}
		}

		private void SetGridCols(){
			//This pattern is wrong.
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col;
			_ListDisplayFields=DisplayFields.GetForCategory(DisplayFieldCategory.PatientSelect);
			for(int i=0;i<_ListDisplayFields.Count;i++){
				if(_ListDisplayFields[i].Description==""){
					col=new GridColumn(_ListDisplayFields[i].InternalName,_ListDisplayFields[i].ColumnWidth);
				}
				else{
					col=new GridColumn(_ListDisplayFields[i].Description,_ListDisplayFields[i].ColumnWidth);
				}
				gridMain.ListGridColumns.Add(col);
			}
			gridMain.EndUpdate();
		}

		///<summary>The pat must not be null.  Takes a partially built patient object and uses it to fill the search by textboxes.
		///Currently only implements FName, LName, and HmPhone.</summary>
		public void PreFillSearchBoxes(Patient pat) {
			_isPreFillLoad=true; //Set to true to stop FillGrid from being called as a result of textChanged events
			if(pat.LName != "") {
				textLName.Text=pat.LName;
			}
			if(pat.FName != "") {
				textFName.Text=pat.FName;
			}
			if(pat.HmPhone != "") {
				textPhone.Text=pat.HmPhone;
			}
		}

		///<summary>Returns the count of chars in all of the textboxes on the form.  For ValidPhone textboxes only digit chars are counted.</summary>
		private int TextBoxCharCount() {
			//only count digits in ValidPhone textboxes because we auto-format with special chars, ex: (xxx)xxx-xxxx
			return this.GetAllControls()
				.OfType<TextBox>()//ValidPhone is a TextBox
				.Sum(x => x is ValidPhone?x.Text.Count(y => char.IsDigit(y)):x.TextLength);
		}

		///<summary>Returns false if either checkRefresh is not checked or PatientSelectSearchWithEmptyParams is Yes or Unknown and all of the textboxes 
		///are empty. Otherwise returns true.</summary>
		private bool DoRefreshGrid() {
			return checkRefresh.Checked && (PIn.Enum<YN>(PrefC.GetInt(PrefName.PatientSelectSearchWithEmptyParams))!=YN.No || TextBoxCharCount()>0);
		}

		///<summary>Just prior to displaying the context menu, enable or disables the UnmaskSSN option</summary>
		private void MenuItemPopupUnmaskSSN(object sender,EventArgs e) {
			MenuItem menuItemSSN=gridMain.ContextMenu.MenuItems.OfType<MenuItem>().FirstOrDefault(x => x.Name == "ViewSS#");
			if(menuItemSSN==null) { return; }//Should not happen
			MenuItem menuItemSeperator=gridMain.ContextMenu.MenuItems.OfType<MenuItem>().FirstOrDefault(x => x.Text == "-");
			if(menuItemSeperator==null) { return; }//Should not happen
			int idxGridPatSSNCol=_ListDisplayFields.FindIndex(x => x.InternalName == "SSN");
			int idxColClick = gridMain.PointToCol(_lastClickedPoint.X);
			int idxRowClick = gridMain.PointToRow(_lastClickedPoint.Y);
			if(idxRowClick > -1 && idxColClick > -1 && idxGridPatSSNCol==idxColClick) {
				if(Security.IsAuthorized(Permissions.PatientSSNView,true) && gridMain.ListGridRows[idxRowClick].Cells[idxColClick].Text!="") {
					menuItemSSN.Visible=true;
					menuItemSSN.Enabled=true;
				}
				else {
					menuItemSSN.Visible=true;
					menuItemSSN.Enabled=false;
				}
				menuItemSeperator.Visible=true;
				menuItemSeperator.Enabled=true;
			}
			else {
				menuItemSSN.Visible=false;
				menuItemSSN.Enabled=false;
				if(gridMain.ContextMenu.MenuItems.OfType<MenuItem>().Count(x => x.Visible==true && x.Text != "-") > 1) {
					//There is more than one item showing, we want the seperator.
					menuItemSeperator.Visible=true;
					menuItemSeperator.Enabled=true;
				}
				else {
					//We dont want the seperator to be there with only one option.
					menuItemSeperator.Visible=false;
					menuItemSeperator.Enabled=false;
				}
			}
		}

		private void MenuItemUnmaskSSN_Click(object sender,EventArgs e) {
			if(_fillGridThread!=null) {//still filtering results (rarely happens). 
				//Slightly annoying to be unresponsive to the click, but the grid is going to overwrite what we fill so don't bother.
				return;
			}
			//Preference and permissions check has already happened by this point.
			//Guaranteed to be clicking on a valid row & column.
			int idxColClick = gridMain.PointToCol(_lastClickedPoint.X);
			int idxRowClick = gridMain.PointToRow(_lastClickedPoint.Y);
			long patNumClicked=PIn.Long(_DataTablePats.Rows[idxRowClick]["PatNum"].ToString());
			gridMain.BeginUpdate();
			gridMain.ListGridRows[idxRowClick].Cells[idxColClick].Text=Patients.SSNFormatHelper(Patients.GetPat(patNumClicked).SSN,false);
			gridMain.EndUpdate();
			string logtext="";
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				logtext="Social Insurance Number";
			}
			else {
				logtext="Social Security Number";
			}
			logtext+=" unmasked in Patient Select";
			SecurityLogs.MakeLogEntry(Permissions.PatientSSNView,patNumClicked,logtext);
		}

		///<summary>Just prior to displaying the context menu, enable or disables the UnmaskDOB option</summary>
		private void MenuItemPopupUnmaskDOB(object sender,EventArgs e) {
			MenuItem menuItemDOB=gridMain.ContextMenu.MenuItems.OfType<MenuItem>().FirstOrDefault(x => x.Name == "ViewDOB");
			if(menuItemDOB==null) { return; }//Should not happen
			MenuItem menuItemSeperator=gridMain.ContextMenu.MenuItems.OfType<MenuItem>().FirstOrDefault(x => x.Text == "-");
			if(menuItemSeperator==null) { return; }//Should not happen
			int idxGridPatDOBCol=_ListDisplayFields.FindIndex(x => x.InternalName == "Birthdate");
			int idxColClick = gridMain.PointToCol(_lastClickedPoint.X);
			int idxRowClick = gridMain.PointToRow(_lastClickedPoint.Y);
			if(idxRowClick > -1 && idxColClick > -1 && idxGridPatDOBCol==idxColClick) {
				if(Security.IsAuthorized(Permissions.PatientDOBView,true) && gridMain.ListGridRows[idxRowClick].Cells[idxColClick].Text!="") {
					menuItemDOB.Visible=true;
					menuItemDOB.Enabled=true;
				}
				else {
					menuItemDOB.Visible=true;
					menuItemDOB.Enabled=false;
				}
				menuItemSeperator.Visible=true;
				menuItemSeperator.Enabled=true;
			}
			else {
				menuItemDOB.Visible=false;
				menuItemDOB.Enabled=false;
				if(gridMain.ContextMenu.MenuItems.OfType<MenuItem>().Count(x => x.Visible==true && x.Text != "-") > 1) {
					//There is more than one item showing, we want the seperator.
					menuItemSeperator.Visible=true;
					menuItemSeperator.Enabled=true;
				}
				else {
					//We dont want the seperator to be there with only one option.
					menuItemSeperator.Visible=false;
					menuItemSeperator.Enabled=false;
				}
			}
		}

		private void MenuItemUnmaskDOB_Click(object sender,EventArgs e) {
			if(_fillGridThread!=null) {//still filtering results (rarely happens). 
				//Slightly annoying to be unresponsive to the click, but the grid is going to overwrite what we fill so don't bother.
				return;
			}
			//Preference and permissions check has already happened by this point.
			//Guaranteed to be clicking on a valid row & column.
			int idxColClick = gridMain.PointToCol(_lastClickedPoint.X);
			int idxRowClick = gridMain.PointToRow(_lastClickedPoint.Y);
			long patNumClicked=PIn.Long(_DataTablePats.Rows[idxRowClick]["PatNum"].ToString());
			DateTime birthdate=PIn.Date(_DataTablePats.Rows[idxRowClick]["Birthdate"].ToString());
			gridMain.BeginUpdate();
			gridMain.ListGridRows[idxRowClick].Cells[idxColClick].Text=Patients.DOBFormatHelper(birthdate,false);
			gridMain.EndUpdate();
			string logtext="Date of birth unmasked in Patient Select";
			SecurityLogs.MakeLogEntry(Permissions.PatientDOBView,patNumClicked,logtext);
		}

		private void textbox_TextChanged(object sender,EventArgs e) {
			timerFillGrid.Stop();
			if(TextBoxCharCount()<_patSearchMinChars) {
				timerFillGrid.Start();//count of characters entered into all textboxes is < _patSearchMinChars, restart the timer
				return;
			}
			OnDataEntered();//count of characters entered into all textboxes is >= _patSearchMinChars, fill the grid
		}

		private void textbox_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.Up || e.KeyCode==Keys.Down) {
				gridMain.Invalidate();
				e.Handled=true;
			}
		}

		private void checkRefresh_Click(object sender,EventArgs e) {
			timerFillGrid.Stop();
			if(checkRefresh.Checked) {
				ComputerPrefs.LocalComputer.PatSelectSearchMode=SearchMode.RefreshWhileTyping;
				if(DoRefreshGrid()) {//only fill grid if PatientSelectSearchWithEmptyParams is true or there is something in at least one textbox
					FillGrid(true);
				}
			}
			else{
				ComputerPrefs.LocalComputer.PatSelectSearchMode=SearchMode.UseSearchButton;
			}
			ComputerPrefs.Update(ComputerPrefs.LocalComputer);
		}

		private void checkShowArchived_CheckedChanged(object sender,EventArgs e) {
			//We are only going to give the option to hide merged patients when Show Archived is checked.
			checkShowMerged.Visible=checkShowArchived.Checked;
			OnDataEntered();
		}

		private void butSearch_Click(object sender, System.EventArgs e) {
			timerFillGrid.Stop();
			_ptTableSearchParams=null;//this will force a grid refresh
			FillGrid(true);
		}

		private void butGetAll_Click(object sender,EventArgs e) {
			timerFillGrid.Stop();
			_ptTableSearchParams=null;//this will force a grid refresh
			FillGrid(false);
		}

		private void OnDataEntered(object sender=null,EventArgs e=null) {
			timerFillGrid.Stop();//stop the timer, otherwise the timer tick will just fire this again
			//Do not call FillGrid unless _isPreFillLoad=false.  Since the first name and last name are pre-filled, the results should be minimal.
			//DoRefreshGrid will return true if checkRefresh is checked and either PatientSelectSearchWithEmptyParams is true (or unset) or there is some
			//text in at least one of the textboxes
			if(!_isPreFillLoad && DoRefreshGrid()) {
				FillGrid(true);
			}
		}

		private void FillGrid(bool doLimitOnePage,List<long> listtExplicitPatNums=null) {
			timerFillGrid.Stop();//stop the timer, we're filling the grid now
			_dateTimeLastRequest=DateTime.Now;
			if(_fillGridThread!=null) {
				return;
			}
			_dateTimeLastSearch=_dateTimeLastRequest;
			long billingType=0;
			if(comboBillingType.SelectedIndex!=0) {
				billingType=_listBillingTypeDefs[comboBillingType.SelectedIndex-1].DefNum;
			}
			long siteNum=0;
			if(!PrefC.GetBool(PrefName.EasyHidePublicHealth) && comboSite.SelectedIndex!=0) {
				siteNum=_listSites[comboSite.SelectedIndex-1].SiteNum;
			}
			DateTime birthdate=PIn.Date(textBirthdate.Text); //this will frequently be minval.
			string clinicNums="";
			if(PrefC.HasClinicsEnabled) {
				if(comboClinic.SelectedIndex==0) {//'All' is selected
					//When below preference is false, don't hide user restricted clinics from view. Just return clinicNums as an empty string.
					//If this preference is true, we DO hide user restricted clinics from view.
					if(PrefC.GetBool(PrefName.PatientSelectFilterRestrictedClinics) && (Security.CurUser.ClinicIsRestricted || !checkShowArchived.Checked)) {
						//only set clinicNums if user is unrestricted and showing hidden clinics, otherwise the search will show patients from all clinics
						clinicNums=string.Join(",",_listClinics
							.Where(x => !x.IsHidden || checkShowArchived.Checked)//Only show hidden clinics if "Show Archived" is checked
							.Select(x => x.ClinicNum));
					}
				}
				else {
					clinicNums=((ODBoxItem<Clinic>)comboClinic.SelectedItem).Tag.ClinicNum.ToString();
					if(checkShowArchived.Checked) {
						foreach(Clinic clinic in _listClinics) {
							if(clinic.IsHidden) {
								clinicNums+=","+clinic.ClinicNum.ToString();
							}
						}
					}
				}
			}
			bool hasSpecialty=_ListDisplayFields.Any(x => x.InternalName=="Specialty");
			bool hasNextLastVisit=_ListDisplayFields.Any(x => x.InternalName.In("NextVisit","LastVisit"));
			DataTable dataTablePats=new DataTable();
			//Because hiding merged patients makes the query take longer, we will default to showing merged patients if the user has not had the 
			//opportunity to set this check box.
			bool doShowMerged=true;
			if(checkShowMerged.Visible) {
				//Only allow hiding merged if the Show Archived box is checked (and Show Merged is therefore visible).
				doShowMerged=checkShowMerged.Checked;
			}
			PtTableSearchParams ptTableSearchParamsCur=new PtTableSearchParams(doLimitOnePage,textLName.Text,textFName.Text,textPhone.Text,textAddress.Text,
				checkHideInactive.Checked,textCity.Text,textState.Text,textSSN.Text,textPatNum.Text,textChartNumber.Text,billingType,checkGuarantors.Checked,
				checkShowArchived.Checked,birthdate,siteNum,textSubscriberID.Text,textEmail.Text,textCountry.Text,textRegKey.Text,clinicNums,"",
				textInvoiceNumber.Text,listtExplicitPatNums,InitialPatNum,doShowMerged,hasSpecialty,hasNextLastVisit);
			if(_ptTableSearchParams!=null && _ptTableSearchParams.Equals(ptTableSearchParamsCur)) {//fill grid search params haven't changed, just return
				return;
			}
			_ptTableSearchParams=ptTableSearchParamsCur.Copy();
			_fillGridThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				dataTablePats=Patients.GetPtDataTable(ptTableSearchParamsCur);
			}));
			_fillGridThread.AddExitHandler(new ODThread.WorkerDelegate((ODThread o) => {
				_fillGridThread=null;
				try {
					this.BeginInvoke((Action)(() => {
						_DataTablePats=dataTablePats;
						FillGridFinal(doLimitOnePage);
					}));
				}catch(Exception) { } //do nothing. Usually just a race condition trying to invoke from a disposed form.
			}));
			_fillGridThread.AddExceptionHandler(new ODThread.ExceptionDelegate((e) => {
				try {
					this.BeginInvoke((Action)(() => {
						MessageBox.Show(e.Message);
					}));
				}catch(Exception) { } //do nothing. Usually just a race condition trying to invoke from a disposed form.
			}));
			_fillGridThread.Start(true);
		}

		private void FillGridFinal(bool doLimitOnePage){
			if(InitialPatNum!=0 && doLimitOnePage) {
				//The InitialPatNum will be at the top, so resort the list alphabetically
				DataView dataView=_DataTablePats.DefaultView;
				dataView.Sort="LName,FName";
				_DataTablePats=dataView.ToTable();
			}
			gridMain.BeginUpdate();
			gridMain.ListGridRows.Clear();
			GridRow row;
			for(int i=0;i<_DataTablePats.Rows.Count;i++) {
				row=new GridRow();
				for(int f=0;f<_ListDisplayFields.Count;f++) {
					switch(_ListDisplayFields[f].InternalName) {
						case "LastName":
							row.Cells.Add(_DataTablePats.Rows[i]["LName"].ToString());
							break;
						case "First Name":
							row.Cells.Add(_DataTablePats.Rows[i]["FName"].ToString());
							break;
						case "MI":
							row.Cells.Add(_DataTablePats.Rows[i]["MiddleI"].ToString());
							break;
						case "Pref Name":
							row.Cells.Add(_DataTablePats.Rows[i]["Preferred"].ToString());
							break;
						case "Age":
							row.Cells.Add(_DataTablePats.Rows[i]["age"].ToString());
							break;
						case "SSN":
							row.Cells.Add(Patients.SSNFormatHelper(_DataTablePats.Rows[i]["SSN"].ToString(),PrefC.GetBool(PrefName.PatientSSNMasked)));
							break;
						case "Hm Phone":
							row.Cells.Add(_DataTablePats.Rows[i]["HmPhone"].ToString());
							if(Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
								row.Cells[row.Cells.Count-1].ColorText=Color.Blue;
								row.Cells[row.Cells.Count-1].Underline=YN.Yes;
							}
							break;
						case "Wk Phone":
							row.Cells.Add(_DataTablePats.Rows[i]["WkPhone"].ToString());
							if(Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
								row.Cells[row.Cells.Count-1].ColorText=Color.Blue;
								row.Cells[row.Cells.Count-1].Underline=YN.Yes;
							}
							break;
						case "PatNum":
							row.Cells.Add(_DataTablePats.Rows[i]["PatNum"].ToString());
							break;
						case "ChartNum":
							row.Cells.Add(_DataTablePats.Rows[i]["ChartNumber"].ToString());
							break;
						case "Address":
							row.Cells.Add(_DataTablePats.Rows[i]["Address"].ToString());
							break;
						case "Status":
							row.Cells.Add(_DataTablePats.Rows[i]["PatStatus"].ToString());
							break;
						case "Bill Type":
							row.Cells.Add(_DataTablePats.Rows[i]["BillingType"].ToString());
							break;
						case "City":
							row.Cells.Add(_DataTablePats.Rows[i]["City"].ToString());
							break;
						case "State":
							row.Cells.Add(_DataTablePats.Rows[i]["State"].ToString());
							break;
						case "Pri Prov":
							row.Cells.Add(_DataTablePats.Rows[i]["PriProv"].ToString());
							break;
						case "Clinic":
							row.Cells.Add(_DataTablePats.Rows[i]["clinic"].ToString());
							break;
						case "Birthdate":
							row.Cells.Add(Patients.DOBFormatHelper(PIn.Date(_DataTablePats.Rows[i]["Birthdate"].ToString()),PrefC.GetBool(PrefName.PatientDOBMasked)));
							break;
						case "Site":
							row.Cells.Add(_DataTablePats.Rows[i]["site"].ToString());
							break;
						case "Email":
							row.Cells.Add(_DataTablePats.Rows[i]["Email"].ToString());
							break;
						case "Country":
							row.Cells.Add(_DataTablePats.Rows[i]["Country"].ToString());
							break;
						case "RegKey":
							row.Cells.Add(_DataTablePats.Rows[i]["RegKey"].ToString());
							break;
						case "OtherPhone": //will only be available if OD HQ
							row.Cells.Add(_DataTablePats.Rows[i]["OtherPhone"].ToString());
							break;
						case "Wireless Ph":
							row.Cells.Add(_DataTablePats.Rows[i]["WirelessPhone"].ToString());
							if(Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
								row.Cells[row.Cells.Count-1].ColorText=Color.Blue;
								row.Cells[row.Cells.Count-1].Underline=YN.Yes;
							}
							break;
						case "Sec Prov":
							row.Cells.Add(_DataTablePats.Rows[i]["SecProv"].ToString());
							break;
						case "LastVisit":
							row.Cells.Add(_DataTablePats.Rows[i]["lastVisit"].ToString());
							break;
						case "NextVisit":
							row.Cells.Add(_DataTablePats.Rows[i]["nextVisit"].ToString());
							break;
						case "Invoice Number":
							row.Cells.Add(_DataTablePats.Rows[i]["StatementNum"].ToString());
							break;
						case "Specialty":
							row.Cells.Add(_DataTablePats.Rows[i]["Specialty"].ToString());
							break;
					}
				}
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
			if(_dateTimeLastSearch!=_dateTimeLastRequest) {
				FillGrid(doLimitOnePage);//in case data was entered while thread was running.
			}
			gridMain.SetSelected(0,true);
			for(int i=0;i<_DataTablePats.Rows.Count;i++) {
				if(PIn.Long(_DataTablePats.Rows[i][0].ToString())==InitialPatNum) {
					gridMain.SetSelected(i,true);
					break;
				}
			}
		}

		private void gridMain_MouseDown(object sender,MouseEventArgs e) {
			_lastClickedPoint=e.Location;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PatSelected();
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			GridCell gridCellCur=gridMain.ListGridRows[e.Row].Cells[e.Col];
			//Only grid cells with phone numbers are blue and underlined.
			if(gridCellCur.ColorText==Color.Blue && gridCellCur.Underline==YN.Yes && Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
				DentalTek.PlaceCall(gridCellCur.Text);
			}
		}

		///<summary>Remember, this button is not even visible if SelectionModeOnly.</summary>
		private void butAddPt_Click(object sender, System.EventArgs e){
			if(ODBuild.IsTrial()) { 
				MsgBox.Show(this,"Trial version.  Maximum 30 patients");
				if(Patients.GetNumberPatients()>30){
					MsgBox.Show(this,"Maximum reached");
					return;
				}
			}
			if(textLName.Text=="" && textFName.Text=="" && textChartNumber.Text==""){
				MessageBox.Show(Lan.g(this,"Not allowed to add a new patient until you have done a search to see if that patient already exists. "
					+"Hint: just type a few letters into the Last Name box above.")); 
				return;
			}
			long priProv=0;
			if(!PrefC.GetBool(PrefName.PriProvDefaultToSelectProv)) {
				//Explicitly use the combo clinic instead of FormOpenDental.ClinicNum because the combo box should default to that clinic unless manually changed by the user.
				if(PrefC.HasClinicsEnabled && comboClinic.SelectedIndex!=0) {//clinics enabled and all isn't selected
					//Set the patients primary provider to the clinic default provider.
					Provider prov=Providers.GetDefaultProvider(((ODBoxItem<Clinic>)comboClinic.SelectedItem).Tag.ClinicNum);
					if(prov!=null) {
						priProv=prov.ProvNum;
					}
				}
				else {
					//Set the patients primary provider to the practice default provider.
					Provider prov=Providers.GetDefaultProvider();
					if(prov!=null) {
						priProv=prov.ProvNum;
					}
				}
			}
			Patient PatCur=Patients.CreateNewPatient(textLName.Text,textFName.Text,PIn.Date(textBirthdate.Text),priProv,Clinics.ClinicNum
				,Lan.g(this,"Created from Select Patient window."));
			Family FamCur=Patients.GetFamily(PatCur.PatNum);
			if(Plugins.HookMethod(this,"FormPatientSelect.butAddPt_Click_showForm",PatCur,FamCur)) {
				return;
			}
			FormPatientEdit FormPE=new FormPatientEdit(PatCur,FamCur);
			FormPE.IsNew=true;
			FormPE.ShowDialog();
			if(FormPE.DialogResult==DialogResult.OK){
				NewPatientAdded=true;
				SelectedPatNum=PatCur.PatNum;
				DialogResult=DialogResult.OK;
			}
		}

		private void butAddAll_Click(object sender,EventArgs e) {
			if(ODBuild.IsTrial()) { 
				MsgBox.Show(this,"Trial version.  Maximum 30 patients");
				if(Patients.GetNumberPatients()>30){
					MsgBox.Show(this,"Maximum reached");
					return;
				}
			}
			if(textLName.Text=="" && textFName.Text=="" && textChartNumber.Text==""){
				MessageBox.Show(Lan.g(this,"Not allowed to add a new patient until you have done a search to see if that patient already exists. Hint: just type a few letters into the Last Name box above.")); 
				return;
			}
			FormPatientAddAll FormP=new FormPatientAddAll();
			if(textLName.Text.Length>1){//eg Sp
				FormP.LName=textLName.Text.Substring(0,1).ToUpper()+textLName.Text.Substring(1);
			}
			if(textFName.Text.Length>1){
				FormP.FName=textFName.Text.Substring(0,1).ToUpper()+textFName.Text.Substring(1);
			}
			if(textBirthdate.Text.Length>1) {
				FormP.Birthdate=PIn.Date(textBirthdate.Text);
			}
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK){
				return;
			}
			NewPatientAdded=true;
			SelectedPatNum=FormP.SelectedPatNum;
			DialogResult=DialogResult.OK;
		}

		private void PatSelected(){
			if(_fillGridThread!=null) {
				return;//still filtering results (rarely happens)
			}
			long patNumSelected=PIn.Long(_DataTablePats.Rows[gridMain.GetSelectedIndex()]["PatNum"].ToString());
			if(PrefC.HasClinicsEnabled){
				long patClinicNum=PIn.Long(_DataTablePats.Rows[gridMain.GetSelectedIndex()]["ClinicNum"].ToString());
				List<long> listUserClinicNums=_listClinics.Select(x => x.ClinicNum).ToList();
				if(!Security.CurUser.ClinicIsRestricted) {
					listUserClinicNums.Add(0);
				}
				//If the user has security permissions to search all patients, or patient is assigned to one of the user's unrestricted clinics,
				//or patient has an appointment in one of the user's unrestricted clincis, 
				//allow them to select the patient
				if(Security.IsAuthorized(Permissions.UnrestrictedSearch,true) 
					|| patClinicNum.In(listUserClinicNums)
					|| Appointments.GetAppointmentsForPat(patNumSelected).Select(x => x.ClinicNum).Any(x => x.In(listUserClinicNums))) 
				{
					SelectedPatNum=patNumSelected;
					DialogResult=DialogResult.OK;
				}
				else {//Otherwise, present the error message explainign why they cannot select the patient.
					MsgBox.Show(this,"This patient is assigned to a clinic that you are not authorized for. Contact an Administrator to grant you access or to " +
						"create an appointment in your clinic to avoid patient duplication.");
				}
			}
			else {
				SelectedPatNum=patNumSelected;
				DialogResult=DialogResult.OK;
			}
		}

		#region On Screen Keyboard

		private void butOnScreenKeyboard_Click(object sender, EventArgs e){
			//Toggle the On Screen Keyboard
			if(_processOnScreenKeyboard==null) {
				OpenOnScreenKeyBoard();
			}
			else {
				CloseOnScreenKeyBoard();
			}
		}

		///<summary>This event handler fires when the user closes the On Screen Keyboard by pressing its "X" button.</summary>
		public void OnScreenKeyboardClosedEventHandler(object sender, EventArgs e){
			ODException.SwallowAnyException(CloseOnScreenKeyBoard);
		}

		///<summary>Closes the On Screen Keyboard</summary>
		private void CloseOnScreenKeyBoard() {
			//Remove the on screen keyboard process if it exists
			if(_processOnScreenKeyboard==null) {
				return;
			}
			ODException.SwallowAnyException(() => {
				//Remove the event handler (prevents it from firing when the On Screen Keys are closed by killing a process)
				_processOnScreenKeyboard.Exited-=OnScreenKeyboardClosedEventHandler;
				//If the on screen keyboard process is still running, kill it
				if(!_processOnScreenKeyboard.HasExited) {
					//Focus on the Form Patient Select before killing the On Screen Keyboard Process to avoid a threading error
					//textLName.Select();
					_processOnScreenKeyboard.Refresh();
					_processOnScreenKeyboard.CloseMainWindow();
				}
				_processOnScreenKeyboard=null;
			});
			if(selectedTxtBox!=null) {
				SelectTextBox();
			}
		}

		///<summary>Opens the On Screen Keyboard</summary>
		private void OpenOnScreenKeyBoard() {
			//Load the on screen keyboard process
			//For 64-Bit Systems, running the 32-bit On Screen Keyboard app requires disabling the Windows on Windows (WOW) path redirection, in order to access the System32 folder
			IntPtr wow64Value = IntPtr.Zero;
			if(Environment.Is64BitOperatingSystem){
				Wow64DisableWow64FsRedirection(ref wow64Value);
			}
			_processOnScreenKeyboard=new Process();
			ProcessStartInfo processStartInfo=new ProcessStartInfo();
			string strPath=System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),Environment.SystemDirectory,"osk.exe");
			processStartInfo.FileName=strPath;
			_processOnScreenKeyboard.StartInfo=processStartInfo;
			//add the event handle for when the on screen keyboard's close button is pressed
			_processOnScreenKeyboard.EnableRaisingEvents=true;
			_processOnScreenKeyboard.Exited+=OnScreenKeyboardClosedEventHandler;
			_processOnScreenKeyboard.Start();
			//Re-enable the WOW path redirection after starting the 32-bit process
			if(Environment.Is64BitOperatingSystem){
				Wow64RevertWow64FsRedirection(wow64Value);
			}
			SelectTextBox();
		}

		///<summary>Focus on the Form Patient Select at the last selected textbox.</summary>
		private void SelectTextBox() {
			if(selectedTxtBox==null) {
				selectedTxtBox=textLName;//Default to the first TextBox in the search criteria if none selected.
			}
			selectedTxtBox.Focus();
			selectedTxtBox.Select(selectedTxtBox.Text.Length,0);
		}

		/// <summary>Keeps track of the latest Selected TextBox (used to maintain cursor location when opening the On-Screen Keyboard)</summary>
		private void textBox_Enter(object sender,EventArgs e) {
			selectedTxtBox=(TextBox)sender;
		}

		/// <summary>Sets the handler for all of the Form's TextBox Enter Events</summary>
		private void SetAllTextBoxEnterEventListeners() {
			foreach(TextBox textBox in this.GetAllControls().OfType<TextBox>()) {
				textBox.Enter+=new System.EventHandler(this.textBox_Enter);
			}
		}
		#endregion

		private void butOK_Click(object sender, System.EventArgs e){
			if(gridMain.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			PatSelected();
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormPatientSelect_FormClosing(object sender,FormClosingEventArgs e) {
			//Try and close the on screen keyboard if one was opened by this form.
			ODException.SwallowAnyException(CloseOnScreenKeyBoard);
			timerFillGrid?.Dispose();//dispose of the timer if it is not null
		}

	}
}
