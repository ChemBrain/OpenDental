using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using CodeBase;
using Avalara.AvaTax.RestClient;

namespace OpenDentBusiness{
	///<summary>Handles database commands related to the adjustment table in the db.</summary>
	public class Adjustments {
		#region Get Methods
		public static List<Adjustment> GetMany(List<long> listAdjNums) {
			if(listAdjNums.IsNullOrEmpty()) {
				return new List<Adjustment>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Adjustment>>(MethodBase.GetCurrentMethod(),listAdjNums);
			}
			string command=$"SELECT * FROM adjustment WHERE adjustment.AdjNum IN ({string.Join(",",listAdjNums.Select(x => POut.Long(x)))})";
			return Crud.AdjustmentCrud.SelectMany(command);
		}
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#region Insert

		#endregion
		#region Udate

		#endregion
		#region Delete
		#endregion
		#endregion
		#region Misc Methods
		#endregion

		///<summary>Gets all adjustments for a single patient.</summary>
		public static Adjustment[] Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Adjustment[]>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command=
				"SELECT * FROM adjustment"
				+" WHERE PatNum = "+POut.Long(patNum)+" ORDER BY AdjDate";
			return Crud.AdjustmentCrud.SelectMany(command).ToArray();
		}

		///<summary>Gets one adjustment from the db.</summary>
		public static Adjustment GetOne(long adjNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Adjustment>(MethodBase.GetCurrentMethod(),adjNum);
			}
			string command=
				"SELECT * FROM adjustment"
				+" WHERE AdjNum = "+POut.Long(adjNum);
			return Crud.AdjustmentCrud.SelectOne(adjNum);
		}

		///<summary>Gets the amount used for the specified adjustment (Sums paysplits that have AdjNum passed in).  Pass in PayNum to exclude splits on that payment.</summary>
		public static double GetAmtAllocated(long adjNum,long excludedPayNum,List<PaySplit> listSplits=null) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDouble(MethodBase.GetCurrentMethod(),adjNum,excludedPayNum,listSplits);
			}
			if(listSplits!=null) {
				return listSplits.FindAll(x => x.PayNum!=excludedPayNum).Sum(x => x.SplitAmt);
			}
			else { 
				string command="SELECT SUM(SplitAmt) FROM paysplit WHERE AdjNum="+POut.Long(adjNum);
				if(excludedPayNum!=0) {
					command+=" AND PayNum!="+POut.Long(excludedPayNum);
				}
				return PIn.Double(Db.GetScalar(command));
			}
		}

		public static void DetachFromInvoice(long statementNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),statementNum);
				return;
			}
			string command="UPDATE adjustment SET StatementNum=0 WHERE StatementNum="+POut.Long(statementNum)+"";
			Db.NonQ(command);
		}

		public static void DetachAllFromInvoices(List<long> listStatementNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listStatementNums);
				return;
			}
			if(listStatementNums==null || listStatementNums.Count==0) {
				return;
			}
			string command="UPDATE adjustment SET StatementNum=0 WHERE StatementNum IN ("+string.Join(",",listStatementNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

		///<summary>Gets all negative or positive adjustments for a patient depending on how isPositive is set.</summary>
		public static List<Adjustment> GetAdjustForPats(List<long> listPatNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Adjustment>>(MethodBase.GetCurrentMethod(),listPatNums);
			}
			string command="SELECT * FROM adjustment "
				+"WHERE PatNum IN("+String.Join(", ",listPatNums)+") ";
			return Crud.AdjustmentCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void Update(Adjustment adj){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),adj);
				return;
			}
			Crud.AdjustmentCrud.Update(adj);
			CreateOrUpdateSalesTaxIfNeeded(adj);
		}

		///<summary></summary>
		public static long Insert(Adjustment adj) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				adj.AdjNum=Meth.GetLong(MethodBase.GetCurrentMethod(),adj);
				return adj.AdjNum;
			}
			//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
			adj.SecUserNumEntry=Security.CurUser.UserNum;
			long adjNum=Crud.AdjustmentCrud.Insert(adj);
			CreateOrUpdateSalesTaxIfNeeded(adj); //Do the update after the insert so the AvaTax API can include the new adjustment in the calculation
			return adjNum;
		}

		///<summary>This will soon be eliminated or changed to only allow deleting on same day as EntryDate.</summary>
		public static void Delete(Adjustment adj){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),adj);
				return;
			}
			Crud.AdjustmentCrud.Delete(adj.AdjNum);
			CreateOrUpdateSalesTaxIfNeeded(adj);
			PaySplits.UnlinkForAdjust(adj);
		}

		///<summary>Loops through the supplied list of adjustments and returns an ArrayList of adjustments for the given proc.</summary>
		public static ArrayList GetForProc(long procNum,Adjustment[] List) {
			//No need to check RemotingRole; no call to db.
			ArrayList retVal=new ArrayList();
			for(int i=0;i<List.Length;i++){
				if(List[i].ProcNum==procNum){
					retVal.Add(List[i]);
				}
			}
			return retVal;
		}

		public static List<Adjustment> GetForProcs(List<long> listProcNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Adjustment>>(MethodBase.GetCurrentMethod(),listProcNums);
			}
			List<Adjustment> listAdjustments=new List<Adjustment>();
			if(listProcNums==null || listProcNums.Count < 1) {
				return listAdjustments;
			}
			string command="SELECT * FROM adjustment WHERE ProcNum IN("+string.Join(",",listProcNums)+")";
			return Crud.AdjustmentCrud.SelectMany(command);
		}

		///<summary>Returns the sales tax adjustment attached to this procedure with a TaxTransID. We do not use the AvaTax.SalesTaxAdjType in case the
		///defnum has changed in the past. </summary>
		public static Adjustment GetSalesTaxForProc(long procNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Adjustment>(MethodBase.GetCurrentMethod(),procNum);
			}
			string command="SELECT * FROM adjustment WHERE ProcNum="+POut.Long(procNum)+" AND AdjType="+POut.Long(AvaTax.SalesTaxAdjType);
			return Crud.AdjustmentCrud.SelectMany(command).FirstOrDefault(x => x.AdjType==AvaTax.SalesTaxAdjType);
		}

		///<summary>Sums all adjustments for a proc then returns that sum. Pass false to canIncludeTax in order to exclude sales tax from the end amount.
		///</summary>
		public static double GetTotForProc(long procNum,bool canIncludeTax=true) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDouble(MethodBase.GetCurrentMethod(),procNum,canIncludeTax);
			}
			string command="SELECT SUM(AdjAmt) FROM adjustment"
				+" WHERE ProcNum="+POut.Long(procNum);
			if(AvaTax.IsEnabled() && !canIncludeTax) {
				command+=" AND AdjType NOT IN ("+string.Join(",",POut.Long(AvaTax.SalesTaxAdjType),POut.Long(AvaTax.SalesTaxReturnAdjType))+")";
			}
			return PIn.Double(Db.GetScalar(command));
		}

		///<summary></summary>
		public static double GetTotTaxForProc(Procedure proc) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDouble(MethodBase.GetCurrentMethod(),proc);
			}
			if(!AvaTax.DoSendProcToAvalara(proc)) {
				return 0;
			}
			string command="SELECT SUM(AdjAmt) FROM adjustment"
				+" WHERE ProcNum="+POut.Long(proc.ProcNum)
				+" AND AdjType IN ("+string.Join(",",POut.Long(AvaTax.SalesTaxAdjType),POut.Long(AvaTax.SalesTaxReturnAdjType))+")";
			return PIn.Double(Db.GetScalar(command));
		}

		///<summary>Creates a new discount adjustment for the given procedure.</summary>
		public static void CreateAdjustmentForDiscount(Procedure procedure) {
			//No need to check RemotingRole; no call to db.
			Adjustment AdjustmentCur=new Adjustment();
			AdjustmentCur.DateEntry=DateTime.Today;
			AdjustmentCur.AdjDate=DateTime.Today;
			AdjustmentCur.ProcDate=procedure.ProcDate;
			AdjustmentCur.ProvNum=procedure.ProvNum;
			AdjustmentCur.PatNum=procedure.PatNum;
			AdjustmentCur.AdjType=PrefC.GetLong(PrefName.TreatPlanDiscountAdjustmentType);
			AdjustmentCur.ClinicNum=procedure.ClinicNum;
			AdjustmentCur.AdjAmt=-procedure.Discount;//Discount must be negative here.
			AdjustmentCur.ProcNum=procedure.ProcNum;
			Insert(AdjustmentCur);
			TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(AdjustmentCur);
		}

		///<summary>Creates a new discount adjustment for the given procedure using the discount plan fee.</summary>
		public static void CreateAdjustmentForDiscountPlan(Procedure procedure) {
			//No need to check RemotingRole; no call to db.
			DiscountPlan discountPlan=DiscountPlans.GetPlan(Patients.GetPat(procedure.PatNum).DiscountPlanNum);
			if(discountPlan==null) {
				return;//No discount plan.
			}
			//Figure out how much the patient saved and make an adjustment for the difference so that the office find how much money they wrote off.
			double discountAmt=Fees.GetAmount(procedure.CodeNum,discountPlan.FeeSchedNum,procedure.ClinicNum,procedure.ProvNum);
			if(discountAmt==-1) {
				return;//No fee entered, don't make adjustment.
			}
			double adjAmt=procedure.ProcFee-discountAmt;
			if(adjAmt <= 0) {
				return;//We do not need to create adjustments for 0 dollars.
			}
			Adjustment adjustmentCur=new Adjustment();
			adjustmentCur.DateEntry=DateTime.Today;
			adjustmentCur.AdjDate=DateTime.Today;
			adjustmentCur.ProcDate=procedure.ProcDate;
			adjustmentCur.ProvNum=procedure.ProvNum;
			adjustmentCur.PatNum=procedure.PatNum;
			adjustmentCur.AdjType=discountPlan.DefNum;
			adjustmentCur.ClinicNum=procedure.ClinicNum;
			adjustmentCur.AdjAmt=(-adjAmt);
			adjustmentCur.ProcNum=procedure.ProcNum;
			Insert(adjustmentCur);
			TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(adjustmentCur);
			SecurityLogs.MakeLogEntry(Permissions.AdjustmentCreate,procedure.PatNum,"Adjustment made for discount plan: "+adjustmentCur.AdjAmt.ToString("f"));
		}

		///<summary>(HQ Only) Automatically creates or updates a sales tax adjustment for the passted in procedure. If an adjustment is passed in, we go 
		///ahead and update that adjustment, otherwise we check if there is already a sales tax adjustment for the given procedure and if not, we create
		///a new one. Pass in false to doCalcTax if we have already called the AvaTax API to get the tax estimate recently to avoid redundant calls
		///(currently only pre-payments uses this flag).
		///isRepeatCharge indicates if the adjustment is being inserted by the repeat charge tool, currently only used to supress error messages
		///in the Avatax API.</summary>
		public static void CreateOrUpdateSalesTaxIfNeeded(Procedure procedure,Adjustment salesTaxAdj=null,bool doCalcTax=true,bool isRepeatCharge=false) {
			if(!AvaTax.DoSendProcToAvalara(procedure,isRepeatCharge)) { //tests isHQ
				return;
			}
			//Check for middle tier as crud is called below
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),procedure,salesTaxAdj,doCalcTax,isRepeatCharge);
				return;
			}
			if(salesTaxAdj==null) {
				salesTaxAdj=Adjustments.GetSalesTaxForProc(procedure.ProcNum);
			}
			//If we didn't find any existing adjustments to modify, create an adjustment instead
			if(salesTaxAdj==null) { 
				salesTaxAdj=new Adjustment();
				salesTaxAdj.DateEntry=DateTime.Today;
				salesTaxAdj.AdjDate=procedure.ProcDate;
				salesTaxAdj.ProcDate=procedure.ProcDate;
				salesTaxAdj.ProvNum=procedure.ProvNum;
				salesTaxAdj.PatNum=procedure.PatNum;
				salesTaxAdj.AdjType=AvaTax.SalesTaxAdjType;
				salesTaxAdj.ClinicNum=procedure.ClinicNum;
				salesTaxAdj.ProcNum=procedure.ProcNum;
			}
			//if the sales tax adjustment is locked, create a sales tax refund adjustment instead
			if(procedure.ProcDate <= AvaTax.TaxLockDate) {
				CreateSalesTaxRefundIfNeeded(procedure,salesTaxAdj);
				return;
			}
			if(!doCalcTax) { //Should only ever happen for pre-payments, where we've already called the api to get the tax amount
				salesTaxAdj.AdjAmt=procedure.TaxAmt;
				Insert(salesTaxAdj);
			}
			else if(AvaTax.DidUpdateAdjustment(procedure,salesTaxAdj)) {
				string note=DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToShortTimeString()+": Tax amount changed from $"+procedure.TaxAmt.ToString("f2")+" to $"+salesTaxAdj.AdjAmt.ToString("f2");
				if(!(procedure.TaxAmt-salesTaxAdj.AdjAmt).IsZero()) {
					procedure.TaxAmt=salesTaxAdj.AdjAmt;
					Crud.ProcedureCrud.Update(procedure);
				}
				if(salesTaxAdj.AdjNum==0) {
					//The only way to get salesTaxAdj.AdjAmt=0 when AvaTax.DidUpdateAdjustment() returns true is if there was an error.
					if(isRepeatCharge && salesTaxAdj.AdjAmt==0) {//this is an error; we would normally not save a new adjustment with amt $0
						throw new ODException("Encountered an error communicating with AvaTax.  Skip for repeating charges only.  "+salesTaxAdj.AdjNote);
					}
					Insert(salesTaxAdj);//This could be an error or a new adjustment/repeating charge, either way we want to insert
				}
				else { //updating an existing adjustment. We don't need to check isRepeatCharge because of 
					if(!string.IsNullOrWhiteSpace(salesTaxAdj.AdjNote)) {
						salesTaxAdj.AdjNote+=Environment.NewLine;
					}
					salesTaxAdj.AdjNote+=note; //If we are updating this adjustment, leave a note indicating what changed
					Update(salesTaxAdj);
				}
			}
			TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(salesTaxAdj);
		}

		public static void CreateSalesTaxRefundIfNeeded(Procedure procedure, Adjustment adjustmentExistingLocked) {
			Adjustment adjustmentSalesTaxReturn=new Adjustment();
			adjustmentSalesTaxReturn.DateEntry=DateTime.Today;
			adjustmentSalesTaxReturn.AdjDate=DateTime.Today;
			adjustmentSalesTaxReturn.ProcDate=procedure.ProcDate;
			adjustmentSalesTaxReturn.ProvNum=procedure.ProvNum;
			adjustmentSalesTaxReturn.PatNum=procedure.PatNum;
			adjustmentSalesTaxReturn.AdjType=AvaTax.SalesTaxReturnAdjType;
			adjustmentSalesTaxReturn.ClinicNum=procedure.ClinicNum;
			adjustmentSalesTaxReturn.ProcNum=procedure.ProcNum;
			if(AvaTax.DoCreateReturnAdjustment(procedure,adjustmentExistingLocked,adjustmentSalesTaxReturn)) {
				Insert(adjustmentSalesTaxReturn);
				TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(adjustmentSalesTaxReturn);
			}
		}

		///<summary>(HQ Only) When we create, modify, or delete a non-sales tax adjustment that is attached to a procedure, we may also need to update
		///sales tax for that procedure. Takes a non-sales tax adjustment and checks if the procedure already has a sales tax adjustment. If one already
		///exists, calculate the new tax after the change to the passed in adjustment, and update the sales tax accordingly. If not, calculate and 
		///create a sales tax adjustment only if the procedure is taxable.</summary>
		public static void CreateOrUpdateSalesTaxIfNeeded(Adjustment modifiedAdj) {
			if(AvaTax.IsEnabled() && modifiedAdj.ProcNum>0 && modifiedAdj.AdjType!=AvaTax.SalesTaxAdjType && modifiedAdj.AdjType!=AvaTax.SalesTaxReturnAdjType) {
				Adjustment taxAdjForProc=GetSalesTaxForProc(modifiedAdj.ProcNum);
				if(taxAdjForProc!=null) {
					Procedure proc=Procedures.GetOneProc(modifiedAdj.ProcNum,false);
					CreateOrUpdateSalesTaxIfNeeded(proc,taxAdjForProc);
				}
			}
		}

		///<summary>Deletes all adjustments for a procedure</summary>
		public static void DeleteForProcedure(long procNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),procNum);
				return;
			}
			//Create log for each adjustment that is going to be deleted.
			string command="SELECT * FROM adjustment WHERE ProcNum = "+POut.Long(procNum); //query for all adjustments of a procedure 
			List<Adjustment> listAdjustments=Crud.AdjustmentCrud.SelectMany(command);
			for(int i=0;i<listAdjustments.Count;i++) { //loops through the rows
				SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,listAdjustments[i].PatNum, //and creates audit trail entry for every row to be deleted
				"Delete adjustment for patient: "
				+Patients.GetLim(listAdjustments[i].PatNum).GetNameLF()+", "
				+(listAdjustments[i].AdjAmt).ToString("c"),0,listAdjustments[i].SecDateTEdit);
			}
			//Delete each adjustment for the procedure.
			command="DELETE FROM adjustment WHERE ProcNum = "+POut.Long(procNum);
			Db.NonQ(command);
		}

		/// <summary>Returns a DataTable of adjustments of a given adjustment type and for a given pat</summary>
		public static List<Adjustment> GetAdjustForPatByType(long patNum,long adjType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Adjustment>>(MethodBase.GetCurrentMethod(),patNum,adjType);
			}
			string queryBrokenApts="SELECT * FROM adjustment WHERE PatNum="+POut.Long(patNum)
				+" AND AdjType="+POut.Long(adjType);
			return Crud.AdjustmentCrud.SelectMany(queryBrokenApts);
		}

		/// <summary>Returns a dictionary of adjustments of a given adjustment type and for the given pats such that the key is the patNum.
		/// Every patNum given will exist as key with a list in the returned dictonary.
		/// Only considers adjs where AdjDate is strictly less than the given maxAdjDate.</summary>
		public static SerializableDictionary<long,List<Adjustment>> GetAdjustForPatsByType(List<long> listPatNums,long adjType,DateTime maxAdjDate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SerializableDictionary<long,List<Adjustment>>>(MethodBase.GetCurrentMethod(),listPatNums,adjType,maxAdjDate);
			}
			if(listPatNums==null || listPatNums.Count==0) {
				return new SerializableDictionary<long, List<Adjustment>>();
			}
			string queryBrokenApts="SELECT * FROM adjustment "
				+"WHERE PatNum IN ("+string.Join(",",listPatNums)+") "
				+"AND AdjType="+POut.Long(adjType)+" "
				+"AND "+DbHelper.DateTConditionColumn("AdjDate",ConditionOperator.LessThan,maxAdjDate);
			List<Adjustment> listAdjs=Crud.AdjustmentCrud.SelectMany(queryBrokenApts);
			SerializableDictionary<long,List<Adjustment>> retVal=new SerializableDictionary<long,List<Adjustment>>();
			foreach(long patNum in listPatNums) {
				retVal[patNum]=listAdjs.FindAll(x => x.PatNum==patNum);
			}
			return retVal;
		}

		///<summary>Used from ContrAccount and ProcEdit to display and calculate adjustments attached to procs.</summary>
		public static double GetTotForProc(long procNum,Adjustment[] List,long excludedNum=0) {
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			for(int i=0;i<List.Length;i++){
				if((List[i].AdjNum==excludedNum)) {
					continue;
				}
				if(List[i].ProcNum==procNum){
					retVal+=List[i].AdjAmt;
				}
			}
			return retVal;
		}

		/*
		///<summary>Must make sure Refresh is done first.  Returns the sum of all adjustments for this patient.  Amount might be pos or neg.</summary>
		public static double ComputeBal(Adjustment[] List){
			double retVal=0;
			for(int i=0;i<List.Length;i++){
				retVal+=List[i].AdjAmt;
			}
			return retVal;
		}*/

		///<summary>Returns the number of finance or billing charges deleted.</summary>
		public static long UndoFinanceOrBillingCharges(DateTime dateUndo,bool isBillingCharges) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),dateUndo,isBillingCharges);
			}
			string adjTypeStr="Finance";
			long adjTypeDefNum=PrefC.GetLong(PrefName.FinanceChargeAdjustmentType);
			if(isBillingCharges) {
				adjTypeStr="Billing";
				adjTypeDefNum=PrefC.GetLong(PrefName.BillingChargeAdjustmentType);
			}
			string command="SELECT adjustment.AdjAmt,patient.PatNum,patient.Guarantor,patient.LName,patient.FName,patient.Preferred,patient.MiddleI,"
				+"adjustment.SecDateTEdit "
				+"FROM adjustment "
				+"INNER JOIN patient ON patient.PatNum=adjustment.PatNum "
				+"WHERE AdjDate="+POut.Date(dateUndo)+" "
				+"AND AdjType="+POut.Long(adjTypeDefNum);
			DataTable table=Db.GetTable(command);
			List<Action> listActions=new List<Action>();
			int loopCount=0;
			foreach(DataRow row in table.Rows) {//loops through the rows and creates audit trail entry for every row to be deleted
				listActions.Add(new Action(() => {
					SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,PIn.Long(row["PatNum"].ToString()),
						"Delete adjustment for patient, undo "+adjTypeStr.ToLower()+" charges: "
						+Patients.GetNameLF(row["LName"].ToString(),row["FName"].ToString(),row["Preferred"].ToString(),row["MiddleI"].ToString())
						+", "+PIn.Double(row["AdjAmt"].ToString()).ToString("c"),0,PIn.DateT(row["SecDateTEdit"].ToString()));
					if(++loopCount%5==0) {
						BillingEvent.Fire(ODEventType.Billing,Lans.g("FinanceCharge","Creating log entries for "+adjTypeStr.ToLower()+" charges")
							+": "+loopCount+" out of "+table.Rows.Count);
					}
				}));
			}
			ODThread.RunParallel(listActions,TimeSpan.FromMinutes(2));
			command="DELETE FROM adjustment WHERE AdjDate="+POut.Date(dateUndo)+" AND AdjType="+POut.Long(adjTypeDefNum);
			BillingEvent.Fire(ODEventType.Billing,Lans.g("FinanceCharge","Deleting")+" "+table.Rows.Count+" "
				+Lans.g("FinanceCharge",adjTypeStr.ToLower()+" charge adjustments")+"...");
			return Db.NonQ(command);
		}

		///<summary>Returns the sum of adjustments for the date range for the passed in operatories or providers.</summary>
		public static decimal GetAdjustAmtForAptView(DateTime dateStart,DateTime dateEnd,long clinicNum,List<long> listOpNums,List<long> listProvNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<decimal>(MethodBase.GetCurrentMethod(),dateStart,dateEnd,clinicNum,listOpNums,listProvNums);
			}
			string command=GetQueryAdjustmentsForAppointments(dateStart,dateEnd,listOpNums,doGetSum:true);
			if(!listProvNums.IsNullOrEmpty()) {
				command+="AND adjustment.ProvNum IN("+string.Join(",",listProvNums.Select(x => POut.Long(x)))+") ";
			}
			if(clinicNum>0 && PrefC.HasClinicsEnabled) {
				command+="AND adjustment.ClinicNum="+POut.Long(clinicNum);
			}
			return PIn.Decimal(Db.GetScalar(command));
		}

		///<summary>Returns a query string used to get adjustments for all patients who have an appointment in the date range and in one of the operatories
		///passed in.</summary>
		public static string GetQueryAdjustmentsForAppointments(DateTime dateStart,DateTime dateEnd,List<long> listOpNums,bool doGetSum) {
			//No need to check RemotingRole; no call to db.
			if(listOpNums.IsNullOrEmpty()) {
				return "SELECT "+(doGetSum ? "SUM(adjustment.AdjAmt)" : "*")
					+" FROM adjustment WHERE AdjDate BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)+" ";
			}
			string command="SELECT "
				+(doGetSum ? "SUM(adjustment.AdjAmt)" : "*")
				+" FROM adjustment WHERE AdjDate BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)
					+" AND PatNum IN("
						+"SELECT PatNum FROM appointment "
							+"WHERE AptDateTime BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd.AddDays(1))
							+ "AND AptStatus IN ("+POut.Int((int)ApptStatus.Scheduled)
							+", "+POut.Int((int)ApptStatus.Complete)
							+", "+POut.Int((int)ApptStatus.Broken)
							+", "+POut.Int((int)ApptStatus.PtNote)
							+", "+POut.Int((int)ApptStatus.PtNoteCompleted)+")"
							+" AND Op IN("+string.Join(",",listOpNums)+")) ";
			return command;
		}
	}

	


	


}










