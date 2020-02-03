using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using System.Drawing;
using System.Drawing.Printing;
using CodeBase;

namespace OpenDental {
	///<summary>This form will not be available to those who are still using PayPlans Version 1.</summary>
	public partial class FormPayPlanCredits:ODForm {
		///<summary>Set to the patient of the payment plan in the constructor.</summary>
		private Patient _patCur;
		private PayPlan _payPlanCur;
		public bool IsNew;
		public bool IsDeleted;
		private List<Adjustment> _listAdjustments;
		private List<Procedure> _listProcs;
		/// <summary>Stores a list of payplan links for procedures that are already associated to dynamic payment plans.</summary>
		private List<PayPlanLink> _listPayPlanLinksForProcs;
		private List<PayPlanCharge> _listPayPlanCharges;
		private List<ClaimProc> _listInsPayAsTotal;
		private List<Payment> _listPayments;
		private List<PaySplit> _listPaySplits;
		private List<AccountEntry>_listAccountCharges;
		private decimal _accountCredits;
		///<summary>Payment plan credits for the current payment plan.  Must be passed-in.</summary>
		public List<PayPlanCharge> ListPayPlanCreditsCur;
		///<summary>For getting insurace estimates for TP procedures and completed procedures whose claims haven't been received.
    ///Also used for getting insurance payments on procedures.</summary>
		private List<ClaimProc> _listClaimProcs;
		private List<PayPlanEdit.PayPlanEntry> _listPayPlanEntries;
		private bool headingPrinted;
		private int pagesPrinted;
		private int headingPrintH;
		//The provider on the payment plan, as set by the comboBox in FormPayPlan.
		private long _provNum;

