using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormAsapSetup:ODForm {
		///<summary>Returns the ClinicNum of the selected clinic. Returns 0 if 'Default' is selected or if clinics are not enabled.</summary>
		private long _selectedClinicNum {
			get {
				if(!PrefC.HasClinicsEnabled) {
					return 0;
				}
				return ((ComboClinicItem)comboClinic.SelectedItem)?.ClinicNum??-1;
			}
		}

		public FormAsapSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormAsapSetup_Load(object sender,EventArgs e) {
			if(PrefC.HasClinicsEnabled) {
				FillClinics();
			}
			else {
				labelClinic.Visible=false;
				comboClinic.Visible=false;
				checkUseDefaults.Visible=false;
			}
			FillPrefs();
		}

		private void FillClinics() {
			comboClinic.Items.Clear();
			List<Clinic> listClinics=Clinics.GetForUserod(Security.CurUser);
			if(!Security.CurUser.ClinicIsRestricted) {
				comboClinic.Items.Add(new ComboClinicItem(Lan.g(this,"Default"),0));
				comboClinic.SelectedIndex=0;
			}
			for(int i=0;i<listClinics.Count;i++) {
				int addedIdx=comboClinic.Items.Add(new ComboClinicItem(listClinics[i].Abbr,listClinics[i].ClinicNum));
				if(listClinics[i].ClinicNum==Clinics.ClinicNum) {
					comboClinic.SelectedIndex=addedIdx;
				}
			}
		}

		private void FillPrefs() {
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col;
			col=new GridColumn(Lan.g(this,"Type"),100);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,""),250);//Random tidbits regarding the template
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Template"),500);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			checkUseDefaults.Checked=true;
			string baseVars=Lan.g(this,"Available variables:")+" [NameF], [Date], [Time], [OfficeName], [OfficePhone]";
			GridRow row;
			row=BuildRowForTemplate(PrefName.ASAPTextTemplate,"Text manual",baseVars);
			gridMain.ListGridRows.Add(row);
			row=BuildRowForTemplate(PrefName.WebSchedAsapTextTemplate,"Web Sched Text",baseVars+", [AsapURL]");
			gridMain.ListGridRows.Add(row);
			row=BuildRowForTemplate(PrefName.WebSchedAsapEmailTemplate,"Web Sched Email Body",baseVars+", [AsapURL]");
			gridMain.ListGridRows.Add(row);
			row=BuildRowForTemplate(PrefName.WebSchedAsapEmailSubj,"Web Sched Email Subject",baseVars);
			gridMain.ListGridRows.Add(row);
			gridMain.EndUpdate();
			if(_selectedClinicNum==0) {
				textWebSchedPerDay.Text=PrefC.GetString(PrefName.WebSchedAsapTextLimit);
				checkUseDefaults.Checked=false;
			}
			else {
				ClinicPref clinicPref=ClinicPrefs.GetPref(PrefName.WebSchedAsapTextLimit,_selectedClinicNum);
				if(clinicPref==null || clinicPref.ValueString==null) {
					textWebSchedPerDay.Text=PrefC.GetString(PrefName.WebSchedAsapTextLimit);
				}
				else {
					textWebSchedPerDay.Text=clinicPref.ValueString;
					checkUseDefaults.Checked=false;
				}
			}
		}

		///<summary>Creates a row for the passed in template type. Unchecks checkUseDefaults if a clinic-level template is in use.</summary>
		private GridRow BuildRowForTemplate(PrefName prefName,string templateName,string availableVars) {
			string templateText;
			bool doShowDefault=false;
			if(_selectedClinicNum==0) {
				templateText=PrefC.GetString(prefName);
				checkUseDefaults.Checked=false;
			}
			else {
				ClinicPref clinicPref=ClinicPrefs.GetPref(prefName,_selectedClinicNum);
				if(clinicPref==null || clinicPref.ValueString==null) {
					templateText=PrefC.GetString(prefName);
					doShowDefault=true;
				}
				else {
					templateText=clinicPref.ValueString;
					checkUseDefaults.Checked=false;
				}
			}
			GridRow row=new GridRow();
			row.Cells.Add(Lan.g(this,templateName)+(doShowDefault ? " "+Lan.g(this,"(Default)") : ""));
			row.Cells.Add(availableVars);
			row.Cells.Add(templateText);
			row.Tag=prefName;
			return row;
		}

		private void comboClinic_SelectedIndexChanged(object sender,EventArgs e) {
			ComboClinicItem selectedItem=(ComboClinicItem)comboClinic.SelectedItem;
			if(selectedItem.ClinicNum==0) {//'Default' is selected.
				checkUseDefaults.Visible=false;
			}
			else {
				checkUseDefaults.Visible=true;
			}
			FillPrefs();
		}

		private void checkUseDefaults_Click(object sender,EventArgs e) {
			List<PrefName> listPrefs=new List<PrefName> {
				PrefName.ASAPTextTemplate,
				PrefName.WebSchedAsapTextTemplate,
				PrefName.WebSchedAsapEmailTemplate,
				PrefName.WebSchedAsapEmailSubj,
				PrefName.WebSchedAsapTextLimit,
			};
			if(checkUseDefaults.Checked) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Delete custom templates for this clinic and switch to using defaults? This cannot be undone.")) {
					ClinicPrefs.DeletePrefs(_selectedClinicNum,listPrefs);
					DataValid.SetInvalid(InvalidType.ClinicPrefs);
				}
				else {
					checkUseDefaults.Checked=false;
				}
			}
			else {//Was checked, now user is unchecking it.
				bool wasChanged=false;
				foreach(PrefName pref in listPrefs) {
					if(ClinicPrefs.Upsert(pref,_selectedClinicNum,PrefC.GetString(pref))) {
						wasChanged=true;
					}
				}
				if(wasChanged) {
					DataValid.SetInvalid(InvalidType.ClinicPrefs);
				}
			}
			FillPrefs();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PrefName prefName=(PrefName)gridMain.ListGridRows[e.Row].Tag;
			string curPrefValue;
			if(_selectedClinicNum==0) {
				curPrefValue=PrefC.GetString(prefName);
			}
			else {
				ClinicPref clinicPref=ClinicPrefs.GetPref(prefName,_selectedClinicNum);
				if(clinicPref==null || string.IsNullOrEmpty(clinicPref.ValueString)) {
					curPrefValue=PrefC.GetString(prefName);
				}
				else {
					curPrefValue=clinicPref.ValueString;
				}
			}
			string newPrefValue;
			bool isHtmlTemplate=prefName==PrefName.WebSchedAsapEmailTemplate;
			if(isHtmlTemplate) {
				FormEmailEdit formEmailEdit=new FormEmailEdit {
					MarkupText=curPrefValue,
					DoCheckForDisclaimer=true,
					IsRawAllowed=false,
				};
				formEmailEdit.ShowDialog();
				if(formEmailEdit.DialogResult!=DialogResult.OK) {
					return;
				}
				newPrefValue=formEmailEdit.MarkupText;
			}
			else {
				FormRecallMessageEdit FormR=new FormRecallMessageEdit(prefName);
				FormR.MessageVal=curPrefValue;
				FormR.ShowDialog();
				if(FormR.DialogResult!=DialogResult.OK) {
					return;
				}
				newPrefValue=FormR.MessageVal;
			}
			if(_selectedClinicNum==0) {
				if(Prefs.UpdateString(prefName,newPrefValue)) {
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			else {
				if(ClinicPrefs.Upsert(prefName,_selectedClinicNum,newPrefValue)) {
					DataValid.SetInvalid(InvalidType.ClinicPrefs);
				}
			}
			FillPrefs();
		}

		private void textWebSchedPerDay_Leave(object sender,EventArgs e) {
			if(!textWebSchedPerDay.IsValid) {
				return;
			}
			if(_selectedClinicNum==0) {
				if(Prefs.UpdateString(PrefName.WebSchedAsapTextLimit,textWebSchedPerDay.Text)) {
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			else {
				if(ClinicPrefs.Upsert(PrefName.WebSchedAsapTextLimit,_selectedClinicNum,textWebSchedPerDay.Text)) {
					DataValid.SetInvalid(InvalidType.ClinicPrefs);
				}
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private class ComboClinicItem {
			public string DisplayName;
			public long ClinicNum;
			public ComboClinicItem(string displayName,long clinicNum) {
				DisplayName=displayName;
				ClinicNum=clinicNum;
			}
			public override string ToString() {
				return DisplayName;
			}
		}

	}
}