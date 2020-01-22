using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;
using CodeBase;

namespace OpenDental{
	/// <summary></summary>
	public class FormOperatoryEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsNew;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private UI.ComboBoxClinicPicker comboClinic;
		private System.Windows.Forms.TextBox textOpName;
		private System.Windows.Forms.TextBox textAbbrev;
		private System.Windows.Forms.CheckBox checkIsHidden;
		private UI.ComboBoxPlus comboHyg;
		private UI.ComboBoxPlus comboProv;
		private System.Windows.Forms.CheckBox checkIsHygiene;
		private CheckBox checkSetProspective;
		private Label label3;
		private Operatory OpCur;
		private Label label4;
		private CheckBox checkIsWebSched;
		private UI.Button butPickProv;
		private UI.Button butPickHyg;
		public List<Operatory> ListOps;
		private Label labelApptType;
		private GroupBox groupBoxApptType;
		private Label label10;
		private UI.Button butWSNPAPickApptTypes;
		private TextBox textWSNPAApptTypes;
		///<summary>All of the Web Sched New Pat Appt appointment type defs that this operatory is associated to.</summary>
		private List<Def> _listWSNPAOperatoryDefs=new List<Def>();
		private UI.Button butUpdateProvs;
		private Label label5;
		private Label label11;
		///<summary>This reference is passed in because it's needed for the "Update Provs on Future Appts" tool.</summary>
		public ContrAppt ContrApptRef;

