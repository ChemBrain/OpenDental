using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDental {
	partial class FormShowFeatures {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormShowFeatures));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.checkCapitation = new System.Windows.Forms.CheckBox();
			this.checkMedicaid = new System.Windows.Forms.CheckBox();
			this.checkAdvancedIns = new System.Windows.Forms.CheckBox();
			this.checkClinical = new System.Windows.Forms.CheckBox();
			this.checkBasicModules = new System.Windows.Forms.CheckBox();
			this.checkPublicHealth = new System.Windows.Forms.CheckBox();
			this.checkEnableClinics = new System.Windows.Forms.CheckBox();
			this.checkDentalSchools = new System.Windows.Forms.CheckBox();
			this.checkRepeatCharges = new System.Windows.Forms.CheckBox();
			this.checkInsurance = new System.Windows.Forms.CheckBox();
			this.checkHospitals = new System.Windows.Forms.CheckBox();
			this.checkMedicalIns = new System.Windows.Forms.CheckBox();
			this.checkEhr = new System.Windows.Forms.CheckBox();
			this.checkSuperFam = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.checkPatClone = new System.Windows.Forms.CheckBox();
			this.checkQuestionnaire = new System.Windows.Forms.CheckBox();
			this.checkShowEnterprise = new System.Windows.Forms.CheckBox();
			this.checkShowReactivations = new System.Windows.Forms.CheckBox();
			this.checkEraShowControlId = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(380, 477);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(380, 447);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// checkCapitation
			// 
			this.checkCapitation.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCapitation.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkCapitation.Location = new System.Drawing.Point(12, 37);
			this.checkCapitation.Name = "checkCapitation";
			this.checkCapitation.Size = new System.Drawing.Size(258, 19);
			this.checkCapitation.TabIndex = 2;
			this.checkCapitation.Text = "Capitation";
			this.checkCapitation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkMedicaid
			// 
			this.checkMedicaid.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMedicaid.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkMedicaid.Location = new System.Drawing.Point(12, 61);
			this.checkMedicaid.Name = "checkMedicaid";
			this.checkMedicaid.Size = new System.Drawing.Size(258, 19);
			this.checkMedicaid.TabIndex = 3;
			this.checkMedicaid.Text = "Medicaid";
			this.checkMedicaid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAdvancedIns
			// 
			this.checkAdvancedIns.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAdvancedIns.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAdvancedIns.Location = new System.Drawing.Point(12, 427);
			this.checkAdvancedIns.Name = "checkAdvancedIns";
			this.checkAdvancedIns.Size = new System.Drawing.Size(258, 19);
			this.checkAdvancedIns.TabIndex = 4;
			this.checkAdvancedIns.Text = "Advanced Insurance Fields (deprecated)";
			this.checkAdvancedIns.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAdvancedIns.Visible = false;
			// 
			// checkClinical
			// 
			this.checkClinical.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClinical.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClinical.Location = new System.Drawing.Point(12, 181);
			this.checkClinical.Name = "checkClinical";
			this.checkClinical.Size = new System.Drawing.Size(258, 19);
			this.checkClinical.TabIndex = 5;
			this.checkClinical.Text = "Clinical (computers in operatories)";
			this.checkClinical.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkBasicModules
			// 
			this.checkBasicModules.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBasicModules.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBasicModules.Location = new System.Drawing.Point(12, 205);
			this.checkBasicModules.Name = "checkBasicModules";
			this.checkBasicModules.Size = new System.Drawing.Size(258, 19);
			this.checkBasicModules.TabIndex = 6;
			this.checkBasicModules.Text = "Basic Modules Only";
			this.checkBasicModules.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPublicHealth
			// 
			this.checkPublicHealth.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPublicHealth.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPublicHealth.Location = new System.Drawing.Point(12, 85);
			this.checkPublicHealth.Name = "checkPublicHealth";
			this.checkPublicHealth.Size = new System.Drawing.Size(258, 19);
			this.checkPublicHealth.TabIndex = 7;
			this.checkPublicHealth.Text = "Public Health";
			this.checkPublicHealth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkEnableClinics
			// 
			this.checkEnableClinics.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEnableClinics.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEnableClinics.Location = new System.Drawing.Point(12, 229);
			this.checkEnableClinics.Name = "checkEnableClinics";
			this.checkEnableClinics.Size = new System.Drawing.Size(258, 19);
			this.checkEnableClinics.TabIndex = 8;
			this.checkEnableClinics.Text = "Clinics (multiple office locations)";
			this.checkEnableClinics.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEnableClinics.Click += new System.EventHandler(this.checkEnableClinics_Click);
			// 
			// checkDentalSchools
			// 
			this.checkDentalSchools.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDentalSchools.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkDentalSchools.Location = new System.Drawing.Point(12, 109);
			this.checkDentalSchools.Name = "checkDentalSchools";
			this.checkDentalSchools.Size = new System.Drawing.Size(258, 19);
			this.checkDentalSchools.TabIndex = 9;
			this.checkDentalSchools.Text = "Dental Schools";
			this.checkDentalSchools.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDentalSchools.Click += new System.EventHandler(this.checkRestart_Click);
			// 
			// checkRepeatCharges
			// 
			this.checkRepeatCharges.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRepeatCharges.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRepeatCharges.Location = new System.Drawing.Point(12, 253);
			this.checkRepeatCharges.Name = "checkRepeatCharges";
			this.checkRepeatCharges.Size = new System.Drawing.Size(258, 19);
			this.checkRepeatCharges.TabIndex = 10;
			this.checkRepeatCharges.Text = "Repeating Charges";
			this.checkRepeatCharges.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkInsurance
			// 
			this.checkInsurance.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsurance.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkInsurance.Location = new System.Drawing.Point(12, 157);
			this.checkInsurance.Name = "checkInsurance";
			this.checkInsurance.Size = new System.Drawing.Size(258, 19);
			this.checkInsurance.TabIndex = 11;
			this.checkInsurance.Text = "All Insurance";
			this.checkInsurance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsurance.Click += new System.EventHandler(this.checkRestart_Click);
			// 
			// checkHospitals
			// 
			this.checkHospitals.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHospitals.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHospitals.Location = new System.Drawing.Point(12, 133);
			this.checkHospitals.Name = "checkHospitals";
			this.checkHospitals.Size = new System.Drawing.Size(258, 19);
			this.checkHospitals.TabIndex = 12;
			this.checkHospitals.Text = "Hospitals";
			this.checkHospitals.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkMedicalIns
			// 
			this.checkMedicalIns.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMedicalIns.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkMedicalIns.Location = new System.Drawing.Point(12, 278);
			this.checkMedicalIns.Name = "checkMedicalIns";
			this.checkMedicalIns.Size = new System.Drawing.Size(258, 19);
			this.checkMedicalIns.TabIndex = 13;
			this.checkMedicalIns.Text = "Medical Insurance";
			this.checkMedicalIns.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkEhr
			// 
			this.checkEhr.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEhr.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEhr.Location = new System.Drawing.Point(12, 303);
			this.checkEhr.Name = "checkEhr";
			this.checkEhr.Size = new System.Drawing.Size(258, 19);
			this.checkEhr.TabIndex = 14;
			this.checkEhr.Text = "EHR";
			this.checkEhr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEhr.Click += new System.EventHandler(this.checkEhr_Click);
			// 
			// checkSuperFam
			// 
			this.checkSuperFam.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSuperFam.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSuperFam.Location = new System.Drawing.Point(12, 328);
			this.checkSuperFam.Name = "checkSuperFam";
			this.checkSuperFam.Size = new System.Drawing.Size(258, 19);
			this.checkSuperFam.TabIndex = 15;
			this.checkSuperFam.Text = "Super Families";
			this.checkSuperFam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSuperFam.Click += new System.EventHandler(this.checkRestart_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(443, 18);
			this.label1.TabIndex = 16;
			this.label1.Text = "The following settings will affect all computers.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// checkPatClone
			// 
			this.checkPatClone.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPatClone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPatClone.Location = new System.Drawing.Point(12, 352);
			this.checkPatClone.Name = "checkPatClone";
			this.checkPatClone.Size = new System.Drawing.Size(258, 19);
			this.checkPatClone.TabIndex = 17;
			this.checkPatClone.Text = "Patient Clone";
			this.checkPatClone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPatClone.Click += new System.EventHandler(this.checkRestart_Click);
			// 
			// checkQuestionnaire
			// 
			this.checkQuestionnaire.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkQuestionnaire.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkQuestionnaire.Location = new System.Drawing.Point(12, 377);
			this.checkQuestionnaire.Name = "checkQuestionnaire";
			this.checkQuestionnaire.Size = new System.Drawing.Size(258, 19);
			this.checkQuestionnaire.TabIndex = 18;
			this.checkQuestionnaire.Text = "Questionnaire";
			this.checkQuestionnaire.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowEnterprise
			// 
			this.checkShowEnterprise.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowEnterprise.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowEnterprise.Location = new System.Drawing.Point(12, 402);
			this.checkShowEnterprise.Name = "checkShowEnterprise";
			this.checkShowEnterprise.Size = new System.Drawing.Size(258, 19);
			this.checkShowEnterprise.TabIndex = 20;
			this.checkShowEnterprise.Text = "Show Enterprise Setup";
			this.checkShowEnterprise.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowReactivations
			// 
			this.checkShowReactivations.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowReactivations.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowReactivations.Location = new System.Drawing.Point(12, 427);
			this.checkShowReactivations.Name = "checkShowReactivations";
			this.checkShowReactivations.Size = new System.Drawing.Size(258, 19);
			this.checkShowReactivations.TabIndex = 21;
			this.checkShowReactivations.Text = "Show Reactivations";
			this.checkShowReactivations.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowReactivations.Click += new System.EventHandler(this.checkRestart_Click);
			// 
			// checkEraShowControlId
			// 
			this.checkEraShowControlId.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEraShowControlId.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEraShowControlId.Location = new System.Drawing.Point(55, 452);
			this.checkEraShowControlId.Name = "checkEraShowControlId";
			this.checkEraShowControlId.Size = new System.Drawing.Size(215, 19);
			this.checkEraShowControlId.TabIndex = 23;
			this.checkEraShowControlId.Text = "ERA window shows ControlId filter";
			this.checkEraShowControlId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormShowFeatures
			// 
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(467, 507);
			this.Controls.Add(this.checkEraShowControlId);
			this.Controls.Add(this.checkShowReactivations);
			this.Controls.Add(this.checkShowEnterprise);
			this.Controls.Add(this.checkQuestionnaire);
			this.Controls.Add(this.checkPatClone);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkSuperFam);
			this.Controls.Add(this.checkEhr);
			this.Controls.Add(this.checkMedicalIns);
			this.Controls.Add(this.checkHospitals);
			this.Controls.Add(this.checkInsurance);
			this.Controls.Add(this.checkRepeatCharges);
			this.Controls.Add(this.checkDentalSchools);
			this.Controls.Add(this.checkEnableClinics);
			this.Controls.Add(this.checkPublicHealth);
			this.Controls.Add(this.checkBasicModules);
			this.Controls.Add(this.checkClinical);
			this.Controls.Add(this.checkAdvancedIns);
			this.Controls.Add(this.checkMedicaid);
			this.Controls.Add(this.checkCapitation);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormShowFeatures";
			this.ShowInTaskbar = false;
			this.Text = "Show Features";
			this.Load += new System.EventHandler(this.FormShowFeatures_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.CheckBox checkAdvancedIns;
		private System.Windows.Forms.CheckBox checkClinical;
		private System.Windows.Forms.CheckBox checkBasicModules;
		private System.Windows.Forms.CheckBox checkPublicHealth;
		private System.Windows.Forms.CheckBox checkEnableClinics;
		private System.Windows.Forms.CheckBox checkDentalSchools;
		private System.Windows.Forms.CheckBox checkRepeatCharges;
		private System.Windows.Forms.CheckBox checkInsurance;
		private System.Windows.Forms.CheckBox checkHospitals;
		private System.Windows.Forms.CheckBox checkMedicalIns;
		private System.Windows.Forms.CheckBox checkEhr;
		private System.Windows.Forms.CheckBox checkSuperFam;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox checkPatClone;
		private System.Windows.Forms.CheckBox checkQuestionnaire;
		///<summary>Keeps track the clinic preference state when the window was loaded.</summary>
		private System.Windows.Forms.CheckBox checkShowEnterprise;
		private System.Windows.Forms.CheckBox checkMedicaid;
		private System.Windows.Forms.CheckBox checkCapitation;
		private System.Windows.Forms.CheckBox checkShowReactivations;
		private System.Windows.Forms.CheckBox checkEraShowControlId;
	}
}
