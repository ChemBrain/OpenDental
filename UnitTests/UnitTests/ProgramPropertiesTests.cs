using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests.ProgramProperties_Tests {
	[TestClass]
	public class ProgramPropertiess:TestBase {
		private static List<ProgramProperty> _listProgramProperties=new List<ProgramProperty>();

		///<summary>This method will execute only once, just before any tests in this class run.</summary>
		[ClassInitialize]
		public static void SetupClass(TestContext testContext) {
			//There are many program properties that are expected to be within the database.
			//Make a deep copy of the entire program property cache and keep it around so that we can reinsert them after these tests run.
			_listProgramProperties=ProgramProperties.GetWhere(x => true);
		}

		///<summary>This method will execute just before each test in this class.</summary>
		[TestInitialize]
		public void SetupTest() {
			ProgramPropertyT.ClearProgamPropertyTable();
		}

		///<summary>This method will execute after each test in this class.</summary>
		[TestCleanup]
		public void TearDownTest() {
			ProgramPropertyT.ClearProgamPropertyTable();
			ProgramProperties.InsertMany(_listProgramProperties);
			ProgramProperties.RefreshCache();
		}

		///<summary>This method will execute only once, just after all tests in this class have run.</summary>
		[ClassCleanup]
		public static void TearDownClass() {
		}

		[TestMethod]
		///<summary>Tests the Delete method in ProgramProperties to ensure it will delete when the PropertyDesc is one of those in 
		///the GetDeletablePropertyDescriptions() list, in this case ProgramProperties.PropertyDescs.ClinicHideButton.</summary>
		public void ProgramProperties_Delete_DeletesWhenDescriptionInGetDeletablePropertyDescriptions() {
			ProgramProperty prop=ProgramPropertyT.CreateProgramProperty(10,ProgramProperties.PropertyDescs.ClinicHideButton,1);
			ProgramProperty getPropBefore=ProgramProperties.GetPropForProgByDesc(prop.ProgramNum,prop.PropertyDesc);
			Assert.IsNotNull(getPropBefore);
			Assert.AreEqual(prop.ProgramPropertyNum,getPropBefore.ProgramPropertyNum);
			try {
				ProgramProperties.Delete(prop);
			}
			catch (Exception ex) {
				ex.DoNothing();
			}
			ProgramProperties.RefreshCache();//Make sure data is as current as it can be.
			//Ensure it was deleted.
			ProgramProperty getPropAfter=ProgramProperties.GetPropForProgByDesc(prop.ProgramNum,prop.PropertyDesc);
			Assert.IsNull(getPropAfter);//No longer in DB
		}

		[TestMethod]
		///<summary>Tests the Delete method in ProgramProperties to ensure it does not delete a ProgramProperty when the PropertyDesc is outside of those
		///in GetDeletablePropertyDescriptions(), instead it throws an exception (which we catch here) and then remains in the db.</summary>
		public void ProgramProperties_Delete_DoesNotDeleteWhenDescriptionIsNotInGetDeletablePropertyDescriptions() {
			ProgramProperty prop=ProgramPropertyT.CreateProgramProperty(20,"Stuff",2);
			ProgramProperty getPropBefore=ProgramProperties.GetPropForProgByDesc(prop.ProgramNum,prop.PropertyDesc);
			Assert.IsNotNull(getPropBefore);
			Assert.AreEqual(prop.ProgramPropertyNum,getPropBefore.ProgramPropertyNum);
			try {
				ProgramProperties.Delete(prop);
			}
			catch (Exception ex) {
				ex.DoNothing();
			}
			ProgramProperties.RefreshCache();//Make sure data is as current as it can be.
			//Ensure it was NOT deleted since the description is most certainly not in GetDeletablePropertyDescriptions().
			ProgramProperty getPropAfter=ProgramProperties.GetPropForProgByDesc(prop.ProgramNum,prop.PropertyDesc);
			Assert.IsNotNull(getPropAfter);//Still in DB
			Assert.AreEqual(prop.ProgramPropertyNum,getPropAfter.ProgramPropertyNum);//And we know its the ProgramProperty we put in.
		}

	}
}
