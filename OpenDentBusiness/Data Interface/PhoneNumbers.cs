using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class PhoneNumbers{

		public static List<PhoneNumber> GetPhoneNumbers(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PhoneNumber>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM phonenumber WHERE PatNum="+POut.Long(patNum);
			return Crud.PhoneNumberCrud.SelectMany(command);
		}

		public static PhoneNumber GetByVal(string phoneNumberVal) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<PhoneNumber>(MethodBase.GetCurrentMethod(),phoneNumberVal);
			}
			string command="SELECT * FROM phonenumber WHERE PhoneNumberVal='"+POut.String(phoneNumberVal)+"'";
			return Crud.PhoneNumberCrud.SelectOne(command);
		}

		///<summary></summary>
		public static long Insert(PhoneNumber phoneNumber) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				phoneNumber.PhoneNumberNum=Meth.GetLong(MethodBase.GetCurrentMethod(),phoneNumber);
				return phoneNumber.PhoneNumberNum;
			}
			return Crud.PhoneNumberCrud.Insert(phoneNumber);
		}

		///<summary></summary>
		public static void Update(PhoneNumber phoneNumber) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),phoneNumber);
				return;
			}
			Crud.PhoneNumberCrud.Update(phoneNumber);
		}

		public static void SyncAllPats() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			string command=$@"SELECT 0 PhoneNumberNum,PatNum,PhoneNumberVal,'' PhoneNumberDigits,PhoneType
				FROM phonenumber WHERE PhoneType={(int)PhoneType.Other} AND PhoneNumberVal!=''
				UNION ALL
				SELECT 0,PatNum,HmPhone,'',{(int)PhoneType.HmPhone} FROM patient WHERE HmPhone!=''
				UNION ALL
				SELECT 0,PatNum,WkPhone,'',{(int)PhoneType.WkPhone} FROM patient WHERE WkPhone!=''
				UNION ALL
				SELECT 0,PatNum,WirelessPhone,'',{(int)PhoneType.WirelessPhone} FROM patient WHERE WirelessPhone!=''";
			List<PhoneNumber> listPhNums=Crud.PhoneNumberCrud.SelectMany(command);
			command="TRUNCATE TABLE phonenumber";
			Db.NonQ(command);
			listPhNums.ForEach(x => x.PhoneNumberDigits=RemoveNonDigitsAndTrimStart(x.PhoneNumberVal));
			listPhNums.RemoveAll(x => x.PhoneType!=PhoneType.Other && string.IsNullOrEmpty(x.PhoneNumberDigits));
			Crud.PhoneNumberCrud.InsertMany(listPhNums);
		}

		///<summary>Syncs patient HmPhone, WkPhone, and WirelessPhone to the PhoneNumber table.  Will delete extra PhoneNumber table rows of each type
		///and any rows for numbers that are now blank in the patient table.</summary>
		public static void SyncPat(Patient pat) {
			SyncPats(new List<Patient>() { pat });
		}

		///<summary>Syncs patient HmPhone, WkPhone, and WirelessPhone to the PhoneNumber table.  Will delete extra PhoneNumber table rows of each type
		///and any rows for numbers that are now blank in the patient table.</summary>
		public static void SyncPats(List<Patient> listPats) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listPats);
				return;
			}
			if(listPats.Count==0) {
				return;
			}
			string command=$@"DELETE FROM phonenumber
				WHERE PatNum IN ({string.Join(",",listPats.Select(x => POut.Long(x.PatNum)))}) AND PhoneType!={(int)PhoneType.Other}";
			Db.NonQ(command);
			List<PhoneNumber> listForInsert=listPats
				.SelectMany(x => Enumerable.Range(1,3)
					.Select(y => {
						string phNumCur=y==1?x.HmPhone:(y==2?x.WkPhone:(y==3?x.WirelessPhone:""));
						return new PhoneNumber() {
							PatNum=x.PatNum,
							PhoneNumberVal=phNumCur,
							PhoneNumberDigits=RemoveNonDigitsAndTrimStart(phNumCur),
							PhoneType=(PhoneType)y
						};
					}))
				.Where(x => !string.IsNullOrEmpty(x.PhoneNumberVal) && !string.IsNullOrEmpty(x.PhoneNumberDigits)).ToList();
			if(listForInsert.Count>0) {
				Crud.PhoneNumberCrud.InsertMany(listForInsert);
			}
		}

		public static void DeleteObject(long phoneNumberNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),phoneNumberNum);
				return;
			}
			Crud.PhoneNumberCrud.Delete(phoneNumberNum);
		}

		///<summary>Removes non-digit chars and any leading 0's and 1's.</summary>
		public static string RemoveNonDigitsAndTrimStart(string phNum) {
			if(string.IsNullOrEmpty(phNum)) {
				return "";
			}
			//Not using Char.IsDigit because it includes characters like '٣' and '෯'
			return new string(phNum.Where(x => x>='0' && x<='9').ToArray()).TrimStart('0','1');
		}


	}
}