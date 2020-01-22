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
	public partial class FormSheetFieldSigBox:FormSheetFieldBase {

		public FormSheetFieldSigBox(SheetDef sheetDef,SheetFieldDef sheetFieldDef, bool isReadOnly,bool isEditMobile=false):
			base(sheetDef,sheetFieldDef,isReadOnly,isEditMobile) {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetFieldSigBox_Load(object sender,EventArgs e) {
			textUiLabelMobile.Visible=SheetDefs.IsMobileAllowed(_sheetDefCur.SheetType);
			labelUiLabelMobile.Visible=SheetDefs.IsMobileAllowed(_sheetDefCur.SheetType);
			checkRequired.Checked=SheetFieldDefCur.IsRequired;
			textUiLabelMobile.Text=SheetFieldDefCur.UiLabelMobile;
			textName.Text=SheetFieldDefCur.FieldName;
		}

        protected override void OnOk() {
            if(!ArePosAndSizeValid()) {
                return;
            }
			SheetFieldDefCur.IsRequired=checkRequired.Checked;
			SheetFieldDefCur.UiLabelMobile=textUiLabelMobile.Text;
			SheetFieldDefCur.FieldName=textName.Text;
			//don't save to database here.
			SheetFieldDefCur.IsNew=false;
			DialogResult=DialogResult.OK;
		}

	}
}