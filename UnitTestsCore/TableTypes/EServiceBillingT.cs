using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class EServiceBillingT {
		public static EServiceBilling GetByRegKeyNum(long regKeyNum) {
			string command="SELECT * FROM eservicebilling WHERE RegistrationKeyNum="+POut.Long(regKeyNum);
			return OpenDentBusiness.Crud.EServiceBillingCrud.SelectOne(command);
		}
	}
}
