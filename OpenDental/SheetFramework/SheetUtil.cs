using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.SheetFramework;
using OpenDentBusiness.WebTypes.WebForms;

namespace OpenDental{
	public class SheetUtil {
		///<summary>Supply a template sheet as well as a list of primary keys.  This method creates a new collection of sheets which each have a parameter of int.  It also fills the sheets with data from the database, so no need to run that separately.</summary>
		public static List<Sheet> CreateBatch(SheetDef sheetDef,List<long> priKeys) {
			//we'll assume for now that a batch sheet has only one parameter, so no need to check for values.
			//foreach(SheetParameter param in sheet.Parameters){
			//	if(param.IsRequired && param.ParamValue==null){
			//		throw new ApplicationException(Lan.g("Sheet","Parameter not specified for sheet: ")+param.ParamName);
			//	}
			//}
			List<Sheet> retVal=new List<Sheet>();
			//List<int> paramVals=(List<int>)sheet.Parameters[0].ParamValue;
			Sheet newSheet;
			SheetParameter paramNew;
			for(int i=0;i<priKeys.Count;i++){
				newSheet=CreateSheet(sheetDef);
				newSheet.Parameters=new List<SheetParameter>();
				paramNew=new SheetParameter(sheetDef.Parameters[0].IsRequired,sheetDef.Parameters[0].ParamName);
				paramNew.ParamValue=priKeys[i];
				newSheet.Parameters.Add(paramNew);
				SheetFiller.FillFields(newSheet);
				retVal.Add(newSheet);
			}
			return retVal;
		}

		///<summary>Supply a template sheet as well as a list of primary keys.  This method creates a new collection of sheets which each have a parameter of int.  It also fills the sheets with data from the database, so no need to run that separately.</summary>
		public static List<Sheet> TryCreateBatch(SheetDef sheetDef,List<long> priKeys) {
			List<Sheet> retVal=new List<Sheet>();
			SheetParameter paramNew;
			foreach(long key in priKeys) {
				Sheet newSheet=CreateSheet(sheetDef);
				newSheet.Parameters=new List<SheetParameter>();
				paramNew=new SheetParameter(sheetDef.Parameters[0].IsRequired,sheetDef.Parameters[0].ParamName);
				paramNew.ParamValue=key;
				newSheet.Parameters.Add(paramNew);
				if(!string.IsNullOrEmpty(SheetFiller.FillFields(newSheet))) {
					continue;//error while filling sheet.
				}
				retVal.Add(newSheet);
			}
			return retVal;
		}

