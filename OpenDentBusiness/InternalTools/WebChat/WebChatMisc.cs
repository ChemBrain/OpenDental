using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeBase;
using DataConnectionBase;

namespace OpenDentBusiness {
	public class WebChatMisc {
		public delegate void DbActionDelegate();

		///<summary>Creates an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.</summary>
		public static void DbAction(DbActionDelegate actionDelegate,bool isWebChatDb=true) {
			try {
				if(isWebChatDb) {
					DataAction.RunWebChat(() => actionDelegate());
				}
				else {//Customers
					DataAction.RunCustomers(() => actionDelegate(),useConnectionStore:false);
				}
			}
			catch(Exception ex) {
				LogException(ex);
			}
		}

		public static void LogException(Exception ex) {
			Logger.WriteException(ex,"WebChatLogs");
		}

		public static void LogError(string errorMsg) {
			Logger.WriteError(errorMsg,"WebChatLogs");
		}

	}
}