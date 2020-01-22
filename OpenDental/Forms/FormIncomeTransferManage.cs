using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormIncomeTransferManage:ODForm {
		///<summary>List of current paysplits for this payment.</summary>
		private List<PaySplit> _listSplitsCur;
		private Family _famCur;
		private Patient _patCur;
		///<summary>Payment from the Payment window, amount gets modified only in the case that the original paymentAmt is zero.  It gets increased to
		///whatever the paysplit total is when the window is closed.</summary>
		private Payment _paymentCur;
		private List<PaySplits.PaySplitAssociated> _listSplitsAssociated;
		private PaymentEdit.ConstructResults _results;
		///<summary>A dictionary or patients that we may need to reference to fill the grids to eliminate unnecessary calls to the DB.
		///Should contain all patients in the current family along with any patients of payment plans of which a member of this family is the guarantor.</summary>
		private Dictionary<long,Patient> _dictPatients;
		///<summary>Gets set when an unallocated transfer has been made automatically and inserted upon entering this window.</summary>
		private long _unallocatedPayNum=0;
		///<summary>Stores the claimpay transfer (combination of claimprocs and paysplits) that was inserted into the database upon success.</summary>
		private ClaimTransferResult _claimTransferResult;

		private List<long> _listFamilyPatNums {
			get {
				return _famCur.ListPats.Select(x => x.PatNum).ToList();
			}
		}

		public FormIncomeTransferManage(Family famCur,Patient patCur,Payment payCur) {
			_famCur=famCur;
			_patCur=patCur;
			_paymentCur=payCur;
			InitializeComponent();
			Lan.F(this);
		}

		private void FormIncomeTransferManage_Load(object sender,EventArgs e) {
			if(Security.IsAuthorized(Permissions.PaymentCreate,true)) {
				//Anything else added to the logic that runs in this form potentially needs to be added to the family balancer tool as well.
				TransferUnallocatedToUnearned();
				TransferClaimsPayAsTotal();//intentionally checked by the payment create permission even though it is claim supplementals (InsPayCreate).
			}
			Init();
		}

		///<summary></summary>
		private void TransferUnallocatedToUnearned() {
			List<PaySplit> listSplitsForPats=PaySplits.GetForPats(_listFamilyPatNums);
			if(listSplitsForPats.IsNullOrEmpty()) {
				return;
			}
			//Pass in an invalid payNum of 0 which will get set correctly later if there are in fact splits to transfer.
			PaymentEdit.IncomeTransferData unallocatedTransfers=PaymentEdit.TransferUnallocatedSplitToUnearned(listSplitsForPats,0);
			if(unallocatedTransfers.ListSplitsCur.Count==0) {
				return;
			}
			if(!unallocatedTransfers.ListSplitsCur.Sum(x => x.SplitAmt).IsZero()) {
				//Display the UnearnedType and the SplitAmt for each split in the list.
				string splitInfo=string.Join("\r\n  ",unallocatedTransfers.ListSplitsCur
					.Select(x => $"SplitAmt: {x.SplitAmt}"));
				//Show the sum of all splits first and then give a breakdown of each individual split.
				string details=$"Sum of unallocatedTransfers.ListSplitsCur: {unallocatedTransfers.ListSplitsCur.Sum(x => x.SplitAmt)}\r\n"
					+$"Individual Split Info:\r\n  {splitInfo}";
				FriendlyException.Show("Error transferring unallocated paysplits.  Please call support.",new ApplicationException(details),"Close");
				//Close the window and do not let the user create transfers because something is wrong.
				DialogResult=DialogResult.Cancel;
				Close();
				return;
			}
			//There are unallocated paysplits that need to be transferred to unearned.
			_unallocatedPayNum=PaymentEdit.CreateAndInsertUnallocatedPayment(_patCur);
			foreach(PaySplit split in unallocatedTransfers.ListSplitsCur) {
				split.PayNum=_unallocatedPayNum;//Set the PayNum because it was purposefully set to 0 above to save queries.
				PaySplits.Insert(split);
			}
			foreach(PaySplits.PaySplitAssociated splitAssociated in unallocatedTransfers.ListSplitsAssociated) {
				if(splitAssociated.PaySplitLinked!=null && splitAssociated.PaySplitOrig!=null) {
					PaySplits.UpdateFSplitNum(splitAssociated.PaySplitOrig.SplitNum,splitAssociated.PaySplitLinked.SplitNum);
				}
			}
			SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,_patCur.PatNum
				,$"Unallocated splits automatically transferred to unearned for payment {_unallocatedPayNum}.");
		}

		private void TransferClaimsPayAsTotal() {
			ClaimProcs.FixClaimsNoProcedures(_listFamilyPatNums);
			try {
				_claimTransferResult=ClaimProcs.TransferClaimsAsTotalToProcedures(_listFamilyPatNums);
			}
			catch(ApplicationException ex) {
				FriendlyException.Show(ex.Message,ex);
				return;
			}
			if(_claimTransferResult!=null && _claimTransferResult.ListInsertedClaimProcs.Count > 0) {//valid and items were created
				SecurityLogs.MakeLogEntry(Permissions.ClaimProcReceivedEdit,_patCur.PatNum,"Automatic transfer of claims pay as total from income transfer.");
			}
		}

		///<summary>Performs all of the Load functionality.</summary>
		private void Init() {
			_listSplitsCur=new List<PaySplit>();
			_listSplitsAssociated=new List<PaySplits.PaySplitAssociated>();
			_dictPatients=Patients.GetAssociatedPatients(_patCur.PatNum).ToDictionary(x => x.PatNum,x => x);
			_results=PaymentEdit.ConstructAndLinkChargeCredits(_famCur.ListPats.Select(x => x.PatNum).ToList(),_patCur.PatNum,new List<PaySplit>(),
				_paymentCur,new List<AccountEntry>(),true,false);
			FillGridSplits();
		}

		///<summary>Fills the paysplit grid.</summary>
		private void FillGridSplits() {
			//Fill left grid with paysplits created
			gridSplits.BeginUpdate();
			gridSplits.ListGridColumns.Clear();
			gridSplits.ListGridColumns.Add(new GridColumn(Lan.g(this,"Date"),65,HorizontalAlignment.Center));
			gridSplits.ListGridColumns.Add(new GridColumn(Lan.g(this,"Prov"),40));
			if(PrefC.HasClinicsEnabled) {//Clinics
				gridSplits.ListGridColumns.Add(new GridColumn(Lan.g(this,"Clinic"),40));
			}
			gridSplits.ListGridColumns.Add(new GridColumn(Lan.g(this,"Patient"),100));
			gridSplits.ListGridColumns.Add(new GridColumn(Lan.g(this,"ProcCode"),60));
			gridSplits.ListGridColumns.Add(new GridColumn(Lan.g(this,"Type"),100));
			gridSplits.ListGridColumns.Add(new GridColumn(Lan.g(this,"Amount"),55,HorizontalAlignment.Right));
			gridSplits.ListGridRows.Clear();
			GridRow row;
			decimal splitTotal=0;
			Dictionary<long,Procedure> dictProcs=Procedures.GetManyProc(_listSplitsCur.Where(x => x.ProcNum>0).Select(x => x.ProcNum).Distinct().ToList(),false).ToDictionary(x => x.ProcNum);
			for(int i=0;i<_listSplitsCur.Count;i++) {
				splitTotal+=(decimal)_listSplitsCur[i].SplitAmt;
				row=new GridRow();
				row.Tag=_listSplitsCur[i];
				row.Cells.Add(_listSplitsCur[i].DatePay.ToShortDateString());//Date
				row.Cells.Add(Providers.GetAbbr(_listSplitsCur[i].ProvNum));//Prov
				if(PrefC.HasClinicsEnabled) {//Clinics
					if(_listSplitsCur[i].ClinicNum!=0) {
						row.Cells.Add(Clinics.GetClinic(_listSplitsCur[i].ClinicNum).Description);//Clinic
					}
					else {
						row.Cells.Add("");//Clinic
					}
				}
				Patient patCur;
				if(!_dictPatients.TryGetValue(_listSplitsCur[i].PatNum,out patCur)) {
					patCur=Patients.GetPat(_listSplitsCur[i].PatNum);
				}
				row.Cells.Add(patCur.GetNameFL());//Patient
				Procedure proc=new Procedure();
				if(_listSplitsCur[i].ProcNum>0 && !dictProcs.TryGetValue(_listSplitsCur[i].ProcNum,out proc)) {
					proc=Procedures.GetOneProc(_listSplitsCur[i].ProcNum,false);
				}
				row.Cells.Add(ProcedureCodes.GetStringProcCode(proc?.CodeNum??0));//ProcCode
				string type="";
				if(_listSplitsCur[i].PayPlanNum!=0) {
					type+="PayPlanCharge";//Type
					if(_listSplitsCur[i].IsInterestSplit && _listSplitsCur[i].ProcNum==0 && _listSplitsCur[i].ProvNum!=0) {
						type+=" (interest)";
					}
				}
				if(_listSplitsCur[i].ProcNum!=0) {//Procedure
					string procDesc=Procedures.GetDescription(proc??new Procedure());
					if(type!="") {
						type+="\r\n";
					}
					type+="Proc: "+procDesc;//Type
				}
				if(_listSplitsCur[i].ProvNum==0) {//Unattached split
					if(type!="") {
						type+="\r\n";
					}
					type+="Unallocated";//Type
				}
				if(_listSplitsCur[i].ProvNum!=0 && _listSplitsCur[i].UnearnedType!=0) {
					if(type!="") {
						type+="\r\n";
					}
					type+=Defs.GetDef(DefCat.PaySplitUnearnedType,_listSplitsCur[i].UnearnedType).ItemName;
				}
				row.Cells.Add(type);
				if(row.Cells[row.Cells.Count-1].Text=="Unallocated") {
					row.Cells[row.Cells.Count-1].ColorText=Color.Red;
				}
				row.Cells.Add(_listSplitsCur[i].SplitAmt.ToString("f"));//Amount
				gridSplits.ListGridRows.Add(row);
			}
			gridSplits.EndUpdate();
			textSplitTotal.Text=splitTotal.ToString("f");
			FillGridCharges();
		}

		///<summary>Fills charge grid, and then split grid.</summary>
		private void FillGridCharges() {
			//Fill right-hand grid with all the charges, filtered based on checkbox and filters.
			List<int> listExpandedRows=new List<int>();
			for(int i=0;i<gridCharges.ListGridRows.Count;i++) {
				if(gridCharges.ListGridRows[i].State.DropDownState==ODGridDropDownState.Down) {
					listExpandedRows.Add(i);//Keep track of expanded rows
				}
			}
			gridCharges.BeginUpdate();
			gridCharges.ListGridColumns.Clear();
			gridCharges.ListGridColumns.Add(new GridColumn(Lan.g(this,"Prov"),100));
			gridCharges.ListGridColumns.Add(new GridColumn(Lan.g(this,"Patient"),100));
			if(PrefC.HasClinicsEnabled) {
				gridCharges.ListGridColumns.Add(new GridColumn(Lan.g(this,"Clinic"),60));
			}
			gridCharges.ListGridColumns.Add(new GridColumn(Lan.g(this,"Codes"),-200));//negative so it will dynamically grow when no clinic column is present
			gridCharges.ListGridColumns.Add(new GridColumn(Lan.g(this,"Amt Orig"),61,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridCharges.ListGridColumns.Add(new GridColumn(Lan.g(this,"Amt Start"),61,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridCharges.ListGridColumns.Add(new GridColumn(Lan.g(this,"Amt End"),61,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridCharges.ListGridRows.Clear();
			decimal chargeTotal=0;
			//Item1=ProvNum,Item2=ClinicNum,Item3=PatNum
			List<Tuple<long,long,long>> listAddedProvNums=new List<Tuple<long,long,long>>();//this needs to be prov/clinic/patnum
			List<Tuple<long,long,long>> listAddedParents=new List<Tuple<long,long,long>>();//prov/clinic/pat
			foreach(AccountEntry entryCharge in _results.ListAccountCharges) {
				if(Math.Round(entryCharge.AmountStart,3)==0) {
					continue;
				}
				if(listAddedProvNums.Any(x => x.Item1==entryCharge.ProvNum && x.Item2==entryCharge.ClinicNum && x.Item3==entryCharge.PatNum)) {
					continue;
				}
				listAddedProvNums.Add(Tuple.Create(entryCharge.ProvNum,entryCharge.ClinicNum,entryCharge.PatNum));
				List<AccountEntry> listEntriesForProvAndClinicAndPatient=_results.ListAccountCharges
					.FindAll(x => x.ProvNum==entryCharge.ProvNum && x.ClinicNum==entryCharge.ClinicNum && x.PatNum==entryCharge.PatNum);
				List<GridRow> listRows=CreateChargeRows(listEntriesForProvAndClinicAndPatient,listExpandedRows);
				foreach(GridRow row in listRows) {
					AccountEntry accountEntry=(AccountEntry)row.Tag;
					if(accountEntry.Tag==null) {//Parent row
						chargeTotal+=PIn.Decimal(row.Cells[row.Cells.Count-1].Text);
						listAddedParents.Add(Tuple.Create(accountEntry.ProvNum,accountEntry.ClinicNum,accountEntry.PatNum));
					}
					else if(!listAddedParents.Exists(x => x.Item1==accountEntry.ProvNum && x.Item2==accountEntry.ClinicNum && x.Item3==accountEntry.PatNum)) {//In case a parent AND child are selected, don't add child amounts if parent was added already
						chargeTotal+=accountEntry.AmountEnd;
					}
					gridCharges.ListGridRows.Add(row);
				}
			}
			gridCharges.EndUpdate();
			textChargeTotal.Text=chargeTotal.ToString("f");
		}

		private List<GridRow> CreateChargeRows(List<AccountEntry> listEntries,List<int> listExpandedRows) {
			List<GridRow> listRows=new List<GridRow>();
			//Parent row
			GridRow row=new GridRow();
			//A placeholder accountEntry.  The Tag being null tells us it's a parent row.
			row.Tag=new AccountEntry() { ProvNum=listEntries[0].ProvNum,PatNum=listEntries[0].PatNum,ClinicNum=listEntries[0].ClinicNum };
			row.Cells.Add(Providers.GetAbbr(listEntries[0].ProvNum));//Provider
			Patient pat;
			if(!_dictPatients.TryGetValue(listEntries[0].PatNum,out pat)) {
				pat=Patients.GetLim(listEntries[0].PatNum);
				_dictPatients[pat.PatNum]=pat;
			}
			row.Cells.Add(pat.LName+", "+pat.FName);//Patient
			if(PrefC.HasClinicsEnabled) {
				row.Cells.Add(Clinics.GetAbbr(listEntries[0].ClinicNum));
			}
			string procCodes="";
			int addedCodes=0;
			for(int i=0;i<listEntries.Count;i++) {
				if(Math.Round(listEntries[i].AmountStart,3)==0) {
					continue;
				}
				if(listEntries[i].GetType()==typeof(Procedure)) {
					if(procCodes!="") {
						procCodes+=", ";
					}
					procCodes+=ProcedureCodes.GetStringProcCode(((Procedure)listEntries[i].Tag).CodeNum);
					addedCodes++;
				}
				else if(listEntries[i].GetType()==typeof(PaySplit) && ((PaySplit)listEntries[i].Tag).ProcNum!=0) {
					if(procCodes!="") {
						procCodes+=", ";
					}
					//Look for the procedure in AccountEntries (it should be there) - We don't want to go to the DB
					AccountEntry entry=_results.ListAccountCharges.Find(x => x.GetType()==typeof(Procedure) && x.PriKey==((PaySplit)listEntries[i].Tag).ProcNum);
					if(entry!=null) {//Can be null if paysplit is on a procedure that's outside the family.  
						procCodes+=ProcedureCodes.GetStringProcCode(((Procedure)entry.Tag).CodeNum);
						addedCodes++;
					}
				}
				else if(listEntries[i].GetType()==typeof(Adjustment) && ((Adjustment)listEntries[i].Tag).ProcNum!=0) {
					if(procCodes!="") {
						procCodes+=", ";
					}
					//Look for the procedure in AccountEntries (it should be there) - We don't want to go to the DB
					AccountEntry entry=_results.ListAccountCharges.Find(x => x.GetType()==typeof(Procedure) && x.PriKey==((Adjustment)listEntries[i].Tag).ProcNum);
					if(entry!=null) {//Can be null if paysplit is on a procedure that's outside the family.  
						procCodes+=ProcedureCodes.GetStringProcCode(((Procedure)entry.Tag).CodeNum);
						addedCodes++;
					}
				}
				if(addedCodes==9) {//1 less than above, this column is shorter when filtering by prov + clinic
					procCodes+=", (...)";
					break;
				}
			}
			row.Cells.Add(procCodes);
			//parent row should be sum of all child rows, and we skip entries that have an amount start of 0 for children so skip for parent as well. 
			List<AccountEntry> listEntriesWithAmountStart=listEntries.FindAll(x => !x.AmountStart.IsEqual(0));
			row.Cells.Add(listEntriesWithAmountStart.Sum(x => x.AmountOriginal).ToString("f"));//Amount Original
			row.Cells.Add(listEntriesWithAmountStart.Sum(x => x.AmountStart).ToString("f"));//Amount Start
			row.Cells.Add(listEntriesWithAmountStart.Sum(x => x.AmountEnd).ToString("f"));//Amount End
			row.Bold=true;
			listRows.Add(row);
			if(listExpandedRows.Contains(listRows.Count-1)) {
				row.DropDownInitiallyDown=true;//If the expanded rows contains the one we just added, expand it
			}
			//Child rows
			foreach(AccountEntry entryCharge in listEntries) {
				if(Math.Round(entryCharge.AmountStart,3)==0) {
					continue;
				}
				GridRow childRow=new GridRow();
				childRow.Tag=entryCharge; 
				childRow.Cells.Add("     "+Providers.GetAbbr(entryCharge.ProvNum));//Provider
				childRow.Cells.Add(pat.LName+", "+pat.FName);//Patient
				if(PrefC.HasClinicsEnabled) {
					childRow.Cells.Add(Clinics.GetAbbr(entryCharge.ClinicNum));
				}
				procCodes="";
				if(entryCharge.GetType()==typeof(Procedure)) {
					procCodes=((Procedure)entryCharge.Tag).ProcDate.ToShortDateString()+" "+ProcedureCodes.GetStringProcCode(((Procedure)entryCharge.Tag).CodeNum);
				}
				else if(entryCharge.GetType()==typeof(PayPlanCharge)) {
					procCodes="PayPlanCharge";
				}
				else if(entryCharge.GetType()==typeof(PaySplit) && ((PaySplit)entryCharge.Tag).UnearnedType!=0) {
					PaySplit split=(PaySplit)entryCharge.Tag;
					if(split.ProvNum==0) {
						procCodes=entryCharge.Date.ToShortDateString()+" Unallocated";
					}
					else {
						procCodes=entryCharge.Date.ToShortDateString()+" "+Defs.GetName(DefCat.PaySplitUnearnedType,split.UnearnedType);
					}
				}
				else if(entryCharge.GetType()==typeof(PaySplit) && ((PaySplit)entryCharge.Tag).ProcNum!=0) {
					AccountEntry entry=_results.ListAccountCharges.Find(x => x.GetType()==typeof(Procedure) && x.PriKey==((PaySplit)entryCharge.Tag).ProcNum);
					if(entry!=null) {//Entry can be null if there is a split for X patient and the attached procedure is for Y patient outside family.  Possible with old data.
						procCodes=entryCharge.Date.ToShortDateString()+" PaySplit: "+ProcedureCodes.GetStringProcCode(((Procedure)entry.Tag).CodeNum);
					}
				}
				else if(entryCharge.GetType()==typeof(Adjustment) && ((Adjustment)entryCharge.Tag).ProcNum!=0) {
					AccountEntry entry=_results.ListAccountCharges.Find(x => x.GetType()==typeof(Procedure) && x.PriKey==((Adjustment)entryCharge.Tag).ProcNum);
					if(entry!=null) {//Entry can be null if there is a split for X patient and the attached procedure is for Y patient outside family.  Possible with old data.
						procCodes=entryCharge.Date.ToShortDateString()+" Adjustment: "+ProcedureCodes.GetStringProcCode(((Procedure)entry.Tag).CodeNum);
					}
				}
				else if(entryCharge.GetType()==typeof(PaySplit) && ((PaySplit)entryCharge.Tag).PayPlanNum!=0) {//Don't need this really
					procCodes=entryCharge.Date.ToShortDateString()+" PayPlan Split";
				}
				else if(entryCharge.GetType()==typeof(Adjustment)) {
					procCodes=entryCharge.Date.ToShortDateString()+" Adjustment";
				}
				else if(entryCharge.GetType()==typeof(ClaimProc)) {
					procCodes=entryCharge.Date.ToShortDateString()+" Claim Payment";
				}
				else {
					procCodes=entryCharge.Date.ToShortDateString()+" "+entryCharge.GetType().Name;
				}
				childRow.Cells.Add(procCodes);//ProcCodes
				childRow.Cells.Add(entryCharge.AmountOriginal.ToString("f"));//Amount Original
				childRow.Cells.Add(entryCharge.AmountStart.ToString("f"));//Amount Start
				childRow.Cells.Add(entryCharge.AmountEnd.ToString("f"));//Amount End
				childRow.DropDownParent=row;
				listRows.Add(childRow);
			}
			return listRows;
		}

		///<summary>Creates micro-allocations intelligently based on most to least matching criteria of selected charges.</summary>
		private void CreateTransfers(List<AccountEntry> listPosCharges,List<AccountEntry> listNegCharges,List<AccountEntry> listAccountEntries) {
			//No logic that manipulates these lists should happen before the regular transfer. If necessary (like fixing incorrect unearned)
			//they will need to be made as DBMs instead or wait for another logic overhaul. 
			#region transfer within payment plans first
			List<PayPlanCharge> listCredits=_results.ListPayPlanCharges.FindAll(x => x.ChargeType==PayPlanChargeType.Credit);
			PaymentEdit.IncomeTransferData payPlanResults=PaymentEdit.CreatePayplanLoop(listPosCharges,listNegCharges,listAccountEntries
				,_paymentCur.PayNum,listCredits,DateTimeOD.Today);
			_listSplitsAssociated.AddRange(payPlanResults.ListSplitsAssociated);
			_listSplitsCur.AddRange(payPlanResults.ListSplitsCur);
			#endregion
			#region regular transfers
			PaymentEdit.IncomeTransferData results=PaymentEdit.CreateTransferLoop(listPosCharges,listNegCharges,listAccountEntries
				,_paymentCur.PayNum,listCredits,DateTimeOD.Today);
			_listSplitsAssociated.AddRange(results.ListSplitsAssociated);
			_listSplitsCur.AddRange(results.ListSplitsCur);
			#endregion
			if(results.HasInvalidSplits || payPlanResults.HasInvalidSplits) {
				MsgBox.Show(this,"Due to Rigorous Accounting, one or more invalid transactions have been cancelled.  Please fix those manually.");
			}
			else if(results.HasIvalidProcWithPayPlan || payPlanResults.HasIvalidProcWithPayPlan) {
				MsgBox.Show(this,"One or more over allocated paysplit was not able to be reversed.");
			}
		}

		///<summary>Deletes selected paysplits from the grid and attributes amounts back to where they originated from.
		///This will return a list of payment plan charges that were affected. This is so that splits can be correctly re-attributed to the payplancharge
		///when the user edits the paysplit. There should only ever be one payplancharge in that list, since the user can only edit one split at a time.</summary>
		private List<long> DeleteSelected() {
			//we need to return the payplancharge that the paysplit was associated to so that this paysplit can be correctly re-attributed to that charge.
			List<long> listPayPlanChargeNum=new List<long>();
			List<PaySplit> listSplits=gridSplits.SelectedIndices.Select(x => (PaySplit)gridSplits.ListGridRows[x].Tag).ToList();
			foreach(PaySplit paySplit in listSplits) {
				//Find split associations so we can remove both when one is deleted
				PaySplits.PaySplitAssociated associated=_listSplitsAssociated.Find(x => x.PaySplitLinked==paySplit || x.PaySplitOrig==paySplit);
				for(int j = 0;j<_results.ListAccountCharges.Count;j++) {
					AccountEntry charge=_results.ListAccountCharges[j];
					decimal chargeAmtNew=charge.AmountEnd;
					if(charge.SplitCollection.Contains(associated.PaySplitLinked)) {
						chargeAmtNew+=(decimal)associated.PaySplitLinked.SplitAmt;
					}
					else if(charge.SplitCollection.Contains(associated.PaySplitOrig)) {
						chargeAmtNew+=(decimal)associated.PaySplitOrig.SplitAmt;
					}
					else {
						continue;
					}
					if(Math.Abs(chargeAmtNew)>Math.Abs(charge.AmountStart)) {//Trying to delete an overpayment, just increase charge's amount to the max.
						charge.AmountEnd=charge.AmountStart;
					}
					else {
						charge.AmountEnd=chargeAmtNew;//Give the money back to the charge so it will display.
					}
					charge.SplitCollection.Remove(paySplit);
				}
				_listSplitsCur.Remove(associated.PaySplitLinked);
				_listSplitsCur.Remove(associated.PaySplitOrig);
			}
			_listSplitsAssociated.RemoveAll(x => listSplits.Any(y => y==x.PaySplitLinked || y==x.PaySplitOrig));
			FillGridSplits();
			return listPayPlanChargeNum;
		}

		///<summary>Only change checked state if there are zero transfer splits.</summary>
		private void checkIncludeHiddenSplits_Click(object sender,EventArgs e) {
			if(_listSplitsCur.Count > 0) {
				MsgBox.Show("You must delete all splits before including or excluding hidden payment splits.");
				checkIncludeHiddenSplits.Checked=!checkIncludeHiddenSplits.Checked;
				return;
			}
			_results=PaymentEdit.ConstructAndLinkChargeCredits(_famCur.ListPats.Select(x => x.PatNum).ToList(),_patCur.PatNum,new List<PaySplit>(),
				_paymentCur,new List<AccountEntry>(),true,false,doShowHiddenSplits:checkIncludeHiddenSplits.Checked);
			FillGridSplits();
		}

		///<summary>When a paysplit is selected, this method highlights all charges associated with it.</summary>
		private void gridSplits_CellClick(object sender,ODGridClickEventArgs e) {
			gridCharges.SetSelected(false);
			foreach(PaySplit paySplit in gridSplits.SelectedIndices.Select(x => (PaySplit)gridSplits.ListGridRows[x].Tag)) {
				for(int j=0;j<gridCharges.ListGridRows.Count;j++) {
					if(!gridCharges.ListGridRows[j].State.Visible){
						continue;
					}
					if(((AccountEntry)gridCharges.ListGridRows[j].Tag)?.SplitCollection?.Contains(paySplit)??false) {
						gridCharges.SetSelected(j,true);
						break;
					}
				}
			}
		}

		///<summary>When a charge is selected this method highlights all paysplits associated with it.</summary>
		private void gridCharges_CellClick(object sender,ODGridClickEventArgs e) {
			//Charge total text set here
			List<Tuple<long,long,long>> listAddedParents=new List<Tuple<long,long,long>>();//Prov/Clinic/Pat
			gridSplits.SetSelected(false);
			decimal chargeTotal=0;
			foreach(GridRow row in gridCharges.SelectedGridRows) {
				AccountEntry accountEntry=(AccountEntry)row.Tag;
				if(accountEntry.Tag==null) {//Parent row
					chargeTotal+=PIn.Decimal(row.Cells[row.Cells.Count-1].Text);
					listAddedParents.Add(Tuple.Create(accountEntry.ProvNum,accountEntry.ClinicNum,accountEntry.PatNum));
					continue;
				}
				else {
					for(int j=0;j<gridSplits.ListGridRows.Count;j++) {
						PaySplit paySplit=(PaySplit)gridSplits.ListGridRows[j].Tag;
						if(accountEntry.SplitCollection.Contains(paySplit)) {
							gridSplits.SetSelected(j,true);
						}
						if(accountEntry.GetType()==typeof(PayPlanCharge) && paySplit.PayPlanNum==((PayPlanCharge)accountEntry.Tag).PayPlanNum) {
							gridSplits.SetSelected(j,true);
						}
					}
					if(!listAddedParents.Exists(x => x.Item1==accountEntry.ProvNum && x.Item2==accountEntry.ClinicNum && x.Item3==accountEntry.PatNum)) {//In case a parent AND child are selected, don't add child amounts if parent was added already
						chargeTotal+=accountEntry.AmountEnd;
					}
				}
			}
			textChargeTotal.Text=chargeTotal.ToString("F");
		}

		///<summary>Creates paysplits for selected charges if there is enough payment left.</summary>
		private void butTransfer_Click(object sender,EventArgs e) {	
			if(gridCharges.SelectedGridRows.Count==0) {//Nothing selected, transfer everything.
				gridCharges.SetSelected(true);
			}
			//Make list of positive charges
			List<AccountEntry> listPosCharges=gridCharges.SelectedGridRows.Select(x => (AccountEntry)x.Tag).Where(x => x.AmountEnd>0).ToList();
			//Make list of negative charges
			List<AccountEntry> listNegCharges=gridCharges.SelectedGridRows.Select(x => (AccountEntry)x.Tag).Where(x => x.AmountEnd<0).ToList();
			//We need to detect if parent rows are selected (row.DropDownParent==null).   If it is, we need to find all child rows of that row
			//We need to add the child rows into the list of charges even if they aren't explicitly selected
			foreach(GridRow row in gridCharges.SelectedGridRows) {
				if(row.DropDownParent!=null) {
					continue;
				}
				AccountEntry parentEntry=(AccountEntry)row.Tag;
				//The user has a parent row selected - Make sure all child rows are added to appropriate lists (if they aren't in there already)
				foreach(GridRow row2 in gridCharges.ListGridRows) {
					AccountEntry childEntry=(AccountEntry)row2.Tag;
					if(parentEntry.AccountEntryNum==childEntry.AccountEntryNum) {
						continue;//Don't add parent row into anything.
					}
					if(!listPosCharges.Contains(childEntry) && !listNegCharges.Contains(childEntry) && childEntry.ClinicNum==parentEntry.ClinicNum 
						&& childEntry.PatNum==parentEntry.PatNum && childEntry.ProvNum==parentEntry.ProvNum && childEntry.AmountEnd!=0)
					{
						if(childEntry.AmountEnd>0) {
							listPosCharges.Add(childEntry);
						}
						else {
							listNegCharges.Add(childEntry);
						}
					}
				}
			}
			List<long> listPatNumsForCharges=listPosCharges.Select(x => x.PatNum).Distinct().ToList();
			//Get account entries so if any procs are attached to payment plans, their attached procedures can be attached to the new split from the transfer. 
			List<AccountEntry> listEntriesForPats=_results.ListAccountCharges.FindAll(x => x.PatNum.In(listPatNumsForCharges));
			//Sort charge lists so they maintain FIFO order regardless of order selected.
			listPosCharges=listPosCharges.OrderBy(x => x.Date).ToList();
			listNegCharges=listNegCharges.OrderBy(x => x.Date).ToList();
			CreateTransfers(listPosCharges,listNegCharges,listEntriesForPats);
			FillGridSplits();//Fills charge grid too.
		}

		///<summary>Deletes all paysplits.</summary>
		private void butClearAll_Click(object sender,EventArgs e) {
			gridSplits.SetSelected(true);
			DeleteSelected();
		}

		///<summary>Deletes selected paysplits.</summary>
		private void butDeleteSplit_Click(object sender,EventArgs e) {
			DeleteSelected();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.PaymentCreate)) {
				return;
			}
			double splitTotal=_listSplitsCur.Select(x => x.SplitAmt).Sum();
			if(!splitTotal.IsZero()) { //income transfer
				MsgBox.Show(this,"Income transfers must have a split total of 0.");
				return;
			}
			_listSplitsCur.RemoveAll(x => x.SplitAmt.IsZero());//We don't want any zero splits.  They were there just for display purposes.
			if(_listSplitsCur.Count==0) {
				Payments.Delete(_paymentCur);
			}
			else {
				foreach(PaySplit split in _listSplitsCur) {
					PaySplits.Insert(split);
				}
				foreach(PaySplits.PaySplitAssociated split in _listSplitsAssociated) {
					//Update the FSplitNum after inserts are made. 
					if(split.PaySplitLinked!=null && split.PaySplitOrig!=null) {
						PaySplits.UpdateFSplitNum(split.PaySplitOrig.SplitNum,split.PaySplitLinked.SplitNum);
					}
				}
				if(_listSplitsCur.Count>0) {//only make log when a payment with splits was made. 
					string logText=Payments.GetSecuritylogEntryText(_paymentCur,_paymentCur,isNew:true)+", "+Lans.g(this,"from Income Transfer Manager.");
					SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,_paymentCur.PatNum,logText);
				}
				string strErrorMsg=Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum,_listSplitsCur);
				if(!string.IsNullOrEmpty(strErrorMsg)) {
					MessageBox.Show(strErrorMsg);
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void FormIncomeTransferManage_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult==DialogResult.OK) {
				return;
			}
			//Clean up any potential entities that were inserted during window interactions.
			if(_unallocatedPayNum!=0) {
				//user is canceling out of the window and an unallocated transfer was made.
				ODException.SwallowAnyException(() => {
					Payments.Delete(_unallocatedPayNum);
					SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,_patCur.PatNum,$"Automatic transfer deleted for payNum: {_unallocatedPayNum}.");
				});
			}
			if(_claimTransferResult!=null) {
				//user is canceling out of the window and an claim transfer was made.
				if(_claimTransferResult.ListInsertedClaimProcs.Count > 0) {
					ODException.SwallowAnyException(() => {
						ClaimProcs.DeleteMany(_claimTransferResult.ListInsertedClaimProcs);
						SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,_patCur.PatNum,$"Automatically transferred claimprocs deleted.");
					});
				}
				if(_claimTransferResult.ListInsertedPaySplits.Count > 0) {
					ODException.SwallowAnyException(() => {
						List<long> listDistinctPayNums=_claimTransferResult.ListInsertedPaySplits.Select(x => x.PayNum).Distinct().ToList();
						foreach(long payNum in listDistinctPayNums) {
							Payments.Delete(payNum);
						}
						SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,_patCur.PatNum,$"Automatically transferred unearned from claimprocs deleted.");
					});
				}
			}
		}

	}
}