using System;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormBlockoutCutCopyPaste:ODForm {
		private GroupBox groupBox1;
		private OpenDental.UI.Button butCopyWeek;
		private OpenDental.UI.Button butCopyDay;
		private GroupBox groupBox2;
		private OpenDental.UI.Button butRepeat;
		private Label label4;
		private CheckBox checkReplace;
		private TextBox textRepeat;
		private OpenDental.UI.Button butPaste;
		private CheckBox checkWeekend;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private TextBox textClipboard;
		private Label label3;
		private Label label1;
		private static DateTime DateCopyStart=DateTime.MinValue;
		private static DateTime DateCopyEnd=DateTime.MinValue;
		public long ApptViewNumCur;
		private static long ApptViewNumPrevious;
		private OpenDental.UI.Button butClearDay;
		public DateTime DateSelected;

		private bool _isWeekend {
			get {
				return DateSelected.DayOfWeek==DayOfWeek.Saturday || DateSelected.DayOfWeek==DayOfWeek.Sunday;
			}
		}

		///<summary></summary>
		public FormBlockoutCutCopyPaste()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBlockoutCutCopyPaste));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textClipboard = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.butCopyWeek = new OpenDental.UI.Button();
			this.butCopyDay = new OpenDental.UI.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butRepeat = new OpenDental.UI.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.checkReplace = new System.Windows.Forms.CheckBox();
			this.textRepeat = new System.Windows.Forms.TextBox();
			this.butPaste = new OpenDental.UI.Button();
			this.checkWeekend = new System.Windows.Forms.CheckBox();
			this.butClearDay = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textClipboard);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.butCopyWeek);
			this.groupBox1.Controls.Add(this.butCopyDay);
			this.groupBox1.Location = new System.Drawing.Point(26, 50);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(158, 198);
			this.groupBox1.TabIndex = 40;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Copy";
			// 
			// textClipboard
			// 
			this.textClipboard.Location = new System.Drawing.Point(6, 113);
			this.textClipboard.Name = "textClipboard";
			this.textClipboard.ReadOnly = true;
			this.textClipboard.Size = new System.Drawing.Size(146, 20);
			this.textClipboard.TabIndex = 30;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(149, 80);
			this.label1.TabIndex = 47;
			this.label1.Text = "Copying only applies to the visible operatories for the current appointment view." +
    " It also does not copy to a different operatory.";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 96);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(146, 14);
			this.label3.TabIndex = 29;
			this.label3.Text = "Clipboard Contents";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butCopyWeek
			// 
			this.butCopyWeek.Location = new System.Drawing.Point(6, 165);
			this.butCopyWeek.Name = "butCopyWeek";
			this.butCopyWeek.Size = new System.Drawing.Size(75, 24);
			this.butCopyWeek.TabIndex = 28;
			this.butCopyWeek.Text = "Copy Week";
			this.butCopyWeek.Click += new System.EventHandler(this.butCopyWeek_Click);
			// 
			// butCopyDay
			// 
			this.butCopyDay.Location = new System.Drawing.Point(6, 138);
			this.butCopyDay.Name = "butCopyDay";
			this.butCopyDay.Size = new System.Drawing.Size(75, 24);
			this.butCopyDay.TabIndex = 27;
			this.butCopyDay.Text = "Copy Day";
			this.butCopyDay.Click += new System.EventHandler(this.butCopyDay_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.butRepeat);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.checkReplace);
			this.groupBox2.Controls.Add(this.textRepeat);
			this.groupBox2.Controls.Add(this.butPaste);
			this.groupBox2.Location = new System.Drawing.Point(26, 263);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(158, 97);
			this.groupBox2.TabIndex = 45;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Paste";
			// 
			// butRepeat
			// 
			this.butRepeat.Location = new System.Drawing.Point(6, 64);
			this.butRepeat.Name = "butRepeat";
			this.butRepeat.Size = new System.Drawing.Size(75, 24);
			this.butRepeat.TabIndex = 30;
			this.butRepeat.Text = "Repeat";
			this.butRepeat.Click += new System.EventHandler(this.butRepeat_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(70, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(37, 14);
			this.label4.TabIndex = 32;
			this.label4.Text = "#";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// checkReplace
			// 
			this.checkReplace.Checked = true;
			this.checkReplace.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkReplace.Location = new System.Drawing.Point(6, 14);
			this.checkReplace.Name = "checkReplace";
			this.checkReplace.Size = new System.Drawing.Size(146, 18);
			this.checkReplace.TabIndex = 31;
			this.checkReplace.Text = "Replace Existing";
			this.checkReplace.UseVisualStyleBackColor = true;
			// 
			// textRepeat
			// 
			this.textRepeat.Location = new System.Drawing.Point(110, 67);
			this.textRepeat.Name = "textRepeat";
			this.textRepeat.Size = new System.Drawing.Size(39, 20);
			this.textRepeat.TabIndex = 31;
			this.textRepeat.Text = "1";
			// 
			// butPaste
			// 
			this.butPaste.Location = new System.Drawing.Point(6, 37);
			this.butPaste.Name = "butPaste";
			this.butPaste.Size = new System.Drawing.Size(75, 24);
			this.butPaste.TabIndex = 29;
			this.butPaste.Text = "Paste";
			this.butPaste.Click += new System.EventHandler(this.butPaste_Click);
			// 
			// checkWeekend
			// 
			this.checkWeekend.Location = new System.Drawing.Point(123, 16);
			this.checkWeekend.Name = "checkWeekend";
			this.checkWeekend.Size = new System.Drawing.Size(143, 18);
			this.checkWeekend.TabIndex = 46;
			this.checkWeekend.Text = "Include Weekends";
			this.checkWeekend.UseVisualStyleBackColor = true;
			// 
			// butClearDay
			// 
			this.butClearDay.Location = new System.Drawing.Point(32, 12);
			this.butClearDay.Name = "butClearDay";
			this.butClearDay.Size = new System.Drawing.Size(75, 24);
			this.butClearDay.TabIndex = 48;
			this.butClearDay.Text = "Clear Day";
			this.butClearDay.Click += new System.EventHandler(this.butClearDay_Click);
			// 
			// FormBlockoutCutCopyPaste
			// 
			this.ClientSize = new System.Drawing.Size(290, 383);
			this.Controls.Add(this.butClearDay);
			this.Controls.Add(this.checkWeekend);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormBlockoutCutCopyPaste";
			this.ShowInTaskbar = false;
			this.Text = "Blockout Cut-Copy-Paste";
			this.Load += new System.EventHandler(this.FormBlockoutCutCopyPaste_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormBlockoutCutCopyPaste_Load(object sender,EventArgs e) {
			if(_isWeekend) {
				checkWeekend.Checked=true;
			}			
			if(ApptViewNumCur!=ApptViewNumPrevious){
				DateCopyStart=DateTime.MinValue;
				DateCopyEnd=DateTime.MinValue;
			}
			FillClipboard();
			ApptViewNumPrevious=ApptViewNumCur;//remember the appt view for next time.
		}

		private void butClearDay_Click(object sender,EventArgs e) {
			if(PrefC.HasClinicsEnabled) {
				string clincAbbr=(Clinics.ClinicNum==0?Lan.g(this,"Headquarters"):Clinics.GetAbbr(Clinics.ClinicNum));
				if(MessageBox.Show(Lan.g(this,"Clear all blockouts for day for clinic: ")+clincAbbr+Lan.g(this,"?")+"\r\n"
					+Lan.g(this,"(This may include blockouts not shown in the current appointment view)")
					,Lan.g(this,"Clear Blockouts"),MessageBoxButtons.OKCancel)!=DialogResult.OK) 
				{ 
					return;
				}
				Schedules.ClearBlockoutsForClinic(Clinics.ClinicNum,DateSelected);//currently selected clinic only, works for daily or weekly
				Schedules.BlockoutLogHelper(BlockoutAction.Clear,dateTime:DateSelected,clinicNum:Clinics.ClinicNum);
			}
			else {
				if(!MsgBox.Show(this,true,"Clear all blockouts for day? (This may include blockouts not shown in the current appointment view)")) {
					return;
				}
				Schedules.ClearBlockoutsForDay(DateSelected);//works for daily or weekly
				Schedules.BlockoutLogHelper(BlockoutAction.Clear,dateTime:DateSelected);
			}
			Close();
		}

		private void FillClipboard(){
			if(DateCopyStart.Year<1880){
				textClipboard.Text="";
			}
			else if(DateCopyStart==DateCopyEnd) {
				textClipboard.Text=DateCopyStart.ToShortDateString();
			}
			else {
				textClipboard.Text=DateCopyStart.ToShortDateString()+"-"+DateCopyEnd.ToShortDateString();
			}
		}

		private void butCopyDay_Click(object sender,EventArgs e) {
			DateCopyStart=DateSelected;
			DateCopyEnd=DateSelected;
			Close();
		}

		private void butCopyWeek_Click(object sender,EventArgs e) {
			//Always start week on Monday
			if(DateSelected.DayOfWeek==DayOfWeek.Sunday) {//if selecting Sunday, go back to the previous Monday.
				DateCopyStart=DateSelected.AddDays(-6);
			}
			else {//Any other day. eg Wed.AddDays(1-3)=Wed.AddDays(-2)=Monday
				DateCopyStart=DateSelected.AddDays(1-(int)DateSelected.DayOfWeek);//eg Wed.AddDays(1-3)=Wed.AddDays(-2)=Monday
			}
			if(checkWeekend.Checked){
				DateCopyEnd=DateCopyStart.AddDays(6);
			}
			else{
				DateCopyEnd=DateCopyStart.AddDays(4);
			}
			Close();
		}

		private void butPaste_Click(object sender,EventArgs e) {
			CopyOverBlockouts(1);
		}

		private void butRepeat_Click(object sender,EventArgs e) {
			try {
				int.Parse(textRepeat.Text);
			}
			catch {
				MsgBox.Show(this,"Please fix number box first.");
				return;
			}
			CopyOverBlockouts(PIn.Int(textRepeat.Text));
		}

		private void CopyOverBlockouts(int numRepeat) {
			if(DateCopyStart.Year < 1880) {
				MsgBox.Show(this,"Please copy a selection to the clipboard first.");
				return;
			}
			if(_isWeekend && !checkWeekend.Checked) {//user is trying to 'paste' onto a weekend date
				MsgBox.Show(this,"You must check 'Include Weekends' if you would like to paste into weekends.");
				return;
			}
			//calculate which day or week is currently selected.
			DateTime dateSelectedStart;
			DateTime dateSelectedEnd;
			bool isWeek=DateCopyStart!=DateCopyEnd;
			if(isWeek) {
				//Always start week on Monday
				if(DateSelected.DayOfWeek==DayOfWeek.Sunday) {//if selecting Sunday, go back to the previous Monday.
					dateSelectedStart=DateSelected.AddDays(-6);
				}
				else {//Any other day. eg Wed.AddDays(1-3)=Wed.AddDays(-2)=Monday
					dateSelectedStart=DateSelected.AddDays(1-(int)DateSelected.DayOfWeek);//eg Wed.AddDays(1-3)=Wed.AddDays(-2)=Monday
				}
				//DateCopyEnd is greater than DateCopyStart and is either 4 days greater or 6 days greater, so clear/paste the same number of days
				dateSelectedEnd=dateSelectedStart.AddDays((DateCopyEnd-DateCopyStart).Days);
			}
			else {
				dateSelectedStart=DateSelected;
				dateSelectedEnd=DateSelected;
			}
			//When pasting, it's not allowed to paste back over the same day or week.
			if(dateSelectedStart==DateCopyStart && numRepeat==1) {
				MsgBox.Show(this,"Not allowed to paste back onto the same date as is on the clipboard.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			string errors=Schedules.CopyBlockouts(ApptViewNumCur,isWeek,checkWeekend.Checked,checkReplace.Checked,DateCopyStart,DateCopyEnd,
				dateSelectedStart,dateSelectedEnd,numRepeat);
			Cursor=Cursors.Default;
			if(!string.IsNullOrEmpty(errors)) {
				MessageBox.Show(errors);//Error was translated inside of the S class method.
				return;
			}
			Close();
		}
	}
}