using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class RepeatChargeT {

		public static RepeatCharge GetRepeatChargeCustomers(eServiceCode eService,long patNum)
		{
			return DataAction.GetCustomers(() => {
				var all=RepeatCharges.Refresh(patNum).ToList();
				var link=EServiceCodeLink.GetAll().FirstOrDefault(x => x.EService==eService)??new EServiceCodeLink();
				return all.FirstOrDefault(x => x.ProcCode==link.ProcCode);
			});
			
		}
	}
}
