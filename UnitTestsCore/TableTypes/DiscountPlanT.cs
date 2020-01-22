using System;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class DiscountPlanT {

		///<summary></summary>
		public static DiscountPlan CreateDiscountPlan(string description,long defNum=0,long feeSchedNum=0,bool isHidden=false) {
			DiscountPlan discountPlan=new DiscountPlan() {
				Description=description,
				DefNum=defNum,
				FeeSchedNum=feeSchedNum,
				IsHidden=isHidden,
			};
			DiscountPlans.Insert(discountPlan);
			return discountPlan;
		}

	}
}
