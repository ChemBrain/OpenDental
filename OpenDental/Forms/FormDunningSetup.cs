using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormDunningSetup:ODForm {
		private List<Dunning> _listAllDunnings;
		private List<Clinic> _listClinics;
		private List<Def> _listBillingTypeDefs;

		public FormDunningSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormDunningSetup_Load(object sender,EventArgs e) {
			_listBillingTypeDefs=Defs.GetDefsForCategory(DefCat.BillingTypes,true);
			listBill.Items.Add("("+Lan.g(this,"all")+")");
			listBill.SetSelected(0,true);
			listBill.Items.AddRange(_listBillingTypeDefs.Select(x => x.ItemName).ToArray());
			FillClinics();
			FillGrids(true);
		}

		private void FillClinics() {
			if(!PrefC.HasClinicsEnabled) {
				_listClinics=new List<Clinic>();//Just in case.
				return;
			}
			_listClinics=Clinics.GetForUserod(Security.CurUser);
			if(!Security.CurUser.ClinicIsRestricted) {
				_listClinics.Insert(0,new Clinic() { ClinicNum = 0,Abbr = "Unassigned",Description = "Unassigned" });
			}
			labelClinic.Visible=true;
			comboClinics.Visible=true;
			butPickClinic.Visible=true;
			//ClinicNum: -1 for All
			comboClinics.Items.Add(new ODBoxItem<Clinic>(Lan.g(this,"All"),new Clinic() { ClinicNum=-1,Abbr="All",Description="All" }));
			comboClinics.SelectedIndex=0; //select 'All' by default
			foreach(Clinic clinic in _listClinics) {
				int index=comboClinics.Items.Add(new ODBoxItem<Clinic>(clinic.Abbr,clinic));
				if(clinic.ClinicNum==Clinics.ClinicNum) {
					comboClinics.SelectedIndex=index;
				}
			}
		}
		
		private void butPickClinic_Click(object sender,EventArgs e) {
			FormClinics FormC=new FormClinics();
			FormC.IsSelectionMode=true;
			FormC.ListClinics=_listClinics;//Includes 'Unassigned'
			if(comboClinics.SelectedIndex!=0) {//'All'
				FormC.ListSelectedClinicNums=new List<long> { _listClinics[comboClinics.SelectedIndex-1].ClinicNum };//-1 for 'All'
			}
			if(FormC.ShowDialog()!=DialogResult.OK) {
				return;
			}
			comboClinics.SelectedIndex=_listClinics.FindIndex(x => x.ClinicNum==FormC.SelectedClinicNum)+1;//+1 for 'All'
			FillGrids();
		}

		private void FillGrids(bool doRefreshList=false) {
			if(doRefreshList) {
				_listAllDunnings=Dunnings.Refresh(_listClinics.Select(x => x.ClinicNum).ToList());
			}
			List<Dunning> listSubDunnings=_listAllDunnings.FindAll(x => ValidateDunningFilters(x));
			if(!PrefC.GetBool(PrefName.ShowFeatureSuperfamilies)) {
				listSubDunnings.RemoveAll(x => x.IsSuperFamily);
			}
			gridDunning.BeginUpdate();
			gridDunning.ListGridColumns.Clear();
			gridDunning.ListGridColumns.Add(new GridColumn("Billing Type",80));
			gridDunning.ListGridColumns.Add(new GridColumn("Aging",70));
			gridDunning.ListGridColumns.Add(new GridColumn("Ins",40));
			gridDunning.ListGridColumns.Add(new GridColumn("Message",150));
			gridDunning.ListGridColumns.Add(new GridColumn("Bold Message",150));
			gridDunning.ListGridColumns.Add(new GridColumn("Email",35,HorizontalAlignment.Center));
			if(PrefC.GetBool(PrefName.ShowFeatureSuperfamilies)) {
				gridDunning.ListGridColumns.Add(new GridColumn("SF",30,HorizontalAlignment.Center));
			}
			if(PrefC.HasClinicsEnabled) {
				gridDunning.ListGridColumns.Add(new GridColumn("Clinic",50));
			}
			gridDunning.ListGridRows.Clear();
			GridRow row;
			foreach(Dunning dunnCur in listSubDunnings) {
				row=new GridRow();
				if(dunnCur.BillingType==0){
					row.Cells.Add(Lan.g(this,"all"));
				}
				else{
					row.Cells.Add(Defs.GetName(DefCat.BillingTypes,dunnCur.BillingType));
				}
				if(dunnCur.AgeAccount==0){
					row.Cells.Add(Lan.g(this,"any"));
				}
				else{
					row.Cells.Add(Lan.g(this,"Over ")+dunnCur.AgeAccount.ToString());
				}
				if(dunnCur.InsIsPending==YN.Yes) {
					row.Cells.Add(Lan.g(this,"Y"));
				}
				else if(dunnCur.InsIsPending==YN.No) {
					row.Cells.Add(Lan.g(this,"N"));
				}
				else {//YN.Unknown
					row.Cells.Add(Lan.g(this,"any"));
				}
				row.Cells.Add(dunnCur.DunMessage);
				row.Cells.Add(new GridCell(dunnCur.MessageBold) { Bold=YN.Yes,ColorText=Color.DarkRed });
				row.Cells.Add((!string.IsNullOrEmpty(dunnCur.EmailBody) || !string.IsNullOrEmpty(dunnCur.EmailSubject))?"X":"");
				if(PrefC.GetBool(PrefName.ShowFeatureSuperfamilies)) {
					row.Cells.Add(dunnCur.IsSuperFamily?"X":"");
				}
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(_listClinics.Find(x => x.ClinicNum==dunnCur.ClinicNum)?.Abbr??"");
				}
				row.Tag=dunnCur;
				gridDunning.ListGridRows.Add(row);
			}
			gridDunning.EndUpdate();
		}

		private bool ValidateDunningFilters(Dunning dunning) {
			long clinicNum=GetSelectedClinicNum();
			if((clinicNum!=-1 && clinicNum!=dunning.ClinicNum)
				||(!listBill.SelectedIndices.Contains(0) && !listBill.SelectedIndices.OfType<int>().Select(x => _listBillingTypeDefs[x-1].DefNum).Contains(dunning.BillingType))
				||(!radioAny.Checked && dunning.AgeAccount!=(byte)(30*new List<RadioButton> { radioAny,radio30,radio60,radio90 }.FindIndex(x => x.Checked)))//0, 30, 60, or 90
				||(!string.IsNullOrWhiteSpace(textAdv.Text) && dunning.DaysInAdvance!=PIn.Int(textAdv.Text,false))//blank=0
				||(!radioU.Checked && dunning.InsIsPending!=(YN)new List<RadioButton> { radioU,radioY,radioN }.FindIndex(x => x.Checked)))//0=Unknown, 1=Yes, 2=No+
			{
				return false;
			}
			return true;
		}

		///<summary>Returns -1 if clinics are not enabled or for 'All'. Otherwise 0 for 'Unassigned' or a valid ClinicNum </summary>
		private long GetSelectedClinicNum() {
			if(!PrefC.HasClinicsEnabled || comboClinics.SelectedIndex==0) {
				return -1;
			}
			return _listClinics[comboClinics.SelectedIndex-1].ClinicNum;//-1 for All
		}

		private void OnFilterChanged(object sender,EventArgs e) {
		 if(_listAllDunnings==null) {//Not initialized yet
			return;
		 }
			FillGrids();
		}

		private void gridDunning_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormDunningEdit formD=new FormDunningEdit((Dunning)gridDunning.ListGridRows[e.Row].Tag);
			if(formD.ShowDialog()!=DialogResult.OK) {
				return;
			}
			FillGrids(true);
		}

		private void butAdd_Click(object sender,EventArgs e) {
			Dunning dun=new Dunning();
			long clinicNum=GetSelectedClinicNum();
			if(clinicNum!=-1) {//If 'All' is selected, then ClinicNums=0
				dun.ClinicNum=clinicNum;
			}
			FormDunningEdit FormD=new FormDunningEdit(dun);
			FormD.IsNew=true;
			if(FormD.ShowDialog()==DialogResult.OK) {
				FillGrids(true);
			}
		}
		
		private void butDuplicate_Click(object sender,EventArgs e) {
			if(gridDunning.SelectedIndices.Count()==0) {
				MsgBox.Show(this,"Please select a message to duplicate first.");
				return;
			}
			Dunning dun=((Dunning)gridDunning.ListGridRows[gridDunning.GetSelectedIndex()].Tag).Copy();
			Dunnings.Insert(dun);
			FillGrids(true);
			gridDunning.SetSelected(gridDunning.ListGridRows.OfType<GridRow>().ToList().FindIndex(x => ((Dunning)x.Tag).DunningNum==dun.DunningNum),true);
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}
		
	}
}
