using System;
using System.Text;
using CodeBase;

namespace OpenDentBusiness {
	public class ReplaceTags {
		///<summary>Replaces one individual tag. Case insensitive.</summary>
		public static void ReplaceOneTag(StringBuilder template,string tagToReplace,string replaceWith,bool isHtmlEmail) {
			if(isHtmlEmail) {
				//Because we are sending emails as HTML, we need to escape characters that can cause our email text to become invalid HTML.
				replaceWith=(replaceWith??"").Replace(">","&>").Replace("<","&<");
			}
			//Note: RegReplace is case insensitive by default.
			template.RegReplace("\\"+tagToReplace,replaceWith??"");
		}
	}
}
