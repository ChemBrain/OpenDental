using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PdfSharp.Drawing;
using OpenDentBusiness;
using System.Drawing.Drawing2D;
using CodeBase;

namespace OpenDental {
	public class GraphicsHelper {

		///<summary>Since Graphics doesn't have a line height property.
		///Our rendering algorithm focuses on wrapping the text on the same character on each line for all printing options (screen, printer, pdf),
		///as well as making the text exactly fix vertically within the bounds given in order to properly support sheet vertical growth behavior.
		///Since all printing options have slightly different implementations within their drivers, the same font when used in each option is slightly
		///different.  As a result, the printed text width will vary depending on which printing option is used.  We return a rectangle representing the
		///actual drawn area of the output string for use in special cases such as the TreatmentPlan.Note, which has a border drawn around it always.
		///</summary>
		public static RectangleF DrawString(Graphics g,string str,Font font,Brush brush,Rectangle bounds,HorizontalAlignment align) {
			if(str.Trim()=="") {
				return bounds;//Nothing to draw.
			}
			StringFormat sf=StringFormat.GenericTypographic;
			//The overload for DrawString that takes a StringFormat will cause the tabs '\t' to be ignored.
			//In order for the tabs to not get ignored, we have to tell StringFormat how many pixels each tab should be.  
			//50.0f is the closest to our Fill Sheet Edit preview.
			sf.SetTabStops(0.0f,new float[1] { 50.0f });
			RichTextBox textbox=CreateTextBoxForSheetDisplay(str,font,bounds.Width,bounds.Height,align);
			List<RichTextLineInfo> listTextLines=GetTextSheetDisplayLines(textbox);
			float deviceLineHeight=g.MeasureString(str.Replace("\r","").Replace("\n",""),font,int.MaxValue,sf).Height;
			float scale=deviceLineHeight/textbox.Font.Height;//(size when printed)/(size on screen)
			font=new Font(font.FontFamily,font.Size*scale,font.Style);
			float maxLineWidth=0;
			for(int i=0;i<listTextLines.Count;i++) {
				string line=RichTextLineInfo.GetTextForLine(textbox.Text,listTextLines,i);
				if(line.Trim().Length > 0) {
					float textWidth=g.MeasureString(line,font,int.MaxValue,sf).Width;
					float x;
					if(align==HorizontalAlignment.Left) {
						x=bounds.X + listTextLines[i].Left * scale;
					}
					else if(align==HorizontalAlignment.Center) {
						x=bounds.X + ((bounds.Width - (textWidth * scale)) / 2);
					}
					else {//Right
						x=bounds.X + bounds.Width - (textWidth * scale);
					}
					float y=bounds.Y+listTextLines[i].Top*scale;
					g.DrawString(line,font,brush,x,y,sf);
					maxLineWidth=Math.Max(maxLineWidth,(listTextLines[i].Left*scale)+textWidth);
				}
			}
			textbox.Font.Dispose();//This font was dynamically created when the textbox was created.
			textbox.Dispose();
			sf.Dispose();
			font.Dispose();
			return new RectangleF(bounds.X,bounds.Y,maxLineWidth,bounds.Height);
		}

