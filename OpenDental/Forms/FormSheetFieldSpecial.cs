using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using SparksToothChart;

namespace OpenDental {
	public partial class FormSheetFieldSpecial:FormSheetFieldBase {
		private List<SheetFieldDef> _listFieldDefsAvailable;
		///<summary>Only SheetFieldDefs that are associated to this SheetFieldLayoutMode will be shown in the listBoxAvailable.</summary>
		public SheetFieldLayoutMode LayoutMode=SheetFieldLayoutMode.Default;

		public FormSheetFieldSpecial(SheetDef sheetDef,SheetFieldDef sheetFieldDef,bool isReadOnly,SheetFieldLayoutMode layoutMode): base(sheetDef,sheetFieldDef,isReadOnly) {
			InitializeComponent();
			Lan.F(this);
			LayoutMode=layoutMode;
		}

		private void FormSheetFieldDefEdit_Load(object sender,EventArgs e) {
			base.SetFilterControlsAndAction(()=>SheetFilterChanged(),0,textWidth);
			textYPos.MaxVal=_sheetDefCur.HeightTotal-1;//The maximum y-value of the sheet field must be within the page vertically.
			if(_isReadOnly){
				butOK.Enabled=false;
				butDelete.Enabled=false;
			}
			_listFieldDefsAvailable=SheetFieldsAvailable.GetSpecial(_sheetDefCur.SheetType,LayoutMode);
			listBoxAvailable.Items.AddRange(_listFieldDefsAvailable.Select(x => (object)x.FieldName).ToArray());
			if(SheetFieldDefCur.IsNew) {
				listBoxAvailable.SetSelected(0,true);
				SheetFieldDefCur=_listFieldDefsAvailable[0];
			}
			else {
				listBoxAvailable.SetSelected(_listFieldDefsAvailable.FindIndex(x => x.FieldName==SheetFieldDefCur.FieldName),true);
				listBoxAvailable.Enabled=false;
			}
			if(SheetFieldDefCur.FieldName.In("SetPriorityListBox","PanelEcw")) {//Dynamic special controls which have growth/fill logic.
				comboGrowthBehavior.Visible=true;
				labelGrowth.Visible=true;
			}
			FillFields();
		}

		private void SheetFilterChanged() {
			if(_sheetDefCur.SheetType.GetAttributeOrDefault<SheetLayoutAttribute>().IsDynamic && SheetFieldDefCur.FieldName=="toothChart") {
				int height=(int)Math.Round(PIn.Double(textWidth.Text)*ToothChartData.SizeOriginalDrawing.Height)/ToothChartData.SizeOriginalDrawing.Width;
				textHeight.Text=POut.Int(height);
			}
		}

		///<summary>Each special field type is a little bit different, this allows each field to fill the form in its own way.</summary>
		private void FillFields() {
			labelSpecialInfo.Text="";
			//textXPos.Enabled=true;
			//textYPos.Enabled=true;
			textHeight.Enabled=true;
			textWidth.Enabled=true;
			textHeight.ReadOnly=false;
			//These are set in the base constructor, but also need to be updated here for listBoxAvailable_SelectedIndexChanged
			textXPos.Text=SheetFieldDefCur.XPos.ToString();
			textYPos.Text=SheetFieldDefCur.YPos.ToString();
			textWidth.Text=SheetFieldDefCur.Width.ToString();
			textHeight.Text=SheetFieldDefCur.Height.ToString();
			bool isDynamicSheetType=_sheetDefCur.SheetType.GetAttributeOrDefault<SheetLayoutAttribute>().IsDynamic;
			SheetUtil.FillComboGrowthBehavior(comboGrowthBehavior,SheetFieldDefCur.GrowthBehavior,isDynamicSheetType);
			textWidth.MinVal=-100;//Default for control in this window.
			textHeight.MinVal=-100;//Default for control in this window.
			switch(listBoxAvailable.SelectedItem.ToString()) {
				case "toothChart":
					labelSpecialInfo.Text=Lan.g(this,"The tooth chart will display a graphical toothchart based on which patient and treatment plan is selected. "+
					                                 "Fixed aspect ratio of 410/307");
					if(_sheetDefCur.SheetType.GetAttributeOrDefault<SheetLayoutAttribute>().IsDynamic) {
						//Allow user to edit toothChart width.
						textHeight.ReadOnly=true;
						textWidth.MinVal=410;
						textHeight.MinVal=307;
					}
					break;
				case "toothChartLegend":
					labelSpecialInfo.Text=Lan.g(this,"The tooth chart legend shows what the colors on the tooth chart mean.");
					textWidth.Text=POut.Int(DashToothChartLegend.DefaultWidth);
					textHeight.Text=POut.Int(DashToothChartLegend.DefaultHeight);
					if(!SheetDefs.IsDashboardType(_sheetDefCur)) {
						textWidth.Enabled=false;
						textHeight.Enabled=false;
					}
					break;
				case "toothGrid"://not used
				default:
					break;
			}
		}

		private void listBoxAvailable_SelectedIndexChanged(object sender,EventArgs e) {
			if(!SheetFieldDefCur.IsNew) {
				//We don't allow this to be changed once the field is no longer "new"
				//If this method was like the ChangeCommited() this would not be needed but
				//the setting of the index on a existing sheetfield comes into this method.
				return;
			}
			if(listBoxAvailable.SelectedIndices.Count==0 || listBoxAvailable.SelectedIndex<0) {
				return;
			}
			SheetFieldDefCur=_listFieldDefsAvailable[listBoxAvailable.SelectedIndex];
			FillFields();
		}

        protected override void OnDelete() {
			if(SheetFieldDefCur.IsNew) {
				DialogResult=DialogResult.Cancel;
			}
			else {
				SheetFieldDefCur=null; //null this out so the calling form knows to delete this sheetfield
				DialogResult=DialogResult.OK;
			}
		}

        protected override void OnOk() {
            if(!ArePosAndSizeValid()) {
                return;
            }
			SheetFieldDefCur.GrowthBehavior=comboGrowthBehavior.GetSelected<GrowthBehaviorEnum>();
			//don't save to database here.
			SheetFieldDefCur.IsNew=false;
			DialogResult=DialogResult.OK;
		}

	}
}