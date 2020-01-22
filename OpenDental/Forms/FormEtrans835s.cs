using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormEtrans835s:ODForm {
	
		///<summary>Start date used to populate _listEtranss.</summary>
		private DateTime _reportDateFrom=DateTime.MaxValue;
		///<summary>End date used to populate _listEtranss.</summary>
		private DateTime _reportDateTo=DateTime.MaxValue;
		///<summary>List of clinics user has access to.</summary>
		private List<Clinic> _listUserClinics;
		///<summary>List of every 835 Etrans in date range for etype of EtransType.ERA_835.</summary>
		private List<Etrans> _listAllEtrans=new List<Etrans>();
		///<summary>Dictionary such that they key is an etrans.EtransNum and value is a list of paid claims associated to it from the database.
		///We allow NULL in our List, this way we know that there was a claim object that can not be found and we use this in determining the status.</summary>
		private Dictionary<long,List<Claim>> _dictEtransClaims=new Dictionary<long, List<Claim>>();
		///<summary>Dictionary such that they key is an etrans.EtransNum and value is the 835 object.</summary>
		private Dictionary<long,X835> _dictEtrans835s=new Dictionary<long, X835>();
		///<summary>All attaches for every 835.  Used to get status of each 835.</summary>
		private List<Etrans835Attach> _listAllAttaches;
		///<summary>List of all claimProcs associated to all claims for ever 835.</summary>
		private List<ClaimProc> _listAllClaimProcs;

		public FormEtrans835s() {
			InitializeComponent();
			Lan.F(this);
		}
		
		private void FormEtrans835s_Load(object sender,EventArgs e) {
			base.SetFilterControlsAndAction((() => FilterAndFillGrid())
				,dateRangePicker,textRangeMin,textRangeMax,textControlId,textCarrier,textCheckTrace,comboClinics,listStatus
			);
			dateRangePicker.SetDateTimeFrom(DateTimeOD.Today.AddDays(-7));
			dateRangePicker.SetDateTimeTo(DateTimeOD.Today);
			#region User Clinics
			if(PrefC.HasClinicsEnabled) {
				comboClinics.IsAllSelected=true;//Defaults to 'All' so that 835s with missing clinic will show.
			}
			#endregion
			#region Statuses
			foreach(X835Status status in Enum.GetValues(typeof(X835Status))) {
				if(status.In(X835Status.None,X835Status.FinalizedSomeDetached,X835Status.FinalizedAllDetached)) {
					//FinalizedSomeDetached and FinalizedAllDetached are shown via Finalized.
					continue;
				}
				listStatus.Items.Add(Lan.g(this,status.GetDescription()));
				bool isSelected=true;
				if(status==X835Status.Finalized) {
					isSelected=false;
				}
				listStatus.SetSelected(listStatus.Items.Count-1,isSelected);
			}
			#endregion
		}

		private void FormEtrans835s_Shown(object sender,EventArgs e) {
			//This must be in Shown due to the progress bar forcing this window behind other windows.
			RefreshAndFillGrid();//Will not run query, simply initilizes the grid.
		}

		///<summary>Called when we want to refresh form list and data. Also calls FillGrid().
		///Set hasFilters to true when we want to refresh and apply current filters.</summary>
		private void RefreshAndFillGrid() {
			_listAllEtrans=new List<Etrans>();
			if(ValidateFields()) {
				DataTable table=Etranss.RefreshHistory(_reportDateFrom,_reportDateTo,new List<EtransType>() { EtransType.ERA_835 });
				foreach(DataRow row in table.Rows) {
					Etrans etrans=new Etrans();
					etrans.EtransNum=PIn.Long(row["EtransNum"].ToString());
					etrans.ClaimNum=PIn.Long(row["ClaimNum"].ToString());
					etrans.Note=row["Note"].ToString();
					etrans.EtransMessageTextNum=PIn.Long(row["EtransMessageTextNum"].ToString());
					etrans.TranSetId835=row["TranSetId835"].ToString();
					etrans.UserNum=Security.CurUser.UserNum;
					etrans.DateTimeTrans=PIn.DateT(row["dateTimeTrans"].ToString());
					_listAllEtrans.Add(etrans);
				}
			}
			FilterAndFillGrid(true);
		}

		///<summary>Returns false when either _reportDateFrom or _reportDateTo are invalid.</summary>
		private bool ValidateFields() {
			_reportDateFrom=dateRangePicker.GetDateTimeFrom();
			_reportDateTo=dateRangePicker.GetDateTimeTo();
			if(PrefC.HasClinicsEnabled) {
				if(comboClinics.ListSelectedClinicNums.Count==0){
					comboClinics.IsAllSelected=true;//All clinics.
				}
			}
			if(_reportDateFrom==DateTime.MinValue || _reportDateTo==DateTime.MinValue) {
				return false;
			}
			return true;
		}

		///<summary>Fills grid based on values in _listEtrans.
		///Set isRefreshNeeded to true when we need to reinitialize local dictionarys after in memory list is also updated. Required true for first time running.
		///Also allows you to passed in predetermined filter options.</summary>
		private void FillGrid(bool isRefreshNeeded,List<string> listSelectedStatuses,List<long> listSelectedClinicNums,
			string carrierName,string checkTraceNum,string amountMin,string amountMax,string controlId)
		{
			Cursor=Cursors.WaitCursor;
			labelControlId.Visible=PrefC.GetBool(PrefName.EraShowControlIdFilter);
			textControlId.Visible=PrefC.GetBool(PrefName.EraShowControlIdFilter);
			Action actionCloseProgress=null;
			if(isRefreshNeeded) {
				actionCloseProgress=ODProgress.Show(ODEventType.Etrans,typeof(EtransEvent),Lan.g(this,"Gathering data")+"...");
				_dictEtrans835s.Clear();
				_dictEtransClaims.Clear();
				List <Etrans835Attach> listAttached=Etrans835Attaches.GetForEtrans(_listAllEtrans.Select(x => x.EtransNum).ToArray());
				Dictionary<long,string> dictEtransMessages=new Dictionary<long, string>();
				List<X12ClaimMatch> list835ClaimMatches=new List<X12ClaimMatch>();
				Dictionary<long,int> dictClaimMatchCount=new Dictionary<long,int>();//1:1 with _listEtranss. Stores how many claim matches each 835 has.
				int batchQueryInterval=500;//Every 500 rows we get the next 500 message texts to save memory.
				int rowCur=0;
				foreach(Etrans etrans in _listAllEtrans) {
					if(rowCur%batchQueryInterval==0) {
						int range=Math.Min(batchQueryInterval,_listAllEtrans.Count-rowCur);//Either the full batchQueryInterval amount or the remaining amount of etrans.
						dictEtransMessages=EtransMessageTexts.GetMessageTexts(_listAllEtrans.GetRange(rowCur,range).Select(x => x.EtransMessageTextNum).ToList(),false);
					}
					rowCur++;
					EtransEvent.Fire(ODEventType.Etrans,Lan.g(this,"Processing 835: ")+": "+rowCur+" out of "+_listAllEtrans.Count);
					List <Etrans835Attach> listAttachedTo835=listAttached.FindAll(x => x.EtransNum==etrans.EtransNum);
					X835 x835=new X835(etrans,dictEtransMessages[etrans.EtransMessageTextNum],etrans.TranSetId835,listAttachedTo835,true);
					_dictEtrans835s.Add(etrans.EtransNum,x835);
					List<X12ClaimMatch> listClaimMatches=x835.GetClaimMatches();
					dictClaimMatchCount.Add(etrans.EtransNum,listClaimMatches.Count);
					list835ClaimMatches.AddRange(listClaimMatches);
				}
				#region Set 835 unattached in batch and build _dictEtransClaims and _dictClaimPayCheckNums.
				EtransEvent.Fire(ODEventType.Etrans,Lan.g(this,"Gathering internal claim matches."));
				List<long> listClaimNums=Claims.GetClaimFromX12(list835ClaimMatches);//Can return null.
				EtransEvent.Fire(ODEventType.Etrans,Lan.g(this,"Building data sets."));
				int claimIndexCur=0;
				List<long> listMatchedClaimNums=new List<long>();
				foreach(Etrans etrans in _listAllEtrans) {
						X835 x835=_dictEtrans835s[etrans.EtransNum];
						if(listClaimNums!=null) {
							x835.SetClaimNumsForUnattached(listClaimNums.GetRange(claimIndexCur,dictClaimMatchCount[etrans.EtransNum]));
						}
						claimIndexCur+=dictClaimMatchCount[etrans.EtransNum];
						listMatchedClaimNums.AddRange(x835.ListClaimsPaid.FindAll(x => x.ClaimNum!=0).Select(x => x.ClaimNum).ToList());
				}
				List<Claim> listClaims=Claims.GetClaimsFromClaimNums(listMatchedClaimNums.Distinct().ToList());
				//The following line includes manually detached and split attaches.
				_listAllAttaches=Etrans835Attaches.GetForEtransNumOrClaimNums(false,_listAllEtrans.Select(x => x.EtransNum).ToList(),listMatchedClaimNums.ToArray());
				_listAllClaimProcs=ClaimProcs.RefreshForClaims(listMatchedClaimNums);
				foreach(Etrans etrans in _listAllEtrans) {
					X835 x835=_dictEtrans835s[etrans.EtransNum];
					#region _dictEtransClaims, _dictClaimPayCheckNums
					_dictEtransClaims.Add(etrans.EtransNum,new List<Claim>());
					List <long> listSubClaimNums=x835.ListClaimsPaid.FindAll(x => x.ClaimNum!=0).Select(y => y.ClaimNum).ToList();
					List <Claim> listClaimsFor835=listClaims.FindAll(x => listSubClaimNums.Contains(x.ClaimNum));
					foreach(Hx835_Claim claim in x835.ListClaimsPaid) {
						Claim claimCur=listClaimsFor835.FirstOrDefault(x => x.ClaimNum==claim.ClaimNum);//Can be null.
						_dictEtransClaims[etrans.EtransNum].Add(claimCur);
					}
					#endregion
				}
				EtransEvent.Fire(ODEventType.Etrans,Lan.g(this,"Filling Grid."));
				#endregion
			}
			gridMain.BeginUpdate();
			#region Initilize columns
			gridMain.ListGridColumns.Clear();
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableEtrans835s","Patient Name"),250));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableEtrans835s","Carrier Name"),190));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableEtrans835s","Status"),80));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableEtrans835s","Date"),80,GridSortingStrategy.DateParse));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableEtrans835s","Amount"),80,GridSortingStrategy.AmountParse));
			if(PrefC.HasClinicsEnabled) {
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableEtrans835s","Clinic"),70));
			}
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableEtrans835s","Code"),37,HorizontalAlignment.Center));
			if(PrefC.GetBool(PrefName.EraShowControlIdFilter)) {
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableEtrans835s","ControlID"),-1));
			}
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g("TableEtrans835s","Note"),-2));
			#endregion
			gridMain.ListGridRows.Clear();
			foreach(Etrans etrans in _listAllEtrans) {
				X835 x835=_dictEtrans835s[etrans.EtransNum];
				#region Filter: Carrier Name
				if(carrierName!="" && !x835.PayerName.ToLower().Contains(carrierName.ToLower().Trim())) {
					continue;
				}
				#endregion
				string status=GetStringStatus(etrans.EtransNum);
				#region Filter: Status
				if(!listSelectedStatuses.Contains(status.Replace("*",""))) {//The filter will ignore finalized with detached claims.
					continue;
				}
				#endregion
				//List of ClinicNums for the current etrans.ListClaimsPaid from the DB.
				List<long> listClinicNums=_dictEtransClaims[etrans.EtransNum].Select(x => x==null? 0 :x.ClinicNum).Distinct().ToList();
				#region Filter: Clinics
				if(PrefC.HasClinicsEnabled && !listClinicNums.Exists(x => listSelectedClinicNums.Contains(x))) {
					continue;//The ClinicNums associated to the 835 do not match any of the selected ClinicNums, so nothing to show in this 835.
				}
				#endregion
				#region Filter: Check and Trace Value
				if(checkTraceNum!="" && !x835.TransRefNum.ToLower().Contains(checkTraceNum.ToLower().Trim())) {//Trace Number does not match
					continue;
				}
				#endregion
				#region Filter: Insurance Check Range Min and Max
				if(amountMin!="" && x835.InsPaid < PIn.Decimal(amountMin) || amountMax!="" && x835.InsPaid > PIn.Decimal(amountMax)) {
					continue;//Either the InsPaid is below or above our range.
				}
				#endregion
				#region Filter: ControlID
				if(controlId!="" && !x835.ControlId.ToLower().Contains(controlId.ToLower())) {
					continue;
				}
				#endregion
				GridRow row=new GridRow();
				#region Column: Patient Name
				List<string> listPatNames=x835.ListClaimsPaid.Select(x => x.PatientName.ToString()).Distinct().ToList();
				string patName=(listPatNames.Count>0 ? listPatNames[0] : "");
				if(listPatNames.Count>1) {
					patName="("+POut.Long(listPatNames.Count)+")";
				}
				row.Cells.Add(patName);
				#endregion
				row.Cells.Add(x835.PayerName);
				row.Cells.Add(status);//See GetStringStatus(...) for possible values.
				row.Cells.Add(POut.Date(etrans.DateTimeTrans));
				row.Cells.Add(POut.Decimal(x835.InsPaid));
				#region Column: Clinic
				if(PrefC.HasClinicsEnabled) {	
					string clinicAbbr="";
					if(listClinicNums.Count==1) {
						if(listClinicNums[0]==0) {
							clinicAbbr=Lan.g(this,"Unassigned");
						}
						else {
							clinicAbbr=Clinics.GetAbbr(listClinicNums[0]);
						}
					}
					else if(listClinicNums.Count>1) {
						clinicAbbr="("+Lan.g(this,"Multiple")+")";
					}
					row.Cells.Add(clinicAbbr);
				}
				#endregion
				row.Cells.Add(x835._paymentMethodCode);
				if(PrefC.GetBool(PrefName.EraShowControlIdFilter)) {
					row.Cells.Add(x835.ControlId);
				}
				row.Cells.Add(etrans.Note);
				row.Tag=etrans;
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
			actionCloseProgress?.Invoke();//When this function executes quickly this can fail rarely, fail silently because of WaitCursor.
			Cursor=Cursors.Default;
		}

		private string GetStringStatus(long etransNum) {
			List<Claim> listValidClaims=_dictEtransClaims[etransNum].FindAll(x => x!=null);
			//Either description tag or enum.ToString().
			return Lan.g(this,_dictEtrans835s[etransNum].GetStatus(listValidClaims,_listAllClaimProcs,_listAllAttaches).GetDescription());
		}

		///<summary>Called when we need to filter the current in memory contents in _listEtrans. Calls FillGrid()</summary>
		private void FilterAndFillGrid(bool isRefreshNeeded=false) {
			List<string> listSelectedStatuses=new List<string>();
			foreach(int index in listStatus.SelectedIndices) {
				listSelectedStatuses.Add(listStatus.Items[index].ToString());
			}
			List<long> listClinicNums=null;//A null signifies that clinics are disabled.
			if(PrefC.HasClinicsEnabled) {
				listClinicNums=comboClinics.ListSelectedClinicNums;
			}
			FillGrid(
				isRefreshNeeded:				isRefreshNeeded,
				listSelectedStatuses:		listSelectedStatuses,
				listSelectedClinicNums:	listClinicNums,
				carrierName:						textCarrier.Text,
				checkTraceNum:					textCheckTrace.Text,
				amountMin:							textRangeMin.Text,
				amountMax:							textRangeMax.Text,
				controlId:							textControlId.Text
			);
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			RefreshAndFillGrid();
		}

		private void gridMain_DoubleClick(object sender,EventArgs e) {
			int index=gridMain.GetSelectedIndex();
			if(index==-1) {//Clicked in empty space. 
				return;
			}
			//Mimics FormClaimsSend.gridHistory_CellDoubleClick(...)
			Cursor=Cursors.WaitCursor;
			Etrans et=(Etrans)gridMain.ListGridRows[index].Tag;
			//Sadly this is needed due to FormEtrans835Edit calling Etranss.Update .
			//See Etranss.RefreshHistory(...), this query does not select all etrans columns.
			//Mimics FormClaimsSend.gridHistory_CellDoubleClick(...)
			et=Etranss.GetEtrans(et.EtransNum);
			if(et==null) {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"ERA could not be found, it was most likely deleted.");
				RefreshAndFillGrid();
				return;
			}
			EtransL.ViewFormForEra(et,this);
			Cursor=Cursors.Default;
		}
		
		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
			Close();
		}

	}

}
