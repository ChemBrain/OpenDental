using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenDental{
///<summary></summary>
	public class FormEmailAddressEdit:ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textSMTPserver;
		private System.Windows.Forms.TextBox textSender;
		private TextBox textUsername;
		private Label label3;
		private TextBox textPassword;
		private Label label4;
		private TextBox textPort;
		private Label label5;
		private Label label6;
		private CheckBox checkSSL;
		private System.ComponentModel.Container components = null;
		private UI.Button butDelete;
		private EmailAddress _emailAddressCur;
		private GroupBox groupOutgoing;
		private GroupBox groupIncoming;
		private TextBox textSMTPserverIncoming;
		private Label label8;
		private Label label10;
		private TextBox textPortIncoming;
		private Label label11;
		private Label label9;
		private UI.Button butRegisterCertificate;
		private Label label13;
		private Label label14;
		private GroupBox groupUserod;
		private UI.Button butPickUserod;
		private Label labelUserod;
		private TextBox textUserod;
		private TextBox textAccessToken;
		private TextBox textRefreshToken;
		private Label labelAccess;
		private Label labelRefresh;
		private GroupBox groupGoogleAuth;
		private UI.Button butClearTokens;
		private Label butAuthGoogle;
		private GroupBox groupAuthentication;
		private Label label7;
		private Label label12;
		private bool _isNew;

		private int _groupAuthLocationXAuthorized=14;
		private int _groupAuthLocationXNotAuthorized=165;

		///<summary></summary>
		public FormEmailAddressEdit(EmailAddress emailAddress,bool isOpenedFromEmailSetup=false) {
			InitializeComponent();
			Lan.F(this);
			_emailAddressCur=emailAddress;
			List<long> listDefaultAddressNums=new List<long>() {
				PrefC.GetLong(PrefName.EmailNotifyAddressNum),
				PrefC.GetLong(PrefName.EmailDefaultAddressNum)
			};
			if(isOpenedFromEmailSetup && Security.IsAuthorized(Permissions.SecurityAdmin,true) 
				&& (_isNew || !_emailAddressCur.EmailAddressNum.In(listDefaultAddressNums)))
			{
				butPickUserod.Visible=true;
			}
		}

		public FormEmailAddressEdit(long userNum,bool isOpenedFromEmailSetup=false) : this(new EmailAddress() { UserNum=userNum },isOpenedFromEmailSetup) {
			_isNew=true;
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailAddressEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textSMTPserver = new System.Windows.Forms.TextBox();
			this.textSender = new System.Windows.Forms.TextBox();
			this.textUsername = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textPort = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.checkSSL = new System.Windows.Forms.CheckBox();
			this.butDelete = new OpenDental.UI.Button();
			this.groupOutgoing = new System.Windows.Forms.GroupBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.groupIncoming = new System.Windows.Forms.GroupBox();
			this.label14 = new System.Windows.Forms.Label();
			this.textSMTPserverIncoming = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.textPortIncoming = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.butRegisterCertificate = new OpenDental.UI.Button();
			this.groupUserod = new System.Windows.Forms.GroupBox();
			this.butPickUserod = new OpenDental.UI.Button();
			this.labelUserod = new System.Windows.Forms.Label();
			this.textUserod = new System.Windows.Forms.TextBox();
			this.textAccessToken = new System.Windows.Forms.TextBox();
			this.textRefreshToken = new System.Windows.Forms.TextBox();
			this.labelAccess = new System.Windows.Forms.Label();
			this.labelRefresh = new System.Windows.Forms.Label();
			this.groupGoogleAuth = new System.Windows.Forms.GroupBox();
			this.butClearTokens = new OpenDental.UI.Button();
			this.butAuthGoogle = new System.Windows.Forms.Label();
			this.groupAuthentication = new System.Windows.Forms.GroupBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.groupOutgoing.SuspendLayout();
			this.groupIncoming.SuspendLayout();
			this.groupUserod.SuspendLayout();
			this.groupGoogleAuth.SuspendLayout();
			this.groupAuthentication.SuspendLayout();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(658, 544);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 7;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(577, 544);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 6;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(87, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(173, 20);
			this.label1.TabIndex = 2;
			this.label1.Text = "Outgoing SMTP Server";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(81, 143);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(177, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "E-mail address of sender";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSMTPserver
			// 
			this.textSMTPserver.Location = new System.Drawing.Point(260, 20);
			this.textSMTPserver.Name = "textSMTPserver";
			this.textSMTPserver.Size = new System.Drawing.Size(187, 20);
			this.textSMTPserver.TabIndex = 1;
			// 
			// textSender
			// 
			this.textSender.Location = new System.Drawing.Point(260, 143);
			this.textSender.Name = "textSender";
			this.textSender.Size = new System.Drawing.Size(187, 20);
			this.textSender.TabIndex = 3;
			// 
			// textUsername
			// 
			this.textUsername.Location = new System.Drawing.Point(273, 12);
			this.textUsername.Name = "textUsername";
			this.textUsername.Size = new System.Drawing.Size(188, 20);
			this.textUsername.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(116, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(151, 20);
			this.label3.TabIndex = 0;
			this.label3.Text = "Username or email address";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(110, 18);
			this.textPassword.Name = "textPassword";
			this.textPassword.PasswordChar = '*';
			this.textPassword.Size = new System.Drawing.Size(188, 20);
			this.textPassword.TabIndex = 2;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(9, 18);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(95, 20);
			this.label4.TabIndex = 0;
			this.label4.Text = "Password";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPort
			// 
			this.textPort.Location = new System.Drawing.Point(260, 115);
			this.textPort.Name = "textPort";
			this.textPort.Size = new System.Drawing.Size(56, 20);
			this.textPort.TabIndex = 2;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(84, 115);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(174, 20);
			this.label5.TabIndex = 22;
			this.label5.Text = "Outgoing Port";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(322, 115);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(251, 20);
			this.label6.TabIndex = 0;
			this.label6.Text = "Usually 587.  Sometimes 25 or 465.";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkSSL
			// 
			this.checkSSL.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSSL.Location = new System.Drawing.Point(165, 35);
			this.checkSSL.Name = "checkSSL";
			this.checkSSL.Size = new System.Drawing.Size(122, 17);
			this.checkSSL.TabIndex = 3;
			this.checkSSL.Text = "Use SSL";
			this.checkSSL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSSL.UseVisualStyleBackColor = true;
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(14, 544);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 24);
			this.butDelete.TabIndex = 8;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// groupOutgoing
			// 
			this.groupOutgoing.Controls.Add(this.label13);
			this.groupOutgoing.Controls.Add(this.label9);
			this.groupOutgoing.Controls.Add(this.textSMTPserver);
			this.groupOutgoing.Controls.Add(this.label1);
			this.groupOutgoing.Controls.Add(this.label2);
			this.groupOutgoing.Controls.Add(this.label6);
			this.groupOutgoing.Controls.Add(this.textSender);
			this.groupOutgoing.Controls.Add(this.textPort);
			this.groupOutgoing.Controls.Add(this.label5);
			this.groupOutgoing.Location = new System.Drawing.Point(14, 188);
			this.groupOutgoing.Name = "groupOutgoing";
			this.groupOutgoing.Size = new System.Drawing.Size(719, 180);
			this.groupOutgoing.TabIndex = 4;
			this.groupOutgoing.TabStop = false;
			this.groupOutgoing.Text = "Outgoing Email Settings";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(262, 43);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(198, 69);
			this.label13.TabIndex = 0;
			this.label13.Text = "smtp.comcast.net\r\nmailhost.mycompany.com \r\nmail.mycompany.com\r\nsmtp.gmail.com\r\nor" +
    " similar...";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(451, 142);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(159, 20);
			this.label9.TabIndex = 0;
			this.label9.Text = "(not used in encrypted email)";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupIncoming
			// 
			this.groupIncoming.Controls.Add(this.label14);
			this.groupIncoming.Controls.Add(this.textSMTPserverIncoming);
			this.groupIncoming.Controls.Add(this.label8);
			this.groupIncoming.Controls.Add(this.label10);
			this.groupIncoming.Controls.Add(this.textPortIncoming);
			this.groupIncoming.Controls.Add(this.label11);
			this.groupIncoming.Location = new System.Drawing.Point(14, 374);
			this.groupIncoming.Name = "groupIncoming";
			this.groupIncoming.Size = new System.Drawing.Size(719, 116);
			this.groupIncoming.TabIndex = 5;
			this.groupIncoming.TabStop = false;
			this.groupIncoming.Text = "Incoming Email Settings";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(262, 38);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(198, 43);
			this.label14.TabIndex = 0;
			this.label14.Text = "pop.secureserver.net\r\npop.gmail.com\r\nor similar...";
			// 
			// textSMTPserverIncoming
			// 
			this.textSMTPserverIncoming.Location = new System.Drawing.Point(260, 15);
			this.textSMTPserverIncoming.Name = "textSMTPserverIncoming";
			this.textSMTPserverIncoming.Size = new System.Drawing.Size(187, 20);
			this.textSMTPserverIncoming.TabIndex = 1;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(87, 15);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(173, 20);
			this.label8.TabIndex = 0;
			this.label8.Text = "Incoming POP3 Server";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(322, 84);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(251, 20);
			this.label10.TabIndex = 0;
			this.label10.Text = "Usually 110.  Sometimes 995.";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textPortIncoming
			// 
			this.textPortIncoming.Location = new System.Drawing.Point(260, 84);
			this.textPortIncoming.Name = "textPortIncoming";
			this.textPortIncoming.Size = new System.Drawing.Size(56, 20);
			this.textPortIncoming.TabIndex = 2;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(84, 84);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(174, 20);
			this.label11.TabIndex = 0;
			this.label11.Text = "Incoming Port";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butRegisterCertificate
			// 
			this.butRegisterCertificate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butRegisterCertificate.Location = new System.Drawing.Point(346, 544);
			this.butRegisterCertificate.Name = "butRegisterCertificate";
			this.butRegisterCertificate.Size = new System.Drawing.Size(122, 24);
			this.butRegisterCertificate.TabIndex = 9;
			this.butRegisterCertificate.Text = "Certificate";
			this.butRegisterCertificate.Click += new System.EventHandler(this.butRegisterCertificate_Click);
			// 
			// groupUserod
			// 
			this.groupUserod.Controls.Add(this.butPickUserod);
			this.groupUserod.Controls.Add(this.labelUserod);
			this.groupUserod.Controls.Add(this.textUserod);
			this.groupUserod.Location = new System.Drawing.Point(15, 496);
			this.groupUserod.Name = "groupUserod";
			this.groupUserod.Size = new System.Drawing.Size(718, 42);
			this.groupUserod.TabIndex = 10;
			this.groupUserod.TabStop = false;
			this.groupUserod.Text = "User";
			// 
			// butPickUserod
			// 
			this.butPickUserod.Location = new System.Drawing.Point(483, 15);
			this.butPickUserod.Name = "butPickUserod";
			this.butPickUserod.Size = new System.Drawing.Size(26, 23);
			this.butPickUserod.TabIndex = 4;
			this.butPickUserod.Text = "...";
			this.butPickUserod.UseVisualStyleBackColor = true;
			this.butPickUserod.Visible = false;
			this.butPickUserod.Click += new System.EventHandler(this.butPickUserod_Click);
			// 
			// labelUserod
			// 
			this.labelUserod.Location = new System.Drawing.Point(85, 16);
			this.labelUserod.Name = "labelUserod";
			this.labelUserod.Size = new System.Drawing.Size(173, 20);
			this.labelUserod.TabIndex = 3;
			this.labelUserod.Text = "User";
			this.labelUserod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textUserod
			// 
			this.textUserod.Location = new System.Drawing.Point(258, 16);
			this.textUserod.Name = "textUserod";
			this.textUserod.ReadOnly = true;
			this.textUserod.Size = new System.Drawing.Size(218, 20);
			this.textUserod.TabIndex = 0;
			// 
			// textAccessToken
			// 
			this.textAccessToken.Location = new System.Drawing.Point(131, 24);
			this.textAccessToken.Name = "textAccessToken";
			this.textAccessToken.ReadOnly = true;
			this.textAccessToken.Size = new System.Drawing.Size(115, 20);
			this.textAccessToken.TabIndex = 11;
			// 
			// textRefreshToken
			// 
			this.textRefreshToken.Location = new System.Drawing.Point(131, 50);
			this.textRefreshToken.Name = "textRefreshToken";
			this.textRefreshToken.ReadOnly = true;
			this.textRefreshToken.Size = new System.Drawing.Size(115, 20);
			this.textRefreshToken.TabIndex = 12;
			// 
			// labelAccess
			// 
			this.labelAccess.Location = new System.Drawing.Point(13, 21);
			this.labelAccess.Name = "labelAccess";
			this.labelAccess.Size = new System.Drawing.Size(116, 20);
			this.labelAccess.TabIndex = 14;
			this.labelAccess.Text = "Access Token";
			this.labelAccess.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelRefresh
			// 
			this.labelRefresh.Location = new System.Drawing.Point(10, 47);
			this.labelRefresh.Name = "labelRefresh";
			this.labelRefresh.Size = new System.Drawing.Size(115, 20);
			this.labelRefresh.TabIndex = 15;
			this.labelRefresh.Text = "Refresh Token";
			this.labelRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupGoogleAuth
			// 
			this.groupGoogleAuth.Controls.Add(this.textAccessToken);
			this.groupGoogleAuth.Controls.Add(this.butClearTokens);
			this.groupGoogleAuth.Controls.Add(this.labelAccess);
			this.groupGoogleAuth.Controls.Add(this.textRefreshToken);
			this.groupGoogleAuth.Controls.Add(this.labelRefresh);
			this.groupGoogleAuth.Location = new System.Drawing.Point(381, 54);
			this.groupGoogleAuth.Name = "groupGoogleAuth";
			this.groupGoogleAuth.Size = new System.Drawing.Size(352, 128);
			this.groupGoogleAuth.TabIndex = 16;
			this.groupGoogleAuth.TabStop = false;
			this.groupGoogleAuth.Text = "Gmail Authorization";
			// 
			// butClearTokens
			// 
			this.butClearTokens.Location = new System.Drawing.Point(131, 76);
			this.butClearTokens.Name = "butClearTokens";
			this.butClearTokens.Size = new System.Drawing.Size(115, 23);
			this.butClearTokens.TabIndex = 16;
			this.butClearTokens.Text = "Clear Tokens";
			this.butClearTokens.UseVisualStyleBackColor = true;
			this.butClearTokens.Click += new System.EventHandler(this.butClearTokens_Click);
			// 
			// butAuthGoogle
			// 
			this.butAuthGoogle.Image = global::OpenDental.Properties.Resources.google_signin_normal;
			this.butAuthGoogle.Location = new System.Drawing.Point(107, 76);
			this.butAuthGoogle.Name = "butAuthGoogle";
			this.butAuthGoogle.Size = new System.Drawing.Size(191, 46);
			this.butAuthGoogle.TabIndex = 17;
			this.butAuthGoogle.Click += new System.EventHandler(this.butAuthGoogle_Click);
			this.butAuthGoogle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.butAuthGoogle_MouseDown);
			this.butAuthGoogle.MouseEnter += new System.EventHandler(this.butAuthGoogle_MouseEnter);
			this.butAuthGoogle.MouseLeave += new System.EventHandler(this.butAuthGoogle_MouseLeave);
			this.butAuthGoogle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.butAuthGoogle_MouseUp);
			// 
			// groupAuthentication
			// 
			this.groupAuthentication.Controls.Add(this.label12);
			this.groupAuthentication.Controls.Add(this.textPassword);
			this.groupAuthentication.Controls.Add(this.label4);
			this.groupAuthentication.Controls.Add(this.butAuthGoogle);
			this.groupAuthentication.Controls.Add(this.label7);
			this.groupAuthentication.Location = new System.Drawing.Point(14, 54);
			this.groupAuthentication.Name = "groupAuthentication";
			this.groupAuthentication.Size = new System.Drawing.Size(345, 128);
			this.groupAuthentication.TabIndex = 19;
			this.groupAuthentication.TabStop = false;
			this.groupAuthentication.Text = "Email Authentication";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(9, 79);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(95, 43);
			this.label12.TabIndex = 19;
			this.label12.Text = "Required for Gmail addresses";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(110, 41);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(188, 31);
			this.label7.TabIndex = 18;
			this.label7.Text = "or";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FormEmailAddressEdit
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(749, 584);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textUsername);
			this.Controls.Add(this.groupAuthentication);
			this.Controls.Add(this.groupGoogleAuth);
			this.Controls.Add(this.groupUserod);
			this.Controls.Add(this.butRegisterCertificate);
			this.Controls.Add(this.groupIncoming);
			this.Controls.Add(this.groupOutgoing);
			this.Controls.Add(this.checkSSL);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(765, 623);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(765, 623);
			this.Name = "FormEmailAddressEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Email Address";
			this.Load += new System.EventHandler(this.FormEmailAddress_Load);
			this.groupOutgoing.ResumeLayout(false);
			this.groupOutgoing.PerformLayout();
			this.groupIncoming.ResumeLayout(false);
			this.groupIncoming.PerformLayout();
			this.groupUserod.ResumeLayout(false);
			this.groupUserod.PerformLayout();
			this.groupGoogleAuth.ResumeLayout(false);
			this.groupGoogleAuth.PerformLayout();
			this.groupAuthentication.ResumeLayout(false);
			this.groupAuthentication.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormEmailAddress_Load(object sender, System.EventArgs e) {
			if(_emailAddressCur!=null) {
				textSMTPserver.Text=_emailAddressCur.SMTPserver;
				textUsername.Text=_emailAddressCur.EmailUsername;
				if(!String.IsNullOrEmpty(_emailAddressCur.EmailPassword)) { //can happen if creating a new user email.
					textPassword.Text=MiscUtils.Decrypt(_emailAddressCur.EmailPassword);
				}
				textPort.Text=_emailAddressCur.ServerPort.ToString();
				checkSSL.Checked=_emailAddressCur.UseSSL;
				textSender.Text=_emailAddressCur.SenderAddress;
				textSMTPserverIncoming.Text=_emailAddressCur.Pop3ServerIncoming;
				textPortIncoming.Text=_emailAddressCur.ServerPortIncoming.ToString();
				//Both EmailNotifyAddressNum and EmailDefaultAddressNum could be 0 (unset), in which case we still may want to display the user.
				List<long> listDefaultAddressNums=new List<long>() {
					PrefC.GetLong(PrefName.EmailNotifyAddressNum),
					PrefC.GetLong(PrefName.EmailDefaultAddressNum)
				};
				if(_isNew || !_emailAddressCur.EmailAddressNum.In(listDefaultAddressNums)) {
					Userod user=Userods.GetUser(_emailAddressCur.UserNum);
					textUserod.Tag=user;
					textUserod.Text=user?.UserName;
				}
				else {
					groupUserod.Visible=false;
				}
				textAccessToken.Text=_emailAddressCur.AccessToken;
				textRefreshToken.Text=_emailAddressCur.RefreshToken;
			}
			groupGoogleAuth.Visible=!textAccessToken.Text.IsNullOrEmpty();
			if(groupGoogleAuth.Visible) {
				groupAuthentication.Location=new Point(_groupAuthLocationXAuthorized,groupAuthentication.Location.Y);
			}
			else {
				groupAuthentication.Location=new Point(_groupAuthLocationXNotAuthorized,groupAuthentication.Location.Y);
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(_isNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(_emailAddressCur.EmailAddressNum==PrefC.GetLong(PrefName.EmailDefaultAddressNum)) {
				MsgBox.Show(this,"Cannot delete the default email address.");
				return;
			}
			if(_emailAddressCur.EmailAddressNum==PrefC.GetLong(PrefName.EmailNotifyAddressNum)) {
				MsgBox.Show(this,"Cannot delete the notify email address.");
				return;
			}
			Clinic clinic=Clinics.GetFirstOrDefault(x => x.EmailAddressNum==_emailAddressCur.EmailAddressNum);
			if(clinic!=null) {
				MessageBox.Show(Lan.g(this,"Cannot delete the email address because it is used by clinic")+" "+clinic.Description);
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete this email address?")) {
				return;
			}
			EmailAddresses.Delete(_emailAddressCur.EmailAddressNum);
			DialogResult=DialogResult.OK;//OK triggers a refresh for the grid.
		}

		private void butRegisterCertificate_Click(object sender,EventArgs e) {
			FormEmailCertRegister form=new FormEmailCertRegister(textUsername.Text);
			form.ShowDialog();
		}

		private void butPickUserod_Click(object sender,EventArgs e) {
			FormUserPick formUserPick=new FormUserPick();
			formUserPick.SuggestedUserNum=((Userod)textUserod.Tag)?.UserNum??0;//Preselect current selection.
			if(formUserPick.ShowDialog()==DialogResult.OK) {
				Userod user=Userods.GetUser(formUserPick.SelectedUserNum);
				if(user.UserNum==(((Userod)textUserod.Tag)?.UserNum??0)) {
					return;//No change.
				}
				EmailAddress emailUserExisting=EmailAddresses.GetForUser(user.UserNum);
				if(emailUserExisting!=null) {
					MsgBox.Show(this,"User email already exists for "+user.UserName);
					return;
				}
				textUserod.Tag=user;
				textUserod.Text=user.UserName;
			}
		}

		private void butClearTokens_Click(object sender,EventArgs e) {
			textAccessToken.Text="";
			textRefreshToken.Text="";
		}

		private void butAuthGoogle_Click(object sender,EventArgs e) {
			try {
				string url=Google.GetGoogleAuthorizationUrl(textUsername.Text);
				Process.Start(url);
				InputBox inputbox=new InputBox("Please enter the authorization code from your browser");
				inputbox.setTitle("Google Account Authorization");
				inputbox.ShowDialog();
				if(inputbox.DialogResult!=DialogResult.OK) {
					return;
				}
				if(string.IsNullOrWhiteSpace(inputbox.textResult.Text)) {
					throw new ODException(Lan.g(this,"There was no authorization code entered."));
				}
				string authCode=inputbox.textResult.Text;
				GoogleToken tokens=Google.MakeAccessTokenRequest(authCode);
				if(tokens.ErrorMessage!="") {
					throw new Exception(tokens.ErrorMessage);
				}
				textAccessToken.Text=tokens.AccessToken;
				textRefreshToken.Text=tokens.RefreshToken;
				groupAuthentication.Location=new Point(_groupAuthLocationXAuthorized,groupAuthentication.Location.Y);
				groupGoogleAuth.Visible=true;
			}
			catch(ODException ae) {
				MsgBox.Show(ae.Message);
			}
			catch(Exception ex) {
				MsgBox.Show("Error: "+ex.Message);
			}
		}

		private void butAuthGoogle_MouseEnter(object sender,EventArgs e) {
			butAuthGoogle.BackgroundImage=Properties.Resources.google_signin_focus;
		}

		private void butAuthGoogle_MouseLeave(object sender,EventArgs e) {
			butAuthGoogle.BackgroundImage=Properties.Resources.google_signin_normal;
		}

		private void butAuthGoogle_MouseDown(object sender,MouseEventArgs e) {
			butAuthGoogle.BackgroundImage=Properties.Resources.google_signin_pressed;
		}

		private void butAuthGoogle_MouseUp(object sender,MouseEventArgs e) {
			butAuthGoogle.BackgroundImage=Properties.Resources.google_signin_normal;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			try {
				PIn.Int(textPort.Text);
			}
			catch {
				MsgBox.Show(this,"Invalid outgoing port number.");
				return;
			}
			try {
				PIn.Int(textPortIncoming.Text);
			}
			catch {
				MsgBox.Show(this,"Invalid incoming port number.");
				return;
			}
			if(string.IsNullOrWhiteSpace(textUsername.Text)) {
				MsgBox.Show(this,"Please enter a valid email address.");
				return;
			}
			//Only checks against non-user email addresses.
			if(EmailAddresses.AddressExists(textUsername.Text,_emailAddressCur.EmailAddressNum)) {
				MsgBox.Show(this,"This email address already exists.");
				return;
			}
			if(!textPassword.Text.IsNullOrEmpty() && !textAccessToken.Text.IsNullOrEmpty()) {
				MsgBox.Show(this,"There is an email password and access token entered.  Please clear one and try again.");
				return;
			}
			_emailAddressCur.AccessToken=textAccessToken.Text;
			_emailAddressCur.RefreshToken=textRefreshToken.Text;
			if(!_isNew && (!string.IsNullOrWhiteSpace(_emailAddressCur.AccessToken) || !string.IsNullOrWhiteSpace(_emailAddressCur.RefreshToken))//If has a token
				&&( _emailAddressCur.SMTPserver!=PIn.String(textSMTPserver.Text) || _emailAddressCur.Pop3ServerIncoming!=PIn.String(textSMTPserverIncoming.Text)))//And changed a server
			{
				if(!MsgBox.Show(MsgBoxButtons.OKCancel,"There is an access token associated to this email address.  "
					+"Changing servers will wipe out the access token and may require reauthentication.  Continue?"))
				{
					return;
				}
				_emailAddressCur.AccessToken="";
				_emailAddressCur.RefreshToken="";
				//TODO: If this is a google token, we may want to tell Google we no longer require access to their account.
				//This will limit our total number of active users
			}
			_emailAddressCur.SMTPserver=PIn.String(textSMTPserver.Text);
			_emailAddressCur.EmailUsername=PIn.String(textUsername.Text);
			_emailAddressCur.EmailPassword=PIn.String(MiscUtils.Encrypt(textPassword.Text));
			_emailAddressCur.ServerPort=PIn.Int(textPort.Text);
			_emailAddressCur.UseSSL=checkSSL.Checked;
			_emailAddressCur.SenderAddress=PIn.String(textSender.Text);
			_emailAddressCur.Pop3ServerIncoming=PIn.String(textSMTPserverIncoming.Text);
			_emailAddressCur.ServerPortIncoming=PIn.Int(textPortIncoming.Text);
			_emailAddressCur.UserNum=((Userod)(textUserod.Tag))?.UserNum??0;
			if(_isNew) {
				EmailAddresses.Insert(_emailAddressCur);
			}
			else {
				EmailAddresses.Update(_emailAddressCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
