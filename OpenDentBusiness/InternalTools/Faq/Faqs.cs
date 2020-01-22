using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary>This class requires that you switch database context to the manual publisher database (jordans_mp on server 174). This should be done by using the DataAction pattern.</summary>
	public class Faqs {
		#region Get Methods
		///<summary>Returns a list of Faq's that contain the given question text.  Does not have to be an exact match.</summary>
		public static List<Faq> GetByQuestion(string questionText){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Faq>>(MethodBase.GetCurrentMethod(),questionText);
			}
			string command=$"SELECT * FROM faq WHERE QuestionText LIKE '%{POut.String(questionText)}%'";
			List<Faq> retVal=new List<Faq>();
			DataAction.RunManualPublisherHQ(() => {
				retVal=Crud.FaqCrud.SelectMany(command);
			});
			return retVal;
		}
		
		///<summary>Gets one Faq from the db.</summary>
		public static Faq GetOne(long faqNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<Faq>(MethodBase.GetCurrentMethod(),faqNum);
			}
			Faq retVal=null;
			DataAction.RunManualPublisherHQ(() => {
				retVal=Crud.FaqCrud.SelectOne(faqNum);
			});
			return retVal;
		}

		public static List<Faq> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<List<Faq>>(MethodBase.GetCurrentMethod());
			}
			string command=$"SELECT * FROM faq";
			List<Faq> retVal=new List<Faq>();
			DataAction.RunManualPublisherHQ(() => {
				retVal=Crud.FaqCrud.SelectMany(command);
			});
			return retVal;
		}

		///<summary>Gets all faq from the database for a given version. Please note the manual publisher treats version as major+minor.
		///For example, to get all faq's for version 19.1.45 version should be "191".</summary>
		public static List<Faq> GetAllForVersion(int version) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Faq>>(MethodBase.GetCurrentMethod(),version);
			}
			string command=$"SELECT * FROM faq WHERE ManualVersion={version.ToString()}";
			List<Faq> retVal=new List<Faq>();
			DataAction.RunManualPublisherHQ(() => {
				retVal=Crud.FaqCrud.SelectMany(command);
			});
			return retVal;
		}

		///<summary>Returns a list of Faq objects containing the given manualPageName.  Does not have to be an exact match.</summary>
		public static List<Faq> GetForPageName(string manualPageName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Faq>>(MethodBase.GetCurrentMethod(),manualPageName);
			}
			string command=$@"
				SELECT faq.*
				FROM manualpage
				INNER JOIN faqmanualpagelink ON manualpage.ManualPageNum=faqmanualpagelink.ManualPageNum
				INNER JOIN faq ON faqmanualpagelink.FaqNum=faq.FaqNum
				WHERE manualpage.FileName LIKE '%{manualPageName}%'
				GROUP BY faqmanualpagelink.FaqNum
			";
			List<Faq> retVal=new List<Faq>();
			DataAction.RunManualPublisherHQ(() => {
				retVal=Crud.FaqCrud.SelectMany(command);
			});
			return retVal;
		}

		///<summary>Returns a single manualPageNum for the given manualPageName and manualVersion.</summary>
		public static long GetOneManualPageNum(string manualPageName,int manualVersion) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetLong(MethodBase.GetCurrentMethod(),manualPageName,manualVersion);
			}
			string command=$@"
				SELECT ManualPageNum
				FROM manualpage
				WHERE manualpage.FileName='{manualPageName}'
				AND VersionOd={manualVersion}
			";
			long manualPageNum=0;
			DataAction.RunManualPublisherHQ(() => {
				manualPageNum=Db.GetLong(command);
			});
			return manualPageNum;
		}

		///<summary>Returns a list of manual page names that the given FaqNum is currently linked to.</summary>
		public static List<string> GetLinkedManualPages(long faqNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<List<string>>(MethodBase.GetCurrentMethod(),faqNum);
			}
			string command=$@"
				SELECT manualpage.FileName
				FROM faqmanualpagelink
				INNER JOIN manualpage ON manualpage.ManualPageNum=faqmanualpagelink.ManualPageNum
				WHERE faqmanualpagelink.FaqNum={faqNum}
			";
			List<string> retVal=new List<string>();
			DataAction.RunManualPublisherHQ(() => {
				retVal=Db.GetListString(command);
			});
			return retVal;
		}

		
		///<summary>Gets all manual page names from database. Excludes entries in the manualpage table that are used for the manual index.</summary>
		public static List<string> GetAllManualPageNames() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<List<string>>(MethodBase.GetCurrentMethod());
			}
			string command=$@"
				SELECT manualpage.FileName
				FROM manualpage
				WHERE FileName NOT LIKE '+%';
			";
			List<string> retVal=new List<string>();
			DataAction.RunManualPublisherHQ(() => {
				retVal=Db.GetListString(command).Distinct().ToList();
			});
			return retVal;
		}

		///<summary>Gets all Faq's associated to the given manual page name and version. If version is blank will query for all versions.</summary>
		public static List<Faq> GetAllForNameAndVersion(string pageName,int version) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Faq>>(MethodBase.GetCurrentMethod(),pageName,version);
			}
			string command=$@"
			SELECT faq.*
			FROM faqmanualpagelink
			INNER JOIN manualpage ON manualpage.ManualPageNum=faqmanualpagelink.ManualPageNum
			INNER JOIN faq ON faq.FaqNum=faqmanualpagelink.FaqNum
			WHERE manualpage.FileName LIKE '%{POut.String(pageName)}%'";
			if(version>0) {
				command+=$"AND faq.ManualVersion={POut.Int(version)}";
			}
			List<Faq> retVal=new List<Faq>();
			DataAction.RunManualPublisherHQ(() => {
				retVal=Crud.FaqCrud.SelectMany(command);
			});
			return retVal;
		}

		///<summary>Gets the version that is considered 'stable' to the manual publisher. This will always be in the form '183', '184', '191' as far as I can tell.</summary>
		public static int GetStableManualVersion() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod());
			}
			string command="SELECT VersionOd FROM version WHERE IsMainNoNumber=1";
			int retval=0;
			DataAction.RunManualPublisherHQ(() => {
				retval=PIn.Int(Db.GetScalar(command));
			});
			return retval;
		}

		#endregion Get Methods
		#region Modification Methods
		#region Insert
		///<summary></summary>
		public static long Insert(Faq faq){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				faq.FaqNum=Meth.GetLong(MethodBase.GetCurrentMethod(),faq);
				return faq.FaqNum;
			}
			long faqNum=0;
			DataAction.RunManualPublisherHQ(() => {
				faqNum=Crud.FaqCrud.Insert(faq);
			});
			return faqNum;
		}

		///<summary>Inserts a row into jordans_mp.faqmanualpagelink for each of the specified pages to link the given FaqNum to.
		///This linker table allows a many to many relationship between faq objects and manualpages i.e.- a manual page can have many faq's
		///and faq's can be linked to many manual pages. Only links to the manualpage of the specified version.</summary>
		public static void CreateManualPageLinks(long faqNum,List<string> listPagesToLink,int manualVersion) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),faqNum,listPagesToLink,manualVersion);
				return;
			}
			DataAction.RunManualPublisherHQ(() => {
				foreach(string page in listPagesToLink) {
					long manualPageNum=GetOneManualPageNum(page,manualVersion);
					Db.NonQ($@"INSERT INTO faqmanualpagelink(ManualPageNum,FaqNum) VALUES ({manualPageNum},{faqNum})");
				}
			});
		}
		#endregion Insert
		#region Update
		///<summary></summary>
		public static void Update(Faq faq){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),faq);
				return;
			}
			DataAction.RunManualPublisherHQ(() => {
				Crud.FaqCrud.Update(faq);
			});
		}
		#endregion Update
		#region Delete
		///<summary></summary>
		public static void Delete(long faqNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),faqNum);
				return;
			}
			DataAction.RunManualPublisherHQ(() => {
				Crud.FaqCrud.Delete(faqNum);
			});
		}

		///<summary>Deletes all faqmanualpagelink rows that have the given faqNum</summary>
		public static void DeleteManualPageLinkForFaqNum(long faqNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),faqNum);
				return;
			}
			string command=$@"DELETE FROM faqmanualpagelink WHERE FaqNum={faqNum}";
			DataAction.RunManualPublisherHQ(() => {
				Db.NonQ(command);
			});
		}
		#endregion Delete
		#endregion Modification Methods
		#region Misc Methods
		///<summary>Helper method that copies existing Faq's (and their links) for the most recent version
		///and inserts them for the new specified version. This is how Jordan handles releases in the manual publisher.
		///Faq must follow this pattern as well. PageForVersionExists() should be called before this method.</summary>
		public static void CreateFaqsForNewVersion(int newVersion) {
			//No need to check RemotingRole; no direct calls to the Db from this method.
			string version=VersionReleases.GetLastReleases(2);
			string[] versionParts=version.Split('.');
			int lastStableVersion=PIn.Int(versionParts[0]+versionParts[1]);
			List<Faq> listFaqs=GetAllForVersion(lastStableVersion);
			foreach(Faq faq in listFaqs) {
				List<string> listPagesLinkedToFaq=GetLinkedManualPages(faq.FaqNum);
				Faq faqNew=faq.Copy();
				faqNew.ManualVersion=newVersion;
				long faqNewNum=Insert(faqNew);
				//Attach the manual pages from the beta version to the new version.
				CreateManualPageLinks(faqNew.FaqNum,listPagesLinkedToFaq,faqNew.ManualVersion);
			}
		}

		///<summary>Checks the manual publisher database to see if the manual pages for a given version exists.
		///Used when releasing a new version of the OD FAQ system.</summary>
		public static bool PageForVersionExists(int manualVersion) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),manualVersion);
			}
			string command=$"SELECT COUNT(*)>0 FROM manualpage WHERE VersionOd={manualVersion.ToString()}";
			bool retVal=false;
			DataAction.RunManualPublisherHQ(() => {
				retVal=PIn.Bool(Db.GetScalar(command));
			});
			return retVal;
		}
		#endregion Misc Methods
	}
}