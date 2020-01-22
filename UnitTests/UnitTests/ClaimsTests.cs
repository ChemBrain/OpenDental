using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnitTestsCore;

namespace UnitTests.Claims_Tests {
	[TestClass]
	public class ClaimsTests:TestBase {
		
		///<summary>This test is for making sure that writeoffs are correct even when given a preauth estimate before a claim estimate.</summary>
		[TestMethod]
		public void Claims_CalculateAndUpdate_PreauthOrderWriteoff() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			//create the patient and insurance information
			Patient pat=PatientT.CreatePatient(suffix);
			//proc - Crown
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2750",ProcStat.C,"8",1000);
			long feeSchedNum1=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			FeeT.CreateFee(feeSchedNum1,proc.CodeNum,900);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan insPlan=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum1);
			BenefitT.CreateAnnualMax(insPlan.PlanNum,1000);
			BenefitT.CreateCategoryPercent(insPlan.PlanNum,EbenefitCategory.Crowns,100);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,insPlan.PlanNum);
			PatPlan pp=PatPlanT.CreatePatPlan(1,pat.PatNum,sub.InsSubNum);
			//create lists and variables required for ComputeEstimates()
			List<InsSub> SubList=InsSubs.RefreshForFam(Patients.GetFamily(pat.PatNum));
			List<InsPlan> listInsPlan=InsPlans.RefreshForSubList(SubList);
			List<PatPlan> listPatPlan=PatPlans.Refresh(pat.PatNum);
			List<Benefit> listBenefits=Benefits.Refresh(listPatPlan,SubList);
			List<Procedure> listProcsForPat=Procedures.Refresh(pat.PatNum);
			List<Procedure> procsForClaim=new List<Procedure>();
			procsForClaim.Add(proc);
			//Create the claim and associated claimprocs
			//The order of these claimprocs is the whole point of the unit test.
			//Create Preauth
			ClaimProcs.CreateEst(new ClaimProc(),proc,insPlan,sub,0,500,true,true);
			//Create Estimate 
			ClaimProcs.CreateEst(new ClaimProc(),proc,insPlan,sub,1000,1000,true,false);
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			Claim claimWaiting=ClaimT.CreateClaim("W",listPatPlan,listInsPlan,listClaimProcs,listProcsForPat,pat,procsForClaim,listBenefits,SubList,false);
			Assert.AreEqual(100,claimWaiting.WriteOff,"WriteOff Amount");
		}

		///<summary></summary>
		[TestMethod]
		public void Claims_CalculateAndUpdate_Allowed1Allowed2CompletedProcedures() {
			string suffix="8";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			long feeSchedNum1=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			long feeSchedNum2=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix+"b");
			//Standard Fee
			long codeNum=ProcedureCodes.GetCodeNum("D2750");
			Fee fee=Fees.GetFee(codeNum,53,0,0);
			if(fee==null) {
				fee=new Fee();
				fee.CodeNum=codeNum;
				fee.FeeSched=53;
				fee.Amount=1200;
				Fees.Insert(fee);
			}
			else {
				fee.Amount=1200;
				Fees.Update(fee);
			}
			//PPO fees
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum1;
			fee.Amount=600;
			Fees.Insert(fee);
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum2;
			fee.Amount=800;
			Fees.Insert(fee);
			//Carrier
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long planNum1=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum1).PlanNum;
			long planNum2=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum2).PlanNum;
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,planNum1);
			long subNum1=sub1.InsSubNum;
			InsSub sub2=InsSubT.CreateInsSub(pat.PatNum,planNum2);
			long subNum2=sub2.InsSubNum;
			BenefitT.CreateCategoryPercent(planNum1,EbenefitCategory.Crowns,50);
			BenefitT.CreateCategoryPercent(planNum2,EbenefitCategory.Crowns,50);
			BenefitT.CreateAnnualMax(planNum1,1000);
			BenefitT.CreateAnnualMax(planNum2,1000);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			PatPlanT.CreatePatPlan(2,patNum,subNum2);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2750",ProcStat.TP,"8",Fees.GetAmount0(codeNum,53));//crown on 8
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<Procedure> procList=Procedures.Refresh(patNum);
			//Set complete and attach to claim
			ProcedureT.SetComplete(proc,pat,planList,patPlans,claimProcs,benefitList,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			List<Procedure> procsForClaim=new List<Procedure>();
			procsForClaim.Add(proc);
			Claim claim=ClaimT.CreateClaim("P",patPlans,planList,claimProcs,procList,pat,procsForClaim,benefitList,subList);
			//Validate
			Assert.AreEqual(500,claim.WriteOff);
		}

		///<summary>Downgrade insurance estimates #1. The PPO fee schedule has a blank fee for the downgraded code.</summary>
		[TestMethod]
		public void Claims_CalculateAndUpdate_ProcedureCodeDowngradeBlankFee() {
			string suffix="60";
			Patient pat=PatientT.CreatePatient(suffix);
			long ucrFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"UCR Fees"+suffix);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO Downgrades"+suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Restorative,100);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			ProcedureCode originalProcCode=ProcedureCodes.GetProcCode("D2393");
			ProcedureCode downgradeProcCode=ProcedureCodes.GetProcCode("D2160");
			originalProcCode.SubstitutionCode="D2160";
			originalProcCode.SubstOnlyIf=SubstitutionCondition.Always;
			ProcedureCodes.Update(originalProcCode);
			FeeT.CreateFee(ucrFeeSchedNum,originalProcCode.CodeNum,300);
			FeeT.CreateFee(ucrFeeSchedNum,downgradeProcCode.CodeNum,100);
			FeeT.CreateFee(ppoFeeSchedNum,originalProcCode.CodeNum,120);
			//No fee entered for D2160 in PPO Downgrades
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2393",ProcStat.C,"1",300);//Tooth 1
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			InsPlan insPlan = planList[0];//Should only be one
			InsPlan planOld = insPlan.Copy();//Should only be one
			insPlan.PlanType="p";
			insPlan.FeeSched=ppoFeeSchedNum;
			InsPlans.Update(insPlan,planOld);
			//Creates the claim in the same manner as the account module, including estimates.
			Claim claim=ClaimT.CreateClaim("P",patPlans,planList,claimProcs,ProcList,pat,ProcList,benefitList,subList);
			ClaimProc clProc=ClaimProcs.Refresh(pat.PatNum)[0];//Should only be one
			Assert.AreEqual(120,clProc.InsEstTotal);
			Assert.AreEqual(180,clProc.WriteOff);
		}

		///<summary>Downgrade insurance estimates #2. The PPO fee schedule has a higher fee for the downgraded code than for the original code.</summary>
		[TestMethod]
		public void Claims_CalculateAndUpdate_ProcedureCodeDowngradeHigherFee() {
			string suffix="61";
			Patient pat=PatientT.CreatePatient(suffix);
			long ucrFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"UCR Fees"+suffix);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO Downgrades"+suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Restorative,100);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			ProcedureCode originalProcCode=ProcedureCodes.GetProcCode("D2391");
			ProcedureCode downgradeProcCode=ProcedureCodes.GetProcCode("D2140");
			originalProcCode.SubstitutionCode="D2140";
			originalProcCode.SubstOnlyIf=SubstitutionCondition.Always;
			ProcedureCodes.Update(originalProcCode);
			FeeT.CreateFee(ucrFeeSchedNum,originalProcCode.CodeNum,140);
			FeeT.CreateFee(ucrFeeSchedNum,downgradeProcCode.CodeNum,120);
			FeeT.CreateFee(ppoFeeSchedNum,originalProcCode.CodeNum,80);
			FeeT.CreateFee(ppoFeeSchedNum,downgradeProcCode.CodeNum,100);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2391",ProcStat.C,"1",140);//Tooth 1
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			InsPlan insPlan=planList[0];//Should only be one
			InsPlan planOld = insPlan.Copy();
			insPlan.PlanType="p";
			insPlan.FeeSched=ppoFeeSchedNum;
			InsPlans.Update(insPlan,planOld);
			//Creates the claim in the same manner as the account module, including estimates.
			Claim claim=ClaimT.CreateClaim("P",patPlans,planList,claimProcs,ProcList,pat,ProcList,benefitList,subList);
			ClaimProc clProc=ClaimProcs.Refresh(pat.PatNum)[0];//Should only be one
			Assert.AreEqual(80,clProc.InsEstTotal);
			Assert.AreEqual(60,clProc.WriteOff);
		}

		///<summary></summary>
		[TestMethod]
		public void Claims_ValidatePOBoxAddress() {
			List<string> listValidPOBox=new List<string> {
				"PO Box 12345",
				"P O box 12345",
				"P. O. Box 12345",
				"P.O.Box 12345",
				"post box 12345",
				"post office box 12345",
				"P.O.B 12345",
				"P.O.B. 12345",
				"Post Box #12345",
				"Postal Box 12345",
				"P.O. Box 12345",
				"PO. Box 12345",
				"P.o box 12345",
				"Pobox 12345",
				"p.o. Box12345",
				"po-box12345",
				"p.o.-box 12345",
				"PO-Box 12345",
				"p-o-box 12345",
				"p-o box 12345",
				"box 12345",
				"Box12345",
				"Box-12345"
			};
			List<string> listInValidPOBox=new List<string> {
				"12345 Tapo Cannon Rd.",
				"12345 Tapo 1st Ave",
				"12345 Box Turtle Circle",
				"12345 Boxing Pass",
				"12345 Poblano Lane",
				"12345 P O Davis Drive",
				"12345 P O Boxing Drive",
				"12345 PO Boxing Drive",
				"12345 Postal Circle"
			};
			foreach(string address in listValidPOBox) {
				if(!X837_5010.HasPOBox(address)) {
					Assert.Fail(address);
				}
			}
			foreach(string address in listInValidPOBox) {
				if(X837_5010.HasPOBox(address)) {
					Assert.Fail(address);
				}
			}
		}

	}
}
