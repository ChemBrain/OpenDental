using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormSchedule:ODForm {
		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.Button butRefresh;
		private ValidDate textDateTo;
		private Label label2;
		private ValidDate textDateFrom;
		private Label label1;
		private ListBox listBoxProvs;
		private CheckBox checkWeekend;
		private OpenDental.UI.Button butPrint;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private DateTime _dateCopyStart;
		private OpenDental.UI.Button butDelete;
		private DateTime _dateCopyEnd;
		private CheckBox checkPracticeNotes;
		private ListBox listBoxEmps;
		///<summary>This tracks whether the provList or empList has been click on since the last refresh.  
		///Forces user to refresh before deleting or pasting so that the list exactly matches the grid.</summary>
		private bool _provsChanged;
		private bool headingPrinted;
		private int pagesPrinted;
		private int headingPrintH;
		bool changed;
		private List<Provider> _listProviders;
		private TabControl tabControl1;
		private TabPage tabPageProv;
		private TabPage tabPageEmp;
		private CheckBox checkClinicNotes;
		private List<Employee> _listEmps;
		private DataTable _tableScheds;
		private UI.ComboBoxClinicPicker comboClinic;
		private bool _isResizing;
		private CheckBox checkShowClinicSchedules;
		private Point _clickedCell;
		///<summary>By default is FormScheduleMode.SetupSchedule.
		///If user does not have Schedule permission then will be in FormScheduleMode.ViewSchedule.</summary>
		private FormScheduleMode _formMode;
		private List<long> _listPreSelectedEmpNums;
		private CheckBox checkWarnProvOverlap;
		private GroupBox groupCopy;
		private UI.Button butCopyWeek;
		private UI.Button butCopyDay;
		private TextBox textClipboard;
		private Label label3;
		private GroupBox groupPaste;
		private UI.Button butRepeat;
		private Label label4;
		private CheckBox checkReplace;
		private TextBox textRepeat;
		private UI.Button butPaste;
		private List<long> _listPreSelectedProvNums;
		///<summary>Keeps track of the last time the "From Date" was set via the fill grid.
		///Used to determine if the user has changed the date since last fill grid.</summary>
		private DateTime _fromDateCur;
		///<summary>Keeps track of the last time the "To Date" was set via the fill grid.
		///Used to determine if the user has changed the date since last fill grid.</summary>
		private DateTime _toDateCur;

		///<summary>Checks if dates in textbox match current dates stored.</summary>
		private bool HaveDatesChanged {
			get {
				if(_fromDateCur!=PIn.Date(textDateFrom.Text) || _toDateCur!=PIn.Date(textDateTo.Text)) {
					return true;
				}
				return false;
			}
		}

		///<summary></summary>
		public FormSchedule(List<long> listPreSelectedEmpNums=null,List<long> listPreSelectedProvNums=null)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			_listPreSelectedEmpNums=listPreSelectedEmpNums;
			_listPreSelectedProvNums=listPreSelectedProvNums;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSchedule));
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.listBoxProvs = new System.Windows.Forms.ListBox();
			this.checkWeekend = new System.Windows.Forms.CheckBox();
			this.checkWarnProvOverlap = new System.Windows.Forms.CheckBox();
			this.checkPracticeNotes = new System.Windows.Forms.CheckBox();
			this.listBoxEmps = new System.Windows.Forms.ListBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPageProv = new System.Windows.Forms.TabPage();
			this.tabPageEmp = new System.Windows.Forms.TabPage();
			this.checkClinicNotes = new System.Windows.Forms.CheckBox();
			this.butDelete = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.textDateTo = new OpenDental.ValidDate();
			this.textDateFrom = new OpenDental.ValidDate();
			this.butRefresh = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.comboClinic = new OpenDental.UI.ComboBoxClinicPicker();
			this.checkShowClinicSchedules = new System.Windows.Forms.CheckBox();
			this.groupCopy = new System.Windows.Forms.GroupBox();
			this.butCopyWeek = new OpenDental.UI.Button();
			this.butCopyDay = new OpenDental.UI.Button();
			this.textClipboard = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupPaste = new System.Windows.Forms.GroupBox();
			this.butRepeat = new OpenDental.UI.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.checkReplace = new System.Windows.Forms.CheckBox();
			this.textRepeat = new System.Windows.Forms.TextBox();
			this.butPaste = new OpenDental.UI.Button();
			this.tabControl1.SuspendLayout();
			this.tabPageProv.SuspendLayout();
			this.tabPageEmp.SuspendLayout();
			this.groupCopy.SuspendLayout();
			this.groupPaste.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(100, 38);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(87, 15);
			this.label2.TabIndex = 9;
			this.label2.Text = "To Date";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 38);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 15);
			this.label1.TabIndex = 7;
			this.label1.Text = "From Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listBoxProvs
			// 
			this.listBoxProvs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBoxProvs.Location = new System.Drawing.Point(3, 3);
			this.listBoxProvs.Name = "listBoxProvs";
			this.listBoxProvs.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxProvs.Size = new System.Drawing.Size(186, 213);
			this.listBoxProvs.TabIndex = 23;
			this.listBoxProvs.Click += new System.EventHandler(this.listProv_Click);
			this.listBoxProvs.SelectedIndexChanged += new System.EventHandler(this.listProv_SelectedIndexChanged);
			// 
			// checkWeekend
			// 
			this.checkWeekend.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkWeekend.Location = new System.Drawing.Point(50, 422);
			this.checkWeekend.Name = "checkWeekend";
			this.checkWeekend.Size = new System.Drawing.Size(143, 17);
			this.checkWeekend.TabIndex = 24;
			this.checkWeekend.Text = "Show Weekends";
			this.checkWeekend.UseVisualStyleBackColor = true;
			this.checkWeekend.Click += new System.EventHandler(this.checkWeekend_Click);
			this.checkWeekend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.checkWeekend_KeyDown);
			// 
			// checkWarnProvOverlap
			// 
			this.checkWarnProvOverlap.Checked = true;
			this.checkWarnProvOverlap.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkWarnProvOverlap.Location = new System.Drawing.Point(6, 14);
			this.checkWarnProvOverlap.Name = "checkWarnProvOverlap";
			this.checkWarnProvOverlap.Size = new System.Drawing.Size(176, 18);
			this.checkWarnProvOverlap.TabIndex = 33;
			this.checkWarnProvOverlap.Text = "Warn on Provider Overlap";
			this.checkWarnProvOverlap.UseVisualStyleBackColor = true;
			// 
			// checkPracticeNotes
			// 
			this.checkPracticeNotes.Checked = true;
			this.checkPracticeNotes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkPracticeNotes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPracticeNotes.Location = new System.Drawing.Point(12, 74);
			this.checkPracticeNotes.Name = "checkPracticeNotes";
			this.checkPracticeNotes.Size = new System.Drawing.Size(189, 17);
			this.checkPracticeNotes.TabIndex = 28;
			this.checkPracticeNotes.Text = "Show Practice Holidays and Notes";
			this.checkPracticeNotes.UseVisualStyleBackColor = true;
			this.checkPracticeNotes.Click += new System.EventHandler(this.checkPracticeNotes_Click);
			// 
			// listBoxEmps
			// 
			this.listBoxEmps.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBoxEmps.Location = new System.Drawing.Point(3, 3);
			this.listBoxEmps.Name = "listBoxEmps";
			this.listBoxEmps.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxEmps.Size = new System.Drawing.Size(186, 213);
			this.listBoxEmps.TabIndex = 30;
			this.listBoxEmps.Click += new System.EventHandler(this.listEmp_Click);
			this.listBoxEmps.SelectedIndexChanged += new System.EventHandler(this.listEmp_SelectedIndexChanged);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPageProv);
			this.tabControl1.Controls.Add(this.tabPageEmp);
			this.tabControl1.Location = new System.Drawing.Point(1, 163);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(200, 245);
			this.tabControl1.TabIndex = 36;
			// 
			// tabPageProv
			// 
			this.tabPageProv.Controls.Add(this.listBoxProvs);
			this.tabPageProv.Location = new System.Drawing.Point(4, 22);
			this.tabPageProv.Name = "tabPageProv";
			this.tabPageProv.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageProv.Size = new System.Drawing.Size(192, 219);
			this.tabPageProv.TabIndex = 0;
			this.tabPageProv.Text = "Providers (0)";
			this.tabPageProv.UseVisualStyleBackColor = true;
			// 
			// tabPageEmp
			// 
			this.tabPageEmp.Controls.Add(this.listBoxEmps);
			this.tabPageEmp.Location = new System.Drawing.Point(4, 22);
			this.tabPageEmp.Name = "tabPageEmp";
			this.tabPageEmp.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageEmp.Size = new System.Drawing.Size(192, 219);
			this.tabPageEmp.TabIndex = 1;
			this.tabPageEmp.Text = "Employees (0)";
			this.tabPageEmp.UseVisualStyleBackColor = true;
			// 
			// checkClinicNotes
			// 
			this.checkClinicNotes.Checked = true;
			this.checkClinicNotes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkClinicNotes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClinicNotes.Location = new System.Drawing.Point(12, 94);
			this.checkClinicNotes.Name = "checkClinicNotes";
			this.checkClinicNotes.Size = new System.Drawing.Size(189, 17);
			this.checkClinicNotes.TabIndex = 37;
			this.checkClinicNotes.Text = "Show Clinic Holidays and Notes";
			this.checkClinicNotes.UseVisualStyleBackColor = true;
			this.checkClinicNotes.Click += new System.EventHandler(this.checkClinicNotes_Click);
			// 
			// butDelete
			// 
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(50, 441);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(103, 24);
			this.butDelete.TabIndex = 27;
			this.butDelete.Text = "Clear Week";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butPrint
			// 
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrint;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(551, 666);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(90, 24);
			this.butPrint.TabIndex = 26;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(102, 54);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(85, 20);
			this.textDateTo.TabIndex = 10;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(12, 54);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(85, 20);
			this.textDateFrom.TabIndex = 8;
			// 
			// butRefresh
			// 
			this.butRefresh.Location = new System.Drawing.Point(57, 8);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 24);
			this.butRefresh.TabIndex = 11;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(207, 8);
			this.gridMain.Name = "gridMain";
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridMain.Size = new System.Drawing.Size(761, 652);
			this.gridMain.TabIndex = 0;
			this.gridMain.Title = "Schedule";
			this.gridMain.TranslationName = "TableSchedule";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// comboClinic
			// 
			this.comboClinic.HqDescription = "Headquarters";
			this.comboClinic.IncludeUnassigned = true;
			this.comboClinic.Location = new System.Drawing.Point(8, 117);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(189, 21);
			this.comboClinic.TabIndex = 35;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// checkShowClinicSchedules
			// 
			this.checkShowClinicSchedules.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowClinicSchedules.Location = new System.Drawing.Point(12, 142);
			this.checkShowClinicSchedules.Name = "checkShowClinicSchedules";
			this.checkShowClinicSchedules.Size = new System.Drawing.Size(189, 17);
			this.checkShowClinicSchedules.TabIndex = 38;
			this.checkShowClinicSchedules.Text = "Limit to Ops in Clinic";
			this.checkShowClinicSchedules.UseVisualStyleBackColor = true;
			this.checkShowClinicSchedules.CheckedChanged += new System.EventHandler(this.checkShowClinicSchedules_CheckedChanged);
			// 
			// groupCopy
			// 
			this.groupCopy.Controls.Add(this.butCopyWeek);
			this.groupCopy.Controls.Add(this.butCopyDay);
			this.groupCopy.Controls.Add(this.textClipboard);
			this.groupCopy.Controls.Add(this.label3);
			this.groupCopy.Location = new System.Drawing.Point(5, 468);
			this.groupCopy.Name = "groupCopy";
			this.groupCopy.Size = new System.Drawing.Size(192, 113);
			this.groupCopy.TabIndex = 39;
			this.groupCopy.TabStop = false;
			this.groupCopy.Text = "Copy";
			// 
			// butCopyWeek
			// 
			this.butCopyWeek.Location = new System.Drawing.Point(6, 83);
			this.butCopyWeek.Name = "butCopyWeek";
			this.butCopyWeek.Size = new System.Drawing.Size(75, 24);
			this.butCopyWeek.TabIndex = 28;
			this.butCopyWeek.Text = "Copy Week";
			this.butCopyWeek.Click += new System.EventHandler(this.butCopyWeek_Click);
			// 
			// butCopyDay
			// 
			this.butCopyDay.Location = new System.Drawing.Point(6, 56);
			this.butCopyDay.Name = "butCopyDay";
			this.butCopyDay.Size = new System.Drawing.Size(75, 24);
			this.butCopyDay.TabIndex = 27;
			this.butCopyDay.Text = "Copy Day";
			this.butCopyDay.Click += new System.EventHandler(this.butCopyDay_Click);
			// 
			// textClipboard
			// 
			this.textClipboard.Location = new System.Drawing.Point(6, 33);
			this.textClipboard.Name = "textClipboard";
			this.textClipboard.ReadOnly = true;
			this.textClipboard.Size = new System.Drawing.Size(176, 20);
			this.textClipboard.TabIndex = 26;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(146, 14);
			this.label3.TabIndex = 8;
			this.label3.Text = "Clipboard Contents";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupPaste
			// 
			this.groupPaste.Controls.Add(this.butRepeat);
			this.groupPaste.Controls.Add(this.label4);
			this.groupPaste.Controls.Add(this.checkWarnProvOverlap);
			this.groupPaste.Controls.Add(this.checkReplace);
			this.groupPaste.Controls.Add(this.textRepeat);
			this.groupPaste.Controls.Add(this.butPaste);
			this.groupPaste.Location = new System.Drawing.Point(5, 581);
			this.groupPaste.Name = "groupPaste";
			this.groupPaste.Size = new System.Drawing.Size(192, 109);
			this.groupPaste.TabIndex = 40;
			this.groupPaste.TabStop = false;
			this.groupPaste.Text = "Paste";
			// 
			// butRepeat
			// 
			this.butRepeat.Location = new System.Drawing.Point(6, 79);
			this.butRepeat.Name = "butRepeat";
			this.butRepeat.Size = new System.Drawing.Size(75, 24);
			this.butRepeat.TabIndex = 30;
			this.butRepeat.Text = "Repeat";
			this.butRepeat.Click += new System.EventHandler(this.butRepeat_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(70, 85);
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
			this.checkReplace.Location = new System.Drawing.Point(6, 34);
			this.checkReplace.Name = "checkReplace";
			this.checkReplace.Size = new System.Drawing.Size(146, 18);
			this.checkReplace.TabIndex = 31;
			this.checkReplace.Text = "Replace Existing";
			this.checkReplace.UseVisualStyleBackColor = true;
			// 
			// textRepeat
			// 
			this.textRepeat.Location = new System.Drawing.Point(110, 82);
			this.textRepeat.Name = "textRepeat";
			this.textRepeat.Size = new System.Drawing.Size(39, 20);
			this.textRepeat.TabIndex = 31;
			this.textRepeat.Text = "1";
			// 
			// butPaste
			// 
			this.butPaste.Location = new System.Drawing.Point(6, 52);
			this.butPaste.Name = "butPaste";
			this.butPaste.Size = new System.Drawing.Size(75, 24);
			this.butPaste.TabIndex = 29;
			this.butPaste.Text = "Paste";
			this.butPaste.Click += new System.EventHandler(this.butPaste_Click);
			// 
			// FormSchedule
			// 
			this.ClientSize = new System.Drawing.Size(974, 695);
			this.Controls.Add(this.groupCopy);
			this.Controls.Add(this.groupPaste);
			this.Controls.Add(this.checkShowClinicSchedules);
			this.Controls.Add(this.checkClinicNotes);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.checkPracticeNotes);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.checkWeekend);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(647, 727);
			this.Name = "FormSchedule";
			this.ShowInTaskbar = false;
			this.Text = "Schedule";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSchedule_FormClosing);
			this.Load += new System.EventHandler(this.FormSchedule_Load);
			this.ResizeBegin += new System.EventHandler(this.FormSchedule_ResizeBegin);
			this.ResizeEnd += new System.EventHandler(this.FormSchedule_ResizeEnd);
			this.Resize += new System.EventHandler(this.FormSchedule_Resize);
			this.tabControl1.ResumeLayout(false);
			this.tabPageProv.ResumeLayout(false);
			this.tabPageEmp.ResumeLayout(false);
			this.groupCopy.ResumeLayout(false);
			this.groupCopy.PerformLayout();
			this.groupPaste.ResumeLayout(false);
			this.groupPaste.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormSchedule_Load(object sender,EventArgs e) {
			_formMode=FormScheduleMode.ViewSchedule;
			if(Security.IsAuthorized(Permissions.Schedules,DateTime.MinValue,true)){
				_formMode=FormScheduleMode.SetupSchedule;
			};
			switch(_formMode) {
				case FormScheduleMode.SetupSchedule:
					DateTime dateFrom=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1);
					textDateFrom.Text=dateFrom.ToShortDateString();
					textDateTo.Text=dateFrom.AddMonths(12).AddDays(-1).ToShortDateString();
					break;
				case FormScheduleMode.ViewSchedule:
					butDelete.Visible=false;
					groupCopy.Visible=false;
					groupPaste.Visible=false;
					if(PrefC.GetBool(PrefName.DistributorKey)) {//if this is OD HQ
						checkPracticeNotes.Checked=false;
						checkPracticeNotes.Enabled=false;
					}
					dateFrom=DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);//Sunday of current week.
					textDateFrom.Text=dateFrom.ToShortDateString();
					textDateTo.Text=dateFrom.AddMonths(1).AddDays(-1).ToShortDateString();
					break;
			}
			RefreshClinicData();
			FillEmployeesAndProviders();
			FillGrid();
		}

		private void RefreshClinicData() {
			if(!PrefC.HasClinicsEnabled) {
				checkShowClinicSchedules.Visible=false;
				checkClinicNotes.Visible=false;
				checkClinicNotes.Checked=false;
				return;
			}
		}

		///<summary>Fills the employee box based on what clinic is selected.  Set selectAll to true to have all employees in the list box selected by default.</summary>
		private void FillEmployeesAndProviders() {
			tabPageEmp.Text=Lan.g(this,"Employees")+" (0)";
			tabPageProv.Text=Lan.g(this,"Providers")+" (0)";
			//Seed emp list and prov list with a dummy emp/prov with 'none' for the field that fills the list, FName and Abbr respectively.
			//That way we don't have to add/subtract one in order when selecting from the list based on selected indexes.
			_listEmps=new List<Employee>() { new Employee() { EmployeeNum=0,FName="none" } };
			_listProviders=new List<Provider>() { new Provider() { ProvNum=0,Abbr="none" } };
			if(PrefC.HasClinicsEnabled) {
				//clinicNum will be 0 for unrestricted users with HQ selected in which case this will get only emps/provs not assigned to a clinic
				_listEmps.AddRange(Employees.GetEmpsForClinic(comboClinic.SelectedClinicNum));
				_listProviders.AddRange(Providers.GetProvsForClinic(comboClinic.SelectedClinicNum));
			}
			else {//Not using clinics
				_listEmps.AddRange(Employees.GetDeepCopy(true));
				_listProviders.AddRange(Providers.GetDeepCopy(true));
			}
			List<long> listPreviouslySelectedEmpNums=listBoxEmps.GetListSelected<Employee>().Select(x => x.EmployeeNum).ToList();
			listBoxEmps.Items.Clear();
			_listEmps.ForEach(x => listBoxEmps.Items.Add(new ODBoxItem<Employee>(x.FName,x)));
			List<long> listPreviouslySelectedProvNums=listBoxProvs.GetListSelected<Provider>().Select(x => x.ProvNum).ToList();
			listBoxProvs.Items.Clear();
			_listProviders.ForEach(x => listBoxProvs.Items.Add(new ODBoxItem<Provider>(x.Abbr,x)));
			if(_listPreSelectedEmpNums!=null || _listPreSelectedProvNums!=null) {
				if(_listPreSelectedEmpNums!=null && _listPreSelectedEmpNums.Count>0) {
					//Employee Listbox
					for(int i=1;i<listBoxEmps.Items.Count;i++) {
						if(!_listPreSelectedEmpNums.Contains(_listEmps[i].EmployeeNum)) {
							continue;
						}
						listBoxEmps.SetSelected(i,true);
					}
				}
				else {
					listBoxEmps.SelectedIndex=0;//select the 'none' entry;
				}
				if(_listPreSelectedProvNums!=null && _listPreSelectedProvNums.Count>0) {
					//Provider Listbox
					for(int i=1;i<listBoxProvs.Items.Count;i++) {
						if(!_listPreSelectedProvNums.Contains(_listProviders[i].ProvNum)) {
							continue;
						}
						listBoxProvs.SetSelected(i,true);
					}
				}
				else {
					listBoxProvs.SelectedIndex=0;//select the 'none' entry; 
				}
			}
			else if(PrefC.GetBool(PrefName.ScheduleProvEmpSelectAll)) {
				//Employee Listbox
				for(int i=1;i<listBoxEmps.Items.Count;i++) {
					listBoxEmps.SetSelected(i,true);
				}
				//Provider Listbox
				for(int i=1;i<listBoxProvs.Items.Count;i++) {
					listBoxProvs.SetSelected(i,true);
				}
			}
			else {
				if(listPreviouslySelectedEmpNums.Count > 0) {
					listBoxEmps.SetSelectedItem<Employee>(x => listPreviouslySelectedEmpNums.Contains(x.EmployeeNum));
				}
				else {
					listBoxEmps.SelectedIndex=0;//select the 'none' entry;
				}
				if(listPreviouslySelectedProvNums.Count > 0) {
					listBoxProvs.SetSelectedItem<Provider>(x => listPreviouslySelectedProvNums.Contains(x.ProvNum));
				}
				else {
					listBoxProvs.SelectedIndex=0;//select the 'none' entry; 
				}
			}
		}

		///<summary>Returns true if date text boxes have no errors and the emp and prov lists don't have 'none' selected with other emps/provs.
		///Set isQuiet to true to suppress the message box with the warning.</summary>
		private bool ValidateInputs(bool isQuiet=false) {
			List<string> listErrorMsgs=new List<string>();
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!="" || textDateTo.errorProvider1.GetError(textDateTo)!="") {
				listErrorMsgs.Add(Lan.g(this,"Please fix date errors first."));
			}
			if(listBoxProvs.SelectedIndices.Count>1 && listBoxProvs.SelectedIndices.Contains(0)) {//'none' selected with additional provs
				listErrorMsgs.Add(Lan.g(this,"Invalid selection of providers."));
			}
			if(listBoxEmps.SelectedIndices.Count>1 && listBoxEmps.SelectedIndices.Contains(0)) {//'none' selected with additional emps
				listErrorMsgs.Add(Lan.g(this,"Invalid selection of employees."));
			}
			if(listErrorMsgs.Count > 0 && !isQuiet) {
				MessageBox.Show(string.Join("\r\n",listErrorMsgs));
			}
			return (listErrorMsgs.Count==0);
		}

		private void checkWeekend_KeyDown(object sender,KeyEventArgs e) {
#if DEBUG
			if(e.KeyCode==Keys.W) {
#else	
			if(PrefC.IsODHQ && e.KeyCode==Keys.W) {
#endif
				if(checkWeekend.CheckState!=CheckState.Indeterminate) {//Checked or Unchecked
					checkWeekend.Checked=true;
					checkWeekend.CheckState=CheckState.Indeterminate;//Show ONLY the weekend
					butDelete.Visible=false;
					groupCopy.Visible=false;
					groupPaste.Visible=false;
				}
				else {
					checkWeekend.Checked=false;
					checkWeekend.CheckState=CheckState.Unchecked;//Display Normally
					butDelete.Visible=(_formMode==FormScheduleMode.SetupSchedule);
					groupCopy.Visible=(_formMode==FormScheduleMode.SetupSchedule);
					groupPaste.Visible=(_formMode==FormScheduleMode.SetupSchedule);
				}
				FillGrid();
			}
		}

		private void FillGrid(bool doRefreshData=true){
			_dateCopyStart=DateTime.MinValue;
			_dateCopyEnd=DateTime.MinValue;
			textClipboard.Text="";
			if(!ValidateInputs(true)) {
				return;
			}
			//Clear out the columns and rows for dynamic resizing of the grid to calculate column widths
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			gridMain.ListGridRows.Clear();
			gridMain.EndUpdate();
			_provsChanged=false;
			List<long> provNums=new List<long>();
			for(int i=0;i<listBoxProvs.SelectedIndices.Count;i++){
				provNums.Add(_listProviders[listBoxProvs.SelectedIndices[i]].ProvNum);
			}
			List<long> empNums=new List<long>();
			for(int i=0;i<listBoxEmps.SelectedIndices.Count;i++){
				empNums.Add(_listEmps[listBoxEmps.SelectedIndices[i]].EmployeeNum);
			}
			provNums.RemoveAll(x => x==0);
			empNums.RemoveAll(x => x==0);
			if(doRefreshData || this._tableScheds==null) {
				bool canViewNotes=true;
				if(PrefC.IsODHQ) {
					canViewNotes=Security.IsAuthorized(Permissions.Schedules,true);
				}
				_fromDateCur=PIn.Date(textDateFrom.Text);
				_toDateCur=PIn.Date(textDateTo.Text);
				Logger.LogToPath("Schedules.GetPeriod",LogPath.Signals,LogPhase.Start);
				_tableScheds=Schedules.GetPeriod(_fromDateCur,_toDateCur,provNums,empNums,checkPracticeNotes.Checked,
					checkClinicNotes.Checked,comboClinic.SelectedClinicNum,checkShowClinicSchedules.Checked,canViewNotes);
				Logger.LogToPath("Schedules.GetPeriod",LogPath.Signals,LogPhase.End);
			}
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			if(checkWeekend.Checked && checkWeekend.CheckState!=CheckState.Indeterminate) {
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableSchedule","Sunday"),0));
			}
			if(checkWeekend.CheckState!=CheckState.Indeterminate) {
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableSchedule","Monday"),0));
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableSchedule","Tuesday"),0));
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableSchedule","Wednesday"),0));
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableSchedule","Thursday"),0));
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableSchedule","Friday"),0));
			}
			if(checkWeekend.Checked){
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableSchedule","Saturday"),0));
			}
			gridMain.ListGridRows.Clear();
			GridRow row;
			for(int i=0;i<_tableScheds.Rows.Count;i++){
				row=new GridRow();
				if(checkWeekend.Checked && checkWeekend.CheckState!=CheckState.Indeterminate){
					row.Cells.Add(_tableScheds.Rows[i][0].ToString());
				}
				if(checkWeekend.CheckState!=CheckState.Indeterminate) {
					row.Cells.Add(_tableScheds.Rows[i][1].ToString());
					row.Cells.Add(_tableScheds.Rows[i][2].ToString());
					row.Cells.Add(_tableScheds.Rows[i][3].ToString());
					row.Cells.Add(_tableScheds.Rows[i][4].ToString());
					row.Cells.Add(_tableScheds.Rows[i][5].ToString());
				}
				if(checkWeekend.Checked) {
					row.Cells.Add(_tableScheds.Rows[i][6].ToString());
				}
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
			//Set today red
			if(!checkWeekend.Checked && (DateTime.Today.DayOfWeek==DayOfWeek.Sunday || DateTime.Today.DayOfWeek==DayOfWeek.Saturday)){
				return;
			}
			if(DateTime.Today>_toDateCur || DateTime.Today<_fromDateCur){
				return;
			}
			if(checkWeekend.CheckState==CheckState.Indeterminate) {
				return;//don't highlight if we are on the weekend.
			}
			int colI=(int)DateTime.Today.DayOfWeek;
			if(!checkWeekend.Checked){
				colI--;
			}
			gridMain.ListGridRows[Schedules.GetRowCal(_fromDateCur,DateTime.Today)].Cells[colI].ColorText=Color.Red;
			if(_clickedCell!=null //when first opening form
				&& _clickedCell.Y>-1 
				&& _clickedCell.Y< gridMain.ListGridRows.Count
				&& _clickedCell.X>-1
				&& _clickedCell.X<gridMain.ListGridColumns.Count) 
			{
				gridMain.SetSelected(_clickedCell);
			}
			//scroll to cell to keep it in view when editing schedules.
			if(gridMain.SelectedCell.X>-1 && gridMain.SelectedCell.Y>-1) {
				gridMain.ScrollToIndex(gridMain.SelectedCell.Y);
			}
		}

		private void checkShowClinicSchedules_CheckedChanged(object sender,EventArgs e) {
			if(checkShowClinicSchedules.Checked) {
				SelectAllProvsAndEmps();
				butDelete.Enabled=false;
				butCopyDay.Enabled=false;
				butCopyWeek.Enabled=false;
				butPaste.Enabled=false;
				butRepeat.Enabled=false;
			}
			else {
				butDelete.Enabled=true;
				butCopyDay.Enabled=true;
				butCopyWeek.Enabled=true;
				butPaste.Enabled=true;
				butRepeat.Enabled=true;
			}
			if(!ValidateInputs()) {
				return;
			}
			_clickedCell=gridMain.SelectedCell;
			FillGrid();
		}

		private void SelectAllProvsAndEmps() {
			listBoxProvs.ClearSelected();
			for(int i=1;i<listBoxProvs.Items.Count;i++) {//i=1 to skip the none
				listBoxProvs.SetSelected(i,true);
			}
			listBoxEmps.ClearSelected();
			for(int i=1;i<listBoxEmps.Items.Count;i++) {//i=1 to skip the none
				listBoxEmps.SetSelected(i,true);
			}
		}

		///<summary>Helper method because this exact code happens several times in this form.  Returns selected providers, employees, and clinic to the variables supplied.</summary>
		private void GetSelectedProvidersEmployeesAndClinic(out List<long> listProvNums,out List<long> listEmployeeNums) {
			listProvNums=new List<long>();
			//Don't populate listProvNums if 'none' is selected; not allowed to select 'none' and another prov validated above.
			if(!listBoxProvs.SelectedIndices.Contains(0)) {
				listProvNums=listBoxProvs.GetListSelected<Provider>().Select(x => x.ProvNum).ToList();
			}
			listEmployeeNums=new List<long>();
			//Don't populate listEmployeeNums if 'none' is selected; not allowed to select 'none' and another emp validated above.
			if(!listBoxEmps.SelectedIndices.Contains(0)) {
				listEmployeeNums=listBoxEmps.GetListSelected<Employee>().Select(x => x.EmployeeNum).ToList();
			}
		}

		private void listProv_SelectedIndexChanged(object sender,EventArgs e) {
			tabPageProv.Text=Lan.g(this,"Providers")+" ("+listBoxProvs.SelectedIndices.OfType<int>().Count(x => x>0)+")";
		}

		private void listEmp_SelectedIndexChanged(object sender,EventArgs e) {
			tabPageEmp.Text=Lan.g(this,"Employees")+" ("+listBoxEmps.SelectedIndices.OfType<int>().Count(x => x>0)+")";
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			comboClinic.Text=Lan.g(this,"Show Practice Notes");
			if(comboClinic.SelectedClinicNum>0) {
				comboClinic.Text=Lan.g(this,"Show Practice and Clinic Notes");
			}
			FillEmployeesAndProviders();
			if(checkShowClinicSchedules.Checked) {
				SelectAllProvsAndEmps();
			}
			FillGrid();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			if(!ValidateInputs()) {
				return;
			}
			_clickedCell=gridMain.SelectedCell;
			FillGrid();
		}

		private void checkWeekend_Click(object sender,EventArgs e) {
			if(_formMode==FormScheduleMode.SetupSchedule && !butDelete.Visible) {//ODHQ only
				butDelete.Visible=true;
				groupCopy.Visible=true;
				groupPaste.Visible=true;
			}
			//Shift selected cell to account for adding/subtracting weekend columns from gridMain.
			else if(checkWeekend.Checked) {//impossible to already have a weekend day selected, no need to doctor _clickedCell
				_clickedCell=new Point(gridMain.SelectedCell.X+1,gridMain.SelectedCell.Y);//All values shifted right because Sunday was added at begining of row.
			}
			else {//weekend may have been selected, which is now no longer valid.
				_clickedCell=new Point(gridMain.SelectedCell.X-1,gridMain.SelectedCell.Y);
				if(_clickedCell.X==-1 && gridMain.SelectedCell.X==0) {//Sunday WAS selected, reselect Monday
					_clickedCell.X=0;//Monday will be the 0th cell
				}
				if(_clickedCell.X==5 && gridMain.SelectedCell.X==6) {//Saturday WAS selected, reselect Friday
					_clickedCell.X=4;//Friday will be the 4th cell
				}
			}
			//If the _clickedCell above has an X or Y that is -1, because sunday or saturday were selected, it will be handled in fill grid.
			//We will scroll to the same row, but no cells will be selected.
			FillGrid(false);
			if(checkWeekend.Checked && _clickedCell.Y>-1) {
				gridMain.ScrollToIndex(_clickedCell.Y);
			}
		}

		private void checkPracticeNotes_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void checkClinicNotes_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			_clickedCell=gridMain.SelectedCell;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.Schedules,DateTime.MinValue)) {
				return;
			}
			if(!ValidateInputs()) {
				return;
			}
			if(checkShowClinicSchedules.Checked) {
				MsgBox.Show(this,"Schedules cannot be edited in clinic view mode");
				return;
			}
			int clickedCol=e.Col;
			if(!checkWeekend.Checked) {
				clickedCol++;
			}
			if(checkWeekend.CheckState==CheckState.Indeterminate) {
				clickedCol=6;//used to calculate correct day to edit.
			}
			//the "clickedCell" is in terms of the entire 7 col layout.
			DateTime selectedDate=Schedules.GetDateCal(_fromDateCur,e.Row,clickedCol);
			if(selectedDate<_fromDateCur || selectedDate>_toDateCur){
				return;
			}
			//MessageBox.Show(selectedDate.ToShortDateString());
			long clinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				if(comboClinic.SelectedClinicNum==-1) {
					MsgBox.Show(this,"Please select a clinic.");
					return;
				}
			}
			string provAbbr="";
			string empFName="";
			//Get all of the selected providers and employees (removing the "none" options).
			List<Provider> listSelectedProvs=listBoxProvs.GetListSelected<Provider>().FindAll(x => x.ProvNum > 0);
			List<Employee> listSelectedEmps=listBoxEmps.GetListSelected<Employee>().FindAll(x => x.EmployeeNum > 0);
			if(listSelectedProvs.Count==1 && listSelectedEmps.Count==0) {//only 1 provider selected, pass into schedule day filter
				provAbbr=listSelectedProvs[0].Abbr;
			}
			else if(listSelectedEmps.Count==1 && listSelectedProvs.Count==0) {//only 1 employee selected, pass into schedule day filter
				empFName=listSelectedEmps[0].FName;
			}
			else if(listSelectedProvs.Count==1 && listSelectedEmps.Count==1) {//1 provider and 1 employee selected
				//see if the names match, if we're dealing with the same person it's okay to pass both in, if not then don't pass in either. 
				if(listSelectedProvs[0].FName==listSelectedEmps[0].FName 
					&& listSelectedProvs[0].LName==listSelectedEmps[0].LName) 
				{
					provAbbr=listSelectedProvs[0].Abbr;
					empFName=listSelectedEmps[0].FName;
				}
			}
			FormScheduleDayEdit FormS=new FormScheduleDayEdit(selectedDate,clinicNum,provAbbr,empFName,true);
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK){
				return;
			}
			FillGrid();
			changed=true;
		}

		private void listProv_Click(object sender,EventArgs e) {
			_provsChanged=true;
		}

		private void listEmp_Click(object sender,EventArgs e) {
			_provsChanged=true;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!ValidateInputs()) {
				return;
			}
			if(gridMain.SelectedCell.X==-1) {
				MsgBox.Show(this,"Please select a date first.");
				return;
			}
			if(_provsChanged) {
				MsgBox.Show(this,"Provider or Employee selection has been changed.  Please refresh first.");
				return;
			}
			if(HaveDatesChanged) {
				MsgBox.Show(this,"Dates have changed, refresh before continuing.");
				return;
			}
			int startI=1;
			if(checkWeekend.Checked) {
				startI=0;
			}
			DateTime dateSelectedStart=Schedules.GetDateCal(_fromDateCur,gridMain.SelectedCell.Y,startI);
			DateTime dateSelectedEnd;
			if(checkWeekend.Checked) {
				dateSelectedEnd=dateSelectedStart.AddDays(6);
			}
			else {
				dateSelectedEnd=dateSelectedStart.AddDays(4);
			}
			List<long> listProvNums;
			List<long> listEmployeeNums;
			GetSelectedProvidersEmployeesAndClinic(out listProvNums,out listEmployeeNums);
			if(listProvNums.Count>0) {
				if(MessageBox.Show(Lan.g(this,"Delete schedules for")+" "+listProvNums.Distinct().Count()+" "
					+Lan.g(this,"provider(s) for the selected week?"),"",MessageBoxButtons.YesNo)!=DialogResult.Yes) 
				{
					return;
				}
			}
			Schedules.Clear(dateSelectedStart,dateSelectedEnd,listProvNums,listEmployeeNums,checkPracticeNotes.Checked,checkClinicNotes.Checked,comboClinic.SelectedClinicNum);
			FillGrid();
			changed=true;
		}

		private void butCopyDay_Click(object sender,EventArgs e) {
			if(!ValidateInputs()) {
				return;
			}
			if(gridMain.SelectedCell.X==-1){
				MsgBox.Show(this,"Please select a date first.");
				return;
			}
			if((listBoxEmps.SelectedIndices.Count==1 && listBoxEmps.SelectedIndices.Contains(0)//'none' selected only
				|| listBoxEmps.SelectedIndices.Count==0)//Nothing selected
				&& (listBoxProvs.SelectedIndices.Count==1 && listBoxProvs.SelectedIndices.Contains(0)//'none' selected only
				|| listBoxProvs.SelectedIndices.Count==0))//Nothing selected 
			{
				MsgBox.Show(this,"No providers or employees have been selected.");
				return;
			}
			if(HaveDatesChanged) {
				MsgBox.Show(this,"Dates have changed, refresh before continuing.");
				return;
			}
			int selectedCol=gridMain.SelectedCell.X;
			if(!checkWeekend.Checked) {
				selectedCol++;
			}
			_dateCopyStart=Schedules.GetDateCal(_fromDateCur,gridMain.SelectedCell.Y,selectedCol);
			_dateCopyEnd=_dateCopyStart;
			textClipboard.Text=_dateCopyStart.ToShortDateString();
		}

		private void butCopyWeek_Click(object sender,EventArgs e) {
			if(!ValidateInputs()) {
				return;
			}
			if(gridMain.SelectedCell.X==-1) {
				MsgBox.Show(this,"Please select a date first.");
				return;
			}
			if((listBoxEmps.SelectedIndices.Count==1 && listBoxEmps.SelectedIndices.Contains(0)//'none' selected only
				|| listBoxEmps.SelectedIndices.Count==0)//Nothing selected
				&& (listBoxProvs.SelectedIndices.Count==1 && listBoxProvs.SelectedIndices.Contains(0)//'none' selected only
				|| listBoxProvs.SelectedIndices.Count==0))//Nothing selected 
			{
				MsgBox.Show(this,"No providers or employees have been selected.");
				return;
			}
			if(HaveDatesChanged) {
				MsgBox.Show(this,"Dates have changed, refresh before continuing.");
				return;
			}
			int startI=1;
			if(checkWeekend.Checked){
				startI=0;
			}
			_dateCopyStart=Schedules.GetDateCal(_fromDateCur,gridMain.SelectedCell.Y,startI);
			if(checkWeekend.Checked){
				_dateCopyEnd=_dateCopyStart.AddDays(6);
			}
			else{
				_dateCopyEnd=_dateCopyStart.AddDays(4);
			}
			textClipboard.Text=_dateCopyStart.ToShortDateString()+" - "+_dateCopyEnd.ToShortDateString();
		}

		private void butPaste_Click(object sender,EventArgs e) {
			if(!ValidateInputs()) {
				return;
			}
			if(gridMain.SelectedCell.X==-1) {
				MsgBox.Show(this,"Please select a date first.");
				return;
			}
			if(_dateCopyStart.Year<1880) {
				MsgBox.Show(this,"Please copy a selection to the clipboard first.");
				return;
			}
			if(_provsChanged){
				MsgBox.Show(this,"Provider or Employee selection has been changed.  Please refresh first.");
				return;
			}
			//calculate which day or week is currently selected.
			DateTime dateSelectedStart;
			DateTime dateSelectedEnd;
			bool isWeek=(_dateCopyStart!=_dateCopyEnd);
			if(isWeek){
				int startI=1;
				if(checkWeekend.Checked) {
					startI=0;
				}
				dateSelectedStart=Schedules.GetDateCal(_fromDateCur,gridMain.SelectedCell.Y,startI);
				if(checkWeekend.Checked) {
					dateSelectedEnd=dateSelectedStart.AddDays(6);
				}
				else {
					dateSelectedEnd=dateSelectedStart.AddDays(4);
				}
			}
			else{
				int selectedCol=gridMain.SelectedCell.X;
				if(!checkWeekend.Checked) {
					selectedCol++;
				}
				dateSelectedStart=Schedules.GetDateCal(_fromDateCur,gridMain.SelectedCell.Y,selectedCol);
				dateSelectedEnd=dateSelectedStart;
			}
			//it's not allowed to paste back over the same day or week.
			if(dateSelectedStart==_dateCopyStart) {
				MsgBox.Show(this,"Not allowed to paste back onto the same date as is on the clipboard.");
				return;
			}
			Action actionCloseScheduleProgress=ODProgress.Show(ODEventType.Schedule,typeof(ScheduleEvent));
			List<long> listProvNums;
			List<long> listEmployeeNums;
			GetSelectedProvidersEmployeesAndClinic(out listProvNums,out listEmployeeNums);
			//Get the official list of schedules that are going to be copied over.
			List<Schedule> listSchedulesToCopy=Schedules.RefreshPeriod(_dateCopyStart,_dateCopyEnd,listProvNums,listEmployeeNums,checkPracticeNotes.Checked,
				checkClinicNotes.Checked,comboClinic.SelectedClinicNum);
			listSchedulesToCopy.RemoveAll(x => x.SchedType==ScheduleType.Practice && x.Status==SchedStatus.Holiday);//Remove holiday schedules from the copy
			if(checkReplace.Checked) {
				if(listProvNums.Count > 0) {
					int countDistinctProvNums=listSchedulesToCopy.Where(x => x.ProvNum!=0).Select(y => y.ProvNum).Distinct().Count();
					actionCloseScheduleProgress?.Invoke();
					if(MessageBox.Show(Lan.g(this,"Replace schedules for")+" "+countDistinctProvNums+" "
						+Lan.g(this,"provider(s)?"),"",MessageBoxButtons.YesNo)!=DialogResult.Yes) 
					{
						return;
					}
					if(listBoxProvs.SelectedIndices.Count > countDistinctProvNums && !MsgBox.Show(this,MsgBoxButtons.YesNo,
						"One or more selected providers do not have schedules for the date range copied to the Clipboard Contents. "
						+"These providers will have their schedules removed wherever pasted.  Continue?")) 
					{
						return;
					}
					//user chose to continue.
					actionCloseScheduleProgress=ODProgress.Show(ODEventType.Schedule,typeof(ScheduleEvent));
				}
			}
			//Flag every schedule that we are copying as new (because conflict detection requires schedules marked as new)
			listSchedulesToCopy.ForEach(x => x.IsNew=true);
			//Always check for overlapping schedules regardless of checkReplace.Checked.
			if(checkWarnProvOverlap.Checked) {
				//Only check overlapping provider schedules.
				List<Schedule> listProvSchedules=listSchedulesToCopy.FindAll(x => x.SchedType==ScheduleType.Provider);
				List<long> listOverlappingProvNums=Schedules.GetOverlappingSchedProvNumsForRange(listProvSchedules,dateSelectedStart,dateSelectedEnd
					,listIgnoreProvNums:(checkReplace.Checked ? listProvNums : null));
				if(listOverlappingProvNums.Count>0) {
					actionCloseScheduleProgress?.Invoke();
					if(MessageBox.Show(Lan.g(this,"Overlapping provider schedules detected, would you like to continue anyway?")
						+"\r\n"+Lan.g(this,"Providers affected")
						+":\r\n  "+string.Join("\r\n  ",listOverlappingProvNums.Select(x=>Providers.GetLongDesc(x))),"",MessageBoxButtons.YesNo)!=DialogResult.Yes) 
					{
						return;
					}
					actionCloseScheduleProgress=ODProgress.Show(ODEventType.Schedule,typeof(ScheduleEvent));
				}
			}
			List<Schedule> listHolidaySchedules=new List<Schedule>();
			bool isHoliday=false;
			if(checkReplace.Checked) {
				List<Schedule> listSchedsToDelete=Schedules.GetSchedulesToDelete(dateSelectedStart,dateSelectedEnd,listProvNums,listEmployeeNums,
					checkPracticeNotes.Checked,checkClinicNotes.Checked,comboClinic.SelectedClinicNum);
				for(int i=listSchedsToDelete.Count-1;i>=0;i--) {//When pasting, do not paste over a holiday schedule.
					if(listSchedsToDelete[i].SchedType==ScheduleType.Practice && listSchedsToDelete[i].Status==SchedStatus.Holiday) {
						listHolidaySchedules.Add(listSchedsToDelete[i]);
						listSchedsToDelete.Remove(listSchedsToDelete[i]);//This is a holiday schedule, do not delete it when clearing.
						isHoliday=true;
					}
				}
				Schedules.DeleteMany(listSchedsToDelete.Select(x => x.ScheduleNum).ToList());
			}
			Schedule sched;
			int weekDelta=0;
			if(isWeek){
				TimeSpan span=dateSelectedStart-_dateCopyStart;
				weekDelta=span.Days/7;//usually a positive # representing a future paste, but can be negative
			}
			List<Schedule> listSchedulesToInsert=new List<Schedule>();
			for(int i=0;i<listSchedulesToCopy.Count;i++){
				sched=listSchedulesToCopy[i];
				if(isWeek){
					sched.SchedDate=sched.SchedDate.AddDays(weekDelta*7);
				}
				else{
					sched.SchedDate=dateSelectedStart;
				}
				if(listHolidaySchedules.Exists(x => x.SchedDate==sched.SchedDate)) {
					continue;//Don't add schedules to a day that's a holiday.
				}
				listSchedulesToInsert.Add(sched);
			}
			if(isHoliday) {
				MsgBox.Show(this,"One or more holidays exist in the destination date range.  These will not be replaced.  "
					+"To replace them, remove holiday schedules from the destination date range and repeat this process.");
			}
			Schedules.Insert(false,true,listSchedulesToInsert.ToArray());
			DateTime rememberDateStart=_dateCopyStart;
			DateTime rememberDateEnd=_dateCopyEnd;
			_clickedCell=gridMain.SelectedCell;
			FillGrid();
			_dateCopyStart=rememberDateStart;
			_dateCopyEnd=rememberDateEnd;
			if(isWeek){
				textClipboard.Text=_dateCopyStart.ToShortDateString()+" - "+_dateCopyEnd.ToShortDateString();
			}
			else{
				textClipboard.Text=_dateCopyStart.ToShortDateString();
			}
			changed=true;
			actionCloseScheduleProgress?.Invoke();
		}

		private void butRepeat_Click(object sender,EventArgs e) {
			bool isWeek=false;
			if(_dateCopyStart!=_dateCopyEnd) {
				isWeek=true;
			}
			if(!ValidateInputs()) {
				return;
			}
			int repeatCount;
			try{
				repeatCount=PIn.Int(textRepeat.Text);
			}
			catch{
				MsgBox.Show(this,"Please fix number box first.");
				return;
			}
			if(repeatCount>1250 && !isWeek) {
				MsgBox.Show(this,"Please enter a number of days less than 1250.");
				return;
			}
			if(repeatCount>250 && isWeek) {
				MsgBox.Show(this,"Please enter a number of weeks less than 250.");
				return;
			}
			if(gridMain.SelectedCell.X==-1) {
				MsgBox.Show(this,"Please select a date first.");
				return;
			}
			if(_dateCopyStart.Year<1880) {
				MsgBox.Show(this,"Please copy a selection to the clipboard first.");
				return;
			}
			if(_provsChanged) {
				MsgBox.Show(this,"Provider or Employee selection has been changed.  Please refresh first.");
				return;
			}
			Action actionCloseScheduleProgress=ODProgress.Show(ODEventType.Schedule,typeof(ScheduleEvent));
			Logger.LogToPath("",LogPath.Signals,LogPhase.Start);
			//calculate which day or week is currently selected.
			DateTime dateSelectedStart;
			DateTime dateSelectedEnd;
			if(isWeek) {
				int startI=1;
				if(checkWeekend.Checked) {
					startI=0;
				}
				dateSelectedStart=Schedules.GetDateCal(_fromDateCur,gridMain.SelectedCell.Y,startI);
				if(checkWeekend.Checked) {
					dateSelectedEnd=dateSelectedStart.AddDays(6);
				}
				else {
					dateSelectedEnd=dateSelectedStart.AddDays(4);
				}
			}
			else {
				int selectedCol=gridMain.SelectedCell.X;
				if(!checkWeekend.Checked) {
					selectedCol++;
				}
				dateSelectedStart=Schedules.GetDateCal(_fromDateCur,gridMain.SelectedCell.Y,selectedCol);
				dateSelectedEnd=dateSelectedStart;
			}
			List<long> listProvNums;
			List<long> listEmployeeNums;
			GetSelectedProvidersEmployeesAndClinic(out listProvNums,out listEmployeeNums);
			Logger.LogToPath("RefreshPeriod",LogPath.Signals,LogPhase.Start);
			List<Schedule> listSchedulesToCopy=Schedules.RefreshPeriod(_dateCopyStart,_dateCopyEnd,listProvNums,listEmployeeNums,checkPracticeNotes.Checked,
				checkClinicNotes.Checked,comboClinic.SelectedClinicNum);
			listSchedulesToCopy.RemoveAll(x => x.SchedType==ScheduleType.Practice && x.Status==SchedStatus.Holiday);//Remove holiday schedules from the copy
			if(checkReplace.Checked) {
				if(listProvNums.Count > 0) {
					int countDistinctProvNums=listSchedulesToCopy.Where(x => x.ProvNum!=0).Select(y => y.ProvNum).Distinct().Count();
					actionCloseScheduleProgress?.Invoke();
					if(MessageBox.Show(Lan.g(this,"Replace schedules for")+" "+countDistinctProvNums+" "
						+Lan.g(this,"provider(s)?"),"",MessageBoxButtons.YesNo)==DialogResult.No) 
					{
						return;
					}
					if(listBoxProvs.SelectedIndices.Count > countDistinctProvNums && !MsgBox.Show(this,MsgBoxButtons.YesNo,
						"One or more selected providers do not have schedules for the date range copied to the Clipboard Contents. "
						+"These providers will have their schedules removed wherever pasted.  Continue?")) 
					{
						return;
					}
					//user chose to continue.
					actionCloseScheduleProgress=ODProgress.Show(ODEventType.Schedule,typeof(ScheduleEvent));
				}
			}
			//Flag every schedule that we are copying as new (because conflict detection requires schedules marked as new)
			listSchedulesToCopy.ForEach(x => x.IsNew=true);
			Logger.LogToPath("RefreshPeriod",LogPath.Signals,LogPhase.End);
			Schedule sched;
			int weekDelta=0;
			TimeSpan span;
			if(isWeek) {
				span=dateSelectedStart-_dateCopyStart;
				weekDelta=span.Days/7;//usually a positive # representing a future paste, but can be negative
			}
			int dayDelta=0;//this is needed when repeat pasting days in order to calculate skipping weekends.
			//Get the official list of schedules that are going to be copied.
			//Always check for overlapping schedules regardless of checkReplace.Checked.
			if(checkWarnProvOverlap.Checked) {
				//Only check overlapping provider schedules.
				List<Schedule> listProvSchedules=listSchedulesToCopy.FindAll(x => x.SchedType==ScheduleType.Provider);
				List<long> listOverlappingProvNums=new List<long>();
				DateTime dateStart;
				DateTime dateEnd;
				for(int i=0;i<repeatCount;i++) {
					if(isWeek) {
						dateStart=dateSelectedStart.AddDays(i*7);
						dateEnd=dateSelectedEnd.AddDays(i*7);
					}
					else {
						dateStart=dateSelectedStart.AddDays(dayDelta);
						dateEnd=dateSelectedEnd.AddDays(dayDelta);
					}
					listOverlappingProvNums=listOverlappingProvNums
						.Union(Schedules.GetOverlappingSchedProvNumsForRange(listProvSchedules,dateStart,dateEnd
							,listIgnoreProvNums: (checkReplace.Checked ? listProvNums : null)))
						.ToList();
					if(!checkWeekend.Checked && dateSelectedStart.AddDays(dayDelta).DayOfWeek==DayOfWeek.Friday) {
						dayDelta+=3;
					}
					else {
						dayDelta++;
					}
				}
				if(listOverlappingProvNums.Count > 0) {
					actionCloseScheduleProgress?.Invoke();
					if(MessageBox.Show(Lan.g(this,"Overlapping provider schedules detected, would you like to continue anyway?")
						+"\r\n"+Lan.g(this,"Providers affected")
						+":\r\n  "+string.Join("\r\n  ",listOverlappingProvNums.Select(x=>Providers.GetLongDesc(x))),"",MessageBoxButtons.YesNo)!=DialogResult.Yes) 
					{
						return;
					}
					actionCloseScheduleProgress=ODProgress.Show(ODEventType.Schedule,typeof(ScheduleEvent));
				}
			}
			Logger.LogToPath("ScheduleUpsert",LogPath.Signals,LogPhase.Start,"repeatCount: "+repeatCount.ToString());
			List<Schedule> listSchedulesToInsert=new List<Schedule>();
			List<long> listSchedulesToDelete=new List<long>();
			dayDelta=0;//this is needed when repeat pasting days in order to calculate skipping weekends.
			//dayDelta will start out zero and increment separately from r.
			bool isHoliday=false;
			for(int r=0;r<repeatCount;r++){//for example, user wants to repeat 3 times.
				List<Schedule> listHolidaySchedules=new List<Schedule>();
				if(checkReplace.Checked) {
					List<Schedule> listSchedsToDelete=new List<Schedule>();
					if(isWeek){
						Logger.LogToPath("isWeek.Schedules.Clear",LogPath.Signals,LogPhase.Start);
						listSchedsToDelete=Schedules.GetSchedulesToDelete(dateSelectedStart.AddDays(r*7),dateSelectedEnd.AddDays(r*7),listProvNums,
							listEmployeeNums,checkPracticeNotes.Checked,checkClinicNotes.Checked,comboClinic.SelectedClinicNum);
						Logger.LogToPath("isWeek.Schedules.Clear",LogPath.Signals,LogPhase.End);
					}
					else{
						Logger.LogToPath("!isWeek.Schedules.Clear",LogPath.Signals,LogPhase.Start);
					  listSchedsToDelete=Schedules.GetSchedulesToDelete(dateSelectedStart.AddDays(dayDelta),dateSelectedEnd.AddDays(dayDelta),
							listProvNums,listEmployeeNums,checkPracticeNotes.Checked,checkClinicNotes.Checked,comboClinic.SelectedClinicNum);
						Logger.LogToPath("!isWeek.Schedules.Clear",LogPath.Signals,LogPhase.End);
					}
					for(int i=listSchedsToDelete.Count-1;i>=0;i--) {//When pasting, do not paste over a holiday schedule.
						if(listSchedsToDelete[i].SchedType==ScheduleType.Practice && listSchedsToDelete[i].Status==SchedStatus.Holiday) {
							listHolidaySchedules.Add(listSchedsToDelete[i]);
							listSchedsToDelete.Remove(listSchedsToDelete[i]);//This is a holiday schedule, do not delete it when clearing.
							isHoliday=true;
						}
					}
					listSchedulesToDelete.AddRange(listSchedsToDelete.Select(x => x.ScheduleNum).ToList());
				}
				Logger.LogToPath("SchedList.Insert",LogPath.Signals,LogPhase.Start,"SchedList.Count: "+listSchedulesToCopy.Count.ToString());
				for(int i=0;i<listSchedulesToCopy.Count;i++) {//For example, if 3 weeks for one provider, then about 30 loops.
					sched=listSchedulesToCopy[i].Copy();
					if(isWeek) {
						sched.SchedDate=sched.SchedDate.AddDays((weekDelta+r)*7).AddHours(1).Date;
					}
					else {
						sched.SchedDate=dateSelectedStart.AddDays(dayDelta);
					}
					if(listHolidaySchedules.Exists(x => x.SchedDate==sched.SchedDate)) {
						continue;//Don't add a new schedule to a day that's a holiday.
					}
					listSchedulesToInsert.Add(sched);
				}
				Logger.LogToPath("SchedList.Insert",LogPath.Signals,LogPhase.End);		
				if(!checkWeekend.Checked && dateSelectedStart.AddDays(dayDelta).DayOfWeek==DayOfWeek.Friday){
					dayDelta+=3;
				}
				else{
					dayDelta++;
				}
			}
			if(isHoliday) {
				MsgBox.Show(this,"One or more holidays exist in the destination date range.  These will not be replaced.  "
					+"To replace them, remove holiday schedules from the destination date range and repeat this process.");
			}
			Schedules.DeleteMany(listSchedulesToDelete);
			Schedules.Insert(false,true,listSchedulesToInsert.ToArray());
			Logger.LogToPath("ScheduleUpsert",LogPath.Signals,LogPhase.End);
			DateTime rememberDateStart=_dateCopyStart;
			DateTime rememberDateEnd=_dateCopyEnd;
			_clickedCell=gridMain.SelectedCell;
			FillGrid();
			_dateCopyStart=rememberDateStart;
			_dateCopyEnd=rememberDateEnd;
			if(isWeek) {
				textClipboard.Text=_dateCopyStart.ToShortDateString()+" - "+_dateCopyEnd.ToShortDateString();
			}
			else {
				textClipboard.Text=_dateCopyStart.ToShortDateString();
			}
			changed=true;
			actionCloseScheduleProgress?.Invoke();
			Logger.LogToPath("",LogPath.Signals,LogPhase.End);
		}

		private void butPrint_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			headingPrinted=false;
			PrinterL.TryPrintOrDebugRpPreview(pd_PrintPage,Lan.g(this,"Staff schedule printed"));
		}

		private void pd_PrintPage(object sender,PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			//new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text=Lan.g(this,"Schedule");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				//yPos+=(int)g.MeasureString(text,headingFont).Height;
				//text=textDateFrom.Text+" "+Lan.g(this,"to")+" "+textDateTo.Text;
				//g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=25;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			yPos=gridMain.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		///<summary>Fires as resizing is happening.</summary>
		private void FormSchedule_Resize(object sender,EventArgs e) {
			if(_isResizing || WindowState==FormWindowState.Minimized) {
				return;
			}
			FillGrid(false);
		}

		///<summary>Fires when manual resizing begins.</summary>
		private void FormSchedule_ResizeBegin(object sender,EventArgs e) {
			_clickedCell=gridMain.SelectedCell;
			_isResizing=true;
		}

		///<summary>Fires when resizing is complete, except when changing window state. I.e. this is not fired when the window is maximized or minimized.</summary>
		private void FormSchedule_ResizeEnd(object sender,EventArgs e) {
			FillGrid(false);
			_isResizing=false;
		}

		private void FormSchedule_FormClosing(object sender,FormClosingEventArgs e) {
			if(changed){
				SecurityLogs.MakeLogEntry(Permissions.Schedules,0,"");
			}
		}
	}

	public enum FormScheduleMode {
		SetupSchedule,
		ViewSchedule
	}
}





















