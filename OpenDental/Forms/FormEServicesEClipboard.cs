using CodeBase;
using Microsoft.Win32;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.Mobile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms; 
using System.Xml;
using System.Globalization;
using System.Data;
using System.Linq;
using System.IO;
using WebServiceSerializer;
using OpenDentBusiness.WebServiceMainHQ;
using OpenDentBusiness.WebTypes.WebSched.TimeSlot;

namespace OpenDental {
	public partial class FormEServicesSetup {
		#region Fields - Private
		private bool _eClipboardAllowEdit;
		///<summary>A list of all eclipboard sheet defs that are edited in this window. Synced with the database list on the ok click.</summary>
		private List<EClipboardSheetDef> _listEClipboardSheets;
		///<summary>Helper to manager prefs relating to eClipboard and getting them to/from the db.</summary>
		private ClinicPrefHelper _clinicPrefHelperEClipboard=new ClinicPrefHelper(
			PrefName.EClipboardUseDefaults,
			PrefName.EClipboardAllowSelfCheckIn,
			PrefName.EClipboardAllowSelfPortraitOnCheckIn,
			PrefName.EClipboardPresentAvailableFormsOnCheckIn,
			PrefName.EClipboardCreateMissingFormsOnCheckIn,
			PrefName.EClipboardPopupKioskOnCheckIn,
			PrefName.EClipboardMessageComplete);

		#endregion Fields - Private

		#region Properties - Private
		///<summary>The current clinic num for this tab, handles whether or not the practice has clinics.</summary>
		private long _clinicNumEClipboardTab {
			get {
				if(!PrefC.HasClinicsEnabled) {
					return 0; //No clinics, HQ clinic
				}
				if(clinicPickerEClipboard==null) {
					return 0; //combobox hasn't loaded yet
				}
				return clinicPickerEClipboard.SelectedClinicNum;
			}
		}
		#endregion Properties - Private

		#region Methods - FormEServices Boilerplate

		private void FillTabEClipboard() {
			//Fill the list of sheets
			_listEClipboardSheets=EClipboardSheetDefs.Refresh();
			//Set Clinics or No
			if(!PrefC.HasClinicsEnabled) {
				checkEClipboardUseDefaults.Visible=false;
			}
			else {
				clinicPickerEClipboard.SelectionChangeCommitted += new System.EventHandler(this.ClinicPickerEClipboard_SelectionChangeCommitted);
			}
			EClipboardSetControlsForClinicPrefs();
			//Subscribe to eclipboard events to fill grid if needed
			EClipboardEvent.Fired+=eClipboardChangedEvent_Fired;
		}
		
		private void AuthorizeTabEClipboard(bool allowEdit) {
			_eClipboardAllowEdit=allowEdit;
			FillGridMobileAppDevices();
			FillGridEClipboardSheetInUse();
			SetUIEClipboardEnabled();
			groupEClipboardRules.Enabled=allowEdit && (_clinicNumEClipboardTab==0 || !checkEClipboardUseDefaults.Checked);
		}

		private bool IsTabValidEClipboard() {
			return true;
		}

		private void SaveTabEClipboard() {
			EClipboardPushPrefs();
			EClipboardSheetDefs.Sync(_listEClipboardSheets,EClipboardSheetDefs.Refresh());
			EClipboardEvent.Fired-=eClipboardChangedEvent_Fired;
		}

		#endregion Methods - FormEServices Boilerplate

