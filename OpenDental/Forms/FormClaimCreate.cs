using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
///<summary>Lists all insurance plans for which the supplied patient is the subscriber. Lets you select an insurance plan based on a patNum. SelectedPlan will contain the plan selected.</summary>
	public class FormClaimCreate : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ListBox listRelat;
		//private OpenDental.TableInsPlans tbPlans;
		///<summary></summary>
		public Relat PatRelat;
		private System.Windows.Forms.Label labelRelat;
		//<summary>Set to true to view the relationship selection</summary>
		//public bool ViewRelat;
		private Patient PatCur;
		private Family FamCur;
		///<summary>After closing this form, this will contain the selected plan.</summary>
		public InsPlan SelectedPlan;
		public InsSub SelectedSub;
		private List <InsPlan> PlanList;
		private OpenDental.UI.ODGrid gridMain;
		private long PatNum;
		//public long ClaimFormNum;
		private List<InsSub> SubList;
		private CheckBox checkShowPlansNotInUse;
		//List of PatPlans for PatCur.
		private List<PatPlan> _listPatCurPatPlans;

		///<summary></summary>
		public FormClaimCreate(long patNum) {
			InitializeComponent();
			PatNum=patNum;
			//tbPlans.CellDoubleClicked += new OpenDental.ContrTable.CellEventHandler(tbPlans_CellDoubleClicked);
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose(bool disposing){
			if(disposing){
				if(components!=null){
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClaimCreate));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.labelRelat = new System.Windows.Forms.Label();
			this.listRelat = new System.Windows.Forms.ListBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.checkShowPlansNotInUse = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(649, 275);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(76, 26);
			this.butCancel.TabIndex = 6;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(556, 275);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(76, 26);
			this.butOK.TabIndex = 5;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// labelRelat
			// 
			this.labelRelat.Location = new System.Drawing.Point(563, 17);
			this.labelRelat.Name = "labelRelat";
			this.labelRelat.Size = new System.Drawing.Size(192, 20);
			this.labelRelat.TabIndex = 8;
			this.labelRelat.Text = "Relationship to Subscriber";
			this.labelRelat.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listRelat
			// 
			this.listRelat.Location = new System.Drawing.Point(565, 38);
			this.listRelat.Name = "listRelat";
			this.listRelat.Size = new System.Drawing.Size(180, 186);
			this.listRelat.TabIndex = 9;
			// 
			// gridMain
			// 
			this.gridMain.Location = new System.Drawing.Point(8, 38);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(551, 186);
			this.gridMain.TabIndex = 10;
			this.gridMain.Title = "Insurance Plans for Family";
			this.gridMain.TranslationName = "TableInsPlans";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// checkShowPlansNotInUse
			// 
			this.checkShowPlansNotInUse.Location = new System.Drawing.Point(8, 19);
			this.checkShowPlansNotInUse.Name = "checkShowPlansNotInUse";
			this.checkShowPlansNotInUse.Size = new System.Drawing.Size(329, 17);
			this.checkShowPlansNotInUse.TabIndex = 11;
			this.checkShowPlansNotInUse.Text = "Show plans for family which are not in use by the current patient.";
			this.checkShowPlansNotInUse.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.checkShowPlansNotInUse.UseVisualStyleBackColor = true;
			this.checkShowPlansNotInUse.Click += new System.EventHandler(this.checkPlansNotInUse_Click);
			// 
			// FormClaimCreate
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(755, 319);
			this.Controls.Add(this.checkShowPlansNotInUse);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.listRelat);
			this.Controls.Add(this.labelRelat);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormClaimCreate";
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Create New Claim";
			this.Load += new System.EventHandler(this.FormClaimCreate_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormClaimCreate_Load(object sender, System.EventArgs e) {
			//usage: eg. from coverage.  Since can be totally new subscriber, get all plans for them.
			FamCur=Patients.GetFamily(PatNum);
			PatCur=FamCur.GetPatient(PatNum);
			SubList=InsSubs.RefreshForFam(FamCur);
			PlanList=InsPlans.RefreshForSubList(SubList);
			_listPatCurPatPlans=PatPlans.Refresh(PatNum);
			FillPlanData();
			//FillClaimForms();
    }

		/*
		private void FillClaimForms(){
			for(int i=0;i<ClaimForms.ListShort.Length;i++) {
				comboClaimForm.Items.Add(ClaimForms.ListShort[i].Description);
				if(ClaimForms.ListShort[i].ClaimFormNum==PrefC.GetLong(PrefName.DefaultClaimForm)) {
					comboClaimForm.SelectedIndex=i;
				}
			}
		}*/

		private void FillPlanData(){
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableInsPlans","Plan"),50);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableInsPlans","Subscriber"),140);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableInsPlans","Ins Carrier"),100);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableInsPlans","Date Effect."),90);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableInsPlans","Date Term."),90);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			List<GridRow> listRows=new List<GridRow>(); //create a list of gridrows so that we can order them by Ordinal after creating them.
			for(int i=0;i<SubList.Count;i++) {
				row=new GridRow();
				if(!checkShowPlansNotInUse.Checked && //Only show insurance plans for PatCur.
					!_listPatCurPatPlans.Exists(x => x.InsSubNum==SubList[i].InsSubNum)) 
				{
					continue;
				}
				else if(checkShowPlansNotInUse.Checked && !_listPatCurPatPlans.Exists(x => x.InsSubNum==SubList[i].InsSubNum)) {
					row.Cells.Add("Not Used");
				}
				else {
					PatPlan patPlan=_listPatCurPatPlans.FirstOrDefault(x => x.InsSubNum==SubList[i].InsSubNum);
					if(patPlan==null) {
						continue;
					}
					if(patPlan.Ordinal==1) {
						row.Cells.Add("Pri");
					}
					else if(patPlan.Ordinal==2) {
						row.Cells.Add("Sec");
					}
					else {
						row.Cells.Add("Other");
					}					
				}
				InsPlan plan=InsPlans.GetPlan(SubList[i].PlanNum,PlanList);
				row.Tag=SubList[i];
				row.Cells.Add(FamCur.GetNameInFamLF(SubList[i].Subscriber));
				row.Cells.Add(Carriers.GetName(plan.CarrierNum));
				if(SubList[i].DateEffective.Year<1880) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(SubList[i].DateEffective.ToString("d"));
				}
				if(SubList[i].DateTerm.Year<1880) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(SubList[i].DateTerm.ToString("d"));
				}
				listRows.Add(row);
			}
			listRows=listRows.OrderBy(x => x.Cells[0].Text != "Pri")
				.ThenBy(x => x.Cells[0].Text !="Sec")
				.ThenBy(x => x.Cells[0].Text !="Other")
				.ThenBy(x => x.Cells[0].Text !="Not Used").ToList();
			for(int i=0; i<listRows.Count;i++) {
				gridMain.ListGridRows.Add(listRows[i]);
			}
			gridMain.EndUpdate();
			listRelat.Items.Clear();
			for(int i=0;i<Enum.GetNames(typeof(Relat)).Length;i++){
				listRelat.Items.Add(Lan.g("enumRelat",Enum.GetNames(typeof(Relat))[i]));
			}
		}

		private void checkPlansNotInUse_Click(object sender,EventArgs e) {
			FillPlanData();
		}

		private void gridMain_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			if(listRelat.SelectedIndex==-1) {
				MessageBox.Show(Lan.g(this,"Please select a relationship first."));
				return;
			}
			PatRelat=(Relat)listRelat.SelectedIndex;
			SelectedSub=(InsSub)gridMain.ListGridRows[e.Row].Tag;
			SelectedPlan=InsPlans.GetPlan(SelectedSub.PlanNum,PlanList);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				MessageBox.Show(Lan.g(this,"Please select a plan first."));
				return;
			}
			if(listRelat.SelectedIndex==-1){
				MessageBox.Show(Lan.g(this,"Please select a relationship first."));
				return;
			}
			//if(comboClaimForm.SelectedIndex==-1) {
			//	MessageBox.Show(Lan.g(this,"Please select a claimform first."));
			//	return;
			//}
			PatRelat=(Relat)listRelat.SelectedIndex;
			SelectedSub=(InsSub)gridMain.ListGridRows[gridMain.GetSelectedIndex()].Tag;
			SelectedPlan=InsPlans.GetPlan(SelectedSub.PlanNum,PlanList);
			//ClaimFormNum=ClaimForms.ListShort[comboClaimForm.SelectedIndex].ClaimFormNum;
      DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		//cancel already handled
	}
}