using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebServiceSerializer;

namespace OpenDentBusiness {
	public class ItransNCpl {
		///<summary></summary>
		[JsonProperty("Carriers")]
		private List<Carrier> ListCarriers { get; set; }
		///<summary>BINs is provided for retrieval of all outstanding response transactions.
		///Making a type 04 request to each of the supplied BINs will ensure that all outstanding mailboxes are cleared.</summary>
		[JsonProperty("Rot_Bins")]
		public List<string> ListRotBins { get; set; }
		///<summary></summary>
		[JsonProperty("Change_Log")]
		private List<ChangeLog> ListChangeLogs { get; set; }
		
		///<summary>Returns a blank string if there were no errors while attempting to update internal carriers using iTrans n-cpl.json file.</summary>
		public static string TryCarrierUpdate(bool isAutomatic=true,ItransImportFields fieldsToImport=ItransImportFields.None) {
			string json;
			DateTime dateTimeTrans=DateTime.Now;
			Clearinghouse clearinghouse=Clearinghouses.GetDefaultDental();
			if(clearinghouse==null) {
				return Lans.g("Clearinghosue","Unable to update. No default dental clearinghouse set.");
			}
			//If ITRANS2 is fully setup, then use the local ITRANS2 install on server to import carrier data.
			if(clearinghouse.CommBridge==EclaimsCommBridge.ITRANS && !string.IsNullOrEmpty(clearinghouse.ResponsePath)) {
				if(!File.Exists(ODFileUtils.CombinePaths(clearinghouse.ResponsePath,"ITRANS Claims Director.exe"))) {
					return Lans.g("Clearinghouse","Unable to find 'ITRANS Claims Director.exe'. Make sure the file exists and the path is correct.");
				}
				if(isAutomatic && PrefC.GetString(PrefName.WebServiceServerName).ToLower()!=Dns.GetHostName().ToLower()) {//Only server can run when isOnlyServer is true.
					return Lans.g("Clearinghouse","Update can only run on the web service server "+PrefC.GetString(PrefName.WebServiceServerName))+
						". "+Lans.g("Clearinghouse","Connect to the server and try again.");
				}
				Process process=new Process {
					StartInfo=new ProcessStartInfo {
						FileName=ODFileUtils.CombinePaths(clearinghouse.ResponsePath,"ITRANS Claims Director.exe"),
						Arguments=" --getncpl"
					}
				};
				process.Start();
				process.WaitForExit();
				string ncplFilePath=ODFileUtils.CombinePaths(clearinghouse.ResponsePath,"n-cpl.json");
				json=File.ReadAllText(ncplFilePath);//Read n-cpl.json
				dateTimeTrans=File.GetCreationTime(ncplFilePath);
			}
			else {//ITRANS2 not used or not setup correctly, go to HQ for file content.
				try {
					string result=WebServiceMainHQProxy.GetWebServiceMainHQInstance().CanadaCarrierUpdate(PayloadHelper.CreatePayload("",eServiceCode.Undefined));
					json=WebSerializer.DeserializePrimitiveOrThrow<string>(result);
				}
				catch(Exception ex) {
					return Lans.g("Clearinghouse","Unable to update carrier list from HQ web services.")+"\r\n"+ex.Message.ToString();
				}
			}
			EtransMessageText msgTextPrev=EtransMessageTexts.GetMostRecentForType(EtransType.ItransNcpl);
			if(msgTextPrev!=null && msgTextPrev.MessageText==json) {
				if(isAutomatic || 
					ODMessageBox.Show("Carrier list has not changed since last checked.\r\nContinue?","",MessageBoxButtons.YesNo)!=DialogResult.Yes)
				{
					return Lans.g("Clearinghouse","Carrier list has not changed since last checked.");//json has not changed since we last checked, no need to update.
				}
			}
			//Save json as new etrans entry.
			Etrans etrans=Etranss.CreateEtrans(dateTimeTrans,clearinghouse.HqClearinghouseNum,json,0);
			etrans.Etype=EtransType.ItransNcpl;
			Etranss.Insert(etrans);
			ItransNCpl iTransNCpl=null;
			try {
				iTransNCpl=JsonConvert.DeserializeObject<ItransNCpl>(json);//Deserialize n-cpl.json
			}
			catch(Exception ex) {
				ex.DoNothing();
				return Lans.g("Clearinghouse","Failed to import json.");
			}
			List<CanadianNetwork> listCanadianNetworks=CanadianNetworks.GetDeepCopy();
			//List of carriers from json file that were matched by electId to multiple internal carriers
			List<Carrier> listUnmatchedJsonCarriers=new List<Carrier>();
			List<long> listMatchedDbCarrierNums=new List<long> ();
			foreach(ItransNCpl.Carrier jsonCarrier in iTransNCpl.ListCarriers) {//Update carriers.
				string jsonCarrierPhone=jsonCarrier.Telephone?.First().Value;//Will be empty string if not found.
				List<OpenDentBusiness.Carrier> listDbCarriers=Carriers.GetAllByElectId(jsonCarrier.Bin).FindAll(x => x.IsCDA);
				if(listDbCarriers.Count>1) {//Some Canadian carriers share ElectId, need to filter further.  This happens with carrier resellers.
					#region Additional matching based on phone numbers, 'continues' loop if a single match is not found.
					List<OpenDentBusiness.Carrier> listPhoneMatchedDbCarriers=listDbCarriers.FindAll(x =>
						TelephoneNumbers.AreNumbersEqual(x.Phone,jsonCarrierPhone)
					);
					if(listPhoneMatchedDbCarriers.Count!=1) {//Either 0 or multiple matches, either way do not update any carriers.
						//When 0 matches found:	jsonCarrier changed their phone number, can not determine which carrier to update.
						//E.G. - JsonCarrier A matched to OD carriers B and C by electId.
						//Phone number from JsonCarrier A did not match either carrier B or C due to jsonCarrier phone number change.
						//Future iterations for jsonCarrier D might match to carrier B or C if phone number for jsonCarrier D did not change.
						//If jsonCarrier D is matched to single OD carrier, then jsonCarrier A will attempt to match near end of method to a unmatched internal carrier.
						//If ther are no future matches to OD carrier B or C then all unmatched jsonCarriers will not be imported and no OD carries will not be updated.
						//----------------------------------------------------------------------//
						//When greater than 1:	jsonCarrier number not changed and both internal carriers have matching electIds and phone numbers.
						//This should be rare, most likely a setup error.
						//There should not be multiple carriers that share electId and phone numbers. User should change or remove one of the matched carriers.
						listUnmatchedJsonCarriers.Add(jsonCarrier);
						continue;
					}
					listDbCarriers=listPhoneMatchedDbCarriers;
					#endregion
				}
				//At this point listDbCarriers should either be empty or contain a single OD carrier.
				OpenDentBusiness.Carrier carrierInDb=listDbCarriers.FirstOrDefault();//Null if list is empty.
				if(carrierInDb==null) {//Carrier can not be matched to internal Carrier based on ElectID.
					#region Insert new carrier
					if(!fieldsToImport.HasFlag(ItransImportFields.AddMissing)) {
						continue;
					}
					OpenDentBusiness.Carrier carrierNew=new OpenDentBusiness.Carrier();
					carrierNew.CanadianEncryptionMethod=1;//Default.  Deprecated for all Canadian carriers and will never be any other value.
					TrySetCanadianNetworkNum(jsonCarrier,carrierNew,listCanadianNetworks);
					carrierNew.ElectID=jsonCarrier.Bin;
					carrierNew.IsCDA=true;
					carrierNew.CarrierName=jsonCarrier.Name.En;
					carrierNew.Phone=TelephoneNumbers.AutoFormat(jsonCarrierPhone);
					if(jsonCarrier.Address.Count()>0) {
						Address add=jsonCarrier.Address.First();
						carrierNew.Address=add.Street1;
						carrierNew.Address2=add.Street2;
						carrierNew.City=add.City;
						carrierNew.State=add.Province;
						carrierNew.Zip=add.Postal_Code;
					}
					carrierNew.CanadianSupportedTypes=GetSupportedTypes(jsonCarrier);
					carrierNew.CDAnetVersion=POut.Int(jsonCarrier.Versions.Max(x => PIn.Int(x))).PadLeft(2,'0');//Version must be in 2 digit format. ex. 02.
					carrierNew.CarrierName=jsonCarrier.Name.En;
					try {
						Carriers.Insert(carrierNew);
					}
					catch(Exception ex) {
						ex.DoNothing();
					}
					#endregion
					continue;
				}
				listMatchedDbCarrierNums.Add(carrierInDb.CarrierNum);
				UpdateCarrierInDb(carrierInDb,jsonCarrier,listCanadianNetworks,fieldsToImport,jsonCarrierPhone,isAutomatic);
			}
			foreach(Carrier jsonCarrier in listUnmatchedJsonCarriers) { 
				List<OpenDentBusiness.Carrier> listDbCarriers=Carriers.GetWhere(x => x.IsCDA
					&& x.ElectID==jsonCarrier.Bin && !listMatchedDbCarrierNums.Contains(x.CarrierNum)
				);
				if(listDbCarriers.Count!=1) {//Either 0 or multiple matches, either way do not update any carriers.
					continue;
				}
				OpenDentBusiness.Carrier carrierInDb=listDbCarriers.FirstOrDefault();
				string jsonCarrierPhone=jsonCarrier.Telephone?.First().Value;
				UpdateCarrierInDb(carrierInDb,jsonCarrier,listCanadianNetworks,fieldsToImport,jsonCarrierPhone,isAutomatic);
			}
			return "";//Blank string represents a completed update.
		}

