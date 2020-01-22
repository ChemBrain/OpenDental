using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDentBusiness;
using CodeBase;
using System.Globalization;

namespace UnitTestsCore {
	public class PayPlanT {
		public static PayPlan CreatePayPlan(long patNum,double totalAmt,double payAmt,DateTime datePayStart,long provNum) {
			PayPlan payPlan=new PayPlan();
			payPlan.Guarantor=patNum;
			payPlan.PatNum=patNum;
			payPlan.PayAmt=totalAmt;
			payPlan.PayPlanDate=datePayStart;
			payPlan.PayAmt=totalAmt;
			PayPlans.Insert(payPlan);
			PayPlanCharge charge=new PayPlanCharge();
			charge.PayPlanNum=payPlan.PayPlanNum;
			charge.PatNum=patNum;
			charge.ChargeDate=datePayStart;
			charge.Principal=totalAmt;
			charge.ChargeType=PayPlanChargeType.Credit;
			double sumCharges=0;
			int countPayments=0;
			while(sumCharges < totalAmt) { 
				charge=new PayPlanCharge();
				charge.ChargeDate=datePayStart.AddMonths(countPayments);
				charge.PatNum=patNum;
				charge.Guarantor=patNum;
				charge.PayPlanNum=payPlan.PayPlanNum;
				charge.Principal=Math.Min(payAmt,totalAmt-sumCharges);
				charge.ProvNum=provNum;
				sumCharges+=charge.Principal;
				charge.ChargeType=PayPlanChargeType.Debit;
				PayPlanCharges.Insert(charge);
				countPayments++;
			}
			return payPlan;
		}

		public static PayPlan CreatePayPlanNoCharges(long patNum,double totalAmt,DateTime payPlanDate,long guarantorNum) {
			PayPlan payPlan=new PayPlan();
			payPlan.Guarantor=patNum;
			payPlan.PatNum=patNum;
			payPlan.PayAmt=totalAmt;
			payPlan.PayPlanDate=payPlanDate;
			payPlan.PayAmt=totalAmt;
			payPlan.Guarantor=guarantorNum;
			PayPlans.Insert(payPlan);
			return payPlan;
		}

		public static PayPlanCharge CreatePayPlanCharge(DateTime chargeDate,long patNum,long guarantor,long payplanNum,double prinicpal,long provNum
			,long procNum,PayPlanChargeType chargeType) 
		{
			PayPlanCharge ppc=new PayPlanCharge() {
				PayPlanNum=payplanNum,
				PatNum=patNum,
				ChargeDate=chargeDate,
				Principal=prinicpal,
				Guarantor=guarantor,
				ProvNum=provNum,
				ProcNum=procNum,
				ChargeType=chargeType
			};
			PayPlanCharges.Insert(ppc);
			return ppc;
		}

		/// <summary>Creates a payplan and payplan charges with credits. Credit amount generated based off the total amount of the procedures in the list.
		/// If credits are not attached,list of procedures must be null and a total amount must be specified.</summary>
		public static PayPlan CreatePayPlanWithCredits(long patNum,double payAmt,DateTime datePayStart,long provNum=0,List<Procedure> listProcs=null
			,double totalAmt=0,long guarantorNum=0,long clinicNum=0)
		{
			double totalAmount;
			guarantorNum=guarantorNum==0?patNum:guarantorNum;//if it's 0, default to the patNum. 
			if(listProcs!=null) {
				totalAmount=listProcs.Sum(x => x.ProcFee);
			}
			else {
				totalAmount=totalAmt;
			}
			PayPlan payPlan=CreatePayPlanNoCharges(patNum,totalAmount,datePayStart,guarantorNum);//create charges later depending on if attached to procs or not.
			if(listProcs!=null) {
				foreach(Procedure proc in listProcs) {
					PayPlanCharge credit=new PayPlanCharge();
					credit.PayPlanNum=payPlan.PayPlanNum;
					credit.PatNum=patNum;
					credit.ProcNum=proc.ProcNum;
					credit.ProvNum=proc.ProvNum;
					credit.Guarantor=patNum;//credits should always appear on the patient of the payment plan.
					credit.ChargeDate=datePayStart;
					credit.ClinicNum=clinicNum;
					credit.Principal=proc.ProcFee;
					credit.ChargeType=PayPlanChargeType.Credit;
					PayPlanCharges.Insert(credit);//attach the credit for the proc amount. 
				}
			}
			else {//make one credit for the lump sum.
				PayPlanCharge credit=new PayPlanCharge();
				credit.PayPlanNum=payPlan.PayPlanNum;
				credit.PatNum=patNum;
				credit.ChargeDate=datePayStart;
				credit.ProvNum=provNum;
				credit.ClinicNum=clinicNum;
				credit.Guarantor=patNum;//credits should always appear on the patient of the payment plan.
				credit.Principal=totalAmount;
				credit.ChargeType=PayPlanChargeType.Credit;
				PayPlanCharges.Insert(credit);//attach the credit for the total amount.
			}
			//make debit charges for the payment plan
			double sumCharges=0;
			int countPayments=0;
			while(sumCharges < totalAmount) { 
				PayPlanCharge charge=new PayPlanCharge();
				charge.ChargeDate=datePayStart.AddMonths(countPayments);
				charge.PatNum=patNum;
				charge.Guarantor=guarantorNum;
				charge.ClinicNum=clinicNum;
				charge.PayPlanNum=payPlan.PayPlanNum;
				charge.Principal=Math.Min(payAmt,totalAmount-sumCharges);
				charge.ProvNum=provNum;
				sumCharges+=charge.Principal;
				charge.ChargeType=PayPlanChargeType.Debit;
				PayPlanCharges.Insert(charge);
				countPayments++;
			}
			return payPlan;
		}

