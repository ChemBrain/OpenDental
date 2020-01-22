using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDental.UI {
	public partial class ODCodeRangeFilter:UserControl {

		private bool _hideExampleText;

		[Category("Behavior"), Description("Hide the example text label.")]
		public bool HideExampleText {
			get {
				return _hideExampleText;
			}
			set {
				_hideExampleText=value;
				ResizeControl();
			}
		}

		/// <summary>Gets the start value of the control.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string StartRange {
			get {
				return GetRange(true);
			}
		}

		/// <summary>Gets the end value of the control.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string EndRange {
			get {
				return GetRange(false);
			}
		}

		private void ResizeControl() {
			this.Size=new Size(Width,37);
			labelExample.Visible=true;
			MaximumSize=new Size(MaximumSize.Width,37);
			if(HideExampleText) {
				this.Size=new Size(Width,20);
				labelExample.Visible=false;
				MaximumSize=new Size(MaximumSize.Width,20);
			}
		}

		public ODCodeRangeFilter() {
			InitializeComponent();
		}

		private string GetRange(bool getStart) {
			string codeStart="";
			string codeEnd="";
			if(!string.IsNullOrEmpty(textCodeRange.Text.Trim())) {
				if(textCodeRange.Text.Contains("-")) {
					string[] codeSplit=textCodeRange.Text.Split('-');
					codeStart=codeSplit[0].Trim().Replace('d','D');
					codeEnd=codeSplit[1].Trim().Replace('d','D');
				}
				else {
					codeStart=textCodeRange.Text.Trim().Replace('d','D');
					codeEnd=textCodeRange.Text.Trim().Replace('d','D');
				}
			}
			return (getStart ? codeStart : codeEnd);
		}
	}
}
