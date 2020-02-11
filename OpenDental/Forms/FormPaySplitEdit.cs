using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using CodeBase;

namespace OpenDental {
	///<summary>Summary description for FormPaySplitEdit.</summary>
	public partial class FormPaySplitEdit : ODForm {
		#region Public variables
		///<summary></summary>
		public bool IsNew;
		///<summary>The value needed to make the splits balance.</summary>
		public double Remain;
		///<summary>Used to figure out what procedures have amounts left due on them when attaching this splits to a proc. 
		///Splits from the current payment are also included in this calculation.</summary>
		public List<PaySplit> ListSplitsCur;
		///<summary>The pattern in this window is different than other windows.  We generally keep PaySplitCur updated as we go, rather than when we click OK. This is because we sometimes send it out for calcs.</summary>
		public PaySplit PaySplitCur;
		///<summary>PaySplit associations for PaySplitCur</summary>
		public PaySplits.PaySplitAssociated SplitAssociated;
		///<summary>List of PaySplitAssociated for the current payment. Intended to be read only. DO NOT MANIPULATE ITEMS IN LIST.</summary>
		public List<PaySplits.PaySplitAssociated> ListPaySplitAssociated=new List<PaySplits.PaySplitAssociated>();
		#endregion
		#region _private variables
		private bool _isEditAnyway;
		private decimal _remainAmt;
		private decimal _patPort;
		private Family _famCur;
		private PaySplit _paySplitCopy;
		private Procedure ProcCur;
		///<summary>True if the payment for this paysplit is an income transfer.</summary>
		private bool _isIncomeTransfer;
		private Adjustment _adjCur;
		private double _adjPrevPaid;
		///<summary>The pre-paid amount that is remaining if the current split is allocating unearned.</summary>
		private decimal _prePaidRemain;
		private Procedure _procedureOld;
		private Def _unearnedOld;
		///<summary>Cannot manipulate ListPaySPlitAssociated, so we need track the original association when it is detached so that it doesn't
		///affect amounts on FormPaySplitSelect</summary>
		private PaySplits.PaySplitAssociated _paySplitAssociatedDetached;
		///<summary>Flag to prevent us from overwriting the original _paySplitAssociatedDetached 
		///if multipe detaches occur before the window is closed.</summary>
		private bool _isOrigPaySplitAssociatedDetached=false;
		#endregion
		///<summary>The PayPlanCharge that this paysplit is linked to. May be zero if the paysplit is not attached to a payplan or there are no charges
		///due for the payplan.</summary>
		public long PayPlanChargeNum {
			get;set;
		}

		public FormPaySplitEdit(Family famCur,bool isIncomeTransfer){
			InitializeComponent();
			_famCur=famCur;
			_isIncomeTransfer=isIncomeTransfer;
			Lan.F(this);
		}

		private void FormPaySplitEdit_Load(object sender, System.EventArgs e) {
			List<PatientLink> listLinks=PatientLinks.GetLinks(_famCur.ListPats.Select(x => x.PatNum).ToList(),PatientLinkType.Merge);
			List<Patient> listNonMergedPats=_famCur.ListPats.Where(x => !PatientLinks.WasPatientMerged(x.PatNum,listLinks)).ToList();
			//New object to break reference to famCur in calling method/class; avoids removing merged patients from original object.
			_famCur=new Family(listNonMergedPats);
			_paySplitCopy=PaySplitCur.Copy();
			textDateEntry.Text=PaySplitCur.DateEntry.ToShortDateString();
			textDatePay.Text=PaySplitCur.DatePay.ToShortDateString();
			textAmount.Text=PaySplitCur.SplitAmt.ToString("F");
			comboUnearnedTypes.Items.AddDefNone();
			comboUnearnedTypes.Items.AddDefs(Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType,true));
			comboUnearnedTypes.SetSelectedDefNum(PaySplitCur.UnearnedType); 
			if(comboUnearnedTypes.SelectedIndex!=-1){
				_unearnedOld=comboUnearnedTypes.GetSelected<Def>();
			}
			if(PrefC.HasClinicsEnabled) {
				comboClinic.SelectedClinicNum=PaySplitCur.ClinicNum;
			}
			FillComboProv();//also sets the combo to PaySplitCur.ProvNum. Handles 0
			if(PaySplitCur.PayPlanNum==0){
				checkPayPlan.Checked=false;
			}
			else{
				checkPayPlan.Checked=true;
			}
			if(Clinics.IsMedicalPracticeOrClinic(PaySplitCur.ClinicNum)) {
				textProcTooth.Visible=false;
				labelProcTooth.Visible=false;
			}
			if(SplitAssociated!=null) {
				FillSplitAssociated();
			}
			ProcCur=PaySplitCur.ProcNum==0 ? null : Procedures.GetOneProc(PaySplitCur.ProcNum,false);
			_adjCur=PaySplitCur.AdjNum==0 ? null : Adjustments.GetOne(PaySplitCur.AdjNum);
			butAttachPrepay.Enabled=(SplitAssociated?.PaySplitOrig==null);
			if(ProcCur!=null) {
				_procedureOld=ProcCur.Copy();
				tabAdjustment.Enabled=false;//Intellisense doesn't know this is here for some reason.  Shhh it's a secret.
				tabControl.SelectedIndex=0;//Set it on Proc tab automagically (this is just a safety precaution, it should be 0 already).
			}
			else if(_adjCur!=null) {
				tabProcedure.Enabled=false;//Intellisense doesn't know this is here for some reason as well.  Super double secret.
				tabControl.SelectedIndex=1;//Set it on Adjustment tab automagically.
			}
			SetEnabledProc();
			FillPatient();
			FillProcedure();
			FillAdjustment();
		}

