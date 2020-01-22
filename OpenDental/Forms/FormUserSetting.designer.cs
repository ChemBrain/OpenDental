namespace OpenDental{
	partial class FormUserSetting {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserSetting));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkSuppressMessage = new System.Windows.Forms.CheckBox();
			this.comboTheme = new System.Windows.Forms.ComboBox();
			this.labelTheme = new System.Windows.Forms.Label();
			this.textLogOffAfterMinutes = new System.Windows.Forms.TextBox();
			this.labelAutoLogoff = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(176, 228);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(257, 228);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkSuppressMessage
			// 
			this.checkSuppressMessage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSuppressMessage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSuppressMessage.Location = new System.Drawing.Point(5, 25);
			this.checkSuppressMessage.Name = "checkSuppressMessage";
			this.checkSuppressMessage.Size = new System.Drawing.Size(327, 17);
			this.checkSuppressMessage.TabIndex = 4;
			this.checkSuppressMessage.Text = "Log off message is suppressed";
			this.checkSuppressMessage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTheme
			// 
			this.comboTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTheme.FormattingEnabled = true;
			this.comboTheme.Location = new System.Drawing.Point(202, 48);
			this.comboTheme.Name = "comboTheme";
			this.comboTheme.Size = new System.Drawing.Size(130, 21);
			this.comboTheme.TabIndex = 232;
			// 
			// labelTheme
			// 
			this.labelTheme.Location = new System.Drawing.Point(118, 48);
			this.labelTheme.Name = "labelTheme";
			this.labelTheme.Size = new System.Drawing.Size(81, 18);
			this.labelTheme.TabIndex = 233;
			this.labelTheme.Text = "Theme";
			this.labelTheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textLogOffAfterMinutes
			// 
			this.textLogOffAfterMinutes.Location = new System.Drawing.Point(303, 77);
			this.textLogOffAfterMinutes.Name = "textLogOffAfterMinutes";
			this.textLogOffAfterMinutes.Size = new System.Drawing.Size(29, 20);
			this.textLogOffAfterMinutes.TabIndex = 264;
			this.textLogOffAfterMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// labelAutoLogoff
			// 
			this.labelAutoLogoff.Location = new System.Drawing.Point(2, 71);
			this.labelAutoLogoff.Name = "labelAutoLogoff";
			this.labelAutoLogoff.Size = new System.Drawing.Size(298, 32);
			this.labelAutoLogoff.TabIndex = 265;
			this.labelAutoLogoff.Text = "Automatic logoff time in minutes\r\n(0 is disabled, blank is global value)";
			this.labelAutoLogoff.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormUserSetting
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(344, 264);
			this.Controls.Add(this.textLogOffAfterMinutes);
			this.Controls.Add(this.labelAutoLogoff);
			this.Controls.Add(this.comboTheme);
			this.Controls.Add(this.labelTheme);
			this.Controls.Add(this.checkSuppressMessage);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormUserSetting";
			this.Text = "User Settings";
			this.Load += new System.EventHandler(this.FormUserSetting_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.CheckBox checkSuppressMessage;
		private System.Windows.Forms.ComboBox comboTheme;
		private System.Windows.Forms.Label labelTheme;
		private System.Windows.Forms.TextBox textLogOffAfterMinutes;
		private System.Windows.Forms.Label labelAutoLogoff;
	}
}