namespace OpenDental {
	partial class FormApptReminderRuleAggEdit {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptReminderRuleAggEdit));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBoxTags = new System.Windows.Forms.GroupBox();
			this.labelTags = new System.Windows.Forms.Label();
			this.tabEmailTemplate = new System.Windows.Forms.TabPage();
			this.groupBoxEmailSubjAggShared = new System.Windows.Forms.GroupBox();
			this.labelEmailSubjAggShared = new System.Windows.Forms.Label();
			this.textEmailSubjAggShared = new OpenDental.ODtextBox();
			this.groupBoxEmailAggShared = new System.Windows.Forms.GroupBox();
			this.butEditEmail = new OpenDental.UI.Button();
			this.browserEmailBody = new System.Windows.Forms.WebBrowser();
			this.labelEmailAggShared = new System.Windows.Forms.Label();
			this.groupBoxEmailAggPerAppt = new System.Windows.Forms.GroupBox();
			this.labelEmailAggPerAppt = new System.Windows.Forms.Label();
			this.textEmailAggPerAppt = new OpenDental.ODtextBox();
			this.tabSMSTemplate = new System.Windows.Forms.TabPage();
			this.groupBoxSMSAggPerAppt = new System.Windows.Forms.GroupBox();
			this.labelSMSAggPerAppt = new System.Windows.Forms.Label();
			this.textSMSAggPerAppt = new OpenDental.ODtextBox();
			this.groupBoxSMSAggShared = new System.Windows.Forms.GroupBox();
			this.labelSMSAggShared = new System.Windows.Forms.Label();
			this.textSMSAggShared = new OpenDental.ODtextBox();
			this.tabTemplates = new System.Windows.Forms.TabControl();
			this.tabAutoReplyTemplate = new System.Windows.Forms.TabPage();
			this.groupAggregateAutoReplyTemplate = new System.Windows.Forms.GroupBox();
			this.labelAggregateAutoReply = new System.Windows.Forms.Label();
			this.textAggregateAutoReply = new OpenDental.ODtextBox();
			this.groupAutoReplySingle = new System.Windows.Forms.GroupBox();
			this.labelSingleAutoReply = new System.Windows.Forms.Label();
			this.textSingleAutoReply = new OpenDental.ODtextBox();
			this.groupBoxTags.SuspendLayout();
			this.tabEmailTemplate.SuspendLayout();
			this.groupBoxEmailSubjAggShared.SuspendLayout();
			this.groupBoxEmailAggShared.SuspendLayout();
			this.groupBoxEmailAggPerAppt.SuspendLayout();
			this.tabSMSTemplate.SuspendLayout();
			this.groupBoxSMSAggPerAppt.SuspendLayout();
			this.groupBoxSMSAggShared.SuspendLayout();
			this.tabTemplates.SuspendLayout();
			this.tabAutoReplyTemplate.SuspendLayout();
			this.groupAggregateAutoReplyTemplate.SuspendLayout();
			this.groupAutoReplySingle.SuspendLayout();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(500, 567);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 6;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(581, 567);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 7;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// groupBoxTags
			// 
			this.groupBoxTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxTags.Controls.Add(this.labelTags);
			this.groupBoxTags.Location = new System.Drawing.Point(12, 12);
			this.groupBoxTags.Name = "groupBoxTags";
			this.groupBoxTags.Size = new System.Drawing.Size(637, 73);
			this.groupBoxTags.TabIndex = 18;
			this.groupBoxTags.TabStop = false;
			this.groupBoxTags.Text = "Template Replacement Tags";
			// 
			// labelTags
			// 
			this.labelTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTags.Location = new System.Drawing.Point(3, 16);
			this.labelTags.Name = "labelTags";
			this.labelTags.Size = new System.Drawing.Size(631, 54);
			this.labelTags.TabIndex = 19;
			this.labelTags.Text = "Use template tags to create dynamic messages.";
			// 
			// tabEmailTemplate
			// 
			this.tabEmailTemplate.BackColor = System.Drawing.SystemColors.Control;
			this.tabEmailTemplate.Controls.Add(this.groupBoxEmailSubjAggShared);
			this.tabEmailTemplate.Controls.Add(this.groupBoxEmailAggShared);
			this.tabEmailTemplate.Controls.Add(this.groupBoxEmailAggPerAppt);
			this.tabEmailTemplate.Location = new System.Drawing.Point(4, 22);
			this.tabEmailTemplate.Name = "tabEmailTemplate";
			this.tabEmailTemplate.Padding = new System.Windows.Forms.Padding(3);
			this.tabEmailTemplate.Size = new System.Drawing.Size(633, 436);
			this.tabEmailTemplate.TabIndex = 1;
			this.tabEmailTemplate.Text = "Email Templates";
			// 
			// groupBoxEmailSubjAggShared
			// 
			this.groupBoxEmailSubjAggShared.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxEmailSubjAggShared.BackColor = System.Drawing.SystemColors.Control;
			this.groupBoxEmailSubjAggShared.Controls.Add(this.labelEmailSubjAggShared);
			this.groupBoxEmailSubjAggShared.Controls.Add(this.textEmailSubjAggShared);
			this.groupBoxEmailSubjAggShared.Location = new System.Drawing.Point(4, 2);
			this.groupBoxEmailSubjAggShared.Name = "groupBoxEmailSubjAggShared";
			this.groupBoxEmailSubjAggShared.Size = new System.Drawing.Size(627, 66);
			this.groupBoxEmailSubjAggShared.TabIndex = 12;
			this.groupBoxEmailSubjAggShared.TabStop = false;
			this.groupBoxEmailSubjAggShared.Text = "Aggregated E-mail Subject";
			// 
			// labelEmailSubjAggShared
			// 
			this.labelEmailSubjAggShared.Location = new System.Drawing.Point(7, 20);
			this.labelEmailSubjAggShared.Name = "labelEmailSubjAggShared";
			this.labelEmailSubjAggShared.Size = new System.Drawing.Size(471, 13);
			this.labelEmailSubjAggShared.TabIndex = 13;
			this.labelEmailSubjAggShared.Text = "The subject heading template.";
			// 
			// textEmailSubjAggShared
			// 
			this.textEmailSubjAggShared.AcceptsTab = true;
			this.textEmailSubjAggShared.BackColor = System.Drawing.SystemColors.Window;
			this.textEmailSubjAggShared.DetectLinksEnabled = false;
			this.textEmailSubjAggShared.DetectUrls = false;
			this.textEmailSubjAggShared.Location = new System.Drawing.Point(6, 39);
			this.textEmailSubjAggShared.Name = "textEmailSubjAggShared";
			this.textEmailSubjAggShared.QuickPasteType = OpenDentBusiness.QuickPasteType.Email;
			this.textEmailSubjAggShared.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textEmailSubjAggShared.Size = new System.Drawing.Size(615, 20);
			this.textEmailSubjAggShared.TabIndex = 3;
			this.textEmailSubjAggShared.Text = "";
			// 
			// groupBoxEmailAggShared
			// 
			this.groupBoxEmailAggShared.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxEmailAggShared.BackColor = System.Drawing.SystemColors.Control;
			this.groupBoxEmailAggShared.Controls.Add(this.butEditEmail);
			this.groupBoxEmailAggShared.Controls.Add(this.browserEmailBody);
			this.groupBoxEmailAggShared.Controls.Add(this.labelEmailAggShared);
			this.groupBoxEmailAggShared.Location = new System.Drawing.Point(5, 68);
			this.groupBoxEmailAggShared.Name = "groupBoxEmailAggShared";
			this.groupBoxEmailAggShared.Size = new System.Drawing.Size(627, 229);
			this.groupBoxEmailAggShared.TabIndex = 14;
			this.groupBoxEmailAggShared.TabStop = false;
			this.groupBoxEmailAggShared.Text = "Aggregated E-mail Template";
			// 
			// butEditEmail
			// 
			this.butEditEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butEditEmail.Location = new System.Drawing.Point(546, 198);
			this.butEditEmail.Name = "butEditEmail";
			this.butEditEmail.Size = new System.Drawing.Size(75, 26);
			this.butEditEmail.TabIndex = 127;
			this.butEditEmail.Text = "&Edit";
			this.butEditEmail.UseVisualStyleBackColor = true;
			this.butEditEmail.Click += new System.EventHandler(this.butEditEmail_Click);
			// 
			// browserEmailBody
			// 
			this.browserEmailBody.AllowWebBrowserDrop = false;
			this.browserEmailBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.browserEmailBody.Location = new System.Drawing.Point(10, 36);
			this.browserEmailBody.MinimumSize = new System.Drawing.Size(20, 20);
			this.browserEmailBody.Name = "browserEmailBody";
			this.browserEmailBody.Size = new System.Drawing.Size(611, 158);
			this.browserEmailBody.TabIndex = 115;
			this.browserEmailBody.WebBrowserShortcutsEnabled = false;
			// 
			// labelEmailAggShared
			// 
			this.labelEmailAggShared.Location = new System.Drawing.Point(7, 20);
			this.labelEmailAggShared.Name = "labelEmailAggShared";
			this.labelEmailAggShared.Size = new System.Drawing.Size(471, 13);
			this.labelEmailAggShared.TabIndex = 16;
			this.labelEmailAggShared.Text = "The message body template. Used once per aggregate message.";
			// 
			// groupBoxEmailAggPerAppt
			// 
			this.groupBoxEmailAggPerAppt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxEmailAggPerAppt.BackColor = System.Drawing.SystemColors.Control;
			this.groupBoxEmailAggPerAppt.Controls.Add(this.labelEmailAggPerAppt);
			this.groupBoxEmailAggPerAppt.Controls.Add(this.textEmailAggPerAppt);
			this.groupBoxEmailAggPerAppt.Location = new System.Drawing.Point(4, 303);
			this.groupBoxEmailAggPerAppt.Name = "groupBoxEmailAggPerAppt";
			this.groupBoxEmailAggPerAppt.Size = new System.Drawing.Size(627, 134);
			this.groupBoxEmailAggPerAppt.TabIndex = 16;
			this.groupBoxEmailAggPerAppt.TabStop = false;
			this.groupBoxEmailAggPerAppt.Text = "Aggregated E-mail Template Per Appointment";
			// 
			// labelEmailAggPerAppt
			// 
			this.labelEmailAggPerAppt.Location = new System.Drawing.Point(7, 13);
			this.labelEmailAggPerAppt.Name = "labelEmailAggPerAppt";
			this.labelEmailAggPerAppt.Size = new System.Drawing.Size(471, 26);
			this.labelEmailAggPerAppt.TabIndex = 17;
			this.labelEmailAggPerAppt.Text = "A single appointment template. Formats each appointment listed in the aggregate m" +
    "essage.";
			this.labelEmailAggPerAppt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textEmailAggPerAppt
			// 
			this.textEmailAggPerAppt.AcceptsTab = true;
			this.textEmailAggPerAppt.BackColor = System.Drawing.SystemColors.Window;
			this.textEmailAggPerAppt.DetectLinksEnabled = false;
			this.textEmailAggPerAppt.DetectUrls = false;
			this.textEmailAggPerAppt.Location = new System.Drawing.Point(6, 39);
			this.textEmailAggPerAppt.Name = "textEmailAggPerAppt";
			this.textEmailAggPerAppt.QuickPasteType = OpenDentBusiness.QuickPasteType.Email;
			this.textEmailAggPerAppt.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textEmailAggPerAppt.Size = new System.Drawing.Size(615, 89);
			this.textEmailAggPerAppt.TabIndex = 5;
			this.textEmailAggPerAppt.Text = "";
			// 
			// tabSMSTemplate
			// 
			this.tabSMSTemplate.BackColor = System.Drawing.SystemColors.Control;
			this.tabSMSTemplate.Controls.Add(this.groupBoxSMSAggPerAppt);
			this.tabSMSTemplate.Controls.Add(this.groupBoxSMSAggShared);
			this.tabSMSTemplate.Location = new System.Drawing.Point(4, 22);
			this.tabSMSTemplate.Name = "tabSMSTemplate";
			this.tabSMSTemplate.Padding = new System.Windows.Forms.Padding(3);
			this.tabSMSTemplate.Size = new System.Drawing.Size(633, 436);
			this.tabSMSTemplate.TabIndex = 0;
			this.tabSMSTemplate.Text = "SMS Templates";
			// 
			// groupBoxSMSAggPerAppt
			// 
			this.groupBoxSMSAggPerAppt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxSMSAggPerAppt.BackColor = System.Drawing.SystemColors.Control;
			this.groupBoxSMSAggPerAppt.Controls.Add(this.labelSMSAggPerAppt);
			this.groupBoxSMSAggPerAppt.Controls.Add(this.textSMSAggPerAppt);
			this.groupBoxSMSAggPerAppt.Location = new System.Drawing.Point(2, 4);
			this.groupBoxSMSAggPerAppt.Name = "groupBoxSMSAggPerAppt";
			this.groupBoxSMSAggPerAppt.Size = new System.Drawing.Size(625, 155);
			this.groupBoxSMSAggPerAppt.TabIndex = 10;
			this.groupBoxSMSAggPerAppt.TabStop = false;
			this.groupBoxSMSAggPerAppt.Text = "Aggregated SMS Template Per Appointment";
			// 
			// labelSMSAggPerAppt
			// 
			this.labelSMSAggPerAppt.Location = new System.Drawing.Point(7, 20);
			this.labelSMSAggPerAppt.Name = "labelSMSAggPerAppt";
			this.labelSMSAggPerAppt.Size = new System.Drawing.Size(471, 13);
			this.labelSMSAggPerAppt.TabIndex = 11;
			this.labelSMSAggPerAppt.Text = "A single appointment template. Formats each appointment listed in the aggregate m" +
    "essage.";
			// 
			// textSMSAggPerAppt
			// 
			this.textSMSAggPerAppt.AcceptsTab = true;
			this.textSMSAggPerAppt.BackColor = System.Drawing.SystemColors.Window;
			this.textSMSAggPerAppt.DetectLinksEnabled = false;
			this.textSMSAggPerAppt.DetectUrls = false;
			this.textSMSAggPerAppt.Location = new System.Drawing.Point(6, 39);
			this.textSMSAggPerAppt.Name = "textSMSAggPerAppt";
			this.textSMSAggPerAppt.QuickPasteType = OpenDentBusiness.QuickPasteType.TxtMsg;
			this.textSMSAggPerAppt.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textSMSAggPerAppt.Size = new System.Drawing.Size(615, 110);
			this.textSMSAggPerAppt.TabIndex = 2;
			this.textSMSAggPerAppt.Text = "";
			// 
			// groupBoxSMSAggShared
			// 
			this.groupBoxSMSAggShared.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxSMSAggShared.BackColor = System.Drawing.SystemColors.Control;
			this.groupBoxSMSAggShared.Controls.Add(this.labelSMSAggShared);
			this.groupBoxSMSAggShared.Controls.Add(this.textSMSAggShared);
			this.groupBoxSMSAggShared.Location = new System.Drawing.Point(3, 165);
			this.groupBoxSMSAggShared.Name = "groupBoxSMSAggShared";
			this.groupBoxSMSAggShared.Size = new System.Drawing.Size(624, 211);
			this.groupBoxSMSAggShared.TabIndex = 8;
			this.groupBoxSMSAggShared.TabStop = false;
			this.groupBoxSMSAggShared.Text = "Aggregated SMS Template";
			// 
			// labelSMSAggShared
			// 
			this.labelSMSAggShared.Location = new System.Drawing.Point(7, 20);
			this.labelSMSAggShared.Name = "labelSMSAggShared";
			this.labelSMSAggShared.Size = new System.Drawing.Size(471, 13);
			this.labelSMSAggShared.TabIndex = 9;
			this.labelSMSAggShared.Text = "The message body template. Used once per aggregate message.";
			// 
			// textSMSAggShared
			// 
			this.textSMSAggShared.AcceptsTab = true;
			this.textSMSAggShared.BackColor = System.Drawing.SystemColors.Window;
			this.textSMSAggShared.DetectLinksEnabled = false;
			this.textSMSAggShared.DetectUrls = false;
			this.textSMSAggShared.Location = new System.Drawing.Point(6, 39);
			this.textSMSAggShared.Name = "textSMSAggShared";
			this.textSMSAggShared.QuickPasteType = OpenDentBusiness.QuickPasteType.TxtMsg;
			this.textSMSAggShared.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textSMSAggShared.Size = new System.Drawing.Size(615, 166);
			this.textSMSAggShared.TabIndex = 1;
			this.textSMSAggShared.Text = "";
			// 
			// tabTemplates
			// 
			this.tabTemplates.Controls.Add(this.tabEmailTemplate);
			this.tabTemplates.Controls.Add(this.tabSMSTemplate);
			this.tabTemplates.Controls.Add(this.tabAutoReplyTemplate);
			this.tabTemplates.Location = new System.Drawing.Point(12, 91);
			this.tabTemplates.Name = "tabTemplates";
			this.tabTemplates.SelectedIndex = 0;
			this.tabTemplates.Size = new System.Drawing.Size(641, 462);
			this.tabTemplates.TabIndex = 10;
			this.tabTemplates.SelectedIndexChanged += new System.EventHandler(this.tabTemplates_SelectedIndexChanged);
			// 
			// tabAutoReplyTemplate
			// 
			this.tabAutoReplyTemplate.BackColor = System.Drawing.SystemColors.Control;
			this.tabAutoReplyTemplate.Controls.Add(this.groupAggregateAutoReplyTemplate);
			this.tabAutoReplyTemplate.Controls.Add(this.groupAutoReplySingle);
			this.tabAutoReplyTemplate.Location = new System.Drawing.Point(4, 22);
			this.tabAutoReplyTemplate.Name = "tabAutoReplyTemplate";
			this.tabAutoReplyTemplate.Size = new System.Drawing.Size(633, 436);
			this.tabAutoReplyTemplate.TabIndex = 2;
			this.tabAutoReplyTemplate.Text = "Auto Reply Templates";
			// 
			// groupAggregateAutoReplyTemplate
			// 
			this.groupAggregateAutoReplyTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupAggregateAutoReplyTemplate.BackColor = System.Drawing.SystemColors.Control;
			this.groupAggregateAutoReplyTemplate.Controls.Add(this.labelAggregateAutoReply);
			this.groupAggregateAutoReplyTemplate.Controls.Add(this.textAggregateAutoReply);
			this.groupAggregateAutoReplyTemplate.Location = new System.Drawing.Point(2, 141);
			this.groupAggregateAutoReplyTemplate.Name = "groupAggregateAutoReplyTemplate";
			this.groupAggregateAutoReplyTemplate.Size = new System.Drawing.Size(628, 155);
			this.groupAggregateAutoReplyTemplate.TabIndex = 12;
			this.groupAggregateAutoReplyTemplate.TabStop = false;
			this.groupAggregateAutoReplyTemplate.Text = "Aggregate Auto Reply Template";
			// 
			// labelAggregateAutoReply
			// 
			this.labelAggregateAutoReply.Location = new System.Drawing.Point(7, 20);
			this.labelAggregateAutoReply.Name = "labelAggregateAutoReply";
			this.labelAggregateAutoReply.Size = new System.Drawing.Size(471, 13);
			this.labelAggregateAutoReply.TabIndex = 11;
			this.labelAggregateAutoReply.Text = "Aggregate appointment confirmation auto reply template.";
			// 
			// textAggregateAutoReply
			// 
			this.textAggregateAutoReply.AcceptsTab = true;
			this.textAggregateAutoReply.BackColor = System.Drawing.SystemColors.Window;
			this.textAggregateAutoReply.DetectLinksEnabled = false;
			this.textAggregateAutoReply.DetectUrls = false;
			this.textAggregateAutoReply.Location = new System.Drawing.Point(6, 39);
			this.textAggregateAutoReply.Name = "textAggregateAutoReply";
			this.textAggregateAutoReply.QuickPasteType = OpenDentBusiness.QuickPasteType.TxtMsg;
			this.textAggregateAutoReply.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textAggregateAutoReply.Size = new System.Drawing.Size(615, 110);
			this.textAggregateAutoReply.TabIndex = 2;
			this.textAggregateAutoReply.Text = "";
			// 
			// groupAutoReplySingle
			// 
			this.groupAutoReplySingle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupAutoReplySingle.BackColor = System.Drawing.SystemColors.Control;
			this.groupAutoReplySingle.Controls.Add(this.labelSingleAutoReply);
			this.groupAutoReplySingle.Controls.Add(this.textSingleAutoReply);
			this.groupAutoReplySingle.Location = new System.Drawing.Point(2, 3);
			this.groupAutoReplySingle.Name = "groupAutoReplySingle";
			this.groupAutoReplySingle.Size = new System.Drawing.Size(628, 155);
			this.groupAutoReplySingle.TabIndex = 11;
			this.groupAutoReplySingle.TabStop = false;
			this.groupAutoReplySingle.Text = "Single Auto Reply Template";
			// 
			// labelSingleAutoReply
			// 
			this.labelSingleAutoReply.Location = new System.Drawing.Point(7, 20);
			this.labelSingleAutoReply.Name = "labelSingleAutoReply";
			this.labelSingleAutoReply.Size = new System.Drawing.Size(471, 13);
			this.labelSingleAutoReply.TabIndex = 11;
			this.labelSingleAutoReply.Text = "A single appointment confirmation auto reply template.";
			// 
			// textSingleAutoReply
			// 
			this.textSingleAutoReply.AcceptsTab = true;
			this.textSingleAutoReply.BackColor = System.Drawing.SystemColors.Window;
			this.textSingleAutoReply.DetectLinksEnabled = false;
			this.textSingleAutoReply.DetectUrls = false;
			this.textSingleAutoReply.Location = new System.Drawing.Point(6, 39);
			this.textSingleAutoReply.Name = "textSingleAutoReply";
			this.textSingleAutoReply.QuickPasteType = OpenDentBusiness.QuickPasteType.TxtMsg;
			this.textSingleAutoReply.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textSingleAutoReply.Size = new System.Drawing.Size(615, 110);
			this.textSingleAutoReply.TabIndex = 2;
			this.textSingleAutoReply.Text = "";
			// 
			// FormApptReminderRuleAggEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(668, 603);
			this.Controls.Add(this.tabTemplates);
			this.Controls.Add(this.groupBoxTags);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormApptReminderRuleAggEdit";
			this.Text = "Automated Messages Advanced Settings";
			this.Load += new System.EventHandler(this.FormApptReminderRuleEdit_Load);
			this.groupBoxTags.ResumeLayout(false);
			this.tabEmailTemplate.ResumeLayout(false);
			this.groupBoxEmailSubjAggShared.ResumeLayout(false);
			this.groupBoxEmailAggShared.ResumeLayout(false);
			this.groupBoxEmailAggPerAppt.ResumeLayout(false);
			this.tabSMSTemplate.ResumeLayout(false);
			this.groupBoxSMSAggPerAppt.ResumeLayout(false);
			this.groupBoxSMSAggShared.ResumeLayout(false);
			this.tabTemplates.ResumeLayout(false);
			this.tabAutoReplyTemplate.ResumeLayout(false);
			this.groupAggregateAutoReplyTemplate.ResumeLayout(false);
			this.groupAutoReplySingle.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.GroupBox groupBoxTags;
		private System.Windows.Forms.Label labelTags;
		private System.Windows.Forms.TabPage tabEmailTemplate;
		private System.Windows.Forms.GroupBox groupBoxEmailSubjAggShared;
		private System.Windows.Forms.Label labelEmailSubjAggShared;
		private ODtextBox textEmailSubjAggShared;
		private System.Windows.Forms.GroupBox groupBoxEmailAggShared;
		private UI.Button butEditEmail;
		private System.Windows.Forms.WebBrowser browserEmailBody;
		private System.Windows.Forms.Label labelEmailAggShared;
		private System.Windows.Forms.GroupBox groupBoxEmailAggPerAppt;
		private System.Windows.Forms.Label labelEmailAggPerAppt;
		private ODtextBox textEmailAggPerAppt;
		private System.Windows.Forms.TabPage tabSMSTemplate;
		private System.Windows.Forms.GroupBox groupBoxSMSAggPerAppt;
		private System.Windows.Forms.Label labelSMSAggPerAppt;
		private ODtextBox textSMSAggPerAppt;
		private System.Windows.Forms.GroupBox groupBoxSMSAggShared;
		private System.Windows.Forms.Label labelSMSAggShared;
		private ODtextBox textSMSAggShared;
		private System.Windows.Forms.TabControl tabTemplates;
		private System.Windows.Forms.TabPage tabAutoReplyTemplate;
		private System.Windows.Forms.GroupBox groupAggregateAutoReplyTemplate;
		private System.Windows.Forms.Label labelAggregateAutoReply;
		private ODtextBox textAggregateAutoReply;
		private System.Windows.Forms.GroupBox groupAutoReplySingle;
		private System.Windows.Forms.Label labelSingleAutoReply;
		private ODtextBox textSingleAutoReply;
	}
}