		///<summary>The PdfSharp version of Graphics.DrawString().  scaleToPix scales xObjects to pixels.
		///Our rendering algorithm focuses on wrapping the text on the same character on each line for all printing options (screen, printer, pdf),
		///as well as making the text exactly fix vertically within the bounds given in order to properly support sheet vertical growth behavior.
		///Since all printing options have slightly different implementations within their drivers, the same font when used in each option is slightly
		///different.  As a result, the printed text width will vary depending on which printing option is used.  We return a rectangle representing the
		///actual drawn area of the output string for use in special cases such as the TreatmentPlan.Note, which has a border drawn around it always.
		///</summary>
		public static RectangleF DrawStringX(XGraphics xg,string str,XFont xfont,XBrush xbrush,RectangleF bounds,HorizontalAlignment align) {
			if(str.Trim()=="") {
				return bounds;//Nothing to draw.
			}
			XStringFormat sf=XStringFormats.Default;
			//There are two coordinate systems here: pixels (used by us) and points (used by PdfSharp).
			//MeasureString and ALL related measurement functions must use pixels.
			//DrawString is the ONLY function that uses points.
			//pixels:
			FontStyle fontstyle=FontStyle.Regular;
			if(xfont.Style==XFontStyle.Bold) {
				fontstyle=FontStyle.Bold;
			}
			//pixels: (except Size is em-size)
			Font font=new Font(xfont.Name,(float)xfont.Size,fontstyle);
			RichTextBox textbox=CreateTextBoxForSheetDisplay(str,font,(int)Math.Ceiling(bounds.Width),(int)Math.Ceiling(bounds.Height),align);
			List<RichTextLineInfo> listTextLines=GetTextSheetDisplayLines(textbox);
			float deviceLineHeight=PointsToPixels((float)xg.MeasureString(str.Replace("\r","").Replace("\n",""),xfont,sf).Height);
			float scale=deviceLineHeight/textbox.Font.Height;//(size when printed)/(size on screen)
			font.Dispose();
			xfont=new XFont(xfont.Name,xfont.Size*scale,xfont.Style);
			double maxLineWidth=0;
			for(int i=0;i<listTextLines.Count;i++) {
				string line=RichTextLineInfo.GetTextForLine(textbox.Text,listTextLines,i);
				if(line.Trim().Length > 0) {
					double textWidth=xg.MeasureString(line,xfont,sf).Width;
					float x;
					if(align==HorizontalAlignment.Left) {
						x=PixelsToPoints(bounds.X + listTextLines[i].Left * scale);
					}
					else if(align==HorizontalAlignment.Center) {
						x=PixelsToPoints(bounds.X) + ((PixelsToPoints(bounds.Width) - (float)textWidth) / 2);
					}
					else {//Right
						x=PixelsToPoints(bounds.X + bounds.Width) - (float)textWidth;
					}
					float y=PixelsToPoints(bounds.Y+listTextLines[i].Bottom*scale);
					//There is currently a problem with printing the tab character '\t' when using XStringFormat.
					//C#'s StringFormat has a method called SetTabStops() which can be used to get the tabs to be drawn (see regular printing above).
					//We're doing nothing for now because the current complaint is only for printing, not PDF creation.  
					//A workaround is to not use tabs and to instead use separate static text fields that are spaced out as desired.
					xg.DrawString(line,xfont,xbrush,x,y,sf);
					maxLineWidth=Math.Max(maxLineWidth,PixelsToPoints(listTextLines[i].Left*scale)+textWidth);
				}
			}
			textbox.Font.Dispose();//This font was dynamically created when the textbox was created.
			textbox.Dispose();
			//sf.Dispose();//Does not exist for PDF.
			//xfont.Dispose();//Does not exist for PDF fonts.
			return new RectangleF(bounds.X,bounds.Y,PointsToPixels((float)maxLineWidth),bounds.Height);
		}

