using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using CodeBase;

namespace OpenDentBusiness {
	public class PaymentEdit {

		#region FormPayment
		///<summary>Gets most all the data needed to load FormPayment.</summary>
		public static LoadData GetLoadData(Patient patCur,Payment paymentCur,List<long> listPatNumsFamily,bool isNew,bool isIncomeTxfr) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<LoadData>(MethodBase.GetCurrentMethod(),patCur,paymentCur,listPatNumsFamily,isNew,isIncomeTxfr);
			}
			LoadData data=new LoadData();
			data.SuperFam=new Family(Patients.GetBySuperFamily(patCur.SuperFamily));
			data.ListCreditCards=CreditCards.Refresh(patCur.PatNum);
			data.XWebResponse=XWebResponses.GetOneByPaymentNum(paymentCur.PayNum);
			data.PayConnectResponseWeb=PayConnectResponseWebs.GetOneByPayNum(paymentCur.PayNum);
			data.TableBalances=Patients.GetPaymentStartingBalances(patCur.Guarantor,paymentCur.PayNum);
			data.TableBalances.TableName="TableBalances";
			data.ListSplits=PaySplits.GetForPayment(paymentCur.PayNum);
			data.ListPaySplitAllocations=PaySplits.GetSplitsForPrepay(data.ListSplits);
			if(!isNew) {
				data.Transaction=Transactions.GetAttachedToPayment(paymentCur.PayNum);
			}
			data.ListValidPayPlans=PayPlans.GetValidPlansNoIns(patCur.PatNum);
			//If this payment contains splits for other superfamily members, add the superfamily members to listPatNumsFamily (which we use later to generate charges)
			if(data.SuperFam.ListPats.Length>1) {//Always has current patient.
				foreach(PaySplit split in data.ListSplits) {
					if(!listPatNumsFamily.Contains(split.PatNum) && data.SuperFam.ListPats.Select(x => x.PatNum).Contains(split.PatNum)) {
						listPatNumsFamily.AddRange(data.SuperFam.ListPats.Select(x => x.PatNum));
						break;//This means that it is a historical payment and has splits for family members that are outside the current family.  Add the family members, no need to continue
					}
				}
			}
			//if there were no payment plans found where this patient is the guarantor, find payment plans in the family.
			if(data.ListValidPayPlans.Count == 0) {
				//Do not include insurance payment plans.
				data.ListValidPayPlans=PayPlans.GetForPats(listPatNumsFamily,patCur.PatNum)
					.Where(x => !x.IsClosed)
					.Where(x => x.PlanNum==0).ToList();
			}
			data.ListAssociatedPatients=Patients.GetAssociatedPatients(patCur.PatNum);
			data.ListPrePaysForPayment=PaySplits.GetSplitsLinked(data.ListSplits);
			data.ListProcsForSplits=Procedures.GetManyProc(data.ListSplits.Select(x => x.ProcNum).ToList(),false);
			data.ConstructChargesData=GetConstructChargesData(listPatNumsFamily,patCur.PatNum,data.ListSplits,paymentCur.PayNum,isIncomeTxfr);
			List<Procedure> listTpProcs=data.ConstructChargesData.ListProcsCompleted.Where(x => x.ProcStatus==ProcStat.TP).ToList();
			LoadDiscountPlanInfo(ref data,listTpProcs);
			return data;
		}

		/// <summary>Performs load functionality. Fills dictPatients with PatNums and Patients of patients that are associated to patCurNum.
		///Gets all patients that are in the same family or super family as patCurNum and that patCurNum is the payplan guarantor of.
		///Pass in load data to keep current objects instead of refreshing.</summary>
		public static InitData Init(List<Patient> listPats,Family fam,Family superFam,Payment payCur,List<PaySplit> listSplitsCur
			,List<AccountEntry> listEntriesLoading,long patCurNum,Dictionary<long,Patient> dictPatients=null,bool isIncomeTxfr=false,bool isPatPrefer=false
			,LoadData loadData=null,bool doAutoSplit=true,bool doIncludeExplicitCreditsOnly=false) 
		{
			//No remoting role check; no call to db
			InitData initData=new InitData();
			initData.DictPats=dictPatients;
			//get patients who have this patient's guarantor as their payplan's guarantor
			List<Patient> listPatients=listPats;
			listPatients.AddRange(fam.ListPats);
			if(superFam.ListPats!=null) {
				listPatients.AddRange(superFam.ListPats);
			}
			//Add patients with paysplits on this payment
			listPatients.AddRange(Patients.GetLimForPats(listSplitsCur.Select(x => x.PatNum).Where(x => listPatients.All(y => y.PatNum!=x)).ToList()));
			if(initData.DictPats==null) {
				initData.DictPats=new Dictionary<long,Patient>();
			}
			listPatients=listPatients.DistinctBy(x => x.PatNum).ToList();
			foreach(Patient pat in listPatients) {
				initData.DictPats[pat.PatNum]=pat;
			}
			//This logic will ensure that regardless of if it's a new, or old payment any created paysplits that haven't been saved, 
			//such as if splits were made in this window then the window was closed and then reopened, will persist.
			initData.SplitTotal=0;
			for(int i=0;i<listSplitsCur.Count;i++) {
				initData.SplitTotal+=(decimal)listSplitsCur[i].SplitAmt;
			}
			payCur.PayAmt=Math.Round(payCur.PayAmt-(double)initData.SplitTotal,3);
			initData.AutoSplitData=AutoSplitForPayment(listPatients.Select(x => x.PatNum).ToList(),patCurNum,listSplitsCur,payCur,listEntriesLoading
				,isIncomeTxfr,isPatPrefer,loadData,doAutoSplit,doIncludeExplicitCreditsOnly);
			listSplitsCur.AddRange(initData.AutoSplitData.ListAutoSplits);
			initData.AutoSplitData.ListSplitsCur=listSplitsCur;//handle getting the full list here for init instead of getting the auto splits and adding splits.
			if(listPatients.Select(x => x.PatNum).ToList().Any(x => !initData.DictPats.ContainsKey(x))) {
				Patients.GetLimForPats(listPatients.Select(x => x.PatNum).ToList()
					.FindAll(x => !initData.DictPats.ContainsKey(x)))
					.ForEach(x => initData.DictPats[x.PatNum]=x);
			}
			return initData;
		}
		#endregion

		#region constructing and linking charges and credits
		///<summary>Gets the data needed to construct a list of charges on FormPayment.</summary>
		public static ConstructChargesData GetConstructChargesData(List<long> listPatNums,long patNum,List<PaySplit> listSplitsCur,long payCurNum
			,bool isIncomeTransfer) 
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ConstructChargesData>(MethodBase.GetCurrentMethod(),listPatNums,patNum,listSplitsCur,payCurNum,isIncomeTransfer);
			}
			ConstructChargesData data=new ConstructChargesData();
			data.ListPaySplits=PaySplits.GetForPats(listPatNums);//Might contain payplan payments.
			data.ListProcsCompleted=Procedures.GetCompleteForPats(listPatNums);//will also contain TP procs if pref is set to ON
			if(!isIncomeTransfer) {
				//sum ins pay as totals by claims to include the value for the ones that have already been transferred and taken care of. 
				data.ListInsPayAsTotal=ClaimProcs.GetOutstandingClaimPayByTotal(listPatNums);
			}
			data.ListPayPlans=PayPlans.GetForPats(listPatNums,patNum);//Used to figure out how much we need to pay off procs with, also contains ins payplans
			data.ListPayPlanCharges=new List<PayPlanCharge>();
			data.ListPaySplitsPayPlan=new List<PaySplit>();
			if(isIncomeTransfer) {
				//do not show pre-payment splits that are attached to TP procedures, they should not be transferred since they are already reserved money
				//only TP unearned should have an unearned type and a procNum. 
				data.ListPaySplits.RemoveAll(x => x.UnearnedType!=0 && x.ProcNum!=0 && !x.ProcNum.In(data.ListProcsCompleted.Select(y => y.ProcNum)));
			}
			if(PrefC.GetYN(PrefName.PrePayAllowedForTpProcs) && !isIncomeTransfer) {
				data.ListProcsCompleted.AddRange(Procedures.GetTpForPats(listPatNums));
			}
			if(data.ListPayPlans.Count>0) {
				List<long> listPayPlanNums=data.ListPayPlans.Select(x => x.PayPlanNum).ToList();
				//get list where payplan guar is not in the fam)
				data.ListPaySplitsPayPlan=PaySplits.GetForPayPlans(listPayPlanNums);
				data.ListPayPlanCharges=PayPlanCharges.GetDueForPayPlans(listPayPlanNums,listPatNums);//Does not get charges for the future.
				data.ListPayPlanLinks=PayPlanLinks.GetForPayPlans(listPayPlanNums);
				data.ListPaySplits.AddRange(data.ListPaySplitsPayPlan
					.Where(x => !data.ListPaySplits.Any(y => y.SplitNum==x.SplitNum)).ToList());
			}
			//Look for splits that are in the database yet have been deleted from the pay splits grid.
			for(int i=data.ListPaySplits.Count-1;i>=0;i--) {
				bool isFound=listSplitsCur.Any(x => x.SplitNum==data.ListPaySplits[i].SplitNum);
				if(!isFound && data.ListPaySplits[i].PayNum==payCurNum) {
					//If we have a split that's not found in the passed-in list of splits for the payment
					//and the split we got from the DB is on this payment, remove it because the user must have deleted the split from the payment window.
					//The payment window won't update the DB with the change until it's closed.
					data.ListPaySplits.RemoveAt(i);
				}
			}
			//In the rare case that a Payment Plan has a Gaurantor outside of the Patient's family, we want to make sure to collect more 
			//data for Adjustments and ClaimProcs so that debits and credits balance propperly.
			if(data.ListPayPlans.Any(x => x.Guarantor!=x.PatNum && !x.PatNum.In(listPatNums))) {
				data.ListAdjustments=Adjustments.GetForProcs(data.ListProcsCompleted.Select(x => x.ProcNum).ToList());
				//still does not contain PayAsTotals
				data.ListClaimProcsFiltered=ClaimProcs.GetForProcs(data.ListProcsCompleted.FindAll(x => x.ProcNum!=0).Select(x => x.ProcNum).ToList());
			}
			else {//Otherwise get the smaller dataset as it will suffice for our needs.
				data.ListAdjustments=Adjustments.GetAdjustForPats(listPatNums);
				data.ListClaimProcsFiltered=ClaimProcs.Refresh(listPatNums).Where(x => x.ProcNum!=0).ToList();//does not contain PayAsTotals
			}
			return data;
		}

		/// <summary>Helper method that does the entire original auto split for payment. Gets the charges, and links the credits.</summary>
		public static ConstructResults ConstructAndLinkChargeCredits(List<long> listPatNums,long patCurNum,List<PaySplit> listSplitsCur,Payment payCur
			,List<AccountEntry> listEntriesLoading,bool isIncomeTxfr=false,bool isPreferCurPat=false,LoadData loadData=null
			,bool doIncludeExplicitCreditsOnly=false,bool doShowHiddenSplits=false)
		{
			//No remoting role check; no call to db
			ConstructResults retVal=new ConstructResults();
			retVal.Payment=payCur;
			#region Get data
			//Get the lists of items we'll be using to calculate with.
			ConstructChargesData constructChargesData=loadData?.ConstructChargesData
				??GetConstructChargesData(listPatNums,patCurNum,listSplitsCur,retVal.Payment.PayNum,isIncomeTxfr);
			List<Procedure> listProcs=constructChargesData.ListProcsCompleted;//filled from db. List of completed procs for patient(s).
			List<Adjustment> listAdjustments=constructChargesData.ListAdjustments;
			List<PayAsTotal> listInsPayAsTotals=constructChargesData.ListInsPayAsTotal;//Claimprocs paid as total, might contain ins payplan payments.
			//Used to figure out how much we need to pay off procs with, also contains insurance payplans.
			List<PayPlan> listPayPlans=constructChargesData.ListPayPlans;
			List<PayPlanCharge> listPayPlanCharges=constructChargesData.ListPayPlanCharges;
			List<PayPlanLink> listPayPlanLinks=constructChargesData.ListPayPlanLinks;
			List<ClaimProc> listClaimProcsFiltered=constructChargesData.ListClaimProcsFiltered;
			#endregion
			#region Construct List of Charges
			ConstructListChargesResult constructChargeResult=ConstructListCharges(listPayPlanCharges,listPayPlans,listAdjustments,listProcs
				,constructChargesData.ListPaySplits,listInsPayAsTotals,isIncomeTxfr,doShowHiddenSplits);
			retVal.ListAccountCharges=constructChargeResult.ListAccountEntries;
			retVal.ListPayPlanCharges=constructChargeResult.ListPayPlanCharges;
			retVal.ListAccountCharges.Sort(AccountEntrySort);
			#endregion
			#region Explicitly Link Credits
			SplitCollection listSplitsCurrentAndHistoric=new SplitCollection();
			//This ordering is necessary so parents come before their children when explicitly linking credits.
			listSplitsCurrentAndHistoric.AddRange(constructChargesData.ListPaySplits.OrderBy(x => x.DatePay).ThenBy(x => x.FSplitNum).ToList());
			listSplitsCurrentAndHistoric.AddRange(listSplitsCur.Where(x => x.SplitNum==0).Select(y => y.Copy()).ToList());
			retVal.ListAccountCharges=ExplicitlyLinkCredits(retVal,listSplitsCurrentAndHistoric,listClaimProcsFiltered
				,listAdjustments.Where(x => x.ProcNum!=0).ToList(),listPayPlans,listPayPlanLinks,isIncomeTxfr,loadData);
			#endregion
			#region Implicitly Link Credits
			if(!isIncomeTxfr && !doIncludeExplicitCreditsOnly) {//If this payment is an income transfer, do NOT use unallocated income to pay off charges.
				PayResults implicitResult=ImplicitlyLinkCredits(constructChargesData.ListPaySplits,listAdjustments,listInsPayAsTotals
					,retVal.ListAccountCharges,listSplitsCur,listEntriesLoading,retVal.Payment,patCurNum,isPreferCurPat);
				retVal.ListAccountCharges=implicitResult.ListAccountCharges;
				retVal.ListSplitsCur=implicitResult.ListSplitsCur;
				retVal.Payment=implicitResult.Payment;
			}
			#endregion Implicitly Link Credits
			return retVal;
		}

		public static ConstructListChargesResult ConstructListCharges(List<PayPlanCharge> listPayPlanCharges,List<PayPlan>listPayPlans
			,List<Adjustment>listAdjustments,List<Procedure>listProcs,List<PaySplit>listPaySplits,List<PayAsTotal>listInsPayAsTotal,bool hasPayTypeNone
			,bool doShowHiddenSplits) 
		{
			//No remoting role check; no call to db
			ConstructListChargesResult retVal=new ConstructListChargesResult();
			List<AccountEntry> listCharges=new List<AccountEntry>();
			List<PayPlanCharge> listChargesToRemove=new List<PayPlanCharge>();
			foreach(PayPlanCharge ppc in listPayPlanCharges) {
				PayPlan pp=listPayPlans.Find(x => x.PayPlanNum==ppc.PayPlanNum);
				if(!(pp.IsDynamic) && pp.IsClosed && PrefC.IsPayPlanVersion2) {
					listChargesToRemove.Add(ppc);//payment plan is no longer active, we will pay any remaining balance to the procedure directly. 
					continue;
				}
				if(ppc.ChargeType==PayPlanChargeType.Debit && pp.PlanNum==0 && ppc.Principal.IsGreaterThan(0)) {
					listCharges.Add(new AccountEntry(ppc));//only make account charges for positive debits on patient plans.
				}
			}
			retVal.ListPayPlanCharges=listPayPlanCharges.Except(listChargesToRemove).ToList();
			for(int i=0;i<listAdjustments.Count;i++) {
				if((listAdjustments[i].ProcNum==0 && listAdjustments[i].AdjAmt>0) || hasPayTypeNone) {//If there is no procnum on the adjustment, add it to charges if it's charge adjustment, or if it's income transfer mode add them all regardless.
					listCharges.Add(new AccountEntry(listAdjustments[i]));
				}
			}
			for(int i=0;i<listProcs.Count;i++) {
				listCharges.Add(new AccountEntry(listProcs[i]));
			}
			if(hasPayTypeNone) {
				List<long> listHiddenUnearnedTypes=PaySplits.GetHiddenUnearnedDefNums();
				for(int i=listPaySplits.Count-1;i>=0;i--) {
					//If we are hiding hidden splits and current split doesn't have hidden type OR we are not hiding hidden splits, add split to listCharges
					if(doShowHiddenSplits || !listHiddenUnearnedTypes.Contains(listPaySplits[i].UnearnedType)){
						//In Income Transfer mode, add all paysplits to the buckets.  
						//The income transfers made previously should balance out any adjustments/inspaytotals that were transferred previously.
						listCharges.Add(new AccountEntry(listPaySplits[i]));
					}
				}
				foreach(PayAsTotal totalPmt in listInsPayAsTotal) {//Ins pay totals need to be added to the sum total for income transfers
					listCharges.Add(new AccountEntry(totalPmt));
				}
			}
			retVal.ListAccountEntries=listCharges;
			return retVal;
		}

		public static List<AccountEntry> ExplicitlyLinkCredits(ConstructResults dataSet,SplitCollection listSplitsCurrentAndHistoric
			,List<ClaimProc> listClaimProcs,List<Adjustment> listAdjusts,List<PayPlan> listPayPlans,List<PayPlanLink> listPayPlanLinks,
			bool isIncomeTransfer=false, LoadData loadData=null) 
		{
			//No remoting role check; no call to db
			List<AccountEntry> listAccountCharges=dataSet.ListAccountCharges;
			List<PayPlanCharge> listPayPlanCharges=dataSet.ListPayPlanCharges;
			List<PayPlanProductionEntry> listPayPlanProductionEntries=new List<PayPlanProductionEntry>();
			listPayPlanProductionEntries=PayPlanProductionEntry.GetProductionForLinks(listPayPlanLinks);
			long payNum=dataSet.Payment.PayNum;
			if(loadData==null) {
				List<Procedure> listTpProcs=listAccountCharges.Where(x => x.Tag is Procedure)
					.Select(x => (Procedure)x.Tag).Where(x => x.ProcStatus==ProcStat.TP).ToList();
				LoadDiscountPlanInfo(ref loadData,listTpProcs);
			}
			//Explicitly link any of the credits to their corresponding charges if a link can be made. (ie. PaySplit.ProcNum to a Procedure.ProcNum)
			//Any payment plan credits for procedures should get applied to that procedure and removed from the credit total bucket.
			foreach(AccountEntry chargeCur in listAccountCharges) {
				if(chargeCur.Tag.GetType() == typeof(Procedure)) {
					List<Adjustment> listAdjustmentsForProc=new List<Adjustment>();//list of adjustments that will be directly applied to the procedure
					if(isIncomeTransfer) {//Only apply adjustments that have matching pat/prov/clinic values to the procedure.
						listAdjustmentsForProc=listAdjusts.FindAll(x => x.ProcNum==chargeCur.PriKey && x.ProvNum==chargeCur.ProvNum && x.PatNum==chargeCur.PatNum 
							&& x.ClinicNum==chargeCur.ClinicNum);
					}
					else {//Not transfer. Use adjustments to directly modify procedure value regardless
						listAdjustmentsForProc=listAdjusts.FindAll(x => x.ProcNum==chargeCur.PriKey);
					}
					Procedure curProc=(Procedure)chargeCur.Tag;
					//If the patient has a discount plan and this is a TP proc, use the discount plan fee for the AmountStart
					if(curProc.ProcStatus==ProcStat.TP
						&& loadData.DictPatToDPFeeScheds!=null
						&& loadData.DictPatToDPFeeScheds.TryGetValue(chargeCur.PatNum,out long dpFeeSchedNum)
						&& dpFeeSchedNum!=0) {
						decimal amtStart=(decimal)Fees.GetAmount(curProc.CodeNum,dpFeeSchedNum,curProc.ClinicNum,curProc.ProvNum,loadData.ListFeesForDiscountPlans);
						if(amtStart.IsEqual(-1)) { //Use the UCR fee if there is no provided fee in the Discount Plan
							amtStart=ClaimProcs.GetPatPortion((Procedure)chargeCur.Tag,listClaimProcs,listAdjustmentsForProc);
						}
						//Insurance does not need to be considered because a patient cannot have a discount plan and insurance at the same time
						chargeCur.AmountStart=amtStart;
						chargeCur.AmountEnd=amtStart;
					}
					else {
						chargeCur.AmountStart=ClaimProcs.GetPatPortion((Procedure)chargeCur.Tag,listClaimProcs,listAdjustmentsForProc);
						chargeCur.AmountEnd=chargeCur.AmountStart;
					}
					decimal sumCreditsForProc=0;
					if(isIncomeTransfer) {
						List<long> listValidAdjNums=listAdjustmentsForProc.Select(x => x.AdjNum).ToList();
						foreach(AccountEntry adjEntry in listAccountCharges.FindAll(x => x.GetType()==typeof(Adjustment) && x.PriKey.In(listValidAdjNums))) {
							//remove value from the adjustment entries since they were just applied to the procedure in GetPatPortion()
							adjEntry.AmountStart=0;
							adjEntry.AmountEnd=0;
						}
						sumCreditsForProc=(decimal)listPayPlanCharges
						.Where(x => x.ChargeType==PayPlanChargeType.Credit && x.ProcNum==chargeCur.PriKey && x.ProvNum==chargeCur.ProvNum 
							&& x.PatNum==chargeCur.PatNum && x.ClinicNum==chargeCur.ClinicNum)
						.Sum(x => x.Principal);
						//dynamic payment plans store credits differently and the user may have both
						sumCreditsForProc+=listPayPlanProductionEntries.Where(x => x.ProductionTag.GetType()==typeof(Procedure) 
						&& ((Procedure)x.ProductionTag).ProcNum==chargeCur.PriKey && x.ProvNum==chargeCur.ProvNum && x.PatNum==chargeCur.PatNum 
						&& x.ClinicNum==chargeCur.ClinicNum).Sum(x => x.AmountOverride==0?x.AmountOriginal:x.AmountOverride);
					}
					else {
						sumCreditsForProc=(decimal)listPayPlanCharges
						.Where(x => x.ChargeType==PayPlanChargeType.Credit && x.ProcNum==chargeCur.PriKey)
						.Sum(x => x.Principal);
						sumCreditsForProc+=listPayPlanProductionEntries.Where(x => x.ProductionTag.GetType()==typeof(Procedure) 
						&& ((Procedure)x.ProductionTag).ProcNum==chargeCur.PriKey).Sum(x => x.AmountOverride==0?x.AmountOriginal:x.AmountOverride);
					}
					//attaching more credits than the procedure is worth will apply that credit to account charges below (after Explicitly Link Credits region end)
					chargeCur.AmountOriginal-=sumCreditsForProc;
					chargeCur.AmountStart-=sumCreditsForProc;
					chargeCur.AmountEnd-=sumCreditsForProc;
				}
				if(chargeCur.Tag.GetType()==typeof(Adjustment) && ((Adjustment)chargeCur.Tag).ProcNum==0) {
					//this section is to specifically handle adjustments with no procedure that are attached to dynamic payment plans.
					decimal sumCreditsForAdj=0;
					sumCreditsForAdj=listPayPlanProductionEntries.Where(x => x.ProductionTag.GetType()==typeof(Adjustment)
						&& ((Adjustment)x.ProductionTag).AdjNum==chargeCur.PriKey).Sum(x => x.AmountOverride==0 ? x.AmountOriginal : x.AmountOverride);
					chargeCur.AmountOriginal-=sumCreditsForAdj;
					chargeCur.AmountStart-=sumCreditsForAdj;
					chargeCur.AmountEnd-=sumCreditsForAdj;
				}
				//Link payplan adjustments to their payplan charge (linked by date) to reduce the amount on the charge
				if(chargeCur.Tag.GetType()==typeof(PayPlanCharge)) {
					PayPlan payplan=listPayPlans.Find(x => x.PayPlanNum==((PayPlanCharge)chargeCur.Tag).PayPlanNum);
					//find all negative adjustments for charge.
					List<PayPlanCharge> listNegatives=listPayPlanCharges.FindAll(x => x.ChargeDate==((PayPlanCharge)chargeCur.Tag).ChargeDate
							&& x.PayPlanNum==payplan.PayPlanNum && x.IsDebitAdjustment);
					if(listNegatives.Count>0) {
						decimal sumAdjustmentsForCharge=Math.Abs((decimal)listNegatives.Sum(x => x.Principal));//stored as negative debit
						//modify the principal amt to apply the adjustment to the charge
						//users can modify adj to be more than the corresponding charge. We would not expect accuracy, but safeguard so it does not break stuff
						chargeCur.AmountStart-=Math.Min(sumAdjustmentsForCharge,(decimal)((PayPlanCharge)chargeCur.Tag).Principal);
						chargeCur.AmountEnd-=sumAdjustmentsForCharge;
					}
				}
			}
			//Procedures First
			//Existing Payments on the payment plan
			foreach(PaySplit splitCur in listSplitsCurrentAndHistoric) {
				foreach(AccountEntry chargeCur in listAccountCharges) {
					if(splitCur.SplitAmt==0) {//necessary for payplancharges to split and track the correct amount
						break;
					}
					//We want to explicitly pay the procedure if the payment plan is closed and on version 2, but don't if it is open or on version 1,3.
					bool doAttachSplitForPayPlan=listPayPlans.Any(x => x.PayPlanNum==splitCur.PayPlanNum && x.IsClosed && !x.IsDynamic) && PrefC.IsPayPlanVersion2;
					//======== NOTE: Any explicitly linked paysplit needs to be used on what it's attached to in its entirety (even if it's overpaid). =========  
					//Overpayments are taken care of later.
					//if the account entry is a procedure and the split is attached to it, then explicitly apply the payment to the procedure.
					if(chargeCur.Tag.GetType() == typeof(Procedure) && splitCur.ProcNum==chargeCur.PriKey && (splitCur.PayPlanNum==0 || doAttachSplitForPayPlan)
						&& (!isIncomeTransfer
							|| (isIncomeTransfer && chargeCur.ProvNum==splitCur.ProvNum && chargeCur.PatNum==splitCur.PatNum && chargeCur.ClinicNum==splitCur.ClinicNum))) 
					{
						decimal amtStart=(decimal)splitCur.SplitAmt;//Overpayment on procedures is handled later
						if(splitCur.PayNum!=payNum) {
							chargeCur.AmountStart-=amtStart;
						}
						PaySplit splitForCollection=splitCur.Copy();//take copy so we can get amtPaid without overwriting.
						splitForCollection.AccountEntryAmtPaid=(decimal)amtStart;
						chargeCur.AmountEnd-=amtStart;
						chargeCur.SplitCollection.Add(splitForCollection);
						splitCur.SplitAmt-=(double)amtStart;
						AccountEntry acctEntry=listAccountCharges.Find(x => x.GetType()==typeof(PaySplit) && x.SplitCollection.Contains(splitCur));
						if(acctEntry==null) {
							break;
						}
						acctEntry.AmountOriginal+=amtStart;
						acctEntry.AmountStart+=amtStart;
						acctEntry.AmountEnd+=amtStart;
						break;
					}
					//else if the account entry is a payplancharge, then explicitly apply it to that payplancharge
					else if(chargeCur.Tag.GetType()==typeof(PayPlanCharge)
						&& splitCur.PayPlanNum==((PayPlanCharge)chargeCur.Tag).PayPlanNum
						&& ((PayPlanCharge)chargeCur.Tag).ChargeType==PayPlanChargeType.Debit
						&& chargeCur.AmountEnd>0
						&& (!isIncomeTransfer
							|| (isIncomeTransfer && chargeCur.ProvNum==splitCur.ProvNum && (chargeCur.PatNum==splitCur.PatNum || ((PayPlanCharge)chargeCur.Tag).Guarantor==splitCur.PatNum) && chargeCur.ClinicNum==splitCur.ClinicNum))) 
					{  //If this payplancharge is for the pat or guarantor of a payplan, and the split is for either one, use the split to pay off payplan.
						double amt=Math.Min(splitCur.SplitAmt,(double)chargeCur.AmountEnd);
						if(splitCur.PayNum!=payNum) {
							chargeCur.AmountStart-=(decimal)amt;
						}
						PaySplit splitForCollection=splitCur.Copy();//take copy so we can get amtPaid without overwriting (for autosplitting payplancharges)
						splitForCollection.AccountEntryAmtPaid=(decimal)amt;
						if(isIncomeTransfer) {
							chargeCur.AmountOriginal-=(decimal)amt;
						}
						chargeCur.AmountEnd-=(decimal)amt;
						splitCur.SplitAmt-=amt;
						//add a copy of the paysplit to the charge so that we can keep track AccountEntryAmtPaid.
						chargeCur.SplitCollection.Add(splitForCollection);
						//also add to the linked procedure's split collection so we can keep track of which procedure to link to depending on amount paid. 
						listAccountCharges.Where(x => x.GetType()==typeof(Procedure) && x.PriKey==splitForCollection.ProcNum)
							.ForEach(x => x.SplitCollection.Add(splitForCollection));
						AccountEntry negEntry=listAccountCharges.Find(x => x.GetType()==typeof(PaySplit) && x.PriKey==splitCur.SplitNum);
						if(negEntry!=null) {
							negEntry.AmountOriginal=0;
							negEntry.AmountStart=0;
							negEntry.AmountEnd=0;
						}
					}
					else if(chargeCur.Tag.GetType()==typeof(Adjustment)
						&& (splitCur.AdjNum==chargeCur.PriKey || (splitCur.ProcNum!=0 && splitCur.ProcNum==((Adjustment)chargeCur.Tag).ProcNum))
						&& (splitCur.PayPlanNum==0 || doAttachSplitForPayPlan)
						&& chargeCur.ProvNum==splitCur.ProvNum && chargeCur.PatNum==splitCur.PatNum && chargeCur.ClinicNum==splitCur.ClinicNum) 
					{
						decimal amtStart=(decimal)splitCur.SplitAmt;
						if(splitCur.PayNum!=payNum) {
							chargeCur.AmountStart-=amtStart;
						}
						PaySplit splitForCollection=splitCur.Copy();//take copy so we can get amtPaid without overwriting.
						splitForCollection.AccountEntryAmtPaid=(decimal)amtStart;
						chargeCur.AmountEnd-=amtStart;
						chargeCur.SplitCollection.Add(splitForCollection);
						splitCur.SplitAmt-=(double)amtStart;
						AccountEntry acctEntry=listAccountCharges.Find(x => x.GetType()==typeof(PaySplit) && x.SplitCollection.Contains(splitCur));
						if(acctEntry==null) {
							break;
						}
						acctEntry.AmountOriginal+=amtStart;
						acctEntry.AmountStart+=amtStart;
						acctEntry.AmountEnd+=amtStart;
						break;
					}
					else if(chargeCur.Tag.GetType()==typeof(PaySplit)//PaySplits will only ever be in the charge list if it's in income transfer mode.
						&& chargeCur.AmountEnd!=0 && chargeCur.PriKey==splitCur.FSplitNum //fsplit num is correctly linked
						&& (chargeCur.ProvNum==splitCur.ProvNum || chargeCur.ProvNum==0 || splitCur.ProvNum==0)//matching provs, except for unearned provider(0)
						&& chargeCur.PatNum==splitCur.PatNum && chargeCur.ClinicNum==splitCur.ClinicNum //matching pat/prov/clinics
						&& splitCur.ProcNum==0 && splitCur.AdjNum==0 && splitCur.PayPlanNum==0) //split is unattached
					{
						if(splitCur.PayNum!=payNum) {
							chargeCur.AmountStart-=(decimal)splitCur.SplitAmt;//splits counteracting prepay are negative
						}
						PaySplit splitForCollection=splitCur.Copy();
						splitForCollection.AccountEntryAmtPaid=(decimal)splitCur.SplitAmt;
						chargeCur.AmountEnd-=(decimal)splitCur.SplitAmt;
						chargeCur.SplitCollection.Add(splitForCollection);
						List<AccountEntry> listPaySplitAccountEntries=listAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit));
						AccountEntry splitCurEntry=listPaySplitAccountEntries.FirstOrDefault(x => x.PriKey==splitCur.SplitNum);//split entry
						if(splitCurEntry!=null) {
							splitCurEntry.AmountStart=0;//final allocations are likely to a procedure which gets handled elsewhere, don't double count them. 
							splitCurEntry.AmountEnd=0;
						}
						break;
					}
				}
				//Do not subtract amount from split or parent split here since we are looping through them and may be modifying split value that hasn't
				//been evaluated yet. If this code absolutely needs to be added back, make to only have it execute when it is not an income transfer.
			}
			listAccountCharges=LinkUnearnedForPatProvClinicUnearnedType(listAccountCharges);
			if(isIncomeTransfer) {
				//Apply payments to pat/prov/clinic buckets. This is similar to "implicit linking" for paysplits, which does not happen for transfers
				List<AccountEntry> listSplitEntries=listAccountCharges.Where(x => x.GetType()==typeof(PaySplit) && !x.AmountEnd.IsEqual(0)).ToList();
				foreach(PaySplit splitCur in listSplitsCurrentAndHistoric.Where(x => x.SplitNum.In(listSplitEntries.Select(y => y.PriKey).ToList()))) {
					foreach(AccountEntry chargeCur in listSplitEntries) {
						AccountEntry splitEntry=listSplitEntries.FirstOrDefault(x => x.PriKey==splitCur.SplitNum);
						if(splitEntry==null || splitEntry.AmountEnd.IsEqual(0)) {
							break;
						}
						//if splitCur.ProcNum==0 NEEDS to be added back in the future, there is an additional case that will need to be handled elsewhere.  B13471.
						if(!chargeCur.AmountEnd.IsEqual(0) && chargeCur.PriKey!=splitCur.SplitNum
							&& (chargeCur.ProvNum==splitCur.ProvNum || chargeCur.ProvNum==0 || splitCur.ProvNum==0)//matching provs, except unearned provider (0)
							&& chargeCur.PatNum==splitCur.PatNum && chargeCur.ClinicNum==splitCur.ClinicNum) 
						{
							if(Math.Sign(Math.Round(chargeCur.AmountEnd,3))==Math.Sign(Math.Round(splitEntry.AmountEnd,3))) {
								continue;//do not apply splits to other splits when they are of the same sign, we want them to stay separated, not get combined.
							}
							decimal amount=Math.Min(Math.Abs(splitEntry.AmountEnd),Math.Abs(chargeCur.AmountEnd));
							decimal splitAmount=amount;
							if(chargeCur.AmountEnd<0) {//negative charge, positive splitEntry
								amount=amount*-1;
							}
							if(splitCur.SplitAmt<0) {
								splitAmount=amount*-1;
							}
							if(splitCur.PayNum!=payNum) {
								chargeCur.AmountStart-=amount;
							}
							PaySplit splitForCollection=splitCur.Copy();
							splitForCollection.AccountEntryAmtPaid=splitAmount;
							chargeCur.AmountEnd-=amount;
							chargeCur.SplitCollection.Add(splitForCollection);
							splitEntry.AmountStart+=amount;
							splitEntry.AmountEnd+=amount;
						}
					}
				}
			}
			return listAccountCharges;
		}

		///<summary>This method applies positive and negative unearned to each other so the sum is correct before making any transfers.
		///This method assumes that explicit linking has been done before it is called so the unallocated splits no longer have any value (if any)
		///Without this method when a procedure, postive unearned, and negative unearned are on an account the postive would be applied to the procedure,
		///leaving a negative on the account. We want the positive and negative to cancel each other out before any transfers are made.</summary>
		public static List<AccountEntry> LinkUnearnedForPatProvClinicUnearnedType(List<AccountEntry> listAccountCharges) {
			List<AccountEntry> listUnearned=listAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit) && ((PaySplit)x.Tag).UnearnedType!=0);
			List<AccountEntry> listPositiveUnearned=listUnearned.FindAll(x => x.AmountEnd > 0);//negative paysplit (like from refunds)
			List<AccountEntry> listNegativeUnearned=listUnearned.FindAll(x => x.AmountEnd < 0);//regular positve paysplits
			foreach(AccountEntry positiveEntry in listPositiveUnearned) {
				foreach(AccountEntry negativeEntry in listNegativeUnearned) {
					if(positiveEntry.AmountEnd.IsEqual(0)) {
						continue;//no more money to apply.
					}
					if(positiveEntry.ProvNum!=negativeEntry.ProvNum 
						|| positiveEntry.PatNum!=negativeEntry.PatNum 
						|| positiveEntry.ClinicNum!=negativeEntry.ClinicNum	
						|| ((PaySplit)positiveEntry.Tag).UnearnedType!=((PaySplit)negativeEntry.Tag).UnearnedType) 
					{
						continue;
					}
					decimal amount=Math.Min(Math.Abs(positiveEntry.AmountEnd),Math.Abs(negativeEntry.AmountEnd));
					positiveEntry.AmountStart-=amount;
					positiveEntry.AmountEnd-=amount;
					negativeEntry.AmountEnd+=amount;
					negativeEntry.AmountStart+=amount;
				}
			}
			return listAccountCharges;
		}

		///<summary>Attempts to implicitly link old unattached payments to past production that has not explicitly has payments attached to them.
		///This will give the patient a better idea on what they need to pay off next.
		///It is important to invoke ExplicitlyLinkCredits() prior to this method.</summary>
		///<param name="listPaySplits">All payment splits for the family.</param>
		///<param name="listAdjustments">All adjustments for the patient.</param>
		///<param name="listInsPayAsTotal">All claimprocs paid as total for the family, might contain ins payplan payments.
		///Adds claimprocs for the completed procedures if in income xfer mode.</param>
		///<param name="listAccountCharges">All account entries generated by ConstructListCharges() and ExplicitlyLinkCredits().
		///Can include account charges for the family.</param>
		///<param name="listSplitsCur">All splits associated to payCur.  Empty list for a new payment.</param>
		///<param name="listPayFirstAcctEntries">All account entries that payCur should be linked to first.
		///If payCur.PayAmt is greater than the sum of these account entries then any leftover amount will be implicitly linked to other entries.</param>
		///<param name="payCur">The payment that is wanting to be linked to other account entries.  Could have entites linked already.</param>
		///<param name="patNum">The PatNum of the currently selected patient.</param>
		///<param name="isPatPrefer">Set to true if account entries for patNum should be prioritized before other entries.</param>
		///<returns>A helper class that represents the implicit credits that this method made.</returns>
		public static PayResults ImplicitlyLinkCredits(List<PaySplit> listPaySplits,List<Adjustment> listAdjustments
			,List<PayAsTotal> listInsPayAsTotal,List<AccountEntry> listAccountCharges,List<PaySplit> listSplitsCur,List<AccountEntry> listPayFirstAcctEntries
			,Payment payCur,long patNum,bool isPatPrefer) 
		{
			//No remoting role check; no call to db
			//use negative paysplits to counteract inspaytotals/paysplits/adjustments for same prov/pat/clinic-Accounts for income transfers and prevents using thetransferred money.
			PayResults implicitCredits=new PayResults();
			//only get splits that have not yet been explicitly allocated, not attached to procs or payplan.
			List<PaySplit> listSplitsCopied=new List<PaySplit>(listPaySplits.Where(x => x.ProcNum==0 && x.PayPlanNum==0).Select(x => x.Copy()).ToList());
			List<Adjustment> listNegAdjustCopied=new List<Adjustment>(listAdjustments.Where(x => x.ProcNum==0 && x.AdjAmt<0).Select(x => x.Clone()).ToList());
			//use negative paysplits to counteract inspaytotals/paysplits/adjustments for same prov/pat/clinic.  
			//Accounts for income transfers and prevents using the transferred money.
			List<PaySplit> listNegSplits=listSplitsCopied.FindAll(x => x.SplitAmt<0);
			List<long> listHiddenUnearnedDefNums=Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType)
				.FindAll(x => !string.IsNullOrEmpty(x.ItemValue))//If ItemValue is not blank, it means "do not show on account"
				.Select(x => x.DefNum).ToList();
			//Remvoing TP procs from the listAccountCharges as they must be explicitly paid, not implicitly
			List<AccountEntry> listTpProcs=listAccountCharges.FindAll(x => x.GetType()==typeof(Procedure) && ((Procedure)x.Tag).ProcStatus==ProcStat.TP);
			List<long> listTpPRocAccountEntryNums=listTpProcs.Select(y => y.AccountEntryNum).ToList();
			listAccountCharges.RemoveAll(x => x.AccountEntryNum.In(listTpPRocAccountEntryNums));
			foreach(PaySplit negSplit in listNegSplits) {
				if(negSplit.UnearnedType.In(listHiddenUnearnedDefNums)) {
					continue;
				}
				List<PaySplit> posSplits=listSplitsCopied.FindAll(x => x.SplitAmt>0 && x.PatNum==negSplit.PatNum && x.ProvNum==negSplit.ProvNum &&x.ClinicNum==negSplit.ClinicNum);
				foreach(PaySplit posSplit in posSplits) {
					if(posSplit.UnearnedType.In(listHiddenUnearnedDefNums)) {
						//splits with hidden unearned types are not allowed to be used for implicit linking or income transfers.
						continue;
					}
					if(negSplit.SplitAmt==0) {
						break;
					}
					if(posSplit.SplitAmt==0) {
						continue;
					}
					double amt=Math.Min(Math.Abs(negSplit.SplitAmt),posSplit.SplitAmt);
					negSplit.SplitAmt+=amt;
					posSplit.SplitAmt-=amt;
				}
				List<PayAsTotal> payByTotals=listInsPayAsTotal.FindAll(x => x.PatNum==negSplit.PatNum && x.ProvNum==negSplit.ProvNum 
					&& x.ClinicNum==negSplit.ClinicNum &&x.SummedInsPayAmt!=0);
				foreach(PayAsTotal claimProc in payByTotals) {
					if(negSplit.SplitAmt==0) {
						break;
					}
					if(claimProc.SummedInsPayAmt==0 && claimProc.SummedWriteOff==0) {
						continue;
					}
					double amtPay=Math.Min(Math.Abs(negSplit.SplitAmt),claimProc.SummedInsPayAmt);
					negSplit.SplitAmt+=amtPay;
					claimProc.SummedInsPayAmt-=amtPay;
					double amtWriteOff=Math.Min(Math.Abs(negSplit.SplitAmt),claimProc.SummedWriteOff);
					negSplit.SplitAmt+=amtWriteOff;
					claimProc.SummedWriteOff-=amtWriteOff;
				}
				List<Adjustment> listAdjusts=listNegAdjustCopied.FindAll(x => x.PatNum==negSplit.PatNum && x.ProvNum==negSplit.ProvNum 
					&& x.ClinicNum==negSplit.ClinicNum);
				foreach(Adjustment negAdjust in listAdjusts) {
					if(negSplit.SplitAmt==0) {
						break;
					}
					if(negAdjust.AdjAmt==0) {
						continue;
					}
					double amt=Math.Min(Math.Abs(negSplit.SplitAmt),Math.Abs(negAdjust.AdjAmt));
					//The split is always negative here.
					negSplit.SplitAmt+=amt;
					negAdjust.AdjAmt+=amt;
				}
			}
			if(isPatPrefer) {
				//Sort current pat charges so they are more likely to be "unpaid" by implicit linking which leads to them being paid by this payment.
				listAccountCharges=listAccountCharges.OrderByDescending(x => x.PatNum!=patNum).ThenBy(x => x.Date).ToList();
			}
			else if(PrefC.GetInt(PrefName.AutoSplitLogic)==(int)AutoSplitPreference.Adjustments) {
				//Sort by Adjustments, followed by Procedures. Need to sort by type, then sort by date so get earliest adjs followed by earliest procs
				listAccountCharges=listAccountCharges.OrderBy(x => x.GetType()!=typeof(Adjustment)).ThenBy(x => x.Date).ToList(); 
			}
			if(listPayFirstAcctEntries.Count>0) {//User has specific procs/payplancharges/adjustments selected prior to entering the Payment window.  
				//They wish these be paid by this payment specifically.
				//To accomplish this, we need to auto-split to the selected procedures prior to implicit linking.
				foreach(AccountEntry entry in listPayFirstAcctEntries) {
					if(payCur.PayAmt<=0) {
						break;//Will be empty
					}
					AccountEntry charge=listAccountCharges.Find(x => x.PriKey==entry.PriKey);
					if(charge==null) {
						continue;//likely only for the event of selecting payplan line items when the plan is closed and on version 2. 
					}
					PaySplit split=new PaySplit();
					double amt=Math.Min((double)charge.AmountEnd,payCur.PayAmt);
					payCur.PayAmt=Math.Round(payCur.PayAmt-amt,3);
					if(charge.GetType()==typeof(PayPlanCharge)) {
						//handles payment plan splits in a special way. Continue to the next entry after we add to the list. 
						PayPlanCharge ppCharge=(PayPlanCharge)charge.Tag;
						PayPlan payplan=PayPlans.GetOne(((PayPlanCharge)charge.Tag).PayPlanNum);
						List<PaySplit> listPayPlanSplitsToAdd=new List<PaySplit>();
						decimal payAmtOut;
						if(payplan.IsDynamic) {
							listPayPlanSplitsToAdd=PaySplits.CreateSplitsForDynamicPayPlan(payCur.PayNum,amt,charge,listAccountCharges,0,true,out payAmtOut);
						}
						else{
							listPayPlanSplitsToAdd=PaySplits.CreateSplitForPayPlan(payCur.PayNum,amt,charge
							,PayPlanCharges.GetChargesForPayPlanChargeType(ppCharge.PayPlanNum,PayPlanChargeType.Credit),listAccountCharges,0,true,out payAmtOut);
						}
						listSplitsCur.AddRange(listPayPlanSplitsToAdd);
						//payCur.PayAmt=(double)payAmtOut;
						charge.SplitCollection.AddRange(listPayPlanSplitsToAdd);
						continue;
					}
					else if(charge.GetType()==typeof(Procedure)) {
						split.ProcNum=charge.PriKey;
					}
					else if(charge.GetType()==typeof(Adjustment) && ((Adjustment)charge.Tag).ProcNum==0) {
						//should already be verified to have no procedure and positive amount
						split.AdjNum=charge.PriKey;
					}
					if(PrefC.HasClinicsEnabled) {//Clinics
						split.ClinicNum=charge.ClinicNum;
					}
					split.SplitAmt=amt;
					charge.AmountEnd-=(decimal)amt;
					split.DatePay=DateTime.Today;
					split.PatNum=charge.PatNum;
					split.ProvNum=charge.ProvNum;
					split.PayNum=payCur.PayNum;
					charge.SplitCollection.Add(split);
					listSplitsCur.Add(split);
				}
			}
			Enumerable.Range(1,8).ForEach(x => PayPatientBalances(ref listInsPayAsTotal,ref listSplitsCopied,ref listNegAdjustCopied,x,ref listAccountCharges));
			listAccountCharges.AddRange(listTpProcs);//Add TP procs back in so they can display in FormPayments "Outstanding Charges" grid.
			if(isPatPrefer) {
				//Sort current pat charges so they are more likely to be paid by this payment.
				listAccountCharges=listAccountCharges.OrderBy(x => x.PatNum!=patNum).ThenBy(x => x.Date).ToList();
			}
			implicitCredits.ListAccountCharges=listAccountCharges;
			implicitCredits.ListSplitsCur=listSplitsCur;
			implicitCredits.Payment=payCur;
			return implicitCredits;
		}

		///<summary>This function takes a list of a patient's credits and implicitly links them to the patient's charges.  Lists are passed in by reference
		///because the lists are used multiple times (they keep track of if a PaySplit has been fully used, for example).  Make sure to pass in copies
		///if you don't want original data overwritten.  Phase 1 matches on Prov, Clinic, and Patient.  Phase 2 matches on Prov and Patient.  Phase 3 matches only on Patient.
		///The purpose is to implicitly link a patient's credits to their charges as accurately as possible.</summary>
		public static void PayPatientBalances(ref List<PayAsTotal> listInsPayAsTotal,ref List<PaySplit> listPaySplits,ref List<Adjustment> listAdjustments
			,int phase,ref List<AccountEntry> listAccountCharges) 
		{
			//No remoting role check; no call to db
			List<long> listHiddenUnearnedDefNums=Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType)
				.FindAll(x => !string.IsNullOrEmpty(x.ItemValue))//If ItemValue is not blank, it means "do not show on account"
				.Select(x => x.DefNum).ToList();
			foreach(PayAsTotal payAsTotal in listInsPayAsTotal) {//Use claim payments by total to pay off procedures for that specific patient.
				if(payAsTotal.SummedInsPayAmt==0 && payAsTotal.SummedWriteOff==0) {
					continue;
				}
				//Find all charges that have this claimproc's patient, provider, or clinic, and use the claimproc to pay them off.
				List<AccountEntry> listEntriesForPayAsTotal=new List<AccountEntry>();
				switch (phase) {
					case 1:
						listEntriesForPayAsTotal=listAccountCharges.FindAll(x => x.ProvNum==payAsTotal.ProvNum && x.PatNum==payAsTotal.PatNum && x.ClinicNum==payAsTotal.ClinicNum);
						break;
					case 2:
						listEntriesForPayAsTotal=listAccountCharges.FindAll(x => x.ProvNum==payAsTotal.ProvNum && x.PatNum==payAsTotal.PatNum);
						break;
					case 3:
						listEntriesForPayAsTotal=listAccountCharges.FindAll(x => x.ProvNum==payAsTotal.ProvNum && x.ClinicNum==payAsTotal.ClinicNum);
						break;
					case 4:
						listEntriesForPayAsTotal=listAccountCharges.FindAll(x => x.PatNum==payAsTotal.PatNum && x.ClinicNum==payAsTotal.ClinicNum);
						break;
					case 5:
						listEntriesForPayAsTotal=listAccountCharges.FindAll(x => x.ProvNum==payAsTotal.ProvNum);
						break;
					case 6:
						listEntriesForPayAsTotal=listAccountCharges.FindAll(x => x.PatNum==payAsTotal.PatNum);
						break;
					case 7:
						listEntriesForPayAsTotal=listAccountCharges.FindAll(x => x.ClinicNum==payAsTotal.ClinicNum);
						break;
					case 8:
						listEntriesForPayAsTotal=listAccountCharges;//Any unpaid charge.
						break;
				}
				foreach(AccountEntry accountEntry in listEntriesForPayAsTotal) {
					if(payAsTotal.SummedInsPayAmt==0 && payAsTotal.SummedWriteOff==0) {
						break;
					}
					if(accountEntry.AmountEnd==0) {
						continue;
					}
					if(accountEntry.GetType()==typeof(PayPlanCharge)) {//B12401
						continue;
					}
					double amt=Math.Min((double)accountEntry.AmountEnd,payAsTotal.SummedInsPayAmt);
					accountEntry.AmountStart-=(decimal)amt;
					accountEntry.AmountEnd-=(decimal)amt;
					payAsTotal.SummedInsPayAmt-=amt;
					double amtWriteOff=Math.Min((double)accountEntry.AmountEnd,payAsTotal.SummedWriteOff);
					accountEntry.AmountStart-=(decimal)amtWriteOff;
					accountEntry.AmountEnd-=(decimal)amtWriteOff;
					payAsTotal.SummedWriteOff-=amtWriteOff;
				}
			}
			foreach(PaySplit split in listPaySplits) {//Use unattached positive paysplits to pay off patient's procedures as accurately as possible.
				if(split.UnearnedType.In(listHiddenUnearnedDefNums)) {
					continue;//Splits that do not show on the account are not valid to use for implicit linking
				}
				if(split.ProcNum!=0) {
					continue;//Don't use explicitly linked paysplits.
				}
				if(split.SplitAmt<=0) {
					continue;
				}
				//Find all charges that have this split's patient, provider, or clinic, and use the split to pay them off.
				List<AccountEntry> listEntriesForSplit=new List<AccountEntry>();
				switch (phase) {
					case 1:
						listEntriesForSplit=listAccountCharges.FindAll(x => x.ProvNum==split.ProvNum && x.PatNum==split.PatNum && x.ClinicNum==split.ClinicNum);
						break;
					case 2:
						listEntriesForSplit=listAccountCharges.FindAll(x => x.ProvNum==split.ProvNum && x.PatNum==split.PatNum);
						break;
					case 3:
						listEntriesForSplit=listAccountCharges.FindAll(x => x.ProvNum==split.ProvNum && x.ClinicNum==split.ClinicNum);
						break;
					case 4:
						listEntriesForSplit=listAccountCharges.FindAll(x => x.PatNum==split.PatNum && x.ClinicNum==split.ClinicNum);
						break;
					case 5:
						listEntriesForSplit=listAccountCharges.FindAll(x => x.ProvNum==split.ProvNum);
						break;
					case 6:
						listEntriesForSplit=listAccountCharges.FindAll(x => x.PatNum==split.PatNum);
						break;
					case 7:
						listEntriesForSplit=listAccountCharges.FindAll(x => x.ClinicNum==split.ClinicNum);
						break;
					case 8:
						listEntriesForSplit=listAccountCharges;//Any unpaid charge.
						break;
				}
				foreach(AccountEntry accountEntry in listEntriesForSplit) {
					if(split.SplitAmt==0) {
						break;//Split's amount has been used by previous charges.
					}
					if(accountEntry.AmountEnd==0) {
						continue;
					}
					if(accountEntry.GetType()==typeof(PayPlanCharge)) {//B12401
						continue;
					}
					if(accountEntry.GetType()==typeof(Procedure) && ((Procedure)accountEntry.Tag).ProcStatus==ProcStat.TP) {
						continue;//we do not implicitly link to TP procedures
					}
					double amt=Math.Min((double)accountEntry.AmountEnd,split.SplitAmt);
					accountEntry.AmountStart-=(decimal)amt;
					accountEntry.AmountEnd-=(decimal)amt;
					accountEntry.SplitCollection.Add(split);
					split.SplitAmt-=amt;
				}
			}
			foreach(Adjustment adjustment in listAdjustments) {//Use unattached credit adjustments to pay off patient's procedures as accurately as possible.
				if(adjustment.ProcNum!=0) {
					continue;//Don't use explicitly linked adjustments.
				}
				if(adjustment.AdjAmt>=0) {
					continue;//Don't use charge adjustments.
				}
				//Find all charges that have this adjustment's patient, provider, or clinic, and use the adjustment to pay them off.
				List<AccountEntry> listEntriesForAdjust=new List<AccountEntry>();
				switch (phase) {
					case 1:
						listEntriesForAdjust=listAccountCharges.FindAll(x => x.ProvNum==adjustment.ProvNum && x.PatNum==adjustment.PatNum && x.ClinicNum==adjustment.ClinicNum);
						break;
					case 2:
						listEntriesForAdjust=listAccountCharges.FindAll(x => x.ProvNum==adjustment.ProvNum && x.PatNum==adjustment.PatNum);
						break;
					case 3:
						listEntriesForAdjust=listAccountCharges.FindAll(x => x.ProvNum==adjustment.ProvNum && x.ClinicNum==adjustment.ClinicNum);
						break;
					case 4:
						listEntriesForAdjust=listAccountCharges.FindAll(x => x.PatNum==adjustment.PatNum && x.ClinicNum==adjustment.ClinicNum);
						break;
					case 5:
						listEntriesForAdjust=listAccountCharges.FindAll(x => x.ProvNum==adjustment.ProvNum);
						break;
					case 6:
						listEntriesForAdjust=listAccountCharges.FindAll(x => x.PatNum==adjustment.PatNum);
						break;
					case 7:
						listEntriesForAdjust=listAccountCharges.FindAll(x => x.ClinicNum==adjustment.ClinicNum);
						break;
					case 8:
						listEntriesForAdjust=listAccountCharges;//Any unpaid charge.
						break;
				}
				foreach(AccountEntry accountEntry in listEntriesForAdjust) {
					if(adjustment.AdjAmt==0) {
						break;//Adjustment's amount has been used by previous charges.
					}
					if(accountEntry.AmountEnd==0) {
						continue;
					}
					if(accountEntry.GetType()==typeof(PayPlanCharge)) {//B12401
						continue;
					}
					double amt=Math.Min((double)accountEntry.AmountEnd,Math.Abs(adjustment.AdjAmt));//Credit adjustments are negative.
					accountEntry.AmountStart-=(decimal)amt;
					accountEntry.AmountEnd-=(decimal)amt;
					adjustment.AdjAmt+=amt;//Credit ajustments are negative.
				}
			}
		}

		/// <summary>Attaches information into the LoadData.DictPatToDPFeeSchedule and LoadData.ListFeesForDiscountPlan variables. Requires
		/// a list of TP procedures to get fees for.</summary>
		private static void LoadDiscountPlanInfo(ref LoadData loadData,List<Procedure> listTpProcs) {
			//No remoting role check; private method and uses ref parameter.
			if(loadData==null) {
				loadData=new LoadData();
			}
			loadData.DictPatToDPFeeScheds=DiscountPlans.GetFeeSchedNumsByPatNums(listTpProcs.Select(x => x.PatNum).ToList());
			loadData.ListFeesForDiscountPlans=Fees.GetByFeeSchedNumsClinicNums(loadData.DictPatToDPFeeScheds.Select(x => x.Value).Distinct()
					.Where(x => x!=0).ToList(),listTpProcs.Select(x => x.ClinicNum).Union(new List<long> { 0 }).ToList())
				.Select(x => (Fee)x).ToList();
		}
		#endregion

		#region AllocateUnearned
		///<summary>This function takes a list of a patient's prepayments and first explicitly links prepayment then implicitly links them to the existing prepayments.</summary>
		public static List<PaySplits.PaySplitAssociated> AllocateUnearned(List<AccountEntry> listAccountEntries,ref List<PaySplit> listPaySplit,Payment payCur
			,double unearnedAmt,Family fam) 
		{
			//No remoting role check; Method that uses ref parameters.
			List<PaySplits.PaySplitAssociated> retVal=new List<PaySplits.PaySplitAssociated>();
			if(unearnedAmt.IsLessThan(0) || listAccountEntries==null) {
				return retVal;
			}
			//don't allocate prepayments that are attached to TP procedures or that are flagged as hidden on account. 
			List<PaySplit> listFamPrePaySplits=PaySplits.GetPrepayForFam(fam,doExcludeTpPrepay:true);
			//Manipulate the original prepayments SplitAmt by subtracting counteracting SplitAmt.
			foreach(PaySplit prePaySplit in listFamPrePaySplits.FindAll(x => x.SplitAmt.IsGreaterThan(0))) {
				List<PaySplit> listSplitsForPrePay=PaySplits.GetSplitsForPrepay(new List<PaySplit>() { prePaySplit });//Find all splits for the pre-payment.
				foreach(PaySplit splitForPrePay in listSplitsForPrePay) {
					prePaySplit.SplitAmt+=splitForPrePay.SplitAmt;//Sum the amount of the pre-payment that's used. (balancing splits are negative usually)
				}
			}
			//We will try our best to automatically link any prepayments that don't have an explicit counteracting paysplit.
			//After all prepayments have been "linked", manipulate unearnedAmt accordingly.
			ImplicitlyLinkPrepayments(ref listFamPrePaySplits,ref unearnedAmt);
			//Get all claimprocs and adjustments for the proc;
			List<Procedure> listProcs=PaymentEdit.GetProcsForAccountEntries(listAccountEntries);
			List<ClaimProc> listClaimProcs=ClaimProcs.GetForProcs(listProcs.Select(x => x.ProcNum).Distinct().ToList());
			List<Adjustment> listAdjusts=Adjustments.GetForProcs(listProcs.Select(x => x.ProcNum).Distinct().ToList());
			foreach(Procedure proc in listProcs) {//For each proc see how much of the Unearned we use per proc
				if(unearnedAmt<=0) {
					break;
				}
				//Calculate the amount remaining on the procedure so we can know how much of the remaining pre-payment amount we can use.
				proc.ProcFee-=PaySplits.GetPaySplitsFromProc(proc.ProcNum).Sum(x => x.SplitAmt);//Figure out how much is due on this proc.
				double patPortion=(double)ClaimProcs.GetPatPortion(proc,listClaimProcs,listAdjusts);
				foreach(PaySplit prePaySplit in listFamPrePaySplits) {
					//First we need to decide how much of each pre-payment split we can use per proc.
					if(patPortion<=0) {//Proc has been paid for, go to next proc
						break;
					}
					if(prePaySplit.SplitAmt<=0) {//Split has been used, go to next split
						continue;
					}
					if(patPortion<=0) {
						break;
					}
					decimal splitTotal=0;
					if(splitTotal<(decimal)prePaySplit.SplitAmt) {//If the sum indicates there's pre-payment amount left over, let's use it.
						double amtToUse=0;
						if(prePaySplit.SplitAmt<patPortion) {
							amtToUse=prePaySplit.SplitAmt;
						}
						else {
							amtToUse=patPortion;
						}
						unearnedAmt-=amtToUse;//Reflect the new unearned amount available for future proc use.
						PaySplit splitNeg=new PaySplit();
						splitNeg.PatNum=prePaySplit.PatNum;
						splitNeg.PayNum=payCur.PayNum;
						splitNeg.FSplitNum=prePaySplit.SplitNum;
						splitNeg.ClinicNum=prePaySplit.ClinicNum;
						splitNeg.ProvNum=prePaySplit.ProvNum;
						splitNeg.SplitAmt=0-amtToUse;
						splitNeg.UnearnedType=prePaySplit.UnearnedType;
						splitNeg.DatePay=DateTime.Now;
						listPaySplit.Add(splitNeg);
						//Make a different paysplit attached to proc and prov they want to use it for.
						PaySplit splitPos=new PaySplit();
						splitPos.PatNum=proc.PatNum;//Use procedure's pat to allocate to that patient's account even when viewing entire family.
						splitPos.PayNum=payCur.PayNum;
						splitPos.FSplitNum=0;//The association will be done on form closing.
						splitPos.ProvNum=proc.ProvNum;
						splitPos.ClinicNum=proc.ClinicNum;
						splitPos.SplitAmt=amtToUse;
						splitPos.DatePay=DateTime.Now;
						splitPos.ProcNum=proc.ProcNum;
						listPaySplit.Add(splitPos);
						//link negSplit to posSplit. 
						retVal.Add(new PaySplits.PaySplitAssociated(splitNeg,splitPos));
						//link original prepayment to neg split.
						PaySplit paySplitPrePayOrig=PaySplits.GetOne(prePaySplit.SplitNum);
						retVal.Add(new PaySplits.PaySplitAssociated(paySplitPrePayOrig,splitNeg));
						prePaySplit.SplitAmt-=amtToUse;
						patPortion-=amtToUse;
					}
				}
			}
			return retVal;
		}

		///<summary>This function takes a list of a patient's prepayments and implicitly links them to unlinked paysplits.  Lists are passed in by reference
		///because the lists are used multiple times.</summary>
		private static void ImplicitlyLinkPrepayments(ref List<PaySplit> listFamPrePaySplits, ref double unearnedAmt) {
			//No remoting role check; private method that uses ref parameters.
			List<PaySplit> listPosPrePay=listFamPrePaySplits.FindAll(x => x.SplitAmt.IsGreaterThan(0));
			List<PaySplit> listNegPrePay=listFamPrePaySplits.FindAll(x => x.SplitAmt.IsLessThan(0));
			//Logic check PatNum - match, ProvNum - match, ClinicNum - match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			//Logic check PatNum - match, ProvNum - match, ClinicNum - zero
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumZero:true);
			//Logic check PatNum - match, ProvNum - match, ClinicNum - non zero & non match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNonZeroNonMatch:true);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumZero:true,isClinicNumMatch:true);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - zero
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumZero:true,isClinicNumZero:true);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - non zero & non match 
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumZero:true,isClinicNonZeroNonMatch:true);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - match 
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNumMatch:true);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - zero
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNumZero:true);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - non zero & non match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNonZeroNonMatch:true);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - zero
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNumZero:true);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - non zero & non match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNonZeroNonMatch:true);
			//Logic check PatNum - other family members, ProvNum - zero, ClinicNum - match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNumMatch:true);
			//Logic check PatNum - other family members, ProvNum - zero, ClinicNum - zero
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNumZero:false);
			//Logic checkPatNum - other family members, ProvNum - zero, ClinicNum - non zero & non match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNonZeroNonMatch:true);
			//Logic checkPatNum - other family members, ProvNum - non zero & non match, ClinicNum - match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNumMatch:true);
			//Logic check PatNum - other family members, ProvNum - non zero & non match, ClinicNum - zero
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNumZero:true);
			//Logic check PatNum - other family members, ProvNum - non zero & non match, ClinicNum - non zero & non match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNonZeroNonMatch:true);
		}

		/// <summary> Helpler method to allocate unearned implicitly. Returns amount remaining to be allocated after implicitly linking.</summary>
		public static double ImplicitlyLinkPrepaymentsHelper(List<PaySplit> listPosPrePay,List<PaySplit> listNegPrePay,double unearnedAmt,bool isPatMatch=false
			,bool isProvNumMatch=false,bool isClinicNumMatch=false,bool isFamMatch=false,bool isProvNumZero=false,bool isClinicNumZero=false
			,bool isProvNonZeroNonMatch=false,bool isClinicNonZeroNonMatch=false) 
		{
			//No remoting role check; no call to db.
			if(unearnedAmt.IsLessThan(0) || unearnedAmt.IsZero()){
				return 0;
			}
			//Manipulate the amounts (never letting them go into the negative) and then use whatever is left to represent the unearned amount.
			foreach(PaySplit posSplit in listPosPrePay) {
				if(posSplit.SplitAmt.IsEqual(0)) {
					continue;
				}
				//Find all negative paysplits with the filters
				List<PaySplit> filteredNegSplit=listNegPrePay
					.Where(x => !isPatMatch               || x.PatNum==posSplit.PatNum)
					.Where(x => !isFamMatch               || x.PatNum!=posSplit.PatNum)
					.Where(x => !isProvNumMatch           || x.ProvNum==posSplit.ProvNum)
					.Where(x => !isProvNumZero            || x.ProvNum==0)
					.Where(x => !isProvNonZeroNonMatch    || (x.ProvNum!=0 && x.ProvNum!=posSplit.ProvNum))
					.Where(x => !isClinicNumMatch         || x.ClinicNum==posSplit.ClinicNum)
					.Where(x => !isClinicNumZero          || x.ClinicNum==0)
					.Where(x => !isClinicNonZeroNonMatch  || (x.ClinicNum!=0 && x.ClinicNum!=posSplit.ClinicNum))
					.ToList();
				foreach(PaySplit negSplit in filteredNegSplit) {
					if(negSplit.SplitAmt.IsEqual(0)) {
						continue;
					}
					//Deduct split amount from the positive split for the amount of the negative split,or the postive split depending on which is smaller according to the absolute value. 
					double amt=Math.Min(posSplit.SplitAmt,Math.Abs(negSplit.SplitAmt));
					negSplit.SplitAmt+=amt;
					posSplit.SplitAmt-=amt;
				}
			}
			return listPosPrePay.Sum(x => x.SplitAmt);
		}
		#endregion

		#region Misc
		/// <summary>Makes a payment from a passed in list of charges.</summary>
		public static PayResults MakePayment(List<List<AccountEntry>> listSelectedCharges,List<PaySplit> listSplitsCur,bool isShowAll,Payment payCur
			,int groupIdx,bool hasPayTypeNone,decimal textAmount,List<AccountEntry> listAllCharges) 
		{
			//No remoting role check; no call to db.
			PayResults splitData=null;
			if(hasPayTypeNone) {//Make simple splits, negative and positive, to no procedures, to even out the account balances.
				splitData=new PayResults { Payment=payCur };
				for(int i=0;i<listSelectedCharges.Count;i++) {
					PayResults createdSplit=new PayResults();
					PaySplit paySplit=new PaySplit();
					if(groupIdx==1) {//Grouped by provider
						//listCharges should all have same provider and patient.
						paySplit.ClinicNum=Clinics.ClinicNum;//Use current clinic
					}
					else {//Grouped by provider and clinic
						//listCharges should all have same provider and patient and clinic.
						paySplit.ClinicNum=listSelectedCharges[i][0].ClinicNum;
					}
					paySplit.DatePay=DateTime.Today;
					paySplit.PatNum=listSelectedCharges[i][0].PatNum;
					paySplit.PayNum=payCur.PayNum;
					paySplit.ProvNum=listSelectedCharges[i][0].ProvNum;
					paySplit.SplitAmt=(double)listSelectedCharges[i].Sum(x => x.AmountEnd);
					if(listSelectedCharges[i].Any(x => x.GetType()==typeof(PaySplit))) {
						paySplit.UnearnedType=((PaySplit)listSelectedCharges[i].First(x => x.GetType()==typeof(PaySplit)).Tag).UnearnedType;
						foreach(AccountEntry entry in listSelectedCharges[i]) {
							if(entry.GetType()==typeof(PaySplit)) {
								PaySplit split=(PaySplit)entry.Tag;
								if(split.UnearnedType!=paySplit.UnearnedType) {
									paySplit.UnearnedType=Defs.GetDef(DefCat.PaySplitUnearnedType,PrefC.GetLong(PrefName.PrepaymentUnearnedType)).DefNum;
									break;
								}
							}
						}
					}
					listSelectedCharges[i][0].SplitCollection.Add(paySplit);
					splitData.ListSplitsCur.Add(paySplit);
					listSelectedCharges[i].ForEach(x => x.AmountEnd=0);
				}
				splitData.ListAccountCharges.AddRange(listAllCharges);
				splitData.ListSplitsCur.AddRange(listSplitsCur);
				return splitData;
			}
			bool isPayAmtZeroUponEntering=(payCur.PayAmt==0);
			for(int i=0;i<listSelectedCharges.Count;i++) {
				if(payCur.PayAmt==0 && !isPayAmtZeroUponEntering) {
					break;
				}
				if(groupIdx>0) {//group by clinic and/or provider
					foreach(AccountEntry charge in listSelectedCharges[i]) {
						if(charge.AmountEnd==0 && !isShowAll) {
							continue;
						}
						decimal splitAmt=(isPayAmtZeroUponEntering ? charge.AmountEnd : (decimal)payCur.PayAmt);
						splitData=CreatePaySplit(charge.AccountEntryNum,splitAmt,payCur,textAmount,listSplitsCur,listAllCharges);
						listSplitsCur=splitData.ListSplitsCur;
						listAllCharges=splitData.ListAccountCharges;
						payCur=splitData.Payment;
					}
				}
				else {//group by none
					AccountEntry charge=listSelectedCharges[i].First();//Each item in listSelectedCharges should have exactly one AccountEntry.
					if(charge.AmountEnd==0 && !isShowAll) {
						continue;
					}
					decimal splitAmt=(isPayAmtZeroUponEntering ? charge.AmountEnd : (decimal)payCur.PayAmt);
					splitData=CreatePaySplit(charge.AccountEntryNum,splitAmt,payCur,textAmount,listSplitsCur,listAllCharges);
					listSplitsCur=splitData.ListSplitsCur;
					listAllCharges=splitData.ListAccountCharges;
					payCur=splitData.Payment;						
				}
			}
			return splitData??new PayResults { ListAccountCharges=listAllCharges, ListSplitsCur=listSplitsCur, Payment=payCur };
		}

		public static PayResults CreatePaySplit(long accountEntryNum,decimal payAmt,Payment payCur,decimal textAmount,List<PaySplit> listSplitsCur
			,List<AccountEntry> listCharges,bool isManual=false) 
		{
			//No remoting role check; no call to db.
			PayResults createdSplit=new PayResults();
			AccountEntry charge=listCharges.FirstOrDefault(x => x.AccountEntryNum==accountEntryNum);//get charge from passed list so it can be modified.
			createdSplit.ListSplitsCur=listSplitsCur;
			createdSplit.ListAccountCharges=listCharges;
			createdSplit.Payment=payCur;
			PaySplit split=new PaySplit();
			split.DatePay=DateTime.Today;
			if(charge.GetType()==typeof(Procedure)) {//Row selected is a Procedure.
				split.ProcNum=charge.PriKey;
				if(((Procedure)charge.Tag).ProcStatus==ProcStat.TP) {
					split.UnearnedType=PrefC.GetLong(PrefName.TpUnearnedType);
				}
			}
			else if(charge.GetType()==typeof(PayPlanCharge)) {//Row selected is a PayPlanCharge.
				PayPlanCharge ppChargeCur=(PayPlanCharge)charge.Tag;
				PayPlan payplan=PayPlans.GetOne(ppChargeCur.PayPlanNum);
				decimal payAmtCur;
				if(payplan.IsDynamic) {
					createdSplit.ListSplitsCur.AddRange(
						PaySplits.CreateSplitsForDynamicPayPlan(payCur.PayNum,payCur.PayAmt,charge,listCharges,payAmt,false,out payAmtCur));
				}
				else {
					createdSplit.ListSplitsCur.AddRange(
					PaySplits.CreateSplitForPayPlan(payCur.PayNum,payCur.PayAmt,charge
						,PayPlanCharges.GetChargesForPayPlanChargeType(ppChargeCur.PayPlanNum,PayPlanChargeType.Credit),listCharges,payAmt,false,out payAmtCur));
				}
				payCur.PayAmt=(double)payAmtCur;
				return createdSplit;
			}
			else if(charge.Tag.GetType()==typeof(Adjustment)) {//Row selected is an Adjustment.
				split.AdjNum=charge.PriKey;
			}
			else {//PaySplits and overpayment refunds.
				//Do nothing, nothing to link.
			}
			if(isManual) {
				split.SplitAmt=(double)payAmt;
				charge.AmountEnd-=payAmt;
			}
			else { 
				decimal chargeAmt=charge.AmountEnd;
				if(Math.Abs(chargeAmt)<Math.Abs(payAmt) || textAmount==0) {//Full payment of charge
					split.SplitAmt=(double)chargeAmt;
					charge.AmountEnd=0;//Reflect payment in underlying datastructure
				}
				else {//Partial payment of charge
					charge.AmountEnd-=payAmt;
					split.SplitAmt=(double)payAmt;
				}
			}
			if(PrefC.HasClinicsEnabled) {
				split.ClinicNum=charge.ClinicNum;
			}
			payCur.PayAmt-=split.SplitAmt;
			split.ProvNum=charge.ProvNum;
			split.PatNum=charge.PatNum;
			split.PayNum=payCur.PayNum;
			charge.SplitCollection.Add(split);
			createdSplit.ListSplitsCur.Add(split);
			createdSplit.Payment=payCur;
			return createdSplit;
		}

		///<summary>Checks if the amtEntered will result in the AccountEntry.Tag procedure to be overpaid. Returns false if AccountEntry.Tag is not a procedure.</summary>
		public static bool IsProcOverPaid(decimal amtEntered,AccountEntry accountEntry) {
			if(accountEntry.GetType()!=typeof(Procedure)) {
				return false;//AccountEntry is not a procedure. Return false.
			}
			//Only look for explicitly linked paysplits when calculating amount remaining. 
			decimal amtOverpay=accountEntry.AmountOriginal-(decimal)accountEntry.SplitCollection.Sum(x=>x.SplitAmt)-amtEntered;
			return amtOverpay.IsLessThanZero();
		}

		///<summary>Finds and updates matching charge's amount end and adds the current paySplit to the split collection for the charge.
		///Updates the underlying data structures when a manual split is created or edited.</summary>
		public static void UpdateForManualSplit(PaySplit paySplit,List<AccountEntry> listAccountCharges,long payPlanChargeNum=0,long prePaymentNum=0,bool isAllCharges=false) {
			//No remoting role check; no call to db
			//Find the charge row for this new split.
			//Locate a charge to apply the credit to, if a reasonable match exists.
			for(int i=0;i<listAccountCharges.Count;i++) {
				AccountEntry charge=listAccountCharges[i];
				if(charge.AmountEnd==0 && !isAllCharges) {
					continue;
				}
				bool isMatchFound=false;
				//New Split is for this proc (but not attached to a payplan)
				if(charge.GetType()==typeof(Procedure) && charge.PriKey==paySplit.ProcNum && paySplit.PayPlanNum==0) {
					isMatchFound=true;
				}
				else if(charge.GetType()==typeof(Adjustment) //New split is for this adjust
						&& paySplit.ProcNum==0 && paySplit.PayPlanNum==0 //Both being 0 is the only way we can tell it's for an Adj
						&& paySplit.DatePay==charge.Date
						&& paySplit.ProvNum==charge.ProvNum
						&& paySplit.PatNum==charge.PatNum
						&& paySplit.ClinicNum==charge.ClinicNum)
				{
					isMatchFound=true;
				}
				else if(charge.GetType()==typeof(PayPlanCharge) && charge.PriKey==payPlanChargeNum) {//split is for this payplancharge
					isMatchFound=true;
				}
				else if(charge.GetType()==typeof(PaySplit) && (charge.PriKey==prePaymentNum || charge.PriKey==paySplit.FSplitNum)) {//prepayment
					isMatchFound=true;
				}
				if(isMatchFound) {
					charge.AmountEnd=charge.AmountEnd-(decimal)paySplit.SplitAmt;
					PaySplit splitForCollection=paySplit.Copy();
					splitForCollection.AccountEntryAmtPaid=(decimal)paySplit.SplitAmt;
					charge.SplitCollection.Add(splitForCollection);
				}
				//If none of these, it's unattached to the best of our knowledge.
			}
		}
		#endregion

		#region AutoSplit
		/// <summary>Leave loadData blank for doRefreshData to be true and get a new copy of the objects.</summary>
		public static AutoSplit AutoSplitForPayment(List<long> listPatNums,long patCurNum,List<PaySplit> listSplitsCur,Payment payCur,
			List<AccountEntry> listEntriesLoading,bool isIncomeTxfr,bool isPatPrefer,LoadData loadData,bool doAutoSplit=true,bool doIncludeExplicitCreditsOnly=false) 
		{
			ConstructResults constructResults=ConstructAndLinkChargeCredits(listPatNums,patCurNum,listSplitsCur,payCur
				,listEntriesLoading,isIncomeTxfr,isPatPrefer,loadData,doIncludeExplicitCreditsOnly);
			AutoSplit autoSplit=AutoSplitForPayment(constructResults,doAutoSplit);
			return autoSplit;
		}

		public static AutoSplit AutoSplitForPayment(ConstructResults constructResults,bool doAutoSplit=true) 
		{
			AutoSplit autoSplitData=new AutoSplit();
			List<PayPlanCharge> listPayPlanCharges=constructResults.ListPayPlanCharges;
			autoSplitData.ListAccountCharges=constructResults.ListAccountCharges;
			autoSplitData.ListSplitsCur=constructResults.ListSplitsCur;
			autoSplitData.Payment=constructResults.Payment;
			//Create Auto-splits for the current payment to any remaining non-zero charges FIFO by date.
			if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.DontEnforce) {
				return autoSplitData;
			}
			if(!doAutoSplit) {
				return autoSplitData;
			}
			#region Auto-Split Current Payment
			double payAmt=autoSplitData.Payment.PayAmt;//keep track of the money that can be allocated for this payment.
			long payNum=autoSplitData.Payment.PayNum;
			//At this point we have a list of procs, positive adjustments, and payplancharges that require payment if the Amount>0.   
			//Create and associate new paysplits to their respective charge items.
			PaySplit split;
			for(int i=0;i<autoSplitData.ListAccountCharges.Count;i++) {
				if(payAmt==0) {
					break;
				}
				AccountEntry charge=autoSplitData.ListAccountCharges[i];
				if(Math.Round(charge.AmountEnd,3)<=0) {
					continue;//Skip charges which are already paid or negative.
				}
				if(payAmt<0 && Math.Round(charge.AmountEnd,3)>0) {//If they're different signs, don't make any guesses.  
					//This can happen if the user has less available than there are current splits for.
					//Remaining credits will always be all of one sign.
					return autoSplitData;//Will be empty
				}
				if(charge.GetType()==typeof(Procedure) && ((Procedure)charge.Tag).ProcStatus==ProcStat.TP) {
					continue;//do not auto split to TP procs. 
				}
				split=new PaySplit();
				split.IsNew=true;
				if(charge.GetType()==typeof(PayPlanCharge)) { //payments are allocated differently for payment plan charges
					//it's an autosplit, so pass in 0 for payAmt
					PayPlan payplan=PayPlans.GetOne(((PayPlanCharge)charge.Tag).PayPlanNum);
					PayPlanVersions payPlanVer=(PayPlanVersions)PrefC.GetInt(PrefName.PayPlansVersion);
					if(payPlanVer!=PayPlanVersions.AgeCreditsAndDebits
						|| (payPlanVer==PayPlanVersions.AgeCreditsAndDebits && !payplan.IsClosed)
						|| payplan.IsDynamic)//dynamic payment plans still associate payments to the payment plan, even once closed.
					{ 
						decimal payAmtCur;
						if(payplan.IsDynamic) {
							autoSplitData.ListAutoSplits.AddRange(PaySplits.CreateSplitsForDynamicPayPlan(payNum,payAmt,charge,autoSplitData.ListAccountCharges
								,0,true,out payAmtCur));
						}
						else {
							autoSplitData.ListAutoSplits.AddRange(PaySplits.CreateSplitForPayPlan(payNum,payAmt,charge,
								listPayPlanCharges.Where(x => x.ChargeType==PayPlanChargeType.Credit && x.PayPlanNum==payplan.PayPlanNum).ToList(),
								autoSplitData.ListAccountCharges,0,true,out payAmtCur));
						}
						payAmt=(double)payAmtCur;
					}
					continue;
				}
				else {
					if((double)Math.Abs(charge.AmountEnd)<Math.Abs(payAmt)) {//charge has "less" than the payment, use partial payment.
						split.SplitAmt=(double)charge.AmountEnd;
						payAmt=Math.Round(payAmt-(double)charge.AmountEnd,3);
						charge.AmountEnd=0;
					}
					else {//Use full payment
						split.SplitAmt=payAmt;
						charge.AmountEnd-=(decimal)payAmt;
						payAmt=0;
					}
				}
				split.DatePay=autoSplitData.Payment.PayDate;
				split.PatNum=charge.PatNum;
				split.ProvNum=charge.ProvNum;
				if(PrefC.HasClinicsEnabled) {
					split.ClinicNum=charge.ClinicNum;
				}
				if(charge.GetType()==typeof(Procedure)) {
					split.ProcNum=charge.PriKey;
				}
				else if(charge.GetType()==typeof(PayPlanCharge)) {
					split.PayPlanNum=((PayPlanCharge)charge.Tag).PayPlanNum;
				}
				else if(charge.GetType()==typeof(Adjustment)) {
					split.AdjNum=((Adjustment)charge.Tag).AdjNum;
				}
				split.PayNum=autoSplitData.Payment.PayNum;
				charge.SplitCollection.Add(split);
				autoSplitData.ListAutoSplits.Add(split);
			}
			if(autoSplitData.ListAutoSplits.Count==0 && autoSplitData.ListSplitsCur.Count==0 && payAmt!=0) {//Ensure at least 1 autosplit if payAmt was entered
				split=new PaySplit();
				split.SplitAmt=payAmt;
				payAmt=0;
				split.DatePay=autoSplitData.Payment.PayDate;
				split.PatNum=autoSplitData.Payment.PatNum;
				if(PrefC.IsODHQ) {
					split.ProvNum=7;//Jordan's ProvNum
				}
				else { 
					split.ProvNum=0;
				}
				split.UnearnedType=PrefC.GetLong(PrefName.PrepaymentUnearnedType);//Use default unallocated type
				if(PrefC.HasClinicsEnabled) {
					split.ClinicNum=autoSplitData.Payment.ClinicNum;
				}
				split.PayNum=autoSplitData.Payment.PayNum;
				autoSplitData.ListAutoSplits.Add(split);
			}
			if(payAmt != 0) {//Create an unallocated split if there is any remaining payment amount.
				split=new PaySplit();
				split.SplitAmt=payAmt;
				payAmt=0;
				split.DatePay=autoSplitData.Payment.PayDate;
				split.PatNum=autoSplitData.Payment.PatNum;
				if(PrefC.IsODHQ) {
					split.ProvNum=7;//Jordan's ProvNum
				}
				else { 
					split.ProvNum=0;
				}
				split.UnearnedType=PrefC.GetLong(PrefName.PrepaymentUnearnedType);//Use default unallocated type
				if(PrefC.HasClinicsEnabled) {
					split.ClinicNum=autoSplitData.Payment.ClinicNum;
				}
				split.PayNum=autoSplitData.Payment.PayNum;
				autoSplitData.ListAutoSplits.Add(split);
			}
			#endregion Auto-Split Current Payment
			autoSplitData.Payment.PayAmt=payAmt;
			return autoSplitData;
		}
		#endregion

		#region AccountEntry
		///<summary>Sorts similar to account module (AccountModules.cs, AccountLineComparer).Groups procedures together, then Adjustments, then everything else.</summary>
		private static int AccountEntrySort(AccountEntry x,AccountEntry y) {
			if(x.Date==y.Date) {
				if(x.GetType()==typeof(Procedure) && y.GetType()!=typeof(Procedure)) {
					return -1;
				}
				if(x.GetType()!=typeof(Procedure) && y.GetType()==typeof(Procedure)) {
					return 1;
				}
				if(x.GetType()==typeof(Adjustment) && y.GetType()!=typeof(Adjustment)) {
					return -1;
				}
				if(x.GetType()!=typeof(Adjustment) && y.GetType()==typeof(Adjustment)) {
					return 1;
				}
			}
			return x.Date.CompareTo(y.Date);
		}

		public static List<Procedure> GetProcsForAccountEntries(List<AccountEntry> listAccountEntry) {
			List<Procedure> listProcs=new List<Procedure>();
			List<AccountEntry> listAccountEntryProcs=listAccountEntry.FindAll(x => x.GetType()==typeof(Procedure));
			foreach(AccountEntry entry in listAccountEntryProcs) {
				listProcs.Add((Procedure)entry.Tag);
			}
			return listProcs;
		}

		public static List<AccountEntry> CreateAccountEntries(List<Procedure> listProcs) {
			//No remoting role check; no call to db
			List<AccountEntry> listAccountEntries=new List<AccountEntry>();
			foreach(Procedure proc in listProcs) {
				listAccountEntries.Add(new AccountEntry(proc));
			}
			return listAccountEntries;
		}
		#endregion

		#region Income Transfer
		public static IncomeTransferData CreatePayplanLoop(List<AccountEntry> listPosCharges,List<AccountEntry> listNegCharges
			,List<AccountEntry> listAccountEntries,long payNum,List<PayPlanCharge> listPayPlanCredits,DateTime datePay) 
		{
			//0. Go through pos charges.  Find charges with payplannums and perform the below process with all things within the same payplan.
			//Find all unique payplan nums.
			IncomeTransferData transferSplits=new IncomeTransferData();
			Dictionary<long,List<AccountEntry>> dictPayPlanEntries=
				listPosCharges.GroupBy(x => (x.GetType()==typeof(PayPlanCharge) ? ((PayPlanCharge)x.Tag).PayPlanNum : 
					x.GetType()==typeof(PaySplit) ? ((PaySplit)x.Tag).PayPlanNum : 0))
				.Where(x => x.Key!=0)
				.ToDictionary(x => x.Key,x => x.ToList());
			foreach(KeyValuePair<long,List<AccountEntry>> kvp in dictPayPlanEntries) {
				List<AccountEntry> listMatchingForPayPlan=listNegCharges.FindAll(x => 
					(x.GetType()==typeof(PayPlanCharge) && ((PayPlanCharge)x.Tag).PayPlanNum==kvp.Key)
					||	(x.GetType()==typeof(PaySplit) && ((PaySplit)x.Tag).PayPlanNum==kvp.Key)
				);
				transferSplits.MergeIncomeTransferData(CreateTransferLoop(kvp.Value,listMatchingForPayPlan,listAccountEntries,payNum,listPayPlanCredits
					,datePay)); 
			}
			return transferSplits;
		}

		private static IncomeTransferData TransferLoopHelper(List<AccountEntry> listPosCharges,List<AccountEntry> listNegCharges
			,List<AccountEntry> listAccountEntries,long payNum,List<PayPlanCharge> listCredits,bool doMatchProv,bool doMatchPat
			,bool doMatchClinic,long guarNum,DateTime datePay) 
		{
			IncomeTransferData transferData=new IncomeTransferData();
			foreach(AccountEntry posCharge in listPosCharges) {
				if(posCharge.AmountEnd<=0) {
					continue;
				}
				foreach(AccountEntry negCharge in listNegCharges) {
					if(negCharge.AmountEnd>=0) {
						continue;
					}
					if(doMatchProv && posCharge.ProvNum!=negCharge.ProvNum) {
						continue;
					}
					if(doMatchPat && posCharge.PatNum!=negCharge.PatNum) {
						continue;
					}
					if(doMatchClinic && posCharge.ClinicNum!=negCharge.ClinicNum) {
						continue;
					}
					transferData.MergeIncomeTransferData(CreateTransferHelper(posCharge,negCharge,listAccountEntries,payNum,listCredits,guarNum,datePay));
				}
			}
			return transferData;
		}

		public static IncomeTransferData CreateTransferLoop(List<AccountEntry> listPosCharges,List<AccountEntry> listNegCharges
			,List<AccountEntry> listAccountEntries,long payNum,List<PayPlanCharge> listCredits,DateTime datePay,long guarNum=0) 
		{
			IncomeTransferData transferResultsTotal=new IncomeTransferData();
			//Positive charges need to be paid off
			//Eight separate loops going through the entire data set, each loop will have different requirements.
			//1. Prov/Pat/Clinic
			transferResultsTotal.MergeIncomeTransferData(TransferLoopHelper(listPosCharges,listNegCharges,listAccountEntries,payNum,listCredits,
				doMatchProv:true,doMatchPat:true,doMatchClinic:true,guarNum,datePay));
			//2. Prov/Pat
			transferResultsTotal.MergeIncomeTransferData(TransferLoopHelper(listPosCharges,listNegCharges,listAccountEntries,payNum,listCredits,
				doMatchProv:true,doMatchPat:true,doMatchClinic:false,guarNum,datePay));
			//3. Prov/Clinic
			transferResultsTotal.MergeIncomeTransferData(TransferLoopHelper(listPosCharges,listNegCharges,listAccountEntries,payNum,listCredits,
				doMatchProv:true,doMatchPat:false,doMatchClinic:true,guarNum,datePay));
			//4. Pat/Clinic
			transferResultsTotal.MergeIncomeTransferData(TransferLoopHelper(listPosCharges,listNegCharges,listAccountEntries,payNum,listCredits,
				doMatchProv:false,doMatchPat:true,doMatchClinic:true,guarNum,datePay));
			//5. Prov
			transferResultsTotal.MergeIncomeTransferData(TransferLoopHelper(listPosCharges,listNegCharges,listAccountEntries,payNum,listCredits,
				doMatchProv:true,doMatchPat:false,doMatchClinic:false,guarNum,datePay));
			//6. Pat
			transferResultsTotal.MergeIncomeTransferData(TransferLoopHelper(listPosCharges,listNegCharges,listAccountEntries,payNum,listCredits,
				doMatchProv:false,doMatchPat:true,doMatchClinic:false,guarNum,datePay));
			//7. Clinic
			transferResultsTotal.MergeIncomeTransferData(TransferLoopHelper(listPosCharges,listNegCharges,listAccountEntries,payNum,listCredits,
				doMatchProv:false,doMatchPat:false,doMatchClinic:true,guarNum,datePay));
			//8. Nothing
			transferResultsTotal.MergeIncomeTransferData(TransferLoopHelper(listPosCharges,listNegCharges,listAccountEntries,payNum,listCredits,
				doMatchProv:false,doMatchPat:false,doMatchClinic:false,guarNum,datePay));
			//9. Transfer all remaining excess to unearned (but keep the same pat/prov/clinic, unless they're on rigorous accounting in which case go to 0 prov)
			foreach(AccountEntry negCharge in listNegCharges) {
				if(negCharge.AmountEnd>=0 || !negCharge.GetType().In(typeof(PaySplit),typeof(Procedure),typeof(Adjustment))) {
					continue;
				}
				if(negCharge.GetType()==typeof(PaySplit) && ((PaySplit)negCharge.Tag).UnearnedType!=0) {//Only perform transfer with non-unearned splits.
					continue;
				}
				decimal amt=Math.Abs(negCharge.AmountEnd);
				PaySplit negSplit=new PaySplit();
				if(negCharge.GetType()==typeof(PaySplit)) {
					negSplit.ProcNum=((PaySplit)negCharge.Tag).ProcNum;
					negSplit.UnearnedType=((PaySplit)negCharge.Tag).UnearnedType;
					negSplit.FSplitNum=negCharge.PriKey;
				}
				else if(negCharge.GetType()==typeof(Procedure)) {
					if(((Procedure)negCharge.Tag).ProcFee < 0) {
						continue;//do not use negative procedures as a souce of income. 
					}
					negSplit.ProcNum=((Procedure)negCharge.Tag).ProcNum;
					negSplit.FSplitNum=0;
				}
				else if(negCharge.GetType()==typeof(Adjustment)) {
					negSplit.AdjNum=((Adjustment)negCharge.Tag).AdjNum;
					negSplit.FSplitNum=0;
				}
				//we may also need to handle claimspaybytotal and payplancharges in the future.
				negSplit.DatePay=DateTimeOD.Today;
				negSplit.ClinicNum=negCharge.ClinicNum;
				negSplit.PatNum=negCharge.PatNum;
				negSplit.PayPlanNum=0;
				negSplit.PayNum=payNum;
				negSplit.ProvNum=negCharge.ProvNum;
				negSplit.SplitAmt=0-(double)amt;
				PaySplit posSplit=new PaySplit();//the split that's going to unearned
				posSplit.DatePay=DateTimeOD.Today;
				posSplit.ClinicNum=negSplit.ClinicNum;
				posSplit.FSplitNum=0;
				posSplit.PatNum=negSplit.PatNum;
				posSplit.PayPlanNum=0;
				posSplit.PayNum=payNum;
				posSplit.ProcNum=0;
				posSplit.ProvNum=PrefC.GetBool(PrefName.AllowPrepayProvider) ? negSplit.ProvNum : 0 ;
				posSplit.AdjNum=0;
				posSplit.SplitAmt=(double)amt;
				posSplit.UnearnedType=(negSplit.UnearnedType==0 ? PrefC.GetLong(PrefName.PrepaymentUnearnedType) : negSplit.UnearnedType);
				transferResultsTotal.ListSplitsCur.AddRange(new[] { posSplit,negSplit });
				transferResultsTotal.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated(negSplit,posSplit));
				negCharge.SplitCollection.Add(negSplit);
				negCharge.AmountEnd+=amt;
			}
			return transferResultsTotal;
		}

		///<summary>Creates and links paysplits with micro-allocations based on the charges passed in.  
		///Constructs the paysplits with all necessary information depending on the type of the charges.
		///Returns true if attempting to create invalid splits and rigorous accounting is enabled.</summary>
		private static IncomeTransferData CreateTransferHelper(AccountEntry posCharge,AccountEntry negCharge
			,List<AccountEntry> listAccountEntriesForPat,long payNum,List<PayPlanCharge> listCredits,long guarNum,DateTime datePay) 
		{
			IncomeTransferData transferSplits=new IncomeTransferData();
			if(negCharge.GetType()==typeof(Procedure) && ((Procedure)negCharge.Tag).ProcFee < 0) {
				return transferSplits;//do not use negative procedures as sources of income. 
			}
			decimal amt=Math.Min(Math.Abs(posCharge.AmountEnd),Math.Abs(negCharge.AmountEnd));
			if(amt.IsEqual(0)) {
				return transferSplits;//there is no income to transfer
			}
			if(posCharge.GetType()==typeof(PayPlanCharge)) {//Allocate to payplancharges in a special way
				transferSplits=TransferPayPlanChargeHelper(posCharge,negCharge,listAccountEntriesForPat,payNum,listCredits,amt,datePay);
			}
			else {
				transferSplits=new IncomeTransferData();
				PaySplit negSplit=new PaySplit();
				PaySplit posSplit=new PaySplit();
				if(posCharge.GetType()==typeof(PaySplit)) {
					//it's a negative paysplit - Likely used for a refund or something.
					//We need to use it as a source and transfer it to overpaid procedures or credit adjustments only.
					if(negCharge.GetType()==typeof(Adjustment) || negCharge.GetType()==typeof(Procedure)) {
						//Since the source of the money is a positive charge (negative split) we have to reverse the posSplit and negSplit associations.
						//In this particular case it is neg source -> pos split -> neg split -> destination proc/adj
						negSplit=new PaySplit();
						negSplit.DatePay=datePay;
						negSplit.ClinicNum=negCharge.ClinicNum;
						negSplit.FSplitNum=0;//Can't set FSplitNum yet, the positive isn't inserted yet. 
						negSplit.PatNum=negCharge.PatNum;
						negSplit.PayPlanNum=0;
						negSplit.PayNum=payNum;
						negSplit.ProcNum=negCharge.GetType()==typeof(Procedure) ? negCharge.PriKey : 0;//Refund is headed to a procedure.
						negSplit.ProvNum=negCharge.ProvNum;
						negSplit.AdjNum=negCharge.GetType()==typeof(Adjustment) ? negCharge.PriKey : 0;//Refund is headed to an adjustment.
						negSplit.SplitAmt=0-(double)amt;
						negSplit.UnearnedType=0;
						posSplit=new PaySplit();
						posSplit.DatePay=datePay;
						posSplit.ClinicNum=posCharge.ClinicNum;
						posSplit.FSplitNum=posCharge.PriKey;//FSplitNum is the refund splitnum.
						if(posCharge.PriKey==0) {
							//hasn't been inserted into the database yet, so make split association for it instead.
							transferSplits.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated((PaySplit)posCharge.Tag,posSplit));
						}
						posSplit.PatNum=posCharge.PatNum;
						posSplit.PayPlanNum=0;
						posSplit.PayNum=payNum;
						posSplit.ProcNum=0;
						posSplit.ProvNum=posCharge.ProvNum;
						posSplit.AdjNum=0;
						posSplit.SplitAmt=(double)amt;
						posSplit.UnearnedType=((PaySplit)posCharge.Tag).UnearnedType;
						transferSplits.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated(posSplit,negSplit));
						transferSplits.ListSplitsCur.AddRange(new[] { negSplit,posSplit });
						negCharge.SplitCollection.Add(negSplit);
						posCharge.SplitCollection.Add(posSplit);
						posCharge.AmountEnd-=amt;
						negCharge.AmountEnd+=amt;
					}
					transferSplits.HasInvalidSplits=false;
					return transferSplits;
				}
				else{
					posSplit=new PaySplit();
					posSplit.DatePay=datePay;
					posSplit.ClinicNum=posCharge.ClinicNum;
					posSplit.FSplitNum=0;//Can't set FSplitNum just yet, the neg split isn't inserted so there is no FK yet.
					posSplit.PatNum=posCharge.PatNum;
					posSplit.PayPlanNum=0;
					posSplit.PayNum=payNum; 
					posSplit.AdjNum=0;//will be overwritten by an actual adjustment num if the positive charge is an adjustment that does not have a procedure
					if(posCharge.GetType()==typeof(Procedure)) {//PosCharge may or may not be a proc.
						posSplit.ProcNum=posCharge.PriKey;//If it is proc, use ProcNum.
					}
					else if(posCharge.GetType()==typeof(PaySplit)) {
						posSplit.ProcNum=((PaySplit)posCharge.Tag).ProcNum;//If it is a paysplit, use the paysplit's procnum.
					}
					else if(posCharge.GetType()==typeof(Adjustment)) {
						posSplit.ProcNum=((Adjustment)posCharge.Tag).ProcNum;
						if(posSplit.ProcNum==0) {
							posSplit.AdjNum=((Adjustment)posCharge.Tag).AdjNum;
						}
					}
					else if(posCharge.GetType()==typeof(ClaimProc)) {
						posSplit.ProcNum=((ClaimProc)posCharge.Tag).ProcNum;
					}
					else {
						posSplit.ProcNum=0;
					}
					posSplit.ProvNum=posCharge.ProvNum;
					posSplit.SplitAmt=(double)amt;
					//Unearned type will likely be 0, but if we make a positive split to ProvNum=0, use default unearned type.
					//explicit because of old adjustments that didn't have to have a provider.
					if(posSplit.ProvNum==0 && posSplit.AdjNum==0 && posSplit.ProcNum==0) {
						posSplit.UnearnedType=PrefC.GetLong(PrefName.PrepaymentUnearnedType);
					}
					else if(posCharge.GetType()==typeof(PaySplit)) {
						posSplit.UnearnedType=((PaySplit)posCharge.Tag).UnearnedType;
					}
					else {
						posSplit.UnearnedType=0;
					}
					negSplit=new PaySplit();
					negSplit.DatePay=datePay;
					negSplit.ClinicNum=negCharge.ClinicNum;
					negSplit.FSplitNum=0;
					negSplit.PatNum=negCharge.PatNum;
					negSplit.PayPlanNum=0;
					negSplit.PayNum=payNum;
					negSplit.AdjNum=0;
					//Money may be coming from an overpaid procedure instead of paysplit.
					if(negCharge.GetType()==typeof(PaySplit)) {
						negSplit.ProcNum=((PaySplit)negCharge.Tag).ProcNum;
						negSplit.FSplitNum=negCharge.PriKey;
						if(negCharge.PriKey==0) {
							//money is coming from a split that we created earlier, and hasn't been inserted into the database yet. Make a split association for it.
							transferSplits.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated((PaySplit)negCharge.Tag,negSplit));
						}
					}
					else if(negCharge.GetType()==typeof(Procedure)) {
						negSplit.ProcNum=negCharge.PriKey;
					}
					else if(negCharge.GetType()==typeof(Adjustment)) {
						negSplit.ProcNum=((Adjustment)negCharge.Tag).ProcNum;
						if(negSplit.ProcNum==0) {
							negSplit.AdjNum=((Adjustment)negCharge.Tag).AdjNum;
						}
					}
					else if(negCharge.GetType()==typeof(ClaimProc)) {
						negSplit.ProcNum=((ClaimProc)negCharge.Tag).ProcNum;
					}
					else {
						negSplit.ProcNum=0;
					}
					negSplit.ProvNum=negCharge.ProvNum;
					negSplit.SplitAmt=0-(double)amt;
					if(negCharge.GetType()==typeof(PaySplit)) {//If money is coming from paysplit, use its unearned type (if any)
						negSplit.UnearnedType=((PaySplit)negCharge.Tag).UnearnedType;
					}
					else {
						negSplit.UnearnedType=0;
					}
					if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
						//split being created needs to have a procedure with a provider, or an adjustment with a provider 
						//or unearned type (optionally w/ provider) BUT if unearned, cannot have a procedure or adjustment. 
						if((Math.Sign(posSplit.ProcNum)!=Math.Sign(posSplit.ProvNum) && Math.Sign(posSplit.AdjNum)!=Math.Sign(posSplit.ProvNum))
							|| (posSplit.UnearnedType==0 && posSplit.ProvNum==0))//Allow the negative unearned split to have a provider to fix bad scenarios
						{
							transferSplits.HasInvalidSplits=true;
							transferSplits.SummaryText+=$"Due to Rigorous Accounting, one or more invalid transactions have been cancelled for the " +
								$"family of PatNum: "+$"{guarNum}.  Please fix those manually.\r\n";
							return transferSplits;
						}
					}
					if(!PrefC.GetBool(PrefName.AllowPrepayProvider)) {//this pref used to only be available for enforce fully, but now is for all types.
						if(posSplit.UnearnedType!=0 && posSplit.ProvNum!=0) {//Allow the negative unearned split to have a provider to fix bad scenarios
							transferSplits.HasInvalidSplits=true;
							return transferSplits;
						}
					}
					transferSplits.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated(negSplit,posSplit));
					transferSplits.ListSplitsCur.AddRange(new List<PaySplit>() {posSplit, negSplit});
					negCharge.SplitCollection.Add(negSplit);
					posCharge.SplitCollection.Add(posSplit);
					posCharge.AmountEnd-=amt;
					negCharge.AmountEnd+=amt;
					transferSplits.SummaryText+=$"Paysplit of {negSplit.SplitAmt.ToString("f")} created for PatNum: {negSplit.PatNum}\r\n"
						+$"Paysplit of {posSplit.SplitAmt.ToString("f")} created for PatNum: {posSplit.PatNum}\r\n";
				}
			}
			transferSplits.HasInvalidSplits=false;
			return transferSplits;
		}

		private static IncomeTransferData TransferPayPlanChargeHelper(AccountEntry posCharge,AccountEntry negCharge
			,List<AccountEntry> listAccountEntriesForPat,long payNum,List<PayPlanCharge> listCredits,decimal amt,DateTime datePay) 
		{
			IncomeTransferData transferSplits=new IncomeTransferData();
			negCharge.AmountEnd+=amt;
			PaySplit negSplit=new PaySplit();
			negSplit.DatePay=datePay;
			negSplit.ClinicNum=negCharge.ClinicNum;
			negSplit.FSplitNum=0;
			if(negCharge.GetType()==typeof(PaySplit)) {
				negSplit.FSplitNum=negCharge.PriKey;
				if(negCharge.PriKey==0) {//hasn't been inserted into the database yet.
					transferSplits.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated((PaySplit)negCharge.Tag,negSplit));
				}
			}
			negSplit.PatNum=negCharge.PatNum;
			negSplit.PayPlanNum=negCharge.GetType()==typeof(PaySplit) ? ((PaySplit)negCharge.Tag).PayPlanNum : 0;
			negSplit.PayNum=payNum;
			negSplit.ProcNum=negCharge.GetProcNumFromTag();
			negSplit.ProvNum=negCharge.ProvNum;
			negSplit.AdjNum=negCharge.GetType()==typeof(Adjustment) ? negCharge.PriKey : 0;//Money may be coming from an adjustment.
			negSplit.SplitAmt=0-(double)amt;
			//If money is coming from paysplit, use its unearned type (if any)
			negSplit.UnearnedType=negCharge.GetType()==typeof(PaySplit) ? ((PaySplit)negCharge.Tag).UnearnedType : 0;
			negCharge.SplitCollection.Add(negSplit);
			decimal amount;
			PayPlan payplan=PayPlans.GetOne(((PayPlanCharge)posCharge.Tag).PayPlanNum);
			List<PayPlanCharge> listCreditsForPayPlan=listCredits.FindAll(x => x.PayPlanNum==((PayPlanCharge)posCharge.Tag).PayPlanNum);
			List<PaySplit> listSplitsForPayPlan=new List<PaySplit>();
			if(payplan.IsDynamic) {
				listSplitsForPayPlan=PaySplits.CreateSplitsForDynamicPayPlan(payNum,(double)amt,posCharge,listAccountEntriesForPat,amt,false,out amount);
			}
			else {
				listSplitsForPayPlan=PaySplits.CreateSplitForPayPlan(payNum,(double)amt,posCharge,listCreditsForPayPlan,listAccountEntriesForPat,amt
					,false,out amount);
			}
			if(listSplitsForPayPlan.Count>0) {
				foreach(PaySplit split in listSplitsForPayPlan) {
					split.FSplitNum=0;
					split.PayNum=payNum;
					split.DatePay=datePay;
					posCharge.SplitCollection.Add(split);
					transferSplits.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated(negSplit,split));
					transferSplits.ListSplitsCur.Add(split);
					transferSplits.SummaryText+=$"Paysplit of {split.SplitAmt.ToString("f")} created for PatNum: {split.PatNum}\r\n";
				}
				transferSplits.ListSplitsCur.Add(negSplit);
				transferSplits.SummaryText+=$"Paysplit of {negSplit.SplitAmt.ToString("f")} created for PatNum: {negSplit.PatNum}\r\n";
			}
			return transferSplits;
		}

		/// <summary> This is prepayment allocation reversal. Because of this, items may need to be moved back to the list of positive charges. 
		/// The returned list will be any charges that need to be added back to the list of positive charges. </summary>
		private static IncomeTransferData TransferUnearnedHelper(AccountEntry positiveCharge,long paymentNumCur,List<AccountEntry> listEntriesForPat) {
			//Special case where unearned has been over allocated. 
			//Negative charge (split) masquarading around as a positve split becasuse it was overallocated. 
			IncomeTransferData transferData=new IncomeTransferData();
			decimal amountOverallocated=positiveCharge.AmountEnd;
			PaySplit negSplit=new PaySplit();
			PaySplit posSplit=new PaySplit();
			//get what is attached to this original pre-payment
			List<PaySplit> listAttachedToOverallocated=PaySplits.GetSplitsForPrepay(new List<PaySplit>{((PaySplit)positiveCharge.Tag)})
				.OrderByDescending(x => x.DatePay)
				.ToList();//verify this order is correct. We want the most recent first. 
			List<PaySplit> listAttachedElseWhere=PaySplits.GetAllocatedElseWhere(positiveCharge.PriKey);
			foreach(PaySplit offset in listAttachedToOverallocated) {
				if(amountOverallocated.IsEqual(0)) {
					break;
				}
				//attempt to find the most recent split that was allocated from the offset
				List<PaySplit> listAttachedtoOffset=listAttachedElseWhere.FindAll(x => x.FSplitNum==offset.SplitNum)
					.OrderByDescending(x => x.DatePay)
					.ToList();
				AccountEntry productionEntry=null;
				foreach(PaySplit allocated in listAttachedtoOffset) {
					if(amountOverallocated.IsEqual(0)) {
						break;
					}
					if(allocated.ProcNum!=0 && allocated.PayPlanNum==0) {
						//find charge from list to money can be correctly taken away from the production that previously had been attached to overallocation
						productionEntry=listEntriesForPat.FirstOrDefault(x => x.PriKey==allocated.ProcNum);
					}
					else if(allocated.AdjNum!=0) {
						productionEntry=listEntriesForPat.FirstOrDefault(x => x.PriKey==allocated.AdjNum);
					}
					else {
						//if it was not previously explicitly attached to a procedure (that was not on a payment plan) or a positive adjustment, we do
						//not want to handle it at this point in time. 
						transferData.HasIvalidProcWithPayPlan=true;
						return transferData;
					}
					//reverse the split up to the amount overallocated
					decimal reverseAmt=Math.Min(amountOverallocated,(decimal)allocated.SplitAmt);
					//make a positive split to put back to unallocated. Reverse offset. 
					posSplit.DatePay=DateTimeOD.Today;
					posSplit.ClinicNum=allocated.ClinicNum;
					posSplit.FSplitNum=offset.FSplitNum;//Split should be directly attached to the original prepayment positivecharge.PriKey
					posSplit.PatNum=allocated.PatNum;
					posSplit.PayPlanNum=0;
					posSplit.PayNum=paymentNumCur;
					posSplit.ProcNum=0;
					posSplit.ProvNum=positiveCharge.ProvNum;
					posSplit.AdjNum=0;
					posSplit.SplitAmt=(double)reverseAmt;
					posSplit.UnearnedType=offset.UnearnedType;
					//make a negative split to take away from this allocated split (likely taking away from a procedure or other production) Reverse allocation
					negSplit.DatePay=DateTimeOD.Today;
					negSplit.ClinicNum=positiveCharge.ClinicNum;
					negSplit.FSplitNum=0;//Split should be directly attached to the reverse offset we just made. Will be 0 for now since it has not yet been inserted.
					negSplit.PatNum=allocated.PatNum;
					negSplit.PayPlanNum=0;
					negSplit.PayNum=paymentNumCur;
					negSplit.ProcNum=allocated.ProcNum;
					negSplit.ProvNum=allocated.ProvNum;
					negSplit.AdjNum=allocated.AdjNum;
					negSplit.SplitAmt=0-(double)reverseAmt;
					negSplit.UnearnedType=0;
					amountOverallocated-=reverseAmt;
					positiveCharge.AmountEnd-=reverseAmt;
					transferData.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated(posSplit,negSplit));
					transferData.ListSplitsCur.AddRange(new[] { posSplit,negSplit });
					positiveCharge.SplitCollection.Add(posSplit);
					if(productionEntry!=null) {
						productionEntry.AmountEnd+=reverseAmt;
						transferData.ListModifiedProductionEntries.Add(productionEntry);
					}
				}
			}
			return transferData;
		}

		///<summary>Takes unallocated money (splits that do not have procNum,adjNum,fSplitNum,payplanNum, or unearned type) and moves them to unearned 
		///through a transfer that balances to 0. 
		///In detail, loop through all splits. We are treating each split on the account as a parent split. 
		///If the split has children we will look at the childrens values to see if they equate to the parent's value.
		///If the children equate to the parent, we do not need to do anything. We will look at the children splits again when they come up as a 'parent'
		///If The chidren do not equate to the parent, an additional transfer need to be made to unearned to make them equate
		///If the split does not have children, we will look to see if it needs to be transferred to unearned (if it's unallocated) or not. </summary>
		public static IncomeTransferData TransferUnallocatedSplitToUnearned(List<PaySplit> listSplitsAll,long payNum) {
			IncomeTransferData unearnedTransfers=new IncomeTransferData();
			//TODO future job: Eliminate the need for query passed in. Make it optional to have account entries to implicit/explicit linking.
			//List<PaySplit> listSplitsAll=listAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit)).Select(x => (PaySplit)x.Tag).ToList();							
			foreach(PaySplit parentSplit in listSplitsAll) {
				List<PaySplit> listChildrenSplits=listSplitsAll.FindAll(x => x.FSplitNum==parentSplit.SplitNum && x.FSplitNum!=0);
				if(listChildrenSplits.IsNullOrEmpty()) {
					//no children, just evaluate if parent is unallocated.
					if(parentSplit.ProcNum!=0 || parentSplit.AdjNum!=0 || parentSplit.PayPlanNum!=0 || parentSplit.UnearnedType!=0) {
						continue;//split has been allocated, move along.
					}
					unearnedTransfers.MergeIncomeTransferData(TransferUnallocatedSplitHelper(parentSplit,payNum));//split is unallocated, transfer to unearned. 
				}
				else {
					//children exist for this parent split, evaluate their values in comparison.
					double sumChildren=listChildrenSplits.Sum(x => x.SplitAmt);
					double sumParentChildren=parentSplit.SplitAmt + sumChildren;
					if(sumParentChildren.IsZero() || 
						(parentSplit.SplitAmt.IsGreaterThan(0) && parentSplit.UnearnedType!=0 && Math.Abs(sumChildren)<parentSplit.SplitAmt)) 
					{
						continue;//either the children equate, or this is an unearned split that hasn't been fully allocated yet and that is fine.
					}
					if(Math.Abs(parentSplit.SplitAmt) > Math.Abs(sumChildren)) {
						unearnedTransfers.MergeIncomeTransferData(TransferUnallocatedSplitHelper(parentSplit,payNum,sumParentChildren));//under allocated parent
					}
					else {
						unearnedTransfers.MergeIncomeTransferData(TransferUnallocatedSplitHelper(parentSplit,payNum,sumParentChildren*-1));//over allocated parent
					}
				}
			}
			return unearnedTransfers;
		}

		///<summary>Method to encapsulate the creation of a new payment that is specifically meant to store payment information for the unallocated
		///payment transfer.</summary>
		public static long CreateAndInsertUnallocatedPayment(Patient patCur) {
			//user clicked ok and has permisson to save splits to the database. 
			Payment unallocatedTransferPayment=new Payment();
			unallocatedTransferPayment.PatNum=patCur.PatNum;
			unallocatedTransferPayment.PayDate=DateTime.Today;
			unallocatedTransferPayment.ClinicNum=0;
			if(PrefC.HasClinicsEnabled) {//if clinics aren't enabled default to 0
				unallocatedTransferPayment.ClinicNum=Clinics.ClinicNum;
				if((PayClinicSetting)PrefC.GetInt(PrefName.PaymentClinicSetting)==PayClinicSetting.PatientDefaultClinic) {
					unallocatedTransferPayment.ClinicNum=patCur.ClinicNum;
				}
				else if((PayClinicSetting)PrefC.GetInt(PrefName.PaymentClinicSetting)==PayClinicSetting.SelectedExceptHQ) {
					unallocatedTransferPayment.ClinicNum=(Clinics.ClinicNum==0 ? patCur.ClinicNum : Clinics.ClinicNum);
				}
			}
			unallocatedTransferPayment.DateEntry=DateTime.Today;
			unallocatedTransferPayment.PaymentSource=CreditCardSource.None;
			unallocatedTransferPayment.PayAmt=0;
			unallocatedTransferPayment.PayType=0;
			long payNum=Payments.Insert(unallocatedTransferPayment);
			return payNum;
		}

		private static IncomeTransferData TransferUnallocatedSplitHelper(PaySplit splitToTransfer,long payNum,double splitAmtOverride=0) {
			IncomeTransferData transfer=new IncomeTransferData();
			//make an offsetting split that is the opposite sign as the split to transfer. 
			PaySplit offsetSplit=new PaySplit();
			offsetSplit.DatePay=DateTime.Today;
			offsetSplit.ClinicNum=splitToTransfer.ClinicNum;
			offsetSplit.FSplitNum=splitToTransfer.SplitNum;
			if(splitToTransfer.SplitNum==0) {
				transfer.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated(splitToTransfer,offsetSplit));
			}
			offsetSplit.PatNum=splitToTransfer.PatNum;
			offsetSplit.PayPlanNum=splitToTransfer.PayPlanNum;//should usually be 0. Only not when production had been overallocated.
			offsetSplit.PayNum=payNum;
			offsetSplit.ProcNum=splitToTransfer.ProcNum;//should usually be 0
			offsetSplit.ProvNum=splitToTransfer.ProvNum;
			offsetSplit.AdjNum=splitToTransfer.AdjNum;//should ususally be 0
			offsetSplit.SplitAmt=splitAmtOverride==0?(splitToTransfer.SplitAmt*-1):(splitAmtOverride*-1);//should be for the opposite sign as the passed in split.
			offsetSplit.UnearnedType=splitToTransfer.UnearnedType;//this should stay in the case when we're correct unbalanced unearned specifically.
			PaySplit unearnedSplit=new PaySplit();
			unearnedSplit.DatePay=DateTime.Today;
			unearnedSplit.ClinicNum=splitToTransfer.ClinicNum;
			unearnedSplit.FSplitNum=0;//0 because offset split hasn't been inserted into the database yet. Make split associated for this.
			unearnedSplit.PatNum=splitToTransfer.PatNum;
			unearnedSplit.PayPlanNum=0;
			unearnedSplit.PayNum=payNum;
			unearnedSplit.ProvNum=0;
			if(PrefC.GetBool(PrefName.AllowPrepayProvider)==true) {
				unearnedSplit.ProvNum=splitToTransfer.ProvNum;
			}
			unearnedSplit.AdjNum=0;
			unearnedSplit.SplitAmt=offsetSplit.SplitAmt*-1;//should be the opposite sign as the offset split.
			unearnedSplit.UnearnedType=offsetSplit.UnearnedType==0?PrefC.GetLong(PrefName.PrepaymentUnearnedType):offsetSplit.UnearnedType;
			transfer.ListSplitsCur.AddRange(new List<PaySplit> {offsetSplit,unearnedSplit});
			transfer.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated(offsetSplit,unearnedSplit));
			//make account entries for these new items, they may need to be used in the regular transfers down the line.
			AccountEntry offsetEntry=new AccountEntry(offsetSplit);
			offsetEntry.AmountStart=0;
			offsetEntry.AmountEnd=0;
			transfer.ListModifiedProductionEntries.Add(offsetEntry);
			transfer.ListModifiedProductionEntries.Add(new AccountEntry(unearnedSplit));
			return transfer;
		}
		#endregion

		#region Data Classes
		///<summary>The data needed to load FormPayment.</summary>
		[Serializable]
		public class LoadData {
			public Family SuperFam;
			public List<CreditCard> ListCreditCards;
			public XWebResponse XWebResponse;
			public PayConnectResponseWeb PayConnectResponseWeb;
			[XmlIgnore]
			public DataTable TableBalances;
			public List<PaySplit> ListSplits;//current list of splits associated to this payment
			public List<PaySplit> ListPaySplitAllocations;
			public Transaction Transaction;
			public List<PayPlan> ListValidPayPlans;
			public List<Patient> ListAssociatedPatients;
			public List<PaySplit> ListPrePaysForPayment;
			public ConstructChargesData ConstructChargesData;
			public List<Procedure> ListProcsForSplits;
			public SerializableDictionary<long,long> DictPatToDPFeeScheds;
			public List<Fee> ListFeesForDiscountPlans;

			[XmlElement(nameof(TableBalances))]
			public string TableBalancesXml {
				get {
					if(TableBalances==null) {
						return null;
					}
					return XmlConverter.TableToXml(TableBalances);
				}
				set {
					if(value==null) {
						TableBalances=null;
						return;
					}
					TableBalances=XmlConverter.XmlToTable(value);
				}
			}
		}

		///<summary>The data needed to construct a list of charges for FormPayment.</summary>
		[Serializable]
		public class ConstructChargesData {
			///<summary>List from the db, completed for pat. Not list of pre-selected procs from acct. Will also contain TP procs if pref is set to ON.</summary>
			public List<Procedure> ListProcsCompleted=new List<Procedure>();
			public List<Adjustment> ListAdjustments=new List<Adjustment>();
			///<summary>Current list of all splits from database</summary>
			public List<PaySplit> ListPaySplits=new List<PaySplit>();
			///<summary>Stores the summed outstanding ins pay as totals (amounts and write offs) for the list of patnums</summary>
			public List<PayAsTotal> ListInsPayAsTotal=new List<PayAsTotal>();
			public List<PayPlan> ListPayPlans=new List<PayPlan>();
			public List<PaySplit> ListPaySplitsPayPlan=new List<PaySplit>();
			public List<PayPlanCharge> ListPayPlanCharges=new List<PayPlanCharge>();
			///<summary>Stores the list of claimprocs (not ins pay as totals) for the list of pat nums</summary>
			public List<ClaimProc> ListClaimProcsFiltered=new List<ClaimProc>();
			public List<PayPlanLink> ListPayPlanLinks=new List<PayPlanLink>();
		}

		///<summary>Data retrieved upon initialization. AutpSplit stores data retireved from going through list of charges, linking,and autosplitting.</summary>
		[Serializable]
		public class InitData {
			public AutoSplit AutoSplitData;
			public Dictionary<long,Patient> DictPats=new Dictionary<long,Patient>();
			public decimal SplitTotal;
		}

		/// <summary>Data resulting after making a payment.</summary>
		[Serializable]
		public class PayResults {
			public List<PaySplit> ListSplitsCur=new List<PaySplit>();
			public Payment Payment;
			public List<AccountEntry> ListAccountCharges=new List<AccountEntry>();
		}

		/// <summary>Data results after constructing list of charges and linking credits to them.</summary>
		[Serializable]
		public class ConstructResults {
			public List<PaySplit> ListSplitsCur=new List<PaySplit>();
			public Payment Payment;
			public List<AccountEntry> ListAccountCharges=new List<AccountEntry>();
			public List<PayPlanCharge> ListPayPlanCharges=new List<PayPlanCharge>();
		}
		
		/// <summary>Data after autosplitting. ListAutoSplits is separate from ListSplitsCur./// </summary>
		[Serializable]
		public class AutoSplit {
			public List<PaySplit> ListAutoSplits=new List<PaySplit>();
			public Payment Payment;
			public List<AccountEntry> ListAccountCharges=new List<AccountEntry>();
			public List<PaySplit> ListSplitsCur=new List<PaySplit>();
		}

		/// <summary> Data results after getting a just a list of charges.</summary>
		[Serializable]
		public class ConstructListChargesResult {
			public List<AccountEntry> ListAccountEntries=new List<AccountEntry>();
			public List<PayPlanCharge> ListPayPlanCharges=new List<PayPlanCharge>();
		}

		///<summary>Keeps the list of splits that are being transferred, and a bool telling if one or more that was attempted are invalid.</summary>
		[Serializable]
		public class IncomeTransferData {
			public List<PaySplit> ListSplitsCur=new List<PaySplit>();
			public List<PaySplits.PaySplitAssociated> ListSplitsAssociated=new List<PaySplits.PaySplitAssociated>();
			public bool HasInvalidSplits=false;
			public List<AccountEntry> ListModifiedProductionEntries=new List<AccountEntry>();//specifically for when transferring unearned
			public bool HasIvalidProcWithPayPlan=false;
			///<summary>Used when performing logic for family balancer. </summary>
			public string SummaryText="";

			///<summary>Merges given data list into this objects assocaited list.</summary>
			public void MergeIncomeTransferData(IncomeTransferData data) {
				this.ListSplitsAssociated.AddRange(data.ListSplitsAssociated);
				this.ListSplitsCur.AddRange(data.ListSplitsCur);
				this.ListModifiedProductionEntries.AddRange(data.ListModifiedProductionEntries);
				this.HasInvalidSplits|=data.HasInvalidSplits;
				this.HasIvalidProcWithPayPlan|=data.HasIvalidProcWithPayPlan;
				this.SummaryText+=data.SummaryText;
			}

			///<summary>Creates the negative and positive splits from a given parentSplit and a payment's payNum. When isTransferToUnearned is true, these
			///splits will transfer to the user's unearned type based on preference values. If transferAmtOverride is given a value, it will be used for the
			///split amounts in place of the parentSplit.SplitAmt.</summary>
			public static IncomeTransferData CreateTransfer(PaySplit parentSplit,long payNum,bool isTransferToUnearned=false
				,double transferAmtOverride=0) 
			{
				IncomeTransferData transferReturning=new IncomeTransferData();
				PaySplit offsetSplit=new PaySplit();
				offsetSplit.DatePay=DateTime.Today;
				offsetSplit.PatNum=parentSplit.PatNum;
				offsetSplit.PayNum=payNum;
				offsetSplit.ProvNum=parentSplit.ProvNum;
				offsetSplit.ClinicNum=parentSplit.ClinicNum;
				offsetSplit.UnearnedType=parentSplit.UnearnedType;
				offsetSplit.ProcNum=parentSplit.ProcNum;
				offsetSplit.AdjNum=parentSplit.AdjNum;
				offsetSplit.SplitAmt=(transferAmtOverride==0?parentSplit.SplitAmt:transferAmtOverride)*-1;
				offsetSplit.FSplitNum=parentSplit.SplitNum;
				PaySplit allocationSplit=new PaySplit();
				allocationSplit.DatePay=DateTime.Today;
				allocationSplit.PatNum=parentSplit.PatNum;
				allocationSplit.PayNum=payNum;
				allocationSplit.ProvNum=parentSplit.ProvNum;
				allocationSplit.ClinicNum=parentSplit.ClinicNum;
				allocationSplit.ProcNum=parentSplit.ProcNum;
				allocationSplit.AdjNum=parentSplit.AdjNum;
				allocationSplit.SplitAmt=transferAmtOverride==0?parentSplit.SplitAmt:transferAmtOverride;
				allocationSplit.UnearnedType=parentSplit.UnearnedType;
				allocationSplit.FSplitNum=0;//should be offsetSplit's splitNum but has not been inserted into DB yet
				if(isTransferToUnearned) {
					allocationSplit.UnearnedType=PrefC.GetLong(PrefName.PrepaymentUnearnedType);
				}
				transferReturning.ListSplitsCur.AddRange(new List<PaySplit>{offsetSplit,allocationSplit });
				transferReturning.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated(offsetSplit,allocationSplit));
				return transferReturning;
			} 
		}

		#endregion
	}
}
