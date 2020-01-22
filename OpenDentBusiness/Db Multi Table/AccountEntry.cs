using CodeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	///<summary>Separate class for keeping track of accounting transactions (procedures, payplancharges, and adjustments).
	///This should be used when you need to get a list of all transactions for a patient and sort them, eg in FormProcSelect.</summary>
	[Serializable]
	public class AccountEntry {
		private static long AccountEntryAutoIncrementValue=1;
		///<summary>No matter which constructor is used, the AccountEntryNum will be unique and automatically assigned.</summary>
		public long AccountEntryNum = (AccountEntryAutoIncrementValue++);
		//Read only data.  Do not modify, or else the historic information will be changed.
		[XmlIgnore]
		public object Tag;
		public DateTime Date;
		public long PriKey;
		public long ProvNum;
		public long ClinicNum;
		public long PatNum;
		public decimal AmountOriginal;
		//Variables below will be changed as needed.
		public decimal AmountStart;
		///<summary>The amount remaining that needs to be paid.</summary>
		public decimal AmountEnd;
		public SplitCollection SplitCollection=new SplitCollection();

		[XmlElement(nameof(Tag))]
		public DtoObject TagXml {
			get {
				if(Tag==null) {
					return null;
				}
				return new DtoObject(Tag,Tag.GetType());
			}
			set {
				if(value==null) {
					Tag=null;
					return;
				}
				Tag=value.Obj;
			}
		}
		
		public new Type GetType() {
			return Tag.GetType();
		}

		///<summary>For serialization only.</summary>
		public AccountEntry() {
		}

		///<summary>Only for claimprocs with status NotReceived, Received, Supplemental, CapClaim.</summary>
		public AccountEntry(ClaimProc claimProc) {
			Tag=claimProc;
			Date=claimProc.DateCP;
			PriKey=claimProc.ClaimProcNum;
			if(claimProc.Status==ClaimProcStatus.NotReceived) {
				AmountOriginal=0-(decimal)(claimProc.InsPayEst+claimProc.WriteOff);
			}
			else {//Received, Supplemental, CapClaim
				AmountOriginal=0-(decimal)(claimProc.InsPayAmt+claimProc.WriteOff);
			}
			AmountStart=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=claimProc.ProvNum;
			ClinicNum=claimProc.ClinicNum;
			PatNum=claimProc.PatNum;
		}

		public AccountEntry(PayPlanCharge payPlanCharge) {
			Tag=payPlanCharge;
			Date=payPlanCharge.ChargeDate;
			PriKey=payPlanCharge.PayPlanChargeNum;
			AmountOriginal=(decimal)payPlanCharge.Principal+(decimal)payPlanCharge.Interest;
			AmountStart=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=payPlanCharge.ProvNum;
			ClinicNum=payPlanCharge.ClinicNum;
			PatNum=payPlanCharge.PatNum;
		}

		///<summary>Turns negative adjustments positive.</summary>
		public AccountEntry(Adjustment adjustment) {
			Tag=adjustment;
			Date=adjustment.AdjDate;
			PriKey=adjustment.AdjNum;
			AmountOriginal=(decimal)adjustment.AdjAmt;
			AmountStart=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=adjustment.ProvNum;
			ClinicNum=adjustment.ClinicNum;
			PatNum=adjustment.PatNum;
		}

		public AccountEntry(Procedure proc) {
			Tag=proc;
			Date=proc.ProcDate;
			PriKey=proc.ProcNum;
			AmountOriginal=(decimal)proc.ProcFeeTotal;
			AmountStart=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=proc.ProvNum;
			ClinicNum=proc.ClinicNum;
			PatNum=proc.PatNum;
		}

		public AccountEntry(ProcExtended procE) {
			Tag=procE;
			Date=procE.Proc.ProcDate;
			PriKey=procE.Proc.ProcNum;
			AmountOriginal=(decimal)procE.AmountOriginal;
			AmountStart=(decimal)procE.AmountStart;
			AmountEnd=(decimal)procE.AmountEnd;
			ProvNum=procE.Proc.ProvNum;
			ClinicNum=procE.Proc.ClinicNum;
			PatNum=procE.Proc.PatNum;
		}

		public AccountEntry(PaySplit paySplit) {
			Tag=paySplit;
			Date=paySplit.DatePay;
			PriKey=paySplit.SplitNum;
			AmountOriginal=0-(decimal)paySplit.SplitAmt;
			AmountStart=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=paySplit.ProvNum;
			SplitCollection.Add(paySplit);
			ClinicNum=paySplit.ClinicNum;
			PatNum=paySplit.PatNum;
		}

		///<summary>Similar to a claimproc, payAsTotal stores the total InsPayAmt and WriteOffAmt for a group of Pat/Prov/Clinic's on a single claim 
		///representing all of the claimprocs for that group. There is no primary key here because this is a placeholder for multiple objects.</summary>
		public AccountEntry(PayAsTotal payAsTotal) {
			Tag=payAsTotal;
			Date=payAsTotal.DateEntry;
			PriKey=0;//This is not a database object, no primary keys are available 
			AmountOriginal=0-(decimal)(payAsTotal.SummedInsPayAmt+payAsTotal.SummedWriteOff);
			AmountStart=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=payAsTotal.ProvNum;
			ClinicNum=payAsTotal.ClinicNum;
			PatNum=payAsTotal.PatNum;
		}

		///<summary></summary>
		public AccountEntry Copy(){
			return (AccountEntry)this.MemberwiseClone();
		}

		///<summary>Simple sort that sorts based on date.</summary>
		private static int AccountEntrySort(AccountEntry x,AccountEntry y) {
			return x.Date.CompareTo(y.Date);
		}

		///<summary>Gets all charges for the current patient. Returns a list of AccountEntries.</summary>
		public static List<AccountEntry> GetAccountCharges(List<PayPlanCharge> listPayPlanCharges,List<Adjustment> listAdjustments,List<Procedure> listProcs) {
			List<AccountEntry> listCharges=new List<AccountEntry>();
			for(int i=0;i<listPayPlanCharges.Count;i++) {
					if(listPayPlanCharges[i].ChargeType==PayPlanChargeType.Debit) {
						listCharges.Add(new AccountEntry(listPayPlanCharges[i]));
					}
				}
			for(int i=0;i<listAdjustments.Count;i++) {
				if(listAdjustments[i].AdjAmt>0 && listAdjustments[i].ProcNum==0) {
					listCharges.Add(new AccountEntry(listAdjustments[i]));
				}
			}
			for(int i=0;i<listProcs.Count;i++) {
				listCharges.Add(new AccountEntry(listProcs[i]));
			}
			listCharges.Sort(AccountEntrySort);
			return listCharges;
		}

		///<summary>Returns the ProcNum based on the current Tag object.
		///If Tag is null or not associated to a valid AccountEntry tag type, 0 is returned.</summary>
		public long GetProcNumFromTag() {
			switch(GetType()?.Name??"") {//Type of the this.Tag 
				case nameof(PaySplit):
					return (this.Tag as PaySplit).ProcNum;
				case nameof(Adjustment):
					return (this.Tag as Adjustment).ProcNum;
				case nameof(ClaimProc):
					return (this.Tag as ClaimProc).ProcNum;
				default:
					return 0;
			}
		}

	}
}
