using System;
using OpenDentBusiness;
using CodeBase;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental {
	///<summary>A splash screen to show when the program launches.</summary>
	public partial class FormSplash : Form {

		///<summary>Launches a splash screen.</summary>
		public FormSplash() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSplash_Load(object sender,EventArgs e) {
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				BackgroundImage=Properties.Resources.splashCanada;
			}
			if(File.Exists(Directory.GetCurrentDirectory()+@"\Splash.jpg")) {
				BackgroundImage=new Bitmap(Directory.GetCurrentDirectory()+@"\Splash.jpg");
			}
			if(Plugins.PluginsAreLoaded) {
				Plugins.HookAddCode(this,"FormSplash.FormSplash_Load_end");
			}
		}

		
  }
}