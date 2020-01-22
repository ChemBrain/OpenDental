using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSheetFieldComboBox:FormSheetFieldBase {
		private string _selectedOption;

		public FormSheetFieldComboBox(SheetDef sheetDef,SheetFieldDef sheetFieldDef,bool isReadOnly,bool isEditMobile=false): base(sheetDef,sheetFieldDef,isReadOnly,isEditMobile) {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetFieldComboBox_Load(object sender,EventArgs e) {
			if(_isEditMobile) {
				textTabOrder.Enabled=false;
			}
			textUiLabelMobile.Visible=SheetDefs.IsMobileAllowed(_sheetDefCur.SheetType);
			labelUiLabelMobile.Visible=SheetDefs.IsMobileAllowed(_sheetDefCur.SheetType);
			textTabOrder.Text=SheetFieldDefCur.TabOrder.ToString();
			textReportable.Text=SheetFieldDefCur.ReportableName;
			textUiLabelMobile.Text=SheetFieldDefCur.UiLabelMobile;
			if(SheetFieldDefCur.FieldValue!="") {
				_selectedOption=SheetFieldDefCur.FieldValue.Split(';')[0];
				string[] arrayOptions=SheetFieldDefCur.FieldValue.Split(';')[1].Split('|');
				foreach(string option in arrayOptions) {
					if(String.IsNullOrWhiteSpace(option)) {
						continue;
					}
					listboxComboOptions.Items.Add(option);
				}
			}
		}

		private void textOption_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyData!=Keys.Enter) {
				return;
			}
			e.SuppressKeyPress=true;
			butAdd_Click(null,null);//If they press enter on the text, add the text to the listbox.
		}

		private void listComboType_SelectedIndexChanged(object sender,EventArgs e) {
			if(listComboType.SelectedIndex==0) {
				listboxComboOptions.Items.Clear();
				listboxComboOptions.Enabled=true;
				butRemove.Enabled=true;
				butUp.Enabled=true;
				butDown.Enabled=true;
			}
			else {
				if(listComboType.SelectedIndex==1) {//Patient Race
					listboxComboOptions.Items.Clear();
					string[] enumVals=Enum.GetNames(typeof(PatientRaceOld));
					listboxComboOptions.Items.AddRange(enumVals);
				}
				else if(listComboType.SelectedIndex==2) {//Patient Grade
					listboxComboOptions.Items.Clear();
					string[] enumVals=Enum.GetNames(typeof(PatientGrade));
					listboxComboOptions.Items.AddRange(enumVals);
				}
				else if(listComboType.SelectedIndex==3) {//Urgency
					listboxComboOptions.Items.Clear();
					string[] enumVals=Enum.GetNames(typeof(TreatmentUrgency));
					listboxComboOptions.Items.AddRange(enumVals);
				}
				listboxComboOptions.Enabled=false;
				butRemove.Enabled=false;
				butUp.Enabled=false;
				butDown.Enabled=false;
			}
		}

		private void butAdd_Click(object sender,EventArgs e) {
			if(String.IsNullOrWhiteSpace(textOption.Text)) {
				return;
			}
			listboxComboOptions.Items.Add(textOption.Text);
			textOption.Clear();
		}

		private void butRemove_Click(object sender,EventArgs e) {
			if(listboxComboOptions.SelectedIndex==-1) {
				return;
			}
			listboxComboOptions.Items.RemoveAt(listboxComboOptions.SelectedIndex);
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(listboxComboOptions.SelectedIndex==-1 || listboxComboOptions.SelectedIndex==0) {
				return;
			}
			int idx=listboxComboOptions.SelectedIndex;
			string item=listboxComboOptions.Items[idx].ToString();
			listboxComboOptions.Items.RemoveAt(idx);
			listboxComboOptions.Items.Insert(idx-1,item);
			listboxComboOptions.SelectedIndex=idx-1;
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(listboxComboOptions.SelectedIndex==-1 || listboxComboOptions.SelectedIndex==listboxComboOptions.Items.Count-1) {
				return;
			}
			int idx=listboxComboOptions.SelectedIndex;
			string item=listboxComboOptions.Items[idx].ToString();
			listboxComboOptions.Items.RemoveAt(idx);
			listboxComboOptions.Items.Insert(idx+1,item);
			listboxComboOptions.SelectedIndex=idx+1;
		}

		protected override void OnOk(){
            if(!ArePosAndSizeValid()) {
                return;
            }
			SheetFieldDefCur.TabOrder=PIn.Int(textTabOrder.Text);
			SheetFieldDefCur.ReportableName=PIn.String(textReportable.Text);
			SheetFieldDefCur.UiLabelMobile=textUiLabelMobile.Text;
			//ComboBox FieldValue will be:  selectedItem;all|possible|options|here|with|selectedItem|also
			//This is so we don't have to change the database schema for combo boxes.
			SheetFieldDefCur.FieldValue=_selectedOption+";";//NOTE: ; can change to whatever.  Maybe {?  Maybe something else not used often like @?
			for(int i=0;i<listboxComboOptions.Items.Count;i++) {
				if(i>0) {
					SheetFieldDefCur.FieldValue+="|";
				}
				SheetFieldDefCur.FieldValue+=listboxComboOptions.Items[i].ToString();
			}
			SheetFieldDefCur.IsNew=false;
			DialogResult=DialogResult.OK;
		}
	}
}