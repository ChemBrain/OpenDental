using CodeBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using OpenDentBusiness.UI;

namespace OpenDentBusiness {
	public class PDMP {

		///<summary>Returns true if we got a URL from PDMP.
		///If true, the response will be the URL.
		///If false, the response will be the error.</summary>
		public static bool TrySendData(Program programCur,Patient pat,out string response) {
			string result="";
			StringBuilder sbErrors=new StringBuilder();
			bool isSuccess=false;
			ODProgress.ShowAction(() => { 
				if(!programCur.Enabled) {
					sbErrors.AppendLine(programCur.ProgName+Lans.g("PDMP"," must be enabled in Program Links."));
					result=sbErrors.ToString();
					return;
				}
				if(pat==null) {
					sbErrors.AppendLine(Lans.g("PDMP","Please select a patient."));
					result=sbErrors.ToString();
					return;
				}
				Provider prov=Providers.GetProv(pat.PriProv);
				if(prov==null) {
					sbErrors.AppendLine(Lans.g("PDMP","Patient does not have a primary provider."));
					result=sbErrors.ToString();
					return;
				}
				string strDeaNum=ProviderClinics.GetDEANum(prov.ProvNum,Clinics.ClinicNum);//If no result found, retries using clinicNum=0.
				if(string.IsNullOrWhiteSpace(strDeaNum)) {
					sbErrors.AppendLine(Lans.g("PDMP","Patient's provider does not have a DEA number."));
				}
				string stateWhereLicensed=ProviderClinics.GetStateWhereLicensed(pat.PriProv,Clinics.ClinicNum);
				if(string.IsNullOrWhiteSpace(stateWhereLicensed)) {
					sbErrors.AppendLine(Lans.g("PDMP","Patient's provider is not licensed for any state."));
				}
				string facilityIdPropDesc="";
				string userNamePropDesc="";
				string passwordPropDesc="";
				try {
					facilityIdPropDesc=GetFacilityIdPropDesc(stateWhereLicensed);
					userNamePropDesc=GetClientUserNamePropDesc(stateWhereLicensed);
					passwordPropDesc=GetClientPasswordPropDesc(stateWhereLicensed);
				}
				catch(NotImplementedException niex) {
					sbErrors.AppendLine(niex.Message);
				}
				//Validation failed.  We gave the user as much information to fix as possible.
				if(!string.IsNullOrWhiteSpace(sbErrors.ToString())) {
					result=sbErrors.ToString();
					return;
				}
				//Validation passed and we can now call the PDMP API.
				string facilityId=ProgramProperties.GetPropVal(programCur.ProgramNum,facilityIdPropDesc);
				string clientUsername=ProgramProperties.GetPropVal(programCur.ProgramNum,userNamePropDesc);
				string clientPassword=ProgramProperties.GetPropVal(programCur.ProgramNum,passwordPropDesc);
				try {
					//Each state may use a different API for its PDMP services.  Implement appropriate classes to handle each state we support.
					switch(stateWhereLicensed) {
						case "IL":
							PDMPLogicoy pdmp=new PDMPLogicoy(clientUsername,clientPassword,pat,prov,stateWhereLicensed,strDeaNum,facilityId);
							result=pdmp.GetURL();
							isSuccess=true;
							break;
						default:
							result=Lans.g("PDMP","PDMP program link has not been implemented for state: ")+stateWhereLicensed;
							return;
					}
				}
				catch(Exception ex) {
					result=ex.Message;
					return;
				}
			},startingMessage: Lans.g("PDMP","Fetching data..."));
			response=result;
			return isSuccess;
		}

		private static string GetFacilityIdPropDesc(string stateAbbr)  {
			return GetStateFull(stateAbbr)+" PDMP FacilityID";
		}

		private static string GetClientUserNamePropDesc(string stateAbbr)  {
			return GetStateFull(stateAbbr)+" PDMP Username";
		}

		private static string GetClientPasswordPropDesc(string stateAbbr) {
			return GetStateFull(stateAbbr)+" PDMP Password";
		}

		private static string GetStateFull(string stateAbbr) {
			switch(stateAbbr) {
				case "IL":
					return "Illinois";
				default:
					throw new NotImplementedException("PDMP program link not implemented for state: "+stateAbbr.ToString());
			}
		}

	}
}
