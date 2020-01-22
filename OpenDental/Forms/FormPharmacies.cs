using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDental.Bridges;
using System.Collections.Generic;
using System.Linq;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormPharmacies:ODForm {
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.Button butNone;
		private OpenDental.UI.Button butOK;
		private bool changed;
		public bool IsSelectionMode;
		///<summary>Only used if IsSelectionMode.  On OK, contains selected pharmacyNum.  Can be 0.  Can also be set ahead of time externally.</summary>
		public long SelectedPharmacyNum;
		private List<Pharmacy> _listPharmacies;

		///<summary></summary>
		public FormPharmacies()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPharmacies));
			this.butNone = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butNone
			// 
			this.butNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butNone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNone.Location = new System.Drawing.Point(502, 590);
			this.butNone.Name = "butNone";
			this.butNone.Size = new System.Drawing.Size(68, 24);
			this.butNone.TabIndex = 16;
			this.butNone.Text = "None";
			this.butNone.Click += new System.EventHandler(this.butNone_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(650, 590);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 15;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(17, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(789, 565);
			this.gridMain.TabIndex = 11;
			this.gridMain.Title = "Pharmacies";
			this.gridMain.TranslationName = "TablePharmacies";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// butAdd
			// 
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(17, 590);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(80, 24);
			this.butAdd.TabIndex = 10;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(731, 590);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormPharmacies
			// 
			this.ClientSize = new System.Drawing.Size(834, 630);
			this.Controls.Add(this.butNone);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPharmacies";
			this.ShowInTaskbar = false;
			this.Text = "Pharmacies";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPharmacies_FormClosing);
			this.Load += new System.EventHandler(this.FormPharmacies_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormPharmacies_Load(object sender, System.EventArgs e) {
			if(IsSelectionMode){
				butClose.Text=Lan.g(this,"Cancel");
			}
			else{
				butOK.Visible=false;
				butNone.Visible=false;
			}
			FillGrid();
			if(SelectedPharmacyNum!=0){
				for(int i=0;i<_listPharmacies.Count;i++){
					if(_listPharmacies[i].PharmacyNum==SelectedPharmacyNum){
						gridMain.SetSelected(i,true);
						break;
					}
				}
			}
		}

		private void FillGrid(){
			Pharmacies.RefreshCache();
			_listPharmacies=Pharmacies.GetDeepCopy();
			//Key=>PharmacyNum & Value=>List of clinics
			SerializableDictionary<long,List<Clinic>> dictPharmClinics=null;
			if(PrefC.HasClinicsEnabled) {
				dictPharmClinics=Clinics.GetDictClinicsForPharmacy(_listPharmacies.Select(x => x.PharmacyNum).ToArray());
			}
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TablePharmacies","Store Name"),130);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePharmacies","Phone"),90);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePharmacies","Fax"),90);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePharmacies","Address"),120);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePharmacies","City"),90);
			gridMain.ListGridColumns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new GridColumn(Lan.g("TablePharmacies","Clinics"),115);
				gridMain.ListGridColumns.Add(col);
			}
			col=new GridColumn(Lan.g("TablePharmacies","Note"),100);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			string txt;
			foreach(Pharmacy pharm in _listPharmacies) {
				row=new GridRow();
				row.Cells.Add(pharm.StoreName);
				row.Cells.Add(pharm.Phone);
				if(Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
					row.Cells[row.Cells.Count-1].ColorText=Color.Blue;
					row.Cells[row.Cells.Count-1].Underline=YN.Yes;
				}
				row.Cells.Add(pharm.Fax);
				txt=pharm.Address;
				if(pharm.Address2!="") {
					txt+="\r\n"+pharm.Address2;
				}
				row.Cells.Add(txt);
				row.Cells.Add(pharm.City);
				if(PrefC.HasClinicsEnabled) {
					List<Clinic> listClinics;
					if(!dictPharmClinics.TryGetValue(pharm.PharmacyNum,out listClinics)) {
						listClinics=new List<Clinic>();
					}
					row.Cells.Add(string.Join(",",listClinics.Select(x => x.Abbr)));
				}
				row.Cells.Add(pharm.Note);
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormPharmacyEdit FormPE=new FormPharmacyEdit();
			FormPE.PharmCur=new Pharmacy();
			FormPE.PharmCur.IsNew=true;
			FormPE.ShowDialog();
			FillGrid();
			changed=true;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(IsSelectionMode){
				SelectedPharmacyNum=_listPharmacies[e.Row].PharmacyNum;
				DialogResult=DialogResult.OK;
				return;
			}
			else{
				FormPharmacyEdit FormP=new FormPharmacyEdit();
				FormP.PharmCur=_listPharmacies[e.Row];
				FormP.ShowDialog();
				FillGrid();
				changed=true;
			}
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			GridCell gridCellCur=gridMain.ListGridRows[e.Row].Cells[e.Col];
			//Only grid cells with phone numbers are blue and underlined.
			if(gridCellCur.ColorText==System.Drawing.Color.Blue && gridCellCur.Underline==YN.Yes && Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
				DentalTek.PlaceCall(gridCellCur.Text);
			}
		}

		private void butNone_Click(object sender,EventArgs e) {
			//not even visible unless is selection mode
			SelectedPharmacyNum=0;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			//not even visible unless is selection mode
			if(gridMain.GetSelectedIndex()==-1){
			//	MsgBox.Show(this,"Please select an item first.");
			//	return;
				SelectedPharmacyNum=0;
			}
			else{
				SelectedPharmacyNum=_listPharmacies[gridMain.GetSelectedIndex()].PharmacyNum;
			}
			DialogResult=DialogResult.OK;
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormPharmacies_FormClosing(object sender,FormClosingEventArgs e) {
			if(changed){
				DataValid.SetInvalid(InvalidType.Pharmacies);
			}
		}

	

		

		

		



		
	}
}





















