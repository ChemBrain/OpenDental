using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeBase;
using System.Data;
using System.Text;

namespace OpenDental{
///<summary></summary>
	public class FormProvEdit : ODForm {
		#region UI Elements
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.ComponentModel.Container components = null;// Required designer variable.
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.GroupBox groupBox2;
		private OpenDental.TableProvIdent tbProvIdent;
		private System.Windows.Forms.Label label2;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butAdd;
		private System.Windows.Forms.ComboBox comboSchoolClass;
		private System.Windows.Forms.Label labelSchoolClass;
		private CheckBox checkIsInstructor;
		private TextBox textUserName;
		private TextBox textPassword;
		private TextBox textProvNum;
		private Label label17;
		private Label label16;
		private Label label18;
		private Label labelPassDescription;
		private TabControl tabControlProvider;
		private TabPage tabGeneral;
		private ValidDate textBirthdate;
		private Label label22;
		private TextBox textSchedRules;
		private Label labelSchedRules;
		private CheckBox checkUseErx;
		private CheckBox checkIsHiddenOnReports;
		private Label label21;
		private TextBox textCustomID;
		private Label label20;
		private TextBox textProviderID;
		private UI.ComboBoxPlus comboProv;
		private Label label19;
		private Label labelEhrMU;
		private ComboBox comboEhrMu;
		private TextBox textStateWhereLicensed;
		private Label label15;
		private CheckBox checkIsNotPerson;
		private TextBox textStateRxID;
		private Label label12;
		private TextBox textEcwID;
		private Label labelEcwID;
		private CheckBox checkIsCDAnet;
		private TextBox textTaxonomyOverride;
		private Label label4;
		private GroupBox groupAnesthProvType;
		private RadioButton radAsstCirc;
		private RadioButton radAnesthSurg;
		private RadioButton radNone;
		private Label labelAnesthProvs;
		private TextBox textCanadianOfficeNum;
		private Label labelCanadianOfficeNum;
		private TextBox textNationalProvID;
		private Label labelNPI;
		private Label label14;
		private TextBox textMedicaidID;
		private TextBox textDEANum;
		private TextBox textLName;
		private TextBox textFName;
		private TextBox textMI;
		private TextBox textSuffix;
		private TextBox textStateLicense;
		private TextBox textAbbr;
		private Label label13;
		private CheckBox checkSigOnFile;
		private GroupBox groupBox1;
		private RadioButton radioTIN;
		private RadioButton radioSSN;
		private TextBox textSSN;
		private ListBox listSpecialty;
		private ListBox listFeeSched;
		private CheckBox checkIsSecondary;
		private Label label11;
		private Label label10;
		private Label label9;
		private Label label8;
		private Label label7;
		private Label label6;
		private Label label5;
		private Label label3;
		private Label label1;
		private CheckBox checkIsHidden;
		private Label labelColor;
		private TabPage tabSupplementalIDs;
		private TabPage tabDentalSchools;
		private TabPage tabWebSched;
		private TextBox textWebSchedDescript;
		private Label label24;
		private Label label23;
		private PictureBox pictureWebSched;
		private UI.Button butPickPict;
		private UI.Button butPictureNone;
		private TabPage tabClinics;
		private CheckBox checkAllClinics;
		private ListBox listBoxClinics;
		private Label labelClinics;
		#endregion UI Elements
		///<summary>Provider Identifiers showing in the list for this provider.</summary>
		private ProviderIdent[] ListProvIdent;
		private List<SchoolClass> _listSchoolClasses;
		private List<FeeSched> _listFeeSchedShort;
		private Userod _existingUser;
		public Provider ProvCur;
		private ValidDouble textProdGoalHr;
		private Label labelProdGoalHr;
		private List<ProviderClinic> _listProvClinicsOld;
		private List<ProviderClinic> _listProvClinicsNew;
		private ProviderClinic _provClinicDefault;
		private Label labelTermDate;
		private UI.ODDatePicker dateTerm;
		private CheckBox checkAllowLegacy;
		private UI.Button butClinicOverrides;
		private GroupBox groupClinicOverrides;
		///<summary>The clinics this provider is linked to. May include clinics the user does not have access to.</summary>
		private List<ProviderClinicLink> _listProvClinicLinks=new List<ProviderClinicLink>();
		///<summary>The clinics this user has access to.</summary>
		private List<Clinic> _listClinicsForUser=new List<Clinic>();
		private Label labelClinicsDesc;
		private UI.ODColorPicker odColorPickerOutline;
		private UI.ODColorPicker odColorPickerAppt;

		///<summary></summary>
		public bool IsNew;

