using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CodeBase {
	public class ODInitialize {

		///<summary>Indicates that the program is running unit tests.  Should only be set to true from TestBase.Initialize().
		///Useful for methods that should behave differently in unit tests, such as FriendlyException.Show().</summary>
		public static bool IsRunningInUnitTest;
		///<summary>Indicates that Initialize has been invoked at least once and has successfully executed.</summary>
		public static bool HasInitialized;

		///<summary>This method is called from all Open Dental programs or projects. This method can throw. There is a good
		///chance you should not let the user continue if the method throws as it can cause the program to behave in unexpecting ways.</summary>
		public static void Initialize() {
			//Causes all Application threads to use the same short date format.  Namely, forces computers with a two digit year format (e.g. M/d/yy) to use
			//a four digit format inside OpenDental (bug manifested as DateTime.MinValue being pulled into OD as 01/01/2001), as well as two digit month 
			//and year formatting, which is essential for threads that use date strings in validation.
			DateTimeOD.NormalizeApplicationShortDateFormat();
			//The default SecurityProtocol is "Ssl3|Tls".  We must add Tls12 in order to support Tls1.2 web reference handshakes, 
			//without breaking any web references using Ssl3 or Tls. This is necessary for XWeb payments.
			ServicePointManager.SecurityProtocol|=SecurityProtocolType.Tls12;
			HasInitialized=true;
		}
	}
}
