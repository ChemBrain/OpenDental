using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Drawing.Drawing2D;
using CodeBase;

namespace OpenDental {
	public partial class DashToothChart:PictureBox,IDashWidgetField {
		private Image _imgToothChart;
		private SheetField _sheetField;
		public DashToothChart() {
			InitializeComponent();
		}

		public void SetData(PatientDashboardDataEventArgs data,SheetField sheetField) {
			if(!IsNecessaryDataAvailable(data)) {
				return;
			}
			Image imgToothChart=ExtractToothChart(data);
			_imgToothChart?.Dispose();
			_imgToothChart=imgToothChart;
		}

		private bool IsNecessaryDataAvailable(PatientDashboardDataEventArgs data) {
			if(data.Pat==null || (data.ImageToothChart==null && data.ListTreatPlans==null && data.ListProcedures==null)) {
				return false;
			}
			return true;
		}

		private Image ExtractToothChart(PatientDashboardDataEventArgs data) {
			if(data.ImageToothChart!=null) {//Check for this first, since it will be quicker than creating an image from the active treatment plan.
				return (Image)data.ImageToothChart.Clone();
			}
			else if(data.ListTreatPlans!=null && data.ListProcedures!=null) {
				TreatPlan treatPlan=data.ListTreatPlans.FirstOrDefault(x => x.TPStatus==TreatPlanStatus.Active);
				return GetToothChart(data.Pat.PatNum,treatPlan,data.ListProcedures);
			}
			return null;
		}

		public void RefreshData(Patient pat,SheetField sheetField) {
			long patNum=pat?.PatNum??0;
			_sheetField=sheetField;
			TreatPlan treatPlan=TreatPlans.GetActiveForPat(patNum);
			Image imgToothChart=GetToothChart(patNum,treatPlan);
			_imgToothChart?.Dispose();
			_imgToothChart=imgToothChart;
		}

		private Image GetToothChart(long patNum,TreatPlan treatPlan,List<Procedure> listProcs=null) {
			if(treatPlan!=null && treatPlan.ListProcTPs.IsNullOrEmpty()) {
				listProcs=listProcs??Procedures.Refresh(patNum);
				treatPlan.ListProcTPs=GetTreatProcTPs(listProcs);
			}
			List<Procedure> listProcsFiltered=SheetPrinting.FilterProceduresForToothChart(listProcs,treatPlan,true);
			return SheetPrinting.GetToothChartHelper(patNum,true,treatPlan,listProcsFiltered,isInPatientDashboard:true);
		} 

		///<summary>Returns a list of limited versions of ProcTP corresponding to the Procedures in listProcs.  Intended to mimic the logic in 
		///ContrTreat.LoadActiveTP, on a limited data basis in order to extract the necessary data to create a ToothChart.</summary>
		private List<ProcTP> GetTreatProcTPs(List<Procedure> listProcsAll) {
			List<Procedure> listProcs=listProcsAll.FindAll(x => x.ProcStatus.In(ProcStat.TP,ProcStat.TPi));
			List<ProcTP> listProcTPs=new List<ProcTP>();
			foreach(Procedure proc in listProcs) {
				ProcTP procTP=new ProcTP();
				procTP.ProcNumOrig=proc.ProcNum;
				procTP.ToothNumTP=Tooth.ToInternat(proc.ToothNum);
				procTP.ProcCode=ProcedureCodes.GetStringProcCode(proc.CodeNum);
				if(ProcedureCodes.GetProcCode(proc.CodeNum).TreatArea==TreatmentArea.Surf) {
					procTP.Surf=Tooth.SurfTidyFromDbToDisplay(proc.Surf,proc.ToothNum);
				}
				else {
					procTP.Surf=proc.Surf;//for UR, L, etc.
				}
				listProcTPs.Add(procTP);
			}
			return listProcTPs;
		}

		public void RefreshView() {
			Image img=new Bitmap(_sheetField.Width,_sheetField.Height);
			Graphics g=Graphics.FromImage(img);
			g.SmoothingMode=SmoothingMode.HighQuality;
			SheetPrinting.DrawScaledImage(0,0,_sheetField.Width,_sheetField.Height,g,null,_imgToothChart);
			if(Image!=null) {
				Image.Dispose();
			}
			Image=img;
			g.Dispose();
		}
	}
}
