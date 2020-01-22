using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using OpenDentBusiness;
using OpenDental.UI;
using System.Collections.Generic;
using System.Linq;
using CodeBase;
using System.Resources;
using System.Globalization;
using System.Text;
using OpenDental.Thinfinity;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormClaimForms : ODForm {
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butDuplicate;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butExport;
		private OpenDental.UI.Button butImport;
		private System.Windows.Forms.GroupBox groupBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butDefault;
		private OpenDental.UI.Button butReassign;
		private Label label2;
		private ComboBox comboReassign;
		private UI.ODGrid gridInternal;
		private UI.ODGrid gridCustom;
		private UI.Button butCopy;
		private GroupBox groupBox2;
		private Label label1;
		private bool changed;

		///<summary></summary>
		public FormClaimForms()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClaimForms));
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.comboReassign = new System.Windows.Forms.ComboBox();
			this.butReassign = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butDuplicate = new OpenDental.UI.Button();
			this.butExport = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.butImport = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.gridCustom = new OpenDental.UI.ODGrid();
			this.gridInternal = new OpenDental.UI.ODGrid();
			this.butCopy = new OpenDental.UI.Button();
			this.butDefault = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.comboReassign);
			this.groupBox2.Controls.Add(this.butReassign);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new System.Drawing.Point(540, 71);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(232, 122);
			this.groupBox2.TabIndex = 12;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Reassign";
			// 
			// comboReassign
			// 
			this.comboReassign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboReassign.FormattingEnabled = true;
			this.comboReassign.Location = new System.Drawing.Point(15, 64);
			this.comboReassign.MaxDropDownItems = 20;
			this.comboReassign.Name = "comboReassign";
			this.comboReassign.Size = new System.Drawing.Size(169, 21);
			this.comboReassign.TabIndex = 15;
			// 
			// butReassign
			// 
			this.butReassign.Location = new System.Drawing.Point(15, 87);
			this.butReassign.Name = "butReassign";
			this.butReassign.Size = new System.Drawing.Size(87, 23);
			this.butReassign.TabIndex = 13;
			this.butReassign.Text = "Reassign";
			this.butReassign.Click += new System.EventHandler(this.butReassign_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(203, 47);
			this.label2.TabIndex = 14;
			this.label2.Text = "Reassign all insurance plans that use the selected claim form at the left to the " +
    "claim form below";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.butDuplicate);
			this.groupBox1.Controls.Add(this.butExport);
			this.groupBox1.Controls.Add(this.butAdd);
			this.groupBox1.Controls.Add(this.butImport);
			this.groupBox1.Controls.Add(this.butDelete);
			this.groupBox1.Location = new System.Drawing.Point(540, 258);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(232, 122);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Custom Claim Form Options";
			// 
			// butDuplicate
			// 
			this.butDuplicate.Location = new System.Drawing.Point(15, 87);
			this.butDuplicate.Name = "butDuplicate";
			this.butDuplicate.Size = new System.Drawing.Size(75, 23);
			this.butDuplicate.TabIndex = 4;
			this.butDuplicate.Text = "Duplicate";
			this.butDuplicate.Click += new System.EventHandler(this.butDuplicate_Click);
			// 
			// butExport
			// 
			this.butExport.Location = new System.Drawing.Point(141, 25);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(75, 23);
			this.butExport.TabIndex = 9;
			this.butExport.Text = "Export";
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
			// 
			// butAdd
			// 
			this.butAdd.Location = new System.Drawing.Point(15, 25);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 23);
			this.butAdd.TabIndex = 3;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butImport
			// 
			this.butImport.Location = new System.Drawing.Point(141, 56);
			this.butImport.Name = "butImport";
			this.butImport.Size = new System.Drawing.Size(75, 23);
			this.butImport.TabIndex = 10;
			this.butImport.Text = "Import";
			this.butImport.Click += new System.EventHandler(this.butImport_Click);
			// 
			// butDelete
			// 
			this.butDelete.Location = new System.Drawing.Point(15, 56);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 23);
			this.butDelete.TabIndex = 5;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(5, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(263, 28);
			this.label1.TabIndex = 19;
			this.label1.Text = "Internal forms cannot be edited or printed. \r\nCopy the form over to a custom form" +
    " first.";
			// 
			// gridCustom
			// 
			this.gridCustom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridCustom.Location = new System.Drawing.Point(274, 38);
			this.gridCustom.Name = "gridCustom";
			this.gridCustom.Size = new System.Drawing.Size(260, 372);
			this.gridCustom.TabIndex = 17;
			this.gridCustom.Title = "Custom Claim Forms";
			this.gridCustom.TranslationName = "TableClaimFormsCustom";
			this.gridCustom.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCustom_CellDoubleClick);
			// 
			// gridInternal
			// 
			this.gridInternal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridInternal.Location = new System.Drawing.Point(8, 38);
			this.gridInternal.Name = "gridInternal";
			this.gridInternal.Size = new System.Drawing.Size(186, 372);
			this.gridInternal.TabIndex = 16;
			this.gridInternal.Title = "Internal Claim Forms";
			this.gridInternal.TranslationName = "TableClaimFormsInternal";
			this.gridInternal.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridInternal_CellDoubleClick);
			// 
			// butCopy
			// 
			this.butCopy.Image = global::OpenDental.Properties.Resources.Right;
			this.butCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCopy.Location = new System.Drawing.Point(200, 221);
			this.butCopy.Name = "butCopy";
			this.butCopy.Size = new System.Drawing.Size(68, 23);
			this.butCopy.TabIndex = 18;
			this.butCopy.Text = "Copy";
			this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
			// 
			// butDefault
			// 
			this.butDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDefault.Location = new System.Drawing.Point(540, 38);
			this.butDefault.Name = "butDefault";
			this.butDefault.Size = new System.Drawing.Size(87, 23);
			this.butDefault.TabIndex = 12;
			this.butDefault.Text = "Set Default";
			this.butDefault.Click += new System.EventHandler(this.butDefault_Click);
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(698, 389);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 23);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormClaimForms
			// 
			this.ClientSize = new System.Drawing.Size(785, 424);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.butCopy);
			this.Controls.Add(this.gridCustom);
			this.Controls.Add(this.gridInternal);
			this.Controls.Add(this.butDefault);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(801, 463);
			this.Name = "FormClaimForms";
			this.ShowInTaskbar = false;
			this.Text = "Claim Forms";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormClaimForms_Closing);
			this.Load += new System.EventHandler(this.FormClaimForms_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormClaimForms_Load(object sender, System.EventArgs e) {
			FillGridInternal();
			FillGridCustom();
		}

		private void FillGridInternal() {
			gridInternal.BeginUpdate();
			gridInternal.ListGridColumns.Clear();
			gridInternal.ListGridColumns.Add(new GridColumn(Lan.g("TableClaimFormsInternal","ClaimForm"),150));
			gridInternal.ListGridRows.Clear();
			foreach(ClaimForm internalForm in ClaimForms.GetInternalClaims()) {
				GridRow row = new GridRow();
				row.Cells.Add(internalForm.Description);
				row.Tag = internalForm;
				gridInternal.ListGridRows.Add(row);
			}
			gridInternal.EndUpdate();
		}

		///<summary></summary>
		private void FillGridCustom() {
			ClaimFormItems.RefreshCache();
			ClaimForms.RefreshCache();
			comboReassign.Items.Clear();
			gridCustom.BeginUpdate();
			gridCustom.ListGridColumns.Clear();
			gridCustom.ListGridColumns.Add(new GridColumn(Lan.g("TableClaimFormsCustom","ClaimForm"),145));
			gridCustom.ListGridColumns.Add(new GridColumn(Lan.g("TableClaimFormsCustom","Default"),50,HorizontalAlignment.Center));
			gridCustom.ListGridColumns.Add(new GridColumn(Lan.g("TableClaimFormsCustom","Hidden"),0,HorizontalAlignment.Center));
			gridCustom.ListGridRows.Clear();
			string description;
			foreach(ClaimForm claimFormCur in ClaimForms.GetDeepCopy()) {
				description=claimFormCur.Description;
				GridRow row = new GridRow();
				row.Cells.Add(claimFormCur.Description);
				if(claimFormCur.ClaimFormNum==PrefC.GetLong(PrefName.DefaultClaimForm)) {
					description+=" "+Lan.g(this,"(default)");
					row.Cells.Add("X");
				}
				else {
					row.Cells.Add("");
				}
				if(claimFormCur.IsHidden) {
					description+=" "+Lan.g(this,"(hidden)");
					row.Cells.Add("X");
				}
				else {
					row.Cells.Add("");
				}
				row.Tag = claimFormCur;
				gridCustom.ListGridRows.Add(row);
				comboReassign.Items.Add(new ODBoxItem<ClaimForm>(description,claimFormCur));
			}
			gridCustom.EndUpdate();
		}

		///<summary>Copy an internal form over to a new custom form.</summary>
		private void butCopy_Click(object sender,EventArgs e) {
			if(gridInternal.GetSelectedIndex()==-1) {
				MessageBox.Show(Lan.g(this,"Please select an item from the internal grid to copy over to the custom grid."));
				return;
			}
			//just insert it into the db.
			ClaimForm claimFormInternal = (ClaimForm)gridInternal.ListGridRows[gridInternal.GetSelectedIndex()].Tag;
			long claimFormNum = ClaimForms.Insert(claimFormInternal,true);
			FillGridCustom();
		}

		private void gridInternal_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(e.Row==-1) {
				return;
			}
			FormClaimFormEdit FormCFE = new FormClaimFormEdit((ClaimForm)gridInternal.ListGridRows[e.Row].Tag);
			FormCFE.ShowDialog();
			if(FormCFE.DialogResult==DialogResult.OK) {
				changed=true;
			}
		}

		private void gridCustom_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(e.Row==-1) {
				return;
			}
			FormClaimFormEdit FormCFE = new FormClaimFormEdit((ClaimForm)gridCustom.ListGridRows[e.Row].Tag);
			FormCFE.ShowDialog();
			if(FormCFE.DialogResult!=DialogResult.OK) {
				return;
			}
			changed=true;
			FillGridCustom();
		}

		///<summary>Add a custom claim form.</summary>
		private void butAdd_Click(object sender, System.EventArgs e) {
			ClaimForm ClaimFormCur=new ClaimForm();
			ClaimForms.Insert(ClaimFormCur,false);
			ClaimFormCur.IsNew=true;
			FormClaimFormEdit FormCFE=new FormClaimFormEdit(ClaimFormCur);
			FormCFE.ShowDialog();
			if(FormCFE.DialogResult!=DialogResult.OK){
				return;
			}
			changed=true;
			FillGridCustom();
		}

		///<summary>Delete an unusued custom claim form.</summary>
		private void butDelete_Click(object sender, System.EventArgs e) {
			if(gridCustom.GetSelectedIndex()==-1){
				MessageBox.Show(Lan.g(this,"Please select a Custom Claim Form first."));
				return;
			}
			ClaimForm claimFormCur = (ClaimForm)gridCustom.ListGridRows[gridCustom.GetSelectedIndex()].Tag;
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete custom claim form?")) {
				return;
			}
			if(!ClaimForms.Delete(claimFormCur)){
				MsgBox.Show(this,"Claim form is already in use.");
				return;
			}
			changed=true;
			FillGridCustom();
		}

		///<summary>Duplicate a custom claim form.</summary>
		private void butDuplicate_Click(object sender, System.EventArgs e) {
			if(gridCustom.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select a Custom Claim Form first.");
				return;
			}
			ClaimForm claimFormCur = (ClaimForm)gridCustom.ListGridRows[gridCustom.GetSelectedIndex()].Tag;
			long oldClaimFormNum=claimFormCur.ClaimFormNum;
			//claimFormCur.UniqueID="";//designates it as a user added claimform
			ClaimForms.Insert(claimFormCur,true);//this duplicates the original claimform, but no items.
			changed=true;
			FillGridCustom();
		}

		///<summary>Export a custom claim form. Even though we could probably allow this for internal claim forms as well, 
		///users can always copy over an internal claim form to a custom form and then export it.</summary>
		private void butExport_Click(object sender, System.EventArgs e) {
			if(gridCustom.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select a Custom Claim Form first.");
				return;
			}
			ClaimForm claimFormCur = (ClaimForm)gridCustom.ListGridRows[gridCustom.GetSelectedIndex()].Tag;
			string filename = "ClaimForm"+claimFormCur.Description+".xml";
			if(ODBuild.IsWeb()) {
				StringBuilder strbuild=new StringBuilder();
				using(XmlWriter writer=XmlWriter.Create(strbuild)) {
					XmlSerializer serializer=new XmlSerializer(typeof(ClaimForm));
					serializer.Serialize(writer,claimFormCur);
				}
				ThinfinityUtils.ExportForDownload(filename,strbuild.ToString());
				return;
			}
			try {
				using(SaveFileDialog saveDlg=new SaveFileDialog()) {
					saveDlg.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
					saveDlg.FileName=filename;
					if(saveDlg.ShowDialog()!=DialogResult.OK) {
						return;
					}
					XmlSerializer serializer=new XmlSerializer(typeof(ClaimForm));
					using(TextWriter writer=new StreamWriter(saveDlg.FileName)) {
						serializer.Serialize(writer,claimFormCur);
					}
				}
				MsgBox.Show(this,"Exported");
			}
			catch(Exception ex) {
				ex.DoNothing();
				MsgBox.Show(this,"Export failed.  This could be due to lack of permissions in the designated folder.");
			}
		}

		///<summary>Import an XML file into the custom claim forms list.</summary>
		private void butImport_Click(object sender, System.EventArgs e) {
			OpenFileDialog openDlg=new OpenFileDialog();
			openDlg.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			ClaimForm claimForm;
			if(openDlg.ShowDialog()!=DialogResult.OK){
				return;
			}
			try{
				claimForm=ClaimForms.DeserializeClaimForm(openDlg.FileName,"");
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			ClaimForms.Insert(claimForm,true);//now we have a primary key.
			MsgBox.Show(this,"Imported");
			changed=true;
			FillGridCustom();
		}		

		///<summary>Sets a custom claim form as the default.  We do not currently allow setting internal claim forms as default - users need to copy it over first.</summary>
		private void butDefault_Click(object sender,EventArgs e) {
			if(gridCustom.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select a claimform from the list first.");
				return;
			}
			ClaimForm claimFormCur = (ClaimForm)gridCustom.ListGridRows[gridCustom.GetSelectedIndex()].Tag;
			if(Prefs.UpdateLong(PrefName.DefaultClaimForm,claimFormCur.ClaimFormNum)){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			FillGridCustom();
		}

		///<summary>Reassigns all current insurance plans using the selected claimform to another claimform.</summary>
		private void butReassign_Click(object sender,EventArgs e) {
			if(gridCustom.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a claimform from the list at the left first.");
				return;
			}
			if(comboReassign.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select a claimform from the list below.");
				return;
			}
			ClaimForm claimFormCur = (ClaimForm)gridCustom.ListGridRows[gridCustom.GetSelectedIndex()].Tag;
			ClaimForm claimFormNew = ((ODBoxItem<ClaimForm>)comboReassign.SelectedItem).Tag;
			long result=ClaimForms.Reassign(claimFormCur.ClaimFormNum,claimFormNew.ClaimFormNum);
			MessageBox.Show(result.ToString()+Lan.g(this," plans changed."));
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormClaimForms_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(changed){
				DataValid.SetInvalid(InvalidType.ClaimForms);
			}
		}
	}
}





















