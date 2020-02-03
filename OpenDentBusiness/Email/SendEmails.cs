using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using CDO;
using CodeBase;
using MimeKit;
using MailKit.Security;

namespace OpenDentBusiness.Email {

	public static class SendEmail {

		///<summary>Throws exceptions. Attempts to physically send the message over the network wire. This is used from wherever email needs to be 
		///sent throughout the program. If a message must be encrypted, then encrypt it before calling this function. nameValueCollectionHeaders can 
		///be null.</summary>
		public static void WireEmailUnsecure(BasicEmailAddress address,BasicEmailMessage emailMessage,NameValueCollection nameValueCollectionHeaders,
			params AlternateView[] arrayAlternateViews) {
			if(!address.AccessToken.IsNullOrEmpty() && address.SMTPserver=="smtp.gmail.com") {//For now we only support OAuth for Gmail but this may change in the future.
				MailMessage message=BasicEmailMessageToMailMessage(emailMessage,nameValueCollectionHeaders,arrayAlternateViews);
				SendEmailOAuth(address,message);
			}
			else {
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
						for(int i = 0;i<arrayHeaderKeys.Length;i++) {//Needed for Direct Acks to work.
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
						message=BasicEmailMessageToMailMessage(emailMessage,nameValueCollectionHeaders,arrayAlternateViews);
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

		public static MailMessage BasicEmailMessageToMailMessage(BasicEmailMessage basicMessage,NameValueCollection nameValueCollectionHeaders,
			params AlternateView[] arrayAlternateViews) {
			MailMessage message=new MailMessage();
			message.From=new MailAddress(basicMessage.FromAddress.Trim());
			if(!string.IsNullOrWhiteSpace(basicMessage.ToAddress)) {
				message.To.Add(basicMessage.ToAddress.Trim());
			}
			if(!string.IsNullOrWhiteSpace(basicMessage.CcAddress)) {
				message.CC.Add(basicMessage.CcAddress.Trim());
			}
			if(!string.IsNullOrWhiteSpace(basicMessage.BccAddress)) {
				message.Bcc.Add(basicMessage.BccAddress.Trim());
			}
			message.Subject=basicMessage.Subject;
			if(basicMessage.IsHtml) {
				//create alternate view in case browser cannot render html
				message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(basicMessage.HtmlBody
					,new System.Net.Mime.ContentType("text/html")));
				message.IsBodyHtml=true;
				message.Body=basicMessage.HtmlBody;
				if(!basicMessage.ListHtmlImages.IsNullOrEmpty()) {
					foreach(string imagePath in basicMessage.ListHtmlImages) {
						Attachment imgAttach=new Attachment(imagePath);
						imgAttach.ContentId=HttpUtility.UrlEncode(Path.GetFileName(imagePath));
						imgAttach.ContentDisposition.Inline=true;
						message.Attachments.Add(imgAttach);
					}
				}
			}
			else {
				message.IsBodyHtml=false;
				message.Body=basicMessage.BodyText;
			}
			if(nameValueCollectionHeaders!=null) {
				message.Headers.Add(nameValueCollectionHeaders);//Needed for Direct Acks to work.
			}
			for(int i=0;i<arrayAlternateViews.Length;i++) {//Needed for Direct messages to be interpreted encrypted on the receiver's end.
				message.AlternateViews.Add(arrayAlternateViews[i]);
			}
			if(!basicMessage.ListAttachments.IsNullOrEmpty()) {
				foreach(Tuple<string,string> attachment in basicMessage.ListAttachments) {
					//@"C:\OpenDentalData\EmailAttachments\1");
					Attachment attach=new Attachment(attachment.Item1);
					//"canadian.gif";
					attach.Name=attachment.Item2;
					message.Attachments.Add(attach);
				}
			}
			return message;
		}

		///<summary>Throws exceptions if failing to send emails or authenticate with Google.</summary>
		private static void SendEmailOAuth(BasicEmailAddress address,MailMessage message) {
			MimeMessage mimeMessage=MimeMessage.CreateFromMailMessage(message);
			using MailKit.Net.Smtp.SmtpClient clientMK=new MailKit.Net.Smtp.SmtpClient();
			try {
				clientMK.Connect(address.SMTPserver,address.ServerPort,SecureSocketOptions.Auto);
				// Note: only needed if the SMTP server requires authentication
				clientMK.Authenticate(new SaslMechanismOAuth2(address.EmailUsername,address.AccessToken));
				clientMK.Send(mimeMessage);
			}
			catch(AuthenticationException ae) {//Don't try to refresh the token here, we don't have a db context if we're in the OpenDentalEmail project.
				ae.DoNothing();
				throw new Exception("Unable to authenticate with Google: "+ae.Message);//This will bubble up to the UI level and be caught in a copypaste box.
			}
			catch(Exception e) {//Need this catch to dispose our client before bubbling up to the UI.
				throw new Exception($"Error sending email with OAuth authorization: {e.Message}");//This will bubble up to the UI level and be caught in a copypaste box.
			}
			finally {
				clientMK.Disconnect(true);
				clientMK.Dispose();
			}
		}

	}

}
