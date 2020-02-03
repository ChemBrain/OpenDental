using System;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Text.RegularExpressions;
using System.Linq;
using CodeBase;
using System.Diagnostics;

namespace OpenDental {
	public partial class FormEmailEdit:ODForm {
		///<summary></summary>
		private bool _isInvalidPreview;
		///<summary>Used to handle the webBrowser navigation events in the case a user clicks the external link in the edit window. </summary>
		private bool _isLoading;
		///<summary>The message text that will be displayed in plain text on the left side.</summary>
		public string MarkupText;
		///<summary>The full HTML text of the email.</summary>
		public string HtmlText { get;private set; }
		///<summary>When true, the img tags will contain paths to local temp files where the images are stored. If false, the images
		///need to be downloaded from the cloud.</summary>
		public bool AreImagesDownloaded { get; private set; }
		///<summary>True if the text includes the entire HTML document and is not using the master template.</summary>
		public bool IsRaw;
		///<summary>If true, will not let the user click OK if the tag [EmailDisclaimer] is not present.</summary>
		public bool DoCheckForDisclaimer;
		///<summary>Defaults to true. If false, the user cannot save this as a raw email.</summary>
		public bool IsRawAllowed=true;

		public FormEmailEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEmailEdit_Load(object sender,EventArgs e) {
			SetFilterControlsAndAction(() => RefreshHTML(),
				(int)TimeSpan.FromSeconds(0.5).TotalMilliseconds,
				textContentEmail);
			ResizeControls();
			textContentEmail.Text=MarkupText;//display the message text in plain text on the left side. 
			textContentEmail.Focus();
			textContentEmail.SelectionStart=0;
			textContentEmail.SelectionLength=0;
			textContentEmail.ScrollToCaret();
			checkIsRaw.Checked=IsRaw;
			//Per Nathan, discussing I2339 and E19146, this checkbox and label should not show if we would block the user from interacting with it.
			checkIsRaw.Visible=IsRawAllowed;
			labelPlainText.Visible=IsRawAllowed;
			RefreshHTML();
			_isLoading=true;
		}

		private void FormEmailEdit_SizeChanged(object sender,EventArgs e) {
			ResizeControls();
		}

		private void ResizeControls() {
			//Manually setting height due to a bug that caused textBoxes to expand over buttons when form minimized and then maximized again. 
			textContentEmail.Height=this.Height-(toolBarTop.Height+toolBarBottom.Height+butOk.Height+labelPreview.Height+50);//50 is the extra white space
			webBrowserEmail.Height=this.Height-(toolBarTop.Height+toolBarBottom.Height+butOk.Height+labelPreview.Height+50);
			//text resize
			textContentEmail.Width=ClientSize.Width/2-2-textContentEmail.Left;
			////Browser resize
			webBrowserEmail.Left=ClientSize.Width/2+2;
			webBrowserEmail.Width=ClientSize.Width/2-2;
			labelPreview.Left=webBrowserEmail.Location.X;
			//Toolbar resize
			toolBarTop.Width=ClientSize.Width;
			toolBarBottom.Width=ClientSize.Width;
			toolBarTop.Invalidate();
			toolBarBottom.Invalidate();
			LayoutToolBars();
		}

