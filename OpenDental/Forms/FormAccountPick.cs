using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormAccountPick:ODForm {
		private OpenDental.UI.ODGrid gridMain;
		private IContainer components;
		private CheckBox checkInactive;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private ImageList imageListMain;
		///<summary>Upon closing with OK, this will be the selected account.</summary>
		public Account SelectedAccount;
		public bool IsQuickBooks;
		public List<string> SelectedAccountsQB;

		///<summary></summary>
		public FormAccountPick()
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccountPick));
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.checkInactive = new System.Windows.Forms.CheckBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0,"Add.gif");
			this.imageListMain.Images.SetKeyName(1,"editPencil.gif");
			// 
			// checkInactive
			// 
			this.checkInactive.AutoSize = true;
			this.checkInactive.Location = new System.Drawing.Point(20,525);
			this.checkInactive.Name = "checkInactive";
			this.checkInactive.Size = new System.Drawing.Size(150,17);
			this.checkInactive.TabIndex = 2;
			this.checkInactive.Text = "Include Inactive Accounts";
			this.checkInactive.UseVisualStyleBackColor = true;
			this.checkInactive.Click += new System.EventHandler(this.checkInactive_Click);
			// 
			// gridMain
			// 
			this.gridMain.Location = new System.Drawing.Point(8,14);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(475,505);
			this.gridMain.TabIndex = 1;
			this.gridMain.Title = "Accounts";
			this.gridMain.TranslationName = "TableChartOfAccounts";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(408,546);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,26);
			this.butCancel.TabIndex = 11;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(315,546);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,26);
			this.butOK.TabIndex = 10;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// FormAccountPick
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(492,584);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.checkInactive);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAccountPick";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Pick Account";
			this.Load += new System.EventHandler(this.FormAccountPick_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormAccountPick_Load(object sender,EventArgs e) {
			if(IsQuickBooks) {
				SelectedAccountsQB=new List<string>();
				checkInactive.Visible=false;
				FillGridQB();
				gridMain.SelectionMode=GridSelectionMode.MultiExtended;
			}
			else {
				FillGrid();
			}
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableChartOfAccounts","Type"),70);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableChartOfAccounts","Description"),170);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableChartOfAccounts","Balance"),65,HorizontalAlignment.Right);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableChartOfAccounts","Bank Number"),100);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableChartOfAccounts","Inactive"),70);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			List<Account> listAccounts=Accounts.GetDeepCopy(false);
			for(int i=0;i<listAccounts.Count;i++){
				if(!checkInactive.Checked && listAccounts[i].Inactive){
					continue;
				}
				row=new GridRow();
				row.Cells.Add(Lan.g("enumAccountType",listAccounts[i].AcctType.ToString()));
				row.Cells.Add(listAccounts[i].Description);
				if(listAccounts[i].AcctType==AccountType.Asset){
					row.Cells.Add(Accounts.GetBalance(listAccounts[i].AccountNum,listAccounts[i].AcctType).ToString("n"));
				}
				else{
					row.Cells.Add("");
				}
				row.Cells.Add(listAccounts[i].BankNumber);
				if(listAccounts[i].Inactive){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				if(i<listAccounts.Count-1//if not the last row
					&& listAccounts[i].AcctType != listAccounts[i+1].AcctType){
					row.ColorLborder=Color.Black;
				}
				row.Tag=listAccounts[i].Clone();
				row.ColorBackG=listAccounts[i].AccountColor;
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void FillGridQB(){
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableChartOfAccountsQB","Description"),200);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			//Get the list of accounts from QuickBooks.
			Cursor.Current=Cursors.WaitCursor;
			List<string> accountList=new List<string>();
			try {
				accountList=QuickBooks.GetListOfAccounts();
			}
			catch(Exception e) {
				MessageBox.Show(e.Message);
			}
			Cursor.Current=Cursors.Default;
			for(int i=0;i<accountList.Count;i++){
				row=new GridRow();
				row.Cells.Add(accountList[i]);
				row.Tag=accountList[i];
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(IsQuickBooks) {
				SelectedAccountsQB.Add((string)gridMain.ListGridRows[e.Row].Tag);
			}
			else {
				SelectedAccount=((Account)gridMain.ListGridRows[e.Row].Tag).Clone();
			}
			DialogResult=DialogResult.OK;
		}

		private void checkInactive_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select an account first.");
				return;
			}
			if(IsQuickBooks) {
				for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
					SelectedAccountsQB.Add((string)(gridMain.ListGridRows[gridMain.SelectedIndices[i]].Tag));
				}
			}
			else {
				SelectedAccount=((Account)gridMain.ListGridRows[gridMain.GetSelectedIndex()].Tag).Clone();
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

	

		

		

	


	}
}





