		///<summary>Just before printing or displaying the final sheet output, the heights and y positions of various fields are adjusted according to 
		///their growth behavior.  This also now gets run every time a user changes the value of a textbox while filling out a sheet.
		///dataSet should be prefilled by calling AccountModules.GetAccount() before calling this method in order to calculate for statements.</summary>
		public static void CalculateHeights(Sheet sheet,DataSet dataSet=null,Statement stmt=null,bool isPrinting=false,
			int topMargin=40,int bottomMargin=60,MedLab medLab=null,Patient pat=null,Patient patGuar=null)
		{
			int calcH;
			FontStyle fontstyle;
			sheet.SheetFields=sheet.SheetFields.OrderBy(x => x.YPos).ThenBy(x => x.XPos).ToList();
			foreach(SheetField field in sheet.SheetFields) {
				if(field.FieldType==SheetFieldType.MobileHeader) {
					continue; //We never want to display mobile headers in desktop so we don't ever want to set/change their height
				}
				if(field.FieldType==SheetFieldType.Image || field.FieldType==SheetFieldType.PatImage) {
					#region Get the path for the image
					string filePathAndName;
					switch(field.FieldType) {
						case SheetFieldType.Image:
							filePathAndName=ODFileUtils.CombinePaths(GetImagePath(),field.FieldName);
							break;
						case SheetFieldType.PatImage:
							if(field.FieldValue=="") {
								//There is no document object to use for display, but there may be a baked in image and that situation is dealt with below.
								filePathAndName="";
								break;
							}
							Document patDoc=Documents.GetByNum(PIn.Long(field.FieldValue));
							List<string> paths=Documents.GetPaths(new List<long> { patDoc.DocNum },ImageStore.GetPreferredAtoZpath());
							if(paths.Count < 1) {//No path was found so we cannot draw the image.
								continue;
							}
							filePathAndName=paths[0];
							break;
						default:
							//not an image field
							continue;
					}
					#endregion
					if(field.FieldName=="Patient Info.gif") {
						continue;
					}
					if(CloudStorage.IsCloudStorage) {
						if(CloudStorage.FileExists(filePathAndName)) {
							continue;
						}
					}
					else if(File.Exists(filePathAndName)) {
						continue;
					}
					else {//img doesn't exist or we do not have access to it.
						field.Height=0;//Set height to zero so that it will not cause extra pages to print.
					}
				}
				if(field.GrowthBehavior==GrowthBehaviorEnum.None){//Images don't have growth behavior, so images are excluded below this point.
					continue;
				}
				fontstyle=FontStyle.Regular;
				if(field.FontIsBold){
					fontstyle=FontStyle.Bold;
				}
				//calcH=(int)g.MeasureString(field.FieldValue,font).Height;//this was too short
				switch(field.FieldType) {
					case SheetFieldType.Grid:
						if(sheet.SheetType==SheetTypeEnum.ERA) {
							//This logic mimics SheetPrinting.drawFieldGrid(...)
							SheetParameter param=SheetParameter.GetParamByName(sheet.Parameters,"IsSingleClaimPaid");
							bool isSingleClaim=(param.ParamValue==null)?false:true;//param is only set when true
							bool isOneClaimPerPage=PrefC.GetBool(PrefName.EraPrintOneClaimPerPage);
							X835 era=(X835)SheetParameter.GetParamByName(sheet.Parameters,"ERA").ParamValue;//Required field.
							DataTable tableClaimsPaid=SheetDataTableUtil.GetDataTableForGridType(sheet,dataSet,field.FieldName,stmt,medLab);
							DataTable tableRemarks=SheetDataTableUtil.GetDataTableForGridType(sheet,dataSet,"EraClaimsPaidProcRemarks",stmt,medLab);
							SheetDef gridHeaderSheetDef=SheetDefs.GetInternalOrCustom(SheetInternalType.ERAGridHeader);
							int printableHeight=sheet.Height-topMargin-bottomMargin;
							calcH=0;
							foreach(Hx835_Claim claim in era.ListClaimsPaid) {
								int pageMaxY=printableHeight*((calcH/printableHeight)+1);
								SheetField fieldCopy=field.Copy();
								if(isOneClaimPerPage && !isSingleClaim) {
									fieldCopy.YPos=topMargin+1;//Set field to highest point on page.
									calcH+=printableHeight;//Moves the field 1 page at a time.
								}
								fieldCopy.YPos+=calcH;
								if(fieldCopy.YPos<=pageMaxY && fieldCopy.YPos+gridHeaderSheetDef.Height>pageMaxY) {//Header is split between 2 pages.
									//When sheet is printing we move the field down from above the 
									//next page to the top of the current page in this situation.
									//Since pageMaxY is the same as the next pages pageMinY we can 
									//mimic how the the field would be moved to the next page in our calcH calculation.
									calcH+=((pageMaxY+topMargin+1)-fieldCopy.YPos);
								}
								int procGridHeight=CalculateGridHeightHelper(fieldCopy,sheet,stmt,topMargin,bottomMargin,medLab,dataSet,claim.ClpSegmentIndex,tableClaimsPaid);
								SheetField fieldRemarks=fieldCopy.Copy();
								fieldRemarks.FieldName="EraClaimsPaidProcRemarks";
								fieldRemarks.YPos=fieldCopy.YPos+procGridHeight;
								int remarkGridHeight=CalculateGridHeightHelper(fieldRemarks,sheet,stmt,topMargin,bottomMargin,medLab,dataSet,claim.ClpSegmentIndex,tableRemarks);
								int subFieldHeight=gridHeaderSheetDef.Height+procGridHeight+remarkGridHeight;
								if(isOneClaimPerPage) {
									if(subFieldHeight>printableHeight) {//Grid wraps onto a second page.
										calcH+=(printableHeight*(subFieldHeight/printableHeight));
									}
								}
								else {
									subFieldHeight+=25;//Space between each claim if printed multiple per page.
									calcH+=subFieldHeight;
								}
							}
						}
						else {
							calcH=CalculateGridHeightHelper(field,sheet,stmt,topMargin,bottomMargin,medLab,dataSet,pat: pat,patGuar: patGuar);
						}
						break;
					case SheetFieldType.Special:
						calcH=field.Height;
						break;
					default:
						using(Font font=new Font(field.FontName,field.FontSize,fontstyle)) {
							calcH=GraphicsHelper.MeasureStringH(field.FieldValue,font,field.Width,field.TextAlign);
						}
						break;
				}
				if(calcH<=field.Height //calc height is smaller
					&& field.FieldName!="StatementPayPlan" //allow this grid to shrink and disappear.
					&& field.FieldName!="TreatPlanBenefitsFamily" //allow this grid to shrink and disappear.
					&& field.FieldName!="TreatPlanBenefitsIndividual") //allow this grid to shrink and disappear.
				{
					continue;
				}
				int heightOld=field.Height;
				int amountOfGrowth=calcH-field.Height;
				field.Height=calcH;
				if(field.GrowthBehavior==GrowthBehaviorEnum.DownLocal){
					MoveAllDownWhichIntersect(sheet,field,amountOfGrowth);
				}
				else if(field.GrowthBehavior==GrowthBehaviorEnum.DownGlobal){
					//All sheet grids should have DownGlobal growth.
					if(amountOfGrowth < 0) {
						//The field shrunk, move all fields UP instead of DOWN.
						MoveAllUpBelowThis(sheet,field,heightOld);
					}
					else {//Make room for this field by moving all fields below it DOWN.
						MoveAllDownBelowThis(sheet,field,amountOfGrowth);
					}
				}
			}
			if(isPrinting && !Sheets.SheetTypeIsSinglePage(sheet.SheetType)) {
				//now break all text fields in between lines, not in the middle of actual text
				sheet.SheetFields.Sort(SheetFields.SortDrawingOrderLayers);
				int originalSheetFieldCount=sheet.SheetFields.Count;
				for(int i=0;i<originalSheetFieldCount;i++) {
					SheetField fieldCur=sheet.SheetFields[i];
					if(fieldCur.FieldType==SheetFieldType.StaticText
						|| fieldCur.FieldType==SheetFieldType.OutputText
						|| fieldCur.FieldType==SheetFieldType.InputField)
					{
						//recursive function to split text boxes for page breaks in between lines of text, not in the middle of text
						CalculateHeightsPageBreakForText(fieldCur,sheet);
					}
				}
			}
			//sort the fields again since we may have broken up some of the text fields into multiple fields and added them to sheetfields.
			sheet.SheetFields.Sort(SheetFields.SortDrawingOrderLayers);
			//return sheetCopy;
		}

