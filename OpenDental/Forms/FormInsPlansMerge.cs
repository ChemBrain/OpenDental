/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormInsPlansMerge:ODForm {
		private System.ComponentModel.Container components = null;// Required designer variable.
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		///<summary>After closing this form, if OK, then this will contain the Plan that the others will be merged into.</summary>
		public InsPlan PlanToMergeTo;
		private OpenDental.UI.ODGrid gridMain;
		///<summary>This list must be set before loading the form.  All of the PlanNums will be 0.</summary>
		public InsPlan[] ListAll;
		private Label label1;

		///<summary></summary>
		public FormInsPlansMerge(){
			InitializeComponent();// Required for Windows Form Designer support
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInsPlansMerge));
			this.label1 = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12,9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(935,27);
			this.label1.TabIndex = 20;
			this.label1.Text = "Please select one plan from the list and click OK.  All the other plans will be m" +
    "ade identical to the plan you select.  You can double click on a plan to view pe" +
    "rcentages, etc.";
			// 
			// gridMain
			// 
			this.gridMain.Location = new System.Drawing.Point(11,39);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(936,591);
			this.gridMain.TabIndex = 19;
			this.gridMain.Title = "Insurance Plans";
			this.gridMain.TranslationName = "TableTemplates";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(776,636);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(78,26);
			this.butOK.TabIndex = 4;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(871,636);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(77,26);
			this.butCancel.TabIndex = 5;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormInsPlansMerge
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(962,669);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormInsPlansMerge";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Combine Insurance Plans";
			this.Load += new System.EventHandler(this.FormInsPlansMerge_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormInsPlansMerge_Load(object sender, System.EventArgs e) {
			FillGrid();
		}

		///<summary></summary>
		private void FillGrid(){
			Cursor=Cursors.WaitCursor;
			int indexSelected=0;
			if(gridMain.SelectedIndices.Length>0) {
				indexSelected=gridMain.GetSelectedIndex();
			}
			//ListAll: Set externally before loading.
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn("Employer",100);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("Carrier",100);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("Phone",82);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("Address",100);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("City",80);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("ST",25);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("Zip",50);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("Group#",70);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("Group Name",90);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("Subs",40);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("Plan Note",180);
			gridMain.ListGridColumns.Add(col);
			//TrojanID and PlanNote not shown
			gridMain.ListGridRows.Clear();
			GridRow row;
			Carrier carrier;
			for(int i=0;i<ListAll.Length;i++) {
				row=new GridRow();
				row.Cells.Add(Employers.GetName(ListAll[i].EmployerNum));
				carrier=Carriers.GetCarrier(ListAll[i].CarrierNum);
				row.Cells.Add(carrier.CarrierName);
				row.Cells.Add(carrier.Phone);
				row.Cells.Add(carrier.Address);
				row.Cells.Add(carrier.City);
				row.Cells.Add(carrier.State);
				row.Cells.Add(carrier.Zip);
				row.Cells.Add(ListAll[i].GroupNum);
				row.Cells.Add(ListAll[i].GroupName);
				row.Cells.Add(ListAll[i].NumberSubscribers.ToString());
				row.Cells.Add(ListAll[i].PlanNote);
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.SetSelected(indexSelected,true);
			Cursor=Cursors.Default;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e){
			InsPlan PlanCur=ListAll[e.Row].Copy();
			FormInsPlan FormIP=new FormInsPlan(PlanCur,null,null);
			//FormIP.IsForAll=true;
			//FormIP.IsReadOnly=true;
			FormIP.ShowDialog();
			if(FormIP.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Once insurance plans have been merged, it is not possible to unmerge them.")) {
				return;
			}
			PlanToMergeTo=ListAll[gridMain.GetSelectedIndex()].Copy();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		

		

		

		
	

		

		

		
		

		

		

	}
}


















