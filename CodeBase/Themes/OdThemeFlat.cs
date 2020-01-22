using System.Drawing;

namespace CodeBase {
	///<summary>Flat theme</summary>
	public class OdThemeFlat:OdThemeBase {
		///<summary>Sets the theme to blue colors.</summary>
		public override void SetTheme() {
			base.SetTheme();
			//toolbar button
			_toolBarTogglePushedTopColor=Color.FromArgb(255,255,255);
			_toolBarTogglePushedBottomColor=_toolBarTogglePushedTopColor;
			_toolBarHoverTopColor=Color.FromArgb(192,193,216);
			_toolBarHoverBottomColor=_toolBarHoverTopColor;
			_toolBarPushedTopColor=Color.FromArgb(141,151,179);
			_toolBarPushedBottomColor=_toolBarPushedTopColor;
			SetFont(ref _toolBarFont,"Microsoft Sans Serif",8.25f,FontStyle.Regular);
			//outlook bar-------------------------------------------------------------------
			_outlookHoverCornerRadius=1;
			SetSolidBrush(ref _outlookHotBrush,Color.FromArgb(141,151,179));
			SetSolidBrush(ref _outlookPressedBrush,_toolBarHoverTopColor);//Pressed and selected need to be the same color if there is no gradient
			SetSolidBrush(ref _outlookSelectedBrush,_toolBarHoverTopColor);
			SetPen(ref _outlookOutlinePen,SystemColors.Control);
			SetOutlookImages(true);
			_isOutlookImageInverse=false;
		}
	}
}
