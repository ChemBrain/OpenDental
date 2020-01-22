using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class ProviderT {

		///<summary>Inserts the new provider, refreshes the cache and then returns ProvNum</summary>
		public static long CreateProvider(string abbr,string fName="",string lName="",long feeSchedNum=0,bool isSecondary=false,bool isHidden=false,
			string ssn="",bool isUsingTIN=false,string nationalProvID="")
		{
			Provider prov=new Provider();
			prov.Abbr=abbr;
			prov.FName=fName;
			prov.LName=lName;
			prov.FeeSched=feeSchedNum;
			prov.IsSecondary=isSecondary;
			prov.IsHidden=isHidden;
			prov.SSN=ssn;
			prov.UsingTIN=isUsingTIN;
			prov.NationalProvID=nationalProvID;
			Providers.Insert(prov);
			Providers.RefreshCache();
			return prov.ProvNum;
		}

		///<summary>Updates the provider passed in to the database and then refreshes the cache.</summary>
		public static void Update(Provider provider) {
			Providers.Update(provider);
			Providers.RefreshCache();
		}
	}
}
