using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using OpenDentBusiness;
using CodeBase;
using SparksToothChart;
using OpenDental.UI;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenDental {
///<summary></summary>
	public partial class FormProcEdit : ODForm {
		///<summary>Mostly used for permissions.</summary>
		public bool IsNew;
		public long OrionProvNum;
		public bool OrionDentist;
		public List<ClaimProcHist> HistList;
		public List<ClaimProcHist> LoopList;
		private ProcedureCode _procedureCode2;
		private Procedure _procCur;
		private Procedure _procOld;
		private List<ClaimProc> _listClaimProcsForProc;
		private ArrayList _paySplitsForProc;
		private ArrayList _adjustmentsForProc;
		private Patient _patCur;
		private Family _famCur;
		private List<InsPlan> _listInsPlans;
		///<summary>Lazy loaded, do not directly use this variable, use the property instead.</summary>
		private List<SubstitutionLink> _listSubstLinks=null;
		///<summary>List of all payments (not paysplits) that this procedure is attached to.</summary>
		private List<Payment> _listPaymentsForProc;
		private const string APPBAR_AUTOMATION_API_MESSAGE = "EZNotes.AppBarStandalone.Auto.API.Message"; 
		private const uint MSG_RESTORE=2;
		private const uint MSG_GETLASTNOTE=3;
		private List<PatPlan> _listPatPlans;
		private List<Benefit> _listBenefits;
		private bool _sigChanged;
		///<summary>This keeps the noteChanged event from erasing the signature when first loading.</summary>
		private bool _isStartingUp;
		private List<Claim> _listClaims;
		private bool _startedAttachedToClaim;
		private OrionProc _orionProcCur;
		private OrionProc _orionProcOld;
		private DateTime _cancelledScheduleByDate;
		private List<InsSub> _listInsSubs;
		private List<Procedure> _listCanadaLabFees;
		private Snomed _snomedBodySite=null;
		private bool _isQuickAdd=false;
		///<summary>Users can temporarily log in on this form.  Defaults to Security.CurUser.</summary>
		private Userod _curUser=Security.CurUser;
		///<summary>True if the user clicked the Change User button.</summary>
		private bool _hasUserChanged;
		///<summary></summary>
		private long _selectedProvOrderNum;
		///<summary>If this procedure is attached to an ordering referral, then this varible will not be null.</summary>
		private Referral _referralOrdering=null;
		private const string _autoNotePromptRegex=@"\[Prompt:""[a-zA-Z_0-9 ]+""\]";
		///<summary>True only when modifications to this canadian lab proc will affect the attached parent proc ins estimate.</summary>
		private bool _isEstimateRecompute=false;
		private OrthoProcLink _orthoProcLink;
		private List<Def> _listDiagnosisDefs;
		private List<Def> _listPrognosisDefs;
		private List<Def> _listTxPriorityDefs;
		private List<Def> _listBillingTypeDefs;
		///<summary>Most of the data necessary to load this form.</summary>
		private ProcEdit.LoadData _loadData;
		///<summary>There are a number of places in this form that need fees, but none of them are heavily used.  This will help a little.  Lazy loaded, do not directly use this variable, use the property instead.</summary>
		private Lookup<FeeKey2,Fee> _lookupFees;
		///<summary>See _lookupFees.  Sometimes, we need a list instead of a lookup.</summary>
		private List<Fee> _listFees;
		private List<ToothInitial> _listPatToothInitials=null;
		///<summary>All primary teeth currently being displayed in the UI, but stored as permanent teeth so that indexes are easy to calculate.</summary>
		private List<string> _listPriTeeth=null;

		private List<SubstitutionLink> ListSubstLinks {
			get {
				if(_listSubstLinks==null) {
					_listSubstLinks=SubstitutionLinks.GetAllForPlans(_listInsPlans);
				}
				return _listSubstLinks;
			}
		}

		private Lookup<FeeKey2,Fee> LookupFees {
			get {
				if(_lookupFees==null) {
					FillFees();
				}
				return _lookupFees;
			}
		}

		private void FillFees(){
			List<ProcedureCode> listProcedureCodes=new List<ProcedureCode>(){ ProcedureCodes.GetProcCode(_procCur.CodeNum) };
			List<Procedure> listProcedures=new List<Procedure>(){_procCur };
			_listFees=Fees.GetListFromObjects(listProcedureCodes,listProcedures.Select(x=>x.MedicalCode).ToList(),listProcedures.Select(x=>x.ProvNum).ToList(),
				_patCur.PriProv,_patCur.SecProv,_patCur.FeeSched,_listInsPlans,listProcedures.Select(x=>x.ClinicNum).ToList(),null,//appts not needed
				ListSubstLinks,_patCur.DiscountPlanNum);
			_lookupFees=(Lookup<FeeKey2,Fee>)_listFees.ToLookup(x => new FeeKey2(x.CodeNum,x.FeeSched));
		}

		private List<Fee> ListFees {
			get {
				if(_listFees==null) {
					FillFees();
				}
				return _listFees;
			}
		}

		///<summary>Inserts are not done within this dialog, but must be done ahead of time from outside.  You must specify a procedure to edit, and only the changes that are made in this dialog get saved.  Only used when double click in Account, Chart, TP, and in ContrChart.AddProcedure().  The procedure may be deleted if new, and user hits Cancel.</summary>
		public FormProcEdit(Procedure proc,Patient patCur,Family famCur,bool isQuickAdd=false,List<ToothInitial> listPatToothInitials=null) {
			_procCur=proc;
			_procOld=proc.Copy();
			_patCur=patCur;
			_famCur=famCur;
			//HistList=null;
			//LoopList=null;
			InitializeComponent();
			Lan.F(this);
			if(!PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance)) {
				tabControl.TabPages.Remove(tabPageMedical);
				//groupMedical.Visible=false;
			}
			_isQuickAdd=isQuickAdd;
			if(isQuickAdd) {
				this.WindowState=FormWindowState.Minimized;
			}
			_listPatToothInitials=listPatToothInitials;
		}

		private void FormProcInfo_Load(object sender,System.EventArgs e) {
			if(PrefC.IsODHQ) {
				labelTaxEst.Visible=true;
				textTaxAmt.Visible=true;
				textTaxAmt.Text=POut.Double(_procCur.TaxAmt);
				if(_procCur.ProcStatus==ProcStat.C) {
					labelTaxEst.Text="Tax Amt";
				}
			}
			_loadData=ProcEdit.GetLoadData(_procCur,_patCur,_famCur);
			_orthoProcLink=_loadData.OrthoProcedureLink;
			if(_orthoProcLink!=null) {
				textProcFee.Enabled=false;
				butChange.Enabled=false;
				checkNoBillIns.Enabled=false;
				textDiscount.Enabled=false;
				butAddAdjust.Enabled=false;
				butAddExistAdj.Enabled=false;
			}
			_listInsSubs=_loadData.ListInsSubs;
			_listInsPlans=_loadData.ListInsPlans;
			signatureBoxWrapper.SetAllowDigitalSig(true);
			_listClaimProcsForProc=new List<ClaimProc>();
			//Set the title bar to show the patient's name much like the main screen does.
			this.Text+=" - "+_patCur.GetNameLF();
			textDateEntry.Text=_procCur.DateEntryC.ToShortDateString();
			if(PrefC.GetBool(PrefName.EasyHidePublicHealth)){
				labelPlaceService.Visible=false;
				comboPlaceService.Visible=false;
				labelSite.Visible=false;
				textSite.Visible=false;
				butPickSite.Visible=false;
			}
			if(!Security.IsAuthorized(Permissions.ProcEditShowFee,true)){
				labelAmount.Visible=false;
				textProcFee.Visible=false;
				labelTaxEst.Visible=false;
				textTaxAmt.Visible=false;
			}
			_listClaims=_loadData.ListClaims;
			_procedureCode2=ProcedureCodes.GetProcCode(_procCur.CodeNum);
			if(_procCur.ProcStatus==ProcStat.C && PrefC.GetBool(PrefName.ProcLockingIsAllowed) && !_procCur.IsLocked) {
				butLock.Visible=true;
			}
			else {
				butLock.Visible=false;
			}
			if(IsNew){
				if(_procCur.ProcStatus==ProcStat.C){
					if(!_isQuickAdd && !Security.IsAuthorized(Permissions.ProcComplCreate)){
						DialogResult=DialogResult.Cancel;
						return;
					}
				}
				//SetControls();
				//return;
			}
			else{
				if(_procCur.ProcStatus==ProcStat.C){
					textDiscount.Enabled=false;
					if(_procCur.IsLocked) {//Whether locking is currently allowed, this proc may have been locked previously.
						butOK.Enabled=false;//use this state to cascade permission to any form opened from here
						butDelete.Enabled=false;
						butChange.Enabled=false;
						butEditAnyway.Enabled=false;
						butSetComplete.Enabled=false;
						butSnomedBodySiteSelect.Enabled=false;
						butNoneSnomedBodySite.Enabled=false;
						labelLocked.Visible=true;
						butAppend.Visible=true;
						textNotes.ReadOnly=true;//just for visual cue.  No way to save changes, anyway.
						textNotes.BackColor=SystemColors.Control;
						butInvalidate.Visible=true;
						butInvalidate.Location=butLock.Location;
					}
					else{
						butInvalidate.Visible=false;
					}
				}
			}
			//ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
			_listClaimProcsForProc=_loadData.ListClaimProcsForProc;
			_listPatPlans=_loadData.ListPatPlans;
			_listBenefits=_loadData.ListBenefits;
			if(Procedures.IsAttachedToClaim(_procCur,_listClaimProcsForProc)){
				_startedAttachedToClaim=true;
				//however, this doesn't stop someone from creating a claim while this window is open,
				//so this is checked at the end, too.
				panel1.Enabled=false;
				comboProcStatus.Enabled=false;
				checkNoBillIns.Enabled=false;
				butChange.Enabled=false;
				butEditAnyway.Visible=true;
				butSetComplete.Enabled=false;
			}
			if(Procedures.IsAttachedToClaim(_procCur,_listClaimProcsForProc,false)) {
				butDelete.Enabled=false;
				labelClaim.Visible=true;
				butAddEstimate.Enabled=false;
			}
			if(PrefC.GetBool(PrefName.EasyHideClinical)){
				labelDx.Visible=false;
				comboDx.Visible=false;
				labelPrognosis.Visible=false;
				comboPrognosis.Visible=false;
			}
			if(PrefC.GetBool(PrefName.EasyHideMedicaid)) {
				comboBillingTypeOne.Visible=false;
				labelBillingTypeOne.Visible=false;
				comboBillingTypeTwo.Visible=false;
				labelBillingTypeTwo.Visible=false;
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				//groupCanadianProcType.Location=new Point(106,301);
				groupProsth.Visible=false;
				labelClaimNote.Visible=false;
				textClaimNote.Visible=false;
				butBF.Text=Lan.g(this,"B/V");//vestibular instead of facial
				butV.Text=Lan.g(this,"5");
				if(_procedureCode2.IsCanadianLab) { //Prevent lab fees from having lab fees attached.
					labelCanadaLabFee1.Visible=false;
					textCanadaLabFee1.Visible=false;
					labelCanadaLabFee2.Visible=false;
					textCanadaLabFee2.Visible=false;
				}
				else {
					_listCanadaLabFees=Procedures.GetCanadianLabFees(_procCur.ProcNum);
					if(_listCanadaLabFees.Count>0) {
						textCanadaLabFee1.Text=_listCanadaLabFees[0].ProcFee.ToString("n");
						if(_listCanadaLabFees[0].ProcStatus==ProcStat.C) {
							textCanadaLabFee1.ReadOnly=true;
						}
					}
					if(_listCanadaLabFees.Count>1) {
						textCanadaLabFee2.Text=_listCanadaLabFees[1].ProcFee.ToString("n");
						if(_listCanadaLabFees[1].ProcStatus==ProcStat.C) {
							textCanadaLabFee2.ReadOnly=true;
						}
					}
				}
			}
			else {
				tabControl.Controls.Remove(tabPageCanada);
				//groupCanadianProcType.Visible=false;
			}
			if(Programs.UsingOrion) {
				if(IsNew) {
					_orionProcCur=new OrionProc();
					_orionProcCur.ProcNum=_procCur.ProcNum;
					if(_procCur.ProcStatus==ProcStat.EO) {
						_orionProcCur.Status2=OrionStatus.E;
					}
					else {
						_orionProcCur.Status2=OrionStatus.TP;
					}
				}
				else {
					_orionProcCur=OrionProcs.GetOneByProcNum(_procCur.ProcNum);
					if(_procCur.DateTP<MiscData.GetNowDateTime().Date && 
						(_orionProcCur.Status2==OrionStatus.CA_EPRD
						|| _orionProcCur.Status2==OrionStatus.CA_PD
						|| _orionProcCur.Status2==OrionStatus.CA_Tx
						|| _orionProcCur.Status2==OrionStatus.R)) {//Not allowed to edit procedures with these statuses that are older than a day.
						MsgBox.Show(this,"You cannot edit refused or cancelled procedures.");
						DialogResult=DialogResult.Cancel;
					}
					if(_orionProcCur.Status2==OrionStatus.C || _orionProcCur.Status2==OrionStatus.CR || _orionProcCur.Status2==OrionStatus.CS) {
						textNotes.Enabled=false;
					}
				}
				textDateTP.ReadOnly=true;
				//panelOrion.Visible=true;
				butAddEstimate.Visible=false;
				checkNoBillIns.Visible=false;
				gridIns.Visible=false;
				butAddAdjust.Visible=false;
				gridPay.Visible=false;
				gridAdj.Visible=false;
				comboProcStatus.Enabled=false;
				labelAmount.Visible=false;
				textProcFee.Visible=false;
				labelPriority.Visible=false;
				comboPriority.Visible=false;
				butSetComplete.Visible=false;
				labelSetComplete.Visible=false;
			}
			else {
				tabControl.Controls.Remove(tabPageOrion);
			}
			if(Programs.UsingOrion || PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance)) {
				labelEndTime.Visible=true;
				textTimeEnd.Visible=true;
				butNow.Visible=true;
				labelTimeFinal.Visible=true;
				textTimeFinal.Visible=true;
			}
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				textNotes.HideSelection=false;//When text is selected programmatically using our Search function, this causes the selection to be visible to the users.
			}
			else {
				butSearch.Visible=false;
				labelSnomedBodySite.Visible=false;
				textSnomedBodySite.Visible=false;
				butSnomedBodySiteSelect.Visible=false;
				butNoneSnomedBodySite.Visible=false;
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				radioS1.Text="03";//Sextant 1 in the United States is sextant 03 in Canada.
				radioS2.Text="04";//Sextant 2 in the United States is sextant 04 in Canada.
				radioS3.Text="05";//Sextant 3 in the United States is sextant 05 in Canada.
				radioS4.Text="06";//Sextant 4 in the United States is sextant 06 in Canada.
				radioS5.Text="07";//Sextant 5 in the United States is sextant 07 in Canada.
				radioS6.Text="08";//Sextant 6 in the United States is sextant 08 in Canada.
			}
			SetOrderingProvider(null);//Clears both the internal ordering and referral ordering providers.
			if(_procCur.ProvOrderOverride!=0) {
				SetOrderingProvider(Providers.GetProv(_procCur.ProvOrderOverride));
			}
			else if(_procCur.OrderingReferralNum!=0) {
				Referral referral;
				Referrals.TryGetReferral(_procCur.OrderingReferralNum,out referral);
				SetOrderingReferral(referral);
			}
			FillComboClinic();
			comboClinic.SelectedClinicNum=_procCur.ClinicNum;
			FillComboProv();
			comboProv.SetSelectedProvNum(_procCur.ProvNum);
			_isStartingUp=true;
			FillControlsOnStartup();
			SetControlsUpperLeft();
			SetControlsEnabled(_isQuickAdd);
			FillReferral(false);
			FillIns(false);
			FillPayments(false);
			FillAdj();
			_isStartingUp=false;
			bool canEditNote=false;
			if(Security.IsAuthorized(Permissions.ProcedureNoteFull,true)) {
				canEditNote=true;
			}
			else if(Security.IsAuthorized(Permissions.ProcedureNoteUser,true) && (_procCur.UserNum==Security.CurUser.UserNum || signatureBoxWrapper.SigIsBlank)) {
				canEditNote=true;//They have limited permission and this is their note that they signed.
			}
			if(!canEditNote) {
				textNotes.ReadOnly=true;
				buttonUseAutoNote.Enabled=false;
				butEditAutoNote.Enabled=false;
				signatureBoxWrapper.Enabled=false;
				labelPermAlert.Visible=true;
				butAppend.Enabled=false;//don't allow appending notes either.
				butChangeUser.Enabled=false;
			}
			bool hasAutoNotePrompt=Regex.IsMatch(textNotes.Text,_autoNotePromptRegex);
			butEditAutoNote.Visible=hasAutoNotePrompt;
			//string retVal=ProcCur.Note+ProcCur.UserNum.ToString();
			//MsgBoxCopyPaste msgb=new MsgBoxCopyPaste(retVal);
			//msgb.ShowDialog();
			if(_isQuickAdd) {
				textDate.Enabled=false;
				ProcNoteUiHelper();//Add any default notes.
				butOK_Click(this,new EventArgs());
				if(this.DialogResult!=DialogResult.OK) {
					this.WindowState=FormWindowState.Normal;
					this.CenterToScreen();
					this.BringToFront();
				}
			}
		}

		private void FillComboClinic() {
			long clinicNum=comboClinic.SelectedClinicNum;
			comboClinic.SetUser(_curUser);//Not Security.CurUser
			if(clinicNum!=-1){
				comboClinic.SelectedClinicNum=clinicNum;
			}
		}

		private void ComboClinic_SelectionChangeCommitted(object sender, EventArgs e){
			FillComboProv();
		}

		private void butPickProv_Click(object sender,EventArgs e) {
			FormProviderPick formp = new FormProviderPick(comboProv.Items.GetAll<Provider>());
			formp.SelectedProvNum=comboProv.GetSelectedProvNum();
			formp.ShowDialog();
			if(formp.DialogResult!=DialogResult.OK) {
				return;
			}
			comboProv.SetSelectedProvNum(formp.SelectedProvNum);
		}

		private void butPickOrderProvInternal_Click(object sender,EventArgs e) {
			FormProviderPick formP = new FormProviderPick(comboProv.Items.GetAll<Provider>());
			formP.SelectedProvNum=_selectedProvOrderNum;
			formP.ShowDialog();
			if(formP.DialogResult!=DialogResult.OK) {
				return;
			}
			SetOrderingProvider(Providers.GetProv(formP.SelectedProvNum));
		}

		private void butPickOrderProvReferral_Click(object sender,EventArgs e) {
			FormReferralSelect form=new FormReferralSelect();
			form.IsSelectionMode=true;
			form.IsDoctorSelectionMode=true;
			form.IsShowPat=false;
			form.IsShowDoc=true;
			form.IsShowOther=false;
			form.ShowDialog();
			if(form.DialogResult!=DialogResult.OK) {
				return;
			}
			SetOrderingReferral(form.SelectedReferral);
		}

		private void butNoneOrderProv_Click(object sender,EventArgs e) {
			SetOrderingProvider(null);//Clears both the internal ordering and referral ordering providers.
		}

		private void SetOrderingProvider(Provider prov) {
			if(prov==null) {
				_selectedProvOrderNum=0;
				textOrderingProviderOverride.Text="";
			}
			else {
				_selectedProvOrderNum=prov.ProvNum;
				textOrderingProviderOverride.Text=prov.GetFormalName()+"  NPI: "+(prov.NationalProvID.Trim()==""?"Missing":prov.NationalProvID);
			}
			_referralOrdering=null;
		}

		private void SetOrderingReferral(Referral referral) {
			_referralOrdering=referral;
			if(referral==null) {
				textOrderingProviderOverride.Text="";
			}
			else {
				textOrderingProviderOverride.Text=referral.GetNameFL()+"  NPI: "+(referral.NationalProvID.Trim()==""?"Missing":referral.NationalProvID);
			}
			_selectedProvOrderNum=0;
		}

		///<summary>Fills combo provider based on which clinic is selected and attempts to preserve provider selection if any.</summary>
		private void FillComboProv() {
			long provNum=comboProv.GetSelectedProvNum();
			comboProv.Items.Clear();
			comboProv.Items.AddProvsAbbr(Providers.GetProvsForClinic(comboClinic.SelectedClinicNum));
			comboProv.SetSelectedProvNum(provNum);
		}

		///<summary>ONLY run on startup. Fills the basic controls, except not the ones in the upper left panel which are handled in SetControlsUpperLeft.</summary>
		private void FillControlsOnStartup(){
			if(_procCur.ProcStatus==ProcStat.D && _procCur.IsLocked) {//only set this when coming in with this status. 
				comboProcStatus.Items.Add(Lan.g("Procedures","Invalidated"),ProcStat.D);
			}
			else{
				comboProcStatus.Items.Add(Lan.g("Procedures","Treatment Planned"),ProcStat.TP);
				//For the "Complete" option, instead of showing current value,
				//show what the value would represent if set to complete, in case user changes to complete from another status.
				bool isInProcess=ProcMultiVisits.IsProcInProcess(_procCur.ProcNum,true);
				comboProcStatus.Items.Add(Lan.g("Procedures","Complete"+(isInProcess?" (In Process)":"")),ProcStat.C);
				if(!PrefC.GetBool(PrefName.EasyHideClinical)) {
					comboProcStatus.Items.Add(Lan.g("Procedures","Existing-Current Prov"),ProcStat.EC);
					comboProcStatus.Items.Add(Lan.g("Procedures","Existing-Other Prov"),ProcStat.EO);
					comboProcStatus.Items.Add(Lan.g("Procedures","Referred Out"),ProcStat.R);
					comboProcStatus.Items.Add(Lan.g("Procedures","Condition"),ProcStat.Cn);
				}
				if(_procCur.ProcStatus==ProcStat.TPi) {//only set this when coming in with that status, users should not choose this status otherwise.
					comboProcStatus.Items.Add(Lan.g("Procedures","Treatment Planned Inactive"),ProcStat.TPi);
				}
			}
			comboProcStatus.SetSelectedEnum(_procCur.ProcStatus);//has no effect if enum isn't in the list
			if(comboProcStatus.GetSelected<ProcStat>()==ProcStat.TPi) {
				comboProcStatus.Enabled=false;
				butSetComplete.Enabled=false;
			}
			if(comboProcStatus.GetSelected<ProcStat>()==ProcStat.D){//an invalidated proc
				comboProcStatus.Enabled=false;
				butInvalidate.Visible=false;
				butOK.Enabled=false;
				butDelete.Enabled=false;
				butChange.Enabled=false;
				butEditAnyway.Enabled=false;
				butSetComplete.Enabled=false;
				butAddEstimate.Enabled=false;
				butAddAdjust.Enabled=false;
			}
			//if clinical is hidden, then there's a chance that no item is selected at this point.
			_listDiagnosisDefs=Defs.GetDefsForCategory(DefCat.Diagnosis,true);
			_listPrognosisDefs=Defs.GetDefsForCategory(DefCat.Prognosis,true);
			_listTxPriorityDefs=Defs.GetDefsForCategory(DefCat.TxPriorities,true);
			_listBillingTypeDefs=Defs.GetDefsForCategory(DefCat.BillingTypes,true);
			comboDx.Items.Clear();
			for(int i=0;i<_listDiagnosisDefs.Count;i++){
				comboDx.Items.Add(_listDiagnosisDefs[i].ItemName);
				if(_listDiagnosisDefs[i].DefNum==_procCur.Dx)
					comboDx.SelectedIndex=i;
			}
			comboPrognosis.Items.Clear();
			comboPrognosis.Items.Add(Lan.g(this,"no prognosis"));
			comboPrognosis.SelectedIndex=0;
			for(int i=0;i<_listPrognosisDefs.Count;i++) {
				comboPrognosis.Items.Add(_listPrognosisDefs[i].ItemName);
				if(_listPrognosisDefs[i].DefNum==_procCur.Prognosis)
					comboPrognosis.SelectedIndex=i+1;
			}
			checkHideGraphics.Checked=_procCur.HideGraphics;
			if(Programs.UsingOrion && this.IsNew && !OrionDentist){
				_procCur.ProvNum=Providers.GetOrionProvNum(_procCur.ProvNum);//Returns 0 if logged in as non provider.
				comboProv.SetSelectedProvNum(_procCur.ProvNum);
				FillComboProv();//Second time this is called, but only if using Orion.
			}//ProvNum of 0 will be required to change before closing form.
			comboPriority.Items.Clear();
			comboPriority.Items.Add(Lan.g(this,"no priority"));
			comboPriority.SelectedIndex=0;
			for(int i=0;i<_listTxPriorityDefs.Count;i++){
				comboPriority.Items.Add(_listTxPriorityDefs[i].ItemName);
				if(_listTxPriorityDefs[i].DefNum==_procCur.Priority)
					comboPriority.SelectedIndex=i+1;
			}
			comboBillingTypeOne.Items.Clear();
			comboBillingTypeOne.Items.Add(Lan.g(this,"none"));
			comboBillingTypeOne.SelectedIndex=0;
			for(int i=0;i<_listBillingTypeDefs.Count;i++) {
				comboBillingTypeOne.Items.Add(_listBillingTypeDefs[i].ItemName);
				if(_listBillingTypeDefs[i].DefNum==_procCur.BillingTypeOne)
					comboBillingTypeOne.SelectedIndex=i+1;
			}
			comboBillingTypeTwo.Items.Clear();
			comboBillingTypeTwo.Items.Add(Lan.g(this,"none"));
			comboBillingTypeTwo.SelectedIndex=0;
			for(int i=0;i<_listBillingTypeDefs.Count;i++) {
				comboBillingTypeTwo.Items.Add(_listBillingTypeDefs[i].ItemName);
				if(_listBillingTypeDefs[i].DefNum==_procCur.BillingTypeTwo)
					comboBillingTypeTwo.SelectedIndex=i+1;
			}
			textBillingNote.Text=_procCur.BillingNote;
			textNotes.Text=_procCur.Note;
			comboPlaceService.Items.Clear();
			comboPlaceService.Items.AddRange(Enum.GetNames(typeof(PlaceOfService)));
			comboPlaceService.SelectedIndex=(int)_procCur.PlaceService;
			//checkHideGraphical.Checked=ProcCur.HideGraphical;
			textSite.Text=Sites.GetDescription(_procCur.SiteNum);
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				if(_procCur.CanadianTypeCodes==null || _procCur.CanadianTypeCodes=="") {
					checkTypeCodeX.Checked=true;
				}
				else {
					if(_procCur.CanadianTypeCodes.Contains("A")) {
						checkTypeCodeA.Checked=true;
					}
					if(_procCur.CanadianTypeCodes.Contains("B")) {
						checkTypeCodeB.Checked=true;
					}
					if(_procCur.CanadianTypeCodes.Contains("C")) {
						checkTypeCodeC.Checked=true;
					}
					if(_procCur.CanadianTypeCodes.Contains("E")) {
						checkTypeCodeE.Checked=true;
					}
					if(_procCur.CanadianTypeCodes.Contains("L")) {
						checkTypeCodeL.Checked=true;
					}
					if(_procCur.CanadianTypeCodes.Contains("S")) {
						checkTypeCodeS.Checked=true;
					}
					if(_procCur.CanadianTypeCodes.Contains("X")) {
						checkTypeCodeX.Checked=true;
					}
				}
			}
			else{
				if(_procedureCode2.IsProsth){
					listProsth.Items.Add(Lan.g(this,"No"));
					listProsth.Items.Add(Lan.g(this,"Initial"));
					listProsth.Items.Add(Lan.g(this,"Replacement"));
					switch(_procCur.Prosthesis){
						case "":
							listProsth.SelectedIndex=0;
							break;
						case "I":
							listProsth.SelectedIndex=1;
							break;
						case "R":
							listProsth.SelectedIndex=2;
							break;
					}
					if(_procCur.DateOriginalProsth.Year>1880){
						textDateOriginalProsth.Text=_procCur.DateOriginalProsth.ToShortDateString();
					}
					checkIsDateProsthEst.Checked=_procCur.IsDateProsthEst;
				}
				else{
					groupProsth.Visible=false;
				}
			}
			textDiscount.Text=_procCur.Discount.ToString("f");
			//medical
			textMedicalCode.Text=_procCur.MedicalCode;
			if(_procCur.IcdVersion==9) {
				checkIcdVersion.Checked=false;
			}
			else {//ICD-10
				checkIcdVersion.Checked=true;
			}
			SetIcdLabels();
			textDiagnosisCode.Text=_procCur.DiagnosticCode;
			textDiagnosisCode2.Text=_procCur.DiagnosticCode2;
			textDiagnosisCode3.Text=_procCur.DiagnosticCode3;
			textDiagnosisCode4.Text=_procCur.DiagnosticCode4;
			checkIsPrincDiag.Checked=_procCur.IsPrincDiag;
			textCodeMod1.Text = _procCur.CodeMod1;
			textCodeMod2.Text = _procCur.CodeMod2;
			textCodeMod3.Text = _procCur.CodeMod3;
			textCodeMod4.Text = _procCur.CodeMod4;
			textUnitQty.Text = _procCur.UnitQty.ToString();
			comboUnitType.Items.Clear();
			_snomedBodySite=Snomeds.GetByCode(_procCur.SnomedBodySite);
			if(_snomedBodySite==null) {
				textSnomedBodySite.Text="";
			}
			else {
				textSnomedBodySite.Text=_snomedBodySite.Description;
			}
			for(int i=0;i<Enum.GetNames(typeof(ProcUnitQtyType)).Length;i++) {
				comboUnitType.Items.Add(Enum.GetNames(typeof(ProcUnitQtyType))[i]);
			}
			comboUnitType.SelectedIndex=(int)_procCur.UnitQtyType;
			textRevCode.Text = _procCur.RevCode;
			//DrugNDC is handled in SetControlsUpperLeft
			comboDrugUnit.Items.Clear();
			for(int i=0;i<Enum.GetNames(typeof(EnumProcDrugUnit)).Length;i++){
				comboDrugUnit.Items.Add(Enum.GetNames(typeof(EnumProcDrugUnit))[i]);
			}
			comboDrugUnit.SelectedIndex=(int)_procCur.DrugUnit;
			if(_procCur.DrugQty!=0){
				textDrugQty.Text=_procCur.DrugQty.ToString();
			}
			checkIsEmergency.Checked=(_procCur.Urgency==ProcUrgency.Emergency);
			textClaimNote.Text=_procCur.ClaimNote;
			textUser.Text=Userods.GetName(_procCur.UserNum);//might be blank. Will change automatically if user changes note or alters sig.
			string keyData=GetSignatureKey();
			signatureBoxWrapper.FillSignature(_procCur.SigIsTopaz,keyData,_procCur.Signature);
			if(Programs.UsingOrion) {//panelOrion.Visible) {
				comboDPC.Items.Clear();
				//comboDPC.Items.AddRange(Enum.GetNames(typeof(OrionDPC)));
				comboDPC.Items.Add("Not Specified");
				comboDPC.Items.Add("None");
				comboDPC.Items.Add("1A-within 1 day");
				comboDPC.Items.Add("1B-within 30 days");
				comboDPC.Items.Add("1C-within 60 days");
				comboDPC.Items.Add("2-within 120 days");
				comboDPC.Items.Add("3-within 1 year");
				comboDPC.Items.Add("4-no further treatment/appt");
				comboDPC.Items.Add("5-no appointment needed");
				comboDPCpost.Items.Clear();
				comboDPCpost.Items.Add("Not Specified");
				comboDPCpost.Items.Add("None");
				comboDPCpost.Items.Add("1A-within 1 day");
				comboDPCpost.Items.Add("1B-within 30 days");
				comboDPCpost.Items.Add("1C-within 60 days");
				comboDPCpost.Items.Add("2-within 120 days");
				comboDPCpost.Items.Add("3-within 1 year");
				comboDPCpost.Items.Add("4-no further treatment/appt");
				comboDPCpost.Items.Add("5-no appointment needed");
				comboStatus.Items.Clear();
				comboStatus.Items.Add("TP-treatment planned");
				comboStatus.Items.Add("C-completed");
				comboStatus.Items.Add("E-existing prior to incarceration");
				comboStatus.Items.Add("R-refused treatment");
				comboStatus.Items.Add("RO-referred out to specialist");
				comboStatus.Items.Add("CS-completed by specialist");
				comboStatus.Items.Add("CR-completed by registry");
				comboStatus.Items.Add("CA_Tx-cancelled, tx plan changed");
				comboStatus.Items.Add("CA_EPRD-cancelled, eligible parole");
				comboStatus.Items.Add("CA_P/D-cancelled, parole/discharge");
				comboStatus.Items.Add("S-suspended, unacceptable plaque");
				comboStatus.Items.Add("ST-stop clock, multi visit");
				comboStatus.Items.Add("W-watch");
				comboStatus.Items.Add("A-alternative");
				comboStatus.SelectedIndex=0;
				ProcedureCode pc=ProcedureCodes.GetProcCodeFromDb(_procCur.CodeNum);
				checkIsRepair.Visible=pc.IsProsth;
				if(_procCur.DateTP.Year<1880) {
					textDateTP.Text="";
				}
				else {
					textDateTP.Text=_procCur.DateTP.ToShortDateString();
				}
				BitArray ba=new BitArray(new int[] { (int)_orionProcCur.Status2 });//should nearly always be non-zero
				for(int i=0;i<ba.Length;i++) {
					if(ba[i]) {
						comboStatus.SelectedIndex=i;
						break;
					}
				}
				if(!IsNew) {
					_orionProcOld=_orionProcCur.Copy();
					comboDPC.SelectedIndex=(int)_orionProcCur.DPC;
					comboDPCpost.SelectedIndex=(int)_orionProcCur.DPCpost;
					if(_orionProcCur.DPC==OrionDPC.NotSpecified ||
						_orionProcCur.DPC==OrionDPC.None ||
						_orionProcCur.DPC==OrionDPC._4 ||
						_orionProcCur.DPC==OrionDPC._5) {
						labelScheduleBy.Visible=true;
					}
					if(_orionProcCur.DateScheduleBy.Year>1880) {
						textDateScheduled.Text=_orionProcCur.DateScheduleBy.ToShortDateString();
					}
					if(_orionProcCur.DateStopClock.Year>1880) {
						textDateStop.Text=_orionProcCur.DateStopClock.ToShortDateString();
					}
					checkIsOnCall.Checked=_orionProcCur.IsOnCall;
					checkIsEffComm.Checked=_orionProcCur.IsEffectiveComm;
					checkIsRepair.Checked=_orionProcCur.IsRepair;
				}
				else {
					labelScheduleBy.Visible=true;
					comboDPC.SelectedIndex=0;
					comboDPCpost.SelectedIndex=0;
					textDateStop.Text="";
				}
			}
		}

		private void FormProcEdit_Shown(object sender,EventArgs e) {
			//Prompt users for auto notes if they have the preference set.
			if(PrefC.GetBool(PrefName.ProcPromptForAutoNote)) {//Replace [[text]] sections within the note with AutoNotes.
				PromptForAutoNotes();
			}
			//Scroll to the end of the note for procedures for today (or completed today).
			if(_procCur.DateEntryC.Date==DateTime.Today) {
				textNotes.Select(textNotes.Text.Length,0);
			}
			CheckForCompleteNote();
		}

		///<summary>Loops through textNotes.Text and will insert auto notes and prompt them for prompting auto notes.</summary>
		private void PromptForAutoNotes() {
			List<Match> listMatches=Regex.Matches(textNotes.Text,@"\[\[.+?\]\]").OfType<Match>().ToList();
			listMatches.RemoveAll(x => AutoNotes.GetByTitle(x.Value.TrimStart('[').TrimEnd(']'))=="");//remove matches that are not autonotes.
			int loc=0;
			foreach(Match match in listMatches) {
				string autoNoteTitle=match.Value.TrimStart('[').TrimEnd(']');
				string note=AutoNotes.GetByTitle(autoNoteTitle);
				int matchloc=textNotes.Text.IndexOf(match.Value,loc);
				FormAutoNoteCompose FormA=new FormAutoNoteCompose();
				FormA.MainTextNote=note;
				FormA.ShowDialog();
				if(FormA.DialogResult==DialogResult.Cancel) {
					loc=matchloc+match.Value.Length;
					continue;//if they cancel, go to the next autonote.
				}
				if(FormA.DialogResult==DialogResult.OK) {
					//When setting the Text on a RichTextBox, \r\n is replaced with \n, so we need to do the same so that our location variable is correct.
					loc=matchloc+FormA.CompletedNote.Replace("\r\n","\n").Length;
					string resultstr=textNotes.Text.Substring(0,matchloc)+FormA.CompletedNote;
					if(textNotes.Text.Length > matchloc+match.Value.Length) {
						resultstr+=textNotes.Text.Substring(matchloc+match.Value.Length);
					}
					textNotes.Text=resultstr;
				}
			}
			bool hasAutoNotePrompt=Regex.IsMatch(textNotes.Text,_autoNotePromptRegex);
			butEditAutoNote.Visible=hasAutoNotePrompt;
		}

		private string GetSignatureKey() {
			string keyData=_procCur.Note;
			keyData+=_procCur.UserNum.ToString();
			keyData=keyData.Replace("\r\n","\n");//We need all newlines to be the same, a mix of /r/n and /n can invalidate the procedure signature.
			return keyData;
		}

		private void SetSurfButtons(){
			butBF.BackColor=(textSurfaces.Text.Contains("B") || textSurfaces.Text.Contains("F"))?Color.White:SystemColors.Control;
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				if(textSurfaces.Text.Contains("V")) butBF.BackColor=Color.White;
			}
			butOI.BackColor=(textSurfaces.Text.Contains("O") || textSurfaces.Text.Contains("I"))?Color.White:SystemColors.Control;
			butM.BackColor=textSurfaces.Text.Contains("M")?Color.White:SystemColors.Control;
			butD.BackColor=textSurfaces.Text.Contains("D")?Color.White:SystemColors.Control;
			butL.BackColor=textSurfaces.Text.Contains("L")?Color.White:SystemColors.Control;
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				butV.BackColor=textSurfaces.Text.Contains("5")?Color.White:SystemColors.Control;
			}
			else{
				butV.BackColor=textSurfaces.Text.Contains("V")?Color.White:SystemColors.Control;
			}
		}

		///<summary>Called on open and after changing code.  Sets the visibilities and the data of all the fields in the upper left panel.</summary>
		private void SetControlsUpperLeft(){
			textDateTP.Text=_procCur.DateTP.ToString("d");
			DateTime dateT;
			if(_isStartingUp){
				textDate.Text=_procCur.ProcDate.ToString("d");
				if(_procCur.ProcDate.Date!=_procCur.DateComplete.Date && _procCur.DateComplete.Year>1880) {
					//show proc date Original if the date is different than proc date and set.
					labelOrigDateComp.Visible=true;
					textOrigDateComp.Visible=true;
					textOrigDateComp.Text=_procCur.DateComplete.ToString("d");
				}
				else {//Hide Orig Date Comp if same as current procedure date.
					labelOrigDateComp.Visible=false;
					textOrigDateComp.Visible=false;
				}
				dateT=PIn.DateT(_procCur.ProcTime.ToString());
				if(dateT.ToShortTimeString()!="12:00 AM"){
					textTimeStart.Text+=dateT.ToShortTimeString();
				}
				if(Programs.UsingOrion || PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance)) {
					dateT=PIn.DateT(_procCur.ProcTimeEnd.ToString());
					if(dateT.ToShortTimeString()!="12:00 AM") {
						textTimeEnd.Text=dateT.ToShortTimeString();
					}
					UpdateFinalMin();			
					if(_procCur.DateTP.Year<1880) {
						textDateTP.Text="";
					}
				}
			}
			textProc.Text=_procedureCode2.ProcCode;
			textDesc.Text=_procedureCode2.Descript;
			textDrugNDC.Text=_procedureCode2.DrugNDC;
			switch (_procedureCode2.TreatArea){
				case TreatmentArea.Surf:
					this.textTooth.Visible=true;
					this.labelTooth.Visible=true;
					this.textSurfaces.Visible=true;
					this.labelSurfaces.Visible=true;
					this.panelSurfaces.Visible=true;
					if(Tooth.IsValidDB(_procCur.ToothNum)) {
						errorProvider2.SetError(textTooth,"");
						textTooth.Text=Tooth.ToInternat(_procCur.ToothNum);
						textSurfaces.Text=Tooth.SurfTidyFromDbToDisplay(_procCur.Surf,_procCur.ToothNum);
						SetSurfButtons();
					}
					else{
						errorProvider2.SetError(textTooth,Lan.g(this,"Invalid tooth number."));
						textTooth.Text=_procCur.ToothNum;
						//textSurfaces.Text=Tooth.SurfTidy(ProcCur.Surf,"");//only valid toothnums allowed
					}
					if(textSurfaces.Text=="")
						errorProvider2.SetError(textSurfaces,"No surfaces selected.");
					else
						errorProvider2.SetError(textSurfaces,"");
					break;
				case TreatmentArea.Tooth:
					this.textTooth.Visible=true;
					this.labelTooth.Visible=true;
					if(Tooth.IsValidDB(_procCur.ToothNum)){
						errorProvider2.SetError(textTooth,"");
						textTooth.Text=Tooth.ToInternat(_procCur.ToothNum);
					}
					else{
						errorProvider2.SetError(textTooth,Lan.g(this,"Invalid tooth number."));
						textTooth.Text=_procCur.ToothNum;
					}
					break;
				case TreatmentArea.Mouth:
						break;
				case TreatmentArea.Quad:
					this.groupQuadrant.Visible=true;
					switch (_procCur.Surf){
						case "UR": this.radioUR.Checked=true; break;
						case "UL": this.radioUL.Checked=true; break;
						case "LR": this.radioLR.Checked=true; break;
						case "LL": this.radioLL.Checked=true; break;
						//default : 
					}
					break;
				case TreatmentArea.Sextant:
					this.groupSextant.Visible=true;
					switch (_procCur.Surf){
						case "1": this.radioS1.Checked=true; break;
						case "2": this.radioS2.Checked=true; break;
						case "3": this.radioS3.Checked=true; break;
						case "4": this.radioS4.Checked=true; break;
						case "5": this.radioS5.Checked=true; break;
						case "6": this.radioS6.Checked=true; break;
						//default:
					}
					if(IsSextantSelected()) {
						errorProvider2.SetError(groupSextant,"");
					}
					else {
						errorProvider2.SetError(groupSextant,Lan.g(this,"Please select a sextant treatment area."));
					}
					break;
				case TreatmentArea.Arch:
					this.groupArch.Visible=true;
					switch (_procCur.Surf){
						case "U": this.radioU.Checked=true; break;
						case "L": this.radioL.Checked=true; break;
					}
					if(IsArchSelected()) {
						errorProvider2.SetError(groupArch,"");
					}
					else {
						errorProvider2.SetError(groupArch,Lan.g(this,"Please select a arch treatment area."));
					}
					break;
				case TreatmentArea.ToothRange:
					this.labelRange.Visible=true;
					this.listBoxTeeth.Visible=true;
					this.listBoxTeeth2.Visible=true;
					listBoxTeeth.SelectionMode=SelectionMode.MultiExtended;
					listBoxTeeth2.SelectionMode=SelectionMode.MultiExtended;
					if(_listPatToothInitials==null) {
						_listPatToothInitials=ToothInitials.Refresh(_patCur.PatNum);
					}
					//First add teeth flagged as primary teeth for this specific patient from the Chart into _listPriTeeth.
					_listPriTeeth=ToothInitials.GetPriTeeth(_listPatToothInitials);
					//Preserve tooth range history for this procedure by ensuring that the UI shows the values from the database for the relevant teeth.
					string[] arrayProcToothNums=new string[0];
					if(!string.IsNullOrWhiteSpace(_procCur.ToothRange)){
						arrayProcToothNums=_procCur.ToothRange.Split(',');//in Universal (American) nomenclature
					}
					foreach(string procToothNum in arrayProcToothNums) {
						if(Tooth.IsPrimary(procToothNum)) {
							_listPriTeeth.Add(Tooth.ToInt(procToothNum).ToString());//Convert the primary tooth to a permanent tooth.
						}
						else {//Permanent tooth
							_listPriTeeth.Remove(procToothNum);//Preserve permanent tooth history.
						}
					}
					//Fill Maxillary/Upper Arch
					listBoxTeeth.Items.Clear();
					for(int toothNum=1;toothNum<=16;toothNum++) {
						string toothId=toothNum.ToString();
						if(_listPriTeeth.Contains(toothNum.ToString())) {//Is Primary
							toothId=Tooth.PermToPri(toothId);
						}
						listBoxTeeth.Items.Add(Tooth.GetToothLabelGraphic(toothId));//Display tooth is dependent on nomenclature preference.
					}
					//Fill Mandibular/Lower	Arch
					listBoxTeeth2.Items.Clear();
					for(int toothNum=32;toothNum>=17;toothNum--) {
						string toothId=toothNum.ToString();
						if(_listPriTeeth.Contains(toothNum.ToString())) {//Is Primary
							toothId=Tooth.PermToPri(toothId);
						}
						listBoxTeeth2.Items.Add(Tooth.GetToothLabelGraphic(toothId));//Display tooth is dependent on nomenclature preference.
					}
					//Select tooth numbers in each arch depending on the database data stored in the procedure ToothRange.
					foreach(string toothNum in arrayProcToothNums) {
						if(Tooth.IsMaxillary(toothNum)) {//Works for primary or permanent tooth numbers.
							int toothIndex=Tooth.ToInt(toothNum)-1;//Works for primary or permanent tooth numbers.
							listBoxTeeth.SetSelected(toothIndex,true);
						}
						else {//Mandibular
							int toothIndex=32-Tooth.ToInt(toothNum);//Works for primary or permanent tooth numbers.
							listBoxTeeth2.SetSelected(toothIndex,true);
						}
					} 
					break;
			}//end switch
			textProcFee.Text=_procCur.ProcFee.ToString("n");
		}

		///<summary>Enable/disable controls based on permissions ProcComplEdit and ProcComplEditLimited.</summary>
		private void SetControlsEnabled(bool isSilent) {
			//Return if the current procedure isn't considered complete (C, EC, EO).
			//Don't allow adding an estimate, since a new estimate could change the total writeoff amount for the proc.
			if(!_procCur.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)) {
				return;
			}
			//Use ProcDate to compare to the date/days newer restriction.
			Permissions perm=Permissions.ProcComplEdit;
			bool isComplete=true;
			if(_procCur.ProcStatus.In(ProcStat.EO,ProcStat.EC)) {
				perm=Permissions.ProcExistingEdit;
				isComplete=false;
			}
			bool hasDateLock=Security.IsGlobalDateLock(perm,_procCur.ProcDate,isSilent);//really only used to silence other security messages.
			bool hasProcEditLim=Security.IsAuthorized(Permissions.ProcComplEditLimited,_procCur.ProcDate,(!isComplete||isSilent));//Silence on ProcExistingEdit
			bool hasProcEditComp=Security.IsAuthorized(perm,_procCur.ProcDate,isSilent||hasDateLock||(!hasProcEditLim && isComplete)
				,hasDateLock);//Silence logic should only consider ProcComplEditLimited when not ProcExistingEdit is used.
			//When user has ProcExistingEdit, ignore procComplEditLimited because it was blocking if there was a date limitation.
			if(!hasProcEditLim && isComplete) {//always silent. All permissions messages generated at top of method.
				//user doesn't have limited or full proc complete edit permission
				List<Control> listControls=Controls.OfType<Control>().Where(x => x!=butCancel && x!=butSearch).ToList();//leave the cancel and search buttons enabled
				listControls.AddRange(tabControl.TabPages.OfType<TabPage>().SelectMany(x => x.Controls.OfType<Control>()).ToList());
				foreach(Control cCur in listControls) {
					if(cCur is TextBox || cCur is ValidDate || cCur is ValidDouble) {
						((TextBox)cCur).ReadOnly=true;
						cCur.BackColor=SystemColors.Control;
					}
					else if(cCur is ODtextBox) {
						((ODtextBox)cCur).ReadOnly=true;
						cCur.BackColor=SystemColors.Control;
					}
					else if(cCur is UI.Button || cCur is ComboBox || cCur is CheckBox || cCur is GroupBox || cCur is Panel || cCur is SignatureBoxWrapper) {
						cCur.Enabled=false;
					}
				}
			}
			if(!hasProcEditComp) {//always silent. All permissions messages generated at top of method.
				//list of controls enabled for those with ProcComplEditLimited permission
				List<Control> listControlsEnabled=new List<Control>() {
					panel1,panelSurfaces,groupArch,listBoxTeeth,listBoxTeeth2,textTooth,textSurfaces,//controls enabled within panel1
					checkHideGraphics,comboDx,groupProsth,textClaimNote,checkNoBillIns,//controls enabled below panel1 and on tabControl
					textNotes,signatureBoxWrapper,buttonUseAutoNote,butEditAutoNote,butChangeUser,butAppend,//proc note controls
					butSearch,butCancel,butOK,butEditAnyway,butAddAdjust//buttons enabled
				};
				//list of all controls on the form that will be disabled since user doesn't have ProcComplEdit (full) permission
				List<Control> listControlsDisabled=Controls.OfType<Control>().ToList();
				//add all controls on the tabControl.TabPages except for the Misc and Medical tabs
				listControlsDisabled.AddRange(tabControl.TabPages.OfType<TabPage>().Where(x => x!=tabPageMisc && x!=tabPageMedical)
					.SelectMany(x => x.Controls.OfType<Control>()));
				//add panel1's (upper left corner panel) controls
				listControlsDisabled.AddRange(panel1.Controls.OfType<Control>());
				//now remove any of the controls in the enabled list
				listControlsDisabled.RemoveAll(x => listControlsEnabled.Contains(x));
				foreach(Control cCur in listControlsDisabled) {
					if(cCur is TextBox || cCur is ValidDate || cCur is ValidDouble) {
						((TextBox)cCur).ReadOnly=true;
						cCur.BackColor=SystemColors.Control;
					}
					else if(cCur is ODtextBox) {
						((ODtextBox)cCur).ReadOnly=true;
						cCur.BackColor=SystemColors.Control;
					}
					else if(cCur is UI.Button || cCur is ComboBox || cCur is CheckBox || cCur is GroupBox || cCur is Panel || cCur is SignatureBoxWrapper) {
						cCur.Enabled=false;
					}
				}
			}
			if(!Security.IsAuthorized(perm,_procCur.ProcDate,true,true)
				&& Security.IsAuthorized(perm,_procCur.ProcDate,true,true,_procCur.CodeNum,_procCur.ProcFee,0,0)) 
			{
				//This is a $0 procedure for a proc code marked as bypassed.
				butDelete.Enabled=true;
			}
		}//end SetControls

		private void FillReferral(bool doRefreshData=true) {
			if(doRefreshData) {
				_loadData.ListRefAttaches=RefAttaches.RefreshFiltered(_procCur.PatNum,false,_procCur.ProcNum);
			}
			List<RefAttach> refsList=_loadData.ListRefAttaches;
			if(refsList.Count==0) {
				textReferral.Text="";
			}
			else {
				Referral referral;
				if(Referrals.TryGetReferral(refsList[0].ReferralNum,out referral)) {
					textReferral.Text=referral.LName+", ";
				}
				if(refsList[0].DateProcComplete.Year<1880) {
					textReferral.Text+=refsList[0].RefDate.ToShortDateString();
				}
				else{
					textReferral.Text+=Lan.g(this,"done:")+refsList[0].DateProcComplete.ToShortDateString();
				}
				if(refsList[0].RefToStatus!=ReferralToStatus.None){
					textReferral.Text+=refsList[0].RefToStatus.ToString();
				}
			}
		}

		private void butReferral_Click(object sender,EventArgs e) {
			FormReferralsPatient FormRP=new FormReferralsPatient();
			FormRP.PatNum=_procCur.PatNum;
			FormRP.ProcNum=_procCur.ProcNum;
			FormRP.ShowDialog();
			FillReferral();
		}

		private void FillIns(){
			FillIns(true);
		}

		private void FillIns(bool refreshClaimProcsFirst){
			if(refreshClaimProcsFirst) {
				//ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
				//}
				_listClaimProcsForProc=ClaimProcs.RefreshForProc(_procCur.ProcNum);
			}
			gridIns.BeginUpdate();
			gridIns.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableProcIns","Ins Plan"),190);
			gridIns.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcIns","Pri/Sec"),50,HorizontalAlignment.Center);
			gridIns.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcIns","Status"),50,HorizontalAlignment.Center);
			gridIns.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcIns","NoBill"),45,HorizontalAlignment.Center);
			gridIns.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcIns","Copay"),55,HorizontalAlignment.Right);
			gridIns.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcIns","Deduct"),55,HorizontalAlignment.Right);
			gridIns.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcIns","Percent"),55,HorizontalAlignment.Center);
			gridIns.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcIns","Ins Est"),55,HorizontalAlignment.Right);
			gridIns.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcIns","Ins Pay"),55,HorizontalAlignment.Right);
			gridIns.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcIns","WriteOff"),55,HorizontalAlignment.Right);
			gridIns.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcIns","Estimate Note"),100);
			gridIns.ListGridColumns.Add(col);		 
			col=new GridColumn(Lan.g("TableProcIns","Remarks"),165);
			gridIns.ListGridColumns.Add(col);		 
			gridIns.ListGridRows.Clear();
			GridRow row;
			checkNoBillIns.CheckState=CheckState.Unchecked;
			bool allNoBillIns=true;
			InsPlan plan;
			//ODGridCell cell;
			for(int i=0;i<_listClaimProcsForProc.Count;i++) {
				if(_listClaimProcsForProc[i].NoBillIns){
					checkNoBillIns.CheckState=CheckState.Indeterminate;
				}
				else{
					allNoBillIns=false;
				}
				row=new GridRow();
				row.Cells.Add(InsPlans.GetDescript(_listClaimProcsForProc[i].PlanNum,_famCur,_listInsPlans,_listClaimProcsForProc[i].InsSubNum,_listInsSubs));
				plan=InsPlans.GetPlan(_listClaimProcsForProc[i].PlanNum,_listInsPlans);
				if(plan==null) {
					MsgBox.Show(this,"No insurance plan exists for this claim proc.  Please run database maintenance.");
					return;
				}
				if(plan.IsMedical) {
					row.Cells.Add("Med");
				}
				else if(_listClaimProcsForProc[i].InsSubNum==PatPlans.GetInsSubNum(_listPatPlans,PatPlans.GetOrdinal(PriSecMed.Primary,_listPatPlans,_listInsPlans,_listInsSubs))){
					row.Cells.Add("Pri");
				}
				else if(_listClaimProcsForProc[i].InsSubNum==PatPlans.GetInsSubNum(_listPatPlans,PatPlans.GetOrdinal(PriSecMed.Secondary,_listPatPlans,_listInsPlans,_listInsSubs))) {
					row.Cells.Add("Sec");
				}
				else {
					row.Cells.Add("");
				}
				switch(_listClaimProcsForProc[i].Status) {
					case ClaimProcStatus.Received:
						row.Cells.Add("Recd");
						break;
					case ClaimProcStatus.NotReceived:
						row.Cells.Add("NotRec");
						break;
					//adjustment would never show here
					case ClaimProcStatus.Preauth:
						row.Cells.Add("PreA");
						break;
					case ClaimProcStatus.Supplemental:
						row.Cells.Add("Supp");
						break;
					case ClaimProcStatus.CapClaim:
						row.Cells.Add("CapClaim");
						break;
					case ClaimProcStatus.Estimate:
						row.Cells.Add("Est");
						break;
					case ClaimProcStatus.CapEstimate:
						row.Cells.Add("CapEst");
						break;
					case ClaimProcStatus.CapComplete:
						row.Cells.Add("CapComp");
						break;
					case ClaimProcStatus.InsHist:
						row.Cells.Add("InsHist");
						break;
					default:
						row.Cells.Add("");
						break;
				}
				if(_listClaimProcsForProc[i].NoBillIns) {
					row.Cells.Add("X");
					if(!_listClaimProcsForProc[i].Status.In(ClaimProcStatus.CapComplete,ClaimProcStatus.CapEstimate)) {
						row.Cells.Add("");
						row.Cells.Add("");
						row.Cells.Add("");
						row.Cells.Add("");
						row.Cells.Add("");
						row.Cells.Add("");
						row.Cells.Add("");
						row.Cells.Add("");
						row.Cells.Add("");
						gridIns.ListGridRows.Add(row);
						continue;
					}
				}
				else {
					row.Cells.Add("");
				}
				row.Cells.Add(ClaimProcs.GetCopayDisplay(_listClaimProcsForProc[i]));
				double ded=ClaimProcs.GetDeductibleDisplay(_listClaimProcsForProc[i]);
				if(ded>0) {
					row.Cells.Add(ded.ToString("n"));
				}
				else {
					row.Cells.Add("");
				}
				row.Cells.Add(ClaimProcs.GetPercentageDisplay(_listClaimProcsForProc[i]));
				row.Cells.Add(ClaimProcs.GetEstimateDisplay(_listClaimProcsForProc[i]));
				if(_listClaimProcsForProc[i].Status==ClaimProcStatus.Estimate
					|| _listClaimProcsForProc[i].Status==ClaimProcStatus.CapEstimate) 
				{
					row.Cells.Add("");
					row.Cells.Add(ClaimProcs.GetWriteOffEstimateDisplay(_listClaimProcsForProc[i]));
				}
				else {
					row.Cells.Add(_listClaimProcsForProc[i].InsPayAmt.ToString("n"));
					row.Cells.Add(_listClaimProcsForProc[i].WriteOff.ToString("n"));
				}
				row.Cells.Add(_listClaimProcsForProc[i].EstimateNote);
				row.Cells.Add(_listClaimProcsForProc[i].Remarks);			  
				gridIns.ListGridRows.Add(row);
			}
			gridIns.EndUpdate();
			if(_listClaimProcsForProc.Count==0) {
				checkNoBillIns.CheckState=CheckState.Unchecked;
			}
			else if(allNoBillIns) {
				checkNoBillIns.CheckState=CheckState.Checked;
			}
		}

		private void gridIns_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormClaimProc FormC=new FormClaimProc(_listClaimProcsForProc[e.Row],_procCur,_famCur,_patCur,_listInsPlans,HistList,ref LoopList,_listPatPlans,true,_listInsSubs);
			if(_procCur.IsLocked || !Procedures.IsProcComplEditAuthorized(_procCur)) {
				FormC.NoPermissionProc=true;
			}
			FormC.ShowDialog();
			FillIns();
		}

		void butNow_Click(object sender,EventArgs e) {
			if(textTimeStart.Text.Trim()=="") {
				textTimeStart.Text=MiscData.GetNowDateTime().ToShortTimeString();
			}
			else {
				textTimeEnd.Text=MiscData.GetNowDateTime().ToShortTimeString();
			}
		}

		private void butAddEstimate_Click(object sender, System.EventArgs e) {
			if(_procCur.ProcNumLab!=0) {
				MsgBox.Show(this,"Estimates cannot be added directly to labs.  Lab estimates will be created automatically when the parent procedure estimates are calculated.");
				return;
			}
			if(!Security.IsAuthorized(Permissions.InsWriteOffEdit,_procCur.DateEntryC)) {
				return;
			}
			FormInsPlanSelect FormIS=new FormInsPlanSelect(_patCur.PatNum);
			if(FormIS.SelectedPlan==null) {
				FormIS.ShowDialog();
				if(FormIS.DialogResult==DialogResult.Cancel){
					return;
				}
			}
			InsPlan plan=FormIS.SelectedPlan;
			InsSub sub=FormIS.SelectedSub;
			_listClaimProcsForProc=ClaimProcs.RefreshForProc(_procCur.ProcNum);
			ClaimProc claimProcForProcInsPlan=_listClaimProcsForProc
				.Where(x => x.PlanNum == plan.PlanNum)
				.Where(x => x.Status != ClaimProcStatus.Preauth)
				.FirstOrDefault();
			ClaimProc cp = new ClaimProc();
			cp.IsNew=true;
			if(claimProcForProcInsPlan!=null) {
				cp = claimProcForProcInsPlan;
				cp.IsNew=false;
			}
			else {
				List<Benefit> benList = Benefits.Refresh(_listPatPlans,_listInsSubs);
				ClaimProcs.CreateEst(cp,_procCur,plan,sub);
				if(plan.PlanType=="c") {//capitation
					double allowed = PIn.Double(textProcFee.Text);
					cp.BaseEst=allowed;
					cp.InsEstTotal=allowed;
					cp.CopayAmt=InsPlans.GetCopay(_procCur.CodeNum,plan.FeeSched,plan.CopayFeeSched,
						!SubstitutionLinks.HasSubstCodeForPlan(plan,_procCur.CodeNum,ListSubstLinks),_procCur.ToothNum,_procCur.ClinicNum,_procCur.ProvNum,plan.PlanNum,
						ListSubstLinks,LookupFees);
					if(cp.CopayAmt > allowed) {//if the copay is greater than the allowed fee calculated above
						cp.CopayAmt=allowed;//reduce the copay
					}
					if(cp.CopayAmt==-1) {
						cp.CopayAmt=0;
					}
					cp.WriteOffEst=cp.BaseEst-cp.CopayAmt;
					if(cp.WriteOffEst<0) {
						cp.WriteOffEst=0;
					}
					cp.WriteOff=cp.WriteOffEst;
					ClaimProcs.Update(cp);
				}
				long patPlanNum = PatPlans.GetPatPlanNum(sub.InsSubNum,_listPatPlans);
				if(patPlanNum > 0) {
					double paidOtherInsTotal = ClaimProcs.GetPaidOtherInsTotal(cp,_listPatPlans);
					double writeOffOtherIns = ClaimProcs.GetWriteOffOtherIns(cp,_listPatPlans);
					ClaimProcs.ComputeBaseEst(cp,_procCur,plan,patPlanNum,benList,
						HistList,LoopList,_listPatPlans,paidOtherInsTotal,paidOtherInsTotal,_patCur.Age,writeOffOtherIns,_listInsPlans,_listInsSubs,ListSubstLinks,false,LookupFees);
				}
			}
			FormClaimProc FormC=new FormClaimProc(cp,_procCur,_famCur,_patCur,_listInsPlans,HistList,ref LoopList,_listPatPlans,true,_listInsSubs);
			//FormC.NoPermission not needed because butAddEstimate not enabled
			FormC.ShowDialog();
			if(FormC.DialogResult==DialogResult.Cancel && cp.IsNew){
				ClaimProcs.Delete(cp);
			}
			FillIns();
		}

		private void FillPayments(bool doRefreshData=true){
			if(doRefreshData) {
				_loadData.ArrPaySplits=PaySplits.Refresh(_procCur.PatNum);
				List<long> listPayNums=_loadData.ArrPaySplits.Where(x => x.ProcNum==_procCur.ProcNum).Select(x => x.PayNum).ToList();
				_loadData.ListPaymentsForProc=Payments.GetPayments(listPayNums);
			}
			_listPaymentsForProc=_loadData.ListPaymentsForProc;
			_paySplitsForProc=PaySplits.GetForProc(_procCur.ProcNum,_loadData.ArrPaySplits);
			gridPay.BeginUpdate();
			gridPay.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableProcPay","Entry Date"),70,HorizontalAlignment.Center);
			gridPay.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcPay","Amount"),55,HorizontalAlignment.Right);
			gridPay.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcPay","Tot Amt"),55,HorizontalAlignment.Right);
			gridPay.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcPay","Note"),250,HorizontalAlignment.Left);
			gridPay.ListGridColumns.Add(col);
			gridPay.ListGridRows.Clear();
			GridRow row;
			Payment PaymentCur;//used in loop
			for(int i=0;i<_paySplitsForProc.Count;i++){
				row=new GridRow();
				row.Cells.Add(((PaySplit)_paySplitsForProc[i]).DatePay.ToShortDateString());
				row.Cells.Add(((PaySplit)_paySplitsForProc[i]).SplitAmt.ToString("F"));
				row.Cells[row.Cells.Count-1].Bold=YN.Yes;
				PaymentCur=Payments.GetFromList(((PaySplit)_paySplitsForProc[i]).PayNum,_listPaymentsForProc);
				row.Cells.Add(PaymentCur.PayAmt.ToString("F"));
				row.Cells.Add(PaymentCur.PayNote);
				gridPay.ListGridRows.Add(row);
			}
			gridPay.EndUpdate();
		}

		private void gridPay_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Payment PaymentCur=Payments.GetFromList(((PaySplit)_paySplitsForProc[e.Row]).PayNum,_listPaymentsForProc);
			FormPayment FormP=new FormPayment(_patCur,_famCur,PaymentCur,false);
			FormP.InitialPaySplitNum=((PaySplit)_paySplitsForProc[e.Row]).SplitNum;
			FormP.ShowDialog();
			FillPayments();
		}

		private void FillAdj(){
			Adjustment[] AdjustmentList=_loadData.ArrAdjustments;
			_adjustmentsForProc=Adjustments.GetForProc(_procCur.ProcNum,AdjustmentList);
			gridAdj.BeginUpdate();
			gridAdj.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableProcAdj","Entry Date"),70,HorizontalAlignment.Center);
			gridAdj.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcAdj","Amount"),55,HorizontalAlignment.Right);
			gridAdj.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcAdj","Type"),100,HorizontalAlignment.Left);
			gridAdj.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableProcAdj","Note"),250,HorizontalAlignment.Left);
			gridAdj.ListGridColumns.Add(col);
			gridAdj.ListGridRows.Clear();
			GridRow row;
			double discountAmt=0;//Total discount amount from all adjustments of default type.
			for(int i=0;i<_adjustmentsForProc.Count;i++){
				row=new GridRow();
				Adjustment adjustment=(Adjustment)_adjustmentsForProc[i];
				row.Cells.Add(adjustment.AdjDate.ToShortDateString());
				row.Cells.Add(adjustment.AdjAmt.ToString("F"));
				row.Cells[row.Cells.Count-1].Bold=YN.Yes;
				row.Cells.Add(Defs.GetName(DefCat.AdjTypes,adjustment.AdjType));
				row.Cells.Add(adjustment.AdjNote);
				gridAdj.ListGridRows.Add(row);
				if(adjustment.AdjType==PrefC.GetLong(PrefName.TreatPlanDiscountAdjustmentType)) {
					discountAmt-=adjustment.AdjAmt;//Discounts are stored as negatives, we want a positive discount value.
				}
			}
			gridAdj.EndUpdate();
			//Because we keep the discount field in sync with the discount adjustment when the procedure has a status of TP,
			//we considered it a bug that the opposite didn't happen once the procedure was set complete.
			if(_procCur.ProcStatus==ProcStat.C) {
				//Updating the discount text box will cause the procedure to get updated if the user clicks OK.
				//This is fine because the Discount column is not designed for accuracy (after being set complete) and is loosely kept updated.
				textDiscount.Text=discountAmt.ToString("F");//Calculated based on all adjustments of type if complete
			}
		}

		private void gridAdj_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormAdjust FormA=new FormAdjust(_patCur,(Adjustment)_adjustmentsForProc[e.Row]);
			if(FormA.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_loadData.ArrAdjustments=Adjustments.GetForProcs(new List<long>() { _procCur.ProcNum }).ToArray();
			FillAdj();
		}

		private void butAddAdjust_Click(object sender, System.EventArgs e) {
			if(_procCur.ProcStatus!=ProcStat.C){
				MsgBox.Show(this,"Adjustments may only be added to completed procedures.");
				return;
			}
			bool isTsiAdj=(TsiTransLogs.IsTransworldEnabled(_patCur.ClinicNum)
				&& Patients.IsGuarCollections(_patCur.Guarantor)
				&& MsgBox.Show(this,MsgBoxButtons.YesNo,"The guarantor of this family has been sent to Transworld for collection.\r\n"
					+"Is this an adjustment due to a payment received from Transworld?"));
			Adjustment adj=new Adjustment();
			adj.PatNum=_patCur.PatNum;
			adj.ProvNum=comboProv.GetSelectedProvNum();
			adj.DateEntry=DateTime.Today;//but will get overwritten to server date
			adj.AdjDate=DateTime.Today;
			adj.ProcDate=_procCur.ProcDate;
			adj.ProcNum=_procCur.ProcNum;
			adj.ClinicNum=_procCur.ClinicNum;
			FormAdjust FormA=new FormAdjust(_patCur,adj,isTsiAdj);
			FormA.IsNew=true;
			if(FormA.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_loadData.ArrAdjustments=Adjustments.GetForProcs(new List<long>() { _procCur.ProcNum }).ToArray();
			FillAdj();
		}

		private void butAddExistAdj_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AdjustmentEdit)) {
				return;
			}
			if(_procCur.ProcStatus!=ProcStat.C){
				MsgBox.Show(this,"Adjustments may only be added to completed procedures.");
				return;
			}
			FormAdjustmentPicker FormAP=new FormAdjustmentPicker(_patCur.PatNum,true);
			if(FormAP.ShowDialog()!=DialogResult.OK) {
				return;
			}
			if(!Security.IsAuthorized(Permissions.AdjustmentEdit,FormAP.SelectedAdjustment.AdjDate)) {
				return;
			}
			if(AvaTax.IsEnabled() && 
				(FormAP.SelectedAdjustment.AdjType==AvaTax.SalesTaxAdjType || FormAP.SelectedAdjustment.AdjType==AvaTax.SalesTaxReturnAdjType) && 
					!Security.IsAuthorized(Permissions.SalesTaxAdjEdit)) 
			{
				return;
			}
			List<PayPlanLink> listPayPlanLinks=PayPlanLinks.GetForFKeyAndLinkType(FormAP.SelectedAdjustment.AdjNum,PayPlanLinkType.Adjustment);
			if(listPayPlanLinks.Count>0) {
				MsgBox.Show(this,"Cannot attach adjustment which is associated to a payment plan.");
				return;
			}
			decimal estPatPort=ClaimProcs.GetPatPortion(_procCur,_loadData.ListClaimProcsForProc,_loadData.ArrAdjustments.ToList());
			decimal procPatPaid=(decimal)PaySplits.GetTotForProc(_procCur);
			decimal adjRemAmt=estPatPort-procPatPaid+(decimal)FormAP.SelectedAdjustment.AdjAmt;
			if(adjRemAmt<0 && !MsgBox.Show(this,MsgBoxButtons.OKCancel,"Remaining amount is negative.  Continue?","Overpaid Procedure Warning")) {
				return;
			}
			FormAP.SelectedAdjustment.ProcNum=_procCur.ProcNum;
			FormAP.SelectedAdjustment.ProcDate=_procCur.ProcDate;
			Adjustments.Update(FormAP.SelectedAdjustment);
			_loadData.ArrAdjustments=Adjustments.GetForProcs(new List<long>() { _procCur.ProcNum }).ToArray();
			FillAdj();
		}

		private void textProcFee_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			if(textProcFee.errorProvider1.GetError(textProcFee)!=""){
				return;
			}
			double procFee;
			if(textProcFee.Text==""){
				procFee=0;
			}
			else{
				procFee=PIn.Double(textProcFee.Text);
			}
			if(_procCur.ProcFee==procFee){
				return;
			}
			_procCur.ProcFee=procFee;
			_isEstimateRecompute=true;
			Procedures.ComputeEstimates(_procCur,_patCur.PatNum,ref _listClaimProcsForProc,false,_listInsPlans,_listPatPlans,_listBenefits,
				null,null,true,
				_patCur.Age,_listInsSubs,
				null,false,false,ListSubstLinks,false,
				null,LookupFees);
			FillIns();
		}

		private void textTooth_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			textTooth.Text=textTooth.Text.ToUpper();
			if(!Tooth.IsValidEntry(textTooth.Text))
				errorProvider2.SetError(textTooth,Lan.g(this,"Invalid tooth number."));
			else
				errorProvider2.SetError(textTooth,"");
		}

		private void textSurfaces_TextChanged(object sender, System.EventArgs e) {
			int cursorPos = textSurfaces.SelectionStart;
			textSurfaces.Text=textSurfaces.Text.ToUpper();
			textSurfaces.SelectionStart=cursorPos;
		}

		private void textSurfaces_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			if(Tooth.IsValidEntry(textTooth.Text)){
				textSurfaces.Text=Tooth.SurfTidyForDisplay(textSurfaces.Text,Tooth.FromInternat(textTooth.Text));
			}
			else{
				textSurfaces.Text=Tooth.SurfTidyForDisplay(textSurfaces.Text,"");
			}
			if(textSurfaces.Text=="")
				errorProvider2.SetError(textSurfaces,"No surfaces selected.");
			else
				errorProvider2.SetError(textSurfaces,"");
			SetSurfButtons();
		}

		private void groupSextant_Validating(object sender,CancelEventArgs e) {
			if(IsSextantSelected()) {
				errorProvider2.SetError(groupSextant,"");
			}
			else {
				errorProvider2.SetError(groupSextant,Lan.g(this,"Please select a sextant treatment area."));
			}
		}

		private bool IsSextantSelected() {
			return groupSextant.Controls.OfType<RadioButton>().Any(x => x.Checked);
		}

		private void groupArch_Validating(object sender,CancelEventArgs e) {
			if(IsArchSelected()) {
				errorProvider2.SetError(groupArch,"");
			}
			else {
				errorProvider2.SetError(groupArch,Lan.g(this,"Please select a arch treatment area."));
			}
		}

		private bool IsArchSelected() {
			return groupArch.Controls.OfType<RadioButton>().Any(x => x.Checked);
		}

		///<summary>Deletes any ClaimProcs in the list that do not have a claim payment.</summary>
		private void ClearClaimProcs(List<ClaimProc> listClaimProcs) {
			for(int i=listClaimProcs.Count-1;i>=0;i--) {
				if(listClaimProcs[i].ClaimPaymentNum!=0) {
					continue;
				}
				ClaimProcs.Delete(listClaimProcs[i]);//that way, completely new ones will be added back, and NoBillIns will be accurate.
				listClaimProcs.RemoveAt(i);
			}
		}

		private void butChange_Click(object sender, System.EventArgs e) {
			FormProcCodes FormP=new FormProcCodes();
      FormP.IsSelectionMode=true;
      FormP.ShowDialog();
      if(FormP.DialogResult!=DialogResult.OK){
				return;
			}
			_listFees=null;
			_lookupFees=null;//will trigger another lazy load, later, with this new code.
			Procedure procOld=_procCur.Copy();
			ProcedureCode procCodeOld=ProcedureCodes.GetProcCode(_procCur.CodeNum);
			ProcedureCode procCodeNew=ProcedureCodes.GetProcCode(FormP.SelectedCodeNum);
			if(procCodeOld.TreatArea != procCodeNew.TreatArea) {
				MsgBox.Show(this,"Not allowed due to treatment area mismatch.");
				return;
			}
      _procCur.CodeNum=FormP.SelectedCodeNum;
			_procCur.ProcFee=Procedures.GetProcFee(_patCur,_listPatPlans,_listInsSubs,_listInsPlans,_procCur.CodeNum,_procCur.ProvNum,_procCur.ClinicNum
				,_procCur.MedicalCode,listFees:ListFees);
			switch(procCodeNew.TreatArea){ 
				case TreatmentArea.Quad:
					_procCur.Surf="UR";
					break;
				case TreatmentArea.Sextant:
					_procCur.Surf="1";
					break;
				case TreatmentArea.Arch:
					_procCur.Surf="U";
					break;
			}
			ClearClaimProcs(_listClaimProcsForProc);
			_listClaimProcsForProc=new List<ClaimProc>();
			Procedures.ComputeEstimates(_procCur,_patCur.PatNum,ref _listClaimProcsForProc,true,_listInsPlans,_listPatPlans,_listBenefits,
				null,null,true,
				_patCur.Age,_listInsSubs,
				null,false,false,ListSubstLinks,false,
				null,LookupFees);
			#region New Procedure Code Overallocated
			if(_paySplitsForProc.Count>0 || _adjustmentsForProc.Count>0) {
				//Need to refresh from the database because this Procedures.ComputeEstimates() may have lost the reference to a new list.
				_listClaimProcsForProc=ClaimProcs.RefreshForProc(_procCur.ProcNum);
				double chargeOld=procOld.ProcFeeTotal;
				double chargeNew=_procCur.ProcFeeTotal;
				double sumWO=ClaimProcs.ProcWriteoff(_listClaimProcsForProc,_procCur.ProcNum);
				double sumInsPaids=ClaimProcs.ProcInsPay(_listClaimProcsForProc,_procCur.ProcNum);
				double sumInsEsts=ClaimProcs.ProcEstNotReceived(_listClaimProcsForProc,_procCur.ProcNum);
				//Adjustments are already negative if a discount, etc.
				double sumAdjs=_adjustmentsForProc.Cast<Adjustment>().ToList().FindAll(x => x.ProcNum==_procCur.ProcNum).Sum(x => x.AdjAmt);
				double sumPaySplits=_paySplitsForProc.Cast<PaySplit>().ToList().FindAll(x => x.ProcNum==_procCur.ProcNum).Sum(x => x.SplitAmt);
				double credits=sumWO+sumInsPaids+sumInsEsts-sumAdjs+sumPaySplits;
				//Check if the new ProcCode will result in the procedure being overallocated due to a change in ProcFee.
				if(credits>chargeNew) {//Procedure is overallocated.
					string strMsg=Lan.g(this,"The fee will be changed from")+" "+chargeOld.ToString("c")+" "
						+Lan.g(this,"to")+" "+chargeNew.ToString("c")
						+Lan.g(this,", and the procedure has credits attached in the amount of")+" "+credits.ToString("c")
						+Lan.g(this,".  This will result in an overallocated procedure")+".\r\n"+Lan.g(this,"Continue?");
					//Prompt user to accept the overallocation or revert back to old ProcCode.
					if(MessageBox.Show(this,strMsg,Lan.g(this,"Overpaid Procedure Warning"),MessageBoxButtons.YesNo)==DialogResult.No) {
						_procCur=procOld;
						ClearClaimProcs(_listClaimProcsForProc);
						Procedures.ComputeEstimates(_procCur,_patCur.PatNum,ref _listClaimProcsForProc,false,_listInsPlans,_listPatPlans,_listBenefits,
							null,null,true,
							_patCur.Age,_listInsSubs,
							null,false,false,ListSubstLinks,false,
							null,LookupFees);
						_listClaimProcsForProc=ClaimProcs.RefreshForProc(_procCur.ProcNum);
					}
				}
			}
			#endregion
			_procedureCode2=ProcedureCodes.GetProcCode(_procCur.CodeNum);
			textDesc.Text=_procedureCode2.Descript;
			//Update the UI now that we know the procedure code change is fine
			switch(_procedureCode2.TreatArea){ 
				case TreatmentArea.Quad:
					radioUR.Checked=true;
					break;
				case TreatmentArea.Sextant:
					radioS1.Checked=true;
					break;
				case TreatmentArea.Arch:
					radioU.Checked=true;
					break;
			}
			FillIns();
      SetControlsUpperLeft();
		}

		private void butEditAnyway_Click(object sender, System.EventArgs e) {
			DateTime dateOldestClaim=Procedures.GetOldestClaimDate(_listClaimProcsForProc);
			if(!Security.IsAuthorized(Permissions.ClaimSentEdit,dateOldestClaim)) {
				return;
			}
			if(_orthoProcLink!=null) {
				MsgBox.Show(this,"Not allowed to edit specific procedure fields for procedures linked to an ortho case.");
				return;
			}
			panel1.Enabled=true;
			comboProcStatus.Enabled=true;
			checkNoBillIns.Enabled=true;
			butDelete.Enabled=true;
			butSetComplete.Enabled=true;
			SetControlsEnabled(true);//enables/disables controls based on whether or not the user has permission (limited and/or full) to edit completed procs
			//Disable controls that may have been overzealously enabled.
			textTooth.BackColor=SystemColors.Control;
			textTooth.ReadOnly=true;
			textSurfaces.BackColor=SystemColors.Control;
			textSurfaces.ReadOnly=true;
			butAddEstimate.Enabled=true;
			radioL.Enabled=false;
			radioU.Enabled=false;
			radioLL.Enabled=false;
			radioLR.Enabled=false;
			radioUL.Enabled=false;
			radioUR.Enabled=false;
			radioS1.Enabled=false;
			radioS2.Enabled=false;
			radioS3.Enabled=false;
			radioS4.Enabled=false;
			radioS5.Enabled=false;
			radioS6.Enabled=false;
			listBoxTeeth.Enabled=false;
			listBoxTeeth2.Enabled=false;
			panelSurfaces.Enabled=false;
			//butChange.Enabled=true;//No. We no longer allow this because part of "change" is to delete all the claimprocs.  This is a terrible idea for a completed proc attached to a claim.
			//checkIsCovIns.Enabled=true;
		}

		private void comboProcStatus_SelectionChangeCommitted(object sender,EventArgs e) {
			//status cannot be changed for completed procedures attached to a claim, except we allow changing status for preauths.
			//cannot edit status for TPi procedures.
			if(ProcedureL.IsProcCompleteAttachedToClaim(_procOld,_listClaimProcsForProc)) {
				comboProcStatus.SetSelectedEnum(ProcStat.C);//Complete
				return;
			}
			if(comboProcStatus.GetSelected<ProcStat>()==ProcStat.TP) {//fee starts out 0 if EO, EC, etc.  This updates fee if changing to TP so it won't stay 0.
				_procCur.ProcStatus=ProcStat.TP;
				if(_procCur.ProcFee==0) {
					_procCur.ProcFee=Procedures.GetProcFee(_patCur,_listPatPlans,_listInsSubs,_listInsPlans,_procCur.CodeNum,_procCur.ProvNum,_procCur.ClinicNum,
						_procCur.MedicalCode,listFees:ListFees);
					textProcFee.Text=_procCur.ProcFee.ToString("f");
				}
			}
			if(comboProcStatus.GetSelected<ProcStat>()==ProcStat.C) {
				bool isAllowedToCompl=true;
				if(!PrefC.GetBool(PrefName.AllowSettingProcsComplete)) {
					MsgBox.Show(this,"Set the procedure complete by setting the appointment complete.  "
						+"If you want to be able to set procedures complete, you must turn on that option in Setup | Chart | Chart Preferences.");
					isAllowedToCompl=false;
				}
				//else if so that we don't give multiple notifications to the user.
				else if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text),_procCur.CodeNum,PIn.Double(textProcFee.Text))) {
					isAllowedToCompl=false;
				}
				//Check to see if the user is allowed to set the procedure complete.
				if(!isAllowedToCompl) {
					//User not allowed to complete procedures so set it back to whatever it was before
					if(_procCur.ProcStatus==ProcStat.TP) {
						comboProcStatus.SetSelectedEnum(ProcStat.TP);
					}
					else if(PrefC.GetBool(PrefName.EasyHideClinical)) {
						comboProcStatus.SelectedIndex=-1;//original status must not be visible
					}
					else {
						if(_procCur.ProcStatus.In(ProcStat.EC,ProcStat.EO,ProcStat.R,ProcStat.Cn))
						{
							comboProcStatus.SetSelectedEnum(_procCur.ProcStatus);
						}
					}
					return;
				}
				if(_procCur.AptNum!=0) {//if attached to an appointment
					Appointment apt=Appointments.GetOneApt(_procCur.AptNum);
					if(apt.AptDateTime.Date > MiscData.GetNowDateTime().Date) {//if appointment is in the future
						MessageBox.Show(Lan.g(this,"Not allowed because procedure is attached to a future appointment with a date of ")
							+apt.AptDateTime.ToShortDateString());
						return;
					}
					if(apt.AptDateTime.Year<1880) {
						textDate.Text=MiscData.GetNowDateTime().ToShortDateString();
					}
					else {
						textDate.Text=apt.AptDateTime.ToShortDateString();
					}
				}
				else {
					textDate.Text=MiscData.GetNowDateTime().ToShortDateString();
				}
				//broken appointment procedure codes shouldn't trigger DateFirstVisit update.
				if(ProcedureCodes.GetStringProcCode(_procCur.CodeNum)!="D9986" && ProcedureCodes.GetStringProcCode(_procCur.CodeNum)!="D9987") {
					Procedures.SetDateFirstVisit(DateTimeOD.Today,2,_patCur);
				}
				_procCur.ProcStatus=ProcStat.C;
				//Setting a procedure to complete from the dropdown menu should only change the procedure status, not the procedure dates or copy default 
				//procedure notes, per the manual (https://opendental.com/manual/procedureedit.html). It is desireable to not add the note from the combobox
				//as this allows a way for the user to change Complete to TP (to update fees) and back without adding duplicate notes.
			}
			ProcStat selecteStat=comboProcStatus.GetSelected<ProcStat>();
			if(selecteStat.In(ProcStat.EC,ProcStat.EO,ProcStat.R,ProcStat.Cn)) {
				_procCur.ProcStatus=selecteStat;
			}
			//If it's already locked, there's simply no way to save the changes made to this control.
			//If status was just changed to C, then we should show the lock button.
			if(_procCur.ProcStatus==ProcStat.C) {
				if(PrefC.GetBool(PrefName.ProcLockingIsAllowed) && !_procCur.IsLocked) {
					butLock.Visible=true;
				}
			}
		}

		private void butSetComplete_Click(object sender, System.EventArgs e) {
			//can't get to here if attached to a claim, even if use the Edit Anyway button.
			if(_procOld.ProcStatus==ProcStat.C){
				MsgBox.Show(this,"Procedure was already set complete.");
				return;
			}
			if(!PrefC.GetBool(PrefName.AllowSettingProcsComplete)) {
				MsgBox.Show(this,"Set the procedure complete by setting the appointment complete.  "
					+"If you want to be able to set procedures complete, you must turn on that option in Setup | Chart | Chart Preferences.");
				return;
			}
			if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text),_procCur.CodeNum,PIn.Double(textProcFee.Text))) {
				return;
			}
			//If user is trying to change status to complete and using eCW.
			if((IsNew || _procOld.ProcStatus!=ProcStat.C) && Programs.UsingEcwTightOrFullMode()) {
				MsgBox.Show(this,"Procedures cannot be set complete in this window.  Set the procedure complete by setting the appointment complete.");
				return;
			}
			//broken appointment procedure codes shouldn't trigger DateFirstVisit update.
			if(ProcedureCodes.GetStringProcCode(_procCur.CodeNum)!="D9986" && ProcedureCodes.GetStringProcCode(_procCur.CodeNum)!="D9987") {
				Procedures.SetDateFirstVisit(DateTimeOD.Today,2,_patCur);
			}
			if(_procCur.AptNum!=0){//if attached to an appointment
				Appointment apt=Appointments.GetOneApt(_procCur.AptNum);
				if(apt.AptDateTime.Date > MiscData.GetNowDateTime().Date){//if appointment is in the future
					MessageBox.Show(Lan.g(this,"Not allowed because procedure is attached to a future appointment with a date of ")
						+apt.AptDateTime.ToShortDateString());
					return;
				}
				if(apt.AptDateTime.Year<1880) {
					textDate.Text=MiscData.GetNowDateTime().ToShortDateString();
				}
				else {
					textDate.Text=apt.AptDateTime.ToShortDateString();
				}
			}
			else{
				textDate.Text=MiscData.GetNowDateTime().ToShortDateString();
			}
			if(_procedureCode2.PaintType==ToothPaintingType.Extraction){//if an extraction, then mark previous procs hidden
				//Procedures.SetHideGraphical(ProcCur);//might not matter anymore
				ToothInitials.SetValue(_procCur.PatNum,_procCur.ToothNum,ToothInitialType.Missing);
			}
			ProcNoteUiHelper();
			Plugins.HookAddCode(this,"FormProcEdit.butSetComplete_Click_end",_procCur,_procOld,textNotes);
			comboProcStatus.SelectedIndex=-1;
			_procCur.ProcStatus=ProcStat.C;
			_procCur.SiteNum=_patCur.SiteNum;
			comboPlaceService.SelectedIndex=PrefC.GetInt(PrefName.DefaultProcedurePlaceService);
			if(EntriesAreValid()){
				SaveAndClose();
			}
		}

		///<summary>Sets the UI textNotes.Text to the default proc note if any.
		///Also checks PrefName.ProcPromptForAutoNote and remots auto notes if needed.</summary>
		private void ProcNoteUiHelper() {
			string procNoteDefault="";
			if(_isQuickAdd) {//Quick Procs should insert both TP Default Note and C Default Note.
				procNoteDefault=ProcCodeNotes.GetNote(comboProv.GetSelectedProvNum(),_procCur.CodeNum,ProcStat.TP);
				if(!string.IsNullOrEmpty(procNoteDefault)) {
					procNoteDefault+="\r\n";
				}
			}
			procNoteDefault+=ProcCodeNotes.GetNote(comboProv.GetSelectedProvNum(),_procCur.CodeNum,ProcStat.C);
			if(textNotes.Text!="" && procNoteDefault!="") { //check to see if a default note is defined.
				textNotes.Text+="\r\n"; //add a new line if there was already a ProcNote on the procedure.
			}
			if(!string.IsNullOrEmpty(procNoteDefault)) {
				textNotes.Text+=procNoteDefault;
			}
			if(!PrefC.GetBool(PrefName.ProcPromptForAutoNote)) {
				//Users do not want to be prompted for auto notes, so remove them all from the procedure note.
				textNotes.Text=Regex.Replace(textNotes.Text,@"\[\[.+?\]\]","");
			}
		}

		private void radioUR_Click(object sender, System.EventArgs e) {
			_procCur.Surf="UR";
		}

		private void radioUL_Click(object sender, System.EventArgs e) {
			_procCur.Surf="UL";
		}

		private void radioLR_Click(object sender, System.EventArgs e) {
			_procCur.Surf="LR";
		}

		private void radioLL_Click(object sender, System.EventArgs e) {
			_procCur.Surf="LL";
		}

		private void radioU_Click(object sender, System.EventArgs e) {
			_procCur.Surf="U";
			errorProvider2.SetError(groupArch,"");
		}

		private void radioL_Click(object sender, System.EventArgs e) {
			_procCur.Surf="L";
			errorProvider2.SetError(groupArch,"");
		}

		private void radioS1_Click(object sender, System.EventArgs e) {
			_procCur.Surf="1";
			errorProvider2.SetError(groupSextant,"");
		}

		private void radioS2_Click(object sender, System.EventArgs e) {
			_procCur.Surf="2";
			errorProvider2.SetError(groupSextant,"");
		}

		private void radioS3_Click(object sender, System.EventArgs e) {
			_procCur.Surf="3";
			errorProvider2.SetError(groupSextant,"");
		}

		private void radioS4_Click(object sender, System.EventArgs e) {
			_procCur.Surf="4";
			errorProvider2.SetError(groupSextant,"");
		}

		private void radioS5_Click(object sender, System.EventArgs e) {
			_procCur.Surf="5";
			errorProvider2.SetError(groupSextant,"");
		}

		private void radioS6_Click(object sender, System.EventArgs e) {
			_procCur.Surf="6";
			errorProvider2.SetError(groupSextant,"");
		}

		private void checkNoBillIns_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsWriteOffEdit,_procCur.DateEntryC)) {
				checkNoBillIns.CheckState=checkNoBillIns.Checked ? CheckState.Unchecked : CheckState.Checked;
				return;
			}
			if(checkNoBillIns.CheckState==CheckState.Indeterminate){
				//not allowed to set to indeterminate, so move on
				checkNoBillIns.CheckState=CheckState.Unchecked;
			}
			Cursor=Cursors.WaitCursor;
			foreach(ClaimProc claimProc in _listClaimProcsForProc) {
				if(!claimProc.Status.In(ClaimProcStatus.Estimate,ClaimProcStatus.CapClaim,ClaimProcStatus.CapEstimate)) {
					continue;
				}
				claimProc.NoBillIns=(checkNoBillIns.CheckState==CheckState.Checked);
				ClaimProcs.Update(claimProc);
			}
			//next lines are needed to recalc BaseEst, etc, for claimprocs that are no longer NoBillIns
			//also, if they are NoBillIns, then it clears out the other values.
			Procedures.ComputeEstimates(_procCur,_patCur.PatNum,ref _listClaimProcsForProc,false,_listInsPlans,_listPatPlans,_listBenefits,
				null,null,true,
				_patCur.Age,_listInsSubs,
				null,false,false,ListSubstLinks,false,
				null,LookupFees
				);
			FillIns();
			Cursor=Cursors.Default;
		}

		private void textNotes_TextChanged(object sender, System.EventArgs e) {
			CheckForCompleteNote();
			if(!_isStartingUp//so this happens only if user changes the note
				&& !_sigChanged)//and the original signature is still showing.
			{
				signatureBoxWrapper.ClearSignature();
				//this will call OnSignatureChanged to set UserNum, textUser, and SigChanged
			}
		}

		private void CheckForCompleteNote(){
			if(textNotes.Text.IndexOf("\"\"")==-1){
				//no occurances of ""
				labelIncomplete.Visible=false;
			}
			else{
				labelIncomplete.Visible=true;
			}
		}

		private void butSearch_Click(object sender,EventArgs e) {
			InputBox input=new InputBox(Lan.g(this,"Search for"));
			input.ShowDialog();
			if(input.DialogResult!=DialogResult.OK) {
				return;
			}
			string searchText=input.textResult.Text;
			int index=textNotes.Find(input.textResult.Text);//Gets the location of the first character in the control.
			if(index<0) {//-1 is returned when the text is not found.
				textNotes.DeselectAll();
				MessageBox.Show("\""+searchText+"\"\r\n"+Lan.g(this,"was not found in the notes")+".");
				return;
			}
			textNotes.Select(index,searchText.Length);
		}

		private void signatureBoxWrapper_SignatureChanged(object sender,EventArgs e) {
			_sigChanged=true;
			_procCur.UserNum=_curUser.UserNum;
			textUser.Text=_curUser.UserName;
		}

		private void buttonUseAutoNote_Click(object sender,EventArgs e) {
			FormAutoNoteCompose FormA=new FormAutoNoteCompose();
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.OK) {
				textNotes.AppendText(FormA.CompletedNote);
				bool hasAutoNotePrompt=Regex.IsMatch(textNotes.Text,_autoNotePromptRegex);
				butEditAutoNote.Visible=hasAutoNotePrompt;
			}
		}

		private void butEditAutoNote_Click(object sender,EventArgs e) {
			if(Regex.IsMatch(textNotes.Text,_autoNotePromptRegex)) {
				FormAutoNoteCompose FormA=new FormAutoNoteCompose();
				FormA.MainTextNote=textNotes.Text;
				FormA.ShowDialog();
				if(FormA.DialogResult==DialogResult.OK) {
					textNotes.Text=FormA.CompletedNote;
					bool hasAutoNotePrompt=Regex.IsMatch(textNotes.Text,_autoNotePromptRegex);
					butEditAutoNote.Visible=hasAutoNotePrompt;
				}
			}
			else {
				MessageBox.Show(Lan.g(this,"No Auto Note available to edit."));
			}
		}

		/*private void butShowMedical_Click(object sender,EventArgs e) {
			if(groupMedical.Height<200) {
				groupMedical.Height=200;
				butShowMedical.Text="^";
			}
			else {
				groupMedical.Height=170;
				butShowMedical.Text="V";
			}
		}*/

		private void comboDPC_SelectionChangeCommitted(object sender,EventArgs e) {
			DateTime tempDate=DateTime.Today;
			if(textDateTP.Text!="") {
				tempDate=PIn.Date(textDateTP.Text);
			}
			switch(comboDPC.SelectedIndex) {
				case 2:
					tempDate=tempDate.Date.AddDays(1);
					if(_cancelledScheduleByDate.Year>1880 && _cancelledScheduleByDate<tempDate) {
						tempDate=_cancelledScheduleByDate;
					}
					break;
				case 3:
					tempDate=tempDate.Date.AddDays(30);
					if(_cancelledScheduleByDate.Year>1880 && _cancelledScheduleByDate<tempDate) {
						tempDate=_cancelledScheduleByDate;
					}
					break;
				case 4:
					tempDate=tempDate.Date.AddDays(60);
					if(_cancelledScheduleByDate.Year>1880 && _cancelledScheduleByDate<tempDate) {
						tempDate=_cancelledScheduleByDate;
					}
					break;
				case 5:
					tempDate=tempDate.Date.AddDays(120);
					if(_cancelledScheduleByDate.Year>1880 && _cancelledScheduleByDate<tempDate) {
						tempDate=_cancelledScheduleByDate;
					}
					break;
				case 6:
					tempDate=tempDate.Date.AddYears(1);
					if(_cancelledScheduleByDate.Year>1880 && _cancelledScheduleByDate<tempDate) {
						tempDate=_cancelledScheduleByDate;
					}
					break;
			}
			textDateScheduled.Text=tempDate.ToShortDateString();
			labelScheduleBy.Visible=false;
			if(comboDPC.SelectedIndex==0
				|| comboDPC.SelectedIndex==1
				|| comboDPC.SelectedIndex==7
				|| comboDPC.SelectedIndex==8) {
				textDateScheduled.Text="";
				labelScheduleBy.Visible=true;
			}
		}

		private void comboStatus_SelectionChangeCommitted(object sender,EventArgs e) {
			switch(comboStatus.SelectedIndex) {
				case 0:
					comboProcStatus.SetSelectedEnum(ProcStat.TP);
					_procCur.ProcStatus=ProcStat.TP;
					break;
				case 1:
					if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text),_procCur.CodeNum,PIn.Double(textProcFee.Text))) {
						//set it back to whatever it was before
						if(_orionProcCur.Status2==OrionStatus.TP) {
							comboStatus.SelectedIndex=0;
						}
						if(_orionProcCur.Status2==OrionStatus.E) {
							comboStatus.SelectedIndex=2;
						}
						if(_orionProcCur.Status2==OrionStatus.R) {
							comboStatus.SelectedIndex=3;
						}
						if(_orionProcCur.Status2==OrionStatus.RO) {
							comboStatus.SelectedIndex=4;
						}
						if(_orionProcCur.Status2==OrionStatus.CS) {
							comboStatus.SelectedIndex=5;
						}
						if(_orionProcCur.Status2==OrionStatus.CR) {
							comboStatus.SelectedIndex=6;
						}
						if(_orionProcCur.Status2==OrionStatus.CA_Tx) {
							comboStatus.SelectedIndex=7;
						}
						if(_orionProcCur.Status2==OrionStatus.CA_EPRD) {
							comboStatus.SelectedIndex=8;
						}
						if(_orionProcCur.Status2==OrionStatus.CA_PD) {
							comboStatus.SelectedIndex=9;
						}
						if(_orionProcCur.Status2==OrionStatus.S) {
							comboStatus.SelectedIndex=10;
						}
						if(_orionProcCur.Status2==OrionStatus.ST) {
							comboStatus.SelectedIndex=11;
						}
						if(_orionProcCur.Status2==OrionStatus.W) {
							comboStatus.SelectedIndex=12;
						}
						if(_orionProcCur.Status2==OrionStatus.A) {
							comboStatus.SelectedIndex=13;
						}
						return;
					}
					comboProcStatus.SetSelectedEnum(ProcStat.C);
					_procCur.ProcStatus=ProcStat.C;
					textTimeStart.Text=MiscData.GetNowDateTime().ToShortTimeString();
					break;
				case 2:
					comboProcStatus.SetSelectedEnum(ProcStat.EO);
					_procCur.ProcStatus=ProcStat.EO;
					break;
				case 3:
					comboProcStatus.SetSelectedEnum(ProcStat.TP);
					_procCur.ProcStatus=ProcStat.TP;
					break;
				case 4:
					comboProcStatus.SetSelectedEnum(ProcStat.R);
					_procCur.ProcStatus=ProcStat.R;
					break;
				case 5:
					comboProcStatus.SetSelectedEnum(ProcStat.EO);
					_procCur.ProcStatus=ProcStat.EO;
					break;
				case 6:
					comboProcStatus.SetSelectedEnum(ProcStat.EO);
					_procCur.ProcStatus=ProcStat.EO;
					break;
				case 7:
					comboProcStatus.SetSelectedEnum(ProcStat.TP);
					_procCur.ProcStatus=ProcStat.TP;
					break;
				case 8:
					comboProcStatus.SetSelectedEnum(ProcStat.TP);
					_procCur.ProcStatus=ProcStat.TP;
					break;
				case 9:
					comboProcStatus.SetSelectedEnum(ProcStat.TP);
					_procCur.ProcStatus=ProcStat.TP;
					break;
				case 10:
					comboProcStatus.SetSelectedEnum(ProcStat.TP);
					_procCur.ProcStatus=ProcStat.TP;
					break;
				case 11:
					comboProcStatus.SetSelectedEnum(ProcStat.TP);
					_procCur.ProcStatus=ProcStat.TP;
					break;
				case 12:
					comboProcStatus.SetSelectedEnum(ProcStat.Cn);
					_procCur.ProcStatus=ProcStat.Cn;
					break;
				case 13:
					comboProcStatus.SetSelectedEnum(ProcStat.TP);
					_procCur.ProcStatus=ProcStat.TP;
					break;
			}
			_orionProcCur.Status2=(OrionStatus)((int)(Math.Pow(2d,(double)(comboStatus.SelectedIndex))));
			//Do not automatically set the stop clock date if status is set to treatment planned, existing, or watch.
			if(_orionProcCur.Status2==OrionStatus.TP || _orionProcCur.Status2==OrionStatus.E || _orionProcCur.Status2==OrionStatus.W) {
				//Clear the stop the clock date if there was no stop the clock date defined in a previous edit. Therefore, for a new procedure, always clear.
				if(_orionProcCur.DateStopClock.Year<1880){
					textDateStop.Text="";
				}
			}
			else {//Set the stop the clock date for all other statuses.
				//Use the previously set stop the clock date if one exists. Will never be true if this is a new procedure.
				if(_orionProcCur.DateStopClock.Year>1880) {
					textDateStop.Text=_orionProcCur.DateStopClock.ToShortDateString();
				}
				else {
					//When the stop the clock date has not already been set, set to the ProcDate for the procedure.
					textDateStop.Text=textDate.Text.Trim();
				}
			}
		}

		private void textTimeStart_TextChanged(object sender,EventArgs e) {
			UpdateFinalMin();			
		}

		private void textTimeEnd_TextChanged(object sender,EventArgs e) {
			UpdateFinalMin();
		}

		///<summary>Populates final time box with total number of minutes.</summary>
		private void UpdateFinalMin() {
			if(textTimeStart.Text=="" || textTimeEnd.Text=="") {
				return;
			}
			int startTime=0;
			int stopTime=0;
			try {
				startTime=PIn.Int(textTimeStart.Text);
			}
			catch { 
				try {//Try DateTime format.
					DateTime sTime=DateTime.Parse(textTimeStart.Text);
					startTime=(sTime.Hour*(int)Math.Pow(10,2))+sTime.Minute;
				}
				catch {//Not a valid time.
					return;
				}
			}
			try {
				stopTime=PIn.Int(textTimeEnd.Text);
			}
			catch { 
				try {//Try DateTime format.
					DateTime eTime=DateTime.Parse(textTimeEnd.Text);
					stopTime=(eTime.Hour*(int)Math.Pow(10,2))+eTime.Minute;
				}
				catch {//Not a valid time.
					return;
				}
			}
			int total=(((stopTime/100)*60)+(stopTime%100))-(((startTime/100)*60)+(startTime%100));
			textTimeFinal.Text=total.ToString();
		}

		///<summary>Returns min value if blank or invalid string passed in.</summary>
		private DateTime ParseTime(string time) {
			string militaryTime=time;
			DateTime dTime=DateTime.MinValue;
			if(militaryTime=="") {
				return dTime;
			}
			if(militaryTime.Length<4) {
				militaryTime=militaryTime.PadLeft(4,'0');
			}
			//Test if user typed in military time. Ex: 0830 or 1536
			try {
				int hour=PIn.Int(militaryTime.Substring(0,2));
				int minute=PIn.Int(militaryTime.Substring(2,2));
				dTime=new DateTime(1,1,1,hour,minute,0);
				return dTime;
			}
			catch { }
			//Test if user typed in a typical DateTime format. Ex: 1:00 PM
			try { 
				return DateTime.Parse(time);
			}
			catch { }
			return dTime;
		}

		private void UpdateSurf() {
			if(!Tooth.IsValidEntry(textTooth.Text)){
				return;
			}
			errorProvider2.SetError(textSurfaces,"");
			textSurfaces.Text="";
			if(butM.BackColor==Color.White) {
				textSurfaces.AppendText("M");
			}
			if(butOI.BackColor==Color.White) {
				//if(ToothGraphic.IsAnterior(Tooth.FromInternat(textTooth.Text))) {
				if(Tooth.IsAnterior(Tooth.FromInternat(textTooth.Text))) {
					textSurfaces.AppendText("I");
				}
				else {
					textSurfaces.AppendText("O");
				}
			}
			if(butD.BackColor==Color.White) {
				textSurfaces.AppendText("D");
			}
			if(butV.BackColor==Color.White) {
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					textSurfaces.AppendText("5");
				}
				else {
					textSurfaces.AppendText("V");
				}
			}
			if(butBF.BackColor==Color.White) {
				//if(ToothGraphic.IsAnterior(Tooth.FromInternat(textTooth.Text))) {
				if(Tooth.IsAnterior(Tooth.FromInternat(textTooth.Text))) {
					if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
						textSurfaces.AppendText("V");//vestibular
					}
					else {
						textSurfaces.AppendText("F");
					}
				}
				else {
					textSurfaces.AppendText("B");
				}
			}
			if(butL.BackColor==Color.White) {
				textSurfaces.AppendText("L");
			}
		}

		private void butM_Click(object sender,EventArgs e) {
			if(butM.BackColor==Color.White) {
				butM.BackColor=SystemColors.Control;
			}
			else {
				butM.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butOI_Click(object sender,EventArgs e) {
			if(butOI.BackColor==Color.White) {
				butOI.BackColor=SystemColors.Control;
			}
			else {
				butOI.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butL_Click(object sender,EventArgs e) {
			if(butL.BackColor==Color.White) {
				butL.BackColor=SystemColors.Control;
			}
			else {
				butL.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butV_Click(object sender,EventArgs e) {
			if(butV.BackColor==Color.White) {
				butV.BackColor=SystemColors.Control;
			}
			else {
				butV.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butBF_Click(object sender,EventArgs e) {
			if(butBF.BackColor==Color.White) {
				butBF.BackColor=SystemColors.Control;
			}
			else {
				butBF.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butD_Click(object sender,EventArgs e) {
			if(butD.BackColor==Color.White) {
				butD.BackColor=SystemColors.Control;
			}
			else {
				butD.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butPickSite_Click(object sender,EventArgs e) {
			FormSites FormS=new FormSites();
			FormS.IsSelectionMode=true;
			FormS.SelectedSiteNum=_procCur.SiteNum;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK){
				return;
			}
			_procCur.SiteNum=FormS.SelectedSiteNum;
			textSite.Text=Sites.GetDescription(_procCur.SiteNum);
		}

		///<summary>This button is only visible if 1. Pref ProcLockingIsAllowed is true, 2. Proc isn't already locked, 3. Proc status is C.</summary>
		private void butLock_Click(object sender,EventArgs e) {
			if(!EntriesAreValid()) {
				return;
			}
			_procCur.IsLocked=true;
			SaveAndClose();//saves all the other various changes that the user made
			DialogResult=DialogResult.OK;
		}

		///<summary>This button is only visible when proc IsLocked.</summary>
		private void butInvalidate_Click(object sender,EventArgs e) {
			Permissions perm=Permissions.ProcComplEdit;
			if(_procCur.ProcStatus.In(ProcStat.EO,ProcStat.EC)) {
				perm=Permissions.ProcExistingEdit;
			}
			//What this will really do is "delete" the procedure.
			if(!Security.IsAuthorized(perm,_procCur.ProcDate)) {
				return;
			}
			if(Procedures.IsAttachedToClaim(_procCur,_listClaimProcsForProc)) {
				MsgBox.Show(this,"This procedure is attached to a claim and cannot be invalidated without first deleting the claim.");
				return;
			}
			try {
				Procedures.Delete(_procCur.ProcNum);//also deletes any claimprocs (other than ins payments of course).
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			SecurityLogs.MakeLogEntry(perm,_patCur.PatNum,Lan.g(this,"Invalidated: ")+
				ProcedureCodes.GetStringProcCode(_procCur.CodeNum).ToString()+" ("+_procCur.ProcStatus+"), "
				+_procCur.ProcDate.ToShortDateString()+", "+_procCur.ProcFee.ToString("c")+", Deleted");
			DialogResult=DialogResult.OK;
		}

		///<summary>This button is only visible when proc IsLocked.</summary>
		private void butAppend_Click(object sender,EventArgs e) {
			FormProcNoteAppend formPNA=new FormProcNoteAppend();
			formPNA.ProcCur=_procCur;
			formPNA.ShowDialog();
			if(formPNA.DialogResult!=DialogResult.OK) {
				return;
			}
			DialogResult=DialogResult.OK;//exit out of this window.  Change already saved, and OK button is disabled in this window, anyway.
		}

		private void butSnomedBodySiteSelect_Click(object sender,EventArgs e) {
			FormSnomeds formS=new FormSnomeds();
			formS.IsSelectionMode=true;
			if(formS.ShowDialog()==DialogResult.OK) {
				_snomedBodySite=formS.SelectedSnomed;
				textSnomedBodySite.Text=_snomedBodySite.Description;
			}
		}

		private void butNoneSnomedBodySite_Click(object sender,EventArgs e) {
			_snomedBodySite=null;
			textSnomedBodySite.Text="";
		}

		private void SetIcdLabels() {
			byte icdVersion=9;
			if(checkIcdVersion.Checked) {
				icdVersion=10;
			}
			labelDiagnosisCode.Text=Lan.g(this,"ICD")+"-"+icdVersion+" "+Lan.g(this,"Diagnosis Code 1");
			labelDiagnosisCode2.Text=Lan.g(this,"ICD")+"-"+icdVersion+" "+Lan.g(this,"Diagnosis Code 2");
			labelDiagnosisCode3.Text=Lan.g(this,"ICD")+"-"+icdVersion+" "+Lan.g(this,"Diagnosis Code 3");
			labelDiagnosisCode4.Text=Lan.g(this,"ICD")+"-"+icdVersion+" "+Lan.g(this,"Diagnosis Code 4");
		}

		private void checkIcdVersion_Click(object sender,EventArgs e) {
			SetIcdLabels();
		}

		private void PickDiagnosisCode(TextBox textBoxDiagnosisCode) {
			if(checkIcdVersion.Checked) {//ICD-10
				FormIcd10s formI=new FormIcd10s();
				formI.IsSelectionMode=true;
				if(formI.ShowDialog()==DialogResult.OK) {
					textBoxDiagnosisCode.Text=formI.SelectedIcd10.Icd10Code;
				}
			}
			else {//ICD-9
				FormIcd9s formI=new FormIcd9s();
				formI.IsSelectionMode=true;
				if(formI.ShowDialog()==DialogResult.OK) {
					textBoxDiagnosisCode.Text=formI.SelectedIcd9.ICD9Code;
				}
			}
		}

		private void butDiagnosisCode1_Click(object sender,EventArgs e) {
			PickDiagnosisCode(textDiagnosisCode);
		}

		private void butNoneDiagnosisCode1_Click(object sender,EventArgs e) {
			textDiagnosisCode.Text="";
		}

		private void butDiagnosisCode2_Click(object sender,EventArgs e) {
			PickDiagnosisCode(textDiagnosisCode2);
		}

		private void butNoneDiagnosisCode2_Click(object sender,EventArgs e) {
			textDiagnosisCode2.Text="";
		}

		private void butDiagnosisCode3_Click(object sender,EventArgs e) {
			PickDiagnosisCode(textDiagnosisCode3);
		}

		private void butNoneDiagnosisCode3_Click(object sender,EventArgs e) {
			textDiagnosisCode3.Text="";
		}

		private void butDiagnosisCode4_Click(object sender,EventArgs e) {
			PickDiagnosisCode(textDiagnosisCode4);
		}

		private void butNoneDiagnosisCode4_Click(object sender,EventArgs e) {
			textDiagnosisCode4.Text="";
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(IsNew) {
				DialogResult=DialogResult.Cancel;//verified that this triggers a delete when window closed from all places where FormProcEdit is used, and where proc could be new.
				return;
			}
			//If this is an existing completed proc, then this delete button is only enabled if the user has permission for ProcComplEdit based on the ProcDate.
			if(!_procOld.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)
				&& !Security.IsAuthorized(Permissions.ProcDelete,_procCur.ProcDate)) //This should be a much more lenient permission since completed procedures are already safeguarded.
			{
				return;
			}
			if(!Procedures.IsProcComplEditAuthorized(_procOld,true)) {
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Delete Procedure?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
				return;
			}
			if(_orthoProcLink!=null) {
				MsgBox.Show(this,"Not allowed to delete a procedure that is linked to an ortho case. " +
					"Detach the procedure from the ortho case or delete the ortho case first.");
				return;
			}
			try {
				Procedures.Delete(_procCur.ProcNum);//also deletes the claimProcs and adjustments. Might throw exception.
				_isEstimateRecompute=true;
				Recalls.Synch(_procCur.PatNum);//needs to be moved into Procedures.Delete
				Permissions perm=Permissions.ProcDelete;
				string tag="";
				switch(_procOld.ProcStatus) {
					case ProcStat.C:
						perm=Permissions.ProcComplEdit;
						tag=", "+Lan.g(this,"Deleted");
						break;
					case ProcStat.EO:
					case ProcStat.EC:
						perm=Permissions.ProcExistingEdit;
						tag=", "+Lan.g(this,"Deleted");
						break;
				}
				SecurityLogs.MakeLogEntry(perm,_procOld.PatNum,
					ProcedureCodes.GetProcCode(_procOld.CodeNum).ProcCode+" ("+_procOld.ProcStatus+"), "+_procOld.ProcFee.ToString("c")+tag);
				DialogResult=DialogResult.OK;
				Plugins.HookAddCode(this,"FormProcEdit.butDelete_Click_end",_procCur);
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
			}
		}

		private bool EntriesAreValid(){
			#region Surfaces, Tooth, Sextant, Arch, Date UI
			if(  textDateTP.errorProvider1.GetError(textDateTP)!=""
				|| textDate.errorProvider1.GetError(textDate)!=""
				|| textProcFee.errorProvider1.GetError(textProcFee)!=""
				|| textDateOriginalProsth.errorProvider1.GetError(textDateOriginalProsth)!=""
				|| textDiscount.errorProvider1.GetError(textDiscount)!=""
				){
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return false;
			}
			if(textDate.Text==""){
				MessageBox.Show(Lan.g(this,"Please enter a date first."));
				return false;
			}
			#endregion
			#region Note
			//There have been 2 or 3 cases where a customer entered a note with thousands of new lines and when OD tries to display such a note in the chart, a GDI exception occurs because the progress notes grid is very tall and takes up too much video memory. To help prevent this issue, we block the user from entering any note where there are 50 or more consecutive new lines anywhere in the note. Any number of new lines less than 50 are considered to be intentional.
			StringBuilder tooManyNewLines=new StringBuilder();
			for(int i=0;i<50;i++) {
				tooManyNewLines.Append("\r\n");
			}
			if(textNotes.Text.Contains(tooManyNewLines.ToString())) {
				MsgBox.Show(this,"The notes contain 50 or more consecutive blank lines. Probably unintentional and must be fixed.");
				return false;
			}
			#endregion
			#region textTimeStart, textTimeEnd validation
			if(!ProcedureL.AreTimesValid(textTimeStart.Text,textTimeEnd.Text)) {
				return false;
			}
			#endregion
			#region textUnitQty validation
			if(!ProcedureL.IsQuantityValid(PIn.Int(textUnitQty.Text,false))) {
				return false;
			}
			#endregion
			#region Provider UI
			if(comboProv.GetSelectedProvNum()==0){
				MsgBox.Show(this,"You must select a provider first.");
				return false;
			}
			#endregion
			if(errorProvider2.GetError(textSurfaces)!=""
				|| errorProvider2.GetError(textTooth)!="")
			{
				MsgBox.Show(this,"Please fix tooth number or surfaces first.");
				return false;
			}
			if(errorProvider2.GetError(groupSextant)!=""
				|| errorProvider2.GetError(groupArch)!="") 
			{
				MsgBox.Show(this,"Please fix arch or sextant first.");
				return false;
			}
			#region Medical Code
			if(textMedicalCode.Text!="" && !ProcedureCodes.GetContainsKey(textMedicalCode.Text)){
				MsgBox.Show(this,"Invalid medical code.  It must refer to an existing procedure code.");
				return false;
			}
			#endregion
			#region Drug UI
			if(textDrugNDC.Text!=""){
				if(comboDrugUnit.SelectedIndex==(int)EnumProcDrugUnit.None || textDrugQty.Text==""){
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Drug quantity and unit are not entered.  Continue anyway?")){
						return false;
					}
				}
			}
			if(textDrugQty.Text!=""){
				try{
					float.Parse(textDrugQty.Text);
				}
				catch{
					MsgBox.Show(this,"Please fix drug qty first.");
					return false;
				}
			}
			#endregion
			#region Procedure Status
			//If user is trying to change status to complete and using eCW.
			if(_procCur.ProcStatus==ProcStat.C && (IsNew || _procOld.ProcStatus!=ProcStat.C) && Programs.UsingEcwTightOrFullMode()) {
				MsgBox.Show(this,"Procedures cannot be set complete in this window.  Set the procedure complete by setting the appointment complete.");
				return false;
			}
			if(_procCur.ProcStatus==ProcStat.C && PIn.Date(textDate.Text).Date > DateTime.Today.Date && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
				MsgBox.Show(this,"Completed procedures cannot have future dates.");
				return false;
			}
			if(_procOld.ProcStatus!=ProcStat.C && _procCur.ProcStatus==ProcStat.C){//if status was changed to complete
				if(_procCur.AptNum!=0) {//if attached to an appointment
					Appointment apt=Appointments.GetOneApt(_procCur.AptNum);
					if(apt.AptDateTime.Date > MiscData.GetNowDateTime().Date) {//if appointment is in the future
						MessageBox.Show(Lan.g(this,"Not allowed because procedure is attached to a future appointment with a date of ")
							+apt.AptDateTime.ToShortDateString());
						return false;
					}
					if(apt.AptDateTime.Year>=1880) {
						textDate.Text=apt.AptDateTime.ToShortDateString();
					}
				}
				if(!_isQuickAdd && !Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text))){//use the new date
					return false;
				}
			}
			else if(!_isQuickAdd && IsNew && _procCur.ProcStatus==ProcStat.C) {//if new procedure is complete
				if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text),_procCur.CodeNum,PIn.Double(textProcFee.Text))){
					return false;
				}
			}
			else if(!IsNew){//an old procedure
				if(_procOld.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)) {//that was already complete
					if(!ProcedureL.CheckPermissionsAndGlobalLockDate(_procOld,_procCur,PIn.Date(textDate.Text),PIn.Double(textProcFee.Text))) {
						return false;
					}
				}
			}
			#endregion
			#region Canada and Prosthesis
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				if(checkTypeCodeX.Checked) {
					if(checkTypeCodeA.Checked
						|| checkTypeCodeB.Checked
						|| checkTypeCodeC.Checked
						|| checkTypeCodeE.Checked
						|| checkTypeCodeL.Checked
						|| checkTypeCodeS.Checked) 
					{
						MsgBox.Show(this,"If type code 'none' is checked, no other type codes may be checked.");
						return false;
					}
				}
				if(_procedureCode2.IsProsth
					&& !checkTypeCodeA.Checked
					&& !checkTypeCodeB.Checked
					&& !checkTypeCodeC.Checked
					&& !checkTypeCodeE.Checked
					&& !checkTypeCodeL.Checked
					&& !checkTypeCodeS.Checked
					&& !checkTypeCodeX.Checked) 
				{
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"At least one type code should be checked for prosthesis.  Continue anyway?")) {
						return false;
					}
				}
				if(textCanadaLabFee1.errorProvider1.GetError(textCanadaLabFee1)!="" || textCanadaLabFee2.errorProvider1.GetError(textCanadaLabFee2)!="") {
					MessageBox.Show(Lan.g(this,"Please fix lab fees."));
					return false;
				}
			}
			else {
				if(_procedureCode2.IsProsth) {
					if(listProsth.SelectedIndex==0
					|| (listProsth.SelectedIndex==2 && textDateOriginalProsth.Text=="")) {
						if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Prosthesis date not entered. Continue anyway?")){
							return false;
						}
					}
				}
			}
#endregion
			#region Orion Validation
			if(Programs.UsingOrion) {
			//if(panelOrion.Visible) {
				if(comboStatus.SelectedIndex==-1) {
					MsgBox.Show(this,"Invalid status.");
					return false;
				}
				if(textDateScheduled.errorProvider1.GetError(textDateScheduled)!="") {
					MsgBox.Show(this,"Invalid schedule date.");
					return false;
				}
				if(textDateStop.errorProvider1.GetError(textDateStop)!="") {
					MsgBox.Show(this,"Invalid stop clock date.");
					return false;
				}
			}
			#endregion
			#region Quadrant UI
			if(_procedureCode2.TreatArea==TreatmentArea.Quad) {
				if(!radioUL.Checked && !radioUR.Checked && !radioLL.Checked && !radioLR.Checked) {
					MsgBox.Show(this,"Please select a quadrant.");
					return false;
				}
			}
			#endregion
			#region Provider
			_listClaimProcsForProc=ClaimProcs.GetForProc(ClaimProcs.Refresh(_procCur.PatNum),_procCur.ProcNum);//update for accuracy
			if(!ProcedureL.ValidateProvider(_listClaimProcsForProc,comboProv.GetSelectedProvNum(),_procOld.ProvNum)) {
				return false;
			}
			#endregion
			//Block if proc is linked to ortho case and user tries to set status from complete to any other status.
			if(_orthoProcLink!=null && (_procOld.ProcStatus==ProcStat.C && _procCur.ProcStatus!=ProcStat.C)) {
				MsgBox.Show(this,"The status of a completed procedure that is attached to an ortho case cannot be changed. " +
					"Detach the procedure from the ortho case or delete the ortho case first.");
				return false;
			}
			//Customers have been complaining about procedurelog entries changing their CodeNum column to 0.
			//Based on a security log provided by a customer, we were able to determine that this is one of two potential violators.
			//The following code is here simply to try and get the user to call us so that we can have proof and hopefully find the core of the issue.
			long verifyCode=ProcedureCodes.GetProcCode(textProc.Text).CodeNum;
			try {
				if(verifyCode < 1) {
					throw new ApplicationException("Invalid Procedure Text");
				}
			}
			catch(ApplicationException ae) {
				string error="Please notify support with the following information.\r\n"
					+"Error: "+ae.Message+"\r\n"
					+"verifyCode: "+verifyCode.ToString()+"\r\n"
					+"textProc.Text: "+textProc.Text+"\r\n"
					+"ProcOld.CodeNum: "+(_procOld==null ? "NULL" : _procOld.CodeNum.ToString())+"\r\n"
					+"ProcCur.CodeNum: "+(_procCur==null ? "NULL" : _procCur.CodeNum.ToString())+"\r\n"
					+"ProcedureCode2.CodeNum: "+(_procedureCode2==null ? "NULL" : _procedureCode2.CodeNum.ToString())+"\r\n"
					+"\r\n"
					+"StackTrace:\r\n"+ae.StackTrace;
				MsgBoxCopyPaste MsgBCP=new MsgBoxCopyPaste(error);
				MsgBCP.Text="Fatal Error!!!";
				MsgBCP.Show();//Use .Show() to make it easy for the user to keep this window open while they call in.
				return false;
			}
			return true;
		}

		///<summary>MUST call EntriesAreValid first.  Used from OK_Click and from butSetComplete_Click</summary>
		private void SaveAndClose() {
			if(textProcFee.Text=="") {
				textProcFee.Text="0";
			}
			_procCur.PatNum=_patCur.PatNum;
			//ProcCur.Code=this.textProc.Text;
			_procedureCode2=ProcedureCodes.GetProcCode(textProc.Text);
			_procCur.CodeNum=_procedureCode2.CodeNum;
			_procCur.MedicalCode=textMedicalCode.Text;
			_procCur.Discount=PIn.Double(textDiscount.Text);
			if(_snomedBodySite==null) {
				_procCur.SnomedBodySite="";
			}
			else {
				_procCur.SnomedBodySite=_snomedBodySite.SnomedCode;
			}
			_procCur.IcdVersion=9;
			if(checkIcdVersion.Checked) {
				_procCur.IcdVersion=10;
			}
			_procCur.DiagnosticCode="";
			_procCur.DiagnosticCode2="";
			_procCur.DiagnosticCode3="";
			_procCur.DiagnosticCode4="";
			List<string> diagnosticCodes=new List<string>();//A list of all the diagnostic code boxes.
			if(textDiagnosisCode.Text!="") {
				diagnosticCodes.Add(textDiagnosisCode.Text);
			}
			if(textDiagnosisCode2.Text!="") {
				diagnosticCodes.Add(textDiagnosisCode2.Text);
			}
			if(textDiagnosisCode3.Text!="") {
				diagnosticCodes.Add(textDiagnosisCode3.Text);
			}
			if(textDiagnosisCode4.Text!="") {
				diagnosticCodes.Add(textDiagnosisCode4.Text);
			}
			if(diagnosticCodes.Count>0) {
				_procCur.DiagnosticCode=diagnosticCodes[0];
			}
			if(diagnosticCodes.Count>1) {
				_procCur.DiagnosticCode2=diagnosticCodes[1];
			}
			if(diagnosticCodes.Count>2) {
				_procCur.DiagnosticCode3=diagnosticCodes[2];
			}
			if(diagnosticCodes.Count>3) {
				_procCur.DiagnosticCode4=diagnosticCodes[3];
			}
			_procCur.IsPrincDiag=checkIsPrincDiag.Checked;
			_procCur.ProvOrderOverride=_selectedProvOrderNum;
			if(_referralOrdering==null) {
				_procCur.OrderingReferralNum=0;
			}
			else {
				_procCur.OrderingReferralNum=_referralOrdering.ReferralNum;
			}
			_procCur.CodeMod1 = textCodeMod1.Text;
			_procCur.CodeMod2 = textCodeMod2.Text;
			_procCur.CodeMod3 = textCodeMod3.Text;
			_procCur.CodeMod4 = textCodeMod4.Text;
			_procCur.UnitQty = PIn.Int(textUnitQty.Text);
			_procCur.UnitQtyType=(ProcUnitQtyType)comboUnitType.SelectedIndex;
			_procCur.RevCode = textRevCode.Text;
			_procCur.DrugUnit=(EnumProcDrugUnit)comboDrugUnit.SelectedIndex;
			_procCur.DrugQty=PIn.Float(textDrugQty.Text);
			_procCur.Urgency=(checkIsEmergency.Checked?ProcUrgency.Emergency:ProcUrgency.Normal);
			_procCur.ProvNum=comboProv.GetSelectedProvNum();			
			ClaimProcs.TrySetProvFromProc(_procCur,_listClaimProcsForProc);
			_procCur.ClinicNum=comboClinic.SelectedClinicNum;
			bool hasSplitProvChanged=false;
			bool hasAdjProvChanged=false;
			if(_procCur.ProvNum!=_procOld.ProvNum) {
				if(PaySplits.IsPaySplitAttached(_procCur.ProcNum)) {
					List<PaySplit> listPaySplit=PaySplits.GetPaySplitsFromProc(_procCur.ProcNum);
					foreach(PaySplit paySplit in listPaySplit) {
						if(!Security.IsAuthorized(Permissions.PaymentEdit,Payments.GetPayment(paySplit.PayNum).PayDate)) {
							return;
						}
						if(_procCur.ProvNum != paySplit.ProvNum) {
							hasSplitProvChanged=true;
						}
					}
					if(hasSplitProvChanged
						&& !MsgBox.Show(this,MsgBoxButtons.OKCancel,"The provider for the associated payment splits will be changed to match the provider on the procedure.")) {
						return;
					}
				}
				List<Adjustment> listAdjusts=_adjustmentsForProc.Cast<Adjustment>().ToList();
				foreach(Adjustment adjust in listAdjusts) {
					if(!Security.IsAuthorized(Permissions.AdjustmentEdit,adjust.AdjDate)) {
						return;
					}
					if(_procCur.ProvNum!=adjust.ProvNum && PrefC.GetInt(PrefName.RigorousAdjustments)==(int)RigorousAdjustments.EnforceFully) {
						hasAdjProvChanged=true;
					}
				}
				if(hasAdjProvChanged
					&& !MsgBox.Show(this,MsgBoxButtons.OKCancel,"The provider for the associated adjustments will be changed to match the provider on the procedure.")) {
					return;
				}
			}
			double sumPaySplits=0;
			for(int i=0;i<_paySplitsForProc.Count;i++) {
				sumPaySplits+=((PaySplit)_paySplitsForProc[i]).SplitAmt;
			}
			if(_procOld.ProcStatus==ProcStat.C && _procCur.ProcStatus!=ProcStat.C) {//Proc was complete but was changed.
				if(Adjustments.GetForProc(_procCur.ProcNum,Adjustments.Refresh(_procCur.PatNum)).Count!=0
				&& !MsgBox.Show(this,MsgBoxButtons.YesNo,"This procedure has adjustments attached to it. Changing the status from completed will delete any adjustments for the procedure. Continue?")) {
					return;
				}
				if(sumPaySplits!=0) {
					MsgBox.Show(this,"Not allowed to modify the status of a procedure that has payments attached to it. Detach payments from the procedure first.");
					return;
				}
			}
			else if(_procOld.ProcStatus!=ProcStat.C && _procCur.ProcStatus==ProcStat.C) {//Proc set complete.
				_procCur.DateEntryC=DateTime.Now;//this triggers it to set to server time NOW().
				if(_procCur.DiagnosticCode=="") {
					_procCur.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
					_procCur.IcdVersion=PrefC.GetByte(PrefName.DxIcdVersion);
				}
			}
			// textDateTP.Text is blank upon load if date in DB is before 1/1/1880. We don't want to update this if the DateTP box is left blank.
			if(_procCur.DateTP.Year>1880 || this.textDateTP.Text!="") {
				_procCur.DateTP=PIn.Date(this.textDateTP.Text);
			}
			_procCur.ProcDate=PIn.Date(this.textDate.Text);
			DateTime dateT=PIn.DateT(this.textTimeStart.Text);
			_procCur.ProcTime=new TimeSpan(dateT.Hour,dateT.Minute,0);
			if(Programs.UsingOrion || PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance)) {
				dateT=ParseTime(textTimeStart.Text);
				_procCur.ProcTime=new TimeSpan(dateT.Hour,dateT.Minute,0);
				dateT=ParseTime(textTimeEnd.Text);
				_procCur.ProcTimeEnd=new TimeSpan(dateT.Hour,dateT.Minute,0);
			}
			_procCur.ProcFee=PIn.Double(textProcFee.Text);
			//ProcCur.LabFee=PIn.PDouble(textLabFee.Text);
			//ProcCur.LabProcCode=textLabCode.Text;
			//MessageBox.Show(ProcCur.ProcFee.ToString());
			//Dx taken care of when radio pushed
			switch(_procedureCode2.TreatArea){
				case TreatmentArea.Surf:
					_procCur.ToothNum=Tooth.FromInternat(textTooth.Text);
					_procCur.Surf=Tooth.SurfTidyFromDisplayToDb(textSurfaces.Text,_procCur.ToothNum);
					break;
				case TreatmentArea.Tooth:
					_procCur.Surf="";
					_procCur.ToothNum=Tooth.FromInternat(textTooth.Text);
					break;
				case TreatmentArea.Mouth:
					_procCur.Surf="";
					_procCur.ToothNum="";	
					break;
				case TreatmentArea.Quad:
					//surf set when radio pushed
					_procCur.ToothNum="";	
					break;
				case TreatmentArea.Sextant:
					//surf taken care of when radio pushed
					_procCur.ToothNum="";	
					break;
				case TreatmentArea.Arch:
					//don't HAVE to select arch
					//taken care of when radio pushed
					_procCur.ToothNum="";	
					break;
				case TreatmentArea.ToothRange:
					//Deselect empty tooth selections in Maxillary/Upper Arch.
					for(int j=0;j<listBoxTeeth.Items.Count;j++) {
						if(listBoxTeeth.Items[j].ToString()=="") {//Can be blank when the tooth is flagged as primary when it is an adult tooth.
							listBoxTeeth.SetSelected(j,false);
						}
					}
					//Deselect empty tooth selections in Mandibular/Lower Arch.
					for(int j=0;j<listBoxTeeth2.Items.Count;j++) {
						if(listBoxTeeth2.Items[j].ToString()=="") {//Can be blank when the tooth is flagged as primary when it is an adult tooth.
							listBoxTeeth2.SetSelected(j,false);
						}
					}
					if(listBoxTeeth.SelectedItems.Count<1 && listBoxTeeth2.SelectedItems.Count<1) {
						MessageBox.Show(Lan.g(this,"Must pick at least 1 tooth"));
						return;
					}
					List <string> listSelectedToothNums=new List<string>();
					//Store selected teeth in Maxillary/Upper Arch.
					foreach(int index in listBoxTeeth.SelectedIndices) {
						listSelectedToothNums.Add((index+1).ToString());
					}
					//Store selected teeth in Mandibular/Lower Arch.
					foreach(int index in listBoxTeeth2.SelectedIndices) {
						listSelectedToothNums.Add((32-index).ToString());
					}
					//Identify selected teeth which are primary and convert from permanent tooth num to primary tooth num for storage into database.
					for(int j=0;j<listSelectedToothNums.Count;j++) {
						if(_listPriTeeth.Contains(listSelectedToothNums[j])) {
							listSelectedToothNums[j]=Tooth.PermToPri(listSelectedToothNums[j]);
						}
					}
					_procCur.ToothRange=String.Join(",",listSelectedToothNums);
					_procCur.Surf="";
					_procCur.ToothNum="";	
					break;
			}
			//Status taken care of when list pushed
			_procCur.Note=this.textNotes.Text;
			//Larger offices have trouble with doctors editing specific procedure notes at the same time.
			//One of our customers paid for custom programming that will merge the two notes together in a specific fashion if there was concurrency issues.
			//A specific preference was added because this functionality is so custom.  Typical users can just use the Chart View Audit mode for this info.
			if(_procOld.ProcNum > 0 && PrefC.GetBool(PrefName.ProcNoteConcurrencyMerge)) {
				//Go to the database to get the most recent version of the current procedure's note and check it against ProcOld.Note to see if they differ.
				List<ProcNote> listProcNotes=ProcNotes.GetProcNotesForProc(_procOld.ProcNum)
					.OrderByDescending(x => x.EntryDateTime)
					.ThenBy(x => x.ProcNoteNum)//Just in case two notes were entered at the "same time" (current version of MySQL can't handle milliseconds)
					.ToList();
				//If there are notes for the current procedure, get the most recent note and compare it to ProcOld.Note.
				//If the current database note differs from the ProcOld.Note then there was a concurrency issue and we have to merge the db note.
				if(listProcNotes.Count > 0 && _procOld.Note!=listProcNotes[0].Note) {
					//Manipulate ProcCur.Note to include the most recent note in its entirety with some custom information required by job #2484
					//Use DateTime.Now because the ProcNote won't get inserted until farther down in this method but we have to do this manipulation before sig.
					_procCur.Note=DateTime.Now.ToString()+"  "+Userods.GetName(_procCur.UserNum)+"\r\n"+_procCur.Note;
					//Now we need to append the old note from the database in the same format.
					_procCur.Note+="\r\n------------------------------------------------------\r\n"
						+listProcNotes[0].EntryDateTime.ToString()+"  "+Userods.GetName(listProcNotes[0].UserNum)
						+"\r\n"+listProcNotes[0].Note;
				}
			}
			try {
				SaveSignature();
			}
			catch(Exception ex){
				MessageBox.Show(Lan.g(this,"Error saving signature.")+"\r\n"+ex.Message);
				//and continue with the rest of this method
			}
			#region Update paysplits
			if(hasSplitProvChanged) {
				PaySplits.UpdateAttachedPaySplits(_procCur);//update the attached paysplits.
			}
			if(hasAdjProvChanged) {
				foreach(Adjustment adjust in _adjustmentsForProc) {//update the attached adjustments
					adjust.ProvNum=_procCur.ProvNum;
					Adjustments.Update(adjust);
				}
			}
			#endregion
			_procCur.HideGraphics=checkHideGraphics.Checked;
			if(comboDx.SelectedIndex!=-1) {
				_procCur.Dx=_listDiagnosisDefs[comboDx.SelectedIndex].DefNum;
			}
			if(comboPrognosis.SelectedIndex==0) {
				_procCur.Prognosis=0;
			}
			else {
				_procCur.Prognosis=_listPrognosisDefs[comboPrognosis.SelectedIndex-1].DefNum;
			}
			if(comboPriority.SelectedIndex==0) {
				_procCur.Priority=0;
			}
			else {
				_procCur.Priority=_listTxPriorityDefs[comboPriority.SelectedIndex-1].DefNum;
			}
			_procCur.PlaceService=(PlaceOfService)comboPlaceService.SelectedIndex;
			//site set when user picks from list.
			if(comboBillingTypeOne.SelectedIndex==0){
				_procCur.BillingTypeOne=0;
			}
			else{
				_procCur.BillingTypeOne=_listBillingTypeDefs[comboBillingTypeOne.SelectedIndex-1].DefNum;
			}
			if(comboBillingTypeTwo.SelectedIndex==0) {
				_procCur.BillingTypeTwo=0;
			}
			else {
				_procCur.BillingTypeTwo=_listBillingTypeDefs[comboBillingTypeTwo.SelectedIndex-1].DefNum;
			}
			_procCur.BillingNote=textBillingNote.Text;
			//ProcCur.HideGraphical=checkHideGraphical.Checked;
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				_procCur.CanadianTypeCodes="";
				if(checkTypeCodeA.Checked) {
					_procCur.CanadianTypeCodes+="A";
				}
				if(checkTypeCodeB.Checked) {
					_procCur.CanadianTypeCodes+="B";
				}
				if(checkTypeCodeC.Checked) {
					_procCur.CanadianTypeCodes+="C";
				}
				if(checkTypeCodeE.Checked) {
					_procCur.CanadianTypeCodes+="E";
				}
				if(checkTypeCodeL.Checked) {
					_procCur.CanadianTypeCodes+="L";
				}
				if(checkTypeCodeS.Checked) {
					_procCur.CanadianTypeCodes+="S";
				}
				if(checkTypeCodeX.Checked) {
					_procCur.CanadianTypeCodes+="X";
				}
				double canadaLabFee1=0;
				if(textCanadaLabFee1.Text!="") {
					canadaLabFee1=PIn.Double(textCanadaLabFee1.Text);
				}
				if(canadaLabFee1==0) {
					if(textCanadaLabFee1.Visible && _listCanadaLabFees.Count>0) { //Don't worry about deleting child lab fees if we are editing a lab fee. No such concept.
						Procedures.TryDeleteLab(_listCanadaLabFees[0]);
					}
				}
				else { //canadaLabFee1!=0
					if(_listCanadaLabFees.Count>0) { //Retain the old lab code if present.
						Procedure labFee1Old=_listCanadaLabFees[0].Copy();
						_listCanadaLabFees[0].ProcFee=canadaLabFee1;
						Procedures.Update(_listCanadaLabFees[0],labFee1Old);
					}
					else {
						Procedure labFee1=new Procedure();
						labFee1.PatNum=_procCur.PatNum;
						labFee1.ProcDate=_procCur.ProcDate;
						labFee1.ProcFee=canadaLabFee1;
						labFee1.ProcStatus=_procCur.ProcStatus;
						labFee1.ProvNum=_procCur.ProvNum;
						labFee1.DateEntryC=DateTime.Now;
						labFee1.ClinicNum=_procCur.ClinicNum;
						labFee1.ProcNumLab=_procCur.ProcNum;
						labFee1.CodeNum=ProcedureCodes.GetCodeNum("99111");
						//Not sure if Place of Service is required for canadian labs. (I don't see any reason why this would/could/should break anything.)
						labFee1.PlaceService=(PlaceOfService)PrefC.GetInt(PrefName.DefaultProcedurePlaceService);//Default proc place of service for the Practice is used.
						if(labFee1.CodeNum==0) { //Code does not exist.
							ProcedureCode code99111=new ProcedureCode();
							code99111.IsCanadianLab=true;
							code99111.ProcCode="99111";
							code99111.Descript="+L Commercial Laboratory Procedures";
							code99111.AbbrDesc="Lab Fee";
							code99111.ProcCat=Defs.GetByExactNameNeverZero(DefCat.ProcCodeCats,"Adjunctive General Services");
							ProcedureCodes.Insert(code99111);
							labFee1.CodeNum=code99111.CodeNum;
							ProcedureCodes.RefreshCache();
						}
						Procedures.Insert(labFee1);
					}
				}
				double canadaLabFee2=0;
				if(textCanadaLabFee2.Text!="") {
					canadaLabFee2=PIn.Double(textCanadaLabFee2.Text);
				}
				if(canadaLabFee2==0) {
					if(textCanadaLabFee2.Visible && _listCanadaLabFees.Count>1) { //Don't worry about deleting child lab fees if we are editing a lab fee. No such concept.
						Procedures.TryDeleteLab(_listCanadaLabFees[1]);
					}
				}
				else { //canadaLabFee2!=0
					if(_listCanadaLabFees.Count>1) { //Retain the old lab code if present.
						Procedure labFee2Old=_listCanadaLabFees[1].Copy();
						_listCanadaLabFees[1].ProcFee=canadaLabFee2;
						Procedures.Update(_listCanadaLabFees[1],labFee2Old);
					}
					else {
						Procedure labFee2=new Procedure();
						labFee2.PatNum=_procCur.PatNum;
						labFee2.ProcDate=_procCur.ProcDate;
						labFee2.ProcFee=canadaLabFee2;
						labFee2.ProcStatus=_procCur.ProcStatus;
						labFee2.ProvNum=_procCur.ProvNum;
						labFee2.DateEntryC=DateTime.Now;
						labFee2.ClinicNum=_procCur.ClinicNum;
						labFee2.ProcNumLab=_procCur.ProcNum;
						labFee2.CodeNum=ProcedureCodes.GetCodeNum("99111");
						//Not sure if Place of Service is required for canadian labs. (I don't see any reason why this would/could/should break anything.)
						labFee2.PlaceService=(PlaceOfService)PrefC.GetInt(PrefName.DefaultProcedurePlaceService);//Default proc place of service for the Practice is used.
						if(labFee2.CodeNum==0) { //Code does not exist.
							ProcedureCode code99111=new ProcedureCode();
							code99111.IsCanadianLab=true;
							code99111.ProcCode="99111";
							code99111.Descript="+L Commercial Laboratory Procedures";
							code99111.AbbrDesc="Lab Fee";
							code99111.ProcCat=Defs.GetByExactNameNeverZero(DefCat.ProcCodeCats,"Adjunctive General Services");
							ProcedureCodes.Insert(code99111);
							labFee2.CodeNum=code99111.CodeNum;
							ProcedureCodes.RefreshCache();
						}
						Procedures.Insert(labFee2);
					}
				}
			}
			else {
				if(_procedureCode2.IsProsth) {
					switch(listProsth.SelectedIndex) {
						case 0:
							_procCur.Prosthesis="";
							break;
						case 1:
							_procCur.Prosthesis="I";
							break;
						case 2:
							_procCur.Prosthesis="R";
							break;
					}
					_procCur.DateOriginalProsth=PIn.Date(textDateOriginalProsth.Text);
					_procCur.IsDateProsthEst=checkIsDateProsthEst.Checked;
				}
				else {
					_procCur.Prosthesis="";
					_procCur.DateOriginalProsth=DateTime.MinValue;
					_procCur.IsDateProsthEst=false;
				}
			}
			_procCur.ClaimNote=textClaimNote.Text;
			//Last chance to run this code before Proc gets updated.
			if(Programs.UsingOrion){//Ask for an explanation. If they hit cancel here, return and don't save.
				_orionProcCur.DPC=(OrionDPC)comboDPC.SelectedIndex;
				_orionProcCur.DPCpost=(OrionDPC)comboDPCpost.SelectedIndex;
				_orionProcCur.DateScheduleBy=PIn.Date(textDateScheduled.Text);
				_orionProcCur.DateStopClock=PIn.Date(textDateStop.Text);
				_orionProcCur.IsOnCall=checkIsOnCall.Checked;
				_orionProcCur.IsEffectiveComm=checkIsEffComm.Checked;
				_orionProcCur.IsRepair=checkIsRepair.Checked;
				if(IsNew) {
					OrionProcs.Insert(_orionProcCur);
				}
				else {//Is not new.
					if(FormProcEditExplain.GetChanges(_procCur,_procOld,_orionProcCur,_orionProcOld)!="") {//Checks if any changes were made. Also sets static variable Changes.
						//If a day old and the orion procedure status did not go from TP to C, CS or CR, then show explaination window.
						if((_procOld.DateTP.Date<MiscData.GetNowDateTime().Date &&
							(_orionProcOld.Status2!=OrionStatus.TP || (_orionProcCur.Status2!=OrionStatus.C && _orionProcCur.Status2!=OrionStatus.CS && _orionProcCur.Status2!=OrionStatus.CR))))
						{
							FormProcEditExplain FormP=new FormProcEditExplain();
							FormP.dpcChange=((int)_orionProcOld.DPC!=(int)_orionProcCur.DPC);
							if(FormP.ShowDialog()!=DialogResult.OK) {
								return;
							}
							Procedure ProcPreExplain=_procOld.Copy();
							_procOld.Note=FormProcEditExplain.Explanation;
							Procedures.Update(_procOld,ProcPreExplain);
							Thread.Sleep(1100);
						}
					}
					OrionProcs.Update(_orionProcCur);
					//Date entry needs to be updated when status changes to cancelled or refused and at least a day old.
					if(_procOld.DateTP.Date<MiscData.GetNowDateTime().Date &&
						_orionProcCur.Status2==OrionStatus.CA_EPRD ||
						_orionProcCur.Status2==OrionStatus.CA_PD ||
						_orionProcCur.Status2==OrionStatus.CA_Tx ||
						_orionProcCur.Status2==OrionStatus.R) 
					{
						_procCur.DateEntryC=MiscData.GetNowDateTime().Date;
					}
				}//End of "is not new."
			}
			if(_procCur.ProvNum!=_procOld.ProvNum 
				&& _procCur.ProcFee==_procOld.ProcFee)
			{
				string promptText="";
				ProcFeeHelper procFeeHelper=new ProcFeeHelper(_patCur,ListFees,_listPatPlans,_listInsSubs,_listInsPlans,_listBenefits);
				bool isUpdatingFee=Procedures.ShouldFeesChange(new List<Procedure>() { _procCur.Copy() },new List<Procedure>() { _procOld.Copy() },
					ref promptText,procFeeHelper);
				if(isUpdatingFee) {//Made it past the pref check.
					if(promptText!="" && !MsgBox.Show(MsgBoxButtons.YesNo,promptText)) {
							isUpdatingFee=false;
					}
					if(isUpdatingFee) {
						_procCur.ProcFee=Procedures.GetProcFee(_patCur,_listPatPlans,_listInsSubs,_listInsPlans,_procCur.CodeNum,_procCur.ProvNum,
							_procCur.ClinicNum,_procCur.MedicalCode,_listBenefits,listFees:ListFees);
					}
				}
			}
			//Autocodes----------------------------------------------------------------------------------------------------------------------------------------
			Permissions perm=Permissions.ProcComplEdit;
			if(_procCur.ProcStatus.In(ProcStat.EC,ProcStat.EO)) {
				perm=Permissions.ProcExistingEdit;
			}
			if(!_procOld.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)
				|| Security.IsAuthorized(perm,_procCur.ProcDate,true)) {
				//Only check auto codes if the procedure is not complete or the user has permission to edit completed procedures.
				long verifyCode;
				bool isMandibular=(listBoxTeeth.SelectedIndices.Count < 1);
				if(AutoCodeItems.ShouldPromptForCodeChange(_procCur,_procedureCode2,_patCur,isMandibular,_listClaimProcsForProc,out verifyCode)) {
					FormAutoCodeLessIntrusive FormACLI=new FormAutoCodeLessIntrusive(_patCur,_procCur,_procedureCode2,verifyCode,_listPatPlans,_listInsSubs,_listInsPlans,
						_listBenefits,_listClaimProcsForProc,listBoxTeeth.Text);
					if(FormACLI.ShowDialog() != DialogResult.OK
						&& PrefC.GetBool(PrefName.ProcEditRequireAutoCodes)) 
					{
						return;//send user back to fix information or use suggested auto code.
					}
					_procCur=FormACLI.Proc;
					_listClaimProcsForProc=ClaimProcs.RefreshForProc(_procCur.ProcNum);//FormAutoCodeLessIntrusive may have added claimprocs.
				}
			}
			//OrthoCase-------------------------------------------------------------------------------------------------------------------------
			//If proc is set complete and orthocases are on, check if we need to link it to an ortho case.
			if(OrthoCases.HasOrthoCasesEnabled() && _procOld.ProcStatus!=ProcStat.C && _procCur.ProcStatus==ProcStat.C) {
				_orthoProcLink=OrthoProcLinks.LinkProcForActiveOrthoCase(_procCur);//Updates the _procCur.ProcFee for in memory object.
			}
			else if(_orthoProcLink!=null && _procOld.ProcDate!=_procCur.ProcDate) {
				OrthoCases.UpdateDatesByLinkedProc(_orthoProcLink,_procCur);
			}
			bool isProcLinkedToOrthoCase=_orthoProcLink!=null;
			//The actual update----------------------------------------------------------------------------------------------------------------------------------
			Procedures.FormProcEditUpdate(_procCur,_procOld,_procedureCode2,isProcLinkedToOrthoCase,IsNew,listBoxTeeth.Text);
			_listClaimProcsForProc.ForEach(x => x.ClinicNum=comboClinic.SelectedClinicNum);//These changes save in Form_Closing ComputeEstimates depending on DialogResult
			//Recall synch---------------------------------------------------------------------------------------------------------------------------------
			Recalls.Synch(_procCur.PatNum);
			if(_procOld.ProcStatus!=ProcStat.C && _procCur.ProcStatus==ProcStat.C) {
				List<string> procCodeList=new List<string>();
				procCodeList.Add(ProcedureCodes.GetStringProcCode(_procCur.CodeNum));
				AutomationL.Trigger(AutomationTrigger.CompleteProcedure,procCodeList,_procCur.PatNum);
			}
			DialogResult=DialogResult.OK;
			//it is assumed that we will do an immediate refresh after closing this window.
		}

		private void butChangeUser_Click(object sender,EventArgs e) {
			FormLogOn FormChangeUser=new FormLogOn(isSimpleSwitch:true);
			FormChangeUser.ShowDialog();
			if(FormChangeUser.DialogResult==DialogResult.OK) { //if successful
				_curUser=FormChangeUser.CurUserSimpleSwitch; //assign temp user
				FillComboClinic();
				FillComboProv();
				signatureBoxWrapper.ClearSignature(); //clear sig
				signatureBoxWrapper.UserSig=_curUser;
				textUser.Text=_curUser.UserName; //update user textbox.
				_sigChanged=true;
				_hasUserChanged=true;
			}
		}

		private void SaveSignature(){
			if(_sigChanged){
				string keyData=GetSignatureKey();
				_procCur.Signature=signatureBoxWrapper.GetSignature(keyData);
				_procCur.SigIsTopaz=signatureBoxWrapper.GetSigIsTopaz();
			}
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(!EntriesAreValid()) {
				return;
			}
			if(Programs.UsingOrion) {
				if(_orionProcOld!=null && _orionProcOld.DateStopClock.Year>1880) {
					if(PIn.Date(textDateStop.Text)>_orionProcOld.DateStopClock.Date) {
						MsgBox.Show(this,"Future date not allowed for Date Stop Clock.");
						return;
					}
				}
				else if(PIn.Date(textDateStop.Text)>MiscData.GetNowDateTime().Date) {
					MsgBox.Show(this,"Future date not allowed for Date Stop Clock.");
					return;
				}
				//Strange logic for Orion for setting sched by date to a scheduled date from a previously cancelled TP on the same tooth/surf.
				if(_procCur.Surf!="" || textTooth.Text!="" || textSurfaces.Text!="") {
					DataTable table=OrionProcs.GetCancelledScheduleDateByToothOrSurf(_procCur.PatNum,textTooth.Text.ToString(),_procCur.Surf);
					if(table.Rows.Count>0) {
						if(textDateScheduled.Text!="" && DateTime.Parse(textDateScheduled.Text)>PIn.DateT(table.Rows[0]["DateScheduleBy"].ToString())) {
							//If the cancelled sched by date is in the past then do nothing.
							if(PIn.DateT(table.Rows[0]["DateScheduleBy"].ToString())>MiscData.GetNowDateTime().Date) {
								textDateScheduled.Text=((DateTime)table.Rows[0]["DateScheduleBy"]).ToShortDateString();
								_cancelledScheduleByDate=DateTime.Parse(textDateScheduled.Text);
								MsgBox.Show(this,"Schedule by date cannot be later than: "+textDateScheduled.Text+".");
								return;
							}
						}
					}
				}
				if(comboDPC.SelectedIndex==0) {
					MsgBox.Show(this,"DPC should not be \"Not Specified\".");
					return;
				}
			}
			if(_hasUserChanged && signatureBoxWrapper.SigIsBlank && !MsgBox.Show(this,MsgBoxButtons.OKCancel,
				"The signature box has not been re-signed.  Continuing will remove the previous signature from this procedure.  Exit anyway?")) {
				return;
			}
			SaveAndClose();
			Plugins.HookAddCode(this,"FormProcEdit.butOK_Click_end",_procCur); 
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormProcEdit_FormClosing(object sender,FormClosingEventArgs e) {
			//We need to update the CPOE status even if the user is cancelling out of the window.
			if(Userods.IsUserCpoe(_curUser) && !_procOld.IsCpoe) {
				//There's a possibility that we are making a second, unnecessary call to the database here but it is worth it to help meet EHR measures.
				Procedures.UpdateCpoeForProc(_procCur.ProcNum,true);
				//Make a log that we edited this procedure's CPOE flag.
				SecurityLogs.MakeLogEntry(Permissions.ProcEdit,_procCur.PatNum,ProcedureCodes.GetProcCode(_procCur.CodeNum).ProcCode
					+", "+_procCur.ProcFee.ToString("c")+", "+Lan.g(this,"automatically flagged as CPOE."));
			}
			if(DialogResult==DialogResult.OK) {
				//this catches date,prov,fee,status,etc for all claimProcs attached to this proc.
				if(_startedAttachedToClaim!=Procedures.IsAttachedToClaim(_procCur.ProcNum)) {
					//unless they got attached to a claim while this window was open.  Then it doesn't touch them.
					//We don't want to allow ComputeEstimates to reattach the procedure to the old claim which could have deleted.
					return;
				}
				//Now we have to double check that every single claimproc is attached to the same claim that they were originally attached to.
				if(ClaimProcs.IsAttachedToDifferentClaim(_procCur.ProcNum,_listClaimProcsForProc)) {
					return;//The claimproc is not attached to the same claim it was originally pointing to.  Do not run ComputeEstimates which would point it back to the old (potentially deleted) claim.
				}
				List<ClaimProcHist> histList=ClaimProcs.GetHistList(_procCur.PatNum,_listBenefits,_listPatPlans,_listInsPlans,-1,DateTimeOD.Today,_listInsSubs);
				//We don't want already existing claim procs on this procedure to affect the calculation for this procedure.
				histList.RemoveAll(x => x.ProcNum==_procCur.ProcNum);
				Procedures.ComputeEstimates(_procCur,_patCur.PatNum,ref _listClaimProcsForProc,_isQuickAdd,_listInsPlans,_listPatPlans,_listBenefits,
					histList,new List<ClaimProcHist> { },true,
					_patCur.Age,_listInsSubs,
					null,false,false,ListSubstLinks,false,
					null,LookupFees,_orthoProcLink);
				if(_isEstimateRecompute
					&& _procCur.ProcNumLab!=0//By definition of procedure.ProcNumLab, this will only happen in Canada and if ProcCur is a lab fee.
					&& !Procedures.IsAttachedToClaim(_procCur.ProcNumLab))//If attached to a claim, then user should recreate claim because estimates will be inaccurate not matter what.
				{
					Procedure procParent=Procedures.GetOneProc(_procCur.ProcNumLab,false);
					if(procParent!=null) {//A null parent proc could happen in rare cases for older databases.
						List<ClaimProc> listParentClaimProcs=ClaimProcs.RefreshForProc(procParent.ProcNum);
						Procedures.ComputeEstimates(procParent,_patCur.PatNum,ref listParentClaimProcs,false,_listInsPlans,_listPatPlans,_listBenefits,
							null,null,true,
							_patCur.Age,_listInsSubs,
							null,false,false,ListSubstLinks,false,
							null,LookupFees);
					}
				}
				return;
			}
			if(IsNew) {//if cancelling on a new procedure
				//delete any newly created claimprocs
				for(int i=0;i<_listClaimProcsForProc.Count;i++) {
					//if(ClaimProcsForProc[i].ProcNum==ProcCur.ProcNum) {
					ClaimProcs.Delete(_listClaimProcsForProc[i]);
					//}
				}
			}
		}

		
	}
}