		///<summary>Draws a rectangle with rounded edges.  Sets the SmoothingMode to AntiAlias before drawing and then puts it back afterwards.</summary>
		///<param name="g">The System.Drawing.Graphics object to be used to draw the rectangle.</param>
		///<param name="pen">A System.Drawing.Pen object that determines the color, width, and style of the rectangle.</param>
		///<param name="rect">A System.Drawing.RectangleF structure that represents the rectangle to draw.</param>
		///<param name="round">Determines the radius of the corners.</param>
		public static void DrawRoundedRectangle(Graphics g,Pen pen,RectangleF rect,float round) {
			//Keep track of what the SmoothingMode was before this method was invoked.
			SmoothingMode oldSmoothingMode=g.SmoothingMode;
			g.SmoothingMode=SmoothingMode.AntiAlias;
			//top,right
			g.DrawLine(pen,rect.Left+round,rect.Top,rect.Right-round,rect.Top);
			g.DrawArc(pen,rect.Right-round*2,rect.Top,round*2,round*2,270,90);
			//right,bottom
			g.DrawLine(pen,rect.Right,rect.Top+round,rect.Right,rect.Bottom-round);
			g.DrawArc(pen,rect.Right-round*2,rect.Bottom-round*2,round*2,round*2,0,90);
			//bottom,left
			g.DrawLine(pen,rect.Right-round,rect.Bottom,rect.Left+round,rect.Bottom);
			g.DrawArc(pen,rect.Left,rect.Bottom-round*2,round*2,round*2,90,90);
			//left,top
			g.DrawLine(pen,rect.Left,rect.Bottom-round,rect.Left,rect.Top+round);
			g.DrawArc(pen,rect.Left,rect.Top,round*2,round*2,180,90);
			//Put the SmoothingMode back the way it was before this method was invoked.
			g.SmoothingMode=oldSmoothingMode;
		}

		public static GraphicsPath GetRoundedPath(RectangleF rectF,float radiusCorner){
			//There is a similar copy in ContrApptPanel.
			GraphicsPath graphicsPath=new GraphicsPath();
			graphicsPath.AddLine(rectF.Left+radiusCorner,rectF.Top,rectF.Right-radiusCorner,rectF.Top);//top
			graphicsPath.AddArc(rectF.Right-radiusCorner*2,rectF.Top,radiusCorner*2,radiusCorner*2,270,90);//UR
			graphicsPath.AddLine(rectF.Right,rectF.Top+radiusCorner,rectF.Right,rectF.Bottom-radiusCorner);//right
			graphicsPath.AddArc(rectF.Right-radiusCorner*2,rectF.Bottom-radiusCorner*2,radiusCorner*2,radiusCorner*2,0,90);//LR
			graphicsPath.AddLine(rectF.Right-radiusCorner,rectF.Bottom,rectF.Left+radiusCorner,rectF.Bottom);//bottom
			graphicsPath.AddArc(rectF.Left,rectF.Bottom-radiusCorner*2,radiusCorner*2,radiusCorner*2,90,90);//LL
			graphicsPath.AddLine(rectF.Left,rectF.Bottom-radiusCorner,rectF.Left,rectF.Top+radiusCorner);//left
			graphicsPath.AddArc(rectF.Left,rectF.Top,radiusCorner*2,radiusCorner*2,180,90);//UL
			return graphicsPath;
		}

		///<summary>This function is critical for measuring dynamic text fields on sheets when displaying or printing.
		///Measures the size of a text string when displayed on screen.  This also differs from the regular MeasureString in that it will
		///correctly measure trailing carriage returns as requiring additional lines.</summary>
		public static int MeasureStringH(string text,Font font,int width,HorizontalAlignment align) {
			if(!text.EndsWith("\n")) {
				//Add an extra line of text so that we can calculate the top of that last fake line to figure out where the bottom of the last real line is.
				text+="\n";
			}
			text+="*";
			//Assume height of font.Height+2 to force multi-line.
			RichTextBox textbox=CreateTextBoxForSheetDisplay(text,font,width,font.Height+2,align,false);
			int h=textbox.GetPositionFromCharIndex(textbox.Text.Length-1).Y;//The top of the fake line is the bottom of the last real line.
			textbox.Font.Dispose();//This font was dynamically created when the textbox was created.
			textbox.Dispose();
			return h;
		}

		///<summary>Returns a list of strings representing the lines of sheet text which will display on screen when viewing the specified text.</summary>
		public static List<RichTextLineInfo> GetTextSheetDisplayLines(RichTextBox textbox) {
			return GetTextSheetDisplayLines(textbox,-1,-1);
		}

