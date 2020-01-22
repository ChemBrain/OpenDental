using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.ReportingComplex;
using System.Collections.Generic;
using OpenDental.UI;
using System.Linq;
using CodeBase;

namespace OpenDental{
///<summary></summary>
	public class FormRpClaimNotSent : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butRunReport;
		private System.ComponentModel.Container components = null;
		private UI.Button butRefresh;
		private UI.ODGrid gridMain;
		private GroupBox groupBoxFilters;
		DateTime _startDate=new DateTime();
		DateTime _endDate=new DateTime();
		private List<ClaimTracking> _listNewClaimTrackings=new List<ClaimTracking>();
		private List<ClaimTracking> _listOldClaimTrackings=new List<ClaimTracking>();
		private List<Userod> _listClaimSentEditUsers=new List<Userod>();
		private Label labelClaimFilter;
		private ComboBox comboBoxInsFilter;
		private ODDateRangePicker odDateRangePicker;
		private ComboBoxClinicPicker comboClinicMulti;
		private ContextMenu rightClickMenu=new ContextMenu();

		///<summary></summary>
		public FormRpClaimNotSent(){
			InitializeComponent();
			gridMain.ContextMenu=rightClickMenu;
			Lan.F(this);
		}

		///<summary></summary>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpClaimNotSent));
			this.butCancel = new OpenDental.UI.Button();
			this.butRunReport = new OpenDental.UI.Button();
			this.butRefresh = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.groupBoxFilters = new System.Windows.Forms.GroupBox();
			this.comboClinicMulti = new OpenDental.UI.ComboBoxClinicPicker();
			this.odDateRangePicker = new OpenDental.UI.ODDateRangePicker();
			this.comboBoxInsFilter = new System.Windows.Forms.ComboBox();
			this.labelClaimFilter = new System.Windows.Forms.Label();
			this.groupBoxFilters.SuspendLayout();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(1049, 591);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butRunReport
			// 
			this.butRunReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butRunReport.Location = new System.Drawing.Point(12, 591);
			this.butRunReport.Name = "butRunReport";
			this.butRunReport.Size = new System.Drawing.Size(75, 26);
			this.butRunReport.TabIndex = 3;
			this.butRunReport.Text = "&Run Report";
			this.butRunReport.Click += new System.EventHandler(this.butRunReport_Click);
			// 
			// butRefresh
			// 
			this.butRefresh.Location = new System.Drawing.Point(1035, 12);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 26);
			this.butRefresh.TabIndex = 60;
			this.butRefresh.Text = "&Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(12, 65);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(1112, 517);
			this.gridMain.TabIndex = 61;
			this.gridMain.Title = "Claims Not Sent";
			this.gridMain.TranslationName = "TableClaimsNotSent";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// groupBoxFilters
			// 
			this.groupBoxFilters.Controls.Add(this.comboClinicMulti);
			this.groupBoxFilters.Controls.Add(this.odDateRangePicker);
			this.groupBoxFilters.Controls.Add(this.comboBoxInsFilter);
			this.groupBoxFilters.Controls.Add(this.butRefresh);
			this.groupBoxFilters.Controls.Add(this.labelClaimFilter);
			this.groupBoxFilters.Location = new System.Drawing.Point(12, 12);
			this.groupBoxFilters.Name = "groupBoxFilters";
			this.groupBoxFilters.Size = new System.Drawing.Size(1112, 47);
			this.groupBoxFilters.TabIndex = 63;
			this.groupBoxFilters.TabStop = false;
			this.groupBoxFilters.Text = "Filters";
			// 
			// comboClinicMulti
			// 
			this.comboClinicMulti.IncludeAll = true;
			this.comboClinicMulti.IncludeUnassigned = true;
			this.comboClinicMulti.SelectionModeMulti = true;
			this.comboClinicMulti.Location = new System.Drawing.Point(446, 15);
			this.comboClinicMulti.Name = "comboClinicMulti";
			this.comboClinicMulti.Size = new System.Drawing.Size(176, 21);
			this.comboClinicMulti.TabIndex = 67;
			// 
			// odDateRangePicker
			// 
			this.odDateRangePicker.BackColor = System.Drawing.SystemColors.Control;
			this.odDateRangePicker.DefaultDateTimeFrom = new System.DateTime(((long)(0)));
			this.odDateRangePicker.DefaultDateTimeTo = new System.DateTime(((long)(0)));
			this.odDateRangePicker.Location = new System.Drawing.Point(6, 13);
			this.odDateRangePicker.MaximumSize = new System.Drawing.Size(0, 185);
			this.odDateRangePicker.MinimumSize = new System.Drawing.Size(0, 24);
			this.odDateRangePicker.Name = "odDateRangePicker";
			this.odDateRangePicker.Size = new System.Drawing.Size(445, 24);
			this.odDateRangePicker.TabIndex = 66;
			// 
			// comboBoxInsFilter
			// 
			this.comboBoxInsFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxInsFilter.FormattingEnabled = true;
			this.comboBoxInsFilter.Location = new System.Drawing.Point(741, 15);
			this.comboBoxInsFilter.Name = "comboBoxInsFilter";
			this.comboBoxInsFilter.Size = new System.Drawing.Size(121, 21);
			this.comboBoxInsFilter.TabIndex = 65;
			// 
			// labelClaimFilter
			// 
			this.labelClaimFilter.Location = new System.Drawing.Point(645, 18);
			this.labelClaimFilter.Name = "labelClaimFilter";
			this.labelClaimFilter.Size = new System.Drawing.Size(93, 14);
			this.labelClaimFilter.TabIndex = 64;
			this.labelClaimFilter.Text = "Claim Filter";
			this.labelClaimFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormRpClaimNotSent
			// 
			this.AcceptButton = this.butRunReport;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(1136, 626);
			this.Controls.Add(this.groupBoxFilters);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butRunReport);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormRpClaimNotSent";
			this.Text = "Claims Not Sent Report";
			this.Load += new System.EventHandler(this.FormClaimNotSent_Load);
			this.groupBoxFilters.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormClaimNotSent_Load(object sender, System.EventArgs e) {
			SetFilterControlsAndAction(() => { FillGrid(); }
				,odDateRangePicker,comboClinicMulti,comboBoxInsFilter
			);
			odDateRangePicker.SetDateTimeTo(DateTime.Now.Date);
			odDateRangePicker.SetDateTimeFrom(odDateRangePicker.GetDateTimeTo().AddDays(-7)); //default to the previous week
			_listClaimSentEditUsers=Userods.GetUsersByPermission(Permissions.ClaimSentEdit,false);
			_listOldClaimTrackings=ClaimTrackings.RefreshForUsers(ClaimTrackingType.ClaimUser,_listClaimSentEditUsers.Select(x => x.UserNum).ToList());
			_listNewClaimTrackings=_listOldClaimTrackings.Select(x => x.Copy()).ToList();
			//Fill the Ins Filter box
			//comboBoxInsFilter.Items.Add(Lan.g(this,"All"));//always adding an 'All' option first
			foreach(ClaimNotSentStatuses claimStat in Enum.GetValues(typeof(ClaimNotSentStatuses))) {
				comboBoxInsFilter.Items.Add(claimStat);
			}
			comboBoxInsFilter.SelectedIndex=0;//Pre-select 'All' for the user
			//Fill the right-click menu for the grid
			List<MenuItem> listMenuItems=new List<MenuItem>();
			//The first item in the list will always exists, but we toggle it's visibility to only show when 1 row is selected.
			listMenuItems.Add(new MenuItem(Lan.g(this,"Go to Account"),new EventHandler(gridMain_RightClickHelper)));
			listMenuItems[0].Tag=0;//Tags are used to identify what to do in gridMain_RightClickHelper.
			Menu.MenuItemCollection menuItemCollection=new Menu.MenuItemCollection(rightClickMenu);
			menuItemCollection.AddRange(listMenuItems.ToArray());
			rightClickMenu.Popup+=new EventHandler((o,ea) => {
				rightClickMenu.MenuItems[0].Visible=(gridMain.SelectedIndices.Count()==1);//Only show 'Go to Account' when there is exactly 1 row selected.
			});
		}

		///<summary>Gets all unsent claims in the database between the user entered date range and with the appropriate user selected filters.</summary>
		private void FillGrid() {
			if(!ValidateFilters()) {
				return;
			}
			List<long> listClinicNums=comboClinicMulti.ListSelectedClinicNums;
			DataTable table=RpClaimNotSent.GetClaimsNotSent(_startDate,_endDate,listClinicNums,false
				,(ClaimNotSentStatuses)comboBoxInsFilter.SelectedItem);//this query can get slow with a large number of clinics (like NADG)
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col;
			if(PrefC.HasClinicsEnabled) {
				col=new GridColumn(Lan.g(gridMain.TranslationName,"Clinic"),90);
				gridMain.ListGridColumns.Add(col);
			}
			col=new GridColumn(Lan.g(gridMain.TranslationName,"Date of Service"),90,GridSortingStrategy.DateParse);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(gridMain.TranslationName,"Claim Type"),90);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(gridMain.TranslationName,"Claim Status"),100);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(gridMain.TranslationName,"Patient Name"),150);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(gridMain.TranslationName,"Carrier Name"),150);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(gridMain.TranslationName,"Claim Fee"),90,GridSortingStrategy.AmountParse);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(gridMain.TranslationName,"Proc Codes"),100);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			for(int i = 0;i<table.Rows.Count;i++) {
				row=new GridRow();
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(table.Rows[i]["Clinic"].ToString());
				}
				DateTime dateService=PIn.Date(table.Rows[i]["DateService"].ToString());
				row.Cells.Add(dateService.ToShortDateString());
				string type=table.Rows[i]["ClaimType"].ToString();
				switch(type) {
					case "P":
						type="Pri";
						break;
					case "S":
						type="Sec";
						break;
					case "PreAuth":
						type="Preauth";
						break;
					case "Other":
						type="Other";
						break;
					case "Cap":
						type="Cap";
						break;
					case "Med":
						type="Medical";//For possible future use.
						break;
					default:
						type="Error";//Not allowed to be blank.
						break;
				}
				row.Cells.Add(type);
				row.Cells.Add(table.Rows[i]["ClaimStatus"].ToString());
				row.Cells.Add(table.Rows[i]["Patient Name"].ToString());
				row.Cells.Add(table.Rows[i]["CarrierName"].ToString());
				row.Cells.Add(PIn.Double(table.Rows[i]["ClaimFee"].ToString()).ToString("c"));
				row.Cells.Add(table.Rows[i]["ProcCodes"].ToString());
				UnsentInsClaim unsentClaim=new UnsentInsClaim();
				unsentClaim.ClaimNum=PIn.Long(table.Rows[i]["ClaimNum"].ToString());
				unsentClaim.PatNum=PIn.Long(table.Rows[i]["PatNum"].ToString());
				ClaimTracking claimTrackingCur=_listNewClaimTrackings.FirstOrDefault(x => x.ClaimNum==unsentClaim.ClaimNum);
				if(claimTrackingCur!=null) {
					unsentClaim.ClaimTrackingNum=claimTrackingCur.ClaimTrackingNum;
				}
				row.Tag=unsentClaim;
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		///<summary>Checks the filters available on this form for correct format, order, and non-empty selections.
		///Will return false and show a message to the user if filters are not valid.  Otherwise, returns true.</summary>
		private bool ValidateFilters() {
			//ODDateRangePicker only does the bare minimum for date validation
			if(odDateRangePicker.GetDateTimeFrom()==DateTime.MinValue || odDateRangePicker.GetDateTimeTo()==DateTime.MinValue) {
				MsgBox.Show(this,"Please enter valid dates.");
				return false;
			}
			if(_endDate<_startDate) {
				MsgBox.Show(this,"End date cannot be before start date.");
				return false;
			}
			if(PrefC.HasClinicsEnabled && comboClinicMulti.ListSelectedClinicNums.Count==0) {
				MsgBox.Show(this,"At least one clinic must be selected.");
				return false;
			}
			_startDate=odDateRangePicker.GetDateTimeFrom();
			_endDate=odDateRangePicker.GetDateTimeTo();
			return true;
		}

		///<summary>Uses the report complex pattern to print the same report generated by FillGrid()</summary>
		private void butRunReport_Click(object sender,EventArgs e) {
			if(!ValidateFilters()) {
				return;
			}
			ReportComplex report=new ReportComplex(true,false);
			List<long> listClinicNums=comboClinicMulti.ListSelectedClinicNums;
			DataTable table=RpClaimNotSent.GetClaimsNotSent(_startDate,_endDate,listClinicNums,true,(ClaimNotSentStatuses)comboBoxInsFilter.SelectedItem);
			string subtitleClinics="";
			subtitleClinics=comboClinicMulti.GetStringSelectedClinics();
			report.ReportName=Lan.g(this,"Claims Not Sent");
			report.AddTitle("Title",Lan.g(this,"Claims Not Sent"));
			if(PrefC.HasClinicsEnabled) {
				report.AddSubTitle("Clinics",subtitleClinics);
			}
			QueryObject query=report.AddQuery(table,"Date: " + DateTimeOD.Today.ToShortDateString());
			if(PrefC.HasClinicsEnabled) {
				query.AddColumn("Clinic",60,FieldValueType.String);
			}
			query.AddColumn("Date",85,FieldValueType.Date);
			query.GetColumnDetail("Date").StringFormat="d";
			query.AddColumn("Type",90,FieldValueType.String);
			query.AddColumn("Claim Status",100,FieldValueType.String);
			query.AddColumn("Patient Name",150,FieldValueType.String);
			query.AddColumn("Insurance Carrier",150,FieldValueType.String);
			query.AddColumn("Amount",90,FieldValueType.Number);
			query.AddColumn("Proc Codes",90);
			report.AddPageNum();
			if(!report.SubmitQueries()) {
				return;
			}
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();
			DialogResult=DialogResult.OK;
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}

		///<summary>Click method used by all gridMain right click options.
		///We identify what logic to run by the menuItem.Tag.</summary>
		private void gridMain_RightClickHelper(object sender,EventArgs e) {
			int index=gridMain.GetSelectedIndex();
			if(index==-1) {
				return;
			}
			int menuCode=(int)((MenuItem)sender).Tag;
			switch(menuCode) {
				case 0://Go to Account
					GotoModule.GotoAccount(((UnsentInsClaim)gridMain.ListGridRows[index].Tag).PatNum);
					break;
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.ClaimView)) {
				return;
			}
			Claim claim=Claims.GetClaim(((UnsentInsClaim)gridMain.ListGridRows[e.Row].Tag).ClaimNum);
			if(claim==null) {
				MsgBox.Show(this,"The claim has been deleted.");
				FillGrid();
				return;
			}
			Patient pat=Patients.GetPat(claim.PatNum);
			Family fam=Patients.GetFamily(pat.PatNum);
			FormClaimEdit FormCE=new FormClaimEdit(claim,pat,fam);
			FormCE.IsNew=false;
			FormCE.ShowDialog();
			if(FormCE.DialogResult==DialogResult.OK) {
				FillGrid();//FillGrid() is necessary here as we are allowing the user to open up claim details and they could change the claim status or claim type.
				return;
			}
		}
		
		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();
		}




		///<summary>Only used in this form to keep track of both the ClaimNum and PatNum within the grid. This is the grid's tag object.</summary>
		private class UnsentInsClaim {
			public long ClaimNum;
			public long PatNum;
			public long ClaimTrackingNum;
		}
	}
}
