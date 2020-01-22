namespace OpenDental {
	partial class FormJobManager {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJobManager));
			this.contextMenuQueries = new System.Windows.Forms.ContextMenu();
			this.menuGoToAccount = new System.Windows.Forms.MenuItem();
			this.timerSearch = new System.Windows.Forms.Timer(this.components);
			this.timerDocumentationVersion = new System.Windows.Forms.Timer(this.components);
			this.textSearch = new System.Windows.Forms.TextBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tabControlNav = new System.Windows.Forms.TabControl();
			this.tabPatternReview = new System.Windows.Forms.TabPage();
			this.label13 = new System.Windows.Forms.Label();
			this.dateExcludeCompleteBefore = new System.Windows.Forms.DateTimePicker();
			this.gridPatternReview = new OpenDental.UI.ODGrid();
			this.tabAction = new System.Windows.Forms.TabPage();
			this.label15 = new System.Windows.Forms.Label();
			this.comboProposedVersionNeedsAction = new OpenDental.UI.ComboBoxPlus();
			this.checkShowUnassigned = new System.Windows.Forms.CheckBox();
			this.gridAction = new OpenDental.UI.ODGrid();
			this.tabSpecialProjects = new System.Windows.Forms.TabPage();
			this.checkShowUnassignedSpecial = new System.Windows.Forms.CheckBox();
			this.gridSpecial = new OpenDental.UI.ODGrid();
			this.tabDocumentation = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.textDocumentationVersion = new System.Windows.Forms.TextBox();
			this.gridDocumentation = new OpenDental.UI.ODGrid();
			this.tabTesting = new System.Windows.Forms.TabPage();
			this.checkHideNotTested = new System.Windows.Forms.CheckBox();
			this.butTestingRefresh = new OpenDental.UI.Button();
			this.checkShowAllUsers = new System.Windows.Forms.CheckBox();
			this.checkHideTested = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textVersionText = new System.Windows.Forms.TextBox();
			this.gridTesting = new OpenDental.UI.ODGrid();
			this.tabQuery = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butQueriesRefresh = new OpenDental.UI.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.dateTo = new System.Windows.Forms.DateTimePicker();
			this.dateFrom = new System.Windows.Forms.DateTimePicker();
			this.checkShowQueryComplete = new System.Windows.Forms.CheckBox();
			this.checkShowQueryCancelled = new System.Windows.Forms.CheckBox();
			this.gridQueries = new OpenDental.UI.ODGrid();
			this.tabNotify = new System.Windows.Forms.TabPage();
			this.gridNotify = new OpenDental.UI.ODGrid();
			this.tabSubscribed = new System.Windows.Forms.TabPage();
			this.checkSubscribedIncludeOnHold = new System.Windows.Forms.CheckBox();
			this.gridSubscribedJobs = new OpenDental.UI.ODGrid();
			this.tabNeedsEngineer = new System.Windows.Forms.TabPage();
			this.label7 = new System.Windows.Forms.Label();
			this.comboProposedVersionNeedsEngineer = new OpenDental.UI.ComboBoxPlus();
			this.gridAvailableJobs = new OpenDental.UI.ODGrid();
			this.tabNeedsExpert = new System.Windows.Forms.TabPage();
			this.label8 = new System.Windows.Forms.Label();
			this.comboProposedVersionNeedsExpert = new OpenDental.UI.ComboBoxPlus();
			this.gridAvailableJobsExpert = new OpenDental.UI.ODGrid();
			this.tabOnHold = new System.Windows.Forms.TabPage();
			this.gridJobsOnHold = new OpenDental.UI.ODGrid();
			this.tabSearch = new System.Windows.Forms.TabPage();
			this.gridSearch = new OpenDental.UI.ODGrid();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label12 = new System.Windows.Forms.Label();
			this.comboProposedVersionSearch = new OpenDental.UI.ComboBoxPlus();
			this.textUserSearch = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.comboPrioritySearch = new OpenDental.UI.ComboBoxPlus();
			this.label10 = new System.Windows.Forms.Label();
			this.checkContactSearch = new System.Windows.Forms.CheckBox();
			this.comboCatSearch = new OpenDental.UI.ComboBoxPlus();
			this.label9 = new System.Windows.Forms.Label();
			this.tabTree = new System.Windows.Forms.TabPage();
			this.checkIncludeCustContact = new System.Windows.Forms.CheckBox();
			this.checkResults = new System.Windows.Forms.CheckBox();
			this.treeJobs = new System.Windows.Forms.TreeView();
			this.checkCollapse = new System.Windows.Forms.CheckBox();
			this.comboCategorySearch = new System.Windows.Forms.ComboBox();
			this.labelCategory = new System.Windows.Forms.Label();
			this.labelGroupBy = new System.Windows.Forms.Label();
			this.comboGroup = new System.Windows.Forms.ComboBox();
			this.userControlJobEdit = new OpenDental.InternalTools.Job_Manager.UserControlJobEdit();
			this.userControlQueryEdit = new OpenDental.InternalTools.Job_Manager.UserControlQueryEdit();
			this.label5 = new System.Windows.Forms.Label();
			this.butAdvSearch = new OpenDental.UI.Button();
			this.butMe = new OpenDental.UI.Button();
			this.comboUser = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.addJobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addChildJobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.backportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dashboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.jobTimeHelperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.releaseCalculatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.jobOverviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.bugSubmissionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.timerUserSearch = new System.Windows.Forms.Timer(this.components);
			this.butSearch = new OpenDental.UI.Button();
			this.butBack = new OpenDental.UI.Button();
			this.butForward = new OpenDental.UI.Button();
			this.butRefresh = new OpenDental.UI.Button();
			this.contextMenuPatternReview = new System.Windows.Forms.ContextMenu();
			this.menuItemNone = new System.Windows.Forms.MenuItem();
			this.menuItemNotNeeded = new System.Windows.Forms.MenuItem();
			this.menuItemAwaiting = new System.Windows.Forms.MenuItem();
			this.menuItemApproved = new System.Windows.Forms.MenuItem();
			this.menuItemTentative = new System.Windows.Forms.MenuItem();
			this.menuItemNotApproved = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabControlNav.SuspendLayout();
			this.tabPatternReview.SuspendLayout();
			this.tabAction.SuspendLayout();
			this.tabSpecialProjects.SuspendLayout();
			this.tabDocumentation.SuspendLayout();
			this.tabTesting.SuspendLayout();
			this.tabQuery.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabNotify.SuspendLayout();
			this.tabSubscribed.SuspendLayout();
			this.tabNeedsEngineer.SuspendLayout();
			this.tabNeedsExpert.SuspendLayout();
			this.tabOnHold.SuspendLayout();
			this.tabSearch.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabTree.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuQueries
			// 
			this.contextMenuQueries.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuGoToAccount});
			// 
			// menuGoToAccount
			// 
			this.menuGoToAccount.Index = 0;
			this.menuGoToAccount.Text = "Go To Account";
			this.menuGoToAccount.Click += new System.EventHandler(this.menuGoToAccount_Click);
			// 
			// timerSearch
			// 
			this.timerSearch.Interval = 500;
			this.timerSearch.Tick += new System.EventHandler(this.timerSearch_Tick);
			// 
			// timerDocumentationVersion
			// 
			this.timerDocumentationVersion.Interval = 500;
			this.timerDocumentationVersion.Tick += new System.EventHandler(this.timerDocumentationVersion_Tick);
			// 
			// textSearch
			// 
			this.textSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textSearch.Location = new System.Drawing.Point(512, 31);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(619, 20);
			this.textSearch.TabIndex = 240;
			this.textSearch.TextChanged += new System.EventHandler(this.textSearchAction_TextChanged);
			this.textSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textSearch_KeyDown);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(-1, 58);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tabControlNav);
			this.splitContainer1.Panel1MinSize = 250;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.userControlJobEdit);
			this.splitContainer1.Panel2.Controls.Add(this.userControlQueryEdit);
			this.splitContainer1.Panel2MinSize = 250;
			this.splitContainer1.Size = new System.Drawing.Size(1519, 767);
			this.splitContainer1.SplitterDistance = 357;
			this.splitContainer1.TabIndex = 6;
			// 
			// tabControlNav
			// 
			this.tabControlNav.Controls.Add(this.tabPatternReview);
			this.tabControlNav.Controls.Add(this.tabAction);
			this.tabControlNav.Controls.Add(this.tabSpecialProjects);
			this.tabControlNav.Controls.Add(this.tabDocumentation);
			this.tabControlNav.Controls.Add(this.tabTesting);
			this.tabControlNav.Controls.Add(this.tabQuery);
			this.tabControlNav.Controls.Add(this.tabNotify);
			this.tabControlNav.Controls.Add(this.tabSubscribed);
			this.tabControlNav.Controls.Add(this.tabNeedsEngineer);
			this.tabControlNav.Controls.Add(this.tabNeedsExpert);
			this.tabControlNav.Controls.Add(this.tabOnHold);
			this.tabControlNav.Controls.Add(this.tabSearch);
			this.tabControlNav.Controls.Add(this.tabTree);
			this.tabControlNav.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlNav.Location = new System.Drawing.Point(0, 0);
			this.tabControlNav.Multiline = true;
			this.tabControlNav.Name = "tabControlNav";
			this.tabControlNav.SelectedIndex = 0;
			this.tabControlNav.Size = new System.Drawing.Size(357, 767);
			this.tabControlNav.TabIndex = 1;
			this.tabControlNav.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControlNav_DrawItem);
			this.tabControlNav.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControlNav_Selecting);
			// 
			// tabPatternReview
			// 
			this.tabPatternReview.Controls.Add(this.label13);
			this.tabPatternReview.Controls.Add(this.dateExcludeCompleteBefore);
			this.tabPatternReview.Controls.Add(this.gridPatternReview);
			this.tabPatternReview.Location = new System.Drawing.Point(4, 58);
			this.tabPatternReview.Name = "tabPatternReview";
			this.tabPatternReview.Padding = new System.Windows.Forms.Padding(3);
			this.tabPatternReview.Size = new System.Drawing.Size(349, 705);
			this.tabPatternReview.TabIndex = 12;
			this.tabPatternReview.Text = "Pattern Review";
			this.tabPatternReview.UseVisualStyleBackColor = true;
			// 
			// label13
			// 
			this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label13.Location = new System.Drawing.Point(64, 3);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(181, 20);
			this.label13.TabIndex = 243;
			this.label13.Text = "Exclude Jobs Completed Before";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// dateExcludeCompleteBefore
			// 
			this.dateExcludeCompleteBefore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dateExcludeCompleteBefore.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateExcludeCompleteBefore.Location = new System.Drawing.Point(247, 3);
			this.dateExcludeCompleteBefore.Name = "dateExcludeCompleteBefore";
			this.dateExcludeCompleteBefore.Size = new System.Drawing.Size(96, 20);
			this.dateExcludeCompleteBefore.TabIndex = 242;
			this.dateExcludeCompleteBefore.ValueChanged += new System.EventHandler(this.dateExcludeCompleteBefore_ValueChanged);
			// 
			// gridPatternReview
			// 
			this.gridPatternReview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridPatternReview.HasMultilineHeaders = true;
			this.gridPatternReview.Location = new System.Drawing.Point(3, 26);
			this.gridPatternReview.Name = "gridPatternReview";
			this.gridPatternReview.ShowContextMenu = false;
			this.gridPatternReview.Size = new System.Drawing.Size(343, 676);
			this.gridPatternReview.TabIndex = 239;
			this.gridPatternReview.Title = "Jobs For Review";
			this.gridPatternReview.TranslationName = "FormJobManager";
			this.gridPatternReview.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPatternReview_CellClick);
			// 
			// tabAction
			// 
			this.tabAction.Controls.Add(this.label15);
			this.tabAction.Controls.Add(this.comboProposedVersionNeedsAction);
			this.tabAction.Controls.Add(this.checkShowUnassigned);
			this.tabAction.Controls.Add(this.gridAction);
			this.tabAction.Location = new System.Drawing.Point(4, 58);
			this.tabAction.Name = "tabAction";
			this.tabAction.Padding = new System.Windows.Forms.Padding(3);
			this.tabAction.Size = new System.Drawing.Size(349, 705);
			this.tabAction.TabIndex = 0;
			this.tabAction.Text = "Needs Action";
			this.tabAction.UseVisualStyleBackColor = true;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(-2, 4);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(72, 16);
			this.label15.TabIndex = 322;
			this.label15.Text = "Est. Version";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboProposedVersionNeedsAction
			// 
			this.comboProposedVersionNeedsAction.Location = new System.Drawing.Point(72, 2);
			this.comboProposedVersionNeedsAction.Name = "comboProposedVersionNeedsAction";
			this.comboProposedVersionNeedsAction.Size = new System.Drawing.Size(117, 21);
			this.comboProposedVersionNeedsAction.TabIndex = 323;
			this.comboProposedVersionNeedsAction.SelectionChangeCommitted += new System.EventHandler(this.comboProposedVersionNeedsAction_SelectionChangeCommitted);
			// 
			// checkShowUnassigned
			// 
			this.checkShowUnassigned.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowUnassigned.Location = new System.Drawing.Point(187, 4);
			this.checkShowUnassigned.Name = "checkShowUnassigned";
			this.checkShowUnassigned.Size = new System.Drawing.Size(159, 20);
			this.checkShowUnassigned.TabIndex = 238;
			this.checkShowUnassigned.Text = "Show OnHold/Unassigned";
			this.checkShowUnassigned.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowUnassigned.UseVisualStyleBackColor = true;
			this.checkShowUnassigned.CheckedChanged += new System.EventHandler(this.comboUser_SelectionChangeCommitted);
			// 
			// gridAction
			// 
			this.gridAction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAction.HasMultilineHeaders = true;
			this.gridAction.Location = new System.Drawing.Point(3, 31);
			this.gridAction.Name = "gridAction";
			this.gridAction.ShowContextMenu = false;
			this.gridAction.Size = new System.Drawing.Size(343, 671);
			this.gridAction.TabIndex = 227;
			this.gridAction.Title = "Action Items";
			this.gridAction.TranslationName = "FormJobManager";
			this.gridAction.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAction_CellDoubleClick);
			this.gridAction.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAction_CellClick);
			this.gridAction.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridAction_MouseMove);
			// 
			// tabSpecialProjects
			// 
			this.tabSpecialProjects.Controls.Add(this.checkShowUnassignedSpecial);
			this.tabSpecialProjects.Controls.Add(this.gridSpecial);
			this.tabSpecialProjects.Location = new System.Drawing.Point(4, 58);
			this.tabSpecialProjects.Name = "tabSpecialProjects";
			this.tabSpecialProjects.Padding = new System.Windows.Forms.Padding(3);
			this.tabSpecialProjects.Size = new System.Drawing.Size(349, 705);
			this.tabSpecialProjects.TabIndex = 11;
			this.tabSpecialProjects.Text = "Special Projects";
			this.tabSpecialProjects.UseVisualStyleBackColor = true;
			// 
			// checkShowUnassignedSpecial
			// 
			this.checkShowUnassignedSpecial.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowUnassignedSpecial.Dock = System.Windows.Forms.DockStyle.Top;
			this.checkShowUnassignedSpecial.Location = new System.Drawing.Point(3, 3);
			this.checkShowUnassignedSpecial.Name = "checkShowUnassignedSpecial";
			this.checkShowUnassignedSpecial.Size = new System.Drawing.Size(343, 20);
			this.checkShowUnassignedSpecial.TabIndex = 240;
			this.checkShowUnassignedSpecial.Text = "Show OnHold/Unassigned";
			this.checkShowUnassignedSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowUnassignedSpecial.UseVisualStyleBackColor = true;
			this.checkShowUnassignedSpecial.CheckedChanged += new System.EventHandler(this.comboUser_SelectionChangeCommitted);
			// 
			// gridSpecial
			// 
			this.gridSpecial.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridSpecial.HasMultilineHeaders = true;
			this.gridSpecial.Location = new System.Drawing.Point(3, 31);
			this.gridSpecial.Name = "gridSpecial";
			this.gridSpecial.ShowContextMenu = false;
			this.gridSpecial.Size = new System.Drawing.Size(343, 671);
			this.gridSpecial.TabIndex = 239;
			this.gridSpecial.Title = "Action Items";
			this.gridSpecial.TranslationName = "FormTaskEdit";
			this.gridSpecial.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSpecial_CellDoubleClick);
			this.gridSpecial.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSpecial_CellClick);
			// 
			// tabDocumentation
			// 
			this.tabDocumentation.Controls.Add(this.label1);
			this.tabDocumentation.Controls.Add(this.textDocumentationVersion);
			this.tabDocumentation.Controls.Add(this.gridDocumentation);
			this.tabDocumentation.Location = new System.Drawing.Point(4, 58);
			this.tabDocumentation.Name = "tabDocumentation";
			this.tabDocumentation.Padding = new System.Windows.Forms.Padding(3);
			this.tabDocumentation.Size = new System.Drawing.Size(349, 705);
			this.tabDocumentation.TabIndex = 6;
			this.tabDocumentation.Text = "Documentation";
			this.tabDocumentation.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(253, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(42, 20);
			this.label1.TabIndex = 244;
			this.label1.Text = "Version";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDocumentationVersion
			// 
			this.textDocumentationVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textDocumentationVersion.Location = new System.Drawing.Point(301, 3);
			this.textDocumentationVersion.Name = "textDocumentationVersion";
			this.textDocumentationVersion.Size = new System.Drawing.Size(42, 20);
			this.textDocumentationVersion.TabIndex = 243;
			this.textDocumentationVersion.TextChanged += new System.EventHandler(this.textDocumentationVersion_TextChanged);
			// 
			// gridDocumentation
			// 
			this.gridDocumentation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridDocumentation.HasMultilineHeaders = true;
			this.gridDocumentation.Location = new System.Drawing.Point(3, 26);
			this.gridDocumentation.Name = "gridDocumentation";
			this.gridDocumentation.Size = new System.Drawing.Size(343, 675);
			this.gridDocumentation.TabIndex = 239;
			this.gridDocumentation.Title = "Action Items";
			this.gridDocumentation.TranslationName = "FormTaskEdit";
			this.gridDocumentation.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridDocumentation_CellDoubleClick);
			this.gridDocumentation.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridDocumention_CellClick);
			// 
			// tabTesting
			// 
			this.tabTesting.Controls.Add(this.checkHideNotTested);
			this.tabTesting.Controls.Add(this.butTestingRefresh);
			this.tabTesting.Controls.Add(this.checkShowAllUsers);
			this.tabTesting.Controls.Add(this.checkHideTested);
			this.tabTesting.Controls.Add(this.label3);
			this.tabTesting.Controls.Add(this.textVersionText);
			this.tabTesting.Controls.Add(this.gridTesting);
			this.tabTesting.Location = new System.Drawing.Point(4, 58);
			this.tabTesting.Name = "tabTesting";
			this.tabTesting.Padding = new System.Windows.Forms.Padding(3);
			this.tabTesting.Size = new System.Drawing.Size(349, 705);
			this.tabTesting.TabIndex = 9;
			this.tabTesting.Text = "Testing";
			this.tabTesting.UseVisualStyleBackColor = true;
			// 
			// checkHideNotTested
			// 
			this.checkHideNotTested.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkHideNotTested.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHideNotTested.Checked = true;
			this.checkHideNotTested.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkHideNotTested.Location = new System.Drawing.Point(101, 32);
			this.checkHideNotTested.Name = "checkHideNotTested";
			this.checkHideNotTested.Size = new System.Drawing.Size(113, 20);
			this.checkHideNotTested.TabIndex = 246;
			this.checkHideNotTested.Text = "Hide Not Tested";
			this.checkHideNotTested.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHideNotTested.UseVisualStyleBackColor = true;
			this.checkHideNotTested.CheckedChanged += new System.EventHandler(this.checkHideNotTested_CheckedChanged);
			// 
			// butTestingRefresh
			// 
			this.butTestingRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butTestingRefresh.Location = new System.Drawing.Point(235, 32);
			this.butTestingRefresh.Name = "butTestingRefresh";
			this.butTestingRefresh.Size = new System.Drawing.Size(87, 24);
			this.butTestingRefresh.TabIndex = 245;
			this.butTestingRefresh.Text = "Refresh";
			this.butTestingRefresh.Click += new System.EventHandler(this.butTestingRefresh_Click);
			// 
			// checkShowAllUsers
			// 
			this.checkShowAllUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowAllUsers.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowAllUsers.Location = new System.Drawing.Point(9, 6);
			this.checkShowAllUsers.Name = "checkShowAllUsers";
			this.checkShowAllUsers.Size = new System.Drawing.Size(100, 20);
			this.checkShowAllUsers.TabIndex = 244;
			this.checkShowAllUsers.Text = "Show All Users";
			this.checkShowAllUsers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowAllUsers.UseVisualStyleBackColor = true;
			this.checkShowAllUsers.CheckedChanged += new System.EventHandler(this.checkShowAllUsers_CheckedChanged);
			// 
			// checkHideTested
			// 
			this.checkHideTested.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkHideTested.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHideTested.Checked = true;
			this.checkHideTested.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkHideTested.Location = new System.Drawing.Point(125, 6);
			this.checkHideTested.Name = "checkHideTested";
			this.checkHideTested.Size = new System.Drawing.Size(89, 20);
			this.checkHideTested.TabIndex = 243;
			this.checkHideTested.Text = "Hide Tested";
			this.checkHideTested.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHideTested.UseVisualStyleBackColor = true;
			this.checkHideTested.CheckedChanged += new System.EventHandler(this.checkHideTested_CheckedChanged);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(232, 5);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(42, 20);
			this.label3.TabIndex = 242;
			this.label3.Text = "Version";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textVersionText
			// 
			this.textVersionText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textVersionText.Location = new System.Drawing.Point(280, 6);
			this.textVersionText.Name = "textVersionText";
			this.textVersionText.Size = new System.Drawing.Size(42, 20);
			this.textVersionText.TabIndex = 241;
			this.textVersionText.Text = "18.3";
			// 
			// gridTesting
			// 
			this.gridTesting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridTesting.HasMultilineHeaders = true;
			this.gridTesting.Location = new System.Drawing.Point(3, 62);
			this.gridTesting.Name = "gridTesting";
			this.gridTesting.Size = new System.Drawing.Size(343, 626);
			this.gridTesting.TabIndex = 228;
			this.gridTesting.Title = "Completed Jobs";
			this.gridTesting.TranslationName = "FormTaskEdit";
			this.gridTesting.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTesting_CellDoubleClick);
			this.gridTesting.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTesting_CellClick);
			// 
			// tabQuery
			// 
			this.tabQuery.Controls.Add(this.groupBox1);
			this.tabQuery.Controls.Add(this.gridQueries);
			this.tabQuery.Location = new System.Drawing.Point(4, 58);
			this.tabQuery.Name = "tabQuery";
			this.tabQuery.Padding = new System.Windows.Forms.Padding(3);
			this.tabQuery.Size = new System.Drawing.Size(349, 705);
			this.tabQuery.TabIndex = 5;
			this.tabQuery.Text = "Queries";
			this.tabQuery.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butQueriesRefresh);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.dateTo);
			this.groupBox1.Controls.Add(this.dateFrom);
			this.groupBox1.Controls.Add(this.checkShowQueryComplete);
			this.groupBox1.Controls.Add(this.checkShowQueryCancelled);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(343, 82);
			this.groupBox1.TabIndex = 239;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Complete and Cancelled Filters";
			// 
			// butQueriesRefresh
			// 
			this.butQueriesRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butQueriesRefresh.Location = new System.Drawing.Point(257, 46);
			this.butQueriesRefresh.Name = "butQueriesRefresh";
			this.butQueriesRefresh.Size = new System.Drawing.Size(80, 24);
			this.butQueriesRefresh.TabIndex = 243;
			this.butQueriesRefresh.Text = "Refresh";
			this.butQueriesRefresh.Click += new System.EventHandler(this.butQueriesRefresh_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(172, 17);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(35, 23);
			this.label6.TabIndex = 242;
			this.label6.Text = "To";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(21, 17);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 23);
			this.label2.TabIndex = 241;
			this.label2.Text = "From";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// dateTo
			// 
			this.dateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateTo.Location = new System.Drawing.Point(209, 20);
			this.dateTo.Name = "dateTo";
			this.dateTo.Size = new System.Drawing.Size(96, 20);
			this.dateTo.TabIndex = 240;
			// 
			// dateFrom
			// 
			this.dateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateFrom.Location = new System.Drawing.Point(58, 20);
			this.dateFrom.Name = "dateFrom";
			this.dateFrom.Size = new System.Drawing.Size(96, 20);
			this.dateFrom.TabIndex = 239;
			// 
			// checkShowQueryComplete
			// 
			this.checkShowQueryComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowQueryComplete.Location = new System.Drawing.Point(0, 49);
			this.checkShowQueryComplete.Name = "checkShowQueryComplete";
			this.checkShowQueryComplete.Size = new System.Drawing.Size(135, 20);
			this.checkShowQueryComplete.TabIndex = 237;
			this.checkShowQueryComplete.Text = "Include Complete";
			this.checkShowQueryComplete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowQueryComplete.UseVisualStyleBackColor = true;
			// 
			// checkShowQueryCancelled
			// 
			this.checkShowQueryCancelled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowQueryCancelled.Location = new System.Drawing.Point(116, 49);
			this.checkShowQueryCancelled.Name = "checkShowQueryCancelled";
			this.checkShowQueryCancelled.Size = new System.Drawing.Size(135, 20);
			this.checkShowQueryCancelled.TabIndex = 238;
			this.checkShowQueryCancelled.Text = "Include Cancelled";
			this.checkShowQueryCancelled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowQueryCancelled.UseVisualStyleBackColor = true;
			// 
			// gridQueries
			// 
			this.gridQueries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridQueries.HasMultilineHeaders = true;
			this.gridQueries.Location = new System.Drawing.Point(2, 86);
			this.gridQueries.Name = "gridQueries";
			this.gridQueries.Size = new System.Drawing.Size(344, 616);
			this.gridQueries.TabIndex = 230;
			this.gridQueries.Title = "Queries to be done";
			this.gridQueries.TranslationName = "Job Edit";
			this.gridQueries.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridQueries_CellDoubleClick);
			this.gridQueries.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridQueries_CellClick);
			// 
			// tabNotify
			// 
			this.tabNotify.Controls.Add(this.gridNotify);
			this.tabNotify.Location = new System.Drawing.Point(4, 58);
			this.tabNotify.Name = "tabNotify";
			this.tabNotify.Size = new System.Drawing.Size(349, 705);
			this.tabNotify.TabIndex = 7;
			this.tabNotify.Text = "Notify Customer";
			this.tabNotify.UseVisualStyleBackColor = true;
			// 
			// gridNotify
			// 
			this.gridNotify.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridNotify.HasMultilineHeaders = true;
			this.gridNotify.Location = new System.Drawing.Point(3, 4);
			this.gridNotify.Name = "gridNotify";
			this.gridNotify.Size = new System.Drawing.Size(343, 698);
			this.gridNotify.TabIndex = 240;
			this.gridNotify.Title = "Action Items";
			this.gridNotify.TranslationName = "FormTaskEdit";
			this.gridNotify.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridNotify_CellDoubleClick);
			this.gridNotify.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridNotify_CellClick);
			// 
			// tabSubscribed
			// 
			this.tabSubscribed.Controls.Add(this.checkSubscribedIncludeOnHold);
			this.tabSubscribed.Controls.Add(this.gridSubscribedJobs);
			this.tabSubscribed.Location = new System.Drawing.Point(4, 58);
			this.tabSubscribed.Name = "tabSubscribed";
			this.tabSubscribed.Padding = new System.Windows.Forms.Padding(3);
			this.tabSubscribed.Size = new System.Drawing.Size(349, 705);
			this.tabSubscribed.TabIndex = 8;
			this.tabSubscribed.Text = "Subscribed Jobs";
			this.tabSubscribed.UseVisualStyleBackColor = true;
			// 
			// checkSubscribedIncludeOnHold
			// 
			this.checkSubscribedIncludeOnHold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkSubscribedIncludeOnHold.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSubscribedIncludeOnHold.Location = new System.Drawing.Point(211, 4);
			this.checkSubscribedIncludeOnHold.Name = "checkSubscribedIncludeOnHold";
			this.checkSubscribedIncludeOnHold.Size = new System.Drawing.Size(135, 20);
			this.checkSubscribedIncludeOnHold.TabIndex = 242;
			this.checkSubscribedIncludeOnHold.Text = "Include On Hold";
			this.checkSubscribedIncludeOnHold.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSubscribedIncludeOnHold.UseVisualStyleBackColor = true;
			this.checkSubscribedIncludeOnHold.CheckedChanged += new System.EventHandler(this.checkSubscribedIncludeOnHold_CheckedChanged);
			// 
			// gridSubscribedJobs
			// 
			this.gridSubscribedJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridSubscribedJobs.HasMultilineHeaders = true;
			this.gridSubscribedJobs.Location = new System.Drawing.Point(2, 30);
			this.gridSubscribedJobs.Name = "gridSubscribedJobs";
			this.gridSubscribedJobs.NoteSpanStop = 4;
			this.gridSubscribedJobs.Size = new System.Drawing.Size(344, 672);
			this.gridSubscribedJobs.TabIndex = 239;
			this.gridSubscribedJobs.Title = "Subscribed Jobs";
			this.gridSubscribedJobs.TranslationName = "Job Edit";
			this.gridSubscribedJobs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSubscribedJobs_CellDoubleClick);
			this.gridSubscribedJobs.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSubscribedJobs_CellClick);
			// 
			// tabNeedsEngineer
			// 
			this.tabNeedsEngineer.Controls.Add(this.label7);
			this.tabNeedsEngineer.Controls.Add(this.comboProposedVersionNeedsEngineer);
			this.tabNeedsEngineer.Controls.Add(this.gridAvailableJobs);
			this.tabNeedsEngineer.Location = new System.Drawing.Point(4, 58);
			this.tabNeedsEngineer.Name = "tabNeedsEngineer";
			this.tabNeedsEngineer.Padding = new System.Windows.Forms.Padding(3);
			this.tabNeedsEngineer.Size = new System.Drawing.Size(349, 705);
			this.tabNeedsEngineer.TabIndex = 2;
			this.tabNeedsEngineer.Text = "Needs Engineer";
			this.tabNeedsEngineer.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(-2, 8);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(72, 16);
			this.label7.TabIndex = 322;
			this.label7.Text = "Est. Version";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboProposedVersionNeedsEngineer
			// 
			this.comboProposedVersionNeedsEngineer.Location = new System.Drawing.Point(73, 7);
			this.comboProposedVersionNeedsEngineer.Name = "comboProposedVersionNeedsEngineer";
			this.comboProposedVersionNeedsEngineer.Size = new System.Drawing.Size(117, 21);
			this.comboProposedVersionNeedsEngineer.TabIndex = 323;
			this.comboProposedVersionNeedsEngineer.SelectionChangeCommitted += new System.EventHandler(this.comboProposedVersionNeedsEngineer_SelectionChangeCommitted);
			// 
			// gridAvailableJobs
			// 
			this.gridAvailableJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAvailableJobs.HasMultilineHeaders = true;
			this.gridAvailableJobs.Location = new System.Drawing.Point(0, 33);
			this.gridAvailableJobs.Name = "gridAvailableJobs";
			this.gridAvailableJobs.Size = new System.Drawing.Size(349, 672);
			this.gridAvailableJobs.TabIndex = 228;
			this.gridAvailableJobs.Title = "Available Engineer Jobs";
			this.gridAvailableJobs.TranslationName = "Job Edit";
			this.gridAvailableJobs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAvailableJobs_CellDoubleClick);
			this.gridAvailableJobs.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAvailableJobs_CellClick);
			// 
			// tabNeedsExpert
			// 
			this.tabNeedsExpert.Controls.Add(this.label8);
			this.tabNeedsExpert.Controls.Add(this.comboProposedVersionNeedsExpert);
			this.tabNeedsExpert.Controls.Add(this.gridAvailableJobsExpert);
			this.tabNeedsExpert.Location = new System.Drawing.Point(4, 58);
			this.tabNeedsExpert.Name = "tabNeedsExpert";
			this.tabNeedsExpert.Padding = new System.Windows.Forms.Padding(3);
			this.tabNeedsExpert.Size = new System.Drawing.Size(349, 705);
			this.tabNeedsExpert.TabIndex = 3;
			this.tabNeedsExpert.Text = "Needs Expert";
			this.tabNeedsExpert.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(1, 8);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(69, 16);
			this.label8.TabIndex = 324;
			this.label8.Text = "Est. Version";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboProposedVersionNeedsExpert
			// 
			this.comboProposedVersionNeedsExpert.Location = new System.Drawing.Point(73, 7);
			this.comboProposedVersionNeedsExpert.Name = "comboProposedVersionNeedsExpert";
			this.comboProposedVersionNeedsExpert.Size = new System.Drawing.Size(117, 21);
			this.comboProposedVersionNeedsExpert.TabIndex = 325;
			this.comboProposedVersionNeedsExpert.SelectionChangeCommitted += new System.EventHandler(this.comboProposedVersionNeedsExpert_SelectionChangeCommitted);
			// 
			// gridAvailableJobsExpert
			// 
			this.gridAvailableJobsExpert.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAvailableJobsExpert.HasMultilineHeaders = true;
			this.gridAvailableJobsExpert.Location = new System.Drawing.Point(0, 33);
			this.gridAvailableJobsExpert.Name = "gridAvailableJobsExpert";
			this.gridAvailableJobsExpert.Size = new System.Drawing.Size(349, 672);
			this.gridAvailableJobsExpert.TabIndex = 229;
			this.gridAvailableJobsExpert.Title = "Available Expert Jobs";
			this.gridAvailableJobsExpert.TranslationName = "Job Edit";
			this.gridAvailableJobsExpert.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAvailableJobsExpert_CellDoubleClick);
			this.gridAvailableJobsExpert.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAvailableJobsExpert_CellClick);
			// 
			// tabOnHold
			// 
			this.tabOnHold.Controls.Add(this.gridJobsOnHold);
			this.tabOnHold.Location = new System.Drawing.Point(4, 58);
			this.tabOnHold.Name = "tabOnHold";
			this.tabOnHold.Padding = new System.Windows.Forms.Padding(3);
			this.tabOnHold.Size = new System.Drawing.Size(349, 705);
			this.tabOnHold.TabIndex = 4;
			this.tabOnHold.Text = "On Hold";
			this.tabOnHold.UseVisualStyleBackColor = true;
			// 
			// gridJobsOnHold
			// 
			this.gridJobsOnHold.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridJobsOnHold.HasMultilineHeaders = true;
			this.gridJobsOnHold.Location = new System.Drawing.Point(0, 0);
			this.gridJobsOnHold.Name = "gridJobsOnHold";
			this.gridJobsOnHold.Size = new System.Drawing.Size(349, 705);
			this.gridJobsOnHold.TabIndex = 230;
			this.gridJobsOnHold.Title = "Jobs On Hold";
			this.gridJobsOnHold.TranslationName = "Job Edit";
			this.gridJobsOnHold.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridJobsOnHold_CellDoubleClick);
			this.gridJobsOnHold.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridJobsOnHold_CellClick);
			// 
			// tabSearch
			// 
			this.tabSearch.Controls.Add(this.gridSearch);
			this.tabSearch.Controls.Add(this.groupBox2);
			this.tabSearch.Location = new System.Drawing.Point(4, 58);
			this.tabSearch.Name = "tabSearch";
			this.tabSearch.Padding = new System.Windows.Forms.Padding(3);
			this.tabSearch.Size = new System.Drawing.Size(349, 705);
			this.tabSearch.TabIndex = 10;
			this.tabSearch.Text = "Search";
			this.tabSearch.UseVisualStyleBackColor = true;
			// 
			// gridSearch
			// 
			this.gridSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridSearch.HasMultilineHeaders = true;
			this.gridSearch.Location = new System.Drawing.Point(3, 134);
			this.gridSearch.Name = "gridSearch";
			this.gridSearch.Size = new System.Drawing.Size(343, 568);
			this.gridSearch.TabIndex = 240;
			this.gridSearch.Title = "Results";
			this.gridSearch.TranslationName = "Job Edit";
			this.gridSearch.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSearch_CellDoubleClick);
			this.gridSearch.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSearch_CellClick);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.comboProposedVersionSearch);
			this.groupBox2.Controls.Add(this.textUserSearch);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.comboPrioritySearch);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.checkContactSearch);
			this.groupBox2.Controls.Add(this.comboCatSearch);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox2.Location = new System.Drawing.Point(3, 3);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(343, 131);
			this.groupBox2.TabIndex = 241;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Filters";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(22, 59);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(69, 16);
			this.label12.TabIndex = 326;
			this.label12.Text = "Est. Version";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboProposedVersionSearch
			// 
			this.comboProposedVersionSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboProposedVersionSearch.Location = new System.Drawing.Point(92, 58);
			this.comboProposedVersionSearch.Name = "comboProposedVersionSearch";
			this.comboProposedVersionSearch.Size = new System.Drawing.Size(247, 21);
			this.comboProposedVersionSearch.TabIndex = 327;
			this.comboProposedVersionSearch.SelectionChangeCommitted += new System.EventHandler(this.comboProposedVersionSearch_SelectionChangeCommitted);
			// 
			// textUserSearch
			// 
			this.textUserSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textUserSearch.Location = new System.Drawing.Point(223, 83);
			this.textUserSearch.Name = "textUserSearch";
			this.textUserSearch.Size = new System.Drawing.Size(116, 20);
			this.textUserSearch.TabIndex = 249;
			this.textUserSearch.TextChanged += new System.EventHandler(this.textUserSearch_TextChanged);
			// 
			// label11
			// 
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label11.Location = new System.Drawing.Point(182, 79);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(35, 23);
			this.label11.TabIndex = 248;
			this.label11.Text = "User";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPrioritySearch
			// 
			this.comboPrioritySearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboPrioritySearch.Location = new System.Drawing.Point(92, 35);
			this.comboPrioritySearch.Name = "comboPrioritySearch";
			this.comboPrioritySearch.Size = new System.Drawing.Size(247, 21);
			this.comboPrioritySearch.TabIndex = 247;
			this.comboPrioritySearch.SelectionChangeCommitted += new System.EventHandler(this.comboPrioritySearch_SelectionChangeCommitted);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(36, 34);
			this.label10.Margin = new System.Windows.Forms.Padding(0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(55, 20);
			this.label10.TabIndex = 246;
			this.label10.Text = "Priority";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkContactSearch
			// 
			this.checkContactSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkContactSearch.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkContactSearch.Location = new System.Drawing.Point(181, 107);
			this.checkContactSearch.Name = "checkContactSearch";
			this.checkContactSearch.Size = new System.Drawing.Size(158, 20);
			this.checkContactSearch.TabIndex = 245;
			this.checkContactSearch.Text = "Include Customer Contact";
			this.checkContactSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkContactSearch.UseVisualStyleBackColor = true;
			this.checkContactSearch.CheckedChanged += new System.EventHandler(this.checkContactSearch_CheckedChanged);
			// 
			// comboCatSearch
			// 
			this.comboCatSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboCatSearch.Location = new System.Drawing.Point(92, 12);
			this.comboCatSearch.Name = "comboCatSearch";
			this.comboCatSearch.Size = new System.Drawing.Size(247, 21);
			this.comboCatSearch.TabIndex = 244;
			this.comboCatSearch.SelectionChangeCommitted += new System.EventHandler(this.comboCatSearch_SelectionChangeCommitted);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(36, 11);
			this.label9.Margin = new System.Windows.Forms.Padding(0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(55, 20);
			this.label9.TabIndex = 243;
			this.label9.Text = "Category";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabTree
			// 
			this.tabTree.Controls.Add(this.checkIncludeCustContact);
			this.tabTree.Controls.Add(this.checkResults);
			this.tabTree.Controls.Add(this.treeJobs);
			this.tabTree.Controls.Add(this.checkCollapse);
			this.tabTree.Controls.Add(this.comboCategorySearch);
			this.tabTree.Controls.Add(this.labelCategory);
			this.tabTree.Controls.Add(this.labelGroupBy);
			this.tabTree.Controls.Add(this.comboGroup);
			this.tabTree.Location = new System.Drawing.Point(4, 58);
			this.tabTree.Name = "tabTree";
			this.tabTree.Padding = new System.Windows.Forms.Padding(3);
			this.tabTree.Size = new System.Drawing.Size(349, 705);
			this.tabTree.TabIndex = 1;
			this.tabTree.Text = "Tree View";
			this.tabTree.UseVisualStyleBackColor = true;
			// 
			// checkIncludeCustContact
			// 
			this.checkIncludeCustContact.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkIncludeCustContact.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeCustContact.Location = new System.Drawing.Point(189, 57);
			this.checkIncludeCustContact.Name = "checkIncludeCustContact";
			this.checkIncludeCustContact.Size = new System.Drawing.Size(158, 20);
			this.checkIncludeCustContact.TabIndex = 237;
			this.checkIncludeCustContact.Text = "Include Customer Contact";
			this.checkIncludeCustContact.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeCustContact.UseVisualStyleBackColor = true;
			this.checkIncludeCustContact.CheckedChanged += new System.EventHandler(this.checkIncludeCustContact_CheckedChanged);
			// 
			// checkResults
			// 
			this.checkResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkResults.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkResults.Location = new System.Drawing.Point(222, 77);
			this.checkResults.Name = "checkResults";
			this.checkResults.Size = new System.Drawing.Size(125, 20);
			this.checkResults.TabIndex = 235;
			this.checkResults.Text = "Search Results";
			this.checkResults.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkResults.UseVisualStyleBackColor = true;
			this.checkResults.Visible = false;
			this.checkResults.CheckedChanged += new System.EventHandler(this.checkResults_CheckedChanged);
			// 
			// treeJobs
			// 
			this.treeJobs.AllowDrop = true;
			this.treeJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeJobs.HideSelection = false;
			this.treeJobs.Indent = 9;
			this.treeJobs.Location = new System.Drawing.Point(3, 103);
			this.treeJobs.Name = "treeJobs";
			this.treeJobs.Size = new System.Drawing.Size(347, 599);
			this.treeJobs.TabIndex = 220;
			this.treeJobs.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeJobs_ItemDrag);
			this.treeJobs.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeJobs_NodeMouseClick);
			this.treeJobs.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeJobs_NodeMouseDoubleClick);
			this.treeJobs.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeJobs_DragDrop);
			this.treeJobs.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeJobs_DragEnter);
			this.treeJobs.DragOver += new System.Windows.Forms.DragEventHandler(this.treeJobs_DragOver);
			// 
			// checkCollapse
			// 
			this.checkCollapse.Location = new System.Drawing.Point(6, 77);
			this.checkCollapse.Name = "checkCollapse";
			this.checkCollapse.Size = new System.Drawing.Size(103, 20);
			this.checkCollapse.TabIndex = 226;
			this.checkCollapse.Text = "Collapse All";
			this.checkCollapse.UseVisualStyleBackColor = true;
			this.checkCollapse.CheckedChanged += new System.EventHandler(this.checkCollapse_CheckedChanged);
			// 
			// comboCategorySearch
			// 
			this.comboCategorySearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboCategorySearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCategorySearch.FormattingEnabled = true;
			this.comboCategorySearch.Location = new System.Drawing.Point(68, 6);
			this.comboCategorySearch.Name = "comboCategorySearch";
			this.comboCategorySearch.Size = new System.Drawing.Size(279, 21);
			this.comboCategorySearch.TabIndex = 234;
			this.comboCategorySearch.SelectedIndexChanged += new System.EventHandler(this.comboCategorySearch_SelectedIndexChanged);
			// 
			// labelCategory
			// 
			this.labelCategory.Location = new System.Drawing.Point(12, 7);
			this.labelCategory.Margin = new System.Windows.Forms.Padding(0);
			this.labelCategory.Name = "labelCategory";
			this.labelCategory.Size = new System.Drawing.Size(55, 20);
			this.labelCategory.TabIndex = 233;
			this.labelCategory.Text = "Category";
			this.labelCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelGroupBy
			// 
			this.labelGroupBy.Location = new System.Drawing.Point(12, 33);
			this.labelGroupBy.Margin = new System.Windows.Forms.Padding(0);
			this.labelGroupBy.Name = "labelGroupBy";
			this.labelGroupBy.Size = new System.Drawing.Size(55, 15);
			this.labelGroupBy.TabIndex = 222;
			this.labelGroupBy.Text = "Group By";
			this.labelGroupBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboGroup
			// 
			this.comboGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGroup.FormattingEnabled = true;
			this.comboGroup.Location = new System.Drawing.Point(68, 30);
			this.comboGroup.Name = "comboGroup";
			this.comboGroup.Size = new System.Drawing.Size(279, 21);
			this.comboGroup.TabIndex = 221;
			this.comboGroup.SelectionChangeCommitted += new System.EventHandler(this.comboGroup_SelectionChangeCommitted);
			// 
			// userControlJobEdit
			// 
			this.userControlJobEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userControlJobEdit.Enabled = false;
			this.userControlJobEdit.IsOverride = false;
			this.userControlJobEdit.Location = new System.Drawing.Point(0, 0);
			this.userControlJobEdit.Name = "userControlJobEdit";
			this.userControlJobEdit.Size = new System.Drawing.Size(1158, 767);
			this.userControlJobEdit.TabIndex = 0;
			this.userControlJobEdit.SaveClick += new System.EventHandler(this.userControlJobEdit_SaveClick);
			this.userControlJobEdit.RequestJob += new OpenDental.InternalTools.Job_Manager.UserControlJobEdit.RequestJobEvent(this.userControlJobEdit_RequestJob);
			this.userControlJobEdit.JobOverride += new OpenDental.InternalTools.Job_Manager.UserControlJobEdit.JobOverrideEvent(this.userControlJobEdit_JobOverride);
			// 
			// userControlQueryEdit
			// 
			this.userControlQueryEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userControlQueryEdit.IsOverride = false;
			this.userControlQueryEdit.Location = new System.Drawing.Point(0, 0);
			this.userControlQueryEdit.Name = "userControlQueryEdit";
			this.userControlQueryEdit.Size = new System.Drawing.Size(1158, 767);
			this.userControlQueryEdit.TabIndex = 1;
			this.userControlQueryEdit.Visible = false;
			this.userControlQueryEdit.SaveClick += new System.EventHandler(this.userControlQueryEdit_SaveClick);
			this.userControlQueryEdit.RequestJob += new OpenDental.InternalTools.Job_Manager.UserControlQueryEdit.RequestJobEvent(this.userControlJobEdit_RequestJob);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(466, 30);
			this.label5.Margin = new System.Windows.Forms.Padding(0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(43, 20);
			this.label5.TabIndex = 241;
			this.label5.Text = "Search";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butAdvSearch
			// 
			this.butAdvSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdvSearch.Location = new System.Drawing.Point(1195, 29);
			this.butAdvSearch.Name = "butAdvSearch";
			this.butAdvSearch.Size = new System.Drawing.Size(80, 24);
			this.butAdvSearch.TabIndex = 231;
			this.butAdvSearch.Text = "Adv. Search";
			this.butAdvSearch.Click += new System.EventHandler(this.butAdvSearch_Click);
			// 
			// butMe
			// 
			this.butMe.Location = new System.Drawing.Point(254, 31);
			this.butMe.Name = "butMe";
			this.butMe.Size = new System.Drawing.Size(31, 21);
			this.butMe.TabIndex = 239;
			this.butMe.Text = "Me";
			this.butMe.Click += new System.EventHandler(this.butMe_Click);
			// 
			// comboUser
			// 
			this.comboUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUser.FormattingEnabled = true;
			this.comboUser.Location = new System.Drawing.Point(64, 31);
			this.comboUser.Name = "comboUser";
			this.comboUser.Size = new System.Drawing.Size(184, 21);
			this.comboUser.TabIndex = 236;
			this.comboUser.SelectionChangeCommitted += new System.EventHandler(this.comboUser_SelectionChangeCommitted);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 35);
			this.label4.Margin = new System.Windows.Forms.Padding(0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(55, 15);
			this.label4.TabIndex = 237;
			this.label4.Text = "User";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addJobToolStripMenuItem,
            this.addChildJobToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.bugSubmissionsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1516, 24);
			this.menuStrip1.TabIndex = 247;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// addJobToolStripMenuItem
			// 
			this.addJobToolStripMenuItem.Name = "addJobToolStripMenuItem";
			this.addJobToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
			this.addJobToolStripMenuItem.Text = "Add Job";
			this.addJobToolStripMenuItem.Click += new System.EventHandler(this.butAddJob_Click);
			// 
			// addChildJobToolStripMenuItem
			// 
			this.addChildJobToolStripMenuItem.Name = "addChildJobToolStripMenuItem";
			this.addChildJobToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
			this.addChildJobToolStripMenuItem.Text = "Add Child Job";
			this.addChildJobToolStripMenuItem.Click += new System.EventHandler(this.butAddChildJob_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backportToolStripMenuItem,
            this.dashboardToolStripMenuItem,
            this.jobTimeHelperToolStripMenuItem,
            this.releaseCalculatorToolStripMenuItem,
            this.jobOverviewToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// backportToolStripMenuItem
			// 
			this.backportToolStripMenuItem.Name = "backportToolStripMenuItem";
			this.backportToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.backportToolStripMenuItem.Text = "Backport";
			this.backportToolStripMenuItem.Click += new System.EventHandler(this.backportToolStripMenuItem_Click);
			// 
			// dashboardToolStripMenuItem
			// 
			this.dashboardToolStripMenuItem.Name = "dashboardToolStripMenuItem";
			this.dashboardToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.dashboardToolStripMenuItem.Text = "Dashboard";
			this.dashboardToolStripMenuItem.Click += new System.EventHandler(this.butDashboard_Click);
			// 
			// jobTimeHelperToolStripMenuItem
			// 
			this.jobTimeHelperToolStripMenuItem.Name = "jobTimeHelperToolStripMenuItem";
			this.jobTimeHelperToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.jobTimeHelperToolStripMenuItem.Text = "Job Time Helper";
			this.jobTimeHelperToolStripMenuItem.Click += new System.EventHandler(this.jobTimeHelperToolStripMenuItem_Click);
			// 
			// releaseCalculatorToolStripMenuItem
			// 
			this.releaseCalculatorToolStripMenuItem.Name = "releaseCalculatorToolStripMenuItem";
			this.releaseCalculatorToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.releaseCalculatorToolStripMenuItem.Text = "Release Calculator";
			this.releaseCalculatorToolStripMenuItem.Visible = false;
			this.releaseCalculatorToolStripMenuItem.Click += new System.EventHandler(this.butReleaseCalc_Click);
			// 
			// jobOverviewToolStripMenuItem
			// 
			this.jobOverviewToolStripMenuItem.Name = "jobOverviewToolStripMenuItem";
			this.jobOverviewToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.jobOverviewToolStripMenuItem.Text = "Job Overview";
			this.jobOverviewToolStripMenuItem.Visible = false;
			this.jobOverviewToolStripMenuItem.Click += new System.EventHandler(this.jobOverviewToolStripMenuItem_Click);
			// 
			// bugSubmissionsToolStripMenuItem
			// 
			this.bugSubmissionsToolStripMenuItem.Name = "bugSubmissionsToolStripMenuItem";
			this.bugSubmissionsToolStripMenuItem.Size = new System.Drawing.Size(109, 20);
			this.bugSubmissionsToolStripMenuItem.Text = "Bug Submissions";
			this.bugSubmissionsToolStripMenuItem.Click += new System.EventHandler(this.butBugSubs_Click);
			// 
			// timerUserSearch
			// 
			this.timerUserSearch.Interval = 500;
			this.timerUserSearch.Tick += new System.EventHandler(this.timerUserSearch_Tick);
			// 
			// butSearch
			// 
			this.butSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butSearch.Location = new System.Drawing.Point(1137, 29);
			this.butSearch.Name = "butSearch";
			this.butSearch.Size = new System.Drawing.Size(52, 24);
			this.butSearch.TabIndex = 248;
			this.butSearch.Text = "Search";
			this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
			// 
			// butBack
			// 
			this.butBack.Enabled = false;
			this.butBack.Image = global::OpenDental.Properties.Resources.Left;
			this.butBack.Location = new System.Drawing.Point(349, 29);
			this.butBack.Name = "butBack";
			this.butBack.Size = new System.Drawing.Size(27, 24);
			this.butBack.TabIndex = 249;
			this.butBack.Click += new System.EventHandler(this.butBack_Click);
			// 
			// butForward
			// 
			this.butForward.Enabled = false;
			this.butForward.Image = global::OpenDental.Properties.Resources.Right;
			this.butForward.Location = new System.Drawing.Point(382, 29);
			this.butForward.Name = "butForward";
			this.butForward.Size = new System.Drawing.Size(27, 24);
			this.butForward.TabIndex = 250;
			this.butForward.Click += new System.EventHandler(this.butForward_Click);
			// 
			// butRefresh
			// 
			this.butRefresh.Enabled = false;
			this.butRefresh.Location = new System.Drawing.Point(415, 29);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(49, 24);
			this.butRefresh.TabIndex = 250;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// contextMenuPatternReview
			// 
			this.contextMenuPatternReview.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemNone,
            this.menuItemNotNeeded,
            this.menuItemAwaiting,
            this.menuItemApproved,
            this.menuItemTentative,
            this.menuItemNotApproved});
			// 
			// menuItemNone
			// 
			this.menuItemNone.Index = 0;
			this.menuItemNone.Text = "None";
			this.menuItemNone.Click += new System.EventHandler(this.menuItemNone_Click);
			// 
			// menuItemNotNeeded
			// 
			this.menuItemNotNeeded.Index = 1;
			this.menuItemNotNeeded.Text = "Not Needed";
			this.menuItemNotNeeded.Click += new System.EventHandler(this.menuItemNotNeeded_Click);
			// 
			// menuItemAwaiting
			// 
			this.menuItemAwaiting.Index = 2;
			this.menuItemAwaiting.Text = "Awaiting Approval";
			this.menuItemAwaiting.Click += new System.EventHandler(this.menuItemAwaiting_Click);
			// 
			// menuItemApproved
			// 
			this.menuItemApproved.Index = 3;
			this.menuItemApproved.Text = "Approved";
			this.menuItemApproved.Click += new System.EventHandler(this.menuItemApproved_Click);
			// 
			// menuItemTentative
			// 
			this.menuItemTentative.Index = 4;
			this.menuItemTentative.Text = "Tentative";
			this.menuItemTentative.Click += new System.EventHandler(this.menuItemTentative_Click);
			// 
			// menuItemNotApproved
			// 
			this.menuItemNotApproved.Index = 5;
			this.menuItemNotApproved.Text = "Not Approved";
			this.menuItemNotApproved.Click += new System.EventHandler(this.menuItemNotApproved_Click);
			// 
			// FormJobManager
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1516, 825);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.butForward);
			this.Controls.Add(this.butBack);
			this.Controls.Add(this.butSearch);
			this.Controls.Add(this.textSearch);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.butAdvSearch);
			this.Controls.Add(this.butMe);
			this.Controls.Add(this.comboUser);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(1532, 864);
			this.Name = "FormJobManager";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Job Manager";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormJobManager_FormClosing);
			this.Load += new System.EventHandler(this.FormJobManager_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tabControlNav.ResumeLayout(false);
			this.tabPatternReview.ResumeLayout(false);
			this.tabAction.ResumeLayout(false);
			this.tabSpecialProjects.ResumeLayout(false);
			this.tabDocumentation.ResumeLayout(false);
			this.tabDocumentation.PerformLayout();
			this.tabTesting.ResumeLayout(false);
			this.tabTesting.PerformLayout();
			this.tabQuery.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tabNotify.ResumeLayout(false);
			this.tabSubscribed.ResumeLayout(false);
			this.tabNeedsEngineer.ResumeLayout(false);
			this.tabNeedsExpert.ResumeLayout(false);
			this.tabOnHold.ResumeLayout(false);
			this.tabSearch.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabTree.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.CheckBox checkCollapse;
		private System.Windows.Forms.Label labelGroupBy;
		private System.Windows.Forms.TreeView treeJobs;
		private System.Windows.Forms.ComboBox comboGroup;
		private InternalTools.Job_Manager.UserControlJobEdit userControlJobEdit;
		private UI.Button butAdvSearch;
		private System.Windows.Forms.ComboBox comboCategorySearch;
		private System.Windows.Forms.Label labelCategory;
		private UI.ODGrid gridAction;
		private System.Windows.Forms.TabControl tabControlNav;
		private System.Windows.Forms.TabPage tabAction;
		private System.Windows.Forms.ComboBox comboUser;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TabPage tabTree;
		private System.Windows.Forms.CheckBox checkShowUnassigned;
		private UI.Button butMe;
		private System.Windows.Forms.TextBox textSearch;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox checkResults;
		private System.Windows.Forms.TabPage tabNeedsEngineer;
		private UI.ODGrid gridAvailableJobs;
		private System.Windows.Forms.TabPage tabNeedsExpert;
		private UI.ODGrid gridAvailableJobsExpert;
		private System.Windows.Forms.TabPage tabOnHold;
		private UI.ODGrid gridJobsOnHold;
		private InternalTools.Job_Manager.UserControlQueryEdit userControlQueryEdit;
		private System.Windows.Forms.TabPage tabQuery;
		private UI.ODGrid gridQueries;
		private System.Windows.Forms.CheckBox checkShowQueryCancelled;
		private System.Windows.Forms.CheckBox checkShowQueryComplete;
		private System.Windows.Forms.TabPage tabDocumentation;
		private UI.ODGrid gridDocumentation;
		private System.Windows.Forms.TabPage tabNotify;
		private UI.ODGrid gridNotify;
		private System.Windows.Forms.ContextMenu contextMenuQueries;
		private System.Windows.Forms.MenuItem menuGoToAccount;
		private System.Windows.Forms.TabPage tabSubscribed;
		private UI.ODGrid gridSubscribedJobs;
		private System.Windows.Forms.CheckBox checkSubscribedIncludeOnHold;
		private System.Windows.Forms.Timer timerSearch;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem addJobToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addChildJobToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dashboardToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem releaseCalculatorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem bugSubmissionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem jobTimeHelperToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem jobOverviewToolStripMenuItem;
		private System.Windows.Forms.TabPage tabTesting;
		private UI.ODGrid gridTesting;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textVersionText;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textDocumentationVersion;
		private System.Windows.Forms.Timer timerDocumentationVersion;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DateTimePicker dateTo;
		private System.Windows.Forms.DateTimePicker dateFrom;
		private System.Windows.Forms.ToolStripMenuItem backportToolStripMenuItem;
		private System.Windows.Forms.CheckBox checkShowAllUsers;
		private System.Windows.Forms.CheckBox checkHideTested;
		private System.Windows.Forms.CheckBox checkIncludeCustContact;
		private System.Windows.Forms.TabPage tabSearch;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox textUserSearch;
		private System.Windows.Forms.Label label11;
		private UI.ComboBoxPlus comboPrioritySearch;
		private System.Windows.Forms.Label label10;
		private UI.ComboBoxPlus comboCatSearch;
		private System.Windows.Forms.Label label9;
		private UI.ODGrid gridSearch;
		private System.Windows.Forms.Timer timerUserSearch;
		private System.Windows.Forms.CheckBox checkContactSearch;
		private System.Windows.Forms.TabPage tabSpecialProjects;
		private System.Windows.Forms.CheckBox checkShowUnassignedSpecial;
		private UI.ODGrid gridSpecial;
		private UI.Button butQueriesRefresh;
		private UI.Button butTestingRefresh;
		private UI.Button butSearch;
		private System.Windows.Forms.TabPage tabPatternReview;
		private UI.ODGrid gridPatternReview;
		private UI.Button butBack;
		private UI.Button butForward;
		private UI.Button butRefresh;
		private System.Windows.Forms.Label label15;
		private UI.ComboBoxPlus comboProposedVersionNeedsAction;
		private System.Windows.Forms.Label label7;
		private UI.ComboBoxPlus comboProposedVersionNeedsEngineer;
		private System.Windows.Forms.Label label8;
		private UI.ComboBoxPlus comboProposedVersionNeedsExpert;
		private System.Windows.Forms.Label label12;
		private UI.ComboBoxPlus comboProposedVersionSearch;
		private System.Windows.Forms.CheckBox checkHideNotTested;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.DateTimePicker dateExcludeCompleteBefore;
		private System.Windows.Forms.ContextMenu contextMenuPatternReview;
		private System.Windows.Forms.MenuItem menuItemNone;
		private System.Windows.Forms.MenuItem menuItemNotNeeded;
		private System.Windows.Forms.MenuItem menuItemAwaiting;
		private System.Windows.Forms.MenuItem menuItemApproved;
		private System.Windows.Forms.MenuItem menuItemTentative;
		private System.Windows.Forms.MenuItem menuItemNotApproved;
	}
}