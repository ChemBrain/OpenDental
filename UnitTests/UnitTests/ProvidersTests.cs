using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests.Providers_Tests {
	[TestClass]
	public class ProvidersTests:TestBase {
		[ClassInitialize]
		public static void SetupClass(TestContext testContext) {
			//Add anything here that you want to run once before the tests in this class run.
		}

		[TestInitialize]
		public void SetupTest() {
			//Add anything here that you want to run before every test in this class.
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
		public void Providers_GetAll_BlankBirthdate() {
			long provNum=ProviderT.CreateProvider(MethodBase.GetCurrentMethod().Name);//New Provider with default Provider.Birthdate.
			List<Provider> listProviders=Providers.GetAll();
			Provider prov=listProviders.FirstOrDefault(x => x.ProvNum==provNum);
			Assert.AreEqual(DateTime.MinValue,prov.Birthdate);
		}

		[TestMethod]
		public void Providers_GetAll_BlankDateTerm() {
			long provNum=ProviderT.CreateProvider(MethodBase.GetCurrentMethod().Name);//New Provider with default Provider.Birthdate.
			List<Provider> listProviders=Providers.GetAll();
			Provider prov=listProviders.FirstOrDefault(x => x.ProvNum==provNum);
			Assert.AreEqual(DateTime.MinValue,prov.DateTerm);
		}

	}
}
