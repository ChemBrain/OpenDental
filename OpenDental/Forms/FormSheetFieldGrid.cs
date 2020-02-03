using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using CodeBase;

namespace OpenDental {
	public partial class FormSheetFieldGrid:FormSheetFieldBase {
		private bool _isDynamicSheetType;

		public FormSheetFieldGrid(SheetDef sheetDef,SheetFieldDef sheetFieldDef,bool isReadOnly): base(sheetDef,sheetFieldDef,isReadOnly) {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetFieldGrid_Load(object sender,EventArgs e) {
			textGridType.Text=SheetFieldDefCur.FieldName;
			_isDynamicSheetType=_sheetDefCur.SheetType.GetAttributeOrDefault<SheetLayoutAttribute>().IsDynamic;
			if(_isDynamicSheetType || SheetDefs.IsDashboardType(_sheetDefCur)) {
				//Allow user to set dimensions of grids in dynamic layout defs.
				//These values define the min width and height.
				textHeight.Enabled=true;
				textWidth.Enabled=true;
				if(_isDynamicSheetType) {
					comboGrowthBehavior.Enabled=true;
				}
			}
			else {
				List<DisplayField> Columns=SheetUtil.GetGridColumnsAvailable(SheetFieldDefCur.FieldName);
				SheetFieldDefCur.Width=0;
				foreach(DisplayField f in Columns) {
					SheetFieldDefCur.Width+=f.ColumnWidth;
				}
			}
			UI.ODGrid odGrid=new ODGrid();
			odGrid.TranslationName="";
			using(Graphics g=Graphics.FromImage(new Bitmap(100,100))) {
				if(SheetFieldDefCur.FieldName=="EraClaimsPaid" || SheetDefs.IsDashboardType(_sheetDefCur) || _isDynamicSheetType) {
					//Do not modify grid heights for Eras, Appt grid and dynamic layouts as the heights are calculated elsewhere.
				}
				else {
					//Why do we change the grid title height here?  The heights are also set elsewhere...
					SheetFieldDefCur.Height=0;
					//These grids display a title.
					if(new[] {"StatementPayPlan","StatementDynamicPayPlan","StatementInvoicePayment","TreatPlanBenefitsFamily","TreatPlanBenefitsIndividual"}.Contains(SheetFieldDefCur.FieldName)) {
						SheetFieldDefCur.Height+=18;//odGrid.TitleHeight;
					}
					SheetFieldDefCur.Height+=15//odGrid.HeaderHeight
						+(int)g.MeasureString("Any",odGrid.Font,100,StringFormat.GenericTypographic).Height+3;
				}
				textHeight.Text=SheetFieldDefCur.Height.ToString();
			}
			SheetUtil.FillComboGrowthBehavior(comboGrowthBehavior,SheetFieldDefCur.GrowthBehavior,_isDynamicSheetType,true);
		}
		private bool IsValid(out string error) {
			error="";
			if(comboGrowthBehavior.GetSelected<GrowthBehaviorEnum>()==GrowthBehaviorEnum.FillDownFitColumns 
				&& SheetFieldDefCur.FieldName!="ProgressNotes")
			{ 
				error="FillDownFitColumns can only be selected for the ProgressNotes grid.";
			}
			return error.IsNullOrEmpty();
		}

		protected override void OnOk() {
			string error;
			if(!IsValid(out error)) {
				MsgBox.Show(error);
				return;
			}
			//don't save to database here.
			SheetFieldDefCur.XPos=PIn.Int(textXPos.Text);
			SheetFieldDefCur.YPos=PIn.Int(textYPos.Text);
			//Only enabled for grids related to a dynamic sheetType, and Dashboard Appointment Grid.
			if(_isDynamicSheetType || SheetDefs.IsDashboardType(_sheetDefCur)) {
				SheetFieldDefCur.Height=PIn.Int(textHeight.Text);
				SheetFieldDefCur.Width=PIn.Int(textWidth.Text);
				SheetFieldDefCur.GrowthBehavior=comboGrowthBehavior.GetSelected<GrowthBehaviorEnum>();
			}
			SheetFieldDefCur.IsNew=false;
			DialogResult=DialogResult.OK;
		}
	}
}