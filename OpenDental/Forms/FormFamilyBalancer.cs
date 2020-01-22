using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormFamilyBalancer:ODForm {
		///<summary>Stores the families that the tool is currently working with.
		///Guarantor.PatNum is the key, and the data for the entire family is the value.</summary>
		private Dictionary<long,FamilyAccount> _dictCurrentFamilyBatch;
		///<summary>A list of GuarantorNums split into groups of 500 or less.  (The batches of families for the tool)</summary>
		private List<List<long>> _listBatches;
		///<summary>1-based.  Tracks the tool's current position in _listBatches for when we go to fetch more families from the database.</summary>
		private int _batchNum;
		///<summary>Controls how many families to fetch at once. Consider enhancing to let the user set this value.</summary>
		private const int BATCH_SIZE=500;
		private LogWriter _logger;
		
		public FormFamilyBalancer() {
			InitializeComponent();
			Lan.F(this);
		}
		
		private void FormFamilyBalancer_Load(object sender, System.EventArgs e) {
			//Get list of guarantor patnums, alphabetical by LName,FName.
			List<long> listGuarantors=Patients.GetAllGuarantorsWithFamiliesAlphabetical();
			//Split into lists of BATCH_SIZE
			_listBatches=listGuarantors.Select((x,i) => new { Index=i,Value=x })//Make an anonymous type to access index.
				.GroupBy(x => x.Index/BATCH_SIZE)//Divide Index by BATCH_SIZE and group by this value. 
				.Select(x => x.Select(y => y.Value).ToList())//Creates sub-lists from our groups no larger than BATCH_SIZE. 
				.ToList();//Store all sub-lists in a master list.
			_batchNum=0;
			datePicker.Value=DateTime.Now;
			datePicker.MaxDate=DateTime.Now;//Users can only pick dates in the past.
			labelBatchCount.Text=$"Current batch: {_batchNum} Total batches: {_listBatches.Count()}";
			_logger=new LogWriter(LogLevel.Information,"Family Balancer");
		}

		///<summary>Using shown so we can properly display a progress bar on the initial fillgrid.</summary>
		private void FormFamilyBalancer_Shown(object sender,EventArgs e) {
			Action actionCloseProgress=ODProgress.Show(ODEventType.Billing,startingMessage:"Loading first batch.  Please wait...");
			FillNextBatch();
			actionCloseProgress();
		}

		///<summary>Fills _dictCurrentFamilyBatch with FamilyAccount entries, giving us the guarantor and all the family members.  Slow on large databases.</summary>
		private void FillNextBatch() {
			//at the last batch
			if(_batchNum>=_listBatches.Count()) {
				butGetNextBatch.Enabled=false;
				return;
			}
			try {
				_dictCurrentFamilyBatch=new Dictionary<long,FamilyAccount>();
				List<Family> listFamiliesInBatch=Patients.GetFamilies(_listBatches[_batchNum]);
				listFamiliesInBatch=listFamiliesInBatch.OrderBy(x => x.Guarantor.LName).ToList();
				foreach(Family fam in listFamiliesInBatch) {
					_dictCurrentFamilyBatch.Add(fam.Guarantor.PatNum,new FamilyAccount(fam.ListPats.ToList(),fam.Guarantor));
				}
				_batchNum++;
			}
			catch(System.OutOfMemoryException) {
				//Above we are passing around full patient and family objects, which contain a lot of data.  
				//In cases where this tool is being run against thousands of families, memory can fill up very quickly.
				//An attempt is made to manage it using dictionaries and tricks to lessen amount of memory used.
				//It can still run out, and when it does we want it to fail gracefully(ish).
				_dictCurrentFamilyBatch=new Dictionary<long,FamilyAccount>();
				GC.Collect();
				MsgBox.Show(this,"The system has run out of memory. "+
					"In order to run this tool on the current database, increase the amount of memory available.");
				this.Close();
			}
			catch(Exception e) {
				MsgBox.Show(e.Message);
			}
			if(_batchNum>=_listBatches.Count()) {
				butGetNextBatch.Enabled=false;
			}
			FillGridMain();
		}

		///<summary>Fills the main grid.  If reload Data is true, account data will be (re)fetched from the database.  
		///If false then data already in memory is used.</summary>
		private void FillGridMain() {
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col;
			col=new GridColumn(Lan.g(this,"Name"),240);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g(this,"Balance"),100,GridSortingStrategy.AmountParse);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			PaymentEdit.ConstructResults results;
			//Make a row for every guarantor that has family members with positive and negative balances.
			List<long> listPatNumsForBatch=_dictCurrentFamilyBatch.Values.Select(x => x.ListFamilyMembers)
				.SelectMany(y => y.Select(z => z.PatNum)).ToList();
			List<PaySplit> listSplitsForBatch=PaySplits.GetForPats(listPatNumsForBatch);
			List<ClaimProc> listClaimsPayAsTotalForBatch=ClaimProcs.GetByTotForPats(listPatNumsForBatch);
			foreach(KeyValuePair<long,FamilyAccount> kvp in _dictCurrentFamilyBatch) {
				//Get all family members now so we cut down on memory used.
				FamilyAccount famAccount=kvp.Value;
				List<Patient> listPatients=famAccount.ListFamilyMembers;
				List<long> listFamilyPatNums=listPatients.Select(x => x.PatNum).ToList();
				long guarantorNum=kvp.Key;
				List<PaySplit> listSplitsForPats=listSplitsForBatch.FindAll(x => x.PatNum.In(listFamilyPatNums));
				long unallocatedTransferPayNum=PaymentEdit.CreateAndInsertUnallocatedPayment(listPatients.First(x => x.PatNum==guarantorNum));
				if(!listSplitsForPats.IsNullOrEmpty()) {
					PaymentEdit.IncomeTransferData txfrResults=PaymentEdit.TransferUnallocatedSplitToUnearned(listSplitsForPats,unallocatedTransferPayNum);
					foreach(PaySplit split in txfrResults.ListSplitsCur) {
						split.PayNum=unallocatedTransferPayNum;//Set the PayNum because it was purposefully set to 0 above to save queries.
						PaySplits.Insert(split);//Need to insert in a loop to get the PrimaryKey
					}
					foreach(PaySplits.PaySplitAssociated splitAssociated in txfrResults.ListSplitsAssociated) {
						if(splitAssociated.PaySplitLinked!=null && splitAssociated.PaySplitOrig!=null) {
							PaySplits.UpdateFSplitNum(splitAssociated.PaySplitOrig.SplitNum,splitAssociated.PaySplitLinked.SplitNum);
						}
					}
				}
				List<ClaimProc> listClaimsAsTotalForPats=listClaimsPayAsTotalForBatch.FindAll(x => x.PatNum.In(listFamilyPatNums));
				ClaimProcs.TransferClaimsAsTotalToProcedures(listPatients.Select(x => x.PatNum).ToList(),listClaimsAsTotalForPats);
				results=PaymentEdit.ConstructAndLinkChargeCredits(listPatients.Select(x => x.PatNum).ToList(),guarantorNum,new List<PaySplit>(),
					new Payment(),new List<AccountEntry>(),true);
				famAccount.Account=results;
				List<AccountEntry> listAccountEntries=results.ListAccountCharges;
				//Get guarantor info and fill the row/grid
				Patient guarantor=listPatients.FirstOrDefault(x => x.PatNum==guarantorNum);
				row=new GridRow();
				row.Cells.Add(guarantor.GetNameLFnoPref());
				row.Cells.Add((listAccountEntries.Sum(x => x.AmountEnd)).ToString("f"));
				row.Tag=famAccount;//Store relevant family info in the guarantor row.
				row.DropDownInitiallyDown=false;
				row.Bold=true;//Bold parent rows to show it is giving the family balance.
				gridMain.ListGridRows.Add(row);
				//Make child rows
				foreach(Patient p in listPatients) {
					GridRow rowChild=new GridRow();
					rowChild.Cells.Add(p.GetNameLFnoPref());
					rowChild.Cells.Add(listAccountEntries.Where(x => x.PatNum==p.PatNum).Sum(x => x.AmountEnd).ToString("f"));
					rowChild.DropDownParent=row;
					gridMain.ListGridRows.Add(rowChild);
				}
				
			}
			gridMain.EndUpdate();
			gridMain.Update();
			labelBatchCount.Text=$"Current batch: {_batchNum} Total batches: {_listBatches.Count()}";
			//Invalidate and update the label to force it to be in sync with the progress bar that is on a separate thread.
			labelBatchCount.Invalidate();
			labelBatchCount.Update();
		}

		///<summary>Transfers all family balance to the guarantor, balancing the account to the best of our ability.
		///Returns a log of what was moved from the family member(s) of the selected guarantor(s)</summary>
		private string TransferToGuarantor() {
			string summaryText="";
			//Iterate through every family.
			foreach(FamilyAccount famAccountCur in _dictCurrentFamilyBatch.Select(x => x.Value)) {
				double totalTransferAmount=0;
				long provNum=famAccountCur.Guarantor.PriProv;
				Payment payCur=CreatePaymentTransferHelper(famAccountCur.Guarantor);
				List<PaySplit> listPaySplitsCreated=new List<PaySplit>();
				#region Family PaySplits        
				string logText="";
				foreach(Patient pat in famAccountCur.ListFamilyMembers) { //Make a counteracting split for each patient.
					//Don't make a split for the Guarantor yet.
					if(pat.PatNum==famAccountCur.Guarantor.PatNum) {
						continue;
					}
					//Check the family member's balance and skip if it is $0.00.
					double splitAmt=(double)famAccountCur.Account.ListAccountCharges.Where(x => x.PatNum==pat.PatNum).Sum(x => x.AmountEnd);
					if(splitAmt==0) {
						continue;
					}
					PaySplit paySplit=new PaySplit();
					paySplit.DatePay=datePicker.Value;
					paySplit.PatNum=pat.PatNum;
					paySplit.PayNum=payCur.PayNum;
					paySplit.ProvNum=pat.PriProv;
					paySplit.ClinicNum=payCur.ClinicNum;
					//Since we're transferring all family member balances to the guarantor, we set the split amount to their balance.
					paySplit.SplitAmt=splitAmt;
					totalTransferAmount+=paySplit.SplitAmt;
					listPaySplitsCreated.Add(paySplit);
					if(logText!="") {
						logText+=", ";
					}
					logText+=paySplit.SplitAmt.ToString("f");
				}
				#endregion Family PaySplits
				if(listPaySplitsCreated.Count==0) {
					//No splits were made, delete payment and skip guarantor.
					Payments.Delete(payCur);
					continue;
				}
				//Since we skipped the guarantor before, we make one for them now.
				PaySplit splitGuarantor=new PaySplit();
				splitGuarantor.DatePay=datePicker.Value;
				splitGuarantor.PatNum=famAccountCur.Guarantor.PatNum;
				splitGuarantor.PayNum=payCur.PayNum;
				splitGuarantor.ProvNum=provNum;
				splitGuarantor.ClinicNum=payCur.ClinicNum;
				splitGuarantor.SplitAmt=0-totalTransferAmount;//Split is the opposite amount of the total of the other splits.
				if(splitGuarantor.SplitAmt!=0) {
					listPaySplitsCreated.Add(splitGuarantor);
				}
				//Insert paysplits all at once for speed.
				PaySplits.InsertMany(listPaySplitsCreated);
				List<Patient> listTransferredPats=famAccountCur.ListFamilyMembers.FindAll(x =>
					x.PatNum!=famAccountCur.Guarantor.PatNum
					&& listPaySplitsCreated.Select(y => y.PatNum).ToList().Contains(x.PatNum));
				payCur.PayNote="Auto-created by Family Balancer tool "+MiscData.GetNowDateTime().ToString("MM/dd/yyyy")+"\r\n"
					+"Allocated "
					+logText+$" transfers from family member{(famAccountCur.ListFamilyMembers.Count>1 ? "s " : " ")}"
					+string.Join(", ",listTransferredPats.Select(x => x.FName).ToList())
					+" to guarantor "+famAccountCur.Guarantor.FName+".";
				//Shown after all family members have been processed.
				summaryText+="PatNum(s):"+string.Join(", ",listTransferredPats.Select(x => x.PatNum).ToList()) 
					+" moved to guarantor: "+famAccountCur.Guarantor.PatNum+"; Amount(s): "+logText+"\r\n";
				Payments.Update(payCur,true);
			}
			return summaryText;
		}

		///<summary>Creates micro-allocations intelligently based on most to least matching criteria of selected charges.</summary>
		private string CreateTransfers(List<AccountEntry> listPosCharges,List<AccountEntry> listNegCharges,List<AccountEntry> listAccountEntries
			,FamilyAccount famAccount,Payment payCur) 
		{
			PaymentEdit.IncomeTransferData transferResults=new PaymentEdit.IncomeTransferData();
			List<PayPlanCharge> listCredits=famAccount.Account.ListPayPlanCharges.FindAll(x => x.ChargeType==PayPlanChargeType.Credit);
			string retVal="";
			transferResults=PaymentEdit.CreatePayplanLoop(listPosCharges,listNegCharges,listAccountEntries,payCur.PayNum,listCredits,datePicker.Value);
			transferResults.MergeIncomeTransferData(PaymentEdit.CreateTransferLoop(listPosCharges,listNegCharges,listAccountEntries,payCur.PayNum
				,listCredits,datePicker.Value,famAccount.Guarantor.PatNum));
			famAccount.ListSplits.AddRange(transferResults.ListSplitsCur);
			famAccount.ListSplitsAssociated.AddRange(transferResults.ListSplitsAssociated);
			retVal+=transferResults.SummaryText;
			return retVal;
		}

		///<summary>Creates and inserts a payment similar to the load logic for the income transfer manager.</summary>
		private Payment CreatePaymentTransferHelper(Patient pat) {
			Payment payCur=new Payment();
			payCur.ClinicNum=0;
			if(PrefC.HasClinicsEnabled) {//if clinics aren't enabled default to 0
				payCur.ClinicNum=Clinics.ClinicNum;
				if((PayClinicSetting)PrefC.GetInt(PrefName.PaymentClinicSetting)==PayClinicSetting.PatientDefaultClinic
					|| (Clinics.ClinicNum==0 && (PayClinicSetting)PrefC.GetInt(PrefName.PaymentClinicSetting)==PayClinicSetting.SelectedExceptHQ))
				{
					payCur.ClinicNum=pat.ClinicNum;
				}
			}
			payCur.DateEntry=DateTime.Now;
			payCur.PatNum=pat.PatNum;
			payCur.PayDate=datePicker.Value;
			payCur.PaymentSource=CreditCardSource.None;
			payCur.ProcessStatus=ProcessStat.OfficeProcessed;
			payCur.PayType=0;//Income transfer (will always be income transfer).
			payCur.PayAmt=0;//Income transfer payment.
			payCur.PayNum=Payments.Insert(payCur);//Insert and get the payment PayNum which is used with linking the splits.
			SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,pat.PatNum,$"{Patients.GetLim(pat.PatNum).GetNameLF()}, created by Family Balancer tool.");
			return payCur;
		}

		///<summary>Turns all entries in listCredits into Unallocated. Needs to be called for each family after creating transfers. </summary>
		private string CreditsToUnallocated(List<AccountEntry> listCredits,FamilyAccount famAccount,Payment payCur) {
			List<PaySplit> listPaySplitsCreated=new List<PaySplit>();
			string retVal="";
			//Loop through each family member as unallocated is by patient, so each family member needs their own paysplit. 
			foreach(Patient patCur in famAccount.ListFamilyMembers) {
				//Get all non-zero credits for this patient.
				List<AccountEntry> listCreditsForPat=listCredits.FindAll(x => x.PatNum==patCur.PatNum && x.AmountEnd!=0);
				//Skip this pat if there are no credits.
				if(!listCreditsForPat.Any()) {
					continue;
				}
				bool doInsertSplits=false;
				//One large paysplit into unallocated
				PaySplit splitUnallocated=new PaySplit();
				splitUnallocated.DatePay=datePicker.Value;
				splitUnallocated.PatNum=patCur.PatNum;
				splitUnallocated.PayNum=payCur.PayNum;
				splitUnallocated.ProvNum=0;
				splitUnallocated.ClinicNum=0;
				splitUnallocated.UnearnedType=PrefC.GetLong(PrefName.PrepaymentUnearnedType);
				//create offsetting paysplits for each credit
				double totalCreditAmount=0;
				foreach(AccountEntry credit in listCreditsForPat) {
					//Prevents us from spliting unearned into unearned, which would be a major problem.
					if(credit.GetType()==typeof(PaySplit) && ((PaySplit)credit.Tag).UnearnedType!=0) {
						continue;
					}
					PaySplit splitCredit=new PaySplit();
					splitCredit.DatePay=datePicker.Value;
					splitCredit.PatNum=credit.PatNum;
					splitCredit.PayNum=payCur.PayNum;
					splitCredit.ProvNum=credit.ProvNum;
					splitCredit.ClinicNum=credit.ClinicNum;
					splitCredit.UnearnedType=0;
					splitCredit.ProcNum=credit.GetType()==typeof(PaySplit) ? ((PaySplit)credit.Tag).ProcNum : credit.GetType()==typeof(Adjustment) ? ((Adjustment)credit.Tag).ProcNum : 0;
					splitCredit.AdjNum=credit.GetType()==typeof(Adjustment) ? credit.PriKey : 0;//Money may be coming from an adjustment.
					//Since we're moving all credits to unallocated, we set the split amount to the balance.
					splitCredit.SplitAmt=(double)credit.AmountEnd;
					totalCreditAmount+=splitCredit.SplitAmt;
					credit.SplitCollection.Add(splitCredit);
					listPaySplitsCreated.Add(splitCredit);
					//A split for a credit was added, flag that we want to split to unearned.
					doInsertSplits=true;
					credit.AmountEnd=0;
				}
				if(doInsertSplits) {
					splitUnallocated.SplitAmt=-totalCreditAmount;//Split is the opposite amount of the total of the other splits.
					retVal+=$"Total credits of {splitUnallocated.SplitAmt.ToString("f")} moved to unallocated for PatNum: {patCur.PatNum}\r\n";
					listPaySplitsCreated.Add(splitUnallocated);
				}
			}
			famAccount.ListSplits.AddRange(listPaySplitsCreated);
			return retVal;
		}

		private void butGetNextBatch_Click(object sender,EventArgs e) {
			Action actionCloseProgress=ODProgress.Show(ODEventType.Billing,startingMessage:"Retrieving family information.  Please wait...");
			ODEvent.Fire(ODEventType.Billing,Lan.g(this,"Filling the grid.  This may take awhile..."));
			FillNextBatch();
			actionCloseProgress();
		}

		private void butGridSelectAll_Click(object sender,EventArgs e) {
			gridMain.SetSelected(true);
		}

		private void butGridSelectNone_Click(object sender,EventArgs e) {
			gridMain.SetSelected(false);
		}

		///<summary>Balances all selected accounts.</summary>
		private void butTransfer_Click(object sender,EventArgs e) {
			//FormIncomeTransferManage requires PaymentCreate to run.
			//This form requires SecurityAdmin and a password to open, and although rare, 
			// a SecuirtyAdmin doesn't have to have PaymentCreate permission
			if(!Security.IsAuthorized(Permissions.PaymentCreate)) {
				return;
			}
			//Make sure the user wants to run the tool.
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"This process can take a long time and cannot be reversed.\r\n\r\nContinue?")) {
				return;
			}
			Action actionCloseProgress=ODProgress.Show(ODEventType.Billing);
			//Build list of families based off of selected rows.
			_logger.WriteLine("This is the summary of all transactions that took place.This can be saved for later reference if one or more"
			   +" transactions need to be undone outside of the tool.\r\n",LogLevel.Information);
			//Do income transfers if applicable.
			for(int i=_batchNum-1;i<_listBatches.Count();i++) {//_batchNum is 1-based, so drop i by 1 to make sure the current batch is included
				string logText="";
				if(checkAllocateCharges.Checked) {
					ODEvent.Fire(ODEventType.Billing,Lan.g(this,$"Creating income transfers for batch {_batchNum}/{_listBatches.Count()} please wait..."));
					foreach(FamilyAccount famAccountCur in _dictCurrentFamilyBatch.Select(x => x.Value)) {
						//Clear the list of splits in case any are hanging around from a previous run.
						famAccountCur.ListSplits.Clear();
						famAccountCur.ListSplitsAssociated.Clear();
						//Make lists of positive and negative charges.
						List<AccountEntry> listPosCharges=famAccountCur.Account.ListAccountCharges.Where(x => x.AmountEnd>0).ToList();
						List<AccountEntry> listNegCharges=famAccountCur.Account.ListAccountCharges.Where(x => x.AmountEnd<0).ToList();
						List<long> listPatNumsForCharges=listPosCharges.Select(x => x.PatNum).Distinct().ToList();
						List<AccountEntry> listEntriesForPats=famAccountCur.Account.ListAccountCharges.FindAll(x => x.PatNum.In(listPatNumsForCharges));
						//This catch will save us some time if they run the tool on the same family twice.
						if(listPosCharges.Count()==0 || listNegCharges.Count()==0) {
							continue;//No need to add to logText, CreateTransfers wouldn't return anything either.
						}
						Payment payCur=CreatePaymentTransferHelper(famAccountCur.Guarantor);
						logText+=CreateTransfers(listPosCharges,listNegCharges,listEntriesForPats,famAccountCur,payCur);
						logText+=CreditsToUnallocated(listNegCharges,famAccountCur,payCur);
						//Remove any $0 splits created from CreateTransfers.
						famAccountCur.ListSplits.RemoveAll(x => x.SplitAmt==0);
						foreach(PaySplit split in famAccountCur.ListSplits) {
							PaySplits.Insert(split);
						}
						//Go through family accounts and update FSplitNums.
						foreach(PaySplits.PaySplitAssociated split in famAccountCur.ListSplitsAssociated) {
							//Update the FSplitNum after inserts are made. 
							if(split.PaySplitLinked!=null && split.PaySplitOrig!=null) {
								PaySplits.UpdateFSplitNum(split.PaySplitOrig.SplitNum,split.PaySplitLinked.SplitNum);
							}
						}
					}
				}
				//Transfer balances to guarantor if applicable.	
				if(checkGuarAllocate.Checked) {
					ODEvent.Fire(ODEventType.Billing,Lan.g(this,$"Transferring remaining balance to guarantor for batch {_batchNum}/{_listBatches.Count()} please wait..."));
					logText+="Balances transferred to Guarantor:\r\n";
					logText+=TransferToGuarantor();
				}
				//load up the next batch
				ODEvent.Fire(ODEventType.Billing,Lan.g(this,$"Loading next batch please wait..."));
				FillNextBatch();
				_logger.WriteLine(logText,LogLevel.Information);
			}
			actionCloseProgress();
		}
	
		private void butCancel_Click(object sender,EventArgs e) {
			this.Close();
		}
	}
}

