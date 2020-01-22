using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using OpenDental.UI;
using System.Linq;

namespace OpenDental {
	public partial class FormFeeSchedGroups:ODForm {
		///<summary>List of all FeeSchedGroups in db.</summary>
		private List<FeeSchedGroup> _listFeeSchedGroups;
		///<summary>List of all clinics for the selected FeeSchedGroup in the grid.  Used to fill gridClinics.</summary>
		private List<Clinic> _listClinicsForGroup=new List<Clinic>();

		public FormFeeSchedGroups() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormFeeSchedGroups_Load(object sender,EventArgs e) {
			//No selections when loading form.
			FillGridGroups();
			//Need to call on load to set column headers.
			FillGridClinics();
		}

		private void FillGridGroups() {
			gridGroups.BeginUpdate();
			gridGroups.ListGridColumns.Clear();
			GridColumn col;
			col=new GridColumn(Lan.g(this,"Group Name"),100);
			gridGroups.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Fee Schedule"),50);
			gridGroups.ListGridColumns.Add(col);
			gridGroups.ListGridRows.Clear();
			GridRow row;
			_listFeeSchedGroups=FeeSchedGroups.GetAll().OrderBy(x => x.Description).ToList();
			foreach(FeeSchedGroup feeSchedGroupCur in _listFeeSchedGroups) {
				row=new GridRow();
				row.Cells.Add(feeSchedGroupCur.Description);
				row.Cells.Add(FeeScheds.GetDescription(feeSchedGroupCur.FeeSchedNum));//Returns empty string if the FeeSched couldn't be found.
				row.Tag=feeSchedGroupCur;
				gridGroups.ListGridRows.Add(row);
			} 
			gridGroups.EndUpdate();
		}

		private void FillGridClinics() {
			_listClinicsForGroup.Clear();
			if(gridGroups.GetSelectedIndex()>=0) {
				_listClinicsForGroup=Clinics.GetClinics(gridGroups.SelectedTag<FeeSchedGroup>().ListClinicNumsAll).OrderBy(x => x.Abbr).ToList();
			}
			gridClinics.BeginUpdate();
			gridClinics.ListGridColumns.Clear();
			GridColumn col;
			col=new GridColumn(Lan.g(this,"Abbr"),-1);
			gridClinics.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Description"),-2);
			gridClinics.ListGridColumns.Add(col);
			gridClinics.ListGridRows.Clear();
			GridRow row;
			foreach(Clinic clinicCur in _listClinicsForGroup) {
				row=new GridRow();
				row.Cells.Add(clinicCur.Abbr);
				row.Cells.Add(clinicCur.Description+(clinicCur.IsHidden?" (Hidden)":""));
				row.Tag=clinicCur;
				gridClinics.ListGridRows.Add(row);
			}
			gridClinics.EndUpdate();
		}

		private void gridGroups_CellClick(object sender,ODGridClickEventArgs e) {
			FillGridClinics();
		}

		private void gridGroups_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			FeeSchedGroup feeSchedGroupCur=(FeeSchedGroup)gridGroups.ListGridRows[e.Row].Tag;
			FormFeeSchedGroupEdit formFG=new FormFeeSchedGroupEdit(feeSchedGroupCur);
			formFG.ShowDialog();
			if(formFG.DialogResult==DialogResult.OK) {
				FeeSchedGroups.Update(feeSchedGroupCur);
			}
			//Still need to refresh incase the user deleted the FeeSchedGroup, since it returns DialogResult.Cancel.
			FillGridGroups();
			FillGridClinics();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FeeSchedGroup feeSchedGroupNew=new FeeSchedGroup(){ ListClinicNumsAll=new List<long>(), IsNew=true };
			FormFeeSchedGroupEdit formFG=new FormFeeSchedGroupEdit(feeSchedGroupNew);
			formFG.ShowDialog();
			if(formFG.DialogResult==DialogResult.OK) {
				FeeSchedGroups.Insert(feeSchedGroupNew);
				FillGridGroups();
				FillGridClinics();
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}