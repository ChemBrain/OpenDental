using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace CodeBase {
	///<summary>Contains all color information for the currently selected ODColorTheme</summary>
	public class ODColorTheme {
		#region Private Variables
		
		private static OdTheme _themeCur=OdTheme.None;
		private static ConcurrentDictionary<string,Action> _dictOnThemeChangedActions=new ConcurrentDictionary<string,Action>();
		///<summary>Locks objects so we don't half set them in the middle of changing themes.</summary>
		private static ReaderWriterLockSlim _locker=new ReaderWriterLockSlim();
		///<summary>True if the current theme has flat icons in main toolbar and main outlook bar.</summary>
		private static bool _hasFlatIcons=false;
		#region Private Toolbar Variables
		//odtoolbar
		///<summary>.</summary>
		protected static SolidBrush _toolBarTextForeBrush=null;
		///<summary>.</summary>
		protected static SolidBrush _toolBarTextForeBrushError=null;
		///<summary>.</summary>
		protected static Pen _toolBarPenOutlinePen=null;
		///<summary>.</summary>
		protected static Pen _toolBarPenOutlinePenError=null;
		///<summary>.</summary>
		protected static SolidBrush _toolBarTextDisabledBrush=null;
		///<summary>.</summary>
		protected static SolidBrush _toolBarTextDisabledBrushError=null;
		///<summary>.</summary>
		protected static Pen _toolBarDividerPen=null;
		///<summary>.</summary>
		protected static Pen _toolBarDividerPenError=null;
		///<summary></summary>
		protected static SolidBrush _toolBarDisabledOverlayBrush=null;
		///<summary>Toolbar Button Text Font.</summary>
		protected static Font _toolBarFont=null;
		///<summary></summary>
		protected static Color _toolBarTogglePushedTopColor;
		///<summary></summary>
		protected static Color _toolBarTogglePushedTopColorError;
		///<summary></summary>
		protected static Color _toolBarTogglePushedBottomColor;
		///<summary></summary>
		protected static Color _toolBarTogglePushedBottomColorError;
		///<summary></summary>
		protected static Color _toolBarHoverTopColor;
		///<summary></summary>
		protected static Color _toolBarHoverTopColorError;
		///<summary></summary>
		protected static Color _toolBarHoverBottomColor;
		///<summary></summary>
		protected static Color _toolBarHoverBottomColorError;
		///<summary></summary>
		protected static Color _toolBarTopColor;
		///<summary></summary>
		protected static Color _toolBarTopColorError;
		///<summary></summary>
		protected static Color _toolBarBottomColor;
		///<summary></summary>
		protected static Color _toolBarBottomColorError;
		///<summary></summary>
		protected static Color _toolBarPushedTopColor;
		///<summary></summary>
		protected static Color _toolBarPushedTopColorError;
		///<summary></summary>
		protected static Color _toolBarPushedBottomColor;
		///<summary></summary>
		protected static Color _toolBarPushedBottomColorError;
		///<summary></summary>
		protected static Color _colorNotify;
		///<summary></summary>
		protected static Color _colorNotifyDark;
		#endregion
		#region Private OutlookBar Variables
		//main modules outlook bar
		///<summary>Newer themes are square, others are rounded.</summary>
		protected static float _outlookHoverCornerRadius=0;
		///<summary>When an item is hovered over.</summary>
		protected static SolidBrush _outlookHotBrush=null;
		///<summary>When an item is pressed.</summary>
		protected static SolidBrush _outlookPressedBrush=null;
		///<summary>When the item is currently selected.</summary>
		protected static SolidBrush _outlookSelectedBrush=null;
		///<summary>Outline.</summary>
		protected static Pen _outlookOutlinePen=null;
		///<summary>Text Color.</summary>
		protected static SolidBrush _outlookTextBrush=null;
		///<summary>Background color.</summary>
		protected static SolidBrush _outlookBackBrush=null;
		///<summary>.</summary>
		protected static int _outlookApptImageIndex=0;
		///<summary>.</summary>
		protected static int _outlookFamilyImageIndex=1;
		///<summary>.</summary>
		protected static int _outlookAcctImageIndex=2;
		///<summary>.</summary>
		protected static int _outlookTreatPlanImageIndex=3;
		///<summary>.</summary>
		protected static int _outlookChartImageIndex=4;
		///<summary>.</summary>
		protected static int _outlookImagesImageIndex=5;
		///<summary>.</summary>
		protected static int _outlookManageImageIndex=6;
		///<summary>.</summary>
		protected static int _outlookEcwTreatPlanImageIndex=7;
		///<summary>.</summary>
		protected static int _outlookEcwChartImageIndex=8;
		///<summary></summary>
		protected static bool _isOutlookImageInverse=false;
		#endregion
		#region ODForm
		///<summary></summary>
		protected static Color _formBackColor;
		protected static Font _fontMenuItem;
		#endregion

		#endregion

		#region Getters

		///<summary>The current theme in use.</summary>
		public static OdTheme ThemeCur {
			get { return _themeCur; }
		}

		private static T Get<T>(ref T obj) {
			if(_themeCur==OdTheme.None) {
				SetTheme(OdTheme.Standard);//Locks for write to block reading.
			}
			_locker.EnterReadLock();//Block writing.
			try {
				return obj;
			}
			finally {
				_locker.ExitReadLock();
			}
		}

		///<summary>True if the current theme has flat icons in main toolbar and main outlook bar.</summary>
		public static bool HasFlatIcons {
			get { return _hasFlatIcons;	}
		}

		///<summary>The same orange color as the unread task notification color.</summary>
		public static Color ColorNotify {
			get {return _colorNotify; }
		}

		///<summary>The darker version of ColorNotify.  This color is useful for coloring grid cell text, because it contrasts well with the 
		///normal white background, as well as the slate gray background of selected cells, as well as the normal black text.  Calculated by using website 
		///paletton.com.  This darker color is not calculated by using a simple division or subtraction,
		///but is instead calculated using a more advanced formula.</summary>
		public static Color ColorNotifyDark {
			get { return _colorNotifyDark;}
		}

		///<summary>Usually set to the same font as the rest of the theme. This has been created specifically for menuItems since their font size 
		///sometimes needs to be different. </summary>
		public static Font FontMenuItem {
			get { return Get(ref _fontMenuItem);}
		}

		///<summary>Originally for OdForm.  The background color of the form.</summary>
		public static Color FormBackColor {
			get {	return Get(ref _formBackColor); }
		}

		///<summary>Originally used for outlook bar.  Booelan for if the images of an opposite color scheme (ex. white instead of black).</summary>
		public static bool IsImageInverse {
			get { return Get(ref _isOutlookImageInverse); }
		}

		///<summary>Originally used for outlook bar.  The index of the image for the account module.</summary>
		public static int OutlookAcctImageIndex {
			get { return Get(ref _outlookAcctImageIndex); }
		}

		///<summary>Originally used for outlook bar.  The index of the image for the appointment module.</summary>
		public static int OutlookApptImageIndex {
			get { return Get(ref _outlookApptImageIndex); }
		}

		///<summary>Originally used for outlook bar.  The background color of the outlook bar buttons when not selected.</summary>
		public static SolidBrush OutlookBackBrush {
			get { return Get(ref _outlookBackBrush); }
		}

		///<summary>Originally used for outlook bar.  The index of the image for the chart module.</summary>
		public static int OutlookChartImageIndex {
			get { return Get(ref _outlookChartImageIndex); }
		}

		///<summary>Originally used for outlook bar.  The index of the image for ecw chart.</summary>
		public static int OutlookEcwChartImageIndex {
			get { return Get(ref _outlookEcwChartImageIndex); }
		}

		///<summary>Originally used for outlook bar.  The index of the image for ecw treatment plan.</summary>
		public static int OutlookEcwTreatPlanImageIndex {
			get { return Get(ref _outlookEcwTreatPlanImageIndex); }
		}

		///<summary>Originally used for outlook bar.  The index of the image for the family module.</summary>
		public static int OutlookFamilyImageIndex {
			get { return Get(ref _outlookFamilyImageIndex); }
		}

		///<summary>Originally used for outlook bar.  The color of the outlook button when hovering over it.</summary>
		public static SolidBrush OutlookHotBrush {
			get { return Get(ref _outlookHotBrush); }
		}

		///<summary>Originally used for outlook bar.  The roundness of the outlook button corners.</summary>
		public static float OutlookHoverCornerRadius {
			get { return Get(ref _outlookHoverCornerRadius); }
		}

		///<summary>Originally used for outlook bar.  The index of the image for image module.</summary>
		public static int OutlookImagesImageIndex {
			get { return Get(ref _outlookImagesImageIndex); }
		}

		///<summary>Originally used for outlook bar.  The index of the image for the manage module.</summary>
		public static int OutlookManageImageIndex {
			get { return Get(ref _outlookManageImageIndex); }
		}

		///<summary>Originally used for outlook bar.  The color of the outline of the outlook bar button when it is selected.</summary>
		public static Pen OutlookOutlineColor {
			get { return Get(ref _outlookOutlinePen); }
		}

		///<summary>Originally used for outlook bar.  The color of the outlook bar button when it is actively being pressed.</summary>
		public static SolidBrush OutlookPressedBrush {
			get { return Get(ref _outlookPressedBrush); }
		}

		///<summary>Originally used for outlook bar.  The color of the outlook button when it is seleted.</summary>
		public static SolidBrush OutlookSelectedBrush {
			get { return Get(ref _outlookSelectedBrush); }
		}

		///<summary>Originally used for outlook bar.  The text color of the outlook bar buttons.</summary>
		public static SolidBrush OutlookTextBrush {
			get { return Get(ref _outlookTextBrush); }
		}

		///<summary>Originally used for outlook bar.  The index of the image for the treatment plan module.</summary>
		public static int OutlookTreatPlanImageIndex {
			get { return Get(ref _outlookTreatPlanImageIndex); }
		}

		///<summary>Originally used in the toolbar.  The font of toolbar buttons.</summary>
		public static Font ToolBarFont {
			get { return Get(ref _toolBarFont); }
		}

		///<summary>Originally used in the toolbar.</summary>
		public static SolidBrush ToolBarDisabledOverlayBrush {
			get { return Get(ref _toolBarDisabledOverlayBrush); }
		}

		///<summary>Originally used in the toolbar.  The color of the vertical line that separates tool bar buttons.</summary>
		public static Pen ToolBarDividerPen {
			get { return Get(ref _toolBarDividerPen); }
		}

		///<summary>Originally used in the toolbar.  The color of the vertical line that separates error tool bar buttons.</summary>
		public static Pen ToolBarDividerPenError {
			get { return Get(ref _toolBarDividerPenError); }
		}

		///<summary>Originally used in the toolbar.  The color of the toolbar button outline.</summary>
		public static Pen ToolBarOutlinePen {
			get { return Get(ref _toolBarPenOutlinePen); }
		}

		///<summary>Originally used in the toolbar.  The color of the error toolbar button outline.</summary>
		public static Pen ToolBarPenOutlinePenError {
			get { return Get(ref _toolBarPenOutlinePenError); }
		}

		///<summary>Originally used in the toolbar.  The color of the text for toolbar buttons when disabled.</summary>
		public static SolidBrush ToolBarTextDisabledBrush {
			get { return Get(ref _toolBarTextDisabledBrush); }
		}

		///<summary>Originally used in the toolbar.  The color of the text for toolbar buttons when disabled.</summary>
		public static SolidBrush ToolBarTextDisabledBrushError {
			get { return Get(ref _toolBarTextDisabledBrushError); }
		}

		///<summary>Originally used in the toolbar.  The main color of the text for toolbar buttons.</summary>
		public static SolidBrush ToolBarTextForeBrush {
			get { return Get(ref _toolBarTextForeBrush); }
		}

		///<summary>Originally used in the toolbar.  The main color of the text for toolbar buttons.</summary>
		public static SolidBrush ToolBarTextForeBrushError {
			get { return Get(ref _toolBarTextForeBrushError); }
		}

		///<summary>Originally used in the toolbar.  The top gradient color of the tool bar button when pressed and is a toggle</summary>
		public static Color ToolBarTogglePushedTopColor {
			get { return Get(ref _toolBarTogglePushedTopColor); }
		}

		///<summary>Originally used in the toolbar.  The top gradient color of the tool bar button when pressed, and is a toggle
		///Created from algorithm Color=(Math.Min(255,baseColor.R+20,),Math.Min(255,baseColor.G+20),Math.Min(255,baseColor.B+20))</summary>
		public static Color ToolBarTogglePushedTopColorError {
			get { return Get(ref _toolBarTogglePushedTopColorError); }
		}

		///<summary>Originally used in the toolbar.  The bottomw color of the tool bar button when pressed and is a toggle
		///Created from algorithm. newColor=(Math.Min(255,baseColor.R+20,),Math.Min(255,baseColor.G+20),Math.Min(255,baseColor.B+20))</summary>
		public static Color ToolBarTogglePushedBottomColor {
			get { return Get(ref _toolBarTogglePushedBottomColor); }
		}

		///<summary>Originally used in the toolbar.  The bottomw color of the tool bar button when pressed and is a toggle and error.</summary>
		public static Color ToolBarTogglePushedBottomColorError {
			get { return Get(ref _toolBarTogglePushedBottomColorError); }
		}

		///<summary>Originally used in the toolbar.  The top color of the tool bar button when hovered over.</summary>
		public static Color ToolBarHoverTopColor {
			get { return Get(ref _toolBarHoverTopColor); }
		}

		///<summary>Originally used in the toolbar.  The top color of the tool bar button when hovered over.</summary>
		public static Color ToolBarHoverTopColorError {
			get { return Get(ref _toolBarHoverTopColorError); }
		}

		///<summary>Originally used in the toolbar.  The bottom color of the tool bar button when hovered over.</summary>
		public static Color ToolBarHoverBottomColor {
			get { return Get(ref _toolBarHoverBottomColor); }
		}

		///<summary>Originally used in the toolbar.  The bottom color of the tool bar button when hovered over.</summary>
		public static Color ToolBarHoverBottomColorError {
			get { return Get(ref _toolBarHoverBottomColorError); }
		}

		///<summary>Originally used in the toolbar.  The top color of the tool bar buttons.</summary>
		public static Color ToolBarTopColor {
			get { return Get(ref _toolBarTopColor); }
		}

		///<summary>Originally used in the toolbar.  The top color of the tool bar buttons.</summary>
		public static Color ToolBarTopColorError {
			get { return Get(ref _toolBarTopColorError); }
		}

		///<summary>Originally used in the toolbar.  The bottom color of the toolbar buttons.</summary>
		public static Color ToolBarBottomColor {
			get { return Get(ref _toolBarBottomColor); }
		}

		///<summary>Originally used in the toolbar.  The bottom color of the toolbar buttons.</summary>
		public static Color ToolBarBottomColorError {
			get { return Get(ref _toolBarBottomColorError); }
		}

		///<summary>Originally used in the toolbar.  The top color of the toolbar buttons when they're disabled.</summary>
		public static Color ToolBarPressedTopColor {
			get { return Get(ref _toolBarPushedTopColor); }//used to be tool bar dark color
		}

		///<summary></summary>
		public static Color ToolBarPressedTopColorError {
			get {	return Get(ref _toolBarPushedTopColorError); }
		}

		///<summary>Originally used in the toolbar.  The bottom color of the toolbar buttons when thye're disabled.</summary>
		public static Color ToolBarPressedBottomColor {
			get { return Get(ref _toolBarPushedBottomColor); }//used to be tool bar dark color
		}

		///<summary></summary>
		public static Color ToolBarPressedBottomColorError {
			get { return Get(ref _toolBarPushedBottomColorError); }
		}
		#endregion Getters

		#region Setters
		///<summary>Disposes of the previous brush (do not pass in a system brush), then initializes the font using the given parameters.</summary>
		protected static void SetSolidBrush(ref SolidBrush brush,Color color) {
			if(brush==null) {
				brush=new SolidBrush(color);
			}
			else {
				brush.Color=color;
			}			
		}

		///<summary>Disposes of the previous pen (do not pass in a system pen), then initializes the font using the given parameters.</summary>
		protected static void SetPen(ref Pen pen,Color color) {
			Pen tempPen=pen;
			pen=new Pen(color);
			if(tempPen!=null) {
				tempPen.Dispose();
				tempPen=null;
			}
		}

		///<summary>Disposes of the previous font if it is not a system font, then initializes the font using the given parameters.</summary>
		protected static void SetFont(ref Font font,FontFamily family,float emSize,FontStyle style) {
			if(font!=null && font.FontFamily==family && font.Size==emSize && font.Style==style) {
				return;//No changed needed.
			}
			Font tempFont=font;
			font=new Font(family,emSize,style);
			if(tempFont!=null && !tempFont.IsSystemFont) {
				//For some reason disposing the font was causing errors when switching the theme.
				//Theme switching does not happen often, thus we are OK with leaving it floating in memory.
				//tempFont.Dispose();
				tempFont=null;
			}			
		}

		///<summary>Disposes of the previous font if it is not a system font, then initializes the font using the given parameters.</summary>
		protected static void SetFont(ref Font font,string familyName,float emSize,FontStyle style) {
			if(font!=null && font.FontFamily.Name==familyName && font.Size==emSize && font.Style==style) {
				return;//No changed needed.
			}
			Font tempFont=font;
			font=new Font(familyName,emSize,style);
			if(tempFont!=null && !tempFont.IsSystemFont) {
				//For some reason disposing the font was causing errors when switching the theme.
				//Theme switching does not happen often, thus we are OK with leaving it floating in memory.
				//tempFont.Dispose();
				tempFont=null;
			}
		}

		/// <summary>Sets the and reloads the theme colors based on the passed in enum.</summary>
		public static void SetTheme(OdTheme themeCur) {
			if(_themeCur==themeCur) {
				return;
			}
			_locker.EnterWriteLock();
			try {
				_themeCur=themeCur;
				ODColorTheme themeSetter=null;
				switch(themeCur) {
					case OdTheme.Standard:
						themeSetter=new OdThemeStandard();
						break;
					case OdTheme.Flat:
						themeSetter=new OdThemeFlat();
						break;
					default:
						themeSetter=new OdThemeStandard();
						break;
				}
				themeSetter.SetTheme();
			}
			finally {
				_locker.ExitWriteLock();
			}
			foreach(Action action in _dictOnThemeChangedActions.Values) {
				action.Invoke();
			}
		}

		///<summary>Overridden</summary>
		public virtual void SetTheme() {
		}

		///<summary></summary>
		protected static void SetOutlookImages(bool hasFlatIcons) {
			_hasFlatIcons=hasFlatIcons;
			if(hasFlatIcons) {
				_outlookApptImageIndex					=9;
				_outlookFamilyImageIndex				=10;
				_outlookAcctImageIndex					=11;
				_outlookTreatPlanImageIndex			=12;
				_outlookChartImageIndex					=13;
				_outlookImagesImageIndex				=14;
				_outlookManageImageIndex				=15;
				_outlookEcwTreatPlanImageIndex	=12;
				_outlookEcwChartImageIndex			=13;
			}
			else {
				_outlookApptImageIndex					=0;
				_outlookFamilyImageIndex				=1;
				_outlookAcctImageIndex					=2;
				_outlookTreatPlanImageIndex			=3;
				_outlookChartImageIndex					=4;
				_outlookImagesImageIndex				=5;
				_outlookManageImageIndex				=6;
				_outlookEcwTreatPlanImageIndex	=7;
				_outlookEcwChartImageIndex			=8;
			}
		}
		#endregion Setters

		///<summary>Thread safe.  If the same action is added multiple times, then the action will only be called once when the theme is changed.</summary>
		public static void AddOnThemeChanged(Action action) {
			_dictOnThemeChangedActions[action.Method.ToString()]=action;
		}

		///<summary></summary>
		public static Image InvertImageIfNeeded(Image img) {
			if(!IsImageInverse) {
				return img;
			}
			Bitmap inversion=new Bitmap(img);
			for(int y=0;(y<=(inversion.Height-1));y++) {
				for(int x=0;(x<=(inversion.Width-1));x++) {
					Color inv=inversion.GetPixel(x,y);
					inv=Color.FromArgb(inv.A,(255-inv.R),(255-inv.G),(255-inv.B));
					inversion.SetPixel(x,y,inv);
				}
			}
			return inversion;
		}

	}

	///<summary>.</summary>
	public enum OdTheme {
		///<summary>-1 - Do not save to database.  Only used during initalization to indicate that a default must be set.</summary>
		[Description("None")]
		None=-1,
		///<summary>0 - Gradient style. The default theme.</summary>
		[Description("Standard")]
		Standard,
		///<summary>1 - Flat styel</summary>
		[Description("Flat")]
		Flat,
	}

}
