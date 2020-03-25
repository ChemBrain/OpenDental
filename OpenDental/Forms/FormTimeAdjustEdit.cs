using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormTimeAdjustEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsNew;
		private Label label1;
		private Label label2;
		private TextBox textTimeEntry;
		private Label label4;
		private TextBox textNote;
		private CheckBox checkOvertime;
		private OpenDental.UI.Button butDelete;
		private TextBox textHours;
		private Label label3;
		private RadioButton radioAuto;
		private RadioButton radioManual;
		private Label label5;
		private ComboBox comboPTO;
		private Label labelPTO;
		private TimeAdjust TimeAdjustCur;

		///<summary></summary>
		public FormTimeAdjustEdit(TimeAdjust timeAdjustCur){
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			TimeAdjustCur=timeAdjustCur.Copy();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTimeAdjustEdit));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textTimeEntry = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textNote = new System.Windows.Forms.TextBox();
			this.checkOvertime = new System.Windows.Forms.CheckBox();
			this.textHours = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.radioAuto = new System.Windows.Forms.RadioButton();
			this.radioManual = new System.Windows.Forms.RadioButton();
			this.label5 = new System.Windows.Forms.Label();
			this.comboPTO = new System.Windows.Forms.ComboBox();
			this.labelPTO = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(11, 49);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(126, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Date/Time Entry";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(11, 76);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(126, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Hours";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTimeEntry
			// 
			this.textTimeEntry.Location = new System.Drawing.Point(137, 50);
			this.textTimeEntry.Name = "textTimeEntry";
			this.textTimeEntry.Size = new System.Drawing.Size(155, 20);
			this.textTimeEntry.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(11, 150);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(126, 20);
			this.label4.TabIndex = 0;
			this.label4.Text = "Note";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textNote
			// 
			this.textNote.Location = new System.Drawing.Point(137, 151);
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.Size = new System.Drawing.Size(377, 96);
			this.textNote.TabIndex = 7;
			// 
			// checkOvertime
			// 
			this.checkOvertime.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkOvertime.Location = new System.Drawing.Point(12, 103);
			this.checkOvertime.Name = "checkOvertime";
			this.checkOvertime.Size = new System.Drawing.Size(139, 17);
			this.checkOvertime.TabIndex = 5;
			this.checkOvertime.Text = "Overtime Adjustment";
			this.checkOvertime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkOvertime.UseVisualStyleBackColor = true;
			// 
			// textHours
			// 
			this.textHours.Location = new System.Drawing.Point(137, 77);
			this.textHours.Name = "textHours";
			this.textHours.Size = new System.Drawing.Size(68, 20);
			this.textHours.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(152, 101);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(300, 18);
			this.label3.TabIndex = 0;
			this.label3.Text = "(the hours will be shifted from regular time to overtime)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(37, 269);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(79, 24);
			this.butDelete.TabIndex = 90;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(358, 269);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 98;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(439, 269);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 99;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// radioAuto
			// 
			this.radioAuto.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioAuto.Location = new System.Drawing.Point(12, 10);
			this.radioAuto.Name = "radioAuto";
			this.radioAuto.Size = new System.Drawing.Size(139, 18);
			this.radioAuto.TabIndex = 1;
			this.radioAuto.TabStop = true;
			this.radioAuto.Text = "Automatically entered";
			this.radioAuto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioAuto.UseVisualStyleBackColor = true;
			// 
			// radioManual
			// 
			this.radioManual.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioManual.Location = new System.Drawing.Point(12, 27);
			this.radioManual.Name = "radioManual";
			this.radioManual.Size = new System.Drawing.Size(139, 18);
			this.radioManual.TabIndex = 2;
			this.radioManual.TabStop = true;
			this.radioManual.Text = "Manually entered";
			this.radioManual.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioManual.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(152, 27);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(170, 18);
			this.label5.TabIndex = 0;
			this.label5.Text = "(protected from auto deletion)";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboPTO
			// 
			this.comboPTO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPTO.FormattingEnabled = true;
			this.comboPTO.Location = new System.Drawing.Point(137, 123);
			this.comboPTO.Name = "comboPTO";
			this.comboPTO.Size = new System.Drawing.Size(121, 21);
			this.comboPTO.TabIndex = 6;
			// 
			// labelPTO
			// 
			this.labelPTO.Location = new System.Drawing.Point(11, 122);
			this.labelPTO.Name = "labelPTO";
			this.labelPTO.Size = new System.Drawing.Size(126, 20);
			this.labelPTO.TabIndex = 0;
			this.labelPTO.Text = "PTO Type";
			this.labelPTO.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormTimeAdjustEdit
			// 
			this.ClientSize = new System.Drawing.Size(540, 313);
			this.Controls.Add(this.labelPTO);
			this.Controls.Add(this.comboPTO);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.radioManual);
			this.Controls.Add(this.radioAuto);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textHours);
			this.Controls.Add(this.checkOvertime);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textTimeEntry);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormTimeAdjustEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Time Adjustment";
			this.Load += new System.EventHandler(this.FormTimeAdjustEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormTimeAdjustEdit_Load(object sender, System.EventArgs e) {
			if(TimeAdjustCur.IsAuto) {
				radioAuto.Checked=true;
			}
			else {
				radioManual.Checked=true;
			}
			textTimeEntry.Text=TimeAdjustCur.TimeEntry.ToString();
			if(TimeAdjustCur.OTimeHours.TotalHours==0){
				if(TimeAdjustCur.PtoDefNum==0) {
					textHours.Text=ClockEvents.Format(TimeAdjustCur.RegHours);
				}
				else {//Is PTO
					textHours.Text=ClockEvents.Format(TimeAdjustCur.PtoHours);
				}
			}
			else{
				checkOvertime.Checked=true;
				textHours.Text=ClockEvents.Format(TimeAdjustCur.OTimeHours);
			}
			textNote.Text=TimeAdjustCur.Note;
			comboPTO.Items.Clear();
			comboPTO.Items.Add(Lan.g(this,"None"));
			comboPTO.SelectedIndex=0;
			List<Def> listPtoTypes=Defs.GetDefsForCategory(DefCat.TimeCardAdjTypes).OrderBy(x => x.ItemName).ToList();
			foreach(Def def in listPtoTypes) {
				if(def.IsHidden && def.DefNum==TimeAdjustCur.PtoDefNum) {
					comboPTO.Items.Add(new ODBoxItem<Def>(def.ItemName+" "+Lan.g(this,"(hidden)"),def));
				}
				else if(!def.IsHidden) {
					comboPTO.Items.Add(new ODBoxItem<Def>(def.ItemName,def));
				}
				if(def.DefNum==TimeAdjustCur.PtoDefNum) {
					comboPTO.SelectedIndex=comboPTO.Items.Count-1;
				}
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew){
				DialogResult=DialogResult.Cancel;
				return;
			}
			TimeAdjusts.Delete(TimeAdjustCur);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			try {
				DateTime.Parse(textTimeEntry.Text);
			}
			catch {
				MsgBox.Show(this,"Please enter a valid Date/Time.");
				return;
			}
			TimeSpan hoursEntered=TimeSpan.FromHours(0);
			try {
				if(textHours.Text.Contains(":")) {
					hoursEntered=ClockEvents.ParseHours(textHours.Text);
				}
				else {
					hoursEntered=TimeSpan.FromHours(Double.Parse(textHours.Text));
				}
				if(hoursEntered==TimeSpan.FromHours(0)) {
					throw new ApplicationException("Invalid hoursEntered");
				}
			}
			catch {
				MsgBox.Show(this,"Please enter valid Hours and Minutes.");
				return;
			}
			if(checkOvertime.Checked && comboPTO.SelectedIndex!=0) {
				MsgBox.Show(this,"Overtime Adjustments must have PTO Type set to 'None'.\r\n"
					+"Please select 'None' for PTO Type or uncheck Overtime Adjustment.");
				return;
			}
			//end of validation
			TimeAdjustCur.IsAuto=radioAuto.Checked;
			TimeAdjustCur.TimeEntry=DateTime.Parse(textTimeEntry.Text);
			if(checkOvertime.Checked){
				TimeAdjustCur.RegHours=-hoursEntered;
				TimeAdjustCur.OTimeHours=hoursEntered;
				TimeAdjustCur.PtoHours=TimeSpan.FromHours(0);
				TimeAdjustCur.PtoDefNum=0;
			}
			else if(comboPTO.SelectedIndex==0) {
				TimeAdjustCur.RegHours=hoursEntered;
				TimeAdjustCur.OTimeHours=TimeSpan.FromHours(0);
				TimeAdjustCur.PtoHours=TimeSpan.FromHours(0);
				TimeAdjustCur.PtoDefNum=0;
			}
			else {//Is PTO
				ODBoxItem<Def> item=(ODBoxItem<Def>)comboPTO.Items[comboPTO.SelectedIndex];
				TimeAdjustCur.RegHours=TimeSpan.FromHours(0);
				TimeAdjustCur.OTimeHours=TimeSpan.FromHours(0);
				TimeAdjustCur.PtoHours=hoursEntered;
				TimeAdjustCur.PtoDefNum=((Def)item.Tag).DefNum;
			}
			TimeAdjustCur.Note=textNote.Text;
			if(IsNew){
				TimeAdjusts.Insert(TimeAdjustCur);
			}
			else{
				TimeAdjusts.Update(TimeAdjustCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	

	

		

		

		


	}
}





















