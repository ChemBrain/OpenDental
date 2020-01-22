//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class PaymentCrud {
		///<summary>Gets one Payment object from the database using the primary key.  Returns null if not found.</summary>
		public static Payment SelectOne(long payNum) {
			string command="SELECT * FROM payment "
				+"WHERE PayNum = "+POut.Long(payNum);
			List<Payment> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Payment object from the database using a query.</summary>
		public static Payment SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Payment> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Payment objects from the database using a query.</summary>
		public static List<Payment> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Payment> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Payment> TableToList(DataTable table) {
			List<Payment> retVal=new List<Payment>();
			Payment payment;
			foreach(DataRow row in table.Rows) {
				payment=new Payment();
				payment.PayNum             = PIn.Long  (row["PayNum"].ToString());
				payment.PayType            = PIn.Long  (row["PayType"].ToString());
				payment.PayDate            = PIn.Date  (row["PayDate"].ToString());
				payment.PayAmt             = PIn.Double(row["PayAmt"].ToString());
				payment.CheckNum           = PIn.String(row["CheckNum"].ToString());
				payment.BankBranch         = PIn.String(row["BankBranch"].ToString());
				payment.PayNote            = PIn.String(row["PayNote"].ToString());
				payment.IsSplit            = PIn.Bool  (row["IsSplit"].ToString());
				payment.PatNum             = PIn.Long  (row["PatNum"].ToString());
				payment.ClinicNum          = PIn.Long  (row["ClinicNum"].ToString());
				payment.DateEntry          = PIn.Date  (row["DateEntry"].ToString());
				payment.DepositNum         = PIn.Long  (row["DepositNum"].ToString());
				payment.Receipt            = PIn.String(row["Receipt"].ToString());
				payment.IsRecurringCC      = PIn.Bool  (row["IsRecurringCC"].ToString());
				payment.SecUserNumEntry    = PIn.Long  (row["SecUserNumEntry"].ToString());
				payment.SecDateTEdit       = PIn.DateT (row["SecDateTEdit"].ToString());
				payment.PaymentSource      = (OpenDentBusiness.CreditCardSource)PIn.Int(row["PaymentSource"].ToString());
				payment.ProcessStatus      = (OpenDentBusiness.ProcessStat)PIn.Int(row["ProcessStatus"].ToString());
				payment.RecurringChargeDate= PIn.Date  (row["RecurringChargeDate"].ToString());
				payment.ExternalId         = PIn.String(row["ExternalId"].ToString());
				payment.PaymentStatus      = (OpenDentBusiness.PaymentStatus)PIn.Int(row["PaymentStatus"].ToString());
				retVal.Add(payment);
			}
			return retVal;
		}

		///<summary>Converts a list of Payment into a DataTable.</summary>
		public static DataTable ListToTable(List<Payment> listPayments,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Payment";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("PayNum");
			table.Columns.Add("PayType");
			table.Columns.Add("PayDate");
			table.Columns.Add("PayAmt");
			table.Columns.Add("CheckNum");
			table.Columns.Add("BankBranch");
			table.Columns.Add("PayNote");
			table.Columns.Add("IsSplit");
			table.Columns.Add("PatNum");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("DateEntry");
			table.Columns.Add("DepositNum");
			table.Columns.Add("Receipt");
			table.Columns.Add("IsRecurringCC");
			table.Columns.Add("SecUserNumEntry");
			table.Columns.Add("SecDateTEdit");
			table.Columns.Add("PaymentSource");
			table.Columns.Add("ProcessStatus");
			table.Columns.Add("RecurringChargeDate");
			table.Columns.Add("ExternalId");
			table.Columns.Add("PaymentStatus");
			foreach(Payment payment in listPayments) {
				table.Rows.Add(new object[] {
					POut.Long  (payment.PayNum),
					POut.Long  (payment.PayType),
					POut.DateT (payment.PayDate,false),
					POut.Double(payment.PayAmt),
					            payment.CheckNum,
					            payment.BankBranch,
					            payment.PayNote,
					POut.Bool  (payment.IsSplit),
					POut.Long  (payment.PatNum),
					POut.Long  (payment.ClinicNum),
					POut.DateT (payment.DateEntry,false),
					POut.Long  (payment.DepositNum),
					            payment.Receipt,
					POut.Bool  (payment.IsRecurringCC),
					POut.Long  (payment.SecUserNumEntry),
					POut.DateT (payment.SecDateTEdit,false),
					POut.Int   ((int)payment.PaymentSource),
					POut.Int   ((int)payment.ProcessStatus),
					POut.DateT (payment.RecurringChargeDate,false),
					            payment.ExternalId,
					POut.Int   ((int)payment.PaymentStatus),
				});
			}
			return table;
		}

		///<summary>Inserts one Payment into the database.  Returns the new priKey.</summary>
		public static long Insert(Payment payment) {
			return Insert(payment,false);
		}

		///<summary>Inserts one Payment into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Payment payment,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				payment.PayNum=ReplicationServers.GetKey("payment","PayNum");
			}
			string command="INSERT INTO payment (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PayNum,";
			}
			command+="PayType,PayDate,PayAmt,CheckNum,BankBranch,PayNote,IsSplit,PatNum,ClinicNum,DateEntry,DepositNum,Receipt,IsRecurringCC,SecUserNumEntry,PaymentSource,ProcessStatus,RecurringChargeDate,ExternalId,PaymentStatus) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(payment.PayNum)+",";
			}
			command+=
				     POut.Long  (payment.PayType)+","
				+    POut.Date  (payment.PayDate)+","
				+"'"+POut.Double(payment.PayAmt)+"',"
				+"'"+POut.String(payment.CheckNum)+"',"
				+"'"+POut.String(payment.BankBranch)+"',"
				+    DbHelper.ParamChar+"paramPayNote,"
				+    POut.Bool  (payment.IsSplit)+","
				+    POut.Long  (payment.PatNum)+","
				+    POut.Long  (payment.ClinicNum)+","
				+    DbHelper.Now()+","
				+    POut.Long  (payment.DepositNum)+","
				+    DbHelper.ParamChar+"paramReceipt,"
				+    POut.Bool  (payment.IsRecurringCC)+","
				+    POut.Long  (payment.SecUserNumEntry)+","
				//SecDateTEdit can only be set by MySQL
				+    POut.Int   ((int)payment.PaymentSource)+","
				+    POut.Int   ((int)payment.ProcessStatus)+","
				+    POut.Date  (payment.RecurringChargeDate)+","
				+"'"+POut.String(payment.ExternalId)+"',"
				+    POut.Int   ((int)payment.PaymentStatus)+")";
			if(payment.PayNote==null) {
				payment.PayNote="";
			}
			OdSqlParameter paramPayNote=new OdSqlParameter("paramPayNote",OdDbType.Text,POut.StringNote(payment.PayNote));
			if(payment.Receipt==null) {
				payment.Receipt="";
			}
			OdSqlParameter paramReceipt=new OdSqlParameter("paramReceipt",OdDbType.Text,POut.StringParam(payment.Receipt));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramPayNote,paramReceipt);
			}
			else {
				payment.PayNum=Db.NonQ(command,true,"PayNum","payment",paramPayNote,paramReceipt);
			}
			return payment.PayNum;
		}

		///<summary>Inserts one Payment into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Payment payment) {
			return InsertNoCache(payment,false);
		}

		///<summary>Inserts one Payment into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Payment payment,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO payment (";
			if(!useExistingPK && isRandomKeys) {
				payment.PayNum=ReplicationServers.GetKeyNoCache("payment","PayNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="PayNum,";
			}
			command+="PayType,PayDate,PayAmt,CheckNum,BankBranch,PayNote,IsSplit,PatNum,ClinicNum,DateEntry,DepositNum,Receipt,IsRecurringCC,SecUserNumEntry,PaymentSource,ProcessStatus,RecurringChargeDate,ExternalId,PaymentStatus) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(payment.PayNum)+",";
			}
			command+=
				     POut.Long  (payment.PayType)+","
				+    POut.Date  (payment.PayDate)+","
				+"'"+POut.Double(payment.PayAmt)+"',"
				+"'"+POut.String(payment.CheckNum)+"',"
				+"'"+POut.String(payment.BankBranch)+"',"
				+    DbHelper.ParamChar+"paramPayNote,"
				+    POut.Bool  (payment.IsSplit)+","
				+    POut.Long  (payment.PatNum)+","
				+    POut.Long  (payment.ClinicNum)+","
				+    DbHelper.Now()+","
				+    POut.Long  (payment.DepositNum)+","
				+    DbHelper.ParamChar+"paramReceipt,"
				+    POut.Bool  (payment.IsRecurringCC)+","
				+    POut.Long  (payment.SecUserNumEntry)+","
				//SecDateTEdit can only be set by MySQL
				+    POut.Int   ((int)payment.PaymentSource)+","
				+    POut.Int   ((int)payment.ProcessStatus)+","
				+    POut.Date  (payment.RecurringChargeDate)+","
				+"'"+POut.String(payment.ExternalId)+"',"
				+    POut.Int   ((int)payment.PaymentStatus)+")";
			if(payment.PayNote==null) {
				payment.PayNote="";
			}
			OdSqlParameter paramPayNote=new OdSqlParameter("paramPayNote",OdDbType.Text,POut.StringNote(payment.PayNote));
			if(payment.Receipt==null) {
				payment.Receipt="";
			}
			OdSqlParameter paramReceipt=new OdSqlParameter("paramReceipt",OdDbType.Text,POut.StringParam(payment.Receipt));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramPayNote,paramReceipt);
			}
			else {
				payment.PayNum=Db.NonQ(command,true,"PayNum","payment",paramPayNote,paramReceipt);
			}
			return payment.PayNum;
		}

		///<summary>Updates one Payment in the database.</summary>
		public static void Update(Payment payment) {
			string command="UPDATE payment SET "
				+"PayType            =  "+POut.Long  (payment.PayType)+", "
				+"PayDate            =  "+POut.Date  (payment.PayDate)+", "
				+"PayAmt             = '"+POut.Double(payment.PayAmt)+"', "
				+"CheckNum           = '"+POut.String(payment.CheckNum)+"', "
				+"BankBranch         = '"+POut.String(payment.BankBranch)+"', "
				+"PayNote            =  "+DbHelper.ParamChar+"paramPayNote, "
				+"IsSplit            =  "+POut.Bool  (payment.IsSplit)+", "
				+"PatNum             =  "+POut.Long  (payment.PatNum)+", "
				+"ClinicNum          =  "+POut.Long  (payment.ClinicNum)+", "
				//DateEntry not allowed to change
				//DepositNum excluded from update
				+"Receipt            =  "+DbHelper.ParamChar+"paramReceipt, "
				+"IsRecurringCC      =  "+POut.Bool  (payment.IsRecurringCC)+", "
				//SecUserNumEntry excluded from update
				//SecDateTEdit can only be set by MySQL
				+"PaymentSource      =  "+POut.Int   ((int)payment.PaymentSource)+", "
				+"ProcessStatus      =  "+POut.Int   ((int)payment.ProcessStatus)+", "
				+"RecurringChargeDate=  "+POut.Date  (payment.RecurringChargeDate)+", "
				+"ExternalId         = '"+POut.String(payment.ExternalId)+"', "
				+"PaymentStatus      =  "+POut.Int   ((int)payment.PaymentStatus)+" "
				+"WHERE PayNum = "+POut.Long(payment.PayNum);
			if(payment.PayNote==null) {
				payment.PayNote="";
			}
			OdSqlParameter paramPayNote=new OdSqlParameter("paramPayNote",OdDbType.Text,POut.StringNote(payment.PayNote));
			if(payment.Receipt==null) {
				payment.Receipt="";
			}
			OdSqlParameter paramReceipt=new OdSqlParameter("paramReceipt",OdDbType.Text,POut.StringParam(payment.Receipt));
			Db.NonQ(command,paramPayNote,paramReceipt);
		}

		///<summary>Updates one Payment in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Payment payment,Payment oldPayment) {
			string command="";
			if(payment.PayType != oldPayment.PayType) {
				if(command!="") { command+=",";}
				command+="PayType = "+POut.Long(payment.PayType)+"";
			}
			if(payment.PayDate.Date != oldPayment.PayDate.Date) {
				if(command!="") { command+=",";}
				command+="PayDate = "+POut.Date(payment.PayDate)+"";
			}
			if(payment.PayAmt != oldPayment.PayAmt) {
				if(command!="") { command+=",";}
				command+="PayAmt = '"+POut.Double(payment.PayAmt)+"'";
			}
			if(payment.CheckNum != oldPayment.CheckNum) {
				if(command!="") { command+=",";}
				command+="CheckNum = '"+POut.String(payment.CheckNum)+"'";
			}
			if(payment.BankBranch != oldPayment.BankBranch) {
				if(command!="") { command+=",";}
				command+="BankBranch = '"+POut.String(payment.BankBranch)+"'";
			}
			if(payment.PayNote != oldPayment.PayNote) {
				if(command!="") { command+=",";}
				command+="PayNote = "+DbHelper.ParamChar+"paramPayNote";
			}
			if(payment.IsSplit != oldPayment.IsSplit) {
				if(command!="") { command+=",";}
				command+="IsSplit = "+POut.Bool(payment.IsSplit)+"";
			}
			if(payment.PatNum != oldPayment.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(payment.PatNum)+"";
			}
			if(payment.ClinicNum != oldPayment.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(payment.ClinicNum)+"";
			}
			//DateEntry not allowed to change
			//DepositNum excluded from update
			if(payment.Receipt != oldPayment.Receipt) {
				if(command!="") { command+=",";}
				command+="Receipt = "+DbHelper.ParamChar+"paramReceipt";
			}
			if(payment.IsRecurringCC != oldPayment.IsRecurringCC) {
				if(command!="") { command+=",";}
				command+="IsRecurringCC = "+POut.Bool(payment.IsRecurringCC)+"";
			}
			//SecUserNumEntry excluded from update
			//SecDateTEdit can only be set by MySQL
			if(payment.PaymentSource != oldPayment.PaymentSource) {
				if(command!="") { command+=",";}
				command+="PaymentSource = "+POut.Int   ((int)payment.PaymentSource)+"";
			}
			if(payment.ProcessStatus != oldPayment.ProcessStatus) {
				if(command!="") { command+=",";}
				command+="ProcessStatus = "+POut.Int   ((int)payment.ProcessStatus)+"";
			}
			if(payment.RecurringChargeDate.Date != oldPayment.RecurringChargeDate.Date) {
				if(command!="") { command+=",";}
				command+="RecurringChargeDate = "+POut.Date(payment.RecurringChargeDate)+"";
			}
			if(payment.ExternalId != oldPayment.ExternalId) {
				if(command!="") { command+=",";}
				command+="ExternalId = '"+POut.String(payment.ExternalId)+"'";
			}
			if(payment.PaymentStatus != oldPayment.PaymentStatus) {
				if(command!="") { command+=",";}
				command+="PaymentStatus = "+POut.Int   ((int)payment.PaymentStatus)+"";
			}
			if(command=="") {
				return false;
			}
			if(payment.PayNote==null) {
				payment.PayNote="";
			}
			OdSqlParameter paramPayNote=new OdSqlParameter("paramPayNote",OdDbType.Text,POut.StringNote(payment.PayNote));
			if(payment.Receipt==null) {
				payment.Receipt="";
			}
			OdSqlParameter paramReceipt=new OdSqlParameter("paramReceipt",OdDbType.Text,POut.StringParam(payment.Receipt));
			command="UPDATE payment SET "+command
				+" WHERE PayNum = "+POut.Long(payment.PayNum);
			Db.NonQ(command,paramPayNote,paramReceipt);
			return true;
		}

		///<summary>Returns true if Update(Payment,Payment) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Payment payment,Payment oldPayment) {
			if(payment.PayType != oldPayment.PayType) {
				return true;
			}
			if(payment.PayDate.Date != oldPayment.PayDate.Date) {
				return true;
			}
			if(payment.PayAmt != oldPayment.PayAmt) {
				return true;
			}
			if(payment.CheckNum != oldPayment.CheckNum) {
				return true;
			}
			if(payment.BankBranch != oldPayment.BankBranch) {
				return true;
			}
			if(payment.PayNote != oldPayment.PayNote) {
				return true;
			}
			if(payment.IsSplit != oldPayment.IsSplit) {
				return true;
			}
			if(payment.PatNum != oldPayment.PatNum) {
				return true;
			}
			if(payment.ClinicNum != oldPayment.ClinicNum) {
				return true;
			}
			//DateEntry not allowed to change
			//DepositNum excluded from update
			if(payment.Receipt != oldPayment.Receipt) {
				return true;
			}
			if(payment.IsRecurringCC != oldPayment.IsRecurringCC) {
				return true;
			}
			//SecUserNumEntry excluded from update
			//SecDateTEdit can only be set by MySQL
			if(payment.PaymentSource != oldPayment.PaymentSource) {
				return true;
			}
			if(payment.ProcessStatus != oldPayment.ProcessStatus) {
				return true;
			}
			if(payment.RecurringChargeDate.Date != oldPayment.RecurringChargeDate.Date) {
				return true;
			}
			if(payment.ExternalId != oldPayment.ExternalId) {
				return true;
			}
			if(payment.PaymentStatus != oldPayment.PaymentStatus) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Payment from the database.</summary>
		public static void Delete(long payNum) {
			string command="DELETE FROM payment "
				+"WHERE PayNum = "+POut.Long(payNum);
			Db.NonQ(command);
		}

	}
}