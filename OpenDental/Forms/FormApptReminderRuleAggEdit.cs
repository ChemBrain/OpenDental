using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormApptReminderRuleAggEdit:ODForm {
		public ApptReminderRule ApptReminderRuleCur;
		///<summary>The plain text of the shared portion of the email body template.</summary>
		private string _emailPlainTextShared;

		public FormApptReminderRuleAggEdit(ApptReminderRule apptReminderCur) {
			InitializeComponent();
			Lan.F(this);
			//This needs to remain a shallow copy because FormEServicesECR is expecting shallow copy changes only. Making a new instance would break that.
			ApptReminderRuleCur=apptReminderCur;
		}

		///<summary>These tags are not allowed in confirmation auto reply.</summary>
		private List<string> ListTagsExludedFromAutoReply {
			get {
				return new List<string>() { "[Appts]","[ConfirmCode]","[ConfirmURL]" };
			}
		}

		private void FormApptReminderRuleEdit_Load(object sender,EventArgs e) {
			textSMSAggShared.Text=PIn.String(ApptReminderRuleCur.TemplateSMSAggShared);
			textSMSAggPerAppt.Text=PIn.String(ApptReminderRuleCur.TemplateSMSAggPerAppt);
			textEmailSubjAggShared.Text=PIn.String(ApptReminderRuleCur.TemplateEmailSubjAggShared);
			_emailPlainTextShared=PIn.String(ApptReminderRuleCur.TemplateEmailAggShared);
			RefreshEmail();
			textEmailAggPerAppt.Text=PIn.String(ApptReminderRuleCur.TemplateEmailAggPerAppt);
			labelTags.Text=GetTagsAvailable();
			textSingleAutoReply.Text=PIn.String(ApptReminderRuleCur.TemplateAutoReply);
			textAggregateAutoReply.Text=PIn.String(ApptReminderRuleCur.TemplateAutoReplyAgg);
			if(ApptReminderRuleCur.TypeCur==ApptReminderType.PatientPortalInvite) {
				textSMSAggShared.Enabled=false;	
				textSMSAggPerAppt.Enabled=false;
				labelEmailAggPerAppt.Text+="  "+Lans.g(this,"Replaces the [Credentials] tag.");
			}
			if(ApptReminderRuleCur.TypeCur!=ApptReminderType.ConfirmationFutureDay) {
				tabTemplates.TabPages.Remove(tabAutoReplyTemplate);
			}
		}

		private string GetTagsAvailable() {
			List<string> listTagsAvailable=ApptReminderRules.GetAvailableAggTags(ApptReminderRuleCur.TypeCur);
			if(tabTemplates.SelectedTab==tabAutoReplyTemplate) {
				listTagsAvailable.RemoveAll(x => x.In(ListTagsExludedFromAutoReply));
			}
			return Lan.g(this,"Use the following replacement tags to customize messages: ")
				 +string.Join(", ",listTagsAvailable);
		}

		private void RefreshEmail() {
			try {
				string text=MarkupEdit.TranslateToXhtml(_emailPlainTextShared,isPreviewOnly:true,hasWikiPageTitles:false,isEmail:true);
				browserEmailBody.DocumentText=text;
			}
			catch(Exception ex) {
				ex.DoNothing();
			}
		}
		
		private void butEditEmail_Click(object sender,EventArgs e) {
			FormEmailEdit formEE=new FormEmailEdit();
			formEE.IsRawAllowed=false;
			formEE.MarkupText=_emailPlainTextShared;
			formEE.ShowDialog();
			if(formEE.DialogResult!=DialogResult.OK) {
				return;
			}
			_emailPlainTextShared=formEE.MarkupText;
			RefreshEmail();
		}

		private void tabTemplates_SelectedIndexChanged(object sender,EventArgs e) {
			labelTags.Text=GetTagsAvailable();
		}

		private bool ValidateRule() {
			List<string> errors = new List<string>();
			if(ApptReminderRuleCur.TypeCur!=ApptReminderType.PatientPortalInvite) {
				if(string.IsNullOrWhiteSpace(textSMSAggShared.Text)) {
					errors.Add(Lan.g(this,"Text message cannot be blank."));
				}
				if(!textSMSAggShared.Text.ToLower().Contains("[appts]")) {
					errors.Add(Lan.g(this,"Text message must contain the \"[Appts]\" tag."));
				}
				if(!_emailPlainTextShared.ToLower().Contains("[appts]")) {
					errors.Add(Lan.g(this,"Email message must contain the \"[Appts]\" tag."));
				}
			}
			if(string.IsNullOrWhiteSpace(textEmailSubjAggShared.Text)) {
				errors.Add(Lan.g(this,"Email subject cannot be blank."));
			}
			if(string.IsNullOrWhiteSpace(_emailPlainTextShared)) {
				errors.Add(Lan.g(this,"Email message cannot be blank."));
			}	
			if(ApptReminderRuleCur.TypeCur==ApptReminderType.ConfirmationFutureDay) {
				if(_emailPlainTextShared.ToLower().Contains("[confirmcode]")) {
					errors.Add(Lan.g(this,"Confirmation emails should not contain the \"[ConfirmCode]\" tag."));
				}
				if(!_emailPlainTextShared.ToLower().Contains("[confirmurl]")) {
					errors.Add(Lan.g(this,"Confirmation emails must contain the \"[ConfirmURL]\" tag."));
				}
				if(string.IsNullOrWhiteSpace(textSingleAutoReply.Text)) {
					errors.Add(Lan.g(this,"Single auto reply text cannot be blank."));
				}
				if(string.IsNullOrWhiteSpace(textAggregateAutoReply.Text)) {
					errors.Add(Lan.g(this,"Aggregate auto reply text cannot be blank."));
				}
				List<string> listInvalidTags=ListTagsExludedFromAutoReply.FindAll(x => textSingleAutoReply.Text.ToLower().Contains(x.ToLower()));
				if(listInvalidTags.Count>0) {
					errors.Add(Lan.g(this,"Single auto reply text contains invalid tags:\r\n"+$"  {string.Join(", ",listInvalidTags)}"));
				}
				listInvalidTags=ListTagsExludedFromAutoReply.FindAll(x => textAggregateAutoReply.Text.ToLower().Contains(x.ToLower()));
				if(listInvalidTags.Count>0) {
					errors.Add(Lan.g(this,"Aggregate auto reply text contains invalid tags:\r\n"+$"  {string.Join(", ",listInvalidTags)}"));
				}
			}
			if(PrefC.GetBool(PrefName.EmailDisclaimerIsOn) && !_emailPlainTextShared.ToLower().Contains("[emaildisclaimer]")) {
				errors.Add(Lan.g(this,"Email must contain the \"[EmailDisclaimer]\" tag."));
			}
			if(errors.Count>0) {
				MessageBox.Show(Lan.g(this,"You must fix the following errors before continuing.")+"\r\n\r\n-"+string.Join("\r\n-",errors));
			}
			return errors.Count==0;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!ValidateRule()) {
				return;
			}
			ApptReminderRuleCur.TemplateSMSAggShared=textSMSAggShared.Text.Replace("[ConfirmURL].","[ConfirmURL] .");//Clicking a link with a period will not get recognized. 
			ApptReminderRuleCur.TemplateSMSAggPerAppt=textSMSAggPerAppt.Text;
			ApptReminderRuleCur.TemplateEmailSubjAggShared=textEmailSubjAggShared.Text;
			ApptReminderRuleCur.TemplateEmailAggShared=_emailPlainTextShared;
			ApptReminderRuleCur.TemplateEmailAggPerAppt=textEmailAggPerAppt.Text;
			ApptReminderRuleCur.TemplateAutoReply=textSingleAutoReply.Text;
			ApptReminderRuleCur.TemplateAutoReplyAgg=textAggregateAutoReply.Text;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}