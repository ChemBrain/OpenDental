using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormApptBreakRequired:ODForm {

		protected override bool HasHelpKey {
			get {
				return false;
			}
		}

		public ProcedureCode SelectedBrokenProcCode { get; private set; }

		public FormApptBreakRequired() {
			InitializeComponent();
			Lan.F(this);;
		}

		private void butMissed_Click(object sender,EventArgs e) {
			SelectedBrokenProcCode=ProcedureCodes.GetProcCode("D9986");
			DialogResult=DialogResult.OK;
		}

		private void butCancelled_Click(object sender,EventArgs e) {
			SelectedBrokenProcCode=ProcedureCodes.GetProcCode("D9987");
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}