		///<summary>Set start index or end index to -1 to return information for all lines.
		///Returns a list of strings representing the lines of sheet text which will display on screen when viewing the specified text.</summary>
		public static List<RichTextLineInfo> GetTextSheetDisplayLines(RichTextBox textbox,int startCharIndex,int endCharIndex) {
			List <RichTextLineInfo> listLines=new List<RichTextLineInfo>();
			int startLineIndex=0;
			int endLineIndex=0;
			if(startCharIndex==-1 || endCharIndex==-1) {//All lines.
				startLineIndex=0;
				endLineIndex=GetTextLineCount(textbox)-1;
			}
			else {
				startLineIndex=textbox.GetLineFromCharIndex(startCharIndex);
				endLineIndex=textbox.GetLineFromCharIndex(endCharIndex);
			}
			for(int i=startLineIndex;i<=endLineIndex;i++) {
				listLines.Add(GetOneTextLine(textbox,i));
			}
			return listLines;
		}

		///<summary>If lineIndex is past the last line, then the information of the last line will be returned. </summary>
		public static RichTextLineInfo GetOneTextLine(RichTextBox textbox,int lineIndex) {
			if(lineIndex<0 || GetTextLineCount(textbox)==0) {
				return new RichTextLineInfo();
			}
			RichTextLineInfo lineInfo=new RichTextLineInfo();
			//GetFirstCharIndexFromLine() returns -1 if lineIndex is past the last line.
			lineInfo.FirstCharIndex=textbox.GetFirstCharIndexFromLine(lineIndex);//-1 if lineIndex >= count of lines in rtb.
			if(lineInfo.FirstCharIndex==-1) {//Return the last line's information.
				lineIndex=GetTextLineCount(textbox)-1;
				lineInfo.FirstCharIndex=textbox.GetFirstCharIndexFromLine(lineIndex);//First character of last line.
			}
			Point posThisLine=textbox.GetPositionFromCharIndex(lineInfo.FirstCharIndex);
			Point posNextLine;
			if(lineIndex!=GetTextLineCount(textbox)-1) {//This is not the last line.
				posNextLine=textbox.GetPositionFromCharIndex(textbox.GetFirstCharIndexFromLine(lineIndex+1));//Top of next line=bottom of this line.
			}
			else { //Only do this "phony last line" logic when we are looking at the actual last line.  Trying to copy all the content/formatting of an 
				//entire RichTextBox causes all kinds of printing/Pdf/alignment/spacing issues in Sheets and Forms.  
				using(RichTextBox rtb=new RichTextBox()) {
					//Copy values to new RichTextBox because we might modify text below.
					rtb.Rtf=textbox.Rtf;
					//Setting the rtb.Font property in this RichTextBox causes an Out of Memory Exception when the Treatment Finder report attempted to run for 100+ 
					//pdfs. Attempts were made to employ a "using" to dispose of the font, as well as to ensure the textbox passing it in was disposed of, 
					//however neither method succeeded. Since the only importance of the Font in this context is getting the position of the Next Line, it is also 
					//not necessary here because we obtain that from the Rtf and postNextLine. It's removal should not change the user experience. However, it will
					//remain here, commented out, for future information so that others do not attempt to employ a font here.
					//rtb.Font=textbox.Font;
					rtb.Size=textbox.Size;
					//According to MSDN, "If no paragraph is selected in the control, setting this property applies the alignment setting to the paragraph in
					//which the insertion point appears as well as to paragraphs created after the paragraph that has the alignment property setting."
					//https://msdn.microsoft.com/en-us/library/system.windows.forms.richtextbox.selectionalignment(v=vs.110).aspx
					//When copying the RTF from the given textbox it also passes the selection to the new RichTextBox which is needed for the SelectionAlignment.
					rtb.SelectionAlignment=textbox.SelectionAlignment;
					rtb.AppendText("\r\n\t");//Add a phony line.
					posNextLine=rtb.GetPositionFromCharIndex(rtb.GetFirstCharIndexFromLine(lineIndex+1));//Top of next line=bottom of this line.
				}
			}
			lineInfo.Left=posThisLine.X;
			lineInfo.Top=posThisLine.Y;
			lineInfo.Bottom=posNextLine.Y;
			return lineInfo;
		}

