using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormLaboratories : ODForm {
		private OpenDental.UI.Button butClose;
		private System.ComponentModel.Container components = null;
		private ODGrid gridMain;
		private OpenDental.UI.Button butAdd;// Required designer variable.
		//private bool changed;
		private List<Laboratory> ListLabs;

		///<summary></summary>
		public FormLaboratories()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLaboratories));
			this.butClose = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(592, 469);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridMain
			// 
			this.gridMain.Location = new System.Drawing.Point(33, 40);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(634, 334);
			this.gridMain.TabIndex = 1;
			this.gridMain.Title = "Labs";
			this.gridMain.TranslationName = "TableLabs";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butAdd
			// 
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(592, 394);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 26);
			this.butAdd.TabIndex = 2;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// FormLaboratories
			// 
			this.ClientSize = new System.Drawing.Size(719, 520);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(735, 515);
			this.Name = "FormLaboratories";
			this.ShowInTaskbar = false;
			this.Text = "Laboratories";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLaboratories_FormClosing);
			this.Load += new System.EventHandler(this.FormLaboratories_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormLaboratories_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid(){
			ListLabs=Laboratories.Refresh();
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableLabs","Description"),100);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableLabs","Phone"),100);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableLabs","Hidden"),50,HorizontalAlignment.Center);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableLabs","Notes"),200);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			for(int i=0;i<ListLabs.Count;i++){
				row=new GridRow();
				row.Cells.Add(ListLabs[i].Description);
				row.Cells.Add(ListLabs[i].Phone);
				row.Cells.Add(ListLabs[i].IsHidden?"X":"");
				row.Cells.Add(ListLabs[i].Notes);
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormLaboratoryEdit FormL=new FormLaboratoryEdit();
			FormL.LabCur=ListLabs[e.Row];
			FormL.ShowDialog();
			//if(FormL.DialogResult==DialogResult.OK){
				//changed=true;
			FillGrid();
			//}
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormLaboratoryEdit FormL=new FormLaboratoryEdit();
			FormL.LabCur=new Laboratory();
			FormL.IsNew=true;
			FormL.ShowDialog();
			FillGrid();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormLaboratories_FormClosing(object sender,FormClosingEventArgs e) {
			//if(changed){
				//Labs are not global.
				//DataValid.SetInvalid(InvalidTypes.Providers);
			//}
		}

		

		

		


	}
}





