		public FormPayPlanCredits(PayPlan payPlanCur,long provNum) {
			_payPlanCur=payPlanCur;
			_provNum=provNum;
			_patCur=Patients.GetPat(payPlanCur.PatNum);
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPayPlanCredits_Load(object sender,EventArgs e) {
			PayPlanEdit.PayPlanCreditLoadData loadData=PayPlanEdit.GetLoadDataForPayPlanCredits(_patCur.PatNum,_payPlanCur.PayPlanNum);
			_listAdjustments=loadData.ListAdjustments;
			_listProcs=loadData.ListProcs;
			_listPayPlanLinksForProcs=loadData.ListPayPlanLinksForProcs;
			_listPayPlanCharges=loadData.ListPayPlanCharges;
			_listPaySplits=loadData.ListTempPaySplit;
			_listPayments=loadData.ListPayments;
			_listInsPayAsTotal=loadData.ListInsPayAsTotal;
			_listClaimProcs=loadData.ListClaimProcs;
			textCode.Text=Lan.g(this,"None");
			FillGrid();
			if(!Security.IsAuthorized(Permissions.PayPlanEdit,true)) {
				this.DisableForm(butCancel,checkHideUnattached,checkShowImplicit,butPrint,gridMain);
			}
		}

		private void CreatePayPlanEntries(bool showImplicitlyPaidOffProcs=false) {
			_listAccountCharges=AccountEntry.GetAccountCharges(_listPayPlanCharges,_listAdjustments,_listProcs);
			if(showImplicitlyPaidOffProcs) {
				_accountCredits=0;
			}
			else {
				_accountCredits=PayPlanEdit.GetAccountCredits(_listAdjustments,_listPaySplits,_listPayments,_listInsPayAsTotal,_listPayPlanCharges);
			}
			LinkChargesToCredits();
			_listPayPlanEntries=new List<PayPlanEdit.PayPlanEntry>();
			_listPayPlanEntries.AddRange(PayPlanEdit.CreatePayPlanEntriesForAccountCharges(_listAccountCharges,_patCur));
			ListPayPlanCreditsCur=ListPayPlanCreditsCur.OrderBy(x => x.ChargeDate).ToList();
			_listPayPlanEntries.AddRange(PayPlanEdit.CreatePayPlanEntriesForPayPlanCredits(_listAccountCharges,ListPayPlanCreditsCur));
			if(ListPayPlanCreditsCur.Exists(x => x.ProcNum==0)) {//only add "Unattached" if there is a credit without a procedure. 
				_listPayPlanEntries.Add(PayPlanEdit.CreatePayPlanEntryForUnattachedProcs(ListPayPlanCreditsCur,_patCur.FName));
			}
			_listPayPlanEntries=_listPayPlanEntries
				.OrderByDescending(x => x.ProcStatOrd)
				.ThenByDescending(x => x.ProcNumOrd)
				.ThenBy(x => x.IsChargeOrd)
				.ThenBy(x => x.DateOrd).ToList();
		}

		private void FillGrid() {
			if(checkShowImplicit.Checked) {
				CreatePayPlanEntries(true);
			}
			else {
				CreatePayPlanEntries();
			}
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col;
			col=new GridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Date"),70);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Provider"),65);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Stat"),30);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Code"),70);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Fee"),55,HorizontalAlignment.Right);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Rem Before"),70,HorizontalAlignment.Right);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Credit Date"),70);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Amount"),55,HorizontalAlignment.Right);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Rem After"),60,HorizontalAlignment.Right);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Note"),0);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			double totalAttached=0;
			foreach(PayPlanEdit.PayPlanEntry entryCur in _listPayPlanEntries) { //for all account charges
				if(checkHideUnattached.Checked && !ListPayPlanCreditsCur.Exists(x => x.ProcNum==entryCur.ProcNumOrd)) {
					continue;
				}
				row=new GridRow();
				//we color the relevant cells to make the table easier to read.
				//the colors have been looked-at and approved by colourblind Josh.
				//In the future, we will probably make these customizable definitions.
				GridCell cell=new GridCell(entryCur.DateStr);
				//for procedure rows, cell color should be LightYellow for all relevant fields.
				cell.ColorBackG=entryCur.IsChargeOrd ? Color.White : Color.LightYellow;
				row.Cells.Add(cell);
				cell=new GridCell(entryCur.ProvAbbr);
				cell.ColorBackG=entryCur.IsChargeOrd ? Color.White : Color.LightYellow;
				row.Cells.Add(cell);
				cell=new GridCell(entryCur.StatStr);
				cell.ColorBackG=entryCur.IsChargeOrd ? Color.White : Color.LightYellow;
				row.Cells.Add(cell);
				cell=new GridCell(entryCur.ProcStr);
				cell.ColorBackG=entryCur.IsChargeOrd ? Color.White : Color.LightYellow;
				row.Cells.Add(cell);
				cell=new GridCell(entryCur.FeeStr);
				cell.ColorBackG=entryCur.IsChargeOrd ? Color.White : Color.LightYellow;
				row.Cells.Add(cell);
				cell=new GridCell(entryCur.RemBefStr);
				cell.ColorBackG=entryCur.IsChargeOrd ? Color.White : Color.LightYellow;
				row.Cells.Add(cell);
				cell=new GridCell(entryCur.CredDateStr);
				//for charge rows, cell color should be LightCyan for all relevant fields.
				cell.ColorBackG=entryCur.IsChargeOrd ? Color.LightCyan : Color.White;
				row.Cells.Add(cell);
				cell=new GridCell(entryCur.AmtStr);
				cell.ColorBackG=entryCur.IsChargeOrd ? Color.LightCyan : Color.White;
				row.Cells.Add(cell);
				totalAttached+=PIn.Double(entryCur.AmtStr);
				cell=new GridCell(entryCur.RemAftStr);
				cell.ColorBackG=entryCur.IsChargeOrd ? Color.LightCyan : Color.White;
				row.Cells.Add(cell);
				cell=new GridCell(entryCur.NoteStr);
				cell.ColorBackG=entryCur.IsChargeOrd ? Color.LightCyan : Color.White;
				row.Cells.Add(cell);
				row.Tag=entryCur;
				if(!entryCur.IsChargeOrd) {
					row.ColorLborder=Color.Black;
				}
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
			textTotal.Text=totalAttached.ToString("f");
		}

		

		///<summary>Links charges to credits explicitly based on FKs first, then implicitly based on Date.</summary>
		private void LinkChargesToCredits() {
			PayPlanEdit.PayPlanLinked linkData=PayPlanEdit.LinkChargesToCredits(_listAccountCharges,_listPaySplits,_listAdjustments,_listPayPlanCharges
				,_listClaimProcs,_payPlanCur.PayPlanNum,_accountCredits);
			_listAccountCharges=linkData.ListAccountCharges;
			_accountCredits=linkData.AccountCredits;
		}

		private void SetTextBoxes() {
			List<PayPlanEdit.PayPlanEntry> listSelectedEntries=new List<PayPlanEdit.PayPlanEntry>();
			for(int i=0;i < gridMain.SelectedIndices.Count();i++) { //fill the list with all the selected items in the grid.
				listSelectedEntries.Add((PayPlanEdit.PayPlanEntry)(gridMain.ListGridRows[gridMain.SelectedIndices[i]].Tag));
			}
			bool isUpdateButton=false;//keep track of the state of the button, if it is add or update. 
			if(listSelectedEntries.Count==0) { //if there are no entries selected
				//button should say Add, textboxes should be editable. No attached procedure.
				butAddOrUpdate.Text=Lan.g(this,"Add");
				textAmt.Text="";
				textDate.Text="";
				textCode.Text=Lan.g(this,"None");
				textNote.Text="";
				textAmt.ReadOnly=false;
				textDate.ReadOnly=false;
				textNote.ReadOnly=false;
			}
			else if(listSelectedEntries.Count==1) { //if there is one entry selected
				PayPlanEdit.PayPlanEntry selectedEntry=listSelectedEntries[0]; //all textboxes should be editable
				textAmt.ReadOnly=false;
				textDate.ReadOnly=false;
				textNote.ReadOnly=false;
				if(selectedEntry.IsChargeOrd) { //if it's a PayPlanCharge
					//button should say Update, text boxes should fill with info from that charge.
					butAddOrUpdate.Text=Lan.g(this,"Update");
					isUpdateButton=true;
					textAmt.Text=selectedEntry.AmtStr;
					textNote.Text=selectedEntry.NoteStr;
					if(selectedEntry.ProcStatOrd==ProcStat.TP && selectedEntry.ProcNumOrd!=0) {//if tp, grey out the date textbox. it should always be maxvalue.
						//tp and procnum==0 means that it's the unattached row, in which case we don't want to make the text boxes ready-only.
						textDate.ReadOnly=true;
						textDate.Text="";
					}
					else {
						textDate.Text=selectedEntry.CredDateStr;
					}
					if(selectedEntry.Proc==null) { //selected charge could be unattached.
						textCode.Text=Lan.g(this,"Unattached");
					}
					else {
						textCode.Text=ProcedureCodes.GetStringProcCode(selectedEntry.Proc.CodeNum);
					}
				}
				else {// selected line item is a procedure (or the "Unattached" entry)
					//button should say "Add", text boxes should fill with info from that procedure (or "unattached").
					butAddOrUpdate.Text=Lan.g(this,"Add");
					if(selectedEntry.Proc==null) {
						textCode.Text=Lan.g(this,"Unattached");
						textAmt.Text="0.00";
						textNote.Text="";
						textDate.Text=DateTimeOD.Today.ToShortDateString();
					}
					else { //if it is a procedure (and not the "unattached" row)
						List<PayPlanEdit.PayPlanEntry> listEntriesForProc=_listPayPlanEntries
						.Where(x => x.ProcNumOrd==selectedEntry.ProcNumOrd)
						.Where(x => x.IsChargeOrd==true).ToList();
						if(listEntriesForProc.Count==0) { //if there are no other charges attached to the procedure
							textAmt.Text=selectedEntry.RemBefStr; //set textAmt to the value in RemBefore
						}
						else {//if there are other charges attached, fill the amount textbox with the minimum value in the RemAftr column.
							textAmt.Text=listEntriesForProc.Min(x => PIn.Double(x.RemAftStr)).ToString("f");
						}
						textDate.Text=DateTimeOD.Today.ToShortDateString();
						textNote.Text=ProcedureCodes.GetStringProcCode(selectedEntry.Proc.CodeNum)+": "+Procedures.GetDescription(selectedEntry.Proc);
						textCode.Text=ProcedureCodes.GetStringProcCode(selectedEntry.Proc.CodeNum);
					}
				}
			}
			else if(listSelectedEntries.Count>1) { //if they selected multiple line items
				//change the button to say "add"
				//blank out and make read-only all text boxes.
				butAddOrUpdate.Text=Lan.g(this,"Add");
				textAmt.Text="";
				textDate.Text="";
				textNote.Text="";
				textCode.Text=Lan.g(this,"Multiple");
				textAmt.ReadOnly=true;
				textDate.ReadOnly=true;
				textNote.ReadOnly=true;
			}
			if(listSelectedEntries.Any(x => Security.IsGlobalDateLock(Permissions.PayPlanEdit,x.DateOrd,true))) {
				if(isUpdateButton) {
					butAddOrUpdate.Enabled=false;//only disallow them from updating a tx credit, adding a new one is okay. 
				}
				else {
					butAddOrUpdate.Enabled=true;
				}
				butDelete.Enabled=false;
			}
			else {
				butAddOrUpdate.Enabled=true;
				butDelete.Enabled=true;
			}
		}

		private void gridMain_MouseUp(object sender,MouseEventArgs e) {
			SetTextBoxes();
		}

		private void butClear_Click(object sender,EventArgs e) {
			gridMain.SetSelected(false);
			SetTextBoxes();
			//txtAmt does not need this because validation is done on text changed, textDate validation logic is not linked to text change so we will 
			//manually clear error since there is no text and current control logic would result in the error being cleared when empty.
			textDate.errorProvider1.Clear();
		}

		private void butAddOrUpdate_Click(object sender,EventArgs e) {
			List<PayPlanEdit.PayPlanEntry> listSelectedEntries=new List<PayPlanEdit.PayPlanEntry>();
			for(int i=0;i < gridMain.SelectedIndices.Count();i++) { //add all of the currently selected entries to this list.
				listSelectedEntries.Add((PayPlanEdit.PayPlanEntry)(gridMain.ListGridRows[gridMain.SelectedIndices[i]].Tag));
			}
			if(listSelectedEntries.Count<=1) { //validation (doesn't matter if multiple are selected)
				if(string.IsNullOrEmpty(textAmt.Text) || textAmt.errorProvider1.GetError(textAmt)!="" || PIn.Double(textAmt.Text)==0) {
					MsgBox.Show(this,"Please enter a valid amount.");
					return;
				}
				if(textDate.Text!="" && textDate.errorProvider1.GetError(textDate)!="") {
					MsgBox.Show(this,"Please enter a valid date.");
					return;
				}
			}
			if(textDate.Text=="") {
				textDate.Text=DateTime.Today.ToShortDateString();
			}
			if(Security.IsGlobalDateLock(Permissions.PayPlanEdit,PIn.Date(textDate.Text))) {
				return;
			}
			if(listSelectedEntries.Count==0 ) {
				if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) { //if they have none selected
					MsgBox.Show(this,"All treatment credits (excluding adjustments) must have a procedure.");
					return;
				}
				//add an unattached charge only if not on enforce fully
				PayPlanCharge addCharge=PayPlanEdit.CreateUnattachedCredit(textDate.Text,_patCur.PatNum,textNote.Text,_payPlanCur.PayPlanNum,
					PIn.Double(textAmt.Text));
				ListPayPlanCreditsCur.Add(addCharge);
			}
			else if(listSelectedEntries.Count==1) { //if they have one selected
				PayPlanEdit.PayPlanEntry selectedEntry=listSelectedEntries[0];
				if(selectedEntry.Proc!=null && _listPayPlanLinksForProcs.Select(x => x.FKey).Contains(selectedEntry.Proc.ProcNum)) {
					MsgBox.Show(this,"This procedure is already linked to a dynamic payment plan.");
					return;
				}
				if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
					if((selectedEntry.Proc==null || selectedEntry.Proc.ProcNum==0)
						&& !(selectedEntry.Charge!=null && selectedEntry.Charge.IsCreditAdjustment)) 
					{
						MsgBox.Show(this,"All treatment credits (excluding adjustments) must have a procedure.");
						return;
					}
				}
				PayPlanCharge selectedCharge=new PayPlanCharge();
				if(selectedEntry.IsChargeOrd) {
					//get the charge from the grid.
					//DO NOT use PayPlanChargeNum. They are not pre-inserted so they will all be 0 if new.
					selectedCharge=((PayPlanEdit.PayPlanEntry)(gridMain.ListGridRows[gridMain.SelectedIndices[0]].Tag)).Charge;
				}
				ListPayPlanCreditsCur=PayPlanEdit.CreateOrUpdateChargeForSelectedEntry(selectedEntry,ListPayPlanCreditsCur,PIn.Double(textAmt.Text),textNote.Text,textDate.Text
					,_patCur.PatNum,_payPlanCur.PayPlanNum,selectedCharge);
			}
			else if(listSelectedEntries.Count>1) { //if they have more than one entry selected
				//remove everythig that doesn't have a procnum from the list
				List<PayPlanEdit.PayPlanEntry> listSelectedProcs=listSelectedEntries.Where(x => !x.IsChargeOrd).Where(x => x.Proc != null).ToList();
				if(listSelectedEntries.Count==0) { //if the list is then empty, there's nothing to do.
					MsgBox.Show(this,"You must have at least one procedure selected.");
					return;
				}
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel, 
					"Add a payment plan credit for each of the selected procedure's remaining amount?  Selected credits will be ignored.")) {
					return;
				}
				List<PayPlanEdit.PayPlanEntry> listValidSelectedProcs=
					listSelectedProcs.FindAll(x => !_listPayPlanLinksForProcs.Select(y => y.FKey).Contains(x.Proc.ProcNum));
				int countProcsSkipped=listSelectedProcs.Count-listValidSelectedProcs.Count;
				ListPayPlanCreditsCur=PayPlanEdit.CreateCreditsForAllSelectedEntries(listValidSelectedProcs,_listPayPlanEntries,DateTimeOD.Today
					,_patCur.PatNum,_payPlanCur.PayPlanNum,ListPayPlanCreditsCur);
				if(countProcsSkipped>0) {
					MsgBox.Show(this,"Credits were not made for "+countProcsSkipped+" procedure(s) because they are linked to one or more dynamic payment plans.");
				}
			}
			textAmt.Text="";
			textDate.Text="";
			textNote.Text="";
			FillGrid();
			SetTextBoxes();
		}

		private void checkHideUnattached_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
			SetTextBoxes();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			headingPrinted=false;
			PrinterL.TryPrintOrDebugRpPreview(pd_PrintPage,Lan.g(this,"Outstanding insurance report printed"),PrintoutOrientation.Landscape);
		}

		private void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			//new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text=Lan.g(this,"Payment Plan Credits");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=DateTime.Today.ToShortDateString();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=_patCur.LName+", "+_patCur.FName;
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			yPos=gridMain.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
				text=Lan.g(this,"Total")+": "+PIn.Double(textTotal.Text).ToString("c");
				g.DrawString(text,subHeadingFont,Brushes.Black,center+gridMain.Width/2-g.MeasureString(text,subHeadingFont).Width-10,yPos);
			}
			g.Dispose();
		}

		private void checkShowImplicit_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			List<PayPlanEdit.PayPlanEntry> listSelectedEntries=new List<PayPlanEdit.PayPlanEntry>();
			for(int i=0;i < gridMain.SelectedIndices.Count();i++) {
				listSelectedEntries.Add((PayPlanEdit.PayPlanEntry)(gridMain.ListGridRows[gridMain.SelectedIndices[i]].Tag));
			}
			List<PayPlanCharge> listSelectedCharges=listSelectedEntries.Where(x => x.Charge != null).Select(x => x.Charge).ToList();
			//remove all procedures from the list. you cannot delete procedures from here.
			if(listSelectedCharges.Count<1) {
				MsgBox.Show(this,"You must have at least one payment plan charge selected.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Delete selected payment plan charges?")) {
				return;
			}
			foreach(PayPlanCharge chargeCur in listSelectedCharges) {
				ListPayPlanCreditsCur.Remove(chargeCur);
			}
			FillGrid();
			SetTextBoxes();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
				//If no procs attached and not an adjustment with a negative amount
				if(ListPayPlanCreditsCur.Any(x => x.ProcNum==0 && !x.IsCreditAdjustment)) {
					MsgBox.Show(this,"All treatment credits (excluding adjustments) must have a procedure.");
					return;
				}
			}
			//find if any charges exist for attached credits that have a provider other than the provdier on the payment plan. Unattached credits will be 0.
			if(ListPayPlanCreditsCur.Count>0 && _listPayPlanEntries.FindAll(x => x.Charge!=null && x.ProvNum!=0).Any(x => x.ProvNum!=_provNum)) {
				//All credits go to the provider on the payment plan. We need to check if the procedure's provider does not match.
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Provider(s) for procedure credits do not match the provider on the payment plan. Continue?")) {
					return;
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
