using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestsCore;
using OpenDentBusiness;
using System.Reflection;
using CodeBase;

namespace UnitTests.Payments_Tests {
	[TestClass]
	public class PaymentsTests:TestBase {
		#region Prepayment Test
		private List<PaySplit> _listFamPrePaySplits;
		private List<PaySplit> _listPosPrePay;
		private List<PaySplit> _listNegPrePay;
		#endregion

		[ClassInitialize]
		public static void SetUp(TestContext context) {

		}


		[TestMethod]
		public void PaymentEdit_ImplicitlyLinkCredits_NeverPayTPProc() {
			PrefT.UpdateBool(PrefName.PrePayAllowedForTpProcs,true);
			PrefT.UpdateBool(PrefName.AllowPrepayProvider,true);
			long provNumDoc=ProviderT.CreateProvider("DOC");
			long provNumHyg=ProviderT.CreateProvider("HYG");
			Patient pat1=PatientT.CreatePatient(lName:"Jones",fName:"Jane",priProvNum:provNumDoc,clinicNum:1,secProvNum:provNumHyg);
			//Patient2 is a family member of Patient1, who is Gaurantor
			Patient pat2=PatientT.CreatePatient(lName:"Jones",fName:"Jack",priProvNum:provNumDoc,clinicNum:1,secProvNum:provNumHyg,guarantor:pat1.PatNum);
			//Patient 1, has 3 C Procs and 1 TP proc
			Procedure pat1Proc1=ProcedureT.CreateProcedure(pat1,"D0150",ProcStat.C,"",139.10,DateTime.Today.AddDays(-5),0,0,provNumDoc);
			Procedure pat1Proc2=ProcedureT.CreateProcedure(pat1,"D0210",ProcStat.C,"",483.00,DateTime.Today.AddDays(-5),0,0,provNumDoc);
			Procedure pat1Proc3=ProcedureT.CreateProcedure(pat1,"D1110",ProcStat.C,"",174.80,DateTime.Today.AddDays(-5),0,0,provNumHyg);
			Procedure pat1Proc4=ProcedureT.CreateProcedure(pat1,"D1351",ProcStat.TP,"16",120.70,DateTime.Today,0,provNumDoc);
			//Patient2,  has 3 C Procs
			Procedure pat2Proc1=ProcedureT.CreateProcedure(pat2,"D0150",ProcStat.C,"",139.10,DateTime.Today.AddDays(-2),0,0,provNumDoc);
			Procedure pat2Proc2=ProcedureT.CreateProcedure(pat2,"D0210",ProcStat.C,"",483.00,DateTime.Today.AddDays(-2),0,0,provNumDoc);
			Procedure pat2Proc3=ProcedureT.CreateProcedure(pat2,"D1110",ProcStat.C,"",174.80,DateTime.Today.AddDays(-2),0,0,provNumHyg);
			//Patient2 has insurance
			Carrier carrier=CarrierT.CreateCarrier("ABC");
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub insSub=InsSubT.CreateInsSub(pat2.PatNum,plan.PlanNum);
			InsuranceInfo insInfo=InsuranceT.AddInsurance(pat2,carrier.CarrierName);
			//Patient2 has partially paid their procs with insurance claim, paid "As Total"
			ClaimProcT.AddInsPaidAsTotal(pat2.PatNum,plan.PlanNum,provNumDoc,398.45,insSub.InsSubNum,0,0);
			//Patient 2 pays of the rest of their balance while allocating to all but 1 proc
			Payment pat2attachedPayment1=PaymentT.MakePayment(pat2.PatNum,139.10,DateTime.Now,0,provNumDoc,pat2Proc1.ProcNum,0,1);
			Payment pat2attachedPayment2=PaymentT.MakePayment(pat2.PatNum,259.35,DateTime.Now,0,provNumDoc,pat2Proc2.ProcNum,0,1);
			//Patient 2 now has a zero balance
			Assert.AreEqual(0,pat2.EstBalance);
			//Patient 1 pays of the rest of their balance with an unallocated lump-sum payment
			Payment pat1unattachedPayment=PaymentT.MakePayment(pat1.PatNum,796.90,DateTime.Today.AddDays(-1),0,provNumDoc,0,1);
			//Patient 1 now has a zero balance
			Assert.AreEqual(0,pat1.EstBalance);
			Assert.AreEqual(0,pat1.BalTotal);//Ensure the family has a zero balance at this point
			//Create a zero charge payment to process the rest, use Guarantor
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat1.PatNum,0,DateTime.Now,true,0,1);
			//Create paysplits for Family
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat1,transferPayment,new List<long> {pat1.PatNum, pat2.PatNum},true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat1.PatNum, pat2.PatNum},pat1.PatNum
				,PaySplits.GetForPayment(transferPayment.PayNum),transferPayment.PayNum,false);
			PaymentEdit.ConstructResults chargeResult=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat1.PatNum, pat2.PatNum},pat1.PatNum
				,chargeData.ListPaySplits,transferPayment,new List<AccountEntry>());
			PaymentEdit.AutoSplitForPayment(chargeResult);
			List<AccountEntry> listTPProcs=chargeResult.ListAccountCharges.FindAll(//Create a list of all TP Procs in ListAccountCharges
				x => x.GetType()==typeof(Procedure) 
				&& ((Procedure)x.Tag).ProcStatus==ProcStat.TP
			);
			List<AccountEntry> listCProcs=chargeResult.ListAccountCharges.FindAll(//Create a list of all C Procs in ListAccountCharges
				x => x.GetType()==typeof(Procedure) 
				&& ((Procedure)x.Tag).ProcStatus==ProcStat.C
			);
			Assert.IsTrue(listTPProcs.Sum(x =>x.AmountEnd)!=0);//All TP PRocs have not been paid (so AmountEnd should not be 0) THIS LINE FAILS WITHOUT FIX
			Assert.IsTrue(listCProcs.Sum(x => x.AmountEnd)==0);//All C Procs are paid (so AmountEnd is 0)
		}


		#region Payment Tests - Payment Edit
		///<summary>Make sure auto splits go to each procedure for proper amounts.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_SplitForPaymentLessThanTotalofProcs() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",40);
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,50);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,new List<long> { pat.PatNum },true,false);
			PaymentEdit.ConstructResults chargeResult=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum },pat.PatNum
				,loadData.ConstructChargesData.ListPaySplits,pay,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplit=PaymentEdit.AutoSplitForPayment(chargeResult);
			Assert.AreEqual(2,autoSplit.ListAutoSplits.Count);
			Assert.AreEqual(1,autoSplit.ListAutoSplits.Count(x => x.SplitAmt.IsEqual(40)));
			Assert.AreEqual(1,autoSplit.ListAutoSplits.Count(x => x.SplitAmt.IsEqual(10)));
			Assert.AreEqual(2,autoSplit.ListAutoSplits.Count(x => x.UnearnedType==0));
		}

		///<summary>Make sure there are no negative auto splits created for an overpaid procedure.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_NoNegativeAutoSplits() {
			long provNumA=ProviderT.CreateProvider("provA");
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",70);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0150",ProcStat.C,"",20);
			//make an overpayment for one of the procedures so it spills over.
			DateTime payDate=DateTime.Today;
			Payment pay=PaymentT.MakePayment(pat.PatNum,71,payDate,procNum:proc1.ProcNum);//pre-existing payment
			//attempt to make another payment. Auto splits should not suggest a negative split.
			Payment newPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,2,payDate,isNew:true,
				payType:Defs.GetDefsForCategory(DefCat.PaymentTypes,true)[0].DefNum);//current payment we're trying to make
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,newPayment,new List<long>() {pat.PatNum },true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long> {pat.PatNum },pat.PatNum,
				PaySplits.GetForPayment(pay.PayNum),pay.PayNum,false);
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum },pat.PatNum
				,chargeData.ListPaySplits,newPayment,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplits=PaymentEdit.AutoSplitForPayment(constructResults);
			Assert.AreEqual(0,autoSplits.ListAutoSplits.FindAll(x => x.SplitAmt<0).Count);//assert no negative auto splits were made.
			Assert.AreEqual(0,autoSplits.ListSplitsCur.FindAll(x => x.SplitAmt<0).Count);//auto splits not catching everything
			Assert.AreEqual(0,autoSplits.ListAutoSplits.Count(x => x.UnearnedType!=0));
		}

		///<summary>Make sure auto splits go to payment plan charges for proper amounts and aren't marked as unearned.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_NoExtraZeroSplitsForPayPlanCharges() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",75);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0150",ProcStat.C,"",75);
			Procedure proc3=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",75);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,75,DateTime.Today.AddMonths(-4),0,new List<Procedure>() {proc1,proc2,proc3 });
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,100,DateTime.Today,true,1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,new List<long>() {pat.PatNum},true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat.PatNum},pat.PatNum
				,PaySplits.GetForPayment(pay.PayNum),pay.PayNum,false);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() {pat.PatNum},pat.PatNum,chargeData.ListPaySplits
				,pay,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplits=PaymentEdit.AutoSplitForPayment(results);
			//only two auto splits should exist. 1 covering the first whole payplancharge,and a second partial.
			Assert.AreEqual(2,autoSplits.ListAutoSplits.Count);
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count(x => x.SplitAmt.IsEqual(75)));
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count(x => x.SplitAmt.IsEqual(25)));
			Assert.AreEqual(2,autoSplits.ListAutoSplits.Count(x => x.UnearnedType==0));
		}

		///<summary>Make sure procedures are implicitly paid by unattached payment, and that a later procedure is paid by auto splits.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayments_ProceduresAndUnattachedPayments() {
			//Note, timing matters for this test. If all procedures are for today, the asserts will not be true.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			//make past procedures and pay them off with an unattached payment. Ok for prov, but do not link the procs.
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",55,DateTime.Today.AddDays(-2));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0150",ProcStat.C,"",65,DateTime.Today.AddDays(-2));
			Procedure proc3=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",75,DateTime.Today.AddDays(-2));
			Payment unattachedPayment=PaymentT.MakePayment(pat.PatNum,195,DateTime.Today.AddDays(-1));//no other fields because unattached.
			//make new procedure
			Procedure newProc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,DateTime.Today);
			Payment newPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,100);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,newPayment,new List<long>() {pat.PatNum},true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat.PatNum},pat.PatNum
				,PaySplits.GetForPayment(newPayment.PayNum),newPayment.PayNum,false);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() {pat.PatNum},pat.PatNum,chargeData.ListPaySplits
				,newPayment,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplits=PaymentEdit.AutoSplitForPayment(results);
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count); //should only be one procedure that still needs to be paid off. 
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count(x => x.SplitAmt.IsEqual(100)));
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count(x => x.UnearnedType==0));
			Assert.AreEqual(1,autoSplits.ListAccountCharges.Count(x => x.AmountStart.IsGreaterThanZero()));
		}

		///<summary>Make sure auto split will be for procedure even when payment plan payment covers more than payment plan charge (but less than proc total)</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_NoUnallocatedSplitsWhenPayPlanPaymentCoversMoreThanOneChargeInAPayment() {
			//Full split amount was being added to both charge's split collections during Explicit Linking
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("Prov"+MethodBase.GetCurrentMethod().Name);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0150",ProcStat.C,"",100,DateTime.Today.AddMonths(-2),provNum:provNum);
			//Payment Plan For The Procedure
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,25,DateTime.Today.AddMonths(-1),provNum,new List<Procedure> {proc});
			//Initial payment - important that it covers more than the first charge
			Payment firstPayment=PaymentT.MakePayment(pat.PatNum,31,DateTime.Today.AddDays(-1),payplan.PayPlanNum,provNum,proc.ProcNum,1);
			//Go to make another payment - 19 should be the current amount remaining on the payment plan. (25+25) - 31 = 19 
			Payment curPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,19,DateTime.Today,true,payType:1);//1 because not income txfr.
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,curPayment,new List<long>() {pat.PatNum },true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat.PatNum },pat.PatNum,
				loadData.ConstructChargesData.ListPaySplits,curPayment.PayNum,false);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,chargeData.ListPaySplits,
				curPayment,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplits=PaymentEdit.AutoSplitForPayment(results);
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count);
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count(x => x.SplitAmt.IsEqual(19)));
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count(x => x.UnearnedType==0));//make sure it's not unallocated.
		}
		
		///<summary>Make sure auto splits that are created are in the correct number and order (earliest proc paid first).</summary>
		[TestMethod]
		public void PaymentEdit_Init_CorrectlyOrderedAutoSplits() {//Legacy_TestFortyOne
			string suffix="41";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",40,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Now.AddDays(-3));
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,150);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment,new List<PaySplit>(),new List<AccountEntry>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			Assert.AreEqual(3,init.AutoSplitData.ListAutoSplits.Count);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[0].SplitAmt!=60 || init.AutoSplitData.ListAutoSplits[0].ProcNum!=procedure3.ProcNum);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[1].SplitAmt!=40 || init.AutoSplitData.ListAutoSplits[1].ProcNum!=procedure2.ProcNum);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[2].SplitAmt!=50 || init.AutoSplitData.ListAutoSplits[2].ProcNum!=procedure1.ProcNum);
		}

		///<summary>Make sure auto splits are created in correct number and order with an existing payment already present.</summary>
		[TestMethod]
		public void PaymentEdit_Init_CorrectlyOrderedAutoSplitsWithExistingPayment() {//Legacy_TestFortyTwo
			string suffix="42";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",60,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",80,DateTime.Now.AddDays(-3));
			Payment payment1=PaymentT.MakePayment(patNum,110,DateTime.Now.AddDays(-2));
			Payment payment2=PaymentT.MakePaymentNoSplits(patNum,80,DateTime.Today);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment2,new List<PaySplit>(),new List<AccountEntry>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain three paysplits, one for procedure1 for 40, and one for procedure2 for 30,
			//and an unallocated split for 10 with the remainder on the payment (40+30+10=80).
			Assert.AreEqual(3,init.AutoSplitData.ListAutoSplits.Count);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[0].SplitAmt!=30 || init.AutoSplitData.ListAutoSplits[0].ProcNum!=procedure2.ProcNum);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[1].SplitAmt!=40 || init.AutoSplitData.ListAutoSplits[1].ProcNum!=procedure1.ProcNum);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[2].SplitAmt!=10 || init.AutoSplitData.ListAutoSplits[2].ProcNum!=0);
		}

		///<summary>Make sure if existing procedures are overpaid with an unallocated payment that an additional payment doesn't autosplit to the procs.</summary>
		[TestMethod]
		public void PaymentEdit_Init_AutoSplitOverAllocation() {//Legacy_TestFortyThree
			string suffix="43";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",60,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",80,DateTime.Now.AddDays(-3));
			Payment payment1=PaymentT.MakePayment(patNum,200,DateTime.Now.AddDays(-2));
			Payment payment2=PaymentT.MakePaymentNoSplits(patNum,50,DateTime.Today);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment2,new List<PaySplit>(),new List<AccountEntry>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain one paysplit worth 50 and not attached to any procedures.
			Assert.AreEqual(1,init.AutoSplitData.ListAutoSplits.Count);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[0].SplitAmt!=50 || init.AutoSplitData.ListAutoSplits[0].ProcNum!=0);
		}

		///<summary>Make sure if a payment is created fo rnegative amount that it makes no auto splits.</summary>
		[TestMethod]
		public void PaymentEdit_Init_AutoSplitForPaymentNegativePaymentAmount() {//Legacy_TestFortyFour
			string suffix="44";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40,DateTime.Now.AddDays(-1));
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,-50,DateTime.Today);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment,new List<PaySplit>(),new List<AccountEntry>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain no paysplits since it doesn't make sense to create negative payments when there are outstanding charges.
			Assert.AreEqual(0,init.AutoSplitData.ListAutoSplits.Count);
		}

		///<summary>Make sure auto splits take into account unallocated adjustment, an overpayment on a procedure, 
		///and are attributed correctly to the remaining procedure with an unallocated split for the rest.</summary>
		[TestMethod]
		public void PaymentEdit_Init_AutoSplitWithAdjustmentAndExistingPayment() {//Legacy_TestFortyFive
			string suffix="45";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",60,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",80,DateTime.Now.AddDays(-3));
			Adjustment adjustment=AdjustmentT.MakeAdjustment(patNum,-40,procDate:DateTime.Now.AddDays(-2));
			Payment payment=PaymentT.MakePayment(patNum,100,DateTime.Now.AddDays(-2),procNum:procedure3.ProcNum);
			Payment payment2=PaymentT.MakePaymentNoSplits(patNum,50,DateTime.Today);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment2,new List<PaySplit>(),new List<AccountEntry>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain two paysplits, 40 attached to the D1110 and another for the remainder of 10, not attached to any procedure.
			Assert.AreEqual(2,init.AutoSplitData.ListAutoSplits.Count);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[0].SplitAmt!=40 || init.AutoSplitData.ListAutoSplits[0].ProcNum!=procedure1.ProcNum);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[1].SplitAmt!=10 || init.AutoSplitData.ListAutoSplits[1].ProcNum!=0);
		}

		///<summary>Make sure if there is a negative procedure and a negative payment amount that a new payment goes fully to unallocated.</summary>
		[TestMethod]
		public void PaymentEdit_Init_AutoSplitForPaymentNegativePaymentAmountNegProcedure() {//Legacy_TestFortySix
			string suffix="46";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",-40,DateTime.Now.AddDays(-1));
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,-50,DateTime.Today);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment,new List<PaySplit>(),new List<AccountEntry>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain one paysplit for the amount passed in that is unallocated.
			Assert.AreEqual(1,init.AutoSplitData.ListAutoSplits.Count);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[0].SplitAmt!=-50 || init.AutoSplitData.ListAutoSplits[0].ProcNum!=0);
		}

		///<summary>Make sure auto split suggestions go to the correct patients, for the correct amounts.</summary>
		[TestMethod]
		public void PaymentEdit_Init_AutoSplitProcedureGuarantor() {//Legacy_TestFortySeven
			string suffix="47";
			Patient pat=PatientT.CreatePatient(suffix);
			Patient patOld=PatientT.CreatePatient(suffix+"fam");
			Patient pat2=patOld.Copy();
			long patNum=pat.PatNum;
			pat2.Guarantor=patNum;
			Patients.Update(pat2,patOld);
			long patNum2=pat2.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat2,"D0120",ProcStat.C,"",40,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Now.AddDays(-3));
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,150,DateTime.Today);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment,new List<PaySplit>(),new List<AccountEntry>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			Assert.AreEqual(3,init.AutoSplitData.ListAutoSplits.Count);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[0].SplitAmt!=60 || init.AutoSplitData.ListAutoSplits[0].ProcNum!=procedure3.ProcNum 
				|| init.AutoSplitData.ListAutoSplits[0].PatNum!=patNum);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[1].SplitAmt!=40 || init.AutoSplitData.ListAutoSplits[1].ProcNum!=procedure2.ProcNum 
				|| init.AutoSplitData.ListAutoSplits[1].PatNum!=patNum2);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[2].SplitAmt!=50 || init.AutoSplitData.ListAutoSplits[2].ProcNum!=procedure1.ProcNum 
				|| init.AutoSplitData.ListAutoSplits[2].PatNum!=patNum);
		}

		///<summary>Make sure auto split suggestions take into account claim payments on procedures.</summary>
		[TestMethod]
		public void PaymentEdit_Init_AutoSplitWithClaimPayments() {//Legacy_TestFortyEight
			string suffix="48";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			InsPlan insPlan=InsPlanT.CreateInsPlan(CarrierT.CreateCarrier(suffix).CarrierNum);
			InsSub insSub=InsSubT.CreateInsSub(patNum,insPlan.PlanNum);
			PatPlan patPlan=PatPlanT.CreatePatPlan(1,patNum,insSub.InsSubNum);
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",40,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Now.AddDays(-3));
			ClaimProcT.AddInsPaid(patNum,insPlan.PlanNum,procedure1.ProcNum,20,insSub.InsSubNum,0,0);
			ClaimProcT.AddInsPaid(patNum,insPlan.PlanNum,procedure2.ProcNum,5,insSub.InsSubNum,5,0);
			ClaimProcT.AddInsPaid(patNum,insPlan.PlanNum,procedure3.ProcNum,20,insSub.InsSubNum,0,10);
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,150,DateTime.Today);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment,new List<PaySplit>(),new List<AccountEntry>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain four splits, 30, 35, and 30, then one unallocated for the remainder of the payment 55.
			Assert.AreEqual(4,init.AutoSplitData.ListAutoSplits.Count);
			//First auto split not not 30, not not procedure 3, and not not patNum
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[0].SplitAmt!=30 
				|| init.AutoSplitData.ListAutoSplits[0].ProcNum!=procedure3.ProcNum
				|| init.AutoSplitData.ListAutoSplits[0].PatNum!=patNum);
			//Second auto split not not 35, not not procedure 2, and not not patNum
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[1].SplitAmt!=35 
				|| init.AutoSplitData.ListAutoSplits[1].ProcNum!=procedure2.ProcNum
				|| init.AutoSplitData.ListAutoSplits[1].PatNum!=patNum);
			//Third auto split not not 30, not not procedure 1, and not not patNum
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[2].SplitAmt!=30 
				|| init.AutoSplitData.ListAutoSplits[2].ProcNum!=procedure1.ProcNum
				|| init.AutoSplitData.ListAutoSplits[2].PatNum!=patNum);
			//Fourth auto split not not 55, and not not procNum of 0
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[3].SplitAmt!=55
				|| init.AutoSplitData.ListAutoSplits[3].ProcNum!=0);
		}

		///<summary>Make sure that with a positive adjustment and the preference set to not prefer adjustments in FIFO logic that the first procedure is 
		///worth 55 at the end of auto splitting (was originally 75, and payment is for 20)</summary>
		[TestMethod]
		public void PaymentEdit_Init_FIFOWithPosAdjustment() {
			PrefT.UpdateInt(PrefName.AutoSplitLogic,(int)AutoSplitPreference.FIFO);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov1");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",75,DateTime.Today.AddMonths(-1),provNum:provNum);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",135,DateTime.Today.AddMonths(-1).AddDays(1),provNum:provNum);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat.PatNum,20,DateTime.Today.AddDays(-15),provNum:provNum);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,20);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payCur,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },payCur
					,loadData.ListSplits,new List<AccountEntry>(),pat.PatNum);
			//Verify the logic pays starts to pay off the first procedure
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(Procedure) && x.AmountEnd==55));
		}

		///<summary>Make sure that with a positive adjustment and the preference set to prefer adjustments in FIFO logic that the first item paid is 
		///an adjustment and that the adjustment (worth 20) is fully paid (by payment worth 20)</summary>
		[TestMethod]
		public void PaymentEdit_Init_AdjustmentPreferWithPosAdjustment() {
			PrefT.UpdateInt(PrefName.AutoSplitLogic,(int)AutoSplitPreference.Adjustments);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov1");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",75,DateTime.Today.AddMonths(-1),provNum:provNum);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",135,DateTime.Today.AddMonths(-1).AddDays(1),provNum:provNum);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat.PatNum,20,DateTime.Today.AddDays(-15),provNum:provNum);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,20);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payCur,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },payCur
					,loadData.ListSplits,new List<AccountEntry>(),pat.PatNum);
			//Verify the logic chooses to pay off the adjustment first
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(Adjustment) && x.AmountEnd==0));
		}

		///<summary>Make sure that if there are two procs and a payplan made with no procs attached that has a start date 4 months ago, 
		///the payment logic returns 6 owed charges, two of which are the procs and 4 of which are payplan charges.</summary>
		[TestMethod]
		public void PaymentEdit_Init_PayPlanChargesWithUnattachedCredits() {
			//new payplan
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",135,DateTime.Today.AddMonths(-4));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Today.AddMonths(-4));
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,DateTime.Today.AddMonths(-3),0,totalAmt:195);
			//Go to make a payment for the charges due
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,60,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,new List<long> {pat.PatNum},true,false);
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum },pat.PatNum
				,loadData.ConstructChargesData.ListPaySplits,pay,new List<AccountEntry>());
			Assert.AreEqual(6,constructResults.ListAccountCharges.Count);//2 procedures and 4 months of charges since unattached credits.
			Assert.AreEqual(2,constructResults.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(Procedure)));
			Assert.AreEqual(4,constructResults.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(PayPlanCharge)));
		}

		///<summary>Make sure that if there are two procs both attached to a payplan that has a start date of 4 months ago, that the payment logic
		///returns 4 owed charges, all four of which are payplan charges.</summary>
		[TestMethod]
		public void PaymentEdit_Init_PayPlanChargesWithAttachedCredits() {
			//new payplan
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",135,DateTime.Today.AddMonths(-4));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Today.AddMonths(-4));
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,DateTime.Today.AddMonths(-3),0,new List<Procedure>() {proc1,proc2});
			//Go to make a payment for the charges that are due
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,60,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,new List<long>{pat.PatNum},true,false);
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum },pat.PatNum
				,loadData.ConstructChargesData.ListPaySplits,pay,new List<AccountEntry>());
			Assert.AreEqual(4,constructResults.ListAccountCharges.FindAll(x => x.AmountStart>0).Count);//Procs shouldn't show - only the 4 pay plan charges
			Assert.AreEqual(4,constructResults.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(PayPlanCharge)));
		}

		///<summary>Make sure that if there are two procedures, a payment plan that has unattached credits (started 4 months ago), and two payments on the payment plan,
		///that our payment logic returns 4 charges that owe money, there are two charges that are procedures, and 4 charges that are payplan charges.</summary>
		[TestMethod]
		public void PaymentEdit_Init_ChargesWithUnattachedPayPlanCreditsWithPreviousPayments() {
			//new payplan
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long prov=ProviderT.CreateProvider("ProvA");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",135,DateTime.Today.AddMonths(-4),provNum:prov);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Today.AddMonths(-4).AddDays(1),provNum:prov);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,DateTime.Today.AddMonths(-3),prov,totalAmt:195);//totalAmt since unattached credits
			//Make initial payments.
			PaymentT.MakePayment(pat.PatNum,30,DateTime.Today.AddMonths(-2),payplan.PayPlanNum,prov,0,1);
			PaymentT.MakePayment(pat.PatNum,30,DateTime.Today.AddMonths(-1),payplan.PayPlanNum,prov,0,1);
			//Go to make another payment. 2 pay plan charges should have been "removed" (amtStart to 0) from being paid. 
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,30,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },pay
					,loadData.ListSplits,new List<AccountEntry>(),pat.PatNum);
			//2 procs and 2 pp charges
			Assert.AreEqual(4,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.AmountStart>0).Count);
			Assert.AreEqual(2,initData.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(Procedure)));
			Assert.AreEqual(4,initData.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(PayPlanCharge)));
		}

		///<summary>Make sure that a new payment goes to a procedure instead of a payplan due to the payplan being paid.</summary>
		[TestMethod]
		public void PaymentEdit_Init_PayPlansWithAttachedCreditsAndUnattachedProcedureOnPaySplits() {
			//Test create for instance when implicit linking was taking payment plan splits into account twice. In this test scenario it would lead
			//to the resulting procedure having a starting amount of 25 upon coming into the payment window when it should have been 75, the full amount.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",50);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,50,DateTime.Today.AddMonths(-1),0,new List<Procedure>() {proc1 });
			Payment paymentForPayPlan=PaymentT.MakePayment(pat.PatNum,50,DateTime.Today,payplan.PayPlanNum);//specifically testing when not attached to proc
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0150",ProcStat.C,"",75);
			Payment paymentForNewProc=PaymentT.MakePaymentNoSplits(pat.PatNum,75,DateTime.Today,true,1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,paymentForNewProc,new List<long> {pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },paymentForNewProc
				,loadData.ListSplits,new List<AccountEntry>(),pat.PatNum);
			//Verify the starting amount of the new procedure is correct and an autosplit exists for only the procedure, no unallocated.
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.AmountStart==75).Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAutoSplits.Count);
		}

		///<summary>Make sure if an adjustment is attached to a procedure that it affects the procedure's amount.</summary>
		[TestMethod]
		public void PaymentEdit_Init_AttachedAdjustment() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov1");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",135,DateTime.Today.AddMonths(-1).AddDays(1),provNum:provNum);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat.PatNum,20,DateTime.Today.AddDays(-15),provNum:provNum,procNum:proc.ProcNum);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,20);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payCur,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },payCur
					,loadData.ListSplits,new List<AccountEntry>(),pat.PatNum);
			//Verify there is only one charge (the procedure's charge + the adjustment for the amount original)
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(typeof(Procedure),initData.AutoSplitData.ListAccountCharges[0].Tag.GetType());
			Assert.AreEqual(135,initData.AutoSplitData.ListAccountCharges[0].AmountOriginal);
		}

		///<summary>Make sure that adjustments attached to a procedure affects its amount and unallocated splits reduce the procedure's end amount to 0.</summary>
		[TestMethod]
		public void PaymentEdit_Init_UnattachedPaymentsAndAttachedAdjustments() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov1");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",135,DateTime.Today.AddMonths(-1).AddDays(1),provNum:provNum);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat.PatNum,20,DateTime.Today.AddDays(-15),provNum:provNum,procNum:proc.ProcNum);
			Payment existingPayment1=PaymentT.MakePayment(pat.PatNum,35,DateTime.Today.AddDays(-1));//no prov or proc because it's unattached.
			Payment existingPayment2=PaymentT.MakePayment(pat.PatNum,20,DateTime.Today.AddDays(-1));
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,100);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payCur,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },payCur
					,loadData.ListSplits,new List<AccountEntry>(),pat.PatNum);
			//Verify there is only one charge (the procedure's charge + the adjustment for the amount original)
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(typeof(Procedure),initData.AutoSplitData.ListAccountCharges[0].Tag.GetType());
			Assert.AreEqual(0,initData.AutoSplitData.ListAccountCharges[0].AmountEnd);
		}

		///<summary>Make sure that if a procedure isn't paid explicitly that income transfer mode displays all things separately (procedures and paysplit).</summary>
		[TestMethod]
		public void PaymentEdit_Init_IncomeTransferWhenIncomeIncorrectlyAllocatedToOneProvider() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum1=ProviderT.CreateProvider("prov1");
			long provNum2=ProviderT.CreateProvider("prov2");
			Procedure procByProv1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",100,DateTime.Today.AddMonths(-1),provNum:provNum1);
			Procedure procByProv2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",50,DateTime.Today.AddMonths(-1),provNum:provNum2);
			Payment payAllForOneProv=PaymentT.MakePayment(pat.PatNum,150,DateTime.Today.AddDays(-1),provNum:provNum1,payType:1);//make entire payment to prov1
			//make an income transfer and see if it catches the over and underallocations
			Payment incomeTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0,isNew:true);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,incomeTransfer,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },incomeTransfer
					,loadData.ListSplits,new List<AccountEntry>(),pat.PatNum,isIncomeTxfr:true);
			//Assert there the appropriate amounts going to the correct providers.
			Assert.AreEqual(2,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.Tag.GetType()==typeof(Procedure)).Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.Tag.GetType()==typeof(PaySplit)).Count);
			//FormPayment.FillGridCharges will sum up the charges depending on what grouping is selected. Here we are just going to validate the output of
			//PaymentEdit.Init is as it should be.
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.AmountStart==-150 && x.ProvNum==provNum1).Count);
		}

		///<summary>Make sure in income transfer mode that if nothing is explicitly attached, all charges/credits show in the list.</summary>
		[TestMethod]
		public void PaymentEdit_Init_IncomeTransferWhenAdjustmentsIncorrectlyAllocatedToOneProvider() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum1=ProviderT.CreateProvider("prov1");
			long provNum2=ProviderT.CreateProvider("prov2");
			Adjustment procForProv1=AdjustmentT.MakeAdjustment(pat.PatNum,100,DateTime.Today.AddMonths(-1),provNum:provNum1);
			Adjustment procForProv2=AdjustmentT.MakeAdjustment(pat.PatNum,150,DateTime.Today.AddMonths(-1),provNum:provNum2);
			Payment payAllForOneProv=PaymentT.MakePayment(pat.PatNum,250,DateTime.Today.AddDays(-1),provNum:provNum1,payType:1);//make entire payment to prov1
			//make an income transfer and see if it catches the over and underallocations
			Payment incomeTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0,isNew:true);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,incomeTransfer,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },incomeTransfer
					,loadData.ListSplits,new List<AccountEntry>(),pat.PatNum,isIncomeTxfr:true);
			//Assert there the appropriate amounts going to the correct providers.
			Assert.AreEqual(2,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.Tag.GetType()==typeof(Adjustment)).Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.Tag.GetType()==typeof(PaySplit)).Count);
			//FormPayment.FillGridCharges will sum up the charges depending on what grouping is selected. Here we are just going to validate the output of
			//PaymentEdit.Init is as it should be.
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.AmountStart==-250 && x.ProvNum==provNum1).Count);
		}

		[TestMethod]
		public void PaymentEdit_Init_IncomeTransferPreDateTransfer() {
			//Create procedure for date X, create payment (unallocated) for date X.  
			//Create transfer to transfer from unallocated to procedure, but on date X-Y, where Y is some time.  
			//There should be no charges for transfer.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum1=ProviderT.CreateProvider("prov1");
			Procedure procByProv1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",100,DateTime.Today.AddMonths(-1),provNum:provNum1);
			//Create an income transfer to transfer from the total payment to the proc, but make it for prior to proc/payment date
			PaySplit prePay=PaySplitT.CreatePrepayment(pat.PatNum,100,DateTime.Today.AddMonths(-1),provNum1,0);
			Payment incomeTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0,isNew:true,payDate:DateTime.Today.AddMonths(-2));
			PaySplit negSplit=PaySplitT.CreateSplit(0,pat.PatNum,incomeTransfer.PayNum,0,DateTime.Today.AddMonths(-2),0,provNum1,-100,0
				,prePay.SplitNum);//negative transfer
			PaySplitT.CreateSplit(0,pat.PatNum,incomeTransfer.PayNum,0,DateTime.Today.AddMonths(-2),procByProv1.ProcNum,procByProv1.ProvNum,100,0
				,negSplit.SplitNum);//Positive allocation
			Payment incomeTransfer2=PaymentT.MakePaymentNoSplits(pat.PatNum,0,isNew:true,payDate:DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,incomeTransfer2,new List<long>{pat.PatNum},true,false);
			//we no longer implicitly link, so we need to attempt to make an actual transfer and verify that nothing happened. 
			Payment txfrPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.IncomeTransferData results=PaymentT.IncomeTransfer(pat.PatNum,Patients.GetFamily(pat.PatNum),txfrPayment,new List<PayPlanCharge>());
			Assert.AreEqual(0,results.ListSplitsCur.Count);
		}

		///<summary>Make sure that payment logic takes into account base units.</summary>
		[TestMethod]
		public void PaymentEdit_Init_ProcedureWithBaseUnits() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure procWithBaseUnits=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",100,baseUnits:1);//1 proc fee is $100, so total should be $200.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,200);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payCur,new List<long> {pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },payCur,
				loadData.ListSplits,new List<AccountEntry> {new AccountEntry(procWithBaseUnits) },pat.PatNum);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.AmountOriginal==200 && x.AmountStart==200 && x.AmountEnd==0).Count);
		}

		///<summary>Make sure that two procedures on a payment plan, with payments attached to the plan, that there are two charges requiring payment still.</summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_ChargesWithAttachedPayPlanCreditsWithPreviousPayments() {
			//new payplan
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",135,DateTime.Today.AddMonths(-4));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Today.AddMonths(-4));
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,DateTime.Today.AddMonths(-3),0,new List<Procedure>() {proc1,proc2});
			//Procedures's amount start should now be 0 from being attached. Make initial payments.
			PaymentT.MakePayment(pat.PatNum,30,DateTime.Today.AddMonths(-2),payplan.PayPlanNum,procNum:proc1.ProcNum);
			PaymentT.MakePayment(pat.PatNum,30,DateTime.Today.AddMonths(-1),payplan.PayPlanNum,procNum:proc1.ProcNum);
			//2 pay plan charges should have been removed from being paid. Make a new payment. 
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,30,DateTime.Today,isNew:true,payType:1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },pay
					,loadData.ListSplits,new List<AccountEntry>(),pat.PatNum);
			//should only see 2 pay plan charges that have not been paid, along with 2 pay plan charges that have been paid. 
			Assert.AreEqual(2,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.AmountStart>0).Count);
			Assert.AreEqual(4,initData.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(PayPlanCharge)));
		}

		///<summary>Make sure that if there is a positive adjustment (and only that) that auto split logic will make a split to it.</summary>
		[TestMethod]
		public void PaymentEdit_MakePayment_Adjustment() {
			//equivalent of being in the payment window and then hitting the 'pay' button to move a charge over from outstanding to current list of splits.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat.PatNum,100,DateTime.Today.AddDays(-1));
			Payment payment=PaymentT.MakePaymentNoSplits(pat.PatNum,100,DateTime.Today,true,1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payment,new List<long> {pat.PatNum },true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long> {pat.PatNum },pat.PatNum,
				PaySplits.GetForPayment(payment.PayNum),payment.PayNum,false);
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum },pat.PatNum
				,chargeData.ListPaySplits,payment,new List<AccountEntry>());
			List<List<AccountEntry>> listListAE=new List<List<AccountEntry>>();
			listListAE.Add(constructResults.ListAccountCharges);
			PaymentEdit.PayResults results=PaymentEdit.MakePayment(listListAE,PaySplits.GetForPayment(payment.PayNum),false,payment,0,false,100,
				constructResults.ListAccountCharges);
			Assert.AreEqual(0,results.ListAccountCharges.FindAll(x => x.AmountEnd>0).Count);//no more splits should need to be paid off.
			Assert.AreEqual(1,results.ListSplitsCur.FindAll(x => x.SplitAmt==100).Count);//Adjustment should now be paid in full. 
		}

		///<summary>Make sure that if there is a 100 prepayment and a procedure for 50, that allocating unearned allocates properly and unearned amount total
		///is correct.</summary>
		[TestMethod]
		public void PaymentEdit_AllocateUnearned_LinkToOriginalPrepayment() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			//create prepayment of $100
			long provNum=ProviderT.CreateProvider("SG");
			Clinic clinic1=ClinicT.CreateClinic("Clinic1");
			Family fam=Patients.GetFamily(pat.PatNum);
			//create original prepayment.
			PaySplit prePay=PaySplitT.CreatePrepayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum,clinic1.ClinicNum);
			//complete a procedure
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,provNum:provNum);
			//Setup to run the PaymentEdit.AllocateUnearned
			List<PaySplit> listPaySplits=new List<PaySplit>();
			List<PaySplit> listFamPrePaySplits=PaySplits.GetPrepayForFam(fam);
			//Unearned amount should be $100.
			double unearnedAmt=(double)PaySplits.GetUnearnedForFam(fam,listFamPrePaySplits);
			Assert.AreEqual(100,unearnedAmt);
			//Create the payment we will use to allocate some of the $100 prepayment.
			Payment pay=PaymentT.MakePaymentForPrepayment(pat,clinic1);
			//Run the AllocateUnearned method. This a list of paysplitAssociated.
			//The ref list of paysplits should also have the new paysplits that are associated to the original prepayment.
			List<PaySplits.PaySplitAssociated> listPaySplitAssociated=PaymentEdit.AllocateUnearned(new List<AccountEntry> { new AccountEntry(proc1) },ref listPaySplits,pay,unearnedAmt,fam);
			//Insert the paysplits and link the prepayment paysplits. This is similar to what is done when a user creates a payment from FormPayment.cs.
			PaySplitT.InsertPrepaymentSplits(listPaySplits,listPaySplitAssociated);
			//The ref list of paysplits should have the correct allocated prepayment amount. 
			Assert.AreEqual(-50,listPaySplits.Where(x => x.UnearnedType!=0).Sum(x => x.SplitAmt));
			//Create new procedure
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",100,provNum:provNum);
			//Now do what we just did again for a new procedure. The unallocated amount should be $50.
			listFamPrePaySplits=PaySplits.GetPrepayForFam(fam);
			unearnedAmt=(double)PaySplits.GetUnearnedForFam(fam,listFamPrePaySplits);
			Assert.AreEqual(50,unearnedAmt);
			List<PaySplit> listPaySplitsUnearned=new List<PaySplit>();
			pay=PaymentT.MakePaymentForPrepayment(pat,clinic1);
			List<PaySplits.PaySplitAssociated> retVal=PaymentEdit.AllocateUnearned(new List<AccountEntry> { new AccountEntry(proc2) },ref listPaySplitsUnearned,pay,unearnedAmt,fam);
			Assert.AreEqual(2,retVal.Count);
			Assert.AreEqual(-50,listPaySplitsUnearned.Where(x => x.UnearnedType!=0).Sum(x => x.SplitAmt));
		}

		///<summary>Make sure that when a prepayment is allocated incorrectly (not linked) that it will allocate the correct partial amount later.</summary>
		[TestMethod]
		public void PaymentEdit_AllocateUnearned_NoLinkToOriginalPrepayment() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			//create prepayment of $100
			long provNum=ProviderT.CreateProvider("SG");
			Clinic clinic1=ClinicT.CreateClinic("Clinic1");
			//create original prepayment.
			PaySplit prePay=PaySplitT.CreatePrepayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum,clinic1.ClinicNum);
			//complete a procedure
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,provNum:provNum);
			//Manually allocate prepayment without linking to the original prepayment.
			//We want to do it manually so we don't link this to the orginal prepayment correctly.
			//Not linking correctly will test that the AllocateUnearned method is implicitly linking prepayments correctly.
			PaySplitT.CreatePaySplitsForPrepayment(proc1,50,prov: provNum,clinic: clinic1);
			//Create new procedure
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",100,provNum:provNum);
			//test the PaymentEdit.AllocateUnearned() method.
			Family fam=Patients.GetFamily(pat.PatNum);
			List<PaySplit> listFamPrePaySplits=PaySplits.GetPrepayForFam(fam);
			//Should be $100
			double unearnedAmt=(double)PaySplits.GetUnearnedForFam(fam,listFamPrePaySplits);
			Assert.AreEqual(100,unearnedAmt);
			List<PaySplit> listPaySplitsUnearned=new List<PaySplit>();
			Payment pay=PaymentT.MakePaymentForPrepayment(pat,clinic1);
			List<PaySplits.PaySplitAssociated> retVal=PaymentEdit.AllocateUnearned(new List<AccountEntry> { new AccountEntry(proc2) },ref listPaySplitsUnearned,pay,unearnedAmt,fam);
			Assert.AreEqual(2,retVal.Count);
			//After running the AllocateUnearned, we should implicitly link the incorrect prepayment made when we call CreatePaySplitsForPrepayment above.
			Assert.AreEqual(-50,listPaySplitsUnearned.Where(x => x.UnearnedType!=0).Sum(x => x.SplitAmt));
		}

		///<summary>Various tests for allocating unearned under different circumstances (linked and unlinked splits, matching or mismatching pat/prov/clinic)</summary>
		[TestMethod]
		public void PaymentEdit_ImplicitlyLinkPrepaymentsHelper() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Patient patFam=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Patient patFamOld=patFam.Copy();
			patFam.Guarantor=pat.PatNum;
			Patients.Update(patFam,patFamOld);
			//create prepayment of $100
			long provNum=ProviderT.CreateProvider("SG");
			Clinic clinic1=ClinicT.CreateClinic("Clinic1");
			//create original prepayment.
			PaySplit prePay=PaySplitT.CreatePrepayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum,clinic1.ClinicNum);
			//complete a procedure
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,provNum:provNum);
			//Manually allocate prepayment without linking to the original prepayment.
			List<PaySplit> listPaySplits=PaySplitT.CreatePaySplitsForPrepayment(proc1,50,prov:provNum,clinic:clinic1);
			ResetPrepayments(pat);
			long nonMatch=100000;
			//test the PaymentEdit.AllocateUnearned() method.
			double unearnedAmt=(double)PaySplits.GetUnearnedForFam(Patients.GetFamily(pat.PatNum),_listFamPrePaySplits);
			//Logic check PatNum - match, ProvNum - match, ClinicNum - match
			double retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - match, ClinicNum - zero
			ResetPrepayments(pat);
			//update the clinicnum to 0
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,provNum,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//previous Test one should be $100
			ResetPrepayments(pat);
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(100,retVal);
			//Logic check PatNum - match, ProvNum - match, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,provNum,100000,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch: true,isProvNumMatch: true,isClinicNonZeroNonMatch: true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,0,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch: true,isProvNumZero:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,0,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumZero:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - non zero & non match 
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,0,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumZero:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - match 
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,nonMatch,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,nonMatch,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,nonMatch,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,provNum,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,provNum,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,provNum,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - zero, ClinicNum - match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,0,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - zero, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,0,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic checkPatNum - other family members, ProvNum - zero, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,0,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
			//Old test from above
			ResetPrepayments(pat);
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(100,retVal);
			//Logic checkPatNum - other family members, ProvNum - non zero & non match, ClinicNum - match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,nonMatch,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - non zero & non match, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,nonMatch,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - non zero & non match, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,nonMatch,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
		}

		///<summary>Make sure that when a payment plan is closed out but partially paid that procedures still calculate as owing correct amount</summary>
		[TestMethod]
		public void PaymentEdit_ExplicitlyLinkCredits_ShowCorrectAmountNeedingPaymentWhenPaymentPlanV2IsClosedAndPartiallyPaid() {
			//Explicitly Link Credits is the method that will contain the method being tested, but test will call Init to run through the whole gambit.
			PrefT.UpdateInt(PrefName.PayPlansVersion,(int)PayPlanVersions.AgeCreditsAndDebits);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",100,DateTime.Today.AddMonths(-4));
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,DateTime.Today.AddMonths(-3),0,new List<Procedure>() {proc});
			PaymentT.MakePayment(pat.PatNum,30,DateTime.Today.AddMonths(-2),payplan.PayPlanNum,procNum:proc.ProcNum);
			List<PayPlanCharge> listCharges=PayPlanCharges.GetForPayPlan(payplan.PayPlanNum);
			listCharges.Add(PayPlanEdit.CloseOutPatPayPlan(listCharges,payplan,DateTime.Today));
			listCharges.RemoveAll(x => x.ChargeDate > DateTime.Today);
			payplan.IsClosed=true;
			PayPlans.Update(payplan);
			PayPlanCharges.Sync(listCharges,payplan.PayPlanNum);
			Payment currentPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,60,DateTime.Today,true,1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,currentPayment,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },currentPayment
					,loadData.ListSplits,new List<AccountEntry>(),pat.PatNum);
			//only one procedure should come up as needing payment
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.AmountStart>0 && x.GetType()==typeof(Procedure)).Count);
			//procedure amount start should be reduced by the amount of the previous payments for the procedure. 
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.AmountStart==70).Count);
		}

		/// <summary>Make sure that discount plan amounts are considered for treatment planned procs and, if there is no discount plan amount, 
		/// it uses the existing fee on the procedure</summary>
		[TestMethod]
		public void PaymentEdit_ExplicityLinkCredits_DiscountPlanAmountsForTreatmentPlannedProcedures() {
			//Setup
			decimal discountAmount=50;
			decimal startingProcFee=100;
			long discSched=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Discount");
			DiscountPlan dp=DiscountPlanT.CreateDiscountPlan("Discount", feeSchedNum:discSched);
			Patient pat1=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Patient updatedPat1=pat1.Copy();
			updatedPat1.DiscountPlanNum=dp.DiscountPlanNum;
			Patients.Update(updatedPat1,pat1);
			Procedure proc1=ProcedureT.CreateProcedure(pat1,"D0120",ProcStat.TP,"0",(double)startingProcFee);
			Procedure proc2=ProcedureT.CreateProcedure(pat1,"D0150",ProcStat.TP,"0",(double)startingProcFee);
			FeeT.CreateFee(discSched,proc1.CodeNum,(double)discountAmount);
			Payment payment=PaymentT.MakePayment(pat1.PatNum,10,DateTime.Now);
			List<AccountEntry> listAccountEntries=new List<AccountEntry>() { new AccountEntry(proc1), new AccountEntry(proc2) };
			PaymentEdit.ConstructResults construct=new PaymentEdit.ConstructResults();
			construct.ListAccountCharges=listAccountEntries;
			construct.Payment=payment;
			//Testing
			List<AccountEntry> testResults=PaymentEdit.ExplicitlyLinkCredits(construct,new SplitCollection(),new List<ClaimProc>(),new List<Adjustment>(),new List<PayPlan>(),new List<PayPlanLink>());
			Assert.AreEqual(discountAmount,testResults[0].AmountStart);
			Assert.AreEqual(startingProcFee,testResults[1].AmountStart);
		}

		///<summary>Make sure that if a payment plan has a payment plan adjustment that the procedure owes the correct amount.</summary>
		[TestMethod]
		public void PaymentEdit_Init_ShowCorrectAmountNeedingPaymentOnChargeWhenPaymentPlanAdjustmentsExistForThePayPlanCharge() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",100,DateTime.Today.AddMonths(-3));
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,100,DateTime.Today.AddMonths(-3),0,new List<Procedure> {proc1 });
			List<PayPlanCharge> listChargesAndCredits=PayPlanCharges.GetForPayPlan(payplan.PayPlanNum);
			listChargesAndCredits=PayPlanEdit.CreatePayPlanAdjustments(-60,listChargesAndCredits,0);//create a 60 adjustment for the $100 charge.
			PayPlans.Update(payplan);
			PayPlanCharges.Sync(listChargesAndCredits,payplan.PayPlanNum);
			Payment paymentCur=PaymentT.MakePaymentNoSplits(pat.PatNum,40,DateTime.Today,true);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,paymentCur,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },paymentCur
					,loadData.ListSplits,new List<AccountEntry>(),pat.PatNum);
			//the charge that will show in oustanding charges when the user goes to make a payment should show that only $40 (100 proc - 60 adj) is due
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.GetType()==typeof(PayPlanCharge) 
				&& x.AmountOriginal==100 && x.AmountStart==40).Count);

		}

		///<summary>Make sure if there are claims paid by total that they are implicitly used by a valid transfer (neg split -> pos split).</summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_NoClaimProcsInIncomeTransfer() {
			//Make procedure for Provider A worth 50.
			//Make claimproc paid by total for Provider B for 50.
			//Perform an income transfer - It should display the claimproc as a source of income and the procedure as a destination.
			//Create the income transfer (manually create a -50 for Prov B, and create a +50 on procedure for ProvA)
			//Once complete, perform an income transfer again - There should be no targets for transfer since the procedure is paid properly and claimproc is counteracted by transfer.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provA=ProviderT.CreateProvider("ProvA");
			long provB=ProviderT.CreateProvider("ProvB");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",50,provNum:provA);
			Carrier carrier=CarrierT.CreateCarrier("ABC");
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub insSub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			ClaimProcT.AddInsPaidAsTotal(pat.PatNum,plan.PlanNum,provB,50,insSub.InsSubNum,0,0);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			//Make sure that the logic creates two charges - One for the procedure (original, start, and end are 100) and one for the claimproc paid by total (original, start, and end are -50).
			Assert.AreEqual(0,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(ClaimProc)).Count);
		}

		///<summary>Make sure if there are unattached adjustments that they are implicitly used by a valid previous transfer (neg split -> pos split).</summary>
		[TestMethod]
		public void PaymentEdit_ExplicitlyLinkAdjustmentTransfers() {
			//Make procedure for Provider A worth 50.
			//Make unattached negative adjustment Provider B for 50.
			//Perform an income transfer - It should display the adjustment as a source of income and the procedure as a destination.
			//Create the income transfer (manually create a -50 for Prov B, and create a +50 on procedure for ProvA)
			//Once complete, perform an income transfer again - There should be nothing since the procedure is paid properly and the transfer should counteract the adjustment.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provA=ProviderT.CreateProvider("ProvA");
			long provB=ProviderT.CreateProvider("ProvB");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",50,provNum:provA);
			Adjustment adjust=AdjustmentT.MakeAdjustment(pat.PatNum,-50,proc.ProcDate,provNum:provB);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(Procedure) && x.AmountOriginal==50 && x.AmountStart==50 && x.AmountEnd==50).Count);
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(Adjustment) && x.AmountOriginal==-50 && x.AmountStart==-50 && x.AmountEnd==-50).Count);
			//Create income transfer manually (cuz test)
			PaySplit negSplit=PaySplitT.CreateSplit(proc.ClinicNum,pat.PatNum,payCur.PayNum,0,proc.ProcDate,0,provB,-50,0);//Negative split for adjustment
			PaySplit posSplit=PaySplitT.CreateSplit(proc.ClinicNum,pat.PatNum,payCur.PayNum,0,proc.ProcDate,proc.ProcNum,provA,50,0);//Positive split for correct provider on the procedure
			Payment payCur2=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			//make an income transfer using the manager
			Payment txfrPay=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.IncomeTransferData txfr=PaymentT.BalanceAndIncomeTransfer(pat.PatNum,Patients.GetFamily(pat.PatNum),txfrPay);
			Assert.AreEqual(0,txfr.ListSplitsCur.FindAll(x => x.ProcNum!=0).Count);//Proc has previously been paid
			Assert.AreEqual(2,txfr.ListSplitsCur.Count);//2 splits to attach negative adj to the unearned
			Assert.AreEqual(1,txfr.ListSplitsCur.FindAll(x => x.AdjNum!=0).Count);
			//insert the results from the transfer into the database. Then attempt to make another transfer. Nothing should be transferred this time.
			foreach(PaySplit split in txfr.ListSplitsCur) {
				PaySplits.Insert(split);
			}
			foreach(PaySplits.PaySplitAssociated associateSplit in txfr.ListSplitsAssociated) {
				associateSplit.PaySplitLinked.FSplitNum=associateSplit.PaySplitOrig.SplitNum;
				PaySplits.Update(associateSplit.PaySplitLinked);
			}
			Payment secondTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			txfr=PaymentT.IncomeTransfer(pat.PatNum,Patients.GetFamily(pat.PatNum),secondTransfer,new List<PayPlanCharge>());
			Assert.AreEqual(0,txfr.ListSplitsCur.Count);
		}

		///<summary>Make sure if a procedure has been paid by an incorrect provider and a negative unallocated split was made to counteract that payment that
		///the procedure shows as owing money still.</summary>
		[TestMethod]
		public void PaymentEdit_UnlinkedIncomeXferShowsProcOwingMoney() {
			//Make a procedure for Provider A
			//Make a payment on that procedure (in full) for Provider B
			//"Correct" the mistake by creating a negative split for Provider B, but unattached
			//When making another payment it should show the procedure as owing money for Provider A (instead of owing none)
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provA=ProviderT.CreateProvider("ProvA");
			long provB=ProviderT.CreateProvider("ProvB");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",50,provNum:provA);
			PaySplit wrongSplit=PaySplitT.CreateSplit(proc.ClinicNum,pat.PatNum,0,0,proc.ProcDate,proc.ProcNum,provB,50,0);
			PaySplit incorrectXferSplit=PaySplitT.CreateSplit(proc.ClinicNum,pat.PatNum,0,0,proc.ProcDate,0,provB,-50,0);
			//In income xfer mode it'll show that there is a proc owing money.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			//Make sure that the logic creates 3 charges - One for the procedure (original, start, and end are 50) 
			//Splits should get explicitly linked correctly for the wrong provider which will equate provB's balance to 0. 
			//The procedure will still think it has been paid, there is not a way to know if the user intended that or not so no action needs to be taken.
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(Procedure) && x.AmountOriginal==50 && x.AmountStart==50 && x.AmountEnd==50).Count);
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit) && x.AmountOriginal==50 && x.AmountEnd==0).Count);
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit) && x.AmountOriginal==50 && x.AmountEnd==0).Count);
			//The user needs to either unattach the paysplit on the procedure, attach the negative split on the procedure, or delete both splits.
		}

		///<summary>In income transfer mode,claimprocs shouldn't show  and the paysplit should.</summary>
		[TestMethod]
		public void PaymentEdit_ConstructLinkChargeCredits_NegSplitsImplicitlyUsedByPosSources() {
			//Make a payment for Provider A for -50 (a charge)
			//Make an unattached claimproc for Provider A for 50
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provA=ProviderT.CreateProvider("ProvA");
			Carrier carrier=CarrierT.CreateCarrier("ABC");
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub insSub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			ClaimProcT.AddInsPaidAsTotal(pat.PatNum,plan.PlanNum,provA,50,insSub.InsSubNum,0,0);
			PaySplit wrongSplit=PaySplitT.CreateSplit(0,pat.PatNum,0,0,DateTime.Today,0,provA,-50,0);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			Assert.AreEqual(0,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(ClaimProc)).Count);
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.AmountEnd==50).Count);//PaySplits are opposite signed as account charges. 
		}

		///<summary>Assert that an adjustment can be explicitly offset by a paysplit when both are associated to the same procedure.</summary>
		[TestMethod]
		public void PaymentEdit_ConstructLinkChargeCredits_AdjustmentsLinkToPaySplitsThroughProcedures() {
			/*****************************************************
				Create Patient: pat1
				Create Provider: provNum1
				Create Provider: provNum2
				Create Procedure:  Today  provNum1  Pat1  $60
				Create Adjustment: Today  provNum2  Pat1  $10
					^Attached to Procedure
				Create Payment:    Today  provNum2  Pat1  $10
					^Attached to Procedure
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			ProcedureCode procedureCode=ProcedureCodeT.CreateProcCode("T1234");
			Procedure procedure=ProcedureT.CreateProcedure(pat1,procedureCode.ProcCode,ProcStat.C,"",60,provNum:provNum1);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat1.PatNum,10,procNum:procedure.ProcNum,provNum:provNum2);
			Payment payment=PaymentT.MakePayment(pat1.PatNum,10,DateTime.Today,provNum:provNum2,procNum:procedure.ProcNum);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat1.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat1.PatNum },pat1.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			//The AmountEnd on the adjustment and the payment should be 0 because they should explicitly link to each other via the procedure.
			Assert.AreEqual(3,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.AmountEnd==60
				&& x.AmountOriginal==60
				&& x.AmountStart==60));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.AmountEnd==0
				&& x.AmountOriginal==10
				&& x.AmountStart==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(PaySplit)
				&& x.AmountEnd==0
				&& x.AmountOriginal==0
				&& x.AmountStart==0));
			Payment transfer=PaymentT.MakePaymentNoSplits(pat1.PatNum,0);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum,Patients.GetFamily(pat1.PatNum),transfer);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary>If there is an adjustment that needs paying, autosplit logic should attach the paysplit to the adjustment. (treat it like a proc)</summary>
		[TestMethod]
		public void PaymentEdit_PaymentAutoSplitToAdjustment() {
			//Make an unattached adjustment for 50 (a charge)
			//Make a new payment for 50
			//Perform a normal payment - There should be one split for 50 that has the adjustment's AdjNum
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("ProvA");
			Adjustment adjust1=AdjustmentT.MakeAdjustment(pat.PatNum,50,DateTime.Today,provNum:provNum);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,50);
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,new PaymentEdit.LoadData());
			Assert.AreEqual(1,results.ListAutoSplits.Count);
			Assert.AreEqual(results.ListAutoSplits[0].AdjNum,adjust1.AdjNum);
		}

		///<summary>Auto split logic should not use later payments to allocate to the same adjustment that's been allocated to already.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitDoesntReuseAllocatedAdjustment() {
			//Make an unattached adjustment for 50 (a charge)
			//Make a payment for 50 to that adjustment
			//Create a procedure that comes after the adjustment for 25
			//Make a new payment for 50 and autosplit it.
			//The auto payment should have 2 splits - One for the procedure and one to unallocated, each for 25.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("ProvA");
			Adjustment adjust1=AdjustmentT.MakeAdjustment(pat.PatNum,50,DateTime.Today.AddDays(-3),provNum:provNum);
			Payment payOld=PaymentT.MakePaymentNoSplits(pat.PatNum,50,DateTime.Today.AddDays(-3));
			PaySplitT.CreateSplit(adjust1.ClinicNum,pat.PatNum,payOld.PayNum,0,DateTime.Today.AddDays(-3),0,provNum,50,0,adjust1.AdjNum);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",25);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,50,DateTime.Today);
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,new PaymentEdit.LoadData());
			Assert.AreEqual(2,results.ListAutoSplits.Count);
			Assert.AreEqual(1,results.ListAutoSplits.FindAll(x => x.PatNum==proc.PatNum && x.ProvNum==proc.ProvNum && x.ClinicNum==proc.ClinicNum && x.ProcNum==proc.ProcNum && x.AdjNum==0 && x.UnearnedType==0 && x.SplitAmt==25).Count);
			Assert.AreEqual(1,results.ListAutoSplits.FindAll(x => x.PatNum==pat.PatNum && x.ProvNum==0 && x.ClinicNum==pat.ClinicNum && x.ProcNum==0 && x.AdjNum==0 && x.UnearnedType>0 && x.SplitAmt==25).Count);
		}
			
		///<summary>Adjustments explicitly attached to procedure should not be used to implicitly pay off anything.</summary>
		[TestMethod]
		public void PaymentEdit_AdjustmentsExplicitlyAllocatedArentUsedImplicitly() {
			//Make procedure worth 50
			//Make adjustment worth 70 that's on the first procedure
			//Make another procedure worth 20
			//Auto-split logic should show the second procedure worth 20 still and the first worth -20
			//Autosplits created using 50 payment should be 20 for the second procedure, and 30 with unearned>0
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("ProvA");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",50);
			Adjustment adjust1=AdjustmentT.MakeAdjustment(pat.PatNum,-70,DateTime.Today.AddDays(-3),provNum:provNum,procNum:proc.ProcNum);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",20);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,50,DateTime.Today.AddDays(-3));
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,new PaymentEdit.LoadData());
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.AmountEnd==-20).Count);//First proc with overpaid adjustment
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.AmountEnd==0).Count);//Second proc, paid fully by payment
			Assert.AreEqual(2,results.ListAutoSplits.Count);//Make sure there are only 2 splits created.
			Assert.AreEqual(1,results.ListAutoSplits.FindAll(x => x.SplitAmt==20 && x.ProcNum==proc2.ProcNum).Count);//One split for 20 for second proc
			Assert.AreEqual(1,results.ListAutoSplits.FindAll(x => x.SplitAmt==30 && x.ProcNum==0 && x.UnearnedType>0).Count);//One split for 30 to unearned
		}

		///<summary>Transferring income to an adjustment should no longer show that adjustment as having owed money.</summary>
		[TestMethod]
		public void PaymentEdit_IncomeTransferToAdjustment() {
			//Create charge adjustment for 50
			//Create payment for 50 that's unallocated
			//Create an income transfer that takes 50 from the unallocated and allocates it to the adjustment.
			//Create new procedure for 25 and backdate it to before the adjustment
			//Create new payment and perform income transfer logic.  
			//There should display only one charge owing any money - The backdated procedure for 25.  (It is no longer implicitly paid due to adjustment link)
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("ProvA");
			Adjustment adjust1=AdjustmentT.MakeAdjustment(pat.PatNum,50,DateTime.Today,provNum:provNum);
			Payment payOld=PaymentT.MakePaymentNoSplits(pat.PatNum,50,DateTime.Today);
			PaySplit prepaySplit=PaySplitT.CreateSplit(0,pat.PatNum,payOld.PayNum,0,DateTime.Today,0,0,50,20);//Some arbitrary unearned type number
			Payment payXfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Today);
			PaySplit negSplit=PaySplitT.CreateSplit(prepaySplit.ClinicNum,prepaySplit.PatNum,payXfer.PayNum,0,DateTime.Today,0,prepaySplit.ProvNum,-50,prepaySplit.UnearnedType,0,prepaySplit.SplitNum);//negative split taking 50 from prepay split
			PaySplit posSplit=PaySplitT.CreateSplit(adjust1.ClinicNum,adjust1.PatNum,payXfer.PayNum,0,DateTime.Today,0,adjust1.ProvNum,50,0,adjust1.AdjNum,negSplit.SplitNum);//positive split allocating 50 to adjustment
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",25,DateTime.Today.AddDays(-3));//pre-date procedure for before adjust.  In the old way, we'd implicitly use the income on this instead of adjustment.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Today);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(Procedure) && x.AmountEnd==25).Count);
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(Adjustment) && x.AmountEnd==0).Count);
		}


		///<summary>Make sure if there are unattached adjustments that they are implicitly used by a valid previous transfer (neg split -> pos split).</summary>
		[TestMethod]
		public void PaymentEdit_IncomeTransfer_ExplicitlyLinkAdjustmentPaySplitTransfers() {
			//Make procedure for Provider A worth 50.
			//Make unattached negative adjustment Provider B for 50.
			//Perform an income transfer - It should display the adjustment as a source of income and the procedure as a destination.
			//Create the income transfer (manually create a -50 for Prov B, and create a +50 on procedure for ProvA)
			//Once complete, perform an income transfer again - There should be nothing since the procedure is paid properly and the transfer should 
			//counteract the adjustment.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provA=ProviderT.CreateProvider("ProvA");
			Adjustment adjust=AdjustmentT.MakeAdjustment(pat.PatNum,50,DateTime.Today.AddDays(-2),provNum:provA);//unattached adjustment
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,50,DateTime.Today.AddDays(-1));
			PaySplit originalSplit=PaySplitT.CreateSplit(0,pat.PatNum,pay.PayNum,0,DateTime.Today.AddDays(-1),0,provA,50,0,0);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			//Create income transfer manually
			PaySplit negSplit=PaySplitT.CreateSplit(0,pat.PatNum,payCur.PayNum,0,DateTime.Today,0,provA,-50,0,0,originalSplit.SplitNum);//offset for original
			PaySplit posSplit=PaySplitT.CreateSplit(0,pat.PatNum,payCur.PayNum,0,DateTime.Today,0,provA,50,0,0,negSplit.SplitNum);//final allocation
			Payment payCur2=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum,Patients.GetFamily(pat.PatNum),payCur2);
			//insert into database
			foreach(PaySplit split in transferResults.ListSplitsCur) {
				PaySplits.Insert(split);
			}
			foreach(PaySplits.PaySplitAssociated associatedSplit in transferResults.ListSplitsAssociated) {
				PaySplits.UpdateFSplitNum(associatedSplit.PaySplitOrig.SplitNum,associatedSplit.PaySplitLinked.SplitNum);
			}
			//now if we were to go into the transfer window again, we should see that the adjustment is explicitly linked and has no value.
			payCur2=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>()
				,payCur2,new List<AccountEntry>(),true,false);
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(Adjustment) && x.AmountOriginal==50 && x.AmountStart==0 && x.AmountEnd==0).Count);//Adjustment has been paid
			Assert.AreEqual(3,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit) && x.AmountOriginal==-50 && x.AmountStart==0 
				&& x.AmountEnd==0 && x.ProvNum==provA).Count);//the original, and the allocation, and the allocation transfer to unearned
			Assert.AreEqual(0,results.ListAccountCharges.Sum(x => x.AmountEnd));
		}

		[TestMethod]
		public void PaymentEdit_ExplicitlyLinkCredits_TransferFromWrongProviderSplit() {
			//To simulate a scenario where a procedure has been paid to the wrong provider and making an income transfer to correct it.
			//Problem was that after making the transfer, the original split and the offsetting split would still show in the window.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNumA=ProviderT.CreateProvider("ProvA");
			long provNumB=ProviderT.CreateProvider("ProvB");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",65,provNum:provNumA);
			Payment originalPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,65);
			PaySplit originalPaySplit=PaySplitT.CreateSplit(0,pat.PatNum,originalPayment.PayNum,0,proc.ProcDate,proc.ProcNum,provNumB,65,0);
			Payment transfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			//create income transfer manually
			PaySplit negOffsetSplit=PaySplitT.CreateSplit(0,pat.PatNum,transfer.PayNum,0,DateTime.Today,proc.ProcNum,provNumB,-65,0,0,originalPaySplit.SplitNum);
			PaySplit posAllocation=PaySplitT.CreateSplit(0,pat.PatNum,transfer.PayNum,0,DateTime.Today,proc.ProcNum,provNumA,65,0,0,negOffsetSplit.SplitNum);
			//Pretend like user is going to make another transfer so we can verify nothing would show up in the window
			Payment fakeTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>()
				,fakeTransfer,new List<AccountEntry>(),true);
			//ProvB should have two splits, a positive and a negative. 
			Assert.AreEqual(2,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit) && x.ProvNum==provNumB).Count);
			//ProvA should have two positive charges. Original procedure and allocated split. 
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit) && x.ProvNum==provNumA).Count);
			//there should be two positive splits total for the amount, and negative (note since they're charges right now their signs are opposite for splits)
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit) && x.AmountOriginal==65 && x.AmountStart==0 
				&& x.AmountEnd==0).Count);
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit) && x.AmountOriginal==-65 && x.AmountStart==0 
				&& x.AmountEnd==0).Count);//only 1 because one of them (the final allocating split) gets taken care of in Explicitly link and has OrigAmt==0
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(Procedure) && x.ProvNum==provNumA && x.AmountOriginal==65 
				&& x.AmountStart==0 && x.AmountEnd==0).Count);
			Assert.AreEqual(0,results.ListAccountCharges.Sum(x => x.AmountEnd));
		}

		[TestMethod]
		public void PaymentEdit_ImplicitlyLink_TransferredClaimsAreNotCountedTwice() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			List<long> listFamilyPatNums=Patients.GetFamily(pat.PatNum).ListPats.Select(x => x.PatNum).ToList();
			long provNum=ProviderT.CreateProvider("LS");
			Carrier carrier=CarrierT.CreateCarrier("BestCarrier");
			InsuranceInfo ins=InsuranceT.AddInsurance(pat,carrier.CarrierName);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,DateTime.Today,provNum:provNum);
			ins.AddBenefit(BenefitT.CreatePercentForProc(ins.PriPatPlan.PatPlanNum,proc.CodeNum,50));
			Claim claim=ClaimT.CreateClaim(new List<Procedure>{proc},ins);
			ins.ListAllClaimProcs=ClaimProcs.Refresh(ins.Pat.PatNum);
			ClaimT.ReceiveClaim(claim,ins.ListAllClaimProcs);
			//create new as total claim proc payment.
			ClaimProcT.AddInsPaidAsTotal(pat.PatNum,ins.PriInsPlan.PlanNum,provNum,65,ins.PriInsSub.InsSubNum,0,0,claim.ClaimNum);
			ins.ListAllClaimProcs=ClaimProcs.Refresh(ins.Pat.PatNum);
			Procedures.ComputeEstimatesForAll(pat.PatNum,ins.ListAllClaimProcs,ins.ListAllProcs,ins.ListInsPlans,ins.ListPatPlans,ins.ListBenefits,pat.Age,
				ins.ListInsSubs);
			List<ClaimProc> listValid=ClaimProcs.TransferClaimsAsTotalToProcedures(listFamilyPatNums).ListInsertedClaimProcs;
			Payment patPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,35);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>()
				,patPayment,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplitData=PaymentEdit.AutoSplitForPayment(results);
			Assert.AreEqual(1,autoSplitData.ListAutoSplits.Count);
			Assert.AreEqual(1,autoSplitData.ListAutoSplits.FindAll(x => x.SplitAmt==35 && x.UnearnedType==0 && x.ProcNum==proc.ProcNum).Count);
		}

		#region TP PrePay
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_TpProcsInPaymentWindowWhenDisplayCorrectlyAccordingToPref() {
			PrefT.UpdateInt(PrefName.PrePayAllowedForTpProcs,(int)YN.Yes);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov");
			Procedure treatPlanProc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.TP,"",50,provNum:provNum);
			Payment currentPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,50,isNew:true);
			PaymentEdit.ConstructResults constructData=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>{pat.PatNum},pat.PatNum
				,new List<PaySplit>(),currentPayment,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplitData=PaymentEdit.AutoSplitForPayment(constructData);
			Assert.AreEqual(1,autoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(0,autoSplitData.ListAutoSplits.FindAll(x => x.ProcNum!=0).Count);//auto split should be for reg. unallocated, not the tp.
			//now do the same with the pref turned off. Account charge should not show in the list.
			PrefT.UpdateInt(PrefName.PrePayAllowedForTpProcs,(int)YN.No);
			constructData=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>{pat.PatNum},pat.PatNum,new List<PaySplit>(),currentPayment,
				new List<AccountEntry>());
			autoSplitData=PaymentEdit.AutoSplitForPayment(constructData);
			Assert.AreEqual(0,autoSplitData.ListAccountCharges.Count);
		}

		[TestMethod]
		public void Payments_CreateTransferForTpProcs_TransferTpPreallocationsToAllocatedMoneyOnceCompleted() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov");
			Procedure treatPlanProc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.TP,"",50,provNum:provNum);
			long hiddenUnearnedType=Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType).FirstOrDefault(x => x.ItemValue!="").DefNum;
			Payment paidTpPreAllocation=PaymentT.MakePayment(pat.PatNum,50,DateTime.Today.AddDays(-1),0,provNum,treatPlanProc.ProcNum,1,0,
				hiddenUnearnedType);
			ProcedureT.SetComplete(treatPlanProc,pat,new InsuranceInfo());
			//setting proceudre complete should have made a transfer taking the unearned on the proc, and making it allocated.
			List<PaySplit> listSplitsOnProc=PaySplits.GetPaySplitsFromProc(treatPlanProc.ProcNum);
			Assert.AreEqual(1,listSplitsOnProc.Count);//In the end only one split ends up being attached to the procedure.
			//check to make sure the original prepayment got the procedure disassociated from it. 
			Assert.AreEqual(0,PaySplits.GetForPayment(paidTpPreAllocation.PayNum).First().ProcNum);
			//the negative split (attached to original prepay) should also be the FSplitNum of the final allocating split that has the procedure.
			Assert.AreEqual(listSplitsOnProc.First().FSplitNum
				,PaySplits.GetSplitsForPrepay(PaySplits.GetForPayment(paidTpPreAllocation.PayNum)).First().SplitNum);
			Assert.AreEqual(50,listSplitsOnProc.Sum(x => x.SplitAmt));
		}

		[TestMethod]
		public void Payments_CreateTransferForTpProcs_TransferTpPreAllocationToBrokenProcedure() {
			PrefT.UpdateBool(PrefName.TpPrePayIsNonRefundable,true);
			PrefT.UpdateInt(PrefName.PrePayAllowedForTpProcs,(int)YN.Yes);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov");
			Appointment appt=AppointmentT.CreateAppointment(pat.PatNum,DateTime.Today,1,provNum);
			Procedure treatPlanProc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.TP,"",50,provNum:provNum,aptNum:appt.AptNum);
			long hiddenUnearnedType=Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType).FirstOrDefault(x => x.ItemValue!="").DefNum;
			Payment paidTpPreAllocation=PaymentT.MakePayment(pat.PatNum,50,DateTime.Today.AddDays(-1),0,provNum,treatPlanProc.ProcNum,1,0,
				hiddenUnearnedType);
			//D9986 == missed appointment
			ProcedureCode procCode=new ProcedureCode{CodeNum=treatPlanProc.CodeNum,ProcCode="D9986",NoBillIns=true,ProvNumDefault=provNum};
			Procedure brokenProcedure=AppointmentT.BreakAppointment(appt,pat,procCode,50);
			List<PaySplit> listSplitsForBrokenProc=PaySplits.GetPaySplitsFromProc(brokenProcedure.ProcNum);
			Assert.AreEqual(50,listSplitsForBrokenProc.Sum(x => x.SplitAmt));
			Assert.AreEqual(0,PaySplits.GetPaySplitsFromProc(treatPlanProc.ProcNum).Sum(x => x.SplitAmt));
		}

		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_HiddenPaysplitsAreNotUsedInImplicitLinking() {
			Def def=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,"hiddenType","x");
			PrefT.UpdateInt(PrefName.PrePayAllowedForTpProcs,(int)YN.Yes);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov");
			//Make an unattached hidden payment that will be reserved for some treatment planned procedure down the line.
			Payment hiddenPayment=PaymentT.MakePayment(pat.PatNum,50,DateTime.Today.AddDays(-1),unearnedType:def.DefNum);
			//Make a completed procedure that we do not want linked to the payment we just made. 
			Procedure procedure=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"4",100);
			Payment currentPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,25,DateTime.Today);
			PaymentEdit.ConstructResults constructData=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>{pat.PatNum},pat.PatNum
				,new List<PaySplit>(),currentPayment,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplitData=PaymentEdit.AutoSplitForPayment(constructData);
			Assert.AreEqual(1,autoSplitData.ListAccountCharges.FindAll(x => x.AmountStart==100 && x.AmountEnd==75).Count);
			Assert.AreEqual(1,autoSplitData.ListAutoSplits.FindAll(x => x.SplitAmt==25).Count);//auto split should be for reg. unallocated, not the tp.
		}

		[TestMethod]
		public void Payments_CreateTransferForTpProcs_NoRefundTransfersKeepOriginalPrePaymentAmount() {
			//bug for this was when a customer was transferring a no-refund tp pre pay to a broken procedure and only transferred some of the money.
			//if a user was on don't enforce specifically, they would make the transfer and the original split would be modified with no other split created.
			PrefT.UpdateInt(PrefName.PrePayAllowedForTpProcs,(int)YN.Yes);
			PrefT.UpdateBool(PrefName.TpPrePayIsNonRefundable,true);
			long hiddenUnearnedType=Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType).FirstOrDefault(x => x.ItemValue!="").DefNum;
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov");
			Appointment appt=AppointmentT.CreateAppointment(pat.PatNum,DateTime.Today.AddDays(1),1,provNum);
			Procedure treatPlanProc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.TP,"",50,provNum:provNum,aptNum:appt.AptNum);
			Payment pay=PaymentT.MakePayment(pat.PatNum,50,DateTime.Today,provNum:provNum,procNum:treatPlanProc.ProcNum,unearnedType:hiddenUnearnedType);
			//D9986 == missed appointment
			ProcedureCode procCode=new ProcedureCode{CodeNum=treatPlanProc.CodeNum,ProcCode="D9986",NoBillIns=true,ProvNumDefault=provNum};
			Procedure brokenProcedure=AppointmentT.BreakAppointment(appt,pat,procCode,30);
			List<PaySplit> listSplitsForBrokenProc=PaySplits.GetPaySplitsFromProc(brokenProcedure.ProcNum);
			Assert.AreEqual(0,PaySplits.GetPaySplitsFromProc(treatPlanProc.ProcNum).Sum(x => x.SplitAmt));//proc isn't complete. Should have no money.
			Assert.AreEqual(30,listSplitsForBrokenProc.Sum(x => x.SplitAmt));
			List<PaySplit> listSplitsForPayment=PaySplits.GetForPayment(pay.PayNum);
			Assert.AreEqual(50,listSplitsForPayment.Sum(x => x.SplitAmt));//payment should be worth it's original value.
		}
		#endregion

		///<summary>Make sure that Amount begin, Amount start, and Amount end are correct after clicking and unclicking checkExplicitCreditsOnly.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_CheckExplicitCreditsOnly() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",100);
			Payment payment1=PaymentT.MakePayment(pat.PatNum,50,DateTime.Now.AddDays(-1));
			Payment payment2=PaymentT.MakePaymentNoSplits(pat.PatNum,10);
			PaymentEdit.ConstructResults chargeResult=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum},pat.PatNum,
				new List<PaySplit>(),payment2,new List<AccountEntry>());//chargeResults for making autosplit
			PaymentEdit.AutoSplit autoSplit=PaymentEdit.AutoSplitForPayment(chargeResult);//Create autosplit for payment2
			autoSplit.ListSplitsCur.AddRange(autoSplit.ListAutoSplits);//Add auto splits to current splits like in PaymentEdit.Init()
			Assert.AreEqual(100,chargeResult.ListAccountCharges[0].AmountOriginal);
			Assert.AreEqual(50,chargeResult.ListAccountCharges[0].AmountStart);
			Assert.AreEqual(40,chargeResult.ListAccountCharges[0].AmountEnd);
			//Check explicit credits only box
			chargeResult.ListAccountCharges=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> { pat.PatNum },pat.PatNum,
				autoSplit.ListSplitsCur,autoSplit.Payment,new List<AccountEntry>(),doIncludeExplicitCreditsOnly: true).ListAccountCharges;
			Assert.AreEqual(100,chargeResult.ListAccountCharges[0].AmountOriginal);
			Assert.AreEqual(100,chargeResult.ListAccountCharges[0].AmountStart);
			Assert.AreEqual(90,chargeResult.ListAccountCharges[0].AmountEnd);
			//Uncheck explicit credits only box
			chargeResult.ListAccountCharges=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> { pat.PatNum },pat.PatNum,
				autoSplit.ListSplitsCur,autoSplit.Payment,new List<AccountEntry>(),doIncludeExplicitCreditsOnly: false).ListAccountCharges;
			Assert.AreEqual(100,chargeResult.ListAccountCharges[0].AmountOriginal);
			Assert.AreEqual(50,chargeResult.ListAccountCharges[0].AmountStart);
			Assert.AreEqual(40,chargeResult.ListAccountCharges[0].AmountEnd);
		}
		#endregion
		#region IncomeTransferTests
		////This is failing intentionally. We removed this logic from the income transfer manager. It needs to be made into a DBM. 
		/////Leaving for now just in case we go back on that decision, but for now income transfer code is not expected to handle this case. 
		//[TestMethod]
		//public void PaymentEdit_CreateUnearnedLoop_FixIncorrectUnearnedAllocations() {
		//	Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
		//	Family fam=Patients.GetFamily(pat.PatNum);
		//	Def defPrePay=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,"prepay");
		//	long provNum=ProviderT.CreateProvider("FixIncorrectUnearned");
		//	Payment overallocated=PaymentT.MakePayment(pat.PatNum,200,DateTime.Today.AddDays(-5),unearnedType:defPrePay.DefNum);
		//	PaymentT.MakePayment(pat.PatNum,100,DateTime.Today.AddDays(-4),unearnedType: defPrePay.DefNum);
		//	PaySplit overallocatedsplit=PaySplits.GetForPayment(overallocated.PayNum).First();
		//	Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",300,DateTime.Today.AddDays(-3),provNum:provNum);
		//	//simulate incorrect transfer (used to be an issue in the code)
		//	Payment badTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,300,DateTime.Today.AddDays(-2));
		//	PaySplit negSplit=PaySplitT.CreateSplit(0,pat.PatNum,badTransfer.PayNum,0,DateTime.Today.AddDays(-3),0,0,-300,defPrePay.DefNum
		//		,fSplitNum:overallocatedsplit.SplitNum);
		//	PaySplitT.CreateSplit(0,pat.PatNum,badTransfer.PayNum,0,DateTime.Today.AddDays(-3),proc.ProcNum,proc.ProvNum,300,0,fSplitNum: negSplit.SplitNum);
		//	//move income transfer logic over so we can call MakeTransfer 
		//	Payment txfrPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
		//	PaymentEdit.IncomeTransferData txfrResults=PaymentT.IncomeTransfer(pat.PatNum,fam,txfrPayment,new List<PayPlanCharge>{ });
		//	//positive unearned split made to counteract a previously made split at the same level that made it overallocated.
		//	Assert.AreEqual(1,txfrResults.ListSplitsCur.FindAll(x => x.SplitAmt==100 && x.UnearnedType==defPrePay.DefNum).Count);
		//	//negative allocation made to take away from the procedure
		//	Assert.AreEqual(1,txfrResults.ListSplitsCur.FindAll(x => x.SplitAmt==-100 && x.UnearnedType==0 && x.ProcNum==proc.ProcNum).Count);
		//	//now test the scenario again, but with unallocated splits that also need to be transferred to ensure both are working as intended.
		//	Procedure newProc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",50,DateTime.Today.AddDays(-1));
		//	Payment unallocated=PaymentT.MakePayment(pat.PatNum,50,DateTime.Today.AddDays(-1));
		//	Payment txfr=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Today);
		//	txfrResults=PaymentT.IncomeTransfer(pat.PatNum,fam,txfr,new List<PayPlanCharge>());
		//	//positive unearned split made to counteract a previously made split at the same level that made it overallocated.
		//	Assert.AreEqual(1,txfrResults.ListSplitsCur.FindAll(x => x.SplitAmt==100 && x.UnearnedType==defPrePay.DefNum).Count);
		//	//negative allocation made to take away from the procedure
		//	Assert.AreEqual(1,txfrResults.ListSplitsCur.FindAll(x => x.SplitAmt==-100 && x.UnearnedType==0 && x.ProcNum==proc.ProcNum).Count);
		//	//Also assert money got moved to and taken from unearned for the unallocated procedure.
		//	Assert.AreEqual(2,txfrResults.ListSplitsCur.FindAll(x => Math.Abs(x.SplitAmt)==50 && x.UnearnedType!=0).Count);
		//	Assert.AreEqual(1,txfrResults.ListSplitsCur.FindAll(x => x.SplitAmt==50 && x.ProcNum==newProc.ProcNum).Count);
		//}

		[TestMethod]
		public void PaymentEdit_CreateTransfers_TransfersMoveOverPaidAndExtraMoneyToUnearned() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("UnearnedTxfr");
			//create procedure that gets overpaid by 100
			Procedure procPaid=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum);
			Payment overpaymentProc=PaymentT.MakePayment(pat.PatNum,200,DateTime.Today,procNum:procPaid.ProcNum,provNum:provNum);
			//create adjustment that gets overpaid by 100
			Adjustment adjPaid=AdjustmentT.MakeAdjustment(pat.PatNum,100,provNum:provNum
				,adjType:Defs.GetDefsForCategory(DefCat.AdjTypes).FirstOrDefault(x => x.ItemValue=="+").DefNum);
			Payment overpaymentAdj=PaymentT.MakePayment(pat.PatNum,200,DateTime.Today,provNum:provNum,adjNum:adjPaid.AdjNum);
			//create unattached split for $100 that should get moved to unearned. 
			Payment unattachedPayment=PaymentT.MakePayment(pat.PatNum,100,DateTime.Today,provNum:provNum);
			//make transfer. We're expecting a total of $300 to get moved to unearned by the end. 
			Payment txfrPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.IncomeTransferData transfer=PaymentT.IncomeTransfer(pat.PatNum,fam,txfrPayment,new List<PayPlanCharge>{ });
			Assert.AreEqual(6,transfer.ListSplitsCur.Count);//six splits should have been created to move the money to unearend. 
			Assert.AreEqual(300,transfer.ListSplitsCur.FindAll(x => x.SplitAmt.IsGreaterThan(0)).Sum(x => x.SplitAmt));
			Assert.AreEqual(3,transfer.ListSplitsCur.FindAll(x => x.SplitAmt.IsGreaterThan(0) && x.UnearnedType!=0).Count);
			//make sure that the split is only attached if we are taking money from a paysplit
			Assert.AreEqual(0,transfer.ListSplitsCur.FirstOrDefault(x => x.SplitAmt.IsLessThan(0) && x.ProcNum==procPaid.ProcNum).FSplitNum);
			Assert.AreEqual(0,transfer.ListSplitsCur.FirstOrDefault(x => x.SplitAmt.IsLessThan(0) && x.AdjNum==adjPaid.AdjNum).FSplitNum);
		}

		[TestMethod]
		public void PaymentEdit_CreateTransfers_AllowTransfersForRigorousAccounting() {
			//To test scenario when office previously had rigorous accouting turned off, made some bad splits, and now have it turned on.
			//Transfers should still be allowed to fix the previous bad splits and get them going to a place they should be. 
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("Rig");
			//create an unattached payments
			Payment unattachedPayment=PaymentT.MakePayment(pat.PatNum,300,DateTime.Today,provNum:provNum);
			//create procedure for same amount
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",300,provNum:provNum);
			//Turn on Rigorous Accounting
			PrefT.UpdateInt(PrefName.RigorousAccounting,(int)RigorousAccounting.EnforceFully);
			//attempt to transfer the payment so it ends up going to the procedure
			Payment txfrPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.IncomeTransferData transfer=PaymentT.IncomeTransfer(pat.PatNum,fam,txfrPayment,new List<PayPlanCharge>{ });
			Assert.IsFalse(transfer.HasInvalidSplits);//we just created the scenario where we expect splits to be transferred to unearned with a provider.
			Assert.AreEqual(2,transfer.ListSplitsCur.Count);//2 splits should be created that moves the money to unearned and then 2 that allocate
			Assert.AreEqual(1,transfer.ListSplitsCur.FindAll(x => x.SplitAmt.IsGreaterThan(0) && x.UnearnedType==0 && x.ProcNum==proc.ProcNum).Count);
		}

		[TestMethod]
		public void PaymentEdit_CreateTransfers_NegativeProceduresDoNotGetUsedAsIncomeSource() {
			//Users have been allowed to make negative procedures. We need to make sure we don't use that money to fund something else. 
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("NoNegProcs");
			//create negative completed procedure
			Procedure procNeg=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",-100,DateTime.Today.AddDays(-2));
			//create regular positive procedure for less than the absolute value of the negative
			Procedure procPos=ProcedureT.CreateProcedure(pat,"D0210",ProcStat.C,"",50,DateTime.Today.AddDays(-1));
			//make our transfer payment shell
			Payment txfrPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.IncomeTransferData transfer=PaymentT.IncomeTransfer(pat.PatNum,fam,txfrPayment,new List<PayPlanCharge>{ });
			Assert.AreEqual(0,transfer.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_CreateTransferHelper_TransferKeepsUnearnedTypeFromSplit() {
			//default unearned type was being used during transfer even though the original prepayment did not use the default.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			//create pre-payment that does not use the default, so create a new prepayment type as well to use.
			Def newUnearned=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,"HiddenPrepay","x");
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today,unearnedType:newUnearned.DefNum);
			//make procedure to transfer to
			long provNum=ProviderT.CreateProvider("TransferKeepUnearned");
			ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum);
			//Make transfer
			Payment txfrPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.IncomeTransferData transfer=PaymentT.IncomeTransfer(pat.PatNum,fam,txfrPayment,new List<PayPlanCharge>{ },true);
			Assert.AreEqual(2,transfer.ListSplitsCur.Count);
			Assert.AreEqual(newUnearned.DefNum,transfer.ListSplitsCur.FirstOrDefault(x => x.SplitAmt<0).UnearnedType);
		}

		///<summary>Test checking and unchecking the Income Transfer Manager Window's Include Hidden Payment Splits check box.
		///Checking and unchecking the box will show or hide hidden pay splits in the Income Sources pane.</summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_CheckIncludeHiddenSplitsAndTransfersWithHidden() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			Def newUnearned=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,"HiddenPrepay","x");
			Payment paymentHiddenUnearned=PaymentT.MakePayment(pat.PatNum,20,DateTime.Today,unearnedType:newUnearned.DefNum);
			Payment paymentUnearned=PaymentT.MakePayment(pat.PatNum,10,DateTime.Today);
			long provNum=ProviderT.CreateProvider("prov");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",100,provNum:provNum);
			Payment paymentTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.IncomeTransferData returnedData=new PaymentEdit.IncomeTransferData();
			//Get list of account charges without hidden pay splits.
			List<AccountEntry> listAccountCharges=PaymentEdit.ConstructAndLinkChargeCredits(fam.ListPats.Select(x => x.PatNum).ToList(),pat.PatNum,
				new List<PaySplit>(),paymentTransfer,new List<AccountEntry>(),true,false,doShowHiddenSplits:false).ListAccountCharges;
			Assert.AreEqual(0,listAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit) && ((PaySplit)x.Tag).UnearnedType==newUnearned.DefNum).Count);
			//Run income transfer logic and make sure hidden splits do not transfer
			returnedData=PaymentT.IncomeTransfer(pat.PatNum,fam,paymentTransfer,new List<PayPlanCharge>());
			Assert.AreEqual(0,returnedData.ListSplitsCur.FindAll(x => x.UnearnedType==newUnearned.DefNum).Count);
			//Get list of account charges with hidden pay splits to simulate box being checked.
			listAccountCharges=PaymentEdit.ConstructAndLinkChargeCredits(fam.ListPats.Select(x => x.PatNum).ToList(),pat.PatNum,
				new List<PaySplit>(),paymentTransfer,new List<AccountEntry>(),true,false,doShowHiddenSplits:true).ListAccountCharges;
			Assert.AreEqual(1,listAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit) && ((PaySplit)x.Tag).UnearnedType==newUnearned.DefNum).Count);
			//Run the income transfer logic and make sure hidden splits can be transferred
			returnedData=PaymentT.IncomeTransfer(pat.PatNum,fam,paymentTransfer,new List<PayPlanCharge>(),true);
			Assert.AreEqual(1,returnedData.ListSplitsCur.FindAll(x => x.UnearnedType==newUnearned.DefNum).Count);
			//Uncheck box. Change it back and verify we get the same results as first run through. 
			listAccountCharges=PaymentEdit.ConstructAndLinkChargeCredits(fam.ListPats.Select(x => x.PatNum).ToList(),pat.PatNum,
				new List<PaySplit>(),paymentTransfer,new List<AccountEntry>(),true,false,doShowHiddenSplits: false).ListAccountCharges;
			Assert.AreEqual(0,listAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit) && ((PaySplit)x.Tag).UnearnedType==newUnearned.DefNum).Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceUnattachedSplits_PreBalanceAccountForUnattachedSplits() {
			//bug here was transferring more money that available to unearned because it was double counting splits when a manual transfer was entred.
			//note this scenario is specifically for when prepayments are allowed to providers, when they are not, the count will only be 6 (non zero)
			PrefT.UpdateBool(PrefName.AllowPrepayProvider,true);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNumA=ProviderT.CreateProvider("LSA");
			long provNumB=ProviderT.CreateProvider("LSB");
			//make a procedure for the patient
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNumA);
			//make a payment for the incorrect provider for this procedure.
			Payment pay=PaymentT.MakePayment(pat.PatNum,100,DateTime.Today,provNum:provNumB,procNum:proc.ProcNum);
			//make a manual transfer (unattached) that attempts to fix the incorrect provider being paid. 
			Payment payShell=PaymentT.MakePaymentNoSplits(pat.PatNum,100,DateTime.Today);
			PaySplit splitA=PaySplitT.CreateSplit(0,pat.PatNum,payShell.PayNum,0,DateTime.Today,0,provNumB,-100,0);
			PaySplit splitB=PaySplitT.CreateSplit(0,pat.PatNum,payShell.PayNum,0,DateTime.Today,0,provNumA,100,0);
			Payment txfrPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.IncomeTransferData unallocatedTransfer=PaymentEdit.TransferUnallocatedSplitToUnearned
				(PaySplits.GetForPats(fam.ListPats.Select(x =>x.PatNum).ToList()),txfrPayment.PayNum);
			//unattached split transferred to unearned for provider A. 
			Assert.AreEqual(1,unallocatedTransfer.ListSplitsCur.FindAll(x => x.UnearnedType==0 && x.ProvNum==provNumA && x.SplitAmt<0).Count);
			Assert.AreEqual(1,unallocatedTransfer.ListSplitsCur.FindAll(x => x.UnearnedType!=0 && x.ProvNum==provNumA && x.SplitAmt>0).Count);
			//unattached negative split transferred to unearned for provider B (provider B now has negative unearned).
			Assert.AreEqual(1,unallocatedTransfer.ListSplitsCur.FindAll(x => x.UnearnedType==0 && x.ProvNum==provNumB && x.SplitAmt>0).Count);
			Assert.AreEqual(1,unallocatedTransfer.ListSplitsCur.FindAll(x => x.UnearnedType!=0 && x.ProvNum==provNumB && x.SplitAmt<0).Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceUnattachedSplits_BalanceAndUseCreatedUnearned() {
			//bug here was transferring more money that available to unearned because it was double counting splits when a manual transfer was entred.
			PrefT.UpdateInt(PrefName.RigorousAccounting,(int)RigorousAccounting.AutoSplitOnly);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LSA");
			//make a procedure for the patient
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum);
			//make an unallocated payment.
			Payment pay=PaymentT.MakePayment(pat.PatNum,100,DateTime.Today,provNum:provNum);
			Payment txfrPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.IncomeTransferData transferUnallocated=
				PaymentEdit.TransferUnallocatedSplitToUnearned(PaySplits.GetForPats(fam.ListPats.Select(x => x.PatNum).ToList()),txfrPayment.PayNum);
			//transfer should result in a negative split to offset the unallocated, a positive split to unearned to balance the account.
			//then the transfer should continue and should have used that unearned to pay the procedure, making a negative to the unearned we created,
			//and a positive split that gets applied to the procedure resulting in 4 splits total.
			Assert.AreEqual(2,transferUnallocated.ListSplitsCur.Count);//transfer from unallocated to unearned.
			Assert.AreEqual(1,transferUnallocated.ListSplitsCur.FindAll(x => x.UnearnedType==0 && x.SplitAmt<0).Count);//offset to unallocated	
			Assert.AreEqual(1,transferUnallocated.ListSplitsCur.FindAll(x => x.UnearnedType!=0 && x.SplitAmt>0).Count);//unattached split transferred to unearend 
			//save data and then perform regular income transfer
			foreach(PaySplit split in transferUnallocated.ListSplitsCur) {
				PaySplits.Insert(split);
			}
			foreach(PaySplits.PaySplitAssociated split in transferUnallocated.ListSplitsAssociated) {
				PaySplits.UpdateFSplitNum(split.PaySplitOrig.SplitNum,split.PaySplitLinked.SplitNum);
			}
			txfrPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			//make an income transfer using the income transfer manager logic
			PaymentEdit.IncomeTransferData transfer=PaymentT.IncomeTransfer(pat.PatNum,fam,txfrPayment,new List<PayPlanCharge>{ });
			Assert.AreEqual(1,transfer.ListSplitsCur.FindAll(x => x.UnearnedType!=0 && x.SplitAmt<0).Count);//offset to unearned
			//and the final allocation.
			Assert.AreEqual(1,transfer.ListSplitsCur.FindAll(x => x.UnearnedType==0 && x.ProvNum==provNum && x.SplitAmt>0 && x.ProcNum==proc.ProcNum).Count);
		}

		[TestMethod]
		public void PaymentEdit_CreateTransfers_NonProviderUnearnedDoesNotCreateContinualLoopForTransferring() {
			//bug here was that when providers were not allowed on pre-payments the provider did not match for the pat/prov/clinic against it's link
			//so explicitly link credits did not correctly evaluate its true value from the split and thought money still existed to be moved.
			PrefT.UpdateBool(PrefName.AllowPrepayProvider,false);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNumA=ProviderT.CreateProvider("LSA");
			long provNumB=ProviderT.CreateProvider("LSB");
			//make a procedure for the patient
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNumA);
			//make a payment for the incorrect provider for this procedure.
			Payment pay=PaymentT.MakePayment(pat.PatNum,100,DateTime.Today,provNum:provNumB,procNum:proc.ProcNum);
			//make a manual transfer (unattached) that attempts to fix the incorrect provider being paid. 
			Payment payShell=PaymentT.MakePaymentNoSplits(pat.PatNum,100,DateTime.Today);
			PaySplit splitA=PaySplitT.CreateSplit(0,pat.PatNum,payShell.PayNum,0,DateTime.Today,0,provNumB,-100,0);
			PaySplit splitB=PaySplitT.CreateSplit(0,pat.PatNum,payShell.PayNum,0,DateTime.Today,0,provNumA,100,0);
			Payment unallocatedTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			//make an income transfer using the income transfer manager logic (call balance unattached splits)
			PaymentEdit.IncomeTransferData unallocatedData=
				PaymentEdit.TransferUnallocatedSplitToUnearned(PaySplits.GetForPats(fam.ListPats.Select(x => x.PatNum).ToList()),unallocatedTransfer.PayNum);
			//Insert transfer into the database.
			foreach(PaySplit split in unallocatedData.ListSplitsCur) {
				PaySplits.Insert(split);
			}
			foreach(PaySplits.PaySplitAssociated associateSplit in unallocatedData.ListSplitsAssociated) {
				PaySplits.UpdateFSplitNum(associateSplit.PaySplitOrig.SplitNum,associateSplit.PaySplitLinked.SplitNum);
			}
			Payment txfrPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			//make an income transfer using the income transfer manager logic
			PaymentEdit.IncomeTransferData transfer=PaymentT.IncomeTransfer(pat.PatNum,fam,txfrPayment,new List<PayPlanCharge>{ });
			PaySplits.InsertMany(transfer.ListSplitsCur);
			//AND NOW THE MAIN EVENT - CHECK TO MAKE SURE THAT ANOTHER TRANSFER IS NOT POSSIBLE
			Payments.Update(txfrPayment,true);
			Payment newUnallocatedTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.IncomeTransferData newData=
				PaymentEdit.TransferUnallocatedSplitToUnearned(PaySplits.GetForPats(fam.ListPats.Select(x => x.PatNum).ToList()),newUnallocatedTransfer.PayNum);
			Assert.AreEqual(0,newData.ListSplitsCur.Count);
			Payment newTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			transfer=PaymentT.IncomeTransfer(pat.PatNum,fam,newTransfer,new List<PayPlanCharge>());
			Assert.AreEqual(0,transfer.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_CreateTransferHelper_DoNotTransferToUnearnedWhenAdjNumIsPresent() {
			//This is seemingly more of an edge case scenario but we need to check for it since it is present is some databases.
			//There was a point in time when adjustments did not have providers. We need to make sure those adjustments do not get transferred to unearned.
			PrefT.UpdateBool(PrefName.AllowPrepayProvider,false);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNumA=ProviderT.CreateProvider("LS");
			AdjustmentT.MakeAdjustment(pat.PatNum,100);
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today);
			Payment txfrPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Today,true);
			PaymentEdit.IncomeTransferData results=PaymentT.IncomeTransfer(pat.PatNum,fam,txfrPayment,new List<PayPlanCharge>());
			Assert.AreEqual(0,results.ListSplitsCur.FindAll(x => x.AdjNum!=0 && x.UnearnedType!=0).Count);
		}

		[TestMethod]
		public void PaymentEdit_CreateTransferHelper_DoNotMakeTransfersThatCreateNegativeUnearned() {
			//There are transfers that are okay to make negative unearned (unallocated transfer logic) but that is a specific case. The regular transfer 
			//logic should not leave the negative unearned on an account in this specific cicumstance (proceudre, positive unearned, negative unearned).
			//The unearned should balance out fist, and then the income transfer will know it has no splits to created. 
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum,procDate:DateTime.Today);//procedure
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum:provNum);//unallocated positive
			PaymentT.MakePayment(pat.PatNum,-100,DateTime.Today.AddDays(-1),provNum:provNum);//unallocated negative
			Payment transfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);//current income transfer
			PaymentEdit.IncomeTransferData results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum,fam,transfer);
			//we should get a total of 4 splits that moved unallocated to unearned from the unallocated transfer (different payNum) + 2 made above
			Assert.AreEqual(6,PaySplits.GetForPats(new List<long> {pat.PatNum }).Count);
			//and no other splits after that. The unearned should net to each other and nothing should need to be transferred.
			Assert.AreEqual(0,results.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_IncomeTransfer_UnearnedCreatedFromTransferGetsAppliedToProduction() {
			//Bug was that the unearned created from the income transfer manager would not be correctly evaluated and thus would not 
			//think that the income had any value. 
			PrefT.UpdateBool(PrefName.AllowPrepayProvider,true);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",70,provNum:provNum,procDate:DateTime.Today.AddDays(-2));
			PaymentT.MakePayment(pat.PatNum,70,DateTime.Today.AddDays(-1),0,provNum,0,1,0,0);//unallocated payment
			Payment transfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);//transfer should create txfr to unearned first, and then do other transfers.
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum,fam,transfer);
			//after both logics have run, the unearned that was created should have been applied to the procedure.
			Assert.AreEqual(1,transferResults.ListSplitsCur.FindAll(x => x.UnearnedType!=0 && x.SplitAmt.IsLessThan(0)).Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.FindAll(x => x.ProcNum==proc.ProcNum && x.SplitAmt.IsEqual(70)).Count);
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_UnallocatedSplitsGetTransferredAndReducedValue() {
			//Bug would make the correct transfer splits but would then not evaluate the amount correctly going into the window
			//making it seem like things still needed to be paid, which is false.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			PaymentT.MakePayment(pat.PatNum,70,DateTime.Today.AddDays(-1),0,provNum,0,1,0,0);//positive unallocated payment
			PaymentT.MakePayment(pat.PatNum,-70,DateTime.Today.AddDays(-1),0,provNum,0,1,0,0);//negative unallocated payment
			Payment transfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);//unearned transfers will not be in this paynum. 
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum,fam,transfer);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
			Payment transferTwo=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults explicitLinkResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>(){pat.PatNum},pat.PatNum
				,new List<PaySplit>(),transferTwo,new List<AccountEntry>(),true);
			Assert.AreEqual(0,explicitLinkResults.ListAccountCharges.FindAll(x => x.AmountEnd!=0).Count);
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_UnallocatedTransferAndOverpaidProcedureGetLinkedCorrectly() {
			//Buggy behavior: If an unallocated split transfer happened, and there was an overpaid procedure that needed to be transferred,
			//the explicit linking logic would not correctly reduce the value on the unallocated paysplit (that had been transferred and has no value)
			//so it would be available to be transferred incorrectly.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			PaymentT.MakePayment(pat.PatNum,70,DateTime.Today.AddDays(-2));//positive unallocated payment
			PaymentT.MakePayment(pat.PatNum,-70,DateTime.Today.AddDays(-1));//negative unallocated payment
			Payment transfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);//unearned transfers will not be in this paynum.
			PaymentT.BalanceAndIncomeTransfer(pat.PatNum,fam,transfer);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",70,DateTime.Today,provNum:provNum);
			PaymentT.MakePayment(pat.PatNum,105,DateTime.Today,provNum:provNum,procNum:proc.ProcNum);
			//unearned has already been transferred and inserted at this point. 
			Payment transferTwo=PaymentT.MakePaymentNoSplits(pat.PatNum,0,payType:1,isNew:true);
			PaymentEdit.ConstructResults linkResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>(){pat.PatNum},pat.PatNum,new List<PaySplit>()
				,transferTwo,new List<AccountEntry>(),true);
			Assert.AreEqual(1,linkResults.ListAccountCharges.FindAll(x => x.AmountEnd!=0).Count);//only the procedure should show.
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_NoNegativeUnearnedFromUnattachedAdjustment() {
			//Unattached adjustments could sometimes make the account go into negative unearned
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",200,provNum:provNum,procDate:DateTime.Today.AddDays(-5));
			AdjustmentT.MakeAdjustment(pat.PatNum,-100,DateTime.Today.AddDays(-4),provNum:provNum);
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today.AddDays(-3),provNum:provNum,payType:1);
			//manual transfer
			Payment manualTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Today.AddDays(-2));
			PaySplitT.CreateSplit(0,pat.PatNum,manualTransfer.PayNum,0,DateTime.Today.AddDays(-5),proc.ProcNum,provNum,200,0);
			PaySplitT.CreateSplit(0,pat.PatNum,manualTransfer.PayNum,0,DateTime.Today.AddDays(-2),0,provNum,-200,0);
			Payment transfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Today);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum,fam,transfer);
			//4 Splits will have been made to transfer the unallocated payments to unearned.
			//A final transfer should have been made to offset the negative adjustment.
			Assert.AreEqual(2,transferResults.ListSplitsCur.Count);
			foreach(PaySplit split in transferResults.ListSplitsCur) {
				PaySplits.Insert(split);
			}
			foreach(PaySplits.PaySplitAssociated associated in transferResults.ListSplitsAssociated) {
				PaySplits.UpdateFSplitNum(associated.PaySplitOrig.SplitNum,associated.PaySplitLinked.SplitNum);
			}
			transfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			//Now if we went to make another transfer, nothing should be transferred.
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum,fam,transfer);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_IncomeTransfer_OverPaidProcedureDoesNotResultInMulitpleTransfers() {
			//If this test fails in the future, it may be due to the order of splits while explicitly linking.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",65,DateTime.Today,provNum:provNum);
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today,provNum:provNum,procNum:proc.ProcNum,payType:1);
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.IncomeTransferData transfer=PaymentT.BalanceAndIncomeTransfer(pat.PatNum,fam,transferPayment);
			Assert.AreEqual(2,transfer.ListSplitsCur.Count);
			//should be a transfer to move the overpayment to unearned.
			Assert.AreEqual(1,transfer.ListSplitsCur.FindAll(x => x.ProcNum!=0 && x.SplitAmt==-35).Count);
			Assert.AreEqual(1,transfer.ListSplitsCur.FindAll(x => x.UnearnedType!=0 && x.SplitAmt==35).Count);
			//insert and attempt to make another transfer.
			foreach(PaySplit split in transfer.ListSplitsCur) {
				PaySplits.Insert(split);
			}
			foreach(PaySplits.PaySplitAssociated associatedSplit in transfer.ListSplitsAssociated) {
				PaySplits.UpdateFSplitNum(associatedSplit.PaySplitOrig.SplitNum,associatedSplit.PaySplitLinked.SplitNum);
			}
			Payment secondTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			transfer.ListSplitsCur.Clear();
			transfer.ListSplitsAssociated.Clear();
			transfer=PaymentT.BalanceAndIncomeTransfer(pat.PatNum,fam,secondTransfer);
			Assert.AreEqual(0,transfer.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_ExplicitlyLinkChargeCredits_AdjustmentsAreNotCountedTwiceInTheIncomeTransferManger() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			Def unearnedType=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,"unearned1");
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today,unearnedType:unearnedType.DefNum);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",150,provNum:provNum);
			AdjustmentT.MakeAdjustment(pat.PatNum,-50,procNum:proc.ProcNum,provNum:proc.ProvNum);
			Payment transfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0,payType:1,isNew:true);
			PaymentEdit.ConstructResults linkResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>(){pat.PatNum},pat.PatNum,new List<PaySplit>()
				,transfer,new List<AccountEntry>(),true);
			Assert.AreEqual(2,linkResults.ListAccountCharges.FindAll(x => x.AmountEnd!=0).Count);
			//adjustment should appear in the list of items needing to be transferred.
			Assert.AreEqual(0,linkResults.ListAccountCharges.First(x => x.GetType()==typeof(Adjustment)).AmountEnd);//adjustment should be used up
			//procedure should have it's value without considering the adjustment
			Assert.AreEqual(100,linkResults.ListAccountCharges.First(x => x.GetType()==typeof(Procedure)).AmountEnd);//adjustment should have applied to proc
		}

		[TestMethod]
		public void PaymentEdit_ExplicitlyLinkChargeCredits_PaySplitsWithMissingLinksStillBalanceCorrectly() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			Def unearnedType=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,"unearned"+MethodBase.GetCurrentMethod().Name);
			Payment unearnedPay=PaymentT.MakePayment(pat.PatNum,150,DateTime.Today,unearnedType:unearnedType.DefNum,provNum:provNum);
			long unearnedSplitNum=PaySplits.GetForPayment(unearnedPay.PayNum).First().SplitNum;
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum);
			//Allocate unearned to the procedure, but with the negative split unlinked to simulate workaround given for a previous bug. 
			Payment allocatedUnearnedPayment=PaymentT.MakePayment(pat.PatNum,0,DateTime.Today);
			PaySplitT.CreateSplit(0,pat.PatNum,allocatedUnearnedPayment.PayNum,0,DateTime.MinValue,0,provNum,-100,unearnedType.DefNum,fSplitNum:unearnedSplitNum);
			PaySplitT.CreateSplit(0,pat.PatNum,allocatedUnearnedPayment.PayNum,0,proc.ProcDate,proc.ProcNum,provNum,100,0);
			//Run income transfer logic - should end up with a family balance of $50 in credit
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults linkResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>(){pat.PatNum},pat.PatNum,new List<PaySplit>()
				,transferPayment,new List<AccountEntry>(),true);
			Assert.AreEqual(-50,linkResults.ListAccountCharges.Sum(x => x.AmountEnd));//charges should sum to -50 (credit)
		}

		[TestMethod]
		public void PaymentEdit_ExplicitlyLinkChargeCredits_RefundsBalanceToZero() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			Def unearnedType=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,"unearned"+MethodBase.GetCurrentMethod().Name);
			Payment payment=PaymentT.MakePayment(pat.PatNum,30,DateTime.Today,unearnedType:unearnedType.DefNum);
			Payment refund=PaymentT.MakePayment(pat.PatNum,-30,DateTime.Today,unearnedType:unearnedType.DefNum);
			List<PaySplit> listPaymentSplits=PaySplits.GetForPayment(payment.PayNum);
			List<PaySplit> listRefundSplits=PaySplits.GetForPayment(refund.PayNum);
			listRefundSplits.First().FSplitNum=listPaymentSplits.First().SplitNum;
			PaySplits.Update(listRefundSplits.First());
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults linkResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>(){pat.PatNum},pat.PatNum,new List<PaySplit>()
				,transferPayment,new List<AccountEntry>(),true);
			Assert.AreEqual(0,linkResults.ListAccountCharges.Sum(x => x.AmountEnd));//splits should counteract each other.
		}
		#endregion
		#region PayPlan Tests - PayPlanEdit
		[TestMethod]
		public void PayPlanEdit_CreatePayPlanAdjustments_PayPlanWithNegAdjustmentsForPlansWithUnattachedCredits() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",135,DateTime.Today.AddMonths(-3));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Today.AddMonths(-3));
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,DateTime.Today.AddMonths(-3),provNum:0,totalAmt:195);//totalAmt since unattached credits
			List<PayPlanCharge> listCharges=PayPlanCharges.GetForPayPlan(payplan.PayPlanNum);
			double totalFutureNegAdjs=PayPlanT.GetTotalNegFutureAdjs(listCharges);
			List<PayPlanCharge> listChargesAndCredits=PayPlanEdit.CreatePayPlanAdjustments(-45,listCharges,totalFutureNegAdjs);
			Assert.AreEqual(2,listChargesAndCredits.FindAll(x => x.ChargeType==PayPlanChargeType.Debit && x.Principal < 0).Count);//2 negative debits
			//Assert that the method only created adjustments for furthest out dates.
			Assert.AreEqual(2,listChargesAndCredits.FindAll(x => x.ChargeType==PayPlanChargeType.Debit 
				&& x.ChargeDate.Month==DateTime.Today.AddMonths(2).Month).Count);//1 positive and 1 negative debit
			Assert.AreEqual(2,listChargesAndCredits.FindAll(x => x.ChargeType==PayPlanChargeType.Debit 
				&& x.ChargeDate.Month==DateTime.Today.AddMonths(3).Month).Count);
		}

		[TestMethod]
		public void PayPlanEdit_CreatePayPlanAdjustments_PayPlanWithNegAdjustmentForPlansWithAttachedCredits() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",135,DateTime.Today.AddMonths(-3));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Today.AddMonths(-3));
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,DateTime.Today.AddMonths(-3),0,new List<Procedure> {proc1,proc2 });
			List<PayPlanCharge> listCharges=PayPlanCharges.GetForPayPlan(payplan.PayPlanNum);
			double totalFutureNegAdjs=PayPlanT.GetTotalNegFutureAdjs(listCharges);
			List<PayPlanCharge> listChargesAndCredits=PayPlanEdit.CreatePayPlanAdjustments(-45,listCharges,totalFutureNegAdjs);
			Assert.AreEqual(2,listChargesAndCredits.FindAll(x => x.ChargeType==PayPlanChargeType.Debit && x.Principal < 0).Count);//2 negative debits
			//Assert that the method only created adjustments for furthest out dates.
			Assert.AreEqual(2,listChargesAndCredits.FindAll(x => x.ChargeType==PayPlanChargeType.Debit 
				&& x.ChargeDate.Month==DateTime.Today.AddMonths(2).Month).Count);//1 positive and 1 negative debit
			Assert.AreEqual(2,listChargesAndCredits.FindAll(x => x.ChargeType==PayPlanChargeType.Debit 
				&& x.ChargeDate.Month==DateTime.Today.AddMonths(3).Month).Count);
		}

		[TestMethod]
		public void PayPlanEdit_CloseOutPatPayPlan_CloseOutPatPayPlanWithAdjustments() {
			DateTime today=new DateTime(DateTime.Today.Year,DateTime.Today.Month,DateTime.Today.Day,0,0,0,DateTimeKind.Unspecified);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",100,DateTime.Today.AddMonths(-3));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",92,DateTime.Today.AddMonths(-3));
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,DateTime.Today.AddMonths(-3),0,new List<Procedure> {proc1,proc2 });
			Payment payment=PaymentT.MakePayment(pat.PatNum,30,DateTime.Today.AddMonths(-2),payplan.PayPlanNum);//make a payment for the plan
			List<PayPlanCharge> listCharges=PayPlanCharges.GetForPayPlan(payplan.PayPlanNum);
			double totalFutureNegAdjs=PayPlanT.GetTotalNegFutureAdjs(listCharges);
			List<PayPlanCharge> listChargesAndCredits=PayPlanEdit.CreatePayPlanAdjustments(-62,listCharges,totalFutureNegAdjs);//make adjustments for the plan.
			listChargesAndCredits.Add(PayPlanT.CreateNegativeCreditForAdj(pat.PatNum,payplan.PayPlanNum,-62));//add the tx credit for the adjustment
			//Balance should equal 100. $192 of completed tx - $30 payment + $-62 adjustment. 
			PayPlanCharge closeOutCharge=PayPlanEdit.CloseOutPatPayPlan(listChargesAndCredits,payplan,today);
			//List<PayPlanCharge> listFinalCharges=listChargesAndCredits.RemoveAll(x => x.ChargeDate > today.Date);
			listChargesAndCredits.RemoveAll(x => x.ChargeDate > DateTime.Today);
			listChargesAndCredits.Add(closeOutCharge);
			double debitsDue=120;//4 pay plan charges of $30 each that have come due
			double creditsOutstanding=130;//Original $192 - $62 adjustment
			Assert.AreEqual(creditsOutstanding - debitsDue,closeOutCharge.Principal);//close out charge should equal 10 (remaining debits - credits)
			//total balance should equal 100 (-30 to take out the payment that was made). 
			Assert.AreEqual(100,listChargesAndCredits.FindAll(x => x.ChargeType==PayPlanChargeType.Debit).Sum(x => x.Principal)-30); 
		}

		///<summary>If a payment plan is created with a guarantor outside the payment plan, and payments made by guarantor, don't show the payplan's procedures nor guarantor's payment as valid income transfer sources/destinations.</summary>
		[TestMethod]
		public void PayPlanEdit_IncomeXferWithPayPlanAndGuarantorPaymentsOutsideFamily() {
			DateTime today=new DateTime(DateTime.Today.Year,DateTime.Today.Month,DateTime.Today.Day,0,0,0,DateTimeKind.Unspecified);
			Patient pat1=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Patient pat2=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name+"2");
			Procedure proc1=ProcedureT.CreateProcedure(pat1,"D1120",ProcStat.C,"",100,DateTime.Today.AddMonths(-3));
			Procedure proc2=ProcedureT.CreateProcedure(pat2,"D0220",ProcStat.C,"",92,DateTime.Today.AddMonths(-3));
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat1.PatNum,100,DateTime.Today.AddMonths(-3),0,new List<Procedure> {proc1},guarantorNum:pat2.PatNum);
			Payment payment=PaymentT.MakePayment(pat2.PatNum,100,DateTime.Today.AddMonths(-2),payplan.PayPlanNum);//make a payment for the plan
			List<PayPlanCharge> listCharges=PayPlanCharges.GetForPayPlan(payplan.PayPlanNum);
			PayPlanCharge closeOutCharge=PayPlanEdit.CloseOutPatPayPlan(listCharges,payplan,today);
			//When performing an income transfer, we should see only one charge - The charge for the guarantor that's unpaid for 92, even though the guarantor paid on the payplan.
			Payment incomeTransfer=PaymentT.MakePaymentNoSplits(pat2.PatNum,0,isNew:true);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat2,incomeTransfer,new List<long>{pat2.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat2.PatNum),new Family { },incomeTransfer
					,loadData.ListSplits,new List<AccountEntry>(),pat2.PatNum,isIncomeTxfr:true);
			Assert.AreEqual(initData.AutoSplitData.ListAccountCharges.FindAll(x => x.AmountOriginal!=0).Count,1);
			Assert.AreEqual(initData.AutoSplitData.ListAccountCharges.Find(x => x.AmountOriginal!=0).AmountOriginal,92);
		}

		[TestMethod]
		public void PaySplits_CreateSplitForPayPlan_CreateAttachedSplitsForMultipleAttachedCredits() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",100,DateTime.Today.AddMonths(-3));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,DateTime.Today.AddMonths(-3));
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,100,DateTime.Today.AddMonths(-3),0,new List<Procedure> {proc1,proc2 });
			Payment payment=PaymentT.MakePayment(pat.PatNum,100,DateTime.Today.AddMonths(-2),payplan.PayPlanNum,procNum:proc1.ProcNum);//make a payment for the plan
			Payment newPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,100,DateTime.Today);
			PaymentEdit.InitData init=PaymentEdit.Init(new List<Patient> {pat },Patients.GetFamily(pat.PatNum),Patients.GetFamily(pat.PatNum),newPayment
				,new List<PaySplit>(),new List<AccountEntry>(),pat.PatNum);
			Assert.AreEqual(proc2.ProcNum,init.AutoSplitData.ListAutoSplits[0].ProcNum);
		}

		[TestMethod]
		public void PayPlanEdit_GetLoadDataForPayPlanCredits_CreditsGetCorrectlyLinkedToCorrectPayPlan() {
			Patient patInFam=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name+"famMember");
			Patient patGuar=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name+"famGuar");
			Family fam=new Family(new List<Patient>() {patGuar,patInFam});
			Procedure procForFamMember=ProcedureT.CreateProcedure(patInFam,"D1120",ProcStat.C,"",100,DateTime.Today.AddMonths(-3));
			Procedure procForFamGuar=ProcedureT.CreateProcedure(patGuar,"D0220",ProcStat.C,"",80,DateTime.Today.AddMonths(-3));
			PayPlan payplanForFamMember=PayPlanT.CreatePayPlanWithCredits(patInFam.PatNum,30,DateTime.Today.AddMonths(-1),0
				,new List<Procedure> {procForFamMember},guarantorNum:patGuar.PatNum);
			PayPlan payplanForFamGuar=PayPlanT.CreatePayPlanWithCredits(patGuar.PatNum,30,DateTime.Today.AddMonths(-1),0,new List<Procedure> {procForFamGuar});
			PayPlanEdit.PayPlanCreditLoadData data=PayPlanEdit.GetLoadDataForPayPlanCredits(patGuar.PatNum,payplanForFamGuar.PayPlanNum);
			decimal amt=PayPlanEdit.GetAccountCredits(data.ListAdjustments,data.ListTempPaySplit,data.ListPayments,data.ListClaimProcs,data.ListPayPlanCharges);
			//list of credits is supposed to include credits for pat on differen't payplans for pat. Should not take family memeber's credits into account.
			Assert.AreEqual(0,amt);
		}

		[TestMethod]
		public void PaySplits_GetUnearnedForFam_ManuallyAllocatingUnearnedForFamily() {
			//Test scenario. A user has manually allocated unearned but did not attach the final linking split to the procedure pay split. 
			//Orginal prepayment is linked on the negative allocating split, but the postive allocating split does not have the negative split attached.
			//We need to check to make sure the unearned is calculating correctly, previosly is was double counting some things and not getting correct 
			//unearned amount. 
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=new Family(new List<Patient> {pat});
			Def unearned=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,MethodBase.GetCurrentMethod().Name);
			//Create 2 separate prepayments (bug would only show when more than 1 was present)
			PaySplit prepay1=PaySplitT.CreatePrepayment(pat.PatNum,65,DateTime.Today.AddMonths(-3));
			PaySplit prepay2=PaySplitT.CreatePrepayment(pat.PatNum,5,DateTime.Today.AddMonths(-3));
			//make the procedures for the prepayments
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",65,DateTime.Today);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",5,DateTime.Today);
			//Manually allocate the prepayments to the procedures.
			Payment pay1=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Today);
			PaySplitT.CreateSplit(0,pat.PatNum,pay1.PayNum,0,prepay1.SecDateTEdit,0,0,-65,unearned.DefNum,prepay1.SplitNum);
			PaySplitT.CreateSplit(0,pat.PatNum,pay1.PayNum,0,proc1.ProcDate,proc1.ProcNum,proc1.ProvNum,65,0);
			Payment pay2=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Today);
			PaySplitT.CreateSplit(0,pat.PatNum,pay2.PayNum,0,prepay2.SecDateTEdit,0,0,-5,unearned.DefNum,prepay2.SplitNum);
			PaySplitT.CreateSplit(0,pat.PatNum,pay2.PayNum,0,proc2.ProcDate,proc2.ProcNum,proc2.ProvNum,5,0);
			decimal unearnedAmt=PaySplits.GetUnearnedForFam(fam);
			Assert.AreEqual(0,unearnedAmt);
		}
		#region Dynamic Payment Plans
		[TestMethod]
		public void PayPlanEdit_GetListExpectedCharges_ChargesAreGeneratedCorrectly() {
			//set up dynamic pay plan prefs
			PrefT.UpdateDateT(PrefName.DynamicPayPlanLastDateTime,DateTime.MinValue);
			PrefT.UpdateDateT(PrefName.DynamicPayPlanRunTime,DateTime.Now);
			//set up payment plan
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			//create the produciton that will be attached to the payment plan
			List<Procedure> listProcs=new List<Procedure>();
			List<Adjustment> listAdjs=new List<Adjustment>();
			listProcs.Add(ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",165,DateTime.Today));
			listProcs.Add(ProcedureT.CreateProcedure(pat,"D0221",ProcStat.C,"",25,DateTime.Today));
			listAdjs.Add(AdjustmentT.MakeAdjustment(pat.PatNum,10));
			PayPlan dynamicPayPlan=PayPlanT.CreateDynamicPaymentPlan(pat.PatNum,pat.PatNum,DateTime.Today,5,0,22,listProcs,listAdjs);
			//run logic to generate charges (look at recurring charges tests for reference
			List<PayPlanCharge> listChargesDb=PayPlanCharges.GetForPayPlan(dynamicPayPlan.PayPlanNum);
			List<PayPlanLink> listEntries=PayPlanLinks.GetForPayPlans(new List<long>{dynamicPayPlan.PayPlanNum});
			PayPlanTerms terms=PayPlanT.GetTerms(dynamicPayPlan,listEntries);
			List<PayPlanCharge> listChargesThisPeriod=PayPlanEdit.GetListExpectedCharges(listChargesDb,terms,fam,listEntries,dynamicPayPlan,true);
			//assert expected results
			Assert.AreEqual(22,listChargesThisPeriod.Sum(x => x.Principal));
		}

		[TestMethod]
		public void PaymentEdit_ExplicitlyLinkCredits_CreditsAreAppliedCorrectly() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			List<Procedure> listProcs=new List<Procedure>();
			List<Adjustment> listAdjs=new List<Adjustment>();
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",165,DateTime.Today,provNum:provNum);
			listProcs.Add(proc);
			listAdjs.Add(AdjustmentT.MakeAdjustment(pat.PatNum,10,procNum:proc.ProcNum,provNum:proc.ProvNum));//attached adjustment
			listAdjs.Add(AdjustmentT.MakeAdjustment(pat.PatNum,20,provNum:provNum));//unattached adjustment
			PayPlan dynamicPayPlan=PayPlanT.CreateDynamicPaymentPlan(pat.PatNum,pat.PatNum,DateTime.Today,0,0,65,listProcs,listAdjs);
			//Dynamic payment plan is set up and ready to go. Now we need to make a payment and make sure that things are getting linked correctly.
			//Create a zero charge payment to process the rest, use Guarantor
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Now,true,0,1);
			//Create paysplits for Family
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,transferPayment,new List<long> {pat.PatNum},true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat.PatNum},pat.PatNum
				,PaySplits.GetForPayment(transferPayment.PayNum),transferPayment.PayNum,false);
			PaymentEdit.ConstructResults chargeResult=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum},pat.PatNum
				,chargeData.ListPaySplits,transferPayment,new List<AccountEntry>(),loadData:loadData);
			Assert.AreEqual(0,chargeResult.ListAccountCharges.FindAll(x => x.GetType()==typeof(Adjustment)).Sum(x => x.AmountEnd));
			Assert.AreEqual(0,chargeResult.ListAccountCharges.FindAll(x => x.GetType()==typeof(Procedure)).Sum(x => x.AmountEnd));
		}
		#endregion
		#endregion
		#region AllocateUnearned
		///<summary>Make sure that when a we are allocating prepayments, that we do not allocate TP prepayments, and do not allocate hidden prepayments. </summary>
		[TestMethod]
		public void PaymentEdit_AllocateUnearned_NoAllocationForHiddenAndTpPrepayments() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("LS");
			Clinic clinic1=ClinicT.CreateClinic("Clinic1");
			//create tp prepayment of $100
			Procedure tpProc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.TP,"",100,provNum:provNum);
			PaySplit prePay=PaySplitT.CreateTpPrepayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum,clinic1.ClinicNum,tpProc.ProcNum);
			//make a hidden payment split that is attached to nothing
			PaySplit hiddenSplit=PaySplitT.CreateTpPrepayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum,clinic1.ClinicNum,isHidden:true);
			//complete a different procedure
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",200,provNum:provNum);
			//Allocate the unearned
			Family fam=Patients.GetFamily(pat.PatNum);
			List<PaySplit> listPaySplitsUnearned=new List<PaySplit>();
			Payment pay=PaymentT.MakePaymentForPrepayment(pat,clinic1);
			//unearned amount is only 100 because only 100 will show on the account. Unearned doesn't take into account hidden unearned. 
			List<PaySplits.PaySplitAssociated> retVal=PaymentEdit.AllocateUnearned(new List<AccountEntry> {new AccountEntry(proc1)}
				,ref listPaySplitsUnearned,pay,100,fam);
			//nothing should get allocated, because there was no valid unearned to allocate. 
			Assert.AreEqual(0,retVal.Count);
		}
		#endregion
		#region Helper Methods
		private void UpdatePaySplitHelper(PaySplit ps,long patNum,long provNum,long clinicNum,long payNum) {
			if(payNum!=0 && payNum==ps.PayNum) {
				ps.PatNum=patNum;
				ps.ProvNum=provNum;
				ps.ClinicNum=clinicNum;
				PaySplits.Update(ps);
			}
		}

		private void ResetPrepayments(Patient pat) {
			Family fam=Patients.GetFamily(pat.PatNum);
			_listFamPrePaySplits=PaySplits.GetPrepayForFam(fam);
			_listPosPrePay=_listFamPrePaySplits.FindAll(x => x.SplitAmt.IsGreaterThan(0));
			_listNegPrePay=_listFamPrePaySplits.FindAll(x => x.SplitAmt.IsLessThan(0));
		}
		#endregion
	}
}
