using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormExamSheets:ODForm {
		//DataTable table;
		private List<Sheet> sheetList;
		public long PatNum;

		public FormExamSheets() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormExamSheets_Load(object sender,EventArgs e) {
			Patient pat=Patients.GetLim(PatNum);
			Text=Lan.g(this,"Exam Sheets for")+" "+pat.GetNameFL();
			FillListExamTypes();
			FillGrid();
		}

		private void FillListExamTypes(){
			listExamTypes.Items.Clear();
			List<SheetDef> sheetDefs=SheetDefs.GetCustomForType(SheetTypeEnum.ExamSheet);
			listExamTypes.Items.Add(Lan.g(this,"All"));//Option to filter for all exam types.
			for(int i=0;i<sheetDefs.Count;i++) {
				listExamTypes.Items.Add(sheetDefs[i].Description);
			}
			listExamTypes.SelectedIndex=0;//Default to "All".
		}

		private void listExamTypes_SelectedIndexChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid(){
			//if a sheet is selected, remember it
			long selectedSheetNum=0;
			if(gridMain.GetSelectedIndex()!=-1) {
				selectedSheetNum=sheetList[gridMain.GetSelectedIndex()].SheetNum;//PIn.Long(table.Rows[gridMain.GetSelectedIndex()]["SheetNum"].ToString());
			}
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g(this,"Date"),70);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Time"),50);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Description"),210);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			//Pass in empty description string when filtering for "All".
			string filterStr=(listExamTypes.SelectedIndex==0?"":listExamTypes.SelectedItem.ToString());//0 => All
			sheetList=Sheets.GetExamSheetsTable(PatNum,DateTime.MinValue,DateTime.MaxValue,filterStr,true);
			for(int i=0;i<sheetList.Count;i++){
				row=new GridRow();
				row.Cells.Add(sheetList[i].DateTimeSheet.ToShortDateString());// ["date"].ToString());
				row.Cells.Add(sheetList[i].DateTimeSheet.ToShortTimeString());// ["time"].ToString());
				row.Cells.Add(sheetList[i].Description);
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
			//reselect
			if(selectedSheetNum!=0) {
				for(int i=0;i<sheetList.Count;i++) {
					if(sheetList[i].SheetNum==selectedSheetNum){ //table.Rows[i]["SheetNum"].ToString()==selectedSheetNum.ToString()) {
						gridMain.SetSelected(i,true);
						break;
					}
				}
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			//Sheets
			long sheetNum=sheetList[e.Row].SheetNum;// PIn.Long(table.Rows[e.Row]["SheetNum"].ToString());
			Sheet sheet=Sheets.GetSheet(sheetNum);//must use this to get fields
			FormSheetFillEdit.ShowForm(sheet,FormSheetFillEdit_Grid_FormClosing);
		}

		private void menuItemSheets_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormSheetDefs FormSD=new FormSheetDefs();
			FormSD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Sheets");
			FillListExamTypes();
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormSheetPicker FormS=new FormSheetPicker();
			FormS.SheetType=SheetTypeEnum.ExamSheet;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			SheetDef sheetDef;
			Sheet sheet=null;//only useful if not Terminal
			for(int i=0;i<FormS.SelectedSheetDefs.Count;i++) {
				sheetDef=FormS.SelectedSheetDefs[i];
				sheet=SheetUtil.CreateSheet(sheetDef,PatNum);
				SheetParameter.SetParameter(sheet,"PatNum",PatNum);
				SheetFiller.FillFields(sheet);
				SheetUtil.CalculateHeights(sheet);
			}
			FormSheetFillEdit.ShowForm(sheet,FormSheetFillEdit_Add_FormClosing);
		}

		/// <summary>Event handler for closing FormSheetFillEdit when it is non-modal.</summary>
		private void FormSheetFillEdit_Grid_FormClosing(object sender,FormClosingEventArgs e) {
			if(((FormSheetFillEdit)sender).DialogResult==DialogResult.OK || ((FormSheetFillEdit)sender).DidChangeSheet) {
				FillGrid();
			}
		}

		/// <summary>Event handler for closing FormSheetFillEdit when it is non-modal.</summary>
		private void FormSheetFillEdit_Add_FormClosing(object sender,FormClosingEventArgs e) {
			if(((FormSheetFillEdit)sender).DialogResult==DialogResult.OK || ((FormSheetFillEdit)sender).DidChangeSheet) {
				if(((FormSheetFillEdit)sender).SheetCur!=null && ((FormSheetFillEdit)sender).SheetCur.Description!=listExamTypes.SelectedItem.ToString()) {
					listExamTypes.SelectedIndex=0;//0 => All
				}
				FillGrid();
				gridMain.SetSelected(false);//unselect all rows
				gridMain.SetSelected(gridMain.ListGridRows.Count-1,true);//Select the newly added row. Always last, since ordered by date.
			}
		}

		private void butCancel_Click(object sender,EventArgs e) {
			Close();
		}
		

		

		
	}
}