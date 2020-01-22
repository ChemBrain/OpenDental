using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormJobManager:ODForm {
		///<summary>All jobs</summary>
		private List<Job> _listJobsAll=new List<Job>();
		///<summary>Jobs to be displayed in tree.</summary>
		private List<Job> _listJobsFiltered=new List<Job>();
		///<summary>Jobs to be highlighted in the tree.</summary>
		private List<long> _listJobNumsHighlight=new List<long>();
		///<summary>Cached permissions for Job Manager.</summary>
		private List<JobPermission> _listJobPermissionsAll=new List<JobPermission>();
		private bool _isOverride;
		///<summary>Dictionary containing row notes shown when hovering.
		///Key		=> Row index
		///Value	=> Note for the row.</summary>
		private Dictionary<int,List<string>> _dicRowNotes=new Dictionary<int, List<string>>();
		///<summary>Used when hovering to show flag explanations.
		///Object tag is the location of the mouse the last time the tip was shown. Used to reduce redraw and flicker.</summary>
		private ToolTip _toolTipHover=new ToolTip();
		private List<Def> _listJobPriorities;
		private List<TabPage> _listHiddenTabs;
		private List<Userod> _listUsers;
		private Stack<long> _stackBackJobNums=new Stack<long>();
		private Stack<long> _stackForwardJobNums=new Stack<long>();
		private static List<FormJobEdit> _listJobEditForms=new List<FormJobEdit>();
		///<summary>Grid themes are gone.  This static color gets reused here, as needed.</summary>
		private static Color _colorGridHeaderBack=Color.FromArgb(223,234,245);
		///<summary>Filled with JobActions that will be collapsed when filling the Needs Action grid.</summary>
		private List<JobAction> _listJobActionsCollapsed=new List<JobAction>();

		private Brush _brushDefault {
			get {
				return SystemBrushes.Control;
			}
		}

		private Brush _brushSelected {
			get {
				return Brushes.White;
			}
		}

		private Brush _brushNotify {
			get {
				return Brushes.LightSalmon;
			}
		}

		public FormJobManager() {
			InitializeComponent();
			//This is here so we can see the tabs in the designer, but use the ownerdraw when the program is run.
			tabControlNav.DrawMode=TabDrawMode.OwnerDrawFixed;
		}

		private void FormJobManager_Load(object sender,EventArgs e) {
			comboUser.Tag=Security.CurUser;
			if(Security.IsAuthorized(Permissions.SecurityAdmin,true)) {
				releaseCalculatorToolStripMenuItem.Visible=true;
				jobOverviewToolStripMenuItem.Visible=true;
			}
			_listUsers=Userods.GetUsersForJobs();
			FillPriorityList();
			FillComboUser();
			#region Fill Proposed Version Combos
			comboProposedVersionNeedsAction.Items.Add("All");
			comboProposedVersionNeedsEngineer.Items.Add("All");
			comboProposedVersionNeedsExpert.Items.Add("All");
			comboProposedVersionSearch.Items.Add("All");
			List<string> listProposedVersions=Enum.GetNames(typeof(JobProposedVersion)).ToList();
			foreach(string proposedVersion in listProposedVersions) {
				comboProposedVersionNeedsAction.Items.Add(proposedVersion,(JobProposedVersion)Enum.Parse(typeof(JobProposedVersion),proposedVersion));
				comboProposedVersionNeedsEngineer.Items.Add(proposedVersion,(JobProposedVersion)Enum.Parse(typeof(JobProposedVersion),proposedVersion));
				comboProposedVersionNeedsExpert.Items.Add(proposedVersion,(JobProposedVersion)Enum.Parse(typeof(JobProposedVersion),proposedVersion));
				comboProposedVersionSearch.Items.Add(proposedVersion,(JobProposedVersion)Enum.Parse(typeof(JobProposedVersion),proposedVersion));
			}
			comboProposedVersionNeedsAction.SelectedIndex=0;
			comboProposedVersionNeedsEngineer.SelectedIndex=0;
			comboProposedVersionNeedsExpert.SelectedIndex=0;
			comboProposedVersionSearch.SelectedIndex=0;
			#endregion
			comboCategorySearch.Items.Add("All");
			comboCatSearch.Items.Add("All");
			List<string> listJobCategories=Enum.GetNames(typeof(JobCategory)).ToList();
			foreach(string jobCategory in listJobCategories) {
				comboCategorySearch.Items.Add(jobCategory);
				comboCatSearch.Items.Add(jobCategory,(JobCategory)Enum.Parse(typeof(JobCategory),jobCategory));
			}
			comboCategorySearch.SelectedIndex=0;
			comboCatSearch.SelectedIndex=0;
			comboPrioritySearch.Items.Add("All");
			List<Def> jobPriorities=Defs.GetCatList((int)DefCat.JobPriorities).ToList();
			foreach(Def jobPriority in jobPriorities) {
				comboPrioritySearch.Items.Add(jobPriority.ItemName,jobPriority);
			}
			comboPrioritySearch.SelectedIndex=0;
			Enum.GetNames(typeof(GroupJobsBy)).ToList().ForEach(x => comboGroup.Items.Add(x));
			comboGroup.SelectedIndex=(int)GroupJobsBy.Hierarchy;
			if(!JobPermissions.IsAuthorized(JobPerm.Concept,true) && !JobPermissions.IsAuthorized(JobPerm.QueryTech,true)) {
				addJobToolStripMenuItem.Enabled=false;
				addChildJobToolStripMenuItem.Enabled=false;
			}
			dateExcludeCompleteBefore.Value=DateTime.Now.AddMonths(-3);
			_listHiddenTabs=new List<TabPage>();
			RefreshTabs();//This speeds up RefreshAndFillThreaded since it will remove some of the grids.
			RefreshAndFillThreaded();
			_toolTipHover.OwnerDraw=true;
			_toolTipHover.Draw += new DrawToolTipEventHandler(toolTipHover_Draw);
			_toolTipHover.Popup += new PopupEventHandler(toolTipHover_PopupHelper);
			gridTesting.ContextMenu=new ContextMenu();
			gridTesting.ContextMenu.MenuItems.Add("Assign Tester",(o,arg) => menuItemAssignTester_Click(o,arg));
			gridTesting.ContextMenu.MenuItems.Add("Mark As Tested",(o,arg) => menuItemMarkAsTested_Click(o,arg));
			gridTesting.ContextMenu.MenuItems.Add("Clear Tested",(o,arg) => menuItemClearTested_Click(o,arg));
			try {
				Version lastVersion=new Version(VersionReleases.GetLastReleases(1));
				textVersionText.Text=lastVersion.Major+"."+lastVersion.Minor;
			}
			catch {
				textVersionText.Text="";
			}
			comboCategorySearch.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
			comboCatSearch.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
			comboPrioritySearch.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
		}

		private void comboBox_MouseWheel(object sender, MouseEventArgs e) {
			ComboBox comboControl = (ComboBox)sender;
			if(!comboControl.DroppedDown) {
				((HandledMouseEventArgs)e).Handled = true;
			}
		}

		private void RefreshTabs() {
			#region JobPerm.Concept
			if(!JobPermissions.IsAuthorized(JobPerm.Concept,true)) {
				if(tabControlNav.TabPages.Contains(tabAction)) {
					tabControlNav.TabPages.Remove(tabAction);
					_listHiddenTabs.Add(tabAction);
				}
				if(tabControlNav.TabPages.Contains(tabNeedsExpert)) {
					tabControlNav.TabPages.Remove(tabNeedsExpert);
					_listHiddenTabs.Add(tabNeedsExpert);
				}
				if(tabControlNav.TabPages.Contains(tabNeedsEngineer)) {
					tabControlNav.TabPages.Remove(tabNeedsEngineer);
					_listHiddenTabs.Add(tabNeedsEngineer);
				}
			}
			else {
				if(_listHiddenTabs.Contains(tabAction)) {
					tabControlNav.TabPages.Add(tabAction);
					_listHiddenTabs.Remove(tabAction);
				}
				if(_listHiddenTabs.Contains(tabNeedsExpert)) {
					tabControlNav.TabPages.Add(tabNeedsExpert);
					_listHiddenTabs.Remove(tabNeedsExpert);
				}
				if(_listHiddenTabs.Contains(tabNeedsEngineer)) {
					tabControlNav.TabPages.Add(tabNeedsEngineer);
					_listHiddenTabs.Remove(tabNeedsEngineer);
				}
			}
			#endregion
			#region JobPerm.QueryTech
			if(!JobPermissions.IsAuthorized(JobPerm.QueryTech,true)) {
				if(tabControlNav.TabPages.Contains(tabQuery)) {
					tabControlNav.TabPages.Remove(tabQuery);
					_listHiddenTabs.Add(tabQuery);
				}
			}
			else {
				if(_listHiddenTabs.Contains(tabQuery)) {
					tabControlNav.TabPages.Add(tabQuery);
					_listHiddenTabs.Remove(tabQuery);
				}
			}
			#endregion
			#region JobPerm.Documentation
			if(!JobPermissions.IsAuthorized(JobPerm.Documentation,true)) {
				if(tabControlNav.TabPages.Contains(tabDocumentation)) {
					tabControlNav.TabPages.Remove(tabDocumentation);
					_listHiddenTabs.Add(tabDocumentation);
				}
			}
			else {
				if(_listHiddenTabs.Contains(tabDocumentation)) {
					tabControlNav.TabPages.Add(tabDocumentation);
					_listHiddenTabs.Remove(tabDocumentation);
				}
			}
			#endregion
			#region JobPerm.NotifyCustomer
			if(!JobPermissions.IsAuthorized(JobPerm.NotifyCustomer,true)) {
				if(tabControlNav.TabPages.Contains(tabNotify)) {
					tabControlNav.TabPages.Remove(tabNotify);
					_listHiddenTabs.Add(tabNotify);
				}
			}
			else {
				if(_listHiddenTabs.Contains(tabNotify)) {
					tabControlNav.TabPages.Add(tabNotify);
					_listHiddenTabs.Remove(tabNotify);
				}
			}
			#endregion
			#region JobPerm.TestingCoordinator
			if(!JobPermissions.IsAuthorized(JobPerm.TestingCoordinator,true)) {
				if(tabControlNav.TabPages.Contains(tabTesting)) {
					tabControlNav.TabPages.Remove(tabTesting);
					_listHiddenTabs.Add(tabTesting);
				}
			}
			else {
				if(_listHiddenTabs.Contains(tabTesting)) {
					tabControlNav.TabPages.Add(tabTesting);
					_listHiddenTabs.Remove(tabTesting);
				}
			}
			#endregion
			#region JobPerm.Approval
			if(!JobPermissions.IsAuthorized(JobPerm.Approval,true)) {
				if(tabControlNav.TabPages.Contains(tabOnHold)) {
					tabControlNav.TabPages.Remove(tabOnHold);
					_listHiddenTabs.Add(tabOnHold);
				}
			}
			else {
				if(_listHiddenTabs.Contains(tabOnHold)) {
					tabControlNav.TabPages.Add(tabOnHold);
					_listHiddenTabs.Remove(tabOnHold);
				}
			}
			#endregion
			#region JobPerm.SpecialProject
			if(!JobPermissions.IsAuthorized(JobPerm.SpecialProject,true)) {
				if(tabControlNav.TabPages.Contains(tabSpecialProjects)) {
					tabControlNav.TabPages.Remove(tabSpecialProjects);
					_listHiddenTabs.Add(tabSpecialProjects);
				}
			}
			else {
				if(_listHiddenTabs.Contains(tabSpecialProjects)) {
					tabControlNav.TabPages.Add(tabSpecialProjects);
					_listHiddenTabs.Remove(tabSpecialProjects);
				}
			}
			#endregion
			#region JobPerm.PatternReview
			if(!JobPermissions.IsAuthorized(JobPerm.PatternReview,true)) {
				if(tabControlNav.TabPages.Contains(tabPatternReview)) {
					tabControlNav.TabPages.Remove(tabPatternReview);
					_listHiddenTabs.Add(tabPatternReview);
				}
			}
			else {
				if(_listHiddenTabs.Contains(tabPatternReview)) {
					tabControlNav.TabPages.Add(tabPatternReview);
					_listHiddenTabs.Remove(tabPatternReview);
				}
			}
			#endregion
		}

		private void tabControlNav_DrawItem(object sender,DrawItemEventArgs e) {
			TabPage page=tabControlNav.TabPages[e.Index];
			//We only care about changing the color for the Subscribed Jobs tab
			bool hasNotifications=_listJobsAll.Where(x => x.ListJobLinks.Exists(y => y.LinkType==JobLinkType.Subscriber && y.FKey==Security.CurUser.UserNum) 
			&& x.ListJobNotifications.Exists(y => y.UserNum==Security.CurUser.UserNum)).Count()>0;
			if(page.Equals(tabSubscribed) && hasNotifications) {
				e.Graphics.FillRectangle(_brushNotify,e.Bounds);
			}
			else if(e.State == DrawItemState.Selected) {
				e.Graphics.FillRectangle(_brushSelected,e.Bounds);
			}
			else {
				e.Graphics.FillRectangle(_brushDefault,e.Bounds);
			}
			Rectangle paddedBounds=e.Bounds;
			int yOffset=(e.State == DrawItemState.Selected) ? -2 : 1;
			paddedBounds.Offset(1,yOffset);
			TextRenderer.DrawText(e.Graphics,page.Text,Font,paddedBounds,page.ForeColor);
		}

		private void FillPriorityList() {
			_listJobPriorities=Defs.GetDefsForCategory(DefCat.JobPriorities);
			List<string> listPriorityItemValues=_listJobPriorities.SelectMany(y => y.ItemValue.Split(',')).ToList();
			if(_listJobPriorities.Count<1
				|| !listPriorityItemValues.Contains("OnHold")
				|| !listPriorityItemValues.Contains("Low")
				|| !listPriorityItemValues.Contains("Normal")
				|| !listPriorityItemValues.Contains("MediumHigh")
				|| !listPriorityItemValues.Contains("High")
				|| !listPriorityItemValues.Contains("Urgent")
				|| !listPriorityItemValues.Contains("JobDefault")
				|| !listPriorityItemValues.Contains("BugDefault")
				|| !listPriorityItemValues.Contains("DocumentationDefault"))
			{
				MsgBox.Show(this,"Job priority definition is not currently set up in a way that the JobManager can function");
				Close();
				return;
			}
		}

		private void FillComboUser() {
			Userod userCur=(Userod)comboUser.Tag;
			comboUser.SelectedIndex=-1;
			comboUser.Items.Clear();
			comboUser.Items.Add("All");//All is first.
			comboUser.Items.Add("Unassigned");
			if(userCur.UserNum==0) {//All
				comboUser.SelectedIndex=0;
			}
			else if(userCur.UserNum==-1) {//Unassigned
				comboUser.SelectedIndex=1;
			}
			for(int i=0;i<_listUsers.Count;i++) {
				comboUser.Items.Add(_listUsers[i].UserName);
				if(userCur.UserNum==_listUsers[i].UserNum) {
					comboUser.SelectedIndex=i+2;
				}
			}
			if(comboUser.SelectedIndex==-1) {
				comboUser.SelectedIndex=1;
			}
			this.Text="Job Manager"+(comboUser.Text=="" ? "" : " - "+comboUser.Text);
		}

		///<summary>Loads whatever job control is necessary in order to display the job within the manager.
		///Also refreshes the cache with the job passed in and updates all corresponding controls and grids.
		///This method will not load the passed in job if the current job cannot be saved correctly.</summary>
		private void LoadJob(Job job,bool doRefreshUI,LoadJobAction loadAction=LoadJobAction.Select) {
			if(job==null || JobUnsavedChangesCheck() || QueryUnsavedChangesCheck()) {
				return;
			}
			#region Load Related Jobs
			Dictionary<long,Job> dictRefreshJobs=new Dictionary<long,Job>() { { job.JobNum,job } };
			List<Job> listRelatedJobs=Jobs.GetJobsForOneLayer(job);
			Jobs.FillInMemoryLists(listRelatedJobs);
			foreach(Job jobRelated in listRelatedJobs) {
				dictRefreshJobs[jobRelated.JobNum]=jobRelated;
			}
			#endregion
			RefreshControlsAndCacheWithJobs(dictRefreshJobs,doRefreshUI);
			#region Back/Forward Nav logic
			long jobNumCur;
			if(userControlJobEdit.Visible) {
				jobNumCur=userControlJobEdit.GetJob()?.JobNum??0;
			}
			else {
				jobNumCur=userControlQueryEdit.GetJob()?.JobNum??0;
			}
			switch(loadAction) {
				case(LoadJobAction.Select):
					if(jobNumCur!=0) {
						_stackBackJobNums.Push(jobNumCur);
						butBack.Enabled=true;
					}
					if(_stackForwardJobNums.Count>0 && _stackForwardJobNums.Peek()==job.JobNum) {
						_stackForwardJobNums.Pop();
					}
					else {
						_stackForwardJobNums.Clear();
					}
					if(_stackForwardJobNums.Count==0) {
						butForward.Enabled=false;
					}
					break;
				case(LoadJobAction.Back):
					if(_stackBackJobNums.Count==0) {
						butBack.Enabled=false;
					}
					if(jobNumCur!=0) {
						_stackForwardJobNums.Push(jobNumCur);
						butForward.Enabled=true;
					}
					break;
				case(LoadJobAction.Forward):
					if(_stackForwardJobNums.Count==0) {
						butForward.Enabled=false;
					}
					if(jobNumCur!=0) {
						_stackBackJobNums.Push(jobNumCur);
						butBack.Enabled=true;
					}
					break;
			}
			#endregion
			//Forcefully load the new job into whatever control it needs to be loaded into in order to be interacted with.
			if(job.Category==JobCategory.Query) {
				userControlQueryEdit.Visible=true;
				userControlJobEdit.Visible=false;
				userControlQueryEdit.LoadJob(job,Jobs.GetJobTree(job,_listJobsAll),_listJobsAll);
				butRefresh.Enabled=true;
			}
			else {
				userControlQueryEdit.Visible=false;
				userControlJobEdit.Visible=true;
				userControlJobEdit.LoadJob(job,Jobs.GetJobTree(job,_listJobsAll),_listJobsAll);
				butRefresh.Enabled=true;
			}
		}

		private void RefreshAndFill(List<long> listJobNums=null) {
			List<Job> listJobs = new List<Job>();
			//Make a dictionary that will hold a key for every known JobNum (especially for when specific JobNums are passed in).
			//This is mainly for removing jobs that have been deleted when refreshing for signal processing purposes.
			Dictionary<long,Job> dictRefreshJobs;
			//Get all pertinent jobs related to the job manager if no specific JobNums were specified.
			if(listJobNums.IsNullOrEmpty()) {
				listJobs=Jobs.GetAllForManager();
				Jobs.FillInMemoryLists(listJobs);
				dictRefreshJobs=listJobs.ToDictionary(x => x.JobNum,x => x);
				//This is most likely the first time the job manager has been launched.  Always override the filtered list of jobs here.
				_listJobsFiltered=listJobs.Select(x => x.Copy()).ToList();
			}
			else {
				listJobNums=listJobNums.Distinct().ToList();
				listJobs=Jobs.GetMany(listJobNums);
				Jobs.FillInMemoryLists(listJobs);
				//Make the dictionary out of listJobNums but fill it with jobs that were returned from the database.
				//This will allow us to leave some values as null if the job was deleted.  This will cause us to remove the job from our cache later.
				dictRefreshJobs=listJobNums.ToDictionary(x => x,x => listJobs.FirstOrDefault(y => y.JobNum==x));
			}
			_listJobPermissionsAll=JobPermissions.GetList();
			RefreshControlsAndCacheWithJobs(dictRefreshJobs);
		}


		///<summary>Fills all in memory data from the DB on a seperate thread and then refills controls.
		///Optionally pass in a list of job nums to only refresh those specific jobs.</summary>
		private void RefreshAndFillThreaded(List<long> listJobNums=null) {
			ODThread thread=new ODThread((o) => {
				RefreshAndFill(listJobNums);
			});
			thread.GroupName="RefreshAndFillJobManager";
			thread.Name="RefreshAndFillJobManager";
			thread.AddExceptionHandler((ex) => { FriendlyException.Show("Error refreshing the Job Manager",ex); });
			thread.Start();
		}

		///<summary>Pass in a dictionary of JobNums and their corresponding Job object.
		///It is acceptable to have the value portion of the dictionary be null to indicate that a specific JobNum has been deleted.
		///Refreshes all user controls and grids with the new jobs that are passed in.
		///Optionally set doRefreshUI false in order to skip refreshing all of the UI elements.
		///This is useful when the UI elements already know about the job in question (e.g. single clicking on a job from a grid).</summary>
		private void RefreshControlsAndCacheWithJobs(Dictionary<long,Job> dictRefreshJobs,bool doRefreshUI=true) {
			if(dictRefreshJobs==null) {
				return;//Nothing to do.
			}
			if(this.InvokeRequired) {
				this.Invoke(() => RefreshControlsAndCacheWithJobs(dictRefreshJobs,doRefreshUI));
				return;
			}
			#region Update Job in Controls
			//Get the job currently loaded into the UserControlJobEdit control.
			Job jobCur=userControlJobEdit.GetJob();
			//Check to see if the current job is included in the list of jobs we need to refresh (could be due to processing a signal).
			if(jobCur!=null && dictRefreshJobs.ContainsKey(jobCur.JobNum)) {
				//Set jobCur to the new version of the job that was just retrieved from the database.
				jobCur=dictRefreshJobs[jobCur.JobNum];//Could be null if someone else just deleted this job...
			}
			if(jobCur!=null) {//Merge the new job with the control if the job was found in our dictionary of jobs to refresh or wasn't deleted.
				userControlJobEdit.LoadMergeJob(jobCur);
			}
			//Get the job currently loaded into the UserControlQueryEdit control.
			jobCur=userControlQueryEdit.GetJob();
			//Check to see if the current job is included in the list of jobs we need to refresh (could be due to processing a signal).
			if(jobCur!=null && dictRefreshJobs.ContainsKey(jobCur.JobNum)) {
				//Set jobCur to the new version of the job that was just retrieved from the database.
				jobCur=dictRefreshJobs[jobCur.JobNum];//Could be null if someone else just deleted this job...
			}
			if(jobCur!=null) {//Merge the new job with the control if the job was found in our dictionary of jobs to refresh or wasn't deleted.
				userControlQueryEdit.LoadMergeJob(jobCur);
			}
			#endregion
			#region Update Cached Lists
			foreach(KeyValuePair<long,Job> kvp in dictRefreshJobs) {
				if(kvp.Value==null) {//deleted job
					_listJobsAll.RemoveAll(x => x.JobNum==kvp.Key);
					_listJobsFiltered.RemoveAll(x => x.JobNum==kvp.Key);
					List<TreeNode> treeNodes=new List<TreeNode>(treeJobs.Nodes.Cast<TreeNode>());
					for(int i=0;i<treeNodes.Count;i++) {//flat recursion
						TreeNode nodeCur=treeNodes[i];
						if((nodeCur.Tag is Job) && ((Job)nodeCur.Tag).JobNum==kvp.Key) {
							nodeCur.Text="(Deleted) - "+nodeCur.Text;//update label text to indicate deleted.
							nodeCur.Tag=null;
						}
						treeNodes.AddRange(nodeCur.Nodes.Cast<TreeNode>());
					}
					continue;
				}
				//Master Job List
				Job jobOld=_listJobsAll.FirstOrDefault(x => x.JobNum==kvp.Key);
				if(jobOld==null) {//new job entirely, no need to update anything in memory, just add to jobs list.
					_listJobsAll.Add(kvp.Value);
					continue;
				}
				_listJobsAll[_listJobsAll.IndexOf(jobOld)]=kvp.Value;
				//Filtered Job List
				jobOld=_listJobsFiltered.FirstOrDefault(x => x.JobNum==kvp.Key);
				if(jobOld!=null) {//update item in filtered list.
					_listJobsFiltered[_listJobsFiltered.IndexOf(jobOld)]=kvp.Value;
				}
				//Jobs in tree
				UpdateNodes(kvp.Value);
			}
			#endregion
			#region Refresh UI Elements
			if(doRefreshUI) {
				FillActiveTabGrid();
				RefreshTabs();
			}
			#endregion
		}

		private void tabControlNav_Selecting(object sender,TabControlCancelEventArgs e) {
			FillActiveTabGrid();
		}

		///<summary>Refreshes the grid in the active tab. Should be the only reference to any FillGrid in FormJobManager</summary>
		private void FillActiveTabGrid() {
			Cursor=Cursors.WaitCursor;
			if(tabControlNav.SelectedTab==tabAction) {
				FillGridActions();
			}
			else if(tabControlNav.SelectedTab==tabDocumentation) {
				FillGridDocumentation();
			}
			else if(tabControlNav.SelectedTab==tabNeedsEngineer) {
				FillGridNeedsEngineer();
			}
			else if(tabControlNav.SelectedTab==tabNeedsExpert) {
				FillGridNeedsExpert();
			}
			else if(tabControlNav.SelectedTab==tabNotify) {
				FillGridNotify();
			}
			else if(tabControlNav.SelectedTab==tabOnHold) {
				FillGridOnHold();
			}
			else if(tabControlNav.SelectedTab==tabPatternReview) {
				FillGridPatternReview();
			}
			else if(tabControlNav.SelectedTab==tabQuery) {
				FillGridQueries();
			}
			else if(tabControlNav.SelectedTab==tabSearch) {
				FillGridSearch();
			}
			else if(tabControlNav.SelectedTab==tabSpecialProjects) {
				FillGridSpecial();
			}
			else if(tabControlNav.SelectedTab==tabSubscribed) {
				FillGridSubscribed();
			}
			else if(tabControlNav.SelectedTab==tabTesting) {
				FillGridTesting();
			}
			else if(tabControlNav.SelectedTab==tabTree) {
				FilterAndFill();
			}
			Cursor=Cursors.Default;
		}

		#region FillGrids
		//All of these Fill methods should only be called from FillActiveTabGrid
		///<summary>Always fills from _ListJobsAll.</summary>
		private void FillGridActions() {
			if(!tabControlNav.TabPages.Contains(tabAction)) {
				return;
			}
			_dicRowNotes.Clear();
			Userod userFilter=Security.CurUser;
			if(comboUser.SelectedIndex==1) {
				userFilter=new Userod() { UserName="Unassigned",UserNum=0 };
			}
			//If all user set userFilter to null in order to get all jobs
			if(comboUser.SelectedIndex==0) {
				userFilter=null;
			}
			else if(comboUser.SelectedIndex>1) {
				userFilter=_listUsers[comboUser.SelectedIndex-2];
			}
			gridAction.Title="Action Items";
			tabAction.Text="Needs Action";
			checkShowUnassigned.Enabled=true;
			long selectedJobNum=0;
			if(userControlJobEdit.GetJob()!=null) {
				selectedJobNum=userControlJobEdit.GetJob().JobNum;
			}
			gridAction.BeginUpdate();
			gridAction.ListGridColumns.Clear();
			gridAction.ListGridColumns.Add(new GridColumn("Priority",50,HorizontalAlignment.Center));
			gridAction.ListGridColumns.Add(new GridColumn("Flag",30,HorizontalAlignment.Center));// X for yes, - for unassigned
			gridAction.ListGridColumns.Add(new GridColumn("Owner",65,HorizontalAlignment.Center));// X for yes, - for unassigned
			gridAction.ListGridColumns.Add(new GridColumn("",245));
			gridAction.ListGridRows.Clear();
			//Sort jobs into action dictionary
			Dictionary<JobAction,List<Job>> dictActions=new Dictionary<JobAction,List<Job>>();
			foreach(Job job in _listJobsAll) {
				JobAction action;
				if(userFilter==null) {
					action=job.OwnerAction;
				}
				else {
					action=job.ActionForUser(userFilter);
				}
				if(action.In(JobAction.Document,JobAction.NeedsTechnicalWriter,JobAction.ContactCustomer,JobAction.ContactCustomerPreDoc)) {
					continue;
				}
				if(job.Category==JobCategory.Query || job.Category==JobCategory.SpecialProject) {
					continue;
				}
				if(comboProposedVersionNeedsAction.SelectedIndex!=0 && job.ProposedVersion!=comboProposedVersionNeedsAction.GetSelected<JobProposedVersion>()) {//All is not selected, only select the jobs with the specified proposedversion
					continue;
				}
				if(!dictActions.ContainsKey(action)) {
					dictActions[action]=new List<Job>();
				}
				dictActions[action].Add(job);
			}
			//sort dictionary so actions will appear in same order
			dictActions=dictActions.OrderBy(x => (int)x.Key).ToDictionary(x => x.Key,x => x.Value);
			foreach(KeyValuePair<JobAction,List<Job>> kvp in dictActions) {
				if(kvp.Key==JobAction.Undefined || kvp.Key==JobAction.UnknownJobPhase || kvp.Key==JobAction.None) {
					//Undefined occurs when there is no action to take. 
					//UnknownJobPhase occurs when there is something wrong with the programming.
					continue;
				}
				List<Job> listJobsSorted=kvp.Value//filter
					.OrderBy(x => userFilter==null || x.OwnerNum!=userFilter.UserNum)//sort
					.ThenBy(x => x.OwnerNum!=0)
					//This is the reverse order of the actual priority of different categories of jobs
					//Purposefully put in this order so they appear correctly in the list.
					.ThenBy(x => x.Category==JobCategory.NeedNoApproval)
					.ThenBy(x => x.Category==JobCategory.Research)
					.ThenBy(x => x.Category==JobCategory.Conversion)
					.ThenBy(x => x.Category==JobCategory.HqRequest)
					.ThenBy(x => x.Category==JobCategory.InternalRequest)
					.ThenBy(x => x.Category==JobCategory.Feature)
					.ThenBy(x => x.Category==JobCategory.Query)
					.ThenBy(x => x.Category==JobCategory.ProgramBridge)
					.ThenBy(x => x.Category==JobCategory.Enhancement)
					.ThenBy(x => x.Category==JobCategory.Bug)
					.ThenBy(x => _listJobPriorities.FirstOrDefault(y => y.DefNum==x.Priority).ItemOrder).ToList();
				if(!checkShowUnassigned.Checked) {
					listJobsSorted.RemoveAll(x => x.Priority==_listJobPriorities.FirstOrDefault(y => y.ItemValue.Contains("OnHold")).DefNum);
					if(userFilter!=null) {
						//Actions in this list will be filtered by checkShowUnassigned. If not in this list, items will always show if applicable 
						//(For example ApproveJob always shows if user has approval permission.)
						JobAction[] JobActionsUnassigned=new JobAction[] {
							JobAction.WriteConcept,
							JobAction.WriteJob,
							JobAction.WriteCode,
							JobAction.TakeJob,
							JobAction.ReviewCode
						};
						if(userFilter.UserNum>0 && JobActionsUnassigned.Contains(kvp.Key)) {
							listJobsSorted.RemoveAll(x => x.OwnerNum==0 || kvp.Key==JobAction.TakeJob);//filters out passive actions if unassigned. Bug.
						}
					}
				}
				if(listJobsSorted.Count==0) {
					continue;
				}
				//Add a 'category title' row to the grid which will have the corresponding JobAction enum as its tag.
				//Always show the count of sorted jobs within each job action category regardless if the category is collapsed or not.
				string jobActionRowTitleStr=$"{kvp.Key.ToString()} ({listJobsSorted.Count})";
				gridAction.ListGridRows.Add(new GridRow("","","",jobActionRowTitleStr) { ColorBackG=_colorGridHeaderBack,Bold=true,Tag=kvp.Key });
				if(_listJobActionsCollapsed.Contains(kvp.Key)) {
					continue;
				}
				JobAction[] writeAdviseReview=new[] { JobAction.WriteCode,JobAction.ReviewCode,JobAction.WaitForReview,JobAction.Advise };
				Color changedColor=Security.CurUser.UserNum==9 ? Color.FromArgb(20,Color.LightGreen):Color.FromArgb(80,Color.LightGreen);
				foreach(Job job in listJobsSorted) {
					JobActiveLink activeLink=job.ListJobActiveLinks.FirstOrDefault(x => x.UserNum==userFilter?.UserNum && x.DateTimeEnd==DateTime.MinValue);
					Def jobPriority=_listJobPriorities.FirstOrDefault(y => y.DefNum==job.Priority);
					string ownerString=job.OwnerNum==0 ? "-" : Userods.GetName(job.OwnerNum);
					//If in ReviewCode (you are the reviewer for the job), add a string for who sent it to you
					if(kvp.Key==JobAction.ReviewCode) {
						ownerString+="\r\n("+Userods.GetName(job.UserNumEngineer)+")";
					}
					gridAction.ListGridRows.Add(
					new GridRow(
						new GridCell(jobPriority.ItemName) {
							ColorBackG=jobPriority.ItemColor,
							ColorText=(job.Priority==_listJobPriorities.FirstOrDefault(y => y.ItemValue.Contains("Urgent")).DefNum) ? Color.White : Color.Black,
						},
						new GridCell(FlagHelper(job,gridAction.ListGridRows.Count)) {
							ColorText=job.TagOD!=null ? (Color)job.TagOD : Color.Black,//Set in FlagCellHelper(...), tag is reset everytime FillGridActions() is called.
							ColorBackG=activeLink!=null?Color.FromArgb(90,Color.Cyan):Color.Empty,
							},
						new GridCell(ownerString),
						new GridCell(job.ToString()) { ColorBackG=(job.ToString().ToLower().Contains(textSearch.Text.ToLower())&&!string.IsNullOrWhiteSpace(textSearch.Text) ? Color.LightYellow : 
							job.ListJobNotifications.Where(x => x.UserNum==Security.CurUser.UserNum).Count()>0 ? changedColor : Color.Empty) }
						) {
						Tag=job
					}
					);
				}
			}
			gridAction.EndUpdate();
			//RESELECT JOB
			if(selectedJobNum>0 && selectedJobNum==userControlJobEdit.GetJob().JobNum) {
				for(int i=0;i<gridAction.ListGridRows.Count;i++) {
					if((gridAction.ListGridRows[i].Tag is Job) && ((Job)gridAction.ListGridRows[i].Tag).JobNum==selectedJobNum) {
						gridAction.SetSelected(i,true);
						break;
					}
				}
			}
		}

		///<summary>Always fills from _ListJobsAll.</summary>
		private void FillGridSpecial() {
			if(!tabControlNav.TabPages.Contains(tabSpecialProjects)) {
				return;
			}
			_dicRowNotes.Clear();
			Userod userFilter=Security.CurUser;
			if(comboUser.SelectedIndex==1) {
				userFilter=new Userod() { UserName="Unassigned",UserNum=0 };
			}
			//If all user set userFilter to null in order to get all jobs
			if(comboUser.SelectedIndex==0) {
				userFilter=null;
			}
			else if(comboUser.SelectedIndex>1) {
				userFilter=_listUsers[comboUser.SelectedIndex-2];
			}
			gridSpecial.Title="Special Project Items";
			checkShowUnassigned.Enabled=true;
			long selectedJobNum=0;
			if(userControlJobEdit.GetJob()!=null) {
				selectedJobNum=userControlJobEdit.GetJob().JobNum;
			}
			gridSpecial.BeginUpdate();
			gridSpecial.ListGridColumns.Clear();
			gridSpecial.ListGridColumns.Add(new GridColumn("Priority",50,HorizontalAlignment.Center));
			gridSpecial.ListGridColumns.Add(new GridColumn("Flag",30,HorizontalAlignment.Center));// X for yes, - for unassigned
			gridSpecial.ListGridColumns.Add(new GridColumn("Owner",65,HorizontalAlignment.Center));// X for yes, - for unassigned
			gridSpecial.ListGridColumns.Add(new GridColumn("",245));
			gridSpecial.ListGridRows.Clear();
			//Sort jobs into action dictionary
			Dictionary<JobAction,List<Job>> dictActions=new Dictionary<JobAction,List<Job>>();
			foreach(Job job in _listJobsAll) {
				JobAction action;
				if(userFilter==null) {
					action=job.OwnerAction;
				}
				else {
					action=job.ActionForUser(userFilter);
				}
				if(action.In(JobAction.Document,JobAction.NeedsTechnicalWriter,JobAction.ContactCustomer,JobAction.ContactCustomerPreDoc)) {
					continue;
				}
				if(job.Category!=JobCategory.SpecialProject) {
					continue;
				}
				if(!dictActions.ContainsKey(action)) {
					dictActions[action]=new List<Job>();
				}
				dictActions[action].Add(job);
			}
			//sort dictionary so actions will appear in same order
			dictActions=dictActions.OrderBy(x => (int)x.Key).ToDictionary(x => x.Key,x => x.Value);
			foreach(KeyValuePair<JobAction,List<Job>> kvp in dictActions) {
				if(kvp.Key==JobAction.Undefined || kvp.Key==JobAction.UnknownJobPhase || kvp.Key==JobAction.None) {
					//Undefined occurs when there is no action to take. 
					//UnknownJobPhase occurs when there is something wrong with the programming.
					continue;
				}
				List<Job> listJobsSorted=kvp.Value//filter
					.OrderBy(x => userFilter==null || x.OwnerNum!=userFilter.UserNum)//sort
					.ThenBy(x => x.OwnerNum!=0)
					.ThenBy(x => _listJobPriorities.FirstOrDefault(y => y.DefNum==x.Priority).ItemOrder).ToList();
				if(!checkShowUnassigned.Checked) {
					listJobsSorted.RemoveAll(x => x.Priority==_listJobPriorities.FirstOrDefault(y => y.ItemValue.Contains("OnHold")).DefNum);
					if(userFilter!=null) {
						//Actions in this list will be filtered by checkShowUnassigned. If not in this list, items will always show if applicable 
						//(For example ApproveJob always shows if user has approval permission.)
						JobAction[] JobActionsUnassigned=new JobAction[] {
							JobAction.WriteConcept,
							JobAction.WriteJob,
							JobAction.WriteCode,
							JobAction.TakeJob,
							JobAction.ReviewCode
						};
						if(userFilter.UserNum>0 && JobActionsUnassigned.Contains(kvp.Key)) {
							listJobsSorted.RemoveAll(x => x.OwnerNum==0 || kvp.Key==JobAction.TakeJob);//filters out passive actions if unassigned. Bug.
						}
					}
				}
				if(listJobsSorted.Count==0) {
					continue;
				}
				gridSpecial.ListGridRows.Add(new GridRow("","","",kvp.Key.ToString()) { ColorBackG=_colorGridHeaderBack,Bold=true });
				JobAction[] writeAdviseReview=new[] { JobAction.WriteCode,JobAction.ReviewCode,JobAction.WaitForReview,JobAction.Advise };
				Color changedColor=Security.CurUser.UserNum==9 ? Color.FromArgb(20,Color.LightGreen):Color.FromArgb(80,Color.LightGreen);
				foreach(Job job in listJobsSorted) {
					Def jobPriority=_listJobPriorities.FirstOrDefault(y => y.DefNum==job.Priority);
					string ownerString=job.OwnerNum==0 ? "-" : Userods.GetName(job.OwnerNum);
					//If in ReviewCode (you are the reviewer for the job), add a string for who sent it to you
					if(kvp.Key==JobAction.ReviewCode) {
						ownerString+="\r\n("+Userods.GetName(job.UserNumEngineer)+")";
					}
					gridSpecial.ListGridRows.Add(
					new GridRow(
						new GridCell(jobPriority.ItemName) {
							ColorBackG=jobPriority.ItemColor,
							ColorText=(job.Priority==_listJobPriorities.FirstOrDefault(y => y.ItemValue.Contains("Urgent")).DefNum) ? Color.White : Color.Black,
						},
						new GridCell(FlagHelper(job,gridSpecial.ListGridRows.Count)) {
							ColorText=job.TagOD!=null ? (Color)job.TagOD : Color.Black//Set in FlagCellHelper(...), tag is reset everytime FillGridActions() is called.
							},
						new GridCell(ownerString),
						new GridCell(job.ToString()) { ColorBackG=(job.ToString().ToLower().Contains(textSearch.Text.ToLower())&&!string.IsNullOrWhiteSpace(textSearch.Text) ? Color.LightYellow : 
							job.ListJobNotifications.Where(x => x.UserNum==Security.CurUser.UserNum).Count()>0 ? changedColor : Color.Empty) }
						) {
						Tag=job
					}
					);
				}
			}
			gridSpecial.EndUpdate();
			//RESELECT JOB
			if(selectedJobNum>0 && selectedJobNum==userControlJobEdit.GetJob().JobNum) {
				for(int i=0;i<gridSpecial.ListGridRows.Count;i++) {
					if((gridSpecial.ListGridRows[i].Tag is Job) && ((Job)gridSpecial.ListGridRows[i].Tag).JobNum==selectedJobNum) {
						gridSpecial.SetSelected(i,true);
						break;
					}
				}
			}
		}

		///<summary>Always fills from _ListJobsAll.</summary>
		private void FillGridDocumentation() {
			if(!tabControlNav.TabPages.Contains(tabDocumentation)) {
				return;
			}
			_dicRowNotes.Clear();
			Userod userFilter=Security.CurUser;
			if(comboUser.SelectedIndex==1) {
				userFilter=new Userod() { UserName="Unassigned",UserNum=0 };
			}
			else if(comboUser.SelectedIndex>1) {
				userFilter=_listUsers[comboUser.SelectedIndex-2];
			}
			if(JobPermissions.IsAuthorized(JobPerm.NotifyCustomer,true,userFilter.UserNum)) {
				gridDocumentation.Title="Action Items";
				tabAction.Text="Needs Action";
			}
			else {
				gridDocumentation.Title="Jobs To Document";
				tabAction.Text="Needs Documentation";
			}
			long selectedJobNum=0;
			if(userControlJobEdit.GetJob()!=null) {
				selectedJobNum=userControlJobEdit.GetJob().JobNum;
			}
			gridDocumentation.BeginUpdate();
			gridDocumentation.ListGridColumns.Clear();
			gridDocumentation.ListGridColumns.Add(new GridColumn("Priority",50,HorizontalAlignment.Center));
			gridDocumentation.ListGridColumns.Add(new GridColumn("Version",95,HorizontalAlignment.Center));// X for yes, - for unassigned
			gridDocumentation.ListGridColumns.Add(new GridColumn("",205));
			gridDocumentation.ListGridRows.Clear();
			//Sort jobs into action dictionary
			Dictionary<string,List<Job>> dictActions=new Dictionary<string,List<Job>>();
			foreach(Job job in _listJobsAll) {
				JobAction action;
				if(userFilter==null) {
					action=job.OwnerAction;
				}
				else {
					action=job.ActionForUser(userFilter);
				}
				if(!action.In(JobAction.Document,JobAction.NeedsTechnicalWriter)) {
					continue;
				}
				if(job.Category==JobCategory.Query) {
					continue;
				}
				if(!String.IsNullOrEmpty(textDocumentationVersion.Text) && !job.JobVersion.Contains(textDocumentationVersion.Text)) {
					continue;
				}
				Userod user=new Userod() { UserName="Unassigned",UserNum=0 };
				if(action==JobAction.Document) {
					user=Userods.GetUser(job.UserNumDocumenter)??user;
				}
				if(!JobPermissions.IsAuthorized(JobPerm.DocumentationManager,true) && user.UserNum!=userFilter.UserNum && user.UserNum!=0) {
					continue;
				}
				if(!dictActions.ContainsKey(user.UserName)) {
					dictActions[user.UserName]=new List<Job>();
				}
				dictActions[user.UserName].Add(job);
			}
			//sort dictionary so actions will appear in same order
			dictActions=dictActions.OrderBy(x => x.Key).ToDictionary(x => x.Key,x => x.Value);
			foreach(KeyValuePair<string,List<Job>> kvp in dictActions) {
				List<Job> listJobsSorted=kvp.Value//filter
					.OrderBy(x => _listJobPriorities.FirstOrDefault(y => y.DefNum==x.Priority).ItemOrder).ToList();
				if(listJobsSorted.Count==0) {
					continue;
				}
				if(listJobsSorted.Count>0) {
					gridDocumentation.ListGridRows.Add(new GridRow("","",kvp.Key.ToString()) { ColorBackG=_colorGridHeaderBack,Bold=true });
					JobAction[] writeAdviseReview=new[] { JobAction.WriteCode,JobAction.ReviewCode,JobAction.WaitForReview,JobAction.Advise };
					foreach(Job job in listJobsSorted) {
						Def jobPriority=_listJobPriorities.FirstOrDefault(y => y.DefNum==job.Priority);
						gridDocumentation.ListGridRows.Add(
							new GridRow(
								new GridCell(jobPriority.ItemName+
									(!writeAdviseReview.Contains(job.OwnerAction) ? ""
										: ((job.ListJobReviews.Count==0) ? ""
											: ((job.ListJobReviews.Any(y => y.ReviewStatus!=JobReviewStatus.Done)) ? "\r\n(!)" : "\r\n(R)")))) {
									ColorBackG=jobPriority.ItemColor
								},
								new GridCell(job.JobVersion),
								new GridCell(job.ToString()) { ColorBackG=(job.ToString().ToLower().Contains(textSearch.Text.ToLower())&&!string.IsNullOrWhiteSpace(textSearch.Text) ? Color.LightYellow : Color.Empty) }) {
								Tag=job
							}
						);
					}
				}
			}
			gridDocumentation.EndUpdate();
			//RESELECT JOB
			if(selectedJobNum>0) {
				for(int i=0;i<gridDocumentation.ListGridRows.Count;i++) {
					if((gridDocumentation.ListGridRows[i].Tag is Job) && ((Job)gridDocumentation.ListGridRows[i].Tag).JobNum==selectedJobNum) {
						gridDocumentation.SetSelected(i,true);
						break;
					}
				}
			}
		}		

		///<summary></summary>
		private void FillGridTesting() {
			if(!tabControlNav.TabPages.Contains(tabTesting)) {
				return;
			}
			Userod userFilter=Security.CurUser;
			if(comboUser.SelectedIndex==1) {
				userFilter=new Userod() { UserName="Unassigned",UserNum=0 };
			}
			//If all user set userFilter to null in order to get all jobs
			if(comboUser.SelectedIndex==0) {
				userFilter=null;
			}
			else if(comboUser.SelectedIndex>1) {
				userFilter=_listUsers[comboUser.SelectedIndex-2];
			}
			long selectedJobNum=0;
			if(userControlJobEdit.GetJob()!=null) {
				selectedJobNum=userControlJobEdit.GetJob().JobNum;
			}
			//Completed jobs are not included in _listJobsAll by default
			List<Job> listJobs=Jobs.GetForTesting(textVersionText.Text,_listJobsAll.Select(x => x.JobNum).ToList());
			if(!listJobs.IsNullOrEmpty()) {
				Jobs.FillInMemoryLists(listJobs);
				_listJobsAll.AddRange(listJobs);
			}
			gridTesting.BeginUpdate();
			gridTesting.ListGridColumns.Clear();
			gridTesting.ListGridColumns.Add(new GridColumn("Priority",50,HorizontalAlignment.Center));
			gridTesting.ListGridColumns.Add(new GridColumn("Version",95,HorizontalAlignment.Center));
			gridTesting.ListGridColumns.Add(new GridColumn("Date Tested",70,HorizontalAlignment.Center));
			gridTesting.ListGridColumns.Add(new GridColumn("",150));
			gridTesting.ListGridRows.Clear();
			//Get a list of all jobs that should be tested.
			List<Job> listTestingJobs=_listJobsAll.FindAll(x => !x.Category.In(JobCategory.Query,JobCategory.Research,JobCategory.Conversion)
					&& x.PhaseCur.In(JobPhase.Complete,JobPhase.Documentation)
					&& (string.IsNullOrEmpty(textVersionText.Text) ? true : x.JobVersion.Contains(textVersionText.Text))
					&& (string.IsNullOrEmpty(textSearch.Text) ? true : x.ToString().ToLower().Contains(textSearch.Text.ToLower())))
				.OrderBy(x => Userods.GetUser(x.UserNumTester)?.UserName??"ZZZ")
				//This is the reverse order of the actual priority of different categories of jobs
				//Purposefully put in this order so they appear correctly in the list for testing.
				.ThenBy(x => x.Category==JobCategory.NeedNoApproval)
				.ThenBy(x => x.Category==JobCategory.SpecialProject)
				.ThenBy(x => x.Category==JobCategory.Query)
				.ThenBy(x => x.Category==JobCategory.Conversion)
				.ThenBy(x => x.Category==JobCategory.ProgramBridge)
				.ThenBy(x => x.Category==JobCategory.Research)
				.ThenBy(x => x.Category==JobCategory.HqRequest)
				.ThenBy(x => x.Category==JobCategory.Bug)
				.ThenBy(x => x.Category==JobCategory.InternalRequest)
				.ThenBy(x => x.Category==JobCategory.Feature)
				.ThenBy(x => x.Category==JobCategory.Enhancement)
				.ThenBy(x => _listJobPriorities.FirstOrDefault(y => y.DefNum==x.PriorityTesting)?.ItemOrder??1000)
				.ToList();
			//Make a dictionary separated by users.
			Dictionary<long,List<Job>> dictTestingJobsByUser=listTestingJobs.GroupBy(x => x.UserNumTester).ToDictionary(x => x.Key,x => x.ToList());
			//Fill the grid with the jobs.
			foreach(long userNum in dictTestingJobsByUser.Keys) {
				if(!checkShowAllUsers.Checked 
					&& userFilter!=null 
					&& userNum!=0
					&& userNum!=userFilter.UserNum) 
				{
					continue;
				}
				//Every user will have their own section.  Might hide other users later once we start getting busy with testing.
				Userod user=Userods.GetUser(userNum)??new Userod() { UserName="Unassigned",UserNum=0 };
				gridTesting.ListGridRows.Add(new GridRow("","","",user.UserName) { ColorBackG=_colorGridHeaderBack,Bold=true });
				foreach(Job job in dictTestingJobsByUser[userNum]) {
					if(job.DateTimeTested.Year>1880 && checkHideTested.Checked) {
						continue;
					}
					if(job.IsNotTested && checkHideNotTested.Checked) {
						continue;
					}
					Def jobPriority=_listJobPriorities.FirstOrDefault(y => y.DefNum==job.PriorityTesting)??new Def() { ItemName="",ItemColor=Color.Empty };
					gridTesting.ListGridRows.Add(
						new GridRow(
							new GridCell(jobPriority.ItemName) {
								ColorBackG=jobPriority.ItemColor
							},
							new GridCell(job.JobVersion),
							new GridCell(job.DateTimeTested.Year>1880 ? job.DateTimeTested.ToShortDateString() : ""),
							new GridCell(job.ToString()) 
							)
						{
							Tag=job
						}
					);
				}
			}
			gridTesting.EndUpdate();
			//RESELECT JOB
			if(selectedJobNum>0) {
				for(int i=0;i<gridTesting.ListGridRows.Count;i++) {
					if((gridTesting.ListGridRows[i].Tag is Job) && ((Job)gridTesting.ListGridRows[i].Tag).JobNum==selectedJobNum) {
						gridTesting.SetSelected(i,true);
						break;
					}
				}
			}
		}

		private void FillGridNeedsEngineer() {
			long selectedJobNum=0;
			int jobCount=0;
			//sort dictionary so actions will appear in same order
			List<int> listCategoriesSorted=new List<int> {
				(int)JobCategory.Bug,
				(int)JobCategory.Enhancement,
				(int)JobCategory.ProgramBridge,
				(int)JobCategory.Feature,
				(int)JobCategory.Query,
				(int)JobCategory.InternalRequest,
				(int)JobCategory.HqRequest,
				(int)JobCategory.Conversion,
				(int)JobCategory.Research,
				(int)JobCategory.NeedNoApproval,
			};
			//Sort jobs into category dictionary
			Dictionary<JobCategory,List<Job>> dictCategories=_listJobsAll.GroupBy(x => x.Category).ToDictionary(y => y.Key,y => y.ToList());
			#region Needs Engineer
			if(tabControlNav.TabPages.Contains(tabNeedsEngineer)) {
				if(userControlJobEdit.GetJob()!=null) {
					selectedJobNum=userControlJobEdit.GetJob().JobNum;
				}
				gridAvailableJobs.BeginUpdate();
				gridAvailableJobs.ListGridColumns.Clear();
				gridAvailableJobs.ListGridColumns.Add(new GridColumn("Priority",50) { TextAlign=HorizontalAlignment.Center });
				gridAvailableJobs.ListGridColumns.Add(new GridColumn("",245));
				gridAvailableJobs.ListGridRows.Clear();
				dictCategories=dictCategories.OrderBy(x => listCategoriesSorted.IndexOf((int)x.Key)).ToDictionary(x => x.Key,x => x.Value);
				foreach(KeyValuePair<JobCategory,List<Job>> kvp in dictCategories) {
					if(kvp.Key==JobCategory.Query) {
						continue;
					}
					List<Job> listJobsSorted=kvp.Value.OrderBy(x => _listJobPriorities.FirstOrDefault(y => y.DefNum==x.Priority).ItemOrder).ToList();
					listJobsSorted.RemoveAll(x => x.Priority==_listJobPriorities.FirstOrDefault(y => y.ItemValue.Contains("OnHold")).DefNum || x.OwnerAction!=JobAction.AssignEngineer);
					if(comboProposedVersionNeedsEngineer.SelectedIndex!=0) {//All is not selected, only select the jobs with the specified proposedversion
						listJobsSorted.RemoveAll(x => x.ProposedVersion!=comboProposedVersionNeedsEngineer.GetSelected<JobProposedVersion>());
					}
					if(listJobsSorted.Count==0) {
						continue;
					}
					gridAvailableJobs.ListGridRows.Add(new GridRow("",kvp.Key.ToString()) { ColorBackG=_colorGridHeaderBack,Bold=true });
					foreach(Job job in listJobsSorted) {
						Def jobPriority=_listJobPriorities.FirstOrDefault(y => y.DefNum==job.Priority);
						gridAvailableJobs.ListGridRows.Add(
							new GridRow(
								new GridCell(jobPriority.ItemName) { ColorBackG=jobPriority.ItemColor },
								new GridCell(job.ToString()) { ColorBackG=(job.ToString().ToLower().Contains(textSearch.Text.ToLower())&&!string.IsNullOrWhiteSpace(textSearch.Text) ? Color.LightYellow : Color.Empty) }
								) { Tag=job }
						);
					}
					jobCount+=listJobsSorted.Count;
				}
				gridAvailableJobs.EndUpdate();
				tabNeedsEngineer.Text="Needs Engineer ("+jobCount+")";
				//RESELECT JOB
				if(selectedJobNum>0) {
					for(int i=0;i<gridAvailableJobs.ListGridRows.Count;i++) {
						if((gridAvailableJobs.ListGridRows[i].Tag is Job) && ((Job)gridAvailableJobs.ListGridRows[i].Tag).JobNum==selectedJobNum) {
							gridAvailableJobs.SetSelected(i,true);
							break;
						}
					}
				}
			}
			#endregion
		}

		private void FillGridNeedsExpert() {
			long selectedJobNum=0;
			int jobCount=0;
			//sort dictionary so actions will appear in same order
			List<int> listCategoriesSorted=new List<int> {
				(int)JobCategory.Bug,
				(int)JobCategory.Enhancement,
				(int)JobCategory.ProgramBridge,
				(int)JobCategory.Feature,
				(int)JobCategory.Query,
				(int)JobCategory.InternalRequest,
				(int)JobCategory.HqRequest,
				(int)JobCategory.Conversion,
				(int)JobCategory.Research,
				(int)JobCategory.NeedNoApproval,
			};
			//Sort jobs into category dictionary
			Dictionary<JobCategory,List<Job>> dictCategories=_listJobsAll.GroupBy(x => x.Category).ToDictionary(y => y.Key,y => y.ToList());
			#region Needs Expert
			if(tabControlNav.TabPages.Contains(tabNeedsExpert)) {
				selectedJobNum=0;
				if(userControlJobEdit.GetJob()!=null) {
					selectedJobNum=userControlJobEdit.GetJob().JobNum;
				}
				gridAvailableJobsExpert.BeginUpdate();
				gridAvailableJobsExpert.ListGridColumns.Clear();
				gridAvailableJobsExpert.ListGridColumns.Add(new GridColumn("Priority",50,HorizontalAlignment.Center));
				gridAvailableJobsExpert.ListGridColumns.Add(new GridColumn("",245));
				gridAvailableJobsExpert.ListGridRows.Clear();
				dictCategories=dictCategories.OrderBy(x => listCategoriesSorted.IndexOf((int)x.Key)).ToDictionary(x => x.Key,x => x.Value);
				jobCount=0;
				foreach(KeyValuePair<JobCategory,List<Job>> kvp in dictCategories) {
					if(kvp.Key==JobCategory.Query) {
						continue;
					}
					List<Job> listJobsSorted=kvp.Value.OrderBy(x => _listJobPriorities.FirstOrDefault(y => y.DefNum==x.Priority).ItemOrder).ToList();
					listJobsSorted.RemoveAll(x => x.Priority==_listJobPriorities.FirstOrDefault(y => y.ItemValue.Contains("OnHold")).DefNum || x.OwnerAction!=JobAction.AssignExpert);
					if(comboProposedVersionNeedsExpert.SelectedIndex!=0) {//All is not selected, only select the jobs with the specified proposedversion
						listJobsSorted.RemoveAll(x => x.ProposedVersion!=comboProposedVersionNeedsExpert.GetSelected<JobProposedVersion>());
					}
					if(listJobsSorted.Count==0) {
						continue;
					}
					gridAvailableJobsExpert.ListGridRows.Add(new GridRow("",kvp.Key.ToString()) { ColorBackG=_colorGridHeaderBack,Bold=true });
					foreach(Job job in listJobsSorted) {
						Def jobPriority=_listJobPriorities.FirstOrDefault(y => y.DefNum==job.Priority);
						gridAvailableJobsExpert.ListGridRows.Add(
							new GridRow(
								new GridCell(jobPriority.ItemName) { ColorBackG=jobPriority.ItemColor },
								new GridCell(job.ToString()) { ColorBackG=(job.ToString().ToLower().Contains(textSearch.Text.ToLower())&&!string.IsNullOrWhiteSpace(textSearch.Text) ? Color.LightYellow : Color.Empty) }
								) { Tag=job }
							);
					}
					jobCount+=listJobsSorted.Count;
				}
				gridAvailableJobsExpert.EndUpdate();
				tabNeedsExpert.Text="Needs Expert ("+jobCount+")";
				//RESELECT JOB
				if(selectedJobNum>0) {
					for(int i=0;i<gridAvailableJobsExpert.ListGridRows.Count;i++) {
						if((gridAvailableJobsExpert.ListGridRows[i].Tag is Job) && ((Job)gridAvailableJobsExpert.ListGridRows[i].Tag).JobNum==selectedJobNum) {
							gridAvailableJobsExpert.SetSelected(i,true);
							break;
						}
					}
				}
			}
			#endregion
		}

		private void FillGridOnHold() {
			long selectedJobNum=0;
			int jobCount=0;
			//sort dictionary so actions will appear in same order
			List<int> listCategoriesSorted=new List<int> {
				(int)JobCategory.Bug,
				(int)JobCategory.Enhancement,
				(int)JobCategory.ProgramBridge,
				(int)JobCategory.Feature,
				(int)JobCategory.Query,
				(int)JobCategory.InternalRequest,
				(int)JobCategory.HqRequest,
				(int)JobCategory.Conversion,
				(int)JobCategory.Research,
				(int)JobCategory.NeedNoApproval,
			};
			//Sort jobs into category dictionary
			Dictionary<JobCategory,List<Job>> dictCategories=_listJobsAll.GroupBy(x => x.Category).ToDictionary(y => y.Key,y => y.ToList());
			#region On Hold
			if(tabControlNav.TabPages.Contains(tabOnHold)) {
				selectedJobNum=0;
				if(userControlJobEdit.GetJob()!=null) {
					selectedJobNum=userControlJobEdit.GetJob().JobNum;
				}
				gridJobsOnHold.BeginUpdate();
				gridJobsOnHold.ListGridColumns.Clear();
				gridJobsOnHold.ListGridColumns.Add(new GridColumn("Priority",50,HorizontalAlignment.Center));
				gridJobsOnHold.ListGridColumns.Add(new GridColumn("Owner",55,HorizontalAlignment.Center));// X for yes, - for unassigned
				gridJobsOnHold.ListGridColumns.Add(new GridColumn("",245));
				gridJobsOnHold.ListGridRows.Clear();
				dictCategories=dictCategories.OrderBy(x => listCategoriesSorted.IndexOf((int)x.Key)).ToDictionary(x => x.Key,x => x.Value);
				jobCount=0;
				foreach(KeyValuePair<JobCategory,List<Job>> kvp in dictCategories) {
					if(kvp.Key==JobCategory.Query) {
						continue;
					}
					List<Job> listJobsSorted=kvp.Value.OrderBy(x => _listJobPriorities.FirstOrDefault(y => y.DefNum==x.Priority).ItemOrder).ToList();
					listJobsSorted.RemoveAll(x => x.Priority!=_listJobPriorities.FirstOrDefault(y => y.ItemValue.Contains("OnHold")).DefNum || x.PhaseCur==JobPhase.Complete || x.PhaseCur==JobPhase.Documentation || x.PhaseCur==JobPhase.Cancelled);
					if(listJobsSorted.Count==0) {
						continue;
					}
					gridJobsOnHold.ListGridRows.Add(new GridRow("","",kvp.Key.ToString()) { ColorBackG=_colorGridHeaderBack,Bold=true });
					foreach(Job job in listJobsSorted) {
						Def jobPriority=_listJobPriorities.FirstOrDefault(y => y.DefNum==job.Priority);
						gridJobsOnHold.ListGridRows.Add(
						new GridRow(
							new GridCell(jobPriority.ItemName) { ColorBackG=jobPriority.ItemColor },
							new GridCell(job.OwnerNum==0 ? "-" : Userods.GetName(job.OwnerNum)),
							new GridCell(job.ToString()) { ColorBackG=(job.ToString().ToLower().Contains(textSearch.Text.ToLower())&&!string.IsNullOrWhiteSpace(textSearch.Text) ? Color.LightYellow : Color.Empty) }
							) { Tag=job }
						);
					}
					jobCount+=listJobsSorted.Count;
				}
				gridJobsOnHold.EndUpdate();
				tabOnHold.Text="On Hold ("+jobCount+")";
				//RESELECT JOB
				if(selectedJobNum>0) {
					for(int i=0;i<gridJobsOnHold.ListGridRows.Count;i++) {
						if((gridJobsOnHold.ListGridRows[i].Tag is Job) && ((Job)gridJobsOnHold.ListGridRows[i].Tag).JobNum==selectedJobNum) {
							gridJobsOnHold.SetSelected(i,true);
							break;
						}
					}
				}
			}
			#endregion
		}

		private void FillGridPatternReview() {
			long selectedJobNum=0;
			int jobCount=0;
			//sort dictionary so actions will appear in same order
			List<int> listCategoriesSorted=new List<int> {
				(int)JobCategory.Bug,
				(int)JobCategory.Enhancement,
				(int)JobCategory.ProgramBridge,
				(int)JobCategory.Feature,
				(int)JobCategory.Query,
				(int)JobCategory.InternalRequest,
				(int)JobCategory.HqRequest,
				(int)JobCategory.Conversion,
				(int)JobCategory.Research,
				(int)JobCategory.NeedNoApproval,
			};
			//Sort jobs into category dictionary
			Dictionary<JobCategory,List<Job>> dictCategories=_listJobsAll.GroupBy(x => x.Category).ToDictionary(y => y.Key,y => y.ToList());
			#region Pattern Review
			if(tabControlNav.TabPages.Contains(tabPatternReview)) {
				selectedJobNum=0;
				if(userControlJobEdit.GetJob()!=null) {
					selectedJobNum=userControlJobEdit.GetJob().JobNum;
				}
				gridPatternReview.ContextMenu=contextMenuPatternReview;
				gridPatternReview.BeginUpdate();
				gridPatternReview.ListGridColumns.Clear();
				gridPatternReview.ListGridColumns.Add(new GridColumn("DateC",65,HorizontalAlignment.Center));
				gridPatternReview.ListGridColumns.Add(new GridColumn("Owner",50,HorizontalAlignment.Center));
				gridPatternReview.ListGridColumns.Add(new GridColumn("Status",55,HorizontalAlignment.Center));
				gridPatternReview.ListGridColumns.Add(new GridColumn("",245));
				gridPatternReview.ListGridRows.Clear();
				dictCategories=dictCategories.OrderBy(x => listCategoriesSorted.IndexOf((int)x.Key)).ToDictionary(x => x.Key,x => x.Value);
				jobCount=0;
				foreach(KeyValuePair<JobCategory,List<Job>> kvp in dictCategories) {
					if(kvp.Key.In(JobCategory.Query,JobCategory.Conversion)) {
						continue;
					}
					List<Job> listJobsSorted=kvp.Value.OrderBy(x => _listJobPriorities.FirstOrDefault(y => y.DefNum==x.Priority).ItemOrder).ToList();
					listJobsSorted.RemoveAll(x => !x.PatternReviewStatus.In(JobPatternReviewStatus.AwaitingApproval,JobPatternReviewStatus.Tentative) 
						|| x.PatternReviewProject!=JobPatternReviewProject.OD 
						|| !x.PhaseCur.In(JobPhase.Complete,JobPhase.Documentation,JobPhase.Development) 
						|| x.DateTimeImplemented < dateExcludeCompleteBefore.Value);
					if(listJobsSorted.Count==0) {
						continue;
					}
					gridPatternReview.ListGridRows.Add(new GridRow("","","",kvp.Key.ToString()) { ColorBackG=_colorGridHeaderBack,Bold=true });
					foreach(Job job in listJobsSorted) {
						Def jobPriority=_listJobPriorities.FirstOrDefault(y => y.DefNum==job.Priority);
						gridPatternReview.ListGridRows.Add(
							new GridRow(
								new GridCell(job.DateTimeImplemented==DateTime.MinValue?"":job.DateTimeImplemented.ToShortDateString()),
								new GridCell(Userods.GetName(job.OwnerNum)),
								new GridCell(job.PatternReviewStatus.ToString()),
								new GridCell(job.ToString()) { ColorBackG=(job.ToString().ToLower().Contains(textSearch.Text.ToLower())&&!string.IsNullOrWhiteSpace(textSearch.Text) ? Color.LightYellow : Color.Empty) }
								) { Tag=job }
							);
					}
					jobCount+=listJobsSorted.Count;
				}
				gridPatternReview.EndUpdate();
				//RESELECT JOB
				if(selectedJobNum>0) {
					for(int i=0;i<gridPatternReview.ListGridRows.Count;i++) {
						if((gridPatternReview.ListGridRows[i].Tag is Job) && ((Job)gridPatternReview.ListGridRows[i].Tag).JobNum==selectedJobNum) {
							gridPatternReview.SetSelected(i,true);
							break;
						}
					}
				}
			}
			#endregion
		}
		
		///<summary></summary>
		private void FillGridNotify() {
			if(!tabControlNav.TabPages.Contains(tabNotify)) {
				return;
			}
			_dicRowNotes.Clear();
			Userod userFilter=Security.CurUser;
			if(comboUser.SelectedIndex==1) {
				userFilter=new Userod() { UserName="Unassigned",UserNum=0 };
			}
			else if(comboUser.SelectedIndex>1) {
				userFilter=_listUsers[comboUser.SelectedIndex-2];
			}
			gridNotify.Title="Action Items";
			tabAction.Text="Needs Action";
			long selectedJobNum=0;
			if(userControlJobEdit.GetJob()!=null) {
				selectedJobNum=userControlJobEdit.GetJob().JobNum;
			}
			gridNotify.BeginUpdate();
			gridNotify.ListGridColumns.Clear();
			gridNotify.ListGridColumns.Add(new GridColumn("Owner",55,HorizontalAlignment.Center));// X for yes, - for unassigned
			gridNotify.ListGridColumns.Add(new GridColumn("Version",55,HorizontalAlignment.Center));// X for yes, - for unassigned
			gridNotify.ListGridColumns.Add(new GridColumn("",245));
			gridNotify.ListGridRows.Clear();
			//Sort jobs into action dictionary
			Dictionary<JobAction,List<Job>> dictActions=new Dictionary<JobAction,List<Job>>();
			foreach(Job job in _listJobsAll) {
				JobAction action;
				if(userFilter==null) {
					action=job.OwnerAction;
				}
				else {
					action=job.ActionForUser(userFilter);
				}
				if(!action.In(JobAction.ContactCustomer,JobAction.ContactCustomerPreDoc)) {
					continue;
				}
				if(job.Category==JobCategory.Query) {
					continue;
				}
				if(!dictActions.ContainsKey(action)) {
					dictActions[action]=new List<Job>();
				}
				dictActions[action].Add(job);
			}
			//sort dictionary so actions will appear in same order
			dictActions=dictActions.OrderBy(x => (int)x.Key).ToDictionary(x => x.Key,x => x.Value);
			foreach(KeyValuePair<JobAction,List<Job>> kvp in dictActions) {
				if(kvp.Key==JobAction.Undefined || kvp.Key==JobAction.UnknownJobPhase || kvp.Key==JobAction.None) {
					//Undefined occurs when there is no action to take. 
					//UnknownJobPhase occurs when there is something wrong with the programming.
					continue;
				}
				List<Job> listJobsSorted=kvp.Value//filter
					.OrderBy(x => userFilter==null || x.OwnerNum!=userFilter.UserNum)//sort
					.ThenBy(x => x.OwnerNum!=0)
					.ThenBy(x => _listJobPriorities.FirstOrDefault(y => y.DefNum==x.Priority).ItemOrder).ToList();
				if(listJobsSorted.Count==0) {
					continue;
				}
				//Remove jobs with no FR for the ContactCustomer permission because documentation users should never have customer contact permission as well.
				listJobsSorted.RemoveAll(x => kvp.Key.In(JobAction.ContactCustomer,JobAction.ContactCustomerPreDoc)
								&& !x.ListJobLinks.Any(y => y.LinkType==JobLinkType.Request));
				if(listJobsSorted.Count>0) {
					gridNotify.ListGridRows.Add(new GridRow("","",kvp.Key.ToString()) { ColorBackG=_colorGridHeaderBack,Bold=true });
					JobAction[] writeAdviseReview=new[] { JobAction.WriteCode,JobAction.ReviewCode,JobAction.WaitForReview,JobAction.Advise };
					foreach(Job job in listJobsSorted) {
						Def jobPriority=_listJobPriorities.FirstOrDefault(y => y.DefNum==job.Priority);
						gridNotify.ListGridRows.Add(
							new GridRow(
								new GridCell(job.OwnerNum==0 ? "-" : Userods.GetName(job.OwnerNum)),
								new GridCell(job.JobVersion),
								new GridCell(job.ToString()) { ColorBackG=(job.ToString().ToLower().Contains(textSearch.Text.ToLower())&&!string.IsNullOrWhiteSpace(textSearch.Text) ? Color.LightYellow : Color.Empty) }) {
								Tag=job
							}
						);
					}
				}
			}
			gridNotify.EndUpdate();
			//RESELECT JOB
			if(selectedJobNum>0) {
				for(int i=0;i<gridNotify.ListGridRows.Count;i++) {
					if((gridNotify.ListGridRows[i].Tag is Job) && ((Job)gridNotify.ListGridRows[i].Tag).JobNum==selectedJobNum) {
						gridNotify.SetSelected(i,true);
						break;
					}
				}
			}
		}

		private void FillGridSubscribed() {
			if(!tabControlNav.TabPages.Contains(tabSubscribed)) {
				return;
			}
			long selectedJobNum=0;
			int jobCount=0;
			//sort dictionary so actions will appear in same order
			List<int> listCategoriesSorted=new List<int> {
				(int)JobCategory.Bug,
				(int)JobCategory.Enhancement,
				(int)JobCategory.ProgramBridge,
				(int)JobCategory.Feature,
				(int)JobCategory.Query,
				(int)JobCategory.InternalRequest,
				(int)JobCategory.HqRequest,
				(int)JobCategory.Conversion,
				(int)JobCategory.Research,
				(int)JobCategory.NeedNoApproval,
			};
			//Sort jobs into category dictionary
			Dictionary<JobCategory,List<Job>> dictCategories=_listJobsAll.GroupBy(x => x.Category).ToDictionary(y => y.Key,y => y.ToList());
			Userod userFilter=Security.CurUser;
			if(comboUser.SelectedIndex==1) {
				userFilter=new Userod() { UserName="Unassigned",UserNum=0 };
			}
			else if(comboUser.SelectedIndex>1) {
				userFilter=_listUsers[comboUser.SelectedIndex-2];
			}
			if(userControlJobEdit.GetJob()!=null) {
				selectedJobNum=userControlJobEdit.GetJob().JobNum;
			}
			gridSubscribedJobs.BeginUpdate();
			gridSubscribedJobs.ListGridColumns.Clear();
			gridSubscribedJobs.ListGridColumns.Add(new GridColumn("Priority",50,HorizontalAlignment.Center));
			gridSubscribedJobs.ListGridColumns.Add(new GridColumn("Expert",55,HorizontalAlignment.Center));
			gridSubscribedJobs.ListGridColumns.Add(new GridColumn("Engineer",55,HorizontalAlignment.Center));
			gridSubscribedJobs.ListGridColumns.Add(new GridColumn("Phase",85,HorizontalAlignment.Center));
			gridSubscribedJobs.ListGridColumns.Add(new GridColumn("",0));
			gridSubscribedJobs.ListGridRows.Clear();
			dictCategories=dictCategories.OrderBy(x => listCategoriesSorted.IndexOf((int)x.Key)).ToDictionary(x => x.Key,x => x.Value);
			foreach(KeyValuePair<JobCategory,List<Job>> kvp in dictCategories) {
				List<Job> listJobsSorted=kvp.Value.OrderBy(x => _listJobPriorities.FirstOrDefault(y => y.DefNum==x.Priority).ItemOrder).ToList();
				if(!checkSubscribedIncludeOnHold.Checked) {
					listJobsSorted.RemoveAll(x => x.Priority==_listJobPriorities.FirstOrDefault(y => y.ItemValue.Contains("OnHold")).DefNum);
				}
				listJobsSorted.RemoveAll(x => !x.ListJobLinks.Exists(y => y.LinkType==JobLinkType.Subscriber && y.FKey==userFilter.UserNum));
				if(listJobsSorted.Count==0) {
					continue;
				}
				gridSubscribedJobs.ListGridRows.Add(new GridRow("","","","",kvp.Key.ToString()) { ColorBackG=_colorGridHeaderBack,Bold=true });
				foreach(Job job in listJobsSorted) {
					Def jobPriority=_listJobPriorities.FirstOrDefault(y => y.DefNum==job.Priority);
					string note=JobNotifications.GetNoteForChanges(job.ListJobNotifications.FirstOrDefault(x => x.UserNum==Security.CurUser.UserNum)?.Changes??JobNotificationChanges.None);
					gridSubscribedJobs.ListGridRows.Add(
						new GridRow(
							new GridCell(jobPriority.ItemName),
							new GridCell(job.UserNumExpert==0?"-":Userods.GetName(job.UserNumExpert)),
							new GridCell(job.UserNumEngineer==0?"-":Userods.GetName(job.UserNumEngineer)),
							new GridCell(job.PhaseCur.ToString()),
							new GridCell(job.ToString())
							) {
							Tag=job,
							Note=note,
							ColorBackG=string.IsNullOrEmpty(note)?Color.White:Color.LightSalmon
						}
					);
				}
				jobCount+=listJobsSorted.Count;
			}
			gridSubscribedJobs.EndUpdate();
			//RESELECT JOB
			if(selectedJobNum>0) {
				for(int i=0;i<gridSubscribedJobs.ListGridRows.Count;i++) {
					if((gridSubscribedJobs.ListGridRows[i].Tag is Job) && ((Job)gridSubscribedJobs.ListGridRows[i].Tag).JobNum==selectedJobNum) {
						gridSubscribedJobs.SetSelected(i,true);
						break;
					}
				}
			}
		}

		private void FillGridQueries() {
			if(!tabControlNav.TabPages.Contains(tabQuery)) {
				return;
			}
			Userod userFilter=Security.CurUser;
			if(comboUser.SelectedIndex==1) {
				userFilter=new Userod() { UserName="Unassigned",UserNum=0 };
			}
			//If all user set userFilter to null in order to get all jobs
			if(comboUser.SelectedIndex==0) {
				userFilter=null;
			}
			else if(comboUser.SelectedIndex>1) {
				userFilter=_listUsers[comboUser.SelectedIndex-2];
			}
			gridQueries.ContextMenu=contextMenuQueries;
			gridQueries.Title="Query Jobs";
			long selectedJobNum=0;
			if(userControlQueryEdit.GetJob()!=null) {
				selectedJobNum=userControlQueryEdit.GetJob().JobNum;
			}
			gridQueries.BeginUpdate();
			gridQueries.ListGridColumns.Clear();
			gridQueries.ListGridColumns.Add(new GridColumn("Sched Date",70,HorizontalAlignment.Center));//Oldest at the top
			gridQueries.ListGridColumns.Add(new GridColumn("Priority",50,HorizontalAlignment.Center));
			gridQueries.ListGridColumns.Add(new GridColumn("Owner",55,HorizontalAlignment.Center));// X for yes, - for unassigned
			gridQueries.ListGridColumns.Add(new GridColumn("",245));
			gridQueries.ListGridRows.Clear();
			//Sort jobs into action dictionary
			List<Job> listJobsSorted=_listJobsAll.Where(x => x.Category==JobCategory.Query).ToList();
			listJobsSorted=listJobsSorted.OrderBy(x => x.AckDateTime).ThenBy(x => _listJobPriorities.FirstOrDefault(y => y.DefNum==x.Priority).ItemOrder).ToList();
			Dictionary<JobPhase,List<Job>> dictPhases=new Dictionary<JobPhase,List<Job>>();
			foreach(Job job in listJobsSorted) {
				if(userFilter!=null && userFilter.UserNum!=job.UserNumEngineer) {
					continue;
				}
				if((!checkShowQueryComplete.Checked && job.PhaseCur==JobPhase.Complete) 
					|| (checkShowQueryComplete.Checked && job.PhaseCur==JobPhase.Complete
					&& !job.ListJobLogs.Exists(y => y.Description.Contains("Job implemented") && y.DateTimeEntry.Between(dateFrom.Value,dateTo.Value)))) 
				{
					continue;
				}
				if((!checkShowQueryCancelled.Checked && job.PhaseCur==JobPhase.Cancelled)
					|| (checkShowQueryCancelled.Checked && job.PhaseCur==JobPhase.Cancelled
					&& !job.ListJobLogs.Exists(y => y.Description.Contains("Job Cancelled") && y.DateTimeEntry.Between(dateFrom.Value,dateTo.Value)))) 
				{
					continue;
				}
				JobPhase phase=job.PhaseCur;
				if(!dictPhases.ContainsKey(phase)) {
					dictPhases[phase]=new List<Job>();
				}
				dictPhases[phase].Add(job);
			}
			//sort dictionary so actions will appear in same order
			//This is in reverse order in the code so it is correct in the UI
			dictPhases=dictPhases.OrderBy(x => x.Key==JobPhase.Cancelled)
					.ThenBy(x => x.Key==JobPhase.Complete)
					.ThenBy(x => x.Key==JobPhase.Development)
					.ThenBy(x => x.Key==JobPhase.Quote)
					.ThenBy(x => x.Key==JobPhase.Definition)
					.ThenBy(x => x.Key==JobPhase.Concept).ToDictionary(x => x.Key,x => x.Value);
			foreach(KeyValuePair<JobPhase,List<Job>> kvp in dictPhases) {
				if(listJobsSorted.Count==0) {
					continue;
				}
				gridQueries.ListGridRows.Add(new GridRow("","","",kvp.Key.ToString()) { ColorBackG=_colorGridHeaderBack,Bold=true });
				foreach(Job job in kvp.Value) {
					Color backColor=Color.White;
					if(selectedJobNum>0 
						&& job.ListJobQuotes.FirstOrDefault().PatNum==userControlQueryEdit.GetJob().ListJobQuotes.FirstOrDefault().PatNum) 
					{
						backColor=Color.LightBlue;
					}
					Def jobPriority=_listJobPriorities.FirstOrDefault(y => y.DefNum==job.Priority);
					gridQueries.ListGridRows.Add(
					new GridRow(
						new GridCell(job.AckDateTime.Year>1880?job.AckDateTime.ToShortDateString():"N/A"),
						new GridCell(jobPriority.ItemName) {
							ColorBackG=jobPriority.ItemColor,
							ColorText=(job.Priority==_listJobPriorities.FirstOrDefault(y => y.ItemValue.Contains("Urgent")).DefNum) ? Color.White : Color.Black,
						},
						new GridCell(job.UserNumEngineer==0 ? "-" : Userods.GetName(job.UserNumEngineer)),
						new GridCell(job.ToString()) { ColorBackG=(job.ToString().ToLower().Contains(textSearch.Text.ToLower())&&!string.IsNullOrWhiteSpace(textSearch.Text) ? Color.LightYellow : Color.Empty) }
						) {
						Tag=job,
						ColorBackG=backColor
					}
					);
				}
			}
			gridQueries.EndUpdate();
			//RESELECT JOB
			if(selectedJobNum>0 && selectedJobNum==userControlQueryEdit.GetJob().JobNum) {
				for(int i=0;i<gridQueries.ListGridRows.Count;i++) {
					if((gridQueries.ListGridRows[i].Tag is Job) && ((Job)gridQueries.ListGridRows[i].Tag).JobNum==selectedJobNum) {
						gridQueries.SetSelected(i,true);
						break;
					}
				}
			}
		}

		#region Tree Control and Filtering

		///<summary>listJobsAll must already be updated.</summary>
		private void FilterAndFill() {
			List<string> listFilters=textSearch.Text.ToLower().Split(' ').ToList();
			_listJobsFiltered=_listJobsAll.Select(x => x.Copy()).ToList();
			if(GroupJobsBy.MyHierarchy==(GroupJobsBy)comboGroup.SelectedIndex) {
				_listJobsFiltered.RemoveAll(x => !Jobs.GetUserNums(x).Contains(Security.CurUser.UserNum));
			}
			_listJobNumsHighlight=new List<long>();
			if(!checkIncludeCustContact.Checked) {
				_listJobsFiltered.RemoveAll(x => x.PhaseCur.In(JobPhase.Complete) && x.OwnerAction==JobAction.ContactCustomer && x.ListJobLinks.Count(y => y.LinkType==JobLinkType.Request)!=0);
			}
			if(comboCategorySearch.SelectedIndex>0) {
				JobCategory cat=(JobCategory)(comboCategorySearch.SelectedIndex-1);
				_listJobsFiltered.RemoveAll(x => x.Category!=cat);
			}
			if(listFilters.Count>0) {
				List<Job> matches=_listJobsFiltered.ToList();
				foreach(string filter in listFilters) {
					matches=matches.FindAll(x => x.Title.ToLower().Contains(filter)||x.JobNum.ToString().Contains(filter));
					_listJobNumsHighlight=matches.Select(x => x.JobNum).ToList();
					//if(!checkHighlight.Checked) {//not highlight only, actually filter results.
					_listJobsFiltered=matches.Select(x => x.Copy()).ToList();
				}
			}//end if filtering results
			if(new[] { GroupJobsBy.Hierarchy,GroupJobsBy.MyHierarchy }.Contains((GroupJobsBy)comboGroup.SelectedIndex)) {//find parent if we are in heirarchy view
				List<Job> parentJobs;
				do {//This loop finds the parents of orphaned nodes so that when searching you can see results in context.
					long[] jobs, parents;
					jobs=_listJobsFiltered.Select(x => x.JobNum).ToArray();
					parents=_listJobsFiltered.Select(x => x.ParentNum).Distinct().ToArray();
					parentJobs=_listJobsAll.FindAll(x => !jobs.Contains(x.JobNum) && parents.Contains(x.JobNum));
					_listJobsFiltered.AddRange(parentJobs);
				} while(parentJobs.Count>0);
			}//end heirarchy do/while
			FillTree();
		}
		
		private void FillTree() {
			treeJobs.BeginUpdate();
			treeJobs.Nodes.Clear();
			switch((GroupJobsBy)comboGroup.SelectedIndex) {
				case GroupJobsBy.None:
					foreach(Job job in _listJobsFiltered) {//Add top level nodes.
						treeJobs.Nodes.Add(new TreeNode(job.ToString()) {
							Tag=job,
							BackColor=(_listJobNumsHighlight.Contains(job.JobNum)? Color.FromArgb(255,255,230) : Color.White)
						});
					}
					break;
				case GroupJobsBy.MyHierarchy:
				case GroupJobsBy.Hierarchy:
					foreach(Job job in _listJobsFiltered.Where(x=>x.ParentNum==0)) {//Add top level nodes.
						TreeNode node=GetNodeHierarchyFiltered(job);//get child nodes for each top level node.
						treeJobs.Nodes.Add(node);
					}
					break;
				case GroupJobsBy.Status:
					foreach(JobPhase status in Enum.GetValues(typeof(JobPhase))) {//Add top level nodes.
						TreeNode node=new TreeNode(status.ToString()) { Tag=status };//get child nodes for each top level node.
						foreach(Job job in _listJobsFiltered.Where(x=>x.PhaseCur==status)) {
							TreeNode child=new TreeNode(job.ToString()) { Tag=job };
							if(_listJobNumsHighlight.Contains(job.JobNum)) {
								child.BackColor=Color.FromArgb(255,255,230);
							}
							node.Nodes.Add(child);
						}
						treeJobs.Nodes.Add(node);
					}
					break;
				//case GroupJobsBy.MyJobs:
				case GroupJobsBy.User:
					List<Userod> listUsers;
					//if(UserFilter!=null) {
					//	listUsers=new List<Userod>() {
					//		UserFilter
					//	};
					//}
					//else{
					List<long> userNums=_listJobPermissionsAll.Select(x=>x.UserNum).Distinct().ToList();//show users with job permissions
					userNums=userNums.Union(_listJobsFiltered.SelectMany(x=>Jobs.GetUserNums(x,true))).ToList();//show users with jobs
					listUsers=Userods.GetWhere(x=>userNums.Contains(x.UserNum)).OrderBy(x=>x.UserName).ToList();
					listUsers.Add(new Userod() {UserName="Un-Assigned"});
					//}
					foreach(Userod user in listUsers){//Userods.ListShallow.FindAll(z=>_listJobsFiltered.SelectMany(x => new[] { x.Expert,x.Owner }.Union(_listJobLinksUsers.Select(y => y.FKey))).Distinct().Contains(z.UserNum)).OrderBy(x=>x.UserName)) {
						TreeNode node=new TreeNode(user.UserName) {Tag=user};
						TreeNode nodeChild=null;
						nodeChild=CreateNodeByStatus("Expert",_listJobsFiltered.FindAll(x=>x.UserNumExpert==user.UserNum));
						if(nodeChild!=null){
							node.Nodes.Add(nodeChild);
						}
						nodeChild=CreateNodeByStatus("Engineer",_listJobsFiltered.FindAll(x=>x.UserNumEngineer==user.UserNum));
						if(nodeChild!=null){
							node.Nodes.Add(nodeChild);
						}
						nodeChild=CreateNodeByStatus("Subscribed",_listJobsFiltered.FindAll(x => x.ListJobLinks.Any(y => y.LinkType==JobLinkType.Subscriber && y.FKey==user.UserNum)));
						if(nodeChild!=null){
							node.Nodes.Add(nodeChild);
						}
						nodeChild=CreateNodeByStatus("Reviews",_listJobsFiltered.FindAll(x=>x.ListJobReviews.Any(y=>y.ReviewerNum==user.UserNum)));
						if(nodeChild!=null){
							node.Nodes.Add(nodeChild);
						}
						if(node.Nodes.Count>0) {
							treeJobs.Nodes.Add(node);
						}
					}
					break;
				case GroupJobsBy.Owner:
					List<long> expOwnNums;
					expOwnNums=_listJobsFiltered.Select(x => x.OwnerNum).ToList();
					listUsers=Userods.GetWhere(x => expOwnNums.Contains(x.UserNum)).OrderBy(x => x.UserName).ToList();
					listUsers.Add(new Userod() {UserName="Unassigned"});
					foreach(Userod user in listUsers) {//Add top level nodes.
						TreeNode node=new TreeNode(user.UserName) { Tag=user };//get child nodes for each top level node.
						node=CreateNodeByStatus(user.UserName,_listJobsFiltered.Where(x=>user.UserNum==x.OwnerNum).ToList());
						if(node!=null) {
							node.Tag=user;
							treeJobs.Nodes.Add(node);
						}
					}
					break;
			}
			treeJobs.EndUpdate();
			if(checkCollapse.Checked) {
				treeJobs.CollapseAll();
			}
			else {
				treeJobs.ExpandAll();
			}
		}

		///<summary>Returns a single node with the given name, and adds all jobs to the node with a status node in between. Returns null if no jobs in list.</summary>
		private TreeNode CreateNodeByStatus(string NodeName,List<Job> listJobs) {
			if(listJobs==null || listJobs.Count==0) {
				return null;
			}
			TreeNode node=new TreeNode(NodeName);
			foreach(JobPhase status in Enum.GetValues(typeof(JobPhase)).Cast<JobPhase>().ToList()) {
				TreeNode nodeStatus=new TreeNode(status.ToString());
				listJobs.FindAll(x=>x.PhaseCur==status).ForEach(x=>nodeStatus.Nodes.Add(new TreeNode(x.ToString()) {
					Tag=x,
					BackColor=(_listJobNumsHighlight.Contains(x.JobNum)? Color.FromArgb(255,255,230) : Color.White)
				}));
				if(nodeStatus.Nodes==null || nodeStatus.Nodes.Count==0) {
					continue;
				}
				node.Nodes.Add(nodeStatus);
			}
			if(node.Nodes==null || node.Nodes.Count==0) {
				return null;
			}
			return node;
		}

		///<summary></summary>
		private TreeNode GetNodeHierarchyFiltered(Job job) {
			TreeNode[] children=_listJobsFiltered.FindAll(x => x.ParentNum==job.JobNum).Select(GetNodeHierarchyFiltered).ToArray();//can be enhanced by removing matches from the search set.
			TreeNode node=new TreeNode(job.ToString()) { Tag=job };
			if(children.Length>0) {
				node.Nodes.AddRange(children);
			}
			if(_listJobNumsHighlight.Contains(job.JobNum)) {
				node.BackColor=Color.FromArgb(255,255,230);
			}
			return node;
		}

		///<summary>Check for heirarchical loops when moving a child job to a parent job. Returns true if loop is found. Example A>B>C>A would be a loop.</summary>
		private bool IsJobLoop(Job jobChild,long jobNumParent) {
			List<long> lineage=new List<long>(){jobChild.JobNum};
			Job jobCur=jobChild.Copy();
			jobCur.ParentNum=jobNumParent;
			while(jobCur.ParentNum!=0){
				if(lineage.Contains(jobCur.ParentNum)) {
					MessageBox.Show(this,"Invalid hierarchy detected. Moving the job there would create an infinite loop.");
					return true;//loop found
				}
				Job jobNext=_listJobsAll.FirstOrDefault(x=>x.JobNum==jobCur.ParentNum);
				if(jobNext==null) {
					MessageBox.Show(this,"Invalid hierarchy detected. Cannot find job "+jobCur.ParentNum);
					return true;
				}
				jobCur=jobNext;
				lineage.Add(jobCur.JobNum);
			} 
			return false;//no loop detected
		}

		private void treeJobs_NodeMouseClick(object sender,TreeNodeMouseClickEventArgs e) {
			if(e.Node!=null && (e.Node.Tag is Job)) {
				LoadJob((Job)e.Node.Tag,false);
			}
		}

		private void treeJobs_ItemDrag(object sender,ItemDragEventArgs e) {
			treeJobs.SelectedNode=(TreeNode)e.Item;
			DoDragDrop(e.Item,DragDropEffects.Move);
		}

		private void treeJobs_DragEnter(object sender,DragEventArgs e) {
			e.Effect=DragDropEffects.Move;
		}

		private void treeJobs_DragDrop(object sender,DragEventArgs e) {
			if(grayNode!=null) {
				grayNode.BackColor=Color.White;
			}
			if(comboGroup.SelectedIndex!=(int)GroupJobsBy.Hierarchy) {
				return;//drag and drop only applies to heirarchy view.
			}
			if(userControlJobEdit.IsChanged) {
				MessageBox.Show("You must save changes to current job before making drag and drop changes.");
				return;
			}
			if(!e.Data.GetDataPresent("System.Windows.Forms.TreeNode",false)) { 
				return; 
			}
			Point pt=((TreeView)sender).PointToClient(new Point(e.X,e.Y));
			TreeNode destinationNode=((TreeView)sender).GetNodeAt(pt);
			TreeNode sourceNode=(TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
			if(!(sourceNode.Tag is Job)) {//only allow move is source node was a job.
				return;//might have to set some additional variable instead of just returning.
			}
			Job j1=(Job)sourceNode.Tag;
			if(!_isOverride
				&& j1.UserNumEngineer!=Security.CurUser.UserNum
				&& j1.UserNumExpert!=Security.CurUser.UserNum
				&& !JobPermissions.IsAuthorized(JobPerm.Approval,true)
				&& !JobPermissions.IsAuthorized(JobPerm.FeatureManager)) 
			{
				return;//only expert, engineer, Approver, FeatureManager, or override can drag and drop.
			}
			if(!TryMoveJobtoJob(j1,destinationNode)) {
				return;
			}
			if(sourceNode.Parent==null) {
				treeJobs.Nodes.Remove(sourceNode);
			}
			else {
				sourceNode.Parent.Nodes.Remove(sourceNode);
			}
			if(destinationNode!=null) {
				destinationNode.Nodes.Add(sourceNode);
			}
			else {
				treeJobs.Nodes.Add(sourceNode);
			}
			//Can be improved, this updates in memory list.
			Job temp=_listJobsAll.FirstOrDefault(x => x.JobNum==j1.JobNum);
			if(temp!=null) {//should never be null
				temp.ParentNum=j1.ParentNum; //update in memory list.
				temp.UserNumEngineer=j1.UserNumEngineer; //update in memory list.
				temp.UserNumExpert=j1.UserNumExpert; //update in memory list.
			}
			FillActiveTabGrid();//this is annoying and can be improved, but reflects the proper changes. tree will expand or collapse based on collapse all check.
		}

		private bool TryMoveJobtoJob(Job j1,TreeNode destinationNode) {
			if(destinationNode==null) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Move selected job to top level?")) {
					return false;
				}
				j1.ParentNum=0;
			}
			else if(destinationNode.Tag is Job) {
				Job j2=(Job)destinationNode.Tag;
				if(j1.JobNum==j2.JobNum) {
					return false;
				}
				if(IsJobLoop(j1,j2.JobNum)) {
					return false;
				}
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Move selected job?")) {
					return false;
				}
				j1.ParentNum=j2.JobNum;
			}
			else {
				return false;//no valid target
			}
			Jobs.Update(j1);
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,j1.JobNum);
			return true;
		}

		private TreeNode grayNode=null;//only used in treeJobs_DragOver to reduce flickering.

		private void treeJobs_DragOver(object sender,DragEventArgs e) {
			Point p=treeJobs.PointToClient(new Point(e.X,e.Y));
			TreeNode node=treeJobs.GetNodeAt(p.X,p.Y);
			if(grayNode!=null && grayNode!=node) {
				grayNode.BackColor=Color.White;
				grayNode=null;
			}
			if(node!=null && node.BackColor!=Color.LightGray) {
				node.BackColor=Color.LightGray;
				grayNode=node;
			}
			if(p.Y<25) {
				MiscUtils.SendMessage(treeJobs.Handle,277,0,0);//Scroll Up
			}
			else if(p.Y>treeJobs.Height-25) {
				MiscUtils.SendMessage(treeJobs.Handle,277,1,0);//Scroll down.
			}
		}

		private void comboGroup_SelectionChangeCommitted(object sender,EventArgs e) {
			checkCollapse.Checked=true;
			FillActiveTabGrid();
		}

		private void comboCategorySearch_SelectedIndexChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void checkCollapse_CheckedChanged(object sender,EventArgs e) {
			if(checkCollapse.Checked) {
				treeJobs.CollapseAll();
			}
			else {
				treeJobs.ExpandAll();
			}
		}

		private void checkComplete_CheckedChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void checkCancelled_CheckedChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void checkIncludeCustContact_CheckedChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		#endregion

		#region Job Quick Search

		private void FillGridSearch() {
			if(!tabControlNav.TabPages.Contains(tabSearch)) {
				return;
			}
			Job selectedJob=userControlJobEdit.GetJob();
			long selectedJobNum=0;
			if(selectedJob!=null) {
				selectedJobNum=selectedJob.JobNum;
			}
			gridSearch.BeginUpdate();
			gridSearch.ListGridColumns.Clear();
			gridSearch.ListGridColumns.Add(new GridColumn("Priority",50,HorizontalAlignment.Center));
			gridSearch.ListGridColumns.Add(new GridColumn("Owner",55,HorizontalAlignment.Center));// X for yes, - for unassigned
			gridSearch.ListGridColumns.Add(new GridColumn("",245));
			gridSearch.ListGridRows.Clear();
			//Sort jobs into action dictionary
			List<Job> listJobsSorted=_listJobsAll.Where(x => x.ToString().ToLower().Contains(textSearch.Text.ToLower())).ToList();
			if(!checkContactSearch.Checked) {
				listJobsSorted.RemoveAll(x => x.PhaseCur.In(JobPhase.Complete) && x.OwnerAction==JobAction.ContactCustomer && x.ListJobLinks.Count(y => y.LinkType==JobLinkType.Request)!=0);
			}
			if(comboCatSearch.SelectedIndex!=0) {//All is not selected
				listJobsSorted=listJobsSorted.Where(x => x.Category==comboCatSearch.GetSelected<JobCategory>()).ToList();
			}
			if(comboProposedVersionSearch.SelectedIndex!=0) {//All is not selected
				listJobsSorted=listJobsSorted.Where(x => x.ProposedVersion==comboProposedVersionSearch.GetSelected<JobProposedVersion>()).ToList();
			}
			if(comboPrioritySearch.SelectedIndex!=0) {//All is not selected
				listJobsSorted=listJobsSorted.Where(x => x.Priority==comboPrioritySearch.GetSelected<Def>().DefNum).ToList();
			}
			if(!String.IsNullOrEmpty(textUserSearch.Text)) {
				listJobsSorted=listJobsSorted.Where(x => Userods.GetName(x.OwnerNum).ToLower().Contains(textUserSearch.Text.ToLower())).ToList();
			}
			listJobsSorted=listJobsSorted.OrderBy(x => x.OwnerNum!=0)
					//This is the reverse order of the actual priority of different categories of jobs
					//Purposefully put in this order so they appear correctly in the list.
					.ThenBy(x => x.Category==JobCategory.NeedNoApproval)
					.ThenBy(x => x.Category==JobCategory.Research)
					.ThenBy(x => x.Category==JobCategory.Conversion)
					.ThenBy(x => x.Category==JobCategory.HqRequest)
					.ThenBy(x => x.Category==JobCategory.InternalRequest)
					.ThenBy(x => x.Category==JobCategory.Feature)
					.ThenBy(x => x.Category==JobCategory.Query)
					.ThenBy(x => x.Category==JobCategory.ProgramBridge)
					.ThenBy(x => x.Category==JobCategory.Enhancement)
					.ThenBy(x => x.Category==JobCategory.Bug)
					.ThenBy(x => _listJobPriorities.FirstOrDefault(y => y.DefNum==x.Priority).ItemOrder).ToList();
			Dictionary<JobPhase,List<Job>> dictPhases=new Dictionary<JobPhase,List<Job>>();
			foreach(Job job in listJobsSorted) {
				JobPhase phase=job.PhaseCur;
				if(!dictPhases.ContainsKey(phase)) {
					dictPhases[phase]=new List<Job>();
				}
				dictPhases[phase].Add(job);
			}
			//sort dictionary so actions will appear in same order
			//This is in reverse order in the code so it is correct in the UI
			dictPhases=dictPhases.OrderBy(x => x.Key==JobPhase.Cancelled)
					.ThenBy(x => x.Key==JobPhase.Complete)
					.ThenBy(x => x.Key==JobPhase.Development)
					.ThenBy(x => x.Key==JobPhase.Quote)
					.ThenBy(x => x.Key==JobPhase.Definition)
					.ThenBy(x => x.Key==JobPhase.Concept).ToDictionary(x => x.Key,x => x.Value);
			int indexSelectedJob=-1;
			foreach(KeyValuePair<JobPhase,List<Job>> kvp in dictPhases) {
				if(listJobsSorted.Count==0) {
					continue;
				}
				gridSearch.ListGridRows.Add(new GridRow("","",kvp.Key.ToString()) { ColorBackG=_colorGridHeaderBack,Bold=true });
				foreach(Job job in kvp.Value) {
					Color backColor=Color.White;
					bool isSelectedJob=(selectedJobNum>0 && job.JobNum==selectedJobNum);
					if(isSelectedJob) {
						backColor=Color.LightBlue;//Currently selected job should stand out.
						indexSelectedJob=gridSearch.ListGridRows.Count;
					}
					Def jobPriority=_listJobPriorities.FirstOrDefault(y => y.DefNum==job.Priority);
					gridSearch.ListGridRows.Add(
					new GridRow(
						new GridCell(jobPriority.ItemName) {
							ColorBackG=jobPriority.ItemColor,
							ColorText=(job.Priority==_listJobPriorities.FirstOrDefault(y => y.ItemValue.Contains("Urgent")).DefNum) ? Color.White : Color.Black,
						},
						new GridCell(job.OwnerNum==0 ? "-" : Userods.GetName(job.OwnerNum)),
						new GridCell(job.ToString())
						) {
						Tag=job,
						ColorBackG=backColor
					}
					);
				}
			}
			gridSearch.EndUpdate();
			if(indexSelectedJob>-1) {
				gridSearch.SetSelected(indexSelectedJob,true);
			}
		}

		private void comboPrioritySearch_SelectionChangeCommitted(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void comboCatSearch_SelectionChangeCommitted(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void comboProposedVersionNeedsAction_SelectionChangeCommitted(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void comboProposedVersionNeedsEngineer_SelectionChangeCommitted(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void comboProposedVersionNeedsExpert_SelectionChangeCommitted(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void comboProposedVersionSearch_SelectionChangeCommitted(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void timerUserSearch_Tick(object sender,EventArgs e) {
			timerUserSearch.Stop();
			FillActiveTabGrid();
		}

		private void checkCompleteSearch_CheckedChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void checkCancelledSearch_CheckedChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void checkContactSearch_CheckedChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void textUserSearch_TextChanged(object sender,EventArgs e) {
			timerUserSearch.Stop();
			timerUserSearch.Start();
		}

		private void gridSearch_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridSearch.ListGridRows[e.Row].Tag!=null && gridSearch.ListGridRows[e.Row].Tag is Job) {
				LoadJob((Job)gridSearch.ListGridRows[e.Row].Tag,false);
			}
		}

		#endregion

		#endregion

		private void menuItemAssignTester_Click(object o,EventArgs arg) {
			Job jobOld=gridTesting.SelectedTag<Job>();
			if(jobOld==null) {
				return;
			}
			List<Userod> listUsersForPicker=Userods.GetUsersByJobRole(JobPerm.TestingCoordinator,false);
			FormUserPick FormUP=new FormUserPick();
			FormUP.Text="Assign a Tester";
			FormUP.IsSelectionmode=true;
			FormUP.ListUserodsFiltered=listUsersForPicker;
			FormUP.IsPickNoneAllowed=true;
			FormUP.IsShowAllAllowed=false;
			if(FormUP.ShowDialog()!=DialogResult.OK) {
				return;
			}
			Job jobCur=jobOld.Copy();
			jobCur.UserNumTester=FormUP.SelectedUserNum;
			JobLogs.MakeLogEntryForTesting(jobCur,"Test User assigned to: "+Userods.GetName(FormUP.SelectedUserNum));
			if(Jobs.Update(jobCur,jobOld)) {
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jobCur.JobNum);
				GoToJob(jobCur.JobNum);
			}
		}
		
		private void menuItemMarkAsTested_Click(object o,EventArgs arg) {
			Job jobOld=gridTesting.SelectedTag<Job>();
			if(jobOld==null) {
				return;
			}
			Job jobCur=jobOld.Copy();
			jobCur.DateTimeTested=DateTime.Now;
			JobLogs.MakeLogEntryForTesting(jobCur,"Job Marked as Tested");
			if(Jobs.Update(jobCur,jobOld)) {
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jobCur.JobNum);
				GoToJob(jobCur.JobNum);
			}
		}
		
		private void menuItemClearTested_Click(object o,EventArgs arg) {
			Job jobOld=gridTesting.SelectedTag<Job>();
			if(jobOld==null) {
				return;
			}
			Job jobCur=jobOld.Copy();
			jobCur.DateTimeTested=DateTime.MinValue;
			JobLogs.MakeLogEntryForTesting(jobCur,"Job Marked as Untested");
			if(Jobs.Update(jobCur,jobOld)) {
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jobCur.JobNum);
				GoToJob(jobCur.JobNum);
			}
		}

		private void datePickerFrom_ValueChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void datePickerTo_ValueChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		///<summary>Attempts to identify flags for the given job.
		///Also constructs an explanation to be displayed when mouse hovers over jobs with flags.</summary>
		private string FlagHelper(Job jobCur,int gridIndex) {
			List<string> listNotes=new List<string>();
			if(jobCur.PhaseCur.In(JobPhase.Concept,JobPhase.Quote) && jobCur.ListJobQuotes.Count>0) {
				jobCur.TagOD=Color.Red;
				listNotes.Add("$: Quote Pending");
			}
			if(jobCur.PhaseCur.In(JobPhase.Development,JobPhase.Definition) && jobCur.ListJobQuotes.Count>0) {
				jobCur.TagOD=Color.Black;
				listNotes.Add("$: Quote Approved");
			}
			if(jobCur.ListJobReviews.Exists(x => x.ReviewStatus==JobReviewStatus.Done)) {
				jobCur.TagOD=Color.Black;
				listNotes.Add("R: Reviewed");
			}
			else if(jobCur.ListJobLinks.Exists(x => x.LinkType==JobLinkType.Appointment)) {
				jobCur.TagOD=Color.Red;
				listNotes.Add("A: Appt");
			}
			if(jobCur.ListJobReviews.Exists(x => x.ReviewStatus==JobReviewStatus.SaveCommit)) {
				jobCur.TagOD=Color.Red;
				listNotes.Add("S: Save Commit");
			}
			else if(jobCur.ListJobReviews.Exists(x => x.ReviewStatus==JobReviewStatus.SaveCommitted)) {
				jobCur.TagOD=Color.Black;
				listNotes.Add("S: Save Committed");
			}
			_dicRowNotes.Add(gridIndex,listNotes);//To be used on mouse hover.
			List<string> listFlagCodes=listNotes.Select(x => x.Substring(0,x.IndexOf(":"))).ToList();
			return string.Join(",",listFlagCodes);
		}

		private void menuGoToAccount_Click(object sender,EventArgs e) {
			GotoModule.GotoChart(((Job)gridQueries.ListGridRows[gridQueries.SelectedIndices[0]].Tag).ListJobQuotes[0].PatNum);
		}

		private void butBack_Click(object sender,EventArgs e) {
			if(_stackBackJobNums.Count>0) {
				long jobNumBack=_stackBackJobNums.Pop();
				GoToJob(jobNumBack,LoadJobAction.Back);
			}
		}

		private void butForward_Click(object sender,EventArgs e) {
			if(_stackForwardJobNums.Count>0) {
				long jobNumForward=_stackForwardJobNums.Pop();
				GoToJob(jobNumForward,LoadJobAction.Forward);
			}
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			if(userControlJobEdit.Visible) {
				GoToJob(userControlJobEdit.GetJob().JobNum);
			}
			else {
				GoToJob(userControlQueryEdit.GetJob().JobNum);
			}
		}

		private void butAddJob_Click(object sender,EventArgs e) {
			AddNewJob();
		}

		private void butAddChildJob_Click(object sender,EventArgs e) {
			if(userControlJobEdit.GetJob()==null) {
				MsgBox.Show(this,"No job currently selected. Select a job to be a parent job.");
				return;
			}
			AddNewJob(userControlJobEdit.GetJob().JobNum);
		}

		private void AddNewJob(long parentNum=0) {
			Job jobNew=new Job();
			jobNew.ParentNum=parentNum;
			List<string> categoryList=Enum.GetNames(typeof(JobCategory)).ToList();
			if(!JobPermissions.IsAuthorized(JobPerm.QueryTech,true)) {
				//Queries can't be created from here
				categoryList.Remove("Query");
			}
			if(!JobPermissions.IsAuthorized(JobPerm.SpecialProject,true)) {
				//SpecialProjects can't be created from here
				categoryList.Remove("SpecialProject");
			}
			if(!JobPermissions.IsAuthorized(JobPerm.ProjectManager,true)) {
				//SpecialProjects can't be created from here
				categoryList.Remove("NeedNoApproval");
			}
			if(!JobPermissions.IsAuthorized(JobPerm.Concept,true)) {
				categoryList.Remove("Feature");
				categoryList.Remove("Bug");
				categoryList.Remove("Enhancement");
				categoryList.Remove("ProgramBridge");
				categoryList.Remove("InternalRequest");
				categoryList.Remove("Conversion");
				categoryList.Remove("Research");
			}
			if(categoryList.Count==0) {//Should only happen if we forget to stop them from being able to click the button
				MsgBox.Show(this,"You are not authorized to create jobs.");
				return;
			}
			InputBox categoryChoose=new InputBox("Choose a job category",categoryList,0);
			if(categoryChoose.ShowDialog()!=DialogResult.OK) {
				return;
			}
			if(categoryChoose.comboSelection.SelectedIndex==-1) {//Shouldn't ever happen, but I am leaving this here
				MsgBox.Show(this,"You must choose a category to create a job.");
				return;
			}
			List<Def> listJobPriorities=Defs.GetDefsForCategory(DefCat.JobPriorities,true);
			if(listJobPriorities.Count==0) {
				MsgBox.Show(this,"You have no priorities setup in definitions.");
				return;
			}
			jobNew.Category=(JobCategory)Enum.GetNames(typeof(JobCategory)).ToList().IndexOf(categoryChoose.comboSelection.GetSelected<string>());
			long priorityNum=0;
			priorityNum=listJobPriorities.FirstOrDefault(x => x.ItemValue.Contains("JobDefault")).DefNum;
			if(jobNew.Category.In(JobCategory.Bug,JobCategory.Conversion,JobCategory.NeedNoApproval)) {
				priorityNum=listJobPriorities.FirstOrDefault(x => x.ItemValue.Contains("BugDefault")).DefNum;
			}
			jobNew.Priority=priorityNum==0 ? listJobPriorities.First().DefNum : priorityNum;
			jobNew.PhaseCur=JobPhase.Concept;
			jobNew.UserNumConcept=Security.CurUser.UserNum;
			if(jobNew.Category!=JobCategory.Query) {
				FormJobAdd FormJA=new FormJobAdd(jobNew);
				if(FormJA.ShowDialog()==DialogResult.OK) {
					GoToJob(jobNew.JobNum);
					Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jobNew.JobNum);
				}
				return;//We don't want to continue for normal jobs
			}
			//------------EVERYTHING BELOW IS ONLY FOR QUERY JOBS----------------------
			InputBox titleBox=new InputBox("Provide a brief title for the job.");
			if(titleBox.ShowDialog()!=DialogResult.OK) {
				return;
			}
			if(String.IsNullOrEmpty(titleBox.textResult.Text)) {
				MsgBox.Show(this,"You must type a title to create a job.");
				return;
			}
			jobNew.Title=titleBox.textResult.Text;
			JobLink jobLinkNew=new JobLink();
			JobQuote jobQuoteNew=new JobQuote();
			Bug bugNew=new Bug();
			if(jobNew.Category==JobCategory.Bug) {
				jobLinkNew.LinkType=JobLinkType.Bug;
				bugNew=Bugs.GetNewBugForUser();
				InputBox bugDescription=new InputBox("Provide a brief description for the bug. This will appear in the bug tracker.",jobNew.Title);
				if(bugDescription.ShowDialog()!=DialogResult.OK) {
					return;
				}
				if(String.IsNullOrEmpty(bugDescription.textResult.Text)) {
					MsgBox.Show(this,"You must type a description to create a bug.");
					return;
				}
				FormVersionPrompt FormVP=new FormVersionPrompt("Enter versions found");
				FormVP.ShowDialog();
				if(FormVP.DialogResult!=DialogResult.OK || string.IsNullOrEmpty(FormVP.VersionText)) {
					MsgBox.Show(this,"You must type a description to create a bug.");
					return;
				}
				bugNew.Status_=BugStatus.Accepted;
				bugNew.VersionsFound=FormVP.VersionText;
				bugNew.Description=bugDescription.textResult.Text;
			}
			else if(jobNew.Category==JobCategory.Query) {
				FormPatientSelect FormPS=new FormPatientSelect();
				if(jobQuoteNew.PatNum!=0) {
					FormPS.ExplicitPatNums=new List<long> { jobQuoteNew.PatNum };
				}
				FormPS.ShowDialog();
				if(FormPS.DialogResult!=DialogResult.OK) {
					return;
				}
				Patient pat=Patients.GetPat(FormPS.SelectedPatNum);
				if(!pat.BillingType.In(41,165,183,200,219,224,226,288,371,379,423,430,436,485,486) &&
					!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This customer is currently not on support.\r\nIf you choose to continue, be sure to discuss support options with this customer.")) 
				{
					return;
				}
				if(pat!=null) {
					jobQuoteNew.PatNum=pat.PatNum;
				}
				else {
					jobQuoteNew.PatNum=0;
				}
			}
			Jobs.Insert(jobNew);
			jobLinkNew.JobNum=jobNew.JobNum;
			if(jobNew.Category==JobCategory.Bug) {
				jobLinkNew.FKey=Bugs.Insert(bugNew);
				JobLinks.Insert(jobLinkNew);
			}
			if(jobNew.Category==JobCategory.Query) {
				jobQuoteNew.JobNum=jobNew.JobNum;
				JobQuotes.Insert(jobQuoteNew);
			}
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jobNew.JobNum);
			GoToJob(jobNew.JobNum);
		}

		private void butOverride_Click(object sender,EventArgs e) {
			_isOverride=true;
			userControlJobEdit.IsOverride=true;
		}

		private void userControlJobEdit_JobOverride(object sender,bool isOverride) {
			_isOverride=isOverride;
		}

		private void userControlJobEdit_SaveClick(object sender,EventArgs e) {
			Job jobNew=userControlJobEdit.GetJob();
			Job jobStale=_listJobsAll.FirstOrDefault(x=>x.JobNum==jobNew.JobNum);
			if(jobStale==null) {
				_listJobsAll.Add(jobNew);
			}
			else {
				_listJobsAll[_listJobsAll.IndexOf(jobStale)]=jobNew;
			}
			UpdateNodes(jobNew);
			FillActiveTabGrid();
		}

		private void userControlQueryEdit_SaveClick(object sender,EventArgs e) {
			Job jobNew=userControlQueryEdit.GetJob();
			Job jobStale=_listJobsAll.FirstOrDefault(x=>x.JobNum==jobNew.JobNum);
			if(jobStale==null) {
				_listJobsAll.Add(jobNew);
			}
			else {
				_listJobsAll[_listJobsAll.IndexOf(jobStale)]=jobNew;
			}
			UpdateNodes(jobNew);
			FillActiveTabGrid();
		}

		///<summary>Flat recursion. Updates any nodes displaying outdated information for the passed in job (identified by JobNum). Does not move nodes in tree, just updates job information.</summary>
		private void UpdateNodes(Job jobNew) {
			List<TreeNode> treeNodes=new List<TreeNode>(treeJobs.Nodes.Cast<TreeNode>());
			for(int i=0;i<treeNodes.Count;i++) {//flat recursion
				TreeNode nodeCur=treeNodes[i];
				if((nodeCur.Tag is Job) && ((Job)nodeCur.Tag).JobNum==jobNew.JobNum) {
					nodeCur.Text=jobNew.ToString();//update label if Title has changed.
					nodeCur.Tag=jobNew;
				}
				treeNodes.AddRange(nodeCur.Nodes.Cast<TreeNode>());
			}
		}

		public void GoToJob(long jobNum) {
			GoToJob(jobNum,LoadJobAction.Select);
		}

		private void GoToJob(long jobNum,LoadJobAction loadAction) {
			Job job=Jobs.GetOneFilled(jobNum);
			if(job==null) {
				MessageBox.Show("Job not found.");
				return;
			}
			LoadJob(job,true,loadAction);
		}

		private void userControlJobEdit_RequestJob(object sender,long jobNum) {
			Job job=_listJobsAll.FirstOrDefault(x => x.JobNum==jobNum);
			if(job==null) {
				GoToJob(jobNum);//Try and get the job from the database.
				return;
			}
			LoadJob(job,true);
		}

		private void textSearchAction_TextChanged(object sender,EventArgs e) {
			timerSearch.Stop();
			timerSearch.Start();
		}

		private void textSearch_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.Enter) {
				JobSearch();
			}
		}

		private void JobSearch() {
			string searchTrimmed=textSearch.Text.Trim();
			Job jobFound=null;
			if(!string.IsNullOrEmpty(searchTrimmed) && textSearch.Text.All(x => char.IsNumber(x))) {
				jobFound=Jobs.GetOneFilled(PIn.Long(searchTrimmed));
			}
			else {
				jobFound=Jobs.GetOneFilled(searchTrimmed);
			}
			if(jobFound!=null) {
				//Load the new job into the job mananger controls and cache.
				LoadJob(jobFound,false);
				RefreshGridsForSearch();
				gridSearch.ScrollToIndex(gridSearch.GetSelectedIndex());
			}
			tabControlNav.SelectedTab=tabSearch;
		}

		private void RefreshGridsForSearch() {
			timerSearch.Stop();
			FillActiveTabGrid();
		}

		private void butAdvSearch_Click(object sender,EventArgs e) {
			FormJobSearch FormJS=new FormJobSearch(_listJobsAll);
			FormJS.InitialSearchString=textSearch.Text;
			//pass in data here to reduce calls to DB.
			FormJS.ShowDialog(this);
			if(FormJS.DialogResult!=DialogResult.OK) {
				return;
			}
			comboGroup.SelectedIndex=(int)GroupJobsBy.None;
			checkCollapse.Checked=false;
			checkIncludeCustContact.Checked=true;
			checkResults.Checked=true;//sets control visibility as well.
			tabControlNav.SelectedTab=tabTree;//tree view to see search results.
			LoadJob(FormJS.SelectedJob,true);
			//Make the tree represent the search results...  Preserves old behavior.
			_listJobsFiltered=FormJS.GetSearchResults();
			_listJobsAll.AddRange(_listJobsFiltered.FindAll(x => !_listJobsAll.Exists(y => y.JobNum==x.JobNum)));
			FillTree();
		}

		private void timerSearch_Tick(object sender,EventArgs e) {
			RefreshGridsForSearch();
		}

		private void butSearch_Click(object sender,EventArgs e) {
			JobSearch();
		}

		private void butMe_Click(object sender,EventArgs e) {
			if(comboUser.Tag!=Security.CurUser) {
				comboUser.Tag=Security.CurUser;
			}
			else {
				comboUser.Tag=new Userod();
			}
			FillComboUser();
			FillActiveTabGrid();
		}

		public override void OnProcessSignals(List<Signalod> listSignals) {
			if(!listSignals.Exists(x => x.IType==InvalidType.Jobs || x.IType==InvalidType.Security || x.IType==InvalidType.Defs)) {
				return;//no job signals;
			}
			if(listSignals.Any(x => x.IType==InvalidType.Security)) {
				FillComboUser();
			}
			if(listSignals.Any(x => x.IType==InvalidType.Defs)) {
				FillPriorityList();
			}
			//Get the job nums from the signals passed in.
			List<long> listJobNums=listSignals.FindAll(x => x.IType==InvalidType.Jobs && x.FKeyType==KeyType.Job)
				.Select(x => x.FKey)
				.Distinct()
				.ToList();
			RefreshAndFillThreaded(listJobNums);
		}

		private void gridAction_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			OpenNonModalJob(GetSelectedJob(gridAction,e.Row),_listJobsAll);
		}

		private void gridSpecial_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			OpenNonModalJob(GetSelectedJob(gridSpecial,e.Row),_listJobsAll);
		}

		private void gridDocumentation_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			OpenNonModalJob(GetSelectedJob(gridDocumentation,e.Row),_listJobsAll);
		}

		private void gridAvailableJobs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			OpenNonModalJob(GetSelectedJob(gridAvailableJobs,e.Row),_listJobsAll);
		}

		private void gridAvailableJobsExpert_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			OpenNonModalJob(GetSelectedJob(gridAvailableJobsExpert,e.Row),_listJobsAll);
		}

		private void gridJobsOnHold_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			OpenNonModalJob(GetSelectedJob(gridJobsOnHold,e.Row),_listJobsAll);
		}

		private void gridSearch_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			OpenNonModalJob(GetSelectedJob(gridSearch,e.Row),_listJobsAll);
		}

		private void gridTesting_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			OpenNonModalJob(GetSelectedJob(gridTesting,e.Row),_listJobsAll);
		}

		private void gridQueries_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			OpenNonModalJob(GetSelectedJob(gridQueries,e.Row),_listJobsAll);
		}

		private void gridNotify_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			OpenNonModalJob(GetSelectedJob(gridNotify,e.Row),_listJobsAll);
		}

		private void gridSubscribedJobs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			OpenNonModalJob(GetSelectedJob(gridSubscribedJobs,e.Row),_listJobsAll);
		}

		///<summary>Helper method to get job from the passed in grid and row selection. Can return null if not called correctly.</summary>
		private Job GetSelectedJob(ODGrid grid,int rowSelection) {
			Job job;
			try {
				job=(Job)grid.ListGridRows[rowSelection].Tag;
			}
			catch(Exception ex) {
				job=null;
			}
			return job;
		}

		private void treeJobs_NodeMouseDoubleClick(object sender,TreeNodeMouseClickEventArgs e) {
			OpenNonModalJob((Job)e.Node.Tag,_listJobsAll);
		}

		public static void OpenNonModalJob(Job job,List<Job> listJobs) {
			if(job==null) {//Double clicking on a title row, or something went wrong.
				return;
			}
			FormJobEdit FormJE=_listJobEditForms.FirstOrDefault(x=> x.JobCur.JobNum==job.JobNum);
			if(FormJE==null) {
				FormJE=new FormJobEdit(job,listJobs);
				FormJE.Show();
				_listJobEditForms.Add(FormJE);
				return;
			}
			FormJE.Activate();
		}

		public static void RemoveFormJobEdit(FormJobEdit form) {
			_listJobEditForms.Remove(form);
		}

		private void menuItemNone_Click(object sender,EventArgs e) {
			UpdatePatternReviewStatusForSelected(JobPatternReviewStatus.None);
		}

		private void menuItemNotNeeded_Click(object sender,EventArgs e) {
			UpdatePatternReviewStatusForSelected(JobPatternReviewStatus.NotNeeded);
		}

		private void menuItemAwaiting_Click(object sender,EventArgs e) {
			UpdatePatternReviewStatusForSelected(JobPatternReviewStatus.AwaitingApproval);
		}

		private void menuItemApproved_Click(object sender,EventArgs e) {
			UpdatePatternReviewStatusForSelected(JobPatternReviewStatus.NotApproved);
		}

		private void menuItemTentative_Click(object sender,EventArgs e) {
			UpdatePatternReviewStatusForSelected(JobPatternReviewStatus.Tentative);
		}

		private void menuItemNotApproved_Click(object sender,EventArgs e) {
			UpdatePatternReviewStatusForSelected(JobPatternReviewStatus.NotApproved);
		}

		private void UpdatePatternReviewStatusForSelected(JobPatternReviewStatus status) {
			Job job=null;
			if(gridPatternReview.GetSelectedIndex()!=-1) {
				job=((Job)gridPatternReview.ListGridRows[gridPatternReview.GetSelectedIndex()].Tag);
			}
			if(job==null) {
				return;
			}
			Job jobOld=job.Copy();
			job.PatternReviewStatus=status;
			Jobs.Update(job);
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,job.JobNum);
			LoadJob(job,true);
			JobLogs.MakeLogEntryForPatternReview(job,jobOld);
			FillGridPatternReview();
		}

		private void dateExcludeCompleteBefore_ValueChanged(object sender,EventArgs e) {
			FillGridPatternReview();
		}

		private void gridPatternReview_CellClick(object sender,ODGridClickEventArgs e) {
			if( gridPatternReview.ListGridRows[e.Row].Tag==null || !( gridPatternReview.ListGridRows[e.Row].Tag is Job)) {
				return;
			}
			Job job=((Job) gridPatternReview.ListGridRows[e.Row].Tag);
			LoadJob(job,false);
		}

		private void gridAction_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridAction.ListGridRows[e.Row].Tag==null) {
				return;
			}
			//Rows that have a tag of type JobAction are the 'category title' rows that are bold with a colored background.
			//When a user single clicks on one of these rows it should collapse or expand the entire category based on its current state.
			if(gridAction.ListGridRows[e.Row].Tag is JobAction) {
				JobAction jobAction=(JobAction)gridAction.ListGridRows[e.Row].Tag;
				if(_listJobActionsCollapsed.Contains(jobAction)) {
					_listJobActionsCollapsed.RemoveAll(x => x==jobAction);
				}
				else {
					_listJobActionsCollapsed.Add(jobAction);
				}
				FillGridActions();
				return;
			}
			//Rows that have a tag of type Job should cause the entire JM window and registered entities notified that they need to load the selected job.
			if(gridAction.ListGridRows[e.Row].Tag is Job) {
				LoadJob((Job)gridAction.ListGridRows[e.Row].Tag,false);
				return;
			}
		}

		private void gridSpecial_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridSpecial.ListGridRows[e.Row].Tag!=null && gridSpecial.ListGridRows[e.Row].Tag is Job) {
				LoadJob((Job)gridSpecial.ListGridRows[e.Row].Tag,false);
			}
		}

		private void gridDocumention_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridDocumentation.ListGridRows[e.Row].Tag!=null && gridDocumentation.ListGridRows[e.Row].Tag is Job) {
				LoadJob((Job)gridDocumentation.ListGridRows[e.Row].Tag,false);
			}
		}

		private void gridNotify_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridNotify.ListGridRows[e.Row].Tag!=null && gridNotify.ListGridRows[e.Row].Tag is Job) {
				LoadJob((Job)gridNotify.ListGridRows[e.Row].Tag,false);
			}
		}

		private void checkShowAllUsers_CheckedChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void checkHideTested_CheckedChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void checkHideNotTested_CheckedChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void checkSubscribedIncludeCancelled_CheckedChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void checkSubscribedIncludeComplete_CheckedChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void checkSubscribedIncludeOnHold_CheckedChanged(object sender,EventArgs e) {
			FillActiveTabGrid();
		}

		private void gridQueries_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridQueries.ListGridRows[e.Row].Tag!=null && gridQueries.ListGridRows[e.Row].Tag is Job) {
				LoadJob((Job)gridQueries.ListGridRows[e.Row].Tag,false);
				FillActiveTabGrid();
			}
		}

		private void butQueriesRefresh_Click(object sender,EventArgs e) {
			//Completed and Cancelled jobs are not cached by default.
			//Go get any jobs from the database that match the filter criteria for the queries tab (check boxes and date range).
			if(checkShowQueryComplete.Checked || checkShowQueryCancelled.Checked) {
				List<Job> listJobs=Jobs.GetForQueries(dateFrom.Value,dateTo.Value,_listJobsAll.Select(x => x.JobNum).ToList());
				if(!listJobs.IsNullOrEmpty()) {
					Jobs.FillInMemoryLists(listJobs);
					_listJobsAll.AddRange(listJobs);
				}
			}
			FillActiveTabGrid();
		}

		private void gridAction_MouseMove(object sender,MouseEventArgs e) {
			if(gridAction.Title!="Action Items") {
				return;//Only show toolTip when Non-Documentation.
			}
			if(_toolTipHover.Tag!=null && (Point)_toolTipHover.Tag==e.Location) {
				return;//Mouse has not moved. Avoid flicker.
			}
			_toolTipHover.Tag=e.Location;
			ODGrid grid=(ODGrid)sender;
			int row=grid.PointToRow(grid.PointToClient(Cursor.Position).Y);
			int col=grid.PointToCol(grid.PointToClient(Cursor.Position).X);
			if(row==-1 || !_dicRowNotes.ContainsKey(row) || col!=1) {
				_toolTipHover.RemoveAll();
				return;
			}
			_toolTipHover.SetToolTip(grid,string.Join("\r",_dicRowNotes[row]));
		}

		private void toolTipHover_PopupHelper(object sender,PopupEventArgs e) {
			string message=_toolTipHover.GetToolTip(e.AssociatedControl);
			Size size=TextRenderer.MeasureText(message,label4.Font);
			size.Width+=2;//Padding
			size.Height+=2;//Padding
			e.ToolTipSize=size;
		}

		private void toolTipHover_Draw(object sender,DrawToolTipEventArgs e) {
			e.DrawBackground();
			e.DrawBorder();
			List<string> listNotes=e.ToolTipText.Split('\r').ToList();//This way we can change individual Notes text color.
			int y=0;
			foreach(string str in listNotes) {
				Brush brush=Brushes.Black;//Default 
				if(str=="$: Quote Pending") {
					brush=Brushes.Red;
				}
				e.Graphics.DrawString(str,label4.Font,brush,2,y);
				y+=TextRenderer.MeasureText(str,label4.Font).Height;
			}
		}

		private void gridAvailableJobs_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridAvailableJobs.ListGridRows[e.Row].Tag!=null && gridAvailableJobs.ListGridRows[e.Row].Tag is Job) {
				LoadJob((Job)gridAvailableJobs.ListGridRows[e.Row].Tag,false);
			}
		}

		private void gridAvailableJobsExpert_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridAvailableJobsExpert.ListGridRows[e.Row].Tag!=null && gridAvailableJobsExpert.ListGridRows[e.Row].Tag is Job) {
				LoadJob((Job)gridAvailableJobsExpert.ListGridRows[e.Row].Tag,false);
			}
		}

		private void gridJobsOnHold_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridJobsOnHold.ListGridRows[e.Row].Tag!=null && gridJobsOnHold.ListGridRows[e.Row].Tag is Job) {
				LoadJob((Job)gridJobsOnHold.ListGridRows[e.Row].Tag,false);
			}
		}

		private void gridSubscribedJobs_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridSubscribedJobs.ListGridRows[e.Row].Tag!=null && gridSubscribedJobs.ListGridRows[e.Row].Tag is Job) {
				LoadJob((Job)gridSubscribedJobs.ListGridRows[e.Row].Tag,false);
				FillActiveTabGrid();
				RefreshTabs();
			}
		}

		private void comboUser_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboUser.SelectedIndex==0) {//All
				comboUser.Tag=new Userod() { UserNum=0 };
			}
			else if(comboUser.SelectedIndex==1) {//Unassigned
				comboUser.Tag=new Userod() { UserNum=-1 };
			}
			else {
				comboUser.Tag=_listUsers[comboUser.SelectedIndex-2];
			}
			FillActiveTabGrid();
			this.Text="Job Manager"+(comboUser.Text=="" ? "" : " - "+comboUser.Text);
		}
		
		private void butDashboard_Click(object sender,EventArgs e) {
			FormJobSprints FormJS=new FormJobSprints();
			if(FormJS.ShowDialog()!=DialogResult.OK) {
				return;
			}
			RefreshAndFill(Jobs.GetCompletedJobNumsForRange(FormJS.SelectedJobSprint.DateStart,FormJS.SelectedJobSprint.DateEndTarget));
			FormJobManagerDashboard FormJMD=new FormJobManagerDashboard(_listJobsAll,FormJS.SelectedJobSprint);
			FormJMD.Show(this);
		}
		
		private void butBugSubs_Click(object sender,EventArgs e) {
			FormBugSubmissions FormBugSubs=new FormBugSubmissions();
			FormBugSubs.Show();//Non-modal
		}

		private void butReleaseCalc_Click(object sender,EventArgs e) {
			if(_listJobsAll.Count==0) {
				return;
			}
			if(Application.OpenForms.OfType<FormReleaseCalculator>().Count()>0) {
				Application.OpenForms.OfType<FormReleaseCalculator>().ToList()[0].BringToFront();
				return;
			}
			FormReleaseCalculator FormRC=new FormReleaseCalculator(_listJobsAll);
			FormRC.Show(this);
		}

		private void jobTimeHelperToolStripMenuItem_Click(object sender,EventArgs e) {
			if(Application.OpenForms.OfType<FormJobTime>().Count()>0) {
				Application.OpenForms.OfType<FormJobTime>().ToList()[0].BringToFront();
				return;
			}
			FormJobTime FormJT=new FormJobTime(_listJobsAll);
			FormJT.Show(this);
		}

		private void jobOverviewToolStripMenuItem_Click(object sender,EventArgs e) {
			if(_listJobsAll.Count==0) {
				return;
			}
			if(Application.OpenForms.OfType<FormJobManagerOverview>().Count()>0) {
				Application.OpenForms.OfType<FormJobManagerOverview>().ToList()[0].BringToFront();
				return;
			}
			FormJobManagerOverview FormJMO=new FormJobManagerOverview(_listJobsAll);
			FormJMO.Show(this);
		}

		private void backportToolStripMenuItem_Click(object sender,EventArgs e) {
			FormBackport formB=new FormBackport(userControlJobEdit.JobNumCur);
			formB.Show();
		}

		private void gridTesting_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridTesting.ListGridRows[e.Row].Tag!=null && gridTesting.ListGridRows[e.Row].Tag is Job) {
				LoadJob((Job)gridTesting.ListGridRows[e.Row].Tag,false);
			}
		}

		private void butTestingRefresh_Click(object sender,EventArgs e) {
			FillActiveTabGrid();
		}
		
		private void textDocumentationVersion_TextChanged(object sender,EventArgs e) {
			timerDocumentationVersion.Stop();
			timerDocumentationVersion.Start();
		}

		private void timerDocumentationVersion_Tick(object sender,EventArgs e) {
			timerDocumentationVersion.Stop();
			FillActiveTabGrid();
		}

		private bool JobUnsavedChangesCheck() {
			if(userControlJobEdit.IsChanged) {
				switch(MessageBox.Show("Save changes to current job?","",MessageBoxButtons.YesNoCancel)) {
					case System.Windows.Forms.DialogResult.OK:
					case System.Windows.Forms.DialogResult.Yes:
						if(!userControlJobEdit.ForceSave()) {
							return true;
						}
						break;
					case System.Windows.Forms.DialogResult.No:
						CheckinJob();
						break;
					case System.Windows.Forms.DialogResult.Cancel:
						return true;
				}
			}
			return false;//no unsaved changes
		}

		private bool QueryUnsavedChangesCheck() {
			if(userControlQueryEdit.IsChanged) {
				switch(MessageBox.Show("Save changes to current job?","",MessageBoxButtons.YesNoCancel)) {
					case System.Windows.Forms.DialogResult.OK:
					case System.Windows.Forms.DialogResult.Yes:
						if(!userControlQueryEdit.ForceSave()) {
							return true;
						}
						break;
					case System.Windows.Forms.DialogResult.No:
						CheckinQuery();
						break;
					case System.Windows.Forms.DialogResult.Cancel:
						return true;
				}
			}
			return false;//no unsaved changes
		}

		private void CheckinJob() {
			Job jobCur=userControlJobEdit.GetJob();
			if(jobCur==null) {
				return;
			}
			if(jobCur.UserNumCheckout==Security.CurUser.UserNum) {
				jobCur=Jobs.GetOne(jobCur.JobNum);
				jobCur.UserNumCheckout=0;
				Jobs.Update(jobCur);
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jobCur.JobNum);
			}
		}

		private void CheckinQuery() {
			Job jobCur=userControlQueryEdit.GetJob();
			if(jobCur==null) {
				return;
			}
			if(jobCur.UserNumCheckout==Security.CurUser.UserNum) {
				jobCur=Jobs.GetOne(jobCur.JobNum);
				jobCur.UserNumCheckout=0;
				Jobs.Update(jobCur);
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jobCur.JobNum);
			}
		}

		private void checkResults_CheckedChanged(object sender,EventArgs e) {
			if(!checkResults.Checked) {
				//Unfilter results if we unchecked it.
				_listJobsFiltered=_listJobsAll.Select(x=>x.Copy()).ToList();
				FillTree();
			}
			//visible==Checked
			checkResults.Visible=checkResults.Checked;
			//visible==!Checked
			comboCategorySearch.Visible=!checkResults.Checked;
			comboGroup.Visible=!checkResults.Checked;
			checkCollapse.Visible=!checkResults.Checked;
			checkIncludeCustContact.Visible=!checkResults.Checked;
			labelCategory.Visible=!checkResults.Checked;
			labelGroupBy.Visible=!checkResults.Checked;
		}

		private bool CloseJobEditForms() {
			for(int i=Application.OpenForms.Count-1;i>=0;i--) {
				Form formToClose=Application.OpenForms[i];
				if(formToClose.GetType() == typeof(FormJobEdit)) {
					//If the window which showed the messagebox popup causes the form to stay open, then stop the log off event, because the user chose to.
					formToClose.InvokeIfRequired(() => formToClose.Close());//Attempt to close the form, even if created in another thread.
					//Run Application.DoEvents() to allow the FormClosing/FormClosed events to fire in the form before checking if they have closed below.
					Application.DoEvents();//Required due to invoking.  Otherwise FormClosing/FormClosed will not fire until after we exit.
					if(!IsDisposedOrClosed(formToClose)) {
						return false;//This form needs to stay open and stop all other forms from being closed.
					}
					RemoveFormJobEdit((FormJobEdit)formToClose);
				}
			}
			return true;
		}

		private void FormJobManager_FormClosing(object sender,FormClosingEventArgs e) {
			if(JobUnsavedChangesCheck()) {
				e.Cancel=true;
				return;
			}
			if(!CloseJobEditForms()) {
				e.Cancel=true;
				return;
			}
			//Close();//FormJobManager wasn't closing properly if there were any FormJobEdits open. May have to do with calling Application.DoEvents()
			ODThread.QuitSyncThreadsByGroupName(100,"RefreshAndFillJobManager");//Give the thread 100ms before killing it.
		}

		///<summary>For UI only. Never saved to DB.</summary>
		private enum GroupJobsBy {
			None,
			MyHierarchy,
			Hierarchy,
			User,
			Status,
			Owner
		}

		///<summary>For UI only. Never saved to DB.</summary>
		private enum LoadJobAction {
			Select,
			Back,
			Forward
		}
	}
}
