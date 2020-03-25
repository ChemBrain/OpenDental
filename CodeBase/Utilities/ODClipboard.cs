using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBase {
	public class ODClipboard {
		///<summary>Writes the text to the user's clipboard.</summary>
		public static void SetClipboard(string text) {
			if(ODBuild.IsWeb()) {
				ODCloudClient.SendDataToBrowser(text,(int)ODCloudClient.BrowserAction.SetClipboard);
			}
			else {
				Clipboard.SetText(text);
			}
		}

	}
}
