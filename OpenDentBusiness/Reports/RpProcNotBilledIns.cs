using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpProcNotBilledIns {
		///<summary>If not using clinics then supply an empty list of clinicNums.  listClinicNums must have at least one item if using clinics.
		///The table returned has the following columns in this order: 
		///PatientName, ProcDate, Descript, ProcFee, ProcNum, ClinicNum, PatNum, IsInProcess</summary>
		public static DataTable GetProcsNotBilled(List<long> listClinicNums,bool includeMedProcs,DateTime dateStart,DateTime dateEnd,
			bool showProcsBeforeIns,bool hasMultiVisitProcs)
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),listClinicNums,includeMedProcs,dateStart,dateEnd,showProcsBeforeIns,hasMultiVisitProcs);
			}
			string query="SELECT ";
			if(PrefC.GetBool(PrefName.ReportsShowPatNum)) {
				query+=DbHelper.Concat("CAST(PatNum AS CHAR)","'-'","LName","', '","FName","' '","MiddleI");
			}
			else {
				query+=DbHelper.Concat("LName","', '","FName","' '","MiddleI");
			}
			query+=" AS 'PatientName',Stat,ProcDate,Descript,procFee,ProcNum,ClinicNum,PatNum "
				+"FROM (SELECT patient.LName,patient.FName,patient.MiddleI,"
				+"CASE WHEN procmultivisit.ProcMultiVisitNum IS NULL "
					+"THEN '"+Lans.g("enumProcStat",ProcStat.C.ToString())+"' ELSE '"+Lans.g("enumProcStat",ProcStatExt.InProcess)+"' END Stat,"
				+"procedurelog.ProcDate,procedurecode.Descript,procedurelog.ProcFee*(procedurelog.UnitQty+procedurelog.BaseUnits) procFee,"
				+"procedurelog.ProcNum,procedurelog.ClinicNum,patient.PatNum,insplan.PlanNum,MAX(insplan.IsMedical) isMedical,"
				+"CASE WHEN MIN(insplan.IsMedical)=0 THEN 1 ELSE 0 END hasDental "
				+"FROM patient "
				+"INNER JOIN procedurelog ON procedurelog.PatNum = patient.PatNum "
					+"AND procedurelog.ProcFee>0 "
					+"AND procedurelog.procstatus="+(int)ProcStat.C+" "
					+"AND procedurelog.ProcDate	BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)+" "
				+"INNER JOIN procedurecode ON procedurecode.CodeNum = procedurelog.CodeNum ";
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {
				query+="AND procedurecode.IsCanadianLab=0 ";//ignore Canadian labs
			}
			query+="LEFT JOIN claimproc ON claimproc.ProcNum = procedurelog.ProcNum "
				+"LEFT JOIN insplan ON insplan.PlanNum = claimproc.PlanNum "
				+"LEFT JOIN procmultivisit ON procmultivisit.ProcNum=procedurelog.ProcNum AND procmultivisit.IsInProcess=1 "
				+"WHERE EXISTS(SELECT 1 FROM patplan WHERE patplan.PatNum=patient.PatNum) "
				+"AND ((claimproc.NoBillIns=0 AND claimproc.Status="+(int)ClaimProcStatus.Estimate+") ";
			if(showProcsBeforeIns) {
				query+="OR claimproc.ClaimProcNum IS NULL ";
			}
			query+=") ";
			if(!hasMultiVisitProcs) {
				query+="AND (procmultivisit.ProcMultiVisitNum IS NULL) ";
			}
			if(listClinicNums.Count>0) {
				query+="AND procedurelog.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			query+="GROUP BY procedurelog.ProcNum "
				+") procnotbilled ";//End of the main query which is treated like a sub query in order to process includeMedProcs and showProcsBeforeIns.
			//Having the "AND insplan.IsMedical=0" check within the WHERE clause of the main query causes slowness for large databases.
			//MySQL will freak out when looking for what index to use which causes full row scans to take place instead of simply filtering the results.
			//This problem can be resolved by putting the insplan.IsMedical=0 check into the LEFT JOIN clause and performing a corresponding NULL check.
			//However, the "OR insplan.PlanNum IS NULL" complicates the query enough to where it is easier to just put the old WHERE clause outside.
			//This sub query trick improved the following report for a large office from ~55 seconds to ~5 seconds.
			query+="WHERE (procnotbilled.hasDental=1 ";//Always include procedures when the patient has dental insurance.
			if(includeMedProcs) {
				query+="OR procnotbilled.isMedical=1 ";
			}
			if(showProcsBeforeIns) {
				query+="OR procnotbilled.PlanNum IS NULL ";
			}
			query+=") ORDER BY LName,FName,PatNum,ProcDate";
			return Db.GetTable(query);
		}

	}
}
