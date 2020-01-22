using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.IO;
using CodeBase;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormMedications : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsSelectionMode;
		private OpenDental.UI.Button butAddGeneric;
		private OpenDental.UI.Button butAddBrand;
		private OpenDental.UI.ODGrid gridAllMedications;
		public TextBox textSearch;
		private Label label1;
		///<summary>the number returned if using select mode.</summary>
		public long SelectedMedicationNum;
		private TabControl tabMedications;
		private TabPage tabAllMedications;
		private TabPage tabMissing;
		private ODGrid gridMissing;
		private UI.Button butConvertBrand;
		private UI.Button butImportMedications;
		private UI.Button butExportMedications;
		private UI.Button butConvertGeneric;

		///<summary>Set isAll to true to start in the All Medications tab, or false to start in the Meds In Use tab.</summary>
		public FormMedications() {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMedications));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butAddGeneric = new OpenDental.UI.Button();
			this.butAddBrand = new OpenDental.UI.Button();
			this.gridAllMedications = new OpenDental.UI.ODGrid();
			this.textSearch = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabMedications = new System.Windows.Forms.TabControl();
			this.tabAllMedications = new System.Windows.Forms.TabPage();
			this.tabMissing = new System.Windows.Forms.TabPage();
			this.butConvertBrand = new OpenDental.UI.Button();
			this.butConvertGeneric = new OpenDental.UI.Button();
			this.gridMissing = new OpenDental.UI.ODGrid();
			this.butImportMedications = new OpenDental.UI.Button();
			this.butExportMedications = new OpenDental.UI.Button();
			this.tabMedications.SuspendLayout();
			this.tabAllMedications.SuspendLayout();
			this.tabMissing.SuspendLayout();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(858, 635);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(777, 635);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butAddGeneric
			// 
			this.butAddGeneric.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddGeneric.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddGeneric.Location = new System.Drawing.Point(6, 6);
			this.butAddGeneric.Name = "butAddGeneric";
			this.butAddGeneric.Size = new System.Drawing.Size(113, 26);
			this.butAddGeneric.TabIndex = 33;
			this.butAddGeneric.Text = "Add Generic";
			this.butAddGeneric.Click += new System.EventHandler(this.butAddGeneric_Click);
			// 
			// butAddBrand
			// 
			this.butAddBrand.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddBrand.Location = new System.Drawing.Point(125, 6);
			this.butAddBrand.Name = "butAddBrand";
			this.butAddBrand.Size = new System.Drawing.Size(113, 26);
			this.butAddBrand.TabIndex = 34;
			this.butAddBrand.Text = "Add Brand";
			this.butAddBrand.Click += new System.EventHandler(this.butAddBrand_Click);
			// 
			// gridAllMedications
			// 
			this.gridAllMedications.AllowSortingByColumn = true;
			this.gridAllMedications.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAllMedications.Location = new System.Drawing.Point(5, 38);
			this.gridAllMedications.Name = "gridAllMedications";
			this.gridAllMedications.Size = new System.Drawing.Size(907, 558);
			this.gridAllMedications.TabIndex = 37;
			this.gridAllMedications.Title = "All Medications";
			this.gridAllMedications.TranslationName = "FormMedications";
			this.gridAllMedications.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAllMedications_CellDoubleClick);
			this.gridAllMedications.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAllMedications_CellClick);
			// 
			// textSearch
			// 
			this.textSearch.Location = new System.Drawing.Point(367, 9);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(195, 20);
			this.textSearch.TabIndex = 0;
			this.textSearch.TextChanged += new System.EventHandler(this.textSearch_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(239, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(127, 17);
			this.label1.TabIndex = 39;
			this.label1.Text = "Search";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tabMedications
			// 
			this.tabMedications.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabMedications.Controls.Add(this.tabAllMedications);
			this.tabMedications.Controls.Add(this.tabMissing);
			this.tabMedications.Location = new System.Drawing.Point(9, 3);
			this.tabMedications.Name = "tabMedications";
			this.tabMedications.SelectedIndex = 0;
			this.tabMedications.Size = new System.Drawing.Size(924, 626);
			this.tabMedications.TabIndex = 40;
			this.tabMedications.SelectedIndexChanged += new System.EventHandler(this.tabMedications_SelectedIndexChanged);
			// 
			// tabAllMedications
			// 
			this.tabAllMedications.Controls.Add(this.butExportMedications);
			this.tabAllMedications.Controls.Add(this.butImportMedications);
			this.tabAllMedications.Controls.Add(this.gridAllMedications);
			this.tabAllMedications.Controls.Add(this.textSearch);
			this.tabAllMedications.Controls.Add(this.butAddGeneric);
			this.tabAllMedications.Controls.Add(this.label1);
			this.tabAllMedications.Controls.Add(this.butAddBrand);
			this.tabAllMedications.Location = new System.Drawing.Point(4, 22);
			this.tabAllMedications.Name = "tabAllMedications";
			this.tabAllMedications.Padding = new System.Windows.Forms.Padding(3);
			this.tabAllMedications.Size = new System.Drawing.Size(916, 600);
			this.tabAllMedications.TabIndex = 0;
			this.tabAllMedications.Text = "All Medications";
			this.tabAllMedications.UseVisualStyleBackColor = true;
			// 
			// butExportMedications
			// 
			this.butExportMedications.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butExportMedications.Location = new System.Drawing.Point(825, 6);
			this.butExportMedications.Name = "butExportMedications";
			this.butExportMedications.Size = new System.Drawing.Size(85, 26);
			this.butExportMedications.TabIndex = 42;
			this.butExportMedications.Text = "Export";
			this.butExportMedications.Click += new System.EventHandler(this.butExportMedications_Click);
			// 
			// butImportMedications
			// 
			this.butImportMedications.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butImportMedications.Location = new System.Drawing.Point(734, 6);
			this.butImportMedications.Name = "butImportMedications";
			this.butImportMedications.Size = new System.Drawing.Size(85, 26);
			this.butImportMedications.TabIndex = 41;
			this.butImportMedications.Text = "Import";
			this.butImportMedications.Click += new System.EventHandler(this.butImportMedications_Click);
			// 
			// tabMissing
			// 
			this.tabMissing.Controls.Add(this.butConvertBrand);
			this.tabMissing.Controls.Add(this.butConvertGeneric);
			this.tabMissing.Controls.Add(this.gridMissing);
			this.tabMissing.Location = new System.Drawing.Point(4, 22);
			this.tabMissing.Name = "tabMissing";
			this.tabMissing.Size = new System.Drawing.Size(916, 600);
			this.tabMissing.TabIndex = 2;
			this.tabMissing.Text = "Missing Generic/Brand";
			this.tabMissing.UseVisualStyleBackColor = true;
			// 
			// butConvertBrand
			// 
			this.butConvertBrand.Image = global::OpenDental.Properties.Resources.Add;
			this.butConvertBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butConvertBrand.Location = new System.Drawing.Point(161, 6);
			this.butConvertBrand.Name = "butConvertBrand";
			this.butConvertBrand.Size = new System.Drawing.Size(150, 26);
			this.butConvertBrand.TabIndex = 40;
			this.butConvertBrand.Text = "Convert To Brand";
			this.butConvertBrand.Click += new System.EventHandler(this.butConvertBrand_Click);
			// 
			// butConvertGeneric
			// 
			this.butConvertGeneric.Image = global::OpenDental.Properties.Resources.Add;
			this.butConvertGeneric.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butConvertGeneric.Location = new System.Drawing.Point(5, 6);
			this.butConvertGeneric.Name = "butConvertGeneric";
			this.butConvertGeneric.Size = new System.Drawing.Size(150, 26);
			this.butConvertGeneric.TabIndex = 39;
			this.butConvertGeneric.Text = "Convert To Generic";
			this.butConvertGeneric.Click += new System.EventHandler(this.butConvertGeneric_Click);
			// 
			// gridMissing
			// 
			this.gridMissing.AllowSortingByColumn = true;
			this.gridMissing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMissing.Location = new System.Drawing.Point(5, 38);
			this.gridMissing.Name = "gridMissing";
			this.gridMissing.Size = new System.Drawing.Size(907, 559);
			this.gridMissing.TabIndex = 38;
			this.gridMissing.Title = "Medications Missing Generic or Brand";
			this.gridMissing.TranslationName = "FormMedications";
			// 
			// FormMedications
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(941, 671);
			this.Controls.Add(this.tabMedications);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(793, 432);
			this.Name = "FormMedications";
			this.ShowInTaskbar = false;
			this.Text = "Medications";
			this.Load += new System.EventHandler(this.FormMedications_Load);
			this.Shown += new System.EventHandler(this.FormMedications_Shown);
			this.tabMedications.ResumeLayout(false);
			this.tabAllMedications.ResumeLayout(false);
			this.tabAllMedications.PerformLayout();
			this.tabMissing.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormMedications_Load(object sender, System.EventArgs e) {
			if(!CultureInfo.CurrentCulture.Name.EndsWith("US")) {//Not United States
				//Medications missing generic/brand are not visible for foreigners because there will be no data available.
				//This type of data can only be created in the United States for customers using NewCrop.
				tabMedications.TabPages.Remove(tabMissing);
			}
			FillTab();
			if(IsSelectionMode){
				this.Text=Lan.g(this,"Select Medication");
			}
			else{
				butOK.Visible=false;
				butCancel.Text=Lan.g(this,"Close");
			}
		}

		///<summary>Forces cursor to start in the search textbox.</summary>
		private void FormMedications_Shown(object sender,EventArgs e) {
			textSearch.Focus();//We've previously had issues with the tabindex not working.
		}

		private void tabMedications_SelectedIndexChanged(object sender,EventArgs e) {
			FillTab();
		}

		private void FillTab() {
			if(tabMedications.SelectedIndex==0) {//All Medication
				FillGridAllMedications();
			}
			else if(tabMedications.SelectedIndex==1) {//Missing Generic/Brand
				FillGridMissing();
			}
		}

		private void FillGridAllMedications(bool shouldRetainSelection=true){
			Medication medSelected=null;
			if(shouldRetainSelection && gridAllMedications.GetSelectedIndex()!=-1) {
				medSelected=(Medication)gridAllMedications.ListGridRows[gridAllMedications.GetSelectedIndex()].Tag;
			}
			List <long> listInUseMedicationNums=Medications.GetAllInUseMedicationNums();
			int sortColIndex=gridAllMedications.SortedByColumnIdx;
			bool isSortAscending=gridAllMedications.SortedIsAscending;
			gridAllMedications.BeginUpdate();
			gridAllMedications.ListGridColumns.Clear();
			//The order of these columns is important.  See gridAllMedications_CellClick()
			GridColumn col=new GridColumn(Lan.g(this,"Drug Name"),120,GridSortingStrategy.StringCompare);
			gridAllMedications.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Generic Name"),120,GridSortingStrategy.StringCompare);
			gridAllMedications.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"InUse"),55,HorizontalAlignment.Center,GridSortingStrategy.StringCompare);
			gridAllMedications.ListGridColumns.Add(col);
			if(CultureInfo.CurrentCulture.Name.EndsWith("US")) {//United States
				col=new GridColumn(Lan.g(this,"RxNorm"),70,GridSortingStrategy.StringCompare);
				gridAllMedications.ListGridColumns.Add(col);
			}
			col=new GridColumn(Lan.g(this,"Notes for Generic"),250,GridSortingStrategy.StringCompare);
			gridAllMedications.ListGridColumns.Add(col);
			gridAllMedications.ListGridRows.Clear();
			List <Medication> listMeds=Medications.GetList(textSearch.Text);
			foreach(Medication med in listMeds) {
				GridRow row=new GridRow();
				row.Tag=med;
				if(med.MedicationNum==med.GenericNum) {//isGeneric
					row.Cells.Add(med.MedName);
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(med.MedName);
					row.Cells.Add(Medications.GetGenericName(med.GenericNum));
				}
				if(listInUseMedicationNums.Contains(med.MedicationNum)) {
					row.Cells.Add("X");//InUse
				}
				else {
					row.Cells.Add("");//InUse
				}
				if(CultureInfo.CurrentCulture.Name.EndsWith("US")) {//United States
					if(med.RxCui==0) {
						row.Cells.Add(Lan.g(this,"(select)"));
						row.Cells[row.Cells.Count-1].Bold=YN.Yes;
					}
					else {
						row.Cells.Add(med.RxCui.ToString());
					}
				}
				row.Cells.Add(med.Notes);
				gridAllMedications.ListGridRows.Add(row);
			}
			gridAllMedications.EndUpdate();
			gridAllMedications.SortForced(sortColIndex,isSortAscending);
			if(medSelected!=null) {//Will be null if nothing is selected.
				for(int i=0;i<gridAllMedications.ListGridRows.Count;i++) {
					Medication medCur=(Medication)gridAllMedications.ListGridRows[i].Tag;
					if(medCur.MedicationNum==medSelected.MedicationNum) {
						gridAllMedications.SetSelected(i,true);
						break;
					}
				}
			}
		}

		private void FillGridMissing() {
			int sortColIndex=gridMissing.SortedByColumnIdx;
			bool isSortAscending=gridMissing.SortedIsAscending;
			gridMissing.BeginUpdate();
			gridMissing.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g(this,"RxNorm"),70,GridSortingStrategy.StringCompare);
			gridMissing.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Drug Description"),0,GridSortingStrategy.StringCompare);
			gridMissing.ListGridColumns.Add(col);
			gridMissing.ListGridRows.Clear();
			List<MedicationPat> listMedPats=MedicationPats.GetAllMissingMedications();
			Dictionary <string,List<MedicationPat>> dictMissingUnique=new Dictionary<string,List<MedicationPat>>();
			foreach(MedicationPat medPat in listMedPats) {
				string key=medPat.RxCui.ToString()+" - "+medPat.MedDescript.ToLower().Trim();
				if(!dictMissingUnique.ContainsKey(key)) {
					dictMissingUnique[key]=new List<MedicationPat>();
					GridRow row=new GridRow();
					row.Tag=dictMissingUnique[key];
					if(medPat.RxCui==0) {
						row.Cells.Add("");
					}
					else {
						row.Cells.Add(medPat.RxCui.ToString());
					}
					row.Cells.Add(medPat.MedDescript);
					gridMissing.ListGridRows.Add(row);
				}
				dictMissingUnique[key].Add(medPat);
			}
			gridMissing.EndUpdate();
			gridMissing.SortForced(sortColIndex,isSortAscending);
		}

		private void butAddGeneric_Click(object sender, System.EventArgs e) {
			Medication MedicationCur=new Medication();
			Medications.Insert(MedicationCur);//so that we will have the primary key
			MedicationCur.GenericNum=MedicationCur.MedicationNum;
			FormMedicationEdit FormME=new FormMedicationEdit();
			FormME.MedicationCur=MedicationCur;
			FormME.IsNew=true;
			FormME.ShowDialog();//This window refreshes the Medication cache if the user clicked OK.
			FillTab();
		}

		private void butAddBrand_Click(object sender, System.EventArgs e) {
			if(gridAllMedications.GetSelectedIndex()==-1){
				MessageBox.Show(Lan.g(this,"You must first highlight the generic medication from the list.  If it is not already on the list, then you must add it first."));
				return;
			}
			Medication medSelected=(Medication)gridAllMedications.ListGridRows[gridAllMedications.GetSelectedIndex()].Tag;
			if(medSelected.MedicationNum!=medSelected.GenericNum){
				MessageBox.Show(Lan.g(this,"The selected medication is not generic."));
				return;
			}
			Medication MedicationCur=new Medication();
			Medications.Insert(MedicationCur);//so that we will have the primary key
			MedicationCur.GenericNum=medSelected.MedicationNum;
			FormMedicationEdit FormME=new FormMedicationEdit();
			FormME.MedicationCur=MedicationCur;
			FormME.IsNew=true;
			FormME.ShowDialog();//This window refreshes the Medication cache if the user clicked OK.
			FillTab();
		}
		
		private void butImportMedications_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			List<ODTuple<Medication,string>> listImportMeds=new List<ODTuple<Medication,string>>();
			//Leaving code here to include later when we have developed a default medications list available for download.
			//if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Click Yes to download and import default medication list.\r\nClick No to import from a file.")) {
			//	Cursor=Cursors.WaitCursor;
			//	listImportMeds=DownloadDefaultMedications();//Import from OpenDental.com
			//}
			//else {//Prompt for file.
			string fileName=GetFilenameFromUser(true);
			if(string.IsNullOrEmpty(fileName)) {
				return;
			}
			try {
				Cursor=Cursors.WaitCursor;
				listImportMeds=MedicationL.GetMedicationsFromFile(fileName);
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				string msg=Lans.g(this,"Error accessing file. Close all programs using file and try again.");
				MessageBox.Show(this,msg+"\r\n: "+ex.Message);
				return;
			}
			//}
			int countImportedMedications=MedicationL.ImportMedications(listImportMeds,Medications.GetList());
			int countDuplicateMedications=listImportMeds.Count-countImportedMedications;
			DataValid.SetInvalid(InvalidType.Medications);
			Cursor=Cursors.Default;
			MessageBox.Show(this,POut.Int(countDuplicateMedications)+" "+Lan.g(this,"duplicate medications found.")+"\r\n"
				+POut.Int(countImportedMedications)+" "+Lan.g(this,"medications imported."));
			FillTab();
		}

		///<summary>Attempts to download the default medication list from HQ.
		///If there is an exception returns an empty list after showing the user an error prompt.</summary>
		private List<ODTuple<Medication,string>> DownloadDefaultMedications() {
			List<ODTuple<Medication,string>> listMedsNew=new List<ODTuple<Medication,string>>();
			string tempFile="";
			try {
				tempFile=MedicationL.DownloadDefaultMedicationsFile();
				listMedsNew=MedicationL.GetMedicationsFromFile(tempFile,true);
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Failed to download medications.")+"\r\n"+ex.Message);
			}
			return listMedsNew;
		}

		private void butExportMedications_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			int countExportedMeds=0;
			string fileName;
			if(ODBuild.IsWeb()) {
				fileName="ExportedMedications.txt";
			}
			else {
				//Prompt for file.
				fileName=GetFilenameFromUser(false);
			}
			if(string.IsNullOrEmpty(fileName)) {
				return;
			}
			try {
				Cursor=Cursors.WaitCursor;
				countExportedMeds=MedicationL.ExportMedications(fileName,Medications.GetList());
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				string msg=Lans.g(this,"Error: ");
				MessageBox.Show(this,msg+": "+ex.Message);
			}
			Cursor=Cursors.Default;
			MessageBox.Show(this,POut.Int(countExportedMeds)+" "+Lan.g(this,"medications exported to:")+" "+fileName);
		}

		///<summary>When isImport is true, prompts users to select file and returns the full file path if OK clicked, otherwise an empty string.
		///When isImport is false (export), prompts user to select a file destination if OK clicked, otherwise an empty string..</summary>
		private string GetFilenameFromUser(bool isImport) {
			Cursor=Cursors.WaitCursor;
			string initialDirectory="";
			if(Directory.Exists(PrefC.GetString(PrefName.DocPath))) {
				initialDirectory=PrefC.GetString(PrefName.DocPath);
			}
			else if(Directory.Exists("C:\\")) {
				initialDirectory="C:\\";
			}
			FileDialog dlg=null;
			if(isImport) {
				dlg=new OpenFileDialog();
			}
			else {//Export
				dlg=new SaveFileDialog();
				dlg.Filter="(*.txt)|*.txt|All files (*.*)|*.*";
				dlg.FilterIndex=1;
				dlg.DefaultExt=".txt";
				dlg.FileName="ExportedMedications.txt";
			}
			dlg.InitialDirectory=initialDirectory;
			if(dlg.ShowDialog()!=DialogResult.OK) {
				dlg=null;
			}
			else if(isImport && !File.Exists(dlg.FileName)){
				MsgBox.Show(this,"Error accessing file.");
				dlg=null;
			}
			Cursor=Cursors.Default;
			return (dlg==null ? "" : dlg.FileName);
		}
		
		private void textSearch_TextChanged(object sender,EventArgs e) {
			FillTab();
		}

		private void gridAllMedications_CellClick(object sender,ODGridClickEventArgs e) {
			Medication med=(Medication)gridAllMedications.ListGridRows[e.Row].Tag;
			if(CultureInfo.CurrentCulture.Name.EndsWith("US") && e.Col==3) {//United States RxNorm Column
				FormRxNorms formRxNorm=new FormRxNorms();
				formRxNorm.IsSelectionMode=true;
				formRxNorm.InitSearchCodeOrDescript=med.MedName;
				formRxNorm.ShowDialog();
				if(formRxNorm.DialogResult==DialogResult.OK) {
					med.RxCui=PIn.Long(formRxNorm.SelectedRxNorm.RxCui);
					//The following behavior mimics FormMedicationEdit OK click.
					Medications.Update(med);
					MedicationPats.UpdateRxCuiForMedication(med.MedicationNum,med.RxCui);
					DataValid.SetInvalid(InvalidType.Medications);
				}
				FillTab();
			}
		}

		private void gridAllMedications_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Medication med=(Medication)gridAllMedications.ListGridRows[e.Row].Tag;
			med=Medications.GetMedication(med.MedicationNum);
			if(med==null) {//Possible to delete the medication from a separate WS while medication loaded in memory.
				MsgBox.Show(this,"An error occurred loading medication.");
				return;
			}
			if(IsSelectionMode){
				SelectedMedicationNum=med.MedicationNum;
				DialogResult=DialogResult.OK;
			}
			else{//normal mode from main menu
				if(!CultureInfo.CurrentCulture.Name.EndsWith("US") || e.Col!=3) {//Not United States RxNorm Column
					FormMedicationEdit FormME=new FormMedicationEdit();
					FormME.MedicationCur=med;
					FormME.ShowDialog();//This window refreshes the Medication cache if the user clicked OK.
					FillTab();
				}
			}
		}

		private void butConvertGeneric_Click(object sender,EventArgs e) {
			if(gridMissing.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item from the list before attempting to convert.");
				return;
			}
			List<MedicationPat> listMedPats=(List<MedicationPat>)gridMissing.ListGridRows[gridMissing.SelectedIndices[0]].Tag;
			List<Medication> listRxCuiMeds=null;
			Medication medGeneric=null;
			if(listMedPats[0].RxCui!=0) {
				listRxCuiMeds=Medications.GetAllMedsByRxCui(listMedPats[0].RxCui);
				medGeneric=listRxCuiMeds.FirstOrDefault(x => x.MedicationNum==x.GenericNum);
				if(medGeneric==null && listRxCuiMeds.FirstOrDefault(x => x.MedicationNum!=x.GenericNum)!=null) {//A Brand Medication exists with matching RxCui.
					MsgBox.Show(this,"A brand medication matching the RxNorm of the selected medication already exists in the medication list.  "
						+"You cannot create a generic for the selected medication.  Use the Convert to Brand button instead.");
					return;
				}
			}
			if(listRxCuiMeds==null || listRxCuiMeds.Count==0) {//No medications found matching the RxCui
				medGeneric=new Medication();
				medGeneric.MedName=listMedPats[0].MedDescript;
				medGeneric.RxCui=listMedPats[0].RxCui;
				Medications.Insert(medGeneric);//To get primary key.
				medGeneric.GenericNum=medGeneric.MedicationNum;
				Medications.Update(medGeneric);//Now that we have primary key, flag the medication as a generic.
				FormMedicationEdit FormME=new FormMedicationEdit();
				FormME.MedicationCur=medGeneric;
				FormME.IsNew=true;
				FormME.ShowDialog();//This window refreshes the Medication cache if the user clicked OK.
				if(FormME.DialogResult!=DialogResult.OK) {
					return;//User canceled.
				}
			}
			else if(medGeneric!=null &&
				!MsgBox.Show(this,true,"A generic medication matching the RxNorm of the selected medication already exists in the medication list.  "
					+"Click OK to use the existing medication as the generic for the selected medication, or click Cancel to abort."))
			{
				return;
			}
			Cursor=Cursors.WaitCursor;
			MedicationPats.UpdateMedicationNumForMany(medGeneric.MedicationNum,listMedPats.Select(x => x.MedicationPatNum).ToList());
			FillTab();
			Cursor=Cursors.Default;
			MsgBox.Show(this,"Done.");
		}

		private void butConvertBrand_Click(object sender,EventArgs e) {
			if(gridMissing.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item from the list before attempting to convert.");
				return;
			}
			List<MedicationPat> listMedPats=(List<MedicationPat>)gridMissing.ListGridRows[gridMissing.SelectedIndices[0]].Tag;
			List<Medication> listRxCuiMeds=null;
			Medication medBrand=null;
			if(listMedPats[0].RxCui!=0) {
				listRxCuiMeds=Medications.GetAllMedsByRxCui(listMedPats[0].RxCui);
				medBrand=listRxCuiMeds.FirstOrDefault(x => x.MedicationNum!=x.GenericNum);
				if(medBrand==null && listRxCuiMeds.FirstOrDefault(x => x.MedicationNum==x.GenericNum)!=null) {//A Generic Medication exists with matching RxCui.
					MsgBox.Show(this,"A generic medication matching the RxNorm of the selected medication already exists in the medication list.  "
						+"You cannot create a brand for the selected medication.  Use the Convert to Generic button instead.");
					return;
				}
			}
			if(listRxCuiMeds==null || listRxCuiMeds.Count==0) {//No medications found matching the RxCui
				Medication medGeneric=null;
				if(gridAllMedications.SelectedIndices.Length > 0) {
					medGeneric=(Medication)gridAllMedications.ListGridRows[gridAllMedications.SelectedIndices[0]].Tag;
					if(medGeneric.MedicationNum!=medGeneric.GenericNum) {
						medGeneric=null;//The selected medication is a brand medication, not a generic medication.
					}
				}
				if(medGeneric==null) {
					MsgBox.Show(this,"Please select a generic medication from the All Medications tab before attempting to convert.  "
						+"The selected medication will be used as the generic medication for the new brand medication.");
					return;
				}
				medBrand=new Medication();
				medBrand.MedName=listMedPats[0].MedDescript;
				medBrand.RxCui=listMedPats[0].RxCui;
				medBrand.GenericNum=medGeneric.MedicationNum;
				Medications.Insert(medBrand);
				FormMedicationEdit FormME=new FormMedicationEdit();
				FormME.MedicationCur=medBrand;
				FormME.IsNew=true;
				FormME.ShowDialog();//This window refreshes the Medication cache if the user clicked OK.
				if(FormME.DialogResult!=DialogResult.OK) {
					return;//User canceled.
				}
			}
			else if(medBrand!=null &&
				!MsgBox.Show(this,true,"A brand medication matching the RxNorm of the selected medication already exists in the medication list.  "
					+"Click OK to use the existing medication as the brand for the selected medication, or click Cancel to abort."))
			{
				return;
			}
			Cursor=Cursors.WaitCursor;
			MedicationPats.UpdateMedicationNumForMany(medBrand.MedicationNum,listMedPats.Select(x => x.MedicationPatNum).ToList());
			FillTab();
			Cursor=Cursors.Default;
			MsgBox.Show(this,"Done.");
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			//this button is not visible if not selection mode.
			if(gridAllMedications.GetSelectedIndex()==-1) {
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}
			SelectedMedicationNum=((Medication)gridAllMedications.ListGridRows[gridAllMedications.GetSelectedIndex()].Tag).MedicationNum;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}





















