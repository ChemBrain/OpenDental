using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDental.UI;

namespace UnitTests
{
	public partial class FormButtonTest : Form
	{
		public FormButtonTest()
		{
			InitializeComponent();
		}

		private void FormButtonTest_Load(object sender, EventArgs e)
		{
			//CodeBase.OdThemeModernGrey.SetTheme(CodeBase.OdTheme.MonoFlatBlue);
		}

		private void ButDeleteProc_Click(object sender, EventArgs e)
		{
			
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			button2.Text="Longer text result";
		}
	}
}
