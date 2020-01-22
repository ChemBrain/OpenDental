using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	///<summary>Display fields specifically for the ortho chart.</summary>
	public class FormDisplayFieldsOrthoChart : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.ODGrid gridMain;
		///<summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butUp;
		private ListBox listAvailable;
		private Label labelAvailable;
		private OpenDental.UI.Button butRight;
		private OpenDental.UI.Button butLeft;
		private bool changed;
		private OpenDental.UI.Button butOK;
		private Label labelCategory;
		private Label labelCustomField;
		private TextBox textCustomField;
		private ComboBox comboOrthoChartTabs;
		private UI.Button butSetupTabs;
		private Label label1;
		///<summary>The outter list represents each tab.  Within each tab, there can be multiple display fields.
		///An individual display field record can be associated to multiple tabs (the exact same object can exist in multiple lists).</summary>
		private List<OrthoChartTabFields> _listTabDisplayFields=new List<OrthoChartTabFields>();
		///<summary>The list of existing display fields which are not currently in use within the selected ortho chart tab.</summary>
		private List <DisplayField> _listAvailableFields=null;
		///<summary>All ortho chart display fields available which includes "orphaned" display fields.  Filled on load.</summary>
		private List<DisplayField> _listAllDisplayFields=null;
		private List<OrthoChartTab> _listOrthoChartTabs;
		///<summary>Local copy of cached OrthoChartTabLinks. Ensures that any cached changes cause by others will not effect what we see.</summary>
		private List<OrthoChartTabLink> _listOrthoChartTabLinks;

		public FormDisplayFieldsOrthoChart() {
			InitializeComponent();
			Lan.F(this);
		}

		protected override void Dispose(bool disposing)	{
			if(disposing)	{
				if(components!=null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDisplayFieldsOrthoChart));
			this.listAvailable = new System.Windows.Forms.ListBox();
			this.labelAvailable = new System.Windows.Forms.Label();
			this.labelCategory = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.labelCustomField = new System.Windows.Forms.Label();
			this.textCustomField = new System.Windows.Forms.TextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butRight = new OpenDental.UI.Button();
			this.butLeft = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.comboOrthoChartTabs = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butSetupTabs = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// listAvailable
			// 
			this.listAvailable.FormattingEnabled = true;
			this.listAvailable.IntegralHeight = false;
			this.listAvailable.Location = new System.Drawing.Point(373, 89);
			this.listAvailable.Name = "listAvailable";
			this.listAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listAvailable.Size = new System.Drawing.Size(158, 227);
			this.listAvailable.TabIndex = 3;
			this.listAvailable.Click += new System.EventHandler(this.listAvailable_Click);
			this.listAvailable.DoubleClick += new System.EventHandler(this.ListAvailable_DoubleClick);
			// 
			// labelAvailable
			// 
			this.labelAvailable.Location = new System.Drawing.Point(370, 69);
			this.labelAvailable.Name = "labelAvailable";
			this.labelAvailable.Size = new System.Drawing.Size(213, 17);
			this.labelAvailable.TabIndex = 16;
			this.labelAvailable.Text = "Available Fields";
			this.labelAvailable.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelCategory
			// 
			this.labelCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCategory.Location = new System.Drawing.Point(12, 9);
			this.labelCategory.Name = "labelCategory";
			this.labelCategory.Size = new System.Drawing.Size(213, 25);
			this.labelCategory.TabIndex = 57;
			this.labelCategory.Text = "Ortho Chart";
			// 
			// gridMain
			// 
			this.gridMain.Location = new System.Drawing.Point(12, 76);
			this.gridMain.Name = "gridMain";
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(292, 425);
			this.gridMain.TabIndex = 3;
			this.gridMain.Title = "Fields Showing";
			this.gridMain.TranslationName = "FormDisplayFields";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// labelCustomField
			// 
			this.labelCustomField.Location = new System.Drawing.Point(371, 319);
			this.labelCustomField.Name = "labelCustomField";
			this.labelCustomField.Size = new System.Drawing.Size(213, 17);
			this.labelCustomField.TabIndex = 58;
			this.labelCustomField.Text = "New Field";
			this.labelCustomField.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textCustomField
			// 
			this.textCustomField.Location = new System.Drawing.Point(373, 339);
			this.textCustomField.Name = "textCustomField";
			this.textCustomField.Size = new System.Drawing.Size(158, 20);
			this.textCustomField.TabIndex = 0;
			this.textCustomField.Click += new System.EventHandler(this.textCustomField_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(566, 474);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 8;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butRight
			// 
			this.butRight.Image = global::OpenDental.Properties.Resources.Right;
			this.butRight.Location = new System.Drawing.Point(320, 292);
			this.butRight.Name = "butRight";
			this.butRight.Size = new System.Drawing.Size(35, 24);
			this.butRight.TabIndex = 1;
			this.butRight.Click += new System.EventHandler(this.butRight_Click);
			// 
			// butLeft
			// 
			this.butLeft.AdjustImageLocation = new System.Drawing.Point(-1, 0);
			this.butLeft.Image = global::OpenDental.Properties.Resources.Left;
			this.butLeft.Location = new System.Drawing.Point(320, 252);
			this.butLeft.Name = "butLeft";
			this.butLeft.Size = new System.Drawing.Size(35, 24);
			this.butLeft.TabIndex = 2;
			this.butLeft.Click += new System.EventHandler(this.butLeft_Click);
			// 
			// butDown
			// 
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(109, 507);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(82, 24);
			this.butDown.TabIndex = 7;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(12, 507);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(82, 24);
			this.butUp.TabIndex = 6;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(566, 504);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 9;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// comboOrthoChartTabs
			// 
			this.comboOrthoChartTabs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboOrthoChartTabs.FormattingEnabled = true;
			this.comboOrthoChartTabs.Location = new System.Drawing.Point(59, 51);
			this.comboOrthoChartTabs.Name = "comboOrthoChartTabs";
			this.comboOrthoChartTabs.Size = new System.Drawing.Size(160, 21);
			this.comboOrthoChartTabs.TabIndex = 4;
			this.comboOrthoChartTabs.SelectionChangeCommitted += new System.EventHandler(this.comboOrthoChartTabs_SelectionChangeCommitted);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(15, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 21);
			this.label1.TabIndex = 61;
			this.label1.Text = "Tab";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butSetupTabs
			// 
			this.butSetupTabs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSetupTabs.Location = new System.Drawing.Point(222, 49);
			this.butSetupTabs.Name = "butSetupTabs";
			this.butSetupTabs.Size = new System.Drawing.Size(82, 24);
			this.butSetupTabs.TabIndex = 5;
			this.butSetupTabs.Text = "Setup Tabs";
			this.butSetupTabs.Click += new System.EventHandler(this.butSetupTabs_Click);
			// 
			// FormDisplayFieldsOrthoChart
			// 
			this.ClientSize = new System.Drawing.Size(664, 556);
			this.Controls.Add(this.butSetupTabs);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboOrthoChartTabs);
			this.Controls.Add(this.textCustomField);
			this.Controls.Add(this.labelCustomField);
			this.Controls.Add(this.labelCategory);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butRight);
			this.Controls.Add(this.butLeft);
			this.Controls.Add(this.labelAvailable);
			this.Controls.Add(this.listAvailable);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDisplayFieldsOrthoChart";
			this.ShowInTaskbar = false;
			this.Text = "Setup Display Fields";
			this.Load += new System.EventHandler(this.FormDisplayFields_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormDisplayFields_Load(object sender,EventArgs e) {
			_listOrthoChartTabs=OrthoChartTabs.GetDeepCopy(true);
			_listOrthoChartTabLinks=OrthoChartTabLinks.GetDeepCopy();
			LoadDisplayFields();
			FillComboOrthoChartTabs();
			FillGrids();
		}

		private void LoadDisplayFields() {
			OrthoChartTabs.RefreshCache();
			OrthoChartTabLinks.RefreshCache();
			DisplayFields.RefreshCache();
			_listAllDisplayFields=DisplayFields.GetAllAvailableList(DisplayFieldCategory.OrthoChart);
			_listTabDisplayFields=new List<OrthoChartTabFields>();
			//Add all fields that are actively associated to a tab to our class wide list of tabs and fields.
			for(int i=0;i<_listOrthoChartTabs.Count;i++) {
				OrthoChartTabFields orthoChartTabFields=new OrthoChartTabFields();
				orthoChartTabFields.OrthoChartTab=_listOrthoChartTabs[i];
				orthoChartTabFields.ListDisplayFields=new List<DisplayField>();
				List<OrthoChartTabLink> listOrthoChartTabLinks=_listOrthoChartTabLinks.FindAll(
					x => x.OrthoChartTabNum==_listOrthoChartTabs[i].OrthoChartTabNum
				);
				listOrthoChartTabLinks.OrderBy(x => x.ItemOrder);
				foreach(OrthoChartTabLink orthoChartTabLink in listOrthoChartTabLinks) {
					orthoChartTabFields.ListDisplayFields.AddRange(_listAllDisplayFields.FindAll(x => x.DisplayFieldNum==orthoChartTabLink.DisplayFieldNum));
				}
				_listTabDisplayFields.Add(orthoChartTabFields);
			}
			//Add a dummy OrthoChartTabFields object to the list that represents available fields that are not part of any tab.
			//These "display fields" were previously used at least once. A patient has info for this field, then the office removed the field from all tabs.
			List<DisplayField> listOrphanedFields=_listAllDisplayFields.FindAll(x => x.DisplayFieldNum==0
				|| !OrthoChartTabLinks.GetExists(y => y.DisplayFieldNum==x.DisplayFieldNum));
			if(listOrphanedFields!=null && listOrphanedFields.Count > 0) {
				OrthoChartTabFields orphanedFields=new OrthoChartTabFields();
				orphanedFields.OrthoChartTab=null;//These are fields not associated to any tab.  Purposefully use null.
				orphanedFields.ListDisplayFields=new List<DisplayField>();
				orphanedFields.ListDisplayFields.AddRange(listOrphanedFields);
				_listTabDisplayFields.Add(orphanedFields);
			}
		}

		private void FillComboOrthoChartTabs() {
			//We can't remember the previously selected tab (unless we did it by name?) so just refill the combo box and select the first item in the list.
			comboOrthoChartTabs.Items.Clear();
			for(int i=0;i<_listOrthoChartTabs.Count;i++) {
				comboOrthoChartTabs.Items.Add(_listOrthoChartTabs[i].TabName);
			}
			comboOrthoChartTabs.SelectedIndex=0;
		}

		private void FillGrids() {
			labelCategory.Text=_listOrthoChartTabs[0].TabName;//Placed here so that Up/Down buttons will affect the label text.			
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col;
			col=new GridColumn(Lan.g("FormDisplayFields","Description"),200);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("FormDisplayFields","Width"),80);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			OrthoChartTabFields orthoChartTabFields=GetSelectedFields();
			List<OrthoChartTabLink> listLinks=_listOrthoChartTabLinks.FindAll(x => x.OrthoChartTabNum==orthoChartTabFields.OrthoChartTab.OrthoChartTabNum);
			_listAvailableFields=GetAllFields();
			for(int i=0;i<orthoChartTabFields.ListDisplayFields.Count;i++){
				DisplayField df=orthoChartTabFields.ListDisplayFields[i];
				_listAvailableFields.Remove(df);
				GridRow row=new GridRow();
				row.Tag=df;
				string description=df.Description;
				if(!string.IsNullOrEmpty(df.DescriptionOverride)) {
					description+=" ("+df.DescriptionOverride+")";
				}
				row.Cells.Add(description);
				int columnWidth=df.ColumnWidth;
				OrthoChartTabLink link=listLinks.FirstOrDefault(x => x.DisplayFieldNum==df.DisplayFieldNum);
				if(link!=null && link.ColumnWidthOverride>0) {
					columnWidth=link.ColumnWidthOverride;
				}
				row.Cells.Add(POut.Int(columnWidth));
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
			listAvailable.Items.Clear();
			for(int i=0;i<_listAvailableFields.Count;i++) {
				listAvailable.Items.Add(_listAvailableFields[i].Description);
			}
		}

		///<summary>Get the list of all unique display fields accross all ortho chart tabs.
		///Set hasOrphanedFields to false to exclude any "available" fields that are not currently associated to a tab.</summary>
		private List<DisplayField> GetAllFields(bool hasOrphanedFields=true) {
			List<DisplayField> listDisplayFields=new List<DisplayField>();
			foreach(OrthoChartTabFields orthoChartTabFields in _listTabDisplayFields) {
				if(!hasOrphanedFields && orthoChartTabFields.OrthoChartTab==null) {
					continue;//Do not include orphaned fields.
				}
				foreach(DisplayField df in orthoChartTabFields.ListDisplayFields) {
					if(!listDisplayFields.Contains(df)) {
						listDisplayFields.Add(df);
					}
				}
			}
			return listDisplayFields;
		}

		///<summary>Gets the currently selected tab information.</summary>
		private OrthoChartTabFields GetSelectedFields() {
			long orthoChartTabNum=_listOrthoChartTabs[comboOrthoChartTabs.SelectedIndex].OrthoChartTabNum;
			return _listTabDisplayFields.FirstOrDefault(x => x.OrthoChartTab!=null && x.OrthoChartTab.OrthoChartTabNum==orthoChartTabNum);
		}

		private void comboOrthoChartTabs_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrids();
		}

		private void butSetupTabs_Click(object sender,EventArgs e) {
			FormOrthoChartSetup form=new FormOrthoChartSetup();
			if(form.ShowDialog()!=DialogResult.OK) {
				return;
			}
			List<OrthoChartTabFields> listOldTabDisplayFields=_listTabDisplayFields;
			_listTabDisplayFields=new List<OrthoChartTabFields>();
			//The tab order may have changed.  Also new tabs may have been added.  Tabs were not removed, because they can only be hidden not deleted.
			//If orthocharttabs order changed or new tabs were added, then the cache was updated in FormOrthoChartSetup in OK click.
			_listOrthoChartTabs=OrthoChartTabs.GetDeepCopy(true);
			foreach(OrthoChartTab orthoChartTab in _listOrthoChartTabs) {
				OrthoChartTabFields orthoChartTabFieldsOld=listOldTabDisplayFields.FirstOrDefault(
					x => x.OrthoChartTab!=null && x.OrthoChartTab.OrthoChartTabNum==orthoChartTab.OrthoChartTabNum);
				OrthoChartTabFields orthoChartTabFields=new OrthoChartTabFields();
				orthoChartTabFields.OrthoChartTab=orthoChartTab;
				if(orthoChartTabFieldsOld==null) {//Either a new tab was added or a hidden tab was un-hidden.
					orthoChartTabFields.ListDisplayFields=new List<DisplayField>();
					//Hidden OrthoChartTabLinks were already included on loading this form.
					List<OrthoChartTabLink> listOrthoChartTabLinks=_listOrthoChartTabLinks.FindAll(x => x.OrthoChartTabNum==orthoChartTab.OrthoChartTabNum);
					foreach(OrthoChartTabLink orthoChartTabLink in listOrthoChartTabLinks) {
						orthoChartTabFields.ListDisplayFields.AddRange(_listAllDisplayFields.FindAll(x => x.DisplayFieldNum==orthoChartTabLink.DisplayFieldNum));
					}
				}
				else {//The tab already existed.  Maintain the display field names and order within the tab, especially if the tab order changed.
					orthoChartTabFields.ListDisplayFields=orthoChartTabFieldsOld.ListDisplayFields;
				}
				_listTabDisplayFields.Add(orthoChartTabFields);
			}
			//Always add the orphaned OrthoChartTabFields back because they can not be affected from the Tab Setup window.
			_listTabDisplayFields.AddRange(listOldTabDisplayFields.FindAll(x => x.OrthoChartTab==null));
			//Refresh the combo box and the grid because there is a chance that the user was viewing a different tab than the first in the list.
			FillComboOrthoChartTabs();
			FillGrids();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			DisplayField df=(DisplayField)gridMain.ListGridRows[e.Row].Tag;
			//OrthoChartTabFields orthoChartTabFields=GetSelectedFields();
			long orthoChartTabNum=_listOrthoChartTabs[comboOrthoChartTabs.SelectedIndex].OrthoChartTabNum;
			OrthoChartTabLink linkCur=_listOrthoChartTabLinks.FirstOrDefault(x => x.OrthoChartTabNum==orthoChartTabNum
				&& x.DisplayFieldNum==df.DisplayFieldNum);
			bool isAddingNewLink=false;
			if(linkCur==null) {//Avoid thrown exception in FormDisplayFieldOrthoEdit_Load(...) when adding a new link and then attempting to edit it.
				isAddingNewLink=true;
				linkCur=new OrthoChartTabLink() {
					OrthoChartTabNum=orthoChartTabNum,
					//ItemOrder will be set when FormDisplayFieldsOrthoChart closes/syncs.
					DisplayFieldNum=df.DisplayFieldNum,
					ColumnWidthOverride=0,
				};
			}
			FormDisplayFieldOrthoEdit form=new FormDisplayFieldOrthoEdit(df,GetAllFields(),linkCur);
			form.ShowDialog();
			if(form.DialogResult!=DialogResult.OK) {
				return;
			}
			if(isAddingNewLink) {//Do not add new link to list if user clicks cancel because we want to act like nothing happened
				_listOrthoChartTabLinks.Add(linkCur);
			}
			FillGrids();
			changed=true;
		}

		private void butLeft_Click(object sender,EventArgs e) {
			if(listAvailable.SelectedItems.Count==0 && textCustomField.Text=="") {
				MsgBox.Show(this,"Please select an item in the list on the right or create a new field first.");
				return;
			}
			OrthoChartTabFields orthoChartTabFields=GetSelectedFields();
			if(textCustomField.Text!="") {//Add new ortho chart field
				//Block adding new field if already showing.
				for(int i=0;i<orthoChartTabFields.ListDisplayFields.Count;i++) {
					if(textCustomField.Text==orthoChartTabFields.ListDisplayFields[i].Description) {
						MsgBox.Show(this,"That field is already displaying.");
						return;
					}
				}
				//Use available field if user typed in a "new" field name which matches an available field that is not already showing.
				List<DisplayField> listAllFields=GetAllFields();
				foreach(DisplayField df in listAllFields) {
					if(textCustomField.Text==df.Description) {
						orthoChartTabFields.ListDisplayFields.Add(df);
						textCustomField.Text="";
						changed=true;
						FillGrids();
						return;
					}
				}
				//The new field name is unique.  Create a cached copy in memory to be saved when OK is clicked.
				//This gives the user the option to remove the field before it ever reaches the database.
				DisplayField dfNew=new DisplayField("",100,DisplayFieldCategory.OrthoChart);
				dfNew.Description=textCustomField.Text;
				orthoChartTabFields.ListDisplayFields.Add(dfNew);
				textCustomField.Text="";
			}
			else {//Add existing ortho chart field(s).
				OrthoChartTabFields orphanedTab=_listTabDisplayFields.Find(x => x.OrthoChartTab==null);
				for(int i=0;i<listAvailable.SelectedItems.Count;i++) {
					DisplayField df=_listAvailableFields[listAvailable.SelectedIndices[i]];
					orthoChartTabFields.ListDisplayFields.Add(df);
					if(orphanedTab!=null) {
						//Remove the display field from the orphaned list if there is one.
						orphanedTab.ListDisplayFields.Remove(df);
					}
				}
			}
			changed=true;
			FillGrids();
		}

		private void butRight_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid on the left first.");
				return;
			}
			List<DisplayField> listRemovedFields=new List<DisplayField>();
			OrthoChartTabFields orthoChartTabFields=GetSelectedFields();
			for(int i=gridMain.SelectedIndices.Length-1;i>=0;i--) {//go backwards
				int index=gridMain.SelectedIndices[i];
				DisplayField df=(DisplayField)gridMain.ListGridRows[index].Tag;
				listRemovedFields.Add(df);//Keep track of all display fields removed.
				orthoChartTabFields.ListDisplayFields.Remove(df);
			}
			//Now we need to check all removed fields and see if any are still associated with a tab.
			//If they are not associated with any tabs then we need to remove them from our list.
			//They will show up in the available fields list if a patient in the database has a value for the field.
			foreach(DisplayField field in listRemovedFields) {
				bool isFieldOrphaned=true;
				foreach(OrthoChartTabFields tabFields in _listTabDisplayFields) {
					if(tabFields.OrthoChartTab==null) {
						continue;//Do not consider the orphaned tab.
					}
					if(tabFields.ListDisplayFields.Exists(x => x.DisplayFieldNum==field.DisplayFieldNum)) {
						isFieldOrphaned=false;
						break;//The field that was removed is still associated with a different tab so no action needed.
					}
				}
				if(!isFieldOrphaned) {
					continue;
				}
				//No tab has this display field so move it to the list of fields associated with the null tab
				OrthoChartTabFields orphanedTab=_listTabDisplayFields.Find(x => x.OrthoChartTab==null);
				if(orphanedTab==null) {//An orphaned list doesn't exist yet so create one.
					OrthoChartTabFields orphanedFields=new OrthoChartTabFields();
					orphanedFields.OrthoChartTab=null;//These are fields not associated to any tab.  Purposefully use null.
					orphanedFields.ListDisplayFields=new List<DisplayField>();
					orphanedFields.ListDisplayFields.Add(field);
					_listTabDisplayFields.Add(orphanedFields);
				}
				else {
					orphanedTab.ListDisplayFields.Add(field);
				}
			}
			FillGrids();
			changed=true;
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			if(gridMain.SelectedIndices[0]==0) {
				return;
			}
			int[] arrayIndicies=new int[gridMain.SelectedIndices.Length];
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				arrayIndicies[i]=gridMain.SelectedIndices[i];
			}
			OrthoChartTabFields orthoChartTabFields=GetSelectedFields();
			for(int i=0;i<arrayIndicies.Length;i++){
				orthoChartTabFields.ListDisplayFields.Reverse(arrayIndicies[i]-1,2);
			}
			FillGrids();
			for(int i=0;i<arrayIndicies.Length;i++){
				gridMain.SetSelected(arrayIndicies[i]-1,true);
			}
			changed=true;
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			OrthoChartTabFields orthoChartTabFields=GetSelectedFields();
			if(gridMain.SelectedIndices[gridMain.SelectedIndices.Length-1]==orthoChartTabFields.ListDisplayFields.Count-1) {
				return;
			}
			int[] arrayIndicies=new int[gridMain.SelectedIndices.Length];
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				arrayIndicies[i]=gridMain.SelectedIndices[i];
			}
			for(int i=arrayIndicies.Length-1;i>=0;i--) {//go backwards
				orthoChartTabFields.ListDisplayFields.Reverse(arrayIndicies[i],2);
			}
			FillGrids();
			for(int i=0;i<arrayIndicies.Length;i++) {
				gridMain.SetSelected(arrayIndicies[i]+1,true);
			}
			changed=true;
		}

		private void listAvailable_Click(object sender,EventArgs e) {
			textCustomField.Text="";
		}

		private void textCustomField_Click(object sender,EventArgs e) {
			listAvailable.SelectedIndex=-1;
		}

		private void ListAvailable_DoubleClick(object sender,EventArgs e) {
			if(listAvailable.SelectedIndices.Count!=1) {//make sure only one is selected
				return;
			}
			FormDisplayFieldOrthoEdit form=new FormDisplayFieldOrthoEdit(_listAvailableFields[listAvailable.SelectedIndices[0]],GetAllFields());
			if(form.ShowDialog()!=DialogResult.OK) {
				return;
			}
			FillGrids();
			changed=true;
		}

		private void butOK_Click(object sender,EventArgs e) {
			OrthoChartTabFields orphanedTab=_listTabDisplayFields.Find(x => x.OrthoChartTab==null);
			//No need to do anything if nothing changed and there are no 'orphaned' display fields to delete.
			if(!changed && (orphanedTab!=null && orphanedTab.ListDisplayFields.All(x => x.DisplayFieldNum==0))) {
				DialogResult=DialogResult.OK;
				return;
			}
			//Get all fields associated to a tab in order to sync with the database later.
			List<DisplayField> listAllFields=GetAllFields(false);
			if(listAllFields.Count(x=>x.InternalName=="Signature") > 1) {
				MessageBox.Show(Lan.g(this,"Only one display field can be a signature field.  Fields that have the signature field checkbox checked:")+" "
					+string.Join(", ",listAllFields.FindAll(x => x.InternalName=="Signature").Select(x => x.Description)));
				return;
			}
			//Ensure all new displayfields have a primary key so that tab links can be created below.  Update existing displayfields.
			foreach(DisplayField df in listAllFields) {
				if(df.DisplayFieldNum==0) {//New displayfield
					DisplayFields.Insert(df);
				}
				else {//Existing displayfield.
					DisplayFields.Update(df);
				}
			}
			DataValid.SetInvalid(InvalidType.DisplayFields);
			//Remove tab links which no longer exist.  Update tab link item order for tab links which still belong to the same tab.
			for(int i=_listOrthoChartTabLinks.Count-1;i>=0;i--) {
				OrthoChartTabLink orthoChartTabLink=_listOrthoChartTabLinks[i];
				OrthoChartTabFields orthoChartTabFields=_listTabDisplayFields.FirstOrDefault(
					x => x.OrthoChartTab!=null && x.OrthoChartTab.OrthoChartTabNum==orthoChartTabLink.OrthoChartTabNum);
				if(orthoChartTabFields==null) {
					continue;//The tab was hidden and we are going to leave the tab links alone.
				}
				DisplayField df=orthoChartTabFields.ListDisplayFields.FirstOrDefault(x => x.DisplayFieldNum==orthoChartTabLink.DisplayFieldNum);
				if(df==null) {//The tab link no longer exists (was removed).
					_listOrthoChartTabLinks.RemoveAt(i);
				}
				else {//The tab link still exists.  Update the link with any changes.
					orthoChartTabLink.ItemOrder=orthoChartTabFields.ListDisplayFields.IndexOf(df);
				}
			}
			//Add new tab links which were just created.
			foreach(OrthoChartTabFields orthoChartTabFields in _listTabDisplayFields) {
				//Skip "orphaned" fields that just show in the available fields list.
				if(orthoChartTabFields.OrthoChartTab==null) {
					continue;
				}
				foreach(DisplayField df in orthoChartTabFields.ListDisplayFields) {
					OrthoChartTabLink orthoChartTabLink=_listOrthoChartTabLinks.FirstOrDefault(
						x => x.OrthoChartTabNum==orthoChartTabFields.OrthoChartTab.OrthoChartTabNum && x.DisplayFieldNum==df.DisplayFieldNum);
					if(orthoChartTabLink!=null) {
						continue;
					}
					orthoChartTabLink=new OrthoChartTabLink();
					orthoChartTabLink.ItemOrder=orthoChartTabFields.ListDisplayFields.IndexOf(df);
					orthoChartTabLink.OrthoChartTabNum=orthoChartTabFields.OrthoChartTab.OrthoChartTabNum;
					orthoChartTabLink.DisplayFieldNum=df.DisplayFieldNum;
					_listOrthoChartTabLinks.Add(orthoChartTabLink);
				}
			}
			//Delete any display fields that have a valid PK and are in the "orphaned" list.  
			//This is fine to do because the field will show back up in the available list of display fields if a patient is still using the field.
			//This is because we link the ortho chart display fields by their name instead of by their PK.
			if(orphanedTab!=null) {//An orphaned list actually exists.
				//Look for any display fields that have a valid PK (this means the user removed this field from every tab and we need to delete it).
				List<DisplayField> listFieldsToDelete=orphanedTab.ListDisplayFields.FindAll(x => x.DisplayFieldNum!=0);
				listFieldsToDelete.ForEach(x => DisplayFields.Delete(x.DisplayFieldNum));
			}
			OrthoChartTabLinks.Sync(_listOrthoChartTabLinks,OrthoChartTabLinks.GetDeepCopy());
			DataValid.SetInvalid(InvalidType.OrthoChartTabs);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}

	internal class OrthoChartTabFields {
		///<summary>Set or leave OrthoChartTab to null when storing the "orphaned" available fields (used in past and not associated to a tab).</summary>
		public OrthoChartTab OrthoChartTab=null;
		public List<DisplayField> ListDisplayFields=null;
	}

}