		private void LayoutToolBars() {
			toolBarTop.Buttons.Clear();
			toolBarTop.Buttons.Add(new ODToolBarButton(Lan.g(this,"Setup"),19,Lan.g(this,"Setup master page and styles."),"Setup"));
			toolBarTop.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			toolBarTop.Buttons.Add(new ODToolBarButton(Lan.g(this,"Ext Link"),8,"","Ext Link"));
			toolBarTop.Buttons.Add(new ODToolBarButton(Lan.g(this,"Heading1"),9,"","H1"));
			toolBarTop.Buttons.Add(new ODToolBarButton(Lan.g(this,"Heading2"),10,"","H2"));
			toolBarTop.Buttons.Add(new ODToolBarButton(Lan.g(this,"Heading3"),11,"","H3"));
			toolBarTop.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			toolBarTop.Buttons.Add(new ODToolBarButton(Lan.g(this,"Table"),15,"","Table"));
			toolBarTop.Buttons.Add(new ODToolBarButton(Lan.g(this,"Image"),16,"","Image"));
			//The autograph button and drop down
			ODToolBarButton buttonAutograph=new ODToolBarButton(Lan.g(this,"Autograph"),-1,"","Autograph");
			buttonAutograph.Style=ODToolBarButtonStyle.DropDownButton;
			FillAutographDropdown();
			buttonAutograph.DropDownMenu=menuAutographDropdown;
			toolBarTop.Buttons.Add(buttonAutograph);
			//Create the tool bar on the bottom
			toolBarBottom.Buttons.Clear();
			toolBarBottom.Buttons.Add(new ODToolBarButton(Lan.g(this,"Cut"),3,"","Cut"));
			toolBarBottom.Buttons.Add(new ODToolBarButton(Lan.g(this,"Copy"),4,"","Copy"));
			toolBarBottom.Buttons.Add(new ODToolBarButton(Lan.g(this,"Paste"),5,"","Paste"));
			toolBarBottom.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			toolBarBottom.Buttons.Add(new ODToolBarButton(Lan.g(this,"Undo"),6,"","Undo"));
			toolBarBottom.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			toolBarBottom.Buttons.Add(new ODToolBarButton(Lan.g(this,"Bold"),12,"","Bold"));
			toolBarBottom.Buttons.Add(new ODToolBarButton(Lan.g(this,"Italic"),13,"","Italic"));
			toolBarBottom.Buttons.Add(new ODToolBarButton(Lan.g(this,"Color"),14,"","Color"));
			toolBarBottom.Buttons.Add(new ODToolBarButton(Lan.g(this,"Font"),17,"","Font"));
		}

		private void toolBarTop_ButtonClick(object sender,ODToolBarButtonClickEventArgs e) {
			switch(e.Button.Tag.ToString()) {
				case "Setup":
					Setup_Click();
					break;
				case "Ext Link":
					Ext_Link_Click();
					break;
				case "H1": 
					H1_Click(); 
					break;
				case "H2": 
					H2_Click(); 
					break;
				case "H3": 
					H3_Click(); 
					break;
				case "Table": 
					Table_Click();
					break;
				case "Image":
					Image_Click();
					break;
				case "Autograph":
					Autograph_Click();
					break;
			}
		}

		private void toolBarBottom_ButtonClick(object sender,ODToolBarButtonClickEventArgs e) {
			switch(e.Button.Tag.ToString()) {
				case "Cut":
					Cut_Click();
					break;
				case "Copy":
					Copy_Click();
					break;
				case "Paste":
					Paste_Click();
					break;
				case "Undo":
					Undo_Click();
					break;
				case "Bold":
					Bold_Click();
					break;
				case "Italic":
					Italic_Click();
					break;
				case "Color":
					Color_Click();
					break;
				case "Font":
					Font_Click();
					break;
			}
		}

		private void toolBarAutograph_MenuItemClick(object sender,EventArgs e) {
			if(sender.GetType()==typeof(MenuItem)) {
				MenuItem itemCur=(MenuItem)sender;
				if(itemCur.Tag.GetType()==typeof(EmailAutograph)) {
					EmailAutograph autograph=(EmailAutograph)itemCur.Tag;
					InsertAutograph(autograph);
				}
			}
		}

		private void FillAutographDropdown() {
			menuAutographDropdown.MenuItems.Clear();
			foreach(EmailAutograph autograph in EmailAutographs.GetDeepCopy()) {
				MenuItem menuCur=new MenuItem();
				menuCur.Tag=autograph;
				menuCur.Text=autograph.Description;
				menuCur.Click+=toolBarAutograph_MenuItemClick;
				menuAutographDropdown.MenuItems.Add(menuCur);
			}
		}

