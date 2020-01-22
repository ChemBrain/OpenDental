using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpPPOwriteoff {
		///<summary></summary>
		public static DataTable GetWriteoffTable(DateTime dateStart,DateTime dateEnd,bool isIndividual,string carrierText, PPOWriteoffDateCalc writeoffType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateStart,dateEnd,isIndividual,carrierText,writeoffType);
			}
			string queryText="";
			//individual
			if(isIndividual) {
				queryText="SET @DateFrom="+POut.Date(dateStart)+", @DateTo="+POut.Date(dateEnd)
				+", @CarrierName='%"+POut.String(carrierText)+"%';";
				if(writeoffType==PPOWriteoffDateCalc.InsPayDate) {
					queryText+=@"SELECT claimproc.DateCP,
					CONCAT(CONCAT(CONCAT(CONCAT(patient.LName,', '),patient.FName),' '),patient.MiddleI),
					carrier.CarrierName,
					provider.Abbr,
					SUM(claimproc.FeeBilled),
					SUM(claimproc.FeeBilled-claimproc.WriteOff),
					SUM(claimproc.WriteOff),
					claimproc.ClaimNum
					FROM claimproc
					INNER JOIN insplan ON claimproc.PlanNum = insplan.PlanNum 
						AND insplan.PlanType='p'
					INNER JOIN patient ON claimproc.PatNum = patient.PatNum
					INNER JOIN carrier ON carrier.CarrierNum = insplan.CarrierNum 
						AND carrier.CarrierName LIKE @CarrierName
					INNER JOIN provider ON provider.ProvNum = claimproc.ProvNum
					WHERE claimproc.Status IN ("+(int)ClaimProcStatus.Received+","+(int)ClaimProcStatus.Supplemental+@") 
					AND claimproc.DateCP >= @DateFrom
					AND claimproc.DateCP <= @DateTo
					GROUP BY claimproc.ClaimNum 
					ORDER BY claimproc.DateCP";
				}
				else if(writeoffType==PPOWriteoffDateCalc.ProcDate){//use procedure date
					queryText+=@"SELECT claimproc.ProcDate,
					CONCAT(CONCAT(CONCAT(CONCAT(patient.LName,', '),patient.FName),' '),patient.MiddleI),
					carrier.CarrierName,
					provider.Abbr,
					SUM(claimproc.FeeBilled),
					SUM(claimproc.FeeBilled-claimproc.WriteOff),
					SUM(claimproc.WriteOff),
					claimproc.ClaimNum
					FROM claimproc
					INNER JOIN insplan ON claimproc.PlanNum = insplan.PlanNum 
						AND insplan.PlanType='p'
					INNER JOIN patient ON claimproc.PatNum = patient.PatNum
					INNER JOIN carrier ON carrier.CarrierNum = insplan.CarrierNum 
						AND carrier.CarrierName LIKE @CarrierName
					INNER JOIN provider ON provider.ProvNum = claimproc.ProvNum
					WHERE claimproc.Status IN ("+(int)ClaimProcStatus.Received+","+(int)ClaimProcStatus.Supplemental+","+(int)ClaimProcStatus.NotReceived+@") 
					AND claimproc.ProcDate >= @DateFrom
					AND claimproc.ProcDate <= @DateTo
					GROUP BY claimproc.ClaimNum 
					ORDER BY claimproc.ProcDate";
				}
				else  {//writeoffType==PPOWriteoffDateCalc.ClaimPayDate
					queryText+=@"SELECT claimsnapshot.DateTEntry,
					CONCAT(CONCAT(CONCAT(CONCAT(patient.LName,', '),patient.FName),' '),patient.MiddleI),
					carrier.CarrierName,
					provider.Abbr,
					SUM(claimproc.FeeBilled),
					SUM(claimproc.FeeBilled-claimsnapshot.WriteOff),
					SUM(claimsnapshot.WriteOff),
					claimproc.ClaimNum
					FROM claimproc
					INNER JOIN insplan ON insplan.PlanNum=claimProc.PlanNum 
						AND insplan.PlanType='p'
					INNER JOIN patient ON patient.PatNum=claimProc.PatNum
					INNER JOIN carrier ON carrier.CarrierNum=insplan.CarrierNum 
						AND carrier.CarrierName LIKE @CarrierName
					INNER JOIN provider ON provider.ProvNum=claimProc.ProvNum
					INNER JOIN claimsnapshot ON claimsnapshot.ClaimProcNum=claimproc.ClaimProcNum 
						AND claimsnapshot.DateTEntry BETWEEN @DateFrom AND @DateTo
					WHERE claimproc.Status IN ("+(int)ClaimProcStatus.Received+","+(int)ClaimProcStatus.Supplemental+","+(int)ClaimProcStatus.NotReceived+@") 
					GROUP BY claimproc.ClaimNum 
					ORDER BY claimsnapshot.DateTEntry";
				}
			}
			else {
				//group
				if(writeoffType==PPOWriteoffDateCalc.InsPayDate) {
					queryText="SET @DateFrom="+POut.Date(dateStart)+", @DateTo="+POut.Date(dateEnd)
						+", @CarrierName='%"+POut.String(carrierText)+"%';"
						+@"SELECT carrier.CarrierName,
						SUM(claimproc.FeeBilled),
						SUM(claimproc.FeeBilled-claimproc.WriteOff),
						SUM(claimproc.WriteOff),
						claimproc.ClaimNum
						FROM claimproc
						INNER JOIN insplan ON claimproc.PlanNum = insplan.PlanNum
							AND insplan.PlanType='p'
						INNER JOIN carrier ON carrier.CarrierNum = insplan.CarrierNum
							AND carrier.CarrierName LIKE @CarrierName
						WHERE claimproc.Status IN ("+(int)ClaimProcStatus.Received+","+(int)ClaimProcStatus.Supplemental+@") 
						AND claimproc.DateCP >= @DateFrom
						AND claimproc.DateCP <= @DateTo
						GROUP BY carrier.CarrierNum 
						ORDER BY carrier.CarrierName";
				}
				else if(writeoffType==PPOWriteoffDateCalc.ProcDate) {
					queryText="SET @DateFrom="+POut.Date(dateStart)+", @DateTo="+POut.Date(dateEnd)
						+", @CarrierName='%"+POut.String(carrierText)+"%';"
						+@"SELECT carrier.CarrierName,
						SUM(claimproc.FeeBilled),
						SUM(claimproc.FeeBilled-claimproc.WriteOff),
						SUM(claimproc.WriteOff),
						claimproc.ClaimNum
						FROM claimproc
						INNER JOIN insplan ON claimproc.PlanNum = insplan.PlanNum
							AND insplan.PlanType='p'
						INNER JOIN carrier ON carrier.CarrierNum = insplan.CarrierNum
							AND carrier.CarrierName LIKE @CarrierName
						WHERE claimproc.Status IN ("+(int)ClaimProcStatus.Received+","+(int)ClaimProcStatus.Supplemental+","+(int)ClaimProcStatus.NotReceived+@") 
						AND claimproc.ProcDate >= @DateFrom
						AND claimproc.ProcDate <= @DateTo
						GROUP BY carrier.CarrierNum 
						ORDER BY carrier.CarrierName";
				}
				else {	// writeoffType==PPOWriteoffDateCalc.ClaimPayDate
					queryText="SET @DateFrom="+POut.Date(dateStart)+", @DateTo="+POut.Date(dateEnd)
						+", @CarrierName='%"+POut.String(carrierText)+"%';"
						+@"SELECT carrier.CarrierName,
						SUM(claimproc.FeeBilled),
						SUM(claimproc.FeeBilled-claimproc.WriteOff),
						SUM(claimproc.WriteOff),
						claimproc.ClaimNum
						FROM claimproc
						INNER JOIN insplan on claimproc.PlanNum = insplan.PlanNum
							AND insplan.PlanType='p'
						INNER JOIN carrier on carrier.CarrierNum = insplan.CarrierNum
							AND carrier.CarrierName LIKE @CarrierName						
						INNER JOIN claimsnapshot on claimsnapshot.ClaimProcNum=claimproc.ClaimProcNum
							AND claimsnapshot.DateTEntry >= @DateFrom
							AND claimsnapshot.DateTEntry <= @DateTo
						WHERE claimproc.Status IN ("+(int)ClaimProcStatus.Received+","+(int)ClaimProcStatus.Supplemental+","+(int)ClaimProcStatus.NotReceived+@") 
						GROUP BY carrier.CarrierNum 
						ORDER BY carrier.CarrierName";
				}
			}
			return ReportsComplex.RunFuncOnReportServer(() => ReportsComplex.GetTable(queryText));
		}	
	}

}