		///<summary></summary>
		public FormProvEdit(){
			InitializeComponent();// Required for Windows Form Designer support
			Lan.F(this);
			//ProvCur=provCur;
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				labelNPI.Text=Lan.g(this,"CDA Number");
			}
			else{
				labelCanadianOfficeNum.Visible=false;
				textCanadianOfficeNum.Visible=false;
			}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProvEdit));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.tbProvIdent = new OpenDental.TableProvIdent();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butAdd = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.comboSchoolClass = new System.Windows.Forms.ComboBox();
			this.labelSchoolClass = new System.Windows.Forms.Label();
			this.checkIsInstructor = new System.Windows.Forms.CheckBox();
			this.labelPassDescription = new System.Windows.Forms.Label();
			this.textUserName = new System.Windows.Forms.TextBox();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.textProvNum = new System.Windows.Forms.TextBox();
			this.label17 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.tabControlProvider = new System.Windows.Forms.TabControl();
			this.tabGeneral = new System.Windows.Forms.TabPage();
			this.groupClinicOverrides = new System.Windows.Forms.GroupBox();
			this.label15 = new System.Windows.Forms.Label();
			this.butClinicOverrides = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.textStateLicense = new System.Windows.Forms.TextBox();
			this.textDEANum = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.textStateRxID = new System.Windows.Forms.TextBox();
			this.textStateWhereLicensed = new System.Windows.Forms.TextBox();
			this.checkAllowLegacy = new System.Windows.Forms.CheckBox();
			this.labelTermDate = new System.Windows.Forms.Label();
			this.textProdGoalHr = new OpenDental.ValidDouble();
			this.labelProdGoalHr = new System.Windows.Forms.Label();
			this.textBirthdate = new OpenDental.ValidDate();
			this.label22 = new System.Windows.Forms.Label();
			this.textSchedRules = new System.Windows.Forms.TextBox();
			this.labelSchedRules = new System.Windows.Forms.Label();
			this.checkUseErx = new System.Windows.Forms.CheckBox();
			this.checkIsHiddenOnReports = new System.Windows.Forms.CheckBox();
			this.label21 = new System.Windows.Forms.Label();
			this.textCustomID = new System.Windows.Forms.TextBox();
			this.label20 = new System.Windows.Forms.Label();
			this.textProviderID = new System.Windows.Forms.TextBox();
			this.comboProv = new OpenDental.UI.ComboBoxPlus();
			this.label19 = new System.Windows.Forms.Label();
			this.labelEhrMU = new System.Windows.Forms.Label();
			this.comboEhrMu = new System.Windows.Forms.ComboBox();
			this.checkIsNotPerson = new System.Windows.Forms.CheckBox();
			this.textEcwID = new System.Windows.Forms.TextBox();
			this.labelEcwID = new System.Windows.Forms.Label();
			this.checkIsCDAnet = new System.Windows.Forms.CheckBox();
			this.textTaxonomyOverride = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupAnesthProvType = new System.Windows.Forms.GroupBox();
			this.radAsstCirc = new System.Windows.Forms.RadioButton();
			this.radAnesthSurg = new System.Windows.Forms.RadioButton();
			this.radNone = new System.Windows.Forms.RadioButton();
			this.labelAnesthProvs = new System.Windows.Forms.Label();
			this.textCanadianOfficeNum = new System.Windows.Forms.TextBox();
			this.labelCanadianOfficeNum = new System.Windows.Forms.Label();
			this.textNationalProvID = new System.Windows.Forms.TextBox();
			this.labelNPI = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.textMedicaidID = new System.Windows.Forms.TextBox();
			this.textLName = new System.Windows.Forms.TextBox();
			this.textFName = new System.Windows.Forms.TextBox();
			this.textMI = new System.Windows.Forms.TextBox();
			this.textSuffix = new System.Windows.Forms.TextBox();
			this.textAbbr = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.checkSigOnFile = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioTIN = new System.Windows.Forms.RadioButton();
			this.radioSSN = new System.Windows.Forms.RadioButton();
			this.textSSN = new System.Windows.Forms.TextBox();
			this.listSpecialty = new System.Windows.Forms.ListBox();
			this.listFeeSched = new System.Windows.Forms.ListBox();
			this.checkIsSecondary = new System.Windows.Forms.CheckBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.checkIsHidden = new System.Windows.Forms.CheckBox();
			this.labelColor = new System.Windows.Forms.Label();
			this.dateTerm = new OpenDental.UI.ODDatePicker();
			this.tabSupplementalIDs = new System.Windows.Forms.TabPage();
			this.tabDentalSchools = new System.Windows.Forms.TabPage();
			this.tabWebSched = new System.Windows.Forms.TabPage();
			this.butPictureNone = new OpenDental.UI.Button();
			this.butPickPict = new OpenDental.UI.Button();
			this.label24 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.pictureWebSched = new System.Windows.Forms.PictureBox();
			this.textWebSchedDescript = new System.Windows.Forms.TextBox();
			this.tabClinics = new System.Windows.Forms.TabPage();
			this.labelClinicsDesc = new System.Windows.Forms.Label();
			this.checkAllClinics = new System.Windows.Forms.CheckBox();
			this.listBoxClinics = new System.Windows.Forms.ListBox();
			this.labelClinics = new System.Windows.Forms.Label();
			this.odColorPickerAppt = new OpenDental.UI.ODColorPicker();
			this.odColorPickerOutline = new OpenDental.UI.ODColorPicker();
			this.groupBox2.SuspendLayout();
			this.tabControlProvider.SuspendLayout();
			this.tabGeneral.SuspendLayout();
			this.groupClinicOverrides.SuspendLayout();
			this.groupAnesthProvType.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabSupplementalIDs.SuspendLayout();
			this.tabDentalSchools.SuspendLayout();
			this.tabWebSched.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureWebSched)).BeginInit();
			this.tabClinics.SuspendLayout();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(726, 628);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 35;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(807, 628);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 36;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// colorDialog1
			// 
			this.colorDialog1.FullOpen = true;
			// 
			// tbProvIdent
			// 
			this.tbProvIdent.BackColor = System.Drawing.SystemColors.Window;
			this.tbProvIdent.Location = new System.Drawing.Point(11, 58);
			this.tbProvIdent.Name = "tbProvIdent";
			this.tbProvIdent.ScrollValue = 211;
			this.tbProvIdent.SelectedIndices = new int[0];
			this.tbProvIdent.SelectionMode = System.Windows.Forms.SelectionMode.One;
			this.tbProvIdent.Size = new System.Drawing.Size(319, 88);
			this.tbProvIdent.TabIndex = 43;
			this.tbProvIdent.CellDoubleClicked += new OpenDental.ContrTable.CellEventHandler(this.tbProvIdent_CellDoubleClicked);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.butAdd);
			this.groupBox2.Controls.Add(this.butDelete);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.tbProvIdent);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(19, 15);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(496, 157);
			this.groupBox2.TabIndex = 33;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Supplemental Provider Identifiers";
			// 
			// butAdd
			// 
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(360, 59);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(90, 24);
			this.butAdd.TabIndex = 0;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butDelete
			// 
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(360, 94);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(90, 24);
			this.butDelete.TabIndex = 1;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(14, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(481, 32);
			this.label2.TabIndex = 44;
			this.label2.Text = "This is where you store provider IDs assigned by individual insurance companies, " +
    "especially BC/BS.";
			// 
			// comboSchoolClass
			// 
			this.comboSchoolClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSchoolClass.Location = new System.Drawing.Point(126, 19);
			this.comboSchoolClass.MaxDropDownItems = 30;
			this.comboSchoolClass.Name = "comboSchoolClass";
			this.comboSchoolClass.Size = new System.Drawing.Size(158, 21);
			this.comboSchoolClass.TabIndex = 0;
			this.comboSchoolClass.Visible = false;
			// 
			// labelSchoolClass
			// 
			this.labelSchoolClass.Location = new System.Drawing.Point(33, 20);
			this.labelSchoolClass.Name = "labelSchoolClass";
			this.labelSchoolClass.Size = new System.Drawing.Size(93, 16);
			this.labelSchoolClass.TabIndex = 89;
			this.labelSchoolClass.Text = "Class";
			this.labelSchoolClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelSchoolClass.Visible = false;
			// 
			// checkIsInstructor
			// 
			this.checkIsInstructor.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsInstructor.Enabled = false;
			this.checkIsInstructor.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsInstructor.Location = new System.Drawing.Point(32, 119);
			this.checkIsInstructor.Name = "checkIsInstructor";
			this.checkIsInstructor.Size = new System.Drawing.Size(107, 17);
			this.checkIsInstructor.TabIndex = 4;
			this.checkIsInstructor.Text = "Is Instructor";
			this.checkIsInstructor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPassDescription
			// 
			this.labelPassDescription.Location = new System.Drawing.Point(145, 120);
			this.labelPassDescription.Name = "labelPassDescription";
			this.labelPassDescription.Size = new System.Drawing.Size(138, 27);
			this.labelPassDescription.TabIndex = 248;
			this.labelPassDescription.Text = "To keep the old password, leave the box empty.";
			this.labelPassDescription.Visible = false;
			// 
			// textUserName
			// 
			this.textUserName.Location = new System.Drawing.Point(126, 69);
			this.textUserName.MaxLength = 100;
			this.textUserName.Name = "textUserName";
			this.textUserName.Size = new System.Drawing.Size(157, 20);
			this.textUserName.TabIndex = 2;
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(126, 93);
			this.textPassword.MaxLength = 100;
			this.textPassword.Name = "textPassword";
			this.textPassword.Size = new System.Drawing.Size(157, 20);
			this.textPassword.TabIndex = 3;
			// 
			// textProvNum
			// 
			this.textProvNum.Location = new System.Drawing.Point(126, 45);
			this.textProvNum.MaxLength = 15;
			this.textProvNum.Name = "textProvNum";
			this.textProvNum.ReadOnly = true;
			this.textProvNum.Size = new System.Drawing.Size(157, 20);
			this.textProvNum.TabIndex = 1;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(30, 73);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(95, 14);
			this.label17.TabIndex = 116;
			this.label17.Text = "User Name";
			this.label17.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(56, 48);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(69, 14);
			this.label16.TabIndex = 112;
			this.label16.Text = "ProvNum";
			this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(30, 97);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(95, 14);
			this.label18.TabIndex = 115;
			this.label18.Text = "Password";
			this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tabControlProvider
			// 
			this.tabControlProvider.Controls.Add(this.tabGeneral);
			this.tabControlProvider.Controls.Add(this.tabSupplementalIDs);
			this.tabControlProvider.Controls.Add(this.tabDentalSchools);
			this.tabControlProvider.Controls.Add(this.tabWebSched);
			this.tabControlProvider.Controls.Add(this.tabClinics);
			this.tabControlProvider.Location = new System.Drawing.Point(12, 12);
			this.tabControlProvider.Name = "tabControlProvider";
			this.tabControlProvider.SelectedIndex = 0;
			this.tabControlProvider.Size = new System.Drawing.Size(870, 604);
			this.tabControlProvider.TabIndex = 268;
			// 
			// tabGeneral
			// 
			this.tabGeneral.BackColor = System.Drawing.SystemColors.Control;
			this.tabGeneral.Controls.Add(this.odColorPickerOutline);
			this.tabGeneral.Controls.Add(this.odColorPickerAppt);
			this.tabGeneral.Controls.Add(this.groupClinicOverrides);
			this.tabGeneral.Controls.Add(this.checkAllowLegacy);
			this.tabGeneral.Controls.Add(this.labelTermDate);
			this.tabGeneral.Controls.Add(this.textProdGoalHr);
			this.tabGeneral.Controls.Add(this.labelProdGoalHr);
			this.tabGeneral.Controls.Add(this.textBirthdate);
			this.tabGeneral.Controls.Add(this.label22);
			this.tabGeneral.Controls.Add(this.textSchedRules);
			this.tabGeneral.Controls.Add(this.labelSchedRules);
			this.tabGeneral.Controls.Add(this.checkUseErx);
			this.tabGeneral.Controls.Add(this.checkIsHiddenOnReports);
			this.tabGeneral.Controls.Add(this.label21);
			this.tabGeneral.Controls.Add(this.textCustomID);
			this.tabGeneral.Controls.Add(this.label20);
			this.tabGeneral.Controls.Add(this.textProviderID);
			this.tabGeneral.Controls.Add(this.comboProv);
			this.tabGeneral.Controls.Add(this.label19);
			this.tabGeneral.Controls.Add(this.labelEhrMU);
			this.tabGeneral.Controls.Add(this.comboEhrMu);
			this.tabGeneral.Controls.Add(this.checkIsNotPerson);
			this.tabGeneral.Controls.Add(this.textEcwID);
			this.tabGeneral.Controls.Add(this.labelEcwID);
			this.tabGeneral.Controls.Add(this.checkIsCDAnet);
			this.tabGeneral.Controls.Add(this.textTaxonomyOverride);
			this.tabGeneral.Controls.Add(this.label4);
			this.tabGeneral.Controls.Add(this.groupAnesthProvType);
			this.tabGeneral.Controls.Add(this.textCanadianOfficeNum);
			this.tabGeneral.Controls.Add(this.labelCanadianOfficeNum);
			this.tabGeneral.Controls.Add(this.textNationalProvID);
			this.tabGeneral.Controls.Add(this.labelNPI);
			this.tabGeneral.Controls.Add(this.label14);
			this.tabGeneral.Controls.Add(this.textMedicaidID);
			this.tabGeneral.Controls.Add(this.textLName);
			this.tabGeneral.Controls.Add(this.textFName);
			this.tabGeneral.Controls.Add(this.textMI);
			this.tabGeneral.Controls.Add(this.textSuffix);
			this.tabGeneral.Controls.Add(this.textAbbr);
			this.tabGeneral.Controls.Add(this.label13);
			this.tabGeneral.Controls.Add(this.checkSigOnFile);
			this.tabGeneral.Controls.Add(this.groupBox1);
			this.tabGeneral.Controls.Add(this.listSpecialty);
			this.tabGeneral.Controls.Add(this.listFeeSched);
			this.tabGeneral.Controls.Add(this.checkIsSecondary);
			this.tabGeneral.Controls.Add(this.label10);
			this.tabGeneral.Controls.Add(this.label9);
			this.tabGeneral.Controls.Add(this.label8);
			this.tabGeneral.Controls.Add(this.label7);
			this.tabGeneral.Controls.Add(this.label6);
			this.tabGeneral.Controls.Add(this.label5);
			this.tabGeneral.Controls.Add(this.label1);
			this.tabGeneral.Controls.Add(this.checkIsHidden);
			this.tabGeneral.Controls.Add(this.labelColor);
			this.tabGeneral.Controls.Add(this.dateTerm);
			this.tabGeneral.Location = new System.Drawing.Point(4, 22);
			this.tabGeneral.Name = "tabGeneral";
			this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tabGeneral.Size = new System.Drawing.Size(862, 578);
			this.tabGeneral.TabIndex = 0;
			this.tabGeneral.Text = "General";
			// 
			// groupClinicOverrides
			// 
			this.groupClinicOverrides.Controls.Add(this.label15);
			this.groupClinicOverrides.Controls.Add(this.butClinicOverrides);
			this.groupClinicOverrides.Controls.Add(this.label3);
			this.groupClinicOverrides.Controls.Add(this.label11);
			this.groupClinicOverrides.Controls.Add(this.textStateLicense);
			this.groupClinicOverrides.Controls.Add(this.textDEANum);
			this.groupClinicOverrides.Controls.Add(this.label12);
			this.groupClinicOverrides.Controls.Add(this.textStateRxID);
			this.groupClinicOverrides.Controls.Add(this.textStateWhereLicensed);
			this.groupClinicOverrides.Location = new System.Drawing.Point(37, 269);
			this.groupClinicOverrides.Name = "groupClinicOverrides";
			this.groupClinicOverrides.Size = new System.Drawing.Size(340, 98);
			this.groupClinicOverrides.TabIndex = 341;
			this.groupClinicOverrides.TabStop = false;
			this.groupClinicOverrides.Text = "Clinic Overrides";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(24, 36);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(139, 14);
			this.label15.TabIndex = 319;
			this.label15.Text = "State Where Licensed";
			this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butClinicOverrides
			// 
			this.butClinicOverrides.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butClinicOverrides.Location = new System.Drawing.Point(287, 72);
			this.butClinicOverrides.Name = "butClinicOverrides";
			this.butClinicOverrides.Size = new System.Drawing.Size(47, 21);
			this.butClinicOverrides.TabIndex = 329;
			this.butClinicOverrides.Text = "Edit";
			this.butClinicOverrides.UseVisualStyleBackColor = true;
			this.butClinicOverrides.Click += new System.EventHandler(this.butClinicOverrides_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(22, 15);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(142, 14);
			this.label3.TabIndex = 285;
			this.label3.Text = "State License Number";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(26, 57);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(137, 14);
			this.label11.TabIndex = 299;
			this.label11.Text = "DEA Number";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textStateLicense
			// 
			this.textStateLicense.Location = new System.Drawing.Point(165, 11);
			this.textStateLicense.MaxLength = 15;
			this.textStateLicense.Name = "textStateLicense";
			this.textStateLicense.Size = new System.Drawing.Size(102, 20);
			this.textStateLicense.TabIndex = 275;
			// 
			// textDEANum
			// 
			this.textDEANum.Location = new System.Drawing.Point(165, 53);
			this.textDEANum.MaxLength = 15;
			this.textDEANum.Name = "textDEANum";
			this.textDEANum.Size = new System.Drawing.Size(102, 20);
			this.textDEANum.TabIndex = 277;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(27, 78);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(137, 14);
			this.label12.TabIndex = 318;
			this.label12.Text = "State Rx ID";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textStateRxID
			// 
			this.textStateRxID.Location = new System.Drawing.Point(165, 74);
			this.textStateRxID.MaxLength = 15;
			this.textStateRxID.Name = "textStateRxID";
			this.textStateRxID.Size = new System.Drawing.Size(102, 20);
			this.textStateRxID.TabIndex = 279;
			// 
			// textStateWhereLicensed
			// 
			this.textStateWhereLicensed.Location = new System.Drawing.Point(165, 32);
			this.textStateWhereLicensed.MaxLength = 15;
			this.textStateWhereLicensed.Name = "textStateWhereLicensed";
			this.textStateWhereLicensed.Size = new System.Drawing.Size(34, 20);
			this.textStateWhereLicensed.TabIndex = 276;
			// 
			// checkAllowLegacy
			// 
			this.checkAllowLegacy.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllowLegacy.Location = new System.Drawing.Point(640, 526);
			this.checkAllowLegacy.Name = "checkAllowLegacy";
			this.checkAllowLegacy.Size = new System.Drawing.Size(189, 17);
			this.checkAllowLegacy.TabIndex = 340;
			this.checkAllowLegacy.Text = "Allow Legacy eRx Option";
			this.checkAllowLegacy.Visible = false;
			// 
			// labelTermDate
			// 
			this.labelTermDate.Location = new System.Drawing.Point(106, 436);
			this.labelTermDate.Name = "labelTermDate";
			this.labelTermDate.Size = new System.Drawing.Size(95, 16);
			this.labelTermDate.TabIndex = 330;
			this.labelTermDate.Text = "Term Date";
			this.labelTermDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textProdGoalHr
			// 
			this.textProdGoalHr.Location = new System.Drawing.Point(202, 540);
			this.textProdGoalHr.MaxVal = 100000000D;
			this.textProdGoalHr.MinVal = -100000000D;
			this.textProdGoalHr.Name = "textProdGoalHr";
			this.textProdGoalHr.Size = new System.Drawing.Size(102, 20);
			this.textProdGoalHr.TabIndex = 291;
			// 
			// labelProdGoalHr
			// 
			this.labelProdGoalHr.Location = new System.Drawing.Point(33, 540);
			this.labelProdGoalHr.Name = "labelProdGoalHr";
			this.labelProdGoalHr.Size = new System.Drawing.Size(168, 21);
			this.labelProdGoalHr.TabIndex = 328;
			this.labelProdGoalHr.Text = "Hourly Production Goal";
			this.labelProdGoalHr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBirthdate
			// 
			this.textBirthdate.Location = new System.Drawing.Point(202, 164);
			this.textBirthdate.Name = "textBirthdate";
			this.textBirthdate.Size = new System.Drawing.Size(102, 20);
			this.textBirthdate.TabIndex = 273;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(54, 168);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(147, 14);
			this.label22.TabIndex = 326;
			this.label22.Text = "Birthdate";
			this.label22.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textSchedRules
			// 
			this.textSchedRules.AcceptsReturn = true;
			this.textSchedRules.Location = new System.Drawing.Point(419, 240);
			this.textSchedRules.Multiline = true;
			this.textSchedRules.Name = "textSchedRules";
			this.textSchedRules.Size = new System.Drawing.Size(330, 60);
			this.textSchedRules.TabIndex = 300;
			// 
			// labelSchedRules
			// 
			this.labelSchedRules.Location = new System.Drawing.Point(417, 223);
			this.labelSchedRules.Name = "labelSchedRules";
			this.labelSchedRules.Size = new System.Drawing.Size(170, 14);
			this.labelSchedRules.TabIndex = 325;
			this.labelSchedRules.Text = "Scheduling Note";
			this.labelSchedRules.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkUseErx
			// 
			this.checkUseErx.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUseErx.Location = new System.Drawing.Point(419, 526);
			this.checkUseErx.Name = "checkUseErx";
			this.checkUseErx.Size = new System.Drawing.Size(212, 17);
			this.checkUseErx.TabIndex = 310;
			this.checkUseErx.Text = "Use Electronic Prescriptions (eRx)";
			// 
			// checkIsHiddenOnReports
			// 
			this.checkIsHiddenOnReports.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsHiddenOnReports.Location = new System.Drawing.Point(419, 510);
			this.checkIsHiddenOnReports.Name = "checkIsHiddenOnReports";
			this.checkIsHiddenOnReports.Size = new System.Drawing.Size(158, 17);
			this.checkIsHiddenOnReports.TabIndex = 309;
			this.checkIsHiddenOnReports.Text = "Hidden On Reports";
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(417, 301);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(107, 14);
			this.label21.TabIndex = 324;
			this.label21.Text = "Custom ID";
			this.label21.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textCustomID
			// 
			this.textCustomID.Location = new System.Drawing.Point(419, 318);
			this.textCustomID.MaxLength = 255;
			this.textCustomID.Name = "textCustomID";
			this.textCustomID.Size = new System.Drawing.Size(108, 20);
			this.textCustomID.TabIndex = 301;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(64, 41);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(136, 14);
			this.label20.TabIndex = 323;
			this.label20.Text = "Provider ID";
			this.label20.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textProviderID
			// 
			this.textProviderID.Location = new System.Drawing.Point(202, 38);
			this.textProviderID.MaxLength = 255;
			this.textProviderID.Name = "textProviderID";
			this.textProviderID.ReadOnly = true;
			this.textProviderID.Size = new System.Drawing.Size(121, 20);
			this.textProviderID.TabIndex = 322;
			this.textProviderID.TabStop = false;
			// 
			// comboProv
			// 
			this.comboProv.Location = new System.Drawing.Point(202, 518);
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(161, 21);
			this.comboProv.TabIndex = 290;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(34, 518);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(168, 21);
			this.label19.TabIndex = 321;
			this.label19.Text = "Claim Billing Prov Override";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelEhrMU
			// 
			this.labelEhrMU.Location = new System.Drawing.Point(57, 496);
			this.labelEhrMU.Name = "labelEhrMU";
			this.labelEhrMU.Size = new System.Drawing.Size(144, 21);
			this.labelEhrMU.TabIndex = 320;
			this.labelEhrMU.Text = "EHR Meaningful Use";
			this.labelEhrMU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboEhrMu
			// 
			this.comboEhrMu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboEhrMu.Location = new System.Drawing.Point(202, 496);
			this.comboEhrMu.MaxDropDownItems = 30;
			this.comboEhrMu.Name = "comboEhrMu";
			this.comboEhrMu.Size = new System.Drawing.Size(121, 21);
			this.comboEhrMu.TabIndex = 288;
			// 
			// checkIsNotPerson
			// 
			this.checkIsNotPerson.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsNotPerson.Location = new System.Drawing.Point(419, 478);
			this.checkIsNotPerson.Name = "checkIsNotPerson";
			this.checkIsNotPerson.Size = new System.Drawing.Size(410, 17);
			this.checkIsNotPerson.TabIndex = 307;
			this.checkIsNotPerson.Text = "Not a Person (for example, a dummy provider representing the organization)";
			// 
			// textEcwID
			// 
			this.textEcwID.Location = new System.Drawing.Point(202, 17);
			this.textEcwID.MaxLength = 255;
			this.textEcwID.Name = "textEcwID";
			this.textEcwID.ReadOnly = true;
			this.textEcwID.Size = new System.Drawing.Size(121, 20);
			this.textEcwID.TabIndex = 316;
			// 
			// labelEcwID
			// 
			this.labelEcwID.Location = new System.Drawing.Point(65, 21);
			this.labelEcwID.Name = "labelEcwID";
			this.labelEcwID.Size = new System.Drawing.Size(136, 14);
			this.labelEcwID.TabIndex = 317;
			this.labelEcwID.Text = "eCW ID";
			this.labelEcwID.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkIsCDAnet
			// 
			this.checkIsCDAnet.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsCDAnet.Location = new System.Drawing.Point(419, 430);
			this.checkIsCDAnet.Name = "checkIsCDAnet";
			this.checkIsCDAnet.Size = new System.Drawing.Size(168, 17);
			this.checkIsCDAnet.TabIndex = 304;
			this.checkIsCDAnet.Text = "Is CDAnet Member";
			this.checkIsCDAnet.Visible = false;
			// 
			// textTaxonomyOverride
			// 
			this.textTaxonomyOverride.Location = new System.Drawing.Point(595, 318);
			this.textTaxonomyOverride.MaxLength = 255;
			this.textTaxonomyOverride.Name = "textTaxonomyOverride";
			this.textTaxonomyOverride.Size = new System.Drawing.Size(154, 20);
			this.textTaxonomyOverride.TabIndex = 302;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(592, 301);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(154, 14);
			this.label4.TabIndex = 315;
			this.label4.Text = "Taxonomy Code Override";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupAnesthProvType
			// 
			this.groupAnesthProvType.Controls.Add(this.radAsstCirc);
			this.groupAnesthProvType.Controls.Add(this.radAnesthSurg);
			this.groupAnesthProvType.Controls.Add(this.radNone);
			this.groupAnesthProvType.Controls.Add(this.labelAnesthProvs);
			this.groupAnesthProvType.Location = new System.Drawing.Point(407, 346);
			this.groupAnesthProvType.Name = "groupAnesthProvType";
			this.groupAnesthProvType.Size = new System.Drawing.Size(347, 83);
			this.groupAnesthProvType.TabIndex = 303;
			this.groupAnesthProvType.TabStop = false;
			this.groupAnesthProvType.Text = "Anesthesia Provider Groups (optional)";
			// 
			// radAsstCirc
			// 
			this.radAsstCirc.AutoSize = true;
			this.radAsstCirc.Location = new System.Drawing.Point(16, 56);
			this.radAsstCirc.Name = "radAsstCirc";
			this.radAsstCirc.Size = new System.Drawing.Size(116, 17);
			this.radAsstCirc.TabIndex = 9;
			this.radAsstCirc.Text = "Assistant/Circulator";
			this.radAsstCirc.UseVisualStyleBackColor = true;
			// 
			// radAnesthSurg
			// 
			this.radAnesthSurg.AutoSize = true;
			this.radAnesthSurg.Location = new System.Drawing.Point(16, 37);
			this.radAnesthSurg.Name = "radAnesthSurg";
			this.radAnesthSurg.Size = new System.Drawing.Size(122, 17);
			this.radAnesthSurg.TabIndex = 8;
			this.radAnesthSurg.Text = "Anesthetist/Surgeon";
			this.radAnesthSurg.UseVisualStyleBackColor = true;
			// 
			// radNone
			// 
			this.radNone.AutoSize = true;
			this.radNone.Checked = true;
			this.radNone.Location = new System.Drawing.Point(16, 18);
			this.radNone.Name = "radNone";
			this.radNone.Size = new System.Drawing.Size(51, 17);
			this.radNone.TabIndex = 7;
			this.radNone.TabStop = true;
			this.radNone.Text = "None";
			this.radNone.UseVisualStyleBackColor = true;
			// 
			// labelAnesthProvs
			// 
			this.labelAnesthProvs.Location = new System.Drawing.Point(157, 22);
			this.labelAnesthProvs.Name = "labelAnesthProvs";
			this.labelAnesthProvs.Size = new System.Drawing.Size(188, 52);
			this.labelAnesthProvs.TabIndex = 4;
			this.labelAnesthProvs.Text = "Assign this user to a group. This will populate the corresponding dropdowns on th" +
    "e Anesthetic Record.";
			// 
			// textCanadianOfficeNum
			// 
			this.textCanadianOfficeNum.Location = new System.Drawing.Point(202, 412);
			this.textCanadianOfficeNum.MaxLength = 20;
			this.textCanadianOfficeNum.Name = "textCanadianOfficeNum";
			this.textCanadianOfficeNum.Size = new System.Drawing.Size(102, 20);
			this.textCanadianOfficeNum.TabIndex = 283;
			// 
			// labelCanadianOfficeNum
			// 
			this.labelCanadianOfficeNum.Location = new System.Drawing.Point(60, 416);
			this.labelCanadianOfficeNum.Name = "labelCanadianOfficeNum";
			this.labelCanadianOfficeNum.Size = new System.Drawing.Size(141, 14);
			this.labelCanadianOfficeNum.TabIndex = 314;
			this.labelCanadianOfficeNum.Text = "Office Number";
			this.labelCanadianOfficeNum.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textNationalProvID
			// 
			this.textNationalProvID.Location = new System.Drawing.Point(202, 391);
			this.textNationalProvID.MaxLength = 20;
			this.textNationalProvID.Name = "textNationalProvID";
			this.textNationalProvID.Size = new System.Drawing.Size(102, 20);
			this.textNationalProvID.TabIndex = 282;
			// 
			// labelNPI
			// 
			this.labelNPI.Location = new System.Drawing.Point(60, 395);
			this.labelNPI.Name = "labelNPI";
			this.labelNPI.Size = new System.Drawing.Size(141, 14);
			this.labelNPI.TabIndex = 313;
			this.labelNPI.Text = "National Provider ID";
			this.labelNPI.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(62, 478);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(140, 16);
			this.label14.TabIndex = 312;
			this.label14.Text = "Highlight Outline Color";
			this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textMedicaidID
			// 
			this.textMedicaidID.Location = new System.Drawing.Point(202, 370);
			this.textMedicaidID.MaxLength = 20;
			this.textMedicaidID.Name = "textMedicaidID";
			this.textMedicaidID.Size = new System.Drawing.Size(102, 20);
			this.textMedicaidID.TabIndex = 280;
			// 
			// textLName
			// 
			this.textLName.Location = new System.Drawing.Point(202, 80);
			this.textLName.MaxLength = 100;
			this.textLName.Name = "textLName";
			this.textLName.Size = new System.Drawing.Size(161, 20);
			this.textLName.TabIndex = 269;
			// 
			// textFName
			// 
			this.textFName.Location = new System.Drawing.Point(202, 101);
			this.textFName.MaxLength = 100;
			this.textFName.Name = "textFName";
			this.textFName.Size = new System.Drawing.Size(161, 20);
			this.textFName.TabIndex = 270;
			// 
			// textMI
			// 
			this.textMI.Location = new System.Drawing.Point(202, 122);
			this.textMI.MaxLength = 100;
			this.textMI.Name = "textMI";
			this.textMI.Size = new System.Drawing.Size(63, 20);
			this.textMI.TabIndex = 271;
			// 
			// textSuffix
			// 
			this.textSuffix.Location = new System.Drawing.Point(202, 143);
			this.textSuffix.MaxLength = 100;
			this.textSuffix.Name = "textSuffix";
			this.textSuffix.Size = new System.Drawing.Size(102, 20);
			this.textSuffix.TabIndex = 272;
			// 
			// textAbbr
			// 
			this.textAbbr.Location = new System.Drawing.Point(202, 59);
			this.textAbbr.MaxLength = 255;
			this.textAbbr.Name = "textAbbr";
			this.textAbbr.Size = new System.Drawing.Size(121, 20);
			this.textAbbr.TabIndex = 268;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(60, 374);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(141, 14);
			this.label13.TabIndex = 311;
			this.label13.Text = "Medicaid ID";
			this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkSigOnFile
			// 
			this.checkSigOnFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSigOnFile.Location = new System.Drawing.Point(419, 462);
			this.checkSigOnFile.Name = "checkSigOnFile";
			this.checkSigOnFile.Size = new System.Drawing.Size(121, 17);
			this.checkSigOnFile.TabIndex = 306;
			this.checkSigOnFile.Text = "Signature on File";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioTIN);
			this.groupBox1.Controls.Add(this.radioSSN);
			this.groupBox1.Controls.Add(this.textSSN);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(194, 190);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(156, 80);
			this.groupBox1.TabIndex = 274;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "SSN or TIN (no dashes)";
			// 
			// radioTIN
			// 
			this.radioTIN.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioTIN.Location = new System.Drawing.Point(13, 34);
			this.radioTIN.Name = "radioTIN";
			this.radioTIN.Size = new System.Drawing.Size(135, 15);
			this.radioTIN.TabIndex = 1;
			this.radioTIN.Text = "TIN";
			this.radioTIN.Click += new System.EventHandler(this.radioTIN_Click);
			// 
			// radioSSN
			// 
			this.radioSSN.Checked = true;
			this.radioSSN.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioSSN.Location = new System.Drawing.Point(13, 17);
			this.radioSSN.Name = "radioSSN";
			this.radioSSN.Size = new System.Drawing.Size(104, 14);
			this.radioSSN.TabIndex = 0;
			this.radioSSN.TabStop = true;
			this.radioSSN.Text = "SSN";
			this.radioSSN.Click += new System.EventHandler(this.radioSSN_Click);
			// 
			// textSSN
			// 
			this.textSSN.Location = new System.Drawing.Point(8, 54);
			this.textSSN.Name = "textSSN";
			this.textSSN.Size = new System.Drawing.Size(102, 20);
			this.textSSN.TabIndex = 2;
			// 
			// listSpecialty
			// 
			this.listSpecialty.Items.AddRange(new object[] {
            "Dental General Practice",
            "Dental Hygienist",
            "Endodontics",
            "Pediatric Dentistry",
            "Periodontics",
            "Prosthodontics",
            "Orthodontics",
            "Denturist",
            "Surgery, Oral & Maxillofacial",
            "Dental Assistant",
            "Dental Laboratory Technician",
            "Pathology, Oral & MaxFac",
            "Public Health",
            "Radiology"});
			this.listSpecialty.Location = new System.Drawing.Point(598, 34);
			this.listSpecialty.Name = "listSpecialty";
			this.listSpecialty.Size = new System.Drawing.Size(154, 186);
			this.listSpecialty.TabIndex = 297;
			// 
			// listFeeSched
			// 
			this.listFeeSched.Location = new System.Drawing.Point(419, 34);
			this.listFeeSched.Name = "listFeeSched";
			this.listFeeSched.Size = new System.Drawing.Size(168, 186);
			this.listFeeSched.TabIndex = 296;
			// 
			// checkIsSecondary
			// 
			this.checkIsSecondary.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsSecondary.Location = new System.Drawing.Point(419, 446);
			this.checkIsSecondary.Name = "checkIsSecondary";
			this.checkIsSecondary.Size = new System.Drawing.Size(155, 17);
			this.checkIsSecondary.TabIndex = 305;
			this.checkIsSecondary.Text = "Secondary Provider (Hyg)";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(57, 84);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(143, 14);
			this.label10.TabIndex = 298;
			this.label10.Text = "Last Name";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(54, 147);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(147, 14);
			this.label9.TabIndex = 295;
			this.label9.Text = "Suffix (MD,DMD,DDS,etc)";
			this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(63, 105);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(138, 14);
			this.label8.TabIndex = 293;
			this.label8.Text = "First Name";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(94, 126);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(102, 14);
			this.label7.TabIndex = 338;
			this.label7.Text = "MI";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(417, 17);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(105, 14);
			this.label6.TabIndex = 289;
			this.label6.Text = "Fee Schedule";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(598, 17);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(116, 14);
			this.label5.TabIndex = 287;
			this.label5.Text = "Specialty";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(65, 63);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 14);
			this.label1.TabIndex = 281;
			this.label1.Text = "Abbreviation";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkIsHidden
			// 
			this.checkIsHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsHidden.Location = new System.Drawing.Point(419, 494);
			this.checkIsHidden.Name = "checkIsHidden";
			this.checkIsHidden.Size = new System.Drawing.Size(158, 17);
			this.checkIsHidden.TabIndex = 308;
			this.checkIsHidden.Text = "Hidden";
			// 
			// labelColor
			// 
			this.labelColor.Location = new System.Drawing.Point(62, 457);
			this.labelColor.Name = "labelColor";
			this.labelColor.Size = new System.Drawing.Size(140, 16);
			this.labelColor.TabIndex = 278;
			this.labelColor.Text = "Appointment Color";
			this.labelColor.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// dateTerm
			// 
			this.dateTerm.BackColor = System.Drawing.SystemColors.Control;
			this.dateTerm.CalendarLocation = OpenDental.UI.ODDatePicker.CalendarLocationOptions.ToTheRight;
			this.dateTerm.DefaultDateTime = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
			this.dateTerm.Location = new System.Drawing.Point(139, 432);
			this.dateTerm.MaximumSize = new System.Drawing.Size(0, 184);
			this.dateTerm.MinimumSize = new System.Drawing.Size(228, 184);
			this.dateTerm.Name = "dateTerm";
			this.dateTerm.Size = new System.Drawing.Size(228, 184);
			this.dateTerm.TabIndex = 339;
			// 
			// tabSupplementalIDs
			// 
			this.tabSupplementalIDs.BackColor = System.Drawing.SystemColors.Control;
			this.tabSupplementalIDs.Controls.Add(this.groupBox2);
			this.tabSupplementalIDs.Location = new System.Drawing.Point(4, 22);
			this.tabSupplementalIDs.Name = "tabSupplementalIDs";
			this.tabSupplementalIDs.Padding = new System.Windows.Forms.Padding(3);
			this.tabSupplementalIDs.Size = new System.Drawing.Size(862, 578);
			this.tabSupplementalIDs.TabIndex = 1;
			this.tabSupplementalIDs.Text = "Supplemental IDs";
			// 
			// tabDentalSchools
			// 
			this.tabDentalSchools.BackColor = System.Drawing.SystemColors.Control;
			this.tabDentalSchools.Controls.Add(this.labelPassDescription);
			this.tabDentalSchools.Controls.Add(this.textUserName);
			this.tabDentalSchools.Controls.Add(this.comboSchoolClass);
			this.tabDentalSchools.Controls.Add(this.textPassword);
			this.tabDentalSchools.Controls.Add(this.checkIsInstructor);
			this.tabDentalSchools.Controls.Add(this.textProvNum);
			this.tabDentalSchools.Controls.Add(this.labelSchoolClass);
			this.tabDentalSchools.Controls.Add(this.label17);
			this.tabDentalSchools.Controls.Add(this.label18);
			this.tabDentalSchools.Controls.Add(this.label16);
			this.tabDentalSchools.Location = new System.Drawing.Point(4, 22);
			this.tabDentalSchools.Name = "tabDentalSchools";
			this.tabDentalSchools.Padding = new System.Windows.Forms.Padding(3);
			this.tabDentalSchools.Size = new System.Drawing.Size(862, 578);
			this.tabDentalSchools.TabIndex = 2;
			this.tabDentalSchools.Text = "Dental Schools";
			// 
			// tabWebSched
			// 
			this.tabWebSched.BackColor = System.Drawing.SystemColors.Control;
			this.tabWebSched.Controls.Add(this.butPictureNone);
			this.tabWebSched.Controls.Add(this.butPickPict);
			this.tabWebSched.Controls.Add(this.label24);
			this.tabWebSched.Controls.Add(this.label23);
			this.tabWebSched.Controls.Add(this.pictureWebSched);
			this.tabWebSched.Controls.Add(this.textWebSchedDescript);
			this.tabWebSched.Location = new System.Drawing.Point(4, 22);
			this.tabWebSched.Name = "tabWebSched";
			this.tabWebSched.Padding = new System.Windows.Forms.Padding(3);
			this.tabWebSched.Size = new System.Drawing.Size(862, 578);
			this.tabWebSched.TabIndex = 3;
			this.tabWebSched.Text = "Web Sched";
			// 
			// butPictureNone
			// 
			this.butPictureNone.Location = new System.Drawing.Point(320, 141);
			this.butPictureNone.Name = "butPictureNone";
			this.butPictureNone.Size = new System.Drawing.Size(57, 23);
			this.butPictureNone.TabIndex = 117;
			this.butPictureNone.TabStop = false;
			this.butPictureNone.Text = "None";
			this.butPictureNone.Click += new System.EventHandler(this.butPictureNone_Click);
			// 
			// butPickPict
			// 
			this.butPickPict.Location = new System.Drawing.Point(286, 141);
			this.butPickPict.Name = "butPickPict";
			this.butPickPict.Size = new System.Drawing.Size(27, 23);
			this.butPickPict.TabIndex = 116;
			this.butPickPict.Text = "...";
			this.butPickPict.Click += new System.EventHandler(this.butPickPict_Click);
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(54, 141);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(93, 16);
			this.label24.TabIndex = 115;
			this.label24.Text = "Picture";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(54, 37);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(93, 16);
			this.label23.TabIndex = 114;
			this.label23.Text = "Description";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// pictureWebSched
			// 
			this.pictureWebSched.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureWebSched.Location = new System.Drawing.Point(153, 141);
			this.pictureWebSched.Name = "pictureWebSched";
			this.pictureWebSched.Size = new System.Drawing.Size(128, 128);
			this.pictureWebSched.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureWebSched.TabIndex = 113;
			this.pictureWebSched.TabStop = false;
			// 
			// textWebSchedDescript
			// 
			this.textWebSchedDescript.AcceptsReturn = true;
			this.textWebSchedDescript.AcceptsTab = true;
			this.textWebSchedDescript.BackColor = System.Drawing.SystemColors.Window;
			this.textWebSchedDescript.Location = new System.Drawing.Point(153, 36);
			this.textWebSchedDescript.MaxLength = 65000;
			this.textWebSchedDescript.Multiline = true;
			this.textWebSchedDescript.Name = "textWebSchedDescript";
			this.textWebSchedDescript.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textWebSchedDescript.Size = new System.Drawing.Size(366, 96);
			this.textWebSchedDescript.TabIndex = 1;
			// 
			// tabClinics
			// 
			this.tabClinics.BackColor = System.Drawing.SystemColors.Control;
			this.tabClinics.Controls.Add(this.labelClinicsDesc);
			this.tabClinics.Controls.Add(this.checkAllClinics);
			this.tabClinics.Controls.Add(this.listBoxClinics);
			this.tabClinics.Controls.Add(this.labelClinics);
			this.tabClinics.Location = new System.Drawing.Point(4, 22);
			this.tabClinics.Name = "tabClinics";
			this.tabClinics.Padding = new System.Windows.Forms.Padding(3);
			this.tabClinics.Size = new System.Drawing.Size(862, 578);
			this.tabClinics.TabIndex = 4;
			this.tabClinics.Text = "Clinics";
			// 
			// labelClinicsDesc
			// 
			this.labelClinicsDesc.Location = new System.Drawing.Point(12, 9);
			this.labelClinicsDesc.Name = "labelClinicsDesc";
			this.labelClinicsDesc.Size = new System.Drawing.Size(504, 45);
			this.labelClinicsDesc.TabIndex = 48;
			this.labelClinicsDesc.Text = resources.GetString("labelClinicsDesc.Text");
			this.labelClinicsDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkAllClinics
			// 
			this.checkAllClinics.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllClinics.Location = new System.Drawing.Point(12, 72);
			this.checkAllClinics.Name = "checkAllClinics";
			this.checkAllClinics.Size = new System.Drawing.Size(154, 16);
			this.checkAllClinics.TabIndex = 45;
			this.checkAllClinics.Text = "All";
			this.checkAllClinics.CheckedChanged += new System.EventHandler(this.checkAllClinics_CheckedChanged);
			// 
			// listBoxClinics
			// 
			this.listBoxClinics.Location = new System.Drawing.Point(12, 91);
			this.listBoxClinics.Name = "listBoxClinics";
			this.listBoxClinics.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxClinics.Size = new System.Drawing.Size(154, 186);
			this.listBoxClinics.TabIndex = 46;
			this.listBoxClinics.SelectedIndexChanged += new System.EventHandler(this.listBoxClinics_SelectedIndexChanged);
			// 
			// labelClinics
			// 
			this.labelClinics.Location = new System.Drawing.Point(12, 54);
			this.labelClinics.Name = "labelClinics";
			this.labelClinics.Size = new System.Drawing.Size(154, 16);
			this.labelClinics.TabIndex = 47;
			this.labelClinics.Text = "Clinics";
			this.labelClinics.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// odColorPickerAppt
			// 
			this.odColorPickerAppt.BackgroundColor = System.Drawing.Color.Empty;
			this.odColorPickerAppt.Location = new System.Drawing.Point(201, 454);
			this.odColorPickerAppt.Name = "odColorPickerAppt";
			this.odColorPickerAppt.Size = new System.Drawing.Size(74, 21);
			this.odColorPickerAppt.TabIndex = 342;
			// 
			// odColorPickerOutline
			// 
			this.odColorPickerOutline.BackgroundColor = System.Drawing.Color.Empty;
			this.odColorPickerOutline.Location = new System.Drawing.Point(201, 475);
			this.odColorPickerOutline.Name = "odColorPickerOutline";
			this.odColorPickerOutline.Size = new System.Drawing.Size(74, 21);
			this.odColorPickerOutline.TabIndex = 343;
			// 
			// FormProvEdit
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(895, 664);
			this.Controls.Add(this.tabControlProvider);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(827, 666);
			this.Name = "FormProvEdit";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Edit Provider";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProvEdit_Closing);
			this.Load += new System.EventHandler(this.FormProvEdit_Load);
			this.groupBox2.ResumeLayout(false);
			this.tabControlProvider.ResumeLayout(false);
			this.tabGeneral.ResumeLayout(false);
			this.tabGeneral.PerformLayout();
			this.groupClinicOverrides.ResumeLayout(false);
			this.groupClinicOverrides.PerformLayout();
			this.groupAnesthProvType.ResumeLayout(false);
			this.groupAnesthProvType.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabSupplementalIDs.ResumeLayout(false);
			this.tabDentalSchools.ResumeLayout(false);
			this.tabDentalSchools.PerformLayout();
			this.tabWebSched.ResumeLayout(false);
			this.tabWebSched.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureWebSched)).EndInit();
			this.tabClinics.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormProvEdit_Load(object sender, System.EventArgs e) {
			//if(IsNew){
			//	Providers.Cur.SigOnFile=true;
			//	Providers.InsertCur();
				//one field handled from previous form
			//}
			comboEhrMu.Items.Add("Use Global");
			comboEhrMu.Items.Add("Stage 1");
			comboEhrMu.Items.Add("Stage 2");
			comboEhrMu.Items.Add("Modified Stage 2");
			comboEhrMu.SelectedIndex=ProvCur.EhrMuStage;
			if(!PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				comboEhrMu.Visible=false;
				labelEhrMU.Visible=false;
			}
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools) //Dental Schools is turned on
				&& (ProvCur.SchoolClassNum!=0 || ProvCur.IsInstructor))//Adding/Editing Students or Instructors
			{
				if(!ProvCur.IsNew) {
					labelPassDescription.Visible=true;
					textProvNum.Text=ProvCur.ProvNum.ToString();
					List<Userod> userList=Providers.GetAttachedUsers(ProvCur.ProvNum);
					if(userList.Count>0) {
						textUserName.Text=userList[0].UserName;//Should always happen if they are a student.
						_existingUser=userList[0];
					}
				}
				else {
					textUserName.Text=Providers.GetNextAvailableProvNum().ToString();//User-names are suggested to be the ProvNum of the provider.  This can be changed at will.
				}
				_listSchoolClasses=SchoolClasses.GetDeepCopy();
				for(int i=0;i<_listSchoolClasses.Count;i++) {
					comboSchoolClass.Items.Add(SchoolClasses.GetDescript(_listSchoolClasses[i]));
					comboSchoolClass.SelectedIndex=0;
					if(_listSchoolClasses[i].SchoolClassNum==ProvCur.SchoolClassNum) {
						comboSchoolClass.SelectedIndex=i;
					}
				}
				if(ProvCur.SchoolClassNum!=0) {
					labelSchoolClass.Visible=true;
					comboSchoolClass.Visible=true;
				}
			}
			else {
				tabControlProvider.TabPages.Remove(tabDentalSchools);
			}
			if(Programs.IsEnabled(ProgramName.eClinicalWorks)) {
				textEcwID.Text=ProvCur.EcwID;
			}
			else{
				labelEcwID.Visible=false;
				textEcwID.Visible=false;
			}
			List<EhrProvKey> listProvKey=EhrProvKeys.GetKeysByFLName(ProvCur.LName,ProvCur.FName);
			if(listProvKey.Count>0) {
				textLName.Enabled=false;
				textFName.Enabled=false;
			}
			else{
				textLName.Enabled=true;
				textFName.Enabled=true;
			}
			//We'll just always show the Anesthesia fields since they are part of the standard database.
			if(ProvCur.ProvNum!=0) {
				textProviderID.Text=ProvCur.ProvNum.ToString();
			}
			textAbbr.Text=ProvCur.Abbr;
			textLName.Text=ProvCur.LName;
			textFName.Text=ProvCur.FName;
			textMI.Text=ProvCur.MI;
			textSuffix.Text=ProvCur.Suffix;
			textSSN.Text=ProvCur.SSN;
			dateTerm.SetDateTime(ProvCur.DateTerm);
			if(ProvCur.UsingTIN){
				radioTIN.Checked=true;
			}
			else {
				radioSSN.Checked=true;
			}
			_listProvClinicsOld=ProviderClinics.GetListForProvider(ProvCur.ProvNum,
				Clinics.GetForUserod(Security.CurUser,true).Select(x => x.ClinicNum)
				.Union(new List<long>() { 0 })//Always include 0 clinic, this is the default, NOT a headquarters only value.
				.Distinct()
				.ToList());
			_listProvClinicsNew=_listProvClinicsOld.Select(x => x.Copy()).ToList();
			_provClinicDefault=_listProvClinicsNew.Find(x => x.ClinicNum==0);
			//Doesn't exist in Db, create a new one.
			if(_provClinicDefault==null) {
				_provClinicDefault=new ProviderClinic {
					ProvNum=ProvCur.ProvNum,
					ClinicNum=0,
					DEANum=ProvCur.DEANum,
					StateLicense=ProvCur.StateLicense,
					StateRxID=ProvCur.StateRxID,
					StateWhereLicensed=ProvCur.StateWhereLicensed,
				};
				_listProvClinicsNew.Add(_provClinicDefault);
			}
			textDEANum.Text=_provClinicDefault.DEANum;
			textStateLicense.Text=_provClinicDefault.StateLicense;
			textStateWhereLicensed.Text=_provClinicDefault.StateWhereLicensed;
			textStateRxID.Text=_provClinicDefault.StateRxID;
			//textBlueCrossID.Text=ProvCur.BlueCrossID;
			textMedicaidID.Text=ProvCur.MedicaidID;
			textNationalProvID.Text=ProvCur.NationalProvID;
			textCanadianOfficeNum.Text=ProvCur.CanadianOfficeNum;
			textCustomID.Text=ProvCur.CustomID;
			textSchedRules.Text=ProvCur.SchedNote;
			checkIsSecondary.Checked=ProvCur.IsSecondary;
			checkSigOnFile.Checked=ProvCur.SigOnFile;
			checkIsHidden.Checked=ProvCur.IsHidden;
			checkIsInstructor.Checked=ProvCur.IsInstructor;
			odColorPickerAppt.AllowTransparentColor=true;
			odColorPickerOutline.AllowTransparentColor=true;
			odColorPickerAppt.BackgroundColor=ProvCur.ProvColor;
			odColorPickerOutline.BackgroundColor=ProvCur.OutlineColor;
			checkIsHiddenOnReports.Checked=ProvCur.IsHiddenReport;
			checkUseErx.Checked=(ProvCur.IsErxEnabled!=ErxEnabledStatus.Disabled);
			ErxOption erxOption=Erx.GetErxOption();
			if(erxOption==ErxOption.DoseSpotWithLegacy) {
				checkAllowLegacy.Visible=true;
				checkAllowLegacy.Checked=(ProvCur.IsErxEnabled==ErxEnabledStatus.EnabledWithLegacy);
			}
			textBirthdate.Text="";
			textProdGoalHr.Text=ProvCur.HourlyProdGoalAmt.ToString("f");
			if(ProvCur.Birthdate.Year>=1880) {
				textBirthdate.Text=ProvCur.Birthdate.ToShortDateString();
			}
			_listFeeSchedShort=FeeScheds.GetDeepCopy(true);
			for(int i=0;i<_listFeeSchedShort.Count;i++){
				this.listFeeSched.Items.Add(_listFeeSchedShort[i].Description);
				if(_listFeeSchedShort[i].FeeSchedNum==ProvCur.FeeSched){
					listFeeSched.SelectedIndex=i;
				}
			}
			if(listFeeSched.SelectedIndex<0){
				listFeeSched.SelectedIndex=0;
			}
			listSpecialty.Items.Clear();
			Def[] specDefs=Defs.GetDefsForCategory(DefCat.ProviderSpecialties,true).ToArray();
			for(int i=0;i<specDefs.Length;i++) {
				listSpecialty.Items.Add(Lan.g("enumDentalSpecialty",specDefs[i].ItemName));
				if(i==0 || ProvCur.Specialty==specDefs[i].DefNum) {//default to the first item in the list
					listSpecialty.SelectedIndex=i;
				}
			}
			textTaxonomyOverride.Text=ProvCur.TaxonomyCodeOverride;
			FillProvIdent();
			//These radio buttons are used to properly filter the provider dropdowns on FormAnetheticRecord
			if (ProvCur.AnesthProvType == 0)
				{
					radNone.Checked = true;
				}
			
			if (ProvCur.AnesthProvType == 1)
				{
					radAnesthSurg.Checked = true;
				}

			if (ProvCur.AnesthProvType == 2)
			{
				radAsstCirc.Checked = true;
			}
			checkIsCDAnet.Checked=ProvCur.IsCDAnet;
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				checkIsCDAnet.Visible=true;
			}
			checkIsNotPerson.Checked=ProvCur.IsNotPerson;
			comboProv.Items.AddProvNone();
			comboProv.Items.AddProvsFull(Providers.GetDeepCopy(true));
			comboProv.SetSelectedProvNum(ProvCur.ProvNumBillingOverride);
			textWebSchedDescript.Text=ProvCur.WebSchedDescript;
			FillImage();
			if(ProvCur.ProvStatus==ProviderStatus.Deleted) {
				foreach(Control con in this.Controls) {
					con.Enabled=false;
				}
				//Make the cancel button the only thing the user can click on.
				butCancel.Enabled=true;
			}
			if(PrefC.HasClinicsEnabled) {
				_listProvClinicLinks=ProviderClinicLinks.GetForProvider(ProvCur.ProvNum);
				_listClinicsForUser=Clinics.GetForUserod(Security.CurUser);
				//If there are no ProviderClinicLinks, then the provider is associated to all clinics.
				bool doSelectAll=(_listProvClinicLinks.Count==0 || _listClinicsForUser.All(x => _listProvClinicLinks.Any(y => y.ClinicNum==x.ClinicNum)));
				listBoxClinics.SetItems(_listClinicsForUser,x => x.Abbr,x => !doSelectAll && _listProvClinicLinks.Any(y => y.ClinicNum==x.ClinicNum));
				checkAllClinics.Checked=doSelectAll;
			}
			else {
				butClinicOverrides.Visible=false;
				groupClinicOverrides.Text="";
				tabControlProvider.TabPages.Remove(tabClinics);
			}
		}

		private void FillImage() {
			if(string.IsNullOrEmpty(ProvCur.WebSchedImageLocation)) {
				return;
			}
			string fullImagePath=FileAtoZ.CombinePaths(ImageStore.GetProviderImagesFolder(),ProvCur.WebSchedImageLocation);
			try {
				pictureWebSched.Image=FileAtoZ.GetImage(fullImagePath);
			}
			catch(Exception ex) {
				Shown+=new EventHandler((o,e) => FriendlyException.Show(Lans.g(this,"Unable to display image."),ex));
			}
		}

		private void radioSSN_Click(object sender, System.EventArgs e) {
			ProvCur.UsingTIN=false;
		}

		private void radioTIN_Click(object sender, System.EventArgs e) {
			ProvCur.UsingTIN=true;
		}

		private void FillProvIdent(){
			ProviderIdents.RefreshCache();
			ListProvIdent=ProviderIdents.GetForProv(ProvCur.ProvNum);
			tbProvIdent.ResetRows(ListProvIdent.Length);
			tbProvIdent.SetGridColor(Color.Gray);
			for(int i=0;i<ListProvIdent.Length;i++){
				tbProvIdent.Cell[0,i]=ListProvIdent[i].PayorID;
				tbProvIdent.Cell[1,i]=ListProvIdent[i].SuppIDType.ToString();
				tbProvIdent.Cell[2,i]=ListProvIdent[i].IDNumber;
			}
			tbProvIdent.LayoutTables();
		}

		private void tbProvIdent_CellDoubleClicked(object sender, OpenDental.CellEventArgs e) {
			FormProviderIdentEdit FormP=new FormProviderIdentEdit();
			FormP.ProvIdentCur=ListProvIdent[e.Row];
			FormP.ShowDialog();
			FillProvIdent();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormProviderIdentEdit FormP=new FormProviderIdentEdit();
			FormP.ProvIdentCur=new ProviderIdent();
			FormP.ProvIdentCur.ProvNum=ProvCur.ProvNum;
			FormP.IsNew=true;
			FormP.ShowDialog();
			FillProvIdent();
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(tbProvIdent.SelectedRow==-1){
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Delete the selected Provider Identifier?"),"",
				MessageBoxButtons.OKCancel)!=DialogResult.OK)
			{
				return;
			}
			ProviderIdents.Delete(ListProvIdent[tbProvIdent.SelectedRow]);
			FillProvIdent();
		}

		private void butPickPict_Click(object sender,EventArgs e) {
			OpenFileDialog dlg=new OpenFileDialog();
			dlg.Multiselect=false;
			if(dlg.ShowDialog()!=DialogResult.OK) {
				return;
			}
			string localFileName=dlg.FileName;
			if(!File.Exists(localFileName)) {
				MsgBox.Show(this,"File does not exist.");
				return;
			}
			if(!ImageHelper.HasImageExtension(localFileName)) {
				MsgBox.Show(this,"Only allowed to import an image.");
				return;
			}
			string atoZFileName=FileAtoZ.CombinePaths(ImageStore.GetProviderImagesFolder(),Path.GetFileName(localFileName));
			if(FileAtoZ.Exists(atoZFileName)) {
				int attempts=1;
				string newAtoZFileName=FileAtoZ.AppendSuffix(atoZFileName,"_"+attempts);
				while(FileAtoZ.Exists(newAtoZFileName)) {
					if(attempts++ > 1000) {
						MsgBox.Show(this,"Unable to upload image.");
						return;
					}
					newAtoZFileName=FileAtoZ.AppendSuffix(atoZFileName,"_"+attempts);
				}
				atoZFileName=newAtoZFileName;
			}
			try {
				FileAtoZ.Upload(localFileName,atoZFileName);
			}
			catch(Exception ex) {
				FriendlyException.Show(Lans.g(this,"Unable to upload image."),ex);
				return;
			}
			ProvCur.WebSchedImageLocation=Path.GetFileName(atoZFileName);
			try {
				pictureWebSched.Image=Image.FromFile(localFileName);
			}
			catch(Exception ex) {
				pictureWebSched.Image=null;
				FriendlyException.Show(Lans.g(this,"Unable to display image."),ex);
			}
		}

		private void butPictureNone_Click(object sender,EventArgs e) {
			ProvCur.WebSchedImageLocation="";
			pictureWebSched.Image=null;
		}

		private void butClinicOverrides_Click(object sender,EventArgs e) {
			//Update current changes in the form before going to look at all values
			_provClinicDefault.DEANum=textDEANum.Text;
			_provClinicDefault.StateLicense=textStateLicense.Text;
			_provClinicDefault.StateRxID=textStateRxID.Text;
			_provClinicDefault.StateWhereLicensed=textStateWhereLicensed.Text;
			FormProvAdditional FormPA=new FormProvAdditional(_listProvClinicsNew,ProvCur);
			FormPA.ShowDialog();
			if(FormPA.DialogResult==DialogResult.OK) {
				_listProvClinicsNew=FormPA.ListProviderClinicOut;
				_provClinicDefault=_listProvClinicsNew.Find(x => x.ClinicNum==0);
				textDEANum.Text=_provClinicDefault.DEANum;
				textStateLicense.Text=_provClinicDefault.StateLicense;
				textStateRxID.Text=_provClinicDefault.StateRxID;
				textStateWhereLicensed.Text=_provClinicDefault.StateWhereLicensed;
			}
		}

		private void listBoxClinics_SelectedIndexChanged(object sender,EventArgs e) {
			if(checkAllClinics.Checked) {
				checkAllClinics.CheckedChanged-=checkAllClinics_CheckedChanged;
				checkAllClinics.Checked=false;
				checkAllClinics.CheckedChanged+=checkAllClinics_CheckedChanged;
			}
		}

		private void checkAllClinics_CheckedChanged(object sender,EventArgs e) {
			if(checkAllClinics.Checked) {
				listBoxClinics.SelectedIndexChanged-=listBoxClinics_SelectedIndexChanged;
				listBoxClinics.SelectedIndices.Clear();
				listBoxClinics.SelectedIndexChanged+=listBoxClinics_SelectedIndexChanged;
			}
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(!dateTerm.IsValid) {
				MsgBox.Show(this,"Term Date invalid.");
				return;
			}
			if(textAbbr.Text=="") {
				MessageBox.Show(Lan.g(this,"Abbreviation not allowed to be blank."));
				return;
			}
			if(textSSN.Text.Contains("-")) {
				MsgBox.Show(this,"SSN/TIN not allowed to have dash.");
				return;
			}
			if(checkIsHidden.Checked) {
				if(PrefC.GetLong(PrefName.PracticeDefaultProv)==ProvCur.ProvNum) {
					MsgBox.Show(this,"Not allowed to hide practice default provider.");
					return;
				}
				if(Clinics.IsDefaultClinicProvider(ProvCur.ProvNum)) {
					MsgBox.Show(this,"Not allowed to hide a clinic default provider.");
					return;
				}
				if(PrefC.GetLong(PrefName.InsBillingProv)==ProvCur.ProvNum) {
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"You are about to hide the default ins billing provider. Continue?")) {
						return;
					}
				}
				if(Clinics.IsInsBillingProvider(ProvCur.ProvNum)) {
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"You are about to hide a clinic ins billing provider. Continue?")) {
						return;
					}
				}
				List<ApptViewItem> listApptViewItems=ApptViewItems.GetForProvider(ProvCur.ProvNum);
				if(!listApptViewItems.IsNullOrEmpty()) {	
					#region Provider Attached to View Check
					//Create a list of Provider associated Appointment Views
					List<ApptView> listApptView=listApptViewItems.Select(x => ApptViews.GetApptView(x.ApptViewNum)).ToList();
					//This list must be distincted before being shown to the user. A single Provider can be associated to the same "view" multiple times.
					string listProviderAssociatedViews=string.Join("\r\n",listApptView.Select(x => x.Description).Distinct().OrderBy(x => x));
					string msg=Lans.g(this,"Not allowed to hide a Provider associated to an Appointment View.");
					msg+=("\r\n"+Lans.g(this,"To continue remove them from the following Appointment View(s):"));
					msg+=("\r\n"+listProviderAssociatedViews);
					MsgBox.Show(msg);
					return;
					#endregion
				}
			}
			if(Providers.GetExists(x => x.ProvNum!=ProvCur.ProvNum && x.Abbr==textAbbr.Text && PrefC.GetBool(PrefName.EasyHideDentalSchools))) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This abbreviation is already in use by another provider.  Continue anyway?")) {
					return;
				}
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && checkIsCDAnet.Checked) {
				if(textNationalProvID.Text!=OpenDentBusiness.Eclaims.Canadian.TidyAN(textNationalProvID.Text,9,true)) {
					MsgBox.Show(this,"CDA number must be 9 characters long and composed of numbers and letters only.");
					return;
				}
				if(textCanadianOfficeNum.Text!=OpenDentBusiness.Eclaims.Canadian.TidyAN(textCanadianOfficeNum.Text,4,true)) {
					MsgBox.Show(this,"Office number must be 4 characters long and composed of numbers and letters only.");
					return;
				}
			}
			if(checkIsNotPerson.Checked) {
				if(textFName.Text!="" || textMI.Text!="") {
					MsgBox.Show(this,"When the 'Not a Person' box is checked, the provider may not have a First Name or Middle Initial entered.");
					return;
				}
			}
			if(checkIsHidden.Checked && ProvCur.IsHidden==false) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"If there are any future hours on this provider's schedule, they will be removed.  "
					+"This does not affect scheduled appointments or any other appointments in any way.")) 
				{
					return;
				}
				Providers.RemoveProvFromFutureSchedule(ProvCur.ProvNum);
			}
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools) && (ProvCur.IsInstructor || ProvCur.SchoolClassNum!=0)) {//Is an Instructor or a Student
				if(textUserName.Text=="") {
					MsgBox.Show(this,"User Name is not allowed to be blank.");
					return;
				}
			}
			long claimBillingOverrideProvNum=comboProv.GetSelectedProvNum();
			if(claimBillingOverrideProvNum!=0) {
				Provider provClaimBillingOverride=comboProv.GetSelected<Provider>();
				if(provClaimBillingOverride==null) {//Hidden provider, need to get them the normal way
					provClaimBillingOverride=Providers.GetProv(claimBillingOverrideProvNum);//Can return null if the provider doesn't exist
				}
				if(provClaimBillingOverride!=null && !provClaimBillingOverride.IsNotPerson) {//Override is a person.
					MsgBox.Show(this,"E-claim Billing Prov Override cannot be a person.");
					return;
				}
			}
			if(ProvCur.IsNew == false && comboProv.GetSelectedProvNum()==ProvCur.ProvNum) {//Override is the same provider.
				MsgBox.Show(this,"E-claim Billing Prov Override cannot be the same provider.");
				return;
			}
			if(ProvCur.IsErxEnabled==ErxEnabledStatus.Disabled && checkUseErx.Checked) {//The user enabled eRx for this provider when it was previously disabled.
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"By clicking Yes, you acknowledge and approve Electronic Rx (eRx) fees for this "
					+"provider. See the website for more details. ERx only works for the United States and its territories. Do you want to continue?"))
				{
					return;
				}
			}
			if(textBirthdate.Text!="" && textBirthdate.errorProvider1.GetError(textBirthdate)!="") {
				MsgBox.Show(this,"Birthdate invalid.");
				return;
			}
			if(textProdGoalHr.errorProvider1.GetError(textProdGoalHr)!="") {
				MsgBox.Show(this,"Hourly production goal invalid.");
				return;
			}
			ProvCur.Abbr=textAbbr.Text;
			ProvCur.LName=textLName.Text;
			ProvCur.FName=textFName.Text;
			ProvCur.MI=textMI.Text;
			ProvCur.Suffix=textSuffix.Text;
			ProvCur.SSN=textSSN.Text;
			ProvCur.StateLicense=textStateLicense.Text;
			_provClinicDefault.StateLicense=textStateLicense.Text;
			ProvCur.StateWhereLicensed=textStateWhereLicensed.Text;
			_provClinicDefault.StateWhereLicensed=textStateWhereLicensed.Text;
			ProvCur.DEANum=textDEANum.Text;
			_provClinicDefault.DEANum=textDEANum.Text;
			ProvCur.StateRxID=textStateRxID.Text;
			_provClinicDefault.StateRxID=textStateRxID.Text;
			//ProvCur.BlueCrossID=textBlueCrossID.Text;
			ProvCur.MedicaidID=textMedicaidID.Text;
			ProvCur.NationalProvID=textNationalProvID.Text;
			ProvCur.CanadianOfficeNum=textCanadianOfficeNum.Text;
			//EhrKey and EhrHasReportAccess set when user uses the ... button
			ProvCur.IsSecondary=checkIsSecondary.Checked;
			ProvCur.SigOnFile=checkSigOnFile.Checked;
			ProvCur.IsHidden=checkIsHidden.Checked;
			ProvCur.IsCDAnet=checkIsCDAnet.Checked;
			ProvCur.ProvColor=odColorPickerAppt.BackgroundColor;
			ProvCur.OutlineColor=odColorPickerOutline.BackgroundColor;
			ProvCur.IsInstructor=checkIsInstructor.Checked;
			ProvCur.EhrMuStage=comboEhrMu.SelectedIndex;
			ProvCur.IsHiddenReport=checkIsHiddenOnReports.Checked;
			ProvCur.IsErxEnabled=checkUseErx.Checked?ErxEnabledStatus.Enabled:ErxEnabledStatus.Disabled;
			ErxOption erxOption=Erx.GetErxOption();
			//If the ErxOption is Legacy, we want to keep the EnabledStatus as EnabledWithLegacy.
			//If the office switches eRx Options we will know to prompt those providers which eRx solution to use until the office disables legacy.
			if(erxOption!=ErxOption.Legacy && checkAllowLegacy.Visible && checkAllowLegacy.Checked) {
				ProvCur.IsErxEnabled=ErxEnabledStatus.EnabledWithLegacy;
			}
			ProvCur.CustomID=textCustomID.Text;
			ProvCur.SchedNote=textSchedRules.Text;
			ProvCur.Birthdate=PIn.Date(textBirthdate.Text);
			ProvCur.WebSchedDescript=textWebSchedDescript.Text;
			ProvCur.HourlyProdGoalAmt=PIn.Double(textProdGoalHr.Text);
			ProvCur.DateTerm=dateTerm.GetDateTime();
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				if(ProvCur.SchoolClassNum!=0) {
					ProvCur.SchoolClassNum=_listSchoolClasses[comboSchoolClass.SelectedIndex].SchoolClassNum;
				}
			}
			if(listFeeSched.SelectedIndex!=-1) {
				ProvCur.FeeSched=_listFeeSchedShort[listFeeSched.SelectedIndex].FeeSchedNum;
			}
			//default to first specialty in the list if it can't find the specialty by exact name
			ProvCur.Specialty=Defs.GetByExactNameNeverZero(DefCat.ProviderSpecialties,listSpecialty.SelectedItem.ToString());//selected index defaults to 0
			ProvCur.TaxonomyCodeOverride=textTaxonomyOverride.Text;
			if(radAnesthSurg.Checked) {
				ProvCur.AnesthProvType=1;
			}
			else if(radAsstCirc.Checked) {
				ProvCur.AnesthProvType=2;
			}
			else {
				ProvCur.AnesthProvType=0;
			}
			ProvCur.IsNotPerson=checkIsNotPerson.Checked;
			ProvCur.ProvNumBillingOverride=comboProv.GetSelectedProvNum();
			if(IsNew) {
				long provNum=Providers.Insert(ProvCur);
				//Set the providerclinics to the new provider's ProvNum that was just retreived from the database.
				_listProvClinicsNew.ForEach(x => x.ProvNum=provNum);
				if(ProvCur.IsInstructor) {
					Userod user=new Userod();
					user.UserName=textUserName.Text;
					user.LoginDetails=Authentication.GenerateLoginDetailsSHA512(textPassword.Text);
					user.ProvNum=provNum;
					try {
						Userods.Insert(user,new List<long> { PrefC.GetLong(PrefName.SecurityGroupForInstructors) });
					}
					catch(Exception ex) {
						Providers.Delete(ProvCur);
						MessageBox.Show(ex.Message);
						return;
					}
				}
			}
			else {
				try {
					if(_existingUser!=null && (ProvCur.IsInstructor || ProvCur.SchoolClassNum!=0)) {
						_existingUser.UserName=textUserName.Text;
						_existingUser.LoginDetails=Authentication.GenerateLoginDetailsSHA512(textPassword.Text);
						Userods.Update(_existingUser);
					}
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
					return;
				}
				Providers.Update(ProvCur);
				#region Date Term Check
				if(ProvCur.DateTerm.Year > 1880 && ProvCur.DateTerm < DateTime.Now) {
					List<ClaimPaySplit> listClaimPaySplits=Claims.GetOutstandingClaimsByProvider(ProvCur.ProvNum,ProvCur.DateTerm);
					StringBuilder claimMessage=new StringBuilder(Lan.g(this,"Clinic\tPatNum\tPatient Name\tDate of Service\tClaim Status\tFee\tCarrier")+"\r\n");
					foreach(ClaimPaySplit claimPaySplit in listClaimPaySplits) {
						claimMessage.Append(claimPaySplit.ClinicDesc+"\t"
							+POut.Long(claimPaySplit.PatNum)+"\t"
							+claimPaySplit.PatName+"\t"
							+claimPaySplit.DateClaim.ToShortDateString()+"\t");
						switch(claimPaySplit.ClaimStatus) {
							case "W":
								claimMessage.Append("Waiting in Queue\t");
								break;
							case "H":
								claimMessage.Append("Hold\t");
								break;
							case "U":
								claimMessage.Append("Unsent\t");
								break;
							case "S":
								claimMessage.Append("Sent\t");
								break;
							default:
								break;
						}
						claimMessage.AppendLine(claimPaySplit.FeeBilled+"\t"+claimPaySplit.Carrier);
					}
					MsgBoxCopyPaste msg=new MsgBoxCopyPaste(claimMessage.ToString());
					msg.Text=Lan.g(this,"Outstanding Claims for the Provider Whose Term Has Expired");
					if(listClaimPaySplits.Count > 0) {
						msg.ShowDialog();
					}
				}
				#endregion Date Term Check
			}
			ProviderClinics.Sync(_listProvClinicsNew,_listProvClinicsOld);
			if(PrefC.HasClinicsEnabled) {
				List<long> listSelectedClinicNumLinks=listBoxClinics.GetListSelected<Clinic>().Select(x => x.ClinicNum).ToList();
				List<Clinic> listClinicsAll=Clinics.GetDeepCopy(true);
				List<long> listClinicNumsForUser=_listClinicsForUser.Select(x => x.ClinicNum).ToList();
				bool canUserAccessAllClinics=(_listClinicsForUser.Count==listClinicsAll.Count);
				if(checkAllClinics.Checked) {
					if(canUserAccessAllClinics) {
						listSelectedClinicNumLinks.Clear();//No clinic links means the provider is associated to all clinics
					}
					else {
						listSelectedClinicNumLinks=_listClinicsForUser.Select(x => x.ClinicNum).ToList();
					}
				}
				else {//'All' is not checked
					if(listSelectedClinicNumLinks.Count==0 
						&& !_listProvClinicLinks.Any(x => x.ClinicNum > -1 && !x.ClinicNum.In(listClinicNumsForUser))) 
					{
						//The user wants to assign this provider to no clinics.
						listSelectedClinicNumLinks.Add(-1);//Since no clinic links means the provider is associated to all clinics, we're gonna use -1.
					}
					else if(!canUserAccessAllClinics && _listProvClinicLinks.Count==0) {
						//The provider previously was associated to all clinics. We need to add in the clinics this user does not have access to.
						listSelectedClinicNumLinks.AddRange(listClinicsAll.Where(x => !x.ClinicNum.In(listClinicNumsForUser)).Select(x => x.ClinicNum));
					}
				}
				List<ProviderClinicLink> listProvClinicLinksNew=_listProvClinicLinks.Select(x => x.Copy()).ToList();
				listProvClinicLinksNew.RemoveAll(x => x.ClinicNum.In(listClinicNumsForUser) || x.ClinicNum==-1);
				listProvClinicLinksNew.AddRange(listSelectedClinicNumLinks.Select(x => new ProviderClinicLink(x,ProvCur.ProvNum)));
				if(ProviderClinicLinks.Sync(listProvClinicLinksNew,_listProvClinicLinks)) {
					DataValid.SetInvalid(InvalidType.ProviderClinicLink);
				}
			}
			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormProvEdit_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(DialogResult==DialogResult.OK) {
				//There can be a "hasChanged" boolean added in the future.  For now, due to FormProviderSetup, we need to refresh the cache just in case.
				DataValid.SetInvalid(InvalidType.Providers);
				return;
			}
			if(IsNew){
				//UserPermissions.DeleteAllForProv(Providers.Cur.ProvNum);
				ProviderIdents.DeleteAllForProv(ProvCur.ProvNum);
				Providers.Delete(ProvCur);
			}
		}
	}
}




