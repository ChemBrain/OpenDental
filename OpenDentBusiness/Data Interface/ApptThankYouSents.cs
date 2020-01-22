using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ApptThankYouSents{

		public static List<ApptThankYouSent> GetForApt(long aptNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ApptThankYouSent>>(MethodBase.GetCurrentMethod(),aptNum);
			}
			string command="SELECT * FROM apptthankyousent WHERE ApptNum="+POut.Long(aptNum);
			return Crud.ApptThankYouSentCrud.SelectMany(command);
		}

		public static void InsertMany(List<ApptThankYouSent> listApptThankYouSents) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listApptThankYouSents);
				return;
			}
			Crud.ApptThankYouSentCrud.InsertMany(listApptThankYouSents);
		}

		#region Update
		///<summary></summary>
		public static void Update(ApptThankYouSent apptThankYouSent){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),apptThankYouSent);
				return;
			}
			Crud.ApptThankYouSentCrud.Update(apptThankYouSent);
		}
		#endregion Update

		//If this table type will exist as cached data, uncomment the Cache Pattern region below and edit.
		/*
		#region Cache Pattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add GetTableFromCache and FillCacheFromTable to the Cache.cs file with all the other Cache types.
		//Also, consider making an invalid type for this class in Cache.GetAllCachedInvalidTypes() if needed.

		private class ApptThankYouSentCache : CacheListAbs<ApptThankYouSent> {
			protected override List<ApptThankYouSent> GetCacheFromDb() {
				string command="SELECT * FROM apptthankyousent";
				return Crud.ApptThankYouSentCrud.SelectMany(command);
			}
			protected override List<ApptThankYouSent> TableToList(DataTable table) {
				return Crud.ApptThankYouSentCrud.TableToList(table);
			}
			protected override ApptThankYouSent Copy(ApptThankYouSent apptThankYouSent) {
				return apptThankYouSent.Copy();
			}
			protected override DataTable ListToTable(List<ApptThankYouSent> listApptThankYouSents) {
				return Crud.ApptThankYouSentCrud.ListToTable(listApptThankYouSents,"ApptThankYouSent");
			}
			protected override void FillCacheIfNeeded() {
				ApptThankYouSents.GetTableFromCache(false);
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static ApptThankYouSentCache _apptThankYouSentCache=new ApptThankYouSentCache();

		public static List<ApptThankYouSent> GetDeepCopy(bool isShort=false) {
			return _apptThankYouSentCache.GetDeepCopy(isShort);
		}

		public static int GetCount(bool isShort=false) {
			return _apptThankYouSentCache.GetCount(isShort);
		}

		public static bool GetExists(Predicate<ApptThankYouSent> match,bool isShort=false) {
			return _apptThankYouSentCache.GetExists(match,isShort);
		}

		public static int GetFindIndex(Predicate<ApptThankYouSent> match,bool isShort=false) {
			return _apptThankYouSentCache.GetFindIndex(match,isShort);
		}

		public static ApptThankYouSent GetFirst(bool isShort=false) {
			return _apptThankYouSentCache.GetFirst(isShort);
		}

		public static ApptThankYouSent GetFirst(Func<ApptThankYouSent,bool> match,bool isShort=false) {
			return _apptThankYouSentCache.GetFirst(match,isShort);
		}

		public static ApptThankYouSent GetFirstOrDefault(Func<ApptThankYouSent,bool> match,bool isShort=false) {
			return _apptThankYouSentCache.GetFirstOrDefault(match,isShort);
		}

		public static ApptThankYouSent GetLast(bool isShort=false) {
			return _apptThankYouSentCache.GetLast(isShort);
		}

		public static ApptThankYouSent GetLastOrDefault(Func<ApptThankYouSent,bool> match,bool isShort=false) {
			return _apptThankYouSentCache.GetLastOrDefault(match,isShort);
		}

		public static List<ApptThankYouSent> GetWhere(Predicate<ApptThankYouSent> match,bool isShort=false) {
			return _apptThankYouSentCache.GetWhere(match,isShort);
		}

		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_apptThankYouSentCache.FillCacheFromTable(table);
		}

		///<summary>Returns the cache in the form of a DataTable. Always refreshes the ClientWeb's cache.</summary>
		///<param name="doRefreshCache">If true, will refresh the cache if RemotingRole is ClientDirect or ServerWeb.</param> 
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_apptThankYouSentCache.FillCacheFromTable(table);
				return table;
			}
			return _apptThankYouSentCache.GetTableFromCache(doRefreshCache);
		}
		#endregion Cache Pattern
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		#region Get Methods
		///<summary></summary>
		public static List<ApptThankYouSent> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ApptThankYouSent>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM apptthankyousent WHERE PatNum = "+POut.Long(patNum);
			return Crud.ApptThankYouSentCrud.SelectMany(command);
		}
		
		///<summary>Gets one ApptThankYouSent from the db.</summary>
		public static ApptThankYouSent GetOne(long apptThankYouSentNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ApptThankYouSent>(MethodBase.GetCurrentMethod(),apptThankYouSentNum);
			}
			return Crud.ApptThankYouSentCrud.SelectOne(apptThankYouSentNum);
		}
		#endregion Get Methods
		#region Modification Methods
		#region Insert
		///<summary></summary>
		public static long Insert(ApptThankYouSent apptThankYouSent){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				apptThankYouSent.ApptThankYouSentNum=Meth.GetLong(MethodBase.GetCurrentMethod(),apptThankYouSent);
				return apptThankYouSent.ApptThankYouSentNum;
			}
			return Crud.ApptThankYouSentCrud.Insert(apptThankYouSent);
		}
		#endregion Insert
		
		#region Delete
		///<summary></summary>
		public static void Delete(long apptThankYouSentNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),apptThankYouSentNum);
				return;
			}
			Crud.ApptThankYouSentCrud.Delete(apptThankYouSentNum);
		}
		#endregion Delete
		#endregion Modification Methods
		#region Misc Methods
		

		
		#endregion Misc Methods
		*/



	}
}