		///<summary></summary>
		private static void UpdateCarrierInDb(OpenDentBusiness.Carrier odCarrier,Carrier jsonCarrier,List<CanadianNetwork> listCanadianNetworks
			,ItransImportFields fieldsToImport,string jsonCarrierPhone,bool isAutomatic)
		{
			OpenDentBusiness.Carrier odCarrierOld=odCarrier.Copy();
			odCarrier.CanadianEncryptionMethod=1;//Default.  Deprecated for all Canadian carriers and will never be any other value.
			TrySetCanadianNetworkNum(jsonCarrier,odCarrier,listCanadianNetworks);
			odCarrier.CanadianSupportedTypes=GetSupportedTypes(jsonCarrier);
			odCarrier.CDAnetVersion=POut.Int(jsonCarrier.Versions.Max(x => PIn.Int(x))).PadLeft(2,'0');//Version must be in 2 digit format. ex. 02.
			List<ItransImportFields> listFields=Enum.GetValues(typeof(ItransImportFields)).Cast<ItransImportFields>().ToList();
			foreach(ItransImportFields field in listFields) {
				if(fieldsToImport==ItransImportFields.None) {
					break;//No point in looping.
				}
				if(field==ItransImportFields.None || !fieldsToImport.HasFlag(field)) {
					continue;
				}
				switch(field) {
					case ItransImportFields.Phone:
						odCarrier.Phone=TelephoneNumbers.AutoFormat(jsonCarrierPhone);
						break;
					case ItransImportFields.Address:
						if(jsonCarrier.Address.Count()>0) {
							Address add=jsonCarrier.Address.First();
							odCarrier.Address=add.Street1;
							odCarrier.Address2=add.Street2;
							odCarrier.City=add.City;
							odCarrier.State=add.Province;
							odCarrier.Zip=add.Postal_Code;
						}
						break;
					case ItransImportFields.Name:
						odCarrier.CarrierName=jsonCarrier.Name.En;
						break;
				}
			}
			try {
				long userNum=0;
				if(!isAutomatic) {
					userNum=Security.CurUser.UserNum;
				}
				Carriers.Update(odCarrier,odCarrierOld,userNum);
			}
			catch(Exception ex) {
				ex.DoNothing();
			}
		}