		public static int GetTextLineCount(RichTextBox textbox) {
			if(textbox.Text.Length==0) {
				return 0;
			}
			return textbox.GetLineFromCharIndex(textbox.Text.Length-1)+1;//GetLineFromCharIndex() returns a zero-based index.
		}

		///<summary>Creates a textbox from the text sheetfield provided.  Used for display and for text wrapping calculations.
		///Calling methods are responsible for explicitly disposing of the returned text box itself and it's Font property.</summary>
		public static RichTextBox CreateTextBoxForSheetDisplay(string text,Font font,int width,int height,HorizontalAlignment align,bool isClipText=true){
			SheetField field=new SheetField();
			field.FieldType=SheetFieldType.StaticText;
			field.TabOrder=0;
			field.XPos=0;
			field.YPos=0;
			field.Width=width;
			field.ItemColor=Color.Black;
			field.TextAlign=align;
			field.FieldValue=text;
			field.FontIsBold=font.Bold;	
			field.FontName=font.Name;
			field.FontSize=font.Size;
			field.Height=height;
			return CreateTextBoxForSheetDisplay(field,isClipText);
		}

		///<summary>Creates a textbox from the text sheetfield provided.  Used for display and for text wrapping calculations.
		///Calling methods are responsible for explicitly disposing of the returned text box itself and it's Font property.</summary>
		public static RichTextBox CreateTextBoxForSheetDisplay(SheetField field,bool isClipText=true) {
			RichTextBox textbox=new RichTextBox();
			textbox.BorderStyle=BorderStyle.None;
			textbox.TabStop=false;//Only input fields allow tab stop (set below for input text).
			//Input fields need to have a yellow background so that they stand out to the user.
			if(field.FieldType==SheetFieldType.InputField) {
				textbox.BackColor=Color.FromArgb(245,245,200);
				textbox.TabStop=(field.TabOrder>0);
				textbox.TabIndex=field.TabOrder;
			}
			textbox.Location=new Point(field.XPos,field.YPos);
			textbox.Width=field.Width;
			textbox.ScrollBars=RichTextBoxScrollBars.None;
			if(field.ItemColor!=Color.FromArgb(0)){
				textbox.ForeColor=field.ItemColor;
			}
			textbox.SelectionAlignment=field.TextAlign;
			textbox.SelectedText=field.FieldValue;
			FontStyle style=FontStyle.Regular;
			if(field.FontIsBold) {
				style=FontStyle.Bold;
			}
			textbox.Font=new Font(field.FontName,field.FontSize,style);
			bool isSingleLineTextBox=(field.Height < textbox.Font.Height+2);
			//There is a chance that data can get truncated when loading text into a text box that has Multiline set to false.
			//E.g. the text "line 1\r\nline 2\r\nline 3" will get truncated to be "line 1"
			//This causes a nasty bug where the user could have filled out the sheet as a Web Form (which does not truncate the text) and signed the sheet.
			//The signature would invalidate if the office were to open the downloaded web form within Open Dental proper and simply click OK.
			//Therefore, only allow InputField text boxes to have Multiline set to false (which will stop users from making newlines).
			if(field.FieldType==SheetFieldType.InputField && isSingleLineTextBox) {
				textbox.Multiline=false;
			}
			else {//InputField that is too tall to be considered a "single line" text box or not an InputField.
				//An office can set up a sheet def with a static text field that has newlines but is only sized (height) for a single line of text.
				//Always display output to the user in a text box that has the capabilities of preserving anything the user wants to display.
				textbox.Multiline=true;
			}
			textbox.Height=field.Height;
			textbox.ScrollBars=RichTextBoxScrollBars.None;
			textbox.Tag=field;
			#region Text Clipping
			//For multi-line textboxes, clip vertically to remove lines which extend beyond the bottom of the field.
			if(isClipText && textbox.Text.Length > 0 && !isSingleLineTextBox) {
				List <RichTextLineInfo> listLines=GetTextSheetDisplayLines(textbox);
				int fitLineIndex=listLines.Count-1;//Line numbers are zero-based.
				while(fitLineIndex >= 0	&& listLines[fitLineIndex].Top+textbox.Font.Height-1 > textbox.Height) {//if bottom of line is past bottom of textbox
					fitLineIndex--;
				}
				if(fitLineIndex < listLines.Count-1) {//If at least one line was truncated from the bottom.
					textbox.Text=textbox.Text.Substring(0,listLines[fitLineIndex+1].FirstCharIndex);//Truncate text to the end of the fit line.
				}
			}
			//For single-line textboxes, clip the text to the width of the textbox.  This forces the display to look the same on screen as when printed.
			if(isClipText && textbox.Text.Length > 0 && isSingleLineTextBox) {
				int indexRight=textbox.GetCharIndexFromPosition(new Point(textbox.Width,0));
				if(indexRight < textbox.Text.Length-1) {//If the string is too wide for the textbox.
					textbox.Text=textbox.Text.Substring(0,indexRight+1);//Truncate the text to fit the textbox width.
				}
			}
			#endregion Text Clipping
			//Take over the ability to paste formatted text into the RTB.
			textbox.KeyDown+=(o,e) => {
				//Two different ways to paste are "Ctrl + V" and "Shift + Insert".
				if((e.Control && e.KeyCode==Keys.V) || (e.Shift && e.KeyCode==Keys.Insert)) {
					if(Clipboard.ContainsText()) {
						//Paste just the plain text value from the clipboard.
						textbox.Paste(DataFormats.GetFormat(DataFormats.Text));
					}
					e.Handled=true;//Do not let the base Paste function to be invoked because it allows pasting formatted text, pictures, etc.
				}
			};
			return textbox;
		}

