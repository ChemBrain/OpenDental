using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.Thinfinity;
using OpenDental.UI;
using OpenDentBusiness;
using PdfSharp.Pdf;

namespace OpenDental{
///<summary></summary>
	public class FormRpTreatmentFinder:ODForm {
		private OpenDental.UI.Button butCancel;
		private System.ComponentModel.Container components = null;
		private Label label1;
		private CheckBox checkIncludeNoIns;
		private UI.ODGrid gridMain;
		private GroupBox groupBox1;
		private UI.Button butPrint;
		private UI.Button butRefresh;
		private bool headingPrinted;
		private int headingPrintH;
		private ContextMenu contextRightClick;
		private MenuItem menuItemFamily;
		private MenuItem menuItemAccount;
		private UI.Button butGotoFamily;
		private UI.Button butGotoAccount;
		private ValidDate textDateStart;
		private Label label2;
		private Label label4;
		private Label label3;
		private Label labelCodeRange;
		private UI.Button butLabelSingle;
		private UI.Button butLabelPreview;
		private Label label7;
		private UI.Button butLettersPreview;
		private UI.Button buttonExport;
		private Label label8;
		private ComboBox comboMonthStart;
		private ValidDouble textOverAmount;
		private ComboBoxMulti comboBoxMultiProv;
		private ComboBoxMulti comboBoxMultiBilling;
		private int pagesPrinted;
		private CheckBox checkIncludePatsWithApts;
		private int patientsPrinted;
		private List<Provider> _listProviders;
		private CheckBox checkBenefitAssumeGeneral;
		private ODCodeRangeFilter codeRangeFilter;
		private ComboBoxClinicPicker comboClinics;
		///<summary></summary>
		public FormRpTreatmentFinder() {
			InitializeComponent();
			Lan.F(this);
			gridMain.ContextMenu=contextRightClick;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpTreatmentFinder));
			this.label1 = new System.Windows.Forms.Label();
			this.checkIncludeNoIns = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.comboClinics = new OpenDental.UI.ComboBoxClinicPicker();
			this.codeRangeFilter = new OpenDental.UI.ODCodeRangeFilter();
			this.checkBenefitAssumeGeneral = new System.Windows.Forms.CheckBox();
			this.comboBoxMultiBilling = new OpenDental.UI.ComboBoxMulti();
			this.comboBoxMultiProv = new OpenDental.UI.ComboBoxMulti();
			this.textOverAmount = new OpenDental.ValidDouble();
			this.comboMonthStart = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.labelCodeRange = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textDateStart = new OpenDental.ValidDate();
			this.label2 = new System.Windows.Forms.Label();
			this.butRefresh = new OpenDental.UI.Button();
			this.checkIncludePatsWithApts = new System.Windows.Forms.CheckBox();
			this.contextRightClick = new System.Windows.Forms.ContextMenu();
			this.menuItemFamily = new System.Windows.Forms.MenuItem();
			this.menuItemAccount = new System.Windows.Forms.MenuItem();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.buttonExport = new OpenDental.UI.Button();
			this.butLettersPreview = new OpenDental.UI.Button();
			this.butLabelSingle = new OpenDental.UI.Button();
			this.butLabelPreview = new OpenDental.UI.Button();
			this.butGotoAccount = new OpenDental.UI.Button();
			this.butGotoFamily = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(22, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(872, 29);
			this.label1.TabIndex = 29;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// checkIncludeNoIns
			// 
			this.checkIncludeNoIns.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeNoIns.Location = new System.Drawing.Point(31, 14);
			this.checkIncludeNoIns.Name = "checkIncludeNoIns";
			this.checkIncludeNoIns.Size = new System.Drawing.Size(214, 18);
			this.checkIncludeNoIns.TabIndex = 30;
			this.checkIncludeNoIns.Text = "Include patients without insurance";
			this.checkIncludeNoIns.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeNoIns.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboClinics);
			this.groupBox1.Controls.Add(this.codeRangeFilter);
			this.groupBox1.Controls.Add(this.checkBenefitAssumeGeneral);
			this.groupBox1.Controls.Add(this.comboBoxMultiBilling);
			this.groupBox1.Controls.Add(this.comboBoxMultiProv);
			this.groupBox1.Controls.Add(this.textOverAmount);
			this.groupBox1.Controls.Add(this.comboMonthStart);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.labelCodeRange);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.textDateStart);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.butRefresh);
			this.groupBox1.Controls.Add(this.checkIncludePatsWithApts);
			this.groupBox1.Controls.Add(this.checkIncludeNoIns);
			this.groupBox1.Location = new System.Drawing.Point(5, 41);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(1040, 83);
			this.groupBox1.TabIndex = 33;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "View";
			// 
			// comboClinics
			// 
			this.comboClinics.IncludeUnassigned = true;
			this.comboClinics.Location = new System.Drawing.Point(478, 54);
			this.comboClinics.Name = "comboClinics";
			this.comboClinics.Size = new System.Drawing.Size(197, 21);
			this.comboClinics.TabIndex = 77;
			// 
			// codeRangeFilter
			// 
			this.codeRangeFilter.HideExampleText = false;
			this.codeRangeFilter.Location = new System.Drawing.Point(755, 11);
			this.codeRangeFilter.MaximumSize = new System.Drawing.Size(300, 37);
			this.codeRangeFilter.MinimumSize = new System.Drawing.Size(0, 20);
			this.codeRangeFilter.Name = "codeRangeFilter";
			this.codeRangeFilter.Size = new System.Drawing.Size(150, 37);
			this.codeRangeFilter.TabIndex = 76;
			// 
			// checkBenefitAssumeGeneral
			// 
			this.checkBenefitAssumeGeneral.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBenefitAssumeGeneral.Location = new System.Drawing.Point(682, 50);
			this.checkBenefitAssumeGeneral.Name = "checkBenefitAssumeGeneral";
			this.checkBenefitAssumeGeneral.Size = new System.Drawing.Size(226, 18);
			this.checkBenefitAssumeGeneral.TabIndex = 75;
			this.checkBenefitAssumeGeneral.Text = "Assume procedures are General";
			this.checkBenefitAssumeGeneral.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBenefitAssumeGeneral.UseVisualStyleBackColor = true;
			// 
			// comboBoxMultiBilling
			// 
			this.comboBoxMultiBilling.ArraySelectedIndices = new int[0];
			this.comboBoxMultiBilling.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiBilling.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiBilling.Items")));
			this.comboBoxMultiBilling.Location = new System.Drawing.Point(515, 32);
			this.comboBoxMultiBilling.Name = "comboBoxMultiBilling";
			this.comboBoxMultiBilling.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiBilling.SelectedIndices")));
			this.comboBoxMultiBilling.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiBilling.TabIndex = 50;
			this.comboBoxMultiBilling.Leave += new System.EventHandler(this.comboBoxMultiBilling_Leave);
			// 
			// comboBoxMultiProv
			// 
			this.comboBoxMultiProv.ArraySelectedIndices = new int[0];
			this.comboBoxMultiProv.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiProv.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiProv.Items")));
			this.comboBoxMultiProv.Location = new System.Drawing.Point(515, 10);
			this.comboBoxMultiProv.Name = "comboBoxMultiProv";
			this.comboBoxMultiProv.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiProv.SelectedIndices")));
			this.comboBoxMultiProv.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiProv.TabIndex = 49;
			this.comboBoxMultiProv.Leave += new System.EventHandler(this.comboBoxMultiProv_Leave);
			// 
			// textOverAmount
			// 
			this.textOverAmount.Location = new System.Drawing.Point(177, 55);
			this.textOverAmount.MaxVal = 100000000D;
			this.textOverAmount.MinVal = -100000000D;
			this.textOverAmount.Name = "textOverAmount";
			this.textOverAmount.Size = new System.Drawing.Size(68, 20);
			this.textOverAmount.TabIndex = 48;
			// 
			// comboMonthStart
			// 
			this.comboMonthStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboMonthStart.Items.AddRange(new object[] {
            "Calendar Year",
            "01 - January",
            "02 - February",
            "03 - March",
            "04 - April",
            "05 - May",
            "06 - June",
            "07 - July",
            "08 - August",
            "09 - September",
            "10 - October",
            "11 - November",
            "12 - December"});
			this.comboMonthStart.Location = new System.Drawing.Point(346, 32);
			this.comboMonthStart.MaxDropDownItems = 40;
			this.comboMonthStart.Name = "comboMonthStart";
			this.comboMonthStart.Size = new System.Drawing.Size(98, 21);
			this.comboMonthStart.TabIndex = 47;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(31, 58);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(140, 14);
			this.label8.TabIndex = 46;
			this.label8.Text = "Amount remaining over";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(445, 35);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(70, 14);
			this.label7.TabIndex = 43;
			this.label7.Text = "Billing Type";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(249, 36);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(93, 14);
			this.label3.TabIndex = 37;
			this.label3.Text = "Ins Month Start";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelCodeRange
			// 
			this.labelCodeRange.Location = new System.Drawing.Point(679, 12);
			this.labelCodeRange.Name = "labelCodeRange";
			this.labelCodeRange.Size = new System.Drawing.Size(77, 17);
			this.labelCodeRange.TabIndex = 40;
			this.labelCodeRange.Text = "Code Range";
			this.labelCodeRange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(445, 14);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 14);
			this.label4.TabIndex = 35;
			this.label4.Text = "Provider";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateStart
			// 
			this.textDateStart.Location = new System.Drawing.Point(367, 11);
			this.textDateStart.Name = "textDateStart";
			this.textDateStart.Size = new System.Drawing.Size(77, 20);
			this.textDateStart.TabIndex = 34;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(246, 14);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(119, 14);
			this.label2.TabIndex = 33;
			this.label2.Text = "TP Date Since";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butRefresh
			// 
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Location = new System.Drawing.Point(346, 55);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(98, 24);
			this.butRefresh.TabIndex = 32;
			this.butRefresh.Text = "&Refresh List";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// checkIncludePatsWithApts
			// 
			this.checkIncludePatsWithApts.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludePatsWithApts.Location = new System.Drawing.Point(6, 33);
			this.checkIncludePatsWithApts.Name = "checkIncludePatsWithApts";
			this.checkIncludePatsWithApts.Size = new System.Drawing.Size(239, 18);
			this.checkIncludePatsWithApts.TabIndex = 30;
			this.checkIncludePatsWithApts.Text = "Include patients with upcoming appointments";
			this.checkIncludePatsWithApts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludePatsWithApts.UseVisualStyleBackColor = true;
			// 
			// contextRightClick
			// 
			this.contextRightClick.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFamily,
            this.menuItemAccount});
			// 
			// menuItemFamily
			// 
			this.menuItemFamily.Index = 0;
			this.menuItemFamily.Text = "See Family";
			this.menuItemFamily.Click += new System.EventHandler(this.menuItemFamily_Click);
			// 
			// menuItemAccount
			// 
			this.menuItemAccount.Index = 1;
			this.menuItemAccount.Text = "See Account";
			this.menuItemAccount.Click += new System.EventHandler(this.menuItemAccount_Click);
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(3, 130);
			this.gridMain.Name = "gridMain";
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(1043, 453);
			this.gridMain.TabIndex = 31;
			this.gridMain.Title = "Treatment Finder";
			this.gridMain.TranslationName = "TableTreatmentFinder";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// buttonExport
			// 
			this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonExport.Location = new System.Drawing.Point(7, 613);
			this.buttonExport.Name = "buttonExport";
			this.buttonExport.Size = new System.Drawing.Size(119, 24);
			this.buttonExport.TabIndex = 72;
			this.buttonExport.Text = "Export to File";
			this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
			// 
			// butLettersPreview
			// 
			this.butLettersPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butLettersPreview.Image = global::OpenDental.Properties.Resources.butPreview;
			this.butLettersPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butLettersPreview.Location = new System.Drawing.Point(7, 587);
			this.butLettersPreview.Name = "butLettersPreview";
			this.butLettersPreview.Size = new System.Drawing.Size(119, 24);
			this.butLettersPreview.TabIndex = 71;
			this.butLettersPreview.Text = "Letters Preview";
			this.butLettersPreview.Click += new System.EventHandler(this.butLettersPreview_Click);
			// 
			// butLabelSingle
			// 
			this.butLabelSingle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butLabelSingle.Image = global::OpenDental.Properties.Resources.butLabel;
			this.butLabelSingle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butLabelSingle.Location = new System.Drawing.Point(132, 587);
			this.butLabelSingle.Name = "butLabelSingle";
			this.butLabelSingle.Size = new System.Drawing.Size(119, 24);
			this.butLabelSingle.TabIndex = 70;
			this.butLabelSingle.Text = "Single Labels";
			this.butLabelSingle.Click += new System.EventHandler(this.butLabelSingle_Click);
			// 
			// butLabelPreview
			// 
			this.butLabelPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butLabelPreview.Image = global::OpenDental.Properties.Resources.butLabel;
			this.butLabelPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butLabelPreview.Location = new System.Drawing.Point(132, 613);
			this.butLabelPreview.Name = "butLabelPreview";
			this.butLabelPreview.Size = new System.Drawing.Size(119, 24);
			this.butLabelPreview.TabIndex = 69;
			this.butLabelPreview.Text = "Label Preview";
			this.butLabelPreview.Click += new System.EventHandler(this.butLabelPreview_Click);
			// 
			// butGotoAccount
			// 
			this.butGotoAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butGotoAccount.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butGotoAccount.Location = new System.Drawing.Point(787, 613);
			this.butGotoAccount.Name = "butGotoAccount";
			this.butGotoAccount.Size = new System.Drawing.Size(96, 24);
			this.butGotoAccount.TabIndex = 68;
			this.butGotoAccount.Text = "Go to Account";
			this.butGotoAccount.Click += new System.EventHandler(this.butGotoAccount_Click);
			// 
			// butGotoFamily
			// 
			this.butGotoFamily.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butGotoFamily.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butGotoFamily.Location = new System.Drawing.Point(787, 587);
			this.butGotoFamily.Name = "butGotoFamily";
			this.butGotoFamily.Size = new System.Drawing.Size(96, 24);
			this.butGotoFamily.TabIndex = 67;
			this.butGotoFamily.Text = "Go to Family";
			this.butGotoFamily.Click += new System.EventHandler(this.butGotoFamily_Click);
			// 
			// butPrint
			// 
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(544, 613);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(87, 24);
			this.butPrint.TabIndex = 34;
			this.butPrint.Text = "Print List";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(970, 613);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormRpTreatmentFinder
			// 
			this.ClientSize = new System.Drawing.Size(1049, 641);
			this.Controls.Add(this.buttonExport);
			this.Controls.Add(this.butLettersPreview);
			this.Controls.Add(this.butLabelSingle);
			this.Controls.Add(this.butLabelPreview);
			this.Controls.Add(this.butGotoAccount);
			this.Controls.Add(this.butGotoFamily);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(1065, 309);
			this.Name = "FormRpTreatmentFinder";
			this.Text = "Treatment Finder";
			this.Load += new System.EventHandler(this.FormRpTreatmentFinder_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRpTreatmentFinder_Load(object sender, System.EventArgs e) {
			_listProviders=Providers.GetListReports();
			//DateTime today=DateTime.Today;
			//will start out 1st through 30th of previous month
			//date1.SelectionStart=new DateTime(today.Year,today.Month,1).AddMonths(-1);
			//date2.SelectionStart=new DateTime(today.Year,today.Month,1).AddDays(-1);
			comboBoxMultiProv.Items.Add(new ODBoxItem<Provider>("All",new Provider() { ProvNum = 0 }));
			for(int i=0;i<_listProviders.Count;i++){
			  comboBoxMultiProv.Items.Add(new ODBoxItem<Provider>(_listProviders[i].GetLongDesc(),_listProviders[i]));
			}
			comboBoxMultiProv.SetSelected(0,true);
			comboBoxMultiBilling.Items.Add(new ODBoxItem<Def>("All",new Def() { DefNum = 0 }));
			List<Def> listBillingTypeDefs=Defs.GetDefsForCategory(DefCat.BillingTypes,true);
			for(int i=0;i<listBillingTypeDefs.Count;i++){
				comboBoxMultiBilling.Items.Add(new ODBoxItem<Def>(listBillingTypeDefs[i].ItemName,listBillingTypeDefs[i]));
			}
			comboBoxMultiBilling.SetSelected(0,true);
			comboMonthStart.SelectedIndex=0;
			checkBenefitAssumeGeneral.Checked=PrefC.GetBool(PrefName.TreatFinderProcsAllGeneral);
			if(RemotingClient.RemotingRole!=RemotingRole.ClientWeb) {//for middle tier, don't allow mutiple clinics
				comboClinics.IncludeAll=true;
				comboClinics.SelectionModeMulti=true;
			}
			FillGrid();
		}

		private void FillGrid() {
			if(!string.IsNullOrEmpty(textDateStart.errorProvider1.GetError(textDateStart))
				|| !string.IsNullOrEmpty(textOverAmount.errorProvider1.GetError(textOverAmount)))
			{
				return;
			}
			DateTime dateSince=PIn.Date(textDateStart.Text);
			int monthStart=comboMonthStart.SelectedIndex;
			double aboveAmount=PIn.Double(textOverAmount.Text);
#if DEBUG
			Stopwatch sw=Stopwatch.StartNew();
#endif
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			//0=PatNum
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableTreatmentFinder","LName"),100));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableTreatmentFinder","FName"),100));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableTreatmentFinder","Contact"),120));
			//4=address
			//5=cityStateZip
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableTreatmentFinder","Annual Max"),80,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableTreatmentFinder","Amt Used"),70,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableTreatmentFinder","Amt Pend"),70,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableTreatmentFinder","Amt Rem"),70,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableTreatmentFinder","Treat Plan"),70,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableTreatmentFinder","Insurance Carrier"),225));
			if(PrefC.HasClinicsEnabled) {
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableTreatmentFinder","Clinic"),120));
			}
			gridMain.ListGridRows.Clear();
			Cursor=Cursors.WaitCursor;
			using(DataTable table=RpTreatmentFinder.GetTreatmentFinderList(checkIncludeNoIns.Checked,checkIncludePatsWithApts.Checked,monthStart,dateSince,
				aboveAmount,comboBoxMultiProv.SelectedTags<Provider>().Select(x => x.ProvNum).ToList(),
				comboBoxMultiBilling.SelectedTags<Def>().Select(x => x.DefNum).ToList(),codeRangeFilter.StartRange,codeRangeFilter.EndRange,
				comboClinics.ListSelectedClinicNums,checkBenefitAssumeGeneral.Checked))
			{
				GridRow row;
				foreach(DataRow rowCur in table.Rows) {
					row=new GridRow() { Tag=rowCur };
					double indMax=PIn.Double(rowCur[8].ToString());
					double famMax=PIn.Double(rowCur[9].ToString());
					//Temporary filter just showing columns wanted. Probably it will become user defined.
					for(int j=0;j<table.Columns.Count;j++) {
						//0- PatNum,4-address,5-city,6-State,7-Zip are just for the export, 9-AnnualMaxFam,11-AmtUsedFam,13-AmtPendingFam,15-AmtRemainingFam on new line
						if(j.In(0,4,5,6,7,9,11,13,15) || (j==18 && !PrefC.HasClinicsEnabled)) {
							continue;
						}
						string cellData=rowCur[j].ToString();
						if(j.In(8,10,12,14)) {//AnnualMax,AmtUsed,AmtPending,AmtRemaining
							cellData=(indMax<=0?"":("I: "+rowCur[j].ToString()+(famMax>0?"\r\n":"")))+(famMax<=0?"":("F: "+rowCur[j+1].ToString()));
						}
						row.Cells.Add(cellData);
					}
					gridMain.ListGridRows.Add(row);
				}
			}
			gridMain.EndUpdate();