		private void Setup_Click() {
			FormEmailMasterTemplate FormESMT=new FormEmailMasterTemplate();
			FormESMT.ShowDialog();
			if(FormESMT.DialogResult!=DialogResult.OK) {
				return;
			}
		}

		private void Ext_Link_Click() {
			FormExternalLink FormEL=new FormExternalLink();
			FormEL.ShowDialog();
			int tempStart=textContentEmail.SelectionStart;
			if(FormEL.DialogResult!=DialogResult.OK) {
				return;
			}
			textContentEmail.SelectedText="<a href=\""+FormEL.URL+"\">"+FormEL.DisplayText+"</a>";
			textContentEmail.SelectionLength=0;
			if(FormEL.URL=="" && FormEL.DisplayText=="") {
				textContentEmail.SelectionStart=tempStart+11;
			}
			textContentEmail.Focus();
		}

		private void Bold_Click() {
			MarkupL.AddTag("<b>","</b>",textContentEmail);
		}

		private void Italic_Click() {
			MarkupL.AddTag("<i>","</i>",textContentEmail);
		}

		private void Color_Click() {
			MarkupL.AddTag("[[color:red|","]]",textContentEmail);
		}

		private void Font_Click() {
			MarkupL.AddTag("[[font:courier|","]]",textContentEmail);
		}

		private void H1_Click() {
			MarkupL.AddTag("<h1>","</h1>",textContentEmail);
		}

		private void H2_Click() {
			MarkupL.AddTag("<h2>","</h2>",textContentEmail);
		}

		private void H3_Click() {
			MarkupL.AddTag("<h3>","</h3>",textContentEmail);
		}		

		private void Table_Click() {
			int idx=textContentEmail.SelectionStart;
			if(TableOrDoubleClick(idx)) {
				return;//so it was already handled with an edit table dialog
			}
			//User did not click inside a table, so they must want to add a new table.
			FormMarkupTableEdit FormMTE=new FormMarkupTableEdit();
			FormMTE.Markup=@"{|
!Width=""100""|Heading1!!Width=""100""|Heading2!!Width=""100""|Heading3
|-
|||||
|-
|||||
|}";
			FormMTE.IsNew=true;
			FormMTE.ShowDialog();
			if(FormMTE.DialogResult!=DialogResult.OK){
				return;
			}
			textContentEmail.SelectionLength=0;
			textContentEmail.SelectedText=FormMTE.Markup;
			textContentEmail.SelectionLength=0;
			textContentEmail.Focus();
		}

		private void Image_Click() {
			//Store the images in their own a-z folder and retrieve them from there.
			string emailFolder="";
			try {
				emailFolder=ImageStore.GetEmailImagePath();//will create the folder if it doesn't exist already. 
			}
			catch(Exception ex) {
				FriendlyException.Show(Lans.g(this,"Unable to find Email Images folder"),ex);
				return;
			}
			FormImagePicker FormIP=new FormImagePicker(emailFolder); 
			FormIP.ShowDialog();
			if(FormIP.DialogResult!=DialogResult.OK) {
				return;
			}
			textContentEmail.SelectionLength=0;
			textContentEmail.SelectedText="[[img:"+FormIP.SelectedImageName+"]]";
		}

		private void Autograph_Click() {
			FormEmailAutographEdit formEAE=new FormEmailAutographEdit(new EmailAutograph(),isNew:true);
			if(formEAE.ShowDialog()==DialogResult.OK) {
				EmailAutographs.RefreshCache();
				FillAutographDropdown();
				InsertAutograph(formEAE.EmailAutographCur);
			}
		}

		private void Cut_Click() {
			textContentEmail.Cut();
			textContentEmail.Focus();
		}

		private void Copy_Click() {
			textContentEmail.Copy();
			textContentEmail.Focus();
		}