		private static void TrySetCanadianNetworkNum(Carrier jsonCarrier,OpenDentBusiness.Carrier odCarrier,List<CanadianNetwork> listCanadianNetworks) {
			if(jsonCarrier.Network==null || jsonCarrier.Network.Count==0) {
				return;//We need to be careful not to fully trust data coming from other applications.
			}
			//Telus B is now the primary network in Canada, includes almost all carriers.
			Network jsonCarrierNetwork=jsonCarrier.Network.FirstOrDefault(x => x.Network_Folder=="TGB");
			if(jsonCarrierNetwork==null) {//We have a bypass that treats INSTREAM like Telus B.  Try looking for INSTREAM now.
				jsonCarrierNetwork=jsonCarrier.Network.FirstOrDefault(x => x.Network_Folder=="INS");
			}
			if(jsonCarrierNetwork==null) {//Otherswise default to first value in list.
				jsonCarrierNetwork=jsonCarrier.Network[0];
			}
			string odAbbr="";
			switch(jsonCarrierNetwork.Network_Folder) {
				case "ABC"://Alberta Blue Cross
					odAbbr="ABC";
					break;
				case "TGA"://Telus Group A, this isn't in our current json file. Best guess.
					odAbbr="TELUS A";
					break;
				case "TGB"://Telus Group B
					odAbbr="TELUS B";
					break;
				case "INS"://instream
					odAbbr="CSI";//CSI is the previous name for the network now known as INSTREAM.
					break;
				default://Unknown network_foler, CanadianNetworkNum will not be set.
					ODException.SwallowAnyException(() => {//Let HQ known if this ever happens so that it doesn't go unnoticed.
						BugSubmissions.SubmitException(
							new ApplicationException("Unknown iTrans Network. Name: "+jsonCarrierNetwork.Name+" - Folder: "+jsonCarrierNetwork.Network_Folder)
						);
					});
					break;
			}
			CanadianNetwork network=listCanadianNetworks.FirstOrDefault(x => x.Abbrev==odAbbr);
			if(network!=null) {
				odCarrier.CanadianNetworkNum=network.CanadianNetworkNum;
			}
		}

