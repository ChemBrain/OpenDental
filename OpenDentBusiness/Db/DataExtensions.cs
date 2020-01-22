using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness {
	public static class DataExtensions {

		///<summary>Simpler way to get a long from a DataRow.</summary>
		public static long GetLong(this DataRow row,string columnName) {
			return PIn.Long(row[columnName].ToString());
		}

		///<summary>Simpler way to get a string from a DataRow.</summary>
		public static string GetString(this DataRow row,string columnName) {
			return PIn.String(row[columnName].ToString());
		}

		///<summary>Simpler way to get a Date (without time) from a DataRow.</summary>
		public static DateTime GetDate(this DataRow row,string columnName) {
			return PIn.Date(row[columnName].ToString());
		}

		///<summary>Simpler way to get a DateTime from a DataRow.</summary>
		public static DateTime GetDateT(this DataRow row,string columnName) {
			return PIn.DateT(row[columnName].ToString());
		}
	}
}