		///<summary>Recursive.  Only for text sheet fields.</summary>
		private static void CalculateHeightsPageBreakForText(SheetField field,Sheet sheet) {
			FontStyle fontstyle=FontStyle.Regular;
			if(field.FontIsBold) {
				fontstyle=FontStyle.Bold;
			}
			Font font=new Font(field.FontName,field.FontSize,fontstyle);
			string text=field.FieldValue.Replace("\r\n","\n");//The RichTextBox control converts \r\n to \n.  We need to mimic so we can substring() below.
			//adjust the height of the text box to accomodate PDFs if the field has a growth behavior other than None
			int calcH=GraphicsHelper.MeasureStringH(text,font,field.Width,field.TextAlign);
			//If "field.Height < calcH" is ever removed then MoveAllUpBelowThis() would need to be considered below.
			if(field.GrowthBehavior!=GrowthBehaviorEnum.None && field.Height < calcH) {
				int amtGrowth=calcH-field.Height;
				field.Height+=amtGrowth;
				if(field.GrowthBehavior==GrowthBehaviorEnum.DownLocal) {
					MoveAllDownWhichIntersect(sheet,field,amtGrowth);
				}
				else if(field.GrowthBehavior==GrowthBehaviorEnum.DownGlobal) {
					MoveAllDownBelowThis(sheet,field,amtGrowth);
				}
			}
			int topMargin=40;
			if(sheet.SheetType==SheetTypeEnum.MedLabResults) {
				topMargin=120;
			}
			int pageCount;
			int bottomCurPage=SheetPrinting.BottomCurPage(field.YPos,sheet,out pageCount);
			//recursion base case, the field now fits on the current page, break out of recursion
			if(field.YPos+field.Height<=bottomCurPage) {
				return;
			}
			//field extends beyond the bottom of the current page, so we will split the text box in between lines, not through the middle of text
			//if the height of one line is greater than the printable height of the page, don't try to split between lines (only for huge fonts)
			if(font.Height+2 > (sheet.HeightPage-60-topMargin) || text.Length==0) {
				return;
			}
			int textBoxHeight=bottomCurPage-field.YPos;//the max height that the new text box can be in order to fit on the current page.
			//figure out how many lines of text will fit on the current page
			RichTextBox textboxClip=GraphicsHelper.CreateTextBoxForSheetDisplay(text,font,field.Width,textBoxHeight,field.TextAlign);
			List <RichTextLineInfo> listClipTextLines=GraphicsHelper.GetTextSheetDisplayLines(textboxClip);
			//if no lines of text will fit on current page or textboxClip's height is smaller than one line, move the entire text box to the next page
			if(listClipTextLines.Count==0 || textBoxHeight < (font.Height+2)) {
				int moveAmount=bottomCurPage+1-field.YPos;
				field.Height+=moveAmount;
				MoveAllDownWhichIntersect(sheet,field,moveAmount);
				field.Height-=moveAmount;
				field.YPos+=moveAmount;
				//recursive call
				CalculateHeightsPageBreakForText(field,sheet);
				return;
			}
			//prepare to split the text box into two text boxes, one with the lines that will fit on the current page, the other with all other lines
			int fieldH=GraphicsHelper.MeasureStringH(textboxClip.Text,textboxClip.Font,textboxClip.Width,textboxClip.SelectionAlignment);
			//get ready to copy text from the current field to a copy of the field that will be moved down.
			//find the character in the text box that makes the text box taller than the calculated max line height and split the text box at that line
			SheetField fieldNew;
			fieldNew=field.Copy();
			field.Height=fieldH;
			fieldNew.Height-=fieldH;//reduce the size of the new text box by the height of the text removed
			fieldNew.YPos+=fieldH;//move the new field down the amount of the removed text to maintain the distance between all fields below
			fieldNew.FieldValue=text.Substring(textboxClip.Text.Length);
			field.FieldValue=textboxClip.Text;
			textboxClip.Font.Dispose();
			textboxClip.Dispose();
			int moveAmountNew=bottomCurPage+1-fieldNew.YPos;
			fieldNew.Height+=moveAmountNew;
			MoveAllDownWhichIntersect(sheet,fieldNew,moveAmountNew);
			fieldNew.Height-=moveAmountNew;
			fieldNew.YPos+=moveAmountNew;
			sheet.SheetFields.Add(fieldNew);
			//recursive call
			CalculateHeightsPageBreakForText(fieldNew,sheet);
		}

		///<summary>Calculates height of grid taking into account page breaks, word wrapping, cell width, font size, and actual data to be used to fill this grid.
		///DataSet should be prefilled with AccountModules.GetAccount() before calling this method if calculating for a statement.</summary>
		private static int CalculateGridHeightHelper(SheetField field,Sheet sheet,Statement stmt,int topMargin,int bottomMargin,MedLab medLab
			,DataSet dataSet,long eraClaimSegmentIdx=-1,DataTable table=null,Patient pat=null,Patient patGuar=null) 
		{
			using(ODGrid odGrid=new ODGrid()) {
				if(!string.IsNullOrEmpty(field.FontName)) {
					odGrid.FontForSheets=new Font(field.FontName,field.FontSize,field.FontIsBold ? FontStyle.Bold : FontStyle.Regular);
				}
				odGrid.Width=field.Width;
				odGrid.HideScrollBars=true;
				odGrid.YPosField=field.YPos;
				odGrid.TopMargin=topMargin;
				odGrid.BottomMargin=bottomMargin;
				odGrid.PageHeight=sheet.HeightPage;
				odGrid.Title=field.FieldName;
				odGrid.TranslationName="";
				if(stmt!=null) {
					odGrid.Title+=((stmt.Intermingled || stmt.SinglePatient)?".Intermingled":".NotIntermingled");//Important for calculating heights.
				}
				if(table==null) {
					table=SheetDataTableUtil.GetDataTableForGridType(sheet,dataSet,field.FieldName,stmt,medLab,patGuar: patGuar);
				}
				List<DisplayField> columns=GetGridColumnsAvailable(field.FieldName);
				if(sheet.SheetType==SheetTypeEnum.Statement && stmt!=null &&  stmt.SuperFamily==0) {
					columns.RemoveAll(x => x.InternalName=="AcctTotal" || x.InternalName=="Account");
					columns.RemoveAll(x => x.InternalName=="PatNum");
				}
				if(sheet.SheetType==SheetTypeEnum.PaymentPlan) {
					PayPlan payPlan=(PayPlan)SheetParameter.GetParamByName(sheet.Parameters,"payplan").ParamValue;
					if(payPlan.IsDynamic) {
						columns.RemoveAll(x => x.InternalName=="Adjustment");
					}
				}
				bool isDiscountPlan=false;
				pat=(pat==null || pat.PatNum!=sheet.PatNum ? Patients.GetPat(sheet.PatNum) : pat);
				if(pat!=null) {
					isDiscountPlan=(pat.DiscountPlanNum>0);
				}
				if(isDiscountPlan) {
					columns.RemoveAll(x => x.InternalName=="Pri Ins" || x.InternalName=="Sec Ins" || x.InternalName=="Allowed");
				}
				else {
					columns.RemoveAll(x => x.InternalName=="DPlan");
				}
				if(field.FieldName=="TreatPlanMain") {
					columns.RemoveAll(x => x.InternalName==DisplayFields.InternalNames.TreatmentPlanModule.Appt);
				}
				#region  Fill Grid
				odGrid.BeginUpdate();
				odGrid.ListGridColumns.Clear();
				GridColumn col;
				for(int i=0;i<columns.Count;i++) {
					col=new GridColumn(columns[i].InternalName,columns[i].ColumnWidth);
					odGrid.ListGridColumns.Add(col);
				}
				GridRow row;
				for(int i=0;i<table.Rows.Count;i++) {
					if(eraClaimSegmentIdx!=-1 && table.Rows[i]["ClpSegmentIndex"].ToString()!=POut.Long(eraClaimSegmentIdx)) {
						continue;
					}
					row=new GridRow();
					for(int c=0;c<columns.Count;c++) {//Selectively fill columns from the dataTable into the odGrid.
						//Some DisplayFields require additional formatting and/or logic to display properly in a grid.
						string value=GetStringFromInternalName(field,columns[c],table.Rows[i]);
						row.Cells.Add(value);
					}
					if(table.Columns.Contains("PatNum")) {//Used for statments to determine account splitting.
						row.Tag=table.Rows[i]["PatNum"].ToString();
					}
					odGrid.ListGridRows.Add(row);
				}
				odGrid.EndUpdate(true);//Calls ComputeRows and ComputeColumns, meaning the RowHeights int[] has been filled.
				#endregion
				return odGrid.PrintHeight;
			}
		}

