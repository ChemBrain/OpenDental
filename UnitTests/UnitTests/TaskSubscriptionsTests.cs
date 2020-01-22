using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests.TaskSubscriptions_Tests {
	[TestClass]
	public class TaskSubscriptionsTests:TestBase {
		private long _userNum=1;
		private TaskList taskListParent;
		private TaskList taskListChild;
		private TaskList taskListGrandchild;

		[ClassInitialize]
		public static void SetupClass(TestContext testContext) {
			//Add anything here that you want to run once before the tests in this class run.
		}

		[TestInitialize]
		public void SetupTest() {
			TaskListT.ClearTaskListTable();
			TaskT.ClearTaskTable();
			TaskSubscriptionT.ClearTaskSubscriptionTable();
			taskListParent=TaskListT.CreateTaskList(descript:"TaskList1");
			taskListChild=TaskListT.CreateTaskList(descript:"TaskList1Child",parent:taskListParent.TaskListNum,parentDesc:taskListParent.Descript);
			taskListGrandchild=TaskListT.CreateTaskList(descript:"TaskList1Grandchild",parent:taskListChild.TaskListNum,
				parentDesc:taskListChild.Descript);
		}

		[TestCleanup]
		public void TearDownTest() {
			//Add anything here that you want to run after every test in this class.
		}

		[ClassCleanup]
		public static void TearDownClass() {
			//Add anything here that you want to run after all the tests in this class have been run.
		}

		[TestMethod]
		public void TaskSubscriptions_GetSubscribedTaskLists_SubscribedToParent() {
			TaskSubscriptionT.CreateTaskSubscription(_userNum,taskListParent.TaskListNum);
			List<TaskSubscription> listTaskSubs=TaskSubscriptions.GetTaskSubscriptionsForUser(_userNum);
			Assert.AreEqual(1,listTaskSubs.Count);
			Assert.IsTrue(listTaskSubs.Any(x => x.TaskListNum==taskListParent.TaskListNum));//parent
		}

		[TestMethod]
		public void TaskSubscriptions_GetSubscribedTaskLists_SubscribedToParentAndChild() {
			TaskSubscriptionT.CreateTaskSubscription(_userNum,taskListParent.TaskListNum);
			TaskSubscriptionT.CreateTaskSubscription(_userNum,taskListChild.TaskListNum);
			List<TaskSubscription> listTaskSubs=TaskSubscriptions.GetTaskSubscriptionsForUser(_userNum);
			Assert.AreEqual(2,listTaskSubs.Count);
			Assert.IsTrue(listTaskSubs.Any(x => x.TaskListNum==taskListParent.TaskListNum));//parent
			Assert.IsTrue(listTaskSubs.Any(x => x.TaskListNum==taskListChild.TaskListNum));//child
		}

		[TestMethod]
		public void TaskSubscriptions_GetSubscribedTaskLists_SubscribedToGrandchildOnlyl() {
			TaskSubscriptionT.CreateTaskSubscription(_userNum,taskListGrandchild.TaskListNum);
			List<TaskSubscription> listTaskSubs=TaskSubscriptions.GetTaskSubscriptionsForUser(_userNum);
			Assert.AreEqual(1,listTaskSubs.Count);
			Assert.IsFalse(listTaskSubs.Any(x => x.TaskListNum==taskListParent.TaskListNum));//not parent
			Assert.IsFalse(listTaskSubs.Any(x => x.TaskListNum==taskListChild.TaskListNum));//not child
			Assert.IsTrue(listTaskSubs.Any(x => x.TaskListNum==taskListGrandchild.TaskListNum));//grandchild
		}

		[TestMethod]
		public void TaskSubscriptions_TrySubscList_MarkOldRemindersRead() {
			TaskSubscriptionT.CreateTaskSubscription(_userNum,taskListChild.TaskListNum);//Subscribed to TaskListChild (and by extension, TaskListGrandchild).
			#region Create Unread Past Due Reminders
			OpenDentBusiness.Task taskParent=TaskT.CreateTask(taskListParent.TaskListNum,descript:"ParentReminder",isUnread:true,reminderGroupId:"1"
				,dateTimeEntry:DateTime.Now.AddSeconds(-1),reminderType:TaskReminderType.Once);
			OpenDentBusiness.Task taskChild=TaskT.CreateTask(taskListChild.TaskListNum,descript:"ChildReminder",isUnread:true,reminderGroupId:"1"
				,dateTimeEntry:DateTime.Now.AddSeconds(-1),reminderType:TaskReminderType.Once);
			OpenDentBusiness.Task taskGrandchild=TaskT.CreateTask(taskListGrandchild.TaskListNum,descript:"GrandchildReminder",isUnread:true
				,dateTimeEntry:DateTime.Now.AddSeconds(-1),reminderGroupId:"1",reminderType:TaskReminderType.Once);
			TaskAncestors.SynchAll();
			TaskUnreads.SetUnread(_userNum,taskParent);
			TaskUnreads.SetUnread(_userNum,taskChild);
			TaskUnreads.SetUnread(_userNum,taskGrandchild);
			#endregion 
			bool isSuccess=TaskSubscriptions.TrySubscList(taskListParent.TaskListNum,_userNum);
			OpenDentBusiness.Task taskParentDb=Tasks.GetOne(taskParent.TaskNum);
			OpenDentBusiness.Task taskChildDb=Tasks.GetOne(taskChild.TaskNum);
			OpenDentBusiness.Task taskGrandchildDb=Tasks.GetOne(taskGrandchild.TaskNum);
			Assert.IsTrue(isSuccess);
			Assert.IsFalse(TaskUnreads.IsUnread(_userNum,taskParentDb));//Only the task in taskListParent should be Read.
			Assert.IsTrue(TaskUnreads.IsUnread(_userNum,taskChildDb));//The task in taskListChild should still be Unread.
			Assert.IsTrue(TaskUnreads.IsUnread(_userNum,taskGrandchildDb));//The task in taskListGrandchild should still be Unread.
		}

	}
}
