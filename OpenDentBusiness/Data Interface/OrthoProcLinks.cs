using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OpenDentBusiness{
	///<summary></summary>
	public class OrthoProcLinks{
		#region Get Methods
		///<summary>Gets all orthoproclinks from DB.</summary>
		public static List<OrthoProcLink> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<OrthoProcLink>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT orthoproclink.* FROM orthoproclink";
			return Crud.OrthoProcLinkCrud.SelectMany(command);
		}

		///<summary>Get a list of all OrthoProcLinks for an OrthoCase.</summary>
		public static List<OrthoProcLink> GetManyByOrthoCase(long orthoCaseNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<OrthoProcLink>>(MethodBase.GetCurrentMethod(),orthoCaseNum);
			}
			string command="SELECT * FROM orthoproclink WHERE orthoproclink.OrthoCaseNum = "+POut.Long(orthoCaseNum);
			return Crud.OrthoProcLinkCrud.SelectMany(command);
		}

		///<summary>Gets one OrthoProcLink of the specified OrthoProcType for an OrthoCase. This should only be used to get procedures of the 
		///Banding or Debond types as only one of each can be linked to an Orthocase.</summary>
		public static OrthoProcLink GetByType(long orthoCaseNum,OrthoProcType linkType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<OrthoProcLink>(MethodBase.GetCurrentMethod(),orthoCaseNum,linkType);
			}
			string command=$@"SELECT * FROM orthoproclink WHERE orthoproclink.OrthoCaseNum={POut.Long(orthoCaseNum)}
				AND orthoproclink.ProcLinkType={POut.Int((int)linkType)}";
			return Crud.OrthoProcLinkCrud.SelectOne(command);
		}

		///<summary>Returns a list of OrthoProcLinks of the specified type that are associated to any OrthoCaseNum from the list passed in.</summary>
		public static List<OrthoProcLink> GetManyByOrthoCases(List<long> listOrthoCaseNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<OrthoProcLink>>(MethodBase.GetCurrentMethod(),listOrthoCaseNums);
			}
			if(listOrthoCaseNums.Count<=0) {
				return new List<OrthoProcLink>();
			}
			string command=$@"SELECT * FROM orthoproclink
				WHERE orthoproclink.OrthoCaseNum IN({string.Join(",",listOrthoCaseNums)})";
			return Crud.OrthoProcLinkCrud.SelectMany(command);
		}

		///<summary>Gets all OrthoProcLinks associated to any procedures in the list passed in.</summary>
		public static List<OrthoProcLink> GetManyForProcs(List<long> listProcNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<OrthoProcLink>>(MethodBase.GetCurrentMethod(),listProcNums);
			}
			if(listProcNums.Count<=0) {
				return new List<OrthoProcLink>();
			}
			string command=$@"SELECT * FROM orthoproclink
				WHERE orthoproclink.ProcNum IN({string.Join(",",listProcNums)})";
			return Crud.OrthoProcLinkCrud.SelectMany(command);
		}

		///<summary>Returns a single OrthoProcLink for the procNum. There should only be one in db per procedure.</summary>
		public static OrthoProcLink GetByProcNum(long procNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<OrthoProcLink>(MethodBase.GetCurrentMethod(),procNum);
			}
			string command="SELECT * FROM orthoproclink WHERE ProcNum="+POut.Long(procNum);
			return Crud.OrthoProcLinkCrud.SelectOne(command);
		}

		///<summary>Returns a list of all ProcLinks of the visit type associated to an OrthoCase.</summary>
		public static List<OrthoProcLink> GetVisitLinksForOrthoCase(long orthoCaseNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<OrthoProcLink>>(MethodBase.GetCurrentMethod(),orthoCaseNum);
			}
			string command=$@"SELECT * FROM orthoproclink WHERE orthoproclink.OrthoCaseNum={POut.Long(orthoCaseNum)}
			AND orthoproclink.ProcLinkType={POut.Int((int)OrthoProcType.Visit)}";
			return Crud.OrthoProcLinkCrud.SelectMany(command);
		}
		#endregion Get Methods

		#region Modification Methods
		#region Insert
		///<summary>Inserts an OrthoProcLink into the database. Returns the OrthoProcLinkNum.</summary>
		public static long Insert(OrthoProcLink orthoProcLink) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				orthoProcLink.OrthoProcLinkNum=Meth.GetLong(MethodBase.GetCurrentMethod(),orthoProcLink);
				return orthoProcLink.OrthoProcLinkNum;
			}
			return Crud.OrthoProcLinkCrud.Insert(orthoProcLink);
		}

		///<summary>Changes procFee. Links procedure to an active OrthoCase. Returns new ProcLink if the procedure is linked, else returns null.
		///Should only be used when a procedure is set complete. This will set the procedure's ProcFee but does not update the procedure in the DB.
		///This must be done if this function returns true.</summary>
		public static OrthoProcLink LinkProcForActiveOrthoCase(Procedure proc,OrthoCase orthoCase=null,List<OrthoProcLink> listProcLinksForCase=null
			,OrthoPlanLink schedulePlanLink=null,OrthoSchedule orthoSchedule=null) {
			//No remoting role check; no call to db
			if(orthoCase==null) {
				orthoCase=OrthoCases.GetActiveForPat(proc.PatNum);
			}
			if(orthoCase==null) {//No active ortho case for pat so return.
				return null;
			}
			OrthoCase orthoCaseOld=orthoCase.Copy();
			if(listProcLinksForCase==null) {
				listProcLinksForCase=GetManyByOrthoCase(orthoCase.OrthoCaseNum);
			}
			List<OrthoProcLink> listAllVisitProcLinks=listProcLinksForCase.Where(x => x.ProcLinkType==OrthoProcType.Visit).ToList();
			//Don't link procs to an OrthoCase with a completed debond procedure
			if(listProcLinksForCase.FirstOrDefault(x => x.ProcLinkType==OrthoProcType.Debond)!=null) {
				return null;
			}
			if(!orthoCase.IsTransfer) {
				OrthoProcLink bandingProcLink=listProcLinksForCase.FirstOrDefault(x => x.ProcLinkType==OrthoProcType.Banding);
				//If proc being set complete is the banding, it is already linked. We just need to set the fee.
				if(bandingProcLink.ProcNum==proc.ProcNum) {
					SetProcFeeForLinkedProc(orthoCase,proc,OrthoProcType.Banding,listAllVisitProcLinks);
					orthoCase.BandingDate=proc.ProcDate;
					OrthoCases.Update(orthoCase,orthoCaseOld);
					return bandingProcLink;
				}
				Procedure bandingProc=Procedures.GetOneProc(bandingProcLink.ProcNum,false);
				//If proc is not banding and banding is not complete yet, don't link procedure
				if(bandingProc.ProcStatus!=ProcStat.C) {
					return null;
				}
			}
			if(listProcLinksForCase.Select(x => x.ProcNum).ToList().Contains(proc.ProcNum)) {
				return null;//Procedure is not banding and is already linked so do nothing.
			}
			string procCode=ProcedureCodes.GetProcCode(proc.CodeNum).ProcCode;
			List<string> listDebondProcs=OrthoCases.GetListProcTypeProcCodes(PrefName.OrthoDebondCodes);
			List<string> listVisitProcs=OrthoCases.GetListProcTypeProcCodes(PrefName.OrthoVisitCodes);
			if(listVisitProcs.Contains(procCode) || listDebondProcs.Contains(procCode)) {
				if(schedulePlanLink==null) {
					schedulePlanLink=OrthoPlanLinks.GetOneForOrthoCaseByType(orthoCase.OrthoCaseNum,OrthoPlanLinkType.OrthoSchedule);
				}
				if(orthoSchedule==null) {
					orthoSchedule=OrthoSchedules.GetOne(schedulePlanLink.FKey);
				}
				//Link visit procedure
				if(listVisitProcs.Contains(procCode)) {
					OrthoProcLink newVisitProcLink=CreateHelper(orthoCase.OrthoCaseNum,proc.ProcNum,OrthoProcType.Visit);
					newVisitProcLink.OrthoProcLinkNum=Insert(newVisitProcLink);
					listAllVisitProcLinks.Add(newVisitProcLink);
					listProcLinksForCase.Add(newVisitProcLink);
					SetProcFeeForLinkedProc(orthoCase,proc,OrthoProcType.Visit,listAllVisitProcLinks,schedulePlanLink,orthoSchedule);
					return newVisitProcLink;
				}
				//Link debond procedure
				else if(listDebondProcs.Contains(procCode)) {
					OrthoProcLink newDebondProcLink=CreateHelper(orthoCase.OrthoCaseNum,proc.ProcNum,OrthoProcType.Debond);
					newDebondProcLink.OrthoProcLinkNum=Insert(newDebondProcLink);
					listProcLinksForCase.Add(newDebondProcLink);
					OrthoCases.SetActiveState(orthoCase,schedulePlanLink,orthoSchedule,false);//deactivate the ortho case
					SetProcFeeForLinkedProc(orthoCase,proc,OrthoProcType.Debond,listAllVisitProcLinks,schedulePlanLink,orthoSchedule);
					orthoCase.DebondDate=proc.ProcDate;
					OrthoCases.Update(orthoCase,orthoCaseOld);
					return newDebondProcLink;
				}
			}
			return null;//Procedure is not a Banding, Visit, or Debond. Do nothing.
		}
		#endregion Insert

		#region Update
		/// <summary>Update only data that is different in newOrthoProcLink</summary>
		public static void Update(OrthoProcLink newOrthoProcLink,OrthoProcLink oldOrthoProcLink) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),newOrthoProcLink,oldOrthoProcLink);
				return;
			}
			Crud.OrthoProcLinkCrud.Update(newOrthoProcLink,oldOrthoProcLink);
		}
		#endregion Update

		#region Delete
		///<summary>Delete an OrthoProcLink from the database.</summary>
		public static void Delete(long orthoProcLinkNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),orthoProcLinkNum);
				return;
			}
			Crud.OrthoProcLinkCrud.Delete(orthoProcLinkNum);
		}

		///<summary>Deletes all ProcLinks in the provided list of OrthoProcLinkNums.</summary>
		public static void DeleteMany(List<long> listOrthoProcLinkNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listOrthoProcLinkNums);
				return;
			}
			if(listOrthoProcLinkNums.Count<=0) {
				return;
			}
			string command=$"DELETE FROM orthoproclink WHERE OrthoProcLinkNum IN({string.Join(",",listOrthoProcLinkNums)})";
			Db.NonQ(command);
		}
		#endregion Delete
		#endregion Modification Methods

		#region Misc Methods
		///<summary>Does not insert it in the DB. Returns an OrthoProcLink of the specified type for the OrthoCaseNum and procNum passed in.</summary>
		public static OrthoProcLink CreateHelper(long orthoCaseNum,long procNum,OrthoProcType procType) {
			//No remoting role check; no call to db
			OrthoProcLink orthoProcLink=new OrthoProcLink();
			orthoProcLink.OrthoCaseNum=orthoCaseNum;
			orthoProcLink.ProcNum=procNum;
			orthoProcLink.ProcLinkType=procType;
			orthoProcLink.SecUserNumEntry=Security.CurUser.UserNum;
			return orthoProcLink;
		}

		///<summary>Determines whether the passed in procedure is a Banding, Visit, or Debond procedure and
		///sets the ProcFee accordingly. Does not update the procedure in the database.</summary>
		public static void SetProcFeeForLinkedProc(OrthoCase orthoCase,Procedure proc,OrthoProcType procType,List<OrthoProcLink> listVisitProcLinks,
			OrthoPlanLink scheduleOrthoPlanLink=null,OrthoSchedule orthoSchedule=null)
		{
			//No remoting role check; no call to db
			if(scheduleOrthoPlanLink==null && orthoSchedule==null) {
				scheduleOrthoPlanLink=OrthoPlanLinks.GetOneForOrthoCaseByType(orthoCase.OrthoCaseNum,OrthoPlanLinkType.OrthoSchedule);
			}
			if(orthoSchedule==null) {
				orthoSchedule=OrthoSchedules.GetOne(scheduleOrthoPlanLink.FKey);
			}
			double procFee=0;
			switch(procType) {
				case OrthoProcType.Banding:
					procFee=orthoSchedule.BandingAmount;
					break;
				case OrthoProcType.Debond:
					procFee=orthoSchedule.DebondAmount;
					break;
				case OrthoProcType.Visit:
					double allVisitsAmount=Math.Round((orthoCase.Fee-orthoSchedule.BandingAmount-orthoSchedule.DebondAmount)*100)/100;
					int plannedVisitCount=OrthoSchedules.CalculatePlannedVisitsCount(orthoSchedule.BandingAmount,orthoSchedule.DebondAmount
						,orthoSchedule.VisitAmount,orthoCase.Fee);
					if(listVisitProcLinks.Count==plannedVisitCount) {
						procFee=Math.Round((allVisitsAmount-orthoSchedule.VisitAmount*(plannedVisitCount-1))*100)/100;
					}
					else if(listVisitProcLinks.Count<plannedVisitCount) {
						procFee=orthoSchedule.VisitAmount;
					}
					break;
			}
			proc.ProcFee=procFee;
			proc.BaseUnits=0;
			proc.UnitQty=1;
		}

		///<summary>Returns true if the procNum is contained in at least one OrthoProcLink.</summary>
		public static bool IsProcLinked(long procNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),procNum);
			}
			string command="SELECT * FROM orthoproclink WHERE orthoproclink.ProcNum="+POut.Long(procNum);
			return Crud.OrthoProcLinkCrud.SelectMany(command).Count>0;
		}

		///<summary>Returns true if OrthoCase feature is enabled, pat has an active ortho case, no debond procedures are linked to ortho case,
		///banding procedure is complete if a link for one exists.</summary>
		public static bool WillProcLinkToOrthoCase(long patNum,string procCode,ref OrthoCase activeOrthoCase
			,ref List<OrthoProcLink> listOrthoProcLinksForCase) {
			//No remoting role check; no call to db
			List<string> listVisitAndDebondCodes=new List<string>();
			listVisitAndDebondCodes.AddRange(OrthoCases.GetListProcTypeProcCodes(PrefName.OrthoVisitCodes));
			listVisitAndDebondCodes.AddRange(OrthoCases.GetListProcTypeProcCodes(PrefName.OrthoDebondCodes));
			//If Orthocases aren't enabled or code in question is not a visit or debond code return false.
			if(!OrthoCases.HasOrthoCasesEnabled() || !listVisitAndDebondCodes.Contains(procCode)) {
				return false;
			}
			if(activeOrthoCase==null) {
				activeOrthoCase=OrthoCases.GetActiveForPat(patNum);
			}
			if(activeOrthoCase==null) {
				return false;//If patient doesn't have an active ortho case return false.
			}
			if(listOrthoProcLinksForCase==null) {
				listOrthoProcLinksForCase=GetManyByOrthoCase(activeOrthoCase.OrthoCaseNum);
			}
			//If active ortho case already has a debond procedure completed return false. Ortho case is considered completed. 
			if(listOrthoProcLinksForCase.Where(x => x.ProcLinkType==OrthoProcType.Debond).ToList().Count>0) {
				return false;
			}
			//If banding procedure is linked and it is not complete, return false.
			OrthoProcLink bandingProcLink=listOrthoProcLinksForCase.Where(x => x.ProcLinkType==OrthoProcType.Banding).ToList().FirstOrDefault();
			if(bandingProcLink!=null && Procedures.GetOneProc(bandingProcLink.ProcNum,false).ProcStatus!=ProcStat.C) {
				return false;
			}
			return true;
		}
		#endregion Misc Methods
		//If this table type will exist as cached data, uncomment the Cache Pattern region below and edit.
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		///<summary>Gets one OrthoProcLink from the db.</summary>
		public static OrthoProcLink GetOne(long orthoProcLinkNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<OrthoProcLink>(MethodBase.GetCurrentMethod(),orthoProcLinkNum);
			}
			return Crud.OrthoProcLinkCrud.SelectOne(orthoProcLinkNum);
		}
		///<summary></summary>
		public static void Update(OrthoProcLink orthoProcLink){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),orthoProcLink);
				return;
			}
			Crud.OrthoProcLinkCrud.Update(orthoProcLink);
		}
		*/
	}
}