		///<summary>Total of the adjustments made to the payment plan that have not come due yet. </summary>
		public static double GetTotalNegFutureAdjs(List<PayPlanCharge> listAllCharges) {
				return listAllCharges.FindAll(x => x.ChargeType==PayPlanChargeType.Debit && x.Principal.IsLessThan(0) 
					&& x.ChargeDate > DateTime.Today).Sum(x => x.Principal);
		}

		public static PayPlanCharge CreateNegativeCreditForAdj(long patNum,long payPlanNum,double negAdjAmt) {
			PayPlanCharge txOffset=new PayPlanCharge() {
				ChargeDate=DateTime.Now.Date,
				ChargeType=PayPlanChargeType.Credit,//needs to be saved as a credit to show in Tx Form
				Guarantor=patNum,
				Note="Adjustment",
				PatNum=patNum,
				PayPlanNum=payPlanNum,
				Principal=negAdjAmt,
				ProcNum=0,
			};
			return txOffset;
		}

		#region Dynamic Payment Plans
		public static PayPlan CreateDynamicPaymentPlan(long patNum,long guarantorNum,DateTime date,double downPaymentAmt,int APR,double payAmt,
			List<Procedure> listProcs,List<Adjustment> listAdjustments) 
		{
			PayPlan payPlan=CreatePayPlanNoCharges(patNum,0,date,guarantorNum);
			//create the production links for the payment plan.
			foreach(Procedure proc in listProcs) {
				CreatePaymentPlanLink(payPlan,proc.ProcNum,PayPlanLinkType.Procedure);
			}
			foreach(Adjustment adj in listAdjustments) {
				CreatePaymentPlanLink(payPlan,adj.AdjNum,PayPlanLinkType.Adjustment);
			}
			payPlan.IsDynamic=true;
			payPlan.DatePayPlanStart=date;
			payPlan.DownPayment=downPaymentAmt;
			payPlan.APR=APR;
			payPlan.IsLocked=APR==0?false:true;
			payPlan.Guarantor=guarantorNum;
			payPlan.PayAmt=payAmt;
			PayPlans.Update(payPlan);
			return payPlan;
		}

		///<summary>For use with Dynamic Payment Plans. Production (proceudres and adjustments) is attached via PayPlanLinks.</summary>
		public static PayPlanLink CreatePaymentPlanLink(PayPlan payplan,long procOrAdjNum,PayPlanLinkType linkType) {
			PayPlanLink link=new PayPlanLink();
			link.PayPlanNum=payplan.PayPlanNum;
			link.AmountOverride=0;
			link.FKey=procOrAdjNum;
			link.LinkType=linkType;
			PayPlanLinks.Insert(link);
			return link;
		}


		public static PayPlanTerms GetTerms(PayPlan payplan,List<PayPlanLink> listLinksForPayPlan) {
			PayPlanTerms terms=new PayPlanTerms();
			terms.APR=payplan.APR;
			terms.DateAgreement=payplan.PayPlanDate;
			terms.DateFirstPayment=payplan.DatePayPlanStart;
			terms.Frequency=payplan.ChargeFrequency;
			terms.PaySchedule=payplan.PaySchedule;
			terms.PeriodPayment=(decimal)payplan.PayAmt;
			terms.PrincipalAmount=(double)PayPlanProductionEntry.GetProductionForLinks(listLinksForPayPlan)
				.Sum(x => x.AmountOverride==0?x.AmountOriginal:x.AmountOverride);
			terms.RoundDec=CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits;
			return terms;
		}
		#endregion
	}

}
