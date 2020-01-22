using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests.Patients_Tests {
	[TestClass]
	public class PatientsTests:TestBase {

		[TestInitialize] 
		public void RunBeforeEachTest() {
			PatientT.ClearPatientTable();
		}

		#region GetPatientsByPartialName

		[TestMethod]
		public void Patients_GetPatientsByPartialName_LastAndFirst() {
			PatientT.CreatePatient(lName: "Owre",fName: "Sam");
			PatientT.CreatePatient(lName: "Owre",fName: "Sarah");
			List<Patient> listPats=Patients.GetPatientsByPartialName("sam owre");
			Assert.AreEqual(1,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_MatchTwoLastAndFirst() {
			PatientT.CreatePatient(lName: "OWRE",fName: "sam");
			PatientT.CreatePatient(lName: "Owre",fName: "sarah");
			List<Patient> listPats=Patients.GetPatientsByPartialName("owre s");
			Assert.AreEqual(2,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_JustFirst() {
			PatientT.CreatePatient(lName: "Owre",fName: "Sam");
			PatientT.CreatePatient(lName: "Owre",fName: "Sarah");
			List<Patient> listPats=Patients.GetPatientsByPartialName("SaRah");
			Assert.AreEqual(1,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_JustLast() {
			PatientT.CreatePatient(lName: "Owre",fName: "Sam");
			PatientT.CreatePatient(lName: "Owre",fName: "Sarah");
			List<Patient> listPats=Patients.GetPatientsByPartialName("OWRE");
			Assert.AreEqual(2,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_FirstNotPreferred() {
			PatientT.CreatePatient(lName: "Brock",fName: "Taylor");
			PatientT.CreatePatient(lName: "McGehee",fName: "Christopher",preferredName: "Chris");
			List<Patient> listPats=Patients.GetPatientsByPartialName("chris owre");
			Assert.AreEqual(0,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_LastAndPreferred() {
			PatientT.CreatePatient(lName: "Jansen",fName: "Andrew");
			PatientT.CreatePatient(lName: "Montano",fName: "Joseph",preferredName: "Joe");
			List<Patient> listPats=Patients.GetPatientsByPartialName("Joe Montano");
			Assert.AreEqual(1,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_TwoWordLastName() {
			PatientT.CreatePatient(lName: "Owre",fName: "Sam");
			PatientT.CreatePatient(lName: "Van Damme",fName: "Jean-Claude");
			List<Patient> listPats=Patients.GetPatientsByPartialName("van damme");
			Assert.AreEqual(1,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_TwoWordLastNamePlusFirst() {
			PatientT.CreatePatient(lName: "Owre",fName: "Sam");
			PatientT.CreatePatient(lName: "Van Damme",fName: "Jean-Claude");
			List<Patient> listPats=Patients.GetPatientsByPartialName("sam van damme");
			Assert.AreEqual(0,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_LotsOfNames() {
			PatientT.CreatePatient(lName: "Salmon",fName: "Jason");
			PatientT.CreatePatient(lName: "Jansen",fName: "Andrew");
			List<Patient> listPats=Patients.GetPatientsByPartialName("andrew jansen thinks programming is fun");
			Assert.AreEqual(0,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_SameNames() {
			PatientT.CreatePatient(lName: "Owre",fName: "Sam");
			PatientT.CreatePatient(lName: "Owre",fName: "Sarah");
			List<Patient> listPats=Patients.GetPatientsByPartialName("sam sam");
			Assert.AreEqual(1,listPats.Count);
		}


		[TestMethod]
		public void Patients_GetPatientsByPartialName_AllCaps() {
			PatientT.CreatePatient(lName: "Buchanan",fName: "Cameron");
			PatientT.CreatePatient(lName: "Montano",fName: "Joseph");
			List<Patient> listPats=Patients.GetPatientsByPartialName("CAMERON BUCHANAN");
			Assert.AreEqual(1,listPats.Count);
		}


		[TestMethod]
		public void Patients_GetPatientsByPartialName_Everybody() {
			PatientT.CreatePatient(lName: "Buchanan",fName: "Cameron");
			PatientT.CreatePatient(lName: "Montano",fName: "Joseph");
			PatientT.CreatePatient(lName: "Owre",fName: "Sam");
			PatientT.CreatePatient(lName: "Owre",fName: "Sarah");
			PatientT.CreatePatient(lName: "Salmon",fName: "Jason");
			PatientT.CreatePatient(lName: "Jansen",fName: "Andrew");
			PatientT.CreatePatient(lName: "Van Damme",fName: "Jean-Claude");
			PatientT.CreatePatient(lName: "Brock",fName: "Taylor");
			PatientT.CreatePatient(lName: "McGehee",fName: "Christopher",preferredName: "Chris");
			List<Patient> listPats=Patients.GetPatientsByPartialName("");
			Assert.AreEqual(9,listPats.Count);
		}
		#endregion GetPatientsByPartialName

		#region GetPatNumsByNameBirthdayEmailAndPhone

		[TestMethod]
		public void Patients_GetPatNumsByNameBirthdayEmailAndPhone_SingleEmailMatch() {
			Patient patA,patB,patA2;
			GenerateThreePatients(out patA,out patB,out patA2);
			//Match using email on family member's account
			List<long> listMatchingPatNums=GetPatNumsByNameBirthdayEmailAndPhone(patA.LName,patA.FName,patA.Birthdate,patB.Email,
				new List<string>());
			Assert.AreEqual(1,listMatchingPatNums.Count);
			Assert.IsTrue(listMatchingPatNums.Contains(patA.PatNum));
		}
		
		[TestMethod]
		public void Patients_GetPatNumsByNameBirthdayEmailAndPhone_SameNameAndBirthdate() {
			Patient patA,patB,patA2;
			GenerateThreePatients(out patA,out patB,out patA2);
			//Match two patients based on name and birthdate
			List<long> listMatchingPatNums=GetPatNumsByNameBirthdayEmailAndPhone(patA.LName,patA.FName,patA.Birthdate,"",new List<string>());
			Assert.AreEqual(2,listMatchingPatNums.Count);
			Assert.IsTrue(listMatchingPatNums.Contains(patA.PatNum));
			Assert.IsTrue(listMatchingPatNums.Contains(patA2.PatNum));
		}
		
		[TestMethod]
		public void Patients_GetPatNumsByNameBirthdayEmailAndPhone_SameEmail() {
			Patient patA,patB,patA2;
			GenerateThreePatients(out patA,out patB,out patA2);
			//Match two patients based on email
			List<long> listMatchingPatNums=GetPatNumsByNameBirthdayEmailAndPhone(patA.LName,patA.FName,patA.Birthdate,patA.Email,new List<string>());
			Assert.AreEqual(2,listMatchingPatNums.Count);
			Assert.IsTrue(listMatchingPatNums.Contains(patA.PatNum));
			Assert.IsTrue(listMatchingPatNums.Contains(patA2.PatNum));
		}
		
		[TestMethod]
		public void Patients_GetPatNumsByNameBirthdayEmailAndPhone_SamePhoneNumbers() {
			Patient patA,patB,patA2;
			GenerateThreePatients(out patA,out patB,out patA2);
			//Match two patients based on phone numbers
			List<long> listMatchingPatNums=GetPatNumsByNameBirthdayEmailAndPhone(patA.LName,patA.FName,patA.Birthdate,"",
				new List<string> { patB.HmPhone,patA2.WirelessPhone });
			Assert.AreEqual(2,listMatchingPatNums.Count);
			Assert.IsTrue(listMatchingPatNums.Contains(patA.PatNum));
			Assert.IsTrue(listMatchingPatNums.Contains(patA2.PatNum));
		}
		
		[TestMethod]
		public void Patients_GetPatNumsByNameBirthdayEmailAndPhone_SinglePhoneNumber() {
			Patient patA,patB,patA2;
			GenerateThreePatients(out patA,out patB,out patA2);
			//Match one patient based on phone numbers
			List<long> listMatchingPatNums=GetPatNumsByNameBirthdayEmailAndPhone(patA.LName,patA.FName,patA.Birthdate,"",
				new List<string> { patA2.WirelessPhone });
			Assert.AreEqual(1,listMatchingPatNums.Count);
			Assert.IsTrue(listMatchingPatNums.Contains(patA2.PatNum));
		}
		
		[TestMethod]
		public void Patients_GetPatNumsByNameBirthdayEmailAndPhone_SingleNameAndBirthdate() {
			Patient patA,patB,patA2;
			GenerateThreePatients(out patA,out patB,out patA2);
			//Match one patient based on name and birthdate
			List<long> listMatchingPatNums=GetPatNumsByNameBirthdayEmailAndPhone(patB.LName,patB.FName,patB.Birthdate,"",new List<string>());
			Assert.AreEqual(1,listMatchingPatNums.Count);
			Assert.IsTrue(listMatchingPatNums.Contains(patB.PatNum));
		}

		#endregion

		#region HelperMethods

		///<summary>Calls the Patient S class method.</summary>
		private List<long> GetPatNumsByNameBirthdayEmailAndPhone(string lName,string fName,DateTime birthDate,string email,List<string> listPhones) {
			return Patients.GetPatNumsByNameBirthdayEmailAndPhone(lName,fName,birthDate,email,listPhones);
		}

		///<summary>This method generates three patients and returns them as out variables. The patients have overlapping names, birthdates, emails, 
		///and phones.</summary>
		private void GenerateThreePatients(out Patient patA,out Patient patB,out Patient patA2) {
			patA=PatientT.CreatePatient(fName:"Billy",lName:"Bob");
			Patient oldPat=patA.Copy();
			patA.Birthdate=new DateTime(1971,6,28);
			patA.WirelessPhone="5413635432";
			patA.Email="joe@schmoe.com";
			patA.ClinicNum=1;
			Patients.Update(patA,oldPat);
			patB=PatientT.CreatePatient(fName:"Joe",lName:"Schmoe");
			oldPat=patB.Copy();
			patB.Birthdate=new DateTime(2000,6,28);
			patB.HmPhone="5413631111";
			patB.Email="chuck@schmuck.com";
			patB.Guarantor=patA.PatNum;
			patB.ClinicNum=1;
			Patients.Update(patB,oldPat);
			patA2=PatientT.CreatePatient(fName:"Billy",lName:"Bob");
			oldPat=patA2.Copy();
			patA2.Birthdate=new DateTime(1971,6,28);
			patA2.WirelessPhone="5478982525";
			patA2.Email=patA.Email;
			patA2.ClinicNum=0;
			Patients.Update(patA2,oldPat);
		}

		#endregion

	}
}
