using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests.AlertItems_Tests {
	[TestClass]
	public class AlertItemsTests:TestBase {
		[ClassInitialize]
		public static void SetupClass(TestContext testContext) {
			//Add anything here that you want to run once before the tests in this class run.
		}

		[TestInitialize]
		public void SetupTest() {
			//Clear out the alertitem table before every test.
			AlertItemT.ClearAlertItemTable();
		}
		
		public void AlertItems_CreateAlertsForWebmailMethodCall() {
			AlertItems.CreateAlertsForNewWebmail(_log);
		}

		/// <summary></summary>
		[TestMethod]
		public void AlertItems_CreateAlertsForNewWebmail() {
			//Test Sections:
			//Create 5 users, part of 2 providers.
			//Test adding an email for each provider, then clear alerts table.
			//Test adding 4 emails for each provider
			//Test adding 3 additional emails for 1 provider
			//Test marking 2 emails as read for 1 provider
			//Test marking all emails as read for 1 provider
			EmailMessageT.ClearEmailMessageTable();//Clear out the emailmessage table
			List<Userod> listTestUsers=new List<Userod>();
			//Create or reuse 5 users, and set their provnum to 1 or 2.  There'll be 3 provnum=1 and 2 provnum=2
			//In queries always filter by usernum because there may be users left over from other/old tests.
			for(int i=0;i<5;i++) { 
				Userod user=UserodT.CreateUser();
				user.ProvNum=i%2+1;
				listTestUsers.Add(user);
				Userods.Update(user);
			}
			listTestUsers=listTestUsers.Distinct().ToList();
			long examplePatnum=2;	//Patnum can be anything, needed for webmail.
			//Create one email for each provider.
			foreach(long provnum in listTestUsers.Select(x => x.ProvNum).Distinct()) {
				EmailMessageT.CreateWebMail(provnum,examplePatnum);
			}
			AlertItems_CreateAlertsForWebmailMethodCall();
			//Count the total # of alertitem entries, not what the description is.
			string alertCount=DataCore.GetScalar("SELECT COUNT(*) FROM alertitem WHERE UserNum IN ("+string.Join(",",listTestUsers.Select(x => POut.Long(x.UserNum)))
				+") AND Type="+POut.Int((int)AlertType.WebMailRecieved));
			Assert.AreEqual("5",alertCount);
			//
			//Clear out ALERT table and add some new emails
			AlertItemT.ClearAlertItemTable();
			foreach(long provnum in listTestUsers.Select(x => x.ProvNum).Distinct()) {
				EmailMessageT.CreateWebMail(provnum,examplePatnum);
				EmailMessageT.CreateWebMail(provnum,examplePatnum);
				EmailMessageT.CreateWebMail(provnum,examplePatnum);
				EmailMessageT.CreateWebMail(provnum,examplePatnum);
			}
			//This section tests adding more unread emails, and changing the description of the alertitem
			Userod selectedUser=listTestUsers.First();
			AlertItems_CreateAlertsForWebmailMethodCall();
			alertCount=DataCore.GetScalar("SELECT Description FROM alertitem WHERE Type="+POut.Int((int)AlertType.WebMailRecieved)+" AND UserNum="+selectedUser.UserNum);
			Assert.AreEqual("5",alertCount);
			//
			//Add 3 more unread emails.
			EmailMessageT.CreateWebMail(selectedUser.ProvNum,examplePatnum);
			EmailMessageT.CreateWebMail(selectedUser.ProvNum,examplePatnum);
			EmailMessageT.CreateWebMail(selectedUser.ProvNum,examplePatnum);
			AlertItems_CreateAlertsForWebmailMethodCall();
			alertCount=DataCore.GetScalar("SELECT Description FROM alertitem WHERE Type="+POut.Int((int)AlertType.WebMailRecieved)+" AND UserNum="+selectedUser.UserNum);
			Assert.AreEqual("8",alertCount);
			//
			//Mark 2 of the emails as read, to decrease the amount of unread emails
			string command="UPDATE emailmessage SET SentOrReceived="+POut.Int((int)EmailSentOrReceived.WebMailRecdRead)+
				" WHERE SentOrReceived="+POut.Int((int)EmailSentOrReceived.WebMailReceived)+" AND ProvNumWebMail="+POut.Long(selectedUser.ProvNum)+" LIMIT 2";
			DataCore.NonQ(command);
			AlertItems_CreateAlertsForWebmailMethodCall();
			alertCount=DataCore.GetScalar("SELECT Description FROM alertitem WHERE Type="+POut.Int((int)AlertType.WebMailRecieved)+" AND UserNum="+selectedUser.UserNum);
			Assert.AreEqual("6",alertCount);
			//
			//Now we mark all of this user's emails as read, as if that user has read all of their webmail.
			command="UPDATE emailmessage SET SentOrReceived="+POut.Int((int)EmailSentOrReceived.WebMailRecdRead)+
				" WHERE SentOrReceived="+POut.Int((int)EmailSentOrReceived.WebMailReceived)+" AND ProvNumWebMail="+POut.Long(selectedUser.ProvNum);
			DataCore.NonQ(command);
			AlertItems_CreateAlertsForWebmailMethodCall();
			alertCount=DataCore.GetScalar("SELECT COUNT(*) FROM alertitem WHERE Type="+POut.Int((int)AlertType.WebMailRecieved)+" AND UserNum="+selectedUser.UserNum);
			Assert.AreEqual("0",alertCount);
		}
	}
}
