using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using OpenDental.UI;
using System.Threading;
using OpenDentBusiness.WebTypes.WebForms;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OpenDental {
	public partial class FormSheetDefEdit:ODForm {
		private SheetDef _sheetDefCur;
		public bool IsInternal;
		private bool MouseIsDown;
		private bool CtrlIsDown;
		private Point MouseOriginalPos;
		private Point MouseCurrentPos;
		private List<Point> OriginalControlPositions;
		///<summary>When you first mouse down, if you clicked on a valid control, this will be false.  For drag selection, this must be true.</summary>
		private bool ClickedOnBlankSpace;
		private bool AltIsDown;
		///<summary>This is our 'clipboard' for copy/paste of fields.</summary>
		private List<SheetFieldDef> ListSheetFieldDefsCopyPaste;
		private int PasteOffset=0;
		///<summary>After each 10 pastes to the upper left origin, this increments 10 to shift the next 10 down.</summary>
		private int PasteOffsetY=0;
		private bool IsTabMode;
		private List<SheetFieldDef> ListSheetFieldDefsTabOrder;
		public static Font tabOrderFont = new Font("Times New Roman",12f,FontStyle.Regular,GraphicsUnit.Pixel);
		private Bitmap BmBackground;
		private Graphics GraphicsBackground;
		///<summary>This stores the previous calculations so that we don't have to recal unless certain things have changed.  The key is the index of the sheetfield.  The data is an array of objects of different types as seen in the code.</summary>
		private Hashtable HashRtfStringCache=new Hashtable();
		///<summary>Arguments to draw fields such as pens, brushes, and fonts.</summary>
		private DrawFieldArgs _argsDF;
		///<summary>Only used here to draw the dashed margin lines.</summary>
		private System.Drawing.Printing.Margins _printMargin=new System.Drawing.Printing.Margins(0,0,40,60);
		private Image _toothChart;
		private bool _hasChartSealantComplete;
		private bool _hasChartSealantTreatment;
		///<summary>If the sheet def is linked to any web sheets, this will hold those web sheet defs.</summary>
		private List<WebForms_SheetDef> _listWebSheetDefs;
		private object _lock=new object();
		///<summary>Set to true if the web forms have been downloaded or if an error occurred.</summary>
		private bool _hasSheetsDownloaded=false;
		private Random _rand=new Random();
		private SheetEditMobileCtrl sheetEditMobile=new SheetEditMobileCtrl() { Size=new Size(642,758) };
		///<summary>When using auto snap feature this will be set to a positve value, otherwise 0.
		///This is the width and height of the squares that fieldDefs will snap to when enabled.</summary>
		private int _autoSnapDistance=0;
		///<summary>List of X coordinates that will be shown on panelMain. Empty if auto snap is not enabled.</summary>
		private List<int> _listSnapXVals=new List<int>();
		///<summary>List of Y coordinates that will be shown on panelMain. Empty if auto snap is not enabled.</summary>
		private List<int> _listSnapYVals=new List<int>();
		///<summary>Most sheets will only have the default mode. Dynamic module layouts can have multiple.</summary>
		private SheetFieldLayoutMode _sheetLayoutModeCur;

		private bool GetIsDynamicSheetType() {
			return _sheetDefCur.SheetType.GetAttributeOrDefault<SheetLayoutAttribute>().IsDynamic;
		}

		///<summary>Some controls (panels in this case) do not pass key events to the parent (the form in this case) even when the property KeyPreview is set.  Instead, the default key functionality occurs.  An example would be the arrow keys.  By default, arrow keys set focus to the "next" control.  Instead, want all key presses on this form and all of it's child controls to always call the FormSheetDefEdit_KeyDown method.</summary>
		protected override bool ProcessCmdKey(ref Message msg,Keys keyData) {
			FormSheetDefEdit_KeyDown(this,new KeyEventArgs(keyData));
			return true;//This indicates that all keys have been processed.
			//return base.ProcessCmdKey(ref msg,keyData);//We don't need this right now, because no textboxes, for example.
		}

		public FormSheetDefEdit(SheetDef sheetDef) {
			InitializeComponent();
			InitializeComponentSheetEditMobile();
			Lan.F(this);
			_sheetDefCur=sheetDef;
			if(sheetDef.IsLandscape){
				Width=sheetDef.Height+190;
				Height=sheetDef.Width+65;
			}
			else{
				Width=sheetDef.Width+190;
				Height=sheetDef.Height+65;
			}
			if(Width<600){
				Width=600;
			}
			if(Height<600){
				Height=600;
			}
			if(Width>SystemInformation.WorkingArea.Width){
				Width=SystemInformation.WorkingArea.Width;
			}
			if(Height>SystemInformation.WorkingArea.Height){
				Height=SystemInformation.WorkingArea.Height;
			}
		}
		
		private void InitializeComponentSheetEditMobile() {
			sheetEditMobile.SheetFieldDefSelected+=new System.EventHandler<long>(this.sheetEditMobile_SheetDefSelected);
			sheetEditMobile.AddMobileHeader+=new EventHandler(butAddMobileHeader_Click);
			//HasMobileLayout must be kept in sync because both FromSheetDefEdit and SheetEditMobileCtrl can changes its value at any time.
			sheetEditMobile.HasMobileLayoutChanged+=new EventHandler<bool>((o,hasMobileLayout) => {
				_sheetDefCur.HasMobileLayout=hasMobileLayout;
			});
			sheetEditMobile.TranslationProvider=new Func<string,string>((s) => { return Lan.g(this,s); });
			sheetEditMobile.NewMobileHeader+=new EventHandler<SheetEditMobileCtrl.NewMobileFieldValueArgs>((o,e) => {
				var item=_sheetDefCur.SheetFieldDefs.FirstOrDefault(x => x.SheetFieldDefNum==e.SheetFieldDefNum);
				if(item!=null) {
					item.UiLabelMobile=e.NewFieldValue;
					FillFieldList();
				}
			});
			sheetEditMobile.NewStaticText+=new EventHandler<SheetEditMobileCtrl.NewMobileFieldValueArgs>((o,e) => {
				var item=_sheetDefCur.SheetFieldDefs.FirstOrDefault(x => x.SheetFieldDefNum==e.SheetFieldDefNum);
				if(item!=null) {
					item.FieldValue=e.NewFieldValue;
					FillFieldList();
				}
			});
			sheetEditMobile.SheetFieldDefEdit+=new EventHandler<SheetEditMobileCtrl.SheetFieldDefEditArgs>((o,e) => {
				if(!_sheetDefCur.SheetFieldDefs.Any(x => e.SheetFieldDefNums.Contains(x.SheetFieldDefNum))) {
					return;
				}
				List<SheetFieldDef> sheetFields=_sheetDefCur.SheetFieldDefs.FindAll(x => e.SheetFieldDefNums.Contains(x.SheetFieldDefNum));
				SheetFieldDef sheetField=sheetFields[0];
				LaunchEditWindow(sheetField,isEditMobile:true);
			});
		}

		private void FormSheetDefEdit_Load(object sender,EventArgs e) {
			if(IsInternal) {
				butDelete.Visible=false;
				butOK.Visible=false;
				butCancel.Text=Lan.g(this,"Close");
				groupAddNew.Visible=false;
				groupPage.Visible=false;
				groupAlignH.Visible=false;
				groupAlignV.Visible=false;
				linkLabelTips.Visible=false;
				butCopy.Visible=false;
				butPaste.Visible=false;
				butTabOrder.Visible=false;
			}
			else {
				labelInternal.Visible=false;
			}
			//TODO: We really should clean up the logic related to setting the UI, this is getting messy.
			sheetEditMobile.IsReadOnly=IsInternal;
			butMobile.Visible=SheetDefs.IsMobileAllowed(_sheetDefCur.SheetType);
			if(!_sheetDefCur.SheetType.In(SheetTypeEnum.Statement,SheetTypeEnum.MedLabResults,SheetTypeEnum.TreatmentPlan,SheetTypeEnum.PaymentPlan,SheetTypeEnum.ReferralLetter)){
				butAddGrid.Visible=false;
			}
			if(Sheets.SheetTypeIsSinglePage(_sheetDefCur.SheetType)) {
				groupPage.Visible=false;
			}
			if(_sheetDefCur.SheetType.In(SheetTypeEnum.TreatmentPlan,SheetTypeEnum.ReferralLetter)){
				butAddSpecial.Visible=true;
				_toothChart=GetToothChartHelper(0);
			}
			if(_sheetDefCur.SheetType==SheetTypeEnum.Screening) {
				butScreenChart.Visible=true;
			}
			if(_sheetDefCur.SheetType.In(SheetTypeEnum.ERA,SheetTypeEnum.ERAGridHeader)){
				butAddCheckBox.Visible=false;
				butAddSigBox.Visible=false;
				butAddCombo.Visible=false;
				butAddPatImage.Visible=false;
			}
			if(_sheetDefCur.SheetType!=SheetTypeEnum.TreatmentPlan) {
				butAddSigBoxPractice.Visible=false;//only show button if sheet is a treatment plan type.
			}
			if(_sheetDefCur.SheetType==SheetTypeEnum.ChartModule) {
				butAddOutputText.Visible=false;
				butAddStaticText.Visible=false;
				butAddInputField.Visible=false;
				butAddLine.Visible=false;
				butAddCheckBox.Visible=false;
				butAddRect.Visible=false;
				butAddImage.Visible=false;
				butAddSigBox.Visible=false;
				butAddCombo.Visible=false;
				butAddPatImage.Visible=false;
				butScreenChart.Visible=false;
				groupPage.Visible=false;
				butTabOrder.Visible=false;
				butAddGrid.Visible=true;
				butAddSpecial.Visible=true;
				_toothChart=GetToothChartHelper(0);
			}
			if(_sheetDefCur.IsLandscape) {
				panelMain.Width=_sheetDefCur.Height;
				panelMain.Height=_sheetDefCur.Width;
			}
			else {
				panelMain.Width=_sheetDefCur.Width;
				panelMain.Height=_sheetDefCur.Height;
			}
			EnableDashboardWidgetOptions(_sheetDefCur.SheetType==SheetTypeEnum.PatientDashboardWidget);
			textDescription.Text=_sheetDefCur.Description;
			panelMain.Height=_sheetDefCur.HeightTotal;
			InitLayoutModes();//Must be before initial FillFieldList().
			FillFieldList();
			RefreshDoubleBuffer();
			panelMain.Refresh();
			panelMain.Focus();
			if(_sheetDefCur.HasMobileLayout) { //Always open the mobile editor since they want to use the mobile layout.
				sheetEditMobile.ShowHideModeless(true,_sheetDefCur.Description,this);
			}
			ODThread threadGetWebSheetId=new ODThread(GetWebSheetDefs);
			threadGetWebSheetId.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception ex) => {
				//So that the main thread will know the worker thread is done getting the web sheet defs.
				_hasSheetsDownloaded=true;
			}));
			threadGetWebSheetId.AddExitHandler(new ODThread.WorkerDelegate((ODThread o) => {
				//So that the main thread will know the worker thread is done getting the web sheet defs.
				_hasSheetsDownloaded=true;
			}));
			threadGetWebSheetId.Name="GetWebSheetIdThread";
			threadGetWebSheetId.Start(true);
			//textDescription.Focus();
		}

		///<summary>If areDashboardWidgetOptionsEnabled, only certain buttons will show, including DashboardWidget specific buttons.  Otherwise, 
		///DashboardWidget specific buttons will be hidden.</summary>
		private void EnableDashboardWidgetOptions(bool areDashboardWidgetOptionsEnabled) {
			List<UI.Button> listButtons=this.GetAllControls().OfType<UI.Button>().ToList();
			List<UI.Button> listDashboardButtons;
			if(areDashboardWidgetOptionsEnabled) {
				//List of the only buttons that should be visible if in DashboardWidget edit mode.
				listDashboardButtons=new List<UI.Button>() { butEdit,butAlignTop,butAlignLeft,butAlignCenterH,butAlignRight,butCancel,butAddStaticText
					,butAddPatImage,butAddGrid,butAddSpecial,butAddLine,butAddRect };
				if(!IsInternal) {
					listDashboardButtons.AddRange(new List<UI.Button>() { butOK,butDelete,butCopy,butPaste });
				}
				foreach(UI.Button but in listButtons) {
					if(but.In(listDashboardButtons)) {//In DashboardWidget mode, so only show these buttons.
						but.Visible=true;
						continue;
					}
					but.Visible=false;
				}
				_toothChart=GetToothChartHelper(0);
			}
		}

		///<summary>Called during load to set _sheetTypeModeCur and to identify pertinent secondary TreatPlan layout mode.
		///The initial layout will never be a treatment plan layout because the Treatment Plan checkbox is always disabled in Chart initially.</summary>
		private void InitLayoutModes() {
			_sheetLayoutModeCur=SheetFieldLayoutMode.Default;
			//Eventually we might introduce sheet layouts to other modules.
			//For now it is only implemented in the Chart Module.
			switch(_sheetDefCur.SheetType) {
				case SheetTypeEnum.ChartModule:
					if(Programs.UsingEcwTightOrFullMode()) {
						_sheetLayoutModeCur=SheetFieldLayoutMode.Ecw;
					}
					else if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
						_sheetLayoutModeCur=SheetFieldLayoutMode.MedicalPractice;
					}
					else if(Programs.UsingOrion) {
						_sheetLayoutModeCur=SheetFieldLayoutMode.Orion;
					}
					SheetFieldLayoutMode treatPlanLayout;//The TreatmentPlan counterpart layout mode.
					switch(_sheetLayoutModeCur) {
						default:
						case SheetFieldLayoutMode.Default:
							treatPlanLayout=SheetFieldLayoutMode.TreatPlan;
							break;
						case SheetFieldLayoutMode.Ecw:
							treatPlanLayout=SheetFieldLayoutMode.EcwTreatPlan;
							break;
						case SheetFieldLayoutMode.MedicalPractice:
							treatPlanLayout=SheetFieldLayoutMode.MedicalPracticeTreatPlan;
							break;
						case SheetFieldLayoutMode.Orion:
							treatPlanLayout=SheetFieldLayoutMode.OrionTreatPlan;
							break;
					}
					radioLayoutDefault.Tag=_sheetLayoutModeCur;//Used in radioLayoutDefault_CheckedChanged()
					radioLayoutTP.Tag=treatPlanLayout;//Used in radioLayoutDefault_CheckedChanged()
					groupBoxLayoutModes.Visible=true;//Must be after setting the Tags
					radioLayoutDefault.Checked=true;
					break;
				default:
					break;
			}
		}

		///<summary>Fills _listWebSheetIds with any webforms_sheetdefs that have the same SheetDefNum of the current SheetDef.</summary>
		private void GetWebSheetDefs(ODThread odThread) {
			//Ignore the certificate errors for the staging machine and development machine
			if(PrefC.GetString(PrefName.WebHostSynchServerURL).In(WebFormL.SynchUrlStaging,WebFormL.SynchUrlDev)) {
				WebFormL.IgnoreCertificateErrors();
			}
			List<WebForms_SheetDef> listWebFormSheetDefs;
			if(WebForms_SheetDefs.TryDownloadSheetDefs(out listWebFormSheetDefs)) {
				lock(_lock) {
					_listWebSheetDefs=listWebFormSheetDefs.Where(x => x.SheetDefNum==_sheetDefCur.SheetDefNum).ToList();
				}
			}
		}

		private void FillFieldList() {
			listFields.Items.Clear();
			string txt;
			if(GetIsDynamicSheetType()) {
				_sheetDefCur.SheetFieldDefs=_sheetDefCur.SheetFieldDefs
					.OrderByDescending(x => x.FieldType==SheetFieldType.Grid)//Grids first
					.ThenBy(x => x.FieldName.Contains("Button")).ToList();//Buttons last, always drawn on top.
			}
			else {
				_sheetDefCur.SheetFieldDefs.Sort(SheetFieldDefs.CompareTabOrder);
			}
			foreach(SheetFieldDef fieldDef in _sheetDefCur.SheetFieldDefs) {
				if(fieldDef.LayoutMode!=_sheetLayoutModeCur) {
					continue;
				}
				switch(fieldDef.FieldType) {
					case SheetFieldType.StaticText:
						txt=fieldDef.FieldValue;
						break;
					case SheetFieldType.Image:
						txt=Lan.g(this,"Image:")+fieldDef.FieldName;
						break;
					case SheetFieldType.PatImage:
						txt=Lan.g(this,"PatImg:")+Defs.GetName(DefCat.ImageCats,PIn.Long(fieldDef.FieldName));
						break;
					case SheetFieldType.Line:
						txt=Lan.g(this,"Line:")+fieldDef.XPos.ToString()+","+fieldDef.YPos.ToString()+","+"W:"+fieldDef.Width.ToString()+","+"H:"+fieldDef.Height.ToString();
						break;
					case SheetFieldType.Rectangle:
						txt=Lan.g(this,"Rect:")+fieldDef.XPos.ToString()+","+fieldDef.YPos.ToString()+","+"W:"+fieldDef.Width.ToString()+","+"H:"+fieldDef.Height.ToString();
						break;
					case SheetFieldType.SigBox:
						txt=Lan.g(this,"Signature Box");
						break;
					case SheetFieldType.SigBoxPractice:
						txt=Lan.g(this,"Practice Signature Box");
						break;
					case SheetFieldType.CheckBox:
						txt=fieldDef.TabOrder.ToString()+": ";
						if(fieldDef.FieldName.StartsWith("allergy:") || fieldDef.FieldName.StartsWith("problem:")) {
							txt+=fieldDef.FieldName.Remove(0,8);
						}
						else {
							txt+=fieldDef.FieldName;
						}
						if(fieldDef.RadioButtonValue!="") {
							txt+=" - "+fieldDef.RadioButtonValue;
						}
						break;
					case SheetFieldType.ComboBox:
						txt=fieldDef.TabOrder > 0 ? fieldDef.TabOrder.ToString()+": " : "";
						txt+=Lan.g(this,"ComboBox:")+fieldDef.XPos.ToString()+","+fieldDef.YPos.ToString()
							+","+"W:"+fieldDef.Width.ToString();
						break;
					case SheetFieldType.InputField:
						txt=fieldDef.TabOrder.ToString()+": "+fieldDef.FieldName;
						break;
					case SheetFieldType.Grid:
						txt="Grid:"+fieldDef.FieldName;
						break;
					case SheetFieldType.MobileHeader:
						txt=Lan.g(this,"Mobile Only:")+" "+fieldDef.UiLabelMobile;
						break;
					default:
						txt=fieldDef.FieldName;
						break;
				} //end switch
				listFields.Items.Add(new ODBoxItem<SheetFieldDef>(txt,fieldDef));
			}
			sheetEditMobile.SheetDef=_sheetDefCur;
		}

		private void panelMain_Paint(object sender,PaintEventArgs e) {
			Bitmap doubleBuffer=new Bitmap(panelMain.Width,panelMain.Height);
			Graphics g=Graphics.FromImage(doubleBuffer);
			g.DrawImage(BmBackground,0,0);
			if(_autoSnapDistance>0) {
				DrawAutoSnapLines(g);
			}
			DrawFields(_sheetDefCur,g,false);
			e.Graphics.DrawImage(doubleBuffer,0,0);
			g.Dispose();
			doubleBuffer.Dispose();
			doubleBuffer=null;
		}

		public void DrawAutoSnapLines(Graphics g) {
			if(_listSnapXVals.Count==0) {
				int x=0;
				while(x<=panelMain.Width) {
					_listSnapXVals.Add(x);
					x+=_autoSnapDistance;
				}
				int y=0;
				while(y<=panelMain.Height) {
					_listSnapYVals.Add(y);
					y+=_autoSnapDistance;
				}
			}
			using(Pen pen=new Pen(Color.LightGray)) {
				foreach(int x in _listSnapXVals) {//Vertical lines
					g.DrawLine(pen,new Point(x,0),new Point(x,this.Height));
				}
				foreach(int y in _listSnapYVals) {//Horizontal lines
					g.DrawLine(pen,new Point(0,y),new Point(this.Width,y));
				}
			}
		}

		///<summary>Whenever a user might have edited or moved a background image, this gets called.</summary>
		private void RefreshDoubleBuffer() {
			GraphicsBackground.FillRectangle(Brushes.White,0,0,BmBackground.Width,BmBackground.Height);
			DrawFields(_sheetDefCur,GraphicsBackground,true);
		}

		///<summary>If drawImages is true then only image fields will be drawn. Otherwise, all fields but images will be drawn.</summary>
		private void DrawFields(SheetDef sheetDef,Graphics g,bool onlyDrawImages,bool isHighlightEligible=true) {
			SetGraphicsHelper(g);
			List<SheetFieldDef> listSheetFieldDefs=GetSheetFieldDefsForLayoutModeCur(sheetDef);
			for(int i=0;i<listSheetFieldDefs.Count;i++) {
				SheetFieldDef sheetFieldDef=listSheetFieldDefs[i];
				if(sheetFieldDef.LayoutMode!=_sheetLayoutModeCur) {
					continue;
				}
				if(onlyDrawImages) {
					if(sheetFieldDef.FieldType==SheetFieldType.Image) {
						DrawImagesHelper(sheetFieldDef,g);
					}
					continue;
				} //end onlyDrawImages
				bool isSelected=listFields.SelectedIndices.Contains(i);
				bool isFieldSelected=(isHighlightEligible && isSelected);
				switch(sheetFieldDef.FieldType) {
					case SheetFieldType.Parameter: //Skip
					case SheetFieldType.MobileHeader: //Skip
					case SheetFieldType.Image: //Handled above
						continue;
					case SheetFieldType.PatImage:
						DrawPatImageHelper(sheetFieldDef,g,isFieldSelected);
						continue;
					case SheetFieldType.Line:
						DrawLineHelper(sheetFieldDef,g,isFieldSelected);
						continue;
					case SheetFieldType.Rectangle:
						DrawRectangleHelper(sheetFieldDef,g,isFieldSelected);
						continue;
					case SheetFieldType.CheckBox:
						DrawCheckBoxHelper(sheetFieldDef,g,isFieldSelected);
						continue;
					case SheetFieldType.ComboBox:
						DrawComboBoxHelper(sheetFieldDef,g,isFieldSelected);
						DrawTabModeHelper(sheetFieldDef,g,isHighlightEligible);
						continue;
					case SheetFieldType.ScreenChart:
						DrawToothChartHelper(sheetFieldDef,g,isFieldSelected);
						continue;
					case SheetFieldType.SigBox:
					case SheetFieldType.SigBoxPractice:
						DrawSigBoxHelper(sheetFieldDef,g,isFieldSelected);
						continue;
					case SheetFieldType.Special:
						DrawSpecialHelper(sheetFieldDef,g,sheetDef.Width,isFieldSelected);
						continue;
					case SheetFieldType.Grid:
						DrawGridHelper(sheetFieldDef,g,isSelected);
						continue;
					case SheetFieldType.InputField:
					case SheetFieldType.StaticText:
					case SheetFieldType.OutputText:
					default:
						DrawStringHelper(sheetFieldDef,g,isFieldSelected);
						DrawTabModeHelper(sheetFieldDef,g,isHighlightEligible);
						continue;
				} //end switch
			}
			DrawSelectionRectangle(g);
			//Draw pagebreak
			Pen pDashPage=new Pen(Color.Green);
			pDashPage.DashPattern=new float[] {4.0F,3.0F,2.0F,3.0F};
			Pen pDashMargin=new Pen(Color.Green);
			pDashMargin.DashPattern=new float[] {1.0F,5.0F};
			int margins=(_printMargin.Top+_printMargin.Bottom);
			for(int i=1;i<sheetDef.PageCount;i++) {
				//g.DrawLine(pDashMargin,0,i*SheetDefCur.HeightPage-_printMargin.Bottom,SheetDefCur.WidthPage,i*SheetDefCur.HeightPage-_printMargin.Bottom);
				g.DrawLine(pDashPage,0,i*(sheetDef.HeightPage-margins)+_printMargin.Top,sheetDef.WidthPage,i*(sheetDef.HeightPage-margins)+_printMargin.Top);
				//g.DrawLine(pDashMargin,0,i*SheetDefCur.HeightPage+_printMargin.Top,SheetDefCur.WidthPage,i*SheetDefCur.HeightPage+_printMargin.Top);
			}
			//End Draw Page Break
		}

		#region DrawFields Helpers (In Alphabetical Order)
		private void SetGraphicsHelper(Graphics g) {
			g.SmoothingMode=SmoothingMode.HighQuality;
			g.CompositingQuality=CompositingQuality.HighQuality; //This has to be here or the line thicknesses are wrong.
			if(_argsDF!=null) {
				_argsDF.Dispose();
			}
			_argsDF=new DrawFieldArgs(); //reset _argsDF
		}

		private void DrawCheckBoxHelper(SheetFieldDef sheetFieldDef,Graphics g,bool isSelected) {
			if(isSelected) {
				_argsDF.pen=_argsDF.penRedThick;
			}
			else {
				_argsDF.pen=_argsDF.penBlueThick;
			}
			g.DrawLine(_argsDF.pen,sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.XPos+sheetFieldDef.Width-1,sheetFieldDef.YPos+sheetFieldDef.Height-1);
			g.DrawLine(_argsDF.pen,sheetFieldDef.XPos+sheetFieldDef.Width-1,sheetFieldDef.YPos,sheetFieldDef.XPos,sheetFieldDef.YPos+sheetFieldDef.Height-1);
			if(IsTabMode) {
				Rectangle tabRect=new Rectangle(sheetFieldDef.XPos-1, //X
					sheetFieldDef.YPos-1, //Y
					(int)g.MeasureString(sheetFieldDef.TabOrder.ToString(),tabOrderFont).Width+1, //Width
					12); //height
				if(ListSheetFieldDefsTabOrder.Contains(sheetFieldDef)) { //blue border, white box, blue letters
					g.FillRectangle(Brushes.White,tabRect);
					g.DrawRectangle(Pens.Blue,tabRect);
					g.DrawString(sheetFieldDef.TabOrder.ToString(),tabOrderFont,Brushes.Blue,tabRect.X,tabRect.Y-1);
				}
				else { //Blue border, blue box, white letters
					g.FillRectangle(_argsDF.brushBlue,tabRect);
					g.DrawString(sheetFieldDef.TabOrder.ToString(),tabOrderFont,Brushes.White,tabRect.X,tabRect.Y-1);
				}
			}
		}

		private void DrawComboBoxHelper(SheetFieldDef sheetFieldDef,Graphics g,bool isSelected) {
			if(isSelected) {
				_argsDF.pen=_argsDF.penRed;
				_argsDF.brush=_argsDF.brushRed;
			}
			else {
				_argsDF.pen=_argsDF.penBlue;
				_argsDF.brush=_argsDF.brushBlue;
			}
			g.DrawRectangle(_argsDF.pen,sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.Width,sheetFieldDef.Height);
			g.DrawString("("+Lan.g(this,"combo box")+")",Font,_argsDF.brush,sheetFieldDef.XPos,sheetFieldDef.YPos);
		}

		private void DrawToothChartHelper(SheetFieldDef sheetFieldDef,Graphics g,bool isSelected) {
			if(isSelected) {
				_argsDF.pen=_argsDF.penRed;
				_argsDF.brush=_argsDF.brushRed;
			}
			else {
				_argsDF.pen=_argsDF.penBlue;
				_argsDF.brush=_argsDF.brushBlue;
			}
			g.DrawRectangle(_argsDF.pen,sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.Width,sheetFieldDef.Height);
			string toothChart="("+Lan.g(this,"tooth chart")+" "+sheetFieldDef.FieldName;
			if(sheetFieldDef.FieldValue[0]=='1') {//Primary teeth chart
				toothChart+=" "+Lan.g(this,"primary teeth");
			}
			else {//Permanent teeth chart
				toothChart+=" "+Lan.g(this,"permanent teeth");
			}
			toothChart+=")";
			g.DrawString(toothChart,Font,_argsDF.brush,sheetFieldDef.XPos,sheetFieldDef.YPos);
			if(sheetFieldDef.FieldName=="ChartSealantTreatment") {
				_hasChartSealantTreatment=true;
			}
			if(sheetFieldDef.FieldName=="ChartSealantComplete") {
				_hasChartSealantComplete=true;
			}
		}

		private void DrawGridHelper(SheetFieldDef sheetFieldDef,Graphics g,bool isSelected) {
			int heightGridTitle=18;
			int heightGridHeader=15;
			int heightGridRow=13+2;
			List<DisplayField> columns=SheetUtil.GetGridColumnsAvailable(sheetFieldDef.FieldName);
			ODGrid odGrid;
			if(GetIsDynamicSheetType()) {
				odGrid=CreateDyanmicGrid(sheetFieldDef,columns);
				SheetUtil.SetControlSizeAndAnchors(sheetFieldDef,odGrid,panelMain);
			}
			else {
				odGrid=CreateGridHelper(columns);
			}
			int yPosGrid=sheetFieldDef.YPos;
			SizeF sSize;
			bool drawHeaders=true;
			switch(sheetFieldDef.FieldName) {
				#region StatementPayPlan
				case "StatementPayPlan":
				case "StatementDynamicPayPlan":
					string text="Payment Plans";
					if(sheetFieldDef.FieldName=="StatementDynamicPayPlan") {
						text="Dynamic Payment Plans";
					}
					sSize=g.MeasureString(text,new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold));
					g.FillRectangle(Brushes.White,sheetFieldDef.XPos,yPosGrid,odGrid.Width,heightGridTitle);
					g.DrawString(text,new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold),new SolidBrush(Color.Black),sheetFieldDef.XPos+(sheetFieldDef.Width-sSize.Width)/2,yPosGrid);
					yPosGrid+=heightGridTitle;
					break;
				#endregion
				#region StatementInvoicePayment
				case "StatementInvoicePayment":
					sSize=g.MeasureString("Payments",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold));
					g.FillRectangle(Brushes.White,sheetFieldDef.XPos,yPosGrid,odGrid.Width,heightGridTitle);
					g.DrawString("Payments",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold),new SolidBrush(Color.Black),sheetFieldDef.XPos,yPosGrid);
					yPosGrid+=heightGridTitle;
					break;
				#endregion
				#region TreatPlanBenefitsFamily
				case "TreatPlanBenefitsFamily":
					sSize=g.MeasureString("Family Insurance Benefits",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold));
					g.FillRectangle(Brushes.White,sheetFieldDef.XPos,yPosGrid,odGrid.Width,heightGridTitle);
					g.DrawString("Family Insurance Benefits",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold),Brushes.Black,sheetFieldDef.XPos+(sheetFieldDef.Width-sSize.Width)/2,yPosGrid);
					yPosGrid+=heightGridTitle;
					break;
				#endregion
				#region TreatPlanBenefitsIndividual
				case "TreatPlanBenefitsIndividual":
					sSize=g.MeasureString("Individual Insurance Benefits",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold));
					g.FillRectangle(Brushes.White,sheetFieldDef.XPos,yPosGrid,odGrid.Width,heightGridTitle);
					g.DrawString("Individual Insurance Benefits",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold),Brushes.Black,sheetFieldDef.XPos+(sheetFieldDef.Width-sSize.Width)/2,yPosGrid);
					yPosGrid+=heightGridTitle;
					break;
				#endregion
				#region EraClaimsPaid
				case "EraClaimsPaid":
					int xPosGrid=sheetFieldDef.XPos;
					SheetDef gridHeaderSheetDef;
					if(IsInternal) {
						gridHeaderSheetDef=SheetsInternal.GetSheetDef(SheetTypeEnum.ERAGridHeader);
					}
					else {
						gridHeaderSheetDef=SheetDefs.GetInternalOrCustom(SheetInternalType.ERAGridHeader);
					}
					gridHeaderSheetDef.SheetFieldDefs.ForEach(x => {
						x.XPos+=(xPosGrid);
						x.YPos+=(yPosGrid);//-gridHeaderSheetDef.Height);
					});//Make possitions relative to this sheet
					DrawFields(gridHeaderSheetDef,g,false,false);
					yPosGrid+=gridHeaderSheetDef.Height;
					/*---------------------------------------------------------------------------------------------*/
					sSize=g.MeasureString("ERA Claims Paid",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold));
					g.FillRectangle(Brushes.White,xPosGrid,yPosGrid,odGrid.Width,heightGridTitle);
					g.DrawString("ERA Claims Paid",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold),Brushes.Black,xPosGrid+(sheetFieldDef.Width-sSize.Width)/2,yPosGrid);
					yPosGrid+=heightGridTitle;
					/*---------------------------------------------------------------------------------------------*/
					odGrid.PrintHeader(g,sheetFieldDef.XPos,yPosGrid);
					yPosGrid+=heightGridHeader;
					odGrid.PrintRow(0,g,sheetFieldDef.XPos,yPosGrid,false,true); //a single dummy row.
					yPosGrid+=heightGridRow;
					/*---------------------------------------------------------------------------------------------*/
					List<DisplayField> remarkColumns=SheetUtil.GetGridColumnsAvailable("EraClaimsPaidProcRemarks");
					ODGrid remarkGrid=CreateGridHelper(remarkColumns);
					remarkGrid.PrintHeader(g,sheetFieldDef.XPos,yPosGrid+1);
					yPosGrid+=heightGridHeader;
					remarkGrid.PrintRow(0,g,sheetFieldDef.XPos,yPosGrid,false,true); //a single dummy row.
					yPosGrid+=heightGridRow;
					/*---------------------------------------------------------------------------------------------*/
					drawHeaders=false;
					break;
				#endregion
				#region PatientInfo, ProgressNotes, TreatmentPlans, Procedures
				case "PatientInfo":
				case "ProgressNotes":
				case "TreatmentPlans":
				case "Procedures":
					int gridRemainingHeight; //Used to draw "rows" in the SheetDefEdit
					if(sheetFieldDef.GrowthBehavior.In(GrowthBehaviorEnum.FillDown,GrowthBehaviorEnum.FillRightDown,GrowthBehaviorEnum.FillDownFitColumns)) {
						int lowerBoundary=0;
						switch(sheetFieldDef.GrowthBehavior) {
							case GrowthBehaviorEnum.FillDownFitColumns:
								int minY=0;//Mimics height logic in SheetUserControl.IsControlAbove(...)
								List<SheetFieldDef> listAboveControls=GetSheetFieldDefsForLayoutModeCur().FindAll(x => IsSheetFieldDefAbove(sheetFieldDef,x));
								if(listAboveControls.Count>0) {
									minY=listAboveControls.Max(x => (x.YPos+x.Height))+1;
								}
								sheetFieldDef.YPos=minY;
								yPosGrid=minY;
								break;
							case GrowthBehaviorEnum.FillDown:
								List<SheetFieldDef> listBelowControls=GetSheetFieldDefsForLayoutModeCur().FindAll(x => IsSheetFieldDefBelow(sheetFieldDef, x));
								if(listBelowControls.Count>0) {
									lowerBoundary=(panelMain.Height-listBelowControls.Min(x => x.YPos)-1);
								}
								break;
						}
						gridRemainingHeight=Math.Max(1,(panelMain.Height-yPosGrid-lowerBoundary));
					}
					else {
						gridRemainingHeight=sheetFieldDef.Height;
					}					
					odGrid.Title=sheetFieldDef.FieldName;
					odGrid.PrintTitle(g,sheetFieldDef.XPos,yPosGrid);
					yPosGrid+=heightGridTitle;
					gridRemainingHeight-=heightGridTitle;
					odGrid.PrintHeader(g,sheetFieldDef.XPos,yPosGrid);
					yPosGrid+=heightGridHeader;
					gridRemainingHeight-=heightGridHeader;
					int gridRowHeight=heightGridRow;//Zero until we calculate the height of the first row.
					while(gridRemainingHeight>gridRowHeight) {
						odGrid.PrintRow(0,g,sheetFieldDef.XPos,yPosGrid,(gridRemainingHeight-gridRowHeight <= gridRowHeight),true);//A single dummy row.
						yPosGrid+=gridRowHeight;
						gridRemainingHeight-=gridRowHeight;
					}
					drawHeaders=false;
					break;
				#endregion
			}
			if(drawHeaders) {
				odGrid.PrintHeader(g,sheetFieldDef.XPos,yPosGrid);
				yPosGrid+=heightGridHeader;
				odGrid.PrintRow(0,g,sheetFieldDef.XPos,yPosGrid,false,true); //a single dummy row.
				yPosGrid+=heightGridRow;
			}
			#region drawFooter
			if(sheetFieldDef.FieldName=="StatementPayPlan" || sheetFieldDef.FieldName=="StatementDynamicPayPlan") {
				string text="Payment Plan Amount Due: "+"0.00";
				if(sheetFieldDef.FieldName=="StatementDynamicPayPlan") {
					text="Dynamic Payment Plan Amount Due: "+"0.00";
				}
				RectangleF rf=new RectangleF(sheetFieldDef.Width-sheetFieldDef.Width-60,yPosGrid,sheetFieldDef.Width,heightGridTitle);
				g.FillRectangle(Brushes.White,rf);
				StringFormat sf=new StringFormat();
				sf.Alignment=StringAlignment.Far;
				g.DrawString(text,new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold),new SolidBrush(Color.Black),rf,sf);
			}
			if(sheetFieldDef.FieldName=="StatementInvoicePayment") {
				RectangleF rf=new RectangleF(sheetFieldDef.Width-sheetFieldDef.Width-60,yPosGrid,sheetFieldDef.Width,heightGridTitle);
				g.FillRectangle(Brushes.White,rf);
				StringFormat sf=new StringFormat();
				sf.Alignment=StringAlignment.Far;
				if(PrefC.GetBool(PrefName.InvoicePaymentsGridShowNetProd)) {
					g.DrawString("Total Payments & WriteOffs:  0.00",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold),new SolidBrush(Color.Black),rf,sf);
				}
				else {
					g.DrawString("Total Payments:  0.00",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold),new SolidBrush(Color.Black),rf,sf);
				}
			}
			#endregion
			if(isSelected) {
				//Most dynamic grids do not modify the user set width.
				if(!(GetIsDynamicSheetType() || SheetDefs.IsDashboardType(_sheetDefCur)) 
					|| sheetFieldDef.GrowthBehavior==GrowthBehaviorEnum.FillDownFitColumns) 
				{
					columns=SheetUtil.GetGridColumnsAvailable(sheetFieldDef.FieldName);
					sheetFieldDef.Width=0;
					for(int c=0;c<columns.Count;c++) {
						sheetFieldDef.Width+=columns[c].ColumnWidth;
					}
				}
				g.DrawRectangle(_argsDF.penRedThick,sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.Width,sheetFieldDef.Height);//hightlight selected
			}
			else if(GetIsDynamicSheetType()) {//Always highlight dynamic module grid defs so user can see min height and width.
				g.DrawRectangle(_argsDF.penBlue,sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.Width,sheetFieldDef.Height);//hightlight all dynamic
			}
		}

		private bool IsSheetFieldDefBelow(SheetFieldDef fieldDef,SheetFieldDef otherFieldDef) {
			return (otherFieldDef.YPos>fieldDef.YPos
				&& ((otherFieldDef.XPos.Between(fieldDef.XPos,(fieldDef.XPos+fieldDef.Width))
				|| (otherFieldDef.XPos+otherFieldDef.Width).Between(fieldDef.XPos,(fieldDef.XPos+fieldDef.Width))
				|| fieldDef.XPos.Between(otherFieldDef.XPos,otherFieldDef.XPos+otherFieldDef.Width)
				))
			);
		}

		private ODGrid CreateGridHelper(List<DisplayField> columns) {
			ODGrid odGrid=new ODGrid();
			odGrid.Width=0;
			odGrid.TranslationName="";
			for(int c=0;c<columns.Count;c++) {
				odGrid.Width+=columns[c].ColumnWidth;
			}
			odGrid.HideScrollBars=true;
			odGrid.BeginUpdate();
			odGrid.ListGridColumns.Clear();
			GridColumn col;
			for(int c=0;c<columns.Count;c++) {
				col=new GridColumn(columns[c].Description,columns[c].ColumnWidth);
				odGrid.ListGridColumns.Add(col);
			}
			GridRow row=new GridRow(); //Add dummy row
			for(int c=0;c<columns.Count;c++) {
				row.Cells.Add(" "); //add dummy row.
			}
			odGrid.ListGridRows.Add(row);
			odGrid.EndUpdate(); //Calls ComputeRows and ComputeColumns, meaning the RowHeights int[] has been filled.
			return odGrid;
		}

		///<summary>Creates a grid with the given columns for grids associated to a dynamic layout sheetDef.</summary>
		private ODGrid CreateDyanmicGrid(SheetFieldDef fieldDef,List<DisplayField> listColumns) {
			ODGrid odGrid=new ODGrid();
			odGrid.Width=fieldDef.Width;
			odGrid.TranslationName="";
			odGrid.BeginUpdate();
			odGrid.ListGridColumns.Clear();
			foreach(DisplayField column in listColumns) { 
				string colHeader=column.Description;
				if(string.IsNullOrEmpty(colHeader)) {
					colHeader=column.InternalName;
				}
				odGrid.ListGridColumns.Add(new GridColumn(colHeader,column.ColumnWidth));
			}
			GridRow row=new GridRow();//Add empty row
			for(int c=0;c<listColumns.Count; c++) {
				row.Cells.Add(" ");//Add empty cell to give the empty row some content
			}
			odGrid.ListGridRows.Add(row);
			odGrid.EndUpdate();//Calls ComputeRows and ComputeColumns, meaning the RowHeights int[] has been filled.
			return odGrid;
		}

		///<summary>Does not check for vertical overlap.
		///Returns true if targetFieldDef is above the given baseFieldDef such that the YPos is less and the XPos is within baseFieldDefs range.
		///Otherwise returns false.</summary>
		private bool IsSheetFieldDefAbove(SheetFieldDef fieldDef,SheetFieldDef otherFieldDef) {
			//Logic mimics SheetUserControl.IsControlAbove(...)
			return (otherFieldDef.YPos<fieldDef.YPos 
				&& (otherFieldDef.XPos.Between(fieldDef.XPos,(fieldDef.XPos+fieldDef.Width))
				|| (otherFieldDef.XPos+otherFieldDef.Width).Between(fieldDef.XPos,(fieldDef.XPos+fieldDef.Width)))
			);
		}

		private void DrawImagesHelper(SheetFieldDef sheetFieldDef,Graphics g) {
			string filePathAndName=ODFileUtils.CombinePaths(SheetUtil.GetImagePath(),sheetFieldDef.FieldName);
			Image img=null;
			if(sheetFieldDef.ImageField!=null) {//The image has already been downloaded.
				img=new Bitmap(sheetFieldDef.ImageField);
			}
			else if(sheetFieldDef.FieldName=="Patient Info.gif") {
				img=OpenDentBusiness.Properties.Resources.Patient_Info;
			}
			else if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ && File.Exists(filePathAndName)) {
				img=Image.FromFile(filePathAndName);
			}
			else if(CloudStorage.IsCloudStorage) {
				FormProgress FormP=new FormProgress();
				FormP.DisplayText=Lan.g(CloudStorage.LanThis,"Downloading...");
				FormP.NumberFormat="F";
				FormP.NumberMultiplication=1;
				FormP.MaxVal=100;//Doesn't matter what this value is as long as it is greater than 0
				FormP.TickMS=1000;
				OpenDentalCloud.Core.TaskStateDownload state=CloudStorage.DownloadAsync(SheetUtil.GetImagePath(),sheetFieldDef.FieldName,
					new OpenDentalCloud.ProgressHandler(FormP.OnProgress));
				if(FormP.ShowDialog()==DialogResult.Cancel) {
					state.DoCancel=true;
					return;
				}
				if(state==null || state.FileContent==null) {
					img=null;
				}
				else {
					using(MemoryStream stream=new MemoryStream(state.FileContent)) {
						img=new Bitmap(Image.FromStream(stream));
						sheetFieldDef.ImageField=new Bitmap(Image.FromStream(stream));//So it doesn't have to be downloaded again the next time.
					}
				}
			}
			else {
#if DEBUG
				g.DrawRectangle(new Pen(Brushes.IndianRed),sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.Width,sheetFieldDef.Height);
				g.DrawString(Lan.g(this,"Cannot find image")+": "+sheetFieldDef.FieldName,Font,_argsDF.brush??Brushes.Black,sheetFieldDef.XPos,sheetFieldDef.YPos);
#endif
				return;
			}
			g.DrawImage(img,sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.Width,sheetFieldDef.Height);
