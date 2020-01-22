using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormSheetFieldPatImage:FormSheetFieldBase {
		private List<Def> _listImageCatDefs;

		public FormSheetFieldPatImage(SheetDef sheetDef,SheetFieldDef sheetFieldDef,bool isReadOnly):base(sheetDef,sheetFieldDef,isReadOnly) {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetFieldPatImage_Load(object sender,EventArgs e) {
			FillCombo();
		}

		private void FillCombo(){
			comboImageCategory.Items.Clear();
			_listImageCatDefs=Defs.GetDefsForCategory(DefCat.ImageCats,true);
			for(int i=0;i<_listImageCatDefs.Count;i++) {
				comboImageCategory.Items.Add(_listImageCatDefs[i].ItemName);
				if(SheetFieldDefCur.FieldName==_listImageCatDefs[i].DefNum.ToString()) {
					comboImageCategory.SelectedIndex=i;
				}
			}
		}

        protected override void OnOk() {
            if(!ArePosAndSizeValid()) {
                return;
            }
			if(comboImageCategory.SelectedIndex<0) {
				MsgBox.Show(this,"Please select an image category first.");
				return;
			}
			SheetFieldDefCur.FieldName=_listImageCatDefs[comboImageCategory.SelectedIndex].DefNum.ToString();
			//don't save to database here.
			SheetFieldDefCur.IsNew=false;
			DialogResult=DialogResult.OK;
		}
	}
}