		#region Methods - Private
		private void EClipboardPushPrefs() {
			//First get all of the clinic nums that have preference changes
			List<long> listClinicNumsWithChanges=_clinicPrefHelperEClipboard.GetClinicsWithChanges();
			if(listClinicNumsWithChanges.Contains(0)) {
				//Default clinic has changed so find any clinics that used defaults. They have changed by association.
				listClinicNumsWithChanges.AddRange(
					//The default option only appears for users that don't have restricted clinic access, so start with the list of all non-hidden clinics
					Clinics.GetDeepCopy(isShort:true)
					//Filter to only those clinics that currently use defaults (they have no rules of their own).
					.Where(x => ClinicPrefs.GetBool(PrefName.EClipboardUseDefaults,x.ClinicNum))
					//Only need the clinicNums.
					.Select(x => x.ClinicNum)
				);
			}
			//Save to db.  Form's _doSetInvalidClinicPrefs will trigger signals.
			if(_clinicPrefHelperEClipboard.SyncAllPrefs()) {
				_doSetInvalidClinicPrefs=true;
			}
			//Push all of the new preference values to the clinic nums that have preference changes
			foreach(long clinicNum in listClinicNumsWithChanges.Distinct()) {
				OpenDentBusiness.WebTypes.PushNotificationUtils.CI_NewEClipboardPrefs(clinicNum);
			}
		}

		///<summary>Doesn't touch the Defaults checkbox itself.  Sets the 5 checkboxes and the textbox that are involved in prefs.  Importantly, it also sends those same default values to the list of clinicprefs in memory where they will later be synched.</summary>
		private void EClipboardSetControlsToPrefDefaults(){
			//EClipboardUseDefaults not included here
			checkEClipboardAllowCheckIn.Checked=_clinicPrefHelperEClipboard.GetDefaultBoolVal(PrefName.EClipboardAllowSelfCheckIn);
			checkEClipboardAllowSelfPortrait.Checked=_clinicPrefHelperEClipboard.GetDefaultBoolVal(PrefName.EClipboardAllowSelfPortraitOnCheckIn);
			checkEClipboardAllowSheets.Checked=_clinicPrefHelperEClipboard.GetDefaultBoolVal(PrefName.EClipboardPresentAvailableFormsOnCheckIn);
			checkEClipboardCreateMissingForms.Checked=_clinicPrefHelperEClipboard.GetDefaultBoolVal(PrefName.EClipboardCreateMissingFormsOnCheckIn);
			checkEClipboardPopupKiosk.Checked=_clinicPrefHelperEClipboard.GetDefaultBoolVal(PrefName.EClipboardPopupKioskOnCheckIn);
			textEClipboardMessage.Text=_clinicPrefHelperEClipboard.GetDefaultStringVal(PrefName.EClipboardMessageComplete);
			_clinicPrefHelperEClipboard.ValChangedByUser(PrefName.EClipboardAllowSelfCheckIn,clinicPickerEClipboard.SelectedClinicNum,
				POut.Bool(checkEClipboardAllowCheckIn.Checked));
			_clinicPrefHelperEClipboard.ValChangedByUser(PrefName.EClipboardAllowSelfPortraitOnCheckIn,clinicPickerEClipboard.SelectedClinicNum,
				POut.Bool(checkEClipboardAllowSelfPortrait.Checked));
			_clinicPrefHelperEClipboard.ValChangedByUser(PrefName.EClipboardPresentAvailableFormsOnCheckIn,clinicPickerEClipboard.SelectedClinicNum,
				POut.Bool(checkEClipboardAllowSheets.Checked));
			_clinicPrefHelperEClipboard.ValChangedByUser(PrefName.EClipboardCreateMissingFormsOnCheckIn,clinicPickerEClipboard.SelectedClinicNum,
				POut.Bool(checkEClipboardCreateMissingForms.Checked));
			_clinicPrefHelperEClipboard.ValChangedByUser(PrefName.EClipboardPopupKioskOnCheckIn,clinicPickerEClipboard.SelectedClinicNum,
				POut.Bool(checkEClipboardPopupKiosk.Checked));
			_clinicPrefHelperEClipboard.ValChangedByUser(PrefName.EClipboardMessageComplete,clinicPickerEClipboard.SelectedClinicNum,
				textEClipboardMessage.Text);
		}

