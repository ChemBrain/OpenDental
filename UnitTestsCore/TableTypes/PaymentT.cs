using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBase;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class PaymentT {
		public static Payment MakePayment(long patNum,double payAmt,DateTime payDate,long payPlanNum=0,long provNum=0,long procNum=0,long payType=0,
			long clinicNum=0,long unearnedType=0,bool isRecurringCharge=false,DateTime recurringChargeDate=default(DateTime),string externalId=""
			,long adjNum=0) 
		{
			Payment payment=new Payment();
			payment.PatNum=patNum;
			payment.PayDate=payDate;
			payment.PayAmt=payAmt;
			payment.PayType=payType;
			payment.ClinicNum=clinicNum;
			payment.DateEntry=payDate;
			payment.IsRecurringCC=isRecurringCharge;
			payment.RecurringChargeDate=recurringChargeDate;
			payment.ExternalId=externalId;
			Payments.Insert(payment);
			PaySplit split=new PaySplit();
			split.PayNum=payment.PayNum;
			split.PatNum=payment.PatNum;
			split.DatePay=payDate;
			split.ClinicNum=payment.ClinicNum;
			split.PayPlanNum=payPlanNum;
			split.AdjNum=adjNum;
			split.ProvNum=provNum;
			split.ProcNum=procNum;
			split.SplitAmt=payAmt;
			split.DateEntry=payDate;
			split.UnearnedType=unearnedType;
			PaySplits.Insert(split);
			return payment;
		}

		///<summary>Use this to test auto-split or income transfer logic.  This makes no splits, just the payment shell.</summary>
		public static Payment MakePaymentNoSplits(long patNum,double payAmt,DateTime payDate=default(DateTime),bool isNew=false,long payType=0
			,long clinicNum=0) 
		{//payType defaulted to non-income transfer
			if(payDate==default(DateTime)) {
				payDate=DateTime.Today;
			}
			Payment payment=new Payment();
			payment.PatNum=patNum;
			payment.PayDate=payDate;
			payment.PayAmt=payAmt;
			payment.IsNew=isNew;
			payment.ClinicNum=clinicNum;
			payment.PayType=payType;
			Payments.Insert(payment);
			return payment;
		}

		public static Payment MakePaymentForPrepayment(Patient pat,Clinic clinic) {
			Payment paymentCur=new Payment();
			paymentCur.PayDate=DateTime.Today;
			paymentCur.PatNum=pat.PatNum;
			paymentCur.ClinicNum=clinic.ClinicNum;
			paymentCur.DateEntry=DateTime.Today;
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
			if(listDefs.Count>0) {
				paymentCur.PayType=listDefs[0].DefNum;
			}
			paymentCur.PaymentSource=CreditCardSource.None;
			paymentCur.ProcessStatus=ProcessStat.OfficeProcessed;
			paymentCur.PayAmt=0;
			Payments.Insert(paymentCur);
			return paymentCur;
		}

		public static PaymentEdit.IncomeTransferData IncomeTransfer(long patNum,Family fam,Payment payCur,List<PayPlanCharge> listPayPlanCredits,
			bool doIncludeHidden=false) 
		{
			#region generate charges and credits for account
			//go through the logic that constructs the charges for the income transfer manager
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(fam.ListPats.Select(x => x.PatNum).ToList(),
				patNum,new List<PaySplit>(),payCur,new List<AccountEntry>(),true,false,doShowHiddenSplits:doIncludeHidden);
			PaymentEdit.IncomeTransferData transfers=new PaymentEdit.IncomeTransferData();
			List<AccountEntry> listPosCharges=results.ListAccountCharges.FindAll(x => x.AmountEnd > 0).OrderBy(x => x.Date).ToList();
			List<AccountEntry> listNegCharges=results.ListAccountCharges.FindAll(x => x.AmountEnd < 0).OrderBy(x => x.Date).ToList();
			List<long> listPatsWithPosCharges=listPosCharges.Select(y => y.PatNum).Distinct().ToList();
			List<AccountEntry> listAccountEntries=results.ListAccountCharges.FindAll(x => x.PatNum.In(listPatsWithPosCharges));
			#endregion
			//begin transfer loops
			#region transfer within payplans first
			PaymentEdit.IncomeTransferData payPlanResults=PaymentEdit.CreatePayplanLoop(listPosCharges,listNegCharges,listAccountEntries
				,payCur.PayNum,listPayPlanCredits,DateTimeOD.Today);
			transfers.MergeIncomeTransferData(payPlanResults);
			#endregion
			#region regular transfers
			PaymentEdit.IncomeTransferData txfrResults=PaymentEdit.CreateTransferLoop(listPosCharges,listNegCharges,listAccountEntries
				,payCur.PayNum,listPayPlanCredits,DateTimeOD.Today);
			transfers.MergeIncomeTransferData(txfrResults);
			#endregion
			return transfers;
		}

		
		///<summary>Transfers unallocated to unearned (if present) and inserts those results into the database. Then performs transfer.
		///This is the best representation of what the income transfer window currently does.</summary>
		public static PaymentEdit.IncomeTransferData BalanceAndIncomeTransfer(long patNum,Family fam,Payment regularTransferPayment
			,List<PayPlanCharge> payPlanCharges=null) 
		{
			//get all paysplits associated to the family passed in
			List<PaySplit> listSplitsForPat=PaySplits.GetForPats(fam.ListPats.Select(x => x.PatNum).ToList());
			//perform unallocated transfer
			Payment unallocatedTransfer=MakePaymentNoSplits(patNum,0,payDate:DateTime.Today);
			PaymentEdit.IncomeTransferData unallocatedResults=PaymentEdit.TransferUnallocatedSplitToUnearned(listSplitsForPat,unallocatedTransfer.PayNum);
			foreach(PaySplit split in unallocatedResults.ListSplitsCur) {
				if(split.SplitAmt.IsZero()) {
					continue;
				}
				PaySplits.Insert(split);
			}
			foreach(PaySplits.PaySplitAssociated splitAssociated in unallocatedResults.ListSplitsAssociated) {
				if(splitAssociated.PaySplitOrig!=null && splitAssociated.PaySplitLinked!=null) {
					PaySplits.UpdateFSplitNum(splitAssociated.PaySplitOrig.SplitNum,splitAssociated.PaySplitLinked.SplitNum);
				}
			}
			if(payPlanCharges==null) {
				payPlanCharges=new List<PayPlanCharge>();
			}
			#region claim fix and transfer
			//both of these methods have objects that get immediately inserted into the database. While testing, a spcific call wil need to be made to delete.
			ClaimProcs.FixClaimsNoProcedures(fam.ListPats.Select(x => x.PatNum).ToList());//make dummy procedures and claimprocs for claims missing procs.
			ClaimProcs.TransferClaimsAsTotalToProcedures(fam.ListPats.Select(x => x.PatNum).ToList());//transfer AsTotals into claim procedures
			#endregion
			return IncomeTransfer(patNum,fam,regularTransferPayment,payPlanCharges);
		}
	}
}