#if DEBUG
			g.DrawRectangle(new Pen(Brushes.IndianRed),sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.Width,sheetFieldDef.Height);
#endif
			if(img!=null) {
				img.Dispose();
			}
		}

		private void DrawLineHelper(SheetFieldDef sheetFieldDef,Graphics g,bool isSelected) {
			if(isSelected) {
				_argsDF.pen=_argsDF.penRed;
			}
			else {
				_argsDF.penLine.Color=sheetFieldDef.ItemColor;
				_argsDF.pen=_argsDF.penLine;
			}
			g.DrawLine(_argsDF.pen,sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.XPos+sheetFieldDef.Width,sheetFieldDef.YPos+sheetFieldDef.Height);
		}

		private void DrawPatImageHelper(SheetFieldDef sheetFieldDef,Graphics g,bool isSelected) {
			if(isSelected) {
				_argsDF.pen=_argsDF.penRed;
				_argsDF.brush=_argsDF.brushRed;
			}
			else {
				_argsDF.pen=_argsDF.penBlack;
				_argsDF.brush=_argsDF.brushBlue;
			}
			g.DrawRectangle(_argsDF.pen,sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.Width,sheetFieldDef.Height);
			g.DrawString("PatImage: "+Defs.GetName(DefCat.ImageCats,PIn.Long(sheetFieldDef.FieldName)),Font /*NOT _argsDF.font*/,_argsDF.brush,sheetFieldDef.XPos+1,sheetFieldDef.YPos+1);
		}

		private void DrawRectangleHelper(SheetFieldDef sheetFieldDef,Graphics g,bool isSelected) {
			if(isSelected) {
				_argsDF.pen=_argsDF.penRed;
			}
			else {
				_argsDF.pen=_argsDF.penBlack;
			}
			g.DrawRectangle(_argsDF.pen,sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.Width,sheetFieldDef.Height);
		}

		private void DrawSelectionRectangle(Graphics g) {
			if(ClickedOnBlankSpace) {
				g.DrawRectangle(_argsDF.penSelection,
					//The math functions are used below to account for users clicking and dragging up, down, left, or right.
					Math.Min(MouseOriginalPos.X,MouseCurrentPos.X), //X
					Math.Min(MouseOriginalPos.Y,MouseCurrentPos.Y), //Y
					Math.Abs(MouseCurrentPos.X-MouseOriginalPos.X), //Width
					Math.Abs(MouseCurrentPos.Y-MouseOriginalPos.Y)); //Height
			}
		}

		private void DrawSigBoxHelper(SheetFieldDef sheetFieldDef,Graphics g,bool isSelected) {
			//font=new Font(Font,
			if(isSelected) {
				_argsDF.pen=_argsDF.penRed;
				_argsDF.brush=_argsDF.brushRed;
			}
			else {
				_argsDF.pen=_argsDF.penBlue;
				_argsDF.brush=_argsDF.brushBlue;
			}
			g.DrawRectangle(_argsDF.pen,sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.Width,sheetFieldDef.Height);
			g.DrawString((sheetFieldDef.FieldType==SheetFieldType.SigBoxPractice ? "(practice signature box)" : "(signature box)"),Font,_argsDF.brush,
				sheetFieldDef.XPos,sheetFieldDef.YPos);
		}

		private void DrawSpecialHelper(SheetFieldDef sheetFieldDef,Graphics g,int sheetDefWidth,bool isSelected) {
			if(isSelected) {
				_argsDF.pen=_argsDF.penRed;
				_argsDF.brush=_argsDF.brushRed;
			}
			else {
				_argsDF.pen=_argsDF.penBlue;
				_argsDF.brush=_argsDF.brushBlue;
			}
			Panel panel;
			switch(sheetFieldDef.FieldName) {
				#region toothChart
				case "toothChart":
					Rectangle rectBoundingBox=SheetPrinting.DrawScaledImage(sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.Width,sheetFieldDef.Height,g,null,_toothChart);
					g.DrawRectangle(Pens.LightGray,rectBoundingBox); //outline tooth grid so user can see how much wasted space there is.
					break;
				#endregion
				#region toothChartLegend
				case "toothChartLegend":
					List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ChartGraphicColors,true);
					int width;
					if (SheetDefs.IsDashboardType(_sheetDefCur)) {
						width=sheetFieldDef.Width;
					}
					else {
						width=sheetDefWidth;
					}
					SheetPrinting.DrawToothChartLegend(sheetFieldDef.XPos,sheetFieldDef.YPos,width,0,listDefs,g,null,SheetDefs.IsDashboardType(_sheetDefCur));
					break;
				case "familyInsurance":
				case "individualInsurance":
						DrawInsuranceHelper(sheetFieldDef,g);
					break;
				#endregion
				#region ChartModuleTabs
				case "ChartModuleTabs":
					TabControl controlTabPrimary=new TabControl();
					controlTabPrimary.Width=sheetFieldDef.Width;
					controlTabPrimary.Height=sheetFieldDef.Height;
					FormOpenDental.S_Contr_TabProcPageTitles.ForEach(x => controlTabPrimary.TabPages.Add(x));
					controlTabPrimary.TabPages[0].Controls.Add(
						new Label() { Text="Controls Show Here",Location=new Point(0,0),AutoSize=true
					});
					controlTabPrimary.SelectTab(0);
					DrawControlForSheetField(g,controlTabPrimary,sheetFieldDef);
					break;
				#endregion
				#region TreatmentNotes
				case "TreatmentNotes":
					//Not using ODtextBox becuase RichTextBox.DrawToBitmap(...) does not work.
					//https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.richtextbox.drawtobitmap?view=netframework-4.7.2
					//"This method is not relevant for this class."
					TextBox controlTreatNotes=new TextBox();
					controlTreatNotes.Multiline=true;
					controlTreatNotes.BackColor=Color.LightGray;
					controlTreatNotes.Text="Treatment notes";
					controlTreatNotes.Width=sheetFieldDef.Width;
					controlTreatNotes.Height=sheetFieldDef.Height;
					DrawControlForSheetField(g,controlTreatNotes,sheetFieldDef);
					break;
				#endregion
				#region TrackToothProcDates
				case "TrackToothProcDates":
					Label textTrackLabel=new Label();
					textTrackLabel.AutoSize=false;
					textTrackLabel.TextAlign=ContentAlignment.MiddleRight;
					textTrackLabel.Text="01/01/2000";
					textTrackLabel.Width=(int)(g.MeasureString(textTrackLabel.Text,textTrackLabel.Font).Width+5);
					textTrackLabel.Height=sheetFieldDef.Height;
					textTrackLabel.Location=new Point(0,0);
					TrackBar controlTrackBar=new TrackBar();
					controlTrackBar.AutoSize=false;
					controlTrackBar.Width=(sheetFieldDef.Width-textTrackLabel.Width);
					controlTrackBar.Height=sheetFieldDef.Height;
					controlTrackBar.Location=new Point(textTrackLabel.Width+1,0);
					panel=new Panel();
					panel.Height=sheetFieldDef.Height;
					panel.Width=sheetFieldDef.Width;
					panel.Controls.Add(textTrackLabel);
					panel.Controls.Add(controlTrackBar);
					DrawControlForSheetField(g,panel,sheetFieldDef);
					break;
				#endregion
				#region PanelEcw
				case "PanelEcw":
					Panel panelEcw=new Panel();
					panelEcw.BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle;
					panelEcw.Width=sheetFieldDef.Width;
					if(sheetFieldDef.GrowthBehavior.In(GrowthBehaviorEnum.FillDown,GrowthBehaviorEnum.FillRightDown)) {
						panelEcw.Height=Math.Max(1,(panelMain.Height-sheetFieldDef.YPos));
					}
					else {
						panelEcw.Height=sheetFieldDef.Height;
					}
					Label panelLabel=new Label();
					panelLabel.Text="ECW Browser";
					panelLabel.Location=new Point((panelEcw.Width/2)-panelLabel.Width/2,(panelEcw.Height/2));
					panelEcw.Controls.Add(panelLabel);
					DrawControlForSheetField(g,panelLabel,sheetFieldDef);
					break;
				#endregion
				#region ErxAccess PhoneNums ForeignKey USAKey
				case "ButtonErxAccess":
				case "ButtonPhoneNums":
				case "ButtonForeignKey":
				case "ButtonUSAKey":
				case "ButtonNewTP":
					UI.Button button=new UI.Button();
					if(sheetFieldDef.FieldName=="ButtonNewTP") {
						button.Image=OpenDental.Properties.Resources.Add;
						button.ImageAlign=System.Drawing.ContentAlignment.MiddleLeft;
					}
					button.Enabled=true;
					button.Text=(sheetFieldDef.FieldValue);
					button.Size=new Size(sheetFieldDef.Width,sheetFieldDef.Height);
					DrawControlForSheetField(g,button,sheetFieldDef);
					break;
				#endregion
				#region SetPriorityListBox
				case "SetPriorityListBox":
					Label label=new Label();
					label.AutoSize=false;
					label.Size=new Size(sheetFieldDef.Width,15);
					label.Text="Set Priority";
					label.TextAlign=ContentAlignment.BottomLeft;
					label.Location=new Point(0,0);
					ListBox listBox=new ListBox();
					listBox.Items.Add(Lan.g("ContrChart","No Priority"));
					Defs.GetDefsForCategory(DefCat.TxPriorities,true).ForEach(def => listBox.Items.Add(def.ItemName));
					listBox.Width=sheetFieldDef.Width;
					listBox.Top=label.Bottom;
					panel=new Panel();
					panel.Width=sheetFieldDef.Width;
					if(sheetFieldDef.GrowthBehavior.In(GrowthBehaviorEnum.FillDown,GrowthBehaviorEnum.FillRightDown)) {
						panel.Height=Math.Max(1,(panelMain.Height-sheetFieldDef.YPos)-1);
					}
					else {
						panel.Height=sheetFieldDef.Height;
					}					
					int rowCount=((panel.Height-label.Height)/listBox.ItemHeight);//Truncate to the nearest row, just as the listBox does internally.
					listBox.Height=rowCount*listBox.ItemHeight;
					panel.Height=Math.Max(1,listBox.Bottom-8);//-8 because with several test values in design mode we noticed the panel was always a fixed value too large.
					panel.Controls.Add(label);
					panel.Controls.Add(listBox);
					DrawControlForSheetField(g,panel,sheetFieldDef);
					break;
				#endregion
				default:
					g.DrawString("(Special:Tooth Grid)",Font,_argsDF.brush,sheetFieldDef.XPos,sheetFieldDef.YPos);
					break;
			} //end switch
			//draw rectangle on top of special so that user can see how big the field actually is.
			g.DrawRectangle(_argsDF.pen,sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.Width,sheetFieldDef.Height);
		}

		private void DrawControlForSheetField(Graphics g,Control control,SheetFieldDef sheetFieldDef) {
			Bitmap bitmap=new Bitmap(control.Width,control.Height);
			control.DrawToBitmap(bitmap,control.Bounds);
			g.DrawImage(bitmap,new Rectangle(sheetFieldDef.XPos,sheetFieldDef.YPos,control.Width,control.Height));
			bitmap.Dispose();
		}

		private void DrawInsuranceHelper(SheetFieldDef sheetFieldDef,Graphics g) {
			Bitmap bitmapIns=new Bitmap(sheetFieldDef.Width,sheetFieldDef.Height);
			Control ctrIns;
			switch(sheetFieldDef.FieldName) {
				case "individualInsurance":
					ctrIns=new DashIndividualInsurance();
					break;
				case "familyInsurance":
					ctrIns=new DashFamilyInsurance();
					break;
				default:
					g.DrawString("("+sheetFieldDef.FieldName+")",Font,_argsDF.brush,sheetFieldDef.XPos
						,sheetFieldDef.YPos);
					//draw rectangle on top of special so that user can see how big the field actually is.
					g.DrawRectangle(_argsDF.pen,sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.Width
						,sheetFieldDef.Height);
					return;
			} //end switch
			ctrIns.DrawToBitmap(bitmapIns,new Rectangle(0,0,sheetFieldDef.Width,sheetFieldDef.Height));
			Rectangle rectBound=SheetPrinting.DrawScaledImage(sheetFieldDef.XPos,sheetFieldDef.YPos,sheetFieldDef.Width,sheetFieldDef.Height,g,null,bitmapIns);
			g.DrawRectangle(Pens.LightGray,rectBound); //outline insurance grid so user can see how much wasted space there is.
			bitmapIns.Dispose();
		}

		private Image GetToothChartHelper(long patNum) {
			//linesPrinted=0;
			double[] colTotal=new double[10];
			//headingPrinted=false;
			//graphicsPrinted=false;
			//mainPrinted=false;
			//benefitsPrinted=false;
			//notePrinted=false;
			//pagesPrinted=0;
			//prints the graphical tooth chart and legend
			//Panel panelHide=new Panel();
			//panelHide.Size=new Size(600,500);
			//panelHide.BackColor=this.BackColor;
			//panelHide.SendToBack();
			//this.Controls.Add(panelHide);
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ChartGraphicColors);
			SparksToothChart.ToothChartWrapper toothChart=new SparksToothChart.ToothChartWrapper();
			toothChart.ColorBackground=listDefs[14].ItemColor;
			toothChart.ColorText=listDefs[15].ItemColor;
			//toothChart.TaoRenderEnabled=true;
			//toothChart.TaoInitializeContexts();
			toothChart.Size=new Size(500,370);
			//toothChart.Location=new Point(-600,-500);//off the visible screen
			//toothChart.SendToBack();
			//ComputerPref computerPref=ComputerPrefs.GetForLocalComputer();
			toothChart.UseHardware=ComputerPrefs.LocalComputer.GraphicsUseHardware;
			toothChart.SetToothNumberingNomenclature((ToothNumberingNomenclature)PrefC.GetInt(PrefName.UseInternationalToothNumbers));
			//Must be last preference set, last so that all settings are caried through in the reinitialization this line triggers.
			if(ComputerPrefs.LocalComputer.GraphicsSimple==DrawingMode.Simple2D) {
				toothChart.DrawMode=DrawingMode.Simple2D;
			}
			else if(ComputerPrefs.LocalComputer.GraphicsSimple==DrawingMode.DirectX) {
				toothChart.DeviceFormat=new SparksToothChart.ToothChartDirectX.DirectXDeviceFormat(ComputerPrefs.LocalComputer.DirectXFormat);
				toothChart.DrawMode=DrawingMode.DirectX;
			}
			else {
				toothChart.DrawMode=DrawingMode.OpenGL;
			}
			//The preferred pixel format number changes to the selected pixel format number after a context is chosen.
			ComputerPrefs.LocalComputer.PreferredPixelFormatNum=toothChart.PreferredPixelFormatNumber;
			ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			//this.Controls.Add(toothChart);
			//toothChart.BringToFront();
			toothChart.ResetTeeth();
			List<ToothInitial> ToothInitialList=patNum==0?new List<ToothInitial>():ToothInitials.Refresh(patNum);
			//first, primary.  That way, you can still set a primary tooth missing afterwards.
			for(int i=0;i<ToothInitialList.Count;i++) {
				if(ToothInitialList[i].InitialType==ToothInitialType.Primary) {
					toothChart.SetPrimary(ToothInitialList[i].ToothNum);
				}
			}
			for(int i=0;i<ToothInitialList.Count;i++) {
				switch(ToothInitialList[i].InitialType) {
					case ToothInitialType.Missing:
						toothChart.SetMissing(ToothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Hidden:
						toothChart.SetHidden(ToothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Rotate:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,ToothInitialList[i].Movement,0,0,0,0,0);
						break;
					case ToothInitialType.TipM:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,ToothInitialList[i].Movement,0,0,0,0);
						break;
					case ToothInitialType.TipB:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,ToothInitialList[i].Movement,0,0,0);
						break;
					case ToothInitialType.ShiftM:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,ToothInitialList[i].Movement,0,0);
						break;
					case ToothInitialType.ShiftO:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,0,ToothInitialList[i].Movement,0);
						break;
					case ToothInitialType.ShiftB:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,0,0,ToothInitialList[i].Movement);
						break;
					case ToothInitialType.Drawing:
						toothChart.AddDrawingSegment(ToothInitialList[i].Copy());
						break;
				}
			}
			//ComputeProcListFiltered();
			//DrawProcsGraphics();
			toothChart.AutoFinish=true;
			Image chartBitmap=toothChart.GetBitmap();
			toothChart.Dispose();
			return chartBitmap;
		}

		private void DrawStringHelper(SheetFieldDef sheetFieldDef,Graphics g,bool isSelected) {
			_argsDF.fontstyle=FontStyle.Regular;
			if(sheetFieldDef.FontIsBold) {
				_argsDF.fontstyle=FontStyle.Bold;
			}
			_argsDF.font=new Font(sheetFieldDef.FontName,sheetFieldDef.FontSize,_argsDF.fontstyle,GraphicsUnit.Point);
			if(isSelected) {
				g.DrawRectangle(_argsDF.penRed,sheetFieldDef.Bounds);
				_argsDF.brush=_argsDF.brushRed;
			}
			else {
				g.DrawRectangle(_argsDF.penBlue,sheetFieldDef.Bounds);
				_argsDF.brush=_argsDF.brushBlue;
			}
			string str;
			if(sheetFieldDef.FieldType==SheetFieldType.StaticText) {
				str=sheetFieldDef.FieldValue;
				//Static text can have a custom color.
				//Check to see if this text box is selected.  If it is, do not change the color.
				if(!isSelected) {
					_argsDF.brushText.Color=sheetFieldDef.ItemColor;
					_argsDF.brush=_argsDF.brushText;
				}
			}
			else {
				str=sheetFieldDef.FieldName;
			}
			DrawRTFstring(sheetFieldDef,str,_argsDF.font,_argsDF.brush,g);
		}

		private void DrawTabModeHelper(SheetFieldDef sheetFieldDef,Graphics g,bool isHighlightEligible) {
			if(!IsTabMode || !sheetFieldDef.FieldType.In(SheetFieldType.InputField,SheetFieldType.ComboBox)) {
				return;
			}
			Rectangle tabRect=new Rectangle(sheetFieldDef.XPos-1, //X
				sheetFieldDef.YPos-1, //Y
				(int)g.MeasureString(sheetFieldDef.TabOrder.ToString(),tabOrderFont).Width+1, //Width
				12); //height
			if(isHighlightEligible && ListSheetFieldDefsTabOrder.Contains(sheetFieldDef)) { //blue border, white box, blue letters
				g.FillRectangle(Brushes.White,tabRect);
				g.DrawRectangle(Pens.Blue,tabRect);
				g.DrawString(sheetFieldDef.TabOrder.ToString(),tabOrderFont,Brushes.Blue,tabRect.X,tabRect.Y-1);
			}
			else { //Blue border, blue box, white letters
				g.FillRectangle(_argsDF.brushBlue,tabRect);
				g.DrawString(sheetFieldDef.TabOrder.ToString(),tabOrderFont,Brushes.White,tabRect.X,tabRect.Y-1);
			}
		}

		///<summary>We need this special function to draw strings just like the RichTextBox control does, because sheet text is displayed using RichTextBoxes within FormSheetFillEdit.
		///Graphics.DrawString() uses a different font spacing than the RichTextBox control does.</summary>
		private void DrawRTFstring(SheetFieldDef field,string str,Font font,Brush brush,Graphics g) {
			str=str.Replace("\r",""); //For some reason '\r' throws off character position calculations.  \n still handles the CRs.
			//Font spacing is different for g.DrawString() as compared to RichTextBox and TextBox controls.
			//We create a RichTextBox here in the same manner as in FormSheetFillEdit, but we only use it to determine where to draw text.
			//We do not add the RichTextBox control to this form, because its background will overwrite everything behind that we have already drawn.
			bool doCalc=true;
			int index=_sheetDefCur.SheetFieldDefs.IndexOf(field);
			object[] data=(object[])HashRtfStringCache[index.ToString()];
			if(data!=null) { //That field has been calculated
				//If any of the following factors change, then that could potentially change text positions.
				if(field.FontName.CompareTo(data[1])==0 //Has font name changed since last pass?
				   && field.FontSize.CompareTo(data[2])==0 //Has font size changed since last pass?
				   && field.FontIsBold.CompareTo(data[3])==0 //Has font boldness changed since last pass?
				   && field.Width.CompareTo(data[4])==0 //Has field width changed since last pass?
				   && field.Height.CompareTo(data[5])==0 //Has field height changed since last pass?
				   && str.CompareTo(data[6])==0 //Has field text changed since last pass?
				   && field.TextAlign.CompareTo(data[7])==0) //Has field text align changed since last pass?
				{
					doCalc=false; //Nothing has changed. Do not recalculate.
				}
			}
			if(doCalc) {	
				//Data has not yet been cached for this text field, or the field has changed and needs to be recalculated.
				//All of these textbox fields are set using the similar logic as GraphicsHelper.CreateTextBoxForSheetDisplay(),
				//so that text in this form matches FormSheetFillEdit.
				RichTextBox textbox=new RichTextBox();
				textbox.Visible=false;
				textbox.BorderStyle=BorderStyle.None;
				textbox.ScrollBars=RichTextBoxScrollBars.None;
				textbox.SelectionAlignment=field.TextAlign;
				textbox.Location=new Point(field.XPos,field.YPos);
				textbox.Width=field.Width;
				textbox.Height=field.Height;
				textbox.Font=font;
				textbox.ForeColor=((SolidBrush)brush).Color;
				if(field.Height<textbox.Font.Height+2) { //Same logic as GraphicsHelper.CreateTextBoxForSheetDisplay().
					textbox.Multiline=false;
				}
				else {
					textbox.Multiline=true;
				}
				textbox.Text=str;
				Point[] positions=new Point[str.Length];
				for(int j=0;j<str.Length;j++) {
					positions[j]=textbox.GetPositionFromCharIndex(j); //This line is slow, so we try to minimize calling it by chaching positions each time there are changes.
				}
				textbox.Dispose();
				data=new object[] {positions,field.FontName,field.FontSize,field.FontIsBold,field.Width,field.Height,str,field.TextAlign};
				HashRtfStringCache[index.ToString()]=data;
			}
			Point[] charPositions=(Point[])data[0];
			if(PrefC.GetBool(PrefName.ImeCompositionCompatibility) && !IsWesternChars(str)) {
				//Only draw word by word if the user defined preference is set, and there is at least one character in the string that isn't a "Western" 
				//alphabet character.  Only use right-to-left formatting if the user's computer is set to a right-to-left culture.  This will preserve
				//desired formatting and spacing in as many scenarios as we can unless we later add a textbox specific setting for this type of formatting.
				DrawStringWordByWord(str,font,brush,field,CultureInfo.CurrentCulture.TextInfo.IsRightToLeft,charPositions,g);
			}
			else {
				DrawStringCharByChar(str,font,brush,field,charPositions,g);
			}
		}

		///<summary>Draws the string one char at a time, using specified positions for each char.</summary>
		private void DrawStringCharByChar(string str,Font font,Brush brush,SheetFieldDef field,Point[] charPositions,Graphics g) {
			//This will draw text below the bottom line if the text is long. This is by design, so the user can see that the text is too big.
			for(int j=0;j<charPositions.Length;j++) {
				g.DrawString(str.Substring(j,1),font,brush,field.Bounds.X+charPositions[j].X,field.Bounds.Y+charPositions[j].Y);
			}
		}

		///<summary>Draws the string one word at a time, using specified positions of the first char of each word.  This is important when a language is 
		///in use that uses words and/or characters composed from multiple characters, i.e. Arabic, Korean, etc.</summary>
		private void DrawStringWordByWord(string str,Font font,Brush brush,SheetFieldDef field,bool isRightToLeft,Point[] charPositions,Graphics g) {
			StringFormat format=new StringFormat();
			if(isRightToLeft) {
				format.FormatFlags|=StringFormatFlags.DirectionRightToLeft;
			}
			List<string> listWords=SplitStringByWhitespace(str);
			int posIndex=0;
			//This will draw text below the bottom line if the text is long. This is by design, so the user can see that the text is too big.
			foreach(string strCur in listWords) { 
				float xPos=field.Bounds.X+charPositions[posIndex].X;
				float yPos=field.Bounds.Y+charPositions[posIndex].Y;
				g.DrawString(strCur,font,brush,xPos,yPos,format);
				posIndex+=strCur.Length;
			}
		}

		/// <summary>Splits up a string into a list of "words" using whitespace as the delimiter.  The whitespace will be included as individual words if
		/// isWhitespaceIncluded is set to true.</summary>
		private List<string> SplitStringByWhitespace(string str,bool isWhitespaceIncluded=true) {
			List<string> listWords=new List<string>();
			StringBuilder strBuild=new StringBuilder();
			foreach(char c in str.ToCharArray()) {
				if(!char.IsWhiteSpace(c)) {//Includes tab character.
					strBuild.Append(c);
				}
				else {
					if(strBuild.Length>0) {//If we encounter consecutive white space characters in a row.
						listWords.Add(strBuild.ToString());
					}
					if(isWhitespaceIncluded) {
						listWords.Add(c.ToString());
					}
					strBuild.Clear();
				}
			}
			if(strBuild.Length>0) {
				listWords.Add(strBuild.ToString());//Add the last word.
			}
			return listWords;
		}

		private static bool IsWesternChars(string inputstring) {
			Regex regex=new Regex(@"[A-Za-z0-9 ?'.,-_=+(){}\[\]\\\t\r\n]");
			MatchCollection matches=regex.Matches(inputstring);
			if(matches.Count.Equals(inputstring.Length)) {
				return true;
			}
			return false;
		}
		#endregion

		private void butEdit_Click(object sender,EventArgs e) {
			FormSheetDef FormS=new FormSheetDef();
			FormS.SheetDefCur=_sheetDefCur;
			if(this.IsInternal) {
				FormS.IsReadOnly=true;
			}
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			textDescription.Text=_sheetDefCur.Description;
			//resize
			if(_sheetDefCur.IsLandscape) {
				panelMain.Width=_sheetDefCur.Height;
				panelMain.Height=_sheetDefCur.Width;
			}
			else {
				panelMain.Width=_sheetDefCur.Width;
				panelMain.Height=_sheetDefCur.Height;
			}
			panelMain.Height=_sheetDefCur.HeightTotal-(_sheetDefCur.PageCount==1?0:_sheetDefCur.PageCount*100-40);
			FillFieldList();
			RefreshDoubleBuffer();
			panelMain.Refresh();
			if(_sheetDefCur.HasMobileLayout) { //Always open the mobile editor since they want to use the mobile layout.
				sheetEditMobile.ShowHideModeless(true,_sheetDefCur.Description,this);
			}
		}

		///<summary>Change the show/hide state of the mobile designer. Open if it is currently closed or close if it is currently open.</summary>
		private void butMobile_Click(object sender,EventArgs e) {
			sheetEditMobile.ShowHideModeless(false,_sheetDefCur.Description,this);
		}

		///<summary>Use this to add a new item to SheetDefCur.SheetFieldDefs. The new item will be given a random primary key so it can be distinguished from other items.</summary>
		private void AddNewSheeFieldDef(SheetFieldDef sheetFieldDef) {
			sheetFieldDef.LayoutMode=_sheetLayoutModeCur;
			//This new key is only used for copy and paste function.
			//When this sheet is saved, all sheetfielddefs are deleted and reinserted, so the dummy PKs are harmless.
			//There's a VERY slight chance of PK duplication so try until we find an unused PK.
			int tries=0;
			do {
				sheetFieldDef.SheetFieldDefNum=_rand.Next(int.MaxValue);
			} while(_sheetDefCur.SheetFieldDefs.Any(x => x.SheetFieldDefNum==sheetFieldDef.SheetFieldDefNum) && ++tries<100);
			_sheetDefCur.SheetFieldDefs.Add(sheetFieldDef);
			sheetEditMobile.ScrollToSheetFieldDefNum=sheetFieldDef.SheetFieldDefNum;
		}

		///<summary>Handles near-universal logic for adding adding sheet field defs to the sheet. Does not handle Screen Chart, Special, and Grid.</summary>
		private void CreateSheetFieldDef(SheetFieldType fieldType) {
			Font font=new Font(_sheetDefCur.FontName,_sheetDefCur.FontSize);
			FormSheetFieldBase FormS;
			switch(fieldType) {
				case SheetFieldType.OutputText:
					FormS=new FormSheetFieldOutput(_sheetDefCur,SheetFieldDef.NewOutput("",_sheetDefCur.FontSize,_sheetDefCur.FontName,false,0,0,100,font.Height),IsInternal);
					break;
				case SheetFieldType.StaticText:
				default:
					FormS=new FormSheetFieldStatic(_sheetDefCur,SheetFieldDef.NewStaticText("",_sheetDefCur.FontSize,_sheetDefCur.FontName,false,0,0,100,font.Height),IsInternal);
					break;
				case SheetFieldType.InputField:
					FormS=new FormSheetFieldInput(_sheetDefCur,SheetFieldDef.NewInput("",_sheetDefCur.FontSize,_sheetDefCur.FontName,false,0,0,100,font.Height),IsInternal);
					break;
				case SheetFieldType.Image:
					FormS=new FormSheetFieldImage(_sheetDefCur,SheetFieldDef.NewImage("",0,0,100,100),IsInternal);
					break;
				case SheetFieldType.Line:
					FormS=new FormSheetFieldLine(_sheetDefCur,SheetFieldDef.NewLine(0,0,0,0),IsInternal);
					break;
				case SheetFieldType.Rectangle:
					FormS=new FormSheetFieldRect(_sheetDefCur,SheetFieldDef.NewRect(0,0,0,0),IsInternal);
					break;
				case SheetFieldType.CheckBox:
					FormS=new FormSheetFieldCheckBox(_sheetDefCur,SheetFieldDef.NewCheckBox("",0,0,11,11),IsInternal);
					break;
				case SheetFieldType.ComboBox:
					FormS=new FormSheetFieldComboBox(_sheetDefCur,SheetFieldDef.NewComboBox("","",0,0),IsInternal);
					break;
				case SheetFieldType.SigBox:
				case SheetFieldType.SigBoxPractice:
					FormS=new FormSheetFieldSigBox(_sheetDefCur,SheetFieldDef.NewSigBox(0,0,364,81,fieldType),IsInternal);
					break;
				case SheetFieldType.PatImage:
					FormS=new FormSheetFieldPatImage(_sheetDefCur,SheetFieldDef.NewPatImage(0,0,100,100),IsInternal);
					break;
			}
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK  || FormS.SheetFieldDefCur==null) {//SheetFieldDefCur==null if it was Deleted
				return;
			}
			AddNewSheeFieldDef(FormS.SheetFieldDefCur);
			FillFieldList();
			if(fieldType==SheetFieldType.Image) {
				RefreshDoubleBuffer();
			}
			panelMain.Refresh();
		}

		private void butAddOutputText_Click(object sender,EventArgs e) {
			if(SheetFieldsAvailable.GetList(_sheetDefCur.SheetType,OutInCheck.Out).Count==0) {
				MsgBox.Show(this,"There are no output fields available for this type of sheet.");
				return;
			}
			CreateSheetFieldDef(SheetFieldType.OutputText);
		}

		private void butAddStaticText_Click(object sender,EventArgs e) {
			CreateSheetFieldDef(SheetFieldType.StaticText);
		}

		private void butAddInputField_Click(object sender,EventArgs e) {
			if(SheetFieldsAvailable.GetList(_sheetDefCur.SheetType,OutInCheck.In).Count==0) {
				MsgBox.Show(this,"There are no input fields available for this type of sheet.");
				return;
			}
			CreateSheetFieldDef(SheetFieldType.InputField);
		}

		private void butAddImage_Click(object sender,EventArgs e) {
			if(PrefC.AtoZfolderUsed==DataStorageType.InDatabase) {
				MsgBox.Show(this,"Not allowed because not using AtoZ folder");
				return;
			}
			CreateSheetFieldDef(SheetFieldType.Image);
		}

		private void butAddLine_Click(object sender,EventArgs e) {
			CreateSheetFieldDef(SheetFieldType.Line);
		}

		private void butAddRect_Click(object sender,EventArgs e) {
			CreateSheetFieldDef(SheetFieldType.Rectangle);
		}

		private void butAddCheckBox_Click(object sender,EventArgs e) {
			if(SheetFieldsAvailable.GetList(_sheetDefCur.SheetType,OutInCheck.Check).Count==0) {
				MsgBox.Show(this,"There are no checkbox fields available for this type of sheet.");
				return;
			}
			CreateSheetFieldDef(SheetFieldType.CheckBox);
		}

		private void butAddCombo_Click(object sender,EventArgs e) {
			CreateSheetFieldDef(SheetFieldType.ComboBox);
		}

		private void butScreenChart_Click(object sender,EventArgs e) {
			string fieldValue="0;d,m,ling;d,m,ling;,,;,,;,,;,,;m,d,ling;m,d,ling;m,d,buc;m,d,buc;,,;,,;,,;,,;d,m,buc;d,m,buc";
			if(!_hasChartSealantComplete) {
				AddNewSheeFieldDef(SheetFieldDef.NewScreenChart("ChartSealantComplete",fieldValue,0,0));
			}
			else if(!_hasChartSealantTreatment) {
				AddNewSheeFieldDef(SheetFieldDef.NewScreenChart("ChartSealantTreatment",fieldValue,0,0));
			}
			else {
				MsgBox.Show(this,"Only two charts are allowed per screening sheet.");
				return;
			}
			FillFieldList();
			panelMain.Refresh();
		}

		private void butAddSigBox_Click(object sender,EventArgs e) {
			CreateSheetFieldDef(SheetFieldType.SigBox);
		}

		private void butAddSigBoxPractice_Click(object sender,EventArgs e) {
			CreateSheetFieldDef(SheetFieldType.SigBoxPractice);
		}

		private void butAddSpecial_Click(object sender,EventArgs e) {
			FormSheetFieldSpecial FormSFS=new FormSheetFieldSpecial(_sheetDefCur,new SheetFieldDef() { IsNew=true },IsInternal,_sheetLayoutModeCur);
			FormSFS.ShowDialog();
			if(FormSFS.DialogResult!=DialogResult.OK) {
				return;
			}
			if(GetIsDynamicSheetType() && GetSheetFieldDefsForLayoutModeCur().Any(x => x.FieldName==FormSFS.SheetFieldDefCur.FieldName)) {
				MsgBox.Show(this,"Field already exists.");
				return;
			}
			AddNewSheeFieldDef(FormSFS.SheetFieldDefCur);
			FillFieldList();
			panelMain.Refresh();
		}

		private void butAddPatImage_Click(object sender,EventArgs e) {
			if(PrefC.AtoZfolderUsed==DataStorageType.InDatabase) {
				MsgBox.Show(this,"Not allowed because not using AtoZ folder");
				return;
			}
			CreateSheetFieldDef(SheetFieldType.PatImage);
		}

		private void butAddGrid_Click(object sender,EventArgs e) {
			SheetFieldDef grid=SheetFieldDef.NewGrid(DashApptGrid.SheetFieldName,0,0,100,150,growthBehavior:GrowthBehaviorEnum.None);
			if(!SheetDefs.IsDashboardType(_sheetDefCur)) {
				FormSheetFieldGridType FormT=new FormSheetFieldGridType();
				FormT.SheetDefCur=_sheetDefCur;
				FormT.LayoutMode=_sheetLayoutModeCur;
				FormT.ShowDialog();
				if(FormT.DialogResult!=DialogResult.OK) {
					return;
				}
				grid=SheetFieldDef.NewGrid(FormT.SelectedSheetGridType,0,0,100,100); //is resized from dialog window.
				if(GetIsDynamicSheetType()) { //Grids dimmensions in dynamic sheetDefs should be static by default because easieast to understand.
					grid.GrowthBehavior=GrowthBehaviorEnum.None;
				}
			}
			FormSheetFieldGrid FormS=new FormSheetFieldGrid(_sheetDefCur,grid,IsInternal);
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK  || FormS.SheetFieldDefCur==null) {//SheetFieldDefCur==null if it was Deleted
				return;
			}
			if(GetIsDynamicSheetType() && GetSheetFieldDefsForLayoutModeCur().Any(x => x.FieldName==FormS.SheetFieldDefCur.FieldName)) {
				MsgBox.Show(this,"Grid already exists.");
				return;
			}
			AddNewSheeFieldDef(FormS.SheetFieldDefCur);
			FillFieldList();
			panelMain.Refresh();
		}
		
		private void butAddMobileHeader_Click(object sender,EventArgs e) {
			InputBox FormIB=new InputBox(Lan.g(this,"Mobile Header")) {
				MaxInputTextLength=255,
				ShowDelete=true,
			};
			if(FormIB.ShowDialog()!=DialogResult.OK || FormIB.IsDeleteClicked) {
				return;
			}
			AddNewSheeFieldDef(SheetFieldDef.NewMobileHeader(FormIB.textResult.Text));
			FillFieldList();
			panelMain.Refresh();
		}

		private void sheetEditMobile_SheetDefSelected(object sender,long sheetFieldDefNum) {
			listFields.ClearSelected();
			GetSheetFieldDefsForLayoutModeCur()
				.Select((x,y) => new { sfd = x,index = y,})
				.Where(x => x.sfd.SheetFieldDefNum==sheetFieldDefNum)
				.ForEach(x => listFields.SetSelected(x.index,true));
			panelMain.Refresh();
		}

		///<summary>Returns all sheetFieldDefs from sheetDef for the current layout mode, sheetDef defaults to _sheetDefCur.</summary>
		private List<SheetFieldDef> GetSheetFieldDefsForLayoutModeCur(SheetDef sheetDef=null) {
			if(sheetDef==null) {
				sheetDef=_sheetDefCur;
			}
			return sheetDef.SheetFieldDefs.FindAll(x => x.LayoutMode==_sheetLayoutModeCur);
		}
		
		private void listFields_SelectedIndexChanged(object sender,EventArgs e) {
			sheetEditMobile.SetHighlightedFieldDefs(
				GetSheetFieldDefsForLayoutModeCur()
				.Select((x,y) => new { sheetFieldDef = x,index = y })
				.Where(x => listFields.SelectedIndices.Contains(x.index))
				.Select(x => x.sheetFieldDef.SheetFieldDefNum).ToList());
			panelMain.Refresh();
		}

		private void listFields_MouseDoubleClick(object sender,MouseEventArgs e) {
			int idx=listFields.IndexFromPoint(e.Location);
			if(idx==-1) {
				return;
			}
			listFields.SelectedIndices.Clear();
			listFields.SetSelected(idx,true);
			panelMain.Refresh();
			SheetFieldDef field=GetSheetFieldDefsForLayoutModeCur()[idx];
			SheetFieldDef fieldold=field.Copy();
			LaunchEditWindow(field,isEditMobile:false);
			if(field.TabOrder!=fieldold.TabOrder) { //otherwise a different control will be selected.
				listFields.SelectedIndices.Clear();
			}
		}

		///<summary>Only for editing fields that already exist.</summary>
		private void LaunchEditWindow(SheetFieldDef field,bool isEditMobile) {
			//not every field will have been saved to the database, so we can't depend on SheetFieldDefNum.
			int idx=_sheetDefCur.SheetFieldDefs.IndexOf(field);
			FormSheetFieldBase FormS=null;
			switch(field.FieldType) {
				case SheetFieldType.InputField:
					FormS=new FormSheetFieldInput(_sheetDefCur,field,IsInternal,isEditMobile);
					break;
				case SheetFieldType.OutputText:
					FormS=new FormSheetFieldOutput(_sheetDefCur,field,IsInternal,isEditMobile);
					break;
				case SheetFieldType.StaticText:
				default:
					FormS=new FormSheetFieldStatic(_sheetDefCur,field,IsInternal,isEditMobile);
					break;
				case SheetFieldType.Image:
					FormS=new FormSheetFieldImage(_sheetDefCur,field,IsInternal);  //available for mobile but all fields are relevant
					break;
				case SheetFieldType.PatImage:
					FormS=new FormSheetFieldPatImage(_sheetDefCur,field,IsInternal);  //available for mobile but all fields are relevant
					break;
				case SheetFieldType.Line:
					FormS=new FormSheetFieldLine(_sheetDefCur,field,IsInternal); //available for mobile but all fields are relevant
					break;
				case SheetFieldType.Rectangle:
					FormS=new FormSheetFieldRect(_sheetDefCur,field,IsInternal); //available for mobile but all fields are relevant
					break;
				case SheetFieldType.CheckBox:
					FormS=new FormSheetFieldCheckBox(_sheetDefCur,field,IsInternal,isEditMobile);
					break;
				case SheetFieldType.ComboBox:
					FormS=new FormSheetFieldComboBox(_sheetDefCur,field,IsInternal,isEditMobile);
					break;
				case SheetFieldType.SigBox:
				case SheetFieldType.SigBoxPractice:
					FormS=new FormSheetFieldSigBox(_sheetDefCur,field,IsInternal,isEditMobile);
					break;
				case SheetFieldType.Special:
					FormS=new FormSheetFieldSpecial(_sheetDefCur,field,IsInternal,_sheetLayoutModeCur); //not available for mobile
					break;
				case SheetFieldType.Grid:
					FormS=new FormSheetFieldGrid(_sheetDefCur,field,IsInternal); //not available for mobile
					break;
				case SheetFieldType.ScreenChart:
					FormS=new FormSheetFieldChart(_sheetDefCur,field,IsInternal); //not available for mobile
					break;
				case SheetFieldType.MobileHeader: //This is the one weird/different one
					InputBox FormIB=new InputBox(Lan.g(this,"Mobile Header"),field.UiLabelMobile) {
						MaxInputTextLength=255,
						ShowDelete=true,
					};
					if(FormIB.ShowDialog()!=DialogResult.OK) {
						return;
					}
					if(FormIB.IsDeleteClicked) {
						_sheetDefCur.SheetFieldDefs.RemoveAt(idx);//Deleted
					}
					else {
						field.UiLabelMobile=FormIB.textResult.Text;
					}
					break;
			}
			if(FormS!=null) { //in case MobileHeader was used instead
				FormS.ShowDialog();
				if(FormS.DialogResult!=DialogResult.OK) {
					return;
				}
				if(FormS.SheetFieldDefCur==null) {
					if(field.FieldType==SheetFieldType.Image) {
						_sheetDefCur.SheetFieldDefs[idx].ImageField?.Dispose();
					}
					_sheetDefCur.SheetFieldDefs.RemoveAt(idx);
				}
			}
			if(IsTabMode) {
				if(ListSheetFieldDefsTabOrder.Contains(field)) {
					ListSheetFieldDefsTabOrder.RemoveAt(ListSheetFieldDefsTabOrder.IndexOf(field));
				}
				if(field.TabOrder>0 && field.TabOrder<=(ListSheetFieldDefsTabOrder.Count+1)) {
					ListSheetFieldDefsTabOrder.Insert(field.TabOrder-1,field);
				}
				RenumberTabOrderHelper();
				return;
			}
			FillFieldList();
			if(field.FieldType==SheetFieldType.Image) { //Only when image was edited (pressing cancel in FormSheetFieldImage would've returned)
				RefreshDoubleBuffer();
			}
			if(listFields.Items.Count-1>=idx) {//Panel will refresh either way
				listFields.SetSelectedItem<SheetFieldDef>((sheetFieldDef) => sheetFieldDef==field);
			}
			else {
				panelMain.Refresh();
			}
		}

		private void panelMain_MouseDown(object sender,MouseEventArgs e) {
			splitContainer1.Panel1.Select();
			if(AltIsDown) {
				PasteControlsFromMemory(e.Location);
				return;
			}
			MouseIsDown=true;
			ClickedOnBlankSpace=false;
			MouseOriginalPos=e.Location;
			MouseCurrentPos=e.Location;
			SheetFieldDef field=HitTest(e.X,e.Y);
			if(IsTabMode) {
				MouseIsDown=false;
				CtrlIsDown=false;
				AltIsDown=false;
				if(field==null
					//Some of the fields below are redundant and should never be returned from HitTest but are here to explicity exclude them.
				   || field.FieldType==SheetFieldType.Drawing || field.FieldType==SheetFieldType.Image || field.FieldType==SheetFieldType.Line || field.FieldType==SheetFieldType.OutputText || field.FieldType==SheetFieldType.Parameter || field.FieldType==SheetFieldType.PatImage || field.FieldType==SheetFieldType.Rectangle || field.FieldType==SheetFieldType.StaticText) {
					return;
				}
				if(ListSheetFieldDefsTabOrder.Contains(field)) {
					field.TabOrder=0;
					ListSheetFieldDefsTabOrder.RemoveAt(ListSheetFieldDefsTabOrder.IndexOf(field));
				}
				else {
					ListSheetFieldDefsTabOrder.Add(field);
				}
				RenumberTabOrderHelper();
				return;
			}
			if(field==null) {
				ClickedOnBlankSpace=true;
				if(CtrlIsDown) {
					return; //so that you can add more to the previous selection
				}
				listFields.SelectedIndices.Clear(); //clear the existing selection
				panelMain.Refresh();
				return;
			}
			List<SheetFieldDef> listSheetFieldDefs=GetSheetFieldDefsForLayoutModeCur();
			int idx=listSheetFieldDefs.IndexOf(field);
			if(CtrlIsDown) {
				if(listFields.SelectedIndices.Contains(idx)) {
					listFields.SetSelected(idx,false);
				}
				else {
					listFields.SetSelected(idx,true);
				}
			}
			else { //Ctrl not down
				if(listFields.SelectedIndices.Contains(idx)) {
					//clicking on the group, probably to start a drag.
				}
				else {
					listFields.SelectedIndices.Clear();
					listFields.SetSelected(idx,true);
				}
			}
			OriginalControlPositions=new List<Point>();
			Point point;
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				point=new Point(listSheetFieldDefs[listFields.SelectedIndices[i]].XPos,listSheetFieldDefs[listFields.SelectedIndices[i]].YPos);
				OriginalControlPositions.Add(point);
			}
			panelMain.Refresh();
		}

		private void panelMain_MouseMove(object sender,MouseEventArgs e) {
			if(!MouseIsDown) {
				return;
			}
			if(IsInternal) {
				return;
			}
			if(IsTabMode) {
				return;
			}
			if(ClickedOnBlankSpace) {
				MouseCurrentPos=e.Location;
				panelMain.Refresh();
				return;
			}
			List<SheetFieldDef> listSheetFieldDefs=GetSheetFieldDefsForLayoutModeCur();
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				listSheetFieldDefs[listFields.SelectedIndices[i]].XPos=OriginalControlPositions[i].X+e.X-MouseOriginalPos.X;
				listSheetFieldDefs[listFields.SelectedIndices[i]].YPos=OriginalControlPositions[i].Y+e.Y-MouseOriginalPos.Y;
			}
			panelMain.Refresh();
		}

		private void panelMain_MouseUp(object sender,MouseEventArgs e) {
			MouseIsDown=false;
			OriginalControlPositions=null;
			List<SheetFieldDef> listSheetFieldDefs=GetSheetFieldDefsForLayoutModeCur();
			if(ClickedOnBlankSpace) { //if initial mouse down was not on a control.  ie, if we are dragging to select.
				Rectangle selectionBounds=new Rectangle(Math.Min(MouseOriginalPos.X,MouseCurrentPos.X), //X
					Math.Min(MouseOriginalPos.Y,MouseCurrentPos.Y), //Y
					Math.Abs(MouseCurrentPos.X-MouseOriginalPos.X), //Width
					Math.Abs(MouseCurrentPos.Y-MouseOriginalPos.Y)); //Height
				for(int i=0;i<listSheetFieldDefs.Count;i++) {
					SheetFieldDef tempDef=listSheetFieldDefs[i]; //to speed this process up instead of referencing the array every time.
					if(tempDef.FieldType==SheetFieldType.Line || tempDef.FieldType==SheetFieldType.Image) {
						continue; //lines and images are currently not selectable by drag and drop. will require lots of calculations, completely possible, but complex.
					}
					//If the selection is contained within the "hollow" portion of the rectangle, it shouldn't be selected.
					if(tempDef.FieldType==SheetFieldType.Rectangle) {
						Rectangle tempDefBounds=new Rectangle(tempDef.Bounds.X+4,tempDef.Bounds.Y+4,tempDef.Bounds.Width-8,tempDef.Bounds.Height-8);
						if(tempDefBounds.Contains(selectionBounds)) {
							continue;
						}
					}
					if(tempDef.BoundsF.IntersectsWith(selectionBounds)) {
						listFields.SetSelected(i,true); //Add to selected indicies
					}
				}
			}
			else if(_autoSnapDistance>0){//Attempt to autosnap field to nearest cell top left corner.
				for(int i=0;i<listFields.SelectedIndices.Count;i++) {
					SheetFieldDef def=listSheetFieldDefs[listFields.SelectedIndices[i]];
					def.XPos=_listSnapXVals.LastOrDefault(x => x<=def.XPos);
					def.YPos=_listSnapYVals.LastOrDefault(y => y<=def.YPos);
				}
			}
			ClickedOnBlankSpace=false;
			panelMain.Refresh();
		}

		private void panelMain_MouseDoubleClick(object sender,MouseEventArgs e) {
			if(AltIsDown) {
				return;
			}
			SheetFieldDef field=HitTest(e.X,e.Y);
			if(field==null) {
				return;
			}
			SheetFieldDef fieldold=field.Copy();
			LaunchEditWindow(field,isEditMobile:false);
			//if(field.TabOrder!=fieldold.TabOrder) {
			//  listFields.SelectedIndices.Clear();
			//}
			//if(isTabMode) {
			//  if(ListSheetFieldDefsTabOrder.Contains(field)){
			//    ListSheetFieldDefsTabOrder.RemoveAt(ListSheetFieldDefsTabOrder.IndexOf(field));
			//  }
			//  if(field.TabOrder>0 && field.TabOrder<ListSheetFieldDefsTabOrder.Count+1) {
			//    ListSheetFieldDefsTabOrder.Insert(field.TabOrder-1,field);
			//  }
			//  RenumberTabOrderHelper();
			//}
		}

		private void panelMain_Resize(object sender,EventArgs e) {
			if(BmBackground!=null && panelMain.Size==BmBackground.Size) {
				return;
			}
			if(GraphicsBackground!=null) {
				GraphicsBackground.Dispose();
			}
			if(BmBackground!=null) {
				BmBackground.Dispose();
			}
			BmBackground=new Bitmap(panelMain.Width,panelMain.Height);
			GraphicsBackground=Graphics.FromImage(BmBackground);
			panelMain.Refresh();
		}

		///<summary>Used To renumber TabOrder on controls</summary>
		private void RenumberTabOrderHelper() {
			for(int i=0;i<ListSheetFieldDefsTabOrder.Count;i++) {
				ListSheetFieldDefsTabOrder[i].TabOrder=i+1; //Start number tab order at 1
			}
			FillFieldList();
			panelMain.Refresh();
		}

		///<summary>Images will be ignored in the hit test since they frequently fill the entire background.  Lines will be ignored too, since a diagonal line could fill a large area.</summary>
		private SheetFieldDef HitTest(int x,int y) {
			List<SheetFieldDef> listSheetFieldDefs=GetSheetFieldDefsForLayoutModeCur();
			//Loop from the back of the list since those sheetfielddefs were added last, so they show on top of items at beginning of list.
			for(int i=listSheetFieldDefs.Count-1;i>=0;i--) {
				SheetFieldDef sheetFieldDef=listSheetFieldDefs[i];
				if(sheetFieldDef.FieldType.In(SheetFieldType.Image,SheetFieldType.Line)) {
					continue;
				}
				Rectangle fieldDefBounds=sheetFieldDef.Bounds;
				if(fieldDefBounds.Contains(x,y)) {
					//Center of the rectangle will not be considered a hit.
					if(sheetFieldDef.FieldType==SheetFieldType.Rectangle && new Rectangle(fieldDefBounds.X+4,fieldDefBounds.Y+4,fieldDefBounds.Width-8,fieldDefBounds.Height-8).Contains(x,y)) {
						continue;
					}
					return sheetFieldDef;
				}
			}
			return null;
		}

		private void FormSheetDefEdit_KeyDown(object sender,KeyEventArgs e) {
			List<SheetFieldDef> listSheetFieldDefs=listFields.GetListSelected<SheetFieldDef>();
			bool refreshBuffer=false;
			e.Handled=true;
			if(e.KeyCode==Keys.ControlKey && CtrlIsDown) {
				return;
			}
			if(IsInternal) {
				return;
			}
			if(e.Control) {
				CtrlIsDown=true;
			}
			if(CtrlIsDown && e.KeyCode==Keys.C) { //CTRL-C
				CopyControlsToMemory();
			}
			else if(CtrlIsDown && e.KeyCode==Keys.V) { //CTRL-V
				PasteControlsFromMemory(new Point(0,0));
			}
			else if(e.Alt) {
				Cursor=Cursors.Cross; //change cursor to rubber stamp cursor
				AltIsDown=true;
			}
			else if(e.KeyCode==Keys.Delete || e.KeyCode==Keys.Back) {
				if(listFields.SelectedIndices.Count==0) {
					return;
				}
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete selected fields?")) {
					return;
				}
				for(int i=listSheetFieldDefs.Count-1;i>=0;i--) { //iterate backwards through list
					SheetFieldDef fieldI=listSheetFieldDefs[i];
					if(fieldI.FieldType==SheetFieldType.Image) {
						refreshBuffer=true;
					}
					if(fieldI.FieldType==SheetFieldType.Grid && fieldI.FieldName=="TreatPlanMain"
						&& _sheetDefCur.SheetFieldDefs.FindAll(x=>x.FieldType==SheetFieldType.Grid && x.FieldName=="TreatPlanMain").Count==1) 
					{
						MsgBox.Show(this,"Cannot delete the last main grid from treatment plan.");
						continue;//skip this one.
					}
					if(fieldI.FieldType==SheetFieldType.ScreenChart) {
						if(fieldI.FieldName=="ChartSealantComplete") {
							_hasChartSealantComplete=false;
						}
						if(fieldI.FieldName=="ChartSealantTreatment") {
							_hasChartSealantTreatment=false;
						}
					}
					_sheetDefCur.SheetFieldDefs.Remove(fieldI);
				}
				FillFieldList();
			}
			foreach(SheetFieldDef fieldDef in listSheetFieldDefs) {
				if(fieldDef.FieldType==SheetFieldType.Image) {
					refreshBuffer=true;
				}
				switch(e.KeyCode) {
					case Keys.Up:
						if(e.Shift) {
							fieldDef.YPos-=7;
						}
						else {
							fieldDef.YPos--;
						}
						break;
					case Keys.Down:
						if(e.Shift) {
							fieldDef.YPos+=7;
						}
						else {
							fieldDef.YPos++;
						}
						break;
					case Keys.Left:
						if(e.Shift) {
							fieldDef.XPos-=7;
						}
						else {
							fieldDef.XPos--;
						}
						break;
					case Keys.Right:
						if(e.Shift) {
							fieldDef.XPos+=7;
						}
						else {
							fieldDef.XPos++;
						}
						break;
					default:
						break;
				}
			}
			if(refreshBuffer) { //Only when an image was selected.
				RefreshDoubleBuffer();
			}
			panelMain.Refresh();
		}

		private void FormSheetDefEdit_KeyUp(object sender,KeyEventArgs e) {
			if((e.KeyCode&Keys.ControlKey)==Keys.ControlKey) {
				CtrlIsDown=false;
			}
			if(!e.Alt) {
				Cursor=Cursors.Default;
				AltIsDown=false;
			}
		}

		private void CopyControlsToMemory() {
			if(IsTabMode) {
				return;
			}
			if(listFields.SelectedIndices.Count==0) {
				return;
			}
			List<SheetFieldDef> listSheetFieldDefs=GetSheetFieldDefsForLayoutModeCur();
			string strPrompt=Lan.g(this,"The following selected fields can cause conflicts if they are copied:\r\n");
			bool conflictingfield=false;
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				SheetFieldDef fielddef=listSheetFieldDefs[listFields.SelectedIndices[i]];
				switch(fielddef.FieldType) {
					case SheetFieldType.Drawing:
					case SheetFieldType.Image:
					case SheetFieldType.Line:
					case SheetFieldType.PatImage:
					//case SheetFieldType.Parameter://would not be seen on the sheet.
					case SheetFieldType.Rectangle:
					case SheetFieldType.SigBox:
					case SheetFieldType.SigBoxPractice:
					case SheetFieldType.StaticText:
					case SheetFieldType.ComboBox:
						break; //it will always be ok to copy the types of fields above.
					case SheetFieldType.CheckBox:
						if(fielddef.FieldName!="misc") { //custom fields should be okay to copy
							strPrompt+=fielddef.FieldName+"."+fielddef.RadioButtonValue+"\r\n";
							conflictingfield=true;
						}
						break;
					case SheetFieldType.InputField:
					case SheetFieldType.OutputText:
						if(fielddef.FieldName!="misc") { //custom fields should be okay to copy
							strPrompt+=fielddef.FieldName+"\r\n";
							conflictingfield=true;
						}
						break;
				}
			}
			strPrompt+=Lan.g(this,"Would you like to continue anyways?");
			if(conflictingfield && MessageBox.Show(strPrompt,Lan.g(this,"Warning"),MessageBoxButtons.OKCancel)!=DialogResult.OK) {
				splitContainer1.Panel1.Select();
				CtrlIsDown=false;
				return;
			}
			ListSheetFieldDefsCopyPaste=new List<SheetFieldDef>(); //empty the remembered field list
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				ListSheetFieldDefsCopyPaste.Add(listSheetFieldDefs[listFields.SelectedIndices[i]].Copy()); //fill clipboard with copies of the controls. 
				//It would probably be safe to fill the clipboard with the originals. but it is safer to fill it with copies.
			}
			PasteOffset=0;
			PasteOffsetY=0; //reset PasteOffset for pasting a new set of fields.
		}

		private void PasteControlsFromMemory(Point origin) {
			if(IsTabMode) {
				return;
			}
			if(GetIsDynamicSheetType() && ListSheetFieldDefsCopyPaste!=null) {
				//Only paste controls that are valid for _sheetFieldLayoutModeCur
				List<SheetFieldDef> listSheetFieldDefs=GetSheetFieldDefsForLayoutModeCur();
				List<string> listGridNames=SheetUtil.GetGridsAvailable(_sheetDefCur.SheetType,_sheetLayoutModeCur);
				List<SheetFieldDef> listSpecial=SheetFieldsAvailable.GetSpecial(_sheetDefCur.SheetType,_sheetLayoutModeCur);
				ListSheetFieldDefsCopyPaste.RemoveAll(
					x =>  listSheetFieldDefs.Any(y => y.FieldName==x.FieldName) //Remove duplicates
					|| (x.FieldType==SheetFieldType.Grid && !listGridNames.Any(y => y==x.FieldName))//Remove invalid grid from paste logic
					|| (x.FieldType==SheetFieldType.Special && !listSpecial.Any(y => y.FieldName==x.FieldName))//Remove invalid special fields from paste logic.
				);
			}
			if(ListSheetFieldDefsCopyPaste==null || ListSheetFieldDefsCopyPaste.Count==0) {
				return;
			}
			if(origin.X==0 && origin.Y==0) { //allows for cascading pastes in the upper right hand corner.
				Rectangle r=panelMain.Bounds; //Gives relative position of panel (scroll position)
				int h=splitContainer1.Panel1.Height; //Current resized height/width of parent panel
				int w=splitContainer1.Panel1.Width;
				int maxH=0;
				int maxW=0;
				for(int i=0;i<ListSheetFieldDefsCopyPaste.Count;i++) { //calculate height/width of control to be pasted
					maxH=Math.Max(maxH,ListSheetFieldDefsCopyPaste[i].Height);
					maxW=Math.Max(maxW,ListSheetFieldDefsCopyPaste[i].Width);
				}
				origin=new Point((-1)*r.X+w/2-maxW/2-10,(-1)*r.Y+h/2-maxH/2-10); //Center: scroll position * (-1) + 1/2 size of window - 1/2 the size of the field - 10 for scroll bar
				origin.X+=PasteOffset;
				origin.Y+=PasteOffset+PasteOffsetY;
			}
			listFields.ClearSelected();
			int minX=int.MaxValue;
			int minY=int.MaxValue;
			for(int i=0;i<ListSheetFieldDefsCopyPaste.Count;i++) { //calculate offset
				minX=Math.Min(minX,ListSheetFieldDefsCopyPaste[i].XPos);
				minY=Math.Min(minY,ListSheetFieldDefsCopyPaste[i].YPos);
			}
			List<SheetFieldDef> listNewSheetFieldDefs=new List<SheetFieldDef>();
			for(int i=0;i<ListSheetFieldDefsCopyPaste.Count;i++) { //create new controls
				SheetFieldDef fielddef=ListSheetFieldDefsCopyPaste[i].Copy();
				fielddef.XPos=fielddef.XPos-minX+origin.X;
				fielddef.YPos=fielddef.YPos-minY+origin.Y;
				AddNewSheeFieldDef(fielddef);
				listNewSheetFieldDefs.Add(fielddef);
			}
			if(!AltIsDown) {
				PasteOffsetY+=((PasteOffset+10)/100)*10; //this will shift the pastes down 10 pixels every 10 pastes.
				PasteOffset=(PasteOffset+10)%100; //cascades and allows for 90 consecutive pastes without overlap
			}
			FillFieldList();
			//Select newly pasted controls.
			GetSheetFieldDefsForLayoutModeCur()
				.Select((Item,Index) => new { Item,Index })
				.Where(x => listNewSheetFieldDefs.Any(y => y.SheetFieldDefNum==x.Item.SheetFieldDefNum))
				.ForEach(x => { listFields.SetSelected(x.Index,true); });
			panelMain.Refresh();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsTabMode) {
				return;
			}
			if(_sheetDefCur.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(GetIsDynamicSheetType() && _sheetDefCur.SheetDefNum==PrefC.GetLong(PrefName.SheetsDefaultChartModule)) {
				MsgBox.Show(this,"This is the current Chart module default layout.\r\nPlease select a new default in Sheet Def Defaults first.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete entire sheet?")) {
				return;
			}
			try {
				SheetDefs.DeleteObject(_sheetDefCur.SheetDefNum);
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,Lan.g(this,"SheetDef")+" "+_sheetDefCur.Description+" "+Lan.g(this,"was deleted."));
				DialogResult=DialogResult.OK;
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private bool VerifyDesign() {
			//Keep a temporary list of every medical input and check box so it saves time checking for duplicates.
			List<SheetFieldDef> medChkBoxList=new List<SheetFieldDef>();
			List<SheetFieldDef> inputMedList=new List<SheetFieldDef>();
			//Verify radio button groups.
			for(int i=0;i<_sheetDefCur.SheetFieldDefs.Count;i++) {
				SheetFieldDef field=_sheetDefCur.SheetFieldDefs[i];
				if(field.FieldType==SheetFieldType.CheckBox && field.IsRequired && (field.RadioButtonGroup!="" //for misc radio groups
				                                                                    || field.RadioButtonValue!="")) //for built-in radio groups
				{
					//All radio buttons within a group must either all be marked required or all be marked not required. 
					//Not the most efficient check, but there won't usually be more than a few hundred items so the user will not ever notice. We can speed up later if needed.
					for(int j=0;j<_sheetDefCur.SheetFieldDefs.Count;j++) {
						SheetFieldDef field2=_sheetDefCur.SheetFieldDefs[j];
						if(field2.FieldType==SheetFieldType.CheckBox && !field2.IsRequired && field2.RadioButtonGroup.ToLower()==field.RadioButtonGroup.ToLower() //for misc groups
						   && field2.FieldName.ToLower()==field.FieldName.ToLower()) //for misc groups
						{
							MessageBox.Show(Lan.g(this,"Radio buttons in radio button group")+" '"+(field.RadioButtonGroup==""?field.FieldName:field.RadioButtonGroup)+"' "+Lan.g(this,"must all be marked required or all be marked not required."));
							return false;
						}
					}
				}
				if(field.FieldType==SheetFieldType.CheckBox && (field.FieldName.StartsWith("allergy:")) || field.FieldName.StartsWith("checkMed") || field.FieldName.StartsWith("problem:")) {
					for(int j=0;j<medChkBoxList.Count;j++) { //Check for duplicates.
						if(medChkBoxList[j].FieldName==field.FieldName && medChkBoxList[j].RadioButtonValue==field.RadioButtonValue) {
							MessageBox.Show(Lan.g(this,"Duplicate check box found")+": '"+field.FieldName+" "+field.RadioButtonValue+"'. "+Lan.g(this,"Only one of each type is allowed."));
							return false;
						}
					}
					//Not a duplicate so add it to the med chk box list.
					medChkBoxList.Add(field);
				}
				else if(field.FieldType==SheetFieldType.InputField && field.FieldName.StartsWith("inputMed")) {
					for(int j=0;j<inputMedList.Count;j++) {
						if(inputMedList[j].FieldName==field.FieldName) {
							MessageBox.Show(Lan.g(this,"Duplicate inputMed boxes found")+": '"+field.FieldName+"'. "+Lan.g(this,"Only one of each is allowed."));
							return false;
						}
					}
					inputMedList.Add(field);
				}
			}
			switch(_sheetDefCur.SheetType) {
				case SheetTypeEnum.TreatmentPlan:
					if(_sheetDefCur.SheetFieldDefs.FindAll(x => x.FieldType==SheetFieldType.SigBox).Count!=1) {
						MessageBox.Show(Lan.g(this,"Treatment plans must have exactly one patient signature box."));
						return false;
					}
					if(_sheetDefCur.SheetFieldDefs.Count(x => x.FieldType==SheetFieldType.SigBoxPractice)>1) {
						MessageBox.Show(Lan.g(this,"Treatment plans cannot have more than one practice signature box."));
						return false;
					}
					if(_sheetDefCur.SheetFieldDefs.FindAll(x => x.FieldType==SheetFieldType.Grid && x.FieldName=="TreatPlanMain").Count<1) {
						MessageBox.Show(Lan.g(this,"Treatment plans must have one main grid."));
						return false;
					}
					break;
			}
			return true;
		}

		///<summary>Updates the web sheet defs linked to this sheet def if the user agrees. Returns true if this sheet is okay to be saved to the 
		///database.</summary>
		private bool UpdateWebSheetDef() {
			if(_sheetDefCur.IsNew) {
				//There is no Web Form to sync because they just created this sheet def.
				//Without this return we would show the user that this Sheet Def matches all web forms that are not yet linked to a valid Sheet Def.
				return true;
			}
			Cursor=Cursors.WaitCursor;
			while(!_hasSheetsDownloaded) {//The thread has not finished getting the list. 
				Application.DoEvents();
				Thread.Sleep(100);
			}
			Cursor=Cursors.Default;
			if(_listWebSheetDefs==null || _listWebSheetDefs.Count==0) {//No web forms use this sheet def.
				return true;
			}
			string message=Lan.g(this,"This Sheet Def is used by the following web "+(_listWebSheetDefs.Count==1?"form":"forms"))+":\r\n"
				+string.Join("\r\n",_listWebSheetDefs.Select(x => x.Description))+"\r\n"
				+Lan.g(this,"Do you want to update "+(_listWebSheetDefs.Count==1?"that web form":"those web forms")+"?");
			if(MessageBox.Show(message,"",MessageBoxButtons.YesNo)==DialogResult.No) {
				return true;
			}
			if(!WebFormL.VerifyRequiredFieldsPresent(_sheetDefCur)) {
				return false;
			}
			Cursor=Cursors.WaitCursor;
			WebFormL.LoadImagesToSheetDef(_sheetDefCur);
			bool isSuccess=WebFormL.TryAddOrUpdateSheetDef(this,_sheetDefCur,false,_listWebSheetDefs);
			Cursor=Cursors.Default;
			return isSuccess;
		}
		
		private void linkLabelTips_LinkClicked(object sender,LinkLabelLinkClickedEventArgs e) {
			if(IsTabMode) {
				return;
			}
			string tips="";
			tips+="The following shortcuts and hotkeys are supported:\r\n";
			tips+="\r\n";
			tips+="CTRL + C : Copy selected field(s).\r\n";
			tips+="\r\n";
			tips+="CTRL + V : Paste.\r\n";
			tips+="\r\n";
			tips+="ALT + Click : 'Rubber stamp' paste to the cursor position.\r\n";
			tips+="\r\n";
			tips+="Click + Drag : Click on a blank space and then drag to group select.\r\n";
			tips+="\r\n";
			tips+="CTRL + Click + Drag : Add a group of fields to the selection.\r\n";
			tips+="\r\n";
			tips+="Delete or Backspace : Delete selected field(s).\r\n";
			MessageBox.Show(Lan.g(this,tips));
		}

		private void butAlignLeft_Click(object sender,EventArgs e) {
			if(listFields.SelectedIndices.Count<2) {
				return;
			}
			List<SheetFieldDef> listSheetFieldDefs=GetSheetFieldDefsForLayoutModeCur();
			float minX=listSheetFieldDefs[listFields.SelectedIndices[0]].BoundsF.Left;
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				if(listSheetFieldDefs[listFields.SelectedIndices[i]].BoundsF.Left<minX) { //current element is higher up than the current 'highest' element.
					minX=listSheetFieldDefs[listFields.SelectedIndices[i]].BoundsF.Left;
				}
				for(int j=0;j<listFields.SelectedIndices.Count;j++) {
					if(i==j) { //Don't compare element to itself.
						continue;
					}
					if(listSheetFieldDefs[listFields.SelectedIndices[i]].Bounds.Y //compare the int bounds not the boundsF for practical use
					   ==listSheetFieldDefs[listFields.SelectedIndices[j]].Bounds.Y) //compare the int bounds not the boundsF for practical use
					{
						MsgBox.Show(this,"Cannot align controls. Two or more selected controls will overlap.");
						return;
					}
				}
			}
			for(int i=0;i<listFields.SelectedIndices.Count;i++) { //Actually move the controls now
				listSheetFieldDefs[listFields.SelectedIndices[i]].XPos=(int)minX;
			}
			panelMain.Refresh();
		}

		private void butAlignCenterH_Click(object sender,EventArgs e) {
			if(listFields.SelectedIndices.Count<2) {
				return;
			}
			List<SheetFieldDef> listSheetFieldDefs=GetSheetFieldDefsForLayoutModeCur();
			List<SheetFieldDef> listSelectedFields=new List<SheetFieldDef>();
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				listSelectedFields.Add(listSheetFieldDefs[listFields.SelectedIndices[i]]);
			}
			List<int> yPositions=new List<int>();
			float maxX=int.MinValue;
			float minX=int.MaxValue;
			foreach(SheetFieldDef field in listSelectedFields) {
				if(yPositions.Contains(field.YPos)) {
					MsgBox.Show(this,"Cannot align controls. Two or more selected controls will overlap.");
					return;
				}
				yPositions.Add(field.YPos);
				if(maxX<field.Bounds.Right) {
					maxX=field.Bounds.Right;
				}
				if(minX>field.Bounds.Left) {
					minX=field.Bounds.Left;
				}
			}
			int avgX=(int)(minX+maxX)/2;
			listSelectedFields.ForEach(x => x.XPos=avgX-x.Width/2);
			panelMain.Refresh();
		}

		private void butAlignRight_Click(object sender,EventArgs e) {
			if(listFields.SelectedIndices.Count<2) {
				return;
			}
			List<SheetFieldDef> listSheetFieldDefs=GetSheetFieldDefsForLayoutModeCur();
			List<SheetFieldDef> listSelectedFields=new List<SheetFieldDef>();
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				listSelectedFields.Add(listSheetFieldDefs[listFields.SelectedIndices[i]]);
			}
			if(listSelectedFields.Exists(f1 => listSelectedFields.FindAll(f2 => f1.YPos==f2.YPos).Count>1)) {
				MsgBox.Show(this,"Cannot align controls. Two or more selected controls will overlap.");
				return;
			}
			int maxX=listSelectedFields.Max(d => d.Bounds.Right);
			listSelectedFields.ForEach(field => field.XPos=maxX-field.Width);
			panelMain.Refresh();
		}

		private void butAutoSnap_Click(object sender,EventArgs e) {
			string prompt="Please enter auto snap column width between 10 and 100, or 0 to disable.";
			List<InputBoxParam> listParams=new List<InputBoxParam> { 
				new InputBoxParam(InputBoxType.TextBox,prompt,_autoSnapDistance.ToString(),Size.Empty) 
			};
			Func<string,bool> onOkClick=new Func<string, bool>((inputVal)=> { 
				int val=PIn.Int(inputVal,false);
				if(val!=0 && !val.Between(10,100)) {
					MsgBox.Show("Please enter a value between 10 and 100, or 0 to disable.");
					return false;
				}
				return true;
			});
			InputBox formIB=new InputBox(listParams,onOkClick);
			if(formIB.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_listSnapXVals.Clear();
			_listSnapYVals.Clear();
			_autoSnapDistance=PIn.Int(formIB.textResult.Text);
			panelMain.Refresh();
		}

		/// <summary>When clicked it will set all selected elements' Y coordinates to the smallest Y coordinate in the group, unless two controls have the same X coordinate.</summary>
		private void butAlignTop_Click(object sender,EventArgs e) {
			if(listFields.SelectedIndices.Count<2) {
				return;
			}
			List<SheetFieldDef> listSheetFieldDefs=GetSheetFieldDefsForLayoutModeCur();
			float minY=listSheetFieldDefs[listFields.SelectedIndices[0]].BoundsF.Top;
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				if(listSheetFieldDefs[listFields.SelectedIndices[i]].BoundsF.Top<minY) { //current element is higher up than the current 'highest' element.
					minY=listSheetFieldDefs[listFields.SelectedIndices[i]].BoundsF.Top;
				}
				for(int j=0;j<listFields.SelectedIndices.Count;j++) {
					if(i==j) { //Don't compare element to itself.
						continue;
					}
					if(listSheetFieldDefs[listFields.SelectedIndices[i]].Bounds.X //compair the int bounds not the boundsF for practical use
					   ==listSheetFieldDefs[listFields.SelectedIndices[j]].Bounds.X) //compair the int bounds not the boundsF for practical use
					{
						MsgBox.Show(this,"Cannot align controls. Two or more selected controls will overlap.");
						return;
					}
				}
			}
			for(int i=0;i<listFields.SelectedIndices.Count;i++) { //Actually move the controls now
				listSheetFieldDefs[listFields.SelectedIndices[i]].YPos=(int)minY;
			}
			panelMain.Refresh();
		}

		private void butPaste_Click(object sender,EventArgs e) {
			PasteControlsFromMemory(new Point(0,0));
		}

		private void butCopy_Click(object sender,EventArgs e) {
			CopyControlsToMemory();
		}

		private void butTabOrder_Click(object sender,EventArgs e) {
			IsTabMode=!IsTabMode;
			if(IsTabMode) {
				butOK.Enabled=false;
				butCancel.Enabled=false;
				butDelete.Enabled=false;
				groupAddNew.Enabled=false;
				butCopy.Enabled=false;
				butPaste.Enabled=false;
				groupAlignH.Enabled=false;
				groupAlignV.Enabled=false;
				//butAlignLeft.Enabled=false;
				//butAlignTop.Enabled=false;
				butEdit.Enabled=false;
				butAutoSnap.Enabled=false;
				groupBoxLayoutModes.Enabled=false;
				ListSheetFieldDefsTabOrder=new List<SheetFieldDef>(); //clear or create the list of tab orders.
			}
			else {
				butOK.Enabled=true;
				butCancel.Enabled=true;
				butDelete.Enabled=true;
				groupAddNew.Enabled=true;
				butCopy.Enabled=true;
				butPaste.Enabled=true;
				groupAlignH.Enabled=true;
				groupAlignV.Enabled=true;
				//butAlignLeft.Enabled=true;
				//butAlignTop.Enabled=true;
				butEdit.Enabled=true;
				butAutoSnap.Enabled=true;
				groupBoxLayoutModes.Enabled=true;
			}
			panelMain.Refresh();
		}
		
		private void radioLayoutDefault_CheckedChanged(object sender,EventArgs e) {
			if(radioLayoutDefault.Checked) {
				_sheetLayoutModeCur=((SheetFieldLayoutMode)radioLayoutDefault.Tag);//Set in InitLayoutModes()
			}
			else {//radioLayoutTP
				_sheetLayoutModeCur=((SheetFieldLayoutMode)radioLayoutTP.Tag);//Set in InitLayoutModes()
			}
			FillFieldList();
			panelMain.Refresh();
		}

		private void butPageAdd_Click(object sender,EventArgs e) {
			if(_sheetDefCur.PageCount>9) {
				//Maximum PageCount 10, this is an arbitrary number. If this number gets too big there may be issues with the graphics trying to draw the sheet.
				return;
			}
			_sheetDefCur.PageCount++;
			//SheetDefCur.IsMultiPage=true;
			panelMain.Height=_sheetDefCur.HeightTotal-(_sheetDefCur.PageCount==1?0:_sheetDefCur.PageCount*100-40);
			RefreshDoubleBuffer();
			panelMain.Refresh();
		}

		private void butPageRemove_Click(object sender,EventArgs e) {
			if(_sheetDefCur.PageCount<2) {
				//SheetDefCur.IsMultiPage=false;
				//Minimum PageCount 1
				return;
			}
			_sheetDefCur.PageCount--;
			if(_sheetDefCur.PageCount==1) {
				//SheetDefCur.IsMultiPage=false;
			}
			//Once sheet defs are enhanced to allow showing users editable margins we can go back to using the SheetDefCur.HeightTotal property.
			//For now we need to use the algorithm that Ryan provided me.  See task #743211 for more information.
			int lowestYPos=SheetUtil.GetLowestYPos(_sheetDefCur.SheetFieldDefs);
			int arbitraryHeight=lowestYPos-60;
			int onePageHeight=_sheetDefCur.IsLandscape ? _sheetDefCur.Width : _sheetDefCur.Height;
			int minimumPageCount=(int)Math.Ceiling((double)arbitraryHeight / (onePageHeight-40-60));//40/60 for the header/footer respectively.
			if(minimumPageCount > _sheetDefCur.PageCount) { //There are fields that have a YPos and heights that push them past the bottom of the page.
				MsgBox.Show(this,"Cannot remove pages that contain sheet fields.");
				_sheetDefCur.PageCount++;//Forcefully add a page back.
				return;
			}
			panelMain.Height=_sheetDefCur.HeightTotal-(_sheetDefCur.PageCount==1?0:_sheetDefCur.PageCount*100-40);
			RefreshDoubleBuffer();
			panelMain.Refresh();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!VerifyDesign()) {
				return;
			}
			if(!sheetEditMobile.MergeMobileSheetFieldDefs(_sheetDefCur,true,new Action<string>((err) => { MsgBox.Show(this,err); }))) {
				return;
			}
			if(!UpdateWebSheetDef()) {
				return;
			}
			SheetDefs.InsertOrUpdate(_sheetDefCur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormSheetDefEdit_FormClosing(object sender,FormClosingEventArgs e) {
			foreach(SheetFieldDef field in _sheetDefCur.SheetFieldDefs.Where(x => x.FieldType==SheetFieldType.Image)) {
				field.ImageField?.Dispose();
			}
		}
	}

	public class DrawFieldArgs:IDisposable {
		public Pen penBlue;
		public Pen penRed;
		public Pen penBlueThick;
		public Pen penRedThick;
		public Pen penBlack;
		public Pen penSelection;
		///<summary>Line color can be customized.  Make sure to explicitly set the color of this pen before using it because it might contain a color of a previous line.</summary>
		public Pen penLine;
		public Pen pen;
		public Brush brush;
		public SolidBrush brushBlue;
		public SolidBrush brushRed;
		///<summary>Static text color can be customized.  Make sure to explicitly set the color of this brush before using it because it might contain a color of previous static text.</summary>
		public SolidBrush brushText;
		public Font font;
		public FontStyle fontstyle;

		public DrawFieldArgs() {
			penBlue=new Pen(Color.Blue);
			penRed=new Pen(Color.Red);
			penBlueThick=new Pen(Color.Blue,1.6f);
			penRedThick=new Pen(Color.Red,1.6f);
			penBlack=new Pen(Color.Black);
			penSelection=new Pen(Color.Black);
			penLine=new Pen(Color.Black);
			brushBlue=new SolidBrush(Color.Blue);
			brushRed=new SolidBrush(Color.Red);
			brushText=new SolidBrush(Color.Black);
			pen=penBlack;
			brush=brushText;
		}

		public void Dispose() {
			penBlue.Dispose();
			penRed.Dispose();
			penBlueThick.Dispose();
			penRedThick.Dispose();
			penBlack.Dispose();
			penSelection.Dispose();
			penLine.Dispose();
			brushBlue.Dispose();
			brushRed.Dispose();
			brushText.Dispose();
		}
	}
}