		private static CanSupTransTypes GetSupportedTypes(Carrier jsonCarrier) {
			CanSupTransTypes supportedTypes=CanSupTransTypes.None;
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Eligibility_08,CanSupTransTypes.EligibilityTransaction_08);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Eligibility_18,CanSupTransTypes.EligibilityResponse_18);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Cob_07,CanSupTransTypes.CobClaimTransaction_07);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Claim_11,CanSupTransTypes.ClaimAckEmbedded_11e);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Claim_21,CanSupTransTypes.ClaimEobEmbedded_21e);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Reversal_02,CanSupTransTypes.ClaimReversal_02);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Reversal_12,CanSupTransTypes.ClaimReversalResponse_12);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Predetermination_03,CanSupTransTypes.PredeterminationSinglePage_03);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Predetermination_Multi,CanSupTransTypes.PredeterminationMultiPage_03);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Predetermination_13,CanSupTransTypes.PredeterminationAck_13|CanSupTransTypes.PredeterminationAckEmbedded_13e);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Predetermination_23,CanSupTransTypes.PredeterminationAck_13|CanSupTransTypes.PredeterminationAckEmbedded_13e);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Outstanding_04,CanSupTransTypes.RequestForOutstandingTrans_04);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Outstanding_14,CanSupTransTypes.OutstandingTransAck_14);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Summary_Reconciliation_05,CanSupTransTypes.RequestForSummaryReconciliation_05);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Summary_Reconciliation_15,CanSupTransTypes.SummaryReconciliation_15);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Payment_Reconciliation_06,CanSupTransTypes.RequestForPaymentReconciliation_06);
			UpdateSupportedTypes(ref supportedTypes,jsonCarrier.Payment_Reconciliation_16,CanSupTransTypes.PaymentReconciliation_16);
			return supportedTypes;
		}

		///<summary>Updates the given bitwise supportedType enum to include onSuccessTransType when jsonFieldValue passes validation.</summary>
		private static void UpdateSupportedTypes(ref CanSupTransTypes supportedType,EnumNXYZ jsonFieldValue,CanSupTransTypes onSuccessTransType) {
			//EnumNXYZ.X is supported because it should only ever be given to us if the associated carrier is a version 04 carrier.
			//Some json carriers report that they support both versions 02 and 04, in this case we always select 04.
			//If we ever get a version 02 carrier that also returns X then we will need to check the version somewhere around here.
			supportedType=(jsonFieldValue.In(EnumNXYZ.Y,EnumNXYZ.X)?(supportedType|onSuccessTransType):supportedType);
		}

		private class Carrier {
			public EnglishFrenchStr Name { get; set; }
			public string Change_Date { get; set; }
			public List<CarrierTelephone> Telephone { get; set; }
			public string Bin { get; set; }
			public List<string> Versions { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Batch { get; set; }
			public long Age_Days { get; set; }
			public EnglishFrenchStr Policy_Number { get; set; }
			public EnglishFrenchStr Division_Number { get; set; }
			public EnglishFrenchStr Certificate_Number { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Claim_01 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Claim_11 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Claim_21 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Reversal_02 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Reversal_12 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Predetermination_03 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Predetermination_13 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Predetermination_23 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Predetermination_Multi { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Outstanding_04 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Outstanding_14 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Summary_Reconciliation_05 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Summary_Reconciliation_15 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Payment_Reconciliation_06 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Payment_Reconciliation_16 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Cob_07 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Eligibility_08 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Eligibility_18 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Attachment_09 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Attachment_19 { get; set; }
			public EnglishFrenchStr Cob_Instructions { get; set; }
			public EnglishFrenchStr Notes { get; set; }
			public ClaimsProcessor Claims_Processor { get; set; }
			public List<Address> Address { get; set; }
			public List<Network> Network { get; set; }
		}

		private class Address {
			public string Street1 { get; set; }
			public string Street2 { get; set; }
			public string City { get; set; }
			public string Province { get; set; }
			public string Postal_Code { get; set; }
			public string Attention { get; set; }
			public EnglishFrenchStr Notes { get; set; }
		}

		private class EnglishFrenchStr {
			public string En { get; set; }
			public string Fr { get; set; }
		}

		private class ClaimsProcessor {
			public string ChangeDate { get; set; }
			public EnglishFrenchStr Name { get; set; }
			public EnglishFrenchStr ShortName { get; set; }
		}

		private class Network {
			public List<NetworkTelephone> Telephone { get; set; }
			public Load Load { get; set; }
			public string Network_Folder { get; set; }
			public EnglishFrenchStr Name { get; set; }
			public string ChangeDate { get; set; }
		}

		private class Load {
			public string ChangeDate { get; set; }
			public long Percent { get; set; }
		}

		private class NetworkTelephone {
			public EnglishFrenchStr Name { get; set; }
			public string ChangeDate { get; set; }
			public string Phone { get; set; }
		}

		private class CarrierTelephone {
			public string ChangeDate { get; set; }
			public string Value { get; set; }
			public EnglishFrenchStr Name { get; set; }
		}

		private class ChangeLog {
			public string ChangeDate { get; set; }
			public EnglishFrenchStr Description { get; set; }
		}

		[JsonConverter(typeof(EnumConverter))]
		public enum EnumNXYZ {
			///<summary>Never in carrier json file. This represents that the value was either null or empty.</summary>
			Missing,
			///<summary>Not supported</summary>
			N,
			///<summary>Version 4 only. Should only be returned when this carrier is version 04.</summary>
			X,
			///<summary>Version 2 or version 4.</summary>
			Y,
			///<summary>Only blue on blue, same carrier (primary and secondary).
			///We expect (and have observed) this flag to only be for COB transaction type 07.
			///This is treated the same as N (no), because the OD user cannot sent a secondary claim by itself,
			///thus can only trigger this scenraio by sending primray claim.</summary>
			Z 
		}

		public class EnumConverter:StringEnumConverter {
			public override object ReadJson(JsonReader reader,Type objectType,object existingValue,JsonSerializer serializer) {
				if(string.IsNullOrEmpty(reader.Value.ToString())) {
					return EnumNXYZ.Missing;
				}
				return base.ReadJson(reader,objectType,existingValue,serializer);
			}
		}

	}


	[Flags]
	public enum ItransImportFields {
		///<summary></summary>
		None=0,
		///<summary>When enabled, updates carrier.Phone field from ITRANS 2.0 json file.</summary>
		Phone=1,
		///<summary>When enabled, updates various carrier address fields from ITRANS 2.0 json file.</summary>
		Address=2,
		///<summary>When enabled, updates carrier.CarrierName field from ITRANS 2.0 json file.</summary>
		Name=4,
		///<summary>When enabled, inserts a new carrier from json file when it can not be matched to internal carrier via ElectID.</summary>
		AddMissing=8,
	}
}
