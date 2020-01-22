using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OpenDentBusiness{
	///<summary></summary>
	public class FeeSchedGroups{
		#region Get Methods
		/// <summary>There will be at most one result for a FeeSched/Clinic combination.  Can return NULL.</summary>
		public static FeeSchedGroup GetOneForFeeSchedAndClinic(long feeSchedNum, long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<FeeSchedGroup>(MethodBase.GetCurrentMethod(),feeSchedNum,clinicNum);
			}
			//ClinicNums are stored as a comma delimited list requiring a LIKE condition.
			string command=$"SELECT * FROM feeschedgroup WHERE FeeSchedNum={feeSchedNum} AND FIND_IN_SET('{clinicNum}',ClinicNums)";
			return Crud.FeeSchedGroupCrud.SelectOne(command);
		}

		///<summary>Returns a list of every single FeeSchedGroup in the database.</summary>
		public static List<FeeSchedGroup> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<FeeSchedGroup>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM feeschedgroup";
			return Crud.FeeSchedGroupCrud.SelectMany(command);
		}

		///<summary>Returns a list of all FeeSchedGroups for a given FeeSched.  A feeSchedNum of 0 will return all feeschedgroups.</summary>
		public static List<FeeSchedGroup> GetAllForFeeSched(long feeSchedNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<FeeSchedGroup>>(MethodBase.GetCurrentMethod(),feeSchedNum);
			}
			string command=$"SELECT * FROM feeschedgroup {(feeSchedNum>0? $"WHERE FeeSchedNum={feeSchedNum}":"")}";
			return Crud.FeeSchedGroupCrud.SelectMany(command);
		}

		#endregion

		#region Modification Methods

		#region Insert

		///<summary></summary>
		public static long Insert(FeeSchedGroup feeSchedGroup) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				feeSchedGroup.FeeSchedGroupNum=Meth.GetLong(MethodBase.GetCurrentMethod(),feeSchedGroup);
				return feeSchedGroup.FeeSchedGroupNum;
			}
			return Crud.FeeSchedGroupCrud.Insert(feeSchedGroup);
		}
		#endregion

		#region Update

		///<summary></summary>
		public static void Update(FeeSchedGroup feeSchedGroup) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),feeSchedGroup);
				return;
			}
			Crud.FeeSchedGroupCrud.Update(feeSchedGroup);
		}

		#endregion

		#region Delete

		///<summary></summary>
		public static void Delete(long feeSchedGroupNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),feeSchedGroupNum);
				return;
			}
			Crud.FeeSchedGroupCrud.Delete(feeSchedGroupNum);
		}
		#endregion

		#endregion

		#region Misc Methods

		#region Fee Operations
		///<summary>Takes a list of fees that have been inserted/updated and copies those changes to the rest of the clinics in the feeschedgroup.</summary>
		public static void UpsertGroupFees(List<Fee> listFees) {
			//No need to check RemotingRole; no call to db.
			foreach(Fee feeCur in listFees) {
				FeeSchedGroup groupCur=GetOneForFeeSchedAndClinic(feeCur.FeeSched, feeCur.ClinicNum);
				if(groupCur==null) {
					continue;
				}
				//Found a group, pass feeCur and list of clinics to upsert method.  Remove clinic for feeCur as that fee has already been modified.
				CopyFeeAmountToGroup(feeCur,groupCur.ListClinicNumsAll.Where(x => x!=feeCur.ClinicNum).ToList());
			}
		}

		///<summary>Takes a list of fees to be deleted and deletes them from rest of the clinics in the feeschedgroup.</summary>
		public static void DeleteGroupFees(List<long> listFeeNums) {
			//No need to check RemotingRole; no call to db.
			DeleteGroupFees(Fees.GetManyByFeeNum(listFeeNums));
		}

		///<summary>Takes a list of fees to be deleted and deletes them from rest of the clinics in the feeschedgroup.</summary>
		public static void DeleteGroupFees(List<Fee> listFees) {
			//No need to check RemotingRole; no call to db.
			List<long> listFeeNumsToDelete=new List<long>();
			foreach(Fee feeCur in listFees) {
				FeeSchedGroup groupCur=GetOneForFeeSchedAndClinic(feeCur.FeeSched,feeCur.ClinicNum);
				if(groupCur==null) {
					continue;
				}
				//Go through the clinics in the group and delete the fee.
				foreach(long clinicNum in groupCur.ListClinicNumsAll) {
					//Skip fees passed into this method, they will be deleted elsewhere by FeeCrud
					if(clinicNum==feeCur.ClinicNum) {
						continue;
					}
					Fee feeToDelete=Fees.GetFeeFromDb(feeCur.CodeNum,feeCur.FeeSched,clinicNum,feeCur.ProvNum,true);
					if(feeToDelete!=null) {
						listFeeNumsToDelete.Add(feeToDelete.FeeNum);
					}
				}
			}
			Fees.DeleteMany(listFeeNumsToDelete,false);
		}

		///<summary>Only used by Fees.SynchList, this is basically a copy of the CRUD generated sync method with slight tweaks to work with FeeSchedGroups.
		///Only calls the group helper methods that only modify the other fees in the group, the fess in listFeesNew and listFeesOld will be left to the fees.cs
		///sync method to handle.</summary>
		public static void SyncGroupFees(List<Fee> listFeesNew,List<Fee> listFeesOld) {
			//No need to check RemotingRole; no call to db.
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<Fee> listUpsert=new List<Fee>();
			List<Fee> listDel=new List<Fee>();
			listFeesNew.Sort((Fee x,Fee y) => { return x.FeeNum.CompareTo(y.FeeNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listFeesOld.Sort((Fee x,Fee y) => { return x.FeeNum.CompareTo(y.FeeNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			Fee fieldNew;
			Fee fieldDB;
			//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.
			//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.
			while(idxNew<listFeesNew.Count || idxDB<listFeesOld.Count) {
				fieldNew=null;
				if(idxNew<listFeesNew.Count) {
					fieldNew=listFeesNew[idxNew];
				}
				fieldDB=null;
				if(idxDB<listFeesOld.Count) {
					fieldDB=listFeesOld[idxDB];
				}
				//begin compare
				if(fieldNew!=null && fieldDB==null) {//listNew has more items, listDB does not.
					//Make sure this fee is in a group before sending to the group update logic.
					if(FeeSchedGroups.GetOneForFeeSchedAndClinic(fieldNew.FeeSched,fieldNew.ClinicNum)!=null) {
						listUpsert.Add(fieldNew);
					}
					idxNew++;
					continue;
				}
				else if(fieldNew==null && fieldDB!=null) {//listDB has more items, listNew does not.
					if(FeeSchedGroups.GetOneForFeeSchedAndClinic(fieldDB.FeeSched,fieldDB.ClinicNum)!=null) {
						listDel.Add(fieldDB);
					}
					idxDB++;
					continue;
				}
				else if(fieldNew.FeeNum<fieldDB.FeeNum) {//newPK less than dbPK, newItem is 'next'
					if(FeeSchedGroups.GetOneForFeeSchedAndClinic(fieldNew.FeeSched,fieldNew.ClinicNum)!=null) {
						listUpsert.Add(fieldNew);
					}
					idxNew++;
					continue;
				}
				else if(fieldNew.FeeNum>fieldDB.FeeNum) {//dbPK less than newPK, dbItem is 'next'
					if(FeeSchedGroups.GetOneForFeeSchedAndClinic(fieldDB.FeeSched,fieldDB.ClinicNum)!=null) {
						listDel.Add(fieldDB);
					}
					idxDB++;
					continue;
				}
				//Both lists contain the 'next' item, update required
				if(FeeSchedGroups.GetOneForFeeSchedAndClinic(fieldNew.FeeSched,fieldNew.ClinicNum)!=null) {
					listUpsert.Add(fieldNew);
				}
				idxNew++;
				idxDB++;
			}
			//Check for groups to update with changes.
			UpsertGroupFees(listUpsert);
			DeleteGroupFees(listDel);
		}

		#endregion Fee Operations

		#endregion Misc Methods
		///<summary>This method Upserts copies of the supplied fee for every clinic in the list.  Matches fees based on FeeSched, CodeNum, and ProvNum.</summary>
		public static void CopyFeeAmountToGroup(Fee fee,List<long> listClinicNums) {
			//No need to check RemotingRole; no call to db.
			List<Fee> listFeesInsert=new List<Fee>();
			foreach(long clinicNum in listClinicNums) {
				//Skip the passed in fee if it was included in the list.  It is assumed to be handled by FeeCrud
				if(clinicNum==fee.ClinicNum) {
					continue;
				}
				Fee feeToChange=Fees.GetFeeFromDb(fee.CodeNum,fee.FeeSched,clinicNum,fee.ProvNum,true);
				//Couldn't find the fee in the Db, insert it.
				if(feeToChange==null) {
					Fee newFee=new Fee(){
						Amount=fee.Amount,
						FeeSched=fee.FeeSched,
						CodeNum=fee.CodeNum,
						ClinicNum=clinicNum,
						ProvNum=fee.ProvNum,
					};
					listFeesInsert.Add(newFee);
					continue;
				}
				//Found the fee, make it match the passed in fee.  All of these values are required to match to keep the group in sync.
				feeToChange.Amount=fee.Amount;
				feeToChange.FeeSched=fee.FeeSched;
				feeToChange.CodeNum=fee.CodeNum;
				feeToChange.ProvNum=fee.ProvNum;
				//Don't call FeeSchedGroup logic or we'll be sucked into an infinite void.
				Fees.Update(feeToChange,false);
			}
			if(listFeesInsert.Count>0) {
				//Don't call FeeSchedGroup logic or we'll be sucked into an infinite void.
				Fees.InsertMany(listFeesInsert,false);
			}
		}
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		#region Get Methods
		///<summary></summary>
		public static List<FeeSchedGroup> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<FeeSchedGroup>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM feeschedgroup WHERE PatNum = "+POut.Long(patNum);
			return Crud.FeeSchedGroupCrud.SelectMany(command);
		}
		
		///<summary>Gets one FeeSchedGroup from the db.</summary>
		public static FeeSchedGroup GetOne(long feeSchedGroupNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<FeeSchedGroup>(MethodBase.GetCurrentMethod(),feeSchedGroupNum);
			}
			return Crud.FeeSchedGroupCrud.SelectOne(feeSchedGroupNum);
		}
		#endregion Get Methods
		#region Modification Methods
		
		///<summary></summary>
		public static void Update(FeeSchedGroup feeSchedGroup){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),feeSchedGroup);
				return;
			}
			Crud.FeeSchedGroupCrud.Update(feeSchedGroup);
		}
		
		#endregion Modification Methods
		
		*/



	}
}