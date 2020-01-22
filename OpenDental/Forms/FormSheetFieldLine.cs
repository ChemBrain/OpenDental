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
	public partial class FormSheetFieldLine:FormSheetFieldBase {

		public FormSheetFieldLine(SheetDef sheetDef,SheetFieldDef sheetFieldDef,bool isReadOnly): base(sheetDef,sheetFieldDef,isReadOnly) {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetFieldLine_Load(object sender,EventArgs e) {
			if(_sheetDefCur.SheetType!=SheetTypeEnum.Statement) {
				checkPmtOpt.Visible=false;
			}
			checkPmtOpt.Checked=SheetFieldDefCur.IsPaymentOption;
			butColor.BackColor=SheetFieldDefCur.ItemColor;
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
			SheetFieldDefCur.IsPaymentOption=checkPmtOpt.Checked;
			SheetFieldDefCur.ItemColor=butColor.BackColor;
			//don't save to database here.
			SheetFieldDefCur.IsNew=false;
			DialogResult=DialogResult.OK;
		}
	}
}