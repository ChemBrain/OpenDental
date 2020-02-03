using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBase;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace OpenDentBusiness {
	public partial class ConvertDatabases {

		#region Helper Functions/Variables
		///<summary>These two lists are for To19_2_15 and are meant to be static lists of table names as of that version.</summary>
		private static List<string> _listTableNames=new List<string> {
			"account","accountingautopay","adjustment","alertcategory","alertcategorylink","alertitem","alertread","alertsub","allergy","allergydef",
			"anestheticdata","anestheticrecord","anesthmedsgiven","anesthmedsintake","anesthmedsinventory","anesthmedsinventoryadj","anesthmedsuppliers",
			"anesthscore","anesthvsdata","appointmentrule","appointmenttype","apptfield","apptfielddef","apptreminderrule","apptremindersent","apptview",
			"apptviewitem","asapcomm","autocode","autocodecond","autocodeitem","automation","automationcondition","autonote","autonotecontrol","benefit",
			"bugsubmission","canadiannetwork","carrier","cdcrec","cdspermission","centralconnection","chartview","chatuser","claim","claimattach",
			"claimcondcodelog","claimform","claimformitem","claimpayment","claimsnapshot","claimtracking","claimvalcodelog","clearinghouse","clinic",
			"clinicerx","clinicpref","clockevent","codesystem","commoptout","computer","computerpref","confirmationrequest","connectiongroup",
			"conngroupattach","contact","county","covcat","covspan","cpt","creditcard","custrefentry","custreference","cvx","dashboardar","dashboardcell",
			"dashboardlayout","databasemaintenance","dbmlog","definition","deflink","deletedobject","deposit","dictcustom","discountplan","disease",
			"diseasedef","displayfield","displayreport","dispsupply","documentmisc","drugmanufacturer","drugunit","dunning","ebill","eclipboardsheetdef",
			"eduresource","ehramendment","ehraptobs","ehrcareplan","ehrlab","ehrlabclinicalinfo","ehrlabimage","ehrlabnote","ehrlabresult",
			"ehrlabresultscopyto","ehrlabspecimen","ehrlabspecimencondition","ehrlabspecimenrejectreason","ehrmeasure","ehrmeasureevent","ehrnotperformed",
			"ehrpatient","ehrprovkey","ehrquarterlykey","ehrsummaryccd","ehrtrigger","electid","emailaddress","emailattach","emailautograph",
			"emailmessage","emailmessageuid","emailtemplate","employee","employer","encounter","entrylog","eobattach","equipment","erxlog",
			"eservicebilling","eservicecodelink","eservicesignal","etrans","etrans835attach","evaluation","evaluationcriterion","evaluationcriteriondef",
			"evaluationdef","famaging","familyhealth","faq","fee","feesched","fhircontactpoint","fhirsubscription","fielddeflink","files","formpat",
			"gradingscale","gradingscaleitem","grouppermission","guardian","hcpcs","hl7def","hl7deffield","hl7defmessage","hl7defsegment","hl7msg",
			"hl7procattach","icd10","icd9","insfilingcode","insfilingcodesubtype","insplan","installmentplan","instructor","insverify","insverifyhist",
			"intervention","job","jobcontrol","joblink","joblog","jobnote","jobnotification","jobpermission","jobproject","jobquote","jobreview",
			"jobsprint","jobsprintlink","journalentry","labcase","laboratory","labpanel","labresult","labturnaround","language","languageforeign","letter",
			"lettermerge","lettermergefield","loginattempt","loinc","maparea","medicalorder","medication","medicationpat","medlab","medlabfacattach",
			"medlabfacility","medlabresult","medlabspecimen","mobileappdevice","mount","mountdef","mountitem","mountitemdef","oidexternal","oidinternal",
			"operatory","orionproc","orthochart","orthocharttab","orthocharttablink","patfield","patfielddef","patientlink","patientnote",
			"patientportalinvite","patientrace","patplan","patrestriction","payconnectresponseweb","payment","payortype","payperiod","payplan",
			"payplancharge","paysplit","perioexam","pharmacy","pharmclinic","phone","phonecomp","phoneconf","phoneempdefault","phoneempsubgroup",
			"phonegraph","phonemetric","phonenumber","plannedappt","popup","preference","printer","procapptcolor","procbutton","procbuttonitem",
			"procbuttonquick","proccodenote","procedurecode","procgroupitem","procmultivisit","proctp","program","programproperty","provider",
			"providerclinic","providercliniclink","providererx","providerident","question","questiondef","quickpastecat","quickpastenote","reactivation",
			"recalltrigger","recalltype","reconcile","recurringcharge","refattach","referral","registrationkey","reminderrule","repeatcharge",
			"replicationserver","reqneeded","reqstudent","requiredfield","requiredfieldcondition","reseller","resellerservice","rxalert","rxdef","rxnorm",
			"rxpat","scheduledprocess","schoolclass","schoolcourse","screen","screengroup","screenpat","sheet","sheetdef","sheetfielddef","sigbutdef",
			"sigelementdef","sigmessage","signalod","site","sitelink","smsbilling","smsblockphone","smsfrommobile","smsphone","smstomobile","snomed","sop",
			"stateabbr","statement","stmtlink","substitutionlink","supplier","supply","supplyneeded","supplyorder","supplyorderitem","task","taskancestor",
			"taskhist","tasklist","tasknote","tasksubscription","tasktaken","taskunread","terminalactive","timeadjust","timecardrule","toolbutitem",
			"toothgridcell","toothgridcol","toothgriddef","transaction","treatplanattach","triagemetric","tsitranslog","ucum","updatehistory","userclinic",
			"usergroup","usergroupattach","userod","userodapptview","userodpref","userquery","userweb","vaccinedef","vaccineobs","vaccinepat","vitalsign",
			"voicemail","webchatmessage","webchatpref","webchatsession","webchatsurvey","webforms_log","webforms_preference","webforms_sheet",
			"webforms_sheetdef","webforms_sheetfield","webforms_sheetfielddef","webschedrecall","wikilistheaderwidth","wikilisthist","wikipage",
			"wikipagehist","xchargetransaction","xwebresponse","zipcode"
		};
		///<summary>These two lists are for To19_2_15 and are meant to be static lists of table names as of that version.</summary>
		private static List<string> _listLargeTableNames=new List<string> {
			"appointment","claimproc","commlog","document","etransmessagetext","histappointment","inseditlog","inssub","patient","periomeasure",
			"procedurelog","procnote","recall","schedule","scheduleop","securitylog","securityloghash","sheetfield","toothinitial","treatplan"
		};

		///<summary>Attempts to detach all claim procs with IsTransfer set to true from their corresponding claim payment.
		///Only detaches if the sum of all transfer claim procs (that are going to be detached) equate to $0.
		///This has the potential to orphan claim payments (claim payment with no claim procs attached).
		///These orphaned claim payments will need to be handled manually by the user (DBM ClaimPaymentsNotPartialWithNoClaimProcs).</summary>
		private static void DetachTransferClaimProcsFromClaimPayments() {
			//Get all ClaimNums attached to claimprocs flagged as IsTransfer that are also attached to a claimpayment.
			//The information regarding these claimprocs is being selected instead of updated so that we can perform calculations and handle rounding in C#
			//Several tricky rounding issues made the 'one query to rule them all' hard to read (and we just didn't trust MySQL to do our bidding).
			//Also, using a subselect was slow for MySQL 5.5 which is the officially supported version for Open Dental (v5.6 was fast).
			string command=@"
				SELECT DISTINCT claimproc.ClaimNum
				FROM claimproc
				WHERE claimproc.ClaimNum > 0
				AND claimproc.ClaimPaymentNum > 0
				AND claimproc.IsTransfer=1 ";
			List<long> listClaimNums=Db.GetListLong(command);
			if(listClaimNums.Count > 0) {
				List<long> listClaimProcNumsToUpdate=new List<long>();
				command=$@"
				SELECT claimproc.ClaimProcNum,claimproc.ClaimPaymentNum,claimproc.ClaimNum,claimproc.InsPayAmt,claimproc.IsTransfer
				FROM claimproc
				WHERE claimproc.ClaimNum IN({string.Join(",",listClaimNums.Select(x => POut.Long(x)))})
				AND claimproc.ClaimPaymentNum > 0";
				DataTable table=Db.GetTable(command);
				//Group the claimprocs by ClaimPaymentNum because we do not want to detach offsetting claimprocs that are attached to different claim payments.
				Dictionary<long,List<DataRow>> dictClaimPayNumClaimProcs=table.Select()
					.GroupBy(x => PIn.Long(x["ClaimPaymentNum"].ToString()))
					.ToDictionary(x => x.Key,x => x.ToList());
				//Loop through every claim payment to find and detach any offsetting claimproc transfers that are associated to the same claim.
				foreach(KeyValuePair<long,List<DataRow>> claimProcsForPayment in dictClaimPayNumClaimProcs) {
					//Group the claimprocs for this claim payment by claim because we do not want to detach offsetting claimprocs for different claims.
					Dictionary<long,List<DataRow>> dictClaimNumClaimProcs=claimProcsForPayment.Value
						.GroupBy(x => PIn.Long(x["ClaimNum"].ToString()))
						.ToDictionary(x => x.Key,x => x.ToList());
					//Loop through every claim and find claimproc transfers that can be detached (equate to 0).
					foreach(KeyValuePair<long,List<DataRow>> claimProcsForClaim in dictClaimNumClaimProcs) {
						double sumAllClaimProcsForClaim=claimProcsForClaim.Value.Sum(x => PIn.Double(x["InsPayAmt"].ToString()));
						double sumClaimProcsNoTransfers=claimProcsForClaim.Value
							.Where(x => PIn.Bool(x["IsTransfer"].ToString())==false)
							.Sum(x => PIn.Double(x["InsPayAmt"].ToString()));
						double sumClaimProcsTransfers=claimProcsForClaim.Value
							.Where(x => PIn.Bool(x["IsTransfer"].ToString())==true)
							.Sum(x => PIn.Double(x["InsPayAmt"].ToString()));
						//make sure that the transfer procs equate to 0, that removing them does not change the claimpayment amount
						if(AreNumsEqual(sumAllClaimProcsForClaim,sumClaimProcsNoTransfers) && IsNumZero(sumClaimProcsTransfers)) {
							List<long> listTransferClaimProcsForClaim=claimProcsForClaim.Value
								.Where(x => PIn.Bool(x["IsTransfer"].ToString())==true)
								.Select(y => PIn.Long(y["ClaimProcNum"].ToString()))
								.ToList();
							listClaimProcNumsToUpdate.AddRange(listTransferClaimProcsForClaim);
						}
					}
				}
				if(listClaimProcNumsToUpdate.Count>0) {
					command=$@"UPDATE claimproc SET claimproc.ClaimPaymentNum=0 
										WHERE claimproc.ClaimProcNum IN ({string.Join(",",listClaimProcNumsToUpdate.Select(x => POut.Long(x)))})";
					Db.NonQ(command);
				}
			}
		}

		///<summary>Used to check if two doubles are "equal" based on some epsilon. 
		/// Epsilon is 0.0000001f and will return true if the absolute value of (val - val2) is less than that.</summary>
		private static bool AreNumsEqual(double val,double val2) {
			return IsNumZero(val-val2);
		}

		///<summary>Used to check if a double is "equal" to zero based on some epsilon. 
		/// Epsilon is 0.0000001f and will return true if the absolute value of the double is less than that.</summary>
		private static bool IsNumZero(double val) {
			return Math.Abs(val)<=0.0000001f;
		}
		#endregion Helper Functions/Variables

		private static void To18_3_1() {
			string command;
			DataTable table;
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ClaimTrackingStatusExcludesNone','0')";
			Db.NonQ(command);			
			//command="ALTER TABLE sheetfield ADD TabOrderMobile int NOT NULL"; //Moved and combined into AlterLargeTable call below
			//Db.NonQ(command);
			//command="ALTER TABLE sheetfield ADD UiLabelMobile varchar(255) NOT NULL";//Moved and combined into AlterLargeTable call below
			//Db.NonQ(command);
			//Adding three columns to sheetfield at once using AlterLargeTable() for performance reasons.
			//No translation in convert script.
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 18.3.1 - Adding TabOrderMobile,UiLabelMobile,UiLabelMobileRadioButton to sheetfield.");
			LargeTableHelper.AlterLargeTable("sheetfield","SheetFieldNum",new List<Tuple<string,string>> { Tuple.Create("TabOrderMobile","int NOT NULL"),
				Tuple.Create("UiLabelMobile","varchar(255) NOT NULL"),Tuple.Create("UiLabelMobileRadioButton","varchar(255) NOT NULL") });
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 18.3.1");//No translation in convert script.
			command="ALTER TABLE sheetfielddef ADD TabOrderMobile int NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE sheetfielddef ADD UiLabelMobile varchar(255) NOT NULL";
			Db.NonQ(command);
			//Add edit archived patient permission to everyone
			command="SELECT DISTINCT UserGroupNum FROM grouppermission";
			table=Db.GetTable(command);
			long groupNum;
			foreach(DataRow row in table.Rows) {
				groupNum=PIn.Long(row["UserGroupNum"].ToString());
				command="INSERT INTO grouppermission (UserGroupNum,PermType) "
				   +"VALUES("+POut.Long(groupNum)+",165)";//165 is ArchivedPatientEdit
				Db.NonQ(command);
			}
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 18.3.1 - Adding credit card frequency");//No translation in convert script.
			command="ALTER TABLE creditcard ADD ChargeFrequency varchar(150) NOT NULL";
			Db.NonQ(command);
			command="UPDATE creditcard SET ChargeFrequency=CONCAT('0"//0 for ChargeFrequencyType.FixedDayOfMonth
				+"|',DAY(DateStart)) WHERE YEAR(DateStart) > 1880";
			Db.NonQ(command);
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 18.3.1");//No translation in convert script.
			command="ALTER TABLE insfilingcode ADD GroupType bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE insfilingcode ADD INDEX (GroupType)";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('OpenDentalServiceHeartbeat','0001-01-01 00:00:00')";
			Db.NonQ(command);
			command="SELECT AlertCategoryNum FROM alertcategory WHERE InternalName='OdAllTypes' AND IsHQCategory=1";
			string alertCategoryNum=Db.GetScalar(command);
			command="INSERT INTO alertcategorylink(AlertCategoryNum,AlertType) VALUES('"+alertCategoryNum+"','20')";//20 for alerttype OpenDentalServiceDown
			Db.NonQ(command);
			command="ALTER TABLE rxdef ADD PatientInstruction text NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE rxpat ADD PatientInstruction text NOT NULL";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('SheetsDefaultRxInstructions','0')";
			Db.NonQ(command);
			//Insert Midway Dental Supply bridge----------------------------------------------------------------- 
			command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
			   +") VALUES("
			   +"'Midway', "
			   +"'Midway Dental', "
			   +"'0', "
			   +"'"+POut.String(@"http://www.midwaydental.com/")+"', "
			   +"'"+"', "//No command line args
			   +"'')";
			long programNum=Db.NonQ(command,true);
			command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
			   +"VALUES ("
			   +"'"+POut.Long(programNum)+"', "
			   +"'7', "//ToolBarsAvail.MainToolbar
			   +"'Midway Dental')";
			Db.NonQ(command);
			//end Midway Dental Supply bridge
			command="INSERT INTO preference (PrefName,ValueString) VALUES('InsPayNoWriteoffMoreThanProc','1')";//InsPayNoWriteoffMoreThanProc-default to true
			Db.NonQ(command);
			#region Sales Tax
			//Insert new columns for sales tax properties
			command="ALTER TABLE adjustment ADD TaxTransID bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE adjustment ADD INDEX (TaxTransID)";
			Db.NonQ(command);
			//command="ALTER TABLE procedurelog ADD TaxAmt double NOT NULL"; //Moved to AlterLargeTable call below.
			//Db.NonQ(command); 
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 18.3.1 - Adding TaxAmt to procedurelog.");//No translation in convert script.
			LargeTableHelper.AlterLargeTable("procedurelog","ProcNum",new List<Tuple<string,string>> { Tuple.Create("TaxAmt","double NOT NULL") });
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 18.3.1");//No translation in convert script.
			command="ALTER TABLE procedurecode ADD TaxCode varchar(16) NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE proctp ADD TaxAmt double NOT NULL";
			Db.NonQ(command);
			//Create the Avalara Program Bridge
			command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note" 
				+") VALUES(" 
				+"'AvaTax', " 
				+"'AvaTax from Avalara.com', " 
				+"'0', " 
				+"'', " 
				+"'', "//leave blank if none 
				+"'')"; 
			programNum=Db.NonQ(command,true);
			long defNum=0;
			//Check to see if they already have a 'Sales Tax' Adjustment Type.  If they do, use that definition, otherwise insert a new definition.
			command="SELECT DefNum FROM definition WHERE Category=1 "//DefCat.AdjTypes
				+"AND LOWER(ItemName)='sales tax'";
			table=Db.GetTable(command);
			if(table.Rows.Count > 0) {
				defNum=PIn.Long(table.Rows[0][0].ToString());
			}
			if(defNum==0) {//We didn't find a definition named 'Sales Tax'.
				//Insert new status for 'Sales Tax'
				command="SELECT MAX(ItemOrder)+1 FROM definition WHERE Category=1";
				int maxOrder=Db.GetInt(command);
				command="INSERT INTO definition (Category,ItemOrder,ItemName) VALUES (1,"+POut.Int(maxOrder)+",'Sales Tax')";
				defNum=Db.NonQ(command,true);
			}
			else {
				//Now set the def to not hidden (in case they already had a sales tax def that was hidden)
				command="UPDATE definition SET IsHidden=0 WHERE DefNum="+POut.Long(defNum);
				Db.NonQ(command);
			}
			//Avalara Program Property - SalesTaxAdjustmentType
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				 +") VALUES(" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'Sales Tax Adjustment Type', " 
				 +"'"+POut.Long(defNum)+"')"; 
			Db.NonQ(command);
			//Avalara Program Property - SalesTaxStates
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				 +") VALUES(" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'Taxable States', " 
				 +"'')"; 
			Db.NonQ(command);
			//Avalara Program Property - Username
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				 +") VALUES(" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'Username', " 
				 +"'')";  
			Db.NonQ(command);
			//Avalara Program Property - Password
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				 +") VALUES(" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'Password', " 
				 +"'')";
			Db.NonQ(command);
			//Avalara Program Property - CompanyCode
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				 +") VALUES(" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'Company Code', " 
				 +"'')"; 
			Db.NonQ(command); 
			//Avalara Program Property - Environment
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				 +") VALUES(" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'Test (T) or Production (P)', " 
				 +"'P')"; 
			Db.NonQ(command); 
			//Avalara Program Property - Log Level
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				 +") VALUES(" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'Log Level', " //0 - error, 1- information, 2- verbose 
				 +"'0')"; 
			Db.NonQ(command); 
			//Avalara Program Property - PrepayProcCodes
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				 +") VALUES(" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'Prepay Proc Codes', " //comma-separate list of proccodes we will allow users to pre-pay for
				 +"'')"; 
			Db.NonQ(command);
			//Avalara Program Property -  DiscountedProcCodes
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
				 +") VALUES("
				 +"'"+POut.Long(programNum)+"', "
				 +"'Discount Proc Codes', " //comma-separate list of proccodes we give users discounts on when they prepay.
				 +"'')";
			Db.NonQ(command);
			#endregion Sales Tax
			command="ALTER TABLE substitutionlink ADD SubstitutionCode VARCHAR(25) NOT NULL,ADD SubstOnlyIf int NOT NULL";
			Db.NonQ(command);
			//Set all current substitutionlink.SubstOnlyIf to SubstitutionCondition.Never
			//The rows currently indicate substitutions for codes/conditions that will be ignored for the PlanNum.
			//Setting them to Never maintains this functionality
			command="UPDATE substitutionlink SET SubstOnlyIf=3";//SubstitutionCondition.Never;
			Db.NonQ(command);
			#region HQ Only
			//We are running this section of code for HQ only
			//This is very uncommon and normally manual queries should be run instead of doing a convert script.
			command="SELECT ValueString FROM preference WHERE PrefName='DockPhonePanelShow'";
			table=Db.GetTable(command);
			if(table.Rows.Count > 0 && PIn.Bool(table.Rows[0][0].ToString())) {
				command="DROP TABLE IF EXISTS jobnotification";
				Db.NonQ(command);
				command=@"CREATE TABLE jobnotification (
					JobNotificationNum BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
					JobNum BIGINT NOT NULL,
					UserNum BIGINT NOT NULL,
					Changes TINYINT(4) NOT NULL,
					INDEX(JobNum),
					INDEX(UserNum)
					) DEFAULT CHARSET=utf8";
				Db.NonQ(command);
			}
			#endregion
			//command="ALTER TABLE sheetfield ADD UiLabelMobileRadioButton varchar(255) NOT NULL"; //Moved to AlterLargeTable call above.
			//Db.NonQ(command);
			command="ALTER TABLE sheetfielddef ADD UiLabelMobileRadioButton varchar(255) NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE sheet ADD HasMobileLayout tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE sheetdef ADD HasMobileLayout tinyint NOT NULL";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('PromptForSecondaryClaim','1')";//default to true.
			Db.NonQ(command);
			command="ALTER TABLE repeatcharge ADD ChargeAmtAlt double NOT NULL";
			Db.NonQ(command);
		}

		private static void To18_3_2() {
			string command;
			command="UPDATE repeatcharge SET ChargeAmtAlt=-1";//This indicates that they have not started using Zipwhip yet.
			Db.NonQ(command);
		}

		private static void To18_3_4() {
			string command;
			command="ALTER TABLE claimform ADD Width int NOT NULL";
			Db.NonQ(command);
			command="UPDATE claimform SET Width=850";
			Db.NonQ(command);
			command="ALTER TABLE claimform ADD Height int NOT NULL";
			Db.NonQ(command);
			command="UPDATE claimform SET Height=1100";
			Db.NonQ(command);
		}

		private static void To18_3_8() {
			string command;
			command="SELECT COUNT(*) FROM program WHERE ProgName='PandaPeriodAdvanced'";
			//We may have already added this bridge in 18.2.29
			if(Db.GetCount(command)=="0") {
				//Update current PandaPerio description to Panda Perio (simple)
				command="UPDATE program SET ProgDesc='Panda Perio (simple) from www.pandaperio.com' WHERE ProgName='PandaPerio'";
				Db.NonQ(command);
				//Insert PandaPeriodAdvanced bridge----------------------------------------------------------------- 
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					 +") VALUES("
					 +"'PandaPeriodAdvanced', "
					 +"'Panda Perio (advanced) from www.pandaperio.com', "
					 +"'0', "
					 +"'"+POut.String(@"C:\Program Files (x86)\Panda Perio\Panda.exe")+"', "
					 +"'', "//leave blank if none 
					 +"'')";
				long programNum=Db.NonQ(command,true);
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					 +") VALUES("
					 +"'"+POut.Long(programNum)+"', "
					 +"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					 +"'0')";
				Db.NonQ(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					 +"VALUES ("
					 +"'"+POut.Long(programNum)+"', "
					 +"'2', "//ToolBarsAvail.ChartModule 
					 +"'PandaPeriodAdvanced')";
				Db.NonQ(command);
				//end PandaPeriodAdvanced bridge 
			}
		}

		private static void To18_3_9() {
			string command;
			//Add Avalara Program Property for Tax Exempt Pat Field
			command="SELECT ProgramNum FROM program WHERE ProgName='AvaTax'";
			long programNum=Db.GetLong(command);
			command="SELECT COUNT(*) FROM programproperty WHERE ProgramNum='"+POut.Long(programNum)+"' AND PropertyDesc='Tax Exempt Pat Field Def'";
			string taxExemptProperty=Db.GetCount(command);
			if(taxExemptProperty=="0") {
				//Avalara Program Property - SalesTaxAdjustmentType
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
						+") VALUES(" 
						+"'"+POut.Long(programNum)+"', " 
						+"'Tax Exempt Pat Field Def', " 
						+"'')"; 
				Db.NonQ(command);
			}
		}

		private static void To18_3_15() {
			string command;
			command="ALTER TABLE emailtemplate ADD IsHtml tinyint NOT NULL";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('PrintStatementsAlphabetically','0')";
			Db.NonQ(command);
		}

		private static void To18_3_18() {
			string command;
			//Some of the default Web Sched Notify templates had '[ApptDate]' where they should have had '[ApptTime]'.
			foreach(string prefName in new List<string> { "WebSchedVerifyRecallText","WebSchedVerifyNewPatText","WebSchedVerifyASAPText" }) {
				command="SELECT ValueString FROM preference WHERE PrefName='"+POut.String(prefName)+"'";
				string curValue=Db.GetScalar(command);
				if(curValue=="Appointment scheduled for [FName] on [ApptDate] [ApptDate] at [OfficeName], [OfficeAddress]") {
					string newValue="Appointment scheduled for [FName] on [ApptDate] [ApptTime] at [OfficeName], [OfficeAddress]";
					command="UPDATE preference SET ValueString='"+POut.String(newValue)+"' WHERE PrefName='"+POut.String(prefName)+"'";
					Db.NonQ(command);
				}
			}
			//Remove supplyorders with supplynum=0 and update supplyorder totals
			command="SELECT DISTINCT SupplyOrderNum FROM supplyorderitem WHERE SupplyNum=0";
			List<long> supplyOrderNums=Db.GetListLong(command);
			for(int i=0;i<supplyOrderNums.Count;i++) {
				command="UPDATE supplyorder SET AmountTotal="
					+"(SELECT SUM(Qty*Price) FROM supplyorderitem WHERE SupplyOrderNum="+POut.Long(supplyOrderNums[i])+" AND SupplyNum!=0) "
					+"WHERE SupplyOrderNum="+POut.Long(supplyOrderNums[i]);
				Db.NonQ(command);
			}
			command="DELETE FROM supplyorderitem WHERE SupplyNum=0";
			Db.NonQ(command);
		}

		private static void To18_3_19() {
			string command;
			command="SELECT ProgramNum,Enabled FROM program WHERE ProgName='eRx'";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count>0 && PIn.Bool(table.Rows[0]["Enabled"].ToString())) {
				long programErx=PIn.Long(table.Rows[0]["ProgramNum"].ToString());
				command=$"SELECT PropertyValue FROM programproperty WHERE PropertyDesc='eRx Option' AND ProgramNum={programErx}";
				//0 is the enum value of Legacy, 1 is the enum value of DoseSpot
				bool isNewCrop=PIn.Int(Db.GetScalar(command))==0;
				if(isNewCrop) {//Only update rows if the office has eRx enabled and is using NewCrop.
					command="UPDATE provider SET IsErxEnabled=2 WHERE IsErxEnabled=1";
					Db.NonQ(command);
				}
			}
			//check databasemaintenance for TransactionsWithFutureDates, insert if not there and set IsOld to True or update to set IsOld to true
			command="SELECT MethodName FROM databasemaintenance WHERE MethodName='TransactionsWithFutureDates'";
			string methodName=Db.GetScalar(command);
			if(methodName=="") {//didn't find row in table, insert
				command="INSERT INTO databasemaintenance (MethodName,IsOld) VALUES ('TransactionsWithFutureDates',1)";
			}
			else {//found row, update IsOld
				command="UPDATE databasemaintenance SET IsOld = 1 WHERE MethodName = 'TransactionsWithFutureDates'";
			}
			Db.NonQ(command);
			//Add Avalara Program Property for TaxCodeOverrides
			command="SELECT ProgramNum FROM program WHERE ProgName='AvaTax'";
			long programNum=Db.GetLong(command);
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				+") VALUES(" 
				+"'"+POut.Long(programNum)+"', " 
				+"'Tax Code Overrides', " 
				+"'')"; 
			Db.NonQ(command);
		}

		private static void To18_3_21() {
			string command;
			command="INSERT INTO preference (PrefName,ValueString) VALUES('SaveDXCAttachments','1')";
			Db.NonQ(command);
		}

		private static void To18_3_22() {
			string command;
			//We are running this section of code for HQ only
			//This is very uncommon and normally manual queries should be run instead of doing a convert script.
			command="SELECT ValueString FROM preference WHERE PrefName='DockPhonePanelShow'";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count > 0 && PIn.Bool(table.Rows[0][0].ToString())) {
				//Change TimeEstimate to TimeEstimateDevelopment
				command="ALTER TABLE job CHANGE COLUMN TimeEstimate TimeEstimateDevelopment bigint(20) NOT NULL";
				Db.NonQ(command);
				command="ALTER TABLE job ADD TimeEstimateConcept bigint NOT NULL";
				Db.NonQ(command);
				command="ALTER TABLE job ADD INDEX (TimeEstimateConcept)";
				Db.NonQ(command);
				command="ALTER TABLE job ADD TimeEstimateWriteup bigint NOT NULL";
				Db.NonQ(command);
				command="ALTER TABLE job ADD INDEX (TimeEstimateWriteup)";
				Db.NonQ(command);
				command="ALTER TABLE job ADD TimeEstimateReview bigint NOT NULL";
				Db.NonQ(command);
				command="ALTER TABLE job ADD INDEX (TimeEstimateReview)";
				Db.NonQ(command);
			}
		}

		private static void To18_3_23() {
			string command;
			command="ALTER TABLE confirmationrequest ADD DoNotResend tinyint NOT NULL";
			Db.NonQ(command);
		}

		private static void To18_3_24() {
			string command;
			command="UPDATE displayfield SET InternalName='Country' WHERE InternalName='Contry' AND Category=16;";
			Db.NonQ(command);
		}

		private static void To18_3_26() {
			string command;
			FixB11013();
			//Moving codes to the Obsolete category that were deleted in CDT 2019.
			if(CultureInfo.CurrentCulture.Name.EndsWith("US")) {//United States
				//Move deprecated codes to the Obsolete procedure code category.
				//Make sure the procedure code category exists before moving the procedure codes.
				string procCatDescript="Obsolete";
				long defNum=0;
				command="SELECT DefNum FROM definition WHERE Category=11 AND ItemName='"+POut.String(procCatDescript)+"'";//11 is DefCat.ProcCodeCats
				DataTable dtDef=Db.GetTable(command);
				if(dtDef.Rows.Count==0) { //The procedure code category does not exist, add it
					command="SELECT COUNT(*) FROM definition WHERE Category=11";//11 is DefCat.ProcCodeCats
					int countCats=PIn.Int(Db.GetCount(command));
					command="INSERT INTO definition (Category,ItemName,ItemOrder) "
								+"VALUES (11"+",'"+POut.String(procCatDescript)+"',"+POut.Int(countCats)+")";//11 is DefCat.ProcCodeCats
					defNum=Db.NonQ(command,true);
				}
				else { //The procedure code category already exists, get the existing defnum
					defNum=PIn.Long(dtDef.Rows[0]["DefNum"].ToString());
				}
				string[] cdtCodesDeleted=new string[] {
					"D1515",
					"D1525",
					"D5281",
					"D9940"
				};
				//Change the procedure codes' category to Obsolete.
				command="UPDATE procedurecode SET ProcCat="+POut.Long(defNum)
					+" WHERE ProcCode IN('"+string.Join("','",cdtCodesDeleted.Select(x => POut.String(x)))+"') ";
				Db.NonQ(command);
			}//end United States CDT codes update
		}

		private static void To18_3_30() {
			string command;
			command=@"UPDATE preference SET ValueString='https://www.patientviewer.com:49997/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx' 
				WHERE PrefName='WebServiceHQServerURL' AND ValueString='http://www.patientviewer.com:49999/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx'";
			Db.NonQ(command);
		}

		private static void To18_3_49() {
			string command;
			command="UPDATE preference SET ValueString='https://opendentalsoft.com:1943/WebServiceCustomerUpdates/Service1.asmx' "
				+"WHERE PrefName='UpdateServerAddress' AND ValueString='http://opendentalsoft.com:1942/WebServiceCustomerUpdates/Service1.asmx'";
			Db.NonQ(command);
		}

		private static void To18_4_1() {
			string command;
			DataTable table;
			command="ALTER TABLE rxpat ADD ClinicNum bigint NOT NULL,ADD INDEX (ClinicNum)";
			Db.NonQ(command);
			//Set rxpat's ClinicNum to default patient's ClinicNum
			command=@"UPDATE rxpat
				INNER JOIN patient ON rxpat.PatNum=patient.PatNum
				SET rxpat.ClinicNum=patient.ClinicNum";
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS pharmclinic";
			Db.NonQ(command);
			command=@"CREATE TABLE pharmclinic (
					PharmClinicNum bigint NOT NULL auto_increment PRIMARY KEY,
					PharmacyNum bigint NOT NULL,
					ClinicNum bigint NOT NULL,
					INDEX(PharmacyNum),
					INDEX(ClinicNum)
					) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			//Fill table with current combinations of ClinicNums and PharmacyNums that appear in the rxpat table
			command=@"INSERT INTO pharmclinic (PharmacyNum,ClinicNum)
				SELECT DISTINCT PharmacyNum,ClinicNum FROM rxpat WHERE PharmacyNum != 0";
			Db.NonQ(command);
			//Convert appt reminder rules' plain text email templates into HTML templates.
			command=@"SELECT ApptReminderRuleNum,TemplateEmail,TemplateEmailAggShared,TemplateEmailAggPerAppt,TypeCur
				FROM apptreminderrule";
			table=Db.GetTable(command);
			command="SELECT ValueString FROM preference WHERE PrefName='EmailDisclaimerIsOn'";
			bool isEmailDisclaimerOn=(Db.GetScalar(command)=="1");
			Func<string,bool,string> fConvertReminderRule=new Func<string, bool, string>((template,includeDisclaimer) =>
				template
					.Replace(">","&>")
					.Replace("<","&<")
					+(includeDisclaimer && isEmailDisclaimerOn ? "\r\n\r\n\r\n[EmailDisclaimer]" : "")
			);
			foreach(DataRow row in table.Rows) {
				string templateEmail=fConvertReminderRule(PIn.String(row["TemplateEmail"].ToString()),true);
				if(PIn.Int(row["TypeCur"].ToString())==1) {//Confirmation
					templateEmail=templateEmail.Replace("[ConfirmURL]","<a href=\"[ConfirmURL]\">[ConfirmURL]</a>");
				}
				if(PIn.Int(row["TypeCur"].ToString())==3) {//Patient Portal Invites
					templateEmail=templateEmail.Replace("[PatientPortalURL]","<a href=\"[PatientPortalURL]\">[PatientPortalURL]</a>");
				}
				string templateEmailAggShared=fConvertReminderRule(PIn.String(row["TemplateEmailAggShared"].ToString()),true);
				if(PIn.Int(row["TypeCur"].ToString())==1) {//Confirmation
					templateEmailAggShared=templateEmailAggShared.Replace("[ConfirmURL]","<a href=\"[ConfirmURL]\">[ConfirmURL]</a>");
				}
				if(PIn.Int(row["TypeCur"].ToString())==3) {//Patient Portal Invites
					templateEmailAggShared=templateEmailAggShared.Replace("[PatientPortalURL]","<a href=\"[PatientPortalURL]\">[PatientPortalURL]</a>");
				}
				string templateEmailAggPerAppt=fConvertReminderRule(PIn.String(row["TemplateEmailAggPerAppt"].ToString()),false);
				command=@"UPDATE apptreminderrule SET 
					TemplateEmail='"+POut.String(templateEmail)+@"',
					templateEmailAggShared='"+POut.String(templateEmailAggShared)+@"',
					TemplateEmailAggPerAppt='"+POut.String(templateEmailAggPerAppt)+@"'
					WHERE ApptReminderRuleNum="+POut.Long(PIn.Long(row["ApptReminderRuleNum"].ToString()));
				Db.NonQ(command);
			}
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EraIncludeWOPercCoPay','1')";
			Db.NonQ(command);
			command="ALTER TABLE tsitranslog ADD ClinicNum bigint NOT NULL,ADD INDEX (ClinicNum)";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReplaceExistingBlockout','0')";
			Db.NonQ(command);
			command="ALTER TABLE etrans835attach ADD DateTimeEntry datetime NOT NULL DEFAULT '0001-01-01 00:00:00'";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('ArchiveKey','')";
			Db.NonQ(command);		
			#region DentalXChange Patient Credit Score Bridge
			command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note" 
				+") VALUES(" 
				+"'DXCPatientCreditScore', " 
				+"'DentalXChange Patient Credit Score from register.dentalxchange.com', "
				+"'0', " 
				+"'', "//Takes to web portal. No local executable
				+"'', "
				+"'')"; 
			long programNum=Db.NonQ(command,true); 
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
			   +") VALUES("
			   +"'"+POut.Long(programNum)+"', "
			   +"'Disable Advertising', "
			   +"'0')";
			Db.NonQ(command);
			command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
			   +"VALUES ("
			   +"'"+POut.Long(programNum)+"', "
			   +"'0', "//ToolBarsAvail.AccoutModule
			   +"'DXC Patient Credit Score')";
			Db.NonQ(command);
			#endregion
			command="INSERT INTO preference (PrefName,ValueString) VALUES('InsEstRecalcReceived','0')";
			Db.NonQ(command);	
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 18.4.1 - Adding index to appointment.Priority.");//No translation in convert script.
			LargeTableHelper.AlterLargeTable("appointment","AptNum",null,
				new List<Tuple<string,string>> { Tuple.Create("Priority","") });//no need to send index name. only adds index if not exists
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 18.4.1");//No translation in convert script.
			command="SELECT DISTINCT UserGroupNum FROM grouppermission";
			table=Db.GetTable(command);
			long groupNum;
			foreach(DataRow row in table.Rows) {
				 groupNum=PIn.Long(row["UserGroupNum"].ToString());
				 command="INSERT INTO grouppermission (UserGroupNum,PermType) "
						+"VALUES("+POut.Long(groupNum)+",169)";  //169 is InsuranceVerification
				 Db.NonQ(command);
			}
			//Add columns to confirmationrequest
			command="ALTER TABLE confirmationrequest ADD SmsSentOk tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE confirmationrequest ADD EmailSentOk tinyint NOT NULL";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('UnscheduledListNoRecalls','1')";//True by default.
			Db.NonQ(command);	
			command="INSERT INTO preference (PrefName,ValueString) VALUES('EnterpriseApptList','0')";
			Db.NonQ(command);
			//Insurance History preferences
			command="INSERT INTO preference(PrefName,ValueString) SELECT 'InsHistExamCodes',ValueString FROM preference WHERE PrefName='InsBenExamCodes'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) SELECT 'InsHistProphyCodes',ValueString FROM preference WHERE PrefName='InsBenProphyCodes'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) SELECT 'InsHistBWCodes',ValueString FROM preference WHERE PrefName='InsBenBWCodes'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) SELECT 'InsHistPanoCodes',ValueString FROM preference WHERE PrefName='InsBenPanoCodes'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) SELECT 'InsHistPerioURCodes',ValueString FROM preference WHERE PrefName='InsBenSRPCodes'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) SELECT 'InsHistPerioULCodes',ValueString FROM preference WHERE PrefName='InsBenSRPCodes'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) SELECT 'InsHistPerioLRCodes',ValueString FROM preference WHERE PrefName='InsBenSRPCodes'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) SELECT 'InsHistPerioLLCodes',ValueString FROM preference WHERE PrefName='InsBenSRPCodes'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) SELECT 'InsHistPerioMaintCodes',ValueString FROM preference WHERE PrefName='InsBenPerioMaintCodes'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) SELECT 'InsHistDebridementCodes',ValueString FROM preference WHERE PrefName='InsBenFullDebridementCodes'";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('ApptAutoRefreshRange','4')";//Default to 4
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('RecurringChargesPayType','0')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('InsPlanUseUcrFeeForExclusions','0')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('InsPlanExclusionsMarkDoNotBillIns','0')";
			Db.NonQ(command);
			command="ALTER TABLE insplan ADD ExclusionFeeRule tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE procbutton ADD IsMultiVisit tinyint NOT NULL";
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS procmultivisit";
			Db.NonQ(command);
			command=@"CREATE TABLE procmultivisit (
				ProcMultiVisitNum bigint NOT NULL auto_increment PRIMARY KEY,
				GroupProcMultiVisitNum bigint NOT NULL,
				ProcNum bigint NOT NULL,
				ProcStatus tinyint NOT NULL,
				IsInProcess tinyint NOT NULL,
				INDEX(GroupProcMultiVisitNum),
				INDEX(ProcNum),
				INDEX(IsInProcess)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			//Create an Enterprise Setup Window
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ShowFeatureEnterprise','0')";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('RepeatingChargesAutomated','0')";//False by default
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('RepeatingChargesAutomatedTime','2018-01-01 08:00:00')";//8:00am by default(date portion not used).
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('RepeatingChargesRunAging','1')";//True by default
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('RepeatingChargesLastDateTime','0001-01-01 00:00:00')";//Initialize to MinDate.
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('RecurringChargesBeginDateTime','')";//Initialize to empty.
			Db.NonQ(command);
			command="ALTER TABLE automation ADD PatStatus tinyint NOT NULL";
			Db.NonQ(command);
		}//End of 18_4_1() method

		private static void To18_4_16() {
			string command;
			//Add Avalara Program Property for Sales Tax Return Adjustment Type
			command="SELECT ProgramNum FROM program WHERE ProgName='AvaTax'";
			long programNum=Db.GetLong(command);
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				+") VALUES(" 
				+"'"+POut.Long(programNum)+"', " 
				+"'Sales Tax Return Adjustment Type', " 
				+"'')"; 
			Db.NonQ(command);
			//Add Avalara Program Property for Tax Lock Date
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				+") VALUES(" 
				+"'"+POut.Long(programNum)+"', " 
				+"'Tax Lock Date', " 
				+"'')"; 
			Db.NonQ(command);
		}//End of 18_4_16() method
	
		private static void To18_4_17() {
			string command;
			command="UPDATE preference SET ValueString=SUBSTRING_INDEX(ValueString,',',1) WHERE PrefName='InsHistExamCodes'";
			Db.NonQ(command);
			command="UPDATE preference SET ValueString=SUBSTRING_INDEX(ValueString,',',1) WHERE PrefName='InsHistProphyCodes'";
			Db.NonQ(command);
			command="UPDATE preference SET ValueString=SUBSTRING_INDEX(ValueString,',',1) WHERE PrefName='InsHistBWCodes'";
			Db.NonQ(command);
			command="UPDATE preference SET ValueString=SUBSTRING_INDEX(ValueString,',',1) WHERE PrefName='InsHistPanoCodes'";
			Db.NonQ(command);
			command="UPDATE preference SET ValueString=SUBSTRING_INDEX(ValueString,',',1) WHERE PrefName='InsHistPerioURCodes'";
			Db.NonQ(command);
			command="UPDATE preference SET ValueString=SUBSTRING_INDEX(ValueString,',',1) WHERE PrefName='InsHistPerioULCodes'";
			Db.NonQ(command);
			command="UPDATE preference SET ValueString=SUBSTRING_INDEX(ValueString,',',1) WHERE PrefName='InsHistPerioLRCodes'";
			Db.NonQ(command);
			command="UPDATE preference SET ValueString=SUBSTRING_INDEX(ValueString,',',1) WHERE PrefName='InsHistPerioLLCodes'";
			Db.NonQ(command);
			command="UPDATE preference SET ValueString=SUBSTRING_INDEX(ValueString,',',1) WHERE PrefName='InsHistPerioMaintCodes'";
			Db.NonQ(command);
			command="UPDATE preference SET ValueString=SUBSTRING_INDEX(ValueString,',',1) WHERE PrefName='InsHistDebridementCodes'";
			Db.NonQ(command);
		}//End of 18_4_17() method

		private static void To18_4_22() {
			string command;
			command="UPDATE preference SET ValueString='https://opendentalsoft.com:1943/WebServiceCustomerUpdates/Service1.asmx' "
				+"WHERE PrefName='UpdateServerAddress' AND ValueString='http://opendentalsoft.com:1942/WebServiceCustomerUpdates/Service1.asmx'";
			Db.NonQ(command);
		}

		private static void To18_4_29() {
			string command;
			command="ALTER TABLE tsitranslog ADD AggTransLogNum bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE tsitranslog ADD INDEX (AggTransLogNum)";
			Db.NonQ(command);
		}

		private static void To19_1_1() {
			string command;
			//Oryx bridge-------------------------------------------------
			command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
				+") VALUES("
				+"'Oryx', "
				+"'Oryx from oryxdentalsoftware.com', "
				+"'0', "
				+"'', "//Opens website. No local executable.
				+"'', "
				+"'')";
			long programNum=Db.NonQ(command,true);
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
				 +") VALUES("
				 +"'"+POut.Long(programNum)+"', "
				 +"'Client URL', "
				 +"'')";
			Db.NonQ(command);
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
				 +") VALUES("
				 +"'"+POut.Long(programNum)+"', "
				 +"'Disable Advertising', "
				 +"'0')";
			Db.NonQ(command);
			command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
				 +"VALUES ("
				 +"'"+POut.Long(programNum)+"', "
				 +"'2', "//ToolBarsAvail.ChartModule
				 +"'Oryx')";
			Db.NonQ(command);
			//End Oryx bridge---------------------------------------------
			command="INSERT INTO preference(PrefName,ValueString) VALUES('FeesUseCache','0')";
			Db.NonQ(command);
			command="ALTER TABLE providerclinic ADD StateLicense varchar(50) NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE providerclinic ADD StateRxID varchar(255) NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE providerclinic ADD StateWhereLicensed varchar(15) NOT NULL";
			Db.NonQ(command);
			command="SELECT ProvNum FROM providerclinic WHERE ClinicNum=0 GROUP BY ProvNum";
			List<long> listDefaultProvClinicsProvNums=Db.GetListLong(command);
			command="SELECT ProvNum,DEANum,StateLicense,StateRxID,StateWhereLicensed FROM provider";
			Dictionary<long,DataRow> dictProvs=Db.GetTable(command).Select().ToDictionary(x=> PIn.Long(x["ProvNum"].ToString()));
			foreach(KeyValuePair<long,DataRow> kvp in dictProvs) {
				string stateRxId=kvp.Value["StateRxID"].ToString();
				string stateLicense=kvp.Value["StateLicense"].ToString();
				string stateWhereLicensed=kvp.Value["StateWhereLicensed"].ToString();
				if(kvp.Key.In(listDefaultProvClinicsProvNums)) {
					//A default providerClinic already exist for this provider. Update the providerclinic row for this provider with ClinicNum=0
					command="UPDATE providerclinic SET StateLicense='"+POut.String(stateLicense)+"', "
						+"StateRxID='"+POut.String(stateRxId)+"', "
						+"StateWhereLicensed='"+POut.String(stateWhereLicensed)+"' "
						+"WHERE ProvNum="+POut.Long(kvp.Key)+" AND ClinicNum=0";
				}
				else {
					//Providerclinic doesn't exist for this provider. Add a new default providerclinic for ClinicNum=0
					command="INSERT INTO providerclinic (ProvNum,ClinicNum,DEANum,StateLicense,StateRxID,StateWhereLicensed) "
						+"VALUES ("+POut.Long(kvp.Key)+","//ProvNum
							+"0,"//ClinicNum
							+"'"+POut.String(kvp.Value["DEANum"].ToString())+"',"
							+"'"+POut.String(stateLicense)+"',"
							+"'"+POut.String(stateRxId)+"',"
							+"'"+POut.String(stateWhereLicensed)+"')";
				}
				Db.NonQ(command);
			}
			command="SELECT ProgramNum FROM program WHERE ProgName='PayConnect'";
			long payConnectProgNum=PIn.Long(Db.GetScalar(command));
			command="SELECT DISTINCT ClinicNum FROM programproperty WHERE ProgramNum="+POut.Long(payConnectProgNum);
			List<long> listClinicNums=Db.GetListLong(command);
			foreach(long clinicNum in listClinicNums) {
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue,ComputerName,ClinicNum"
					 +") VALUES("
					 +"'"+POut.Long(payConnectProgNum)+"', "
					 +"'Patient Portal Payments Token', "
					 +"'',"
					 +"'',"
					 +POut.Long(clinicNum)+")";
				Db.NonQ(command);
			}
			command="SELECT MAX(displayreport.ItemOrder) FROM displayreport WHERE displayreport.Category = 2"; //monthly
			long itemorder = Db.GetLong(command)+1; //get the next available ItemOrder for the Monthly Category to put this new report last.
			command="INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) "
				 +"VALUES('ODProcOverpaid',"+POut.Long(itemorder)+",'Procedures Overpaid',2,0)";
			long reportNumNew=Db.NonQ(command,getInsertID:true);
			long reportNum=Db.GetLong("SELECT DisplayReportNum FROM displayreport WHERE InternalName='ODProcsNotBilled'");
			List<long> listGroupNums=Db.GetListLong("SELECT UserGroupNum FROM grouppermission WHERE PermType=22 AND FKey="+POut.Long(reportNum));
			foreach(long groupNumCur in listGroupNums) {
				command="INSERT INTO grouppermission (NewerDate,NewerDays,UserGroupNum,PermType,FKey) "
					 +"VALUES('0001-01-01',0,"+POut.Long(groupNumCur)+",22,"+POut.Long(reportNumNew)+")";
				Db.NonQ(command);
			}
			//Add preference WebSchedRecallDoubleBooking, default to 1 which will preserve old behavior by preventing double booking.
			command="INSERT INTO preference(PrefName,ValueString) VALUES('WebSchedRecallDoubleBooking','1')";
			Db.NonQ(command);
			//Add preference WebSchedNewPatApptDoubleBooking, default to 1 which will preserve old behavior by preventing double booking.
			command="INSERT INTO preference(PrefName,ValueString) VALUES('WebSchedNewPatApptDoubleBooking','1')";
			Db.NonQ(command);
			//Add user column to alertItem table
			command="ALTER TABLE alertitem ADD UserNum bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE alertitem ADD INDEX (UserNum)";
			Db.NonQ(command);
			long itemOrder=Db.GetLong("SELECT ItemOrder FROM displayreport WHERE InternalName='ODProviderPayrollSummary'")+1;
			command="UPDATE displayreport SET ItemOrder="+POut.Long(itemOrder)+" WHERE InternalName='ODProviderPayrollSummary'";
			Db.NonQ(command);//Move down provider payroll summary
			itemOrder=Db.GetLong("SELECT ItemOrder FROM displayreport WHERE InternalName='ODProviderPayrollDetailed'")+1;
			command="UPDATE displayreport SET ItemOrder="+POut.Long(itemOrder)+" WHERE InternalName='ODProviderPayrollDetailed'";
			Db.NonQ(command);//Move down provider payroll detailed
			itemOrder=Db.GetLong("SELECT displayreport.ItemOrder FROM displayreport WHERE displayreport.InternalName='ODMoreOptions'")+1;
			command="INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden,IsVisibleInSubMenu) "
					+"VALUES('ODMonthlyProductionGoal',"+POut.Long(itemOrder)+",'Monthly Prod Inc Goal',0,0,0)";//Insert Monthly Production Goal in the new space we just made after More Options
			Db.NonQ(command);
			long moreOptionsFKey=Db.GetLong("SELECT DisplayReportNum FROM displayreport WHERE InternalName='ODMoreOptions'");
			DataTable userGroupTable=Db.GetTable("SELECT UserGroupNum FROM grouppermission WHERE PermType=22 AND FKey="+POut.Long(moreOptionsFKey));
			reportNum=Db.GetLong("SELECT DisplayReportNum FROM displayreport WHERE InternalName='ODMonthlyProductionGoal'");
			foreach(DataRow row in userGroupTable.Rows) {
					command="INSERT INTO grouppermission (NewerDate,NewerDays,UserGroupNum,PermType,FKey) "
						+"VALUES('0001-01-01',0,"+POut.Long(PIn.Long(row["UserGroupNum"].ToString()))+",22,"+POut.Long(reportNum)+")";
					Db.NonQ(command);
			}
			//Create the Reactivatione Table
			command="DROP TABLE IF EXISTS reactivation";
				Db.NonQ(command);
				command=@"CREATE TABLE reactivation (
					ReactivationNum bigint NOT NULL auto_increment PRIMARY KEY,
					PatNum bigint NOT NULL,
					ReactivationStatus bigint NOT NULL,
					ReactivationNote text NOT NULL,
					DoNotContact tinyint NOT NULL,
					INDEX(PatNum),
					INDEX(ReactivationStatus)
					) DEFAULT CHARSET=utf8";
				Db.NonQ(command);
			//Add Reactivation Preferences
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ShowFeatureReactivations','0')";
			Db.NonQ(command);
			//Add preference ReactivationContactInterval
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReactivationContactInterval','0')";
			Db.NonQ(command);
			//Add preference ReactivationCountContactMax
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReactivationCountContactMax','0')";
			Db.NonQ(command);
			//Add preference ReactivationDaysPast
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReactivationDaysPast','1095')";
			Db.NonQ(command);
			//Add preference ReactivationEmailFamMsg
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReactivationEmailFamMsg','')";
			Db.NonQ(command);
			//Add preference ReactivationEmailMessage
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReactivationEmailMessage','')";
			Db.NonQ(command);
			//Add preference ReactivationEmailSubject
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReactivationEmailSubject','')";
			Db.NonQ(command);
			//Add preference ReactivationGroupByFamily
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReactivationGroupByFamily','0')";
			Db.NonQ(command);
			//Add preference ReactivationPostcardFamMsg
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReactivationPostcardFamMsg','')";
			Db.NonQ(command);
			//Add preference ReactivationPostcardMessage
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReactivationPostcardMessage','')";
			Db.NonQ(command);
			//Add preference ReactivationPostcardsPerSheet
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReactivationPostcardsPerSheet','3')";
			Db.NonQ(command);
			//Add preference ReactivationStatusEmailed
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReactivationStatusEmailed','0')";
			Db.NonQ(command);
			//Add preference ReactivationStatusEmailedTexted
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReactivationStatusEmailedTexted','0')";
			Db.NonQ(command);
			//Add preference ReactivationStatusMailed
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReactivationStatusMailed','0')";
			Db.NonQ(command);
			//Add preference ReactivationStatusTexted
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReactivationStatusTexted','0')";
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS mobileappdevice";
			Db.NonQ(command);
			command=@"CREATE TABLE mobileappdevice (
					MobileAppDeviceNum bigint NOT NULL auto_increment PRIMARY KEY,
					ClinicNum bigint NOT NULL,
					DeviceName varchar(255) NOT NULL,
					UniqueID varchar(255) NOT NULL,
					IsAllowed tinyint NOT NULL,
					LastAttempt datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
					LastLogin datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
					INDEX(ClinicNum)
					) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('DatabaseMode','0')";//Default to 'Normal'
			Db.NonQ(command);
			command="ALTER TABLE dunning ADD IsSuperFamily tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE registrationkey ADD HasEarlyAccess tinyint NOT NULL";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('Ins834DropExistingPatPlans','0')";//Default to false
			Db.NonQ(command);
			LargeTableHelper.AlterLargeTable("treatplan","TreatPlanNum",new List<Tuple<string,string>> {
				Tuple.Create("SignaturePractice","text NOT NULL"),
				Tuple.Create("DateTSigned","datetime NOT NULL DEFAULT '0001-01-01 00:00:00'"),
				Tuple.Create("DateTPracticeSigned","datetime NOT NULL DEFAULT '0001-01-01 00:00:00'"),
				Tuple.Create("SignatureText","varchar(255) NOT NULL"),
				Tuple.Create("SignaturePracticeText","varchar(255) NOT NULL")
			});
			command="DROP TABLE IF EXISTS providercliniclink";
			Db.NonQ(command);
			command=@"CREATE TABLE providercliniclink (
				ProviderClinicLinkNum bigint NOT NULL auto_increment PRIMARY KEY,
				ProvNum bigint NOT NULL,
				ClinicNum bigint NOT NULL,
				INDEX(ProvNum),
				INDEX(ClinicNum)
			) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			//PaySimple ACH Payment Type
			command="SELECT ProgramNum FROM program WHERE ProgName='PaySimple'";
			programNum=Db.GetLong(command);
			command=$@"SELECT ClinicNum,PropertyValue FROM programproperty 
				WHERE ProgramNum={POut.Long(programNum)} AND PropertyDesc='PaySimple Payment Type'";
			DataTable table=Db.GetTable(command);
			foreach(DataRow row in table.Rows) {
				command=$@"INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue,ClinicNum
					) VALUES(
					{POut.Long(programNum)},
					'PaySimple Payment Type ACH',
					'{POut.String(PIn.String(row["PropertyValue"].ToString()))}',
					{POut.Long(PIn.Long(row["ClinicNum"].ToString()))})";
				Db.NonQ(command);
			}
			//Rename 'PaySimple Payment Type' program property to make it clear it's only for credit cards.
			command=$@"UPDATE programproperty SET PropertyDesc='PaySimple Payment Type CC' 
				WHERE ProgramNum={POut.Long(programNum)} AND PropertyDesc='PaySimple Payment Type'";
			Db.NonQ(command);
			//Rename 'RecurringChargesPayType' preference to make it clear it's only for credit cards.
			command=$@"UPDATE preference SET PrefName='RecurringChargesPayTypeCC' WHERE PrefName='RecurringChargesPayType'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ThemeSetByUser','0')";
			Db.NonQ(command);
			//Transworld Adjustment
			command="SELECT ProgramNum FROM program WHERE ProgName='Transworld'";
			long tsiProgNum=Db.GetLong(command);
			command="SELECT DISTINCT ClinicNum FROM programproperty WHERE ProgramNum="+POut.Long(tsiProgNum);//copied from different example, but found
			//that if program is not yet enabled, but using clinics, properties will not get inserted for all clinics if you decide to enable feature later. 
			List<long> listProgClinicNums=Db.GetListLong(command);
			for(int i=0;i < listProgClinicNums.Count;i++) {
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue,ComputerName,ClinicNum) "
					+"VALUES ("+POut.Long(tsiProgNum)+",'SyncExcludePosAdjType','0','',"+POut.Long(listProgClinicNums[i])+"),"
					+"("+POut.Long(tsiProgNum)+",'SyncExcludeNegAdjType','0','',"+POut.Long(listProgClinicNums[i])+")";
				Db.NonQ(command);
			}
			command="ALTER TABLE sheetdef ADD UserNum bigint NOT NULL";//New column UserNum will default to 0.
			Db.NonQ(command);
			command="ALTER TABLE sheetdef ADD INDEX (UserNum)";
			Db.NonQ(command);
			command="ALTER TABLE sheetfielddef ADD LayoutMode tinyint NOT NULL";//New column SheetTypeMode will default to 0 (SheetTypeMode.Default)
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('SendEmailsInDiffProcess','0')";//Default to 'False'
			Db.NonQ(command);
			command="ALTER TABLE tasklist ADD GlobalTaskFilterType tinyint NOT NULL";
			Db.NonQ(command);
			command="UPDATE tasklist SET GlobalTaskFilterType=1";//GlobalTaskFilterType.Default
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('TasksGlobalFilterType','0')";//Require offices to turn this on.
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS payconnectresponseweb";
			Db.NonQ(command);
			command=@"CREATE TABLE payconnectresponseweb (
				PayConnectResponseWebNum bigint NOT NULL auto_increment PRIMARY KEY,
				PatNum bigint NOT NULL,
				PayNum bigint NOT NULL,
				AccountToken varchar(255) NOT NULL,
				PaymentToken varchar(255) NOT NULL,
				ProcessingStatus tinyint NOT NULL,
				DateTimeEntry datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				DateTimePending datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				DateTimeCompleted datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				DateTimeExpired datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				DateTimeLastError datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				LastResponseStr text NOT NULL,
				INDEX(PatNum),
				INDEX(PayNum)
			) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('GlobalUpdateWriteOffLastClinicCompleted','')";
			Db.NonQ(command);
			command="ALTER TABLE paysplit ADD AdjNum bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE paysplit ADD INDEX (AdjNum)";
			Db.NonQ(command);
		}//End of 19_1_1() method

		private static void To19_1_3() {
			string command;
			command="ALTER TABLE repeatcharge ADD UnearnedTypes varchar(4000) NOT NULL";
			Db.NonQ(command);
		}

		private static void To19_1_4() {
			string command;
			command="ALTER TABLE payconnectresponseweb ADD CCSource tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE payconnectresponseweb ADD Amount double NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE payconnectresponseweb ADD PayNote varchar(255) NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE payconnectresponseweb MODIFY ProcessingStatus VARCHAR(255) NOT NULL";
			Db.NonQ(command);
		}

		private static void To19_1_6() {
			string command;
			command="ALTER TABLE sheetdef DROP COLUMN UserNum";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('ChartDefaultLayoutSheetDefNum','0')";
			Db.NonQ(command);
		}

		private static void To19_1_7() {
			string command;
			command="ALTER TABLE laboratory ADD IsHidden tinyint NOT NULL";
			Db.NonQ(command);
		}

		private static void To19_1_9() {
			string command;
			command="UPDATE displayreport SET Description='Monthly Production Goal' WHERE InternalName='ODMonthlyProductionGoal'";
			Db.NonQ(command);
			DataTable table;
			//Add permission to everyone------------------------------------------------------
			command="SELECT DISTINCT UserGroupNum FROM grouppermission";
			table=Db.GetTable(command);
			long groupNum;
			foreach(DataRow row in table.Rows) {
				 groupNum=PIn.Long(row["UserGroupNum"].ToString());
				 command="INSERT INTO grouppermission (UserGroupNum,NewerDays,PermType) "
						+"VALUES("+POut.Long(groupNum)+",1,174)";//174 - NewClaimsProcNotBilled, default lock days to 1 day.
				 Db.NonQ(command);
			}
		}

		private static void To19_1_21() {
			string command;
			command="INSERT INTO preference (PrefName,ValueString) VALUES('CanadaCreatePpoLabEst','1')";//Enabled by default.
			Db.NonQ(command);
		}

		private static void To19_1_23() {
			string command;			
			command="INSERT INTO preference (PrefName,ValueString) VALUES('PatientSelectSearchMinChars','1')";//1 char by default, to maintain current behavior
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('PatientSelectSearchPauseMs','1')";//1 ms by default, to maintain current behavior
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('PatientSelectSearchWithEmptyParams','0')";//0 for Unknown (YN enum), to maintain current behavior
			Db.NonQ(command);
		}
		private static void To19_1_25() {
			string command;
			command="INSERT INTO alertcategorylink (AlertCategoryNum,AlertType) VALUES (1,21)";//eservices, WebMailRecieved
			Db.NonQ(command);
		}

		private static void To19_1_31() {
			string command;
			command="INSERT INTO preference (PrefName,ValueString) VALUES('EnterpriseNoneApptViewDefaultDisabled','0')";//Default to false
			Db.NonQ(command);
		}

		private static void To19_1_34() {
			string command;
			command=$"INSERT INTO preference (PrefName,ValueString) VALUES('WebSchedRecallApptSearchAfterDays','')";//Default to no days
			Db.NonQ(command);
		}

		private static void To19_2_1() {
			string command;
			DataTable table;
			//TSI - ArManager Excluded default preferences - The values are set to the unsent tab defaults
			command="INSERT INTO preference(PrefName,ValueString) "
				+"SELECT 'ArManagerExcludedExcludeBadAddresses',ValueString FROM preference WHERE PrefName='ArManagerExcludeBadAddresses'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) "
				+"SELECT 'ArManagerExcludedExcludeIfUnsentProcs',ValueString FROM preference WHERE PrefName='ArManagerExcludeIfUnsentProcs'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) "
				+"SELECT 'ArManagerExcludedExcludeInsPending',ValueString FROM preference WHERE PrefName='ArManagerExcludeInsPending'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) "
				+"SELECT 'ArManagerExcludedDaysSinceLastPay',ValueString FROM preference WHERE PrefName='ArManagerUnsentDaysSinceLastPay'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) "
				+"SELECT 'ArManagerExcludedMinBal',ValueString FROM preference WHERE PrefName='ArManagerUnsentMinBal'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) "
				+"SELECT 'ArManagerExcludedAgeOfAccount',ValueString FROM preference WHERE PrefName='ArManagerUnsentAgeOfAccount'";
			Db.NonQ(command);
			//Add the new AccountingCashPaymentType preference
			string payTypeDescript="Cash";
			long defNum=0;
			command=$"SELECT DefNum,IsHidden FROM definition WHERE Category=10 AND ItemName='{POut.String(payTypeDescript)}'";//10 is DefCat.PaymentTypes
			table=Db.GetTable(command);
			if(table.Rows.Count==0) { //The cash payment type does not exist, add it
				command="SELECT COUNT(*) FROM definition WHERE Category=10";//10 is DefCat.PaymentTypes
				int countCats=PIn.Int(Db.GetCount(command));
				command=$"INSERT INTO definition (Category,ItemName,ItemOrder) VALUES (10,'{POut.String(payTypeDescript)}',{POut.Int(countCats)})";//10 is DefCat.PaymentTypes
				defNum=Db.NonQ(command,true);
			}
			else { //The cash payment type already exists, get the existing DefNum
				defNum=PIn.Long(table.Rows[0]["DefNum"].ToString());
				if(PIn.Bool(table.Rows[0]["IsHidden"].ToString())) {
					//Unhide if Cash payment type is hidden.
					command=$"UPDATE definition SET IsHidden=0 WHERE DefNum={POut.Long(defNum)} AND Category=10";
					Db.NonQ(command);
				}
			}
			command=$"INSERT INTO preference (PrefName,ValueString) VALUES('AccountingCashPaymentType','{POut.Long(defNum)}')";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('ApptModuleUses2019Overhaul','0')";
			Db.NonQ(command);
			command="ALTER TABLE userod ADD MobileWebPin varchar(255) NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE userod ADD MobileWebPinFailedAttempts tinyint unsigned NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE proctp ADD ProvNum bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE proctp ADD INDEX (ProvNum)";
			Db.NonQ(command);
			command="ALTER TABLE proctp ADD DateTP date NOT NULL DEFAULT '0001-01-01'";
			Db.NonQ(command);
			command="ALTER TABLE proctp ADD ClinicNum bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE proctp ADD INDEX (ClinicNum)";
			Db.NonQ(command);
			command="ALTER TABLE carrier ADD TrustedEtransFlags tinyint NOT NULL";
			Db.NonQ(command);
			//Convert plain text email templates into HTML templates.
			command=@"SELECT PrefName,ValueString FROM preference 
				WHERE PrefName IN('WebSchedMessage','WebSchedMessage2','WebSchedMessage3','WebSchedAggregatedEmailBody','WebSchedVerifyRecallEmailBody',
					'WebSchedVerifyNewPatEmailBody','WebSchedVerifyASAPEmailBody','WebSchedAsapEmailTemplate','PatientPortalNotifyBody')";
			table=Db.GetTable(command);
			command="SELECT ValueString FROM preference WHERE PrefName='EmailDisclaimerIsOn'";
			bool isEmailDisclaimerOn=(Db.GetScalar(command)=="1");
			string ConvertEmailTemplate(string template) {
				return template
					.Replace(">","&>")
					.Replace("<","&<")
					+(isEmailDisclaimerOn ? "\r\n\r\n\r\n[EmailDisclaimer]" : "");
			}
			foreach(DataRow row in table.Rows) {
				string prefName=PIn.String(row["PrefName"].ToString());
				string valueString=ConvertEmailTemplate(PIn.String(row["ValueString"].ToString()))
					.Replace("[OfficePhone]","<a href=\"tel:[OfficePhone]\">[OfficePhone]</a>");
				switch(prefName) {
					case "WebSchedMessage":
					case "WebSchedMessage2":
					case "WebSchedMessage3":
					case "PatientPortalNotifyBody":
						valueString=valueString.Replace("[URL]","<a href=\"[URL]\">[URL]</a>");
						break;
					case "WebSchedAsapEmailTemplate":
						valueString=valueString.Replace("[AsapURL]","<a href=\"[AsapURL]\">[AsapURL]</a>");
						break;
				}
				command=$"UPDATE preference SET ValueString='{POut.String(valueString)}' WHERE PrefName='{POut.String(prefName)}'";
				Db.NonQ(command);
			}
			command=@"SELECT ClinicNum,PrefName,ValueString FROM clinicpref 
				WHERE PrefName IN('WebSchedVerifyRecallEmailBody','WebSchedVerifyNewPatEmailBody','WebSchedVerifyASAPEmailBody','WebSchedAsapEmailTemplate')";
			table=Db.GetTable(command);
			foreach(DataRow row in table.Rows) {
				long clinicNum=PIn.Long(row["ClinicNum"].ToString());
				string prefName=PIn.String(row["PrefName"].ToString());
				string valueString=ConvertEmailTemplate(PIn.String(row["ValueString"].ToString()))
					.Replace("[OfficePhone]","<a href=\"tel:[OfficePhone]\">[OfficePhone]</a>");
				switch(prefName) {
					case "WebSchedAsapEmailTemplate":
						valueString=valueString.Replace("[AsapURL]","<a href=\"[AsapURL]\">[AsapURL]</a>");
						break;
				}
				command=$@"UPDATE clinicpref SET ValueString='{POut.String(valueString)}' WHERE PrefName='{POut.String(prefName)}' 
					AND ClinicNum={POut.Long(clinicNum)}";
				Db.NonQ(command);
			}
			//Previously www.open-dent.com/updates/ redirected to www.opendental.com/updates/. Since we want to use HTTPS now, we will simply go directly
			//to www.opendental.com.
			command="UPDATE preference SET ValueString='https://www.opendental.com/updates/' WHERE PrefName='UpdateWebsitePath' "
				+"AND ValueString='http://www.open-dent.com/updates/'";
			Db.NonQ(command);
			command="ALTER TABLE payment ADD ExternalId varchar(255) NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE payment ADD PaymentStatus tinyint NOT NULL";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ArchiveDoBackupFirst','1')";//Default to 'True'
			Db.NonQ(command);
			command="ALTER TABLE emailtemplate CHANGE IsHtml TemplateType tinyint(4) NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE emailmessage CHANGE IsHtml HtmlType tinyint(4) NOT NULL";
			Db.NonQ(command);
			//eClipboad Prefs
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EClipboardCreateMissingFormsOnCheckIn','1')";//Default to 'True'
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EClipboardAllowSelfCheckIn','1')";//Default to 'True'
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EClipboardMessageComplete','You have successfully checked in. Please return this device to the front desk.')";//Default to 'False'
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EClipboardPopupKioskOnCheckIn','0')";//Default to 'False'
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EClipboardPresentAvailableFormsOnCheckIn','1')";//Default to 'True'
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EClipboardAllowSelfPortraitOnCheckIn','0')";//Default to 'False'
			Db.NonQ(command);
			//Default to 'True' (A new clinic with no entry in ClinicPref will implicitly have EClipboardUseDefaults==true)
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EClipboardUseDefaults','1')";
			Db.NonQ(command);
			//Set the EClipboardUseDefaults pref to true for all clinics
			command="SELECT ClinicNum FROM clinic";
			List<long> listClinicNums=Db.GetListLong(command);
			if(listClinicNums.Count>0) {
				foreach(long clinicNum in listClinicNums) {
					command="INSERT INTO clinicpref(ClinicNum,PrefName,ValueString) VALUES("+clinicNum+",'EClipboardUseDefaults',1)"; //Default to true
					Db.NonQ(command);
				}
			}
			//add EClipboardSheetDef
			command="DROP TABLE IF EXISTS eclipboardsheetdef";
			Db.NonQ(command);
			command=@"CREATE TABLE eclipboardsheetdef (
				EClipboardSheetDefNum bigint NOT NULL auto_increment PRIMARY KEY,
				SheetDefNum bigint NOT NULL,
				ClinicNum bigint NOT NULL,
				ResubmitInterval bigint NOT NULL,
				ItemOrder int NOT NULL,
				INDEX(SheetDefNum),
				INDEX(ClinicNum),
				INDEX(ResubmitInterval)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			//There was a spelling typo in the FieldName for the X835.TransactionHandlingDescript field for ERA sheet defs.
			//Sheet defs of type ERA are not saved to the database so the only thing that needs to be updated are custom sheet field defs.
			//This typo correction does not need to be backported because the misspelled word does not get printed.  It is only used in the sheet setup.
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1 - ERA SheetFieldDefs");
			command="UPDATE sheetfielddef SET FieldName='TransHandlingDesc' WHERE FieldName='TransHandilingDesc'";
			Db.NonQ(command);
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1");
			//Changes to mobileappdevice
			command="ALTER TABLE mobileappdevice ADD PatNum bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE mobileappdevice ADD INDEX (PatNum)";
			Db.NonQ(command);
			command="ALTER TABLE mobileappdevice ADD LastCheckInActivity datetime NOT NULL DEFAULT '0001-01-01 00:00:00'";
			Db.NonQ(command);
			command="SELECT DefNum FROM definition WHERE Category=38 AND ItemName='ServiceError'";//38 is DefCat.InsuranceVerificationStatus
			DataTable dtDef=Db.GetTable(command);
			int itemOrder;
			if(dtDef.Rows.Count==0) { //Def does not exist
				command="SELECT MAX(ItemOrder)+1 FROM definition WHERE Category=38";//38 is DefCat.InsuranceVerificationStatus
				itemOrder=PIn.Int(Db.GetCount(command));
				command="INSERT INTO definition (Category,ItemName,ItemOrder,ItemValue) "
					+"VALUES (38"+",'ServiceError',"+POut.Int(itemOrder)+",'SE')";//38 is DefCat.InsuranceVerificationStatus
				Db.NonQ(command);
			}
			//Insert PDMP bridge-----------------------------------------------------------------
			command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note" 
				+") VALUES(" 
				+"'PDMP', " 
				+"'PDMP', " 
				+"'0', " 
				+"'', "//Opens website. No local executable.
				+"'', "//leave blank if none 
				+"'')"; 
			long programNum=Db.NonQ(command,true); 
			command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
			   +"VALUES ("
			   +"'"+POut.Long(programNum)+"', "
			   +"'7', "//ToolBarsAvail.MainToolbar
			   +"'PDMP')";
			Db.NonQ(command);
			//Insert Illinois PDMP program properties--------------------------------------------
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				+") VALUES(" 
				+"'"+POut.Long(programNum)+"', " 
				+"'Illinois PDMP FacilityID', " 
				+"'')"; 
			Db.NonQ(command); 
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				+") VALUES(" 
				+"'"+POut.Long(programNum)+"', " 
				+"'Illinois PDMP Username', " 
				+"'')"; 
			Db.NonQ(command); 
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				+") VALUES(" 
				+"'"+POut.Long(programNum)+"', " 
				+"'Illinois PDMP Password', " 
				+"'')"; 
			Db.NonQ(command); 
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EraShowControlIdFilter','0')";//Default to false
			Db.NonQ(command);
			//add ScheduledProcess			
			command="DROP TABLE IF EXISTS scheduledprocess";
			Db.NonQ(command);
			command=@"CREATE TABLE scheduledprocess (
					ScheduledProcessNum bigint NOT NULL auto_increment PRIMARY KEY,
					ScheduledAction varchar(50) NOT NULL,
					TimeToRun datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
					FrequencyToRun varchar(50) NOT NULL,
					LastRanDateTime datetime NOT NULL DEFAULT '0001-01-01 00:00:00'
					) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command=$"INSERT INTO preference (PrefName,ValueString) VALUES('EClipboardClinicsSignedUp','')";
			Db.NonQ(command);
			command="SELECT DISTINCT UserGroupNum FROM grouppermission WHERE PermType=8";//Setup
			table=Db.GetTable(command);
			long groupNum;
			foreach(DataRow row in table.Rows) {
				groupNum=PIn.Long(row["UserGroupNum"].ToString());
				command="INSERT INTO grouppermission (UserGroupNum,PermType) "
					 +"VALUES("+POut.Long(groupNum)+",177)";  //FeatureRequestEdit
				Db.NonQ(command);
			}
			#region TsiTransLogs Excluded Adjustment Type
			command=@"SELECT programproperty.ClinicNum,
				MAX(CASE WHEN programproperty.PropertyDesc='SyncExcludePosAdjType' THEN programproperty.PropertyValue END) SyncExcludePosAdjType,
				MAX(CASE WHEN programproperty.PropertyDesc='SyncExcludeNegAdjType' THEN programproperty.PropertyValue END) SyncExcludeNegAdjType
				FROM programproperty
				INNER JOIN program ON programproperty.ProgramNum=program.ProgramNum AND program.ProgName='Transworld' AND program.Enabled
				WHERE programproperty.PropertyDesc IN('SyncExcludePosAdjType','SyncExcludeNegAdjType')
				GROUP BY programproperty.ClinicNum";
			//dictionary key=ClinicNum, value=string of comma delimited adj type longs SyncExcludePosAdjType and SyncExcludeNegAdjType, i.e. "3,2"
			Dictionary<long,string> dictClinExclAdjTypes=Db.GetTable(command).Select()
				.ToDictionary(x => PIn.Long(x["ClinicNum"].ToString()),
					x => POut.Long(PIn.Long(x["SyncExcludePosAdjType"].ToString()))+","+POut.Long(PIn.Long(x["SyncExcludeNegAdjType"].ToString())));
			if(dictClinExclAdjTypes.Count>0) {
				if(dictClinExclAdjTypes.ContainsKey(0)) {//if HQ clinic has props set, use them for any clinic with no props
					command="SELECT ClinicNum FROM clinic";
					Db.GetListLong(command)
						.FindAll(x => !dictClinExclAdjTypes.ContainsKey(x))
						.ForEach(x => dictClinExclAdjTypes[x]=dictClinExclAdjTypes[0]);
				}
				command=@"SELECT tsitranslog.TsiTransLogNum
					FROM tsitranslog
					INNER JOIN adjustment ON adjustment.AdjNum=tsitranslog.FKey
					WHERE tsitranslog.FKeyType=0 "//TsiFKeyType.Adjustment
					+"AND tsitranslog.TransType=-1 "//TsiTransType.None
					+$@"AND ({
						string.Join(" OR ",dictClinExclAdjTypes
							.GroupBy(x => x.Value,x => x.Key)
							.Select(x => $@"(tsitranslog.ClinicNum IN ({string.Join(",",x.Select(y => POut.Long(y)))}) AND adjustment.AdjType IN ({x.Key}))"))
					})";
				List<long> listLogNumsToUpdate=Db.GetListLong(command);
				if(listLogNumsToUpdate.Count>0) {
					command="UPDATE tsitranslog SET TransType=10 "//TsiTransType.Excluded
						+$@"WHERE TsiTransLogNum IN({string.Join(",",listLogNumsToUpdate)})";
					Db.NonQ(command);
				}
			}
			#endregion TsiTransLogs Excluded Adjustment Type
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ApptPreventChangesToCompleted','0')";//Off by default
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EraAllowTotalPayments','1')";//Default to enabled for backward compatibility
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS loginattempt";
			Db.NonQ(command);
			command=@"CREATE TABLE loginattempt (
					LoginAttemptNum bigint NOT NULL auto_increment PRIMARY KEY,
					UserName varchar(255) NOT NULL,
					LoginType tinyint NOT NULL,
					DateTFail datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
					INDEX(UserName(10))
					) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			//This was incorrectly implemented should've created a new Trojan Express Collect program link instead of enabling the Trojan program link.
			//And it shouldn't have disabled the show feature, it should have left the show feature setting and enabled the program link accordingly.
			//command="SELECT ValueString FROM preference WHERE PrefName='AccountShowTrojanExpressCollect'";
			//if(PIn.Bool(Db.GetScalar(command))) {//They had it enabled, enable the Trojan program link
			//	command="UPDATE program SET Enabled=1 WHERE ProgName='Trojan'";
			//	Db.NonQ(command);
			//	command="UPDATE preference SET ValueString='0' WHERE PrefName='AccountShowTrojanExpressCollect'";
			//	Db.NonQ(command);
			//}
			command="INSERT INTO preference(PrefName,ValueString) VALUES('RecurringChargesAllowedWhenNoPatBal','1')";//Default to 'True'
			Db.NonQ(command);
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1 - Adding CanChargeWhenNoBal to creditcard");
			command="ALTER TABLE creditcard ADD CanChargeWhenNoBal tinyint NOT NULL";
			Db.NonQ(command);
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1 - claim table structure");
			command=@"ALTER TABLE claim ADD DateIllnessInjuryPreg date NOT NULL DEFAULT '0001-01-01',
				ADD DateIllnessInjuryPregQualifier tinyint NOT NULL,
				ADD DateOther date NOT NULL DEFAULT '0001-01-01',
				ADD DateOtherQualifier tinyint NOT NULL,
				ADD IsOutsideLab tinyint NOT NULL,
				ADD ResubmissionCode tinyint NOT NULL";
			Db.NonQ(command);
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1");
			command="INSERT INTO preference(PrefName,ValueString) VALUES('PrePayAllowedForTpProcs','0')";//default to Off. 
			Db.NonQ(command);
			command="SELECT MAX(ItemOrder)+1 FROM definition WHERE Category=29";//29 is PaySplitUnearnedType
			int order=PIn.Int(Db.GetCount(command));
			command="INSERT INTO definition (Category,ItemName,ItemOrder,ItemValue) "
				+"VALUES (29,'Treat Plan Prepayment',"+POut.Int(order)+",'X')";//29 is PaySplitUnearnedType
			defNum=Db.NonQ(command,true);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('TpUnearnedType','"+POut.Long(defNum)+"')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('TpPrePayIsNonRefundable','0')";
			Db.NonQ(command);
			command="SELECT MAX(displayreport.ItemOrder)+1 FROM displayreport WHERE displayreport.Category=3";//DisplayReportCategory.Lists
			itemOrder=Db.GetInt(command);
			command="INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES ('ODHiddenPaySplits',"+POut.Int(itemOrder)
				+",'Hidden Payment Splits',3,0)";//3 is DisplayReportCategory.Lists
			long reportNumNew=Db.NonQ(command,getInsertID:true);
			long reportNum=Db.GetLong("SELECT DisplayReportNum FROM displayreport WHERE InternalName='ODPayments'");
			List<long> listGroupNums=Db.GetListLong("SELECT UserGroupNum FROM grouppermission WHERE PermType=22 AND FKey="+POut.Long(reportNum));
			foreach(long groupNumCur in listGroupNums) {
				command="INSERT INTO grouppermission (NewerDate,NewerDays,UserGroupNum,PermType,FKey) "
				+"VALUES('0001-01-01',0,"+POut.Long(groupNumCur)+",22,"+POut.Long(reportNumNew)+")";
				Db.NonQ(command);
			}
			To19_2_1_LargeTableScripts();//IMPORTANT: Leave the large table scripts at the end of To19_2_0.
		}//End of 19_2_1() method

		private static void To19_2_1_LargeTableScripts() {
			//We want it to be the last thing done so that if it fails for a large customer and we have to manually finish the update script we will only
			//have to verify that these large table scripts have run.
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1 - claimproc table structure");//No translation in convert script.
			//If adding an index with the same name as an existing index, but with different column(s), the existing index will be dropped before adding the
			//new one.  If an index exists that has the same column(s) as an index being added, the index won't be added again regardless of the index names
			LargeTableHelper.AlterLargeTable("claimproc","ClaimProcNum",null,
				new List<Tuple<string,string>> {
					Tuple.Create("ClaimNum,ClaimPaymentNum,InsPayAmt,DateCP","indexOutClaimCovering"),
					Tuple.Create("Status,PatNum,DateCP,PayPlanNum,InsPayAmt,WriteOff,InsPayEst,ProcDate,ProcNum","indexAgingCovering")
				});
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1 - securitylog table structure");//No translation in convert script.
			LargeTableHelper.AlterLargeTable("securitylog","SecurityLogNum",null,
				new List<Tuple<string, string>> { Tuple.Create("PermType","") });//no need to send index name. only adds index if not exists
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1 - benefit table structure");//No translation in convert script.
			LargeTableHelper.AlterLargeTable("benefit","BenefitNum",
				new List<Tuple<string,string>> {
					Tuple.Create("SecDateTEntry","datetime NOT NULL DEFAULT '0001-01-01 00:00:00'"),
					Tuple.Create("SecDateTEdit","timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
				},
				new List<Tuple<string,string>> { Tuple.Create("SecDateTEntry",""),Tuple.Create("SecDateTEdit","") });//no need to send index name
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1 - insverify table structure");//No translation in convert script.
			LargeTableHelper.AlterLargeTable("insverify","InsVerifyNum",
				new List<Tuple<string,string>> { Tuple.Create("SecDateTEdit","timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP") },
				new List<Tuple<string,string>> { Tuple.Create("SecDateTEdit","") });//no need to send index name
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1 - insverifyhist table structure");//No translation in convert script.
			LargeTableHelper.AlterLargeTable("insverifyhist","InsVerifyHistNum",
				new List<Tuple<string,string>> { Tuple.Create("SecDateTEdit","timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP") },
				new List<Tuple<string,string>> { Tuple.Create("SecDateTEdit","") });//no need to send index name
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1 - patientnote table structure");//No translation in convert script.
			LargeTableHelper.AlterLargeTable("patientnote","PatNum",
				new List<Tuple<string,string>> {
					Tuple.Create("SecDateTEntry","datetime NOT NULL DEFAULT '0001-01-01 00:00:00'"),
					Tuple.Create("SecDateTEdit","timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
				},
				new List<Tuple<string,string>> { Tuple.Create("SecDateTEntry",""),Tuple.Create("SecDateTEdit","") });//no need to send index name
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1 - task table structure");//No translation in convert script.
			LargeTableHelper.AlterLargeTable("task","TaskNum",
				new List<Tuple<string,string>> { Tuple.Create("SecDateTEdit","timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP") },
				new List<Tuple<string,string>> { Tuple.Create("SecDateTEdit","") });//no need to send index name
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1 - taskhist table structure");//No translation in convert script.
			LargeTableHelper.AlterLargeTable("taskhist","TaskHistNum",
				new List<Tuple<string,string>> { Tuple.Create("SecDateTEdit","timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP") },
				new List<Tuple<string,string>> { Tuple.Create("SecDateTEdit","") });//no need to send index name
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1 - procedurelog table structure");//No translation in convert script.
			LargeTableHelper.AlterLargeTable("procedurelog","ProcNum",new List<Tuple<string,string>>{ Tuple.Create("Urgency","tinyint NOT NULL") });
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.2.1");//No translation in convert script.
		}

		private static void To19_2_2() {
			string command;
			DataTable table;
			//XDR locationID for headquarters, LocationIDs for other clinics will be generated through the UI
			command="SELECT ProgramNum FROM program WHERE ProgName='XDR'";
			long programNum=Db.GetLong(command);
			command=$@"INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue) VALUES(
				'{POut.Long(programNum)}', 
				'Location ID', 
				'')";
			Db.NonQ(command);
			command="SELECT ValueString FROM preference WHERE PrefName='RigorousAccounting'";
			int intValue=Db.GetInt(command);//Rigorous accounting has three selection options Don't Enforce(2), AutoSplitOnly(1), EnforceFully(0)
			if(intValue==1 || intValue==2) {//if 1 or 2, prepayments to providers were always allowed. 
				command="UPDATE preference SET ValueString='1' WHERE PrefName='AllowPrepayProvider'";
				Db.NonQ(command);
			}
			command="ALTER TABLE claim MODIFY DateIllnessInjuryPregQualifier smallint NOT NULL,MODIFY DateOtherQualifier smallint NOT NULL";
			Db.NonQ(command);
		}

		private static void To19_2_3() {
			string command;
			DataTable table;
			#region HQ Only
			//We are running this section of code for HQ only
			//This is very uncommon and normally manual queries should be run instead of doing a convert script.
			command="SELECT ValueString FROM preference WHERE PrefName='DockPhonePanelShow'";
			table=Db.GetTable(command);
			if(table.Rows.Count > 0 && PIn.Bool(table.Rows[0][0].ToString())) {
				//Add new columns
				command="ALTER TABLE job ADD DateTimeConceptApproval datetime NOT NULL DEFAULT '0001-01-01 00:00:00'";
				Db.NonQ(command);
				command="ALTER TABLE job ADD DateTimeJobApproval datetime NOT NULL DEFAULT '0001-01-01 00:00:00'";
				Db.NonQ(command);
				command="ALTER TABLE job ADD DateTimeImplemented datetime NOT NULL DEFAULT '0001-01-01 00:00:00'";
				Db.NonQ(command);
				//Use JobLog rows to update correct info
				command=@"UPDATE job j 
									INNER JOIN(
										SELECT jl.JobNum,MAX(jl.DateTimeEntry) SetDate
										FROM JobLog jl
										WHERE jl.Description LIKE '%Concept approved.%'
										GROUP BY jl.JobNum) AS jl2 ON j.JobNum=jl2.JobNum
									SET DateTimeConceptApproval=jl2.SetDate";
				Db.NonQ(command);
				command=@"UPDATE job j 
									INNER JOIN (
										SELECT jl.JobNum,MAX(jl.DateTimeEntry) SetDate
										FROM JobLog jl
										WHERE jl.Description LIKE '%Job approved.%' OR jl.Description LIKE '%Changes approved.%'
										GROUP BY jl.JobNum) AS jl2 ON j.JobNum=jl2.JobNum
									SET DateTimeJobApproval=jl2.SetDate";
				Db.NonQ(command);
				command=@"UPDATE job j 
									INNER JOIN (
										SELECT jl.JobNum,MAX(jl.DateTimeEntry) SetDate
										FROM JobLog jl
										WHERE jl.Description LIKE '%Job implemented.%' OR jl.Description LIKE '%to Complete.%'
										GROUP BY jl.JobNum) AS jl2 ON j.JobNum=jl2.JobNum
									SET DateTimeImplemented=jl2.SetDate";
				Db.NonQ(command);
			}
			#endregion
			//Change the type of sheetfielddef.UiLabelMobile from varchar to text
			LargeTableHelper.AlterLargeTable("sheetfielddef","SheetFieldDefNum",new List<Tuple<string, string>>() { 
				Tuple.Create("UiLabelMobile","text NOT NULL"),
				Tuple.Create("UiLabelMobileRadioButton","text NOT NULL")
			});
			LargeTableHelper.AlterLargeTable("sheetfield","SheetFieldNum",new List<Tuple<string, string>>() { 
				Tuple.Create("UiLabelMobile","text NOT NULL"),
				Tuple.Create("UiLabelMobileRadioButton","text NOT NULL")
			});
		}

		private static void To19_2_4() {
			string command;
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EmailAlertMaxConsecutiveFails','3')";
			Db.NonQ(command);
			command="SELECT AlertCategoryNum FROM alertcategory WHERE InternalName='OdAllTypes' AND IsHQCategory=1";
			long alertCategoryNum=Db.GetLong(command);
			//22 for alerttype EconnectorEmailTooManySendFails
			command="INSERT INTO alertcategorylink(AlertCategoryNum,AlertType) VALUES("+POut.Long(alertCategoryNum)+",'22')";
			Db.NonQ(command);
			command="SELECT AlertCategoryNum FROM alertcategory WHERE InternalName='eServices' AND IsHQCategory=1";
			alertCategoryNum=Db.GetLong(command);
			//22 for alerttype EconnectorEmailTooManySendFails
			command="INSERT INTO alertcategorylink(AlertCategoryNum,AlertType) VALUES("+POut.Long(alertCategoryNum)+",'22')";
			Db.NonQ(command);
			command="ALTER TABLE insverify ADD INDEX (DateTimeEntry)";
			Db.NonQ(command);
			command="ALTER TABLE task ADD INDEX (DateTimeOriginal)";
			Db.NonQ(command);
			command="ALTER TABLE taskhist ADD INDEX (DateTStamp)";
			Db.NonQ(command);
		}

		private static void To19_2_7() {
			long chartDefaultDefNum=PIn.Long(Db.GetScalar("SELECT ValueString FROM preference WHERE PrefName='ChartDefaultLayoutSheetDefNum'"),false);
			Db.NonQ("DELETE FROM userodpref WHERE FKeyType=18 AND Fkey="+POut.Long(chartDefaultDefNum));//18 => DynamicChartLayout
		}

		private static void To19_2_8() {
			string command="INSERT INTO preference(PrefName,ValueString) VALUES('OpenDentalServiceComputerName','')";
			Db.NonQ(command);
			command="UPDATE claim SET CorrectionType=1 WHERE ResubmissionCode=7 AND CorrectionType=0";//CorrectionType 1 and ResubmissionCode 7 are 'Replacement'
			Db.NonQ(command);
			command="UPDATE claim SET CorrectionType=2 WHERE ResubmissionCode=8 AND CorrectionType=0";//CorrectionType 2 and ResubmissionCode 8 are 'Void'
			Db.NonQ(command);
			command="UPDATE claimformitem SET FieldName='CorrectionType' WHERE FieldName='ResubmissionCode'";
			Db.NonQ(command);
			command="ALTER TABLE claim DROP COLUMN ResubmissionCode";
			Db.NonQ(command);
		}

		private static void To19_2_15() {
			string command="INSERT INTO preference(PrefName,ValueString) VALUES('AgingProcLifo','0')";
			Db.NonQ(command);//Unset by default (same effect as Off for now, for backwards compatibility).
			command=$@"SELECT c.TABLE_NAME tableName,c.COLUMN_NAME colName,c1.priKey
				FROM information_schema.COLUMNS c
				INNER JOIN (
					SELECT TABLE_NAME,MAX(COLUMN_NAME) priKey
					FROM information_schema.COLUMNS
					WHERE TABLE_SCHEMA=DATABASE()
					AND TABLE_NAME IN ({string.Join(",",_listTableNames.Concat(_listLargeTableNames).Select(x => "'"+POut.String(x)+"'"))})
					AND COLUMN_KEY='PRI'
					GROUP BY TABLE_NAME
				) c1 ON c1.TABLE_NAME=c.TABLE_NAME
				WHERE c.TABLE_SCHEMA=DATABASE()
				AND c.DATA_TYPE='timestamp'
				AND c.TABLE_NAME IN ({string.Join(",",_listTableNames.Concat(_listLargeTableNames).Select(x => "'"+POut.String(x)+"'"))})
				AND (
					c.IS_NULLABLE='YES'
					OR (c.COLUMN_DEFAULT!='CURRENT_TIMESTAMP' AND c.COLUMN_DEFAULT!='CURRENT_TIMESTAMP()')
					OR (c.EXTRA!='ON UPDATE CURRENT_TIMESTAMP' AND c.EXTRA!='ON UPDATE CURRENT_TIMESTAMP()')
				)";
			DataTable table=Db.GetTable(command);
			string tableName;
			string columnName;
			foreach(DataRow row in table.Rows) {
				tableName=PIn.String(row["tableName"].ToString()).ToLower();
				columnName=PIn.String(row["colName"].ToString());
				if(_listLargeTableNames.Contains(tableName)) {
					LargeTableHelper.AlterLargeTable(tableName,PIn.String(row["priKey"].ToString()),
						new List<Tuple<string,string>> { Tuple.Create(columnName,"timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP") });
				}
				else if(_listTableNames.Contains(tableName)) {
					command=$@"ALTER TABLE `{tableName}` MODIFY `{columnName}` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP";
					Db.NonQ(command);
				}
			}
		}

		private static void To19_2_29() {
			string command;
			#region Move Trojan Express Collect to Program Link
			command="SELECT ValueString FROM preference WHERE PrefName='TrojanExpressCollectPath'";
			string trojanPath=PIn.String(Db.GetScalar(command));
			command="SELECT ValueString FROM preference WHERE PrefName='TrojanExpressCollectBillingType'";
			long trojanBillType=PIn.Long(Db.GetScalar(command));
			command="SELECT ValueString FROM preference WHERE PrefName='TrojanExpressCollectPassword'";
			string trojanPwd=PIn.String(Db.GetScalar(command));
			command="SELECT ValueString FROM preference WHERE PrefName='TrojanExpressCollectPreviousFileNumber'";
			int trojanPrevFileNum=PIn.Int(Db.GetScalar(command));
			command="SELECT ValueString FROM preference WHERE PrefName='AccountShowTrojanExpressCollect'";
			bool isTrojanEnabled=PIn.Bool(Db.GetScalar(command));
			if(!isTrojanEnabled) {
				//The convert script for 19.2.1.0 set the show feature pref to disabled, so if they had previously updated to 19.2.1.0 or above we need to
				//check for values in the other prefs to determine whether or not to enable this program link.
				command="SELECT ProgramVersion FROM updatehistory ORDER BY DateTimeUpdated DESC LIMIT 1";
				string programVersion=Db.GetScalar(command);
				if(string.IsNullOrEmpty(programVersion) || new Version(programVersion)>=new Version(19,2,1,0)) {
					isTrojanEnabled=trojanBillType>0 || trojanPrevFileNum>0 || !string.IsNullOrEmpty(trojanPath) || !string.IsNullOrEmpty(trojanPwd);
				}
			}
			//Create the Trojan Express Collect program using the existing show feature prefs
			command=$@"INSERT INTO program (ProgName,ProgDesc,Enabled,Note)
				VALUES('TrojanExpressCollect','Trojan Express Collect',{POut.Bool(isTrojanEnabled)},'')"; 
			long programNum=Db.NonQ(command,true);
			//Trojan Express Collect Program Property - Folder Path
			command=$@"INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue)
				VALUES({POut.Long(programNum)},'FolderPath','{POut.String(trojanPath)}')"; 
			Db.NonQ(command);
			//Trojan Express Collect Program Property - Billing Type
			command=$@"INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue)
				VALUES({POut.Long(programNum)},'BillingType','{POut.Long(trojanBillType)}')"; 
			Db.NonQ(command);
			//Trojan Express Collect Program Property - Password
			command=$@"INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue)
				VALUES({POut.Long(programNum)},'Password','{POut.String(trojanPwd)}')"; 
			Db.NonQ(command);
			//Trojan Express Collect Program Property - Previous File Number
			command=$@"INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue)
				VALUES({POut.Long(programNum)},'PreviousFileNumber','{POut.Int(trojanPrevFileNum)}')"; 
			Db.NonQ(command);
			command=$@"INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText)
				VALUES ({POut.Long(programNum)},0,'TrojanCollect')";//ToolBarsAvail.AccountModule
			Db.NonQ(command);
			#endregion Move Trojan Express Collect to Program Link
		}

		private static void To19_2_36() {
			//Add hidden pref UseAlternateOpenFileDialogWindow if it hasn't been added.
			string command = "SELECT * FROM preference WHERE preference.PrefName = 'UseAlternateOpenFileDialogWindow'";
			if(Db.GetTable(command).Rows.Count == 0) {
				command="INSERT INTO preference(PrefName,ValueString) VALUES('UseAlternateOpenFileDialogWindow','0')"; //Default to false.
				Db.NonQ(command);
			}
		}

		private static void To19_2_42() {
			LargeTableHelper.AlterLargeTable("claimproc","ClaimProcNum",
					new List<Tuple<string,string>> { Tuple.Create("IsTransfer","tinyint NOT NULL") });
		}

		private static void To19_2_57() {
			//There was a 19.2.57 method backported to the 19.2 solution but it doesn't actually do anything so preserving the code isn't important.
			//This method stub is here as an indicator that there was in fact a 19.2.57 convert script method.
			//What this convert script method was supposed to do can be found in the 19.3.33 convert script method.
		}

		private static void To19_2_62() {
			DetachTransferClaimProcsFromClaimPayments();
		}

		private static void To19_3_1() {
			string command;
			DataTable table;
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.3.1 - SMS Short Codes");
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ShortCodeEConfirmationTemplate','')";//Default to blank
			Db.NonQ(command);
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.3.1");
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ApptsAllowOverlap','0')";
			Db.NonQ(command);
			command="UPDATE preference SET ValueString='1' WHERE PrefName='ApptModuleUses2019Overhaul'";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('PatientPhoneUsePhonenumberTable','0')";//0 for Unknown (YN enum), behaves the same as No
			Db.NonQ(command);
			command=@"ALTER TABLE phonenumber ADD PhoneNumberDigits varchar(30) NOT NULL,
				ADD PhoneType tinyint NOT NULL,
				ADD INDEX PatPhoneDigits (PatNum,PhoneNumberDigits)";
			Db.NonQ(command);
			command="ALTER TABLE apptview ADD WidthOpMinimum smallint unsigned NOT NULL";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('ApptFontSize','8')";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('ApptProvbarWidth','11')";
			Db.NonQ(command);
			LargeTableHelper.AlterLargeTable("appointment","AptNum",
				new List<Tuple<string,string>> { Tuple.Create("ProvBarText","varchar(60) NOT NULL") });
			LargeTableHelper.AlterLargeTable("histappointment","HistApptNum",
				new List<Tuple<string,string>> { Tuple.Create("ProvBarText","varchar(60) NOT NULL") });
			command="ALTER TABLE rxpat ADD INDEX (ProvNum)";
			Db.NonQ(command);
			command="ALTER TABLE orthocharttablink ADD ColumnWidthOverride int NOT NULL";
			Db.NonQ(command);
			if(!LargeTableHelper.IndexExists("task","TaskStatus")) {
				command="ALTER TABLE task ADD INDEX (TaskStatus)";
				Db.NonQ(command);
			}
			command="DROP TABLE IF EXISTS orthocase";
			Db.NonQ(command);
			command=@"CREATE TABLE orthocase (
				OrthoCaseNum bigint NOT NULL auto_increment PRIMARY KEY,
				PatNum bigint NOT NULL,
				ProvNum bigint NOT NULL,
				ClinicNum bigint NOT NULL,
				Fee double NOT NULL,
				FeeIns double NOT NULL,
				FeePat double NOT NULL,
				BandingDate date NOT NULL DEFAULT '0001-01-01',
				DebondDate date NOT NULL DEFAULT '0001-01-01',
				DebondDateExpected date NOT NULL DEFAULT '0001-01-01',
				IsTransfer tinyint NOT NULL,
				OrthoType bigint NOT NULL,
				SecDateTEntry datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				SecUserNumEntry bigint NOT NULL,
				SecDateTEdit timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
				INDEX(PatNum),
				INDEX(ProvNum),
				INDEX(ClinicNum),
				INDEX(OrthoType),
				INDEX(SecUserNumEntry)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS orthoplanlink";
			Db.NonQ(command);
			command=@"CREATE TABLE orthoplanlink (
				OrthoPlanLinkNum bigint NOT NULL auto_increment PRIMARY KEY,
				OrthoCaseNum bigint NOT NULL,
				LinkType tinyint NOT NULL,
				FKey bigint NOT NULL,
				IsActive tinyint NOT NULL,
				SecDateTEntry datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				SecUserNumEntry date NOT NULL DEFAULT '0001-01-01',
				INDEX(OrthoCaseNum),
				INDEX(FKey)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS orthoproclink";
			Db.NonQ(command);
			command=@"CREATE TABLE orthoproclink (
				OrthoProcLinkNum bigint NOT NULL auto_increment PRIMARY KEY,
				OrthoCaseNum bigint NOT NULL,
				ProcNum bigint NOT NULL,
				SecDateTEntry datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				SecUserNumEntry bigint NOT NULL,
				INDEX(OrthoCaseNum),
				INDEX(ProcNum),
				INDEX(SecUserNumEntry)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS orthoschedule";
			Db.NonQ(command);
			command=@"CREATE TABLE orthoschedule (
				OrthoScheduleNum bigint NOT NULL auto_increment PRIMARY KEY,
				BandingDateOverride date NOT NULL DEFAULT '0001-01-01',
				DebondDateOverride date NOT NULL DEFAULT '0001-01-01',
				BandingPercent double NOT NULL,
				VisitPercent double NOT NULL,
				DebondPercent double NOT NULL,
				IsActive tinyint NOT NULL,
				SecDateTEdit timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command="ALTER TABLE payplan ADD IsDynamic tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE payplancharge ADD FKey bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE payplancharge ADD INDEX (FKey)";
			Db.NonQ(command);
			command="ALTER TABLE payplancharge ADD LinkType tinyint NOT NULL";
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS payplanlink";
			Db.NonQ(command);
			command=@"CREATE TABLE payplanlink (
				PayPlanLinkNum bigint NOT NULL auto_increment PRIMARY KEY,
				PayPlanNum bigint NOT NULL,
				LinkType tinyint NOT NULL,
				FKey bigint NOT NULL,
				AmountOverride double NOT NULL,
				SecDateTEntry datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				INDEX(PayPlanNum),
				INDEX(FKey)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command="ALTER TABLE tasklist ADD TaskListStatus tinyint NOT NULL";//Default to 0 (Active).
			Db.NonQ(command);
			command="SELECT ProgramNum FROM program WHERE ProgName = 'XVWeb'";
			long xvWebProgramNum=PIn.Long(Db.GetScalar(command));
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
				+") VALUES("
				+"'"+POut.Long(xvWebProgramNum)+"', "//xvweb
				+"'Image Quality', "
				+"'Moderate')";//Default to Moderate
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ODMobileCacheDurationHours','6')";//Defaults to 6 hours.
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EConnectorSmsNotificationFrequency','5')";//Defaults to 5 seconds.
			Db.NonQ(command);
			command="ALTER TABLE signalod ADD INDEX (IType)";
			Db.NonQ(command);
			command="ALTER TABLE smsfrommobile ADD SecDateTEdit timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP";
			Db.NonQ(command);
			//Exising rows set to DateTime the message was inserted into the DB.
			command="UPDATE smsfrommobile SET SecDateTEdit = DateTimeReceived";
			Db.NonQ(command);
			command="ALTER TABLE smsfrommobile ADD INDEX (SecDateTEdit)";
			Db.NonQ(command);
			command="ALTER TABLE smstomobile ADD SecDateTEdit timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP";
			Db.NonQ(command);
			//Exising rows set to DateTime the message was either successfully delivered or failed.
			command="UPDATE smstomobile SET SecDateTEdit = DateTimeTerminated";
			Db.NonQ(command);
			command="ALTER TABLE smstomobile ADD INDEX (SecDateTEdit)";
			Db.NonQ(command);
			if(!LargeTableHelper.IndexExists("refattach","ReferralNum")) {
				command="ALTER TABLE refattach ADD INDEX (ReferralNum)";
				Db.NonQ(command);
			}
			if(!LargeTableHelper.IndexExists("sheetfield","SheetNum,FieldType")) {
				LargeTableHelper.AlterLargeTable("sheetfield","SheetFieldNum",null,indexColsAndNames:new List<Tuple<string,string>> {
					Tuple.Create("SheetNum,FieldType","SheetNumFieldType")
				});
			}
			LargeTableHelper.AlterLargeTable("patient","PatNum",new List<Tuple<string,string>> { Tuple.Create("HasSignedTil","tinyint NOT NULL") });
			command="SELECT ValueString FROM preference WHERE PrefName='BackupReminderLastDateRun'";
			DateTime.TryParse(Db.GetScalar(command),out DateTime dateBackupReminderLastRun);
			command="SELECT ValueString FROM preference WHERE PrefName='UpdateStreamLinePassword'";
			DataTable tableStreamline=Db.GetTable(command);
			bool isStreamline=false;
			if(tableStreamline.Rows.Count > 0) {
				isStreamline=(tableStreamline.Rows[0]["ValueString"].ToString()=="abracadabra");
			}
			command="INSERT INTO preference (PrefName,ValueString) VALUES('SupplementalBackupEnabled',"
				+((isStreamline || dateBackupReminderLastRun > DateTime.Today.AddYears(10))?"'0'":"'1'")+")";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('SupplementalBackupDateLastComplete','0001-01-01 00:00:00')";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('SupplementalBackupNetworkPath','')";
			Db.NonQ(command);
			command="ALTER TABLE registrationkey ADD DateTBackupScheduled datetime NOT NULL DEFAULT '0001-01-01 00:00:00'";
			Db.NonQ(command);
			command="ALTER TABLE registrationkey ADD BackupPassCode varchar(32) NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE userod ADD DateTLastLogin datetime NOT NULL DEFAULT '0001-01-01 00:00:00'";
			Db.NonQ(command);
			command="ALTER TABLE payplan ADD ChargeFrequency tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE paysplit ADD PayPlanChargeNum bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE paysplit ADD INDEX (PayPlanChargeNum)";
			Db.NonQ(command);
			command="ALTER TABLE payplan ADD DatePayPlanStart date NOT NULL DEFAULT '0001-01-01'";
			Db.NonQ(command);
			command="ALTER TABLE sheetdef ADD DateTCreated datetime NOT NULL DEFAULT '0001-01-01 00:00:00'";
			Db.NonQ(command);
			command="UPDATE preference SET PrefName='SheetsDefaultRxInstruction' WHERE PrefName='SheetsDefaultRxInstructions'";
			Db.NonQ(command);
			command="UPDATE preference SET PrefName='SheetsDefaultChartModule' WHERE PrefName='ChartDefaultLayoutSheetDefNum'";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('DynamicPayPlanRunTime','2019-01-01 09:00:00')";//9 AM
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('DynamicPayPlanLastDateTime','0001-01-01 00:00:00')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('DynamicPayPlanStartDateTime','')";
			Db.NonQ(command);
			command="ALTER TABLE payplan ADD IsLocked tinyint NOT NULL";
			Db.NonQ(command);
		}//End of 19_3_1() method

		private static void To19_3_3() {
			//Rename the prog name to the correct name. It was misspelled when it was initially created. 
			string command="UPDATE program SET ProgName='PandaPerioAdvanced' WHERE ProgName='PandaPeriodAdvanced'";
			Db.NonQ(command);
			command="UPDATE toolbutitem SET ButtonText='PandaPerioAdvanced' WHERE ButtonText='PandaPeriodAdvanced'";
			Db.NonQ(command);
		}

		private static void To19_3_9() {
			string command="";
			#region Theme Conversion
			//Convert preference ColorTheme for theme changes. See conversion below.
			//Original(0) and Blue(1) will be converted to Gradient(0)
			//MonoFlatGray(2) and MonoFlatBlue(3) will be converted to Flat(1)
			command="SELECT ValueString FROM preference WHERE PrefName='ColorTheme'";
			long curThemeVal=Db.GetLong(command);
			int newThemeVal=(curThemeVal>1) ? 1 : 0;
			command=$"UPDATE preference SET ValueString='{POut.Int(newThemeVal)}' WHERE PrefName='ColorTheme'";
			Db.NonQ(command);
			//Convert UserTheme userodpref's to work with theme changes.
			command="SELECT * FROM userodpref WHERE FkeyType=17";//17=UserTheme
			DataTable userThemePrefs=Db.GetTable(command);
			foreach(DataRow row in userThemePrefs.Rows) {
				long priKey=PIn.Long(row["UserOdPrefNum"].ToString());
				int themeVal=PIn.Int(row["FKey"].ToString());
				int newVal=(themeVal>1) ? 1 : 0;
				Db.NonQ($"UPDATE userodpref SET Fkey={newVal} WHERE UserOdPrefNum={priKey}");
			}
			#endregion
			command=$"INSERT INTO preference (PrefName,ValueString) VALUES('MobileWebClinicsSignedUp','')";
			Db.NonQ(command);
			command="SELECT AlertCategoryNum FROM alertcategory WHERE InternalName='OdAllTypes' AND IsHQCategory=1";
			long odAllTypesAlertCategoryNum=Db.GetLong(command);
			if(odAllTypesAlertCategoryNum > 0) {
				//23 for SupplementalBackups
				command=$"INSERT INTO alertcategorylink(AlertCategoryNum,AlertType) VALUES({POut.Long(odAllTypesAlertCategoryNum)},23)";
				Db.NonQ(command);
			}
		}

		private static void To19_3_10() {
			string command="";
			//Prophy image on appt will no longer appear, so change the default color to something visible instead of empty
			command="UPDATE apptviewitem SET ElementColor=-6859558 WHERE ElementDesc='Prophy/PerioPastDue[P]'";
			Db.NonQ(command);
			//Recall image on appt will no longer appear, so change the default color to something visible instead of empty
			command="UPDATE apptviewitem SET ElementColor=-1822956 WHERE ElementDesc='RecallPastDue[R]'";
			Db.NonQ(command);
		}

		private static void To19_3_11() {
			string command="ALTER TABLE xwebresponse ADD OrderId varchar(255) NOT NULL";
			Db.NonQ(command);
		}

		private static void To19_3_14() {
			//Add hidden pref UseAlternateOpenFileDialogWindow if it hasn't been added.
			string command = "SELECT * FROM preference WHERE preference.PrefName = 'UseAlternateOpenFileDialogWindow'";
			if(Db.GetTable(command).Rows.Count == 0) {
				command="INSERT INTO preference(PrefName,ValueString) VALUES('UseAlternateOpenFileDialogWindow','0')"; //Default to false.
				Db.NonQ(command);
			}
		}

		private static void To19_3_18() {
			if(!LargeTableHelper.ColumnExists(LargeTableHelper.GetCurrentDatabase(),"IsTransfer","claimproc")) {
				LargeTableHelper.AlterLargeTable("claimproc","ClaimProcNum",new List<Tuple<string,string>>{ Tuple.Create("IsTransfer","tinyint NOT NULL") });
			}
		}

		private static void To19_3_22() {
			string command="ALTER TABLE reseller ADD BillingType bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE reseller ADD INDEX (BillingType)";
			Db.NonQ(command);
			command="ALTER TABLE reseller ADD VotesAllotted int NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE reseller ADD Note varchar(4000) NOT NULL";
			Db.NonQ(command);
			//42 is 'No Support: Developer/Reseller'
			command="UPDATE reseller SET BillingType=42,Note='This is a customer of a reseller.  We do not directly support this customer.'";
			Db.NonQ(command);
		}

		private static void To19_3_25() {
			string command=$@"INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note)
				VALUES ('DexisIntegrator','Dexis Integrator from www.kavo.com','0','','','')";
			long programNum=Db.NonQ(command,true);
			command=$@"SELECT ProgramNum FROM program WHERE ProgName='Dexis'";
			long programNumDexis=Db.GetLong(command);
			command=$@"INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue)
				SELECT {POut.Long(programNum)},PropertyDesc,PropertyValue
				FROM programproperty
				WHERE ProgramNum={POut.Long(programNumDexis)}
				AND PropertyDesc='Enter 0 to use PatientNum, or 1 to use ChartNum'";
			Db.NonQ(command);
			command=$@"INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText)
				SELECT {POut.Long(programNum)},ToolBar,'DexisIntegrator'
				FROM toolbutitem
				WHERE ProgramNum={POut.Long(programNumDexis)}";
			Db.NonQ(command);
		}

		private static void To19_3_26() {
			string command;
			if(!LargeTableHelper.IndexExists("statement","IsSent")) {
				command="ALTER TABLE statement ADD INDEX (IsSent)";
				Db.NonQ(command);
			}
			command="ALTER TABLE claim MODIFY COLUMN ClaimNote VARCHAR (400)";
			Db.NonQ(command);
		}

		private static void To19_3_29() {
			string command;
			command="ALTER TABLE timeadjust ADD PtoDefNum bigint NOT NULL DEFAULT 0";
			Db.NonQ(command);
			command="ALTER TABLE timeadjust ADD INDEX (PtoDefNum)";
			Db.NonQ(command);
			command="ALTER TABLE timeadjust ADD PtoHours time NOT NULL DEFAULT '00:00:00'";
			Db.NonQ(command);
		}

		private static void To19_3_33() {
			string command;
			//Moving codes to the Obsolete category that were deleted in CDT 2020.
			if(CultureInfo.CurrentCulture.Name.EndsWith("US")) {//United States
				//Move deprecated codes to the Obsolete procedure code category.
				//Make sure the procedure code category exists before moving the procedure codes.
				string procCatDescript="Obsolete";
				long defNumForCat=0;
				command="SELECT DefNum FROM definition WHERE Category=11 AND ItemName='"+POut.String(procCatDescript)+"'";//11 is DefCat.ProcCodeCats
				DataTable dtDef=Db.GetTable(command);
				if(dtDef.Rows.Count==0) { //The procedure code category does not exist, add it
					command="SELECT COUNT(*) FROM definition WHERE Category=11";//11 is DefCat.ProcCodeCats
					int countCats=PIn.Int(Db.GetCount(command));
					command="INSERT INTO definition (Category,ItemName,ItemOrder) "
								+"VALUES (11"+",'"+POut.String(procCatDescript)+"',"+POut.Int(countCats)+")";//11 is DefCat.ProcCodeCats
					defNumForCat=Db.NonQ(command,true);
				}
				else { //The procedure code category already exists, get the existing defnum
					defNumForCat=PIn.Long(dtDef.Rows[0]["DefNum"].ToString());
				}
				string[] cdtCodesDeleted=new string[] {
					"D1550",
					"D1555",
					"D8691",
					"D8692",
					"D8693",
					"D8694"
				};
				//Change the procedure codes' category to Obsolete.
				command="UPDATE procedurecode SET ProcCat="+POut.Long(defNumForCat)
					+" WHERE ProcCode IN('"+string.Join("','",cdtCodesDeleted.Select(x => POut.String(x)))+"') ";
				Db.NonQ(command);
			}//end United States CDT codes update
		}

		private static void To19_3_36() {
			string command="";
			//Add pref ReportsDoShowHiddenTPPrepayments.
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReportsDoShowHiddenTPPrepayments','0')";//Boolean, false by default.
			Db.NonQ(command);
		}

		private static void To19_3_43() {
			DetachTransferClaimProcsFromClaimPayments();
		}

		private static void To19_4_1() {
			string command;
			DataTable table;
			command="ALTER TABLE patientnote ADD Consent tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE labcase ADD InvoiceNum varchar(255) NOT NULL";
			Db.NonQ(command);
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.4.1 - claimproc table structure");//No translation in convert script.
			LargeTableHelper.AlterLargeTable("claimproc","ClaimProcNum",
				indexColsAndNames:new[] { Tuple.Create("InsSubNum,ProcNum,Status,ProcDate,PatNum,InsPayAmt,InsPayEst","indexTxFinder") }.ToList());
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.4.1 - apptreminderrule");//No translation in convert script.
			command="ALTER TABLE apptreminderrule ADD IsEnabled tinyint NOT NULL";
			Db.NonQ(command);
			command="UPDATE apptreminderrule SET IsEnabled=TSPrior!=0";//Previously, IsEnabled => TSPrior!=TimeSpan.Zero
			Db.NonQ(command);
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.4.1 - apptthankyousent");//No translation in convert script.
			command="DROP TABLE IF EXISTS apptthankyousent";
			Db.NonQ(command);
			command=@"CREATE TABLE apptthankyousent (
				ApptThankYouSentNum bigint NOT NULL auto_increment PRIMARY KEY,
				ApptNum bigint NOT NULL,
				ApptDateTime datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				ApptSecDateTEntry datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				TSPrior bigint NOT NULL,
				ApptReminderRuleNum bigint NOT NULL,
				IsSmsSent tinyint NOT NULL,
				IsEmailSent tinyint NOT NULL,
				IsForSms tinyint NOT NULL,
				IsForEmail tinyint NOT NULL,
				ClinicNum bigint NOT NULL,
				PatNum bigint NOT NULL,
				PhonePat varchar(255) NOT NULL,
				ResponseDescript text NOT NULL,
				GuidMessageToMobile text NOT NULL,
				MsgTextToMobileTemplate text NOT NULL,
				MsgTextToMobile text NOT NULL,
				EmailSubjTemplate text NOT NULL,
				EmailSubj text NOT NULL,
				EmailTextTemplate text NOT NULL,
				EmailText text NOT NULL,
				DateTimeThankYouTransmit datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				Status tinyint NOT NULL,
				ShortGuidEmail varchar(255) NOT NULL,
				ShortGUID varchar(255) NOT NULL,
				INDEX(ApptNum),
				INDEX(TSPrior),
				INDEX(ApptReminderRuleNum),
				INDEX(ClinicNum),
				INDEX(PatNum),
				INDEX(ApptDateTime)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command=$"INSERT INTO preference (PrefName,ValueString) VALUES('ApptThankYouAutoEnabled','0')";
			Db.NonQ(command);
			//command=$"INSERT INTO preference (PrefName,ValueString) VALUES('ApptConfirmExcludeEThankYou','')";
			//Db.NonQ(command);
			////Any patient configured to opt out of eReminders and eConfirmations for TEXT and/or EMAIL should also be opted out for eThankYous.
			//command="INSERT INTO commoptout (PatNum,CommType,CommMode) SELECT PatNum,3 AS CommType,CommMode FROM commoptout GROUP BY PatNum,CommMode";
			//Db.NonQ(command);
			command="DROP TABLE IF EXISTS mobiledatabyte";
			Db.NonQ(command);
			command=@"CREATE TABLE mobiledatabyte (
				MobileDataByteNum bigint NOT NULL auto_increment PRIMARY KEY,
				RawBase64Data mediumtext NOT NULL,
				RawBase64Code mediumtext NOT NULL,
				RawBase64Tag mediumtext NOT NULL,
				PatNum bigint NOT NULL,
				ActionType tinyint NOT NULL,
				DateTimeEntry datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				DateTimeExpires datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				INDEX(PatNum),
				INDEX(RawBase64Code(16))
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command=$"INSERT INTO preference (PrefName,ValueString) VALUES('MobileAutoUnlockCode','1')";//Default to true
			Db.NonQ(command);
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.4.1 - appointment SecDateTEntry");//No translation in convert script.
			command=$"ALTER TABLE appointment CHANGE COLUMN SecDateEntry SecDateTEntry DATETIME NOT NULL DEFAULT '0001-01-01 00:00:00'";
			Db.NonQ(command);
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.4.1 - histappointment SecDateTEntry");//No translation in convert script.
			command=$"ALTER TABLE histappointment CHANGE COLUMN SecDateEntry SecDateTEntry DATETIME NOT NULL DEFAULT '0001-01-01 00:00:00'";
			Db.NonQ(command);
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.4.1 - patient table structure");//No translation in convert script.
			LargeTableHelper.AlterLargeTable("patient","PatNum",indexColsAndNames:new[] { Tuple.Create("PriProv",""),Tuple.Create("SecProv","") }.ToList());
			ODEvent.Fire(ODEventType.ConvertDatabases,"Upgrading database to version: 19.4.1 - proctp CatPercUCR");//No translation in convert script.
			command="ALTER TABLE proctp ADD CatPercUCR double NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE computerpref ADD HelpButtonXAdjustment double NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE centralconnection ADD HasClinicBreakdownReports tinyint(4) NOT NULL";
			Db.NonQ(command);
			command="UPDATE centralconnection SET HasClinicBreakdownReports=1";//Default to true
			Db.NonQ(command);
			command=$"INSERT INTO preference (PrefName,ValueString) VALUES('PatientSelectFilterRestrictedClinics','0')";//Default to false
			Db.NonQ(command);
			//Add PatientSSNView permission to everyone------------------------------------------------------
			//Grant the PatientSSNView permission to all user groups to preserve old behavior.
			command="SELECT DISTINCT UserGroupNum FROM grouppermission";
			table=Db.GetTable(command);
			long groupNum;
			foreach(DataRow row in table.Rows) {
				groupNum=PIn.Long(row["UserGroupNum"].ToString());
				command="INSERT INTO grouppermission (UserGroupNum,PermType) "
				   +"VALUES("+POut.Long(groupNum)+",181)";//PatientSSNView
				Db.NonQ(command);
			}
			//Add PatientSSNMasked preference, on by default for US only.
			if(CultureInfo.CurrentCulture.Name.EndsWith("US")) {//United States
				command="INSERT INTO preference(PrefName,ValueString) VALUES('PatientSSNMasked','1')";
			}
			else {
				command="INSERT INTO preference(PrefName,ValueString) VALUES('PatientSSNMasked','0')";
			}
			Db.NonQ(command);
			//Widen the default SSN column for the Patient Select window from 65 to 70 because we will now format the SSN field with hyphens for US.
			//displayfield.Category of 1 is PatientSelect.
			command=@"UPDATE displayfield SET ColumnWidth=70
				WHERE Category=1
				AND InternalName='SSN'
				AND ColumnWidth=65";
			Db.NonQ(command);
			command="ALTER TABLE apptreminderrule ADD TemplateAutoReply text NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE apptreminderrule ADD TemplateAutoReplyAgg text NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE apptreminderrule ADD IsAutoReplyEnabled tinyint NOT NULL";
			Db.NonQ(command);
			command="UPDATE apptreminderrule SET TemplateAutoReply='Thank you for confirming your appointment with [OfficeName].  We look forward to seeing you.' WHERE TypeCur=1";//ConfirmationFurtureDay
			Db.NonQ(command);
			command="UPDATE apptreminderrule SET TemplateAutoReplyAgg='Thank you for confirming your appointments with [OfficeName].  We look forward to seeing you.' WHERE TypeCur=1";//ConfirmationFurtureDay
			Db.NonQ(command);
			command="ALTER TABLE confirmationrequest ADD ApptReminderRuleNum bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE confirmationrequest ADD INDEX (ApptReminderRuleNum)";
			Db.NonQ(command);
			//Add PatientDOBView permission to everyone------------------------------------------------------
			//Grant the PatientDOBView permission to all user groups to preserve old behavior.
			command="SELECT DISTINCT UserGroupNum FROM grouppermission";
			table=Db.GetTable(command);
			groupNum=0;
			foreach(DataRow row in table.Rows) {
				groupNum=PIn.Long(row["UserGroupNum"].ToString());
				command="INSERT INTO grouppermission (UserGroupNum,PermType) "
				   +"VALUES("+POut.Long(groupNum)+",182)";//PatientDOBView
				Db.NonQ(command);
			}
			//Add PatientDOBMasked preference, off by default.
			command="INSERT INTO preference(PrefName,ValueString) VALUES('PatientDOBMasked','0')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('CentralManagerUseDynamicMode','0')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('CentralManagerIsAutoLogon','0')";
			Db.NonQ(command);
			command="ALTER TABLE orthocase ADD IsActive tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE orthoproclink ADD ProcLinkType tinyint NOT NULL";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('OrthoBandingCodes','')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('OrthoDebondCodes','')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('OrthoVisitCodes','')";
			Db.NonQ(command);
			command="ALTER TABLE orthoschedule CHANGE VisitPercent VisitAmount double NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE orthoschedule CHANGE BandingPercent BandingAmount double NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE orthoschedule CHANGE DebondPercent DebondAmount double NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE orthoplanlink MODIFY SecUserNumEntry bigint NOT NULL";
			Db.NonQ(command);
			LargeTableHelper.AlterLargeTable("appointment","AptNum",
				new List<Tuple<string,string>> { Tuple.Create("PatternSecondary","varchar(255) NOT NULL") });
			LargeTableHelper.AlterLargeTable("histappointment","HistApptNum",
				new List<Tuple<string,string>> { Tuple.Create("PatternSecondary","varchar(255) NOT NULL") });
			command="ALTER TABLE supply ADD OrderQty int NOT NULL";
			Db.NonQ(command);
			if(!LargeTableHelper.IndexExists("rxpat","PatNum")) {
				command="ALTER TABLE rxpat ADD INDEX (PatNum)";
				Db.NonQ(command);
			}
			command="SELECT MAX(ItemOrder)+1 FROM definition WHERE Category=10";//10 is PaymentTypes
			int maxOrder=PIn.Int(Db.GetCount(command));
			command="INSERT INTO definition (Category,ItemOrder,ItemName) VALUES (10,"+POut.Int(maxOrder)+",'From API')";//10 is PaymentTypes
			long defNum=Db.NonQ(command,true);
			command=$"INSERT INTO preference (PrefName,ValueString) VALUES('ApiPaymentType','{defNum}')";
			Db.NonQ(command);
			command="ALTER TABLE payconnectresponseweb ADD IsTokenSaved tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE payconnectresponseweb ADD PayToken varchar(255) NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE payconnectresponseweb ADD ExpDateToken varchar(255) NOT NULL";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('PaymentsTransferPatientIncomeOnly','0')";//Boolean, false by default.
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('SecurityLogOffAllowUserOverride','0')";//Boolean, false by default.
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('BrokenApptRequiredOnMove','0')";//Boolean, false by default.
			Db.NonQ(command);
			command="SELECT MAX(ItemOrder)+1 FROM definition WHERE Category=27";//27 is CommLogTypes
			maxOrder=PIn.Int(Db.GetCount(command));
			command=$@"INSERT INTO definition (Category,ItemName,ItemOrder,ItemValue) 
				VALUES(27,'FHIR',{POut.Int(maxOrder)},'FHIR')";//27 is CommLogTypes
			Db.NonQ(command);
			command="ALTER TABLE supplyorder ADD DateReceived date NOT NULL DEFAULT '0001-01-01'";
			Db.NonQ(command);
			command="UPDATE supplyorder SET DateReceived = CURDATE()";
			Db.NonQ(command);
			//FeeSchedGroup
			command="DROP TABLE IF EXISTS feeschedgroup";
			Db.NonQ(command);
			command=@"CREATE TABLE feeschedgroup (
					FeeSchedGroupNum bigint NOT NULL auto_increment PRIMARY KEY,
					Description varchar(255) NOT NULL,
					FeeSchedNum bigint NOT NULL,
					ClinicNums varchar(255) NOT NULL,
					INDEX(FeeSchedNum)
					) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command=$"INSERT INTO preference (PrefName,ValueString) VALUES('ShowFeeSchedGroups','0')";
			Db.NonQ(command);
		}//End of 19_4_1() method

		private static void To19_4_2() {
			string command="";
			command="ALTER TABLE payconnectresponseweb ADD RefNumber varchar(255) NOT NULL";
			Db.NonQ(command);
		}

		private static void To19_4_3() {
			string command="";
			//Add pref ReportsDoShowHiddenTPPrepayments if it hasn't been added.
			command = "SELECT * FROM preference WHERE preference.PrefName = 'ReportsDoShowHiddenTPPrepayments'";
			if(Db.GetTable(command).Rows.Count == 0) {
				command="INSERT INTO preference(PrefName,ValueString) VALUES('ReportsDoShowHiddenTPPrepayments','0')";//Boolean, false by default.
				Db.NonQ(command);
			}
			if(!LargeTableHelper.IndexExists("preference","PrefName")) {
				command="ALTER TABLE preference ADD INDEX (PrefName)";
				Db.NonQ(command);
			}			
			command="ALTER TABLE payconnectresponseweb ADD TransType varchar(255) NOT NULL";
			Db.NonQ(command);	
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ShortCodeOptInOnApptComplete','1')";//Boolean, true by default.
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ShortCodeOptInScript','')";//String, can be updated by HQ via eServices sync.
			Db.NonQ(command);
			LargeTableHelper.AlterLargeTable("patient","PatNum",new List<Tuple<string,string>> { Tuple.Create("ShortCodeOptIn","tinyint NOT NULL") });	
		}

		private static void To19_4_9() {
			string command="";
			command="ALTER TABLE orthocase CHANGE FeeIns FeeInsPrimary double NOT NULL, ADD FeeInsSecondary double NOT NULL";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ShortCodeOptedOutScript','')";//String, will be updated by HQ via eServices sync.
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ShortCodeOptInClinicTitle','')";//String, empty defaults to PracticeTitle.
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ShortCodeOptInOnApptCompleteOffScript','')";//String, will be updated by HQ via eServices sync.
			Db.NonQ(command);
			DetachTransferClaimProcsFromClaimPayments();
			command="INSERT INTO preference (PrefName,ValueString) VALUES('TreatPlanPromptSave','1')"; //Boolean, true by default.
			Db.NonQ(command);
		}

		private static void To19_4_12() {
			string command=@"INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note)
				 VALUES(
				 'BencoPracticeManagement', 
				 'Benco Practice Management', 
				 '0', 
				 'https://identity.benco.com/auth/login',
				 '',
				 '');";
			long programNum=Db.NonQ(command);
			command=$"INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) VALUES ({POut.Long(programNum)},7,'Benco');"; //7 = Main Toolbar
			Db.NonQ(command);
			//We are running this section of code for HQ only
			//This is very uncommon and normally manual queries should be run instead of doing a convert script.
			command="SELECT ValueString FROM preference WHERE PrefName='DockPhonePanelShow'";
			DataTable tableTriage=Db.GetTable(command);
			if(tableTriage.Rows.Count > 0 && PIn.Bool(tableTriage.Rows[0][0].ToString())) {
				command="INSERT INTO preference(PrefName,ValueString) VALUES('TriageCallsWarning','35')";
				Db.NonQ(command);
			}
		}

		private static void To19_4_15() {
			//Gmail OAuth2.0
			string command="ALTER TABLE emailaddress ADD AccessToken varchar(255) NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE emailaddress ADD RefreshToken varchar(255) NOT NULL";
			Db.NonQ(command);
		}
	}
}