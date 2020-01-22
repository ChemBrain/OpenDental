//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ApptReminderRuleCrud {
		///<summary>Gets one ApptReminderRule object from the database using the primary key.  Returns null if not found.</summary>
		public static ApptReminderRule SelectOne(long apptReminderRuleNum) {
			string command="SELECT * FROM apptreminderrule "
				+"WHERE ApptReminderRuleNum = "+POut.Long(apptReminderRuleNum);
			List<ApptReminderRule> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ApptReminderRule object from the database using a query.</summary>
		public static ApptReminderRule SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ApptReminderRule> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ApptReminderRule objects from the database using a query.</summary>
		public static List<ApptReminderRule> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ApptReminderRule> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ApptReminderRule> TableToList(DataTable table) {
			List<ApptReminderRule> retVal=new List<ApptReminderRule>();
			ApptReminderRule apptReminderRule;
			foreach(DataRow row in table.Rows) {
				apptReminderRule=new ApptReminderRule();
				apptReminderRule.ApptReminderRuleNum       = PIn.Long  (row["ApptReminderRuleNum"].ToString());
				apptReminderRule.TypeCur                   = (OpenDentBusiness.ApptReminderType)PIn.Int(row["TypeCur"].ToString());
				apptReminderRule.TSPrior                   = TimeSpan.FromTicks(PIn.Long(row["TSPrior"].ToString()));
				apptReminderRule.SendOrder                 = PIn.String(row["SendOrder"].ToString());
				apptReminderRule.IsSendAll                 = PIn.Bool  (row["IsSendAll"].ToString());
				apptReminderRule.TemplateSMS               = PIn.String(row["TemplateSMS"].ToString());
				apptReminderRule.TemplateEmailSubject      = PIn.String(row["TemplateEmailSubject"].ToString());
				apptReminderRule.TemplateEmail             = PIn.String(row["TemplateEmail"].ToString());
				apptReminderRule.ClinicNum                 = PIn.Long  (row["ClinicNum"].ToString());
				apptReminderRule.TemplateSMSAggShared      = PIn.String(row["TemplateSMSAggShared"].ToString());
				apptReminderRule.TemplateSMSAggPerAppt     = PIn.String(row["TemplateSMSAggPerAppt"].ToString());
				apptReminderRule.TemplateEmailSubjAggShared= PIn.String(row["TemplateEmailSubjAggShared"].ToString());
				apptReminderRule.TemplateEmailAggShared    = PIn.String(row["TemplateEmailAggShared"].ToString());
				apptReminderRule.TemplateEmailAggPerAppt   = PIn.String(row["TemplateEmailAggPerAppt"].ToString());
				apptReminderRule.DoNotSendWithin           = TimeSpan.FromTicks(PIn.Long(row["DoNotSendWithin"].ToString()));
				apptReminderRule.IsEnabled                 = PIn.Bool  (row["IsEnabled"].ToString());
				apptReminderRule.TemplateAutoReply         = PIn.String(row["TemplateAutoReply"].ToString());
				apptReminderRule.TemplateAutoReplyAgg      = PIn.String(row["TemplateAutoReplyAgg"].ToString());
				apptReminderRule.IsAutoReplyEnabled        = PIn.Bool  (row["IsAutoReplyEnabled"].ToString());
				retVal.Add(apptReminderRule);
			}
			return retVal;
		}

		///<summary>Converts a list of ApptReminderRule into a DataTable.</summary>
		public static DataTable ListToTable(List<ApptReminderRule> listApptReminderRules,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="ApptReminderRule";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ApptReminderRuleNum");
			table.Columns.Add("TypeCur");
			table.Columns.Add("TSPrior");
			table.Columns.Add("SendOrder");
			table.Columns.Add("IsSendAll");
			table.Columns.Add("TemplateSMS");
			table.Columns.Add("TemplateEmailSubject");
			table.Columns.Add("TemplateEmail");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("TemplateSMSAggShared");
			table.Columns.Add("TemplateSMSAggPerAppt");
			table.Columns.Add("TemplateEmailSubjAggShared");
			table.Columns.Add("TemplateEmailAggShared");
			table.Columns.Add("TemplateEmailAggPerAppt");
			table.Columns.Add("DoNotSendWithin");
			table.Columns.Add("IsEnabled");
			table.Columns.Add("TemplateAutoReply");
			table.Columns.Add("TemplateAutoReplyAgg");
			table.Columns.Add("IsAutoReplyEnabled");
			foreach(ApptReminderRule apptReminderRule in listApptReminderRules) {
				table.Rows.Add(new object[] {
					POut.Long  (apptReminderRule.ApptReminderRuleNum),
					POut.Int   ((int)apptReminderRule.TypeCur),
					POut.Long (apptReminderRule.TSPrior.Ticks),
					            apptReminderRule.SendOrder,
					POut.Bool  (apptReminderRule.IsSendAll),
					            apptReminderRule.TemplateSMS,
					            apptReminderRule.TemplateEmailSubject,
					            apptReminderRule.TemplateEmail,
					POut.Long  (apptReminderRule.ClinicNum),
					            apptReminderRule.TemplateSMSAggShared,
					            apptReminderRule.TemplateSMSAggPerAppt,
					            apptReminderRule.TemplateEmailSubjAggShared,
					            apptReminderRule.TemplateEmailAggShared,
					            apptReminderRule.TemplateEmailAggPerAppt,
					POut.Long (apptReminderRule.DoNotSendWithin.Ticks),
					POut.Bool  (apptReminderRule.IsEnabled),
					            apptReminderRule.TemplateAutoReply,
					            apptReminderRule.TemplateAutoReplyAgg,
					POut.Bool  (apptReminderRule.IsAutoReplyEnabled),
				});
			}
			return table;
		}

		///<summary>Inserts one ApptReminderRule into the database.  Returns the new priKey.</summary>
		public static long Insert(ApptReminderRule apptReminderRule) {
			return Insert(apptReminderRule,false);
		}

		///<summary>Inserts one ApptReminderRule into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ApptReminderRule apptReminderRule,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				apptReminderRule.ApptReminderRuleNum=ReplicationServers.GetKey("apptreminderrule","ApptReminderRuleNum");
			}
			string command="INSERT INTO apptreminderrule (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ApptReminderRuleNum,";
			}
			command+="TypeCur,TSPrior,SendOrder,IsSendAll,TemplateSMS,TemplateEmailSubject,TemplateEmail,ClinicNum,TemplateSMSAggShared,TemplateSMSAggPerAppt,TemplateEmailSubjAggShared,TemplateEmailAggShared,TemplateEmailAggPerAppt,DoNotSendWithin,IsEnabled,TemplateAutoReply,TemplateAutoReplyAgg,IsAutoReplyEnabled) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(apptReminderRule.ApptReminderRuleNum)+",";
			}
			command+=
				     POut.Int   ((int)apptReminderRule.TypeCur)+","
				+"'"+POut.Long  (apptReminderRule.TSPrior.Ticks)+"',"
				+"'"+POut.String(apptReminderRule.SendOrder)+"',"
				+    POut.Bool  (apptReminderRule.IsSendAll)+","
				+    DbHelper.ParamChar+"paramTemplateSMS,"
				+    DbHelper.ParamChar+"paramTemplateEmailSubject,"
				+    DbHelper.ParamChar+"paramTemplateEmail,"
				+    POut.Long  (apptReminderRule.ClinicNum)+","
				+    DbHelper.ParamChar+"paramTemplateSMSAggShared,"
				+    DbHelper.ParamChar+"paramTemplateSMSAggPerAppt,"
				+    DbHelper.ParamChar+"paramTemplateEmailSubjAggShared,"
				+    DbHelper.ParamChar+"paramTemplateEmailAggShared,"
				+    DbHelper.ParamChar+"paramTemplateEmailAggPerAppt,"
				+"'"+POut.Long  (apptReminderRule.DoNotSendWithin.Ticks)+"',"
				+    POut.Bool  (apptReminderRule.IsEnabled)+","
				+    DbHelper.ParamChar+"paramTemplateAutoReply,"
				+    DbHelper.ParamChar+"paramTemplateAutoReplyAgg,"
				+    POut.Bool  (apptReminderRule.IsAutoReplyEnabled)+")";
			if(apptReminderRule.TemplateSMS==null) {
				apptReminderRule.TemplateSMS="";
			}
			OdSqlParameter paramTemplateSMS=new OdSqlParameter("paramTemplateSMS",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateSMS));
			if(apptReminderRule.TemplateEmailSubject==null) {
				apptReminderRule.TemplateEmailSubject="";
			}
			OdSqlParameter paramTemplateEmailSubject=new OdSqlParameter("paramTemplateEmailSubject",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailSubject));
			if(apptReminderRule.TemplateEmail==null) {
				apptReminderRule.TemplateEmail="";
			}
			OdSqlParameter paramTemplateEmail=new OdSqlParameter("paramTemplateEmail",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmail));
			if(apptReminderRule.TemplateSMSAggShared==null) {
				apptReminderRule.TemplateSMSAggShared="";
			}
			OdSqlParameter paramTemplateSMSAggShared=new OdSqlParameter("paramTemplateSMSAggShared",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateSMSAggShared));
			if(apptReminderRule.TemplateSMSAggPerAppt==null) {
				apptReminderRule.TemplateSMSAggPerAppt="";
			}
			OdSqlParameter paramTemplateSMSAggPerAppt=new OdSqlParameter("paramTemplateSMSAggPerAppt",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateSMSAggPerAppt));
			if(apptReminderRule.TemplateEmailSubjAggShared==null) {
				apptReminderRule.TemplateEmailSubjAggShared="";
			}
			OdSqlParameter paramTemplateEmailSubjAggShared=new OdSqlParameter("paramTemplateEmailSubjAggShared",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailSubjAggShared));
			if(apptReminderRule.TemplateEmailAggShared==null) {
				apptReminderRule.TemplateEmailAggShared="";
			}
			OdSqlParameter paramTemplateEmailAggShared=new OdSqlParameter("paramTemplateEmailAggShared",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailAggShared));
			if(apptReminderRule.TemplateEmailAggPerAppt==null) {
				apptReminderRule.TemplateEmailAggPerAppt="";
			}
			OdSqlParameter paramTemplateEmailAggPerAppt=new OdSqlParameter("paramTemplateEmailAggPerAppt",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailAggPerAppt));
			if(apptReminderRule.TemplateAutoReply==null) {
				apptReminderRule.TemplateAutoReply="";
			}
			OdSqlParameter paramTemplateAutoReply=new OdSqlParameter("paramTemplateAutoReply",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateAutoReply));
			if(apptReminderRule.TemplateAutoReplyAgg==null) {
				apptReminderRule.TemplateAutoReplyAgg="";
			}
			OdSqlParameter paramTemplateAutoReplyAgg=new OdSqlParameter("paramTemplateAutoReplyAgg",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateAutoReplyAgg));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramTemplateSMS,paramTemplateEmailSubject,paramTemplateEmail,paramTemplateSMSAggShared,paramTemplateSMSAggPerAppt,paramTemplateEmailSubjAggShared,paramTemplateEmailAggShared,paramTemplateEmailAggPerAppt,paramTemplateAutoReply,paramTemplateAutoReplyAgg);
			}
			else {
				apptReminderRule.ApptReminderRuleNum=Db.NonQ(command,true,"ApptReminderRuleNum","apptReminderRule",paramTemplateSMS,paramTemplateEmailSubject,paramTemplateEmail,paramTemplateSMSAggShared,paramTemplateSMSAggPerAppt,paramTemplateEmailSubjAggShared,paramTemplateEmailAggShared,paramTemplateEmailAggPerAppt,paramTemplateAutoReply,paramTemplateAutoReplyAgg);
			}
			return apptReminderRule.ApptReminderRuleNum;
		}

		///<summary>Inserts one ApptReminderRule into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ApptReminderRule apptReminderRule) {
			return InsertNoCache(apptReminderRule,false);
		}

		///<summary>Inserts one ApptReminderRule into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ApptReminderRule apptReminderRule,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO apptreminderrule (";
			if(!useExistingPK && isRandomKeys) {
				apptReminderRule.ApptReminderRuleNum=ReplicationServers.GetKeyNoCache("apptreminderrule","ApptReminderRuleNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ApptReminderRuleNum,";
			}
			command+="TypeCur,TSPrior,SendOrder,IsSendAll,TemplateSMS,TemplateEmailSubject,TemplateEmail,ClinicNum,TemplateSMSAggShared,TemplateSMSAggPerAppt,TemplateEmailSubjAggShared,TemplateEmailAggShared,TemplateEmailAggPerAppt,DoNotSendWithin,IsEnabled,TemplateAutoReply,TemplateAutoReplyAgg,IsAutoReplyEnabled) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(apptReminderRule.ApptReminderRuleNum)+",";
			}
			command+=
				     POut.Int   ((int)apptReminderRule.TypeCur)+","
				+"'"+POut.Long(apptReminderRule.TSPrior.Ticks)+"',"
				+"'"+POut.String(apptReminderRule.SendOrder)+"',"
				+    POut.Bool  (apptReminderRule.IsSendAll)+","
				+    DbHelper.ParamChar+"paramTemplateSMS,"
				+    DbHelper.ParamChar+"paramTemplateEmailSubject,"
				+    DbHelper.ParamChar+"paramTemplateEmail,"
				+    POut.Long  (apptReminderRule.ClinicNum)+","
				+    DbHelper.ParamChar+"paramTemplateSMSAggShared,"
				+    DbHelper.ParamChar+"paramTemplateSMSAggPerAppt,"
				+    DbHelper.ParamChar+"paramTemplateEmailSubjAggShared,"
				+    DbHelper.ParamChar+"paramTemplateEmailAggShared,"
				+    DbHelper.ParamChar+"paramTemplateEmailAggPerAppt,"
				+"'"+POut.Long(apptReminderRule.DoNotSendWithin.Ticks)+"',"
				+    POut.Bool  (apptReminderRule.IsEnabled)+","
				+    DbHelper.ParamChar+"paramTemplateAutoReply,"
				+    DbHelper.ParamChar+"paramTemplateAutoReplyAgg,"
				+    POut.Bool  (apptReminderRule.IsAutoReplyEnabled)+")";
			if(apptReminderRule.TemplateSMS==null) {
				apptReminderRule.TemplateSMS="";
			}
			OdSqlParameter paramTemplateSMS=new OdSqlParameter("paramTemplateSMS",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateSMS));
			if(apptReminderRule.TemplateEmailSubject==null) {
				apptReminderRule.TemplateEmailSubject="";
			}
			OdSqlParameter paramTemplateEmailSubject=new OdSqlParameter("paramTemplateEmailSubject",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailSubject));
			if(apptReminderRule.TemplateEmail==null) {
				apptReminderRule.TemplateEmail="";
			}
			OdSqlParameter paramTemplateEmail=new OdSqlParameter("paramTemplateEmail",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmail));
			if(apptReminderRule.TemplateSMSAggShared==null) {
				apptReminderRule.TemplateSMSAggShared="";
			}
			OdSqlParameter paramTemplateSMSAggShared=new OdSqlParameter("paramTemplateSMSAggShared",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateSMSAggShared));
			if(apptReminderRule.TemplateSMSAggPerAppt==null) {
				apptReminderRule.TemplateSMSAggPerAppt="";
			}
			OdSqlParameter paramTemplateSMSAggPerAppt=new OdSqlParameter("paramTemplateSMSAggPerAppt",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateSMSAggPerAppt));
			if(apptReminderRule.TemplateEmailSubjAggShared==null) {
				apptReminderRule.TemplateEmailSubjAggShared="";
			}
			OdSqlParameter paramTemplateEmailSubjAggShared=new OdSqlParameter("paramTemplateEmailSubjAggShared",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailSubjAggShared));
			if(apptReminderRule.TemplateEmailAggShared==null) {
				apptReminderRule.TemplateEmailAggShared="";
			}
			OdSqlParameter paramTemplateEmailAggShared=new OdSqlParameter("paramTemplateEmailAggShared",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailAggShared));
			if(apptReminderRule.TemplateEmailAggPerAppt==null) {
				apptReminderRule.TemplateEmailAggPerAppt="";
			}
			OdSqlParameter paramTemplateEmailAggPerAppt=new OdSqlParameter("paramTemplateEmailAggPerAppt",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailAggPerAppt));
			if(apptReminderRule.TemplateAutoReply==null) {
				apptReminderRule.TemplateAutoReply="";
			}
			OdSqlParameter paramTemplateAutoReply=new OdSqlParameter("paramTemplateAutoReply",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateAutoReply));
			if(apptReminderRule.TemplateAutoReplyAgg==null) {
				apptReminderRule.TemplateAutoReplyAgg="";
			}
			OdSqlParameter paramTemplateAutoReplyAgg=new OdSqlParameter("paramTemplateAutoReplyAgg",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateAutoReplyAgg));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramTemplateSMS,paramTemplateEmailSubject,paramTemplateEmail,paramTemplateSMSAggShared,paramTemplateSMSAggPerAppt,paramTemplateEmailSubjAggShared,paramTemplateEmailAggShared,paramTemplateEmailAggPerAppt,paramTemplateAutoReply,paramTemplateAutoReplyAgg);
			}
			else {
				apptReminderRule.ApptReminderRuleNum=Db.NonQ(command,true,"ApptReminderRuleNum","apptReminderRule",paramTemplateSMS,paramTemplateEmailSubject,paramTemplateEmail,paramTemplateSMSAggShared,paramTemplateSMSAggPerAppt,paramTemplateEmailSubjAggShared,paramTemplateEmailAggShared,paramTemplateEmailAggPerAppt,paramTemplateAutoReply,paramTemplateAutoReplyAgg);
			}
			return apptReminderRule.ApptReminderRuleNum;
		}

		///<summary>Updates one ApptReminderRule in the database.</summary>
		public static void Update(ApptReminderRule apptReminderRule) {
			string command="UPDATE apptreminderrule SET "
				+"TypeCur                   =  "+POut.Int   ((int)apptReminderRule.TypeCur)+", "
				+"TSPrior                   =  "+POut.Long  (apptReminderRule.TSPrior.Ticks)+", "
				+"SendOrder                 = '"+POut.String(apptReminderRule.SendOrder)+"', "
				+"IsSendAll                 =  "+POut.Bool  (apptReminderRule.IsSendAll)+", "
				+"TemplateSMS               =  "+DbHelper.ParamChar+"paramTemplateSMS, "
				+"TemplateEmailSubject      =  "+DbHelper.ParamChar+"paramTemplateEmailSubject, "
				+"TemplateEmail             =  "+DbHelper.ParamChar+"paramTemplateEmail, "
				+"ClinicNum                 =  "+POut.Long  (apptReminderRule.ClinicNum)+", "
				+"TemplateSMSAggShared      =  "+DbHelper.ParamChar+"paramTemplateSMSAggShared, "
				+"TemplateSMSAggPerAppt     =  "+DbHelper.ParamChar+"paramTemplateSMSAggPerAppt, "
				+"TemplateEmailSubjAggShared=  "+DbHelper.ParamChar+"paramTemplateEmailSubjAggShared, "
				+"TemplateEmailAggShared    =  "+DbHelper.ParamChar+"paramTemplateEmailAggShared, "
				+"TemplateEmailAggPerAppt   =  "+DbHelper.ParamChar+"paramTemplateEmailAggPerAppt, "
				+"DoNotSendWithin           =  "+POut.Long  (apptReminderRule.DoNotSendWithin.Ticks)+", "
				+"IsEnabled                 =  "+POut.Bool  (apptReminderRule.IsEnabled)+", "
				+"TemplateAutoReply         =  "+DbHelper.ParamChar+"paramTemplateAutoReply, "
				+"TemplateAutoReplyAgg      =  "+DbHelper.ParamChar+"paramTemplateAutoReplyAgg, "
				+"IsAutoReplyEnabled        =  "+POut.Bool  (apptReminderRule.IsAutoReplyEnabled)+" "
				+"WHERE ApptReminderRuleNum = "+POut.Long(apptReminderRule.ApptReminderRuleNum);
			if(apptReminderRule.TemplateSMS==null) {
				apptReminderRule.TemplateSMS="";
			}
			OdSqlParameter paramTemplateSMS=new OdSqlParameter("paramTemplateSMS",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateSMS));
			if(apptReminderRule.TemplateEmailSubject==null) {
				apptReminderRule.TemplateEmailSubject="";
			}
			OdSqlParameter paramTemplateEmailSubject=new OdSqlParameter("paramTemplateEmailSubject",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailSubject));
			if(apptReminderRule.TemplateEmail==null) {
				apptReminderRule.TemplateEmail="";
			}
			OdSqlParameter paramTemplateEmail=new OdSqlParameter("paramTemplateEmail",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmail));
			if(apptReminderRule.TemplateSMSAggShared==null) {
				apptReminderRule.TemplateSMSAggShared="";
			}
			OdSqlParameter paramTemplateSMSAggShared=new OdSqlParameter("paramTemplateSMSAggShared",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateSMSAggShared));
			if(apptReminderRule.TemplateSMSAggPerAppt==null) {
				apptReminderRule.TemplateSMSAggPerAppt="";
			}
			OdSqlParameter paramTemplateSMSAggPerAppt=new OdSqlParameter("paramTemplateSMSAggPerAppt",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateSMSAggPerAppt));
			if(apptReminderRule.TemplateEmailSubjAggShared==null) {
				apptReminderRule.TemplateEmailSubjAggShared="";
			}
			OdSqlParameter paramTemplateEmailSubjAggShared=new OdSqlParameter("paramTemplateEmailSubjAggShared",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailSubjAggShared));
			if(apptReminderRule.TemplateEmailAggShared==null) {
				apptReminderRule.TemplateEmailAggShared="";
			}
			OdSqlParameter paramTemplateEmailAggShared=new OdSqlParameter("paramTemplateEmailAggShared",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailAggShared));
			if(apptReminderRule.TemplateEmailAggPerAppt==null) {
				apptReminderRule.TemplateEmailAggPerAppt="";
			}
			OdSqlParameter paramTemplateEmailAggPerAppt=new OdSqlParameter("paramTemplateEmailAggPerAppt",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailAggPerAppt));
			if(apptReminderRule.TemplateAutoReply==null) {
				apptReminderRule.TemplateAutoReply="";
			}
			OdSqlParameter paramTemplateAutoReply=new OdSqlParameter("paramTemplateAutoReply",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateAutoReply));
			if(apptReminderRule.TemplateAutoReplyAgg==null) {
				apptReminderRule.TemplateAutoReplyAgg="";
			}
			OdSqlParameter paramTemplateAutoReplyAgg=new OdSqlParameter("paramTemplateAutoReplyAgg",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateAutoReplyAgg));
			Db.NonQ(command,paramTemplateSMS,paramTemplateEmailSubject,paramTemplateEmail,paramTemplateSMSAggShared,paramTemplateSMSAggPerAppt,paramTemplateEmailSubjAggShared,paramTemplateEmailAggShared,paramTemplateEmailAggPerAppt,paramTemplateAutoReply,paramTemplateAutoReplyAgg);
		}

		///<summary>Updates one ApptReminderRule in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ApptReminderRule apptReminderRule,ApptReminderRule oldApptReminderRule) {
			string command="";
			if(apptReminderRule.TypeCur != oldApptReminderRule.TypeCur) {
				if(command!="") { command+=",";}
				command+="TypeCur = "+POut.Int   ((int)apptReminderRule.TypeCur)+"";
			}
			if(apptReminderRule.TSPrior != oldApptReminderRule.TSPrior) {
				if(command!="") { command+=",";}
				command+="TSPrior = '"+POut.Long  (apptReminderRule.TSPrior.Ticks)+"'";
			}
			if(apptReminderRule.SendOrder != oldApptReminderRule.SendOrder) {
				if(command!="") { command+=",";}
				command+="SendOrder = '"+POut.String(apptReminderRule.SendOrder)+"'";
			}
			if(apptReminderRule.IsSendAll != oldApptReminderRule.IsSendAll) {
				if(command!="") { command+=",";}
				command+="IsSendAll = "+POut.Bool(apptReminderRule.IsSendAll)+"";
			}
			if(apptReminderRule.TemplateSMS != oldApptReminderRule.TemplateSMS) {
				if(command!="") { command+=",";}
				command+="TemplateSMS = "+DbHelper.ParamChar+"paramTemplateSMS";
			}
			if(apptReminderRule.TemplateEmailSubject != oldApptReminderRule.TemplateEmailSubject) {
				if(command!="") { command+=",";}
				command+="TemplateEmailSubject = "+DbHelper.ParamChar+"paramTemplateEmailSubject";
			}
			if(apptReminderRule.TemplateEmail != oldApptReminderRule.TemplateEmail) {
				if(command!="") { command+=",";}
				command+="TemplateEmail = "+DbHelper.ParamChar+"paramTemplateEmail";
			}
			if(apptReminderRule.ClinicNum != oldApptReminderRule.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(apptReminderRule.ClinicNum)+"";
			}
			if(apptReminderRule.TemplateSMSAggShared != oldApptReminderRule.TemplateSMSAggShared) {
				if(command!="") { command+=",";}
				command+="TemplateSMSAggShared = "+DbHelper.ParamChar+"paramTemplateSMSAggShared";
			}
			if(apptReminderRule.TemplateSMSAggPerAppt != oldApptReminderRule.TemplateSMSAggPerAppt) {
				if(command!="") { command+=",";}
				command+="TemplateSMSAggPerAppt = "+DbHelper.ParamChar+"paramTemplateSMSAggPerAppt";
			}
			if(apptReminderRule.TemplateEmailSubjAggShared != oldApptReminderRule.TemplateEmailSubjAggShared) {
				if(command!="") { command+=",";}
				command+="TemplateEmailSubjAggShared = "+DbHelper.ParamChar+"paramTemplateEmailSubjAggShared";
			}
			if(apptReminderRule.TemplateEmailAggShared != oldApptReminderRule.TemplateEmailAggShared) {
				if(command!="") { command+=",";}
				command+="TemplateEmailAggShared = "+DbHelper.ParamChar+"paramTemplateEmailAggShared";
			}
			if(apptReminderRule.TemplateEmailAggPerAppt != oldApptReminderRule.TemplateEmailAggPerAppt) {
				if(command!="") { command+=",";}
				command+="TemplateEmailAggPerAppt = "+DbHelper.ParamChar+"paramTemplateEmailAggPerAppt";
			}
			if(apptReminderRule.DoNotSendWithin != oldApptReminderRule.DoNotSendWithin) {
				if(command!="") { command+=",";}
				command+="DoNotSendWithin = '"+POut.Long  (apptReminderRule.DoNotSendWithin.Ticks)+"'";
			}
			if(apptReminderRule.IsEnabled != oldApptReminderRule.IsEnabled) {
				if(command!="") { command+=",";}
				command+="IsEnabled = "+POut.Bool(apptReminderRule.IsEnabled)+"";
			}
			if(apptReminderRule.TemplateAutoReply != oldApptReminderRule.TemplateAutoReply) {
				if(command!="") { command+=",";}
				command+="TemplateAutoReply = "+DbHelper.ParamChar+"paramTemplateAutoReply";
			}
			if(apptReminderRule.TemplateAutoReplyAgg != oldApptReminderRule.TemplateAutoReplyAgg) {
				if(command!="") { command+=",";}
				command+="TemplateAutoReplyAgg = "+DbHelper.ParamChar+"paramTemplateAutoReplyAgg";
			}
			if(apptReminderRule.IsAutoReplyEnabled != oldApptReminderRule.IsAutoReplyEnabled) {
				if(command!="") { command+=",";}
				command+="IsAutoReplyEnabled = "+POut.Bool(apptReminderRule.IsAutoReplyEnabled)+"";
			}
			if(command=="") {
				return false;
			}
			if(apptReminderRule.TemplateSMS==null) {
				apptReminderRule.TemplateSMS="";
			}
			OdSqlParameter paramTemplateSMS=new OdSqlParameter("paramTemplateSMS",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateSMS));
			if(apptReminderRule.TemplateEmailSubject==null) {
				apptReminderRule.TemplateEmailSubject="";
			}
			OdSqlParameter paramTemplateEmailSubject=new OdSqlParameter("paramTemplateEmailSubject",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailSubject));
			if(apptReminderRule.TemplateEmail==null) {
				apptReminderRule.TemplateEmail="";
			}
			OdSqlParameter paramTemplateEmail=new OdSqlParameter("paramTemplateEmail",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmail));
			if(apptReminderRule.TemplateSMSAggShared==null) {
				apptReminderRule.TemplateSMSAggShared="";
			}
			OdSqlParameter paramTemplateSMSAggShared=new OdSqlParameter("paramTemplateSMSAggShared",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateSMSAggShared));
			if(apptReminderRule.TemplateSMSAggPerAppt==null) {
				apptReminderRule.TemplateSMSAggPerAppt="";
			}
			OdSqlParameter paramTemplateSMSAggPerAppt=new OdSqlParameter("paramTemplateSMSAggPerAppt",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateSMSAggPerAppt));
			if(apptReminderRule.TemplateEmailSubjAggShared==null) {
				apptReminderRule.TemplateEmailSubjAggShared="";
			}
			OdSqlParameter paramTemplateEmailSubjAggShared=new OdSqlParameter("paramTemplateEmailSubjAggShared",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailSubjAggShared));
			if(apptReminderRule.TemplateEmailAggShared==null) {
				apptReminderRule.TemplateEmailAggShared="";
			}
			OdSqlParameter paramTemplateEmailAggShared=new OdSqlParameter("paramTemplateEmailAggShared",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailAggShared));
			if(apptReminderRule.TemplateEmailAggPerAppt==null) {
				apptReminderRule.TemplateEmailAggPerAppt="";
			}
			OdSqlParameter paramTemplateEmailAggPerAppt=new OdSqlParameter("paramTemplateEmailAggPerAppt",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateEmailAggPerAppt));
			if(apptReminderRule.TemplateAutoReply==null) {
				apptReminderRule.TemplateAutoReply="";
			}
			OdSqlParameter paramTemplateAutoReply=new OdSqlParameter("paramTemplateAutoReply",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateAutoReply));
			if(apptReminderRule.TemplateAutoReplyAgg==null) {
				apptReminderRule.TemplateAutoReplyAgg="";
			}
			OdSqlParameter paramTemplateAutoReplyAgg=new OdSqlParameter("paramTemplateAutoReplyAgg",OdDbType.Text,POut.StringParam(apptReminderRule.TemplateAutoReplyAgg));
			command="UPDATE apptreminderrule SET "+command
				+" WHERE ApptReminderRuleNum = "+POut.Long(apptReminderRule.ApptReminderRuleNum);
			Db.NonQ(command,paramTemplateSMS,paramTemplateEmailSubject,paramTemplateEmail,paramTemplateSMSAggShared,paramTemplateSMSAggPerAppt,paramTemplateEmailSubjAggShared,paramTemplateEmailAggShared,paramTemplateEmailAggPerAppt,paramTemplateAutoReply,paramTemplateAutoReplyAgg);
			return true;
		}

		///<summary>Returns true if Update(ApptReminderRule,ApptReminderRule) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ApptReminderRule apptReminderRule,ApptReminderRule oldApptReminderRule) {
			if(apptReminderRule.TypeCur != oldApptReminderRule.TypeCur) {
				return true;
			}
			if(apptReminderRule.TSPrior != oldApptReminderRule.TSPrior) {
				return true;
			}
			if(apptReminderRule.SendOrder != oldApptReminderRule.SendOrder) {
				return true;
			}
			if(apptReminderRule.IsSendAll != oldApptReminderRule.IsSendAll) {
				return true;
			}
			if(apptReminderRule.TemplateSMS != oldApptReminderRule.TemplateSMS) {
				return true;
			}
			if(apptReminderRule.TemplateEmailSubject != oldApptReminderRule.TemplateEmailSubject) {
				return true;
			}
			if(apptReminderRule.TemplateEmail != oldApptReminderRule.TemplateEmail) {
				return true;
			}
			if(apptReminderRule.ClinicNum != oldApptReminderRule.ClinicNum) {
				return true;
			}
			if(apptReminderRule.TemplateSMSAggShared != oldApptReminderRule.TemplateSMSAggShared) {
				return true;
			}
			if(apptReminderRule.TemplateSMSAggPerAppt != oldApptReminderRule.TemplateSMSAggPerAppt) {
				return true;
			}
			if(apptReminderRule.TemplateEmailSubjAggShared != oldApptReminderRule.TemplateEmailSubjAggShared) {
				return true;
			}
			if(apptReminderRule.TemplateEmailAggShared != oldApptReminderRule.TemplateEmailAggShared) {
				return true;
			}
			if(apptReminderRule.TemplateEmailAggPerAppt != oldApptReminderRule.TemplateEmailAggPerAppt) {
				return true;
			}
			if(apptReminderRule.DoNotSendWithin != oldApptReminderRule.DoNotSendWithin) {
				return true;
			}
			if(apptReminderRule.IsEnabled != oldApptReminderRule.IsEnabled) {
				return true;
			}
			if(apptReminderRule.TemplateAutoReply != oldApptReminderRule.TemplateAutoReply) {
				return true;
			}
			if(apptReminderRule.TemplateAutoReplyAgg != oldApptReminderRule.TemplateAutoReplyAgg) {
				return true;
			}
			if(apptReminderRule.IsAutoReplyEnabled != oldApptReminderRule.IsAutoReplyEnabled) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ApptReminderRule from the database.</summary>
		public static void Delete(long apptReminderRuleNum) {
			string command="DELETE FROM apptreminderrule "
				+"WHERE ApptReminderRuleNum = "+POut.Long(apptReminderRuleNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<ApptReminderRule> listNew,List<ApptReminderRule> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<ApptReminderRule> listIns    =new List<ApptReminderRule>();
			List<ApptReminderRule> listUpdNew =new List<ApptReminderRule>();
			List<ApptReminderRule> listUpdDB  =new List<ApptReminderRule>();
			List<ApptReminderRule> listDel    =new List<ApptReminderRule>();
			listNew.Sort((ApptReminderRule x,ApptReminderRule y) => { return x.ApptReminderRuleNum.CompareTo(y.ApptReminderRuleNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((ApptReminderRule x,ApptReminderRule y) => { return x.ApptReminderRuleNum.CompareTo(y.ApptReminderRuleNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			ApptReminderRule fieldNew;
			ApptReminderRule fieldDB;
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
				else if(fieldNew.ApptReminderRuleNum<fieldDB.ApptReminderRuleNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.ApptReminderRuleNum>fieldDB.ApptReminderRuleNum) {//dbPK less than newPK, dbItem is 'next'
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
				Delete(listDel[i].ApptReminderRuleNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}