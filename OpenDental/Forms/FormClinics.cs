using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness; 
using CodeBase;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormClinics:ODForm {
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private UI.ODGrid gridMain;
		private UI.Button butOK;
		private GroupBox groupMovePats;
		private UI.Button butMovePats;
		private UI.Button butClinicPick;
		private TextBox textMoveTo;
		private Label label2;
		private GroupBox groupClinicOrder;
		private UI.Button butUp;
		private UI.Button butDown;
		private UI.Button butSelectAll;
		private UI.Button butSelectNone;
		#region Private Variables
		///<summary>Set to true to open the form in selection mode. This will cause the 'Show hidden' checkbox to be hidden.</summary>
		public bool IsSelectionMode;
		public long SelectedClinicNum;
		private CheckBox checkOrderAlphabetical;
		///<summary>Set this list prior to loading this window to use a custom list of clinics.  Otherwise, uses the cache.</summary>
		public List<Clinic> ListClinics;
		///<summary>This list will be a copy of ListClinics and is used for syncing on window closed.</summary>
		private List<Clinic> _listClinicsOld;
		private CheckBox checkShowHidden;
		private long _clinicNumTo=-1;
		///<summary>Set to true prior to loading to include a 'Headquarters' option.</summary>
		public bool IncludeHQInList;
		private SerializableDictionary<long,int> _dictClinicalCounts;
		private List<DefLink> _listClinicDefLinksAllOld;
		///<summary>Pass in a list of clinics that should be pre-selected. 
		///When this form is closed, this list will be the list of clinics that the user selected.</summary>
		public List<long> ListSelectedClinicNums = new List<long>();
		///<summary>Set to true if the user can select multiple clinics.</summary>
		public bool IsMultiSelect;
		#endregion

		///<summary></summary>
		public FormClinics()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClinics));
			this.groupClinicOrder = new System.Windows.Forms.GroupBox();
			this.butUp = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.checkOrderAlphabetical = new System.Windows.Forms.CheckBox();
			this.groupMovePats = new System.Windows.Forms.GroupBox();
			this.butMovePats = new OpenDental.UI.Button();
			this.butClinicPick = new OpenDental.UI.Button();
			this.textMoveTo = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.butOK = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.butAdd = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.butSelectAll = new OpenDental.UI.Button();
			this.butSelectNone = new OpenDental.UI.Button();
			this.groupClinicOrder.SuspendLayout();
			this.groupMovePats.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupClinicOrder
			// 
			this.groupClinicOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupClinicOrder.Controls.Add(this.butUp);
			this.groupClinicOrder.Controls.Add(this.butDown);
			this.groupClinicOrder.Controls.Add(this.checkOrderAlphabetical);
			this.groupClinicOrder.Location = new System.Drawing.Point(636, 126);
			this.groupClinicOrder.Name = "groupClinicOrder";
			this.groupClinicOrder.Size = new System.Drawing.Size(266, 91);
			this.groupClinicOrder.TabIndex = 20;
			this.groupClinicOrder.TabStop = false;
			this.groupClinicOrder.Text = "Clinic Order";
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(6, 19);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(75, 26);
			this.butUp.TabIndex = 4;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butDown
			// 
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(6, 54);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(75, 26);
			this.butDown.TabIndex = 5;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// checkOrderAlphabetical
			// 
			this.checkOrderAlphabetical.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkOrderAlphabetical.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkOrderAlphabetical.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkOrderAlphabetical.Location = new System.Drawing.Point(128, 40);
			this.checkOrderAlphabetical.Name = "checkOrderAlphabetical";
			this.checkOrderAlphabetical.Size = new System.Drawing.Size(132, 18);
			this.checkOrderAlphabetical.TabIndex = 16;
			this.checkOrderAlphabetical.Text = "Order Alphabetical";
			this.checkOrderAlphabetical.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkOrderAlphabetical.UseVisualStyleBackColor = true;
			this.checkOrderAlphabetical.Click += new System.EventHandler(this.checkOrderAlphabetical_Click);
			// 
			// groupMovePats
			// 
			this.groupMovePats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupMovePats.Controls.Add(this.butMovePats);
			this.groupMovePats.Controls.Add(this.butClinicPick);
			this.groupMovePats.Controls.Add(this.textMoveTo);
			this.groupMovePats.Controls.Add(this.label2);
			this.groupMovePats.Location = new System.Drawing.Point(636, 37);
			this.groupMovePats.Name = "groupMovePats";
			this.groupMovePats.Size = new System.Drawing.Size(266, 83);
			this.groupMovePats.TabIndex = 18;
			this.groupMovePats.TabStop = false;
			this.groupMovePats.Text = "Move Patients";
			// 
			// butMovePats
			// 
			this.butMovePats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butMovePats.Location = new System.Drawing.Point(185, 46);
			this.butMovePats.Name = "butMovePats";
			this.butMovePats.Size = new System.Drawing.Size(75, 26);
			this.butMovePats.TabIndex = 15;
			this.butMovePats.Text = "&Move";
			this.butMovePats.UseVisualStyleBackColor = true;
			this.butMovePats.Click += new System.EventHandler(this.butMovePats_Click);
			// 
			// butClinicPick
			// 
			this.butClinicPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butClinicPick.Location = new System.Drawing.Point(237, 20);
			this.butClinicPick.Name = "butClinicPick";
			this.butClinicPick.Size = new System.Drawing.Size(23, 20);
			this.butClinicPick.TabIndex = 23;
			this.butClinicPick.Text = "...";
			this.butClinicPick.Click += new System.EventHandler(this.butClinicPick_Click);
			// 
			// textMoveTo
			// 
			this.textMoveTo.Location = new System.Drawing.Point(98, 20);
			this.textMoveTo.MaxLength = 15;
			this.textMoveTo.Name = "textMoveTo";
			this.textMoveTo.ReadOnly = true;
			this.textMoveTo.Size = new System.Drawing.Size(135, 20);
			this.textMoveTo.TabIndex = 22;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 21);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(91, 18);
			this.label2.TabIndex = 18;
			this.label2.Text = "To Clinic";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowHidden.Checked = true;
			this.checkShowHidden.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowHidden.Location = new System.Drawing.Point(506, 12);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.Size = new System.Drawing.Size(124, 18);
			this.checkShowHidden.TabIndex = 17;
			this.checkShowHidden.Text = "Show Hidden";
			this.checkShowHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowHidden.UseVisualStyleBackColor = true;
			this.checkShowHidden.CheckedChanged += new System.EventHandler(this.checkShowHidden_CheckedChanged);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(827, 541);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 13;
			this.butOK.Text = "&OK";
			this.butOK.Visible = false;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(12, 37);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(618, 562);
			this.gridMain.TabIndex = 0;
			this.gridMain.Title = "Clinics";
			this.gridMain.TranslationName = "TableClinics";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(351, 18);
			this.label1.TabIndex = 11;
			this.label1.Text = "This is usually only used if you have multiple locations";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butAdd
			// 
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(827, 223);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 26);
			this.butAdd.TabIndex = 10;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(827, 573);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 1;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butSelectAll
			// 
			this.butSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butSelectAll.Location = new System.Drawing.Point(636, 311);
			this.butSelectAll.Name = "butSelectAll";
			this.butSelectAll.Size = new System.Drawing.Size(81, 26);
			this.butSelectAll.TabIndex = 21;
			this.butSelectAll.Text = "Select All";
			this.butSelectAll.UseVisualStyleBackColor = true;
			this.butSelectAll.Visible = false;
			this.butSelectAll.Click += new System.EventHandler(this.butSelectAll_Click);
			// 
			// butSelectNone
			// 
			this.butSelectNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butSelectNone.Location = new System.Drawing.Point(636, 343);
			this.butSelectNone.Name = "butSelectNone";
			this.butSelectNone.Size = new System.Drawing.Size(81, 26);
			this.butSelectNone.TabIndex = 22;
			this.butSelectNone.Text = "Select None";
			this.butSelectNone.UseVisualStyleBackColor = true;
			this.butSelectNone.Visible = false;
			this.butSelectNone.Click += new System.EventHandler(this.butSelectNone_Click);
			// 
			// FormClinics
			// 
			this.ClientSize = new System.Drawing.Size(914, 611);
			this.Controls.Add(this.butSelectNone);
			this.Controls.Add(this.butSelectAll);
			this.Controls.Add(this.groupClinicOrder);
			this.Controls.Add(this.groupMovePats);
			this.Controls.Add(this.checkShowHidden);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(930, 650);
			this.Name = "FormClinics";
			this.ShowInTaskbar = false;
			this.Text = "Clinics";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormClinics_Closing);
			this.Load += new System.EventHandler(this.FormClinics_Load);
			this.groupClinicOrder.ResumeLayout(false);
			this.groupMovePats.ResumeLayout(false);
			this.groupMovePats.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormClinics_Load(object sender, System.EventArgs e) {
			checkOrderAlphabetical.Checked=PrefC.GetBool(PrefName.ClinicListIsAlphabetical);
			if(ListClinics==null) {
				ListClinics=Clinics.GetAllForUserod(Security.CurUser);
				if(IncludeHQInList) {
					ListClinics.Insert(0,new Clinic() { ClinicNum=0,Description=Lan.g(this,"Headquarters"),Abbr=Lan.g(this,"HQ") });
				}
				//if alphabetical checkbox is checked/unchecked it triggers a pref cache refresh, but does not refill the clinic cache, so we need to sort here
				//in case the pref was changed since last time this was opened.
				ListClinics.Sort(ClinicSort);
			}
			_listClinicsOld=ListClinics.Select(x => x.Copy()).ToList();
			_listClinicDefLinksAllOld=DefLinks.GetDefLinksByType(DefLinkType.Clinic);
			_dictClinicalCounts=new SerializableDictionary<long,int>();
			if(IsSelectionMode) {
				butAdd.Visible=false;
				butOK.Visible=true;
				groupClinicOrder.Visible=false;
				groupMovePats.Visible=false;
				int widthDiff=(groupClinicOrder.Width-butOK.Width);
				MinimumSize=new Size(MinimumSize.Width-widthDiff,MinimumSize.Height);
				Width-=widthDiff;
				gridMain.Width+=widthDiff;
				butSelectAll.Location=new Point(butSelectAll.Location.X+widthDiff,butSelectAll.Location.Y);
				butSelectNone.Location=new Point(butSelectNone.Location.X+widthDiff,butSelectNone.Location.Y);
				checkShowHidden.Visible=false;
				checkShowHidden.Checked=false;
			}
			else {
				if(checkOrderAlphabetical.Checked) {
					butUp.Enabled=false;
					butDown.Enabled=false;
				}
				_dictClinicalCounts=Clinics.GetClinicalPatientCount();
			}
			if(IsMultiSelect) {
				butSelectAll.Visible=true;
				butSelectNone.Visible=true;
				gridMain.SelectionMode=GridSelectionMode.MultiExtended;
			}
			FillGrid(false);
			if(!ListSelectedClinicNums.IsNullOrEmpty()) {
				for(int i=0;i<gridMain.ListGridRows.Count;i++) {
					if(ListSelectedClinicNums.Contains(((Clinic)gridMain.ListGridRows[i].Tag).ClinicNum)) {
						gridMain.SetSelected(i,true);
					}
				}
			}
		}

		private void FillGrid(bool doReselctRows=true) {
			List<long> listSelectedClinicNums=new List<long>();
			if(doReselctRows) {
				listSelectedClinicNums=gridMain.SelectedTags<Clinic>().Select(x => x.ClinicNum).ToList();
			}
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableClinics","Abbr"),120));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableClinics","Description"),200));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableClinics","Specialty"),150));
			if(!IsSelectionMode) {
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableClinics","Pat Count"),80,HorizontalAlignment.Center));
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableClinics","Hidden"),0,HorizontalAlignment.Center));
			}
			gridMain.ListGridRows.Clear();
			GridRow row;
			Dictionary<long,string> dictClinicSpecialtyDescripts=Defs.GetDefsForCategory(DefCat.ClinicSpecialty).ToDictionary(x => x.DefNum,x => x.ItemName);
			List<int> listIndicesToReselect=new List<int>();
			foreach(Clinic clinCur in ListClinics) {
				if(!checkShowHidden.Checked && clinCur.IsHidden) {
					continue;
				}
				row=new GridRow();
				row.Cells.Add((clinCur.ClinicNum==0?"":clinCur.Abbr));
				row.Cells.Add(clinCur.Description);
				string specialty="";
				string specialties=string.Join(",",clinCur.ListClinicSpecialtyDefLinks
					.Select(x => dictClinicSpecialtyDescripts.TryGetValue(x.DefNum,out specialty)?specialty:"")
					.Where(x => !string.IsNullOrWhiteSpace(x)));
				row.Cells.Add(specialties);
				if(!IsSelectionMode) {//selection mode means no IsHidden or Pat Count columns
					int patCount=0;
					_dictClinicalCounts.TryGetValue(clinCur.ClinicNum,out patCount);
					row.Cells.Add(POut.Int(patCount));
					row.Cells.Add(clinCur.IsHidden?"X":"");
				}
				row.Tag=clinCur;
				gridMain.ListGridRows.Add(row);
				if(listSelectedClinicNums.Contains(clinCur.ClinicNum)) {
					listIndicesToReselect.Add(gridMain.ListGridRows.Count-1);
				}
			}
			gridMain.EndUpdate();
			if(doReselctRows && listIndicesToReselect.Count>0) {
				gridMain.SetSelected(listIndicesToReselect.ToArray(),true);
			}
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			Clinic clinicCur=new Clinic();
			clinicCur.IsNew=true;
			if(PrefC.GetBool(PrefName.PracticeIsMedicalOnly)) {
				clinicCur.IsMedicalOnly=true;
			}
			clinicCur.ItemOrder=gridMain.ListGridRows.Count-(IncludeHQInList?1:0);//Set it last in the last position (minus 1 for HQ)
			FormClinicEdit FormCE=new FormClinicEdit(clinicCur);
			if(FormCE.ShowDialog()==DialogResult.OK) {
				clinicCur.ClinicNum=Clinics.Insert(clinicCur);//inserting this here so we have a ClinicNum; the user cannot cancel and undo this anyway
				//ClinicCur.ListClinicSpecialtyDefLinks FKeys are set in FormClosing to ClinicCur.ClinicNum
				ListClinics.Add(clinicCur);
				_listClinicsOld.Add(clinicCur.Copy());//add to both lists so the sync doesn't try to insert it again or delete it.
				_dictClinicalCounts[clinicCur.ClinicNum]=0;
				ListClinics.Sort(ClinicSort);
			}
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridMain.ListGridRows.Count==0){
				return;
			}
			if(IsSelectionMode) {
				SelectedClinicNum=((Clinic)gridMain.ListGridRows[e.Row].Tag).ClinicNum;
				DialogResult=DialogResult.OK;
				return;
			}
			if(IncludeHQInList && e.Row==0) {
				return;
			}
			Clinic clinicOld=(Clinic)gridMain.ListGridRows[e.Row].Tag;
			FormClinicEdit FormCE=new FormClinicEdit(clinicOld.Copy());
			if(FormCE.ShowDialog()==DialogResult.OK) {
				Clinic clinicNew=FormCE.ClinicCur;
				if(clinicNew==null) {//Clinic was deleted
					//Fix ItemOrders
					ListClinics.FindAll(x => x.ItemOrder>clinicOld.ItemOrder)
						.ForEach(x => x.ItemOrder--);
					ListClinics.Remove(clinicOld);
				}
				else {
					ListClinics[ListClinics.IndexOf(clinicOld)]=clinicNew;
				}
			}
			FillGrid();			
		}
		
		private void butClinicPick_Click(object sender,EventArgs e) {
			List<Clinic> listClinics=gridMain.GetTags<Clinic>();
			FormClinics formC=new FormClinics();
			formC.ListClinics=listClinics;
			formC.IsSelectionMode=true;
			if(formC.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_clinicNumTo=formC.SelectedClinicNum;
			textMoveTo.Text=(listClinics.FirstOrDefault(x => x.ClinicNum==_clinicNumTo)?.Abbr??"");
		}

		private void butMovePats_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length<1) {
				MsgBox.Show(this,"You must select at least one clinic to move patients from.");
				return;
			}
			if(_clinicNumTo==-1){
				MsgBox.Show(this,"You must pick a 'To' clinic in the box above to move patients to.");
				return;
			}
			Dictionary<long,Clinic> dictClinicsFrom=gridMain.SelectedTags<Clinic>().ToDictionary(x => x.ClinicNum);
			Clinic clinicTo=gridMain.GetTags<Clinic>().FirstOrDefault(x => x.ClinicNum==_clinicNumTo);
			if(clinicTo==null) {
				MsgBox.Show(this,"The clinic could not be found.");
				return;
			}
			if(dictClinicsFrom.ContainsKey(clinicTo.ClinicNum)) {
				MsgBox.Show(this,"The 'To' clinic should not also be one of the 'From' clinics.");
				return;
			}
			Dictionary<long,int> dictClinFromCounts=Clinics.GetClinicalPatientCount(true)
				.Where(x => dictClinicsFrom.ContainsKey(x.Key)).ToDictionary(x => x.Key,x => x.Value);
			if(dictClinFromCounts.Sum(x => x.Value)==0) {
				MsgBox.Show(this,"There are no patients assigned to the selected clinics.");
				return;
			}
			string msg=Lan.g(this,"This will move all patients to")+" "+clinicTo.Abbr+" "+Lan.g(this,"from the following clinics")+":\r\n"
				+string.Join("\r\n",dictClinFromCounts.Select(x => dictClinicsFrom[x.Key].Abbr))+"\r\n"+Lan.g(this,"Continue?");
			if(MessageBox.Show(msg,"",MessageBoxButtons.YesNo)!=DialogResult.Yes) {
				return;
			}
			ODProgress.ShowAction(() => {
					int patsMoved=0;
					List<Action> listActions=dictClinFromCounts.Select(x => new Action(() => {
						Patients.ChangeClinicsForAll(x.Key,clinicTo.ClinicNum);//update all clinicNums to new clinic
						Clinic clinicCur;
						SecurityLogs.MakeLogEntry(Permissions.PatientEdit,0,"Clinic changed for "+x.Value+" patients from "
							+(dictClinicsFrom.TryGetValue(x.Key,out clinicCur)?clinicCur.Abbr:"")+" to "+clinicTo.Abbr+".");
						patsMoved+=x.Value;
						ClinicEvent.Fire(ODEventType.Clinic,Lan.g(this,"Moved patients")+": "+patsMoved+" "+Lan.g(this,"out of")+" "
							+dictClinFromCounts.Sum(y => y.Value));
					})).ToList();
					ODThread.RunParallel(listActions,TimeSpan.FromMinutes(2));
				},
				startingMessage:Lan.g(this,"Moving patients")+"...",
				eventType:typeof(ClinicEvent),
				odEventType:ODEventType.Clinic);
			_dictClinicalCounts=Clinics.GetClinicalPatientCount();
			FillGrid();
			MsgBox.Show(this,"Done");
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a clinic first.");
				return;
			}
			int selectedIdx=gridMain.GetSelectedIndex();
			//Already at the top of the list or the clinic just below the HQ 'clinic' is selected, moving up does nothing
			if(selectedIdx==0 || (IncludeHQInList && selectedIdx==1)) {
				return;
			}
			//Swap clinic ItemOrders
			Clinic sourceClin=((Clinic)gridMain.ListGridRows[selectedIdx].Tag);
			Clinic destClin=((Clinic)gridMain.ListGridRows[selectedIdx-1].Tag);
			if(sourceClin.ItemOrder==destClin.ItemOrder) {
				sourceClin.ItemOrder--;
			}
			else {
				int sourceOrder=sourceClin.ItemOrder;
				sourceClin.ItemOrder=destClin.ItemOrder;
				destClin.ItemOrder=sourceOrder;
			}
			//Move selected clinic up
			ListClinics.Sort(ClinicSort);
			FillGrid();
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a clinic first.");
				return;
			}
			int selectedIdx=gridMain.GetSelectedIndex();
			//Already at the bottom of the list or the HQ 'clinic' is selected, moving down does nothing
			if(selectedIdx==gridMain.ListGridRows.Count-1 || (IncludeHQInList && selectedIdx==0)) {
				return;
			}
			//Swap clinic ItemOrders
			Clinic sourceClin=((Clinic)gridMain.ListGridRows[selectedIdx].Tag);
			Clinic destClin=((Clinic)gridMain.ListGridRows[selectedIdx+1].Tag);
			if(sourceClin.ItemOrder==destClin.ItemOrder) {//just in case, an issue in the past would cause ItemOrder inconsitencies, shouldn't happen
				sourceClin.ItemOrder++;
			}
			else {
				int sourceOrder=sourceClin.ItemOrder;
				sourceClin.ItemOrder=destClin.ItemOrder;
				destClin.ItemOrder=sourceOrder;
			}
			//Move selected clinic down
			ListClinics.Sort(ClinicSort);
			FillGrid();
		}

		private void checkOrderAlphabetical_Click(object sender,EventArgs e) {
			if(checkOrderAlphabetical.Checked) {
				butUp.Enabled=false;
				butDown.Enabled=false;
			}
			else {
				butUp.Enabled=true;
				butDown.Enabled=true;
			}
			ListClinics.Sort(ClinicSort);//Sorts based on the status of the checkbox.
			FillGrid();
		}

		private int ClinicSort(Clinic x,Clinic y) {
			if(IncludeHQInList) {//always keep the HQ clinic at the top if it's in the list
				if(x.ClinicNum==0) {
					return -1;
				}
				if(y.ClinicNum==0) {
					return 1;
				}
			}
			int retval=0;
			if(checkOrderAlphabetical.Checked) {//order alphabetical by Abbr
				retval=x.Abbr.CompareTo(y.Abbr);
			}
			else {//not alphabetical, order by ItemOrder
				retval=x.ItemOrder.CompareTo(y.ItemOrder);
			}
			if(retval==0) {//if Abbr's are alphabetically the same or ItemOrder's are the same, order alphabetical by Description
				retval=x.Description.CompareTo(y.Description);
			}
			if(retval==0) {//if Abbrs/ItemOrders are the same and Descriptions are alphabetically the same, order by ClinicNum (guaranteed deterministic)
				retval=x.ClinicNum.CompareTo(y.ClinicNum);
			}
			return retval;
		}

		///<summary>Does nothing and returns false if checkOrderAlphabetical is checked.  Uses ClinicSort to put the clinics in the correct order and then
		///updates the ItemOrder for all clinics.  Includes hidden clinics and clinics the user does not have permission to access.  It must include all
		///clinics for the ordering to be correct for all users.  This method corrects item ordering issues that were caused by past code and is just a
		///precaution.  After this runs once, there shouldn't be any ItemOrder inconsistencies moving forward, so this should generally just return false.
		///Returns true if the db was changed.</summary>
		private bool CorrectItemOrders() {
			if(checkOrderAlphabetical.Checked) {
				return false;
			}
			List<Clinic> listAllClinicsDb=Clinics.GetClinicsNoCache();//get all clinics, even hidden ones, in order to set the ItemOrders correctly
			List<Clinic> listAllClinicsNew=listAllClinicsDb.Select(x => x.Copy()).ToList();
			bool isHqInList=IncludeHQInList;
			IncludeHQInList=false;
			listAllClinicsNew.Sort(ClinicSort);
			IncludeHQInList=isHqInList;
			for(int i=0;i<listAllClinicsNew.Count;i++) {
				listAllClinicsNew[i].ItemOrder=i+1;//1 based ItemOrder because the HQ 'clinic' has ItemOrder 0
			}
			return Clinics.Sync(listAllClinicsNew,listAllClinicsDb);
		}

		private void checkShowHidden_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void butSelectAll_Click(object sender,EventArgs e) {
			gridMain.SetSelected(true);
			gridMain.Focus();//Allows user to use ODGrid CTRL functionality.
		}

		private void butSelectNone_Click(object sender,EventArgs e) {
			gridMain.SetSelected(false);
			gridMain.Focus();//Allows user to use ODGrid CTRL functionality.
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(IsSelectionMode && gridMain.SelectedIndices.Length>0) {
				SelectedClinicNum=gridMain.SelectedTag<Clinic>()?.ClinicNum??0;
				ListSelectedClinicNums=gridMain.SelectedTags<Clinic>().Select(x => x.ClinicNum).ToList();
				DialogResult=DialogResult.OK;
			}
			Close();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			SelectedClinicNum=0;
			ListSelectedClinicNums=new List<long>();
			Close();
		}

		private void FormClinics_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(IsSelectionMode) {
				return;
			}
			if(Prefs.UpdateBool(PrefName.ClinicListIsAlphabetical,checkOrderAlphabetical.Checked)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			bool hasClinicChanges=Clinics.Sync(ListClinics,_listClinicsOld);//returns true if clinics were updated/inserted/deleted
			//Update the ClinicNum on all specialties associated to each clinic.
			ListClinics.ForEach(x => x.ListClinicSpecialtyDefLinks.ForEach(y => y.FKey=x.ClinicNum));
			List<DefLink> listAllClinicSpecialtyDefLinks=ListClinics.SelectMany(x => x.ListClinicSpecialtyDefLinks).ToList();
			hasClinicChanges|=DefLinks.Sync(listAllClinicSpecialtyDefLinks,_listClinicDefLinksAllOld);
			hasClinicChanges|=CorrectItemOrders();
			//Joe - Now that we have called sync on ListClinics we want to make sure that each clinic has program properties for PayConnect and XCharge
			//We are doing this because of a previous bug that caused some customers to have over 3.4 million duplicate rows in their programproperty table
			long payConnectProgNum=Programs.GetProgramNum(ProgramName.PayConnect);
			long xChargeProgNum=Programs.GetProgramNum(ProgramName.Xcharge);
			//Don't need to do this for PaySimple, because these will get generated as needed in FormPaySimpleSetup
			bool hasChanges=ProgramProperties.InsertForClinic(payConnectProgNum,
				ListClinics.Select(x => x.ClinicNum).Where(x => ProgramProperties.GetListForProgramAndClinic(payConnectProgNum,x).Count==0).ToList());
			hasChanges|=ProgramProperties.InsertForClinic(xChargeProgNum,
				ListClinics.Select(x => x.ClinicNum).Where(x => ProgramProperties.GetListForProgramAndClinic(xChargeProgNum,x).Count==0).ToList());
			if(hasChanges) {
				DataValid.SetInvalid(InvalidType.Programs);
			}
			if(hasClinicChanges) {
				DataValid.SetInvalid(InvalidType.Providers);
			}
		}
	}
}





















