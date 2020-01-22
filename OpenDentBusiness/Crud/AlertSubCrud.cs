//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class AlertSubCrud {
		///<summary>Gets one AlertSub object from the database using the primary key.  Returns null if not found.</summary>
		public static AlertSub SelectOne(long alertSubNum) {
			string command="SELECT * FROM alertsub "
				+"WHERE AlertSubNum = "+POut.Long(alertSubNum);
			List<AlertSub> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one AlertSub object from the database using a query.</summary>
		public static AlertSub SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<AlertSub> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of AlertSub objects from the database using a query.</summary>
		public static List<AlertSub> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<AlertSub> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<AlertSub> TableToList(DataTable table) {
			List<AlertSub> retVal=new List<AlertSub>();
			AlertSub alertSub;
			foreach(DataRow row in table.Rows) {
				alertSub=new AlertSub();
				alertSub.AlertSubNum     = PIn.Long  (row["AlertSubNum"].ToString());
				alertSub.UserNum         = PIn.Long  (row["UserNum"].ToString());
				alertSub.ClinicNum       = PIn.Long  (row["ClinicNum"].ToString());
				alertSub.Type            = (OpenDentBusiness.AlertType)PIn.Int(row["Type"].ToString());
				alertSub.AlertCategoryNum= PIn.Long  (row["AlertCategoryNum"].ToString());
				retVal.Add(alertSub);
			}
			return retVal;
		}

		///<summary>Converts a list of AlertSub into a DataTable.</summary>
		public static DataTable ListToTable(List<AlertSub> listAlertSubs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="AlertSub";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("AlertSubNum");
			table.Columns.Add("UserNum");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("Type");
			table.Columns.Add("AlertCategoryNum");
			foreach(AlertSub alertSub in listAlertSubs) {
				table.Rows.Add(new object[] {
					POut.Long  (alertSub.AlertSubNum),
					POut.Long  (alertSub.UserNum),
					POut.Long  (alertSub.ClinicNum),
					POut.Int   ((int)alertSub.Type),
					POut.Long  (alertSub.AlertCategoryNum),
				});
			}
			return table;
		}

		///<summary>Inserts one AlertSub into the database.  Returns the new priKey.</summary>
		public static long Insert(AlertSub alertSub) {
			return Insert(alertSub,false);
		}

		///<summary>Inserts one AlertSub into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(AlertSub alertSub,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				alertSub.AlertSubNum=ReplicationServers.GetKey("alertsub","AlertSubNum");
			}
			string command="INSERT INTO alertsub (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="AlertSubNum,";
			}
			command+="UserNum,ClinicNum,Type,AlertCategoryNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(alertSub.AlertSubNum)+",";
			}
			command+=
				     POut.Long  (alertSub.UserNum)+","
				+    POut.Long  (alertSub.ClinicNum)+","
				+    POut.Int   ((int)alertSub.Type)+","
				+    POut.Long  (alertSub.AlertCategoryNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				alertSub.AlertSubNum=Db.NonQ(command,true,"AlertSubNum","alertSub");
			}
			return alertSub.AlertSubNum;
		}

		///<summary>Inserts one AlertSub into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(AlertSub alertSub) {
			return InsertNoCache(alertSub,false);
		}

		///<summary>Inserts one AlertSub into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(AlertSub alertSub,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO alertsub (";
			if(!useExistingPK && isRandomKeys) {
				alertSub.AlertSubNum=ReplicationServers.GetKeyNoCache("alertsub","AlertSubNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="AlertSubNum,";
			}
			command+="UserNum,ClinicNum,Type,AlertCategoryNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(alertSub.AlertSubNum)+",";
			}
			command+=
				     POut.Long  (alertSub.UserNum)+","
				+    POut.Long  (alertSub.ClinicNum)+","
				+    POut.Int   ((int)alertSub.Type)+","
				+    POut.Long  (alertSub.AlertCategoryNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				alertSub.AlertSubNum=Db.NonQ(command,true,"AlertSubNum","alertSub");
			}
			return alertSub.AlertSubNum;
		}

		///<summary>Updates one AlertSub in the database.</summary>
		public static void Update(AlertSub alertSub) {
			string command="UPDATE alertsub SET "
				+"UserNum         =  "+POut.Long  (alertSub.UserNum)+", "
				+"ClinicNum       =  "+POut.Long  (alertSub.ClinicNum)+", "
				+"Type            =  "+POut.Int   ((int)alertSub.Type)+", "
				+"AlertCategoryNum=  "+POut.Long  (alertSub.AlertCategoryNum)+" "
				+"WHERE AlertSubNum = "+POut.Long(alertSub.AlertSubNum);
			Db.NonQ(command);
		}

		///<summary>Updates one AlertSub in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(AlertSub alertSub,AlertSub oldAlertSub) {
			string command="";
			if(alertSub.UserNum != oldAlertSub.UserNum) {
				if(command!="") { command+=",";}
				command+="UserNum = "+POut.Long(alertSub.UserNum)+"";
			}
			if(alertSub.ClinicNum != oldAlertSub.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(alertSub.ClinicNum)+"";
			}
			if(alertSub.Type != oldAlertSub.Type) {
				if(command!="") { command+=",";}
				command+="Type = "+POut.Int   ((int)alertSub.Type)+"";
			}
			if(alertSub.AlertCategoryNum != oldAlertSub.AlertCategoryNum) {
				if(command!="") { command+=",";}
				command+="AlertCategoryNum = "+POut.Long(alertSub.AlertCategoryNum)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE alertsub SET "+command
				+" WHERE AlertSubNum = "+POut.Long(alertSub.AlertSubNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(AlertSub,AlertSub) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(AlertSub alertSub,AlertSub oldAlertSub) {
			if(alertSub.UserNum != oldAlertSub.UserNum) {
				return true;
			}
			if(alertSub.ClinicNum != oldAlertSub.ClinicNum) {
				return true;
			}
			if(alertSub.Type != oldAlertSub.Type) {
				return true;
			}
			if(alertSub.AlertCategoryNum != oldAlertSub.AlertCategoryNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one AlertSub from the database.</summary>
		public static void Delete(long alertSubNum) {
			string command="DELETE FROM alertsub "
				+"WHERE AlertSubNum = "+POut.Long(alertSubNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<AlertSub> listNew,List<AlertSub> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<AlertSub> listIns    =new List<AlertSub>();
			List<AlertSub> listUpdNew =new List<AlertSub>();
			List<AlertSub> listUpdDB  =new List<AlertSub>();
			List<AlertSub> listDel    =new List<AlertSub>();
			listNew.Sort((AlertSub x,AlertSub y) => { return x.AlertSubNum.CompareTo(y.AlertSubNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((AlertSub x,AlertSub y) => { return x.AlertSubNum.CompareTo(y.AlertSubNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			AlertSub fieldNew;
			AlertSub fieldDB;
			//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.
			//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.
			while(idxNew<listNew.Count || idxDB<listDB.Count) {
				fieldNew=null;
				if(idxNew<listNew.Count) {
					fieldNew=listNew[idxNew];
				}
				fieldDB=null;
				if(idxDB<listDB.Count) {
					fieldDB=listDB[idxDB];
				}
				//begin compare
				if(fieldNew!=null && fieldDB==null) {//listNew has more items, listDB does not.
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew==null && fieldDB!=null) {//listDB has more items, listNew does not.
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				else if(fieldNew.AlertSubNum<fieldDB.AlertSubNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.AlertSubNum>fieldDB.AlertSubNum) {//dbPK less than newPK, dbItem is 'next'
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				//Both lists contain the 'next' item, update required
				listUpdNew.Add(fieldNew);
				listUpdDB.Add(fieldDB);
				idxNew++;
				idxDB++;
			}
			//Commit changes to DB
			for(int i=0;i<listIns.Count;i++) {
				Insert(listIns[i]);
			}
			for(int i=0;i<listUpdNew.Count;i++) {
				if(Update(listUpdNew[i],listUpdDB[i])) {
					rowsUpdatedCount++;
				}
			}
			for(int i=0;i<listDel.Count;i++) {
				Delete(listDel[i].AlertSubNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}