		///<summary></summary>
		public FormOperatoryEdit(Operatory opCur)
		{
			//
			// Required for Windows Form Designer support
			//
			OpCur=opCur;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOperatoryEdit));
			this.labelApptType = new System.Windows.Forms.Label();
			this.butPickHyg = new OpenDental.UI.Button();
			this.butPickProv = new OpenDental.UI.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.checkIsWebSched = new System.Windows.Forms.CheckBox();
			this.comboClinic = new OpenDental.UI.ComboBoxClinicPicker();
			this.label3 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.checkSetProspective = new System.Windows.Forms.CheckBox();
			this.checkIsHygiene = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.comboHyg = new OpenDental.UI.ComboBoxPlus();
			this.comboProv = new OpenDental.UI.ComboBoxPlus();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.checkIsHidden = new System.Windows.Forms.CheckBox();
			this.textAbbrev = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textOpName = new System.Windows.Forms.TextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBoxApptType = new System.Windows.Forms.GroupBox();
			this.textWSNPAApptTypes = new System.Windows.Forms.TextBox();
			this.butWSNPAPickApptTypes = new OpenDental.UI.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.butUpdateProvs = new OpenDental.UI.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.groupBoxApptType.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelApptType
			// 
			this.labelApptType.Location = new System.Drawing.Point(16, 42);
			this.labelApptType.Name = "labelApptType";
			this.labelApptType.Size = new System.Drawing.Size(117, 17);
			this.labelApptType.TabIndex = 126;
			this.labelApptType.Text = "New Pat Appt Types";
			this.labelApptType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPickHyg
			// 
			this.butPickHyg.Location = new System.Drawing.Point(412, 123);
			this.butPickHyg.Name = "butPickHyg";
			this.butPickHyg.Size = new System.Drawing.Size(23, 21);
			this.butPickHyg.TabIndex = 123;
			this.butPickHyg.Text = "...";
			this.butPickHyg.Click += new System.EventHandler(this.butPickHyg_Click);
			// 
			// butPickProv
			// 
			this.butPickProv.Location = new System.Drawing.Point(412, 102);
			this.butPickProv.Name = "butPickProv";
			this.butPickProv.Size = new System.Drawing.Size(23, 21);
			this.butPickProv.TabIndex = 122;
			this.butPickProv.Text = "...";
			this.butPickProv.Click += new System.EventHandler(this.butPickProv_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(151, 20);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(358, 16);
			this.label4.TabIndex = 120;
			this.label4.Text = "This operatory will be available for Web Sched recall appointments.";
			// 
			// checkIsWebSched
			// 
			this.checkIsWebSched.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsWebSched.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsWebSched.Location = new System.Drawing.Point(11, 19);
			this.checkIsWebSched.Name = "checkIsWebSched";
			this.checkIsWebSched.Size = new System.Drawing.Size(135, 16);
			this.checkIsWebSched.TabIndex = 119;
			this.checkIsWebSched.Text = "Is Recall";
			this.checkIsWebSched.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClinic
			// 
			this.comboClinic.HqDescription = "None";
			this.comboClinic.IncludeUnassigned = true;
			this.comboClinic.Location = new System.Drawing.Point(123, 81);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(289, 21);
			this.comboClinic.TabIndex = 3;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.ComboClinic_SelectionChangeCommitted);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(178, 169);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(363, 16);
			this.label3.TabIndex = 117;
			this.label3.Text = "Change status of patients in this operatory to prospective.";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(178, 150);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(363, 16);
			this.label9.TabIndex = 117;
			this.label9.Text = "The hygienist will be considered the main provider for this op.";
			// 
			// checkSetProspective
			// 
			this.checkSetProspective.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSetProspective.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSetProspective.Location = new System.Drawing.Point(69, 168);
			this.checkSetProspective.Name = "checkSetProspective";
			this.checkSetProspective.Size = new System.Drawing.Size(104, 16);
			this.checkSetProspective.TabIndex = 7;
			this.checkSetProspective.Text = "Set Prospective";
			this.checkSetProspective.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIsHygiene
			// 
			this.checkIsHygiene.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsHygiene.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsHygiene.Location = new System.Drawing.Point(69, 149);
			this.checkIsHygiene.Name = "checkIsHygiene";
			this.checkIsHygiene.Size = new System.Drawing.Size(104, 16);
			this.checkIsHygiene.TabIndex = 6;
			this.checkIsHygiene.Text = "Is Hygiene";
			this.checkIsHygiene.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(178, 64);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(316, 16);
			this.label8.TabIndex = 115;
			this.label8.Text = "(because you can\'t delete an operatory)";
			// 
			// comboHyg
			// 
			this.comboHyg.Location = new System.Drawing.Point(160, 123);
			this.comboHyg.Name = "comboHyg";
			this.comboHyg.Size = new System.Drawing.Size(252, 21);
			this.comboHyg.TabIndex = 5;
			// 
			// comboProv
			// 
			this.comboProv.Location = new System.Drawing.Point(160, 102);
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(252, 21);
			this.comboProv.TabIndex = 4;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(36, 125);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(123, 16);
			this.label6.TabIndex = 112;
			this.label6.Text = "Hygienist";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(25, 104);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(134, 16);
			this.label7.TabIndex = 111;
			this.label7.Text = "Provider";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIsHidden
			// 
			this.checkIsHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsHidden.Location = new System.Drawing.Point(69, 63);
			this.checkIsHidden.Name = "checkIsHidden";
			this.checkIsHidden.Size = new System.Drawing.Size(104, 16);
			this.checkIsHidden.TabIndex = 2;
			this.checkIsHidden.Text = "Is Hidden";
			this.checkIsHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAbbrev
			// 
			this.textAbbrev.Location = new System.Drawing.Point(160, 40);
			this.textAbbrev.MaxLength = 5;
			this.textAbbrev.Name = "textAbbrev";
			this.textAbbrev.Size = new System.Drawing.Size(78, 20);
			this.textAbbrev.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(151, 17);
			this.label2.TabIndex = 99;
			this.label2.Text = "Abbrev (max 5 char)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textOpName
			// 
			this.textOpName.Location = new System.Drawing.Point(160, 20);
			this.textOpName.MaxLength = 255;
			this.textOpName.Name = "textOpName";
			this.textOpName.Size = new System.Drawing.Size(252, 20);
			this.textOpName.TabIndex = 0;
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(396, 354);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 8;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(487, 354);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 9;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(9, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(148, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "Op Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBoxApptType
			// 
			this.groupBoxApptType.Controls.Add(this.textWSNPAApptTypes);
			this.groupBoxApptType.Controls.Add(this.butWSNPAPickApptTypes);
			this.groupBoxApptType.Controls.Add(this.label10);
			this.groupBoxApptType.Controls.Add(this.labelApptType);
			this.groupBoxApptType.Controls.Add(this.label4);
			this.groupBoxApptType.Controls.Add(this.checkIsWebSched);
			this.groupBoxApptType.Location = new System.Drawing.Point(26, 190);
			this.groupBoxApptType.Name = "groupBoxApptType";
			this.groupBoxApptType.Size = new System.Drawing.Size(515, 100);
			this.groupBoxApptType.TabIndex = 128;
			this.groupBoxApptType.TabStop = false;
			this.groupBoxApptType.Text = "Web Sched Settings";
			// 
			// textWSNPAApptTypes
			// 
			this.textWSNPAApptTypes.Location = new System.Drawing.Point(134, 41);
			this.textWSNPAApptTypes.MaxLength = 255;
			this.textWSNPAApptTypes.Name = "textWSNPAApptTypes";
			this.textWSNPAApptTypes.ReadOnly = true;
			this.textWSNPAApptTypes.Size = new System.Drawing.Size(252, 20);
			this.textWSNPAApptTypes.TabIndex = 129;
			// 
			// butWSNPAPickApptTypes
			// 
			this.butWSNPAPickApptTypes.Location = new System.Drawing.Point(386, 40);
			this.butWSNPAPickApptTypes.Name = "butWSNPAPickApptTypes";
			this.butWSNPAPickApptTypes.Size = new System.Drawing.Size(23, 22);
			this.butWSNPAPickApptTypes.TabIndex = 129;
			this.butWSNPAPickApptTypes.Text = "...";
			this.butWSNPAPickApptTypes.Click += new System.EventHandler(this.butWSNPAPickApptTypes_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(131, 65);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(378, 29);
			this.label10.TabIndex = 128;
			this.label10.Text = "Only the above appointment types will be allowed within this operatory.\r\nAppt Typ" +
    "e is required to be considered for Web Sched New Pat Appt.";
			// 
			// butUpdateProvs
			// 
			this.butUpdateProvs.Location = new System.Drawing.Point(160, 300);
			this.butUpdateProvs.Name = "butUpdateProvs";
			this.butUpdateProvs.Size = new System.Drawing.Size(75, 26);
			this.butUpdateProvs.TabIndex = 129;
			this.butUpdateProvs.Text = "Update All";
			this.butUpdateProvs.Click += new System.EventHandler(this.ButUpdateProvs_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(-1, 305);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(160, 17);
			this.label5.TabIndex = 130;
			this.label5.Text = "Update Provs on Future Appts";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(240, 307);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(160, 21);
			this.label11.TabIndex = 131;
			this.label11.Text = "for this operatory.  ";
			// 
			// FormOperatoryEdit
			// 
			this.ClientSize = new System.Drawing.Size(574, 392);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.butUpdateProvs);
			this.Controls.Add(this.groupBoxApptType);
			this.Controls.Add(this.butPickHyg);
			this.Controls.Add(this.butPickProv);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.checkSetProspective);
			this.Controls.Add(this.checkIsHygiene);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.comboHyg);
			this.Controls.Add(this.comboProv);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.checkIsHidden);
			this.Controls.Add(this.textAbbrev);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textOpName);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormOperatoryEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Operatory";
			this.Load += new System.EventHandler(this.FormOperatoryEdit_Load);
			this.groupBoxApptType.ResumeLayout(false);
			this.groupBoxApptType.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormOperatoryEdit_Load(object sender, System.EventArgs e) {
			textOpName.Text=OpCur.OpName;
			textAbbrev.Text=OpCur.Abbrev;
			checkIsHidden.Checked=OpCur.IsHidden;
			comboClinic.SelectedClinicNum=OpCur.ClinicNum;//can be 0
			FillCombosProv();
			comboProv.SetSelectedProvNum(OpCur.ProvDentist);
			comboHyg.SetSelectedProvNum(OpCur.ProvHygienist);
			if(OpCur.ListWSNPAOperatoryDefNums!=null) {
				//This is an existing operatory with WSNPA appointment types associated.  Go get them in order to display to the user.
				_listWSNPAOperatoryDefs=Defs.GetDefs(DefCat.WebSchedNewPatApptTypes,OpCur.ListWSNPAOperatoryDefNums);
			}
			FillWSNPAApptTypes();
			checkIsHygiene.Checked=OpCur.IsHygiene;
			checkSetProspective.Checked=OpCur.SetProspective;
			checkIsWebSched.Checked=OpCur.IsWebSched;
		}

		private void FillWSNPAApptTypes() {
			textWSNPAApptTypes.Clear();
			if(_listWSNPAOperatoryDefs.Count < 1) {
				return;
			}
			textWSNPAApptTypes.Text=string.Join(", ",_listWSNPAOperatoryDefs.Select(x => x.ItemName));
		}

		private void butPickProv_Click(object sender,EventArgs e) {
			FormProviderPick FormPP=new FormProviderPick(comboProv.Items.GetAll<Provider>());
			FormPP.SelectedProvNum=comboProv.GetSelectedProvNum();
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			comboProv.SetSelectedProvNum(FormPP.SelectedProvNum);
		}

		private void butPickHyg_Click(object sender,EventArgs e) {
			FormProviderPick FormPP=new FormProviderPick(comboHyg.Items.GetAll<Provider>());
			FormPP.SelectedProvNum=comboHyg.GetSelectedProvNum();
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			comboHyg.SetSelectedProvNum(FormPP.SelectedProvNum);
		}

		private void ComboClinic_SelectionChangeCommitted(object sender, EventArgs e){
			FillCombosProv();
		}

		private void butWSNPAPickApptTypes_Click(object sender,EventArgs e) {
			FormDefinitionPicker FormDP=new FormDefinitionPicker(DefCat.WebSchedNewPatApptTypes,_listWSNPAOperatoryDefs);
			FormDP.IsMultiSelectionMode=true;
			if(FormDP.ShowDialog()==DialogResult.OK) {
				_listWSNPAOperatoryDefs=FormDP.ListSelectedDefs.Select(x => x.Copy()).ToList();
				FillWSNPAApptTypes();
			}
		}

		///<summary>Fills combo provider based on which clinic is selected and attempts to preserve provider selection if any.</summary>
		private void FillCombosProv() {
			long provNum=comboProv.GetSelectedProvNum();
			comboProv.Items.Clear();
			comboProv.Items.AddProvNone();
			comboProv.Items.AddProvsAbbr(Providers.GetProvsForClinic(comboClinic.SelectedClinicNum));
			comboProv.SetSelectedProvNum(provNum);
			provNum=comboHyg.GetSelectedProvNum();
			comboHyg.Items.Clear();
			comboHyg.Items.AddProvNone();
			comboHyg.Items.AddProvsAbbr(Providers.GetProvsForClinic(comboClinic.SelectedClinicNum));
			comboHyg.SetSelectedProvNum(provNum);
		}

		private void ButUpdateProvs_Click(object sender, EventArgs e){
			if(IsNew){
				MsgBox.Show(this,"Not for new operatories.");
				return;
			}
			//Check against cache. Instead of saving changes, make them get out and reopen. Safer and simpler.
			Operatory op=Operatories.GetOperatory(OpCur.OperatoryNum);
			if(op.OpName!=textOpName.Text
				|| op.Abbrev!=textAbbrev.Text
				|| op.IsHidden!=checkIsHidden.Checked
				|| op.ClinicNum!=comboClinic.SelectedClinicNum
				|| op.ProvDentist!=comboProv.GetSelectedProvNum()
				|| op.ProvHygienist!=comboHyg.GetSelectedProvNum()
				|| op.IsHygiene!=checkIsHygiene.Checked
				|| op.SetProspective!=checkSetProspective.Checked
				|| op.IsWebSched!=checkIsWebSched.Checked)
			{
				MsgBox.Show(this,"Changes were detected above.  Save all changes, get completely out of the operatories window, and then re-enter.");
				return;
			}
			if(!Security.IsAuthorized(Permissions.Setup) || !Security.IsAuthorized(Permissions.AppointmentEdit)) {
				return;
			}
			//Operatory operatory=Operatories.GetOperatory(contrApptPanel.OpNumClicked);
			if(Security.CurUser.ClinicIsRestricted && !Clinics.GetForUserod(Security.CurUser).Exists(x => x.ClinicNum==OpCur.ClinicNum)) {
				MsgBox.Show(this,"You are restricted from accessing the clinic belonging to the selected operatory.  No changes will be made.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,
				"WARNING: We recommend backing up your database before running this tool.  "
				+"This tool may take a very long time to run and should be run after hours.  "
				+"In addition, this tool could potentially change hundreds of appointments.  "
				+"The changes made by this tool can only be manually reversed.  "
				+"Are you sure you want to continue?"))
			{
				return;
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,Lan.g(this,"Update Provs on Future Appts tool run on operatory ")+OpCur.Abbrev+".");
			List<Appointment> listAppts=Appointments.GetAppointmentsForOpsByPeriod(new List<long>() {OpCur.OperatoryNum},DateTime.Now);//no end date, so all future
			List<Appointment> listApptsOld=new List<Appointment>();
			foreach(Appointment appt in listAppts) {
				listApptsOld.Add(appt.Copy());
			}
			ContrApptRef.MoveAppointments(listAppts,listApptsOld,OpCur);
			MsgBox.Show(this,"Done");
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textOpName.Text==""){
				MsgBox.Show(this,"Operatory name cannot be blank.");
				return;
			}
			if(checkIsHidden.Checked==true && Operatories.HasFutureApts(OpCur.OperatoryNum,ApptStatus.UnschedList)) {
				MsgBox.Show(this,"Can not hide an operatory with future appointments.");
				checkIsHidden.Checked=false;
				return;
			}
			OpCur.OpName=textOpName.Text;
			OpCur.Abbrev=textAbbrev.Text;
			OpCur.IsHidden=checkIsHidden.Checked;
			OpCur.ClinicNum=comboClinic.SelectedClinicNum;
			OpCur.ProvDentist=comboProv.GetSelectedProvNum();
			OpCur.ProvHygienist=comboHyg.GetSelectedProvNum();
			OpCur.IsHygiene=checkIsHygiene.Checked;
			OpCur.SetProspective=checkSetProspective.Checked;
			OpCur.IsWebSched=checkIsWebSched.Checked;
			OpCur.ListWSNPAOperatoryDefNums=_listWSNPAOperatoryDefs.Select(x => x.DefNum).ToList();
			if(IsNew) {
				ListOps.Insert(OpCur.ItemOrder,OpCur);//Insert into list at appropriate spot
				for(int i=0;i<ListOps.Count;i++) {
					ListOps[i].ItemOrder=i;//reset/correct item orders
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}

}





















