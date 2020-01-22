using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormTaskListSelect : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.ListBox listMain;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private TaskObjectType OType;
		private Label labelMulti;
		private CheckBox checkMulti;
		private CheckBox checkIncludeSubTasks;
		public List<long> ListSelectedLists=new List<long>();
		private List<TaskList> _listUnfilteredTaskList;
		///<summary>Can be null. The inbox of the current users. Used in order to add a shortcut to send to at top of list.</summary>
		private TaskList _userCurTaskListInbox;
		private TextBox textFilter;
		private Label label1;

		///<summary></summary>
		public FormTaskListSelect(TaskObjectType oType,bool IsTaskNew=false)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			OType=oType;
			checkMulti.Visible=IsTaskNew;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTaskListSelect));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.listMain = new System.Windows.Forms.ListBox();
			this.labelMulti = new System.Windows.Forms.Label();
			this.checkMulti = new System.Windows.Forms.CheckBox();
			this.textFilter = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.checkIncludeSubTasks = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(207, 423);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(207, 393);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// listMain
			// 
			this.listMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listMain.HorizontalScrollbar = true;
			this.listMain.Location = new System.Drawing.Point(11, 150);
			this.listMain.Name = "listMain";
			this.listMain.Size = new System.Drawing.Size(182, 303);
			this.listMain.TabIndex = 2;
			this.listMain.DoubleClick += new System.EventHandler(this.listMain_DoubleClick);
			this.listMain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listMain_KeyDown);
			// 
			// labelMulti
			// 
			this.labelMulti.Location = new System.Drawing.Point(11, 3);
			this.labelMulti.Name = "labelMulti";
			this.labelMulti.Size = new System.Drawing.Size(270, 57);
			this.labelMulti.TabIndex = 10;
			this.labelMulti.Text = "Pick task list to send to.";
			this.labelMulti.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkMulti
			// 
			this.checkMulti.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkMulti.Location = new System.Drawing.Point(12, 112);
			this.checkMulti.Name = "checkMulti";
			this.checkMulti.Size = new System.Drawing.Size(182, 18);
			this.checkMulti.TabIndex = 9;
			this.checkMulti.Text = "Send copies to multiple";
			this.checkMulti.UseVisualStyleBackColor = true;
			this.checkMulti.Visible = false;
			this.checkMulti.CheckedChanged += new System.EventHandler(this.checkMulti_CheckedChanged);
			// 
			// textFilter
			// 
			this.textFilter.Location = new System.Drawing.Point(12, 90);
			this.textFilter.Name = "textFilter";
			this.textFilter.Size = new System.Drawing.Size(181, 20);
			this.textFilter.TabIndex = 0;
			this.textFilter.TextChanged += new System.EventHandler(this.textFilter_TextChanged);
			this.textFilter.DoubleClick += new System.EventHandler(this.textFilter_DoubleClick);
			this.textFilter.Enter += new System.EventHandler(this.textFilter_Enter);
			this.textFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textFilter_KeyUp);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 70);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(183, 17);
			this.label1.TabIndex = 11;
			this.label1.Text = "Search";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkIncludeSubTasks
			// 
			this.checkIncludeSubTasks.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIncludeSubTasks.Location = new System.Drawing.Point(12, 129);
			this.checkIncludeSubTasks.Name = "checkIncludeSubTasks";
			this.checkIncludeSubTasks.Size = new System.Drawing.Size(182, 18);
			this.checkIncludeSubTasks.TabIndex = 12;
			this.checkIncludeSubTasks.Text = "Include sub-task lists";
			this.checkIncludeSubTasks.UseVisualStyleBackColor = true;
			this.checkIncludeSubTasks.CheckedChanged += new System.EventHandler(this.checkIncludeSubTasks_CheckedChanged);
			// 
			// FormTaskListSelect
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(294, 459);
			this.Controls.Add(this.checkIncludeSubTasks);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textFilter);
			this.Controls.Add(this.labelMulti);
			this.Controls.Add(this.checkMulti);
			this.Controls.Add(this.listMain);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(310, 484);
			this.Name = "FormTaskListSelect";
			this.ShowInTaskbar = false;
			this.Text = "Select Task List";
			this.Load += new System.EventHandler(this.FormTaskListSelect_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormTaskListSelect_Load(object sender, System.EventArgs e) {
			SetLabelText();
			_userCurTaskListInbox=TaskLists.GetOne(Security.CurUser.TaskListInBox);
			if(_userCurTaskListInbox!=null) {
				_userCurTaskListInbox.Descript=Lan.g(this,"My Inbox")+": "+_userCurTaskListInbox.Descript;
			}
			else {
				//Is null when the current user does not have an inbox set up
				//or if OType is TaskObjectType.Patient and the current users inbox is not of ObjectType Patient.
			}
			FillList();
		}

		private void LoadUnfilteredTaskLists() {
			switch(OType) {
				case TaskObjectType.Patient:
					_listUnfilteredTaskList=TaskLists.GetForObjectType(OType,false);
					break;
				case TaskObjectType.Appointment:
					_listUnfilteredTaskList=TaskLists.GetForObjectType(OType,false);
					_listUnfilteredTaskList.AddRange(
						GetUserInboxTaskLists().FindAll(x => x.ObjectType!=TaskObjectType.Appointment)
					);
					_listUnfilteredTaskList.Sort(SortTaskListByDescript);
					break;
				case TaskObjectType.None:
					_listUnfilteredTaskList=GetUserInboxTaskLists();
					this.Text=Lan.g(this,"Task Send User");//Form title assumes tasklist.
					break;
				default://Just in case
					_listUnfilteredTaskList=new List<TaskList>();
					break;
			}
		}

		///<summary>Returns a translated string representing the OType to be shown in the UI.</summary>
		private string GetOTypeDescription() {
			string retVal="";
			switch(OType) {
				case TaskObjectType.Patient:
				case TaskObjectType.Appointment:
					retVal=Lan.g(this,"task list");
					break;
				case TaskObjectType.None:
					retVal=Lan.g(this,"user");
					if(checkMulti.Checked) {
						retVal=Lan.g(this,"users");
					}
					break;
			}
			return retVal;
		}

		///<summary>Compares two given tasklist based on their Descript.</summary>
		private static int SortTaskListByDescript(TaskList x,TaskList y) {
			return x.Descript.CompareTo(y.Descript);
		}

		///<summary>Returns a list of TaskList inboxes for non hidden users with an inbox setup.</summary>
		private List<TaskList> GetUserInboxTaskLists() {
			List<TaskList> listUserInboxTaskLists=TaskLists.GetMany(Userods.GetDeepCopy(true).Select(x => x.TaskListInBox).ToList());
			return listUserInboxTaskLists.OrderBy(x => x.Descript).ThenBy(x => x.DateTimeEntry).ToList();
		}

		private void FillList() {
			//Clear the lists for the fill methods
			listMain.Items.Clear();
			//If we are showing all tasks lists, use a different method to fill and filter the list
			if(checkIncludeSubTasks.Checked) {
				FillListSubTasks();
			}
			else {
				LoadUnfilteredTaskLists();
				FillListUser();
			}
			if(listMain.Items.Count!=0) {
				listMain.SelectedIndex=0;//Select first item.
			}
		}

		/// <summary>Builds a list of task list inboxes for a given user, and stores the resulting list and descriptions in the out vars.
		/// Also performs filtering of the list</summary>
		private void FillListUser() {
			List<TaskList> listFilteredTaskLists=FilterList(textFilter.Text,_listUnfilteredTaskList);
			if(_userCurTaskListInbox!=null) {
				if(_userCurTaskListInbox.Descript.ToUpper().Trim().Contains(textFilter.Text.ToUpper().Trim())
					|| listFilteredTaskLists.Any(x => x.TaskListNum==Security.CurUser.TaskListInBox))//Show "My Inbox:" shortcut if current users inbox is in filtered list.
				{
					listFilteredTaskLists.Insert(0,_userCurTaskListInbox);
				}
			}
			TaskList taskListTriage=listFilteredTaskLists.Find(x => x.Descript==" Triage");
			if(taskListTriage!=null) {
				listFilteredTaskLists.Remove(taskListTriage);
				listFilteredTaskLists.Insert(0,taskListTriage);
			}
			listMain.SetItems(listFilteredTaskLists,x => x.Descript);
		}

		/// <summary>Fills listMain with all tasklists with DateType.None.  Each task list will also list out it's children lists.
		/// Items will be filtered the same way as FillListUser (based on textFilter).</summary>
		private void FillListSubTasks() {
			List<TaskList> listAllTaskLists=TaskLists.GetForDateType(TaskDateType.None,false);
			//Convert to dictionary for faster lookups
			Dictionary<long,TaskList> dictAllTaskLists=listAllTaskLists.ToDictionary(x => x.TaskListNum);
			//This will hold the final list of task lists, which will have different descriptions than normal.
			List<TaskList> listFilteredTaskLists=new List<TaskList>();
			TaskList tempTaskList;
			StringBuilder itemName=new StringBuilder();	//String builder because we want speeeed
			foreach(TaskList tList in listAllTaskLists) {
				itemName.Clear();
				TaskList tempList=tList.Copy();	//Copy so we don't modify the original in case it's another list's parent.
				long listParent=tList.Parent;
				itemName.Append(tList.Descript);
				while(listParent!=0) {  //When listParent is 0, is means the current item doesn't have a parent or we can't find the parent.
					if(dictAllTaskLists.TryGetValue(listParent,out tempTaskList)) {	
						listParent=tempTaskList.Parent;
						itemName.Insert(0,tempTaskList.Descript+"/");	//Add the parent name to the beginning of the description
					}
					else {
						//If we can't find the parent, it'll be listed by itself.
						listParent=0;
					}
				}
				tempList.Descript=itemName.ToString();
				//Store task list with extended name in our final list of tasklists
				listFilteredTaskLists.Add(tempList);
			}
			//If the user has typed in a filter term, check it now.
			//We wait until the entire description is created because one task might be filtered, but it's parent isn't
			listFilteredTaskLists=FilterList(textFilter.Text,listFilteredTaskLists).OrderBy(x => x.Descript).ToList();
			//Add the user's primary inbox to the top
			if(_userCurTaskListInbox!=null && _userCurTaskListInbox.Descript.Contains(textFilter.Text)) {
				listFilteredTaskLists.Insert(0,_userCurTaskListInbox);
			}
			listMain.SetItems(listFilteredTaskLists,x => x.Descript);
		}

		///<summary>Returns the list of tasklists filtered from the input list based on the text in textFilter. Items that start with the filter are 
		///placed before items that contain the filter text in the returned list.</summary>
		private List<TaskList> FilterList(string filterText,List<TaskList> listToFilter) {
			string filter=filterText.ToUpper().Trim();
			if(filter=="") {
				return listToFilter;
			}
			List<TaskList> listFiltered=listToFilter.FindAll(x => x.Descript.ToUpper().Trim().StartsWith(filter));
			//Grab all the items that contain the filter string.  This includes items that we grabbed the previous line
			List<TaskList> listContainsFilter=listToFilter.FindAll(x => x.Descript.ToUpper().Trim().Contains(filter));
			//listFiltered will now contain duplicates of items that start with the filter string
			listFiltered.AddRange(listContainsFilter);
			//Distinct() will remove the 2nd occurence of repeated lists, so items that start with the filter only show up once at the start of the list
			//It also preserves the previous order of items.
			return listFiltered.Distinct().ToList();
		}

		///<summary>Sets the main label on this form depending on OType.</summary>
		private void SetLabelText() {
			//strUserSetup only shows when selecting a user to send to.
			string strUserSetup=(OType==TaskObjectType.None ? Lan.g(this,"If a user is not in the list, then their inbox has not been setup yet. ") : "");
			string objTypeStr=GetOTypeDescription();
			if(checkMulti.Checked) {
				listMain.SelectionMode=SelectionMode.MultiSimple;
				labelMulti.Text=Lan.g(this,"Pick")+" "+objTypeStr+" "+Lan.g(this,"to send to.")+"  "
					+strUserSetup+Lan.g(this,"Click on")+" "+objTypeStr+" "+Lan.g(this,"to toggle.");
			}
			else {
				listMain.SelectionMode=SelectionMode.One;
				labelMulti.Text=Lan.g(this,"Pick ")+objTypeStr+" "+Lan.g(this,"to send to.  ")+strUserSetup;
			}
		}

		private void listMain_DoubleClick(object sender, System.EventArgs e) {
			if(listMain.SelectedIndex==-1 || (checkMulti.Checked && PrefC.IsODHQ)){//If user is in checkMulti state, disable doubleclick
				return;
			}
			ListSelectedLists=listMain.GetListSelected<TaskList>().Select(x => x.TaskListNum).ToList();
			DialogResult=DialogResult.OK;
		}

		private void checkMulti_CheckedChanged(object sender,EventArgs e) {
			if(checkMulti.Checked && PrefC.IsODHQ) {//If user is in checkMulti state, deselect the list
				listMain.ClearSelected();
				textFilter.Focus();
			}
			SetLabelText();
		}

		private void checkIncludeSubTasks_CheckedChanged(object sender,EventArgs e) {
			//Refills the list.  This method checks the status of this checkbox.
			FillList();
		}

		private void textFilter_KeyUp(object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.Down) {
				if(!checkMulti.Checked && listMain.Items.Count!=0) {
					listMain.Focus();
					if(listMain.SelectedIndex<listMain.Items.Count-1) {
						listMain.SelectedIndex++;
					}
				}
			}
		}

		private void listMain_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.Up) {//Allow user to get back to the search textbox
				if(!checkMulti.Checked && listMain.Items.Count!=0 && listMain.SelectedIndex==0) {
					textFilter.Focus();
					textFilter.Select(textFilter.Text.Length,0);
				}
			}
		}
		
		private void textFilter_Enter(object sender,EventArgs e) {
			textFilter.SelectAll();
		}

		private void textFilter_DoubleClick(object sender,EventArgs e) {
			textFilter.SelectAll();
		}
		
		private void textFilter_TextChanged(object sender,EventArgs e) {
			FillList();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(listMain.SelectedIndex==-1){
				string msg=Lan.g(this,"Please select a ")+GetOTypeDescription()+Lan.g(this," first.");
				MessageBox.Show(msg);
				return;
			}
			ListSelectedLists=listMain.GetListSelected<TaskList>().Select(x => x.TaskListNum).ToList();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}





















