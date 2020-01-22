using OpenDentBusiness;
using System;

namespace UnitTestsCore {
	public class PayPlanChargeT {

		public static PayPlanCharge CreateOne(long payPlanNum,long guarantor,long patNum,DateTime chargeDate,double principal,double interest,
			string note="",long provNum=0,long clinicNum=0,PayPlanChargeType chargeType=PayPlanChargeType.Debit,long procNum=0)
		{
			PayPlanCharge charge=new PayPlanCharge();
			charge.PayPlanNum=payPlanNum;
			charge.Guarantor=guarantor;
			charge.PatNum=patNum;
			charge.ChargeDate=chargeDate;
			charge.Principal=principal;
			charge.Interest=interest;
			charge.Note=note;
			charge.ProvNum=provNum;
			charge.ClinicNum=clinicNum;
			charge.ChargeType=chargeType;
			charge.ProcNum=procNum;
			PayPlanCharges.Insert(charge);
			return charge;
		}
		
	}	
}
