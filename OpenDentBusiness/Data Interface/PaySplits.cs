using CodeBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class PaySplits {
		#region Get Methods
		///<summary>Returns all paySplits for the given patNum, organized by DatePay.  WARNING! Also includes related paysplits that aren't actually attached to patient.  Includes any split where payment is for this patient.</summary>
		public static PaySplit[] Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<PaySplit[]>(MethodBase.GetCurrentMethod(),patNum);
			}
			/*This query was too slow
			string command=
				"SELECT DISTINCT paysplit.* FROM paysplit,payment "
				+"WHERE paysplit.PayNum=payment.PayNum "
				+"AND (paysplit.PatNum = '"+POut.Long(patNum)+"' OR payment.PatNum = '"+POut.Long(patNum)+"') "
				+"ORDER BY DatePay";*/
			//this query goes 10 times faster for very large databases
			string command=@"select DISTINCT paysplitunion.* FROM "
				+"(SELECT DISTINCT paysplit.* FROM paysplit,payment "
				+"WHERE paysplit.PayNum=payment.PayNum and payment.PatNum='"+POut.Long(patNum)+"' "
				+"UNION "
				+"SELECT DISTINCT paysplit.* FROM paysplit,payment "
				+"WHERE paysplit.PayNum = payment.PayNum AND paysplit.PatNum='"+POut.Long(patNum)+"') paysplitunion "
				+"ORDER BY paysplitunion.DatePay";
			return Crud.PaySplitCrud.SelectMany(command).ToArray();
		}

		///<summary>Returns a list of paysplits that have AdjNum of any of the passed in adjustments.</summary>
		public static List<PaySplit> GetForAdjustments(List<long> listAdjustNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),listAdjustNums);
			}
			if(listAdjustNums==null || listAdjustNums.Count==0) {
				return new List<PaySplit>();
			}
			string command="SELECT * FROM paysplit WHERE AdjNum IN ("+string.Join(",",listAdjustNums)+")";
			return Crud.PaySplitCrud.SelectMany(command);
		}

		public static List<PaySplit> GetForPayPlanCharges(List<long> listPayPlanChargeNums) {
			if(listPayPlanChargeNums.IsNullOrEmpty()) {
				return new List<PaySplit>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),listPayPlanChargeNums);
			}
			string command=$"SELECT * FROM paysplit WHERE PayPlanChargeNum IN ({string.Join(",",listPayPlanChargeNums)})";
			return Crud.PaySplitCrud.SelectMany(command);
		}

		///<summary>Used from payment window to get all paysplits for the payment.</summary>
		public static List<PaySplit> GetForPayment(long payNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),payNum);
			}
			string command=
				"SELECT * FROM paysplit "
				+"WHERE PayNum="+POut.Long(payNum);
			return Crud.PaySplitCrud.SelectMany(command);
		}

		///<summary>Gets the splits for all the payments passed in.</summary>
		public static List<PaySplit> GetForPayments(List<long> listPayNums) {
			if(listPayNums.IsNullOrEmpty()) {
				return new List<PaySplit>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),listPayNums);
			}
			string command=
				"SELECT * FROM paysplit "
				+"WHERE PayNum IN("+string.Join(",",listPayNums.Select( x=> POut.Long(x)))+")";
			return Crud.PaySplitCrud.SelectMany(command);
		}

		public static void InsertMany(List<PaySplit> listSplits) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listSplits);
				return;
			}
			foreach(PaySplit split in listSplits) {
				//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
				split.SecUserNumEntry=Security.CurUser.UserNum;
			}
			Crud.PaySplitCrud.InsertMany(listSplits);
		}

		///<summary>Gets one paysplit using the specified SplitNum.</summary>
		public static PaySplit GetOne(long splitNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<PaySplit>(MethodBase.GetCurrentMethod(),splitNum);
			}
			string command="SELECT * FROM paysplit WHERE SplitNum="+POut.Long(splitNum);
			return Crud.PaySplitCrud.SelectOne(command);
		}

		///<summary>Used from FormPayment to return the total payments for a procedure without requiring a supplied list.</summary>
		public static string GetTotForProc(long procNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),procNum);
			}
			string command="SELECT SUM(paysplit.SplitAmt) FROM paysplit "
				+"WHERE paysplit.ProcNum="+POut.Long(procNum);
			return Db.GetScalar(command);

		}

		///<summary>Returns all paySplits for the given procNum. Must supply a list of all paysplits for the patient.</summary>
		public static ArrayList GetForProc(long procNum,PaySplit[] List) {
			//No need to check RemotingRole; no call to db.
			ArrayList retVal=new ArrayList();
			for(int i=0;i<List.Length;i++){
				if(List[i].ProcNum==procNum){
					retVal.Add(List[i]);
				}
			}
			return retVal;
		}

		///<summary>Used from FormAdjust to display and calculate payments for procs attached to adjustments.</summary>
		public static double GetTotForProc(Procedure procCur) {
			//No need to check RemotingRole; no call to db.
			return Refresh(procCur.PatNum).Where(x => x.ProcNum==procCur.ProcNum).Select(x => x.SplitAmt).Sum();
		}

		///<summary>Used from FormPaySplitEdit.  Returns total payments for a procedure for all paysplits other than the supplied excluded paysplit.</summary>
		public static double GetTotForProc(long procNum,PaySplit[] List,PaySplit paySplitToExclude,out int countSplitsAttached) {
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			countSplitsAttached=0;
			for(int i=0;i<List.Length;i++){
				if(List[i].IsSame(paySplitToExclude)) {
					continue;
				}
				if(List[i].ProcNum==procNum){
					countSplitsAttached++;
					retVal+=List[i].SplitAmt;
				}
			}
			return retVal;
		}

		///<summary>Used once in ContrAccount.  WARNING!  The returned list of 'paysplits' are not real paysplits.  They are actually grouped by patient and date.  Only the DateEntry, SplitAmt, PatNum, and ProcNum(one of many) are filled. Must supply a list which would include all paysplits for this payment.</summary>
		public static ArrayList GetGroupedForPayment(long payNum,PaySplit[] List) {
			//No need to check RemotingRole; no call to db.
			ArrayList retVal=new ArrayList();
			int matchI;
			for(int i=0;i<List.Length;i++){
				if(List[i].PayNum==payNum){
					//find a 'paysplit' with matching DateEntry and patnum
					matchI=-1;
					for(int j=0;j<retVal.Count;j++){
						if(((PaySplit)retVal[j]).DateEntry==List[i].DateEntry && ((PaySplit)retVal[j]).PatNum==List[i].PatNum){
							matchI=j;
							break;
						}
					}
					if(matchI==-1){
						retVal.Add(new PaySplit());
						matchI=retVal.Count-1;
						((PaySplit)retVal[matchI]).DateEntry=List[i].DateEntry;
						((PaySplit)retVal[matchI]).PatNum=List[i].PatNum;
					}
					if(((PaySplit)retVal[matchI]).ProcNum==0 && List[i].ProcNum!=0){
						((PaySplit)retVal[matchI]).ProcNum=List[i].ProcNum;
					}
					((PaySplit)retVal[matchI]).SplitAmt+=List[i].SplitAmt;
				}
			}
			return retVal;
		}

		///<summary>Used in Payment window to get all paysplits for a single patient without using a supplied list.</summary>
		public static List<PaySplit> GetForPats(List<long> listPatNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),listPatNums);
			}
			string command="SELECT * FROM paysplit "
				+"WHERE PatNum IN("+String.Join(", ",listPatNums)+")";
			return Crud.PaySplitCrud.SelectMany(command);
		}

		///<summary>Used once in ContrAccount to just get the splits for a single patient.  The supplied list also contains splits that are not necessarily for this one patient.</summary>
		public static PaySplit[] GetForPatient(long patNum,PaySplit[] List) {
			//No need to check RemotingRole; no call to db.
			ArrayList retVal=new ArrayList();
			for(int i=0;i<List.Length;i++){
				if(List[i].PatNum==patNum){
					retVal.Add(List[i]);
				}
			}
			PaySplit[] retList=new PaySplit[retVal.Count];
			retVal.CopyTo(retList);
			return retList;
		}

		///<summary>For a given PayPlan, returns a table of PaySplits with additional payment information.
		///The additional information from the payment table will be columns titled "CheckNum", "PayAmt", and "PayType"</summary>
		public static DataTable GetForPayPlan(long payPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),payPlanNum);
			}
			string command="SELECT paysplit.*,payment.CheckNum,payment.PayAmt,payment.PayType "
					+"FROM paysplit "
					+"LEFT JOIN payment ON paysplit.PayNum=payment.PayNum "
					+"WHERE paysplit.PayPlanNum="+POut.Long(payPlanNum)+" "
					+"ORDER BY DatePay";
			DataTable tableSplits=Db.GetTable(command);
			return tableSplits;
		}

		///<summary>For a given PayPlan, returns a list of PaySplits associated to that PayPlan.</summary>
		public static List<PaySplit> GetForPayPlans(List<long> listPayPlanNums) {
			if(listPayPlanNums.Count==0) {
				return new List<PaySplit>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),listPayPlanNums);
			}
			string command="SELECT paysplit.* "
					+"FROM paysplit "
					+"WHERE paysplit.PayPlanNum IN ("+POut.String(String.Join(",",listPayPlanNums))+") "
					+"ORDER BY DatePay";
			List<PaySplit> listSplits=Crud.PaySplitCrud.SelectMany(command);
			return listSplits;
		}

		///<summary>Gets paysplits from a provided datatable.  This was originally part of GetForPayPlan but can't be because it's passed through the Middle Tier.</summary>
		public static List<PaySplit> GetFromBundled(DataTable dataTable) {
			//No need to check RemotingRole; no call to db.
			return Crud.PaySplitCrud.TableToList(dataTable);
		}

		///<summary>Used once in ContrAccount.  Usually returns 0 unless there is a payplan for this payment and patient.</summary>
		public static long GetPayPlanNum(long payNum,long patNum,PaySplit[] List) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<List.Length;i++){
				if(List[i].PayNum==payNum && List[i].PatNum==patNum && List[i].PayPlanNum!=0){
					return List[i].PayPlanNum;
				}
			}
			return 0;
		}

		///<summary>Helper method for GetPrepayForFam(...) which checks if the resulting query should be for onlyUnallocated or if we doExcludeTpPrepay, 
		///it appends the necessary results to the query command and returns it to be added to the larger query.</summary>
		private static string UnearnedQueryHelper(bool onlyUnallocated,bool doExcludeTpPrepay) {
			string command="";
			if(!onlyUnallocated) {
				command+="AND UnearnedType!=0 ";
			}
			if(doExcludeTpPrepay) {//do not retrieve prepayments that have been set aside for a tp procedure
				command+=$"AND ProcNum=0 ";
				if(GetHiddenUnearnedDefNums().Count > 0){
					command+=$"AND UnearnedType NOT IN ({string.Join(",",GetHiddenUnearnedDefNums())}) ";
				}
			}
			return command;
		}

		///<summary>Gets all paysplits that have are designated as prepayments for the patient's family. isAccountModule should be true only when the call 
		///to this method originates from the Account Module and is being used for display of the resulting paysplits in the account grid, at all other 
		///times it should remain false.</summary>
		public static List<PaySplit> GetPrepayForFam(Family fam,bool onlyUnallocated=true,bool doExcludeTpPrepay=false,bool isAccountModule=false) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),fam,onlyUnallocated,doExcludeTpPrepay,isAccountModule);
			}
			List<long> listFamPatNums=fam.ListPats.Select(x => x.PatNum).Distinct().ToList();
			string command="SELECT * FROM paysplit WHERE PatNum IN ("+String.Join(",",listFamPatNums)+") "
				+UnearnedQueryHelper(onlyUnallocated,doExcludeTpPrepay);
			//If the method is called from the Account Module, check that there are no hidden splits paid to those outside the family by this patient
			//this will ensure that the "Hidden" tab appears on this account and shows the paysplits to people outside of the patient's fmaily, if
			//we don't do this, the only account the payment will show on is the one who received it, making it very hard to keep track of the paying 
			//patient's paysplit or know that it even happened at all
			if(isAccountModule) {
				command+="UNION ALL "
					+"SELECT paysplit.* FROM payment "//We use payment here so that we can filter the results based on payment.PatNum
					+"INNER JOIN paysplit ON paysplit.PayNum=payment.PayNum " 
					+$"WHERE payment.PatNum IN ({String.Join(",",listFamPatNums)}) "//Ensures we do not get _all_ the paysplits that are not in the family
					//This would result in a lot of data and very slow response - beyond inaccurate to what we are after (showing payments made by this 
					//patient) besides!
					+$"AND paysplit.PatNum NOT IN ({String.Join(",",listFamPatNums)}) "
					+UnearnedQueryHelper(onlyUnallocated,doExcludeTpPrepay);
			}
			command+="ORDER BY DatePay";
			List<PaySplit> listSplitsAll=Crud.PaySplitCrud.SelectMany(command);//need a full list for when searching for allocations.
			if(!onlyUnallocated) {
				return listSplitsAll;
			}
			List<PaySplit> listUnearnedSplits=listSplitsAll.FindAll(x => x.UnearnedType!=0);//excluded from query so we can get parent splits. 
			//only allocated splits
			List<PaySplit> listSplitsAllocated=new List<PaySplit>();
			foreach(PaySplit split in listUnearnedSplits) {
				if(split.FSplitNum==0) {
					listSplitsAllocated.Add(split);//having an FsplitNum of 0 automatically means that this split is an original unearned split.
					continue;
				}
				//Unearned can be unallocated and also have an FSplitNum, so we need to check the parent of this split. If it is not unearned then it
				//is valid to be added to this list.
				//If the parent is unearned then it is likey coming from different unearned. 
				PaySplit parentSplit=listSplitsAll.FirstOrDefault(x => x.SplitNum==split.FSplitNum);
				if(parentSplit==null || parentSplit.UnearnedType==0) {
					listSplitsAllocated.Add(split);//either we couldn't find a parent OR we did find a parent, but the parent is not unearned so it is valid.
				}
			}
			return listSplitsAllocated;
		}

		///<summary>Gets all paysplits that are attached to the prepayment paysplits specified.</summary>
		public static List<PaySplit> GetSplitsForPrepay(List<PaySplit> listPrepaymentSplits) {
			if(listPrepaymentSplits==null || listPrepaymentSplits.Count(x => x.SplitNum!=0) < 1) {
				return new List<PaySplit>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),listPrepaymentSplits);
			}
			List<long> listSplitNums=listPrepaymentSplits.Where(x => x.SplitNum!=0).Select(x => x.SplitNum).Distinct().ToList();
			string command="SELECT * FROM paysplit WHERE FSplitNum IN ("+String.Join(",",listSplitNums)+")";
			return Crud.PaySplitCrud.SelectMany(command);
		}

		///<summary>Returns the original prepayment.</summary>
		public static PaySplit GetOriginalPrepayment(PaySplit paySplit) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<PaySplit>(MethodBase.GetCurrentMethod(),paySplit);
			}
			long fSplitNum=paySplit.FSplitNum;
			if(paySplit.UnearnedType==0) {//paySplit is pos allocation split, find negative income transfer split first
				fSplitNum=Db.GetLong("SELECT FSplitNum FROM paysplit WHERE paysplit.SplitNum="+POut.Long(paySplit.FSplitNum));
			}
			string command="SELECT * FROM paysplit WHERE paysplit.SplitNum="+POut.Long(fSplitNum);
			return Crud.PaySplitCrud.SelectOne(command);
		}

		///<summary>Gets a list of all PaySplits allocated to the SplitNum passed in.</summary>
		public static List<PaySplit> GetAllocatedElseWhere(long splitNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),splitNum);
			}
			string command=@"
				SELECT * 
				FROM paysplit
				WHERE EXISTS (SELECT SplitNum FROM paysplit ps2 WHERE paysplit.FSplitNum=ps2.SplitNum AND ps2.FSplitNum="+POut.Long(splitNum)+@")
				AND UnearnedType=0 ";
			return Crud.PaySplitCrud.SelectMany(command);
		}

		public static List<PaySplit> GetSplitsLinked(List<PaySplit> listPaySplits) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),listPaySplits);
			}
			if(listPaySplits==null || listPaySplits.Count < 1) {
				return new List<PaySplit>();
			}
			List<long> listSplitNums=listPaySplits.Select(x => x.FSplitNum).Distinct().ToList();
			string command="SELECT * FROM paysplit WHERE SplitNum IN ("+String.Join(",",listSplitNums)+")";
			return Crud.PaySplitCrud.SelectMany(command);
		}

		///<summary>A function that specifically returns the sum of all unearned splits for a family regardless of status or FSplitNum associations.</summary>
		public static decimal GetSumUnearnedForFam(Family fam,List<PaySplit> listPrePayments=null) {
			listPrePayments=listPrePayments??PaySplits.GetPrepayForFam(fam,false);
			return (decimal)listPrePayments.Sum(x => x.SplitAmt);
		}

		/// <summary>Gets a list of all unearned types that are marked as hidden on account.</summary>
		public static List<long> GetHiddenUnearnedDefNums() {
			//No need to check RemotingRole; no call to db.
			return Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType).FindAll(x => !string.IsNullOrEmpty(x.ItemValue)).Select(x => x.DefNum).ToList();
		}

		///<summary>Returns the total amount of prepayments for the entire family.  To ignore a specific payment provide the payNumExcluded param.</summary>
		public static decimal GetUnearnedForFam(Family fam,List<PaySplit> listPrePayments=null,long payNumExcluded=0) {
			//No need to check RemotingRole; no call to db.
			//Find all paysplits for this account with provnum=0
			//Foreach paysplit find all other paysplits with paysplitnum == provnum0 paysplit
			//Sum paysplit amounts, see if it covers provnum0 split.
			//Any money left over sum and show as "Unallocated" aka unearned
			decimal unearnedTotal=0;
			listPrePayments=listPrePayments??PaySplits.GetPrepayForFam(fam);//set back to true for a previous bug
			if(listPrePayments.Count>0) { 
				foreach(PaySplit split in listPrePayments) {
					if(payNumExcluded>0 && split.PayNum==payNumExcluded) {
						continue;//skip paysplits with passed in PayNum
					}
					unearnedTotal+=(decimal)split.SplitAmt;
				}
				List<PaySplit> listSplitsForPrePayment=PaySplits.GetSplitsForPrepay(listPrePayments);
				foreach(PaySplit split in listSplitsForPrePayment) {
					if(payNumExcluded>0 && split.PayNum==payNumExcluded) {
						continue;//skip paysplits with passed in PayNum
					}
					unearnedTotal+=(decimal)split.SplitAmt;//Splits for prepayments are generally negative.
				}
			}
			return unearnedTotal;
		}

		///<summary>Takes a procNum and returns a list of all paysplits associated to the procedure.Returns an empty list if there are none.</summary>
		public static List<PaySplit> GetPaySplitsFromProc(long procNum) {
			//No need to check RemotingRole; no call to db.
			return GetPaySplitsFromProcs(new List<long>() { procNum });
		}

		///<summary>Takes a list of procNums and returns a list of all paysplits associated to the procedures.  Returns an empty list if there are none.</summary>
		public static List<PaySplit> GetPaySplitsFromProcs(List<long> listProcNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),listProcNums);
			}
			if(listProcNums==null || listProcNums.Count<1) {
				return new List<PaySplit>();
			}
			string command="SELECT * FROM paysplit WHERE ProcNum IN("+string.Join(",",listProcNums)+")";
			return Crud.PaySplitCrud.SelectMany(command);
		}
		#endregion

		#region Modification Methods
		
		#region Insert
		///<summary></summary>
		public static long Insert(PaySplit split) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				split.SplitNum=Meth.GetLong(MethodBase.GetCurrentMethod(),split);
				return split.SplitNum;
			}
			//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
			split.SecUserNumEntry=Security.CurUser.UserNum;
			return Crud.PaySplitCrud.Insert(split);
		}
		#endregion

		#region Update
		///<summary></summary>
		public static void Update(PaySplit split){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),split);
				return;
			}
			Crud.PaySplitCrud.Update(split);
		}

		///<summary></summary>
		public static void Update(PaySplit paySplit,PaySplit oldPaySplit) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),paySplit,oldPaySplit);
				return;
			}
			Crud.PaySplitCrud.Update(paySplit,oldPaySplit);
		}

		public static void UpdateFSplitNum(long paySplitNumOrig,long paySplitNumLinked) {
			if(paySplitNumLinked==0) {
				//There is no such thing as a paysplit with a PK of 0.
				//There are reports of this query running a lot at HQ which is making replication fall behind (probably due to MyISAM table level locking).
				return;
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),paySplitNumOrig,paySplitNumLinked);
				return;
			}
			string command="UPDATE paysplit SET FSplitNum="+POut.Long(paySplitNumOrig)
				+" WHERE SplitNum="+POut.Long(paySplitNumLinked);
			Db.NonQ(command);
		}

		///<summary>Takes a procedure and updates the provnum of each of the paysplits attached.
		///Does nothing if there are no paysplits attached to the passed-in procedure.</summary>
		public static void UpdateAttachedPaySplits(Procedure proc) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),proc);
				return;
			}
			Db.NonQ($@"UPDATE paysplit SET ProvNum = {POut.Long(proc.ProvNum)} WHERE ProcNum = {POut.Long(proc.ProcNum)}");
		}

		///<summary>Unlinks all paysplits that are currently linked to the passed-in adjustment. (Sets paysplit.AdjNum to 0)</summary>
		public static void UnlinkForAdjust(Adjustment adj) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),adj);
				return;
			}
			Db.NonQ($@"UPDATE paysplit SET AdjNum = 0 WHERE AdjNum = {POut.Long(adj.AdjNum)}");
		}

		///<summary>Updates the provnum of all paysplits for a supplied adjustment.  Supply a list of splits to use that instead of querying the database.</summary>
		public static void UpdateProvForAdjust(Adjustment adj,List<PaySplit> listSplits=null) {
			if(listSplits!=null && listSplits.Count==0) {
				return;
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),adj,listSplits);
				return;
			}
			if(listSplits==null) {
				Db.NonQ($@"UPDATE paysplit SET ProvNum = {POut.Long(adj.ProvNum)} WHERE AdjNum = {POut.Long(adj.AdjNum)}");
			}
			else {
				Db.NonQ($@"UPDATE paysplit SET ProvNum = {POut.Long(adj.ProvNum)}
					WHERE SplitNum IN({string.Join(",",listSplits.Select(x => POut.Long(x.SplitNum)))})");
			}
		}

		///<summary>Inserts, updates, or deletes db rows to match listNew.</summary>
		public static bool Sync(List<PaySplit> listNew,List<PaySplit> listOld) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listNew,listOld);
			}
			//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
			return Crud.PaySplitCrud.Sync(listNew,listOld,Security.CurUser.UserNum);
		}
		#endregion

		#region Delete
		///<summary>Deletes the paysplit.</summary>
		public static void Delete(PaySplit split){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),split);
				return;
			}
			string command= "DELETE from paysplit WHERE SplitNum = "+POut.Long(split.SplitNum);
 			Db.NonQ(command);
		}

		///<summary>Used from payment window AutoSplit button to delete paysplits when clicking AutoSplit more than once.</summary>
		public static void DeleteForPayment(long payNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),payNum);
				return;
			}
			string command="DELETE FROM paysplit"
				+" WHERE PayNum="+POut.Long(payNum);
			Db.NonQ(command);
		}
		#endregion

		#endregion

		#region Misc Methods
		///<summary>Returns true if a paysplit is attached to the associated procnum. Returns false otherwise.</summary>
		public static bool IsPaySplitAttached(long procNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),procNum);
			}
			string command="SELECT COUNT(*) FROM paysplit WHERE ProcNum="+POut.Long(procNum);
			if(Db.GetCount(command)=="0") {
				return false;
			}
			return true;
		}

		///<summary>This method takes care of creating all splits for payment plan charges whether they be Auto, Manual, or Partial splits.
		///For AutoSplits, set isAutoSplit to true and pass 0 into payAmt.
		///For manual and partial splits, set isAutoSplit=false and pass the amount that the user typed into the Payment window into the method as payAmt.
		///For manual and partial splits, if payAmt != 0, then only pay up to payAmt for that charge. Otherwise, pay the entire charge.
		///This also takes care of splitting to procedures, only paying up to the amount that the procedure is attached to the payment plan.</summary>
		public static List<PaySplit> CreateSplitForPayPlan(long payNum,double payAmtAvail,AccountEntry payPlanEntry,List<PayPlanCharge> listPayPlanCredits, 
			List<AccountEntry> listAccountCharges,decimal payAmt,bool isAutoSplit,out decimal paymentCurPayAmt) 
		{
			//No remoting role check; no call to db. Plus this method has an out parameter.
			paymentCurPayAmt=(decimal)payAmtAvail;//begins as original payment amount and gets decremented as paysplits get applied.
			List<PaySplit> listSplits=new List<PaySplit>();
			DateTime today=new DateTime(DateTime.Today.Year,DateTime.Today.Month,DateTime.Today.Day,0,0,0,DateTimeKind.Unspecified);
			//if the payAmt is > 0 and it's NOT an autosplit, we only want to pay up to the payAmt.
			bool isPartial=(payAmt > 0) && !isAutoSplit;
			PayPlanCharge payPlanChargeCur=(PayPlanCharge)payPlanEntry.Tag;
			//amtUnattached will also include principal payments if those payments are not allocated to a procedure. 
			//This is accounted for when creating an unattached split.
			double amtUnattachedAlreadyPaid=(double)payPlanEntry.SplitCollection.Where(x => x.ProcNum == 0).Sum(y => y.AccountEntryAmtPaid);
			double amtAttachedAlreadyPaid=(double)payPlanEntry.SplitCollection.Where(x => x.ProcNum != 0).Sum(y => y.AccountEntryAmtPaid);
			//Below is the logic of this method: For the passed in charge
			//create a split for the amount of the interest
			//create a split for the amount of the charge, attach it to the procedure if possible.
			//interest split, only create if interest hasn't already been paid.
			#region Create Interest Split
			if(amtUnattachedAlreadyPaid < payPlanChargeCur.Interest) {
				PaySplit interest=new PaySplit();
				interest.DatePay=today;
				//the split should always go to the payplancharge's guarantor.
				interest.PatNum=payPlanChargeCur.Guarantor;
				interest.ProvNum=payPlanChargeCur.ProvNum;
				if(PrefC.HasClinicsEnabled) {//Clinics
					interest.ClinicNum=payPlanChargeCur.ClinicNum;
				}
				interest.PayPlanNum=payPlanChargeCur.PayPlanNum;
				interest.PayNum=payNum;
				//if it's an autoSplit, then only use up to the global PaymentAmt.
				//else if it's a partial split, then only use up to payAmt.
				//else it's a manual split, so just add a split for the entire charge.
				if(isAutoSplit) {
					interest.SplitAmt=Math.Min(payPlanChargeCur.Interest-amtUnattachedAlreadyPaid,(double)paymentCurPayAmt);
					paymentCurPayAmt-=(decimal)interest.SplitAmt;
				}
				else if(isPartial) {
					interest.SplitAmt=Math.Min(payPlanChargeCur.Interest-amtUnattachedAlreadyPaid,(double)payAmt);
					payAmt-=(decimal)interest.SplitAmt;
					paymentCurPayAmt-=(decimal)interest.SplitAmt;
				}
				else {
					interest.SplitAmt=payPlanChargeCur.Interest-amtUnattachedAlreadyPaid;
				}
				//this is so the paysplit says (interest) in the grid.
				interest.IsInterestSplit=true;
				payPlanEntry.AmountEnd-=(decimal)interest.SplitAmt;
				payPlanEntry.SplitCollection.Add(interest);
				listSplits.Add(interest);
			}
			#endregion
			//get all procs associated to this plan
			List<AccountEntry> listPayPlanProcsEntries=new List<AccountEntry>();
			foreach(PayPlanCharge payPlanCredit in listPayPlanCredits) {//For each credit
				foreach(AccountEntry entry in listAccountCharges) {//Go through account entries
					if(entry.GetType()!=typeof(Procedure) || (entry.GetType()==typeof(Procedure) && ((Procedure)entry.Tag).ProcStatus!=ProcStat.C)) {
						continue;//If it's not a  completed proc
					}
					Procedure proc=(Procedure)entry.Tag;
					if(proc.ProcNum==payPlanCredit.ProcNum) {//See if the proc has the credit's procnum
						listPayPlanProcsEntries.Add(entry);//if so, add it so we then have a list of payplan's procs, and we can also show how much is left due for it
					}
				}
			}
			listPayPlanProcsEntries=listPayPlanProcsEntries.OrderBy(x => x.Date).ToList();
			if(amtAttachedAlreadyPaid < payPlanChargeCur.Principal) {
				if(listPayPlanProcsEntries.Count>0) {
					double amtAvail=(double)payPlanEntry.AmountEnd;
					foreach(AccountEntry entryProc in listPayPlanProcsEntries) {
						#region Create Attached Split to Proc
						if(isAutoSplit && paymentCurPayAmt == 0) {
							break;
						}
						Procedure procCur=(Procedure)entryProc.Tag;
						PaySplit procSplit=new PaySplit();
						//get the amount of this procedure that's attached to this payment plan
						double maxSplitForCurrentProc=listPayPlanCredits.Where(x => x.ProcNum == procCur.ProcNum).Sum(y => y.Principal);
						//get the amount of this procedure that's already been paid off in this payment plan. Could be negative due to a previous bug. 
						maxSplitForCurrentProc-=entryProc.SplitCollection.Where(x => x.PayPlanNum == payPlanChargeCur.PayPlanNum).Sum(y => y.SplitAmt);
						if(maxSplitForCurrentProc<=0) {
							continue;
						}
						procSplit.DatePay=today;
						//the payment should always go to the account of the payplancharge's guarantor.
						procSplit.PatNum=payPlanChargeCur.Guarantor;
						procSplit.PayPlanNum=payPlanChargeCur.PayPlanNum;
						procSplit.PayNum=payNum;
						if(isAutoSplit) {
							//make a split for the procedure for no more than the sum of all the credits on the payment plan for that procedure.
							procSplit.SplitAmt=Math.Min(maxSplitForCurrentProc,Math.Min(amtAvail,(double)paymentCurPayAmt));
							paymentCurPayAmt-=(decimal)procSplit.SplitAmt;
						}
						else if(isPartial) {
							if(payAmt == 0) {
								break;
							}
							procSplit.SplitAmt=Math.Min(maxSplitForCurrentProc,Math.Min(amtAvail,(double)payAmt));
							payAmt-=(decimal)procSplit.SplitAmt;
							paymentCurPayAmt-=(decimal)procSplit.SplitAmt;
						}
						else {
							procSplit.SplitAmt=Math.Min((double)maxSplitForCurrentProc,amtAvail); //make a split for the procedure for no more than the sum of all the credits on the payment plan for that procedure.
						}
						procSplit.ProvNum=procCur.ProvNum;
						procSplit.ClinicNum=procCur.ClinicNum;
						procSplit.ProcNum=procCur.ProcNum;
						amtAvail-=procSplit.SplitAmt;
						entryProc.SplitCollection.Add(procSplit);
						payPlanEntry.AmountEnd-=(decimal)procSplit.SplitAmt;
						if(procSplit.SplitAmt.IsGreaterThan(0)) {
							payPlanEntry.SplitCollection.Add(procSplit);
							listSplits.Add(procSplit);
						}
						#endregion
					}
					#region Create Unattached Split
					//if they have a mix of attached and unattached credits: (if there are payplan charges not linked to a procedure)
					bool doCreateUnattachedSplit=payPlanEntry.AmountEnd > 0 && ((isAutoSplit && Math.Min(amtAvail,(double)paymentCurPayAmt) > 0) 
						|| (!isAutoSplit && Math.Min(amtAvail,(double)payAmt) > 0));
					if(doCreateUnattachedSplit) {
						PaySplit unattachedSplit=new PaySplit();
						unattachedSplit.DatePay=today;
						//the payment should always go to the account of the payplancharge's guarantor.
						unattachedSplit.PatNum=payPlanChargeCur.Guarantor;
						unattachedSplit.PayPlanNum=payPlanChargeCur.PayPlanNum;
						unattachedSplit.PayNum=payNum;
						if(isAutoSplit) {
							unattachedSplit.SplitAmt=Math.Min(amtAvail,(double)paymentCurPayAmt);
							paymentCurPayAmt-=(decimal)unattachedSplit.SplitAmt;
						}
						else if(isPartial) {
							unattachedSplit.SplitAmt=Math.Min(amtAvail,(double)payAmt);
							payAmt-=(decimal)unattachedSplit.SplitAmt;
							paymentCurPayAmt-=(decimal)unattachedSplit.SplitAmt;
						}
						else {
							unattachedSplit.SplitAmt=amtAvail;
						}
						unattachedSplit.ProvNum=payPlanEntry.ProvNum;
						unattachedSplit.ClinicNum=payPlanEntry.ClinicNum;
						unattachedSplit.ProcNum=0;
						amtAvail-=unattachedSplit.SplitAmt;
						payPlanEntry.AmountEnd-=(decimal)unattachedSplit.SplitAmt;
						payPlanEntry.SplitCollection.Add(unattachedSplit);
						listSplits.Add(unattachedSplit);
					}
					#endregion
				}
				else {
					#region Create Unattached Split
					PaySplit unattachedSplit = new PaySplit();
					//for payplans with no attached procedures, all paysplits would end up in the interestAlreadyPaid amount. 
					//any extra money that was accidentally lumped into interestAlreadyPaid should be included in principleAlreadyPaid here.
					amtAttachedAlreadyPaid=(amtUnattachedAlreadyPaid-payPlanChargeCur.Interest>0) ? amtUnattachedAlreadyPaid-payPlanChargeCur.Interest : 0;
					unattachedSplit.DatePay=today;
					//the payment should always go to the account of the payplancharge's guarantor.
					unattachedSplit.PatNum=payPlanChargeCur.Guarantor;
					unattachedSplit.PayPlanNum=payPlanChargeCur.PayPlanNum;
					unattachedSplit.PayNum=payNum;
					if(isAutoSplit) {
						unattachedSplit.SplitAmt=Math.Min((double)paymentCurPayAmt,payPlanChargeCur.Principal-amtAttachedAlreadyPaid);
						paymentCurPayAmt-=(decimal)unattachedSplit.SplitAmt;
					}
					else if(isPartial) {
						unattachedSplit.SplitAmt=Math.Min((double)payAmt,payPlanChargeCur.Principal-amtAttachedAlreadyPaid);
						payAmt-=(decimal)unattachedSplit.SplitAmt;
						paymentCurPayAmt-=(decimal)unattachedSplit.SplitAmt;
					}
					else {
						unattachedSplit.SplitAmt=payPlanChargeCur.Principal-amtAttachedAlreadyPaid;
					}
					unattachedSplit.ProvNum=payPlanChargeCur.ProvNum;
					unattachedSplit.ClinicNum=payPlanChargeCur.ClinicNum;
					payPlanEntry.AmountEnd-=(decimal)unattachedSplit.SplitAmt;
					payPlanEntry.SplitCollection.Add(unattachedSplit);
					listSplits.Add(unattachedSplit);
					#endregion
				}
			}
			return listSplits;
		}

		///<summary>This method takes care of creating all splits for payment plan charges whether they be Auto, Manual, or Partial splits.
		///For AutoSplits, set isAutoSplit to true and pass 0 into payAmt.
		///For manual and partial splits, set isAutoSplit=false and pass the amount that the user typed into the Payment window into the method as payAmt.
		///For manual and partial splits, if payAmt != 0, then only pay up to payAmt for that charge. Otherwise, pay the entire charge.
		///This also takes care of splitting to procedures, only paying up to the amount that the procedure is attached to the payment plan.
		///payAmtAvailRemaining will be set to the amount that is leftover (still needs to be split).</summary>
		public static List<PaySplit> CreateSplitsForDynamicPayPlan(long payNum,double payAmtAvail,AccountEntry payPlanEntry
			,List<AccountEntry> listAccountCharges,decimal payAmt,bool isAutoSplit,out decimal payAmtAvailRemaining) 
		{
			//No remoting role check; no call to db. Plus this method has an out parameter.
			payAmtAvailRemaining=(decimal)payAmtAvail;//begins as original payment amount and gets decremented as paysplits get applied.
			List<PaySplit> listSplits=new List<PaySplit>();
			DateTime today=new DateTime(DateTime.Today.Year,DateTime.Today.Month,DateTime.Today.Day,0,0,0,DateTimeKind.Unspecified);
			//if the payAmt is > 0 and it's NOT an autosplit, we only want to pay up to the payAmt.
			bool isPartial=(payAmt > 0) && !isAutoSplit;
			PayPlanCharge payPlanChargeCur=(PayPlanCharge)payPlanEntry.Tag;
			//Below is the logic of this method: For the passed in charge
			//create a split for the amount of the interest
			//create a split for the amount of the charge, attach it to the procedure if possible.
			AccountEntry productionEntry=null;
			if(payPlanChargeCur.LinkType==PayPlanLinkType.Procedure) {
				productionEntry=listAccountCharges.FirstOrDefault(x => x.PriKey==payPlanChargeCur.FKey && x.GetType()==typeof(Procedure));
			}
			else if(payPlanChargeCur.LinkType==PayPlanLinkType.Adjustment) {
				productionEntry=listAccountCharges.FirstOrDefault(x => x.PriKey==payPlanChargeCur.FKey && x.GetType()==typeof(Adjustment));
			}
			if(productionEntry==null) {
				return listSplits;
			}
			#region Make Interest Split
			if(payPlanChargeCur.Interest>0) {
				//check to see if an interest split has already been made for this payment plan charge
				//We can determine an interest split if it has no attachment to any production type
				List<PaySplit> listInterest=GetForPayPlanCharges(new List<long> {payPlanChargeCur.PayPlanChargeNum})
					.Where(x => x.ProcNum==0 && x.AdjNum==0).ToList();
				double interestNeedingPayment=payPlanChargeCur.Interest-listInterest.Sum(x => x.SplitAmt);
				if(interestNeedingPayment.IsGreaterThanZero()) {
					PaySplit interest=new PaySplit();
					interest.DatePay=today;
					//the split should always go to the payplancharge's guarantor.
					interest.PatNum=payPlanChargeCur.Guarantor;
					interest.ProvNum=payPlanChargeCur.ProvNum;
					if(PrefC.HasClinicsEnabled) {//Clinics
						interest.ClinicNum=payPlanChargeCur.ClinicNum;
					}
					interest.PayPlanNum=payPlanChargeCur.PayPlanNum;
					interest.PayPlanChargeNum=payPlanChargeCur.PayPlanChargeNum;
					interest.PayNum=payNum;
					if(isAutoSplit) {//if it's an autoSplit, then only use up to the global PaymentAmt.
						interest.SplitAmt=Math.Min(interestNeedingPayment,(double)payAmtAvailRemaining);
						payAmtAvailRemaining-=(decimal)interest.SplitAmt;
					}
					else if(isPartial) {//else if it's a partial split, then only use up to payAmt.
						interest.SplitAmt=Math.Min(interestNeedingPayment,(double)payAmt);
						payAmt-=(decimal)interest.SplitAmt;
						payAmtAvailRemaining-=(decimal)interest.SplitAmt;
					}
					else {//else it's a manual split, so just add a split for the entire charge.
						interest.SplitAmt=interestNeedingPayment;
					}
					//this is so the paysplit says (interest) in the grid.
					interest.IsInterestSplit=true;
					payPlanEntry.AmountEnd-=(decimal)interest.SplitAmt;
					payPlanEntry.SplitCollection.Add(interest);
					listSplits.Add(interest);
				}
			}
			#endregion
			double amtAvail=(double)payPlanEntry.AmountEnd;
			while(amtAvail > 0) {
				//need to check the amount still needing to be paid for this production - comes from amount original - sum paysplits attached
				if(isAutoSplit && payAmtAvailRemaining==0) {
					break;
				}
				PaySplit split=new PaySplit();
				split.DatePay=today;
				split.PatNum=payPlanChargeCur.Guarantor;//the payment should always go to the account of the payplancharge's guarantor.
				split.PayPlanNum=payPlanChargeCur.PayPlanNum;
				split.PayPlanChargeNum=payPlanChargeCur.PayPlanChargeNum;
				split.PayNum=payNum;
				if(isAutoSplit) {
					split.SplitAmt=Math.Min(amtAvail,(double)payAmtAvailRemaining);
					payAmtAvailRemaining-=(decimal)split.SplitAmt;
				}
				else if(isPartial) {
					if(payAmt==0) {
						break;
					}
					split.SplitAmt=Math.Min(amtAvail,(double)payAmt);
					payAmt-=(decimal)split.SplitAmt;
					payAmtAvailRemaining-=(decimal) split.SplitAmt;
				}
				else {
					split.SplitAmt=amtAvail;
				}
				split.ProvNum=payPlanEntry.ProvNum;
				split.ClinicNum=payPlanEntry.ClinicNum;
				//paysplits cannot be attached to both an adjustment and a procedure. If our credit is for an adjustment that is attached to a procedure...
				//if an adjustment is attached to a procedure we don't allow you to pay just the adjustment. You have to pay the procedure.
				if(payPlanChargeCur.LinkType==PayPlanLinkType.Procedure) {
					split.ProcNum=payPlanChargeCur.FKey;
				}
				if(split.ProcNum==0 && payPlanChargeCur.LinkType==PayPlanLinkType.Adjustment) {//unattached adjustment
					split.AdjNum=payPlanChargeCur.FKey;//AdjNum
				}
				amtAvail-=split.SplitAmt;
				productionEntry.SplitCollection.Add(split);
				payPlanEntry.AmountEnd-=(decimal)split.SplitAmt;
				if(split.SplitAmt.IsGreaterThan(0)) {
					payPlanEntry.SplitCollection.Add(split);
					listSplits.Add(split);
				}
			}
			return listSplits;
		}

		public static string GetSecurityLogMsgDelete(PaySplit paySplit,Payment payment=null) {
			return $"Paysplit deleted for: {Patients.GetLim(paySplit.PatNum).GetNameLF()}, {paySplit.SplitAmt.ToString("c")}, with payment type "
				+$"'{Payments.GetPaymentTypeDesc(payment??Payments.GetPayment(paySplit.PayNum))}'";
		}
		#endregion

		///<summary>There were reports of this object failing to serialize when being passed to the middle tier.
		///Consider moving this class outside of the PaySplits class?  This works in debug but fails for customers.  See job #6069.</summary>
		[Serializable]
		public class PaySplitAssociated {
			///<summary>Paysplit that PaySplitLinked has FSplitNum for.</summary>
			public PaySplit PaySplitOrig;
			///<summary>Paysplit that is linked to PaySplitOrig</summary>
			public PaySplit PaySplitLinked;

			///<summary>Necessary for serialization.</summary>
			public PaySplitAssociated() { }

			public PaySplitAssociated(PaySplit paySplitOrig,PaySplit paySplitLinked) {
				//assign passed-in values
				PaySplitOrig=paySplitOrig;
				PaySplitLinked=paySplitLinked;
			}

			///<summary>Returns a copy of this PaySplitAssociated.</summary>
			public PaySplitAssociated Copy() {
				return (PaySplitAssociated)this.MemberwiseClone();
			}

			///<summary>Checks TagOD to see if a given PaySplit is in a PaySplitAssociated object.</summary>
			public bool ContainsSplit(PaySplit split) {
				//Null check first as it is valid to have null PaySplits in a PaySplitAssociated object.
				if((PaySplitOrig!=null && PaySplitOrig.IsSame(split)) || (PaySplitLinked!=null && PaySplitLinked.IsSame(split))) {
					return true;
				}
				return false;
			}
		}

	}
}










