using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// </summary>
	public class FormQuestionDefs:ODForm {
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butAdd;
		private System.ComponentModel.IContainer components;
		private Label label1;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butUp;
		private OpenDental.UI.ODGrid gridMain;
		private System.Windows.Forms.ToolTip toolTip1;
		private QuestionDef[] QuestionList;

		///<summary></summary>
		public FormQuestionDefs()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQuestionDefs));
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.butClose = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(15,9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(709,50);
			this.label1.TabIndex = 8;
			this.label1.Text = "These are the questions that will show on the patient questionnaire.  You can saf" +
    "ely move or delete any questions without harming previously completed questionna" +
    "ires.";
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(740,637);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(79,26);
			this.butClose.TabIndex = 1;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butAdd
			// 
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(740,462);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(79,26);
			this.butAdd.TabIndex = 7;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butDown
			// 
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(740,569);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(79,26);
			this.butDown.TabIndex = 14;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0,1);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(740,537);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(79,26);
			this.butUp.TabIndex = 13;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// gridMain
			// 
			this.gridMain.Location = new System.Drawing.Point(18,62);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(706,601);
			this.gridMain.TabIndex = 15;
			this.gridMain.Title = "Questions";
			this.gridMain.TranslationName = "TableQuestionDefs";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// FormQuestionDefs
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(838,675);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butAdd);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormQuestionDefs";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Questionnaire Setup";
			this.Load += new System.EventHandler(this.FormQuestionDefs_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormQuestionDefs_Load(object sender, System.EventArgs e) {
			FillGrid();
		}

		private void FillGrid(){
			QuestionList=QuestionDefs.Refresh();
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableQuestionDefs","Type"),110);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableQuestionDefs","Question"),570);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			for(int i=0;i<QuestionList.Length;i++){
				row=new GridRow();
				row.Cells.Add(Lan.g("enumQuestionType",QuestionList[i].QuestType.ToString()));
				row.Cells.Add(QuestionList[i].Description);
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormQuestionDefEdit FormQ=new FormQuestionDefEdit(QuestionList[e.Row]);
			FormQ.ShowDialog();
			if(FormQ.DialogResult!=DialogResult.OK)
				return;
			FillGrid();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			QuestionDef def=new QuestionDef();
			def.ItemOrder=QuestionList.Length;
			FormQuestionDefEdit FormQ=new FormQuestionDefEdit(def);
			FormQ.IsNew=true;
			FormQ.ShowDialog();
			FillGrid();
		}

		private void butUp_Click(object sender,EventArgs e) {
			int selected=gridMain.GetSelectedIndex();
			try{
				QuestionDefs.MoveUp(selected,QuestionList);
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			FillGrid();
			if(selected==0) {
				gridMain.SetSelected(0,true);
			}
			else{
				gridMain.SetSelected(selected-1,true);
			}
		}

		private void butDown_Click(object sender,EventArgs e) {
			int selected=gridMain.GetSelectedIndex();
			try {
				QuestionDefs.MoveDown(selected,QuestionList);
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			FillGrid();
			if(selected==QuestionList.Length-1) {
				gridMain.SetSelected(selected,true);
			}
			else{
				gridMain.SetSelected(selected+1,true);
			}
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

	

		

		

		

		

		

		

		

		


	}
}



