#if DEBUG
			sw.Stop();
			Console.WriteLine("Finished fetching data and filling grid: {0}, Rows: {1}",(sw.Elapsed.Seconds==0?"":(sw.Elapsed.Seconds+" sec "))+(sw.Elapsed.TotalMilliseconds-(sw.Elapsed.Seconds*1000))+" ms",gridMain.ListGridRows.Count);
#endif
			Cursor=Cursors.Default;
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridMain.SelectedGridRows.Count==0) {//When deselecting with CTR+Click.
				return;
			}
			GotoModule.GotoChart(PIn.Long(gridMain.SelectedTag<DataRow>()["PatNum"].ToString()));
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			//Might not need cellDoubleClick
		}

		private void comboBoxMultiProv_Leave(object sender,EventArgs e) {
			if(comboBoxMultiProv.SelectedIndices.Count==0) {
				comboBoxMultiProv.SetSelected(0,true);
			}
			else if(comboBoxMultiProv.SelectedIndices.Contains(0)) {
				comboBoxMultiProv.SelectedIndicesClear();
				comboBoxMultiProv.SetSelected(0,true);
			}
		}

		private void comboBoxMultiBilling_Leave(object sender,EventArgs e) {
			if(comboBoxMultiBilling.SelectedIndices.Count==0) {
				comboBoxMultiBilling.SetSelected(0,true);
			}
			else if(comboBoxMultiBilling.SelectedIndices.Contains(0)) {
				comboBoxMultiBilling.SelectedIndicesClear();
				comboBoxMultiBilling.SetSelected(0,true);
			}
		}

		private void butLettersPreview_Click(object sender,EventArgs e) {
			//Create letters. loop through each row and insert information into sheets,
			//take all the sheets and add to one giant pdf for preview.
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select patient(s) first.");
				return;
			}
			FormSheetPicker FormS=new FormSheetPicker();
			FormS.SheetType=SheetTypeEnum.PatientLetter;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			SheetDef sheetDef;
			Sheet sheet=null;
			List<Sheet> listSheets=new List<Sheet>(); //for saving
			for(int i=0;i<FormS.SelectedSheetDefs.Count;i++) {
				PdfDocument document=new PdfDocument();
				PdfPage page=new PdfPage();
				string filePathAndName="";
				for(int j=0;j<gridMain.SelectedIndices.Length;j++) {
					sheetDef=FormS.SelectedSheetDefs[i];
					sheet=SheetUtil.CreateSheet(sheetDef,PIn.Long(((DataRow)gridMain.SelectedGridRows[j].Tag)["PatNum"].ToString()));
					SheetParameter.SetParameter(sheet,"PatNum",PIn.Long(((DataRow)gridMain.SelectedGridRows[j].Tag)["PatNum"].ToString()));
					SheetFiller.FillFields(sheet);
					sheet.SheetFields.Sort(SheetFields.SortDrawingOrderLayers);
					SheetUtil.CalculateHeights(sheet);
					SheetPrinting.PagesPrinted=0;//Clear out the pages printed variable before printing all pages for this pdf.
					int pageCount=Sheets.CalculatePageCount(sheet,SheetPrinting.PrintMargin);
					for(int k=0;k<pageCount;k++) {
						page=document.AddPage();
						SheetPrinting.CreatePdfPage(sheet,page,null);
					}
					listSheets.Add(sheet);
				}
				filePathAndName=PrefC.GetRandomTempFile(".pdf");
				document.Save(filePathAndName);
				if(ODBuild.IsWeb()) {
					ThinfinityUtils.HandleFile(filePathAndName);
				}
				else {
					Process.Start(filePathAndName);
				}
				DialogResult=DialogResult.OK;
			}
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Would you like to save the sheets for the selected patients?")) {
					Sheets.SaveNewSheetList(listSheets);
			}
		}

		private void butLabelSingle_Click(object sender,EventArgs e) {
		  if(gridMain.SelectedIndices.Length==0) {
		    MsgBox.Show(this,"Please select patient(s) first.");
		    return;
		  }
		  int patientsPrinted=0;
		  string text;
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				text="";
				//print single label
				DataRow curRow=(DataRow)gridMain.SelectedGridRows[i].Tag;
		    text=curRow["FName"].ToString()+" "+curRow["LName"].ToString()+"\r\n";
		    text+=curRow["address"].ToString()+"\r\n";
		    text+=curRow["City"].ToString()+", "+curRow["State"].ToString()+" "+curRow["Zip"].ToString()+"\r\n";
		    LabelSingle.PrintText(0,text);
		    patientsPrinted++;
			}
		}

		private void butLabelPreview_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0){
				MsgBox.Show(this,"Please select patient(s) first.");
		    return;
			}
			pagesPrinted=0;
			patientsPrinted=0;
			PrinterL.TryPreview(pdLabels_PrintPage,
				Lan.g(this,"Treatment finder labels printed"),
				PrintSituation.LabelSheet,
				new Margins(0,0,0,0),
				PrintoutOrigin.AtMargin,
				totalPages:(int)Math.Ceiling((double)gridMain.SelectedIndices.Length/30)
			);
		}

		private void pdLabels_PrintPage(object sender, PrintPageEventArgs ev){
			int totalPages=(int)Math.Ceiling((double)gridMain.SelectedIndices.Length/30);
			Graphics g=ev.Graphics;
			float yPos=63;
			float xPos=50;
			string text="";
			while(yPos<1000 && patientsPrinted<gridMain.SelectedIndices.Length){
				text="";
				DataRow curRow=(DataRow)gridMain.SelectedGridRows[patientsPrinted].Tag;
				text=curRow["FName"].ToString()+" "+curRow["LName"].ToString()+"\r\n";
				text+=curRow["address"].ToString()+"\r\n";
				text+=curRow["City"].ToString()+", "+curRow["State"].ToString()+" "+curRow["Zip"].ToString()+"\r\n";
				Rectangle rect=new Rectangle((int)xPos,(int)yPos,275,100);
				MapAreaRoomControl.FitText(text,new Font(FontFamily.GenericSansSerif,11),Brushes.Black,rect,new StringFormat(),g);
				//reposition for next label
				xPos+=275;
				if(xPos>850){//drop a line
					xPos=50;
					yPos+=100;
				}
				patientsPrinted++;
			}
			pagesPrinted++;
			if(pagesPrinted==totalPages){
				ev.HasMorePages=false;
				pagesPrinted=0;//because it has to print again from the print preview
				patientsPrinted=0;
			}
			else{
				ev.HasMorePages=true;
			}
			g.Dispose();
		}

		private void menuItemFamily_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.FamilyModule)) {
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			long patNum=PIn.Long(gridMain.SelectedTag<DataRow>()["PatNum"].ToString());
			GotoModule.GotoFamily(patNum);
		}

		private void menuItemAccount_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AccountModule)) {
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			long patNum=PIn.Long(gridMain.SelectedTag<DataRow>()["PatNum"].ToString());
			GotoModule.GotoAccount(patNum);
		}

		private void butGotoFamily_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.FamilyModule)) {
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			WindowState=FormWindowState.Minimized;
			long patNum=PIn.Long(gridMain.SelectedTag<DataRow>()["PatNum"].ToString());
			GotoModule.GotoFamily(patNum);
		}

		private void butGotoAccount_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AccountModule)) {
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			WindowState=FormWindowState.Minimized;
			long patNum=PIn.Long(gridMain.SelectedTag<DataRow>()["PatNum"].ToString());
			GotoModule.GotoAccount(patNum);
		}

		private void buttonExport_Click(object sender,EventArgs e) {
			string fileName=Lan.g(this,"Treatment Finder");
			string filePath=ODFileUtils.CombinePaths(Path.GetTempPath(),fileName);
			if(ODBuild.IsWeb()) {
				//file download dialog will come up later, after file is created.
				filePath+=".txt";//Provide the filepath an extension so that Thinfinity can offer as a download.
			}
			else {
				SaveFileDialog saveFileDialog2=new SaveFileDialog();
				saveFileDialog2.AddExtension=true;
				saveFileDialog2.Title=Lan.g(this,"Treatment Finder");
				saveFileDialog2.FileName=Lan.g(this,"Treatment Finder");
				if(!Directory.Exists(PrefC.GetString(PrefName.ExportPath))) {
					try {
						Directory.CreateDirectory(PrefC.GetString(PrefName.ExportPath));
						saveFileDialog2.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
					}
					catch {
						//initialDirectory will be blank
					}
				}
				else saveFileDialog2.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
				saveFileDialog2.Filter="Text files(*.txt)|*.txt|Excel Files(*.xls)|*.xls|All files(*.*)|*.*";
				saveFileDialog2.FilterIndex=0;
				if(saveFileDialog2.ShowDialog()!=DialogResult.OK) {
					return;
				}
				filePath=saveFileDialog2.FileName;
			}
			try{
			  using(StreamWriter sw=new StreamWriter(filePath,false))
				{
					sw.WriteLine(string.Join("\t",gridMain.ListGridColumns.Select(x => x.Heading)));
					gridMain.ListGridRows.ForEach(x => sw.WriteLine(
						string.Join("\t",x.Cells.Select(y => string.Concat(y.Text.Where(z => !z.In('\r','\n','\t','\"')))))));
				}
      }
      catch{
        MessageBox.Show(Lan.g(this,"File in use by another program.  Close and try again."));
				return;
			}
			if(ODBuild.IsWeb()) {
				ThinfinityUtils.ExportForDownload(filePath);
			}
			else {
				MessageBox.Show(Lan.g(this,"File created successfully"));
			}
		}

		private void butPrint_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			headingPrinted=false;
			PrinterL.TryPrintOrDebugRpPreview(pd_PrintPage,Lan.g(this,"Treatment finder list printed"),PrintoutOrientation.Landscape);
		}
		
		private void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
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
				text=Lan.g(this,"Treatment Finder");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				if(checkIncludeNoIns.Checked) {
					text="Include patients without insurance";
				}
				else {
					text="Only patients with insurance";
				}
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
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

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			Close();
		}

	}
}
