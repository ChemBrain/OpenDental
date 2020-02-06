using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormLimitedStatementSelect:ODForm {
		private DataTable _tableAccount;
		private List<LimitedRow> _listLimitedRows=new List<LimitedRow>();
		private List<long> _listSelectedPaymentClaimNums=new List<long>();
		private List<long> _listSelectedProcClaimNums=new List<long>();
		private List<long> _listSelectedAdjustments=new List<long>();
		private List<long> _listSelectedPayNums=new List<long>();
		private List<long> _listSelectedProcedureNums=new List<long>();
		private List<long> _listSelectedPatNums=new List<long>();
		
		public List<long> ListSelectedPaymentClaimNums {
			get { return _listSelectedPaymentClaimNums; }
		}

		public List<long> ListSelectedProcClaimNums {
			get { return _listSelectedProcClaimNums; }
		}

		public List<long> ListSelectedAdjustments {
			get { return _listSelectedAdjustments; }
		}

		public List<long> ListSelectedPayNums {
			get { return _listSelectedPayNums; }
		}

		public List<long> ListSelectedProcedureNums {
			get {return _listSelectedProcedureNums; }
		}

		public List<long> ListPatNumsSelected {
			get { return _listSelectedPatNums;}
		}

		public FormLimitedStatementSelect(DataTable accountTable) {
			InitializeComponent();
			_tableAccount=accountTable;
			Lan.F(this);
		}

		private void FormLimitedStatementSelect_Load(object sender,EventArgs e) {
			SetFilterControlsAndAction(() => FillGrid(),comboBoxMultiTransTypes,odDateRangePicker);
			odDateRangePicker.SetDateTimeFrom(DateTime.Today.AddDays(-7));
			odDateRangePicker.SetDateTimeTo(DateTime.Today);
			FillTransType();
			ConstructListFromTable();
			FillGrid();
		}

		private void ConstructListFromTable() {
			List<LimitedRow> listLimitedRows=new List<LimitedRow>();
			foreach(DataRow tableRow in _tableAccount.Rows) {
				LimitedRow row=new LimitedRow();
				row.PatNum=PIn.Long(tableRow["PatNum"].ToString());
				row.PatientName=tableRow["patient"].ToString();
				row.Description=tableRow["description"].ToString();
				row.ProcCode=tableRow["ProcCode"].ToString();//isn't just a proc code. Can be "Claim" etc...
				row.Charges=tableRow["charges"].ToString();
				row.Credits=tableRow["credits"].ToString();
				row.ProvName=tableRow["prov"].ToString();
				row.Tooth=Tooth.ToInternat(tableRow["ToothNum"].ToString());
				row.DateTime=PIn.DateT(tableRow["DateTime"].ToString());
				if(tableRow["AdjNum"].ToString()!="0") {
					row.PrimaryKey=PIn.Long(tableRow["AdjNum"].ToString());
					row.Type=AccountEntryType.Adjustment;
				}
				else if(tableRow["ProcNum"].ToString()!="0") {
					row.PrimaryKey=PIn.Long(tableRow["ProcNum"].ToString());
					row.Type=AccountEntryType.Procedure;
				}
				else if(tableRow["PayNum"].ToString()!="0") {
					row.PrimaryKey=PIn.Long(tableRow["PayNum"].ToString());
					row.Type=AccountEntryType.Payment;
				}
				else if(tableRow["ClaimNum"].ToString()!="0") {
					//can mean that this is either a claim or a claim payment.
					//we really only care about claim payments, but we need procedure from the claim.
					row.PrimaryKey=PIn.Long(tableRow["ClaimNum"].ToString());
					row.Type=AccountEntryType.Claim;
					if(tableRow["ClaimPaymentNum"].ToString()=="1") {
						row.Type=AccountEntryType.ClaimPayment;
					}		
				}
				else {
					//type is not one that is currently supported, skip it.
					continue;
				}
				row.ListProcsForObject=tableRow["procsOnObj"].ToString().Split(',').Select(x => PIn.Long(x)).ToList();
				_listLimitedRows.Add(row);
			}
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			gridMain.ListGridColumns.Add(new GridColumn("Date",70,GridSortingStrategy.DateParse));
			gridMain.ListGridColumns.Add(new GridColumn("Patient",-100,GridSortingStrategy.StringCompare));//grows dynamically
			gridMain.ListGridColumns.Add(new GridColumn("Prov",-40,GridSortingStrategy.StringCompare));//grows dynamically
			gridMain.ListGridColumns.Add(new GridColumn("Code",-50,GridSortingStrategy.StringCompare));//grows dynamically
			gridMain.ListGridColumns.Add(new GridColumn("Tooth",45,GridSortingStrategy.ToothNumberParse));
			gridMain.ListGridColumns.Add(new GridColumn("Description",-200,GridSortingStrategy.StringCompare));//grows dynamically
			gridMain.ListGridColumns.Add(new GridColumn("Charges",65,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridMain.ListGridColumns.Add(new GridColumn("Credits",65,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridMain.ListGridRows.Clear();
			GridRow row;
			foreach(LimitedRow limitedRow in _listLimitedRows.FindAll(x => x.Type.In(comboBoxMultiTransTypes.SelectedTags<AccountEntryType>()))) {
				row=new GridRow();
				DateTime date=limitedRow.DateTime.Date;
				if(date < odDateRangePicker.GetDateTimeFrom().Date || date > odDateRangePicker.GetDateTimeTo().Date) {
					continue;//do not add to grid if it is outside the filtered date range. 
				}
				row.Cells.Add(date.ToShortDateString());
				row.Cells.Add(limitedRow.PatientName);
				row.Cells.Add(limitedRow.ProvName);
				row.Cells.Add(limitedRow.ProcCode);
				row.Cells.Add(limitedRow.Tooth);
				row.Cells.Add(limitedRow.Description);
				row.Cells.Add(limitedRow.Charges);
				row.Cells.Add(limitedRow.Credits);
				row.Tag=limitedRow;
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void FillTransType() {
			comboBoxMultiTransTypes.Items.Clear();
			ODBoxItem<AccountEntryType> type=new ODBoxItem<AccountEntryType>(Lan.g(this,"Adjustment"),AccountEntryType.Adjustment);
			comboBoxMultiTransTypes.Items.Add(type);
			type=new ODBoxItem<AccountEntryType>(Lan.g(this,"Procedure"),AccountEntryType.Procedure);
			comboBoxMultiTransTypes.Items.Add(type);
			type=new ODBoxItem<AccountEntryType>(Lan.g(this,"Payment"),AccountEntryType.Payment);
			comboBoxMultiTransTypes.Items.Add(type);
			type=new ODBoxItem<AccountEntryType>(Lan.g(this,"Claim Payment"),AccountEntryType.ClaimPayment);
			comboBoxMultiTransTypes.Items.Add(type);
			comboBoxMultiTransTypes.SetSelected(true);
			//add for payplans later
		}

		private void butAll_Click(object sender,EventArgs e) {
			gridMain.SetSelected(true);
		}

		private void butNone_Click(object sender,EventArgs e) {
			gridMain.SetSelected(false);
		}

		private void butOK_Click(object sender,EventArgs e) {
			List<LimitedRow> listSelectedRows=gridMain.SelectedTags<LimitedRow>();
			//get PKs since the original row was saved as the tag. 
			_listSelectedPatNums.AddRange(listSelectedRows.Select(x => x.PatNum).Distinct());
			_listSelectedAdjustments.AddRange(listSelectedRows.FindAll(x => x.Type==AccountEntryType.Adjustment).Select(x => x.PrimaryKey));
			_listSelectedPayNums.AddRange(listSelectedRows.FindAll(x => x.Type==AccountEntryType.Payment).Select(x => x.PrimaryKey));
			_listSelectedProcedureNums.AddRange(listSelectedRows.FindAll(x => x.Type==AccountEntryType.Procedure).Select(x => x.PrimaryKey));
			_listSelectedPaymentClaimNums.AddRange(listSelectedRows.FindAll(x => x.Type==AccountEntryType.ClaimPayment).Select(x => x.PrimaryKey));
			//get procs for the claims. 
			List<LimitedRow> listClaimsForClaimPayments=_listLimitedRows
				.FindAll(x => x.Type==AccountEntryType.Claim && x.PrimaryKey.In(_listSelectedPaymentClaimNums));
			_listSelectedProcedureNums.AddRange(listClaimsForClaimPayments.SelectMany(x => x.ListProcsForObject));
			_listSelectedProcedureNums=_listSelectedProcedureNums.Distinct().ToList();
			//get a list of claims for all selected procedures. 
			_listSelectedProcClaimNums=ClaimProcs.GetForProcs(_listSelectedProcedureNums).FindAll(x => x.ClaimNum!=0).Select(x => x.ClaimNum).ToList();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		/// <summary>Class to represent items coming in from the account module grid.</summary>
		private class LimitedRow {
			//Can be paymentNum,adjustmentNum,procedureNum,claimNum,payplanNum. NOTE: ClaimPayments will hold the ClaimNum. 
			public long PrimaryKey;
			public AccountEntryType Type;
			//List of procedures attached to the object (if any)
			public List<long> ListProcsForObject=new List<long>();
			public string Description;
			public string Charges;
			public string Credits;
			public string PatientName;
			public long PatNum;
			public string ProcCode;
			public string Tooth;
			public string ProvName;
			public DateTime DateTime;
		}

	}

	
}