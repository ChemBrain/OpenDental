using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests.Userods_Tests {
	[TestClass]
	public class UserodsTests:TestBase {
		///<summary>This method will get invoked after every single test.</summary>
		[TestCleanup]
		public void Cleanup() {
			CredentialsFailedAfterLoginEvent.Fired-=CredentialsFailedAfterLoginEvent_Fired1;
			RemotingClient.HasLoginFailed=false;
			RevertMiddleTierSettingsIfNeeded();
			Security.CurUser=Userods.GetFirstOrDefault(x => x.UserName==UnitTestUserName);
			Security.PasswordTyped=UnitTestPassword;
		}

		[TestMethod]
		public void Userods_CheckUserAndPassword_IncreaseFailedAttemptsAfterUserHasLoggedInButPasswordIsNotCorrect() {
			//First, setup the test scenario.
			//This test is intended to be tested on middle tier. 
			long group1=UserGroupT.CreateUserGroup("usergroup1");
			Userod myUser=UserodT.CreateUser(MethodBase.GetCurrentMethod().Name+DateTime.Now.Ticks,"reallystrongpassword",userGroupNumbers:new List<long>() {group1 });
			RunTestsAgainstMiddleTier(new OpenDentBusiness.WebServices.OpenDentalServerMockIIS(user:myUser.UserName,password:myUser.Password));
			Security.CurUser=myUser;
			Security.PasswordTyped="passwordguess#1";
			CredentialsFailedAfterLoginEvent.Fired+=CredentialsFailedAfterLoginEvent_Fired1;
			//make a single bad password attempt.
			ODException.SwallowAnyException(() => {
				Userods.CheckUserAndPassword(myUser.UserName,"passwordguess#1",false);
			});
			//Get our user from the DB
			RunTestsAgainstDirectConnection();
			myUser=Userods.GetUserByNameNoCache(myUser.UserName);
			//Asssert that the failed attempt got incremented correctly. 
			Assert.AreEqual(1,myUser.FailedAttempts);
		}

		private void CredentialsFailedAfterLoginEvent_Fired1(ODEventArgs e) {
			//If we don't subscribe to this event then the failed event will keep firing over and over.
			RemotingClient.HasLoginFailed=true;
			throw new Exception("Incorrect username and password");
		}

		//Test #2 Attempt 6 times in total and validate that count increases to 5 and then get message that account has been locked. 
		[TestMethod]
		public void Userods_CheckUserAndPassword_LockoutAfterUserHasLoggedInButPasswordIsNotCorrectAfter5Attempts() {
			//First, setup the test scenario.
			long group1=UserGroupT.CreateUserGroup("usergroup1");
			bool isAccountLocked=false;
			Userod myUser=UserodT.CreateUser(MethodBase.GetCurrentMethod().Name+DateTime.Now.Ticks,"reallystrongpassword",userGroupNumbers:new List<long>() {group1 });
			//Make 5 bad password attempts
			for(int i = 1 ;i < 6;i++) {
				ODException.SwallowAnyException(() => {
					Userods.CheckUserAndPassword(myUser.UserName,"passwordguess#"+i,false);
				});
			}
			try {
				//the 6th bad attempt should kick us with a message saying that our account has been locked.
				Userods.CheckUserAndPassword(myUser.UserName,"passwordguess#6",false);
			}
			catch (Exception e) {
				if(e.Message.Contains("Account has been locked due to failed log in attempts")) {
					isAccountLocked=true;
				}
			}
			//Get our updated user from the DB. 
			myUser=Userods.GetUserByNameNoCache(myUser.UserName);
			//Assert that we got to 5 failed attempts and that the account has been locked. 
			Assert.AreEqual(5,myUser.FailedAttempts);
			Assert.AreEqual(true,isAccountLocked);
		}

		//Test #3 Middle tier specific. Try with wrong password, try different method that calls CheckUSersAndPassword but so that we don't do it directly.
		//Make sure that we don't get to the lockout winodw, or query the DB to make sure failed attempts doesn't get past 1. 
		[TestMethod]
		public void Userods_CheckUserAndPassoword_UpdateFailedAttemptsFromOtherMethods() {
			//First, setup the test scenario.
			long group1=UserGroupT.CreateUserGroup("usergroup1");
			Userod myUser=UserodT.CreateUser(MethodBase.GetCurrentMethod().Name+DateTime.Now.Ticks,"reallystrongpassword",userGroupNumbers:new List<long>() {group1 });
			Security.CurUser=myUser;
			Security.PasswordTyped="passwordguess#1";
			CredentialsFailedAfterLoginEvent.Fired+=CredentialsFailedAfterLoginEvent_Fired1;
			RunTestsAgainstMiddleTier(new OpenDentBusiness.WebServices.OpenDentalServerMockIIS(user:myUser.UserName,password:myUser.Password));
			//try once with the wrong password. Failed attempt should get incremented to 1. 
			ODException.SwallowAnyException(() => {
				Userods.CheckUserAndPassword(myUser.UserName,"passwordguess#1",false);
			});
			//Get our updated user from the DB.
			RunTestsAgainstDirectConnection(); 
			myUser=Userods.GetUserByNameNoCache(myUser.UserName);
			//Assert that we only have 1 failed attempt. 
			Assert.AreEqual(1,myUser.FailedAttempts);
			//now wait for another method to get called
			RunTestsAgainstMiddleTier(new OpenDentBusiness.WebServices.OpenDentalServerMockIIS(user:myUser.UserName,password:myUser.Password));
			ODException.SwallowAnyException(() => {
				Computers.UpdateHeartBeat(Environment.MachineName,false);
			});
			RunTestsAgainstDirectConnection();
			//Get our updated user from the DB. 
			myUser=Userods.GetUserByNameNoCache(myUser.UserName);
			//Assert that we only have 1 failed attempt. 
			Assert.AreEqual(1,myUser.FailedAttempts);
		}

		//Test #4 Similar to #3, call method checkusersandpassword specifically over middle tier assert that failed attempts do get increased to 5. 
		[TestMethod]
		public void Userods_CheckUserAndPassoword_UpdateFailedAttemptsTo5() {
			//First, setup the test scenario.
			long group1=UserGroupT.CreateUserGroup("usergroup1");
			Userod myUser=UserodT.CreateUser(MethodBase.GetCurrentMethod().Name+DateTime.Now.Ticks,"reallystrongpassword",userGroupNumbers:new List<long>() {group1 });
			CredentialsFailedAfterLoginEvent.Fired+=CredentialsFailedAfterLoginEvent_Fired1;
			Security.CurUser=myUser;
			Security.PasswordTyped="passwordguess#1";
			RunTestsAgainstMiddleTier();
			//try with 5 incorrect passwords. Failed attempt should get incremented to 5. 
			for(int i = 1 ;i < 6;i++) {
				ODException.SwallowAnyException(() => {
					try {
						Userods.CheckUserAndPassword(myUser.UserName,"passwordguess#"+i,false);
					}
					catch(Exception e) {

					}
					
				});
			}
			//Get our updated user from the DB. 
			RunTestsAgainstDirectConnection();
			myUser=Userods.GetUserByNameNoCache(myUser.UserName);
			//Assert that there are 5 failed attempts. 
			Assert.AreEqual(5,myUser.FailedAttempts);
		}
		
	}
}
