using System;
using CodeBase;

namespace DataConnectionBase {
	public class QueryMonitor {

		public static void RunMonitoredQuery(Action queryAction,string command) {
			DbQueryObj dbQueryObj=new DbQueryObj(command);
			//Synchronously notify anyone that cares that the query has started to execute.
			QueryMonitorEvent.Fire(ODEventType.QueryMonitor,dbQueryObj);
			dbQueryObj.DateTimeStart=DateTime.Now;
			queryAction();
			dbQueryObj.DateTimeStop=DateTime.Now;
			//Synchronously notify anyone that cares that the query has finished executing.
			QueryMonitorEvent.Fire(ODEventType.QueryMonitor,dbQueryObj);
		}
	}


	public class QueryMonitorEvent {
		public static event ODEventHandler Fired;
		public static void Fire(ODEventType odEventType,object tag) {
			Fired?.Invoke(new ODEventArgs(odEventType,tag));
		}
	}

}
