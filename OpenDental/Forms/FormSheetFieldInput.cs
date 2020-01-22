using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormSheetFieldInput:FormSheetFieldBase {

		public FormSheetFieldInput(SheetDef sheetDef,SheetFieldDef sheetFieldDef,bool isReadOnly,bool isEditMobile=false):base(sheetDef,sheetFieldDef,isReadOnly,isEditMobile) {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetFieldInput_Load(object sender,EventArgs e) {
			labelReportableName.Visible=false;
			textReportableName.Visible=false;
			if(SheetFieldDefCur.FieldName.StartsWith("misc")) {
				labelReportableName.Visible=true;
				textReportableName.Visible=true;
				textReportableName.Text=SheetFieldDefCur.ReportableName;
			}
            if(_isEditMobile) {
                groupBox1.Enabled=false;
                comboGrowthBehavior.Enabled=false;
                textTabOrder.Enabled=false;
            }
            textUiLabelMobile.Visible=SheetDefs.IsMobileAllowed(_sheetDefCur.SheetType);
			labelUiLabelMobile.Visible=SheetDefs.IsMobileAllowed(_sheetDefCur.SheetType);
			//not allowed to change sheettype or fieldtype once created.  So get all avail fields for this sheettype
			AvailFields=SheetFieldsAvailable.GetList(_sheetDefCur.SheetType,OutInCheck.In);
			listFields.Items.Clear();
			for(int i=0;i<AvailFields.Count;i++){
				//static text is not one of the options.
				listFields.Items.Add(AvailFields[i].FieldName);
				if(SheetFieldDefCur.FieldName.StartsWith(AvailFields[i].FieldName)){
					listFields.SelectedIndex=i;
				}
			}
			InstalledFontCollection fColl=new InstalledFontCollection();
			for(int i=0;i<fColl.Families.Length;i++){
				comboFontName.Items.Add(fColl.Families[i].Name);
			}
			comboFontName.Text=SheetFieldDefCur.FontName;
			textFontSize.Text=SheetFieldDefCur.FontSize.ToString();
			checkFontIsBold.Checked=SheetFieldDefCur.FontIsBold;
			SheetUtil.FillComboGrowthBehavior(comboGrowthBehavior,SheetFieldDefCur.GrowthBehavior);
			checkRequired.Checked=SheetFieldDefCur.IsRequired;
			textTabOrder.Text=SheetFieldDefCur.TabOrder.ToString();
			if(!string.IsNullOrEmpty(SheetFieldDefCur.UiLabelMobile)) { //Already has a value that user has setup previously.
				textUiLabelMobile.Text=SheetFieldDefCur.UiLabelMobile;
			}
		}

		private void listFields_DoubleClick(object sender,EventArgs e) {
			OnOk();
		}

		private void listFields_SelectedIndexChanged(object sender,EventArgs e) {
			if(listFields.SelectedIndex==-1) {
				labelReportableName.Visible=false;
				textReportableName.Visible=false;
				textReportableName.Text="";
				return;
			}
			string fieldName=AvailFields[listFields.SelectedIndex].FieldName;
			if(fieldName=="misc") {
				labelReportableName.Visible=true;
				textReportableName.Visible=true;
				textReportableName.Text=SheetFieldDefCur.ReportableName;//will either be "" or saved ReportableName.
				textUiLabelMobile.Text="Misc";
			}
			else {
				labelReportableName.Visible=false;
				textReportableName.Visible=false;
				textReportableName.Text="";
			}
			if(fieldName.StartsWith("inputMed")) {
				int inputMedNum=0;
				if(int.TryParse(fieldName.Replace("inputMed",""),out inputMedNum)) {
					textUiLabelMobile.Text="Input Medication "+inputMedNum.ToString();
				}
			}
			else if(fieldName=="Birthdate") {
				textUiLabelMobile.Text="Birthdate";
			}
			else if(fieldName=="FName") {
				textUiLabelMobile.Text="First Name";
			}
			else if(fieldName=="LName") {
				textUiLabelMobile.Text="Last Name";
			}
			else if(fieldName=="ICEName") {
				textUiLabelMobile.Text="Emergency Contact Name";
			}
			else if(fieldName=="ICEPhone") {
				textUiLabelMobile.Text="Emergency Phone";
			}
			else if(fieldName=="toothNum") {
				textUiLabelMobile.Text="Tooth Number(s)";
			}
		}

		protected override void OnOk(){
            if(!ArePosAndSizeValid()) {
                return;
            }
			if(textTabOrder.errorProvider1.GetError(textTabOrder)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(listFields.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a field name first.");
				return;
			}
			if(comboFontName.Text==""){
				//not going to bother testing for validity unless it will cause a crash.
				MsgBox.Show(this,"Please select a font name first.");
				return;
			}
			if(_sheetDefCur.SheetType==SheetTypeEnum.ExamSheet) {
				if(textReportableName.Text.Contains(";") || textReportableName.Text.Contains(":")) {
					MsgBox.Show(this,"Reportable name for Exam Sheet fields may not contain a ':' or a ';'.");
					return;
				}
			}
			float fontSize;
			try{
				fontSize=float.Parse(textFontSize.Text);
				if(fontSize<2){
					MsgBox.Show(this,"Font size is invalid.");
					return;
				}
			}
			catch{
				MsgBox.Show(this,"Font size is invalid.");
				return;
			}
			SheetFieldDefCur.FieldName=AvailFields[listFields.SelectedIndex].FieldName;
			SheetFieldDefCur.ReportableName=textReportableName.Text;//always safe even if not a misc field or if textReportableName is blank.
			SheetFieldDefCur.FontName=comboFontName.Text;
			SheetFieldDefCur.FontSize=fontSize;
			SheetFieldDefCur.FontIsBold=checkFontIsBold.Checked;
			SheetFieldDefCur.GrowthBehavior=comboGrowthBehavior.GetSelected<GrowthBehaviorEnum>();
			SheetFieldDefCur.IsRequired=checkRequired.Checked;
			SheetFieldDefCur.TabOrder=PIn.Int(textTabOrder.Text);
			SheetFieldDefCur.UiLabelMobile=textUiLabelMobile.Text;
			//don't save to database here.
			SheetFieldDefCur.IsNew=false;
			DialogResult=DialogResult.OK;
		}
	}
}