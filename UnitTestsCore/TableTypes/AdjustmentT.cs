using OpenDentBusiness;
using System;

namespace UnitTestsCore {
	public class AdjustmentT {
		public static Adjustment MakeAdjustment(long patNum,double adjAmt,DateTime adjDate=default(DateTime),DateTime procDate=default(DateTime)
			,long procNum=0,long provNum=0,long adjType=0) 
		{
			Adjustment adjustment=new Adjustment();
			if(adjDate==default(DateTime)) {
				adjDate=DateTime.Today;
			}
			if(procDate==default(DateTime)) {
				procDate=DateTime.Today;
			}
			adjustment.PatNum=patNum;
			adjustment.AdjAmt=adjAmt;
			adjustment.ProcNum=procNum;
			adjustment.ProvNum=provNum;
			adjustment.AdjDate=adjDate;
			adjustment.ProcDate=procDate;
			adjustment.AdjType=adjType;
			Adjustments.Insert(adjustment);
			return adjustment;
		}

	}
}
