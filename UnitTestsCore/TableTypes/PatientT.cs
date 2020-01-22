using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class PatientT {
		///<summary>Creates a patient.  Practice default provider and billing type.</summary>
		public static Patient CreatePatient(string suffix="",long priProvNum=0,long clinicNum=0,string email="",string phone="",
			ContactMethod contactMethod=ContactMethod.Email,string lName="",string fName="",string preferredName="",DateTime birthDate=default(DateTime)
			,long secProvNum=0,long guarantor=0,bool setPortalAccessInfo=false,PatientStatus patStatus=PatientStatus.Patient,bool hasSignedTil=false) 
		{
			Patient pat=new Patient {
				Email=email,
				PreferConfirmMethod=contactMethod,
				PreferContactConfidential=contactMethod,
				PreferContactMethod=contactMethod,
				PreferRecallMethod=contactMethod,
				HmPhone=phone,
				WirelessPhone=phone,
				IsNew=true,
				LName=lName+suffix,
				FName=fName+suffix,
				BillingType=PrefC.GetLong(PrefName.PracticeDefaultBillType),
				ClinicNum=clinicNum,
				Preferred=preferredName,
				Birthdate=birthDate,
				SecProv=secProvNum,
				PatStatus=patStatus,
				HasSignedTil=hasSignedTil,
			};
			if(priProvNum!=0) {
				pat.PriProv=priProvNum;
			}
			else {
				long practiceProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
				//Check the provider cache to see if the ProvNum set for the PracticeDefaultProv pref actually exists.
				if(!Providers.GetExists(x => x.ProvNum==practiceProvNum)) {
					practiceProvNum=Providers.GetFirst().ProvNum;
					//Update the preference in the database NOT the unit test preference cache to a valid provider if the current pref value is invalid.
					Prefs.UpdateLong(PrefName.PracticeDefaultProv,practiceProvNum);
				}
				pat.PriProv=practiceProvNum;
			}
			if(setPortalAccessInfo) {
				pat.Address="666 Church St NE";
				pat.City="Salem";
				pat.State="OR";
				pat.Zip="97301";
				if(pat.Birthdate.Year<1880) {
					pat.Birthdate=new DateTime(1970,1,1);
				}
				if(string.IsNullOrEmpty(pat.WirelessPhone)) {
					pat.WirelessPhone="5555555555";
				}
			}
			Patients.Insert(pat,false);
			Patient oldPatient=pat.Copy();
			pat.Guarantor=pat.PatNum;
			if(guarantor > 0) {
				pat.Guarantor=guarantor;
			}
			if(lName=="") {
				pat.LName=pat.PatNum.ToString()+"L";
			}
			if(fName=="") {
				pat.FName=pat.PatNum.ToString()+"F";
			}
			Patients.Update(pat,oldPatient);
			return pat;
		}

		public static void SetGuarantor(Patient pat,long guarantorNum){
			Patient oldPatient=pat.Copy();
			pat.Guarantor=guarantorNum;
			Patients.Update(pat,oldPatient);
		}
		
		///<summary>Deletes everything from the patient table.  Does not truncate the table so that PKs are not reused on accident.</summary>
		public static void ClearPatientTable() {
			string command="DELETE FROM patient WHERE PatNum > 0";
			DataCore.NonQ(command);
		}

		/// <summary>Creates patient objects corresponding to the totalPat parameter. Each patient has a procedure
		/// and statement created on the specified date. Aging is run for each patient.</summary>
		public static void CreatePatWithProcAndStatement(int totalPat, DateTime dateTimeSentStmt=default(DateTime),bool hasPortalAccessInfo=false,
			PatientStatus patStatus=PatientStatus.Patient,StatementMode stmtMode=StatementMode.Mail,bool hasSignedTil=false,double procFee=0) {
			for(int i=0;i<totalPat;i++) {
				Patient patient=CreatePatient("",0,0,"","",ContactMethod.Email,"","","",default(DateTime),0,0,hasPortalAccessInfo,patStatus,hasSignedTil);
				DateTime dateProc=DateTime.Today.AddDays(-1);
				//Create a completed procedure that was completed the day before the first payplan charge date AND before the payment plan creation date.
				ProcedureT.CreateProcedure(patient,"D1100",ProcStat.C,"",procFee,dateProc);
				//Run Ledgers to update the patient balance from the procedure fee
				Ledgers.ComputeAging(patient.PatNum,dateTimeSentStmt);
				//Insert a statement that was sent during the "bill in advance days" for the payment plan charge AND before the payment plan creation date.
				StatementT.CreateStatement(patient.PatNum,mode_: stmtMode,isSent: true,dateSent: dateTimeSentStmt);
			}
		}

	}
}