		///<summary>Uses the SheetField, DisplayField, and DataRow to determine the appropriate value for these parameters.  Some DisplayFields for 
		///various SheetFields or SheetTypes require additional formatting or logic to display data as expected to the user, and this logic is executed
		///in this method.  Ex: a date earlier than 1880 should not be displayed.</summary>
		public static string GetStringFromInternalName(SheetField field,DisplayField col,DataRow row) {
			string retVal=string.Empty;
			//DPlan is a DisplayField in multiple SheetTypes, including TreatPlanMain; important to check this InternalName before field.FieldName.
			if(col.InternalName=="DPlan") {
				retVal=row["Pri Ins"].ToString();
			}
			else if(field.FieldName=="TreatPlanMain" && col.InternalName.In("Prov","DateTP","Clinic")) {
				if(col.InternalName=="Prov") {
					long provNum=PIn.Long(row["ProvNum"].ToString());
					if(provNum>0) {
						retVal=Providers.GetAbbr(provNum);
					}
					else {
						retVal=string.Empty;
					}
				}
				else if(col.InternalName=="DateTP") {
					DateTime dateTP=PIn.Date(row["DateTP"].ToString());
					if(dateTP.Year>=1880) {
						retVal=dateTP.ToShortDateString();
					}
					else {
						retVal=string.Empty;
					}
				}
				else if(col.InternalName=="Clinic") {
					long clinicNum=PIn.Long(row["ClinicNum"].ToString());
					retVal=Clinics.GetAbbr(clinicNum);//Will be blank if ClinicNum not found, i.e. the 0 clinic.
				}
			}
			else { 
				retVal=row[col.InternalName].ToString();//Selectively fill columns from the dataTable into the odGrid.
			}
			return retVal;
		}

		public static void MoveAllDownBelowThis(Sheet sheet,SheetField field,int amountOfGrowth) {
			foreach(SheetField field2 in sheet.SheetFields) {
				if(field2.YPos>field.YPos) {//for all fields that are below this one
					field2.YPos+=amountOfGrowth;//bump down by amount that this one grew
				}
			}
		}

		///<summary>When a sheet field shrinks, we need to move all fields below it up to fill the gap that was left.
		///fieldToRemove.Height should be manipulated to the new and desired height before calling this method.
		///heightOld needs to be the height of fieldToRemove BEFORE manipulating it's Height property due to the shrink.</summary>
		public static void MoveAllUpBelowThis(Sheet sheet,SheetField fieldToRemove,int heightOld) {
			//We only want to move fields up as much as needed.
			//If there is a field on the same horizontal plane, then nothing needs to move.
			//For two fields A and B, where B is the field with the minimum y value that is greater than A.Ypos:
			//if there is a field that falls partially between the horizontal plane of A.YPos and (A.YPos + A.Height) then we only move up B.YPos - A.YPos;
			//else nothing is encompassed within our entire horizontal plane footprint, then move every field below up the entire height of our field.
			int amountOfShrink=(heightOld - fieldToRemove.Height);
			List<SheetField> listOtherSheetFields=sheet.SheetFields.FindAll(x => x!=fieldToRemove && x.Height > 0);//ignore previously shrunken fields.
			//Check for any fields on our exact plane.  If at least one field is on our exact YPos plane then don't move anything.
			if(listOtherSheetFields.Any(x => x.YPos==fieldToRemove.YPos)) {
				//Nothing needs to move because there is something on the same YPos horizontal plane.
				return;
			}
			int top=fieldToRemove.YPos;
			int bottom=fieldToRemove.YPos+heightOld;
			//Check for any fields that dangle partially between our horizontal plane of A.Top (A.YPos) and A.Bottom (A.YPos + A.Height).
			//Ignore fields which entirely encapsulate the field to remove (ex background images).
			List<SheetField> listDanglingSheetFields=listOtherSheetFields.FindAll(x => top > x.YPos
				&& ((x.YPos + x.Height) > top)
				&& ((x.YPos + x.Height) < bottom));
			if(listDanglingSheetFields.Count > 0) {
				//We only want to move up A.Bottom - B.Bottom where B is the field with the largest/bottommost point (B.YPos + B.Height).
				int deepestDangler=listDanglingSheetFields.Max(x => x.YPos + x.Height);
				int dangleShrink=(bottom - deepestDangler);
				if(amountOfShrink > dangleShrink) {
					//We need to treat the bottommost point as our new "top" for all logic that follows.
					top=deepestDangler;
					amountOfShrink=dangleShrink;//reduce the amount to shrink
				}
			}
			//----------------------------------------------------------------------------------------------------------------------------------------------
			//We can't do any movement calculations for sheet fields that completely encompass the fieldToRemove (often times it's a background image).
			//As of right now this is the only acceptable edge case that can make the sheet start overlapping / look funny.
			//Typically the only sheet field type that will completely engulf other sheet fields is a background image so it should be safe to ignore.
			//----------------------------------------------------------------------------------------------------------------------------------------------
			//Check for any fields that start partially between our horizontal plane of A.Top (A.YPos) and A.Bottom (A.YPos + A.Height)
			List<SheetField> listPeekingSheetFields=listOtherSheetFields.FindAll(x => x.YPos > top && x.YPos < bottom);
			if(listPeekingSheetFields.Count > 0) {
				//We only want to move up B.YPos - A.YPos where B is the field with the smallest/topmost YPos.
				int topmostPeeker=listPeekingSheetFields.Min(x => x.YPos);
				int peekerShrink=(topmostPeeker - top);
				if(amountOfShrink > peekerShrink) {
					amountOfShrink=peekerShrink;//reduce the amount to shrink
				}
			}
			foreach(SheetField field in sheet.SheetFields) {
				if(field.YPos > top) {//for all fields that are below this one
					field.YPos-=amountOfShrink;//bump up by amount that this one shrunk
				}
			}
		}

