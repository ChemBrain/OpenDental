using DataConnectionBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;

namespace UnitTests.SIn_Tests {
	[TestClass]
	public class SInTests:TestBase {

		[TestMethod]
		public void SInTests_SInEnum() {
			ProcStat procStatus=ProcStat.EC;
			int procStatusInt=(int)procStatus;
			Assert.AreEqual(procStatus,SIn.Enum<ProcStat>(procStatusInt));
		}

		[TestMethod]
		public void SInTests_SInEnum_Flags() {
			CrudSpecialColType specialType=CrudSpecialColType.CleanText | CrudSpecialColType.EnumAsString;
			int specialTypeInt=(int)specialType;
			Assert.AreEqual(specialType,SIn.Enum<CrudSpecialColType>(specialTypeInt));
		}

		[TestMethod]
		public void SInTests_SInEnum_FlagsDefault() {
			CrudSpecialColType specialType=CrudSpecialColType.None;
			int specialTypeInt=(int)specialType;
			Assert.AreEqual(specialType,SIn.Enum(specialTypeInt,defaultEnumOption:CrudSpecialColType.TextIsClob));
		}

		[TestMethod]
		public void SInTests_SInEnum_FlagsNoMatch() {
			int specialTypeInt=int.MaxValue;
			Assert.AreEqual(CrudSpecialColType.None,SIn.Enum(specialTypeInt,defaultEnumOption: CrudSpecialColType.None));
		}

		[TestMethod]
		public void SInTests_SInEnum_AsString() {
			PatientStatus patStat=PatientStatus.Inactive;
			string patStatStr=patStat.ToString();
			Assert.AreEqual(patStat,SIn.Enum<PatientStatus>(patStatStr,isEnumAsString:true));
		}

		[TestMethod]
		public void SInTests_SInEnum_IntAsString() {
			PatientStatus patStat=PatientStatus.Inactive;
			string patStatIntStr=((int)patStat).ToString();
			Assert.AreEqual(patStat,SIn.Enum<PatientStatus>(patStatIntStr,isEnumAsString:false));
		}

		[TestMethod]
		public void SInTests_SInEnum_IntNoMatch() {
			int patStatInt=(int)PatientStatus.Inactive+(int)PatientStatus.Prospective;
			Assert.AreEqual(PatientStatus.Patient,SIn.Enum(patStatInt,defaultEnumOption: PatientStatus.Patient));
		}
		
	}
}
