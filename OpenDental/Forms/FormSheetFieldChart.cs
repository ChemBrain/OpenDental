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
	public partial class FormSheetFieldChart:FormSheetFieldBase {

		public FormSheetFieldChart(SheetDef sheetDef,SheetFieldDef sheetFieldDef,bool isReadOnly): base(sheetDef,sheetFieldDef,isReadOnly) {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetFieldChart_Load(object sender,EventArgs e) {
			if(SheetFieldDefCur.FieldValue[0]!='0' && SheetFieldDefCur.FieldValue[0]!='1') {
				SheetFieldDefCur.FieldValue="0;"+SheetFieldDefCur.FieldValue;//For sheets created previously that have no Primary or Permanent chart type
			}
			if(SheetFieldDefCur.FieldValue[0]=='0') {
				radioPermanent.Checked=true;
			}
			else {
				radioPrimary.Checked=true;
			}
		}

        protected override void OnOk() {
			if(radioPermanent.Checked) {
				SheetFieldDefCur.FieldValue="0;"+SheetFieldDefCur.FieldValue.Substring(2);
			}
			else {
				//Switching from permanent tooth chart to primary tooth chart.  Primary tooth charts need 4 more tooth values.
				SheetFieldDefCur.FieldValue="1;,,;,,;,,;,,;,,;,,;,,;,,;,,;,,;,,;,,;,,;,,;,,;,,;,,;,,;,,;,,;,,;";
			}
			SheetFieldDefCur.IsNew=false;
			DialogResult=DialogResult.OK;
		}
	}
}