		///<Summary>Supply the field that we are testing.  All other fields which intersect with it will be moved down.  Each time one (or maybe some) is moved down, this method is called recursively.  The end result should be no intersections among fields near the original field that grew.</Summary>
		public static void MoveAllDownWhichIntersect(Sheet sheet,SheetField field,int amountOfGrowth) {
			//Phase 1 is to move everything that intersects with the field down. Phase 2 is to call this method on everything that was moved.
			//Phase 1: Move 
			List<SheetField> affectedFields=new List<SheetField>();
			foreach(SheetField field2 in sheet.SheetFields) {
				if(field2==field){
					continue;
				}
				if(field2.YPos<field.YPos){//only fields which are below this one
					continue;
				}
				if(field2.FieldType==SheetFieldType.Drawing){
					continue;
					//drawings do not get moved down.
				}
				if(field.Bounds.IntersectsWith(field2.Bounds)) {
					field2.YPos+=amountOfGrowth;
					affectedFields.Add(field2);
				}
			}
			//Phase 2: Recursion
			foreach(SheetField field2 in affectedFields) {
			  //reuse the same amountOfGrowth again.
			  MoveAllDownWhichIntersect(sheet,field2,amountOfGrowth);
			}
		}

		///<summary>Returns the lowest Y position from all the sheet field defs passed in.</summary>
		public static int GetLowestYPos(List<SheetFieldDef> listSheetFieldDefs) {
			int lowestYPos=0;
			foreach(SheetFieldDef sheetFieldDef in listSheetFieldDefs) {
				int bottom=sheetFieldDef.YPos + sheetFieldDef.Height;
				if(bottom > lowestYPos) {
					lowestYPos=bottom;
				}
			}
			return lowestYPos;
		}

		///<summary>Creates a Sheet object from a sheetDef, complete with fields and parameters.  Sets date to today. If patNum=0, do not save to DB, such as for labels.
		///Passes through to Sheets.CreateSheetFromSheetDef().</summary>
		public static Sheet CreateSheet(SheetDef sheetDef,long patNum=0,bool hidePaymentOptions=false) {
			return Sheets.CreateSheetFromSheetDef(sheetDef,patNum,hidePaymentOptions);
		}

		///<summary>Creates a Sheet from a WebSheet. Does not insert it into the database.</summary>
		public static Sheet CreateSheetFromWebSheet(long PatNum,WebForms_Sheet sheet) {
			Sheet newSheet=null;
			SheetDef sheetDef=new SheetDef((SheetTypeEnum)sheet.SheetType);
			newSheet=CreateSheet(sheetDef,PatNum);
			SheetParameter.SetParameter(newSheet,"PatNum",PatNum);
			newSheet.DateTimeSheet=sheet.DateTimeSheet;
			newSheet.Description=sheet.Description;
			newSheet.Height=sheet.Height;
			newSheet.Width=sheet.Width;
			newSheet.FontName=sheet.FontName;
			newSheet.FontSize=sheet.FontSize;
			newSheet.SheetType=(SheetTypeEnum)sheet.SheetType;
			newSheet.IsLandscape=sheet.IsLandscape;
			newSheet.InternalNote="";
			newSheet.IsWebForm=true;
			newSheet.ClinicNum=sheet.ClinicNum;
			newSheet.SheetDefNum=sheet.SheetDefNum;
			newSheet.HasMobileLayout=sheet.HasMobileLayout;
			//loop through each variable in a single sheetfield
			foreach(WebForms_SheetField field in sheet.SheetFields) {
				SheetField sheetfieldCopy=new SheetField();
				sheetfieldCopy.FieldName=field.FieldName;
				sheetfieldCopy.FieldType=field.FieldType;
				sheetfieldCopy.FontIsBold=field.FontIsBold;
				sheetfieldCopy.FontIsBold=field.FontIsBold;
				sheetfieldCopy.FontName=field.FontName;
				sheetfieldCopy.FontSize=field.FontSize;
				sheetfieldCopy.Height=field.Height;
				sheetfieldCopy.Width=field.Width;
				sheetfieldCopy.XPos=field.XPos;
				sheetfieldCopy.YPos=field.YPos;
				sheetfieldCopy.IsRequired=field.IsRequired;
				sheetfieldCopy.TabOrder=field.TabOrder;
				sheetfieldCopy.ReportableName=field.ReportableName;
				sheetfieldCopy.RadioButtonGroup=field.RadioButtonGroup;
				sheetfieldCopy.RadioButtonValue=field.RadioButtonValue;
				sheetfieldCopy.GrowthBehavior=field.GrowthBehavior;
				sheetfieldCopy.FieldValue=field.FieldValue;
				sheetfieldCopy.TextAlign=field.TextAlign;
				sheetfieldCopy.ItemColor=field.ItemColor;
				if(sheetfieldCopy.FieldType==SheetFieldType.SigBox && !string.IsNullOrWhiteSpace(sheetfieldCopy.FieldValue)) {
					sheetfieldCopy.DateTimeSig=newSheet.DateTimeSheet;
				}
				newSheet.SheetFields.Add(sheetfieldCopy);
			}
			return newSheet;
		}

		///<summary>Returns either a user defined statements sheet, the internal sheet if StatementsUseSheets is true. Returns null if StatementsUseSheets is false.</summary>
		public static SheetDef GetStatementSheetDef() {
			List<SheetDef> listDefs=SheetDefs.GetCustomForType(SheetTypeEnum.Statement);
			if(listDefs.Count>0) {
				return SheetDefs.GetSheetDef(listDefs[0].SheetDefNum);//Return first custom statement. Should be ordred by Description ascending.
			}
			return SheetsInternal.GetSheetDef(SheetInternalType.Statement);
		}

