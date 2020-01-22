using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class PatPlanT {
		public static PatPlan CreatePatPlan(byte ordinal,long patNum,long subNum){
			PatPlan patPlan=new PatPlan();
			patPlan.Ordinal=ordinal;
			patPlan.PatNum=patNum;
			patPlan.InsSubNum=subNum;
			PatPlans.Insert(patPlan);
			return patPlan;
		}

		///<summary>Deletes everything from the patplan table.  Does not truncate the table so that PKs are not reused on accident.</summary>
		public static void ClearPatPlanTable() {
			string command="DELETE FROM patplan";
			DataCore.NonQ(command);
		}
	}
}
