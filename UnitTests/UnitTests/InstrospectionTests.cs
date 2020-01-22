using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests.Instrospection_Tests {
	[TestClass]
	public class InstrospectionTests:TestBase {
		[ClassCleanup]
		public static void CleanupIntrospection() {
			IntrospectionT.DeletePref();
		}

		[TestMethod]
		public void Introspection_Preference_Missing() {
			IntrospectionT.DeletePref();
			string retVal=Introspection.GetOverride(Introspection.IntrospectionEntity.DentalXChangeDwsURL,"thisIsADefaultURL");
			//Because there is no preference in the DB we should get back the default value we passed in.
			Assert.AreEqual(retVal,"thisIsADefaultURL");
		}

		[TestMethod]
		public void Introspection_Preference_Malformed() {
			IntrospectionT.UpsertPref("INVALID JSON ValueString");
			string retVal="";
			try {
				retVal=Introspection.GetOverride(Introspection.IntrospectionEntity.DentalXChangeDwsURL,"thisIsADefaultURL");
			}
			catch(ApplicationException) {
				//GetOverride() should throw an application exception if the json is malformed and retVal should never be set.
				Assert.IsTrue(retVal=="");
				return;
			}
			Assert.Fail();
		}

		[TestMethod]
		public void Introspection_Preference_Present() {
			IntrospectionT.UpsertPref(new Dictionary<Introspection.IntrospectionEntity, string>() {
				{ Introspection.IntrospectionEntity.DentalXChangeDwsURL,"https://prelive2.dentalxchange.com/dws/DwsService" }
			});
			string retVal=Introspection.GetOverride(Introspection.IntrospectionEntity.DentalXChangeDwsURL,"thisIsADefaultURL");
			//The preference is present and valid. retVal should be overridden with the preference value.
			Assert.AreEqual("https://prelive2.dentalxchange.com/dws/DwsService",retVal);
		}
	}
}