		///<summary>Returns either a user defined MedLabResults sheet or the internal sheet.</summary>
		public static SheetDef GetMedLabResultsSheetDef() {
			List<SheetDef> listDefs=SheetDefs.GetCustomForType(SheetTypeEnum.MedLabResults);
			if(listDefs.Count>0) {
				return SheetDefs.GetSheetDef(listDefs[0].SheetDefNum);//Return first custom statement. Should be ordred by Description ascending.
			}
			return SheetsInternal.GetSheetDef(SheetInternalType.MedLabResults);
		}

		/*
		///<summary>After pulling a list of SheetFieldData objects from the database, we use this to convert it to a list of SheetFields as we create the Sheet.</summary>
		public static List<SheetField> CreateSheetFields(List<SheetFieldData> sheetFieldDataList){
			List<SheetField> retVal=new List<SheetField>();
			SheetField field;
			FontStyle style;
			for(int i=0;i<sheetFieldDataList.Count;i++){
				style=FontStyle.Regular;
				if(sheetFieldDataList[i].FontIsBold){
					style=FontStyle.Bold;
				}
				field=new SheetField(sheetFieldDataList[i].FieldType,sheetFieldDataList[i].FieldName,sheetFieldDataList[i].FieldValue,
					sheetFieldDataList[i].XPos,sheetFieldDataList[i].YPos,sheetFieldDataList[i].Width,sheetFieldDataList[i].Height,
					new Font(sheetFieldDataList[i].FontName,sheetFieldDataList[i].FontSize,style),sheetFieldDataList[i].GrowthBehavior);
				retVal.Add(field);
			}
			return retVal;
		}*/