		///<summary>Sets the Defaults checkbox itself.  Then sets the 5 other checkboxes and the textbox that are involved in prefs.  Sets them based on the values in the local clinicpref list.  Does not change any of those values.  Called only on startup.</summary>
		private void EClipboardSetControlsForClinicPrefs(){
			long clinicNumSelected=clinicPickerEClipboard.SelectedClinicNum;
			checkEClipboardUseDefaults.Checked=_clinicPrefHelperEClipboard.GetBoolVal(PrefName.EClipboardUseDefaults,clinicNumSelected);
			if(checkEClipboardUseDefaults.Checked) {
				EClipboardSetControlsToPrefDefaults();
			}
			else {
				checkEClipboardAllowCheckIn.Checked=_clinicPrefHelperEClipboard.GetBoolVal(PrefName.EClipboardAllowSelfCheckIn,clinicNumSelected);
				checkEClipboardAllowSelfPortrait.Checked=_clinicPrefHelperEClipboard.GetBoolVal(PrefName.EClipboardAllowSelfPortraitOnCheckIn,clinicNumSelected);
				checkEClipboardAllowSheets.Checked=_clinicPrefHelperEClipboard.GetBoolVal(PrefName.EClipboardPresentAvailableFormsOnCheckIn,clinicNumSelected);
				checkEClipboardCreateMissingForms.Checked=_clinicPrefHelperEClipboard.GetBoolVal(PrefName.EClipboardCreateMissingFormsOnCheckIn,clinicNumSelected);
				checkEClipboardPopupKiosk.Checked=_clinicPrefHelperEClipboard.GetBoolVal(PrefName.EClipboardPopupKioskOnCheckIn,clinicNumSelected);
				textEClipboardMessage.Text=_clinicPrefHelperEClipboard.GetStringVal(PrefName.EClipboardMessageComplete,clinicNumSelected);
			}
		}

		///<summary>Fills listEClipboardSheetsAvailable and small grid to its right.</summary>
		private void FillGridEClipboardSheetInUse() {
			//Fill the list of available sheets with the custom PatientForm and MedicalHist sheet defs
			List<SheetDef> listSheets=new List<SheetDef>();
			listSheets.AddRange(SheetDefs.GetCustomForType(SheetTypeEnum.PatientForm));
			listSheets.AddRange(SheetDefs.GetCustomForType(SheetTypeEnum.MedicalHistory));
			listSheets.AddRange(SheetDefs.GetCustomForType(SheetTypeEnum.Consent));
			//Clear any custom sheet defs that don't have a mobile layout
			listSheets.RemoveAll(x => !x.HasMobileLayout);
			//Get the list of in-memory eclipboard sheets
			long clinicNum=checkEClipboardUseDefaults.Checked?0:_clinicNumEClipboardTab;
			List<EClipboardSheetDef> listClinicSheets=_listEClipboardSheets.FindAll(x => x.ClinicNum==clinicNum);
			//Put the sheets that are not in use to the list of available sheets
			listEClipboardSheetsAvailable.Items.Clear();
			listSheets.RemoveAll(x => listClinicSheets.Select(y => y.SheetDefNum).Contains(x.SheetDefNum));
			foreach(SheetDef sheet in listSheets) {
				listEClipboardSheetsAvailable.Items.Add(new ODBoxItem<SheetDef>(sheet.Description,sheet));
			}
			//Put the sheets that are in use into the grid of sheets in use
			gridEClipboardSheetsInUse.ListGridRows.Clear();
			gridEClipboardSheetsInUse.ListGridColumns.Clear();
			gridEClipboardSheetsInUse.BeginUpdate();
			gridEClipboardSheetsInUse.AddColumn("Sheet Name",120);
			gridEClipboardSheetsInUse.AddColumn("Frequency",60);
			foreach(EClipboardSheetDef sheet in listClinicSheets.OrderBy(x => x.ItemOrder)) {
				gridEClipboardSheetsInUse.AddRow(tag:sheet,SheetDefs.GetDescription(sheet.SheetDefNum),sheet.ResubmitInterval.TotalDays.ToString());
			}
			gridEClipboardSheetsInUse.EndUpdate();
		}

