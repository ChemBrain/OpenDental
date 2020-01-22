using System.Collections.Generic;
using OpenDentBusiness;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormJobEdit:ODForm {

		public Job JobCur {
			get {
				if(userControlQueryEdit.Visible) {
					return userControlQueryEdit.GetJob();
				}
				else {
					return userControlJobEdit.GetJob();
				}
			}
		}

		public FormJobEdit(Job job,List<Job> listJobs) {
			InitializeComponent();
			Lan.F(this);
			Text="Job: "+job.ToString();
			if(job.Category==JobCategory.Query) {
				userControlQueryEdit.LoadJob(job,Jobs.GetJobTree(job,listJobs),listJobs);
				userControlJobEdit.Visible=false;
				userControlQueryEdit.Visible=true;
			}
			else {
				userControlJobEdit.LoadJob(job,Jobs.GetJobTree(job,listJobs),listJobs);
				userControlJobEdit.Visible=true;
				userControlQueryEdit.Visible=false;
			}
		}

		public override void OnProcessSignals(List<Signalod> listSignals) {
			if(!listSignals.Exists(x => x.IType==InvalidType.Jobs)) {
				return;//no job signals;
			}
			//Get the job nums from the signals passed in.
			List<long> listJobNums=listSignals.FindAll(x => x.IType==InvalidType.Jobs && x.FKeyType==KeyType.Job)
				.Select(x => x.FKey)
				.Distinct()
				.ToList();
			if(listJobNums.Contains(userControlJobEdit.GetJob().JobNum)) {
				Job jobMerge=Jobs.GetOneFilled(userControlJobEdit.GetJob().JobNum);
				userControlJobEdit.LoadMergeJob(jobMerge);
			}
		}

		private bool JobUnsavedChangesCheck() {
			if(userControlJobEdit.IsChanged) {
				switch(MessageBox.Show($"Save changes to Job: {userControlJobEdit.GetJob().ToString()}","",MessageBoxButtons.YesNoCancel)) {
					case System.Windows.Forms.DialogResult.OK:
					case System.Windows.Forms.DialogResult.Yes:
						if(!userControlJobEdit.ForceSave()) {
							return true;
						}
						break;
					case System.Windows.Forms.DialogResult.No:
						CheckinJob();
						break;
					case System.Windows.Forms.DialogResult.Cancel:
						return true;
				}
			}
			return false;//no unsaved changes
		}

		private void CheckinJob() {
			Job jobCur=userControlJobEdit.GetJob();
			if(jobCur==null) {
				return;
			}
			if(jobCur.UserNumCheckout==Security.CurUser.UserNum) {
				jobCur=Jobs.GetOne(jobCur.JobNum);
				jobCur.UserNumCheckout=0;
				Jobs.Update(jobCur);
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jobCur.JobNum);
			}
		}

		private void userControlJobEdit_SaveClick(object sender,System.EventArgs e) {
			Text="Job: "+userControlJobEdit.GetJob().ToString();
		}

		private void userControlQueryEdit_SaveClick(object sender,System.EventArgs e) {
			Text="Job: "+userControlQueryEdit.GetJob().ToString();
		}

		private void FormJobEdit_Activated(object sender,System.EventArgs e) {
			if(WindowState==FormWindowState.Minimized) {
				this.WindowState=FormWindowState.Normal;
			}
		}

		private void FormJobEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(JobUnsavedChangesCheck()) {
				e.Cancel=true;
				return;
			}
			FormJobManager.RemoveFormJobEdit(this);
		}
	}
}