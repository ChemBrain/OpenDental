 using CodeBase;
using OpenDental.ReportingComplex;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	///<summary>This window is used to set up Dyanmic Payment Plans.  These rules will be used by the OpenDentalService for making charges.</summary>
	public partial class FormPayPlanDynamic : ODForm {
		#region Public Variables
		///<summary>Go to the specified patnum. If this number is not 0, patients.Cur will change to new patnum, and Account refreshed.</summary>
		public long GotoPatNum;
		#endregion
		#region Private Variables
		private Patient _patCur;
		private PayPlan _payPlanCur;
		private PayPlan _payPlanOld;
		///<summary>Family for the patient of this payplan.  Used to display insurance info.</summary>
		private Family _famCur;
		private double _amountPaid;
		private double _totalInterest;
		///<summary>List of charges from the database. These are the charges that have been made and are on the account. Debits only.</summary>
		private List<PayPlanCharge> _listPayPlanChargesDb=new List<PayPlanCharge>();
		private bool _isSigOldValid;
		///<summary>List of attached production. Contains both from db and ones attempting to add.</summary>
		private List<PayPlanLink> _listPayPlanLinks=new List<PayPlanLink>();
		///<summary>List of pay splits that are associated to the current pay plan.</summary>
		private List<PaySplit> _listPaySplits=new List<PaySplit>();
		///<summary>In memory list of attached production for this payment plan</summary>
		private List<PayPlanProductionEntry> _listPayPlanProductionEntries=new List<PayPlanProductionEntry>();
		///<summary>Flag for when the user has modified terms but has not saved yet.</summary>
		private bool _hasChanges=false;
		private bool _isLoading=true;
		private List<PaySplit> _listSplitsForCharges=new List<PaySplit>();
		#endregion
		#region Properties
		private decimal _sumAttachedProduction {
			get {
				return _listPayPlanProductionEntries.Sum(x => (x.AmountOverride==0 ? x.AmountOriginal : x.AmountOverride));
			}
		}
		#endregion

		///<summary></summary>
		public FormPayPlanDynamic(PayPlan payPlanCur) {
			InitializeComponent();
			_patCur=Patients.GetPat(payPlanCur.PatNum);
			_payPlanCur=payPlanCur.Copy();
			_payPlanOld=_payPlanCur.Copy();
			_famCur=Patients.GetFamily(_patCur.PatNum);
			Lan.F(this);
		}

		private void FormPayPlanDynamic_Load(object sender,System.EventArgs e) {
			#region Set Data
			_amountPaid=PayPlans.GetAmtPaid(_payPlanCur);
			if(!_payPlanCur.IsNew) {
				_listPayPlanChargesDb=PayPlanCharges.GetForPayPlan(_payPlanCur.PayPlanNum).OrderBy(x => x.ChargeDate).ToList();
				_listPayPlanLinks=PayPlanLinks.GetListForPayplan(_payPlanCur.PayPlanNum);
				_listPayPlanProductionEntries=PayPlanProductionEntry.GetProductionForLinks(_listPayPlanLinks).OrderBy(x => x.CreditDate).ToList();
				_listPaySplits=PaySplits.GetForPayPlans(new List<long>(){_payPlanCur.PayPlanNum});
				_listSplitsForCharges=
					_listPaySplits.Where(x => _listPayPlanChargesDb.Select(y => y.PayPlanChargeNum).ToList().Contains(x.PayPlanChargeNum)).ToList();
			}
			#endregion
			#region Fill and Set UI Fields
			comboCategory.Items.AddDefNone();
			comboCategory.Items.AddDefs(Defs.GetDefsForCategory(DefCat.PayPlanCategories,true));
			comboCategory.SetSelectedDefNum(_payPlanCur.PlanCategory); 
			textPatient.Text=Patients.GetLim(_payPlanCur.PatNum).GetNameLF();
			textGuarantor.Text=Patients.GetLim(_payPlanCur.Guarantor).GetNameLF();
			SetTermsFromDb();
			textAmtPaid.Text=_amountPaid.ToString("f");
			textCompletedAmt.Text=_payPlanCur.CompletedAmt.ToString("f");
			textTotalTxAmt.Text=_sumAttachedProduction.ToString("f");//possibly will not by in sync with total amount if using TP procs... 
			textNote.Text=_payPlanCur.Note;
			if(_payPlanCur.IsNew) {
				tabControl1.SelectedIndex=1;//Show the Attached Production page to the user first. 
				butDelete.Visible=false;
				butClosePlan.Visible=false;
				butUnlock.Visible=false;
				textTotalPrincipal.Text="";
				butSave.Visible=false;
				butCancelTerms.Visible=false;
			}
			else {//already saved payment plan, user needs to unlock before they can edit anything.
				FillUiForSavedPayPlan();
			}
			if(_payPlanCur.IsClosed) {
				butOK.Text=Lan.g(this,"Reopen");
				butDelete.Enabled=false;
				butClosePlan.Enabled=false;
				labelClosed.Visible=true;
			}
			checkExcludePast.Checked=PrefC.GetBool(PrefName.PayPlansExcludePastActivity);
			#endregion
			if(PrefC.GetBool(PrefName.PayPlansUseSheets)) {
				Sheet sheetPP=null;
				sheetPP=PayPlanToSheet(_payPlanCur);
				//check to see if sig box is on the sheet
				//hides butPrint and adds butSignPrint,groupbox,and sigwrapper
				for(int i = 0;i<sheetPP.SheetFields.Count;i++) {
					if(sheetPP.SheetFields[i].FieldType==SheetFieldType.SigBox) {
						butPrint.Visible=false;
						butSignPrint.Visible=true;					
					}
				}
			}
			List<PayPlan> listOverPaidPayPlans=PayPlans.GetOverpaidPayPlans(new List<long>{_payPlanCur.PayPlanNum});
			foreach(PayPlan payPlan in listOverPaidPayPlans) {
				if(_payPlanCur.PayPlanNum==payPlan.PayPlanNum) {
					labelOverpaidWarning.Visible=true;
				}
			}
			FillCharges();
			FillProduction();
			#region Signature
			if(_payPlanCur.Signature!="" && _payPlanCur.Signature!=null) {
				//check to see if sheet is signed before showing
				signatureBoxWrapper.Visible=true;
				groupBoxSignature.Visible=true;
				butSignPrint.Text="View && Print";
				signatureBoxWrapper.FillSignature(_payPlanCur.SigIsTopaz,GetKeyDataForSignature(),_payPlanCur.Signature); //fill signature
			}
			if(string.IsNullOrEmpty(_payPlanCur.Signature) || !signatureBoxWrapper.IsValid || signatureBoxWrapper.SigIsBlank) {
				_isSigOldValid=false;
			}
			else {
				_isSigOldValid=true;
			}
			#endregion
			if(!Security.IsAuthorized(Permissions.PayPlanEdit)) {
				DisableForm(butGoToGuar,butGoToPat,butCancel,butPrint,checkExcludePast,gridCharges,gridLinkedProduction);
				//allow grid so users can scroll, but de-register for event so charges cannot be modified. 
				this.gridCharges.CellDoubleClick-=gridCharges_CellDoubleClick;
			}
			_isLoading=false;
		}

		private void FillUiForSavedPayPlan() {
			if(_payPlanCur.IsLocked) {
				butUnlock.Visible=false;
				checkProductionLock.Checked=true;
				checkProductionLock.Enabled=false;
				butChangeGuar.Visible=false;
				LockProduction();
			}
			if(_listPayPlanChargesDb.Count > 0) {
				textDateFirstPay.ReadOnly=true;
			}
			textDownPayment.ReadOnly=true;//users can only add or modify downpayment on new plans since they will get immediately inserted.
			groupBoxFrequency.Enabled=false;
			groupTerms.Enabled=false;
			textTotalPrincipal.Text=_sumAttachedProduction.ToString("f");
		}

		private void FillProduction() {
			gridLinkedProduction.BeginUpdate();
			gridLinkedProduction.ListGridColumns.Clear();
			int widthClinic=140;
			int widthDesc=(PrefC.HasClinicsEnabled ? 170 : 170 + widthClinic);
			GridColumn col;
			col=new GridColumn(Lan.g(gridLinkedProduction.TranslationName,"Date\r\nAdded"),70,HorizontalAlignment.Center);
			gridLinkedProduction.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(gridLinkedProduction.TranslationName,"Date"),70,HorizontalAlignment.Center);
			gridLinkedProduction.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(gridLinkedProduction.TranslationName,"Provider"),70);
			gridLinkedProduction.ListGridColumns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new GridColumn(Lan.g(gridLinkedProduction.TranslationName,"Clinic"),widthClinic);
				gridLinkedProduction.ListGridColumns.Add(col);
			}
			col=new GridColumn(Lan.g(gridLinkedProduction.TranslationName,"Description"),widthDesc);
			gridLinkedProduction.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(gridLinkedProduction.TranslationName,"Amount"),60,HorizontalAlignment.Right);//amount from production value.
			gridLinkedProduction.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(gridLinkedProduction.TranslationName,"Amount")+"\r\n"+Lan.g(gridLinkedProduction.TranslationName,"Attached"),60,HorizontalAlignment.Right,true);
			gridLinkedProduction.ListGridColumns.Add(col);
			gridLinkedProduction.ListGridRows.Clear();
			foreach(PayPlanProductionEntry entry in _listPayPlanProductionEntries) {
				GridRow row=new GridRow();
				if(entry.CreditDate==DateTime.MinValue) {
					//credit was just added 
					row.Cells.Add(DateTime.Today.ToShortDateString());
				}
				else {
					row.Cells.Add(entry.CreditDate.ToShortDateString());
				}
				row.Cells.Add(entry.ProductionDate.ToShortDateString());
				row.Cells.Add(Providers.GetAbbr(entry.ProvNum));
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetAbbr(entry.ClinicNum));
				}
				row.Cells.Add(entry.Description);
				row.Cells.Add(entry.AmountOriginal.ToString("f"));
				if(entry.AmountOverride==0) {
					row.Cells.Add(entry.AmountOriginal.ToString("f"));//if no override was entered, the full amount is being attached to the plan.
				}
				else {
					row.Cells.Add(entry.AmountOverride.ToString("f"));
				}
				row.Tag=entry;
				gridLinkedProduction.ListGridRows.Add(row);
			}
			gridLinkedProduction.EndUpdate();
		}

		///<summary>Fills both charges that have come due (black in color) and expected charges (grey) that have not been added yet as well
		///as payments that have been made for the charges that have come due. Returns true if the method wasn't returned from early due to errors.
		///Returns false if TryGetTermsFromUI fails or terms.AreTermsValid fails.</summary>
		private bool FillCharges() {
			if(!TryGetTermsFromUI(out PayPlanTerms terms)) {
				return false;
			}
			gridCharges.BeginUpdate();
			gridCharges.ListGridColumns.Clear();
			GridColumn col;
			//If this column is changed from a date column then the comparer method (ComparePayPlanRows) needs to be updated.
			//If changes are made to the order of the grid, changes need to also be made for butPrint_Click
			col=new GridColumn(Lan.g("PayPlanAmortization","Date"),64,HorizontalAlignment.Center);//0
			gridCharges.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("PayPlanAmortization","Provider"),50);//1
			gridCharges.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("PayPlanAmortization","Description"),147);//2
			gridCharges.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("PayPlanAmortization","Principal"),75,HorizontalAlignment.Right);//3
			gridCharges.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("PayPlanAmortization","Interest"),67,HorizontalAlignment.Right);//4
			gridCharges.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("PayPlanAmortization","Due"),75,HorizontalAlignment.Right);//5
			gridCharges.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("PayPlanAmortization","Payment"),75,HorizontalAlignment.Right);//6
			gridCharges.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("PayPlanAmortization","Balance"),70,HorizontalAlignment.Right);//7
			gridCharges.ListGridColumns.Add(col);
			gridCharges.ListGridRows.Clear();
			List<PayPlanCharge> listChargesExpected=new List<PayPlanCharge>();
			if(_payPlanCur.IsNew && terms.DownPayment!=0) {
				//they have a down payment (possibly broken into multiple charges for differing providers) that has not yet been put into the database.
				List<PayPlanCharge> listForDownPayment=PayPlanCharges.GetForDownPayment(terms,_famCur,_listPayPlanLinks,_payPlanCur);
				listChargesExpected.AddRange(listForDownPayment);
				listChargesExpected.AddRange(PayPlanEdit.GetListExpectedCharges(_listPayPlanChargesDb,terms,_famCur,_listPayPlanLinks,_listPaySplits
					,_payPlanCur,false,listForDownPayment.Count,0,false));
			}
			else {
				listChargesExpected.AddRange(PayPlanEdit.GetListExpectedCharges(_listPayPlanChargesDb,terms,_famCur,_listPayPlanLinks,_payPlanCur,false
					,listPaySplits:_listPaySplits));
			}
			if(!terms.AreTermsValid) {
				MsgBox.Show("This payment plan will never be paid off. The interest is too high or the payment amount is too low.");
				gridCharges.EndUpdate();
				return false;
			}
			List<GridRow> listPayPlanRows=new List<GridRow>();
			int numCharges=1;
			foreach(PayPlanCharge chargeDue in _listPayPlanChargesDb) {
				listPayPlanRows.Add(PayPlanL.CreateRowForPayPlanCharge(chargeDue,numCharges,true));
				numCharges++;
			}
			foreach(PayPlanCharge chargeExpected in listChargesExpected) {
				listPayPlanRows.Add(PayPlanL.CreateRowForPayPlanCharge(chargeExpected,numCharges,true));
				numCharges++;
			}
			List<PaySplit> listPaySplits=new List<PaySplit>();
			DataTable bundledPayments=PaySplits.GetForPayPlan(_payPlanCur.PayPlanNum);
			listPaySplits=PaySplits.GetFromBundled(bundledPayments);
			for(int i=0;i<listPaySplits.Count;i++) {
				listPayPlanRows.Add(PayPlanL.CreateRowForPaySplit(bundledPayments.Rows[i],listPaySplits[i],true));
			}
			listPayPlanRows.Sort(PayPlanL.ComparePayPlanRows);
			int totalsRowIndex = -1; //if -1, then don't show a bold line as the first charge showing has not come due yet.
			#region Fill Sum Text
			double balanceAmt=0;
			double principalDue=0;
			_totalInterest=0;
			double totalPay=0;
			for(int i=0;i<listPayPlanRows.Count;i++) {
				bool isFutureCharge=PIn.Date(listPayPlanRows[i].Cells[0].Text)>DateTime.Today;
				if(!checkExcludePast.Checked || isFutureCharge) {
					//Add the row if we aren't excluding past activity or the activity is in the future.
					gridCharges.ListGridRows.Add(listPayPlanRows[i]);
				}
				if(listPayPlanRows[i].Cells[3].Text!="") {//Principal
					principalDue+=PIn.Double(listPayPlanRows[i].Cells[3].Text);
					balanceAmt+=PIn.Double(listPayPlanRows[i].Cells[3].Text);
				}
				if(listPayPlanRows[i].Cells[4].Text!="") {//Interest
					_totalInterest+=PIn.Double(listPayPlanRows[i].Cells[4].Text);
					balanceAmt+=PIn.Double(listPayPlanRows[i].Cells[4].Text);
				}
				else if(listPayPlanRows[i].Cells[6].Text!="") {//Payment
					totalPay+=PIn.Double(listPayPlanRows[i].Cells[6].Text);
					balanceAmt-=PIn.Double(listPayPlanRows[i].Cells[6].Text);
				}
				if(!checkExcludePast.Checked || isFutureCharge) {
					gridCharges.ListGridRows[gridCharges.ListGridRows.Count-1].Cells[7].Text=balanceAmt.ToString("f");
				}
				if(!isFutureCharge) {
					textPrincipalSum.Text=principalDue.ToString("f");
					textInterestSum.Text=_totalInterest.ToString("f");
					textDueSum.Text=(principalDue+_totalInterest).ToString("f");
					textPaymentSum.Text=totalPay.ToString("f");
					textBalanceSum.Text=balanceAmt.ToString("f");
					if(gridCharges.ListGridRows.Count>0) {
						totalsRowIndex=gridCharges.ListGridRows.Count-1;
					}
				}
			}
			#endregion
			_totalInterest=0;
			for(int i=0;i<_listPayPlanChargesDb.Count;i++) {
				_totalInterest+=_listPayPlanChargesDb[i].Interest;
			}
			for(int i=0;i<listChargesExpected.Count;i++) {//combine with list expected.
				_totalInterest+=listChargesExpected[i].Interest;
			}
			textTotalCost.Text=(_sumAttachedProduction+(decimal)_totalInterest).ToString("f");
			gridCharges.EndUpdate();
			if(gridCharges.ListGridRows.Count>0 && totalsRowIndex != -1) {
				gridCharges.ListGridRows[totalsRowIndex].ColorLborder=Color.Black;
				gridCharges.ListGridRows[totalsRowIndex].Cells[6].Bold=YN.Yes;
			}
			textAccumulatedDue.Text=PayPlans.GetAccumDue(_payPlanCur.PayPlanNum,_listPayPlanChargesDb).ToString("f");
			textPrincPaid.Text=PayPlans.GetPrincPaid(_amountPaid,_payPlanCur.PayPlanNum,_listPayPlanChargesDb).ToString("f");
			return true;
		}

		///<summary>Helper to get and store the UI elements so we do not need to pass in more than what is necessary. Set from the UI, not DB.</summary>
		private bool TryGetTermsFromUI(out PayPlanTerms terms) {
			terms=new PayPlanTerms();
			try {
				if(!ValidateTerms()) {//saveData relies on this, if removed from this method, needs to be addded to SaveData()
					return false;
				}
				terms.APR=PIn.Double(textAPR.Text);
				terms.DateFirstPayment=PIn.Date(textDateFirstPay.Text);
				terms.Frequency=GetChargeFrequency();//verify this is just based on the ui, not the db.
				terms.PayCount=PIn.Int(textPaymentCount.Text);
				terms.PeriodPayment=PIn.Decimal(textPeriodPayment.Text);
				terms.PrincipalAmount=PIn.Double(textTotalPrincipal.Text);
				terms.RoundDec=CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits;
				terms.DateAgreement=PIn.Date(textDate.Text);
				terms.DownPayment=PIn.Double(textDownPayment.Text);
				terms.PaySchedule=PayPlanEdit.GetPayScheduleFromFrequency(terms.Frequency);
				//now that terms are set, we need to potentially calculate the periodpayment amount since we only store that and not the payCount
				if(terms.PayCount!=0) {
					terms.PeriodPayment=PayPlanEdit.CalculatePeriodPayment(terms.APR,terms.Frequency,terms.PeriodPayment,terms.PayCount,terms.RoundDec
					,terms.PrincipalAmount,terms.DownPayment);
					terms.PayCount=0;
				}
			}
			catch(Exception ex) {
				ex.DoNothing();
				MsgBox.Show(this,"Unexpected error getting the terms from the window.");
				return false;
			}
			return true;
		}

		///<summary>Resets the UI to what was loaded from the database.</summary>
		private void SetTermsFromDb() {
			if(_payPlanCur.IsNew) {
				//If a plan is created "today" with the customer making their first payment on the spot, they will over pay interest.  
				//If there is a larger gap than 1 month before the first payment, interest will be under calculated.
				//Our temporary solution is to prefill the date of first payment box with next months date which is most accurate for calculating interest.
				textDateFirstPay.Text=DateTime.Now.AddMonths(1).ToShortDateString();
			}
			else {
				textDateFirstPay.Text=_payPlanCur.DatePayPlanStart.ToShortDateString();
			}
			textPeriodPayment.Text=_payPlanCur.PayAmt.ToString("f");//we only save the amount, not the # of payments for dynamic payment plans
			textPaymentCount.Text="";
			textDownPayment.Text=_payPlanCur.DownPayment.ToString("f");
			textDate.Text=_payPlanCur.PayPlanDate.ToShortDateString();
			textAPR.Text=_payPlanCur.APR.ToString();
			switch(_payPlanCur.ChargeFrequency) {
				case PayPlanFrequency.Weekly:
					radioWeekly.Checked=true;
					break;
				case PayPlanFrequency.EveryOtherWeek:
					radioEveryOtherWeek.Checked=true;
					break;
				case PayPlanFrequency.OrdinalWeekday:
					radioOrdinalWeekday.Checked=true;
					break;
				case PayPlanFrequency.Monthly:
					radioMonthly.Checked=true;
					break;
				case PayPlanFrequency.Quarterly:
					radioQuarterly.Checked=true;
					break;
				default://default to monthly for new plans (should be 0 and do this regardless)
					radioMonthly.Checked=true;
					break;
			}
		}

		///<summary>Unlocks a saved payment plan for editing. Allows users to try out new terms which will not be saved unless user specifies.</summary>
		private void ButUnlock_Click(object sender,EventArgs e) {
			UnlockTerms();
		}

		private void ButSave_Click(object sender,EventArgs e) {
			LockTerms(true,FillCharges());
		}

		///<summary>Sets UI elements back to what they were before unlocking (change back to what the window loaded with originally)</summary>
		private void ButCancelTerms_Click(object sender,EventArgs e) {
			SetTermsFromDb();
			LockTerms(false,FillCharges());//re-lock. Nothing is getting saved though since nothing changed. 
		}

		///<summary></summary>
		private void LockTerms(bool isSaveData,bool isUiValid=true) {
			if(isSaveData) {
				if(SaveData(isUiValid:isUiValid)) {
					_hasChanges=false;//Terms were saved
					groupTerms.Enabled=false;
					groupBoxFrequency.Enabled=false;
				}
				else {
					//failed saving. Return user to form.
					return;
				}
			}
			groupTerms.Enabled=false;
			groupBoxFrequency.Enabled=false;
			if(!_payPlanCur.IsLocked || _payPlanCur.IsNew) {
				butUnlock.Visible=true;
			}
		}

		///<summary></summary>
		private void UnlockTerms() {
			_hasChanges=true;//general so we don't have to track every text box to see if something was modified.
			groupTerms.Enabled=true;
			groupBoxFrequency.Enabled=true;
			butUnlock.Visible=false;
		}

		///<summary>different then locking the terms. This checkbox should lock the entire payment plan down. No changing terms, no adding production.</summary>
		private void LockProduction() {
			LockTerms(false);
			butAddAdjustment.Enabled=false;
			butAddProcedure.Enabled=false;
			butDeleteProduction.Enabled=false;
		}

		///<summary>allowed for user to change their mind on new payment plans. They will be stopped during validation if they have APR (required to be locked)</summary>
		private void UnlockProduction() {
			butAddAdjustment.Enabled=true;
			butAddProcedure.Enabled=true;
			butDeleteProduction.Enabled=true;
			UnlockTerms();
		}

		private void CheckProductionLock_CheckedChanged(object sender,EventArgs e) {
			if(!checkProductionLock.Checked && _payPlanCur.IsNew) {
				UnlockProduction();
			}
			else if(checkProductionLock.Checked) {
				LockProduction();
				if(!_isLoading) {
					if(CreateSchedule()) {//create schedule button will get disabled once this box is checked so we need to do this for the user 
						butAddAdjustment.Enabled=false;
						butAddProcedure.Enabled=false;
						butDeleteProduction.Enabled=false;
					}
					else {
						//terms were not correctly validated, schedule was not able to be set so uncheck the box for the user to make edits.
						checkProductionLock.Checked=false;
					}
				}
			}
		}

		private void ButAddProcedure_Click(object sender,EventArgs e) {
			FormProcSelect formProcSelect=new FormProcSelect(_patCur.PatNum,false,true);
			formProcSelect.ShowTpProcs=false;
			if(formProcSelect.ShowDialog()!=DialogResult.OK) {
				return;
			}
			int countSkipped=0;
			List<long> listProcNums=formProcSelect.ListSelectedProcs.Select(x => x.ProcNum).ToList();
			List<ClaimProc> listClaimProcs=ClaimProcs.GetForProcs(listProcNums);
			List<Adjustment> listAdjustments=Adjustments.GetForProcs(listProcNums);
			List<PayPlanCharge> listPayPlanCreditsForProcs=PayPlanCharges.GetPatientPayPlanCreditsForProcs(listProcNums);
			List<PayPlanLink> listPayPlanLinksForProcs=PayPlanLinks.GetForFKeysAndLinkType(listProcNums,PayPlanLinkType.Procedure);
			foreach(Procedure proc in formProcSelect.ListSelectedProcs) {
				if(listPayPlanCreditsForProcs.Select(x => x.ProcNum).Contains(proc.ProcNum)
					|| listPayPlanLinksForProcs.Select(x => x.FKey).Contains(proc.ProcNum)
					|| _listPayPlanLinks.Exists(x => x.LinkType==PayPlanLinkType.Procedure && x.FKey==proc.ProcNum))
				{
					countSkipped++;
					continue;
				}
				PayPlanLink creditAdding=new PayPlanLink() {
					PayPlanNum=_payPlanCur.PayPlanNum,
					LinkType=PayPlanLinkType.Procedure,
					FKey=proc.ProcNum
				};
				_listPayPlanLinks.Add(creditAdding);
				_listPayPlanProductionEntries.Add(new PayPlanProductionEntry(proc,creditAdding,listClaimProcs,listAdjustments));
			}
			textTotalPrincipal.Text=_sumAttachedProduction.ToString("f");
			FillProduction();
			if(countSkipped>0) {
				MsgBox.Show(this,"Procedures can only be attached to one payment plan at a time.");
			}
		}

		private void ButAddAdjustment_Click(object sender,EventArgs e) {
			FormAdjustmentPicker formAdjustmentPicker=new FormAdjustmentPicker(_patCur.PatNum,true);
			if(formAdjustmentPicker.ShowDialog()!=DialogResult.OK) {
				return;
			}
			Adjustment adjustmentAdding=formAdjustmentPicker.SelectedAdjustment;
			//check to see if adjustment is already on a different payment plan
			if(PayPlanLinks.GetForFKeyAndLinkType(adjustmentAdding.AdjNum,PayPlanLinkType.Adjustment).Count > 0
				|| _listPayPlanLinks.Exists(x => x.LinkType==PayPlanLinkType.Adjustment && x.FKey==adjustmentAdding.AdjNum)) 
			{
				MsgBox.Show("Adjustment can only be attached to one active payment plan at a time.");
				return;
			}
			PayPlanLink creditAdding=new PayPlanLink() {
				PayPlanNum=_payPlanCur.PayPlanNum,
				LinkType=PayPlanLinkType.Adjustment,
				FKey=adjustmentAdding.AdjNum
			};
			_listPayPlanLinks.Add(creditAdding);
			_listPayPlanProductionEntries.Add(new PayPlanProductionEntry(adjustmentAdding,creditAdding));
			textTotalPrincipal.Text=_sumAttachedProduction.ToString("f");
			FillProduction();
		}

		private void ButDeleteProduction_Click(object sender,EventArgs e) {
			if(gridLinkedProduction.SelectedIndices.Count() <= 0) {
				MsgBox.Show(this,"Please select an item from the grid to remove.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"The selected items will be deleted from the database.  This cannot be undone.\r\n"+
				"Continue?"))
			{
				return;
			}
			int countInvalid=0;
			List<PayPlanProductionEntry> listSelectedProduction=gridLinkedProduction.SelectedTags<PayPlanProductionEntry>();
			Dictionary<PayPlanLinkType,List<PayPlanProductionEntry>> dictProdEntries=listSelectedProduction
				.GroupBy(x => x.LinkType)
				.ToDictionary(x => x.Key,x => x.ToList());
			List<PayPlanCharge> listChargesForEntry=new List<PayPlanCharge>();
			foreach(PayPlanLinkType payPlanLinkType in dictProdEntries.Keys) {
				listChargesForEntry.AddRange(PayPlanCharges.GetForLinkTypeAndFKeys(payPlanLinkType,
					dictProdEntries[payPlanLinkType].Select(x => x.PriKey).ToArray()));
			}
			foreach(PayPlanProductionEntry entry in listSelectedProduction) {
				//check to see if payments exist for the entry and disallow removing while payments exist.
				switch(entry.LinkType) {
					case PayPlanLinkType.Adjustment:
						if(_listSplitsForCharges.Any(x => x.AdjNum==entry.PriKey)) {
							countInvalid++;
							continue;//patient payments are attached, not valid for deletion.
						}
						break;
					case PayPlanLinkType.Procedure:
						if(_listSplitsForCharges.Any(x => x.ProcNum==entry.PriKey)) {
							countInvalid++;
							continue;//patient payments are attached, not valid for deletion.
						}
						break;
					default:
						continue;//Unknown link type, play it safe and do not delete the charges.
				}
				List<long> listPayPlanChargeNumsDeleting=listChargesForEntry.Select(x => x.PayPlanChargeNum).ToList();
				PayPlanCharges.DeleteMany(listPayPlanChargeNumsDeleting);
				PayPlanLinks.Delete(entry.LinkedCredit.PayPlanLinkNum);
				_listPayPlanLinks.Remove(entry.LinkedCredit);
				_listPayPlanProductionEntries.Remove(entry);
				_listPayPlanChargesDb.RemoveAll(x => x.PayPlanChargeNum.In(listPayPlanChargeNumsDeleting));
			}
			if(countInvalid>0) {
				MsgBox.Show(this,"Some production was not able to be removed due to having patient payments attached. Remove those first.");
			}
			FillProduction();
			FillCharges();
		}

		///<summary>Gets the hashstring for generating signatures. This is only to be used if the payment plan has interest or if the user
		///electively chose to make it a fully locked payment plan. Dynamic payment plans in general are variable and a user cannot agree
		///to a total amount.</summary>
		private string GetKeyDataForSignature() {
			//strb is a concatenation of the following:
			//pp: DateOfAgreement+ Total Amt+ APR+ Num of Payments+ Payment Amt + Note
			StringBuilder strb = new StringBuilder();
			Sheet sheetPP=null;
			sheetPP=PayPlanToSheet(_payPlanCur);
			strb.Append(_payPlanCur.PayPlanDate.ToShortDateString());
			if(_payPlanCur.APR!=0) {//only if using interest are these fields static, otherwise it is assumed they are variable and expected to change.
				//once you're using APR you can't go back to not using APR
				strb.Append(textTotalPrincipal.Text);
			}
			strb.Append(_payPlanCur.PayAmt.ToString("f"));
			strb.Append(_payPlanCur.APR.ToString());
			strb.Append(textNote.Text);
			strb.Append(Sheets.GetSignatureKey(sheetPP));
			return PayPlans.GetHashStringForSignature(strb.ToString());
		}

		private void butGoToPat_Click(object sender,System.EventArgs e) {
			GoToHelper(false);
		}

		private void butGoTo_Click(object sender,System.EventArgs e) {
			GoToHelper(true);
		}

		private void GoToHelper(bool isGuarantor) {
			if(!SaveData()) {
				return;
			}
			GotoPatNum=isGuarantor?_payPlanCur.Guarantor:_payPlanCur.PatNum;
			DialogResult=DialogResult.OK;
		}

		private void butChangeGuar_Click(object sender,System.EventArgs e) {
			if(PayPlans.GetAmtPaid(_payPlanCur)!=0) {
				MsgBox.Show(this,"Not allowed to change the guarantor because payments are attached.");
				return;
			}
			if(_listPayPlanChargesDb.Count>0) {
				MsgBox.Show(this,"Not allowed to change the guarantor when charges have been created.");
				return;
			}
			FormPatientSelect formPatientSelect=new FormPatientSelect();
			formPatientSelect.SelectionModeOnly=true;
			formPatientSelect.ShowDialog();
			if(formPatientSelect.DialogResult!=DialogResult.OK) {
				return;
			}
			_payPlanCur.Guarantor=formPatientSelect.SelectedPatNum;
			textGuarantor.Text=Patients.GetLim(_payPlanCur.Guarantor).GetNameLF();
			FillCharges();
		}
		
		private void textAmount_Validating(object sender,CancelEventArgs e) {
			if(textCompletedAmt.Text=="") {
				return;
			}
			if(PIn.Double(textCompletedAmt.Text)==PIn.Double(textTotalPrincipal.Text)) {
				return;
			}
		}

		private void textPaymentCount_KeyPress(object sender,System.Windows.Forms.KeyPressEventArgs e) {
			textPeriodPayment.Text="";
		}

		private void textPeriodPayment_KeyPress(object sender,System.Windows.Forms.KeyPressEventArgs e) {
			textPaymentCount.Text="";
		}

		private void butCreateSched_Click(object sender,System.EventArgs e) {
			CreateSchedule();
			tabControl1.SelectedIndex=0;//flip back to charges tab
		}

		///<summary>Goes through the logic to create a new schedule. Returns true if a terms were successfully validated and correct.</summary>
		private bool CreateSchedule() {
			if(ValidateTerms()) {
				FillCharges();
				SetNote();
				signatureBoxWrapper.FillSignature(_payPlanCur.SigIsTopaz,GetKeyDataForSignature(),_payPlanCur.Signature);
				return true;
			}
			return false;
		}

		private bool ValidateTerms() {
			if(_isLoading) {
				return true;
			}
			if(textDate.errorProvider1.GetError(textDate)!=""
				|| textDateFirstPay.errorProvider1.GetError(textDateFirstPay)!=""
				|| textDownPayment.errorProvider1.GetError(textDownPayment)!=""
				|| textAPR.errorProvider1.GetError(textAPR)!=""
				|| textPaymentCount.errorProvider1.GetError(textPaymentCount)!=""
				|| textPeriodPayment.errorProvider1.GetError(textPeriodPayment)!=""
				|| textCompletedAmt.errorProvider1.GetError(textCompletedAmt)!=""
				) 
			{
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return false;
			}
			if(PIn.Date(textDate.Text).Date > DateTime.Today.Date && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
				MsgBox.Show(this,"Payment plan date cannot be set for the future.");
				return false;
			}
			if(textTotalPrincipal.Text=="" || PIn.Double(textTotalPrincipal.Text)==0) {
				MsgBox.Show(this,"Please attach production to this payment plan.");
				return false;
			}
			if(textDateFirstPay.Text=="" || (PIn.Date(textDateFirstPay.Text).Date < DateTime.Today && _payPlanCur.IsNew)) {
				MsgBox.Show(this,"Please enter a date on or after today for the date of the first payment.");
				return false;
			}
			if(textPaymentCount.Text=="" && textPeriodPayment.Text=="") {
				//message box also used when butRecalculate is clicked
				MsgBox.Show(this,"Please enter a term or payment amount first.");
				return false;
			}
			if(textPaymentCount.Text=="" && PIn.Double(textPeriodPayment.Text)==0) {
				MsgBox.Show(this,"Payment cannot be 0.");
				return false;
			}
			if(textPaymentCount.Text!="" && textPeriodPayment.Text!="") {
				MsgBox.Show(this,"Please choose either Number of Payments or Payment Amt.");
				return false;
			}
			if(textPeriodPayment.Text=="" && PIn.Long(textPaymentCount.Text) < 1) {
				MsgBox.Show(this,"Term cannot be less than 1.");
				return false;
			}
			if(PIn.Double(textTotalPrincipal.Text)-PIn.Double(textDownPayment.Text) < 0) {
				MsgBox.Show(this,"Down payment must be less than or equal to total amount.");
				return false;
			}
			if(PIn.Int(textAPR.Text)!=0 && checkProductionLock.Checked==false) {
				MsgBox.Show(this,"Payment plans with APR must be locked. Remove the APR or check the box for Full Lock.");
				return false;
			}
			return true;
		}

		private void gridCharges_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) { 
			if(gridCharges.ListGridRows[e.Row].Tag==null) {//Prevent double clicking on the "Current Totals" row
				return;
			}
			if(gridCharges.ListGridRows[e.Row].Tag.GetType()==typeof(PayPlanCharge)) {
				//don't do anything. Dynamic payment plan charges are not editable. 
			}
			else if(gridCharges.ListGridRows[e.Row].Tag.GetType()==typeof(PaySplit)) {
				PaySplit paySplit=(PaySplit)gridCharges.ListGridRows[e.Row].Tag;
				Payment pay=Payments.GetPayment(paySplit.PayNum);
				if(pay==null) {
					MessageBox.Show(Lans.g(this,"No payment exists.  Please run database maintenance method")+" "+nameof(DatabaseMaintenances.PaySplitWithInvalidPayNum));
					return;
				}
				FormPayment formPayment=new FormPayment(_patCur,_famCur,pay,false);//FormPayment may insert and/or update the paysplits. 
				formPayment.IsNew=false;
				formPayment.ShowDialog();
				if(formPayment.DialogResult==DialogResult.Cancel) {
					return;
				}
			}
			else if(gridCharges.ListGridRows[e.Row].Tag.GetType()==typeof(DataRow)) {//Claim payment or bundle.
				DataRow bundledClaimProc=(DataRow)gridCharges.ListGridRows[e.Row].Tag;
				Claim claimCur=Claims.GetClaim(PIn.Long(bundledClaimProc["ClaimNum"].ToString()));
				if(claimCur==null) {
					MsgBox.Show(this,"The claim has been deleted.");
				}
				else {
					if(!Security.IsAuthorized(Permissions.ClaimView)) {
						return;
					}
					//FormClaimEdit inserts and/or updates the claim and/or claimprocs, which could potentially change the bundle.
					FormClaimEdit formClaimEdit=new FormClaimEdit(claimCur,_patCur,_famCur);
					formClaimEdit.IsNew=false;
					formClaimEdit.ShowDialog();
					//Cancel from FormClaimEdit does not cancel payment edits, fill grid every time
				}
			}
			FillCharges();
		}

		private void gridLinkedProduction_CellLeave(object sender,ODGridClickEventArgs e) {
			if(checkProductionLock.Checked) {
				FillProduction();//Show the user that their changes were not saved.
				return;
			}
			if(gridLinkedProduction.ListGridRows[e.Row].Tag==null) {
				return;
			}
			PayPlanProductionEntry entry=(PayPlanProductionEntry)gridLinkedProduction.ListGridRows[e.Row].Tag;
			decimal overrideVal=PIn.Decimal(gridLinkedProduction.ListGridRows[e.Row].Cells[e.Col].Text);//if zero, attempting to remove override if set.
			if((entry.AmountOriginal.IsEqual(overrideVal))) {//attempting to set override
				//de-register the event so it doesn't get called after the message box shows.
				gridLinkedProduction.CellLeave -= new ODGridClickEventHandler(this.gridLinkedProduction_CellLeave);
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"You are attempting to set an override for this entry which will prevent the row " +
					"from auto updating. Do you want to set the override?")) 
				{
					gridLinkedProduction.CellLeave += new ODGridClickEventHandler(this.gridLinkedProduction_CellLeave);
					SetOverride(entry,0);//In the event the user previously had an override set and they are attempting to clear it. 
					return;
				}
				gridLinkedProduction.CellLeave += new ODGridClickEventHandler(this.gridLinkedProduction_CellLeave);
			}
			SetOverride(entry,overrideVal);
		}

		private void SetOverride(PayPlanProductionEntry entry,decimal overrideVal) {
			entry.AmountOverride=overrideVal;
			entry.LinkedCredit.AmountOverride=(double)entry.AmountOverride;
			textTotalPrincipal.Text=_sumAttachedProduction.ToString("f");
			FillProduction();
			FillCharges();
		}

		private void butSignPrint_Click(object sender,EventArgs e) {
			if(!SaveData()) {
				return;
			}
			Sheet sheetPP=null;
			sheetPP=PayPlanToSheet(_payPlanCur);
			string keyData=GetKeyDataForSignature();
			SheetParameter.SetParameter(sheetPP,"keyData",keyData);
			SheetUtil.CalculateHeights(sheetPP);
			FormSheetFillEdit FormSF=new FormSheetFillEdit(sheetPP);
			FormSF.ShowDialog();
			if(FormSF.DialogResult==DialogResult.OK) {//save signature
				if(_payPlanCur.Signature=="") {//clear signature and hide sigbox if blank sig was saved
					signatureBoxWrapper.ClearSignature();
					butSignPrint.Text="Sign && Print";
					signatureBoxWrapper.Visible=false;
					groupBoxSignature.Visible=false;
				}
				else {
					signatureBoxWrapper.Visible=true;//show after PP has been signed for the first time
					groupBoxSignature.Visible=true;
					butSignPrint.Text="View && Print";
					signatureBoxWrapper.FillSignature(_payPlanCur.SigIsTopaz,keyData,_payPlanCur.Signature); //fill signature on form
				}
			}
		}

		private void ButPrintProduction_Click(object sender,EventArgs e) {
			PrinterL.TryPrintOrDebugRpPreview(pd_PrintPage,Lan.g(this,"Attached PayPlan Production printed"),PrintoutOrientation.Landscape);
		}

		private void pd_PrintPage(object sender,PrintPageEventArgs e) {
			int pagesPrinted=0; 
			Rectangle bounds=e.MarginBounds;
			//new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			bool headingPrinted=false;
			int headingPrintH=0;
			#region printHeading
			if(!headingPrinted) {
				text=Lan.g(this,"Payment Plan Credits");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=DateTime.Today.ToShortDateString();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=Patients.GetNameFLnoPref(_patCur.LName,_patCur.FName,"");
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			yPos=gridLinkedProduction.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
				text=Lan.g(this,"Total")+": "+_sumAttachedProduction.ToString("c");
				g.DrawString(text,subHeadingFont,Brushes.Black,center+gridLinkedProduction.Width/2-g.MeasureString(text,subHeadingFont).Width-10,yPos);
			}
			g.Dispose();
		}

		private void butPrint_Click(object sender,System.EventArgs e) {
			if(!ValidateTerms()) {
				return;
			} 
			if(PrefC.GetBool(PrefName.PayPlansUseSheets)) {
				Sheet sheetPP=null;
				sheetPP=PayPlanToSheet(_payPlanCur);
				SheetPrinting.Print(sheetPP);
			}
			else {
				Font font=new Font("Tahoma",9);
				Font fontBold=new Font("Tahoma",9,FontStyle.Bold);
				Font fontTitle=new Font("Tahoma",17,FontStyle.Bold);
				Font fontSubTitle=new Font("Tahoma",10,FontStyle.Bold);
				ReportComplex report=new ReportComplex(false,false);
				report.AddTitle("Title",Lan.g(this,"Payment Plan Terms"),fontTitle);
				report.AddSubTitle("PracTitle",PrefC.GetString(PrefName.PracticeTitle),fontSubTitle);
				report.AddSubTitle("Date SubTitle",DateTime.Today.ToShortDateString(),fontSubTitle);
				AreaSectionType sectType=AreaSectionType.ReportHeader;
				Section section=report.Sections[AreaSectionType.ReportHeader];
				//int sectIndex=report.Sections.GetIndexOfKind(AreaSectionKind.ReportHeader);
				Size size=new Size(300,20);//big enough for any text
				ContentAlignment alignL=ContentAlignment.MiddleLeft;
				ContentAlignment alignR=ContentAlignment.MiddleRight;
				int yPos=140;
				int space=30;
				int x1=175;
				int x2=275;
				report.ReportObjects.Add(new ReportObject
					("Patient Title",sectType,new Point(x1,yPos),size,"Patient",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Patient Detail",sectType,new Point(x2,yPos),size,textPatient.Text,font,alignR));
				yPos+=space;
				report.ReportObjects.Add(new ReportObject
					("Guarantor Title",sectType,new Point(x1,yPos),size,"Guarantor",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Guarantor Detail",sectType,new Point(x2,yPos),size,textGuarantor.Text,font,alignR));
				yPos+=space;
				report.ReportObjects.Add(new ReportObject
					("Date of Agreement Title",sectType,new Point(x1,yPos),size,"Date of Agreement",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Date of Agreement Detail",sectType,new Point(x2,yPos),size,_payPlanCur.PayPlanDate.ToString("d"),font,alignR));
				yPos+=space;
				report.ReportObjects.Add(new ReportObject
					("Principal Title",sectType,new Point(x1,yPos),size,"Principal",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Principal Detail",sectType,new Point(x2,yPos),size,_sumAttachedProduction.ToString("n"),font,alignR));
				yPos+=space;
				report.ReportObjects.Add(new ReportObject
					("Annual Percentage Rate Title",sectType,new Point(x1,yPos),size,"Annual Percentage Rate",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Annual Percentage Rate Detail",sectType,new Point(x2,yPos),size,_payPlanCur.APR.ToString("f1"),font,alignR));
				yPos+=space;
				report.ReportObjects.Add(new ReportObject
					("Total Finance Charges Title",sectType,new Point(x1,yPos),size,"Total Finance Charges",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Total Finance Charges Detail",sectType,new Point(x2,yPos),size,_totalInterest.ToString("n"),font,alignR));
				yPos+=space;
				report.ReportObjects.Add(new ReportObject
					("Total Cost of Loan Title",sectType,new Point(x1,yPos),size,"Total Cost of Loan",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Total Cost of Loan Detail",sectType,new Point(x2,yPos),size,(_sumAttachedProduction+(decimal)_totalInterest).ToString("n"),font,alignR));
				yPos+=space;
				section.Height=yPos+30;
				DataTable tbl=new DataTable();
				tbl.Columns.Add("date");
				tbl.Columns.Add("prov");
				tbl.Columns.Add("description");
				tbl.Columns.Add("principal");
				tbl.Columns.Add("interest");
				tbl.Columns.Add("due");
				tbl.Columns.Add("payment");
				tbl.Columns.Add("balance");
				DataRow row;
				for(int i = 0;i<gridCharges.ListGridRows.Count;i++) {
					row=tbl.NewRow();
					row["date"]=gridCharges.ListGridRows[i].Cells[0].Text;
					row["prov"]=gridCharges.ListGridRows[i].Cells[1].Text;
					row["description"]=gridCharges.ListGridRows[i].Cells[2].Text;
					row["principal"]=gridCharges.ListGridRows[i].Cells[3].Text;
					row["interest"]=gridCharges.ListGridRows[i].Cells[4].Text;
					row["due"]=gridCharges.ListGridRows[i].Cells[5].Text;
					row["payment"]=gridCharges.ListGridRows[i].Cells[6].Text;
					row["balance"]=gridCharges.ListGridRows[i].Cells[7].Text;
					tbl.Rows.Add(row);
				}
				QueryObject query=report.AddQuery(tbl,"","",SplitByKind.None,1,true);
				query.AddColumn("ChargeDate",80,FieldValueType.Date,font);
				query.GetColumnHeader("ChargeDate").StaticText="Date";
				query.AddColumn("Provider",75,FieldValueType.String,font);
				query.AddColumn("Description",130,FieldValueType.String,font);
				query.AddColumn("Principal",70,FieldValueType.Number,font);
				query.AddColumn("Interest",52,FieldValueType.Number,font);
				query.AddColumn("Due",70,FieldValueType.Number,font);
				query.AddColumn("Payment",70,FieldValueType.Number,font);
				query.AddColumn("Balance",70,FieldValueType.String,font);
				query.GetColumnHeader("Balance").ContentAlignment=ContentAlignment.MiddleRight;
				query.GetColumnDetail("Balance").ContentAlignment=ContentAlignment.MiddleRight;
				report.ReportObjects.Add(new ReportObject("Note",AreaSectionType.ReportFooter,new Point(x1,20),new Size(500,200),textNote.Text,font,ContentAlignment.TopLeft));
				report.ReportObjects.Add(new ReportObject("Signature",AreaSectionType.ReportFooter,new Point(x1,220),new Size(500,20),"Signature of Guarantor: ____________________________________________",font,alignL));
				if(!report.SubmitQueries()) {
					return;
				}
				FormReportComplex FormR=new FormReportComplex(report);
				FormR.ShowDialog();
			}
		}

		private void butCloseOut_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Closing out this payment plan will remove interest from all future charges "
				+"and make them due immediately.  Do you want to continue?"))
			{
				return;
			}
			if(!ValidateTerms()) {
				return;
			}
			if(!FillCharges()) {
				return;
			}
			//make payment plan charges for the remaining amount of each piece of production
			List<PayPlanCharge> listChargesClosing=new List<PayPlanCharge>();
			List<long> listProcNumsForChargesInDb=_listPayPlanChargesDb.FindAll(x => x.LinkType==PayPlanLinkType.Procedure).Select(x => x.FKey).ToList();
			List<long> listAdjNumsForChargesInDb=_listPayPlanChargesDb.FindAll(x => x.LinkType==PayPlanLinkType.Adjustment).Select(x => x.FKey).ToList();
			List<PayPlanProductionEntry> listProductionEntries=PayPlanProductionEntry.GetWithAmountRemaining(_listPayPlanLinks,_listPayPlanChargesDb);
			foreach(PayPlanProductionEntry entry in listProductionEntries) {
				//These links never had actual charges made for them. Remove them so they can be attached to a new payment plan in the future.
				if(entry.LinkType==PayPlanLinkType.Adjustment && !entry.PriKey.In(listAdjNumsForChargesInDb)) {
					_listPayPlanLinks.RemoveAll(x => x.LinkType==PayPlanLinkType.Adjustment && x.FKey==entry.PriKey);//Sync will delete when saving.
				}
				if(entry.LinkType==PayPlanLinkType.Procedure && !entry.PriKey.In(listProcNumsForChargesInDb)) {
					_listPayPlanLinks.RemoveAll(x => x.LinkType==PayPlanLinkType.Procedure && x.FKey==entry.PriKey);//Sync will delete when saving.
				}
				if(entry.AmountRemaining.IsEqual(entry.AmountOriginal)) {
					continue;//this was a link that never ended up getting charged and saved in the database. Skip it. It was just deleted above. 
				}
				//make a charge for the amount that has yet to be put on the plan
				PayPlanCharge charge=PayPlanEdit.CreateDebitChargeDynamic(_payPlanCur,_famCur,entry.ProvNum,entry.ClinicNum,(double)entry.AmountRemaining,0
					,DateTime.Today,Lans.g(this,"Close Out Charge"),entry.PriKey,entry.LinkType);
				listChargesClosing.Add(charge);
			}
			_payPlanCur.IsClosed=true;
			if(!SaveData(isUiValid:true)) {
				return;
			}
			PayPlanCharges.InsertMany(listChargesClosing);
			DialogResult=DialogResult.OK;
		}

		private PayPlanFrequency GetChargeFrequency() {
			if(radioWeekly.Checked) {
				return PayPlanFrequency.Weekly;
			}
			else if(radioEveryOtherWeek.Checked) {
				return PayPlanFrequency.EveryOtherWeek;
			}
			else if(radioOrdinalWeekday.Checked) {
				return PayPlanFrequency.OrdinalWeekday;
			}
			else if(radioMonthly.Checked) {
				return PayPlanFrequency.Monthly;
			}
			else {
				return PayPlanFrequency.Quarterly;
			}
		}

		private void SetNote() {
			textNote.Text=_payPlanCur.Note+DateTime.Today.ToShortDateString()
				+" - "+Lan.g(this,"Date of Agreement")+": "+textDate.Text
				+", "+Lan.g(this,"Total Amount")+": "+textTotalPrincipal.Text
				+", "+Lan.g(this,"APR")+": "+textAPR.Text
				+", "+Lan.g(this,"Total Cost of Loan")+": "+textTotalCost.Text;
		}

		///<summary>Creates a new sheet from a given Pay plan.</summary>
		private Sheet PayPlanToSheet(PayPlan payPlan) {
			Sheet sheetPP=SheetUtil.CreateSheet(SheetDefs.GetInternalOrCustom(SheetInternalType.PaymentPlan),_patCur.PatNum);
			sheetPP.Parameters.Add(new SheetParameter(true,"payplan") { ParamValue=payPlan });
			sheetPP.Parameters.Add(new SheetParameter(true,"Principal") { ParamValue=_sumAttachedProduction.ToString("n") });
			sheetPP.Parameters.Add(new SheetParameter(true,"totalFinanceCharge") { ParamValue=_totalInterest });
			sheetPP.Parameters.Add(new SheetParameter(true,"totalCostOfLoan") {ParamValue=(_sumAttachedProduction+(decimal)_totalInterest).ToString("n")});
			SheetFiller.FillFields(sheetPP);
			return sheetPP;
		}

		private void checkExcludePast_CheckedChanged(object sender,EventArgs e) {
			FillCharges();
		}

		///<summary>Returns true for successful saving and false if there was a problem. 
		///isUiValid is only true if a previous method running TryGetTermsFromUI and has returned true. 
		///isUiValid is false when another method has run TryGetTermsFromUI and returned false, 
		///meaning an error message has been presented and we don't want to present another here.</summary>
		private bool SaveData(bool isPrinting=false,bool isUiValid=true) {
			if(PayPlans.GetOne(_payPlanCur.PayPlanNum)==null) {
				//The payment plan no longer exists in the database. 
				MsgBox.Show(this,"This payment plan has been deleted by another user.");
				return false;
			}
			if(textAPR.Text=="") {
				textAPR.Text="0";
			}
			if(gridCharges.ListGridRows.Count==0) {
				MsgBox.Show(this,"An amortization schedule must be created first.");
				return false;
			}
			if(!isUiValid || !TryGetTermsFromUI(out PayPlanTerms terms)) {//also validates terms
				return false;
			}
			_payPlanCur.PayPlanDate=PIn.Date(textDate.Text);
			_payPlanCur.IsDynamic=true;
			_payPlanCur.Note=textNote.Text;
			_payPlanCur.ChargeFrequency=terms.Frequency;
			//this will need to change to only account for completed production once TP procs are allowed
			_payPlanCur.CompletedAmt=(double)_sumAttachedProduction;
			_payPlanCur.NumberOfPayments=0;//This should never be set. We only store the payment amount for dynamic payplans. 
			_payPlanCur.PayAmt=(double)terms.PeriodPayment;
			_payPlanCur.APR=terms.APR;
			_payPlanCur.DownPayment=terms.DownPayment;
			_payPlanCur.PaySchedule=PayPlanEdit.GetPayScheduleFromFrequency(terms.Frequency);
			_payPlanCur.DatePayPlanStart=terms.DateFirstPayment;
			List<PayPlanCharge> listDownPaymentCharges=new List<PayPlanCharge>();
			if(_payPlanCur.IsNew) {
				if(!terms.DownPayment.IsZero()) {
					//insert down payment(s) if none exist. Get a new copy from DB so we can guarantee no one else has made this payment plan yet. 
					if(PayPlanCharges.GetForPayPlan(_payPlanCur.PayPlanNum).Count==0) {
						List<PayPlanCharge> listDownPayments=PayPlanCharges.GetForDownPayment(terms,_famCur,_listPayPlanLinks,_payPlanCur);
						foreach(PayPlanCharge downPayment in listDownPayments) {
							PayPlanCharges.Insert(downPayment);
							listDownPaymentCharges.Add(downPayment);
						}
					}
				}
			}
			if(terms.DateFirstPayment.Date==DateTime.Today) {
				//immediately call the code to run the "service" on this payment plan in case they created a plan who's first charge is today. 
				List<PayPlanCharge> listCharges=PayPlanEdit.GetListExpectedCharges(_listPayPlanChargesDb,terms,_famCur,_listPayPlanLinks,_payPlanCur,true
					,listPaySplits:_listPaySplits)
					.FindAll(x => x.ChargeDate <= DateTime.Today);
				if(listCharges.Count > 0) {
					PayPlanCharges.InsertMany(listCharges);
					_listPayPlanChargesDb.AddRange(listCharges);
				}
			}
			else if(listDownPaymentCharges.Count>0) {
				_listPayPlanChargesDb.AddRange(listDownPaymentCharges);
			}
			if(!_payPlanCur.IsLocked && checkProductionLock.Checked) {
				foreach(PayPlanProductionEntry entry in _listPayPlanProductionEntries) {
					//find matching credit.
					PayPlanLink credit=_listPayPlanLinks.FirstOrDefault(x => x.FKey==entry.PriKey && x.LinkType==entry.LinkType);
					if(credit.AmountOverride.IsEqual(0)) {//credit has not been set, set amount override so production never changes.
						credit.AmountOverride=(double)entry.AmountOriginal;
					}
				}
				_payPlanCur.IsLocked=true;
			}
			_payPlanCur.PlanCategory=comboCategory.GetSelectedDefNum();
			_payPlanCur.IsNew=false;
			PayPlans.Update(_payPlanCur);//always saved to db before opening this form
			PayPlanL.MakeSecLogEntries(_payPlanCur,_payPlanOld,signatureBoxWrapper.GetSigChanged(),
				_isSigOldValid,signatureBoxWrapper.SigIsBlank,signatureBoxWrapper.IsValid,isPrinting);
			PayPlanLinks.Sync(_listPayPlanLinks,_payPlanCur.PayPlanNum);
			//When sign & print is clicked (saves the plan) on new plan to let users know it has been saved and some thing can no longer be edited.
			FillUiForSavedPayPlan();
			return true;
		}

		private void butDelete_Click(object sender,System.EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete payment plan?  All debits and credits will also be deleted.")) {
				return;
			}
			try {
				PayPlans.Delete(_payPlanCur);
				//Delete log here since this button doesn't call SaveData().
				SecurityLogs.MakeLogEntry(Permissions.PayPlanEdit,_patCur.PatNum,"Dynamic Payment Plan deleted.",_payPlanOld.PayPlanNum,DateTime.MinValue);
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(_payPlanCur.IsClosed) {
				butOK.Text="OK";
				butDelete.Enabled=true;
				butClosePlan.Enabled=true;
				labelClosed.Visible=false;
				_payPlanCur.IsClosed=false;
				return;
			}
			if(!TryGetTermsFromUI(out PayPlanTerms terms)) {
				return;
			}
			if(!SaveData()) {
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormPayPlanDynamic_Closing(object sender,CancelEventArgs e) {
			if(DialogResult==DialogResult.OK) {
				return;
			}
			if(_payPlanCur.IsNew){
				try{
					PayPlans.Delete(_payPlanCur);
				}
				catch(Exception ex){
					MessageBox.Show(ex.Message);
					e.Cancel=true;
					return;
				}
			}
		}
	}
}
