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
	public partial class FormApptProvSliderTest : Form
	{
		public FormApptProvSliderTest()
		{
			InitializeComponent();
			contrApptProvSlider1.ProvBarText="Testing- 123";
		}
	}
}
