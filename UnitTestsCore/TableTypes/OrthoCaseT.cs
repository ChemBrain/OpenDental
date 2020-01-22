using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenDentBusiness;

namespace UnitTestsCore {
	/// <summary>	/// </summary>
	public class OrthoCaseT {
		///<summary>Inserts the OrthoCase, OrthoSchedule, Schedule OrthoPlanLink, and banding OrthoProcLink for an Ortho Case.</summary>
		public static long InsertForFormOrthoCase(long patNum,double fee,double feeInsPrimary,double feeInsSecondary,double feePat,DateTime bandingDate
			,bool isTransfer,DateTime debondDateExpected,double bandingAmount,double debondAmount,double visitAmount,Procedure bandingProc)
		{
			//No remoting role check; no call to db
			//Ortho Case
			OrthoCase newOrthoCase=new OrthoCase();
			newOrthoCase.PatNum=patNum;
			newOrthoCase.Fee=fee;
			newOrthoCase.FeeInsPrimary=feeInsPrimary;
			newOrthoCase.FeeInsSecondary=feeInsSecondary;
			newOrthoCase.FeePat=feePat;
			newOrthoCase.BandingDate=bandingDate;
			newOrthoCase.DebondDateExpected=debondDateExpected;
			newOrthoCase.IsTransfer=isTransfer;
			newOrthoCase.SecUserNumEntry=Security.CurUser.UserNum;
			newOrthoCase.IsActive=true;//New Ortho Cases can only be added if there are no other active ones. So we automatically set a new ortho case as active.
			if(bandingProc.AptNum!=0) {//If banding is scheduled save the appointment date instead.
				newOrthoCase.BandingDate=bandingProc.ProcDate;
			}
			long orthoCaseNum=OrthoCases.Insert(newOrthoCase);
			//Ortho Schedule
			OrthoSchedule newOrthoSchedule=new OrthoSchedule();
			newOrthoSchedule.BandingAmount=bandingAmount;
			newOrthoSchedule.DebondAmount=debondAmount;
			newOrthoSchedule.VisitAmount=visitAmount;
			newOrthoSchedule.IsActive=true;
			long orthoScheduleNum=OrthoSchedules.Insert(newOrthoSchedule);
			//Ortho Plan Link
			OrthoPlanLink newOrthoPlanLink=new OrthoPlanLink();
			newOrthoPlanLink.OrthoCaseNum=orthoCaseNum;
			newOrthoPlanLink.LinkType=OrthoPlanLinkType.OrthoSchedule;
			newOrthoPlanLink.FKey=orthoScheduleNum;
			newOrthoPlanLink.IsActive=true;
			newOrthoPlanLink.SecUserNumEntry=Security.CurUser.UserNum; 
			OrthoPlanLinks.Insert(newOrthoPlanLink);
			//Banding Proc Link
			if(!newOrthoCase.IsTransfer) {
				OrthoProcLinks.Insert(OrthoProcLinks.CreateHelper(orthoCaseNum,bandingProc.ProcNum,OrthoProcType.Banding));
			}
			return orthoCaseNum;
		}

		///<summary>Updates the OrthoCase, OrthoSchedule, and banding OrthoProcLink for an Ortho Case.</summary>
		public static void UpdateForFormOrthoCase(OrthoCase oldOrthoCase,OrthoSchedule oldOrthoSchedule,double fee,double feeInsPrimary
			,double feeInsSecondary,double feePat,DateTime bandingDate,bool isTransfer,DateTime debondDateExpected,double bandingAmount,double debondAmount
			,double visitAmount,Procedure bandingProc,Procedure debondProc,OrthoProcLink bandingProcLink)
		{
			//No remoting role check; no call to db
			OrthoCase newOrthoCase=oldOrthoCase.Copy();
			//OrthoCase
			newOrthoCase.Fee=fee;
			newOrthoCase.FeeInsPrimary=feeInsPrimary;
			newOrthoCase.FeeInsSecondary=feeInsSecondary;
			newOrthoCase.FeePat=feePat;
			newOrthoCase.BandingDate=bandingDate;
			newOrthoCase.IsTransfer=isTransfer;
			if(debondProc==null) {
				newOrthoCase.DebondDateExpected=debondDateExpected;
				newOrthoCase.DebondDate=DateTime.MinValue;
			}
			if(isTransfer && bandingProcLink!=null) {//OrthoCase has been changed to a transfer. Delete linked banding proc if it exists.
				OrthoProcLinks.Delete(bandingProcLink.OrthoProcLinkNum);
			}
			else if(!isTransfer) {
				if(bandingProcLink==null) {//OrthoCase has been changed to a non-transfer. Link banding proc.
					OrthoProcLinks.Insert(OrthoProcLinks.CreateHelper(newOrthoCase.OrthoCaseNum,bandingProc.ProcNum,OrthoProcType.Banding));
				}
				else {//OrthoCase is still a non-transfer, but banding proc linked may have changed so update.
					OrthoProcLink newBandingProcLink=bandingProcLink.Copy();
					newBandingProcLink.ProcNum=bandingProc.ProcNum;
					OrthoProcLinks.Update(newBandingProcLink,bandingProcLink);
				}
				if(bandingProc!=null && bandingProc.AptNum!=0) {//Banding proc may have changed, so check to see if it is scheduled and update bandingDate.
					newOrthoCase.BandingDate=bandingProc.ProcDate;
				}
			}
			OrthoCases.Update(newOrthoCase,oldOrthoCase);
			//OrthoSchedule
			OrthoSchedule newOrthoSchedule=oldOrthoSchedule.Copy();
			newOrthoSchedule.BandingAmount=bandingAmount;
			newOrthoSchedule.DebondAmount=debondAmount;
			newOrthoSchedule.VisitAmount=visitAmount;
			OrthoSchedules.Update(newOrthoSchedule,oldOrthoSchedule);
		}
	}
}
