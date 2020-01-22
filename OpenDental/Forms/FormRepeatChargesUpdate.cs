using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormRepeatChargesUpdate :ODForm{
		// ReSharper disable once InconsistentNaming
		private UI.Button butCancel;
		// ReSharper disable once InconsistentNaming
		private UI.Button butOK;
		private CheckBox checkRunAging;
		private Label labelDescription;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		///<summary></summary>
		public FormRepeatChargesUpdate()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRepeatChargesUpdate));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkRunAging = new System.Windows.Forms.CheckBox();
			this.labelDescription = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(352, 205);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(433, 205);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkRunAging
			// 
			this.checkRunAging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkRunAging.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRunAging.Location = new System.Drawing.Point(12, 210);
			this.checkRunAging.Name = "checkRunAging";
			this.checkRunAging.Size = new System.Drawing.Size(290, 17);
			this.checkRunAging.TabIndex = 4;
			this.checkRunAging.Text = "Run aging on accounts after posting charges.";
			this.checkRunAging.UseVisualStyleBackColor = true;
			this.checkRunAging.CheckedChanged += new System.EventHandler(this.checkRunAging_CheckedChanged);
			// 
			// labelDescription
			// 
			this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelDescription.Location = new System.Drawing.Point(12, 13);
			this.labelDescription.Name = "labelDescription";
			this.labelDescription.Size = new System.Drawing.Size(496, 182);
			this.labelDescription.TabIndex = 5;
			this.labelDescription.Text = resources.GetString("labelDescription.Text");
			// 
			// FormRepeatChargesUpdate
			// 
			this.ClientSize = new System.Drawing.Size(520, 243);
			this.Controls.Add(this.labelDescription);
			this.Controls.Add(this.checkRunAging);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(495, 271);
			this.Name = "FormRepeatChargesUpdate";
			this.ShowInTaskbar = false;
			this.Text = "Update Repeating Charges";
			this.Load += new System.EventHandler(this.FormRepeatChargesUpdate_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRepeatChargesUpdate_Load(object sender, EventArgs e) {
			checkRunAging.Checked=PrefC.GetBool(PrefName.RepeatingChargesRunAging);//this will cause the label text to be updated
		}

		///<summary>Do not use this method in release code. This is only to be used for Unit Tests 53-56.</summary>
		public void RunRepeatingChargesForUnitTests(DateTime dateRun) {
			RepeatCharges.RunRepeatingCharges(dateRun);
		}

		private void checkRunAging_CheckedChanged(object sender,EventArgs e) {
			labelDescription.Text=Lan.g(this,"This will add completed procedures to each account that has a repeating charge set up.   The date of the "
				+"procedure will be based on the settings in the repeating charge.")+"\r\n\r\n"
				+Lan.g(this,"This tool must be run at least every month for the repeating charges to be added.  The procedure will be backdated up "
				+"to one month and 20 days.")+"\r\n\r\n"
				+Lan.g(this,"If the repeating charge 'Creates Claim' check box is selected, and the patient has insurance, and the procedure added "
				+"is not marked as 'Do not usually bill to Ins', then a claim will be created for the procedure. If the patient has a secondary "
				+"insurance plan, a secondary claim will be created with a hold status.");
			if(!checkRunAging.Checked) {
				labelDescription.Text=labelDescription.Text+"\r\n\r\n"+Lan.g(this,"You should run aging when you are done.");
			}
		}

		///<summary>Checks if Repeating Charges are already running on another workstation or by the OpenDental Service.  If less than 24 hours have 
		///passed since the tool was started, user will be blocked from running Repeating Charges.  Otherwise, SecurityAdmin users can restart the tool.
		///</summary>
		private bool CheckBeginDateTime() {
			Prefs.RefreshCache();//Just to be sure we don't miss someone who has just started running repeating charges.
			if(PrefC.GetString(PrefName.RepeatingChargesBeginDateTime)=="") {
				return true;
			}
			DateTime repeatingChargesBeginDateTime=PrefC.GetDateT(PrefName.RepeatingChargesBeginDateTime);
			if((MiscData.GetNowDateTime()-repeatingChargesBeginDateTime).TotalHours < 24) {
				MsgBox.Show(this,"Repeating charges already running on another workstation, you must wait for them to finish before continuing.");
				return false;
			}
			//It's been more than 24 hours since repeat charges started.
			if(Security.IsAuthorized(Permissions.SecurityAdmin,true)) {
				string message=Lans.g(this,"Repeating Charges last started on")+" "+repeatingChargesBeginDateTime.ToString()
					+Lans.g(this,".  Restart repeating charges?");
				if(MsgBox.Show(MsgBoxButtons.OKCancel,message)) {
					SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin,0,"Restarted repeating charges. Previous Repeating Charges Begin DateTime was "
						+repeatingChargesBeginDateTime.ToString()+".");
					return true;
				}
				return false;//Security admin doesn't want to restart repeat charges.
			}
			//User isn't a security admin.
			MsgBox.Show(Lans.g(this,"Repeating Charges last started on")+" "+repeatingChargesBeginDateTime.ToString()
				+Lans.g(this,".  Contact a user with SecurityAdmin permission to restart repeating charges."));
			return false;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!CheckBeginDateTime()) {
				return;
			}
			//If the AvaTax API is not available at HQ show popup and return.
			if(AvaTax.IsEnabled() && !AvaTax.PingAvaTax()) {
				MsgBox.Show(this,"Unable to connect to AvaTax API.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			RepeatChargeResult result=RepeatCharges.RunRepeatingCharges(MiscData.GetNowDateTime(),checkRunAging.Checked);
			string metrics=result.ProceduresAddedCount+" "+Lan.g(this,"procedures added.")+"\r\n"+result.ClaimsAddedCount+" "
				+Lan.g(this,"claims added.");
			SecurityLogs.MakeLogEntry(Permissions.RepeatChargeTool,0,"Repeat Charge Tool ran.\r\n"+metrics);
			Cursor=Cursors.Default;
			MessageBox.Show(metrics);
			if(!string.IsNullOrEmpty(result.ErrorMsg)) {
				SecurityLogs.MakeLogEntry(Permissions.RepeatChargeTool,0,"Repeat Charge Tool Error: "+result.ErrorMsg);
				MessageBox.Show(result.ErrorMsg);
			}
			DialogResult=DialogResult.OK;
		}	

		private void butCancel_Click(object sender, EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		


	}
}





















