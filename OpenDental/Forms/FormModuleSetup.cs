using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	public partial class FormModuleSetup:ODForm {
		#region Fields - Private
		private List<Def> listPosAdjTypes;
		private List<Def> listNegAdjTypes;
		private bool _changed;
		private ColorDialog colorDialog;
		///<summary>Used to determine a specific tab to have opened upon load.  Only set via the constructor and only used during load.</summary>
		private int _selectedTab;
		private List<BrokenApptProcedure> _listComboBrokenApptProcOptions=new List<BrokenApptProcedure>();
		private YN _prePayAllowedForTpProcs;
		#endregion Fields - Private

		#region Constructors
		///<summary>Default constructor.  Opens the form with the Appts tab selected.</summary>
		public FormModuleSetup():this(0) {
		}

		///<summary>Opens the form with a specific tab selected.  Currently 0-6 are the only valid values.  Defaults to Appts tab if invalid value passed in. 0=Appts, 1=Family, 2=Account, 3=Treat' Plan, 4=Chart, 5=Images, 6=Manage</summary>
		public FormModuleSetup(int selectedTab) {
			InitializeComponent();
			Lan.F(this);
			if(selectedTab<0 || selectedTab>6) {
				selectedTab=0;//Default to Appts tab.
			}
			_selectedTab=selectedTab;
		}
		#endregion Constructors
		
		#region Events Main
		private void FormModuleSetup_Load(object sender, System.EventArgs e) {
			_changed=false;
			try {//try/catch used to prevent setup form from partially loading and filling controls.  Causes UEs, Example: TimeCardOvertimeFirstDayOfWeek set to -1 because UI control not filled properly.
				FillAppts();
				FillFamily();
				FillAccount();
				FillTreatPlan();
				FillChart();
				FillImages();
				FillManage();
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g(this,"An error has occurred while attempting to load preferences.  Run database maintenance and try again."),ex);
				DialogResult=DialogResult.Abort;
				return;
			}
			//Now that all the tabs are filled, use _selectedTab to open a specific tab that the user is trying to view.
			tabControlMain.SelectedTab=tabControlMain.TabPages[_selectedTab];//Guaranteed to be a valid tab.  Validated in constructor.
			if(PrefC.RandomKeys) {
				groupTreatPlanSort.Visible=false;
			}
			Plugins.HookAddCode(this,"FormModuleSetup.FormModuleSetup_Load_end");
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			//validation is done within each save.
			//One save to db might succeed, and then a subsequent save can fail to validate.  That's ok.
			if(!SaveAppts()
				|| !SaveFamily()
				|| !SaveAccount()
				|| !SaveTreatPlan()
				|| !SaveChart()
				|| !SaveImages()
				|| !SaveManage()				
			){
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormModuleSetup_FormClosing(object sender,FormClosingEventArgs e) {
			if(_changed){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}
		#endregion Events Main

		#region Events - Uncategorized
		//todo: Move all these events into their appropriate region
		private void comboPayPlansVersion_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboPayPlansVersion.SelectedIndex==(int)PayPlanVersions.AgeCreditsAndDebits-1) {//Minus 1 because the enum starts at 1.
				checkHideDueNow.Visible=true;
				checkHideDueNow.Checked=PrefC.GetBool(PrefName.PayPlanHideDueNow);
			}
			else {
				checkHideDueNow.Visible=false;
				checkHideDueNow.Checked=false;
			}
		}

		private void radioTreatPlanSortTooth_Click(object sender,EventArgs e) {
			if(PrefC.GetBool(PrefName.TreatPlanSortByTooth)!=radioTreatPlanSortTooth.Checked) {
				MsgBox.Show(this,"You will need to restart the program for the change to take effect.");
			}
		}

		private void radioTreatPlanSortOrder_Click(object sender,EventArgs e) {
			//Sort by order is a false 
			if(PrefC.GetBool(PrefName.TreatPlanSortByTooth)==radioTreatPlanSortOrder.Checked) {
				MsgBox.Show(this,"You will need to restart the program for the change to take effect.");
			}
		}

		private void butDiagnosisCode_Click(object sender,EventArgs e) {
			if(checkDxIcdVersion.Checked) {//ICD-10
				FormIcd10s formI=new FormIcd10s();
				formI.IsSelectionMode=true;
				if(formI.ShowDialog()==DialogResult.OK) {
					textICD9DefaultForNewProcs.Text=formI.SelectedIcd10.Icd10Code;
				}
			}
			else {//ICD-9
				FormIcd9s formI=new FormIcd9s();
				formI.IsSelectionMode=true;
				if(formI.ShowDialog()==DialogResult.OK) {
					textICD9DefaultForNewProcs.Text=formI.SelectedIcd9.ICD9Code;
				}
			}
		}

		private void checkDxIcdVersion_Click(object sender,EventArgs e) {
			SetIcdLabels();
		}

		private void checkFrequency_Click(object sender,EventArgs e) {
			textInsBW.Enabled=checkFrequency.Checked;
			textInsPano.Enabled=checkFrequency.Checked;
			textInsExam.Enabled=checkFrequency.Checked;
			textInsCancerScreen.Enabled=checkFrequency.Checked;
			textInsProphy.Enabled=checkFrequency.Checked;
			textInsFlouride.Enabled=checkFrequency.Checked;
			textInsSealant.Enabled=checkFrequency.Checked;
			textInsCrown.Enabled=checkFrequency.Checked;
			textInsSRP.Enabled=checkFrequency.Checked;
			textInsDebridement.Enabled=checkFrequency.Checked;
			textInsPerioMaint.Enabled=checkFrequency.Checked;
			textInsDentures.Enabled=checkFrequency.Checked;
			textInsImplant.Enabled=checkFrequency.Checked;
		}

		private void butReplacements_Click(object sender,EventArgs e) {
			FormMessageReplacements form=new FormMessageReplacements(MessageReplaceType.Patient);
			form.IsSelectionMode=true;
			form.ShowDialog();
			if(form.DialogResult!=DialogResult.OK) {
				return;
			}
			textClaimIdentifier.Focus();
			int cursorIndex=textClaimIdentifier.SelectionStart;
			textClaimIdentifier.Text=textClaimIdentifier.Text.Insert(cursorIndex,form.Replacement);
			textClaimIdentifier.SelectionStart=cursorIndex+form.Replacement.Length;
		}

		private void butBadDebt_Click(object sender,EventArgs e) {
			string[] arrayDefNums=PrefC.GetString(PrefName.BadDebtAdjustmentTypes).Split(new char[] {','}); //comma-delimited list.
			List<long> listBadAdjDefNums = new List<long>();
			foreach(string strDefNum in arrayDefNums) {
				listBadAdjDefNums.Add(PIn.Long(strDefNum));
			}
			List<Def> listBadAdjDefs=Defs.GetDefs(DefCat.AdjTypes,listBadAdjDefNums);
			FormDefinitionPicker FormDP = new FormDefinitionPicker(DefCat.AdjTypes,listBadAdjDefs);
			FormDP.HasShowHiddenOption=true;
			FormDP.IsMultiSelectionMode=true;
			FormDP.ShowDialog();
			if(FormDP.DialogResult==DialogResult.OK) {
				FillListboxBadDebt(FormDP.ListSelectedDefs);
				string strListBadDebtAdjTypes=String.Join(",",FormDP.ListSelectedDefs.Select(x => x.DefNum));
				Prefs.UpdateString(PrefName.BadDebtAdjustmentTypes,strListBadDebtAdjTypes);
			}
		}
		
		private void checkRecurringChargesAutomated_CheckedChanged(object sender,EventArgs e) {
			labelRecurringChargesAutomatedTime.Enabled=checkRecurringChargesAutomated.Checked;
			textRecurringChargesTime.Enabled=checkRecurringChargesAutomated.Checked;
		}

		private void checkRepeatingChargesAutomated_CheckedChanged(object sender,EventArgs e) {
			labelRepeatingChargesAutomatedTime.Enabled=checkRepeatingChargesAutomated.Checked;
			textRepeatingChargesAutomatedTime.Enabled=checkRepeatingChargesAutomated.Checked;
		}

		///<summary>Turning on automated repeating charges, but recurring charges are also enabled and set to run before auto repeating charges.  Prompt user that this is unadvisable.</summary>
		private void PromptRecurringRepeatingChargesTimes(object sender,EventArgs e) {
			if(checkRepeatingChargesAutomated.Checked && checkRecurringChargesAutomated.Checked
				&& PIn.DateT(textRepeatingChargesAutomatedTime.Text).TimeOfDay>=PIn.DateT(textRecurringChargesTime.Text).TimeOfDay)
			{
				MsgBox.Show(this,"Recurring charges run time is currently set before Repeating charges run time.\r\nConsider setting repeating charges to "
					+"automatically run before recurring charges.");
			}
		}
		#endregion Events - Uncategorized

		#region Events - Appts
		private void butApptLineColor_Click(object sender,EventArgs e) {
			colorDialog.Color=butColor.BackColor;//Pre-select current pref color
			if(colorDialog.ShowDialog()==DialogResult.OK) {
				butApptLineColor.BackColor=colorDialog.Color;
			}
		}

		private void butColor_Click(object sender,EventArgs e) {
			colorDialog.Color=butColor.BackColor;//Pre-select current pref color
			if(colorDialog.ShowDialog()==DialogResult.OK) {
				butColor.BackColor=colorDialog.Color;
			}
		}

		private void checkAppointmentTimeIsLocked_MouseUp(object sender,MouseEventArgs e) {
			if(checkAppointmentTimeIsLocked.Checked) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Would you like to lock appointment times for all existing appointments?")){
					Appointments.SetAptTimeLocked();
				}
			}
		}

		private void checkApptsRequireProcs_CheckedChanged(object sender,EventArgs e) {
			textApptWithoutProcsDefaultLength.Enabled=(!checkApptsRequireProcs.Checked);
		}
		#endregion Events - Appts

		#region Events - Family
		private void checkAllowedFeeSchedsAutomate_Click(object sender,EventArgs e) {
			if(!checkAllowedFeeSchedsAutomate.Checked){
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Allowed fee schedules will now be set up for all insurance plans that do not already have one.\r\nThe name of each fee schedule will exactly match the name of the carrier.\r\nOnce created, allowed fee schedules can be easily managed from the fee schedules window.\r\nContinue?")){
				checkAllowedFeeSchedsAutomate.Checked=false;
				return;
			}
			Cursor=Cursors.WaitCursor;
			long schedsAdded=InsPlans.GenerateAllowedFeeSchedules();
			Cursor=Cursors.Default;
			MessageBox.Show(Lan.g(this,"Done.  Allowed fee schedules added: ")+schedsAdded.ToString());
			DataValid.SetInvalid(InvalidType.FeeScheds);
		}

		private void checkInsDefaultAssignmentOfBenefits_Click(object sender,EventArgs e) {
			//Users with Setup permission are always allowed to change the Checked property of this check box.
			//However, there is a second step when changing the value that can only be performed by users with the InsPlanChangeAssign permission.
			if(!Security.IsAuthorized(Permissions.InsPlanChangeAssign,true)) {
				return;
			}
			string promptMsg=Lan.g(this,"Would you like to immediately change all plans to use assignment of benefits?\r\n"
					+$"Warning: This will update all existing plans to render payment to the provider on all future claims.");
			if(!checkInsDefaultAssignmentOfBenefits.Checked) {
				promptMsg=Lan.g(this,"Would you like to immediately change all plans to use assignment of benefits?\r\n"
					+$"Warning: This will update all existing plans to render payment to the patient on all future claims.");
			}
			if(MessageBox.Show(promptMsg,Lan.g(this,"Change all plans?"),MessageBoxButtons.YesNo)==DialogResult.No) {
				return;
			}
			long subsAffected=InsSubs.SetAllSubsAssignBen(checkInsDefaultAssignmentOfBenefits.Checked);
			SecurityLogs.MakeLogEntry(Permissions.InsPlanChangeAssign,0
				,Lan.g(this,"The following count of plan(s) had their assignment of benefits updated in the Family tab in Module Preferences:")+" "+POut.Long(subsAffected)
			);
			MessageBox.Show(Lan.g(this,"Plans affected:")+" "+POut.Long(subsAffected));
		}

		private void checkInsDefaultShowUCRonClaims_Click(object sender,EventArgs e) {
			if(!checkInsDefaultShowUCRonClaims.Checked) {
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Would you like to immediately change all plans to show office UCR fees on claims?")) {
				return;
			}
			long plansAffected=InsPlans.SetAllPlansToShowUCR();
			MessageBox.Show(Lan.g(this,"Plans affected: ")+plansAffected.ToString());
		}

		private void comboCobRule_SelectionChangeCommitted(object sender,EventArgs e) {
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Would you like to change the COB rule for all existing insurance plans?")) {
				InsPlans.UpdateCobRuleForAll((EnumCobRule)comboCobRule.SelectedIndex);
			}
		}
		#endregion Events - Family

		#region Events - Account
		private void CheckAllowPrePayToTpProcs_Click(object sender,EventArgs e) {
			if(checkAllowPrePayToTpProcs.Checked) {
				checkIsRefundable.Visible=true;
				checkIsRefundable.Checked=PrefC.GetBool(PrefName.TpPrePayIsNonRefundable);
				_prePayAllowedForTpProcs=YN.Yes;
			}
			else {
				checkIsRefundable.Visible=false;
				checkIsRefundable.Checked=false;
				_prePayAllowedForTpProcs=YN.No;
			}
		}

		private void CheckShowFamilyCommByDefault_Click(object sender,EventArgs e) {
			MsgBox.Show(this,"You will need to restart the program for the change to take effect.");
		}

		#endregion Events - Account

		#region Events - Treat' Plan
		private void checkTreatPlanItemized_Click(object sender,EventArgs e) {
			if(Prefs.UpdateBool(PrefName.TreatPlanItemized,checkTreatPlanItemized.Checked)) {
				_changed=true;
			}
		}
		#endregion Events - Treat' Plan

		#region Events - Chart
		private void butProblemsIndicateNone_Click(object sender,EventArgs e) {
			FormDiseaseDefs formD=new FormDiseaseDefs();
			formD.IsSelectionMode=true;
			formD.ShowDialog();
			if(formD.DialogResult!=DialogResult.OK) {
				return;
			}
			//the list should only ever contain one item.
			if(Prefs.UpdateLong(PrefName.ProblemsIndicateNone,formD.ListSelectedDiseaseDefs[0].DiseaseDefNum)) {
				_changed=true;
			}
			textProblemsIndicateNone.Text=formD.ListSelectedDiseaseDefs[0].DiseaseName;
		}

		private void butMedicationsIndicateNone_Click(object sender,EventArgs e) {
			FormMedications formM=new FormMedications();
			formM.IsSelectionMode=true;
			formM.ShowDialog();
			if(formM.DialogResult!=DialogResult.OK) {
				return;
			}
			if(Prefs.UpdateLong(PrefName.MedicationsIndicateNone,formM.SelectedMedicationNum)) {
				_changed=true;
			}
			textMedicationsIndicateNone.Text=Medications.GetDescription(formM.SelectedMedicationNum);
		}

		private void butAllergiesIndicateNone_Click(object sender,EventArgs e) {
			FormAllergySetup formA=new FormAllergySetup();
			formA.IsSelectionMode=true;
			formA.ShowDialog();
			if(formA.DialogResult!=DialogResult.OK) {
				return;
			}
			if(Prefs.UpdateLong(PrefName.AllergiesIndicateNone,formA.SelectedAllergyDefNum)) {
				_changed=true;
			}
			textAllergiesIndicateNone.Text=AllergyDefs.GetOne(formA.SelectedAllergyDefNum).Description;
		}

		private void checkChartNonPatientWarn_Click(object sender,EventArgs e) {
			if(Prefs.UpdateBool(PrefName.ChartNonPatientWarn,checkChartNonPatientWarn.Checked)) {
				_changed=true;
			}
		}

		private void checkProcLockingIsAllowed_Click(object sender,EventArgs e) {
			if(checkProcLockingIsAllowed.Checked) {//if user is checking box			
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This option is not normally used, because all notes are already locked internally, and all changes to notes are viewable in the audit mode of the Chart module.  This option is only for offices that insist on locking each procedure and only allowing notes to be appended.  Using this option, there really is no way to unlock a procedure, regardless of security permission.  So locked procedures can instead be marked as invalid in the case of mistakes.  But it's a hassle to mark procedures invalid, and they also cause clutter.  This option can be turned off later, but locked procedures will remain locked.\r\n\r\nContinue anyway?")) {
					checkProcLockingIsAllowed.Checked=false;
				}
			}
			else {//unchecking box
				MsgBox.Show(this,"Turning off this option will not affect any procedures that are already locked or invalidated.");
			}
		}

		private void checkClaimProcsAllowEstimatesOnCompl_CheckedChanged(object sender,EventArgs e) {
			if(checkClaimProcsAllowEstimatesOnCompl.Checked) {//user is attempting to Allow Estimates to be created for backdated complete procedures
				InputBox inputBox=new InputBox("Please enter password");
				inputBox.ShowDialog();
				if(inputBox.DialogResult!=DialogResult.OK) {
					checkClaimProcsAllowEstimatesOnCompl.Checked=false;
					return;
				}
				if(inputBox.textResult.Text!="abracadabra") {//To prevent unaware users from clicking this box
					checkClaimProcsAllowEstimatesOnCompl.Checked=false;
					MsgBox.Show(this,"Wrong password");
					return;
				}
			}
		}

		#endregion Events - Chart

		#region Events - Images

		#endregion Events - Images

		#region Events - Manage

		#endregion Events - Manage
		
		#region Methods - Appts
		private void FillAppts(){
			BrokenApptProcedure brokenApptCodeDB=(BrokenApptProcedure)PrefC.GetInt(PrefName.BrokenApptProcedure);
			foreach(BrokenApptProcedure option in Enum.GetValues(typeof(BrokenApptProcedure))) {
				if(option==BrokenApptProcedure.Missed && !ProcedureCodes.HasMissedCode()) {
					continue;
				}
				if(option==BrokenApptProcedure.Cancelled && !ProcedureCodes.HasCancelledCode()) {
					continue;
				}
				if(option==BrokenApptProcedure.Both && (!ProcedureCodes.HasMissedCode() || !ProcedureCodes.HasCancelledCode())) {
					continue;
				}
				_listComboBrokenApptProcOptions.Add(option);
				int index=comboBrokenApptProc.Items.Add(Lans.g(this,option.ToString()));
				if(option==brokenApptCodeDB) {
					comboBrokenApptProc.SelectedIndex=index;
				}
			}
			if(comboBrokenApptProc.Items.Count==1) {//None
				comboBrokenApptProc.SelectedIndex=0;
				comboBrokenApptProc.Enabled=false;
			}
			checkSolidBlockouts.Checked=PrefC.GetBool(PrefName.SolidBlockouts);
			checkBrokenApptAdjustment.Checked=PrefC.GetBool(PrefName.BrokenApptAdjustment);
			checkBrokenApptCommLog.Checked=PrefC.GetBool(PrefName.BrokenApptCommLog);
			//checkBrokenApptNote.Checked=PrefC.GetBool(PrefName.BrokenApptCommLogNotAdjustment);
			checkApptBubbleDelay.Checked = PrefC.GetBool(PrefName.ApptBubbleDelay);
			checkAppointmentBubblesDisabled.Checked=PrefC.GetBool(PrefName.AppointmentBubblesDisabled);
			listPosAdjTypes=Defs.GetPositiveAdjTypes();
			listNegAdjTypes=Defs.GetNegativeAdjTypes();
			comboBrokenApptAdjType.Items.AddDefs(listPosAdjTypes);
			comboBrokenApptAdjType.SetSelectedDefNum(PrefC.GetLong(PrefName.BrokenAppointmentAdjustmentType));
			comboProcDiscountType.Items.AddDefs(listNegAdjTypes);
			comboProcDiscountType.SetSelectedDefNum(PrefC.GetLong(PrefName.TreatPlanDiscountAdjustmentType));
			textDiscountPercentage.Text=PrefC.GetDouble(PrefName.TreatPlanDiscountPercent).ToString();
			checkApptExclamation.Checked=PrefC.GetBool(PrefName.ApptExclamationShowForUnsentIns);
			comboTimeArrived.Items.AddDefNone();
			comboTimeArrived.SelectedIndex=0;
			comboTimeArrived.Items.AddDefs(Defs.GetDefsForCategory(DefCat.ApptConfirmed,true));
			comboTimeArrived.SetSelectedDefNum(PrefC.GetLong(PrefName.AppointmentTimeArrivedTrigger));
			comboTimeSeated.Items.AddDefNone();
			comboTimeSeated.SelectedIndex=0;
			comboTimeSeated.Items.AddDefs(Defs.GetDefsForCategory(DefCat.ApptConfirmed,true));
			comboTimeSeated.SetSelectedDefNum(PrefC.GetLong(PrefName.AppointmentTimeSeatedTrigger));
			comboTimeDismissed.Items.AddDefNone();
			comboTimeDismissed.SelectedIndex=0;
			comboTimeDismissed.Items.AddDefs(Defs.GetDefsForCategory(DefCat.ApptConfirmed,true));
			comboTimeDismissed.SetSelectedDefNum(PrefC.GetLong(PrefName.AppointmentTimeDismissedTrigger));
			checkApptRefreshEveryMinute.Checked=PrefC.GetBool(PrefName.ApptModuleRefreshesEveryMinute);
			foreach(SearchBehaviorCriteria searchBehavior in Enum.GetValues(typeof(SearchBehaviorCriteria))) {
				comboSearchBehavior.Items.Add(Lan.g(this,searchBehavior.GetDescription()));
			}
			ODBoxItem<double> comboItem;
			for(int i=0;i<11;i++) {
				double seconds=(double)i/10;
				if(i==0) {
					comboItem = new ODBoxItem<double>(Lan.g(this,"No delay"),seconds);
				}
				else {
					comboItem = new ODBoxItem<double>(seconds.ToString("f1") + " "+Lan.g(this,"seconds"),seconds);
				}
				comboDelay.Items.Add(comboItem);
				if(PrefC.GetDouble(PrefName.FormClickDelay,doUseEnUSFormat:true)==seconds) {
					comboDelay.SelectedIndex = i;
				}
			}
			comboSearchBehavior.SelectedIndex=PrefC.GetInt(PrefName.AppointmentSearchBehavior);
			checkAppointmentTimeIsLocked.Checked=PrefC.GetBool(PrefName.AppointmentTimeIsLocked);
			textApptBubNoteLength.Text=PrefC.GetInt(PrefName.AppointmentBubblesNoteLength).ToString();
			checkWaitingRoomFilterByView.Checked=PrefC.GetBool(PrefName.WaitingRoomFilterByView);
			textWaitRoomWarn.Text=PrefC.GetInt(PrefName.WaitingRoomAlertTime).ToString();
			butColor.BackColor=PrefC.GetColor(PrefName.WaitingRoomAlertColor);
			butApptLineColor.BackColor=PrefC.GetColor(PrefName.AppointmentTimeLineColor);
			checkApptModuleDefaultToWeek.Checked=PrefC.GetBool(PrefName.ApptModuleDefaultToWeek);
			checkApptTimeReset.Checked=PrefC.GetBool(PrefName.AppointmentClinicTimeReset);
			if(!PrefC.HasClinicsEnabled) {
				checkApptTimeReset.Visible=false;
			}
			checkApptModuleAdjInProd.Checked=PrefC.GetBool(PrefName.ApptModuleAdjustmentsInProd);
			checkUseOpHygProv.Checked=PrefC.GetBool(PrefName.ApptSecondaryProviderConsiderOpOnly);
			checkApptModuleProductionUsesOps.Checked=PrefC.GetBool(PrefName.ApptModuleProductionUsesOps);
			checkApptsRequireProcs.Checked=PrefC.GetBool(PrefName.ApptsRequireProc);
			checkApptAllowFutureComplete.Checked=PrefC.GetBool(PrefName.ApptAllowFutureComplete);
			checkApptAllowEmptyComplete.Checked=PrefC.GetBool(PrefName.ApptAllowEmptyComplete);
			comboApptSchedEnforceSpecialty.Items.AddRange(Enum.GetValues(typeof(ApptSchedEnforceSpecialty)).OfType<ApptSchedEnforceSpecialty>()
				.Select(x => x.GetDescription()).ToArray());
			comboApptSchedEnforceSpecialty.SelectedIndex=PrefC.GetInt(PrefName.ApptSchedEnforceSpecialty);
			if(!PrefC.HasClinicsEnabled) {
				comboApptSchedEnforceSpecialty.Visible=false;
				labelApptSchedEnforceSpecialty.Visible=false;
			}
			textApptWithoutProcsDefaultLength.Text=PrefC.GetString(PrefName.AppointmentWithoutProcsDefaultLength);
			checkReplaceBlockouts.Checked=PrefC.GetBool(PrefName.ReplaceExistingBlockout);
			checkUnscheduledListNoRecalls.Checked=PrefC.GetBool(PrefName.UnscheduledListNoRecalls);
			textApptAutoRefreshRange.Text=PrefC.GetString(PrefName.ApptAutoRefreshRange);
			checkPreventChangesToComplAppts.Checked=PrefC.GetBool(PrefName.ApptPreventChangesToCompleted);
			checkApptsAllowOverlap.CheckState=PrefC.GetYNCheckState(PrefName.ApptsAllowOverlap);
			textApptFontSize.Text=PrefC.GetString(PrefName.ApptFontSize);
			textApptProvbarWidth.Text=PrefC.GetString(PrefName.ApptProvbarWidth);
			checkBrokenApptRequiredOnMove.Checked=PrefC.GetBool(PrefName.BrokenApptRequiredOnMove);
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveAppts(){
			int noteLength=0;
			if(!int.TryParse(textApptBubNoteLength.Text,out noteLength)) {
				MsgBox.Show(this,"Max appointment note length is invalid. Please enter a valid number to continue.");
				return false;
			}
			if(noteLength<0) {
				MsgBox.Show(this,"Max appointment note length cannot be a negative number.");
				return false;
			}
			int waitingRoomAlertTime=0;
			try {
				waitingRoomAlertTime=PIn.Int(textWaitRoomWarn.Text);
				if(waitingRoomAlertTime<0) {
					throw new ApplicationException("Waiting room time cannot be negative");//User never sees this message.
				}
			}
			catch {
				MsgBox.Show(this,"Waiting room alert time is invalid.");
				return false;
			}
			if(textApptWithoutProcsDefaultLength.errorProvider1.GetError(textApptWithoutProcsDefaultLength)!=""
				| textApptAutoRefreshRange.errorProvider1.GetError(textApptAutoRefreshRange)!="")
			{
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return false;
			}
			float apptFontSize=0;
			if(!float.TryParse(textApptFontSize.Text,out apptFontSize)){
				MsgBox.Show(this,"Appt Font Size invalid.");
				return false;
			}
			if(apptFontSize<1 || apptFontSize>40){
				MsgBox.Show(this,"Appt Font Size must be between 1 and 40.");
				return false;
			}
			if(!textApptProvbarWidth.IsValid) {
				MsgBox.Show(this,"Please fix data errors first.");
				return false;
			}
			_changed|=Prefs.UpdateBool(PrefName.AppointmentBubblesDisabled,checkAppointmentBubblesDisabled.Checked)
				| Prefs.UpdateBool(PrefName.ApptBubbleDelay,checkApptBubbleDelay.Checked)
				| Prefs.UpdateBool(PrefName.SolidBlockouts,checkSolidBlockouts.Checked)
				//| Prefs.UpdateBool(PrefName.BrokenApptCommLogNotAdjustment,checkBrokenApptNote.Checked) //Deprecated
				| Prefs.UpdateBool(PrefName.BrokenApptAdjustment,checkBrokenApptAdjustment.Checked)
				| Prefs.UpdateBool(PrefName.BrokenApptCommLog,checkBrokenApptCommLog.Checked)
				| Prefs.UpdateInt(PrefName.BrokenApptProcedure,(int)_listComboBrokenApptProcOptions[comboBrokenApptProc.SelectedIndex])
				| Prefs.UpdateBool(PrefName.ApptExclamationShowForUnsentIns,checkApptExclamation.Checked)
				| Prefs.UpdateBool(PrefName.ApptModuleRefreshesEveryMinute,checkApptRefreshEveryMinute.Checked)
				| Prefs.UpdateInt(PrefName.AppointmentSearchBehavior,comboSearchBehavior.SelectedIndex)
				| Prefs.UpdateBool(PrefName.AppointmentTimeIsLocked,checkAppointmentTimeIsLocked.Checked)
				| Prefs.UpdateInt(PrefName.AppointmentBubblesNoteLength,noteLength)
				| Prefs.UpdateBool(PrefName.WaitingRoomFilterByView,checkWaitingRoomFilterByView.Checked)
				| Prefs.UpdateInt(PrefName.WaitingRoomAlertTime,waitingRoomAlertTime)
				| Prefs.UpdateInt(PrefName.WaitingRoomAlertColor,butColor.BackColor.ToArgb())
				| Prefs.UpdateInt(PrefName.AppointmentTimeLineColor,butApptLineColor.BackColor.ToArgb())
				| Prefs.UpdateBool(PrefName.ApptModuleDefaultToWeek,checkApptModuleDefaultToWeek.Checked)
				| Prefs.UpdateBool(PrefName.AppointmentClinicTimeReset,checkApptTimeReset.Checked)
				| Prefs.UpdateBool(PrefName.ApptModuleAdjustmentsInProd,checkApptModuleAdjInProd.Checked)
				| Prefs.UpdateBool(PrefName.ApptSecondaryProviderConsiderOpOnly,checkUseOpHygProv.Checked)
				| Prefs.UpdateBool(PrefName.ApptModuleProductionUsesOps,checkApptModuleProductionUsesOps.Checked)
				| Prefs.UpdateBool(PrefName.ApptsRequireProc,checkApptsRequireProcs.Checked)
				| Prefs.UpdateBool(PrefName.ApptAllowFutureComplete,checkApptAllowFutureComplete.Checked)
				| Prefs.UpdateBool(PrefName.ApptAllowEmptyComplete,checkApptAllowEmptyComplete.Checked)
				| Prefs.UpdateInt(PrefName.ApptSchedEnforceSpecialty,comboApptSchedEnforceSpecialty.SelectedIndex)
				| Prefs.UpdateDouble(PrefName.FormClickDelay,((ODBoxItem<double>)comboDelay.Items[comboDelay.SelectedIndex]).Tag)
				| Prefs.UpdateString(PrefName.AppointmentWithoutProcsDefaultLength,textApptWithoutProcsDefaultLength.Text)
				| Prefs.UpdateBool(PrefName.ReplaceExistingBlockout,checkReplaceBlockouts.Checked)
				| Prefs.UpdateBool(PrefName.UnscheduledListNoRecalls,checkUnscheduledListNoRecalls.Checked)
				| Prefs.UpdateString(PrefName.ApptAutoRefreshRange,textApptAutoRefreshRange.Text)
				| Prefs.UpdateBool(PrefName.ApptPreventChangesToCompleted,checkPreventChangesToComplAppts.Checked)
				| Prefs.UpdateYN(PrefName.ApptsAllowOverlap,checkApptsAllowOverlap.CheckState)
				| Prefs.UpdateString(PrefName.ApptFontSize,apptFontSize.ToString())
				| Prefs.UpdateInt(PrefName.ApptProvbarWidth,PIn.Int(textApptProvbarWidth.Text))
				| Prefs.UpdateBool(PrefName.BrokenApptRequiredOnMove,checkBrokenApptRequiredOnMove.Checked);
			
			if(comboBrokenApptAdjType.SelectedIndex!=-1) {
				_changed|=Prefs.UpdateLong(PrefName.BrokenAppointmentAdjustmentType,comboBrokenApptAdjType.GetSelectedDefNum());
			}
			if(comboProcDiscountType.SelectedIndex!=-1) {
				_changed|=Prefs.UpdateLong(PrefName.TreatPlanDiscountAdjustmentType,comboProcDiscountType.GetSelectedDefNum());
			}
			long timeArrivedTrigger=0;
			if(comboTimeArrived.SelectedIndex>0){
				timeArrivedTrigger=comboTimeArrived.GetSelectedDefNum();
			}
			List<string> listTriggerNewNums=new List<string>();
			if(Prefs.UpdateLong(PrefName.AppointmentTimeArrivedTrigger,timeArrivedTrigger)){
				listTriggerNewNums.Add(POut.Long(timeArrivedTrigger));
				_changed=true;
			}
			long timeSeatedTrigger=0;
			if(comboTimeSeated.SelectedIndex>0){
				timeSeatedTrigger=comboTimeSeated.GetSelectedDefNum();
			}
			if(Prefs.UpdateLong(PrefName.AppointmentTimeSeatedTrigger,timeSeatedTrigger)){
				listTriggerNewNums.Add(POut.Long(timeSeatedTrigger));
				_changed=true;
			}
			long timeDismissedTrigger=0;
			if(comboTimeDismissed.SelectedIndex>0){
				timeDismissedTrigger=comboTimeDismissed.GetSelectedDefNum();
			}
			if(Prefs.UpdateLong(PrefName.AppointmentTimeDismissedTrigger,timeDismissedTrigger)){
				listTriggerNewNums.Add(POut.Long(timeDismissedTrigger));
				_changed=true;
			}
			if(listTriggerNewNums.Count>0) {
				//Adds the appointment triggers to the list of confirmation statuses excluded from sending eConfirms and eReminders.
				List<string> listEConfirm=PrefC.GetString(PrefName.ApptConfirmExcludeEConfirm).Split(',')
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Union(listTriggerNewNums).ToList();
				List<string> listESend=PrefC.GetString(PrefName.ApptConfirmExcludeESend).Split(',')
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Union(listTriggerNewNums).ToList();
				List<string> listERemind=PrefC.GetString(PrefName.ApptConfirmExcludeERemind).Split(',')
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Union(listTriggerNewNums).ToList();
				//Update new Value strings in database.  We don't remove the old ones.
				Prefs.UpdateString(PrefName.ApptConfirmExcludeEConfirm,string.Join(",",listEConfirm));
				Prefs.UpdateString(PrefName.ApptConfirmExcludeESend,string.Join(",",listESend));
				Prefs.UpdateString(PrefName.ApptConfirmExcludeERemind,string.Join(",",listERemind));
			}
			return true;
		}
		#endregion Methods - Appts

		#region Methods - Family
		private void FillFamily(){
			checkInsurancePlansShared.Checked=PrefC.GetBool(PrefName.InsurancePlansShared);
			checkPPOpercentage.Checked=PrefC.GetBool(PrefName.InsDefaultPPOpercent);
			checkAllowedFeeSchedsAutomate.Checked=PrefC.GetBool(PrefName.AllowedFeeSchedsAutomate);
			checkCoPayFeeScheduleBlankLikeZero.Checked=PrefC.GetBool(PrefName.CoPay_FeeSchedule_BlankLikeZero);
			checkFixedBenefitBlankLikeZero.Checked=PrefC.GetBool(PrefName.FixedBenefitBlankLikeZero);
			checkInsDefaultShowUCRonClaims.Checked=PrefC.GetBool(PrefName.InsDefaultShowUCRonClaims);
			checkInsDefaultAssignmentOfBenefits.Checked=PrefC.GetBool(PrefName.InsDefaultAssignBen);
			checkInsPPOsecWriteoffs.Checked=PrefC.GetBool(PrefName.InsPPOsecWriteoffs);
			for(int i=0;i<Enum.GetNames(typeof(EnumCobRule)).Length;i++) {
				comboCobRule.Items.Add(Lan.g("enumEnumCobRule",Enum.GetNames(typeof(EnumCobRule))[i]));
			}
			comboCobRule.SelectedIndex=PrefC.GetInt(PrefName.InsDefaultCobRule);
			checkTextMsgOkStatusTreatAsNo.Checked=PrefC.GetBool(PrefName.TextMsgOkStatusTreatAsNo);
			checkFamPhiAccess.Checked=PrefC.GetBool(PrefName.FamPhiAccess);
			checkGoogleAddress.Checked=PrefC.GetBool(PrefName.ShowFeatureGoogleMaps);
			checkSelectProv.Checked=PrefC.GetBool(PrefName.PriProvDefaultToSelectProv);
			if(!PrefC.GetBool(PrefName.ShowFeatureSuperfamilies)) {
				groupBoxSuperFamily.Visible=false;
			}
			else {
				foreach(SortStrategy option in Enum.GetValues(typeof(SortStrategy))) {
					comboSuperFamSort.Items.Add(option.GetDescription());
				}
				comboSuperFamSort.SelectedIndex=PrefC.GetInt(PrefName.SuperFamSortStrategy);
				checkSuperFamSync.Checked=PrefC.GetBool(PrefName.PatientAllSuperFamilySync);
				checkSuperFamAddIns.Checked=PrefC.GetBool(PrefName.SuperFamNewPatAddIns);
				checkSuperFamCloneCreate.Checked=PrefC.GetBool(PrefName.CloneCreateSuperFamily);
			}
			//users should only see the claimsnapshot tab page if they have it set to something other than ClaimCreate.
			//if a user wants to be able to change claimsnapshot settings, the following MySQL statement should be run:
			//UPDATE preference SET ValueString = 'Service'	 WHERE PrefName = 'ClaimSnapshotTriggerType'
			if(PIn.Enum<ClaimSnapshotTrigger>(PrefC.GetString(PrefName.ClaimSnapshotTriggerType),true) == ClaimSnapshotTrigger.ClaimCreate) {
				groupBoxClaimSnapshot.Visible=false;
			}
			foreach(ClaimSnapshotTrigger trigger in Enum.GetValues(typeof(ClaimSnapshotTrigger))) {
				comboClaimSnapshotTrigger.Items.Add(trigger.GetDescription());
			}
			comboClaimSnapshotTrigger.SelectedIndex=(int)PIn.Enum<ClaimSnapshotTrigger>(PrefC.GetString(PrefName.ClaimSnapshotTriggerType),true);
			textClaimSnapshotRunTime.Text=PrefC.GetDateT(PrefName.ClaimSnapshotRunTime).ToShortTimeString();
			checkClaimUseOverrideProcDescript.Checked=PrefC.GetBool(PrefName.ClaimPrintProcChartedDesc);
			checkClaimTrackingRequireError.Checked=PrefC.GetBool(PrefName.ClaimTrackingRequiresError);
			checkPatInitBillingTypeFromPriInsPlan.Checked=PrefC.GetBool(PrefName.PatInitBillingTypeFromPriInsPlan);
			checkPreferredReferrals.Checked=PrefC.GetBool(PrefName.ShowPreferedReferrals);
			checkAutoFillPatEmail.Checked=PrefC.GetBool(PrefName.AddFamilyInheritsEmail);
			if(!PrefC.HasClinicsEnabled) {
				checkAllowPatsAtHQ.Visible=false;
			}
			checkAllowPatsAtHQ.Checked=PrefC.GetBool(PrefName.ClinicAllowPatientsAtHeadquarters);
			checkInsPlanExclusionsUseUCR.Checked=PrefC.GetBool(PrefName.InsPlanUseUcrFeeForExclusions);
			checkInsPlanExclusionsMarkDoNotBill.Checked=PrefC.GetBool(PrefName.InsPlanExclusionsMarkDoNotBillIns);
			checkPatientSSNMasked.Checked=PrefC.GetBool(PrefName.PatientSSNMasked);
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				checkPatientSSNMasked.Text=Lan.g(this,"Mask patient Social Insurance Numbers");
			}
			checkPatientDOBMasked.Checked=PrefC.GetBool(PrefName.PatientDOBMasked);
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveFamily(){
			DateTime claimSnapshotRunTime=DateTime.MinValue;
			if(!DateTime.TryParse(textClaimSnapshotRunTime.Text,out claimSnapshotRunTime)) {
				MsgBox.Show(this,"Service Snapshot Run Time must be a valid time value.");
				return false;
			}
			claimSnapshotRunTime=new DateTime(1881,01,01,claimSnapshotRunTime.Hour,claimSnapshotRunTime.Minute,claimSnapshotRunTime.Second);
			_changed|= Prefs.UpdateBool(PrefName.InsurancePlansShared,checkInsurancePlansShared.Checked)
				| Prefs.UpdateBool(PrefName.InsDefaultPPOpercent,checkPPOpercentage.Checked)
				| Prefs.UpdateBool(PrefName.AllowedFeeSchedsAutomate,checkAllowedFeeSchedsAutomate.Checked)
				| Prefs.UpdateBool(PrefName.CoPay_FeeSchedule_BlankLikeZero,checkCoPayFeeScheduleBlankLikeZero.Checked)
				| Prefs.UpdateBool(PrefName.FixedBenefitBlankLikeZero,checkFixedBenefitBlankLikeZero.Checked)
				| Prefs.UpdateBool(PrefName.InsDefaultShowUCRonClaims,checkInsDefaultShowUCRonClaims.Checked)
				| Prefs.UpdateBool(PrefName.InsDefaultAssignBen,checkInsDefaultAssignmentOfBenefits.Checked)
				| Prefs.UpdateInt(PrefName.InsDefaultCobRule,comboCobRule.SelectedIndex)
				| Prefs.UpdateBool(PrefName.TextMsgOkStatusTreatAsNo,checkTextMsgOkStatusTreatAsNo.Checked)
				| Prefs.UpdateBool(PrefName.FamPhiAccess,checkFamPhiAccess.Checked)
				| Prefs.UpdateBool(PrefName.InsPPOsecWriteoffs,checkInsPPOsecWriteoffs.Checked)
				| Prefs.UpdateBool(PrefName.ShowFeatureGoogleMaps,checkGoogleAddress.Checked)
				| Prefs.UpdateBool(PrefName.PriProvDefaultToSelectProv,checkSelectProv.Checked)
				| Prefs.UpdateInt(PrefName.SuperFamSortStrategy,comboSuperFamSort.SelectedIndex)
				| Prefs.UpdateBool(PrefName.PatientAllSuperFamilySync,checkSuperFamSync.Checked)
				| Prefs.UpdateBool(PrefName.SuperFamNewPatAddIns,checkSuperFamAddIns.Checked)
				| Prefs.UpdateBool(PrefName.CloneCreateSuperFamily,checkSuperFamCloneCreate.Checked)
				| Prefs.UpdateDateT(PrefName.ClaimSnapshotRunTime,claimSnapshotRunTime)
				| Prefs.UpdateBool(PrefName.ClaimPrintProcChartedDesc,checkClaimUseOverrideProcDescript.Checked)
				| Prefs.UpdateBool(PrefName.ClaimTrackingRequiresError,checkClaimTrackingRequireError.Checked)
				| Prefs.UpdateBool(PrefName.PatInitBillingTypeFromPriInsPlan,checkPatInitBillingTypeFromPriInsPlan.Checked)
				| Prefs.UpdateBool(PrefName.ShowPreferedReferrals,checkPreferredReferrals.Checked)
				| Prefs.UpdateBool(PrefName.AddFamilyInheritsEmail,checkAutoFillPatEmail.Checked)
				| Prefs.UpdateBool(PrefName.ClinicAllowPatientsAtHeadquarters,checkAllowPatsAtHQ.Checked)
				| Prefs.UpdateBool(PrefName.InsPlanExclusionsMarkDoNotBillIns,checkInsPlanExclusionsMarkDoNotBill.Checked)
				| Prefs.UpdateBool(PrefName.InsPlanUseUcrFeeForExclusions,checkInsPlanExclusionsUseUCR.Checked)
				| Prefs.UpdateBool(PrefName.PatientSSNMasked,checkPatientSSNMasked.Checked)
				| Prefs.UpdateBool(PrefName.PatientDOBMasked,checkPatientDOBMasked.Checked);
			foreach(ClaimSnapshotTrigger trigger in Enum.GetValues(typeof(ClaimSnapshotTrigger))) {
				if(trigger.GetDescription()==comboClaimSnapshotTrigger.Text) {
					if(Prefs.UpdateString(PrefName.ClaimSnapshotTriggerType,trigger.ToString())) {
						_changed=true;
					}
					break;
				}
			}
			return true;
		}
		#endregion Methods - Family

		#region Methods - Account
		private void FillAccount(){
			#region Pay/Adj Tab
			checkStoreCCTokens.Checked=PrefC.GetBool(PrefName.StoreCCtokens);
			foreach(PayClinicSetting prompt in Enum.GetValues(typeof(PayClinicSetting))) {
				comboPaymentClinicSetting.Items.Add(Lan.g(this,prompt.GetDescription()));
			}
			comboPaymentClinicSetting.SelectedIndex=PrefC.GetInt(PrefName.PaymentClinicSetting);
			checkPaymentsPromptForPayType.Checked=PrefC.GetBool(PrefName.PaymentsPromptForPayType);
			comboUnallocatedSplits.Items.AddDefs(Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType,true));
			comboUnallocatedSplits.SetSelectedDefNum(PrefC.GetLong(PrefName.PrepaymentUnearnedType));
			comboPayPlanAdj.Items.AddDefs(listNegAdjTypes);
			comboPayPlanAdj.SetSelectedDefNum(PrefC.GetLong(PrefName.PayPlanAdjType));
			comboFinanceChargeAdjType.Items.AddDefs(listPosAdjTypes);
			comboFinanceChargeAdjType.SetSelectedDefNum(PrefC.GetLong(PrefName.FinanceChargeAdjustmentType));
			comboBillingChargeAdjType.Items.AddDefs(listPosAdjTypes);
			comboBillingChargeAdjType.SetSelectedDefNum(PrefC.GetLong(PrefName.BillingChargeAdjustmentType));
			comboSalesTaxAdjType.Items.AddDefs(listPosAdjTypes);
			comboSalesTaxAdjType.SetSelectedDefNum(PrefC.GetLong(PrefName.SalesTaxAdjustmentType));
			textTaxPercent.Text=PrefC.GetDouble(PrefName.SalesTaxPercentage).ToString();
			string[] arrayDefNums=PrefC.GetString(PrefName.BadDebtAdjustmentTypes).Split(new char[] {','}); //comma-delimited list.
			List<long> listBadAdjDefNums = new List<long>();
			foreach(string strDefNum in arrayDefNums) {
				listBadAdjDefNums.Add(PIn.Long(strDefNum));
			}
			FillListboxBadDebt(Defs.GetDefs(DefCat.AdjTypes,listBadAdjDefNums));
			checkAllowFutureDebits.Checked=PrefC.GetBool(PrefName.AccountAllowFutureDebits);
			checkAllowEmailCCReceipt.Checked=PrefC.GetBool(PrefName.AllowEmailCCReceipt);
			List<RigorousAccounting> listEnums=Enum.GetValues(typeof(RigorousAccounting)).OfType<RigorousAccounting>().ToList();
			for(int i=0;i<listEnums.Count;i++) {
				comboRigorousAccounting.Items.Add(listEnums[i].GetDescription());
			}
			comboRigorousAccounting.SelectedIndex=PrefC.GetInt(PrefName.RigorousAccounting);
			List<RigorousAdjustments> listAdjEnums=Enum.GetValues(typeof(RigorousAdjustments)).OfType<RigorousAdjustments>().ToList();
			for(int i=0;i<listAdjEnums.Count;i++) {
				comboRigorousAdjustments.Items.Add(listAdjEnums[i].GetDescription());
			}
			comboRigorousAdjustments.SelectedIndex=PrefC.GetInt(PrefName.RigorousAdjustments);
			for(int i = 0;i<Enum.GetNames(typeof(AutoSplitPreference)).Length;i++) {
				comboAutoSplitPref.Items.Add(Lans.g(this,Enum.GetNames(typeof(AutoSplitPreference))[i]));
			}
			comboAutoSplitPref.SelectedIndex=PrefC.GetInt(PrefName.AutoSplitLogic);
			checkHidePaysplits.Checked=PrefC.GetBool(PrefName.PaymentWindowDefaultHideSplits);
			checkPaymentsTransferPatientIncomeOnly.Checked=PrefC.GetBool(PrefName.PaymentsTransferPatientIncomeOnly);
			checkAllowPrepayProvider.Checked=PrefC.GetBool(PrefName.AllowPrepayProvider);
			comboRecurringChargePayType.Items.AddDefNone("("+Lan.g(this,"default")+")");
			comboRecurringChargePayType.Items.AddDefs(Defs.GetDefsForCategory(DefCat.PaymentTypes,true));
			comboRecurringChargePayType.SetSelectedDefNum(PrefC.GetLong(PrefName.RecurringChargesPayTypeCC)); 
			_prePayAllowedForTpProcs=PrefC.GetEnum<YN>(PrefName.PrePayAllowedForTpProcs);
			checkAllowPrePayToTpProcs.Checked=PrefC.GetYN(PrefName.PrePayAllowedForTpProcs);
			checkIsRefundable.Checked=PrefC.GetBool(PrefName.TpPrePayIsNonRefundable);
			checkIsRefundable.Visible=checkAllowPrePayToTpProcs.Checked;//pref will be unchecked if parent gets turned off.
			comboTpUnearnedType.Items.AddDefs(Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType,true));
			comboTpUnearnedType.SetSelectedDefNum(PrefC.GetLong(PrefName.TpUnearnedType));
			#endregion Pay/Adj Tab
			#region Insurance Tab
			checkProviderIncomeShows.Checked=PrefC.GetBool(PrefName.ProviderIncomeTransferShows);
			checkClaimMedTypeIsInstWhenInsPlanIsMedical.Checked=PrefC.GetBool(PrefName.ClaimMedTypeIsInstWhenInsPlanIsMedical);
			checkClaimFormTreatDentSaysSigOnFile.Checked=PrefC.GetBool(PrefName.ClaimFormTreatDentSaysSigOnFile);
			textInsWriteoffDescript.Text=PrefC.GetString(PrefName.InsWriteoffDescript);
			textClaimAttachPath.Text=PrefC.GetString(PrefName.ClaimAttachExportPath);
			checkEclaimsMedicalProvTreatmentAsOrdering.Checked=PrefC.GetBool(PrefName.ClaimMedProvTreatmentAsOrdering);
			checkEclaimsSeparateTreatProv.Checked=PrefC.GetBool(PrefName.EclaimsSeparateTreatProv);
			checkClaimsValidateACN.Checked=PrefC.GetBool(PrefName.ClaimsValidateACN);
			checkAllowProcAdjFromClaim.Checked=PrefC.GetBool(PrefName.AllowProcAdjFromClaim);
			comboClaimCredit.Items.AddRange(Enum.GetNames(typeof(ClaimProcCreditsGreaterThanProcFee)));
			comboClaimCredit.SelectedIndex=PrefC.GetInt(PrefName.ClaimProcAllowCreditsGreaterThanProcFee);
			checkAllowFuturePayments.Checked=PrefC.GetBool(PrefName.AllowFutureInsPayments);
			textClaimIdentifier.Text=PrefC.GetString(PrefName.ClaimIdPrefix);
			foreach(ClaimZeroDollarProcBehavior procBehavior in Enum.GetValues(typeof(ClaimZeroDollarProcBehavior))) {
				comboZeroDollarProcClaimBehavior.Items.Add(Lan.g(this,procBehavior.ToString()));
			}
			comboZeroDollarProcClaimBehavior.SelectedIndex=PrefC.GetInt(PrefName.ClaimZeroDollarProcBehavior);
			checkClaimTrackingExcludeNone.Checked=PrefC.GetBool(PrefName.ClaimTrackingStatusExcludesNone);
			checkInsPayNoWriteoffMoreThanProc.Checked=PrefC.GetBool(PrefName.InsPayNoWriteoffMoreThanProc);
			checkPromptForSecondaryClaim.Checked=PrefC.GetBool(PrefName.PromptForSecondaryClaim);
			checkInsEstRecalcReceived.Checked=PrefC.GetBool(PrefName.InsEstRecalcReceived);
			checkCanadianPpoLabEst.Checked=PrefC.GetBool(PrefName.CanadaCreatePpoLabEst);
			#endregion Insurance Tab
			#region Misc Account Tab
			checkBalancesDontSubtractIns.Checked=PrefC.GetBool(PrefName.BalancesDontSubtractIns);
			checkAgingMonthly.Checked=PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily);
			if(!PrefC.GetBool(PrefName.AgingIsEnterprise)) {//AgingIsEnterprise requires aging to be daily
				checkAgingMonthly.Text=Lan.g(this,"Aging calculated monthly instead of daily");
				checkAgingMonthly.Enabled=true;
			}
			checkAccountShowPaymentNums.Checked=PrefC.GetBool(PrefName.AccountShowPaymentNums);
			checkShowFamilyCommByDefault.Checked=PrefC.GetBool(PrefName.ShowAccountFamilyCommEntries);
			checkRecurChargPriProv.Checked=PrefC.GetBool(PrefName.RecurringChargesUsePriProv);
			checkPpoUseUcr.Checked=PrefC.GetBool(PrefName.InsPpoAlwaysUseUcrFee);
			checkRecurringChargesUseTransDate.Checked=PrefC.GetBool(PrefName.RecurringChargesUseTransDate);
			checkStatementInvoiceGridShowWriteoffs.Checked=PrefC.GetBool(PrefName.InvoicePaymentsGridShowNetProd);
			checkShowAllocateUnearnedPaymentPrompt.Checked=PrefC.GetBool(PrefName.ShowAllocateUnearnedPaymentPrompt);
			checkPayPlansExcludePastActivity.Checked=PrefC.GetBool(PrefName.PayPlansExcludePastActivity);
			checkPayPlansUseSheets.Checked=PrefC.GetBool(PrefName.PayPlansUseSheets);
			checkAllowFutureTrans.Checked=PrefC.GetBool(PrefName.FutureTransDatesAllowed);
			checkAgingProcLifo.CheckState=PrefC.GetYNCheckState(PrefName.AgingProcLifo);
			foreach(PayPlanVersions version in Enum.GetValues(typeof(PayPlanVersions))) {
				comboPayPlansVersion.Items.Add(Lan.g("enumPayPlanVersions",version.GetDescription()));
			}
			comboPayPlansVersion.SelectedIndex=PrefC.GetInt(PrefName.PayPlansVersion) - 1;
			if(comboPayPlansVersion.SelectedIndex==(int)PayPlanVersions.AgeCreditsAndDebits-1) {//Minus 1 because the enum starts at 1.
				checkHideDueNow.Visible=true;
				checkHideDueNow.Checked=PrefC.GetBool(PrefName.PayPlanHideDueNow);
			}
			else {
				checkHideDueNow.Visible=false;
				checkHideDueNow.Checked=false;
			}
			checkRecurringChargesAutomated.Checked=PrefC.GetBool(PrefName.RecurringChargesAutomatedEnabled);
			textRecurringChargesTime.Text=PrefC.GetDateT(PrefName.RecurringChargesAutomatedTime).TimeOfDay.ToShortTimeString();
			checkRecurPatBal0.Checked=PrefC.GetBool(PrefName.RecurringChargesAllowedWhenNoPatBal);
			checkRepeatingChargesRunAging.Checked=PrefC.GetBool(PrefName.RepeatingChargesRunAging);
			checkRepeatingChargesAutomated.Checked=PrefC.GetBool(PrefName.RepeatingChargesAutomated);
			textRepeatingChargesAutomatedTime.Text=PrefC.GetDateT(PrefName.RepeatingChargesAutomatedTime).TimeOfDay.ToShortTimeString();
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {
				checkCanadianPpoLabEst.Visible=false;
			}
			textDynamicPayPlan.Text=PrefC.GetDateT(PrefName.DynamicPayPlanRunTime).TimeOfDay.ToShortTimeString();
			#endregion Misc Account Tab
		}

		private void FillListboxBadDebt(List<Def> listSelectedDefs) {
			listboxBadDebtAdjs.Items.Clear();
			foreach(Def defCur in listSelectedDefs) {
				listboxBadDebtAdjs.Items.Add(defCur.ItemName);
			}
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveAccount(){
			double taxPercent=0;
			if(!double.TryParse(textTaxPercent.Text,out taxPercent)) {
				MsgBox.Show(this,"Sales Tax percent is invalid.  Please enter a valid number to continue.");
				return false;
			}
			if(taxPercent<0) {
				MsgBox.Show(this,"Sales Tax percent cannot be a negative number.");
				return false;
			}
			if(checkRecurringChargesAutomated.Checked 
				&& (string.IsNullOrWhiteSpace(textRecurringChargesTime.Text) || !textRecurringChargesTime.IsValid)) 
			{
				MsgBox.Show(this,"Recurring charge time must be a valid time.");
				return false;
			}
			if(checkRepeatingChargesAutomated.Checked 
				&& (string.IsNullOrWhiteSpace(textRepeatingChargesAutomatedTime.Text) || !textRepeatingChargesAutomatedTime.IsValid)) 
			{
				MsgBox.Show(this,"Repeating charge time must be a valid time.");
				return false;
			}
			if(string.IsNullOrWhiteSpace(textDynamicPayPlan.Text) || !textDynamicPayPlan.IsValid) {
				MsgBox.Show(this,"Dynamic payment plan time must be a valid time.");
				return false;
			}
			_changed|= Prefs.UpdateBool(PrefName.BalancesDontSubtractIns,checkBalancesDontSubtractIns.Checked)
				| Prefs.UpdateBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily,checkAgingMonthly.Checked)
				| Prefs.UpdateBool(PrefName.StoreCCtokens,checkStoreCCTokens.Checked)
				| Prefs.UpdateBool(PrefName.ProviderIncomeTransferShows,checkProviderIncomeShows.Checked)
				| Prefs.UpdateBool(PrefName.ShowAccountFamilyCommEntries,checkShowFamilyCommByDefault.Checked)
				| Prefs.UpdateBool(PrefName.ClaimFormTreatDentSaysSigOnFile,checkClaimFormTreatDentSaysSigOnFile.Checked)
				| Prefs.UpdateString(PrefName.ClaimAttachExportPath,textClaimAttachPath.Text)
				| Prefs.UpdateBool(PrefName.EclaimsSeparateTreatProv,checkEclaimsSeparateTreatProv.Checked)
				| Prefs.UpdateBool(PrefName.ClaimsValidateACN,checkClaimsValidateACN.Checked)
				| Prefs.UpdateBool(PrefName.ClaimMedTypeIsInstWhenInsPlanIsMedical,checkClaimMedTypeIsInstWhenInsPlanIsMedical.Checked)
				| Prefs.UpdateBool(PrefName.AccountShowPaymentNums,checkAccountShowPaymentNums.Checked)
				| Prefs.UpdateBool(PrefName.PayPlansUseSheets,checkPayPlansUseSheets.Checked)
				| Prefs.UpdateBool(PrefName.RecurringChargesUsePriProv,checkRecurChargPriProv.Checked)
				| Prefs.UpdateString(PrefName.InsWriteoffDescript,textInsWriteoffDescript.Text)
				| Prefs.UpdateInt(PrefName.PaymentClinicSetting,comboPaymentClinicSetting.SelectedIndex)
				| Prefs.UpdateBool(PrefName.PaymentsPromptForPayType,checkPaymentsPromptForPayType.Checked)
				| Prefs.UpdateInt(PrefName.PayPlansVersion,comboPayPlansVersion.SelectedIndex+1)
				| Prefs.UpdateBool(PrefName.ClaimMedProvTreatmentAsOrdering,checkEclaimsMedicalProvTreatmentAsOrdering.Checked)
				| Prefs.UpdateBool(PrefName.InsPpoAlwaysUseUcrFee,checkPpoUseUcr.Checked)
				| Prefs.UpdateBool(PrefName.ProcPromptForAutoNote,checkProcsPromptForAutoNote.Checked)
				| Prefs.UpdateDouble(PrefName.SalesTaxPercentage,taxPercent,false)//Do not round this double for Hawaii
				| Prefs.UpdateBool(PrefName.PayPlansExcludePastActivity,checkPayPlansExcludePastActivity.Checked)
				| Prefs.UpdateBool(PrefName.RecurringChargesUseTransDate,checkRecurringChargesUseTransDate.Checked)
				| Prefs.UpdateBool(PrefName.InvoicePaymentsGridShowNetProd,checkStatementInvoiceGridShowWriteoffs.Checked)
				| Prefs.UpdateBool(PrefName.AccountAllowFutureDebits,checkAllowFutureDebits.Checked)
				| Prefs.UpdateBool(PrefName.PayPlanHideDueNow,checkHideDueNow.Checked)
				| Prefs.UpdateBool(PrefName.AllowProcAdjFromClaim,checkAllowProcAdjFromClaim.Checked)
				| Prefs.UpdateInt(PrefName.ClaimProcAllowCreditsGreaterThanProcFee,comboClaimCredit.SelectedIndex)
				| Prefs.UpdateBool(PrefName.AllowEmailCCReceipt,checkAllowEmailCCReceipt.Checked)
				| Prefs.UpdateInt(PrefName.AutoSplitLogic,comboAutoSplitPref.SelectedIndex)
				| Prefs.UpdateString(PrefName.ClaimIdPrefix,textClaimIdentifier.Text);
			int prefRigorousAccounting=PrefC.GetInt(PrefName.RigorousAccounting);
			if(Prefs.UpdateInt(PrefName.RigorousAccounting,comboRigorousAccounting.SelectedIndex)) {
				_changed=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rigorous accounting changed from "+
					((RigorousAccounting)prefRigorousAccounting).GetDescription()+" to "
					+((RigorousAccounting)comboRigorousAccounting.SelectedIndex).GetDescription()+".");
			}
			int prefRigorousAdjustments=PrefC.GetInt(PrefName.RigorousAdjustments);
			if(Prefs.UpdateInt(PrefName.RigorousAdjustments,comboRigorousAdjustments.SelectedIndex)) {
				_changed=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rigorous adjustments changed from "+
					((RigorousAdjustments)prefRigorousAdjustments).GetDescription()+" to "
					+((RigorousAdjustments)comboRigorousAdjustments.SelectedIndex).GetDescription()+".");
			}
				//| Prefs.UpdateBool(PrefName.PayPlanHideDebitsFromAccountModule,checkHidePayPlanDebits.Checked)
			_changed|= Prefs.UpdateBool(PrefName.AllowFutureInsPayments,checkAllowFuturePayments.Checked)
				| Prefs.UpdateBool(PrefName.PaymentWindowDefaultHideSplits,checkHidePaysplits.Checked)
				| Prefs.UpdateBool(PrefName.PaymentsTransferPatientIncomeOnly,checkPaymentsTransferPatientIncomeOnly.Checked)
				| Prefs.UpdateBool(PrefName.ShowAllocateUnearnedPaymentPrompt,checkShowAllocateUnearnedPaymentPrompt.Checked)
				| Prefs.UpdateBool(PrefName.FutureTransDatesAllowed,checkAllowFutureTrans.Checked)
				| Prefs.UpdateYN(PrefName.AgingProcLifo,checkAgingProcLifo.CheckState)
				| Prefs.UpdateBool(PrefName.AllowPrepayProvider,checkAllowPrepayProvider.Checked)
				| Prefs.UpdateBool(PrefName.RecurringChargesAutomatedEnabled,checkRecurringChargesAutomated.Checked)
				| Prefs.UpdateDateT(PrefName.RecurringChargesAutomatedTime,PIn.DateT(textRecurringChargesTime.Text))
				| Prefs.UpdateBool(PrefName.RepeatingChargesAutomated,checkRepeatingChargesAutomated.Checked)
				| Prefs.UpdateDateT(PrefName.RepeatingChargesAutomatedTime,PIn.DateT(textRepeatingChargesAutomatedTime.Text))
				| Prefs.UpdateBool(PrefName.RepeatingChargesRunAging,checkRepeatingChargesRunAging.Checked)
				| Prefs.UpdateInt(PrefName.ClaimZeroDollarProcBehavior,comboZeroDollarProcClaimBehavior.SelectedIndex)
				| Prefs.UpdateBool(PrefName.ClaimTrackingStatusExcludesNone,checkClaimTrackingExcludeNone.Checked)
				| Prefs.UpdateBool(PrefName.InsPayNoWriteoffMoreThanProc,checkInsPayNoWriteoffMoreThanProc.Checked)
				| Prefs.UpdateBool(PrefName.PromptForSecondaryClaim,checkPromptForSecondaryClaim.Checked)
				| Prefs.UpdateBool(PrefName.InsEstRecalcReceived,checkInsEstRecalcReceived.Checked)
				| Prefs.UpdateBool(PrefName.CanadaCreatePpoLabEst,checkCanadianPpoLabEst.Checked)
				| Prefs.UpdateLong(PrefName.RecurringChargesPayTypeCC,comboRecurringChargePayType.GetSelectedDefNum())
				| Prefs.UpdateBool(PrefName.RecurringChargesAllowedWhenNoPatBal,checkRecurPatBal0.Checked)
				| Prefs.UpdateYN(PrefName.PrePayAllowedForTpProcs,_prePayAllowedForTpProcs)
				| Prefs.UpdateLong(PrefName.TpUnearnedType,comboTpUnearnedType.GetSelectedDefNum())
				| Prefs.UpdateBool(PrefName.TpPrePayIsNonRefundable,checkIsRefundable.Checked)
				| Prefs.UpdateDateT(PrefName.DynamicPayPlanRunTime,PIn.DateT(textDynamicPayPlan.Text));
			if(comboFinanceChargeAdjType.SelectedIndex!=-1) {
				_changed|=Prefs.UpdateLong(PrefName.FinanceChargeAdjustmentType,comboFinanceChargeAdjType.GetSelectedDefNum());
			}
			if(comboBillingChargeAdjType.SelectedIndex!=-1) {
				_changed|=Prefs.UpdateLong(PrefName.BillingChargeAdjustmentType,comboBillingChargeAdjType.GetSelectedDefNum());
			}
			if(comboSalesTaxAdjType.SelectedIndex!=-1) {
				_changed|=Prefs.UpdateLong(PrefName.SalesTaxAdjustmentType,comboSalesTaxAdjType.GetSelectedDefNum());
			}
			if(comboPayPlanAdj.SelectedIndex!=-1) {
				_changed|=Prefs.UpdateLong(PrefName.PayPlanAdjType,comboPayPlanAdj.GetSelectedDefNum());
			}
			if(comboUnallocatedSplits.SelectedIndex!=-1) {
				_changed|=Prefs.UpdateLong(PrefName.PrepaymentUnearnedType,comboUnallocatedSplits.GetSelectedDefNum());
			}
			return true;
		}
		#endregion Methods - Account

		#region Methods - Treat' Plan
		private void FillTreatPlan(){
			textTreatNote.Text=PrefC.GetString(PrefName.TreatmentPlanNote);
			checkTreatPlanShowCompleted.Checked=PrefC.GetBool(PrefName.TreatPlanShowCompleted);
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				checkTreatPlanShowCompleted.Visible=false;
			}
			else {
				checkTreatPlanShowCompleted.Checked=PrefC.GetBool(PrefName.TreatPlanShowCompleted);
			}
			checkTreatPlanItemized.Checked=PrefC.GetBool(PrefName.TreatPlanItemized);
			checkTPSaveSigned.Checked=PrefC.GetBool(PrefName.TreatPlanSaveSignedToPdf);
			checkFrequency.Checked=PrefC.GetBool(PrefName.InsChecksFrequency);
			textInsBW.Text=PrefC.GetString(PrefName.InsBenBWCodes);
			textInsPano.Text=PrefC.GetString(PrefName.InsBenPanoCodes);
			textInsExam.Text=PrefC.GetString(PrefName.InsBenExamCodes);
			textInsCancerScreen.Text=PrefC.GetString(PrefName.InsBenCancerScreeningCodes);
			textInsProphy.Text=PrefC.GetString(PrefName.InsBenProphyCodes);
			textInsFlouride.Text=PrefC.GetString(PrefName.InsBenFlourideCodes);
			textInsSealant.Text=PrefC.GetString(PrefName.InsBenSealantCodes);
			textInsCrown.Text=PrefC.GetString(PrefName.InsBenCrownCodes);
			textInsSRP.Text=PrefC.GetString(PrefName.InsBenSRPCodes);
			textInsDebridement.Text=PrefC.GetString(PrefName.InsBenFullDebridementCodes);
			textInsPerioMaint.Text=PrefC.GetString(PrefName.InsBenPerioMaintCodes);
			textInsDentures.Text=PrefC.GetString(PrefName.InsBenDenturesCodes);
			textInsImplant.Text=PrefC.GetString(PrefName.InsBenImplantCodes);
			if(!checkFrequency.Checked) {
				textInsBW.Enabled=false;
				textInsPano.Enabled=false;
				textInsExam.Enabled=false;
				textInsCancerScreen.Enabled=false;
				textInsProphy.Enabled=false;
				textInsFlouride.Enabled=false;
				textInsSealant.Enabled=false;
				textInsCrown.Enabled=false;
				textInsSRP.Enabled=false;
				textInsDebridement.Enabled=false;
				textInsPerioMaint.Enabled=false;
				textInsDentures.Enabled=false;
				textInsImplant.Enabled=false;
			}
			radioTreatPlanSortTooth.Checked=PrefC.GetBool(PrefName.TreatPlanSortByTooth) || PrefC.RandomKeys;
			//Currently, the TreatPlanSortByTooth preference gets overridden by 
			//the RandomPrimaryKeys preferece due to "Order Entered" being based on the ProcNum
			groupTreatPlanSort.Enabled=!PrefC.RandomKeys;
			textInsHistBW.Text=PrefC.GetString(PrefName.InsHistBWCodes);
			textInsHistDebridement.Text=PrefC.GetString(PrefName.InsHistDebridementCodes);
			textInsHistExam.Text=PrefC.GetString(PrefName.InsHistExamCodes);
			textInsHistFMX.Text=PrefC.GetString(PrefName.InsHistPanoCodes);
			textInsHistPerioMaint.Text=PrefC.GetString(PrefName.InsHistPerioMaintCodes);
			textInsHistPerioLL.Text=PrefC.GetString(PrefName.InsHistPerioLLCodes);
			textInsHistPerioLR.Text=PrefC.GetString(PrefName.InsHistPerioLRCodes);
			textInsHistPerioUL.Text=PrefC.GetString(PrefName.InsHistPerioULCodes);
			textInsHistPerioUR.Text=PrefC.GetString(PrefName.InsHistPerioURCodes);
			textInsHistProphy.Text=PrefC.GetString(PrefName.InsHistProphyCodes);
			checkPromptSaveTP.Checked=PrefC.GetBool(PrefName.TreatPlanPromptSave);
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveTreatPlan(){
			float percent=0;
			if(!float.TryParse(textDiscountPercentage.Text,out percent)) {
				MsgBox.Show(this,"Procedure discount percent is invalid. Please enter a valid number to continue.");
				return false;
			}
			if(PrefC.GetString(PrefName.TreatmentPlanNote)!=textTreatNote.Text) {
				List<long> listTreatPlanNums=TreatPlans.GetNumsByNote(PrefC.GetString(PrefName.TreatmentPlanNote));//Find active/inactive TP's that match exactly.
				if(listTreatPlanNums.Count>0) {
					DialogResult dr=MessageBox.Show(Lan.g(this,"Unsaved treatment plans found with default notes")+": "+listTreatPlanNums.Count+"\r\n"
						+Lan.g(this,"Would you like to change them now?"),"",MessageBoxButtons.YesNoCancel);
					switch(dr) {
						case DialogResult.Cancel:
							return false;
						case DialogResult.Yes:
						case DialogResult.OK:
							TreatPlans.UpdateNotes(textTreatNote.Text,listTreatPlanNums);//change tp notes
							break;
						default://includes "No"
							//do nothing
							break;
					}
				}
			}
			_changed|= Prefs.UpdateString(PrefName.TreatmentPlanNote,textTreatNote.Text)
				| Prefs.UpdateBool(PrefName.TreatPlanShowCompleted,checkTreatPlanShowCompleted.Checked)
				| Prefs.UpdateDouble(PrefName.TreatPlanDiscountPercent,percent)
				| Prefs.UpdateBool(PrefName.TreatPlanSaveSignedToPdf,checkTPSaveSigned.Checked)
				| Prefs.UpdateBool(PrefName.InsChecksFrequency,checkFrequency.Checked)
				| Prefs.UpdateString(PrefName.InsBenBWCodes,textInsBW.Text)
				| Prefs.UpdateString(PrefName.InsBenPanoCodes,textInsPano.Text)
				| Prefs.UpdateString(PrefName.InsBenExamCodes,textInsExam.Text)
				| Prefs.UpdateString(PrefName.InsBenCancerScreeningCodes,textInsCancerScreen.Text)
				| Prefs.UpdateString(PrefName.InsBenProphyCodes,textInsProphy.Text)
				| Prefs.UpdateString(PrefName.InsBenFlourideCodes,textInsFlouride.Text)
				| Prefs.UpdateString(PrefName.InsBenSealantCodes,textInsSealant.Text)
				| Prefs.UpdateString(PrefName.InsBenCrownCodes,textInsCrown.Text)
				| Prefs.UpdateString(PrefName.InsBenSRPCodes,textInsSRP.Text)
				| Prefs.UpdateString(PrefName.InsBenFullDebridementCodes,textInsDebridement.Text)
				| Prefs.UpdateString(PrefName.InsBenPerioMaintCodes,textInsPerioMaint.Text)
				| Prefs.UpdateString(PrefName.InsBenDenturesCodes,textInsDentures.Text)
				| Prefs.UpdateString(PrefName.InsBenImplantCodes,textInsImplant.Text)
				| Prefs.UpdateBool(PrefName.TreatPlanSortByTooth,radioTreatPlanSortTooth.Checked || PrefC.RandomKeys)
				| Prefs.UpdateString(PrefName.InsHistBWCodes,textInsHistBW.Text)
				| Prefs.UpdateString(PrefName.InsHistDebridementCodes,textInsHistDebridement.Text)
				| Prefs.UpdateString(PrefName.InsHistExamCodes,textInsHistExam.Text)
				| Prefs.UpdateString(PrefName.InsHistPanoCodes,textInsHistFMX.Text)
				| Prefs.UpdateString(PrefName.InsHistPerioMaintCodes,textInsHistPerioMaint.Text)
				| Prefs.UpdateString(PrefName.InsHistPerioLLCodes,textInsHistPerioLL.Text)
				| Prefs.UpdateString(PrefName.InsHistPerioLRCodes,textInsHistPerioLR.Text)
				| Prefs.UpdateString(PrefName.InsHistPerioULCodes,textInsHistPerioUL.Text)
				| Prefs.UpdateString(PrefName.InsHistPerioURCodes,textInsHistPerioUR.Text)
				| Prefs.UpdateString(PrefName.InsHistProphyCodes,textInsHistProphy.Text)
				| Prefs.UpdateBool(PrefName.TreatPlanPromptSave, checkPromptSaveTP.Checked);
			return true;
		}
		#endregion Methods - Treat' Plan

		#region Methods - Chart
		private void FillChart(){
			comboToothNomenclature.Items.Add(Lan.g(this,"Universal (Common in the US, 1-32)"));
			comboToothNomenclature.Items.Add(Lan.g(this,"FDI Notation (International, 11-48)"));
			comboToothNomenclature.Items.Add(Lan.g(this,"Haderup (Danish)"));
			comboToothNomenclature.Items.Add(Lan.g(this,"Palmer (Ortho)"));
			comboToothNomenclature.SelectedIndex = PrefC.GetInt(PrefName.UseInternationalToothNumbers);
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				labelToothNomenclature.Visible=false;
				comboToothNomenclature.Visible=false;
			}
			checkAutoClearEntryStatus.Checked=PrefC.GetBool(PrefName.AutoResetTPEntryStatus);
			checkAllowSettingProcsComplete.Checked=PrefC.GetBool(PrefName.AllowSettingProcsComplete);
			//checkChartQuickAddHideAmalgam.Checked=PrefC.GetBool(PrefName.ChartQuickAddHideAmalgam); //Deprecated.
			//checkToothChartMoveMenuToRight.Checked=PrefC.GetBool(PrefName.ToothChartMoveMenuToRight);
			textProblemsIndicateNone.Text		=DiseaseDefs.GetName(PrefC.GetLong(PrefName.ProblemsIndicateNone)); //DB maint to fix corruption
			textMedicationsIndicateNone.Text=Medications.GetDescription(PrefC.GetLong(PrefName.MedicationsIndicateNone)); //DB maint to fix corruption
			textAllergiesIndicateNone.Text	=AllergyDefs.GetDescription(PrefC.GetLong(PrefName.AllergiesIndicateNone)); //DB maint to fix corruption
			checkProcGroupNoteDoesAggregate.Checked=PrefC.GetBool(PrefName.ProcGroupNoteDoesAggregate);
			checkChartNonPatientWarn.Checked=PrefC.GetBool(PrefName.ChartNonPatientWarn);
			//checkChartAddProcNoRefreshGrid.Checked=PrefC.GetBool(PrefName.ChartAddProcNoRefreshGrid);//Not implemented.  May revisit some day.
			checkMedicalFeeUsedForNewProcs.Checked=PrefC.GetBool(PrefName.MedicalFeeUsedForNewProcs);
			checkProvColorChart.Checked=PrefC.GetBool(PrefName.UseProviderColorsInChart);
			checkPerioSkipMissingTeeth.Checked=PrefC.GetBool(PrefName.PerioSkipMissingTeeth);
			checkPerioTreatImplantsAsNotMissing.Checked=PrefC.GetBool(PrefName.PerioTreatImplantsAsNotMissing);
			if(PrefC.GetByte(PrefName.DxIcdVersion)==9) {
				checkDxIcdVersion.Checked=false;
			}
			else {//ICD-10
				checkDxIcdVersion.Checked=true;
			}
			SetIcdLabels();
			textICD9DefaultForNewProcs.Text=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
			checkProcLockingIsAllowed.Checked=PrefC.GetBool(PrefName.ProcLockingIsAllowed);
			textMedDefaultStopDays.Text=PrefC.GetString(PrefName.MedDefaultStopDays);
			checkScreeningsUseSheets.Checked=PrefC.GetBool(PrefName.ScreeningsUseSheets);
			checkProcsPromptForAutoNote.Checked=PrefC.GetBool(PrefName.ProcPromptForAutoNote);
			for(int i=0;i<Enum.GetNames(typeof(ProcCodeListSort)).Length;i++) {
				comboProcCodeListSort.Items.Add(Enum.GetNames(typeof(ProcCodeListSort))[i]);
			}
			comboProcCodeListSort.SelectedIndex=PrefC.GetInt(PrefName.ProcCodeListSortOrder);
			checkProcEditRequireAutoCode.Checked=PrefC.GetBool(PrefName.ProcEditRequireAutoCodes);
			checkClaimProcsAllowEstimatesOnCompl.Checked=PrefC.GetBool(PrefName.ClaimProcsAllowedToBackdate);
			checkSignatureAllowDigital.Checked=PrefC.GetBool(PrefName.SignatureAllowDigital);
			checkCommLogAutoSave.Checked=PrefC.GetBool(PrefName.CommLogAutoSave);
			comboProcFeeUpdatePrompt.Items.Add(Lan.g(this,"No prompt, don't change fee"));
			comboProcFeeUpdatePrompt.Items.Add(Lan.g(this,"No prompt, always change fee"));
			comboProcFeeUpdatePrompt.Items.Add(Lan.g(this,"Prompt, when patient portion changes"));
			comboProcFeeUpdatePrompt.Items.Add(Lan.g(this,"Prompt, always"));
			comboProcFeeUpdatePrompt.SelectedIndex=PrefC.GetInt(PrefName.ProcFeeUpdatePrompt);
			checkProcProvChangesCp.Checked=PrefC.GetBool(PrefName.ProcProvChangesClaimProcWithClaim);
			checkBoxRxClinicUseSelected.Checked=PrefC.GetBool(PrefName.ElectronicRxClinicUseSelected);
			if(!PrefC.HasClinicsEnabled) {
				checkBoxRxClinicUseSelected.Visible=false;
			}
			checkProcNoteConcurrencyMerge.Checked=PrefC.GetBool(PrefName.ProcNoteConcurrencyMerge);
			checkIsAlertRadiologyProcsEnabled.Checked=PrefC.GetBool(PrefName.IsAlertRadiologyProcsEnabled);
			checkShowPlannedApptPrompt.Checked=PrefC.GetBool(PrefName.ShowPlannedAppointmentPrompt);
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveChart(){
			int daysStop=0;
			if(!int.TryParse(textMedDefaultStopDays.Text,out daysStop)) {
				MsgBox.Show(this,"Days until medication order stop date entered was is invalid. Please enter a valid number to continue.");
				return false;
			}
			if(daysStop<0) {
				MsgBox.Show(this,"Days until medication order stop date cannot be a negative number.");
				return false;
			}
			_changed|= Prefs.UpdateBool(PrefName.AutoResetTPEntryStatus,checkAutoClearEntryStatus.Checked)
				| Prefs.UpdateBool(PrefName.AllowSettingProcsComplete,checkAllowSettingProcsComplete.Checked)
				| Prefs.UpdateLong(PrefName.UseInternationalToothNumbers,comboToothNomenclature.SelectedIndex)
				| Prefs.UpdateBool(PrefName.ProcGroupNoteDoesAggregate,checkProcGroupNoteDoesAggregate.Checked)
				| Prefs.UpdateBool(PrefName.MedicalFeeUsedForNewProcs,checkMedicalFeeUsedForNewProcs.Checked)
				| Prefs.UpdateByte(PrefName.DxIcdVersion,(byte)(checkDxIcdVersion.Checked?10:9))
				| Prefs.UpdateString(PrefName.ICD9DefaultForNewProcs,textICD9DefaultForNewProcs.Text)
				| Prefs.UpdateBool(PrefName.ProcLockingIsAllowed,checkProcLockingIsAllowed.Checked)
				| Prefs.UpdateInt(PrefName.MedDefaultStopDays,daysStop)
				| Prefs.UpdateBool(PrefName.UseProviderColorsInChart,checkProvColorChart.Checked)
				| Prefs.UpdateBool(PrefName.PerioSkipMissingTeeth,checkPerioSkipMissingTeeth.Checked)
				| Prefs.UpdateBool(PrefName.PerioTreatImplantsAsNotMissing,checkPerioTreatImplantsAsNotMissing.Checked)
				| Prefs.UpdateBool(PrefName.ScreeningsUseSheets,checkScreeningsUseSheets.Checked)
				| Prefs.UpdateInt(PrefName.ProcCodeListSortOrder,comboProcCodeListSort.SelectedIndex)
				| Prefs.UpdateBool(PrefName.ProcEditRequireAutoCodes,checkProcEditRequireAutoCode.Checked)
				| Prefs.UpdateBool(PrefName.ClaimProcsAllowedToBackdate,checkClaimProcsAllowEstimatesOnCompl.Checked)
				| Prefs.UpdateBool(PrefName.SignatureAllowDigital,checkSignatureAllowDigital.Checked)
				| Prefs.UpdateBool(PrefName.CommLogAutoSave,checkCommLogAutoSave.Checked)
				| Prefs.UpdateLong(PrefName.ProcFeeUpdatePrompt,comboProcFeeUpdatePrompt.SelectedIndex)
				//| Prefs.UpdateBool(PrefName.ToothChartMoveMenuToRight,checkToothChartMoveMenuToRight.Checked)
				//| Prefs.UpdateBool(PrefName.ChartQuickAddHideAmalgam, checkChartQuickAddHideAmalgam.Checked) //Deprecated.
				//| Prefs.UpdateBool(PrefName.ChartAddProcNoRefreshGrid,checkChartAddProcNoRefreshGrid.Checked)//Not implemented.  May revisit someday.
				| Prefs.UpdateBool(PrefName.ProcProvChangesClaimProcWithClaim,checkProcProvChangesCp.Checked)
				| Prefs.UpdateBool(PrefName.ElectronicRxClinicUseSelected,checkBoxRxClinicUseSelected.Checked)
				| Prefs.UpdateBool(PrefName.ProcNoteConcurrencyMerge,checkProcNoteConcurrencyMerge.Checked)
				| Prefs.UpdateBool(PrefName.IsAlertRadiologyProcsEnabled,checkIsAlertRadiologyProcsEnabled.Checked)
				| Prefs.UpdateBool(PrefName.ShowPlannedAppointmentPrompt,checkShowPlannedApptPrompt.Checked);
			return true;
		}

		private void SetIcdLabels() {
			byte icdVersion=9;
			if(checkDxIcdVersion.Checked) {
				icdVersion=10;
			}
			labelIcdCodeDefault.Text=Lan.g(this,"Default ICD")+"-"+icdVersion+" "+Lan.g(this,"code for new procedures and when set complete");
		}
		#endregion Methods - Chart

		#region Methods - Images
		private void FillImages(){
			switch(PrefC.GetInt(PrefName.ImagesModuleTreeIsCollapsed)) {
				case 0:
					radioImagesModuleTreeIsExpanded.Checked=true;
					break;
				case 1:
					radioImagesModuleTreeIsCollapsed.Checked=true;
					break;
				case 2:
					radioImagesModuleTreeIsPersistentPerUser.Checked=true;
					break;
			}
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveImages(){
			int imageModuleIsCollapsedVal=0;
			if(radioImagesModuleTreeIsExpanded.Checked) {
				imageModuleIsCollapsedVal=0;
			}
			else if(radioImagesModuleTreeIsCollapsed.Checked) {
				imageModuleIsCollapsedVal=1;
			}
			else if(radioImagesModuleTreeIsPersistentPerUser.Checked) {
				imageModuleIsCollapsedVal=2;
			}
			_changed|= Prefs.UpdateInt(PrefName.ImagesModuleTreeIsCollapsed,imageModuleIsCollapsedVal);
			return true;
		}
		#endregion Methods - Images

		#region Methods - Manage
		private void FillManage(){
			checkRxSendNewToQueue.Checked=PrefC.GetBool(PrefName.RxSendNewToQueue);
			int claimZeroPayRollingDays=PrefC.GetInt(PrefName.ClaimPaymentNoShowZeroDate);
			if(claimZeroPayRollingDays>=0) {
				textClaimsReceivedDays.Text=(claimZeroPayRollingDays+1).ToString();//The minimum value is now 1 ("today"), to match other areas of OD.
			}
			for(int i=0;i<7;i++) {
				comboTimeCardOvertimeFirstDayOfWeek.Items.Add(Lan.g("enumDayOfWeek",Enum.GetNames(typeof(DayOfWeek))[i]));
			}
			comboTimeCardOvertimeFirstDayOfWeek.SelectedIndex=PrefC.GetInt(PrefName.TimeCardOvertimeFirstDayOfWeek);
			checkTimeCardADP.Checked=PrefC.GetBool(PrefName.TimeCardADPExportIncludesName);
			checkClaimsSendWindowValidateOnLoad.Checked=PrefC.GetBool(PrefName.ClaimsSendWindowValidatesOnLoad);
			checkScheduleProvEmpSelectAll.Checked=PrefC.GetBool(PrefName.ScheduleProvEmpSelectAll);
			checkClockEventAllowBreak.Checked=PrefC.GetBool(PrefName.ClockEventAllowBreak);
			checkEraAllowTotalPayment.Checked=PrefC.GetBool(PrefName.EraAllowTotalPayments);
			//Statements
			checkStatementShowReturnAddress.Checked=PrefC.GetBool(PrefName.StatementShowReturnAddress);
			checkStatementShowNotes.Checked=PrefC.GetBool(PrefName.StatementShowNotes);
			checkStatementShowAdjNotes.Checked=PrefC.GetBool(PrefName.StatementShowAdjNotes);
			checkStatementShowProcBreakdown.Checked=PrefC.GetBool(PrefName.StatementShowProcBreakdown);
			comboUseChartNum.Items.Add(Lan.g(this,"PatNum"));
			comboUseChartNum.Items.Add(Lan.g(this,"ChartNumber"));
			if(PrefC.GetBool(PrefName.StatementAccountsUseChartNumber)) {
				comboUseChartNum.SelectedIndex=1;
			}
			else {
				comboUseChartNum.SelectedIndex=0;
			}
			checkStatementsAlphabetically.Checked=PrefC.GetBool(PrefName.PrintStatementsAlphabetically);
			checkStatementsAlphabetically.Visible=PrefC.HasClinicsEnabled;
			if(PrefC.GetLong(PrefName.StatementsCalcDueDate)!=-1) {
				textStatementsCalcDueDate.Text=PrefC.GetLong(PrefName.StatementsCalcDueDate).ToString();
			}
			textPayPlansBillInAdvanceDays.Text=PrefC.GetLong(PrefName.PayPlansBillInAdvanceDays).ToString();
			textBillingElectBatchMax.Text=PrefC.GetInt(PrefName.BillingElectBatchMax).ToString();
			checkIntermingleDefault.Checked=PrefC.GetBool(PrefName.IntermingleFamilyDefault);
			checkBillingShowProgress.Checked=PrefC.GetBool(PrefName.BillingShowSendProgress);
			checkClaimPaymentBatchOnly.Checked=PrefC.GetBool(PrefName.ClaimPaymentBatchOnly);
			checkEraOneClaimPerPage.Checked=PrefC.GetBool(PrefName.EraPrintOneClaimPerPage);
			checkIncludeEraWOPercCoPay.Checked=PrefC.GetBool(PrefName.EraIncludeWOPercCoPay);
			checkShowAutoDeposit.Checked=PrefC.GetBool(PrefName.ShowAutoDeposit);
		}

		///<summary>Returns false if validation fails.</summary>
		private bool SaveManage(){
			if(textStatementsCalcDueDate.errorProvider1.GetError(textStatementsCalcDueDate)!=""
				| textPayPlansBillInAdvanceDays.errorProvider1.GetError(textPayPlansBillInAdvanceDays)!=""
				| textBillingElectBatchMax.errorProvider1.GetError(textBillingElectBatchMax)!="")
			{
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return false;
			}
			if(textClaimsReceivedDays.errorProvider1.GetError(textClaimsReceivedDays)!="") {
				MsgBox.Show(this,"Show claims received after days must be a positive integer or blank.");
				return false;
			}
			_changed|= Prefs.UpdateBool(PrefName.RxSendNewToQueue,checkRxSendNewToQueue.Checked)
				| Prefs.UpdateInt(PrefName.TimeCardOvertimeFirstDayOfWeek,comboTimeCardOvertimeFirstDayOfWeek.SelectedIndex)
				| Prefs.UpdateBool(PrefName.TimeCardADPExportIncludesName,checkTimeCardADP.Checked)
				| Prefs.UpdateBool(PrefName.ClaimsSendWindowValidatesOnLoad,checkClaimsSendWindowValidateOnLoad.Checked)
				| Prefs.UpdateBool(PrefName.StatementShowReturnAddress,checkStatementShowReturnAddress.Checked)
				| Prefs.UpdateBool(PrefName.ScheduleProvEmpSelectAll,checkScheduleProvEmpSelectAll.Checked)
				| Prefs.UpdateBool(PrefName.ClockEventAllowBreak,checkClockEventAllowBreak.Checked)
				| Prefs.UpdateBool(PrefName.EraAllowTotalPayments,checkEraAllowTotalPayment.Checked)
				| Prefs.UpdateBool(PrefName.StatementShowNotes,checkStatementShowNotes.Checked)
				| Prefs.UpdateBool(PrefName.StatementShowAdjNotes,checkStatementShowAdjNotes.Checked)
				| Prefs.UpdateBool(PrefName.StatementShowProcBreakdown,checkStatementShowProcBreakdown.Checked)
				| Prefs.UpdateBool(PrefName.StatementAccountsUseChartNumber,comboUseChartNum.SelectedIndex==1)
				| Prefs.UpdateLong(PrefName.PayPlansBillInAdvanceDays,PIn.Long(textPayPlansBillInAdvanceDays.Text))
				| Prefs.UpdateBool(PrefName.IntermingleFamilyDefault,checkIntermingleDefault.Checked)
				| Prefs.UpdateInt(PrefName.BillingElectBatchMax,PIn.Int(textBillingElectBatchMax.Text))
				| Prefs.UpdateBool(PrefName.BillingShowSendProgress,checkBillingShowProgress.Checked)
				| Prefs.UpdateInt(PrefName.ClaimPaymentNoShowZeroDate,(textClaimsReceivedDays.Text=="")?-1:(PIn.Int(textClaimsReceivedDays.Text)-1))
				| Prefs.UpdateBool(PrefName.ClaimPaymentBatchOnly,checkClaimPaymentBatchOnly.Checked)
				| Prefs.UpdateBool(PrefName.EraPrintOneClaimPerPage,checkEraOneClaimPerPage.Checked)
				| Prefs.UpdateBool(PrefName.EraIncludeWOPercCoPay,checkIncludeEraWOPercCoPay.Checked)
				| Prefs.UpdateBool(PrefName.ShowAutoDeposit,checkShowAutoDeposit.Checked)
				| Prefs.UpdateBool(PrefName.PrintStatementsAlphabetically,checkStatementsAlphabetically.Checked);
			if(textStatementsCalcDueDate.Text==""){
				if(Prefs.UpdateLong(PrefName.StatementsCalcDueDate,-1)){
					_changed=true;
				}
			}
			else{
				if(Prefs.UpdateLong(PrefName.StatementsCalcDueDate,PIn.Long(textStatementsCalcDueDate.Text))){
					_changed=true;
				}
			}
			return true;
		}
		#endregion Methods - Manage

		#region Methods - Other
		protected override string ShowHelpOverride() {
			return tabControlMain.SelectedTab.Name;
		}
		#endregion
	}
}






