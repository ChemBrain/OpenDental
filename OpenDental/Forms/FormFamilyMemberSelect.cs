using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	/// <summary>For a given subscriber, this list all their plans.  User then selects one plan from the list or creates a blank plan.</summary>
	public class FormFamilyMemberSelect:ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.ListBox listPats;
		private OpenDental.UI.Button butOther;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Family Fam;
		///<summary>When dialogResult=OK, this will contain the PatNum of the selected pat.</summary>
		public long SelectedPatNum;
		///<summary>When this flag is set, the patient status will appear in parenthesis by the patient's name in the list.</summary>
		private bool _isPatientStatusVisible;

		///<summary></summary>
		public FormFamilyMemberSelect(Family fam,bool isPatientStatusVisible=false)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			Fam=fam;
			_isPatientStatusVisible=isPatientStatusVisible;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFamilyMemberSelect));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.listPats = new System.Windows.Forms.ListBox();
			this.butOther = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(318,227);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(227,227);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// listPats
			// 
			this.listPats.Location = new System.Drawing.Point(24,23);
			this.listPats.Name = "listPats";
			this.listPats.Size = new System.Drawing.Size(271,160);
			this.listPats.TabIndex = 2;
			this.listPats.DoubleClick += new System.EventHandler(this.listPats_DoubleClick);
			// 
			// butOther
			// 
			this.butOther.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butOther.Location = new System.Drawing.Point(22,227);
			this.butOther.Name = "butOther";
			this.butOther.Size = new System.Drawing.Size(76,24);
			this.butOther.TabIndex = 3;
			this.butOther.Text = "Other";
			this.butOther.Click += new System.EventHandler(this.butOther_Click);
			// 
			// FormFamilyMemberSelect
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(405,263);
			this.Controls.Add(this.butOther);
			this.Controls.Add(this.listPats);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormFamilyMemberSelect";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Select Family Member";
			this.Load += new System.EventHandler(this.FormFamilyMemberSelect_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormFamilyMemberSelect_Load(object sender, System.EventArgs e) {
			for(int i=0;i<Fam.ListPats.Length;i++){
				string patientStatus=" ("+Lan.g("enumPatientStatus",Fam.ListPats[i].PatStatus.GetDescription())+")";
				listPats.Items.Add(new ODBoxItem<Patient>(Fam.ListPats[i].GetNameFL()+(_isPatientStatusVisible ? patientStatus : ""),Fam.ListPats[i]));
			}
		}

		private void listPats_DoubleClick(object sender, System.EventArgs e) {
			if(listPats.SelectedIndex==-1){
				return;
			}
			SelectedPatNum=listPats.GetSelected<Patient>().PatNum;
			DialogResult=DialogResult.OK;
		}

		private void butOther_Click(object sender, System.EventArgs e) {
			FormPatientSelect FormPS=new FormPatientSelect();
			FormPS.SelectionModeOnly=true;
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK) {
				return;
			}
			SelectedPatNum=FormPS.SelectedPatNum;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(listPats.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			SelectedPatNum=listPats.GetSelected<Patient>().PatNum;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		


	}
}





