		#region Scaling DPI
		///<summary>Converts pixels used by us (100dpi) to points used by PdfSharp.</summary>
		public static float PixelsToPoints(float pixels) {
			double inches=pixels/100d;//100 ppi
			XUnit xunit=XUnit.FromInch(inches);
			return (float)xunit.Point;
		}

		///<summary>Converts points used by PdfSharp to pixels used by us (100dpi).</summary>
		public static float PointsToPixels(float points) {
			double pointsPerInch=XUnit.FromInch(1).Point;
			double inches=points/pointsPerInch;
			return (float)(inches*100d);//100dpi
		}
		#endregion Scaling DPI

	}

	///<summary>Used to collect information about a single line of text within a RichTextBox.</summary>
	public class RichTextLineInfo {
		///<summary>The index of the first character on the line within the RichTextBox.</summary>
		public int FirstCharIndex=0;
		///<summary>The horizontral location in pixels where the text begins on the line.</summary>
		public int Left=0;
		///<summary>The vertical location in pixels where the text begins on the line.</summary>
		public int Top=0;
		///<summary>The vertical location in pixels where the text ends on the line.</summary>
		public int Bottom=0;

		///<summary>We get text in this manner so that we can wait as long as possible in the process before converting strings.
		///This lazy loading approach increases efficiency.</summary>
		public static string GetTextForLine(string text,List<RichTextLineInfo> listLines,int lineIndex) {
			if(lineIndex==listLines.Count-1) {//Last line
				return text.Substring(listLines[lineIndex].FirstCharIndex);
			}
			return text.Substring(listLines[lineIndex].FirstCharIndex,listLines[lineIndex+1].FirstCharIndex-listLines[lineIndex].FirstCharIndex);
		}

	}

}
