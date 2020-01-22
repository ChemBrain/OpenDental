using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using CDO;
using CodeBase;

namespace OpenDentBusiness.Email {

	public static class SendEmail {

		///<summary>Throws exceptions. Attempts to physically send the message over the network wire. This is used from wherever email needs to be 
		///sent throughout the program. If a message must be encrypted, then encrypt it before calling this function. nameValueCollectionHeaders can 
		///be null.</summary>
		public static void WireEmailUnsecure(BasicEmailAddress address,BasicEmailMessage emailMessage,NameValueCollection nameValueCollectionHeaders,
			params AlternateView[] arrayAlternateViews) 
		{
			bool isImplicitSsl=(address.ServerPort==465);
			if(isImplicitSsl) {//Microsoft CDO supports implicit SSL, System.Net.Mail.SmtpClient only supports explicit SSL.
				var cdo=new Message();
				var cfg=cdo.Configuration;
				cfg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"].Value=address.SMTPserver;
				cfg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"].Value=address.ServerPort;
				cfg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"].Value="2";//sendusing: 1=pickup, 2=port, 3=using microsoft exchange
				cfg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"].Value="1";//0=anonymous,1=clear text auth,2=context (NTLM)				
				cfg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"].Value=address.EmailUsername.Trim();
				cfg.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"].Value=address.EmailPassword;
				cfg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"].Value=true;//false was also tested and does not work
				if(nameValueCollectionHeaders!=null) {
					//How to add headers which do not have formal fields - https://msdn.microsoft.com/en-us/library/ms526317(v=exchg.10).aspx
					string[] arrayHeaderKeys=nameValueCollectionHeaders.AllKeys;
					for(int i=0;i<arrayHeaderKeys.Length;i++) {//Needed for Direct Acks to work.
						string headerName=arrayHeaderKeys[i];
						string headerValue=nameValueCollectionHeaders[headerName];
						cfg.Fields["urn:schemas:mailheader:"+headerName].Value=headerValue;
					}
				}
				cfg.Fields.Update();
				cdo.From=emailMessage.FromAddress.Trim();
				if(!string.IsNullOrWhiteSpace(emailMessage.ToAddress)) {
					cdo.To=emailMessage.ToAddress.Trim();
				}
				if(!string.IsNullOrWhiteSpace(emailMessage.CcAddress)) {
					cdo.CC=emailMessage.CcAddress.Trim();
				}
				if(!string.IsNullOrWhiteSpace(emailMessage.BccAddress)) {
					cdo.BCC=emailMessage.BccAddress.Trim();
				}
				cdo.Subject=emailMessage.Subject;
				if(emailMessage.IsHtml) {
					cdo.HTMLBody=emailMessage.HtmlBody;
					if(!emailMessage.ListHtmlImages.IsNullOrEmpty()) {
						foreach(string imagePath in emailMessage.ListHtmlImages) {
							var imgAttach=cdo.AddAttachment(imagePath);
							imgAttach.Fields["urn:schemas:mailheader:content-id"].Value=HttpUtility.UrlEncode(Path.GetFileName(imagePath));
							imgAttach.Fields.Update();
						}
					}
				}
				else {
					cdo.TextBody=emailMessage.BodyText;
				}
				if(!emailMessage.ListAttachments.IsNullOrEmpty()) {
					foreach(Tuple<string,string> attachmentPath in emailMessage.ListAttachments) {
						var cdoatt=cdo.AddAttachment(attachmentPath.Item1);
						//Use actual file name for this field.
						cdoatt.Fields["urn:schemas:mailheader:content-id"].Value=Path.GetFileName(attachmentPath.Item1);
						cdoatt.Fields.Update();
					}
				}
				cdo.Send();
			}
			else {//No SSL or explicit SSL on port 587  
				SmtpClient client=null;
				MailMessage message=null;
				try {
					client=new SmtpClient(address.SMTPserver,address.ServerPort);
					//The default credentials are not used by default, according to: 
					//http://msdn2.microsoft.com/en-us/library/system.net.mail.smtpclient.usedefaultcredentials.aspx
					client.Credentials=new NetworkCredential(address.EmailUsername.Trim(),address.EmailPassword);
					client.DeliveryMethod=SmtpDeliveryMethod.Network;
					client.EnableSsl=address.UseSSL;
					client.Timeout=180000;//3 minutes
					message=new MailMessage();
					message.From=new MailAddress(emailMessage.FromAddress.Trim());
					if(!string.IsNullOrWhiteSpace(emailMessage.ToAddress)) {
						message.To.Add(emailMessage.ToAddress.Trim());
					}
					if(!string.IsNullOrWhiteSpace(emailMessage.CcAddress)) {
						message.CC.Add(emailMessage.CcAddress.Trim());
					}
					if(!string.IsNullOrWhiteSpace(emailMessage.BccAddress)) {
						message.Bcc.Add(emailMessage.BccAddress.Trim());
					}
					message.Subject=emailMessage.Subject;
					if(emailMessage.IsHtml) {
						//create alternate view in case browser cannot render html
						message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(emailMessage.HtmlBody
							,new System.Net.Mime.ContentType("text/html")));
						message.IsBodyHtml=true;
						message.Body=emailMessage.HtmlBody;
						if(!emailMessage.ListHtmlImages.IsNullOrEmpty()) {
							foreach(string imagePath in emailMessage.ListHtmlImages) {
								Attachment imgAttach=new Attachment(imagePath);
								imgAttach.ContentId=HttpUtility.UrlEncode(Path.GetFileName(imagePath));
								imgAttach.ContentDisposition.Inline=true;
								message.Attachments.Add(imgAttach);
							}
						}
					}
					else {
						message.IsBodyHtml=false;
						message.Body=emailMessage.BodyText;
					}
					if(nameValueCollectionHeaders!=null) {
						message.Headers.Add(nameValueCollectionHeaders);//Needed for Direct Acks to work.
					}
					for(int i=0;i<arrayAlternateViews.Length;i++) {//Needed for Direct messages to be interpreted encrypted on the receiver's end.
						message.AlternateViews.Add(arrayAlternateViews[i]);
					}
					if(!emailMessage.ListAttachments.IsNullOrEmpty()) {
						foreach(Tuple<string,string> attachment in emailMessage.ListAttachments) {
							//@"C:\OpenDentalData\EmailAttachments\1");
							Attachment attach=new Attachment(attachment.Item1);
							//"canadian.gif";
							attach.Name=attachment.Item2;
							message.Attachments.Add(attach);
						}
					}
					client.Send(message);
				}
				finally {
					//Dispose of the client and messages here. For large customers, sending thousands of emails will start to fail until they restart the
					//app. Freeing memory here can prevent OutOfMemoryExceptions.
					client?.Dispose();
					if(message!=null) {
						if(message.Attachments!=null) {
							message.Attachments.ForEach(x => x.Dispose());
						}
						message.Dispose();
					}
				}
			}
		}

	}

}