		///<summary>Fills the big main grid.</summary>
		private void FillGridMobileAppDevices() {
			gridMobileAppDevices.BeginUpdate();
			//Columns
			gridMobileAppDevices.ListGridColumns.Clear();
			gridMobileAppDevices.AddColumn("Device Name",0);
			gridMobileAppDevices.AddColumn("Last Attempt",0);
			gridMobileAppDevices.AddColumn("Last Login",0);
			if(PrefC.HasClinicsEnabled) {
				gridMobileAppDevices.AddColumn("Clinic",0);
			}
			gridMobileAppDevices.ListGridColumns.Add(new GridColumn("Enabled",50,HorizontalAlignment.Center));
			if(_eClipboardAllowEdit) {
				gridMobileAppDevices.ListGridColumns.Add(new GridColumn("Delete",45,HorizontalAlignment.Center));
			}
			//Rows
			gridMobileAppDevices.ListGridRows.Clear();
			List<MobileAppDevice> listDevicesToShow=MobileAppDevices.GetForUser(Security.CurUser);
			if(_clinicNumEClipboardTab>0) {
				listDevicesToShow.RemoveAll(x => x.ClinicNum!=_clinicNumEClipboardTab);
			}
			foreach(MobileAppDevice device in listDevicesToShow) {
				GridRow row=new GridRow();
				row.Cells.Add(device.DeviceName+"\r\n("+device.UniqueID+")");
				row.Cells.Add((device.LastAttempt.Year > 1880 ? device.LastAttempt.ToString() : ""));
				row.Cells.Add((device.LastLogin.Year > 1880 ? device.LastLogin.ToString() : ""));
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add((device.ClinicNum==0 ? Clinics.GetPracticeAsClinicZero() : Clinics.GetClinic(device.ClinicNum)).Abbr);
				}
				row.Cells.Add((device.IsAllowed ? "X" : ""));
				if(_eClipboardAllowEdit) {
					#region Delete click handler
					void DeleteClick(object sender,EventArgs e) {
						if(device.PatNum>0) {
							MsgBox.Show("A patient is currently using this device. Please clear the patient from the device using the Kiosk Manager" +
								" or wait until the patient is no longer using the device.");
							return;
						}
						if(!MsgBox.Show(MsgBoxButtons.YesNo,"This will immediately remove the device from the database and all other workstations." +
							" Continue?")) {
							return;
						}
						MobileAppDevices.Delete(device.MobileAppDeviceNum);
						FillGridMobileAppDevices();
					}
					#endregion Delete click handler
					GridCell cell=new GridCell("Delete");
					cell.ColorBackG=Color.LightGray;
					cell.ClickEvent=DeleteClick;
					row.Cells.Add(cell);
				}
				row.Tag=device;
				gridMobileAppDevices.ListGridRows.Add(row);
			}
			gridMobileAppDevices.EndUpdate();
		}

		private void SetEClipboardSheetOrder() {
			if(gridEClipboardSheetsInUse.ListGridRows.Count<1) {
				return;
			}
			int idx=0;
			foreach(EClipboardSheetDef sheetDef in gridEClipboardSheetsInUse.ListGridRows.Select(x => (EClipboardSheetDef)x.Tag)) {
				sheetDef.ItemOrder=idx;
				idx++;
			}
		}