		///<summary>Sets the patient GroupBox, provider combobox & picker button, 
		///and clinic combobox enabled/disabled depending on whether a proc is attached.</summary>
		private void SetEnabledProc() {
			if((ProcCur!=null || _adjCur!=null) && !_isEditAnyway && PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
				groupPatient.Enabled=false;
				comboProvider.Enabled=false;
				butPickProv.Enabled=false;
				if(PrefC.HasClinicsEnabled) {
					comboClinic.Enabled=false;
				}
				if(Security.IsAuthorized(Permissions.Setup,true)) {
					labelEditAnyway.Visible=true;
					butEditAnyway.Visible=true;
				}
			}
			else {
				groupPatient.Enabled=true;
				comboProvider.Enabled=true;
				butPickProv.Enabled=true;
				if(PrefC.HasClinicsEnabled) {
					comboClinic.Enabled=true;
				}
				comboUnearnedTypes.Enabled=true;
				labelEditAnyway.Visible=false;
				butEditAnyway.Visible=false;
				checkPatOtherFam.Enabled=true;
			}
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			PaySplitCur.ClinicNum=comboClinic.SelectedClinicNum;
			FillComboProv();
		}

		private void comboProvider_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboProvider.SelectedIndex>-1) {
				PaySplitCur.ProvNum=comboProvider.GetSelectedProvNum();
			}
			else {
				PaySplitCur.ProvNum=0;
			}
			if(_isEditAnyway || PrefC.GetBool(PrefName.AllowPrepayProvider)) {
				return;
			}
			if(PaySplitCur.ProvNum>0) {
				comboUnearnedTypes.SelectedIndex=0;
				comboUnearnedTypes.Enabled=false;
				PaySplitCur.UnearnedType=0;
			}
			else {
				comboUnearnedTypes.Enabled=true;
			}
		}

		private void comboUnearnedTypes_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboUnearnedTypes.SelectedIndex>0) {
				PaySplitCur.UnearnedType=comboUnearnedTypes.GetSelectedDefNum();
			}
			else {
				PaySplitCur.UnearnedType=0;
			}
			if(_isEditAnyway || PrefC.GetBool(PrefName.AllowPrepayProvider)) {
				return;
			}
			if(PaySplitCur.UnearnedType>0) {//If they use an unearned type the provnum must be zero if Edit Anyway isn't pressed
				PaySplitCur.ProvNum=0;
				comboProvider.SelectedIndex=0;
				comboProvider.Enabled=false;
				butPickProv.Enabled=false;
				checkPayPlan.Checked=false;
				checkPayPlan.Enabled=false;
			}
			else {
				comboProvider.Enabled=true;
				butPickProv.Enabled=true;
				checkPayPlan.Enabled=true;
			}
		}

		private void butPickProv_Click(object sender,EventArgs e) {
			FormProviderPick formp = new FormProviderPick(comboProvider.Items.GetAll<Provider>());
			formp.SelectedProvNum=PaySplitCur.ProvNum;
			formp.ShowDialog();
			if(formp.DialogResult!=DialogResult.OK) {
				return;
			}
			PaySplitCur.ProvNum=formp.SelectedProvNum;
			comboProvider.SetSelectedProvNum(PaySplitCur.ProvNum);
		}

		///<summary>Fills combo provider based on which clinic is selected and attempts to preserve provider selection if any.</summary>
		private void FillComboProv() {
			comboProvider.Items.Clear();
			comboProvider.Items.AddProvNone();
			comboProvider.Items.AddProvsAbbr(Providers.GetProvsForClinic(PaySplitCur.ClinicNum));
			comboProvider.SetSelectedProvNum(PaySplitCur.ProvNum);
		}

		private void butRemainder_Click(object sender, System.EventArgs e) {
			textAmount.Text=Remain.ToString("F");
		}

		///<summary>PaySplit.Patient is one value that is always kept in synch with the display.  If program changes PaySplit.Patient, then it will run this method to update the display.  If user changes display, then _MouseDown is run to update the PaySplit.Patient.</summary>
		private void FillPatient(){
			listPatient.Items.Clear();
			for(int i=0;i<_famCur.ListPats.Length;i++){
				listPatient.Items.Add(_famCur.GetNameInFamLFI(i));
				if(PaySplitCur.PatNum==_famCur.ListPats[i].PatNum){
					listPatient.SelectedIndex=i;
				}
			}
			//this can happen if user unchecks the "Is From Other Fam" box. Need to reset.
			if(PaySplitCur.PatNum==0){
				listPatient.SelectedIndex=0;
				//the initial patient will be the first patient in the family, usually guarantor
				PaySplitCur.PatNum=_famCur.ListPats[0].PatNum;
			}
			if(listPatient.SelectedIndex==-1){//patient not in family
				checkPatOtherFam.Checked=true;
				textPatient.Visible=true;
				listPatient.Visible=false;
				textPatient.Text=Patients.GetLim(PaySplitCur.PatNum).GetNameLF();
			}
			else{//show the family list that was just filled
				checkPatOtherFam.Checked=false;
				textPatient.Visible=false;
				listPatient.Visible=true;
			}
		}

		private void checkPatOtherFam_Click(object sender, System.EventArgs e) {
			//this happens after the check change has been registered
			if(checkPatOtherFam.Checked){
				FormPatientSelect FormPS=new FormPatientSelect();
				FormPS.SelectionModeOnly=true;
				FormPS.ShowDialog();
				if(FormPS.DialogResult!=DialogResult.OK){
					checkPatOtherFam.Checked=false;
					return;
				}
				PaySplitCur.PatNum=FormPS.SelectedPatNum;
			}
			else{//switch to family view
				PaySplitCur.PatNum=0;//this will reset the selected patient to current patient
			}
			butAttachPrepay.Enabled=true;
			if(!_isEditAnyway) {//When user clicks Edit Anyway they are specifically trying to correct a bad split, so don't clear it out.
				ProcCur=null;
				FillProcedure();
			}
			FillAdjustment();
			FillPatient();
		}

		private void listPatient_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			if(listPatient.SelectedIndex==-1){
				return;
			}
			PaySplitCur.PatNum=_famCur.ListPats[listPatient.SelectedIndex].PatNum;
		}

		private void FillProcedure(){
			if(ProcCur==null){
				textProcDate.Text="";
				textProcProv.Text="";
				textProcTooth.Text="";
				textProcDescription.Text="";
				textProcFee.Text="";
				textProcWriteoff.Text="";
				textProcInsPaid.Text="";
				textProcInsEst.Text="";
				textProcAdj.Text="";
				textProcPrevPaid.Text="";
				textProcPaidHere.Text="";
				labelProcRemain.Text="";
				butAttachProc.Enabled=true;
				comboProvider.Enabled=true;
				comboClinic.Enabled=true;
				comboUnearnedTypes.Enabled=true;
				textPatient.Enabled=true;
				groupPatient.Enabled=true;
				butPickProv.Enabled=true;
				tabAdjustment.Enabled=true;
				if(_adjCur==null) {
					checkPatOtherFam.Enabled=true;
					checkPayPlan.Enabled=true;
				}
				return;
			}
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(ProcCur.PatNum);
			Adjustment[] arrAdjustments=Adjustments.Refresh(ProcCur.PatNum);
			List<PaySplit> listPaySplitsForProc=PaySplits.Refresh(ProcCur.PatNum).Where(x => x.ProcNum==ProcCur.ProcNum).ToList();
			List<PaySplit> listPaySplitsForProcPaymentWindow=ListSplitsCur.Where(x => x.ProcNum==ProcCur.ProcNum && x.PayNum==PaySplitCur.PayNum).ToList();
			//Add new paysplits created for the current paysplits payment.
			listPaySplitsForProc.AddRange(listPaySplitsForProcPaymentWindow.Where(x => x.SplitNum==0));
			//Remove paysplits that have been deleted in the payment window but have not been saved to db. We don't want to use these paysplits 
			//when calculating procPrevPaid.
			listPaySplitsForProc.RemoveAll(x => !listPaySplitsForProcPaymentWindow.Any(y => y.IsSame(x)) && x.PayNum==PaySplitCur.PayNum);
			textProcDate.Text=ProcCur.ProcDate.ToShortDateString();
			textProcProv.Text=Providers.GetAbbr(ProcCur.ProvNum);
			textProcTooth.Text=Tooth.ToInternat(ProcCur.ToothNum);
			textProcDescription.Text="";
			if(ProcCur.ProcStatus==ProcStat.TP) {
				textProcDescription.Text="(TP) ";
			}
			textProcDescription.Text+=ProcedureCodes.GetProcCode(ProcCur.CodeNum).Descript;
			double procWriteoff=-ClaimProcs.ProcWriteoff(listClaimProcs,ProcCur.ProcNum);
			double procInsPaid=-ClaimProcs.ProcInsPay(listClaimProcs,ProcCur.ProcNum);
			double procInsEst=-ClaimProcs.ProcEstNotReceived(listClaimProcs,ProcCur.ProcNum);
			double procAdj=Adjustments.GetTotForProc(ProcCur.ProcNum,arrAdjustments);
			//next line will still work even if IsNew
			int countSplitsAttached;
			double procPrevPaid=-PaySplits.GetTotForProc(ProcCur.ProcNum,listPaySplitsForProc.ToArray(),PaySplitCur,out countSplitsAttached);
			//Intelligently sum the values associated to the procedure, claim procs, and adjustments via status instead of blindly adding them together.
			_patPort=ClaimProcs.GetPatPortion(ProcCur,listClaimProcs,arrAdjustments.ToList());
			textProcFee.Text=ProcCur.ProcFeeTotal.ToString("F");
			if(procWriteoff==0){
				textProcWriteoff.Text="";
			}
			else{
				textProcWriteoff.Text=procWriteoff.ToString("F");
			}
			if(procInsPaid==0){
				textProcInsPaid.Text="";
			}
			else{
				textProcInsPaid.Text=procInsPaid.ToString("F");
			}
			if(procInsEst==0){
				textProcInsEst.Text="";
			}
			else{
				textProcInsEst.Text=procInsEst.ToString("F");
			}
			if(procAdj==0){
				textProcAdj.Text="";
			}
			else{
				textProcAdj.Text=procAdj.ToString("F");
			}
			if(procPrevPaid==0 && countSplitsAttached==0){
				textProcPrevPaid.Text="";
			}
			else{
				textProcPrevPaid.Text=procPrevPaid.ToString("F");
			}
			if(PrefC.HasClinicsEnabled) {
				comboClinic.SelectedClinicNum=PaySplitCur.ClinicNum;
			}
			butAttachProc.Enabled=false;
			if(!_isEditAnyway && PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
				comboProvider.Enabled=false;
				comboClinic.Enabled=false;
				butPickProv.Enabled=false;
			}
			if(ProcCur.ProcStatus!=ProcStat.C) {
				comboUnearnedTypes.Enabled=true;
				if(ProcCur.ProcStatus==ProcStat.TP) {
					checkPayPlan.Checked=false;
				}
			}
			else {//There is no good way to determine if a proc previously had TP unearned so we will just keep whatever loaded in and disable the box. 
				comboUnearnedTypes.SelectedIndex=0;//First item is always None, if there is a procedure it cannot be a prepayment, regardless of enforce fully.
				comboUnearnedTypes.Enabled=false;
			}
			textPatient.Enabled=false;
			groupPatient.Enabled=false;
			checkPatOtherFam.Enabled=false;
			//Find the combo option for the procedure's clinic and provider.  If they don't exist in the list (are hidden) then it will set the text of the combo box instead.
			comboProvider.SetSelectedProvNum(PaySplitCur.ProvNum);
			if(PrefC.HasClinicsEnabled) {
				comboClinic.SelectedClinicNum=PaySplitCur.ClinicNum;//not sure why this is here again
			}
			//Proc selected will always be for the pat this paysplit was made for
			listPatient.SelectedIndex=_famCur.ListPats.ToList().FindIndex(x => x.PatNum==PaySplitCur.PatNum);
			ComputeTotals();
			tabAdjustment.Enabled=false;
		}

		private void FillAdjustment() {
			if(_adjCur==null) {
				textAdjDate.Text="";
				textAdjProv.Text="";
				textAdjAmt.Text="";
				textAdjPrevUsed.Text="";
				_adjPrevPaid=0;
				textAdjPaidHere.Text="";
				labelAdjRemaining.Text="";
				tabProcedure.Enabled=true;//Intellisense doesn't know about this, but it does exist.
				butAttachAdjust.Enabled=true;
				if(ProcCur==null) {
					checkPatOtherFam.Enabled=true;
					groupPatient.Enabled=true;
				}
				checkPayPlan.Enabled=true;
				return;
			}
			textAdjDate.Text=_adjCur.AdjDate.ToShortDateString();
			textAdjProv.Text=Providers.GetAbbr(_adjCur.ProvNum);
			textAdjAmt.Text=_adjCur.AdjAmt.ToString("F");//Adjustment's original amount
			//Don't include any splits on current payment - Since they could be modified and DB doesn't know about it yet.
			_adjPrevPaid=Adjustments.GetAmtAllocated(_adjCur.AdjNum,PaySplitCur.PayNum);
			//ListSplitsCur contains current paysplit, we need to remove it somehow.  PaySplitCur could have SplitNum=0 though.
			List<PaySplit> listSplits=ListSplitsCur.FindAll(x => x.AdjNum==_adjCur.AdjNum);
			if(listSplits.Count>0) {
				_adjPrevPaid+=listSplits.Sum(x => x.SplitAmt);
				//There needs to be something here so _adjPrevPaid isn't adjusted by current split amt if the split isn't in listSplits
				if(PaySplitCur.IsNew || (!PaySplitCur.IsNew && listSplits.Exists(x => x.SplitNum==PaySplitCur.SplitNum))) {
					_adjPrevPaid-=PaySplitCur.SplitAmt;//To prevent double counting the current split
				}
			}
			textAdjPrevUsed.Text=(-_adjPrevPaid).ToString("F");//How much was previously used
			if(textAmount.errorProvider1.GetError(textAmount)!="" || string.IsNullOrWhiteSpace(textAmount.Text)) {
				textAdjPaidHere.Text="";
			}
			else{
				textAdjPaidHere.Text=PIn.Double(textAmount.Text).ToString("F");//How much is used here
			}
			ComputeTotals();
			butAttachAdjust.Enabled=false;
			if(PaySplitCur.PayPlanNum==0) {
				checkPayPlan.Checked=false;
				checkPayPlan.Enabled=false;
			}
			if(!_isEditAnyway && PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
				comboProvider.Enabled=false;
				comboClinic.Enabled=false;
				butPickProv.Enabled=false;
			}
			comboUnearnedTypes.SelectedIndex=0;//First item is always None, if there is a procedure it cannot be a prepayment, regardless of enforce fully.
			comboUnearnedTypes.Enabled=false;
			textPatient.Enabled=false;
			groupPatient.Enabled=false;
			checkPatOtherFam.Enabled=false;
			//Find the combo option for the adjustment's clinic and provider.  If they don't exist in the list (are hidden) then it will set the text of the combo box instead.
			comboProvider.SetSelectedProvNum(_adjCur.ProvNum);
			if(PrefC.HasClinicsEnabled) {
				comboClinic.SelectedClinicNum=_adjCur.ClinicNum;
			}
			//Proc selected will always be for the pat this paysplit was made for
			listPatient.SelectedIndex=_famCur.ListPats.ToList().FindIndex(x => x.PatNum==_adjCur.PatNum);
			tabProcedure.Enabled=false;//paysplits cannot have both procedure and adjustment
		}

		private void FillSplitAssociated() {
			textPrePayDate.Text="";
			textPrePayType.Text="";
			textPrePayAmt.Text="";
			textPrePaidHere.Text="";
			textPrePaidRemain.Text="";
			textPrePaidElsewhere.Text="";
			butAttachPrepay.Enabled=true;
			if(SplitAssociated!=null && SplitAssociated.PaySplitOrig!=null) {
				SetSplitAssociatedText();
			}
		}

		private void SetSplitAssociatedText() {
			PaySplit paySplitPrePayOrig=PaySplits.GetOriginalPrepayment(SplitAssociated.PaySplitOrig);
			//if the paySplitPrePayOrig is still null, check to see if the original is in the list of ListPaySplitAssociation
			if(paySplitPrePayOrig==null && ListPaySplitAssociated!=null) {
				//SplitAssociated.PaySplitOrig absolutely not null, see PaySplits.GetOriginalPrepayment() which would have thrown.
				PaySplits.PaySplitAssociated splitAssociated=ListPaySplitAssociated.Find(x=>x.PaySplitLinked==SplitAssociated.PaySplitOrig);
				if(splitAssociated!=null && splitAssociated.PaySplitOrig!=null) {//splitAssociated.PaySplitOrig can be null sometimes.
					paySplitPrePayOrig=PaySplits.GetOne(splitAssociated.PaySplitOrig.SplitNum);
				}
			}
			//if the paySplitPrePayOrig is still null, check to see if the original is SplitAssociated.PaySplitOrig
			if(paySplitPrePayOrig==null && SplitAssociated.PaySplitOrig.UnearnedType!=0) {
				paySplitPrePayOrig=SplitAssociated.PaySplitOrig;
			}
			List<PaySplit> listPaySplitAllocatedElseWhere=new List<PaySplit>();
			DateTime datePay=SplitAssociated.PaySplitOrig.DatePay;
			string prePayType=Defs.GetName(DefCat.PaySplitUnearnedType,SplitAssociated.PaySplitOrig.UnearnedType);
			decimal amt=PIn.Decimal(textAmount.Text);
			decimal prepayAmt=0;
			decimal usedHere=Math.Abs(amt);
			decimal prePayUsedElsewhere=0;
			if(paySplitPrePayOrig!=null) {
				//add the prepayments allocated from the database. Excludes the current payment.
				listPaySplitAllocatedElseWhere.AddRange(PaySplits.GetAllocatedElseWhere(paySplitPrePayOrig.SplitNum)
					.FindAll(x=>x.PayNum!=PaySplitCur.PayNum));
				List<PaySplits.PaySplitAssociated> listAssociated=ListPaySplitAssociated
					.FindAll(x=> x.PaySplitLinked==SplitAssociated.PaySplitOrig || x.PaySplitOrig==SplitAssociated.PaySplitLinked).ToList();
				List<PaySplit> listOrig=listAssociated.Select(x=>x.PaySplitOrig).ToList();
				List<PaySplit> listLinked=listAssociated.Select(x=>x.PaySplitLinked).ToList();
				List<PaySplit> listPaySplitFromGrid=ListSplitsCur.FindAll(x=> x.SplitAmt<0 && !listOrig.Exists(y => y==x) && !listLinked.Exists(y => y==x) 
					&& x.TagOD!=PaySplitCur.TagOD && x.FSplitNum==SplitAssociated.PaySplitOrig.SplitNum);
				//add only paysplits from the left grid. 
				listPaySplitAllocatedElseWhere.AddRange(listPaySplitFromGrid);
				prepayAmt=(decimal)paySplitPrePayOrig.SplitAmt;
			}
			prePayUsedElsewhere=Math.Abs(listPaySplitAllocatedElseWhere.Sum(y => (decimal)y.SplitAmt));
			textPrePayDate.Text=datePay.ToShortDateString();
			textPrePayType.Text=prePayType;
			textPrePayAmt.Text=prepayAmt.ToString("F");//Total Original prepay amount.
			textPrePaidHere.Text=usedHere.ToString("F");//The prepay amount used here
			textPrePaidElsewhere.Text=prePayUsedElsewhere.ToString("F");//paySplitTotal- Sum of all allocated paysplits
			_prePaidRemain=(prepayAmt-prePayUsedElsewhere-usedHere);
			textPrePaidRemain.Text=_prePaidRemain.ToString("F");
			butAttachPrepay.Enabled=false;
		}

		///<summary>Does not alter any of the proc amounts except PaidHere and Remaining.  Also calculates Adjust Amounts</summary>
		private void ComputeTotals() {
			double procPaidHere=0;
			double adjPaidHere=0;
			if(textAmount.errorProvider1.GetError(textAmount)==""){
				procPaidHere=-PIn.Double(textAmount.Text);
				adjPaidHere=+PIn.Double(textAmount.Text);	
			}
			if(procPaidHere==0){
				textProcPaidHere.Text="";
				textAdjPaidHere.Text="";	
			}
			else{
				textProcPaidHere.Text=procPaidHere.ToString("F");
				textAdjPaidHere.Text=adjPaidHere.ToString("F");
			}
			labelAdjRemaining.Text="";
			labelProcRemain.Text="";
			_remainAmt=0;
			if(ProcCur!=null) {
				_remainAmt=_patPort+(decimal)procPaidHere+PIn.Decimal(textProcPrevPaid.Text);
				labelProcRemain.Text=_remainAmt.ToString("c");
			}
			else if(_adjCur!=null) {
				_remainAmt=(decimal)_adjCur.AdjAmt-(decimal)_adjPrevPaid-(decimal)adjPaidHere;
				labelAdjRemaining.Text=_remainAmt.ToString("c");//How much is remaining
			}
		}

		private void textAmount_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			//can not use textAmount_TextChanged without redesigning the validDouble control
			if(textPrePayAmt.Text!="") {
				return;
			}
			ComputeTotals();
		}

		private void TextAmount_TextChanged(object sender,EventArgs e) {
			//can not use textAmount_TextChanged without redesigning the validDouble control
			if(textPrePayAmt.Text!="") {
				return;
			}
			ComputeTotals();
		}

		///<summary>Attaches procedure, sets the selected provider, and fills Procedure information.</summary>
		private void butAttachProc_Click(object sender, System.EventArgs e) {
			FormProcSelect FormPS=new FormProcSelect(PaySplitCur.PatNum,false);
			FormPS.ListSplitsCur = ListSplitsCur;
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK){
				return;
			}
			ProcCur=FormPS.ListSelectedProcs[0];
			PaySplitCur.ProvNum=FormPS.ListSelectedProcs[0].ProvNum;
			PaySplitCur.ClinicNum=FormPS.ListSelectedProcs[0].ClinicNum;
			if(ProcCur.ProcStatus==ProcStat.TP) {
				PaySplitCur.UnearnedType=PrefC.GetLong(PrefName.TpUnearnedType);//use default tp unearned for tp procedures.
				comboUnearnedTypes.SetSelectedDefNum(PaySplitCur.UnearnedType);
			}
			else {
				comboUnearnedTypes.SelectedIndex=0;
				PaySplitCur.UnearnedType=0;
			}
			SetEnabledProc();
			FillProcedure();
			FillAdjustment();
		}

		private void butDetachProc_Click(object sender, System.EventArgs e) {
			if(ProcCur!=null) {
				ListSplitsCur.Where(x => x.ProcNum==ProcCur.ProcNum && x.IsSame(PaySplitCur))
					.ForEach(x => x.ProcNum=0);
			}
			ProcCur=null;
			SetEnabledProc();
			FillProcedure();
			FillAdjustment();
		}

		private void butAttachAdjust_Click(object sender,EventArgs e) {
			FormAdjustSelect FormAdj=new FormAdjustSelect();
			FormAdj.PatNumCur=PaySplitCur.PatNum;
			FormAdj.PaySplitCurAmt=PIn.Double(textAmount.Text);
			FormAdj.PaySplitCur=PaySplitCur;
			FormAdj.ListSplitsForPayment=ListSplitsCur;
			FormAdj.PayNumCur=PaySplitCur.PayNum;
			if(FormAdj.ShowDialog()!=DialogResult.OK){
				return;
			}
			_adjCur=FormAdj.SelectedAdj;
			PaySplitCur.ProvNum=FormAdj.SelectedAdj.ProvNum;
			PaySplitCur.ClinicNum=FormAdj.SelectedAdj.ClinicNum;
			PaySplitCur.UnearnedType=0;
			SetEnabledProc();
			FillProcedure();
			FillAdjustment();
		}

		private void butDetachAdjust_Click(object sender,EventArgs e) {
			if(_adjCur!=null) {
				ListSplitsCur.Where(x => x.AdjNum==_adjCur.AdjNum && x.IsSame(PaySplitCur))
					.ForEach(x => x.AdjNum=0);
			}
			_adjCur=null;
			SetEnabledProc();//think about this.
			FillProcedure();
			FillAdjustment();
		}

		private void butEditAnyway_Click(object sender,EventArgs e) {
			_isEditAnyway=true;
			SetEnabledProc();
		}

		private void butAttachPrepay_Click(object sender,EventArgs e) {
			if(!textAmount.IsValid) {
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			double amount=PIn.Double(textAmount.Text);
			if(amount==0) {
				MsgBox.Show(this,"Please enter an amount");
				return;
			}
			bool isNegSplit=true;
			FormPaySplitSelect FormPSS=new FormPaySplitSelect(PaySplitCur.PatNum);
			FormPSS.IsPrePay=true;
			if(amount>0) {
				isNegSplit=false;
				//pass over the list of prepayments from the ListSplitCur. This will allow users to associated the negative split with a procedure.
				//will only show the splits that are negative, prepayments, and not associated to another paysplit. 
				List<PaySplit> listNegSplitAssociated=ListPaySplitAssociated.Select(x=>x.PaySplitOrig).ToList();
				FormPSS.ListUnallocatedSplits=ListSplitsCur.FindAll(x => x.SplitAmt<0 && x.UnearnedType!=0 && !x.In(listNegSplitAssociated)).ToList();
			}
			FormPSS.SplitNumCur=PaySplitCur.SplitNum;
			FormPSS.ListPaySplitAssociated=ListPaySplitAssociated;
			FormPSS.SplitAssociatedDetached=_paySplitAssociatedDetached;//Can be null
			if(FormPSS.ShowDialog()!=DialogResult.OK) {
				return;
			}
			if(FormPSS.ListSelectedSplits[0].IsSame(PaySplitCur)) {
				MsgBox.Show(this,"Please choose a prepayment that is not the current split.");
				return;
			}
			SplitAssociated=new PaySplits.PaySplitAssociated(FormPSS.ListSelectedSplits[0],PaySplitCur);
			PaySplitCur.FSplitNum=FormPSS.ListSelectedSplits[0].SplitNum;
			if(isNegSplit) {//if negative then we're allocating money from the original prepayment.
				butAttachProc.Enabled=false;
				butDetachProc.Enabled=false;
				if(!PrefC.GetBool(PrefName.AllowPrepayProvider)) {
					comboProvider.Enabled=false;
					butPickProv.Enabled=false;  
				}
				if(comboUnearnedTypes.SelectedIndex==0) {//unearned has not been set
					comboUnearnedTypes.SetSelectedDefNum(SplitAssociated.PaySplitOrig.UnearnedType);
					PaySplitCur.UnearnedType=SplitAssociated.PaySplitOrig.UnearnedType;
				}
				comboUnearnedTypes.Enabled=false;
			}
			//Always switch when negative so it matches the original, only switch for positive proc splits when prepayment has provider
			if(isNegSplit || SplitAssociated.PaySplitOrig.ProvNum!=0) {
				comboProvider.SetSelectedProvNum(SplitAssociated.PaySplitOrig.ProvNum);
				PaySplitCur.ProvNum=SplitAssociated.PaySplitOrig.ProvNum;
			}
			FillSplitAssociated();
		}

		private void butDetachPrepay_Click(object sender,EventArgs e) {
			labelPrePayWarning.Visible=false;
			PaySplit splitCur=ListSplitsCur.FirstOrDefault(x => x.TagOD==PaySplitCur.TagOD);
			if(splitCur!=null) {
				splitCur.FSplitNum=0;//list and object are same but different. List is from copy so need to modify both.
			}
			PaySplitCur.FSplitNum=0;
			if(!_isOrigPaySplitAssociatedDetached) {
				_paySplitAssociatedDetached=SplitAssociated;
				_isOrigPaySplitAssociatedDetached=true;
			}
			SplitAssociated=null;
			if(_isEditAnyway || (ProcCur==null && _adjCur==null)) {
				comboProvider.Enabled=true;
				butPickProv.Enabled=true;
				comboUnearnedTypes.Enabled=true;
			}
			FillSplitAssociated();
		}

		///<summary>Get the selected pay plan's current charges. If there is a charge, attach the split to that charge.</summary>
		private void AttachPlanCharge(PayPlan payPlan, long guar) {
			//get all current charges for that pay plan. If there are no current charges, don't allow the pay plan attach. 
			List<PayPlanCharge> listPayPlanChargesCurrent=PayPlanCharges.GetDueForPayPlan(payPlan,guar);
				if(listPayPlanChargesCurrent.Count==0) {
					//No current payments due for patient. Payment may be made ahead of schedule if procedure is attached.
					PayPlanChargeNum = 0;
				}
				else { 
					PayPlanChargeNum=listPayPlanChargesCurrent.OrderBy(x => x.ChargeDate).First().PayPlanChargeNum;//get oldest
				}
		}

		private void checkPayPlan_Click(object sender, System.EventArgs e) {
			if(checkPayPlan.Checked){
				if(checkPatOtherFam.Checked){//prevents a bug.
					checkPayPlan.Checked=false;
					return;
				}
				if(comboUnearnedTypes.SelectedIndex!=0 && ProcCur!=null && ProcCur.ProcStatus==ProcStat.TP) {
					MsgBox.Show("Treatment planned unearned cannot be applied to payment plans.");
					checkPayPlan.Checked=false;
					return;
				}
				//get all plans where the selected patient is the patnum or the guarantor of the payplan. Do not include insurance payment plans
				List<PayPlan> payPlanList=PayPlans.GetForPatNum(_famCur.ListPats[listPatient.SelectedIndex].PatNum).Where(x => x.PlanNum == 0).ToList();
				if(payPlanList.Count==0){//no valid plans
					MsgBox.Show(this,"The selected patient is not the guarantor for any payment plans.");
					checkPayPlan.Checked=false;
					return;
				}
				if(payPlanList.Count==1){ //if there is only one valid payplan
					PaySplitCur.PayPlanNum=payPlanList[0].PayPlanNum;
					AttachPlanCharge(payPlanList[0],payPlanList[0].Guarantor);
					return;
				}
				//more than one valid PayPlan
				FormPayPlanSelect FormPPS=new FormPayPlanSelect(payPlanList);
				//FormPPS.ValidPlans=payPlanList;
				FormPPS.ShowDialog();
				if(FormPPS.DialogResult==DialogResult.Cancel){
					checkPayPlan.Checked=false;
					return;
				}
				PaySplitCur.PayPlanNum=FormPPS.SelectedPayPlanNum; 
				PayPlan selectPayPlan=payPlanList.FirstOrDefault(x => x.PayPlanNum==PaySplitCur.PayPlanNum);
				//get the selected pay plan's current charges. If there is a charge, attach the split to that charge.
				AttachPlanCharge(selectPayPlan,selectPayPlan.Guarantor);
			}
			else{//payPlan unchecked
				PaySplitCur.PayPlanNum=0;
			}
		}

		private string SecurityLogEntryHelper(string oldVal,string newVal,string textInLog) {
			if(oldVal!=newVal) {
				return "\r\n "+textInLog+" changed from '"+oldVal+"' to '"+newVal+"'";
			}
			return "";
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(!MsgBox.Show(this,true,"Delete Item?")) {
				return;
			}
			if(IsNew) {
				PaySplitCur=null;
				DialogResult=DialogResult.Cancel;
				return;
			}
			//This is the main problem with leaving it up to engineers to manually set public variables before showing forms...
			//We have been getting null reference reports from this security log entry.
			//Only check if PaySplitCur is null because _paySplitCopy gets set OnLoad() which must have been invoked especially if they clicked Delete.
			if(PaySplitCur!=null) {
				SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,PaySplitCur.PatNum,"Paysplit deleted for: "+Patients.GetLim(PaySplitCur.PatNum).GetNameLF()
					+", "+PaySplitCur.SplitAmt.ToString("c")+", with payment type '"+Payments.GetPaymentTypeDesc(Payments.GetPayment(PaySplitCur.PayNum))+"'",
					0,_paySplitCopy.SecDateTEdit);
			}
			//If there are objects in the list and the current paysplit is associated to another paysplit.
			if(!ListPaySplitAssociated.IsNullOrEmpty() && ListPaySplitAssociated.Exists(x => x.ContainsSplit(PaySplitCur))) {
				MsgBox.Show(this,"Attached splits have been allocated elsewhere.  Please delete those first.");
				return;
			}
			PaySplitCur=null;
			DialogResult=DialogResult.OK;
		}

		private bool IsValid() {
			if(textAmount.errorProvider1.GetError(textAmount)!=""	|| textDatePay.errorProvider1.GetError(textDatePay)!=""){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return false;
			}
			//check for TP pre-pay changes. If money has been detached from procedure it needs to be transferred to regular unearned if had been hidden.
			if(_procedureOld!=null && _procedureOld.ProcStatus==ProcStat.TP && ProcCur==null && !string.IsNullOrEmpty(_unearnedOld?.ItemValue)) {
				//user has detached the hidden paysplit 
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Hidden split is no longer attached to a procedure. Change unearned type to default?")) {
					comboUnearnedTypes.SetSelectedDefNum(PrefC.GetLong(PrefName.PrepaymentUnearnedType));
					PaySplitCur.UnearnedType=comboUnearnedTypes.GetSelectedDefNum();
				}
				if(!PrefC.GetBool(PrefName.AllowPrepayProvider)) {
					PaySplitCur.ProvNum=0;
				}
			}
			double amount=PIn.Double(textAmount.Text);
			if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully && PaySplitCur.UnearnedType!=0 && ProcCur!=null 
				&& !_isEditAnyway && ProcCur.ProcStatus!=ProcStat.TP) 
			{
				MsgBox.Show(this,"Cannot have an unallocated split that also has an attached procedure.");
				return false;
			}
			if(ProcCur!=null && _adjCur!=null) {//should not be possible, but as an extra precaution because this is not allowed. 
				MsgBox.Show(this,"Cannot have a paysplit with both a procedure and an adjustment.");
				return false;
			}
			if(_remainAmt<0 && ProcCur!=null) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Remaining amount is negative.  Continue?","Overpaid Procedure Warning")) {
					return false;
				}
			}
      if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully && ProcCur==null && PaySplitCur.UnearnedType==0 
					&& _adjCur==null) 
			{
				MsgBox.Show(this,"You must attach a procedure or adjustment to this payment.");
				return false;
      }
			if(PaySplitCur.UnearnedType!=0 && amount.IsLessThan(0)) {//Allocating money from the original pre-payment
				PaySplit paySplitOriginal=SplitAssociated?.PaySplitOrig??null;
				if(_isIncomeTransfer && paySplitOriginal==null) {
					//To handle the case when they are manually making their prepayment and they forget to attach the original prepayment split.
					//Only when this split is negative because they are free to correct previous mistakes with a positive split. 
					MsgBox.Show(this,"You must attach a prepayment to this split.");
					return false;
				}
				if(paySplitOriginal!=null) {
					SetSplitAssociatedText();//Recalculate _prePaidRemain
					if(_prePaidRemain.IsLessThanZero() && !MsgBox.Show(this,MsgBoxButtons.YesNo,
						"Warning! You are attempting to allocate more money from unearned than there are funds available. Do you still want to continue?")) 
					{
						return false;
					}
				}
			}
			//Provider and Unearned combos will be correct at this point, based on ProvNum or UnearnedType.
			//Unearned type and provider are set in SelectionChangeCommitted events for the respective combo boxes, when rigorous and provs not allowed
			if(!_isEditAnyway && PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
				PaySplit split=SplitAssociated?.PaySplitOrig??new PaySplit();
				if(split.ProvNum!=0 && ProcCur!=null && split.ProvNum!=ProcCur.ProvNum) {
					MsgBox.Show(this,"Procedure provider and original paysplit provider do not match.");
					return false;
				}
			}
			else if(comboUnearnedTypes.SelectedIndex==0 && comboProvider.SelectedIndex==0) {
				MsgBox.Show(this,"Please select an unearned type or a provider.");
				return false;
			}
			//at this point in time, TP procs are allowed to have providers even if provs are typically not allowed on prepayments.
			if((ProcCur==null || ProcCur.ProcStatus==ProcStat.C) && PaySplitCur.UnearnedType!=0) {
				if(PaySplitCur.ProvNum>0 && !PrefC.GetBool(PrefName.AllowPrepayProvider)) {
					PaySplitCur.UnearnedType=0;
				}
				else if(PaySplitCur.ProvNum<=0){
					if(comboUnearnedTypes.SelectedIndex==0 
						&& !MsgBox.Show(this,MsgBoxButtons.YesNo,"Having a provider of \"None\" will mark this paysplit as a prepayment.  Continue?")) 
					{
						return false;
					}
					PaySplitCur.ProvNum=0;//This means it's unallocated.
					if(comboUnearnedTypes.SelectedIndex==0) {
						PaySplitCur.UnearnedType=PrefC.GetLong(PrefName.PrepaymentUnearnedType);
					}
					else {
						PaySplitCur.UnearnedType=comboUnearnedTypes.GetSelectedDefNum();
					}
				}
			}
			return true;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(!IsValid()) {
				return;
			}
			double amount=PIn.Double(textAmount.Text);
			PaySplitCur.DatePay=PIn.Date(textDatePay.Text);//gets overwritten anyway
			PaySplitCur.SplitAmt=amount;
			PaySplitCur.ProcNum=ProcCur == null ? 0 : ProcCur.ProcNum;
			PaySplitCur.AdjNum=_adjCur == null ? 0 : _adjCur.AdjNum;
			if(IsNew) {
				string secLogText="Paysplit created";
				if(_isEditAnyway) {
					secLogText+=" using Edit Anyway";
				}
				secLogText+=" with provider "+Providers.GetAbbr(PaySplitCur.ProvNum);
				if(Clinics.GetAbbr(PaySplitCur.ClinicNum)!="") {
					secLogText+=", clinic "+Clinics.GetAbbr(PaySplitCur.ClinicNum);
				}
				secLogText+=", amount "+PaySplitCur.SplitAmt.ToString("F");
				SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,PaySplitCur.PatNum,secLogText);
			}
			else {
				string secLogText="Paysplit edited";
				if(_isEditAnyway) {
					secLogText+=" using Edit Anyway";
				}
				secLogText+=SecurityLogEntryHelper(Providers.GetAbbr(_paySplitCopy.ProvNum),Providers.GetAbbr(PaySplitCur.ProvNum),"provider");
				secLogText+=SecurityLogEntryHelper(Clinics.GetAbbr(_paySplitCopy.ClinicNum),Clinics.GetAbbr(PaySplitCur.ClinicNum),"clinic");
				secLogText+=SecurityLogEntryHelper(_paySplitCopy.SplitAmt.ToString("F"),PaySplitCur.SplitAmt.ToString("F"),"amount");
				secLogText+=SecurityLogEntryHelper(_paySplitCopy.PatNum.ToString(),PaySplitCur.PatNum.ToString(),"patient number");
				SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,PaySplitCur.PatNum,secLogText,0,_paySplitCopy.SecDateTEdit);
			}
			DialogResult=DialogResult.OK;
		}

		private void ButCancel_Click(object sender, System.EventArgs e) {
			if(IsNew) {
				PaySplitCur=null;
			}
			DialogResult=DialogResult.Cancel;
		}

		private void FormPaySplitEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult==DialogResult.OK || PaySplitCur==null) {
				return;
			}
			PaySplitCur.ClinicNum=_paySplitCopy.ClinicNum;
			PaySplitCur.DateEntry=_paySplitCopy.DateEntry;
			PaySplitCur.DatePay=_paySplitCopy.DatePay;
			PaySplitCur.DiscountType=_paySplitCopy.DiscountType;
			PaySplitCur.IsDiscount=_paySplitCopy.IsDiscount;
			PaySplitCur.IsInterestSplit=_paySplitCopy.IsInterestSplit;
			PaySplitCur.PatNum=_paySplitCopy.PatNum;
			PaySplitCur.PayNum=_paySplitCopy.PayNum;
			PaySplitCur.PayPlanNum=_paySplitCopy.PayPlanNum;
			PaySplitCur.FSplitNum=_paySplitCopy.FSplitNum;
			PaySplitCur.ProcDate=_paySplitCopy.ProcDate;
			PaySplitCur.ProcNum=_paySplitCopy.ProcNum;
			PaySplitCur.ProvNum=_paySplitCopy.ProvNum;
			PaySplitCur.SecDateTEdit=_paySplitCopy.SecDateTEdit;
			PaySplitCur.SecUserNumEntry=_paySplitCopy.SecUserNumEntry;
			PaySplitCur.SplitAmt=_paySplitCopy.SplitAmt;
			PaySplitCur.SplitNum=_paySplitCopy.SplitNum;
			PaySplitCur.UnearnedType=_paySplitCopy.UnearnedType;
			ListSplitsCur.Where(x => x.IsSame(_paySplitCopy)).ForEach(x => x.ProcNum=_paySplitCopy.ProcNum);
		}
	}
}
