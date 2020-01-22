//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EhrNotPerformedCrud {
		///<summary>Gets one EhrNotPerformed object from the database using the primary key.  Returns null if not found.</summary>
		public static EhrNotPerformed SelectOne(long ehrNotPerformedNum) {
			string command="SELECT * FROM ehrnotperformed "
				+"WHERE EhrNotPerformedNum = "+POut.Long(ehrNotPerformedNum);
			List<EhrNotPerformed> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EhrNotPerformed object from the database using a query.</summary>
		public static EhrNotPerformed SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrNotPerformed> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EhrNotPerformed objects from the database using a query.</summary>
		public static List<EhrNotPerformed> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrNotPerformed> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EhrNotPerformed> TableToList(DataTable table) {
			List<EhrNotPerformed> retVal=new List<EhrNotPerformed>();
			EhrNotPerformed ehrNotPerformed;
			foreach(DataRow row in table.Rows) {
				ehrNotPerformed=new EhrNotPerformed();
				ehrNotPerformed.EhrNotPerformedNum= PIn.Long  (row["EhrNotPerformedNum"].ToString());
				ehrNotPerformed.PatNum            = PIn.Long  (row["PatNum"].ToString());
				ehrNotPerformed.ProvNum           = PIn.Long  (row["ProvNum"].ToString());
				ehrNotPerformed.CodeValue         = PIn.String(row["CodeValue"].ToString());
				ehrNotPerformed.CodeSystem        = PIn.String(row["CodeSystem"].ToString());
				ehrNotPerformed.CodeValueReason   = PIn.String(row["CodeValueReason"].ToString());
				ehrNotPerformed.CodeSystemReason  = PIn.String(row["CodeSystemReason"].ToString());
				ehrNotPerformed.Note              = PIn.String(row["Note"].ToString());
				ehrNotPerformed.DateEntry         = PIn.Date  (row["DateEntry"].ToString());
				retVal.Add(ehrNotPerformed);
			}
			return retVal;
		}

		///<summary>Converts a list of EhrNotPerformed into a DataTable.</summary>
		public static DataTable ListToTable(List<EhrNotPerformed> listEhrNotPerformeds,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EhrNotPerformed";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EhrNotPerformedNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("ProvNum");
			table.Columns.Add("CodeValue");
			table.Columns.Add("CodeSystem");
			table.Columns.Add("CodeValueReason");
			table.Columns.Add("CodeSystemReason");
			table.Columns.Add("Note");
			table.Columns.Add("DateEntry");
			foreach(EhrNotPerformed ehrNotPerformed in listEhrNotPerformeds) {
				table.Rows.Add(new object[] {
					POut.Long  (ehrNotPerformed.EhrNotPerformedNum),
					POut.Long  (ehrNotPerformed.PatNum),
					POut.Long  (ehrNotPerformed.ProvNum),
					            ehrNotPerformed.CodeValue,
					            ehrNotPerformed.CodeSystem,
					            ehrNotPerformed.CodeValueReason,
					            ehrNotPerformed.CodeSystemReason,
					            ehrNotPerformed.Note,
					POut.DateT (ehrNotPerformed.DateEntry,false),
				});
			}
			return table;
		}

		///<summary>Inserts one EhrNotPerformed into the database.  Returns the new priKey.</summary>
		public static long Insert(EhrNotPerformed ehrNotPerformed) {
			return Insert(ehrNotPerformed,false);
		}

		///<summary>Inserts one EhrNotPerformed into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EhrNotPerformed ehrNotPerformed,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				ehrNotPerformed.EhrNotPerformedNum=ReplicationServers.GetKey("ehrnotperformed","EhrNotPerformedNum");
			}
			string command="INSERT INTO ehrnotperformed (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EhrNotPerformedNum,";
			}
			command+="PatNum,ProvNum,CodeValue,CodeSystem,CodeValueReason,CodeSystemReason,Note,DateEntry) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(ehrNotPerformed.EhrNotPerformedNum)+",";
			}
			command+=
				     POut.Long  (ehrNotPerformed.PatNum)+","
				+    POut.Long  (ehrNotPerformed.ProvNum)+","
				+"'"+POut.String(ehrNotPerformed.CodeValue)+"',"
				+"'"+POut.String(ehrNotPerformed.CodeSystem)+"',"
				+"'"+POut.String(ehrNotPerformed.CodeValueReason)+"',"
				+"'"+POut.String(ehrNotPerformed.CodeSystemReason)+"',"
				+    DbHelper.ParamChar+"paramNote,"
				+    POut.Date  (ehrNotPerformed.DateEntry)+")";
			if(ehrNotPerformed.Note==null) {
				ehrNotPerformed.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(ehrNotPerformed.Note));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				ehrNotPerformed.EhrNotPerformedNum=Db.NonQ(command,true,"EhrNotPerformedNum","ehrNotPerformed",paramNote);
			}
			return ehrNotPerformed.EhrNotPerformedNum;
		}

		///<summary>Inserts one EhrNotPerformed into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrNotPerformed ehrNotPerformed) {
			return InsertNoCache(ehrNotPerformed,false);
		}

		///<summary>Inserts one EhrNotPerformed into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrNotPerformed ehrNotPerformed,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO ehrnotperformed (";
			if(!useExistingPK && isRandomKeys) {
				ehrNotPerformed.EhrNotPerformedNum=ReplicationServers.GetKeyNoCache("ehrnotperformed","EhrNotPerformedNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EhrNotPerformedNum,";
			}
			command+="PatNum,ProvNum,CodeValue,CodeSystem,CodeValueReason,CodeSystemReason,Note,DateEntry) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(ehrNotPerformed.EhrNotPerformedNum)+",";
			}
			command+=
				     POut.Long  (ehrNotPerformed.PatNum)+","
				+    POut.Long  (ehrNotPerformed.ProvNum)+","
				+"'"+POut.String(ehrNotPerformed.CodeValue)+"',"
				+"'"+POut.String(ehrNotPerformed.CodeSystem)+"',"
				+"'"+POut.String(ehrNotPerformed.CodeValueReason)+"',"
				+"'"+POut.String(ehrNotPerformed.CodeSystemReason)+"',"
				+    DbHelper.ParamChar+"paramNote,"
				+    POut.Date  (ehrNotPerformed.DateEntry)+")";
			if(ehrNotPerformed.Note==null) {
				ehrNotPerformed.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(ehrNotPerformed.Note));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				ehrNotPerformed.EhrNotPerformedNum=Db.NonQ(command,true,"EhrNotPerformedNum","ehrNotPerformed",paramNote);
			}
			return ehrNotPerformed.EhrNotPerformedNum;
		}

		///<summary>Updates one EhrNotPerformed in the database.</summary>
		public static void Update(EhrNotPerformed ehrNotPerformed) {
			string command="UPDATE ehrnotperformed SET "
				+"PatNum            =  "+POut.Long  (ehrNotPerformed.PatNum)+", "
				+"ProvNum           =  "+POut.Long  (ehrNotPerformed.ProvNum)+", "
				+"CodeValue         = '"+POut.String(ehrNotPerformed.CodeValue)+"', "
				+"CodeSystem        = '"+POut.String(ehrNotPerformed.CodeSystem)+"', "
				+"CodeValueReason   = '"+POut.String(ehrNotPerformed.CodeValueReason)+"', "
				+"CodeSystemReason  = '"+POut.String(ehrNotPerformed.CodeSystemReason)+"', "
				+"Note              =  "+DbHelper.ParamChar+"paramNote, "
				+"DateEntry         =  "+POut.Date  (ehrNotPerformed.DateEntry)+" "
				+"WHERE EhrNotPerformedNum = "+POut.Long(ehrNotPerformed.EhrNotPerformedNum);
			if(ehrNotPerformed.Note==null) {
				ehrNotPerformed.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(ehrNotPerformed.Note));
			Db.NonQ(command,paramNote);
		}

		///<summary>Updates one EhrNotPerformed in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EhrNotPerformed ehrNotPerformed,EhrNotPerformed oldEhrNotPerformed) {
			string command="";
			if(ehrNotPerformed.PatNum != oldEhrNotPerformed.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(ehrNotPerformed.PatNum)+"";
			}
			if(ehrNotPerformed.ProvNum != oldEhrNotPerformed.ProvNum) {
				if(command!="") { command+=",";}
				command+="ProvNum = "+POut.Long(ehrNotPerformed.ProvNum)+"";
			}
			if(ehrNotPerformed.CodeValue != oldEhrNotPerformed.CodeValue) {
				if(command!="") { command+=",";}
				command+="CodeValue = '"+POut.String(ehrNotPerformed.CodeValue)+"'";
			}
			if(ehrNotPerformed.CodeSystem != oldEhrNotPerformed.CodeSystem) {
				if(command!="") { command+=",";}
				command+="CodeSystem = '"+POut.String(ehrNotPerformed.CodeSystem)+"'";
			}
			if(ehrNotPerformed.CodeValueReason != oldEhrNotPerformed.CodeValueReason) {
				if(command!="") { command+=",";}
				command+="CodeValueReason = '"+POut.String(ehrNotPerformed.CodeValueReason)+"'";
			}
			if(ehrNotPerformed.CodeSystemReason != oldEhrNotPerformed.CodeSystemReason) {
				if(command!="") { command+=",";}
				command+="CodeSystemReason = '"+POut.String(ehrNotPerformed.CodeSystemReason)+"'";
			}
			if(ehrNotPerformed.Note != oldEhrNotPerformed.Note) {
				if(command!="") { command+=",";}
				command+="Note = "+DbHelper.ParamChar+"paramNote";
			}
			if(ehrNotPerformed.DateEntry.Date != oldEhrNotPerformed.DateEntry.Date) {
				if(command!="") { command+=",";}
				command+="DateEntry = "+POut.Date(ehrNotPerformed.DateEntry)+"";
			}
			if(command=="") {
				return false;
			}
			if(ehrNotPerformed.Note==null) {
				ehrNotPerformed.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(ehrNotPerformed.Note));
			command="UPDATE ehrnotperformed SET "+command
				+" WHERE EhrNotPerformedNum = "+POut.Long(ehrNotPerformed.EhrNotPerformedNum);
			Db.NonQ(command,paramNote);
			return true;
		}

		///<summary>Returns true if Update(EhrNotPerformed,EhrNotPerformed) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EhrNotPerformed ehrNotPerformed,EhrNotPerformed oldEhrNotPerformed) {
			if(ehrNotPerformed.PatNum != oldEhrNotPerformed.PatNum) {
				return true;
			}
			if(ehrNotPerformed.ProvNum != oldEhrNotPerformed.ProvNum) {
				return true;
			}
			if(ehrNotPerformed.CodeValue != oldEhrNotPerformed.CodeValue) {
				return true;
			}
			if(ehrNotPerformed.CodeSystem != oldEhrNotPerformed.CodeSystem) {
				return true;
			}
			if(ehrNotPerformed.CodeValueReason != oldEhrNotPerformed.CodeValueReason) {
				return true;
			}
			if(ehrNotPerformed.CodeSystemReason != oldEhrNotPerformed.CodeSystemReason) {
				return true;
			}
			if(ehrNotPerformed.Note != oldEhrNotPerformed.Note) {
				return true;
			}
			if(ehrNotPerformed.DateEntry.Date != oldEhrNotPerformed.DateEntry.Date) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EhrNotPerformed from the database.</summary>
		public static void Delete(long ehrNotPerformedNum) {
			string command="DELETE FROM ehrnotperformed "
				+"WHERE EhrNotPerformedNum = "+POut.Long(ehrNotPerformedNum);
			Db.NonQ(command);
		}

	}
}