		///<summary>Called when user clicks on use defaults for clinic, AuthorizeTab, clinicPicker.SelectedIndexChanged, and CheckEClipboardCreateMissingForms_Click.  It sets various areas enabled or disabled.  Doesn't change the checked values.</summary>
		private void SetUIEClipboardEnabled() {
			//bool isClinicSignedUp=EClipboardDisplayAsEnabled();
			bool isClinicSignedUp=MobileAppDevices.IsClinicSignedUpForEClipboard(_clinicNumEClipboardTab);
			if(PrefC.HasClinicsEnabled && _clinicNumEClipboardTab==0) {
				isClinicSignedUp=Clinics.GetForUserod(Security.CurUser).Any(x => MobileAppDevices.IsClinicSignedUpForEClipboard(x.ClinicNum));
			}
			bool notUsingDefaults=_clinicNumEClipboardTab==0 || !checkEClipboardUseDefaults.Checked;
			bool enableSheets=checkEClipboardCreateMissingForms.Checked;
			checkEClipboardUseDefaults.Enabled=_clinicNumEClipboardTab!=0 && isClinicSignedUp && _eClipboardAllowEdit;
			groupEClipboardRules.Enabled=isClinicSignedUp && _eClipboardAllowEdit && notUsingDefaults;
			groupEClipboardSheets.Enabled=isClinicSignedUp && _eClipboardAllowEdit && notUsingDefaults && enableSheets;
			gridMobileAppDevices.Enabled=isClinicSignedUp;
			labelEClipboardNotSignedUp.Visible=!isClinicSignedUp;
		}

		private void SwapSheets(bool moveUp) {
			if(gridEClipboardSheetsInUse.SelectedIndices.Count()==0) {
				return;
			}
			int selectedIdx=gridEClipboardSheetsInUse.GetSelectedIndex();
			if(moveUp && selectedIdx==0) {
				return;
			}
			if(!moveUp && selectedIdx==gridEClipboardSheetsInUse.ListGridRows.Count-1) {
				return;
			}
			int swapIdx=selectedIdx+(moveUp?-1:1);
			EClipboardSheetDef selectedSheet=(EClipboardSheetDef)gridEClipboardSheetsInUse.ListGridRows[selectedIdx].Tag;
			EClipboardSheetDef swapSheet=(EClipboardSheetDef)gridEClipboardSheetsInUse.ListGridRows[swapIdx].Tag;
			int selectedItemOrder=selectedSheet.ItemOrder;
			selectedSheet.ItemOrder=swapSheet.ItemOrder;
			swapSheet.ItemOrder=selectedItemOrder;
			FillGridEClipboardSheetInUse();
			gridEClipboardSheetsInUse.SetSelected(swapIdx,setValue:true);
		}

		private void UpdateEClipboardDefaultsIfNeeded(PrefName prefName,string newVal) {
			if(_clinicNumEClipboardTab==0) { //we are making changes to the default
				foreach(ClinicPref cp in _clinicPrefHelperEClipboard.GetWhere(PrefName.EClipboardUseDefaults,"1")) {
					_clinicPrefHelperEClipboard.ValChangedByUser(prefName,cp.ClinicNum,newVal);
				}
			}
		}

		#endregion Methods - Private

		#region Events - Main
		private void ButEClipboardAddSheets_Click(object sender,EventArgs e) {
			FormSheetDefs formSD=new FormSheetDefs(SheetTypeEnum.PatientForm,SheetTypeEnum.MedicalHistory,SheetTypeEnum.Consent);
			formSD.ShowDialog();
			FillGridEClipboardSheetInUse();
		}
		
		private void ButEClipboardRight_Click(object sender,EventArgs e) {
			if(listEClipboardSheetsAvailable.SelectedItems.Count==0) {
				return;
			}
			foreach(SheetDef sheetCur in listEClipboardSheetsAvailable.SelectedItems.Cast<ODBoxItem<SheetDef>>().Select(x => x.Tag)) {
				EClipboardSheetDef eSheet=_listEClipboardSheets.FirstOrDefault(x => x.ClinicNum==_clinicNumEClipboardTab && x.SheetDefNum==sheetCur.SheetDefNum);
				if(eSheet==null) {
					eSheet=new EClipboardSheetDef() {
						SheetDefNum=sheetCur.SheetDefNum,
						ClinicNum=_clinicNumEClipboardTab,
						ResubmitInterval=TimeSpan.FromDays(30),
					};
					_listEClipboardSheets.Add(eSheet);
				}
			}
			FillGridEClipboardSheetInUse();
			SetEClipboardSheetOrder();
		}

