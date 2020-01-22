using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using CodeBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests {
	[TestClass]
	public class PDMPTests:TestBase {
		[ClassInitialize]
		public static void SetupClass(TestContext testContext) {
			IntrospectionT.UpsertPref(new Dictionary<Introspection.IntrospectionEntity,string>() {
				{ Introspection.IntrospectionEntity.PDMPURL,"https://openid.logicoy.com/ilpdmp/test/getReport" },
			});
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
		public void PDMPLogicoy_GetURL_TestData() {
			long provNum=ProviderT.CreateProvider("provPDMP",fName:"Barry",lName:"Anderson");
			Provider prov=Providers.GetProv(provNum);
			prov.NationalProvID="2621233875";
			Patient pat=PatientT.CreatePatient(fName:"Bilbo",lName:"Baggins",birthDate:new DateTime(1959,12,26),priProvNum: provNum);
			string testUsername="guest";
			string testPassword="welcome123";
			string stateWhereLicensed="IL";
			string strDeaNum="BR3932553";
			string facilityId="RVC";
			PDMPLogicoy pdmp=new PDMPLogicoy(testUsername,testPassword,pat,prov,stateWhereLicensed,strDeaNum,facilityId);
			string urlActual=pdmp.GetURL();
			string urlUniqueID=".*";//Ex: "68fa8c70-d253-417b-a438-13909ee2134d&amp;"
			string urlExpected="https://openid\\.logicoy\\.com/pdmp/dataServices/getReport/fetchSummaryReport\\?SSOToken="+urlUniqueID+"stateCode=IL";
			Assert.IsTrue(Regex.IsMatch(urlActual,urlExpected));
		}

	}
}