		///<summary>Typically returns something similar to \\SERVER\OpenDentImages\SheetImages</summary>
		public static string GetImagePath(){
			string imagePath;
			if(PrefC.AtoZfolderUsed==DataStorageType.InDatabase) {
				throw new ApplicationException("Must be using AtoZ folders.");
			}
			imagePath=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"SheetImages");
			if(CloudStorage.IsCloudStorage) {
				imagePath=imagePath.Replace("\\","/");
			}
			if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ && !Directory.Exists(imagePath)) {
				Directory.CreateDirectory(imagePath);
			}
			return imagePath;
		}

		///<summary>Typically returns something similar to \\SERVER\OpenDentImages\SheetImages</summary>
		public static string GetPatImagePath() {
			string imagePath;
			if(PrefC.AtoZfolderUsed==DataStorageType.InDatabase) {
				throw new ApplicationException("Must be using AtoZ folders.");
			}
			imagePath=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"SheetPatImages");
			if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ && !Directory.Exists(imagePath)) {
				Directory.CreateDirectory(imagePath);
			}
			if(CloudStorage.IsCloudStorage) {
				imagePath=imagePath.Replace("\\","/");
			}
			return imagePath;
		}

		///<summary>Returns the current list of all columns available for the grid in the data table.</summary>
		public static List<DisplayField> GetGridColumnsAvailable(string gridType) {
			int i=0;
			List<DisplayField> retVal=new List<DisplayField>();
			switch(gridType) {
				case "StatementMain":
					retVal=DisplayFields.GetForCategory(DisplayFieldCategory.StatementMainGrid);
					break;
				case "StatementEnclosed":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="AmountDue",Description="Amount Due",ColumnWidth=107,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="DateDue",Description="Date Due",ColumnWidth=107,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="AmountEnclosed",Description="Amount Enclosed",ColumnWidth=107,ItemOrder=++i });
					break;
				case "StatementAging":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="PatNum",Description="PatNum",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Account",Description="Account",ColumnWidth=200,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Age00to30",Description="0-30",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Age31to60",Description="31-60",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Age61to90",Description="61-90",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Age90plus",Description="over 90",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="AcctTotal",Description="Total",ColumnWidth=75,ItemOrder=++i });
					break;
				case "StatementPayPlan":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="date",Description="Date",ColumnWidth=80,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="description",Description="Description",ColumnWidth=250,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="charges",Description="Charges",ColumnWidth=60,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="credits",Description="Credits",ColumnWidth=60,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="balance",Description="Balance",ColumnWidth=60,ItemOrder=++i });
					break;
				case "StatementInvoicePayment":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="date",Description="Date",ColumnWidth=80,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="prov",Description="Prov",ColumnWidth=60,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="patient",Description="Patient",ColumnWidth=60,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="type",Description="Type",ColumnWidth=60,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="ProcCode",Description="Proc",ColumnWidth=60,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="description",Description="Description",ColumnWidth=250,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="amt",Description="Amt",ColumnWidth=60,ItemOrder=++i });
					break;
				case "MedLabResults":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="obsIDValue",Description="Test / Result",ColumnWidth=500,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="obsAbnormalFlag",Description="Flag",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="obsUnits",Description="Units",ColumnWidth=70,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="obsRefRange",Description="Ref Interval",ColumnWidth=97,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="facilityID",Description="Lab",ColumnWidth=28,ItemOrder=++i });
					break;
				case "PayPlanMain":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="ChargeDate",Description="Date",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Provider",Description="Provider",ColumnWidth=60,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Description",Description="Description",ColumnWidth=140,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Principal",Description="Principal",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Interest",Description="Interest",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="due",Description="Due",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="payment",Description="Payment",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Adjustment",Description="Adjustment",ColumnWidth=70,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="balance",Description="Balance",ColumnWidth=75,ItemOrder=++i });
					break;
				case "TreatPlanMain":
					retVal=DisplayFields.GetForCategory(DisplayFieldCategory.TreatmentPlanModule);
					break;
				case "TreatPlanBenefitsFamily":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="BenefitName",Description="",ColumnWidth=150,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Primary",Description="Primary",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Secondary",Description="Secondary",ColumnWidth=75,ItemOrder=++i });
					break;
				case "TreatPlanBenefitsIndividual":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="BenefitName",Description="",ColumnWidth=150,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Primary",Description="Primary",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Secondary",Description="Secondary",ColumnWidth=75,ItemOrder=++i });
					break;
				case "EraClaimsPaid":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="ProcCode",Description="Proc Code",ColumnWidth=70,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="ProcDescript",Description="Proc Descript",ColumnWidth=100,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="FeeBilled",Description="Fee Billed",ColumnWidth=90,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="PatResp",Description="Pat Resp.",ColumnWidth=90,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Contractual",Description="Contractual",ColumnWidth=90,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="PayorReduct",Description="Payor Reduct.",ColumnWidth=90,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="OtherAdjust",Description="Other Adjust",ColumnWidth=90,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="InsPaid",Description="Ins Paid.",ColumnWidth=90,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="RemarkCodes",Description="Remark Codes",ColumnWidth=90,ItemOrder=++i });
					break;
				case "ReferralLetterProceduresCompleted":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Date",Description="Date",ColumnWidth=65,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Prov",Description="Prov",ColumnWidth=70,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="ProcCode",Description="Proc Code",ColumnWidth=90,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Tth",Description="Tth",ColumnWidth=25,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Surf",Description="Surf",ColumnWidth=35,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Description",Description="Description",ColumnWidth=200,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Note",Description="Note",ColumnWidth=200,ItemOrder=++i });
					break;
				case "EraClaimsPaidProcRemarks":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="RemarkCode",Description="Remark",ColumnWidth=70,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="RemarkDescription",Description="Remark Description",ColumnWidth=730,ItemOrder=++i });
					break;
				case DashApptGrid.SheetFieldName:
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="ApptStatus",Description="ApptStatus",ColumnWidth=70,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Prov",Description="Prov",ColumnWidth=40,ItemOrder=++i });
					if(PrefC.HasClinicsEnabled) {
						retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Clinic",Description="Clinic",ColumnWidth=60,ItemOrder=++i });
					}
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Date",Description="Date",ColumnWidth=65,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Time",Description="Time",ColumnWidth=55,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Min",Description="Min",ColumnWidth=20,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Procedures",Description="Procedures",ColumnWidth=100,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Notes",Description="Notes",ColumnWidth=75,ItemOrder=++i });
					break;
				case "PatientInfo":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="FieldName",Description="FieldName",ColumnWidth=100,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="FieldValue",Description="FieldValue",ColumnWidth=0,ItemOrder=++i });
					break;
				case "ProgressNotes":
					if(ChartViews.GetDeepCopy().Count==0) {
						retVal.AddRange(DisplayFields.GetDefaultList(DisplayFieldCategory.None));
					}
					else {//This is primarly done for GrowthBehaviorEnum.FillDownFitColumns.
						//Get the least wide grid for dynamic layout design in FormSheetDefEdit.cs
						//By getting the least wide grid we can ensure that all controls to the right of this grid
						//will move according to the width of this grid, they will never overlap.
						//See SheetUtil.SetControlSizeAndAnchors(...) and SheetUserControl.InitLayoutForSheetDef(...).
						int minGridWidth=-1;
						long minChartViewNum=-1;
						foreach(ChartView view in ChartViews.GetDeepCopy()) {
							List<DisplayField> listDisplayFields=DisplayFields.GetForChartView(view.ChartViewNum);
							int widthGridTotal=listDisplayFields.Sum(x => x.ColumnWidth);
							if(widthGridTotal>minGridWidth) {
								continue;
							}
							minGridWidth=widthGridTotal;
							minChartViewNum=view.ChartViewNum;
						}
						retVal.AddRange(DisplayFields.GetForChartView(minChartViewNum));
					}
					retVal.ForEach(x => x.Description=x.InternalName);
					break;
				case "TreatmentPlans":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Status",Description="Status",ColumnWidth=50,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Heading",Description="Heading",ColumnWidth=315,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Procs",Description="Procs",ColumnWidth=0,ItemOrder=++i });
					break;
				case "Procedures":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Priority",Description="Priority",ColumnWidth=50,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Tth",Description="Tth",ColumnWidth=35,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Surf",Description="Surf",ColumnWidth=40,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Code",Description="Code",ColumnWidth=50,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Description",Description="Description",ColumnWidth=0,ItemOrder=++i });
					break;
			}
			return retVal;
		}

		///<summary>Returns valid names of grids that can be used for given sheetType.
		///Set layoutMode when sheetType is associated to a dynamic SheetTypeEnum. When set different grids become available.</summary>
		public static List<string> GetGridsAvailable(SheetTypeEnum sheetType,SheetFieldLayoutMode layoutMode) {
			List<string> retVal=new List<string>();
			switch(sheetType) {
				case SheetTypeEnum.Statement:
					retVal.Add("StatementAging");
					retVal.Add("StatementEnclosed");
					retVal.Add("StatementMain");
					retVal.Add("StatementPayPlan");
					retVal.Add("StatementInvoicePayment");
					break;
				case SheetTypeEnum.MedLabResults:
					retVal.Add("MedLabResults");
					break;
				case SheetTypeEnum.PaymentPlan:
					retVal.Add("PayPlanMain");
					break;
				case SheetTypeEnum.TreatmentPlan:
					retVal.Add("TreatPlanMain");
					retVal.Add("TreatPlanBenefitsFamily");
					retVal.Add("TreatPlanBenefitsIndividual");
					break;
				case SheetTypeEnum.ERA:
					retVal.Add("EraClaimsPaid");
					break;
				case SheetTypeEnum.ReferralLetter:
					retVal.Add("ReferralLetterProceduresCompleted");
					break;
				case SheetTypeEnum.ChartModule:
					retVal.Add("ProgressNotes");
					if(layoutMode.In(SheetFieldLayoutMode.TreatPlan,SheetFieldLayoutMode.EcwTreatPlan,SheetFieldLayoutMode.OrionTreatPlan,SheetFieldLayoutMode.MedicalPracticeTreatPlan)) {
						retVal.Add("TreatmentPlans");
						retVal.Add("Procedures");
					}
					if(!layoutMode.In(SheetFieldLayoutMode.Orion,SheetFieldLayoutMode.OrionTreatPlan)) {//PatientInfo control is in the tab control when using Orion.
						retVal.Add("PatientInfo");
					}
					break;
			}
			return retVal;
		}

		///<summary>For all combo boxes that have not had an item selected, select the first option.</summary>
		public static void SetDefaultValueForComboBoxes(Sheet sheet) {
			foreach(SheetField field in sheet.SheetFields.Where(x => x.FieldType==SheetFieldType.ComboBox)) {
				if(SheetComboBox.HasSelection(field.FieldValue)) {
					continue;
				}
				field.FieldValue=SheetComboBox.FirstOption(field.FieldValue);
			}
		}

		///<summary>Fills the given comboBox with valid GrowthBehaviorEnum options.
		///Centralizes fill of growth behavior into comboboxes in several forms to ensure consistent behavior regarding the available options.</summary>
		public static void FillComboGrowthBehavior(UI.ComboBoxPlus combo,GrowthBehaviorEnum growthBehaviorSelected,bool isDynamicSheetType=false,bool isGridCombo=false) {
			List<GrowthBehaviorEnum> listGrowthOptions=new List<GrowthBehaviorEnum>();
			foreach(GrowthBehaviorEnum growthBehaviorEnum in Enum.GetValues(typeof(GrowthBehaviorEnum)).OfType<GrowthBehaviorEnum>().ToList()) {
				SheetGrowthAttribute sheetGrowthAttribute=growthBehaviorEnum.GetAttributeOrDefault<SheetGrowthAttribute>();
				if(growthBehaviorEnum==GrowthBehaviorEnum.None || isDynamicSheetType==sheetGrowthAttribute.IsDynamic) {
					//When filling a comboBox associated to setting the growthBehavior of a grid we skip GridOnly growthAttributes.
					if(!isGridCombo && sheetGrowthAttribute.IsGridOnly) {
						continue;
					}
					listGrowthOptions.Add(growthBehaviorEnum);
				}
			}
			combo.Items.AddList(listGrowthOptions,(growthOption) => growthOption.ToString());//can't use AddEnum
			//since this is a filtered enum, we can't just set the index
			for(int i=0;i<combo.Items.Count;i++){
				if((GrowthBehaviorEnum)combo.Items.GetObjectAt(i)==growthBehaviorSelected){
					combo.SetSelected(i);
				}
			}
		}

		///<summary>Returns the associated SheetFieldLayoutMode for given sheetType. Always includes SheetFieldLayoutMode.Default.</summary>
		public static List<SheetFieldLayoutMode> GetModesForSheetType(SheetTypeEnum sheetType) {
			return sheetType.GetAttributeOrDefault<SheetLayoutAttribute>().ArraySheetFieldLayoutModes.ToList();
		}

		///<summary>Sets the size and anchors for the given targetControl based on fieldDef.GrowthBehavior.
		///Typically only used when working with dynamic sheet defs (ex: chart module).
		///The logic in this function assume the targetControl is always and completely inside the parentControl.
		///When doAnchorToBottom is set false and fieldDef is associated to GrowthBehaviorEnum.FillDown then targetControls anchors will only be set to Top and Left.</summary>
		public static void SetControlSizeAndAnchors(SheetFieldDef fieldDef,Control targetControl,Control parentControl,int fillHeightAdj=0,bool doAnchorToBottom=true) {
			//When the sheet is refreshed, we will set the Width and Height.
			//When the sheet is resized, .NET will resize the controls based on anchors.
			switch(fieldDef.GrowthBehavior) {
				case GrowthBehaviorEnum.FillRight:
					targetControl.Width=(parentControl.Width-fieldDef.XPos)-1;
					targetControl.Height=fieldDef.Height;
					targetControl.Anchor=(AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right);
					break;
				case GrowthBehaviorEnum.FillDown:
					targetControl.Width=fieldDef.Width;
					targetControl.Height=Math.Max(1,(parentControl.Height-fieldDef.YPos-fillHeightAdj)-1);
					if(doAnchorToBottom) {
						targetControl.Anchor=(AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Bottom);
					}
					else {
						targetControl.Anchor=(AnchorStyles.Top|AnchorStyles.Left);
					}
					break;
				case GrowthBehaviorEnum.FillRightDown:
					targetControl.Width=(parentControl.Width-fieldDef.XPos)-1;
					targetControl.Height=Math.Max(1,(parentControl.Height-fieldDef.YPos-fillHeightAdj)-1);
					targetControl.Anchor=(AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right|AnchorStyles.Bottom);
					break;
				case GrowthBehaviorEnum.FillDownFitColumns://Only grids have access to this option.
					int width=0;
					if(targetControl is ODGrid) {
						width=(targetControl as ODGrid).ListGridColumns.Sum(x => x.ColWidth)+20;//+20 for border and vertical scroll bar width
					}
					else {//Must contain a ODGrid directly then
						width=(targetControl.Controls.OfType<Control>().First(x => x is ODGrid) as ODGrid).ListGridColumns.Sum(x => x.ColWidth)+22;//+22 for border and vertical scroll bar width
					}
					targetControl.Width=Math.Max(width,524);
					targetControl.Height=Math.Max(1,(parentControl.Height-fieldDef.YPos-fillHeightAdj)-1);
					targetControl.Anchor=(AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Bottom);
					break;
				default:
					targetControl.Width=fieldDef.Width;
					targetControl.Height=fieldDef.Height;
					targetControl.Anchor=(AnchorStyles.Top|AnchorStyles.Left);
					break;
			}
			if(fieldDef.FieldName!="ChartModuleTabs") {//ChartModuleTabs can be collapsed in the Chart Module, do not limit size.
				targetControl.MinimumSize=new Size(fieldDef.Width,fieldDef.Height);
			}
		}

		///<summary>Displays a Sheet, first checking if the sheet is linked to an existing Document and if so, displaying the file.  Otherwise, opens the
		///Sheet via FormSheetFillEdit.</summary>
		public static void ShowSheet(Sheet sheet,Patient pat,FormClosingEventHandler onFormClosing) {
			if(sheet==null) {
				MsgBox.Show("Sheets","Error opening sheet.");
				return;
			}
			if(sheet.DocNum!=0) {
				if(PrefC.AtoZfolderUsed==DataStorageType.InDatabase) {
					MsgBox.Show("Sheets","Unable to view saved sheet when storing images in database.");
					return;
				}
				Document sheetDoc=Documents.GetByNum(sheet.DocNum,true);
				if(sheetDoc==null) {
					MsgBox.Show("Sheets","Saved sheet no longer exists.");
					return;
				}
				string patFolder=ImageStore.GetPatientFolder(pat,ImageStore.GetPreferredAtoZpath());
				FileAtoZ.OpenFile(ImageStore.GetFilePath(sheetDoc,patFolder));
			}
			else {
				FormSheetFillEdit.ShowForm(sheet,onFormClosing);
			}	
		}

	}
}
