using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeBase;
using Newtonsoft.Json;

namespace OpenDentBusiness {
	///<summary></summary>
	public class Introspection {
		///<summary>The dictionary of testing overrides.  This variable should ONLY be used from within the DictOverrides property.</summary>
		private static Dictionary<IntrospectionEntity,string> _dictOverrides;

		///<summary>Fills _dictOverrides when it is null and returns it.  Will always return null if the IntrospectionItems preference is not present in the database.
		///This getter will check the preference cache for the aforementioned preference until it finds it.
		///Once found, _dictOverrides will be instantiated and filled with the contents of the preference.  Once instatiated, this getter will never repopulate the dictionary.
		///If the preference is present in the database but is malformed JSON, _dictOverrides will be an empty dictionary which will throw exceptions later on in the program.</summary>
		private static Dictionary<IntrospectionEntity,string> DictOverrides {
			get {
				if(_dictOverrides!=null || !Prefs.GetContainsKey(nameof(PrefName.IntrospectionItems))) {
					return _dictOverrides;
				}
				//Try to extract the introspection overrides from the preference. 
				try {
					string introspectionItems=PrefC.GetString(PrefName.IntrospectionItems);//Cache call so it is fine to do this a lot.  Purposefully throws exceptions.
					//At this point we know the database has the IntrospectionItems preference so we need to instantiate _dictOverrides.
					_dictOverrides=JsonConvert.DeserializeObject<Dictionary<IntrospectionEntity,string>>(introspectionItems);
				}
				catch(Exception ex) {
					throw new ApplicationException("Error encountered while deserializing introspection JSON. \r\nError: "+ex.Message);
				}
				return _dictOverrides;
			}
		}

		///<summary>Returns true if the IntrospectionItems preference is present within the preference cache.  Otherwise; false.</summary>
		public static bool IsTestingMode {
			get {
				return (DictOverrides!=null);
			}
		}

		///<summary>Returns the defaultValue passed in if the entity cannot be found in the global dictionary of testing overrides.
		///Purposefully throws an exception (not meant to be caught) if the IntrospectionItems preference is present in the database (should be missing in general)
		///and does not contain the entity that was passed in.  This will mean that the preference is malformed or is out of date and the preference value needs to be updated.</summary>
		public static string GetOverride(IntrospectionEntity entity,string defaultValue="") {
			if(DictOverrides!=null) {
				//DictOverrides was not null so we can assume it has the IntrospectionEntity passed in.
				//If the dictionary does not have the entity passed in then we will purposefully throw an exception tailored for engineers.
				string overrideStr;
				if(!DictOverrides.TryGetValue(entity,out overrideStr)) {
					throw new ApplicationException("Testing mode is on and the following introspection entity is not present in the IntrospectionItems preference: "+entity.ToString());
				}
				return overrideStr;
			}
			//The database does not have the IntrospectionItems preference so return defaultValue.
			return defaultValue;
		}

		///<summary>Only used for unit tests. SHOULD NOT be used otherwise.</summary>
		public static void ClearDictOverrides() {
			//This wouldn't be the end of the world if a non-unit test class does this because it just causes the dictionary to automatically refresh itself later.
			_dictOverrides=null;
		}

		///<summary>Holds 3rd party API information. IF AN ENTRY IS ADDED TO THIS ENUM PLEASE UPDATE THE QUERY BELOW.</summary>
		public enum IntrospectionEntity {
			///<summary></summary>
			DentalXChangeDwsURL,
			///<summary></summary>
			DentalXChangeDeaURL,
			///<summary></summary>
			DoseSpotURL,
			///<summary></summary>
			PayConnectRestURL,
			///<summary></summary>
			PDMPURL,
			///<summary></summary>
			PaySimpleApiURL,
			///<summary></summary>
			DoseSpotSingleSignOnURL,
			///<summary></summary>
			PayConnectWebServiceURL,
		}
	}
}

/*TIP:  Hold Alt and drag to select the query without C# comment garbage if you are unfortunate enough to need to run this.*/
/*

 INSERT INTO preference (PrefName,ValueString)
 VALUES('IntrospectionItems',
	'{
		"DentalXChangeDwsURL":"https://prelive2.dentalxchange.com/dws/DwsService",
		"DentalXChangeDeaURL":"https://prelive2.dentalxchange.com/dea/DeaPartnerService",
		"DoseSpotURL":"https://my.staging.dosespot.com/webapi",
		"PayConnectRestURL":"https://https://prelive2.dentalxchange.com/pay/rest/PayService",
		"PDMPURL":"https://openid.logicoy.com/ilpdmp/test/getReport",
		"PaySimpleApiURL":"https://sandbox-api.paysimple.com",
		"DoseSpotSingleSignOnURL":"https://my.staging.dosespot.com/LoginSingleSignOn.aspx?b=2",
		"PayConnectWebServiceURL":"https://prelive.dentalxchange.com/merchant/MerchantService?wsdl",
	}'
 );

*/