		private void Paste_Click() {
			textContentEmail.Paste();
			textContentEmail.Focus();
		}

		private void Undo_Click() {
			textContentEmail.Undo();
			textContentEmail.Focus();
		}

		private void InsertAutograph(EmailAutograph autograph) {
			textContentEmail.SelectionLength=0;
			textContentEmail.SelectedText=autograph.AutographText;
		}

		///<summary>This is called both when a user double clicks anywhere in the edit box, or when the click the Table button in the toolbar.  This ONLY 
		///handles popping up an edit window for an existing table.  If the cursor was not in an existing table, then this returns false.  After that, 
		///the behavior in the two areas differs.  Returns true if it popped up.</summary>
		private bool TableOrDoubleClick(int charIdx){
			MatchCollection matches=Regex.Matches(textContentEmail.Text,@"\{\|\n.+?\n\|\}",RegexOptions.Singleline);
			//Find the table match
			Match matchCur=matches.OfType<Match>().ToList().FirstOrDefault(x=>x.Index<=charIdx && x.Index+x.Length>=charIdx);
			//handle the clicks
			if(matchCur==null) {
				return false;//did not click inside a table
			}
			bool isLastCharacter=matchCur.Index+matchCur.Length==textContentEmail.Text.Length;
			textContentEmail.SelectionLength=0;//otherwise we get an annoying highlight
			//==Travis 11/20/15:  If we want to fix wiki tables in the future so duplicate tables dont both get changed from a double click, we'll need to
			//   use a regular expression to find which match of strTableLoad the user clicked on, and only replace that match below.
			FormMarkupTableEdit formT=new FormMarkupTableEdit();
			formT.Markup=matchCur.Value;
			formT.CountTablesInPage=matches.Count;
			formT.ShowDialog();
			if(formT.DialogResult!=DialogResult.OK) {
				return true;
			}
			if(formT.Markup==null) {//indicates delete
				textContentEmail.Text=textContentEmail.Text.Remove(matchCur.Index,matchCur.Length);
				textContentEmail.SelectionLength=0;
				return true;
			}
			textContentEmail.Text=textContentEmail.Text.Substring(0,matchCur.Index)//beginning of markup
				+formT.Markup//replace the table
				+(isLastCharacter ? "" : textContentEmail.Text.Substring(matchCur.Index+matchCur.Length));//continue to end, if any text after table markup.
			textContentEmail.SelectionLength=0;
			return true;
		}

		private void textContentEmail_KeyPress(object sender,KeyPressEventArgs e) {
			//this doesn't always fire, which is good because the user can still use the arrow keys to move around.
			//look through all tables:
			MatchCollection matches = Regex.Matches(textContentEmail.Text,@"\{\|\n.+?\n\|\}",RegexOptions.Singleline);
			//MatchCollection matches = Regex.Matches(textContent.Text,
			//	@"(?<=(?:\n|^))" //Checks for preceding newline or beggining of file
			//	+@"\{\|.+?\n\|\}" //Matches the table markup.
			//	+@"(?=(?:\n|$))" //Checks for following newline or end of file
			//	,RegexOptions.Singleline);
			foreach(Match match in matches) {
				if(textContentEmail.SelectionStart >	match.Index
					&& textContentEmail.SelectionStart <	match.Index+match.Length) 
				{
					e.Handled=true;
					MsgBox.Show(this,"Direct editing of tables is not allowed here.  Use the table button or double click to edit.");
					return;
				}
			}
			//Tab character isn't recognized by our custom markup.  Replace all tab chars with three spaces.
			if(e.KeyChar=='\t') {
				textContentEmail.SelectedText="   ";
				e.Handled=true;
			}
		}

		private void textContentEmail_MouseDoubleClick(object sender,MouseEventArgs e) {
			int idx=textContentEmail.GetCharIndexFromPosition(e.Location);
			TableOrDoubleClick(idx);//we don't care what this returns because we don't want to do anything else.
		}

