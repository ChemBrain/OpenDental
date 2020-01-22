using System.Drawing;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class DashLabel:Label,IDashWidgetField {
		private SheetField _sheetField;

		public DashLabel() {
			InitializeComponent();
		}

		public void SetData(PatientDashboardDataEventArgs data,SheetField sheetField) {
			if(sheetField==null) {
				return;
			}
			_sheetField=sheetField;//Only make a change if valid sheetField given.
		}

		public void RefreshData(Patient pat,SheetField sheetField) {
			_sheetField=sheetField;
		}

		public void RefreshView() {
			Text=_sheetField.FieldValue;
			TextAlign=ConvertToContentAlignment(_sheetField.TextAlign);
			string fontName=_sheetField.FontName;
			if(string.IsNullOrWhiteSpace(fontName)) {//sheet did not have a properly set FontName
				fontName=Font.FontFamily?.Name;//Use the control's default font.
			}
			Font=new Font(fontName,_sheetField.FontSize>0 ? _sheetField.FontSize : 8,_sheetField.FontIsBold ? FontStyle.Bold : FontStyle.Regular);
		}

		private ContentAlignment ConvertToContentAlignment(HorizontalAlignment align) {
			switch(align) {
				case HorizontalAlignment.Right:
					return ContentAlignment.TopRight;
				case HorizontalAlignment.Center:
					return ContentAlignment.TopCenter;
				case HorizontalAlignment.Left:
				default:
					return ContentAlignment.TopLeft;
			}
		}
	}
}