		private void ButEClipboardLeft_Click(object sender,EventArgs e) {
			if(gridEClipboardSheetsInUse.SelectedIndices.Count()==0) {
				return;
			}
			//grid selection mode is One so there should be exactly one row here to remove
			_listEClipboardSheets.Remove((EClipboardSheetDef)gridEClipboardSheetsInUse.SelectedGridRows.First().Tag);
			FillGridEClipboardSheetInUse();
			SetEClipboardSheetOrder();
		}

		private void ButEClipboardUp_Click(object sender,EventArgs e) {
			SwapSheets(moveUp:true);
		}

		private void ButEClipboardDown_Click(object sender,EventArgs e) {
			SwapSheets(moveUp:false);
		}

		private void ClinicPickerEClipboard_SelectionChangeCommitted(object sender,EventArgs e) {
			EClipboardSetControlsForClinicPrefs();
			FillGridMobileAppDevices();
			FillGridEClipboardSheetInUse();
			SetUIEClipboardEnabled();
		}

		public void eClipboardChangedEvent_Fired(ODEventArgs e) {
			if(e.EventType!=ODEventType.eClipboard || this.IsDisposed) {
				return;
			}
			FillGridMobileAppDevices();
		}

		private void GridEClipboardSheetsInUse_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			EClipboardSheetDef eSheet=(EClipboardSheetDef)gridEClipboardSheetsInUse.ListGridRows[e.Row].Tag;
			InputBox input=new InputBox("How often should the patient be asked to resubmit this form? (In days, where 0 indicates never.)",eSheet.ResubmitInterval.TotalDays.ToString());
			if(input.ShowDialog()==DialogResult.OK) {
				int days;
				if(!int.TryParse(input.textResult.Text,out days)) {
					MsgBox.Show("Input must be a valid whole number");
					return;
				}
				eSheet.ResubmitInterval=TimeSpan.FromDays(days);
			}
			FillGridEClipboardSheetInUse();
		}

		private void gridMobileAppDevices_CellClick(object sender,ODGridClickEventArgs e) {
			int indexOfEnabledColumn;
			if(PrefC.HasClinicsEnabled){
				indexOfEnabledColumn=4;
			}
			else{
				indexOfEnabledColumn=3;
			}
			
			if(e.Col!=indexOfEnabledColumn) {//They did not select the right column.
				return;
			}
			MobileAppDevice mobileAppDevice=gridMobileAppDevices.SelectedTag<MobileAppDevice>();
			//There is not a tag somehow.
			if(mobileAppDevice==null) {
				return;
			}
			if(mobileAppDevice.IsAllowed){
				if(!MsgBox.Show(MsgBoxButtons.YesNo,"This will immediately make the device unavailable to all other workstations. Continue?")) {
					return;
				}
			}
			mobileAppDevice.IsAllowed=!mobileAppDevice.IsAllowed;//Flip the bit.
			//Update the device because the signal processing of this form isn't friendly to keeping an in-memory list that syncs when the form closes
			MobileAppDevices.Update(mobileAppDevice);
			OpenDentBusiness.WebTypes.PushNotificationUtils.CI_IsAllowedChanged(mobileAppDevice.MobileAppDeviceNum,mobileAppDevice.IsAllowed);
			FillGridMobileAppDevices();	//Fill the grid to show the changes.
		}
		#endregion Events - Main

		#region Events - Prefs Section
		private void CheckEClipboardUseDefaults_Click(object sender, EventArgs e){
			_clinicPrefHelperEClipboard.ValChangedByUser(PrefName.EClipboardUseDefaults,clinicPickerEClipboard.SelectedClinicNum,
				POut.Bool(checkEClipboardUseDefaults.Checked));
			if(checkEClipboardUseDefaults.Checked) {//If set to true, set the behavior rules and sheets to the default
				EClipboardSetControlsToPrefDefaults();
				_listEClipboardSheets.RemoveAll(x => _clinicNumEClipboardTab!=0 && x.ClinicNum==_clinicNumEClipboardTab);
			}
			else{
				//user unchecked the box, but nothing should change.  Other 5 checkboxes just stay with the old default values.  They can edit from here, which would override the defaults.
			}
			//disable the ability to edit rules if applicable
			FillGridEClipboardSheetInUse();
			SetUIEClipboardEnabled();
		}

		private void CheckEClipboardAllowCheckIn_Click(object sender, EventArgs e){
			string newVal=POut.Bool(checkEClipboardAllowCheckIn.Checked);
			_clinicPrefHelperEClipboard.ValChangedByUser(PrefName.EClipboardAllowSelfCheckIn,clinicPickerEClipboard.SelectedClinicNum, newVal);
			UpdateEClipboardDefaultsIfNeeded(PrefName.EClipboardAllowSelfCheckIn,newVal);
		}

		private void CheckEClipboardAllowSelfPortrait_Click(object sender, EventArgs e){
			string newVal=POut.Bool(checkEClipboardAllowSelfPortrait.Checked);
			_clinicPrefHelperEClipboard.ValChangedByUser(PrefName.EClipboardAllowSelfPortraitOnCheckIn,clinicPickerEClipboard.SelectedClinicNum, newVal);
			UpdateEClipboardDefaultsIfNeeded(PrefName.EClipboardAllowSelfPortraitOnCheckIn,newVal);
		}

		private void CheckEClipboardAllowSheets_Click(object sender, EventArgs e){
			string newVal=POut.Bool(checkEClipboardAllowSheets.Checked);
			_clinicPrefHelperEClipboard.ValChangedByUser(PrefName.EClipboardPresentAvailableFormsOnCheckIn,clinicPickerEClipboard.SelectedClinicNum,newVal);
			UpdateEClipboardDefaultsIfNeeded(PrefName.EClipboardPresentAvailableFormsOnCheckIn,newVal);
		}

		private void CheckEClipboardCreateMissingForms_Click(object sender, EventArgs e){
			SetUIEClipboardEnabled();
			string newVal=POut.Bool(checkEClipboardCreateMissingForms.Checked);
			_clinicPrefHelperEClipboard.ValChangedByUser(PrefName.EClipboardCreateMissingFormsOnCheckIn,clinicPickerEClipboard.SelectedClinicNum,newVal);
			UpdateEClipboardDefaultsIfNeeded(PrefName.EClipboardCreateMissingFormsOnCheckIn,newVal);
		}

		private void CheckEClipboardPopupKiosk_Click(object sender, EventArgs e){
			string newVal=POut.Bool(checkEClipboardPopupKiosk.Checked);
			_clinicPrefHelperEClipboard.ValChangedByUser(PrefName.EClipboardPopupKioskOnCheckIn,clinicPickerEClipboard.SelectedClinicNum,newVal);
			UpdateEClipboardDefaultsIfNeeded(PrefName.EClipboardPopupKioskOnCheckIn,newVal);
		}

		private void TextEClipboardMessage_TextChanged(object sender, EventArgs e){
			string newVal=textEClipboardMessage.Text;
			_clinicPrefHelperEClipboard.ValChangedByUser(PrefName.EClipboardMessageComplete,clinicPickerEClipboard.SelectedClinicNum,newVal);
			UpdateEClipboardDefaultsIfNeeded(PrefName.EClipboardMessageComplete,newVal);
		}
		#endregion Events - Prefs Section

		

		
		
		
				
		

	

		

		

		

		
	}
}
