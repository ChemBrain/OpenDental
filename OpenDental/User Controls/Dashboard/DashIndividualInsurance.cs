using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public partial class DashIndividualInsurance:UserControl,IDashWidgetField {
		private List<InsPlan> _listInsPlans;
		private List<InsSub> _listInsSubs;
		private List<PatPlan> _listPatPlans;
		private List<Benefit> _listBenefits;
		private List<ClaimProcHist> _histList;
		private Patient _pat;

		public DashIndividualInsurance() {
			InitializeComponent();
		}

		public double PriMax {
			get {
				return PIn.Double(textPriMax.Text);
			}
		}

		public double PriDed {
			get {
				return PIn.Double(textPriDed.Text);
			}
		}

		public double PriDedRem {
			get {
				return PIn.Double(textPriDedRem.Text);
			}
		}

		public double PriUsed {
			get {
				return PIn.Double(textPriUsed.Text);
			}
		}

		public double PriPend {
			get {
				return PIn.Double(textPriPend.Text);
			}
		}

		public double PriRem {
			get {
				return PIn.Double(textPriRem.Text);
			}
		}

		public double SecMax {
			get {
				return PIn.Double(textSecMax.Text);
			}
		}

		public double SecDed {
			get {
				return PIn.Double(textSecDed.Text);
			}
		}
		
		public double SecDedRem {
			get {
				return PIn.Double(textSecDedRem.Text);
			}
		}

		public double SecUsed {
			get {
				return PIn.Double(textSecUsed.Text);
			}
		}

		public double SecPend {
			get {
				return PIn.Double(textSecPend.Text);
			}
		}

		public double SecRem {
			get {
				return PIn.Double(textSecRem.Text);
			}
		}

		public void SetData(PatientDashboardDataEventArgs data,SheetField sheetField) {
			if(!IsNecessaryDataAvailable(data)) {
				return;
			}
			ExtractData(data);
		}

		private bool IsNecessaryDataAvailable(PatientDashboardDataEventArgs data) {
			if(data.Pat==null || data.ListInsPlans==null || data.ListInsSubs==null || data.ListPatPlans==null || data.ListBenefits==null) {
				return false;
			}
			return true;
		}

		private void ExtractData(PatientDashboardDataEventArgs data) {
			_listInsPlans=data.ListInsPlans;
			_listInsSubs=data.ListInsSubs;
			_listPatPlans=data.ListPatPlans;
			_listBenefits=data.ListBenefits;
			_histList=data.HistList??ClaimProcs.GetHistList(_pat.PatNum,_listBenefits,_listPatPlans,_listInsPlans,DateTime.Today,_listInsSubs);
			_pat=data.Pat;
		}

		public void RefreshData(Patient pat,SheetField sheetField) {
			_listInsPlans=new List<InsPlan>();
			_listInsSubs=new List<InsSub>();
			_listPatPlans=new List<PatPlan>();
			_listBenefits=new List<Benefit>();
			_histList=new List<ClaimProcHist>();
			_pat=pat;
			if(_pat==null) {
				return;
			}
			_listPatPlans=PatPlans.Refresh(_pat.PatNum);
			_listInsSubs=InsSubs.RefreshForFam(Patients.GetFamily(_pat.PatNum));
			_listInsPlans=InsPlans.RefreshForSubList(_listInsSubs);
			_listBenefits=Benefits.Refresh(_listPatPlans,_listInsSubs);
			_histList=ClaimProcs.GetHistList(_pat.PatNum,_listBenefits,_listPatPlans,_listInsPlans,DateTime.Today,_listInsSubs);
		}

		public void RefreshView() {
			RefreshInsurance(_pat,_listInsPlans,_listInsSubs,_listPatPlans,_listBenefits,_histList);
		}

		public void RefreshInsurance(Patient pat,List<InsPlan> listInsPlans,List<InsSub> listInsSubs,List<PatPlan> listPatPlans,List<Benefit> listBenefits,List<ClaimProcHist> histList){
			textPriMax.Text="";
			textPriDed.Text="";
			textPriDedRem.Text="";
			textPriUsed.Text="";
			textPriPend.Text="";
			textPriRem.Text="";
			textSecMax.Text="";
			textSecDed.Text="";
			textSecDedRem.Text="";
			textSecUsed.Text="";
			textSecPend.Text="";
			textSecRem.Text="";
			if(pat==null){
				return;
			}
			double maxInd=0;
			double ded=0;
			double dedFam=0;
			double dedRem=0;
			double remain=0;
			double pend=0;
			double used=0;
			InsPlan isnPlanCur;//=new InsPlan();
			InsSub subCur;
			if(listPatPlans.Count>0){
				subCur=InsSubs.GetSub(listPatPlans[0].InsSubNum,listInsSubs);
				isnPlanCur=InsPlans.GetPlan(subCur.PlanNum,listInsPlans);
				pend=InsPlans.GetPendingDisplay(histList,DateTime.Today,isnPlanCur,listPatPlans[0].PatPlanNum,-1,pat.PatNum,listPatPlans[0].InsSubNum,listBenefits);
				used=InsPlans.GetInsUsedDisplay(histList,DateTime.Today,isnPlanCur.PlanNum,listPatPlans[0].PatPlanNum,-1,listInsPlans,listBenefits,pat.PatNum,listPatPlans[0].InsSubNum);
				textPriPend.Text=pend.ToString("F");
				textPriUsed.Text=used.ToString("F");
				maxInd=Benefits.GetAnnualMaxDisplay(listBenefits,isnPlanCur.PlanNum,listPatPlans[0].PatPlanNum,false);
				if(maxInd==-1){//if annual max is blank
					textPriMax.Text="";
					textPriRem.Text="";
				}
				else{
					remain=maxInd-used-pend;
					if(remain<0){
						remain=0;
					}
					//textFamPriMax.Text=max.ToString("F");
					textPriMax.Text=maxInd.ToString("F");
					textPriRem.Text=remain.ToString("F");
				}
				//deductible:
				ded=Benefits.GetDeductGeneralDisplay(listBenefits,isnPlanCur.PlanNum,listPatPlans[0].PatPlanNum,BenefitCoverageLevel.Individual);
				dedFam=Benefits.GetDeductGeneralDisplay(listBenefits,isnPlanCur.PlanNum,listPatPlans[0].PatPlanNum,BenefitCoverageLevel.Family);
				if(ded!=-1){
					textPriDed.Text=ded.ToString("F");
					dedRem=InsPlans.GetDedRemainDisplay(histList,DateTime.Today,isnPlanCur.PlanNum,listPatPlans[0].PatPlanNum,-1,listInsPlans,pat.PatNum,ded,dedFam);
					textPriDedRem.Text=dedRem.ToString("F");
				}
			}
			if(listPatPlans.Count>1){
				subCur=InsSubs.GetSub(listPatPlans[1].InsSubNum,listInsSubs);
				isnPlanCur=InsPlans.GetPlan(subCur.PlanNum,listInsPlans);
				pend=InsPlans.GetPendingDisplay(histList,DateTime.Today,isnPlanCur,listPatPlans[1].PatPlanNum,-1,pat.PatNum,listPatPlans[1].InsSubNum,listBenefits);
				textSecPend.Text=pend.ToString("F");
				used=InsPlans.GetInsUsedDisplay(histList,DateTime.Today,isnPlanCur.PlanNum,listPatPlans[1].PatPlanNum,-1,listInsPlans,listBenefits,pat.PatNum,listPatPlans[1].InsSubNum);
				textSecUsed.Text=used.ToString("F");
				maxInd=Benefits.GetAnnualMaxDisplay(listBenefits,isnPlanCur.PlanNum,listPatPlans[1].PatPlanNum,false);
				if(maxInd==-1){//if annual max is blank
					textSecMax.Text="";
					textSecRem.Text="";
				}
				else{
					remain=maxInd-used-pend;
					if(remain<0){
						remain=0;
					}
					//textFamSecMax.Text=max.ToString("F");
					textSecMax.Text=maxInd.ToString("F");
					textSecRem.Text=remain.ToString("F");
				}
				//deductible:
				ded=Benefits.GetDeductGeneralDisplay(listBenefits,isnPlanCur.PlanNum,listPatPlans[1].PatPlanNum,BenefitCoverageLevel.Individual);
				dedFam=Benefits.GetDeductGeneralDisplay(listBenefits,isnPlanCur.PlanNum,listPatPlans[1].PatPlanNum,BenefitCoverageLevel.Family);
				if(ded!=-1){
					textSecDed.Text=ded.ToString("F");
					dedRem=InsPlans.GetDedRemainDisplay(histList,DateTime.Today,isnPlanCur.PlanNum,listPatPlans[1].PatPlanNum,-1,listInsPlans,pat.PatNum,ded,dedFam);
					textSecDedRem.Text=dedRem.ToString("F");
				}
			}
		}
	}
}
