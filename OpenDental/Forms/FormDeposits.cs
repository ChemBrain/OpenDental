using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormDeposits : ODForm {
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butAdd;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid grid;
		private Deposit[] DList;
		private OpenDental.UI.Button butOK;
		///<summary>Use this from Transaction screen when attaching a source document.</summary>
		public bool IsSelectionMode;
		///<summary>List of Clinics the user has access to.</summary>
		private List<Clinic> _listClinics=new List<Clinic>();
		private ComboBoxClinicPicker comboClinics;

		///<summary>In selection mode, when closing form with OK, this contains selected deposit.</summary>
		public Deposit SelectedDeposit;

		///<summary></summary>
		public FormDeposits()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDeposits));
			this.butClose = new OpenDental.UI.Button();
			this.grid = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.comboClinics = new OpenDental.UI.ComboBoxClinicPicker();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(365, 574);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 3;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// grid
			// 
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.Location = new System.Drawing.Point(18, 34);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(339, 567);
			this.grid.TabIndex = 1;
			this.grid.Title = "Deposit Slips";
			this.grid.TranslationName = "TableDepositSlips";
			this.grid.WrapText = false;
			this.grid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellDoubleClick);
			// 
			// butAdd
			// 
			this.butAdd.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(363, 293);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(77, 26);
			this.butAdd.TabIndex = 0;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(365, 542);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 2;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// comboClinics
			// 
			this.comboClinics.IncludeAll = true;
			this.comboClinics.IncludeUnassigned = true;
			this.comboClinics.Location = new System.Drawing.Point(72, 7);
			this.comboClinics.Name = "comboClinics";
			this.comboClinics.SelectionModeMulti = true;
			this.comboClinics.Size = new System.Drawing.Size(200, 21);
			this.comboClinics.TabIndex = 131;
			this.comboClinics.SelectionChangeCommitted += new System.EventHandler(this.ComboClinics_SelectionChangeCommitted);
			// 
			// FormDeposits
			// 
			this.AcceptButton = this.butOK;
			this.ClientSize = new System.Drawing.Size(454, 613);
			this.Controls.Add(this.comboClinics);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butAdd);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(370, 250);
			this.Name = "FormDeposits";
			this.ShowInTaskbar = false;
			this.Text = "Deposit Slips";
			this.Load += new System.EventHandler(this.FormDeposits_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormDeposits_Load(object sender, System.EventArgs e) {
			if(IsSelectionMode){
				butAdd.Visible=false;
			}
			else{
				butOK.Visible=false;
			}
			FillGrid();
		}

		private void FillGrid(){
			if(!PrefC.HasClinicsEnabled) {
				if(IsSelectionMode) {
					DList=Deposits.GetUnattached();
				}
				else {
					DList=Deposits.Refresh();
				}
			}
			else {
				//GetForClinics uses an empty list to indicate "all", which is a loophole if user doesn't select an item.  So:
				if(comboClinics.ListSelectedClinicNums.Count==0){
					DList=Deposits.GetForClinics(new List<long>(){Clinics.ClinicNum },IsSelectionMode);//restrict to current clinic
				}
				else{
					DList=Deposits.GetForClinics(comboClinics.ListSelectedClinicNums,IsSelectionMode);
				}
			}
			grid.BeginUpdate();
			grid.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableDepositSlips","Date"),80);
			grid.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableDepositSlips","Amount"),90,HorizontalAlignment.Right);
			grid.ListGridColumns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new GridColumn(Lan.g("TableDepositSlips","Clinic"),150);
				grid.ListGridColumns.Add(col);
			}
			grid.ListGridRows.Clear();
			OpenDental.UI.GridRow row;
			for(int i=0;i<DList.Length;i++){
				row=new OpenDental.UI.GridRow();
				row.Cells.Add(DList[i].DateDeposit.ToShortDateString());
				row.Cells.Add(DList[i].Amount.ToString("F"));
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(" "+DList[i].ClinicAbbr);//padding left with space to add separation between amount and clinic abbr
				}
				grid.ListGridRows.Add(row);
			}
			grid.EndUpdate();
			grid.ScrollToEnd();
		}

		private void ComboClinics_SelectionChangeCommitted(object sender, EventArgs e){
			FillGrid();
		}

		private void grid_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			if(IsSelectionMode){
				SelectedDeposit=DList[e.Row];
				DialogResult=DialogResult.OK;
				return;
			}
			//not selection mode.
			FormDepositEdit FormD=new FormDepositEdit(DList[e.Row]);
			FormD.ShowDialog();
			if(FormD.DialogResult==DialogResult.Cancel){
				return;
			}
			FillGrid();
		}

		///<summary>Not available in selection mode.</summary>
		private void butAdd_Click(object sender, System.EventArgs e) {
			Deposit deposit=new Deposit();
			deposit.DateDeposit=DateTime.Today;
			deposit.BankAccountInfo=PrefC.GetString(PrefName.PracticeBankNumber);
			FormDepositEdit FormD=new FormDepositEdit(deposit);
			FormD.IsNew=true;
			FormD.ShowDialog();
			if(FormD.DialogResult==DialogResult.Cancel){
				return;
			}
			FillGrid();
		}

		///<summary>Only available in selection mode.</summary>
		private void butOK_Click(object sender,EventArgs e) {
			if(grid.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select a deposit first.");
				return;
			}
			SelectedDeposit=DList[grid.GetSelectedIndex()];
			DialogResult=DialogResult.OK;
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		
	}
}





















