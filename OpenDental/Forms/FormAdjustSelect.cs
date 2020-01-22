using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental {
	public partial class FormAdjustSelect:ODForm {
		public Adjustment SelectedAdj;
		public PaySplit PaySplitCur;
		public List<PaySplit> ListSplitsForPayment;
		public long PatNumCur;
		public double PaySplitCurAmt;
		public long PayNumCur;
		private List<PaySplit> _listSplitsForAdjusts;		
		private List<AccountEntry> _listAdjusts=new List<AccountEntry>();

		public FormAdjustSelect() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormAdjustSelect_Load(object sender,EventArgs e) {
			List<Adjustment> listAdjusts=Adjustments.GetAdjustForPats(new List<long>() { PatNumCur }).FindAll(x => x.ProcNum==0);//Get all unallocated adjustments for current pat.
			foreach(Adjustment adjust in listAdjusts) {
				_listAdjusts.Add(new AccountEntry(adjust));
			}
			_listSplitsForAdjusts=PaySplits.GetForAdjustments(listAdjusts.Select(x => x.AdjNum).ToList());
			foreach(AccountEntry entry in _listAdjusts) {
				//Figure out how much each adjustment has left, not counting this payment.
				entry.AmountStart-=(decimal)Adjustments.GetAmtAllocated(entry.PriKey,PayNumCur,_listSplitsForAdjusts.FindAll(x => x.AdjNum==entry.PriKey));
				//Reduce adjustments based on current payment's splits as well (this is in-memory list, could be new, could be modified) but not the current split
				entry.AmountStart-=(decimal)Adjustments.GetAmtAllocated(entry.PriKey,0,ListSplitsForPayment.FindAll(x => x.AdjNum==entry.PriKey && x!=PaySplitCur));
			}
			FillGrid();
		}

		private void FillGrid() {
			gridAdjusts.BeginUpdate();
			gridAdjusts.ListGridRows.Clear();
			gridAdjusts.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableAdjustSelect","Date"),70);
			gridAdjusts.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableAdjustSelect","Prov"),55);
			gridAdjusts.ListGridColumns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new GridColumn(Lan.g("TableAdjustSelect","Clinic"),55);
				gridAdjusts.ListGridColumns.Add(col);
			}
			col=new GridColumn(Lan.g("TableProcSelect","Amt Orig"),60,HorizontalAlignment.Right);
			gridAdjusts.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcSelect","Amt End"),60,HorizontalAlignment.Right);
			gridAdjusts.ListGridColumns.Add(col);
			GridRow row;
			foreach(AccountEntry entry in _listAdjusts) {
				row=new GridRow();
				row.Cells.Add(((Adjustment)entry.Tag).AdjDate.ToShortDateString());
				row.Cells.Add(Providers.GetAbbr(((Adjustment)entry.Tag).ProvNum));
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetAbbr(((Adjustment)entry.Tag).ClinicNum));
				}
				row.Cells.Add(entry.AmountOriginal.ToString("F"));//Amt Orig
				row.Cells.Add(entry.AmountStart.ToString("F"));//Amt Available
				row.Tag=entry;
				gridAdjusts.ListGridRows.Add(row);
			}
			gridAdjusts.EndUpdate();
		}

		private void gridAdjusts_CellClick(object sender,UI.ODGridClickEventArgs e) {
			//Update text boxes here
			AccountEntry entry=(AccountEntry)gridAdjusts.SelectedGridRows[0].Tag;//Only one can be selected at a time.
			labelAmtOriginal.Text=entry.AmountOriginal.ToString("F");//Adjustment's start original - Negative or positive it doesn't matter.
			labelAmtUsed.Text=(entry.AmountOriginal-entry.AmountStart).ToString("F");//Amount of Adjustment that's been used elsewhere
			labelAmtAvail.Text=entry.AmountStart.ToString("F");//Amount of Adjustment that's left available
			labelCurSplitAmt.Text=(-PaySplitCurAmt).ToString("F");//Amount of current PaySplit (We can only access this window from current PaySplitEdit window right now)
			labelAmtEnd.Text=((double)entry.AmountStart-PaySplitCurAmt).ToString("F");//Amount of Adjustment after everything.
		}

		private void gridAdjusts_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			SelectedAdj=(Adjustment)((AccountEntry)gridAdjusts.SelectedGridRows[0].Tag).Tag;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridAdjusts.SelectedIndices.Length<1) {
				MsgBox.Show(this,"Please select an adjustment first or press Cancel.");
				return;
			}
			SelectedAdj=(Adjustment)((AccountEntry)gridAdjusts.SelectedGridRows[0].Tag).Tag;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}