		private void webBrowserEmail_Navigating(object sender,WebBrowserNavigatingEventArgs e) {
			if(_isLoading) {
				return;
			}
			e.Cancel=true;//Cancel browser navigation (for links clicked within the email message).
			if(e.Url.AbsoluteUri=="about:blank") {
				return;
			}
			//if user did not specify a valid url beginning with http:// then the event args would make the url start with "about:" 
			//ex: about:www.google.com and then would ask the user to get a separate app to open the link since it is unrecognized
			string url=e.Url.ToString();
			if(url.StartsWith("about")) {
				url=url.Replace("about:","http://");
			}
			Process.Start(url);//Instead launch the URL into a new default browser window.
		}

		private void webBrowserEmail_Navigated(object sender,WebBrowserNavigatedEventArgs e) {
			_isLoading=false;
		}

		private void checkIsRaw_CheckedChanged(object sender,EventArgs e) {
			if(!IsRawAllowed) {
				if(checkIsRaw.Checked) {
					checkIsRaw.Checked=false;
					MsgBox.Show(this,"This feature does not support raw HTML emails.");
				}
				return;
			}
			RefreshHTML();
			bool canEnableButtons=!checkIsRaw.Checked;//some buttons should be disabled when RAW is set. 
			SetWikiToolBarItems(canEnableButtons);
		}

		private void RefreshHTML() {
			webBrowserEmail.AllowNavigation=true;
			_isLoading=true;
			try {
				var text="";
				if(checkIsRaw.Checked) {
					text=textContentEmail.Text;
				}
				else {
					text=MarkupEdit.TranslateToXhtml(textContentEmail.Text,true,false,true);
					AreImagesDownloaded=true;
				}
				webBrowserEmail.DocumentText=text;
				_isInvalidPreview=false;
			}
			catch(Exception ex) {
				ex.DoNothing();
				_isInvalidPreview=true;
			}
		}

		/// <summary>Some toolbar items use wiki markup instead of HTML markup. Quickly set wiki markup buttons to enabled/disabled from boolean.</summary>
		private void SetWikiToolBarItems(bool isWikiMarkupEnabled) {
			toolBarTop.Buttons["Table"].Enabled=isWikiMarkupEnabled;
			toolBarTop.Buttons["Image"].Enabled=isWikiMarkupEnabled;
			toolBarBottom.Buttons["Color"].Enabled=isWikiMarkupEnabled;
			toolBarBottom.Buttons["Font"].Enabled=isWikiMarkupEnabled;
			toolBarTop.Refresh();
			toolBarBottom.Refresh();
		}

		private void butOk_Click(object sender,EventArgs e) {
			IsRaw=checkIsRaw.Checked;
			if(!IsRaw) {//do not validate for Raw emails. User is responsible for all validation themselves. 
				if(!MarkupL.ValidateMarkup(textContentEmail,true,true,true)) {
					_isInvalidPreview=true;				
					return;
				}
				if(_isInvalidPreview) {
					MsgBox.Show(this,"This page is in an invalid state and cannot be saved.");
					return;
				}
			}
			if(DoCheckForDisclaimer && PrefC.GetBool(PrefName.EmailDisclaimerIsOn) && !textContentEmail.Text.ToLower().Contains("[emaildisclaimer]")) {
				MsgBox.Show(this,"Email must contain the \"[EmailDisclaimer]\" tag.");
				return;
			}
			HtmlText=webBrowserEmail.DocumentText;
			MarkupText=textContentEmail.Text;
			DialogResult=DialogResult.OK;
			Close();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			_isInvalidPreview=false;//we don't care if it's invalid if they are cancelling. 
			Close();
		}

		private void FormEmailEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(_isInvalidPreview) {
				e.Cancel=true;//don't close the form if there are errors (prevents OK click and the top right X)
			}
		}
	}
}