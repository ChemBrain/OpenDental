using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using OpenDentBusiness.Eclaims;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormClaimFormItemEdit : ODForm,IFormClaimFormItemEdit {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelImageFileName;
		private System.Windows.Forms.TextBox textImageFileName;
		private System.Windows.Forms.Label labelFieldName;
		private System.Windows.Forms.ListBox listFieldName;
		private System.Windows.Forms.TextBox textFormatString;
		private System.Windows.Forms.Label labelFormatString;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public string[] FieldNames { get; set; }
		private OpenDental.UI.Button butDelete;
		///<summary></summary>
		public bool IsNew;
		///<summary>This is the claimformitem that is being currently edited in this window.</summary>
		public ClaimFormItem CFIcur;
		///<summary>Set to true if the Delete button was clicked.</summary>
		public bool IsDeleted;

		///<summary></summary>
		public FormClaimFormItemEdit()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClaimFormItemEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.labelImageFileName = new System.Windows.Forms.Label();
			this.textImageFileName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.labelFieldName = new System.Windows.Forms.Label();
			this.listFieldName = new System.Windows.Forms.ListBox();
			this.textFormatString = new System.Windows.Forms.TextBox();
			this.labelFormatString = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(852, 607);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(852, 576);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// labelImageFileName
			// 
			this.labelImageFileName.Location = new System.Drawing.Point(12, 12);
			this.labelImageFileName.Name = "labelImageFileName";
			this.labelImageFileName.Size = new System.Drawing.Size(236, 17);
			this.labelImageFileName.TabIndex = 2;
			this.labelImageFileName.Text = "Image File Name";
			this.labelImageFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textImageFileName
			// 
			this.textImageFileName.Location = new System.Drawing.Point(12, 30);
			this.textImageFileName.Name = "textImageFileName";
			this.textImageFileName.Size = new System.Drawing.Size(236, 20);
			this.textImageFileName.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 54);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(236, 45);
			this.label2.TabIndex = 4;
			this.label2.Text = "This file must be present in the A to Z folder.  \r\nIt should be a gif, jpg, or em" +
    "f.";
			// 
			// labelFieldName
			// 
			this.labelFieldName.Location = new System.Drawing.Point(254, 12);
			this.labelFieldName.Name = "labelFieldName";
			this.labelFieldName.Size = new System.Drawing.Size(156, 17);
			this.labelFieldName.TabIndex = 5;
			this.labelFieldName.Text = "or Field Name";
			this.labelFieldName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// listFieldName
			// 
			this.listFieldName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listFieldName.Location = new System.Drawing.Point(254, 30);
			this.listFieldName.MultiColumn = true;
			this.listFieldName.Name = "listFieldName";
			this.listFieldName.Size = new System.Drawing.Size(592, 602);
			this.listFieldName.TabIndex = 6;
			this.listFieldName.DoubleClick += new System.EventHandler(this.listFieldName_DoubleClick);
			// 
			// textFormatString
			// 
			this.textFormatString.Location = new System.Drawing.Point(12, 161);
			this.textFormatString.Name = "textFormatString";
			this.textFormatString.Size = new System.Drawing.Size(236, 20);
			this.textFormatString.TabIndex = 8;
			// 
			// labelFormatString
			// 
			this.labelFormatString.Location = new System.Drawing.Point(12, 106);
			this.labelFormatString.Name = "labelFormatString";
			this.labelFormatString.Size = new System.Drawing.Size(236, 52);
			this.labelFormatString.TabIndex = 7;
			this.labelFormatString.Text = "Format String.  All dates must have a format.  Valid entries would include MM/dd/" +
    "yyyy, MM-dd-yy, and M d y as examples.";
			this.labelFormatString.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Location = new System.Drawing.Point(12, 607);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(80, 25);
			this.butDelete.TabIndex = 9;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// FormClaimFormItemEdit
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(939, 644);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textFormatString);
			this.Controls.Add(this.textImageFileName);
			this.Controls.Add(this.labelFormatString);
			this.Controls.Add(this.listFieldName);
			this.Controls.Add(this.labelFieldName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.labelImageFileName);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(418, 261);
			this.Name = "FormClaimFormItemEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Claim Form Item";
			this.Load += new System.EventHandler(this.FormClaimFormItemEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormClaimFormItemEdit_Load(object sender, System.EventArgs e) {
			FillFieldNames();
			FillForm();
		}

		///<summary>This is called externally from Renaissance to error check each of the supplied fieldNames</summary>
		public void FillFieldNames(){
			FieldNames=new string[]
			{
				"FixedText",
				"IsPreAuth",
				"IsStandardClaim",
				"ShowPreauthorizationIfPreauth",
				"IsMedicaidClaim",
				"IsGroupHealthPlan",
				"PreAuthString",
				"PriorAuthString",
				"PriInsCarrierName",
				"PriInsAddress",
				"PriInsAddress2",
				"PriInsAddressComplete",
				"PriInsCity",
				"PriInsST",
				"PriInsZip",
				"OtherInsExists",
				"OtherInsNotExists",
				"OtherInsExistsDent",
				"OtherInsExistsMed",
				"OtherInsSubscrLastFirst",
				"OtherInsSubscrDOB",
				"OtherInsSubscrIsMale",
				"OtherInsSubscrIsFemale",
				"OtherInsSubscrIsGenderUnknown",
				"OtherInsSubscrGender",
				"OtherInsSubscrID",
				"OtherInsGroupNum",
				"OtherInsRelatIsSelf",
				"OtherInsRelatIsSpouse",
				"OtherInsRelatIsChild",
				"OtherInsRelatIsOther",
				"OtherInsCarrierName",
				"OtherInsAddress",
				"OtherInsCity",
				"OtherInsST",
				"OtherInsZip",
				"SubscrLastFirst",
				"SubscrAddress",
				"SubscrAddress2",
				"SubscrAddressComplete",
				"SubscrCity",
				"SubscrST",
				"SubscrZip",
				"SubscrPhone",
				"SubscrDOB",
				"SubscrIsMale",
				"SubscrIsFemale",
				"SubscrIsGenderUnknown",
				"SubscrIsMarried",
				"SubscrIsSingle",
				"SubscrID",
				"SubscrIDStrict",
				"SubscrIsFTStudent",
				"SubscrIsPTStudent",
				"SubscrGender",
				"GroupNum",
				"GroupName",
				"DivisionNo",
				"EmployerName",
				"RelatIsSelf",
				"RelatIsSpouse",
				"RelatIsChild",
				"RelatIsOther",
				"Relationship",
				"IsFTStudent",
				"IsPTStudent",
				"IsStudent",
				"CollegeName",
				"PatientLastFirst",
				"PatientLastFirstMiCommas",//Medical required format for UB04 printed claims
				"PatientFirstMiddleLast",
				"PatientFirstName",
				"PatientMiddleName",
				"PatientLastName",
				"PatientAddress",
				"PatientAddress2",
				"PatientAddressComplete",
				"PatientCity",
				"PatientST",
				"PatientZip",
				"PatientPhone",
				"PatientDOB",
				"PatientIsMale",
				"PatientIsFemale",
				"PatientIsGenderUnknown",
				"PatientGender",
				"PatientGenderLetter",
				"PatientIsMarried",
				"PatientIsSingle",
				"PatIDFromPatPlan",//Dependant Code in Canada
				"PatientSSN",
				"PatientMedicaidID",
				"PatientID-MedicaidOrSSN",
				"PatientPatNum",
				"PatientChartNum",
				"TotalFee",
				"Remarks",
				"PatientRelease",
				"PatientReleaseDate",
				"PatientAssignment",
				"PatientAssignmentDate",
				"PlaceIsOffice",
				"PlaceIsHospADA2002",
				"PlaceIsExtCareFacilityADA2002",
				"PlaceIsOtherADA2002",
				"PlaceIsInpatHosp",
				"PlaceIsOutpatHosp",
				"PlaceIsAdultLivCareFac",
				"PlaceIsSkilledNursFac",
				"PlaceIsPatientsHome",
				"PlaceIsOtherLocation",
				"PlaceNumericCode",
				"IsRadiographsAttached",
				"RadiographsNumAttached",
				"RadiographsNotAttached",
				"IsEnclosuresAttached",
				"AttachedImagesNum",
				"AttachedModelsNum",
				"IsNotOrtho",
				"IsOrtho",
				"DateOrthoPlaced",
				"MonthsOrthoRemaining",
				"MonthsOrthoTotal",
				"IsNotProsth",
				"IsInitialProsth",
				"IsNotReplacementProsth",
				"IsReplacementProsth",
				"DatePriorProsthPlaced",
				"IsOccupational",
				"IsNotOccupational",
				"IsAutoAccident",
				"IsNotAutoAccident",
				"IsOtherAccident",
				"IsNotOtherAccident",
				"IsNotAccident",
				"IsAccident",
				"AccidentDate",
				"AccidentST",
				"BillingDentist",
				"BillingDentistMedicaidID",
				"BillingDentistProviderID",
				"BillingDentistNPI",
				"BillingDentistLicenseNum",
				"BillingDentistSSNorTIN",
				"BillingDentistNumIsSSN",
				"BillingDentistNumIsTIN",
				"BillingDentistPh123",
				"BillingDentistPh456",
				"BillingDentistPh78910",
				"BillingDentistPhoneFormatted",
				"BillingDentistPhoneRaw",
				"PayToDentistAddress",
				"PayToDentistAddress2",
				"PayToDentistCity",
				"PayToDentistST",
				"PayToDentistZip",
				"TreatingDentist",
				"TreatingDentistFName",
				"TreatingDentistLName",
				"TreatingDentistSignature",
				"TreatingDentistSigDate",
				"TreatingDentistMedicaidID",
				"TreatingDentistProviderID",
				"TreatingDentistNPI",
				"TreatingDentistLicense",
				"TreatingDentistAddress",
				"TreatingDentistAddress2",
				"TreatingDentistCity",
				"TreatingDentistST",
				"TreatingDentistZip",
				"TreatingDentistPh123",
				"TreatingDentistPh456",
				"TreatingDentistPh78910",
				"TreatingDentistPhoneRaw",
				"TreatingProviderSpecialty",
				"ReferringProvNPI",
				"ReferringProvNameFL",
				"DateService",
				"TotalPages",
				"MedInsCrossoverIndicator",
				"MedInsAName",
				"MedInsAPlanID",
				"MedInsARelInfo",
				"MedInsAAssignBen",
				"MedInsAPriorPmt",
				"MedInsAAmtDue",
				"MedInsAOtherProvID",
				"MedInsAInsuredName",
				"MedInsARelation",
				"MedInsAInsuredID",
				"MedInsAGroupName",
				"MedInsAGroupNum",
				"MedInsAAuthCode",
				"MedInsAEmployer",
				"MedInsBName",
				"MedInsBPlanID",
				"MedInsBRelInfo",
				"MedInsBAssignBen",
				"MedInsBPriorPmt",
				"MedInsBAmtDue",
				"MedInsBOtherProvID",
				"MedInsBInsuredName",
				"MedInsBRelation",
				"MedInsBInsuredID",
				"MedInsBGroupName",
				"MedInsBGroupNum",
				"MedInsBAuthCode",
				"MedInsBEmployer",
				"MedInsCName",
				"MedInsCPlanID",
				"MedInsCRelInfo",
				"MedInsCAssignBen",
				"MedInsCPriorPmt",
				"MedInsCAmtDue",
				"MedInsCOtherProvID",
				"MedInsCInsuredName",
				"MedInsCRelation",
				"MedInsCInsuredID",
				"MedInsCGroupName",
				"MedInsCGroupNum",
				"MedInsCAuthCode",
				"MedInsCEmployer",
				"MedUniformBillType",
				"MedAdmissionTypeCode",
				"MedAdmissionSourceCode",
				"MedPatientStatusCode",
				"MedAccidentCode",
				"ICDindicator",
				"AcceptAssignmentY",
				"AcceptAssignmentN",
				"ClaimIdentifier",
				"OrigRefNum",
				"CorrectionType",
				"DateIllnessInjuryPreg",
				"DateIllnessInjuryPregQualifier",
				"DateOther",
				"DateOtherQualifier",
				"IsOutsideLab",
				"IsNotOutsideLab"
			};
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {
				FieldNames=FieldNames.Concat("OfficeNumber".SingleItemToList()).ToArray();
			}
			FieldNames=FieldNames.Concat(Enumerable.Range(1,32).Select(x => "Miss"+x))//Miss1-32
			.Concat(Enumerable.Range(18,11).Select(x => "MedConditionCode"+x))//MedConditionCode18-28
			.Concat(Enumerable.Range(39,3).SelectMany(x => Enumerable.Range(97,4)
					.SelectMany(y => new[] { "MedValCode"+x+(char)y,"MedValAmount"+x+(char)y })))//MedValCode39a-41d and MedValAmount39a-41d
			.Concat(Enumerable.Range(1,16).Select(x => "Diagnosis"+(x<5?x.ToString():((char)(x+60)).ToString())))//Diagnosis1-4 and DiagnosisA-L
			.OrderBy(x => x)//alphabatize the list before concatenating with the procedure (P1-P15...) fields.
			.Concat(Enumerable.Range(1,15).SelectMany(x => new[] {
				"Area",
				"Code",
				"CodeAndMods",
				"CodeMod1",
				"CodeMod2",
				"CodeMod3",
				"CodeMod4",
				"Date",
				"Description",
				"Diagnosis",
				"DiagnosisPoint",
				"eClaimNote",
				"Fee",
				"FeeMinusLab",
				"IsEmergency",
				"Lab",
				"Minutes",
				"PlaceNumericCode",
				"RevCode",
				"Surface",
				"System",
				"SystemAndTeeth",
				"ToothNumber",
				"ToothNumOrArea",
				"TreatDentMedicaidID",
				"TreatProvNPI",
				"UnitQty",
				"UnitQtyOrCount"
			}.Select(y => "P"+x+y)))//P1-15SystemAndTeeth..., 28 fields, these will be alphabatized at the end of the list of all fields
			.ToArray();
		}

		private void FillForm(){
			textImageFileName.Text=CFIcur.ImageFileName;
			textFormatString.Text=CFIcur.FormatString;
			listFieldName.Items.Clear();
			for(int i=0;i<FieldNames.Length;i++){
				listFieldName.Items.Add(FieldNames[i]);
				if(FieldNames[i]==CFIcur.FieldName){
					listFieldName.SelectedIndex=i;
				}
			}
			listFieldName.ColumnWidth=FieldNames.Max(x => TextRenderer.MeasureText(x,listFieldName.Font).Width);
		}

		private void listFieldName_DoubleClick(object sender, System.EventArgs e) {
			SaveAndClose();
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			IsDeleted=true;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			SaveAndClose();
		}

		private void SaveAndClose(){
			CFIcur.ImageFileName=textImageFileName.Text;
			CFIcur.FormatString=textFormatString.Text;
			if(listFieldName.SelectedIndex==-1){
				CFIcur.FieldName="";
			}
			else{
				CFIcur.FieldName=FieldNames[listFieldName.SelectedIndex];
			}
			if(IsNew)
				ClaimFormItems.Insert(CFIcur);
			else
				ClaimFormItems.Update(CFIcur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		


	}
}





















