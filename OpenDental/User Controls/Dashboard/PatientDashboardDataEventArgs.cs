using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public class PatientDashboardDataEventArgs {
		public Family Fam;
		public Patient Pat;
		public PatientNote PatNote;
		public List<InsSub> ListInsSubs;
		public List<InsPlan> ListInsPlans;
		public List<PatPlan> ListPatPlans;
		public List<Benefit> ListBenefits;
		public List<Recall> ListRecalls;
		public List<PatField> ListPatFields;
		public List<Patient> SuperFamilyMembers;
		public List<Patient> SuperFamilyGuarantors;
		public SerializableDictionary<Patient,Def> DictCloneSpecialities;
		public Document PatPict;
		///<summary>Is yes if we have retrieved the PatPict from the db. No if we have tried but PatPict is null. Unknown if we have not attempted
		///to retrieve the PatPict.</summary>
		public YN HasPatPict;
		public string PayorTypeDesc;
		public List<RefAttach> ListRefAttaches;
		public List<Guardian> ListGuardians;
		public List<CustRefEntry> ListCustRefEntries;
		public List<PatRestriction> ListPatRestricts;
		public List<FieldDefLink> ListPatFieldDefLinks;
		public DiscountPlan DiscountPlan;
		///<summary>Only has the fields from Patients.GetLim.</summary>
		public Patient ResponsibleParty;
		public List<PatientLink> ListMergeLinks;
		public List<Claim> ListClaims;
		public List<ClaimProcHist> HistList;
		public List<PaySplit> ListPrePayments;
		public List<RepeatCharge> ListRepeatCharges;
		public SerializableDictionary<long,DateTime> DictDateLastOrthoClaims;
		public DateTime FirstOrthoProcDate;
		public List<FieldDefLink> ListFieldDefLinksAcct;
		public List<SubstitutionLink> ListSubstLinks;
		public List<Procedure> ListProcedures;
		public List<TreatPlan> ListTreatPlans;
		public List<ProcTP>	ListProcTPs;
		public List<Document> ListDocuments;
		public List<Appointment> ListAppts;
		public List<PlannedAppt> ListPlannedAppts;
		public List<ToothInitial> ListToothInitials;
		public List<ProcGroupItem> ListProcGroupItems;
		public string PayorType;
		public List<Disease> ListDiseases;
		public DataTable TableMeds;
		public List<MedicationPat> ListMedPats;
		public List<Allergy> ListAllergies;
		public bool HasPatientPortalAccess;
		public List<FieldDefLink> ListFieldDefLinksChart;
		public List<EhrMeasureEvent> ListTobaccoStatuses;
		public List<ProcButtonQuick> ListProcButtonQuicks;
		public Patient SuperFamHead;
		public DataTable TableProgNotes;
		public DataTable TableChartViews;
		public Image ImageToothChart;
		public Bitmap BitmapImagesModule;
		public StaticTextData StaticTextData;

		public PatientDashboardDataEventArgs() {

		}

		public PatientDashboardDataEventArgs(object data) {
			if(data==null) {
				return;
			}
			if(data.GetType()==typeof(Patient)) {
				PatientDashboardDataEventArgsFactory((Patient)data);
			}
			else if(data.GetType()==typeof(FamilyModules.LoadData)) {
				PatientDashboardDataEventArgsFactory((FamilyModules.LoadData)data);
			}
			else if(data.GetType()==typeof(AccountModules.LoadData)) {
				PatientDashboardDataEventArgsFactory((AccountModules.LoadData)data);
			}
			else if(data.GetType()==typeof(TPModuleData)) {
				PatientDashboardDataEventArgsFactory((TPModuleData)data);
			}
			else if(data.GetType()==typeof(ChartModules.LoadData)) {
				PatientDashboardDataEventArgsFactory((ChartModules.LoadData)data);
			}
			else {
				throw new NotImplementedException("Unable to import "+data.GetType()+" into PatientDashboardDataEventArgs.");//Unexpected data object.
			}
			//No "LoadData" class for Images and Manage modules.
			CreateStaticTextData();
		}

		//Appointment Module
		private void PatientDashboardDataEventArgsFactory(Patient data) {
			Pat=data;
		}

		//Family Module
		private void PatientDashboardDataEventArgsFactory(FamilyModules.LoadData data) {
			Fam=data.Fam;
			Pat=data.Pat;
			PatNote=data.PatNote;
			ListInsSubs=data.ListInsSubs;
			ListInsPlans=data.ListInsPlans;
			ListPatPlans=data.ListPatPlans;
			ListBenefits=data.ListBenefits;
			ListRecalls=data.ListRecalls;
			ListPatFields=data.ArrPatFields.ToList();
			SuperFamilyMembers=data.SuperFamilyMembers;
			SuperFamilyGuarantors=data.SuperFamilyGuarantors;
			DictCloneSpecialities=data.DictCloneSpecialities;
			PatPict=data.PatPict;
			HasPatPict=data.HasPatPict;
			PayorTypeDesc=data.PayorTypeDesc;
			ListRefAttaches=data.ListRefAttaches;
			ListGuardians=data.ListGuardians;
			ListCustRefEntries=data.ListCustRefEntries;
			ListPatRestricts=data.ListPatRestricts;
			ListPatFieldDefLinks=data.ListPatFieldDefLinks;
			DiscountPlan=data.DiscountPlan;
			ResponsibleParty=data.ResponsibleParty;
			ListMergeLinks=data.ListMergeLinks;
		}

		//Account Module
		private void PatientDashboardDataEventArgsFactory(AccountModules.LoadData data) {
			//DataSetMain=data.DataSetMain;  Not implementing this for now, as I don't think it's necessary.
			Fam=data.Fam;
			PatNote=data.PatNote;
			ListPatFields=data.ArrPatFields.ToList();
			ListInsSubs=data.ListInsSubs;
			ListInsPlans=data.ListInsPlans;
			ListPatPlans=data.ListPatPlans;
			ListBenefits=data.ListBenefits;
			ListClaims=data.ListClaims;
			HistList=data.HistList;
			ListPrePayments=data.ListPrePayments;
			ListRepeatCharges=data.ArrRepeatCharges.ToList();
			///<summary>Key: PatPlanNum, Value: The date of the last ortho claim for this plan.</summary>
			DictDateLastOrthoClaims=data.DictDateLastOrthoClaims;
			FirstOrthoProcDate=data.FirstOrthoProcDate;
			ListFieldDefLinksAcct=data.ListFieldDefLinksAcct;
			ListMergeLinks=data.ListMergeLinks;
			//AccountModules.LoadData does not have a Pat, so we will figure it out from the data we do have.
			long patNum=GetPatNumFromData();
			Pat=Fam?.GetPatient(patNum);//No db calls.
		}

		//Treatment Plan Module
		private void PatientDashboardDataEventArgsFactory(TPModuleData data) {
			Fam=data.Fam;
			Pat=data.Pat;
			ListInsSubs=data.SubList;
			ListInsPlans=data.InsPlanList;
			ListPatPlans=data.PatPlanList;
			ListBenefits=data.BenefitList;
			ListClaims=data.ClaimList;
			HistList=data.HistList;
			ListSubstLinks=data.ListSubstLinks;
			ListProcedures=data.ListProcedures;
			ListTreatPlans=data.ListTreatPlans;
			ListProcTPs=data.ArrProcTPs.ToList();
		}


		//Chart Module
		private void PatientDashboardDataEventArgsFactory(ChartModules.LoadData data) {
			TableProgNotes=data.TableProgNotes;
			Pat=data.Pat;
			Fam=data.Fam;
			ListPlannedAppts=ExtractPlannedAppts(Pat,data.TablePlannedAppts);
			ListInsSubs=data.ListInsSubs;
			ListInsPlans=data.ListInsPlans;
			ListPatPlans=data.ListPatPlans;
			ListBenefits=data.ListBenefits;
			HistList=data.ListClaimProcHists;
			PatNote=data.PatNote;
			ListDocuments=data.ArrDocuments.ToList();	
			ListAppts=data.ArrAppts.ToList();
			ListToothInitials=data.ListToothInitials;
			ListPatFields=data.ArrPatFields.ToList();
			TableChartViews=data.TableChartViews;
			ListProcGroupItems=data.ListProcGroupItems;
			ListRefAttaches=data.ListRefAttaches;
			PayorType=data.PayorType;
			ListDiseases=data.ListDiseases;
			TableMeds=data.TableMeds;
			ListMedPats=data.ListMedPats;
			ListAllergies=data.ListAllergies;
			HasPatientPortalAccess=data.HasPatientPortalAccess;
			ListFieldDefLinksChart=data.ListFieldDefLinks;
			ListTobaccoStatuses=data.ListTobaccoStatuses;
			ListPatRestricts=data.ListPatRestricts;
			ListProcButtonQuicks=data.ListProcButtonQuicks;
			SuperFamHead=data.SuperFamHead;
			ImageToothChart=data.ToothChartBM;
		}

		private List<PlannedAppt> ExtractPlannedAppts(Patient pat,DataTable tablePlannedAppts) {
			List<PlannedAppt> listPlannedAppts=new List<PlannedAppt>();
			//Essentially a TableToList method, but since the DataTable doesn't have a PatNum row, pull from the Pat object.
			for(int i=0;i<tablePlannedAppts.Rows.Count;i++) {
				DataRow row=tablePlannedAppts.Rows[i];
				PlannedAppt plannedAppt=new PlannedAppt();
				plannedAppt.PlannedApptNum= PIn.Long  (row["PlannedApptNum"].ToString());
				plannedAppt.PatNum        = pat.PatNum;//data.TablePlannedAppts will not have PatNum column.
				plannedAppt.AptNum        = PIn.Long  (row["AptNum"].ToString());
				plannedAppt.ItemOrder     = PIn.Int   (row["ItemOrder"].ToString());
				listPlannedAppts.Add(plannedAppt);
			}
			return listPlannedAppts.OrderBy(x => x.ItemOrder).ToList();
		}

		private void CreateStaticTextData() {
			if(Pat==null) {
				return;
			}
			//Procedure CodeNums-------------------------------------------------------------------------------------------------------------
			List<long> listProcCodeNums=new List<long>();
			//ListIntraoralAndBiteWings consists of the following proccodes: D0210, D0270, D0272, D0274, D0277, D0273
			List<long> listIntraoralAndBiteWings=new List<long>();
			//listOralEvals consists of the following proccodes: D0120, D0140, D0150, D0160
			List<long> listOralEvals=new List<long>();
			//listProphy consists of the following proccodes: D1110, D1120, D1201, D1205
			List<long> listProphy=new List<long>();
			List<long> listIntraoralComplete=new List<long>();
			List<long> listBiteWingSingle=new List<long>();
			List<long> listBiteWingsTwo=new List<long>();
			List<long> listBiteWingsFour=new List<long>();
			List<long> listVertBiteWings7to8=new List<long>();
			List<long> listBiteWingsThree=new List<long>();
			List<long> listPerioOralEval=new List<long>();
			List<long> listLimitedOralEval=new List<long>();
			List<long> listCompOralEval=new List<long>();
			List<long> listDetailedExtensiveOralEval=new List<long>();
			List<long> listPerioMaintenance=new List<long>(); 
			List<long> listPanoramicFilm=new List<long>();
			List<long> listProphylaxisAdult=new List<long>();
			List<long> listProphylaxisChild=new List<long>();
			List<long> listTopicalFluorideProphyChild=new List<long>();
			List<long> listTopicalFluorideProphyAdult=new List<long>();
			listProcCodeNums=SheetFiller.GetListProcCodeNumsForStaticText(listIntraoralAndBiteWings,listOralEvals,listProphy,ref listIntraoralComplete
				,ref listBiteWingSingle,ref listBiteWingsTwo,ref listBiteWingsThree,ref listBiteWingsFour,ref listVertBiteWings7to8,ref listPerioOralEval
				,ref listLimitedOralEval,ref listCompOralEval,ref listDetailedExtensiveOralEval,ref listPerioMaintenance,ref listPanoramicFilm
				,ref listProphylaxisAdult,ref listProphylaxisChild,ref listTopicalFluorideProphyChild,ref listTopicalFluorideProphyAdult);
			//Create a new StaticTextData object, using data we already have, supplementing with queried data where necessary.
			StaticTextData=new StaticTextData();
			StaticTextData.PatNote=PatNote;
			StaticTextData.ListRefAttaches=ListRefAttaches;
			StaticTextData.ListInsSubs=ListInsSubs;
			StaticTextData.ListInsPlans=ListInsPlans;
			StaticTextData.ListPatPlans=ListPatPlans;
			StaticTextData.ListBenefits=ListBenefits;
			StaticTextData.HistList=HistList;
			StaticTextData.ListTreatPlans=ListTreatPlans;
			StaticTextData.ListRecallsForFam=ListRecalls;
			StaticTextData.ListAppts=ListAppts;
			StaticTextData.ListFutureApptsForFam=ListAppts?.FindAll(x => x.AptDateTime>DateTime.Now && x.AptStatus==ApptStatus.Scheduled);
			StaticTextData.ListDiseases=ListDiseases;
			StaticTextData.ListAllergies=ListAllergies;
			StaticTextData.ListMedicationPats=ListMedPats;
			//StaticTextData.ListFamPopups=Popups.GetForFamily(Pat);//Will be handled by StaticTextData.GetStaticTextData() if necessary.
			StaticTextData.ListProceduresSome=ListProcedures?.FindAll(x => x.CodeNum.In(listProcCodeNums));
			StaticTextData.ListDocuments=ListDocuments;
			StaticTextData.ListProceduresPat=ListProcedures;
			StaticTextData.ListPlannedAppts=ListPlannedAppts;
		}

		private long GetPatNumFromData() {
			if(PatNote!=null) {
				return PatNote.PatNum;
			}
			else if(ListClaims!=null && ListClaims.Count>0) {
				return ListClaims.FirstOrDefault().PatNum;
			}
			else if(ListPatPlans!=null && ListPatPlans.Count>0) {
				return ListPatPlans.FirstOrDefault().PatNum;
			}
			return 0;
		}
	}
}
