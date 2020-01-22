using System.Drawing;

namespace CodeBase {

	///<summary>The blue theme.</summary>
	public class OdThemeStandard:OdThemeBase {

		///<summary>Sets the theme to blue colors.</summary>
		public override void SetTheme() {
			base.SetTheme();
			//toolbar button
			_toolBarTogglePushedTopColor=Color.FromArgb(255,255,255);
			_toolBarTogglePushedBottomColor=Color.FromArgb(191,201,229);
			_toolBarHoverTopColor=Color.FromArgb(255,255,255);
			_toolBarHoverBottomColor=Color.FromArgb(171,181,209);
			_toolBarTopColor=Color.FromArgb(255,255,255);
			_toolBarBottomColor=Color.FromArgb(171,181,209);
			_toolBarPushedTopColor=Color.FromArgb(225,225,225);
			_toolBarPushedBottomColor=Color.FromArgb(141,151,179);
			SetFont(ref _toolBarFont,"Microsoft Sans Serif",8.25f,FontStyle.Regular);
			
		}

	}
}
