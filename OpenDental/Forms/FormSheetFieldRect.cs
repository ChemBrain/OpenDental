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
	public partial class FormSheetFieldRect:FormSheetFieldBase {

		public FormSheetFieldRect(SheetDef sheetDef,SheetFieldDef sheetFieldDef,bool isReadOnly): base(sheetDef,sheetFieldDef,isReadOnly) {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetFieldRect_Load(object sender,EventArgs e) {
		}

        protected override void OnOk() {
            if(!ArePosAndSizeValid()) {
                return;
            }
			//don't save to database here.
			SheetFieldDefCur.IsNew=false;
			DialogResult=DialogResult.OK;
		}

	}
}