using System;
using System.Collections.Generic;
using System.Linq;
using OpenDentBusiness;
using UnitTestsCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using CodeBase;
using System.Data;
using System.Collections;
using OpenDentBusiness.WebTypes.WebSched.TimeSlot;

namespace UnitTests.Appointments_Tests {
	//All tests are assumed to be non dynamic unless specified. 
	[TestClass]
	public class ApptReminderRuleTests:TestBase {

		///<summary>All ShortCodeTypeFlags must use ShortCodeAttribute with ApptReminderType set.</summary>
		[TestMethod]
		public void ApptReminderRules_ShortCodeTypeFlags_ApptReminderTypes() {
			foreach(ShortCodeTypeFlag flag in Enum.GetValues(typeof(ShortCodeTypeFlag)).AsEnumerable<ShortCodeTypeFlag>()
				.Where(x => x!=ShortCodeTypeFlag.None)) 
			{
				Assert.IsTrue(flag.GetAttributeOrDefault<ShortCodeAttribute>().ApptReminderType!=ApptReminderType.Undefined);
			}
		}

		///<summary>All ShortCodeTypeFlags must use ShortCodeAttribute with SmsMessageSource set.</summary>
		[TestMethod]
		public void ApptReminderRules_ShortCodeTypeFlags_SmsMessageSources() {
			foreach(ShortCodeTypeFlag flag in Enum.GetValues(typeof(ShortCodeTypeFlag)).AsEnumerable<ShortCodeTypeFlag>()
				.Where(x => x!=ShortCodeTypeFlag.None)) 
			{
				Assert.IsFalse(flag.GetAttributeOrDefault<ShortCodeAttribute>().SmsMessageSource.IsNullOrEmpty());
			}
		}

		/// <summary>Ensures the correct Short Code setting for all ApptReminderTypes supported by ShortCodeTypeFlag is returned by 
		/// ApptReminderRules.IsShortCode()
		/// Truth Table:
		/// Default Clinic | Clinic | IsShortCode()
		/// 0 | 0 | 0
		/// 0 | 1 | 1
		/// 1 | 0 | 1
		/// 1 | 1 | 1
		/// </summary>
		[TestMethod]
		public void ApptReminderRules_IsShortCode_DefaultsAndClinicOverrides() {
			long clinicNum=1;
			foreach(ShortCodeTypeFlag flag in Enum.GetValues(typeof(ShortCodeTypeFlag)).AsEnumerable<ShortCodeTypeFlag>()
				.Where(x => x!=ShortCodeTypeFlag.None)) 
			{
				ApptReminderType type=flag.GetAttributeOrDefault<ShortCodeAttribute>().ApptReminderType;
				//Set default clinic OFF for Short Codes for the corresponding ApptReminderType.
				ClinicPrefs.Upsert(PrefName.ShortCodeApptReminderTypes,0,POut.Long(0));
				//Set clinicA OFF for Short Codes for the corresponding ApptReminderType.
				ClinicPrefs.Upsert(PrefName.ShortCodeApptReminderTypes,clinicNum,POut.Long(0));
				ClinicPrefs.RefreshCache();
				Assert.IsFalse(ApptReminderRules.IsShortCode(type,clinicNum));
				//Set clinicA ON for Short Codes for the corresponding ApptReminderType.
				ClinicPrefs.Upsert(PrefName.ShortCodeApptReminderTypes,clinicNum,POut.Long((long)flag));
				ClinicPrefs.RefreshCache();
				Assert.IsTrue(ApptReminderRules.IsShortCode(type,clinicNum));

				//Set default clinic ON for Short Codes for the corresponding ApptReminderType.
				ClinicPrefs.Upsert(PrefName.ShortCodeApptReminderTypes,0,POut.Long((long)flag));
				//Set clinicA OFF for Short Codes for the corresponding ApptReminderType.
				ClinicPrefs.Upsert(PrefName.ShortCodeApptReminderTypes,clinicNum,POut.Long(0));
				ClinicPrefs.RefreshCache();
				Assert.IsTrue(ApptReminderRules.IsShortCode(type,clinicNum));
				//Set clinicA ON for Short Codes for the corresponding ApptReminderType.
				ClinicPrefs.Upsert(PrefName.ShortCodeApptReminderTypes,clinicNum,POut.Long((long)flag));
				ClinicPrefs.RefreshCache();
				Assert.IsTrue(ApptReminderRules.IsShortCode(type,clinicNum));
			}
		}
	}
}
