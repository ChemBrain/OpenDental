using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSheetFieldOutput:FormSheetFieldBase {

		public FormSheetFieldOutput(SheetDef sheetDef,SheetFieldDef sheetFieldDef,bool isReadOnly,bool isEditMobile=false): base(sheetDef,sheetFieldDef,isReadOnly,isEditMobile) {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetFieldDefEdit_Load(object sender,EventArgs e) {
			if(_isEditMobile) {
				comboGrowthBehavior.Enabled=false;
				groupBox1.Enabled=false;
			}
			textUiLabelMobile.Visible=SheetDefs.IsMobileAllowed(_sheetDefCur.SheetType);
			labelUiLabelMobile.Visible=SheetDefs.IsMobileAllowed(_sheetDefCur.SheetType);
			//not allowed to change sheettype or fieldtype once created.  So get all avail fields for this sheettype
			AvailFields=SheetFieldsAvailable.GetList(_sheetDefCur.SheetType,OutInCheck.Out);
			listFields.Items.Clear();
			for(int i=0;i<AvailFields.Count;i++){
				//static text is not one of the options.
				listFields.Items.Add(AvailFields[i].FieldName);
				if(SheetFieldDefCur.FieldName==AvailFields[i].FieldName){
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
			comboTextAlign.Items.AddEnums<HorizontalAlignment>();
			comboTextAlign.SetSelected((int)SheetFieldDefCur.TextAlign);
			checkIsLocked.Checked=SheetFieldDefCur.IsNew ? true : SheetFieldDefCur.IsLocked;
			butColor.BackColor=SheetFieldDefCur.ItemColor;
			if(!string.IsNullOrEmpty(SheetFieldDefCur.UiLabelMobile)) { //Already has a value that user has setup previously.
				textUiLabelMobile.Text=SheetFieldDefCur.UiLabelMobile;
			}
		}

		private void listFields_SelectedIndexChanged(object sender,EventArgs e) {
			string fieldName=AvailFields[listFields.SelectedIndex].FieldName;
			if(fieldName=="dateTime.Today") {
				textUiLabelMobile.Text="Date";
			}
			else if(fieldName=="patient.nameFL") {
				textUiLabelMobile.Text="Full Name";
			}
		}

		private void listFields_DoubleClick(object sender,EventArgs e) {
			OnOk();
		}

		private void butColor_Click(object sender,EventArgs e) {
			ColorDialog colorDialog1=new ColorDialog();
			colorDialog1.Color=butColor.BackColor;
			colorDialog1.ShowDialog();
			butColor.BackColor=colorDialog1.Color;
		}

        protected override void OnOk() {
            if(!ArePosAndSizeValid()) {
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
			SheetFieldDefCur.FontName=comboFontName.Text;
			SheetFieldDefCur.FontSize=fontSize;
			SheetFieldDefCur.FontIsBold=checkFontIsBold.Checked;
			SheetFieldDefCur.GrowthBehavior=comboGrowthBehavior.GetSelected<GrowthBehaviorEnum>();
			SheetFieldDefCur.TextAlign=(System.Windows.Forms.HorizontalAlignment)comboTextAlign.SelectedIndex;
			SheetFieldDefCur.ItemColor=butColor.BackColor;
			SheetFieldDefCur.IsLocked=checkIsLocked.Checked;
			SheetFieldDefCur.UiLabelMobile=textUiLabelMobile.Text;
			//don't save to database here.
			SheetFieldDefCur.IsNew=false;
			DialogResult=DialogResult.OK;
		}
	}
}