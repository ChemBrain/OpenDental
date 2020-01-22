using System;
using System.Collections.Generic;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class CommOptOutT {

		public static void Create(Patient pat,CommOptOutType optOutType,CommOptOutMode optOutMode) {
			CommOptOuts.InsertMany(new List<CommOptOut> {
				new CommOptOut { PatNum=pat.PatNum,CommType=optOutType,CommMode=optOutMode }
			});
		}
	}
}
