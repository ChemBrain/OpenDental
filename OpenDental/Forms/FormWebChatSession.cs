using System;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OpenDental {
	public partial class FormWebChatSession:ODForm {

		private WebChatSession _webChatSession=null;
		private Action _onCloseAction=null;
		private string _sessionNote="";

		///<summary>This form is only displayed when accessed from the triage list at ODHQ.</summary>
		public FormWebChatSession(WebChatSession webChatSession,Action onCloseAction) {
			InitializeComponent();
			Lan.F(this);
			_webChatSession=webChatSession;
			_onCloseAction=onCloseAction;
		}

		private void FormWebChatSession_Load(object sender,EventArgs e) {
			if(_webChatSession.DateTend.Year > 1880) {//Session has ended?
				DisableForm(textOwner,textWebChatSessionNum,textName,textEmail,textPractice,textPhone,webChatThread,butClose,butPatientSelect,textCustomer
					,tabControlMain);
				//tabControlMain needs to be enabled to view notes, but these options still need to be disabled. 
				butSend.Enabled=false;
				textChatMessage.Enabled=false;
			}
			_sessionNote=_webChatSession.Note;
			FillSession();
			textNote.Text=_sessionNote;
			FillMessageThread();
		}

		public override void OnProcessSignals(List<Signalod> listSignals) {
			if(listSignals.Exists(x => x.IType==InvalidType.WebChatSessions && x.FKey==_webChatSession.WebChatSessionNum)) {
				_sessionNote=textNote.Text;
				_webChatSession=WebChatSessions.GetOne(_webChatSession.WebChatSessionNum);//Refresh the session in case the owner changed.
				FillSession();
				FillMessageThread();
			}
		}

		private void FillSession() {
			textOwner.Text=_webChatSession.TechName;
			textName.Text=_webChatSession.UserName;
			textPractice.Text=_webChatSession.PracticeName;
			textWebChatSessionNum.Text=POut.Long(_webChatSession.WebChatSessionNum);
			textEmail.Text=_webChatSession.EmailAddress;
			textPhone.Text=_webChatSession.PhoneNumber;
			checkIsCustomer.Checked=_webChatSession.IsCustomer;
			textCustomer.Text=(_webChatSession.PatNum==0)?"":Patients.GetNameLF(_webChatSession.PatNum);
		}

		private void FillMessageThread() {
			List<WebChatMessage> listWebChatMessages=WebChatMessages.GetAllForSessions(_webChatSession.WebChatSessionNum);
			List <SmsThreadMessage> listThreadMessages=new List<SmsThreadMessage>();
			foreach(WebChatMessage webChatMessage in listWebChatMessages) {
				string strMsg=webChatMessage.MessageText;
				if(webChatMessage.MessageType==WebChatMessageType.EndSession) {
					strMsg=MarkupEdit.ConvertToPlainText(strMsg);
				}
				SmsThreadMessage msg=new SmsThreadMessage(webChatMessage.DateT,
					strMsg,
					(webChatMessage.MessageType==WebChatMessageType.Customer),
					false,
					false,
					webChatMessage.UserName
				);
				listThreadMessages.Add(msg);
			}
			webChatThread.ListSmsThreadMessages=listThreadMessages;
		}

		private void textChatMessage_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.Enter && !e.Shift) {
				e.Handled=true;
				SendMessage();
			}
		}

		private void SendMessage() {
			WebChatSessions.SendTechMessage(_webChatSession.WebChatSessionNum,textChatMessage.Text);
			textChatMessage.Text="";
			FillMessageThread();
		}

		private void butSend_Click(object sender,EventArgs e) {
			SendMessage();
		}

		private void butPatientSelect_Click(object sender,EventArgs e) {
			FormPatientSelect formPS=new FormPatientSelect();
			if(formPS.ShowDialog()!=DialogResult.OK) {
				return;
			}
			Patient pat=Patients.GetPat(formPS.SelectedPatNum);
			textCustomer.Text=pat.GetNameLF();
			WebChatSession oldWebChatSession=_webChatSession.Clone();
			_webChatSession.PatNum=pat.PatNum;
			WebChatSessions.Update(_webChatSession,oldWebChatSession);//update here so we can associate pats after chat has ended, or before ownership.
		}

		private void butEndSession_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Permanently end the session for everyone?")) {
				return;
			}
			WebChatSessions.EndSession(_webChatSession.WebChatSessionNum);
			DialogResult=DialogResult.OK;
			Close();
		}

		private void butTakeOwnership_Click(object sender,EventArgs e) {
			TakeOwnership();
		}

		private void TakeOwnership() {
			//Refresh the session in case the owner changed in less than the last signal interval.
			_webChatSession=WebChatSessions.GetOne(_webChatSession.WebChatSessionNum);
			bool isOkToTake=true;
			if(_webChatSession.TechName==Security.CurUser.UserName) {
				//Ownership is already claimed.  Do not run a pointless update command.
				isOkToTake=false;
			}
			else if(!String.IsNullOrEmpty(_webChatSession.TechName)) {
				if(MessageBox.Show(Lan.g(this,"This session is owned by another technician.  Take this session from user")+" "+_webChatSession.TechName+"?",
					"",MessageBoxButtons.YesNo)!=DialogResult.Yes)
				{
					isOkToTake=false;
				}
			}
			if(isOkToTake) {
				WebChatSession oldWebChatSession=_webChatSession.Clone();
				_webChatSession.TechName=Security.CurUser.UserName;
				WebChatSessions.Update(_webChatSession,oldWebChatSession);
			}
			textOwner.Text=_webChatSession.TechName;
			if(isOkToTake) {//show after previous text has changed
				MessageBox.Show(Lan.g(this,"Session is now owned by")+" "+_webChatSession.TechName);
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();
		}

		private void FormWebChatSession_FormClosing(object sender,FormClosingEventArgs e) {
			if(Regex.Replace(_webChatSession.Note,@"\s+","")!=Regex.Replace(textNote.Text,@"\s+","")
				&& MsgBox.Show(this,MsgBoxButtons.YesNo,"Notes changed.  Save changes?")) 
			{
				WebChatSession webChatSessionOld=_webChatSession.Clone();
				_webChatSession.Note=textNote.Text;
				WebChatSessions.Update(_webChatSession,webChatSessionOld);
			}
			_onCloseAction();
		}

	}
}